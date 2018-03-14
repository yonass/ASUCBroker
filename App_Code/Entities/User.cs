using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Broker.Controllers.EmployeeManagement;
using Broker.Validators;

/// <summary>
/// Summary description for User
/// </summary>
/// 
namespace Broker.DataAccess {
    public partial class User : EntityBase<User> {
        public static User GetByUserName(string username) {
            User user = Table.Where(v => v.UserName == username).SingleOrDefault();
            if (user != null) {
                return user;
            }
            return null;
        }

        public static User GetFirstSEAdminUser() {
            return Table.Where(c => c.Role.Name == Role.SEAdmin).ToList().First();
        }

        public static List<User> GetUsersByBranch(int branchID, int distributionDocTypeID) {
            DistributionDocType ddt = DistributionDocType.Get(distributionDocTypeID);
            if (ddt.Code == DistributionDocType.ISPRATNICA) {
                return Table.Where(c => c.BranchID == branchID && c.Role.Name != Role.Employee && c.Role.Name != Role.ExternalAgent && c.Role.Name != Role.SIMTAdmin && c.Role.Name != Role.SEAdmin && c.Role.IsVisible == true && c.IsActive == true).OrderByDescending(c => c.Role.Name).ToList();
            } else if (ddt.Code == DistributionDocType.PRIEM || ddt.Code == DistributionDocType.POVRATNICA) {
                return Table.Where(c => c.Role.Name == Role.SEAdmin).ToList();
            } else {
                return null;
            }
        }


        public static bool IsValidPassword(User u, string password) {
            return u.Password == password;
        }

        public static Role GetRoleByUserId(int userId) {
            User user = GetUserById(userId);
            if (user != null) {
                return user.Role;
            }
            return Role.GetRoleByName(RolesInfo.SIMTAdmin);
        }

        /// <summary>
        /// Vrakja user spored id
        /// </summary>
        /// <param name="userID">int userID</param>
        /// <returns>Vrakja User Object</returns>
        /// Se povikuva vo Change_Password.aspx.cs i vo Entitetot user 
        /// vo metodot GetRoleByUserID
        public static User GetUserById(int userID) {
            return Table.Where(u => u.ID == userID).SingleOrDefault();
        }

        /// <summary>
        /// Vrakja lista na USeri od opredelena rolja 
        /// </summary>
        /// <param name="roleId">int roleID</param>
        /// <returns>vrakja numerirana lista (IEnumerable)</returns>
        /// Se povikuva vo EmployeeCreators\EmployeeController.cs i vo
        /// UsersManagementControllers\RolesManagementController.cs
        public static IEnumerable<User> GetUsersInRole(int roleId) {
            return Table.Where(v => v.RoleID == roleId);
        }

        // TODO: Da se preprave u LINQ
        /// <summary>
        /// Vrakja lista so "deca" ma Userot
        /// </summary>
        /// <param name="userId">int userID</param>
        /// <returns>Vrakja numerirana lista (IEnumerable)</returns>
        /// se povikuva vo UsersManagementControllers\RolesManagementController.cs
        public static IEnumerable<User> GetChildUsers(int userId) {
            DataClassesDataContext dc = new DataClassesDataContext();
            string query = @"SELECT u2.ID, u2.RoleID, u2.Username
                             FROM  Users u1
                             INNER JOIN UsersParentships up ON u1.ID = up.ParentID
                             INNER JOIN Users u2 ON up.ChildId= u2.ID 
                             WHERE u1.ID= " + userId;
            return dc.ExecuteQuery<User>(query);
        }

        // TODO: Da se preprave u LINQ 
        /// <summary>
        /// Gi vrakja funkciite koi gi ima user-ot,a ne se vo roljata
        /// </summary>
        /// <param name="userId">int userID</param>
        /// <returns>Vrakja numerirana lista (IEnumerable)</returns>
        /// se povikuva vo UsersManagementControllers\RolesManagementController.cs
        public static IEnumerable<Function> GetCustomFunctions(int userId) {
            DataClassesDataContext dc = new DataClassesDataContext();
            string query = @"SELECT f.ID, f.Name, f.Description,f.CanBeGiven
                             FROM  Users u
                             INNER JOIN UsersFunctions uf ON u.ID = uf.UserID
                             INNER JOIN Functions f ON uf.FunctionId = f.ID 
                             WHERE u.ID= " + userId;
            return dc.ExecuteQuery<Function>(query);
        }

        public static User GetByEMBG(string embg) {
            return Table.Where(u => u.EMBG == embg).SingleOrDefault();
        }

        public void TestEMBG() {
            if (!ClientValidator.isValidPersonalEmbg(EMBG)) {
                ValidationErrors.Add("EMBG_Exist", this.EMBG + "Невалиден ЕМБГ");
            }
            if ((User.GetByEMBG(this.EMBG) != null) && (User.GetByEMBG(this.EMBG).IsActive == true)) {
                ValidationErrors.Add("EMBG_Exist", this.EMBG + " веќе постои во базата");
            }
            this.PerformCustomValidation();
        }

