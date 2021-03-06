<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SC0340.aspx.vb" Inherits="SC_SC0340" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <link type="text/css" rel="stylesheet" href="../StyleSheet.Css" />
    <script type="text/javascript">
    <!--
        function funAction(Param)
        {
            switch(Param)
            {
                case "btnUpdate":
                case "btnDelete":
                    if (!hasSelectedRow(''))
                    {
                        alert("未選取資料列！");
                        return false;
                    }
            }

            switch(Param)
            {
                case "btnDelete":
                    if (!confirm('確定刪除此筆資料？'))
                        return false;
                    break;
            }
        }

        function EntertoSubmit()
        {
            if (window.event.keyCode == 13)
            {
                try
                {
                    window.parent.frames[0].document.getElementById("ucButtonPermission_btnQuery").click();
                }
                catch(ex)
                {}
            }
        }
       -->
    </script>
</head>

<body style="margin-top:5px; margin-left:5px; margin-right:5px; margin-bottom:0" >
    <form id="frmContent" runat="server">
        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 100%">
            <tr>
                <td align="center" style="height: 30px;">
                    <table cellpadding="1" cellspacing="1" border="0" class="tbl_Condition" height="100%" width="100%">
                        <tr>
                            <td width="15%"></td>
                            <td align="right"><asp:Label ID="lblOrganID" Font-Size="15px" runat="server" Text="部門代號：" CssClass="MustInputCaption"></asp:Label>
                            </td>
                            <td align="left"><asp:TextBox CssClass="InputTextStyle_Thin" ID="txtOrganID" runat="server"></asp:TextBox>
                            </td>
                            <td align="right"><asp:Label ID="lblUserID" Font-Size="15px" runat="server" Text="員工編號：" CssClass="MustInputCaption"></asp:Label>
                            </td>
                            <td align="left"><asp:TextBox CssClass="InputTextStyle_Thin" ID="txtUserID" runat="server"></asp:TextBox>
                            </td>
                            <td align="right"><asp:Label ID="lblGroupID" Font-Size="15px" runat="server" Text="群組代號：" CssClass="MustInputCaption"></asp:Label>
                            </td>
                            <td align="left"><asp:TextBox CssClass="InputTextStyle_Thin" ID="txtGroupID" runat="server"></asp:TextBox>
                            </td>
                            <td width="15%"></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="center" width="100%">
                    <table width="100%" height="100%" class="tbl_Content">
                        <tr>
                            <td style="width: 100%">
                                <uc:PagerControl ID="pcMain" runat="server" GridViewName="gvMain" PerPageRecord="100" />
                            </td> 
                        </tr>
                        <tr>
                            <td style="width: 100%">
                                <asp:GridView ID="gvMain" CssClass="GridViewStyle" AllowPaging="false" runat="server" AutoGenerateColumns="False" CellPadding="2" Width="100%">
                                    <FooterStyle BackColor="#99CCCC" ForeColor="#003399" />
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemStyle CssClass="td_detail" Height="15px" />
                                            <HeaderStyle CssClass="td_header" Width="5%" />
                                            <ItemTemplate>
                                                <asp:RadioButton ID="rdo_gvMain" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="部門" SortExpression="OrganID">
                                            <ItemStyle CssClass="td_detail" HorizontalAlign="left" />
                                            <HeaderStyle CssClass="td_header" Width="15%" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblOrganID" runat="server" Text='<%# Eval("OrganID") %>'></asp:Label><asp:Label ID="lblOrganName" runat="server" Text='<%# iif(Eval("OrganID")<>"","-" & Eval("OrganName"),"") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="員工" SortExpression="UserID">
                                            <ItemStyle CssClass="td_detail" HorizontalAlign="left" />
                                            <HeaderStyle CssClass="td_header" Width="15%" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblUserID" runat="server" Text='<%# Eval("UserID") %>'></asp:Label><asp:Label ID="lblUserName" runat="server" Text='<%# iif(Eval("UserID")<>"","-" & Eval("UserName"),"") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="群組" SortExpression="GroupID">
                                            <ItemStyle CssClass="td_detail" />
                                            <HeaderStyle CssClass="td_header" Width="15%" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblGroupID" runat="server" Text='<%# Eval("GroupID") %>'></asp:Label><asp:Label ID="lblGroupName" runat="server" Text='<%# iif(Eval("GroupID")<>"","-" & Eval("GroupName"),"") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="QueryOrganID" HeaderText="可查詢部門" ReadOnly="True" SortExpression="QueryOrganID" >
                                            <HeaderStyle Width="50%" CssClass="td_header" />
                                            <ItemStyle CssClass="td_detail" Wrap="True" HorizontalAlign="Left" />
                                        </asp:BoundField>
                                    </Columns>
                                    <EmptyDataRowStyle CssClass="GridView_EmptyRowStyle" />
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblNoData" runat="server" Text="無資料顯示！"></asp:Label>
                                    </EmptyDataTemplate>
                                    <RowStyle CssClass="tr_evenline" />
                                    <AlternatingRowStyle CssClass="tr_oddline" />
                                    <SelectedRowStyle CssClass="GridView_SelectedRowStyle" BackColor="Moccasin" />
                                    <PagerStyle CssClass="GridView_PagerStyle" />
                                    <PagerSettings Position="Top" />
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
