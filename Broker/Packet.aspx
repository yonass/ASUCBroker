<%@ Page Title="Евидентирање на пакет од полиси" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Packet.aspx.cs" Inherits="Broker_Packet" Culture="mk-MK" UICulture="mk-MK"%>

<%@ Register Namespace="Superexpert.Controls" TagPrefix="super" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <br />
    <asp:Button ID="btnMainInformations" runat="server" Text="Основни информации" OnClick="btnMainInformations_Click"
        CssClass="PacketButton_Active" />
    <asp:Button ID="btnAdditionalInformations" runat="server" Text="Дополнителни информации"
        CssClass="PacketButton" OnClick="btnAdditionalInformations_Click" />
    <div class="PacketOut">
        <asp:MultiView ID="mvPacket" runat="server" ActiveViewIndex="0">
            <asp:View ID="viewMainInformations" runat="server">
                <div class="border1px">
                    <asp:DetailsView ID="PoliciesDetailsView" runat="server" DataKeyNames="ID" DefaultMode="Insert"
                        DataSourceID="dvDataSource" AutoGenerateRows="false" GridLines="None">
                        <Fields>
                            <asp:TemplateField>
                                <InsertItemTemplate>
                                    <table width="394px">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblPolicyNumber" runat="server" Text="Број на полиса"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="tbPolicyNumber" runat="server" Text='<%#Bind("PolicyNumber") %>'></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvPolicyNumber" runat="server" ControlToValidate="tbPolicyNumber"
                                                    Display="Dynamic" ErrorMessage="*"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr class="light-background">
                                            <td>
                                                <asp:Label ID="lblInsuranceCompanies" runat="server" Text="Осигурителна компанија"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlInsuranceCompany" runat="server" DataSourceID="odsInsuranceCompany"
                                                    DataTextField="Name" DataValueField="ID" SelectedValue='<%# Bind("InsuranceCompanyID") %>'
                                                    OnSelectedIndexChanged="ddlInsuranceCompanyIndexChanged" AutoPostBack="true">
                                                </asp:DropDownList>
                                                <asp:ObjectDataSource ID="odsInsuranceCompany" runat="server" SelectMethod="GetWithPackets"
                                                    TypeName="Broker.DataAccess.InsuranceCompany"></asp:ObjectDataSource>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblPacketDropDown" runat="server" Text="Пакети"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlPackets" runat="server" DataTextField="Name" DataValueField="ID"
                                                    DataSourceID="odsDdlPackets" AutoPostBack="True" OnSelectedIndexChanged="ddlPackets_SelectedIndexChanged"
                                                    SelectedValue='<%#Bind("PacketID") %>'>
                                                </asp:DropDownList>
                                                <asp:ObjectDataSource ID="odsDdlPackets" runat="server" SelectMethod="GetForCompany"
                                                    TypeName="Broker.DataAccess.Packet">
                                                    <SelectParameters>
                                                        <asp:ControlParameter ControlID="ddlInsuranceCompany" Name="companyID" PropertyName="SelectedValue"
                                                            Type="Int32" />
                                                    </SelectParameters>
                                                </asp:ObjectDataSource>
                                            </td>
                                        </tr>
                                        <tr class="light-background">
                                            <td>
                                                <asp:Label ID="lblStatus" runat="server" Text="Статус"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlStatus" runat="server" DataSourceID="odsStatus" DataTextField="Description"
                                                    DataValueField="ID" SelectedValue='<%# Bind("StatusID") %>'>
                                                </asp:DropDownList>
                                                <asp:ObjectDataSource ID="odsStatus" runat="server" SelectMethod="GetActiveStatuses"
                                                    TypeName="Broker.DataAccess.Statuse">
                                                    <%-- <SelectParameters>
                                                    <asp:Parameter DefaultValue="" Name="docTypeID" Type="Int32" />
                                                </SelectParameters>--%>
                                                </asp:ObjectDataSource>
                                            </td>
                                        </tr>
                                        <%--<tr>
                                        <td>
                                            <asp:Label ID="lblBroker" runat="server" Text="Брокеража"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tbBrokeragePercentage" runat="server" Text='<%#Bind("BrokeragePercentage") %>'></asp:TextBox>
                                        </td>
                                    </tr>--%>
                                        <tr >
                                            <td>
                                                <asp:Label ID="lblClientEmbg" runat="server" Text="МБ. Договорувач"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="tbClientEMBG" runat="server"></asp:TextBox>
                                                <asp:Button ID="btnClientEMBGSearch" runat="server" CausesValidation="false" Text="..."
                                                    OnClick="btnClientEMBGSearch_Click" />
                                            </td>
                                        </tr>
                                        <tr class="light-background">
                                            <td>
                                                <asp:Panel ID="pnlClient" runat="server" Visible="false">
                                                    <asp:DetailsView ID="ClientDetailsView" runat="server" DataSourceID="ClientdvDataSource"
                                                        DataKeyNames="ID" AutoGenerateRows="False" OnItemCommand="ClientDetailsView_ItemCommand"
                                                        OnItemInserted="ClientDetailsView_ItemInserted" OnModeChanging="ClientDetailsView_ModeChanging"
                                                        OnItemInserting="ClientDetailsView_ItemInserting" GridLines="None" DefaultMode="Insert">
                                                        <Fields>
                                                            <asp:TemplateField>
                                                                <InsertItemTemplate>
                                                                    <div class="subTitles">
                                                                        Нов запис во Клиенти
                                                                    </div>
                                                                </InsertItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Име и Презиме">
                                                                <InsertItemTemplate>
                                                                    <asp:TextBox ID="tbName" runat="server" Text='<%# Bind("Name") %>' MaxLength="100"></asp:TextBox>
                                                                    <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="tbName"
                                                                        Display="Dynamic" ErrorMessage="*">
                                                                        <super:EntityCallOutValidator ID="NameInsertValidator" PropertyName="NAME" runat="server" />
                                                                    </asp:RequiredFieldValidator>
                                                                </InsertItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Матичен Број">
                                                                <InsertItemTemplate>
                                                                    <asp:TextBox ID="tbEMBG" runat="server" Text='<%# Bind("EMBG") %>' MaxLength="100"></asp:TextBox>
                                                                    <asp:RequiredFieldValidator ID="rfvEMBG" runat="server" ControlToValidate="tbEMBG"
                                                                        ErrorMessage="*"></asp:RequiredFieldValidator>
                                                                    <super:EntityCallOutValidator ID="EmbgInsertValidator" PropertyName="EMBG" runat="server" />
                                                                </InsertItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Адреса">
                                                                <InsertItemTemplate>
                                                                    <asp:TextBox ID="tbAddress" MaxLength="100" runat="server" Text='<%# Bind("Address") %>'></asp:TextBox>
                                                                    <asp:RequiredFieldValidator ID="rfvAddress" runat="server" ControlToValidate="tbAddress"
                                                                        ErrorMessage="*"></asp:RequiredFieldValidator>
                                                                    <super:EntityCallOutValidator ID="AddressInsertValidator" PropertyName="ADDRESS"
                                                                        runat="server" />
                                                                </InsertItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Населено Место">
                                                                <InsertItemTemplate>
                                                                    <asp:DropDownList ID="ddlPlaces" runat="server" DataSourceID="odsActivePlaces" DataTextField="Name"
                                                                        DataValueField="ID" SelectedValue='<%# Bind("PlaceID") %>'>
                                                                    </asp:DropDownList>
                                                                    <asp:ObjectDataSource ID="odsActivePlaces" runat="server" SelectMethod="GetActivePlaces"
                                                                        TypeName="Broker.DataAccess.Place"></asp:ObjectDataSource>
                                                                </InsertItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Телефон">
                                                                <InsertItemTemplate>
                                                                    <asp:TextBox ID="tbPhone" MaxLength="30" runat="server" Text='<%# Bind("Phone") %>'></asp:TextBox>
                                                                </InsertItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Мобилен Тел.">
                                                                <InsertItemTemplate>
                                                                    <asp:TextBox ID="tbMobile" MaxLength="30" runat="server" Text='<%# Bind("Mobile") %>'></asp:TextBox>
                                                                </InsertItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Факс">
                                                                <InsertItemTemplate>
                                                                    <asp:TextBox ID="tbFax" MaxLength="30" runat="server" Text='<%# Bind("Fax") %>'></asp:TextBox>
                                                                </InsertItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Email">
                                                                <InsertItemTemplate>
                                                                    <asp:TextBox ID="tbEmail" MaxLength="100" runat="server" Text='<%# Bind("Email") %>'></asp:TextBox>
                                                                </InsertItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Правно лице">
                                                                <InsertItemTemplate>
                                                                    <asp:CheckBox ID="cbIsLaw" runat="server" Checked='<%#Bind("IsLaw") %>' />
                                                                </InsertItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <InsertItemTemplate>
                                                                    <asp:Button ID="btnCancel" runat="server" CommandName="Cancel" Text="Откажи" CausesValidation="false" />
                                                                    <asp:Button ID="btnInsert" runat="server" CommandName="Insert" Text="Внеси" CausesValidation="false" />
                                                                </InsertItemTemplate>
                                                            </asp:TemplateField>
                                                        </Fields>
                                                    </asp:DetailsView>
                                                    <cc1:DetailsViewDataSource ID="ClientdvDataSource" runat="server" TypeName="Broker.DataAccess.Client"
                                                        ConflictDetection="CompareAllValues" DataObjectTypeName="Broker.DataAccess.Client"
                                                        DeleteMethod="Delete" InsertMethod="Insert" SelectMethod="GetByEmbg" UpdateMethod="Update"
                                                        OnInserted="ClientdvDataSource_Inserted" OnInserting="ClientdvDataSource_Inserting">
                                                        <InsertParameters>
                                                            <asp:Parameter Name="entityToInsert" Type="Object" />
                                                        </InsertParameters>
                                                    </cc1:DetailsViewDataSource>
                                                </asp:Panel>
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr class="light-background">
                                            <td>
                                                <asp:Label ID="lblClientName" runat="server" Text="Договорувач"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="tbClientName" runat="server" ReadOnly="true"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr >
                                            <td>
                                                <asp:Label ID="lblOwnerEmbg" runat="server" Text="МБ. Осигуреник"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="tbOwnerEMBG" runat="server"></asp:TextBox>
                                                <asp:Button ID="btnOwnerEMBGSearch" runat="server" CausesValidation="false" Text="..."
                                                    OnClick="btnOwnerEMBGSearch_Click" />
                                            </td>
                                        </tr>
                                        <tr class="light-background">
                                            <td>
                                                <asp:Panel ID="pnlOwner" runat="server" Visible="false">
                                                    <asp:DetailsView ID="OwnerDetailsView" runat="server" DataSourceID="OwnerdvDataSource"
                                                        DataKeyNames="ID" AutoGenerateRows="False" OnItemCommand="OwnerDetailsView_ItemCommand"
                                                        OnItemInserted="OwnerDetailsView_ItemInserted" OnModeChanging="OwnerDetailsView_ModeChanging"
                                                        OnItemInserting="OwnerDetailsView_ItemInserting" GridLines="None" DefaultMode="Insert">
                                                        <Fields>
                                                            <asp:TemplateField>
                                                                <InsertItemTemplate>
                                                                    <div class="subTitles">
                                                                        Нов запис во Клиенти
                                                                    </div>
                                                                </InsertItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Име и Презиме">
                                                                <InsertItemTemplate>
                                                                    <asp:TextBox ID="tbName" runat="server" Text='<%# Bind("Name") %>' MaxLength="100"></asp:TextBox>
                                                                    <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="tbName"
                                                                        Display="Dynamic" ErrorMessage="*">
                                                                    </asp:RequiredFieldValidator>
                                                                    <super:EntityCallOutValidator ID="NameInsertValidator" PropertyName="OwnerNAME" runat="server" />
                                                                </InsertItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Матичен Број">
                                                                <InsertItemTemplate>
                                                                    <asp:TextBox ID="tbEMBG" runat="server" Text='<%# Bind("EMBG") %>' MaxLength="100"></asp:TextBox>
                                                                    <asp:RequiredFieldValidator ID="rfvEMBG" runat="server" ControlToValidate="tbEMBG"
                                                                        ErrorMessage="*"></asp:RequiredFieldValidator>
                                                                    <super:EntityCallOutValidator ID="EmbgInsertValidator" PropertyName="OwnerEMBG" runat="server" />
                                                                </InsertItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Адреса">
                                                                <InsertItemTemplate>
                                                                    <asp:TextBox ID="tbAddress" MaxLength="100" runat="server" Text='<%# Bind("Address") %>'></asp:TextBox>
                                                                    <asp:RequiredFieldValidator ID="rfvAddress" runat="server" ControlToValidate="tbAddress"
                                                                        ErrorMessage="*"></asp:RequiredFieldValidator>
                                                                    <super:EntityCallOutValidator ID="AddressInsertValidator" PropertyName="OwnerADDRESS"
                                                                        runat="server" />
                                                                </InsertItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Населено Место">
                                                                <InsertItemTemplate>
                                                                    <asp:DropDownList ID="ddlPlaces" runat="server" DataSourceID="odsActivePlaces" DataTextField="Name"
                                                                        DataValueField="ID" SelectedValue='<%# Bind("PlaceID") %>'>
                                                                    </asp:DropDownList>
                                                                    <asp:ObjectDataSource ID="odsActivePlaces" runat="server" SelectMethod="GetActivePlaces"
                                                                        TypeName="Broker.DataAccess.Place"></asp:ObjectDataSource>
                                                                </InsertItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Телефон">
                                                                <InsertItemTemplate>
                                                                    <asp:TextBox ID="tbPhone" MaxLength="30" runat="server" Text='<%# Bind("Phone") %>'></asp:TextBox>
                                                                </InsertItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Мобилен Тел.">
                                                                <InsertItemTemplate>
                                                                    <asp:TextBox ID="tbMobile" MaxLength="30" runat="server" Text='<%# Bind("Mobile") %>'></asp:TextBox>
                                                                </InsertItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Факс">
                                                                <InsertItemTemplate>
                                                                    <asp:TextBox ID="tbFax" MaxLength="30" runat="server" Text='<%# Bind("Fax") %>'></asp:TextBox>
                                                                </InsertItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Email">
                                                                <InsertItemTemplate>
                                                                    <asp:TextBox ID="tbEmail" MaxLength="100" runat="server" Text='<%# Bind("Email") %>'></asp:TextBox>
                                                                </InsertItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Правно лице">
                                                                <InsertItemTemplate>
                                                                    <asp:CheckBox ID="cbIsLaw" runat="server" Checked='<%#Bind("IsLaw") %>' />
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
                                                    <cc1:DetailsViewDataSource ID="OwnerdvDataSource" runat="server" TypeName="Broker.DataAccess.Client"
                                                        ConflictDetection="CompareAllValues" DataObjectTypeName="Broker.DataAccess.Client"
                                                        DeleteMethod="Delete" InsertMethod="Insert" SelectMethod="GetByEmbg" UpdateMethod="Update"
                                                        OnInserted="OwnerdvDataSource_Inserted" OnInserting="OwnerdvDataSource_Inserting">
                                                        <InsertParameters>
                                                            <asp:Parameter Name="entityToInsert" Type="Object" />
                                                        </InsertParameters>
                                                    </cc1:DetailsViewDataSource>
                                                </asp:Panel>
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr class="light-background">
                                            <td>
                                                <asp:Label ID="lblOwnerName" runat="server" Text="Осигуреник"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="tbOwnerName" runat="server" ReadOnly="true"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblStartDate" runat="server" Text="Почетна Дата"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="tbStartDate" runat="server" Text='<%# Bind("StartDate", "{0:d}") %>'></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvStartDate" runat="server" ControlToValidate="tbStartDate"
                                                    Display="Dynamic" ErrorMessage="*"></asp:RequiredFieldValidator>
                                                <asp:CompareValidator ID="cvStartDate" runat="server" ControlToValidate="tbStartDate"
                                                    Display="Dynamic" ErrorMessage="*" Operator="DataTypeCheck" Type="Date"></asp:CompareValidator>
                                            </td>
                                        </tr>
                                        <tr class="light-background">
                                            <td>
                                                <asp:Label ID="lblEndDate" runat="server" Text="Крајна Дата"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="tbEndDate" runat="server" Text='<%# Bind("EndDate","{0:d}") %>'></asp:TextBox>
                                                <asp:CompareValidator ID="cvEndDate" runat="server" ControlToCompare="tbStartDate"
                                                    ControlToValidate="tbEndDate" Display="Dynamic" ErrorMessage="*" Operator="GreaterThanEqual"></asp:CompareValidator>
                                                <asp:CompareValidator ID="cvEndDate2" runat="server" ControlToValidate="tbEndDate"
                                                    ErrorMessage="**" Operator="DataTypeCheck" Type="Date"></asp:CompareValidator>
                                            </td>
                                        </tr>
                                         <tr >
                                            <td>
                                                <asp:Label ID="lblApplicationDate" runat="server" Text="Дата на издавање"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="tbApplicationDate" runat="server" Text='<%# Bind("ApplicationDate","{0:d}") %>'></asp:TextBox>
                                                <asp:CompareValidator ID="cvApplicationDate" runat="server" ControlToValidate="tbApplicationDate"
                                                    ErrorMessage="**" Operator="DataTypeCheck" Type="Date"></asp:CompareValidator>
                                                    <asp:RequiredFieldValidator ID="rvfApplicationDate" runat="server" ControlToValidate="tbApplicationDate"
                                                    Display="Dynamic" ErrorMessage="*"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr class="light-background">
                                            <td>
                                                <asp:Label ID="lblPaymentPlace" runat="server" Text="Платена веднаш"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="cbPaymentPlace" runat="server" Checked="true" AutoPostBack="true"
                                                    OnCheckedChanged="Checked_Changed" />
                                            </td>
                                        </tr>

                                    </table>
                                </InsertItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <InsertItemTemplate>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblError" runat="server" Visible="false"></asp:Label>
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                    </table>
                                </InsertItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <InsertItemTemplate>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnCancel" runat="server" CommandName="Cancel" Text="Откажи" CausesValidation="false" />
                                            </td>
                                            <td>
                                                <asp:Button ID="btnInsert" runat="server" CommandName="Insert" Text="Следен" />
                                            </td>
                                        </tr>
                                    </table>
                                </InsertItemTemplate>
                            </asp:TemplateField>
                        </Fields>
                    </asp:DetailsView>
                </div>
                <cc1:DetailsViewDataSource ID="dvDataSource" runat="server" TypeName="Broker.DataAccess.Policy"
                    ConflictDetection="CompareAllValues" DataObjectTypeName="Broker.DataAccess.Policy"
                    InsertMethod="Insert2" OnInserted="dvDataSource_Inserted" OnInserting="dvDataSource_Inserting">
                    <InsertParameters>
                        <asp:Parameter Name="entityToInsert" Type="Object" />
                        <asp:Parameter Name="StartDate" Type="DateTime" ConvertEmptyStringToNull="true" />
                        <asp:Parameter Name="EndDate" Type="DateTime" ConvertEmptyStringToNull="true" />
                        <asp:Parameter Name="ApplicationDate" Type="DateTime" ConvertEmptyStringToNull="true" />
                    </InsertParameters>
                </cc1:DetailsViewDataSource>
            </asp:View>
            <asp:View ID="viewAdditionalInformations" runat="server">
                <asp:Panel ID="pnlEverything" runat="server">
                    <div>
                        <asp:Panel ID="pnlViewButtons" runat="server">
                        </asp:Panel>
                    </div>
                    <div class="mvMainPanel">
                        <asp:Panel ID="pnlMainInformation" runat="server" CssClass="mvMainPanel" Width="520px">
                            <asp:MultiView ID="mvMain" runat="server">
                            </asp:MultiView>
                        </asp:Panel>
                    </div>
                </asp:Panel>
            </asp:View>
            <asp:View ID="FacturePrintingView" runat="server">
                <table>
                    <tr>
                        <td>
                            <asp:Button ID="btnPrintFacture" runat="server" Text="Печати фактура" OnClick="btnPrintFacture_Click" />
                        </td>
                        <td>
                            <asp:Button ID="btnDiscardPacket" runat="server" Text="Сторнирај Пакет" OnClick="btnDiscardPolicy_Click" />
                        </td>
                    </tr>
                </table>
            </asp:View>
            <asp:View ID="BillView" runat="server">
                <table>
                    <tr>
                        <td>
                            <asp:Button ID="btnPrintBill" runat="server" Text="Печати Сметка" OnClick="btnPrintBill_Click" />
                        </td>
                        <td>
                            <asp:Button ID="btnDiscardBill" runat="server" Text="Сторнирај Сметка" Enabled="false"
                                OnClick="btnDiscardBill_Click" />
                        </td>
                        <td>
                            <asp:Button ID="btnDiscardPolicy" runat="server" Text="Сторнирај Пакет" OnClick="btnDiscardPolicy_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblCashPayment" runat="server" Text="Готовина"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="tbCashPayment" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblCardPayment" runat="server" Text="Кредитна Картичка"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="tbCreditCardPayment" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblBanks" runat="server" Text="Банка"></asp:Label>
                            <asp:DropDownList ID="ddlBank" runat="server" DataSourceID="odsBank" AutoPostBack="True"
                                DataTextField="Name" DataValueField="ID">
                            </asp:DropDownList>
                            <asp:ObjectDataSource ID="odsBank" runat="server" SelectMethod="GetBanksWithGreditCards"
                                TypeName="Broker.DataAccess.Bank"></asp:ObjectDataSource>
                        </td>
                        <td>
                            <asp:Label ID="lblCardTypes" runat="server" Text="Тип на Картичка"></asp:Label>
                            <asp:DropDownList ID="ddlCardTypes" runat="server" DataSourceID="odsCardTypes" AutoPostBack="True"
                                DataTextField="Name" DataValueField="ID">
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
                            <asp:Button ID="btnInsertNewPayment" runat="server" Text="Внеси" OnClick="btnInsertNewPayment_Click"
                                Enabled="false" />
                        </td>
                    </tr>
                </table>
            </asp:View>
        </asp:MultiView>
    </div>
</asp:Content>