        /// <summary>
        /// Vnesuva nov zapis vo tabelata
        /// </summary>
        /// <param name="username">string UserName</param>
        /// <param name="roleId">int roleiD</param>
        /// <param name="certificate">string certificate</param>
        /// <param name="password">string password</param>
        /// Se povikuva vo EmployeeCreators\EmployeeController.cs
        public static void InsertUser(string username, int roleId, string address, string password,
            string phone, string EMBG, string fullName, int branchID) {
            User newUser = new User();
            newUser.UserName = username;
            newUser.RoleID = roleId;
            newUser.Password = password;
            newUser.Address = address;
            newUser.Phone = phone;
            newUser.Name = fullName;
            newUser.EMBG = EMBG;
            newUser.BranchID = branchID;
            newUser.IsActive = true;
            User.Insert(newUser);
        }

        /// <summary>
        /// Vrakja User spored username-ot
        /// </summary>
        /// <param name="username">string username</param>
        /// <returns>Vrakja User object</returns>
        /// Se povikuva vo EmployeeCreators\EmployeeController.cs i
        /// UserAuthenticationController\userInfo.cs, Broker entitetot, User entitetot vo 
        /// metodot UserNameExist, MKBIRO\ChangeAllPasswords.aspx.cs
        public static User GetUserByUsername(string username) {
            User user = Table.Where(v => v.UserName == username).SingleOrDefault();
            if (user != null) {
                return user;
            }
            return null;
        }

        /// <summary>
        /// Proverka dali postoi korisnickoto ime
        /// </summary>
        /// <param name="username">string username</param>
        /// <returns>Vrakja logicka promenliva (bool)</returns>
        /// Se povikuva vo User entitetot vo metodot Validate 
        public static bool UsernameExists(string username) {
            return (GetUserByUsername(username) != null) ? true : false;
        }
        /// <summary>
        /// Vrsi validacija pred da se insertira nov user
        /// </summary>
        public override void Validate() {
            if (this.UserName != string.Empty) {
                if (User.UsernameExists(this.UserName)) {
                    ValidationErrors.Add("USERNAME_EXISTS", this.UserName + " е веќе зафатено!");
                }
            }
        }

        public void Test() {
            this.PerformValidation();
        }

        /// <summary>
        /// Go menuva passwordot
        /// </summary>
        /// <param name="user">User user</param>
        /// <param name="password">string password</param>
        /// se povikuva vo MKBIRO\ChangeAllPassword.aspx.cs
        public void UpdatePassword(User user, string password) {
            user.Password = password;
            User.Table.Context.SubmitChanges();
        }
        /// <summary>
        /// Go menuva passwordot
        /// </summary>
        /// <param name="user">User user</param>
        /// <param name="password">string password</param>
        /// Se povikuva vo Change_password.aspx.cs
        public void UpdatePasswordExtend(User user, string password) {
            User _u = User.Get(user.ID);
            _u.Password = password;
            _u.Update2();
        }

        /// <summary>
        /// Vrakja lista na logirani korisnici
        /// </summary>
        /// <returns>Vrakja lista (List)</returns>
        /// Ne se povikuva vo 
        public static List<User> GetOnlineUsers() {
            HttpContext.Current.Application.Lock();
            HashSet<int> usersSet = (HashSet<int>)HttpContext.Current.Application["usersSet"];
            List<User> Online = new List<User>();
            foreach (var userID in usersSet) {
                Online.Add(Table.Where(n => n.ID == userID).SingleOrDefault());
            }
            HttpContext.Current.Application.UnLock();
            return Online;
        }
        /// <summary>
        /// Vrakja lista na administratori na stroga evidencija na kompanija
        /// </summary>
        /// <param name="companyID">int CompanyID</param>
        /// <returns>Vrakja lista (List)</returns>
        /// SE povikuva vo DiStributions entitetot
        public static List<User> GetSeUsersInCompany(int companyID) {
            //int roleID = Role.GetRoleByName(RolesInfo.MK_SE_COMPANY).ID;
            //List<User> users = Table.Where(n => n.RoleID == roleID).ToList();
            List<User> SeUsers = new List<User>();
            //foreach (User user in users) {
            //    Employee emp = Employee.GetByUserId(user.ID);
            //    if (emp.Branch.CompanyID == companyID) {
            //        SeUsers.Add(user);
            //    }
            //}
            return SeUsers;
        }
        /// <summary>
        /// Vrakja lista na administratori na stroga evidencija na kompanija
        /// </summary>
        /// <param name="companyID">int CompanyID</param>
        /// <returns>Vrakja lista (List)</returns>
        /// SE povikuva vo DiStributions entitetot
        public static List<User> GetSeUsersInBranch(int branchID) {
            //int roleID = Role.GetRoleByName(RolesInfo.MK_SE_BRANCH).ID;
            //List<User> users = Table.Where(n => n.RoleID == roleID).ToList();
            List<User> SeUsers = new List<User>();
            //foreach (User user in users) {
            //    Employee emp = Employee.GetByUserId(user.ID);
            //    if (emp.Branch.ID == branchID) {
            //        SeUsers.Add(user);
            //    }
            //}
            return SeUsers;
        }

