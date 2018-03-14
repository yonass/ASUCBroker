<%@ Page Title="Преглед на задолжени полиси" Language="C#" MasterPageFile="~/MasterPage.master"
    AutoEventWireup="true" CodeFile="Distributions.aspx.cs" Inherits="SEAdmin_Distributions" %>

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
                            Caption="Задолжувања" EmptyDataText="Нема записи кои го задоволуваат критериумот на пребарување!"
                            OnRowCommand="GXGridView1_RowCommand" RowStyle-CssClass="row" CssClass="grid"
                            GridLines="None">
                            <Columns>
                                <asp:BoundField HeaderText="Осигурителна компанија" DataField="InsuranceCompanyName"
                                    SortExpression="InsuranceCompanyName" />
                                <asp:BoundField HeaderText="Број на полиса" DataField="PolicyNumber" SortExpression="PolicyNumber" />
                                <asp:CheckBoxField HeaderText="Искор- истена" DataField="IsUsed" SortExpression="IsUsed" />
                                <asp:BoundField HeaderText="Статус" DataField="DistributionStatusName" SortExpression="DistributionStatusName" />
                                <asp:BoundField HeaderText="Подкласа на осигурување" DataField="InsuranceSubTypeShortDescription"
                                    SortExpression="InsuranceSubTypeShortDescription" />
                                <asp:BoundField HeaderText="Дата на задолжување" DataField="Date" SortExpression="Date"
                                    DataFormatString="{0:d}" />
                                <asp:BoundField HeaderText="Филијала" DataField="BranchCode" SortExpression="BranchCode" />
                            </Columns>
                            <PagerStyle HorizontalAlign="Center" />
                            <SelectedRowStyle CssClass="rowSelected" />
                            <PagerSettings FirstPageText="<< Прва " PreviousPageText="< Претходна " NextPageText=" Следна >"
                                LastPageText=" Последна >>" Mode="NextPreviousFirstLast" />
                        </cc1:GXGridView>
                        <cc1:GridViewDataSource ID="gvDataSource" runat="server" TypeName="Broker.DataAccess.ViewDistribution"
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
                                        <cc1:FilterItem FieldName="Број на полиса" PropertyName="PolicyNumber" Comparator="StringComparators"
                                            DataType="String" />
                                        <cc1:FilterItem FieldName="Искористена" PropertyName="IsUsed" Comparator="BooleanComparators"
                                            DataType="Boolean" />
                                        <cc1:FilterItem FieldName="Статус" PropertyName="DistributionStatusName" Comparator="StringComparators"
                                            DataType="String" />
                                        <cc1:FilterItem FieldName="Класа на осиг." PropertyName="InsuranceTypeCode" Comparator="StringComparators"
                                            DataType="String" />
                                        <cc1:FilterItem FieldName="Подкласа на осиг." PropertyName="InsuranceSubTypeCode"
                                            Comparator="StringComparators" DataType="String" />
                                        <cc1:FilterItem FieldName="Дата на задол." PropertyName="Date" Comparator="NumericComparators"
                                            DataType="DateTime" />
                                        <cc1:FilterItem FieldName="Агент" PropertyName="UserName" Comparator="StringComparators"
                                            DataType="String" />
                                        <cc1:FilterItem FieldName="Филијала (шифра)" PropertyName="BranchCode" Comparator="StringComparators"
                                            DataType="String" />
                                        <cc1:FilterItem FieldName="Филијала" PropertyName="BranchName" Comparator="StringComparators"
                                            DataType="String" />
                                    </cc1:FilterControl>
                                </div>
                                <cc1:FilterDataSource ID="odsFilterGridView" runat="server" TypeName="Broker.DataAccess.ViewDistribution">
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
                            <asp:Label ID="lblDocumentNumber" runat="server" Text="Број на документ"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="tbDocumentNumber" runat="server" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblDistributionDocType" runat="server" Text="Тип на документ"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlDistributionDocType" runat="server" DataTextField="Name"
                                DataValueField="ID" Width="300px" DataSourceID="odsDistributionDocType" AutoPostBack="true"
                                OnSelectedIndexChanged="ddlDistributionDocType_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:ObjectDataSource ID="odsDistributionDocType" runat="server" SelectMethod="Select"
                                TypeName="Broker.DataAccess.DistributionDocType"></asp:ObjectDataSource>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblBranch" runat="server" Text="Филијала"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlBranches" runat="server" DataTextField="Name" DataValueField="ID"
                                Width="300px" DataSourceID="odsBranches" AutoPostBack="true" OnSelectedIndexChanged="ddlBranches_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:ObjectDataSource ID="odsBranches" runat="server" SelectMethod="Select" TypeName="Broker.DataAccess.Branch">
                            </asp:ObjectDataSource>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblUser" runat="server" Text="Агент"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlUsers" runat="server" DataTextField="Name" DataValueField="ID"
                                DataSourceID="UsersDataSource" Width="200px">
                            </asp:DropDownList>
                            <asp:ObjectDataSource ID="UsersDataSource" runat="server" SelectMethod="GetUsersByBranch"
                                TypeName="Broker.DataAccess.User">
                                <SelectParameters>
                                    <asp:ControlParameter Type="Int32" ControlID="ddlBranches" PropertyName="SelectedValue"
                                        Name="branchID" />
                                    <asp:ControlParameter Type="Int32" ControlID="ddlDistributionDocType" PropertyName="SelectedValue"
                                        Name="distributionDocTypeID" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblInsuranceCompany" runat="server" Text="Осигурителна Компанија"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlInsuranceCompany" runat="server" DataTextField="Name" DataValueField="ID"
                                Width="300px" DataSourceID="InsuranceCompaniesDataSource" AutoPostBack="true"
                                OnSelectedIndexChanged="ddlCompany_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:ObjectDataSource ID="InsuranceCompaniesDataSource" runat="server" SelectMethod="GetByDealsAndPacketsAndLifeDeals"
                                TypeName="Broker.DataAccess.InsuranceCompany"></asp:ObjectDataSource>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblInsuranceType" runat="server" Text="Класа на осигурување"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlInsuranceType" runat="server" DataTextField="Name" DataValueField="ID"
                                Width="400px" DataSourceID="InsuranceTypeDataSource" AutoPostBack="true" OnSelectedIndexChanged="ddlInsuranceType_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:ObjectDataSource ID="InsuranceTypeDataSource" runat="server" SelectMethod="GetForDistributionByCompanyWithLife"
                                TypeName="Broker.DataAccess.InsuranceType">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="ddlInsuranceCompany" Name="companyID" PropertyName="SelectedValue"
                                        Type="Int32" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblInsuranceSubType" runat="server" Text="Подкласа на осигурување"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlInsuranceSubType" runat="server" DataTextField="Description"
                                DataValueField="ID" DataSourceID="InsuranceSubTypeDataSource" Width="400px">
                            </asp:DropDownList>
                            <asp:ObjectDataSource ID="InsuranceSubTypeDataSource" runat="server" SelectMethod="GetForDistributionByInsuranceTypeAndCompanyWithLife"
                                TypeName="Broker.DataAccess.InsuranceSubType">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="ddlInsuranceType" Name="insuranceTypeID" PropertyName="SelectedValue"
                                        Type="Int32" />
                                    <asp:ControlParameter ControlID="ddlInsuranceCompany" Name="companyID" PropertyName="SelectedValue"
                                        Type="Int32" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Опис
                        </td>
                        <td>
                            <asp:TextBox ID="tbDescription" ReadOnly="true" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblInsuranceCompanyPrefix" Text="Префикс" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ReadOnly="true" runat="server" ID="tbInsuranceCompanyPrefix"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblStartNumber" runat="server" Text="Почетен Број"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="tbStartNumber" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvStartNumber" runat="server" ErrorMessage="*" Display="Dynamic"
                                ControlToValidate="tbStartNumber"></asp:RequiredFieldValidator>
                            <asp:CompareValidator ID="cvStartNumber" runat="server" ErrorMessage="*" Display="Dynamic"
                                ControlToValidate="tbStartNumber" Operator="DataTypeCheck" Type="Integer"></asp:CompareValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblNumberOfPolicies" runat="server" Text="Краен Број"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="tbEndNumber" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvEndNumber" runat="server" ErrorMessage="*" Display="Dynamic"
                                ControlToValidate="tbEndNumber"></asp:RequiredFieldValidator>
                            <asp:CompareValidator ID="cvEndNumber" runat="server" ErrorMessage="*" Display="Dynamic"
                                ControlToValidate="tbEndNumber" Operator="DataTypeCheck" Type="Integer"></asp:CompareValidator>
                            <super:EntityCallOutValidator ID="DistributionValidator" PropertyName="PolicyNumber"
                                runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="btnCreate" runat="server" OnClick="btnCreate_Click" Text="Креирај" />
                            <asp:Button ID="btnPrintDocument" runat="server" OnClick="btnPrintDocument_Click"
                                Text="Печати" Enabled="false" />
                        </td>
                        <td>
                            <asp:Label ID="lblError" runat="server" Text="" Visible="False" Font-Bold="True"
                                ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                </table>
                <asp:GridView ID="gvNewDistrirutions" runat="server" AllowPaging="True" AllowSorting="True"
                    AutoGenerateColumns="False" Caption="Новозадолжени полиси" EmptyDataText="Нема записи кои го задоволуваат критериумот на пребарување!"
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
                        <asp:TemplateField HeaderText="Број на полиса">
                            <ItemTemplate>
                                <asp:Label ID="tbPolicyNumber" ReadOnly="true" Text='<%#Eval("PolicyNumber") %>'
                                    runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Класа на осигурување">
                            <ItemTemplate>
                                <asp:Label ID="tbInsuranceType" ReadOnly="true" Text='<%#Eval("InsuranceSubType.InsuranceType.Name") %>'
                                    runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Подкласа на осигурување">
                            <ItemTemplate>
                                <asp:Label ID="tbInsuranceSubType" ReadOnly="true" Text='<%#Eval("InsuranceSubType.Description") %>'
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
                    <cc1:SearchItem FieldName="Број на полиса" PropertyName="PolicyNumber" Comparator="StringComparators"
                        DataType="String" />
                    <cc1:SearchItem FieldName="Искористена" PropertyName="IsUsed" Comparator="BooleanComparators"
                        DataType="Boolean" />
                    <cc1:SearchItem FieldName="Статус" PropertyName="DistributionStatusName" Comparator="StringComparators"
                        DataType="String" />
                    <cc1:SearchItem FieldName="Класа на осигурување" PropertyName="InsuranceTypeCode"
                        Comparator="StringComparators" DataType="String" />
                    <cc1:SearchItem FieldName="Подкласа на осигурување" PropertyName="InsuranceSubTypeCode"
                        Comparator="StringComparators" DataType="String" />
                    <cc1:SearchItem FieldName="Дата на задолжување" PropertyName="Date" Comparator="NumericComparators"
                        DataType="DateTime" />
                    <cc1:SearchItem FieldName="Агент" PropertyName="UserName" Comparator="StringComparators"
                        DataType="String" />
                </cc1:SearchControl>
                <cc1:GridViewDataSource ID="odsSearch" runat="server" TypeName="Broker.DataAccess.ViewDistribution"
                    SelectCountMethod="SelectSearchCountCached" SelectMethod="SelectSearch">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="SearchControl1" PropertyName="SearchArguments" Name="sArgument" />
                    </SelectParameters>
                </cc1:GridViewDataSource>
            </div>
        </asp:View>
        <asp:View ID="viewReport" runat="server">
            <div class="paddingKontroli">
                <cc1:ReportControl ID="reportControl" runat="server" TypeName="Broker.DataAccess.ViewDistribution"
                    GridViewID="gvDistributions" SearchControlID="SearchControl1">
                    <cc1:PrintItem HeaderText="Осигурителна компанија" PropertyName="InsuranceCompanyName" />
                    <cc1:PrintItem HeaderText="Број на полиса" PropertyName="PolicyNumber" />
                    <cc1:PrintItem HeaderText="Искористена" PropertyName="IsUsed" />
                    <cc1:PrintItem HeaderText="Статус" PropertyName="DistributionStatusName" />
                    <cc1:PrintItem HeaderText="Класа на осигурување" PropertyName="InsuranceTypeCode" />
                    <cc1:PrintItem HeaderText="Подкласа на осигурување" PropertyName="InsuranceSubTypeCode" />
                    <cc1:PrintItem HeaderText="Дата на задолжување" PropertyName="Date" />
                    <cc1:PrintItem HeaderText="Агент" PropertyName="UserName" />
                </cc1:ReportControl>
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>
