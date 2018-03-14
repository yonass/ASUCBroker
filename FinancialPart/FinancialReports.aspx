<%@ Page Title="Финансиски извештаи" Language="C#" MasterPageFile="~/MasterPage.master"
    AutoEventWireup="true" CodeFile="FinancialReports.aspx.cs" Inherits="FinancialPart_FinancialReports" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="paddingKontroli">
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblStartDate" runat="server" Text="Почетна дата"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="tbStartDate" runat="server"></asp:TextBox>
                </td>
                <td>
                    <asp:Label ID="lblEndDate" runat="server" Text="Крајна дата"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="tbEndDate" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblReportType" runat="server" Text="Тип на извештај (сортирање)"></asp:Label>
                </td>
                <td>
                    <asp:RadioButtonList ID="rblReportType" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Selected="True" Value="PolicyNumber" Text="По број на полиса"></asp:ListItem>
                        <asp:ListItem Value="PaymentType" Text="По начин на плаќање"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button runat="server" ID="btnPrint" Text="Печати" OnClick="btnPrint_Click" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
