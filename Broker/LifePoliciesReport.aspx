<%@ Page Title="Извештај за жив.оси." Language="C#" MasterPageFile="~/MasterPage.master"
    AutoEventWireup="true" CodeFile="LifePoliciesReport.aspx.cs" Inherits="Broker_LifePoliciesReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="tabeliFrame">
        <div id="header">
            <div id="content">
                Извештаи за жив. оси.
            </div>
        </div>
        <div id="contentOuter">
            <div id="contentInner">
                <table border="0px" cellpadding="5" cellspacing="0" width="100%">
                    <tr>
                        <td colspan="2">
                            <b>Од датум</b>
                            <asp:TextBox ID="tbStartDate" runat="server"></asp:TextBox>
                            <asp:CompareValidator ID="cvStartDate" runat="server" ControlToValidate="tbStartDate"
                                Display="Dynamic" ErrorMessage="*" Operator="DataTypeCheck" Type="Date"></asp:CompareValidator>
                            <asp:RequiredFieldValidator ID="rfvStartDate" runat="server" ControlToValidate="tbStartDate"
                                Display="Dynamic" ErrorMessage="*"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <b>До датум</b>
                            <asp:TextBox ID="tbEndDate" runat="server"></asp:TextBox>
                            <asp:CompareValidator ID="cvEndDate" runat="server" ControlToValidate="tbEndDate"
                                Display="Dynamic" ErrorMessage="*" Operator="DataTypeCheck" Type="Date"></asp:CompareValidator>
                            <asp:RequiredFieldValidator ID="rfvEndDate" runat="server" ControlToValidate="tbEndDate"
                                Display="Dynamic" ErrorMessage="*"></asp:RequiredFieldValidator>
                            <asp:CompareValidator ID="cvGraterEndDate" runat="server" ControlToValidate="tbEndDate"
                                Display="Dynamic" ErrorMessage="*" Operator="GreaterThanEqual" ControlToCompare="tbStartDate"></asp:CompareValidator>
                        </td>
                    </tr>
                    <tr class="altenativeRow">
                        <td>
                            Извештај за брокеража
                        </td>
                        <td>
                            <asp:Button ID="btnPrintReportForBrokerage" runat="server" Text="Печати" OnClick="btnPrintReportForBrokerage_Click" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
</asp:Content>
