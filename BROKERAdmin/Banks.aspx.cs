using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Broker.DataAccess;
using Broker.Controllers.ManagementControllers;

public partial class BROKERAdmin_Banks : AuthenticationPage
{
    public int GXGridView1SelectedValue
    {
        get
        {
            if (ViewState["GXGridView1SelectedValue"] != null)
            {
                return int.Parse(ViewState["GXGridView1SelectedValue"].ToString());
            } else
            {
                return 0;
            }
        }
        set
        {
            ViewState["GXGridView1SelectedValue"] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            GXGridView1.TotalRecords = ActiveBank.SelectCountCached();
            mvMain.SetActiveView(viewGrid);
            reportControl.BranchName = BrokerHouseInformation.GetBrokerHouseNameByCode(BrokerHouseInformation.FIRST_ROW);
            reportControl.CompanyName = "Брокерско друштво";
        }
    }

    protected void btnEdit_Click(object sender, EventArgs e)
    {
        if (GXGridView1.SelectedIndex != -1)
        {
            if (GXGridView1.SelectedDataKey != null)
            {
                int GXgvSelectedValue = Convert.ToInt32(GXGridView1.SelectedValue);
                dvDataSource.SelectParameters.Clear();
                dvDataSource.SelectParameters.Add("id", GXgvSelectedValue.ToString());
                DetailsView1.DataBind();
            } else
            {
                int GXgvSelectedValue = GXGridView1SelectedValue;
                dvDataSource.SelectParameters.Clear();
                dvDataSource.SelectParameters.Add("id", GXgvSelectedValue.ToString());
                DetailsView1.DataBind();
            }
            mvMain.SetActiveView(viewEdit);
            DetailsView1.ChangeMode(DetailsViewMode.Edit);
            btnNew.CssClass = "novZapis";
            btnEdit.CssClass = "izmeni_Active";
            btnDelete.CssClass = "izbrisi";
            btnPreview.CssClass = "osvezi";
            btnReport.CssClass = "izvestaj";
            btnSearch.CssClass = "prebaraj";
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        mvMain.SetActiveView(viewSearch);
        btnNew.CssClass = "novZapis";
        btnEdit.CssClass = "izmeni";
        btnDelete.CssClass = "izbrisi";
        btnPreview.CssClass = "osvezi";
        btnReport.CssClass = "izvestaj";
        btnSearch.CssClass = "prebaraj_Active";
    }
    protected void btnNew_Click(object sender, EventArgs e)
    {
        mvMain.SetActiveView(viewEdit);
        DetailsView1.ChangeMode(DetailsViewMode.Insert);
        btnNew.CssClass = "novZapis_Active";
        btnEdit.CssClass = "izmeni";
        btnDelete.CssClass = "izbrisi";
        btnPreview.CssClass = "osvezi";
        btnReport.CssClass = "izvestaj";
        btnSearch.CssClass = "prebaraj";
    }
    protected void btnPreview_Click(object sender, EventArgs e)
    {
        mvMain.SetActiveView(viewGrid);
        SearchControl1.SearchArguments = null;
        GXGridView1.DataSourceID = odsGridView.ID;
        btnNew.CssClass = "novZapis";
        btnEdit.CssClass = "izmeni";
        btnDelete.CssClass = "izbrisi";
        btnPreview.CssClass = "osvezi_Active";
        btnReport.CssClass = "izvestaj";
        btnSearch.CssClass = "prebaraj";
    }
    protected void SearchControl1_Search(object sender, EventArgs e)
    {
        GXGridView1.TotalRecords = ActiveBank.SelectSearchCountCached(SearchControl1.SearchArguments);
        mvMain.SetActiveView(viewGrid);
    }
    protected void GXGridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "SingleClick")
        {
            btnDelete.Enabled = true;
            btnEdit.Enabled = true;
            int selIndex = Convert.ToInt32(e.CommandArgument);
            GXGridView1.SelectedIndex = selIndex;
            GXGridView1SelectedValue = Convert.ToInt32(GXGridView1.SelectedValue);
            DetailsView1.DataBind();
        }
        if (e.CommandName == "DoubleClick")
        {
            btnDelete.Enabled = true;
            btnEdit.Enabled = true;
            int selIndex = Convert.ToInt32(e.CommandArgument);
            GXGridView1.SelectedIndex = selIndex;
            GXGridView1SelectedValue = Convert.ToInt32(GXGridView1.SelectedValue);
            DetailsView1.DataBind();
            DetailsView1.ChangeMode(DetailsViewMode.Edit);
            mvMain.SetActiveView(viewEdit);
        }
    }
    protected void btnReport_Click(object sender, EventArgs e)
    {
        mvMain.SetActiveView(viewReport);
        btnNew.CssClass = "novZapis";
        btnEdit.CssClass = "izmeni";
        btnDelete.CssClass = "izbrisi";
        btnPreview.CssClass = "osvezi";
        btnReport.CssClass = "izvestaj_Active";
        btnSearch.CssClass = "prebaraj";
    }
    protected void FilterControl1_Filter(object sender, EventArgs e)
    {
        GXGridView1.TotalRecords = ActiveBank.SelectFilterCountCached(FilterControl1.FCFilterArgument);
    }
    protected void DetailsView1_ItemCommand(object sender, DetailsViewCommandEventArgs e)
    {
        if (e.CommandName == "Cancel")
        {
            mvMain.SetActiveView(viewGrid);
        }
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        if (GXGridView1.SelectedIndex != -1)
        {
            if (GXGridView1.SelectedDataKey != null)
            {
                int GXgvSelectedValue = Convert.ToInt32(GXGridView1.SelectedValue);
                dvDataSource.SelectParameters.Clear();
                dvDataSource.SelectParameters.Add("id", GXgvSelectedValue.ToString());
                DetailsView1.DataBind();
            } else
            {
                int GXgvSelectedValue = GXGridView1SelectedValue;
                dvDataSource.SelectParameters.Clear();
                dvDataSource.SelectParameters.Add("id", GXgvSelectedValue.ToString());
                DetailsView1.DataBind();
            }
            DetailsView1.ChangeMode(DetailsViewMode.ReadOnly);
            mvMain.SetActiveView(viewEdit);
            btnNew.CssClass = "novZapis";
            btnEdit.CssClass = "izmeni";
            btnDelete.CssClass = "izbrisi_Active";
            btnPreview.CssClass = "osvezi";
            btnReport.CssClass = "izvestaj";
            btnSearch.CssClass = "prebaraj";
        }
    }

    protected void DetailsView1_ItemInserted(object sender, DetailsViewInsertedEventArgs e)
    {
        if (e.Exception != null)
        {
            e.ExceptionHandled = true;
            e.KeepInInsertMode = true;
            ValidationUtility.ShowValidationErrors(this, e.Exception);
        } else
        {
            GXGridView1SelectedValue = Convert.ToInt32(e.Values["ID"]);
            GXGridView1.TotalRecords = ActiveBank.SelectCountCached();
            GXGridView1.DataBind();
            mvMain.SetActiveView(viewGrid);
        }
    }

    protected void DetailsView1_ItemUpdated(object sender, DetailsViewUpdatedEventArgs e)
    {
        if (e.Exception != null)
        {
            e.ExceptionHandled = true;
            e.KeepInEditMode = true;
            ValidationUtility.ShowValidationErrors(this, e.Exception);
        } else
        {
            GXGridView1SelectedValue = Convert.ToInt32(e.NewValues["ID"]);
            GXGridView1.TotalRecords = ActiveBank.SelectCountCached();
            GXGridView1.DataBind();
            mvMain.SetActiveView(viewGrid);
        }
    }

    protected void DetailsView1_ItemDeleted(object sender, DetailsViewDeletedEventArgs e)
    {
        GXGridView1.TotalRecords = ActiveBank.SelectCountCached();
        GXGridView1.DataBind();
        mvMain.SetActiveView(viewGrid);
    }

    protected void DetailsView1_ModeChanging(object sender, DetailsViewModeEventArgs e)
    {
        e.Cancel = true;
    }

    protected void DetailsView1_ItemInserting(object sender, DetailsViewInsertEventArgs e)
    {
        e.Values["IsActive"] = true;
    }

    protected void dvDataSource_Updating(object sender, ObjectDataSourceMethodEventArgs e)
    {
        Broker.DataAccess.Bank b = e.InputParameters["newEntity"] as Broker.DataAccess.Bank;
        BankController.ValidateUpdateCode(b.ID, b.Code, b.BankAccount);
    }

    protected void dvDataSource_Updated(object sender, ObjectDataSourceStatusEventArgs e)
    {

    }
    protected void dvDataSource_Inserted(object sender, ObjectDataSourceStatusEventArgs e)
    {

    }
    protected void dvDataSource_Inserting(object sender, ObjectDataSourceMethodEventArgs e)
    {
        Broker.DataAccess.Bank b = e.InputParameters["entityToInsert"] as Broker.DataAccess.Bank;
        BankController.ValidateInsertCode(b.Code, b.BankAccount);
    }

    protected void btnDelete_Click1(object sender, EventArgs e)
    {
        if (GXGridView1.SelectedIndex != -1)
        {
            int bankID = 0;
            if (GXGridView1.SelectedDataKey != null)
            {
                int GXgvSelectedValue = Convert.ToInt32(GXGridView1.SelectedValue);
                bankID = GXgvSelectedValue;
                dvDataSource.SelectParameters.Clear();
                dvDataSource.SelectParameters.Add("id", GXgvSelectedValue.ToString());
                DetailsView1.DataBind();
            } else
            {
                int GXgvSelectedValue = GXGridView1SelectedValue;
                bankID = GXgvSelectedValue;
                dvDataSource.SelectParameters.Clear();
                dvDataSource.SelectParameters.Add("id", GXgvSelectedValue.ToString());
                DetailsView1.DataBind();
            }
            Broker.DataAccess.Bank b = Broker.DataAccess.Bank.Get(bankID);
            b.IsActive = false;
            Broker.DataAccess.Bank.Table.Context.SubmitChanges();

            GXGridView1.TotalRecords = ActiveBank.SelectCountCached();
            GXGridView1.DataBind();
            mvMain.SetActiveView(viewGrid);
        }
    }

    protected void btnLog_Click(object sender, EventArgs e)
    {
        if (GXGridView1.SelectedIndex != -1)
        {
            string id = Convert.ToInt32(GXGridView1.SelectedValue).ToString();
            using (Broker.LoggingDataAccess.LoggingDataContext dc = new Broker.LoggingDataAccess.LoggingDataContext())
            {


                string relativeUrl = this.WebPage.RelativeUrl;
                List<Broker.LoggingDataAccess.ViewDBLoggingByPageWithTableID> listLog = dc.ViewDBLoggingByPageWithTableIDs.Where(c => c.RelativeUrl.Contains(relativeUrl) && c.FieldName == "ID" && c.Value == id).OrderBy(x => x.ID).ToList();

                string script = string.Empty;
                foreach (Broker.LoggingDataAccess.ViewDBLoggingByPageWithTableID logItem in listLog)
                {
                    script += logItem.Action + ": " + logItem.Name + " на " + logItem.TimeOfUpdate.ToShortDateString() + " во " + logItem.TimeOfUpdate.ToShortTimeString() + "\\n";
                }


                string alert = "<script>alert('" + script + "')</script>";


                RegisterStartupScript("myAlert", alert);


            }
        }
    }

    protected void btnLogAccess_Click(object sender, EventArgs e)
    {
        if (GXGridView1.SelectedIndex != -1)
        {
            string id = Convert.ToInt32(GXGridView1.SelectedValue).ToString();
            using (Broker.LoggingDataAccess.LoggingDataContext dc = new Broker.LoggingDataAccess.LoggingDataContext())
            {
                string relativeUrl = this.WebPage.RelativeUrl;
                List<Broker.LoggingDataAccess.ViewDBOtvoreniEdit> listLog = dc.ViewDBOtvoreniEdits.Where(c => c.RelativeUrl.Contains(relativeUrl) && c.OpenedFormID == id).OrderBy(x => x.TimeAccessed).ToList();

                string script = string.Empty;
                foreach (Broker.LoggingDataAccess.ViewDBOtvoreniEdit logItem in listLog)
                {
                    script += logItem.Action + ": " + logItem.Name + " на " + logItem.TimeAccessed.ToShortDateString() + " во " + logItem.TimeAccessed.ToShortTimeString() + "\\n";
                }

                string alert = "<script>alert('" + script + "')</script>";


                RegisterStartupScript("myAlert", alert);


            }
        }
    }
}