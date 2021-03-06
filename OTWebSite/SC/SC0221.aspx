<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SC0221.aspx.vb" Inherits="SC_SC0221" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <link type="text/css" rel="stylesheet" href="../StyleSheet.Css" />
    <script type="text/javascript">
    <!--
        function clearOnKeypress(objName)
        {
            var obj = document.getElementById(objName);
            if (obj != null)
                obj.value = '';
        }
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
                            <td class="td_EditHeader" width="30%" align="center"><asp:Label ID="lblRegionID" CssClass="MustInputCaption" runat="server" Text="*處別代碼"></asp:Label>
                            </td>
                            <td class="td_Edit" style="width:70%" align="left"><asp:TextBox ID="txtRegionID" CssClass="InputTextStyle_Thin" runat="server" MaxLength="6" style="text-transform:uppercase"></asp:TextBox>
                            </td>
                        </tr>
                        <tr style="height:20px">
                            <td class="td_EditHeader" width="30%" align="center"><asp:Label ID="lblRegionName" runat="server" Text="處別名稱" CssClass="MustInputCaption"></asp:Label>
                            </td>
                            <td class="td_Edit" style="width:70%" align="left"><asp:TextBox ID="txtRegionName" CssClass="InputTextStyle_Thin" runat="server" Width="300px" MaxLength="50"></asp:TextBox>
                            </td>
                        </tr>
                        <tr style="height:20px">
                            <td class="td_EditHeader" width="30%" align="center"><asp:Label ID="lblBoss" runat="server" Text="處別主管" CssClass="MustInputCaption"></asp:Label>
                            </td>
                            <td class="td_Edit" style="width:70%" align="left"><asp:TextBox ID="txtBoss" CssClass="InputTextStyle_Thin" runat="server" style="text-transform:uppercase " MaxLength="6" Width="60px"></asp:TextBox>&nbsp;<asp:TextBox ID="txtBossName" ReadOnly="true" runat="server" CssClass="InputTextStyle_Readonly"></asp:TextBox>
                            <uc:ButtonQuerySelect ID="ucButtonQuerySelect" runat="server" ButtonText="..." ButtonHint="處別主管選取..." WindowHeight="550" WindowWidth="500" DataControlID="txtBoss" />
                            </td>
                        </tr>
                        <tr style="height:20px">
                            <td class="td_EditHeader" width="30%" align="center"><asp:Label ID="lblUpRegionID" runat="server" Text="上層處別"></asp:Label>
                            </td>
                            <td class="td_Edit" style="width:70%" align="left"><asp:DropDownList ID="ddlUpRegionID" runat="server" CssClass="DropDownListStyle"></asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>

        </table>
        </form>
</body>
</html>
