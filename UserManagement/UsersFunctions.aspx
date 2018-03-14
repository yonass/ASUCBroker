<%@ Page Title="Доделување на функција на корисник" Language="C#" MasterPageFile="~/MasterPage.master"
    AutoEventWireup="true" CodeFile="UsersFunctions.aspx.cs" Inherits="UserManagement_UsersFunctions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="paddingKontroli">
        <table>
            <tr>
                <td>
                    Корисничко име:
                    <asp:DropDownList ID="ddlUsers" runat="server" DataSourceID="ChildrenDataSource"
                        DataTextField="Username" DataValueField="ID" AutoPostBack="true" OnSelectedIndexChanged="OnChildChanged">
                    </asp:DropDownList>
                    <asp:ObjectDataSource ID="ChildrenDataSource" runat="server" SelectMethod="GetManageChildUsers"
                        TypeName="Broker.Controllers.UserManagement.UserManagementController">
                        <SelectParameters>
                            <asp:SessionParameter SessionField="UserID" Name="userId" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                </td>
            </tr>
        </table>
        <table>
            <tr>
                <td>
                    Преддефинирани функции на улогата:
                </td>
                <td>
                </td>
                <td>
                    Функции на корисникот:
                </td>
            </tr>
            <tr>
                <td align="left" valign="top">
                    <asp:CheckBoxList ID="RoleFunctionsList" runat="server" DataSourceID="ObjectDataSource1"
                        DataTextField="Name" DataValueField="ID" OnDataBound="RoleFunctionsList_DataBound">
                    </asp:CheckBoxList>
                    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetAvaliableFunctionsByUser"
                        TypeName="Broker.Controllers.UserManagement.UserManagementController"></asp:ObjectDataSource>
                </td>
                <td align="left" valign="top">
                    <asp:Button ID="Button2" runat="server" Text=" << " OnClick="Button2_Click" />
                    <asp:Button ID="Button1" runat="server" Text=" >> " OnClick="Button1_Click" />
                </td>
                <td align="right" valign="top">
                    <asp:CheckBoxList ID="UserFunctionsList" runat="server" DataSourceID="ObjectDataSource3"
                        DataTextField="Name" DataValueField="ID">
                    </asp:CheckBoxList>
                    <asp:ObjectDataSource ID="ObjectDataSource3" runat="server" SelectMethod="GetNotGivenFunctions"
                        TypeName="Broker.Controllers.UserManagement.UserManagementController">
                        <%-- <SelectParameters>
                            <asp:Parameter Name="userId" Type="Int32" />
                        </SelectParameters>--%>
                    </asp:ObjectDataSource>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <asp:Button ID="ResetButton" runat="server" Text="Ресетирај" OnClick="Button3_Click" />
                </td>
                <td>
                    <asp:Button ID="Button4" runat="server" Text="Зачувај" OnClick="Button4_Click" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
