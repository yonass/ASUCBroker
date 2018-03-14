<%@ Page Title="Раздолжување" Language="C#" MasterPageFile="~/MasterPage.master"
    AutoEventWireup="true" CodeFile="RollBackDistributions.aspx.cs" Inherits="SEAdmin_RollBackDistributions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="paddingKontroli">
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblInsuranceCompany" runat="server" Text="Осигурителна Компанија"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlInsuranceCompany" runat="server" DataSourceID="InsuranceCompanyDataSource"
                        DataTextField="Name" DataValueField="ID">
                    </asp:DropDownList>
                    <asp:ObjectDataSource ID="InsuranceCompanyDataSource" runat="server" SelectMethod="GetActiveInsuranceCompanies"
                        TypeName="Broker.DataAccess.InsuranceCompany"></asp:ObjectDataSource>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="ldlStartDate" runat="server" Text="Од датум"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="tbStartDate" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblEndDate" runat="server" Text="До датум"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="tbEndDate" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <asp:Button ID="btnCreate" runat="server" OnClick="btnCreateClick" Text="Креирај" />
                </td>
                <td>
                    <asp:Button ID="btnInsert" runat="server" OnClick="btnInsert_Click" Text="Сними" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblFeedBack" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <asp:Button ID="btnCreateRightRestriction" runat="server" OnClick="btnCreateRightRestrictions_Click"
                        Text="Креирај за винкулации" />
                </td>
                <td>
                    <asp:Button ID="btnInsertRightRestrictions" runat="server" OnClick="btnInsertRightRestrictions_Click"
                        Text="Сними" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
