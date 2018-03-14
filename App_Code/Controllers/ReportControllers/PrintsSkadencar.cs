using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Broker.DataAccess;

/// <summary>
/// Summary description for PrintsSkadencar
/// </summary>
/// 
namespace Broker.Controllers.ReportControllers {
    public class PrintsSkadencar {
        public static void PrintSkadencar(DateTime fromDate, DateTime toDate, List<int> lstBranches, List<int> lstInsuranceCompanies, List<int> lstInsuranceSubTypes) {
            DataClassesDataContext dcdc = new DataClassesDataContext();
            DateTime dt1 = fromDate;
            DateTime dt2 = toDate;
            int d1 = dt1.Day;
            int m1 = dt1.Month;
            int y1 = dt1.Year;
            int d2 = dt2.Day;
            int m2 = dt2.Month;
            int y2 = dt2.Year;
            PDFCreators pdf = new PDFCreators(false, 10, 10, 10, 10);
            pdf.OpenPDF();
            pdf.SetTitle("Скаденцар");
            pdf.SetTitle("Истек на полиса од " + fromDate.ToShortDateString() + " до " + toDate.ToShortDateString());
            pdf.SetTitleLeft10("Датум и време на печатење: " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString());
            string query = @"select p.id, pi.id as policyItemID, pi.policynumber, p.applicationdate, ist.shortdescription as insurancesubtypename, ic.shortname as insurancecompanyname, 
                            c.name as clientname, o.name as ownername, it.shortname as insurancetypename, p.startdate, p.enddate, ist.id as insuranceSubTypeID
                            from policies p 
                            inner join policyitems pi on pi.policyid = p.id
                            inner join clients c on p.clientid = c.id
                            inner join clients o on p.ownerid = o.id
                            inner join insurancecompanies ic on p.insurancecompanyid=ic.id
                            inner join insurancesubtypes ist on pi.insurancesubtypeid=ist.id
                            inner join insurancetypes it on ist.insurancetypeid=it.id
                            where p.branchid in (" + ReportFinPolicies.GetFromIDs(lstBranches) + ") " +
                        @" and p.insurancecompanyid in (" + ReportFinPolicies.GetFromIDs(lstInsuranceCompanies) + ") " +
                        @" and pi.insurancesubtypeid in (" + ReportFinPolicies.GetFromIDs(lstInsuranceSubTypes) + ") " +
                        @" and p.enddate>='" + y1 + -+m1 + -+d1 + "'" +
                        @" and p.enddate<='" + y2 + -+m2 + -+d2 + "'" +
                        @" and p.discard = 0 " +
                        @" order by p.enddate";
            //@" --query so marketing agent";
            List<SummaryForSkadencar> lst = dcdc.ExecuteQuery<SummaryForSkadencar>(query).ToList();
            int counter = 1;
            foreach (SummaryForSkadencar ss in lst) {

                string[] headers = new string[5];
                headers[0] = "Р.бр.: " + counter.ToString();
                headers[1] = "Полиса: " + ss.policynumber;
                headers[2] = "Класа: " + ss.insurancetypename;
                headers[3] = "Подкласа: " + ss.insurancesubtypename;
                headers[4] = "О.компанија: " + ss.insurancecompanyname;

                TypeCode[] codes = new TypeCode[headers.Length];

                codes[0] = TypeCode.String;
                codes[1] = TypeCode.String;
                codes[2] = TypeCode.String;
                codes[3] = TypeCode.String;
                codes[4] = TypeCode.String;

                float[] policyColumnsWidths = new float[headers.Length];
                policyColumnsWidths[0] = 16;
                policyColumnsWidths[1] = 20;
                policyColumnsWidths[2] = 24;
                policyColumnsWidths[3] = 24;
                policyColumnsWidths[4] = 16;
                pdf.CreateTableForSkadencar(headers.Count(), false, headers, "", policyColumnsWidths);
                object[] vals = new object[headers.Count()];
                vals[0] = "Издадена на: " + ss.applicationdate.ToShortDateString();
                vals[1] = "Скаденца: " + ss.startdate.ToShortDateString() + " - " + ss.enddate.ToShortDateString();
                vals[2] = "Договорувач: " + ss.clientname;
                vals[3] = "Осигуреник: " + ss.ownername;
                string regNumber = "";
                Broker.DataAccess.Control con = Broker.DataAccess.Control.GetByInsuranceSubTypeAndTexbBoxID(ss.insuranceSubTypeID,  Broker.DataAccess.Control.REGISTRATION_NUMBER);
                if(con!=null){
                    PolicyExtendInformation regNumberPEI = PolicyExtendInformation.GetByPolicyItemAndControl(ss.policyItemID, con.ID);
                    if (regNumberPEI != null) {
                        regNumber = regNumberPEI.Value;
                    }
                }
                vals[4] = "Регистрација: " + regNumber;
                pdf.AddDataRowWithBorder(vals, headers.Count(), codes);
                pdf.AddTable();
                counter++;
            }
            pdf.FinishPDF_FileName("Skadencar");
        }
    }
    
    public class SummaryForSkadencar {
        public int id { get; set; }
        public int policyItemID { get; set; }
        public string policynumber { get; set; }
        public DateTime applicationdate { get; set; }
        public int insuranceSubTypeID { get; set; }
        public string insurancesubtypename { get; set; }
        public string insurancecompanyname { get; set; }
        public string insurancetypename { get; set; }
        public string clientname { get; set; }
        public string ownername { get; set; }
        public DateTime startdate { get; set; }
        public DateTime enddate { get; set; }
    }
}