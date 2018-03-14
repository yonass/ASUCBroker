<%@ Page Title="Уплата по полиси" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="CurrentSaleValues.aspx.cs" Inherits="Broker_CurrentSaleValues" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table>
        <tr>
            <td>
                Датум
                <asp:TextBox ID="tbCurrentDate" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvCurrentDate" runat="server" Display="Dynamic"
                    ErrorMessage="*" ControlToValidate="tbCurrentDate"></asp:RequiredFieldValidator>
                <asp:CompareValidator ID="cvCurrentDate" runat="server" Display="Dynamic" ErrorMessage="*"
                    ControlToValidate="tbCurrentDate" Operator="DataTypeCheck" Type="Date"></asp:CompareValidator>
                <asp:Button runat="server" Text="Освежи" ID="btnRefresh" OnClick="btnRefresh_Click" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:GridView ID="gvCashPaidPolicies" runat="server" AutoGenerateColumns="False"
                    DataSourceID="odsCashPaidPolicies" AllowPaging="true" AllowSorting="false" PageSize="10"
                    GridLines="None" OnPageIndexChanged="gvCashPaidPolicies_PageIndexChanged" OnPageIndexChanging="gvCashPaidPolicies_PageIndexChanging"
                    Caption="Полиси платени во готово" EmptyDataText="Нема записи" CssClass="grid"
                    Width="260px" RowStyle-CssClass="row">
                    <Columns>
                        <asp:BoundField DataField="PolicyNumber" HeaderText="Број на полиса" SortExpression="PolicyNumber" />
                        <asp:BoundField DataField="TotalSum" HeaderText="Премија" SortExpression="TotalSum" />
                        <asp:CheckBoxField DataField="Discard" HeaderText="Сторно" SortExpression="Discard" />
                    </Columns>
                </asp:GridView>
                <asp:ObjectDataSource ID="odsCashPaidPolicies" runat="server" SelectMethod="GetCashPaidPoliciesInPeriodForUser"
                    TypeName="Broker.Controllers.ReportControllers.CurrentSaleController">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="tbCurrentDate" Name="currentDate" PropertyName="Text"
                            Type="DateTime" />
                        <asp:SessionParameter SessionField="UserID" Name="userID" Type="Int32" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </td>
            <td>
                <asp:GridView ID="gvFacturePaidPolicies" runat="server" AutoGenerateColumns="False"
                    DataSourceID="odsFacturePaidPolicies" AllowPaging="true" AllowSorting="false"
                    PageSize="10" GridLines="None" OnPageIndexChanged="gvFacturePaidPolicies_PageIndexChanged"
                    OnPageIndexChanging="gvFacturePaidPolicies_PageIndexChanging" Caption="Полиси платени на фактура"
                    CssClass="grid" RowStyle-CssClass="row" EmptyDataText="Нема записи" Width="260px">
                    <Columns>
                        <asp:BoundField DataField="PolicyNumber" HeaderText="Број на полиса" SortExpression="PolicyNumber" />
                        <asp:BoundField DataField="TotalSum" HeaderText="Премија" SortExpression="TotalSum" />
                        <asp:CheckBoxField DataField="Discard" HeaderText="Сторно" SortExpression="Discard" />
                    </Columns>
                </asp:GridView>
                <asp:ObjectDataSource ID="odsFacturePaidPolicies" runat="server" SelectMethod="GetFacturePaidPoliciesInPeriodForUser"
                    TypeName="Broker.Controllers.ReportControllers.CurrentSaleController">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="tbCurrentDate" Name="currentDate" PropertyName="Text"
                            Type="DateTime" />
                        <asp:SessionParameter SessionField="UserID" Name="userID" Type="Int32" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </td>
        </tr>
    </table>
    <table>
        <tr>
            <td>
                <b>Вкупно платено во кеш:
                    <asp:Label ID="lblCashPaidValues" runat="server" Text=""></asp:Label>
                </b>
            </td>
        </tr>
        <tr>
            <td>
                <b>Вкупно платено на картичка:
                    <asp:Label ID="lblCreditCardPaidValues" runat="server" Text=""></asp:Label>
                </b>
                <asp:Panel ID="pnlCreditCardPayments" runat="server" CssClass="grid">
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td>
                <b>Вкупно платено на фактура:
                    <asp:Label ID="lblFacturePaidValues" runat="server" Text=""></asp:Label>
                </b>
            </td>
        </tr>
    </table>
</asp:Content>
