<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SC0252.aspx.vb" Inherits="SC_SC0252" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <link type="text/css" rel="stylesheet" href="../StyleSheet.Css" />
    <script type="text/javascript">
    <!--
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
                            <td width="150"></td>
                            <td align="right" width="40%"><asp:Label ID="lblGroupID_Con" Font-Size="15px" runat="server" Text="群組選取：" ForeColor="Blue"></asp:Label>
                            </td>
                            <td align="left" width="60%"><asp:DropDownList ID="ddlGroupID" Font-Names="Calibri,新細明體" runat="server"></asp:DropDownList>
                                <asp:LinkButton ID="btnChangeToUser" runat="server" Font-Size="12px" Text="切換以使用者設定"></asp:LinkButton>
                            </td>
                            <td width="150"></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="center" width="100%">
                    <table width="100%" height="100%" class="tbl_Content">
                        <tr>
                            <td style="width: 100%">
                                <table class="tbl_Content" width="80%">
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="30%" align="center"><asp:Label ID="lblGroupID_H" ForeColor="Blue" runat="server" Text="群组代碼"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:70%" align="left"><asp:Label ID="lblGroupID" runat="server" CssClass="InputTextStyle_Thin" Width="115px"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="30%" align="center"><asp:Label ID="lblGroupName_H" ForeColor="blue" runat="server" Text="組名"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:70%" align="left"><asp:Label ID="lblGroupName" CssClass="InputTextStyle_Thin" runat="server" Width="300px"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100%" colspan="2">
                                            <asp:ListBox ID="lstUser" runat="server" Font-Names="Calibri,新細明體" Width="300px" Rows="20"></asp:ListBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
