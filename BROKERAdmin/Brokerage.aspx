<%@ Page Title="Брокеражи" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Brokerage.aspx.cs" Inherits="BROKERAdmin_Brokerage" UICulture="mk-MK"
    Culture="mk-MK" %>

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
    </div>
    <asp:MultiView ID="mvMain" runat="server">
        <asp:View ID="viewGrid" runat="server">
            <div id="tabeliFrame">
                <div id="header">
                    <div id="content">
                        Брокеражи
                    </div>
                </div>
                <div id="contentOuter">
                    <div id="contentInner">
                        <cc1:GXGridView ID="GXGridView1" runat="server" DataSourceID="odsGridView" DataKeyNames="ID"
                            Width="100%" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"
                            Caption="Брокеражи" EmptyDataText="Нема записи кои го задоволуваат критериумот на пребарување!"
                            OnRowCommand="GXGridView1_RowCommand" RowStyle-CssClass="row" CssClass="grid"
                            GridLines="None">
                            <Columns>
                                <asp:BoundField HeaderText="Осигурителна компанија" DataField="InsuranceCompanyName"
                                    SortExpression="InsuranceCompanyName" />
                                <asp:BoundField HeaderText="Договор" DataField="DealDescription" SortExpression="DealDescription" />
                                <asp:BoundField HeaderText="Класа" DataField="InsuranceTypeCode" SortExpression="InsuranceTypeCode" />
                                <asp:BoundField HeaderText="Подкласа" DataField="InsuranceSubTypeCode" SortExpression="InsuranceSubTypeCode" />
                                <asp:BoundField HeaderText="Брокер.(физ.)" DataField="PercentageForPrivates"
                                    SortExpression="PercentageForPrivates" />
                                <asp:BoundField HeaderText="Брокер.(пра.)" DataField="PercentageForLaws" SortExpression="PercentageForLaws" />
                            </Columns>
                            <PagerStyle HorizontalAlign="Center" />
                            <SelectedRowStyle CssClass="rowSelected" />
                            <PagerSettings FirstPageText="<< Прва " PreviousPageText="< Претходна " NextPageText=" Следна >"
                                LastPageText=" Последна >>" Mode="NextPreviousFirstLast" />
                        </cc1:GXGridView>
                        <cc1:GridViewDataSource ID="odsGridView" runat="server" TypeName="Broker.DataAccess.ViewBrokerage"
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
                                        <cc1:FilterItem FieldName="Осигурителна компанија" PropertyName="Code" Comparator="StringComparators"
                                            DataType="String" />
                                        <cc1:FilterItem FieldName="Договор" PropertyName="DealDescription" Comparator="StringComparators"
                                            DataType="String" />
                                        <cc1:FilterItem FieldName="Класа" PropertyName="InsuranceTypeCode" Comparator="StringComparators"
                                            DataType="String" />
                                        <cc1:FilterItem FieldName="Подкласа" PropertyName="InsuranceSubTypeCode" Comparator="StringComparators"
                                            DataType="String" />
                                        <cc1:FilterItem FieldName="Брокер.(физ.)" PropertyName="PercentageForPrivates" Comparator="NumericComparators"
                                            DataType="Decimal" />
                                        <cc1:FilterItem FieldName="Брокер.(пра.)" PropertyName="PercentageForLaws" Comparator="NumericComparators"
                                            DataType="Decimal" />
                                    </cc1:FilterControl>
                                </div>
                                <cc1:FilterDataSource ID="odsFilterGridView" runat="server" TypeName="Broker.DataAccess.ViewBrokerage">
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
                                    Нов запис во Брокеражи
                                </div>
                            </InsertItemTemplate>
                            <EditItemTemplate>
                                <div class="subTitles">
                                    Измена на запис во Брокеражи
                                </div>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <div class="subTitles">
                                    Бришење на запис од Брокеражи
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Договор">
                            <ItemTemplate>
                                <asp:DropDownList ID="ddlDeals" runat="server" DataSourceID="odsDeals" DataTextField="Description"
                                    DataValueField="ID" SelectedValue='<%# Bind("DealID") %>' Enabled="false">
                                </asp:DropDownList>
                                <asp:ObjectDataSource ID="odsDeals" runat="server" SelectMethod="Select" TypeName="Broker.DataAccess.Deal">
                                </asp:ObjectDataSource>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddlDeals" runat="server" DataSourceID="odsDeals" DataTextField="Description"
                                    DataValueField="ID" SelectedValue='<%# Bind("DealID") %>' Enabled="false">
                                </asp:DropDownList>
                                <asp:ObjectDataSource ID="odsDeals" runat="server" SelectMethod="Select" TypeName="Broker.DataAccess.Deal">
                                </asp:ObjectDataSource>
                            </EditItemTemplate>
                            <InsertItemTemplate>
                                <asp:DropDownList ID="ddlDeals" runat="server" DataSourceID="odsDeals" DataTextField="Description"
                                    DataValueField="ID" SelectedValue='<%# Bind("DealID") %>'>
                                </asp:DropDownList>
                                <asp:ObjectDataSource ID="odsDeals" runat="server" SelectMethod="Select" TypeName="Broker.DataAccess.Deal">
                                </asp:ObjectDataSource>
                            </InsertItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Класа на осигурување">
                            <ItemTemplate>
                                <asp:DropDownList ID="ddlInsuranceTypes" Width="300px" runat="server" DataSourceID="odsInsuranceTypes"
                                    DataTextField="Name" DataValueField="ID" Enabled="false">
                                </asp:DropDownList>
                                <asp:ObjectDataSource ID="odsInsuranceTypes" runat="server" SelectMethod="Select"
                                    TypeName="Broker.DataAccess.InsuranceType"></asp:ObjectDataSource>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddlInsuranceTypes" AutoPostBack="true" runat="server" Width="300px"
                                    DataSourceID="odsInsuranceTypes" DataTextField="Name" DataValueField="ID" OnSelectedIndexChanged="ddlInsuranceCompanyIndexChanged"
                                    SelectedValue='<%# Eval("InsuranceSubType.InsuranceTypeID") %>' Enabled="false">
                                </asp:DropDownList>
                                <asp:ObjectDataSource ID="odsInsuranceTypes" runat="server" SelectMethod="Select"
                                    TypeName="Broker.DataAccess.InsuranceType"></asp:ObjectDataSource>
                            </EditItemTemplate>
                            <InsertItemTemplate>
                                <asp:DropDownList ID="ddlInsuranceTypes" AutoPostBack="true" runat="server" Width="300px"
                                    DataSourceID="odsInsuranceTypes" DataTextField="Name" DataValueField="ID" OnSelectedIndexChanged="ddlInsuranceCompanyIndexChanged">
                                </asp:DropDownList>
                                <asp:ObjectDataSource ID="odsInsuranceTypes" runat="server" SelectMethod="Select"
                                    TypeName="Broker.DataAccess.InsuranceType"></asp:ObjectDataSource>
                            </InsertItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Подкласа на осигурување">
                            <ItemTemplate>
                                <asp:DropDownList ID="ddlInsuranceSubTypes" Width="400px" runat="server" DataSourceID="odsInsuranceSubTypes"
                                    DataTextField="Description" DataValueField="ID" SelectedValue='<%# Bind("InsuranceSubTypeID") %>'
                                    Enabled="false">
                                </asp:DropDownList>
                                <asp:ObjectDataSource ID="odsInsuranceSubTypes" runat="server" SelectMethod="Select"
                                    TypeName="Broker.DataAccess.InsuranceSubType"></asp:ObjectDataSource>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddlInsuranceSubTypes" Width="400px" runat="server" DataSourceID="odsInsuranceSubTypes"
                                    DataTextField="Description" Enabled="false" DataValueField="ID" SelectedValue='<%# Bind("InsuranceSubTypeID") %>'>
                                </asp:DropDownList>
                                <asp:ObjectDataSource ID="odsInsuranceSubTypes" runat="server" SelectMethod="GetByInsuranceType"
                                    TypeName="Broker.DataAccess.InsuranceSubType">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="ddlInsuranceTypes" Name="insuranceTypeID" PropertyName="SelectedValue"
                                            Type="Int32" />
                                    </SelectParameters>
                                </asp:ObjectDataSource>
                            </EditItemTemplate>
                            <InsertItemTemplate>
                                <asp:DropDownList ID="ddlInsuranceSubTypes" Width="400px" runat="server" DataSourceID="odsInsuranceSubTypes"
                                    DataTextField="Description" DataValueField="ID" SelectedValue='<%# Bind("InsuranceSubTypeID") %>'>
                                </asp:DropDownList>
                                <asp:ObjectDataSource ID="odsInsuranceSubTypes" runat="server" SelectMethod="GetByInsuranceType"
                                    TypeName="Broker.DataAccess.InsuranceSubType">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="ddlInsuranceTypes" Name="insuranceTypeID" PropertyName="SelectedValue"
                                            Type="Int32" />
                                    </SelectParameters>
                                </asp:ObjectDataSource>
                                <super:EntityCallOutValidator ID="CodeInsertValidator" PropertyName="BROKERAGE_EXISTS"
                                    runat="server" />
                            </InsertItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Процент(физички)">
                            <ItemTemplate>
                                <asp:TextBox ID="tbPercentageForPrivates" runat="server" Text='<%# Bind("PercentageForPrivates") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="tbPercentageForPrivates" runat="server" Text='<%# Bind("PercentageForPrivates") %>'></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvPercentageForPrivates" runat="server" ControlToValidate="tbPercentageForPrivates"
                                    Display="Dynamic" ErrorMessage="*"></asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="cvPercentageForPrivates" runat="server" ControlToValidate="tbPercentageForPrivates"
                                    Display="Dynamic" ErrorMessage="(неправилен формат)" Operator="DataTypeCheck"
                                    Type="Double"></asp:CompareValidator>
                            </EditItemTemplate>
                            <InsertItemTemplate>
                                <asp:TextBox ID="tbPercentageForPrivates" runat="server" Text='<%# Bind("PercentageForPrivates") %>'></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvPercentageForPrivates" runat="server" ControlToValidate="tbPercentageForPrivates"
                                    Display="Dynamic" ErrorMessage="*"></asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="cvPercentageForPrivates" runat="server" ControlToValidate="tbPercentageForPrivates"
                                    Display="Dynamic" ErrorMessage="(неправилен формат)" Operator="DataTypeCheck"
                                    Type="Double"></asp:CompareValidator>
                            </InsertItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Процент(правни)">
                            <ItemTemplate>
                                <asp:TextBox ID="tbPercentageForLaws" runat="server" Text='<%# Bind("PercentageForLaws") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="tbPercentageForLaws" runat="server" Text='<%# Bind("PercentageForLaws") %>'></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvPercentageForLaws" runat="server" ControlToValidate="tbPercentageForLaws"
                                    Display="Dynamic" ErrorMessage="*"></asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="cvPercentageForLaws" runat="server" ControlToValidate="tbPercentageForLaws"
                                    Display="Dynamic" ErrorMessage="(неправилен формат)" Operator="DataTypeCheck"
                                    Type="Double"></asp:CompareValidator>
                            </EditItemTemplate>
                            <InsertItemTemplate>
                                <asp:TextBox ID="tbPercentageForLaws" runat="server" Text='<%# Bind("PercentageForLaws") %>'></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvPercentageForLaws" runat="server" ControlToValidate="tbPercentageForLaws"
                                    Display="Dynamic" ErrorMessage="*"></asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="cvPercentageForLaws" runat="server" ControlToValidate="tbPercentageForLaws"
                                    Display="Dynamic" ErrorMessage="(неправилен формат)" Operator="DataTypeCheck"
                                    Type="Double"></asp:CompareValidator>
                            </InsertItemTemplate>
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
                <cc1:DetailsViewDataSource ID="dvDataSource" runat="server" TypeName="Broker.DataAccess.Brokerage"
                    ConflictDetection="CompareAllValues" DataObjectTypeName="Broker.DataAccess.Brokerage"
                    DeleteMethod="Delete" InsertMethod="Insert" SelectMethod="Get" UpdateMethod="Update"
                    OnUpdating="dvDataSource_Updating" OnUpdated="dvDataSource_Updated" OnInserted="dvDataSource_Inserted"
                    OnInserting="dvDataSource_Inserting">
                    <UpdateParameters>
                        <asp:Parameter Name="oldEntity" Type="Object" />
                        <asp:Parameter Name="newEntity" Type="Object" />
                        <asp:Parameter Name="PercentageForPrivates" Type="Decimal" />
                        <asp:Parameter Name="PercentageForLaws" Type="Decimal" />
                    </UpdateParameters>
                    <InsertParameters>
                        <asp:Parameter Name="entityToInsert" Type="Object" />
                        <asp:Parameter Name="PercentageForPrivates" Type="Decimal" />
                        <asp:Parameter Name="PercentageForLaws" Type="Decimal" />
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
                    <cc1:SearchItem FieldName="Осигурителна компанија" PropertyName="InsuranceCompanyName"
                        Comparator="StringComparators" DataType="String" />
                    <cc1:SearchItem FieldName="Договор" PropertyName="DealDescription" Comparator="StringComparators"
                        DataType="String" />
                    <cc1:SearchItem FieldName="Класа" PropertyName="InsuranceTypeCode" Comparator="StringComparators"
                        DataType="String" />
                    <cc1:SearchItem FieldName="Подкласа" PropertyName="InsuranceSubTypeCode" Comparator="StringComparators"
                        DataType="String" />
                    <cc1:SearchItem FieldName="Брокеража(приватни)" PropertyName="PercentageForPrivates"
                        Comparator="NumericComparators" DataType="Decimal" />
                    <cc1:SearchItem FieldName="Брокеража(правни)" PropertyName="PercentageForLaws" Comparator="NumericComparators"
                        DataType="Decimal" />
                </cc1:SearchControl>
                <cc1:GridViewDataSource ID="odsSearch" runat="server" TypeName="Broker.DataAccess.ViewBrokerage"
                    SelectCountMethod="SelectSearchCountCached" SelectMethod="SelectSearch">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="SearchControl1" PropertyName="SearchArguments" Name="sArgument" />
                    </SelectParameters>
                </cc1:GridViewDataSource>
            </div>
        </asp:View>
        <asp:View ID="viewReport" runat="server">
            <div class="paddingKontroli">
                <cc1:ReportControl ID="reportControl" runat="server" TypeName="Broker.DataAccess.ViewBrokerage"
                    GridViewID="GXGridView1" SearchControlID="SearchControl1">
                    <cc1:PrintItem HeaderText="Осигурителна компанија" PropertyName="InsuranceCompanyName" />
                    <cc1:PrintItem HeaderText="Договор" PropertyName="DealDescription" />
                    <cc1:PrintItem HeaderText="Класа" PropertyName="InsuranceTypeCode" />
                    <cc1:PrintItem HeaderText="Подкласа" PropertyName="InsuranceSubTypeCode" />
                    <cc1:PrintItem HeaderText="Брокеража(приватни)" PropertyName="PercentageForPrivates" />
                    <cc1:PrintItem HeaderText="Брокеража(правни)" PropertyName="PercentageForLaws" />
                </cc1:ReportControl>
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>
