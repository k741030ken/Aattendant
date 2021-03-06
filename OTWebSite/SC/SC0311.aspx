<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SC0311.aspx.vb" Inherits="SC_SC0311" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <link type="text/css" rel="stylesheet" href="../StyleSheet.Css" />
    <script type="text/javascript">
    <!--
        function funAction(Param)
        {
            if (Param == 'btnActionX')
                window.top.close();
        }
        
        function onShowModalReturn(returnValue)
        {
            if (returnValue == undefined || returnValue == '')
                return false
            else
                return true;
        }
       -->
    </script>
</head>

<body style="margin-top:5px; margin-left:5px; margin-right:5px; margin-bottom:0" >
    <form id="frmContent" runat="server">
        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 100%">
            <tr>
                <td align="center" width="100%">
                    <table width="95%" class="tbl_Edit" cellpadding="1" cellspacing="1">
                        <tr style="height:20px">
                            <td class="td_EditHeader" width="25%" align="center"><asp:Label ID="lblAreaID" cssclass="MustInputCaption" runat="server" Text="*區域"></asp:Label>
                            </td>
                            <td class="td_Edit" style="width:75%" align="left">
                                <asp:DropDownList ID="ddlAreaID" runat="server" CssClass="DropDownListStyle" Width="115px">
                                    <asp:ListItem Text="TW-台灣" Value="TW" Selected></asp:ListItem>
                                    <asp:ListItem Text="HK-香港" Value="HK"></asp:ListItem>
                                    <asp:ListItem Text="MO-澳門" Value="MO"></asp:ListItem>
                                    <asp:ListItem Text="LA-Los Angeles" Value="LA"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_EditHeader" width="25%" align="center"><asp:Label ID="lblYear" cssclass="MustInputCaption" runat="server" Text="*年度"></asp:Label>
                            </td>
                            <td class="td_Edit" style="width:75%" align="left">
                                <asp:DropDownList ID="ddlYear" runat="server" CssClass="DropDownListStyle" Width="115px"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_EditHeader" width="25%" align="center"><asp:Label ID="Label1" cssclass="MustInputCaption" runat="server" Text="*檔案路徑"></asp:Label>
                            </td>
                            <td class="td_Edit" style="width:75%" align="left">
                                <asp:FileUpload ID="calendarUpload" runat="server" Width="100%" CssClass="InputTextStyle_Thin" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
