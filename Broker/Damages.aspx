<%@ Page Title="Штети" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Damages.aspx.cs" Inherits="Broker_Damages" UICulture="mk-MK" Culture="mk-MK" %>

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
            <asp:Button ID="btnEdit" runat="server" ToolTip="Измени" OnClick="btnEdit_Click"
                CausesValidation="false" Enabled="false" UseSubmitBehavior="false" CssClass="izmeni"
                BorderWidth="0px" />
        </div>
        <%--<div id="button03">
            <asp:Button ID="btnDelete" runat="server" OnClick="btnDelete_Click" CausesValidation="false"
                Enabled="false" UseSubmitBehavior="false" CssClass="izbrisi" BorderWidth="0px" />
        </div>--%>
        <div id="button03">
            <asp:Button ID="btnPreview" runat="server" ToolTip="Освежи" OnClick="btnPreview_Click"
                CausesValidation="false" UseSubmitBehavior="false" CssClass="osvezi" BorderWidth="0px" />
        </div>
        <div id="button04">
            <asp:Button ID="btnReport" runat="server" ToolTip="Извештај" CausesValidation="false"
                UseSubmitBehavior="false" OnClick="btnReport_Click" CssClass="izvestaj" BorderWidth="0px" />
        </div>
        <div id="button05">
            <asp:Button ID="btnSearch" runat="server" ToolTip="Пребарај" OnClick="btnSearch_Click"
                CausesValidation="false" UseSubmitBehavior="false" CssClass="prebaraj" BorderWidth="0px" />
        </div>
        <div id="button06">
            <asp:Button ID="btnAttachments" runat="server" ToolTip="Документи" CssClass="dokumenti"
                OnClick="btnAttachments_Click" UseSubmitBehavior="false" CausesValidation="false"
                BorderWidth="0px" Enabled="false" />
        </div>
    </div>
    <asp:MultiView ID="mvMain" runat="server">
        <asp:View ID="viewGrid" runat="server">
            <div id="tabeliFrame">
                <div id="header">
                    <div id="content">
                        Штети
                    </div>
                </div>
                <div id="contentOuter">
                    <div id="contentInner">
                        <cc1:GXGridView ID="GXGridView1" runat="server" DataSourceID="odsGridView" DataKeyNames="ID"
                            Width="100%" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"
                            Caption="Штети" EmptyDataText="Нема записи кои го задоволуваат критериумот на пребарување!"
                            OnRowCommand="GXGridView1_RowCommand" RowStyle-CssClass="row" CssClass="grid"
                            GridLines="None">
                            <Columns>
                                <asp:BoundField HeaderText="Број на штета" DataField="DamageNumber" SortExpression="DamageNumber" />
                                <asp:BoundField HeaderText="Полиса" DataField="PolicyNumber" SortExpression="PolicyNumber" />
                                <asp:BoundField HeaderText="Дата на штета" DataField="DamageDate" SortExpression="DamageDate"
                                    DataFormatString="{0:d}" />
                                <asp:BoundField HeaderText="Дата на пријава" DataField="AplicationDate" SortExpression="AplicationDate"
                                    DataFormatString="{0:d}" />
                                <%--<asp:BoundField HeaderText="Место на штета" DataField="DamagePlace" SortExpression="DamagePlace" />--%>
                                <asp:BoundField HeaderText="Проценета вредност" DataField="EstimatedDamageValue"
                                    SortExpression="EstimatedDamageValue" />
                                <asp:BoundField HeaderText="Ликвидирана вредност" DataField="LiquedatedValue" SortExpression="LiquedatedValue" />
                                <asp:BoundField HeaderText="Исплатена вредност" DataField="PaidValue" SortExpression="PaidValue" />
                                <%--<asp:BoundField HeaderText="Корисник" DataField="UserName" SortExpression="UserName" />--%>
                            </Columns>
                            <PagerStyle HorizontalAlign="Center" />
                            <SelectedRowStyle CssClass="rowSelected" />
                            <PagerSettings FirstPageText="<< Прва " PreviousPageText="< Претходна " NextPageText=" Следна >"
                                LastPageText=" Последна >>" Mode="NextPreviousFirstLast" />
                        </cc1:GXGridView>
                        <cc1:GridViewDataSource ID="odsGridView" runat="server" TypeName="Broker.DataAccess.ViewDamage"
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
                                        <cc1:FilterItem FieldName="Број на штета" PropertyName="DamageNumber" Comparator="StringComparators"
                                            DataType="String" />
                                        <cc1:FilterItem FieldName="Полиса" PropertyName="PolicyNumber" Comparator="StringComparators"
                                            DataType="String" />
                                        <cc1:FilterItem FieldName="Дата на штета" PropertyName="DamageDate" Comparator="NumericComparators"
                                            DataType="DateTime" />
                                        <cc1:FilterItem FieldName="Дата на пријава" PropertyName="AplicationDate" Comparator="NumericComparators"
                                            DataType="DateTime" />
                                        <cc1:FilterItem FieldName="Место на штета" PropertyName="DamagePlace" Comparator="StringComparators"
                                            DataType="String" />
                                        <cc1:FilterItem FieldName="Проценета вредност" PropertyName="EstimatedDamageValue"
                                            Comparator="NumericComparators" DataType="Decimal" />
                                        <cc1:FilterItem FieldName="Ликвидирана вредност" PropertyName="LiquedatedValue" Comparator="NumericComparators"
                                            DataType="Decimal" />
                                        <cc1:FilterItem FieldName="Исплатена вредност" PropertyName="PaidValue" Comparator="NumericComparators"
                                            DataType="Decimal" />
                                        <cc1:FilterItem FieldName="Корисник" PropertyName="UserName" Comparator="StringComparators"
                                            DataType="String" />
                                    </cc1:FilterControl>
                                </div>
                                <cc1:FilterDataSource ID="odsFilterGridView" runat="server" TypeName="Broker.DataAccess.ViewDamage">
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
                <asp:DetailsView ID="DetailsView1" runat="server" DataSourceID="dvDataSource" DataKeyNames="ID"
                    AutoGenerateRows="False" OnItemCommand="DetailsView1_ItemCommand" OnItemDeleted="DetailsView1_ItemDeleted"
                    OnItemInserted="DetailsView1_ItemInserted" OnItemUpdated="DetailsView1_ItemUpdated"
                    OnModeChanging="DetailsView1_ModeChanging" OnItemInserting="DetailsView1_ItemInserting"
                    GridLines="None" OnItemUpdating="DetailsView1_ItemUpdating">
                    <Fields>
                        <asp:TemplateField>
                            <InsertItemTemplate>
                                <div class="subTitles">
                                    Нов запис во Штети
                                </div>
                            </InsertItemTemplate>
                            <EditItemTemplate>
                                <div class="subTitles">
                                    Измена на запис во Штети
                                </div>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <div class="subTitles">
                                    Бришење на запис од Штети
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <InsertItemTemplate>
                                <table>
                                    <tr>
                                        <td style="width: 150px;">
                                            <asp:Label ID="lblDamageNumber" Text="Број на штета" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tbDamageNumber" runat="server" Text='<%# Bind("DamageNumber") %>'
                                                MaxLength="50"></asp:TextBox>
                                            <super:EntityCallOutValidator ID="DamageNumberInsertValidator" PropertyName="DAMAGENUMBER_INSERT_EXIST"
                                                runat="server" />
                                            <asp:RequiredFieldValidator ID="rfvDamageNumber" runat="server" ControlToValidate="tbDamageNumber"
                                                Display="Dynamic" ErrorMessage="*">
                                            </asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                </table>
                            </InsertItemTemplate>
                            <EditItemTemplate>
                                <table>
                                    <tr>
                                        <td style="width: 150px;">
                                            <asp:Label ID="lblDamageNumber" Text="Број на штета" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tbDamageNumber" runat="server" Text='<%# Bind("DamageNumber") %>'
                                                MaxLength="50"></asp:TextBox>
                                            <super:EntityCallOutValidator ID="DamageNumberUpdateValidator" PropertyName="DAMAGENUMBER_UPDATE_EXIST"
                                                runat="server" />
                                            <asp:RequiredFieldValidator ID="rfvDamageNumber" runat="server" ControlToValidate="tbDamageNumber"
                                                Display="Dynamic" ErrorMessage="*">
                                            </asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                </table>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <table>
                                    <tr>
                                        <td style="width: 150px;">
                                            <asp:Label ID="lblDamageNumber" Text="Број на штета" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tbDamageNumber" runat="server" Text='<%# Bind("DamageNumber") %>'
                                                ReadOnly="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <InsertItemTemplate>
                                <table>
                                    <tr>
                                        <td style="width: 150px;">
                                            <asp:Label ID="lblPolicyNumber" runat="server" Text="Број на полиса"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tbPolicyNumber" runat="server" MaxLength="30"></asp:TextBox>
                                            <super:EntityCallOutValidator ID="PolicyInsertValidator" PropertyName="POLICYNUMBER_INSERT_DOESNOT_EXISTS"
                                                runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 150px;">
                                            <asp:Label ID="lblInsuranceType" runat="server" Text="Класа:"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlInsuranceTypes" runat="server" AutoPostBack="True" Width="400px"
                                                DataSourceID="odsInsuranceTypes" DataTextField="Name" DataValueField="ID" OnSelectedIndexChanged="ddlInsuranceTypes_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <asp:ObjectDataSource ID="odsInsuranceTypes" runat="server" SelectMethod="Select"
                                                TypeName="Broker.DataAccess.InsuranceType"></asp:ObjectDataSource>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 150px;">
                                            <asp:Label ID="lblInsuranceSubType" runat="server" Text="Подкласа:"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlInsuranceSubTypes" runat="server" Width="400px" DataSourceID="odsInsuranceSubTypes"
                                                DataTextField="Description" DataValueField="ID">
                                            </asp:DropDownList>
                                            <asp:ObjectDataSource ID="odsInsuranceSubTypes" runat="server" SelectMethod="GetByInsuranceType"
                                                TypeName="Broker.DataAccess.InsuranceSubType">
                                                <SelectParameters>
                                                    <asp:ControlParameter ControlID="ddlInsuranceTypes" Name="insuranceTypeID" PropertyName="SelectedValue"
                                                        Type="Int32" />
                                                </SelectParameters>
                                            </asp:ObjectDataSource>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 150px;">
                                            <asp:Label ID="lblInsuranceCompany" runat="server" Text="Осигурителна компанија:"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlInsuranceCompanies" runat="server" Width="200px" DataSourceID="odsInsuranceCompanies"
                                                DataTextField="Name" DataValueField="ID">
                                            </asp:DropDownList>
                                            <asp:ObjectDataSource ID="odsInsuranceCompanies" runat="server" SelectMethod="Select"
                                                TypeName="Broker.DataAccess.InsuranceCompany"></asp:ObjectDataSource>
                                        </td>
                                    </tr>
                                </table>
                            </InsertItemTemplate>
                            <ItemTemplate>
                                <table>
                                    <tr>
                                        <td style="width: 150px;">
                                            <asp:Label ID="lblPolicyNumber" runat="server" Text="Број на полиса"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tbOrderNumber" runat="server" Text='<%# Eval("PolicyItem.PolicyNumber") %>'
                                                ReadOnly="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 150px;">
                                            <asp:Label ID="lblInsuranceType" runat="server" Text="Класа:"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tbInsuranceType" runat="server" Width="400px" Text='<%# Eval("PolicyItem.InsuranceSubType.InsuranceType.Name") %>'
                                                ReadOnly="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 150px;">
                                            <asp:Label ID="lblInsuranceSubTypes" runat="server" Text="Подкласа:"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tbInsuranceSubTypes" runat="server" Width="400px" Text='<%# Eval("PolicyItem.InsuranceSubType.Description") %>'
                                                ReadOnly="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 150px;">
                                            <asp:Label ID="lblInsuranceCompany" runat="server" Text="Осигурителна компанија:"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tbInsuranceCompany" runat="server" Width="200px" Text='<%# Eval("PolicyItem.Policy.InsuranceCompany.Name") %>'
                                                ReadOnly="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <table>
                                    <tr>
                                        <td style="width: 150px;">
                                            <asp:Label ID="lblPolicyNumber" runat="server" Text="Број на полиса"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tbPolicyNumber" ReadOnly="true" runat="server" MaxLength="30" Text='<%# Eval("PolicyItem.PolicyNumber") %>'></asp:TextBox>
                                            <super:EntityCallOutValidator ID="PolicyUpdateValidator" PropertyName="POLICYNUMBER_UPDATE_DOESNOT_EXISTS"
                                                runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 150px;">
                                            <asp:Label ID="lblInsuranceType" runat="server" Text="Класа:"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlInsuranceTypes" Enabled="false" runat="server" AutoPostBack="True"
                                                Width="400px" DataSourceID="odsInsuranceTypes" DataTextField="Name" DataValueField="ID"
                                                SelectedValue='<%# Eval("PolicyItem.InsuranceSubType.InsuranceTypeID") %>' OnSelectedIndexChanged="ddlInsuranceTypes_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <asp:ObjectDataSource ID="odsInsuranceTypes" runat="server" SelectMethod="Select"
                                                TypeName="Broker.DataAccess.InsuranceType"></asp:ObjectDataSource>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 150px;">
                                            <asp:Label ID="lblInsuranceSubTypes" runat="server" Text="Подкласа:"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlInsuranceSubTypes" Enabled="false" runat="server" Width="400px"
                                                DataSourceID="odsInsuranceSubTypes" DataTextField="Description" SelectedValue='<%# Eval("PolicyItem.InsuranceSubTypeID") %>'
                                                DataValueField="ID">
                                            </asp:DropDownList>
                                            <asp:ObjectDataSource ID="odsInsuranceSubTypes" runat="server" SelectMethod="GetByInsuranceType"
                                                TypeName="Broker.DataAccess.InsuranceSubType">
                                                <SelectParameters>
                                                    <asp:ControlParameter ControlID="ddlInsuranceTypes" Name="insuranceTypeID" PropertyName="SelectedValue"
                                                        Type="Int32" />
                                                </SelectParameters>
                                            </asp:ObjectDataSource>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 150px;">
                                            <asp:Label ID="lblInsuranceCompany" runat="server" Text="Осигурителна компанија:"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlInsuranceCompanies" Enabled="false" runat="server" Width="200px"
                                                DataSourceID="odsInsuranceCompanies" DataTextField="Name" DataValueField="ID"
                                                SelectedValue='<%# Eval("PolicyItem.Policy.InsuranceCompanyID") %>'>
                                            </asp:DropDownList>
                                            <asp:ObjectDataSource ID="odsInsuranceCompanies" runat="server" SelectMethod="Select"
                                                TypeName="Broker.DataAccess.InsuranceCompany"></asp:ObjectDataSource>
                                        </td>
                                    </tr>
                                </table>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <table>
                                    <tr>
                                        <td style="width: 150px;">
                                            <asp:Label ID="lblDamageDate" runat="server" Text="Дата на штета"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tbDamageDate" runat="server" Text='<%# Bind("DamageDate", "{0:d}") %>'
                                                ReadOnly="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <table>
                                    <tr>
                                        <td style="width: 150px;">
                                            <asp:Label ID="lblDamageDate" runat="server" Text="Дата на штета"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tbDamageDate" runat="server" Text='<%# Bind("DamageDate", "{0:d}") %>'></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvDamageDate" runat="server" ControlToValidate="tbDamageDate"
                                                ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                            <asp:CompareValidator ID="cvDamageDate" runat="server" ControlToValidate="tbDamageDate"
                                                ErrorMessage="(неправилен формат)" Display="Dynamic" Operator="DataTypeCheck"
                                                Type="Date"></asp:CompareValidator>
                                        </td>
                                    </tr>
                                </table>
                            </EditItemTemplate>
                            <InsertItemTemplate>
                                <table>
                                    <tr>
                                        <td style="width: 150px;">
                                            <asp:Label ID="lblDamageDate" runat="server" Text="Дата на штета"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tbDamageDate" runat="server" Text='<%# Bind("DamageDate", "{0:d}") %>'></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvDamageDate" runat="server" ControlToValidate="tbDamageDate"
                                                ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                            <asp:CompareValidator ID="cvDamageDate" runat="server" ControlToValidate="tbDamageDate"
                                                ErrorMessage="(неправилен формат)" Display="Dynamic" Operator="DataTypeCheck"
                                                Type="Date"></asp:CompareValidator>
                                        </td>
                                    </tr>
                                </table>
                            </InsertItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <table>
                                    <tr>
                                        <td style="width: 150px;">
                                            <asp:Label ID="lblAplicationDate" runat="server" Text="Дата на пријава"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tbAplicationDate" runat="server" Text='<%# Bind("AplicationDate", "{0:d}") %>'
                                                ReadOnly="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <table>
                                    <tr>
                                        <td style="width: 150px;">
                                            <asp:Label ID="lblAplicationDate" runat="server" Text="Дата на пријава"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tbAplicationDate" runat="server" Text='<%# Bind("AplicationDate", "{0:d}") %>'></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvAplicationDate" runat="server" ControlToValidate="tbAplicationDate"
                                                ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                            <asp:CompareValidator ID="cvAplicationDate" runat="server" ControlToValidate="tbAplicationDate"
                                                ErrorMessage="(неправилен формат)" Display="Dynamic" Operator="DataTypeCheck"
                                                Type="Date"></asp:CompareValidator>
                                        </td>
                                    </tr>
                                </table>
                            </EditItemTemplate>
                            <InsertItemTemplate>
                                <table>
                                    <tr>
                                        <td style="width: 150px;">
                                            <asp:Label ID="lblAplicationDate" runat="server" Text="Дата на пријава"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tbAplicationDate" runat="server" Text='<%# Bind("AplicationDate", "{0:d}") %>'></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvAplicationDate" runat="server" ControlToValidate="tbAplicationDate"
                                                ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                            <asp:CompareValidator ID="cvAplicationDate" runat="server" ControlToValidate="tbAplicationDate"
                                                ErrorMessage="(неправилен формат)" Display="Dynamic" Operator="DataTypeCheck"
                                                Type="Date"></asp:CompareValidator>
                                        </td>
                                    </tr>
                                </table>
                            </InsertItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <table>
                                    <tr>
                                        <td style="width: 150px;">
                                            <asp:Label ID="lblDamagePlace" runat="server" Text="Место на штета"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tbDamagePlace" runat="server" Text='<%# Bind("DamagePlace") %>'
                                                ReadOnly="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <table>
                                    <tr>
                                        <td style="width: 150px;">
                                            <asp:Label ID="lblDamagePlace" runat="server" Text="Место на штета"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tbDamagePlace" runat="server" Text='<%# Bind("DamagePlace") %>'
                                                MaxLength="250"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvDamagePlace" runat="server" ControlToValidate="tbDamagePlace"
                                                ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                </table>
                            </EditItemTemplate>
                            <InsertItemTemplate>
                                <table>
                                    <tr>
                                        <td style="width: 150px;">
                                            <asp:Label ID="lblDamagePlace" runat="server" Text="Место на штета"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tbDamagePlace" runat="server" Text='<%# Bind("DamagePlace") %>'
                                                MaxLength="250"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvDamagePlace" runat="server" ControlToValidate="tbDamagePlace"
                                                ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                </table>
                            </InsertItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <table>
                                    <tr>
                                        <td style="width: 150px;">
                                            <asp:Label ID="lblEstimatedDamageValue" runat="server" Text="Проценета вредност"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tbEstimatedDamageValue" runat="server" Text='<%# Bind("EstimatedDamageValue") %>'
                                                ReadOnly="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <table>
                                    <tr>
                                        <td style="width: 150px;">
                                            <asp:Label ID="lblEstimatedDamageValue" runat="server" Text="Проценета вредност"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tbEstimatedDamageValue" runat="server" Text='<%# Bind("EstimatedDamageValue") %>'></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvEstimatedDamageValue" runat="server" ControlToValidate="tbEstimatedDamageValue"
                                                ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                            <asp:CompareValidator ID="cvEstimatedDamageValue" runat="server" ControlToValidate="tbEstimatedDamageValue"
                                                ErrorMessage="(неправилен формат)" Display="Dynamic" Operator="DataTypeCheck"
                                                Type="Double"></asp:CompareValidator>
                                        </td>
                                    </tr>
                                </table>
                            </EditItemTemplate>
                            <InsertItemTemplate>
                                <table>
                                    <tr>
                                        <td style="width: 150px;">
                                            <asp:Label ID="lblEstimatedDamageValue" runat="server" Text="Проценета вредност"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tbEstimatedDamageValue" runat="server" Text='<%# Bind("EstimatedDamageValue") %>'></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvEstimatedDamageValue" runat="server" ControlToValidate="tbEstimatedDamageValue"
                                                ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                            <asp:CompareValidator ID="cvEstimatedDamageValue" runat="server" ControlToValidate="tbEstimatedDamageValue"
                                                ErrorMessage="(неправилен формат)" Display="Dynamic" Operator="DataTypeCheck"
                                                Type="Double"></asp:CompareValidator>
                                        </td>
                                    </tr>
                                </table>
                            </InsertItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <table>
                                    <tr>
                                        <td style="width: 150px;">
                                            <asp:Label ID="lblLiquedatedValue" runat="server" Text="Ликвидирана вредност"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tbLiquedatedValue" runat="server" Text='<%# Bind("LiquedatedValue") %>'
                                                ReadOnly="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <table>
                                    <tr>
                                        <td style="width: 150px;">
                                            <asp:Label ID="lblLiquedatedValue" runat="server" Text="Ликвидирана вредност"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tbLiquedatedValue" runat="server" Text='<%# Bind("LiquedatedValue") %>'></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvLiquedatedValue" runat="server" ControlToValidate="tbLiquedatedValue"
                                                ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                            <asp:CompareValidator ID="cvLiquedatedValue" runat="server" ControlToValidate="tbLiquedatedValue"
                                                ErrorMessage="(неправилен формат)" Display="Dynamic" Operator="DataTypeCheck"
                                                Type="Double"></asp:CompareValidator>
                                        </td>
                                    </tr>
                                </table>
                            </EditItemTemplate>
                            <InsertItemTemplate>
                                <table>
                                    <tr>
                                        <td style="width: 150px;">
                                            <asp:Label ID="lblLiquedatedValue" runat="server" Text="Ликвидирана вредност"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tbLiquedatedValue" runat="server" Text='<%# Bind("LiquedatedValue") %>'></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvLiquedatedValue" runat="server" ControlToValidate="tbLiquedatedValue"
                                                ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                            <asp:CompareValidator ID="cvLiquedatedValue" runat="server" ControlToValidate="tbLiquedatedValue"
                                                ErrorMessage="(неправилен формат)" Display="Dynamic" Operator="DataTypeCheck"
                                                Type="Double"></asp:CompareValidator>
                                        </td>
                                    </tr>
                                </table>
                            </InsertItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <table>
                                    <tr>
                                        <td style="width: 150px;">
                                            <asp:Label ID="lblPaidValue" runat="server" Text="Исплатена вредност"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tbPaidValue" runat="server" Text='<%# Bind("PaidValue") %>' ReadOnly="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <table>
                                    <tr>
                                        <td style="width: 150px;">
                                            <asp:Label ID="lblPaidValue" runat="server" Text="Исплатена вредност"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tbPaidValue" runat="server" Text='<%# Bind("PaidValue") %>'></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvPaidValue" runat="server" ControlToValidate="tbPaidValue"
                                                ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                            <asp:CompareValidator ID="cvPaidValue" runat="server" ControlToValidate="tbPaidValue"
                                                ErrorMessage="(неправилен формат)" Display="Dynamic" Operator="DataTypeCheck"
                                                Type="Double"></asp:CompareValidator>
                                        </td>
                                    </tr>
                                </table>
                            </EditItemTemplate>
                            <InsertItemTemplate>
                                <table>
                                    <tr>
                                        <td style="width: 150px;">
                                            <asp:Label ID="lblPaidValue" runat="server" Text="Исплатена вредност"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tbPaidValue" runat="server" Text='<%# Bind("PaidValue") %>'></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvPaidValue" runat="server" ControlToValidate="tbPaidValue"
                                                ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                            <asp:CompareValidator ID="cvPaidValue" runat="server" ControlToValidate="tbPaidValue"
                                                ErrorMessage="(неправилен формат)" Display="Dynamic" Operator="DataTypeCheck"
                                                Type="Double"></asp:CompareValidator>
                                        </td>
                                    </tr>
                                </table>
                            </InsertItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <InsertItemTemplate>
                                <table>
                                    <tr>
                                        <td style="width: 150px;">
                                            <asp:Label ID="lblDescription" runat="server" Text="Опис"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tbDescription" runat="server" Text='<%#Bind("Description") %>' Rows="4"
                                                TextMode="MultiLine"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </InsertItemTemplate>
                            <EditItemTemplate>
                                <table>
                                    <tr>
                                        <td style="width: 150px;">
                                            <asp:Label ID="lblDescription" runat="server" Text="Опис"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tbDescription" runat="server" Text='<%#Bind("Description") %>' Rows="4"
                                                TextMode="MultiLine"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <table>
                                    <tr>
                                        <td style="width: 150px;">
                                            <asp:Label ID="lblDescription" runat="server" Text="Опис"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tbDescription" runat="server" Text='<%#Bind("Description") %>' Rows="4"
                                                TextMode="MultiLine" ReadOnly="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Button ID="btnCancel" runat="server" CommandName="Cancel" Text="Откажи" CausesValidation="false" />
                                <%--<asp:Button ID="btnDelete" runat="server" Text="Избриши" OnClientClick="return confirm('Дали сте сигурни?')" />--%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Button ID="btnCancel" runat="server" CommandName="Cancel" Text="Откажи" CausesValidation="false" />
                                <asp:Button ID="btnUpdate" runat="server" CommandName="Update" Text="Измени" />
                            </EditItemTemplate>
                            <InsertItemTemplate>
                                <asp:Button ID="btnCancel" runat="server" CommandName="Cancel" Text="Откажи" CausesValidation="false" />
                                <asp:Button ID="btnInsert" runat="server" CommandName="Insert" Text="Внеси" />
                            </InsertItemTemplate>
                        </asp:TemplateField>
                    </Fields>
                </asp:DetailsView>
                <cc1:DetailsViewDataSource ID="dvDataSource" runat="server" TypeName="Broker.DataAccess.Damage"
                    ConflictDetection="CompareAllValues" DataObjectTypeName="Broker.DataAccess.Damage"
                    DeleteMethod="Delete" InsertMethod="Insert" SelectMethod="Get" UpdateMethod="Update"
                    OnUpdating="dvDataSource_Updating" OnUpdated="dvDataSource_Updated" OnInserted="dvDataSource_Inserted"
                    OnInserting="dvDataSource_Inserting" OldValuesParameterFormatString="oldEntity"
                    OnSelecting="dvDataSource_Selecting">
                    <UpdateParameters>
                        <asp:Parameter Name="oldEntity" Type="Object" />
                        <asp:Parameter Name="newEntity" Type="Object" />
                        <asp:Parameter Name="DamageDate" Type="DateTime" />
                        <asp:Parameter Name="AplicationDate" Type="DateTime" />
                        <asp:Parameter Name="LiquedatedValue" Type="Decimal" />
                        <asp:Parameter Name="PaidValue" Type="Decimal" />
                        <asp:Parameter Name="EstimatedDamageValue" Type="Decimal" />
                    </UpdateParameters>
                    <InsertParameters>
                        <asp:Parameter Name="DamageDate" Type="DateTime" />
                        <asp:Parameter Name="AplicationDate" Type="DateTime" />
                        <asp:Parameter Name="LiquedatedValue" Type="Decimal" />
                        <asp:Parameter Name="PaidValue" Type="Decimal" />
                        <asp:Parameter Name="EstimatedDamageValue" Type="Decimal" />
                    </InsertParameters>
                    <SelectParameters>
                        <asp:ControlParameter ControlID="GXGridView1" Name="id" PropertyName="SelectedValue"
                            Type="Int32" />
                    </SelectParameters>
                </cc1:DetailsViewDataSource>
            </div>
        </asp:View>
        <asp:View ID="viewSearch" runat="server">
            <div class="paddingKontroli">
                <cc1:SearchControl ID="SearchControl1" runat="server" SearchDataSourceID="odsSearch"
                    GridViewToSearch="GXGridView1" OnSearch="SearchControl1_Search">
                    <%-- onsavesearch="Save_Search"
                    ondeletesearch="Delete_Search">--%>
                    <cc1:SearchItem FieldName="Број на штета" PropertyName="DamageNumber" Comparator="StringComparators"
                        DataType="String" />
                    <cc1:SearchItem FieldName="Полиса" PropertyName="PolicyNumber" Comparator="StringComparators"
                        DataType="String" />
                    <cc1:SearchItem FieldName="Дата на штета" PropertyName="DamageDate" Comparator="NumericComparators"
                        DataType="DateTime" />
                    <cc1:SearchItem FieldName="Дата на пријава" PropertyName="AplicationDate" Comparator="NumericComparators"
                        DataType="DateTime" />
                    <cc1:SearchItem FieldName="Место на штета" PropertyName="DamagePlace" Comparator="StringComparators"
                        DataType="String" />
                    <cc1:SearchItem FieldName="Проценета вредност" PropertyName="EstimatedDamageValue"
                        Comparator="NumericComparators" DataType="Decimal" />
                    <cc1:SearchItem FieldName="Ликвидирана вредност" PropertyName="LiquedatedValue" Comparator="NumericComparators"
                        DataType="Decimal" />
                    <cc1:SearchItem FieldName="Исплатена вредност" PropertyName="PaidValue" Comparator="NumericComparators"
                        DataType="Decimal" />
                    <cc1:SearchItem FieldName="Корисник" PropertyName="UserName" Comparator="StringComparators"
                        DataType="String" />
                </cc1:SearchControl>
                <cc1:GridViewDataSource ID="odsSearch" runat="server" TypeName="Broker.DataAccess.ViewDamage"
                    SelectCountMethod="SelectSearchCountCached" SelectMethod="SelectSearch">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="SearchControl1" PropertyName="SearchArguments" Name="sArgument" />
                    </SelectParameters>
                </cc1:GridViewDataSource>
            </div>
        </asp:View>
        <asp:View ID="viewReport" runat="server">
            <div class="paddingKontroli">
                <cc1:ReportControl ID="reportControl" runat="server" TypeName="Broker.DataAccess.ViewDamage"
                    GridViewID="GXGridView1" SearchControlID="SearchControl1">
                    <cc1:PrintItem HeaderText="Број на штета" PropertyName="DamageNumber" />
                    <cc1:PrintItem HeaderText="Полиса" PropertyName="PolicyNumber" />
                    <cc1:PrintItem HeaderText="Дата на штета" PropertyName="DamageDate" />
                    <cc1:PrintItem HeaderText="Дата на пријава" PropertyName="AplicationDate" />
                    <cc1:PrintItem HeaderText="Место на штета" PropertyName="DamagePlace" />
                    <cc1:PrintItem HeaderText="Проценета вредност" PropertyName="EstimatedDamageValue" />
                    <cc1:PrintItem HeaderText="Ликвидирана вредност" PropertyName="LiquedatedValue" />
                    <cc1:PrintItem HeaderText="Исплатена вредност" PropertyName="PaidValue" />
                    <cc1:PrintItem HeaderText="Корисник" PropertyName="UserName" />
                </cc1:ReportControl>
            </div>
        </asp:View>
        <asp:View ID="viewAttachments" runat="server">
            <div class="paddingKontroli">
                <asp:DetailsView ID="dvDamageAttachemtsPreview" runat="server" DataSourceID="odsDamageAttachemtsPreview"
                    DataKeyNames="ID" AutoGenerateRows="False" GridLines="None" DefaultMode="ReadOnly">
                    <Fields>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <table>
                                    <tr>
                                        <td style="width: 150px;">
                                            <asp:Label ID="lblDamageNumber" Text="Број на штета" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tbDamageNumber" runat="server" Text='<%# Bind("DamageNumber") %>'
                                                ReadOnly="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <table>
                                    <tr>
                                        <td style="width: 150px;">
                                            <asp:Label ID="lblPolicyNumber" runat="server" Text="Број на полиса"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tbOrderNumber" runat="server" Text='<%# Eval("PolicyItem.PolicyNumber") %>'
                                                ReadOnly="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 150px;">
                                            <asp:Label ID="lblInsuranceType" runat="server" Text="Класа:"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tbInsuranceType" runat="server" Width="400px" Text='<%# Eval("PolicyItem.InsuranceSubType.InsuranceType.Name") %>'
                                                ReadOnly="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 150px;">
                                            <asp:Label ID="lblInsuranceSubTypes" runat="server" Text="Подкласа:"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tbInsuranceSubTypes" runat="server" Width="400px" Text='<%# Eval("PolicyItem.InsuranceSubType.Description") %>'
                                                ReadOnly="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 150px;">
                                            <asp:Label ID="lblInsuranceCompany" runat="server" Text="Осигурителна компанија:"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tbInsuranceCompany" runat="server" Width="200px" Text='<%# Eval("PolicyItem.Policy.InsuranceCompany.Name") %>'
                                                ReadOnly="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <table>
                                    <tr>
                                        <td style="width: 150px;">
                                            <asp:Label ID="lblDamageDate" runat="server" Text="Дата на штета"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tbDamageDate" runat="server" Text='<%# Bind("DamageDate", "{0:d}") %>'
                                                ReadOnly="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <table>
                                    <tr>
                                        <td style="width: 150px;">
                                            <asp:Label ID="lblAplicationDate" runat="server" Text="Дата на пријава"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tbAplicationDate" runat="server" Text='<%# Bind("AplicationDate", "{0:d}") %>'
                                                ReadOnly="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <table>
                                    <tr>
                                        <td style="width: 150px;">
                                            <asp:Label ID="lblDamagePlace" runat="server" Text="Место на штета"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tbDamagePlace" runat="server" Text='<%# Bind("DamagePlace") %>'
                                                ReadOnly="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <table>
                                    <tr>
                                        <td style="width: 150px;">
                                            <asp:Label ID="lblEstimatedDamageValue" runat="server" Text="Проценета вредност"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tbEstimatedDamageValue" runat="server" Text='<%# Bind("EstimatedDamageValue") %>'
                                                ReadOnly="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <table>
                                    <tr>
                                        <td style="width: 150px;">
                                            <asp:Label ID="lblLiquedatedValue" runat="server" Text="Ликвидирана вредност"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tbLiquedatedValue" runat="server" Text='<%# Bind("LiquedatedValue") %>'
                                                ReadOnly="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <table>
                                    <tr>
                                        <td style="width: 150px;">
                                            <asp:Label ID="lblPaidValue" runat="server" Text="Исплатена вредност"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tbPaidValue" runat="server" Text='<%# Bind("PaidValue") %>' ReadOnly="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Fields>
                </asp:DetailsView>
                <cc1:DetailsViewDataSource ID="odsDamageAttachemtsPreview" runat="server" TypeName="Broker.DataAccess.Damage"
                    ConflictDetection="CompareAllValues" DataObjectTypeName="Broker.DataAccess.Damage"
                    SelectMethod="Get" OldValuesParameterFormatString="oldEntity">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="GXGridView1" Name="id" PropertyName="SelectedValue"
                            Type="Int32" />
                    </SelectParameters>
                </cc1:DetailsViewDataSource>
                <table>
                    <tr>
                        <td>
                            <asp:FileUpload ID="FileUpload1" runat="server" />
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
