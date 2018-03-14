<%@ Page Title="Касови извештаи" Language="C#" MasterPageFile="~/MasterPage.master"
    AutoEventWireup="true" CodeFile="CashReports.aspx.cs" Inherits="Broker_CashReports" %>

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
        <div id="button05">
            <asp:Button ID="btnDiscardCashReport" runat="server" ToolTip="Сторнирај" OnClick="btnDiscardCashReport_Click"
                CssClass="storniraj" BorderWidth="0px" OnClientClick="return confirm('Дали сте сигурни дека сакате да го сторнирате касовиот извештај?')" />
        </div>
        <div id="button06">
            <asp:Button ID="btnPintCashReport" runat="server" ToolTip="Печати" OnClick="btnPintCashReport_Click"
                CssClass="pecati" BorderWidth="0px" />
        </div>
    </div>
    <asp:MultiView ID="mvMain" runat="server">
        <asp:View ID="viewGrid" runat="server">
            <div id="tabeliFrame">
                <div id="header">
                    <div id="content">
                        Касови извештаи
                    </div>
                </div>
                <div id="contentOuter">
                    <div id="contentInner">
                        <cc1:GXGridView ID="GXGridView1" runat="server" DataSourceID="odsGridView" DataKeyNames="ID"
                            Width="100%" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"
                            Caption="Касови извештаи" EmptyDataText="Нема записи кои го задоволуваат критериумот на пребарување!"
                            OnRowCommand="GXGridView1_RowCommand" RowStyle-CssClass="row" CssClass="grid"
                            GridLines="None">
                            <Columns>
                                <asp:BoundField HeaderText="Број" DataField="Number" SortExpression="Number" />
                                <asp:BoundField HeaderText="Датум" DataFormatString="{0:d}" DataField="CashReportDate"
                                    SortExpression="CashReportDate" />
                                <asp:BoundField HeaderText="Статус" DataField="CashReportStatusName" SortExpression="CashReportStatusName" />
                                <asp:BoundField HeaderText="Износ" DataField="TotalValue" SortExpression="TotalValue"
                                    ItemStyle-HorizontalAlign="Right" />
                                <asp:CheckBoxField HeaderText="Сторно" DataField="Discard" SortExpression="Discard" />
                            </Columns>
                            <PagerStyle HorizontalAlign="Center" />
                            <SelectedRowStyle CssClass="rowSelected" />
                            <PagerSettings FirstPageText="<< Прва " PreviousPageText="< Претходна " NextPageText=" Следна >"
                                LastPageText=" Последна >>" Mode="NextPreviousFirstLast" />
                        </cc1:GXGridView>
                        <cc1:GridViewDataSource ID="odsGridView" runat="server" TypeName="Broker.DataAccess.ViewCashReport"
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
                                        <cc1:FilterItem FieldName="Број" PropertyName="Number" Comparator="StringComparators"
                                            DataType="String" />
                                        <cc1:FilterItem FieldName="Датум" PropertyName="CashReportDate" Comparator="NumericComparators"
                                            DataType="DateTime" />
                                        <cc1:FilterItem FieldName="Статус" PropertyName="CashReportStatusName" Comparator="StringComparators"
                                            DataType="String" />
                                        <cc1:FilterItem FieldName="Износ" PropertyName="TotalValue" Comparator="NumericComparators"
                                            DataType="Decimal" />
                                        <cc1:FilterItem FieldName="Сторно" PropertyName="Discard" Comparator="BooleanComparators"
                                            DataType="Boolean" />
                                    </cc1:FilterControl>
                                </div>
                                <cc1:FilterDataSource ID="odsFilterGridView" runat="server" TypeName="Broker.DataAccess.ViewCashReport"
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
        <asp:View ID="viewNew" runat="server">
            <div class="paddingKontroli">
                <b>Касов извештај</b>
                <table>
                    <tr>
                        <td>
                            За ден
                            <asp:TextBox ID="tbReportDate" runat="server">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvReportDate" runat="server" Display="Dynamic" ErrorMessage="*"
                                ControlToValidate="tbReportDate"></asp:RequiredFieldValidator>
                            <asp:CompareValidator ID="cvReportDate" runat="server" Display="Dynamic" ControlToValidate="tbReportDate"
                                ErrorMessage="*" Operator="DataTypeCheck" Type="Date"></asp:CompareValidator>
                            <asp:Button ID="btnCheck" runat="server" Text="Провери" OnClick="btnCheck_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:GridView ID="gvNewPolicies" runat="server" AllowPaging="True" AllowSorting="True"
                                AutoGenerateColumns="False" Caption="Плаќања од селектираниот период" EmptyDataText="Нема записи кои го задоволуваат критериумот на пребарување!"
                                RowStyle-CssClass="row" CssClass="grid" GridLines="None" OnPageIndexChanging="gvNewPolicies_PageIndexChanging"
                                DataKeyNames="ID" PageSize="10">
                                <RowStyle CssClass="row"></RowStyle>
                                <Columns>
                                    <asp:BoundField HeaderText="" DataField="ID" />
                                    <asp:BoundField HeaderText="Број на полиса" DataField="PolicyNumber" SortExpression="PolicyNumber" />
                                    <asp:BoundField HeaderText="Тип на плаќање" DataField="PaymentTypeName" SortExpression="PaymentTypeName" />
                                    <asp:BoundField HeaderText="Износ" DataField="Value" SortExpression="Value" />
                                    <asp:TemplateField HeaderText="За касов извештај">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="cbIsForCashReporting" runat="server" Checked='<%#Bind("IsForCashReporting") %>' />
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
                            <asp:Button ID="btnCreate" runat="server" Text="Сними" OnClick="btnCreate_Click" />
                            <asp:Button ID="btnPrintCashReport" runat="server" Text="Печати" Enabled="false"
                                OnClick="btnPrintCashReport_Click" />
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
                    <cc1:SearchItem FieldName="Датум" PropertyName="CashReportDate" Comparator="NumericComparators"
                        DataType="DateTime" />
                    <cc1:SearchItem FieldName="Статус" PropertyName="CashReportStatusName" Comparator="StringComparators"
                        DataType="String" />
                    <cc1:SearchItem FieldName="Износ" PropertyName="TotalValue" Comparator="NumericComparators"
                        DataType="Decimal" />
                    <cc1:SearchItem FieldName="Сторно" PropertyName="Discard" Comparator="BooleanComparators"
                        DataType="Boolean" />
                </cc1:SearchControl>
                <cc1:GridViewDataSource ID="odsSearch" runat="server" TypeName="Broker.DataAccess.ViewCashReport"
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
                <cc1:ReportControl ID="reportControl" runat="server" TypeName="Broker.DataAccess.ViewCashReport"
                    GridViewID="GXGridView1" SearchControlID="SearchControl1" ForeignKeyName="BranchID">
                    <cc1:PrintItem HeaderText="Број" PropertyName="Number" />
                    <cc1:PrintItem HeaderText="Датум" PropertyName="CashReportDate" />
                    <cc1:PrintItem HeaderText="Статус" PropertyName="CashReportStatusName" />
                    <cc1:PrintItem HeaderText="Износ" PropertyName="TotalValue" />
                    <cc1:PrintItem HeaderText="Сторно" PropertyName="Discard" />
                </cc1:ReportControl>
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>
