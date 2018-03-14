<%@ Page Title="Статистички извештаи" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="StatisticalReports.aspx.cs" Inherits="BROKERAdmin_StatisticalReports" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="tabeliFrame">
        <div id="header">
            <div id="content">
                Статистички извештаи
            </div>
        </div>
        <div id="contentOuter">
            <div id="contentInner">
                <table border="0px" cellpadding="5" cellspacing="0" width="100%">
                    <tr>
                        <td>
                            <b>Од датум</b>
                            <asp:TextBox ID="tbStartDate" runat="server" Width="120px"></asp:TextBox>
                            <asp:CompareValidator ID="cvStartDate" runat="server" ControlToValidate="tbStartDate"
                                Display="Dynamic" ErrorMessage="*" Operator="DataTypeCheck" Type="Date"></asp:CompareValidator>
                            <asp:RequiredFieldValidator ID="rfvStartDate" runat="server" ControlToValidate="tbStartDate"
                                Display="Dynamic" ErrorMessage="*"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <b>До датум</b>
                            <asp:TextBox ID="tbEndDate" runat="server" Width="120px"></asp:TextBox>
                            <asp:CompareValidator ID="cvEndDate" runat="server" ControlToValidate="tbEndDate"
                                Display="Dynamic" ErrorMessage="*" Operator="DataTypeCheck" Type="Date"></asp:CompareValidator>
                            <asp:RequiredFieldValidator ID="rfvEndDate" runat="server" ControlToValidate="tbEndDate"
                                Display="Dynamic" ErrorMessage="*"></asp:RequiredFieldValidator>
                            <asp:CompareValidator ID="cvGraterEndDate" runat="server" ControlToValidate="tbEndDate"
                                Display="Dynamic" ErrorMessage="*" Operator="GreaterThanEqual" ControlToCompare="tbStartDate"></asp:CompareValidator>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr class="altenativeRow">
                        <td>
                            <asp:Label ID="lblPremium" runat="server" Text="Бруто полисирана премија по класи "
                                Width="200"></asp:Label>
                        </td>
                        <td>
                        </td>
                        <td>
                            <asp:Button ID="btnPrintByPremium" runat="server" Text="Печати" OnClick="btnPrintByPremium_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblNumber" runat="server" Text="Број на полиси по класи" Width="200"></asp:Label>
                        </td>
                        <td>
                        </td>
                        <td>
                            <asp:Button ID="btnPrintByNumber" runat="server" Text="Печати" OnClick="btnPrintByNumber_Click" />
                        </td>
                    </tr>
                    <tr class="altenativeRow">
                        <td>
                            <asp:Label ID="lblCompanyGroupReport" runat="server" Text="Извештај по осигурителни компании"
                                Width="200"></asp:Label>
                        </td>
                        <td>
                        </td>
                        <td>
                            <asp:Button ID="btnCompanyGroup" runat="server" Text="Печати" OnClick="btnCompanyGroup_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblASOReport" runat="server" Text="Квартален извештај за брокерските друштво (за АСО)"
                                Width="200"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblInsuranceCompanyies" runat="server" Text="Осигурителна компанија"
                                Width="200"></asp:Label>
                            <asp:DropDownList ID="ddlInsuranceCompanies" runat="server" AppendDataBoundItems="True"
                                DataSourceID="odsInsuranceCompanies" DataTextField="Name" DataValueField="ID">
                                <asp:ListItem Text="- Сите -" Value="0" Selected="True"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:ObjectDataSource ID="odsInsuranceCompanies" runat="server" TypeName="Broker.DataAccess.InsuranceCompany"
                                DataObjectTypeName="Broker.DataAccess.InsuranceCompany" SelectMethod="GetActiveInsuranceCompanies">
                            </asp:ObjectDataSource>
                        </td>
                        <td>
                            <asp:Button ID="btnReportForASO" runat="server" Text="Печати" OnClick="btnReportForASO_Click" />
                        </td>
                    </tr>
                   <%-- <tr class="altenativeRow">
                        <td>
                            <asp:Label ID="lblReportFinCard" runat="server" Text="Финансова картица"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlFinCardType" runat="server">
                                <asp:ListItem Selected="True" Text="По датум на издавање" Value="FinCardByApplicationDate"></asp:ListItem>
                                <asp:ListItem Text="По датуми на доспевање/уплата" Value="FinCardByPaidDates"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Button ID="btnFinCard" runat="server" Text="Печати" OnClick="btnFinCard_Click" />
                        </td>
                    </tr>--%>
                </table>
            </div>
        </div>
    </div>
</asp:Content>
