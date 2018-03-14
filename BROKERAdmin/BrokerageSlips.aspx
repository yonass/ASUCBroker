<%@ Page Title="Брокерски слипови" Language="C#" MasterPageFile="~/MasterPage.master"
    AutoEventWireup="true" CodeFile="BrokerageSlips.aspx.cs" Inherits="BROKERAdmin_BrokerageSlips" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="kontroli">
        <div id="button01">
            <asp:Button ID="btnNew" runat="server" ToolTip="Нов запис" OnClick="btnNew_Click"
                CausesValidation="false" UseSubmitBehavior="false" CssClass="novZapis" BorderWidth="0px" />
        </div>
        <%--<div id="button02">
            <asp:Button ID="btnFacturePreview" runat="server" ToolTip="Измени" OnClick="btnFacturePreview_Click"
                CausesValidation="false" Enabled="false" UseSubmitBehavior="false" CssClass="izmeni"
                BorderWidth="0px" />
        </div>--%>
        <%--<div id="button03">
            <asp:Button ID="btnDelete" runat="server" OnClick="btnDelete_Click" CausesValidation="false"
                Enabled="false" UseSubmitBehavior="false" CssClass="izbrisi" BorderWidth="0px" />
        </div>--%>
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
        <%--<div id="button05">
            <asp:Button ID="btnDiscardCashReport" runat="server" ToolTip="Сторнирај" OnClick="btnDiscardCashReport_Click"
                CssClass="storniraj" BorderWidth="0px" OnClientClick="return confirm('Дали сте сигурни дека сакате да го сторнирате касовиот извештај?')" />
        </div>--%>
        <div id="button05">
            <asp:Button ID="btnPrintBrokerageSlip" runat="server" ToolTip="Печати" OnClick="btnPrintBrokerageSlip_Click"
                CssClass="pecati" BorderWidth="0px" />
        </div>
    </div>
    <asp:MultiView ID="mvMain" runat="server">
        <asp:View ID="viewGrid" runat="server">
            <div id="tabeliFrame">
                <div id="header">
                    <div id="content">
                        Брокерски слипови
                    </div>
                </div>
                <div id="contentOuter">
                    <div id="contentInner">
                        <cc1:GXGridView ID="GXGridView1" runat="server" DataSourceID="odsGridView" DataKeyNames="ID"
                            Width="100%" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"
                            Caption="Брокерски слипови" EmptyDataText="Нема записи кои го задоволуваат критериумот на пребарување!"
                            OnRowCommand="GXGridView1_RowCommand" RowStyle-CssClass="row" CssClass="grid"
                            GridLines="None">
                            <Columns>
                                <asp:BoundField HeaderText="Број" DataField="Number" SortExpression="Number" />
                                <asp:BoundField HeaderText="Од датум" DataFormatString="{0:d}" DataField="FromDate"
                                    SortExpression="FromDate" />
                                <asp:BoundField HeaderText="До датум" DataFormatString="{0:d}" DataField="ToDate"
                                    SortExpression="ToDate" />
                                <asp:BoundField HeaderText="Датум" DataFormatString="{0:d}" DataField="Date" SortExpression="Date" />
                                <asp:BoundField HeaderText="Осигурителна компанија" DataField="InsuranceCompanyName"
                                    SortExpression="InsuranceCompanyName" />
                            </Columns>
                            <PagerStyle HorizontalAlign="Center" />
                            <SelectedRowStyle CssClass="rowSelected" />
                            <PagerSettings FirstPageText="<< Прва " PreviousPageText="< Претходна " NextPageText=" Следна >"
                                LastPageText=" Последна >>" Mode="NextPreviousFirstLast" />
                        </cc1:GXGridView>
                        <cc1:GridViewDataSource ID="odsGridView" runat="server" TypeName="Broker.DataAccess.ViewBrokerageSlip"
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
                                        <cc1:FilterItem FieldName="Број" PropertyName="Number" Comparator="StringComparators"
                                            DataType="String" />
                                        <cc1:FilterItem FieldName="Од датум" PropertyName="FromDate" Comparator="NumericComparators"
                                            DataType="DateTime" />
                                        <cc1:FilterItem FieldName="До датум" PropertyName="ToDate" Comparator="NumericComparators"
                                            DataType="DateTime" />
                                        <cc1:FilterItem FieldName="Датум" PropertyName="Date" Comparator="NumericComparators"
                                            DataType="DateTime" />
                                        <cc1:FilterItem FieldName="Осигурителна компанија" PropertyName="InsuranceCompanyName"
                                            Comparator="StringComparators" DataType="String" />
                                    </cc1:FilterControl>
                                </div>
                                <cc1:FilterDataSource ID="odsFilterGridView" runat="server" TypeName="Broker.DataAccess.ViewBrokerageSlip">
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
        <asp:View ID="viewNew" runat="server">
            <div class="paddingKontroli">
                <b>Брокерски слип</b>
                <table>
                    <tr>
                        <td style="width: 170px;">
                            Осигурителна компанија
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlInsuranceCompanies" runat="server" DataValueField="ID" DataSourceID="odsInsuranceCompanies"
                                DataTextField="Name">
                            </asp:DropDownList>
                            <asp:ObjectDataSource ID="odsInsuranceCompanies" runat="server" SelectMethod="GetActiveInsuranceCompanies"
                                TypeName="Broker.DataAccess.InsuranceCompany"></asp:ObjectDataSource>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Осигурени опасности
                        </td>
                        <td>
                            <asp:TextBox ID="tbInsuranceRisks" runat="server" Text="СПОРЕД УСЛОВИТЕ НА ОСИГУРУВАЧОТ"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Осигурани ствари
                        </td>
                        <td>
                            <asp:TextBox ID="tbInsuranceThings" runat="server" Text="ПО ПРИЛОГ ПОЛИСИ"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Основ за обештетување
                        </td>
                        <td>
                            <asp:TextBox ID="tbBasisForCompensation" runat="server" Text="СУМА НА ОСИГУРУВАЊЕ"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Други услови
                        </td>
                        <td>
                            <asp:TextBox ID="tbOtherConditions" runat="server" Text=""></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Забелешка
                        </td>
                        <td>
                            <asp:TextBox ID="tbDescription" runat="server" Text=""></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Од датум
                        </td>
                        <td>
                            <asp:TextBox ID="tbFromDate" runat="server">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" Display="Dynamic" ErrorMessage="*"
                                ControlToValidate="tbFromDate"></asp:RequiredFieldValidator>
                            <asp:CompareValidator ID="cvFromDate" runat="server" Display="Dynamic" ControlToValidate="tbFromDate"
                                ErrorMessage="*" Operator="DataTypeCheck" Type="Date"></asp:CompareValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            До датум
                        </td>
                        <td>
                            <asp:TextBox ID="tbToDate" runat="server">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvToDate" runat="server" Display="Dynamic" ErrorMessage="*"
                                ControlToValidate="tbToDate"></asp:RequiredFieldValidator>
                            <asp:CompareValidator ID="cvToDate" runat="server" Display="Dynamic" ControlToValidate="tbToDate"
                                ErrorMessage="*" Operator="DataTypeCheck" Type="Date"></asp:CompareValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:Button ID="btnCheck" runat="server" Text="Провери" OnClick="btnCheck_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:GridView ID="gvNewPolicies" runat="server" AllowPaging="True" AllowSorting="True"
                                AutoGenerateColumns="False" Caption="Полиси од селектираниот период" EmptyDataText="Нема записи кои го задоволуваат критериумот на пребарување!"
                                RowStyle-CssClass="row" CssClass="grid" GridLines="None" OnPageIndexChanging="gvNewPolicies_PageIndexChanging"
                                DataKeyNames="ID" PageSize="10">
                                <RowStyle CssClass="row"></RowStyle>
                                <Columns>
                                    <asp:BoundField HeaderText="" DataField="ID" ItemStyle-BackColor="Transparent" ItemStyle-ForeColor="Transparent" />
                                    <asp:BoundField HeaderText="Број на полиса" DataField="PolicyNumber" SortExpression="PolicyNumber" />
                                    <asp:TemplateField HeaderText="За касов извештај">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="cbIsForFacturing" runat="server" Checked='<%#Bind("IsForFacturing") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <PagerStyle HorizontalAlign="Center" />
                                <PagerSettings FirstPageText="<< Прва " PreviousPageText="< Претходна " NextPageText=" Следна >"
                                    LastPageText=" Последна >>" Mode="NextPreviousFirstLast" />
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:Button ID="btnCreate" runat="server" Text="Сними" OnClick="btnCreate_Click" />
                            <asp:Button ID="btnPrBrokSlip" runat="server" Text="Печати" Enabled="false" OnClick="btnPrBrokSlip_Click" />
                        </td>
                    </tr>
                </table>
            </div>
        </asp:View>
        <asp:View ID="viewSearch" runat="server">
            <div class="paddingKontroli">
                <cc1:SearchControl ID="SearchControl1" runat="server" SearchDataSourceID="odsSearch"
                    GridViewToSearch="GXGridView1" OnSearch="SearchControl1_Search">
                    <cc1:SearchItem FieldName="Број" PropertyName="Number" Comparator="StringComparators"
                        DataType="String" />
                    <cc1:SearchItem FieldName="Од датум" PropertyName="FromDate" Comparator="NumericComparators"
                        DataType="DateTime" />
                    <cc1:SearchItem FieldName="До датум" PropertyName="ToDate" Comparator="NumericComparators"
                        DataType="DateTime" />
                    <cc1:SearchItem FieldName="Датум" PropertyName="Date" Comparator="NumericComparators"
                        DataType="DateTime" />
                    <cc1:SearchItem FieldName="Осигурителна компанија" PropertyName="InsuranceCompanyName"
                        Comparator="StringComparators" DataType="String" />
                </cc1:SearchControl>
                <cc1:GridViewDataSource ID="odsSearch" runat="server" TypeName="Broker.DataAccess.ViewBrokerageSlip"
                    SelectCountMethod="SelectSearchCountCached" SelectMethod="SelectSearch">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="SearchControl1" PropertyName="SearchArguments" Name="sArgument" />
                    </SelectParameters>
                </cc1:GridViewDataSource>
            </div>
        </asp:View>
        <asp:View ID="viewReport" runat="server">
            <div class="paddingKontroli">
                <cc1:ReportControl ID="reportControl" runat="server" TypeName="Broker.DataAccess.ViewBrokerageSlip"
                    GridViewID="GXGridView1" SearchControlID="SearchControl1">
                    <cc1:PrintItem HeaderText="Број" PropertyName="Number" />
                    <cc1:PrintItem HeaderText="Од датум" PropertyName="FromDate" />
                    <cc1:PrintItem HeaderText="До датум" PropertyName="ToDate" />
                    <cc1:PrintItem HeaderText="Датум" PropertyName="Date" />
                    <cc1:PrintItem HeaderText="Осигурителна компанија" PropertyName="InsuranceCompanyName" />
                </cc1:ReportControl>
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>
