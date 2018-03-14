<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ClientAccounts.aspx.cs" Inherits="Broker_ClientAccounts" Title="Жиро-сметки на клиенти" %>

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
        <%--<div id="button03">
            <asp:Button ID="btnDelete" runat="server" OnClick="btnDelete_Click" CausesValidation="false"
                Enabled="false" UseSubmitBehavior="false" CssClass="izbrisi" BorderWidth="0px" />
        </div>--%>
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
    </div>
    <asp:MultiView ID="mvMain" runat="server">
        <asp:View ID="viewGrid" runat="server">
            <div id="tabeliFrame">
                <div id="header">
                    <div id="content">
                        Сметки
                    </div>
                </div>
                <div id="contentOuter">
                    <div id="contentInner">
                        <cc1:GXGridView ID="GXGridView1" runat="server" DataSourceID="odsGridView" DataKeyNames="ClientID,BankID"
                            Width="100%" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"
                            Caption="Сметки" EmptyDataText="Нема записи кои го задоволуваат критериумот на пребарување!"
                            OnRowCommand="GXGridView1_RowCommand" RowStyle-CssClass="row" CssClass="grid"
                            GridLines="None">
                            <Columns>
                                <asp:BoundField HeaderText="Име" DataField="ClientName" SortExpression="ClientName" />
                                <asp:BoundField HeaderText="Матичен Број" DataField="EMBG" SortExpression="EMBG" />
                                <asp:BoundField HeaderText="Број на сметка" DataField="AccountNumber" SortExpression="AccountNumber" />
                                <asp:BoundField HeaderText="Банка" DataField="BankName" SortExpression="BankName" />
                                <asp:CheckBoxField HeaderText="Активна" DataField="IsActive" SortExpression="IsActive" />
                            </Columns>
                            <PagerStyle HorizontalAlign="Center" />
                            <SelectedRowStyle CssClass="rowSelected" />
                            <PagerSettings FirstPageText="<< Прва " PreviousPageText="< Претходна " NextPageText=" Следна >"
                                LastPageText=" Последна >>" Mode="NextPreviousFirstLast" />
                        </cc1:GXGridView>
                        <cc1:GridViewDataSource ID="odsGridView" runat="server" TypeName="Broker.DataAccess.ClientAccountsView"
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
                                        <cc1:FilterItem FieldName="Име" PropertyName="ClientName" Comparator="StringComparators"
                                            DataType="String" />
                                        <cc1:FilterItem FieldName="Матичен Број" PropertyName="EMBG" Comparator="StringComparators"
                                            DataType="String" />
                                        <cc1:FilterItem FieldName="Број на сметка" PropertyName="AccountNumber" Comparator="StringComparators"
                                            DataType="String" />
                                        <cc1:FilterItem FieldName="Банка" PropertyName="BankName" Comparator="StringComparators"
                                            DataType="String" />
                                    </cc1:FilterControl>
                                </div>
                                <cc1:FilterDataSource ID="odsFilterGridView" runat="server" TypeName="Broker.DataAccess.ClientAccountsView">
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
                                    Нов запис во Сметки
                                </div>
                            </InsertItemTemplate>
                            <EditItemTemplate>
                                <div class="subTitles">
                                    Измена на запис во Сметки
                                </div>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <div class="subTitles">
                                    Бришење на запис од Сметки
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Матичен Број">
                            <InsertItemTemplate>
                                <asp:TextBox ID="tbEMBG" runat="server" Text='<%# Eval("EMBG") %>' MaxLength="13"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvEMBG" runat="server" ControlToValidate="tbEMBG"
                                    Display="Dynamic" ErrorMessage="*">
                                </asp:RequiredFieldValidator>
                                <asp:Button ID="btnSearchEMBG" runat="server" Text="..." OnClick="btnSearchEMBG_Click"
                                    CausesValidation="false" />
                                <%-- <super:entitycalloutvalidator id="CodeInsertValidator" propertyname="MUNICIPALITY_CODE_INSERT_EXISTS"
                                    runat="server" />--%>
                            </InsertItemTemplate>
                            <ItemTemplate>
                                <asp:TextBox ID="tbEMBG" runat="server" Text='<%# Eval("Client.EMBG") %>' ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="tbEMBG" runat="server" Text='<%# Eval("Client.EMBG") %>' ReadOnly="true"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Име">
                            <ItemTemplate>
                                <asp:TextBox ID="tbClientName" runat="server" Text='<%# Eval("Client.Name") %>' ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="tbClientName" runat="server" Text='<%# Eval("Client.Name") %>' MaxLength="100"
                                    ReadOnly="true"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvClientName" runat="server" ControlToValidate="tbClientName"
                                    ErrorMessage="*"></asp:RequiredFieldValidator>
                            </EditItemTemplate>
                            <InsertItemTemplate>
                                <asp:TextBox ID="tbClientName" runat="server" Text='<%# Eval("ClientName") %>' MaxLength="100"
                                    ReadOnly="true"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvClientName" runat="server" ControlToValidate="tbClientName"
                                    ErrorMessage="*"></asp:RequiredFieldValidator>
                            </InsertItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Број на сметка">
                            <ItemTemplate>
                                <asp:TextBox ID="tbAccountNumber" runat="server" Text='<%# Bind("AccountNumber") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="tbAccountNumber" runat="server" Text='<%# Bind("AccountNumber") %>'
                                    MaxLength="50"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvAccountNumber" runat="server" ControlToValidate="tbClientName"
                                    ErrorMessage="*"></asp:RequiredFieldValidator>
                            </EditItemTemplate>
                            <InsertItemTemplate>
                                <asp:TextBox ID="tbAccountNumber" runat="server" Text='<%# Bind("AccountNumber") %>'
                                    MaxLength="50"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvAccountNumber" runat="server" ControlToValidate="tbAccountNumber"
                                    ErrorMessage="*"></asp:RequiredFieldValidator>
                            </InsertItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Банка">
                            <ItemTemplate>
                                <asp:DropDownList ID="ddlBanks" runat="server" Enabled="False" DataSourceID="odsBanks"
                                    DataTextField="Name" DataValueField="ID" SelectedValue='<%# Bind("BankID") %>'>
                                </asp:DropDownList>
                                <asp:ObjectDataSource ID="odsBanks" runat="server" SelectMethod="GetActiveBanks"
                                    TypeName="Broker.DataAccess.Bank"></asp:ObjectDataSource>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddlBanks" runat="server" DataSourceID="odsBanks" DataTextField="Name"
                                    DataValueField="ID" SelectedValue='<%# Bind("BankID") %>' Enabled="false">
                                </asp:DropDownList>
                                <asp:ObjectDataSource ID="odsBanks" runat="server" SelectMethod="GetActiveBanks"
                                    TypeName="Broker.DataAccess.Bank"></asp:ObjectDataSource>
                            </EditItemTemplate>
                            <InsertItemTemplate>
                                <asp:DropDownList ID="ddlBanks" runat="server" DataSourceID="odsBanks" DataTextField="Name"
                                    DataValueField="ID" SelectedValue='<%# Bind("BankID") %>'>
                                </asp:DropDownList>
                                <asp:ObjectDataSource ID="odsBanks" runat="server" SelectMethod="GetActiveBanks"
                                    TypeName="Broker.DataAccess.Bank"></asp:ObjectDataSource>
                            </InsertItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Активна">
                            <ItemTemplate>
                                <asp:CheckBox ID="cbIsActive" runat="server" Checked='<%#Bind("IsActive") %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:CheckBox ID="cbIsActive" runat="server" Checked='<%#Bind("IsActive") %>' />
                            </EditItemTemplate>
                            <InsertItemTemplate>
                                <asp:CheckBox ID="cbIsActive" runat="server" Checked='<%#Bind("IsActive") %>' Visible="false" />
                            </InsertItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:TextBox ID="tbID" runat="server" Text='<%#Bind("ID") %>' Visible="false" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="tbID" runat="server" Text='<%#Bind("ID") %>' Visible="false" />
                            </EditItemTemplate>
                            <InsertItemTemplate>
                                <asp:TextBox ID="tbID" runat="server" Text='<%#Bind("ID") %>' Visible="false" />
                            </InsertItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Button ID="btnCancel" runat="server" CommandName="Cancel" Text="Откажи" CausesValidation="false" />
                                <asp:Button ID="btnDelete" runat="server" Text="Избриши" OnClientClick="return confirm('Дали сте сигурни?')"
                                    OnClick="btnDelete_Click1" />
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
                <cc1:DetailsViewDataSource ID="dvDataSource" runat="server" TypeName="Broker.DataAccess.ClientAccount"
                    ConflictDetection="CompareAllValues" DataObjectTypeName="Broker.DataAccess.ClientAccount"
                    DeleteMethod="Delete" InsertMethod="Insert" SelectMethod="GetByClientAndBank"
                    UpdateMethod="Update" OnUpdating="dvDataSource_Updating" OnUpdated="dvDataSource_Updated"
                    OnInserted="dvDataSource_Inserted" OnInserting="dvDataSource_Inserting" OldValuesParameterFormatString="oldEntity"
                    OnSelecting="dvDataSource_Selecting">
                    <UpdateParameters>
                        <asp:Parameter Name="oldEntity" Type="Object" />
                        <asp:Parameter Name="newEntity" Type="Object" />
                    </UpdateParameters>
                    <SelectParameters>
                        <%-- <asp:ControlParameter ControlID="GXGridView1" Name="ClientID" PropertyName="SelectedValue"
                            Type="Int32" />
                         <asp:ControlParameter ControlID="GXGridView1" Name="BankID" PropertyName="SelectedValue"
                            Type="Int32" />--%>
                    </SelectParameters>
                </cc1:DetailsViewDataSource>
            </div>
        </asp:View>
        <asp:View ID="viewSearch" runat="server">
            <div class="paddingKontroli">
                <cc1:SearchControl ID="SearchControl1" runat="server" SearchDataSourceID="odsSearch"
                    GridViewToSearch="GXGridView1" OnSearch="SearchControl1_Search">
                    <cc1:SearchItem FieldName="Име" PropertyName="ClientName" Comparator="StringComparators"
                        DataType="String" />
                    <cc1:SearchItem FieldName="Матичен Број" PropertyName="EMBG" Comparator="StringComparators"
                        DataType="String" />
                    <cc1:SearchItem FieldName="Број на сметка" PropertyName="AccountNumber" Comparator="StringComparators"
                        DataType="String" />
                    <cc1:SearchItem FieldName="Банка" PropertyName="BankName" Comparator="StringComparators"
                        DataType="String" />
                </cc1:SearchControl>
                <cc1:GridViewDataSource ID="odsSearch" runat="server" TypeName="Broker.DataAccess.ClientAccountsView"
                    SelectCountMethod="SelectSearchCountCached" SelectMethod="SelectSearch">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="SearchControl1" PropertyName="SearchArguments" Name="sArgument" />
                    </SelectParameters>
                </cc1:GridViewDataSource>
            </div>
        </asp:View>
        <asp:View ID="viewReport" runat="server">
            <div class="paddingKontroli">
                <cc1:ReportControl ID="reportControl" runat="server" TypeName="Broker.DataAccess.ClientAccountsView"
                    GridViewID="GXGridView1" SearchControlID="SearchControl1">
                    <cc1:PrintItem HeaderText="Име" PropertyName="ClientName" />
                    <cc1:PrintItem HeaderText="Матичен Број" PropertyName="EMBG" />
                    <cc1:PrintItem HeaderText="Број на сметка" PropertyName="AccountNumber" />
                    <cc1:PrintItem HeaderText="Банка" PropertyName="BankName" />
                </cc1:ReportControl>
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>
