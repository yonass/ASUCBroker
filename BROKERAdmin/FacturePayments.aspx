<%@ Page Title="Евидентирање на наплата по фактури" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="FacturePayments.aspx.cs" Inherits="BROKERAdmin_FacturePayments" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table>
        <tr>
            <td style="width: 150px">
                Број на фактура
            </td>
            <td>
                <asp:TextBox ID="tbFactureNumber" runat="server"></asp:TextBox>
                <asp:Button ID="btnSearch" runat="server" Text="..." OnClick="btnSearch_Click" CausesValidation="false" />
            </td>
        </tr>
        <tr>
            <td style="width: 150px">
                Осигурителна компанија
            </td>
            <td>
                <asp:TextBox ID="tbInsuranceCompany" ReadOnly="true" Enabled="false" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <br />
    <asp:Label ID="lblFeedback" runat="server" Text="" ForeColor="Red" Font-Bold="true"></asp:Label>
    <br />
    <div id="tabeliFrame">
        <div id="header">
            <div id="content">
                <asp:Label ID="lblFacturePreview" runat="server" Text="Преглед на фактура"></asp:Label>
            </div>
        </div>
        <div id="contentOuter">
            <div id="contentInner">
                <asp:GridView ID="gvFactureItemsPreview" runat="server" DataSourceID="odsFactureItemsPreview"
                    AutoGenerateColumns="False" GridLines="None" EmptyDataText="Нема записи!">
                    <Columns>
                        <asp:TemplateField HeaderText="Тип на осигурување">
                            <ItemTemplate>
                                <asp:TextBox ID="tbDescription" runat="server" Text='<%#Eval("Description") %>' ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Број на полиси">
                            <ItemTemplate>
                                <asp:TextBox ID="tbCount" runat="server" Text='<%#Eval("Count") %>' ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Премија">
                            <ItemTemplate>
                                <asp:TextBox ID="tbPremiumValue" runat="server" Text='<%#Eval("PremiumValue", "{0:#,0.00}") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Брокеража">
                            <ItemTemplate>
                                <asp:TextBox ID="tbBrokerageValue" runat="server" Text='<%#Eval("BrokerageValue", "{0:#,0.00}") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:ObjectDataSource ID="odsFactureItemsPreview" runat="server" SelectMethod="GetByFacture"
                    TypeName="Broker.DataAccess.FactureItem" OnSelecting="odsFactureItemsPreview_Selecting">
                    <SelectParameters>
                        <asp:Parameter Name="factureID" Type="Int32" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </div>
        </div>
    </div>
    <table>
        <tr>
            <td>
                Вкупен износ на фактура
            </td>
            <td>
                <asp:TextBox ID="tbFactureTotalCost" Width="100px" runat="server" ReadOnly="true"></asp:TextBox>
            </td>
            <td>
                Платено
            </td>
            <td>
                <asp:TextBox ID="tbFactureTotalPaidValue" Width="100px" runat="server" ReadOnly="true"></asp:TextBox>
            </td>
            <td>
                За плаќање
            </td>
            <td>
                <asp:TextBox ID="tbFactureForPaidValue" Width="100px" runat="server" ReadOnly="true"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                Дата на уплата:
            </td>
            <td>
                <asp:TextBox ID="tbDateOfPayment" Width="100px" runat="server"></asp:TextBox>
            </td>
            <td>
                Износ
            </td>
            <td>
                <asp:TextBox ID="tbValueOfPayment" Width="100px" runat="server"></asp:TextBox>
                <asp:CompareValidator ID="cvPaymentValue" runat="server" ControlToValidate="tbValueOfPayment"
                    ErrorMessage="*" Operator="DataTypeCheck" Type="Double" Display="Dynamic"></asp:CompareValidator>
                <asp:RequiredFieldValidator ID="rfvValueOfPayment" runat="server" ControlToValidate="tbValueOfPayment"
                    Display="Dynamic" ErrorMessage="*"></asp:RequiredFieldValidator>
            </td>
            <td>
                <asp:Button ID="btnGenerate" runat="server" Text="Пресметај" OnClick="btnGenerate_Click"
                    Enabled="false" />
            </td>
        </tr>
    </table>
    <div id="tabeliFrame">
        <div id="header">
            <div id="content">
                <asp:Label ID="Label1" runat="server" Text="Плаќања на фактурата"></asp:Label>
            </div>
        </div>
        <div id="contentOuter">
            <div id="contentInner">
                <asp:GridView ID="GridViewPayments" runat="server" DataSourceID="odsPaidPayments"
                    AutoGenerateColumns="False" GridLines="None" EmptyDataText="Нема записи!">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:TextBox ID="tbFactureItemID" runat="server" Text='<%#Eval("FactureItemID") %>'
                                    ReadOnly="true" Visible="false" Width="0px"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Тип на осигурување">
                            <ItemTemplate>
                                <asp:TextBox ID="tbInsuranceSubType" runat="server" Text='<%#Eval("FactureItem.Description") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Датум на плаќање">
                            <ItemTemplate>
                                <asp:TextBox ID="tbDate" runat="server" Text='<%#Eval("PaidDate", "{0:d}") %>' ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Износ">
                            <ItemTemplate>
                                <asp:TextBox ID="tbValue" runat="server" Text='<%#Eval("PaidValue") %>'></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:ObjectDataSource ID="odsPaidPayments" runat="server" SelectMethod="GetGroupedByFactureID"
                    TypeName="Broker.DataAccess.FactureCollectedPaidValue" OnSelecting="odsPaidPayments_Selecting">
                    <SelectParameters>
                        <asp:Parameter Name="factureID" Type="Int32" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </div>
        </div>
    </div>
    <asp:Button ID="btnInsert" runat="server" Text="Сними" OnClick="btnInsert_Click"
        Enabled="false" />
</asp:Content>
