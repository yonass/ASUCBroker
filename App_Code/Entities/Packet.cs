using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Packet
/// </summary>
/// 
namespace Broker.DataAccess {
    public partial class Packet : EntityBase<Packet> {

        public static List<Packet> GetForCompany(int companyID) {
            return Table.Where(p => p.InsuranceCompanyID == companyID).ToList();
        }

        public static List<Packet> GetActiveForCompany(int companyID)
        {
            return Table.Where(p => p.InsuranceCompanyID == companyID && p.IsActive == true).ToList();
        }

        public static List<Packet> GetActive() {
            return Table.Where(p => p.IsActive == true).ToList();
        }

        public static bool ExistCodeInPackets(string code) {
            return (Table.Where(d => d.Code == code && d.IsActive == true).SingleOrDefault() != null);
        }

        public static bool ExistOtherCode(int codeID, string code) {
            return (Table.Where(d => d.Code == code && d.ID != codeID && d.IsActive == true).SingleOrDefault() != null);
        }


        public override void Validate() {

        }

        public void TestBeforeInsert() {
            if (Packet.ExistCodeInPackets(this.Code)) {
                ValidationErrors.Add("PACKET_CODE_INSERT_EXISTS", this.Code + " е веќе зафатено!");
            }
            this.PerformCustomValidation();
        }

        public void TestBeforeUpdate() {
            if (Packet.ExistOtherCode(this.ID, this.Code)) {
                ValidationErrors.Add("PACKET_CODE_UPDATE_EXISTS", this.Code + " е веќе зафатено!");
            }
            this.PerformCustomValidation();
        }
    }
}
