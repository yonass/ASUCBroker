using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Broker.DataAccess;
using System.Collections;
using Broker.Controllers.ReportControllers;

public partial class SEAdmin_Distributions : AuthenticationPage {

    protected void Page_Load(object sender, EventArgs e) {
        if (!IsPostBack) {
            gvDistributions.TotalRecords = ViewDistribution.SelectCountCached();
            mvMain.SetActiveView(viewGrid);
            reportControl.BranchName = BrokerHouseInformation.GetBrokerHouseNameByCode(BrokerHouseInformation.FIRST_ROW);
            reportControl.CompanyName = "Брокерско друштво";
            SetPrefix();
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e) {
        mvMain.SetActiveView(viewSearch);
        btnNew.CssClass = "novZapis";
        btnPreview.CssClass = "osvezi";
        btnReport.CssClass = "izvestaj";
        btnSearch.CssClass = "prebaraj_Active";
    }
    protected void btnNew_Click(object sender, EventArgs e) {
        mvMain.SetActiveView(viewEdit);
        btnNew.CssClass = "novZapis_Active";
        btnPreview.CssClass = "osvezi";
        btnReport.CssClass = "izvestaj";
        btnSearch.CssClass = "prebaraj";
        gvNewDistrirutions.DataSource = null;
        gvNewDistrirutions.DataBind();
        SetPrefix();
        tbDocumentNumber.Text = DistributionDocument.GetNextNumber();
    }

    protected void btnPreview_Click(object sender, EventArgs e) {
        mvMain.SetActiveView(viewGrid);
        SearchControl1.SearchArguments = null;
        gvDistributions.DataSourceID = gvDataSource.ID;
        btnNew.CssClass = "novZapis";
        btnPreview.CssClass = "osvezi_Active";
        btnReport.CssClass = "izvestaj";
        btnSearch.CssClass = "prebaraj";
    }

    protected void SearchControl1_Search(object sender, EventArgs e) {
        gvDistributions.TotalRecords = ViewDistribution.SelectSearchCountCached(SearchControl1.SearchArguments);
        mvMain.SetActiveView(viewGrid);
    }

    protected void GXGridView1_RowCommand(object sender, GridViewCommandEventArgs e) {
        if (e.CommandName == "SingleClick") {
            int selIndex = Convert.ToInt32(e.CommandArgument);
            gvDistributions.SelectedIndex = selIndex;
        }
    }
    protected void btnReport_Click(object sender, EventArgs e) {
        mvMain.SetActiveView(viewReport);
        btnNew.CssClass = "novZapis";
        btnPreview.CssClass = "osvezi";
        btnReport.CssClass = "izvestaj_Active";
        btnSearch.CssClass = "prebaraj";
    }
    protected void FilterControl1_Filter(object sender, EventArgs e) {
        gvDistributions.TotalRecords = ViewDistribution.SelectFilterCountCached(FilterControl1.FCFilterArgument);
    }

    protected void btnName_click(object sender, EventArgs e) {
        gvDistributions.DataBind();
        ddlInsuranceCompany.Enabled = false;
        ddlInsuranceSubType.Enabled = false;
        ddlUsers.Enabled = false;
        ddlInsuranceType.Enabled = false;
        tbStartNumber.Enabled = false;
    }

    void SetPrefix() {
        Broker.DataAccess.Parameter parHasCompanyPrefixes = Broker.DataAccess.Parameter.GetByCode(Broker.DataAccess.Parameter.SE_KORISTAT_PREFIKSI);
        if (parHasCompanyPrefixes != null) {
            bool hasCompanyPrefixes = Convert.ToBoolean(parHasCompanyPrefixes.Value);
            if (hasCompanyPrefixes == true) {
                lblInsuranceCompanyPrefix.Visible = true;
                tbInsuranceCompanyPrefix.Visible = true;
                if (ddlInsuranceCompany.SelectedValue != null) {
                    InsuranceCompany ic;
                    if (ddlInsuranceCompany.SelectedValue != string.Empty) {
                        ic = InsuranceCompany.Get(Convert.ToInt32(ddlInsuranceCompany.SelectedValue));
                    } else {
                        ddlInsuranceCompany.DataBind();
                        if (ddlInsuranceCompany.Items.Count > 0) {
                            ic = InsuranceCompany.Get(Convert.ToInt32(ddlInsuranceCompany.Items[0].Value));
                        } else {
                            ic = null;
                        }
                    }
                    if (ic != null) {
                        if (ic.Prefix != null) {
                            tbInsuranceCompanyPrefix.Text = ic.Prefix;
                        }
                    }
                }
            } else {
                lblInsuranceCompanyPrefix.Visible = false;
                tbInsuranceCompanyPrefix.Visible = false;
            }
        } else {
            lblInsuranceCompanyPrefix.Visible = false;
            tbInsuranceCompanyPrefix.Visible = false;
        }
    }

    protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e) {
        ddlInsuranceType.DataBind();
        ddlInsuranceSubType.DataBind();
        SetPrefix();
    }
    protected void ddlInsuranceType_SelectedIndexChanged(object sender, EventArgs e) {
        ddlInsuranceSubType.DataBind();
    }

