<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="LifeDeals.aspx.cs" Inherits="BROKERAdmin_LifeDeals" %>

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
        <div id="button03">
            <asp:Button ID="btnDelete" runat="server" OnClick="btnDelete_Click" CausesValidation="false"
                Enabled="false" UseSubmitBehavior="false" CssClass="izbrisi" BorderWidth="0px" />
        </div>
        <div id="button04">
            <asp:Button ID="btnPreview" runat="server" ToolTip="Освежи" OnClick="btnPreview_Click"
                CausesValidation="false" UseSubmitBehavior="false" CssClass="osvezi" BorderWidth="0px" />
        </div>
        <div id="button05">
            <asp:Button ID="btnReport" runat="server" ToolTip="Извештај" CausesValidation="false"
                UseSubmitBehavior="false" OnClick="btnReport_Click" CssClass="izvestaj" BorderWidth="0px" />
        </div>
        <div id="button06">
            <asp:Button ID="btnSearch" runat="server" ToolTip="Пребарај" OnClick="btnSearch_Click"
                CausesValidation="false" UseSubmitBehavior="false" CssClass="prebaraj" BorderWidth="0px" />
        </div>
        <div id="button07">
            <asp:Button ID="btnBrokerages" runat="server" ToolTip="Брокеражи" OnClick="btnBrokerages_Click"
                CausesValidation="false" Enabled="false" UseSubmitBehavior="false" CssClass="brokerazi"
                BorderWidth="0px" />
        </div>
    </div>
    <asp:MultiView ID="mvMain" runat="server">
        <asp:View ID="viewGrid" runat="server">
            <div id="tabeliFrame">
                <div id="header">
                    <div id="content">
                        Договори за жив.оси.
                    </div>
                </div>
                <div id="contentOuter">
                    <div id="contentInner">
                        <cc1:GXGridView ID="GXGridView1" runat="server" DataSourceID="odsGridView" DataKeyNames="ID"
                            Width="100%" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"
                            Caption="Договори" EmptyDataText="Нема записи кои го задоволуваат критериумот на пребарување!"
                            OnRowCommand="GXGridView1_RowCommand" RowStyle-CssClass="row" CssClass="grid"
                            GridLines="None">
                            <Columns>
                                <asp:BoundField HeaderText="Шифра" DataField="Code" SortExpression="Code" />
                                <asp:BoundField HeaderText="Име" DataField="Name" SortExpression="Name" />
                                <asp:BoundField HeaderText="Осиг. компанија" DataField="InsuranceCompanyName" SortExpression="InsuranceCompanyName" />
                                <asp:BoundField HeaderText="Подкласа на осиг." DataField="InsuranceSubTypeShortDescription"
                                    SortExpression="InsuranceSubTypeShortDescription" />
                                <asp:BoundField HeaderText="Про. од пок. за доживување" DataField="PercentageFromSumForRestLiving"
                                    SortExpression="PercentageFromSumForRestLiving" />
                                <asp:BoundField HeaderText="Про. од пре. за незгода" DataField="PercentageFromPremiumForAccident"
                                    SortExpression="PercentageFromPremiumForAccident" />
                            </Columns>
                            <PagerStyle HorizontalAlign="Center" />
                            <SelectedRowStyle CssClass="rowSelected" />
                            <PagerSettings FirstPageText="<< Прва " PreviousPageText="< Претходна " NextPageText=" Следна >"
                                LastPageText=" Последна >>" Mode="NextPreviousFirstLast" />
                        </cc1:GXGridView>
                        <cc1:GridViewDataSource ID="odsGridView" runat="server" TypeName="Broker.DataAccess.ViewLifeDeal"
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
                                        <cc1:FilterItem FieldName="Шифра" PropertyName="Code" Comparator="StringComparators"
                                            DataType="String" />
                                        <cc1:FilterItem FieldName="Име" PropertyName="Name" Comparator="StringComparators"
                                            DataType="String" />
                                        <cc1:FilterItem FieldName="Осиг. компанија" PropertyName="InsuranceCompanyName" Comparator="StringComparators"
                                            DataType="String" />
                                        <cc1:FilterItem FieldName="Подкласа на осиг." PropertyName="InsuranceSubTypeShortDescription"
                                            Comparator="StringComparators" DataType="String" />
                                    </cc1:FilterControl>
                                </div>
                                <cc1:FilterDataSource ID="odsFilterGridView" runat="server" TypeName="Broker.DataAccess.ViewLifeDeal">
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
                    GridLines="None">
                    <Fields>
                        <asp:TemplateField>
                            <InsertItemTemplate>
                                <div class="subTitles">
                                    Нов запис во Договори за жив. оси.
                                </div>
                            </InsertItemTemplate>
                            <EditItemTemplate>
                                <div class="subTitles">
                                    Измена на запис во Договори за жив. оси.
                                </div>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <div class="subTitles">
                                    Бришење на запис од Договори за жив. оси.
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Шифра">
                            <InsertItemTemplate>
                                <asp:TextBox ID="tbCode" runat="server" Text='<%# Bind("Code") %>' MaxLength="10"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvCode" runat="server" ControlToValidate="tbCode"
                                    Display="Dynamic" ErrorMessage="*">
                                </asp:RequiredFieldValidator>
                                <super:EntityCallOutValidator ID="CodeInsertValidator" PropertyName="DEAL_CODE_INSERT_EXISTS"
                                    runat="server" />
                            </InsertItemTemplate>
                            <ItemTemplate>
                                <asp:TextBox ID="tbCode" runat="server" Text='<%# Bind("Code") %>' ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="tbCode" runat="server" Text='<%# Bind("Code") %>' MaxLength="10"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvCode" runat="server" ControlToValidate="tbCode"
                                    ErrorMessage="*"></asp:RequiredFieldValidator>
                                <super:EntityCallOutValidator ID="CodeUpdateValidator" PropertyName="DEAL_CODE_UPDATE_EXISTS"
                                    runat="server" />
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Име">
                            <ItemTemplate>
                                <asp:TextBox ID="tbName" runat="server" Text='<%# Bind("Name") %>' ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="tbName" runat="server" Text='<%# Bind("Name") %>' MaxLength="100"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="tbName"
                                    ErrorMessage="*"></asp:RequiredFieldValidator>
                            </EditItemTemplate>
                            <InsertItemTemplate>
                                <asp:TextBox ID="tbName" runat="server" Text='<%# Bind("Name") %>' MaxLength="100"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="tbName"
                                    ErrorMessage="*"></asp:RequiredFieldValidator>
                            </InsertItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Опис">
                            <ItemTemplate>
                                <asp:TextBox ID="tbDescription" runat="server" Text='<%# Bind("Description") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="tbDescription" runat="server" Text='<%# Bind("Description") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <InsertItemTemplate>
                                <asp:TextBox ID="tbDescription" runat="server" Text='<%# Bind("Description") %>'></asp:TextBox>
                            </InsertItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Осигурителна компанија">
                            <ItemTemplate>
                                <asp:DropDownList ID="ddlInsuranceCompanies" runat="server" Enabled="False" DataSourceID="odsInsuranceCompanies"
                                    DataTextField="Name" DataValueField="ID" SelectedValue='<%# Bind("InsuranceCompanyID") %>'>
                                </asp:DropDownList>
                                <asp:ObjectDataSource ID="odsInsuranceCompanies" runat="server" SelectMethod="Select"
                                    TypeName="Broker.DataAccess.InsuranceCompany"></asp:ObjectDataSource>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddlActiveInsuranceCompanies" runat="server" DataSourceID="odsActiveInsuranceCompanies"
                                    DataTextField="Name" DataValueField="ID" SelectedValue='<%# Bind("InsuranceCompanyID") %>'>
                                </asp:DropDownList>
                                <asp:ObjectDataSource ID="odsActiveInsuranceCompanies" runat="server" SelectMethod="GetActiveInsuranceCompanies"
                                    TypeName="Broker.DataAccess.InsuranceCompany"></asp:ObjectDataSource>
                            </EditItemTemplate>
                            <InsertItemTemplate>
                                <asp:DropDownList ID="ddlActiveInsuranceCompanies" runat="server" DataSourceID="odsActiveInsuranceCompanies"
                                    DataTextField="Name" DataValueField="ID" SelectedValue='<%# Bind("InsuranceCompanyID") %>'>
                                </asp:DropDownList>
                                <asp:ObjectDataSource ID="odsActiveInsuranceCompanies" runat="server" SelectMethod="GetActiveInsuranceCompanies"
                                    TypeName="Broker.DataAccess.InsuranceCompany"></asp:ObjectDataSource>
                            </InsertItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Подкласа на осигурување">
                            <ItemTemplate>
                                <asp:DropDownList ID="ddlInsuranceSubTypes" runat="server" Enabled="False" DataSourceID="odsInsuranceSubTypes"
                                    DataTextField="ShortDescription" DataValueField="ID" SelectedValue='<%# Bind("InsuranceSubTypeID") %>'>
                                </asp:DropDownList>
                                <asp:ObjectDataSource ID="odsInsuranceSubTypes" runat="server" SelectMethod="GetForLife"
                                    TypeName="Broker.DataAccess.InsuranceSubType"></asp:ObjectDataSource>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddlInsuranceSubTypes" runat="server" Enabled="true" DataSourceID="odsInsuranceSubTypes"
                                    DataTextField="ShortDescription" DataValueField="ID" SelectedValue='<%# Bind("InsuranceSubTypeID") %>'>
                                </asp:DropDownList>
                                <asp:ObjectDataSource ID="odsInsuranceSubTypes" runat="server" SelectMethod="GetForLife"
                                    TypeName="Broker.DataAccess.InsuranceSubType"></asp:ObjectDataSource>
                            </EditItemTemplate>
                            <InsertItemTemplate>
                                <asp:DropDownList ID="ddlInsuranceSubTypes" runat="server" Enabled="true" DataSourceID="odsInsuranceSubTypes"
                                    DataTextField="ShortDescription" DataValueField="ID" SelectedValue='<%# Bind("InsuranceSubTypeID") %>'>
                                </asp:DropDownList>
                                <asp:ObjectDataSource ID="odsInsuranceSubTypes" runat="server" SelectMethod="GetForLife"
                                    TypeName="Broker.DataAccess.InsuranceSubType"></asp:ObjectDataSource>
                            </InsertItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Процент од покритие за доживување">
                            <InsertItemTemplate>
                                <asp:TextBox ID="tbPercentageFromSumForRestLiving" runat="server" Text='<%#Bind("PercentageFromSumForRestLiving") %>'></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvPercentageFromSumForRestLiving" runat="server"
                                    ErrorMessage="*" Display="Dynamic" ControlToValidate="tbPercentageFromSumForRestLiving"></asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="cvPercentageFromSumForRestLiving" runat="server" ErrorMessage="*"
                                    Operator="DataTypeCheck" Type="Double" ControlToValidate="tbPercentageFromSumForRestLiving"></asp:CompareValidator>
                            </InsertItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="tbPercentageFromSumForRestLiving" runat="server" Text='<%#Bind("PercentageFromSumForRestLiving") %>'></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvPercentageFromSumForRestLiving" runat="server"
                                    ErrorMessage="*" Display="Dynamic" ControlToValidate="tbPercentageFromSumForRestLiving"></asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="cvPercentageFromSumForRestLiving" runat="server" ErrorMessage="*"
                                    Operator="DataTypeCheck" Type="Double" ControlToValidate="tbPercentageFromSumForRestLiving"></asp:CompareValidator>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:TextBox ID="tbPercentageFromSumForRestLiving" runat="server" Text='<%#Bind("PercentageFromSumForRestLiving") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Процент од премија за незгода">
                            <InsertItemTemplate>
                                <asp:TextBox ID="tbPercentageFromPremiumForAccident" runat="server" Text='<%#Bind("PercentageFromPremiumForAccident") %>'></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvPercentageFromPremiumForAccident" runat="server"
                                    ErrorMessage="*" Display="Dynamic" ControlToValidate="tbPercentageFromPremiumForAccident"></asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="cvPercentageFromPremiumForAccident" runat="server" ErrorMessage="*"
                                    Operator="DataTypeCheck" Type="Double" ControlToValidate="tbPercentageFromPremiumForAccident"></asp:CompareValidator>
                            </InsertItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="tbPercentageFromPremiumForAccident" runat="server" Text='<%#Bind("PercentageFromPremiumForAccident") %>'></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvPercentageFromPremiumForAccident" runat="server"
                                    ErrorMessage="*" Display="Dynamic" ControlToValidate="tbPercentageFromPremiumForAccident"></asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="cvPercentageFromPremiumForAccident" runat="server" ErrorMessage="*"
                                    Operator="DataTypeCheck" Type="Double" ControlToValidate="tbPercentageFromPremiumForAccident"></asp:CompareValidator>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:TextBox ID="tbPercentageFromPremiumForAccident" runat="server" Text='<%#Bind("PercentageFromPremiumForAccident") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:CheckBox ID="cbIsActive" runat="server" Checked='<%#Bind("IsActive") %>' Visible="false" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:CheckBox ID="cbIsActive" runat="server" Checked='<%#Bind("IsActive") %>' Visible="false" />
                            </EditItemTemplate>
                            <InsertItemTemplate>
                                <asp:CheckBox ID="cbIsActive" runat="server" Checked='<%#Bind("IsActive") %>' Visible="false" />
                            </InsertItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Button ID="btnCancel" runat="server" CommandName="Cancel" Text="Откажи" CausesValidation="false" />
                                <asp:Button ID="btnDelete" runat="server" Text="Избриши" OnClientClick="return confirm('Дали сте сигурни?')"
                                    OnClick="btnDelete_Click1" />
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
                <cc1:DetailsViewDataSource ID="dvDataSource" runat="server" TypeName="Broker.DataAccess.LifeDeal"
                    ConflictDetection="CompareAllValues" DataObjectTypeName="Broker.DataAccess.LifeDeal"
                    DeleteMethod="Delete" InsertMethod="Insert" SelectMethod="Get" UpdateMethod="Update"
                    OnUpdating="dvDataSource_Updating" OnUpdated="dvDataSource_Updated" OnInserted="dvDataSource_Inserted"
                    OnInserting="dvDataSource_Inserting">
                    <UpdateParameters>
                        <asp:Parameter Name="oldEntity" Type="Object" />
                        <asp:Parameter Name="newEntity" Type="Object" />
                        <asp:Parameter Name="PercentageFromSumForRestLiving" Type="Decimal" />
                        <asp:Parameter Name="PercentageFromPremiumForAccident" Type="Decimal" />
                    </UpdateParameters>
                    <InsertParameters>
                        <asp:Parameter Name="entityToInsert" Type="Object" />
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
                    <cc1:SearchItem FieldName="Шифра" PropertyName="Code" Comparator="StringComparators"
                        DataType="String" />
                    <cc1:SearchItem FieldName="Име" PropertyName="Name" Comparator="StringComparators"
                        DataType="String" />
                    <cc1:SearchItem FieldName="Осиг. компанија" PropertyName="InsuranceCompanyName" Comparator="StringComparators"
                        DataType="String" />
                    <cc1:SearchItem FieldName="Подкласа на осиг." PropertyName="InsuranceSubTypeShortDescription"
                        Comparator="StringComparators" DataType="String" />
                </cc1:SearchControl>
                <cc1:GridViewDataSource ID="odsSearch" runat="server" TypeName="Broker.DataAccess.ViewLifeDeal"
                    SelectCountMethod="SelectSearchCountCached" SelectMethod="SelectSearch">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="SearchControl1" PropertyName="SearchArguments" Name="sArgument" />
                    </SelectParameters>
                </cc1:GridViewDataSource>
            </div>
        </asp:View>
        <asp:View ID="viewReport" runat="server">
            <div class="paddingKontroli">
                <cc1:ReportControl ID="reportControl" runat="server" TypeName="Broker.DataAccess.ViewLifeDeal"
                    GridViewID="GXGridView1" SearchControlID="SearchControl1">
                    <cc1:PrintItem HeaderText="Шифра" PropertyName="Code" />
                    <cc1:PrintItem HeaderText="Име" PropertyName="Name" />
                    <cc1:PrintItem HeaderText="Осиг. компанија" PropertyName="InsuranceCompanyName" />
                    <cc1:PrintItem HeaderText="Подкласа на осиг." PropertyName="InsuranceSubTypeShortDescription" />
                    <cc1:PrintItem HeaderText="Про. од пок. за доживување" PropertyName="PercentageFromSumForRestLiving" />
                    <cc1:PrintItem HeaderText="Про. од пре. за незгода" PropertyName="PercentageFromPremiumForAccident" />
                </cc1:ReportControl>
            </div>
        </asp:View>
        <asp:View ID="viewBrokerages" runat="server">
            <div class="paddingKontroli">
                <asp:DetailsView ID="dvLifeDealPreview" runat="server" DataSourceID="odsLifeDealPreview"
                    DataKeyNames="ID" AutoGenerateRows="False" OnItemCommand="dvLifeDealPreview_ItemCommand"
                    OnModeChanging="dvLifeDealPreview_ModeChanging" GridLines="None" DefaultMode="ReadOnly">
                    <Fields>
                        <asp:TemplateField HeaderText="Шифра">
                            <ItemTemplate>
                                <asp:TextBox ID="tbCode" runat="server" Text='<%# Bind("Code") %>' ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Име">
                            <ItemTemplate>
                                <asp:TextBox ID="tbName" runat="server" Text='<%# Bind("Name") %>' ReadOnly="true"
                                    Width="200px"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Опис">
                            <ItemTemplate>
                                <asp:TextBox ID="tbDescription" runat="server" Text='<%# Bind("Description") %>'
                                    ReadOnly="true" Width="200px"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Осигурителна компанија">
                            <ItemTemplate>
                                <asp:TextBox ID="tbInsuranceCompanyName" runat="server" Text='<%# Eval("InsuranceCompany.Name") %>'
                                    ReadOnly="true" Width="200px"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Подкласа на осигурување">
                            <ItemTemplate>
                                <asp:TextBox ID="tbInsuranceSubTypeShortDescription" runat="server" Text='<%# Eval("InsuranceSubType.ShortDescription") %>'
                                    ReadOnly="true" Width="200px"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Про. од пок. за доживување">
                            <ItemTemplate>
                                <asp:TextBox ID="tbPreviewPercentageFromSumForRestLiving" runat="server" Text='<%# Bind("PercentageFromSumForRestLiving") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Про. од пре. за незгода">
                            <ItemTemplate>
                                <asp:TextBox ID="tbPercentageFromSumForRestLiving" runat="server" Text='<%# Bind("PercentageFromPremiumForAccident") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Fields>
                </asp:DetailsView>
                <cc1:DetailsViewDataSource ID="odsLifeDealPreview" runat="server" TypeName="Broker.DataAccess.LifeDeal"
                    ConflictDetection="CompareAllValues" DataObjectTypeName="Broker.DataAccess.LifeDeal"
                    SelectMethod="Get" OldValuesParameterFormatString="oldEntity" OnSelecting="odsLifeDealPreview_Selecting">
                </cc1:DetailsViewDataSource>
                <asp:MultiView ID="mvLifeDealItems" runat="server">
                    <asp:View ID="viewLifeDealItemsGrid" runat="server">
                        <asp:GridView ID="GridViewLifeDealItems" runat="server" DataSourceID="odsGridViewLifeDealItems"
                            DataKeyNames="ID" Width="100%" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"
                            Caption="Ставки на договорот" EmptyDataText="Нема записи кои го задоволуваат критериумот на пребарување!"
                            RowStyle-CssClass="row" CssClass="grid" GridLines="None" OnRowDeleting="GridViewLifeDealItems_RowDeleting"
                            OnRowDeleted="GridViewLifeDealItems_RowDeleted">
                            <RowStyle CssClass="row"></RowStyle>
                            <Columns>
                                <asp:TemplateField Visible="false">
                                    <ItemTemplate>
                                        <asp:TextBox ID="tbLifeDealItemID" runat="server" Text='<%#Bind("ID") %>' Visible="false"></asp:TextBox>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="tbLifeDealItemID" runat="server" Text='<%#Bind("ID") %>' Visible="false"></asp:TextBox>
                                    </EditItemTemplate>
                                    <InsertItemTemplate>
                                        <asp:TextBox ID="tbLifeDealItemID" runat="server" Text='<%#Bind("ID") %>' Visible="false"></asp:TextBox>
                                    </InsertItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <InsertItemTemplate>
                                        <asp:Label ID="tbLifeDealID" runat="server" Text='<%#Bind("LifeDealID") %>' Visible="false"></asp:Label>
                                    </InsertItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="tbLifeDealID" runat="server" Text='<%#Bind("LifeDealID") %>' Visible="false"></asp:Label>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="tbLifeDealID" runat="server" Text='<%#Bind("LifeDealID") %>' Visible="false"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Реден број (број на година)">
                                    <InsertItemTemplate>
                                        <asp:TextBox ID="tbOrdinalNumber" Width="50px" runat="server" Text='<%#Bind("OrdinalNumber") %>'></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvOrdinalNumber" runat="server" ControlToValidate="tbOrdinalNumber"
                                            ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:CompareValidator ID="cvOrdinalNumber" runat="server" ControlToValidate="tbOrdinalNumber"
                                            ErrorMessage="*" Display="Dynamic" Operator="DataTypeCheck" Type="Integer"></asp:CompareValidator>
                                    </InsertItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="tbOrdinalNumber" Width="50px" runat="server" Text='<%#Bind("OrdinalNumber") %>'></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvOrdinalNumber" runat="server" ControlToValidate="tbOrdinalNumber"
                                            ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:CompareValidator ID="cvOrdinalNumber" runat="server" ControlToValidate="tbOrdinalNumber"
                                            ErrorMessage="*" Display="Dynamic" Operator="DataTypeCheck" Type="Integer"></asp:CompareValidator>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="tbOrdinalNumber" Width="50px" runat="server" Text='<%#Bind("OrdinalNumber") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Процент на брокеража">
                                    <InsertItemTemplate>
                                        <asp:TextBox ID="tbBrokeragePecentForYear" Width="70px" runat="server" Text='<%#Bind("BrokeragePecentForYear") %>'></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvBrokeragePecentForYear" runat="server" ControlToValidate="tbBrokeragePecentForYear"
                                            ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:CompareValidator ID="cvBrokeragePecentForYear" runat="server" ControlToValidate="tbBrokeragePecentForYear"
                                            ErrorMessage="*" Display="Dynamic" Operator="DataTypeCheck" Type="Double"></asp:CompareValidator>
                                    </InsertItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="tbBrokeragePecentForYear" Width="70px" runat="server" Text='<%#Bind("BrokeragePecentForYear") %>'></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvBrokeragePecentForYear" runat="server" ControlToValidate="tbBrokeragePecentForYear"
                                            ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:CompareValidator ID="cvBrokeragePecentForYear" runat="server" ControlToValidate="tbBrokeragePecentForYear"
                                            ErrorMessage="*" Display="Dynamic" Operator="DataTypeCheck" Type="Double"></asp:CompareValidator>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="tbBrokeragePecentForYear" Width="70px" runat="server" Text='<%#Bind("BrokeragePecentForYear") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:CommandField CancelText="Откажи" DeleteText="Избриши" EditText="Измени" InsertText="Внеси"
                                    InsertVisible="False" NewText="Нов" SelectText="Одбери" ShowDeleteButton="True"
                                    ShowEditButton="True" UpdateText="Ажурирај" />
                            </Columns>
                            <PagerStyle HorizontalAlign="Center" />
                            <%--<SelectedRowStyle CssClass="rowSelected" />--%>
                            <PagerSettings FirstPageText="<< Прва " PreviousPageText="< Претходна " NextPageText=" Следна >"
                                LastPageText=" Последна >>" Mode="NextPreviousFirstLast" />
                        </asp:GridView>
                        <asp:ObjectDataSource ID="odsGridViewLifeDealItems" runat="server" TypeName="Broker.DataAccess.LifeDealBrokerage"
                            OldValuesParameterFormatString="oldEntity" DataObjectTypeName="Broker.DataAccess.LifeDealBrokerage"
                            ConflictDetection="CompareAllValues" DeleteMethod="Delete" InsertMethod="Insert"
                            SelectMethod="GetByLifeDeal" UpdateMethod="Update" OnSelecting="odsGridViewLifeDealItems_Selecting">
                            <UpdateParameters>
                                <asp:Parameter Name="oldEntity" Type="Object" />
                                <asp:Parameter Name="newEntity" Type="Object" />
                                <asp:Parameter Name="OrdinalNumber" Type="Int32" />
                                <asp:Parameter Name="BrokeragePecentForYear" Type="Decimal" />
                            </UpdateParameters>
                            <DeleteParameters>
                                <asp:Parameter Name="entityToDelete" Type="Object" />
                                <asp:Parameter Name="OrdinalNumber" Type="Int32" />
                                <asp:Parameter Name="BrokeragePecentForYear" Type="Decimal" />
                            </DeleteParameters>
                        </asp:ObjectDataSource>
                        <div>
                            <asp:Button ID="btnNewLifeDealItem" runat="server" Text="Нова ставка" OnClick="btnNewLifeDealItem_Click" />
                        </div>
                    </asp:View>
                    <asp:View ID="viewLifeDealItemsEdit" runat="server">
                        <asp:DetailsView ID="DetailsViewLifeDealItems" runat="server" DataSourceID="dvDataSourceLifeDealItems"
                            DataKeyNames="ID" AutoGenerateRows="False" OnItemCommand="DetailsViewLifeDealItems_ItemCommand"
                            OnItemInserted="DetailsViewLifeDealItems_ItemInserted" OnModeChanging="DetailsViewLifeDealItems_ModeChanging"
                            OnItemInserting="DetailsViewLifeDealItems_ItemInserting" GridLines="None" DefaultMode="Insert">
                            <Fields>
                                <asp:TemplateField>
                                    <InsertItemTemplate>
                                        <div class="subTitles">
                                            Нова ставка
                                        </div>
                                    </InsertItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <InsertItemTemplate>
                                        <asp:TextBox ID="tbLifeDealItemID" runat="server" Text='<%#Bind("ID") %>' Visible="false"></asp:TextBox>
                                    </InsertItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Реден број (број на година)">
                                    <InsertItemTemplate>
                                        <asp:TextBox ID="tbOrdinalNumber" runat="server" Text='<%#Bind("OrdinalNumber") %>'></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvOrdinalNumber" runat="server" ControlToValidate="tbOrdinalNumber"
                                            ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:CompareValidator ID="cvOrdinalNumber" runat="server" ControlToValidate="tbOrdinalNumber"
                                            ErrorMessage="*" Display="Dynamic" Operator="DataTypeCheck" Type="Integer"></asp:CompareValidator>
                                    </InsertItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Процент на брокеража">
                                    <InsertItemTemplate>
                                        <asp:TextBox ID="tbBrokeragePecentForYear" Width="100px" runat="server" Text='<%#Bind("BrokeragePecentForYear") %>'></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvBrokeragePecentForYear" runat="server" ControlToValidate="tbBrokeragePecentForYear"
                                            ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:CompareValidator ID="cvBrokeragePecentForYear" runat="server" ControlToValidate="tbBrokeragePecentForYear"
                                            ErrorMessage="*" Display="Dynamic" Operator="DataTypeCheck" Type="Double"></asp:CompareValidator>
                                    </InsertItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <InsertItemTemplate>
                                        <asp:Button ID="btnCancel" runat="server" CommandName="Cancel" Text="Откажи" CausesValidation="false" />
                                        <asp:Button ID="btnInsert" runat="server" CommandName="Insert" Text="Внеси" />
                                    </InsertItemTemplate>
                                </asp:TemplateField>
                            </Fields>
                        </asp:DetailsView>
                        <cc1:DetailsViewDataSource ID="dvDataSourceLifeDealItems" runat="server" TypeName="Broker.DataAccess.LifeDealBrokerage"
                            ConflictDetection="CompareAllValues" DataObjectTypeName="Broker.DataAccess.LifeDealBrokerage"
                            InsertMethod="Insert" SelectMethod="Get" OnSelecting="dvDataSourceLifeDealItems_Selecting">
                            <InsertParameters>
                                <asp:Parameter Name="entityToInsert" Type="Object" />
                                <asp:Parameter Name="OrdinalNumber" Type="Int32" />
                                <asp:Parameter Name="BrokeragePecentForYear" Type="Decimal" />
                            </InsertParameters>
                            <SelectParameters>
                                <asp:ControlParameter ControlID="GridViewLifeDealItems" Name="id" PropertyName="SelectedValue"
                                    Type="Int32" />
                            </SelectParameters>
                        </cc1:DetailsViewDataSource>
                    </asp:View>
                </asp:MultiView>
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>
