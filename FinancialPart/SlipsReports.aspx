<%@ Page Title="Извештај од слипови" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="SlipsReports.aspx.cs" Inherits="FinancialPart_SlipsReports" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table>
        <tr>
            <td>
              <asp:Label ID="lblStartDate" runat="server" Text="Од Датум"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="tbStartDate" runat="server"></asp:TextBox>
                <asp:CompareValidator ID="cvStartDate" runat="server" ControlToValidate="tbStartDate"
                    Display="Dynamic" ErrorMessage="*" Operator="DataTypeCheck" Type="Date"></asp:CompareValidator>
                <asp:RequiredFieldValidator ID="rfvStartDate" runat="server" ControlToValidate="tbStartDate"
                    Display="Dynamic" ErrorMessage="*"></asp:RequiredFieldValidator>
            </td>
            <td>
               <asp:Label ID="lblEndDate" Text="До датум" runat="server"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="tbEndDate" runat="server"></asp:TextBox>
                <asp:CompareValidator ID="cvEndDate" runat="server" ControlToValidate="tbEndDate"
                    Display="Dynamic" ErrorMessage="*" Operator="DataTypeCheck" Type="Date"></asp:CompareValidator>
                <asp:RequiredFieldValidator ID="rfvEndDate" runat="server" ControlToValidate="tbEndDate"
                    Display="Dynamic" ErrorMessage="*"></asp:RequiredFieldValidator>
                <%--<asp:CompareValidator ID="cvGraterEndDate" runat="server" ControlToValidate="tbEndDate"
                                Display="Dynamic" ErrorMessage="*" Operator="GreaterThanEqual" ControlToCompare="tbStartDate"></asp:CompareValidator>--%>
            </td>
        </tr>
        <tr>
            <td>
            <asp:Label runat="server" ID="lblBank" Text="Банка"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="ddlBanks" runat="server" DataSourceID="odsBanks" 
                    DataTextField="Name" DataValueField="ID">
                </asp:DropDownList>
                <asp:ObjectDataSource ID="odsBanks" runat="server" 
                    SelectMethod="GetActiveBanks" TypeName="Broker.DataAccess.Bank"></asp:ObjectDataSource>
            </td>
            <td>
            </td>
            <td>
                <asp:Button ID="btnPrint" runat="server" Text="Печати" 
                    onclick="btnPrint_Click" />
            </td>
        </tr>
    </table>
</asp:Content>
