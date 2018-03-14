using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Broker.Controllers.EmployeeManagement;

/// <summary>
/// Summary description for Distribution
/// </summary>
/// 
namespace Broker.DataAccess {
    [Serializable]
    public partial class Distribution : EntityBase<Distribution> {

        public static List<Distribution> GetByName() {
            return new List<Distribution>();
        }
        //public static List<Distribution> GetByName(string name){
        //    return Table.Where(d => d.Name == name).ToList();
        //}

        public static Distribution GetByNumberAndSubType(int insuranceSubTypeID, string policyNumber) {
            return Table.Where(d => d.InsuranceSubTypeID == insuranceSubTypeID && d.PolicyNumber == policyNumber).SingleOrDefault();
        }

        public static Distribution GetByNumberAndSubTypeAndCompany(int insuranceSubTypeID, string policyNumber, int InsuranceCompanyID) {
            return Table.Where(d => d.InsuranceSubTypeID == insuranceSubTypeID && d.PolicyNumber == policyNumber && d.InsuranceCompanyID == InsuranceCompanyID).SingleOrDefault();
        }

        public static Distribution GetByNumberAndSubTypeAndCompanyIsAccepted(int insuranceSubTypeID, string policyNumber, int InsuranceCompanyID, int branchID) {
            return Table.Where(d => d.InsuranceSubTypeID == insuranceSubTypeID && d.PolicyNumber == policyNumber && d.InsuranceCompanyID == InsuranceCompanyID && d.BranchID == branchID && d.DistributionStatuse.Code == DistributionStatuse.POTVRDENA).SingleOrDefault();
        }

        public static Distribution GetByNumberAndSubTypeAndCompanyForPriem(int insuranceSubTypeID, string policyNumber, int InsuranceCompanyID) {
            return Table.Where(d => d.InsuranceSubTypeID == insuranceSubTypeID && d.PolicyNumber == policyNumber && d.InsuranceCompanyID == InsuranceCompanyID && d.DistributionStatuse.Code != DistributionStatuse.STORNO).SingleOrDefault();
        }

        public static Distribution GetByNumberAndSubTypeAndCompanyForIspratnica(int insuranceSubTypeID, string policyNumber, int InsuranceCompanyID) {
            return Table.Where(d => d.InsuranceSubTypeID == insuranceSubTypeID && d.PolicyNumber == policyNumber && d.InsuranceCompanyID == InsuranceCompanyID && d.DistributionStatuse.Code != DistributionStatuse.STORNO && d.BranchID == null).SingleOrDefault();
        }

        public static Distribution GetByNumberAndSubTypeAndCompanyForPovratnica(int insuranceSubTypeID, string policyNumber, int InsuranceCompanyID, int branchID) {
            return Table.Where(d => d.InsuranceSubTypeID == insuranceSubTypeID && d.PolicyNumber == policyNumber && d.InsuranceCompanyID == InsuranceCompanyID && d.DistributionStatuse.Code == DistributionStatuse.POTVRDENA && d.BranchID == branchID && d.IsUsed == false).SingleOrDefault();
        }

        public static void ValidateNumber(List<Distribution> dList) {
            foreach (Distribution d in dList) {
                d.ValidateNumber();
            }
        }
        public void ValidateNumber() {
            if (Distribution.GetByNumberAndSubTypeAndCompany(this.InsuranceSubTypeID, this.PolicyNumber, this.InsuranceCompanyID) != null) {
                ValidationErrors.Add("PolicyNumber", "Бројот е задолжен!");
            }
        }

        public void ValidateNumberForPriem() {
            if (Distribution.GetByNumberAndSubTypeAndCompanyForPriem(this.InsuranceSubTypeID, this.PolicyNumber, this.InsuranceCompanyID) != null) {
                ValidationErrors.Add("PolicyNumber", "Бројот е задолжен!");
            }
        }

        public void ValidateNumberForIspratnica() {
            if (Distribution.GetByNumberAndSubTypeAndCompanyForIspratnica(this.InsuranceSubTypeID, this.PolicyNumber, this.InsuranceCompanyID) == null) {
                ValidationErrors.Add("PolicyNumber", "Бројот не постои како слободен во централен магацин!");
            }
        }

        public void ValidateNumberForPovratnica(int branchID) {
            if (Distribution.GetByNumberAndSubTypeAndCompanyForPovratnica(this.InsuranceSubTypeID, this.PolicyNumber, this.InsuranceCompanyID, branchID) == null) {
                ValidationErrors.Add("PolicyNumber", "Полисата е искористена или не постои како потврдена во филијала!");
            }
        }

        public static void SetUsedNumber(string number, int companyID, int insuranceSubTypeID) {
            Distribution d = GetByNumberAndSubTypeAndCompany(insuranceSubTypeID, number, companyID);
            if (d != null) {
                d.IsUsed = true;
                d.Update2();
            }
        }
        public static Distribution GetSuggestedNumber(int companyID, int UserID, int insuranceSubtTypeID) {
            List<Distribution> dList = Table.Where(dis => dis.InsuranceCompanyID == companyID && dis.InsuranceSubTypeID == insuranceSubtTypeID && dis.UserID == UserID && !dis.IsUsed).OrderBy(dis => dis.PolicyNumber).ToList();
            if (dList.Count > 0) {
                //TemporaryDistribution.Insert(dList[0].ID, UserID);
                return dList[0];
            } else {
                return null;
                //List<Distribution> dTotalList = Table.Where(di => di.InsuranceCompanyID == companyID && di.InsuranceSubTypeID == insuranceSubtTypeID && di.User.Role.Name == RolesInfo.SEAdmin).OrderBy(di => di.PolicyNumber).ToList();
                //if (dTotalList.Count > 0) {
                //    foreach (Distribution d in dTotalList) {
                //        if (!TemporaryDistribution.Exist(d.ID, UserID)) {
                //            TemporaryDistribution.Insert(d.ID, UserID);
                //            return d;
                //        }
                //    }
                //    return null;
                //} else
                //    return null;
            }
        }

        public static List<Distribution> GetByNumberAndCompany(string policyNumber, int InsuranceCompanyID) {
            return Table.Where(d => d.PolicyNumber == policyNumber && d.InsuranceCompanyID == InsuranceCompanyID).ToList();
        }

        public static List<Distribution> GetByNumberAndCompanyAndBranch(string policyNumber, int InsuranceCompanyID, int branchID) {
            return Table.Where(d => d.PolicyNumber == policyNumber && d.InsuranceCompanyID == InsuranceCompanyID && d.BranchID == branchID && d.DistributionStatuse.Code == DistributionStatuse.POTVRDENA).ToList();
        }
    }
}
