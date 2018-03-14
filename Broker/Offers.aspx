<%@ Page Title="Понуда" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Offers.aspx.cs" Inherits="Broker_Offers" Culture="mk-MK" UICulture="mk-MK" %>

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
        <%--<div id="button03">
            <asp:Button ID="btnDelete" runat="server" OnClick="btnDelete_Click" CausesValidation="false"
                Enabled="false" UseSubmitBehavior="false" CssClass="izbrisi" BorderWidth="0px" />
        </div>--%>
        <div id="button03">
            <asp:Button ID="btnPreview" runat="server" ToolTip="Освежи" OnClick="btnPreview_Click"
                CausesValidation="false" UseSubmitBehavior="false" CssClass="osvezi" BorderWidth="0px" />
        </div>
        <div id="button04">
            <asp:Button ID="btnReport" runat="server" ToolTip="Извештај" CausesValidation="false"
                UseSubmitBehavior="false" OnClick="btnReport_Click" CssClass="izvestaj" BorderWidth="0px" />
        </div>
        <div id="button05">
            <asp:Button ID="btnSearch" runat="server" ToolTip="Пребарај" OnClick="btnSearch_Click"
                CausesValidation="false" UseSubmitBehavior="false" CssClass="prebaraj" BorderWidth="0px" />
        </div>
        <div id="button06">
            <asp:Button ID="btnOfferItems" runat="server" OnClick="btnOfferItems_Click" ToolTip="Ставки"
                CausesValidation="false" UseSubmitBehavior="false" CssClass="stavki" BorderWidth="0px"
                Enabled="false" />
        </div>
        <div id="button07">
            <asp:Button ID="btnAttachments" runat="server" TootTip="Документи" OnClick="btnAttachments_Click"
                CausesValidation="false" UseSubmitBehavior="false" CssClass="dokumenti" BorderWidth="0px"
                Enabled="false" />
        </div>
        <div id="button08">
            <asp:Button ID="btnPrint" runat="server" TootTip="Печати" OnClick="btnPrint_Click"
                CausesValidation="false" UseSubmitBehavior="false" CssClass="pecati" BorderWidth="0px"
                Enabled="false" />
        </div>
    </div>
    <asp:MultiView ID="mvMain" runat="server">
        <asp:View ID="viewGrid" runat="server">
            <div id="tabeliFrame">
                <div id="header">
                    <div id="content">
                        Понуди
                    </div>
                </div>
                <div id="contentOuter">
                    <div id="contentInner">
                        <cc1:GXGridView ID="GXGridView1" runat="server" DataSourceID="odsGridView" DataKeyNames="ID"
                            Width="100%" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"
                            Caption="Понуди" EmptyDataText="Нема записи кои го задоволуваат критериумот на пребарување!"
                            OnRowCommand="GXGridView1_RowCommand" RowStyle-CssClass="row" CssClass="grid"
                            GridLines="None">
                            <Columns>
                                <asp:BoundField HeaderText="Број на понуда" DataField="OfferNumber" SortExpression="OfferNumber" />
                                <asp:BoundField HeaderText="Договорувач" DataField="ClientName" SortExpression="ClientName" />
                                <asp:BoundField HeaderText="Осигуреник" DataField="OwnerName" SortExpression="OwnerName" />
                                <asp:BoundField HeaderText="Статус" DataField="StatusDescription" SortExpression="StatusDescription" />
                                <asp:BoundField HeaderText="Износ" DataField="Cost" SortExpression="Cost" />
                            </Columns>
                            <PagerStyle HorizontalAlign="Center" />
                            <SelectedRowStyle CssClass="rowSelected" />
                            <PagerSettings FirstPageText="<< Прва " PreviousPageText="< Претходна " NextPageText=" Следна >"
                                LastPageText=" Последна >>" Mode="NextPreviousFirstLast" />
                        </cc1:GXGridView>
                        <cc1:GridViewDataSource ID="odsGridView" runat="server" TypeName="Broker.DataAccess.ViewOffer"
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
                                        <cc1:FilterItem FieldName="Број на понуда" PropertyName="OfferNumber" Comparator="StringComparators"
                                            DataType="String" />
                                        <cc1:FilterItem FieldName="ЕМБГ Договорувач" PropertyName="ClientEMBG" Comparator="StringComparators"
                                            DataType="String" />
                                        <cc1:FilterItem FieldName="Договорувач" PropertyName="ClientName" Comparator="StringComparators"
                                            DataType="String" />
                                        <cc1:FilterItem FieldName="ЕМБГ Осигуреник" PropertyName="OwnerEMBG" Comparator="StringComparators"
                                            DataType="String" />
                                        <cc1:FilterItem FieldName="Осигуреник" PropertyName="OwnerName" Comparator="StringComparators"
                                            DataType="String" />
                                        <cc1:FilterItem FieldName="Статус" PropertyName="StatusDescription" Comparator="StringComparators"
                                            DataType="String" />
                                        <cc1:FilterItem FieldName="Износ" PropertyName="Cost" Comparator="NumericComparators"
                                            DataType="Decimal" />
                                    </cc1:FilterControl>
                                </div>
                                <cc1:FilterDataSource ID="odsFilterGridView" runat="server" TypeName="Broker.DataAccess.ViewOffer">
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
                                    Нов запис во Понуди
                                </div>
                            </InsertItemTemplate>
                            <EditItemTemplate>
                                <div class="subTitles">
                                    Измена на запис во Понуди
                                </div>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <div class="subTitles">
                                    Бришење на запис од Понуди
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Број на понуда">
                            <InsertItemTemplate>
                                <asp:TextBox ID="tbOfferNumber" runat="server" Text='<%# Bind("OfferNumber") %>'
                                    MaxLength="20" ReadOnly="true"></asp:TextBox>
                                <super:EntityCallOutValidator ID="OfferNumberInsertValidator" PropertyName="OFFERNUMBER_EXIST"
                                    runat="server" />
                                <asp:RequiredFieldValidator ID="rfvOfferNumber" runat="server" ControlToValidate="tbOfferNumber"
                                    Display="Dynamic" ErrorMessage="*">
                                </asp:RequiredFieldValidator>
                            </InsertItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="tbOfferNumber" runat="server" Text='<%# Bind("OfferNumber") %>'
                                    MaxLength="20" ReadOnly="true"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvOfferNumber" runat="server" ControlToValidate="tbOfferNumber"
                                    Display="Dynamic" ErrorMessage="*">
                                </asp:RequiredFieldValidator>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:TextBox ID="tbOfferNumber" runat="server" Text='<%# Bind("OfferNumber") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Дата на понуда" InsertVisible="false">
                            <EditItemTemplate>
                                <asp:TextBox ID="tbOfferDate" runat="server" Text='<%#Bind("OfferDate") %>' ReadOnly="true"></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:TextBox ID="tbOfferDate" runat="server" Text='<%#Bind("OfferDate") %>' ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Број на налог">
                            <InsertItemTemplate>
                                <asp:TextBox ID="tbOrderNumber" runat="server" MaxLength="20"></asp:TextBox>
                                <super:EntityCallOutValidator ID="CodeInsertValidator" PropertyName="ORDERNUMBER_DOESNOT_EXISTS"
                                    runat="server" />
                                <asp:Button ID="btnSearchOrderNumber" runat="server" Text="..." OnClick="btnSearchOrderNumber_Click"
                                    CausesValidation="false" />
                            </InsertItemTemplate>
                            <ItemTemplate>
                                <asp:TextBox ID="tbOrderNumber" runat="server" Text='<%# Eval("Order.OrderNumber") %>'
                                    ReadOnly="true"></asp:TextBox>
                                <asp:TextBox ID="tbOrderID" runat="server" Text='<%# Bind("OrderID") %>' ReadOnly="true"
                                    Visible="false"></asp:TextBox>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="tbOrderNumber" runat="server" Text='<%# Eval("Order.OrderNumber") %>'
                                    ReadOnly="true"></asp:TextBox>
                                <asp:TextBox ID="tbOrderID" runat="server" Text='<%# Bind("OrderID") %>' Visible="false"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="МБ. Договорувач">
                            <ItemTemplate>
                                <asp:TextBox ID="tbClientEMBG" runat="server" Text='<%# Eval("Client.EMBG") %>' ReadOnly="true"></asp:TextBox>
                                <asp:TextBox ID="tbClientID" runat="server" Text='<%# Bind("ClientID") %>' ReadOnly="true"
                                    Visible="false"></asp:TextBox>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="tbClientEMBG" runat="server" Text='<%# Eval("Client.EMBG") %>' MaxLength="100"
                                    ReadOnly="true"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvClientEMBG" runat="server" ControlToValidate="tbClientName"
                                    ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                <asp:TextBox ID="tbClientID" runat="server" Text='<%# Bind("ClientID") %>' ReadOnly="true"
                                    Visible="false"></asp:TextBox>
                            </EditItemTemplate>
                            <InsertItemTemplate>
                                <asp:TextBox ID="tbClientEMBG" runat="server" Text='<%# Eval("Order.Client.EMBG") %>'
                                    MaxLength="13"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvClientEMBG" runat="server" ControlToValidate="tbClientEMBG"
                                    Display="Dynamic" ErrorMessage="*">
                                </asp:RequiredFieldValidator>
                                <asp:Button ID="btnSearchClientEMBG" runat="server" Text="..." OnClick="btnSearchClientEMBG_Click"
                                    CausesValidation="false" />
                                <asp:Panel ID="pnlClient" runat="server" Visible="false">
                                    <asp:DetailsView ID="ClientDetailsView" runat="server" DataSourceID="ClientdvDataSource"
                                        DataKeyNames="ID" AutoGenerateRows="False" OnItemCommand="ClientDetailsView_ItemCommand"
                                        OnItemDeleted="ClientDetailsView_ItemDeleted" OnItemInserted="ClientDetailsView_ItemInserted"
                                        OnItemUpdated="ClientDetailsView_ItemUpdated" OnModeChanging="ClientDetailsView_ModeChanging"
                                        OnItemInserting="ClientDetailsView_ItemInserting" GridLines="None" DefaultMode="Insert">
                                        <Fields>
                                            <asp:TemplateField>
                                                <InsertItemTemplate>
                                                    <div class="subTitles">
                                                        Нов запис во Клиенти
                                                    </div>
                                                </InsertItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Име и Презиме">
                                                <InsertItemTemplate>
                                                    <asp:TextBox ID="tbName" runat="server" Text='<%# Bind("Name") %>' MaxLength="100"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="tbName"
                                                        Display="Dynamic" ErrorMessage="*">
                                                    </asp:RequiredFieldValidator>
                                                    <super:EntityCallOutValidator ID="NameInsertValidator" PropertyName="NAME" runat="server" />
                                                </InsertItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Матичен Број">
                                                <InsertItemTemplate>
                                                    <asp:TextBox ID="tbEMBG" runat="server" Text='<%# Bind("EMBG") %>' MaxLength="100"
                                                        ReadOnly="true"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvEMBG" runat="server" ControlToValidate="tbEMBG"
                                                        ErrorMessage="*"></asp:RequiredFieldValidator>
                                                    <super:EntityCallOutValidator ID="EmbgInsertValidator" PropertyName="EMBG" runat="server" />
                                                </InsertItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Адреса">
                                                <InsertItemTemplate>
                                                    <asp:TextBox ID="tbAddress" MaxLength="100" runat="server" Text='<%# Bind("Address") %>'></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvAddress" runat="server" ControlToValidate="tbAddress"
                                                        ErrorMessage="*"></asp:RequiredFieldValidator>
                                                    <super:EntityCallOutValidator ID="AddressInsertValidator" PropertyName="ADDRESS"
                                                        runat="server" />
                                                </InsertItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Населено Место">
                                                <InsertItemTemplate>
                                                    <asp:DropDownList ID="ddlPlaces" runat="server" DataSourceID="odsActivePlaces" DataTextField="Name"
                                                        DataValueField="ID" SelectedValue='<%# Bind("PlaceID") %>'>
                                                    </asp:DropDownList>
                                                    <asp:ObjectDataSource ID="odsActivePlaces" runat="server" SelectMethod="GetActivePlaces"
                                                        TypeName="Broker.DataAccess.Place"></asp:ObjectDataSource>
                                                </InsertItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Телефон">
                                                <InsertItemTemplate>
                                                    <asp:TextBox ID="tbPhone" MaxLength="30" runat="server" Text='<%# Bind("Phone") %>'></asp:TextBox>
                                                </InsertItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Мобилен Тел.">
                                                <InsertItemTemplate>
                                                    <asp:TextBox ID="tbMobile" MaxLength="30" runat="server" Text='<%# Bind("Mobile") %>'></asp:TextBox>
                                                </InsertItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Факс">
                                                <InsertItemTemplate>
                                                    <asp:TextBox ID="tbFax" MaxLength="30" runat="server" Text='<%# Bind("Fax") %>'></asp:TextBox>
                                                </InsertItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Email">
                                                <InsertItemTemplate>
                                                    <asp:TextBox ID="tbEmail" MaxLength="100" runat="server" Text='<%# Bind("Email") %>'></asp:TextBox>
                                                </InsertItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Правно лице">
                                                <InsertItemTemplate>
                                                    <asp:CheckBox ID="cbIsLaw" runat="server" Checked='<%#Bind("IsLaw") %>' />
                                                </InsertItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <InsertItemTemplate>
                                                    <asp:Button ID="btnCancel" runat="server" CommandName="Cancel" Text="Откажи" CausesValidation="false" />
                                                    <asp:Button ID="btnInsert" runat="server" CommandName="Insert" Text="Внеси" CausesValidation="false" />
                                                </InsertItemTemplate>
                                            </asp:TemplateField>
                                        </Fields>
                                    </asp:DetailsView>
                                    <cc1:DetailsViewDataSource ID="ClientdvDataSource" runat="server" TypeName="Broker.DataAccess.Client"
                                        ConflictDetection="CompareAllValues" DataObjectTypeName="Broker.DataAccess.Client"
                                        DeleteMethod="Delete" InsertMethod="Insert" SelectMethod="GetClientByOfferID"
                                        UpdateMethod="Update" OnUpdating="ClientdvDataSource_Updating" OnUpdated="ClientdvDataSource_Updated"
                                        OnInserted="ClientdvDataSource_Inserted" OnInserting="ClientdvDataSource_Inserting">
                                        <InsertParameters>
                                            <asp:Parameter Name="entityToInsert" Type="Object" />
                                        </InsertParameters>
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="GXGridView1" Name="offerID" PropertyName="SelectedValue"
                                                Type="Int32" />
                                        </SelectParameters>
                                    </cc1:DetailsViewDataSource>
                                </asp:Panel>
                            </InsertItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Договорувач">
                            <ItemTemplate>
                                <asp:TextBox ID="tbClientName" runat="server" Text='<%# Eval("Client.Name") %>' ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="tbClientName" runat="server" Text='<%# Eval("Client.Name") %>' MaxLength="100"
                                    ReadOnly="true"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvClientName" runat="server" ControlToValidate="tbClientName"
                                    ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                            </EditItemTemplate>
                            <InsertItemTemplate>
                                <asp:TextBox ID="tbClientName" runat="server" Text='<%# Eval("Order.Client.Name") %>'
                                    MaxLength="100" ReadOnly="true"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvClientName" runat="server" ControlToValidate="tbClientName"
                                    ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                            </InsertItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="МБ. Осигуреник">
                            <ItemTemplate>
                                <asp:TextBox ID="tbOwnerEMBG" runat="server" Text='<%# Eval("Client1.EMBG") %>' ReadOnly="true"></asp:TextBox>
                                <asp:TextBox ID="tbOwnerID" runat="server" Text='<%# Bind("OwnerID") %>' ReadOnly="true"
                                    Visible="false"></asp:TextBox>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="tbOwnerEMBG" runat="server" Text='<%# Eval("Client1.EMBG") %>' MaxLength="100"
                                    ReadOnly="true"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvOwnerEMBG" runat="server" ControlToValidate="tbOwnerName"
                                    ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                <asp:TextBox ID="tbOwnerID" runat="server" Text='<%# Bind("OwnerID") %>' ReadOnly="true"
                                    Visible="false"></asp:TextBox>
                            </EditItemTemplate>
                            <InsertItemTemplate>
                                <asp:TextBox ID="tbOwnerEMBG" runat="server" Text='<%# Eval("Order.Client1.EMBG") %>'
                                    MaxLength="13"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvOwnerEMBG" runat="server" ControlToValidate="tbOwnerEMBG"
                                    Display="Dynamic" ErrorMessage="*">
                                </asp:RequiredFieldValidator>
                                <asp:Button ID="btnSearchOwnerEMBG" runat="server" Text="..." OnClick="btnSearchOwnerEMBG_Click"
                                    CausesValidation="false" />
                                <asp:Panel ID="pnlOwner" runat="server" Visible="false">
                                    <asp:DetailsView ID="OwnerDetailsView" runat="server" DataSourceID="OwnerdvDataSource"
                                        DataKeyNames="ID" AutoGenerateRows="False" OnItemCommand="OwnerDetailsView_ItemCommand"
                                        OnItemDeleted="OwnerDetailsView_ItemDeleted" OnItemInserted="OwnerDetailsView_ItemInserted"
                                        OnItemUpdated="OwnerDetailsView_ItemUpdated" OnModeChanging="OwnerDetailsView_ModeChanging"
                                        OnItemInserting="OwnerDetailsView_ItemInserting" GridLines="None" DefaultMode="Insert">
                                        <Fields>
                                            <asp:TemplateField>
                                                <InsertItemTemplate>
                                                    <div class="subTitles">
                                                        Нов запис во Клиенти
                                                    </div>
                                                </InsertItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Име и Презиме">
                                                <InsertItemTemplate>
                                                    <asp:TextBox ID="tbName" runat="server" Text='<%# Bind("Name") %>' MaxLength="100"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="tbName"
                                                        Display="Dynamic" ErrorMessage="*">
                                                    </asp:RequiredFieldValidator>
                                                    <super:EntityCallOutValidator ID="NameInsertValidator" PropertyName="OwnerNAME" runat="server" />
                                                </InsertItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Матичен Број">
                                                <InsertItemTemplate>
                                                    <asp:TextBox ID="tbEMBG" runat="server" Text='<%# Bind("EMBG") %>' MaxLength="100"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvEMBG" runat="server" ControlToValidate="tbEMBG"
                                                        ErrorMessage="*"></asp:RequiredFieldValidator>
                                                    <super:EntityCallOutValidator ID="EmbgInsertValidator" PropertyName="OwnerEMBG" runat="server" />
                                                </InsertItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Адреса">
                                                <InsertItemTemplate>
                                                    <asp:TextBox ID="tbAddress" MaxLength="100" runat="server" Text='<%# Bind("Address") %>'></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvAddress" runat="server" ControlToValidate="tbAddress"
                                                        ErrorMessage="*"></asp:RequiredFieldValidator>
                                                    <super:EntityCallOutValidator ID="AddressInsertValidator" PropertyName="OwnerADDRESS"
                                                        runat="server" />
                                                </InsertItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Населено Место">
                                                <InsertItemTemplate>
                                                    <asp:DropDownList ID="ddlPlaces" runat="server" DataSourceID="odsActivePlaces" DataTextField="Name"
                                                        DataValueField="ID" SelectedValue='<%# Bind("PlaceID") %>'>
                                                    </asp:DropDownList>
                                                    <asp:ObjectDataSource ID="odsActivePlaces" runat="server" SelectMethod="GetActivePlaces"
                                                        TypeName="Broker.DataAccess.Place"></asp:ObjectDataSource>
                                                </InsertItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Телефон">
                                                <InsertItemTemplate>
                                                    <asp:TextBox ID="tbPhone" MaxLength="30" runat="server" Text='<%# Bind("Phone") %>'></asp:TextBox>
                                                </InsertItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Мобилен Тел.">
                                                <InsertItemTemplate>
                                                    <asp:TextBox ID="tbMobile" MaxLength="30" runat="server" Text='<%# Bind("Mobile") %>'></asp:TextBox>
                                                </InsertItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Факс">
                                                <InsertItemTemplate>
                                                    <asp:TextBox ID="tbFax" MaxLength="30" runat="server" Text='<%# Bind("Fax") %>'></asp:TextBox>
                                                </InsertItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Email">
                                                <InsertItemTemplate>
                                                    <asp:TextBox ID="tbEmail" MaxLength="100" runat="server" Text='<%# Bind("Email") %>'></asp:TextBox>
                                                </InsertItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Правно лице">
                                                <InsertItemTemplate>
                                                    <asp:CheckBox ID="cbIsLaw" runat="server" Checked='<%#Bind("IsLaw") %>' />
                                                </InsertItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <InsertItemTemplate>
                                                    <asp:Button ID="btnCancel" runat="server" CommandName="Cancel" Text="Откажи" CausesValidation="false" />
                                                    <asp:Button ID="btnInsert" runat="server" CommandName="Insert" Text="Внеси" CausesValidation="false" />
                                                </InsertItemTemplate>
                                            </asp:TemplateField>
                                        </Fields>
                                    </asp:DetailsView>
                                    <cc1:DetailsViewDataSource ID="OwnerdvDataSource" runat="server" TypeName="Broker.DataAccess.Client"
                                        ConflictDetection="CompareAllValues" DataObjectTypeName="Broker.DataAccess.Client"
                                        DeleteMethod="Delete" InsertMethod="Insert" SelectMethod="GetOwnerByOfferID"
                                        UpdateMethod="Update" OnUpdating="OwnerdvDataSource_Updating" OnUpdated="OwnerdvDataSource_Updated"
                                        OnInserted="OwnerdvDataSource_Inserted" OnInserting="OwnerdvDataSource_Inserting">
                                        <InsertParameters>
                                            <asp:Parameter Name="entityToInsert" Type="Object" />
                                        </InsertParameters>
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="GXGridView1" Name="offerID" PropertyName="SelectedValue"
                                                Type="Int32" />
                                        </SelectParameters>
                                    </cc1:DetailsViewDataSource>
                                </asp:Panel>
                            </InsertItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Осигуреник">
                            <ItemTemplate>
                                <asp:TextBox ID="tbOwnerName" runat="server" Text='<%# Eval("Client1.Name") %>' ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="tbOwnerName" runat="server" Text='<%# Eval("Client1.Name") %>' MaxLength="100"
                                    ReadOnly="true"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvOwnerName" runat="server" ControlToValidate="tbOwnerName"
                                    ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                            </EditItemTemplate>
                            <InsertItemTemplate>
                                <asp:TextBox ID="tbOwnerName" runat="server" Text='<%# Eval("Order.Client1.Name") %>'
                                    MaxLength="100" ReadOnly="true"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvOwnerName" runat="server" ControlToValidate="tbOwnerName"
                                    ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                            </InsertItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Статус">
                            <ItemTemplate>
                                <asp:DropDownList ID="ddlStatuses" runat="server" Enabled="False" DataSourceID="odsStatuses"
                                    DataTextField="Description" DataValueField="ID" SelectedValue='<%# Bind("StatusID") %>'>
                                </asp:DropDownList>
                                <asp:ObjectDataSource ID="odsStatuses" runat="server" SelectMethod="GetActiveStatusesByDocumentType"
                                    TypeName="Broker.DataAccess.Statuse">
                                    <SelectParameters>
                                        <asp:Parameter DefaultValue="6" Name="docTypeID" Type="Int32" />
                                    </SelectParameters>
                                </asp:ObjectDataSource>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddlStatuses" runat="server" DataSourceID="odsStatuses" DataTextField="Description"
                                    DataValueField="ID" SelectedValue='<%# Bind("StatusID") %>'>
                                </asp:DropDownList>
                                <asp:ObjectDataSource ID="odsStatuses" runat="server" SelectMethod="GetActiveStatusesByDocumentType"
                                    TypeName="Broker.DataAccess.Statuse">
                                    <SelectParameters>
                                        <asp:Parameter DefaultValue="6" Name="docTypeID" Type="Int32" />
                                    </SelectParameters>
                                </asp:ObjectDataSource>
                            </EditItemTemplate>
                            <InsertItemTemplate>
                                <asp:DropDownList ID="ddlStatuses" runat="server" DataSourceID="odsStatuses" DataTextField="Description"
                                    DataValueField="ID" SelectedValue='<%# Bind("StatusID") %>'>
                                </asp:DropDownList>
                                <asp:ObjectDataSource ID="odsStatuses" runat="server" SelectMethod="GetActiveStatusesByDocumentType"
                                    TypeName="Broker.DataAccess.Statuse">
                                    <SelectParameters>
                                        <asp:Parameter DefaultValue="6" Name="docTypeID" Type="Int32" />
                                    </SelectParameters>
                                </asp:ObjectDataSource>
                            </InsertItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Износ" InsertVisible="false">
                            <EditItemTemplate>
                                <asp:TextBox ID="tbCost" runat="server" ReadOnly="true" Text='<%#Bind("Cost") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:TextBox ID="tbCost" runat="server" Text='<%#Bind("Cost") %>' ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Изработена од" InsertVisible="false">
                            <EditItemTemplate>
                                <asp:TextBox ID="tbUserName" runat="server" Text='<%#Eval("User.UserName") %>' ReadOnly="true"></asp:TextBox>
                                <asp:TextBox ID="tbUserID" runat="server" Text='<%#Bind("UserID") %>' Visible="false"
                                    ReadOnly="true"></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:TextBox ID="tbUserName" runat="server" Text='<%#Eval("User.UserName") %>' ReadOnly="true"></asp:TextBox>
                                <asp:TextBox ID="tbUserID" runat="server" Text='<%#Bind("UserID") %>' Visible="false"
                                    ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Опис">
                            <InsertItemTemplate>
                                <asp:TextBox ID="tbDescription" runat="server" Text='<%#Bind("Description") %>' Rows="4"
                                    TextMode="MultiLine"></asp:TextBox>
                            </InsertItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="tbDescription" runat="server" Text='<%#Bind("Description") %>' Rows="4"
                                    TextMode="MultiLine"></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:TextBox ID="tbDescription" runat="server" Text='<%#Bind("Description") %>' Rows="4"
                                    TextMode="MultiLine" ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Button ID="btnCancel" runat="server" CommandName="Cancel" Text="Откажи" CausesValidation="false" />
                                <%--<asp:Button ID="btnDelete" runat="server" Text="Избриши" OnClientClick="return confirm('Дали сте сигурни?')" />--%>
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
                <cc1:DetailsViewDataSource ID="dvDataSource" runat="server" TypeName="Broker.DataAccess.Offer"
                    ConflictDetection="CompareAllValues" DataObjectTypeName="Broker.DataAccess.Offer"
                    DeleteMethod="Delete" InsertMethod="Insert" SelectMethod="GetOffer" UpdateMethod="Update"
                    OnUpdating="dvDataSource_Updating" OnUpdated="dvDataSource_Updated" OnInserted="dvDataSource_Inserted"
                    OnInserting="dvDataSource_Inserting" OldValuesParameterFormatString="oldEntity"
                    OnSelecting="dvDataSource_Selecting">
                    <UpdateParameters>
                        <asp:Parameter Name="oldEntity" Type="Object" />
                        <asp:Parameter Name="newEntity" Type="Object" />
                        <asp:Parameter Name="OfferDate" Type="DateTime" />
                        <asp:Parameter Name="Cost" Type="Decimal" />
                        <asp:Parameter Name="OrderID" Type="Int32" />
                    </UpdateParameters>
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
                    <cc1:SearchItem FieldName="Број на понуда" PropertyName="OfferNumber" Comparator="StringComparators"
                        DataType="String" />
                    <cc1:SearchItem FieldName="ЕМБГ Договорувач" PropertyName="ClientEMBG" Comparator="StringComparators"
                        DataType="String" />
                    <cc1:SearchItem FieldName="Договорувач" PropertyName="ClientName" Comparator="StringComparators"
                        DataType="String" />
                    <cc1:SearchItem FieldName="ЕМБГ Осигуреник" PropertyName="OwnerEMBG" Comparator="StringComparators"
                        DataType="String" />
                    <cc1:SearchItem FieldName="Осигуреник" PropertyName="OwnerName" Comparator="StringComparators"
                        DataType="String" />
                    <cc1:SearchItem FieldName="Статус" PropertyName="StatusDescription" Comparator="StringComparators"
                        DataType="String" />
                    <cc1:SearchItem FieldName="Износ" PropertyName="Cost" Comparator="NumericComparators"
                        DataType="Decimal" />
                </cc1:SearchControl>
                <cc1:GridViewDataSource ID="odsSearch" runat="server" TypeName="Broker.DataAccess.ViewOffer"
                    SelectCountMethod="SelectSearchCountCached" SelectMethod="SelectSearch">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="SearchControl1" PropertyName="SearchArguments" Name="sArgument" />
                    </SelectParameters>
                </cc1:GridViewDataSource>
            </div>
        </asp:View>
        <asp:View ID="viewReport" runat="server">
            <div class="paddingKontroli">
                <cc1:ReportControl ID="reportControl" runat="server" TypeName="Broker.DataAccess.ViewOffer"
                    GridViewID="GXGridView1" SearchControlID="SearchControl1">
                    <cc1:PrintItem HeaderText="Број на понуда" PropertyName="OfferNumber" />
                    <cc1:PrintItem HeaderText="Договорувач" PropertyName="ClientName" />
                    <cc1:PrintItem HeaderText="Осигуреник" PropertyName="OwnerName" />
                    <cc1:PrintItem HeaderText="Статус" PropertyName="StatusDescription" />
                    <cc1:PrintItem HeaderText="Износ" PropertyName="Cost" />
                </cc1:ReportControl>
            </div>
        </asp:View>
        <asp:View ID="viewOfferItems" runat="server">
            <div class="paddingKontroli">
                <asp:DetailsView ID="dvOfferPreview" runat="server" DataSourceID="odsOfferPreview"
                    DataKeyNames="ID" AutoGenerateRows="False" OnItemCommand="dvOfferPreview_ItemCommand"
                    OnModeChanging="dvOfferPreview_ModeChanging" GridLines="None" DefaultMode="ReadOnly">
                    <Fields>
                        <asp:TemplateField HeaderText="Број на понуда">
                            <ItemTemplate>
                                <asp:TextBox ID="tbOfferNumber" runat="server" Text='<%# Bind("OfferNumber") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Договорувач">
                            <ItemTemplate>
                                <asp:TextBox ID="tbClientEMBG" runat="server" Text='<%# Eval("Client.EMBG") %>' ReadOnly="true"></asp:TextBox>
                                <asp:TextBox ID="tbClientName" Width="250px" runat="server" Text='<%# Eval("Client.Name") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Осигуреник">
                            <ItemTemplate>
                                <asp:TextBox ID="tbOwnerEMBG" runat="server" Text='<%# Eval("Client1.EMBG") %>' ReadOnly="true"></asp:TextBox>
                                <asp:TextBox ID="tbOwnerName" Width="250px" runat="server" Text='<%# Eval("Client1.Name") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Вкупен износ">
                            <ItemTemplate>
                                <asp:TextBox ID="tbCost" runat="server" Text='<%# Eval("Cost") %>' ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Дата">
                            <ItemTemplate>
                                <asp:TextBox ID="tbOfferDate" runat="server" Text='<%# Eval("OfferDate","{0:d}") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Изработена од">
                            <ItemTemplate>
                                <asp:TextBox ID="tbUserName" runat="server" Text='<%# Eval("User.UserName") %>' ReadOnly="true"></asp:TextBox>
                                <asp:TextBox ID="tbUserFullName" Width="250px" runat="server" Text='<%# Eval("User.Name") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Статус">
                            <ItemTemplate>
                                <asp:TextBox ID="tbStatus" runat="server" Text='<%# Eval("Statuse.Description") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Опис">
                            <ItemTemplate>
                                <asp:TextBox ID="tbDescription" Width="350px" runat="server" ReadOnly="true" Text='<%#Eval("Description") %>'></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Fields>
                </asp:DetailsView>
                <cc1:DetailsViewDataSource ID="odsOfferPreview" runat="server" TypeName="Broker.DataAccess.Offer"
                    ConflictDetection="CompareAllValues" DataObjectTypeName="Broker.DataAccess.Offer"
                    SelectMethod="Get" OldValuesParameterFormatString="oldEntity" OnSelecting="odsOfferPreview_Selecting">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="GXGridView1" Name="id" PropertyName="SelectedValue"
                            Type="Int32" />
                    </SelectParameters>
                </cc1:DetailsViewDataSource>
                <asp:MultiView ID="mvOfferItems" runat="server">
                    <asp:View ID="viewOfferItemsGrid" runat="server">
                        <asp:GridView ID="GridViewOfferItems" runat="server" DataSourceID="odsGridViewOfferItems"
                            DataKeyNames="ID" Width="100%" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"
                            Caption="Ставки на понуда" EmptyDataText="Нема записи кои го задоволуваат критериумот на пребарување!"
                            RowStyle-CssClass="row" CssClass="grid" GridLines="None" OnRowUpdating="GridViewOfferItems_RowUpdating"
                            OnRowDeleted="GridViewOfferItems_RowDeleted" OnRowUpdated="GridViewOfferItems_RowUpdated">
                            <RowStyle CssClass="row"></RowStyle>
                            <Columns>
                                <%--<asp:TemplateField Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="tbOfferItemID" runat="server" Text='<%#Bind("ID") %>' Visible="false"></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="tbOfferItemID" runat="server" Text='<%#Bind("ID") %>' Visible="false"></asp:Label>
                                    </EditItemTemplate>
                                </asp:TemplateField>--%>
                                <asp:TemplateField HeaderStyle-Width="0px">
                                    <EditItemTemplate>
                                        <%--<asp:Label ID="tbOfferNumber" runat="server" Text='<%#Eval("Offer.OfferNumber") %>'
                                            Width="0px" ReadOnly="true" Visible="false"></asp:Label>--%>
                                        <asp:Label Width="0px" ID="tbOfferNumberID" runat="server" Text='<%#Bind("OfferID") %>'
                                            Visible="false"></asp:Label>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <%-- <asp:Label ID="tbOfferNumber" runat="server" Text='<%#Eval("Offer.OfferNumber") %>'
                                            Width="0px" ReadOnly="true" Visible="false"></asp:Label>--%>
                                        <asp:Label ID="tbOfferNumberID" Width="0px" runat="server" Text='<%#Bind("OfferID") %>'
                                            Visible="false"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Класа">
                                    <EditItemTemplate>
                                        <asp:Label ID="tbInsuranceType" runat="server" Text='<%#Eval("InsuranceSubType.InsuranceType.Name") %>'
                                            ReadOnly="true" Width="100px"></asp:Label>
                                        <asp:Label ID="tbInsuranceTypeID" runat="server" Text='<%#Bind("InsuranceSubTypeID") %>'
                                            ReadOnly="true" Visible="false"></asp:Label>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="tbInsuranceType" runat="server" Text='<%#Eval("InsuranceSubType.InsuranceType.Name") %>'
                                            ReadOnly="true" Width="100px"></asp:Label>
                                        <asp:Label ID="tbInsuranceTypeID" runat="server" Text='<%#Bind("InsuranceSubTypeID") %>'
                                            ReadOnly="true" Visible="false"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Подкласа">
                                    <EditItemTemplate>
                                        <asp:Label ID="tbInsuranceSubType" runat="server" Text='<%#Eval("InsuranceSubType.Description") %>'
                                            ReadOnly="true" Width="100px"></asp:Label>
                                        <asp:Label ID="tbInsuranceSubTypeID" runat="server" Text='<%#Bind("InsuranceSubTypeID") %>'
                                            Visible="false"></asp:Label>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="tbInsuranceSubType" runat="server" Text='<%#Eval("InsuranceSubType.Description") %>'
                                            ReadOnly="true" Width="100px"></asp:Label>
                                        <asp:Label ID="tbInsuranceSubTypeID" runat="server" Text='<%#Bind("InsuranceSubTypeID") %>'
                                            Visible="false"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Осигурителна компанија">
                                    <EditItemTemplate>
                                        <asp:Label ID="tbInsuranceCompany" runat="server" Text='<%#Eval("InsuranceCompany.Name") %>'
                                            Width="100px" ReadOnly="true"></asp:Label>
                                        <asp:Label ID="tbInsuranceCompanyID" runat="server" Text='<%#Bind("InsuranceCompanyID") %>'
                                            Visible="false"></asp:Label>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="tbInsuranceCompany" runat="server" Text='<%#Eval("InsuranceCompany.Name") %>'
                                            Width="100px" ReadOnly="true"></asp:Label>
                                        <asp:Label ID="tbInsuranceCompanyID" runat="server" Text='<%#Bind("InsuranceCompanyID") %>'
                                            Visible="false"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Износ">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="tbCost" runat="server" Text='<%#Bind("Cost") %>' Width="50px"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvCost" runat="server" ControlToValidate="tbCost"
                                            Display="Dynamic" ErrorMessage="*"></asp:RequiredFieldValidator>
                                        <asp:CompareValidator ID="cvCost" runat="server" ControlToValidate="tbCost" Display="Dynamic"
                                            ErrorMessage="*" Operator="DataTypeCheck" Type="Double"></asp:CompareValidator>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="tbCost" runat="server" Text='<%#Bind("Cost") %>' ReadOnly="true" Width="50px"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:CommandField CancelText="Откажи" DeleteText="Избриши" EditText="Измени" InsertText="Внеси"
                                    InsertVisible="False" NewText="Нов" SelectText="Одбери" ShowDeleteButton="True"
                                    ShowEditButton="True" UpdateText="Ажурирај" />
                            </Columns>
                            <PagerStyle HorizontalAlign="Center" />
                            <%--<SelectedRowStyle CssClass="rowSelected" />--%>
                            <PagerSettings FirstPageText="<< Прва " PreviousPageText="< Претходна " NextPageText=" Следна >"
                                LastPageText=" Последна >>" Mode="NextPreviousFirstLast" />
                        </asp:GridView>
                        <asp:ObjectDataSource ID="odsGridViewOfferItems" runat="server" TypeName="Broker.DataAccess.OfferItem"
                            OldValuesParameterFormatString="oldEntity" DataObjectTypeName="Broker.DataAccess.OfferItem"
                            ConflictDetection="CompareAllValues" DeleteMethod="Delete" InsertMethod="Insert"
                            SelectMethod="GetByOfferID" UpdateMethod="UpdateCurrentCost" OnInserting="odsGridView_Inserting">
                            <UpdateParameters>
                                <asp:Parameter Name="oldEntity" Type="Object" />
                                <asp:Parameter Name="newEntity" Type="Object" />
                                <asp:Parameter Name="Cost" Type="Decimal" />
                                <asp:Parameter Name="InsuranceCompanyID" Type="Int32" />
                                <asp:Parameter Name="InsuranceSubTypeID" Type="Int32" />
                                <asp:Parameter Name="InsuranceTypeID" Type="Int32" />
                            </UpdateParameters>
                            <DeleteParameters>
                                <asp:Parameter Name="Cost" Type="Decimal" />
                                <asp:Parameter Name="InsuranceCompanyID" Type="Int32" />
                                <asp:Parameter Name="InsuranceSubTypeID" Type="Int32" />
                                <asp:Parameter Name="InsuranceTypeID" Type="Int32" />
                            </DeleteParameters>
                            <SelectParameters>
                                <asp:ControlParameter ControlID="GXGridView1" Name="offerID" PropertyName="SelectedValue"
                                    Type="Int32" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                        <div>
                            <asp:Button ID="btnNewOfferItem" runat="server" Text="Нова ставка" OnClick="btnNewOfferItem_Click" />
                        </div>
                    </asp:View>
                    <asp:View ID="viewOfferItemsEdit" runat="server">
                        <asp:DetailsView ID="DetailsViewOfferItems" runat="server" DataSourceID="DetailsViewDataSourceOfferItems"
                            DataKeyNames="ID" AutoGenerateRows="False" OnItemCommand="DetailsViewOfferItems_ItemCommand"
                            OnItemInserted="DetailsViewOfferItems_ItemInserted" OnModeChanging="DetailsViewOfferItems_ModeChanging"
                            OnItemInserting="DetailsViewOfferItems_ItemInserting" GridLines="None" DefaultMode="Insert">
                            <Fields>
                                <asp:TemplateField>
                                    <InsertItemTemplate>
                                        <div class="subTitles">
                                            Нова ставка
                                        </div>
                                    </InsertItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <InsertItemTemplate>
                                        <asp:TextBox ID="tbOfferItemID" runat="server" Text='<%#Bind("ID") %>' Visible="false"></asp:TextBox>
                                    </InsertItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <InsertItemTemplate>
                                        <table>
                                            <tr>
                                                <td style="width: 130px;">
                                                    <asp:Label ID="lblOrderNumber" runat="server" Text="Број на налог"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="tbOrderNumber" runat="server" ReadOnly="true"></asp:TextBox>
                                                    <%--<asp:Button ID="btnSearchOrderNumber" runat="server" Text="..." OnClick="btnSearchOrdernumber_Click"
                                            CausesValidation="false" />--%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 130px;">
                                                    <asp:Label ID="lblOrderItem" runat="server" Text="Ставка на налог"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlOrderItems" runat="server" AutoPostBack="True" DataSourceID="odsOrderItems"
                                                        DataTextField="OrdinalNumber" DataValueField="ID" OnSelectedIndexChanged="ddlOrderItems_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                    <asp:ObjectDataSource ID="odsOrderItems" runat="server" SelectMethod="GetByOrderNumber"
                                                        TypeName="Broker.DataAccess.OrderItem">
                                                        <SelectParameters>
                                                            <asp:ControlParameter ControlID="tbOrderNumber" Name="orderNumber" PropertyName="Text"
                                                                Type="String" />
                                                        </SelectParameters>
                                                    </asp:ObjectDataSource>
                                                </td>
                                            </tr>
                                        </table>
                                    </InsertItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <InsertItemTemplate>
                                        <table>
                                            <tr>
                                                <td style="width: 130px;">
                                                    Класа
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlInsuranceTypes" runat="server" DataSourceID="odsInsuranceTypes"
                                                        DataTextField="Name" Width="350px" DataValueField="ID" AutoPostBack="true" OnSelectedIndexChanged="ddlInsuranceTypes_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                    <asp:ObjectDataSource ID="odsInsuranceTypes" runat="server" SelectMethod="GetFromOrderItemID"
                                                        TypeName="Broker.DataAccess.InsuranceType">
                                                        <%--<SelectParameters>
                                                            <asp:ControlParameter ControlID="ddlInsuranceSubTypes" Name="insuranceSubTypeID"
                                                                PropertyName="SelectedValue" Type="Int32" />
                                                        </SelectParameters>--%>
                                                    </asp:ObjectDataSource>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 130px;">
                                                    Подкласа
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlInsuranceSubTypes" runat="server" DataSourceID="odsInsuranceSubTypes"
                                                        DataTextField="Description" DataValueField="ID" SelectedValue='<%# Bind("InsuranceSubTypeID") %>'
                                                        AutoPostBack="true" OnSelectedIndexChanged="ddlInsuranceSubTypes_SelectedIndexChanged"
                                                        Width="350px">
                                                    </asp:DropDownList>
                                                    <asp:ObjectDataSource ID="odsInsuranceSubTypes" runat="server" SelectMethod="GetByOrderItemID"
                                                        TypeName="Broker.DataAccess.InsuranceSubType">
                                                        <SelectParameters>
                                                            <asp:ControlParameter ControlID="ddlInsuranceTypes" Name="insuranceTypeID" PropertyName="SelectedValue"
                                                                Type="Int32" />
                                                        </SelectParameters>
                                                    </asp:ObjectDataSource>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 130px;">
                                                    Осиг. компанија
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlInsuranceCompanies" runat="server" DataSourceID="odsInsuranceCompanies"
                                                        Width="350px" DataTextField="Name" DataValueField="ID" SelectedValue='<%# Bind("InsuranceCompanyID") %>'
                                                        AutoPostBack="true" OnSelectedIndexChanged="ddlInsuranceCompanies_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                    <asp:ObjectDataSource ID="odsInsuranceCompanies" runat="server" SelectMethod="GetByInsuranceSubType"
                                                        TypeName="Broker.DataAccess.InsuranceCompany">
                                                        <SelectParameters>
                                                            <asp:ControlParameter ControlID="ddlInsuranceSubTypes" Name="insuranceSubTypeID"
                                                                PropertyName="SelectedValue" Type="Int32" />
                                                        </SelectParameters>
                                                    </asp:ObjectDataSource>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 130px;">
                                                    Договор
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlDeals" runat="server" DataSourceID="odsDeals" DataTextField="Description"
                                                        DataValueField="ID" OnSelectedIndexChanged="ddlDeals_SelectedIndexChanged" AutoPostBack="true"
                                                        Width="350px">
                                                    </asp:DropDownList>
                                                    <asp:ObjectDataSource ID="odsDeals" runat="server" SelectMethod="GetActiveDealsForCompanyAndInsuranceSubType"
                                                        TypeName="Broker.DataAccess.Deal">
                                                        <SelectParameters>
                                                            <asp:ControlParameter ControlID="ddlInsuranceCompanies" Name="InsuranceCompanyID"
                                                                PropertyName="SelectedValue" Type="Int32" />
                                                            <asp:ControlParameter ControlID="ddlInsuranceSubTypes" Name="InsuranceSubTypeID"
                                                                PropertyName="SelectedValue" Type="Int32" />
                                                        </SelectParameters>
                                                    </asp:ObjectDataSource>
                                                </td>
                                            </tr>
                                        </table>
                                    </InsertItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <EditItemTemplate>
                                        <table>
                                            <tr>
                                                <td style="width: 130px;">
                                                    <asp:Label ID="lblCost" Text="Износ" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="tbCost" runat="server" Text='<%#Bind("Cost") %>'></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvCost" runat="server" ControlToValidate="tbCost"
                                                        Display="Dynamic" ErrorMessage="*"></asp:RequiredFieldValidator>
                                                    <asp:CompareValidator ID="cvCost" runat="server" ControlToValidate="tbCost" Display="Dynamic"
                                                        ErrorMessage="*" Operator="DataTypeCheck" Type="Double"></asp:CompareValidator>
                                                </td>
                                            </tr>
                                        </table>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="tbCost" runat="server" Text='<%#Bind("Cost") %>' ReadOnly="true"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <InsertItemTemplate>
                                        <asp:Button ID="btnCancel" runat="server" CommandName="Cancel" Text="Откажи" CausesValidation="false" />
                                        <asp:Button ID="btnInsert" runat="server" CommandName="Insert" Text="Внеси" />
                                    </InsertItemTemplate>
                                </asp:TemplateField>
                            </Fields>
                        </asp:DetailsView>
                        <cc1:DetailsViewDataSource ID="DetailsViewDataSourceOfferItems" runat="server" TypeName="Broker.DataAccess.OfferItem"
                            ConflictDetection="CompareAllValues" DataObjectTypeName="Broker.DataAccess.OfferItem"
                            InsertMethod="Insert" SelectMethod="Get" OnInserted="DetailsViewDataSourceOfferItems_Inserted"
                            OnInserting="DetailsViewDataSourceOfferItems_Inserting">
                            <InsertParameters>
                                <asp:Parameter Name="entityToInsert" Type="Object" />
                            </InsertParameters>
                            <UpdateParameters>
                                <asp:Parameter Name="Cost" Type="Decimal" />
                            </UpdateParameters>
                            <SelectParameters>
                                <asp:ControlParameter ControlID="GridViewOfferItems" Name="id" PropertyName="SelectedValue"
                                    Type="Int32" />
                            </SelectParameters>
                        </cc1:DetailsViewDataSource>
                    </asp:View>
                </asp:MultiView>
            </div>
        </asp:View>
        <asp:View ID="viewAttachments" runat="server">
            <div class="paddingKontroli">
                <asp:DetailsView ID="dvOfferPreviewForAttachments" runat="server" DataSourceID="odsOfferPreviewForAttachments"
                    DataKeyNames="ID" AutoGenerateRows="False" OnItemCommand="dvOfferPreviewForAttachments_ItemCommand"
                    OnModeChanging="dvOfferPreviewForAttachments_ModeChanging" GridLines="None" DefaultMode="ReadOnly">
                    <Fields>
                        <asp:TemplateField HeaderText="Број на понуда">
                            <ItemTemplate>
                                <asp:TextBox ID="tbOfferNumber" runat="server" Text='<%# Bind("OfferNumber") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Договорувач">
                            <ItemTemplate>
                                <asp:TextBox ID="tbClientEMBG" runat="server" Text='<%# Eval("Client.EMBG") %>' ReadOnly="true"></asp:TextBox>
                                <asp:TextBox ID="tbClientName" Width="250px" runat="server" Text='<%# Eval("Client.Name") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Осигуреник">
                            <ItemTemplate>
                                <asp:TextBox ID="tbOwnerEMBG" runat="server" Text='<%# Eval("Client1.EMBG") %>' ReadOnly="true"></asp:TextBox>
                                <asp:TextBox ID="tbOwnerName" Width="250px" runat="server" Text='<%# Eval("Client1.Name") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Вкупен износ">
                            <ItemTemplate>
                                <asp:TextBox ID="tbCost" runat="server" Text='<%# Eval("Cost") %>' ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Дата">
                            <ItemTemplate>
                                <asp:TextBox ID="tbOfferDate" runat="server" Text='<%# Eval("OfferDate","{0:d}") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Изработена од">
                            <ItemTemplate>
                                <asp:TextBox ID="tbUserName" runat="server" Text='<%# Eval("User.UserName") %>' ReadOnly="true"></asp:TextBox>
                                <asp:TextBox ID="tbUserFullName" Width="250px" runat="server" Text='<%# Eval("User.Name") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Статус">
                            <ItemTemplate>
                                <asp:TextBox ID="tbStatus" runat="server" Text='<%# Eval("Statuse.Description") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Опис">
                            <ItemTemplate>
                                <asp:TextBox ID="tbDescription" Width="350px" runat="server" ReadOnly="true" Text='<%#Eval("Description") %>'></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Fields>
                </asp:DetailsView>
                <cc1:DetailsViewDataSource ID="odsOfferPreviewForAttachments" runat="server" TypeName="Broker.DataAccess.Offer"
                    ConflictDetection="CompareAllValues" DataObjectTypeName="Broker.DataAccess.Offer"
                    SelectMethod="Get" OldValuesParameterFormatString="oldEntity" OnSelecting="odsOfferPreviewForAttachments_Selecting">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="GXGridView1" Name="id" PropertyName="SelectedValue"
                            Type="Int32" />
                    </SelectParameters>
                </cc1:DetailsViewDataSource>
                <table>
                    <tr>
                        <td>
                            Датотека
                            <asp:FileUpload ID="FileUpload1" runat="server" />
                        </td>
                        <td>
                            <asp:Button ID="btnAddAttachment" runat="server" Text="Додади" OnClick="btnAddAttachment_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                </table>
                <div>
                    <asp:GridView ID="dataGridFiles" runat="server" AutoGenerateColumns="False" GridLines="None"
                        EmptyDataText="Нема записи!" HeaderStyle-BackColor="#2D3931" HeaderStyle-ForeColor="Wheat"
                        OnSelectedIndexChanged="dataGridFiles_SelectedIndexChanged">
                        <SelectedRowStyle CssClass="rowSelected" />
                        <Columns>
                            <asp:TemplateField HeaderText="Документ">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnFile" runat="server" OnClick="btnFile_Click" Text='<%#Bind("Name") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:CommandField ButtonType="Link" SelectText="Избриши" ShowSelectButton="True"
                                CancelText="Откажи" ShowCancelButton="true" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>
