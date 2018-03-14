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
/// Summary description for PolicyNumberValidator
/// </summary>
namespace Broker.Validators {

    public class PolicyNumberValidator {

        public static bool IsValid(string number) {
            return number.Length > 0;
        }

        public static bool IsDistributed(string number, int InsuranceSubTypeID,int InsuranceCompanyID) {

            Distribution d = Distribution.GetByNumberAndSubTypeAndCompany(InsuranceSubTypeID, number,InsuranceCompanyID);
            
            if (d!=null && !d.IsUsed) {
                return true;
            }
            return false;
            

        }

    }
}
