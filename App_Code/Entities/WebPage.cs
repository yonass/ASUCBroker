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
using System.Reflection;

namespace Broker.DataAccess {

    public partial class WebPage : EntityBase<WebPage> {
        public static string BROKER_DEFAULT = "~/MKPROD/Default.aspx";
        public static string POLICY_PAGE = "~/MKPROD/Policies.aspx";
        public static string GREENCARD_PAGE = "~/MKPROD/NewGreenCard.aspx";
        public static string SUPER_ROOT = "~/MKBIRO/";
        
        /// <summary>
        /// Vrakja dali postoi strana so toa URL
        /// </summary>
        /// <param name="pageUrl">string pageUrl</param>
        /// <returns>Vrakja logicka promenliva (bool)</returns>
        /// Ne se povikuva
        public static bool UrlExists(string pageUrl) {
            if(GetWebPageByUrl(pageUrl)!=null) {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Ja vrakja stranata spored URL
        /// </summary>
        /// <param name="url">string Url</param>
        /// <returns>Vrakja object webpage</returns>
        /// Se povikuva vo Authentication\AuthenticationController.cs,
        /// Authentication\AuthenticationPage.cs i WebPage entitetot vo 
        /// metodot UrlExists
        public static WebPage GetWebPageByUrl(string url) {
            return Table.Where(v => v.RelativeUrl == url).SingleOrDefault();
        }
        /// <summary>
        /// Gi vrakja site zapisi od tabelata vo lista
        /// </summary>
        /// <returns>Vrakja numerirana lista (IEnumerable)/// </returns>
        /// Se povikuva UserManagementControllers\WebPageController.cs
        public static IEnumerable<WebPage> GetSavedWebPages() {
            return Table;
        }
        /// <summary>
        /// Vrakja web strana vo zavisnost od funkcijata
        /// </summary>
        /// <param name="f">Function f</param>
        /// <returns>Vrakja object WebPage</returns>
        /// Se povikuva EmpolyeeCreators\EmployeeController.cs i entitite
        /// UsersWebPage.cs i WebPage vo metodot GetWebPageByFunctionName
        public static WebPage GetWebPageByFunction(Function f) {
            return Table.Where(v => v.FunctionID == f.ID).SingleOrDefault();
        }
        /// <summary>
        /// vrakja web strana spored imeto na funkcijata
        /// </summary>
        /// <param name="name">string name</param>
        /// <returns>Vrakja object WebPage</returns>
        /// Se povikuva vo UsersManagementControllers\WebPagesVisibilityController.cs
        public static WebPage GetWebPageByFunctionName(string name) {
            Function f= Function.GetFunctionByName(name);
            return GetWebPageByFunction(f);
        }


        /// <summary>
        /// Vrakja funkcija spored web stranata
        /// </summary>
        /// <param name="pageId">int WebPageID</param>
        /// <returns>Vrakja Function object</returns>
        /// Se povikuva vo UsersManagementControllers\WebPagesVisibilityController.cs
        public static Function GetFunctionByWebPage(int pageId) {
            WebPage page = Table.Where(v => v.ID==pageId).SingleOrDefault();
            if(page!=null) {
                return page.Function;
            }
            return null;
        }

    }
}