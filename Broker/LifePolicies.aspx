<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="LifePolicies.aspx.cs" Inherits="Broker_LifePolicies" Culture="mk-MK"
    UICulture="mk-MK" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="kontroli">
        <div id="button01">
            <asp:Button ID="btnPreviewPolicy" runat="server" ToolTip="�������" OnClick="btnPreviewPolicy_Click"
                CausesValidation="false" UseSubmitBehavior="false" CssClass="prikaz" BorderWidth="0px"
                Enabled="False" />
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
        <div id="button05">
            <asp:Button ID="btnAttachments" runat="server" ToolTip="���������" OnClick="btnAttachments_Click"
                CausesValidation="false" UseSubmitBehavior="false" CssClass="dokumenti" BorderWidth="0px" />
        </div>
        <div id="button06">
            <asp:Button ID="btnDiscard" runat="server" ToolTip="��������" OnClick="btnDiscard_Click"
                Enabled="false" CausesValidation="false" UseSubmitBehavior="false" CssClass="storniraj"
                BorderWidth="0px" />
        </div>
    </div>
    <asp:MultiView ID="mvMain" runat="server">
        <asp:View ID="viewGrid" runat="server">
            <div id="tabeliFrame">
                <div id="header">
                    <div id="content">
                        <asp:Label ID="lblPageName" runat="server" Text="������ �� ���. ���."></asp:Label>
                    </div>
                </div>
                <div id="contentOuter">
                    <div id="contentInner">
                        <cc1:GXGridView ID="GXGridView1" runat="server" DataSourceID="odsGridView" DataKeyNames="ID"
                            Width="100%" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"
                            Caption="������  �� ���. ���." EmptyDataText="���� ������ ��� �� ������������ ����������� �� �����������!"
                            OnRowCommand="GXGridView1_RowCommand" RowStyle-CssClass="row" CssClass="grid"
                            GridLines="None">
                            <Columns>
                                <asp:BoundField HeaderText="��� �� ������" DataField="PolicyNumber" SortExpression="PolicyNumber" />
                                <%--<asp:BoundField HeaderText="�����" DataField="InsuranceTypeShortName" SortExpression="InsuranceTypeShortName" />--%>
                                <asp:BoundField HeaderText="����" DataFormatString="{0:d}" DataField="ApplicationDate"
                                    SortExpression="ApplicationDate" />
                                <%--<asp:BoundField HeaderText="��. �����������" DataField="ClientEmbg" SortExpression="ClientEMBG" />--%>
                                <asp:BoundField HeaderText="�����������" DataField="ClientName" SortExpression="ClientName" />
                                <%--<asp:BoundField HeaderText="��. ����������" DataField="OwnerEMBG" SortExpression="OwnerEMBG" />--%>
                                <asp:BoundField HeaderText="����������" DataField="OwnerName" SortExpression="OwnerName" />
                                <asp:BoundField HeaderText="��. ��������" DataField="InsuranceCompanyShortName"
                                    SortExpression="InsuranceCompanyShortName" />
                                <asp:CheckBoxField HeaderText="���- ���" DataField="Discard" SortExpression="Discard" />
                            </Columns>
                            <PagerStyle HorizontalAlign="Center" />
                            <SelectedRowStyle CssClass="rowSelected" />
                            <PagerSettings FirstPageText="<< ���� " PreviousPageText="< ��������� " NextPageText=" ������ >"
                                LastPageText=" �������� >>" Mode="NextPreviousFirstLast" />
                        </cc1:GXGridView>
                        <cc1:GridViewDataSource ID="odsGridView" runat="server" TypeName="Broker.DataAccess.LifePoliciesView"
                            OldValuesParameterFormatString="oldEntity" 
                            onselecting="odsGridView_Selecting">
                        </cc1:GridViewDataSource>
                        <div class="pager">
                            <div class="container">
                                <div style="float: left;">
                                    <cc1:PagerControl ID="myGridPager" runat="server" ControlToPage="GXGridView1" />
                                </div>
                                <div style="float: right;">
                                    <cc1:FilterControl ID="FilterControl1" runat="server" GridViewToFilter="GXGridView1"
                                        FilterDataSourceID="odsFilterGridView" OnFilter="FilterControl1_Filter">
                                        <cc1:FilterItem FieldName="��� �� ������" PropertyName="PolicyNumber" Comparator="StringComparators"
                                            DataType="String" />
                                        <cc1:FilterItem FieldName="��������" PropertyName="InsuranceSubTypesShortDescription"
                                            Comparator="StringComparators" DataType="String" />
                                        <cc1:FilterItem FieldName="�������� (�����)" PropertyName="InsuranceSubTypeCode"
                                            Comparator="StringComparators" DataType="String" />
                                        <cc1:FilterItem FieldName="������������ ��������" PropertyName="InsuranceCompanyShortName"
                                            Comparator="StringComparators" DataType="String" />
                                        <cc1:FilterItem FieldName="��. �����������" PropertyName="ClientEMBG" Comparator="StringComparators"
                                            DataType="String" />
                                        <cc1:FilterItem FieldName="�����������" PropertyName="ClientName" Comparator="StringComparators"
                                            DataType="String" />
                                        <cc1:FilterItem FieldName="��. ����������" PropertyName="OwnerEMBG" Comparator="StringComparators"
                                            DataType="String" />
                                        <cc1:FilterItem FieldName="����������" PropertyName="OwnerName" Comparator="StringComparators"
                                            DataType="String" />
                                        <cc1:FilterItem FieldName="�������" PropertyName="StartDate" Comparator="NumericComparators"
                                            DataType="DateTime" />
                                        <cc1:FilterItem FieldName="�����" PropertyName="EndDate" Comparator="NumericComparators"
                                            DataType="DateTime" />
                                        <cc1:FilterItem FieldName="���� �� ��������" PropertyName="ApplicationDate" Comparator="NumericComparators"
                                            DataType="DateTime" />
                                        <cc1:FilterItem FieldName="������" PropertyName="Discard" Comparator="BooleanComparators"
                                            DataType="Boolean" />
                                    </cc1:FilterControl>
                                </div>
                                <cc1:FilterDataSource ID="odsFilterGridView" runat="server" 
                                    TypeName="Broker.DataAccess.LifePoliciesView" 
                                    onselecting="odsFilterGridView_Selecting">
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
            <div style="padding-top: 10px;">
                <asp:Button ID="btnMainInformations" runat="server" Text="������� ����������" OnClick="btnMainInformations_Click"
                    CssClass="PacketButton_Active" CausesValidation="false" />
                <asp:Button ID="btnAdditionalInformations" runat="server" Text="������������ ����������"
                    OnClick="btnAdditionalInformations_Click" CssClass="PacketButton" CausesValidation="false" />
                <div class="PacketOut">
                    <asp:MultiView ID="mvPacket" runat="server" ActiveViewIndex="0">
                        <asp:View ID="viewMainInformations" runat="server">
                            <div class="border1px_Policy">
                                <asp:DetailsView ID="PoliciesDetailsView" runat="server" DataKeyNames="ID" DataSourceID="dvDataSourcePolicyDetails"
                                    AutoGenerateRows="False" GridLines="None">
                                    <Fields>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <table width="680px">
                                                    <tr>
                                                        <td width="172px">
                                                            <asp:Label ID="lblPolicyNumber" runat="server" Text="��� �� ������"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="tbPolicyNumber" runat="server" Font-Bold="true" ReadOnly="true"
                                                                Text='<%#Eval("PolicyNumber") %>'></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td width="172px">
                                                            <asp:Label ID="lblOfferNumber" runat="server" Text="��� �� ������"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="tbOfferNumber" runat="server" ReadOnly="true" Text='<%#Eval("OfferNumber") %>'></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr class="light-background">
                                                        <td>
                                                            <asp:Label ID="lblInsuranceCompanies" runat="server" Text="������������ ��������"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlInsuranceCompany" runat="server" DataSourceID="odsInsuranceCompany"
                                                                DataTextField="Name" DataValueField="ID" SelectedValue='<%# Eval("InsuranceCompanyID") %>'
                                                                Enabled="false" Width="200px">
                                                            </asp:DropDownList>
                                                            <asp:ObjectDataSource ID="odsInsuranceCompany" runat="server" SelectMethod="Select"
                                                                TypeName="Broker.DataAccess.InsuranceCompany"></asp:ObjectDataSource>
                                                        </td>
                                                    </tr>
                                                    <tr class="light-background">
                                                        <td>
                                                            <asp:Label ID="lblClientEmbg" runat="server" Text="��. �����������"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="tbClientEMBG" runat="server" ReadOnly="true" Text='<%#Eval("Client.EMBG") %>'></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblClientName" runat="server" Text="�����������"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="tbClientName" runat="server" ReadOnly="true" Text='<%#Eval("Client.Name") %>'
                                                                Width="300px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr class="light-background">
                                                        <td>
                                                            <asp:Label ID="lblOwnerEmbg" runat="server" Text="��. ����������"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="tbOwnerEMBG" runat="server" ReadOnly="true" Text='<%#Eval("Client1.EMBG") %>'></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblOwnerName" runat="server" Text="����������"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="tbOwnerName" runat="server" ReadOnly="true" Text='<%#Eval("Client1.Name") %>'
                                                                Width="300px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr class="light-background">
                                                        <td>
                                                            <asp:Label ID="lblStartDate" runat="server" Text="������� ����"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="tbStartDate" ReadOnly="true" runat="server" Text='<%# Eval("StartDate", "{0:d}") %>'></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblEndDate" runat="server" Text="����� ����"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="tbEndDate" ReadOnly="true" runat="server" Text='<%# Eval("EndDate","{0:d}") %>'></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr class="light-background">
                                                        <td>
                                                            <asp:Label ID="lblApplicationDate" runat="server" Text="���� �� ��������"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="tbApplicationDate" ReadOnly="true" runat="server" Text='<%# Eval("ApplicationDate", "{0:d}") %>'></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <table>
                                                    <tr>
                                                        <td style="width: 250px;">
                                                            ���������� �� �������� �� ������
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="tbPolicyDurationYears" runat="server" ReadOnly="true" Width="40px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <table>
                                                    <tr>
                                                        <td style="width: 250px;">
                                                            <b>������������ ��������</b>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            ������������ �������� �� ����������
                                                        </td>
                                                        <td>
                                                            �� ����
                                                            <asp:TextBox ID="tbInsuranceCoverageOneEuro" runat="server" Width="50px" Text='<%#Eval("InsuranceCoverageOneEuro") %>'
                                                                ReadOnly="true"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            �� ������
                                                            <asp:TextBox ID="tbInsuranceCoverageOne" ReadOnly="true" runat="server" Width="50px"
                                                                Text='<%#Eval("InsuranceCoverageOne") %>'></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            ������������ �������� �� �������
                                                        </td>
                                                        <td>
                                                            �� ����
                                                            <asp:TextBox ID="tbInsuranceCoverageTwoEuro" runat="server" Width="50px" Text='<%#Eval("InsuranceCoverageTwoEuro") %>'
                                                                ReadOnly="true"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            �� ������
                                                            <asp:TextBox ID="tbInsuranceCoverageTwo" ReadOnly="true" runat="server" Width="50px"
                                                                Text='<%#Eval("InsuranceCoverageTwo") %>'></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            ������:
                                                        </td>
                                                        <td>
                                                            �� ����
                                                            <asp:TextBox ID="tbTotalInsuranceCoverageSumEuro" runat="server" Width="50px" Text='<%#Eval("TotalInsuranceCoverageSumEuro") %>'
                                                                ReadOnly="true"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            �� ������
                                                            <asp:TextBox ID="tbTotalInsuranceCoverageSum" runat="server" Width="50px" Text='<%#Eval("TotalInsuranceCoverageSum") %>'
                                                                ReadOnly="true"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <table>
                                                    <tr>
                                                        <td style="width: 250px;">
                                                            <b>������</b>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            ������� ������ �� ����������
                                                        </td>
                                                        <td>
                                                            �� ����
                                                            <asp:TextBox ID="tbYearlyPremiumValueForLifeEuro" runat="server" Width="50px" Text='<%#Eval("YearlyPremiumValueForLifeEuro") %>'
                                                                ReadOnly="true"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            �� ������
                                                            <asp:TextBox ID="tbYearlyPremiumValueForLife" runat="server" Width="50px" Text='<%#Eval("YearlyPremiumValueForLife") %>'
                                                                AutoPostBack="True" ReadOnly="true"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            ������� ������ �� �������
                                                        </td>
                                                        <td>
                                                            �� ����
                                                            <asp:TextBox ID="tbYearlyPremiumValueForAccidentEuro" runat="server" Width="50px"
                                                                Text='<%#Eval("YearlyPremiumValueForAccidentEuro") %>' ReadOnly="true"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            �� ������
                                                            <asp:TextBox ID="tbYearlyPremiumValueForAccident" ReadOnly="true" runat="server"
                                                                Width="50px" Text='<%#Eval("YearlyPremiumValueForAccident") %>'></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            ������:
                                                        </td>
                                                        <td>
                                                            �� ����
                                                            <asp:TextBox ID="tbTotalPremumValueEuro" runat="server" Width="50px" Text='<%#Eval("TotalPremiumValueEuro") %>'
                                                                ReadOnly="true"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            �� ������
                                                            <asp:TextBox ID="tbTotalPremumValue" runat="server" Width="50px" Text='<%#Eval("TotalPremumValue") %>'
                                                                ReadOnly="true"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Fields>
                                </asp:DetailsView>
                                <asp:ObjectDataSource ID="dvDataSourcePolicyDetails" runat="server" TypeName="Broker.DataAccess.LifePolicy"
                                    ConflictDetection="CompareAllValues" DataObjectTypeName="Broker.DataAccess.LifePolicy"
                                    SelectMethod="Get" OldValuesParameterFormatString="oldEntity" OnSelecting="dvDataSourcePolicyDetails_Selecting">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="GXGridView1" Name="id" PropertyName="SelectedValue"
                                            Type="Int32" />
                                    </SelectParameters>
                                </asp:ObjectDataSource>
                            </div>
                        </asp:View>
                        <asp:View ID="viewAdditionalInformations" runat="server">
                            <asp:Panel runat="server" ID="pnlPremiumInformationsPerYears">
                                <div id="contentOuter">
                                    <div id="contentInner">
                                        <asp:GridView ID="gvLifePolicyPremiumValue" runat="server" GridLines="None" DataSourceID="odsLifePolicyPremiumValue"
                                            Caption="���������� �� ������ �� ������" EmptyDataText="���� ������!" AutoGenerateColumns="false"
                                            AllowPaging="true" AllowSorting="false" RowStyle-CssClass="row" CssClass="grid">
                                            <RowStyle CssClass="row" />
                                            <Columns>
                                                <asp:BoundField HeaderText="�.��." DataField="OrdinalNumberYear" />
                                                <asp:BoundField HeaderText="��" DataField="FromDate" DataFormatString="{0:d}" />
                                                <asp:BoundField HeaderText="��" DataField="ToDate" DataFormatString="{0:d}" />
                                                <asp:BoundField HeaderText="�� ����������" DataField="PremiumValueForLife" DataFormatString="{0:#,0.00}" />
                                                <asp:BoundField HeaderText="�� ���������� (����)" DataField="PremiumValueForLifeEuro"
                                                    DataFormatString="{0:#,0.00}" />
                                                <asp:BoundField HeaderText="�� �������" DataField="PremiumValueForAccident" DataFormatString="{0:#,0.00}" />
                                                <asp:BoundField HeaderText="�� ������� (����)" DataField="PremiumValueForAccidentEuro"
                                                    DataFormatString="{0:#,0.00}" />
                                            </Columns>
                                            <PagerStyle HorizontalAlign="Center" />
                                            <SelectedRowStyle CssClass="rowSelected" />
                                            <PagerSettings FirstPageText="<< ���� " PreviousPageText="< ��������� " NextPageText=" ������ >"
                                                LastPageText=" �������� >>" Mode="NextPreviousFirstLast" />
                                        </asp:GridView>
                                        <asp:ObjectDataSource runat="server" ID="odsLifePolicyPremiumValue" TypeName="Broker.DataAccess.LifePolicyPremiumValue"
                                            DataObjectTypeName="Broker.DataAccess.LifePolicyPremiumValue" SelectMethod="GetByLifePolicy"
                                            OnSelecting="odsLifePolicyPremiumValue_Selecting"></asp:ObjectDataSource>
                                    </div>
                                </div>
                            </asp:Panel>
                            <asp:Panel runat="server" ID="pnlBrokerageInformationsPerYears">
                                <div id="contentOuter">
                                    <div id="contentInner">
                                        <asp:GridView ID="gvLifePolicyBrokerage" runat="server" GridLines="None" DataSourceID="odsLifePolicyBrokerage"
                                            Caption=" ���������� �� ��������� �� ������" EmptyDataText="���� ������!" AutoGenerateColumns="false"
                                            AllowPaging="true" AllowSorting="false" RowStyle-CssClass="row" CssClass="grid">
                                            <RowStyle CssClass="row" />
                                            <Columns>
                                                <asp:BoundField HeaderText="�.��." DataField="OrdinalNumber" />
                                                <asp:BoundField HeaderText="��" DataField="FromDate" DataFormatString="{0:d}" />
                                                <asp:BoundField HeaderText="��" DataField="ToDate" DataFormatString="{0:d}" />
                                                <asp:BoundField HeaderText="����� �� ���������" DataField="Value" DataFormatString="{0:#,0.00}" />
                                                <asp:CheckBoxField HeaderText="�����������" DataField="IsFactured" />
                                            </Columns>
                                            <PagerStyle HorizontalAlign="Center" />
                                            <SelectedRowStyle CssClass="rowSelected" />
                                            <PagerSettings FirstPageText="<< ���� " PreviousPageText="< ��������� " NextPageText=" ������ >"
                                                LastPageText=" �������� >>" Mode="NextPreviousFirstLast" />
                                        </asp:GridView>
                                        <asp:ObjectDataSource runat="server" ID="odsLifePolicyBrokerage" TypeName="Broker.DataAccess.LifePolicyBrokerage"
                                            DataObjectTypeName="Broker.DataAccess.LifePolicyBrokerage" SelectMethod="GetByLifePolicy"
                                            OnSelecting="odsLifePolicyBrokerage_Selecting"></asp:ObjectDataSource>
                                    </div>
                                </div>
                            </asp:Panel>
                        </asp:View>
                    </asp:MultiView>
                </div>
            </div>
        </asp:View>
        <asp:View ID="viewSearch" runat="server">
            <div class="paddingKontroli">
                <cc1:SearchControl ID="SearchControl1" runat="server" SearchDataSourceID="odsSearch"
                    GridViewToSearch="GXGridView1" OnSearch="SearchControl1_Search">
                    <cc1:SearchItem FieldName="��� �� ������" PropertyName="PolicyNumber" Comparator="StringComparators"
                        DataType="String" />
                    <cc1:SearchItem FieldName="��������" PropertyName="InsuranceSubTypesShortDescription"
                        Comparator="StringComparators" DataType="String" />
                    <cc1:SearchItem FieldName="�������� (�����)" PropertyName="InsuranceSubTypeCode"
                        Comparator="StringComparators" DataType="String" />
                    <cc1:SearchItem FieldName="������������ ��������" PropertyName="InsuranceCompanyShortName"
                        Comparator="StringComparators" DataType="String" />
                    <cc1:SearchItem FieldName="��. �����������" PropertyName="ClientEMBG" Comparator="StringComparators"
                        DataType="String" />
                    <cc1:SearchItem FieldName="�����������" PropertyName="ClientName" Comparator="StringComparators"
                        DataType="String" />
                    <cc1:SearchItem FieldName="��. ����������" PropertyName="OwnerEMBG" Comparator="StringComparators"
                        DataType="String" />
                    <cc1:SearchItem FieldName="����������" PropertyName="OwnerName" Comparator="StringComparators"
                        DataType="String" />
                    <cc1:SearchItem FieldName="�������" PropertyName="StartDate" Comparator="NumericComparators"
                        DataType="DateTime" />
                    <cc1:SearchItem FieldName="�����" PropertyName="EndDate" Comparator="NumericComparators"
                        DataType="DateTime" />
                    <cc1:SearchItem FieldName="���� �� ��������" PropertyName="ApplicationDate" Comparator="NumericComparators"
                        DataType="DateTime" />
                    <cc1:SearchItem FieldName="������" PropertyName="Discard" Comparator="BooleanComparators"
                        DataType="Boolean" />
                </cc1:SearchControl>
                <cc1:GridViewDataSource ID="odsSearch" runat="server" TypeName="Broker.DataAccess.LifePoliciesView"
                    SelectCountMethod="SelectSearchCountCached" SelectMethod="SelectSearch" 
                    onselecting="odsSearch_Selecting">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="SearchControl1" PropertyName="SearchArguments" Name="sArgument" />
                    </SelectParameters>
                </cc1:GridViewDataSource>
            </div>
        </asp:View>
        <asp:View ID="viewReport" runat="server">
            <div class="paddingKontroli">
                <cc1:ReportControl ID="reportControl" runat="server" TypeName="Broker.DataAccess.LifePoliciesView"
                    GridViewID="GXGridView1" SearchControlID="SearchControl1">
                    <cc1:PrintItem HeaderText="��� �� ������" PropertyName="PolicyNumber" />
                    <cc1:PrintItem HeaderText="��������" PropertyName="InsuranceSubTypesShortDescription" />
                    <cc1:PrintItem HeaderText="�������� (�����)" PropertyName="InsuranceSubTypeCode" />
                    <cc1:PrintItem HeaderText="������������ ��������" PropertyName="InsuranceCompanyShortName" />
                    <cc1:PrintItem HeaderText="��. �����������" PropertyName="ClientEMBG" />
                    <cc1:PrintItem HeaderText="�����������" PropertyName="ClientName" />
                    <cc1:PrintItem HeaderText="��. ����������" PropertyName="OwnerEMBG" />
                    <cc1:PrintItem HeaderText="����������" PropertyName="OwnerName" />
                    <cc1:PrintItem HeaderText="�������" PropertyName="StartDate" />
                    <cc1:PrintItem HeaderText="�����" PropertyName="EndDate" />
                    <cc1:PrintItem HeaderText="���� �� ��������" PropertyName="ApplicationDate" />
                    <cc1:PrintItem HeaderText="������" PropertyName="Discard" />
                </cc1:ReportControl>
            </div>
        </asp:View>
        <asp:View ID="viewDiscardPolicy" runat="server">
            <div class="paddingKontroli">
                <table>
                    <tr>
                        <td>
                            ��� �� ������
                        </td>
                        <td>
                            <asp:TextBox ID="tbDiscardPolicyNumber" runat="server" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            ������������ ��������
                        </td>
                        <td>
                            <asp:TextBox ID="tbDiscardInsuranceCompany" runat="server" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            �������� �� �����������
                        </td>
                        <td>
                            <asp:TextBox ID="tbDiscardInsuranceSubType" runat="server" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:Button ID="btnDiscardPol" runat="server" Text="��������" OnClick="btnDiscardPol_Click" />
                        </td>
                    </tr>
                </table>
            </div>
        </asp:View>
        <asp:View ID="viewAttachments" runat="server">
            <div class="paddingKontroli">
                <asp:DetailsView ID="DetailsViewPolicyForAttachments" runat="server" DataSourceID="odsPolicyForAttachments"
                    DataKeyNames="ID" AutoGenerateRows="False" OnItemCommand="DetailsViewPolicyForAttachments_ItemCommand"
                    OnModeChanging="DetailsViewPolicyForAttachments_ModeChanging" GridLines="None"
                    DefaultMode="ReadOnly">
                    <Fields>
                        <asp:TemplateField HeaderText="��� �� ������">
                            <ItemTemplate>
                                <asp:TextBox ID="tbPolicyNumber" runat="server" Text='<%# Bind("PolicyNumber") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="�����������">
                            <ItemTemplate>
                                <asp:TextBox ID="tbClientEMBG" runat="server" Text='<%# Eval("Client.EMBG") %>' ReadOnly="true"></asp:TextBox>
                                <asp:TextBox ID="tbClientName" runat="server" Text='<%# Eval("Client.Name") %>' ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="����������">
                            <ItemTemplate>
                                <asp:TextBox ID="tbOwnerEMBG" runat="server" Text='<%# Eval("Client1.EMBG") %>' ReadOnly="true"></asp:TextBox>
                                <asp:TextBox ID="tbOwnerName" runat="server" Text='<%# Eval("Client1.Name") %>' ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Fields>
                </asp:DetailsView>
                <cc1:DetailsViewDataSource ID="odsPolicyForAttachments" runat="server" TypeName="Broker.DataAccess.LifePolicy"
                    ConflictDetection="CompareAllValues" DataObjectTypeName="Broker.DataAccess.LifePolicy"
                    SelectMethod="Get" OldValuesParameterFormatString="oldEntity" OnSelecting="odsPolicyForAttachments_Selecting">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="GXGridView1" Name="id" PropertyName="SelectedValue"
                            Type="Int32" />
                    </SelectParameters>
                </cc1:DetailsViewDataSource>
                <table>
                    <tr>
                        <td>
                            ��������<asp:FileUpload ID="FileUpload1" runat="server" />
                        </td>
                        <td>
                            <asp:Button ID="btnAddAttachment" runat="server" Text="������" OnClick="btnAddAttachment_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
                        </td>
                        <td>
                        </td>
                    </tr>
                </table>
                <div>
                    <asp:GridView ID="dataGridFiles" runat="server" AutoGenerateColumns="False" GridLines="None"
                        EmptyDataText="���� ������!" HeaderStyle-BackColor="#2D3931" HeaderStyle-ForeColor="Wheat"
                        OnSelectedIndexChanged="dataGridFiles_SelectedIndexChanged">
                        <SelectedRowStyle CssClass="rowSelected" />
                        <Columns>
                            <asp:TemplateField HeaderText="��������">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnFile" runat="server" OnClick="btnFile_Click" Text='<%#Bind("Name") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:CommandField ButtonType="Link" SelectText="�������" ShowSelectButton="True"
                                CancelText="������" ShowCancelButton="true" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>
