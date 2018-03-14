<%@ Page Title="Функции според улоги" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="RolesFunctions.aspx.cs" Inherits="UserManagement_RolesFunctions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table>
        <tr>
            <td valign="top">
                <asp:RadioButtonList ID="RadioButtonList1" runat="server" AutoPostBack="True" DataSourceID="RolesDataSource"
                    DataTextField="Name" DataValueField="ID">
                </asp:RadioButtonList>
                <asp:ObjectDataSource ID="RolesDataSource" runat="server" SelectMethod="GerAllRoles"
                    TypeName="Broker.Controllers.UserManagement.RolesManagementController"></asp:ObjectDataSource>
            </td>
            <td>
            </td>
            <td>
                <asp:CheckBoxList ID="CheckBoxList1" runat="server" DataSourceID="FunctionsDataSource"
                    DataTextField="Name" DataValueField="ID" OnDataBound="CheckBoxList1_DataBound">
                </asp:CheckBoxList>
                <asp:ObjectDataSource ID="FunctionsDataSource" runat="server" SelectMethod="GetFucntionsByRole"
                    TypeName="Broker.Controllers.UserManagement.RolesManagementController">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="RadioButtonList1" DefaultValue="1" Name="roleId"
                            PropertyName="SelectedValue" Type="Int32" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <asp:Button ID="SaveButton" runat="server" Text="Зачувај" OnClick="SaveButton_Click"
                    Style="height: 26px" />
            </td>
            <td>
            </td>
        </tr>
    </table>
</asp:Content>
