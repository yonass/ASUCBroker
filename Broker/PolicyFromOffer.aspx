<%@ Page Title="Евидентирање на полиса од понуда" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="PolicyFromOffer.aspx.cs" Inherits="Broker_PolicyFromOffer" Culture="mk-MK" UICulture="mk-MK" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="border1px_Policy">
        <asp:MultiView ID="multiView" runat="server">
            <asp:View ID="firstView" runat="server">
                <table width="695px">
                    <tr>
                        <td style="width: 270px;">
                            <asp:Label ID="lblPolicyNumber" runat="server" Text="Број на полиса"></asp:Label>
                        </td>
                        <td style="width: 445px;">
                            <asp:TextBox ID="tbPolicyNumber" runat="server"></asp:TextBox>
                            <asp:Label ID="lblError" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr class="light-background">
                        <td style="width: 270px;">
                            <asp:Label ID="lblOfferNumber" runat="server" Text="Број на понуда"></asp:Label>
                        </td>
                        <td style="width: 445px;">
                            <asp:TextBox ID="tbOfferNumber" runat="server"> </asp:TextBox>
                            <asp:Button ID="btnSearchOffer" runat="server" Text="..." OnClick="btnSearchOffer_Click"
                                CausesValidation="false" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 270px;">
                            <asp:Label ID="lblOfferItemInsuranceSubType" runat="server" Text="Подкласа на осигурување"></asp:Label>
                        </td>
                        <td style="width: 445px;">
                            <asp:DropDownList ID="ddlOfferItemInsuranceSubType" runat="server" DataSourceID="odsOfferItemInsuranceSubType"
                                DataTextField="InsuranceSubTypeDescription" DataValueField="OfferItemID" Width="250px"
                                OnSelectedIndexChanged="insuranceSubTypeChanged" AutoPostBack="True">
                            </asp:DropDownList>
                            <asp:ObjectDataSource ID="odsOfferItemInsuranceSubType" runat="server" SelectMethod="GetByOfferNumber"
                                TypeName="Broker.DataAccess.OfferItemsView">
                                <SelectParameters>
                                    <asp:Parameter Name="offerNumber" Type="String" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                        </td>
                    </tr>
                    <tr class="light-background">
                        <td style="width: 270px;">
                            <asp:Label ID="lblOfferItemInsuranceCompany" runat="server" Text="Осигурителна Компанија"></asp:Label>
                        </td>
                        <td style="width: 445px;">
                            <asp:TextBox ID="tbInsuranceCompany" runat="server" ReadOnly="True"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 270px;">
                            <asp:Label ID="lblClient" runat="server" Text="Договорувач"></asp:Label>
                        </td>
                        <td style="width: 445px;">
                            <asp:TextBox ID="tbClientName" runat="server" Width="250" ReadOnly="true"></asp:TextBox>
                            <asp:TextBox ID="tbClientEMBG" runat="server" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="light-background">
                        <td style="width: 270px;">
                            <asp:Label ID="lblOwner" runat="server" Text="Осигуреник"></asp:Label>
                        </td>
                        <td style="width: 445px;">
                            <asp:TextBox ID="tbOwnerName" runat="server" Width="250" ReadOnly="true"></asp:TextBox>
                            <asp:TextBox ID="tbOwnerEMBG" runat="server" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 270px;">
                            <asp:Label ID="lblStartDate" runat="server" Text="Почетна Дата"></asp:Label>
                        </td>
                        <td style="width: 445px;">
                            <asp:TextBox ID="tbStartDate" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvStartDate" runat="server" ControlToValidate="tbStartDate"
                                Display="Dynamic" ErrorMessage="*"></asp:RequiredFieldValidator>
                            <asp:CompareValidator ID="cvStartDate" runat="server" ControlToValidate="tbStartDate"
                                Display="Dynamic" ErrorMessage="*" Operator="DataTypeCheck" Type="Date"></asp:CompareValidator>
                        </td>
                    </tr>
                    <tr class="light-background">
                        <td style="width: 270px;">
                            <asp:Label ID="lblEndData" runat="server" Text="Крајна Дата"></asp:Label>
                        </td>
                        <td style="width: 445px;">
                            <asp:TextBox ID="tbEndDate" runat="server"></asp:TextBox>
                            <asp:CompareValidator ID="cvEndDate" runat="server" ControlToCompare="tbStartDate"
                                ControlToValidate="tbEndDate" Display="Dynamic" ErrorMessage="*" Operator="GreaterThanEqual"></asp:CompareValidator>
                            <asp:RequiredFieldValidator ID="rfvEndDate" runat="server" ControlToValidate="tbEndDate"
                                Display="Dynamic" ErrorMessage="*"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblPaidInCash" runat="server" Text="Платена во готово"></asp:Label>
                            <asp:CheckBox ID="cbIsPaidInCash" runat="server" Checked="true" AutoPostBack="True"
                                OnCheckedChanged="cbIsPaidInCash_CheckedChanged" />
                        </td>
                    </tr>
                    <asp:Panel ID="pnlCashPayment" runat="server">
                        <tr>
                            <td style="width: 173px">
                                <asp:Label ID="lblCash" runat="server" Text="Готовина"></asp:Label>
                                <asp:TextBox ID="tbCash" runat="server" Width="155px"></asp:TextBox>
                                <asp:CompareValidator ID="cvCash" runat="server" ControlToValidate="tbCash" ErrorMessage="*"
                                    Operator="DataTypeCheck" Type="Double"></asp:CompareValidator>
                            </td>
                            <td style="width: 173px">
                                <asp:Label ID="lblCreditCard" runat="server" Text="Картичка"></asp:Label>
                                <asp:TextBox ID="tbCreditCard" runat="server" Width="155px"></asp:TextBox>
                                <asp:CompareValidator ID="cvCreditCard" runat="server" ControlToValidate="tbCreditCard"
                                    ErrorMessage="*" Operator="DataTypeCheck" Type="Double"></asp:CompareValidator>
                            </td>
                            <td style="width: 173px">
                                <asp:Label ID="lblBanks" runat="server" Text="Банка"></asp:Label>
                                <asp:DropDownList ID="ddlBank" runat="server" DataSourceID="odsBank" AutoPostBack="True"
                                    DataTextField="Name" DataValueField="ID" Width="155px">
                                </asp:DropDownList>
                                <asp:ObjectDataSource ID="odsBank" runat="server" SelectMethod="GetBanksWithGreditCards"
                                    TypeName="Broker.DataAccess.Bank"></asp:ObjectDataSource>
                            </td>
                            <td style="width: 173px">
                                <asp:Label ID="lblCardTypes" runat="server" Text="Тип на Картичка"></asp:Label>
                                <asp:DropDownList ID="ddlCardTypes" runat="server" DataSourceID="odsCardTypes" AutoPostBack="True"
                                    DataTextField="Name" DataValueField="ID" Width="155px">
                                </asp:DropDownList>
                                <asp:ObjectDataSource ID="odsCardTypes" runat="server" SelectMethod="GetByBank" TypeName="Broker.DataAccess.CreditCard">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="ddlBank" Name="BankID" PropertyName="SelectedValue"
                                            Type="Int32" />
                                    </SelectParameters>
                                </asp:ObjectDataSource>
                            </td>
                        </tr>
                    </asp:Panel>
                </table>
                <table>
                    <tr>
                        <td>
                            <asp:Panel ID="pnlExtendControls" runat="server">
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="btnCancel" runat="server" Text="Откажи" />
                            <%--</td>
            <td>--%>
                            <asp:Button ID="btnInsert" runat="server" Text="Сними" OnClick="btnInsert_Click" />
                        </td>
                    </tr>
                </table>
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
                                <asp:Button ID="btnPrintBill" runat="server" Text="Печати сметка" OnClick="btnPrintBill_Click" />
                            </td>
                            <td>
                                <asp:Button ID="btnDiscardBill" runat="server" Text="Сторнирај сметка" Enabled="false"
                                    OnClick="btnDiscardBill_Click" />
                            </td>
                            <td>
                                <asp:Button ID="btnDiscardPolicy" runat="server" Text="Сторнирај полиса" OnClick="btnDiscardPolicy_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="Готовина"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblCard" runat="server" Text="Картичка"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label2" runat="server" Text="Банка"></asp:Label>
                                <asp:DropDownList ID="DropDownList1" runat="server" DataSourceID="odsBank" AutoPostBack="True"
                                    DataTextField="Name" DataValueField="ID">
                                </asp:DropDownList>
                                <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetBanksWithGreditCards"
                                    TypeName="Broker.DataAccess.Bank"></asp:ObjectDataSource>
                            </td>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="Тип на Картичка"></asp:Label>
                                <asp:DropDownList ID="DropDownList2" runat="server" DataSourceID="odsCardTypes" AutoPostBack="True"
                                    DataTextField="Name" DataValueField="ID">
                                </asp:DropDownList>
                                <asp:ObjectDataSource ID="ObjectDataSource2" runat="server" SelectMethod="GetByBank"
                                    TypeName="Broker.DataAccess.CreditCard">
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
                    </asp:Panel>
                    <asp:Panel ID="pnlBillFacture" runat="server" Visible="false">
                        <tr>
                            <td>
                                <asp:Button ID="btnPrintFacture" runat="server" Text="Печати фактура" OnClick="btnPrintFacture_Click" />
                            </td>
                            <td>
                                <asp:Button ID="btnDiscard" runat="server" Text="Сторнирај полиса" OnClick="btnDiscardPolicy_Click" />
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
                            <asp:Button ID="btnPrintFactureForRates" runat="server" Text="Печати фактура" OnClick="btnPrintFactureForRates_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="btnDiscardPolicyRates" runat="server" Text="Сторнирај полиса" OnClick="btnDiscardPolicy_Click" />
                        </td>
                    </tr>
                </table>
            </asp:View>
        </asp:MultiView>
    </div>
</asp:Content>
