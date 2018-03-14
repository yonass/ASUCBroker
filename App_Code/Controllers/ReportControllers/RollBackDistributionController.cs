using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Broker.DataAccess;
using System.Web.UI.WebControls;
using Broker.Controllers.DistributionControllers;
/// <summary>
/// Summary description for RollBackDistributionController
/// </summary>
namespace Broker.Controllers.ReportControllers {
    public class RollBackDistributionController {
        public static void CreateFileForRollBack(RollBackDistribution rbd, bool isRollBacked) {
            int isRollBackedInt = 0;
            if (isRollBacked) {
                isRollBackedInt = 1;
            } else {
                isRollBackedInt = 0;
            }
            DataClassesDataContext dcdc = new DataClassesDataContext();
            DateTime dt1 = rbd.StartDate;
            DateTime dt2 = rbd.EndDate;
            int d1 = dt1.Day;
            int m1 = dt1.Month;
            int y1 = dt1.Year;
            int d2 = dt2.Day;
            int m2 = dt2.Month;
            int y2 = dt2.Year;

            PDFCreators pdf = new PDFCreators(false, 25, 25, 25, 25);
            pdf.OpenPDF();
            pdf.SetTitle("  ");
            pdf.SetTitle("  ");
            pdf.AddJDBLogo(650, 480);

            pdf.SetTitle("Раздолжување за период " + rbd.StartDate.ToShortDateString() + " - " + rbd.EndDate.ToShortDateString());
            InsuranceCompany ic = InsuranceCompany.Get(rbd.InsuranceCompanyID);
            pdf.SetTitleLeft(ic.Name);
            pdf.SetTitleLeft("");

            List<InsuranceSubType> istList = InsuranceSubType.GetAllSorted();
            int defaultHeadersCount = 9;

            Dictionary<int, int> listCountPerInsuranceSubType = new Dictionary<int, int>();
            Dictionary<int, decimal> listSumPerInsuranceSubType = new Dictionary<int, decimal>();

            foreach (InsuranceSubType ist in istList) {
                string query = @"select policyItems.policynumber, clients.name, policies.startdate, policies.enddate, insuranceSubTypes.description as insurancesubtypedescription , policyitems.premiumvalue,policies.discard as discard,policyItems.ID as policyItemID,policyItems.IspaidInBrokerHouse as paidInBrokersHouse
                             from clients INNER JOIN policies ON policies.ownerid = clients.id 
                             INNER JOIN policyItems ON policyItems.policyid = policies.id 
                             INNER JOIN insuranceSubTypes ON policyItems.InsuranceSubTypeID = insuranceSubTypes.ID
                             where policies.applicationdate >='" + y1 + -+m1 + -+d1 + "' and policies.applicationdate<='" + y2 + -+m2 + -+d2 + "' and policies.InsuranceCompanyID=" + rbd.InsuranceCompanyID.ToString() + "and policyItems.insuranceSubTypeID=" + ist.ID + "and policyItems.IsRollBacked=" + isRollBackedInt;
                ;
                IEnumerable<RollBackClass> rollBackClassIenum = dcdc.ExecuteQuery<RollBackClass>(query);
                List<RollBackClass> rollBackClassList = rollBackClassIenum.ToList();

                if (rollBackClassList.Count != 0) {
                    rollBackClassList = rollBackClassList.OrderBy(c => c.paidInBrokersHouse).ThenBy(c => c.policynumber).ToList();
                    List<Control> controlsList = Control.GetForReportByInsuranceSubType(ist.ID);
                    int additionalHeadersCount = controlsList.Count;
                    string[] headersP = new string[defaultHeadersCount + additionalHeadersCount];
                    int orderNumber = 0;
                    decimal partialValue = 0;
                    decimal basicPartialValue = 0;
                    decimal basicPremiumValue = 0;
                    headersP[0] = "РБ";
                    headersP[1] = "Број на полиса";
                    headersP[2] = "Осигуреник";
                    headersP[3] = "Почеток";
                    headersP[4] = "Истек";
                    headersP[5] = "Статус";
                    headersP[6] = "Тип на плаќање";
                    headersP[7] = "Премија";
                    headersP[8] = "Основна Премија";
                    float[] policyColumnsWidths = new float[defaultHeadersCount + additionalHeadersCount];
                    policyColumnsWidths[0] = 3;
                    policyColumnsWidths[1] = 7;
                    policyColumnsWidths[2] = 18;
                    policyColumnsWidths[3] = 7;
                    policyColumnsWidths[4] = 7;
                    policyColumnsWidths[5] = 7;
                    policyColumnsWidths[6] = 7;
                    policyColumnsWidths[7] = 7;
                    policyColumnsWidths[8] = 7;
                    TypeCode[] codes = new TypeCode[defaultHeadersCount + additionalHeadersCount];
                    codes[0] = TypeCode.String;
                    codes[1] = TypeCode.String;
                    codes[2] = TypeCode.String;
                    codes[3] = TypeCode.DateTime;
                    codes[4] = TypeCode.DateTime;
                    codes[5] = TypeCode.String;
                    codes[6] = TypeCode.String;
                    codes[7] = TypeCode.Decimal;
                    codes[8] = TypeCode.Decimal;
                    for (int i = 0; i < additionalHeadersCount; i++) {
                        codes[defaultHeadersCount + i] = TypeCode.Decimal;
                    }

                    for (int i = 0; i < additionalHeadersCount; i++) {
                        headersP[defaultHeadersCount + i] = controlsList[i].Description;
                        policyColumnsWidths[defaultHeadersCount + i] = 30 / additionalHeadersCount;
                    }

                    pdf.SetTitleLeft(ist.Code + " - " + ist.Description);
                    pdf.CreateTable(defaultHeadersCount + additionalHeadersCount, false, headersP, "", policyColumnsWidths);
                    object[] vals;
                    decimal[] additionalValues = new decimal[additionalHeadersCount];

                    int countPerInsuranceSubType = 0;
                    foreach (RollBackClass rbc in rollBackClassList) {
                        orderNumber++;
                        basicPartialValue = rbc.premiumvalue;
                        vals = new object[defaultHeadersCount + additionalHeadersCount];
                        vals[0] = orderNumber;
                        vals[1] = rbc.policynumber;
                        vals[2] = rbc.name;
                        vals[3] = rbc.startdate.ToShortDateString();
                        vals[4] = rbc.enddate.ToShortDateString();
                        if (rbc.paidInBrokersHouse) {
                            List<Rate> ratesList = Rate.GetByPolicyItemID(rbc.policyItemID);
                            vals[6] = "Еднократно";
                            if (ratesList.Count > 1) {
                                vals[6] = "На рати";
                            }
                        } else {
                            vals[6] = "Директно во о. ком.";
                        }
                        if (rbc.discard) {
                            vals[5] = "Сторно";
                            vals[7] = String.Format("{0:#,0.00}", 0);
                            vals[8] = String.Format("{0:#,0.00}", 0);
                            basicPartialValue = 0;
                        } else {
                            vals[5] = "Активна";
                            countPerInsuranceSubType++;
                            partialValue += rbc.premiumvalue;
                            vals[7] = String.Format("{0:#,0.00}", rbc.premiumvalue);

                        }

                        for (int i = 0; i < additionalHeadersCount; i++) {
                            PolicyExtendInformation pei = PolicyExtendInformation.GetByPolicyItemAndControl(rbc.policyItemID, controlsList[i].ID);
                            if (pei != null) {
                                if (pei.Value != string.Empty && !rbc.discard) {
                                    additionalValues[i] += decimal.Parse(pei.Value);
                                    vals[defaultHeadersCount + i] = String.Format("{0:#,0.00}", decimal.Parse(pei.Value));
                                    if (controlsList[i].IsPositive != null) {
                                        if ((bool)controlsList[i].IsPositive) {
                                            basicPartialValue += decimal.Parse(pei.Value);
                                        } else {
                                            basicPartialValue -= decimal.Parse(pei.Value);
                                        }
                                    }
                                } else {
                                    vals[defaultHeadersCount + i] = String.Format("{0:#,0.00}", 0);
                                }
                            } else {
                                ValidationDataType vt = Broker.DataAccess.VariableType.GetForVariableType(controlsList[i].VariableTypeID);
                                if (vt == ValidationDataType.String || vt == ValidationDataType.Date) {
                                    vals[defaultHeadersCount + i] = "";
                                } else {
                                    vals[defaultHeadersCount + i] = String.Format("{0:#,0.00}", 0);
                                }
                            }
                        }
                        vals[8] = String.Format("{0:#,0.00}", basicPartialValue);
                        basicPremiumValue += basicPartialValue;

                        pdf.AddDataRow1(vals, additionalHeadersCount + defaultHeadersCount, codes);
                    }
                    vals = new object[additionalHeadersCount + defaultHeadersCount];
                    vals[0] = "";
                    vals[1] = "";
                    vals[2] = "";
                    vals[3] = "";
                    vals[4] = "";
                    vals[5] = "";
                    vals[6] = "Вкупно";
                    vals[7] = String.Format("{0:#,0.00}", partialValue);
                    vals[8] = String.Format("{0:#,0.00}", basicPremiumValue);
                    listCountPerInsuranceSubType.Add(ist.ID, countPerInsuranceSubType);
                    listSumPerInsuranceSubType.Add(ist.ID, partialValue);
                    for (int i = 0; i < additionalHeadersCount; i++) {
                        vals[defaultHeadersCount + i] = String.Format("{0:#,0.00}", additionalValues[i]);
                    }
                    pdf.AddDataRow1(vals, additionalHeadersCount + defaultHeadersCount, codes);
                    pdf.AddTable();
                }

            }

            //Total
            pdf.NewPage();
            pdf.SetTitle("Рекапитулација:");
            string[] headersTotalP = new string[5];
            headersTotalP[0] = "РБ";
            headersTotalP[1] = "Шифра";
            headersTotalP[2] = "Подкласа";
            headersTotalP[3] = "Број на полиси";
            headersTotalP[4] = "Сума";
            float[] policyColumnsTotalWidths = new float[5];
            policyColumnsTotalWidths[0] = 5;
            policyColumnsTotalWidths[1] = 7;
            policyColumnsTotalWidths[2] = 50;
            policyColumnsTotalWidths[3] = 15;
            policyColumnsTotalWidths[4] = 23;
            TypeCode[] codesTotal = new TypeCode[5];
            codesTotal[0] = TypeCode.Int32;
            codesTotal[1] = TypeCode.String;
            codesTotal[2] = TypeCode.String;
            codesTotal[3] = TypeCode.Int32;
            codesTotal[4] = TypeCode.Decimal;
            pdf.CreateTable(5, false, headersTotalP, "", policyColumnsTotalWidths);
            object[] valsTotal;
            int counterTotal = 1;
            int totalPolicies = 0;
            decimal totalSum = 0;
            foreach (KeyValuePair<int, decimal> keyValuePair in listSumPerInsuranceSubType) {
                InsuranceSubType insuranceSubType = InsuranceSubType.Get(keyValuePair.Key);
                valsTotal = new object[5];
                valsTotal[0] = counterTotal.ToString();
                valsTotal[1] = insuranceSubType.Code;
                valsTotal[2] = insuranceSubType.Description;
                valsTotal[3] = listCountPerInsuranceSubType[keyValuePair.Key];
                totalPolicies += Convert.ToInt32(valsTotal[3]);
                valsTotal[4] = String.Format("{0:#,0.00}", keyValuePair.Value);
                totalSum += keyValuePair.Value;
                pdf.AddDataRow1(valsTotal, 5, codesTotal);
                counterTotal++;
            }
            valsTotal = new object[5];
            valsTotal[0] = "";
            valsTotal[1] = "";
            valsTotal[2] = "Вкупно";
            valsTotal[3] = totalPolicies;
            valsTotal[4] = String.Format("{0:#,0.00}", totalSum);
            pdf.AddDataRow1(valsTotal, 5, codesTotal);
            pdf.AddTable();

            //Total per payments
            pdf.NewPage();
            pdf.SetTitle("Збирна статистика:");
            string[] headersTotalPayments = new string[4];
            headersTotalPayments[0] = "РБ";
            headersTotalPayments[1] = "Тип на плаќање";
            headersTotalPayments[2] = "Број на полиси";
            headersTotalPayments[3] = "Сума";
            float[] policyColumnsTotalPaymentsWidths = new float[4];
            policyColumnsTotalPaymentsWidths[0] = 5;
            policyColumnsTotalPaymentsWidths[1] = 65;
            policyColumnsTotalPaymentsWidths[2] = 15;
            policyColumnsTotalPaymentsWidths[3] = 15;
            TypeCode[] codesTotalPayments = new TypeCode[4];
            codesTotal[0] = TypeCode.Int32;
            codesTotal[1] = TypeCode.String;
            codesTotal[2] = TypeCode.Int32;
            codesTotal[3] = TypeCode.Decimal;
            pdf.CreateTable(4, false, headersTotalPayments, "", policyColumnsTotalPaymentsWidths);
            int totalPoliciesPayments = 0;
            decimal totalSumPayments = 0;
            object[] valsTotalPayments = new object[4];
            valsTotalPayments[0] = "1.";
            valsTotalPayments[1] = "Готовина";
            int countCashPayments = 0;
            decimal totalCashPaymens = PolicyItem.GetPremiumValueForSummuryRollBacks(isRollBacked, dt1, dt2, true, rbd.InsuranceCompanyID, null, out countCashPayments);
            valsTotalPayments[2] = countCashPayments;
            totalPoliciesPayments += countCashPayments;
            valsTotalPayments[3] = String.Format("{0:#,0.00}", totalCashPaymens);
            totalSumPayments += totalCashPaymens;
            pdf.AddDataRow1(valsTotalPayments, 4, codesTotal);
            valsTotalPayments = new object[4];
            valsTotalPayments[0] = "2.";
            valsTotalPayments[1] = "Фактура кон клиент";
            int countFactureClientPayments = 0;
            Broker.DataAccess.PaymentType ptFactureClient = Broker.DataAccess.PaymentType.GetByCode(Broker.DataAccess.PaymentType.FACTURE);
            decimal totalFactureClientPaymens = Broker.DataAccess.PolicyItem.GetPremiumValueForSummuryRollBacks(isRollBacked, dt1, dt2, false, rbd.InsuranceCompanyID, ptFactureClient, out countFactureClientPayments);
            valsTotalPayments[2] = countFactureClientPayments;
            totalPoliciesPayments += countFactureClientPayments;
            valsTotalPayments[3] = String.Format("{0:#,0.00}", totalFactureClientPaymens);
            totalSumPayments += totalFactureClientPaymens;
            pdf.AddDataRow1(valsTotalPayments, 4, codesTotal);
            valsTotalPayments = new object[4];
            valsTotalPayments[0] = "3.";
            valsTotalPayments[1] = "Фактура кон брокерско друштво";
            int countFactureBrokerHousePayments = 0;
            Broker.DataAccess.PaymentType ptFactureBrokerHouses = Broker.DataAccess.PaymentType.GetByCode(Broker.DataAccess.PaymentType.FACTURE_FOR_BROKER);
            decimal totalFactureBrokerHousesPaymens = Broker.DataAccess.PolicyItem.GetPremiumValueForSummuryRollBacks(isRollBacked, dt1, dt2, false, rbd.InsuranceCompanyID, ptFactureBrokerHouses, out countFactureBrokerHousePayments);
            valsTotalPayments[2] = countFactureBrokerHousePayments;
            totalPoliciesPayments += countFactureBrokerHousePayments;
            valsTotalPayments[3] = String.Format("{0:#,0.00}", totalFactureBrokerHousesPaymens);
            totalSumPayments += totalFactureBrokerHousesPaymens;
            pdf.AddDataRow1(valsTotalPayments, 4, codesTotal);
            valsTotalPayments = new object[4];
            valsTotalPayments[0] = string.Empty;
            valsTotalPayments[1] = "Вкупно";
            valsTotalPayments[2] = totalPoliciesPayments;
            valsTotalPayments[3] = String.Format("{0:#,0.00}", totalSumPayments);
            pdf.AddDataRow1(valsTotalPayments, 4, codesTotal);
            pdf.AddTable();

            pdf.SetTitle(" ");
            pdf.SetTitle(" ");
            pdf.SetTitle(" ");
            pdf.SetTitleLeft("                     Брокер                                                                Скопје                                                       Осигурителна компанија");
            pdf.SetTitleLeft("                                                                                              " + DateTime.Now.ToShortDateString());
            pdf.SetTitleLeft("            ________________                                                                                                               ______________________________");
            pdf.FinishPDF();
        }


