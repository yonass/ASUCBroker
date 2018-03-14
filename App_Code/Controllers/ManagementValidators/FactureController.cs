using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Broker.DataAccess;

/// <summary>
/// Summary description for FactureController
/// </summary>
/// 
namespace Broker.Controllers.ManagementControllers {
    public class FactureController {
        public static void ValidateInsertFactureNumber(string factureNumber) {
            Broker.DataAccess.Facture facture = new Broker.DataAccess.Facture();
            facture.FactureNumber = factureNumber;
            facture.TestBeforeInsert();
        }

        public static void ValidateUpdateFactureNumber(int factureID, string factureNumber) {
            Broker.DataAccess.Facture facture = new Broker.DataAccess.Facture();
            facture.FactureNumber = factureNumber;
            facture.ID = factureID;
            facture.TestBeforeUpdate();
        }
    }
}