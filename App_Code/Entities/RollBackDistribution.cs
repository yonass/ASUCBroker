using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Broker.Controllers.DistributionControllers;

/// <summary>
/// Summary description for RollBackDistribution
/// </summary>
/// 
namespace Broker.DataAccess {
    public partial class RollBackDistribution : EntityBase<RollBackDistribution> {

        public static List<RollBackDistribution> GetByCompanyAndTimePeriod(int companyID, DateTime startDate, DateTime endDate) {
            List<Policy> pList = Policy.GetByCompanyAndPeriod(companyID, startDate, endDate);
            List<RollBackDistribution> rbdList = new List<RollBackDistribution>();
            foreach (Policy p in pList) {
                //    foreach (PolicyItem pi in p.PolicyItems) {
                //        if (!RollBackDistribution.Exist(pi.ID)) {
                //            RollBackDistribution rbd = new RollBackDistribution();
                //            rbd.Date = DateTime.Today;
                //            rbd.InsuranceCompanyID = p.InsuranceCompanyID;
                //            rbd.InsuranceCompany = p.InsuranceCompany;
                //            rbd.InsuranceSubTypeID = pi.InsuranceSubTypeID;
                //            rbd.InsuranceSubType = pi.InsuranceSubType;
                //            rbd.PolicyNumber = pi.PolicyNumber;
                //            rbd.UserID = p.UserID;
                //            rbd.User = p.User;
                //            rbd.PolicyItemID = pi.ID;
                //            rbdList.Add(rbd);
                //        }
                //    }
            }
            return rbdList;
        }

        public static bool Exist(int policyItemID) {
            //return Table.Where(r => r.PolicyItemID == policyItemID).SingleOrDefault() != null;
            return true;
        }
        public static List<RollBackDistribution> GetByCompanyAndTimePeriod() {
            return null;
        }

        public static RollBackDistribution GetRollBack(DateTime startDate, DateTime endDate, int insuranceCompanyID) {
            RollBackDistribution rbd = new RollBackDistribution();
            rbd.StartDate = startDate;
            rbd.EndDate = endDate;
            rbd.Date = DateTime.Today;
            rbd.InsuranceCompanyID = insuranceCompanyID;
            rbd.Number = GetNextNumber();
            rbd.TotalValue = 0;
            rbd.BrokerageValue = 0;
            return rbd;
        }

        public static int GetNextNumber() {
            IEnumerable<RollBackDistribution> rbdEnum = Table.OrderByDescending(c => c.Number);
            if (rbdEnum.FirstOrDefault() == null) {
                return 1;
            } else
                return rbdEnum.First().Number + 1;
        }

        public static int InsertRollBackDistribution(DateTime startDate, DateTime endDate, int insuranceCompanyID) {
            RollBackDistribution rbd = GetRollBack(startDate, endDate, insuranceCompanyID);
            List<Policy> policyList = Policy.GetByCompanyAndPeriodForRollBack(rbd.InsuranceCompanyID, rbd.StartDate, rbd.EndDate);
            if (policyList.Count > 0) {
                rbd.Insert();
                RollBackDistributionItem.GetForRollBack(rbd,policyList);
                return policyList.Count;
            }
            return 0;
        }

        public static void UpdateValues(decimal totalValue, decimal brokerageValue, int rbdID) {
            RollBackDistribution rbd = RollBackDistribution.Get(rbdID);
            rbd.TotalValue = totalValue;
            rbd.BrokerageValue = brokerageValue;
            Table.Context.SubmitChanges();
        }

        public static void InsertByInsuranceSubTypeAndCompanyFromBranch(int companyID, int branchID, int userID, List<RollBackDistributionInfo> listOldRollBackInfos, List<RollBackDistributionInfo> listNewRollBackInfos, DateTime startDate, DateTime endDate) {
            RollBackDistribution rbd = new RollBackDistribution();
            rbd.BranchID = branchID;
            rbd.Date = DateTime.Now;
            rbd.StartDate = startDate;
            rbd.EndDate = endDate;
            rbd.InsuranceCompanyID = companyID;
            rbd.Number = GetNextNumber();
            rbd.RollBackStatusID = RollBackDistributionStatuse.GetByCode(RollBackDistributionStatuse.NEPOTVRDENA_OD_FILIJALA).ID;
            rbd.UserID = userID;
            rbd.Insert();
            RollBackDistributionItem.InsertItems(rbd, listOldRollBackInfos, listNewRollBackInfos);
        }

