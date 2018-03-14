using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for PaymentsPerInsSubType
/// </summary>
/// 
namespace Broker.DataAccess {
    public partial class PaymentsPerInsSubType : EntityBase<PaymentsPerInsSubType> {
        public static List<PaymentsPerInsSubType> GetByPayment(int payment)
        {
            return Table.Where(c => c.PaymentID == payment).ToList();
        }
    }
}
