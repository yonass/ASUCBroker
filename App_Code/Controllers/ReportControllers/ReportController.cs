using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Broker.DataAccess;
using System.Collections.Generic;

/// <summary>
/// Summary description for ReportController
/// </summary>
namespace Broker.Controllers.ReportControllers {
    public class ReportController {


        public static void AllPolicies(DateTime fromDate, DateTime toDate, int insuranceTypeID, int insuranceSubTypeID, int userID, int branchID, int companyID) {
            DataClassesDataContext dcdc = new DataClassesDataContext();
            DateTime dt1 = fromDate;
            DateTime dt2 = toDate;
            int d1 = dt1.Day;
            int m1 = dt1.Month;
            int y1 = dt1.Year;
            int d2 = dt2.Day;
            int m2 = dt2.Month;
            int y2 = dt2.Year;
            string query = @"select clients.name ,clients.embg, policyItems.policynumber,policies.startdate, policies.enddate, users.username, policyitems.premiumvalue,
                            policyitems.realpremiumvalue,
                            case when policyItems.brokerageid is not null then 
                                        (case when clients.islaw=1 then 
                                        (select b.percentageforlaws  from brokerages b
                                        where b.insurancesubtypeid=policyItems.insurancesubtypeid and
                                        b.id=policyItems.brokerageid)
                                        else
                                        (select b.percentageforprivates from brokerages b
                                        where b.insurancesubtypeid=policyItems.insurancesubtypeid
                                        and b.id=policyItems.brokerageid) 
                                        end)
                                        else 
                                        (case when policyItems.packetbrokerageid is not null then 
                                        (case when clients.islaw=1 then 
                                        (select pist.BrokeragePecentageForPrivates from PacketsInsuranceSubTypes pist
                                        where pist.insurancesubtypeid=policyItems.insurancesubtypeid
                                        and pist.id=policyItems.packetbrokerageid)
                                        else
                                        (select pist.BrokeragePecentageForLaws from PacketsInsuranceSubTypes pist
                                        where pist.insurancesubtypeid=policyItems.insurancesubtypeid
                                        and pist.id=policyItems.packetbrokerageid)
                                        end)
                                        else 0 
                                        end)
                                        end as brokeragepercentage,
							 insurancesubtypes.shortdescription as insurancetypename
                             from clients INNER JOIN policies ON policies.clientid = clients.id 
                             INNER JOIN policyItems ON policyItems.policyid = policies.id 
                             INNER JOIN users ON users.id = policies.userID 
                             INNER JOIN insuranceSubTypes ON policyItems.InsuranceSubTypeID = insuranceSubTypes.ID
                             INNER JOIN insuranceTypes ON insuranceSubTypes.InsuranceTypeID = insuranceTypes.id
                             where policies.applicationdate >='" + y1 + -+m1 + -+d1 + "' and policies.applicationdate<='" + y2 + -+m2 + -+d2 + "' and policies.Discard = 0";
            if (insuranceTypeID > 0) {
                query += @" and insuranceTypes.ID = " + insuranceTypeID;
            }
            if (insuranceSubTypeID > 0) {
                query += @" and insuranceSubTypes.ID = " + insuranceSubTypeID;
            } if (userID > 0) {
                query += @" and users.ID = " + userID;
            }
            if (branchID > 0) {
                query += @" and policies.BranchID = " + branchID;
            }
            if (companyID > 0) {
                query += @" and policies.InsuranceCompanyID=" + companyID;
            }


            IEnumerable<Summary> summaryList = dcdc.ExecuteQuery<Summary>(query);

            PDFCreators pdf = new PDFCreators(false, 15, 15, 15, 15);
            pdf.OpenPDF();
            pdf.SetTitle("Продадени полиси за даден период");
            string[] headersP = { "РБ", "Број на полиса", "Осигуреник", "Почеток", "Истек", "Премија за наплата", "Брокеража(%)", "Брокеража", "Агент", "Тип" };
            float[] policyColumnsWidths = { 6, 9, 22, 9, 9, 9, 9, 9, 9, 9 };
            pdf.CreateTable(headersP.Length, false, headersP, "", policyColumnsWidths);

            decimal PremValue = 0;
            decimal BrokerageValue = 0;
            object[] vals;
            int i = 0;
            foreach (Summary s in summaryList) {
                i++;
                vals = new object[headersP.Length];
                vals[0] = i.ToString();
                vals[1] = s.policynumber;
                vals[2] = s.name;
                vals[3] = s.startdate.ToShortDateString();
                vals[4] = s.enddate.ToShortDateString();
                vals[5] = String.Format("{0:#,0.00}", s.premiumvalue);
                vals[6] = s.brokeragepercentage;
                decimal brokValue = (s.realpremiumvalue * s.brokeragepercentage / 100);
                vals[7] = String.Format("{0:#,0.00}", brokValue);
                vals[8] = s.username;
                vals[9] = s.insurancetypename;
                PremValue += s.premiumvalue;
                BrokerageValue += brokValue;
                pdf.AddDataRow1(vals, headersP.Length);
            }
            vals = new object[headersP.Length];
            vals[0] = ""; //i.ToString();
            vals[1] = "";
            vals[2] = "";
            vals[3] = "";
            vals[4] = "Вкупно";
            vals[5] = String.Format("{0:#,0.00}", PremValue);
            vals[6] = "";
            vals[7] = String.Format("{0:#,0.00}", BrokerageValue);
            vals[8] = "";
            vals[9] = "";
            pdf.AddDataRow1(vals, headersP.Length);
            pdf.AddTable();
            pdf.FinishPDF();
        }














    }
    public class Summary {
        public string name { get; set; }
        public string embg { get; set; }
        public string policynumber { get; set; }
        public DateTime startdate { get; set; }
        public DateTime enddate { get; set; }
        public string username { get; set; }
        public string insurancesubtypedescription { get; set; }
        public decimal premiumvalue { get; set; }
        public decimal realpremiumvalue { get; set; }
        public decimal brokeragevalue { get; set; }
        public decimal brokeragepercentage { get; set; }
        public string insurancetypename { get; set; }
    }
}
