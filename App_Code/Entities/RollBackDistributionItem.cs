using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Broker.Controllers.DistributionControllers;

/// <summary>
/// Summary description for RollBackDistributionItem
/// </summary>
namespace Broker.DataAccess {
    public partial class RollBackDistributionItem : EntityBase<RollBackDistributionItem> {

        public static void GetForRollBack(RollBackDistribution rbd, List<Policy> policyList) {
            List<RollBackDistributionItem> rbdiList = new List<RollBackDistributionItem>();
            decimal totalValue = 0;
            decimal brokerageValue = 0;
           // List<Policy> policyList = Policy.GetByCompanyAndPeriod(rbd.InsuranceCompanyID, rbd.StartDate, rbd.EndDate);
            foreach (Policy p in policyList) {
                foreach (PolicyItem pi in p.PolicyItems) {
                    totalValue += pi.PremiumValue;
                    brokerageValue += pi.BrokerageValue;
                    RollBackDistributionItem rdbi = new RollBackDistributionItem();
                    rdbi.PolicyItemID = pi.ID;
                    rdbi.RollBackDistributionID = rbd.ID;
                    pi.IsRollBacked = true;
                    rdbi.Insert();
                }
            }

            RollBackDistribution.UpdateValues(totalValue, brokerageValue, rbd.ID);
        }


        public static List<RollBackDistributionItem> GetByRollBackDistibution(int rollBackDistibutionID) {
            return Table.Where(c => c.RollBackDistributionID == rollBackDistibutionID).ToList();
        }
        public static IEnumerable<RollBackDistributionItem> GetEnumByRollBackDistibution(int rollBackDistibutionID) {
            return Table.Where(c => c.RollBackDistributionID == rollBackDistibutionID);
        }

        public static List<RollBackDistributionItem> GetByRollBackDistibutionAndInsuranceSubType(int rollBackDistibutionID, int insuranceSubTypeID) {
            return Table.Where(c => c.RollBackDistributionID == rollBackDistibutionID && c.PolicyItem.InsuranceSubTypeID == insuranceSubTypeID).ToList();
        }

        public static void InsertItems(RollBackDistribution rbd, List<RollBackDistributionInfo> previousMonths, List<RollBackDistributionInfo> currentMonths) {
            decimal totalValue = 0;
            decimal brokerageValue = 0;

            foreach (RollBackDistributionInfo rbdi in previousMonths) {
                if (rbdi.IsForRollBack) {
                    
                    PolicyItem pi = PolicyItem.Get(rbdi.ID);
                    if (!pi.Policy.Discard) {
                        totalValue += rbdi.PremiumValue;
                        brokerageValue += rbdi.BrokerageValue;
                    }
                    RollBackDistributionItem rbdItem = new RollBackDistributionItem();
                    rbdItem.PolicyItemID = rbdi.ID;
                    rbdItem.RollBackDistributionID = rbd.ID;
                    pi.IsRollBacked = true;
                    rbdItem.Insert();
                }
            }
            foreach (RollBackDistributionInfo rbdi in currentMonths) {
                if (rbdi.IsForRollBack) {
                    PolicyItem pi = PolicyItem.Get(rbdi.ID);
                    if (!pi.Policy.Discard) {
                        totalValue += rbdi.PremiumValue;
                        brokerageValue += rbdi.BrokerageValue;
                    }
                    RollBackDistributionItem rbdItem = new RollBackDistributionItem();
                    rbdItem.PolicyItemID = rbdi.ID;
                    rbdItem.RollBackDistributionID = rbd.ID;
                    pi.IsRollBacked = true;
                    rbdItem.Insert();
                }
            }
            RollBackDistribution.UpdateValues(totalValue, brokerageValue, rbd.ID);
        }

        public static void UnRollBackPolicies(RollBackDistribution rbd) {
            List<RollBackDistributionItem> items = GetByRollBackDistibution(rbd.ID);
            foreach (RollBackDistributionItem item in items) {
                item.PolicyItem.IsRollBacked = false;
                item.Update2();
            }
            //Context.SubmitChanges();
        }

        public static void InsertForCompany(RollBackDistributionCompanyInfo info, int rbdID) {
            
            List<RollBackDistributionItem> items = RollBackDistributionItem.GetByRollBackDistibution(info.ID);
            foreach (RollBackDistributionItem item in items) {
                RollBackDistributionItem newItem = new RollBackDistributionItem();
                newItem.RollBackDistributionID = rbdID;
                newItem.PolicyItemID = item.PolicyItemID;
                newItem.Insert();
            }
        }
    }
}