        public static void CreateFileForExistingRollBack(RollBackDistribution rbd) {
            PDFCreators pdf = new PDFCreators(false, 25, 25, 25, 25);
            pdf.OpenPDF();
            pdf.SetTitle("  ");
            pdf.SetTitle("  ");
            pdf.AddJDBLogo(650, 480);

            pdf.SetTitle("Раздолжување за период " + rbd.StartDate.ToShortDateString() + " - " + rbd.EndDate.ToShortDateString());
            InsuranceCompany ic = InsuranceCompany.Get(rbd.InsuranceCompanyID);
            pdf.SetTitleLeft(ic.Name);
            pdf.SetTitleLeft("");

            List<InsuranceSubType> istList = InsuranceSubType.GetAllSorted();
            int defaultHeadersCount = 9;
            Dictionary<int, int> listCountPerInsuranceSubType = new Dictionary<int, int>();
            Dictionary<int, decimal> listSumPerInsuranceSubType = new Dictionary<int, decimal>();

            DateTime dt1 = rbd.StartDate;
            DateTime dt2 = rbd.EndDate;

            foreach (InsuranceSubType ist in istList) {
                List<RollBackDistributionItem> rbdiList = RollBackDistributionItem.GetByRollBackDistibutionAndInsuranceSubType(rbd.ID, ist.ID);

                if (rbdiList.Count != 0) {
                    rbdiList = rbdiList.OrderBy(c => c.PolicyItem.IsPaidInBrokerHouse).ThenBy(c => c.PolicyItem.PolicyNumber).ToList();
                    List<Control> controlsList = Control.GetForReportByInsuranceSubType(ist.ID);
                    int additionalHeadersCount = controlsList.Count;
                    string[] headersP = new string[defaultHeadersCount + additionalHeadersCount];
                    int orderNumber = 0;
                    decimal partialValue = 0;
                    decimal basicPartialValue = 0;
                    decimal basicPremiumValue = 0;
                    headersP[0] = "РБ";
                    headersP[1] = "Број на полиса";
                    headersP[2] = "Осигуреник";
                    headersP[3] = "Почеток";
                    headersP[4] = "Истек";
                    headersP[5] = "Статус";
                    headersP[6] = "Тип на плаќање";
                    headersP[7] = "Премија";
                    headersP[8] = "Основна Премија";
                    float[] policyColumnsWidths = new float[defaultHeadersCount + additionalHeadersCount];
                    policyColumnsWidths[0] = 3;
                    policyColumnsWidths[1] = 7;
                    policyColumnsWidths[2] = 18;
                    policyColumnsWidths[3] = 7;
                    policyColumnsWidths[4] = 7;
                    policyColumnsWidths[5] = 7;
                    policyColumnsWidths[6] = 7;
                    policyColumnsWidths[7] = 7;
                    policyColumnsWidths[8] = 7;
                    TypeCode[] codes = new TypeCode[defaultHeadersCount + additionalHeadersCount];
                    codes[0] = TypeCode.String;
                    codes[1] = TypeCode.String;
                    codes[2] = TypeCode.String;
                    codes[3] = TypeCode.DateTime;
                    codes[4] = TypeCode.DateTime;
                    codes[5] = TypeCode.String;
                    codes[6] = TypeCode.String;
                    codes[7] = TypeCode.Decimal;
                    codes[8] = TypeCode.Decimal;
                    for (int i = 0; i < additionalHeadersCount; i++) {
                        codes[defaultHeadersCount + i] = TypeCode.Decimal;
                    }

                    for (int i = 0; i < additionalHeadersCount; i++) {
                        headersP[defaultHeadersCount + i] = controlsList[i].Description;
                        policyColumnsWidths[defaultHeadersCount + i] = 30 / additionalHeadersCount;
                    }

                    pdf.SetTitleLeft(ist.Code + " - " + ist.Description);
                    pdf.CreateTable(defaultHeadersCount + additionalHeadersCount, false, headersP, "", policyColumnsWidths);
                    object[] vals;
                    decimal[] additionalValues = new decimal[additionalHeadersCount];

                    int countPerInsuranceSubType = 0;
                    foreach (RollBackDistributionItem rbc in rbdiList) {
                        orderNumber++;
                        basicPartialValue = rbc.PolicyItem.PremiumValue;
                        vals = new object[defaultHeadersCount + additionalHeadersCount];
                        vals[0] = orderNumber;
                        vals[1] = rbc.PolicyItem.PolicyNumber;
                        vals[2] = rbc.PolicyItem.Policy.Client1.Name;
                        vals[3] = rbc.PolicyItem.Policy.StartDate.ToShortDateString();
                        vals[4] = rbc.PolicyItem.Policy.EndDate.ToShortDateString();
                        if (rbc.PolicyItem.IsPaidInBrokerHouse) {
                            vals[6] = "Еднократно";
                            List<Rate> ratesList = Rate.GetByPolicyItemID(rbc.PolicyItemID);
                            if (ratesList.Count > 1) {
                                vals[6] = "На рати";
                            }
                        } else {
                            vals[6] = "Директно кон о.ком.";
                        }
                        if (rbc.PolicyItem.Policy.Discard) {
                            vals[5] = "Сторно";
                            vals[7] = String.Format("{0:#,0.00}", 0);
                            vals[8] = String.Format("{0:#,0.00}", 0);
                            basicPartialValue = 0;
                        } else {
                            vals[5] = "Активна";
                            countPerInsuranceSubType++;
                            partialValue += rbc.PolicyItem.PremiumValue;
                            vals[7] = String.Format("{0:#,0.00}", rbc.PolicyItem.PremiumValue);

                        }

                        for (int i = 0; i < additionalHeadersCount; i++) {
                            PolicyExtendInformation pei = PolicyExtendInformation.GetByPolicyItemAndControl(rbc.PolicyItemID, controlsList[i].ID);
                            if (pei != null) {
                                if (pei.Value != string.Empty && !rbc.PolicyItem.Policy.Discard) {
                                    additionalValues[i] += decimal.Parse(pei.Value);
                                    vals[defaultHeadersCount + i] = String.Format("{0:#,0.00}", decimal.Parse(pei.Value));
                                    if (controlsList[i].IsPositive != null) {
                                        if ((bool)controlsList[i].IsPositive) {
                                            basicPartialValue += decimal.Parse(pei.Value);
                                        } else {
                                            basicPartialValue -= decimal.Parse(pei.Value);
                                        }
                                    }
                                } else {
                                    vals[defaultHeadersCount + i] = String.Format("{0:#,0.00}", 0);
                                }
                            } else {
                                ValidationDataType vt = Broker.DataAccess.VariableType.GetForVariableType(controlsList[i].VariableTypeID);
                                if (vt == ValidationDataType.String || vt == ValidationDataType.Date) {
                                    vals[defaultHeadersCount + i] = "";
                                } else {
                                    vals[defaultHeadersCount + i] = String.Format("{0:#,0.00}", 0);
                                }
                            }
                        }
                        vals[8] = String.Format("{0:#,0.00}", basicPartialValue);
                        basicPremiumValue += basicPartialValue;

                        pdf.AddDataRow1(vals, additionalHeadersCount + defaultHeadersCount, codes);
                    }
                    vals = new object[additionalHeadersCount + defaultHeadersCount];
                    vals[0] = "";
                    vals[1] = "";
                    vals[2] = "";
                    vals[3] = "";
                    vals[4] = "";
                    vals[5] = "";
                    vals[6] = "Вкупно";
                    vals[7] = String.Format("{0:#,0.00}", partialValue);
                    vals[8] = String.Format("{0:#,0.00}", basicPremiumValue);
                    listCountPerInsuranceSubType.Add(ist.ID, countPerInsuranceSubType);
                    listSumPerInsuranceSubType.Add(ist.ID, partialValue);
                    for (int i = 0; i < additionalHeadersCount; i++) {
                        vals[defaultHeadersCount + i] = String.Format("{0:#,0.00}", additionalValues[i]);
                    }
                    pdf.AddDataRow1(vals, additionalHeadersCount + defaultHeadersCount, codes);
                    pdf.AddTable();
                }

            }

            //Total
            pdf.NewPage();
            pdf.SetTitle("Рекапитулација:");
            string[] headersTotalP = new string[5];
            headersTotalP[0] = "РБ";
            headersTotalP[1] = "Шифра";
            headersTotalP[2] = "Подкласа";
            headersTotalP[3] = "Број на полиси";
            headersTotalP[4] = "Сума";
            float[] policyColumnsTotalWidths = new float[5];
            policyColumnsTotalWidths[0] = 5;
            policyColumnsTotalWidths[1] = 7;
            policyColumnsTotalWidths[2] = 50;
            policyColumnsTotalWidths[3] = 15;
            policyColumnsTotalWidths[4] = 23;
            TypeCode[] codesTotal = new TypeCode[5];
            codesTotal[0] = TypeCode.Int32;
            codesTotal[1] = TypeCode.String;
            codesTotal[2] = TypeCode.String;
            codesTotal[3] = TypeCode.Int32;
            codesTotal[4] = TypeCode.Decimal;
            pdf.CreateTable(5, false, headersTotalP, "", policyColumnsTotalWidths);
            object[] valsTotal;
            int counterTotal = 1;
            int totalPolicies = 0;
            decimal totalSum = 0;
            foreach (KeyValuePair<int, decimal> keyValuePair in listSumPerInsuranceSubType) {
                InsuranceSubType insuranceSubType = InsuranceSubType.Get(keyValuePair.Key);
                valsTotal = new object[5];
                valsTotal[0] = counterTotal.ToString();
                valsTotal[1] = insuranceSubType.Code;
                valsTotal[2] = insuranceSubType.Description;
                valsTotal[3] = listCountPerInsuranceSubType[keyValuePair.Key];
                totalPolicies += Convert.ToInt32(valsTotal[3]);
                valsTotal[4] = String.Format("{0:#,0.00}", keyValuePair.Value);
                totalSum += keyValuePair.Value;
                pdf.AddDataRow1(valsTotal, 5, codesTotal);
                counterTotal++;
            }
            valsTotal = new object[5];
            valsTotal[0] = "";
            valsTotal[1] = "";
            valsTotal[2] = "Вкупно";
            valsTotal[3] = totalPolicies;
            valsTotal[4] = String.Format("{0:#,0.00}", totalSum);
            pdf.AddDataRow1(valsTotal, 5, codesTotal);
            pdf.AddTable();

            //Total per payments
            pdf.NewPage();
            pdf.SetTitle("Збирна статистика:");
            string[] headersTotalPayments = new string[4];
            headersTotalPayments[0] = "РБ";
            headersTotalPayments[1] = "Тип на плаќање";
            headersTotalPayments[2] = "Број на полиси";
            headersTotalPayments[3] = "Сума";
            float[] policyColumnsTotalPaymentsWidths = new float[4];
            policyColumnsTotalPaymentsWidths[0] = 5;
            policyColumnsTotalPaymentsWidths[1] = 65;
            policyColumnsTotalPaymentsWidths[2] = 15;
            policyColumnsTotalPaymentsWidths[3] = 15;
            TypeCode[] codesTotalPayments = new TypeCode[4];
            codesTotal[0] = TypeCode.Int32;
            codesTotal[1] = TypeCode.String;
            codesTotal[2] = TypeCode.Int32;
            codesTotal[3] = TypeCode.Decimal;
            pdf.CreateTable(4, false, headersTotalPayments, "", policyColumnsTotalPaymentsWidths);
            int totalPoliciesPayments = 0;
            decimal totalSumPayments = 0;
            object[] valsTotalPayments = new object[4];
            valsTotalPayments[0] = "1.";
            valsTotalPayments[1] = "Готовина";
            int countCashPayments = 0;
            //decimal totalCashPaymens = PolicyItem.GetPremiumValueForSummuryRollBacks(true, dt1, dt2, true, rbd.InsuranceCompanyID, null, out countCashPayments);
            decimal totalCashPaymens = PolicyItem.GetPremiumValueForExistingRollBack(rbd.ID, true, null, out countCashPayments);
            valsTotalPayments[2] = countCashPayments;
            totalPoliciesPayments += countCashPayments;
            valsTotalPayments[3] = String.Format("{0:#,0.00}", totalCashPaymens);
            totalSumPayments += totalCashPaymens;
            pdf.AddDataRow1(valsTotalPayments, 4, codesTotal);
            valsTotalPayments = new object[4];
            valsTotalPayments[0] = "2.";
            valsTotalPayments[1] = "Фактура кон клиент";
            int countFactureClientPayments = 0;
            Broker.DataAccess.PaymentType ptFactureClient = Broker.DataAccess.PaymentType.GetByCode(Broker.DataAccess.PaymentType.FACTURE);
            //decimal totalFactureClientPaymens = PolicyItem.GetPremiumValueForSummuryRollBacks(true, dt1, dt2, false, rbd.InsuranceCompanyID, ptFactureClient, out countFactureClientPayments);
            decimal totalFactureClientPaymens = PolicyItem.GetPremiumValueForExistingRollBack(rbd.ID, false, ptFactureClient, out countFactureClientPayments);
            valsTotalPayments[2] = countFactureClientPayments;
            totalPoliciesPayments += countFactureClientPayments;
            valsTotalPayments[3] = String.Format("{0:#,0.00}", totalFactureClientPaymens);
            totalSumPayments += totalFactureClientPaymens;
            pdf.AddDataRow1(valsTotalPayments, 4, codesTotal);
            valsTotalPayments = new object[4];
            valsTotalPayments[0] = "3.";
            valsTotalPayments[1] = "Фактура кон брокерско друштво";
            int countFactureBrokerHousePayments = 0;
            Broker.DataAccess.PaymentType ptFactureBrokerHouses = Broker.DataAccess.PaymentType.GetByCode(Broker.DataAccess.PaymentType.FACTURE_FOR_BROKER);
            //decimal totalFactureBrokerHousesPaymens = PolicyItem.GetPremiumValueForSummuryRollBacks(true, dt1, dt2, false, rbd.InsuranceCompanyID, ptFactureBrokerHouses, out countFactureBrokerHousePayments);
            decimal totalFactureBrokerHousesPaymens = PolicyItem.GetPremiumValueForExistingRollBack(rbd.ID, false, ptFactureBrokerHouses, out countFactureBrokerHousePayments);
            valsTotalPayments[2] = countFactureBrokerHousePayments;
            totalPoliciesPayments += countFactureBrokerHousePayments;
            valsTotalPayments[3] = String.Format("{0:#,0.00}", totalFactureBrokerHousesPaymens);
            totalSumPayments += totalFactureBrokerHousesPaymens;
            pdf.AddDataRow1(valsTotalPayments, 4, codesTotal);
            valsTotalPayments = new object[4];
            valsTotalPayments[0] = string.Empty;
            valsTotalPayments[1] = "Вкупно";
            valsTotalPayments[2] = totalPoliciesPayments;
            valsTotalPayments[3] = String.Format("{0:#,0.00}", totalSumPayments);
            pdf.AddDataRow1(valsTotalPayments, 4, codesTotal);
            pdf.AddTable();

            pdf.SetTitle(" ");
            pdf.SetTitle(" ");
            pdf.SetTitle(" ");
            pdf.SetTitleLeft("                     Брокер                                                                Скопје                                                       Осигурителна компанија");
            pdf.SetTitleLeft("                                                                                              " + DateTime.Now.ToShortDateString());
            pdf.SetTitleLeft("            ________________                                                                                                               ______________________________");
            pdf.FinishPDF();







        }

