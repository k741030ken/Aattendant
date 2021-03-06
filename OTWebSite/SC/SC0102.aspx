<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SC0102.aspx.vb" Inherits="SC_SC0102" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <link type="text/css" rel="stylesheet" href="../StyleSheet.Css" />
    <script type="text/javascript">
    <!--
    -->
    </script>
    <style type="text/css">
        
    </style>
</head>
<body style="margin-top:5px; margin-left:5px; margin-right:5px; margin-bottom:0">
    <form id="frmContent" runat="server">
        <ajaxToolkit:ToolkitScriptManager runat="Server" EnableScriptGlobalization="true" EnableScriptLocalization="true" ID="ScriptManager1" />
        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 100%">
            <tr>
                <td align="center">
                    <table width="80%" class="tbl_Edit" cellpadding="1" cellspacing="1">
                        <tr style="height:20px">
                            <td class="td_EditHeader" align="center">
                                <asp:Label ID="lblSysID_H" ForeColor="Blue" runat="server" Text="系統別"></asp:Label>
                            </td>
                            <td class="td_Edit" align="left">
                                <asp:Label ID="lblSysName" runat="server"></asp:Label>
                            </td>
                        </tr>                   
                        <tr style="height:20px">                           
                            <td class="td_EditHeader" align="center">
                                <asp:Label ID="lblCompRoleID_H" ForeColor="blue" runat="server" Text="授權公司"></asp:Label>
                            </td>
                            <td class="td_Edit" align="left">
                                <asp:Label ID="lblCompRoleName" runat="server"></asp:Label>
                            </td>
                        </tr>                   
                        <tr style="height:20px">
                            <td class="td_EditHeader" align="center">
                                <asp:Label ID="lblUser_H" ForeColor="Blue" runat="server" Text="員工"></asp:Label>
                            </td>
                            <td class="td_Edit" align="left">
                                <asp:Label ID="lblUserID" runat="server"></asp:Label>
                                <asp:Label ID="lblUser" runat="server" Text=" - "></asp:Label>
                                <asp:Label ID="lblUserName" runat="server"></asp:Label>
                            </td>
                        </tr>                   
                        <tr style="height:20px">
                            <td class="td_EditHeader" align="center">
                                <asp:Label ID="lblDeptOrganID_H" ForeColor="blue" runat="server" Text="部門 / 科組課"></asp:Label>
                            </td>
                            <td class="td_Edit" align="left">
                                <asp:Label ID="lblDeptID" runat="server"></asp:Label>
                                <asp:Label ID="lblDeptOrg" runat="server" Text=" / "></asp:Label>
                                <asp:Label ID="lblOrganID" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr style="height:20px">
                            <td class="td_EditHeader" align="center">
                                <asp:Label ID="lblWorkStatus_H" runat="server" Text="狀態"></asp:Label>
                            </td>
                            <td class="td_Edit" align="left">
                                <asp:Label ID="lblWorkStatus" runat="server"></asp:Label>
                            </td>
                        </tr>                   
                        <tr style="height:20px">
                            <td class="td_EditHeader" align="center">
                                <asp:Label ID="lblTitleID_H" ForeColor="blue" runat="server" Text="職稱"></asp:Label>
                            </td>
                            <td class="td_Edit" align="left">
                                <asp:Label ID="lblTitleID" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr style="height:20px">
                            <td class="td_EditHeader" align="center">
                                <asp:Label ID="lblBanMark" runat="server" Text="禁用註記"></asp:Label>
                            </td>
                            <td class="td_Edit" align="left">
                                <asp:CheckBox ID="chkBanMark" runat="server" AutoPostBack="true" />
                            </td>
                        </tr>
                        <tr style="height:20px">
                            <td class="td_EditHeader" align="center">
                                <asp:Label ID="lblBanMarkValidDate" runat="server" Text="禁用生效日"></asp:Label>
                            </td>
                            <td class="td_Edit" align="left">
                                <asp:TextBox ID="txtBanMarkValidDate" CssClass="InputTextStyle_Thin" runat="server" MaxLength="10" Width="200px"></asp:TextBox>
                                <asp:ImageButton runat="Server" ID="imgBanMarkValidDate" ImageUrl="~/images/Calendar.gif" AlternateText="Click to show calendar" />
                                <ajaxToolkit:CalendarExtender ID="CalendarBanMarkValidDate" runat="server" TargetControlID="txtBanMarkValidDate" PopupButtonID="imgBanMarkValidDate" Format="yyyy/MM/dd" />
                            </td>
                        </tr>
                        <tr style="height:20px">
                            <td class="td_EditHeader" align="center">
                                <asp:Label ID="lblResetPwd" runat="server" Text="放行密碼重設"></asp:Label>
                            </td>
                            <td class="td_Edit" align="left">
                                <%--<asp:CheckBox ID="chkResetPwd" runat="server" />--%>
                                <asp:Button ID="btnResetPwd" Text="密碼重設" CssClass="buttonface" OnClientClick="return confirm('確定要重設密碼？')" runat="server" />
                            </td>
                        </tr>
                        <tr style="height:20px">
                            <td class="td_EditHeader" align="center">
                                <asp:Label ID="lblCreateDate_H" runat="server" Text="建立日期"></asp:Label>
                            </td>
                            <td class="td_Edit" align="left">
                                <asp:Label ID="lblCreateDate" runat="server" CssClass="InputTextStyle_Thin" Width="200px"></asp:Label>
                            </td>
                        </tr>
                        <tr style="height:20px">
                            <td class="td_EditHeader" align="center">
                                <asp:Label ID="lblLastChgComp_H" runat="server" Text="最後異動公司"></asp:Label>
                            </td>
                            <td class="td_Edit" align="left">
                                <asp:Label ID="lblLastChgComp" runat="server" CssClass="InputTextStyle_Thin" Width="200px"></asp:Label>
                            </td>
                        </tr>
                        <tr style="height:20px">
                            <td class="td_EditHeader" align="center">
                                <asp:Label ID="lblLastChgID_H" runat="server" Text="最後異動人員"></asp:Label>
                            </td>
                            <td class="td_Edit" align="left">
                                <asp:Label ID="lblLastChgID" runat="server" CssClass="InputTextStyle_Thin" Width="200px"></asp:Label>
                            </td>
                        </tr>
                        <tr style="height:20px">
                            <td class="td_EditHeader" align="center">
                                <asp:Label ID="lblLastChgDate_H" runat="server" Text="最後異動日期"></asp:Label>
                            </td>
                            <td class="td_Edit" align="left">
                                <asp:Label ID="lblLastChgDate" runat="server" CssClass="InputTextStyle_Thin" Width="200px"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>

        </table>
        </form>
</body>
</html>
