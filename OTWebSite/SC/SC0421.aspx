<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SC0421.aspx.vb" Inherits="SC_SC0421" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <link type="text/css" rel="stylesheet" href="../StyleSheet.Css" />
    <script type="text/javascript">
    <!--
        function EnableSizeBox() {
            var obj = document.getElementById("rblOpenWSize_3");
            
            if (obj.value == "C") {
                frmContent.txtSize.disabled = false;
                frmContent.txtSize.focus();
            }
            else {
                frmContent.txtSize.disabled = true;
            }
        }
    -->
    </script>
</head>
<body style="margin-top:5px; margin-left:5px; margin-right:5px; margin-bottom:0" onload="EnableSizeBox();" >
    <form id="frmContent" runat="server">
        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 100%">
            <tr>
                <td align="center">
                    <table width="80%" class="tbl_Edit" cellpadding="1" cellspacing="1">
                        <tr style="height:20px">
                            <td class="td_EditHeader" width="30%" align="center"><asp:Label ID="lblMsgCode" ForeColor="Blue" runat="server" Text="*訊息代碼"></asp:Label>
                            </td>
                            <td class="td_Edit" style="width:70%" align="left"><asp:TextBox ID="txtMsgCode" runat="server" CssClass="InputTextStyle_Thin" MaxLength="10"></asp:TextBox>
                            </td>
                        </tr>
                        <tr style="height:20px">
                            <td class="td_EditHeader" width="30%" align="center"><asp:Label ID="lblMsgReason" ForeColor="Blue" runat="server" Text="*訊息描述"></asp:Label>
                            </td>
                            <td class="td_Edit" style="width:70%" align="left"><asp:TextBox ID="txtMsgReason" CssClass="InputTextStyle_Thin" runat="server" MaxLength="100" Width="400px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr style="height:20px">
                            <td class="td_EditHeader" width="30%" align="center"><asp:Label ID="lblMsgUrl" ForeColor="Blue" runat="server" Text="*連結網頁"></asp:Label>
                            </td>
                            <td class="td_Edit" style="width:70%" align="left"><asp:TextBox ID="txtMsgUrl" CssClass="InputTextStyle_Thin" runat="server" MaxLength="100" Width="400"></asp:TextBox>
                            </td>
                        </tr>
                        <tr style="height:20px">
                            <td class="td_EditHeader" width="30%" align="center"><asp:Label ID="lblMsgKind" ForeColor="Blue" runat="server" Text="*訊息類別"></asp:Label>
                            </td>
                            <td class="td_Edit" style="width:70%" align="left"><asp:RadioButtonList ID="rblMsgKind" runat="server" RepeatDirection="Horizontal"></asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr style="height:20px">
                            <td class="td_EditHeader" width="30%" align="center"><asp:Label ID="lblOpenFlag" ForeColor="Blue" runat="server" Text="*網頁開啟方式"></asp:Label>
                            </td>
                            <td class="td_Edit" style="width:70%" align="left"><asp:RadioButtonList ID="rblOpenFlag" runat="server" RepeatDirection="Horizontal"></asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr style="height:20px">
                            <td class="td_EditHeader" width="30%" align="center"><asp:Label ID="lblOpenWSize" ForeColor="Blue" runat="server" Text="*開啟視窗大小"></asp:Label>
                            </td>
                            <td class="td_Edit" style="width:70%" align="left">
                                <table border="0">
                                    <tr>
                                        <td>
                                            <asp:RadioButtonList ID="rblOpenWSize" runat="server" RepeatDirection="Horizontal">
                                                <asp:ListItem Text="S" Value="S"></asp:ListItem>
                                                <asp:ListItem Text="M" Value="M"></asp:ListItem>
                                                <asp:ListItem Text="L" Value="L"></asp:ListItem>
                                                <asp:ListItem Text="Custom" Value="C"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtSize" Enabled="false" runat="server" CssClass="InputTextStyle_Thin" MaxLength="30"></asp:TextBox>
                                        </td>
                                        <td>
                                            <font style="color:dimgray; font-size:12px">輸入格式 width*height</font>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr style="height:20px">
                            <td class="td_EditHeader" width="30%" align="center"><asp:Label ID="Label1" runat="server" ForeColor="Blue" Text="*開啟刪除註記"></asp:Label>
                            </td>
                            <td class="td_Edit" style="width:70%" align="left">
                                <asp:RadioButtonList ID="rblDelKind" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Text="刪除" Value="Y"></asp:ListItem>
                                    <asp:ListItem Text="不刪" Value="N"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>

        </table>
        </form>
</body>
</html>
