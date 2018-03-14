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
using Broker.DataAccess;

namespace Broker.Authentication {
    /// <summary>
    /// Klasa - kontroler za dozvolenite privilegii za pregled na
    /// web-strani za daden korisnik
    /// </summary>
    public class AuthenticationController {
        public static string ERROR_PAGE = "~/ErrorPage.aspx";
        public static string ERROR_MESSAGE = "Немате привилегии за преглед на Web страната";
        public static string SESSION_ERROR_MESSAGE = "Поради подолга неактивност (15 мин.) вашата тековна сесија е истечена. Најавете се повторно!";

        /// <summary>
        /// Metod za dozvolenite privilegii za pregled na specificirano url na
        /// web-strana za specificiraniot korisnik
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="relativeUrl"></param>
        /// <returns>bool</returns>
        public static bool IsUserPrivileged(int userId, string relativeUrl) {
            WebPage page = WebPage.GetWebPageByUrl(relativeUrl);
          
            return UsersWebPage.EntityExists(userId, page.ID);
        }
    }
}