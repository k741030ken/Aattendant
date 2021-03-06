<%@ Page Language="VB" AutoEventWireup="false" CodeFile="PA1A01.aspx.vb" Inherits="PA_PA1A01" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <link type="text/css" rel="stylesheet" href="../StyleSheet.Css" />
    <script type="text/javascript">
    <!--
        //        隱藏TR
        function hide_tr(strID) {
            var result_style = document.getElementById(strID).style; result_style.display = 'none';
        }
        //        顯示TR
        function show_tr(strID) {
            var result_style = document.getElementById(strID).style; result_style.display = 'table-row';
        }
        function ChangeValue(e) {
            alert(e.id);         
        }
    -->
    </script>
</head>
<body style="margin-top:5px; margin-left:5px; margin-right:5px; margin-bottom:0" >
    <form id="frmContent" runat="server">
        <ajaxToolkit:ToolkitScriptManager runat="Server" EnableScriptGlobalization="true"
            EnableScriptLocalization="true" ID="ScriptManager1" />
        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 100%">
            <tr>
                <td>            
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 100%">
                        <tr>
                            <td align="center">
                                <table width="80%" class="tbl_Edit" cellpadding="1" cellspacing="1">
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <uc:Release ID="ucRelease" runat="server" WindowHeight="350" WindowWidth="350" style="display:none" />
                                            <asp:Label ID="lblReleaseResult" runat="server" ForeColor="Blue" Text="" style="display:none"></asp:Label>
                                            <asp:Label ID="lblCompID" ForeColor="Blue" runat="server" Text="*公司代碼"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left">                                
                                            <asp:Label ID="lbltxtCompID" runat="server" ></asp:Label>
                                        </td>
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblSysDate" runat="server" ForeColor="Blue" Text="*系統日期"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left">
                                            <uc:uccalender ID="txtSysDate" runat="server" Enabled="True" />
                                        </td>
                                    </tr> 
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblWeek" runat="server" Text="星期"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left">
                                            <asp:DropDownList ID="ddlWeek" runat="server" AutoPostBack="true" Font-Names="細明體">
                                                <asp:ListItem Value="" Text="---請選擇---" Selected="true"></asp:ListItem>
                                                <asp:ListItem Value="1" Text="1-星期一"></asp:ListItem>
                                                <asp:ListItem Value="2" Text="2-星期二"></asp:ListItem>
                                                <asp:ListItem Value="3" Text="3-星期三"></asp:ListItem>
                                                <asp:ListItem Value="4" Text="4-星期四"></asp:ListItem>
                                                <asp:ListItem Value="5" Text="5-星期五"></asp:ListItem>
                                                <asp:ListItem Value="6" Text="6-星期六"></asp:ListItem>
                                                <asp:ListItem Value="7" Text="7-星期日"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>                    
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblHolidayOrNot" runat="server" Text="假日註記"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left">
                                            <asp:RadioButton ID="rbnHolidayOrNot1Show" GroupName="rbnHolidayOrNot" runat="server" Text="假日" />
                                            <asp:RadioButton ID="rbnHolidayOrNot2Show" GroupName="rbnHolidayOrNot" runat="server" Text="營業日" />
                                        </td>
                                    </tr>       
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblLastBusDate" runat="server" ForeColor="Blue" Text="*上一營業日"></asp:Label>
                                        </td>
                                        <td class="td_Edit" width="35%" align="left">
                                            <uc:uccalender ID="txtLastBusDate" runat="server" Enabled="True" />
                                        </td>
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblNextBusDate" runat="server" ForeColor="Blue" Text="*下一營業日"></asp:Label>
                                        </td>
                                        <td class="td_Edit" width="35%" align="left">
                                            <uc:uccalender ID="txtNextBusDate" runat="server" Enabled="True" />
                                        </td> 
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblNeNeBusDate" runat="server" ForeColor="Blue" Text="*下下營業日"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left">
                                            <uc:uccalender ID="txtNeNeBusDate" runat="server" Enabled="True" />
                                        </td>
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblLastDateDiff" runat="server" Text="上營業日差"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left">
                                            <asp:TextBox ID="txtLastDateDiff" CssClass="InputTextStyle_Thin" runat="server" MaxLength="8"></asp:TextBox> 
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblNextDateDiff" runat="server" Text="下營業日差"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left">
                                            <asp:TextBox ID="txtNextDateDiff" CssClass="InputTextStyle_Thin" runat="server" MaxLength="8"></asp:TextBox>
                                        </td>
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblNeNeDateDiff" runat="server" Text="下下營業日差"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left">
                                            <asp:TextBox ID="txtNeNeDateDiff" CssClass="InputTextStyle_Thin" runat="server" MaxLength="8"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr style="height:20px;DISPLAY:table-row" id="trBossType">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblLastEndDate" runat="server" ForeColor="Blue" Text="*上月月底日"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left">
                                            <uc:uccalender ID="txtLastEndDate" runat="server" Enabled="True" />
                                        </td>
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblThisEndDate" runat="server" ForeColor="Blue" Text="*本月月底日"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left">
                                            <uc:uccalender ID="txtThisEndDate" runat="server" Enabled="True" />
                                        </td>
                                    </tr>
                                    <tr style="height:20px;DISPLAY:table-row" id="trIsBoss">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblMonEndDate" runat="server" ForeColor="Blue" Text="*本月最終營業日"></asp:Label>
                                        </td>
                                        <td class="td_Edit" width="35%" align="left">
                                            <uc:uccalender ID="txtMonEndDate" runat="server" Enabled="True" />
                                        </td>
                                         <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblJulianDate" runat="server" Text="太陽日"></asp:Label>
                                        </td>
                                        <td class="td_Edit" width="35%" align="left">
                                            <asp:TextBox ID="txtJulianDate" CssClass="InputTextStyle_Thin" runat="server" MaxLength="8"></asp:TextBox>
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
