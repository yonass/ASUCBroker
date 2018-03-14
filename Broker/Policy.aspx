<%@ Page Title="Евидентирање на полиса" Language="C#" MasterPageFile="~/MasterPage.master"
    AutoEventWireup="true" CodeFile="Policy.aspx.cs" Inherits="Broker_Policy" Culture="mk-MK"
    UICulture="mk-MK" %>

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

        function confirm_delete() {
            if (confirm("Дали сте сигурни дека сакате да ја сторнирате полисата?") == true)
                return true;
            else
                return false;
        }

    </script>

    <div class="border1px_Policy">
        <%--<asp:Label ID="lblOfferNumber" runat="server" Text="Áðî¼ íà ïîíóäà"></asp:Label>
        <asp:TextBox ID="tbOfferNumber" runat="server"></asp:TextBox>
        <asp:Button ID="btnOfferNumber" runat="server" Text="..." OnClick="btnOfferNumber_Click"
            CausesValidation="False" />--%>
        <asp:MultiView ID="PolicyMultiView" runat="server">
            <asp:View ID="MainView" runat="server">
                <asp:DetailsView ID="PoliciesDetailsView" runat="server" DataKeyNames="ID" DefaultMode="Insert"
                    DataSourceID="dvDataSource" AutoGenerateRows="False" Width="330px" OnItemInserted="PolicyDetailesView_ItemInserted"
                    OnItemInserting="PolicyDetailesView_ItemInserting" GridLines="None">
                    <Fields>
                        <asp:TemplateField>
                            <InsertItemTemplate>
                                <div class="sec_policy">
                                    <table width="auto">
                                        <tr class="light-background">
                                            <td class="policy_label_td">
                                                <asp:Label ID="lblInsuranceCompanies" runat="server" Text="Осигурителна компанија"></asp:Label>
                                            </td>
                                            <td class="policy_field_td">
                                                <asp:DropDownList ID="ddlInsuranceCompany" runat="server" DataSourceID="odsInsuranceCompany"
                                                    DataTextField="Name" DataValueField="ID" SelectedValue='<%# Bind("InsuranceCompanyID") %>'
                                                    CssClass="select" OnSelectedIndexChanged="ddlInsuranceCompanyIndexChanged" AutoPostBack="true">
                                                </asp:DropDownList>
                                                <asp:ObjectDataSource ID="odsInsuranceCompany" runat="server" SelectMethod="GetByActiveDeals"
                                                    TypeName="Broker.DataAccess.InsuranceCompany">
                                                    <%--<SelectParameters>
                                                        <asp:ControlParameter ControlID="ddlInsuranceSubTypes" Name="insuranceSubTypeID"
                                                            PropertyName="SelectedValue" Type="Int32" />
                                                    </SelectParameters>--%>
                                                </asp:ObjectDataSource>
                                            </td>
                                            <td class="policy_button_td">
                                            </td>
                                            <td class="policy_label_td">
                                                <asp:Label ID="lblDeal" runat="server" Text="Договор"></asp:Label>
                                            </td>
                                            <td class="policy_field_td">
                                                <asp:DropDownList ID="ddlDeals" runat="server" DataSourceID="odsDeals" DataTextField="Description"
                                                    CssClass="select normal" DataValueField="ID" AutoPostBack="true" OnSelectedIndexChanged="ddlDealsSelectedIndexChanged">
                                                </asp:DropDownList>
                                                <asp:ObjectDataSource ID="odsDeals" runat="server" SelectMethod="GetActiveDealsForCompanyAndInsuranceSubType"
                                                    TypeName="Broker.DataAccess.Deal">
                                                    <SelectParameters>
                                                        <asp:ControlParameter ControlID="ddlInsuranceCompany" Name="InsuranceCompanyID" PropertyName="SelectedValue"
                                                            Type="Int32" />
                                                        <asp:ControlParameter ControlID="ddlInsuranceSubTypes" Name="InsuranceSubTypeID"
                                                            PropertyName="SelectedValue" Type="Int32" />
                                                    </SelectParameters>
                                                </asp:ObjectDataSource>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="policy_label_td">
                                                <asp:Label ID="lblInsuranceType" runat="server" Text="Тип на осигурување"></asp:Label>
                                            </td>
                                            <td class="policy_field_td">
                                                <asp:DropDownList ID="ddlInsuranceType" runat="server" DataSourceID="odsInsuranceType"
                                                    CssClass="select" DataTextField="ShortName" DataValueField="ID" AutoPostBack="true"
                                                    OnSelectedIndexChanged="ddlInsuranceTypeSelectedIndexChanged">
                                                </asp:DropDownList>
                                                <asp:ObjectDataSource ID="odsInsuranceType" runat="server" SelectMethod="GetByCompany"
                                                    TypeName="Broker.DataAccess.InsuranceType">
                                                    <SelectParameters>
                                                        <asp:ControlParameter ControlID="ddlInsuranceCompany" Name="companyID" PropertyName="SelectedValue"
                                                            Type="Int32" />
                                                    </SelectParameters>
                                                </asp:ObjectDataSource>
                                            </td>
                                            <td class="policy_button_td">
                                            </td>
                                            <td class="policy_label_td">
                                                <asp:Label ID="lblInsuranceSubType" runat="server" Text="Подтип на осигурување"></asp:Label>
                                            </td>
                                            <td class="policy_field_td">
                                                <asp:DropDownList ID="ddlInsuranceSubTypes" runat="server" DataSourceID="odsInsuranceSubTypes"
                                                    CssClass="select" DataTextField="ShortDescription" DataValueField="ID" AutoPostBack="true"
                                                    OnSelectedIndexChanged="ddlInsuranceSubType_selecteIndexChanged">
                                                </asp:DropDownList>
                                                <asp:ObjectDataSource ID="odsInsuranceSubTypes" runat="server" SelectMethod="GetByInsuranceTypeAndCompany"
                                                    TypeName="Broker.DataAccess.InsuranceSubType">
                                                    <SelectParameters>
                                                        <asp:ControlParameter ControlID="ddlInsuranceType" Name="insuranceTypeID" PropertyName="SelectedValue"
                                                            Type="Int32" />
                                                        <asp:ControlParameter ControlID="ddlInsuranceCompany" Name="companyID" PropertyName="SelectedValue"
                                                            Type="Int32" />
                                                    </SelectParameters>
                                                </asp:ObjectDataSource>
                                            </td>
                                        </tr>
                                        <tr class="light-background">
                                            <td class="policy_label_td">
                                                <asp:Label ID="lblPolicyNumber" runat="server" Text="Број на полиса"></asp:Label>
                                            </td>
                                            <td class="policy_field_td">
                                                <asp:TextBox ID="tbPolicyNumber" CssClass="tekstPole" runat="server"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvPolicyNumber" runat="server" ControlToValidate="tbPolicyNumber"
                                                    Display="Dynamic" ErrorMessage="*"></asp:RequiredFieldValidator>
                                                <super:EntityCallOutValidator ID="PolicyValidatorValidator" PropertyName="PolicyNumberDistributionValidator"
                                                    runat="server" />
                                                <super:EntityCallOutValidator ID="PolicyValidatorValidator1" PropertyName="PolicyNumberDistributionValidator1"
                                                    runat="server" />
                                                <asp:Button ID="btnSEPolNumberCheck" CssClass="normal_button small_button" runat="server"
                                                    Text="..." OnClick="tbPolicyNumber_Changed" CausesValidation="false" />
                                                <asp:Button ID="btnNBOSearch" CssClass="normal_button small_button" runat="server"
                                                    Text="..." OnClick="btnNBOSearch_Click" CausesValidation="false" />
                                            </td>
                                            <td class="policy_button_td">
                                            </td>
                                            <td>
                                            </td>
                                            <td style="text-align: right;">
                                                <asp:Button ID="btnDiscard" CssClass="normal_button" OnClick="btnDiscard_Click" runat="server"
                                                    Text="Сторнирај" CausesValidation="false" />
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div class="clear">
                                </div>
                                <asp:Panel ID="pnlRegistrationNumberSearch" runat="server">
                                    <div class="sec_policy">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblRegistrationNumberForSearh" runat="server" Text="Регистрација"></asp:Label>
                                                    <asp:TextBox ID="tbRegNumberForSearch" CssClass="tekstPole" runat="server"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnGetByRegNumber" runat="server" CssClass="normal_button" CausesValidation="false"
                                                        Text="Барај" OnClick="btnGetByRegNumber_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </asp:Panel>
                                <asp:Panel ID="pnlSearcPreviousPolicy" runat="server">
                                    <div class="sec_policy">
                                        <table>
                                            <tr>
                                                <td class="policy_label_td">
                                                    Пребарување по:
                                                </td>
                                                <td>
                                                    <asp:RadioButtonList ID="rblSeachParameter" TextAlign="Left" runat="server" DataSourceID="odsSearchParameter"
                                                        DataTextField="LabelName" DataValueField="ID" RepeatDirection="Horizontal">
                                                    </asp:RadioButtonList>
                                                    <asp:ObjectDataSource runat="server" ID="odsSearchParameter" DataObjectTypeName="Broker.DataAccess.Control"
                                                        TypeName="Broker.DataAccess.Control" SelectMethod="SelectSearchParametersExtend">
                                                        <SelectParameters>
                                                            <asp:ControlParameter ControlID="ddlInsuranceSubTypes" Type="Int32" Name="insuranceSubTypeID"
                                                                PropertyName="SelectedValue" />
                                                        </SelectParameters>
                                                    </asp:ObjectDataSource>
                                                </td>
                                            </tr>
                                            <tr class="light-background">
                                                <td class="policy_label_td">
                                                    Пребарај:
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="tbSearchParameters" CssClass="tekstPole" runat="server"></asp:TextBox>
                                                    <asp:Button ID="btnSearcPreviousPolicy" CssClass="normal_button" runat="server" CausesValidation="false"
                                                        Text="Барај" OnClick="btnSearcPreviousPolicy_Click" />
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlFoundPolicyItems" runat="server" DataTextField="PolicyNumber"
                                                        CssClass="select" DataValueField="ID" Visible="false" AutoPostBack="true" OnSelectedIndexChanged="ddlFoundPolicyItems_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </asp:Panel>
                                <div class="sec_policy">
                                    <table width="auto">
                                        <tr>
                                            <td>
                                                <b>Податоци за договорувач, осигуреник и скаденца на полиса</b>
                                            </td>
                                        </tr>
                                    </table>
                                    <table>
                                        <tr class="light-background">
                                            <td class="policy_label_td">
                                                <asp:Label ID="lblClientEmbg" runat="server" Text="МБ. Договорувач"></asp:Label>
                                            </td>
                                            <td class="policy_field_td">
                                                <asp:TextBox ID="tbClientEMBG" CssClass="tekstPole" runat="server"></asp:TextBox><asp:Button
                                                    ID="btnClientEMBGSearch" CssClass="normal_button small_button" runat="server"
                                                    CausesValidation="false" Text="..." OnClick="btnClientEMBGSearch_Click" />
                                            </td>
                                            <td class="policy_button_td">
                                                <asp:Button ID="btnClientToOwner" CssClass="normal_button small_button" Text="->"
                                                    runat="server" OnClick="btnClientToOwner_Click" CausesValidation="false" />
                                            </td>
                                            <td class="policy_label_td">
                                                <asp:Label ID="lblOwnerEmbg" runat="server" Text="МБ. Осигуреник"></asp:Label>
                                            </td>
                                            <td class="policy_field_td">
                                                <asp:TextBox ID="tbOwnerEMBG" CssClass="tekstPole" runat="server"></asp:TextBox><asp:Button
                                                    ID="btnOwnerEMBGSearch" CssClass="normal_button small_button" runat="server"
                                                    CausesValidation="false" Text="..." OnClick="btnOwnerEMBGSearch_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Panel ID="pnlClient" runat="server" Visible="false">
                                                    <asp:DetailsView ID="ClientDetailsView" runat="server" DataSourceID="ClientdvDataSource"
                                                        DataKeyNames="ID" AutoGenerateRows="False" OnItemCommand="ClientDetailsView_ItemCommand"
                                                        OnItemDeleted="ClientDetailsView_ItemDeleted" OnItemInserted="ClientDetailsView_ItemInserted"
                                                        OnItemUpdated="ClientDetailsView_ItemUpdated" OnModeChanging="ClientDetailsView_ModeChanging"
                                                        OnItemInserting="ClientDetailsView_ItemInserting" GridLines="None" DefaultMode="Insert">
                                                        <Fields>
                                                            <asp:TemplateField>
                                                                <InsertItemTemplate>
                                                                    <tr>
                                                                        <td>
                                                                            <b>Нов Клиент</b>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            Тип на клиент
                                                                        </td>
                                                                        <td>
                                                                            <asp:DropDownList ID="ddlClientTypes" runat="server" CssClass="select normal">
                                                                                <asp:ListItem Selected="True" Text="Физичко лице" Value="Private"></asp:ListItem>
                                                                                <asp:ListItem Text="Правно лице" Value="Law"></asp:ListItem>
                                                                                <asp:ListItem Text="Станец-физичко" Value="ForeignPrivate"></asp:ListItem>
                                                                                <asp:ListItem Text="Станец-правно" Value="ForeignLaw"></asp:ListItem>
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            Матичен Број
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="tbEMBG" CssClass="tekstPole" runat="server" Text='<%# Bind("EMBG") %>'
                                                                                MaxLength="100"></asp:TextBox><asp:RequiredFieldValidator ID="rfvEMBG" runat="server"
                                                                                    ControlToValidate="tbEMBG" ErrorMessage="*"></asp:RequiredFieldValidator><super:EntityCallOutValidator
                                                                                        ID="EmbgInsertValidator" PropertyName="EMBG" runat="server" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            Име и Презиме
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="tbName" CssClass="tekstPole" runat="server" Text='<%# Bind("Name") %>'
                                                                                MaxLength="100"></asp:TextBox>
                                                                            <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="tbName"
                                                                                Display="Dynamic" ErrorMessage="*">
                                                                            </asp:RequiredFieldValidator>
                                                                            <super:EntityCallOutValidator ID="NameInsertValidator" PropertyName="NAME" runat="server" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            Адреса
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="tbAddress" CssClass="tekstPole" MaxLength="100" runat="server" Text='<%# Bind("Address") %>'></asp:TextBox><asp:RequiredFieldValidator
                                                                                ID="rfvAddress" runat="server" ControlToValidate="tbAddress" ErrorMessage="*"></asp:RequiredFieldValidator><super:EntityCallOutValidator
                                                                                    ID="AddressInsertValidator" PropertyName="ADDRESS" runat="server" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            Населено Место
                                                                        </td>
                                                                        <td>
                                                                            <asp:DropDownList ID="ddlPlaces" runat="server" DataSourceID="odsActivePlaces" DataTextField="Name"
                                                                                DataValueField="ID" SelectedValue='<%# Bind("PlaceID") %>' CssClass="select normal">
                                                                            </asp:DropDownList>
                                                                            <asp:ObjectDataSource ID="odsActivePlaces" runat="server" SelectMethod="GetActivePlaces"
                                                                                TypeName="Broker.DataAccess.Place"></asp:ObjectDataSource>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            Телефон
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="tbPhone" CssClass="tekstPole" MaxLength="30" runat="server" Text='<%# Bind("Phone") %>'></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            Мобилен Тел.
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="tbMobile" CssClass="tekstPole" MaxLength="30" runat="server" Text='<%# Bind("Mobile") %>'></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            Факс
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="tbFax" CssClass="tekstPole" MaxLength="30" runat="server" Text='<%# Bind("Fax") %>'></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            Email
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="tbEmail" CssClass="tekstPole" MaxLength="100" runat="server" Text='<%# Bind("Email") %>'></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Button ID="btnCancel" CssClass="cancel" runat="server" CommandName="Cancel"
                                                                                Text="Откажи" CausesValidation="false" />
                                                                        </td>
                                                                        <td>
                                                                            <asp:Button ID="btnInsert" CssClass="submit" runat="server" CommandName="Insert"
                                                                                Text="Внеси" CausesValidation="false" />
                                                                        </td>
                                                                    </tr>
                                                                </InsertItemTemplate>
                                                            </asp:TemplateField>
                                                        </Fields>
                                                    </asp:DetailsView>
                                                    <cc1:DetailsViewDataSource ID="ClientdvDataSource" runat="server" TypeName="Broker.DataAccess.Client"
                                                        ConflictDetection="CompareAllValues" DataObjectTypeName="Broker.DataAccess.Client"
                                                        DeleteMethod="Delete" InsertMethod="Insert" SelectMethod="GetByEmbg" UpdateMethod="Update"
                                                        OnUpdating="ClientdvDataSource_Updating" OnUpdated="ClientdvDataSource_Updated"
                                                        OnInserted="ClientdvDataSource_Inserted" OnInserting="ClientdvDataSource_Inserting">
                                                        <InsertParameters>
                                                            <asp:Parameter Name="entityToInsert" Type="Object" />
                                                        </InsertParameters>
                                                    </cc1:DetailsViewDataSource>
                                                </asp:Panel>
                                            </td>
                                            <td>
                                                <asp:Panel ID="pnlOwner" runat="server" Visible="false">
                                                    <asp:DetailsView ID="OwnerDetailsView" runat="server" DataSourceID="OwnerdvDataSource"
                                                        DataKeyNames="ID" AutoGenerateRows="False" OnItemCommand="OwnerDetailsView_ItemCommand"
                                                        OnItemDeleted="OwnerDetailsView_ItemDeleted" OnItemInserted="OwnerDetailsView_ItemInserted"
                                                        OnItemUpdated="OwnerDetailsView_ItemUpdated" OnModeChanging="OwnerDetailsView_ModeChanging"
                                                        OnItemInserting="OwnerDetailsView_ItemInserting" GridLines="None" DefaultMode="Insert">
                                                        <Fields>
                                                            <asp:TemplateField HeaderStyle-Width="120px">
                                                                <InsertItemTemplate>
                                                                    <tr>
                                                                        <td>
                                                                            Нов Клиент
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            Тип на клиент
                                                                        </td>
                                                                        <td>
                                                                            <asp:DropDownList ID="ddlClientTypes" runat="server" CssClass="select normal">
                                                                                <asp:ListItem Selected="True" Text="Физичко лице" Value="Private"></asp:ListItem>
                                                                                <asp:ListItem Text="Правно лице" Value="Law"></asp:ListItem>
                                                                                <asp:ListItem Text="Станец-физичко" Value="ForeignPrivate"></asp:ListItem>
                                                                                <asp:ListItem Text="Станец-правно" Value="ForeignLaw"></asp:ListItem>
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            Матичен Број
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="tbEMBG" CssClass="tekstPole" runat="server" Text='<%# Bind("EMBG") %>'
                                                                                MaxLength="100"></asp:TextBox><asp:RequiredFieldValidator ID="rfvEMBG" runat="server"
                                                                                    ControlToValidate="tbEMBG" ErrorMessage="*"></asp:RequiredFieldValidator><super:EntityCallOutValidator
                                                                                        ID="EmbgInsertValidator" PropertyName="OwnerEMBG" runat="server" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            Име и Презиме
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="tbName" runat="server" CssClass="tekstPole" Text='<%# Bind("Name") %>'
                                                                                MaxLength="100"></asp:TextBox><asp:RequiredFieldValidator ID="rfvName" runat="server"
                                                                                    ControlToValidate="tbName" Display="Dynamic" ErrorMessage="*"> </asp:RequiredFieldValidator>
                                                                            <super:EntityCallOutValidator ID="NameInsertValidator" PropertyName="OwnerNAME" runat="server" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            Адреса
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="tbAddress" CssClass="tekstPole" MaxLength="100" runat="server" Text='<%# Bind("Address") %>'></asp:TextBox><asp:RequiredFieldValidator
                                                                                ID="rfvAddress" runat="server" ControlToValidate="tbAddress" ErrorMessage="*"></asp:RequiredFieldValidator><super:EntityCallOutValidator
                                                                                    ID="AddressInsertValidator" PropertyName="OwnerADDRESS" runat="server" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            Населено Место
                                                                        </td>
                                                                        <td>
                                                                            <asp:DropDownList ID="ddlPlaces" runat="server" DataSourceID="odsActivePlaces" DataTextField="Name"
                                                                                DataValueField="ID" SelectedValue='<%# Bind("PlaceID") %>' CssClass="select normal">
                                                                            </asp:DropDownList>
                                                                            <asp:ObjectDataSource ID="odsActivePlaces" runat="server" SelectMethod="GetActivePlaces"
                                                                                TypeName="Broker.DataAccess.Place"></asp:ObjectDataSource>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            Телефон
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="tbPhone" CssClass="tekstPole" MaxLength="30" runat="server" Text='<%# Bind("Phone") %>'></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            Мобилен Тел
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="tbMobile" CssClass="tekstPole" MaxLength="30" runat="server" Text='<%# Bind("Mobile") %>'></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            Факс
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="tbFax" CssClass="tekstPole" MaxLength="30" runat="server" Text='<%# Bind("Fax") %>'></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            Email
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="tbEmail" CssClass="tekstPole" MaxLength="100" runat="server" Text='<%# Bind("Email") %>'></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Button ID="btnCancel" CssClass="cancel" runat="server" CommandName="Cancel"
                                                                                Text="Откажи" CausesValidation="false" />
                                                                        </td>
                                                                        <td>
                                                                            <asp:Button ID="btnInsert" CssClass="submit" runat="server" CommandName="Insert"
                                                                                Text="Внеси" CausesValidation="false" />
                                                                        </td>
                                                                    </tr>
                                                                </InsertItemTemplate>
                                                            </asp:TemplateField>
                                                        </Fields>
                                                    </asp:DetailsView>
                                                    <cc1:DetailsViewDataSource ID="OwnerdvDataSource" runat="server" TypeName="Broker.DataAccess.Client"
                                                        ConflictDetection="CompareAllValues" DataObjectTypeName="Broker.DataAccess.Client"
                                                        DeleteMethod="Delete" InsertMethod="Insert" SelectMethod="GetByEmbg" UpdateMethod="Update"
                                                        OnUpdating="OwnerdvDataSource_Updating" OnUpdated="OwnerdvDataSource_Updated"
                                                        OnInserted="OwnerdvDataSource_Inserted" OnInserting="OwnerdvDataSource_Inserting">
                                                        <InsertParameters>
                                                            <asp:Parameter Name="entityToInsert" Type="Object" />
                                                        </InsertParameters>
                                                    </cc1:DetailsViewDataSource>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </table>
                                    <table>
                                        <tr>
                                            <td class="policy_label_td">
                                                <asp:Label ID="lblClientName" runat="server" Text="Договорувач"></asp:Label>
                                            </td>
                                            <td class="policy_field_td">
                                                <asp:TextBox ID="tbClientName" CssClass="tekstPole" runat="server"></asp:TextBox>
                                            </td>
                                            <td class="policy_button_td">
                                                <asp:Button ID="btnOwnerToCLient" CssClass="normal_button small_button" Text="<-"
                                                    runat="server" OnClick="btnOwnerToClient_Click" CausesValidation="false" />
                                            </td>
                                            <td class="policy_label_td">
                                                <asp:Label ID="lblOwnerName" runat="server" Text="Осигуреник"></asp:Label>
                                            </td>
                                            <td class="policy_field_td">
                                                <asp:TextBox ID="tbOwnerName" CssClass="tekstPole" runat="server"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr class="light-background">
                                            <td class="policy_label_td">
                                                <asp:Label ID="lblStartDate" runat="server" Text="Почетна Дата:"></asp:Label>
                                            </td>
                                            <td class="policy_field_td">
                                                <asp:TextBox ID="tbStartDate" CssClass="tekstPole" runat="server" Text='<%# Bind("StartDate", "{0:d}") %>'
                                                    AutoPostBack="true" OnTextChanged="tbStartDate_TextChanged"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvStartDate" runat="server" ControlToValidate="tbStartDate"
                                                    Display="Dynamic" ErrorMessage="*"></asp:RequiredFieldValidator>
                                                <asp:CompareValidator ID="cvStartDate" runat="server" ControlToValidate="tbStartDate"
                                                    Display="Dynamic" ErrorMessage="*" Operator="DataTypeCheck" Type="Date"></asp:CompareValidator>
                                            </td>
                                            <td class="policy_button_td">
                                            </td>
                                            <td class="policy_label_td">
                                                <asp:Label ID="lblEndDate" runat="server" Text="Крајна Дата:"></asp:Label>
                                            </td>
                                            <td class="policy_field_td">
                                                <asp:TextBox ID="tbEndDate" CssClass="tekstPole" runat="server" Text='<%# Bind("EndDate","{0:d}") %>'
                                                    AutoPostBack="true" OnTextChanged="tbEndDate_TextChanged"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvEndDate" runat="server" ControlToValidate="tbEndDate"
                                                    Display="Dynamic" ErrorMessage="*"></asp:RequiredFieldValidator>
                                                <asp:CompareValidator ID="cvEndDate2" runat="server" ControlToValidate="tbEndDate"
                                                    ErrorMessage="*" Operator="DataTypeCheck" Type="Date"></asp:CompareValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="policy_label_td">
                                                <asp:Label ID="lblApplicaionDate" runat="server" Text="Дата на издавање:"></asp:Label>
                                            </td>
                                            <td class="policy_field_td">
                                                <asp:TextBox ID="tbApplicationDate" CssClass="tekstPole" runat="server" Text='<%# Bind("ApplicationDate", "{0:d}") %>'
                                                    AutoPostBack="true" OnTextChanged="tbApplicationDate_TextChanged"></asp:TextBox><asp:RequiredFieldValidator
                                                        ID="rfvApplicationDate" runat="server" ControlToValidate="tbApplicationDate"
                                                        Display="Dynamic" ErrorMessage="*"></asp:RequiredFieldValidator><asp:CompareValidator
                                                            ID="cvApplicationDate" runat="server" ControlToValidate="tbApplicationDate" Display="Dynamic"
                                                            ErrorMessage="*" Operator="DataTypeCheck" Type="Date"></asp:CompareValidator>
                                            </td>
                                            <td class="policy_button_td">
                                            </td>
                                            <td class="policy_label_td">
                                                <asp:Label ID="lblMarketingAgent" runat="server" Text="Маркетинг Агент"></asp:Label>
                                            </td>
                                            <td class="policy_field_td">
                                                <asp:DropDownList CssClass="select normal" ID="ddlMarketingAgents" runat="server"
                                                    DataSourceID="odsMarketingAgent" DataTextField="Name" DataValueField="ID">
                                                    <%--AppendDataBoundItems="true">
                                                <asp:ListItem Text="" Value="0"></asp:ListItem>--%>
                                                </asp:DropDownList>
                                                <asp:ObjectDataSource ID="odsMarketingAgent" runat="server" TypeName="Broker.DataAccess.User"
                                                    SelectMethod="GetMarketingAgentsWithBrokerAgeForCompanyAndSubType">
                                                    <SelectParameters>
                                                        <asp:ControlParameter ControlID="ddlInsuranceCompany" Name="companyID" PropertyName="SelectedValue"
                                                            Type="Int32" />
                                                        <asp:ControlParameter ControlID="ddlInsuranceSubTypes" Name="insuranceSubTypeID"
                                                            PropertyName="SelectedValue" Type="Int32" />
                                                    </SelectParameters>
                                                </asp:ObjectDataSource>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </InsertItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <InsertItemTemplate>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblError" runat="server" Visible="false"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </InsertItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <InsertItemTemplate>
                                <div class="sec_policy">
                                    <table width="695px">
                                        <tr>
                                            <td>
                                                <asp:Panel ID="pnlExtendControls" runat="server">
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </table>
                                    <table width="695px" class="sec_policy">
                                        <tr>
                                            <td>
                                                <b>Начин на плаќање</b>&nbsp;&nbsp;
                                                <%--<asp:Label ID="lblPaymentPlace" runat="server" Text="Платено во готово"></asp:Label><asp:CheckBox
                                                    CssClass="checkBox" ID="cbPaymentPlace" runat="server" Checked="true" AutoPostBack="true"
                                                    OnCheckedChanged="Checked_Changed" />
                                            </td>
                                            <td>
                                                <asp:Panel ID="pnlFactureTypes" runat="server" Visible="false">
                                                    <asp:Label ID="lblFactureType" runat="server" Text="Фактура кон ос. компанија"></asp:Label>
                                                    <asp:CheckBox ID="cbFactureType" runat="server" Checked="false" CssClass="checkBox" />
                                                </asp:Panel>--%>
                                                <asp:RadioButtonList ID="rblPaymentTypes" runat="server" RepeatDirection="Horizontal"
                                                    AutoPostBack="true" OnSelectedIndexChanged="Checked_Changed">
                                                    <asp:ListItem Selected="True" Text="ЕДНОКРАТНО ПЛАЌАЊЕ" Value="PaidOnce"></asp:ListItem>
                                                    <asp:ListItem Text="ПЛАЌАЊЕ НА РАТИ" Value="PaidRates"></asp:ListItem>
                                                    <asp:ListItem Text="ДИРЕКТНО ВО О.КОМПАНИЈА" Value="PaidInInsuranceCompany"></asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                    </table>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Panel ID="PaymentTypePanel" runat="server">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                Тип на плаќање
                                                            </td>
                                                            <td>
                                                                <asp:RadioButtonList ID="rblPaymentTypeForOncePayment" runat="server" RepeatDirection="Horizontal"
                                                                    AutoPostBack="true" OnSelectedIndexChanged="rblPaymentTypeForOncePayment_SelectedIndexChanged">
                                                                    <asp:ListItem Selected="True" Text="Готовинско" Value="CashPayment"></asp:ListItem>
                                                                    <asp:ListItem Text="Фактурно" Value="FacturePayment"></asp:ListItem>
                                                                </asp:RadioButtonList>
                                                                <asp:CheckBox ID="cvIsForFacturingOncePaid" runat="server" Checked="true" Visible="false"
                                                                    Text="Се генерира фактура веднаш" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <table>
                                                        <tr>
                                                            <td style="width: 173px">
                                                                <asp:Label ID="lblCash" runat="server" Text="Готовина"></asp:Label><asp:TextBox ID="tbCash"
                                                                    CssClass="tekstPole" runat="server" Width="155px"></asp:TextBox><asp:CompareValidator
                                                                        ID="cvCash" runat="server" ControlToValidate="tbCash" ErrorMessage="*" Operator="DataTypeCheck"
                                                                        Type="Double"></asp:CompareValidator>
                                                            </td>
                                                            <td style="width: 173px">
                                                                <asp:Label ID="lblCreditCard" runat="server" Text="Картичка"></asp:Label><asp:TextBox
                                                                    ID="tbCreditCard" runat="server" CssClass="tekstPole" Width="155px"></asp:TextBox><asp:CompareValidator
                                                                        ID="cvCreditCard" runat="server" ControlToValidate="tbCreditCard" ErrorMessage="*"
                                                                        Operator="DataTypeCheck" Type="Double"></asp:CompareValidator>
                                                            </td>
                                                            <td style="width: 173px">
                                                                <asp:Label ID="lblBanks" runat="server" Text="Банка"></asp:Label><asp:DropDownList
                                                                    ID="ddlBank" runat="server" DataSourceID="odsBank" AutoPostBack="True" DataTextField="Name"
                                                                    DataValueField="ID" Width="155px" CssClass="select">
                                                                </asp:DropDownList>
                                                                <asp:ObjectDataSource ID="odsBank" runat="server" SelectMethod="GetBanksWithGreditCards"
                                                                    TypeName="Broker.DataAccess.Bank"></asp:ObjectDataSource>
                                                            </td>
                                                            <td style="width: 173px">
                                                                <asp:Label ID="lblCardTypes" runat="server" Text="Тип на Картичка"></asp:Label><asp:DropDownList
                                                                    CssClass="select normal" ID="ddlCardTypes" runat="server" DataSourceID="odsCardTypes"
                                                                    AutoPostBack="True" DataTextField="Name" DataValueField="ID" Width="155px">
                                                                </asp:DropDownList>
                                                                <asp:ObjectDataSource ID="odsCardTypes" runat="server" SelectMethod="GetByBank" TypeName="Broker.DataAccess.CreditCard">
                                                                    <SelectParameters>
                                                                        <asp:ControlParameter ControlID="ddlBank" Name="BankID" PropertyName="SelectedValue"
                                                                            Type="Int32" />
                                                                    </SelectParameters>
                                                                </asp:ObjectDataSource>
                                                                <super:EntityCallOutValidator ID="ValuesEntityCallOutValidator" PropertyName="ValuesValidator"
                                                                    runat="server" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Panel ID="pnlRates" runat="server" Visible="false">
                                                    <table>
                                                        <tr>
                                                            <td style="width: 200px;">
                                                                <b>Рати</b>
                                                            </td>
                                                            <td>
                                                            </td>
                                                            <td style="width: 200px;">
                                                            </td>
                                                            <td>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 200px;">
                                                                Број на рати
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlNumberOfRates" runat="server" AutoPostBack="true" CssClass="selectSmall"
                                                                    OnSelectedIndexChanged="ddlNumberOfRates_SelectedIndexChanged">
                                                                    <asp:ListItem Value="1" Text="1"></asp:ListItem>
                                                                    <asp:ListItem Value="2" Text="2"></asp:ListItem>
                                                                    <asp:ListItem Value="3" Text="3"></asp:ListItem>
                                                                    <asp:ListItem Value="4" Text="4"></asp:ListItem>
                                                                    <asp:ListItem Value="5" Text="5"></asp:ListItem>
                                                                    <asp:ListItem Value="6" Text="6"></asp:ListItem>
                                                                    <asp:ListItem Value="7" Text="7"></asp:ListItem>
                                                                    <asp:ListItem Value="8" Text="8"></asp:ListItem>
                                                                    <asp:ListItem Value="9" Text="9"></asp:ListItem>
                                                                    <asp:ListItem Value="10" Text="10"></asp:ListItem>
                                                                    <asp:ListItem Value="11" Text="11"></asp:ListItem>
                                                                    <asp:ListItem Value="12" Text="12"></asp:ListItem>
                                                                </asp:DropDownList>
                                                                <super:EntityCallOutValidator ID="superRateNumberValidator" PropertyName="RateNumberValidator"
                                                                    runat="server" />
                                                                <super:EntityCallOutValidator ID="superRateTotValuesValidator" PropertyName="RateTotValuesValidator"
                                                                    runat="server" />
                                                            </td>
                                                            <td style="width: 200px;">
                                                                Почетна дата на рати
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="tbStartDateForRates" CssClass="tekstPole" runat="server" AutoPostBack="true"
                                                                    OnTextChanged="DateChanged"></asp:TextBox>
                                                            </td>
                                                            <td style="width: 200px;">
                                                                Ратите ги одобрил
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlApproverOfRates" runat="server" DataSourceID="odsApproverOfRates"
                                                                    DataTextField="Name" DataValueField="ID" CssClass="select">
                                                                </asp:DropDownList>
                                                                <asp:ObjectDataSource ID="odsApproverOfRates" runat="server" SelectMethod="GetRatesApprovers"
                                                                    TypeName="Broker.DataAccess.User" DataObjectTypeName="Broker.DataAccess.User">
                                                                    <SelectParameters>
                                                                        <asp:SessionParameter SessionField="UserID" Name="userID" Type="Int32" />
                                                                    </SelectParameters>
                                                                </asp:ObjectDataSource>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 200px;">
                                                                Договор за рати
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlRateDeals" runat="server" CssClass="select" DataTextField="Name"
                                                                    DataValueField="ID" DataSourceID="odsRateDeals" AutoPostBack="true" OnSelectedIndexChanged="ddlRateDeals_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                                <asp:ObjectDataSource ID="odsRateDeals" runat="server" DataObjectTypeName="Broker.DataAccess.ViewRateDeal"
                                                                    TypeName="Broker.DataAccess.ViewRateDeal" SelectMethod="GetRateDealsForInsuranceSubType"
                                                                    OnSelecting="odsRateDeals_Selecting"></asp:ObjectDataSource>
                                                            </td>
                                                            <td>
                                                            </td>
                                                            <td>
                                                                <asp:CheckBox ID="cbIsForFacturing" runat="server" Checked="true" CssClass="checkBox"
                                                                    Text="Се фактурира" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </InsertItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <InsertItemTemplate>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Button ID="btnCancel" CssClass="cancel" runat="server" CommandName="Cancel"
                                                Text="Откажи" CausesValidation="false" />
                                        </td>
                                        <td>
                                            <asp:Button ID="btnInsert" CssClass="submit" runat="server" CommandName="Insert"
                                                Text="Сними" />
                                        </td>
                                        <%--<td>
                                    <asp:Button ID="btnPrintFacture" runat="server" OnClick="btnPrintFacture_Click" Enabled="false"
                                        Text="Ïå÷àòè ôàêòóðà" />
                                </td>--%>
                                    </tr>
                                </table>
                            </InsertItemTemplate>
                        </asp:TemplateField>
                    </Fields>
                </asp:DetailsView>
                <cc1:DetailsViewDataSource ID="dvDataSource" runat="server" TypeName="Broker.DataAccess.Policy"
                    ConflictDetection="CompareAllValues" DataObjectTypeName="Broker.DataAccess.Policy"
                    DeleteMethod="Delete" InsertMethod="Insert" UpdateMethod="Update" OnUpdating="dvDataSource_Updating"
                    OnUpdated="dvDataSource_Updated" OnInserted="dvDataSource_Inserted" OnInserting="dvDataSource_Inserting"
                    OldValuesParameterFormatString="oldEntity">
                    <%--<UpdateParameters>
                        <asp:Parameter Name="oldEntity" Type="Object" />
                        <asp:Parameter Name="newEntity" Type="Object" />
                    </UpdateParameters>--%>
                    <%--<SelectParameters>
                <asp:Parameter Name="oiID" Type="Int32" />
            </SelectParameters>--%>
                    <UpdateParameters>
                        <asp:Parameter Name="oldEntity" Type="Object" />
                        <asp:Parameter Name="newEntity" Type="Object" />
                        <asp:Parameter Name="StartDate" Type="DateTime" ConvertEmptyStringToNull="true" />
                        <asp:Parameter Name="EndDate" Type="DateTime" ConvertEmptyStringToNull="true" />
                    </UpdateParameters>
                    <InsertParameters>
                        <asp:Parameter Name="StartDate" Type="DateTime" ConvertEmptyStringToNull="true" />
                        <asp:Parameter Name="EndDate" Type="DateTime" ConvertEmptyStringToNull="true" />
                        <asp:Parameter Name="ApplicationDate" Type="DateTime" ConvertEmptyStringToNull="true" />
                    </InsertParameters>
                </cc1:DetailsViewDataSource>
            </asp:View>
            <asp:View ID="BillView" runat="server">
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblFeedBack" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <asp:Panel ID="pnlBillCash" runat="server">
                        <tr>
                            <td>
                                <asp:TextBox ID="tbFiscalItems" CssClass="tekstPole" Visible="false" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btnPrintBill" CssClass="gray_button" runat="server" Text="Печати фискална сметка"
                                    OnClick="btnPrintBill_Click" Visible="false" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btnDiscardBill" CssClass="cancel" runat="server" Text="Сторнирај фискална сметка"
                                    Enabled="false" OnClick="btnDiscardBill_Click" Visible="false" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btnPrintBillInfoForCash" CssClass="gray_button" runat="server" Text="Печати потврда"
                                    OnClick="btnPrintBillInfoForPolicy_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btnDiscardPolicy" CssClass="cancel" runat="server" Text="Сторнирај полиса"
                                    OnClick="btnDiscardPolicy_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblCash" runat="server" Text="Готовина" Visible="false"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="tbCash" CssClass="tekstPole" Visible="false" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblCard" runat="server" Visible="false" Text="Картичка"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="tbCreditCard" Visible="false" CssClass="tekstPole" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblBanks" runat="server" Text="Банка" Visible="false"></asp:Label><asp:DropDownList
                                    ID="ddlBank" runat="server" DataSourceID="odsBank" AutoPostBack="True" DataTextField="Name"
                                    DataValueField="ID" Visible="false">
                                </asp:DropDownList>
                                <asp:ObjectDataSource ID="odsBank" runat="server" SelectMethod="GetBanksWithGreditCards"
                                    TypeName="Broker.DataAccess.Bank"></asp:ObjectDataSource>
                            </td>
                            <td>
                                <asp:Label ID="lblCardTypes" runat="server" Text="Тип на Картичка" Visible="false"></asp:Label><asp:DropDownList
                                    ID="ddlCardTypes" runat="server" DataSourceID="odsCardTypes" AutoPostBack="True"
                                    DataTextField="Name" DataValueField="ID" Visible="false">
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
                                <asp:Button ID="btnInsertNewPayment" CssClass="submit" runat="server" Text="Внеси"
                                    OnClick="btnInsertNewPayment_Click" Enabled="false" Visible="false" />
                            </td>
                        </tr>
                    </asp:Panel>
                    <asp:Panel ID="pnlBillFacture" runat="server" Visible="false">
                        <tr>
                            <td>
                                <asp:Button ID="btnPrintFacture" CssClass="gray_button" runat="server" Text="Печати фактура"
                                    OnClick="btnPrintFacture_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btnPrintBillInfoForPolicyInFactures" CssClass="gray_button" runat="server"
                                    Text="Печати потврда" OnClick="btnPrintBillInfoForPolicy_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btnDiscard" CssClass="cancel" runat="server" Text="Сторнирај полиса"
                                    OnClick="btnDiscardPolicy_Click" />
                            </td>
                        </tr>
                    </asp:Panel>
                </table>
            </asp:View>
            <asp:View ID="FactureRatesView" runat="server">
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblFeedbackRates" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="btnPrintFactureForRates" CssClass="gray_button" runat="server" Text="Печати фактура"
                                OnClick="btnPrintFactureForRates_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="btnPrintAnexDealFactureForRates" CssClass="gray_button" runat="server"
                                Text="Печати договор за рати" OnClick="btnPrintAnexDealFactureForRates_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="btnPrintBillInfoForPolicyInRates" CssClass="gray_button" runat="server"
                                Text="Печати потврда" OnClick="btnPrintBillInfoForPolicy_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="btnDiscardPolicyRates" CssClass="cancel" runat="server" Text="Сторнирај полиса"
                                OnClick="btnDiscardPolicy_Click" />
                        </td>
                    </tr>
                </table>
            </asp:View>
            <asp:View ID="viewPaidInInsuranceCompany" runat="server">
                <table>
                    <tr>
                        <td>
                            <asp:Button ID="btnPrintAnexDealPaidInInsuranceCompany" CssClass="gray_button" runat="server"
                                Text="Печати договор за рати" OnClick="btnPrintAnexDealPaidInInsuranceCompany_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="btnPrintBillInfoPaidInInsuranceCompany" CssClass="gray_button" runat="server"
                                Text="Печати потврда" OnClick="btnPrintBillInfoForPolicy_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="btnDiscardPolicyPaidInInsuranceCompany" CssClass="cancel" runat="server"
                                Text="Сторнирај полиса" OnClick="btnDiscardPolicy_Click" />
                        </td>
                    </tr>
                </table>
            </asp:View>
        </asp:MultiView>
    </div>
</asp:Content>
