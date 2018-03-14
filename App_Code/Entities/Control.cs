using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Control
/// </summary>
/// 
namespace Broker.DataAccess {
    public partial class Control : EntityBase<Control> {

        public static string REGISTRATION_NUMBER = "tbRegistrationNumber";
        public static string CHASSIS_NUMBER = "tbChassisNumber";
        public static string BRAND = "tbBrand";
        public static string MODEL = "tbModel";

        public static List<Control> GetByInsuranceSubType(int insuranceSubTypeID) {
            return Table.Where(c => c.InsuranceSubTypeID == insuranceSubTypeID).ToList();
        }

        public static List<Control> GetActiveByInsuranceSubType(int insuranceSubTypeID) {
            return Table.Where(c => c.InsuranceSubTypeID == insuranceSubTypeID && c.IsActive).ToList();
        }

        public static List<Control> GetForReportByInsuranceSubType(int insuranceSubTypeID) {
            SpecialFieldType sft = SpecialFieldType.GetByCode(SpecialFieldType.REPORT);
            return Table.Where(c => c.InsuranceSubTypeID == insuranceSubTypeID && c.SpecialFieldTypeID == sft.ID).ToList();
        }
        public static List<Control> GetForFiscalByInsuranceSubType(int insuranceSubTypeID) {
            SpecialFieldType sft = SpecialFieldType.GetByCode(SpecialFieldType.FISCAL);
            List<Control> controlsList = Table.Where(c => c.InsuranceSubTypeID == insuranceSubTypeID && c.IsActive == true).ToList();
            List<Control> returnList = new List<Control>();
            foreach (Control c in controlsList) {
                if (ControlsSpecialType.IsForFiscal(c.ID)) {
                    returnList.Add(c);
                }
            }
            return returnList;
            //return Table.Where(c => c.InsuranceSubTypeID == insuranceSubTypeID && c.SpecialFieldTypeID == sft.ID).ToList();
        }

        public static Control GetByInsuranceSubTypeAndTexbBoxID(int insuranceSubTypeID, string textBoxID) {
            return Table.Where(c => c.InsuranceSubTypeID == insuranceSubTypeID && c.TextBoxID == textBoxID).SingleOrDefault();
        }

        public static List<Control> SelectSearchParameters(int insuranceSubTypeID) {

            SpecialFieldType sft = SpecialFieldType.GetByCode(SpecialFieldType.SEARCH);
            return Table.Where(c => c.InsuranceSubTypeID == insuranceSubTypeID && c.IsActive == true && c.SpecialFieldTypeID == sft.ID).ToList();



        }

        public static List<Control> SelectSearchParametersExtend(int insuranceSubTypeID) {

            List<Control> retList = new List<Control>();
            SpecialFieldType sft = SpecialFieldType.GetByCode(SpecialFieldType.SEARCH);
            Control dummyControl = new Control();
            dummyControl.ID = 0;
            dummyControl.LabelName = "Број на полиса";
            retList.Add(dummyControl);
            Control dummyControlEMBGClient = new Control();
            dummyControlEMBGClient.ID = -1;
            dummyControlEMBGClient.LabelName = "ЕМБГ Дог.";
            retList.Add(dummyControlEMBGClient);
            Control dummyControlEMBGOwner = new Control();
            dummyControlEMBGOwner.ID = -2;
            dummyControlEMBGOwner.LabelName = "ЕМБГ Оси.";
            retList.Add(dummyControlEMBGOwner);
            //List<Control> searchControlsList = Table.Where(c => c.InsuranceSubTypeID == insuranceSubTypeID && c.IsActive == true && c.SpecialFieldTypeID == sft.ID).ToList();
            List<Control> searchControlsList = ControlsSpecialType.Table.Where(c => c.Control.InsuranceSubTypeID == insuranceSubTypeID && c.IsActive == true && c.SpecialFieldTypeID == sft.ID).Select(c=>c.Control).ToList();
            retList.AddRange(searchControlsList);
            return retList;
        }

        public static bool BelongInOtherInsuranceSubType(Control c) {
            ControlAppropriateInsuranceSubType caist = ControlAppropriateInsuranceSubType.GetByControl(c);
            if (caist != null) {
                if (caist.InsuranceSubTypeID != c.InsuranceSubTypeID) {
                    return true;
                }
            }
            return false;
        }

    }
}
