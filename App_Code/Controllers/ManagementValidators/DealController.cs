using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Broker.DataAccess;

/// <summary>
/// Summary description for DealController
/// </summary>
/// 
namespace Broker.Controllers.ManagementControllers {
    public class DealController {
        public static void ValidateInsertCode(string code) {
            Deal deal = new Deal();
            deal.Code = code;
            deal.TestBeforeInsert();
        }

        public static void ValidateUpdateCode(int codeID, string code) {
            Deal deal = new Deal();
            deal.Code = code;
            deal.ID = codeID;
            deal.TestBeforeUpdate();
        }

        public static void ValidateInsertDealNumber(string dealNumber, int clientID) {
            RateDeal rateDeal = new RateDeal();
            rateDeal.DealNumber = dealNumber;
            rateDeal.ClientID = clientID;
            rateDeal.TestBeforeInsert();
        }

        public static void ValidateUpdateDealNumber(int rateDealID, string dealNumber, int clientID) {
            RateDeal rateDeal = new RateDeal();
            rateDeal.DealNumber = dealNumber;
            rateDeal.ClientID = clientID;
            rateDeal.ID = rateDealID;
            rateDeal.TestBeforeUpdate();
        }
    }
}