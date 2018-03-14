using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Broker.DataAccess;
using System.Reflection;

/// <summary>
/// Summary description for ReportFinPolicies
/// </summary>
/// 
namespace Broker.Controllers.ReportControllers {
    public class ReportFinPolicies {
        public static void PrintReportFinPolicies(DateTime fromDate, DateTime toDate, string separatePeriodType, string groupType, List<int> lstBranhces,
            List<int> lstInsuranceCompanies, List<int> lstInsuranceSubTypes, List<int> lstUsers, List<int> lstMarketingAgents,
            Dictionary<string, string> reportPainColumns, int totalType) {
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
            pdf.SetTitle("Извештај");
            pdf.SetTitle("За период од " + fromDate.ToShortDateString() + " до " + toDate.ToShortDateString());
            pdf.SetTitleLeft10("Датум и време на печатење: " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString());
            if (groupType == "one") {
                string query = @"select p.id, pi.policynumber, p.applicationdate, ist.shortdescription as insurancesubtypename, ic.shortname as insurancecompanyname, c.name as clientname,
                            pi.realpremiumvalue, pi.premiumvalue, pi.discountvalue,
                            isnull((select sum(pay.value) from policies p1, policyitems pi1, rates r, payments pay
                            where pi1.policyid=p1.id and r.policyitemid=pi1.id
                            and pay.rateid=r.id
                            and p1.id=p.id),0) as paidvalue,
                            isnull(pi.premiumvalue - (select sum(pay.value) from policies p1, policyitems pi1, rates r, payments pay
                            where pi1.policyid=p1.id and r.policyitemid=pi1.id
                            and pay.rateid=r.id
                            and p1.id=p.id),0) as debtvalue,
                            (select sum(r.value)- sum(r.paidvalue) from policies p1, policyitems pi1, rates r
                            where pi1.policyid=p1.id and r.policyitemid=pi1.id
                            and p1.id=p.id and r.date>='" + y1 + -+m1 + -+d1 + "'" +
                            @" and r.date<='" + y2 + -+m2 + -+d2 + "'" +
                            @" ) as debtexpectedvalue," +
                            @" case when pi.premiumvalue>0 then (select isnull((select sum(pay.value) from policies p1, policyitems pi1, rates r, payments pay
                            where pi1.policyid=p1.id and r.policyitemid=pi1.id
                            and pay.rateid=r.id
                            and p1.id=p.id),0) * 100)/pi.premiumvalue else 0 end as paidvaluepercent,
                            isnull((select sum(pay.value) from policies p1, policyitems pi1, rates r, payments pay
                            where pi1.policyid=p1.id and r.policyitemid=pi1.id
                            and pay.rateid=r.id
                            and p1.id=p.id
                            and pay.isfactured=1),0) as facturedbrokeragevalue,
                            case when pi.marketingagentbrokerageid is not null
                            then case when c.islaw=1 then (select (percentageforlaws / 100) * pi.premiumvalue from BrokeragesForMarketingAgents
                            where id = pi.marketingagentbrokerageid) else (select (percentageforprivates / 100) * pi.premiumvalue 
                            from BrokeragesForMarketingAgents
                            where id = pi.marketingagentbrokerageid) end else 0 end as marketingagentvalue
                            from policies p, policyitems pi, clients c, insurancecompanies ic, insurancesubtypes ist
                            where pi.policyid=p.id
                            and p.clientid=c.id
                            and p.insurancecompanyid=ic.id
                            and pi.insurancesubtypeid=ist.id
                            and p.branchid in (" + GetFromIDs(lstBranhces) + ") " +
                            @" and p.userid in (" + GetFromIDs(lstUsers) + ") " +
                            @" and p.insurancecompanyid in (" + GetFromIDs(lstInsuranceCompanies) + ") " +
                            @" and pi.insurancesubtypeid in (" + GetFromIDs(lstInsuranceSubTypes) + ") " +
                            @" and p.applicationdate>='" + y1 + -+m1 + -+d1 + "'" +
                            @" and p.applicationdate<='" + y2 + -+m2 + -+d2 + "'" +
                            @" and p.discard = 0 " +
                            @" order by p.applicationdate";
                //@" --query so marketing agent";
                IEnumerable<SummaryForPoedinecno> lst = dcdc.ExecuteQuery<SummaryForPoedinecno>(query);



                string[] headers = new string[6 + reportPainColumns.Count];
                headers[0] = "Р.бр.";
                headers[1] = "Полиса";
                headers[2] = "Датум";
                headers[3] = "Подкласа";
                headers[4] = "О.компанија";
                headers[5] = "Договорувач";
                int i = 0;
                foreach (KeyValuePair<string, string> kvp in reportPainColumns) {
                    headers[6 + i] = kvp.Value;
                    i++;
                }

                TypeCode[] codes = new TypeCode[6 + reportPainColumns.Count];

                codes[0] = TypeCode.Int32;
                codes[1] = TypeCode.String;
                codes[2] = TypeCode.DateTime;
                codes[3] = TypeCode.String;
                codes[4] = TypeCode.String;
                codes[5] = TypeCode.String;
                int j = 0;
                foreach (KeyValuePair<string, string> kvp in reportPainColumns) {
                    codes[6 + j] = TypeCode.Decimal;
                    j++;
                }


                float[] policyColumnsWidths = new float[headers.Count()];
                policyColumnsWidths[0] = 5;
                policyColumnsWidths[1] = 8;
                policyColumnsWidths[2] = 8;
                policyColumnsWidths[3] = 15;
                policyColumnsWidths[4] = 9;
                policyColumnsWidths[5] = 20;
                float widthPerItem = (float)35;
                if (reportPainColumns.Count > 0) {
                    widthPerItem = (float)35 / (float)reportPainColumns.Count;
                }
                int k = 0;
                foreach (KeyValuePair<string, string> kvp in reportPainColumns) {
                    policyColumnsWidths[6 + k] = widthPerItem;
                    k++;
                }
                pdf.CreateTableWithBorder(headers.Count(), false, headers, "", policyColumnsWidths);
                int counter = 1;
                Dictionary<string, decimal> totals = new Dictionary<string, decimal>();
                foreach (SummaryForPoedinecno pol in lst) {
                    object[] vals = new object[headers.Count()];
                    vals[0] = counter;
                    vals[1] = pol.policynumber;
                    vals[2] = pol.applicationdate.ToShortDateString();
                    vals[3] = pol.insurancesubtypename;
                    vals[4] = pol.insurancecompanyname;
                    vals[5] = pol.clientname;
                    int currCounter = 0;
                    foreach (KeyValuePair<string, string> kvp in reportPainColumns) {
                        if (kvp.Key == "RealPolicyValue") {
                            vals[6 + currCounter] = String.Format("{0:#,0.00}", pol.realpremiumvalue);
                            if (totals.Keys.Contains("RealPolicyValue")) {
                                totals["RealPolicyValue"] += pol.realpremiumvalue;
                            } else {
                                totals.Add("RealPolicyValue", pol.realpremiumvalue);
                            }
                        }
                        if (kvp.Key == "PremiumValue") {
                            vals[6 + currCounter] = String.Format("{0:#,0.00}", pol.premiumvalue);
                            if (totals.Keys.Contains("PremiumValue")) {
                                totals["PremiumValue"] += pol.premiumvalue;
                            } else {
                                totals.Add("PremiumValue", pol.premiumvalue);
                            }
                        }
                        if (kvp.Key == "PaidPremiumValue") {
                            vals[6 + currCounter] = String.Format("{0:#,0.00}", pol.paidvalue);
                            if (totals.Keys.Contains("PaidPremiumValue")) {
                                totals["PaidPremiumValue"] += pol.paidvalue;
                            } else {
                                totals.Add("PaidPremiumValue", pol.paidvalue);
                            }
                        }
                        if (kvp.Key == "PaidPremiumValuePercent") {
                            vals[6 + currCounter] = String.Format("{0:#,0.00}", pol.paidvaluepercent);
                            if (totals.Keys.Contains("PaidPremiumValuePercent")) {
                                totals["PaidPremiumValuePercent"] += pol.paidvaluepercent;
                            } else {
                                totals.Add("PaidPremiumValuePercent", pol.paidvaluepercent);
                            }
                        }
                        if (kvp.Key == "DebtPremiumValue") {
                            vals[6 + currCounter] = String.Format("{0:#,0.00}", pol.debtvalue);
                            if (totals.Keys.Contains("DebtPremiumValue")) {
                                totals["DebtPremiumValue"] += pol.debtvalue;
                            } else {
                                totals.Add("DebtPremiumValue", pol.debtvalue);
                            }
                        }
                        if (kvp.Key == "DebtExpectedPremiumValue") {
                            vals[6 + currCounter] = String.Format("{0:#,0.00}", pol.debtexpectedvalue);
                            if (totals.Keys.Contains("DebtExpectedPremiumValue")) {
                                totals["DebtExpectedPremiumValue"] += pol.debtexpectedvalue;
                            } else {
                                totals.Add("DebtExpectedPremiumValue", pol.debtexpectedvalue);
                            }
                        }
                        if (kvp.Key == "FacturedBrokerageValue") {
                            vals[6 + currCounter] = String.Format("{0:#,0.00}", pol.facturedbrokeragevalue);
                            if (totals.Keys.Contains("FacturedBrokerageValue")) {
                                totals["FacturedBrokerageValue"] += pol.facturedbrokeragevalue;
                            } else {
                                totals.Add("FacturedBrokerageValue", pol.facturedbrokeragevalue);
                            }
                        }
                        if (kvp.Key == "MarkAgentProvisionValue") {
                            vals[6 + currCounter] = String.Format("{0:#,0.00}", pol.marketingagentvalue);
                            if (totals.Keys.Contains("MarkAgentProvisionValue")) {
                                totals["MarkAgentProvisionValue"] += pol.marketingagentvalue;
                            } else {
                                totals.Add("MarkAgentProvisionValue", pol.marketingagentvalue);
                            }
                        }
                        currCounter++;
                    }
                    pdf.AddDataRowWithBorder(vals, headers.Count(), codes);
                    counter++;
                }
                object[] valsTot = new object[headers.Count()];
                valsTot[0] = "";
                valsTot[1] = "";
                valsTot[2] = "";
                valsTot[3] = "";
                valsTot[4] = "";
                valsTot[5] = "Вкупно";
                int currTotalCounter = 0;
                foreach (KeyValuePair<string, string> kvp in reportPainColumns) {
                    if (kvp.Key == "RealPolicyValue") {
                        valsTot[6 + currTotalCounter] = String.Format("{0:#,0.00}", totals["RealPolicyValue"]);
                    }
                    if (kvp.Key == "PremiumValue") {
                        valsTot[6 + currTotalCounter] = String.Format("{0:#,0.00}", totals["PremiumValue"]);
                    }
                    if (kvp.Key == "PaidPremiumValue") {
                        valsTot[6 + currTotalCounter] = String.Format("{0:#,0.00}", totals["PaidPremiumValue"]);
                    }
                    if (kvp.Key == "PaidPremiumValuePercent") {
                        valsTot[6 + currTotalCounter] = "";
                    }
                    if (kvp.Key == "DebtPremiumValue") {
                        valsTot[6 + currTotalCounter] = String.Format("{0:#,0.00}", totals["DebtPremiumValue"]);
                    }
                    if (kvp.Key == "DebtExpectedPremiumValue") {
                        valsTot[6 + currTotalCounter] = String.Format("{0:#,0.00}", totals["DebtExpectedPremiumValue"]);
                    }
                    if (kvp.Key == "FacturedBrokerageValue") {
                        valsTot[6 + currTotalCounter] = String.Format("{0:#,0.00}", totals["DebtExpectedPremiumValue"]);
                    }
                    if (kvp.Key == "MarkAgentProvisionValue") {
                        valsTot[6 + currTotalCounter] = String.Format("{0:#,0.00}", totals["DebtExpectedPremiumValue"]);
                    }
                    currTotalCounter++;
                }
                pdf.AddDataRowWithBorder(valsTot, headers.Count(), codes);
                pdf.AddTable();
                pdf.FinishPDF();
            }
            if (groupType == "total") {
                string query = @"select p.id, pi.insurancesubtypeid, p.applicationdate, pi.insurancesubtypeid, p.insurancecompanyid, p.userid, p.branchid,
                            pi.realpremiumvalue, pi.premiumvalue, pi.discountvalue,
                            isnull((select sum(pay.value) from policies p1, policyitems pi1, rates r, payments pay
                            where pi1.policyid=p1.id and r.policyitemid=pi1.id
                            and pay.rateid=r.id
                            and p1.id=p.id),0) as paidvalue,
                            isnull(pi.premiumvalue - (select sum(pay.value) from policies p1, policyitems pi1, rates r, payments pay
                            where pi1.policyid=p1.id and r.policyitemid=pi1.id
                            and pay.rateid=r.id
                            and p1.id=p.id),0) as debtvalue,
                            (select sum(r.value)- sum(r.paidvalue) from policies p1, policyitems pi1, rates r
                            where pi1.policyid=p1.id and r.policyitemid=pi1.id
                            and p1.id=p.id and r.date>='" + y1 + -+m1 + -+d1 + "'" +
                            @" and r.date<='" + y2 + -+m2 + -+d2 + "'" +
                            @" ) as debtexpectedvalue," +
                            @" isnull((select sum(pay.value) from policies p1, policyitems pi1, rates r, payments pay
                            where pi1.policyid=p1.id and r.policyitemid=pi1.id
                            and pay.rateid=r.id
                            and p1.id=p.id
                            and pay.isfactured=1),0) as facturedbrokeragevalue,
                            case when pi.marketingagentbrokerageid is not null
                            then case when c.islaw=1 then (select (percentageforlaws / 100) * pi.premiumvalue from BrokeragesForMarketingAgents
                            where id = pi.marketingagentbrokerageid) else (select (percentageforprivates / 100) * pi.premiumvalue 
                            from BrokeragesForMarketingAgents
                            where id = pi.marketingagentbrokerageid) end else 0 end as marketingagentvalue
                            from policies p, policyitems pi, clients c
                            where pi.policyid=p.id and p.clientid=c.id ";

                if (lstBranhces.Count > 0) {
                    query += @" and p.branchid in (" + GetFromIDs(lstBranhces) + ") ";
                }
                if (lstUsers.Count > 0) {
                    query += @" and p.userid in (" + GetFromIDs(lstUsers) + ") ";
                }
                if (lstInsuranceCompanies.Count > 0) {
                    query += @" and p.insurancecompanyid in (" + GetFromIDs(lstInsuranceCompanies) + ") ";
                }
                if (lstInsuranceSubTypes.Count > 0) {
                    query += @" and pi.insurancesubtypeid in (" + GetFromIDs(lstInsuranceSubTypes) + ") ";
                }
                query += @" and p.applicationdate>='" + y1 + -+m1 + -+d1 + "'" +
                           @" and p.applicationdate<='" + y2 + -+m2 + -+d2 + "'" +
                           @" and p.discard = 0 ";
                List<SummaryForGroup> list = dcdc.ExecuteQuery<SummaryForGroup>(query).ToList();