        /// <summary>
        /// Vrakja lista od site Useri
        /// </summary>
        /// <returns>Vrakja lista (List)</returns>
        /// Se povikuva vo EmployeeCreators\EmployeeController.cs
        public static List<User> GetAllUsers() {
            return Table.ToList();
        }

        public static List<User> GetAllActiveUsers() {
            return Table.Where(u => u.IsActive == true).ToList();
        }


        public static List<User> GetUsersForOrder(int userID) {
            List<User> listUsers = new List<User>();
            List<User> otherUsers = new List<User>();
            User currentUser = User.Get(userID);
            listUsers.Add(currentUser);

            if (currentUser.Role.Name == RolesInfo.Broker) {
                otherUsers = User.Table.Where(u => u.Role.Name == RolesInfo.Broker && u.IsActive == true).ToList();
                otherUsers.Remove(currentUser);
                listUsers.AddRange(otherUsers);
            }
            if (currentUser.Role.Name == RolesInfo.BROKERAdmin) {
                otherUsers.AddRange(User.Table.Where(u => u.Role.Name == RolesInfo.Broker && u.IsActive == true).ToList());
                otherUsers.AddRange(User.Table.Where(u => u.Role.Name == RolesInfo.BROKERAdmin && u.IsActive == true).ToList());
                otherUsers.AddRange(User.Table.Where(u => u.Role.Name == RolesInfo.SEAdmin && u.IsActive == true).ToList());
                otherUsers.Remove(currentUser);
                listUsers.AddRange(otherUsers);
            }
            return listUsers;
        }

        public static List<User> GetNOTSimtUsers() {
            List<User> lUser = Table.ToList();
            List<User> simtUsers = Table.Where(u => u.UserName.Contains("SIMT")).ToList();
            lUser.RemoveAll((u => u.UserName.Contains("SIMT")));
            return lUser;
        }

        public static List<User> GetBrokersAndBrokerAdmins() {
            List<User> lUser = Table.Where(c => c.Role.Name == Role.Broker || c.Role.Name == Role.BROKERAdmin).ToList();
            return lUser;
        }

        public static List<User> GetManageUsers() {
            List<User> lUser = Table.Where(c => c.Role.Name == Role.Broker || c.Role.Name == Role.BROKERAdmin || c.Role.Name == Role.SEAdmin || c.Role.Name == Role.SEBranch).ToList();
            return lUser;
        }


        public static List<User> getRatesApprovers(User u) {
            Function f = Function.GetFunctionByName(Function.RATES_APPROVER);
            List<UsersFunction> listUsersFunctions = UsersFunction.getUsersWithFunction(f.ID);
            List<User> users = new List<User>();
            if (CanApproveRates(u.ID)) {
                users.Add(u);
            } else {
                foreach (UsersFunction uf in listUsersFunctions) {
                    users.Add(User.Get(uf.UserID));
                }
            }
            return users;
        }

        public static List<User> GetRatesApprovers(int userID) {
            User u = User.Get(userID);
            Function f = Function.GetFunctionByName(Function.RATES_APPROVER);
            List<UsersFunction> listUsersFunctions = UsersFunction.getUsersWithFunction(f.ID);
            List<User> users = new List<User>();
            if (CanApproveRates(u.ID)) {
                users.Add(u);
            } else {
                foreach (UsersFunction uf in listUsersFunctions) {
                    users.Add(User.Get(uf.UserID));
                }
            }
            return users;
        }

        public static bool CanApproveRates(int userID) {
            Function f = Function.GetFunctionByName(Function.RATES_APPROVER);
            UsersFunction uf = UsersFunction.GetByFunctionAndUser(f.ID, userID);
            if (uf != null) {
                return true;
            }
            return false;
        }

        public static List<User> GetMarketingAgentsWithBrokerAgeForCompanyAndSubType(int companyID, int insuranceSubTypeID) {
            int roleID = Role.GetRoleByName(RolesInfo.MarketingAgent).ID;
            List<User> marketingAgents = Table.Where(u => u.RoleID == roleID && u.IsActive).ToList();
            User dummyUser = new User();
            dummyUser.Name = string.Empty;
            dummyUser.ID = 0;
            dummyUser.IsActive = true;
            List<User> users = new List<User>();
            users.Add(dummyUser);
            foreach (User agent in marketingAgents) {
                BrokeragesForMarketingAgent bfma = BrokeragesForMarketingAgent.GetByUserAndInsuranceCompanyForSubType(agent.ID, companyID, insuranceSubTypeID);
                if (bfma != null) {
                    users.Add(agent);
                }
            }
            return users;
        }
    }
}