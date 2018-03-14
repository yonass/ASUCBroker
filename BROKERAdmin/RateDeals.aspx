<%@ Page Title="Договори за плаќање на рати со фирми" Language="C#" MasterPageFile="~/MasterPage.master"
    AutoEventWireup="true" CodeFile="RateDeals.aspx.cs" Inherits="BROKERAdmin_RateDeals" %>

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
            <asp:Button ID="btnRateDealItems" runat="server" ToolTip="Ставки" OnClick="btnRateDealItems_Click"
                CausesValidation="false" UseSubmitBehavior="false" CssClass="stavki" BorderWidth="0px"
                Enabled="false" />
        </div>
        <div id="button08">
            <asp:Button ID="btnAttachments" runat="server" ToolTip="Документи" OnClick="btnAttachments_Click"
                Enabled="false" CausesValidation="false" UseSubmitBehavior="false" CssClass="dokumenti"
                BorderWidth="0px" />
        </div>
    </div>
    <asp:MultiView ID="mvMain" runat="server">
        <asp:View ID="viewGrid" runat="server">
            <div id="tabeliFrame">
                <div id="header">
                    <div id="content">
                        Договори со компании за рати
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
                                <asp:BoundField HeaderText="Број на договор" DataField="DealNumber" SortExpression="DealNumber" />
                                <asp:BoundField HeaderText="Компанија (ЕМБГ)" DataField="EMBG" SortExpression="EMBG" />
                                <asp:BoundField HeaderText="Компанија" DataField="Name" SortExpression="Name" />
                                <asp:BoundField HeaderText="Од датум" DataField="FromDate" DataFormatString="{0:d}"
                                    SortExpression="FromDate" />
                                <asp:BoundField HeaderText="До датум" DataField="ToDate" DataFormatString="{0:d}"
                                    SortExpression="ToDate" />
                                <asp:CheckBoxField HeaderText="Активен" DataField="IsActive" SortExpression="IsActive" />
                            </Columns>
                            <PagerStyle HorizontalAlign="Center" />
                            <SelectedRowStyle CssClass="rowSelected" />
                            <PagerSettings FirstPageText="<< Прва " PreviousPageText="< Претходна " NextPageText=" Следна >"
                                LastPageText=" Последна >>" Mode="NextPreviousFirstLast" />
                        </cc1:GXGridView>
                        <cc1:GridViewDataSource ID="odsGridView" runat="server" TypeName="Broker.DataAccess.ViewRateDeal"
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
                                        <cc1:FilterItem FieldName="Број на договор" PropertyName="DealNumber" Comparator="StringComparators"
                                            DataType="String" />
                                        <cc1:FilterItem FieldName="Компанија (ЕМБГ)" PropertyName="EMBG" Comparator="StringComparators"
                                            DataType="String" />
                                        <cc1:FilterItem FieldName="Компанија" PropertyName="Name" Comparator="StringComparators"
                                            DataType="String" />
                                        <cc1:FilterItem FieldName="Адреса" PropertyName="Address" Comparator="StringComparators"
                                            DataType="String" />
                                        <cc1:FilterItem FieldName="Од датум" PropertyName="FromDate" Comparator="NumericComparators"
                                            DataType="DateTime" />
                                        <cc1:FilterItem FieldName="До датум" PropertyName="ToDate" Comparator="NumericComparators"
                                            DataType="DateTime" />
                                        <cc1:FilterItem FieldName="Датум на договор" PropertyName="DealDate" Comparator="NumericComparators"
                                            DataType="DateTime" />
                                        <cc1:FilterItem FieldName="Активен" PropertyName="IsActive" Comparator="BooleanComparators"
                                            DataType="Boolean" />
                                    </cc1:FilterControl>
                                </div>
                                <cc1:FilterDataSource ID="odsFilterGridView" runat="server" TypeName="Broker.DataAccess.ViewRateDeal">
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
                                    Нов запис во Договори
                                </div>
                            </InsertItemTemplate>
                            <EditItemTemplate>
                                <div class="subTitles">
                                    Измена на запис во Договори
                                </div>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <div class="subTitles">
                                    Бришење на запис од Договори
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Број на договор">
                            <InsertItemTemplate>
                                <asp:TextBox ID="tbDealNumber" runat="server" Text='<%# Bind("DealNumber") %>' MaxLength="50"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvDealNumber" runat="server" ControlToValidate="tbDealNumber"
                                    Display="Dynamic" ErrorMessage="*">
                                </asp:RequiredFieldValidator>
                                <super:EntityCallOutValidator ID="DealNumberInsertValidator" PropertyName="DEAL_NUMBER_INSERT_EXISTS"
                                    runat="server" />
                            </InsertItemTemplate>
                            <ItemTemplate>
                                <asp:TextBox ID="tbDealNumber" runat="server" Text='<%# Bind("DealNumber") %>' ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="tbDealNumber" runat="server" Text='<%# Bind("DealNumber") %>' MaxLength="50"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvDealNumber" runat="server" ControlToValidate="tbDealNumber"
                                    ErrorMessage="*"></asp:RequiredFieldValidator>
                                <super:EntityCallOutValidator ID="DealNumberUpdateValidator" PropertyName="DEAL_NUMBER_UPDATE_EXISTS"
                                    runat="server" />
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Компанија">
                            <ItemTemplate>
                                <asp:DropDownList ID="ddlClients" runat="server" DataSourceID="odsClients" DataTextField="Name"
                                    DataValueField="ID" Enabled="false" SelectedValue='<%# Bind("ClientID") %>'>
                                </asp:DropDownList>
                                <asp:ObjectDataSource ID="odsClients" runat="server" SelectMethod="GetLawClients"
                                    TypeName="Broker.DataAccess.Client"></asp:ObjectDataSource>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddlClients" runat="server" DataSourceID="odsClients" DataTextField="Name"
                                    Enabled="false" DataValueField="ID" SelectedValue='<%# Bind("ClientID") %>'>
                                </asp:DropDownList>
                                <asp:ObjectDataSource ID="odsClients" runat="server" SelectMethod="GetLawClients"
                                    TypeName="Broker.DataAccess.Client"></asp:ObjectDataSource>
                            </EditItemTemplate>
                            <InsertItemTemplate>
                                <asp:DropDownList ID="ddlClients" runat="server" DataSourceID="odsClients" DataTextField="Name"
                                    DataValueField="ID" SelectedValue='<%# Bind("ClientID") %>'>
                                </asp:DropDownList>
                                <asp:ObjectDataSource ID="odsClients" runat="server" SelectMethod="GetLawClients"
                                    TypeName="Broker.DataAccess.Client"></asp:ObjectDataSource>
                            </InsertItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Од датум">
                            <InsertItemTemplate>
                                <asp:TextBox ID="tbFromDate" runat="server" Text='<%# Bind("FromDate" , "{0:d}") %>'></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" ControlToValidate="tbFromDate"
                                    Display="Dynamic" ErrorMessage="*">
                                </asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="cvFromDate" runat="server" ErrorMessage="*" Display="Dynamic"
                                    Operator="DataTypeCheck" Type="Date" ControlToValidate="tbFromDate"></asp:CompareValidator>
                            </InsertItemTemplate>
                            <ItemTemplate>
                                <asp:TextBox ID="tbFromDate" runat="server" Text='<%# Bind("FromDate", "{0:d}") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="tbFromDate" runat="server" Text='<%# Bind("FromDate", "{0:d}") %>'></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" ControlToValidate="tbFromDate"
                                    Display="Dynamic" ErrorMessage="*">
                                </asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="cvFromDate" runat="server" ErrorMessage="*" Display="Dynamic"
                                    Operator="DataTypeCheck" Type="Date" ControlToValidate="tbFromDate"></asp:CompareValidator>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="До датум">
                            <InsertItemTemplate>
                                <asp:TextBox ID="tbToDate" runat="server" Text='<%# Bind("ToDate" , "{0:d}") %>'></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvToDate" runat="server" ControlToValidate="tbToDate"
                                    Display="Dynamic" ErrorMessage="*">
                                </asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="cvToDate" runat="server" ErrorMessage="*" Display="Dynamic"
                                    Operator="DataTypeCheck" Type="Date" ControlToValidate="tbToDate"></asp:CompareValidator>
                                <asp:CompareValidator ID="cvToDateGTE" runat="server" ErrorMessage="*" ControlToValidate="tbToDate"
                                    ControlToCompare="tbFromDate" Display="Dynamic" Operator="GreaterThanEqual" Type="Date"></asp:CompareValidator>
                            </InsertItemTemplate>
                            <ItemTemplate>
                                <asp:TextBox ID="tbToDate" runat="server" Text='<%# Bind("ToDate", "{0:d}") %>' ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="tbToDate" runat="server" Text='<%# Bind("ToDate", "{0:d}") %>'></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvToDate" runat="server" ControlToValidate="tbToDate"
                                    Display="Dynamic" ErrorMessage="*">
                                </asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="cvToDate" runat="server" ErrorMessage="*" Display="Dynamic"
                                    Operator="DataTypeCheck" Type="Date" ControlToValidate="tbToDate"></asp:CompareValidator>
                                <asp:CompareValidator ID="cvToDateGTE" runat="server" ErrorMessage="*" ControlToValidate="tbToDate"
                                    ControlToCompare="tbFromDate" Display="Dynamic" Operator="GreaterThanEqual" Type="Date"></asp:CompareValidator>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Датум на договор">
                            <InsertItemTemplate>
                                <asp:TextBox ID="tbDealDate" runat="server" Text='<%# Bind("DealDate" , "{0:d}") %>'></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvDealDate" runat="server" ControlToValidate="tbDealDate"
                                    Display="Dynamic" ErrorMessage="*">
                                </asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="cvDealDate" runat="server" ErrorMessage="*" Display="Dynamic"
                                    Operator="DataTypeCheck" Type="Date" ControlToValidate="tbDealDate"></asp:CompareValidator>
                            </InsertItemTemplate>
                            <ItemTemplate>
                                <asp:TextBox ID="tbDealDate" runat="server" Text='<%# Bind("DealDate", "{0:d}") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="tbDealDate" runat="server" Text='<%# Bind("DealDate", "{0:d}") %>'></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvDealDate" runat="server" ControlToValidate="tbDealDate"
                                    Display="Dynamic" ErrorMessage="*">
                                </asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="cvDealDate" runat="server" ErrorMessage="*" Display="Dynamic"
                                    Operator="DataTypeCheck" Type="Date" ControlToValidate="tbDealDate"></asp:CompareValidator>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Активен">
                            <ItemTemplate>
                                <asp:CheckBox ID="cbIsActive" runat="server" Checked='<%#Bind("IsActive") %>' Enabled="false" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:CheckBox ID="cbIsActive" runat="server" Checked='<%#Bind("IsActive") %>' />
                            </EditItemTemplate>
                            <InsertItemTemplate>
                                <asp:CheckBox ID="cbIsActive" runat="server" Checked="true" Enabled="false" />
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
                <cc1:DetailsViewDataSource ID="dvDataSource" runat="server" TypeName="Broker.DataAccess.RateDeal"
                    ConflictDetection="CompareAllValues" DataObjectTypeName="Broker.DataAccess.RateDeal"
                    DeleteMethod="Delete" InsertMethod="Insert" SelectMethod="Get" UpdateMethod="Update"
                    OnUpdating="dvDataSource_Updating" OnUpdated="dvDataSource_Updated" OnInserted="dvDataSource_Inserted"
                    OnInserting="dvDataSource_Inserting">
                    <UpdateParameters>
                        <asp:Parameter Name="oldEntity" Type="Object" />
                        <asp:Parameter Name="newEntity" Type="Object" />
                        <asp:Parameter Name="DealDate" Type="DateTime" />
                        <asp:Parameter Name="FromDate" Type="DateTime" />
                        <asp:Parameter Name="ToDate" Type="DateTime" />
                    </UpdateParameters>
                    <InsertParameters>
                        <asp:Parameter Name="entityToInsert" Type="Object" />
                        <asp:Parameter Name="DealDate" Type="DateTime" />
                        <asp:Parameter Name="FromDate" Type="DateTime" />
                        <asp:Parameter Name="ToDate" Type="DateTime" />
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
                    <cc1:SearchItem FieldName="Број на договор" PropertyName="DealNumber" Comparator="StringComparators"
                        DataType="String" />
                    <cc1:SearchItem FieldName="Компанија (ЕМБГ)" PropertyName="EMBG" Comparator="StringComparators"
                        DataType="String" />
                    <cc1:SearchItem FieldName="Компанија" PropertyName="Name" Comparator="StringComparators"
                        DataType="String" />
                    <cc1:SearchItem FieldName="Адреса" PropertyName="Address" Comparator="StringComparators"
                        DataType="String" />
                    <cc1:SearchItem FieldName="Од датум" PropertyName="FromDate" Comparator="NumericComparators"
                        DataType="DateTime" />
                    <cc1:SearchItem FieldName="До датум" PropertyName="ToDate" Comparator="NumericComparators"
                        DataType="DateTime" />
                    <cc1:SearchItem FieldName="Датум на договор" PropertyName="DealDate" Comparator="NumericComparators"
                        DataType="DateTime" />
                    <cc1:SearchItem FieldName="Активен" PropertyName="IsActive" Comparator="BooleanComparators"
                        DataType="Boolean" />
                </cc1:SearchControl>
                <cc1:GridViewDataSource ID="odsSearch" runat="server" TypeName="Broker.DataAccess.ViewRateDeal"
                    SelectCountMethod="SelectSearchCountCached" SelectMethod="SelectSearch">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="SearchControl1" PropertyName="SearchArguments" Name="sArgument" />
                    </SelectParameters>
                </cc1:GridViewDataSource>
            </div>
        </asp:View>
        <asp:View ID="viewReport" runat="server">
            <div class="paddingKontroli">
                <cc1:ReportControl ID="reportControl" runat="server" TypeName="Broker.DataAccess.ViewRateDeal"
                    GridViewID="GXGridView1" SearchControlID="SearchControl1">
                    <cc1:PrintItem HeaderText="Број на договор" PropertyName="DealNumber" />
                    <cc1:PrintItem HeaderText="Компанија (ЕМБГ)" PropertyName="EMBG" />
                    <cc1:PrintItem HeaderText="Компанија" PropertyName="Name" />
                    <cc1:PrintItem HeaderText="Адреса" PropertyName="Address" />
                    <cc1:PrintItem HeaderText="Од датум" PropertyName="FromDate" />
                    <cc1:PrintItem HeaderText="До датум" PropertyName="ToDate" />
                    <cc1:PrintItem HeaderText="Датум на договор" PropertyName="DealDate" />
                    <cc1:PrintItem HeaderText="Активен" PropertyName="IsActive" />
                </cc1:ReportControl>
            </div>
        </asp:View>
        <asp:View ID="viewAttachments" runat="server">
            <div class="paddingKontroli">
                <asp:DetailsView ID="DetailsViewRateDealsForAttachments" runat="server" DataSourceID="odsRateDealsForAttachments"
                    DataKeyNames="ID" AutoGenerateRows="False" OnItemCommand="DetailsViewRateDealsForAttachments_ItemCommand"
                    OnModeChanging="DetailsViewRateDealsForAttachments_ModeChanging" GridLines="None"
                    DefaultMode="ReadOnly">
                    <Fields>
                        <asp:TemplateField HeaderText="Број на договор">
                            <ItemTemplate>
                                <asp:TextBox ID="tbDealNumber" runat="server" Text='<%# Bind("DealNumber") %>' ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Дата на договор">
                            <ItemTemplate>
                                <asp:TextBox ID="tbDealDate" runat="server" Text='<%# Bind("DealDate", "{0:d}") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Од">
                            <ItemTemplate>
                                <asp:TextBox ID="tbFromDate" runat="server" Text='<%# Bind("FromDate", "{0:d}") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="До">
                            <ItemTemplate>
                                <asp:TextBox ID="tbToDate" runat="server" Text='<%# Bind("ToDate", "{0:d}") %>' ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Компанија">
                            <ItemTemplate>
                                <asp:TextBox ID="tbClientEMBG" Width="120px" runat="server" Text='<%# Eval("Client.EMBG") %>'
                                    ReadOnly="true"></asp:TextBox>
                                <asp:TextBox ID="tbClientName" Width="250px" runat="server" Text='<%# Eval("Client.Name") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Fields>
                </asp:DetailsView>
                <cc1:DetailsViewDataSource ID="odsRateDealsForAttachments" runat="server" TypeName="Broker.DataAccess.RateDeal"
                    ConflictDetection="CompareAllValues" DataObjectTypeName="Broker.DataAccess.RateDeal"
                    SelectMethod="Get" OldValuesParameterFormatString="oldEntity" OnSelecting="odsRateDealsForAttachments_Selecting">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="GXGridView1" Name="id" PropertyName="SelectedValue"
                            Type="Int32" />
                    </SelectParameters>
                </cc1:DetailsViewDataSource>
                <table>
                    <tr>
                        <td>
                            Датотека<asp:FileUpload ID="FileUpload1" runat="server" />
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
        <asp:View ID="viewRateDealItems" runat="server">
            <div class="paddingKontroli">
                <asp:DetailsView ID="dvRateDealPreview" runat="server" DataSourceID="odsRateDealPreview"
                    DataKeyNames="ID" AutoGenerateRows="False" OnItemCommand="dvRateDealPreview_ItemCommand"
                    OnModeChanging="dvRateDealPreview_ModeChanging" GridLines="None" DefaultMode="ReadOnly">
                    <Fields>
                        <asp:TemplateField HeaderText="Број на договор">
                            <ItemTemplate>
                                <asp:TextBox ID="tbDealNumber" runat="server" Text='<%# Bind("DealNumber") %>' ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Дата на договор">
                            <ItemTemplate>
                                <asp:TextBox ID="tbDealDate" runat="server" Text='<%# Bind("DealDate", "{0:d}") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Од">
                            <ItemTemplate>
                                <asp:TextBox ID="tbFromDate" runat="server" Text='<%# Bind("FromDate", "{0:d}") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="До">
                            <ItemTemplate>
                                <asp:TextBox ID="tbToDate" runat="server" Text='<%# Bind("ToDate", "{0:d}") %>' ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Компанија">
                            <ItemTemplate>
                                <asp:TextBox ID="tbClientEMBG" Width="120px" runat="server" Text='<%# Eval("Client.EMBG") %>'
                                    ReadOnly="true"></asp:TextBox>
                                <asp:TextBox ID="tbClientName" Width="250px" runat="server" Text='<%# Eval("Client.Name") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Fields>
                </asp:DetailsView>
                <cc1:DetailsViewDataSource ID="odsRateDealPreview" runat="server" TypeName="Broker.DataAccess.RateDeal"
                    ConflictDetection="CompareAllValues" DataObjectTypeName="Broker.DataAccess.RateDeal"
                    SelectMethod="Get" OldValuesParameterFormatString="oldEntity" OnSelecting="odsRateDealPreview_Selecting">
                    <%--<SelectParameters>
                        <asp:ControlParameter ControlID="GXGridView1" Name="id" PropertyName="SelectedValue"
                            Type="Int32" />
                    </SelectParameters>--%>
                </cc1:DetailsViewDataSource>
                <asp:MultiView ID="mvRateDealItems" runat="server">
                    <asp:View ID="viewRateDealItemsGrid" runat="server">
                        <asp:GridView ID="GridViewRateDealItems" runat="server" DataSourceID="odsGridViewRateDealItems"
                            DataKeyNames="ID" Width="100%" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"
                            Caption="Ставки на договорот" EmptyDataText="Нема записи кои го задоволуваат критериумот на пребарување!"
                            RowStyle-CssClass="row" CssClass="grid" GridLines="None" OnRowDeleting="GridViewRateDealItems_RowDeleting"
                            OnRowDeleted="GridViewRateDealItems_RowDeleted">
                            <RowStyle CssClass="row"></RowStyle>
                            <Columns>
                                <asp:TemplateField Visible="false">
                                    <ItemTemplate>
                                        <asp:TextBox ID="tbRateDealItemID" runat="server" Text='<%#Bind("ID") %>' Visible="false"></asp:TextBox>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="tbRateDealItemID" runat="server" Text='<%#Bind("ID") %>' Visible="false"></asp:TextBox>
                                    </EditItemTemplate>
                                    <InsertItemTemplate>
                                        <asp:TextBox ID="tbRateDealItemID" runat="server" Text='<%#Bind("ID") %>' Visible="false"></asp:TextBox>
                                    </InsertItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <InsertItemTemplate>
                                        <asp:Label ID="tbRateDealID" runat="server" Text='<%#Bind("RateDealID") %>' Visible="false"></asp:Label>
                                    </InsertItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="tbRateDealID" runat="server" Text='<%#Bind("RateDealID") %>' Visible="false"></asp:Label>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="tbRateDealID" runat="server" Text='<%#Bind("RateDealID") %>' Visible="false"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Подкласа на осигурување">
                                    <InsertItemTemplate>
                                        <asp:DropDownList ID="ddlInsuranceSubTypes" runat="server" DataSourceID="odsInsuranceSubTypes"
                                            Width="200px" DataTextField="Description" DataValueField="ID" SelectedValue='<%# Bind("InsuranceSubTypeID") %>'>
                                        </asp:DropDownList>
                                        <asp:ObjectDataSource ID="odsInsuranceSubTypes" runat="server" SelectMethod="GetForOrder"
                                            TypeName="Broker.DataAccess.InsuranceSubType"></asp:ObjectDataSource>
                                    </InsertItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlInsuranceSubTypes" runat="server" DataSourceID="odsInsuranceSubTypes"
                                            Width="200px" DataTextField="Description" DataValueField="ID" SelectedValue='<%# Bind("InsuranceSubTypeID") %>'>
                                        </asp:DropDownList>
                                        <asp:ObjectDataSource ID="odsInsuranceSubTypes" runat="server" SelectMethod="GetForOrder"
                                            TypeName="Broker.DataAccess.InsuranceSubType"></asp:ObjectDataSource>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="tbInsuranceSubType" runat="server" Text='<%#Eval("InsuranceSubType.Description") %>'
                                            Width="200px"></asp:Label>
                                        <asp:Label ID="tbInsuranceSubTypeID" runat="server" Text='<%#Bind("InsuranceSubTypeID") %>'
                                            Visible="false"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Број на рати">
                                    <InsertItemTemplate>
                                        <asp:TextBox ID="tbNumberOfRates" Width="50px" runat="server" Text='<%#Bind("NumberOfRates") %>'></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvNumberOfRates" runat="server" ControlToValidate="tbNumberOfRates"
                                            ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:CompareValidator ID="cvNumberOfRates" runat="server" ControlToValidate="tbNumberOfRates"
                                            ErrorMessage="*" Display="Dynamic" Operator="DataTypeCheck" Type="Integer"></asp:CompareValidator>
                                    </InsertItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="tbNumberOfRates" Width="50px" runat="server" Text='<%#Bind("NumberOfRates") %>'></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvNumberOfRates" runat="server" ControlToValidate="tbNumberOfRates"
                                            ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:CompareValidator ID="cvNumberOfRates" runat="server" ControlToValidate="tbNumberOfRates"
                                            ErrorMessage="*" Display="Dynamic" Operator="DataTypeCheck" Type="Integer"></asp:CompareValidator>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="tbNumberOfRates" Width="50px" runat="server" Text='<%#Bind("NumberOfRates") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Од вредност">
                                    <InsertItemTemplate>
                                        <asp:TextBox ID="tbFromValue" Width="70px" runat="server" Text='<%#Bind("FromValue", "{0:0}") %>'></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvNumberOfRates" runat="server" ControlToValidate="tbFromValue"
                                            ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:CompareValidator ID="cvFromValue" runat="server" ControlToValidate="tbFromValue"
                                            ErrorMessage="*" Display="Dynamic" Operator="DataTypeCheck" Type="Double"></asp:CompareValidator>
                                    </InsertItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="tbFromValue" Width="70px" runat="server" Text='<%#Bind("FromValue", "{0:0}") %>'></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvFromValue" runat="server" ControlToValidate="tbFromValue"
                                            ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:CompareValidator ID="cvFromValue" runat="server" ControlToValidate="tbFromValue"
                                            ErrorMessage="*" Display="Dynamic" Operator="DataTypeCheck" Type="Double"></asp:CompareValidator>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="tbFromValue" Width="70px" runat="server" Text='<%#Bind("FromValue", "{0:0}") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="До вредност">
                                    <InsertItemTemplate>
                                        <asp:TextBox ID="tbToValue" Width="70px" runat="server" Text='<%#Bind("ToValue", "{0:0}") %>'></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvToOfRates" runat="server" ControlToValidate="tbToValue"
                                            ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:CompareValidator ID="cvToValue" runat="server" ControlToValidate="tbToValue"
                                            ErrorMessage="*" Display="Dynamic" Operator="DataTypeCheck" Type="Integer"></asp:CompareValidator>
                                        <asp:CompareValidator ID="cvToValueGTE" runat="server" ErrorMessage="*" ControlToValidate="tbToValue"
                                            ControlToCompare="tbFromValue" Display="Dynamic" Operator="GreaterThanEqual"
                                            Type="Date"></asp:CompareValidator>
                                    </InsertItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="tbToValue" Width="70px" runat="server" Text='<%#Bind("ToValue", "{0:0}") %>'></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvToValue" runat="server" ControlToValidate="tbToValue"
                                            ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:CompareValidator ID="cvToValue" runat="server" ControlToValidate="tbToValue"
                                            ErrorMessage="*" Display="Dynamic" Operator="DataTypeCheck" Type="Integer"></asp:CompareValidator>
                                        <asp:CompareValidator ID="cvToValueGTE" runat="server" ErrorMessage="*" ControlToValidate="tbToValue"
                                            ControlToCompare="tbFromValue" Display="Dynamic" Operator="GreaterThanEqual"
                                            Type="Date"></asp:CompareValidator>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="tbToValue" Width="70px" runat="server" Text='<%#Bind("ToValue", "{0:0}") %>'></asp:Label>
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
                        <asp:ObjectDataSource ID="odsGridViewRateDealItems" runat="server" TypeName="Broker.DataAccess.RateDealInsuranceSubType"
                            OldValuesParameterFormatString="oldEntity" DataObjectTypeName="Broker.DataAccess.RateDealInsuranceSubType"
                            ConflictDetection="CompareAllValues" DeleteMethod="Delete" InsertMethod="Insert"
                            SelectMethod="GetByRateDeal" UpdateMethod="Update" OnSelecting="odsGridViewRateDealItems_Selecting">
                            <UpdateParameters>
                                <asp:Parameter Name="oldEntity" Type="Object" />
                                <asp:Parameter Name="newEntity" Type="Object" />
                            </UpdateParameters>
                        </asp:ObjectDataSource>
                        <div>
                            <asp:Button ID="btnNewRateDealItem" runat="server" Text="Нова ставка" OnClick="btnNewRateDealItem_Click" />
                        </div>
                    </asp:View>
                    <asp:View ID="viewRateDealItemsEdit" runat="server">
                        <asp:DetailsView ID="DetailsViewRateDealItems" runat="server" DataSourceID="dvDataSourceRateDealItems"
                            DataKeyNames="ID" AutoGenerateRows="False" OnItemCommand="DetailsViewRateDealItems_ItemCommand"
                            OnItemInserted="DetailsViewRateDealItems_ItemInserted" OnModeChanging="DetailsViewRateDealItems_ModeChanging"
                            OnItemInserting="DetailsViewRateDealItems_ItemInserting" GridLines="None" DefaultMode="Insert">
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
                                        <asp:TextBox ID="tbRateDealItemID" runat="server" Text='<%#Bind("ID") %>' Visible="false"></asp:TextBox>
                                    </InsertItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Подкласа на осигурување">
                                    <InsertItemTemplate>
                                        <asp:DropDownList ID="ddlInsuranceSubTypes" runat="server" DataSourceID="odsInsuranceSubTypes"
                                            DataTextField="Description" Width="350px" DataValueField="ID" SelectedValue='<%# Bind("InsuranceSubTypeID") %>'>
                                        </asp:DropDownList>
                                        <asp:ObjectDataSource ID="odsInsuranceSubTypes" runat="server" SelectMethod="GetForOrder"
                                            TypeName="Broker.DataAccess.InsuranceSubType"></asp:ObjectDataSource>
                                    </InsertItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Број на рати">
                                    <InsertItemTemplate>
                                        <asp:TextBox ID="tbNumberOfRates" runat="server" Text='<%#Bind("NumberOfRates") %>'></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvNumberOfRates" runat="server" ControlToValidate="tbNumberOfRates"
                                            ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:CompareValidator ID="cvNumberOfRates" runat="server" ControlToValidate="tbNumberOfRates"
                                            ErrorMessage="*" Display="Dynamic" Operator="DataTypeCheck" Type="Integer"></asp:CompareValidator>
                                    </InsertItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Од вредност">
                                    <InsertItemTemplate>
                                        <asp:TextBox ID="tbFromValue" Width="100px" runat="server" Text='<%#Bind("FromValue") %>'></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvFromValue" runat="server" ControlToValidate="tbFromValue"
                                            ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:CompareValidator ID="cvFromValue" runat="server" ControlToValidate="tbFromValue"
                                            ErrorMessage="*" Display="Dynamic" Operator="DataTypeCheck" Type="Integer"></asp:CompareValidator>
                                    </InsertItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="До вредност">
                                    <InsertItemTemplate>
                                        <asp:TextBox ID="tbToValue" Width="100px" runat="server" Text='<%#Bind("ToValue") %>'></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvToValue" runat="server" ControlToValidate="tbToValue"
                                            ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:CompareValidator ID="cvToValue" runat="server" ControlToValidate="tbToValue"
                                            ErrorMessage="*" Display="Dynamic" Operator="DataTypeCheck" Type="Integer"></asp:CompareValidator>
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
                        <cc1:DetailsViewDataSource ID="dvDataSourceRateDealItems" runat="server" TypeName="Broker.DataAccess.RateDealInsuranceSubType"
                            ConflictDetection="CompareAllValues" DataObjectTypeName="Broker.DataAccess.RateDealInsuranceSubType"
                            InsertMethod="Insert" SelectMethod="Get" OnSelecting="dvDataSourceRateDealItems_Selecting">
                            <InsertParameters>
                                <asp:Parameter Name="entityToInsert" Type="Object" />
                            </InsertParameters>
                            <SelectParameters>
                                <asp:ControlParameter ControlID="GridViewRateDealItems" Name="id" PropertyName="SelectedValue"
                                    Type="Int32" />
                            </SelectParameters>
                        </cc1:DetailsViewDataSource>
                    </asp:View>
                </asp:MultiView>
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>
