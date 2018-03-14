<%@ Page Title="Рути за задолжувања на полиси" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="DistributionRoutes.aspx.cs" Inherits="SEAdmin_DistributionRoutes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="kontroli">
        <div id="button01">
            <asp:Button ID="btnPreview" runat="server" ToolTip="Освежи" OnClick="btnPreview_Click"
                CausesValidation="false" UseSubmitBehavior="false" CssClass="osvezi" BorderWidth="0px" />
        </div>
        <div id="button02">
            <asp:Button ID="btnReport" runat="server" ToolTip="Извештај" CausesValidation="false"
                UseSubmitBehavior="false" OnClick="btnReport_Click" CssClass="izvestaj" BorderWidth="0px" />
        </div>
        <div id="button03">
            <asp:Button ID="btnSearch" runat="server" ToolTip="Пребарај" OnClick="btnSearch_Click"
                CausesValidation="false" UseSubmitBehavior="false" CssClass="prebaraj" BorderWidth="0px" />
        </div>
    </div>
    <asp:MultiView ID="mvMain" runat="server">
        <asp:View ID="viewGrid" runat="server">
            <div id="tabeliFrame">
                <div id="header">
                    <div id="content">
                        Рути на задолжувања
                    </div>
                </div>
                <div id="contentOuter">
                    <div id="contentInner">
                        <cc1:GXGridView ID="GXGridView1" runat="server" DataSourceID="odsGridView" DataKeyNames="ID"
                            Width="100%" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"
                            Caption="Рути на задолжувања" EmptyDataText="Нема записи кои го задоволуваат критериумот на пребарување!"
                            OnRowCommand="GXGridView1_RowCommand" RowStyle-CssClass="row" CssClass="grid"
                            GridLines="None">
                            <Columns>
                                <asp:BoundField HeaderText="Од број" DataField="FromNumber" SortExpression="FromNumber" />
                                <asp:BoundField HeaderText="До број" DataField="ToNumber" SortExpression="ToNumber" />
                                <asp:BoundField HeaderText="Од корисник" DataField="FromName" SortExpression="FromName" />
                                <asp:BoundField HeaderText="За корисник" DataField="ToName" SortExpression="ToName" />
                                <asp:BoundField HeaderText="Број на испратница" DataField="DocumentNumber" SortExpression="DocumentNumber" />
                                <asp:BoundField HeaderText="Датум" DataField="Date" DataFormatString="{0:d}" SortExpression="Date" />
                                <asp:BoundField HeaderText="Ос. компанија" DataField="InsuranceCompanyShortName"
                                    SortExpression="InsuranceCompanyShortName" />
                                <asp:BoundField HeaderText="Подкласа на осигурување" DataField="InsuranceSubTypeShortDescription"
                                    SortExpression="InsuranceSubTypeShortDescription" />
                            </Columns>
                            <PagerStyle HorizontalAlign="Center" />
                            <SelectedRowStyle CssClass="rowSelected" />
                            <PagerSettings FirstPageText="<< Прва " PreviousPageText="< Претходна " NextPageText=" Следна >"
                                LastPageText=" Последна >>" Mode="NextPreviousFirstLast" />
                        </cc1:GXGridView>
                        <cc1:GridViewDataSource ID="odsGridView" runat="server" TypeName="Broker.DataAccess.ViewDistributionRoute"
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
                                        <cc1:FilterItem FieldName="Од број" PropertyName="FromNumber" Comparator="StringComparators"
                                            DataType="String" />
                                        <cc1:FilterItem FieldName="До број" PropertyName="ToNumber" Comparator="StringComparators"
                                            DataType="String" />
                                        <cc1:FilterItem FieldName="Од корисник" PropertyName="FromName" Comparator="StringComparators"
                                            DataType="String" />
                                        <cc1:FilterItem FieldName="За корисник" PropertyName="ToName" Comparator="StringComparators"
                                            DataType="String" />
                                        <cc1:FilterItem FieldName="Број на испратница" PropertyName="DocumentNumber" Comparator="StringComparators"
                                            DataType="String" />
                                        <cc1:FilterItem FieldName="Датум" PropertyName="Date" Comparator="NumericComparators"
                                            DataType="DateTime" />
                                        <cc1:FilterItem FieldName="Ос. компанија" PropertyName="InsuranceCompanyShortName"
                                            Comparator="StringComparators" DataType="String" />
                                        <cc1:FilterItem FieldName="Подкласа на осигурување" PropertyName="InsuranceSubTypeShortDescription"
                                            Comparator="StringComparators" DataType="String" />
                                    </cc1:FilterControl>
                                </div>
                                <cc1:FilterDataSource ID="odsFilterGridView" runat="server" TypeName="Broker.DataAccess.ViewDistributionRoute">
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
        <asp:View ID="viewSearch" runat="server">
            <div class="paddingKontroli">
                <cc1:SearchControl ID="SearchControl1" runat="server" SearchDataSourceID="odsSearch"
                    GridViewToSearch="GXGridView1" OnSearch="SearchControl1_Search">
                    <cc1:SearchItem FieldName="Од број" PropertyName="FromNumber" Comparator="StringComparators"
                        DataType="String" />
                    <cc1:SearchItem FieldName="До број" PropertyName="ToNumber" Comparator="StringComparators"
                        DataType="String" />
                    <cc1:SearchItem FieldName="Од корисник" PropertyName="FromName" Comparator="StringComparators"
                        DataType="String" />
                    <cc1:SearchItem FieldName="За корисник" PropertyName="ToName" Comparator="StringComparators"
                        DataType="String" />
                    <cc1:SearchItem FieldName="Број на испратница" PropertyName="DocumentNumber" Comparator="StringComparators"
                        DataType="String" />
                    <cc1:SearchItem FieldName="Датум" PropertyName="Date" Comparator="NumericComparators"
                        DataType="DateTime" />
                    <cc1:SearchItem FieldName="Ос. компанија" PropertyName="InsuranceCompanyShortName"
                        Comparator="StringComparators" DataType="String" />
                    <cc1:SearchItem FieldName="Подкласа на осигурување" PropertyName="InsuranceSubTypeShortDescription"
                        Comparator="StringComparators" DataType="String" />
                </cc1:SearchControl>
                <cc1:GridViewDataSource ID="odsSearch" runat="server" TypeName="Broker.DataAccess.ViewDistributionRoute"
                    SelectCountMethod="SelectSearchCountCached" SelectMethod="SelectSearch">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="SearchControl1" PropertyName="SearchArguments" Name="sArgument" />
                    </SelectParameters>
                </cc1:GridViewDataSource>
            </div>
        </asp:View>
        <asp:View ID="viewReport" runat="server">
            <div class="paddingKontroli">
                <cc1:ReportControl ID="reportControl" runat="server" TypeName="Broker.DataAccess.ViewDistributionRoute"
                    GridViewID="GXGridView1" SearchControlID="SearchControl1">
                    <cc1:PrintItem HeaderText="Шифра" PropertyName="Code" />
                    <cc1:PrintItem HeaderText="Име" PropertyName="Name" />
                    <cc1:PrintItem HeaderText="Жиро-сметка" PropertyName="BankAccount" />
                    <cc1:PrintItem HeaderText="ЕМБГ" PropertyName="EMBG" />
                    <cc1:PrintItem HeaderText="Телефон" PropertyName="Phone" />
                    <cc1:PrintItem HeaderText="Факс" PropertyName="Fax" />
                    <cc1:PrintItem HeaderText="Мобилен" PropertyName="Mobile" />
                    <cc1:PrintItem HeaderText="Е-Маил" PropertyName="EMail" />
                    <cc1:PrintItem HeaderText="Од број" PropertyName="FromNumber" />
                    <cc1:PrintItem HeaderText="До број" PropertyName="ToNumber" />
                    <cc1:PrintItem HeaderText="Од корисник" PropertyName="FromName" />
                    <cc1:PrintItem HeaderText="За корисник" PropertyName="ToName" />
                    <cc1:PrintItem HeaderText="Број на испратница" PropertyName="DocumentNumber" />
                    <cc1:PrintItem HeaderText="Датум" PropertyName="Date" />
                    <cc1:PrintItem HeaderText="Ос. компанија" PropertyName="InsuranceCompanyShortName" />
                    <cc1:PrintItem HeaderText="Подкласа на осигурување" PropertyName="InsuranceSubTypeShortDescription" />
                </cc1:ReportControl>
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>
