using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Broker.DataAccess;
using Broker.Controllers.ManagementControllers;
using System.IO;
using System.Text;
using ASUC.Controllers.ConvertController;
using Broker.Controllers.FinanceControllers;
using Broker.Controllers.RateController;

public partial class Broker_Bankslips : AuthenticationPage {

    public BankslipInfo BankslipInfo {
        get {
            if (ViewState["BankslipInfo"] != null) {
                return (BankslipInfo)ViewState["BankslipInfo"];
            } else {
                return null;
            }
        }
        set {
            ViewState["BankslipInfo"] = value;
        }
    }

    public List<BankslipItemInfo> listBankslipItemInfos {
        get {
            if (ViewState["lstBankslipItemInfo"] != null) {
                return (List<BankslipItemInfo>)ViewState["lstBankslipItemInfo"];
            } else {
                return new List<BankslipItemInfo>();
            }
        }
        set {
            ViewState["lstBankslipItemInfo"] = value;
        }
    }

    public int GXGridView1SelectedValue {
        get {
            if (ViewState["GXGridView1SelectedValue"] != null) {
                return int.Parse(ViewState["GXGridView1SelectedValue"].ToString());
            } else {
                return 0;
            }
        }
        set {
            ViewState["GXGridView1SelectedValue"] = value;
        }
    }



    protected void Page_Load(object sender, EventArgs e) {
        if (!IsPostBack) {
            GXGridView1.TotalRecords = ViewBankslip.SelectCountCached();
            mvMain.SetActiveView(viewGrid);
            reportControl.BranchName = BrokerHouseInformation.GetBrokerHouseNameByCode(BrokerHouseInformation.FIRST_ROW);
            reportControl.CompanyName = "Брокерско друштво";
            tbBankslipNumber.Text = Bankslip.GetNextNumber(Broker.DataAccess.Bank.Table.First().ID);
        }
    }

    void KomercijalnaBanka() {
        if (FileUpload1.HasFile) {
            try {
                //pateka za dodavanje na datotekata.
                int len = FileUpload1.PostedFile.ContentLength;
                Stream s = FileUpload1.FileContent;
                byte[] data = new byte[len];
                s.Read(data, 0, len);
                MemoryStream ms = new MemoryStream(data);
                StreamReader sr = new StreamReader(ms, Encoding.Default);
                string line = "";
                Bankslip b = new Bankslip();
                decimal totDebtValue = 0;
                decimal totDemandValue = 0;
                List<BankslipItem> lstBI = new List<BankslipItem>();
                DateTime dt = DateTime.Today;
                while ((line = sr.ReadLine()) != null) {
                    if (line.Length > 270) {
                        BankslipItem bi = new BankslipItem();
                        bi.ClientName = ConvertToMacedonian.ConvertToMACEDONIAN(line.Substring(18, 70));
                        bi.ClientAccountNumber = line.Substring(88, 18);
                        string sDebtValue = line.Substring(107, 18);
                        sDebtValue = sDebtValue.Replace(".", ",");
                        bi.DebtValue = Convert.ToDecimal(sDebtValue);
                        totDebtValue += bi.DebtValue;
                        string sDemandValue = line.Substring(126, 18);
                        sDemandValue = sDemandValue.Replace(".", ",");
                        bi.DemandValue = Convert.ToDecimal(sDemandValue);
                        totDemandValue += bi.DemandValue;
                        string sProvisionValue = line.Substring(145, 18);
                        sProvisionValue = sProvisionValue.Replace(".", ",");
                        bi.ProvisionValue = Convert.ToDecimal(sProvisionValue);
                        dt = new DateTime(Convert.ToInt32(line.Substring(163, 4)), Convert.ToInt32(line.Substring(168, 2)), Convert.ToInt32(line.Substring(171, 2)));
                        bi.PaymentDescription = ConvertToMacedonian.ConvertToMACEDONIAN(line.Substring(173, 70));
                        bi.Code = line.Substring(243, 3);
                        string povikuvanjeZadolzuvanje = line.Substring(246, 24);
                        string povikuvanjeOdobruvanje = line.Substring(270, 24);
                        bi.CallOnPaymentNumber = povikuvanjeOdobruvanje + "/" + povikuvanjeOdobruvanje;
                        lstBI.Add(bi);
                    }
                }
                b.BankID = Convert.ToInt32(ddlBanks.SelectedValue);
                b.DebtValue = totDebtValue;
                b.DemandValue = totDemandValue;
                b.Date = dt;
                b.BankslipNumber = tbBankslipNumber.Text;
                List<Bankslip> lstB = new List<Bankslip>();
                lstB.Add(b);
                dvBankslip.DataSource = lstB;
                dvBankslip.DataBind();
                gvBankslipItems.DataSource = lstBI;
                gvBankslipItems.DataBind();
                BankslipInfo = BankslipInfo.GetFromBankslip(b);
                List<BankslipItemInfo> lstBII = new List<BankslipItemInfo>();
                foreach (BankslipItem bi in lstBI) {
                    BankslipItemInfo bii = BankslipItemInfo.GetFromBankslipItem(bi);
                    lstBII.Add(bii);
                }
                listBankslipItemInfos = lstBII;

            } catch (Exception ex) {
                BankslipInfo = null;
                RegisterStartupScript("myAlert", "<script>alert('ГРЕШКА ВО ФОРМАТОТ НА ВЛЕЗНАТА ДАТОТЕКА')</script>");
            }
        } else {
            BankslipInfo = null;
            RegisterStartupScript("myAlert", "<script>alert('НЕМАТЕ ИЗБРАНО ДАТОТЕКА!')</script>");
        }
    }