    protected void ddlDistributionDocType_SelectedIndexChanged(object sender, EventArgs e) {
        DistributionDocType ddt = DistributionDocType.Get(Convert.ToInt32(ddlDistributionDocType.SelectedValue));
        if (ddt.Code == DistributionDocType.PRIEM) {
            ddlBranches.Visible = false;
            ddlUsers.Visible = true;
        } else if (ddt.Code == DistributionDocType.ISPRATNICA) {
            ddlBranches.Visible = true;
            ddlUsers.Visible = true;
        } else if (ddt.Code == DistributionDocType.POVRATNICA) {
            ddlBranches.Visible = true;
            ddlUsers.Visible = true;
        }
        tbDocumentNumber.Text = DistributionDocument.GetNextNumber();
        ddlUsers.DataBind();
    }

    protected void ddlBranches_SelectedIndexChanged(object sender, EventArgs e) {
        ddlUsers.DataBind();
    }

    protected void btnPrintDocument_Click(object sender, EventArgs e) {
        DistributionDocument dd = DistributionDocument.GetByDocumentNumber(tbDocumentNumber.Text);
        if (dd != null) {
            PrintDistributionDocuments.PrintDistributionDocument(dd);
        }
    }

    protected void btnCreate_Click(object sender, EventArgs e) {
        DistributionDocType ddt = DistributionDocType.Get(Convert.ToInt32(ddlDistributionDocType.SelectedValue));
        if (ddt.Code == DistributionDocType.PRIEM) {
            List<Distribution> addedDistributions = new List<Distribution>();
            string endNumber = tbEndNumber.Text;
            string startNumber = tbStartNumber.Text;
            long count = Convert.ToInt64(endNumber) - Convert.ToInt64(startNumber);
            List<Distribution> lstDistributions = new List<Distribution>();
            bool hasErrors = false;
            for (int i = 0; i <= count; i++) {
                Distribution d = new Distribution();
                d.Date = DateTime.Today;
                d.BranchID = null;
                d.InsuranceCompanyID = int.Parse(ddlInsuranceCompany.SelectedValue);
                d.InsuranceSubTypeID = int.Parse(ddlInsuranceSubType.SelectedValue);
                d.UserID = Convert.ToInt32(ddlUsers.SelectedValue);
                string number = (Convert.ToInt64(tbStartNumber.Text) + i).ToString();
                for (int j = 0; j < (endNumber.Length - Convert.ToInt64(number).ToString().Length); j++) {
                    number = "0" + number;
                }
                Broker.DataAccess.Parameter parHasPrefix = Broker.DataAccess.Parameter.GetByCode(Broker.DataAccess.Parameter.SE_KORISTAT_PREFIKSI);
                if (parHasPrefix != null) {
                    bool hasPrefix = Convert.ToBoolean(parHasPrefix.Value);
                    if (hasPrefix == true) {
                        string prefix = tbInsuranceCompanyPrefix.Text;
                        number = prefix + number;
                    }
                }
                d.PolicyNumber = number;
                d.IsUsed = false;
                d.DistributionStatusID = DistributionStatuse.GetByCode(DistributionStatuse.POTVRDENA).ID;
                d.ValidateNumberForPriem();
                if (d.ValidationErrors.Count > 0) {
                    hasErrors = true;
                    break;
                } else {
                    lstDistributions.Add(d);
                }
            }
            if (hasErrors == false) {
                DistributionDocument dd = DistributionDocument.GetByDocumentNumber(tbDocumentNumber.Text);
                if (dd == null) {
                    dd = new DistributionDocument();
                    dd.BranchID = null;
                    dd.DistributionDocTypeID = ddt.ID;
                    dd.DocumentDate = DateTime.Today;
                    dd.DocumentNumber = tbDocumentNumber.Text;
                    dd.DocumentStatusID = DistributionDocumentStatuse.GetByCode(DistributionDocumentStatuse.PRIMEN).ID;
                    dd.Description = tbDescription.Text;
                    dd.Insert();
                }
                //Add routes
                DistributionRoute dr = new DistributionRoute();
                dr.Date = DateTime.Today;
                dr.DistributionDocumentID = dd.ID;
                dr.FromNumber = startNumber;
                dr.ToNumber = endNumber;
                dr.FromUserID = this.PageUser.ID;
                dr.ToUserID = Convert.ToInt32(ddlUsers.SelectedValue);
                dr.InsuranceCompanyID = Convert.ToInt32(ddlInsuranceCompany.SelectedValue);
                dr.InsuranceSubTypeID = Convert.ToInt32(ddlInsuranceSubType.SelectedValue);
                dr.Insert();
                //Insert distributions
                foreach (Distribution distr in lstDistributions) {
                    distr.Insert();
                    DistributionDocumentItem ddi = new DistributionDocumentItem();
                    ddi.DistributionDocumentID = dd.ID;
                    ddi.DistributionID = distr.ID;
                    ddi.Insert();
                    addedDistributions.Add(distr);
                }
                btnPrintDocument.Enabled = true;
                lblError.Text = "";
                lblError.Visible = false;
            } else {
                lblError.Visible = true;
                lblError.Text = "Постојат задолжени полиси од избраниот интервал!";
            }

            gvNewDistrirutions.DataSource = addedDistributions;
            gvNewDistrirutions.DataBind();
            gvDistributions.DataBind();
        } else if (ddt.Code == DistributionDocType.ISPRATNICA) {
            List<Distribution> addedDistributions = new List<Distribution>();
            string endNumber = tbEndNumber.Text;
            string startNumber = tbStartNumber.Text;
            long count = Convert.ToInt64(endNumber) - Convert.ToInt64(startNumber);
            List<Distribution> lstDistributions = new List<Distribution>();
            bool hasErrors = false;
            for (int i = 0; i <= count; i++) {
                Distribution d = new Distribution();
                d.Date = DateTime.Today;
                d.BranchID = Convert.ToInt32(ddlBranches.SelectedValue);
                d.InsuranceCompanyID = int.Parse(ddlInsuranceCompany.SelectedValue);
                d.InsuranceSubTypeID = int.Parse(ddlInsuranceSubType.SelectedValue);
                d.UserID = Convert.ToInt32(ddlUsers.SelectedValue);
                string number = (Convert.ToInt64(tbStartNumber.Text) + i).ToString();
                for (int j = 0; j < (endNumber.Length - Convert.ToInt64(number).ToString().Length); j++) {
                    number = "0" + number;
                }
                Broker.DataAccess.Parameter parHasPrefix = Broker.DataAccess.Parameter.GetByCode(Broker.DataAccess.Parameter.SE_KORISTAT_PREFIKSI);
                if (parHasPrefix != null) {
                    bool hasPrefix = Convert.ToBoolean(parHasPrefix.Value);
                    if (hasPrefix == true) {
                        string prefix = tbInsuranceCompanyPrefix.Text;
                        number = prefix + number;
                    }
                }
                d.PolicyNumber = number;
                d.IsUsed = false;
                d.DistributionStatusID = DistributionStatuse.GetByCode(DistributionStatuse.NEPOTVRDENA).ID;
                d.ValidateNumberForIspratnica();
                if (d.ValidationErrors.Count > 0) {
                    hasErrors = true;
                    break;
                } else {
                    lstDistributions.Add(d);
                }
            }
            if (hasErrors == false) {
                DistributionDocument dd = DistributionDocument.GetByDocumentNumber(tbDocumentNumber.Text);
                if (dd == null) {
                    dd = new DistributionDocument();
                    dd.BranchID = Convert.ToInt32(ddlBranches.SelectedValue); ;
                    dd.DistributionDocTypeID = ddt.ID;
                    dd.DocumentDate = DateTime.Today;
                    dd.DocumentNumber = tbDocumentNumber.Text;
                    dd.DocumentStatusID = DistributionDocumentStatuse.GetByCode(DistributionDocumentStatuse.ZA_PRIMANjE).ID;
                    dd.Description = tbDescription.Text;
                    dd.Insert();
                }
                //Add routes
                DistributionRoute dr = new DistributionRoute();
                dr.Date = DateTime.Today;
                dr.DistributionDocumentID = dd.ID;
                dr.FromNumber = startNumber;
                dr.ToNumber = endNumber;
                dr.FromUserID = this.PageUser.ID;
                dr.ToUserID = Convert.ToInt32(ddlUsers.SelectedValue);
                dr.InsuranceCompanyID = Convert.ToInt32(ddlInsuranceCompany.SelectedValue);
                dr.InsuranceSubTypeID = Convert.ToInt32(ddlInsuranceSubType.SelectedValue);
                dr.Insert();
                //Update distributions
                foreach (Distribution distr in lstDistributions) {
                    Distribution dis = Distribution.Table.Where(c => c.InsuranceCompanyID == distr.InsuranceCompanyID && c.InsuranceSubTypeID == dr.InsuranceSubTypeID
                        && c.PolicyNumber == distr.PolicyNumber).SingleOrDefault();
                    dis.BranchID = Convert.ToInt32(ddlBranches.SelectedValue);
                    dis.DistributionStatusID = DistributionStatuse.GetByCode(DistributionStatuse.NEPOTVRDENA).ID;
                    dis.UserID = Convert.ToInt32(ddlUsers.SelectedValue);
                    Distribution.Table.Context.SubmitChanges();
                    DistributionDocumentItem ddi = new DistributionDocumentItem();
                    ddi.DistributionDocumentID = dd.ID;
                    ddi.DistributionID = dis.ID;
                    ddi.Insert();
                    addedDistributions.Add(dis);
                }
                btnPrintDocument.Enabled = true;
                lblError.Text = "";
                lblError.Visible = false;
            } else {
                lblError.Visible = true;
                lblError.Text = "Полисите од избраниот интервал не се задолжени во централниот магацин или се задолжени во филијала!";
            }

            gvNewDistrirutions.DataSource = addedDistributions;
            gvNewDistrirutions.DataBind();
            gvDistributions.DataBind();
        } else if (ddt.Code == DistributionDocType.POVRATNICA) {
            List<Distribution> addedDistributions = new List<Distribution>();
            string endNumber = tbEndNumber.Text;
            string startNumber = tbStartNumber.Text;
            long count = Convert.ToInt64(endNumber) - Convert.ToInt64(startNumber);
            List<Distribution> lstDistributions = new List<Distribution>();
            bool hasErrors = false;
            for (int i = 0; i <= count; i++) {
                Distribution d = new Distribution();
                d.Date = DateTime.Today;
                d.BranchID = null;
                d.InsuranceCompanyID = int.Parse(ddlInsuranceCompany.SelectedValue);
                d.InsuranceSubTypeID = int.Parse(ddlInsuranceSubType.SelectedValue);
                d.UserID = Convert.ToInt32(ddlUsers.SelectedValue);
                string number = (Convert.ToInt64(tbStartNumber.Text) + i).ToString();
                for (int j = 0; j < (endNumber.Length - Convert.ToInt64(number).ToString().Length); j++) {
                    number = "0" + number;
                }
                Broker.DataAccess.Parameter parHasPrefix = Broker.DataAccess.Parameter.GetByCode(Broker.DataAccess.Parameter.SE_KORISTAT_PREFIKSI);
                if (parHasPrefix != null) {
                    bool hasPrefix = Convert.ToBoolean(parHasPrefix.Value);
                    if (hasPrefix == true) {
                        string prefix = tbInsuranceCompanyPrefix.Text;
                        number = prefix + number;
                    }
                }
                d.PolicyNumber = number;
                d.IsUsed = false;
                d.DistributionStatusID = DistributionStatuse.GetByCode(DistributionStatuse.VRATENA).ID;
                d.ValidateNumberForPovratnica(Convert.ToInt32(ddlBranches.SelectedValue));
                if (d.ValidationErrors.Count > 0) {
                    hasErrors = true;
                    break;
                } else {
                    lstDistributions.Add(d);
                }
            }
            if (hasErrors == false) {
                DistributionDocument dd = DistributionDocument.GetByDocumentNumber(tbDocumentNumber.Text);
                if (dd == null) {
                    dd = new DistributionDocument();
                    dd.BranchID = Convert.ToInt32(ddlBranches.SelectedValue); ;
                    dd.DistributionDocTypeID = ddt.ID;
                    dd.DocumentDate = DateTime.Today;
                    dd.DocumentNumber = tbDocumentNumber.Text;
                    dd.DocumentStatusID = DistributionDocumentStatuse.GetByCode(DistributionDocumentStatuse.ZA_PRIMANjE).ID;
                    dd.Description = tbDescription.Text;
                    dd.Insert();
                }
                //Add routes
                DistributionRoute dr = new DistributionRoute();
                dr.Date = DateTime.Today;
                dr.DistributionDocumentID = dd.ID;
                dr.FromNumber = startNumber;
                dr.ToNumber = endNumber;
                dr.FromUserID = Convert.ToInt32(ddlUsers.SelectedValue);
                dr.ToUserID = this.PageUser.ID;
                dr.InsuranceCompanyID = Convert.ToInt32(ddlInsuranceCompany.SelectedValue);
                dr.InsuranceSubTypeID = Convert.ToInt32(ddlInsuranceSubType.SelectedValue);
                dr.Insert();
                //Update distributions
                foreach (Distribution distr in lstDistributions) {
                    Distribution dis = Distribution.Table.Where(c => c.InsuranceCompanyID == distr.InsuranceCompanyID && c.InsuranceSubTypeID == dr.InsuranceSubTypeID
                        && c.PolicyNumber == distr.PolicyNumber).SingleOrDefault();
                    dis.BranchID = null;
                    dis.DistributionStatusID = DistributionStatuse.GetByCode(DistributionStatuse.VRATENA).ID;
                    dis.UserID = Broker.DataAccess.User.GetFirstSEAdminUser().ID;
                    Distribution.Table.Context.SubmitChanges();
                    DistributionDocumentItem ddi = new DistributionDocumentItem();
                    ddi.DistributionDocumentID = dd.ID;
                    ddi.DistributionID = dis.ID;
                    ddi.Insert();
                    addedDistributions.Add(dis);
                }
                btnPrintDocument.Enabled = true;
                lblError.Text = "";
                lblError.Visible = false;
            } else {
                lblError.Visible = true;
                lblError.Text = "Полисата е искористена или не постои како потврдена во филијала!!";
            }

            gvNewDistrirutions.DataSource = addedDistributions;
            gvNewDistrirutions.DataBind();
            gvDistributions.DataBind();
        }


    }

    protected void gvNewDistrirutions_PageIndexChanging(object sender, GridViewPageEventArgs e) {
        gvNewDistrirutions.PageIndex = e.NewPageIndex;
    }

    protected void gvNewDistrirutions_PageIndexChanged(object sender, EventArgs e) {
        //Distributions distributions = (Distributions)ViewState["NewDistributions"];
        int insuranceCompanyID = Convert.ToInt32(ddlInsuranceCompany.SelectedValue);
        int insuranceSubTypeID = Convert.ToInt32(ddlInsuranceSubType.SelectedValue);
        int userID = Convert.ToInt32(ddlUsers.SelectedValue);
        List<Distribution> NewAddedDistributions = Distribution.Table.Where(d => d.Date.Date == DateTime.Now.Date && d.InsuranceCompanyID == insuranceCompanyID && d.InsuranceSubTypeID == insuranceSubTypeID && d.UserID == userID).ToList();
        gvNewDistrirutions.DataSource = NewAddedDistributions;
        gvNewDistrirutions.DataBind();
    }
}
