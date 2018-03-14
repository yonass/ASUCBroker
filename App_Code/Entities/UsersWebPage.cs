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

namespace Broker.DataAccess {

    public partial class UsersWebPage :EntityBase<UsersWebPage>{

        /// <summary>
        /// Gi vrakja vidlivite web-strani za user-ot
        /// </summary>
        /// <param name="userId">int userID</param>
        /// <returns>Vrakja numerirana lista (IEnumerable)</returns>
        /// Se povikuva vo UserManagementControllers\WebpagesVisibilityController.cs
        public static IEnumerable<UsersWebPage> GetVisibleWebPagesByUser(int userId) {
            return Table.Where(v => v.UserID==userId);
        }
        /// <summary>
        /// Brisi zapisi od tabelata
        /// </summary>
        /// <param name="list">List(UsersWebPage) list</param>
        /// Se povikuva vo UserManagementControllers\WebpagesVisibilityController.cs
        public static void DeleteUserWebPages(List<UsersWebPage> list) {
            // TODO: Problemi so EntityBase::Delete() - da se razreshi
            // pagjashe pri Table.Attach(T); vo EntityBase
            // Za razlika od tuka kaj UsersFunctions.cs e OK !!

            DataClassesDataContext dc = new DataClassesDataContext();
            foreach(UsersWebPage uPage in list) {
                string query = @"DELETE FROM UsersWebPages 
                                 WHERE UserId="+uPage.UserID+@" 
                                 AND WebPageId="+uPage.WebPageID;
                dc.ExecuteCommand(query);
            }
        }

        /// <summary>
        /// Vnesuva zapisi vo tabelata
        /// </summary>
        /// <param name="list">list(UsersWebPage) list</param>
        /// Se povikuva vo UserManagementControllers\WebpagesVisibilityController.cs
        /// i EmployeeCreators\EmployeeController.cs
        public static void InsertUserWebPages(List<UsersWebPage> list) {
            foreach(UsersWebPage uPage in list) {
                uPage.Insert();
            }
        }
        /// <summary>
        /// Vrakja WebPage po ime na funkcija
        /// </summary>
        /// <param name="name">string name</param>
        /// <returns>Vrakja WebPage object</returns>
        /// Ne se povikuva 
        public WebPage GetWebPageByFunctionName(string name) {
            Function f= Function.GetFunctionByName(name);
            return WebPage.GetWebPageByFunction(f);
        }

        /// <summary>
        /// Vrakja dali postoi parot od User i WebPage
        /// </summary>
        /// <param name="userId">int userID</param>
        /// <param name="webPageId">int webPageID</param>
        /// <returns>Vrakja logicka promenliva (bool)</returns>
        /// Se povikuva vo TreeControllers\WebPage.cs i Athentication\AthenticationController.cs
        public static bool EntityExists(int userId, int webPageId) {
            UsersWebPage uPage = Table.Where(v => v.WebPageID==webPageId && v.UserID == userId).SingleOrDefault();
            return (uPage!=null)? true : false;
        }
        /// <summary>
        /// Vrakja UsersWebPage 
        /// </summary>
        /// <param name="userID">int userID</param>
        /// <param name="webPageID">int webPageID</param>
        /// <returns>Vrakja UsersWebPage object</returns>
        /// Se povikuva vo MKOSIG\TariffsWindshieldRates.aspx.cs,MKOSIG\TariffsANCost.aspx.cs,
        /// MKOSIG\TariffsANClass.aspx.cs,MKOSIG\ReziskiDodatoci.aspx.cs,MKOSIG\Partnership_report.aspx.cs,
        /// MKOSIG\CompanyExtraPayment.aspx.cs,MKOSIG\Company_Policies.aspx.cs,MKOSIG\Company_GreenCards.aspx.cs,
        /// MKOSIG\CascoManagement.aspx.cs,MKOSIG\Base_Fee_Cost.aspx.cs,MKBIRO\TechnicalDescription_report.aspx.cs,
        /// MKBIRO\TarifenSistem\Vehicle_Type.aspx.cs,MKBIRO\TarifenSistem\Vehicle_Sub_Types.aspx.cs,
        /// MKBIRO\TarifenSistem\Temp_Licence.aspx.cs,MKBIRO\TarifenSistem\Registrtion_Prefix.aspx.xs,
        /// MKBIRO\TarifenSistem\Premium_Groups.aspx.cs,MKBIRO\TarifenSistem\Insurance_Durations.aspx.cs,
        /// MKBIRO\TarifenSistem\Insurance_Coverages.aspx.cs,MKBIRO\TarifenSistem\GreenCard_Tariff.aspx.cs,
        /// MKBIRO\TarifenSistem\Extra_Payment.aspx.cs,MKBIRO\TarifenSistem\Bonus_degree.aspx.cs,
        /// MKBIRO\RabotnaOkolina\Municipalities_Report.aspx.cs,MKBIRO\ProdaznaMreza\Employees_ForMinistryOfFinance.aspx.cs,
        /// MKBIRO\ProdaznaMreza\Employees_Brokers_Report.aspx.cs,MKBIRO\ProdaznaMreza\Employees.aspx.cs,
        /// MKBIRO\ProdaznaMreza\Companies.aspx.cs, MKBIRO\ProdaznaMreza\BrokersHouses_ForMInistrOfFinance.aspx.cs,
        /// MKBIRO\ProdaznaMreza\BrokersHouses.aspx.cs,MKBIRO\ProdaznaMreza\Brokers_ForMinistryOfFinance.aspx.cs,
        /// MKBIRO\ProdaznaMreza\Brokers.aspx.cs,MKBIRO\ProdaznaMreza\Branches.aspx.cs,MKBIRO\Policies_Report.aspx.cs
        /// MKBIRO\GreenCardReport.aspx.cs,MKBIRO\DamageReport.aspx.cs,ReportControllers\SearchController.cs
        public static UsersWebPage GetByUserIDWebPageID(int userID, int webPageID) {
            return UsersWebPage.Select().Where(w => w.UserID == userID && w.WebPageID == webPageID).SingleOrDefault();
        }
    }
}