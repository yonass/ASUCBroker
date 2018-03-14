<%@ Page Title="Видливи web страни за корисник" Language="C#" MasterPageFile="~/MasterPage.master"
    AutoEventWireup="true" CodeFile="UsersWebPages.aspx.cs" Inherits="UserManagement_UsersWebPages" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="paddingKontroli">
        <table>
            <tr>
                <td>
                    Корисничко име:
                    <asp:DropDownList ID="ddlUsers" runat="server" DataSourceID="ObjectDataSource2" DataTextField="Username"
                        DataValueField="ID" AutoPostBack="true">
                    </asp:DropDownList>
                    <asp:ObjectDataSource ID="ObjectDataSource2" runat="server" SelectMethod="GetManageChildUsers"
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
                    Web-страни (Функции):
                </td>
            </tr>
            <tr>
                <td>
                    <asp:DataList ID="DataList1" runat="server" DataSourceID="FunctionsDataSource">
                        <ItemTemplate>
                            <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# Eval("Checked") %>' Text='<%# Eval("Name") %>' />
                            <br />
                            <br />
                        </ItemTemplate>
                    </asp:DataList>
                    <asp:ObjectDataSource ID="FunctionsDataSource" runat="server" SelectMethod="GetFunctionVisibilityInfo"
                        TypeName="Broker.Controllers.UserManagement.WebPagesVisibilityContoller">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="ddlUsers" DefaultValue="0" Name="userId" PropertyName="SelectedValue"
                                Type="Int32" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <asp:Button ID="SavaButton" runat="server" OnClick="SavaButton_Click" Text="Зачувај" />
                </td>
                <td>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
