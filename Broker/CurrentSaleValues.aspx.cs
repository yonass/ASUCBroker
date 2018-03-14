using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Broker.DataAccess;
using Broker.Controllers.ReportControllers;

public partial class Broker_CurrentSaleValues : AuthenticationPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            tbCurrentDate.Text = DateTime.Today.ToShortDateString();
            ViewState["currentDate"] = tbCurrentDate.Text;
            Refresh();
        }
    }

    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        Refresh();
    }

    void Refresh()
    {
        gvCashPaidPolicies.DataBind();
        gvFacturePaidPolicies.DataBind();
        ViewState["currentDate"] = tbCurrentDate.Text;
        int userID = this.PageUser.ID;
        DateTime currentDate = Convert.ToDateTime(tbCurrentDate.Text);
        lblCashPaidValues.Text = String.Format("{0:#,0.00}", 0);
        lblCreditCardPaidValues.Text = String.Format("{0:#,0.00}", 0);
        lblFacturePaidValues.Text = String.Format("{0:#,0.00}", CurrentSaleController.GetSubExpectedFacturePaymentsPerDay(currentDate, userID));
        CreateChildControls();
    }


    protected override void CreateChildControls()
    {
        DateTime currentDate = DateTime.Now;
        if (ViewState["currentDate"] != null)
        {
            DateTime.TryParse(ViewState["currentDate"].ToString(), out currentDate);
        }
        int userID = this.PageUser.ID;
        List<CreditCard> listCreditCards = CreditCard.Table.OrderBy(c => c.ID).ToList();
        List<Broker.DataAccess.Bank> listBanks = Broker.DataAccess.Bank.GetActiveBanks();
        pnlCreditCardPayments.Controls.Clear();
        HtmlTable titleTable = new HtmlTable();
        //titleTable.Width = "695px";
        HtmlTableRow tableRow = new HtmlTableRow();
        titleTable.Rows.Add(tableRow);
        HtmlTableCell tableCell = new HtmlTableCell();
        tableRow.Cells.Add(tableCell);
        Label lbl = new Label();
        lbl.ID = "lblBanksTitle";
        lbl.Font.Bold = true;
        lbl.Text = "Банка";
        tableCell.Controls.Add(lbl);
        int i = 0;
        foreach (CreditCard cc in listCreditCards)
        {
            tableCell = new HtmlTableCell();
            tableRow.Cells.Add(tableCell);
            lbl = new Label();
            lbl.ID = "lblCreditCard" + i.ToString();
            lbl.Font.Bold = true;
            lbl.Text = cc.Name;
            tableCell.Controls.Add(lbl);
            i++;
        }
        tableCell = new HtmlTableCell();
        tableRow.Cells.Add(tableCell);
        lbl = new Label();
        lbl.ID = "lblTotalTitle";
        lbl.Font.Bold = true;
        lbl.Text = "Вкупно";
        tableCell.Controls.Add(lbl);
        
        
        int j = 0;
        foreach (Broker.DataAccess.Bank b in listBanks)
        {
            decimal totalBankSum = 0;
            tableRow = new HtmlTableRow();
            titleTable.Rows.Add(tableRow);
            tableCell = new HtmlTableCell();
            tableRow.Cells.Add(tableCell);
            lbl = new Label();
            lbl.ID = "lblBankName" + j.ToString();
            lbl.Font.Bold = true;
            lbl.Text = b.Name;
            tableCell.Controls.Add(lbl);
            int k = 0;
            foreach (CreditCard cc in listCreditCards)
            {
                tableCell = new HtmlTableCell();
                tableRow.Cells.Add(tableCell);
                tableRow.Cells.Add(tableCell);
                lbl = new Label();
                lbl.ID = "lblSum" + b.ID.ToString() + "x" + cc.ID.ToString();
                lbl.Font.Bold = false;
                decimal sum = 0; //CurrentSaleController.GetSumForBankAndCreditCardForDay(b.ID, cc.ID, currentDate, userID);
                totalBankSum += sum;
                lbl.Text = String.Format("{0:#,0.00}", sum);
                tableCell.Controls.Add(lbl);
                k++;
            }
            tableCell = new HtmlTableCell();
            tableRow.Cells.Add(tableCell);
            lbl = new Label();
            lbl.ID = "lblTotalValue" + j.ToString();
            lbl.Font.Bold = true;
            lbl.Text = String.Format("{0:#,0.00}", totalBankSum);
            tableCell.Controls.Add(lbl);
            j++;
        }
        pnlCreditCardPayments.Controls.Add(titleTable);

    }
    protected void gvCashPaidPolicies_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvCashPaidPolicies.PageIndex = e.NewPageIndex;
    }
    protected void gvCashPaidPolicies_PageIndexChanged(object sender, EventArgs e)
    {
        gvCashPaidPolicies.DataBind();
    }
    protected void gvFacturePaidPolicies_PageIndexChanged(object sender, EventArgs e)
    {
        gvFacturePaidPolicies.DataBind();
    }
    protected void gvFacturePaidPolicies_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvFacturePaidPolicies.PageIndex = e.NewPageIndex;
    }
}
