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

/// <summary>
/// Summary description for Validator
/// </summary>
namespace Broker.DataAccess {
    public partial class Validator : EntityBase<Validator> {
        public static Validator GetByName (string name){
            return Table.Where(v => v.Name == name).SingleOrDefault();
        }
    }
}