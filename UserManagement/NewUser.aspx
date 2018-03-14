<%@ Page Title="Нов корисник" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="NewUser.aspx.cs" Inherits="UserManagement_NewUser" %>

<%@ Register Namespace="Superexpert.Controls" TagPrefix="super" %>
<%@ Register Namespace="Broker.Controllers.Tree" TagPrefix="tree" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:MultiView ID="MultiView1" runat="server">
        <asp:View ID="View1" runat="server">
            <table>
                <tr>
                    <td align="right">
                        Username:
                    </td>
                    <td>
                        <asp:TextBox ID="UsernameTextBox" runat="server"></asp:TextBox><super:EntityCallOutValidator
                            ID="UsernameValidator" PropertyName="USERNAME_EXISTS" runat="server" />
                        <asp:RequiredFieldValidator ID="fildValidator" runat="server" Text="*"
                            ControlToValidate="UsernameTextBox" />
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        Лозинка:
                    </td>
                    <td>
                        <asp:TextBox ID="passwordTextBox" runat="server" ReadOnly="true"> </asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="passwordTextBox"
                            Display="Dynamic" ErrorMessage="*"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        Име и Презиме:
                    </td>
                    <td>
                        <asp:TextBox ID="FullNameTextBox" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="FullNameTextBox"
                            ErrorMessage="*"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        ЕМБГ:
                    </td>
                    <td>
                        <asp:TextBox ID="EMBGTextBox" runat="server"> </asp:TextBox>
                        <super:EntityCallOutValidator ID="EMBGValidator" PropertyName="EMBG_Exist" runat="server" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="EMBGTextBox"
                            Display="Dynamic" ErrorMessage="*"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        Адреса:
                    </td>
                    <td>
                        <asp:TextBox ID="AddressTextBox" runat="server"> </asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        Телефонски Број:
                    </td>
                    <td>
                        <asp:TextBox ID="PhoneNmberTextBox" runat="server"> </asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        Еmail:
                    </td>
                    <td>
                        <asp:TextBox ID="EmailTextBox" runat="server"> </asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        <asp:RadioButtonList ID="RolesList" runat="server" DataSourceID="RolesDataSource"
                            DataTextField="Name" DataValueField="ID">
                        </asp:RadioButtonList>
                        <asp:ObjectDataSource ID="RolesDataSource" runat="server" SelectMethod="GetAllVisibleRoles"
                            TypeName="Broker.Controllers.EmployeeManagement.EmployRoleController"></asp:ObjectDataSource>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="btnNext" runat="server" OnClick="btnNext_Click1" Text="Наредно" />
                    </td>
                    <td>
                        <asp:Button ID="Generate" runat="server" OnClick="Generate_click" Text="Генерирај лозинка"
                            CausesValidation="false" />
                    </td>
                </tr>
            </table>
            <asp:Label ID="lblError" runat="server" Visible="false" CssClass="reportDivError"></asp:Label>
        </asp:View>
        <asp:View ID="View2" runat="server">
            Филијала:
            <asp:DropDownList ID="BranchesList" runat="server" DataSourceID="BranchesDataSource"
                DataTextField="Name" DataValueField="ID">
            </asp:DropDownList>
            <asp:ObjectDataSource ID="BranchesDataSource" runat="server" SelectMethod="GetActiveBranches"
                TypeName="Broker.DataAccess.Branch"></asp:ObjectDataSource>
            <br />
            <asp:Button ID="btnInsert" runat="server" OnClick="btnInsert_Click" Text="Внеси" />
            <br />
            <asp:Label ID="lblOK" runat="server" Visible="false" CssClass="reportDivOk"></asp:Label>
        </asp:View>
    </asp:MultiView>
</asp:Content>
