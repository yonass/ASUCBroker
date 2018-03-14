using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Broker.DataAccess;

/// <summary>
/// Summary description for ReportForASO
/// </summary>
/// 
namespace Broker.Controllers.ReportControllers {
    public class ReportForASO {
        public static void PrintReportForASO(int insuranceCompanyID, DateTime startDate, DateTime endDate) {

            DateTime dt1 = startDate;
            DateTime dt2 = endDate;
            int d1 = dt1.Day;
            int m1 = dt1.Month;
            int y1 = dt1.Year;
            int d2 = dt2.Day;
            int m2 = dt2.Month;
            int y2 = dt2.Year;

            DataClassesDataContext dcdc = new DataClassesDataContext();
            PDFCreators pdf = new PDFCreators(false, 17, 17, 25, 25);
            pdf.OpenPDF();
            pdf.SetTitle("  ");
            pdf.SetTitle("  ");
            pdf.AddJDBLogo(660, 480);
            pdf.SetTitle("Квартален извештај за Брокерските друштва");
            pdf.SetTitle("за период " + startDate.ToShortDateString() + " - " + endDate.ToShortDateString());

            if (insuranceCompanyID > 0) {
                InsuranceCompany ic = InsuranceCompany.Get(insuranceCompanyID);
                pdf.SetTitle("Осигурителна компанија: " + ic.ShortName);
            }
            pdf.SetTitleLeft("");

            string[] headers = new string[6];
            headers[0] = "Класа/подкласа на осигурување";
            headers[1] = "Шифра";
            headers[2] = "Број на договори";
            headers[3] = "Премија";
            headers[4] = "Реализирана провизија";
            headers[5] = "Пресметана провизија";


            TypeCode[] codes = new TypeCode[headers.Count()];

            codes[0] = TypeCode.String;
            codes[1] = TypeCode.String;
            codes[2] = TypeCode.Int32;
            codes[3] = TypeCode.Decimal;
            codes[4] = TypeCode.Decimal;
            codes[5] = TypeCode.Decimal;

            float[] policyColumnsWidths = new float[headers.Count()];
            policyColumnsWidths[0] = 55;
            policyColumnsWidths[1] = 5;
            policyColumnsWidths[2] = 10;
            policyColumnsWidths[3] = 10;
            policyColumnsWidths[4] = 10;
            policyColumnsWidths[5] = 10;


            pdf.CreateTableWithBorder(headers.Count(), false, headers, "", policyColumnsWidths);
            int totalNumberOfPolicies = 0;
            decimal totalPremiumValue = 0;
            decimal totalBrokValue = 0;
            decimal totalRealBrokValue = 0;
            List<InsuranceType> listInsuranceTypes = InsuranceType.Table.OrderBy(c => c.Code).ToList();
            foreach (InsuranceType it in listInsuranceTypes) {
                object[] vals = new object[headers.Count()];
                vals[0] = it.Name;
                vals[1] = it.Code;
                List<InsuranceSubType> listInsuranceSubTypes = InsuranceSubType.GetByInsuranceType(it.ID).OrderBy(c => c.Code).ToList();
                int numberOfPolicies = 0;
                int premiumValue = 0;
                decimal brokValue = 0;
                decimal realBrokValue = 0;
                decimal tmpBrokValue = 0;
                decimal tmpBrokBaseValue = 0;
                decimal totFacturedValueInClass = 0;
                decimal totFacturedBrokerageValuePerClass = 0;
                //decimal tmpBrokValueInOtherClass = 0;
                decimal k = 0;
                foreach (InsuranceSubType ist in listInsuranceSubTypes) {
                    string queryCountAndPremiumDef = string.Empty;
                    string queryCountAndPremiumFullPremiumValue = string.Empty;
                    string queryCountAndPremiumInOtherClasses = string.Empty;
                    string getCalculatedBrokerages = string.Empty;
                    string queryCalcBrokerageInOtherClases = string.Empty;
                    string queryRealPaidValues = string.Empty;
                    string queryGetFacturedBrokeragesPayments = string.Empty;
                    decimal tmpBrokValueInOtherClass = 0;
                    decimal tmpBrokBaseValueTot = 0;
                    decimal tmpBrokValueTot = 0;

                    
                    if (insuranceCompanyID > 0) {
                        queryCountAndPremiumFullPremiumValue = @"select count(pi1.id) as Count, 
                                                       sum(cast(pi1.premiumvalue as int))   as SumPremiumValue
                                                       from policies p1 inner join policyitems pi1 on pi1.policyid=p1.id
                                                       where p1.discard=0
                                                       and pi1.insurancesubtypeid=" + ist.ID +
                                                       " and p1.insurancecompanyid = " + insuranceCompanyID +
                                                       @" and p1.applicationdate >='" + y1 + -+m1 + -+d1 + "' and p1.applicationdate<='" + y2 + -+m2 + -+d2 + "'";
                        queryCountAndPremiumDef = @"select 1, sum(cast(pei.Value as int)) as SumPremiumValue
                                                       from policies p ,policyitems pi, 
                                                       policyextendinformations pei,controls c, controlappropriateinsurancesubtype caist
                                                       where p.discard=0
                                                       and caist.isactive=1
                                                       and pi.policyid=p.id
                                                       and caist.controlid=pei.controlid
                                                       and c.variabletypeid in (2,3,4)
                                                       and c.specialfieldtypeid = 2
                                                       and c.insurancesubtypeid=" + ist.ID +
                                                       @" and pei.policyitemid=pi.id 
                                                       and pei.controlid=c.id
                                                       and pei.Value<>'0'
                                                       and p.insurancecompanyid = " + insuranceCompanyID +
                                                       @" and p.applicationdate >='" + y1 + -+m1 + -+d1 + "' and p.applicationdate<='" + y2 + -+m2 + -+d2 + "'";
                        queryCountAndPremiumInOtherClasses = @"select count(pi.id) as Count, sum(cast(pei.Value as int)) as SumPremiumValue
                                                                  from policies p ,policyitems pi, 
                                                                  policyextendinformations pei,controls c, controlappropriateinsurancesubtype caist
                                                                  where p.discard=0
                                                                  and caist.insurancesubtypeid=" + ist.ID +
                                                                      @" and pi.policyid=p.id
                                                                  and caist.controlid=pei.controlid
                                                                  and caist.isactive=1
                                                                  and pei.policyitemid=pi.id 
                                                                  and pei.controlid=c.id
                                                                  and pei.Value<>'0'
                                                                  and p.insurancecompanyid = " + insuranceCompanyID +
                                                                  @" and p.applicationdate >='" + y1 + -+m1 + -+d1 + "' and p.applicationdate<='" + y2 + -+m2 + -+d2 + "'";
                        getCalculatedBrokerages = @"select pi1.id, pi1.insurancesubtypeid,pi1.premiumvalue,
                                        (select sum(convert(int,pei.Value))
                                        from policyitems pi, 
                                        policyextendinformations pei,controls c, controlappropriateinsurancesubtype caist
                                        where caist.isactive=1
                                        and caist.controlid=pei.controlid
                                        and c.variabletypeid in (2,3,4)
                                        and c.specialfieldtypeid = 2
                                        and pei.policyitemid=pi.id 
                                        and pei.controlid=c.id
                                        and pei.Value<>'0'
                                        and pi.id = pi1.id) as basicvalue,
                                        case when pi1.brokerageid is not null then 
                                        (case when c1.islaw=1 then 
                                        (select b.percentageforlaws  from brokerages b
                                        where b.insurancesubtypeid=pi1.insurancesubtypeid and
                                        b.id=pi1.brokerageid)
                                        else
                                        (select b.percentageforprivates from brokerages b
                                        where b.insurancesubtypeid=pi1.insurancesubtypeid
                                        and b.id=pi1.brokerageid) 
                                        end)
                                        else 
                                        (case when pi1.packetbrokerageid is not null then 
                                        (case when c1.islaw=1 then 
                                        (select pist.BrokeragePecentageForPrivates from PacketsInsuranceSubTypes pist
                                        where pist.insurancesubtypeid=pi1.insurancesubtypeid
                                        and pist.id=pi1.packetbrokerageid)
                                        else
                                        (select pist.BrokeragePecentageForLaws from PacketsInsuranceSubTypes pist
                                        where pist.insurancesubtypeid=pi1.insurancesubtypeid
                                        and pist.id=pi1.packetbrokerageid)
                                        end)
                                        else 0 
                                        end)
                                        end as brokeragepercentage
                                        from policies p1 inner join policyitems pi1 on pi1.policyid=p1.id
                                        inner join clients c1 on p1.clientid=c1.id
                                        where p1.discard=0
                                        and pi1.insurancesubtypeid=" + ist.ID +
                                        @" and p1.insurancecompanyid = " + insuranceCompanyID +
                                        @" and p1.applicationdate >='" + y1 + -+m1 + -+d1 + "' and p1.applicationdate<='" + y2 + -+m2 + -+d2 + "'";
                        queryCalcBrokerageInOtherClases = @"select pi.id, pei.Value,
                                        case when pi.brokerageid is not null then 
                                        (case when c1.islaw=1 then 
                                        (select b.percentageforlaws  from brokerages b
                                        where b.insurancesubtypeid=pi.insurancesubtypeid and
                                        b.id=pi.brokerageid)
                                        else
                                        (select b.percentageforprivates from brokerages b
                                        where b.insurancesubtypeid=pi.insurancesubtypeid
                                        and b.id=pi.brokerageid) 
                                        end)
                                        else 
                                        (case when pi.packetbrokerageid is not null then 
                                        (case when c1.islaw=1 then 
                                        (select pist.BrokeragePecentageForPrivates from PacketsInsuranceSubTypes pist
                                        where pist.insurancesubtypeid=pi.insurancesubtypeid
                                        and pist.id=pi.packetbrokerageid)
                                        else
                                        (select pist.BrokeragePecentageForLaws from PacketsInsuranceSubTypes pist
                                        where pist.insurancesubtypeid=pi.insurancesubtypeid
                                        and pist.id=pi.packetbrokerageid)
                                        end)
                                        else 0 
                                        end)
                                        end as brokeragepercentage
                                                                  from policies p ,policyitems pi, clients c1,
                                                                  policyextendinformations pei,controls c, controlappropriateinsurancesubtype caist
                                                                  where p.discard=0
                                                                  and p.clientid=c1.id
                                                                  and caist.insurancesubtypeid=" + ist.ID +
                                                                  @" and pi.policyid=p.id
                                                                  and caist.controlid=pei.controlid
                                                                  and caist.isactive=1
                                                                  and pei.policyitemid=pi.id 
                                                                  and pei.controlid=c.id
                                                                  and pei.Value<>'0' " +
                                                                  @" and p.insurancecompanyid = " + insuranceCompanyID +
                                                                  @" and p.applicationdate >='" + y1 + -+m1 + -+d1 + "' and p.applicationdate<='" + y2 + -+m2 + -+d2 + "'";
                        queryRealPaidValues = @"select sum(fcpvpist.paidvalue) as sumpaidvalue
                                                from  FacCollPaidValuesPerInsSubTypes fcpvpist,facturecollectedpaidvalues fcpv, factureitems fi, factures f
                                                where fcpvpist.facturecollectedpaidvalueid=fcpv.id
                                                and fcpv.factureitemid=fi.id
                                                and fi.factureid=f.id
                                                and f.discard=0
                                                and fcpv.paiddate>='" + y1 + -+m1 + -+d1 + "' and fcpv.paiddate<='" + y2 + -+m2 + -+d2 + "'" +
                                                @" and fcpvpist.insurancesubtypeid=" + ist.ID +
                                                @" and f.insurancecompanyid=" + insuranceCompanyID;
                        queryGetFacturedBrokeragesPayments = @"select sum(ppist.brokeragevalue) as tmpValueDecimal from PaymentsPerInsSubTypes ppist, payments pa, rates r,
                                                policyitems pi, policyitemfactureitems pifi, factureitems fi, factures f, policies p, facturepayments fp
                                                where ppist.paymentid=pa.id
                                                and pa.rateid=r.id
                                                and r.policyitemid=pi.id
                                                and pi.policyid=p.id
                                                and pifi.policyitemid=pi.id
                                                and pifi.factureitemid=fi.id
                                                and fi.factureid=f.id
                                                and f.discard=0
                                                and fp.factureid=f.id   
												and fp.paymentid=pa.id
                                                and p.discard=0
                                                and p.applicationdate>='" + y1 + -+m1 + -+d1 + "'" +
                                                @" and p.applicationdate<='" + y2 + -+m2 + -+d2 + "'" +
                                                @" and p.insurancecompanyid=" + insuranceCompanyID +
                                                @" and ppist.insurancesubtypeid=" + ist.ID;
                        

                    } else {
                        queryCountAndPremiumFullPremiumValue = @"select count(pi1.id) as Count, 
                                                       sum(cast(pi1.premiumvalue as int))   as SumPremiumValue
                                                       from policies p1 inner join policyitems pi1 on pi1.policyid=p1.id
                                                       where p1.discard=0
                                                       and pi1.insurancesubtypeid=" + ist.ID +
                                                       @" and p1.applicationdate >='" + y1 + -+m1 + -+d1 + "' and p1.applicationdate<='" + y2 + -+m2 + -+d2 + "'";
                        queryCountAndPremiumDef = @"select 1, sum(cast(pei.Value as int)) as SumPremiumValue
                                                       from policies p ,policyitems pi, 
                                                       policyextendinformations pei,controls c, controlappropriateinsurancesubtype caist
                                                       where p.discard=0
                                                       and caist.isactive=1
                                                       and pi.policyid=p.id
                                                       and caist.controlid=pei.controlid
                                                       and c.variabletypeid in (2,3,4)
                                                       and c.specialfieldtypeid = 2
                                                       and c.insurancesubtypeid=" + ist.ID +
                                                       @" and pei.policyitemid=pi.id 
                                                       and pei.controlid=c.id
                                                       and pei.Value<>'0'" +
                                                       @" and p.applicationdate >='" + y1 + -+m1 + -+d1 + "' and p.applicationdate<='" + y2 + -+m2 + -+d2 + "'";
                        queryCountAndPremiumInOtherClasses = @"select count(pi.id) as Count, sum(cast(pei.Value as int)) as SumPremiumValue
                                                                  from policies p ,policyitems pi, 
                                                                  policyextendinformations pei,controls c, controlappropriateinsurancesubtype caist
                                                                  where p.discard=0
                                                                  and caist.insurancesubtypeid=" + ist.ID +
                                                                      @" and pi.policyid=p.id
                                                                  and caist.controlid=pei.controlid
                                                                  and caist.isactive=1
                                                                  and pei.policyitemid=pi.id 
                                                                  and pei.controlid=c.id
                                                                  and pei.Value<>'0'" +
                                                                  @" and p.applicationdate >='" + y1 + -+m1 + -+d1 + "' and p.applicationdate<='" + y2 + -+m2 + -+d2 + "'";
                        getCalculatedBrokerages = @"select pi1.id, pi1.insurancesubtypeid,pi1.premiumvalue,
                                        (select sum(convert(int,pei.Value))
                                        from policyitems pi, 
                                        policyextendinformations pei,controls c, controlappropriateinsurancesubtype caist
                                        where caist.isactive=1
                                        and caist.controlid=pei.controlid
                                        and c.variabletypeid in (2,3,4)
                                        and c.specialfieldtypeid = 2
                                        and pei.policyitemid=pi.id 
                                        and pei.controlid=c.id
                                        and pei.Value<>'0'
                                        and pi.id = pi1.id) as basicvalue,
                                        case when pi1.brokerageid is not null then 
                                        (case when c1.islaw=1 then 
                                        (select b.percentageforlaws  from brokerages b
                                        where b.insurancesubtypeid=pi1.insurancesubtypeid and
                                        b.id=pi1.brokerageid)
                                        else
                                        (select b.percentageforprivates from brokerages b
                                        where b.insurancesubtypeid=pi1.insurancesubtypeid
                                        and b.id=pi1.brokerageid) 
                                        end)
                                        else 
                                        (case when pi1.packetbrokerageid is not null then 
                                        (case when c1.islaw=1 then 
                                        (select pist.BrokeragePecentageForPrivates from PacketsInsuranceSubTypes pist
                                        where pist.insurancesubtypeid=pi1.insurancesubtypeid
                                        and pist.id=pi1.packetbrokerageid)
                                        else
                                        (select pist.BrokeragePecentageForLaws from PacketsInsuranceSubTypes pist
                                        where pist.insurancesubtypeid=pi1.insurancesubtypeid
                                        and pist.id=pi1.packetbrokerageid)
                                        end)
                                        else 0 
                                        end)
                                        end as brokeragepercentage
                                        from policies p1 inner join policyitems pi1 on pi1.policyid=p1.id
                                        inner join clients c1 on p1.clientid=c1.id
                                        where p1.discard=0
                                        and pi1.insurancesubtypeid=" + ist.ID +
                                        @" and p1.applicationdate >='" + y1 + -+m1 + -+d1 + "' and p1.applicationdate<='" + y2 + -+m2 + -+d2 + "'";
                        queryCalcBrokerageInOtherClases = @"select pi.id, pei.Value,
                                        case when pi.brokerageid is not null then 
                                        (case when c1.islaw=1 then 
                                        (select b.percentageforlaws  from brokerages b
                                        where b.insurancesubtypeid=pi.insurancesubtypeid and
                                        b.id=pi.brokerageid)
                                        else
                                        (select b.percentageforprivates from brokerages b
                                        where b.insurancesubtypeid=pi.insurancesubtypeid
                                        and b.id=pi.brokerageid) 
                                        end)
                                        else 
                                        (case when pi.packetbrokerageid is not null then 
                                        (case when c1.islaw=1 then 
                                        (select pist.BrokeragePecentageForPrivates from PacketsInsuranceSubTypes pist
                                        where pist.insurancesubtypeid=pi.insurancesubtypeid
                                        and pist.id=pi.packetbrokerageid)
                                        else
                                        (select pist.BrokeragePecentageForLaws from PacketsInsuranceSubTypes pist
                                        where pist.insurancesubtypeid=pi.insurancesubtypeid
                                        and pist.id=pi.packetbrokerageid)
                                        end)
                                        else 0 
                                        end)
                                        end as brokeragepercentage
                                                                  from policies p ,policyitems pi, clients c1,
                                                                  policyextendinformations pei,controls c, controlappropriateinsurancesubtype caist
                                                                  where p.discard=0
                                                                  and p.clientid=c1.id
                                                                  and caist.insurancesubtypeid=" + ist.ID +
                                                                  @" and pi.policyid=p.id
                                                                  and caist.controlid=pei.controlid
                                                                  and caist.isactive=1
                                                                  and pei.policyitemid=pi.id 
                                                                  and pei.controlid=c.id
                                                                  and pei.Value<>'0' " +
                                                                  @" and p.applicationdate >='" + y1 + -+m1 + -+d1 + "' and p.applicationdate<='" + y2 + -+m2 + -+d2 + "'";
                        queryRealPaidValues = @"select sum(fcpvpist.paidvalue) as sumpaidvalue
                                                from  FacCollPaidValuesPerInsSubTypes fcpvpist,facturecollectedpaidvalues fcpv, factureitems fi, factures f
                                                where fcpvpist.facturecollectedpaidvalueid=fcpv.id
                                                and fcpv.factureitemid=fi.id
                                                and fi.factureid=f.id
                                                and f.discard=0
                                                and fcpv.paiddate>='" + y1 + -+m1 + -+d1 + "' and fcpv.paiddate<='" + y2 + -+m2 + -+d2 + "'" +
                                                @" and fcpvpist.insurancesubtypeid=" + ist.ID;

                       
                        queryGetFacturedBrokeragesPayments = @"select sum(ppist.brokeragevalue) as tmpValueDecimal from PaymentsPerInsSubTypes ppist, payments pa, rates r,
                                                policyitems pi, policyitemfactureitems pifi, factureitems fi, factures f, policies p, facturepayments fp
                                                where ppist.paymentid=pa.id
                                                and pa.rateid=r.id
                                                and r.policyitemid=pi.id
                                                and pi.policyid=p.id
                                                and pifi.policyitemid=pi.id
                                                and pifi.factureitemid=fi.id
                                                and fi.factureid=f.id
                                                and fp.factureid=f.id   
												and fp.paymentid=pa.id
                                                and f.discard=0
                                                and p.discard=0
                                                and p.applicationdate>='" + y1 + -+m1 + -+d1 + "'" +
                                               @" and p.applicationdate<='" + y2 + -+m2 + -+d2 + "'" +
                                               @" and ppist.insurancesubtypeid=" + ist.ID;

                    }

                    List<SummaryClass> summaryListFullPremValue = dcdc.ExecuteQuery<SummaryClass>(queryCountAndPremiumFullPremiumValue).ToList();
                    List<SummaryClass> summaryListDef = dcdc.ExecuteQuery<SummaryClass>(queryCountAndPremiumDef).ToList();
                    List<SummaryClass> summaryListInOtherClasses = dcdc.ExecuteQuery<SummaryClass>(queryCountAndPremiumInOtherClasses).ToList();
                    List<SummaryCalcBrokerageClass> summaryListBrok = dcdc.ExecuteQuery<SummaryCalcBrokerageClass>(getCalculatedBrokerages).ToList();
                    List<SummaryCalcBrokerageInOtherClass> summaryListCalcBrokerageInOtherClass = dcdc.ExecuteQuery<SummaryCalcBrokerageInOtherClass>(queryCalcBrokerageInOtherClases).ToList();
                    //List<SummaryCalcRealBrokerageClass> summaryListCalcRealBrokerageClass = dcdc.ExecuteQuery<SummaryCalcRealBrokerageClass>(queryRealPaidValues).ToList();
                    decimal facturedBrokValue = 0;
                    
                    List<SummaryForFacturedBrokerages> summaryForFacturedBrokeragesPayments = dcdc.ExecuteQuery<SummaryForFacturedBrokerages>(queryGetFacturedBrokeragesPayments).ToList();
                    if (summaryForFacturedBrokeragesPayments.Count > 0) {
                        if (summaryForFacturedBrokeragesPayments[0].tmpValueDecimal != null) {
                            facturedBrokValue += (decimal)summaryForFacturedBrokeragesPayments[0].tmpValueDecimal;
                        }
                    }
                    totFacturedBrokerageValuePerClass += facturedBrokValue;

                    foreach (SummaryCalcBrokerageClass scbc in summaryListBrok) {
                        if (scbc.basicvalue != null) {
                            //tmpBrokBaseValue = RateController.RateController.Scale5((scbc.brokeragepercentage / 100) * (scbc.premiumvalue - Convert.ToInt32(scbc.basicvalue)));
                            tmpBrokBaseValue = (scbc.brokeragepercentage / 100) * (scbc.premiumvalue - Convert.ToInt32(scbc.basicvalue));
                            brokValue += tmpBrokBaseValue;
                            tmpBrokBaseValueTot += tmpBrokBaseValue;
                        } else {
                            //tmpBrokBaseValue = RateController.RateController.Scale5((scbc.brokeragepercentage / 100) * scbc.premiumvalue);
                            tmpBrokBaseValue = (scbc.brokeragepercentage / 100) * scbc.premiumvalue;
                            brokValue += tmpBrokBaseValue;
                            tmpBrokBaseValueTot += tmpBrokBaseValue;
                        }
                        //tmpBrokValue = RateController.RateController.Scale5((scbc.brokeragepercentage / 100) * scbc.premiumvalue);
                        tmpBrokValue = (scbc.brokeragepercentage / 100) * scbc.premiumvalue;
                        tmpBrokValueTot += tmpBrokValue;
                    }

                    foreach (SummaryCalcBrokerageInOtherClass scbioc in summaryListCalcBrokerageInOtherClass) {
                        //tmpBrokValueInOtherClass += RateController.RateController.Scale5((scbioc.brokeragepercentage / 100) * Convert.ToDecimal(scbioc.value));
                        tmpBrokValueInOtherClass += (scbioc.brokeragepercentage / 100) * Convert.ToDecimal(scbioc.value);
                        //brokValue += RateController.RateController.Scale5((scbioc.brokeragepercentage / 100) * Convert.ToDecimal(scbioc.value));
                        brokValue += (scbioc.brokeragepercentage / 100) * Convert.ToDecimal(scbioc.value);
                    }

                    numberOfPolicies += summaryListFullPremValue[0].Count + summaryListInOtherClasses[0].Count;
                    totalNumberOfPolicies += summaryListFullPremValue[0].Count;
                    int inCiclusPremValue = 0;
                    if (summaryListFullPremValue[0].SumPremiumValue != null) {
                        inCiclusPremValue += (int)summaryListFullPremValue[0].SumPremiumValue;
                    }
                    if (summaryListDef[0].SumPremiumValue != null) {
                        inCiclusPremValue -= (int)summaryListDef[0].SumPremiumValue;
                    }
                    if (summaryListInOtherClasses[0].SumPremiumValue != null) {
                        inCiclusPremValue += (int)summaryListInOtherClasses[0].SumPremiumValue;
                    }
                    premiumValue += inCiclusPremValue;

                    //if (summaryListCalcRealBrokerageClass[0].sumpaidvalue != null) {
                    //    realBrokValue += (decimal)summaryListCalcRealBrokerageClass[0].sumpaidvalue;
                    //}
                }
                vals[2] = numberOfPolicies;
                vals[3] = String.Format("{0:#,0.00}", RateController.RateController.Scale5(premiumValue));
                totalPremiumValue += premiumValue;
                vals[5] = String.Format("{0:#,0.00}", RateController.RateController.Scale5(brokValue));
                totalBrokValue += brokValue;
                //vals[4] = String.Format("{0:#,0.00}", RateController.RateController.Scale5(realBrokValue));
                //vals[4] = String.Format("{0:#,0.00}", RateController.RateController.Scale5(totFacturedValueInClass));
                vals[4] = String.Format("{0:#,0.00}", RateController.RateController.Scale5(totFacturedBrokerageValuePerClass));
                //totalRealBrokValue += realBrokValue;
                //totalRealBrokValue += totFacturedValueInClass;
                totalRealBrokValue += totFacturedBrokerageValuePerClass;
                pdf.AddDataRowWithBorder(vals, headers.Count(), codes);
            }
            object[] valsTotal = new object[headers.Count()];
            valsTotal[0] = "ВКУПНО";
            valsTotal[1] = "";
            valsTotal[2] = totalNumberOfPolicies;
            valsTotal[3] = String.Format("{0:#,0.00}", RateController.RateController.Scale5(totalPremiumValue));
            valsTotal[5] = String.Format("{0:#,0.00}", RateController.RateController.Scale5(totalBrokValue));
            valsTotal[4] = String.Format("{0:#,0.00}", RateController.RateController.Scale5(totalRealBrokValue));
            pdf.AddDataRowWithBorder(valsTotal, headers.Count(), codes);
            pdf.AddTable();
            
            pdf.FinishPDF();

        }
    }

    public class SummaryClass {
        public int Count { get; set; }
        public int? SumPremiumValue { get; set; }
    }

    public class SummaryCalcBrokerageClass {
        public int id { get; set; }
        public int insurancesubtypeid { get; set; }
        public decimal premiumvalue { get; set; }
        public int? basicvalue { get; set; }
        public decimal brokeragepercentage { get; set; }
    }

    public class SummaryCalcBrokerageInOtherClass {
        public int id { get; set; }
        public string value { get; set; }
        public decimal brokeragepercentage { get; set; }
    }

    public class SummaryCalcRealBrokerageClass {
        public decimal? sumpaidvalue { get; set; }
        public decimal? sumtotalvalue { get; set; }
    }

    public class SummaryForPI {
        public int? tmpValueInt { get; set; }
    }

    public class SummaryForFacturedBrokerages {
        public decimal? tmpValueDecimal { get; set; }
    }
}
