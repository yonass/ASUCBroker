<%@ Page Title="������� �����" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Orders.aspx.cs" Inherits="Broker_Orders" Culture="mk-MK" UICulture="mk-MK" %>

<%@ Register Namespace="Superexpert.Controls" TagPrefix="super" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="kontroli">
        <div id="button01">
            <asp:Button ID="btnNew" runat="server" ToolTip="��� �����" OnClick="btnNew_Click"
                CausesValidation="false" UseSubmitBehavior="false" CssClass="novZapis" BorderWidth="0px" />
        </div>
        <div id="button02">
            <asp:Button ID="btnEdit" runat="server" ToolTip="������" OnClick="btnEdit_Click"
                CausesValidation="false" Enabled="false" UseSubmitBehavior="false" CssClass="izmeni"
                BorderWidth="0px" />
        </div>
        <%--<div id="button03">
            <asp:Button ID="btnDelete" runat="server" OnClick="btnDelete_Click" CausesValidation="false"
                Enabled="false" UseSubmitBehavior="false" CssClass="izbrisi" BorderWidth="0px" />
        </div>--%>
        <div id="button03">
            <asp:Button ID="btnPreview" runat="server" ToolTip="������" OnClick="btnPreview_Click"
                CausesValidation="false" UseSubmitBehavior="false" CssClass="osvezi" BorderWidth="0px" />
        </div>
        <div id="button04">
            <asp:Button ID="btnReport" runat="server" ToolTip="�������" CausesValidation="false"
                UseSubmitBehavior="false" OnClick="btnReport_Click" CssClass="izvestaj" BorderWidth="0px" />
        </div>
        <div id="button05">
            <asp:Button ID="btnSearch" runat="server" ToolTip="�������" OnClick="btnSearch_Click"
                CausesValidation="false" UseSubmitBehavior="false" CssClass="prebaraj" BorderWidth="0px" />
        </div>
        <div id="button06">
            <asp:Button ID="btnOrderItems" runat="server" ToolTip="������" OnClick="btnOrderItems_Click"
                CausesValidation="false" UseSubmitBehavior="false" CssClass="stavki" BorderWidth="0px"
                Enabled="false" />
        </div>
        <div id="button07">
            <asp:Button ID="btnAttachments" runat="server" ToolTip="���������" OnClick="btnAttachments_Click"
                Enabled="false" CausesValidation="false" UseSubmitBehavior="false" CssClass="dokumenti"
                BorderWidth="0px" />
        </div>
    </div>
    <asp:MultiView ID="mvMain" runat="server">
        <asp:View ID="viewGrid" runat="server">
            <div id="tabeliFrame">
                <div id="header">
                    <div id="content">
                        ������
                    </div>
                </div>
                <div id="contentOuter">
                    <div id="contentInner">
                        <cc1:GXGridView ID="GXGridView1" runat="server" DataSourceID="odsGridView" DataKeyNames="ID"
                            Width="100%" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"
                            Caption="������" EmptyDataText="���� ������ ��� �� ������������ ����������� �� �����������!"
                            OnRowCommand="GXGridView1_RowCommand" RowStyle-CssClass="row" CssClass="grid"
                            GridLines="None">
                            <Columns>
                                <asp:BoundField HeaderText="��� �� �����" DataField="OrderNumber" SortExpression="OrderNumber" />
                                <asp:BoundField HeaderText="���� �� �����" DataField="OrderDate" SortExpression="OrderDate"
                                    DataFormatString="{0:d}" />
                                <asp:BoundField HeaderText="����� ����" DataField="FinishDate" SortExpression="FinishDate"
                                    DataFormatString="{0:d}" />
                                <%--<asp:BoundField HeaderText="��. �����������" DataField="ClientEMBG" SortExpression="ClientEMBG" />--%>
                                <asp:BoundField HeaderText="�����������" DataField="ClientName" SortExpression="ClientName" />
                                <%--<asp:BoundField HeaderText="��. ����������" DataField="OwnerEMBG" SortExpression="OwnerEMBG" />--%>
                                <asp:BoundField HeaderText="����������" DataField="OwnerName" SortExpression="OwnerName" />
                                <asp:BoundField HeaderText="�����" DataField="UserName" SortExpression="UserName" />
                            </Columns>
                            <PagerStyle HorizontalAlign="Center" />
                            <SelectedRowStyle CssClass="rowSelected" />
                            <PagerSettings FirstPageText="<< ���� " PreviousPageText="< ��������� " NextPageText=" ������ >"
                                LastPageText=" �������� >>" Mode="NextPreviousFirstLast" />
                        </cc1:GXGridView>
                        <cc1:GridViewDataSource ID="odsGridView" runat="server" TypeName="Broker.DataAccess.OrdersView"
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
                                        <cc1:FilterItem FieldName="��� �� �����" PropertyName="OrderNumber" Comparator="StringComparators"
                                            DataType="String" />
                                        <cc1:FilterItem FieldName="���� �� �����" PropertyName="OrderDate" Comparator="NumericComparators"
                                            DataType="DateTime" />
                                        <cc1:FilterItem FieldName="��. �����������" PropertyName="ClientEMBG" Comparator="StringComparators"
                                            DataType="String" />
                                        <cc1:FilterItem FieldName="�����������" PropertyName="ClientName" Comparator="StringComparators"
                                            DataType="String" />
                                        <cc1:FilterItem FieldName="��. ����������" PropertyName="OwnerEMBG" Comparator="StringComparators"
                                            DataType="String" />
                                        <cc1:FilterItem FieldName="����������" PropertyName="OwnerName" Comparator="StringComparators"
                                            DataType="String" />
                                        <cc1:FilterItem FieldName="����� ����" PropertyName="FinishDate" Comparator="NumericComparators"
                                            DataType="DateTime" />
                                        <cc1:FilterItem FieldName="�����" PropertyName="UserName" Comparator="StringComparators"
                                            DataType="String" />
                                    </cc1:FilterControl>
                                </div>
                                <cc1:FilterDataSource ID="odsFilterGridView" runat="server" TypeName="Broker.DataAccess.OrdersView">
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
                    GridLines="None" OnDataBinding="DetailsView1_DataBinding">
                    <Fields>
                        <asp:TemplateField>
                            <InsertItemTemplate>
                                <div class="subTitles">
                                    ��� ����� �� ������
                                </div>
                            </InsertItemTemplate>
                            <EditItemTemplate>
                                <div class="subTitles">
                                    ������ �� ����� �� ������
                                </div>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <div class="subTitles">
                                    ������ �� ����� �� ������
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="��� �� �����">
                            <InsertItemTemplate>
                                <asp:TextBox ID="tbOrderNumber" runat="server" Text='<%# Bind("OrderNumber") %>'
                                    MaxLength="20" ReadOnly="true"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvOrderNumber" runat="server" ControlToValidate="tbOrderNumber"
                                    Display="Dynamic" ErrorMessage="*">
                                </asp:RequiredFieldValidator>
                                <%--<asp:Button ID="btnSearchEMBG" runat="server" Text="..." OnClick="btnSearchEMBG_Click" CausesValidation="false" />--%>
                                <super:EntityCallOutValidator ID="NumberInsertValidator" PropertyName="OrderNumberValidator"
                                    runat="server" />
                            </InsertItemTemplate>
                            <ItemTemplate>
                                <asp:TextBox ID="tbOrderNumber" runat="server" Text='<%# Bind("OrderNumber") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="tbOrderNumber" runat="server" Text='<%# Bind("OrderNumber") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="���� �� �����" InsertVisible="false">
                            <EditItemTemplate>
                                <asp:TextBox runat="server" ID="tbStartDate" Text='<%#Bind("OrderDate") %>' ReadOnly="true"></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="tbStartDate" Text='<%#Bind("OrderDate") %>' ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="��������� �� ��������" InsertVisible="false">
                            <EditItemTemplate>
                                <asp:TextBox runat="server" ID="tbUserName" Text='<%#Eval("User.UserName") %>' ReadOnly="true"></asp:TextBox>
                                <asp:TextBox runat="server" ID="tbUserID" Text='<%#Bind("UserID") %>' ReadOnly="true"
                                    Visible="true"></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="tbUserName" Text='<%#Eval("User.UserName") %>' ReadOnly="true"></asp:TextBox>
                                <asp:TextBox runat="server" ID="tbUserID" Text='<%#Bind("UserID") %>' ReadOnly="true"
                                    Visible="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="��. �����������">
                            <ItemTemplate>
                                <asp:TextBox ID="tbClientEMBG" runat="server" Text='<%# Eval("Client.EMBG") %>' ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="tbClientEMBG" runat="server" Text='<%# Eval("Client.EMBG") %>' MaxLength="100"
                                    ReadOnly="true"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvClientEMBG" runat="server" ControlToValidate="tbClientName"
                                    ErrorMessage="*"></asp:RequiredFieldValidator>
                                <asp:TextBox ID="tbClientID" runat="server" Text='<%# Bind("ClientID") %>' Visible="false"
                                    ReadOnly="true"></asp:TextBox>
                            </EditItemTemplate>
                            <InsertItemTemplate>
                                <asp:TextBox ID="tbClientEMBG" runat="server" Text='<%# Eval("ClientEMBG") %>' MaxLength="13"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvClientEMBG" runat="server" ControlToValidate="tbClientEMBG"
                                    Display="Dynamic" ErrorMessage="*">
                                </asp:RequiredFieldValidator>
                                <asp:Button ID="btnSearchClientEMBG" runat="server" Text="..." OnClick="btnSearchClientEMBG_Click"
                                    CausesValidation="false" />
                                <asp:Panel ID="pnlClient" runat="server" Visible="false">
                                    <asp:DetailsView ID="ClientDetailsView" runat="server" DataSourceID="ClientdvDataSource"
                                        DataKeyNames="ID" AutoGenerateRows="False" OnItemCommand="ClientDetailsView_ItemCommand"
                                        OnItemDeleted="ClientDetailsView_ItemDeleted" OnItemInserted="ClientDetailsView_ItemInserted"
                                        OnItemUpdated="ClientDetailsView_ItemUpdated" OnModeChanging="ClientDetailsView_ModeChanging"
                                        OnItemInserting="ClientDetailsView_ItemInserting" GridLines="None" DefaultMode="Insert">
                                        <Fields>
                                            <asp:TemplateField>
                                                <InsertItemTemplate>
                                                    <div class="subTitles">
                                                        ��� ����� �� �������
                                                    </div>
                                                </InsertItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="��� � �������">
                                                <InsertItemTemplate>
                                                    <asp:TextBox ID="tbName" runat="server" Text='<%# Bind("Name") %>' MaxLength="100"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="tbName"
                                                        Display="Dynamic" ErrorMessage="*">
                                                    </asp:RequiredFieldValidator>
                                                    <super:EntityCallOutValidator ID="NameInsertValidator" PropertyName="NAME" runat="server" />
                                                </InsertItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="������� ���">
                                                <InsertItemTemplate>
                                                    <asp:TextBox ID="tbEMBG" runat="server" Text='<%# Bind("EMBG") %>' MaxLength="100"
                                                        ReadOnly="true"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvEMBG" runat="server" ControlToValidate="tbEMBG"
                                                        ErrorMessage="*"></asp:RequiredFieldValidator>
                                                    <super:EntityCallOutValidator ID="EmbgInsertValidator" PropertyName="EMBG" runat="server" />
                                                </InsertItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="������">
                                                <InsertItemTemplate>
                                                    <asp:TextBox ID="tbAddress" MaxLength="100" runat="server" Text='<%# Bind("Address") %>'></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvAddress" runat="server" ControlToValidate="tbAddress"
                                                        ErrorMessage="*"></asp:RequiredFieldValidator>
                                                    <super:EntityCallOutValidator ID="AddressInsertValidator" PropertyName="ADDRESS"
                                                        runat="server" />
                                                </InsertItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="�������� �����">
                                                <InsertItemTemplate>
                                                    <asp:DropDownList ID="ddlPlaces" runat="server" DataSourceID="odsActivePlaces" DataTextField="Name"
                                                        DataValueField="ID" SelectedValue='<%# Bind("PlaceID") %>'>
                                                    </asp:DropDownList>
                                                    <asp:ObjectDataSource ID="odsActivePlaces" runat="server" SelectMethod="GetActivePlaces"
                                                        TypeName="Broker.DataAccess.Place"></asp:ObjectDataSource>
                                                </InsertItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="�������">
                                                <InsertItemTemplate>
                                                    <asp:TextBox ID="tbPhone" MaxLength="30" runat="server" Text='<%# Bind("Phone") %>'></asp:TextBox>
                                                </InsertItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="������� ���.">
                                                <InsertItemTemplate>
                                                    <asp:TextBox ID="tbMobile" MaxLength="30" runat="server" Text='<%# Bind("Mobile") %>'></asp:TextBox>
                                                </InsertItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="����">
                                                <InsertItemTemplate>
                                                    <asp:TextBox ID="tbFax" MaxLength="30" runat="server" Text='<%# Bind("Fax") %>'></asp:TextBox>
                                                </InsertItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Email">
                                                <InsertItemTemplate>
                                                    <asp:TextBox ID="tbEmail" MaxLength="100" runat="server" Text='<%# Bind("Email") %>'></asp:TextBox>
                                                </InsertItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="������ ����">
                                                <InsertItemTemplate>
                                                    <asp:CheckBox ID="cbIsLaw" runat="server" Checked='<%#Bind("IsLaw") %>' />
                                                </InsertItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <InsertItemTemplate>
                                                    <asp:Button ID="btnCancel" runat="server" CommandName="Cancel" Text="������" CausesValidation="false" />
                                                    <asp:Button ID="btnInsert" runat="server" CommandName="Insert" Text="�����" CausesValidation="false" />
                                                </InsertItemTemplate>
                                            </asp:TemplateField>
                                        </Fields>
                                    </asp:DetailsView>
                                    <cc1:DetailsViewDataSource ID="ClientdvDataSource" runat="server" TypeName="Broker.DataAccess.Client"
                                        ConflictDetection="CompareAllValues" DataObjectTypeName="Broker.DataAccess.Client"
                                        DeleteMethod="Delete" InsertMethod="Insert" SelectMethod="GetClientByOrderID"
                                        UpdateMethod="Update" OnUpdating="ClientdvDataSource_Updating" OnUpdated="ClientdvDataSource_Updated"
                                        OnInserted="ClientdvDataSource_Inserted" OnInserting="ClientdvDataSource_Inserting">
                                        <InsertParameters>
                                            <asp:Parameter Name="entityToInsert" Type="Object" />
                                        </InsertParameters>
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="GXGridView1" Name="orderID" PropertyName="SelectedValue"
                                                Type="Int32" />
                                        </SelectParameters>
                                    </cc1:DetailsViewDataSource>
                                </asp:Panel>
                            </InsertItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="�����������">
                            <ItemTemplate>
                                <asp:TextBox ID="tbClientName" runat="server" Text='<%# Eval("Client.Name") %>' ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="tbClientName" runat="server" Text='<%# Eval("Client.Name") %>' MaxLength="100"
                                    ReadOnly="true"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvClientName" runat="server" ControlToValidate="tbClientName"
                                    ErrorMessage="*"></asp:RequiredFieldValidator>
                                <asp:TextBox ID="tbOwnerID" runat="server" Text='<%# Bind("OwnerID") %>' Visible="false"
                                    ReadOnly="true"></asp:TextBox>
                            </EditItemTemplate>
                            <InsertItemTemplate>
                                <asp:TextBox ID="tbClientName" runat="server" Text='<%# Eval("ClientName") %>' MaxLength="100"
                                    ReadOnly="true"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvClientName" runat="server" ControlToValidate="tbClientName"
                                    ErrorMessage="*"></asp:RequiredFieldValidator>
                            </InsertItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="��. ����������">
                            <ItemTemplate>
                                <asp:TextBox ID="tbOwnerEMBG" runat="server" Text='<%# Eval("Client1.EMBG") %>' ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="tbOwnerEMBG" runat="server" Text='<%# Eval("Client1.EMBG") %>' MaxLength="100"
                                    ReadOnly="true"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvOwnerEMBG" runat="server" ControlToValidate="tbOwnerName"
                                    ErrorMessage="*"></asp:RequiredFieldValidator>
                            </EditItemTemplate>
                            <InsertItemTemplate>
                                <asp:TextBox ID="tbOwnerEMBG" runat="server" Text='<%# Eval("OwnerEMBG") %>' MaxLength="13"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvOwnerEMBG" runat="server" ControlToValidate="tbOwnerEMBG"
                                    Display="Dynamic" ErrorMessage="*">
                                </asp:RequiredFieldValidator>
                                <asp:Button ID="btnSearchOwnerEMBG" runat="server" Text="..." OnClick="btnSearchOwnerEMBG_Click"
                                    CausesValidation="false" />
                                <asp:Panel ID="pnlOwner" runat="server" Visible="false">
                                    <asp:DetailsView ID="OwnerDetailsView" runat="server" DataSourceID="OwnerdvDataSource"
                                        DataKeyNames="ID" AutoGenerateRows="False" OnItemCommand="OwnerDetailsView_ItemCommand"
                                        OnItemDeleted="OwnerDetailsView_ItemDeleted" OnItemInserted="OwnerDetailsView_ItemInserted"
                                        OnItemUpdated="OwnerDetailsView_ItemUpdated" OnModeChanging="OwnerDetailsView_ModeChanging"
                                        OnItemInserting="OwnerDetailsView_ItemInserting" GridLines="None" DefaultMode="Insert">
                                        <Fields>
                                            <asp:TemplateField>
                                                <InsertItemTemplate>
                                                    <div class="subTitles">
                                                        ��� ����� �� �������
                                                    </div>
                                                </InsertItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="��� � �������">
                                                <InsertItemTemplate>
                                                    <asp:TextBox ID="tbName" runat="server" Text='<%# Bind("Name") %>' MaxLength="100"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="tbName"
                                                        Display="Dynamic" ErrorMessage="*">
                                                    </asp:RequiredFieldValidator>
                                                    <super:EntityCallOutValidator ID="NameInsertValidator" PropertyName="OwnerNAME" runat="server" />
                                                </InsertItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="������� ���">
                                                <InsertItemTemplate>
                                                    <asp:TextBox ID="tbEMBG" runat="server" Text='<%# Bind("EMBG") %>' MaxLength="100"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvEMBG" runat="server" ControlToValidate="tbEMBG"
                                                        ErrorMessage="*"></asp:RequiredFieldValidator>
                                                    <super:EntityCallOutValidator ID="EmbgOwnerInsertValidator" PropertyName="OwnerEMBG"
                                                        runat="server" />
                                                </InsertItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="������">
                                                <InsertItemTemplate>
                                                    <asp:TextBox ID="tbAddress" MaxLength="100" runat="server" Text='<%# Bind("Address") %>'></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvAddress" runat="server" ControlToValidate="tbAddress"
                                                        ErrorMessage="*"></asp:RequiredFieldValidator>
                                                    <super:EntityCallOutValidator ID="AddressInsertValidator" PropertyName="OwnerADDRESS"
                                                        runat="server" />
                                                </InsertItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="�������� �����">
                                                <InsertItemTemplate>
                                                    <asp:DropDownList ID="ddlPlaces" runat="server" DataSourceID="odsActivePlaces" DataTextField="Name"
                                                        DataValueField="ID" SelectedValue='<%# Bind("PlaceID") %>'>
                                                    </asp:DropDownList>
                                                    <asp:ObjectDataSource ID="odsActivePlaces" runat="server" SelectMethod="GetActivePlaces"
                                                        TypeName="Broker.DataAccess.Place"></asp:ObjectDataSource>
                                                </InsertItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="�������">
                                                <InsertItemTemplate>
                                                    <asp:TextBox ID="tbPhone" MaxLength="30" runat="server" Text='<%# Bind("Phone") %>'></asp:TextBox>
                                                </InsertItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="������� ���.">
                                                <InsertItemTemplate>
                                                    <asp:TextBox ID="tbMobile" MaxLength="30" runat="server" Text='<%# Bind("Mobile") %>'></asp:TextBox>
                                                </InsertItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="����">
                                                <InsertItemTemplate>
                                                    <asp:TextBox ID="tbFax" MaxLength="30" runat="server" Text='<%# Bind("Fax") %>'></asp:TextBox>
                                                </InsertItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Email">
                                                <InsertItemTemplate>
                                                    <asp:TextBox ID="tbEmail" MaxLength="100" runat="server" Text='<%# Bind("Email") %>'></asp:TextBox>
                                                </InsertItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="������ ����">
                                                <InsertItemTemplate>
                                                    <asp:CheckBox ID="cbIsLaw" runat="server" Checked='<%#Bind("IsLaw") %>' />
                                                </InsertItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <InsertItemTemplate>
                                                    <asp:Button ID="btnCancel" runat="server" CommandName="Cancel" Text="������" CausesValidation="false" />
                                                    <asp:Button ID="btnInsert" runat="server" CommandName="Insert" Text="�����" CausesValidation="false" />
                                                </InsertItemTemplate>
                                            </asp:TemplateField>
                                        </Fields>
                                    </asp:DetailsView>
                                    <cc1:DetailsViewDataSource ID="OwnerdvDataSource" runat="server" TypeName="Broker.DataAccess.Client"
                                        ConflictDetection="CompareAllValues" DataObjectTypeName="Broker.DataAccess.Client"
                                        DeleteMethod="Delete" InsertMethod="Insert" SelectMethod="GetOwnerByOrderID"
                                        UpdateMethod="Update" OnUpdating="OwnerdvDataSource_Updating" OnUpdated="OwnerdvDataSource_Updated"
                                        OnInserted="OwnerdvDataSource_Inserted" OnInserting="OwnerdvDataSource_Inserting">
                                        <InsertParameters>
                                            <asp:Parameter Name="entityToInsert" Type="Object" />
                                        </InsertParameters>
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="GXGridView1" Name="orderID" PropertyName="SelectedValue"
                                                Type="Int32" />
                                        </SelectParameters>
                                    </cc1:DetailsViewDataSource>
                                </asp:Panel>
                            </InsertItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="����������">
                            <ItemTemplate>
                                <asp:TextBox ID="tbOwnerName" runat="server" Text='<%# Eval("Client1.Name") %>' ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="tbOwnerName" runat="server" Text='<%# Eval("Client1.Name") %>' MaxLength="100"
                                    ReadOnly="true"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvOwnerName" runat="server" ControlToValidate="tbOwnerName"
                                    ErrorMessage="*"></asp:RequiredFieldValidator>
                            </EditItemTemplate>
                            <InsertItemTemplate>
                                <asp:TextBox ID="tbOwnerName" runat="server" Text='<%# Eval("OwnerName") %>' MaxLength="100"
                                    ReadOnly="true"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvOwnerName" runat="server" ControlToValidate="tbOwnerName"
                                    ErrorMessage="*"></asp:RequiredFieldValidator>
                            </InsertItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="������ �� ��������">
                            <ItemTemplate>
                                <asp:DropDownList ID="ddlDocumentSubTypes" runat="server" Enabled="False" DataSourceID="odsDocumentSubTypes"
                                    DataTextField="Description" DataValueField="ID" SelectedValue='<%# Bind("DocumentSubTypeID") %>'>
                                </asp:DropDownList>
                                <asp:ObjectDataSource ID="odsDocumentSubTypes" runat="server" SelectMethod="Select"
                                    TypeName="Broker.DataAccess.DocumentSubType"></asp:ObjectDataSource>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddlDocumentSubTypes" runat="server" DataSourceID="odsDocumentSubTypes"
                                    DataTextField="Description" DataValueField="ID" SelectedValue='<%# Bind("DocumentSubTypeID") %>'
                                    Enabled="false">
                                </asp:DropDownList>
                                <asp:ObjectDataSource ID="odsDocumentSubTypes" runat="server" SelectMethod="Select"
                                    TypeName="Broker.DataAccess.DocumentSubType"></asp:ObjectDataSource>
                            </EditItemTemplate>
                            <InsertItemTemplate>
                                <asp:DropDownList ID="ddlDocumentSubTypes" runat="server" DataSourceID="odsDocumentSubTypes"
                                    DataTextField="Description" DataValueField="ID" SelectedValue='<%# Bind("DocumentSubTypeID") %>'>
                                </asp:DropDownList>
                                <asp:ObjectDataSource ID="odsDocumentSubTypes" runat="server" SelectMethod="Select"
                                    TypeName="Broker.DataAccess.DocumentSubType"></asp:ObjectDataSource>
                            </InsertItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="�� ��������">
                            <ItemTemplate>
                                <asp:DropDownList ID="ddlToUsers" runat="server" Enabled="False" DataSourceID="odsToUsers"
                                    DataTextField="UserName" DataValueField="ID" SelectedValue='<%# Bind("ToUserID") %>'>
                                </asp:DropDownList>
                                <asp:ObjectDataSource ID="odsToUsers" runat="server" SelectMethod="GetUsersForOrder"
                                    TypeName="Broker.DataAccess.User">
                                    <SelectParameters>
                                        <asp:SessionParameter SessionField="UserID" Name="userID" Type="Int32" />
                                    </SelectParameters>
                                </asp:ObjectDataSource>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddlToUsers" runat="server" DataSourceID="odsToUsers" DataTextField="UserName"
                                    DataValueField="ID" SelectedValue='<%# Bind("ToUserID") %>'>
                                </asp:DropDownList>
                                <asp:ObjectDataSource ID="odsToUsers" runat="server" SelectMethod="GetUsersForOrder"
                                    TypeName="Broker.DataAccess.User">
                                    <SelectParameters>
                                        <asp:SessionParameter SessionField="UserID" Name="userID" Type="Int32" />
                                    </SelectParameters>
                                </asp:ObjectDataSource>
                            </EditItemTemplate>
                            <InsertItemTemplate>
                                <asp:DropDownList ID="ddlToUsers" runat="server" DataSourceID="odsToUsers" DataTextField="UserName"
                                    DataValueField="ID" SelectedValue='<%# Bind("ToUserID") %>'>
                                </asp:DropDownList>
                                <asp:ObjectDataSource ID="odsToUsers" runat="server" SelectMethod="GetUsersForOrder"
                                    TypeName="Broker.DataAccess.User">
                                    <SelectParameters>
                                        <asp:SessionParameter SessionField="UserID" Name="userID" Type="Int32" />
                                    </SelectParameters>
                                </asp:ObjectDataSource>
                            </InsertItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="����� ���">
                            <ItemTemplate>
                                <asp:TextBox ID="tbFinishDate" runat="server" Text='<%#Bind("FinishDate","{0:d}") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                            <InsertItemTemplate>
                                <asp:TextBox ID="tbFinishDate" runat="server" Text='<%#Bind("FinishDate","{0:d}") %>'></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvFinishDate" runat="server" ControlToValidate="tbFinishDate"
                                    ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="cvFinishDate" runat="server" ControlToValidate="tbFinishDate"
                                    ErrorMessage="*" Display="Dynamic" Operator="DataTypeCheck" Type="Date"></asp:CompareValidator>
                            </InsertItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="tbFinishDate" runat="server" Text='<%#Bind("FinishDate","{0:d}") %>'></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvFinishDate" runat="server" ControlToValidate="tbFinishDate"
                                    ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="cvFinishDate" runat="server" ControlToValidate="tbFinishDate"
                                    ErrorMessage="*" Display="Dynamic" Operator="DataTypeCheck" Type="Date"></asp:CompareValidator>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Button ID="btnCancel" runat="server" CommandName="Cancel" Text="������" CausesValidation="false" />
                                <asp:Button ID="btnDelete" runat="server" Text="�������" OnClientClick="return confirm('���� ��� �������?')"
                                    OnClick="btnDelete_Click1" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Button ID="btnCancel" runat="server" CommandName="Cancel" Text="������" CausesValidation="false" />
                                <asp:Button ID="btnUpdate" runat="server" CommandName="Update" Text="������" />
                            </EditItemTemplate>
                            <InsertItemTemplate>
                                <asp:Button ID="btnCancel" runat="server" CommandName="Cancel" Text="������" CausesValidation="false" />
                                <asp:Button ID="btnInsert" runat="server" CommandName="Insert" Text="�����" />
                            </InsertItemTemplate>
                        </asp:TemplateField>
                    </Fields>
                </asp:DetailsView>
                <cc1:DetailsViewDataSource ID="dvDataSource" runat="server" TypeName="Broker.DataAccess.Order"
                    ConflictDetection="CompareAllValues" DataObjectTypeName="Broker.DataAccess.Order"
                    DeleteMethod="Delete" InsertMethod="Insert" SelectMethod="GetOrder" UpdateMethod="Update"
                    OnUpdating="dvDataSource_Updating" OnUpdated="dvDataSource_Updated" OnInserted="dvDataSource_Inserted"
                    OnInserting="dvDataSource_Inserting" OldValuesParameterFormatString="oldEntity"
                    OnSelecting="dvDataSource_Selecting">
                    <UpdateParameters>
                        <asp:Parameter Name="oldEntity" Type="Object" />
                        <asp:Parameter Name="newEntity" Type="Object" />
                        <asp:Parameter Name="FinishDate" Type="DateTime" />
                        <asp:Parameter Name="OrderDate" Type="DateTime" />
                        <asp:Parameter Name="UserID" Type="Int32" />
                        <asp:Parameter Name="ToUserID" Type="Int32" />
                        <asp:Parameter Name="ClientID" Type="Int32" />
                        <asp:Parameter Name="OwnerID" Type="Int32" />
                    </UpdateParameters>
                    <InsertParameters>
                        <asp:Parameter Name="FinishDate" Type="DateTime" />
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
                    <cc1:SearchItem FieldName="��� �� �����" PropertyName="OrderNumber" Comparator="StringComparators"
                        DataType="String" />
                    <cc1:SearchItem FieldName="���� �� �����" PropertyName="OrderDate" Comparator="NumericComparators"
                        DataType="DateTime" />
                    <cc1:SearchItem FieldName="��. �����������" PropertyName="ClientEMBG" Comparator="StringComparators"
                        DataType="String" />
                    <cc1:SearchItem FieldName="�����������" PropertyName="ClientName" Comparator="StringComparators"
                        DataType="String" />
                    <cc1:SearchItem FieldName="��. ����������" PropertyName="OwnerEMBG" Comparator="StringComparators"
                        DataType="String" />
                    <cc1:SearchItem FieldName="����������" PropertyName="OwnerName" Comparator="StringComparators"
                        DataType="String" />
                    <cc1:SearchItem FieldName="����� ����" PropertyName="FinishDate" Comparator="NumericComparators"
                        DataType="DateTime" />
                    <cc1:SearchItem FieldName="�����" PropertyName="UserName" Comparator="StringComparators"
                        DataType="String" />
                </cc1:SearchControl>
                <cc1:GridViewDataSource ID="odsSearch" runat="server" TypeName="Broker.DataAccess.OrdersView"
                    SelectCountMethod="SelectSearchCountCached" SelectMethod="SelectSearch">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="SearchControl1" PropertyName="SearchArguments" Name="sArgument" />
                    </SelectParameters>
                </cc1:GridViewDataSource>
            </div>
        </asp:View>
        <asp:View ID="viewReport" runat="server">
            <div class="paddingKontroli">
                <cc1:ReportControl ID="reportControl" runat="server" GridViewID="GXGridView1" SearchControlID="SearchControl1"
                    TypeName="Broker.DataAccess.OrdersView">
                    <cc1:PrintItem HeaderText="��� �� �����" PropertyName="OrderNumber" />
                    <cc1:PrintItem HeaderText="���� �� �����" PropertyName="OrderDate" />
                    <cc1:PrintItem HeaderText="��. �����������" PropertyName="ClientEMBG" />
                    <cc1:PrintItem HeaderText="�����������" PropertyName="ClientName" />
                    <cc1:PrintItem HeaderText="��. ����������" PropertyName="OwnerEMBG" />
                    <cc1:PrintItem HeaderText="����������" PropertyName="OwnerName" />
                    <cc1:PrintItem HeaderText="����� ����" PropertyName="FinishDate" />
                    <cc1:PrintItem HeaderText="�����" PropertyName="UserName" />
                </cc1:ReportControl>
            </div>
        </asp:View>
        <asp:View ID="viewOrderItems" runat="server">
            <div class="paddingKontroli">
                <asp:DetailsView ID="dvOrderPreview" runat="server" DataSourceID="odsOrderPreview"
                    DataKeyNames="ID" AutoGenerateRows="False" OnItemCommand="dvOrderPreview_ItemCommand"
                    OnModeChanging="dvOrderPreview_ModeChanging" GridLines="None" DefaultMode="ReadOnly">
                    <Fields>
                        <asp:TemplateField HeaderText="��� �� �����">
                            <ItemTemplate>
                                <asp:TextBox ID="tbOrderNumber" runat="server" Text='<%# Bind("OrderNumber") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="���� �� �����">
                            <ItemTemplate>
                                <asp:TextBox ID="tbOrderDate" runat="server" Text='<%# Bind("OrderDate", "{0:d}") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="����� ����">
                            <ItemTemplate>
                                <asp:TextBox ID="tbFinishDate" runat="server" Text='<%# Bind("FinishDate", "{0:d}") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="�����������">
                            <ItemTemplate>
                                <asp:TextBox ID="tbClientEMBG" runat="server" Text='<%# Eval("Client.EMBG") %>' ReadOnly="true"></asp:TextBox>
                                <asp:TextBox ID="tbClientName" Width="250px" runat="server" Text='<%# Eval("Client.Name") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="����������">
                            <ItemTemplate>
                                <asp:TextBox ID="tbOwnerEMBG" runat="server" Text='<%# Eval("Client1.EMBG") %>' ReadOnly="true"></asp:TextBox>
                                <asp:TextBox ID="tbOwnerName" Width="250px" runat="server" Text='<%# Eval("Client1.Name") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="������ �� ��������">
                            <ItemTemplate>
                                <asp:DropDownList ID="ddlDocumentSubTypes" runat="server" Enabled="False" DataSourceID="odsDocumentSubTypes"
                                    DataTextField="Description" Width="250px" DataValueField="ID" SelectedValue='<%# Bind("DocumentSubTypeID") %>'>
                                </asp:DropDownList>
                                <asp:ObjectDataSource ID="odsDocumentSubTypes" runat="server" SelectMethod="Select"
                                    TypeName="Broker.DataAccess.DocumentSubType"></asp:ObjectDataSource>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Fields>
                </asp:DetailsView>
                <cc1:DetailsViewDataSource ID="odsOrderPreview" runat="server" TypeName="Broker.DataAccess.Order"
                    ConflictDetection="CompareAllValues" DataObjectTypeName="Broker.DataAccess.Order"
                    SelectMethod="Get" OldValuesParameterFormatString="oldEntity" OnSelecting="dvDataSource_Selecting">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="GXGridView1" Name="id" PropertyName="SelectedValue"
                            Type="Int32" />
                    </SelectParameters>
                </cc1:DetailsViewDataSource>
                <asp:MultiView ID="mvOrderItems" runat="server">
                    <asp:View ID="viewOrderItemsGrid" runat="server">
                        <asp:GridView ID="GridViewOrderItems" runat="server" DataSourceID="odsGridViewOrderItems"
                            DataKeyNames="ID" Width="100%" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"
                            Caption="������ �� �����" EmptyDataText="���� ������ ��� �� ������������ ����������� �� �����������!"
                            RowStyle-CssClass="row" CssClass="grid" GridLines="None" OnRowDeleting="GridViewOrderItems_RowDeleting"
                            OnRowDeleted="GridViewOrderItems_RowDeleted">
                            <RowStyle CssClass="row"></RowStyle>
                            <Columns>
                                <asp:TemplateField Visible="false">
                                    <ItemTemplate>
                                        <asp:TextBox ID="tbOrderItemID" runat="server" Text='<%#Bind("ID") %>' Visible="false"></asp:TextBox>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="tbOrderItemID" runat="server" Text='<%#Bind("ID") %>' Visible="false"></asp:TextBox>
                                    </EditItemTemplate>
                                    <InsertItemTemplate>
                                        <asp:TextBox ID="tbOrderItemID" runat="server" Text='<%#Bind("ID") %>' Visible="false"></asp:TextBox>
                                    </InsertItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <InsertItemTemplate>
                                        <asp:Label ID="tbOrderNumberID" runat="server" Text='<%#Bind("OrderID") %>' Visible="false"></asp:Label>
                                    </InsertItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="tbOrderNumberID" runat="server" Text='<%#Bind("OrderID") %>' Visible="false"></asp:Label>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="tbOrderNumberID" runat="server" Text='<%#Bind("OrderID") %>' Visible="false"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="����� ���">
                                    <InsertItemTemplate>
                                        <asp:Label ID="tbOrdinalNumber" runat="server" Text='<%#Bind("OrdinalNumber") %>'></asp:Label>
                                    </InsertItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="tbOrdinalNumber" runat="server" Text='<%#Bind("OrdinalNumber") %>'></asp:Label>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="tbOrdinalNumber" runat="server" Text='<%#Bind("OrdinalNumber") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="��� �� �����������">
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
                                            Width="200px" ReadOnly="true"></asp:Label>
                                        <asp:Label ID="tbInsuranceSubTypeID" runat="server" Text='<%#Bind("InsuranceSubTypeID") %>'
                                            Visible="false"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="��� �� �������">
                                    <InsertItemTemplate>
                                        <asp:DropDownList ID="ddlPaymentTypes" runat="server" DataSourceID="odsPaymentTypes"
                                            Width="100px" DataTextField="Name" DataValueField="ID" SelectedValue='<%# Bind("PaymentTypeID") %>'>
                                        </asp:DropDownList>
                                        <asp:ObjectDataSource ID="odsPaymentTypes" runat="server" SelectMethod="Select" TypeName="Broker.DataAccess.PaymentType">
                                        </asp:ObjectDataSource>
                                    </InsertItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlPaymentTypes" runat="server" DataSourceID="odsPaymentTypes"
                                            Width="100px" DataTextField="Name" DataValueField="ID" SelectedValue='<%# Bind("PaymentTypeID") %>'>
                                        </asp:DropDownList>
                                        <asp:ObjectDataSource ID="odsPaymentTypes" runat="server" SelectMethod="Select" TypeName="Broker.DataAccess.PaymentType">
                                        </asp:ObjectDataSource>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="tbPaymentType" runat="server" Text='<%#Eval("PaymentType.Name") %>'
                                            Width="100px" ReadOnly="true"></asp:Label>
                                        <asp:Label ID="tbPaymentTypeID" runat="server" Text='<%#Bind("PaymentTypeID") %>'
                                            Visible="false"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="����">
                                    <InsertItemTemplate>
                                        <asp:TextBox ID="tbDescription" Width="100px" runat="server" Text='<%#Bind("Description") %>'></asp:TextBox>
                                    </InsertItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="tbDescription" Width="100px" runat="server" Text='<%#Bind("Description") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="tbDescription" Width="100px" runat="server" Text='<%#Bind("Description") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:CommandField CancelText="������" DeleteText="�������" EditText="������" InsertText="�����"
                                    InsertVisible="False" NewText="���" SelectText="������" ShowDeleteButton="True"
                                    ShowEditButton="True" UpdateText="�������" />
                            </Columns>
                            <PagerStyle HorizontalAlign="Center" />
                            <%--<SelectedRowStyle CssClass="rowSelected" />--%>
                            <PagerSettings FirstPageText="<< ���� " PreviousPageText="< ��������� " NextPageText=" ������ >"
                                LastPageText=" �������� >>" Mode="NextPreviousFirstLast" />
                        </asp:GridView>
                        <asp:ObjectDataSource ID="odsGridViewOrderItems" runat="server" TypeName="Broker.DataAccess.OrderItem"
                            OldValuesParameterFormatString="oldEntity" DataObjectTypeName="Broker.DataAccess.OrderItem"
                            ConflictDetection="CompareAllValues" DeleteMethod="Delete" InsertMethod="Insert"
                            SelectMethod="GetByOrderID" UpdateMethod="Update">
                            <UpdateParameters>
                                <asp:Parameter Name="oldEntity" Type="Object" />
                                <asp:Parameter Name="newEntity" Type="Object" />
                            </UpdateParameters>
                            <SelectParameters>
                                <asp:ControlParameter ControlID="GXGridView1" Name="orderID" PropertyName="SelectedValue"
                                    Type="Int32" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                        <div>
                            <asp:Button ID="btnNewOrderItem" runat="server" Text="���� ������" OnClick="btnNewOrderItem_Click" />
                        </div>
                    </asp:View>
                    <asp:View ID="viewOrderItemsEdit" runat="server">
                        <asp:DetailsView ID="DetailsViewOrderItems" runat="server" DataSourceID="dvDataSourceOrderItems"
                            DataKeyNames="ID" AutoGenerateRows="False" OnItemCommand="DetailsViewOrderItems_ItemCommand"
                            OnItemInserted="DetailsViewOrderItems_ItemInserted" OnModeChanging="DetailsViewOrderItems_ModeChanging"
                            OnItemInserting="DetailsViewOrderItems_ItemInserting" GridLines="None" DefaultMode="Insert">
                            <Fields>
                                <asp:TemplateField>
                                    <InsertItemTemplate>
                                        <div class="subTitles">
                                            ���� ������
                                        </div>
                                    </InsertItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <InsertItemTemplate>
                                        <asp:TextBox ID="tbOrderItemID" runat="server" Text='<%#Bind("ID") %>' Visible="false"></asp:TextBox>
                                    </InsertItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="��� �� �����������">
                                    <InsertItemTemplate>
                                        <asp:DropDownList ID="ddlInsuranceSubTypes" runat="server" DataSourceID="odsInsuranceSubTypes"
                                            DataTextField="Description" Width="350px" DataValueField="ID" SelectedValue='<%# Bind("InsuranceSubTypeID") %>'>
                                        </asp:DropDownList>
                                        <asp:ObjectDataSource ID="odsInsuranceSubTypes" runat="server" SelectMethod="GetForOrder"
                                            TypeName="Broker.DataAccess.InsuranceSubType"></asp:ObjectDataSource>
                                    </InsertItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="��� �� �������">
                                    <InsertItemTemplate>
                                        <asp:DropDownList ID="ddlPaymentTypes" runat="server" DataSourceID="odsPaymentTypes"
                                            DataTextField="Name" Width="350px" DataValueField="ID" SelectedValue='<%# Bind("PaymentTypeID") %>'>
                                        </asp:DropDownList>
                                        <asp:ObjectDataSource ID="odsPaymentTypes" runat="server" SelectMethod="Select" TypeName="Broker.DataAccess.PaymentType">
                                        </asp:ObjectDataSource>
                                    </InsertItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="����">
                                    <InsertItemTemplate>
                                        <asp:TextBox ID="tbDescription" runat="server" Text='<%#Bind("Description") %>' MaxLength="100"></asp:TextBox>
                                    </InsertItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <InsertItemTemplate>
                                        <asp:Button ID="btnCancel" runat="server" CommandName="Cancel" Text="������" CausesValidation="false" />
                                        <asp:Button ID="btnInsert" runat="server" CommandName="Insert" Text="�����" />
                                    </InsertItemTemplate>
                                </asp:TemplateField>
                            </Fields>
                        </asp:DetailsView>
                        <cc1:DetailsViewDataSource ID="dvDataSourceOrderItems" runat="server" TypeName="Broker.DataAccess.OrderItem"
                            ConflictDetection="CompareAllValues" DataObjectTypeName="Broker.DataAccess.OrderItem"
                            InsertMethod="Insert" SelectMethod="Get" OnInserted="dvDataSourceOrderItems_Inserted"
                            OnInserting="dvDataSourceOrderItems_Inserting">
                            <InsertParameters>
                                <asp:Parameter Name="entityToInsert" Type="Object" />
                            </InsertParameters>
                            <SelectParameters>
                                <asp:ControlParameter ControlID="GridViewOrderItems" Name="id" PropertyName="SelectedValue"
                                    Type="Int32" />
                            </SelectParameters>
                        </cc1:DetailsViewDataSource>
                    </asp:View>
                </asp:MultiView>
            </div>
        </asp:View>
        <asp:View ID="viewAttachments" runat="server">
            <div class="paddingKontroli">
                <asp:DetailsView ID="DetailsViewOrderForAttachments" runat="server" DataSourceID="odsOrderForAttachments"
                    DataKeyNames="ID" AutoGenerateRows="False" OnItemCommand="DetailsViewOrderForAttachments_ItemCommand"
                    OnModeChanging="DetailsViewOrderForAttachments_ModeChanging" GridLines="None"
                    DefaultMode="ReadOnly">
                    <Fields>
                        <asp:TemplateField HeaderText="��� �� �����">
                            <ItemTemplate>
                                <asp:TextBox ID="tbOrderNumber" runat="server" Text='<%# Bind("OrderNumber") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="���� �� �����">
                            <ItemTemplate>
                                <asp:TextBox ID="tbOrderDate" runat="server" Text='<%# Bind("OrderDate", "{0:d}") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="����� ����">
                            <ItemTemplate>
                                <asp:TextBox ID="tbFinishDate" runat="server" Text='<%# Bind("FinishDate", "{0:d}") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="�����������">
                            <ItemTemplate>
                                <asp:TextBox ID="tbClientEMBG" runat="server" Text='<%# Eval("Client.EMBG") %>' ReadOnly="true"></asp:TextBox>
                                <asp:TextBox ID="tbClientName" Width="250px" runat="server" Text='<%# Eval("Client.Name") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="����������">
                            <ItemTemplate>
                                <asp:TextBox ID="tbOwnerEMBG" runat="server" Text='<%# Eval("Client1.EMBG") %>' ReadOnly="true"></asp:TextBox>
                                <asp:TextBox ID="tbOwnerName" runat="server" Width="250px" Text='<%# Eval("Client1.Name") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="������ �� ��������">
                            <ItemTemplate>
                                <asp:DropDownList ID="ddlDocumentSubTypes" runat="server" Enabled="False" DataSourceID="odsDocumentSubTypes"
                                    DataTextField="Description" Width="250px" DataValueField="ID" SelectedValue='<%# Bind("DocumentSubTypeID") %>'>
                                </asp:DropDownList>
                                <asp:ObjectDataSource ID="odsDocumentSubTypes" runat="server" SelectMethod="Select"
                                    TypeName="Broker.DataAccess.DocumentSubType"></asp:ObjectDataSource>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Fields>
                </asp:DetailsView>
                <cc1:DetailsViewDataSource ID="odsOrderForAttachments" runat="server" TypeName="Broker.DataAccess.Order"
                    ConflictDetection="CompareAllValues" DataObjectTypeName="Broker.DataAccess.Order"
                    SelectMethod="Get" OldValuesParameterFormatString="oldEntity" OnSelecting="odsOrderForAttachments_Selecting">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="GXGridView1" Name="id" PropertyName="SelectedValue"
                            Type="Int32" />
                    </SelectParameters>
                </cc1:DetailsViewDataSource>
                <table>
                    <tr>
                        <td>
                            ��������<asp:FileUpload ID="FileUpload1" runat="server" />
                        </td>
                        <td>
                            <asp:Button ID="btnAddAttachment" runat="server" Text="������" OnClick="btnAddAttachment_Click" />
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
                        EmptyDataText="���� ������!" HeaderStyle-BackColor="#2D3931" HeaderStyle-ForeColor="Wheat"
                        OnSelectedIndexChanged="dataGridFiles_SelectedIndexChanged">
                        <SelectedRowStyle CssClass="rowSelected" />
                        <Columns>
                            <asp:TemplateField HeaderText="��������">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnFile" runat="server" OnClick="btnFile_Click" Text='<%#Bind("Name") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:CommandField ButtonType="Link" SelectText="�������" ShowSelectButton="True"
                                CancelText="������" ShowCancelButton="true" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>
