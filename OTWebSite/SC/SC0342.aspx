<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SC0342.aspx.vb" Inherits="SC_SC0342" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <link type="text/css" rel="stylesheet" href="../StyleSheet.Css" />
    <script type="text/javascript">
    <!--
    -->
    </script>
</head>
<body style="margin-top:5px; margin-left:5px; margin-right:5px; margin-bottom:0">
    <form id="frmContent" runat="server">
        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 100%">
            <tr>
                <td align="center">
                    <table width="80%" class="tbl_Edit" cellpadding="1" cellspacing="1">
                        <tr style="height:20px">
                            <td class="td_EditHeader" width="25%" align="center"><asp:Label ID="lblOrganID_H" CssClass="MustInputCaption" runat="server" Text="*部門"></asp:Label>
                            </td>
                            <td class="td_Edit" style="width:75%" align="left"><asp:Label ID="lblOrganID" Font-Bold="true" CssClass="InputTextStyle_Thin" runat="server" Width="115px"></asp:Label>
                            </td>
                        </tr>
                        <tr style="height:20px">
                            <td class="td_EditHeader" width="25%" align="center"><asp:Label ID="lblUserID_H" CssClass="MustInputCaption" runat="server" Text="*員工"></asp:Label>
                            </td>
                            <td class="td_Edit" style="width:75%" align="left"><asp:Label ID="lblUserID" Font-Bold="true" CssClass="InputTextStyle_Thin" runat="server" Width="115px"></asp:Label>
                            </td>
                        </tr>
                        <tr style="height:20px">
                            <td class="td_EditHeader" width="25%" align="center"><asp:Label ID="lblGroupID_H" CssClass="MustInputCaption" runat="server" Text="*群組"></asp:Label>
                            </td>
                            <td class="td_Edit" style="width:75%" align="left"><asp:Label ID="lblGroupID" Font-Bold="true" CssClass="InputTextStyle_Thin" runat="server" Width="115px"></asp:Label>
                            </td>
                        </tr>
                        <tr style="height:20px">
                            <td class="td_EditHeader" width="25%" align="center"><asp:Label ID="lblQueryOrganID" runat="server" Text="可查詢部門"></asp:Label>
                            </td>
                            <td class="td_Edit" style="width:75%" align="left"><asp:LinkButton ID="btnSelectAll" runat="server" ForeColor="blue" Text="全選"></asp:LinkButton>&nbsp;&nbsp;<asp:LinkButton ID="btnUnSelectAll" ForeColor="blue" runat="server" Text="全不選"></asp:LinkButton><br />
                            <asp:CheckBoxList ID="chkQueryOrganID" runat="server" RepeatColumns="3" RepeatDirection="Horizontal"></asp:CheckBoxList>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>

        </table>
        </form>
</body>
</html>
