using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Broker.DataAccess;
using Broker.Controllers.RateController;

public partial class BROKERAdmin_FacturePayments : AuthenticationPage {
    protected void Page_Load(object sender, EventArgs e) {
        if (!Page.IsPostBack) {
            tbDateOfPayment.Text = DateTime.Today.ToShortDateString();
        }
    }
    protected void odsFactureItemsPreview_Selecting(object sender, ObjectDataSourceSelectingEventArgs e) {

        string factureNumber = tbFactureNumber.Text;
        try {
            Broker.DataAccess.Facture f = Broker.DataAccess.Facture.GetByNumber(factureNumber);

            if (f != null) {
                e.InputParameters.Clear();
                e.InputParameters.Add("factureID", f.ID.ToString());
            }
        } catch (Exception ex) {
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e) {
        string factureNumber = tbFactureNumber.Text;
        try {
            Broker.DataAccess.Facture f = Broker.DataAccess.Facture.GetByNumber(factureNumber);

            if (f != null) {
                UpdateTextBoxes(f);
                lblFeedback.Text = string.Empty;
                gvFactureItemsPreview.DataBind();
                btnGenerate.Enabled = true;
                tbInsuranceCompany.Text = f.InsuranceCompany.ShortName;
            } else {
                lblFeedback.Text = "Не е пронајдена фактура!";
            }
            GridViewPayments.DataBind();
        } catch (Exception ex) {
            lblFeedback.Text = "Изберете фактура!";
            btnGenerate.Enabled = false;
            btnInsert.Enabled = false;
        }
    }

    void UpdateTextBoxes(Broker.DataAccess.Facture f)
    {
        tbFactureTotalCost.Text = String.Format("{0:#,0.00}", f.BrokerageValue);
        tbFactureTotalPaidValue.Text = String.Format("{0:#,0.00}", FactureCollectedPaidValue.GetPaidValueForFacture(f.ID));
        tbFactureForPaidValue.Text = String.Format("{0:#,0.00}", (Convert.ToDecimal(tbFactureTotalCost.Text) - Convert.ToDecimal(tbFactureTotalPaidValue.Text)));
    }

    protected void tbValue_TextChanged(object sender, EventArgs e) {

    }



    protected void btnInsert_Click(object sender, EventArgs e) {
        string factureNumber = tbFactureNumber.Text;
        DateTime paidDate = DateTime.Now;
        DateTime.TryParse(tbDateOfPayment.Text, out paidDate);

        try {
            Broker.DataAccess.Facture f = Broker.DataAccess.Facture.GetByNumber(factureNumber);
            if (f != null) {
                foreach (GridViewRow gvRow in GridViewPayments.Rows) {
                    decimal newTotValue = 0;
                    TextBox tbPaidValue = (TextBox)gvRow.FindControl("tbValue");
                    TextBox tbFactureItemID = (TextBox)gvRow.FindControl("tbFactureItemID");
                    decimal.TryParse(tbPaidValue.Text, out newTotValue);
                    int factureItemID = 0;
                    int.TryParse(tbFactureItemID.Text, out factureItemID);
                    if (factureItemID > 0) {
                        if (newTotValue < FactureCollectedPaidValue.GetPaidValueForFactureItem(factureItemID)) {
                            RegisterStartupScript("myAlert", "<script>alert('ВНЕСОВТЕ ПОМАЛ ИЗНОС ОД ТОА ШТО Е ПЛАТЕНО ДО СЕГА!')</script>");
                            return;
                        }
                        if (newTotValue > FactureItem.Get(factureItemID).BrokerageValue) {
                            RegisterStartupScript("myAlert", "<script>alert('ВНЕСОВТЕ ПОГОЛЕМ ИЗНОС ОД ПРЕСМЕТАНАТА БРОКЕРАЖА!')</script>");
                            return;
                        }
                        if (FactureCollectedPaidValue.GetPaidValueForFactureItem(factureItemID) != FactureItem.Get(factureItemID).BrokerageValue) {
                            FactureCollectedPaidValue fcpv = new FactureCollectedPaidValue();
                            fcpv.FactureItemID = factureItemID;
                            fcpv.PaidDate = paidDate;
                            fcpv.PaidValue = newTotValue - FactureCollectedPaidValue.GetPaidValueForFactureItem(factureItemID);
                            fcpv.Insert();
                            FactureItem fi = FactureItem.Get(factureItemID);
                            List<PolicyItemFactureItem> lstPIFI = PolicyItemFactureItem.GetByFactureItemID(factureItemID);
                            decimal koef = fcpv.PaidValue / fi.BrokerageValue;
                            List<InsuranceSubType> listAppropriateIST = Broker.DataAccess.ControlAppropriateInsuranceSubType.Table.Select(c => c.InsuranceSubType).ToList();
                            Dictionary<InsuranceSubType, decimal> dic = new Dictionary<InsuranceSubType, decimal>();
                            decimal totValue = 0;
                            foreach (PolicyItem pi in lstPIFI.Select(c => c.PolicyItem).Where(c => c.Policy.Discard == false).ToList()) {
                                decimal brokPercentage = 0;
                                if (pi.BrokerageID != null) {
                                        if (pi.Policy.Client.IsLaw) {
                                            brokPercentage = pi.Brokerage.PercentageForLaws;
                                        } else {
                                            brokPercentage = pi.Brokerage.PercentageForPrivates;
                                        }
                                    } else if (pi.PacketBrokerageID != null) {
                                        if (pi.Policy.Client.IsLaw) {
                                            brokPercentage = pi.PacketsInsuranceSubType.BrokeragePecentageForLaws;
                                        } else {
                                            brokPercentage = pi.PacketsInsuranceSubType.BrokeragePecentageForPrivates;
                                        }
                                    }
                                foreach (InsuranceSubType ist in listAppropriateIST) {
                                    List<Broker.DataAccess.Control> listControls = ControlAppropriateInsuranceSubType.GetByInsuranceSubType(ist.ID);
                                    decimal tmpValue = 0;
                                    
                                    foreach (Broker.DataAccess.Control con in listControls) {
                                        PolicyExtendInformation pei = PolicyExtendInformation.GetByPolicyItemAndControl(pi.ID, con.ID);
                                        if (pei != null) {
                                            tmpValue += Convert.ToDecimal(pei.Value);
                                        }
                                    }
                                    if (dic.Keys.Contains(ist)) {
                                        dic[ist] += (tmpValue * brokPercentage / 100);
                                    } else {
                                        dic.Add(ist, (tmpValue * brokPercentage / 100));
                                    }
                                }
                                //totValue += RateController.Scale5(pi.PremiumValue * brokPercentage / 100);
                                totValue += pi.PremiumValue * brokPercentage / 100;
                            }
                            totValue = RateController.Scale5(totValue);
                            decimal addValue = 0;
                            foreach (KeyValuePair<InsuranceSubType, decimal> kvp in dic) {
                                if (kvp.Value > 0) {
                                    addValue += kvp.Value;
                                    FacCollPaidValuesPerInsSubType fcp = new FacCollPaidValuesPerInsSubType();
                                    fcp.FactureCollectedPaidValueID = fcpv.ID;
                                    fcp.InsuranceSubTypeID = kvp.Key.ID;
                                    fcp.PaidValue = RateController.Scale5(fcpv.PaidValue * kvp.Value / totValue);
                                    fcp.Insert();
                                }
                            }
                            decimal baseValue = fi.BrokerageValue - addValue;
                            FacCollPaidValuesPerInsSubType fcpBase = new FacCollPaidValuesPerInsSubType();
                            fcpBase.FactureCollectedPaidValueID = fcpv.ID;
                            fcpBase.InsuranceSubTypeID = fi.InsuranceSubTypeID;
                            fcpBase.PaidValue = RateController.Scale5(fcpv.PaidValue * baseValue / totValue);
                            fcpBase.Insert();
                        }
                    }
                }
            } else {
                lblFeedback.Text = "Не е пронајдена фактура!";
            }
        } catch (Exception ex) {
            lblFeedback.Text = "Грешка!";
        }
    }

    protected void btnGenerate_Click(object sender, EventArgs e) {
        string factureNumber = tbFactureNumber.Text;
        DateTime paidDate = DateTime.Now;
        DateTime.TryParse(tbDateOfPayment.Text, out paidDate);

        try {
            Broker.DataAccess.Facture f = Broker.DataAccess.Facture.GetByNumber(factureNumber);
            if (f != null) {
                decimal newPaymentValue = 0;
                decimal.TryParse(tbValueOfPayment.Text, out newPaymentValue);
                if (newPaymentValue > Convert.ToDecimal(tbFactureForPaidValue.Text)) {
                    RegisterStartupScript("myAlert", "<script>alert('ИЗНОСОТ КОЈ ГО ВНЕСОВТЕ ЗА ПЛАЌАЊЕ Е ПОГОЛЕМ ОД ПРЕОСТАНАТИОТ ДОЛГ!')</script>");
                    return;
                }
                decimal proValue = RateController.Scale5(newPaymentValue / (decimal)f.FactureItems.Count);
                List<FactureCollectedPaidValue> list = FactureCollectedPaidValue.GetGroupedByFactureID(f.ID);
                if (list.Count > 0) {
                    int restColumn = f.FactureItems.Count;
                    foreach (FactureCollectedPaidValue fcpv in list) {
                        if (FactureCollectedPaidValue.GetPaidValueForFactureItem(fcpv.FactureItemID) == fcpv.FactureItem.BrokerageValue) {
                            restColumn--;
                        }
                        proValue = RateController.Scale5(newPaymentValue / (decimal)restColumn);
                        fcpv.PaidDate = paidDate;
                        if ((fcpv.PaidValue + proValue) > fcpv.FactureItem.BrokerageValue) {
                            fcpv.PaidValue = fcpv.FactureItem.BrokerageValue;
                        } else {
                            fcpv.PaidValue += proValue;
                        }
                    }
                } else {
                    decimal rest = newPaymentValue;
                    int i = 0;
                    foreach (FactureItem fi in f.FactureItems) {
                        proValue = RateController.Scale5(rest / (decimal)(f.FactureItems.Count - i));
                        FactureCollectedPaidValue fcpv = new FactureCollectedPaidValue();
                        fcpv.FactureItemID = fi.ID;
                        fcpv.FactureItem = fi;
                        fcpv.PaidDate = paidDate;
                        if (fi.BrokerageValue > (proValue + FactureCollectedPaidValue.GetPaidValueForFactureItem(fi.ID))) {
                            fcpv.PaidValue += proValue;
                        } else {
                            fcpv.PaidValue = fi.BrokerageValue;
                        }
                        rest -= fcpv.PaidValue;
                        list.Add(fcpv);
                        i++;
                    }
                }
                GridViewPayments.DataSourceID = null;
                GridViewPayments.DataSource = list;
                GridViewPayments.DataBind();
                btnInsert.Enabled = true;
            } else {
                lblFeedback.Text = "Не е пронајдена фактура!";
            }
        } catch (Exception ex) {
            lblFeedback.Text = "Грешка!";
        }
    }
    protected void odsPaidPayments_Selecting(object sender, ObjectDataSourceSelectingEventArgs e) {
        string factureNumber = tbFactureNumber.Text;
        try {
            Broker.DataAccess.Facture f = Broker.DataAccess.Facture.GetByNumber(factureNumber);

            if (f != null) {
                e.InputParameters.Clear();
                e.InputParameters.Add("factureID", f.ID.ToString());
            }
        } catch (Exception ex) {
        }
    }
}
