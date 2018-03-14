using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Security.Cryptography;
using Broker.DataAccess;
using System.IO;

namespace Broker.Controllers.EmployeeManagement {
   
    public class EmployeeController {

        /// <summary>
        /// Validacija dali veke postoi korisnik so dadenoto korisnikcko ime
        /// za da nema duplikat na zapisi
        /// </summary>
        /// <param name="username">string username</param>
        /// se povikuva pri kreiranje na nov korisnik 
        /// UserManagement\NewUser.aspx.cs
        public static void ValidateUserInfo(string username) {
            Broker.DataAccess.User user = new Broker.DataAccess.User();
            user.UserName = username;
            user.Test();
        }

    
        public static string InsertBroker(int roleId, string username, string fullname, int branchId, string password, string address, string EMBG, string phone, string Email) {
            string cert = GenerateCertificate(username);
            Broker.DataAccess.User.InsertUser(username, roleId, address, password, phone, EMBG, fullname, branchId);
            Broker.DataAccess.User user = Broker.DataAccess.User.GetByUserName(username);
            ///Se postavuvaat preddefiniranite vidlivi web strani za dadenata uloga za novokreiraniot korisnik
            SetVisibleWebPages(user.ID, roleId);
            ///Se setiraat samo roditelite vo drvoto na hierarhija za novokreiraniot korisnik
            ///bidejki ovaa uloga e posledna vo hierarhijata
            Set_Broker_Parents(user.ID, branchId);
            return cert;
        }

        public static string InsertSEAdmin(int roleId, string username, string fullname, int branchId, string password, string address, string EMBG, string phone, string Email) {
            string cert = GenerateCertificate(username);
            Broker.DataAccess.User.InsertUser(username, roleId, address, password, phone, EMBG, fullname, branchId);
            Broker.DataAccess.User user = Broker.DataAccess.User.GetByUserName(username);
            ///Se postavuvaat preddefiniranite vidlivi web strani za dadenata uloga za novokreiraniot korisnik
            SetVisibleWebPages(user.ID, roleId);
            SetSuperAdminParents(user.ID);
            Set_SEAdmin_Childs(user.ID, branchId);
            return cert;
        }

        private static void Set_SEAdmin_Childs(int userID, int branchId) {
           
        }

        public static string InsertBROKERAdmin(int roleId, string username, string fullname, int branchId, string password, string address, string EMBG, string phone, string Email) {
            string cert = GenerateCertificate(username);
            Broker.DataAccess.User.InsertUser(username, roleId, address, password, phone, EMBG, fullname, branchId);
            Broker.DataAccess.User user = Broker.DataAccess.User.GetByUserName(username);
            SetVisibleWebPages(user.ID, roleId);
            SetSuperAdminParents(user.ID);
            Set_BROKERAdminChilds(user.ID, branchId);
            return cert;
        }

        private static void Set_BROKERAdminChilds(int userID, int branchId) {
            List<Broker.DataAccess.User> UsersInBranchWithRoleBroker = Broker.DataAccess.User.Table.Where(u => u.BranchID == branchId && u.Role.Name == RolesInfo.Broker).ToList();
            UsersParentship.SetChildren(userID, UsersInBranchWithRoleBroker);
        }


        public static void SetVisibleWebPages(int userID, int roleID) {
            List<Broker.DataAccess.Function> functions = Broker.DataAccess.Role.GetFucntionsByRole(roleID).ToList();
            List<UsersWebPage> userPages= new List<UsersWebPage>();
            foreach (Broker.DataAccess.Function f in functions)
            {
                UsersWebPage uPage= new UsersWebPage();
                WebPage webPage = WebPage.GetWebPageByFunction(f);
                uPage.UserID = userID;
                uPage.WebPageID = webPage.ID;
                userPages.Add(uPage);
            }
            UsersWebPage.InsertUserWebPages(userPages);
        }

       
        public static void Set_Broker_Parents(int userId, int branchId) {
            Broker.DataAccess.Role role = Broker.DataAccess.Role.GetRoleByName(RolesInfo.BROKERAdmin);
            SetBranchParentsInRole(userId, branchId, role);
            Broker.DataAccess.Role role1 = Broker.DataAccess.Role.GetRoleByName(RolesInfo.SEAdmin);
            SetBranchParentsInRole(userId, branchId, role1);
            SetSuperAdminParents(userId);
        }

        public static void ValidateUserEMBG(string EMBG) {
            Broker.DataAccess.User user = new Broker.DataAccess.User();
            user.EMBG = EMBG;
            user.TestEMBG();
        }

        public static int InsertMarketingAgent(string EMBG, string Name, int BranchID, string Address,string PhoneNumber, int RoleID) {

            Broker.DataAccess.User u = new Broker.DataAccess.User();
            u.Address = Address;
            u.BranchID = BranchID;
            u.EMBG = EMBG;
            u.IsActive = true;
            u.Name = Name;
            u.Password = string.Empty;
            u.Phone = PhoneNumber;
            u.RoleID = RoleID;
            u.UserName = string.Empty;
            u.Insert();
            Set_Broker_Parents(u.ID, BranchID);
            return u.ID;
        }


        public static void SetBranchParentsInRole(int userId, int branchId, Broker.DataAccess.Role role)
        {
            List<Broker.DataAccess.User> parents = Broker.DataAccess.Branch.GetBranchUsersInRole(role.ID, branchId).ToList();
            UsersParentship.SetParents(userId, parents);
        }

        public static void SetSuperAdminParents(int userId) {
            Broker.DataAccess.Role role = Broker.DataAccess.Role.GetRoleByName(RolesInfo.SIMTAdmin);
            List<Broker.DataAccess.User> parents = Broker.DataAccess.User.GetUsersInRole(role.ID).ToList();
            //parents.AddRange(parents.AsEnumerable());
            UsersParentship.SetParents(userId, parents);
        }

        public static string GenerateCertificate(string username) {
            SymmCrypto crypt = new SymmCrypto(SymmCrypto.SymmProvEnum.Rijndael);
            string data = crypt.base64Encode(username); ;
            string key = username;
            
            string secret = crypt.Encrypting(data, key); ;
            return  secret;
        }

        
        public static bool IsValidCertificate(string username, string certificate, string encrypted){
            return false;
        }

        public static void SendCertificate(string cert) {
            MemoryStream memStream = new MemoryStream();
            TextWriter writer = new StreamWriter(memStream);
            writer.WriteLine(cert);
            writer.Flush();
            writer.Close();
            
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.ClearHeaders();
            //HttpContext.Current.Response.ContentType = "application/pdf";
            HttpContext.Current.Response.Charset = string.Empty;
            HttpContext.Current.Response.Cache.SetCacheability(System.Web.HttpCacheability.Public);
            HttpContext.Current.Response.AddHeader("Content-Disposition:", "attachment; filename=simt.bin");
            HttpContext.Current.Response.OutputStream.Write(memStream.GetBuffer(), 0, cert.Length);
            HttpContext.Current.Response.OutputStream.Flush();
            HttpContext.Current.Response.OutputStream.Close();
            HttpContext.Current.Response.End();
        }

        public static List<Broker.DataAccess.User> GetAllUsers()
        {
            return Broker.DataAccess.User.GetAllUsers();
        }
    }



    public class CertificatePair {
        public string Secret { get; set; }
        public string Secret64Encoded { get; set; }

        public CertificatePair(string secret, string base64Encoded) {
            this.Secret = secret;
            this.Secret64Encoded = base64Encoded;
        }
    }
}
