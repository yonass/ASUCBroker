<%@ Page Title="Клиенти" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Clients.aspx.cs" Inherits="Broker_Clients" Culture="mk-MK" UICulture="mk-MK" %>

<%@ Register Namespace="Superexpert.Controls" TagPrefix="super" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript">
        function ShowChildGrid(obj) {
            var div = document.getElementById(obj);
            var img = document.getElementById('img' + obj);
            var theFlag = div.style.display == "none";
            div.style.display = (theFlag) ? "inline" : "none";
            img.src = (theFlag) ? "../_assets/img/arrowright.jpeg" : "../_assets/img/arrowdown.jpeg";
        }
    </script>

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
            <asp:Button ID="btnAccounts" runat="server" Enabled="false" ToolTip="Жиро-сметки"
                OnClick="btnAccounts_Click" CausesValidation="false" UseSubmitBehavior="false"
                CssClass="smetki" BorderWidth="0px" />
        </div>
        <div id="button07">
            <asp:Button ID="btnRepresentationDeal" runat="server" ToolTip="Договор за застапување"
                CausesValidation="false" UseSubmitBehavior="false" CssClass="dogovor" Enabled="false"
                BorderWidth="0px" OnClick="btnRepresentationDeal_Click" />
        </div>
        <div id="button08">
            <asp:Button ID="btnFinCardForClient" runat="server" ToolTip="Финансова картица" CausesValidation="false"
                UseSubmitBehavior="false" CssClass="fkartica" Enabled="false" BorderWidth="0px"
                OnClick="btnFinCardForClient_Click" />
        </div>
        <div id="button09">
            <asp:Button ID="btnLog" runat="server" ToolTip="Лог на измени" OnClick="btnLog_Click"
                CausesValidation="false" UseSubmitBehavior="false" CssClass="logNaIzmeni" BorderWidth="0px" />
        </div>
        <div id="button10">
            <asp:Button ID="btnLogAccess" runat="server" ToolTip="Лог на прегледи" OnClick="btnLogAccess_Click"
                CausesValidation="false" UseSubmitBehavior="false" CssClass="logNaPoseti" BorderWidth="0px" />
        </div>
    </div>
    <asp:MultiView ID="mvMain" runat="server">
        <asp:View ID="viewGrid" runat="server">
            <div id="tabeliFrame">
                <div id="header">
                    <div id="content">
                        <asp:Label ID="lblPageName" runat="server" Text="Клиенти"></asp:Label>
                    </div>
                </div>
                <div id="contentOuter">
                    <div id="contentInner">
                        <cc1:GXGridView ID="GXGridView1" runat="server" DataSourceID="odsGridView" DataKeyNames="ID"
                            Width="100%" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"
                            Caption="Клиенти" EmptyDataText="Нема записи кои го задоволуваат критериумот на пребарување!"
                            OnRowCommand="GXGridView1_RowCommand" RowStyle-CssClass="row" CssClass="grid"
                            GridLines="None" OnSelectedIndexChanged="GXGridView1_SelectedIndexChanged">
                            <Columns>
                                <asp:BoundField HeaderText="ID" DataField="ID" SortExpression="ID" Visible="false" />
                                <asp:BoundField HeaderText="Име и Презиме" DataField="Name" SortExpression="Name" />
                                <asp:BoundField HeaderText="Матичен Број" DataField="EMBG" SortExpression="EMBG" />
                                <asp:BoundField HeaderText="Адреса" DataField="Address" SortExpression="Address" />
                                <%-- <asp:BoundField HeaderText="Телефон" DataField="Phone" SortExpression="Phone" />
                                <asp:BoundField HeaderText="Мобилен Тел." DataField="Mobile" SortExpression="Mobile" />--%>
                                <asp:BoundField HeaderText="Место" DataField="PlaceName" SortExpression="PlaceName" />
                                <asp:CheckBoxField HeaderText="Правно лице" DataField="IsLaw" SortExpression="IsLaw" />
                            </Columns>
                            <PagerStyle HorizontalAlign="Center" />
                            <SelectedRowStyle CssClass="rowSelected" />
                            <PagerSettings FirstPageText="<< Прва " PreviousPageText="< Претходна " NextPageText=" Следна >"
                                LastPageText=" Последна >>" Mode="NextPreviousFirstLast" />
                        </cc1:GXGridView>
                        <cc1:GridViewDataSource ID="odsGridView" runat="server" TypeName="Broker.DataAccess.ViewClient"
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
                                        <cc1:FilterItem FieldName="Име и Презиме" PropertyName="Name" Comparator="StringComparators"
                                            DataType="String" />
                                        <cc1:FilterItem FieldName="Матичен Број" PropertyName="EMBG" Comparator="StringComparators"
                                            DataType="String" />
                                        <cc1:FilterItem FieldName="Адреса" PropertyName="Address" Comparator="StringComparators"
                                            DataType="String" />
                                        <cc1:FilterItem FieldName="Телефон" PropertyName="Phone" Comparator="StringComparators"
                                            DataType="String" />
                                        <cc1:FilterItem FieldName="Мобилен Тел." PropertyName="Mobile" Comparator="StringComparators"
                                            DataType="String" />
                                        <cc1:FilterItem FieldName="Место" PropertyName="PlaceName" Comparator="StringComparators"
                                            DataType="String" />
                                        <cc1:FilterItem FieldName="Општина" PropertyName="MunicipalityName" Comparator="StringComparators"
                                            DataType="String" />
                                        <cc1:FilterItem FieldName="Правно лице" PropertyName="IsLaw" Comparator="BooleanComparators"
                                            DataType="Boolean" />
                                    </cc1:FilterControl>
                                </div>
                                <cc1:FilterDataSource ID="odsFilterGridView" runat="server" TypeName="Broker.DataAccess.ViewClient">
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
                                    Нов запис во Клиенти
                                </div>
                            </InsertItemTemplate>
                            <EditItemTemplate>
                                <div class="subTitles">
                                    Измена на запис во Клиенти
                                </div>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <div class="subTitles">
                                    Преглед на запис од Клиенти
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Тип на клиент">
                            <InsertItemTemplate>
                                <asp:DropDownList ID="ddlClientTypes" runat="server" CssClass="select normal">
                                    <asp:ListItem Selected="True" Text="Физичко лице" Value="Private"></asp:ListItem>
                                    <asp:ListItem Text="Правно лице" Value="Law"></asp:ListItem>
                                    <asp:ListItem Text="Станец-физичко" Value="ForeignPrivate"></asp:ListItem>
                                    <asp:ListItem Text="Станец-правно" Value="ForeignLaw"></asp:ListItem>
                                </asp:DropDownList>
                            </InsertItemTemplate>
                            <EditItemTemplate>
                                <asp:CheckBox ID="cbIsLaw" runat="server" Checked='<%#Bind("IsLaw") %>' Enabled="false" />
                                <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%#Bind("IsForeigner") %>' Enabled="false" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="cbIsLaw" runat="server" Checked='<%#Bind("IsLaw") %>' Enabled="false" />
                                <asp:CheckBox ID="CheckBox2" runat="server" Checked='<%#Bind("IsForeigner") %>' Enabled="false" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Матичен Број">
                            <ItemTemplate>
                                <asp:TextBox ID="tbEMBG" runat="server" Text='<%# Bind("EMBG") %>' ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="tbEMBG" runat="server" Text='<%# Bind("EMBG") %>' ReadOnly="true"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvEMBG" runat="server" ControlToValidate="tbEMBG"
                                    ErrorMessage="*"></asp:RequiredFieldValidator>
                            </EditItemTemplate>
                            <InsertItemTemplate>
                                <asp:TextBox ID="tbEMBG" runat="server" Text='<%# Bind("EMBG") %>' MaxLength="100"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvEMBG" runat="server" ControlToValidate="tbEMBG"
                                    ErrorMessage="*"></asp:RequiredFieldValidator>
                                <super:EntityCallOutValidator ID="EmbgInsertValidator" PropertyName="EMBG" runat="server" />
                            </InsertItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Име и Презиме">
                            <InsertItemTemplate>
                                <asp:TextBox ID="tbName" runat="server" Text='<%# Bind("Name") %>' MaxLength="100"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="tbName"
                                    Display="Dynamic" ErrorMessage="*">
                                </asp:RequiredFieldValidator>
                            </InsertItemTemplate>
                            <ItemTemplate>
                                <asp:TextBox ID="tbName" runat="server" Text='<%# Bind("Name") %>' ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="tbName" runat="server" Text='<%# Bind("Name") %>' MaxLength="100"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="tbName"
                                    ErrorMessage="*"></asp:RequiredFieldValidator>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Адреса">
                            <ItemTemplate>
                                <asp:TextBox ID="tbAddress" runat="server" Text='<%# Bind("Address") %>' ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="tbAddress" MaxLength="100" runat="server" Text='<%# Bind("Address") %>'></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvAddress" runat="server" ControlToValidate="tbAddress"
                                    ErrorMessage="*"></asp:RequiredFieldValidator>
                            </EditItemTemplate>
                            <InsertItemTemplate>
                                <asp:TextBox ID="tbAddress" MaxLength="100" runat="server" Text='<%# Bind("Address") %>'></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvAddress" runat="server" ControlToValidate="tbAddress"
                                    ErrorMessage="*"></asp:RequiredFieldValidator>
                            </InsertItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Населено Место">
                            <ItemTemplate>
                                <asp:DropDownList ID="ddlPlaces" runat="server" Enabled="False" DataSourceID="odsPlaces"
                                    DataTextField="Name" DataValueField="ID" SelectedValue='<%# Bind("PlaceID") %>'>
                                </asp:DropDownList>
                                <asp:ObjectDataSource ID="odsPlaces" runat="server" SelectMethod="Select" TypeName="Broker.DataAccess.Place">
                                </asp:ObjectDataSource>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddlPlaces" runat="server" DataSourceID="odsPlaces" DataTextField="Name"
                                    DataValueField="ID" SelectedValue='<%# Bind("PlaceID") %>'>
                                </asp:DropDownList>
                                <asp:ObjectDataSource ID="odsPlaces" runat="server" SelectMethod="GetAllPlaces"
                                    TypeName="Broker.DataAccess.Place"></asp:ObjectDataSource>
                            </EditItemTemplate>
                            <InsertItemTemplate>
                                <asp:DropDownList ID="ddlPlaces" runat="server" DataSourceID="odsActivePlaces" DataTextField="Name"
                                    DataValueField="ID" SelectedValue='<%# Bind("PlaceID") %>'>
                                </asp:DropDownList>
                                <asp:ObjectDataSource ID="odsActivePlaces" runat="server" SelectMethod="GetActivePlaces"
                                    TypeName="Broker.DataAccess.Place"></asp:ObjectDataSource>
                            </InsertItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Телефон">
                            <ItemTemplate>
                                <asp:TextBox ID="tbPhone" runat="server" Text='<%# Bind("Phone") %>' ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="tbPhone" MaxLength="30" runat="server" Text='<%# Bind("Phone") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <InsertItemTemplate>
                                <asp:TextBox ID="tbPhone" MaxLength="30" runat="server" Text='<%# Bind("Phone") %>'></asp:TextBox>
                            </InsertItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Мобилен Тел.">
                            <ItemTemplate>
                                <asp:TextBox ID="tbMobile" runat="server" Text='<%# Bind("Mobile") %>' ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="tbMobile" MaxLength="30" runat="server" Text='<%# Bind("Mobile") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <InsertItemTemplate>
                                <asp:TextBox ID="tbMobile" MaxLength="30" runat="server" Text='<%# Bind("Mobile") %>'></asp:TextBox>
                            </InsertItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Факс">
                            <ItemTemplate>
                                <asp:TextBox ID="tbFax" runat="server" Text='<%# Bind("Fax") %>' ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="tbFax" MaxLength="30" runat="server" Text='<%# Bind("Fax") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <InsertItemTemplate>
                                <asp:TextBox ID="tbFax" MaxLength="30" runat="server" Text='<%# Bind("Fax") %>'></asp:TextBox>
                            </InsertItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Email">
                            <ItemTemplate>
                                <asp:TextBox ID="tbEmail" runat="server" Text='<%# Bind("Email") %>' ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="tbEmail" MaxLength="100" runat="server" Text='<%# Bind("Email") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <InsertItemTemplate>
                                <asp:TextBox ID="tbEmail" MaxLength="100" runat="server" Text='<%# Bind("Email") %>'></asp:TextBox>
                            </InsertItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Button ID="btnCancel" runat="server" CommandName="Cancel" Text="Откажи" CausesValidation="false" />
                                <%--<asp:Button ID="btnDelete" runat="server" Text="Избриши" OnClientClick="return confirm('Дали сте сигурни?')"
                                    OnClick="btnDelete_Click1" />--%>
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
                <cc1:DetailsViewDataSource ID="dvDataSource" runat="server" TypeName="Broker.DataAccess.Client"
                    ConflictDetection="CompareAllValues" DataObjectTypeName="Broker.DataAccess.Client"
                    DeleteMethod="Delete" InsertMethod="Insert" SelectMethod="Get" UpdateMethod="Update"
                    OnUpdating="dvDataSource_Updating" OnUpdated="dvDataSource_Updated" OnInserted="dvDataSource_Inserted"
                    OnInserting="dvDataSource_Inserting">
                    <UpdateParameters>
                        <asp:Parameter Name="oldEntity" Type="Object" />
                        <asp:Parameter Name="newEntity" Type="Object" />
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
                    <cc1:SearchItem FieldName="Име и Презиме" PropertyName="Name" Comparator="StringComparators"
                        DataType="String" />
                    <cc1:SearchItem FieldName="Матичен Број" PropertyName="EMBG" Comparator="StringComparators"
                        DataType="String" />
                    <cc1:SearchItem FieldName="Адреса" PropertyName="Address" Comparator="StringComparators"
                        DataType="String" />
                    <cc1:SearchItem FieldName="Телефон" PropertyName="Phone" Comparator="StringComparators"
                        DataType="String" />
                    <cc1:SearchItem FieldName="Мобилен Тел." PropertyName="Mobile" Comparator="StringComparators"
                        DataType="String" />
                    <cc1:SearchItem FieldName="Факс" PropertyName="Fax" Comparator="StringComparators"
                        DataType="String" />
                    <cc1:SearchItem FieldName="Email" PropertyName="Email" Comparator="StringComparators"
                        DataType="String" />
                    <cc1:SearchItem FieldName="Место" PropertyName="PlaceName" Comparator="StringComparators"
                        DataType="String" />
                    <cc1:SearchItem FieldName="Општина" PropertyName="MunicipalityName" Comparator="StringComparators"
                        DataType="String" />
                    <cc1:SearchItem FieldName="Правно лице" PropertyName="IsLaw" Comparator="BooleanComparators"
                        DataType="Boolean" />
                </cc1:SearchControl>
                <cc1:GridViewDataSource ID="odsSearch" runat="server" TypeName="Broker.DataAccess.ViewClient"
                    SelectCountMethod="SelectSearchCountCached" SelectMethod="SelectSearch">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="SearchControl1" PropertyName="SearchArguments" Name="sArgument" />
                    </SelectParameters>
                </cc1:GridViewDataSource>
            </div>
        </asp:View>
        <asp:View ID="viewReport" runat="server">
            <div class="paddingKontroli">
                <cc1:ReportControl ID="reportControl" runat="server" TypeName="Broker.DataAccess.ViewClient"
                    GridViewID="GXGridView1" SearchControlID="SearchControl1">
                    <cc1:PrintItem HeaderText="Име и Презиме" PropertyName="Name" />
                    <cc1:PrintItem HeaderText="Матичен Број" PropertyName="EMBG" />
                    <cc1:PrintItem HeaderText="Адреса" PropertyName="Address" />
                    <cc1:PrintItem HeaderText="Телефон" PropertyName="Phone" />
                    <cc1:PrintItem HeaderText="Мобилен Тел." PropertyName="Mobile" />
                    <cc1:PrintItem HeaderText="Факс" PropertyName="Fax" />
                    <cc1:PrintItem HeaderText="Email" PropertyName="EMail" />
                    <cc1:PrintItem HeaderText="Место" PropertyName="PlaceName" />
                    <cc1:PrintItem HeaderText="Општина" PropertyName="MunicipalityName" />
                    <cc1:PrintItem HeaderText="Правно лице" PropertyName="IsLaw" />
                </cc1:ReportControl>
            </div>
        </asp:View>
        <asp:View ID="viewClientAccounts" runat="server">
            <div class="paddingKontroli">
                <asp:DetailsView ID="dvClientPreview" runat="server" DataSourceID="odsClientPreview"
                    DataKeyNames="ID" AutoGenerateRows="False" OnItemCommand="dvClientPreview_ItemCommand"
                    OnModeChanging="dvClientPreview_ModeChanging" GridLines="None" DefaultMode="ReadOnly">
                    <Fields>
                        <asp:TemplateField HeaderText="Матичен број">
                            <ItemTemplate>
                                <asp:TextBox ID="tbEMBG" runat="server" Text='<%# Eval("EMBG") %>' ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Име и презиме / назив">
                            <ItemTemplate>
                                <asp:TextBox ID="tbName" runat="server" Text='<%# Eval("Name") %>' ReadOnly="true"
                                    Width="300px"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Адреса">
                            <ItemTemplate>
                                <asp:TextBox ID="tbAddress" runat="server" Text='<%# Eval("Address") %>' ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Место">
                            <ItemTemplate>
                                <asp:TextBox ID="tbPlaceName" runat="server" Text='<%# Eval("Place.Name") %>' ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Fields>
                </asp:DetailsView>
                <cc1:DetailsViewDataSource ID="odsClientPreview" runat="server" TypeName="Broker.DataAccess.Client"
                    ConflictDetection="CompareAllValues" DataObjectTypeName="Broker.DataAccess.Client"
                    SelectMethod="Get" OldValuesParameterFormatString="oldEntity" OnSelecting="odsClientPreview_Selecting">
                </cc1:DetailsViewDataSource>
                <asp:MultiView ID="mvClientAccounts" runat="server">
                    <asp:View ID="viewClientAccountsGrid" runat="server">
                        <asp:GridView ID="GridViewClientAccounts" runat="server" DataSourceID="odsGridViewClientAccounts"
                            DataKeyNames="ID" Width="100%" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"
                            Caption="Жиро-сметки на клиент" EmptyDataText="Нема записи кои го задоволуваат критериумот на пребарување!"
                            RowStyle-CssClass="row" CssClass="grid" GridLines="None">
                            <RowStyle CssClass="row"></RowStyle>
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:TextBox ID="tbClientID" runat="server" Text='<%#Bind("ClientID") %>' Visible="false"></asp:TextBox>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="tbClientID" runat="server" Text='<%#Bind("ClientID") %>' Visible="false"></asp:TextBox>
                                    </EditItemTemplate>
                                    <InsertItemTemplate>
                                        <asp:TextBox ID="tbClientID" runat="server" Text='<%#Bind("ClientID") %>' Visible="false"></asp:TextBox>
                                    </InsertItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Банка">
                                    <InsertItemTemplate>
                                        <asp:DropDownList ID="ddlBanks" runat="server" DataSourceID="odsBanks" DataTextField="Name"
                                            Width="70px" DataValueField="ID" SelectedValue='<%# Bind("BankID") %>'>
                                        </asp:DropDownList>
                                        <asp:ObjectDataSource ID="odsBanks" runat="server" SelectMethod="GetActiveBanks"
                                            TypeName="Broker.DataAccess.Bank"></asp:ObjectDataSource>
                                    </InsertItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="lblBank" runat="server" Text='<%#Eval("Bank.Name") %>'></asp:Label>
                                        <asp:TextBox ID="tbBank" runat="server" Text='<%#Bind("BankID") %>' ReadOnly="true"
                                            Visible="false"></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblBank" runat="server" Text='<%#Eval("Bank.Name") %>'></asp:Label>
                                        <asp:TextBox ID="tbBank" runat="server" Text='<%#Bind("BankID") %>' ReadOnly="true"
                                            Visible="false"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Број на жиро-сметка">
                                    <InsertItemTemplate>
                                        <asp:TextBox ID="tbAccountNumber" runat="server" Text='<%#Bind("AccountNumber") %>'></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvAccountNumber" runat="server" ErrorMessage="*"
                                            Display="Dynamic" ControlToValidate="tbAccountNumber"></asp:RequiredFieldValidator>
                                    </InsertItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="tbAccountNumber" runat="server" Text='<%#Bind("AccountNumber") %>'></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvAccountNumber" runat="server" ErrorMessage="*"
                                            Display="Dynamic" ControlToValidate="tbAccountNumber"></asp:RequiredFieldValidator>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="tbAccountNumber" runat="server" Text='<%#Bind("AccountNumber") %>'
                                            ReadOnly="true"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Активна">
                                    <InsertItemTemplate>
                                        <asp:CheckBox ID="cbIsActive" runat="server" Checked='<%#Bind("IsActive") %>' />
                                    </InsertItemTemplate>
                                    <EditItemTemplate>
                                        <asp:CheckBox ID="cbIsActive" runat="server" Checked='<%#Bind("IsActive") %>' />
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="cbIsActive" runat="server" Checked='<%#Bind("IsActive") %>' Enabled="false" />
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
                        <asp:ObjectDataSource ID="odsGridViewClientAccounts" runat="server" TypeName="Broker.DataAccess.ClientAccount"
                            OldValuesParameterFormatString="oldEntity" DataObjectTypeName="Broker.DataAccess.ClientAccount"
                            ConflictDetection="CompareAllValues" DeleteMethod="Delete" InsertMethod="Insert"
                            SelectMethod="GetByClientID" UpdateMethod="Update" OnSelecting="odsGridViewClientAccounts_Selecting">
                            <UpdateParameters>
                                <asp:Parameter Name="oldEntity" Type="Object" />
                                <asp:Parameter Name="newEntity" Type="Object" />
                            </UpdateParameters>
                        </asp:ObjectDataSource>
                        <div>
                            <asp:Button ID="btnNewClientAccount" runat="server" Text="Нова сметка" OnClick="btnNewClientAccount_Click" />
                        </div>
                    </asp:View>
                    <asp:View ID="viewClientAccountsEdit" runat="server">
                        <asp:DetailsView ID="DetailsViewClientAccount" runat="server" DataSourceID="dvDataSourceClientAccount"
                            DataKeyNames="ID" AutoGenerateRows="False" OnItemCommand="DetailsViewClientAccount_ItemCommand"
                            OnItemInserted="DetailsViewClientAccount_ItemInserted" OnModeChanging="DetailsViewClientAccount_ModeChanging"
                            OnItemInserting="DetailsViewClientAccount_ItemInserting" GridLines="None" DefaultMode="Insert">
                            <Fields>
                                <asp:TemplateField>
                                    <InsertItemTemplate>
                                        <div class="subTitles">
                                            Нова жиро-сметка
                                        </div>
                                    </InsertItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Банка">
                                    <InsertItemTemplate>
                                        <asp:DropDownList ID="ddlBanks" runat="server" DataSourceID="odsBanks" DataTextField="Name"
                                            DataValueField="ID" SelectedValue='<%# Bind("BankID") %>'>
                                        </asp:DropDownList>
                                        <asp:ObjectDataSource ID="odsBanks" runat="server" SelectMethod="GetActiveBanks"
                                            TypeName="Broker.DataAccess.Bank"></asp:ObjectDataSource>
                                        <super:EntityCallOutValidator ID="ClientAccountInsertValidator" PropertyName="ClientAccountValidator"
                                            runat="server" />
                                    </InsertItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Број на жиро-сметка">
                                    <InsertItemTemplate>
                                        <asp:TextBox ID="tbAccountNumber" runat="server" Text='<%#Bind("AccountNumber") %>'></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvAccountNumber" runat="server" ControlToValidate="tbAccountNumber"
                                            ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                    </InsertItemTemplate>
                                </asp:TemplateField>
                                <%-- <asp:TemplateField HeaderText="Активна">
                                    <InsertItemTemplate>
                                        <asp:CheckBox ID="cbIsActive" runat="server" Checked='<%#Bind("IsActive") %>' />
                                    </InsertItemTemplate>
                                </asp:TemplateField>--%>
                                <asp:TemplateField>
                                    <InsertItemTemplate>
                                        <asp:Button ID="btnCancel" runat="server" CommandName="Cancel" Text="Откажи" CausesValidation="false" />
                                        <asp:Button ID="btnInsert" runat="server" CommandName="Insert" Text="Внеси" />
                                    </InsertItemTemplate>
                                </asp:TemplateField>
                            </Fields>
                        </asp:DetailsView>
                        <cc1:DetailsViewDataSource ID="dvDataSourceClientAccount" runat="server" TypeName="Broker.DataAccess.ClientAccount"
                            ConflictDetection="CompareAllValues" DataObjectTypeName="Broker.DataAccess.ClientAccount"
                            InsertMethod="Insert" SelectMethod="Get" OnInserted="dvDataSourceClientAccount_Inserted"
                            OnInserting="dvDataSourceClientAccount_Inserting">
                            <InsertParameters>
                                <asp:Parameter Name="entityToInsert" Type="Object" />
                            </InsertParameters>
                            <SelectParameters>
                                <asp:ControlParameter ControlID="GridViewClientAccounts" Name="id" PropertyName="SelectedValue"
                                    Type="Int32" />
                            </SelectParameters>
                        </cc1:DetailsViewDataSource>
                    </asp:View>
                </asp:MultiView>
            </div>
        </asp:View>
        <asp:View ID="viewAccreditation" runat="server">
            <div class="paddingKontroli">
                <asp:DetailsView ID="dvClientForAccreditation" runat="server" DataSourceID="odsClientForAccreditation"
                    DataKeyNames="ID" AutoGenerateRows="False" OnModeChanging="dvClientForAccreditation_ModeChanging"
                    GridLines="None" DefaultMode="ReadOnly">
                    <Fields>
                        <asp:TemplateField HeaderText="Матичен број">
                            <ItemTemplate>
                                <asp:TextBox ID="tbEMBG" runat="server" Text='<%# Eval("EMBG") %>' ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Име и презиме / назив">
                            <ItemTemplate>
                                <asp:TextBox ID="tbName" runat="server" Text='<%# Eval("Name") %>' Width="300px"
                                    ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Адреса">
                            <ItemTemplate>
                                <asp:TextBox ID="tbAddress" runat="server" Text='<%# Eval("Address") %>' ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Место">
                            <ItemTemplate>
                                <asp:TextBox ID="tbPlaceName" runat="server" Text='<%# Eval("Place.Name") %>' ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Fields>
                </asp:DetailsView>
                <cc1:DetailsViewDataSource ID="odsClientForAccreditation" runat="server" TypeName="Broker.DataAccess.Client"
                    ConflictDetection="CompareAllValues" DataObjectTypeName="Broker.DataAccess.Client"
                    SelectMethod="Get" OldValuesParameterFormatString="oldEntity" OnSelecting="odsClientForAccreditation_Selecting">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="GXGridView1" Name="id" PropertyName="SelectedValue"
                            Type="Int32" />
                    </SelectParameters>
                </cc1:DetailsViewDataSource>
            </div>
            <asp:GridView ID="GridViewAccreditations" runat="server" DataSourceID="odsGridViewAccreditations"
                DataKeyNames="ID" Width="100%" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"
                Caption="Овластувања за застапништво" EmptyDataText="Нема записи кои го задоволуваат критериумот на пребарување!"
                RowStyle-CssClass="row" CssClass="grid" GridLines="None" OnSelectedIndexChanged="GridViewAccreditations_SelectedIndexChanged">
                <RowStyle CssClass="row"></RowStyle>
                <Columns>
                    <asp:TemplateField HeaderText="Наслов">
                        <ItemTemplate>
                            <asp:Label ID="lblTitle" runat="server" Text='<%#Eval("Title") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Од дата">
                        <ItemTemplate>
                            <asp:Label ID="lblFromDate" runat="server" Text='<%#Eval("FromDate","{0:d}") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="До дата">
                        <ItemTemplate>
                            <asp:Label ID="lblToDate" runat="server" Text='<%#Eval("ToDate","{0:d}") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:CommandField ButtonType="Link" SelectText="Печати" ShowSelectButton="True" CancelText="Откажи"
                        ShowCancelButton="true" />
                </Columns>
                <PagerStyle HorizontalAlign="Center" />
                <%--<SelectedRowStyle CssClass="rowSelected" />--%>
                <PagerSettings FirstPageText="<< Прва " PreviousPageText="< Претходна " NextPageText=" Следна >"
                    LastPageText=" Последна >>" Mode="NextPreviousFirstLast" />
            </asp:GridView>
            <asp:ObjectDataSource ID="odsGridViewAccreditations" runat="server" TypeName="Broker.DataAccess.Accreditation"
                OldValuesParameterFormatString="oldEntity" DataObjectTypeName="Broker.DataAccess.Accreditation"
                ConflictDetection="CompareAllValues" DeleteMethod="Delete" InsertMethod="Insert"
                SelectMethod="GetByClient">
                <SelectParameters>
                    <asp:ControlParameter ControlID="GXGridView1" Name="clientID" PropertyName="SelectedValue"
                        Type="Int32" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:Button ID="btnNewAccreditation" runat="server" Text="Нов запис" OnClick="btnNewAccreditation_Click" />
            <asp:Panel ID="pnlNewAccreditation" runat="server" Visible="false">
                <table>
                    <tr>
                        <td>
                            Наслов
                        </td>
                        <td>
                            <asp:TextBox ID="tbTitle" runat="server" MaxLength="50"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Од дата
                        </td>
                        <td>
                            <asp:TextBox ID="tbFromDate" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" ErrorMessage="*" ControlToValidate="tbFromDate"
                                Display="Dynamic"></asp:RequiredFieldValidator>
                            <asp:CompareValidator ID="cvFromDate" runat="server" ErrorMessage="*" ControlToValidate="tbFromDate"
                                Display="Dynamic" Operator="DataTypeCheck" Type="Date"></asp:CompareValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            До дата
                        </td>
                        <td>
                            <asp:TextBox ID="tbToDate" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvToDate" runat="server" ErrorMessage="*" ControlToValidate="tbToDate"
                                Display="Dynamic"></asp:RequiredFieldValidator>
                            <asp:CompareValidator ID="cvToDate" runat="server" ErrorMessage="*" ControlToValidate="tbToDate"
                                Display="Dynamic" Operator="DataTypeCheck" Type="Date"></asp:CompareValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Типови на осигурување
                        </td>
                        <td>
                            <asp:CheckBox ID="cbInsuranceTypesAll" Text="Сите" runat="server" AutoPostBack="true"
                                OnCheckedChanged="cbInsuranceTypesAll_CheckedChanged" />
                            <asp:CheckBoxList runat="server" ID="cblInsuranceTypes" DataSourceID="odsInsuranceTypes"
                                DataTextField="Name" DataValueField="ID">
                            </asp:CheckBoxList>
                            <asp:ObjectDataSource ID="odsInsuranceTypes" runat="server" SelectMethod="Select"
                                TypeName="Broker.DataAccess.InsuranceType"></asp:ObjectDataSource>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:Button ID="btnInsertAccreditation" runat="server" Text="Внеси" OnClick="btnInsertAccreditation_Click" />
                            <asp:Button ID="btnCancelAccreditation" CausesValidation="false" runat="server" Text="Откажи"
                                OnClick="btnCancelAccreditation_Click" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </asp:View>
        <asp:View ID="viewFinancialCard" runat="server">
            <div class="paddingKontroli">
                <asp:DetailsView ID="dvClientPriviewForFinCard" runat="server" DataSourceID="odsClientPreview"
                    DataKeyNames="ID" AutoGenerateRows="False" OnItemCommand="dvClientPreview_ItemCommand"
                    OnModeChanging="dvClientPreview_ModeChanging" GridLines="None" DefaultMode="ReadOnly">
                    <Fields>
                        <asp:TemplateField HeaderText="Матичен број">
                            <ItemTemplate>
                                <asp:TextBox ID="tbEMBG" runat="server" Text='<%# Eval("EMBG") %>' ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Име и презиме / назив">
                            <ItemTemplate>
                                <asp:TextBox ID="tbName" runat="server" Text='<%# Eval("Name") %>' Width="300px"
                                    ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Адреса">
                            <ItemTemplate>
                                <asp:TextBox ID="tbAddress" runat="server" Text='<%# Eval("Address") %>' ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Место">
                            <ItemTemplate>
                                <asp:TextBox ID="tbPlaceName" runat="server" Text='<%# Eval("Place.Name") %>' ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Fields>
                </asp:DetailsView>
                <asp:RadioButtonList ID="rblItemsTypes" runat="server" RepeatDirection="Horizontal"
                    AutoPostBack="true" OnSelectedIndexChanged="rblItemsTypes_SelectedIndexChanged">
                    <asp:ListItem Selected="True" Value="OpenItems" Text="Отворени ставки"></asp:ListItem>
                    <asp:ListItem Value="AllItems" Text="Сите ставки"></asp:ListItem>
                </asp:RadioButtonList>
            </div>
            <div id="tabeliFrame">
                <div id="header">
                    <div id="content">
                        <asp:Label ID="Label2" runat="server" Text="Финансова картица"></asp:Label>
                    </div>
                </div>
                <div id="contentOuter">
                    <div id="contentInner">
                        <asp:GridView ID="GridViewMainFinCard" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                            DataKeyNames="ID" DataSourceID="odsPolPerClient" PageSize="20" OnRowDataBound="GridViewMainFinCard_RowDataBound"
                            GridLines="None" RowStyle-CssClass="row" CssClass="grid" OnRowCommand="GridViewMainFinCard_RowCommand"
                            OnRowEditing="GridViewMainFinCard_RowEditing" OnSelectedIndexChanging="GridViewMainFinCard_SelectedIndexChanging">
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <a href="javascript:ShowChildGrid('div<%# Eval("ID") %>');">
                                            <img id="imgdiv<%# Eval("ID") %>" alt="" border="0" src="../_assets/img/arrowdown.jpeg" />
                                        </a>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="policynumber" HeaderText="Број на полиса" SortExpression="policynumber" />
                                <asp:BoundField DataField="applicationdate" HeaderText="Датум" DataFormatString="{0:d}"
                                    SortExpression="applicationdate" />
                                <asp:BoundField DataField="dolzi" HeaderText="Должи" SortExpression="dolzi" DataFormatString="{0:#,0.00}"
                                    ItemStyle-CssClass="debtClass" />
                                <asp:BoundField DataField="pobaruva" HeaderText="Побарува" SortExpression="pobaruva"
                                    DataFormatString="{0:#,0.00}" ItemStyle-CssClass="demandClass" />
                                <asp:BoundField DataField="saldo" HeaderText="Салдо" SortExpression="saldo" DataFormatString="{0:#,0.00}"
                                    ItemStyle-CssClass="saldoClass" />
                                <asp:ButtonField ButtonType="Link" Text="Плаќања" CommandName="Select" />
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        </td> </tr>
                                        <tr>
                                            <td colspan="100%">
                                                <div id="div<%# Eval("ID") %>" style="display: none; position: relative; left: 25px;">
                                                    <asp:GridView ID="GridViewFinCard" runat="server" AutoGenerateColumns="False" GridLines="None"
                                                        EmptyDataText="Нема записи!" RowStyle-CssClass="row" CssClass="grid" Width="620px">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Дата">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="tbDocumentDate" Width="64px" runat="server" Text='<%#Bind("DocumentDate", "{0:d}") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Доспева на">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="tbPaidDate" Width="64px" runat="server" Text='<%#Bind("PaidDate","{0:d}") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Опис">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="tbDescription" Width="150px" runat="server" Text='<%#Bind("Description") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Должи">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="tbDebtValue" runat="server" Width="55px" Text='<%#Bind("DebtValue", "{0:#,0.00}") %>'
                                                                        CssClass="debtClass"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Побарува">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="tbDemandValue" runat="server" Width="55px" Text='<%#Bind("DemandValue", "{0:#,0.00}") %>'
                                                                        CssClass="demandClass"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Салдо">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="tbSaldoValue" runat="server" Width="55px" Text='<%#Bind("SaldoValue", "{0:#,0.00}") %>'
                                                                        CssClass="saldoClass"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                    <asp:ObjectDataSource ID="odsFinCard" runat="server" SelectMethod="GetByClient" TypeName="Broker.Controllers.FinanceControllers.FinanceCardController"
                                                        OnSelecting="odsFinCard_Selecting"></asp:ObjectDataSource>
                                                </div>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <asp:ObjectDataSource ID="odsPolPerClient" runat="server" SelectMethod="GetForFinCard"
                            DataObjectTypeName="Broker.DataAccess.Policy" TypeName="Broker.DataAccess.Policy"
                            OnSelecting="odsPolPerClient_Selecting"></asp:ObjectDataSource>
                    </div>
                </div>
            </div>
            <asp:Button ID="btnPrintFinCard" runat="server" Text="Печати" OnClick="btnPrintFinCard_Click" />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Label ID="lblDebtValue" runat="server" Text="Должи"></asp:Label>
            <asp:TextBox ID="tbDebtValue" runat="server" ReadOnly="true" Width="100px" CssClass="debtClass"></asp:TextBox>
            <asp:Label ID="lblDemandValue" runat="server" Text="Побарува"></asp:Label>
            <asp:TextBox ID="tbDemandValue" runat="server" ReadOnly="true" Width="100px" CssClass="demandClass"></asp:TextBox>
            <asp:Label ID="lblSaldoValue" runat="server" Text="Салдо"></asp:Label>
            <asp:TextBox ID="tbSaldoValue" runat="server" ReadOnly="true" Width="100px" CssClass="saldoClass"></asp:TextBox>
        </asp:View>
        <asp:View ID="viewPaymentsForPolicy" runat="server">
            <div id="tabeliFrame">
                <div id="header">
                    <div id="content">
                        <asp:Label ID="Label1" runat="server" Text="Договорени рати"></asp:Label>
                    </div>
                </div>
                <div id="contentOuter">
                    <div id="contentInner">
                        <asp:GridView ID="gvRates" runat="server" GridLines="None" DataSourceID="odsRates"
                            Caption="Договорени рати" EmptyDataText="Нема записи!" AutoGenerateColumns="false"
                            AllowPaging="true" AllowSorting="false" RowStyle-CssClass="row" CssClass="grid"
                            OnRowUpdating="gvRates_RowUpdating" OnRowEditing="gvRates_RowEditing" OnRowUpdated="gvRates_RowUpdated">
                            <RowStyle CssClass="row" />
                            <Columns>
                                <asp:TemplateField HeaderText="Број на рата">
                                    <ItemTemplate>
                                        <asp:Label ID="tbNumber" runat="server" ReadOnly="true" Text='<%#Bind("Number") %>'
                                            Width="30px"></asp:Label>
                                        <asp:TextBox ID="tbID" runat="server" Text='<%#Bind("ID") %>' ReadOnly="true" Width="0px"
                                            Visible="false" ForeColor="Transparent" BorderColor="Transparent"></asp:TextBox>
                                        <asp:TextBox ID="tbBrokerageValue" runat="server" Text='<%#Bind("BrokerageValue") %>'
                                            Visible="false" ReadOnly="true" Width="0px" ForeColor="Transparent" BorderColor="Transparent"></asp:TextBox>
                                        <asp:TextBox ID="tbPaidValue" runat="server" Text='<%#Bind("PaidValue") %>' ReadOnly="true"
                                            Visible="false" Width="0px" ForeColor="Transparent" BorderColor="Transparent"></asp:TextBox>
                                        <asp:TextBox ID="tbPolicyItemID" runat="server" Text='<%#Bind("PolicyItemID") %>'
                                            ReadOnly="true" Visible="false" Width="0px" ForeColor="Transparent" BorderColor="Transparent"></asp:TextBox>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="tbNumber" runat="server" ReadOnly="true" Text='<%#Bind("Number") %>'
                                            Width="30px"></asp:Label>
                                        <asp:TextBox ID="tbID" runat="server" Text='<%#Bind("ID") %>' ReadOnly="true" Width="0px"
                                            Visible="false" ForeColor="Transparent" BorderColor="Transparent"></asp:TextBox>
                                        <asp:TextBox ID="tbBrokerageValue" runat="server" Text='<%#Bind("BrokerageValue") %>'
                                            Visible="false" ReadOnly="true" Width="0px" ForeColor="Transparent" BorderColor="Transparent"></asp:TextBox>
                                        <asp:TextBox ID="tbPaidValue" runat="server" Text='<%#Bind("PaidValue") %>' ReadOnly="true"
                                            Visible="false" Width="0px" ForeColor="Transparent" BorderColor="Transparent"></asp:TextBox>
                                        <asp:TextBox ID="tbPolicyItemID" runat="server" Text='<%#Bind("PolicyItemID") %>'
                                            ReadOnly="true" Visible="false" Width="0px" ForeColor="Transparent" BorderColor="Transparent"></asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Дата">
                                    <ItemTemplate>
                                        <asp:Label ID="tbDate" runat="server" ReadOnly="true" Text='<%#Bind("Date", "{0:d}") %>'
                                            Width="100px"></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="tbDate" runat="server" Text='<%#Bind("Date", "{0:d}") %>' Width="100px"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvDate" runat="server" Display="Dynamic" ErrorMessage="*"
                                            ControlToValidate="tbDate"></asp:RequiredFieldValidator>
                                        <asp:CompareValidator ID="cvDate" runat="server" Display="Dynamic" ErrorMessage="*"
                                            ControlToValidate="tbDate" Operator="DataTypeCheck" Type="Date"></asp:CompareValidator>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Износ" ControlStyle-CssClass="currencyClass">
                                    <ItemTemplate>
                                        <asp:Label ID="tbValue" runat="server" ReadOnly="true" Text='<%#Bind("Value" , "{0:#,0.00}") %>'
                                            Width="100px"></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="tbValue" runat="server" Text='<%#Bind("Value") %>' Width="100px"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvValue" runat="server" Display="Dynamic" ErrorMessage="*"
                                            ControlToValidate="tbValue"></asp:RequiredFieldValidator>
                                        <asp:CompareValidator ID="cvValue" runat="server" Display="Dynamic" ErrorMessage="*"
                                            ControlToValidate="tbValue" Operator="DataTypeCheck" Type="Double"></asp:CompareValidator>
                                        <super:EntityCallOutValidator ID="RateValueValidator" PropertyName="RateValue" runat="server" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:CommandField CancelText="Откажи" DeleteText="Избриши" EditText="Измени" InsertText="Внеси"
                                    InsertVisible="False" NewText="Нов" SelectText="Одбери" ShowEditButton="True"
                                    UpdateText="Ажурирај" />
                            </Columns>
                            <PagerStyle HorizontalAlign="Center" />
                            <%--<SelectedRowStyle CssClass="rowSelected" />--%>
                            <PagerSettings FirstPageText="<< Прва " PreviousPageText="< Претходна " NextPageText=" Следна >"
                                LastPageText=" Последна >>" Mode="NextPreviousFirstLast" />
                        </asp:GridView>
                        <asp:ObjectDataSource ID="odsRates" runat="server" UpdateMethod="UpdateExtend" SelectMethod="GetByPolicyItemID"
                            DataObjectTypeName="Broker.DataAccess.Rate" TypeName="Broker.DataAccess.Rate"
                            OnSelecting="odsRates_Selecting" OnUpdating="odsRates_Updating" OldValuesParameterFormatString="oldEntity"
                            ConflictDetection="CompareAllValues">
                            <UpdateParameters>
                                <asp:Parameter Name="oldEntity" Type="Object" />
                                <asp:Parameter Name="newEntity" Type="Object" />
                                <asp:Parameter Name="Value" Type="Decimal" />
                                <asp:Parameter Name="BrokerageValue" Type="Decimal" />
                                <asp:Parameter Name="PaidValue" Type="Decimal" />
                                <asp:Parameter Name="Date" Type="DateTime" />
                            </UpdateParameters>
                        </asp:ObjectDataSource>
                    </div>
                </div>
            </div>
            <div id="tabeliFrame">
                <div id="header">
                    <div id="content">
                        <asp:Label ID="Label3" runat="server" Text="Плаќања"></asp:Label>
                    </div>
                </div>
                <div id="contentOuter">
                    <div id="contentInner">
                        <asp:GridView ID="gvPayments" runat="server" GridLines="None" DataSourceID="odsPayments"
                            Caption="Плаќања" EmptyDataText="Нема записи!" AutoGenerateColumns="false" AllowPaging="true"
                            AllowSorting="false" RowStyle-CssClass="row" CssClass="grid" OnRowUpdating="gvPayments_RowUpdating"
                            OnRowUpdated="gvPayments_RowUpdated" OnRowEditing="gvPayments_RowEditing" OnDataBinding="gvPayments_DataBinding"
                            OnRowCommand="gvPayments_RowCommand" OnRowCreated="gvPayments_RowCreated" OnRowDataBound="gvPayments_RowDataBound"
                            OnRowDeleted="gvPayments_RowDeleted" OnRowDeleting="gvPayments_RowDeleting">
                            <RowStyle CssClass="row" />
                            <Columns>
                                <asp:TemplateField HeaderText="За рата">
                                    <ItemTemplate>
                                        <asp:TextBox ID="tbRateID" runat="server" ReadOnly="true" Text='<%#Bind("RateID") %>'
                                            Visible="false" Width="0px" ForeColor="Transparent" BorderColor="Transparent"></asp:TextBox>
                                        <asp:Label ID="tbRateNumber" runat="server" Text='<%#Eval("Rate.Number") %>' ReadOnly="true"
                                            Width="30px"></asp:Label>
                                        <asp:TextBox ID="tbID" runat="server" Text='<%#Bind("ID") %>' ReadOnly="true" Width="0px"
                                            ForeColor="Transparent" BorderColor="Transparent" Visible="false"></asp:TextBox>
                                        <asp:TextBox ID="tbUserID" runat="server" Text='<%#Bind("UserID") %>' ReadOnly="true"
                                            Width="0px" ForeColor="Transparent" BorderColor="Transparent" Visible="false"></asp:TextBox>
                                        <asp:TextBox ID="tbBranchID" runat="server" Text='<%#Bind("BranchID") %>' ReadOnly="true"
                                            Width="0px" ForeColor="Transparent" BorderColor="Transparent" Visible="false"></asp:TextBox>
                                        <asp:CheckBox ID="cbIsFactured" runat="server" Width="0px" ForeColor="Transparent"
                                            BorderColor="Transparent" Checked='<%#Bind("IsFactured") %>' Enabled="false"
                                            Visible="false" />
                                        <asp:CheckBox ID="cbIsCashReported" runat="server" Width="0px" ForeColor="Transparent"
                                            BorderColor="Transparent" Checked='<%#Bind("IsCashReported") %>' Enabled="false"
                                            Visible="false" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="tbRateID" runat="server" ReadOnly="true" Text='<%#Bind("RateID") %>'
                                            Visible="false" Width="0px" ForeColor="Transparent" BorderColor="Transparent"></asp:TextBox>
                                        <asp:Label ID="tbRateNumber" runat="server" Text='<%#Eval("Rate.Number") %>' ReadOnly="true"
                                            Width="30px"></asp:Label>
                                        <asp:TextBox ID="tbID" runat="server" Text='<%#Bind("ID") %>' ReadOnly="true" Width="0px"
                                            Visible="false" ForeColor="Transparent" BorderColor="Transparent"></asp:TextBox>
                                        <asp:TextBox ID="tbUserID" runat="server" Text='<%#Bind("UserID") %>' ReadOnly="true"
                                            Width="0px" ForeColor="Transparent" BorderColor="Transparent" Visible="false"></asp:TextBox>
                                        <asp:TextBox ID="tbBranchID" runat="server" Text='<%#Bind("BranchID") %>' ReadOnly="true"
                                            Width="0px" ForeColor="Transparent" BorderColor="Transparent" Visible="false"></asp:TextBox>
                                        <asp:CheckBox ID="cbIsFactured" runat="server" Width="0px" ForeColor="Transparent"
                                            Visible="false" BorderColor="Transparent" Checked='<%#Bind("IsFactured") %>'
                                            Enabled="false" />
                                        <asp:CheckBox ID="cbIsCashReported" runat="server" Width="0px" ForeColor="Transparent"
                                            Visible="false" BorderColor="Transparent" Checked='<%#Bind("IsCashReported") %>'
                                            Enabled="false" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Тип на плаќање">
                                    <ItemTemplate>
                                        <asp:Label ID="tbPaymentTypeName" runat="server" Text='<%#Eval("PaymentType.Name") %>'
                                            Width="80px"></asp:Label>
                                        <asp:DropDownList ID="ddlPaymentTypes" runat="server" SelectedValue='<%#Bind("PaymentTypeID") %>'
                                            DataTextField="Name" DataValueField="ID" DataSourceID="odsPaymentTypes" Enabled="false"
                                            Width="80px" Visible="false">
                                        </asp:DropDownList>
                                        <asp:ObjectDataSource ID="odsPaymentTypes" runat="server" DataObjectTypeName="Broker.DataAccess.PaymentType"
                                            TypeName="Broker.DataAccess.PaymentType" SelectMethod="Select"></asp:ObjectDataSource>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlPaymentTypes" runat="server" SelectedValue='<%#Bind("PaymentTypeID") %>'
                                            DataTextField="Name" DataValueField="ID" DataSourceID="odsPaymentTypes" Width="80px"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddlPaymentTypesInpaymentsPerPolicy_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:ObjectDataSource ID="odsPaymentTypes" runat="server" DataObjectTypeName="Broker.DataAccess.PaymentType"
                                            TypeName="Broker.DataAccess.PaymentType" SelectMethod="Select"></asp:ObjectDataSource>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Кредитна картица">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCreditCardNameInPayment" runat="server"></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlBankCreditCards" runat="server" DataSourceID="odsBankCreditCards"
                                            DataTextField="Name" DataValueField="ID" Width="80px" AppendDataBoundItems="true">
                                            <asp:ListItem Selected="True" Text="" Value="None"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:ObjectDataSource ID="odsBankCreditCards" runat="server" SelectMethod="GetAll"
                                            TypeName="Broker.DataAccess.BankCreditCard"></asp:ObjectDataSource>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Извод/банка">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBankslipNumberInPayment" runat="server" Text='<%#Eval("BankslipNumber") %>'></asp:Label>
                                        <asp:Label ID="lblBankslipBankName" runat="server"></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="tbBankslipNumber" runat="server" Text='<%#Bind("BankslipNumber") %>'
                                            Width="75px"></asp:TextBox>
                                        <asp:DropDownList ID="ddlBanks" runat="server" DataTextField="Name" DataValueField="ID"
                                            DataSourceID="odsBanks" Width="75px" AppendDataBoundItems="true">
                                            <asp:ListItem Text="" Value="None"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:ObjectDataSource ID="odsBanks" runat="server" DataObjectTypeName="Broker.DataAccess.Bank"
                                            TypeName="Broker.DataAccess.Bank" SelectMethod="Select"></asp:ObjectDataSource>
                                    </EditItemTemplate>
                                    <InsertItemTemplate>
                                        <asp:TextBox ID="tbBankslipNumber" runat="server" Text='<%#Bind("BankslipNumber") %>'
                                            Width="75px"></asp:TextBox>
                                        <asp:DropDownList ID="ddlBanks" runat="server" DataTextField="Name" DataValueField="ID"
                                            DataSourceID="odsBanks" Width="75px" AppendDataBoundItems="true">
                                            <asp:ListItem Selected="True" Text="" Value="None"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:ObjectDataSource ID="odsBanks" runat="server" DataObjectTypeName="Broker.DataAccess.Bank"
                                            TypeName="Broker.DataAccess.Bank" SelectMethod="Select"></asp:ObjectDataSource>
                                    </InsertItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Дата">
                                    <ItemTemplate>
                                        <asp:Label ID="tbDate" runat="server" ReadOnly="true" Text='<%#Bind("Date", "{0:d}") %>'
                                            Width="70px"></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="tbDate" runat="server" Text='<%#Bind("Date", "{0:d}") %>' Width="70px"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvDate" runat="server" Display="Dynamic" ErrorMessage="*"
                                            ControlToValidate="tbDate"></asp:RequiredFieldValidator>
                                        <asp:CompareValidator ID="cvDate" runat="server" Display="Dynamic" ErrorMessage="*"
                                            ControlToValidate="tbDate" Operator="DataTypeCheck" Type="Date"></asp:CompareValidator>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Износ" ItemStyle-CssClass="currencyClass">
                                    <ItemTemplate>
                                        <asp:Label ID="tbValue" runat="server" Text='<%#Bind("Value", "{0:#,0.00}") %>' ReadOnly="true"
                                            Width="80px"></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="tbValue" runat="server" Text='<%#Bind("Value") %>' Width="80px"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvValue" runat="server" ErrorMessage="*" ControlToValidate="tbValue"
                                            Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:CompareValidator ID="cvValue" runat="server" ErrorMessage="*" ControlToValidate="tbValue"
                                            Display="Dynamic" Operator="DataTypeCheck" Type="Double"></asp:CompareValidator>
                                        <super:EntityCallOutValidator ID="ValueValidator" PropertyName="PaymentValue" runat="server" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:CommandField CancelText="Откажи" DeleteText="Избриши" EditText="Измени" InsertText="Внеси"
                                    InsertVisible="false" NewText="Нов" SelectText="Одбери" ShowEditButton="True"
                                    ShowDeleteButton="true" ShowInsertButton="false" UpdateText="Ажурирај" />
                            </Columns>
                            <PagerStyle HorizontalAlign="Center" />
                            <%--<SelectedRowStyle CssClass="rowSelected" />--%>
                            <PagerSettings FirstPageText="<< Прва " PreviousPageText="< Претходна " NextPageText=" Следна >"
                                LastPageText=" Последна >>" Mode="NextPreviousFirstLast" />
                        </asp:GridView>
                        <asp:ObjectDataSource ID="odsPayments" runat="server" UpdateMethod="UpdateExtend"
                            DeleteMethod="DeleteExtend" SelectMethod="GetByPolicyItemID" DataObjectTypeName="Broker.DataAccess.Payment"
                            TypeName="Broker.DataAccess.Payment" OnSelecting="odsPayments_Selecting" OnUpdating="odsPayments_Updating"
                            OldValuesParameterFormatString="oldEntity" ConflictDetection="CompareAllValues"
                            OnDeleting="odsPayments_Deleting" OnInserting="odsPayments_Inserting">
                            <UpdateParameters>
                                <asp:Parameter Name="oldEntity" Type="Object" />
                                <asp:Parameter Name="newEntity" Type="Object" />
                                <asp:Parameter Name="Value" Type="Decimal" />
                                <asp:Parameter Name="Date" Type="DateTime" />
                            </UpdateParameters>
                            <InsertParameters>
                                <asp:Parameter Name="Value" Type="Decimal" />
                                <asp:Parameter Name="Date" Type="DateTime" />
                            </InsertParameters>
                            <DeleteParameters>
                                <asp:Parameter Name="entityToDelete" Type="Object" />
                                <asp:Parameter Name="Value" Type="Decimal" />
                                <asp:Parameter Name="Date" Type="DateTime" />
                            </DeleteParameters>
                        </asp:ObjectDataSource>
                    </div>
                </div>
            </div>
            <table>
                <tr>
                    <td>
                        Премија за наплата
                    </td>
                    <td>
                        <asp:TextBox ID="tbPolicyPremiumCost" Width="100px" runat="server" ReadOnly="true"
                            CssClass="currencyClass"></asp:TextBox>
                    </td>
                    <td>
                        Платено
                    </td>
                    <td>
                        <asp:TextBox ID="tbPolicyTotalPaidValue" Width="100px" runat="server" ReadOnly="true"
                            CssClass="currencyClass"></asp:TextBox>
                    </td>
                    <td>
                        За плаќање
                    </td>
                    <td>
                        <asp:TextBox ID="tbPolicyForPaidValue" Width="100px" runat="server" ReadOnly="true"
                            CssClass="currencyClass"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Дата на уплата:
                    </td>
                    <td>
                        <asp:TextBox ID="tbDateOfPayment" Width="100px" runat="server"></asp:TextBox>
                        <asp:CompareValidator ID="cvDateOfPayment" runat="server" ControlToValidate="tbDateOfPayment"
                            ErrorMessage="*" Operator="DataTypeCheck" Type="Date" Display="Dynamic"></asp:CompareValidator>
                        <asp:RequiredFieldValidator ID="rfvDateOfPayment" runat="server" ControlToValidate="tbDateOfPayment"
                            Display="Dynamic" ErrorMessage="*"></asp:RequiredFieldValidator>
                    </td>
                    <td>
                        <b>Износ</b>
                    </td>
                    <td>
                        <asp:TextBox ID="tbValueOfPayment" Width="100px" runat="server" Font-Bold="true"></asp:TextBox>
                        <asp:CompareValidator ID="cvPaymentValue" runat="server" ControlToValidate="tbValueOfPayment"
                            ErrorMessage="*" Operator="DataTypeCheck" Type="Double" Display="Dynamic"></asp:CompareValidator>
                        <asp:RequiredFieldValidator ID="rfvValueOfPayment" runat="server" ControlToValidate="tbValueOfPayment"
                            Display="Dynamic" ErrorMessage="*"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        Тип на уплата
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlPaymentTypes" runat="server" DataTextField="Name" DataValueField="ID"
                            Width="100px" DataSourceID="odsPaymentTypes" AutoPostBack="true" OnSelectedIndexChanged="ddlPaymentTypes_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:ObjectDataSource ID="odsPaymentTypes" runat="server" DataObjectTypeName="Broker.DataAccess.PaymentType"
                            TypeName="Broker.DataAccess.PaymentType" SelectMethod="Select"></asp:ObjectDataSource>
                    </td>
                    <td>
                        Банка
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlBank" runat="server" DataSourceID="odsBank" AutoPostBack="True"
                            DataTextField="Name" DataValueField="ID" Width="155px" CssClass="select" Enabled="false">
                        </asp:DropDownList>
                        <asp:ObjectDataSource ID="odsBank" runat="server" SelectMethod="GetBanksWithGreditCards"
                            TypeName="Broker.DataAccess.Bank" OnSelecting="odsBank_Selecting"></asp:ObjectDataSource>
                    </td>
                    <td>
                        Тип на картица
                    </td>
                    <td>
                        <asp:DropDownList CssClass="select normal" ID="ddlCardTypes" runat="server" DataSourceID="odsCardTypes"
                            DataTextField="Name" DataValueField="ID" Width="100px" Enabled="false">
                        </asp:DropDownList>
                        <asp:ObjectDataSource ID="odsCardTypes" runat="server" SelectMethod="GetByBank" TypeName="Broker.DataAccess.CreditCard">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="ddlBank" Name="BankID" PropertyName="SelectedValue"
                                    Type="Int32" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                    </td>
                </tr>
                <tr>
                    <td>
                        Број на извод
                    </td>
                    <td>
                        <asp:TextBox ID="tbBankslipNumber" runat="server" Width="100px"></asp:TextBox>
                    </td>
                    <td>
                        Банка на извод
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlBankslipBanks" runat="server" DataSourceID="odsBankslipsBank"
                            AutoPostBack="True" DataTextField="Name" DataValueField="ID" Width="155px" CssClass="select">
                        </asp:DropDownList>
                        <asp:ObjectDataSource ID="odsBankslipsBank" runat="server" SelectMethod="Select"
                            TypeName="Broker.DataAccess.Bank"></asp:ObjectDataSource>
                    </td>
                    <td>
                        <asp:Button ID="btnInsert" runat="server" Text="Нов запис" OnClick="btnInsert_Click" />
                    </td>
                    <td>
                        <asp:Button ID="btnBack" runat="server" CausesValidation="false" Text="Назад" OnClick="btnBack_Click" />
                    </td>
                </tr>
            </table>
        </asp:View>
    </asp:MultiView>
</asp:Content>
