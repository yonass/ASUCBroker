<%@ Page Title="Типови на документи" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="DocumentTypes.aspx.cs" Inherits="SIMTAdmin_DocumentTypes" %>

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
    </div>
    <asp:MultiView ID="mvMain" runat="server">
        <asp:View ID="viewGrid" runat="server">
            <div id="tabeliFrame">
                <div id="header">
                    <div id="content">
                        Типови на документи
                    </div>
                </div>
                <div id="contentOuter">
                    <div id="contentInner">
                        <cc1:GXGridView ID="GXGridView1" runat="server" DataSourceID="odsGridView" DataKeyNames="ID"
                            Width="100%" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"
                            Caption="Типови на документи" EmptyDataText="Нема записи кои го задоволуваат критериумот на пребарување!"
                            OnRowCommand="GXGridView1_RowCommand" RowStyle-CssClass="row" CssClass="grid"
                            GridLines="None">
                            <Columns>
                                <asp:BoundField HeaderText="Шифра" DataField="Code" SortExpression="Code" />
                                <asp:BoundField HeaderText="Име" DataField="Name" SortExpression="Name" />
                            </Columns>
                            <PagerStyle HorizontalAlign="Center" />
                            <SelectedRowStyle CssClass="rowSelected" />
                            <PagerSettings FirstPageText="<< Прва " PreviousPageText="< Претходна " NextPageText=" Следна >"
                                LastPageText=" Последна >>" Mode="NextPreviousFirstLast" />
                        </cc1:GXGridView>
                        <cc1:GridViewDataSource ID="odsGridView" runat="server" TypeName="Broker.DataAccess.DocumentType"
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
                                    </cc1:FilterControl>
                                </div>
                                <cc1:FilterDataSource ID="odsFilterGridView" runat="server" TypeName="Broker.DataAccess.DocumentType">
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
                                    Нов запис во Типови на документи
                                </div>
                            </InsertItemTemplate>
                            <EditItemTemplate>
                                <div class="subTitles">
                                    Измена на запис во Типови на документи
                                </div>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <div class="subTitles">
                                    Бришење на запис од Типови на документи
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Шифра">
                            <InsertItemTemplate>
                                <asp:TextBox ID="tbCode" runat="server" Text='<%# Bind("Code") %>' MaxLength="10"
                                    ReadOnly="true"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvCode" runat="server" ControlToValidate="tbCode"
                                    Display="Dynamic" ErrorMessage="*">
                                </asp:RequiredFieldValidator>
                                <super:EntityCallOutValidator ID="CodeInsertValidator" PropertyName="DOCUMENTTYPE_CODE_INSERT_EXISTS"
                                    runat="server" />
                            </InsertItemTemplate>
                            <ItemTemplate>
                                <asp:TextBox ID="tbCode" runat="server" Text='<%# Bind("Code") %>' ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="tbCode" runat="server" Text='<%# Bind("Code") %>' MaxLength="10"
                                    ReadOnly="true"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvCode" runat="server" ControlToValidate="tbCode"
                                    ErrorMessage="*"></asp:RequiredFieldValidator>
                                <super:EntityCallOutValidator ID="CodeUpdateValidator" PropertyName="DOCUMENTTYPE_CODE_UPDATE_EXISTS"
                                    runat="server" />
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Име">
                            <ItemTemplate>
                                <asp:TextBox ID="tbName" runat="server" Text='<%# Bind("Name") %>' ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="tbName" runat="server" Text='<%# Bind("Name") %>' MaxLength="50"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="tbName"
                                    ErrorMessage="*"></asp:RequiredFieldValidator>
                            </EditItemTemplate>
                            <InsertItemTemplate>
                                <asp:TextBox ID="tbName" runat="server" Text='<%# Bind("Name") %>' MaxLength="50"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="tbName"
                                    ErrorMessage="*"></asp:RequiredFieldValidator>
                            </InsertItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Button ID="btnCancel" runat="server" CommandName="Cancel" Text="Откажи" CausesValidation="false" />
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
                <cc1:DetailsViewDataSource ID="dvDataSource" runat="server" TypeName="Broker.DataAccess.DocumentType"
                    ConflictDetection="CompareAllValues" DataObjectTypeName="Broker.DataAccess.DocumentType"
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
                <cc1:GXGridView ID="GXGridViewDocumentSubType" runat="server" DataSourceID="odsGridViewDocumentSubTypesByType"
                    DataKeyNames="ID" Width="100%" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"
                    Caption="Подтипови на документи" EmptyDataText="Нема записи кои го задоволуваат критериумот на пребарување!"
                    RowStyle-CssClass="row" CssClass="grid" GridLines="None">
                    <RowStyle CssClass="row" />
                    <Columns>
                        <asp:BoundField DataField="Code" HeaderText="Шифра" SortExpression="Code" />
                        <asp:BoundField DataField="Description" HeaderText="Опис" SortExpression="Description" />
                        <asp:BoundField DataField="DocumentTypeName" HeaderText="Тип на документ" SortExpression="DocumentTypeName" />
                    </Columns>
                    <PagerStyle HorizontalAlign="Center" />
                    <SelectedRowStyle CssClass="rowSelected" />
                    <PagerSettings FirstPageText="<< Прва " PreviousPageText="< Претходна " NextPageText=" Следна >"
                        LastPageText=" Последна >>" Mode="NextPreviousFirstLast" />
                </cc1:GXGridView>
                <cc1:GridViewDataSource ID="odsGridViewDocumentSubTypesByType" runat="server" TypeName="Broker.DataAccess.ViewDocumentSubType"
                    OldValuesParameterFormatString="oldEntity" SelectMethod="SelectByFK" SelectCountMethod="SelectByFKCountCached"
                    ConflictDetection="CompareAllValues" DataObjectTypeName="Broker.DataAccess.ViewDocumentSubType"
                    EnablePaging="True" SortParameterName="orderBy">
                    <SelectParameters>
                        <asp:Parameter Name="foreignKeyName" Type="String" DefaultValue="DocumentTypeID" />
                        <asp:ControlParameter Name="id" Type="Int32" ControlID="GXGridView1" PropertyName="selectedValue" />
                    </SelectParameters>
                </cc1:GridViewDataSource>
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
                </cc1:SearchControl>
                <cc1:GridViewDataSource ID="odsSearch" runat="server" TypeName="Broker.DataAccess.DocumentType"
                    SelectCountMethod="SelectSearchCountCached" SelectMethod="SelectSearch">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="SearchControl1" PropertyName="SearchArguments" Name="sArgument" />
                    </SelectParameters>
                </cc1:GridViewDataSource>
            </div>
        </asp:View>
        <asp:View ID="viewReport" runat="server">
            <div class="paddingKontroli">
                <cc1:ReportControl ID="reportControl" runat="server" TypeName="Broker.DataAccess.DocumentType"
                    GridViewID="GXGridView1" SearchControlID="SearchControl1">
                    <cc1:PrintItem HeaderText="Шифра" PropertyName="Code" />
                    <cc1:PrintItem HeaderText="Име" PropertyName="Name" />
                </cc1:ReportControl>
            </div>
        </asp:View>
        <asp:View ID="viewDocumentTypePreview" runat="server">
            <div class="paddingKontroli">
                <asp:DetailsView ID="dvDocumentTypePreview" runat="server" DataSourceID="odsDocumentTypePreview"
                    DataKeyNames="ID" AutoGenerateRows="False" OnItemCommand="dvDocumentSubTypes_ItemCommand"
                    OnModeChanging="dvDocumentSubTypes_ModeChanging" GridLines="None" DefaultMode="ReadOnly">
                    <Fields>
                        <asp:TemplateField HeaderText="Тип на документ">
                            <ItemTemplate>
                                <asp:TextBox ID="tbDocumentTypeDescription" runat="server" Text='<%# Bind("Description") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Шифра">
                            <ItemTemplate>
                                <asp:TextBox ID="tbCode" runat="server" Text='<%# Bind("Code") %>' ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Fields>
                </asp:DetailsView>
                <cc1:DetailsViewDataSource ID="odsDocumentTypePreview" runat="server" TypeName="Broker.DataAccess.DocumentType"
                    ConflictDetection="CompareAllValues" DataObjectTypeName="Broker.DataAccess.DocumentType"
                    SelectMethod="Get" OldValuesParameterFormatString="oldEntity">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="GXGridView1" Name="id" PropertyName="SelectedValue"
                            Type="Int32" />
                    </SelectParameters>
                </cc1:DetailsViewDataSource>
                <asp:MultiView ID="mvDocumentSubTypes" runat="server">
                    <asp:View ID="viewDocumentSubTypesGrid" runat="server">
                        <asp:GridView ID="GridViewDocumentSubTypes" runat="server" DataSourceID="odsGridViewDocumentSubTypes"
                            DataKeyNames="ID" Width="100%" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"
                            Caption="Подтипови на документи" EmptyDataText="Нема записи кои го задоволуваат критериумот на пребарување!"
                            RowStyle-CssClass="row" CssClass="grid" GridLines="None">
                            <RowStyle CssClass="row"></RowStyle>
                            <Columns>
                                <asp:TemplateField Visible="false">
                                    <ItemTemplate>
                                        <asp:TextBox ID="tbDocumentTypeID" runat="server" Text='<%#Bind("ID") %>' Visible="false"></asp:TextBox>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="tbDocumentTypeID" runat="server" Text='<%#Bind("ID") %>' Visible="false"></asp:TextBox>
                                    </EditItemTemplate>
                                    <InsertItemTemplate>
                                        <asp:TextBox ID="tbDocumentTypeID" runat="server" Text='<%#Bind("ID") %>' Visible="false"></asp:TextBox>
                                    </InsertItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Тип на документ">
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlDocumentTypes" runat="server" DataSourceID="odsDocumentTypes"
                                            DataTextField="Description" DataValueField="ID" SelectedValue='<%# Bind("DocumentTypeID") %>'
                                            Enabled="false" Width="70px">
                                        </asp:DropDownList>
                                        <asp:ObjectDataSource ID="odsDocumentTypes" runat="server" SelectMethod="Select"
                                            TypeName="Broker.DataAccess.DocumentType"></asp:ObjectDataSource>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="tbDocumentType" runat="server" Text='<%#Eval("DocumentType.Description") %>'
                                            Width="70px" ReadOnly="true"></asp:TextBox>
                                        <asp:TextBox ID="tbDocumentTypeID" runat="server" Text='<%#Bind("DocumentTypeID") %>'
                                            Visible="false"></asp:TextBox>
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
                        <asp:ObjectDataSource ID="odsGridViewDocumentSubTypes" runat="server" TypeName="Broker.DataAccess.DocumentSubType"
                            OldValuesParameterFormatString="oldEntity" DataObjectTypeName="Broker.DataAccess.DocumentSubType"
                            ConflictDetection="CompareAllValues" DeleteMethod="Delete" InsertMethod="Insert"
                            SelectMethod="GetByDocumentTypeID" UpdateMethod="Update">
                            <UpdateParameters>
                                <asp:Parameter Name="oldEntity" Type="Object" />
                                <asp:Parameter Name="newEntity" Type="Object" />
                            </UpdateParameters>
                            <SelectParameters>
                                <asp:ControlParameter ControlID="GXGridView1" Name="DocumentTypeID" PropertyName="SelectedValue"
                                    Type="Int32" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                        <div>
                            <asp:Button ID="btnNewOrderItem" runat="server" Text="Нова ставка" OnClick="btnNewOrderItem_Click" />
                        </div>
                    </asp:View>
                    <asp:View ID="viewDocumentSubTypes" runat="server">
                        <asp:DetailsView ID="DetailsViewSubTypes" runat="server" DataSourceID="dvDataSourceSubTypes"
                            DataKeyNames="ID" AutoGenerateRows="False" OnItemCommand="DetailsViewSubTypes_ItemCommand"
                            OnItemInserted="DetailsViewSubTypes_ItemInserted" OnModeChanging="DetailsViewSubTypes_ModeChanging"
                            OnItemInserting="DetailsViewSubTypes_ItemInserting" GridLines="None" DefaultMode="Insert">
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
                                        <asp:TextBox ID="tbDocumentTypeID" runat="server" Text='<%#Bind("ID") %>' Visible="false"></asp:TextBox>
                                    </InsertItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Шифра на документ">
                                    <InsertItemTemplate>
                                        <asp:TextBox ID="tbDocumentCode" runat="server" ReadOnly="true"></asp:TextBox>
                                    </InsertItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Опис на документ">
                                    <InsertItemTemplate>
                                        <asp:TextBox ID="tbDocumentDescription" runat="server" ReadOnly="true"></asp:TextBox>
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
                        <cc1:DetailsViewDataSource ID="dvDataSourceSubTypes" runat="server" TypeName="Broker.DataAccess.DocumentSubType"
                            ConflictDetection="CompareAllValues" DataObjectTypeName="Broker.DataAccess.DocumentSubType"
                            InsertMethod="Insert" OnInserted="dvDataSourceSubTypes_Inserted" OnInserting="dvDataSourceSubTypes_Inserting">
                            <InsertParameters>
                                <asp:Parameter Name="entityToInsert" Type="Object" />
                            </InsertParameters>
                        </cc1:DetailsViewDataSource>
                    </asp:View>
                </asp:MultiView>
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>