        public static void SetConfirmedFromBranch(RollBackDistribution rbd) {
            RollBackDistributionStatuse rbds = RollBackDistributionStatuse.GetByCode(RollBackDistributionStatuse.POTVRDENA_OD_FILIJALA);
            rbd.RollBackStatusID = rbds.ID;
            RollBackDistributionStatuse.Table.Context.SubmitChanges();
        }

        public static void SetConfirmedFromCompany(RollBackDistribution rbd)
        {
            RollBackDistributionStatuse rbds = RollBackDistributionStatuse.GetByCode(RollBackDistributionStatuse.POTVRDENA_OD_KOMPANIJA);
            rbd.RollBackStatusID = rbds.ID;
            RollBackDistributionStatuse.Table.Context.SubmitChanges();
        }

        public static void SetDiscardedFromBranch(RollBackDistribution rbd) {
            RollBackDistributionStatuse rbds = RollBackDistributionStatuse.GetByCode(RollBackDistributionStatuse.STORNIRANA_OD_FILIJALA);
            rbd.RollBackStatusID = rbds.ID;
            RollBackDistributionStatuse.Table.Context.SubmitChanges();
            RollBackDistributionItem.UnRollBackPolicies(rbd);
        }

        public static void SetDiscardedFromCompany(RollBackDistribution rbd)
        {
            RollBackDistributionStatuse rbds = RollBackDistributionStatuse.GetByCode(RollBackDistributionStatuse.STORNIRANA_OD_KOMPANIJA);
            rbd.RollBackStatusID = rbds.ID;
            RollBackDistributionStatuse.Table.Context.SubmitChanges();
            RollBackDistributionItem.UnRollBackPolicies(rbd);
        }

        public static void InsertForCompany(List<RollBackDistributionCompanyInfo> finalList, int userID) {

            RollBackDistribution rdb = new RollBackDistribution();
            rdb.BranchID = null;
            decimal[]  values = getSums(finalList);
            rdb.BrokerageValue = values[1];
            rdb.TotalValue = values[0];
            rdb.StartDate = RollBackDistributionCompanyInfo.getDate(finalList, true);
            rdb.EndDate = RollBackDistributionCompanyInfo.getDate(finalList, false);
            rdb.Date = DateTime.Now;
            rdb.InsuranceCompanyID = finalList.First().InsuranceCompanyID;
            rdb.Number = GetNextNumber();
            rdb.RollBackStatusID = RollBackDistributionStatuse.GetByCode(RollBackDistributionStatuse.NEPOTVRDENA_OD_KOMPANIJA).ID;
            rdb.UserID = userID;
            rdb.Insert();

            RollBackDistributionStatuse finalized = RollBackDistributionStatuse.GetByCode(RollBackDistributionStatuse.ZAVRSENA_OD_FILIJALA);
            foreach (RollBackDistributionCompanyInfo info in finalList) {
                RollBackDistribution.Get(info.ID).RollBackStatusID = finalized.ID;
                Table.Context.SubmitChanges();
                RollBackDistributionItem.InsertForCompany(info,rdb.ID);
                RollBackDistributionParentship.InsertParentShip(rdb.ID, info.ID);
            }
        }

        public static decimal[] getSums(List<RollBackDistributionCompanyInfo> finalList) {
            decimal brokerage = 0;
            decimal totalSum = 0;
            foreach (RollBackDistributionCompanyInfo info in finalList) {
                RollBackDistribution roll = RollBackDistribution.Get(info.ID);
                totalSum += roll.TotalValue;
                brokerage += roll.BrokerageValue;
            }
            decimal[] returnArray = new decimal[2];
            returnArray[0] = totalSum;
            returnArray[1] = brokerage;
            return returnArray;
        }

        public static bool CanMakeGroupRollBack(List<RollBackDistributionCompanyInfo> finalList) {
            int companyID = finalList.First().InsuranceCompanyID;
            foreach (RollBackDistributionCompanyInfo info in finalList) {
                if (companyID != info.InsuranceCompanyID) {
                    return false;
                }
            }
            return true;
        }

    }

}

