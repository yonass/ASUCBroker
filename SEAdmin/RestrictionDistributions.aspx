<%@ Page Title="Преглед на задолжени винкулации" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="RestrictionDistributions.aspx.cs" Inherits="SEAdmin_RestrictionDistributions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="kontroli">
        <div id="button01">
            <asp:Button ID="btnNew" runat="server" ToolTip="Нов запис" OnClick="btnNew_Click"
                CausesValidation="false" UseSubmitBehavior="false" CssClass="novZapis" BorderWidth="0px" />
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
    </div>
    <asp:MultiView ID="mvMain" runat="server">
        <asp:View ID="viewGrid" runat="server">
            <div id="tabeliFrame">
                <div id="header">
                    <div id="content">
                        Задолжувања
                    </div>
                </div>
                <div id="contentOuter">
                    <div id="contentInner">
                        <cc1:GXGridView ID="gvDistributions" runat="server" DataSourceID="gvDataSource" DataKeyNames="ID"
                            Width="100%" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"
                            Caption="Задолжувања на винкулации" EmptyDataText="Нема записи кои го задоволуваат критериумот на пребарување!"
                            OnRowCommand="GXGridView1_RowCommand" RowStyle-CssClass="row" CssClass="grid"
                            GridLines="None">
                            <Columns>
                                <asp:BoundField HeaderText="Осигурителна компанија" DataField="InsuranceCompanyName"
                                    SortExpression="InsuranceCompanyName" />
                                <asp:BoundField HeaderText="Број " DataField="Number" SortExpression="Number" />
                                <asp:CheckBoxField HeaderText="Искористена" DataField="IsUsed" SortExpression="IsUsed" />
                                <asp:BoundField HeaderText="Дата на креирање" DataField="Date" DataFormatString="{0:d}"
                                    SortExpression="Date" />
                               <%-- <asp:BoundField HeaderText="Тип на документ" DataField="DocumentTypeDescription"
                                    SortExpression="DocumentTypeDescription" />--%>
                                <%--<asp:BoundField HeaderText="Агент" DataField="UserName" SortExpression="UserName" />--%>
                            </Columns>
                            <PagerStyle HorizontalAlign="Center" />
                            <SelectedRowStyle CssClass="rowSelected" />
                            <PagerSettings FirstPageText="<< Прва " PreviousPageText="< Претходна " NextPageText=" Следна >"
                                LastPageText=" Последна >>" Mode="NextPreviousFirstLast" />
                        </cc1:GXGridView>
                        <cc1:GridViewDataSource ID="gvDataSource" runat="server" TypeName="Broker.DataAccess.ViewRightRestrictionDistribution"
                            OldValuesParameterFormatString="oldEntity">
                        </cc1:GridViewDataSource>
                        <div class="pager">
                            <div class="container">
                                <div style="float: left;">
                                    <cc1:PagerControl ID="myGridPager" runat="server" ControlToPage="gvDistributions" />
                                </div>
                                <div style="float: right;">
                                    <cc1:FilterControl ID="FilterControl1" runat="server" GridViewToFilter="gvDistributions"
                                        FilterDataSourceID="odsFilterGridView" OnFilter="FilterControl1_Filter">
                                        <cc1:FilterItem FieldName="Осиг. компанија" PropertyName="InsuranceCompanyName" Comparator="StringComparators"
                                            DataType="String" />
                                        <cc1:FilterItem FieldName="Број" PropertyName="Number" Comparator="StringComparators"
                                            DataType="String" />
                                        <cc1:FilterItem FieldName="Искористена" PropertyName="IsUsed" Comparator="BooleanComparators"
                                            DataType="Boolean" />
                                        <cc1:FilterItem FieldName="Дата" PropertyName="Date" Comparator="NumericComparators"
                                            DataType="DateTime" />
                                        <cc1:FilterItem FieldName="Тип на док.(код)" PropertyName="DocumentTypeCode" Comparator="StringComparators"
                                            DataType="String" />
                                        <cc1:FilterItem FieldName="Тип на документ" PropertyName="DocumentTypeDescription"
                                            Comparator="StringComparators" DataType="String" />
                                    </cc1:FilterControl>
                                </div>
                                <cc1:FilterDataSource ID="odsFilterGridView" runat="server" TypeName="Broker.DataAccess.ViewRightRestrictionDistribution">
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
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblDocumentType" Text="Тип на документ" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlDocumentType" runat="server" DataSourceID="odsDocumentTypes"
                                DataTextField="Description" DataValueField="ID">
                            </asp:DropDownList>
                            <asp:ObjectDataSource ID="odsDocumentTypes" runat="server" SelectMethod="Select"
                                TypeName="Broker.DataAccess.DistributionDocumentType"></asp:ObjectDataSource>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblUser" runat="server" Text="Агент"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlUsers" runat="server" DataTextField="UserName" DataValueField="ID"
                                DataSourceID="UsersDataSource" Width="200px">
                            </asp:DropDownList>
                            <asp:ObjectDataSource ID="UsersDataSource" runat="server" SelectMethod="GetAllActiveUsers"
                                TypeName="Broker.DataAccess.User"></asp:ObjectDataSource>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblInsuranceCompany" runat="server" Text="Осигурителна Компанија"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlInsuranceCompany" runat="server" DataTextField="Name" DataValueField="ID"
                                Width="300px" DataSourceID="InsuranceCompaniesDataSource">
                            </asp:DropDownList>
                            <asp:ObjectDataSource ID="InsuranceCompaniesDataSource" runat="server" SelectMethod="GetByDealsAndPackets"
                                TypeName="Broker.DataAccess.InsuranceCompany"></asp:ObjectDataSource>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblStartNumber" runat="server" Text="Почетен Број"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="tbStartNumber" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblEndNumber" runat="server" Text="Краен Број"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="tbEndNumber" runat="server"></asp:TextBox>
                            <%--<super:EntityCallOutValidator ID="DistributionValidator" PropertyName="PolicyNumber"
                                runat="server" />--%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="btnCreate" runat="server" OnClick="btnCreate_Click" Text="Креирај" />
                        </td>
                        <td>
                            <asp:Label ID="lblError" runat="server" Text="Бројот е веќе задолжен" Visible="False"
                                Font-Bold="True" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                </table>
                <asp:GridView ID="gvNewDistrirutions" runat="server" AllowPaging="True" AllowSorting="True"
                    AutoGenerateColumns="False" Caption="Новозадолжени броеви" EmptyDataText="Нема записи кои го задоволуваат критериумот на пребарување!"
                    RowStyle-CssClass="row" CssClass="grid" GridLines="None" OnPageIndexChanged="gvNewDistrirutions_PageIndexChanged"
                    OnPageIndexChanging="gvNewDistrirutions_PageIndexChanging">
                    <RowStyle CssClass="row"></RowStyle>
                    <Columns>
                        <asp:TemplateField HeaderText="Осигурителна компанија">
                            <ItemTemplate>
                                <asp:Label ID="tbInsuranceCompany" ReadOnly="true" Text='<%#Eval("InsuranceCompany.Name") %>'
                                    runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Број">
                            <ItemTemplate>
                                <asp:Label ID="tbNumber" ReadOnly="true" Text='<%#Eval("Number") %>' runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Тип на документ">
                            <ItemTemplate>
                                <asp:Label ID="tbDocumentType" ReadOnly="true" Text='<%#Eval("DistributionDocumentType.Description") %>'
                                    runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <PagerStyle HorizontalAlign="Center" />
                    <PagerSettings FirstPageText="<< Прва " PreviousPageText="< Претходна " NextPageText=" Следна >"
                        LastPageText=" Последна >>" Mode="NextPreviousFirstLast" />
                </asp:GridView>
            </div>
        </asp:View>
        <asp:View ID="viewSearch" runat="server">
            <div class="paddingKontroli">
                <cc1:SearchControl ID="SearchControl1" runat="server" SearchDataSourceID="odsSearch"
                    GridViewToSearch="gvDistributions" OnSearch="SearchControl1_Search">
                    <cc1:SearchItem FieldName="Осигурителна компанија" PropertyName="InsuranceCompanyName"
                        Comparator="StringComparators" DataType="String" />
                    <cc1:SearchItem FieldName="Број на винкулација" PropertyName="Number" Comparator="StringComparators"
                        DataType="String" />
                    <cc1:SearchItem FieldName="Искористена" PropertyName="IsUsed" Comparator="BooleanComparators"
                        DataType="Boolean" />
                    <cc1:SearchItem FieldName="Дата" PropertyName="Date" Comparator="NumericComparators"
                        DataType="DateTime" />
                    <cc1:SearchItem FieldName="Тип на док.(код)" PropertyName="DocumentTypeCode" Comparator="StringComparators"
                        DataType="String" />
                    <cc1:SearchItem FieldName="Тип на документ" PropertyName="DocumentTypeDescription"
                        Comparator="StringComparators" DataType="String" />
                </cc1:SearchControl>
                <cc1:GridViewDataSource ID="odsSearch" runat="server" TypeName="Broker.DataAccess.ViewRightRestrictionDistribution"
                    SelectCountMethod="SelectSearchCountCached" SelectMethod="SelectSearch">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="SearchControl1" PropertyName="SearchArguments" Name="sArgument" />
                    </SelectParameters>
                </cc1:GridViewDataSource>
            </div>
        </asp:View>
        <asp:View ID="viewReport" runat="server">
            <div class="paddingKontroli">
                <cc1:ReportControl ID="reportControl" runat="server" TypeName="Broker.DataAccess.ViewRightRestrictionDistribution"
                    GridViewID="gvDistributions" SearchControlID="SearchControl1">
                    <cc1:PrintItem HeaderText="Осигурителна компанија" PropertyName="InsuranceCompanyName" />
                    <cc1:PrintItem HeaderText="Број на полиса" PropertyName="PolicyNumber" />
                    <cc1:PrintItem HeaderText="Искористена" PropertyName="IsUsed" />
                    <cc1:PrintItem HeaderText="Дата" PropertyName="Date" />
                    <cc1:PrintItem HeaderText="Тип на документ" PropertyName="DocumentTypeDescription" />
                </cc1:ReportControl>
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>
