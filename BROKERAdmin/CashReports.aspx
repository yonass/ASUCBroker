<%@ Page Title="Преглед на касови извештаи" Language="C#" MasterPageFile="~/MasterPage.master"
    AutoEventWireup="true" CodeFile="CashReports.aspx.cs" Inherits="BROKERAdmin_CashReports" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="kontroli">
        <div id="button01">
            <asp:Button ID="btnChangeStatus" runat="server" ToolTip="Измени" OnClick="btnChangeStatus_Click"
                CausesValidation="false" Enabled="false" UseSubmitBehavior="false" CssClass="izmeni"
                BorderWidth="0px" />
        </div>
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
                                <asp:BoundField HeaderText="Филијала (код)" DataField="BranchCode" SortExpression="BranchCode" />
                                <asp:BoundField HeaderText="Филијала" DataField="BranchName" SortExpression="BranchName" />
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
                                        <cc1:FilterItem FieldName="Филијала (код)" PropertyName="BranchCode" Comparator="StringComparators"
                                            DataType="String" />
                                        <cc1:FilterItem FieldName="Филијала" PropertyName="BranchName" Comparator="StringComparators"
                                            DataType="String" />
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
                                <cc1:FilterDataSource ID="odsFilterGridView" runat="server" TypeName="Broker.DataAccess.ViewCashReport">
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
        <asp:View ID="viewChangeStatus" runat="server">
            <div class="paddingKontroli">
                <b>Касов извештај</b>
                <asp:DetailsView ID="dvChangeStatus" runat="server" DataSourceID="odsChangeStatus"
                    DataKeyNames="ID" AutoGenerateRows="false" GridLines="None">
                    <Fields>
                        <asp:TemplateField HeaderText="Број">
                            <ItemTemplate>
                                <asp:TextBox ID="tbNumber" runat="server" Text='<%# Eval("Number") %>' ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Филијала">
                            <ItemTemplate>
                                <asp:TextBox ID="tbBranchCode" Width="50px" runat="server" Text='<%# Eval("Branch.Code") %>'
                                    ReadOnly="true"></asp:TextBox>
                                <asp:TextBox ID="tbBranchName" Width="250px" runat="server" Text='<%# Eval("Branch.Name") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Вкупен износ">
                            <ItemTemplate>
                                <asp:TextBox ID="tbTotalCost" runat="server" Text='<%# Eval("TotalValue") %>' ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Дата на фактура">
                            <ItemTemplate>
                                <asp:TextBox ID="tbCashReportDate" runat="server" Text='<%# Bind("CashReportDate","{0:d}") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Сторно">
                            <ItemTemplate>
                                <asp:CheckBox ID="cbDiscard" runat="server" Checked='<%#Bind("Discard") %>' Enabled="false" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Нов статус">
                            <ItemTemplate>
                                <asp:RadioButtonList ID="rblStatuses" runat="server" RepeatDirection="Vertical">
                                    <asp:ListItem Selected="True" Text="Одобрено" Value="ODOBRENO"></asp:ListItem>
                                    <asp:ListItem Text="Вратено" Value="VRATENO"></asp:ListItem>
                                </asp:RadioButtonList>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Button ID="btnUpdateStatus" Text="Измени" runat="server" OnClick="btnUpdateStatus_Click" />
                                <asp:Button ID="btnCancel" CommandName="Cancel" Text="Откажи" CausesValidation="false"
                                    runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Fields>
                </asp:DetailsView>
                <asp:ObjectDataSource ID="odsChangeStatus" runat="server" SelectMethod="Get" TypeName="Broker.DataAccess.CashReport"
                    ConflictDetection="CompareAllValues" DataObjectTypeName="Broker.DataAccess.CashReport">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="GXGridView1" Name="id" PropertyName="SelectedValue"
                            Type="Int32" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </div>
        </asp:View>
        <asp:View ID="viewSearch" runat="server">
            <div class="paddingKontroli">
                <cc1:SearchControl ID="SearchControl1" runat="server" SearchDataSourceID="odsSearch"
                    GridViewToSearch="GXGridView1" OnSearch="SearchControl1_Search">
                    <cc1:SearchItem FieldName="Филијала (код)" PropertyName="BranchCode" Comparator="StringComparators"
                        DataType="String" />
                    <cc1:SearchItem FieldName="Филијала" PropertyName="BranchName" Comparator="StringComparators"
                        DataType="String" />
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
                    SelectCountMethod="SelectSearchCountCached" SelectMethod="SelectSearch">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="SearchControl1" PropertyName="SearchArguments" Name="sArgument" />
                    </SelectParameters>
                </cc1:GridViewDataSource>
            </div>
        </asp:View>
        <asp:View ID="viewReport" runat="server">
            <div class="paddingKontroli">
                <cc1:ReportControl ID="reportControl" runat="server" TypeName="Broker.DataAccess.ViewCashReport"
                    GridViewID="GXGridView1" SearchControlID="SearchControl1">
                    <cc1:PrintItem HeaderText="Филијала (код)" PropertyName="BranchCode" />
                    <cc1:PrintItem HeaderText="Филијала" PropertyName="BranchName" />
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
