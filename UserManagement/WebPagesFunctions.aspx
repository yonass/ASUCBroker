<%@ Page Title="Web-страна - функција" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="WebPagesFunctions.aspx.cs" Inherits="UserManagement_WebPagesFunctions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
 <table>
        <tr>
            <td>
                <asp:ListBox ID="FunctionsList" runat="server" 
                    DataSourceID="FunctionsDataSource" DataTextField="Name" DataValueField="ID" 
                    Height="352px"></asp:ListBox>
                <asp:ObjectDataSource ID="FunctionsDataSource" runat="server" 
                    SelectMethod="GetAllFunctions" 
                    TypeName="Broker.Controllers.UserManagement.WebPagesController">
                </asp:ObjectDataSource>
            </td>
            <td></td>
            <td>
                <asp:ListBox ID="WebPagesList" runat="server" DataSourceID="WebPagesInfo" 
                    DataTextField="Name" DataValueField="ID" Height="328px"></asp:ListBox>
                <asp:ObjectDataSource ID="WebPagesInfo" runat="server" 
                    SelectMethod="GetAllWebPages" 
                    TypeName="Broker.Controllers.UserManagement.WebPagesController">
                </asp:ObjectDataSource>
            </td>
        </tr>
        <tr>
            <td></td>
            <td>
                <asp:Button ID="ButtonSave" runat="server" onclick="ButtonSave_Click" 
                    Text="Зачувај" />
            </td>
            <td></td>
        </tr>
    </table>
</asp:Content>

