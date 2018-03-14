﻿<%@ Page Title="Раздолжувања кон осигурителни компании" Language="C#" MasterPageFile="~/MasterPage.master"
    AutoEventWireup="true" CodeFile="RollBackDistributionForCompany.aspx.cs" Inherits="SEAdmin_RollBackDistributionForCompany" %>

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
        <div id="button04">
            <asp:Button ID="btnPintRollBack" runat="server" ToolTip="Печати" CssClass="pecati"
                BorderWidth="0px" OnClick="btnPintRollBack_Click" Enabled="false" />
        </div>
        <div id="button05">
            <asp:Button ID="btnConfirm" runat="server" ToolTip="Продолжи" CssClass="prodolzi"
                BorderWidth="0px" OnClick="btnConfirm_Click" Enabled="false" />
        </div>
        <div id="button06" visible="false">
            <asp:Button ID="btnPrintAll" runat="server" ToolTip="Печати" CssClass="pecatiAll"
                BorderWidth="0px" OnClick="btnPrintAll_Click" Enabled="false" />
        </div>
        <div id="button07">
            <asp:Button ID="btnInsert" runat="server" ToolTip="Потврди" CssClass="potvrdi" BorderWidth="0px"
                OnClick="btnInsert_Click" Enabled="false" />
        </div>
    </div>
    <asp:MultiView ID="mvMain" runat="server">
        <asp:View ID="viewGrid" runat="server">
            <div id="tabeliFrame">
                <div id="header">
                    <div id="content">
                        Потврдени раз. од филијали
                    </div>
                </div>
                <div id="contentOuter">
                    <div id="contentInner">
                        <cc1:GXGridView ID="GXGridView1" runat="server" DataSourceID="odsGridView" DataKeyNames="ID"
                            Width="100%" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"
                            Caption="Раздолжувања" EmptyDataText="Нема записи кои го задоволуваат критериумот на пребарување!"
                            OnRowCommand="GXGridView1_RowCommand" OnPageIndexChanging="GXGridView1_PageChanged"
                            RowStyle-CssClass="row" CssClass="grid" GridLines="None">
                            <Columns>
                                <asp:BoundField HeaderText="" DataField="ID" ShowHeader="false" ItemStyle-Width="1px"
                                    ItemStyle-BackColor="Transparent" ItemStyle-ForeColor="Transparent" />
                                <asp:TemplateField HeaderText="Селектирај">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="cbSelected" runat="server" Checked='<%#Bind("IsForRollBack") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Број" DataField="Number" SortExpression="Number" />
                                <asp:BoundField HeaderText="Дата на креирање" DataFormatString="{0:d}" DataField="Date"
                                    SortExpression="Date" />
                                <asp:BoundField HeaderText="Од" DataFormatString="{0:d}" DataField="StartDate" SortExpression="StartDate" />
                                <asp:BoundField HeaderText="До" DataFormatString="{0:d}" DataField="EndDate" SortExpression="EndDate" />
                                <asp:BoundField HeaderText="Ос. компанија" DataField="InsuranceCompanyName" SortExpression="InsuranceCompanyName" />
                                <asp:BoundField HeaderText="Филијала" DataField="Name" SortExpression="Name" />
                            </Columns>
                            <PagerStyle HorizontalAlign="Center" />
                            <SelectedRowStyle CssClass="rowSelected" />
                            <PagerSettings FirstPageText="<< Прва " PreviousPageText="< Претходна " NextPageText=" Следна >"
                                LastPageText=" Последна >>" Mode="NextPreviousFirstLast" />
                        </cc1:GXGridView>
                        <cc1:GridViewDataSource ID="odsGridView" runat="server" TypeName="Broker.Controllers.DistributionControllers.RollBackDistributionCompanyInfo"
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
                                        <cc1:FilterItem FieldName="Број" PropertyName="Number" Comparator="NumericComparators"
                                            DataType="Int32" />
                                        <cc1:FilterItem FieldName="Дата на креирање" PropertyName="Date" Comparator="NumericComparators"
                                            DataType="DateTime" />
                                        <cc1:FilterItem FieldName="Од" PropertyName="StartDate" Comparator="NumericComparators"
                                            DataType="DateTime" />
                                        <cc1:FilterItem FieldName="До" PropertyName="EndDate" Comparator="NumericComparators"
                                            DataType="DateTime" />
                                        <cc1:FilterItem FieldName="Ос. компанија" PropertyName="InsuranceCompanyName" Comparator="StringComparators"
                                            DataType="String" />
                                        <cc1:FilterItem FieldName="Филијала" PropertyName="Name" Comparator="StringComparators"
                                            DataType="String" />
                                    </cc1:FilterControl>
                                </div>
                                <cc1:FilterDataSource ID="odsFilterGridView" runat="server" TypeName="Broker.Controllers.DistributionControllers.RollBackDistributionCompanyInfo">
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
        <asp:View ID="viewSelected" runat="server">
            <asp:GridView ID="gvSelectedRollBаcks" runat="server" AllowPaging="True" AllowSorting="True"
                AutoGenerateColumns="False" Caption="Селектирани раздолжници" EmptyDataText="Нема записи кои го задоволуваат критериумот на пребарување!"
                RowStyle-CssClass="rowFacture" CssClass="gridFacture" GridLines="None" OnPageIndexChanging="gvSelectedRollBаcks_PageIndexChanging"
                DataKeyNames="ID" PageSize="10">
                <RowStyle CssClass="rowFacture"></RowStyle>
                <Columns>
                    <asp:TemplateField HeaderText="Селектирај">
                        <ItemTemplate>
                            <asp:CheckBox ID="cbSelected" runat="server" Checked='<%#Bind("IsForRollBack") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Број" DataField="Number" SortExpression="Number" />
                    <asp:BoundField HeaderText="Дата на креирање" DataFormatString="{0:d}" DataField="Date"
                        SortExpression="Date" />
                    <asp:BoundField HeaderText="Од" DataFormatString="{0:d}" DataField="StartDate" SortExpression="StartDate" />
                    <asp:BoundField HeaderText="До" DataFormatString="{0:d}" DataField="EndDate" SortExpression="EndDate" />
                    <asp:BoundField HeaderText="Ос. компанија" DataField="InsuranceCompanyName" SortExpression="InsuranceCompanyName" />
                    <asp:BoundField HeaderText="Филијала" DataField="Name" SortExpression="Name" />
                </Columns>
                <PagerStyle HorizontalAlign="Center" />
                <PagerSettings FirstPageText="<< Прва " PreviousPageText="< Претходна " NextPageText=" Следна >"
                    LastPageText=" Последна >>" Mode="NextPreviousFirstLast" />
            </asp:GridView>
            <asp:Label ID="lblError" Visible="false" runat="server"></asp:Label>
        </asp:View>
        <asp:View ID="viewSearch" runat="server">
            <div class="paddingKontroli">
                <cc1:SearchControl ID="SearchControl1" runat="server" SearchDataSourceID="odsSearch"
                    GridViewToSearch="GXGridView1" OnSearch="SearchControl1_Search">
                    <cc1:SearchItem FieldName="Број" PropertyName="Number" Comparator="NumericComparators"
                        DataType="Int32" />
                    <cc1:SearchItem FieldName="Дата на креирање" PropertyName="Date" Comparator="NumericComparators"
                        DataType="DateTime" />
                    <cc1:SearchItem FieldName="Од" PropertyName="StartDate" Comparator="NumericComparators"
                        DataType="DateTime" />
                    <cc1:SearchItem FieldName="До" PropertyName="EndDate" Comparator="NumericComparators"
                        DataType="DateTime" />
                    <cc1:SearchItem FieldName="Ос. компанија" PropertyName="InsuranceCompanyName" Comparator="StringComparators"
                        DataType="String" />
                    <cc1:SearchItem FieldName="Филијала" PropertyName="Name" Comparator="StringComparators"
                        DataType="String" />
                </cc1:SearchControl>
                <cc1:GridViewDataSource ID="odsSearch" runat="server" TypeName="Broker.Controllers.DistributionControllers.RollBackDistributionCompanyInfo"
                    SelectCountMethod="SelectSearchCountCached" SelectMethod="SelectSearch">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="SearchControl1" PropertyName="SearchArguments" Name="sArgument" />
                    </SelectParameters>
                </cc1:GridViewDataSource>
            </div>
        </asp:View>
        <asp:View ID="viewReport" runat="server">
            <div class="paddingKontroli">
                <cc1:ReportControl ID="reportControl" runat="server" TypeName="Broker.Controllers.DistributionControllers.RollBackDistributionCompanyInfo"
                    GridViewID="GXGridView1" SearchControlID="SearchControl1">
                    <cc1:PrintItem HeaderText="Број" PropertyName="Number" />
                    <cc1:PrintItem HeaderText="Дата на креирање" PropertyName="Date" />
                    <cc1:PrintItem HeaderText="Од" PropertyName="StartDate" />
                    <cc1:PrintItem HeaderText="До" PropertyName="EndDate" />
                  
                    <cc1:PrintItem HeaderText="Ос. компанија" PropertyName="InsuranceCompanyName" />
                    <cc1:PrintItem HeaderText="Филијала" PropertyName="Name" />
                </cc1:ReportControl>
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>