        public static void ForRightRestrictions(int icID, DateTime startDate, DateTime endDate) {

            DataClassesDataContext dcdc = new DataClassesDataContext();
            DateTime dt1 = startDate;
            DateTime dt2 = endDate;
            int d1 = dt1.Day;
            int m1 = dt1.Month;
            int y1 = dt1.Year;
            int d2 = dt2.Day;
            int m2 = dt2.Month;
            int y2 = dt2.Year;

            PDFCreators pdf = new PDFCreators(false, 25, 25, 25, 25);
            pdf.OpenPDF();
            pdf.SetTitle("  ");
            pdf.SetTitle("  ");
            pdf.AddJDBLogo(650, 480);

            List<RightRestriction> rrList = RightRestriction.GetForRollBack(startDate, endDate, icID);
            pdf.SetTitle("Раздолжување винкулации за период " + startDate.ToShortDateString() + " - " + endDate.ToShortDateString());
            InsuranceCompany ic = InsuranceCompany.Get(icID);
            pdf.SetTitleLeft(ic.Name);
            pdf.SetTitleLeft("");
            List<InsuranceSubType> istList = InsuranceSubType.GetAllSorted();
            string[] headers = { "РБ", "Број на винкулација", "Број на полиса", "Банка", "Осигуреник" };
            float[] Widths = { 5, 20, 20, 25, 30 };
            pdf.CreateTable(5, false, headers, "", Widths);
            TypeCode[] codesTotal = new TypeCode[5];
            codesTotal[0] = TypeCode.Int32;
            codesTotal[1] = TypeCode.String;
            codesTotal[2] = TypeCode.String;
            codesTotal[3] = TypeCode.String;
            codesTotal[4] = TypeCode.String;
            foreach (InsuranceSubType ist in istList) {
                List<RightRestriction> rrsubList = rrList.Where(c => c.PolicyItem.InsuranceSubTypeID == ist.ID).ToList();
                if (rrsubList.Count > 0) {
                    int orderNumber = 0;
                    foreach (RightRestriction rr in rrsubList) {
                        orderNumber++;
                        object[] vals = new object[5];
                        vals[0] = orderNumber;
                        vals[1] = rr.Number;
                        vals[2] = rr.PolicyItem.PolicyNumber;
                        vals[3] = rr.Bank.Name;
                        vals[4] = rr.PolicyItem.Policy.Client1.Name;

                        pdf.AddDataRow1(vals, 5, codesTotal);
                    }
                }
            }
            pdf.AddTable();
            pdf.SetTitle(" ");
            pdf.SetTitle(" ");
            pdf.SetTitle(" ");
            pdf.SetTitleLeft("                     Брокер                                                                Скопје                                                       Осигурителна компанија");
            pdf.SetTitleLeft("                                                                                              " + DateTime.Now.ToShortDateString());
            pdf.SetTitleLeft("            ________________                                                                                                               ______________________________");
            pdf.FinishPDF();
        }