    void StopanskaBankaADSkopje() {
        if (FileUpload1.HasFile) {
            try {
                //pateka za dodavanje na datotekata.
                int len = FileUpload1.PostedFile.ContentLength;
                Stream s = FileUpload1.FileContent;
                byte[] data = new byte[len];
                s.Read(data, 0, len);
                MemoryStream ms = new MemoryStream(data);
                StreamReader sr = new StreamReader(ms, Encoding.UTF8);
                string line = "";
                Bankslip b = new Bankslip();
                decimal totDebtValue = 0;
                decimal totDemandValue = 0;
                List<BankslipItem> lstBI = new List<BankslipItem>();
                DateTime dt = DateTime.Today;
                while ((line = sr.ReadLine()) != null) {
                    if (line.Length > 50) {
                        int i = Convert.ToInt32(line.Substring(0, 1));
                        if (i == 1) {
                            dt = new DateTime(Convert.ToInt32(line.Substring(22, 4)), Convert.ToInt32(line.Substring(19, 2)), Convert.ToInt32(line.Substring(16, 2)));
                        } else {
                            BankslipItem bi = new BankslipItem();
                            bi.ClientName = ConvertToMacedonian.ConvertToMACEDONIAN(line.Substring(48, 108));
                            bi.ClientAccountNumber = line.Substring(33, 15);
                            string sDebtValue = line.Substring(19, 14);
                            sDebtValue = sDebtValue.Replace(".", ",");
                            bi.DebtValue = Convert.ToDecimal(sDebtValue);
                            totDebtValue += bi.DebtValue;
                            string sDemandValue = line.Substring(5, 14);
                            sDemandValue = sDemandValue.Replace(".", ",");
                            bi.DemandValue = Convert.ToDecimal(sDemandValue);
                            totDemandValue += bi.DemandValue;
                            //string sProvisionValue = line.Substring(5, 14);
                            //sProvisionValue = sProvisionValue.Replace(".", ",");
                            bi.ProvisionValue = 0;
                            bi.PaymentDescription = ConvertToMacedonian.ConvertToMACEDONIAN(line.Substring(207, 70));
                            bi.Code = line.Substring(156, 3);
                            string povikuvanjeZadolzuvanje = line.Substring(287, 15);
                            string povikuvanjeOdobruvanje = line.Substring(302, 15);
                            bi.CallOnPaymentNumber = povikuvanjeOdobruvanje + "/" + povikuvanjeOdobruvanje;
                            lstBI.Add(bi);
                        }
                    }
                }
                b.BankID = Convert.ToInt32(ddlBanks.SelectedValue);
                b.DebtValue = totDebtValue;
                b.DemandValue = totDemandValue;
                b.Date = dt;
                b.BankslipNumber = tbBankslipNumber.Text;
                List<Bankslip> lstB = new List<Bankslip>();
                lstB.Add(b);
                dvBankslip.DataSource = lstB;
                dvBankslip.DataBind();
                gvBankslipItems.DataSource = lstBI;
                gvBankslipItems.DataBind();
                BankslipInfo = BankslipInfo.GetFromBankslip(b);
                List<BankslipItemInfo> lstBII = new List<BankslipItemInfo>();
                foreach (BankslipItem bi in lstBI) {
                    BankslipItemInfo bii = BankslipItemInfo.GetFromBankslipItem(bi);
                    lstBII.Add(bii);
                }
                listBankslipItemInfos = lstBII;
            } catch (Exception ex) {
                BankslipInfo = null;
                RegisterStartupScript("myAlert", "<script>alert('ГРЕШКА ВО ФОРМАТОТ НА ВЛЕЗНАТА ДАТОТЕКА')</script>");
            }
        } else {
            BankslipInfo = null;
            RegisterStartupScript("myAlert", "<script>alert('НЕМАТЕ ИЗБРАНО ДАТОТЕКА!')</script>");
        }
    }

    protected void btnCheck_Click(object sender, EventArgs e) {
        Broker.DataAccess.Bank b = Broker.DataAccess.Bank.Get(Convert.ToInt32(ddlBanks.SelectedValue));
        if (b.Code == Broker.DataAccess.Bank.KOMERCIJALNA_BANKA)
        {
            KomercijalnaBanka();
        }
        else if (b.Code == Broker.DataAccess.Bank.STOPANSKA_BANKA_ADSKOPJE)
        {
            StopanskaBankaADSkopje();
        } else {
            RegisterStartupScript("myAlert", "<script>alert('ЗА ИЗБРАНАТА БАНКА НЕ ПОСТОИ ФАЈЛ ЗА ИМПОРТ!')</script>");
        }
    }
    protected void btnInsert_Click(object sender, EventArgs e) {
        if (BankslipInfo != null) {
            Bankslip b = new Bankslip();
            b.BankID = BankslipInfo.BankID;
            b.BankslipNumber = BankslipInfo.BankslipNumber;
            b.Date = BankslipInfo.Date;
            b.DebtValue = BankslipInfo.DebtValue;
            b.DemandValue = BankslipInfo.DemandValue;
            b.Insert();
            foreach (BankslipItemInfo bii in listBankslipItemInfos) {
                BankslipItem bi = new BankslipItem();
                bi.BankslipID = b.ID;
                bi.CallOnPaymentNumber = bii.CallOnPaymentNumber;
                bi.ClientAccountNumber = bii.ClientAccountNumber;
                bi.ClientName = bii.ClientName;
                bi.Code = bii.Code;
                bi.DebtValue = bii.DebtValue;
                bi.DemandValue = bii.DemandValue;
                bi.PaymentDescription = bii.PaymentDescription;
                bi.ProvisionValue = bii.ProvisionValue;
                bi.Insert();
            }
        }

    }
    protected void ddlBanks_SelectedIndexChanged(object sender, EventArgs e) {
        tbBankslipNumber.Text = Bankslip.GetNextNumber(Convert.ToInt32(ddlBanks.SelectedValue));
    }

