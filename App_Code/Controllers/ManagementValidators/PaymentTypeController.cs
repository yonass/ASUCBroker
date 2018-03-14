using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Broker.DataAccess;

/// <summary>
/// Summary description for PaymentTypeController
/// </summary>
/// 
namespace Broker.Controllers.ManagementControllers {
    public class PaymentTypeController {
        public static void ValidateInsertCode(string code) {
            Broker.DataAccess.PaymentType paymentType = new Broker.DataAccess.PaymentType();
            paymentType.Code = code;
            paymentType.TestBeforeInsert();
        }

        public static void ValidateUpdateCode(int codeID, string code) {
            Broker.DataAccess.PaymentType paymentType = new Broker.DataAccess.PaymentType();
            paymentType.Code = code;
            paymentType.ID = codeID;
            paymentType.TestBeforeUpdate();
        }
    }
}
