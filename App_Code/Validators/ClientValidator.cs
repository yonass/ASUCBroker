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


/// <summary>
/// Summary description for ClientValidator
/// </summary>
namespace Broker.Validators {
    public class ClientValidator {

        public static int LAW_EMBG_LENGTH = 7;
        public static int PRIVATE_EMBG_LENGTH = 13;
        public static int FOREIGN_EMBG_LENGTH = 10;

        public static bool IsValidLawEmbg(string embg) {
            return embg.Trim().Length == LAW_EMBG_LENGTH;
        }

        public static bool isValidPersonalEmbg(string s) {
            if (s.Trim().Length == FOREIGN_EMBG_LENGTH) {
                return true;
            }
            if (s.Trim().Length != PRIVATE_EMBG_LENGTH)
                return false;
            else {
                int sum = 0;
                if (!validForDate(s))
                    return false;
                for (int i = 0; i < (s.Length) / 2; i++) {
                    sum += (s[i] - '0') * (7 - i);
                    sum += (s[i + 6] - '0') * (7 - i);
                }
                if ((sum % 11) == 0 && s[12] == '0') {
                    return true;
                }
                if ((11 - (sum % 11)) == (s[12] - '0'))
                    return true;
                else return false;
            }
        }
        private static bool validForDate(string embg) {
            string year;
            string month;
            string day;
            if (embg[4] == '0') {
                year = "2" + embg.Substring(4, 3);
            } else {
                if (embg[4] == '9') {
                    year = "1" + embg.Substring(4, 3);
                } else {
                    return false;
                }
            }
            day = embg.Substring(0, 2);
            month = embg.Substring(2, 2);
            DateTime dummy = new DateTime();
            string date = day + "." + month + "." + year;
            //string date = month + "." + day + "." + year;
            return DateTime.TryParse(date, out dummy);
        }
       

    }
}
