<%@ Page Title="Изработка на групна излезна фактура кон клиент" Language="C#" MasterPageFile="~/MasterPage.master"
    AutoEventWireup="true" CodeFile="GroupFactures.aspx.cs" Inherits="BROKERAdmin_GroupFactures" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="paddingKontroli">
        <table>
            <tr>
                <td style="width: 100px;">
                    <asp:Label ID="lblStartDate" runat="server" Text="Од датум"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="tbStartDate" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvStartDate" runat="server" ErrorMessage="*" Display="Dynamic"
                        ControlToValidate="tbStartDate"></asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="cvStartDate" runat="server" ErrorMessage="*" Display="Dynamic"
                        ControlToValidate="tbStartDate" Operator="DataTypeCheck" Type="Date"></asp:CompareValidator>
                </td>
                <td>
                    <asp:Label ID="lblEndDate" runat="server" Text="До датум"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="tbEndDate" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvEndDate" ControlToValidate="tbEndDate" runat="server"
                        ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="cvEndDate" runat="server" ErrorMessage="*" Display="Dynamic"
                        ControlToValidate="tbEndDate" Operator="DataTypeCheck" Type="Date"></asp:CompareValidator>
                </td>
                <td>
                    <asp:Button ID="btnCheck" runat="server" OnClick="btnCheck_Click" Text="Провери" />
                </td>
            </tr>
        </table>
        <table>
            <tr>
                <td style="width: 100px;">
                    <asp:Label ID="lblClient" runat="server" Text="Договорувач" Visible="false"></asp:Label>
                </td>
                <td visible="false">
                </td>
                <td>
                    <asp:DropDownList ID="ddlClients" runat="server" DataTextField="Name" DataValueField="ID"
                        Visible="false" AutoPostBack="True" OnSelectedIndexChanged="ddlClients_SelectedIndexChanged">
                    </asp:DropDownList>
                    <asp:ObjectDataSource ID="odsClients" runat="server" SelectMethod="Select" TypeName="Broker.DataAccess.Client">
                    </asp:ObjectDataSource>
                </td>
                <td>
                </td>
            </tr>
        </table>
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblError" runat="server" Text="Не постојат нефактурирани полиси во дадениот период!"
                        Visible="false"></asp:Label>
                </td>
            </tr>
        </table>
        <table>
            <tr>
                <td style="width: 100px;">
                </td>
                <td>
                    <asp:GridView ID="gvNewPolicies" runat="server" AllowPaging="True" AllowSorting="True"
                        AutoGenerateColumns="False" Caption="Нефактурирани полиси" EmptyDataText="Нема записи!"
                        RowStyle-CssClass="rowFacture" CssClass="gridFacture" GridLines="None" OnPageIndexChanging="gvNewPolicies_PageIndexChanging"
                        DataKeyNames="ID" PageSize="10">
                        <RowStyle CssClass="rowFacture"></RowStyle>
                        <Columns>
                            <asp:BoundField HeaderText="" DataField="ID" ItemStyle-ForeColor="Transparent" ItemStyle-Width="0px" />
                            <asp:BoundField HeaderText="Број на полиса" DataField="PolicyNumber" SortExpression="PolicyNumber" />
                            <asp:BoundField HeaderText="Премија за наплата" DataField="PremiumValue" SortExpression="PremiumValue"
                                DataFormatString="{0:#,0.00}" ItemStyle-CssClass="currencyClass" />
                            <asp:TemplateField HeaderText="За фактурирање">
                                <ItemTemplate>
                                    <asp:CheckBox ID="cbIsForFacturing" runat="server" Checked='<%#Bind("IsForFacturing") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <PagerStyle HorizontalAlign="Center" />
                        <PagerSettings FirstPageText="<< Прва " PreviousPageText="< Претходна " NextPageText=" Следна >"
                            LastPageText=" Последна >>" Mode="NextPreviousFirstLast" />
                    </asp:GridView>
                </td>
            </tr>
        </table>
        <table>
            <tr>
                <td style="width: 100px;">
                </td>
                <td>
                    <asp:Button ID="btnInsert" runat="server" OnClick="btnInsert_Click" Text="Направи фактура"
                        Visible="false" />
                </td>
            </tr>
            <tr>
                <td style="width: 100px;">
                </td>
                <td>
                    <asp:Button ID="btnPrint" runat="server" OnClick="btnPrint_Click" Text="Печати фактура"
                        Visible="false" />
                    <asp:Button ID="btnPrintAnex" runat="server" OnClick="btnPrintAnex_Click" Text="Печати анекс"
                        Visible="false" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
