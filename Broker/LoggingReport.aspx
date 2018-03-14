<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="LoggingReport.aspx.cs" Inherits="Broker_LoggingReport" Title="Logging Report" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
	<script type="text/javascript">
	    function divexpandcollapse(divname) {
	        var img = "img" + divname;
	        if ($("#" + img).attr("src") == "../_assets/img/plus.png") {
	            $("#" + img)
				.closest("tr")
				.after("<tr><td></td><td colspan = '80%'>" + $("#" + divname)
				.html() + "</td></tr>");
	            $("#" + img).attr("src", "../_assets/img/minus.png");
	        } else {
	            $("#" + img).closest("tr").next().remove();
	            $("#" + img).attr("src", "../_assets/img/plus.png");
	        }
	    }

	    function divexpandcollapse2(divname) {
	        var img = "img" + divname;
	        if ($("#" + img).attr("src") == "../_assets/img/plus.png") {
	            $("#" + img)
				.closest("div").
				
	            $("#" + img).attr("src", "../_assets/img/minus.png");
	        } else {
	            $("#" + img).closest("div").style.display = "block";
	            $("#" + img).attr("src", "../_assets/img/plus.png");
	        }
	    }

//	    function expand(divname) {
//        
//	        var img = "img" + divname;
//	        if ($("#" + img).attr("src") == "../_assets/img/plus.png") {
//	            $("#" + divname).style.display = 'block';
//	            $("#" + img).attr("src", "../_assets/img/minus.png");
//	        } else {
//	            $("#" + divname +).style.display = 'none';
//	            $("#" + img).attr("src", "../_assets/img/plus.png");
//	        }

//	    }

        function ExpandCollapseDiv(div_id){
            var obj_div = document.getElementById("ctl00_ContentPlaceHolder1_gvDBActivitiesBase_ctl02_gvDBActivities_" + div_id);
            alert(div_id);
            if(obj_div.style.display == "none")
            {

                obj_div.style.display = "block";
            }
            else
            {
                obj_div.style.display = "none"
            }
        }

	    function showID(divname) {

	        var inputs = document.getElementById(divname);
	        alert(inputs);
	    }

	   
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
   <div class="GridContainer">
       <asp:UpdatePanel ID="pnlTables" runat="server">
            <ContentTemplate>
                <asp:GridView ID="gvDBActivitiesBase" runat="server" AutoGenerateColumns="False" CellPadding="4"
                    ForeColor="#333333" GridLines="None" DataKeyNames = "ID" BorderStyle = "Groove" BorderWidth = "2" 
                    ShowFooter="True" Width="100%" AllowPaging="false" OnRowDataBound = "gvDBActivitiesBase_RowDataBound">
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" BorderStyle = "Groove" BorderWidth = "2" />
                    <Columns>
                        <asp:TemplateField ShowHeader="true" HeaderText = "">
                            <ItemTemplate>
                                <a href="JavaScript:divexpandcollapse('div<%# Eval("ID") %>');">
							        <img alt="Details" id="imgdiv<%# Eval("ID") %>" src="../_assets/img/plus.png" />
						        </a>
                                <div id="div<%# Eval("ID") %>" style="display: none;">
                                    <asp:GridView ID="gvDBActivities" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                        ForeColor="#333333" GridLines="None" OnRowDataBound = "gvDBActivities_RowDataBound" DataKeyNames = "ViewDBActivitiesID"
                                        ShowFooter="True" Width="100%" AllowPaging="false"  BorderStyle = "Groove" BorderWidth = "2">
                                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" BorderStyle = "Groove" BorderWidth = "2" />
                                        <Columns>
                                            <asp:BoundField DataField="Description" ShowHeader = "true" InsertVisible="False" ReadOnly="True"
                                                SortExpression="Description" HeaderText = "Акција" >
                                                <HeaderStyle HorizontalAlign="Left" Height = "50px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="TableName" ShowHeader = "true" InsertVisible="False" ReadOnly="True"
                                                SortExpression="TableName" HeaderText = "Табела" >
                                                <HeaderStyle HorizontalAlign="Left" Height = "50px"/>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Date" ShowHeader = "true" InsertVisible="False" ReadOnly="True"
                                                SortExpression="Date" HeaderText = "Време" >
                                                <HeaderStyle HorizontalAlign="Left" Height = "50px" />
                                            </asp:BoundField>
                                            <asp:TemplateField ShowHeader="true" HeaderText = "" >
                                                <ItemTemplate>
                                                    <%--<a href="JavaScript:divexpandcollapse2('div<%# Eval("ViewDBActivitiesID") %>');">
							                            <img alt="Details" id="imgdiv<%# Eval("ViewDBActivitiesID") %>" src="../_assets/img/plus.png" />
						                            </a>--%>
                                                    <div id="div<%# Eval("ViewDBActivitiesID") %>" style = "display:block" >
                                                        <asp:GridView ID="gvDBActivitiesUpdateFields" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                                            ForeColor="#333333" GridLines="None" Width = "100%"
                                                            ShowFooter="True"  AllowPaging="false"  BorderStyle = "Groove" BorderWidth = "2">
                                                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" BorderStyle = "Groove" BorderWidth = "2" />
                                                            <Columns>
                                                                <asp:BoundField DataField="FieldName" ShowHeader = "true" InsertVisible="False" ReadOnly="True"
                                                                    SortExpression="FieldName" HeaderText = "Име на полето" >
                                                                    <HeaderStyle HorizontalAlign="Left" Height = "50px" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Value" ShowHeader = "true" InsertVisible="False" ReadOnly="True"
                                                                    SortExpression="Value" HeaderText = "Нова вредност" >
                                                                    <HeaderStyle HorizontalAlign="Left" Height = "50px" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="OldValue" ShowHeader = "true" InsertVisible="False" ReadOnly="True"
                                                                    SortExpression="TableName" HeaderText = "Стара вредност" >
                                                                    <HeaderStyle HorizontalAlign="Left" Height = "50px" />
                                                                </asp:BoundField>
                                                            </Columns>
                                                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                            <EditRowStyle BackColor="#999999" />
                                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                        </asp:GridView>
                                                    </div>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                        <EditRowStyle BackColor="#999999" />
                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                    </asp:GridView>
                                </div>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center"/>
                        </asp:TemplateField>
                        <asp:BoundField DataField="ID" ShowHeader = "true" InsertVisible="False" ReadOnly="True"
                            SortExpression="ID" HeaderText = "Број" >
                            <HeaderStyle HorizontalAlign="Left" Height = "50px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Name" ShowHeader = "true" InsertVisible="False" ReadOnly="True"
                            SortExpression="Name" HeaderText = "Име" >
                            <HeaderStyle HorizontalAlign="Left" Height = "50px" />
                        </asp:BoundField>
                         <asp:BoundField DataField="SessionID" ShowHeader = "true" InsertVisible="False" ReadOnly="True"
                            SortExpression="SessionID" HeaderText = "Сесија" >
                            <HeaderStyle HorizontalAlign="Left" Height = "50px" />
                        </asp:BoundField>
                        
                    </Columns>
                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <EditRowStyle BackColor="#999999" />
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