        public static void CreateFileForSelectedPolicies(List<RollBackDistributionInfo> previousInfos, List<RollBackDistributionInfo> currentInfos, DateTime startDate, DateTime endDate, int insuranceCompanyID, int? branchID) {

            PDFCreators pdf = new PDFCreators(false, 25, 25, 25, 25);
            pdf.OpenPDF();
            pdf.SetTitle("  ");
            pdf.SetTitle("  ");
            pdf.AddJDBLogo(650, 480);

            pdf.SetTitle("Раздолжување за период " + startDate.ToShortDateString() + " - " + endDate.ToShortDateString());
            InsuranceCompany ic = InsuranceCompany.Get(insuranceCompanyID);
            pdf.SetTitle(ic.Name);
            pdf.SetTitleLeft("");
            if (branchID != null) {
                pdf.SetTitleLeft(Broker.DataAccess.Branch.Get(branchID).Name);
                pdf.SetTitleLeft("");
            } else {
            }

            previousInfos.AddRange(currentInfos.AsEnumerable());
            List<IGrouping<int, RollBackDistributionInfo>> previous = previousInfos.GroupBy(c => c.InsuranceSubTypeID).OrderBy(c => c.Key).ToList();
            int defaultHeadersCount = 9;

            Dictionary<int, int> listCountPerInsuranceSubType = new Dictionary<int, int>();
            Dictionary<int, decimal> listSumPerInsuranceSubType = new Dictionary<int, decimal>();
            foreach (IGrouping<int, RollBackDistributionInfo> insuranceSubTypeID in previous) {

                if (insuranceSubTypeID.Key != 0) {
                    List<RollBackDistributionInfo> infos = previousInfos.Where(c => c.InsuranceSubTypeID == insuranceSubTypeID.Key && c.IsForRollBack == true).ToList();
                    InsuranceSubType ist = InsuranceSubType.Get(insuranceSubTypeID.Key);
                    int countPerInsuranceSubType = 0;
                    if (infos.Count > 0) {
                        List<Control> controlsList = Control.GetForReportByInsuranceSubType(ist.ID);
                        int additionalHeadersCount = controlsList.Count;
                        string[] headersP = new string[defaultHeadersCount + additionalHeadersCount];
                        int orderNumber = 0;
                        decimal partialValue = 0;
                        decimal basicPartialValue = 0;
                        decimal basicPremiumValue = 0;
                        headersP[0] = "РБ";
                        headersP[1] = "Број на полиса";
                        headersP[2] = "Осигуреник";
                        headersP[3] = "Дата на издавање";
                        headersP[4] = "Почеток";
                        headersP[5] = "Истек";
                        headersP[6] = "Статус";
                        headersP[7] = "Премија";
                        headersP[8] = "Основна Премија";
                        float[] policyColumnsWidths = new float[defaultHeadersCount + additionalHeadersCount];
                        policyColumnsWidths[0] = 3;
                        policyColumnsWidths[1] = 7;
                        policyColumnsWidths[2] = 18;
                        policyColumnsWidths[3] = 7;
                        policyColumnsWidths[4] = 7;
                        policyColumnsWidths[5] = 7;
                        policyColumnsWidths[6] = 10;
                        policyColumnsWidths[7] = 8;
                        policyColumnsWidths[8] = 8;
                        TypeCode[] codes = new TypeCode[defaultHeadersCount + additionalHeadersCount];
                        codes[0] = TypeCode.String;
                        codes[1] = TypeCode.String;
                        codes[2] = TypeCode.String;
                        codes[3] = TypeCode.DateTime;
                        codes[4] = TypeCode.DateTime;
                        codes[5] = TypeCode.DateTime;
                        codes[6] = TypeCode.String;
                        codes[7] = TypeCode.Decimal;
                        codes[8] = TypeCode.Decimal;
                        for (int i = 0; i < additionalHeadersCount; i++) {
                            codes[defaultHeadersCount + i] = TypeCode.Decimal;
                        }

                        for (int i = 0; i < additionalHeadersCount; i++) {
                            headersP[defaultHeadersCount + i] = controlsList[i].Description;
                            policyColumnsWidths[defaultHeadersCount + i] = 25 / additionalHeadersCount;
                        }
                        decimal[] additionalValues = new decimal[additionalHeadersCount];
                        pdf.SetTitleLeft(ist.Code + " - " + ist.Description);
                        pdf.CreateTable(defaultHeadersCount + additionalHeadersCount, false, headersP, "", policyColumnsWidths);
                        foreach (RollBackDistributionInfo rbdi in infos) {
                            object[] vals;
                            PolicyItem pi = PolicyItem.Get(rbdi.ID);
                            orderNumber++;
                            basicPartialValue = pi.PremiumValue;
                            vals = new object[defaultHeadersCount + additionalHeadersCount];
                            vals[0] = orderNumber;
                            vals[1] = pi.PolicyNumber;
                            vals[2] = pi.Policy.Client1.Name;
                            vals[3] = pi.Policy.ApplicationDate.ToShortDateString();
                            vals[4] = pi.Policy.StartDate.ToShortDateString();
                            vals[5] = pi.Policy.EndDate.ToShortDateString();

                            if (pi.Policy.Discard) {
                                vals[6] = "Сторно";
                                vals[7] = String.Format("{0:#,0.00}", 0);
                                vals[8] = String.Format("{0:#,0.00}", 0);
                                countPerInsuranceSubType++;
                                basicPartialValue = 0;
                            } else {
                                vals[6] = "Реализирана";
                                countPerInsuranceSubType++;
                                partialValue += pi.PremiumValue;
                                vals[7] = String.Format("{0:#,0.00}", pi.PremiumValue);

                            }

                            for (int i = 0; i < additionalHeadersCount; i++) {
                                PolicyExtendInformation pei = PolicyExtendInformation.GetByPolicyItemAndControl(pi.ID, controlsList[i].ID);
                                if (pei != null) {
                                    if (pei.Value != string.Empty && !pi.Policy.Discard) {
                                        additionalValues[i] += decimal.Parse(pei.Value);
                                        vals[defaultHeadersCount + i] = String.Format("{0:#,0.00}", decimal.Parse(pei.Value));
                                        if (controlsList[i].IsPositive != null) {
                                            if ((bool)controlsList[i].IsPositive) {
                                                basicPartialValue += decimal.Parse(pei.Value);
                                            } else {
                                                basicPartialValue -= decimal.Parse(pei.Value);
                                            }
                                        }
                                    } else {
                                        vals[defaultHeadersCount + i] = String.Format("{0:#,0.00}", 0);
                                    }
                                } else {
                                    ValidationDataType vt = Broker.DataAccess.VariableType.GetForVariableType(controlsList[i].VariableTypeID);
                                    if (vt == ValidationDataType.String || vt == ValidationDataType.Date) {
                                        vals[defaultHeadersCount + i] = "";
                                    } else {
                                        vals[defaultHeadersCount + i] = String.Format("{0:#,0.00}", 0);
                                    }
                                }
                            }
                            vals[8] = String.Format("{0:#,0.00}", basicPartialValue);
                            basicPremiumValue += basicPartialValue;
                            pdf.AddDataRow1(vals, additionalHeadersCount + defaultHeadersCount, codes);
                        }

                        object[] vals1 = new object[additionalHeadersCount + defaultHeadersCount];
                        vals1[0] = "";
                        vals1[1] = "";
                        vals1[2] = "";
                        vals1[3] = "";
                        vals1[4] = "";
                        vals1[5] = "";
                        vals1[6] = "Вкупно";
                        vals1[7] = String.Format("{0:#,0.00}", partialValue);
                        vals1[8] = String.Format("{0:#,0.00}", basicPremiumValue);
                        listCountPerInsuranceSubType.Add(ist.ID, countPerInsuranceSubType);
                        listSumPerInsuranceSubType.Add(ist.ID, partialValue);
                        for (int i = 0; i < additionalHeadersCount; i++) {
                            vals1[defaultHeadersCount + i] = String.Format("{0:#,0.00}", additionalValues[i]);
                        }
                        pdf.AddDataRow1(vals1, additionalHeadersCount + defaultHeadersCount, codes);
                        pdf.AddTable();
                    }
                }
            }
            //Total
            pdf.NewPage();
            pdf.SetTitle("Рекапитулација:");
            string[] headersTotalP = new string[5];
            headersTotalP[0] = "РБ";
            headersTotalP[1] = "Шифра";
            headersTotalP[2] = "Подкласа";
            headersTotalP[3] = "Број на полиси";
            headersTotalP[4] = "Сума";
            float[] policyColumnsTotalWidths = new float[5];
            policyColumnsTotalWidths[0] = 5;
            policyColumnsTotalWidths[1] = 7;
            policyColumnsTotalWidths[2] = 50;
            policyColumnsTotalWidths[3] = 15;
            policyColumnsTotalWidths[4] = 23;
            TypeCode[] codesTotal = new TypeCode[5];
            codesTotal[0] = TypeCode.Int32;
            codesTotal[1] = TypeCode.String;
            codesTotal[2] = TypeCode.String;
            codesTotal[3] = TypeCode.Int32;
            codesTotal[4] = TypeCode.Decimal;
            pdf.CreateTable(5, false, headersTotalP, "", policyColumnsTotalWidths);
            object[] valsTotal;
            int counterTotal = 1;
            int totalPolicies = 0;
            decimal totalSum = 0;
            foreach (KeyValuePair<int, decimal> keyValuePair in listSumPerInsuranceSubType) {
                InsuranceSubType insuranceSubType = InsuranceSubType.Get(keyValuePair.Key);
                valsTotal = new object[5];
                valsTotal[0] = counterTotal.ToString();
                valsTotal[1] = insuranceSubType.Code;
                valsTotal[2] = insuranceSubType.Description;
                valsTotal[3] = listCountPerInsuranceSubType[keyValuePair.Key];
                totalPolicies += Convert.ToInt32(valsTotal[3]);
                valsTotal[4] = String.Format("{0:#,0.00}", keyValuePair.Value);
                totalSum += keyValuePair.Value;
                pdf.AddDataRow1(valsTotal, 5, codesTotal);
                counterTotal++;
            }
            valsTotal = new object[5];
            valsTotal[0] = "";
            valsTotal[1] = "";
            valsTotal[2] = "Вкупно";
            valsTotal[3] = totalPolicies;
            valsTotal[4] = String.Format("{0:#,0.00}", totalSum);
            pdf.AddDataRow1(valsTotal, 5, codesTotal);
            pdf.AddTable();
            pdf.FinishPDF_FileName("Razdolznica_"+ic.Name+"_"+startDate.ToShortDateString()+"_"+endDate.ToShortDateString());

        }
        
