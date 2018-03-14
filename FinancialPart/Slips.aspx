<%@ Page Title="Слипови" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Slips.aspx.cs" Inherits="FinancialPart_Slips" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table>
        <tr>
            <td>
                <asp:Label ID="lblBank" runat="server" Text="Банка"></asp:Label>
            </td>
            <td width="200px">
                <asp:DropDownList ID="ddlBank" runat="server" DataSourceID="odsBank" DataTextField="Name"
                    DataValueField="ID" AutoPostBack="true" Width="250">
                </asp:DropDownList>
                <asp:ObjectDataSource ID="odsBank" runat="server" SelectMethod="GetActiveBanks" TypeName="Broker.DataAccess.Bank">
                </asp:ObjectDataSource>
            </td>
            <td>
                <asp:Label ID="lblDate" runat="server" Text="Дата:"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="tbDate" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvDate" runat="server" 
                    ControlToValidate="tbDate" Display="Dynamic" ErrorMessage="*"></asp:RequiredFieldValidator>
                <asp:CompareValidator ID="cvDate" runat="server" ControlToValidate="tbDate" 
                    Display="Dynamic" ErrorMessage="*" Operator="DataTypeCheck" Type="Date"></asp:CompareValidator>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblSlipNumber" runat="server" Text="Број на слип"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="tbSlipNumber" runat="server"></asp:TextBox>
            </td>
            <td>
            <asp:Label ID="lblPolicyNumber" runat="server" Text="Број на полиса"></asp:Label>
            </td>
            <td>
            <asp:TextBox ID="tbPolicyNumber" runat="server"></asp:TextBox>
            
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblTotalValue" runat="server" Text="Вкупна сума"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="tbTotalValue" runat="server" OnTextChanged="tbValue_Chaged" AutoPostBack="true" CausesValidation="true"></asp:TextBox>
                <asp:CompareValidator ID="cvTotalValue" runat="server" Display="Dynamic" 
                    ErrorMessage="*" Operator="DataTypeCheck" Type="Double" 
                    ControlToValidate="tbTotalValue"></asp:CompareValidator>
                <asp:RequiredFieldValidator ID="rfvTotalValue" runat="server" 
                    ControlToValidate="tbTotalValue" Display="Dynamic" ErrorMessage="*"></asp:RequiredFieldValidator>
            </td>
            <td>
                <asp:Label ID="lblProvision" runat="server" Text="Провизија" ></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="ddlProvision" runat="server" DataSourceID="odsProvisons" DataTextField="Provision" DataTextFormatString="{0:#,0.00}"
                    DataValueField="ID">
                </asp:DropDownList>
                <asp:ObjectDataSource ID="odsProvisons" runat="server" SelectMethod="GetActiveForBank"
                    TypeName="Broker.DataAccess.ProvisionRate">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="ddlBank" Name="bankID" PropertyName="SelectedValue"
                            Type="Int32" />
                    </SelectParameters>
                </asp:ObjectDataSource>
                <asp:RequiredFieldValidator ID="rfvProvision" runat="server" ControlToValidate="ddlProvision"
                    Display="Dynamic" ErrorMessage="*"></asp:RequiredFieldValidator>
            </td>
            <td>
                <asp:Label ID="lblValue" runat="server" Text="Сума"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="tbValue" runat="server" ></asp:TextBox>
                <asp:CompareValidator ID="cvValue" runat="server" ControlToValidate="tbValue" 
                    Display="Dynamic" ErrorMessage="*" Operator="DataTypeCheck" Type="Double"></asp:CompareValidator>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <asp:Button ID="btnInsert" runat="server" OnClick="btnInsert_Click" Text="Сними" />
            </td>
        </tr>
    </table>
</asp:Content>
