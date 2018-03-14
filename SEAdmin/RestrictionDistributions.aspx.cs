using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Broker.DataAccess;

public partial class SEAdmin_RestrictionDistributions : AuthenticationPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            gvDistributions.TotalRecords = ViewRightRestrictionDistribution.SelectCountCached();
            mvMain.SetActiveView(viewGrid);
            reportControl.BranchName = BrokerHouseInformation.GetBrokerHouseNameByCode(BrokerHouseInformation.FIRST_ROW);
            reportControl.CompanyName = "Брокерско друштво";
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        mvMain.SetActiveView(viewSearch);
        btnNew.CssClass = "novZapis";
        btnPreview.CssClass = "osvezi";
        btnReport.CssClass = "izvestaj";
        btnSearch.CssClass = "prebaraj_Active";
    }

    protected void btnNew_Click(object sender, EventArgs e)
    {
        mvMain.SetActiveView(viewEdit);
        btnNew.CssClass = "novZapis_Active";
        btnPreview.CssClass = "osvezi";
        btnReport.CssClass = "izvestaj";
        btnSearch.CssClass = "prebaraj";
    }

    protected void btnPreview_Click(object sender, EventArgs e)
    {
        mvMain.SetActiveView(viewGrid);
        SearchControl1.SearchArguments = null;
        gvDistributions.DataSourceID = gvDataSource.ID;
        btnNew.CssClass = "novZapis";
        btnPreview.CssClass = "osvezi_Active";
        btnReport.CssClass = "izvestaj";
        btnSearch.CssClass = "prebaraj";
    }

    protected void SearchControl1_Search(object sender, EventArgs e)
    {
        gvDistributions.TotalRecords = ViewRightRestrictionDistribution.SelectSearchCountCached(SearchControl1.SearchArguments);
        mvMain.SetActiveView(viewGrid);
    }

    protected void GXGridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "SingleClick")
        {
            int selIndex = Convert.ToInt32(e.CommandArgument);
            gvDistributions.SelectedIndex = selIndex;
        }
    }
    protected void btnReport_Click(object sender, EventArgs e)
    {
        mvMain.SetActiveView(viewReport);
        btnNew.CssClass = "novZapis";
        btnPreview.CssClass = "osvezi";
        btnReport.CssClass = "izvestaj_Active";
        btnSearch.CssClass = "prebaraj";
    }
    protected void FilterControl1_Filter(object sender, EventArgs e)
    {
        gvDistributions.TotalRecords = ViewRightRestrictionDistribution.SelectFilterCountCached(FilterControl1.FCFilterArgument);
    }



    protected void btnCreate_Click(object sender, EventArgs e)
    {
        List<RightRestrictionDistribution> addedDistributions = new List<RightRestrictionDistribution>();
        int docTypeID = int.Parse(ddlDocumentType.SelectedValue);
        string endNumber = tbEndNumber.Text;
        string startNumber = tbStartNumber.Text;
        int count = int.Parse(endNumber) - int.Parse(startNumber);
        for (int i = 0; i <= count; i++)
        {
            RightRestrictionDistribution d = new RightRestrictionDistribution();
            d.Date = DateTime.Today;
            d.InsuranceCompanyID = int.Parse(ddlInsuranceCompany.SelectedValue);
            d.UserID = int.Parse(ddlUsers.SelectedValue);
            string zeros = string.Empty;

            string number = (int.Parse(tbStartNumber.Text) + i).ToString();
            for (int j = 0; j < (endNumber.Length - int.Parse(number).ToString().Length); j++)
            {
                number = "0" + number;
            }
            d.Number = number;

            d.IsUsed = false;
            d.DocumentTypeID = docTypeID;
            

            d.ValidateNumber();
            if (d.ValidationErrors.Count > 0)
            {
                lblError.Visible = true;
                lblError.Text = "Бројот " + d.Number + " е веќе задолжен";
              gvNewDistrirutions.DataSource = addedDistributions;
              gvNewDistrirutions.DataBind();
                gvDistributions.DataBind();
                return;
            } else
            {
                lblError.Visible = false;
                d.Insert();
                addedDistributions.Add(d);
                //distributions.Add(d);
            }
        
        //ViewState["NewDistributions"] = addedDistributions;
        gvNewDistrirutions.DataSource = addedDistributions;
        gvNewDistrirutions.DataBind();
        
        
        gvDistributions.TotalRecords = ViewRightRestrictionDistribution.SelectCountCached();
        gvDistributions.DataBind();
        }
     //   mvMain.SetActiveView(viewGrid);

    }
    protected void gvNewDistrirutions_PageIndexChanging(object sender, GridViewPageEventArgs e) {
        gvNewDistrirutions.PageIndex = e.NewPageIndex;
    }

    protected void gvNewDistrirutions_PageIndexChanged(object sender, EventArgs e) {
        //Distributions distributions = (Distributions)ViewState["NewDistributions"];
        int insuranceCompanyID = Convert.ToInt32(ddlInsuranceCompany.SelectedValue);
        //int insuranceSubTypeID = Convert.ToInt32(ddlInsuranceSubType.SelectedValue);
        int userID = Convert.ToInt32(ddlUsers.SelectedValue);
        //List<Distribution> NewAddedDistributions = Distribution.Table.Where(d => d.Date.Date == DateTime.Now.Date && d.InsuranceCompanyID == insuranceCompanyID && d.InsuranceSubTypeID == insuranceSubTypeID && d.Name == tbDistributionName.Text && d.UserID == userID).ToList();
        //gvNewDistrirutions.DataSource = NewAddedDistributions;
        //gvNewDistrirutions.DataBind();
    }
}
