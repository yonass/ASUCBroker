<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TEST.aspx.cs" Inherits="TEST" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Button ID="btnSave" runat="server" Text="Сними" onclick="btnSave_Click" />
        <asp:GridView ID="gvTables" runat="server" AutoGenerateColumns="False" CellPadding="4"
            ForeColor="#333333" GridLines="None" Width="100%" >
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" HorizontalAlign="Left" />
            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" HorizontalAlign="Left" />
            <EditRowStyle BackColor="#999999" />
            <AlternatingRowStyle BackColor="White" ForeColor="#244068" />
            <Columns>
                <asp:TemplateField>
                  <ItemTemplate>
                    <%# Container.DataItemIndex + 1 %>
                  </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Name" ShowHeader = "true" InsertVisible="False" ReadOnly="True"
                    SortExpression="Name" HeaderText = "Име колона" >
                </asp:BoundField>
                <asp:TemplateField ShowHeader="true" HeaderText = "Македонско Име">
                    <ItemTemplate>
                        <asp:textbox ID="tbNameMKD" runat="server" Text = '<%# Bind("NameMKD") %>' Width = "250px"/>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ShowHeader="true" HeaderText = "Опис">
                    <ItemTemplate>
                        <asp:textbox ID="tbDescription" runat="server" Text = '<%# Bind("Description") %>' Width = "250px"/>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    </form>
</body>
</html>
