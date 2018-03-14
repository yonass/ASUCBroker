using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Broker.DataAccess;
using Broker.Controllers.ManagementControllers;

public partial class BROKERAdmin_Packets : AuthenticationPage {

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

    protected void Page_Load(object sender, EventArgs e) {
        if (!IsPostBack) {
            GXGridView1.TotalRecords = ActivePacket.SelectCountCached();
            mvMain.SetActiveView(viewGrid);
            reportControl.BranchName = BrokerHouseInformation.GetBrokerHouseNameByCode(BrokerHouseInformation.FIRST_ROW);
            reportControl.CompanyName = "Брокерско друштво";
        
        }
    }
   

    protected void btnBrokerage_Click(object sender, EventArgs e)
    {
        if (GXGridView1.SelectedIndex != -1)
        {
            mvMain.SetActiveView(viewPercentages);
            GridViewPercentages.DataBind();
        }
    }

    protected void btnEdit_Click(object sender, EventArgs e) {
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
    protected void btnSearch_Click(object sender, EventArgs e) {
        mvMain.SetActiveView(viewSearch);
        btnNew.CssClass = "novZapis";
        btnEdit.CssClass = "izmeni";
        btnDelete.CssClass = "izbrisi";
        btnPreview.CssClass = "osvezi";
        btnReport.CssClass = "izvestaj";
        btnSearch.CssClass = "prebaraj_Active";
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
    }
    protected void SearchControl1_Search(object sender, EventArgs e) {
        GXGridView1.TotalRecords = ActivePacket.SelectSearchCountCached(SearchControl1.SearchArguments);
        mvMain.SetActiveView(viewGrid);
    }
    protected void GXGridView1_RowCommand(object sender, GridViewCommandEventArgs e) {
        if (e.CommandName == "SingleClick") {
            btnDelete.Enabled = true;
            btnEdit.Enabled = true;
            int selIndex = Convert.ToInt32(e.CommandArgument);
            GXGridView1.SelectedIndex = selIndex;
            GXGridView1SelectedValue = Convert.ToInt32(GXGridView1.SelectedValue);
            DetailsView1.DataBind();
        }
        if (e.CommandName == "DoubleClick") {
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
    protected void btnReport_Click(object sender, EventArgs e) {
        mvMain.SetActiveView(viewReport);
        btnNew.CssClass = "novZapis";
        btnEdit.CssClass = "izmeni";
        btnDelete.CssClass = "izbrisi";
        btnPreview.CssClass = "osvezi";
        btnReport.CssClass = "izvestaj_Active";
        btnSearch.CssClass = "prebaraj";
    }
    protected void FilterControl1_Filter(object sender, EventArgs e) {
        GXGridView1.TotalRecords = ActivePacket.SelectFilterCountCached(FilterControl1.FCFilterArgument);
    }
    protected void DetailsView1_ItemCommand(object sender, DetailsViewCommandEventArgs e) {
        if (e.CommandName == "Cancel") {
            mvMain.SetActiveView(viewGrid);
        }
    }
    protected void btnDelete_Click(object sender, EventArgs e) {
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

    protected void DetailsView1_ItemInserted(object sender, DetailsViewInsertedEventArgs e) {
        if (e.Exception != null) {
            e.ExceptionHandled = true;
            e.KeepInInsertMode = true;
            ValidationUtility.ShowValidationErrors(this, e.Exception);
        } else {
            GXGridView1.TotalRecords = ActivePacket.SelectCountCached();
            GXGridView1.DataBind();
            mvMain.SetActiveView(viewGrid);
        }
    }

    protected void DetailsView1_ItemUpdated(object sender, DetailsViewUpdatedEventArgs e) {
        if (e.Exception != null) {
            e.ExceptionHandled = true;
            e.KeepInEditMode = true;
            ValidationUtility.ShowValidationErrors(this, e.Exception);
        } else {
            GXGridView1.TotalRecords = ActivePacket.SelectCountCached();
            GXGridView1.DataBind();
            mvMain.SetActiveView(viewGrid);
        }
    }

    protected void DetailsView1_ItemDeleted(object sender, DetailsViewDeletedEventArgs e) {
        GXGridView1.TotalRecords = ActivePacket.SelectCountCached();
        GXGridView1.DataBind();
        mvMain.SetActiveView(viewGrid);
    }

    protected void DetailsView1_ModeChanging(object sender, DetailsViewModeEventArgs e) {
        e.Cancel = true;
    }

    protected void DetailsView1_ItemInserting(object sender, DetailsViewInsertEventArgs e) {
        e.Values["IsActive"] = true;
    }

    protected void dvDataSource_Updating(object sender, ObjectDataSourceMethodEventArgs e) {
        Packet p = e.InputParameters["newEntity"] as Packet;
        PacketController.ValidateUpdateCode(p.ID, p.Code);
    }

    protected void dvDataSource_Updated(object sender, ObjectDataSourceStatusEventArgs e) {

    }
    protected void dvDataSource_Inserted(object sender, ObjectDataSourceStatusEventArgs e) {
        Packet p = (Packet)e.ReturnValue;

        List<int> new_InsuranceSubTypeID = new List<int>();
        List<PacketsInsuranceSubType> new_values = new List<PacketsInsuranceSubType>();
        int packetID = p.ID;
        CheckBoxList cbl = (CheckBoxList)DetailsView1.FindControl("InsuranceSubTypes");
        foreach (ListItem item in cbl.Items) {
            if (item.Selected == true)
                new_InsuranceSubTypeID.Add(Convert.ToInt32(item.Value));
        }

        foreach (int istID in new_InsuranceSubTypeID) {
            PacketsInsuranceSubType pist = new PacketsInsuranceSubType();
            pist.InsuranceSubTypeID = istID;
            pist.PacketID = packetID;
            pist.BrokeragePecentageForPrivates = PacketsInsuranceSubType.GetDefaultBrokerageForPrivates(p.InsuranceCompanyID, istID);
            pist.BrokeragePecentageForLaws = PacketsInsuranceSubType.GetDefaultBrokerageForLaws(p.InsuranceCompanyID, istID);
            new_values.Add(pist);
        }

        PacketsInsuranceSubType.Table.InsertAllOnSubmit(new_values);
        PacketsInsuranceSubType.Table.Context.SubmitChanges();
    }
    protected void dvDataSource_Inserting(object sender, ObjectDataSourceMethodEventArgs e) {
        Packet p = e.InputParameters["entityToInsert"] as Packet;
        PacketController.ValidateInsertCode(p.Code);
    }

    protected void btnDelete_Click1(object sender, EventArgs e)
    {
        if (GXGridView1.SelectedIndex != -1)
        {
            int packetID = 0;
            if (GXGridView1.SelectedDataKey != null)
            {
                int GXgvSelectedValue = Convert.ToInt32(GXGridView1.SelectedValue);
                packetID = GXgvSelectedValue;
                dvDataSource.SelectParameters.Clear();
                dvDataSource.SelectParameters.Add("id", GXgvSelectedValue.ToString());
                DetailsView1.DataBind();
            } else
            {
                int GXgvSelectedValue = GXGridView1SelectedValue;
                packetID = GXgvSelectedValue;
                dvDataSource.SelectParameters.Clear();
                dvDataSource.SelectParameters.Add("id", GXgvSelectedValue.ToString());
                DetailsView1.DataBind();
            }

            Packet p = Packet.Get(packetID);
            p.IsActive = false;
            Packet.Table.Context.SubmitChanges();

            GXGridView1.TotalRecords = ActivePacket.SelectCountCached();
            GXGridView1.DataBind();
            mvMain.SetActiveView(viewGrid);
        }
    }
    protected void DetailsView1_DataBound(object sender, EventArgs e) {
        int packetID;
        if (GXGridView1.SelectedDataKey != null) {
            packetID = Convert.ToInt32(GXGridView1.SelectedDataKey.Value);
        } else {
            return;
        }

        List<PacketsInsuranceSubType> my_list = PacketsInsuranceSubType.GetByPacket(packetID);
        CheckBoxList cbl = (CheckBoxList)DetailsView1.FindControl("InsuranceSubTypes");

        foreach (ListItem item in cbl.Items) {
            foreach (var some in my_list) {
                if (item.Value == some.InsuranceSubTypeID.ToString())
                    item.Selected = true;
            }
        }
    }
    protected void DetailsView1_ItemDeleting(object sender, DetailsViewDeleteEventArgs e) {
        int packetID = Convert.ToInt32(e.Keys["ID"]);
        List<PacketsInsuranceSubType> old_values = PacketsInsuranceSubType.GetByPacket(packetID);
        PacketsInsuranceSubType.Table.DeleteAllOnSubmit(old_values);
        PacketsInsuranceSubType.Table.Context.SubmitChanges();
    }
    protected void DetailsView1_ItemUpdating(object sender, DetailsViewUpdateEventArgs e) {
        int packetID = Convert.ToInt32(e.Keys["ID"]);
        List<PacketsInsuranceSubType> old_values = PacketsInsuranceSubType.GetByPacket(packetID);
        List<int> new_InsuranceSubTypsID = new List<int>();
        List<PacketsInsuranceSubType> new_values = new List<PacketsInsuranceSubType>();

        CheckBoxList cbl = (CheckBoxList)DetailsView1.FindControl("InsuranceSubTypes");
        foreach (ListItem item in cbl.Items) {
            if (item.Selected == true)
                new_InsuranceSubTypsID.Add(Convert.ToInt32(item.Value));
        }

        foreach (int iID in new_InsuranceSubTypsID) {
            PacketsInsuranceSubType pist = new PacketsInsuranceSubType();
            pist.PacketID = packetID;
            pist.InsuranceSubTypeID = iID;
            new_values.Add(pist);
        }

        PacketsInsuranceSubType.Table.DeleteAllOnSubmit(old_values);
        PacketsInsuranceSubType.Table.Context.SubmitChanges();
        PacketsInsuranceSubType.Table.InsertAllOnSubmit(new_values);
        PacketsInsuranceSubType.Table.Context.SubmitChanges();
    }
}