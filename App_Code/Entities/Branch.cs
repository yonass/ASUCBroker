using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Broker.Controllers.EmployeeManagement;

/// <summary>
/// Summary description for Branch
/// </summary>
/// 
namespace Broker.DataAccess {
    public partial class Branch : EntityBase<Branch> {
        public static IEnumerable<User> GetBranchUsersInRole(int roleId, int branchID) {
            List<User> branchUsers = User.Table.Where(u => u.BranchID == branchID).ToList();
            List<User> result = new List<User>();
            foreach (User u in branchUsers) {
                if (u.RoleID == roleId) {
                    result.Add(u);
                }
            }
            return result;
        }


        public static IEnumerable<UserInfo> GetBranchUserInRole(int branchID, int roleId) {
            IEnumerable<User> users = GetBranchUsersInRole(roleId, branchID);
            List<UserInfo> result = new List<UserInfo>();
            foreach (User user in users) {
                result.Add(new UserInfo(user));
            }
            return result;
        }

        public static List<Branch> GetActiveBranches() {
            return Table.Where(b => b.IsActive == true).ToList();
        }

        public static bool ExistCodeInBranches(string code) {
            return (Table.Where(b => b.Code == code && b.IsActive == true).SingleOrDefault() != null);
        }

        public static bool ExistOtherCode(int codeID, string code) {
            return (Table.Where(b => b.Code == code && b.ID != codeID && b.IsActive == true).SingleOrDefault() != null);
        }


        public override void Validate() {

        }

        public void TestBeforeInsert() {
            if (Branch.ExistCodeInBranches(this.Code)) {
                ValidationErrors.Add("BRANCH_CODE_INSERT_EXISTS", this.Code + " е веќе зафатено!");
            }
            this.PerformCustomValidation();
        }

        public void TestBeforeUpdate() {
            if (Branch.ExistOtherCode(this.ID, this.Code)) {
                ValidationErrors.Add("BRANCH_CODE_UPDATE_EXISTS", this.Code + " е веќе зафатено!");
            }
            this.PerformCustomValidation();
        }

    }
}
