<%@ Page Title="Евидентирање на винкулација" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="RightRestrictions.aspx.cs" Inherits="Broker_RightRestrictions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .style1
        {
            width: 374px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table width="695px">
        <tr class="light-background">
            <td style="width: 270px">
                <asp:Label ID="lblRestrictionNumber" runat="server" Text="Број на винкулација"></asp:Label>
            </td>
            <td class="style1">
                <asp:TextBox ID="tbRestrictionNumber" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvRestrictionNumber" runat="server" ControlToValidate="tbRestrictionNumber"
                    Display="Dynamic" ErrorMessage="*"></asp:RequiredFieldValidator>
                    <asp:Label ID="lblError" Visible="false" runat="server" ></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="width: 270px">
                <asp:Label ID="lblPolicyNumber" runat="server" Text="Број на полиса"></asp:Label>
            </td>
            <td class="style1">
                <asp:TextBox ID="tbPolicyNumber" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvPolicyNumber" runat="server" ControlToValidate="tbPolicyNumber"
                    Display="Dynamic" ErrorMessage="*"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr class="light-background">
            <td style="width: 270px">
                <asp:Label ID="lblInsuranceType" runat="server" Text="Тип на осигурување"></asp:Label>
            </td>
            <td class="style1">
                <asp:DropDownList ID="ddlInsuranceType" runat="server" DataSourceID="odsInsuranceType"
                    Width="425px" DataTextField="Name" DataValueField="ID" AutoPostBack="true" OnSelectedIndexChanged="ddlInsuranceTypeSelectedIndexChanged">
                </asp:DropDownList>
                <asp:ObjectDataSource ID="odsInsuranceType" runat="server" SelectMethod="GetForPolicy"
                    TypeName="Broker.DataAccess.InsuranceType"></asp:ObjectDataSource>
            </td>
        </tr>
        <tr>
            <td style="width: 270px">
                <asp:Label ID="lblInsuranceSubType" runat="server" Text="Подтип на осигурување"></asp:Label>
            </td>
            <td class="style1">
                <asp:DropDownList ID="ddlInsuranceSubTypes" runat="server" DataSourceID="odsInsuranceSubTypes"
                    DataTextField="Description" DataValueField="ID" AutoPostBack="true" OnSelectedIndexChanged="ddlInsuranceSubType_selecteIndexChanged"
                    Width="425px">
                </asp:DropDownList>
                <asp:ObjectDataSource ID="odsInsuranceSubTypes" runat="server" SelectMethod="GetByInsuranceTypeAndExistingDeals"
                    TypeName="Broker.DataAccess.InsuranceSubType">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="ddlInsuranceType" Name="insuranceTypeID" PropertyName="SelectedValue"
                            Type="Int32" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </td>
        </tr>
        <tr class="light-background">
            <td style="width: 270px">
                <asp:Label ID="lblInsuranceCompanies" runat="server" Text="Осигурителна компанија"></asp:Label>
            </td>
            <td class="style1">
                <asp:DropDownList ID="ddlInsuranceCompany" runat="server" DataSourceID="odsInsuranceCompany"
                    DataTextField="Name" DataValueField="ID" Width="250px" OnSelectedIndexChanged="ddlInsuranceCompanyIndexChanged"
                    AutoPostBack="true">
                </asp:DropDownList>
                <asp:ObjectDataSource ID="odsInsuranceCompany" runat="server" SelectMethod="GetByInsuranceSubType"
                    TypeName="Broker.DataAccess.InsuranceCompany">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="ddlInsuranceSubTypes" Name="insuranceSubTypeID"
                            PropertyName="SelectedValue" Type="Int32" />
                    </SelectParameters>
                </asp:ObjectDataSource>
                <asp:Button ID="btnSearch" runat="server" Text="Барај" OnClick="btnSearch_Click"
                    CausesValidation="false" />
            </td>
        </tr>
        </table>
        <table>
        <tr>
            <td>
                <asp:Label ID="lblOwnerEMBG" runat="server" Text="EMBG"></asp:Label>
            </td>
            <td class="style1">
                <asp:TextBox ID="tbOwnerEMBG" runat="server" ReadOnly="true"></asp:TextBox>
            </td>
            <td>
                <asp:Label ID="lblOwnerName" runat="server" Text="Осигуреник"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="tbOwnerName" runat="server" ReadOnly="true"></asp:TextBox>
            </td>
        </tr>
        <tr class="light-background">
            <td>
                <asp:Label ID="lblStartDate" runat="server" Text="Почетна Дата"></asp:Label>
            </td>
            <td class="style1">
                <asp:TextBox ID="tbStartDate" runat="server" ReadOnly="true"></asp:TextBox>
            </td>
            <td>
                <asp:Label ID="lblEndDate" runat="server" Text="Крајна Дата"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="tbEndDate" runat="server" ReadOnly="true"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblBank" runat="server" Text="Банка"></asp:Label>
            </td>
            <td class="style1">
                <asp:DropDownList ID="ddlBanks" runat="server" DataTextField="Name" DataSourceID="odsBanks"
                    DataValueField="ID">
                </asp:DropDownList>
                <asp:ObjectDataSource ID="odsBanks" runat="server" SelectMethod="GetActiveBanks"
                TypeName="Broker.DataAccess.Bank"></asp:ObjectDataSource>
            </td>
            
        </tr>
        <tr class="light-background">
            <td>
                <asp:Label ID="lblDescription" runat="server" Text="Предмет"></asp:Label>
            </td>
            <td class="style1">
                <asp:TextBox ID="tbDescription" runat="server"></asp:TextBox>
            </td>
            <td>
                <asp:Label ID="lblCoverage" runat="server" Text="Сума"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="tbCoverage" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="btnInsert" runat="server" Text="Сними" OnClick="btnInsert_Click" />
            </td>
        </tr>
         <tr>
            <td>
                <asp:Label ID="lblSuccess" runat="server" Text="Винкулацијата е успешно внесена" Visible="false"/>
            </td>
        </tr>
    </table>
</asp:Content>