    protected void btnImportBankslip_Click(object sender, EventArgs e) {
        mvMain.SetActiveView(viewImportBankslip);
    }

    protected void btnEdit_Click(object sender, EventArgs e) {
        if (GXGridView1.SelectedIndex != -1) {
            mvMain.SetActiveView(viewEdit);
            DetailsView1.DataBind();
            DetailsView1.ChangeMode(DetailsViewMode.Edit);
            btnNew.CssClass = "novZapis";
            btnEdit.CssClass = "izmeni_Active";
            btnDelete.CssClass = "izbrisi";
            btnPreview.CssClass = "osvezi";
            btnReport.CssClass = "izvestaj";
            btnSearch.CssClass = "prebaraj";
            btnPayments.CssClass = "plakanja";
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e) {
        mvMain.SetActiveView(viewSearch);
        btnNew.CssClass = "novZapis";
        btnEdit.CssClass = "izmeni";
        btnDelete.CssClass = "izbrisi";
        btnPreview.CssClass = "osvezi";
        btnReport.CssClass = "izvestaj";
        btnSearch.CssClass = "prebaraj_Active";
        btnPayments.CssClass = "plakanja";
    }
    protected void btnNew_Click(object sender, EventArgs e) {
        mvMain.SetActiveView(viewEdit);
        DetailsView1.ChangeMode(DetailsViewMode.Insert);
        btnNew.CssClass = "novZapis_Active";
        btnEdit.CssClass = "izmeni";
        btnDelete.CssClass = "izbrisi";
        btnPreview.CssClass = "osvezi";
        btnReport.CssClass = "izvestaj";
        btnSearch.CssClass = "prebaraj";
        btnPayments.CssClass = "plakanja";
    }
    protected void btnPreview_Click(object sender, EventArgs e) {
        mvMain.SetActiveView(viewGrid);
        SearchControl1.SearchArguments = null;
        GXGridView1.DataSourceID = odsGridView.ID;
        btnNew.CssClass = "novZapis";
        btnEdit.CssClass = "izmeni";
        btnDelete.CssClass = "izbrisi";
        btnPreview.CssClass = "osvezi_Active";
        btnReport.CssClass = "izvestaj";
        btnSearch.CssClass = "prebaraj";
        btnPayments.CssClass = "plakanja";
    }
    protected void SearchControl1_Search(object sender, EventArgs e) {
        GXGridView1.TotalRecords = ViewBankslip.SelectSearchCountCached(SearchControl1.SearchArguments);
        mvMain.SetActiveView(viewGrid);
    }
    protected void GXGridView1_RowCommand(object sender, GridViewCommandEventArgs e) {
        if (e.CommandName == "SingleClick") {
            btnDelete.Enabled = true;
            btnEdit.Enabled = true;
            btnPayments.Enabled = true;
            int selIndex = Convert.ToInt32(e.CommandArgument);
            GXGridView1.SelectedIndex = selIndex;
            GXGridView1SelectedValue = Convert.ToInt32(GXGridView1.SelectedValue);
        }
        if (e.CommandName == "DoubleClick") {
            btnDelete.Enabled = true;
            btnEdit.Enabled = true;
            btnPayments.Enabled = true;
            int selIndex = Convert.ToInt32(e.CommandArgument);
            GXGridView1.SelectedIndex = selIndex;
            GXGridView1SelectedValue = Convert.ToInt32(GXGridView1.SelectedValue);
            DetailsView1.DataBind();
            DetailsView1.ChangeMode(DetailsViewMode.Edit);
            //mvMain.SetActiveView(viewEdit);
            btnBankslipItems_Click(null, null);
        }
    }
    protected void btnReport_Click(object sender, EventArgs e) {
        mvMain.SetActiveView(viewReport);
        btnNew.CssClass = "novZapis";
        btnEdit.CssClass = "izmeni";
        btnDelete.CssClass = "izbrisi";
        btnPreview.CssClass = "osvezi";
        btnReport.CssClass = "izvestaj_Active";
        btnSearch.CssClass = "prebaraj";
        btnPayments.CssClass = "plakanja";
    }
    protected void FilterControl1_Filter(object sender, EventArgs e) {
        GXGridView1.TotalRecords = ViewBankslip.SelectFilterCountCached(FilterControl1.FCFilterArgument);
    }
    protected void DetailsView1_ItemCommand(object sender, DetailsViewCommandEventArgs e) {
        if (e.CommandName == "Cancel") {
            mvMain.SetActiveView(viewGrid);
        }
    }
    protected void btnDelete_Click(object sender, EventArgs e) {
        if (GXGridView1.SelectedIndex != -1) {
            DetailsView1.DataBind();
            DetailsView1.ChangeMode(DetailsViewMode.ReadOnly);
            mvMain.SetActiveView(viewEdit);
            btnNew.CssClass = "novZapis";
            btnEdit.CssClass = "izmeni";
            btnDelete.CssClass = "izbrisi_Active";
            btnPreview.CssClass = "osvezi";
            btnReport.CssClass = "izvestaj";
            btnSearch.CssClass = "prebaraj";
            btnPayments.CssClass = "plakanja";
        }
    }

    protected void DetailsView1_ItemInserted(object sender, DetailsViewInsertedEventArgs e) {
        if (e.Exception != null) {
            e.ExceptionHandled = true;
            e.KeepInInsertMode = true;
            ValidationUtility.ShowValidationErrors(this, e.Exception);
        } else {
            GXGridView1.TotalRecords = ViewBankslip.SelectCountCached();
            GXGridView1.DataBind();
            //mvMain.SetActiveView(viewGrid);
            mvMain.SetActiveView(viewBankslipItems);
        }
    }

    protected void DetailsView1_ItemUpdated(object sender, DetailsViewUpdatedEventArgs e) {
        if (e.Exception != null) {
            e.ExceptionHandled = true;
            e.KeepInEditMode = true;
            ValidationUtility.ShowValidationErrors(this, e.Exception);
        } else {
            GXGridView1.TotalRecords = ViewBankslip.SelectCountCached();
            GXGridView1.DataBind();
            mvMain.SetActiveView(viewGrid);
        }
    }

    protected void DetailsView1_ItemDeleted(object sender, DetailsViewDeletedEventArgs e) {
        if (e.Exception != null) {
            e.ExceptionHandled = true;
            ValidationUtility.ShowValidationErrors(this, e.Exception);
        } else {
            GXGridView1.TotalRecords = ViewBankslip.SelectCountCached();
            GXGridView1.DataBind();
            mvMain.SetActiveView(viewGrid);
        }
    }

    protected void DetailsView1_ModeChanging(object sender, DetailsViewModeEventArgs e) {
        e.Cancel = true;
    }

    protected void DetailsView1_ItemInserting(object sender, DetailsViewInsertEventArgs e) {
        e.Values["DebtValue"] = 0;
        e.Values["DemandValue"] = 0;
    }

    protected void dvDataSource_Updating(object sender, ObjectDataSourceMethodEventArgs e) {
        Bankslip b = e.InputParameters["newEntity"] as Bankslip;
        BankslipController.ValidateUpdateBankslipNumber(b.ID, b.BankslipNumber, b.BankID);
    }

    protected void dvDataSource_Updated(object sender, ObjectDataSourceStatusEventArgs e) {

    }
    protected void dvDataSource_Inserted(object sender, ObjectDataSourceStatusEventArgs e) {
        Bankslip b = (Bankslip)e.ReturnValue;
        GXGridView1SelectedValue = b.ID;
    }
    protected void dvDataSource_Inserting(object sender, ObjectDataSourceMethodEventArgs e) {
        Bankslip b = e.InputParameters["entityToInsert"] as Bankslip;
        BankslipController.ValidateInsertBankslipNumber(b.BankslipNumber, b.BankID);
    }

    protected void dvDataSource_Selecting(object sender, ObjectDataSourceSelectingEventArgs e) {
        e.InputParameters.Clear();
        e.InputParameters.Add("id", GXGridView1SelectedValue);
    }
    protected void btnBankslipItems_Click(object sender, EventArgs e) {
        int bankslipID = Convert.ToInt32(GXGridView1.SelectedDataKey.Value);
        GXGridViewBankslipItems.TotalRecords = ViewBankslipItem.SelectByFKCountCached("BankslipID", bankslipID);
        GXGridViewBankslipItems.DataBind();
        mvMain.SetActiveView(viewBankslipItems);
        mvBankslipItems.SetActiveView(viewBankslipGrid);

    }

    void SetNewPaidValue(List<FinCardSaldo> lst, decimal newValue) {
        //previous way
        decimal restValue = newValue;
        decimal perPolValue = 0;
        int count = lst.Count;
        int i = 0;
        Broker.DataAccess.Parameter minPercetageForFirstRatePar = Broker.DataAccess.Parameter.GetByCode(Broker.DataAccess.Parameter.ZADOLZITELEN_PROCENT_ZA_PRVA_RATA);
        decimal minPercetageForFirstRate = Convert.ToDecimal(minPercetageForFirstRatePar.Value);
        foreach (FinCardSaldo fcs in lst) {
            TextBox tbNewPayment = gvNewPayments.Rows[i].FindControl("tbNewPayment") as TextBox;
            perPolValue = RateController.Scale5(restValue / count);
            Policy p = Policy.Get(fcs.id);
            decimal policyValue = p.PolicyItems[0].PremiumValue;
            decimal minValueForFirstRate = RateController.Scale5(policyValue * minPercetageForFirstRate / 100);
            if (perPolValue <= fcs.saldo) {
                restValue -= perPolValue;
                tbNewPayment.Text = perPolValue.ToString();
            } else {
                restValue -= fcs.saldo;
                tbNewPayment.Text = fcs.saldo.ToString();
            }
            count--;
            i++;
        }

        //new way
        //decimal restValue = newValue;
        //int count = lst.Count;
        //int i = 0;
        //foreach (FinCardSaldo fcs in lst) {
        //    TextBox tbNewPayment = gvNewPayments.Rows[i].FindControl("tbNewPayment") as TextBox;
        //    if (restValue > fcs.saldo) {
        //        tbNewPayment.Text = fcs.saldo.ToString();
        //        restValue -= fcs.saldo;
        //    } else {
        //        tbNewPayment.Text = restValue.ToString();
        //        break;
        //    }
        //    count--;
        //    i++;
        //}
    }

    protected void gvForPayments_RowCommand(object sender, GridViewCommandEventArgs e) {
        if (e.CommandName == "Payment") {
            int selIndex = Convert.ToInt32(e.CommandArgument);
            gvForPayments.SelectedIndex = selIndex;
            ViewState["gvForPaymentsSelectedValue"] = gvForPayments.SelectedValue;
            BankslipItem bi = BankslipItem.Get(Convert.ToInt32(gvForPayments.SelectedValue));
            if (bi.IsPaid) {
                RegisterStartupScript("myAlert", "<script>alert('Ставката од изводот веќе е евидентирана во плаќања на полисите!')</script>");
                ViewState["ClientID"] = null;
                btnInsertNewPayments.Enabled = false;
                return;
            }
            if (bi.DemandValue > 0) {
                ClientAccount ca = ClientAccount.GetByAccountNumber(bi.ClientAccountNumber);
                if (ca != null) {
                    List<FinCardSaldo> lst = Policy.GetForFinCard(ca.ClientID, "OpenItems");
                    if (lst.Count > 0) {
                        ViewState["ClientID"] = ca.ClientID;
                        gvNewPayments.DataBind();
                        SetNewPaidValue(lst, bi.DemandValue);
                        btnInsertNewPayments.Enabled = true;
                    } else {
                        RegisterStartupScript("myAlert", "<script>alert('Не постојат неплатени полиси за клиентот!')</script>");
                        ViewState["ClientID"] = null;
                        btnInsertNewPayments.Enabled = false;
                    }
                } else {
                    RegisterStartupScript("myAlert", "<script>alert('Не постои сметката!')</script>");
                    ViewState["ClientID"] = null;
                    btnInsertNewPayments.Enabled = false;
                }
            } else {
                RegisterStartupScript("myAlert", "<script>alert('Износот во ставката на изводот не е внесен како побарува!')</script>");
                ViewState["ClientID"] = null;
                btnInsertNewPayments.Enabled = false;
            }
        }
    }

    protected void GXGridViewBankslipItems_RowCommand(object sender, GridViewCommandEventArgs e) {
        if (e.CommandName == "SingleClick") {
            int selIndex = Convert.ToInt32(e.CommandArgument);
            GXGridViewBankslipItems.SelectedIndex = selIndex;
            ViewState["GXGridViewBankslipItemSelectedValue"] = GXGridViewBankslipItems.SelectedValue;
        }
        if (e.CommandName == "DoubleClick") {
            int selIndex = Convert.ToInt32(e.CommandArgument);
            GXGridViewBankslipItems.SelectedIndex = selIndex;
            ViewState["GXGridViewBankslipItemSelectedValue"] = GXGridViewBankslipItems.SelectedValue;
            DetailsViewBankslipItem.DataBind();
            DetailsViewBankslipItem.ChangeMode(DetailsViewMode.Edit);
            mvBankslipItems.SetActiveView(viewBankslipEdit);
        }
        if (e.CommandName == "Edit") {
            int selIndex = Convert.ToInt32(e.CommandArgument);
            GXGridViewBankslipItems.SelectedIndex = selIndex;
            ViewState["GXGridViewBankslipItemSelectedValue"] = GXGridViewBankslipItems.SelectedValue;
        }
        if (e.CommandName == "Delete") {
            int selIndex = Convert.ToInt32(e.CommandArgument);
            GXGridViewBankslipItems.SelectedIndex = selIndex;
            ViewState["GXGridViewBankslipItemSelectedValue"] = GXGridViewBankslipItems.SelectedValue;
        }

    }
    protected void btnNewBankslipItem_Click(object sender, EventArgs e) {
        mvBankslipItems.SetActiveView(viewBankslipEdit);
        DetailsViewBankslipItem.ChangeMode(DetailsViewMode.Insert);
    }
    protected void btnUpdateBankslipItem_Click(object sender, EventArgs e) {
        if (GXGridViewBankslipItems.SelectedIndex != -1) {
            mvBankslipItems.SetActiveView(viewBankslipEdit);
            DetailsViewBankslipItem.DataBind();
            DetailsViewBankslipItem.ChangeMode(DetailsViewMode.Edit);
        }
    }
    protected void btnDeleteBankslipItem_Click(object sender, EventArgs e) {
        if (GXGridViewBankslipItems.SelectedIndex != -1) {
            DetailsViewBankslipItem.DataBind();
            DetailsViewBankslipItem.ChangeMode(DetailsViewMode.ReadOnly);
            mvBankslipItems.SetActiveView(viewBankslipEdit);
        }
    }
    protected void btnPreviewBankslipItems_Click(object sender, EventArgs e) {
        mvBankslipItems.SetActiveView(viewBankslipGrid);
        SearchControl1.SearchArguments = null;
        GXGridViewBankslipItems.DataSourceID = odsGridViewBankslipItems.ID;
        GXGridViewBankslipItems.DataBind();
    }
    protected void DetailsViewBankslipItem_ItemCommand(object sender, DetailsViewCommandEventArgs e) {
        if (e.CommandName == "Cancel") {
            mvBankslipItems.SetActiveView(viewBankslipGrid);
        }
    }
    protected void DetailsViewBankslipItem_ItemDeleted(object sender, DetailsViewDeletedEventArgs e) {
        if (e.Exception != null) {
            e.ExceptionHandled = true;
            ValidationUtility.ShowValidationErrors(this, e.Exception);
        } else {
            int bankslipID = GXGridView1SelectedValue;
            GXGridViewBankslipItems.TotalRecords = ViewBankslipItem.SelectByFKCountCached("BankslipID", bankslipID);
            GXGridViewBankslipItems.DataBind();
            mvBankslipItems.SetActiveView(viewBankslipGrid);
            Bankslip bk = Bankslip.Get(GXGridView1SelectedValue);
            decimal debtValue = Convert.ToDecimal(e.Values["DebtValue"]);
            decimal demandValue = Convert.ToDecimal(e.Values["DemandValue"]);
            bk.DebtValue -= debtValue;
            bk.DemandValue -= demandValue;
            Bankslip.Table.Context.SubmitChanges();
            dvBankslipForItem.DataBind();
        }
    }
    protected void DetailsViewBankslipItem_ItemInserted(object sender, DetailsViewInsertedEventArgs e) {
        if (e.Exception != null) {
            e.ExceptionHandled = true;
            e.KeepInInsertMode = true;
            ValidationUtility.ShowValidationErrors(this, e.Exception);
        } else {
            int bankslipID = GXGridView1SelectedValue;
            GXGridViewBankslipItems.TotalRecords = ViewBankslipItem.SelectByFKCountCached("BankslipID", bankslipID);
            GXGridViewBankslipItems.DataBind();
            mvBankslipItems.SetActiveView(viewBankslipGrid);
            decimal debtValue = Convert.ToDecimal(e.Values["DebtValue"]);
            decimal demandValue = Convert.ToDecimal(e.Values["DemandValue"]);
            Bankslip bk = Bankslip.Get(bankslipID);
            bk.DebtValue += debtValue;
            bk.DemandValue += demandValue;
            Bankslip.Table.Context.SubmitChanges();
            dvBankslipForItem.DataBind();
        }
    }
    protected void DetailsViewBankslipItem_ItemInserting(object sender, DetailsViewInsertEventArgs e) {
        e.Values["BankslipID"] = GXGridView1SelectedValue;
        e.Values["IsPaid"] = false;
        if (e.Values["PaymentDescription"] == null) {
            e.Values["PaymentDescription"] = string.Empty;
        }
        if (e.Values["CallOnPaymentNumber"] == null) {
            e.Values["CallOnPaymentNumber"] = string.Empty;
        }
    }
    protected void DetailsViewBankslipItem_ItemUpdated(object sender, DetailsViewUpdatedEventArgs e) {
        if (e.Exception != null) {
            e.ExceptionHandled = true;
            e.KeepInEditMode = true;
            ValidationUtility.ShowValidationErrors(this, e.Exception);
        } else {
            int bankslipID = Convert.ToInt32(GXGridView1.SelectedDataKey.Value);
            GXGridViewBankslipItems.TotalRecords = ViewBankslipItem.SelectByFKCountCached("BankslipID", bankslipID);
            //e.KeepInEditMode = false;
            GXGridViewBankslipItems.DataBind();
            mvBankslipItems.SetActiveView(viewBankslipGrid);
            Bankslip bk = Bankslip.Get(GXGridView1SelectedValue);
            decimal debtValue = Convert.ToDecimal(e.NewValues["DebtValue"]) - Convert.ToDecimal(e.OldValues["DebtValue"]);
            decimal demandValue = Convert.ToDecimal(e.NewValues["DemandValue"]) - Convert.ToDecimal(e.OldValues["DemandValue"]);
            bk.DebtValue += debtValue;
            bk.DemandValue += demandValue;
            Bankslip.Table.Context.SubmitChanges();
            dvBankslipForItem.DataBind();
        }
    }
    protected void DetailsViewBankslipItem_ModeChanging(object sender, DetailsViewModeEventArgs e) {
        e.Cancel = true;
    }
    protected void dvDataSourceBankslipItem_Inserted(object sender, ObjectDataSourceStatusEventArgs e) {

    }
    protected void dvDataSourceBankslipItem_Inserting(object sender, ObjectDataSourceMethodEventArgs e) {
        BankslipItem bi = e.InputParameters["entityToInsert"] as BankslipItem;
        bi.ValidateInsertBankslipItem();
    }
    protected void dvDataSourceBankslipItem_Selecting(object sender, ObjectDataSourceSelectingEventArgs e) {
        e.InputParameters.Clear();
        if (ViewState["GXGridViewBankslipItemSelectedValue"] != null) {
            e.InputParameters.Add("id", Convert.ToInt32(ViewState["GXGridViewBankslipItemSelectedValue"]));
        }
    }
    protected void dvDataSourceBankslipItem_Updated(object sender, ObjectDataSourceStatusEventArgs e) {

    }
    protected void dvDataSourceBankslipItem_Updating(object sender, ObjectDataSourceMethodEventArgs e) {
        BankslipItem bi = e.InputParameters["newEntity"] as BankslipItem;
        bi.ValidateUpdateBankslipItem();
    }

    protected void GXGridViewBankslipItems_RowEditing(object sender, GridViewEditEventArgs e) {
        mvBankslipItems.SetActiveView(viewBankslipEdit);
        DetailsViewBankslipItem.ChangeMode(DetailsViewMode.Edit);
        DetailsViewBankslipItem.DataBind();
        e.Cancel = true;
    }
    protected void GXGridViewBankslipItems_RowDeleting(object sender, GridViewDeleteEventArgs e) {
        mvBankslipItems.SetActiveView(viewBankslipEdit);
        DetailsViewBankslipItem.ChangeMode(DetailsViewMode.ReadOnly);
        DetailsViewBankslipItem.DataBind();
        e.Cancel = true;
    }
    protected void DetailsViewBankslipItem_ItemUpdating(object sender, DetailsViewUpdateEventArgs e) {
        if (e.OldValues["PaymentDescription"] == null) {
            e.NewValues["PaymentDescription"] = "";
            e.OldValues["PaymentDescription"] = "";
        }
        if (e.OldValues["CallOnPaymentNumber"] == null) {
            e.NewValues["CallOnPaymentNumber"] = "";
            e.OldValues["CallOnPaymentNumber"] = "";
        }
    }
    protected void GXGridViewBankslipItems_RowUpdated(object sender, GridViewUpdatedEventArgs e) {
        int bankslipID = Convert.ToInt32(GXGridView1.SelectedDataKey.Value);
        GXGridViewBankslipItems.TotalRecords = ViewBankslipItem.SelectByFKCountCached("BankslipID", bankslipID);
        e.KeepInEditMode = false;
        GXGridViewBankslipItems.DataBind();
        mvBankslipItems.SetActiveView(viewBankslipGrid);
    }
    protected void dvDataSourceBankslipItem_Deleting(object sender, ObjectDataSourceMethodEventArgs e) {
        BankslipItem bi = e.InputParameters["entityToDelete"] as BankslipItem;
        bi.ValidateDeleteBankslipItem();
    }
    protected void DetailsViewBankslipItem_ItemDeleting(object sender, DetailsViewDeleteEventArgs e) {
        if (e.Values["PaymentDescription"] == null) {
            e.Values["PaymentDescription"] = "";
        }
        if (e.Values["CallOnPaymentNumber"] == null) {
            e.Values["CallOnPaymentNumber"] = "";
        }
    }

    protected void btnPayments_Click(object sender, EventArgs e) {
        ViewState["ClientID"] = null;
        mvMain.SetActiveView(viewPayments);
        dvBankslipForPayments.DataBind();
        gvForPayments.DataBind();
        gvNewPayments.DataBind();
        btnInsertNewPayments.Enabled = false;
        btnPayments.CssClass = "plakanja_Active";
        btnNew.CssClass = "novZapis";
        btnEdit.CssClass = "izmeni";
        btnDelete.CssClass = "izbrisi";
        btnPreview.CssClass = "osvezi";
        btnReport.CssClass = "izvestaj";
        btnSearch.CssClass = "prebaraj";
    }
    protected void odsGVForPayments_Selecting(object sender, ObjectDataSourceSelectingEventArgs e) {
        e.InputParameters.Clear();
        if (GXGridView1SelectedValue > 0) {
            e.InputParameters.Add("bankslipID", GXGridView1SelectedValue);
        } else {
            e.InputParameters.Add("bankslipID", 0);
        }
    }
    protected void odsGridViewBankslipItems_Selecting(object sender, ObjectDataSourceSelectingEventArgs e) {
        e.InputParameters.Clear();
        e.InputParameters.Add("foreignKeyName", "BankslipID");
        e.InputParameters.Add("id", GXGridView1SelectedValue);
    }

    protected void odsNewPayments_Selecting(object sender, ObjectDataSourceSelectingEventArgs e) {
        e.InputParameters.Clear();
        if (ViewState["ClientID"] != null) {
            e.InputParameters.Add("clientID", Convert.ToInt32(ViewState["ClientID"]));
        } else {
            e.InputParameters.Add("clientID", 0);
        }
        e.InputParameters.Add("itemsType", "OpenItems");
    }
    protected void btnInsertNewPayments_Click(object sender, EventArgs e) {
        Bankslip b = Bankslip.Get(Convert.ToInt32(dvBankslipForPayments.SelectedValue));
        decimal totPaidValue = 0;
        foreach (GridViewRow gvr in gvNewPayments.Rows) {
            int polID = Convert.ToInt32(gvr.Cells[0].Text);
            PolicyItem pi = Policy.Get(polID).PolicyItems[0];
            TextBox tbNewPayment = gvr.FindControl("tbNewPayment") as TextBox;
            decimal newPaidValue = 0;
            decimal.TryParse(tbNewPayment.Text, out newPaidValue);
            totPaidValue += newPaidValue;
        }
        decimal biDemandValue = 0;
        if (ViewState["gvForPaymentsSelectedValue"] != null) {
            BankslipItem bi = BankslipItem.Get(Convert.ToInt32(ViewState["gvForPaymentsSelectedValue"]));
            biDemandValue = bi.DemandValue;
        }
        if (totPaidValue != biDemandValue) {
            RegisterStartupScript("myAlert", "<script>alert('Вкупниот внесен износ по полиси се разликува од вредноста побарува од ставката на изводот!')</script>");
            return;
        }
        foreach (GridViewRow gvr in gvNewPayments.Rows) {
            int polID = Convert.ToInt32(gvr.Cells[0].Text);
            PolicyItem pi = Policy.Get(polID).PolicyItems[0];
            TextBox tbNewPayment = gvr.FindControl("tbNewPayment") as TextBox;
            decimal newPaidValue = 0;
            decimal.TryParse(tbNewPayment.Text, out newPaidValue);
            InsertPaymentsForPolicy(pi, newPaidValue, b.Date, b.BankslipNumber, b.BankID);
        }
        if (ViewState["gvForPaymentsSelectedValue"] != null) {
            BankslipItem bi = BankslipItem.Get(Convert.ToInt32(ViewState["gvForPaymentsSelectedValue"]));
            bi.IsPaid = true;
            BankslipItem.Table.Context.SubmitChanges();
        }
        mvMain.SetActiveView(viewGrid);
        btnInsertNewPayments.Enabled = false;
    }


    void InsertPaymentsForPolicy(PolicyItem pi, decimal newPaidValue, DateTime dateOfNewPaid, string bankslipNumber, int bankslipBankID) {
        List<Payment> listPayments = Payment.GetByPolicyItemID(pi.ID);
        decimal paymentTotalValue = 0;
        foreach (Payment payment in listPayments) {
            paymentTotalValue += payment.Value;
        }
        if (newPaidValue > (pi.PremiumValue - paymentTotalValue)) {
            RegisterStartupScript("myAlert", "<script>alert('Поголем износ од преостанатиот износ за плаќање!')</script>");
        } else {
            decimal valueFromClient = newPaidValue;
            while (valueFromClient > 0) {
                Rate currentRate = Rate.GetCurrentRateForPayment(pi.ID);
                Payment newPayment = new Payment();
                newPayment.Date = dateOfNewPaid;
                newPayment.RateID = currentRate.ID;
                newPayment.IsCashReported = false;
                Broker.DataAccess.PaymentType pt = Broker.DataAccess.PaymentType.GetByCode(Broker.DataAccess.PaymentType.VIRMAN);
                newPayment.PaymentTypeID = pt.ID;
                newPayment.UserID = this.PageUser.ID;
                newPayment.BranchID = this.PageUser.BranchID;
                newPayment.BankslipNumber = bankslipNumber;
                newPayment.BankslipBankID = bankslipBankID;
                if (valueFromClient >= currentRate.Value) {
                    newPayment.Value = currentRate.Value - Payment.GetPaidValueForRate(currentRate.ID);
                    newPayment.Insert();
                    decimal basicValue = newPayment.Value;
                    decimal k = basicValue / newPayment.Rate.PolicyItem.PremiumValue;
                    List<ControlAppropriateInsuranceSubType> listAppropriateIST = Broker.DataAccess.ControlAppropriateInsuranceSubType.Table.ToList();
                    foreach (ControlAppropriateInsuranceSubType c in listAppropriateIST) {
                        PolicyExtendInformation pei = PolicyExtendInformation.GetByPolicyItemAndControl(pi.ID, c.ControlID);
                        if (pei != null) {
                            decimal peiValue = 0;
                            decimal.TryParse(pei.Value, out peiValue);
                            basicValue -= k * peiValue;
                            if (peiValue > 0) {
                                PaymentsPerInsSubType ppist = new PaymentsPerInsSubType();
                                ppist.PaymentID = newPayment.ID;
                                ppist.InsuranceSubTypeID = c.InsuranceSubTypeID;
                                ppist.PaidValue = k * peiValue;
                                ppist.BrokerageValue = ppist.PaidValue * pi.BrokeragePercentage / 100;
                                ppist.Insert();
                            }
                        }
                    }
                    if (basicValue > 0) {
                        PaymentsPerInsSubType ppist = new PaymentsPerInsSubType();
                        ppist.PaymentID = newPayment.ID;
                        ppist.InsuranceSubTypeID = pi.InsuranceSubTypeID;
                        ppist.PaidValue = basicValue;
                        ppist.BrokerageValue = ppist.PaidValue * pi.BrokeragePercentage / 100;
                        ppist.Insert();
                    }
                    valueFromClient -= newPayment.Value;
                    currentRate.PaidValue += newPayment.Value;
                    Rate.Table.Context.SubmitChanges();
                } else {
                    if (valueFromClient <= (currentRate.Value - currentRate.PaidValue)) {
                        newPayment.Value = valueFromClient;
                        newPayment.Insert();
                        decimal basicValue = newPayment.Value;
                        decimal k = basicValue / newPayment.Rate.PolicyItem.PremiumValue;
                        List<ControlAppropriateInsuranceSubType> listAppropriateIST = Broker.DataAccess.ControlAppropriateInsuranceSubType.Table.ToList();
                        foreach (ControlAppropriateInsuranceSubType c in listAppropriateIST) {
                            PolicyExtendInformation pei = PolicyExtendInformation.GetByPolicyItemAndControl(pi.ID, c.ControlID);
                            if (pei != null) {
                                decimal peiValue = 0;
                                decimal.TryParse(pei.Value, out peiValue);
                                basicValue -= k * peiValue;
                                if (peiValue > 0) {
                                    PaymentsPerInsSubType ppist = new PaymentsPerInsSubType();
                                    ppist.PaymentID = newPayment.ID;
                                    ppist.InsuranceSubTypeID = c.InsuranceSubTypeID;
                                    ppist.PaidValue = k * peiValue;
                                    ppist.BrokerageValue = ppist.PaidValue * pi.BrokeragePercentage / 100;
                                    ppist.Insert();
                                }
                            }
                        }
                        if (basicValue > 0) {
                            PaymentsPerInsSubType ppist = new PaymentsPerInsSubType();
                            ppist.PaymentID = newPayment.ID;
                            ppist.InsuranceSubTypeID = pi.InsuranceSubTypeID;
                            ppist.PaidValue = basicValue;
                            ppist.BrokerageValue = ppist.PaidValue * pi.BrokeragePercentage / 100;
                            ppist.Insert();
                        }
                        currentRate.PaidValue += valueFromClient;
                        Rate.Table.Context.SubmitChanges();
                        break;

                    } else {
                        newPayment.Value = (currentRate.Value - currentRate.PaidValue);
                        newPayment.Insert();
                        decimal basicValue = newPayment.Value;
                        decimal k = basicValue / newPayment.Rate.PolicyItem.PremiumValue;
                        List<ControlAppropriateInsuranceSubType> listAppropriateIST = Broker.DataAccess.ControlAppropriateInsuranceSubType.Table.ToList();
                        foreach (ControlAppropriateInsuranceSubType c in listAppropriateIST) {
                            PolicyExtendInformation pei = PolicyExtendInformation.GetByPolicyItemAndControl(pi.ID, c.ControlID);
                            if (pei != null) {
                                decimal peiValue = 0;
                                decimal.TryParse(pei.Value, out peiValue);
                                basicValue -= k * peiValue;
                                if (peiValue > 0) {
                                    PaymentsPerInsSubType ppist = new PaymentsPerInsSubType();
                                    ppist.PaymentID = newPayment.ID;
                                    ppist.InsuranceSubTypeID = c.InsuranceSubTypeID;
                                    ppist.PaidValue = k * peiValue;
                                    ppist.BrokerageValue = ppist.PaidValue * pi.BrokeragePercentage / 100;
                                    ppist.Insert();
                                }
                            }
                        }

                        if (basicValue > 0) {
                            PaymentsPerInsSubType ppist = new PaymentsPerInsSubType();
                            ppist.PaymentID = newPayment.ID;
                            ppist.InsuranceSubTypeID = pi.InsuranceSubTypeID;
                            ppist.PaidValue = basicValue;
                            ppist.BrokerageValue = ppist.PaidValue * pi.BrokeragePercentage / 100;
                            ppist.Insert();
                        }
                        //currentRate.PaidValue += valueFromClient;
                        //Rate.Table.Context.SubmitChanges();
                        currentRate.PaidValue += newPayment.Value;
                        Rate.Table.Context.SubmitChanges();
                        valueFromClient -= newPayment.Value;
                    }
                }
            }
            Broker.DataAccess.Facture.UpdatePaidStatusForFacture(pi.ID);
        }
    }
    protected void dvDataSource_Deleting(object sender, ObjectDataSourceMethodEventArgs e) {
        Bankslip b = e.InputParameters["entityToDelete"] as Bankslip;
        b.ValidateDeleteBankslip();
    }
}