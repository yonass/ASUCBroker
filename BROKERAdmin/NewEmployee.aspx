<%@ Page Title="Евиденција на провизии за маркетинг агенти" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="NewEmployee.aspx.cs" Inherits="BROKERAdmin_NewEmployee" %>

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
    </div>
    <asp:MultiView ID="mvMain" runat="server">
        <asp:View ID="viewGrid" runat="server">
            <div id="tabeliFrame">
                <div id="header">
                    <div id="content">
                        Провизии
                    </div>
                </div>
                <div id="contentOuter">
                    <div id="contentInner">
                        <cc1:GXGridView ID="GXGridView1" runat="server" DataSourceID="odsGridView" DataKeyNames="ID"
                            Width="100%" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"
                            Caption="Провизии" EmptyDataText="Нема записи кои го задоволуваат критериумот на пребарување!"
                            OnRowCommand="GXGridView1_RowCommand" RowStyle-CssClass="row" CssClass="grid"
                            GridLines="None">
                            <Columns>
                                <asp:BoundField HeaderText="Име и презиме" DataField="Name" SortExpression="Name" />
                                <asp:BoundField HeaderText="Класа на осиг." DataField="InsuranceTypeName" SortExpression="InsuranceTypeName" />
                                <asp:BoundField HeaderText="Подкласа на осиг." DataField="Description" SortExpression="Description" />
                                <asp:BoundField HeaderText="Компанија" DataField="InsuranceCompanyName" SortExpression="InsuranceCompanyName" />
                                <asp:BoundField HeaderText="% за физички лица" DataField="PercentageForPrivates"
                                    SortExpression="PercentageForPrivates" />
                                <asp:BoundField HeaderText="% за правни лица" DataField="PercentageForLaws" SortExpression="PercentageForLaws" />
                            </Columns>
                            <PagerStyle HorizontalAlign="Center" />
                            <SelectedRowStyle CssClass="rowSelected" />
                            <PagerSettings FirstPageText="<< Прва " PreviousPageText="< Претходна " NextPageText=" Следна >"
                                LastPageText=" Последна >>" Mode="NextPreviousFirstLast" />
                        </cc1:GXGridView>
                        <cc1:GridViewDataSource ID="odsGridView" runat="server" TypeName="Broker.DataAccess.ViewBrokeragesForMarketingAgent"
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
                                        <cc1:FilterItem FieldName="Име и презиме" PropertyName="Name" Comparator="StringComparators"
                                            DataType="String" />
                                        <cc1:FilterItem FieldName="Класа на осиг." PropertyName="InsuranceTypeName" Comparator="StringComparators"
                                            DataType="String" />
                                        <cc1:FilterItem FieldName="Подкласа на осиг." PropertyName="Description" Comparator="StringComparators"
                                            DataType="String" />
                                        <cc1:FilterItem FieldName="Компанија" PropertyName="InsuranceCompanyName" Comparator="StringComparators"
                                            DataType="String" />
                                        <cc1:FilterItem FieldName="% за физички лица" PropertyName="PercentageForPrivates"
                                            Comparator="NumericComparators" DataType="Decimal" />
                                        <cc1:FilterItem FieldName="% за правни лица" PropertyName="PercentageForLaws" Comparator="NumericComparators"
                                            DataType="Decimal" />
                                    </cc1:FilterControl>
                                </div>
                                <cc1:FilterDataSource ID="odsFilterGridView" runat="server" TypeName="Broker.DataAccess.ViewBrokeragesForMarketingAgent">
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
                    <cc1:SearchItem FieldName="Име и презиме" PropertyName="Name" Comparator="StringComparators"
                        DataType="String" />
                    <cc1:SearchItem FieldName="Класа на осиг." PropertyName="InsuranceTypeName" Comparator="StringComparators"
                        DataType="String" />
                    <cc1:SearchItem FieldName="Подкласа на осиг." PropertyName="Description" Comparator="StringComparators"
                        DataType="String" />
                    <cc1:SearchItem FieldName="Компанија" PropertyName="InsuranceCompanyName" Comparator="StringComparators"
                        DataType="String" />
                    <cc1:SearchItem FieldName="% за физички лица" PropertyName="PercentageForPrivates"
                        Comparator="NumericComparators" DataType="Decimal" />
                    <cc1:SearchItem FieldName="% за правни лица" PropertyName="PercentageForLaws" Comparator="NumericComparators"
                        DataType="Decimal" />
                </cc1:SearchControl>
                <cc1:GridViewDataSource ID="odsSearch" runat="server" TypeName="Broker.DataAccess.ViewBrokeragesForMarketingAgent"
                    SelectCountMethod="SelectSearchCountCached" SelectMethod="SelectSearch">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="SearchControl1" PropertyName="SearchArguments" Name="sArgument" />
                    </SelectParameters>
                </cc1:GridViewDataSource>
            </div>
        </asp:View>
        <asp:View ID="viewReport" runat="server">
            <div class="paddingKontroli">
                <cc1:ReportControl ID="reportControl" runat="server" TypeName="Broker.DataAccess.ViewBrokeragesForMarketingAgent"
                    GridViewID="GXGridView1" SearchControlID="SearchControl1">
                    <cc1:PrintItem HeaderText="Име и презиме" PropertyName="Name" />
                    <cc1:PrintItem HeaderText="Класа на осиг." PropertyName="InsuranceTypeName" />
                    <cc1:PrintItem HeaderText="Подкласа на осиг." PropertyName="Description" />
                    <cc1:PrintItem HeaderText="Компанија" PropertyName="InsuranceCompanyName" />
                    <cc1:PrintItem HeaderText="% за физички лица" PropertyName="PercentageForPrivates" />
                    <cc1:PrintItem HeaderText="% за правни лица" PropertyName="PercentageForLaws" />
                </cc1:ReportControl>
            </div>
        </asp:View>
        <asp:View ID="ViewInsertNewEmployee" runat="server">
            <table>
                <tr>
                    <td align="right">
                        Име и Презиме:
                    </td>
                    <td>
                        <asp:TextBox ID="FullNameTextBox" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="FullNameTextBox"
                            ErrorMessage="*"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        ЕМБГ:
                    </td>
                    <td>
                        <asp:TextBox ID="EMBGTextBox" runat="server"> </asp:TextBox>
                        <%-- <super:entitycalloutvalidator id="EMBGValidator" propertyname="EMBG_Exist" runat="server" />--%>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="EMBGTextBox"
                            Display="Dynamic" ErrorMessage="*"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        Адреса:
                    </td>
                    <td>
                        <asp:TextBox ID="AddressTextBox" runat="server"> </asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        Телефонски Број:
                    </td>
                    <td>
                        <asp:TextBox ID="PhoneNumberTextBox" runat="server"> </asp:TextBox>
                    </td>
                </tr>
               <%-- <tr>
                    <td>
                    </td>
                    <td>
                        <asp:RadioButtonList ID="RolesList" runat="server" DataSourceID="RolesDataSource"
                            DataTextField="Name" DataValueField="ID">
                        </asp:RadioButtonList>
                        <asp:ObjectDataSource ID="RolesDataSource" runat="server" SelectMethod="GetAllVisibleRoles"
                            TypeName="Broker.Controllers.EmployeeManagement.EmployRoleController"></asp:ObjectDataSource>
                    </td>
                </tr>--%>
                <tr>
                    <td align="right">
                        Филијала:
                    </td>
                    <td>
                        <asp:DropDownList ID="BranchesList" runat="server" DataSourceID="BranchesDataSource"
                            DataTextField="Name" DataValueField="ID">
                        </asp:DropDownList>
                        <asp:ObjectDataSource ID="BranchesDataSource" runat="server" SelectMethod="GetActiveBranches"
                            TypeName="Broker.DataAccess.Branch"></asp:ObjectDataSource>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="btnNext" runat="server" OnClick="btnNext_Click" Text="Наредно" />
                    </td>
                    <td>
                    </td>
                </tr>
            </table>
        </asp:View>
        <asp:View ID="ViewInsertBrokerages" runat="server">
            <table>
                <tr>
                    <td>
                        <asp:Label ID="lblName" runat="server" Text="Маркетинг Агент"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="tbName" ReadOnly="true" runat="server"> </asp:TextBox>
                        <asp:TextBox ID="tbUserID" Visible="false" runat="server"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <asp:MultiView ID="mvBrokerages" runat="server">
                <asp:View ID="viewItemsGrid" runat="server">
                    <asp:GridView ID="GridViewItems" runat="server" DataSourceID="odsGridViewItems" DataKeyNames="ID"
                        Width="100%" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"
                        Caption="Провизии за м. агент" EmptyDataText="Нема записи кои го задоволуваат критериумот на пребарување!"
                        RowStyle-CssClass="row" CssClass="grid" GridLines="None" OnRowUpdating="GridViewItems_RowUpdating"
                        OnRowDeleted="GridViewItems_RowDeleted" OnRowUpdated="GridViewItems_RowUpdated">
                        <RowStyle CssClass="row"></RowStyle>
                        <Columns>
                            <asp:TemplateField HeaderStyle-Width="0px">
                                <EditItemTemplate>
                                    <asp:Label Width="0px" ID="tbIsActive" runat="server" Text='<%#Bind("IsActive") %>'
                                        Visible="false"></asp:Label>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="tbIsActive" Width="0px" runat="server" Text='<%#Bind("IsActive") %>'
                                        Visible="false"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-Width="0px">
                                <EditItemTemplate>
                                    <asp:Label Width="0px" ID="tbUserID" runat="server" Text='<%#Bind("UserID") %>' Visible="false"></asp:Label>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="tbUserID" Width="0px" runat="server" Text='<%#Bind("UserID") %>' Visible="false"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Осигурителна компанија">
                                <EditItemTemplate>
                                    <asp:Label ID="tbInsuranceCompany" runat="server" Text='<%#Eval("InsuranceCompany.Name") %>'
                                        Width="100px" ReadOnly="true"></asp:Label>
                                    <asp:Label ID="tbInsuranceCompanyID" runat="server" Text='<%#Bind("InsuranceCompanyID") %>'
                                        Visible="false"></asp:Label>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="tbInsuranceCompany" runat="server" Text='<%#Eval("InsuranceCompany.Name") %>'
                                        Width="100px" ReadOnly="true"></asp:Label>
                                    <asp:Label ID="tbInsuranceCompanyID" runat="server" Text='<%#Bind("InsuranceCompanyID") %>'
                                        Visible="false"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Класа">
                                <EditItemTemplate>
                                    <asp:Label ID="tbInsuranceType" runat="server" Text='<%#Eval("InsuranceSubType.InsuranceType.Name") %>'
                                        ReadOnly="true" Width="100px"></asp:Label>
                                    <asp:Label ID="tbInsuranceTypeID" runat="server" Text='<%#Bind("InsuranceSubTypeID") %>'
                                        ReadOnly="true" Visible="false"></asp:Label>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="tbInsuranceType" runat="server" Text='<%#Eval("InsuranceSubType.InsuranceType.Name") %>'
                                        ReadOnly="true" Width="100px"></asp:Label>
                                    <asp:Label ID="tbInsuranceTypeID" runat="server" Text='<%#Bind("InsuranceSubTypeID") %>'
                                        ReadOnly="true" Visible="false"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Подкласа">
                                <EditItemTemplate>
                                    <asp:Label ID="tbInsuranceSubType" runat="server" Text='<%#Eval("InsuranceSubType.Description") %>'
                                        ReadOnly="true" Width="100px"></asp:Label>
                                    <asp:Label ID="tbInsuranceSubTypeID" runat="server" Text='<%#Bind("InsuranceSubTypeID") %>'
                                        Visible="false"></asp:Label>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="tbInsuranceSubType" runat="server" Text='<%#Eval("InsuranceSubType.Description") %>'
                                        ReadOnly="true" Width="100px"></asp:Label>
                                    <asp:Label ID="tbInsuranceSubTypeID" runat="server" Text='<%#Bind("InsuranceSubTypeID") %>'
                                        Visible="false"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="% за физички лица">
                                <EditItemTemplate>
                                    <asp:TextBox ID="tbPercentageForPrivates" runat="server" Text='<%#Bind("PercentageForPrivates") %>'
                                        Width="50px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvPercentageForPrivates" runat="server" ControlToValidate="tbPercentageForPrivates"
                                        Display="Dynamic" ErrorMessage="*"></asp:RequiredFieldValidator>
                                    <asp:CompareValidator ID="cvPercentageForPrivates" runat="server" ControlToValidate="tbPercentageForPrivates"
                                        Display="Dynamic" ErrorMessage="*" Operator="DataTypeCheck" Type="Double"></asp:CompareValidator>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="tbPercentageForPrivates" runat="server" Text='<%#Bind("PercentageForPrivates") %>'
                                        ReadOnly="true" Width="50px"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="% за правни лица">
                                <EditItemTemplate>
                                    <asp:TextBox ID="tbPercentageForLaws" runat="server" Text='<%#Bind("PercentageForLaws") %>'
                                        Width="50px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvPercentageForLaws" runat="server" ControlToValidate="tbPercentageForLaws"
                                        Display="Dynamic" ErrorMessage="*"></asp:RequiredFieldValidator>
                                    <asp:CompareValidator ID="cvPercentageForLaws" runat="server" ControlToValidate="tbPercentageForLaws"
                                        Display="Dynamic" ErrorMessage="*" Operator="DataTypeCheck" Type="Double"></asp:CompareValidator>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="tbPercentageForLaws" runat="server" Text='<%#Bind("PercentageForLaws") %>'
                                        ReadOnly="true" Width="50px"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:CommandField CancelText="Откажи" DeleteText="Избриши" EditText="Измени" InsertText="Внеси"
                                InsertVisible="False" NewText="Нов" SelectText="Одбери" ShowDeleteButton="True"
                                ShowEditButton="True" UpdateText="Ажурирај" />
                        </Columns>
                        <PagerStyle HorizontalAlign="Center" />
                        <PagerSettings FirstPageText="<< Прва " PreviousPageText="< Претходна " NextPageText=" Следна >"
                            LastPageText=" Последна >>" Mode="NextPreviousFirstLast" />
                    </asp:GridView>
                    <asp:ObjectDataSource ID="odsGridViewItems" runat="server" TypeName="Broker.DataAccess.BrokeragesForMarketingAgent"
                        OldValuesParameterFormatString="oldEntity" DataObjectTypeName="Broker.DataAccess.BrokeragesForMarketingAgent"
                        ConflictDetection="CompareAllValues" DeleteMethod="Delete" InsertMethod="Insert"
                        SelectMethod="GetByUserID" UpdateMethod="UpdateCurrentPercentages" OnInserting="odsGridView_Inserting">
                        <UpdateParameters>
                            <asp:Parameter Name="oldEntity" Type="Object" />
                            <asp:Parameter Name="newEntity" Type="Object" />
                            <asp:Parameter Name="PercentageForPrivates" Type="Decimal" />
                            <asp:Parameter Name="PercentageForLaws" Type="Decimal" />
                            <asp:Parameter Name="InsuranceCompanyID" Type="Int32" />
                            <asp:Parameter Name="InsuranceSubTypeID" Type="Int32" />
                            <asp:Parameter Name="InsuranceTypeID" Type="Int32" />
                        </UpdateParameters>
                        <DeleteParameters>
                            <asp:Parameter Name="PercentageForPrivates" Type="Decimal" />
                            <asp:Parameter Name="PercentageForLaws" Type="Decimal" />
                            <asp:Parameter Name="InsuranceCompanyID" Type="Int32" />
                            <asp:Parameter Name="InsuranceSubTypeID" Type="Int32" />
                            <asp:Parameter Name="InsuranceTypeID" Type="Int32" />
                            <asp:Parameter Name="IsActive" Type="Boolean" />
                        </DeleteParameters>
                    </asp:ObjectDataSource>
                    <div>
                        <asp:Button ID="btnNewItem" runat="server" Text="Нова брокеража" OnClick="btnNewItem_Click" />
                    </div>
                </asp:View>
                <asp:View ID="viewItemsEdit" runat="server">
                    <asp:DetailsView ID="DetailsViewItems" runat="server" DataSourceID="DetailsViewDataSourceItems"
                        DataKeyNames="ID" AutoGenerateRows="False" OnItemCommand="DetailsViewItems_ItemCommand"
                        OnItemInserted="DetailsViewItems_ItemInserted" OnModeChanging="DetailsViewItems_ModeChanging"
                        OnItemInserting="DetailsViewItems_ItemInserting" GridLines="None" DefaultMode="Insert">
                        <Fields>
                            <asp:TemplateField>
                                <InsertItemTemplate>
                                    <div class="subTitles">
                                        Нова провизија
                                    </div>
                                </InsertItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <InsertItemTemplate>
                                    <asp:TextBox ID="tbItemID" runat="server" Text='<%#Bind("ID") %>' Visible="false"></asp:TextBox>
                                </InsertItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <InsertItemTemplate>
                                    <table>
                                        <%-- <tr>
                                            <td style="width: 130px;">
                                                <asp:Label ID="lblName" runat="server" Text="Име и презиме"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="tbName" runat="server" ReadOnly="true"></asp:TextBox>
                                            </td>
                                        </tr>--%>
                                        <tr>
                                            <td style="width: 130px;">
                                                <asp:Label ID="lblCompanyName" runat="server" Text="Компанија"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlCompanies" runat="server" AutoPostBack="True" DataSourceID="odsCompanies"
                                                    DataTextField="Name" DataValueField="ID" SelectedValue='<%# Bind("InsuranceCompanyID") %>'
                                                    OnSelectedIndexChanged="ddlCompanies_SelectedIndexChanged" Width="600px">
                                                </asp:DropDownList>
                                                <asp:ObjectDataSource ID="odsCompanies" runat="server" SelectMethod="GetActiveInsuranceCompanies"
                                                    TypeName="Broker.DataAccess.InsuranceCompany"></asp:ObjectDataSource>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 130px;">
                                                <asp:Label ID="lblInsuranceType" runat="server" Text="Класа на осигуруавање"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlInsuranceType" runat="server" AutoPostBack="True" DataSourceID="odsInsuranceTypes"
                                                    DataTextField="Name" width="600px" DataValueField="ID" OnSelectedIndexChanged="ddlInsuranceType_SelectedIndexChanged">
                                                </asp:DropDownList>
                                                <asp:ObjectDataSource ID="odsInsuranceTypes" runat="server" SelectMethod="GetByCompany"
                                                    TypeName="Broker.DataAccess.InsuranceType">
                                                    <SelectParameters>
                                                        <asp:ControlParameter ControlID="ddlCompanies" Name="companyID" PropertyName="SelectedValue"
                                                            Type="Int32" />
                                                    </SelectParameters>
                                                </asp:ObjectDataSource>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 130px;">
                                                <asp:Label ID="lblInsuranceSubType" runat="server" Text="Подкласа на осигуруавање"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlInsuranceSubType" runat="server" AutoPostBack="True" DataSourceID="odsInsuranceSubType"
                                                    DataTextField="Description" width="600px" DataValueField="ID" SelectedValue='<%# Bind("InsuranceSubTypeID") %>'
                                                    OnSelectedIndexChanged="ddlInsuranceSubType_SelectedIndexChanged">
                                                </asp:DropDownList>
                                                <asp:ObjectDataSource ID="odsInsuranceSubType" runat="server" SelectMethod="GetByInsuranceType"
                                                    TypeName="Broker.DataAccess.InsuranceSubType">
                                                    <SelectParameters>
                                                        <asp:ControlParameter ControlID="ddlInsuranceType" Name="insuranceTypeID" PropertyName="SelectedValue"
                                                            Type="Int32" />
                                                    </SelectParameters>
                                                </asp:ObjectDataSource>
                                            </td>
                                        </tr>
                                    </table>
                                </InsertItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <EditItemTemplate>
                                    <table>
                                        <tr>
                                            <td style="width: 130px;">
                                                <asp:Label ID="lblPercentageForPrivates" Text="% провизија за физички лица" runat="server"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="tbPercentageForPrivates" runat="server" Text='<%#Bind("PercentageForPrivates") %>'></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvPercentageForPrivates" runat="server" ControlToValidate="tbPercentageForPrivates"
                                                    Display="Dynamic" ErrorMessage="*"></asp:RequiredFieldValidator>
                                                <asp:CompareValidator ID="cvPercentageForPrivates" runat="server" ControlToValidate="tbPercentageForPrivates"
                                                    Display="Dynamic" ErrorMessage="*" Operator="DataTypeCheck" Type="Double"></asp:CompareValidator>
                                            </td>
                                        </tr>
                                    </table>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:TextBox ID="tbPercentageForPrivates" runat="server" Text='<%#Bind("PercentageForPrivates") %>'
                                        ReadOnly="true"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <EditItemTemplate>
                                    <table>
                                        <tr>
                                            <td style="width: 130px;">
                                                <asp:Label ID="lblPercentageForLaws" Text="% провизија за правни лица" runat="server"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="tbPercentageForLaws" runat="server" Text='<%#Bind("PercentageForLaws") %>'></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvPercentageForLaws" runat="server" ControlToValidate="tbPercentageForLaws"
                                                    Display="Dynamic" ErrorMessage="*"></asp:RequiredFieldValidator>
                                                <asp:CompareValidator ID="cvPercentageForLaws" runat="server" ControlToValidate="tbPercentageForLaws"
                                                    Display="Dynamic" ErrorMessage="*" Operator="DataTypeCheck" Type="Double"></asp:CompareValidator>
                                            </td>
                                        </tr>
                                    </table>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:TextBox ID="tbPercentageForLaws" runat="server" Text='<%#Bind("PercentageForLaws") %>'
                                        ReadOnly="true"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <InsertItemTemplate>
                                    <asp:Button ID="btnCancel" runat="server" CommandName="Cancel" Text="Откажи" CausesValidation="false" />
                                    <asp:Button ID="btnInsert" runat="server" CommandName="Insert" Text="Внеси" />
                                </InsertItemTemplate>
                            </asp:TemplateField>
                        </Fields>
                    </asp:DetailsView>
                    <cc1:DetailsViewDataSource ID="DetailsViewDataSourceItems" runat="server" TypeName="Broker.DataAccess.BrokeragesForMarketingAgent"
                        ConflictDetection="CompareAllValues" DataObjectTypeName="Broker.DataAccess.BrokeragesForMarketingAgent"
                        InsertMethod="Insert" SelectMethod="Get" OnInserted="DetailsViewDataSourceItems_Inserted"
                        OnInserting="DetailsViewDataSourceItems_Inserting">
                        <InsertParameters>
                            <asp:Parameter Name="entityToInsert" Type="Object" />
                        </InsertParameters>
                        <UpdateParameters>
                            <asp:Parameter Name="PercentageForPrivates" Type="Decimal" />
                            <asp:Parameter Name="PercentageForLaws" Type="Decimal" />
                        </UpdateParameters>
                        <SelectParameters>
                            <asp:ControlParameter ControlID="GridViewItems" Name="id" PropertyName="SelectedValue"
                                Type="Int32" />
                        </SelectParameters>
                    </cc1:DetailsViewDataSource>
                </asp:View>
            </asp:MultiView>
        </asp:View>
    </asp:MultiView>
</asp:Content>
