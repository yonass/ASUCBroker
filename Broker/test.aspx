<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="test.aspx.cs" Inherits="Broker_test" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
   <div class="GridContainer">
       <asp:Button ID="btnSelectAll" runat="server" 
           Text="Селектирај сите" onclick="btnSelectAll_Click" />
        <asp:Button ID="Button1" runat="server" 
           Text="Деселектирај сите" onclick="btnDeSelectAll_Click" />
        <asp:UpdatePanel ID="pnlTables" runat="server">
            <ContentTemplate>
                <asp:GridView ID="gvTables" runat="server" AutoGenerateColumns="False" CellPadding="4"
                    ForeColor="#333333" GridLines="None" DataKeyNames = "Index" DataMember = "Name"
                    ShowFooter="True" Width="100%" AllowPaging="false" >
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                    <Columns>
                        <asp:BoundField DataField="Name" ShowHeader = "true" InsertVisible="False" ReadOnly="True"
                            SortExpression="Name" HeaderText = "ТАБЕЛА" >
                            <HeaderStyle HorizontalAlign="Left" Height = "50px" />
                        </asp:BoundField>
                        <asp:TemplateField ShowHeader="true" HeaderText = "INSERT">
                            <ItemTemplate>
                                <asp:CheckBox ID="cbInsert" runat="server" Checked = '<%# (bool)Eval("Insert") %>' AutoPostBack = "true" OnCheckedChanged = "cbInsert_changed"/>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField ShowHeader="true" HeaderText = "DELETE">
                            <ItemTemplate>
                                <asp:CheckBox ID="cbDelete" runat="server" Checked = '<%# (bool)Eval("Delete") %>' AutoPostBack = "true" OnCheckedChanged = "cbDelete_changed" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField ShowHeader="true" HeaderText = "SELECT">
                            <ItemTemplate>
                                <asp:CheckBox ID="cbSelect" runat="server" Checked = '<%# (bool)Eval("Select") %>' AutoPostBack = "true" OnCheckedChanged = "cbSelect_changed" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField ShowHeader="true" HeaderText = "UPDATE">
                            <ItemTemplate>
                                <asp:CheckBox ID="cbUpdate" runat="server" Checked = '<%# (bool)Eval("Update") %>' AutoPostBack = "true" OnCheckedChanged = "cbUpdate_changed" />
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
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
