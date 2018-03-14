<%@ Page Title="Евиденција на плаќања по полиса" Language="C#" MasterPageFile="~/MasterPage.master"
    AutoEventWireup="true" CodeFile="Payments.aspx.cs" Inherits="Broker_Payments" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table>
        <tr>
            <td style="width: 150px">
                Осигурителна компанија
            </td>
            <td>
                <asp:DropDownList ID="ddlInsuranceCompany" runat="server" DataValueField="ID" DataTextField="Name"
                    DataSourceID="odsCompanies">
                </asp:DropDownList>
                <asp:ObjectDataSource ID="odsCompanies" runat="server" SelectMethod="GetActiveInsuranceCompanies"
                    TypeName="Broker.DataAccess.InsuranceCompany"></asp:ObjectDataSource>
            </td>
        </tr>
        <tr>
            <td style="width: 150px">
                Број на полиса
            </td>
            <td>
                <asp:TextBox ID="tbPolicyNumber" runat="server"></asp:TextBox>
                <asp:Button ID="btnSearch" runat="server" Text="..." OnClick="btnSearch_Click" CausesValidation="false" />
            </td>
        </tr>
    </table>
    <asp:Panel ID="pnlInsurancecSubType" runat="server" Visible="false">
        <table>
            <tr>
                <td style="width: 150px">
                    <asp:Label ID="lblInsuranceSubType" runat="server" Text="Тип на осигурување"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlInsuranceSubTypes" runat="server" AutoPostBack="True" DataSourceID="odsInsuranceSubTypes"
                        DataTextField="ShortDescription" DataValueField="ID" OnSelectedIndexChanged="ddlInsuranceSubTypes_SelectedIndexChanged">
                    </asp:DropDownList>
                    <asp:ObjectDataSource ID="odsInsuranceSubTypes" runat="server" SelectMethod="GetByPolicyNumber"
                        TypeName="Broker.DataAccess.PolicyItem">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="tbPolicyNumber" Name="policyNumber" PropertyName="Text"
                                Type="String" />
                            <asp:ControlParameter ControlID="ddlInsuranceCompany" Name="insuranceCompanyID" PropertyName="SelectedValue"
                                Type="Int32" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <asp:Label ID="lblFeedback" runat="server" Text="" ForeColor="Red" Font-Bold="true"></asp:Label>
    <br />
    <div id="tabeliFrame">
        <div id="header">
            <div id="content">
                <asp:Label ID="lblPageName" runat="server" Text="Договорени рати за плаќање на полисата"></asp:Label>
            </div>
        </div>
        <div id="contentOuter">
            <div id="contentInner">
                <asp:GridView ID="GridViewRates" runat="server" AutoGenerateColumns="False" OnSelectedIndexChanged="GridViewRates_SelectedIndexChanged"
                    OnRowCommand="GridViewRates_RowCommand" GridLines="None" DataSourceID="odsRates"
                    EmptyDataText="Нема записи!">
                    <Columns>
                        <asp:TemplateField HeaderText="Реден број">
                            <ItemTemplate>
                                <asp:TextBox ID="tbNumber" Width="60px" runat="server" ReadOnly="true" Text='<%#Bind("Number") %>'></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Дата">
                            <ItemTemplate>
                                <asp:TextBox ID="tbDate" Width="80px" runat="server" ReadOnly="true" Text='<%#Bind("Date","{0:d}") %>'></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Износ">
                            <ItemTemplate>
                                <asp:TextBox ID="tbValue" Width="100px" runat="server" CssClass="currencyClass" ReadOnly="true"
                                    Text='<%#Bind("Value", "{0:#,0.00}") %>'></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Платено">
                            <ItemTemplate>
                                <asp:TextBox ID="tbPaidValue" runat="server" Width="100px" CssClass="currencyClass"
                                    ReadOnly="true" Text='<%#Bind("PaidValue", "{0:#,0.00}") %>'></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:ObjectDataSource ID="odsRates" runat="server" SelectMethod="GetByNumberAndInsuranceCompany"
                    TypeName="Broker.DataAccess.Rate">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="tbPolicyNumber" Name="policyNumber" PropertyName="Text"
                            Type="String" />
                        <asp:ControlParameter ControlID="ddlInsuranceCompany" Name="insuranceCompanyID" PropertyName="SelectedValue"
                            Type="Int32" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </div>
        </div>
    </div>
    <table>
        <tr>
            <td>
                Премија за наплата
            </td>
            <td>
                <asp:TextBox ID="tbPolicyPremiumCost" Width="100px" runat="server" ReadOnly="true"
                    CssClass="currencyClass"></asp:TextBox>
            </td>
            <td>
                Платено
            </td>
            <td>
                <asp:TextBox ID="tbPolicyTotalPaidValue" Width="100px" runat="server" ReadOnly="true"
                    CssClass="currencyClass"></asp:TextBox>
            </td>
            <td>
                За плаќање
            </td>
            <td>
                <asp:TextBox ID="tbPolicyForPaidValue" Width="100px" runat="server" ReadOnly="true"
                    CssClass="currencyClass"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                Дата на уплата:
            </td>
            <td>
                <asp:TextBox ID="tbDateOfPayment" Width="100px" runat="server"></asp:TextBox>
            </td>
            <td>
                <b>Износ</b>
            </td>
            <td>
                <asp:TextBox ID="tbValueOfPayment" Width="100px" runat="server" Font-Bold="true"></asp:TextBox>
                <asp:CompareValidator ID="cvPaymentValue" runat="server" ControlToValidate="tbValueOfPayment"
                    ErrorMessage="*" Operator="DataTypeCheck" Type="Double" Display="Dynamic"></asp:CompareValidator>
                <asp:RequiredFieldValidator ID="rfvValueOfPayment" runat="server" ControlToValidate="tbValueOfPayment"
                    Display="Dynamic" ErrorMessage="*"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>
                Тип на уплата
            </td>
            <td>
                <asp:DropDownList ID="ddlPaymentTypes" runat="server" DataTextField="Name" DataValueField="ID"
                    Width="100px" DataSourceID="odsPaymentTypes" AutoPostBack="true" OnSelectedIndexChanged="ddlPaymentTypes_SelectedIndexChanged">
                </asp:DropDownList>
                <asp:ObjectDataSource ID="odsPaymentTypes" runat="server" DataObjectTypeName="Broker.DataAccess.PaymentType"
                    TypeName="Broker.DataAccess.PaymentType" SelectMethod="Select"></asp:ObjectDataSource>
            </td>
            <td>
                Банка
            </td>
            <td>
                <asp:DropDownList ID="ddlBank" runat="server" DataSourceID="odsBank" AutoPostBack="True"
                    DataTextField="Name" DataValueField="ID" Width="155px" CssClass="select" Enabled="false">
                </asp:DropDownList>
                <asp:ObjectDataSource ID="odsBank" runat="server" SelectMethod="GetBanksWithGreditCards"
                    TypeName="Broker.DataAccess.Bank" OnSelecting="odsBank_Selecting"></asp:ObjectDataSource>
            </td>
            <td>
                Тип на картица
            </td>
            <td>
                <asp:DropDownList CssClass="select normal" ID="ddlCardTypes" runat="server" DataSourceID="odsCardTypes"
                    DataTextField="Name" DataValueField="ID" Width="100px" Enabled="false">
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
                Број на извод
            </td>
            <td>
                <asp:TextBox ID="tbBankslipNumber" runat="server" Width="100px"></asp:TextBox>
            </td>
            <td>
                Банка на извод
            </td>
            <td>
                <asp:DropDownList ID="ddlBankslipBanks" runat="server" DataSourceID="odsBankslipsBank"
                    AutoPostBack="True" DataTextField="Name" DataValueField="ID" Width="155px" CssClass="select">
                </asp:DropDownList>
                <asp:ObjectDataSource ID="odsBankslipsBank" runat="server" SelectMethod="Select"
                    TypeName="Broker.DataAccess.Bank"></asp:ObjectDataSource>
            </td>
            <td>
            </td>
            <td>
                <asp:Button ID="btnGenerate" runat="server" Text="Пресметај" OnClick="btnGenerate_Click"
                    Enabled="false" />
            </td>
        </tr>
    </table>
    <div id="tabeliFrame">
        <div id="header">
            <div id="content">
                <asp:Label ID="Label1" runat="server" Text="Состојба на платени рати"></asp:Label>
            </div>
        </div>
        <div id="contentOuter">
            <div id="contentInner">
                <asp:GridView ID="GridViewPayments" runat="server" DataSourceID="odsPaidPayments"
                    AutoGenerateColumns="False" GridLines="None" EmptyDataText="Нема записи!">
                    <Columns>
                        <asp:TemplateField HeaderText="За рата">
                            <ItemTemplate>
                                <asp:TextBox ID="tbRateNumber" runat="server" Text='<%#Eval("Rate.Number") %>' ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Датум на плаќање">
                            <ItemTemplate>
                                <asp:TextBox ID="tbDate" runat="server" Text='<%#Eval("Date", "{0:d}") %>' ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Износ">
                            <ItemTemplate>
                                <asp:TextBox ID="tbValue" runat="server" Text='<%#Eval("Value", "{0:#,0.00}") %>'
                                    ReadOnly="true" CssClass="currencyClass"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Тип на плаќање">
                            <ItemTemplate>
                                <asp:DropDownList ID="ddlPaymentTypes" runat="server" Enabled="false" SelectedValue='<%#Eval("PaymentTypeID") %>'
                                    DataSourceID="odsPaymentTypes" Width="120px" DataTextField="Name" DataValueField="ID">
                                </asp:DropDownList>
                                <asp:ObjectDataSource ID="odsPaymentTypes" runat="server" SelectMethod="Select" TypeName="Broker.DataAccess.PaymentType">
                                </asp:ObjectDataSource>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:ObjectDataSource ID="odsPaidPayments" runat="server" SelectMethod="GetByPolicyItemID"
                    TypeName="Broker.DataAccess.Payment">
                    <SelectParameters>
                        <asp:Parameter Name="policyItemID" Type="Int32" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </div>
        </div>
    </div>
    <asp:Button ID="btnInsert" runat="server" Text="Сними" OnClick="btnInsert_Click"
        Enabled="false" />
</asp:Content>