                int counter = 1;
                if (lstBranhces.Count > 0 && lstInsuranceCompanies.Count > 0 && lstInsuranceSubTypes.Count > 0 && lstUsers.Count > 0) {
                    string[] headers = new string[6 + reportPainColumns.Count];
                    headers[0] = "Р.бр.";
                    if (totalType == 0) {
                        headers[1] = "Датум";
                    }
                    if (totalType == 1) {
                        headers[1] = "Месец";
                    }
                    if (totalType == 2) {
                        headers[1] = "Квартал";
                    }
                    headers[2] = "О.компанија";
                    headers[3] = "Филијала";
                    headers[4] = "Подкласа";
                    headers[5] = "Корисник";
                    int i = 0;
                    foreach (KeyValuePair<string, string> kvp in reportPainColumns) {
                        headers[6 + i] = kvp.Value;
                        i++;
                    }
                    TypeCode[] codes = new TypeCode[6 + reportPainColumns.Count];

                    codes[0] = TypeCode.Int32;
                    codes[1] = TypeCode.String;
                    codes[2] = TypeCode.String;
                    codes[3] = TypeCode.String;
                    codes[4] = TypeCode.String;
                    codes[5] = TypeCode.String;
                    int j = 0;
                    foreach (KeyValuePair<string, string> kvp in reportPainColumns) {
                        codes[6 + j] = TypeCode.Decimal;
                        j++;
                    }


                    float[] policyColumnsWidths = new float[headers.Count()];
                    policyColumnsWidths[0] = 5;
                    policyColumnsWidths[1] = 10;
                    policyColumnsWidths[2] = 10;
                    policyColumnsWidths[3] = 10;
                    policyColumnsWidths[4] = 15;
                    policyColumnsWidths[5] = 10;
                    float widthPerItem = (float)40;
                    if (reportPainColumns.Count > 0) {
                        widthPerItem = (float)40 / (float)reportPainColumns.Count;
                    }
                    int k = 0;
                    foreach (KeyValuePair<string, string> kvp in reportPainColumns) {
                        policyColumnsWidths[5 + k] = widthPerItem;
                        k++;
                    }
                    pdf.CreateTableWithBorder(headers.Count(), false, headers, "", policyColumnsWidths);
                    if (totalType == 0) {
                        var queryGroupBy = (from t in list
                                            group t by new { t.applicationdate, t.branchid, t.insurancecompanyid, t.insurancesubtypeid, t.userid }
                                                into grp
                                                select new {
                                                    grp.Key.applicationdate,
                                                    grp.Key.branchid,
                                                    grp.Key.insurancecompanyid,
                                                    grp.Key.insurancesubtypeid,
                                                    grp.Key.userid,
                                                    premiumvalue = grp.Sum(t => t.premiumvalue),
                                                    realpremiumvalue = grp.Sum(t => t.realpremiumvalue),
                                                    discountvalue = grp.Sum(t => t.discountvalue),
                                                    paidvalue = grp.Sum(t => t.paidvalue),
                                                    debtvalue = grp.Sum(t => t.debtvalue),
                                                    debtexpectedvalue = grp.Sum(t => t.debtexpectedvalue),
                                                    facturedbrokeragevalue = grp.Sum(c => c.facturedbrokeragevalue),
                                                    marketingagentvalue = grp.Sum(c => c.marketingagentvalue)
                                                }).ToList();
                        foreach (var c in queryGroupBy) {
                            object[] values = new object[5 + reportPainColumns.Count];
                            values[0] = counter;
                            values[1] = c.applicationdate.ToShortDateString();
                            values[2] = InsuranceCompany.Get(c.insurancecompanyid).ShortName;
                            values[3] = Broker.DataAccess.Branch.Get(c.branchid).Name;
                            values[4] = InsuranceSubType.Get(c.insurancesubtypeid).ShortDescription;
                            values[5] = Broker.DataAccess.User.Get(c.userid).Name;
                            int currCounter = 0;
                            foreach (KeyValuePair<string, string> kvp in reportPainColumns) {
                                if (kvp.Key == "RealPolicyValue") {
                                    values[6 + currCounter] = String.Format("{0:#,0.00}", c.realpremiumvalue);
                                }
                                if (kvp.Key == "PremiumValue") {
                                    values[6 + currCounter] = String.Format("{0:#,0.00}", c.premiumvalue);
                                }
                                if (kvp.Key == "PaidPremiumValue") {
                                    values[6 + currCounter] = String.Format("{0:#,0.00}", c.paidvalue);
                                }
                                if (kvp.Key == "DebtPremiumValue") {
                                    values[6 + currCounter] = String.Format("{0:#,0.00}", c.debtvalue);
                                }
                                if (kvp.Key == "DebtExpectedPremiumValue") {
                                    values[6 + currCounter] = String.Format("{0:#,0.00}", c.debtexpectedvalue);
                                }
                                if (kvp.Key == "FacturedBrokerageValue") {
                                    values[6 + currCounter] = String.Format("{0:#,0.00}", c.facturedbrokeragevalue);
                                }
                                if (kvp.Key == "MarkAgentProvisionValue") {
                                    values[6 + currCounter] = String.Format("{0:#,0.00}", c.marketingagentvalue);
                                }
                                currCounter++;
                            }
                            pdf.AddDataRowWithBorder(values, headers.Count(), codes);
                            counter++;
                        }
                    }
                    if (totalType == 1) {
                        var queryGroupBy = (from t in list
                                            group t by new { t.applicationdate.Year, t.applicationdate.Month, t.branchid, t.insurancecompanyid, t.insurancesubtypeid, t.userid }
                                                into grp
                                                select new {
                                                    grp.Key.Year,
                                                    grp.Key.Month,
                                                    grp.Key.branchid,
                                                    grp.Key.insurancecompanyid,
                                                    grp.Key.insurancesubtypeid,
                                                    grp.Key.userid,
                                                    premiumvalue = grp.Sum(t => t.premiumvalue),
                                                    realpremiumvalue = grp.Sum(t => t.realpremiumvalue),
                                                    discountvalue = grp.Sum(t => t.discountvalue),
                                                    paidvalue = grp.Sum(t => t.paidvalue),
                                                    debtvalue = grp.Sum(t => t.debtvalue),
                                                    debtexpectedvalue = grp.Sum(t => t.debtexpectedvalue),
                                                    facturedbrokeragevalue = grp.Sum(c => c.facturedbrokeragevalue),
                                                    marketingagentvalue = grp.Sum(c => c.marketingagentvalue)
                                                }).ToList();
                        foreach (var c in queryGroupBy) {
                            object[] values = new object[6 + reportPainColumns.Count];
                            values[0] = counter;
                            values[1] = c.Month + "." + c.Year;
                            values[2] = InsuranceCompany.Get(c.insurancecompanyid).ShortName;
                            values[3] = Broker.DataAccess.Branch.Get(c.branchid).Name;
                            values[4] = InsuranceSubType.Get(c.insurancesubtypeid).ShortDescription;
                            values[5] = Broker.DataAccess.User.Get(c.userid).Name;
                            int currCounter = 0;
                            foreach (KeyValuePair<string, string> kvp in reportPainColumns) {
                                if (kvp.Key == "RealPolicyValue") {
                                    values[6 + currCounter] = String.Format("{0:#,0.00}", c.realpremiumvalue);
                                }
                                if (kvp.Key == "PremiumValue") {
                                    values[6 + currCounter] = String.Format("{0:#,0.00}", c.premiumvalue);
                                }
                                if (kvp.Key == "PaidPremiumValue") {
                                    values[6 + currCounter] = String.Format("{0:#,0.00}", c.paidvalue);
                                }
                                if (kvp.Key == "DebtPremiumValue") {
                                    values[6 + currCounter] = String.Format("{0:#,0.00}", c.debtvalue);
                                }
                                if (kvp.Key == "DebtExpectedPremiumValue") {
                                    values[6 + currCounter] = String.Format("{0:#,0.00}", c.debtexpectedvalue);
                                }
                                if (kvp.Key == "FacturedBrokerageValue") {
                                    values[6 + currCounter] = String.Format("{0:#,0.00}", c.facturedbrokeragevalue);
                                }
                                if (kvp.Key == "MarkAgentProvisionValue") {
                                    values[6 + currCounter] = String.Format("{0:#,0.00}", c.marketingagentvalue);
                                }
                                currCounter++;
                            }
                            pdf.AddDataRowWithBorder(values, headers.Count(), codes);
                            counter++;
                        }
                    }
                    if (totalType == 2) {
                        var queryGroupBy = (from t in list
                                            group t by new { t.applicationdate.Year, Quarter = (t.applicationdate.Month - 1) / 3 + 1, t.branchid, t.insurancecompanyid, t.insurancesubtypeid, t.userid }
                                                into grp
                                                select new {
                                                    grp.Key.Year,
                                                    grp.Key.Quarter,
                                                    grp.Key.branchid,
                                                    grp.Key.insurancecompanyid,
                                                    grp.Key.insurancesubtypeid,
                                                    grp.Key.userid,
                                                    premiumvalue = grp.Sum(t => t.premiumvalue),
                                                    realpremiumvalue = grp.Sum(t => t.realpremiumvalue),
                                                    discountvalue = grp.Sum(t => t.discountvalue),
                                                    paidvalue = grp.Sum(t => t.paidvalue),
                                                    debtvalue = grp.Sum(t => t.debtvalue),
                                                    debtexpectedvalue = grp.Sum(t => t.debtexpectedvalue),
                                                    facturedbrokeragevalue = grp.Sum(c => c.facturedbrokeragevalue),
                                                    marketingagentvalue = grp.Sum(c => c.marketingagentvalue)
                                                }).ToList();
                        foreach (var c in queryGroupBy) {
                            object[] values = new object[6 + reportPainColumns.Count];
                            values[0] = counter;
                            values[1] = c.Quarter + " квартал " + c.Year;
                            values[2] = InsuranceCompany.Get(c.insurancecompanyid).ShortName;
                            values[3] = Broker.DataAccess.Branch.Get(c.branchid).Name;
                            values[4] = InsuranceSubType.Get(c.insurancesubtypeid).ShortDescription;
                            values[5] = Broker.DataAccess.User.Get(c.userid).Name;
                            int currCounter = 0;
                            foreach (KeyValuePair<string, string> kvp in reportPainColumns) {
                                if (kvp.Key == "RealPolicyValue") {
                                    values[6 + currCounter] = String.Format("{0:#,0.00}", c.realpremiumvalue);
                                }
                                if (kvp.Key == "PremiumValue") {
                                    values[6 + currCounter] = String.Format("{0:#,0.00}", c.premiumvalue);
                                }
                                if (kvp.Key == "PaidPremiumValue") {
                                    values[6 + currCounter] = String.Format("{0:#,0.00}", c.paidvalue);
                                }
                                if (kvp.Key == "DebtPremiumValue") {
                                    values[6 + currCounter] = String.Format("{0:#,0.00}", c.debtvalue);
                                }
                                if (kvp.Key == "DebtExpectedPremiumValue") {
                                    values[6 + currCounter] = String.Format("{0:#,0.00}", c.debtexpectedvalue);
                                }
                                if (kvp.Key == "FacturedBrokerageValue") {
                                    values[6 + currCounter] = String.Format("{0:#,0.00}", c.facturedbrokeragevalue);
                                }
                                if (kvp.Key == "MarkAgentProvisionValue") {
                                    values[6 + currCounter] = String.Format("{0:#,0.00}", c.marketingagentvalue);
                                }
                                currCounter++;
                            }
                            pdf.AddDataRowWithBorder(values, headers.Count(), codes);
                            counter++;
                        }
                    }
                }
                if (lstBranhces.Count > 0 && lstInsuranceCompanies.Count > 0 && lstInsuranceSubTypes.Count > 0 && lstUsers.Count == 0) {
                    string[] headers = new string[5 + reportPainColumns.Count];
                    headers[0] = "Р.бр.";
                    if (totalType == 0) {
                        headers[1] = "Датум";
                    }
                    if (totalType == 1) {
                        headers[1] = "Месец";
                    }
                    if (totalType == 2) {
                        headers[1] = "Квартал";
                    }
                    headers[2] = "О.компанија";
                    headers[3] = "Филијала";
                    headers[4] = "Подкласа";
                    int i = 0;
                    foreach (KeyValuePair<string, string> kvp in reportPainColumns) {
                        headers[5 + i] = kvp.Value;
                        i++;
                    }
                    TypeCode[] codes = new TypeCode[5 + reportPainColumns.Count];

                    codes[0] = TypeCode.Int32;
                    codes[1] = TypeCode.String;
                    codes[2] = TypeCode.String;
                    codes[3] = TypeCode.String;
                    codes[4] = TypeCode.String;
                    int j = 0;
                    foreach (KeyValuePair<string, string> kvp in reportPainColumns) {
                        codes[5 + j] = TypeCode.Decimal;
                        j++;
                    }


                    float[] policyColumnsWidths = new float[headers.Count()];
                    policyColumnsWidths[0] = 5;
                    policyColumnsWidths[1] = 10;
                    policyColumnsWidths[2] = 10;
                    policyColumnsWidths[3] = 15;
                    policyColumnsWidths[4] = 15;
                    float widthPerItem = (float)45;
                    if (reportPainColumns.Count > 0) {
                        widthPerItem = (float)45 / (float)reportPainColumns.Count;
                    }
                    int k = 0;
                    foreach (KeyValuePair<string, string> kvp in reportPainColumns) {
                        policyColumnsWidths[5 + k] = widthPerItem;
                        k++;
                    }
                    pdf.CreateTableWithBorder(headers.Count(), false, headers, "", policyColumnsWidths);
                    if (totalType == 0) {
                        var queryGroupBy = (from t in list
                                            group t by new { t.applicationdate, t.branchid, t.insurancecompanyid, t.insurancesubtypeid }
                                                into grp
                                                select new {
                                                    grp.Key.applicationdate,
                                                    grp.Key.branchid,
                                                    grp.Key.insurancecompanyid,
                                                    grp.Key.insurancesubtypeid,
                                                    premiumvalue = grp.Sum(t => t.premiumvalue),
                                                    realpremiumvalue = grp.Sum(t => t.realpremiumvalue),
                                                    discountvalue = grp.Sum(t => t.discountvalue),
                                                    paidvalue = grp.Sum(t => t.paidvalue),
                                                    debtvalue = grp.Sum(t => t.debtvalue),
                                                    debtexpectedvalue = grp.Sum(t => t.debtexpectedvalue),
                                                    facturedbrokeragevalue = grp.Sum(c => c.facturedbrokeragevalue),
                                                    marketingagentvalue = grp.Sum(c => c.marketingagentvalue)
                                                }).ToList();
                        foreach (var c in queryGroupBy) {
                            object[] values = new object[5 + reportPainColumns.Count];
                            values[0] = counter;
                            values[1] = c.applicationdate.ToShortDateString();
                            values[2] = InsuranceCompany.Get(c.insurancecompanyid).ShortName;
                            values[3] = Broker.DataAccess.Branch.Get(c.branchid).Name;
                            values[4] = InsuranceSubType.Get(c.insurancesubtypeid).ShortDescription;
                            int currCounter = 0;
                            foreach (KeyValuePair<string, string> kvp in reportPainColumns) {
                                if (kvp.Key == "RealPolicyValue") {
                                    values[5 + currCounter] = String.Format("{0:#,0.00}", c.realpremiumvalue);
                                }
                                if (kvp.Key == "PremiumValue") {
                                    values[5 + currCounter] = String.Format("{0:#,0.00}", c.premiumvalue);
                                }
                                if (kvp.Key == "PaidPremiumValue") {
                                    values[5 + currCounter] = String.Format("{0:#,0.00}", c.paidvalue);
                                }
                                if (kvp.Key == "DebtPremiumValue") {
                                    values[5 + currCounter] = String.Format("{0:#,0.00}", c.debtvalue);
                                }
                                if (kvp.Key == "DebtExpectedPremiumValue") {
                                    values[5 + currCounter] = String.Format("{0:#,0.00}", c.debtexpectedvalue);
                                }
                                if (kvp.Key == "FacturedBrokerageValue") {
                                    values[5 + currCounter] = String.Format("{0:#,0.00}", c.facturedbrokeragevalue);
                                }
                                if (kvp.Key == "MarkAgentProvisionValue") {
                                    values[5 + currCounter] = String.Format("{0:#,0.00}", c.marketingagentvalue);
                                }
                                currCounter++;
                            }
                            pdf.AddDataRowWithBorder(values, headers.Count(), codes);
                            counter++;
                        }
                    }
                    if (totalType == 1) {
                        var queryGroupBy = (from t in list
                                            group t by new { t.applicationdate.Year, t.applicationdate.Month, t.branchid, t.insurancecompanyid, t.insurancesubtypeid }
                                                into grp
                                                select new {
                                                    grp.Key.Year,
                                                    grp.Key.Month,
                                                    grp.Key.branchid,
                                                    grp.Key.insurancecompanyid,
                                                    grp.Key.insurancesubtypeid,
                                                    premiumvalue = grp.Sum(t => t.premiumvalue),
                                                    realpremiumvalue = grp.Sum(t => t.realpremiumvalue),
                                                    discountvalue = grp.Sum(t => t.discountvalue),
                                                    paidvalue = grp.Sum(t => t.paidvalue),
                                                    debtvalue = grp.Sum(t => t.debtvalue),
                                                    debtexpectedvalue = grp.Sum(t => t.debtexpectedvalue),
                                                    facturedbrokeragevalue = grp.Sum(c => c.facturedbrokeragevalue),
                                                    marketingagentvalue = grp.Sum(c => c.marketingagentvalue)
                                                }).ToList();
                        foreach (var c in queryGroupBy) {
                            object[] values = new object[5 + reportPainColumns.Count];
                            values[0] = counter;
                            values[1] = c.Month + "." + c.Year;
                            values[2] = InsuranceCompany.Get(c.insurancecompanyid).ShortName;
                            values[3] = Broker.DataAccess.Branch.Get(c.branchid).Name;
                            values[4] = InsuranceSubType.Get(c.insurancesubtypeid).ShortDescription;
                            int currCounter = 0;
                            foreach (KeyValuePair<string, string> kvp in reportPainColumns) {
                                if (kvp.Key == "RealPolicyValue") {
                                    values[5 + currCounter] = String.Format("{0:#,0.00}", c.realpremiumvalue);
                                }
                                if (kvp.Key == "PremiumValue") {
                                    values[5 + currCounter] = String.Format("{0:#,0.00}", c.premiumvalue);
                                }
                                if (kvp.Key == "PaidPremiumValue") {
                                    values[5 + currCounter] = String.Format("{0:#,0.00}", c.paidvalue);
                                }
                                if (kvp.Key == "DebtPremiumValue") {
                                    values[5 + currCounter] = String.Format("{0:#,0.00}", c.debtvalue);
                                }
                                if (kvp.Key == "DebtExpectedPremiumValue") {
                                    values[5 + currCounter] = String.Format("{0:#,0.00}", c.debtexpectedvalue);
                                }
                                if (kvp.Key == "FacturedBrokerageValue") {
                                    values[5 + currCounter] = String.Format("{0:#,0.00}", c.facturedbrokeragevalue);
                                }
                                if (kvp.Key == "MarkAgentProvisionValue") {
                                    values[5 + currCounter] = String.Format("{0:#,0.00}", c.marketingagentvalue);
                                }
                                currCounter++;
                            }
                            pdf.AddDataRowWithBorder(values, headers.Count(), codes);
                            counter++;
                        }
                    }
                    if (totalType == 2) {
                        var queryGroupBy = (from t in list
                                            group t by new { t.applicationdate.Year, Quarter = (t.applicationdate.Month - 1) / 3 + 1, t.branchid, t.insurancecompanyid, t.insurancesubtypeid }
                                                into grp
                                                select new {
                                                    grp.Key.Year,
                                                    grp.Key.Quarter,
                                                    grp.Key.branchid,
                                                    grp.Key.insurancecompanyid,
                                                    grp.Key.insurancesubtypeid,
                                                    premiumvalue = grp.Sum(t => t.premiumvalue),
                                                    realpremiumvalue = grp.Sum(t => t.realpremiumvalue),
                                                    discountvalue = grp.Sum(t => t.discountvalue),
                                                    paidvalue = grp.Sum(t => t.paidvalue),
                                                    debtvalue = grp.Sum(t => t.debtvalue),
                                                    debtexpectedvalue = grp.Sum(t => t.debtexpectedvalue),
                                                    facturedbrokeragevalue = grp.Sum(c => c.facturedbrokeragevalue),
                                                    marketingagentvalue = grp.Sum(c => c.marketingagentvalue)
                                                }).ToList();
                        foreach (var c in queryGroupBy) {
                            object[] values = new object[5 + reportPainColumns.Count];
                            values[0] = counter;
                            values[1] = c.Quarter + " квартал " + c.Year;
                            values[2] = InsuranceCompany.Get(c.insurancecompanyid).ShortName;
                            values[3] = Broker.DataAccess.Branch.Get(c.branchid).Name;
                            values[4] = InsuranceSubType.Get(c.insurancesubtypeid).ShortDescription;
                            int currCounter = 0;
                            foreach (KeyValuePair<string, string> kvp in reportPainColumns) {
                                if (kvp.Key == "RealPolicyValue") {
                                    values[5 + currCounter] = String.Format("{0:#,0.00}", c.realpremiumvalue);
                                }
                                if (kvp.Key == "PremiumValue") {
                                    values[5 + currCounter] = String.Format("{0:#,0.00}", c.premiumvalue);
                                }
                                if (kvp.Key == "PaidPremiumValue") {
                                    values[5 + currCounter] = String.Format("{0:#,0.00}", c.paidvalue);
                                }
                                if (kvp.Key == "DebtPremiumValue") {
                                    values[5 + currCounter] = String.Format("{0:#,0.00}", c.debtvalue);
                                }
                                if (kvp.Key == "DebtExpectedPremiumValue") {
                                    values[5 + currCounter] = String.Format("{0:#,0.00}", c.debtexpectedvalue);
                                }
                                if (kvp.Key == "FacturedBrokerageValue") {
                                    values[5 + currCounter] = String.Format("{0:#,0.00}", c.facturedbrokeragevalue);
                                }
                                if (kvp.Key == "MarkAgentProvisionValue") {
                                    values[5 + currCounter] = String.Format("{0:#,0.00}", c.marketingagentvalue);
                                }
                                currCounter++;
                            }
                            pdf.AddDataRowWithBorder(values, headers.Count(), codes);
                            counter++;
                        }
                    }
                }
                if (lstBranhces.Count > 0 && lstInsuranceCompanies.Count > 0 && lstInsuranceSubTypes.Count == 0 && lstUsers.Count > 0) {
                    string[] headers = new string[5 + reportPainColumns.Count];
                    headers[0] = "Р.бр.";
                    if (totalType == 0) {
                        headers[1] = "Датум";
                    }
                    if (totalType == 1) {
                        headers[1] = "Месец";
                    }
                    if (totalType == 2) {
                        headers[1] = "Квартал";
                    }
                    headers[2] = "О.компанија";
                    headers[3] = "Филијала";
                    headers[4] = "Корисник";
                    int i = 0;
                    foreach (KeyValuePair<string, string> kvp in reportPainColumns) {
                        headers[5 + i] = kvp.Value;
                        i++;
                    }
                    TypeCode[] codes = new TypeCode[5 + reportPainColumns.Count];

                    codes[0] = TypeCode.Int32;
                    codes[1] = TypeCode.String;
                    codes[2] = TypeCode.String;
                    codes[3] = TypeCode.String;
                    codes[4] = TypeCode.String;
                    int j = 0;
                    foreach (KeyValuePair<string, string> kvp in reportPainColumns) {
                        codes[5 + j] = TypeCode.Decimal;
                        j++;
                    }


                    float[] policyColumnsWidths = new float[headers.Count()];
                    policyColumnsWidths[0] = 5;
                    policyColumnsWidths[1] = 10;
                    policyColumnsWidths[2] = 10;
                    policyColumnsWidths[3] = 15;
                    policyColumnsWidths[4] = 15;
                    float widthPerItem = (float)45;
                    if (reportPainColumns.Count > 0) {
                        widthPerItem = (float)45 / (float)reportPainColumns.Count;
                    }
                    int k = 0;
                    foreach (KeyValuePair<string, string> kvp in reportPainColumns) {
                        policyColumnsWidths[5 + k] = widthPerItem;
                        k++;
                    }
                    pdf.CreateTableWithBorder(headers.Count(), false, headers, "", policyColumnsWidths);
                    if (totalType == 0) {
                        var queryGroupBy = (from t in list
                                            group t by new { t.applicationdate, t.branchid, t.insurancecompanyid, t.userid }
                                                into grp
                                                select new {
                                                    grp.Key.applicationdate,
                                                    grp.Key.branchid,
                                                    grp.Key.insurancecompanyid,
                                                    grp.Key.userid,
                                                    premiumvalue = grp.Sum(t => t.premiumvalue),
                                                    realpremiumvalue = grp.Sum(t => t.realpremiumvalue),
                                                    discountvalue = grp.Sum(t => t.discountvalue),
                                                    paidvalue = grp.Sum(t => t.paidvalue),
                                                    debtvalue = grp.Sum(t => t.debtvalue),
                                                    debtexpectedvalue = grp.Sum(t => t.debtexpectedvalue),
                                                    facturedbrokeragevalue = grp.Sum(c => c.facturedbrokeragevalue),
                                                    marketingagentvalue = grp.Sum(c => c.marketingagentvalue)
                                                }).ToList();
                        foreach (var c in queryGroupBy) {
                            object[] values = new object[5 + reportPainColumns.Count];
                            values[0] = counter;
                            values[1] = c.applicationdate.ToShortDateString();
                            values[2] = InsuranceCompany.Get(c.insurancecompanyid).ShortName;
                            values[3] = Broker.DataAccess.Branch.Get(c.branchid).Name;
                            values[4] = Broker.DataAccess.User.Get(c.userid).Name;
                            int currCounter = 0;
                            foreach (KeyValuePair<string, string> kvp in reportPainColumns) {
                                if (kvp.Key == "RealPolicyValue") {
                                    values[5 + currCounter] = String.Format("{0:#,0.00}", c.realpremiumvalue);
                                }
                                if (kvp.Key == "PremiumValue") {
                                    values[5 + currCounter] = String.Format("{0:#,0.00}", c.premiumvalue);
                                }
                                if (kvp.Key == "PaidPremiumValue") {
                                    values[5 + currCounter] = String.Format("{0:#,0.00}", c.paidvalue);
                                }
                                if (kvp.Key == "DebtPremiumValue") {
                                    values[5 + currCounter] = String.Format("{0:#,0.00}", c.debtvalue);
                                }
                                if (kvp.Key == "DebtExpectedPremiumValue") {
                                    values[5 + currCounter] = String.Format("{0:#,0.00}", c.debtexpectedvalue);
                                }
                                if (kvp.Key == "FacturedBrokerageValue") {
                                    values[5 + currCounter] = String.Format("{0:#,0.00}", c.facturedbrokeragevalue);
                                }
                                if (kvp.Key == "MarkAgentProvisionValue") {
                                    values[5 + currCounter] = String.Format("{0:#,0.00}", c.marketingagentvalue);
                                }
                                currCounter++;
                            }
                            pdf.AddDataRowWithBorder(values, headers.Count(), codes);
                            counter++;
                        }
                    }
                    if (totalType == 1) {
                        var queryGroupBy = (from t in list
                                            group t by new { t.applicationdate.Year, t.applicationdate.Month, t.branchid, t.insurancecompanyid, t.userid }
                                                into grp
                                                select new {
                                                    grp.Key.Year,
                                                    grp.Key.Month,
                                                    grp.Key.branchid,
                                                    grp.Key.insurancecompanyid,
                                                    grp.Key.userid,
                                                    premiumvalue = grp.Sum(t => t.premiumvalue),
                                                    realpremiumvalue = grp.Sum(t => t.realpremiumvalue),
                                                    discountvalue = grp.Sum(t => t.discountvalue),
                                                    paidvalue = grp.Sum(t => t.paidvalue),
                                                    debtvalue = grp.Sum(t => t.debtvalue),
                                                    debtexpectedvalue = grp.Sum(t => t.debtexpectedvalue),
                                                    facturedbrokeragevalue = grp.Sum(c => c.facturedbrokeragevalue),
                                                    marketingagentvalue = grp.Sum(c => c.marketingagentvalue)
                                                }).ToList();
                        foreach (var c in queryGroupBy) {
                            object[] values = new object[5 + reportPainColumns.Count];
                            values[0] = counter;
                            values[1] = c.Month + "." + c.Year;
                            values[2] = InsuranceCompany.Get(c.insurancecompanyid).ShortName;
                            values[3] = Broker.DataAccess.Branch.Get(c.branchid).Name;
                            values[4] = Broker.DataAccess.User.Get(c.userid).Name;
                            int currCounter = 0;
                            foreach (KeyValuePair<string, string> kvp in reportPainColumns) {
                                if (kvp.Key == "RealPolicyValue") {
                                    values[5 + currCounter] = String.Format("{0:#,0.00}", c.realpremiumvalue);
                                }
                                if (kvp.Key == "PremiumValue") {
                                    values[5 + currCounter] = String.Format("{0:#,0.00}", c.premiumvalue);
                                }
                                if (kvp.Key == "PaidPremiumValue") {
                                    values[5 + currCounter] = String.Format("{0:#,0.00}", c.paidvalue);
                                }
                                if (kvp.Key == "DebtPremiumValue") {
                                    values[5 + currCounter] = String.Format("{0:#,0.00}", c.debtvalue);
                                }
                                if (kvp.Key == "DebtExpectedPremiumValue") {
                                    values[5 + currCounter] = String.Format("{0:#,0.00}", c.debtexpectedvalue);
                                }
                                if (kvp.Key == "FacturedBrokerageValue") {
                                    values[5 + currCounter] = String.Format("{0:#,0.00}", c.facturedbrokeragevalue);
                                }
                                if (kvp.Key == "MarkAgentProvisionValue") {
                                    values[5 + currCounter] = String.Format("{0:#,0.00}", c.marketingagentvalue);
                                }
                                currCounter++;
                            }
                            pdf.AddDataRowWithBorder(values, headers.Count(), codes);
                            counter++;
                        }
                    }
                    if (totalType == 2) {
                        var queryGroupBy = (from t in list
                                            group t by new { t.applicationdate.Year, Querter = (t.applicationdate.Month - 1) / 3 + 1, t.branchid, t.insurancecompanyid, t.userid }
                                                into grp
                                                select new {
                                                    grp.Key.Year,
                                                    grp.Key.Querter,
                                                    grp.Key.branchid,
                                                    grp.Key.insurancecompanyid,
                                                    grp.Key.userid,
                                                    premiumvalue = grp.Sum(t => t.premiumvalue),
                                                    realpremiumvalue = grp.Sum(t => t.realpremiumvalue),
                                                    discountvalue = grp.Sum(t => t.discountvalue),
                                                    paidvalue = grp.Sum(t => t.paidvalue),
                                                    debtvalue = grp.Sum(t => t.debtvalue),
                                                    debtexpectedvalue = grp.Sum(t => t.debtexpectedvalue),
                                                    facturedbrokeragevalue = grp.Sum(c => c.facturedbrokeragevalue),
                                                    marketingagentvalue = grp.Sum(c => c.marketingagentvalue)
                                                }).ToList();
                        foreach (var c in queryGroupBy) {
                            object[] values = new object[5 + reportPainColumns.Count];
                            values[0] = counter;
                            values[1] = c.Querter + " квартал " + c.Year;
                            values[2] = InsuranceCompany.Get(c.insurancecompanyid).ShortName;
                            values[3] = Broker.DataAccess.Branch.Get(c.branchid).Name;
                            values[4] = Broker.DataAccess.User.Get(c.userid).Name;
                            int currCounter = 0;
                            foreach (KeyValuePair<string, string> kvp in reportPainColumns) {
                                if (kvp.Key == "RealPolicyValue") {
                                    values[5 + currCounter] = String.Format("{0:#,0.00}", c.realpremiumvalue);
                                }
                                if (kvp.Key == "PremiumValue") {
                                    values[5 + currCounter] = String.Format("{0:#,0.00}", c.premiumvalue);
                                }
                                if (kvp.Key == "PaidPremiumValue") {
                                    values[5 + currCounter] = String.Format("{0:#,0.00}", c.paidvalue);
                                }
                                if (kvp.Key == "DebtPremiumValue") {
                                    values[5 + currCounter] = String.Format("{0:#,0.00}", c.debtvalue);
                                }
                                if (kvp.Key == "DebtExpectedPremiumValue") {
                                    values[5 + currCounter] = String.Format("{0:#,0.00}", c.debtexpectedvalue);
                                }
                                if (kvp.Key == "FacturedBrokerageValue") {
                                    values[5 + currCounter] = String.Format("{0:#,0.00}", c.facturedbrokeragevalue);
                                }
                                if (kvp.Key == "MarkAgentProvisionValue") {
                                    values[5 + currCounter] = String.Format("{0:#,0.00}", c.marketingagentvalue);
                                }
                                currCounter++;
                            }
                            pdf.AddDataRowWithBorder(values, headers.Count(), codes);
                            counter++;
                        }
                    }
                }
                if (lstBranhces.Count > 0 && lstInsuranceCompanies.Count == 0 && lstInsuranceSubTypes.Count > 0 && lstUsers.Count > 0) {
                    string[] headers = new string[5 + reportPainColumns.Count];
                    headers[0] = "Р.бр.";
                    if (totalType == 0) {
                        headers[1] = "Датум";
                    }
                    if (totalType == 1) {
                        headers[1] = "Месец";
                    }
                    if (totalType == 2) {
                        headers[1] = "Квартал";
                    }
                    headers[2] = "Подкласа";
                    headers[3] = "Филијала";
                    headers[4] = "Корисник";
                    int i = 0;
                    foreach (KeyValuePair<string, string> kvp in reportPainColumns) {
                        headers[5 + i] = kvp.Value;
                        i++;
                    }
                    TypeCode[] codes = new TypeCode[5 + reportPainColumns.Count];

                    codes[0] = TypeCode.Int32;
                    codes[1] = TypeCode.String;
                    codes[2] = TypeCode.String;
                    codes[3] = TypeCode.String;
                    codes[4] = TypeCode.String;
                    int j = 0;
                    foreach (KeyValuePair<string, string> kvp in reportPainColumns) {
                        codes[5 + j] = TypeCode.Decimal;
                        j++;
                    }


                    float[] policyColumnsWidths = new float[headers.Count()];
                    policyColumnsWidths[0] = 5;
                    policyColumnsWidths[1] = 10;
                    policyColumnsWidths[2] = 10;
                    policyColumnsWidths[3] = 15;
                    policyColumnsWidths[4] = 15;
                    float widthPerItem = (float)45;
                    if (reportPainColumns.Count > 0) {
                        widthPerItem = (float)45 / (float)reportPainColumns.Count;
                    }
                    int k = 0;
                    foreach (KeyValuePair<string, string> kvp in reportPainColumns) {
                        policyColumnsWidths[5 + k] = widthPerItem;
                        k++;
                    }
                    pdf.CreateTableWithBorder(headers.Count(), false, headers, "", policyColumnsWidths);
                    if (totalType == 0) {
                        var queryGroupBy = (from t in list
                                            group t by new { t.applicationdate, t.branchid, t.insurancesubtypeid, t.userid }
                                                into grp
                                                select new {
                                                    grp.Key.applicationdate,
                                                    grp.Key.branchid,
                                                    grp.Key.insurancesubtypeid,
                                                    grp.Key.userid,
                                                    premiumvalue = grp.Sum(t => t.premiumvalue),
                                                    realpremiumvalue = grp.Sum(t => t.realpremiumvalue),
                                                    discountvalue = grp.Sum(t => t.discountvalue),
                                                    paidvalue = grp.Sum(t => t.paidvalue),
                                                    debtvalue = grp.Sum(t => t.debtvalue),
                                                    debtexpectedvalue = grp.Sum(t => t.debtexpectedvalue),
                                                    facturedbrokeragevalue = grp.Sum(c => c.facturedbrokeragevalue),
                                                    marketingagentvalue = grp.Sum(c => c.marketingagentvalue)
                                                }).ToList();
                        foreach (var c in queryGroupBy) {
                            object[] values = new object[5 + reportPainColumns.Count];
                            values[0] = counter;
                            values[1] = c.applicationdate.ToShortDateString();
                            values[2] = InsuranceSubType.Get(c.insurancesubtypeid).ShortDescription;
                            values[3] = Broker.DataAccess.Branch.Get(c.branchid).Name;
                            values[4] = Broker.DataAccess.User.Get(c.userid).Name;
                            int currCounter = 0;
                            foreach (KeyValuePair<string, string> kvp in reportPainColumns) {
                                if (kvp.Key == "RealPolicyValue") {
                                    values[5 + currCounter] = String.Format("{0:#,0.00}", c.realpremiumvalue);
                                }
                                if (kvp.Key == "PremiumValue") {
                                    values[5 + currCounter] = String.Format("{0:#,0.00}", c.premiumvalue);
                                }
                                if (kvp.Key == "PaidPremiumValue") {
                                    values[5 + currCounter] = String.Format("{0:#,0.00}", c.paidvalue);
                                }
                                if (kvp.Key == "DebtPremiumValue") {
                                    values[5 + currCounter] = String.Format("{0:#,0.00}", c.debtvalue);
                                }
                                if (kvp.Key == "DebtExpectedPremiumValue") {
                                    values[5 + currCounter] = String.Format("{0:#,0.00}", c.debtexpectedvalue);
                                }
                                if (kvp.Key == "FacturedBrokerageValue") {
                                    values[5 + currCounter] = String.Format("{0:#,0.00}", c.facturedbrokeragevalue);
                                }
                                if (kvp.Key == "MarkAgentProvisionValue") {
                                    values[5 + currCounter] = String.Format("{0:#,0.00}", c.marketingagentvalue);
                                }
                                currCounter++;
                            }
                            pdf.AddDataRowWithBorder(values, headers.Count(), codes);
                            counter++;
                        }
                    }
                    if (totalType == 1) {
                        var queryGroupBy = (from t in list
                                            group t by new { t.applicationdate.Year, t.applicationdate.Month, t.branchid, t.insurancesubtypeid, t.userid }
                                                into grp
                                                select new {
                                                    grp.Key.Year,
                                                    grp.Key.Month,
                                                    grp.Key.branchid,
                                                    grp.Key.insurancesubtypeid,
                                                    grp.Key.userid,
                                                    premiumvalue = grp.Sum(t => t.premiumvalue),
                                                    realpremiumvalue = grp.Sum(t => t.realpremiumvalue),
                                                    discountvalue = grp.Sum(t => t.discountvalue),
                                                    paidvalue = grp.Sum(t => t.paidvalue),
                                                    debtvalue = grp.Sum(t => t.debtvalue),
                                                    debtexpectedvalue = grp.Sum(t => t.debtexpectedvalue),
                                                    facturedbrokeragevalue = grp.Sum(c => c.facturedbrokeragevalue),
                                                    marketingagentvalue = grp.Sum(c => c.marketingagentvalue)
                                                }).ToList();
                        foreach (var c in queryGroupBy) {
                            object[] values = new object[5 + reportPainColumns.Count];
                            values[0] = counter;
                            values[1] = c.Month + "." + c.Year;
                            values[2] = InsuranceSubType.Get(c.insurancesubtypeid).ShortDescription;
                            values[3] = Broker.DataAccess.Branch.Get(c.branchid).Name;
                            values[4] = Broker.DataAccess.User.Get(c.userid).Name;
                            int currCounter = 0;
                            foreach (KeyValuePair<string, string> kvp in reportPainColumns) {
                                if (kvp.Key == "RealPolicyValue") {
                                    values[5 + currCounter] = String.Format("{0:#,0.00}", c.realpremiumvalue);
                                }
                                if (kvp.Key == "PremiumValue") {
                                    values[5 + currCounter] = String.Format("{0:#,0.00}", c.premiumvalue);
                                }
                                if (kvp.Key == "PaidPremiumValue") {
                                    values[5 + currCounter] = String.Format("{0:#,0.00}", c.paidvalue);
                                }
                                if (kvp.Key == "DebtPremiumValue") {
                                    values[5 + currCounter] = String.Format("{0:#,0.00}", c.debtvalue);
                                }
                                if (kvp.Key == "DebtExpectedPremiumValue") {
                                    values[5 + currCounter] = String.Format("{0:#,0.00}", c.debtexpectedvalue);
                                }
                                if (kvp.Key == "FacturedBrokerageValue") {
                                    values[5 + currCounter] = String.Format("{0:#,0.00}", c.facturedbrokeragevalue);
                                }
                                if (kvp.Key == "MarkAgentProvisionValue") {
                                    values[5 + currCounter] = String.Format("{0:#,0.00}", c.marketingagentvalue);
                                }
                                currCounter++;
                            }
                            pdf.AddDataRowWithBorder(values, headers.Count(), codes);
                            counter++;
                        }
                    }
                    if (totalType == 2) {
                        var queryGroupBy = (from t in list
                                            group t by new { t.applicationdate.Year, Quarter = (t.applicationdate.Month - 1) / 3 + 1, t.branchid, t.insurancesubtypeid, t.userid }
                                                into grp
                                                select new {
                                                    grp.Key.Year,
                                                    grp.Key.Quarter,
                                                    grp.Key.branchid,
                                                    grp.Key.insurancesubtypeid,
                                                    grp.Key.userid,
                                                    premiumvalue = grp.Sum(t => t.premiumvalue),
                                                    realpremiumvalue = grp.Sum(t => t.realpremiumvalue),
                                                    discountvalue = grp.Sum(t => t.discountvalue),
                                                    paidvalue = grp.Sum(t => t.paidvalue),
                                                    debtvalue = grp.Sum(t => t.debtvalue),
                                                    debtexpectedvalue = grp.Sum(t => t.debtexpectedvalue),
                                                    facturedbrokeragevalue = grp.Sum(c => c.facturedbrokeragevalue),
                                                    marketingagentvalue = grp.Sum(c => c.marketingagentvalue)
                                                }).ToList();
                        foreach (var c in queryGroupBy) {
                            object[] values = new object[5 + reportPainColumns.Count];
                            values[0] = counter;
                            values[1] = c.Quarter + " квартал " + c.Year;
                            values[2] = InsuranceSubType.Get(c.insurancesubtypeid).ShortDescription;
                            values[3] = Broker.DataAccess.Branch.Get(c.branchid).Name;
                            values[4] = Broker.DataAccess.User.Get(c.userid).Name;
                            int currCounter = 0;
                            foreach (KeyValuePair<string, string> kvp in reportPainColumns) {
                                if (kvp.Key == "RealPolicyValue") {
                                    values[5 + currCounter] = String.Format("{0:#,0.00}", c.realpremiumvalue);
                                }
                                if (kvp.Key == "PremiumValue") {
                                    values[5 + currCounter] = String.Format("{0:#,0.00}", c.premiumvalue);
                                }
                                if (kvp.Key == "PaidPremiumValue") {
                                    values[5 + currCounter] = String.Format("{0:#,0.00}", c.paidvalue);
                                }
                                if (kvp.Key == "DebtPremiumValue") {
                                    values[5 + currCounter] = String.Format("{0:#,0.00}", c.debtvalue);
                                }
                                if (kvp.Key == "DebtExpectedPremiumValue") {
                                    values[5 + currCounter] = String.Format("{0:#,0.00}", c.debtexpectedvalue);
                                }
                                if (kvp.Key == "FacturedBrokerageValue") {
                                    values[5 + currCounter] = String.Format("{0:#,0.00}", c.facturedbrokeragevalue);
                                }
                                if (kvp.Key == "MarkAgentProvisionValue") {
                                    values[5 + currCounter] = String.Format("{0:#,0.00}", c.marketingagentvalue);
                                }
                                currCounter++;
                            }
                            pdf.AddDataRowWithBorder(values, headers.Count(), codes);
                            counter++;
                        }
                    }
                }
                if (lstBranhces.Count == 0 && lstInsuranceCompanies.Count > 0 && lstInsuranceSubTypes.Count > 0 && lstUsers.Count > 0) {
                    string[] headers = new string[5 + reportPainColumns.Count];
                    headers[0] = "Р.бр.";
                    if (totalType == 0) {
                        headers[1] = "Датум";
                    }
                    if (totalType == 1) {
                        headers[1] = "Месец";
                    }
                    if (totalType == 2) {
                        headers[1] = "Квартал";
                    }
                    headers[2] = "О.компанија";
                    headers[3] = "Подкласа";
                    headers[4] = "Корисник";
                    int i = 0;
                    foreach (KeyValuePair<string, string> kvp in reportPainColumns) {
                        headers[5 + i] = kvp.Value;
                        i++;
                    }
                    TypeCode[] codes = new TypeCode[5 + reportPainColumns.Count];

                    codes[0] = TypeCode.Int32;
                    codes[1] = TypeCode.String;
                    codes[2] = TypeCode.String;
                    codes[3] = TypeCode.String;
                    codes[4] = TypeCode.String;
                    int j = 0;
                    foreach (KeyValuePair<string, string> kvp in reportPainColumns) {
                        codes[5 + j] = TypeCode.Decimal;
                        j++;
                    }


                    float[] policyColumnsWidths = new float[headers.Count()];
                    policyColumnsWidths[0] = 5;
                    policyColumnsWidths[1] = 10;
                    policyColumnsWidths[2] = 10;
                    policyColumnsWidths[3] = 15;
                    policyColumnsWidths[4] = 15;
                    float widthPerItem = (float)45;
                    if (reportPainColumns.Count > 0) {
                        widthPerItem = (float)45 / (float)reportPainColumns.Count;
                    }
                    int k = 0;
                    foreach (KeyValuePair<string, string> kvp in reportPainColumns) {
                        policyColumnsWidths[5 + k] = widthPerItem;
                        k++;
                    }
                    pdf.CreateTableWithBorder(headers.Count(), false, headers, "", policyColumnsWidths);
                    if (totalType == 0) {
                        var queryGroupBy = (from t in list
                                            group t by new { t.applicationdate, t.insurancecompanyid, t.insurancesubtypeid, t.userid }
                                                into grp
                                                select new {
                                                    grp.Key.applicationdate,
                                                    grp.Key.insurancecompanyid,
                                                    grp.Key.insurancesubtypeid,
                                                    grp.Key.userid,
                                                    premiumvalue = grp.Sum(t => t.premiumvalue),
                                                    realpremiumvalue = grp.Sum(t => t.realpremiumvalue),
                                                    discountvalue = grp.Sum(t => t.discountvalue),
                                                    paidvalue = grp.Sum(t => t.paidvalue),
                                                    debtvalue = grp.Sum(t => t.debtvalue),
                                                    debtexpectedvalue = grp.Sum(t => t.debtexpectedvalue),
                                                    facturedbrokeragevalue = grp.Sum(c => c.facturedbrokeragevalue),
                                                    marketingagentvalue = grp.Sum(c => c.marketingagentvalue)
                                                }).ToList();
                        foreach (var c in queryGroupBy) {
                            object[] values = new object[5 + reportPainColumns.Count];
                            values[0] = counter;
                            values[1] = c.applicationdate.ToShortDateString();
                            values[2] = InsuranceCompany.Get(c.insurancecompanyid).ShortName;
                            values[3] = InsuranceSubType.Get(c.insurancesubtypeid).ShortDescription;
                            values[4] = Broker.DataAccess.User.Get(c.userid).Name;
                            int currCounter = 0;
                            foreach (KeyValuePair<string, string> kvp in reportPainColumns) {
                                if (kvp.Key == "RealPolicyValue") {
                                    values[5 + currCounter] = String.Format("{0:#,0.00}", c.realpremiumvalue);
                                }
                                if (kvp.Key == "PremiumValue") {
                                    values[5 + currCounter] = String.Format("{0:#,0.00}", c.premiumvalue);
                                }
                                if (kvp.Key == "PaidPremiumValue") {
                                    values[5 + currCounter] = String.Format("{0:#,0.00}", c.paidvalue);
                                }
                                if (kvp.Key == "DebtPremiumValue") {
                                    values[5 + currCounter] = String.Format("{0:#,0.00}", c.debtvalue);
                                }
                                if (kvp.Key == "DebtExpectedPremiumValue") {
                                    values[5 + currCounter] = String.Format("{0:#,0.00}", c.debtexpectedvalue);
                                }
                                if (kvp.Key == "FacturedBrokerageValue") {
                                    values[5 + currCounter] = String.Format("{0:#,0.00}", c.facturedbrokeragevalue);
                                }
                                if (kvp.Key == "MarkAgentProvisionValue") {
                                    values[5 + currCounter] = String.Format("{0:#,0.00}", c.marketingagentvalue);
                                }
                                currCounter++;
                            }
                            pdf.AddDataRowWithBorder(values, headers.Count(), codes);
                            counter++;
                        }
                    }
                    if (totalType == 1) {
                        var queryGroupBy = (from t in list
                                            group t by new { t.applicationdate.Year, t.applicationdate.Month, t.insurancecompanyid, t.insurancesubtypeid, t.userid }
                                                into grp
                                                select new {
                                                    grp.Key.Year,
                                                    grp.Key.Month,
                                                    grp.Key.insurancecompanyid,
                                                    grp.Key.insurancesubtypeid,
                                                    grp.Key.userid,
                                                    premiumvalue = grp.Sum(t => t.premiumvalue),
                                                    realpremiumvalue = grp.Sum(t => t.realpremiumvalue),
                                                    discountvalue = grp.Sum(t => t.discountvalue),
                                                    paidvalue = grp.Sum(t => t.paidvalue),
                                                    debtvalue = grp.Sum(t => t.debtvalue),
                                                    debtexpectedvalue = grp.Sum(t => t.debtexpectedvalue),
                                                    facturedbrokeragevalue = grp.Sum(c => c.facturedbrokeragevalue),
                                                    marketingagentvalue = grp.Sum(c => c.marketingagentvalue)
                                                }).ToList();
                        foreach (var c in queryGroupBy) {
                            object[] values = new object[5 + reportPainColumns.Count];
                            values[0] = counter;
                            values[1] = c.Month + "." + c.Year;
                            values[2] = InsuranceCompany.Get(c.insurancecompanyid).ShortName;
                            values[3] = InsuranceSubType.Get(c.insurancesubtypeid).ShortDescription;
                            values[4] = Broker.DataAccess.User.Get(c.userid).Name;
                            int currCounter = 0;
                            foreach (KeyValuePair<string, string> kvp in reportPainColumns) {
                                if (kvp.Key == "RealPolicyValue") {
                                    values[5 + currCounter] = String.Format("{0:#,0.00}", c.realpremiumvalue);
                                }
                                if (kvp.Key == "PremiumValue") {
                                    values[5 + currCounter] = String.Format("{0:#,0.00}", c.premiumvalue);
                                }
                                if (kvp.Key == "PaidPremiumValue") {
                                    values[5 + currCounter] = String.Format("{0:#,0.00}", c.paidvalue);
                                }
                                if (kvp.Key == "DebtPremiumValue") {
                                    values[5 + currCounter] = String.Format("{0:#,0.00}", c.debtvalue);
                                }
                                if (kvp.Key == "DebtExpectedPremiumValue") {
                                    values[5 + currCounter] = String.Format("{0:#,0.00}", c.debtexpectedvalue);
                                }
                                if (kvp.Key == "FacturedBrokerageValue") {
                                    values[5 + currCounter] = String.Format("{0:#,0.00}", c.facturedbrokeragevalue);
                                }
                                if (kvp.Key == "MarkAgentProvisionValue") {
                                    values[5 + currCounter] = String.Format("{0:#,0.00}", c.marketingagentvalue);
                                }
                                currCounter++;
                            }
                            pdf.AddDataRowWithBorder(values, headers.Count(), codes);
                            counter++;
                        }
                    }
                    if (totalType == 2) {
                        var queryGroupBy = (from t in list
                                            group t by new { t.applicationdate.Year, Quarter = (t.applicationdate.Month - 1) / 3 + 1, t.insurancecompanyid, t.insurancesubtypeid, t.userid }
                                                into grp
                                                select new {
                                                    grp.Key.Year,
                                                    grp.Key.Quarter,
                                                    grp.Key.insurancecompanyid,
                                                    grp.Key.insurancesubtypeid,
                                                    grp.Key.userid,
                                                    premiumvalue = grp.Sum(t => t.premiumvalue),
                                                    realpremiumvalue = grp.Sum(t => t.realpremiumvalue),
                                                    discountvalue = grp.Sum(t => t.discountvalue),
                                                    paidvalue = grp.Sum(t => t.paidvalue),
                                                    debtvalue = grp.Sum(t => t.debtvalue),
                                                    debtexpectedvalue = grp.Sum(t => t.debtexpectedvalue),
                                                    facturedbrokeragevalue = grp.Sum(c => c.facturedbrokeragevalue),
                                                    marketingagentvalue = grp.Sum(c => c.marketingagentvalue)
                                                }).ToList();
                        foreach (var c in queryGroupBy) {
                            object[] values = new object[5 + reportPainColumns.Count];
                            values[0] = counter;
                            values[1] = c.Quarter + " квартал " + c.Year;
                            values[2] = InsuranceCompany.Get(c.insurancecompanyid).ShortName;
                            values[3] = InsuranceSubType.Get(c.insurancesubtypeid).ShortDescription;
                            values[4] = Broker.DataAccess.User.Get(c.userid).Name;
                            int currCounter = 0;
                            foreach (KeyValuePair<string, string> kvp in reportPainColumns) {
                                if (kvp.Key == "RealPolicyValue") {
                                    values[5 + currCounter] = String.Format("{0:#,0.00}", c.realpremiumvalue);
                                }
                                if (kvp.Key == "PremiumValue") {
                                    values[5 + currCounter] = String.Format("{0:#,0.00}", c.premiumvalue);
                                }
                                if (kvp.Key == "PaidPremiumValue") {
                                    values[5 + currCounter] = String.Format("{0:#,0.00}", c.paidvalue);
                                }
                                if (kvp.Key == "DebtPremiumValue") {
                                    values[5 + currCounter] = String.Format("{0:#,0.00}", c.debtvalue);
                                }
                                if (kvp.Key == "DebtExpectedPremiumValue") {
                                    values[5 + currCounter] = String.Format("{0:#,0.00}", c.debtexpectedvalue);
                                }
                                if (kvp.Key == "FacturedBrokerageValue") {
                                    values[5 + currCounter] = String.Format("{0:#,0.00}", c.facturedbrokeragevalue);
                                }
                                if (kvp.Key == "MarkAgentProvisionValue") {
                                    values[5 + currCounter] = String.Format("{0:#,0.00}", c.marketingagentvalue);
                                }
                                currCounter++;
                            }
                            pdf.AddDataRowWithBorder(values, headers.Count(), codes);
                            counter++;
                        }
                    }
                }
                if (lstBranhces.Count == 0 && lstInsuranceCompanies.Count == 0 && lstInsuranceSubTypes.Count > 0 && lstUsers.Count > 0) {
                    string[] headers = new string[4 + reportPainColumns.Count];
                    headers[0] = "Р.бр.";
                    if (totalType == 0) {
                        headers[1] = "Датум";
                    }
                    if (totalType == 1) {
                        headers[1] = "Месец";
                    }
                    if (totalType == 2) {
                        headers[1] = "Квартал";
                    }
                    headers[2] = "Подкласа";
                    headers[3] = "Корисник";
                    int i = 0;
                    foreach (KeyValuePair<string, string> kvp in reportPainColumns) {
                        headers[4 + i] = kvp.Value;
                        i++;
                    }
                    TypeCode[] codes = new TypeCode[4 + reportPainColumns.Count];

                    codes[0] = TypeCode.Int32;
                    codes[1] = TypeCode.String;
                    codes[2] = TypeCode.String;
                    codes[3] = TypeCode.String;
                    int j = 0;
                    foreach (KeyValuePair<string, string> kvp in reportPainColumns) {
                        codes[4 + j] = TypeCode.Decimal;
                        j++;
                    }

                    float[] policyColumnsWidths = new float[headers.Count()];
                    policyColumnsWidths[0] = 5;
                    policyColumnsWidths[1] = 10;
                    policyColumnsWidths[2] = 15;
                    policyColumnsWidths[3] = 15;
                    float widthPerItem = (float)55;
                    if (reportPainColumns.Count > 0) {
                        widthPerItem = (float)55 / (float)reportPainColumns.Count;
                    }
                    int k = 0;
                    foreach (KeyValuePair<string, string> kvp in reportPainColumns) {
                        policyColumnsWidths[4 + k] = widthPerItem;
                        k++;
                    }
                    pdf.CreateTableWithBorder(headers.Count(), false, headers, "", policyColumnsWidths);
                    if (totalType == 0) {
                        var queryGroupBy = (from t in list
                                            group t by new { t.applicationdate, t.insurancesubtypeid, t.userid }
                                                into grp
                                                select new {
                                                    grp.Key.applicationdate,
                                                    grp.Key.insurancesubtypeid,
                                                    grp.Key.userid,
                                                    premiumvalue = grp.Sum(t => t.premiumvalue),
                                                    realpremiumvalue = grp.Sum(t => t.realpremiumvalue),
                                                    discountvalue = grp.Sum(t => t.discountvalue),
                                                    paidvalue = grp.Sum(t => t.paidvalue),
                                                    debtvalue = grp.Sum(t => t.debtvalue),
                                                    debtexpectedvalue = grp.Sum(t => t.debtexpectedvalue),
                                                    facturedbrokeragevalue = grp.Sum(c => c.facturedbrokeragevalue),
                                                    marketingagentvalue = grp.Sum(c => c.marketingagentvalue)
                                                }).ToList();
                        foreach (var c in queryGroupBy) {
                            object[] values = new object[4 + reportPainColumns.Count];
                            values[0] = counter;
                            values[1] = c.applicationdate.ToShortDateString();
                            values[2] = InsuranceSubType.Get(c.insurancesubtypeid).ShortDescription;
                            values[3] = Broker.DataAccess.User.Get(c.userid).Name;
                            int currCounter = 0;
                            foreach (KeyValuePair<string, string> kvp in reportPainColumns) {
                                if (kvp.Key == "RealPolicyValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.realpremiumvalue);
                                }
                                if (kvp.Key == "PremiumValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.premiumvalue);
                                }
                                if (kvp.Key == "PaidPremiumValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.paidvalue);
                                }
                                if (kvp.Key == "DebtPremiumValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.debtvalue);
                                }
                                if (kvp.Key == "DebtExpectedPremiumValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.debtexpectedvalue);
                                }
                                if (kvp.Key == "FacturedBrokerageValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.facturedbrokeragevalue);
                                }
                                if (kvp.Key == "MarkAgentProvisionValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.marketingagentvalue);
                                }
                                currCounter++;
                            }
                            pdf.AddDataRowWithBorder(values, headers.Count(), codes);
                            counter++;
                        }
                    }
                    if (totalType == 1) {
                        var queryGroupBy = (from t in list
                                            group t by new { t.applicationdate.Year, t.applicationdate.Month, t.insurancesubtypeid, t.userid }
                                                into grp
                                                select new {
                                                    grp.Key.Year,
                                                    grp.Key.Month,
                                                    grp.Key.insurancesubtypeid,
                                                    grp.Key.userid,
                                                    premiumvalue = grp.Sum(t => t.premiumvalue),
                                                    realpremiumvalue = grp.Sum(t => t.realpremiumvalue),
                                                    discountvalue = grp.Sum(t => t.discountvalue),
                                                    paidvalue = grp.Sum(t => t.paidvalue),
                                                    debtvalue = grp.Sum(t => t.debtvalue),
                                                    debtexpectedvalue = grp.Sum(t => t.debtexpectedvalue),
                                                    facturedbrokeragevalue = grp.Sum(c => c.facturedbrokeragevalue),
                                                    marketingagentvalue = grp.Sum(c => c.marketingagentvalue)
                                                }).ToList();
                        foreach (var c in queryGroupBy) {
                            object[] values = new object[4 + reportPainColumns.Count];
                            values[0] = counter;
                            values[1] = c.Month + "." + c.Year;
                            values[2] = Broker.DataAccess.InsuranceSubType.Get(c.insurancesubtypeid).ShortDescription;
                            values[3] = Broker.DataAccess.User.Get(c.userid).Name;
                            int currCounter = 0;
                            foreach (KeyValuePair<string, string> kvp in reportPainColumns) {
                                if (kvp.Key == "RealPolicyValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.realpremiumvalue);
                                }
                                if (kvp.Key == "PremiumValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.premiumvalue);
                                }
                                if (kvp.Key == "PaidPremiumValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.paidvalue);
                                }
                                if (kvp.Key == "DebtPremiumValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.debtvalue);
                                }
                                if (kvp.Key == "DebtExpectedPremiumValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.debtexpectedvalue);
                                }
                                if (kvp.Key == "FacturedBrokerageValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.facturedbrokeragevalue);
                                }
                                if (kvp.Key == "MarkAgentProvisionValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.marketingagentvalue);
                                }
                                currCounter++;
                            }
                            pdf.AddDataRowWithBorder(values, headers.Count(), codes);
                            counter++;
                        }
                    }
                    if (totalType == 2) {
                        var queryGroupBy = (from t in list
                                            group t by new { t.applicationdate.Year, Quarter = (t.applicationdate.Month - 1) / 3 + 1, t.insurancesubtypeid, t.userid }
                                                into grp
                                                select new {
                                                    grp.Key.Year,
                                                    grp.Key.Quarter,
                                                    grp.Key.insurancesubtypeid,
                                                    grp.Key.userid,
                                                    premiumvalue = grp.Sum(t => t.premiumvalue),
                                                    realpremiumvalue = grp.Sum(t => t.realpremiumvalue),
                                                    discountvalue = grp.Sum(t => t.discountvalue),
                                                    paidvalue = grp.Sum(t => t.paidvalue),
                                                    debtvalue = grp.Sum(t => t.debtvalue),
                                                    debtexpectedvalue = grp.Sum(t => t.debtexpectedvalue),
                                                    facturedbrokeragevalue = grp.Sum(c => c.facturedbrokeragevalue),
                                                    marketingagentvalue = grp.Sum(c => c.marketingagentvalue)
                                                }).ToList();
                        foreach (var c in queryGroupBy) {
                            object[] values = new object[4 + reportPainColumns.Count];
                            values[0] = counter;
                            values[1] = c.Quarter + " квартал " + c.Year;
                            values[2] = InsuranceSubType.Get(c.insurancesubtypeid).ShortDescription;
                            values[3] = Broker.DataAccess.User.Get(c.userid).Name;
                            int currCounter = 0;
                            foreach (KeyValuePair<string, string> kvp in reportPainColumns) {
                                if (kvp.Key == "RealPolicyValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.realpremiumvalue);
                                }
                                if (kvp.Key == "PremiumValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.premiumvalue);
                                }
                                if (kvp.Key == "PaidPremiumValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.paidvalue);
                                }
                                if (kvp.Key == "DebtPremiumValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.debtvalue);
                                }
                                if (kvp.Key == "DebtExpectedPremiumValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.debtexpectedvalue);
                                }
                                if (kvp.Key == "FacturedBrokerageValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.facturedbrokeragevalue);
                                }
                                if (kvp.Key == "MarkAgentProvisionValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.marketingagentvalue);
                                }
                                currCounter++;
                            }
                            pdf.AddDataRowWithBorder(values, headers.Count(), codes);
                            counter++;
                        }
                    }
                }
                if (lstBranhces.Count == 0 && lstInsuranceCompanies.Count > 0 && lstInsuranceSubTypes.Count == 0 && lstUsers.Count > 0) {
                    string[] headers = new string[4 + reportPainColumns.Count];
                    headers[0] = "Р.бр.";
                    if (totalType == 0) {
                        headers[1] = "Датум";
                    }
                    if (totalType == 1) {
                        headers[1] = "Месец";
                    }
                    if (totalType == 2) {
                        headers[1] = "Квартал";
                    }
                    headers[2] = "О.компанија";
                    headers[3] = "Корисник";
                    int i = 0;
                    foreach (KeyValuePair<string, string> kvp in reportPainColumns) {
                        headers[4 + i] = kvp.Value;
                        i++;
                    }
                    TypeCode[] codes = new TypeCode[4 + reportPainColumns.Count];

                    codes[0] = TypeCode.Int32;
                    codes[1] = TypeCode.String;
                    codes[2] = TypeCode.String;
                    codes[3] = TypeCode.String;
                    int j = 0;
                    foreach (KeyValuePair<string, string> kvp in reportPainColumns) {
                        codes[4 + j] = TypeCode.Decimal;
                        j++;
                    }

                    float[] policyColumnsWidths = new float[headers.Count()];
                    policyColumnsWidths[0] = 5;
                    policyColumnsWidths[1] = 10;
                    policyColumnsWidths[2] = 15;
                    policyColumnsWidths[3] = 15;
                    float widthPerItem = (float)55;
                    if (reportPainColumns.Count > 0) {
                        widthPerItem = (float)55 / (float)reportPainColumns.Count;
                    }
                    int k = 0;
                    foreach (KeyValuePair<string, string> kvp in reportPainColumns) {
                        policyColumnsWidths[4 + k] = widthPerItem;
                        k++;
                    }
                    pdf.CreateTableWithBorder(headers.Count(), false, headers, "", policyColumnsWidths);
                    if (totalType == 0) {
                        var queryGroupBy = (from t in list
                                            group t by new { t.applicationdate, t.insurancecompanyid, t.userid }
                                                into grp
                                                select new {
                                                    grp.Key.applicationdate,
                                                    grp.Key.insurancecompanyid,
                                                    grp.Key.userid,
                                                    premiumvalue = grp.Sum(t => t.premiumvalue),
                                                    realpremiumvalue = grp.Sum(t => t.realpremiumvalue),
                                                    discountvalue = grp.Sum(t => t.discountvalue),
                                                    paidvalue = grp.Sum(t => t.paidvalue),
                                                    debtvalue = grp.Sum(t => t.debtvalue),
                                                    debtexpectedvalue = grp.Sum(t => t.debtexpectedvalue),
                                                    facturedbrokeragevalue = grp.Sum(c => c.facturedbrokeragevalue),
                                                    marketingagentvalue = grp.Sum(c => c.marketingagentvalue)
                                                }).ToList();
                        foreach (var c in queryGroupBy) {
                            object[] values = new object[4 + reportPainColumns.Count];
                            values[0] = counter;
                            values[1] = c.applicationdate.ToShortDateString();
                            values[2] = InsuranceCompany.Get(c.insurancecompanyid).ShortName;
                            values[3] = Broker.DataAccess.User.Get(c.userid).Name;
                            int currCounter = 0;
                            foreach (KeyValuePair<string, string> kvp in reportPainColumns) {
                                if (kvp.Key == "RealPolicyValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.realpremiumvalue);
                                }
                                if (kvp.Key == "PremiumValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.premiumvalue);
                                }
                                if (kvp.Key == "PaidPremiumValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.paidvalue);
                                }
                                if (kvp.Key == "DebtPremiumValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.debtvalue);
                                }
                                if (kvp.Key == "DebtExpectedPremiumValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.debtexpectedvalue);
                                }
                                if (kvp.Key == "FacturedBrokerageValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.facturedbrokeragevalue);
                                }
                                if (kvp.Key == "MarkAgentProvisionValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.marketingagentvalue);
                                }
                                currCounter++;
                            }
                            pdf.AddDataRowWithBorder(values, headers.Count(), codes);
                            counter++;
                        }
                    }
                    if (totalType == 1) {
                        var queryGroupBy = (from t in list
                                            group t by new { t.applicationdate.Year, t.applicationdate.Month, t.insurancecompanyid, t.userid }
                                                into grp
                                                select new {
                                                    grp.Key.Year,
                                                    grp.Key.Month,
                                                    grp.Key.insurancecompanyid,
                                                    grp.Key.userid,
                                                    premiumvalue = grp.Sum(t => t.premiumvalue),
                                                    realpremiumvalue = grp.Sum(t => t.realpremiumvalue),
                                                    discountvalue = grp.Sum(t => t.discountvalue),
                                                    paidvalue = grp.Sum(t => t.paidvalue),
                                                    debtvalue = grp.Sum(t => t.debtvalue),
                                                    debtexpectedvalue = grp.Sum(t => t.debtexpectedvalue),
                                                    facturedbrokeragevalue = grp.Sum(c => c.facturedbrokeragevalue),
                                                    marketingagentvalue = grp.Sum(c => c.marketingagentvalue)
                                                }).ToList();
                        foreach (var c in queryGroupBy) {
                            object[] values = new object[4 + reportPainColumns.Count];
                            values[0] = counter;
                            values[1] = c.Month + "." + c.Year;
                            values[2] = InsuranceCompany.Get(c.insurancecompanyid).ShortName;
                            values[3] = Broker.DataAccess.User.Get(c.userid).Name;
                            int currCounter = 0;
                            foreach (KeyValuePair<string, string> kvp in reportPainColumns) {
                                if (kvp.Key == "RealPolicyValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.realpremiumvalue);
                                }
                                if (kvp.Key == "PremiumValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.premiumvalue);
                                }
                                if (kvp.Key == "PaidPremiumValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.paidvalue);
                                }
                                if (kvp.Key == "DebtPremiumValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.debtvalue);
                                }
                                if (kvp.Key == "DebtExpectedPremiumValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.debtexpectedvalue);
                                }
                                if (kvp.Key == "FacturedBrokerageValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.facturedbrokeragevalue);
                                }
                                if (kvp.Key == "MarkAgentProvisionValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.marketingagentvalue);
                                }
                                currCounter++;
                            }
                            pdf.AddDataRowWithBorder(values, headers.Count(), codes);
                            counter++;
                        }
                    }
                    if (totalType == 2) {
                        var queryGroupBy = (from t in list
                                            group t by new { t.applicationdate.Year, Quarter = (t.applicationdate.Month - 1 / 3) + 1, t.insurancecompanyid, t.userid }
                                                into grp
                                                select new {
                                                    grp.Key.Year,
                                                    grp.Key.Quarter,
                                                    grp.Key.insurancecompanyid,
                                                    grp.Key.userid,
                                                    premiumvalue = grp.Sum(t => t.premiumvalue),
                                                    realpremiumvalue = grp.Sum(t => t.realpremiumvalue),
                                                    discountvalue = grp.Sum(t => t.discountvalue),
                                                    paidvalue = grp.Sum(t => t.paidvalue),
                                                    debtvalue = grp.Sum(t => t.debtvalue),
                                                    debtexpectedvalue = grp.Sum(t => t.debtexpectedvalue),
                                                    facturedbrokeragevalue = grp.Sum(c => c.facturedbrokeragevalue),
                                                    marketingagentvalue = grp.Sum(c => c.marketingagentvalue)
                                                }).ToList();
                        foreach (var c in queryGroupBy) {
                            object[] values = new object[4 + reportPainColumns.Count];
                            values[0] = counter;
                            values[1] = c.Quarter + " квартал " + c.Year;
                            values[2] = InsuranceCompany.Get(c.insurancecompanyid).ShortName;
                            values[3] = Broker.DataAccess.User.Get(c.userid).Name;
                            int currCounter = 0;
                            foreach (KeyValuePair<string, string> kvp in reportPainColumns) {
                                if (kvp.Key == "RealPolicyValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.realpremiumvalue);
                                }
                                if (kvp.Key == "PremiumValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.premiumvalue);
                                }
                                if (kvp.Key == "PaidPremiumValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.paidvalue);
                                }
                                if (kvp.Key == "DebtPremiumValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.debtvalue);
                                }
                                if (kvp.Key == "DebtExpectedPremiumValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.debtexpectedvalue);
                                }
                                if (kvp.Key == "FacturedBrokerageValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.facturedbrokeragevalue);
                                }
                                if (kvp.Key == "MarkAgentProvisionValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.marketingagentvalue);
                                }
                                currCounter++;
                            }
                            pdf.AddDataRowWithBorder(values, headers.Count(), codes);
                            counter++;
                        }
                    }
                }
                if (lstBranhces.Count == 0 && lstInsuranceCompanies.Count > 0 && lstInsuranceSubTypes.Count > 0 && lstUsers.Count == 0) {
                    string[] headers = new string[4 + reportPainColumns.Count];
                    headers[0] = "Р.бр.";
                    if (totalType == 0) {
                        headers[1] = "Датум";
                    }
                    if (totalType == 1) {
                        headers[1] = "Месец";
                    }
                    if (totalType == 2) {
                        headers[1] = "Квартал";
                    }
                    headers[2] = "О.компанија";
                    headers[3] = "Подкласа";
                    int i = 0;
                    foreach (KeyValuePair<string, string> kvp in reportPainColumns) {
                        headers[4 + i] = kvp.Value;
                        i++;
                    }
                    TypeCode[] codes = new TypeCode[4 + reportPainColumns.Count];

                    codes[0] = TypeCode.Int32;
                    codes[1] = TypeCode.String;
                    codes[2] = TypeCode.String;
                    codes[3] = TypeCode.String;
                    int j = 0;
                    foreach (KeyValuePair<string, string> kvp in reportPainColumns) {
                        codes[4 + j] = TypeCode.Decimal;
                        j++;
                    }

                    float[] policyColumnsWidths = new float[headers.Count()];
                    policyColumnsWidths[0] = 5;
                    policyColumnsWidths[1] = 10;
                    policyColumnsWidths[2] = 15;
                    policyColumnsWidths[3] = 15;
                    float widthPerItem = (float)55;
                    if (reportPainColumns.Count > 0) {
                        widthPerItem = (float)55 / (float)reportPainColumns.Count;
                    }
                    int k = 0;
                    foreach (KeyValuePair<string, string> kvp in reportPainColumns) {
                        policyColumnsWidths[4 + k] = widthPerItem;
                        k++;
                    }
                    pdf.CreateTableWithBorder(headers.Count(), false, headers, "", policyColumnsWidths);
                    if (totalType == 0) {
                        var queryGroupBy = (from t in list
                                            group t by new { t.applicationdate, t.insurancecompanyid, t.insurancesubtypeid }
                                                into grp
                                                select new {
                                                    grp.Key.applicationdate,
                                                    grp.Key.insurancecompanyid,
                                                    grp.Key.insurancesubtypeid,
                                                    premiumvalue = grp.Sum(t => t.premiumvalue),
                                                    realpremiumvalue = grp.Sum(t => t.realpremiumvalue),
                                                    discountvalue = grp.Sum(t => t.discountvalue),
                                                    paidvalue = grp.Sum(t => t.paidvalue),
                                                    debtvalue = grp.Sum(t => t.debtvalue),
                                                    debtexpectedvalue = grp.Sum(t => t.debtexpectedvalue),
                                                    facturedbrokeragevalue = grp.Sum(c => c.facturedbrokeragevalue),
                                                    marketingagentvalue = grp.Sum(c => c.marketingagentvalue)
                                                }).ToList();
                        foreach (var c in queryGroupBy) {
                            object[] values = new object[4 + reportPainColumns.Count];
                            values[0] = counter;
                            values[1] = c.applicationdate.ToShortDateString();
                            values[2] = InsuranceCompany.Get(c.insurancecompanyid).ShortName;
                            values[3] = InsuranceSubType.Get(c.insurancesubtypeid).ShortDescription;
                            int currCounter = 0;
                            foreach (KeyValuePair<string, string> kvp in reportPainColumns) {
                                if (kvp.Key == "RealPolicyValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.realpremiumvalue);
                                }
                                if (kvp.Key == "PremiumValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.premiumvalue);
                                }
                                if (kvp.Key == "PaidPremiumValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.paidvalue);
                                }
                                if (kvp.Key == "DebtPremiumValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.debtvalue);
                                }
                                if (kvp.Key == "DebtExpectedPremiumValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.debtexpectedvalue);
                                }
                                if (kvp.Key == "FacturedBrokerageValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.facturedbrokeragevalue);
                                }
                                if (kvp.Key == "MarkAgentProvisionValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.marketingagentvalue);
                                }
                                currCounter++;
                            }
                            pdf.AddDataRowWithBorder(values, headers.Count(), codes);
                            counter++;
                        }
                    }
                    if (totalType == 1) {
                        var queryGroupBy = (from t in list
                                            group t by new { t.applicationdate.Year, t.applicationdate.Month, t.insurancecompanyid, t.insurancesubtypeid }
                                                into grp
                                                select new {
                                                    grp.Key.Year,
                                                    grp.Key.Month,
                                                    grp.Key.insurancecompanyid,
                                                    grp.Key.insurancesubtypeid,
                                                    premiumvalue = grp.Sum(t => t.premiumvalue),
                                                    realpremiumvalue = grp.Sum(t => t.realpremiumvalue),
                                                    discountvalue = grp.Sum(t => t.discountvalue),
                                                    paidvalue = grp.Sum(t => t.paidvalue),
                                                    debtvalue = grp.Sum(t => t.debtvalue),
                                                    debtexpectedvalue = grp.Sum(t => t.debtexpectedvalue),
                                                    facturedbrokeragevalue = grp.Sum(c => c.facturedbrokeragevalue),
                                                    marketingagentvalue = grp.Sum(c => c.marketingagentvalue)
                                                }).ToList();
                        foreach (var c in queryGroupBy) {
                            object[] values = new object[4 + reportPainColumns.Count];
                            values[0] = counter;
                            values[1] = c.Month + "." + c.Year;
                            values[2] = InsuranceCompany.Get(c.insurancecompanyid).ShortName;
                            values[3] = InsuranceSubType.Get(c.insurancesubtypeid).ShortDescription;
                            int currCounter = 0;
                            foreach (KeyValuePair<string, string> kvp in reportPainColumns) {
                                if (kvp.Key == "RealPolicyValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.realpremiumvalue);
                                }
                                if (kvp.Key == "PremiumValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.premiumvalue);
                                }
                                if (kvp.Key == "PaidPremiumValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.paidvalue);
                                }
                                if (kvp.Key == "DebtPremiumValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.debtvalue);
                                }
                                if (kvp.Key == "DebtExpectedPremiumValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.debtexpectedvalue);
                                }
                                if (kvp.Key == "FacturedBrokerageValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.facturedbrokeragevalue);
                                }
                                if (kvp.Key == "MarkAgentProvisionValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.marketingagentvalue);
                                }
                                currCounter++;
                            }
                            pdf.AddDataRowWithBorder(values, headers.Count(), codes);
                            counter++;
                        }
                    }
                    if (totalType == 2) {
                        var queryGroupBy = (from t in list
                                            group t by new { t.applicationdate.Year, Quarter = (t.applicationdate.Month - 1) / 3 + 1, t.insurancecompanyid, t.insurancesubtypeid }
                                                into grp
                                                select new {
                                                    grp.Key.Year,
                                                    grp.Key.Quarter,
                                                    grp.Key.insurancecompanyid,
                                                    grp.Key.insurancesubtypeid,
                                                    premiumvalue = grp.Sum(t => t.premiumvalue),
                                                    realpremiumvalue = grp.Sum(t => t.realpremiumvalue),
                                                    discountvalue = grp.Sum(t => t.discountvalue),
                                                    paidvalue = grp.Sum(t => t.paidvalue),
                                                    debtvalue = grp.Sum(t => t.debtvalue),
                                                    debtexpectedvalue = grp.Sum(t => t.debtexpectedvalue),
                                                    facturedbrokeragevalue = grp.Sum(c => c.facturedbrokeragevalue),
                                                    marketingagentvalue = grp.Sum(c => c.marketingagentvalue)
                                                }).ToList();
                        foreach (var c in queryGroupBy) {
                            object[] values = new object[4 + reportPainColumns.Count];
                            values[0] = counter;
                            values[1] = c.Quarter + " квартал " + c.Year;
                            values[2] = InsuranceCompany.Get(c.insurancecompanyid).ShortName;
                            values[3] = InsuranceSubType.Get(c.insurancesubtypeid).ShortDescription;
                            int currCounter = 0;
                            foreach (KeyValuePair<string, string> kvp in reportPainColumns) {
                                if (kvp.Key == "RealPolicyValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.realpremiumvalue);
                                }
                                if (kvp.Key == "PremiumValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.premiumvalue);
                                }
                                if (kvp.Key == "PaidPremiumValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.paidvalue);
                                }
                                if (kvp.Key == "DebtPremiumValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.debtvalue);
                                }
                                if (kvp.Key == "DebtExpectedPremiumValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.debtexpectedvalue);
                                }
                                if (kvp.Key == "FacturedBrokerageValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.facturedbrokeragevalue);
                                }
                                if (kvp.Key == "MarkAgentProvisionValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.marketingagentvalue);
                                }
                                currCounter++;
                            }
                            pdf.AddDataRowWithBorder(values, headers.Count(), codes);
                            counter++;
                        }
                    }
                }
                if (lstBranhces.Count > 0 && lstInsuranceCompanies.Count == 0 && lstInsuranceSubTypes.Count == 0 && lstUsers.Count > 0) {
                    string[] headers = new string[4 + reportPainColumns.Count];
                    headers[0] = "Р.бр.";
                    if (totalType == 0) {
                        headers[1] = "Датум";
                    }
                    if (totalType == 1) {
                        headers[1] = "Месец";
                    }
                    if (totalType == 2) {
                        headers[1] = "Квартал";
                    }
                    headers[2] = "Филијала";
                    headers[3] = "Корисник";
                    int i = 0;
                    foreach (KeyValuePair<string, string> kvp in reportPainColumns) {
                        headers[4 + i] = kvp.Value;
                        i++;
                    }
                    TypeCode[] codes = new TypeCode[4 + reportPainColumns.Count];

                    codes[0] = TypeCode.Int32;
                    codes[1] = TypeCode.String;
                    codes[2] = TypeCode.String;
                    codes[3] = TypeCode.String;
                    int j = 0;
                    foreach (KeyValuePair<string, string> kvp in reportPainColumns) {
                        codes[4 + j] = TypeCode.Decimal;
                        j++;
                    }

                    float[] policyColumnsWidths = new float[headers.Count()];
                    policyColumnsWidths[0] = 5;
                    policyColumnsWidths[1] = 10;
                    policyColumnsWidths[2] = 15;
                    policyColumnsWidths[3] = 15;
                    float widthPerItem = (float)55;
                    if (reportPainColumns.Count > 0) {
                        widthPerItem = (float)55 / (float)reportPainColumns.Count;
                    }
                    int k = 0;
                    foreach (KeyValuePair<string, string> kvp in reportPainColumns) {
                        policyColumnsWidths[4 + k] = widthPerItem;
                        k++;
                    }
                    pdf.CreateTableWithBorder(headers.Count(), false, headers, "", policyColumnsWidths);
                    if (totalType == 0) {
                        var queryGroupBy = (from t in list
                                            group t by new { t.applicationdate, t.branchid, t.userid }
                                                into grp
                                                select new {
                                                    grp.Key.applicationdate,
                                                    grp.Key.branchid,
                                                    grp.Key.userid,
                                                    premiumvalue = grp.Sum(t => t.premiumvalue),
                                                    realpremiumvalue = grp.Sum(t => t.realpremiumvalue),
                                                    discountvalue = grp.Sum(t => t.discountvalue),
                                                    paidvalue = grp.Sum(t => t.paidvalue),
                                                    debtvalue = grp.Sum(t => t.debtvalue),
                                                    debtexpectedvalue = grp.Sum(t => t.debtexpectedvalue),
                                                    facturedbrokeragevalue = grp.Sum(c => c.facturedbrokeragevalue),
                                                    marketingagentvalue = grp.Sum(c => c.marketingagentvalue)
                                                }).ToList();
                        foreach (var c in queryGroupBy) {
                            object[] values = new object[4 + reportPainColumns.Count];
                            values[0] = counter;
                            values[1] = c.applicationdate.ToShortDateString();
                            values[2] = Broker.DataAccess.Branch.Get(c.branchid);
                            values[3] = Broker.DataAccess.User.Get(c.userid);
                            int currCounter = 0;
                            foreach (KeyValuePair<string, string> kvp in reportPainColumns) {
                                if (kvp.Key == "RealPolicyValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.realpremiumvalue);
                                }
                                if (kvp.Key == "PremiumValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.premiumvalue);
                                }
                                if (kvp.Key == "PaidPremiumValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.paidvalue);
                                }
                                if (kvp.Key == "DebtPremiumValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.debtvalue);
                                }
                                if (kvp.Key == "DebtExpectedPremiumValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.debtexpectedvalue);
                                }
                                if (kvp.Key == "FacturedBrokerageValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.facturedbrokeragevalue);
                                }
                                if (kvp.Key == "MarkAgentProvisionValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.marketingagentvalue);
                                }
                                currCounter++;
                            }
                            pdf.AddDataRowWithBorder(values, headers.Count(), codes);
                            counter++;
                        }
                    }
                    if (totalType == 1) {
                        var queryGroupBy = (from t in list
                                            group t by new { t.applicationdate.Year, t.applicationdate.Month, t.branchid, t.userid }
                                                into grp
                                                select new {
                                                    grp.Key.Year,
                                                    grp.Key.Month,
                                                    grp.Key.branchid,
                                                    grp.Key.userid,
                                                    premiumvalue = grp.Sum(t => t.premiumvalue),
                                                    realpremiumvalue = grp.Sum(t => t.realpremiumvalue),
                                                    discountvalue = grp.Sum(t => t.discountvalue),
                                                    paidvalue = grp.Sum(t => t.paidvalue),
                                                    debtvalue = grp.Sum(t => t.debtvalue),
                                                    debtexpectedvalue = grp.Sum(t => t.debtexpectedvalue),
                                                    facturedbrokeragevalue = grp.Sum(c => c.facturedbrokeragevalue),
                                                    marketingagentvalue = grp.Sum(c => c.marketingagentvalue)
                                                }).ToList();
                        foreach (var c in queryGroupBy) {
                            object[] values = new object[4 + reportPainColumns.Count];
                            values[0] = counter;
                            values[1] = c.Month + "." + c.Year;
                            values[2] = Broker.DataAccess.Branch.Get(c.branchid);
                            values[3] = Broker.DataAccess.User.Get(c.userid);
                            int currCounter = 0;
                            foreach (KeyValuePair<string, string> kvp in reportPainColumns) {
                                if (kvp.Key == "RealPolicyValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.realpremiumvalue);
                                }
                                if (kvp.Key == "PremiumValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.premiumvalue);
                                }
                                if (kvp.Key == "PaidPremiumValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.paidvalue);
                                }
                                if (kvp.Key == "DebtPremiumValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.debtvalue);
                                }
                                if (kvp.Key == "DebtExpectedPremiumValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.debtexpectedvalue);
                                }
                                if (kvp.Key == "FacturedBrokerageValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.facturedbrokeragevalue);
                                }
                                if (kvp.Key == "MarkAgentProvisionValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.marketingagentvalue);
                                }
                                currCounter++;
                            }
                            pdf.AddDataRowWithBorder(values, headers.Count(), codes);
                            counter++;
                        }
                    }
                    if (totalType == 2) {
                        var queryGroupBy = (from t in list
                                            group t by new { t.applicationdate.Year, Quarter = (t.applicationdate.Month - 1) / 3 + 1, t.branchid, t.userid }
                                                into grp
                                                select new {
                                                    grp.Key.Year,
                                                    grp.Key.Quarter,
                                                    grp.Key.branchid,
                                                    grp.Key.userid,
                                                    premiumvalue = grp.Sum(t => t.premiumvalue),
                                                    realpremiumvalue = grp.Sum(t => t.realpremiumvalue),
                                                    discountvalue = grp.Sum(t => t.discountvalue),
                                                    paidvalue = grp.Sum(t => t.paidvalue),
                                                    debtvalue = grp.Sum(t => t.debtvalue),
                                                    debtexpectedvalue = grp.Sum(t => t.debtexpectedvalue),
                                                    facturedbrokeragevalue = grp.Sum(c => c.facturedbrokeragevalue),
                                                    marketingagentvalue = grp.Sum(c => c.marketingagentvalue)
                                                }).ToList();
                        foreach (var c in queryGroupBy) {
                            object[] values = new object[4 + reportPainColumns.Count];
                            values[0] = counter;
                            values[1] = c.Quarter + " квартал " + c.Year;
                            values[2] = Broker.DataAccess.Branch.Get(c.branchid);
                            values[3] = Broker.DataAccess.User.Get(c.userid);
                            int currCounter = 0;
                            foreach (KeyValuePair<string, string> kvp in reportPainColumns) {
                                if (kvp.Key == "RealPolicyValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.realpremiumvalue);
                                }
                                if (kvp.Key == "PremiumValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.premiumvalue);
                                }
                                if (kvp.Key == "PaidPremiumValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.paidvalue);
                                }
                                if (kvp.Key == "DebtPremiumValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.debtvalue);
                                }
                                if (kvp.Key == "DebtExpectedPremiumValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.debtexpectedvalue);
                                }
                                if (kvp.Key == "FacturedBrokerageValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.facturedbrokeragevalue);
                                }
                                if (kvp.Key == "MarkAgentProvisionValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.marketingagentvalue);
                                }
                                currCounter++;
                            }
                            pdf.AddDataRowWithBorder(values, headers.Count(), codes);
                            counter++;
                        }
                    }
                }
                if (lstBranhces.Count > 0 && lstInsuranceCompanies.Count == 0 && lstInsuranceSubTypes.Count > 0 && lstUsers.Count == 0) {
                    string[] headers = new string[4 + reportPainColumns.Count];
                    headers[0] = "Р.бр.";
                    if (totalType == 0) {
                        headers[1] = "Датум";
                    }
                    if (totalType == 1) {
                        headers[1] = "Месец";
                    }
                    if (totalType == 2) {
                        headers[1] = "Квартал";
                    }
                    headers[2] = "Филијала";
                    headers[3] = "Подкласа";
                    int i = 0;
                    foreach (KeyValuePair<string, string> kvp in reportPainColumns) {
                        headers[4 + i] = kvp.Value;
                        i++;
                    }
                    TypeCode[] codes = new TypeCode[4 + reportPainColumns.Count];

                    codes[0] = TypeCode.Int32;
                    codes[1] = TypeCode.String;
                    codes[2] = TypeCode.String;
                    codes[3] = TypeCode.String;
                    int j = 0;
                    foreach (KeyValuePair<string, string> kvp in reportPainColumns) {
                        codes[4 + j] = TypeCode.Decimal;
                        j++;
                    }

                    float[] policyColumnsWidths = new float[headers.Count()];
                    policyColumnsWidths[0] = 5;
                    policyColumnsWidths[1] = 10;
                    policyColumnsWidths[2] = 15;
                    policyColumnsWidths[3] = 15;
                    float widthPerItem = (float)55;
                    if (reportPainColumns.Count > 0) {
                        widthPerItem = (float)55 / (float)reportPainColumns.Count;
                    }
                    int k = 0;
                    foreach (KeyValuePair<string, string> kvp in reportPainColumns) {
                        policyColumnsWidths[4 + k] = widthPerItem;
                        k++;
                    }
                    pdf.CreateTableWithBorder(headers.Count(), false, headers, "", policyColumnsWidths);
                    if (totalType == 0) {
                        var queryGroupBy = (from t in list
                                            group t by new { t.applicationdate, t.branchid, t.insurancesubtypeid }
                                                into grp
                                                select new {
                                                    grp.Key.applicationdate,
                                                    grp.Key.branchid,
                                                    grp.Key.insurancesubtypeid,
                                                    premiumvalue = grp.Sum(t => t.premiumvalue),
                                                    realpremiumvalue = grp.Sum(t => t.realpremiumvalue),
                                                    discountvalue = grp.Sum(t => t.discountvalue),
                                                    paidvalue = grp.Sum(t => t.paidvalue),
                                                    debtvalue = grp.Sum(t => t.debtvalue),
                                                    debtexpectedvalue = grp.Sum(t => t.debtexpectedvalue),
                                                    facturedbrokeragevalue = grp.Sum(c => c.facturedbrokeragevalue),
                                                    marketingagentvalue = grp.Sum(c => c.marketingagentvalue)
                                                }).ToList();
                        foreach (var c in queryGroupBy) {
                            object[] values = new object[4 + reportPainColumns.Count];
                            values[0] = counter;
                            values[1] = c.applicationdate.ToShortDateString();
                            values[2] = Broker.DataAccess.Branch.Get(c.branchid);
                            values[3] = InsuranceSubType.Get(c.insurancesubtypeid).ShortDescription;
                            int currCounter = 0;
                            foreach (KeyValuePair<string, string> kvp in reportPainColumns) {
                                if (kvp.Key == "RealPolicyValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.realpremiumvalue);
                                }
                                if (kvp.Key == "PremiumValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.premiumvalue);
                                }
                                if (kvp.Key == "PaidPremiumValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.paidvalue);
                                }
                                if (kvp.Key == "DebtPremiumValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.debtvalue);
                                }
                                if (kvp.Key == "DebtExpectedPremiumValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.debtexpectedvalue);
                                }
                                if (kvp.Key == "FacturedBrokerageValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.facturedbrokeragevalue);
                                }
                                if (kvp.Key == "MarkAgentProvisionValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.marketingagentvalue);
                                }
                                currCounter++;
                            }
                            pdf.AddDataRowWithBorder(values, headers.Count(), codes);
                            counter++;
                        }
                    }
                    if (totalType == 1) {
                        var queryGroupBy = (from t in list
                                            group t by new { t.applicationdate.Year, t.applicationdate.Month, t.branchid, t.insurancesubtypeid }
                                                into grp
                                                select new {
                                                    grp.Key.Year,
                                                    grp.Key.Month,
                                                    grp.Key.branchid,
                                                    grp.Key.insurancesubtypeid,
                                                    premiumvalue = grp.Sum(t => t.premiumvalue),
                                                    realpremiumvalue = grp.Sum(t => t.realpremiumvalue),
                                                    discountvalue = grp.Sum(t => t.discountvalue),
                                                    paidvalue = grp.Sum(t => t.paidvalue),
                                                    debtvalue = grp.Sum(t => t.debtvalue),
                                                    debtexpectedvalue = grp.Sum(t => t.debtexpectedvalue),
                                                    facturedbrokeragevalue = grp.Sum(c => c.facturedbrokeragevalue),
                                                    marketingagentvalue = grp.Sum(c => c.marketingagentvalue)
                                                }).ToList();
                        foreach (var c in queryGroupBy) {
                            object[] values = new object[4 + reportPainColumns.Count];
                            values[0] = counter;
                            values[1] = c.Month + "." + c.Year;
                            values[2] = Broker.DataAccess.Branch.Get(c.branchid);
                            values[3] = Broker.DataAccess.InsuranceSubType.Get(c.insurancesubtypeid).ShortDescription;
                            int currCounter = 0;
                            foreach (KeyValuePair<string, string> kvp in reportPainColumns) {
                                if (kvp.Key == "RealPolicyValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.realpremiumvalue);
                                }
                                if (kvp.Key == "PremiumValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.premiumvalue);
                                }
                                if (kvp.Key == "PaidPremiumValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.paidvalue);
                                }
                                if (kvp.Key == "DebtPremiumValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.debtvalue);
                                }
                                if (kvp.Key == "DebtExpectedPremiumValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.debtexpectedvalue);
                                }
                                if (kvp.Key == "FacturedBrokerageValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.facturedbrokeragevalue);
                                }
                                if (kvp.Key == "MarkAgentProvisionValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.marketingagentvalue);
                                }
                                currCounter++;
                            }
                            pdf.AddDataRowWithBorder(values, headers.Count(), codes);
                            counter++;
                        }
                    }
                    if (totalType == 2) {
                        var queryGroupBy = (from t in list
                                            group t by new { t.applicationdate.Year, Quarter = (t.applicationdate.Month - 1) / 3 + 1, t.branchid, t.insurancesubtypeid }
                                                into grp
                                                select new {
                                                    grp.Key.Year,
                                                    grp.Key.Quarter,
                                                    grp.Key.branchid,
                                                    grp.Key.insurancesubtypeid,
                                                    premiumvalue = grp.Sum(t => t.premiumvalue),
                                                    realpremiumvalue = grp.Sum(t => t.realpremiumvalue),
                                                    discountvalue = grp.Sum(t => t.discountvalue),
                                                    paidvalue = grp.Sum(t => t.paidvalue),
                                                    debtvalue = grp.Sum(t => t.debtvalue),
                                                    debtexpectedvalue = grp.Sum(t => t.debtexpectedvalue),
                                                    facturedbrokeragevalue = grp.Sum(c => c.facturedbrokeragevalue),
                                                    marketingagentvalue = grp.Sum(c => c.marketingagentvalue)
                                                }).ToList();
                        foreach (var c in queryGroupBy) {
                            object[] values = new object[4 + reportPainColumns.Count];
                            values[0] = counter;
                            values[1] = c.Quarter + " квартал " + c.Year;
                            values[2] = Broker.DataAccess.Branch.Get(c.branchid);
                            values[3] = InsuranceSubType.Get(c.insurancesubtypeid).ShortDescription;
                            int currCounter = 0;
                            foreach (KeyValuePair<string, string> kvp in reportPainColumns) {
                                if (kvp.Key == "RealPolicyValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.realpremiumvalue);
                                }
                                if (kvp.Key == "PremiumValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.premiumvalue);
                                }
                                if (kvp.Key == "PaidPremiumValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.paidvalue);
                                }
                                if (kvp.Key == "DebtPremiumValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.debtvalue);
                                }
                                if (kvp.Key == "DebtExpectedPremiumValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.debtexpectedvalue);
                                }
                                if (kvp.Key == "FacturedBrokerageValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.facturedbrokeragevalue);
                                }
                                if (kvp.Key == "MarkAgentProvisionValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.marketingagentvalue);
                                }
                                currCounter++;
                            }
                            pdf.AddDataRowWithBorder(values, headers.Count(), codes);
                            counter++;
                        }
                    }
                }
                if (lstBranhces.Count > 0 && lstInsuranceCompanies.Count > 0 && lstInsuranceSubTypes.Count == 0 && lstUsers.Count == 0) {
                    string[] headers = new string[4 + reportPainColumns.Count];
                    headers[0] = "Р.бр.";
                    if (totalType == 0) {
                        headers[1] = "Датум";
                    }
                    if (totalType == 1) {
                        headers[1] = "Месец";
                    }
                    if (totalType == 2) {
                        headers[1] = "Квартал";
                    }
                    headers[3] = "Филијала";
                    headers[2] = "О.компанија";
                    int i = 0;
                    foreach (KeyValuePair<string, string> kvp in reportPainColumns) {
                        headers[4 + i] = kvp.Value;
                        i++;
                    }
                    TypeCode[] codes = new TypeCode[4 + reportPainColumns.Count];

                    codes[0] = TypeCode.Int32;
                    codes[1] = TypeCode.String;
                    codes[2] = TypeCode.String;
                    codes[3] = TypeCode.String;
                    int j = 0;
                    foreach (KeyValuePair<string, string> kvp in reportPainColumns) {
                        codes[4 + j] = TypeCode.Decimal;
                        j++;
                    }

                    float[] policyColumnsWidths = new float[headers.Count()];
                    policyColumnsWidths[0] = 5;
                    policyColumnsWidths[1] = 10;
                    policyColumnsWidths[2] = 15;
                    policyColumnsWidths[3] = 15;
                    float widthPerItem = (float)55;
                    if (reportPainColumns.Count > 0) {
                        widthPerItem = (float)55 / (float)reportPainColumns.Count;
                    }
                    int k = 0;
                    foreach (KeyValuePair<string, string> kvp in reportPainColumns) {
                        policyColumnsWidths[4 + k] = widthPerItem;
                        k++;
                    }
                    pdf.CreateTableWithBorder(headers.Count(), false, headers, "", policyColumnsWidths);
                    if (totalType == 0) {
                        var queryGroupBy = (from t in list
                                            group t by new { t.applicationdate, t.branchid, t.insurancecompanyid }
                                                into grp
                                                select new {
                                                    grp.Key.applicationdate,
                                                    grp.Key.branchid,
                                                    grp.Key.insurancecompanyid,
                                                    premiumvalue = grp.Sum(t => t.premiumvalue),
                                                    realpremiumvalue = grp.Sum(t => t.realpremiumvalue),
                                                    discountvalue = grp.Sum(t => t.discountvalue),
                                                    paidvalue = grp.Sum(t => t.paidvalue),
                                                    debtvalue = grp.Sum(t => t.debtvalue),
                                                    debtexpectedvalue = grp.Sum(t => t.debtexpectedvalue),
                                                    facturedbrokeragevalue = grp.Sum(c => c.facturedbrokeragevalue),
                                                    marketingagentvalue = grp.Sum(c => c.marketingagentvalue)
                                                }).ToList();
                        foreach (var c in queryGroupBy) {
                            object[] values = new object[4 + reportPainColumns.Count];
                            values[0] = counter;
                            values[1] = c.applicationdate.ToShortDateString();
                            values[3] = Broker.DataAccess.Branch.Get(c.branchid);
                            values[2] = InsuranceCompany.Get(c.insurancecompanyid).ShortName;
                            int currCounter = 0;
                            foreach (KeyValuePair<string, string> kvp in reportPainColumns) {
                                if (kvp.Key == "RealPolicyValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.realpremiumvalue);
                                }
                                if (kvp.Key == "PremiumValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.premiumvalue);
                                }
                                if (kvp.Key == "PaidPremiumValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.paidvalue);
                                }
                                if (kvp.Key == "DebtPremiumValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.debtvalue);
                                }
                                if (kvp.Key == "DebtExpectedPremiumValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.debtexpectedvalue);
                                }
                                if (kvp.Key == "FacturedBrokerageValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.facturedbrokeragevalue);
                                }
                                if (kvp.Key == "MarkAgentProvisionValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.marketingagentvalue);
                                }
                                currCounter++;
                            }
                            pdf.AddDataRowWithBorder(values, headers.Count(), codes);
                            counter++;
                        }
                    }
                    if (totalType == 1) {
                        var queryGroupBy = (from t in list
                                            group t by new { t.applicationdate.Year, t.applicationdate.Month, t.branchid, t.insurancecompanyid }
                                                into grp
                                                select new {
                                                    grp.Key.Year,
                                                    grp.Key.Month,
                                                    grp.Key.branchid,
                                                    grp.Key.insurancecompanyid,
                                                    premiumvalue = grp.Sum(t => t.premiumvalue),
                                                    realpremiumvalue = grp.Sum(t => t.realpremiumvalue),
                                                    discountvalue = grp.Sum(t => t.discountvalue),
                                                    paidvalue = grp.Sum(t => t.paidvalue),
                                                    debtvalue = grp.Sum(t => t.debtvalue),
                                                    debtexpectedvalue = grp.Sum(t => t.debtexpectedvalue),
                                                    facturedbrokeragevalue = grp.Sum(c => c.facturedbrokeragevalue),
                                                    marketingagentvalue = grp.Sum(c => c.marketingagentvalue)
                                                }).ToList();
                        foreach (var c in queryGroupBy) {
                            object[] values = new object[4 + reportPainColumns.Count];
                            values[0] = counter;
                            values[1] = c.Month + "." + c.Year;
                            values[3] = Broker.DataAccess.Branch.Get(c.branchid);
                            values[2] = InsuranceCompany.Get(c.insurancecompanyid).ShortName;
                            int currCounter = 0;
                            foreach (KeyValuePair<string, string> kvp in reportPainColumns) {
                                if (kvp.Key == "RealPolicyValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.realpremiumvalue);
                                }
                                if (kvp.Key == "PremiumValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.premiumvalue);
                                }
                                if (kvp.Key == "PaidPremiumValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.paidvalue);
                                }
                                if (kvp.Key == "DebtPremiumValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.debtvalue);
                                }
                                if (kvp.Key == "DebtExpectedPremiumValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.debtexpectedvalue);
                                }
                                if (kvp.Key == "FacturedBrokerageValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.facturedbrokeragevalue);
                                }
                                if (kvp.Key == "MarkAgentProvisionValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.marketingagentvalue);
                                }
                                currCounter++;
                            }
                            pdf.AddDataRowWithBorder(values, headers.Count(), codes);
                            counter++;
                        }
                    }
                    if (totalType == 2) {
                        var queryGroupBy = (from t in list
                                            group t by new { t.applicationdate.Year, Quarter = (t.applicationdate.Month - 1) / 3 + 1, t.branchid, t.insurancecompanyid }
                                                into grp
                                                select new {
                                                    grp.Key.Year,
                                                    grp.Key.Quarter,
                                                    grp.Key.branchid,
                                                    grp.Key.insurancecompanyid,
                                                    premiumvalue = grp.Sum(t => t.premiumvalue),
                                                    realpremiumvalue = grp.Sum(t => t.realpremiumvalue),
                                                    discountvalue = grp.Sum(t => t.discountvalue),
                                                    paidvalue = grp.Sum(t => t.paidvalue),
                                                    debtvalue = grp.Sum(t => t.debtvalue),
                                                    debtexpectedvalue = grp.Sum(t => t.debtexpectedvalue),
                                                    facturedbrokeragevalue = grp.Sum(c => c.facturedbrokeragevalue),
                                                    marketingagentvalue = grp.Sum(c => c.marketingagentvalue)
                                                }).ToList();
                        foreach (var c in queryGroupBy) {
                            object[] values = new object[4 + reportPainColumns.Count];
                            values[0] = counter;
                            values[1] = c.Quarter + " квартал " + c.Year;
                            values[3] = Broker.DataAccess.Branch.Get(c.branchid);
                            values[2] = InsuranceCompany.Get(c.insurancecompanyid).ShortName;
                            int currCounter = 0;
                            foreach (KeyValuePair<string, string> kvp in reportPainColumns) {
                                if (kvp.Key == "RealPolicyValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.realpremiumvalue);
                                }
                                if (kvp.Key == "PremiumValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.premiumvalue);
                                }
                                if (kvp.Key == "PaidPremiumValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.paidvalue);
                                }
                                if (kvp.Key == "DebtPremiumValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.debtvalue);
                                }
                                if (kvp.Key == "DebtExpectedPremiumValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.debtexpectedvalue);
                                }
                                if (kvp.Key == "FacturedBrokerageValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.facturedbrokeragevalue);
                                }
                                if (kvp.Key == "MarkAgentProvisionValue") {
                                    values[4 + currCounter] = String.Format("{0:#,0.00}", c.marketingagentvalue);
                                }
                                currCounter++;
                            }
                            pdf.AddDataRowWithBorder(values, headers.Count(), codes);
                            counter++;
                        }
                    }
                }
                if (lstBranhces.Count > 0 && lstInsuranceCompanies.Count == 0 && lstInsuranceSubTypes.Count == 0 && lstUsers.Count == 0) {
                    string[] headers = new string[3 + reportPainColumns.Count];
                    headers[0] = "Р.бр.";
                    if (totalType == 0) {
                        headers[1] = "Датум";
                    }
                    if (totalType == 1) {
                        headers[1] = "Месец";
                    }
                    if (totalType == 2) {
                        headers[1] = "Квартал";
                    }
                    headers[2] = "Филијала";
                    int i = 0;
                    foreach (KeyValuePair<string, string> kvp in reportPainColumns) {
                        headers[3 + i] = kvp.Value;
                        i++;
                    }
                    TypeCode[] codes = new TypeCode[3 + reportPainColumns.Count];

                    codes[0] = TypeCode.Int32;
                    codes[1] = TypeCode.String;
                    codes[2] = TypeCode.String;
                    int j = 0;
                    foreach (KeyValuePair<string, string> kvp in reportPainColumns) {
                        codes[3 + j] = TypeCode.Decimal;
                        j++;
                    }

                    float[] policyColumnsWidths = new float[headers.Count()];
                    policyColumnsWidths[0] = 5;
                    policyColumnsWidths[1] = 15;
                    policyColumnsWidths[2] = 15;
                    float widthPerItem = (float)65;
                    if (reportPainColumns.Count > 0) {
                        widthPerItem = (float)65 / (float)reportPainColumns.Count;
                    }
                    int k = 0;
                    foreach (KeyValuePair<string, string> kvp in reportPainColumns) {
                        policyColumnsWidths[3 + k] = widthPerItem;
                        k++;
                    }
                    pdf.CreateTableWithBorder(headers.Count(), false, headers, "", policyColumnsWidths);
                    if (totalType == 0) {
                        var queryGroupBy = (from t in list
                                            group t by new { t.applicationdate, t.branchid }
                                                into grp
                                                select new {
                                                    grp.Key.applicationdate,
                                                    grp.Key.branchid,
                                                    premiumvalue = grp.Sum(t => t.premiumvalue),
                                                    realpremiumvalue = grp.Sum(t => t.realpremiumvalue),
                                                    discountvalue = grp.Sum(t => t.discountvalue),
                                                    paidvalue = grp.Sum(t => t.paidvalue),
                                                    debtvalue = grp.Sum(t => t.debtvalue),
                                                    debtexpectedvalue = grp.Sum(t => t.debtexpectedvalue),
                                                    facturedbrokeragevalue = grp.Sum(c => c.facturedbrokeragevalue),
                                                    marketingagentvalue = grp.Sum(c => c.marketingagentvalue)
                                                }).ToList();
                        foreach (var c in queryGroupBy) {
                            object[] values = new object[3 + reportPainColumns.Count];
                            values[0] = counter;
                            values[1] = c.applicationdate.ToShortDateString();
                            values[2] = Broker.DataAccess.Branch.Get(c.branchid);
                            int currCounter = 0;
                            foreach (KeyValuePair<string, string> kvp in reportPainColumns) {
                                if (kvp.Key == "RealPolicyValue") {
                                    values[3 + currCounter] = String.Format("{0:#,0.00}", c.realpremiumvalue);
                                }
                                if (kvp.Key == "PremiumValue") {
                                    values[3 + currCounter] = String.Format("{0:#,0.00}", c.premiumvalue);
                                }
                                if (kvp.Key == "PaidPremiumValue") {
                                    values[3 + currCounter] = String.Format("{0:#,0.00}", c.paidvalue);
                                }
                                if (kvp.Key == "DebtPremiumValue") {
                                    values[3 + currCounter] = String.Format("{0:#,0.00}", c.debtvalue);
                                }
                                if (kvp.Key == "DebtExpectedPremiumValue") {
                                    values[3 + currCounter] = String.Format("{0:#,0.00}", c.debtexpectedvalue);
                                }
                                if (kvp.Key == "FacturedBrokerageValue") {
                                    values[3 + currCounter] = String.Format("{0:#,0.00}", c.facturedbrokeragevalue);
                                }
                                if (kvp.Key == "MarkAgentProvisionValue") {
                                    values[3 + currCounter] = String.Format("{0:#,0.00}", c.marketingagentvalue);
                                }
                                currCounter++;
                            }
                            pdf.AddDataRowWithBorder(values, headers.Count(), codes);
                            counter++;
                        }
                    }
                    if (totalType == 1) {
                        var queryGroupBy = (from t in list
                                            group t by new { t.applicationdate.Year, t.applicationdate.Month, t.branchid }
                                                into grp
                                                select new {
                                                    grp.Key.Year,
                                                    grp.Key.Month,
                                                    grp.Key.branchid,
                                                    premiumvalue = grp.Sum(t => t.premiumvalue),
                                                    realpremiumvalue = grp.Sum(t => t.realpremiumvalue),
                                                    discountvalue = grp.Sum(t => t.discountvalue),
                                                    paidvalue = grp.Sum(t => t.paidvalue),
                                                    debtvalue = grp.Sum(t => t.debtvalue),
                                                    debtexpectedvalue = grp.Sum(t => t.debtexpectedvalue),
                                                    facturedbrokeragevalue = grp.Sum(c => c.facturedbrokeragevalue),
                                                    marketingagentvalue = grp.Sum(c => c.marketingagentvalue)
                                                }).ToList();
                        foreach (var c in queryGroupBy) {
                            object[] values = new object[3 + reportPainColumns.Count];
                            values[0] = counter;
                            values[1] = c.Month + "." + c.Year;
                            values[2] = Broker.DataAccess.Branch.Get(c.branchid);
                            int currCounter = 0;
                            foreach (KeyValuePair<string, string> kvp in reportPainColumns) {
                                if (kvp.Key == "RealPolicyValue") {
                                    values[3 + currCounter] = String.Format("{0:#,0.00}", c.realpremiumvalue);
                                }
                                if (kvp.Key == "PremiumValue") {
                                    values[3 + currCounter] = String.Format("{0:#,0.00}", c.premiumvalue);
                                }
                                if (kvp.Key == "PaidPremiumValue") {
                                    values[3 + currCounter] = String.Format("{0:#,0.00}", c.paidvalue);
                                }
                                if (kvp.Key == "DebtPremiumValue") {
                                    values[3 + currCounter] = String.Format("{0:#,0.00}", c.debtvalue);
                                }
                                if (kvp.Key == "DebtExpectedPremiumValue") {
                                    values[3 + currCounter] = String.Format("{0:#,0.00}", c.debtexpectedvalue);
                                }
                                if (kvp.Key == "FacturedBrokerageValue") {
                                    values[3 + currCounter] = String.Format("{0:#,0.00}", c.facturedbrokeragevalue);
                                }
                                if (kvp.Key == "MarkAgentProvisionValue") {
                                    values[3 + currCounter] = String.Format("{0:#,0.00}", c.marketingagentvalue);
                                }
                                currCounter++;
                            }
                            pdf.AddDataRowWithBorder(values, headers.Count(), codes);
                            counter++;
                        }
                    }
                    if (totalType == 2) {
                        var queryGroupBy = (from t in list
                                            group t by new { t.applicationdate.Year, Quarter = (t.applicationdate.Month - 1) / 3 + 1, t.branchid }
                                                into grp
                                                select new {
                                                    grp.Key.Year,
                                                    grp.Key.Quarter,
                                                    grp.Key.branchid,
                                                    premiumvalue = grp.Sum(t => t.premiumvalue),
                                                    realpremiumvalue = grp.Sum(t => t.realpremiumvalue),
                                                    discountvalue = grp.Sum(t => t.discountvalue),
                                                    paidvalue = grp.Sum(t => t.paidvalue),
                                                    debtvalue = grp.Sum(t => t.debtvalue),
                                                    debtexpectedvalue = grp.Sum(t => t.debtexpectedvalue),
                                                    facturedbrokeragevalue = grp.Sum(c => c.facturedbrokeragevalue),
                                                    marketingagentvalue = grp.Sum(c => c.marketingagentvalue)
                                                }).ToList();
                        foreach (var c in queryGroupBy) {
                            object[] values = new object[3 + reportPainColumns.Count];
                            values[0] = counter;
                            values[1] = c.Quarter + " квартал " + c.Year;
                            values[2] = Broker.DataAccess.Branch.Get(c.branchid);
                            int currCounter = 0;
                            foreach (KeyValuePair<string, string> kvp in reportPainColumns) {
                                if (kvp.Key == "RealPolicyValue") {
                                    values[3 + currCounter] = String.Format("{0:#,0.00}", c.realpremiumvalue);
                                }
                                if (kvp.Key == "PremiumValue") {
                                    values[3 + currCounter] = String.Format("{0:#,0.00}", c.premiumvalue);
                                }
                                if (kvp.Key == "PaidPremiumValue") {
                                    values[3 + currCounter] = String.Format("{0:#,0.00}", c.paidvalue);
                                }
                                if (kvp.Key == "DebtPremiumValue") {
                                    values[3 + currCounter] = String.Format("{0:#,0.00}", c.debtvalue);
                                }
                                if (kvp.Key == "DebtExpectedPremiumValue") {
                                    values[3 + currCounter] = String.Format("{0:#,0.00}", c.debtexpectedvalue);
                                }
                                if (kvp.Key == "FacturedBrokerageValue") {
                                    values[3 + currCounter] = String.Format("{0:#,0.00}", c.facturedbrokeragevalue);
                                }
                                if (kvp.Key == "MarkAgentProvisionValue") {
                                    values[3 + currCounter] = String.Format("{0:#,0.00}", c.marketingagentvalue);
                                }
                                currCounter++;
                            }
                            pdf.AddDataRowWithBorder(values, headers.Count(), codes);
                            counter++;
                        }
                    }
                }
                if (lstBranhces.Count == 0 && lstInsuranceCompanies.Count > 0 && lstInsuranceSubTypes.Count == 0 && lstUsers.Count == 0) {
                    string[] headers = new string[3 + reportPainColumns.Count];
                    headers[0] = "Р.бр.";
                    if (totalType == 0) {
                        headers[1] = "Датум";
                    }
                    if (totalType == 1) {
                        headers[1] = "Месец";
                    }
                    if (totalType == 2) {
                        headers[1] = "Квартал";
                    }
                    headers[2] = "О.компанија";
                    int i = 0;
                    foreach (KeyValuePair<string, string> kvp in reportPainColumns) {
                        headers[3 + i] = kvp.Value;
                        i++;
                    }
                    TypeCode[] codes = new TypeCode[3 + reportPainColumns.Count];

                    codes[0] = TypeCode.Int32;
                    codes[1] = TypeCode.String;
                    codes[2] = TypeCode.String;
                    int j = 0;
                    foreach (KeyValuePair<string, string> kvp in reportPainColumns) {
                        codes[3 + j] = TypeCode.Decimal;
                        j++;
                    }

                    float[] policyColumnsWidths = new float[headers.Count()];
                    policyColumnsWidths[0] = 5;
                    policyColumnsWidths[1] = 15;
                    policyColumnsWidths[2] = 15;
                    float widthPerItem = (float)65;
                    if (reportPainColumns.Count > 0) {
                        widthPerItem = (float)65 / (float)reportPainColumns.Count;
                    }
                    int k = 0;
                    foreach (KeyValuePair<string, string> kvp in reportPainColumns) {
                        policyColumnsWidths[3 + k] = widthPerItem;
                        k++;
                    }
                    pdf.CreateTableWithBorder(headers.Count(), false, headers, "", policyColumnsWidths);
                    if (totalType == 0) {
                        var queryGroupBy = (from t in list
                                            group t by new { t.applicationdate, t.insurancecompanyid }
                                                into grp
                                                select new {
                                                    grp.Key.applicationdate,
                                                    grp.Key.insurancecompanyid,
                                                    premiumvalue = grp.Sum(t => t.premiumvalue),
                                                    realpremiumvalue = grp.Sum(t => t.realpremiumvalue),
                                                    discountvalue = grp.Sum(t => t.discountvalue),
                                                    paidvalue = grp.Sum(t => t.paidvalue),
                                                    debtvalue = grp.Sum(t => t.debtvalue),
                                                    debtexpectedvalue = grp.Sum(t => t.debtexpectedvalue),
                                                    facturedbrokeragevalue = grp.Sum(c => c.facturedbrokeragevalue),
                                                    marketingagentvalue = grp.Sum(c => c.marketingagentvalue)
                                                }).ToList();
                        foreach (var c in queryGroupBy) {
                            object[] values = new object[5 + reportPainColumns.Count];
                            values[0] = counter;
                            values[1] = c.applicationdate.ToShortDateString();
                            values[2] = InsuranceCompany.Get(c.insurancecompanyid).ShortName;
                            int currCounter = 0;
                            foreach (KeyValuePair<string, string> kvp in reportPainColumns) {
                                if (kvp.Key == "RealPolicyValue") {
                                    values[3 + currCounter] = String.Format("{0:#,0.00}", c.realpremiumvalue);
                                }
                                if (kvp.Key == "PremiumValue") {
                                    values[3 + currCounter] = String.Format("{0:#,0.00}", c.premiumvalue);
                                }
                                if (kvp.Key == "PaidPremiumValue") {
                                    values[3 + currCounter] = String.Format("{0:#,0.00}", c.paidvalue);
                                }
                                if (kvp.Key == "DebtPremiumValue") {
                                    values[3 + currCounter] = String.Format("{0:#,0.00}", c.debtvalue);
                                }
                                if (kvp.Key == "DebtExpectedPremiumValue") {
                                    values[3 + currCounter] = String.Format("{0:#,0.00}", c.debtexpectedvalue);
                                }
                                if (kvp.Key == "FacturedBrokerageValue") {
                                    values[3 + currCounter] = String.Format("{0:#,0.00}", c.facturedbrokeragevalue);
                                }
                                if (kvp.Key == "MarkAgentProvisionValue") {
                                    values[3 + currCounter] = String.Format("{0:#,0.00}", c.marketingagentvalue);
                                }
                                currCounter++;
                            }
                            pdf.AddDataRowWithBorder(values, headers.Count(), codes);
                            counter++;
                        }
                    }
                    if (totalType == 1) {
                        var queryGroupBy = (from t in list
                                            group t by new { t.applicationdate.Year, t.applicationdate.Month, t.insurancecompanyid }
                                                into grp
                                                select new {
                                                    grp.Key.Year,
                                                    grp.Key.Month,
                                                    grp.Key.insurancecompanyid,
                                                    premiumvalue = grp.Sum(t => t.premiumvalue),
                                                    realpremiumvalue = grp.Sum(t => t.realpremiumvalue),
                                                    discountvalue = grp.Sum(t => t.discountvalue),
                                                    paidvalue = grp.Sum(t => t.paidvalue),
                                                    debtvalue = grp.Sum(t => t.debtvalue),
                                                    debtexpectedvalue = grp.Sum(t => t.debtexpectedvalue),
                                                    facturedbrokeragevalue = grp.Sum(c => c.facturedbrokeragevalue),
                                                    marketingagentvalue = grp.Sum(c => c.marketingagentvalue)
                                                }).ToList();
                        foreach (var c in queryGroupBy) {
                            object[] values = new object[5 + reportPainColumns.Count];
                            values[0] = counter;
                            values[1] = c.Month + "." + c.Year;
                            values[2] = InsuranceCompany.Get(c.insurancecompanyid).ShortName;
                            int currCounter = 0;
                            foreach (KeyValuePair<string, string> kvp in reportPainColumns) {
                                if (kvp.Key == "RealPolicyValue") {
                                    values[3 + currCounter] = String.Format("{0:#,0.00}", c.realpremiumvalue);
                                }
                                if (kvp.Key == "PremiumValue") {
                                    values[3 + currCounter] = String.Format("{0:#,0.00}", c.premiumvalue);
                                }
                                if (kvp.Key == "PaidPremiumValue") {
                                    values[3 + currCounter] = String.Format("{0:#,0.00}", c.paidvalue);
                                }
                                if (kvp.Key == "DebtPremiumValue") {
                                    values[3 + currCounter] = String.Format("{0:#,0.00}", c.debtvalue);
                                }
                                if (kvp.Key == "DebtExpectedPremiumValue") {
                                    values[3 + currCounter] = String.Format("{0:#,0.00}", c.debtexpectedvalue);
                                }
                                if (kvp.Key == "FacturedBrokerageValue") {
                                    values[3 + currCounter] = String.Format("{0:#,0.00}", c.facturedbrokeragevalue);
                                }
                                if (kvp.Key == "MarkAgentProvisionValue") {
                                    values[3 + currCounter] = String.Format("{0:#,0.00}", c.marketingagentvalue);
                                }
                                currCounter++;
                            }
                            pdf.AddDataRowWithBorder(values, headers.Count(), codes);
                            counter++;
                        }
                    }
                    if (totalType == 2) {
                        var queryGroupBy = (from t in list
                                            group t by new { t.applicationdate.Year, Quarter = (t.applicationdate.Month - 1) / 3 + 1, t.insurancecompanyid }
                                                into grp
                                                select new {
                                                    grp.Key.Year,
                                                    grp.Key.Quarter,
                                                    grp.Key.insurancecompanyid,
                                                    premiumvalue = grp.Sum(t => t.premiumvalue),
                                                    realpremiumvalue = grp.Sum(t => t.realpremiumvalue),
                                                    discountvalue = grp.Sum(t => t.discountvalue),
                                                    paidvalue = grp.Sum(t => t.paidvalue),
                                                    debtvalue = grp.Sum(t => t.debtvalue),
                                                    debtexpectedvalue = grp.Sum(t => t.debtexpectedvalue),
                                                    facturedbrokeragevalue = grp.Sum(c => c.facturedbrokeragevalue),
                                                    marketingagentvalue = grp.Sum(c => c.marketingagentvalue)
                                                }).ToList();
                        foreach (var c in queryGroupBy) {
                            object[] values = new object[5 + reportPainColumns.Count];
                            values[0] = counter;
                            values[1] = c.Quarter + " квартал " + c.Year;
                            values[2] = InsuranceCompany.Get(c.insurancecompanyid).ShortName;
                            int currCounter = 0;
                            foreach (KeyValuePair<string, string> kvp in reportPainColumns) {
                                if (kvp.Key == "RealPolicyValue") {
                                    values[3 + currCounter] = String.Format("{0:#,0.00}", c.realpremiumvalue);
                                }
                                if (kvp.Key == "PremiumValue") {
                                    values[3 + currCounter] = String.Format("{0:#,0.00}", c.premiumvalue);
                                }
                                if (kvp.Key == "PaidPremiumValue") {
                                    values[3 + currCounter] = String.Format("{0:#,0.00}", c.paidvalue);
                                }
                                if (kvp.Key == "DebtPremiumValue") {
                                    values[3 + currCounter] = String.Format("{0:#,0.00}", c.debtvalue);
                                }
                                if (kvp.Key == "DebtExpectedPremiumValue") {
                                    values[3 + currCounter] = String.Format("{0:#,0.00}", c.debtexpectedvalue);
                                }
                                if (kvp.Key == "FacturedBrokerageValue") {
                                    values[3 + currCounter] = String.Format("{0:#,0.00}", c.facturedbrokeragevalue);
                                }
                                if (kvp.Key == "MarkAgentProvisionValue") {
                                    values[3 + currCounter] = String.Format("{0:#,0.00}", c.marketingagentvalue);
                                }
                                currCounter++;
                            }
                            pdf.AddDataRowWithBorder(values, headers.Count(), codes);
                            counter++;
                        }
                    }
                }
                if (lstBranhces.Count == 0 && lstInsuranceCompanies.Count == 0 && lstInsuranceSubTypes.Count > 0 && lstUsers.Count == 0) {
                    string[] headers = new string[3 + reportPainColumns.Count];
                    headers[0] = "Р.бр.";
                    if (totalType == 0) {
                        headers[1] = "Датум";
                    }
                    if (totalType == 1) {
                        headers[1] = "Месец";
                    }
                    if (totalType == 2) {
                        headers[1] = "Квартал";
                    }
                    headers[2] = "Подкласа";
                    int i = 0;
                    foreach (KeyValuePair<string, string> kvp in reportPainColumns) {
                        headers[3 + i] = kvp.Value;
                        i++;
                    }
                    TypeCode[] codes = new TypeCode[3 + reportPainColumns.Count];

                    codes[0] = TypeCode.Int32;
                    codes[1] = TypeCode.String;
                    codes[2] = TypeCode.String;
                    int j = 0;
                    foreach (KeyValuePair<string, string> kvp in reportPainColumns) {
                        codes[3 + j] = TypeCode.Decimal;
                        j++;
                    }

                    float[] policyColumnsWidths = new float[headers.Count()];
                    policyColumnsWidths[0] = 5;
                    policyColumnsWidths[1] = 15;
                    policyColumnsWidths[2] = 15;
                    float widthPerItem = (float)65;
                    if (reportPainColumns.Count > 0) {
                        widthPerItem = (float)65 / (float)reportPainColumns.Count;
                    }
                    int k = 0;
                    foreach (KeyValuePair<string, string> kvp in reportPainColumns) {
                        policyColumnsWidths[3 + k] = widthPerItem;
                        k++;
                    }
                    pdf.CreateTableWithBorder(headers.Count(), false, headers, "", policyColumnsWidths);
                    if (totalType == 0) {
                        var queryGroupBy = (from t in list
                                            group t by new { t.applicationdate, t.insurancesubtypeid }
                                                into grp
                                                select new {
                                                    grp.Key.applicationdate,
                                                    grp.Key.insurancesubtypeid,
                                                    premiumvalue = grp.Sum(t => t.premiumvalue),
                                                    realpremiumvalue = grp.Sum(t => t.realpremiumvalue),
                                                    discountvalue = grp.Sum(t => t.discountvalue),
                                                    paidvalue = grp.Sum(t => t.paidvalue),
                                                    debtvalue = grp.Sum(t => t.debtvalue),
                                                    debtexpectedvalue = grp.Sum(t => t.debtexpectedvalue),
                                                    facturedbrokeragevalue = grp.Sum(c => c.facturedbrokeragevalue),
                                                    marketingagentvalue = grp.Sum(c => c.marketingagentvalue)
                                                }).ToList();
                        foreach (var c in queryGroupBy) {
                            object[] values = new object[3 + reportPainColumns.Count];
                            values[0] = counter;
                            values[1] = c.applicationdate.ToShortDateString();
                            values[2] = InsuranceSubType.Get(c.insurancesubtypeid).ShortDescription;
                            int currCounter = 0;
                            foreach (KeyValuePair<string, string> kvp in reportPainColumns) {
                                if (kvp.Key == "RealPolicyValue") {
                                    values[3 + currCounter] = String.Format("{0:#,0.00}", c.realpremiumvalue);
                                }
                                if (kvp.Key == "PremiumValue") {
                                    values[3 + currCounter] = String.Format("{0:#,0.00}", c.premiumvalue);
                                }
                                if (kvp.Key == "PaidPremiumValue") {
                                    values[3 + currCounter] = String.Format("{0:#,0.00}", c.paidvalue);
                                }
                                if (kvp.Key == "DebtPremiumValue") {
                                    values[3 + currCounter] = String.Format("{0:#,0.00}", c.debtvalue);
                                }
                                if (kvp.Key == "DebtExpectedPremiumValue") {
                                    values[3 + currCounter] = String.Format("{0:#,0.00}", c.debtexpectedvalue);
                                }
                                if (kvp.Key == "FacturedBrokerageValue") {
                                    values[3 + currCounter] = String.Format("{0:#,0.00}", c.facturedbrokeragevalue);
                                }
                                if (kvp.Key == "MarkAgentProvisionValue") {
                                    values[3 + currCounter] = String.Format("{0:#,0.00}", c.marketingagentvalue);
                                }
                                currCounter++;
                            }
                            pdf.AddDataRowWithBorder(values, headers.Count(), codes);
                            counter++;
                        }
                    }
                    if (totalType == 1) {
                        var queryGroupBy = (from t in list
                                            group t by new { t.applicationdate.Year, t.applicationdate.Month, t.insurancesubtypeid }
                                                into grp
                                                select new {
                                                    grp.Key.Year,
                                                    grp.Key.Month,
                                                    grp.Key.insurancesubtypeid,
                                                    premiumvalue = grp.Sum(t => t.premiumvalue),
                                                    realpremiumvalue = grp.Sum(t => t.realpremiumvalue),
                                                    discountvalue = grp.Sum(t => t.discountvalue),
                                                    paidvalue = grp.Sum(t => t.paidvalue),
                                                    debtvalue = grp.Sum(t => t.debtvalue),
                                                    debtexpectedvalue = grp.Sum(t => t.debtexpectedvalue),
                                                    facturedbrokeragevalue = grp.Sum(c => c.facturedbrokeragevalue),
                                                    marketingagentvalue = grp.Sum(c => c.marketingagentvalue)
                                                }).ToList();
                        foreach (var c in queryGroupBy) {
                            object[] values = new object[3 + reportPainColumns.Count];
                            values[0] = counter;
                            values[1] = c.Month + "." + c.Year;
                            values[2] = InsuranceSubType.Get(c.insurancesubtypeid).ShortDescription;
                            int currCounter = 0;
                            foreach (KeyValuePair<string, string> kvp in reportPainColumns) {
                                if (kvp.Key == "RealPolicyValue") {
                                    values[3 + currCounter] = String.Format("{0:#,0.00}", c.realpremiumvalue);
                                }
                                if (kvp.Key == "PremiumValue") {
                                    values[3 + currCounter] = String.Format("{0:#,0.00}", c.premiumvalue);
                                }
                                if (kvp.Key == "PaidPremiumValue") {
                                    values[3 + currCounter] = String.Format("{0:#,0.00}", c.paidvalue);
                                }
                                if (kvp.Key == "DebtPremiumValue") {
                                    values[3 + currCounter] = String.Format("{0:#,0.00}", c.debtvalue);
                                }
                                if (kvp.Key == "DebtExpectedPremiumValue") {
                                    values[3 + currCounter] = String.Format("{0:#,0.00}", c.debtexpectedvalue);
                                }
                                if (kvp.Key == "FacturedBrokerageValue") {
                                    values[3 + currCounter] = String.Format("{0:#,0.00}", c.facturedbrokeragevalue);
                                }
                                if (kvp.Key == "MarkAgentProvisionValue") {
                                    values[3 + currCounter] = String.Format("{0:#,0.00}", c.marketingagentvalue);
                                }
                                currCounter++;
                            }
                            pdf.AddDataRowWithBorder(values, headers.Count(), codes);
                            counter++;
                        }
                    }
                    if (totalType == 2) {
                        var queryGroupBy = (from t in list
                                            group t by new { t.applicationdate.Year, Quarter = (t.applicationdate.Month - 1) / 3 + 1, t.insurancesubtypeid }
                                                into grp
                                                select new {
                                                    grp.Key.Year,
                                                    grp.Key.Quarter,
                                                    grp.Key.insurancesubtypeid,
                                                    premiumvalue = grp.Sum(t => t.premiumvalue),
                                                    realpremiumvalue = grp.Sum(t => t.realpremiumvalue),
                                                    discountvalue = grp.Sum(t => t.discountvalue),
                                                    paidvalue = grp.Sum(t => t.paidvalue),
                                                    debtvalue = grp.Sum(t => t.debtvalue),
                                                    debtexpectedvalue = grp.Sum(t => t.debtexpectedvalue),
                                                    facturedbrokeragevalue = grp.Sum(c => c.facturedbrokeragevalue),
                                                    marketingagentvalue = grp.Sum(c => c.marketingagentvalue)
                                                }).ToList();
                        foreach (var c in queryGroupBy) {
                            object[] values = new object[3 + reportPainColumns.Count];
                            values[0] = counter;
                            values[1] = c.Quarter + " квартал " + c.Year;
                            values[2] = InsuranceSubType.Get(c.insurancesubtypeid).ShortDescription;
                            int currCounter = 0;
                            foreach (KeyValuePair<string, string> kvp in reportPainColumns) {
                                if (kvp.Key == "RealPolicyValue") {
                                    values[3 + currCounter] = String.Format("{0:#,0.00}", c.realpremiumvalue);
                                }
                                if (kvp.Key == "PremiumValue") {
                                    values[3 + currCounter] = String.Format("{0:#,0.00}", c.premiumvalue);
                                }
                                if (kvp.Key == "PaidPremiumValue") {
                                    values[3 + currCounter] = String.Format("{0:#,0.00}", c.paidvalue);
                                }
                                if (kvp.Key == "DebtPremiumValue") {
                                    values[3 + currCounter] = String.Format("{0:#,0.00}", c.debtvalue);
                                }
                                if (kvp.Key == "DebtExpectedPremiumValue") {
                                    values[3 + currCounter] = String.Format("{0:#,0.00}", c.debtexpectedvalue);
                                }
                                if (kvp.Key == "FacturedBrokerageValue") {
                                    values[3 + currCounter] = String.Format("{0:#,0.00}", c.facturedbrokeragevalue);
                                }
                                if (kvp.Key == "MarkAgentProvisionValue") {
                                    values[3 + currCounter] = String.Format("{0:#,0.00}", c.marketingagentvalue);
                                }
                                currCounter++;
                            }
                            pdf.AddDataRowWithBorder(values, headers.Count(), codes);
                            counter++;
                        }
                    }
                }
                if (lstBranhces.Count == 0 && lstInsuranceCompanies.Count == 0 && lstInsuranceSubTypes.Count == 0 && lstUsers.Count > 0) {
                    string[] headers = new string[3 + reportPainColumns.Count];
                    headers[0] = "Р.бр.";
                    if (totalType == 0) {
                        headers[1] = "Датум";
                    }
                    if (totalType == 1) {
                        headers[1] = "Месец";
                    }
                    if (totalType == 2) {
                        headers[1] = "Квартал";
                    }
                    headers[2] = "Корисник";
                    int i = 0;
                    foreach (KeyValuePair<string, string> kvp in reportPainColumns) {
                        headers[3 + i] = kvp.Value;
                        i++;
                    }
                    TypeCode[] codes = new TypeCode[3 + reportPainColumns.Count];

                    codes[0] = TypeCode.Int32;
                    codes[1] = TypeCode.String;
                    codes[2] = TypeCode.String;
                    int j = 0;
                    foreach (KeyValuePair<string, string> kvp in reportPainColumns) {
                        codes[3 + j] = TypeCode.Decimal;
                        j++;
                    }

                    float[] policyColumnsWidths = new float[headers.Count()];
                    policyColumnsWidths[0] = 5;
                    policyColumnsWidths[1] = 15;
                    policyColumnsWidths[2] = 15;
                    float widthPerItem = (float)65;
                    if (reportPainColumns.Count > 0) {
                        widthPerItem = (float)65 / (float)reportPainColumns.Count;
                    }
                    int k = 0;
                    foreach (KeyValuePair<string, string> kvp in reportPainColumns) {
                        policyColumnsWidths[3 + k] = widthPerItem;
                        k++;
                    }
                    pdf.CreateTableWithBorder(headers.Count(), false, headers, "", policyColumnsWidths);
                    if (totalType == 0) {
                        var queryGroupBy = (from t in list
                                            group t by new { t.applicationdate, t.userid }
                                                into grp
                                                select new {
                                                    grp.Key.applicationdate,
                                                    grp.Key.userid,
                                                    premiumvalue = grp.Sum(t => t.premiumvalue),
                                                    realpremiumvalue = grp.Sum(t => t.realpremiumvalue),
                                                    discountvalue = grp.Sum(t => t.discountvalue),
                                                    paidvalue = grp.Sum(t => t.paidvalue),
                                                    debtvalue = grp.Sum(t => t.debtvalue),
                                                    debtexpectedvalue = grp.Sum(t => t.debtexpectedvalue),
                                                    facturedbrokeragevalue = grp.Sum(c => c.facturedbrokeragevalue),
                                                    marketingagentvalue = grp.Sum(c => c.marketingagentvalue)
                                                }).ToList();
                        foreach (var c in queryGroupBy) {
                            object[] values = new object[3 + reportPainColumns.Count];
                            values[0] = counter;
                            values[1] = c.applicationdate.ToShortDateString();
                            values[2] = Broker.DataAccess.User.Get(c.userid).Name;
                            int currCounter = 0;
                            foreach (KeyValuePair<string, string> kvp in reportPainColumns) {
                                if (kvp.Key == "RealPolicyValue") {
                                    values[3 + currCounter] = String.Format("{0:#,0.00}", c.realpremiumvalue);
                                }
                                if (kvp.Key == "PremiumValue") {
                                    values[3 + currCounter] = String.Format("{0:#,0.00}", c.premiumvalue);
                                }
                                if (kvp.Key == "PaidPremiumValue") {
                                    values[3 + currCounter] = String.Format("{0:#,0.00}", c.paidvalue);
                                }
                                if (kvp.Key == "DebtPremiumValue") {
                                    values[3 + currCounter] = String.Format("{0:#,0.00}", c.debtvalue);
                                }
                                if (kvp.Key == "DebtExpectedPremiumValue") {
                                    values[3 + currCounter] = String.Format("{0:#,0.00}", c.debtexpectedvalue);
                                }
                                if (kvp.Key == "FacturedBrokerageValue") {
                                    values[3 + currCounter] = String.Format("{0:#,0.00}", c.facturedbrokeragevalue);
                                }
                                if (kvp.Key == "MarkAgentProvisionValue") {
                                    values[3 + currCounter] = String.Format("{0:#,0.00}", c.marketingagentvalue);
                                }
                                currCounter++;
                            }
                            pdf.AddDataRowWithBorder(values, headers.Count(), codes);
                            counter++;
                        }
                    }
                    if (totalType == 1) {
                        var queryGroupBy = (from t in list
                                            group t by new { t.applicationdate.Year, t.applicationdate.Month, t.userid }
                                                into grp
                                                select new {
                                                    grp.Key.Year,
                                                    grp.Key.Month,
                                                    grp.Key.userid,
                                                    premiumvalue = grp.Sum(t => t.premiumvalue),
                                                    realpremiumvalue = grp.Sum(t => t.realpremiumvalue),
                                                    discountvalue = grp.Sum(t => t.discountvalue),
                                                    paidvalue = grp.Sum(t => t.paidvalue),
                                                    debtvalue = grp.Sum(t => t.debtvalue),
                                                    debtexpectedvalue = grp.Sum(t => t.debtexpectedvalue),
                                                    facturedbrokeragevalue = grp.Sum(c => c.facturedbrokeragevalue),
                                                    marketingagentvalue = grp.Sum(c => c.marketingagentvalue)
                                                }).ToList();
                        foreach (var c in queryGroupBy) {
                            object[] values = new object[3 + reportPainColumns.Count];
                            values[0] = counter;
                            values[1] = c.Month + "." + c.Year;
                            values[2] = Broker.DataAccess.User.Get(c.userid).Name;
                            int currCounter = 0;
                            foreach (KeyValuePair<string, string> kvp in reportPainColumns) {
                                if (kvp.Key == "RealPolicyValue") {
                                    values[3 + currCounter] = String.Format("{0:#,0.00}", c.realpremiumvalue);
                                }
                                if (kvp.Key == "PremiumValue") {
                                    values[3 + currCounter] = String.Format("{0:#,0.00}", c.premiumvalue);
                                }
                                if (kvp.Key == "PaidPremiumValue") {
                                    values[3 + currCounter] = String.Format("{0:#,0.00}", c.paidvalue);
                                }
                                if (kvp.Key == "DebtPremiumValue") {
                                    values[3 + currCounter] = String.Format("{0:#,0.00}", c.debtvalue);
                                }
                                if (kvp.Key == "DebtExpectedPremiumValue") {
                                    values[3 + currCounter] = String.Format("{0:#,0.00}", c.debtexpectedvalue);
                                }
                                if (kvp.Key == "FacturedBrokerageValue") {
                                    values[3 + currCounter] = String.Format("{0:#,0.00}", c.facturedbrokeragevalue);
                                }
                                if (kvp.Key == "MarkAgentProvisionValue") {
                                    values[3 + currCounter] = String.Format("{0:#,0.00}", c.marketingagentvalue);
                                }
                                currCounter++;
                            }
                            pdf.AddDataRowWithBorder(values, headers.Count(), codes);
                            counter++;
                        }
                    }
                    if (totalType == 2) {
                        var queryGroupBy = (from t in list
                                            group t by new { t.applicationdate.Year, Quarter = (t.applicationdate.Month - 1) / 3 + 1, t.userid }
                                                into grp
                                                select new {
                                                    grp.Key.Year,
                                                    grp.Key.Quarter,
                                                    grp.Key.userid,
                                                    premiumvalue = grp.Sum(t => t.premiumvalue),
                                                    realpremiumvalue = grp.Sum(t => t.realpremiumvalue),
                                                    discountvalue = grp.Sum(t => t.discountvalue),
                                                    paidvalue = grp.Sum(t => t.paidvalue),
                                                    debtvalue = grp.Sum(t => t.debtvalue),
                                                    debtexpectedvalue = grp.Sum(t => t.debtexpectedvalue),
                                                    facturedbrokeragevalue = grp.Sum(c => c.facturedbrokeragevalue),
                                                    marketingagentvalue = grp.Sum(c => c.marketingagentvalue)
                                                }).ToList();
                        foreach (var c in queryGroupBy) {
                            object[] values = new object[3 + reportPainColumns.Count];
                            values[0] = counter;
                            values[1] = c.Quarter + " квартал " + c.Year;
                            values[2] = Broker.DataAccess.User.Get(c.userid).Name;
                            int currCounter = 0;
                            foreach (KeyValuePair<string, string> kvp in reportPainColumns) {
                                if (kvp.Key == "RealPolicyValue") {
                                    values[3 + currCounter] = String.Format("{0:#,0.00}", c.realpremiumvalue);
                                }
                                if (kvp.Key == "PremiumValue") {
                                    values[3 + currCounter] = String.Format("{0:#,0.00}", c.premiumvalue);
                                }
                                if (kvp.Key == "PaidPremiumValue") {
                                    values[3 + currCounter] = String.Format("{0:#,0.00}", c.paidvalue);
                                }
                                if (kvp.Key == "DebtPremiumValue") {
                                    values[3 + currCounter] = String.Format("{0:#,0.00}", c.debtvalue);
                                }
                                if (kvp.Key == "DebtExpectedPremiumValue") {
                                    values[3 + currCounter] = String.Format("{0:#,0.00}", c.debtexpectedvalue);
                                }
                                if (kvp.Key == "FacturedBrokerageValue") {
                                    values[3 + currCounter] = String.Format("{0:#,0.00}", c.facturedbrokeragevalue);
                                }
                                if (kvp.Key == "MarkAgentProvisionValue") {
                                    values[3 + currCounter] = String.Format("{0:#,0.00}", c.marketingagentvalue);
                                }
                                currCounter++;
                            }
                            pdf.AddDataRowWithBorder(values, headers.Count(), codes);
                            counter++;
                        }
                    }
                }
                if (lstBranhces.Count == 0 && lstInsuranceCompanies.Count == 0 && lstInsuranceSubTypes.Count == 0 && lstUsers.Count == 0) {
                    string[] headers = new string[2 + reportPainColumns.Count];
                    headers[0] = "Р.бр.";
                    if (totalType == 0) {
                        headers[1] = "Датум";
                    }
                    if (totalType == 1) {
                        headers[1] = "Месец";
                    }
                    if (totalType == 2) {
                        headers[1] = "Квартал";
                    }
                    int i = 0;
                    foreach (KeyValuePair<string, string> kvp in reportPainColumns) {
                        headers[2 + i] = kvp.Value;
                        i++;
                    }
                    TypeCode[] codes = new TypeCode[2 + reportPainColumns.Count];

                    codes[0] = TypeCode.Int32;
                    codes[1] = TypeCode.String;
                    int j = 0;
                    foreach (KeyValuePair<string, string> kvp in reportPainColumns) {
                        codes[2 + j] = TypeCode.Decimal;
                        j++;
                    }

                    float[] policyColumnsWidths = new float[headers.Count()];
                    policyColumnsWidths[0] = 5;
                    policyColumnsWidths[1] = 15;
                    float widthPerItem = (float)80;
                    if (reportPainColumns.Count > 0) {
                        widthPerItem = (float)80 / (float)reportPainColumns.Count;
                    }
                    int k = 0;
                    foreach (KeyValuePair<string, string> kvp in reportPainColumns) {
                        policyColumnsWidths[2 + k] = widthPerItem;
                        k++;
                    }
                    pdf.CreateTableWithBorder(headers.Count(), false, headers, "", policyColumnsWidths);
                    if (totalType == 0) {
                        var queryGroupBy = (from t in list
                                            group t by new { t.applicationdate }
                                                into grp
                                                select new {
                                                    grp.Key.applicationdate,
                                                    premiumvalue = grp.Sum(t => t.premiumvalue),
                                                    realpremiumvalue = grp.Sum(t => t.realpremiumvalue),
                                                    discountvalue = grp.Sum(t => t.discountvalue),
                                                    paidvalue = grp.Sum(t => t.paidvalue),
                                                    debtvalue = grp.Sum(t => t.debtvalue),
                                                    debtexpectedvalue = grp.Sum(t => t.debtexpectedvalue),
                                                    facturedbrokeragevalue = grp.Sum(c => c.facturedbrokeragevalue),
                                                    marketingagentvalue = grp.Sum(c => c.marketingagentvalue)
                                                }).ToList();
                        foreach (var c in queryGroupBy) {
                            object[] values = new object[2 + reportPainColumns.Count];
                            values[0] = counter;
                            values[1] = c.applicationdate.ToShortDateString();
                            int currCounter = 0;
                            foreach (KeyValuePair<string, string> kvp in reportPainColumns) {
                                if (kvp.Key == "RealPolicyValue") {
                                    values[2 + currCounter] = String.Format("{0:#,0.00}", c.realpremiumvalue);
                                }
                                if (kvp.Key == "PremiumValue") {
                                    values[2 + currCounter] = String.Format("{0:#,0.00}", c.premiumvalue);
                                }
                                if (kvp.Key == "PaidPremiumValue") {
                                    values[2 + currCounter] = String.Format("{0:#,0.00}", c.paidvalue);
                                }
                                if (kvp.Key == "DebtPremiumValue") {
                                    values[2 + currCounter] = String.Format("{0:#,0.00}", c.debtvalue);
                                }
                                if (kvp.Key == "DebtExpectedPremiumValue") {
                                    values[2 + currCounter] = String.Format("{0:#,0.00}", c.debtexpectedvalue);
                                }
                                if (kvp.Key == "FacturedBrokerageValue") {
                                    values[2 + currCounter] = String.Format("{0:#,0.00}", c.facturedbrokeragevalue);
                                }
                                if (kvp.Key == "MarkAgentProvisionValue") {
                                    values[2 + currCounter] = String.Format("{0:#,0.00}", c.marketingagentvalue);
                                }
                                currCounter++;
                            }
                            pdf.AddDataRowWithBorder(values, headers.Count(), codes);
                            counter++;
                        }
                    }
                    if (totalType == 1) {
                        var queryGroupBy = (from t in list
                                            group t by new { t.applicationdate.Year, t.applicationdate.Month }
                                                into grp
                                                select new {
                                                    grp.Key.Year,
                                                    grp.Key.Month,
                                                    premiumvalue = grp.Sum(t => t.premiumvalue),
                                                    realpremiumvalue = grp.Sum(t => t.realpremiumvalue),
                                                    discountvalue = grp.Sum(t => t.discountvalue),
                                                    paidvalue = grp.Sum(t => t.paidvalue),
                                                    debtvalue = grp.Sum(t => t.debtvalue),
                                                    debtexpectedvalue = grp.Sum(t => t.debtexpectedvalue),
                                                    facturedbrokeragevalue = grp.Sum(c => c.facturedbrokeragevalue),
                                                    marketingagentvalue = grp.Sum(c => c.marketingagentvalue)
                                                }).ToList();
                        foreach (var c in queryGroupBy) {
                            object[] values = new object[2 + reportPainColumns.Count];
                            values[0] = counter;
                            values[1] = c.Month + "." + c.Year;
                            int currCounter = 0;
                            foreach (KeyValuePair<string, string> kvp in reportPainColumns) {
                                if (kvp.Key == "RealPolicyValue") {
                                    values[2 + currCounter] = String.Format("{0:#,0.00}", c.realpremiumvalue);
                                }
                                if (kvp.Key == "PremiumValue") {
                                    values[2 + currCounter] = String.Format("{0:#,0.00}", c.premiumvalue);
                                }
                                if (kvp.Key == "PaidPremiumValue") {
                                    values[2 + currCounter] = String.Format("{0:#,0.00}", c.paidvalue);
                                }
                                if (kvp.Key == "DebtPremiumValue") {
                                    values[2 + currCounter] = String.Format("{0:#,0.00}", c.debtvalue);
                                }
                                if (kvp.Key == "DebtExpectedPremiumValue") {
                                    values[2 + currCounter] = String.Format("{0:#,0.00}", c.debtexpectedvalue);
                                }
                                if (kvp.Key == "FacturedBrokerageValue") {
                                    values[2 + currCounter] = String.Format("{0:#,0.00}", c.facturedbrokeragevalue);
                                }
                                if (kvp.Key == "MarkAgentProvisionValue") {
                                    values[2 + currCounter] = String.Format("{0:#,0.00}", c.marketingagentvalue);
                                }
                                currCounter++;
                            }
                            pdf.AddDataRowWithBorder(values, headers.Count(), codes);
                            counter++;
                        }
                    }
                    if (totalType == 2) {
                        var queryGroupBy = (from t in list
                                            group t by new { t.applicationdate.Year, Quarter = (t.applicationdate.Month - 1) / 3 + 1 }
                                                into grp
                                                select new {
                                                    grp.Key.Year,
                                                    grp.Key.Quarter,
                                                    premiumvalue = grp.Sum(t => t.premiumvalue),
                                                    realpremiumvalue = grp.Sum(t => t.realpremiumvalue),
                                                    discountvalue = grp.Sum(t => t.discountvalue),
                                                    paidvalue = grp.Sum(t => t.paidvalue),
                                                    debtvalue = grp.Sum(t => t.debtvalue),
                                                    debtexpectedvalue = grp.Sum(t => t.debtexpectedvalue),
                                                    facturedbrokeragevalue = grp.Sum(c => c.facturedbrokeragevalue),
                                                    marketingagentvalue = grp.Sum(c => c.marketingagentvalue)
                                                }).ToList();
                        foreach (var c in queryGroupBy) {
                            object[] values = new object[2 + reportPainColumns.Count];
                            values[0] = counter;
                            values[1] = c.Quarter + " квартал " + c.Year;
                            int currCounter = 0;
                            foreach (KeyValuePair<string, string> kvp in reportPainColumns) {
                                if (kvp.Key == "RealPolicyValue") {
                                    values[2 + currCounter] = String.Format("{0:#,0.00}", c.realpremiumvalue);
                                }
                                if (kvp.Key == "PremiumValue") {
                                    values[2 + currCounter] = String.Format("{0:#,0.00}", c.premiumvalue);
                                }
                                if (kvp.Key == "PaidPremiumValue") {
                                    values[2 + currCounter] = String.Format("{0:#,0.00}", c.paidvalue);
                                }
                                if (kvp.Key == "DebtPremiumValue") {
                                    values[2 + currCounter] = String.Format("{0:#,0.00}", c.debtvalue);
                                }
                                if (kvp.Key == "DebtExpectedPremiumValue") {
                                    values[2 + currCounter] = String.Format("{0:#,0.00}", c.debtexpectedvalue);
                                }
                                if (kvp.Key == "FacturedBrokerageValue") {
                                    values[2 + currCounter] = String.Format("{0:#,0.00}", c.facturedbrokeragevalue);
                                }
                                if (kvp.Key == "MarkAgentProvisionValue") {
                                    values[2 + currCounter] = String.Format("{0:#,0.00}", c.marketingagentvalue);
                                }
                                currCounter++;
                            }
                            pdf.AddDataRowWithBorder(values, headers.Count(), codes);
                            counter++;
                        }
                    }
                }
                pdf.AddTable();
                pdf.FinishPDF();
            }

        }


        public static string GetFromIDs(List<int> genList) {
            string retString = "";
            for (int i = 0; i < genList.Count; i++) {
                if (i < genList.Count - 1) {
                    retString += genList[i].ToString() + ",";
                } else {
                    retString += genList[i].ToString();
                }
            }
            return retString;
        }

    }

    public class SummaryForPoedinecno {
        public int ID { get; set; }
        public string policynumber { get; set; }
        public DateTime applicationdate { get; set; }
        public string insurancesubtypename { get; set; }
        public string insurancecompanyname { get; set; }
        public string clientname { get; set; }
        public decimal realpremiumvalue { get; set; }
        public decimal premiumvalue { get; set; }
        public decimal discountvalue { get; set; }
        public decimal paidvalue { get; set; }
        public decimal debtvalue { get; set; }
        public decimal debtexpectedvalue { get; set; }
        public decimal paidvaluepercent { get; set; }
        public decimal facturedbrokeragevalue { get; set; }
        public decimal marketingagentvalue { get; set; }
    }

    public class SummaryForGroup {
        public int ID { get; set; }
        public string policynumber { get; set; }
        public DateTime applicationdate { get; set; }
        public int insurancesubtypeid { get; set; }
        public int insurancecompanyid { get; set; }
        public int userid { get; set; }
        public int branchid { get; set; }
        public decimal realpremiumvalue { get; set; }
        public decimal premiumvalue { get; set; }
        public decimal discountvalue { get; set; }
        public decimal paidvalue { get; set; }
        public decimal debtvalue { get; set; }
        public decimal debtexpectedvalue { get; set; }
        public decimal facturedbrokeragevalue { get; set; }
        public decimal marketingagentvalue { get; set; }
    }
}
