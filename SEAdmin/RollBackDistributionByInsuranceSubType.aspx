<%@ Page Title="Раздолжувања по подкласа на осигурување" Language="C#" MasterPageFile="~/MasterPage.master"
    AutoEventWireup="true" CodeFile="RollBackDistributionByInsuranceSubType.aspx.cs"
    Inherits="SEAdmin_RollBackDistributionByInsuranceSubType" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table>
        <tr>
            <td>
                <asp:Label ID="lblInsuranceCompany" runat="server" Text="Осигурителна Компанија"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="ddlInsuranceCompany" runat="server" DataSourceID="InsuranceCompanyDataSource"
                    DataTextField="Name" DataValueField="ID">
                </asp:DropDownList>
                <asp:ObjectDataSource ID="InsuranceCompanyDataSource" runat="server" SelectMethod="GetActiveInsuranceCompanies"
                    TypeName="Broker.DataAccess.InsuranceCompany"></asp:ObjectDataSource>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="ldlStartDate" runat="server" Text="Датум од"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="tbStartDate" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblEndDate" runat="server" Text="Датум до"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="tbEndDate" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table>
        <tr>
            <td style="width: 340px;">
                <asp:Label ID="lblPreviosSubTypes" runat="server" Text="Подкласи од претходни месеци"
                    Visible="false"></asp:Label>
            </td>
            <td style="width: 340px;">
                <asp:Label ID="lblCurentSubTypes" runat="server" Text="Подкласи од селектираниот период"
                    Visible="false"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:DropDownList ID="ddlPreviosSubTypes" runat="server" DataTextField="Description"
                    DataValueField="ID" Visible="false" Width="340px" AutoPostBack="true" OnSelectedIndexChanged="ddlPreviousSubTypes_Changed">
                </asp:DropDownList>
            </td>
            <td>
                <asp:DropDownList ID="ddlCurrentSubTypes" runat="server" DataTextField="Description"
                    DataValueField="ID" Visible="false" Width="340px" AutoPostBack="true" OnSelectedIndexChanged="ddlCurrentSubTypes_Changed">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                <asp:GridView ID="gvOldPolicies" runat="server" AllowPaging="True" AllowSorting="True"
                    AutoGenerateColumns="False" Caption="Полиси од претходни месеци" EmptyDataText="Нема записи од претходниот период кои го задоволуваат критериумот на пребарување!"
                    RowStyle-CssClass="rowFacture" CssClass="gridFacture" GridLines="None" OnPageIndexChanging="gvOldPolicies_PageIndexChanging"
                    DataKeyNames="ID" PageSize="10">
                    <RowStyle CssClass="rowFacture"></RowStyle>
                    <Columns>
                        <asp:BoundField HeaderText="" DataField="ID" ShowHeader="false" ItemStyle-Width="1px" />
                        <asp:BoundField HeaderText="Број на полиса" DataField="PolicyNumber" SortExpression="PolicyNumber" />
                        <asp:TemplateField HeaderText="За раздолжување">
                            <ItemTemplate>
                                <asp:CheckBox ID="cbIsForRollBack" runat="server" Checked='<%#Bind("IsForRollBack") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <PagerStyle HorizontalAlign="Center" />
                    <PagerSettings FirstPageText="<< Прва " PreviousPageText="< Претходна " NextPageText=" Следна >"
                        LastPageText=" Последна >>" Mode="NextPreviousFirstLast" />
                </asp:GridView>
            </td>
            <%--</tr>
        <tr>--%>
            <td>
                <asp:GridView ID="gvNewPolicies" runat="server" AllowPaging="True" AllowSorting="True"
                    AutoGenerateColumns="False" Caption="Полиси од селектираниот период" EmptyDataText="Нема записи кои го задоволуваат критериумот на пребарување!"
                    RowStyle-CssClass="row" CssClass="gridFacture" GridLines="None" OnPageIndexChanging="gvNewPolicies_PageIndexChanging"
                    DataKeyNames="ID" PageSize="10">
                    <RowStyle CssClass="row"></RowStyle>
                    <Columns>
                        <asp:BoundField HeaderText="" DataField="ID" />
                        <asp:BoundField HeaderText="Број на полиса" DataField="PolicyNumber" SortExpression="PolicyNumber" />
                        <asp:TemplateField HeaderText="За раздолжување">
                            <ItemTemplate>
                                <asp:CheckBox ID="cbIsForRollBack" runat="server" Checked='<%#Bind("IsForRollBack") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <PagerStyle HorizontalAlign="Center" />
                    <PagerSettings FirstPageText="<< Прва " PreviousPageText="< Претходна " NextPageText=" Следна >"
                        LastPageText=" Последна >>" Mode="NextPreviousFirstLast" />
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="btnCreate" runat="server" OnClick="btnCreateClick" Text="Креирај"
                    Width="200px" />
                <%--</td>
            <td>--%>
                <asp:Button ID="btnPrintPolicies" runat="server" OnClick="btnPrintPoliciesClick"
                    Text="Печати" Width="200px" Visible="false" />
            </td>
            <td>
                <asp:Button ID="btnInsert" runat="server" OnClick="btnInsert_Click" Text="Сними"
                    Width="200px" Visible="false" />
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblFeedBack" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="btnCreateRightRestriction" runat="server" OnClick="btnCreateRightRestrictions_Click"
                    Text="Креирај за винкулации" Visible="false" Width="200px" />
            </td>
            <td>
                <asp:Button ID="btnInsertRightRestrictions" runat="server" OnClick="btnInsertRightRestrictions_Click"
                    Text="Сними" Width="200px" Visible="false" />
            </td>
            <td>
            </td>
        </tr>
    </table>
</asp:Content>
