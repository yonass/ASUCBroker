<%@ Page Title="Излезни фактури (од филијала)" Language="C#" MasterPageFile="~/MasterPage.master"
    AutoEventWireup="true" CodeFile="FacturesPerUser.aspx.cs" Inherits="Broker_FacturesPerBranch" %>

<%@ Register Namespace="Superexpert.Controls" TagPrefix="super" %>
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
            <asp:Button ID="btnFactureItems" runat="server" ToolTip="Приказ" OnClick="btnFactureItems_Click"
                CssClass="prikaz" BorderWidth="0px" Enabled="false" />
        </div>
        <div id="button05">
            <asp:Button ID="btnAttachments" runat="server" ToolTip="Документи" OnClick="btnAttachments_Click"
                CssClass="dokumenti" Enabled="false" BorderWidth="0px" />
        </div>
        <div id="button06">
            <asp:Button ID="btnPintFacture" runat="server" ToolTip="Печати фактура" OnClick="btnPintFacture_Click"
                CssClass="pecati" BorderWidth="0px" />
        </div>
        <div id="button07">
            <asp:Button ID="btnPrintAnex" runat="server" ToolTip="Печати анекс договор" OnClick="btnPrintAnexDeal_Click"
                CssClass="dogovor" BorderWidth="0px" />
        </div>
        <div id="button08">
            <asp:Button ID="btnDiscardFacture" runat="server" ToolTip="Сторнирај фактура" OnClick="btnDiscardFacture_Click"
                CssClass="storniraj" BorderWidth="0px" Enabled="false" />
        </div>
        <div id="button09">
            <asp:Button ID="btnChangeStatus" runat="server" ToolTip="Измени статус на фактура"
                OnClick="btnChangeStatus_Click" CssClass="promeniStatus" BorderWidth="0px" Enabled="false" />
        </div>
    </div>
    <asp:MultiView ID="mvMain" runat="server">
        <asp:View ID="viewGrid" runat="server">
            <div id="tabeliFrame">
                <div id="header">
                    <div id="content">
                        Фактури
                    </div>
                </div>
                <div id="contentOuter">
                    <div id="contentInner">
                        <cc1:GXGridView ID="GXGridView1" runat="server" DataSourceID="odsGridView" DataKeyNames="ID"
                            Width="100%" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"
                            Caption="Фактури" EmptyDataText="Нема записи кои го задоволуваат критериумот на пребарување!"
                            OnRowCommand="GXGridView1_RowCommand" RowStyle-CssClass="row" CssClass="grid"
                            GridLines="None">
                            <Columns>
                                <asp:BoundField HeaderText="Број на фактура" DataField="FactureNumber" SortExpression="FactureNumber" />
                                <asp:BoundField HeaderText="Тип на фактура" DataField="DocumentSubTypeDescription"
                                    SortExpression="DocumentSubTypeDescription" />
                                <asp:BoundField HeaderText="Статус" DataField="StatusDescription" SortExpression="StatusDescription" />
                                <asp:BoundField HeaderText="Дата на креирање" DataField="DateOfCreation" SortExpression="DateOfCreation"
                                    DataFormatString="{0:d}" />
                                <asp:BoundField HeaderText="Износ" DataField="TotalCost" SortExpression="TotalCost"
                                    DataFormatString="{0:#,0.00}" ItemStyle-CssClass="currencyClass" />
                                <asp:BoundField HeaderText="Брокер.сума" DataField="BrokerageValue" SortExpression="BrokerageValue"
                                    DataFormatString="{0:#,0.00}" ItemStyle-CssClass="currencyClass" />
                                <asp:BoundField HeaderText="Уплаќач" DataField="ClientName" SortExpression="ClientName" />
                            </Columns>
                            <PagerStyle HorizontalAlign="Center" />
                            <SelectedRowStyle CssClass="rowSelected" />
                            <PagerSettings FirstPageText="<< Прва " PreviousPageText="< Претходна " NextPageText=" Следна >"
                                LastPageText=" Последна >>" Mode="NextPreviousFirstLast" />
                        </cc1:GXGridView>
                        <cc1:GridViewDataSource ID="odsGridView" runat="server" TypeName="Broker.DataAccess.ViewFacture"
                            OldValuesParameterFormatString="oldEntity" SelectMethod="SelectByFK" SelectCountMethod="SelectByFKCountCached"
                            OnSelecting="odsGridView_Selecting">
                        </cc1:GridViewDataSource>
                        <div class="pager">
                            <div class="container">
                                <div style="float: left;">
                                    <cc1:PagerControl ID="myGridPager" runat="server" ControlToPage="GXGridView1" />
                                </div>
                                <div style="float: right;">
                                    <cc1:FilterControl ID="FilterControl1" runat="server" GridViewToFilter="GXGridView1"
                                        FilterDataSourceID="odsFilterGridView" OnFilter="FilterControl1_Filter">
                                        <cc1:FilterItem FieldName="Број на фактура" PropertyName="FactureNumber" Comparator="StringComparators"
                                            DataType="String" />
                                        <cc1:FilterItem FieldName="Тип на фактура" PropertyName="DocumentSubTypeDescription"
                                            Comparator="StringComparators" DataType="String" />
                                        <cc1:FilterItem FieldName="Статус" PropertyName="StatusDescription" Comparator="StringComparators"
                                            DataType="String" />
                                        <cc1:FilterItem FieldName="Дата на креирање" PropertyName="DateOfCreation" Comparator="NumericComparators"
                                            DataType="DateTime" />
                                        <cc1:FilterItem FieldName="Износ" PropertyName="TotalCost" Comparator="NumericComparators"
                                            DataType="Decimal" />
                                        <cc1:FilterItem FieldName="Уплаќач" PropertyName="ClientName" Comparator="StringComparators"
                                            DataType="String" />
                                        <cc1:FilterItem FieldName="Од датум" PropertyName="FromDate" Comparator="NumericComparators"
                                            DataType="DateTime" />
                                        <cc1:FilterItem FieldName="До датум" PropertyName="ToDate" Comparator="NumericComparators"
                                            DataType="DateTime" />
                                    </cc1:FilterControl>
                                </div>
                                <cc1:FilterDataSource ID="odsFilterGridView" runat="server" TypeName="Broker.DataAccess.ViewFacture"
                                    SelectMethod="SelectFilterByFK" SelectCountMethod="SelectFilterByFKCountCached"
                                    OnSelecting="odsFilterGridView_Selecting">
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
                    <cc1:SearchItem FieldName="Број на фактура" PropertyName="FactureNumber" Comparator="StringComparators"
                        DataType="String" />
                    <cc1:SearchItem FieldName="Тип на фактура" PropertyName="DocumentSubTypeDescription"
                        Comparator="StringComparators" DataType="String" />
                    <cc1:SearchItem FieldName="Статус" PropertyName="StatusDescription" Comparator="StringComparators"
                        DataType="String" />
                    <cc1:SearchItem FieldName="Дата на креирање" PropertyName="DateOfCreation" Comparator="NumericComparators"
                        DataType="DateTime" />
                    <cc1:SearchItem FieldName="Износ" PropertyName="TotalCost" Comparator="NumericComparators"
                        DataType="Decimal" />
                    <cc1:SearchItem FieldName="Уплаќач" PropertyName="ClientName" Comparator="StringComparators"
                        DataType="String" />
                    <cc1:SearchItem FieldName="Од датум" PropertyName="FromDate" Comparator="NumericComparators"
                        DataType="DateTime" />
                    <cc1:SearchItem FieldName="До датум" PropertyName="ToDate" Comparator="NumericComparators"
                        DataType="DateTime" />
                </cc1:SearchControl>
                <cc1:GridViewDataSource ID="odsSearch" runat="server" TypeName="Broker.DataAccess.ViewFacture"
                    SelectCountMethod="SelectSearchByFKCountCached" SelectMethod="SelectSearchByFK"
                    OnSelecting="odsSearch_Selecting">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="SearchControl1" PropertyName="SearchArguments" Name="sArgument" />
                    </SelectParameters>
                </cc1:GridViewDataSource>
            </div>
        </asp:View>
        <asp:View ID="viewReport" runat="server">
            <div class="paddingKontroli">
                <cc1:ReportControl ID="reportControl" runat="server" TypeName="Broker.DataAccess.ViewFacture"
                    GridViewID="GXGridView1" SearchControlID="SearchControl1" ForeignKeyName="UserID">
                    <cc1:PrintItem HeaderText="Број на фактура" PropertyName="FactureNumber" />
                    <cc1:PrintItem HeaderText="Тип на фактура" PropertyName="DocumentSubTypeDescription" />
                    <cc1:PrintItem HeaderText="Статус" PropertyName="StatusDescription" />
                    <cc1:PrintItem HeaderText="Дата на креирање" PropertyName="DateOfCreation" />
                    <cc1:PrintItem HeaderText="Износ" PropertyName="TotalCost" />
                    <cc1:PrintItem HeaderText="Уплаќач" PropertyName="ClientName" />
                    <cc1:PrintItem HeaderText="Од датум" PropertyName="FromDate" />
                    <cc1:PrintItem HeaderText="До датум" PropertyName="ToDate" />
                </cc1:ReportControl>
            </div>
        </asp:View>
        <asp:View ID="viewDiscardFacture" runat="server">
            <div class="paddingKontroli">
                <asp:DetailsView ID="dvFacture" runat="server" DataSourceID="odsFacturePreview" AutoGenerateRows="false"
                    GridLines="None">
                    <Fields>
                        <asp:TemplateField HeaderText="Број на фактура">
                            <ItemTemplate>
                                <asp:TextBox ID="tbFactureNumber" runat="server" Text='<%# Eval("FactureNumber") %>'
                                    ReadOnly="true" Font-Bold="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Коминтент">
                            <ItemTemplate>
                                <asp:TextBox ID="tbClientEMBG" runat="server" Text='<%# Eval("Client.EMBG") %>' ReadOnly="true"></asp:TextBox>
                                <asp:TextBox ID="tbClientName" Width="250px" runat="server" Text='<%# Eval("Client.Name") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Вкупен износ">
                            <ItemTemplate>
                                <asp:TextBox ID="tbTotalCost" runat="server" CssClass="currencyClass" Text='<%# Eval("TotalCost","{0:#,0.00}") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Брокеража">
                            <ItemTemplate>
                                <asp:TextBox ID="tbBrokerageValue" runat="server" CssClass="currencyClass" Text='<%# Eval("BrokerageValue","{0:#,0.00}") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Дата на фактура">
                            <ItemTemplate>
                                <asp:TextBox ID="tbOfferDate" runat="server" Text='<%# Eval("DateOfCreation","{0:d}") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Валутен рок">
                            <ItemTemplate>
                                <asp:TextBox ID="tbDateOfPayment" runat="server" Text='<%# Bind("DateOfPayment", "{0:d}") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Изработена од">
                            <ItemTemplate>
                                <asp:TextBox ID="tbUserName" runat="server" Text='<%# Eval("User.UserName") %>' ReadOnly="true"></asp:TextBox>
                                <asp:TextBox ID="tbUserFullName" Width="250px" runat="server" Text='<%# Eval("User.Name") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Тип на документ">
                            <ItemTemplate>
                                <asp:TextBox ID="tbDocumentSubType" runat="server" Text='<%# Eval("DocumentSubType.Description") %>'
                                    Width="300px" ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Статус">
                            <ItemTemplate>
                                <asp:TextBox ID="tbStatuse" runat="server" Text='<%# Eval("Statuse.Description") %>'
                                    Width="300px" ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Fields>
                </asp:DetailsView>
                <asp:Button ID="btnDiscardFac" runat="server" Text="Сторнирај" OnClick="btnDiscardFac_Click" />
            </div>
        </asp:View>
        <asp:View ID="viewChangeStatus" runat="server">
            <div class="paddingKontroli">
                <asp:DetailsView ID="dvChangeStatus" runat="server" DataSourceID="odsFactureChangeStatus"
                    DataKeyNames="ID" AutoGenerateRows="false" GridLines="None" DefaultMode="Edit"
                    OnItemCommand="dvChangeStatus_ItemCommand" OnItemUpdating="dvChangeStatus_ItemUpdating">
                    <Fields>
                        <asp:TemplateField HeaderText="Број на фактура">
                            <EditItemTemplate>
                                <asp:TextBox ID="tbFactureNumber" runat="server" Text='<%# Bind("FactureNumber") %>'
                                    ReadOnly="true" Font-Bold="true"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Коминтент">
                            <EditItemTemplate>
                                <asp:TextBox ID="tbClientEMBG" runat="server" Text='<%# Eval("Client.EMBG") %>' ReadOnly="true"></asp:TextBox>
                                <asp:TextBox ID="tbClientName" Width="250px" runat="server" Text='<%# Eval("Client.Name") %>'
                                    ReadOnly="true"></asp:TextBox>
                                <asp:TextBox ID="tbClientID" runat="server" Text='<%#Bind("ClientID") %>' Visible="false"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Вкупен износ">
                            <EditItemTemplate>
                                <asp:TextBox ID="tbTotalCost" runat="server" Text='<%# Bind("TotalCost","{0:#,0.00}") %>'
                                    CssClass="currencyClass" ReadOnly="true"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Брокеража">
                            <EditItemTemplate>
                                <asp:TextBox ID="tbBrokerageValue" runat="server" Text='<%# Bind("BrokerageValue","{0:#,0.00}") %>'
                                    ReadOnly="true" CssClass="currencyClass"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Дата на фактура">
                            <EditItemTemplate>
                                <asp:TextBox ID="tbOfferDate" runat="server" Text='<%# Bind("DateOfCreation","{0:d}") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Изработена од">
                            <EditItemTemplate>
                                <asp:TextBox ID="tbUserName" runat="server" Text='<%# Eval("User.UserName") %>' ReadOnly="true"></asp:TextBox>
                                <asp:TextBox ID="tbUserFullName" Width="250px" runat="server" Text='<%# Eval("User.Name") %>'
                                    ReadOnly="true"></asp:TextBox>
                                <asp:TextBox ID="tbUserID" runat="server" Text='<%#Bind("UserID") %>' Visible="false"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Тип на документ">
                            <EditItemTemplate>
                                <asp:TextBox ID="tbDocumentSubTypeDescription" runat="server" Text='<%# Eval("DocumentSubType.Description") %>'
                                    ReadOnly="true"></asp:TextBox>
                                <asp:TextBox ID="tbDocumentSubTypeDescriptionID" runat="server" Text='<%#Bind("DocumentSubTypeID") %>'
                                    Visible="false"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Од датум">
                            <EditItemTemplate>
                                <asp:TextBox ID="tbFromDate" runat="server" Text='<%# Bind("FromDate", "{0:d}") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="До датум">
                            <EditItemTemplate>
                                <asp:TextBox ID="tbToDate" runat="server" Text='<%# Bind("ToDate", "{0:d}") %>' ReadOnly="true"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Валутен рок">
                            <EditItemTemplate>
                                <asp:TextBox ID="tbDateOfPayment" runat="server" Text='<%# Bind("DateOfPayment", "{0:d}") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Осигурителна компанија">
                            <EditItemTemplate>
                                <asp:TextBox ID="tbInsuranceCompanyName" runat="server" Text='<%# Eval("InsuranceCompany.Name") %>'
                                    ReadOnly="true"></asp:TextBox>
                                <asp:TextBox ID="tbInsuranceCompanyID" runat="server" Text='<%#Bind("InsuranceCompanyID") %>'
                                    Visible="false"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Сторно">
                            <EditItemTemplate>
                                <asp:CheckBox ID="cbDiscard" runat="server" Checked='<%#Bind("Discard") %>' Enabled="false" />
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Статус">
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddlStatuses" runat="server" DataSourceID="odsStatuses" DataTextField="Description"
                                    DataValueField="ID" SelectedValue='<%#Bind("StatusID") %>'>
                                </asp:DropDownList>
                                <asp:ObjectDataSource ID="odsStatuses" runat="server" SelectMethod="GetActiveStatusesForDocumentSubType"
                                    TypeName="Broker.DataAccess.Statuse">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="tbDocumentSubTypeDescriptionID" Name="documentSubTypeID"
                                            PropertyName="Text" Type="Int32" />
                                    </SelectParameters>
                                </asp:ObjectDataSource>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <EditItemTemplate>
                                <asp:Button ID="Button1" Text="Измени" runat="server" OnClick="btnUpdateFactureStatus_Click" />
                                <asp:Button ID="Button2" CommandName="Cancel" Text="Откажи" CausesValidation="false"
                                    runat="server" />
                            </EditItemTemplate>
                        </asp:TemplateField>
                    </Fields>
                </asp:DetailsView>
                <asp:ObjectDataSource ID="odsFactureChangeStatus" runat="server" SelectMethod="Get"
                    TypeName="Broker.DataAccess.Facture" UpdateMethod="Update" ConflictDetection="CompareAllValues"
                    DataObjectTypeName="Broker.DataAccess.Facture" OnUpdating="odsFactureChangeStatus_Updating">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="GXGridView1" Name="id" PropertyName="SelectedValue"
                            Type="Int32" />
                    </SelectParameters>
                    <UpdateParameters>
                        <asp:Parameter Name="oldEntity" Type="Object" />
                        <asp:Parameter Name="newEntity" Type="Object" />
                        <asp:Parameter Name="FromDate" Type="DateTime" />
                        <asp:Parameter Name="ToDate" Type="DateTime" />
                        <asp:Parameter Name="DateOfCreation" Type="DateTime" />
                        <asp:Parameter Name="DateOfPayment" Type="DateTime" />
                        <asp:Parameter Name="BrokerageValue" Type="Decimal" />
                        <asp:Parameter Name="TotalCost" Type="Decimal" />
                        <asp:Parameter Name="ClientID" Type="Int32" />
                        <asp:Parameter Name="InsuranceCompanyID" Type="Int32" />
                        <asp:Parameter Name="DocumentSubTypeID" Type="Int32" />
                        <asp:Parameter Name="StatusID" Type="Int32" />
                        <asp:Parameter Name="UserID" Type="Int32" />
                    </UpdateParameters>
                </asp:ObjectDataSource>
            </div>
        </asp:View>
        <asp:View ID="viewFactureItems" runat="server">
            <div class="paddingKontroli">
                <asp:DetailsView ID="DetailsViewFacturePreview" runat="server" DataSourceID="odsFacturePreview"
                    AutoGenerateRows="false" GridLines="None">
                    <Fields>
                        <asp:TemplateField HeaderText="Број на фактура">
                            <ItemTemplate>
                                <asp:TextBox ID="tbFactureNumber" runat="server" Text='<%# Eval("FactureNumber") %>'
                                    ReadOnly="true" Font-Bold="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Коминтент">
                            <ItemTemplate>
                                <asp:TextBox ID="tbClientEMBG" runat="server" Text='<%# Eval("Client.EMBG") %>' ReadOnly="true"></asp:TextBox>
                                <asp:TextBox ID="tbClientName" Width="250px" runat="server" Text='<%# Eval("Client.Name") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Вкупен износ">
                            <ItemTemplate>
                                <asp:TextBox ID="tbTotalCost" runat="server" CssClass="currencyClass" Text='<%# Eval("TotalCost","{0:#,0.00}") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Брокеража">
                            <ItemTemplate>
                                <asp:TextBox ID="tbBrokerageValue" runat="server" CssClass="currencyClass" Text='<%# Eval("BrokerageValue","{0:#,0.00}") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Дата на фактура">
                            <ItemTemplate>
                                <asp:TextBox ID="tbOfferDate" runat="server" Text='<%# Eval("DateOfCreation","{0:d}") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Валутен рок">
                            <ItemTemplate>
                                <asp:TextBox ID="tbDateOfPayment" runat="server" Text='<%# Bind("DateOfPayment", "{0:d}") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Изработена од">
                            <ItemTemplate>
                                <asp:TextBox ID="tbUserName" runat="server" Text='<%# Eval("User.UserName") %>' ReadOnly="true"></asp:TextBox>
                                <asp:TextBox ID="tbUserFullName" Width="250px" runat="server" Text='<%# Eval("User.Name") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Тип на документ">
                            <ItemTemplate>
                                <asp:TextBox ID="tbDocumentSubType" runat="server" Text='<%# Eval("DocumentSubType.Description") %>'
                                    Width="300px" ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Статус">
                            <ItemTemplate>
                                <asp:TextBox ID="tbStatuse" runat="server" Text='<%# Eval("Statuse.Description") %>'
                                    Width="300px" ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Fields>
                </asp:DetailsView>
                <asp:ObjectDataSource ID="odsFacturePreview" runat="server" SelectMethod="Get" TypeName="Broker.DataAccess.Facture">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="GXGridView1" Name="id" PropertyName="SelectedValue"
                            Type="Int32" />
                    </SelectParameters>
                </asp:ObjectDataSource>
                <asp:GridView ID="GridViewFactureItems" runat="server" OnSelectedIndexChanged="GridViewFactureItems_SelectedIndexChanged"
                    OnRowCommand="GridViewFactureItems_RowCommand" DataSourceID="odsFactureItems"
                    Width="100%" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"
                    Caption="Ставки на фактура" EmptyDataText="Нема записи кои го задоволуваат критериумот на пребарување!"
                    RowStyle-CssClass="row" CssClass="grid" GridLines="None">
                    <RowStyle CssClass="row"></RowStyle>
                    <Columns>
                        <asp:TemplateField HeaderText="Реден број">
                            <ItemTemplate>
                                <asp:Label ID="tbNumber" runat="server" ReadOnly="true" Text='<%#Eval("Number") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Опис на ставка">
                            <ItemTemplate>
                                <asp:Label ID="tbDescription" runat="server" ReadOnly="true" Text='<%#Eval("Description") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Број на полиси">
                            <ItemTemplate>
                                <asp:Label ID="tbCount" runat="server" ReadOnly="true" Text='<%#Eval("Count") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Премија" ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <asp:Label ID="tbCostPremiumValue" runat="server" Text='<%#Eval("PremiumValue", "{0:#,0.00}") %>'
                                    ReadOnly="true"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Брокеража" ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <asp:Label ID="tbCostBrokerageValue" runat="server" Text='<%#Eval("BrokerageValue", "{0:#,0.00}") %>'
                                    ReadOnly="true"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <PagerStyle HorizontalAlign="Center" />
                    <%--<SelectedRowStyle CssClass="rowSelected" />--%>
                    <PagerSettings FirstPageText="<< Прва " PreviousPageText="< Претходна " NextPageText=" Следна >"
                        LastPageText=" Последна >>" Mode="NextPreviousFirstLast" />
                </asp:GridView>
                <asp:ObjectDataSource ID="odsFactureItems" runat="server" SelectMethod="GetByFacture"
                    TypeName="Broker.DataAccess.FactureItem">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="GXGridView1" Name="factureID" PropertyName="SelectedValue"
                            Type="Int32" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </div>
        </asp:View>
        <asp:View ID="viewAttachments" runat="server">
            <div class="paddingKontroli">
                <asp:DetailsView ID="dvFacturePreviewForAttachments" runat="server" DataSourceID="odsFacturePreviewForAttachments"
                    DataKeyNames="ID" AutoGenerateRows="False" OnItemCommand="dvFacturePreviewForAttachments_ItemCommand"
                    OnModeChanging="dvFacturePreviewForAttachments_ModeChanging" GridLines="None"
                    DefaultMode="ReadOnly">
                    <Fields>
                        <asp:TemplateField HeaderText="Број на фактура">
                            <ItemTemplate>
                                <asp:TextBox ID="tbFactureNumber" runat="server" Text='<%# Eval("FactureNumber") %>'
                                    ReadOnly="true" Font-Bold="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Коминтент">
                            <ItemTemplate>
                                <asp:TextBox ID="tbClientEMBG" runat="server" Text='<%# Eval("Client.EMBG") %>' ReadOnly="true"></asp:TextBox>
                                <asp:TextBox ID="tbClientName" Width="250px" runat="server" Text='<%# Eval("Client.Name") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Вкупен износ" ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <asp:TextBox ID="tbTotalCost" runat="server" Text='<%# Eval("TotalCost", "{0:#,0.00}") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Брокеража" ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <asp:TextBox ID="tbBrokerageValue" runat="server" Text='<%# Eval("BrokerageValue", "{0:#,0.00}") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Дата на фактура">
                            <ItemTemplate>
                                <asp:TextBox ID="tbOfferDate" runat="server" Text='<%# Eval("DateOfCreation","{0:d}") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Изработена од">
                            <ItemTemplate>
                                <asp:TextBox ID="tbUserName" runat="server" Text='<%# Eval("User.UserName") %>' ReadOnly="true"></asp:TextBox>
                                <asp:TextBox ID="tbUserFullName" Width="250px" runat="server" Text='<%# Eval("User.Name") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Fields>
                </asp:DetailsView>
                <cc1:DetailsViewDataSource ID="odsFacturePreviewForAttachments" runat="server" TypeName="Broker.DataAccess.Facture"
                    ConflictDetection="CompareAllValues" DataObjectTypeName="Broker.DataAccess.Facture"
                    SelectMethod="Get" OldValuesParameterFormatString="oldEntity" OnSelecting="odsFacturePreviewForAttachments_Selecting">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="GXGridView1" Name="id" PropertyName="SelectedValue"
                            Type="Int32" />
                    </SelectParameters>
                </cc1:DetailsViewDataSource>
                <table>
                    <tr>
                        <td>
                            Датотека
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
                            &nbsp;
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
