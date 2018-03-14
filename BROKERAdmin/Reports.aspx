<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Reports.aspx.cs" Inherits="BROKERAdmin_Reports" Title="Извештаи" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="tabeliFrame">
        <div id="header">
            <div id="content">
                Извештаи
            </div>
        </div>
        <div id="contentOuter">
            <div id="contentInner">
                <table border="0px" cellpadding="5" cellspacing="0" width="100%">
                    <tr>
                        <td colspan="2">
                            <b>Од датум</b>
                            <asp:TextBox ID="tbStartDate" runat="server"></asp:TextBox>
                            <asp:CompareValidator ID="cvStartDate" runat="server" ControlToValidate="tbStartDate"
                                Display="Dynamic" ErrorMessage="*" Operator="DataTypeCheck" Type="Date"></asp:CompareValidator>
                            <asp:RequiredFieldValidator ID="rfvStartDate" runat="server" ControlToValidate="tbStartDate"
                                Display="Dynamic" ErrorMessage="*"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <b>До датум</b>
                            <asp:TextBox ID="tbEndDate" runat="server"></asp:TextBox>
                            <asp:CompareValidator ID="cvEndDate" runat="server" ControlToValidate="tbEndDate"
                                Display="Dynamic" ErrorMessage="*" Operator="DataTypeCheck" Type="Date"></asp:CompareValidator>
                            <asp:RequiredFieldValidator ID="rfvEndDate" runat="server" ControlToValidate="tbEndDate"
                                Display="Dynamic" ErrorMessage="*"></asp:RequiredFieldValidator>
                            <asp:CompareValidator ID="cvGraterEndDate" runat="server" ControlToValidate="tbEndDate"
                                Display="Dynamic" ErrorMessage="*" Operator="GreaterThanEqual" ControlToCompare="tbStartDate"></asp:CompareValidator>
                        </td>
                    </tr>
                    <tr class="altenativeRow">
                        <td>
                            <asp:Label ID="lblInsuranceType" runat="server" Text="Тип на осигурување" Width="200"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlInsuranceTypes" runat="server" DataSourceID="odsInsuranceTypes"
                                DataTextField="Name" DataValueField="ID" AppendDataBoundItems="true" Width="200">
                                <asp:ListItem Value="0" Text="Сите"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:ObjectDataSource ID="odsInsuranceTypes" runat="server" SelectMethod="Select"
                                TypeName="Broker.DataAccess.InsuranceType"></asp:ObjectDataSource>
                        </td>
                        <td>
                            <asp:Button ID="btnPrintByInsuranceType" runat="server" Text="Печати" OnClick="btnPrintByInsuranceType_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblInsuranceSubType" runat="server" Text="Подтип на осигурување" Width="200"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlInsuranceSubTypes" runat="server" DataSourceID="odsInsuranceSubTypes"
                                DataTextField="Description" DataValueField="ID" Width="200">
                            </asp:DropDownList>
                            <asp:ObjectDataSource ID="odsInsuranceSubTypes" runat="server" SelectMethod="Select"
                                TypeName="Broker.DataAccess.InsuranceSubType"></asp:ObjectDataSource>
                        </td>
                        <td>
                            <asp:Button ID="btnPrintByInsuranceSubType" runat="server" Text="Печати" OnClick="btnPrintByInsuranceSubType_Click" />
                        </td>
                    </tr>
                    <tr class="altenativeRow">
                        <td>
                            <asp:Label ID="lblUsers" runat="server" Text="Корисник" Width="200"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlUsers" runat="server" DataSourceID="odsUsers" DataTextField="UserName"
                                DataValueField="ID" Width="200">
                            </asp:DropDownList>
                            <asp:ObjectDataSource ID="odsUsers" runat="server" SelectMethod="GetBrokersAndBrokerAdmins"
                                TypeName="Broker.DataAccess.User"></asp:ObjectDataSource>
                        </td>
                        <td>
                            <asp:Button ID="btnPrintByUser" runat="server" Text="Печати" OnClick="btnPrintByUser_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblBranches" runat="server" Text="Филијала" Width="200"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlBranches" runat="server" DataSourceID="odsBranches" DataTextField="Name"
                                DataValueField="ID" Width="200px">
                            </asp:DropDownList>
                            <asp:ObjectDataSource ID="odsBranches" runat="server" SelectMethod="Select" TypeName="Broker.DataAccess.Branch">
                            </asp:ObjectDataSource>
                        </td>
                        <td>
                            <asp:Button ID="btnPrintByBranch" runat="server" Text="Печати" OnClick="btnPrintByBranch_Click" />
                        </td>
                    </tr>
                    <tr class="altenativeRow">
                        <td>
                            <asp:Label ID="lblCompanies" runat="server" Text="Компанија" Width="200"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlCompanies" runat="server" DataSourceID="odsCompanies" DataTextField="Name"
                                DataValueField="ID" Width="200px">
                            </asp:DropDownList>
                            <asp:ObjectDataSource ID="odsCompanies" runat="server" SelectMethod="Select"
                                TypeName="Broker.DataAccess.InsuranceCompany"></asp:ObjectDataSource>
                        </td>
                        <td>
                            <asp:Button ID="btnPrintByCompany" runat="server" Text="Печати" OnClick="btnPrintByCompany_Click" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
</asp:Content>
