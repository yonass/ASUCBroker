using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Broker.DataAccess;
using System.Web.UI.HtmlControls;

public partial class Broker_PolicyTechnicalError : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            btnUpdate.Visible = false;
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        int insuranceCompanyID = Convert.ToInt32(ddlInsuranceCompany.SelectedValue);
        int insuranceTypeID = Convert.ToInt32(ddlInsuranceType.SelectedValue);
        int insuranceSubTypeID = Convert.ToInt32(ddlInsuranceSubTypes.SelectedValue);
        string policyNumber = tbPolicyNumber.Text;
        dvDataSource.SelectParameters.Clear();
        dvDataSource.SelectParameters.Add("policyNumber", policyNumber);
        dvDataSource.SelectParameters.Add("insuranceSubTypeID", insuranceSubTypeID.ToString());
        dvDataSource.SelectParameters.Add("insuranceCompanyID", insuranceCompanyID.ToString());
        PoliciesDetailsView.DataBind();
        PolicyItem pi = PolicyItem.GetByNumberAndInsuranceSubType(policyNumber, insuranceSubTypeID, insuranceCompanyID);
        if (pi != null)
        {
            btnUpdate.Visible = true;
            CreateChildControls();
        } else
        {
            btnUpdate.Visible = false;
        }
    }

    protected void ddlInsuranceTypeSelectedIndexChanged(object sender, EventArgs e)
    {
        ddlInsuranceSubTypes.DataBind();
        ddlInsuranceCompany.DataBind();
        dvDataSource.SelectParameters.Clear();
        dvDataSource.SelectParameters.Add("policyNumber", "");
        dvDataSource.SelectParameters.Add("insuranceSubTypeID", "0");
        dvDataSource.SelectParameters.Add("insuranceCompanyID", "0");
        PoliciesDetailsView.DataBind();
        //CreateChildControls();
        btnUpdate.Visible = false;
    }

    protected void ddlInsuranceSubType_selecteIndexChanged(object sender, EventArgs e)
    {
        ddlInsuranceCompany.DataBind();
        dvDataSource.SelectParameters.Clear();
        dvDataSource.SelectParameters.Add("policyNumber", "");
        dvDataSource.SelectParameters.Add("insuranceSubTypeID", "0");
        dvDataSource.SelectParameters.Add("insuranceCompanyID", "0");
        PoliciesDetailsView.DataBind();
        //CreateChildControls();
        btnUpdate.Visible = false;
    }

    protected void ddlInsuranceCompanyIndexChanged(object sender, EventArgs e)
    {
        dvDataSource.SelectParameters.Clear();
        dvDataSource.SelectParameters.Add("policyNumber", "");
        dvDataSource.SelectParameters.Add("insuranceSubTypeID", "0");
        dvDataSource.SelectParameters.Add("insuranceCompanyID", "0");
        PoliciesDetailsView.DataBind();
        //CreateChildControls();
        btnUpdate.Visible = false;
    }

    protected void btnClientEMBGSearch_Click(object sender, EventArgs e)
    {
        Panel pnlClient = PoliciesDetailsView.FindControl("pnlClient") as Panel;
        DetailsView clientDetailsView = pnlClient.FindControl("ClientDetailsView") as DetailsView;
        ObjectDataSource clientdvDataSource = pnlClient.FindControl("ClientdvDataSource") as ObjectDataSource;
        if (PoliciesDetailsView.SelectedValue != null)
        {
            int policyItemID = Convert.ToInt32(PoliciesDetailsView.SelectedValue);
            PolicyItem policyItem = PolicyItem.Get(policyItemID);
            string clientEMBG = policyItem.Policy.Client.EMBG;
            pnlClient.Visible = true;
            clientdvDataSource.SelectParameters.Clear();
            clientdvDataSource.SelectParameters.Add("embg", clientEMBG);
            clientDetailsView.DataBind();
        } else
        {
            pnlClient.Visible = false;
            clientdvDataSource.SelectParameters.Clear();
            clientDetailsView.DataBind();
        }
    }

    protected void btnOwnerEMBGSearch_Click(object sender, EventArgs e)
    {
        Panel pnlOwner = PoliciesDetailsView.FindControl("pnlOwner") as Panel;
        DetailsView ownerDetailsView = pnlOwner.FindControl("OwnerDetailsView") as DetailsView;
        ObjectDataSource ownerdvDataSource = pnlOwner.FindControl("OwnerdvDataSource") as ObjectDataSource;
        if (PoliciesDetailsView.SelectedValue != null)
        {
            int policyItemID = Convert.ToInt32(PoliciesDetailsView.SelectedValue);
            PolicyItem policyItem = PolicyItem.Get(policyItemID);
            string ownerEMBG = policyItem.Policy.Client1.EMBG;
            pnlOwner.Visible = true;
            ownerdvDataSource.SelectParameters.Clear();
            ownerdvDataSource.SelectParameters.Add("embg", ownerEMBG);
            ownerDetailsView.DataBind();
        } else
        {
            pnlOwner.Visible = false;
            ownerdvDataSource.SelectParameters.Clear();
            ownerDetailsView.DataBind();
        }
    }

    protected void ClientDetailsView_ItemCommand(object sender, DetailsViewCommandEventArgs e)
    {
        if (e.CommandName == "Cancel")
        {
            Panel pnl = PoliciesDetailsView.FindControl("pnlClient") as Panel;
            pnl.Visible = false;
        }
    }

    protected void OwnerDetailsView_ItemCommand(object sender, DetailsViewCommandEventArgs e)
    {
        if (e.CommandName == "Cancel")
        {
            Panel pnl = PoliciesDetailsView.FindControl("pnlOwner") as Panel;
            pnl.Visible = false;
        }
    }

    protected void ClientDetailsView_ModeChanging(object sender, DetailsViewModeEventArgs e)
    {
        e.Cancel = true;
    }

    protected void OwnerDetailsView_ModeChanging(object sender, DetailsViewModeEventArgs e)
    {
        e.Cancel = true;
    }

    protected void ClientDetailsView_ItemUpdated(object sender, DetailsViewUpdatedEventArgs e)
    {
        if (e.Exception != null)
        {
            e.ExceptionHandled = true;
            e.KeepInEditMode = true;
            ValidationUtility.ShowValidationErrors(this, e.Exception);
        } 
    }

    protected void OwnerDetailsView_ItemUpdated(object sender, DetailsViewUpdatedEventArgs e)
    {
        if (e.Exception != null)
        {
            e.ExceptionHandled = true;
            e.KeepInEditMode = true;
            ValidationUtility.ShowValidationErrors(this, e.Exception);
        } 
    }

    protected void dvDataSource_Updating(object sender, ObjectDataSourceMethodEventArgs e)
    {
    }

    protected void dvDataSource_Updated(object sender, ObjectDataSourceStatusEventArgs e)
    {

    }

    private List<Label> lblFieldNames;
    private List<TextBox> tbValues;
    protected override void CreateChildControls()
    {
        if (PoliciesDetailsView.SelectedValue != null)
        {
            int policyItemID = Convert.ToInt32(PoliciesDetailsView.SelectedValue);
            PolicyItem policyItem = PolicyItem.Get(policyItemID);
            Panel pnlExtendControls = PoliciesDetailsView.FindControl("pnlExtendControls") as Panel;
            int j = 0;
            List<Broker.DataAccess.Control> listControls = Broker.DataAccess.Control.GetByInsuranceSubType(policyItem.InsuranceSubTypeID);

            pnlExtendControls.Controls.Clear();
            lblFieldNames = new List<Label>();
            tbValues = new List<TextBox>();

            HtmlTable titleTable = new HtmlTable();
            titleTable.Width = "695px";
            HtmlTableRow firstTitleTableRow = new HtmlTableRow();
            titleTable.Rows.Add(firstTitleTableRow);
            HtmlTableCell firstCellFirstRowInTitleTable = new HtmlTableCell();
            firstTitleTableRow.Cells.Add(firstCellFirstRowInTitleTable);
            Label lblPremiumBrokerageTitle = new Label();
            lblPremiumBrokerageTitle.ID = "lblPremiumBrokerageTitle";
            lblPremiumBrokerageTitle.Font.Bold = true;
            lblPremiumBrokerageTitle.Text = "Премија и брокеража";
            firstCellFirstRowInTitleTable.Controls.Add(lblPremiumBrokerageTitle);
            pnlExtendControls.Controls.Add(titleTable);

            HtmlTable defaultTable = new HtmlTable();
            defaultTable.Width = "695px";

            HtmlTableRow fourthDefaultTableRow = new HtmlTableRow();
            fourthDefaultTableRow.BgColor = "#FAFAF8";
            defaultTable.Rows.Add(fourthDefaultTableRow);
            HtmlTableCell firstCellFourthRowInDefaultTable = new HtmlTableCell();
            firstCellFourthRowInDefaultTable.Width = "160px";
            fourthDefaultTableRow.Cells.Add(firstCellFourthRowInDefaultTable);
            Label lblPolicyValue = new Label();
            lblPolicyValue.ID = "lblPolicyValue" + (j + 1).ToString();
            lblPolicyValue.Text = "Премија";
            firstCellFourthRowInDefaultTable.Controls.Add(lblPolicyValue);
            HtmlTableCell secondCellFourthRowInDefaultTable = new HtmlTableCell();
            secondCellFourthRowInDefaultTable.Width = "187px";
            fourthDefaultTableRow.Cells.Add(secondCellFourthRowInDefaultTable);
            TextBox tbPolicyValue = new TextBox();
            tbPolicyValue.ID = "tbPolicyValue" + (j + 1).ToString();
            tbPolicyValue.AutoPostBack = true;
            tbPolicyValue.Text = policyItem.PremiumValue.ToString();
            tbPolicyValue.ReadOnly = true;
            
            tbPolicyValue.TextChanged += new EventHandler(tbPolicyValue_TextChanged);
            RequiredFieldValidator rfvPolicyValue = new RequiredFieldValidator();
            rfvPolicyValue.ID = "rfvPolicyValue" + (j + 1).ToString();
            rfvPolicyValue.ErrorMessage = "*";
            rfvPolicyValue.Display = ValidatorDisplay.Dynamic;
            rfvPolicyValue.ControlToValidate = tbPolicyValue.ID;
            CompareValidator cvPolicyValue = new CompareValidator();
            cvPolicyValue.ID = "cvPolicyValue" + (j + 1).ToString();
            cvPolicyValue.ErrorMessage = "*";
            cvPolicyValue.Display = ValidatorDisplay.Dynamic;
            cvPolicyValue.ControlToValidate = tbPolicyValue.ID;
            cvPolicyValue.Operator = ValidationCompareOperator.DataTypeCheck;
            cvPolicyValue.Type = ValidationDataType.Double;
            secondCellFourthRowInDefaultTable.Controls.Add(tbPolicyValue);
            secondCellFourthRowInDefaultTable.Controls.Add(rfvPolicyValue);
            secondCellFourthRowInDefaultTable.Controls.Add(cvPolicyValue);
            HtmlTableCell thirdCellFourthRowInDefaultTable = new HtmlTableCell();
            thirdCellFourthRowInDefaultTable.Width = "160px";
            fourthDefaultTableRow.Cells.Add(thirdCellFourthRowInDefaultTable);
            HtmlTableCell fourthCellFourthRowInDefaultTable = new HtmlTableCell();
            fourthCellFourthRowInDefaultTable.Width = "187px";
            fourthDefaultTableRow.Cells.Add(fourthCellFourthRowInDefaultTable);

            HtmlTableRow fifthDefaultTableRow = new HtmlTableRow();
            defaultTable.Rows.Add(fifthDefaultTableRow);
            HtmlTableCell firstCellFifthRowInDefaultTable = new HtmlTableCell();
            firstCellFifthRowInDefaultTable.Width = "160px";
            fifthDefaultTableRow.Cells.Add(firstCellFifthRowInDefaultTable);
            Label lblBrokeragePercentage = new Label();
            lblBrokeragePercentage.ID = "lblBrokeragePercentage" + (j + 1).ToString();
            lblBrokeragePercentage.Text = "Процент на брокеража";
            firstCellFifthRowInDefaultTable.Controls.Add(lblBrokeragePercentage);
            HtmlTableCell secondCellFifthRowInDefaultTable = new HtmlTableCell();
            secondCellFifthRowInDefaultTable.Width = "187px";
            fifthDefaultTableRow.Cells.Add(secondCellFifthRowInDefaultTable);
            TextBox tbBrokeragePercentage = new TextBox();
            tbBrokeragePercentage.ID = "tbBrokeragePercentage" + (j + 1).ToString();
            tbBrokeragePercentage.Text = policyItem.BrokeragePercentage.ToString();
            tbBrokeragePercentage.ReadOnly = true;
            secondCellFifthRowInDefaultTable.Controls.Add(tbBrokeragePercentage);
            HtmlTableCell thirdCellFifthRowInDefaultTable = new HtmlTableCell();
            thirdCellFifthRowInDefaultTable.Width = "160px";
            fifthDefaultTableRow.Cells.Add(thirdCellFifthRowInDefaultTable);
            HtmlTableCell fourthCellFifthRowInDefaultTable = new HtmlTableCell();
            fourthCellFifthRowInDefaultTable.Width = "187px";
            fifthDefaultTableRow.Cells.Add(fourthCellFifthRowInDefaultTable);

            HtmlTableRow sixthDefaultTableRow = new HtmlTableRow();
            sixthDefaultTableRow.BgColor = "#FAFAF8";
            defaultTable.Rows.Add(sixthDefaultTableRow);
            HtmlTableCell firstCellSixthRowInDefaultTable = new HtmlTableCell();
            firstCellSixthRowInDefaultTable.Width = "160px";
            sixthDefaultTableRow.Cells.Add(firstCellSixthRowInDefaultTable);
            Label lblBrokerageValue = new Label();
            lblBrokerageValue.ID = "lblBrokerageValue" + (j + 1).ToString();
            lblBrokerageValue.Text = "Брокеража";
            firstCellSixthRowInDefaultTable.Controls.Add(lblBrokerageValue);
            HtmlTableCell secondCellSixthRowInDefaultTable = new HtmlTableCell();
            secondCellSixthRowInDefaultTable.Width = "187px";
            sixthDefaultTableRow.Cells.Add(secondCellSixthRowInDefaultTable);
            TextBox tbBrokerageValue = new TextBox();
            tbBrokerageValue.ID = "tbBrokerageValue" + (j + 1).ToString();
            tbBrokerageValue.ReadOnly = true;
            tbBrokerageValue.Text = policyItem.BrokerageValue.ToString();
            secondCellSixthRowInDefaultTable.Controls.Add(tbBrokerageValue);
            HtmlTableCell thirdCellSixthRowInDefaultTable = new HtmlTableCell();
            thirdCellSixthRowInDefaultTable.Width = "160px";
            sixthDefaultTableRow.Cells.Add(thirdCellSixthRowInDefaultTable);
            HtmlTableCell fourthCellSixthRowInDefaultTable = new HtmlTableCell();
            fourthCellSixthRowInDefaultTable.Width = "187px";
            sixthDefaultTableRow.Cells.Add(fourthCellSixthRowInDefaultTable);

            pnlExtendControls.Controls.Add(defaultTable);

            HtmlTable titlePolicyTable = new HtmlTable();
            titlePolicyTable.Width = "695px";
            HtmlTableRow firstTitlePolicyTableRow = new HtmlTableRow();
            titlePolicyTable.Rows.Add(firstTitlePolicyTableRow);
            HtmlTableCell firstCellFirstRowInTitlePolicyTable = new HtmlTableCell();
            firstTitlePolicyTableRow.Cells.Add(firstCellFirstRowInTitlePolicyTable);
            Label lblPolicyDetailsTitle = new Label();
            lblPolicyDetailsTitle.ID = "lblPolicyDetailsTitle";
            lblPolicyDetailsTitle.Font.Bold = true;
            lblPolicyDetailsTitle.Text = "Детални информации за полиса";
            firstCellFirstRowInTitlePolicyTable.Controls.Add(lblPolicyDetailsTitle);
            pnlExtendControls.Controls.Add(titlePolicyTable);

            HtmlTable table = new HtmlTable();
            table.Width = "695px";
            int counter = 0;
            foreach (Broker.DataAccess.Control c in listControls)
            {
                if (c.IsActive)
                {
                    PolicyExtendInformation policyExtendInformation = PolicyExtendInformation.GetByPolicyItemAndControl(policyItem.ID, c.ID);
                    if (listControls.Count == 1)
                    {
                        HtmlTableRow tableRow = new HtmlTableRow();
                        tableRow.BgColor = "#FAFAF8";
                        table.Rows.Add(tableRow);
                        HtmlTableCell tableCell = new HtmlTableCell();
                        tableCell.Width = "160px";
                        tableRow.Cells.Add(tableCell);
                        Label label = new Label();
                        label.ID = c.LabelID;
                        label.Text = c.LabelName;
                        lblFieldNames.Add(label);
                        tableCell.Controls.Add(label);
                        HtmlTableCell tableCellSecond = new HtmlTableCell();
                        tableCellSecond.Width = "187px";
                        tableRow.Cells.Add(tableCellSecond);
                        TextBox textbox = new TextBox();
                        textbox.ID = c.TextBoxID;
                        if (policyExtendInformation != null)
                        {
                            textbox.Text = policyExtendInformation.Value;
                        }
                        if (c.IsLatin != null)
                        {
                            if ((bool)c.IsLatin==true)
                            {
                                textbox.Attributes.Add("onkeyup", "toEnglish(this.id)");
                            }
                        }
                        tbValues.Add(textbox);
                        tableCellSecond.Controls.Add(textbox);
                        if (c.HasRequredFieldValidator)
                        {
                            RequiredFieldValidator rfv = new RequiredFieldValidator();
                            rfv.ID = "rfv" + textbox.ID;
                            rfv.ErrorMessage = "*";
                            rfv.Display = ValidatorDisplay.Dynamic;
                            rfv.ControlToValidate = textbox.ID;
                            tableCellSecond.Controls.Add(rfv);
                        }
                        if (c.HasCompareValidator)
                        {
                            CompareValidator cv = new CompareValidator();
                            cv.ID = "cv" + textbox.ID;
                            cv.ErrorMessage = "*";
                            cv.Display = ValidatorDisplay.Dynamic;
                            cv.ControlToValidate = textbox.ID;
                            cv.Operator = ValidationCompareOperator.DataTypeCheck;
                            cv.Type = Broker.DataAccess.VariableType.GetForVariableType(c.VariableTypeID);
                            tableCellSecond.Controls.Add(cv);
                        }
                        HtmlTableCell tableCellThird = new HtmlTableCell();
                        tableCellThird.Width = "160px";
                        tableRow.Cells.Add(tableCellThird);
                        HtmlTableCell tableCellForth = new HtmlTableCell();
                        tableCellForth.Width = "187px";
                        tableRow.Cells.Add(tableCellForth);
                    } else
                    {
                        if (counter % 2 == 0)
                        {
                            HtmlTableRow tableRow = new HtmlTableRow();
                            if ((counter % 4 == 0) || (counter % 4 == 1))
                            {
                                tableRow.BgColor = "#FAFAF8";
                            }
                            table.Rows.Add(tableRow);
                            HtmlTableCell tableCell = new HtmlTableCell();
                            tableCell.Width = "160px";
                            tableRow.Cells.Add(tableCell);
                            Label label = new Label();
                            label.ID = c.LabelID;
                            label.Text = c.LabelName;
                            lblFieldNames.Add(label);
                            tableCell.Controls.Add(label);
                            HtmlTableCell tableCellSecond = new HtmlTableCell();
                            tableCellSecond.Width = "187px";
                            tableRow.Cells.Add(tableCellSecond);
                            TextBox textbox = new TextBox();
                            textbox.ID = c.TextBoxID;
                            if (policyExtendInformation != null)
                            {
                                textbox.Text = policyExtendInformation.Value;
                            }
                            if (c.IsLatin != null)
                            {
                                if ((bool)c.IsLatin==true)
                                {
                                    textbox.Attributes.Add("onkeyup", "toEnglish(this.id)");
                                }
                            }
                            tbValues.Add(textbox);
                            tableCellSecond.Controls.Add(textbox);
                            if (c.HasRequredFieldValidator)
                            {
                                RequiredFieldValidator rfv = new RequiredFieldValidator();
                                rfv.ID = "rfv" + textbox.ID;
                                rfv.ErrorMessage = "*";
                                rfv.Display = ValidatorDisplay.Dynamic;
                                rfv.ControlToValidate = textbox.ID;
                                tableCellSecond.Controls.Add(rfv);
                            }
                            if (c.HasCompareValidator)
                            {
                                CompareValidator cv = new CompareValidator();
                                cv.ID = "cv" + textbox.ID;
                                cv.ErrorMessage = "*";
                                cv.Display = ValidatorDisplay.Dynamic;
                                cv.ControlToValidate = textbox.ID;
                                cv.Operator = ValidationCompareOperator.DataTypeCheck;
                                cv.Type = Broker.DataAccess.VariableType.GetForVariableType(c.VariableTypeID);
                                tableCellSecond.Controls.Add(cv);
                            }
                        } else if (counter % 2 == 1)
                        {
                            HtmlTableRow tableRow = table.Rows[counter / 2];
                            if ((counter % 4 == 0) || (counter % 4 == 1))
                            {
                                tableRow.BgColor = "#FAFAF8";
                            }
                            HtmlTableCell tableCellThird = new HtmlTableCell();
                            tableCellThird.Width = "160px";
                            tableRow.Cells.Add(tableCellThird);
                            Label label = new Label();
                            label.ID = c.LabelID;
                            label.Text = c.LabelName;
                            lblFieldNames.Add(label);
                            tableCellThird.Controls.Add(label);
                            HtmlTableCell tableCellForth = new HtmlTableCell();
                            tableCellForth.Width = "187px";
                            tableRow.Cells.Add(tableCellForth);
                            TextBox textbox = new TextBox();
                            textbox.ID = c.TextBoxID;
                            if (policyExtendInformation != null)
                            {
                                textbox.Text = policyExtendInformation.Value;
                            }
                            if (c.IsLatin != null)
                            {
                                if ((bool)c.IsLatin)
                                {
                                    textbox.Attributes.Add("onkeyup", "toEnglish(this.id)");
                                }
                            }
                            tbValues.Add(textbox);
                            tableCellForth.Controls.Add(textbox);
                            if (c.HasRequredFieldValidator)
                            {
                                RequiredFieldValidator rfv = new RequiredFieldValidator();
                                rfv.ID = "rfv" + textbox.ID;
                                rfv.ErrorMessage = "*";
                                rfv.Display = ValidatorDisplay.Dynamic;
                                rfv.ControlToValidate = textbox.ID;
                                tableCellForth.Controls.Add(rfv);
                            }
                            if (c.HasCompareValidator)
                            {
                                CompareValidator cv = new CompareValidator();
                                cv.ID = "cv" + textbox.ID;
                                cv.ErrorMessage = "*";
                                cv.Display = ValidatorDisplay.Dynamic;
                                cv.ControlToValidate = textbox.ID;
                                cv.Operator = ValidationCompareOperator.DataTypeCheck;
                                cv.Type = Broker.DataAccess.VariableType.GetForVariableType(c.VariableTypeID);
                                tableCellForth.Controls.Add(cv);
                            }
                        }
                    }
                    counter++;
                }
            }
            pnlExtendControls.Controls.Add(table);
        }
    }

    void tbPolicyValue_TextChanged(object sender, EventArgs e)
    {
        TextBox tbPolicyValue = (TextBox)sender;
        int index = Convert.ToInt32(tbPolicyValue.ID.Substring(tbPolicyValue.ID.Length - 1));
        TextBox tbBrokeragePercentage = (TextBox)tbPolicyValue.Parent.FindControl("tbBrokeragePercentage" + index.ToString());
        TextBox tbBrokerageValue = (TextBox)tbPolicyValue.Parent.FindControl("tbBrokerageValue" + index.ToString());
        tbBrokerageValue.Text = (Convert.ToDecimal(tbPolicyValue.Text) * Convert.ToDecimal(tbBrokeragePercentage.Text) / 100).ToString();
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        if (PoliciesDetailsView.SelectedValue != null)
        {
            int policyItemID = Convert.ToInt32(PoliciesDetailsView.SelectedValue);
            PolicyItem policyItem = PolicyItem.Get(policyItemID);
            DropDownList ddlStatuses = PoliciesDetailsView.FindControl("ddlStatuses") as DropDownList;
            policyItem.StatusID = Convert.ToInt32(ddlStatuses.SelectedValue);
            Policy policy = Policy.Get(policyItem.PolicyID);
            TextBox tbStartDate = PoliciesDetailsView.FindControl("tbStartDate") as TextBox;
            TextBox tbEndDate = PoliciesDetailsView.FindControl("tbEndDate") as TextBox;
            TextBox tbApplicationDate = PoliciesDetailsView.FindControl("tbApplicationDate") as TextBox;
            policy.StartDate = Convert.ToDateTime(tbStartDate.Text);
            policy.EndDate = Convert.ToDateTime(tbEndDate.Text);
            policy.ApplicationDate = Convert.ToDateTime(tbApplicationDate.Text);
            Policy.Table.Context.SubmitChanges();
            Panel pnlExtendControls = PoliciesDetailsView.FindControl("pnlExtendControls") as Panel;
            List<Broker.DataAccess.Control> listControls = Broker.DataAccess.Control.GetActiveByInsuranceSubType(policyItem.InsuranceSubTypeID);
            foreach (Broker.DataAccess.Control c in listControls)
            {
                PolicyExtendInformation pei = PolicyExtendInformation.GetByPolicyItemAndControl(policyItem.ID, c.ID);
                TextBox tbControl = (TextBox)pnlExtendControls.FindControl(c.TextBoxID);
                ControlCollection cc = pnlExtendControls.Controls;
                if (pei != null)
                {
                    pei.Value = tbControl.Text;
                    if (c.HasCompareValidator)
                    {
                        CompareValidator cv = (CompareValidator)pnlExtendControls.FindControl("cv" + c.TextBoxID);
                        if (cv.Type == ValidationDataType.Double)
                        {
                            if (tbControl.Text == string.Empty)
                            {
                                pei.Value = "0";
                            }
                        }
                    }
                    PolicyExtendInformation.Table.Context.SubmitChanges();
                } else
                {
                    PolicyExtendInformation newpei = new PolicyExtendInformation();
                    newpei.ControlID = c.ID;
                    newpei.PolicyItemID = policyItem.ID;
                    newpei.Value = tbControl.Text;
                    if (c.HasCompareValidator)
                    {
                        CompareValidator cv = (CompareValidator)pnlExtendControls.FindControl("cv" + c.TextBoxID);
                        if (cv.Type == ValidationDataType.Double)
                        {
                            if (tbControl.Text == string.Empty)
                            {
                                pei.Value = "0";
                            }
                        }
                    }
                    pei.Insert();
                }
                btnSearch.Enabled = false;
            }
        }
    }
    protected void btnNewChange_Click(object sender, EventArgs e)
    {
        btnSearch.Enabled = true;
        dvDataSource.SelectParameters.Clear();
        dvDataSource.SelectParameters.Add("policyNumber", "");
        dvDataSource.SelectParameters.Add("insuranceSubTypeID", "0");
        dvDataSource.SelectParameters.Add("insuranceCompanyID", "0");
        PoliciesDetailsView.DataBind();
        //CreateChildControls();
        btnUpdate.Visible = false;
    }
}