        public static void CreateFileForExistingWithoutPaymentTypeRecap(RollBackDistribution rbd) {
            PDFCreators pdf = new PDFCreators(false, 25, 25, 25, 25);
            pdf.OpenPDF();
            pdf.SetTitle("  ");
            pdf.SetTitle("  ");
            pdf.AddJDBLogo(650, 480);

            pdf.SetTitle("Раздолжување за период " + rbd.StartDate.ToShortDateString() + " - " + rbd.EndDate.ToShortDateString());
            InsuranceCompany ic = InsuranceCompany.Get(rbd.InsuranceCompanyID);
            pdf.SetTitleLeft(ic.Name);
            pdf.SetTitleLeft("");

            List<InsuranceSubType> istList = InsuranceSubType.GetAllSorted();
            int defaultHeadersCount = 9;
            Dictionary<int, int> listCountPerInsuranceSubType = new Dictionary<int, int>();
            Dictionary<int, decimal> listSumPerInsuranceSubType = new Dictionary<int, decimal>();

            DateTime dt1 = rbd.StartDate;
            DateTime dt2 = rbd.EndDate;

            foreach (InsuranceSubType ist in istList) {
                List<RollBackDistributionItem> rbdiList = RollBackDistributionItem.GetByRollBackDistibutionAndInsuranceSubType(rbd.ID, ist.ID);

                if (rbdiList.Count != 0) {
                    rbdiList = rbdiList.OrderBy(c => c.PolicyItem.IsPaidInBrokerHouse).ThenBy(c => c.PolicyItem.PolicyNumber).ToList();
                    List<Control> controlsList = Control.GetForReportByInsuranceSubType(ist.ID);
                    int additionalHeadersCount = controlsList.Count;
                    string[] headersP = new string[defaultHeadersCount + additionalHeadersCount];
                    int orderNumber = 0;
                    decimal partialValue = 0;
                    decimal basicPartialValue = 0;
                    decimal basicPremiumValue = 0;
                    headersP[0] = "РБ";
                    headersP[1] = "Број на полиса";
                    headersP[2] = "Осигуреник";
                    headersP[3] = "Почеток";
                    headersP[4] = "Истек";
                    headersP[5] = "Статус";
                    headersP[6] = "Тип на плаќање";
                    headersP[7] = "Премија";
                    headersP[8] = "Основна Премија";
                    float[] policyColumnsWidths = new float[defaultHeadersCount + additionalHeadersCount];
                    policyColumnsWidths[0] = 3;
                    policyColumnsWidths[1] = 7;
                    policyColumnsWidths[2] = 18;
                    policyColumnsWidths[3] = 7;
                    policyColumnsWidths[4] = 7;
                    policyColumnsWidths[5] = 7;
                    policyColumnsWidths[6] = 7;
                    policyColumnsWidths[7] = 7;
                    policyColumnsWidths[8] = 7;
                    TypeCode[] codes = new TypeCode[defaultHeadersCount + additionalHeadersCount];
                    codes[0] = TypeCode.String;
                    codes[1] = TypeCode.String;
                    codes[2] = TypeCode.String;
                    codes[3] = TypeCode.DateTime;
                    codes[4] = TypeCode.DateTime;
                    codes[5] = TypeCode.String;
                    codes[6] = TypeCode.String;
                    codes[7] = TypeCode.Decimal;
                    codes[8] = TypeCode.Decimal;
                    for (int i = 0; i < additionalHeadersCount; i++) {
                        codes[defaultHeadersCount + i] = TypeCode.Decimal;
                    }

                    for (int i = 0; i < additionalHeadersCount; i++) {
                        headersP[defaultHeadersCount + i] = controlsList[i].Description;
                        policyColumnsWidths[defaultHeadersCount + i] = 30 / additionalHeadersCount;
                    }

                    pdf.SetTitleLeft(ist.Code + " - " + ist.Description);
                    pdf.CreateTable(defaultHeadersCount + additionalHeadersCount, false, headersP, "", policyColumnsWidths);
                    object[] vals;
                    decimal[] additionalValues = new decimal[additionalHeadersCount];

                    int countPerInsuranceSubType = 0;
                    foreach (RollBackDistributionItem rbc in rbdiList) {
                        orderNumber++;
                        basicPartialValue = rbc.PolicyItem.PremiumValue;
                        vals = new object[defaultHeadersCount + additionalHeadersCount];
                        vals[0] = orderNumber;
                        vals[1] = rbc.PolicyItem.PolicyNumber;
                        vals[2] = rbc.PolicyItem.Policy.Client1.Name;
                        vals[3] = rbc.PolicyItem.Policy.StartDate.ToShortDateString();
                        vals[4] = rbc.PolicyItem.Policy.EndDate.ToShortDateString();
                        if (rbc.PolicyItem.IsPaidInBrokerHouse) {
                            vals[6] = "Еднократно";
                            List<Rate> ratesList = Rate.GetByPolicyItemID(rbc.PolicyItemID);
                            if (ratesList.Count > 1) {
                                vals[6] = "На рати";
                            }
                        } else {
                            vals[6] = "Директно кон о.ком.";
                        }
                        if (rbc.PolicyItem.Policy.Discard) {
                            vals[5] = "Сторно";
                            vals[7] = String.Format("{0:#,0.00}", 0);
                            vals[8] = String.Format("{0:#,0.00}", 0);
                            basicPartialValue = 0;
                        } else {
                            vals[5] = "Активна";
                            countPerInsuranceSubType++;
                            partialValue += rbc.PolicyItem.PremiumValue;
                            vals[7] = String.Format("{0:#,0.00}", rbc.PolicyItem.PremiumValue);

                        }

                        for (int i = 0; i < additionalHeadersCount; i++) {
                            PolicyExtendInformation pei = PolicyExtendInformation.GetByPolicyItemAndControl(rbc.PolicyItemID, controlsList[i].ID);
                            if (pei != null) {
                                if (pei.Value != string.Empty && !rbc.PolicyItem.Policy.Discard) {
                                    additionalValues[i] += decimal.Parse(pei.Value);
                                    vals[defaultHeadersCount + i] = String.Format("{0:#,0.00}", decimal.Parse(pei.Value));
                                    if (controlsList[i].IsPositive != null) {
                                        if ((bool)controlsList[i].IsPositive) {
                                            basicPartialValue += decimal.Parse(pei.Value);
                                        } else {
                                            basicPartialValue -= decimal.Parse(pei.Value);
                                        }
                                    }
                                } else {
                                    vals[defaultHeadersCount + i] = String.Format("{0:#,0.00}", 0);
                                }
                            } else {
                                ValidationDataType vt = Broker.DataAccess.VariableType.GetForVariableType(controlsList[i].VariableTypeID);
                                if (vt == ValidationDataType.String || vt == ValidationDataType.Date) {
                                    vals[defaultHeadersCount + i] = "";
                                } else {
                                    vals[defaultHeadersCount + i] = String.Format("{0:#,0.00}", 0);
                                }
                            }
                        }
                        vals[8] = String.Format("{0:#,0.00}", basicPartialValue);
                        basicPremiumValue += basicPartialValue;

                        pdf.AddDataRow1(vals, additionalHeadersCount + defaultHeadersCount, codes);
                    }
                    vals = new object[additionalHeadersCount + defaultHeadersCount];
                    vals[0] = "";
                    vals[1] = "";
                    vals[2] = "";
                    vals[3] = "";
                    vals[4] = "";
                    vals[5] = "";
                    vals[6] = "Вкупно";
                    vals[7] = String.Format("{0:#,0.00}", partialValue);
                    vals[8] = String.Format("{0:#,0.00}", basicPremiumValue);
                    listCountPerInsuranceSubType.Add(ist.ID, countPerInsuranceSubType);
                    listSumPerInsuranceSubType.Add(ist.ID, partialValue);
                    for (int i = 0; i < additionalHeadersCount; i++) {
                        vals[defaultHeadersCount + i] = String.Format("{0:#,0.00}", additionalValues[i]);
                    }
                    pdf.AddDataRow1(vals, additionalHeadersCount + defaultHeadersCount, codes);
                    pdf.AddTable();
                }

            }

            //Total
            pdf.NewPage();
            pdf.SetTitle("Рекапитулација:");
            string[] headersTotalP = new string[5];
            headersTotalP[0] = "РБ";
            headersTotalP[1] = "Шифра";
            headersTotalP[2] = "Подкласа";
            headersTotalP[3] = "Број на полиси";
            headersTotalP[4] = "Сума";
            float[] policyColumnsTotalWidths = new float[5];
            policyColumnsTotalWidths[0] = 5;
            policyColumnsTotalWidths[1] = 7;
            policyColumnsTotalWidths[2] = 50;
            policyColumnsTotalWidths[3] = 15;
            policyColumnsTotalWidths[4] = 23;
            TypeCode[] codesTotal = new TypeCode[5];
            codesTotal[0] = TypeCode.Int32;
            codesTotal[1] = TypeCode.String;
            codesTotal[2] = TypeCode.String;
            codesTotal[3] = TypeCode.Int32;
            codesTotal[4] = TypeCode.Decimal;
            pdf.CreateTable(5, false, headersTotalP, "", policyColumnsTotalWidths);
            object[] valsTotal;
            int counterTotal = 1;
            int totalPolicies = 0;
            decimal totalSum = 0;
            foreach (KeyValuePair<int, decimal> keyValuePair in listSumPerInsuranceSubType) {
                InsuranceSubType insuranceSubType = InsuranceSubType.Get(keyValuePair.Key);
                valsTotal = new object[5];
                valsTotal[0] = counterTotal.ToString();
                valsTotal[1] = insuranceSubType.Code;
                valsTotal[2] = insuranceSubType.Description;
                valsTotal[3] = listCountPerInsuranceSubType[keyValuePair.Key];
                totalPolicies += Convert.ToInt32(valsTotal[3]);
                valsTotal[4] = String.Format("{0:#,0.00}", keyValuePair.Value);
                totalSum += keyValuePair.Value;
                pdf.AddDataRow1(valsTotal, 5, codesTotal);
                counterTotal++;
            }
            valsTotal = new object[5];
            valsTotal[0] = "";
            valsTotal[1] = "";
            valsTotal[2] = "Вкупно";
            valsTotal[3] = totalPolicies;
            valsTotal[4] = String.Format("{0:#,0.00}", totalSum);
            pdf.AddDataRow1(valsTotal, 5, codesTotal);
            pdf.AddTable();
            pdf.FinishPDF();
        }

