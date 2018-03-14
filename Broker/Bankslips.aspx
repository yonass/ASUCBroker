<%@ Page Title="Изводи" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    Culture="mk-MK" UICulture="mk-MK" CodeFile="Bankslips.aspx.cs" Inherits="Broker_Bankslips" %>

<%@ Register Namespace="Superexpert.Controls" TagPrefix="super" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="kontroli">
        <div id="button01">
            <asp:Button ID="btnNew" runat="server" ToolTip="Нов запис" OnClick="btnNew_Click"
                CausesValidation="false" UseSubmitBehavior="false" CssClass="novZapis" BorderWidth="0px" />
        </div>
        <div id="button02">
            <asp:Button ID="btnEdit" runat="server" ToolTip="Измени" OnClick="btnEdit_Click"
                CausesValidation="false" Enabled="false" UseSubmitBehavior="false" CssClass="izmeni"
                BorderWidth="0px" />
        </div>
        <div id="button03">
            <asp:Button ID="btnDelete" runat="server" OnClick="btnDelete_Click" CausesValidation="false"
                Enabled="false" UseSubmitBehavior="false" CssClass="izbrisi" BorderWidth="0px" />
        </div>
        <div id="button04">
            <asp:Button ID="btnPreview" runat="server" ToolTip="Освежи" OnClick="btnPreview_Click"
                CausesValidation="false" UseSubmitBehavior="false" CssClass="osvezi" BorderWidth="0px" />
        </div>
        <div id="button05">
            <asp:Button ID="btnReport" runat="server" ToolTip="Извештај" CausesValidation="false"
                UseSubmitBehavior="false" OnClick="btnReport_Click" CssClass="izvestaj" BorderWidth="0px" />
        </div>
        <div id="button06">
            <asp:Button ID="btnSearch" runat="server" ToolTip="Пребарај" OnClick="btnSearch_Click"
                CausesValidation="false" UseSubmitBehavior="false" CssClass="prebaraj" BorderWidth="0px" />
        </div>
        <div id="button07">
            <asp:Button ID="btnBankslipItems" runat="server" ToolTip="Ставки" OnClick="btnBankslipItems_Click"
                CausesValidation="false" UseSubmitBehavior="false" CssClass="stavki" BorderWidth="0px" />
        </div>
        <div id="button08">
            <asp:Button ID="btnImportBankslip" runat="server" ToolTip="Импортирај" OnClick="btnImportBankslip_Click"
                CausesValidation="false" UseSubmitBehavior="false" CssClass="dokumenti" BorderWidth="0px" />
        </div>
        <div id="button09">
            <asp:Button ID="btnPayments" runat="server" ToolTip="Плаќања" OnClick="btnPayments_Click"
                CausesValidation="false" UseSubmitBehavior="false" BorderWidth="0px" Enabled="false"
                CssClass="plakanja" />
        </div>
    </div>
    <asp:MultiView ID="mvMain" runat="server">
        <asp:View ID="viewGrid" runat="server">
            <div id="tabeliFrame">
                <div id="header">
                    <div id="content">
                        Изводи
                    </div>
                </div>
                <div id="contentOuter">
                    <div id="contentInner">
                        <cc1:GXGridView ID="GXGridView1" runat="server" DataSourceID="odsGridView" DataKeyNames="ID"
                            Width="100%" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"
                            Caption="Изводи" EmptyDataText="Нема записи кои го задоволуваат критериумот на пребарување!"
                            OnRowCommand="GXGridView1_RowCommand" RowStyle-CssClass="row" CssClass="grid"
                            GridLines="None">
                            <Columns>
                                <asp:BoundField HeaderText="Број на извод" DataField="BankslipNumber" SortExpression="BankslipNumber" />
                                <asp:BoundField HeaderText="Банка" DataField="BankName" SortExpression="BankName" />
                                <asp:BoundField HeaderText="Дата" DataField="Date" SortExpression="Date" DataFormatString="{0:d}" />
                                <asp:BoundField HeaderText="Должи" DataField="DebtValue" SortExpression="DebtValue"
                                    DataFormatString="{0:#,0.00}" />
                                <asp:BoundField HeaderText="Побарува" DataField="DemandValue" SortExpression="DemandValue"
                                    DataFormatString="{0:#,0.00}" />
                            </Columns>
                            <PagerStyle HorizontalAlign="Center" />
                            <SelectedRowStyle CssClass="rowSelected" />
                            <PagerSettings FirstPageText="<< Прва " PreviousPageText="< Претходна " NextPageText=" Следна >"
                                LastPageText=" Последна >>" Mode="NextPreviousFirstLast" />
                        </cc1:GXGridView>
                        <cc1:GridViewDataSource ID="odsGridView" runat="server" TypeName="Broker.DataAccess.ViewBankslip"
                            OldValuesParameterFormatString="oldEntity">
                        </cc1:GridViewDataSource>
                        <div class="pager">
                            <div class="container">
                                <div style="float: left;">
                                    <cc1:PagerControl ID="myGridPager" runat="server" ControlToPage="GXGridView1" />
                                </div>
                                <div style="float: right;">
                                    <cc1:FilterControl ID="FilterControl1" runat="server" GridViewToFilter="GXGridView1"
                                        FilterDataSourceID="odsFilterGridView" OnFilter="FilterControl1_Filter">
                                        <cc1:FilterItem FieldName="Број на извод" PropertyName="BankslipNumber" Comparator="StringComparators"
                                            DataType="String" />
                                        <cc1:FilterItem FieldName="Банка" PropertyName="BankName" Comparator="StringComparators"
                                            DataType="String" />
                                        <cc1:FilterItem FieldName="Дата" PropertyName="Date" Comparator="NumericComparators"
                                            DataType="DateTime" />
                                        <cc1:FilterItem FieldName="Должи" PropertyName="DebtValue" Comparator="NumericComparators"
                                            DataType="Decimal" />
                                        <cc1:FilterItem FieldName="Побарува" PropertyName="DemandValue" Comparator="NumericComparators"
                                            DataType="Decimal" />
                                    </cc1:FilterControl>
                                </div>
                                <cc1:FilterDataSource ID="odsFilterGridView" runat="server" TypeName="Broker.DataAccess.ViewBankslip">
                                    <SelectParameters>
                                        <asp:ControlParameter Name="fArgument" ControlID="FilterControl1" PropertyName="FCFilterArgument" />
                                    </SelectParameters>
                                </cc1:FilterDataSource>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </asp:View>
        <asp:View ID="viewEdit" runat="server">
            <div class="paddingKontroli">
                <asp:DetailsView ID="DetailsView1" runat="server" DataSourceID="dvDataSource" DataKeyNames="ID"
                    AutoGenerateRows="False" OnItemCommand="DetailsView1_ItemCommand" OnItemDeleted="DetailsView1_ItemDeleted"
                    OnItemInserted="DetailsView1_ItemInserted" OnItemUpdated="DetailsView1_ItemUpdated"
                    OnModeChanging="DetailsView1_ModeChanging" OnItemInserting="DetailsView1_ItemInserting"
                    GridLines="None">
                    <Fields>
                        <asp:TemplateField>
                            <InsertItemTemplate>
                                <div class="subTitles">
                                    Нов запис во Изводи
                                </div>
                            </InsertItemTemplate>
                            <EditItemTemplate>
                                <div class="subTitles">
                                    Измена на запис во Изводи
                                </div>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <div class="subTitles">
                                    Бришење на запис од Изводи
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Број на извод">
                            <InsertItemTemplate>
                                <asp:TextBox ID="tbBankslipNumber" runat="server" Text='<%# Bind("BankslipNumber") %>'
                                    MaxLength="30"></asp:TextBox>
                                <super:EntityCallOutValidator ID="BankslipNumberInsertValidator" PropertyName="BANKSLIPNUMBER_INSERT_EXIST"
                                    runat="server" />
                                <asp:RequiredFieldValidator ID="rfvBankslipNumber" runat="server" ControlToValidate="tbBankslipNumber"
                                    Display="Dynamic" ErrorMessage="*">
                                </asp:RequiredFieldValidator>
                            </InsertItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="tbBankslipNumber" runat="server" Text='<%# Bind("BankslipNumber") %>'
                                    MaxLength="30"></asp:TextBox>
                                <super:EntityCallOutValidator ID="BankslipNumberUpdateValidator" PropertyName="BANKSLIPNUMBER_UPDATE_EXIST"
                                    runat="server" />
                                <asp:RequiredFieldValidator ID="rfvBankslipNumber" runat="server" ControlToValidate="tbBankslipNumber"
                                    Display="Dynamic" ErrorMessage="*">
                                </asp:RequiredFieldValidator>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:TextBox ID="tbBankslipNumber" runat="server" Text='<%# Bind("BankslipNumber") %>'
                                    ReadOnly="true"></asp:TextBox>
                                <super:EntityCallOutValidator ID="BankslipDeleteValidator" PropertyName="BANKSLIP_DELETE"
                                    runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Банка">
                            <InsertItemTemplate>
                                <asp:DropDownList ID="ddlBanks" runat="server" DataSourceID="odsBanks" DataTextField="Name"
                                    DataValueField="ID" SelectedValue='<%# Bind("BankID") %>'>
                                </asp:DropDownList>
                                <asp:ObjectDataSource ID="odsBanks" runat="server" SelectMethod="GetActiveBanks"
                                    TypeName="Broker.DataAccess.Bank"></asp:ObjectDataSource>
                            </InsertItemTemplate>
                            <ItemTemplate>
                                <asp:DropDownList ID="ddlBanks" runat="server" DataSourceID="odsBanks" DataTextField="Name"
                                    Enabled="false" DataValueField="ID" SelectedValue='<%# Bind("BankID") %>'>
                                </asp:DropDownList>
                                <asp:ObjectDataSource ID="odsBanks" runat="server" SelectMethod="GetActiveBanks"
                                    TypeName="Broker.DataAccess.Bank"></asp:ObjectDataSource>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddlBanks" runat="server" DataSourceID="odsBanks" DataTextField="Name"
                                    DataValueField="ID" SelectedValue='<%# Bind("BankID") %>'>
                                </asp:DropDownList>
                                <asp:ObjectDataSource ID="odsBanks" runat="server" SelectMethod="GetActiveBanks"
                                    TypeName="Broker.DataAccess.Bank"></asp:ObjectDataSource>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Дата">
                            <ItemTemplate>
                                <asp:TextBox ID="tbDate" runat="server" Text='<%# Bind("Date", "{0:d}") %>' ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="tbDate" runat="server" Text='<%# Bind("Date", "{0:d}") %>'></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvDate" runat="server" ControlToValidate="tbDate"
                                    ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="cvDate" runat="server" ControlToValidate="tbDate" ErrorMessage="(неправилен формат)"
                                    Display="Dynamic" Operator="DataTypeCheck" Type="Date"></asp:CompareValidator>
                            </EditItemTemplate>
                            <InsertItemTemplate>
                                <asp:TextBox ID="tbDate" runat="server" Text='<%# Bind("Date", "{0:d}") %>'></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvDate" runat="server" ControlToValidate="tbDate"
                                    ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="cvDate" runat="server" ControlToValidate="tbDate" ErrorMessage="(неправилен формат)"
                                    Display="Dynamic" Operator="DataTypeCheck" Type="Date"></asp:CompareValidator>
                            </InsertItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Должи" InsertVisible="false">
                            <ItemTemplate>
                                <asp:TextBox ID="tbDebtValue" runat="server" Text='<%# Bind("DebtValue") %>' ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="tbDebtValue" runat="server" Text='<%# Bind("DebtValue") %>'></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvDebtValue" runat="server" ControlToValidate="tbDebtValue"
                                    ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="cvDebtValue" runat="server" ControlToValidate="tbDebtValue"
                                    ErrorMessage="(неправилен формат)" Display="Dynamic" Operator="DataTypeCheck"
                                    Type="Double"></asp:CompareValidator>
                            </EditItemTemplate>
                            <InsertItemTemplate>
                                <%--<asp:TextBox ID="tbDebtValue" runat="server" Text='<%# Bind("DebtValue") %>'></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvDebtValue" runat="server" ControlToValidate="tbDebtValue"
                                    ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="cvDebtValue" runat="server" ControlToValidate="tbDebtValue"
                                    ErrorMessage="(неправилен формат)" Display="Dynamic" Operator="DataTypeCheck"
                                    Type="Double"></asp:CompareValidator>--%>
                            </InsertItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Побарува" InsertVisible="false">
                            <ItemTemplate>
                                <asp:TextBox ID="tbDemandValue" runat="server" Text='<%# Bind("DemandValue") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="tbDemandValue" runat="server" Text='<%# Bind("DemandValue") %>'></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvDemandValue" runat="server" ControlToValidate="tbDemandValue"
                                    ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="cvDemandValue" runat="server" ControlToValidate="tbDemandValue"
                                    ErrorMessage="(неправилен формат)" Display="Dynamic" Operator="DataTypeCheck"
                                    Type="Double"></asp:CompareValidator>
                            </EditItemTemplate>
                            <InsertItemTemplate>
                                <%--<asp:TextBox ID="tbDemandValue" runat="server" Text='<%# Bind("DemandValue") %>'></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvDemandValue" runat="server" ControlToValidate="tbDemandValue"
                                    ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="cvDemandValue" runat="server" ControlToValidate="tbDemandValue"
                                    ErrorMessage="(неправилен формат)" Display="Dynamic" Operator="DataTypeCheck"
                                    Type="Double"></asp:CompareValidator>--%>
                            </InsertItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Button ID="btnCancel" runat="server" CommandName="Cancel" Text="Откажи" CausesValidation="false" />
                                <asp:Button ID="btnDelete" runat="server" CommandName="Delete" Text="Избриши" OnClientClick="return confirm('Дали сте сигурни?')" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Button ID="btnCancel" runat="server" CommandName="Cancel" Text="Откажи" CausesValidation="false" />
                                <asp:Button ID="btnUpdate" runat="server" CommandName="Update" Text="Измени" />
                            </EditItemTemplate>
                            <InsertItemTemplate>
                                <asp:Button ID="btnCancel" runat="server" CommandName="Cancel" Text="Откажи" CausesValidation="false" />
                                <asp:Button ID="btnInsert" runat="server" CommandName="Insert" Text="Внеси" />
                            </InsertItemTemplate>
                        </asp:TemplateField>
                    </Fields>
                </asp:DetailsView>
                <cc1:DetailsViewDataSource ID="dvDataSource" runat="server" TypeName="Broker.DataAccess.Bankslip"
                    ConflictDetection="CompareAllValues" DataObjectTypeName="Broker.DataAccess.Bankslip"
                    DeleteMethod="Delete" InsertMethod="Insert" SelectMethod="Get" UpdateMethod="Update"
                    OnUpdating="dvDataSource_Updating" OnUpdated="dvDataSource_Updated" OnInserted="dvDataSource_Inserted"
                    OnInserting="dvDataSource_Inserting" OldValuesParameterFormatString="oldEntity"
                    OnSelecting="dvDataSource_Selecting" OnDeleting="dvDataSource_Deleting">
                    <UpdateParameters>
                        <asp:Parameter Name="oldEntity" Type="Object" />
                        <asp:Parameter Name="newEntity" Type="Object" />
                        <asp:Parameter Name="Date" Type="DateTime" />
                        <asp:Parameter Name="DebtValue" Type="Decimal" />
                        <asp:Parameter Name="DemandValue" Type="Decimal" />
                    </UpdateParameters>
                    <InsertParameters>
                        <asp:Parameter Name="Date" Type="DateTime" />
                        <asp:Parameter Name="DebtValue" Type="Decimal" />
                        <asp:Parameter Name="DemandValue" Type="Decimal" />
                    </InsertParameters>
                    <DeleteParameters>
                        <asp:Parameter Name="entityToDelete" Type="Object" />
                        <asp:Parameter Name="Date" Type="DateTime" />
                        <asp:Parameter Name="DebtValue" Type="Decimal" />
                        <asp:Parameter Name="DemandValue" Type="Decimal" />
                    </DeleteParameters>
                    <SelectParameters>
                        <asp:ControlParameter ControlID="GXGridView1" Name="id" PropertyName="SelectedValue"
                            Type="Int32" />
                    </SelectParameters>
                </cc1:DetailsViewDataSource>
            </div>
        </asp:View>
        <asp:View ID="viewSearch" runat="server">
            <div class="paddingKontroli">
                <cc1:SearchControl ID="SearchControl1" runat="server" SearchDataSourceID="odsSearch"
                    GridViewToSearch="GXGridView1" OnSearch="SearchControl1_Search">
                    <cc1:SearchItem FieldName="Број на извод" PropertyName="BankslipNumber" Comparator="StringComparators"
                        DataType="String" />
                    <cc1:SearchItem FieldName="Банка" PropertyName="BankName" Comparator="StringComparators"
                        DataType="String" />
                    <cc1:SearchItem FieldName="Дата" PropertyName="Date" Comparator="NumericComparators"
                        DataType="DateTime" />
                    <cc1:SearchItem FieldName="Должи" PropertyName="DebtValue" Comparator="NumericComparators"
                        DataType="Decimal" />
                    <cc1:SearchItem FieldName="Побарува" PropertyName="DemandValue" Comparator="NumericComparators"
                        DataType="Decimal" />
                </cc1:SearchControl>
                <cc1:GridViewDataSource ID="odsSearch" runat="server" TypeName="Broker.DataAccess.ViewBankslip"
                    SelectCountMethod="SelectSearchCountCached" SelectMethod="SelectSearch">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="SearchControl1" PropertyName="SearchArguments" Name="sArgument" />
                    </SelectParameters>
                </cc1:GridViewDataSource>
            </div>
        </asp:View>
        <asp:View ID="viewReport" runat="server">
            <div class="paddingKontroli">
                <cc1:ReportControl ID="reportControl" runat="server" TypeName="Broker.DataAccess.ViewBankslip"
                    GridViewID="GXGridView1" SearchControlID="SearchControl1">
                    <cc1:PrintItem HeaderText="Број на извод" PropertyName="BankslipNumber" />
                    <cc1:PrintItem HeaderText="Банка" PropertyName="BankName" />
                    <cc1:PrintItem HeaderText="Дата" PropertyName="Date" />
                    <cc1:PrintItem HeaderText="Должи" PropertyName="DebtValue" />
                    <cc1:PrintItem HeaderText="Побарува" PropertyName="DemandValue" />
                </cc1:ReportControl>
            </div>
        </asp:View>
        <asp:View ID="viewBankslipItems" runat="server">
            <div class="paddingKontroli">
                <asp:DetailsView ID="dvBankslipForItem" runat="server" DataSourceID="dvDataSource"
                    DataKeyNames="ID" AutoGenerateRows="False" GridLines="None">
                    <Fields>
                        <asp:TemplateField HeaderText="Број на извод">
                            <ItemTemplate>
                                <asp:TextBox ID="tbBankslipNumber" runat="server" Text='<%# Eval("BankslipNumber") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Банка">
                            <ItemTemplate>
                                <asp:TextBox ID="tbBankName" runat="server" Text='<%# Eval("Bank.Name") %>' ReadOnly="true"
                                    Width="300px"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Дата">
                            <ItemTemplate>
                                <asp:TextBox ID="tbDate" runat="server" Text='<%# Eval("Date", "{0:d}") %>' ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Должи">
                            <ItemTemplate>
                                <asp:TextBox ID="tbDebtValue" runat="server" Text='<%# Eval("DebtValue", "{0:#,0.00}") %>'
                                    CssClass="currencyClass" ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Побарува">
                            <ItemTemplate>
                                <asp:TextBox ID="tbDemandValue" runat="server" Text='<%# Eval("DemandValue", "{0:#,0.00}") %>'
                                    CssClass="currencyClass" ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Fields>
                </asp:DetailsView>
                <asp:Button ID="btnNewBankslipItem" runat="server" Text="Нова ставка" OnClick="btnNewBankslipItem_Click" />
                <asp:MultiView ID="mvBankslipItems" runat="server">
                    <asp:View ID="viewBankslipGrid" runat="server">
                        <cc1:GXGridView ID="GXGridViewBankslipItems" runat="server" DataSourceID="odsGridViewBankslipItems"
                            DataKeyNames="ID" Width="100%" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"
                            Caption="Ставки на извод" EmptyDataText="Нема записи кои го задоволуваат критериумот на пребарување!"
                            OnRowCommand="GXGridViewBankslipItems_RowCommand" RowStyle-CssClass="row" CssClass="grid"
                            GridLines="None" OnRowDeleting="GXGridViewBankslipItems_RowDeleting" OnRowEditing="GXGridViewBankslipItems_RowEditing"
                            OnRowUpdated="GXGridViewBankslipItems_RowUpdated">
                            <RowStyle CssClass="row" />
                            <Columns>
                                <asp:BoundField DataField="ClientName" HeaderText="Примач-налогодавач" SortExpression="ClientName" />
                                <asp:BoundField DataField="ClientAccountNumber" HeaderText="Сметка" SortExpression="ClientAccountNumber" />
                                <asp:BoundField DataField="DebtValue" HeaderText="Должи" DataFormatString="{0:#,0.00}"
                                    SortExpression="DebtValue" />
                                <asp:BoundField DataField="DemandValue" HeaderText="Побарува" SortExpression="DemandValue"
                                    DataFormatString="{0:#,0.00}" />
                                <asp:BoundField DataField="PaymentDescription" HeaderText="Опис" SortExpression="PaymentDescription"
                                    DataFormatString="{0:#,0.00}" />
                                <asp:ButtonField CommandName="SingleClick" Text="SingleClick" Visible="False" />
                                <asp:ButtonField CommandName="DoubleClick" Text="DoubleClick" Visible="False" />
                                <asp:CommandField CancelText="Откажи" DeleteText="Избриши" EditText="Измени" InsertText="Внеси"
                                    InsertVisible="false" NewText="Нов" SelectText="Одбери" ShowEditButton="True"
                                    ShowDeleteButton="true" ShowInsertButton="false" UpdateText="Ажурирај" />
                            </Columns>
                            <PagerStyle HorizontalAlign="Center" />
                            <SelectedRowStyle CssClass="rowSelected" />
                            <PagerSettings FirstPageText="<< Прва " PreviousPageText="< Претходна " NextPageText=" Следна >"
                                LastPageText=" Последна >>" Mode="NextPreviousFirstLast" />
                        </cc1:GXGridView>
                        <cc1:GridViewDataSource ID="odsGridViewBankslipItems" runat="server" TypeName="Broker.DataAccess.ViewBankslipItem"
                            OldValuesParameterFormatString="oldEntity" ConflictDetection="CompareAllValues"
                            DataObjectTypeName="Broker.DataAccess.ViewBankslipItem" DeleteMethod="Delete"
                            EnablePaging="True" InsertMethod="Insert" SelectCountMethod="SelectByFKCountCached"
                            SelectMethod="SelectByFK" SortParameterName="orderBy" OnSelecting="odsGridViewBankslipItems_Selecting">
                        </cc1:GridViewDataSource>
                    </asp:View>
                    <asp:View ID="viewBankslipEdit" runat="server">
                        <asp:DetailsView ID="DetailsViewBankslipItem" runat="server" DataSourceID="dvDataSourceBankslipItem"
                            DataKeyNames="ID" AutoGenerateRows="False" OnItemCommand="DetailsViewBankslipItem_ItemCommand"
                            OnItemDeleted="DetailsViewBankslipItem_ItemDeleted" OnItemInserted="DetailsViewBankslipItem_ItemInserted"
                            OnItemUpdated="DetailsViewBankslipItem_ItemUpdated" OnModeChanging="DetailsViewBankslipItem_ModeChanging"
                            OnItemInserting="DetailsViewBankslipItem_ItemInserting" GridLines="None" OnItemUpdating="DetailsViewBankslipItem_ItemUpdating"
                            OnItemDeleting="DetailsViewBankslipItem_ItemDeleting">
                            <Fields>
                                <asp:TemplateField>
                                    <InsertItemTemplate>
                                        <div class="subTitles">
                                            Нов запис во ставки на извод
                                        </div>
                                    </InsertItemTemplate>
                                    <EditItemTemplate>
                                        <div class="subTitles">
                                            Измена на запис во ставки на извод
                                        </div>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <div class="subTitles">
                                            Бришење на запис од ставки на извод
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Број на извод" InsertVisible="false">
                                    <%--<InsertItemTemplate>
                                        <asp:TextBox ID="tbBankslipNumber" runat="server" Text='<%# Eval("Bankslip.BankslipNumber") %>'
                                            ReadOnly="true"></asp:TextBox>
                                    </InsertItemTemplate>--%>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="tbBankslipNumber" runat="server" Text='<%# Eval("Bankslip.BankslipNumber") %>'
                                            ReadOnly="true"></asp:TextBox>
                                        <asp:TextBox ID="tbBankslipNumberID" runat="server" Text='<%# Bind("BankslipID") %>'
                                            Visible="false"></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="tbBankslipNumber" runat="server" Text='<%# Eval("Bankslip.BankslipNumber") %>'
                                            ReadOnly="true"></asp:TextBox>
                                        <asp:TextBox ID="tbBankslipNumberID" runat="server" Text='<%# Bind("BankslipID") %>'
                                            Visible="false"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Примач-налогодавач">
                                    <InsertItemTemplate>
                                        <asp:TextBox ID="tbClientName" runat="server" Text='<%#Bind("ClientName") %>'></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvClientName" runat="server" ErrorMessage="*" Display="Dynamic"
                                            ControlToValidate="tbClientName"></asp:RequiredFieldValidator>
                                    </InsertItemTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="tbClientName" runat="server" Text='<%#Bind("ClientName") %>' ReadOnly="true"></asp:TextBox>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="tbClientName" runat="server" Text='<%#Bind("ClientName") %>'></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvClientName" runat="server" ErrorMessage="*" Display="Dynamic"
                                            ControlToValidate="tbClientName"></asp:RequiredFieldValidator>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Сметка">
                                    <InsertItemTemplate>
                                        <asp:TextBox ID="tbClientAccountNumber" runat="server" Text='<%#Bind("ClientAccountNumber") %>'></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvClientAccountNumber" runat="server" ErrorMessage="*"
                                            Display="Dynamic" ControlToValidate="tbClientAccountNumber"></asp:RequiredFieldValidator>
                                    </InsertItemTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="tbClientAccountNumber" runat="server" Text='<%#Bind("ClientAccountNumber") %>'
                                            ReadOnly="true"></asp:TextBox>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="tbClientAccountNumber" runat="server" Text='<%#Bind("ClientAccountNumber") %>'></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvClientAccountNumber" runat="server" ErrorMessage="*"
                                            Display="Dynamic" ControlToValidate="tbClientAccountNumber"></asp:RequiredFieldValidator>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Должи">
                                    <ItemTemplate>
                                        <asp:TextBox ID="tbDebtValue" runat="server" Text='<%# Bind("DebtValue","{0:#,0.00}") %>'
                                            ReadOnly="true"></asp:TextBox>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="tbDebtValue" runat="server" Text='<%# Bind("DebtValue") %>'></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvDebtValue" runat="server" ControlToValidate="tbDebtValue"
                                            ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:CompareValidator ID="cvDebtValue" runat="server" ControlToValidate="tbDebtValue"
                                            ErrorMessage="(неправилен формат)" Display="Dynamic" Operator="DataTypeCheck"
                                            Type="Double"></asp:CompareValidator>
                                    </EditItemTemplate>
                                    <InsertItemTemplate>
                                        <asp:TextBox ID="tbDebtValue" runat="server" Text='<%# Bind("DebtValue") %>'></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvDebtValue" runat="server" ControlToValidate="tbDebtValue"
                                            ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:CompareValidator ID="cvDebtValue" runat="server" ControlToValidate="tbDebtValue"
                                            ErrorMessage="(неправилен формат)" Display="Dynamic" Operator="DataTypeCheck"
                                            Type="Double"></asp:CompareValidator>
                                    </InsertItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Побарува">
                                    <ItemTemplate>
                                        <asp:TextBox ID="tbDemandValue" runat="server" Text='<%# Bind("DemandValue","{0:#,0.00}") %>'
                                            ReadOnly="true"></asp:TextBox>
                                        <super:EntityCallOutValidator ID="BANKSLIPITEMDELETEVALUESValidator" PropertyName="BANKSLIPITEM_DELETE_VALUES"
                                            runat="server" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="tbDemandValue" runat="server" Text='<%# Bind("DemandValue") %>'></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvDemandValue" runat="server" ControlToValidate="tbDemandValue"
                                            ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:CompareValidator ID="cvDemandValue" runat="server" ControlToValidate="tbDemandValue"
                                            ErrorMessage="(неправилен формат)" Display="Dynamic" Operator="DataTypeCheck"
                                            Type="Double"></asp:CompareValidator>
                                        <super:EntityCallOutValidator ID="BANKSLIPITEMUPDATEVALUESValidator" PropertyName="BANKSLIPITEM_UPDATE_VALUES"
                                            runat="server" />
                                    </EditItemTemplate>
                                    <InsertItemTemplate>
                                        <asp:TextBox ID="tbDemandValue" runat="server" Text='<%# Bind("DemandValue") %>'></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvDemandValue" runat="server" ControlToValidate="tbDemandValue"
                                            ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:CompareValidator ID="cvDemandValue" runat="server" ControlToValidate="tbDemandValue"
                                            ErrorMessage="(неправилен формат)" Display="Dynamic" Operator="DataTypeCheck"
                                            Type="Double"></asp:CompareValidator>
                                        <super:EntityCallOutValidator ID="BANKSLIPITEMINSERTVALUESValidator" PropertyName="BANKSLIPITEM_INSERT_VALUES"
                                            runat="server" />
                                    </InsertItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Провизија">
                                    <ItemTemplate>
                                        <asp:TextBox ID="tbProvisionValue" runat="server" Text='<%# Bind("ProvisionValue","{0:#,0.00}") %>'
                                            ReadOnly="true"></asp:TextBox>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="tbProvisionValue" runat="server" Text='<%# Bind("ProvisionValue") %>'></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvProvisionValue" runat="server" ControlToValidate="tbProvisionValue"
                                            ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:CompareValidator ID="cvProvisionValue" runat="server" ControlToValidate="tbProvisionValue"
                                            ErrorMessage="(неправилен формат)" Display="Dynamic" Operator="DataTypeCheck"
                                            Type="Double"></asp:CompareValidator>
                                    </EditItemTemplate>
                                    <InsertItemTemplate>
                                        <asp:TextBox ID="tbProvisionValue" runat="server" Text='<%# Bind("ProvisionValue") %>'></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvProvisionValue" runat="server" ControlToValidate="tbProvisionValue"
                                            ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:CompareValidator ID="cvProvisionValue" runat="server" ControlToValidate="tbProvisionValue"
                                            ErrorMessage="(неправилен формат)" Display="Dynamic" Operator="DataTypeCheck"
                                            Type="Double"></asp:CompareValidator>
                                    </InsertItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Шифра">
                                    <InsertItemTemplate>
                                        <asp:TextBox ID="tbCode" runat="server" Text='<%#Bind("Code") %>'></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvCode" runat="server" ErrorMessage="*" Display="Dynamic"
                                            ControlToValidate="tbCode"></asp:RequiredFieldValidator>
                                    </InsertItemTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="tbCode" runat="server" Text='<%#Bind("Code") %>' ReadOnly="true"></asp:TextBox>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="tbCode" runat="server" Text='<%#Bind("Code") %>'></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvCode" runat="server" ErrorMessage="*" Display="Dynamic"
                                            ControlToValidate="tbCode"></asp:RequiredFieldValidator>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Цел на дознака">
                                    <InsertItemTemplate>
                                        <asp:TextBox ID="tbPaymentDescription" runat="server" Text='<%#Bind("PaymentDescription") %>'></asp:TextBox>
                                    </InsertItemTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="tbPaymentDescription" runat="server" Text='<%#Bind("PaymentDescription") %>'
                                            ReadOnly="true"></asp:TextBox>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="tbPaymentDescription" runat="server" Text='<%#Bind("PaymentDescription") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Повикување на број">
                                    <InsertItemTemplate>
                                        <asp:TextBox ID="tbCallOnPaymentNumber" runat="server" Text='<%#Bind("CallOnPaymentNumber") %>'></asp:TextBox>
                                    </InsertItemTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="tbCallOnPaymentNumber" runat="server" Text='<%#Bind("CallOnPaymentNumber") %>'
                                            ReadOnly="true"></asp:TextBox>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="tbCallOnPaymentNumber" runat="server" Text='<%#Bind("CallOnPaymentNumber") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Евидентирано плаќање" InsertVisible="false">
                                    <%--<InsertItemTemplate>
                                        <asp:CheckBox ID="cbIsPaid" runat="server" Checked='<%#Bind("IsPaid") %>' />
                                    </InsertItemTemplate>--%>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="cbIsPaid" runat="server" Checked='<%#Bind("IsPaid") %>' Enabled="false" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:CheckBox ID="cbIsPaid" runat="server" Checked='<%#Bind("IsPaid") %>' Enabled="false" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Button ID="btnCancel" runat="server" CommandName="Cancel" Text="Откажи" CausesValidation="false" />
                                        <asp:Button ID="btnDelete" runat="server" Text="Избриши" CommandName="Delete" OnClientClick="return confirm('Дали сте сигурни?')" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Button ID="btnCancel" runat="server" CommandName="Cancel" Text="Откажи" CausesValidation="false" />
                                        <asp:Button ID="btnUpdate" runat="server" CommandName="Update" Text="Измени" />
                                    </EditItemTemplate>
                                    <InsertItemTemplate>
                                        <asp:Button ID="btnCancel" runat="server" CommandName="Cancel" Text="Откажи" CausesValidation="false" />
                                        <asp:Button ID="btnInsert" runat="server" CommandName="Insert" Text="Внеси" />
                                    </InsertItemTemplate>
                                </asp:TemplateField>
                            </Fields>
                        </asp:DetailsView>
                        <cc1:DetailsViewDataSource ID="dvDataSourceBankslipItem" runat="server" TypeName="Broker.DataAccess.BankslipItem"
                            ConflictDetection="CompareAllValues" DataObjectTypeName="Broker.DataAccess.BankslipItem"
                            DeleteMethod="Delete" InsertMethod="Insert" SelectMethod="Get" UpdateMethod="Update"
                            OnUpdating="dvDataSourceBankslipItem_Updating" OnUpdated="dvDataSourceBankslipItem_Updated"
                            OnInserted="dvDataSourceBankslipItem_Inserted" OnInserting="dvDataSourceBankslipItem_Inserting"
                            OldValuesParameterFormatString="oldEntity" OnSelecting="dvDataSourceBankslipItem_Selecting"
                            OnDeleting="dvDataSourceBankslipItem_Deleting">
                            <UpdateParameters>
                                <asp:Parameter Name="oldEntity" Type="Object" />
                                <asp:Parameter Name="newEntity" Type="Object" />
                                <asp:Parameter Name="ProvisionValue" Type="Decimal" />
                                <asp:Parameter Name="DebtValue" Type="Decimal" />
                                <asp:Parameter Name="DemandValue" Type="Decimal" />
                            </UpdateParameters>
                            <InsertParameters>
                                <asp:Parameter Name="ProvisionValue" Type="Decimal" />
                                <asp:Parameter Name="DebtValue" Type="Decimal" />
                                <asp:Parameter Name="DemandValue" Type="Decimal" />
                                <asp:Parameter Name="PaymentDescription" Type="String" ConvertEmptyStringToNull="false" />
                                <asp:Parameter Name="CallOnPaymentNumber" Type="String" ConvertEmptyStringToNull="false" />
                            </InsertParameters>
                            <DeleteParameters>
                                <asp:Parameter Name="entityToDelete" Type="Object" />
                                <asp:Parameter Name="DebtValue" Type="Decimal" />
                                <asp:Parameter Name="DemandValue" Type="Decimal" />
                                <asp:Parameter Name="ProvisionValue" Type="Decimal" />
                                <asp:Parameter Name="PaymentDescription" Type="String" ConvertEmptyStringToNull="false" />
                                <asp:Parameter Name="CallOnPaymentNumber" Type="String" ConvertEmptyStringToNull="false" />
                            </DeleteParameters>
                            <SelectParameters>
                                <asp:ControlParameter ControlID="GXGridViewBankslipItems" Name="id" PropertyName="SelectedValue"
                                    Type="Int32" />
                            </SelectParameters>
                        </cc1:DetailsViewDataSource>
                    </asp:View>
                </asp:MultiView>
            </div>
        </asp:View>
        <asp:View ID="viewImportBankslip" runat="server">
            <div class="paddingKontroli">
                <table>
                    <tr>
                        <td style="width: 100px;">
                            Банка
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlBanks" runat="server" DataSourceID="odsBanks" DataTextField="Name"
                                DataValueField="ID" AutoPostBack="true" OnSelectedIndexChanged="ddlBanks_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:ObjectDataSource ID="odsBanks" runat="server" SelectMethod="Select" TypeName="Broker.DataAccess.Bank">
                            </asp:ObjectDataSource>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td style="width: 100px;">
                            Датотека
                        </td>
                        <td>
                            <asp:FileUpload ID="FileUpload1" runat="server" />
                            <asp:Button ID="btnCheck" runat="server" Text="Приказ" OnClick="btnCheck_Click" />
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td style="width: 100px;">
                            Број на извод
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="tbBankslipNumber"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvBankslipNumber" runat="server" ErrorMessage="*"
                                Display="Dynamic" ControlToValidate="tbBankslipNumber"></asp:RequiredFieldValidator>
                            <asp:CompareValidator ID="cvBankslipNumber" runat="server" ErrorMessage="*" Display="Dynamic"
                                ControlToValidate="tbBankslipNumber" Operator="DataTypeCheck" Type="Integer"></asp:CompareValidator>
                        </td>
                    </tr>
                </table>
                <asp:DetailsView GridLines="None" runat="server" ID="dvBankslip" AutoGenerateRows="false">
                    <Fields>
                        <asp:TemplateField HeaderText="Датум" HeaderStyle-Width="104px">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="tbDate" Text='<%#Eval("Date", "{0:d}") %>' ReadOnly="true"></asp:TextBox></ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Должи">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="tbDebtValue" Text='<%#Eval("DebtValue", "{0:#,0.00}") %>'
                                    ReadOnly="true" CssClass="currencyClass"></asp:TextBox></ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Побарува">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="tbDemandValue" Text='<%#Eval("DemandValue", "{0:#,0.00}") %>'
                                    ReadOnly="true" CssClass="currencyClass"></asp:TextBox></ItemTemplate>
                        </asp:TemplateField>
                    </Fields>
                </asp:DetailsView>
            </div>
            <div id="tabeliFrame">
                <div id="header">
                    <div id="content">
                        Ставки на извод
                    </div>
                </div>
                <div id="contentOuter">
                    <div id="contentInner">
                        <asp:GridView ID="gvBankslipItems" runat="server" GridLines="None" AutoGenerateColumns="false"
                            AllowSorting="false" RowStyle-CssClass="row" CssClass="grid" Caption="Ставки на извод">
                            <Columns>
                                <asp:BoundField DataField="ClientName" HeaderText="Назив на примач - налогодавач" />
                                <asp:BoundField DataField="ClientAccountNumber" HeaderText="Сметка" />
                                <asp:BoundField DataField="DebtValue" HeaderText="Должи" DataFormatString="{0:#,0.00}"
                                    ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField DataField="DemandValue" HeaderText="Побарува" DataFormatString="{0:#,0.00}"
                                    ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField DataField="ProvisionValue" HeaderText="Провизија" DataFormatString="{0:#,0.00}"
                                    ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField DataField="Code" HeaderText="Шифра" />
                                <asp:BoundField DataField="PaymentDescription" HeaderText="Цел на дознака" />
                                <%--<asp:BoundField DataField="CallOnPaymentNumber" HeaderText="Повикување на број" />--%>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
            <asp:Button ID="btnInsert" runat="server" Text="Сними" OnClick="btnInsert_Click" />
        </asp:View>
        <asp:View ID="viewPayments" runat="server">
            <div class="paddingKontroli">
                <asp:DetailsView ID="dvBankslipForPayments" runat="server" DataSourceID="dvDataSource"
                    DataKeyNames="ID" AutoGenerateRows="False" GridLines="None">
                    <Fields>
                        <asp:TemplateField HeaderText="Број на извод">
                            <ItemTemplate>
                                <asp:TextBox ID="tbBankslipNumber" runat="server" Text='<%# Eval("BankslipNumber") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Банка">
                            <ItemTemplate>
                                <asp:TextBox ID="tbBankName" runat="server" Text='<%# Eval("Bank.Name") %>' ReadOnly="true"
                                    Width="300px"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Дата">
                            <ItemTemplate>
                                <asp:TextBox ID="tbDate" runat="server" Text='<%# Eval("Date", "{0:d}") %>' ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Должи">
                            <ItemTemplate>
                                <asp:TextBox ID="tbDebtValue" runat="server" Text='<%# Eval("DebtValue", "{0:#,0.00}") %>'
                                    CssClass="currencyClass" ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Побарува">
                            <ItemTemplate>
                                <asp:TextBox ID="tbDemandValue" runat="server" Text='<%# Eval("DemandValue", "{0:#,0.00}") %>'
                                    CssClass="currencyClass" ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Fields>
                </asp:DetailsView>
            </div>
            <br />
            <asp:GridView ID="gvForPayments" runat="server" DataSourceID="odsGVForPayments" DataKeyNames="ID"
                Width="100%" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"
                Caption="Ставки на извод" EmptyDataText="Нема записи!" OnRowCommand="gvForPayments_RowCommand"
                RowStyle-CssClass="row" CssClass="grid" GridLines="None">
                <RowStyle CssClass="row" />
                <Columns>
                    <asp:BoundField DataField="ClientName" HeaderText="Примач-налогодавач" SortExpression="ClientName" />
                    <asp:BoundField DataField="ClientAccountNumber" HeaderText="Сметка" SortExpression="ClientAccountNumber" />
                    <asp:BoundField DataField="DebtValue" HeaderText="Должи" DataFormatString="{0:#,0.00}"
                        SortExpression="DebtValue" />
                    <asp:BoundField DataField="DemandValue" HeaderText="Побарува" SortExpression="DemandValue"
                        DataFormatString="{0:#,0.00}" />
                    <asp:BoundField DataField="PaymentDescription" HeaderText="Опис" SortExpression="PaymentDescription"
                        DataFormatString="{0:#,0.00}" />
                    <asp:ButtonField CommandName="Payment" Text="Плаќања" />
                    <asp:ButtonField CommandName="SingleClick" Text="SingleClick" Visible="False" />
                    <asp:ButtonField CommandName="DoubleClick" Text="DoubleClick" Visible="False" />
                </Columns>
                <PagerStyle HorizontalAlign="Center" />
                <SelectedRowStyle CssClass="rowSelected" />
                <PagerSettings FirstPageText="<< Прва " PreviousPageText="< Претходна " NextPageText=" Следна >"
                    LastPageText=" Последна >>" Mode="NextPreviousFirstLast" />
            </asp:GridView>
            <asp:ObjectDataSource ID="odsGVForPayments" runat="server" TypeName="Broker.DataAccess.ViewBankslipItem"
                DataObjectTypeName="Broker.DataAccess.ViewBankslipItem" SelectMethod="GetByBankslip"
                OnSelecting="odsGVForPayments_Selecting"></asp:ObjectDataSource>
            <br />
            <asp:GridView ID="gvNewPayments" runat="server" DataSourceID="odsNewPayments" DataKeyNames="ID"
                Width="100%" AllowPaging="True" AllowSorting="false" AutoGenerateColumns="False"
                Caption="Неисплатени полиси" EmptyDataText="Нема записи!" OnRowCommand="gvForPayments_RowCommand"
                RowStyle-CssClass="row" CssClass="grid" GridLines="None">
                <RowStyle CssClass="row" />
                <Columns>
                    <asp:BoundField DataField="ID" ItemStyle-Width="0px" ItemStyle-BackColor="Transparent"
                        ItemStyle-ForeColor="Transparent" />
                    <asp:BoundField DataField="policynumber" HeaderText="Полиса" SortExpression="policynumber" />
                    <asp:BoundField DataField="applicationdate" HeaderText="Дата" SortExpression="applicationdate"
                        DataFormatString="{0:d}" />
                    <asp:BoundField DataField="dolzi" HeaderText="Должи" SortExpression="dolzi" DataFormatString="{0:#,0.00}"
                        ItemStyle-CssClass="debtClass" />
                    <asp:BoundField DataField="pobaruva" HeaderText="Побарува" SortExpression="pobaruva"
                        DataFormatString="{0:#,0.00}" ItemStyle-CssClass="demandClass" />
                    <asp:BoundField DataField="saldo" HeaderText="Салдо" SortExpression="saldo" DataFormatString="{0:#,0.00}"
                        ItemStyle-CssClass="currencyClass" />
                    <asp:TemplateField HeaderText="Износ на плаќање">
                        <ItemTemplate>
                            <asp:TextBox ID="tbNewPayment" runat="server" Width="80px"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <PagerStyle HorizontalAlign="Center" />
                <SelectedRowStyle CssClass="rowSelected" />
                <PagerSettings FirstPageText="<< Прва " PreviousPageText="< Претходна " NextPageText=" Следна >"
                    LastPageText=" Последна >>" Mode="NextPreviousFirstLast" />
            </asp:GridView>
            <asp:ObjectDataSource ID="odsNewPayments" runat="server" TypeName="Broker.DataAccess.Policy"
                DataObjectTypeName="Broker.DataAccess.Policy" SelectMethod="GetForFinCard" OnSelecting="odsNewPayments_Selecting">
            </asp:ObjectDataSource>
            <asp:Button ID="btnInsertNewPayments" runat="server" Text="Внеси нови плаќања" Enabled="false"
                OnClick="btnInsertNewPayments_Click" />
        </asp:View>
    </asp:MultiView>
</asp:Content>
