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

using Broker.DataAccess;

namespace Broker.Controllers.EmployeeManagement {

    public class EmployRoleController {

        /// <summary>
        /// Gi vraka site mozni ulogi vneseni tabelata Roles vo bazata na podatoci
        /// </summary>
        /// <returns>numerirana lista (IEnumerable)</returns>
        public static IEnumerable<Broker.DataAccess.Role> GetAllRoles()
        {
            return Broker.DataAccess.Role.GetRoles();
        }


        public static IEnumerable<Broker.DataAccess.Role> GetAllVisibleRoles()
        {
            return Broker.DataAccess.Role.GetRoles().Where(r => r.IsVisible == true);
        }

       
        /// <summary>
        /// Vraka lista so informacii za korisnicite koi se vraboteni
        /// vo kompanija so opredelena uloga
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="roleId"></param>
        /// <returns>numerirana lista (IEnumerable)</returns>
        /// se povikuva vo metodot GetBranch_SE_Administrators
        public static IEnumerable<UserInfo> GetBranchUsersInRole(int companyId, int roleId)
        {
            IEnumerable<Broker.DataAccess.User> users = Broker.DataAccess.Branch.GetBranchUsersInRole(roleId, companyId);
            List<UserInfo> result = new List<UserInfo>();
            foreach (Broker.DataAccess.User user in users)
            {
                result.Add(new UserInfo(user));
            }
            return result;
        }

        /// <summary>
        /// Vraka lista so informacii za korisnicite koi imaat uloga
        /// administratori na stroga evidencija na nivo na filijala
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns>numerirana lista (IEnumerable)</returns>
        public static IEnumerable<UserInfo> GetSEAdministrators(int branchID) {
            Broker.DataAccess.Role role = Broker.DataAccess.Role.GetRoleByName(RolesInfo.SEAdmin);
            return Broker.DataAccess.Branch.GetBranchUserInRole(branchID, role.ID);
        }
    }
}

