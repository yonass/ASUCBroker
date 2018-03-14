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
using System.IO;
using Broker.DataAccess;

namespace Broker.Controllers.UserManagement {
    /// <summary>
    /// Klasa za mendziranje na web-stranite
    /// </summary>
    public class WebPagesController {

        public static  string ASPX_EXTENSION = ".aspx";

        /// <summary>
        /// Metod koj go vraka root-ot na web-aplikacijata
        /// </summary>
        /// <returns>string</returns>
        public static string GetRoot() {
            HttpRequest req= HttpContext.Current.Request;
            HttpResponse response= HttpContext.Current.Response;
            string path= String.Format("http://{0}{1}", req.ServerVariables["HTTP_HOST"], req.ApplicationPath);
            return path+"/";
        }

        /// <summary>
        /// Metod koj gi vraka site web-strani na aplikacijata za
        /// specificiran root i nivna organizacija vo direktoriumi
        /// </summary>
        /// <param name="root"></param>
        /// <param name="dirInfo"></param>
        /// <returns>lista - List(string)</returns>
        public static List<string> GetAllWebPages(string root, DirectoryInfo dirInfo) {
            List<string> result=new List<string>();
            foreach(FileInfo file in dirInfo.GetFiles()) {
                if(file.Name.EndsWith(ASPX_EXTENSION)) {
                    result.Add(root+file.Name);
                }
            }
            foreach(DirectoryInfo dir in dirInfo.GetDirectories()) {
                result.AddRange(GetAllWebPages(root+dir.Name+"/", dir));
            }
            return result;
        }

        /// <summary>
        /// Metod koj gi vraka site web-strani od tabelata WebPages 
        /// vo bazata na podatoci vo HashSet
        /// </summary>
        /// <returns>mnozestvo od string-ovi - HashSet(string)</returns>
        public static HashSet<string> GetSavedWebPages() {
            HashSet<string> set=new HashSet<string>();
            foreach(WebPage page in WebPage.GetSavedWebPages()) {
                set.Add(page.RelativeUrl);
            }
            return set;
        }

        /// <summary>
        /// Metod koj gi vraka site funkcii za koi se povrzani (postojat)
        /// web-strani kako HashSet
        /// </summary>
        /// <returns>mnozestvo od integer-i - HashSet(int)</returns>
        public static HashSet<int> GetSavedFunctions() {
            HashSet<int> set= new HashSet<int>();
            foreach(WebPage page in WebPage.GetSavedWebPages()) {
                if(page.Function != null) {
                    int funcID = (int)page.FunctionID;
                    set.Add(funcID);
                }
            }
            return set;
        }

        /// <summary>
        /// Metod koj gi vraka site web-strani na aplikacijata 
        /// </summary>
        /// <returns>lista - List(WebPageInfo)</returns>
        public static List<WebPageInfo> GetAllWebPages() {
            DirectoryInfo myDirInfo = new DirectoryInfo(HttpContext.Current.Server.MapPath("../"));
            List<string> list =  GetAllWebPages(myDirInfo.Name+"/", myDirInfo);
            HashSet<string> savedPages = GetSavedWebPages();

            List<WebPageInfo> result = new List<WebPageInfo>();
            int c=0;
            foreach(string page in list) {
                if(!savedPages.Contains(page)) {
                    result.Add(new WebPageInfo(page, c++));
                }
            }
            return result;
        }

        /// <summary>
        /// Vraka lista od site funkcii na nacin sto na zapamtenite funkcii
        /// od HashSet-ot (od funkcijata GetSavedFunctions) 
        /// gi dodava ostanatite funkcii za koi ne se vrzani
        /// web-strani
        /// </summary>
        /// <returns>numerirana lista - IEnumerable(Function)</returns>
        public static IEnumerable<Broker.DataAccess.Function> GetAllFunctions()
        {
            HashSet<int> savedFunctions = GetSavedFunctions();
            List<Broker.DataAccess.Function> result = new List<Broker.DataAccess.Function>();
            foreach (Broker.DataAccess.Function f in Broker.DataAccess.Function.GetFunctions())
            {
                if(!savedFunctions.Contains(f.ID)) {
                    result.Add(f);
                }
            }
            return result;
        }

        /// <summary>
        /// Metod koj vrsi dodavanje na zapis vo tabelata WebPages
        /// vo bazata na podatoci so specificirana funkcija i url kon stranata
        /// </summary>
        /// <param name="functionId"></param>
        /// <param name="webPage"></param>
        public static void SaveFunctionWebPage(int functionId, string webPage) {
            WebPage page = new WebPage();
            page.FunctionID = functionId;
            page.RelativeUrl = webPage;
            IEnumerable<WebPage> webPageIenum = WebPage.Select().OrderBy(d => d.OrderNumber);
            page.OrderNumber = webPageIenum.Last().OrderNumber + 1;
            WebPage.Insert(page);
        }
    }
}