using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ControlBindableValue
/// </summary>
namespace Broker.DataAccess {
    public partial class ControlBindableValue : EntityBase<ControlBindableValue> {


        public static List<ControlBindableValue> GetActiveByControlID(int controlID) {

            List<ControlBindableValue> cList = Table.Where(c => c.ControlID == controlID && c.IsActive == true).ToList();
            return cList;
        }

    }
}
