<%@ Page Title="Преглед на полиси (од корисник)" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="PoliciesPerUser.aspx.cs" Inherits="Broker_PoliciesPerUser" %>

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
                            OldValuesParameterFormatString="oldEntity" SelectMethod="SelectByFK" SelectCountMethod="SelectByFKCountCached"
                            OnSelecting="odsGridView_Selecting">
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
                                <cc1:FilterDataSource ID="odsFilterGridView" runat="server" TypeName="Broker.DataAccess.PoliciesView"
                                    SelectMethod="SelectFilterByFK" SelectCountMethod="SelectFilterByFKCountCached"
                                    OnSelecting="odsFilterGridView_Selecting">
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
                    CssClass="PacketButton_Active" />
                <asp:Button ID="btnAdditionalInformations" runat="server" Text="Дополнителни информации"
                    OnClick="btnAdditionalInformations_Click" CssClass="PacketButton" />
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
                                                            <asp:TextBox ID="tbPolicyNumber" Font-Bold="true" runat="server" ReadOnly="true" Text='<%#Eval("PolicyNumber") %>'></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr class="light-background">
                                                        <td>
                                                            <asp:Label ID="lblInsuranceCompanies" runat="server" Text="Осигурителна компанија"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlInsuranceCompany" runat="server" DataSourceID="odsInsuranceCompany"
                                                                DataTextField="Name" DataValueField="ID" SelectedValue='<%# Eval("InsuranceCompanyID") %>'
                                                                Enabled="false">
                                                            </asp:DropDownList>
                                                            <asp:ObjectDataSource ID="odsInsuranceCompany" runat="server" SelectMethod="Select"
                                                                TypeName="Broker.DataAccess.InsuranceCompany"></asp:ObjectDataSource>
                                                        </td>
                                                    </tr>
                                                    <%--<tr>
                                                        <td>
                                                            <asp:Label ID="lblPacketDropDown" runat="server" Text="Пакети"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlPackets" runat="server" DataTextField="Name" DataValueField="ID"
                                                                Enabled="false" DataSourceID="odsDdlPackets">
                                                            </asp:DropDownList>
                                                            <asp:ObjectDataSource ID="odsDdlPackets" runat="server" SelectMethod="Select" TypeName="Broker.DataAccess.Packet">
                                                            </asp:ObjectDataSource>
                                                        </td>
                                                    </tr>--%>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblStatus" runat="server" Text="Статус"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlStatus" runat="server" DataSourceID="odsStatus" DataTextField="Description"
                                                                DataValueField="ID" SelectedValue='<%# Eval("StatusID") %>' Enabled="false">
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
                                                            <asp:TextBox ID="tbClientName" runat="server" ReadOnly="true" Text='<%#Eval("Client.Name") %>'></asp:TextBox>
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
                                                            <asp:TextBox ID="tbOwnerName" runat="server" ReadOnly="true" Text='<%#Eval("Client1.Name") %>'></asp:TextBox>
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
                            </div>
                            <asp:ObjectDataSource ID="dvDataSourcePolicyDetails" runat="server" TypeName="Broker.DataAccess.Policy"
                                ConflictDetection="CompareAllValues" DataObjectTypeName="Broker.DataAccess.Policy"
                                SelectMethod="Get" OldValuesParameterFormatString="oldEntity">
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
                    SelectCountMethod="SelectSearchByFKCountCached" SelectMethod="SelectSearchByFK"
                    OnSelecting="odsSearch_Selecting">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="SearchControl1" PropertyName="SearchArguments" Name="sArgument" />
                    </SelectParameters>
                </cc1:GridViewDataSource>
            </div>
        </asp:View>
        <asp:View ID="viewReport" runat="server">
            <div class="paddingKontroli">
                <cc1:ReportControl ID="reportControl" runat="server" TypeName="Broker.DataAccess.PoliciesView"
                    GridViewID="GXGridView1" SearchControlID="SearchControl1" ForeignKeyName="UserID">
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
                        <td>
                            Број на полиса
                        </td>
                        <td>
                            <asp:TextBox ID="tbDiscardPolicyNumber" runat="server" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Осигурителна компанија
                        </td>
                        <td>
                            <asp:TextBox ID="tbDiscardInsuranceCompany" runat="server" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Подкласа на осигурување
                        </td>
                        <td>
                            <asp:TextBox ID="tbDiscardInsuranceSubType" runat="server" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
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
                                <asp:ObjectDataSource ID="odsStatuses" runat="server" SelectMethod="Select" TypeName="Broker.DataAccess.Statuse">
                                </asp:ObjectDataSource>
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