        public static void CreateFileForCompanyWithoutPaymentTypeRecap(List<RollBackDistributionCompanyInfo> rbdList) {
            PDFCreators pdf = new PDFCreators(false, 25, 25, 25, 25);
            pdf.OpenPDF();
            pdf.SetTitle("  ");
            pdf.SetTitle("  ");
            pdf.AddJDBLogo(650, 480);

            DateTime startDate = new DateTime(9999, 12, 1);
            DateTime endDate = new DateTime();
            string companyName = rbdList.First().InsuranceCompanyName;
            List<RollBackDistributionItem> rdbiList = new List<RollBackDistributionItem>();
            foreach (RollBackDistributionCompanyInfo info in rbdList) {
                rdbiList.AddRange(RollBackDistributionItem.GetEnumByRollBackDistibution(info.ID));
                if (startDate > info.StartDate) {
                    startDate = info.StartDate;
                }
                if (endDate < info.EndDate) {
                    endDate = info.EndDate;
                }
            }
            pdf.SetTitle("Раздолжување за период " + startDate.ToShortDateString() + " - " + endDate.ToShortDateString());
            //InsuranceCompany ic = InsuranceCompany.Get(rbd.InsuranceCompanyID);
            pdf.SetTitleLeft(companyName);
            pdf.SetTitleLeft("");

            List<InsuranceSubType> istList = InsuranceSubType.GetAllSorted();
            int defaultHeadersCount = 9;
            Dictionary<int, int> listCountPerInsuranceSubType = new Dictionary<int, int>();
            Dictionary<int, decimal> listSumPerInsuranceSubType = new Dictionary<int, decimal>();

           

            foreach (InsuranceSubType ist in istList) {
                List<RollBackDistributionItem> rbdiSubList = rdbiList.Where(c => c.PolicyItem.InsuranceSubTypeID == ist.ID).ToList();

                if (rbdiSubList.Count != 0) {
                    rbdiSubList = rbdiSubList.OrderBy(c => c.PolicyItem.IsPaidInBrokerHouse).ThenBy(c => c.PolicyItem.PolicyNumber).ToList();
                    List<Control> controlsList = Control.GetForReportByInsuranceSubType(ist.ID);
                    int additionalHeadersCount = controlsList.Count;
                    string[] headersP = new string[defaultHeadersCount + additionalHeadersCount];
                    int orderNumber = 0;
                    decimal partialValue = 0;
                    decimal basicPartialValue = 0;
                    decimal basicPremiumValue = 0;
                    headersP[0] = "РБ";
                    headersP[1] = "Број на полиса";
                    headersP[2] = "Осигуреник";
                    headersP[3] = "Почеток";
                    headersP[4] = "Истек";
                    headersP[5] = "Статус";
                    headersP[6] = "Тип на плаќање";
                    headersP[7] = "Премија";
                    headersP[8] = "Основна Премија";
                    float[] policyColumnsWidths = new float[defaultHeadersCount + additionalHeadersCount];
                    policyColumnsWidths[0] = 3;
                    policyColumnsWidths[1] = 7;
                    policyColumnsWidths[2] = 18;
                    policyColumnsWidths[3] = 7;
                    policyColumnsWidths[4] = 7;
                    policyColumnsWidths[5] = 7;
                    policyColumnsWidths[6] = 7;
                    policyColumnsWidths[7] = 7;
                    policyColumnsWidths[8] = 7;
                    TypeCode[] codes = new TypeCode[defaultHeadersCount + additionalHeadersCount];
                    codes[0] = TypeCode.String;
                    codes[1] = TypeCode.String;
                    codes[2] = TypeCode.String;
                    codes[3] = TypeCode.DateTime;
                    codes[4] = TypeCode.DateTime;
                    codes[5] = TypeCode.String;
                    codes[6] = TypeCode.String;
                    codes[7] = TypeCode.Decimal;
                    codes[8] = TypeCode.Decimal;
                    for (int i = 0; i < additionalHeadersCount; i++) {
                        codes[defaultHeadersCount + i] = TypeCode.Decimal;
                    }

                    for (int i = 0; i < additionalHeadersCount; i++) {
                        headersP[defaultHeadersCount + i] = controlsList[i].Description;
                        policyColumnsWidths[defaultHeadersCount + i] = 30 / additionalHeadersCount;
                    }

                    pdf.SetTitleLeft(ist.Code + " - " + ist.Description);
                    pdf.CreateTable(defaultHeadersCount + additionalHeadersCount, false, headersP, "", policyColumnsWidths);
                    object[] vals;
                    decimal[] additionalValues = new decimal[additionalHeadersCount];

                    int countPerInsuranceSubType = 0;
                    foreach (RollBackDistributionItem rbc in rbdiSubList) {
                        orderNumber++;
                        basicPartialValue = rbc.PolicyItem.PremiumValue;
                        vals = new object[defaultHeadersCount + additionalHeadersCount];
                        vals[0] = orderNumber;
                        vals[1] = rbc.PolicyItem.PolicyNumber;
                        vals[2] = rbc.PolicyItem.Policy.Client1.Name;
                        vals[3] = rbc.PolicyItem.Policy.StartDate.ToShortDateString();
                        vals[4] = rbc.PolicyItem.Policy.EndDate.ToShortDateString();
                        if (rbc.PolicyItem.IsPaidInBrokerHouse) {
                            vals[6] = "Еднократно";
                            List<Rate> ratesList = Rate.GetByPolicyItemID(rbc.PolicyItemID);
                            if (ratesList.Count > 1) {
                                vals[6] = "На рати";
                            }
                        } else {
                            vals[6] = "Директно кон о.ком.";
                        }
                        if (rbc.PolicyItem.Policy.Discard) {
                            vals[5] = "Сторно";
                            vals[7] = String.Format("{0:#,0.00}", 0);
                            vals[8] = String.Format("{0:#,0.00}", 0);
                            basicPartialValue = 0;
                        } else {
                            vals[5] = "Активна";
                            countPerInsuranceSubType++;
                            partialValue += rbc.PolicyItem.PremiumValue;
                            vals[7] = String.Format("{0:#,0.00}", rbc.PolicyItem.PremiumValue);

                        }

                        for (int i = 0; i < additionalHeadersCount; i++) {
                            PolicyExtendInformation pei = PolicyExtendInformation.GetByPolicyItemAndControl(rbc.PolicyItemID, controlsList[i].ID);
                            if (pei != null) {
                                if (pei.Value != string.Empty && !rbc.PolicyItem.Policy.Discard) {
                                    additionalValues[i] += decimal.Parse(pei.Value);
                                    vals[defaultHeadersCount + i] = String.Format("{0:#,0.00}", decimal.Parse(pei.Value));
                                    if (controlsList[i].IsPositive != null) {
                                        if ((bool)controlsList[i].IsPositive) {
                                            basicPartialValue += decimal.Parse(pei.Value);
                                        } else {
                                            basicPartialValue -= decimal.Parse(pei.Value);
                                        }
                                    }
                                } else {
                                    vals[defaultHeadersCount + i] = String.Format("{0:#,0.00}", 0);
                                }
                            } else {
                                ValidationDataType vt = Broker.DataAccess.VariableType.GetForVariableType(controlsList[i].VariableTypeID);
                                if (vt == ValidationDataType.String || vt == ValidationDataType.Date) {
                                    vals[defaultHeadersCount + i] = "";
                                } else {
                                    vals[defaultHeadersCount + i] = String.Format("{0:#,0.00}", 0);
                                }
                            }
                        }
                        vals[8] = String.Format("{0:#,0.00}", basicPartialValue);
                        basicPremiumValue += basicPartialValue;

                        pdf.AddDataRow1(vals, additionalHeadersCount + defaultHeadersCount, codes);
                    }
                    vals = new object[additionalHeadersCount + defaultHeadersCount];
                    vals[0] = "";
                    vals[1] = "";
                    vals[2] = "";
                    vals[3] = "";
                    vals[4] = "";
                    vals[5] = "";
                    vals[6] = "Вкупно";
                    vals[7] = String.Format("{0:#,0.00}", partialValue);
                    vals[8] = String.Format("{0:#,0.00}", basicPremiumValue);
                    listCountPerInsuranceSubType.Add(ist.ID, countPerInsuranceSubType);
                    listSumPerInsuranceSubType.Add(ist.ID, partialValue);
                    for (int i = 0; i < additionalHeadersCount; i++) {
                        vals[defaultHeadersCount + i] = String.Format("{0:#,0.00}", additionalValues[i]);
                    }
                    pdf.AddDataRow1(vals, additionalHeadersCount + defaultHeadersCount, codes);
                    pdf.AddTable();
                }

            }

            //Total
            pdf.NewPage();
            pdf.SetTitle("Рекапитулација:");
            string[] headersTotalP = new string[5];
            headersTotalP[0] = "РБ";
            headersTotalP[1] = "Шифра";
            headersTotalP[2] = "Подкласа";
            headersTotalP[3] = "Број на полиси";
            headersTotalP[4] = "Сума";
            float[] policyColumnsTotalWidths = new float[5];
            policyColumnsTotalWidths[0] = 5;
            policyColumnsTotalWidths[1] = 7;
            policyColumnsTotalWidths[2] = 50;
            policyColumnsTotalWidths[3] = 15;
            policyColumnsTotalWidths[4] = 23;
            TypeCode[] codesTotal = new TypeCode[5];
            codesTotal[0] = TypeCode.Int32;
            codesTotal[1] = TypeCode.String;
            codesTotal[2] = TypeCode.String;
            codesTotal[3] = TypeCode.Int32;
            codesTotal[4] = TypeCode.Decimal;
            pdf.CreateTable(5, false, headersTotalP, "", policyColumnsTotalWidths);
            object[] valsTotal;
            int counterTotal = 1;
            int totalPolicies = 0;
            decimal totalSum = 0;
            foreach (KeyValuePair<int, decimal> keyValuePair in listSumPerInsuranceSubType) {
                InsuranceSubType insuranceSubType = InsuranceSubType.Get(keyValuePair.Key);
                valsTotal = new object[5];
                valsTotal[0] = counterTotal.ToString();
                valsTotal[1] = insuranceSubType.Code;
                valsTotal[2] = insuranceSubType.Description;
                valsTotal[3] = listCountPerInsuranceSubType[keyValuePair.Key];
                totalPolicies += Convert.ToInt32(valsTotal[3]);
                valsTotal[4] = String.Format("{0:#,0.00}", keyValuePair.Value);
                totalSum += keyValuePair.Value;
                pdf.AddDataRow1(valsTotal, 5, codesTotal);
                counterTotal++;
            }
            valsTotal = new object[5];
            valsTotal[0] = "";
            valsTotal[1] = "";
            valsTotal[2] = "Вкупно";
            valsTotal[3] = totalPolicies;
            valsTotal[4] = String.Format("{0:#,0.00}", totalSum);
            pdf.AddDataRow1(valsTotal, 5, codesTotal);
            pdf.AddTable();
            pdf.FinishPDF();
        }


        public class RollBackClass {
            public string name { get; set; }
            public string policynumber { get; set; }
            public DateTime startdate { get; set; }
            public DateTime enddate { get; set; }
            public decimal premiumvalue { get; set; }
            public int policyItemID { get; set; }
            public bool discard { get; set; }
            public bool paidInBrokersHouse { get; set; }
        }

    }
}
