using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Broker.DataAccess;


//A customized class for displaying the Template Column

public class DynamicGridViewTemplate : ITemplate {

    //A variable to hold the type of ListItemType.

    ListItemType _templateType;



    //A variable to hold the column name.

    string _columnName;


    bool _isReadOnly;

    bool _isDropDownList;



    //Constructor where we define the template type and column name.

    public DynamicGridViewTemplate(ListItemType type, string colname, bool isReadOnly, bool isDropDownList) {

        //Stores the template type.

        _templateType = type;



        //Stores the column name.

        _columnName = colname;

        _isReadOnly = isReadOnly;

        _isDropDownList = isDropDownList;

    }



    void ITemplate.InstantiateIn(System.Web.UI.Control container) {


        switch (_templateType) {

            case ListItemType.Header:

                //Creates a new label control and add it to the container.

                    Label lbl = new Label();            //Allocates the new label object.
                                 //Assigns the name of the column in the lable.
                    if (_columnName == "Тип на картица") {
                        lbl.Visible = false;
                        lbl.Text = "    ";
                    } else {
                        lbl.Visible = true;
                        lbl.Text = _columnName;
                    }
                    container.Controls.Add(lbl);        //Adds the newly created label control to the container.
                    break;



            case ListItemType.Item:

                //Creates a new text box control and add it to the container.

                if (!_isDropDownList) {
                    TextBox tb1 = new TextBox();                            //Allocates the new text box object.
                    tb1.DataBinding += new EventHandler(tb1_DataBinding);   //Attaches the data binding event.
                    tb1.Columns = 8;                                        //Creates a column with size 8.
                    tb1.CssClass = "tekstPoleSmall";
                    tb1.ID = "tb" + _columnName;
                    if (_isReadOnly) {
                        tb1.ReadOnly = true;
                    }
                    container.Controls.Add(tb1);                            //Adds the newly created textbox to the container.

                } else {
                    if (_columnName == "BankCreditCardID") {
                        DropDownList ddl1 = new DropDownList();
                        ddl1.ID = "ddlBankCreditCards";

                        ObjectDataSource ods1 = new ObjectDataSource();
                        ods1.ID = "odsBankCreditCards";
                        //ods1.SelectMethod = "GetByCode";
                        ods1.SelectMethod = "GetAll";
                        //ods1.SelectParameters.Clear();
                        //ods1.SelectParameters.Add("code", PaymentType.FACTURE);
                        ods1.TypeName = "Broker.DataAccess.BankCreditCard";


                        ddl1.DataSourceID = "odsBankCreditCards";
                        ddl1.DataTextField = "Name";
                        ddl1.DataValueField = "ID";
                        ddl1.CssClass = "select";
                        ddl1.Width = 150;
                        ddl1.DataBinding += new EventHandler(ddl1_DataBinding);
                        ddl1.Visible = false;
                        container.Controls.Add(ods1);
                        container.Controls.Add(ddl1);
                    } else {
                        DropDownList ddl1 = new DropDownList();
                        ddl1.ID = "ddlPaymentTypes";

                        ObjectDataSource ods1 = new ObjectDataSource();
                        ods1.ID = "ods1";
                        //ods1.SelectMethod = "GetByCode";
                        ods1.SelectMethod = "Select";
                        //ods1.SelectParameters.Clear();
                        //ods1.SelectParameters.Add("code", PaymentType.FACTURE);
                        ods1.TypeName = "Broker.DataAccess.PaymentType";

                        ddl1.DataSourceID = "ods1";
                        ddl1.DataTextField = "Name";
                        ddl1.DataValueField = "ID";
                        ddl1.CssClass = "select";
                        ddl1.Width = 150;
                        //ddl1.SelectedIndex = 2;
                        // ddl1.Enabled = !_isReadOnly;
                        //ddl1.SelectedValue = "2";
                        ddl1.AutoPostBack = true;
                        ddl1.SelectedIndexChanged += new EventHandler(ddl1_SelectedIndexChanged);
                        ddl1.DataBinding += new EventHandler(ddl1_DataBinding);

                        container.Controls.Add(ods1);
                        container.Controls.Add(ddl1);
                    }

                }

                break;



            case ListItemType.EditItem:

                //As, I am not using any EditItem, I didnot added any code here.

                break;



            case ListItemType.Footer:

                CheckBox chkColumn = new CheckBox();

                chkColumn.ID = "Chk" + _columnName;

                container.Controls.Add(chkColumn);

                break;

        }

    }

    void ddl1_SelectedIndexChanged(object sender, EventArgs e) {
        DropDownList dd1 = (DropDownList)sender;
        GridViewRow container = (GridViewRow)dd1.NamingContainer;
        GridView gv = (GridView)container.NamingContainer;
        if (dd1.SelectedValue == Broker.DataAccess.PaymentType.GetByCode(Broker.DataAccess.PaymentType.CREDITCARD).ID.ToString())
        {
            DropDownList ddlBankCreditCards = (DropDownList)container.FindControl("ddlBankCreditCards");
            ddlBankCreditCards.Visible = true;
            gv.HeaderRow.Cells[gv.HeaderRow.Cells.Count - 1].Text = "Тип на картица";
        } else {
            gv.HeaderRow.Cells[gv.HeaderRow.Cells.Count - 1].Text = "    ";
        }
    }

    /// <summary>

    /// This is the event, which will be raised when the binding happens.

    /// </summary>

    /// <param name="sender"></param>

    /// <param name="e"></param>

    void tb1_DataBinding(object sender, EventArgs e) {

        TextBox txtdata = (TextBox)sender;

        GridViewRow container = (GridViewRow)txtdata.NamingContainer;

        object dataValue = DataBinder.Eval(container.DataItem, _columnName);

        if (dataValue != DBNull.Value) {

            if (_columnName == "Date") {
                txtdata.Text = String.Format("{0:d}", dataValue);
            } else {
                txtdata.Text = dataValue.ToString();
            }

        }

    }


    void ddl1_DataBinding(object sender, EventArgs e) {
        DropDownList ddldata = (DropDownList)sender;
        GridViewRow container = (GridViewRow)ddldata.NamingContainer;
        //object dataValue = DataBinder.Eval(container.DataItem, _columnName);

        //if (dataValue != DBNull.Value) {

        //ddldata.SelectedValue = dataValue.ToString();

        //}

    }

}
