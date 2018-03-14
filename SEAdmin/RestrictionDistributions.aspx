<%@ Page Title="������� �� ��������� ����������" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="RestrictionDistributions.aspx.cs" Inherits="SEAdmin_RestrictionDistributions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="kontroli">
        <div id="button01">
            <asp:Button ID="btnNew" runat="server" ToolTip="��� �����" OnClick="btnNew_Click"
                CausesValidation="false" UseSubmitBehavior="false" CssClass="novZapis" BorderWidth="0px" />
        </div>
        <div id="button02">
            <asp:Button ID="btnPreview" runat="server" ToolTip="������" OnClick="btnPreview_Click"
                CausesValidation="false" UseSubmitBehavior="false" CssClass="osvezi" BorderWidth="0px" />
        </div>
        <div id="button03">
            <asp:Button ID="btnReport" runat="server" ToolTip="�������" CausesValidation="false"
                UseSubmitBehavior="false" OnClick="btnReport_Click" CssClass="izvestaj" BorderWidth="0px" />
        </div>
        <div id="button04">
            <asp:Button ID="btnSearch" runat="server" ToolTip="�������" OnClick="btnSearch_Click"
                CausesValidation="false" UseSubmitBehavior="false" CssClass="prebaraj" BorderWidth="0px" />
        </div>
    </div>
    <asp:MultiView ID="mvMain" runat="server">
        <asp:View ID="viewGrid" runat="server">
            <div id="tabeliFrame">
                <div id="header">
                    <div id="content">
                        �����������
                    </div>
                </div>
                <div id="contentOuter">
                    <div id="contentInner">
                        <cc1:GXGridView ID="gvDistributions" runat="server" DataSourceID="gvDataSource" DataKeyNames="ID"
                            Width="100%" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"
                            Caption="����������� �� ����������" EmptyDataText="���� ������ ��� �� ������������ ����������� �� �����������!"
                            OnRowCommand="GXGridView1_RowCommand" RowStyle-CssClass="row" CssClass="grid"
                            GridLines="None">
                            <Columns>
                                <asp:BoundField HeaderText="������������ ��������" DataField="InsuranceCompanyName"
                                    SortExpression="InsuranceCompanyName" />
                                <asp:BoundField HeaderText="��� " DataField="Number" SortExpression="Number" />
                                <asp:CheckBoxField HeaderText="�����������" DataField="IsUsed" SortExpression="IsUsed" />
                                <asp:BoundField HeaderText="���� �� ��������" DataField="Date" DataFormatString="{0:d}"
                                    SortExpression="Date" />
                               <%-- <asp:BoundField HeaderText="��� �� ��������" DataField="DocumentTypeDescription"
                                    SortExpression="DocumentTypeDescription" />--%>
                                <%--<asp:BoundField HeaderText="�����" DataField="UserName" SortExpression="UserName" />--%>
                            </Columns>
                            <PagerStyle HorizontalAlign="Center" />
                            <SelectedRowStyle CssClass="rowSelected" />
                            <PagerSettings FirstPageText="<< ���� " PreviousPageText="< ��������� " NextPageText=" ������ >"
                                LastPageText=" �������� >>" Mode="NextPreviousFirstLast" />
                        </cc1:GXGridView>
                        <cc1:GridViewDataSource ID="gvDataSource" runat="server" TypeName="Broker.DataAccess.ViewRightRestrictionDistribution"
                            OldValuesParameterFormatString="oldEntity">
                        </cc1:GridViewDataSource>
                        <div class="pager">
                            <div class="container">
                                <div style="float: left;">
                                    <cc1:PagerControl ID="myGridPager" runat="server" ControlToPage="gvDistributions" />
                                </div>
                                <div style="float: right;">
                                    <cc1:FilterControl ID="FilterControl1" runat="server" GridViewToFilter="gvDistributions"
                                        FilterDataSourceID="odsFilterGridView" OnFilter="FilterControl1_Filter">
                                        <cc1:FilterItem FieldName="����. ��������" PropertyName="InsuranceCompanyName" Comparator="StringComparators"
                                            DataType="String" />
                                        <cc1:FilterItem FieldName="���" PropertyName="Number" Comparator="StringComparators"
                                            DataType="String" />
                                        <cc1:FilterItem FieldName="�����������" PropertyName="IsUsed" Comparator="BooleanComparators"
                                            DataType="Boolean" />
                                        <cc1:FilterItem FieldName="����" PropertyName="Date" Comparator="NumericComparators"
                                            DataType="DateTime" />
                                        <cc1:FilterItem FieldName="��� �� ���.(���)" PropertyName="DocumentTypeCode" Comparator="StringComparators"
                                            DataType="String" />
                                        <cc1:FilterItem FieldName="��� �� ��������" PropertyName="DocumentTypeDescription"
                                            Comparator="StringComparators" DataType="String" />
                                    </cc1:FilterControl>
                                </div>
                                <cc1:FilterDataSource ID="odsFilterGridView" runat="server" TypeName="Broker.DataAccess.ViewRightRestrictionDistribution">
                                    <SelectParameters>
                                        <asp:ControlParameter Name="fArgument" ControlID="FilterControl1" PropertyName="FCFilterArgument" />
                                    </SelectParameters>
                                </cc1:FilterDataSource>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </asp:View>
        <asp:View ID="viewEdit" runat="server">
            <div class="paddingKontroli">
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblDocumentType" Text="��� �� ��������" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlDocumentType" runat="server" DataSourceID="odsDocumentTypes"
                                DataTextField="Description" DataValueField="ID">
                            </asp:DropDownList>
                            <asp:ObjectDataSource ID="odsDocumentTypes" runat="server" SelectMethod="Select"
                                TypeName="Broker.DataAccess.DistributionDocumentType"></asp:ObjectDataSource>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblUser" runat="server" Text="�����"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlUsers" runat="server" DataTextField="UserName" DataValueField="ID"
                                DataSourceID="UsersDataSource" Width="200px">
                            </asp:DropDownList>
                            <asp:ObjectDataSource ID="UsersDataSource" runat="server" SelectMethod="GetAllActiveUsers"
                                TypeName="Broker.DataAccess.User"></asp:ObjectDataSource>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblInsuranceCompany" runat="server" Text="������������ ��������"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlInsuranceCompany" runat="server" DataTextField="Name" DataValueField="ID"
                                Width="300px" DataSourceID="InsuranceCompaniesDataSource">
                            </asp:DropDownList>
                            <asp:ObjectDataSource ID="InsuranceCompaniesDataSource" runat="server" SelectMethod="GetByDealsAndPackets"
                                TypeName="Broker.DataAccess.InsuranceCompany"></asp:ObjectDataSource>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblStartNumber" runat="server" Text="������� ���"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="tbStartNumber" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblEndNumber" runat="server" Text="����� ���"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="tbEndNumber" runat="server"></asp:TextBox>
                            <%--<super:EntityCallOutValidator ID="DistributionValidator" PropertyName="PolicyNumber"
                                runat="server" />--%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="btnCreate" runat="server" OnClick="btnCreate_Click" Text="������" />
                        </td>
                        <td>
                            <asp:Label ID="lblError" runat="server" Text="����� � ��� ��������" Visible="False"
                                Font-Bold="True" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                </table>
                <asp:GridView ID="gvNewDistrirutions" runat="server" AllowPaging="True" AllowSorting="True"
                    AutoGenerateColumns="False" Caption="������������� ������" EmptyDataText="���� ������ ��� �� ������������ ����������� �� �����������!"
                    RowStyle-CssClass="row" CssClass="grid" GridLines="None" OnPageIndexChanged="gvNewDistrirutions_PageIndexChanged"
                    OnPageIndexChanging="gvNewDistrirutions_PageIndexChanging">
                    <RowStyle CssClass="row"></RowStyle>
                    <Columns>
                        <asp:TemplateField HeaderText="������������ ��������">
                            <ItemTemplate>
                                <asp:Label ID="tbInsuranceCompany" ReadOnly="true" Text='<%#Eval("InsuranceCompany.Name") %>'
                                    runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="���">
                            <ItemTemplate>
                                <asp:Label ID="tbNumber" ReadOnly="true" Text='<%#Eval("Number") %>' runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="��� �� ��������">
                            <ItemTemplate>
                                <asp:Label ID="tbDocumentType" ReadOnly="true" Text='<%#Eval("DistributionDocumentType.Description") %>'
                                    runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <PagerStyle HorizontalAlign="Center" />
                    <PagerSettings FirstPageText="<< ���� " PreviousPageText="< ��������� " NextPageText=" ������ >"
                        LastPageText=" �������� >>" Mode="NextPreviousFirstLast" />
                </asp:GridView>
            </div>
        </asp:View>
        <asp:View ID="viewSearch" runat="server">
            <div class="paddingKontroli">
                <cc1:SearchControl ID="SearchControl1" runat="server" SearchDataSourceID="odsSearch"
                    GridViewToSearch="gvDistributions" OnSearch="SearchControl1_Search">
                    <cc1:SearchItem FieldName="������������ ��������" PropertyName="InsuranceCompanyName"
                        Comparator="StringComparators" DataType="String" />
                    <cc1:SearchItem FieldName="��� �� ����������" PropertyName="Number" Comparator="StringComparators"
                        DataType="String" />
                    <cc1:SearchItem FieldName="�����������" PropertyName="IsUsed" Comparator="BooleanComparators"
                        DataType="Boolean" />
                    <cc1:SearchItem FieldName="����" PropertyName="Date" Comparator="NumericComparators"
                        DataType="DateTime" />
                    <cc1:SearchItem FieldName="��� �� ���.(���)" PropertyName="DocumentTypeCode" Comparator="StringComparators"
                        DataType="String" />
                    <cc1:SearchItem FieldName="��� �� ��������" PropertyName="DocumentTypeDescription"
                        Comparator="StringComparators" DataType="String" />
                </cc1:SearchControl>
                <cc1:GridViewDataSource ID="odsSearch" runat="server" TypeName="Broker.DataAccess.ViewRightRestrictionDistribution"
                    SelectCountMethod="SelectSearchCountCached" SelectMethod="SelectSearch">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="SearchControl1" PropertyName="SearchArguments" Name="sArgument" />
                    </SelectParameters>
                </cc1:GridViewDataSource>
            </div>
        </asp:View>
        <asp:View ID="viewReport" runat="server">
            <div class="paddingKontroli">
                <cc1:ReportControl ID="reportControl" runat="server" TypeName="Broker.DataAccess.ViewRightRestrictionDistribution"
                    GridViewID="gvDistributions" SearchControlID="SearchControl1">
                    <cc1:PrintItem HeaderText="������������ ��������" PropertyName="InsuranceCompanyName" />
                    <cc1:PrintItem HeaderText="��� �� ������" PropertyName="PolicyNumber" />
                    <cc1:PrintItem HeaderText="�����������" PropertyName="IsUsed" />
                    <cc1:PrintItem HeaderText="����" PropertyName="Date" />
                    <cc1:PrintItem HeaderText="��� �� ��������" PropertyName="DocumentTypeDescription" />
                </cc1:ReportControl>
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>
