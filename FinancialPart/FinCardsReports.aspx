<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="FinCardsReports.aspx.cs" Inherits="FinancialPart_FinCardsReports" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript">
        var TREEVIEW_ID = "ctl00_ContentPlaceHolder1_tvInsuranceTypes"; //the ID of the TreeView control
        //the constants used by GetNodeIndex()
        var LINK = 0;
        var CHECKBOX = 1;

        //this function is executed whenever user clicks on the node text
        function ToggleCheckBox(senderId) {
            var nodeIndex = GetNodeIndex(senderId, LINK);
            var checkBoxId = TREEVIEW_ID + "n" + nodeIndex + "CheckBox";
            var checkBox = document.getElementById(checkBoxId);
            checkBox.checked = !checkBox.checked;

            ToggleChildCheckBoxes(checkBox);
            ToggleParentCheckBox(checkBox);
        }

        //checkbox click event handler
        function checkBox_Click(eventElement) {
            ToggleChildCheckBoxes(eventElement.target);
            ToggleParentCheckBox(eventElement.target);
        }

        //returns the index of the clicked link or the checkbox
        function GetNodeIndex(elementId, elementType) {
            var nodeIndex;
            if (elementType == LINK) {
                nodeIndex = elementId.substring((TREEVIEW_ID + "t").length);
            }
            else if (elementType == CHECKBOX) {
                nodeIndex = elementId.substring((TREEVIEW_ID + "n").length, elementId.indexOf("CheckBox"));
            }
            return nodeIndex;
        }

        //checks or unchecks the nested checkboxes
        function ToggleChildCheckBoxes(checkBox) {
            var postfix = "n";
            var childContainerId = TREEVIEW_ID + postfix + GetNodeIndex(checkBox.id, CHECKBOX) + "Nodes";
            var childContainer = document.getElementById(childContainerId);
            if (childContainer) {
                var childCheckBoxes = childContainer.getElementsByTagName("input");
                for (var i = 0; i < childCheckBoxes.length; i++) {
                    childCheckBoxes[i].checked = checkBox.checked;
                }
            }
        }

        //unchecks the parent checkboxes if the current one is unchecked
        function ToggleParentCheckBox(checkBox) {
            if (checkBox.checked == false) {
                var parentContainer = GetParentNodeById(checkBox, TREEVIEW_ID);
                if (parentContainer) {
                    var parentCheckBoxId = parentContainer.id.substring(0, parentContainer.id.search("Nodes")) + "CheckBox";
                    if ($get(parentCheckBoxId) && $get(parentCheckBoxId).type == "checkbox") {
                        $get(parentCheckBoxId).checked = false;
                        ToggleParentCheckBox($get(parentCheckBoxId));
                    }
                }
            }
        }

        //returns the ID of the parent container if the current checkbox is unchecked
        function GetParentNodeById(element, id) {
            var parent = element.parentNode;
            if (parent == null) {
                return false;
            }
            if (parent.id.search(id) == -1) {
                return GetParentNodeById(parent, id);
            }
            else {
                return parent;
            }
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div id="tabeliFrame">
        <div id="header">
            <div id="content">
                Финансова картица
            </div>
        </div>
        <div id="contentOuter">
            <div id="contentInner">
                <table>
                    <tr>
                        <td>
                            За период
                        </td>
                        <td>
                            Од
                            <asp:TextBox ID="tbFromDate" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" ErrorMessage="*" ControlToValidate="tbFromDate"
                                Display="Dynamic"></asp:RequiredFieldValidator>
                            <asp:CompareValidator ID="cvFromDate" runat="server" ErrorMessage="*" ControlToValidate="tbFromDate"
                                Display="Dynamic" Type="Date" Operator="DataTypeCheck"></asp:CompareValidator>
                        </td>
                        <td>
                            До
                            <asp:TextBox ID="tbToDate" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvToDate" runat="server" ErrorMessage="*" ControlToValidate="tbToDate"
                                Display="Dynamic"></asp:RequiredFieldValidator>
                            <asp:CompareValidator ID="cvToDate" runat="server" ErrorMessage="*" ControlToValidate="tbToDate"
                                Display="Dynamic" Type="Date" Operator="DataTypeCheck"></asp:CompareValidator>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td style="width: 230px;">
                            Експозитура
                        </td>
                        <td style="width: 230px;">
                            Осигурителна компанија
                        </td>
                        <td style="width: 230px;">
                            Подкласа на осигурување
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Panel ID="checkBoxPanelBranches" runat="server" CssClass="scrollingControlContainer scrollingCheckBoxList">
                                <asp:CheckBoxList ID="cblBranches" runat="server" AppendDataBoundItems="true" DataSourceID="odsBranches"
                                    DataTextField="Name" DataValueField="ID" AutoPostBack="True" OnSelectedIndexChanged="cblBranches_SelectedIndexChanged">
                                    <asp:ListItem Text="СИТЕ" Value="All"></asp:ListItem>
                                </asp:CheckBoxList>
                                <asp:ObjectDataSource ID="odsBranches" runat="server" DataObjectTypeName="Broker.DataAccess.Branch"
                                    TypeName="Broker.DataAccess.Branch" SelectMethod="Select"></asp:ObjectDataSource>
                            </asp:Panel>
                        </td>
                        <td>
                            <asp:Panel ID="checkBoxPanelInsuranceCompanies" runat="server" CssClass="scrollingControlContainer scrollingCheckBoxList">
                                <asp:CheckBoxList ID="cblInsuranceCompanies" runat="server" AppendDataBoundItems="true"
                                    DataSourceID="odsInsuranceCompanies" DataTextField="ShortName" DataValueField="ID"
                                    CssClass="checkBoxList" AutoPostBack="True" OnSelectedIndexChanged="cblInsuranceCompanies_SelectedIndexChanged">
                                    <asp:ListItem Text="СИТЕ" Value="All"></asp:ListItem>
                                </asp:CheckBoxList>
                                <asp:ObjectDataSource ID="odsInsuranceCompanies" SelectMethod="Select" runat="server"
                                    TypeName="Broker.DataAccess.InsuranceCompany" DataObjectTypeName="Broker.DataAccess.InsuranceCompany">
                                </asp:ObjectDataSource>
                            </asp:Panel>
                        </td>
                        <td>
                            <asp:Panel ID="checkBoxPanelInsuranceSubTypes" runat="server" CssClass="scrollingControlContainer scrollingCheckBoxList">
                                <asp:CheckBox ID="cbAllInsuranceSubTypes" runat="server" AutoPostBack="true" 
                                    Text="Сите" oncheckedchanged="cbAllInsuranceSubTypes_CheckedChanged" />
                                <asp:TreeView ID="tvInsuranceTypes" runat="server" ShowCheckBoxes="All" Width="150px">
                                </asp:TreeView>
                                <%--<asp:CheckBoxList ID="cblInsuranceSubTypes" runat="server" AppendDataBoundItems="true"
                                    DataTextField="ShortDescription" DataValueField="ID" DataSourceID="odsInsuranceSubTypes"
                                    AutoPostBack="True" OnSelectedIndexChanged="cblInsuranceSubTypes_SelectedIndexChanged">
                                    <asp:ListItem Text="СИТЕ" Value="All"></asp:ListItem>
                                </asp:CheckBoxList>
                                <asp:ObjectDataSource ID="odsInsuranceSubTypes" SelectMethod="Select" runat="server"
                                    TypeName="Broker.DataAccess.InsuranceSubType" DataObjectTypeName="Broker.DataAccess.InsuranceSubType">
                                </asp:ObjectDataSource>--%>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Корисник(вработен)
                        </td>
                        <td>
                            Маркетинг агенти / Надворешен соработник
                        </td>
                        <td>
                            Селектирано по
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Panel ID="checkBoxPanelUsers" runat="server" CssClass="scrollingControlContainer scrollingCheckBoxList">
                                <asp:CheckBoxList ID="cblUsers" runat="server" AppendDataBoundItems="true" DataSourceID="odsUsers"
                                    DataTextField="Name" DataValueField="ID" AutoPostBack="True" OnSelectedIndexChanged="cblUsers_SelectedIndexChanged">
                                    <asp:ListItem Text="СИТЕ" Value="All"></asp:ListItem>
                                </asp:CheckBoxList>
                                <asp:ObjectDataSource ID="odsUsers" runat="server" SelectMethod="Select" TypeName="Broker.DataAccess.User"
                                    DataObjectTypeName="Broker.DataAccess.User"></asp:ObjectDataSource>
                            </asp:Panel>
                        </td>
                        <td>
                            <asp:Panel ID="checkBoxPanelMarketingAgents" runat="server" CssClass="scrollingControlContainer scrollingCheckBoxList">
                                <asp:CheckBoxList ID="cblMarketingAgents" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblMarketingAgents_SelectedIndexChanged">
                                    <asp:ListItem Text="СИТЕ" Value="All"></asp:ListItem>
                                    <asp:ListItem Text="Без маркетинг агенти" Value="NoMarketingAgents"></asp:ListItem>
                                    <asp:ListItem Text="Само сите маркетинг агенти" Value="AllMarketingAgents"></asp:ListItem>
                                </asp:CheckBoxList>
                                <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="Select"
                                    TypeName="Broker.DataAccess.User" DataObjectTypeName="Broker.DataAccess.User">
                                </asp:ObjectDataSource>
                            </asp:Panel>
                        </td>
                        <td>
                            <asp:RadioButtonList ID="rblFinCardType" runat="server" RepeatDirection="Vertical">
                                <asp:ListItem Selected="True" Text="По датум на издавање" Value="FinCardByApplicationDate"></asp:ListItem>
                                <asp:ListItem Text="По датуми на доспевање/уплата" Value="FinCardByPaidDates"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                </table>
                <asp:Button ID="btnReport" runat="server" Text="Извештај" OnClick="btnReport_Click" />
            </div>
        </div>
    </div>

    <script type="text/javascript">
        var links = document.getElementsByTagName("a");
        for (var i = 0; i < links.length; i++) {
            if (links[i].className == TREEVIEW_ID + "_0") {
                links[i].href = "javascript:ToggleCheckBox(\"" + links[i].id + "\");";
            }
        }

        var checkBoxes = document.getElementsByTagName("input");
        for (var i = 0; i < checkBoxes.length; i++) {
            if (checkBoxes[i].type == "checkbox") {
                $addHandler(checkBoxes[i], "click", checkBox_Click);
            }
        }
    </script>

</asp:Content>
