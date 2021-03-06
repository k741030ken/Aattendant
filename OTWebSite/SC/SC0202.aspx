<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SC0202.aspx.vb" Inherits="SC_SC0202" %>

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
        <ajaxToolkit:ToolkitScriptManager runat="Server" EnableScriptGlobalization="true"
            EnableScriptLocalization="true" ID="ScriptManager1" />
        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 100%">
            <tr>
                <td align="center">
                    <table width="80%" class="tbl_Edit" cellpadding="1" cellspacing="1">
                        <tr style="height:20px">
                            <td class="td_EditHeader" width="30%" align="center"><asp:Label ID="lblSysID_H" ForeColor="Blue" runat="server" Text="*系統別代碼"></asp:Label>
                            </td>
                            <td class="td_Edit" style="width:70%" align="left"><asp:Label ID="lblSysID" runat="server" CssClass="InputTextStyle_Thin" Width="115px"></asp:Label>
                            </td>
                        </tr>
                        <tr style="height:20px">
                            <td class="td_EditHeader" width="30%" align="center"><asp:Label ID="lblSysName" ForeColor="blue" runat="server" Text="系統別名稱"></asp:Label>
                            </td>
                            <td class="td_Edit" style="width:70%" align="left"><asp:TextBox ID="txtSysName" CssClass="InputTextStyle_Thin" runat="server" MaxLength="20" Width="300px"></asp:TextBox>
                            </td>
                        </tr>                        
                        <tr style="height:20px">
                            <td class="td_EditHeader" width="30%" align="center">
                                <asp:Label ID="lblCreateDate_H" runat="server" Text="建立日期"></asp:Label></td>
                            <td class="td_Edit" style="width: 70%;" align="left">
                                <asp:Label ID="lblCreateDate" runat="server" CssClass="InputTextStyle_Thin"
                                    Width="200px"></asp:Label></td>
                        </tr>
                        <tr style="height:20px">
                            <td class="td_EditHeader" width="30%" align="center">
                                <asp:Label ID="lblLastChgComp_H" runat="server" Text="最後異動公司"></asp:Label></td>
                            <td class="td_Edit" style="width: 70%;" align="left">
                                <asp:Label ID="lblLastChgComp" runat="server" CssClass="InputTextStyle_Thin"
                                    Width="200px"></asp:Label></td>
                        </tr>
                        <tr style="height:20px">
                            <td class="td_EditHeader" width="30%" align="center">
                                <asp:Label ID="lblLastChgID_H" runat="server" Text="最後異動人员"></asp:Label></td>
                            <td class="td_Edit" style="width: 70%;" align="left">
                                <asp:Label ID="lblLastChgID" runat="server" CssClass="InputTextStyle_Thin"
                                    Width="200px"></asp:Label></td>
                        </tr>
                        <tr style="height:20px">
                            <td class="td_EditHeader" width="30%" align="center">
                                <asp:Label ID="lblLastChgDate_H" runat="server" Text="最後異動日期"></asp:Label></td>
                            <td class="td_Edit" style="width: 70%;" align="left">
                                <asp:Label ID="lblLastChgDate" runat="server" CssClass="InputTextStyle_Thin"
                                    Width="200px"></asp:Label></td>
                        </tr>
                    </table>
                </td>
            </tr>

        </table>
        </form>
</body>
</html>
