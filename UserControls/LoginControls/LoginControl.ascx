<%@ Control Language="C#" AutoEventWireup="true" CodeFile="LoginControl.ascx.cs"
    Inherits="UserControls_LoginControls_LoginControl" %>
<div class="login">
    <fieldset>
       <%-- <legend>Најава</legend>--%>
        <%-- <asp:Image ID="imgCompanyLogo" runat="server" />--%>
        <table width="100%">
            <tr align="right">
                <td>
                    <label for="tbUserName">
                        Корисничко име:</label>
                    <asp:TextBox ID="tbUserName" runat="server" Width="110px"></asp:TextBox>
                </td>
            </tr>
            <tr align="right">
                <td>
                    <label for="tbPassword">
                        Лозинка:</label>
                    <asp:TextBox ID="tbPassword" runat="server" TextMode="Password" Width="110px"></asp:TextBox>
                </td>
            </tr>
            </table>
            <table width="100%">
            <tr align="right">
                <td>
                    <asp:Label ID="lblMessage" ForeColor="Red" runat="server"></asp:Label>
                </td>
            </tr>
            <tr align="right">
                <td>
                    <asp:Button ID="btnLogin" runat="server" Text="Најави се" OnClick="btnLogin_Click" />
                </td>
            </tr>
        </table>
    </fieldset>
</div>
