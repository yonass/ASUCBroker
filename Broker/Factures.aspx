<%@ Page Title="������� �������" Language="C#" MasterPageFile="~/MasterPage.master"
    AutoEventWireup="true" CodeFile="Factures.aspx.cs" Inherits="Broker_Factures" %>

<%@ Register Namespace="Superexpert.Controls" TagPrefix="super" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript">
        function ShowChildGrid(obj) {
            var div = document.getElementById(obj);
            var img = document.getElementById('img' + obj);
            var theFlag = div.style.display == "none";
            div.style.display = (theFlag) ? "inline" : "none";
            img.src = (theFlag) ? "../_assets/img/arrowright.jpeg" : "../_assets/img/arrowdown.jpeg";
        }
    </script>

    <div id="kontroli">
        <div id="button01">
            <asp:Button ID="btnNew" runat="server" ToolTip="��� �����" OnClick="btnNew_Click"
                CausesValidation="false" UseSubmitBehavior="false" CssClass="novZapis" BorderWidth="0px" />
        </div>
        <%--<div id="button02">
            <asp:Button ID="btnFacturePreview" runat="server" ToolTip="������" OnClick="btnFacturePreview_Click"
                CausesValidation="false" Enabled="false" UseSubmitBehavior="false" CssClass="izmeni"
                BorderWidth="0px" />
        </div>--%>
        <%--<div id="button03">
            <asp:Button ID="btnDelete" runat="server" OnClick="btnDelete_Click" CausesValidation="false"
                Enabled="false" UseSubmitBehavior="false" CssClass="izbrisi" BorderWidth="0px" />
        </div>--%>
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
            <asp:Button ID="btnFactureItems" runat="server" ToolTip="������" OnClick="btnFactureItems_Click"
                CssClass="prikaz" BorderWidth="0px" Enabled="false" />
        </div>
        <div id="button06">
            <asp:Button ID="btnAttachments" runat="server" ToolTip="���������" OnClick="btnAttachments_Click"
                CssClass="dokumenti" Enabled="false" BorderWidth="0px" />
        </div>
        <div id="button07">
            <asp:Button ID="btnPintFacture" runat="server" ToolTip="������ �������" OnClick="btnPintFacture_Click"
                CssClass="pecati" BorderWidth="0px" />
        </div>
        <div id="button08">
            <asp:Button ID="btnPrintAnex" runat="server" ToolTip="������ ����� �������" OnClick="btnPrintAnexDeal_Click"
                CssClass="dogovor" BorderWidth="0px" />
        </div>
        <div id="button09">
            <asp:Button ID="btnDiscardFacture" runat="server" ToolTip="�������� �������" OnClick="btnDiscardFacture_Click"
                CssClass="storniraj" BorderWidth="0px" Enabled="false" />
        </div>
        <div id="button10">
            <asp:Button ID="btnChangeStatus" runat="server" ToolTip="������ ������ �� �������"
                OnClick="btnChangeStatus_Click" CssClass="promeniStatus" BorderWidth="0px" Enabled="false" />
        </div>
        <div id="button11">
            <asp:Button ID="btnPaymentsPerFacture" runat="server" ToolTip="�������" OnClick="btnPaymentsPerFacture_Click"
                CssClass="plakanja" BorderWidth="0px" Enabled="false" />
        </div>
    </div>
    <asp:MultiView ID="mvMain" runat="server">
        <asp:View ID="viewGrid" runat="server">
            <div id="tabeliFrame">
                <div id="header">
                    <div id="content">
                        �������
                    </div>
                </div>
                <div id="contentOuter">
                    <div id="contentInner">
                        <cc1:GXGridView ID="GXGridView1" runat="server" DataSourceID="odsGridView" DataKeyNames="ID"
                            Width="100%" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"
                            Caption="�������" EmptyDataText="���� ������ ��� �� ������������ ����������� �� �����������!"
                            OnRowCommand="GXGridView1_RowCommand" RowStyle-CssClass="row" CssClass="grid"
                            GridLines="None">
                            <Columns>
                                <asp:BoundField HeaderText="��� �� �������" DataField="FactureNumber" SortExpression="FactureNumber" />
                                <asp:BoundField HeaderText="��� �� �������" DataField="DocumentSubTypeDescription"
                                    SortExpression="DocumentSubTypeDescription" />
                                <asp:BoundField HeaderText="������" DataField="StatusDescription" SortExpression="StatusDescription" />
                                <asp:BoundField HeaderText="���� �� ��������" DataField="DateOfCreation" SortExpression="DateOfCreation"
                                    DataFormatString="{0:d}" />
                                <asp:BoundField HeaderText="�����" DataField="TotalCost" SortExpression="TotalCost"
                                    ItemStyle-CssClass="currencyClass" DataFormatString="{0:#,0.00}" />
                                <asp:BoundField HeaderText="������.����" DataField="BrokerageValue" SortExpression="BrokerageValue"
                                    DataFormatString="{0:#,0.00}" ItemStyle-CssClass="currencyClass" />
                                <asp:BoundField HeaderText="�������" DataField="ClientName" SortExpression="ClientName" />
                                <asp:CheckBoxField HeaderText="���- ���" DataField="Discard" SortExpression="Discard" />
                            </Columns>
                            <PagerStyle HorizontalAlign="Center" />
                            <SelectedRowStyle CssClass="rowSelected" />
                            <PagerSettings FirstPageText="<< ���� " PreviousPageText="< ��������� " NextPageText=" ������ >"
                                LastPageText=" �������� >>" Mode="NextPreviousFirstLast" />
                        </cc1:GXGridView>
                        <cc1:GridViewDataSource ID="odsGridView" runat="server" TypeName="Broker.DataAccess.ViewFacture"
                            OldValuesParameterFormatString="oldEntity">
                        </cc1:GridViewDataSource>
                        <div class="pager">
                            <div class="container">
                                <div style="float: left;">
                                    <cc1:PagerControl ID="myGridPager" runat="server" ControlToPage="GXGridView1" />
                                </div>
                                <div style="float: right;">
                                    <cc1:FilterControl ID="FilterControl1" runat="server" GridViewToFilter="GXGridView1"
                                        FilterDataSourceID="odsFilterGridView" OnFilter="FilterControl1_Filter">
                                        <cc1:FilterItem FieldName="��� �� �������" PropertyName="FactureNumber" Comparator="StringComparators"
                                            DataType="String" />
                                        <cc1:FilterItem FieldName="��� �� �������" PropertyName="DocumentSubTypeDescription"
                                            Comparator="StringComparators" DataType="String" />
                                        <cc1:FilterItem FieldName="������" PropertyName="StatusDescription" Comparator="StringComparators"
                                            DataType="String" />
                                        <cc1:FilterItem FieldName="���� �� ��������" PropertyName="DateOfCreation" Comparator="NumericComparators"
                                            DataType="DateTime" />
                                        <cc1:FilterItem FieldName="�����" PropertyName="TotalCost" Comparator="NumericComparators"
                                            DataType="Decimal" />
                                        <cc1:FilterItem FieldName="�������" PropertyName="ClientName" Comparator="StringComparators"
                                            DataType="String" />
                                        <cc1:FilterItem FieldName="�� �����" PropertyName="FromDate" Comparator="NumericComparators"
                                            DataType="DateTime" />
                                        <cc1:FilterItem FieldName="�� �����" PropertyName="ToDate" Comparator="NumericComparators"
                                            DataType="DateTime" />
                                        <cc1:FilterItem FieldName="������" PropertyName="Discard" Comparator="BooleanComparators"
                                            DataType="Boolean" />
                                    </cc1:FilterControl>
                                </div>
                                <cc1:FilterDataSource ID="odsFilterGridView" runat="server" TypeName="Broker.DataAccess.ViewFacture">
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
        <asp:View ID="viewNew" runat="server">
            <div class="paddingKontroli">
                <b>������� ������� �� ��������� ��� ������������ ��������</b>
                <table>
                    <tr>
                        <td style="width: 170px;">�� �����
                        </td>
                        <td>
                            <asp:TextBox ID="tbStartDate" runat="server">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvStartDate" runat="server" Display="Dynamic" ErrorMessage="*"
                                ControlToValidate="tbStartDate"></asp:RequiredFieldValidator>
                            <asp:CompareValidator ID="cvStartDate" runat="server" Display="Dynamic" ControlToValidate="tbStartDate"
                                ErrorMessage="*" Operator="DataTypeCheck" Type="Date"></asp:CompareValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>�� �����
                        </td>
                        <td>
                            <asp:TextBox ID="tbEndDate" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvEndDate" runat="server" Display="Dynamic" ErrorMessage="*"
                                ControlToValidate="tbEndDate"></asp:RequiredFieldValidator>
                            <asp:CompareValidator ID="cvEndDate" runat="server" Display="Dynamic" ControlToValidate="tbEndDate"
                                ErrorMessage="*" Operator="DataTypeCheck" Type="Date"></asp:CompareValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>������������ ��������
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlInsuranceCompanies" runat="server" DataValueField="ID" DataSourceID="odsInsuranceCompanies"
                                DataTextField="Name">
                            </asp:DropDownList>
                            <asp:ObjectDataSource ID="odsInsuranceCompanies" runat="server" SelectMethod="GetActiveInsuranceCompanies"
                                TypeName="Broker.DataAccess.InsuranceCompany"></asp:ObjectDataSource>
                        </td>
                    </tr>
                    <tr>
                        <td>���� �� ���������
                        </td>
                        <td>
                            <asp:TextBox ID="tbDateOfPayment" runat="server"></asp:TextBox>
                            <%--<asp:RequiredFieldValidator ID="rfvDateOfPayment" runat="server" Display="Dynamic"
                                ErrorMessage="*" ControlToValidate="tbDateOfPayment"></asp:RequiredFieldValidator>--%>
                            <asp:CompareValidator ID="cvDateOfPayment" runat="server" Display="Dynamic" ControlToValidate="tbDateOfPayment"
                                ErrorMessage="*" Operator="DataTypeCheck" Type="Date"></asp:CompareValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>��� �� �����������
                        </td>
                        <td>
                            <asp:RadioButtonList ID="rblInsuranceLifeType" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Selected="True" Text="���������" Value="NoLife"></asp:ListItem>
                                <asp:ListItem Text="�������" Value="Life"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>
                            <asp:Button ID="btnCheck" runat="server" OnClick="btnCheck_Click" Text="�������" />
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td style="width: 170px;"></td>
                        <td>
                            <asp:GridView ID="gvOldPolicies" runat="server" AllowPaging="True" AllowSorting="True"
                                AutoGenerateColumns="False" Caption="������ �� ��������� ������" EmptyDataText="���� ������!"
                                RowStyle-CssClass="rowFacture" CssClass="gridFacture" GridLines="None" OnPageIndexChanging="gvOldPolicies_PageIndexChanging"
                                DataKeyNames="ID" PageSize="10">
                                <RowStyle CssClass="rowFacture"></RowStyle>
                                <Columns>
                                    <asp:BoundField HeaderText="" DataField="ID" ItemStyle-ForeColor="Transparent" ItemStyle-Width="0px" />
                                    <asp:BoundField HeaderText="��� �� ������" DataField="PolicyNumber" SortExpression="PolicyNumber" />
                                    <asp:BoundField HeaderText="���������� ������" DataField="PremiumValue" SortExpression="PremiumValue"
                                        DataFormatString="{0:#,0.00}" ItemStyle-CssClass="currencyClass" />
                                    <asp:TemplateField HeaderText="�� �����������">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="cbIsForFacturing" runat="server" Checked='<%#Bind("IsForFacturing") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <PagerStyle HorizontalAlign="Center" />
                                <PagerSettings FirstPageText="<< ���� " PreviousPageText="< ��������� " NextPageText=" ������ >"
                                    LastPageText=" �������� >>" Mode="NextPreviousFirstLast" />
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>
                            <asp:GridView ID="gvNewPolicies" runat="server" AllowPaging="True" AllowSorting="True"
                                AutoGenerateColumns="False" Caption="������ �� ������������� ������" EmptyDataText="���� ������!"
                                RowStyle-CssClass="rowFacture" CssClass="gridFacture" GridLines="None" OnPageIndexChanging="gvNewPolicies_PageIndexChanging"
                                DataKeyNames="ID" PageSize="10">
                                <RowStyle CssClass="rowFacture"></RowStyle>
                                <Columns>
                                    <asp:BoundField HeaderText="" DataField="ID" ItemStyle-ForeColor="Transparent" ItemStyle-Width="0px" />
                                    <asp:BoundField HeaderText="��� �� ������" DataField="PolicyNumber" SortExpression="PolicyNumber" />
                                    <asp:BoundField HeaderText="���������� ������" DataField="PremiumValue" SortExpression="PremiumValue"
                                        DataFormatString="{0:#,0.00}" ItemStyle-CssClass="currencyClass" />
                                    <asp:TemplateField HeaderText="�� �����������">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="cbIsForFacturing" runat="server" Checked='<%#Bind("IsForFacturing") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <PagerStyle HorizontalAlign="Center" />
                                <PagerSettings FirstPageText="<< ���� " PreviousPageText="< ��������� " NextPageText=" ������ >"
                                    LastPageText=" �������� >>" Mode="NextPreviousFirstLast" />
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td style="width: 170px;">������
                        </td>
                        <td>
                            <asp:TextBox ID="tbCurrentPremiumValue" runat="server" ReadOnly="true" CssClass="currencyClass"></asp:TextBox>
                        </td>
                        <td>���������
                        </td>
                        <td>
                            <asp:TextBox ID="tbCurrentBrokerageValue" runat="server" ReadOnly="true" CssClass="currencyClass"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Button ID="btnCalculateCurrentState" runat="server" Text="��������" OnClick="btnCalculateCurrentState_Click" />
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td style="width: 170px;"></td>
                        <td>
                            <asp:Button ID="btnCreate" runat="server" OnClick="btnCreate_Click" Text="������� �������" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 170px;"></td>
                        <td>
                            <asp:Label ID="lblError" runat="server" Visible="false"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
        </asp:View>
        <asp:View ID="viewSearch" runat="server">
            <div class="paddingKontroli">
                <cc1:SearchControl ID="SearchControl1" runat="server" SearchDataSourceID="odsSearch"
                    GridViewToSearch="GXGridView1" OnSearch="SearchControl1_Search">
                    <cc1:SearchItem FieldName="��� �� �������" PropertyName="FactureNumber" Comparator="StringComparators"
                        DataType="String" />
                    <cc1:SearchItem FieldName="��� �� �������" PropertyName="DocumentSubTypeDescription"
                        Comparator="StringComparators" DataType="String" />
                    <cc1:SearchItem FieldName="������" PropertyName="StatusDescription" Comparator="StringComparators"
                        DataType="String" />
                    <cc1:SearchItem FieldName="���� �� ��������" PropertyName="DateOfCreation" Comparator="NumericComparators"
                        DataType="DateTime" />
                    <cc1:SearchItem FieldName="�����" PropertyName="TotalCost" Comparator="NumericComparators"
                        DataType="Decimal" />
                    <cc1:SearchItem FieldName="�������" PropertyName="ClientName" Comparator="StringComparators"
                        DataType="String" />
                    <cc1:SearchItem FieldName="�� �����" PropertyName="FromDate" Comparator="NumericComparators"
                        DataType="DateTime" />
                    <cc1:SearchItem FieldName="�� �����" PropertyName="ToDate" Comparator="NumericComparators"
                        DataType="DateTime" />
                    <cc1:SearchItem FieldName="������" PropertyName="Discard" Comparator="BooleanComparators"
                        DataType="Boolean" />
                </cc1:SearchControl>
                <cc1:GridViewDataSource ID="odsSearch" runat="server" TypeName="Broker.DataAccess.ViewFacture"
                    SelectCountMethod="SelectSearchCountCached" SelectMethod="SelectSearch">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="SearchControl1" PropertyName="SearchArguments" Name="sArgument" />
                    </SelectParameters>
                </cc1:GridViewDataSource>
            </div>
        </asp:View>
        <asp:View ID="viewReport" runat="server">
            <div class="paddingKontroli">
                <cc1:ReportControl ID="reportControl" runat="server" TypeName="Broker.DataAccess.ViewFacture"
                    GridViewID="GXGridView1" SearchControlID="SearchControl1">
                    <cc1:PrintItem HeaderText="��� �� �������" PropertyName="FactureNumber" />
                    <cc1:PrintItem HeaderText="��� �� �������" PropertyName="DocumentSubTypeDescription" />
                    <cc1:PrintItem HeaderText="������" PropertyName="StatusDescription" />
                    <cc1:PrintItem HeaderText="���� �� ��������" PropertyName="DateOfCreation" />
                    <cc1:PrintItem HeaderText="�����" PropertyName="TotalCost" />
                    <cc1:PrintItem HeaderText="�������" PropertyName="ClientName" />
                    <cc1:PrintItem HeaderText="�� �����" PropertyName="FromDate" />
                    <cc1:PrintItem HeaderText="�� �����" PropertyName="ToDate" />
                    <cc1:PrintItem HeaderText="������" PropertyName="Discard" />
                </cc1:ReportControl>
            </div>
        </asp:View>
        <asp:View ID="viewChangeStatus" runat="server">
            <div class="paddingKontroli">
                <asp:DetailsView ID="dvChangeStatus" runat="server" DataSourceID="odsFactureChangeStatus"
                    DataKeyNames="ID" AutoGenerateRows="false" GridLines="None" DefaultMode="Edit"
                    OnItemCommand="dvChangeStatus_ItemCommand" OnItemUpdating="dvChangeStatus_ItemUpdating">
                    <Fields>
                        <asp:TemplateField HeaderText="��� �� �������">
                            <EditItemTemplate>
                                <asp:TextBox ID="tbFactureNumber" runat="server" Text='<%# Bind("FactureNumber") %>'
                                    ReadOnly="true" Font-Bold="true"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="���������">
                            <EditItemTemplate>
                                <asp:TextBox ID="tbClientEMBG" runat="server" Text='<%# Eval("Client.EMBG") %>' ReadOnly="true"></asp:TextBox>
                                <asp:TextBox ID="tbClientName" Width="250px" runat="server" Text='<%# Eval("Client.Name") %>'
                                    ReadOnly="true"></asp:TextBox>
                                <asp:TextBox ID="tbClientID" runat="server" Text='<%#Bind("ClientID") %>' Visible="false"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="������ �����">
                            <EditItemTemplate>
                                <asp:TextBox ID="tbTotalCost" CssClass="currencyClass" runat="server" Text='<%# Bind("TotalCost","{0:#,0.00}") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="���������">
                            <EditItemTemplate>
                                <asp:TextBox ID="tbBrokerageValue" CssClass="currencyClass" runat="server" Text='<%# Bind("BrokerageValue","{0:#,0.00}") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="���� �� �������">
                            <EditItemTemplate>
                                <asp:TextBox ID="tbOfferDate" runat="server" Text='<%# Bind("DateOfCreation","{0:d}") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="���������� ��">
                            <EditItemTemplate>
                                <asp:TextBox ID="tbUserName" runat="server" Text='<%# Eval("User.UserName") %>' ReadOnly="true"></asp:TextBox>
                                <asp:TextBox ID="tbUserFullName" Width="250px" runat="server" Text='<%# Eval("User.Name") %>'
                                    ReadOnly="true"></asp:TextBox>
                                <asp:TextBox ID="tbUserID" runat="server" Text='<%#Bind("UserID") %>' Visible="false"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="��� �� ��������">
                            <EditItemTemplate>
                                <asp:TextBox ID="tbDocumentSubTypeDescription" runat="server" Text='<%# Eval("DocumentSubType.Description") %>'
                                    Width="300px" ReadOnly="true"></asp:TextBox>
                                <asp:TextBox ID="tbDocumentSubTypeDescriptionID" runat="server" Text='<%#Bind("DocumentSubTypeID") %>'
                                    Visible="false"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="�� �����">
                            <EditItemTemplate>
                                <asp:TextBox ID="tbFromDate" runat="server" Text='<%# Bind("FromDate", "{0:d}") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="�� �����">
                            <EditItemTemplate>
                                <asp:TextBox ID="tbToDate" runat="server" Text='<%# Bind("ToDate", "{0:d}") %>' ReadOnly="true"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="������� ���">
                            <EditItemTemplate>
                                <asp:TextBox ID="tbDateOfPayment" runat="server" Text='<%# Bind("DateOfPayment", "{0:d}") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="������������ ��������">
                            <EditItemTemplate>
                                <asp:TextBox ID="tbInsuranceCompanyName" runat="server" Text='<%# Eval("InsuranceCompany.Name") %>'
                                    ReadOnly="true"></asp:TextBox>
                                <asp:TextBox ID="tbInsuranceCompanyID" runat="server" Text='<%#Bind("InsuranceCompanyID") %>'
                                    Visible="false"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="������">
                            <EditItemTemplate>
                                <asp:CheckBox ID="cbDiscard" runat="server" Checked='<%#Bind("Discard") %>' Enabled="false" />
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="������">
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddlStatuses" runat="server" DataSourceID="odsStatuses" DataTextField="Description"
                                    DataValueField="ID" SelectedValue='<%#Bind("StatusID") %>'>
                                </asp:DropDownList>
                                <asp:ObjectDataSource ID="odsStatuses" runat="server" SelectMethod="GetActiveStatusesForDocumentSubType"
                                    TypeName="Broker.DataAccess.Statuse">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="tbDocumentSubTypeDescriptionID" Name="documentSubTypeID"
                                            PropertyName="Text" Type="Int32" />
                                    </SelectParameters>
                                </asp:ObjectDataSource>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <EditItemTemplate>
                                <asp:Button Text="������" runat="server" OnClick="btnUpdateFactureStatus_Click" />
                                <asp:Button CommandName="Cancel" Text="������" CausesValidation="false" runat="server" />
                            </EditItemTemplate>
                        </asp:TemplateField>
                    </Fields>
                </asp:DetailsView>
                <asp:ObjectDataSource ID="odsFactureChangeStatus" runat="server" SelectMethod="Get"
                    TypeName="Broker.DataAccess.Facture" UpdateMethod="Update" ConflictDetection="CompareAllValues"
                    DataObjectTypeName="Broker.DataAccess.Facture" OnUpdating="odsFactureChangeStatus_Updating">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="GXGridView1" Name="id" PropertyName="SelectedValue"
                            Type="Int32" />
                    </SelectParameters>
                    <UpdateParameters>
                        <asp:Parameter Name="oldEntity" Type="Object" />
                        <asp:Parameter Name="newEntity" Type="Object" />
                        <asp:Parameter Name="FromDate" Type="DateTime" />
                        <asp:Parameter Name="ToDate" Type="DateTime" />
                        <asp:Parameter Name="DateOfCreation" Type="DateTime" />
                        <asp:Parameter Name="DateOfPayment" Type="DateTime" />
                        <asp:Parameter Name="BrokerageValue" Type="Decimal" />
                        <asp:Parameter Name="TotalCost" Type="Decimal" />
                        <asp:Parameter Name="ClientID" Type="Int32" />
                        <asp:Parameter Name="InsuranceCompanyID" Type="Int32" />
                        <asp:Parameter Name="DocumentSubTypeID" Type="Int32" />
                        <asp:Parameter Name="StatusID" Type="Int32" />
                        <asp:Parameter Name="UserID" Type="Int32" />
                    </UpdateParameters>
                </asp:ObjectDataSource>
            </div>
        </asp:View>
        <asp:View ID="viewDiscardFacture" runat="server">
            <div class="paddingKontroli">
                <asp:DetailsView ID="dvFacture" runat="server" DataSourceID="odsFacturePreview" AutoGenerateRows="false"
                    GridLines="None">
                    <Fields>
                        <asp:TemplateField HeaderText="��� �� �������">
                            <ItemTemplate>
                                <asp:TextBox ID="tbFactureNumber" runat="server" Text='<%# Eval("FactureNumber") %>'
                                    ReadOnly="true" Font-Bold="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="���������">
                            <ItemTemplate>
                                <asp:TextBox ID="tbClientEMBG" runat="server" Text='<%# Eval("Client.EMBG") %>' ReadOnly="true"></asp:TextBox>
                                <asp:TextBox ID="tbClientName" Width="250px" runat="server" Text='<%# Eval("Client.Name") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="������ �����">
                            <ItemTemplate>
                                <asp:TextBox ID="tbTotalCost" runat="server" CssClass="currencyClass" Text='<%# Eval("TotalCost","{0:#,0.00}") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="���������">
                            <ItemTemplate>
                                <asp:TextBox ID="tbBrokerageValue" runat="server" CssClass="currencyClass" Text='<%# Eval("BrokerageValue","{0:#,0.00}") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="���� �� �������">
                            <ItemTemplate>
                                <asp:TextBox ID="tbOfferDate" runat="server" Text='<%# Eval("DateOfCreation","{0:d}") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="������� ���">
                            <ItemTemplate>
                                <asp:TextBox ID="tbDateOfPayment" runat="server" Text='<%# Bind("DateOfPayment", "{0:d}") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="���������� ��">
                            <ItemTemplate>
                                <asp:TextBox ID="tbUserName" runat="server" Text='<%# Eval("User.UserName") %>' ReadOnly="true"></asp:TextBox>
                                <asp:TextBox ID="tbUserFullName" Width="250px" runat="server" Text='<%# Eval("User.Name") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="��� �� ��������">
                            <ItemTemplate>
                                <asp:TextBox ID="tbDocumentSubType" runat="server" Text='<%# Eval("DocumentSubType.Description") %>'
                                    Width="300px" ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="������">
                            <ItemTemplate>
                                <asp:TextBox ID="tbStatuse" runat="server" Text='<%# Eval("Statuse.Description") %>'
                                    Width="300px" ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Fields>
                </asp:DetailsView>
                <asp:Button ID="btnDiscardFac" runat="server" Text="��������" OnClick="btnDiscardFac_Click" />
            </div>
        </asp:View>
        <asp:View ID="viewFactureItems" runat="server">
            <div class="paddingKontroli">
                <asp:DetailsView ID="DetailsViewFacturePreview" runat="server" DataSourceID="odsFacturePreview"
                    AutoGenerateRows="false" GridLines="None">
                    <Fields>
                        <asp:TemplateField HeaderText="��� �� �������">
                            <ItemTemplate>
                                <asp:TextBox ID="tbFactureNumber" runat="server" Text='<%# Eval("FactureNumber") %>'
                                    ReadOnly="true" Font-Bold="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="���������">
                            <ItemTemplate>
                                <asp:TextBox ID="tbClientEMBG" runat="server" Text='<%# Eval("Client.EMBG") %>' ReadOnly="true"></asp:TextBox>
                                <asp:TextBox ID="tbClientName" Width="250px" runat="server" Text='<%# Eval("Client.Name") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="������ �����">
                            <ItemTemplate>
                                <asp:TextBox ID="tbTotalCost" runat="server" Text='<%# Eval("TotalCost","{0:#,0.00}") %>'
                                    ReadOnly="true" CssClass="currencyClass"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="���������">
                            <ItemTemplate>
                                <asp:TextBox ID="tbBrokerageValue" runat="server" Text='<%# Eval("BrokerageValue","{0:#,0.00}") %>'
                                    ReadOnly="true" CssClass="currencyClass"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="���� �� �������">
                            <ItemTemplate>
                                <asp:TextBox ID="tbOfferDate" runat="server" Text='<%# Eval("DateOfCreation","{0:d}") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="������� ���">
                            <ItemTemplate>
                                <asp:TextBox ID="tbDateOfPayment" runat="server" Text='<%# Bind("DateOfPayment", "{0:d}") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="���������� ��">
                            <ItemTemplate>
                                <asp:TextBox ID="tbUserName" runat="server" Text='<%# Eval("User.UserName") %>' ReadOnly="true"></asp:TextBox>
                                <asp:TextBox ID="tbUserFullName" Width="250px" runat="server" Text='<%# Eval("User.Name") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="��� �� ��������">
                            <ItemTemplate>
                                <asp:TextBox ID="tbDocumentSubType" runat="server" Text='<%# Eval("DocumentSubType.Description") %>'
                                    Width="300px" ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="������">
                            <ItemTemplate>
                                <asp:TextBox ID="tbStatuse" runat="server" Text='<%# Eval("Statuse.Description") %>'
                                    Width="300px" ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Fields>
                </asp:DetailsView>
                <asp:ObjectDataSource ID="odsFacturePreview" runat="server" SelectMethod="Get" TypeName="Broker.DataAccess.Facture">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="GXGridView1" Name="id" PropertyName="SelectedValue"
                            Type="Int32" />
                    </SelectParameters>
                </asp:ObjectDataSource>
                <asp:GridView ID="GridViewFactureItems" runat="server" OnSelectedIndexChanged="GridViewFactureItems_SelectedIndexChanged"
                    OnRowCommand="GridViewFactureItems_RowCommand" DataSourceID="odsFactureItems"
                    Width="100%" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"
                    Caption="������ �� �������" EmptyDataText="���� ������ ��� �� ������������ ����������� �� �����������!"
                    RowStyle-CssClass="row" CssClass="grid" GridLines="None">
                    <RowStyle CssClass="row"></RowStyle>
                    <Columns>
                        <asp:TemplateField HeaderText="����� ���">
                            <ItemTemplate>
                                <asp:Label ID="tbNumber" runat="server" ReadOnly="true" Text='<%#Eval("Number") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="���� �� ������">
                            <ItemTemplate>
                                <asp:Label ID="tbDescription" runat="server" ReadOnly="true" Text='<%#Eval("Description") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="��� �� ������">
                            <ItemTemplate>
                                <asp:Label ID="tbCount" runat="server" ReadOnly="true" Text='<%#Eval("Count") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="������" ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <asp:Label ID="tbCostPremiumValue" runat="server" Text='<%#Eval("PremiumValue", "{0:#,0.00}") %>'
                                    ReadOnly="true"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="���������" ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <asp:Label ID="tbCostBrokerageValue" runat="server" Text='<%#Eval("BrokerageValue", "{0:#,0.00}") %>'
                                    ReadOnly="true"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <PagerStyle HorizontalAlign="Center" />
                    <%--<SelectedRowStyle CssClass="rowSelected" />--%>
                    <PagerSettings FirstPageText="<< ���� " PreviousPageText="< ��������� " NextPageText=" ������ >"
                        LastPageText=" �������� >>" Mode="NextPreviousFirstLast" />
                </asp:GridView>
                <asp:ObjectDataSource ID="odsFactureItems" runat="server" SelectMethod="GetByFacture"
                    TypeName="Broker.DataAccess.FactureItem">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="GXGridView1" Name="factureID" PropertyName="SelectedValue"
                            Type="Int32" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </div>
        </asp:View>
        <asp:View ID="viewAttachments" runat="server">
            <div class="paddingKontroli">
                <asp:DetailsView ID="dvFacturePreviewForAttachments" runat="server" DataSourceID="odsFacturePreviewForAttachments"
                    DataKeyNames="ID" AutoGenerateRows="False" OnItemCommand="dvFacturePreviewForAttachments_ItemCommand"
                    OnModeChanging="dvFacturePreviewForAttachments_ModeChanging" GridLines="None"
                    DefaultMode="ReadOnly">
                    <Fields>
                        <asp:TemplateField HeaderText="��� �� �������">
                            <ItemTemplate>
                                <asp:TextBox ID="tbFactureNumber" runat="server" Text='<%# Eval("FactureNumber") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="���������">
                            <ItemTemplate>
                                <asp:TextBox ID="tbClientEMBG" runat="server" Text='<%# Eval("Client.EMBG") %>' ReadOnly="true"></asp:TextBox>
                                <asp:TextBox ID="tbClientName" Width="250px" runat="server" Text='<%# Eval("Client.Name") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="������ �����" ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <asp:TextBox ID="tbTotalCost" runat="server" Text='<%# Eval("TotalCost") %>' ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="���������" ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <asp:TextBox ID="tbBrokerageValue" runat="server" Text='<%# Eval("BrokerageValue") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="���� �� �������">
                            <ItemTemplate>
                                <asp:TextBox ID="tbOfferDate" runat="server" Text='<%# Eval("DateOfCreation","{0:d}") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="���������� ��">
                            <ItemTemplate>
                                <asp:TextBox ID="tbUserName" runat="server" Text='<%# Eval("User.UserName") %>' ReadOnly="true"></asp:TextBox>
                                <asp:TextBox ID="tbUserFullName" Width="250px" runat="server" Text='<%# Eval("User.Name") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Fields>
                </asp:DetailsView>
                <cc1:DetailsViewDataSource ID="odsFacturePreviewForAttachments" runat="server" TypeName="Broker.DataAccess.Facture"
                    ConflictDetection="CompareAllValues" DataObjectTypeName="Broker.DataAccess.Facture"
                    SelectMethod="Get" OldValuesParameterFormatString="oldEntity" OnSelecting="odsFacturePreviewForAttachments_Selecting">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="GXGridView1" Name="id" PropertyName="SelectedValue"
                            Type="Int32" />
                    </SelectParameters>
                </cc1:DetailsViewDataSource>
                <table>
                    <tr>
                        <td>��������
                            <asp:FileUpload ID="FileUpload1" runat="server" />
                        </td>
                        <td>
                            <asp:Button ID="btnAddAttachment" runat="server" Text="������" OnClick="btnAddAttachment_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
                        </td>
                        <td>&nbsp;
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
        <asp:View ID="viewPayments" runat="server">
            <div class="paddingKontroli">
                <asp:DetailsView ID="dvFacturePreviewForPayments" runat="server" DataSourceID="odsFacturePreview"
                    AutoGenerateRows="false" GridLines="None">
                    <Fields>
                        <asp:TemplateField HeaderText="��� �� �������">
                            <ItemTemplate>
                                <asp:TextBox ID="tbFactureNumber" runat="server" Text='<%# Eval("FactureNumber") %>'
                                    ReadOnly="true" Font-Bold="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="���������">
                            <ItemTemplate>
                                <asp:TextBox ID="tbClientEMBG" runat="server" Text='<%# Eval("Client.EMBG") %>' ReadOnly="true"></asp:TextBox>
                                <asp:TextBox ID="tbClientName" Width="250px" runat="server" Text='<%# Eval("Client.Name") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="������ �����">
                            <ItemTemplate>
                                <asp:TextBox ID="tbTotalCost" runat="server" Text='<%# Eval("TotalCost","{0:#,0.00}") %>'
                                    ReadOnly="true" CssClass="currencyClass"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="���������">
                            <ItemTemplate>
                                <asp:TextBox ID="tbBrokerageValue" runat="server" Text='<%# Eval("BrokerageValue","{0:#,0.00}") %>'
                                    ReadOnly="true" CssClass="currencyClass"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="���� �� �������">
                            <ItemTemplate>
                                <asp:TextBox ID="tbOfferDate" runat="server" Text='<%# Eval("DateOfCreation","{0:d}") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="������� ���">
                            <ItemTemplate>
                                <asp:TextBox ID="tbDateOfPayment" runat="server" Text='<%# Bind("DateOfPayment", "{0:d}") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="���������� ��">
                            <ItemTemplate>
                                <asp:TextBox ID="tbUserName" runat="server" Text='<%# Eval("User.UserName") %>' ReadOnly="true"></asp:TextBox>
                                <asp:TextBox ID="tbUserFullName" Width="250px" runat="server" Text='<%# Eval("User.Name") %>'
                                    ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="��� �� ��������">
                            <ItemTemplate>
                                <asp:TextBox ID="tbDocumentSubType" runat="server" Text='<%# Eval("DocumentSubType.Description") %>'
                                    Width="300px" ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="������">
                            <ItemTemplate>
                                <asp:TextBox ID="tbStatuse" runat="server" Text='<%# Eval("Statuse.Description") %>'
                                    Width="300px" ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Fields>
                </asp:DetailsView>
            </div>
            <div id="tabeliFrame">
                <div id="header">
                    <div id="content">
                        ������� �� ������
                    </div>
                </div>
                <div id="contentOuter">
                    <div id="contentInner">
                        <asp:GridView ID="GridViewMainFinCard" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                            DataKeyNames="ID" DataSourceID="odsPolPerFacture" PageSize="20" OnRowDataBound="GridViewMainFinCard_RowDataBound"
                            GridLines="None" RowStyle-CssClass="row" CssClass="grid" OnRowCommand="GridViewMainFinCard_RowCommand"
                            OnRowEditing="GridViewMainFinCard_RowEditing" OnSelectedIndexChanging="GridViewMainFinCard_SelectedIndexChanging">
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <a href="javascript:ShowChildGrid('div<%# Eval("ID") %>');">
                                            <img id="imgdiv<%# Eval("ID") %>" alt="" border="0" src="../_assets/img/arrowdown.jpeg" />
                                        </a>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="policynumber" HeaderText="��� �� ������" SortExpression="policynumber" />
                                <asp:BoundField DataField="applicationdate" HeaderText="�����" DataFormatString="{0:d}"
                                    SortExpression="applicationdate" />
                                <asp:BoundField DataField="dolzi" HeaderText="�����" SortExpression="dolzi" DataFormatString="{0:#,0.00}"
                                    ItemStyle-CssClass="debtClass" />
                                <asp:BoundField DataField="pobaruva" HeaderText="��������" SortExpression="pobaruva"
                                    DataFormatString="{0:#,0.00}" ItemStyle-CssClass="demandClass" />
                                <asp:BoundField DataField="saldo" HeaderText="�����" SortExpression="saldo" DataFormatString="{0:#,0.00}"
                                    ItemStyle-CssClass="saldoClass" />
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        </td> </tr>
                                        <tr>
                                            <td colspan="100%">
                                                <div id="div<%# Eval("ID") %>" style="display: none; position: relative; left: 25px;">
                                                    <asp:GridView ID="GridViewFinCard" runat="server" AutoGenerateColumns="False" GridLines="None"
                                                        EmptyDataText="���� ������!" RowStyle-CssClass="row" CssClass="grid" Width="620px">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="����">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="tbDocumentDate" Width="64px" runat="server" Text='<%#Bind("DocumentDate", "{0:d}") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="������� ��">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="tbPaidDate" Width="64px" runat="server" Text='<%#Bind("PaidDate","{0:d}") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="����">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="tbDescription" Width="150px" runat="server" Text='<%#Bind("Description") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="�����">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="tbDebtValue" runat="server" Width="55px" Text='<%#Bind("DebtValue", "{0:#,0.00}") %>'
                                                                        CssClass="debtClass"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="��������">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="tbDemandValue" runat="server" Width="55px" Text='<%#Bind("DemandValue", "{0:#,0.00}") %>'
                                                                        CssClass="demandClass"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="�����">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="tbSaldoValue" runat="server" Width="55px" Text='<%#Bind("SaldoValue", "{0:#,0.00}") %>'
                                                                        CssClass="saldoClass"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <asp:ObjectDataSource ID="odsPolPerFacture" runat="server" SelectMethod="GetForFacture"
                            DataObjectTypeName="Broker.DataAccess.Policy" TypeName="Broker.DataAccess.Policy"
                            OnSelecting="odsPolPerFacture_Selecting"></asp:ObjectDataSource>
                    </div>
                </div>
            </div>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Label ID="lblDebtValue" runat="server" Text="�����"></asp:Label>
            <asp:TextBox ID="tbDebtValue" runat="server" ReadOnly="true" Width="100px" CssClass="debtClass"></asp:TextBox>
            <asp:Label ID="lblDemandValue" runat="server" Text="��������"></asp:Label>
            <asp:TextBox ID="tbDemandValue" runat="server" ReadOnly="true" Width="100px" CssClass="demandClass"></asp:TextBox>
            <asp:Label ID="lblSaldoValue" runat="server" Text="�����"></asp:Label>
            <asp:TextBox ID="tbSaldoValue" runat="server" ReadOnly="true" Width="100px" CssClass="saldoClass"></asp:TextBox>
        </asp:View>
    </asp:MultiView>
</asp:Content>
