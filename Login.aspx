<%@ Page Title="" Language="C#" MasterPageFile="~/Login.master" AutoEventWireup="true"
    CodeFile="Login.aspx.cs" Inherits="Login" %>

<%--<%@ Register Src="~/UserControls/LoginControls/LoginControl.ascx" TagName="LoginControl"
    TagPrefix="lc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <lc:LoginControl ID="LoginControl" runat="server" />
</asp:Content>--%>
<%@ Register Src="~/UserControls/LoginControls/LoginControl.ascx" TagName="LoginControl"
    TagPrefix="lc" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="loginBox">
        <div id="title">
            <div id="logo">
                <img src="_assets/img/imgInApp/euromakLogoMedium.png" width="100" height="100" alt="EUROMAK Broker" />
            </div>
            <div id="text">
                <br />
                АСУЦ
                <br />
                БРОКЕР
            </div>
        </div>
        <lc:LoginControl ID="LoginControl" runat="server" />
    </div>
</asp:Content>
