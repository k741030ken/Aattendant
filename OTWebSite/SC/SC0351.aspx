<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SC0351.aspx.vb" Inherits="SC_SC0351" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title></title>
    <link type="text/css" rel="stylesheet" href="/StyleSheet.Css" />
    <script type="text/javascript">
    <!--
    -->
    </script>
</head>
<body style="margin-top:5px; margin-left:5px; margin-right:5px; margin-bottom:0" >
    <form id="frmContent" runat="server">
        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 100%">
            <tr>
                <td align="center">
                    <table width="80%" class="tbl_Edit" cellpadding="1" cellspacing="1">
                        <tr style="height:20px">
                            <td class="td_EditHeader" width="30%" align="center"><asp:Label ID="lblBankID" CssClass="MustInputCaption" runat="server" Text="*銀行代號"></asp:Label>
                            </td>
                            <td class="td_Edit" style="width:70%" align="left"><asp:TextBox ID="txtBankID" CssClass="InputTextStyle_Thin" runat="server" MaxLength="3"></asp:TextBox>
                            </td>
                        </tr>
                        <tr style="height:20px">
                            <td class="td_EditHeader" width="30%" align="center"><asp:Label ID="lblBankName" runat="server" Text="銀行名稱" CssClass="MustInputCaption"></asp:Label>
                            </td>
                            <td class="td_Edit" style="width:70%" align="left"><asp:TextBox ID="txtBankName" CssClass="InputTextStyle_Thin" runat="server" MaxLength="100" Width="300px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr style="height:20px">
                            <td class="td_EditHeader" width="30%" align="center"><asp:Label ID="lblWorldRank" runat="server" Text="世界排名"></asp:Label>
                            </td>
                            <td class="td_Edit" style="width:70%" align="left"><asp:TextBox ID="txtWorldRank" CssClass="InputTextStyle_Thin" runat="server" MaxLength="5"></asp:TextBox>
                            </td>
                        </tr>
                        <tr style="height:20px">
                            <td class="td_EditHeader" width="30%" align="center"><asp:Label ID="lblMoodyGrade" runat="server" Text="Moody's / S&P評等"></asp:Label>
                            </td>
                            <td class="td_Edit" style="width:70%" align="left"><asp:TextBox ID="txtMoodyGrade" CssClass="InputTextStyle_Thin" runat="server" MaxLength="5"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>

        </table>
        </form>
</body>
</html>
