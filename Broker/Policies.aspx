<%@ Page Title="Преглед на полиси" Language="C#" MasterPageFile="~/MasterPage.master"
    AutoEventWireup="true" CodeFile="Policies.aspx.cs" Inherits="Broker_Policies"
    Culture="mk-MK" UICulture="mk-MK" %>

<%@ Register Namespace="Superexpert.Controls" TagPrefix="super" %>
<%@ Register Src="~/UserControls/ExtendContols/ExtendControl.ascx" TagName="extendcontrol"
    TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="kontroli">
        <%-- <div id="button01">
            <asp:Button ID="btnNew" runat="server" ToolTip="Нов запис" OnClick="btnNew_Click"
                CausesValidation="false" UseSubmitBehavior="false" CssClass="novZapis" BorderWidth="0px" />
        </div>--%>
        <%-- <div id="button01">
            <asp:Button ID="btnEdit" runat="server" ToolTip="Измени" OnClick="btnEdit_Click"
                CausesValidation="false" Enabled="false" UseSubmitBehavior="false" CssClass="izmeni"
                BorderWidth="0px" />
        </div>--%>
        <%-- <div id="button03">
            <asp:Button ID="btnDelete" runat="server" OnClick="btnDelete_Click" CausesValidation="false"
                Enabled="false" UseSubmitBehavior="false" CssClass="izbrisi" BorderWidth="0px" />
        </div>--%>
        <div id="button01">
            <asp:Button ID="btnPreviewPolicy" runat="server" ToolTip="Прикажи" OnClick="btnPreviewPolicy_Click"
                CausesValidation="false" UseSubmitBehavior="false" CssClass="prikaz" BorderWidth="0px"
                Enabled="False" />
        </div>
        <div id="button02">
            <asp:Button ID="btnPreview" runat="server" ToolTip="Освежи" OnClick="btnPreview_Click"
                CausesValidation="false" UseSubmitBehavior="false" CssClass="osvezi" BorderWidth="0px" />
        </div>
        <div id="button03">
            <asp:Button ID="btnReport" runat="server" ToolTip="Извештај" CausesValidation="false"
                UseSubmitBehavior="false" OnClick="btnReport_Click" CssClass="izvestaj" BorderWidth="0px" />
        </div>
        <div id="button04">
            <asp:Button ID="btnSearch" runat="server" ToolTip="Пребарај" OnClick="btnSearch_Click"
                CausesValidation="false" UseSubmitBehavior="false" CssClass="prebaraj" BorderWidth="0px" />
        </div>
        <div id="button05">
            <asp:Button ID="btnAttachments" runat="server" ToolTip="Документи" OnClick="btnAttachments_Click"
                CausesValidation="false" UseSubmitBehavior="false" CssClass="dokumenti" BorderWidth="0px" />
        </div>
        <div id="button06">
            <asp:Button ID="btnDiscard" runat="server" ToolTip="Сторнирај" OnClick="btnDiscard_Click"
                Enabled="false" CausesValidation="false" UseSubmitBehavior="false" CssClass="storniraj"
                BorderWidth="0px" />
        </div>
    </div>
    <asp:MultiView ID="mvMain" runat="server">
        <asp:View ID="viewGrid" runat="server">
            <div id="tabeliFrame">
                <div id="header">
                    <div id="content">
                        <asp:Label ID="lblPageName" runat="server" Text="Полиси"></asp:Label>
                    </div>
                </div>
                <div id="contentOuter">
                    <div id="contentInner">
                        <cc1:GXGridView ID="GXGridView1" runat="server" DataSourceID="odsGridView" DataKeyNames="ID"
                            Width="100%" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"
                            Caption="Полиси" EmptyDataText="Нема записи кои го задоволуваат критериумот на пребарување!"
                            OnRowCommand="GXGridView1_RowCommand" RowStyle-CssClass="row" CssClass="grid"
                            GridLines="None">
                            <Columns>
                                <asp:BoundField HeaderText="Број на полиса" DataField="PolicyNumber" SortExpression="PolicyNumber" />
                                <%--<asp:BoundField HeaderText="Класа" DataField="InsuranceTypeShortName" SortExpression="InsuranceTypeShortName" />--%>
                                <asp:BoundField HeaderText="Дата" DataFormatString="{0:d}" DataField="ApplicationDate"
                                    SortExpression="ApplicationDate" />
                                <asp:BoundField HeaderText="Подкласа" DataField="InsuranceSubTypesShortDescription"
                                    SortExpression="InsuranceSubTypesShortDescription" />
                                <%--<asp:BoundField HeaderText="МБ. Договорувач" DataField="ClientEmbg" SortExpression="ClientEMBG" />--%>
                                <asp:BoundField HeaderText="Договорувач" DataField="ClientName" SortExpression="ClientName" />
                                <%--<asp:BoundField HeaderText="МБ. Осигуреник" DataField="OwnerEMBG" SortExpression="OwnerEMBG" />--%>
                                <%--<asp:BoundField HeaderText="Осигуреник" DataField="OwnerName" SortExpression="OwnerName" />--%>
                                <asp:BoundField HeaderText="Ос. компанија" DataField="CompanyShortName" SortExpression="CompanyShortName" />
                                <asp:BoundField HeaderText="Премија" DataField="PremiumValue" SortExpression="PremiumValue"
                                    DataFormatString="{0:#,0.00}" ItemStyle-CssClass="currencyClass" />
                                <asp:BoundField HeaderText="Должи" DataField="DebtValue" SortExpression="DebtValue"
                                    DataFormatString="{0:#,0.00}" ItemStyle-CssClass="currencyClass" />
                                <asp:CheckBoxField HeaderText="Сто- рно" DataField="Discard" SortExpression="Discard" />
                            </Columns>
                            <PagerStyle HorizontalAlign="Center" />
                            <SelectedRowStyle CssClass="rowSelected" />
                            <PagerSettings FirstPageText="<< Прва " PreviousPageText="< Претходна " NextPageText=" Следна >"
                                LastPageText=" Последна >>" Mode="NextPreviousFirstLast" />
                        </cc1:GXGridView>
                        <cc1:GridViewDataSource ID="odsGridView" runat="server" TypeName="Broker.DataAccess.PoliciesView"
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
                                        <cc1:FilterItem FieldName="Број на полиса" PropertyName="PolicyNumber" Comparator="StringComparators"
                                            DataType="String" />
                                        <cc1:FilterItem FieldName="Класа" PropertyName="InsuranceTypeShortName" Comparator="StringComparators"
                                            DataType="String" />
                                        <cc1:FilterItem FieldName="Подкласа" PropertyName="InsuranceSubTypesShortDescription"
                                            Comparator="StringComparators" DataType="String" />
                                        <cc1:FilterItem FieldName="Класа (шифра)" PropertyName="InsuranceTypeCode" Comparator="StringComparators"
                                            DataType="String" />
                                        <cc1:FilterItem FieldName="Подкласа (шифра)" PropertyName="InsuranceSubTypeCode"
                                            Comparator="StringComparators" DataType="String" />
                                        <cc1:FilterItem FieldName="Осигурителна компанија" PropertyName="CompanyShortName"
                                            Comparator="StringComparators" DataType="String" />
                                        <cc1:FilterItem FieldName="МБ. Договорувач" PropertyName="ClientEMBG" Comparator="StringComparators"
                                            DataType="String" />
                                        <cc1:FilterItem FieldName="Договорувач" PropertyName="ClientName" Comparator="StringComparators"
                                            DataType="String" />
                                        <cc1:FilterItem FieldName="МБ. Осигуреник" PropertyName="OwnerEMBG" Comparator="StringComparators"
                                            DataType="String" />
                                        <cc1:FilterItem FieldName="Осигуреник" PropertyName="OwnerName" Comparator="StringComparators"
                                            DataType="String" />
                                        <cc1:FilterItem FieldName="Регистрација" PropertyName="RegistrationNumber" Comparator="StringComparators"
                                            DataType="String" />
                                        <cc1:FilterItem FieldName="Шасија" PropertyName="ChassisNumber" Comparator="StringComparators"
                                            DataType="String" />
                                        <cc1:FilterItem FieldName="Почеток" PropertyName="StartDate" Comparator="NumericComparators"
                                            DataType="DateTime" />
                                        <cc1:FilterItem FieldName="Истек" PropertyName="EndDate" Comparator="NumericComparators"
                                            DataType="DateTime" />
                                        <cc1:FilterItem FieldName="Дата на издавање" PropertyName="ApplicationDate" Comparator="NumericComparators"
                                            DataType="DateTime" />
                                        <cc1:FilterItem FieldName="Реална премија" PropertyName="RealPremiumValue" Comparator="NumericComparators"
                                            DataType="Decimal" />
                                        <cc1:FilterItem FieldName="Премија за наплата" PropertyName="PremiumValue" Comparator="NumericComparators"
                                            DataType="Decimal" />
                                        <cc1:FilterItem FieldName="Платено" PropertyName="PaidValue" Comparator="NumericComparators"
                                            DataType="Decimal" />
                                        <cc1:FilterItem FieldName="Должи" PropertyName="DebtValue" Comparator="NumericComparators"
                                            DataType="Decimal" />
                                        <cc1:FilterItem FieldName="Сторно" PropertyName="Discard" Comparator="BooleanComparators"
                                            DataType="Boolean" />
                                    </cc1:FilterControl>
                                </div>
                                <cc1:FilterDataSource ID="odsFilterGridView" runat="server" TypeName="Broker.DataAccess.PoliciesView">
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
            <div style="padding-top: 10px;">
                <asp:Button ID="btnMainInformations" runat="server" Text="Основни информации" OnClick="btnMainInformations_Click"
                    CssClass="PacketButton_Active" CausesValidation="false" />
                <asp:Button ID="btnAdditionalInformations" runat="server" Text="Дополнителни информации"
                    OnClick="btnAdditionalInformations_Click" CssClass="PacketButton" CausesValidation="false" />
                <asp:Button ID="btnFinanceCardInformations" runat="server" Text="Финансова картица"
                    OnClick="btnFinanceCardInformations_Click" CssClass="PacketButton" CausesValidation="false" />
                <asp:Button ID="btnPaymentsPerPolicy" runat="server" Text="Плаќања" OnClick="btnPaymentsPerPolicy_Click"
                    CssClass="PacketButton" CausesValidation="false" />
                <div class="PacketOut">
                    <asp:MultiView ID="mvPacket" runat="server" ActiveViewIndex="0">
                        <asp:View ID="viewMainInformations" runat="server">
                            <div class="border1px_Policy">
                                <asp:DetailsView ID="PoliciesDetailsView" runat="server" DataKeyNames="ID" DataSourceID="dvDataSourcePolicyDetails"
                                    AutoGenerateRows="False" GridLines="None">
                                    <Fields>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <table width="680px">
                                                    <tr>
                                                        <td width="172px">
                                                            <asp:Label ID="lblPolicyNumber" runat="server" Text="Број на полиса"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="tbPolicyNumber" runat="server" Font-Bold="true" ReadOnly="true"
                                                                Text='<%#Eval("PolicyNumber") %>'></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr class="light-background">
                                                        <td>
                                                            <asp:Label ID="lblInsuranceCompanies" runat="server" Text="Осигурителна компанија"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlInsuranceCompany" runat="server" DataSourceID="odsInsuranceCompany"
                                                                DataTextField="Name" DataValueField="ID" SelectedValue='<%# Eval("InsuranceCompanyID") %>'
                                                                Enabled="false" Width="200px">
                                                            </asp:DropDownList>
                                                            <asp:ObjectDataSource ID="odsInsuranceCompany" runat="server" SelectMethod="Select"
                                                                TypeName="Broker.DataAccess.InsuranceCompany"></asp:ObjectDataSource>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblStatus" runat="server" Text="Статус"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlStatus" runat="server" DataSourceID="odsStatus" DataTextField="Description"
                                                                DataValueField="ID" SelectedValue='<%# Eval("StatusID") %>' Enabled="false" Width="200px">
                                                            </asp:DropDownList>
                                                            <asp:ObjectDataSource ID="odsStatus" runat="server" SelectMethod="GetActiveStatuses"
                                                                TypeName="Broker.DataAccess.Statuse"></asp:ObjectDataSource>
                                                        </td>
                                                    </tr>
                                                    <tr class="light-background">
                                                        <td>
                                                            <asp:Label ID="lblClientEmbg" runat="server" Text="МБ. Договорувач"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="tbClientEMBG" runat="server" ReadOnly="true" Text='<%#Eval("Client.EMBG") %>'></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblClientName" runat="server" Text="Договорувач"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="tbClientName" runat="server" ReadOnly="true" Text='<%#Eval("Client.Name") %>'
                                                                Width="300px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr class="light-background">
                                                        <td>
                                                            <asp:Label ID="lblOwnerEmbg" runat="server" Text="МБ. Осигуреник"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="tbOwnerEMBG" runat="server" ReadOnly="true" Text='<%#Eval("Client1.EMBG") %>'></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblOwnerName" runat="server" Text="Осигуреник"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="tbOwnerName" runat="server" ReadOnly="true" Text='<%#Eval("Client1.Name") %>'
                                                                Width="300px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr class="light-background">
                                                        <td>
                                                            <asp:Label ID="lblStartDate" runat="server" Text="Почетна Дата"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="tbStartDate" ReadOnly="true" runat="server" Text='<%# Eval("StartDate", "{0:d}") %>'></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblEndDate" runat="server" Text="Крајна Дата"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="tbEndDate" ReadOnly="true" runat="server" Text='<%# Eval("EndDate","{0:d}") %>'></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr class="light-background">
                                                        <td>
                                                            <asp:Label ID="lblApplicationDate" runat="server" Text="Дата на Издавање"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="tbApplicationDate" ReadOnly="true" runat="server" Text='<%# Eval("applicationDate", "{0:d}") %>'></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Fields>
                                </asp:DetailsView>
                                <table width="680px" style="padding-left: 4px;">
                                    <tr>
                                        <td width="172px">Влезна фактура
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tbIncomeFactureNumber" runat="server" ReadOnly="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr class="light-background">
                                        <td>Начин на плаќање
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tbPolicyItemPaymentType" runat="server" ReadOnly="true" Width="200px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Фактура кон клиент
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tbFactureForClientNumber" runat="server" ReadOnly="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr class="light-background">
                                        <td>Фактура за брокеража
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tbFactureForBrokerage" runat="server" ReadOnly="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <asp:Button ID="btnAnexDeal" runat="server" Text="Печати договор" OnClick="btnAnexDeal_Click" />
                                            <asp:Button ID="btnDeletePolicy" runat="server" Text="Избриши полиса" OnClick="btnDeletePolicy_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <asp:ObjectDataSource ID="dvDataSourcePolicyDetails" runat="server" TypeName="Broker.DataAccess.Policy"
                                ConflictDetection="CompareAllValues" DataObjectTypeName="Broker.DataAccess.Policy"
                                SelectMethod="Get" OldValuesParameterFormatString="oldEntity" OnSelecting="dvDataSourcePolicyDetails_Selecting">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="GXGridView1" Name="id" PropertyName="SelectedValue"
                                        Type="Int32" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                        </asp:View>
                        <asp:View ID="viewAdditionalInformations" runat="server">
                            <asp:Panel ID="pnlEverything" runat="server">
                                <div>
                                    <asp:Panel ID="pnlViewButtons" runat="server">
                                    </asp:Panel>
                                </div>
                                <div class="mvMainPanel">
                                    <asp:Panel ID="pnlMainInformation" runat="server" CssClass="mvMainPanel" Width="690px">
                                        <asp:MultiView ID="mvPolicyItem" runat="server">
                                        </asp:MultiView>
                                    </asp:Panel>
                                </div>
                            </asp:Panel>
                        </asp:View>
                        <asp:View ID="viewFinancialCard" runat="server">
                            <div id="tabeliFrame">
                                <div id="header">
                                    <div id="content">
                                        <asp:Label ID="lblFinCard" runat="server" Text="Финансова картица"></asp:Label>
                                    </div>
                                </div>
                                <div id="contentOuter">
                                    <div id="contentInner">
                                        <asp:GridView ID="GridViewFinCard" runat="server" AutoGenerateColumns="False" GridLines="None"
                                            DataSourceID="odsFinCard" EmptyDataText="Нема записи!" RowStyle-CssClass="row"
                                            CssClass="grid" Caption="Полиса">
                                            <RowStyle CssClass="row" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Дата">
                                                    <ItemTemplate>
                                                        <asp:Label ID="tbDocumentDate" Width="70px" runat="server" ReadOnly="true" Text='<%#Bind("DocumentDate", "{0:d}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Доспева на">
                                                    <ItemTemplate>
                                                        <asp:Label ID="tbPaidDate" Width="70px" runat="server" ReadOnly="true" Text='<%#Bind("PaidDate","{0:d}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Опис">
                                                    <ItemTemplate>
                                                        <asp:Label ID="tbDescription" Width="215px" runat="server" ReadOnly="true" Text='<%#Bind("Description") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Должи">
                                                    <ItemTemplate>
                                                        <asp:Label ID="tbDebtValue" runat="server" Width="63px" ReadOnly="true" CssClass="debtClass"
                                                            Text='<%#Bind("DebtValue", "{0:#,0.00}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Побарува">
                                                    <ItemTemplate>
                                                        <asp:Label ID="tbDemandValue" runat="server" Width="63px" ReadOnly="true" CssClass="demandClass"
                                                            Text='<%#Bind("DemandValue", "{0:#,0.00}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Салдо">
                                                    <ItemTemplate>
                                                        <asp:Label ID="tbSaldoValue" runat="server" Width="63px" ReadOnly="true" CssClass="saldoClass"
                                                            Text='<%#Bind("SaldoValue", "{0:#,0.00}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                        <asp:ObjectDataSource ID="odsFinCard" runat="server" SelectMethod="GetByPolicy" TypeName="Broker.Controllers.FinanceControllers.FinanceCardController"
                                            OnSelecting="odsFinCard_Selecting"></asp:ObjectDataSource>
                                    </div>
                                </div>
                            </div>
                            <asp:Button ID="btnPrintFinCard" runat="server" Text="Печати" OnClick="btnPrintFinCard_Click" />
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Label ID="lblDebtValue" runat="server" Text="Должи"></asp:Label>
                            <asp:TextBox ID="tbDebtValue" runat="server" ReadOnly="true" Width="100px" CssClass="debtClass"></asp:TextBox>
                            <asp:Label ID="lblDemandValue" runat="server" Text="Побарува"></asp:Label>
                            <asp:TextBox ID="tbDemandValue" runat="server" ReadOnly="true" Width="100px" CssClass="demandClass"></asp:TextBox>
                            <asp:Label ID="lblSaldoValue" runat="server" Text="Салдо"></asp:Label>
                            <asp:TextBox ID="tbSaldoValue" runat="server" ReadOnly="true" Width="100px" CssClass="saldoClass"></asp:TextBox>
                        </asp:View>
                        <asp:View ID="viewPaymentsPerPolicy" runat="server">
                            <div id="tabeliFrame">
                                <div id="header">
                                    <div id="content">
                                        <asp:Label ID="Label2" runat="server" Text="Договорени рати"></asp:Label>
                                    </div>
                                </div>
                                <div id="contentOuter">
                                    <div id="contentInner">
                                        <asp:GridView ID="gvRates" runat="server" GridLines="None" DataSourceID="odsRates"
                                            Caption="Договорени рати" EmptyDataText="Нема записи!" AutoGenerateColumns="false"
                                            AllowPaging="true" AllowSorting="false" RowStyle-CssClass="row" CssClass="grid"
                                            OnRowUpdating="gvRates_RowUpdating" OnRowUpdated="gvRates_RowUpdated" OnRowEditing="gvRates_RowEditing">
                                            <RowStyle CssClass="row" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Број на рата">
                                                    <ItemTemplate>
                                                        <asp:Label ID="tbNumber" runat="server" ReadOnly="true" Text='<%#Bind("Number") %>'
                                                            Width="30px"></asp:Label>
                                                        <asp:TextBox ID="tbID" runat="server" Text='<%#Bind("ID") %>' ReadOnly="true" Width="0px"
                                                            Visible="false" ForeColor="Transparent" BorderColor="Transparent"></asp:TextBox>
                                                        <asp:TextBox ID="tbBrokerageValue" runat="server" Text='<%#Bind("BrokerageValue") %>'
                                                            Visible="false" ReadOnly="true" Width="0px" ForeColor="Transparent" BorderColor="Transparent"></asp:TextBox>
                                                        <asp:TextBox ID="tbPaidValue" runat="server" Text='<%#Bind("PaidValue") %>' ReadOnly="true"
                                                            Visible="false" Width="0px" ForeColor="Transparent" BorderColor="Transparent"></asp:TextBox>
                                                        <asp:TextBox ID="tbPolicyItemID" runat="server" Text='<%#Bind("PolicyItemID") %>'
                                                            ReadOnly="true" Visible="false" Width="0px" ForeColor="Transparent" BorderColor="Transparent"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:Label ID="tbNumber" runat="server" ReadOnly="true" Text='<%#Bind("Number") %>'
                                                            Width="30px"></asp:Label>
                                                        <asp:TextBox ID="tbID" runat="server" Text='<%#Bind("ID") %>' ReadOnly="true" Width="0px"
                                                            Visible="false" ForeColor="Transparent" BorderColor="Transparent"></asp:TextBox>
                                                        <asp:TextBox ID="tbBrokerageValue" runat="server" Text='<%#Bind("BrokerageValue") %>'
                                                            Visible="false" ReadOnly="true" Width="0px" ForeColor="Transparent" BorderColor="Transparent"></asp:TextBox>
                                                        <asp:TextBox ID="tbPaidValue" runat="server" Text='<%#Bind("PaidValue") %>' ReadOnly="true"
                                                            Visible="false" Width="0px" ForeColor="Transparent" BorderColor="Transparent"></asp:TextBox>
                                                        <asp:TextBox ID="tbPolicyItemID" runat="server" Text='<%#Bind("PolicyItemID") %>'
                                                            ReadOnly="true" Visible="false" Width="0px" ForeColor="Transparent" BorderColor="Transparent"></asp:TextBox>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Дата">
                                                    <ItemTemplate>
                                                        <asp:Label ID="tbDate" runat="server" ReadOnly="true" Text='<%#Bind("Date", "{0:d}") %>'
                                                            Width="100px"></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="tbDate" runat="server" Text='<%#Bind("Date", "{0:d}") %>' Width="100px"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="rfvDate" runat="server" Display="Dynamic" ErrorMessage="*"
                                                            ControlToValidate="tbDate"></asp:RequiredFieldValidator>
                                                        <asp:CompareValidator ID="cvDate" runat="server" Display="Dynamic" ErrorMessage="*"
                                                            ControlToValidate="tbDate" Operator="DataTypeCheck" Type="Date"></asp:CompareValidator>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Износ" ControlStyle-CssClass="currencyClass">
                                                    <ItemTemplate>
                                                        <asp:Label ID="tbValue" runat="server" ReadOnly="true" Text='<%#Bind("Value" , "{0:#,0.00}") %>'
                                                            Width="100px"></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="tbValue" runat="server" Text='<%#Bind("Value") %>' Width="100px"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="rfvValue" runat="server" Display="Dynamic" ErrorMessage="*"
                                                            ControlToValidate="tbValue"></asp:RequiredFieldValidator>
                                                        <asp:CompareValidator ID="cvValue" runat="server" Display="Dynamic" ErrorMessage="*"
                                                            ControlToValidate="tbValue" Operator="DataTypeCheck" Type="Double"></asp:CompareValidator>
                                                        <super:EntityCallOutValidator ID="RateValueValidator" PropertyName="RateValue" runat="server" />
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                <asp:CommandField CancelText="Откажи" DeleteText="Избриши" EditText="Измени" InsertText="Внеси"
                                                    InsertVisible="False" NewText="Нов" SelectText="Одбери" ShowEditButton="True"
                                                    UpdateText="Ажурирај" />
                                            </Columns>
                                            <PagerStyle HorizontalAlign="Center" />
                                            <%--<SelectedRowStyle CssClass="rowSelected" />--%>
                                            <PagerSettings FirstPageText="<< Прва " PreviousPageText="< Претходна " NextPageText=" Следна >"
                                                LastPageText=" Последна >>" Mode="NextPreviousFirstLast" />
                                        </asp:GridView>
                                        <asp:ObjectDataSource ID="odsRates" runat="server" UpdateMethod="UpdateExtend" SelectMethod="GetByPolicyItemIDExtend"
                                            DataObjectTypeName="Broker.DataAccess.Rate" TypeName="Broker.DataAccess.Rate"
                                            OnSelecting="odsRates_Selecting" OnUpdating="odsRates_Updating" OldValuesParameterFormatString="oldEntity"
                                            ConflictDetection="CompareAllValues">
                                            <UpdateParameters>
                                                <asp:Parameter Name="oldEntity" Type="Object" />
                                                <asp:Parameter Name="newEntity" Type="Object" />
                                                <asp:Parameter Name="Value" Type="Decimal" />
                                                <asp:Parameter Name="BrokerageValue" Type="Decimal" />
                                                <asp:Parameter Name="PaidValue" Type="Decimal" />
                                                <asp:Parameter Name="Date" Type="DateTime" />
                                            </UpdateParameters>
                                        </asp:ObjectDataSource>
                                    </div>
                                </div>
                            </div>
                            <div id="tabeliFrame">
                                <div id="header">
                                    <div id="content">
                                        <asp:Label ID="Label3" runat="server" Text="Плаќања"></asp:Label>
                                    </div>
                                </div>
                                <div id="contentOuter">
                                    <div id="contentInner">
                                        <asp:GridView ID="gvPayments" runat="server" GridLines="None" DataSourceID="odsPayments"
                                            Caption="Плаќања" EmptyDataText="Нема записи!" AutoGenerateColumns="false" AllowPaging="true"
                                            AllowSorting="false" RowStyle-CssClass="row" CssClass="grid" OnRowUpdating="gvPayments_RowUpdating"
                                            OnRowUpdated="gvPayments_RowUpdated" OnRowEditing="gvPayments_RowEditing" OnDataBinding="gvPayments_DataBinding"
                                            OnRowCommand="gvPayments_RowCommand" OnRowCreated="gvPayments_RowCreated" OnRowDataBound="gvPayments_RowDataBound"
                                            OnRowDeleted="gvPayments_RowDeleted" OnRowDeleting="gvPayments_RowDeleting">
                                            <RowStyle CssClass="row" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="За рата">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="tbRateID" runat="server" ReadOnly="true" Text='<%#Bind("RateID") %>'
                                                            Visible="false" Width="0px" ForeColor="Transparent" BorderColor="Transparent"></asp:TextBox>
                                                        <asp:Label ID="tbRateNumber" runat="server" Text='<%#Eval("Rate.Number") %>' ReadOnly="true"
                                                            Width="30px"></asp:Label>
                                                        <asp:TextBox ID="tbID" runat="server" Text='<%#Bind("ID") %>' ReadOnly="true" Width="0px"
                                                            ForeColor="Transparent" BorderColor="Transparent" Visible="false"></asp:TextBox>
                                                        <asp:TextBox ID="tbUserID" runat="server" Text='<%#Bind("UserID") %>' ReadOnly="true"
                                                            Width="0px" ForeColor="Transparent" BorderColor="Transparent" Visible="false"></asp:TextBox>
                                                        <asp:TextBox ID="tbBranchID" runat="server" Text='<%#Bind("BranchID") %>' ReadOnly="true"
                                                            Width="0px" ForeColor="Transparent" BorderColor="Transparent" Visible="false"></asp:TextBox>
                                                        <asp:CheckBox ID="cbIsFactured" runat="server" Width="0px" ForeColor="Transparent"
                                                            BorderColor="Transparent" Checked='<%#Bind("IsFactured") %>' Enabled="false"
                                                            Visible="false" />
                                                        <asp:CheckBox ID="cbIsCashReported" runat="server" Width="0px" ForeColor="Transparent"
                                                            BorderColor="Transparent" Checked='<%#Bind("IsCashReported") %>' Enabled="false"
                                                            Visible="false" />
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="tbRateID" runat="server" ReadOnly="true" Text='<%#Bind("RateID") %>'
                                                            Visible="false" Width="0px" ForeColor="Transparent" BorderColor="Transparent"></asp:TextBox>
                                                        <asp:Label ID="tbRateNumber" runat="server" Text='<%#Eval("Rate.Number") %>' ReadOnly="true"
                                                            Width="30px"></asp:Label>
                                                        <asp:TextBox ID="tbID" runat="server" Text='<%#Bind("ID") %>' ReadOnly="true" Width="0px"
                                                            Visible="false" ForeColor="Transparent" BorderColor="Transparent"></asp:TextBox>
                                                        <asp:TextBox ID="tbUserID" runat="server" Text='<%#Bind("UserID") %>' ReadOnly="true"
                                                            Width="0px" ForeColor="Transparent" BorderColor="Transparent" Visible="false"></asp:TextBox>
                                                        <asp:TextBox ID="tbBranchID" runat="server" Text='<%#Bind("BranchID") %>' ReadOnly="true"
                                                            Width="0px" ForeColor="Transparent" BorderColor="Transparent" Visible="false"></asp:TextBox>
                                                        <asp:CheckBox ID="cbIsFactured" runat="server" Width="0px" ForeColor="Transparent"
                                                            Visible="false" BorderColor="Transparent" Checked='<%#Bind("IsFactured") %>'
                                                            Enabled="false" />
                                                        <asp:CheckBox ID="cbIsCashReported" runat="server" Width="0px" ForeColor="Transparent"
                                                            Visible="false" BorderColor="Transparent" Checked='<%#Bind("IsCashReported") %>'
                                                            Enabled="false" />
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Тип на плаќање">
                                                    <ItemTemplate>
                                                        <asp:Label ID="tbPaymentTypeName" runat="server" Text='<%#Eval("PaymentType.Name") %>'
                                                            Width="80px"></asp:Label>
                                                        <asp:DropDownList ID="ddlPaymentTypes" runat="server" SelectedValue='<%#Bind("PaymentTypeID") %>'
                                                            DataTextField="Name" DataValueField="ID" DataSourceID="odsPaymentTypes" Enabled="false"
                                                            Width="80px" Visible="false">
                                                        </asp:DropDownList>
                                                        <asp:ObjectDataSource ID="odsPaymentTypes" runat="server" DataObjectTypeName="Broker.DataAccess.PaymentType"
                                                            TypeName="Broker.DataAccess.PaymentType" SelectMethod="Select"></asp:ObjectDataSource>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:DropDownList ID="ddlPaymentTypes" runat="server" SelectedValue='<%#Bind("PaymentTypeID") %>'
                                                            DataTextField="Name" DataValueField="ID" DataSourceID="odsPaymentTypes" Width="80px"
                                                            AutoPostBack="true" OnSelectedIndexChanged="ddlPaymentTypesInpaymentsPerPolicy_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                        <asp:ObjectDataSource ID="odsPaymentTypes" runat="server" DataObjectTypeName="Broker.DataAccess.PaymentType"
                                                            TypeName="Broker.DataAccess.PaymentType" SelectMethod="Select"></asp:ObjectDataSource>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Кредитна картица">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCreditCardNameInPayment" runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:DropDownList ID="ddlBankCreditCards" runat="server" DataSourceID="odsBankCreditCards"
                                                            DataTextField="Name" DataValueField="ID" Width="80px" AppendDataBoundItems="true">
                                                            <asp:ListItem Selected="True" Text="" Value="None"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:ObjectDataSource ID="odsBankCreditCards" runat="server" SelectMethod="GetAll"
                                                            TypeName="Broker.DataAccess.BankCreditCard"></asp:ObjectDataSource>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Извод/банка">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblBankslipNumberInPayment" runat="server" Text='<%#Eval("BankslipNumber") %>'></asp:Label>
                                                        <asp:Label ID="lblBankslipBankName" runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="tbBankslipNumber" runat="server" Text='<%#Bind("BankslipNumber") %>'
                                                            Width="75px"></asp:TextBox>
                                                        <asp:DropDownList ID="ddlBanks" runat="server" DataTextField="Name" DataValueField="ID"
                                                            DataSourceID="odsBanks" Width="75px" AppendDataBoundItems="true">
                                                            <asp:ListItem Selected="True" Text="" Value="None"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:ObjectDataSource ID="odsBanks" runat="server" DataObjectTypeName="Broker.DataAccess.Bank"
                                                            TypeName="Broker.DataAccess.Bank" SelectMethod="Select"></asp:ObjectDataSource>
                                                    </EditItemTemplate>
                                                    <InsertItemTemplate>
                                                        <asp:TextBox ID="tbBankslipNumber" runat="server" Text='<%#Bind("BankslipNumber") %>'
                                                            Width="75px"></asp:TextBox>
                                                        <asp:DropDownList ID="ddlBanks" runat="server" DataTextField="Name" DataValueField="ID"
                                                            DataSourceID="odsBanks" Width="75px" AppendDataBoundItems="true">
                                                            <asp:ListItem Text="" Value="None"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:ObjectDataSource ID="odsBanks" runat="server" DataObjectTypeName="Broker.DataAccess.Bank"
                                                            TypeName="Broker.DataAccess.Bank" SelectMethod="Select"></asp:ObjectDataSource>
                                                    </InsertItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Дата">
                                                    <ItemTemplate>
                                                        <asp:Label ID="tbDate" runat="server" ReadOnly="true" Text='<%#Bind("Date", "{0:d}") %>'
                                                            Width="70px"></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="tbDate" runat="server" Text='<%#Bind("Date", "{0:d}") %>' Width="70px"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="rfvDate" runat="server" Display="Dynamic" ErrorMessage="*"
                                                            ControlToValidate="tbDate"></asp:RequiredFieldValidator>
                                                        <asp:CompareValidator ID="cvDate" runat="server" Display="Dynamic" ErrorMessage="*"
                                                            ControlToValidate="tbDate" Operator="DataTypeCheck" Type="Date"></asp:CompareValidator>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Износ" ItemStyle-CssClass="currencyClass">
                                                    <ItemTemplate>
                                                        <asp:Label ID="tbValue" runat="server" Text='<%#Bind("Value") %>' ReadOnly="true"
                                                            Width="80px"></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="tbValue" runat="server" Text='<%#Bind("Value") %>' Width="80px"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="rfvValue" runat="server" ErrorMessage="*" ControlToValidate="tbValue"
                                                            Display="Dynamic"></asp:RequiredFieldValidator>
                                                        <asp:CompareValidator ID="cvValue" runat="server" ErrorMessage="*" ControlToValidate="tbValue"
                                                            Display="Dynamic" Operator="DataTypeCheck" Type="Double"></asp:CompareValidator>
                                                        <super:EntityCallOutValidator ID="ValueValidator" PropertyName="PaymentValue" runat="server" />
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                <asp:CommandField CancelText="Откажи" DeleteText="Избриши" EditText="Измени" InsertText="Внеси"
                                                    InsertVisible="false" NewText="Нов" SelectText="Одбери" ShowEditButton="True"
                                                    ShowInsertButton="false" UpdateText="Ажурирај" ShowDeleteButton="true" />
                                            </Columns>
                                            <PagerStyle HorizontalAlign="Center" />
                                            <%--<SelectedRowStyle CssClass="rowSelected" />--%>
                                            <PagerSettings FirstPageText="<< Прва " PreviousPageText="< Претходна " NextPageText=" Следна >"
                                                LastPageText=" Последна >>" Mode="NextPreviousFirstLast" />
                                        </asp:GridView>
                                        <asp:ObjectDataSource ID="odsPayments" runat="server" UpdateMethod="UpdateExtend"
                                            SelectMethod="GetByPolicyItemIDExtend" DataObjectTypeName="Broker.DataAccess.Payment"
                                            TypeName="Broker.DataAccess.Payment" OnSelecting="odsPayments_Selecting" DeleteMethod="DeleteExtend"
                                            OnUpdating="odsPayments_Updating" OldValuesParameterFormatString="oldEntity"
                                            ConflictDetection="CompareAllValues" OnInserting="odsPayments_Inserting" OnDeleting="odsPayments_Deleting">
                                            <UpdateParameters>
                                                <asp:Parameter Name="oldEntity" Type="Object" />
                                                <asp:Parameter Name="newEntity" Type="Object" />
                                                <asp:Parameter Name="Value" Type="Decimal" />
                                                <asp:Parameter Name="Date" Type="DateTime" />
                                            </UpdateParameters>
                                            <InsertParameters>
                                                <asp:Parameter Name="Value" Type="Decimal" />
                                                <asp:Parameter Name="Date" Type="DateTime" />
                                            </InsertParameters>
                                            <DeleteParameters>
                                                <asp:Parameter Name="entityToDelete" Type="Object" />
                                                <asp:Parameter Name="Value" Type="Decimal" />
                                                <asp:Parameter Name="Date" Type="DateTime" />
                                            </DeleteParameters>
                                        </asp:ObjectDataSource>
                                    </div>
                                </div>
                            </div>
                            <table>
                                <tr>
                                    <td>Премија за наплата
                                    </td>
                                    <td>
                                        <asp:TextBox ID="tbPolicyPremiumCost" Width="100px" runat="server" ReadOnly="true"
                                            CssClass="currencyClass"></asp:TextBox>
                                    </td>
                                    <td>Платено
                                    </td>
                                    <td>
                                        <asp:TextBox ID="tbPolicyTotalPaidValue" Width="100px" runat="server" ReadOnly="true"
                                            CssClass="currencyClass"></asp:TextBox>
                                    </td>
                                    <td>За плаќање
                                    </td>
                                    <td>
                                        <asp:TextBox ID="tbPolicyForPaidValue" Width="100px" runat="server" ReadOnly="true"
                                            CssClass="currencyClass"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Дата на уплата:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="tbDateOfPayment" Width="100px" runat="server"></asp:TextBox>
                                        <asp:CompareValidator ID="cvDateOfPayment" runat="server" ControlToValidate="tbDateOfPayment"
                                            ErrorMessage="*" Operator="DataTypeCheck" Type="Date" Display="Dynamic"></asp:CompareValidator>
                                        <asp:RequiredFieldValidator ID="rfvDateOfPayment" runat="server" ControlToValidate="tbDateOfPayment"
                                            Display="Dynamic" ErrorMessage="*"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <b>Износ</b>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="tbValueOfPayment" Width="100px" runat="server" Font-Bold="true"></asp:TextBox>
                                        <asp:CompareValidator ID="cvPaymentValue" runat="server" ControlToValidate="tbValueOfPayment"
                                            ErrorMessage="*" Operator="DataTypeCheck" Type="Double" Display="Dynamic"></asp:CompareValidator>
                                        <asp:RequiredFieldValidator ID="rfvValueOfPayment" runat="server" ControlToValidate="tbValueOfPayment"
                                            Display="Dynamic" ErrorMessage="*"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Тип на уплата
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlPaymentTypes" runat="server" DataTextField="Name" DataValueField="ID"
                                            Width="100px" DataSourceID="odsPaymentTypes" AutoPostBack="true" OnSelectedIndexChanged="ddlPaymentTypes_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:ObjectDataSource ID="odsPaymentTypes" runat="server" DataObjectTypeName="Broker.DataAccess.PaymentType"
                                            TypeName="Broker.DataAccess.PaymentType" SelectMethod="Select"></asp:ObjectDataSource>
                                    </td>
                                    <td>Банка
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlBank" runat="server" DataSourceID="odsBank" AutoPostBack="True"
                                            DataTextField="Name" DataValueField="ID" Width="155px" CssClass="select" Enabled="false">
                                        </asp:DropDownList>
                                        <asp:ObjectDataSource ID="odsBank" runat="server" SelectMethod="GetBanksWithGreditCards"
                                            TypeName="Broker.DataAccess.Bank" OnSelecting="odsBank_Selecting"></asp:ObjectDataSource>
                                    </td>
                                    <td>Тип на картица
                                    </td>
                                    <td>
                                        <asp:DropDownList CssClass="select normal" ID="ddlCardTypes" runat="server" DataSourceID="odsCardTypes"
                                            DataTextField="Name" DataValueField="ID" Width="100px" Enabled="false">
                                        </asp:DropDownList>
                                        <asp:ObjectDataSource ID="odsCardTypes" runat="server" SelectMethod="GetByBank" TypeName="Broker.DataAccess.CreditCard">
                                            <SelectParameters>
                                                <asp:ControlParameter ControlID="ddlBank" Name="BankID" PropertyName="SelectedValue"
                                                    Type="Int32" />
                                            </SelectParameters>
                                        </asp:ObjectDataSource>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Број на извод
                                    </td>
                                    <td>
                                        <asp:TextBox ID="tbBankslipNumber" runat="server" Width="100px"></asp:TextBox>
                                    </td>
                                    <td>Банка на извод
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlBankslipBanks" runat="server" DataSourceID="odsBankslipsBank"
                                            AutoPostBack="True" DataTextField="Name" DataValueField="ID" Width="155px" CssClass="select">
                                        </asp:DropDownList>
                                        <asp:ObjectDataSource ID="odsBankslipsBank" runat="server" SelectMethod="Select"
                                            TypeName="Broker.DataAccess.Bank"></asp:ObjectDataSource>
                                    </td>
                                    <td></td>
                                    <td>
                                        <asp:Button ID="btnInsert" runat="server" Text="Нов запис" OnClick="btnInsert_Click" />
                                    </td>
                                </tr>
                            </table>
                        </asp:View>
                    </asp:MultiView>
                </div>
            </div>
        </asp:View>
        <asp:View ID="viewSearch" runat="server">
            <div class="paddingKontroli">
                <cc1:SearchControl ID="SearchControl1" runat="server" SearchDataSourceID="odsSearch"
                    GridViewToSearch="GXGridView1" OnSearch="SearchControl1_Search">
                    <cc1:SearchItem FieldName="Број на полиса" PropertyName="PolicyNumber" Comparator="StringComparators"
                        DataType="String" />
                    <cc1:SearchItem FieldName="Класа" PropertyName="InsuranceTypeShortName" Comparator="StringComparators"
                        DataType="String" />
                    <cc1:SearchItem FieldName="Подкласа" PropertyName="InsuranceSubTypesShortDescription"
                        Comparator="StringComparators" DataType="String" />
                    <cc1:SearchItem FieldName="Класа (шифра)" PropertyName="InsuranceTypeCode" Comparator="StringComparators"
                        DataType="String" />
                    <cc1:SearchItem FieldName="Подкласа (шифра)" PropertyName="InsuranceSubTypeCode"
                        Comparator="StringComparators" DataType="String" />
                    <cc1:SearchItem FieldName="Осигурителна компанија" PropertyName="CompanyShortName"
                        Comparator="StringComparators" DataType="String" />
                    <cc1:SearchItem FieldName="МБ. Договорувач" PropertyName="ClientEMBG" Comparator="StringComparators"
                        DataType="String" />
                    <cc1:SearchItem FieldName="Договорувач" PropertyName="ClientName" Comparator="StringComparators"
                        DataType="String" />
                    <cc1:SearchItem FieldName="МБ. Осигуреник" PropertyName="OwnerEMBG" Comparator="StringComparators"
                        DataType="String" />
                    <cc1:SearchItem FieldName="Осигуреник" PropertyName="OwnerName" Comparator="StringComparators"
                        DataType="String" />
                    <cc1:SearchItem FieldName="Регистрација" PropertyName="RegistrationNumber" Comparator="StringComparators"
                        DataType="String" />
                    <cc1:SearchItem FieldName="Шасија" PropertyName="ChassisNumber" Comparator="StringComparators"
                        DataType="String" />
                    <cc1:SearchItem FieldName="Почеток" PropertyName="StartDate" Comparator="NumericComparators"
                        DataType="DateTime" />
                    <cc1:SearchItem FieldName="Истек" PropertyName="EndDate" Comparator="NumericComparators"
                        DataType="DateTime" />
                    <cc1:SearchItem FieldName="Дата на издавање" PropertyName="ApplicationDate" Comparator="NumericComparators"
                        DataType="DateTime" />
                    <cc1:SearchItem FieldName="Реална премија" PropertyName="RealPremiumValue" Comparator="NumericComparators"
                        DataType="Decimal" />
                    <cc1:SearchItem FieldName="Премија за наплата" PropertyName="PremiumValue" Comparator="NumericComparators"
                        DataType="Decimal" />
                    <cc1:SearchItem FieldName="Платено" PropertyName="PaidValue" Comparator="NumericComparators"
                        DataType="Decimal" />
                    <cc1:SearchItem FieldName="Должи" PropertyName="DebtValue" Comparator="NumericComparators"
                        DataType="Decimal" />
                    <cc1:SearchItem FieldName="Сторно" PropertyName="Discard" Comparator="BooleanComparators"
                        DataType="Boolean" />
                </cc1:SearchControl>
                <cc1:GridViewDataSource ID="odsSearch" runat="server" TypeName="Broker.DataAccess.PoliciesView"
                    SelectCountMethod="SelectSearchCountCached" SelectMethod="SelectSearch">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="SearchControl1" PropertyName="SearchArguments" Name="sArgument" />
                    </SelectParameters>
                </cc1:GridViewDataSource>
            </div>
        </asp:View>
        <asp:View ID="viewReport" runat="server">
            <div class="paddingKontroli">
                <cc1:ReportControl ID="reportControl" runat="server" TypeName="Broker.DataAccess.PoliciesView"
                    GridViewID="GXGridView1" SearchControlID="SearchControl1">
                    <cc1:PrintItem HeaderText="Број на полиса" PropertyName="PolicyNumber" />
                    <cc1:PrintItem HeaderText="Класа" PropertyName="InsuranceTypeShortName" />
                    <cc1:PrintItem HeaderText="Подкласа" PropertyName="InsuranceSubTypesShortDescription" />
                    <cc1:PrintItem HeaderText="Класа (шифра)" PropertyName="InsuranceTypeCode" />
                    <cc1:PrintItem HeaderText="Подкласа (шифра)" PropertyName="InsuranceSubTypeCode" />
                    <cc1:PrintItem HeaderText="Осигурителна компанија" PropertyName="CompanyShortName" />
                    <cc1:PrintItem HeaderText="МБ. Договорувач" PropertyName="ClientEMBG" />
                    <cc1:PrintItem HeaderText="Договорувач" PropertyName="ClientName" />
                    <cc1:PrintItem HeaderText="МБ. Осигуреник" PropertyName="OwnerEMBG" />
                    <cc1:PrintItem HeaderText="Осигуреник" PropertyName="OwnerName" />
                    <cc1:PrintItem HeaderText="Регистрација" PropertyName="RegistrationNumber" />
                    <cc1:PrintItem HeaderText="Шасија" PropertyName="ChassisNumber" />
                    <cc1:PrintItem HeaderText="Почеток" PropertyName="StartDate" />
                    <cc1:PrintItem HeaderText="Истек" PropertyName="EndDate" />
                    <cc1:PrintItem HeaderText="Дата на издавање" PropertyName="ApplicationDate" />
                    <cc1:PrintItem HeaderText="Реална премија" PropertyName="RealPremiumValue" />
                    <cc1:PrintItem HeaderText="Премија за наплата" PropertyName="PremiumValue" />
                    <cc1:PrintItem HeaderText="Платено" PropertyName="PaidValue" />
                    <cc1:PrintItem HeaderText="Должи" PropertyName="DebtValue" />
                    <cc1:PrintItem HeaderText="Сторно" PropertyName="Discard" />
                </cc1:ReportControl>
            </div>
        </asp:View>
        <asp:View ID="viewDiscardPolicy" runat="server">
            <div class="paddingKontroli">
                <table>
                    <tr>
                        <td>Број на полиса
                        </td>
                        <td>
                            <asp:TextBox ID="tbDiscardPolicyNumber" runat="server" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>Осигурителна компанија
                        </td>
                        <td>
                            <asp:TextBox ID="tbDiscardInsuranceCompany" runat="server" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>Подкласа на осигурување
                        </td>
                        <td>
                            <asp:TextBox ID="tbDiscardInsuranceSubType" runat="server" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>
                            <asp:Button ID="btnDiscardPol" runat="server" Text="Сторнирај" OnClick="btnDiscardPol_Click" />
                        </td>
                    </tr>
                </table>
            </div>
        </asp:View>
        <asp:View ID="viewAttachments" runat="server">
            <div class="paddingKontroli">
                <asp:DetailsView ID="DetailsViewPolicyForAttachments" runat="server" DataSourceID="odsPolicyForAttachments"
                    DataKeyNames="ID" AutoGenerateRows="False" OnItemCommand="DetailsViewPolicyForAttachments_ItemCommand"
                    OnModeChanging="DetailsViewPolicyForAttachments_ModeChanging" GridLines="None"
                    DefaultMode="ReadOnly">
                    <Fields>
                        <asp:TemplateField HeaderText="Број на полиса">
                            <ItemTemplate>
                                <asp:TextBox ID="tbPolicyNumber" runat="server" Text='<%# Bind("PolicyNumber") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Договорувач">
                            <ItemTemplate>
                                <asp:TextBox ID="tbClientEMBG" runat="server" Text='<%# Eval("Client.EMBG") %>' ReadOnly="true"></asp:TextBox>
                                <asp:TextBox ID="tbClientName" runat="server" Text='<%# Eval("Client.Name") %>' ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Осигуреник">
                            <ItemTemplate>
                                <asp:TextBox ID="tbOwnerEMBG" runat="server" Text='<%# Eval("Client1.EMBG") %>' ReadOnly="true"></asp:TextBox>
                                <asp:TextBox ID="tbOwnerName" runat="server" Text='<%# Eval("Client1.Name") %>' ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Статус">
                            <ItemTemplate>
                                <asp:DropDownList ID="ddlStatuses" runat="server" Enabled="False" DataSourceID="odsStatuses"
                                    DataTextField="Description" DataValueField="ID" SelectedValue='<%# Bind("StatusID") %>'>
                                </asp:DropDownList>
                                <asp:ObjectDataSource ID="odsStatuses" runat="server" SelectMethod="Select" TypeName="Broker.DataAccess.Statuse"></asp:ObjectDataSource>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Fields>
                </asp:DetailsView>
                <cc1:DetailsViewDataSource ID="odsPolicyForAttachments" runat="server" TypeName="Broker.DataAccess.Policy"
                    ConflictDetection="CompareAllValues" DataObjectTypeName="Broker.DataAccess.Policy"
                    SelectMethod="Get" OldValuesParameterFormatString="oldEntity" OnSelecting="odsPolicyForAttachments_Selecting">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="GXGridView1" Name="id" PropertyName="SelectedValue"
                            Type="Int32" />
                    </SelectParameters>
                </cc1:DetailsViewDataSource>
                <table>
                    <tr>
                        <td>Датотека<asp:FileUpload ID="FileUpload1" runat="server" />
                        </td>
                        <td>
                            <asp:Button ID="btnAddAttachment" runat="server" Text="Додади" OnClick="btnAddAttachment_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
                        </td>
                        <td></td>
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
