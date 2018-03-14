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
using System.Collections;
using System.Reflection;
/// <summary>
/// Summary description for Utility
/// </summary>
namespace Broker.Utility {
    public static class Utility {
        public static void FillObject(Object o, IDictionary i) {
            Type t = o.GetType();
            PropertyInfo[] p = t.GetProperties();
            foreach (PropertyInfo pi in p) {
                if (i.Contains(pi.Name)) {
                    pi.SetValue(o, i[pi.Name], null);
                    
                }
            }
        }
        public static void FillObjectExtend(Object o, IDictionary i) {
            Type t = o.GetType();
            PropertyInfo[] p = t.GetProperties();
            foreach (PropertyInfo pi in p) {
                if (i.Contains(pi.Name)) {
                    pi.SetValue(o, decimal.Parse(i[pi.Name].ToString()), null);

                }
            }
        }
    }
}
