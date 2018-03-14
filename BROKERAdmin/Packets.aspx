<%@ Page Title="Брокеражи за пакети од полиси" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Packets.aspx.cs" Inherits="BROKERAdmin_Packets" UICulture="mk-MK" Culture="mk-MK" %>

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
            <asp:Button ID="btnBrokerage" runat="server" ToolTip="Брокеражи" OnClick="btnBrokerage_Click"
                CausesValidation="false" UseSubmitBehavior="false" CssClass="brokerazi" BorderWidth="0px" />
        </div>
    </div>
    <asp:MultiView ID="mvMain" runat="server">
        <asp:View ID="viewGrid" runat="server">
            <div id="tabeliFrame">
                <div id="header">
                    <div id="content">
                        Пакети
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
                                <asp:BoundField HeaderText="Опис" DataField="Description" SortExpression="Description" />
                                <asp:BoundField HeaderText="Осиг. компанија" DataField="InsuranceCompanyName" SortExpression="InsuranceCompanyName" />
                            </Columns>
                            <PagerStyle HorizontalAlign="Center" />
                            <SelectedRowStyle CssClass="rowSelected" />
                            <PagerSettings FirstPageText="<< Прва " PreviousPageText="< Претходна " NextPageText=" Следна >"
                                LastPageText=" Последна >>" Mode="NextPreviousFirstLast" />
                        </cc1:GXGridView>
                        <cc1:GridViewDataSource ID="odsGridView" runat="server" TypeName="Broker.DataAccess.ActivePacket"
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
                                        <cc1:FilterItem FieldName="Опис" PropertyName="Description" Comparator="StringComparators"
                                            DataType="String" />
                                        <cc1:FilterItem FieldName="Осиг. компанија" PropertyName="InsuranceCompanyName" Comparator="StringComparators"
                                            DataType="String" />
                                        <cc1:FilterItem FieldName="Осиг. компанија (код)" PropertyName="InsuranceCompanyCode"
                                            Comparator="StringComparators" DataType="String" />
                                    </cc1:FilterControl>
                                </div>
                                <cc1:FilterDataSource ID="odsFilterGridView" runat="server" TypeName="Broker.DataAccess.ActivePacket">
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
                    GridLines="None" OnDataBound="DetailsView1_DataBound" OnItemDeleting="DetailsView1_ItemDeleting"
                    OnItemUpdating="DetailsView1_ItemUpdating">
                    <Fields>
                        <asp:TemplateField>
                            <InsertItemTemplate>
                                <div class="subTitles">
                                    Нов запис во Пакети
                                </div>
                            </InsertItemTemplate>
                            <EditItemTemplate>
                                <div class="subTitles">
                                    Измена на запис во Пакети
                                </div>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <div class="subTitles">
                                    Бришење на запис од Пакети
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Шифра">
                            <InsertItemTemplate>
                                <asp:TextBox ID="tbCode" runat="server" Text='<%# Bind("Code") %>' MaxLength="10"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvCode" runat="server" ControlToValidate="tbCode"
                                    Display="Dynamic" ErrorMessage="*">
                                </asp:RequiredFieldValidator>
                                <super:EntityCallOutValidator ID="CodeInsertValidator" PropertyName="PACKET_CODE_INSERT_EXISTS"
                                    runat="server" />
                            </InsertItemTemplate>
                            <ItemTemplate>
                                <asp:TextBox ID="tbCode" runat="server" Text='<%# Bind("Code") %>' ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="tbCode" runat="server" Text='<%# Bind("Code") %>' MaxLength="10"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvCode" runat="server" ControlToValidate="tbCode"
                                    ErrorMessage="*"></asp:RequiredFieldValidator>
                                <super:EntityCallOutValidator ID="CodeUpdateValidator" PropertyName="PACKET_CODE_UPDATE_EXISTS"
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
                                    ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                            </EditItemTemplate>
                            <InsertItemTemplate>
                                <asp:TextBox ID="tbName" runat="server" Text='<%# Bind("Name") %>' MaxLength="50"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="tbName"
                                    ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                            </InsertItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Опис">
                            <ItemTemplate>
                                <asp:TextBox ID="tbDescription" runat="server" Text='<%# Bind("Description") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="tbDescription" runat="server" Text='<%# Bind("Description") %>'
                                    MaxLength="100"></asp:TextBox>
                            </EditItemTemplate>
                            <InsertItemTemplate>
                                <asp:TextBox ID="tbDescription" runat="server" Text='<%# Bind("Description") %>'
                                    MaxLength="100"></asp:TextBox>
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
                        <%--<asp:TemplateField>
                        <ItemTemplate>
                        <asp:DropDownList ID="ddlInsuranceTypes" AutoPostBack="True" runat="server" 
                                DataSourceID="odsInsuranceTypes" DataTextField="Name" DataValueField="ID">
                        
                        </asp:DropDownList>
                        <asp:ObjectDataSource ID="odsInsuranceTypes" runat="server" SelectMethod="Select" 
                                TypeName="Broker.DataAccess.InsuranceType" >
                        </asp:ObjectDataSource>
                        </ItemTemplate>
                        <EditItemTemplate>
                        <asp:DropDownList ID="ddlInsuranceTypes" AutoPostBack="True" runat="server" 
                                DataSourceID="odsInsuranceTypes" DataTextField="Name" DataValueField="ID">
                        
                        </asp:DropDownList>
                        <asp:ObjectDataSource ID="odsInsuranceTypes" runat="server" SelectMethod="Select" 
                                TypeName="Broker.DataAccess.InsuranceType" >
                        </asp:ObjectDataSource>
                        </EditItemTemplate>
                        <InsertItemTemplate>
                        <asp:DropDownList ID="ddlInsuranceTypes" AutoPostBack="True" runat="server" 
                                DataSourceID="odsInsuranceTypes" DataTextField="Name" DataValueField="ID">
                        
                        </asp:DropDownList>
                        <asp:ObjectDataSource ID="odsInsuranceTypes" runat="server" SelectMethod="Select" 
                                TypeName="Broker.DataAccess.InsuranceType" >
                        </asp:ObjectDataSource>
                        </InsertItemTemplate>
                        </asp:TemplateField>--%>
                        <asp:TemplateField HeaderText="Подкласи на осигурување">
                            <EditItemTemplate>
                                <asp:CheckBoxList ID="InsuranceSubTypes" runat="server" DataSourceID="odsInsuranceSubTypes"
                                    DataTextField="Description" DataValueField="ID">
                                </asp:CheckBoxList>
                                <asp:ObjectDataSource ID="odsInsuranceSubTypes" runat="server" SelectMethod="Select"
                                    TypeName="Broker.DataAccess.InsuranceSubType"></asp:ObjectDataSource>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:CheckBoxList ID="InsuranceSubTypes" runat="server" DataSourceID="odsInsuranceSubTypes"
                                    DataTextField="Description" DataValueField="ID">
                                </asp:CheckBoxList>
                                <asp:ObjectDataSource ID="odsInsuranceSubTypes" runat="server" SelectMethod="Select"
                                    TypeName="Broker.DataAccess.InsuranceSubType"></asp:ObjectDataSource>
                            </ItemTemplate>
                            <InsertItemTemplate>
                                <asp:CheckBoxList ID="InsuranceSubTypes" runat="server" DataSourceID="odsInsuranceSubTypes"
                                    DataTextField="Description" DataValueField="ID">
                                </asp:CheckBoxList>
                                <asp:ObjectDataSource ID="odsInsuranceSubTypes" runat="server" SelectMethod="Select"
                                    TypeName="Broker.DataAccess.InsuranceSubType"></asp:ObjectDataSource>
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
                <cc1:DetailsViewDataSource ID="dvDataSource" runat="server" TypeName="Broker.DataAccess.Packet"
                    ConflictDetection="CompareAllValues" DataObjectTypeName="Broker.DataAccess.Packet"
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
                    <cc1:SearchItem FieldName="Шифра" PropertyName="Code" Comparator="StringComparators"
                        DataType="String" />
                    <cc1:SearchItem FieldName="Име" PropertyName="Name" Comparator="StringComparators"
                        DataType="String" />
                    <cc1:SearchItem FieldName="Опис" PropertyName="Description" Comparator="StringComparators"
                        DataType="String" />
                    <cc1:SearchItem FieldName="Осигурителна компанија" PropertyName="InsuranceCompanyName"
                        Comparator="StringComparators" DataType="String" />
                </cc1:SearchControl>
                <cc1:GridViewDataSource ID="odsSearch" runat="server" TypeName="Broker.DataAccess.ActivePacket"
                    SelectCountMethod="SelectSearchCountCached" SelectMethod="SelectSearch">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="SearchControl1" PropertyName="SearchArguments" Name="sArgument" />
                    </SelectParameters>
                </cc1:GridViewDataSource>
            </div>
        </asp:View>
        <asp:View ID="viewReport" runat="server">
            <div class="paddingKontroli">
                <cc1:ReportControl ID="reportControl" runat="server" TypeName="Broker.DataAccess.ActivePacket"
                    GridViewID="GXGridView1" SearchControlID="SearchControl1">
                    <cc1:PrintItem HeaderText="Шифра" PropertyName="Code" />
                    <cc1:PrintItem HeaderText="Име" PropertyName="Name" />
                    <cc1:PrintItem HeaderText="Опис" PropertyName="Description" />
                    <cc1:PrintItem HeaderText="Осигурителна компанија" PropertyName="InsuranceCompanyName" />
                </cc1:ReportControl>
            </div>
        </asp:View>
        <asp:View ID="viewPercentages" runat="server">
            <asp:GridView ID="GridViewPercentages" runat="server" DataSourceID="odsGridViewPercentages"
                DataKeyNames="PacketID, InsuranceSubTypeID" Width="100%" AllowPaging="True" AllowSorting="True"
                AutoGenerateColumns="False" Caption="Проценти на брокеража" EmptyDataText="Нема записи кои го задоволуваат критериумот на пребарување!"
                RowStyle-CssClass="row" CssClass="grid" GridLines="None">
                <RowStyle CssClass="row"></RowStyle>
                <Columns>
                    <asp:TemplateField HeaderText="Пакет">
                        <EditItemTemplate>
                            <asp:TextBox ID="tbPacket" runat="server" Text='<%#Eval("Packet.Name") %>' Width="80px"
                                ReadOnly="true"></asp:TextBox>
                            <asp:TextBox ID="tbPacketID" runat="server" Text='<%#Bind("PacketID") %>' Visible="false"></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="tbPacket" runat="server" Text='<%#Eval("Packet.Name") %>' Width="80px"
                                ReadOnly="true"></asp:TextBox>
                            <asp:TextBox ID="tbPacketID" runat="server" Text='<%#Bind("PacketID") %>' Visible="false"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Класа на осигурување">
                        <EditItemTemplate>
                            <asp:TextBox ID="tbInsuranceType" runat="server" Text='<%#Eval("InsuranceSubType.InsuranceType.Name") %>'
                                Width="150px" ReadOnly="true"></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="tbInsuranceType" runat="server" Text='<%#Eval("InsuranceSubType.InsuranceType.Name") %>'
                                Width="150px" ReadOnly="true"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Подкласа на осигурување">
                        <EditItemTemplate>
                            <asp:TextBox ID="tbInsuranceSubType" runat="server" Text='<%#Eval("InsuranceSubType.Description") %>'
                                Width="150px" ReadOnly="true"></asp:TextBox>
                            <asp:TextBox ID="tbInsuranceSubTypeID" runat="server" Text='<%#Bind("InsuranceSubTypeID") %>'
                                Visible="false"></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="tbInsuranceSubType" runat="server" Text='<%#Eval("InsuranceSubType.Description") %>'
                                Width="150px" ReadOnly="true"></asp:TextBox>
                            <asp:TextBox ID="tbInsuranceSubTypeID" runat="server" Text='<%#Bind("InsuranceSubTypeID") %>'
                                Visible="false"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Процент (физички)" HeaderStyle-Width="54px">
                        <EditItemTemplate>
                            <asp:TextBox ID="tbPercentageForPrivates" runat="server" Text='<%#Bind("BrokeragePecentageForPrivates") %>'
                                Width="50px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvCostForPrivates" runat="server" ControlToValidate="tbPercentageForPrivates"
                                Display="Dynamic" ErrorMessage="*"></asp:RequiredFieldValidator>
                            <asp:CompareValidator ID="cvCostForPrivates" runat="server" ControlToValidate="tbPercentageForPrivates"
                                Display="Dynamic" ErrorMessage="*" Operator="DataTypeCheck" Type="Double"></asp:CompareValidator>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="tbPercentageForPrivates" runat="server" Text='<%#Bind("BrokeragePecentageForPrivates") %>'
                                ReadOnly="true" Width="50px"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Процент (правни)" HeaderStyle-Width="54px">
                        <EditItemTemplate>
                            <asp:TextBox ID="tbPercentageForLaws" runat="server" Text='<%#Bind("BrokeragePecentageForLaws") %>'
                                Width="50px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvCostForLaws" runat="server" ControlToValidate="tbPercentageForLaws"
                                Display="Dynamic" ErrorMessage="*"></asp:RequiredFieldValidator>
                            <asp:CompareValidator ID="cvCostForLaws" runat="server" ControlToValidate="tbPercentageForLaws"
                                Display="Dynamic" ErrorMessage="*" Operator="DataTypeCheck" Type="Double"></asp:CompareValidator>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="tbPercentage" runat="server" Text='<%#Bind("BrokeragePecentageForLaws") %>'
                                ReadOnly="true" Width="50px"></asp:TextBox>
                        </ItemTemplate>
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
            <asp:ObjectDataSource ID="odsGridViewPercentages" runat="server" TypeName="Broker.DataAccess.PacketsInsuranceSubType"
                OldValuesParameterFormatString="oldEntity" DataObjectTypeName="Broker.DataAccess.PacketsInsuranceSubType"
                ConflictDetection="CompareAllValues" DeleteMethod="Delete" InsertMethod="Insert"
                SelectMethod="GetByPacket" UpdateMethod="Update">
                <UpdateParameters>
                    <asp:Parameter Name="oldEntity" Type="Object" />
                    <asp:Parameter Name="newEntity" Type="Object" />
                    <asp:Parameter Name="BrokeragePecentageForPrivates" Type="Decimal" />
                    <asp:Parameter Name="BrokeragePecentageForLaws" Type="Decimal" />
                </UpdateParameters>
                <SelectParameters>
                    <asp:ControlParameter ControlID="GXGridView1" Name="packetID" PropertyName="SelectedValue"
                        Type="Int32" />
                </SelectParameters>
            </asp:ObjectDataSource>
        </asp:View>
    </asp:MultiView>
</asp:Content>
