<%@ Page Title="Промена на техничка грешка на полиса" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="PolicyTechnicalError.aspx.cs" Inherits="Broker_PolicyTechnicalError" %>

<%@ Register Namespace="Superexpert.Controls" TagPrefix="super" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript">
        function toEnglish(x) {
            var y = document.getElementById(x).value;
            y = y.replace("А", "A");
            y = y.replace("Б", "B");
            y = y.replace("В", "V");
            y = y.replace("Г", "G");
            y = y.replace("Д", "D");
            y = y.replace("Ѓ", "Đ");
            y = y.replace("Е", "E");
            y = y.replace("Ж", "Ž");
            y = y.replace("З", "Z");
            y = y.replace("Ѕ", "Y");
            y = y.replace("И", "I");
            y = y.replace("Ј", "J");
            y = y.replace("К", "K");
            y = y.replace("Л", "L");
            y = y.replace("Љ", "Q");
            y = y.replace("М", "M");
            y = y.replace("Н", "N");
            y = y.replace("Њ", "W");
            y = y.replace("О", "O");
            y = y.replace("П", "P");
            y = y.replace("Р", "R");
            y = y.replace("С", "S");
            y = y.replace("Т", "T");
            y = y.replace("Ќ", "Ć");
            y = y.replace("У", "U");
            y = y.replace("Ф", "F");
            y = y.replace("Х", "H");
            y = y.replace("Ц", "C");
            y = y.replace("Ч", "Č");
            y = y.replace("Џ", "X");
            y = y.replace("Ш", "Š");
            y = y.replace("а", "a");
            y = y.replace("б", "b");
            y = y.replace("в", "v");
            y = y.replace("г", "g");
            y = y.replace("д", "d");
            y = y.replace("ѓ", "đ");
            y = y.replace("е", "e");
            y = y.replace("ж", "ž");
            y = y.replace("з", "z");
            y = y.replace("ѕ", "y");
            y = y.replace("и", "i");
            y = y.replace("ј", "j");
            y = y.replace("к", "k");
            y = y.replace("л", "l");
            y = y.replace("љ", "q");
            y = y.replace("м", "m");
            y = y.replace("н", "n");
            y = y.replace("њ", "w");
            y = y.replace("о", "o");
            y = y.replace("п", "p");
            y = y.replace("р", "r");
            y = y.replace("с", "s");
            y = y.replace("т", "t");
            y = y.replace("ќ", "ć");
            y = y.replace("у", "u");
            y = y.replace("ф", "f");
            y = y.replace("х", "h");
            y = y.replace("ц", "c");
            y = y.replace("ч", "č");
            y = y.replace("џ", "x");
            y = y.replace("ш", "š");
            document.getElementById(x).value = y.toUpperCase();
        }

    </script>

    <table width="695px">
        <tr>
            <td style="width: 270px">
                <asp:Label ID="lblPolicyNumber" runat="server" Text="Број на полиса"></asp:Label>
            </td>
            <td style="width: 445px">
                <asp:TextBox ID="tbPolicyNumber" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvPolicyNumber" runat="server" ControlToValidate="tbPolicyNumber"
                    Display="Dynamic" ErrorMessage="*"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr class="light-background">
            <td style="width: 270px">
                <asp:Label ID="lblInsuranceType" runat="server" Text="Тип на осигурување"></asp:Label>
            </td>
            <td style="width: 445px">
                <asp:DropDownList ID="ddlInsuranceType" runat="server" DataSourceID="odsInsuranceType"
                    Width="425px" DataTextField="Name" DataValueField="ID" AutoPostBack="true" OnSelectedIndexChanged="ddlInsuranceTypeSelectedIndexChanged">
                </asp:DropDownList>
                <asp:ObjectDataSource ID="odsInsuranceType" runat="server" SelectMethod="GetForPolicy"
                    TypeName="Broker.DataAccess.InsuranceType"></asp:ObjectDataSource>
            </td>
        </tr>
        <tr>
            <td style="width: 270px">
                <asp:Label ID="lblInsuranceSubType" runat="server" Text="Подтип на осигурување"></asp:Label>
            </td>
            <td style="width: 445px">
                <asp:DropDownList ID="ddlInsuranceSubTypes" runat="server" DataSourceID="odsInsuranceSubTypes"
                    DataTextField="Description" DataValueField="ID" AutoPostBack="true" OnSelectedIndexChanged="ddlInsuranceSubType_selecteIndexChanged"
                    Width="425px">
                </asp:DropDownList>
                <asp:ObjectDataSource ID="odsInsuranceSubTypes" runat="server" SelectMethod="GetByInsuranceTypeAndExistingDeals"
                    TypeName="Broker.DataAccess.InsuranceSubType">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="ddlInsuranceType" Name="insuranceTypeID" PropertyName="SelectedValue"
                            Type="Int32" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </td>
        </tr>
        <tr class="light-background">
            <td style="width: 270px">
                <asp:Label ID="lblInsuranceCompanies" runat="server" Text="Осигурителна компанија"></asp:Label>
            </td>
            <td style="width: 445px">
                <asp:DropDownList ID="ddlInsuranceCompany" runat="server" DataSourceID="odsInsuranceCompany"
                    DataTextField="Name" DataValueField="ID" Width="250px" OnSelectedIndexChanged="ddlInsuranceCompanyIndexChanged"
                    AutoPostBack="true">
                </asp:DropDownList>
                <asp:ObjectDataSource ID="odsInsuranceCompany" runat="server" SelectMethod="GetByInsuranceSubType"
                    TypeName="Broker.DataAccess.InsuranceCompany">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="ddlInsuranceSubTypes" Name="insuranceSubTypeID"
                            PropertyName="SelectedValue" Type="Int32" />
                    </SelectParameters>
                </asp:ObjectDataSource>
                <asp:Button ID="btnSearch" runat="server" Text="Барај" OnClick="btnSearch_Click"
                    CausesValidation="false" />
                <asp:Button ID="btnNewChange" runat="server" Text="Нова измена" OnClick="btnNewChange_Click"
                    CausesValidation="false" />
            </td>
        </tr>
    </table>
    <asp:DetailsView ID="PoliciesDetailsView" runat="server" DataKeyNames="ID" DefaultMode="Edit"
        DataSourceID="dvDataSource" AutoGenerateRows="False" Width="330px" GridLines="None">
        <Fields>
            <asp:TemplateField>
                <EditItemTemplate>
                    <table width="695px">
                        <tr>
                            <td>
                                <b>Податоци за договорувач, осигуреник и скаденца на полиса</b>
                            </td>
                        </tr>
                    </table>
                    <table width="695px">
                        <tr class="light-background">
                            <td style="width: 160px">
                                <asp:Label ID="lblClientEmbg" runat="server" Text="МБ. Договорувач"></asp:Label>
                            </td>
                            <td style="width: 187px">
                                <asp:TextBox ID="tbClientEMBG" Text='<%# Eval("Policy.Client.EMBG") %>' runat="server"></asp:TextBox>
                                <asp:Button ID="btnClientEMBGSearch" runat="server" CausesValidation="false" Text="..."
                                    OnClick="btnClientEMBGSearch_Click" />
                            </td>
                            <td style="width: 160px">
                                <asp:Label ID="lblOwnerEmbg" runat="server" Text="МБ. Осигуреник"></asp:Label>
                            </td>
                            <td style="width: 187px">
                                <asp:TextBox ID="tbOwnerEMBG" Text='<%# Eval("Policy.Client1.EMBG") %>' runat="server"></asp:TextBox>
                                <asp:Button ID="btnOwnerEMBGSearch" runat="server" CausesValidation="false" Text="..."
                                    OnClick="btnOwnerEMBGSearch_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Panel ID="pnlClient" runat="server" Visible="false">
                                    <asp:DetailsView ID="ClientDetailsView" runat="server" DataSourceID="ClientdvDataSource"
                                        DataKeyNames="ID" AutoGenerateRows="False" OnItemCommand="ClientDetailsView_ItemCommand"
                                        OnItemUpdated="ClientDetailsView_ItemUpdated" OnModeChanging="ClientDetailsView_ModeChanging"
                                        GridLines="None" DefaultMode="Edit">
                                        <Fields>
                                            <asp:TemplateField>
                                                <EditItemTemplate>
                                                    <div class="subTitles">
                                                        Измена на податоци за клиент
                                                    </div>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Име и Презиме">
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="tbName" runat="server" Text='<%# Bind("Name") %>' MaxLength="100"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="tbName"
                                                        Display="Dynamic" ErrorMessage="*">
                                                        <super:EntityCallOutValidator ID="NameUpdateValidator" PropertyName="NAME" runat="server" />
                                                    </asp:RequiredFieldValidator>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Матичен Број">
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="tbEMBG" runat="server" Text='<%# Bind("EMBG") %>' MaxLength="100"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvEMBG" runat="server" ControlToValidate="tbEMBG"
                                                        ErrorMessage="*"></asp:RequiredFieldValidator>
                                                    <super:EntityCallOutValidator ID="EmbgUpdateValidator" PropertyName="EMBG" runat="server" />
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Адреса">
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="tbAddress" MaxLength="100" runat="server" Text='<%# Bind("Address") %>'></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvAddress" runat="server" ControlToValidate="tbAddress"
                                                        ErrorMessage="*"></asp:RequiredFieldValidator>
                                                    <super:EntityCallOutValidator ID="AddressUpdateValidator" PropertyName="ADDRESS"
                                                        runat="server" />
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Населено Место">
                                                <EditItemTemplate>
                                                    <asp:DropDownList ID="ddlPlaces" runat="server" DataSourceID="odsActivePlaces" DataTextField="Name"
                                                        DataValueField="ID" SelectedValue='<%# Bind("PlaceID") %>'>
                                                    </asp:DropDownList>
                                                    <asp:ObjectDataSource ID="odsActivePlaces" runat="server" SelectMethod="GetActivePlaces"
                                                        TypeName="Broker.DataAccess.Place"></asp:ObjectDataSource>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Телефон">
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="tbPhone" MaxLength="30" runat="server" Text='<%# Bind("Phone") %>'></asp:TextBox>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Мобилен Тел.">
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="tbMobile" MaxLength="30" runat="server" Text='<%# Bind("Mobile") %>'></asp:TextBox>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Факс">
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="tbFax" MaxLength="30" runat="server" Text='<%# Bind("Fax") %>'></asp:TextBox>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Email">
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="tbEmail" MaxLength="100" runat="server" Text='<%# Bind("Email") %>'></asp:TextBox>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Правно Лице">
                                                <EditItemTemplate>
                                                    <asp:CheckBox ID="cbIsLaw" runat="server" Checked='<%#Bind("IsLaw") %>' />
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <EditItemTemplate>
                                                    <asp:Button ID="btnCancel" runat="server" CommandName="Cancel" Text="Откажи" CausesValidation="false" />
                                                    <asp:Button ID="btnUpdate" runat="server" CommandName="Update" Text="Измени" CausesValidation="false" />
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                        </Fields>
                                    </asp:DetailsView>
                                    <cc1:DetailsViewDataSource ID="ClientdvDataSource" runat="server" TypeName="Broker.DataAccess.Client"
                                        ConflictDetection="CompareAllValues" DataObjectTypeName="Broker.DataAccess.Client"
                                        DeleteMethod="Delete" InsertMethod="Insert" SelectMethod="GetByEmbg" UpdateMethod="Update">
                                        <UpdateParameters>
                                            <asp:Parameter Name="oldEntity" Type="Object" />
                                            <asp:Parameter Name="newEntity" Type="Object" />
                                        </UpdateParameters>
                                    </cc1:DetailsViewDataSource>
                                </asp:Panel>
                            </td>
                            <td colspan="2">
                                <asp:Panel ID="pnlOwner" runat="server" Visible="false">
                                    <asp:DetailsView ID="OwnerDetailsView" runat="server" DataSourceID="OwnerdvDataSource"
                                        DataKeyNames="ID" AutoGenerateRows="False" OnItemCommand="OwnerDetailsView_ItemCommand"
                                        OnItemUpdated="OwnerDetailsView_ItemUpdated" OnModeChanging="OwnerDetailsView_ModeChanging"
                                        GridLines="None" DefaultMode="Insert">
                                        <Fields>
                                            <asp:TemplateField>
                                                <EditItemTemplate>
                                                    <div class="subTitles">
                                                        Измена на податоци за клиент
                                                    </div>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Име и Презиме">
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="tbName" runat="server" Text='<%# Bind("Name") %>' MaxLength="100"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="tbName"
                                                        Display="Dynamic" ErrorMessage="*">
                                                    </asp:RequiredFieldValidator>
                                                    <super:EntityCallOutValidator ID="NameUpdateValidator" PropertyName="OwnerNAME" runat="server" />
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Матичен Број">
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="tbEMBG" runat="server" Text='<%# Bind("EMBG") %>' MaxLength="100"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvEMBG" runat="server" ControlToValidate="tbEMBG"
                                                        ErrorMessage="*"></asp:RequiredFieldValidator>
                                                    <super:EntityCallOutValidator ID="EmbgUpdateValidator" PropertyName="OwnerEMBG" runat="server" />
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Адреса">
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="tbAddress" MaxLength="100" runat="server" Text='<%# Bind("Address") %>'></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvAddress" runat="server" ControlToValidate="tbAddress"
                                                        ErrorMessage="*"></asp:RequiredFieldValidator>
                                                    <super:EntityCallOutValidator ID="AddressUpdateValidator" PropertyName="OwnerADDRESS"
                                                        runat="server" />
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Населено Место">
                                                <EditItemTemplate>
                                                    <asp:DropDownList ID="ddlPlaces" runat="server" DataSourceID="odsActivePlaces" DataTextField="Name"
                                                        DataValueField="ID" SelectedValue='<%# Bind("PlaceID") %>'>
                                                    </asp:DropDownList>
                                                    <asp:ObjectDataSource ID="odsActivePlaces" runat="server" SelectMethod="GetActivePlaces"
                                                        TypeName="Broker.DataAccess.Place"></asp:ObjectDataSource>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Телефон">
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="tbPhone" MaxLength="30" runat="server" Text='<%# Bind("Phone") %>'></asp:TextBox>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Мобилен Тел.">
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="tbMobile" MaxLength="30" runat="server" Text='<%# Bind("Mobile") %>'></asp:TextBox>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Факс">
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="tbFax" MaxLength="30" runat="server" Text='<%# Bind("Fax") %>'></asp:TextBox>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Email">
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="tbEmail" MaxLength="100" runat="server" Text='<%# Bind("Email") %>'></asp:TextBox>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Правно лице">
                                                <EditItemTemplate>
                                                    <asp:CheckBox ID="cbIsLaw" runat="server" Checked='<%#Bind("IsLaw") %>' />
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <EditItemTemplate>
                                                    <asp:Button ID="btnCancel" runat="server" CommandName="Cancel" Text="Откажи" CausesValidation="false" />
                                                    <asp:Button ID="btnUpdate" runat="server" CommandName="Update" Text="Внеси" CausesValidation="false" />
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                        </Fields>
                                    </asp:DetailsView>
                                    <cc1:DetailsViewDataSource ID="OwnerdvDataSource" runat="server" TypeName="Broker.DataAccess.Client"
                                        ConflictDetection="CompareAllValues" DataObjectTypeName="Broker.DataAccess.Client"
                                        DeleteMethod="Delete" InsertMethod="Insert" SelectMethod="GetByEmbg" UpdateMethod="Update">
                                        <UpdateParameters>
                                            <asp:Parameter Name="oldEntity" Type="Object" />
                                            <asp:Parameter Name="newEntity" Type="Object" />
                                        </UpdateParameters>
                                    </cc1:DetailsViewDataSource>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 160">
                                <asp:Label ID="lblClientName" runat="server" Text="Договорувач"></asp:Label>
                            </td>
                            <td style="width: 187px">
                                <asp:TextBox ID="tbClientName" Text='<%# Eval("Policy.Client.Name") %>' runat="server"></asp:TextBox>
                            </td>
                            <td style="width: 160px">
                                <asp:Label ID="lblOwnerName" runat="server" Text="Осигуреник"></asp:Label>
                            </td>
                            <td style="width: 187px">
                                <asp:TextBox ID="tbOwnerName" Text='<%# Eval("Policy.Client1.Name") %>' runat="server"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    <table width="695px">
                        <tr class="light-background">
                            <td style="width: 160px">
                                <asp:Label ID="lblStartDate" runat="server" Text="Почетна Дата:"></asp:Label>
                            </td>
                            <td style="width: 187px">
                                <asp:TextBox ID="tbStartDate" runat="server" Text='<%# Eval("Policy.StartDate", "{0:d}") %>'></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvStartDate" runat="server" ControlToValidate="tbStartDate"
                                    Display="Dynamic" ErrorMessage="*"></asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="cvStartDate" runat="server" ControlToValidate="tbStartDate"
                                    Display="Dynamic" ErrorMessage="не е дата" Operator="DataTypeCheck" Type="Date"></asp:CompareValidator>
                            </td>
                            <td style="width: 160px">
                                <asp:Label ID="lblEndDate" runat="server" Text="Крајна Дата:"></asp:Label>
                            </td>
                            <td style="width: 187px">
                                <asp:TextBox ID="tbEndDate" runat="server" Text='<%# Eval("Policy.EndDate","{0:d}") %>'></asp:TextBox>
                                <%--<asp:CompareValidator ID="cvEndDate" runat="server" ControlToCompare="tbStartDate"
                                    ControlToValidate="tbEndDate" Display="Dynamic" ErrorMessage="помало од стартна дата"
                                    Operator="GreaterThanEqual"></asp:CompareValidator>--%>
                                <asp:CompareValidator ID="cvEndDate2" runat="server" ControlToValidate="tbEndDate"
                                    ErrorMessage="не е дата" Operator="DataTypeCheck" Type="Date"></asp:CompareValidator>
                            </td>
                        </tr>
                    </table>
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <EditItemTemplate>
                    <table>
                        <tr>
                            <td style="width: 160px">
                                <asp:Label ID="lblApplicationDate" runat="server" Text="Дата на издавање"></asp:Label>
                            </td>
                            <td style="width: 187px">
                                <asp:TextBox ID="tbApplicationDate" runat="server" Text='<%# Eval("Policy.ApplicationDate","{0:d}") %>'></asp:TextBox>
                            </td>
                            <td style="width: 160px">
                                <asp:Label ID="lblStatus" runat="server" Text="Статус"></asp:Label>
                            </td>
                            <td style="width: 187px">
                                <asp:DropDownList ID="ddlStatuses" runat="server" DataSourceID="odsStatuses" DataTextField="Description"
                                    DataValueField="ID" SelectedValue='<%# Eval("StatusID") %>'>
                                </asp:DropDownList>
                                <asp:ObjectDataSource ID="odsStatuses" runat="server" SelectMethod="Select" TypeName="Broker.DataAccess.Statuse">
                                </asp:ObjectDataSource>
                            </td>
                        </tr>
                    </table>
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <EditItemTemplate>
                    <table>
                        <tr>
                            <td>
                                <asp:Panel ID="pnlExtendControls" runat="server">
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </EditItemTemplate>
            </asp:TemplateField>
        </Fields>
    </asp:DetailsView>
    <cc1:DetailsViewDataSource ID="dvDataSource" runat="server" TypeName="Broker.DataAccess.PolicyItem"
        ConflictDetection="CompareAllValues" DataObjectTypeName="Broker.DataAccess.PolicyItem"
        DeleteMethod="Delete" InsertMethod="Insert" UpdateMethod="Update" OnUpdating="dvDataSource_Updating"
        OnUpdated="dvDataSource_Updated" OldValuesParameterFormatString="oldEntity" SelectMethod="GetByNumberAndInsuranceSubType">
        <UpdateParameters>
            <asp:Parameter Name="oldEntity" Type="Object" />
            <asp:Parameter Name="newEntity" Type="Object" />
        </UpdateParameters>
        <SelectParameters>
            <asp:ControlParameter ControlID="tbPolicyNumber" Name="policyNumber" PropertyName="Text"
                Type="String" />
            <asp:ControlParameter ControlID="ddlInsuranceSubTypes" Name="insuranceSubTypeID"
                PropertyName="SelectedValue" Type="Int32" />
            <asp:ControlParameter ControlID="ddlInsuranceCompany" Name="insuranceCompanyID" PropertyName="SelectedValue"
                Type="Int32" />
        </SelectParameters>
    </cc1:DetailsViewDataSource>
    <asp:Button ID="btnUpdate" runat="server" Text="Измени" OnClick="btnUpdate_Click" />
</asp:Content>
