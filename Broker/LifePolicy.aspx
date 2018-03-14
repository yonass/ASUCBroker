<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="LifePolicy.aspx.cs" Inherits="Broker_LifePolicy" %>

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
                                                <asp:ObjectDataSource ID="odsInsuranceCompany" runat="server" SelectMethod="GetByActiveDealsForLife"
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
                                                <asp:DropDownList ID="ddlDeals" runat="server" DataSourceID="odsDeals" DataTextField="Name"
                                                    CssClass="select normal" DataValueField="ID" AutoPostBack="true" OnSelectedIndexChanged="ddlDealsSelectedIndexChanged">
                                                </asp:DropDownList>
                                                <asp:ObjectDataSource ID="odsDeals" runat="server" SelectMethod="GetActiveDealsForCompanyAndInsuranceSubType"
                                                    TypeName="Broker.DataAccess.LifeDeal">
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
                                                <asp:Label ID="lblInsuranceSubType" runat="server" Text="Подтип на осигурување"></asp:Label>
                                            </td>
                                            <td class="policy_field_td">
                                                <asp:DropDownList ID="ddlInsuranceSubTypes" runat="server" DataSourceID="odsInsuranceSubTypes"
                                                    CssClass="select" DataTextField="ShortDescription" DataValueField="ID" AutoPostBack="true"
                                                    OnSelectedIndexChanged="ddlInsuranceSubType_selecteIndexChanged">
                                                </asp:DropDownList>
                                                <asp:ObjectDataSource ID="odsInsuranceSubTypes" runat="server" SelectMethod="GetByInsuranceTypeAndCompanyForLife"
                                                    TypeName="Broker.DataAccess.InsuranceSubType">
                                                    <SelectParameters>
                                                        <asp:ControlParameter ControlID="ddlInsuranceCompany" Name="companyID" PropertyName="SelectedValue"
                                                            Type="Int32" />
                                                    </SelectParameters>
                                                </asp:ObjectDataSource>
                                            </td>
                                            <td class="policy_button_td">
                                            </td>
                                            <td class="policy_label_td">
                                                <asp:Label ID="Label1" runat="server" Text="Понуда број"></asp:Label>
                                            </td>
                                            <td class="policy_field_td">
                                                <asp:TextBox ID="tbOfferNumber" runat="server" Text='<%#Bind("OfferNumber") %>'></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvOfferNumber" runat="server" ErrorMessage="*" Display="Dynamic"
                                                    ControlToValidate="tbOfferNumber"></asp:RequiredFieldValidator>
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
                                        <td style="width: 250px;">
                                            Времетраење на полисата во години
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tbPolicyDurationYears" runat="server" ReadOnly="true" Width="40px"></asp:TextBox>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Износ на евро
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tbEuroValue" runat="server" AutoPostBack="true" Width="40px" OnTextChanged="tbEuroValue_TextChanged"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Button ID="btnReCalculate" runat="server" Text="Пресметај" OnClick="btnReCalculate_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </InsertItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <InsertItemTemplate>
                                <table>
                                    <tr>
                                        <td style="width: 250px;">
                                            <b>Осигурителни покритија</b>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Осигурително покритие за доживување
                                        </td>
                                        <td>
                                            Во евра
                                            <asp:TextBox ID="tbInsuranceCoverageOneEuro" runat="server" Width="50px" Text='<%#Bind("InsuranceCoverageOneEuro") %>'
                                                AutoPostBack="True" OnTextChanged="tbInsuranceCoverageOneEuro_TextChanged"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvInsuranceCoverageOneEuro" runat="server" ErrorMessage="*"
                                                ControlToValidate="tbInsuranceCoverageOneEuro" Display="Dynamic">
                                            </asp:RequiredFieldValidator>
                                            <asp:CompareValidator ID="cvInsuranceCoverageOneEuro" runat="server" ErrorMessage="*"
                                                ControlToValidate="tbInsuranceCoverageOneEuro" Display="Dynamic" Operator="DataTypeCheck"
                                                Type="Double"></asp:CompareValidator>
                                        </td>
                                        <td>
                                            Во денари
                                            <asp:TextBox ID="tbInsuranceCoverageOne" runat="server" Width="50px" Text='<%#Bind("InsuranceCoverageOne") %>'></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvInsuranceCoverageOne" runat="server" ErrorMessage="*"
                                                ControlToValidate="tbInsuranceCoverageOne" Display="Dynamic">
                                            </asp:RequiredFieldValidator>
                                            <asp:CompareValidator ID="cvInsuranceCoverageOne" runat="server" ErrorMessage="*"
                                                ControlToValidate="tbInsuranceCoverageOne" Display="Dynamic" Operator="DataTypeCheck"
                                                Type="Double"></asp:CompareValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Осигурително покритие за незгода
                                        </td>
                                        <td>
                                            Во евра
                                            <asp:TextBox ID="tbInsuranceCoverageTwoEuro" runat="server" Width="50px" Text='<%#Bind("InsuranceCoverageTwoEuro") %>'
                                                AutoPostBack="True" OnTextChanged="tbInsuranceCoverageTwoEuro_TextChanged"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvInsuranceCoverageTwoEuro" runat="server" ErrorMessage="*"
                                                ControlToValidate="tbInsuranceCoverageTwoEuro" Display="Dynamic">
                                            </asp:RequiredFieldValidator>
                                            <asp:CompareValidator ID="cvInsuranceCoverageTwoEuro" runat="server" ErrorMessage="*"
                                                ControlToValidate="tbInsuranceCoverageTwoEuro" Display="Dynamic" Operator="DataTypeCheck"
                                                Type="Double"></asp:CompareValidator>
                                        </td>
                                        <td>
                                            Во денари
                                            <asp:TextBox ID="tbInsuranceCoverageTwo" runat="server" Width="50px" Text='<%#Bind("InsuranceCoverageTwo") %>'></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvInsuranceCoverageTwo" runat="server" ErrorMessage="*"
                                                ControlToValidate="tbInsuranceCoverageTwo" Display="Dynamic">
                                            </asp:RequiredFieldValidator>
                                            <asp:CompareValidator ID="cvInsuranceCoverageTwo" runat="server" ErrorMessage="*"
                                                ControlToValidate="tbInsuranceCoverageTwo" Display="Dynamic" Operator="DataTypeCheck"
                                                Type="Double"></asp:CompareValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Вкупно:
                                        </td>
                                        <td>
                                            Во евра
                                            <asp:TextBox ID="tbTotalInsuranceCoverageSumEuro" runat="server" Width="50px" Text='<%#Bind("TotalInsuranceCoverageSumEuro") %>'
                                                ReadOnly="true"></asp:TextBox>
                                        </td>
                                        <td>
                                            Во денари
                                            <asp:TextBox ID="tbTotalInsuranceCoverageSum" runat="server" Width="50px" Text='<%#Bind("TotalInsuranceCoverageSum") %>'
                                                ReadOnly="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </InsertItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <InsertItemTemplate>
                                <table>
                                    <tr>
                                        <td style="width: 250px;">
                                            <b>Премии</b>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Годишна премија за доживување
                                        </td>
                                        <td>
                                            Во евра
                                            <asp:TextBox ID="tbYearlyPremiumValueForLifeEuro" runat="server" Width="50px" Text='<%#Bind("YearlyPremiumValueForLifeEuro") %>'
                                                AutoPostBack="True" OnTextChanged="tbYearlyPremiumValueForLifeEuro_TextChanged"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvYearlyPremiumValueForLifeEuro" runat="server"
                                                ErrorMessage="*" ControlToValidate="tbYearlyPremiumValueForLifeEuro" Display="Dynamic">
                                            </asp:RequiredFieldValidator>
                                            <asp:CompareValidator ID="cvYearlyPremiumValueForLifeEuro" runat="server" ErrorMessage="*"
                                                ControlToValidate="tbYearlyPremiumValueForLifeEuro" Display="Dynamic" Operator="DataTypeCheck"
                                                Type="Double"></asp:CompareValidator>
                                        </td>
                                        <td>
                                            Во денари
                                            <asp:TextBox ID="tbYearlyPremiumValueForLife" runat="server" Width="50px" Text='<%#Bind("YearlyPremiumValueForLife") %>'
                                                AutoPostBack="True"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvYearlyPremiumValueForLife" runat="server" ErrorMessage="*"
                                                ControlToValidate="tbYearlyPremiumValueForLife" Display="Dynamic">
                                            </asp:RequiredFieldValidator>
                                            <asp:CompareValidator ID="cvYearlyPremiumValueForLife" runat="server" ErrorMessage="*"
                                                ControlToValidate="tbYearlyPremiumValueForLife" Display="Dynamic" Operator="DataTypeCheck"
                                                Type="Double"></asp:CompareValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Годишна премија за незгода
                                        </td>
                                        <td>
                                            Во евра
                                            <asp:TextBox ID="tbYearlyPremiumValueForAccidentEuro" runat="server" Width="50px"
                                                Text='<%#Bind("YearlyPremiumValueForAccidentEuro") %>' AutoPostBack="True" OnTextChanged="tbYearlyPremiumValueForAccidentEuro_TextChanged"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvYearlyPremiumValueForAccidentEuro" runat="server"
                                                ErrorMessage="*" ControlToValidate="tbYearlyPremiumValueForAccidentEuro" Display="Dynamic">
                                            </asp:RequiredFieldValidator>
                                            <asp:CompareValidator ID="cvYearlyPremiumValueForAccidentEuro" runat="server" ErrorMessage="*"
                                                ControlToValidate="tbYearlyPremiumValueForAccidentEuro" Display="Dynamic" Operator="DataTypeCheck"
                                                Type="Double"></asp:CompareValidator>
                                        </td>
                                        <td>
                                            Во денари
                                            <asp:TextBox ID="tbYearlyPremiumValueForAccident" runat="server" Width="50px" Text='<%#Bind("YearlyPremiumValueForAccident") %>'></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvYearlyPremiumValueForAccident" runat="server"
                                                ErrorMessage="*" ControlToValidate="tbYearlyPremiumValueForAccident" Display="Dynamic">
                                            </asp:RequiredFieldValidator>
                                            <asp:CompareValidator ID="cvYearlyPremiumValueForAccident" runat="server" ErrorMessage="*"
                                                ControlToValidate="tbYearlyPremiumValueForAccident" Display="Dynamic" Operator="DataTypeCheck"
                                                Type="Double"></asp:CompareValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Вкупно:
                                        </td>
                                        <td>
                                            Во евра
                                            <asp:TextBox ID="tbTotalPremumValueEuro" runat="server" Width="50px" Text='<%#Bind("TotalPremiumValueEuro") %>'
                                                ReadOnly="true"></asp:TextBox>
                                        </td>
                                        <td>
                                            Во денари
                                            <asp:TextBox ID="tbTotalPremumValue" runat="server" Width="50px" Text='<%#Bind("TotalPremumValue") %>'
                                                ReadOnly="true"></asp:TextBox>
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
                                    </tr>
                                </table>
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
                <cc1:DetailsViewDataSource ID="dvDataSource" runat="server" TypeName="Broker.DataAccess.LifePolicy"
                    ConflictDetection="CompareAllValues" DataObjectTypeName="Broker.DataAccess.LifePolicy"
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
                        <asp:Parameter Name="TotalPremumValue" Type="Decimal" />
                        <asp:Parameter Name="TotalPremiumValueEuro" Type="Decimal" />
                        <asp:Parameter Name="YearlyPremiumValueForAccident" Type="Decimal" />
                        <asp:Parameter Name="YearlyPremiumValueForAccidentEuro" Type="Decimal" />
                        <asp:Parameter Name="YearlyPremiumValueForLife" Type="Decimal" />
                        <asp:Parameter Name="YearlyPremiumValueForLifeEuro" Type="Decimal" />
                        <asp:Parameter Name="TotalInsuranceCoverageSum" Type="Decimal" />
                        <asp:Parameter Name="TotalInsuranceCoverageSumEuro" Type="Decimal" />
                        <asp:Parameter Name="InsuranceCoverageTwo" Type="Decimal" />
                        <asp:Parameter Name="InsuranceCoverageTwoEuro" Type="Decimal" />
                        <asp:Parameter Name="InsuranceCoverageOne" Type="Decimal" />
                        <asp:Parameter Name="InsuranceCoverageOneEuro" Type="Decimal" />
                    </UpdateParameters>
                    <InsertParameters>
                        <asp:Parameter Name="StartDate" Type="DateTime" ConvertEmptyStringToNull="true" />
                        <asp:Parameter Name="EndDate" Type="DateTime" ConvertEmptyStringToNull="true" />
                        <asp:Parameter Name="ApplicationDate" Type="DateTime" ConvertEmptyStringToNull="true" />
                        <asp:Parameter Name="TotalPremumValue" Type="Decimal" />
                        <asp:Parameter Name="TotalPremiumValueEuro" Type="Decimal" />
                        <asp:Parameter Name="YearlyPremiumValueForAccident" Type="Decimal" />
                        <asp:Parameter Name="YearlyPremiumValueForAccidentEuro" Type="Decimal" />
                        <asp:Parameter Name="YearlyPremiumValueForLife" Type="Decimal" />
                        <asp:Parameter Name="YearlyPremiumValueForLifeEuro" Type="Decimal" />
                        <asp:Parameter Name="TotalInsuranceCoverageSum" Type="Decimal" />
                        <asp:Parameter Name="TotalInsuranceCoverageSumEuro" Type="Decimal" />
                        <asp:Parameter Name="InsuranceCoverageTwo" Type="Decimal" />
                        <asp:Parameter Name="InsuranceCoverageTwoEuro" Type="Decimal" />
                        <asp:Parameter Name="InsuranceCoverageOne" Type="Decimal" />
                        <asp:Parameter Name="InsuranceCoverageOneEuro" Type="Decimal" />
                    </InsertParameters>
                </cc1:DetailsViewDataSource>
            </asp:View>
        </asp:MultiView>
    </div>
</asp:Content>
