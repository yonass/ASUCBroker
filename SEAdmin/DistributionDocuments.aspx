<%@ Page Title="Документи за задолжување на полиси" Language="C#" MasterPageFile="~/MasterPage.master"
    AutoEventWireup="true" CodeFile="DistributionDocuments.aspx.cs" Inherits="SEAdmin_DistributionDocuments" %>

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
            <asp:Button ID="btnDocumentItems" runat="server" ToolTip="Преглед" CausesValidation="false"
                UseSubmitBehavior="false" OnClick="btnDocumentItems_Click" CssClass="prikaz"
                BorderWidth="0px" Enabled="false" />
        </div>
        <div id="button05">
            <asp:Button ID="btnPrintDocument" runat="server" ToolTip="Печати" OnClick="btnPrintDocument_Click"
                CausesValidation="false" UseSubmitBehavior="false" CssClass="pecati" BorderWidth="0px"
                Enabled="false" />
        </div>
    </div>
    <asp:MultiView ID="mvMain" runat="server">
        <asp:View ID="viewGrid" runat="server">
            <div id="tabeliFrame">
                <div id="header">
                    <div id="content">
                        Документи од задолжување
                    </div>
                </div>
                <div id="contentOuter">
                    <div id="contentInner">
                        <cc1:GXGridView ID="GXGridView1" runat="server" DataSourceID="odsGridView" DataKeyNames="ID"
                            Width="100%" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"
                            Caption="Документи од задолжување" EmptyDataText="Нема записи кои го задоволуваат критериумот на пребарување!"
                            OnRowCommand="GXGridView1_RowCommand" RowStyle-CssClass="row" CssClass="grid"
                            GridLines="None">
                            <Columns>
                                <asp:BoundField HeaderText="Број" DataField="DocumentNumber" SortExpression="DocumentNumber" />
                                <asp:BoundField HeaderText="Дата" DataField="DocumentDate" DataFormatString="{0:d}"
                                    SortExpression="DocumentDate" />
                                <asp:BoundField HeaderText="Тип на документ" DataField="DistributionDocTypeName"
                                    SortExpression="DistributionDocTypeName" />
                                <asp:BoundField HeaderText="Статус" DataField="DistributionDocumentStatusName" SortExpression="DistributionDocumentStatusName" />
                                <asp:BoundField HeaderText="Филијала" DataField="BranchName" SortExpression="BranchName" />
                            </Columns>
                            <PagerStyle HorizontalAlign="Center" />
                            <SelectedRowStyle CssClass="rowSelected" />
                            <PagerSettings FirstPageText="<< Прва " PreviousPageText="< Претходна " NextPageText=" Следна >"
                                LastPageText=" Последна >>" Mode="NextPreviousFirstLast" />
                        </cc1:GXGridView>
                        <cc1:GridViewDataSource ID="odsGridView" runat="server" TypeName="Broker.DataAccess.ViewDistributionDocument"
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
                                        <cc1:FilterItem FieldName="Број" PropertyName="DocumentNumber" Comparator="StringComparators"
                                            DataType="String" />
                                        <cc1:FilterItem FieldName="Дата" PropertyName="DocumentDate" Comparator="NumericComparators"
                                            DataType="DateTime" />
                                        <cc1:FilterItem FieldName="Тип на документ" PropertyName="DistributionDocTypeName"
                                            Comparator="StringComparators" DataType="String" />
                                        <cc1:FilterItem FieldName="Статус" PropertyName="DistributionDocumentStatusName"
                                            Comparator="StringComparators" DataType="String" />
                                        <cc1:FilterItem FieldName="Филијала (шифра)" PropertyName="BranchCode" Comparator="StringComparators"
                                            DataType="String" />
                                        <cc1:FilterItem FieldName="Филијала" PropertyName="BranchName" Comparator="StringComparators"
                                            DataType="String" />
                                    </cc1:FilterControl>
                                </div>
                                <cc1:FilterDataSource ID="odsFilterGridView" runat="server" TypeName="Broker.DataAccess.ViewDistributionDocument">
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
                    <cc1:SearchItem FieldName="Број" PropertyName="DocumentNumber" Comparator="StringComparators"
                        DataType="String" />
                    <cc1:SearchItem FieldName="Дата" PropertyName="DocumentDate" Comparator="NumericComparators"
                        DataType="DateTime" />
                    <cc1:SearchItem FieldName="Тип на документ" PropertyName="DistributionDocTypeName"
                        Comparator="StringComparators" DataType="String" />
                    <cc1:SearchItem FieldName="Статус" PropertyName="DistributionDocumentStatusName"
                        Comparator="StringComparators" DataType="String" />
                    <cc1:SearchItem FieldName="Филијала (шифра)" PropertyName="BranchCode" Comparator="StringComparators"
                        DataType="String" />
                    <cc1:SearchItem FieldName="Филијала" PropertyName="BranchName" Comparator="StringComparators"
                        DataType="String" />
                </cc1:SearchControl>
                <cc1:GridViewDataSource ID="odsSearch" runat="server" TypeName="Broker.DataAccess.ViewDistributionDocument"
                    SelectCountMethod="SelectSearchCountCached" SelectMethod="SelectSearch">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="SearchControl1" PropertyName="SearchArguments" Name="sArgument" />
                    </SelectParameters>
                </cc1:GridViewDataSource>
            </div>
        </asp:View>
        <asp:View ID="viewDistributionDocumentItems" runat="server">
            <div class="paddingKontroli">
                <asp:DetailsView ID="dvDistributionDocumentInfo" runat="server" DataSourceID="odsDistributionDocument"
                    AutoGenerateRows="false" GridLines="None" DefaultMode="ReadOnly">
                    <Fields>
                        <asp:TemplateField HeaderText="Број на документ">
                            <ItemTemplate>
                                <asp:TextBox ID="tbDocumentNumber" runat="server" ReadOnly="true" Text='<%#Eval("DocumentNumber") %>'></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Дата">
                            <ItemTemplate>
                                <asp:TextBox ID="tbDocumentDate" runat="server" ReadOnly="true" Text='<%#Eval("DocumentDate") %>'></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Тип на документ">
                            <ItemTemplate>
                                <asp:TextBox ID="tbDistributionDocTypeName" ReadOnly="true" runat="server" Text='<%#Eval("DistributionDocType.Name") %>'></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Статус">
                            <ItemTemplate>
                                <asp:TextBox ID="tbDistributionDocumentStatuseName" ReadOnly="true" runat="server"
                                    Text='<%#Eval("DistributionDocumentStatuse.Name") %>'></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Опис">
                            <ItemTemplate>
                                <asp:TextBox ID="tbDistributionDocumentDescription" ReadOnly="true" runat="server"
                                    Text='<%#Eval("Description") %>'></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Fields>
                </asp:DetailsView>
                <asp:ObjectDataSource ID="odsDistributionDocument" runat="server" SelectMethod="Get"
                    TypeName="Broker.DataAccess.DistributionDocument" DataObjectTypeName="Broker.DataAccess.DistributionDocument"
                    OnSelecting="odsDistributionDocument_Selecting"></asp:ObjectDataSource>
            </div>
            <div id="tabeliFrame">
                <div id="header">
                    <div id="content">
                        Преглед на полиси
                    </div>
                </div>
                <div id="contentOuter">
                    <div id="contentInner">
                        <asp:GridView ID="GridViewDistributionDocumentItems" runat="server" DataSourceID="odsDistributionDocumentItems"
                            Width="100%" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"
                            Caption="Преглед на полиси" EmptyDataText="Нема записи кои го задоволуваат критериумот на пребарување!"
                            RowStyle-CssClass="row" CssClass="grid" GridLines="None">
                            <RowStyle CssClass="row"></RowStyle>
                            <Columns>
                                <asp:TemplateField HeaderText="Број на полиса">
                                    <ItemTemplate>
                                        <asp:Label ID="tbPolicyNumber" runat="server" ReadOnly="true" Text='<%#Eval("Distribution.PolicyNumber") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Осигурителна компанија">
                                    <ItemTemplate>
                                        <asp:Label ID="tbInsuranceCompany" runat="server" ReadOnly="true" Text='<%#Eval("Distribution.InsuranceCompany.ShortName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Подкласа на осигурување">
                                    <ItemTemplate>
                                        <asp:Label ID="tbInsuranceSubType" runat="server" ReadOnly="true" Text='<%#Eval("Distribution.InsuranceSubType.ShortDescription") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <PagerStyle HorizontalAlign="Center" />
                            <%--<SelectedRowStyle CssClass="rowSelected" />--%>
                            <PagerSettings FirstPageText="<< Прва " PreviousPageText="< Претходна " NextPageText=" Следна >"
                                LastPageText=" Последна >>" Mode="NextPreviousFirstLast" />
                        </asp:GridView>
                        <asp:ObjectDataSource ID="odsDistributionDocumentItems" runat="server" SelectMethod="GetByDistributionDocument"
                            TypeName="Broker.DataAccess.DistributionDocumentItem" DataObjectTypeName="Broker.DataAccess.DistributionDocumentItem"
                            OnSelecting="odsDistributionDocumentItems_Selecting">
                            <%--<SelectParameters>
                                <asp:ControlParameter ControlID="GXGridView1" Name="distributionDocumentID" PropertyName="SelectedValue"
                                    Type="Int32" />
                            </SelectParameters>--%>
                        </asp:ObjectDataSource>
                    </div>
                </div>
            </div>
        </asp:View>
        <asp:View ID="viewReport" runat="server">
            <div class="paddingKontroli">
                <cc1:ReportControl ID="reportControl" runat="server" TypeName="Broker.DataAccess.ViewDistributionDocument"
                    GridViewID="GXGridView1" SearchControlID="SearchControl1">
                    <cc1:PrintItem HeaderText="Број" PropertyName="DocumentNumber" />
                    <cc1:PrintItem HeaderText="Дата" PropertyName="DocumentDate" />
                    <cc1:PrintItem HeaderText="Тип на документ" PropertyName="DistributionDocTypeName" />
                    <cc1:PrintItem HeaderText="Статус" PropertyName="DistributionDocumentStatusName" />
                    <cc1:PrintItem HeaderText="Филијала (шифра)" PropertyName="BranchCode" />
                    <cc1:PrintItem HeaderText="Филијала" PropertyName="BranchName" />
                </cc1:ReportControl>
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>
