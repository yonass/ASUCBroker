<%@ Page Title="Влезни фактури" Language="C#" MasterPageFile="~/MasterPage.master"
    AutoEventWireup="true" CodeFile="IncomingFactures.aspx.cs" Inherits="BROKERAdmin_IncomingFactures" %>

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
                Enabled="false" UseSubmitBehavior="false" CssClass="izbrisi" BIncomingFactureWidth="0px" />
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
            <asp:Button ID="btnAttachments" runat="server" ToolTip="Документи" OnClick="btnAttachments_Click"
                Enabled="false" CausesValidation="false" UseSubmitBehavior="false" CssClass="dokumenti"
                BorderWidth="0px" />
        </div>
    </div>
    <asp:MultiView ID="mvMain" runat="server">
        <asp:View ID="viewGrid" runat="server">
            <div id="tabeliFrame">
                <div id="header">
                    <div id="content">
                        Влезни фактури
                    </div>
                </div>
                <div id="contentOuter">
                    <div id="contentInner">
                        <cc1:GXGridView ID="GXGridView1" runat="server" DataSourceID="odsGridView" DataKeyNames="ID"
                            Width="100%" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"
                            Caption="Влезни фактури" EmptyDataText="Нема записи кои го задоволуваат критериумот на пребарување!"
                            OnRowCommand="GXGridView1_RowCommand" RowStyle-CssClass="row" CssClass="grid"
                            GridLines="None">
                            <Columns>
                                <asp:BoundField HeaderText="Број" DataField="FactureNumber" SortExpression="FactureNumber" />
                                <asp:BoundField HeaderText="Тип на фактура" DataField="IncomingFactureTypeName" SortExpression="IncomingFactureTypeName" />
                                <asp:BoundField HeaderText="Дата на фактура" DataField="FactureDate" SortExpression="FactureDate"
                                    DataFormatString="{0:d}" />
                                <asp:BoundField HeaderText="Дата на плаќање" DataField="ValuteDate" SortExpression="ValuteDate"
                                    DataFormatString="{0:d}" />
                                <asp:BoundField HeaderText="Коминтент" DataField="Name" SortExpression="Name" />
                                <%--<asp:BoundField HeaderText="Данок" DataField="TaxValue" SortExpression="TaxValue" />--%>
                                <asp:BoundField HeaderText="Вкупен износ" DataField="TotalCost" SortExpression="TotalCost"
                                    DataFormatString="{0:#,0.00}" ItemStyle-CssClass="currencyClass" />
                            </Columns>
                            <PagerStyle HorizontalAlign="Center" />
                            <SelectedRowStyle CssClass="rowSelected" />
                            <PagerSettings FirstPageText="<< Прва " PreviousPageText="< Претходна " NextPageText=" Следна >"
                                LastPageText=" Последна >>" Mode="NextPreviousFirstLast" />
                        </cc1:GXGridView>
                        <cc1:GridViewDataSource ID="odsGridView" runat="server" TypeName="Broker.DataAccess.ViewIncomingFacture"
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
                                        <cc1:FilterItem FieldName="Број" PropertyName="FactureNumber" Comparator="StringComparators"
                                            DataType="String" />
                                        <cc1:FilterItem FieldName="Тип на фактура" PropertyName="IncomingFactureTypeName"
                                            Comparator="StringComparators" DataType="String" />
                                        <cc1:FilterItem FieldName="Дата на фактура" PropertyName="FactureDate" Comparator="NumericComparators"
                                            DataType="DateTime" />
                                        <cc1:FilterItem FieldName="Дата на плаќање" PropertyName="ValuteDate" Comparator="NumericComparators"
                                            DataType="Decimal" />
                                        <cc1:FilterItem FieldName="ЕМБГ" PropertyName="EMBG" Comparator="StringComparators"
                                            DataType="String" />
                                        <cc1:FilterItem FieldName="Даночен број" PropertyName="TaxNumber" Comparator="StringComparators"
                                            DataType="String" />
                                        <cc1:FilterItem FieldName="Коминтент" PropertyName="Name" Comparator="StringComparators"
                                            DataType="String" />
                                        <cc1:FilterItem FieldName="Адреса" PropertyName="Address" Comparator="StringComparators"
                                            DataType="String" />
                                        <cc1:FilterItem FieldName="Данок" PropertyName="TaxValue" Comparator="NumericComparators"
                                            DataType="Decimal" />
                                        <cc1:FilterItem FieldName="Износ" PropertyName="TotalCost" Comparator="NumericComparators"
                                            DataType="Decimal" />
                                    </cc1:FilterControl>
                                </div>
                                <cc1:FilterDataSource ID="odsFilterGridView" runat="server" TypeName="Broker.DataAccess.ViewIncomingFacture">
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
                    GridLines="None" OnDataBinding="DetailsView1_DataBinding" OnDataBound="DetailsView1_DataBound">
                    <Fields>
                        <asp:TemplateField>
                            <InsertItemTemplate>
                                <div class="subTitles">
                                    Нов запис во Влезни фактури
                                </div>
                            </InsertItemTemplate>
                            <EditItemTemplate>
                                <div class="subTitles">
                                    Измена на запис во Влезни фактури
                                </div>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <div class="subTitles">
                                    Бришење на запис од Влезни фактури
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Број на фактура">
                            <InsertItemTemplate>
                                <asp:TextBox ID="tbFactureNumber" runat="server" Text='<%# Bind("FactureNumber") %>'
                                    MaxLength="50"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvFactureNumber" runat="server" ControlToValidate="tbFactureNumber"
                                    Display="Dynamic" ErrorMessage="*">
                                </asp:RequiredFieldValidator>
                            </InsertItemTemplate>
                            <ItemTemplate>
                                <asp:TextBox ID="tbFactureNumber" runat="server" Text='<%# Bind("FactureNumber") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="tbFactureNumber" MaxLength="50" runat="server" Text='<%# Bind("FactureNumber") %>'></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvFactureNumber" runat="server" ControlToValidate="tbFactureNumber"
                                    Display="Dynamic" ErrorMessage="*">
                                </asp:RequiredFieldValidator>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Тип на фактура">
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddlIncomingFactureTypes" runat="server" Enabled="false" DataSourceID="odsIncomingFactureTypes"
                                    DataTextField="Name" DataValueField="ID" SelectedValue='<%# Bind("IncomingFactureTypeID") %>'
                                    OnSelectedIndexChanged="ddlIncomingFactureTypes_SelectedIndexChanged">
                                </asp:DropDownList>
                                <asp:ObjectDataSource ID="odsIncomingFactureTypes" runat="server" SelectMethod="Select"
                                    TypeName="Broker.DataAccess.IncomingFactureType"></asp:ObjectDataSource>
                            </EditItemTemplate>
                            <InsertItemTemplate>
                                <asp:DropDownList ID="ddlIncomingFactureTypes" runat="server" DataSourceID="odsIncomingFactureTypes"
                                    DataTextField="Name" DataValueField="ID" SelectedValue='<%# Bind("IncomingFactureTypeID") %>'
                                    AutoPostBack="True" OnSelectedIndexChanged="ddlIncomingFactureTypes_SelectedIndexChanged">
                                </asp:DropDownList>
                                <asp:ObjectDataSource ID="odsIncomingFactureTypes" runat="server" SelectMethod="Select"
                                    TypeName="Broker.DataAccess.IncomingFactureType"></asp:ObjectDataSource>
                            </InsertItemTemplate>
                            <ItemTemplate>
                                <asp:DropDownList ID="ddlIncomingFactureTypes" runat="server" DataSourceID="odsIncomingFactureTypes"
                                    DataTextField="Name" DataValueField="ID" Enabled="false" SelectedValue='<%# Bind("IncomingFactureTypeID") %>'
                                    OnSelectedIndexChanged="ddlIncomingFactureTypes_SelectedIndexChanged">
                                </asp:DropDownList>
                                <asp:ObjectDataSource ID="odsIncomingFactureTypes" runat="server" SelectMethod="Select"
                                    TypeName="Broker.DataAccess.IncomingFactureType"></asp:ObjectDataSource>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Осигурителна компанија">
                            <InsertItemTemplate>
                                <asp:DropDownList ID="ddlInsuranceCompanies" runat="server" AutoPostBack="true" DataSourceID="odsInsuranceCompanies"
                                    SelectedValue='<%# Bind("InsuranceCompanyID") %>' DataTextField="Name" DataValueField="ID"
                                    OnSelectedIndexChanged="ddlInsuranceCompanies_SelectedIndexChanged">
                                </asp:DropDownList>
                                <asp:ObjectDataSource ID="odsInsuranceCompanies" runat="server" SelectMethod="GetActiveInsuranceCompanies"
                                    TypeName="Broker.DataAccess.InsuranceCompany"></asp:ObjectDataSource>
                            </InsertItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddlInsuranceCompanies" runat="server" AutoPostBack="true" DataSourceID="odsInsuranceCompanies"
                                    SelectedValue='<%# Bind("InsuranceCompanyID") %>' DataTextField="Name" DataValueField="ID"
                                    OnSelectedIndexChanged="ddlInsuranceCompanies_SelectedIndexChanged">
                                </asp:DropDownList>
                                <asp:ObjectDataSource ID="odsInsuranceCompanies" runat="server" SelectMethod="GetActiveInsuranceCompanies"
                                    TypeName="Broker.DataAccess.InsuranceCompany"></asp:ObjectDataSource>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Дата на фактура">
                            <EditItemTemplate>
                                <asp:TextBox runat="server" ID="tbFactureDate" Text='<%#Bind("FactureDate", "{0:d}") %>'></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvFactureDate" runat="server" ControlToValidate="tbFactureDate"
                                    Display="Dynamic" ErrorMessage="*"></asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="cvFactureDate" runat="server" ControlToValidate="tbFactureDate"
                                    Display="Dynamic" ErrorMessage="(не е дата)" Operator="DataTypeCheck" Type="Date"></asp:CompareValidator>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="tbFactureDate" Text='<%#Bind("FactureDate", "{0:d}") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                            <InsertItemTemplate>
                                <asp:TextBox runat="server" ID="tbFactureDate" Text='<%#Bind("FactureDate", "{0:d}") %>'></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvFactureDate" runat="server" ControlToValidate="tbFactureDate"
                                    Display="Dynamic" ErrorMessage="*"></asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="cvFactureDate" runat="server" ControlToValidate="tbFactureDate"
                                    Display="Dynamic" ErrorMessage="(не е дата)" Operator="DataTypeCheck" Type="Date"></asp:CompareValidator>
                            </InsertItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Дата на плаќање">
                            <EditItemTemplate>
                                <asp:TextBox runat="server" ID="tbValuteDate" Text='<%#Bind("ValuteDate", "{0:d}") %>'></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvValuteDate" runat="server" ControlToValidate="tbValuteDate"
                                    Display="Dynamic" ErrorMessage="*"></asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="cvValuteDate" runat="server" ControlToValidate="tbValuteDate"
                                    Display="Dynamic" ErrorMessage="(не е дата)" Operator="DataTypeCheck" Type="Date"></asp:CompareValidator>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="tbValuteDate" Text='<%#Bind("ValuteDate", "{0:d}") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                            <InsertItemTemplate>
                                <asp:TextBox runat="server" ID="tbValuteDate" Text='<%#Bind("ValuteDate", "{0:d}") %>'></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvValuteDate" runat="server" ControlToValidate="tbValuteDate"
                                    Display="Dynamic" ErrorMessage="*"></asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="cvValuteDate" runat="server" ControlToValidate="tbValuteDate"
                                    Display="Dynamic" ErrorMessage="(не е дата)" Operator="DataTypeCheck" Type="Date"></asp:CompareValidator>
                            </InsertItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="ЕМБГ">
                            <EditItemTemplate>
                                <asp:TextBox runat="server" ID="tbEMBG" Text='<%#Bind("EMBG") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="tbEMBG" Text='<%#Bind("EMBG") %>' ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                            <InsertItemTemplate>
                                <asp:TextBox runat="server" ID="tbEMBG" Text='<%#Bind("EMBG") %>'></asp:TextBox>
                            </InsertItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Даночен број">
                            <EditItemTemplate>
                                <asp:TextBox runat="server" ID="tbTaxNumber" Text='<%#Bind("TaxNumber") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="tbTaxNumber" Text='<%#Bind("TaxNumber") %>' ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                            <InsertItemTemplate>
                                <asp:TextBox runat="server" ID="tbTaxNumber" Text='<%#Bind("TaxNumber") %>'></asp:TextBox>
                            </InsertItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Коминтент">
                            <EditItemTemplate>
                                <asp:TextBox runat="server" ID="tbName" Text='<%#Bind("Name") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="tbName" Text='<%#Bind("Name") %>' ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                            <InsertItemTemplate>
                                <asp:TextBox runat="server" ID="tbName" Text='<%#Bind("Name") %>'></asp:TextBox>
                            </InsertItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Адреса">
                            <EditItemTemplate>
                                <asp:TextBox runat="server" ID="tbAddress" Text='<%#Bind("Address") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="tbAddress" Text='<%#Bind("Address") %>' ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                            <InsertItemTemplate>
                                <asp:TextBox runat="server" ID="tbAddress" Text='<%#Bind("Address") %>'></asp:TextBox>
                            </InsertItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Место">
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddlPlaces" runat="server" Enabled="true" DataSourceID="odsPlaces"
                                    DataTextField="Name" DataValueField="ID" SelectedValue='<%# Bind("PlaceID") %>'>
                                </asp:DropDownList>
                                <asp:ObjectDataSource ID="odsPlaces" runat="server" SelectMethod="Select" TypeName="Broker.DataAccess.Place">
                                </asp:ObjectDataSource>
                            </EditItemTemplate>
                            <InsertItemTemplate>
                                <asp:DropDownList ID="ddlPlaces" runat="server" Enabled="true" DataSourceID="odsPlaces"
                                    DataTextField="Name" DataValueField="ID" SelectedValue='<%# Bind("PlaceID") %>'>
                                </asp:DropDownList>
                                <asp:ObjectDataSource ID="odsPlaces" runat="server" SelectMethod="Select" TypeName="Broker.DataAccess.Place">
                                </asp:ObjectDataSource>
                            </InsertItemTemplate>
                            <ItemTemplate>
                                <asp:DropDownList ID="ddlPlaces" runat="server" Enabled="true" DataSourceID="odsPlaces"
                                    DataTextField="Name" DataValueField="ID" SelectedValue='<%# Bind("PlaceID") %>'>
                                </asp:DropDownList>
                                <asp:ObjectDataSource ID="odsPlaces" runat="server" SelectMethod="Select" TypeName="Broker.DataAccess.Place">
                                </asp:ObjectDataSource>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Финансиски книжена">
                            <EditItemTemplate>
                                <asp:CheckBox runat="server" ID="cbIsAccountBooked" Checked='<%#Bind("IsAccountBooked") %>'
                                    Enabled="false" />
                                <asp:TextBox ID="tbUserID" runat="server" Text='<%#Bind("UserID") %>' Width="0px"
                                    ForeColor="Transparent" BorderColor="Transparent" BackColor="Transparent"></asp:TextBox>
                                <asp:TextBox ID="tbBranchID" runat="server" Text='<%#Bind("BranchID") %>' Width="0px"
                                    ForeColor="Transparent" BorderColor="Transparent" BackColor="Transparent"></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:CheckBox runat="server" ID="cbIsAccountBooked" Checked='<%#Bind("IsAccountBooked") %>'
                                    Enabled="false" />
                                <asp:TextBox ID="tbUserID" runat="server" Text='<%#Bind("UserID") %>' Width="0px"
                                    ForeColor="Transparent" BorderColor="Transparent" BackColor="Transparent"></asp:TextBox>
                                <asp:TextBox ID="tbBranchID" runat="server" Text='<%#Bind("BranchID") %>' Width="0px"
                                    ForeColor="Transparent" BorderColor="Transparent" BackColor="Transparent"></asp:TextBox>
                            </ItemTemplate>
                            <InsertItemTemplate>
                                <asp:CheckBox runat="server" ID="cbIsAccountBooked" Checked='<%#Bind("IsAccountBooked") %>'
                                    Enabled="false" />
                            </InsertItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Данок">
                            <EditItemTemplate>
                                <asp:TextBox runat="server" ID="tbTaxValue" Text='<%#Bind("TaxValue") %>'></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvTaxValue" runat="server" ControlToValidate="tbTaxValue"
                                    Display="Dynamic" ErrorMessage="*">
                                </asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="cvTaxValue" runat="server" ControlToValidate="tbTaxValue"
                                    Display="Dynamic" ErrorMessage="(не е децимален)" Operator="DataTypeCheck" Type="Double"></asp:CompareValidator>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="tbTaxValue" Text='<%#Bind("TaxValue") %>' ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                            <InsertItemTemplate>
                                <asp:TextBox runat="server" ID="tbTaxValue" Text='<%#Bind("TaxValue") %>'></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvTaxValue" runat="server" ControlToValidate="tbTaxValue"
                                    Display="Dynamic" ErrorMessage="*">
                                </asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="cvTaxValue" runat="server" ControlToValidate="tbTaxValue"
                                    Display="Dynamic" ErrorMessage="(не е децимален)" Operator="DataTypeCheck" Type="Double"></asp:CompareValidator>
                            </InsertItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Вкупен износ">
                            <EditItemTemplate>
                                <asp:TextBox runat="server" ID="tbTotalCost" Text='<%#Bind("TotalCost") %>'></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvTotalCost" runat="server" ControlToValidate="tbTotalCost"
                                    Display="Dynamic" ErrorMessage="*">
                                </asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="cvTotalCost" runat="server" ControlToValidate="tbTotalCost"
                                    Display="Dynamic" ErrorMessage="(не е децимален)" Operator="DataTypeCheck" Type="Double"></asp:CompareValidator>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="tbTotalCost" Text='<%#Bind("TotalCost") %>' ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                            <InsertItemTemplate>
                                <asp:TextBox runat="server" ID="tbTotalCost" Text='<%#Bind("TotalCost") %>'></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvTotalCost" runat="server" ControlToValidate="tbTotalCost"
                                    Display="Dynamic" ErrorMessage="*">
                                </asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="cvTotalCost" runat="server" ControlToValidate="tbTotalCost"
                                    Display="Dynamic" ErrorMessage="(не е децимален)" Operator="DataTypeCheck" Type="Double"></asp:CompareValidator>
                            </InsertItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <InsertItemTemplate>
                                <asp:GridView ID="gvNewPolicies" runat="server" AllowPaging="True" AllowSorting="True"
                                    AutoGenerateColumns="False" Caption="Полиси од селектираниот период" EmptyDataText="Нема записи кои го задоволуваат критериумот на пребарување!"
                                    RowStyle-CssClass="row" CssClass="grid" GridLines="None" OnPageIndexChanging="gvNewPolicies_PageIndexChanging"
                                    DataKeyNames="ID" PageSize="10">
                                    <RowStyle CssClass="row"></RowStyle>
                                    <Columns>
                                        <asp:BoundField HeaderText="" DataField="ID" />
                                        <asp:BoundField HeaderText="Број на полиса" DataField="PolicyNumber" SortExpression="PolicyNumber" />
                                        <asp:TemplateField HeaderText="За фактурирање">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="cbIsForFacturing" runat="server" Checked='<%#Bind("IsForFacturing") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerStyle HorizontalAlign="Center" />
                                    <PagerSettings FirstPageText="<< Прва " PreviousPageText="< Претходна " NextPageText=" Следна >"
                                        LastPageText=" Последна >>" Mode="NextPreviousFirstLast" />
                                </asp:GridView>
                            </InsertItemTemplate>
                            <EditItemTemplate>
                                <asp:GridView ID="gvNewPolicies" runat="server" AllowPaging="True" AllowSorting="True"
                                    AutoGenerateColumns="False" Caption="Полиси од осигурителната компанија" EmptyDataText="Нема записи!"
                                    RowStyle-CssClass="row" CssClass="grid" GridLines="None" OnPageIndexChanging="gvNewPolicies_PageIndexChanging"
                                    DataKeyNames="ID" PageSize="10">
                                    <RowStyle CssClass="row"></RowStyle>
                                    <Columns>
                                        <asp:BoundField HeaderText="" DataField="ID" ItemStyle-ForeColor="Transparent" ItemStyle-Width="0px" />
                                        <asp:BoundField HeaderText="Број на полиса" DataField="PolicyNumber" SortExpression="PolicyNumber" />
                                        <asp:TemplateField HeaderText="За фактурирање">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="cbIsForFacturing" runat="server" Checked='<%#Bind("IsForFacturing") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerStyle HorizontalAlign="Center" />
                                    <PagerSettings FirstPageText="<< Прва " PreviousPageText="< Претходна " NextPageText=" Следна >"
                                        LastPageText=" Последна >>" Mode="NextPreviousFirstLast" />
                                </asp:GridView>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:GridView ID="gvNewPolicies" runat="server" AllowPaging="True" AllowSorting="True"
                                    AutoGenerateColumns="False" Caption="Полиси од осигурителната компанија" EmptyDataText="Нема записи!"
                                    RowStyle-CssClass="row" CssClass="grid" GridLines="None" OnPageIndexChanging="gvNewPolicies_PageIndexChanging"
                                    DataKeyNames="ID" PageSize="10">
                                    <RowStyle CssClass="row"></RowStyle>
                                    <Columns>
                                        <asp:BoundField HeaderText="" DataField="ID" ItemStyle-ForeColor="Transparent" ItemStyle-Width="0px" />
                                        <asp:BoundField HeaderText="Број на полиса" DataField="PolicyNumber" SortExpression="PolicyNumber" />
                                        <asp:TemplateField HeaderText="За фактурирање">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="cbIsForFacturing" runat="server" Checked='<%#Bind("IsForFacturing") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerStyle HorizontalAlign="Center" />
                                    <PagerSettings FirstPageText="<< Прва " PreviousPageText="< Претходна " NextPageText=" Следна >"
                                        LastPageText=" Последна >>" Mode="NextPreviousFirstLast" />
                                </asp:GridView>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <InsertItemTemplate>
                                <asp:TextBox ID="tbPolicyNumberToList" runat="server"></asp:TextBox>
                                <asp:Button ID="btnAddPolicyToList" runat="server" Text="Додади" OnClick="btnAddPolicyToList_Click" />
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
                <cc1:DetailsViewDataSource ID="dvDataSource" runat="server" TypeName="Broker.DataAccess.IncomingFacture"
                    ConflictDetection="CompareAllValues" DataObjectTypeName="Broker.DataAccess.IncomingFacture"
                    DeleteMethod="Delete" InsertMethod="Insert" SelectMethod="Get" UpdateMethod="Update"
                    OnUpdating="dvDataSource_Updating" OnUpdated="dvDataSource_Updated" OnInserted="dvDataSource_Inserted"
                    OnInserting="dvDataSource_Inserting" OldValuesParameterFormatString="oldEntity"
                    OnSelecting="dvDataSource_Selecting" OnDataBinding="dvDataSource_DataBinding"
                    OnSelected="dvDataSource_Selected">
                    <UpdateParameters>
                        <asp:Parameter Name="oldEntity" Type="Object" />
                        <asp:Parameter Name="newEntity" Type="Object" />
                        <asp:Parameter Name="FactureDate" Type="DateTime" />
                        <asp:Parameter Name="ValuteDate" Type="DateTime" />
                        <asp:Parameter Name="TotalCost" Type="Decimal" />
                        <asp:Parameter Name="TaxValue" Type="Decimal" />
                        <asp:Parameter Name="Address" Type="String" ConvertEmptyStringToNull="false" DefaultValue="" />
                        <asp:Parameter Name="Name" Type="String" ConvertEmptyStringToNull="false" DefaultValue="" />
                        <asp:Parameter Name="TaxNumber" Type="String" ConvertEmptyStringToNull="false" DefaultValue="" />
                        <asp:Parameter Name="EMBG" Type="String" ConvertEmptyStringToNull="false" DefaultValue="" />
                    </UpdateParameters>
                    <InsertParameters>
                        <asp:Parameter Name="FactureDate" Type="DateTime" />
                        <asp:Parameter Name="ValuteDate" Type="DateTime" />
                        <asp:Parameter Name="TotalCost" Type="Decimal" />
                        <asp:Parameter Name="TaxValue" Type="Decimal" />
                        <asp:Parameter Name="Address" Type="String" ConvertEmptyStringToNull="false" DefaultValue="" />
                        <asp:Parameter Name="Name" Type="String" ConvertEmptyStringToNull="false" DefaultValue="" />
                        <asp:Parameter Name="TaxNumber" Type="String" ConvertEmptyStringToNull="false" DefaultValue="" />
                        <asp:Parameter Name="EMBG" Type="String" ConvertEmptyStringToNull="false" DefaultValue="" />
                    </InsertParameters>
                    <DeleteParameters>
                        <asp:Parameter Name="FactureDate" Type="DateTime" />
                        <asp:Parameter Name="ValuteDate" Type="DateTime" />
                        <asp:Parameter Name="TotalCost" Type="Decimal" />
                        <asp:Parameter Name="TaxValue" Type="Decimal" />
                        <asp:Parameter Name="Address" Type="String" ConvertEmptyStringToNull="false" DefaultValue="" />
                        <asp:Parameter Name="Name" Type="String" ConvertEmptyStringToNull="false" DefaultValue="" />
                        <asp:Parameter Name="TaxNumber" Type="String" ConvertEmptyStringToNull="false" DefaultValue="" />
                        <asp:Parameter Name="EMBG" Type="String" ConvertEmptyStringToNull="false" DefaultValue="" />
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
                    <cc1:SearchItem FieldName="Број" PropertyName="FactureNumber" Comparator="StringComparators"
                        DataType="String" />
                    <cc1:SearchItem FieldName="Тип на фактура" PropertyName="IncomingFactureTypeName"
                        Comparator="StringComparators" DataType="String" />
                    <cc1:SearchItem FieldName="Дата на фактура" PropertyName="FactureDate" Comparator="NumericComparators"
                        DataType="DateTime" />
                    <cc1:SearchItem FieldName="Дата на плаќање" PropertyName="ValuteDate" Comparator="NumericComparators"
                        DataType="Decimal" />
                    <cc1:SearchItem FieldName="ЕМБГ" PropertyName="EMBG" Comparator="StringComparators"
                        DataType="String" />
                    <cc1:SearchItem FieldName="Даночен број" PropertyName="TaxNumber" Comparator="StringComparators"
                        DataType="String" />
                    <cc1:SearchItem FieldName="Коминтент" PropertyName="Name" Comparator="StringComparators"
                        DataType="String" />
                    <cc1:SearchItem FieldName="Адреса" PropertyName="Address" Comparator="StringComparators"
                        DataType="String" />
                    <cc1:SearchItem FieldName="Данок" PropertyName="TaxValue" Comparator="NumericComparators"
                        DataType="Decimal" />
                    <cc1:SearchItem FieldName="Износ" PropertyName="TotalCost" Comparator="NumericComparators"
                        DataType="Decimal" />
                </cc1:SearchControl>
                <cc1:GridViewDataSource ID="odsSearch" runat="server" TypeName="Broker.DataAccess.ViewIncomingFacture"
                    SelectCountMethod="SelectSearchCountCached" SelectMethod="SelectSearch">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="SearchControl1" PropertyName="SearchArguments" Name="sArgument" />
                    </SelectParameters>
                </cc1:GridViewDataSource>
            </div>
        </asp:View>
        <asp:View ID="viewReport" runat="server">
            <div class="paddingKontroli">
                <cc1:ReportControl ID="reportControl" runat="server" GridViewID="GXGridView1" SearchControlID="SearchControl1"
                    TypeName="Broker.DataAccess.ViewIncomingFacture">
                    <cc1:PrintItem HeaderText="Број" PropertyName="FactureNumber" />
                    <cc1:PrintItem HeaderText="Тип на фактура" PropertyName="IncomingFactureTypeName" />
                    <cc1:PrintItem HeaderText="Дата на фактура" PropertyName="FactureDate" />
                    <cc1:PrintItem HeaderText="Дата на плаќање" PropertyName="ValuteDate" />
                    <cc1:PrintItem HeaderText="ЕМБГ" PropertyName="EMBG" />
                    <cc1:PrintItem HeaderText="Даночен број" PropertyName="TaxNumber" />
                    <cc1:PrintItem HeaderText="Коминтент" PropertyName="Name" />
                    <cc1:PrintItem HeaderText="Адреса" PropertyName="Address" />
                    <cc1:PrintItem HeaderText="Данок" PropertyName="TaxValue" />
                    <cc1:PrintItem HeaderText="Износ" PropertyName="TotalCost" />
                </cc1:ReportControl>
            </div>
        </asp:View>
        <asp:View ID="viewAttachments" runat="server">
            <div class="paddingKontroli">
                <asp:DetailsView ID="DetailsViewIncomingFactureForAttachments" runat="server" DataSourceID="odsIncomingFactureForAttachments"
                    DataKeyNames="ID" AutoGenerateRows="False" OnItemCommand="DetailsViewIncomingFactureForAttachments_ItemCommand"
                    OnModeChanging="DetailsViewIncomingFactureForAttachments_ModeChanging" GridLines="None"
                    DefaultMode="ReadOnly">
                    <Fields>
                        <asp:TemplateField HeaderText="Број на фактура">
                            <ItemTemplate>
                                <asp:TextBox ID="tbIncomingFactureNumber" runat="server" Text='<%# Bind("FactureNumber") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Дата на фактура">
                            <ItemTemplate>
                                <asp:TextBox ID="tbIncomingFactureDate" runat="server" Text='<%# Bind("FactureDate", "{0:d}") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Fields>
                </asp:DetailsView>
                <cc1:DetailsViewDataSource ID="odsIncomingFactureForAttachments" runat="server" TypeName="Broker.DataAccess.IncomingFacture"
                    ConflictDetection="CompareAllValues" DataObjectTypeName="Broker.DataAccess.IncomingFacture"
                    SelectMethod="Get" OldValuesParameterFormatString="oldEntity" OnSelecting="odsIncomingFactureForAttachments_Selecting">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="GXGridView1" Name="id" PropertyName="SelectedValue"
                            Type="Int32" />
                    </SelectParameters>
                </cc1:DetailsViewDataSource>
                <table>
                    <tr>
                        <td>
                            Датотека<asp:FileUpload ID="FileUpload1" runat="server" />
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
