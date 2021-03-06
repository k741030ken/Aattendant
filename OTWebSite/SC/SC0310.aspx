<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SC0310.aspx.vb" Inherits="SC_SC0310" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <link type="text/css" rel="stylesheet" href="../StyleSheet.Css" />
    <script type="text/javascript">
    <!--
        function funAction(Param)
        {
            if (Param == 'btnUpdate')
            {
                var sWord = '';
                if (frmContent.txtHolidayOrNot.value == '1')
                    sWord = '非假日'
                else
                    sWord = '假日';
                if (!confirm('確定將' + frmContent.txtSysDate.value + '改為' + sWord + '？'))
                    return false;
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
                            <td align="right" style="width:25%"><asp:Label ID="lblAreaID_S" runat="server" Text="區域：" Font-Size="15px" CssClass="MustInputCaption"></asp:Label>
                            </td>
                            <td align="left" style="width:45%"><asp:DropDownList ID="ddlAreaID" runat="server" Width="115px" AutoPostBack="true">
                            <asp:ListItem Text="TW-台灣" Value="TW" Selected></asp:ListItem>
                            <asp:ListItem Text="HK-香港" Value="HK"></asp:ListItem>
                            <asp:ListItem Text="MO-澳門" Value="MO"></asp:ListItem>
                            <asp:ListItem Text="LA-Los Angeles" Value="LA"></asp:ListItem>
                            </asp:DropDownList>
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
                            <td style="width: 50%">
                                <asp:Calendar ID="Calendar1" runat="server" BackColor="#FFFFCC" BorderColor="#FFCC66" BorderWidth="1px" DayNameFormat="Shortest" Font-Names="Microsoft Sans Serif" Font-Size="15px" ForeColor="#663399" Height="200px" NextMonthText="下月" NextPrevFormat="ShortMonth" PrevMonthText="上月" ShowGridLines="True" Width="329px">
                                    <SelectedDayStyle BackColor="#CCCCFF" Font-Bold="True" />
                                    <TodayDayStyle BackColor="#FFCC66" ForeColor="White" />
                                    <SelectorStyle BackColor="#FFCC66" />
                                    <WeekendDayStyle ForeColor="Red" />
                                    <OtherMonthDayStyle ForeColor="#CC9966" />
                                    <NextPrevStyle Font-Size="9pt" ForeColor="#FFFFCC" />
                                    <DayHeaderStyle BackColor="#FFCC66" Font-Bold="True" Height="1px" />
                                    <TitleStyle BackColor="#990000" Font-Bold="True" Font-Size="15px" ForeColor="#FFFFCC"
                                        Height="25px" />
                                </asp:Calendar>
                            </td>
                            <td style="width: 50%">
                                <table width="100%" class="tbl_Edit" cellpadding="1" cellspacing="1">
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="30%" align="center"><asp:Label ID="lblAreaID_H" ForeColor="Blue" runat="server" Font-Names="新細明體" Text="*區域"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:70%" align="left"><asp:Label ID="lblAreaID" runat="server" Font-Bold="true" Font-Names="新細明體"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="30%" align="center"><asp:Label ID="lblSysDate_H" ForeColor="Blue" runat="server" Font-Names="新細明體" Text="*系統日期"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:70%" align="left"><asp:Label ID="lblSysDate" runat="server" Font-Bold="true" Font-Names="新細明體"></asp:Label><asp:TextBox ID="txtSysDate" runat="server" style="display:none"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="30%" align="center"><asp:Label ID="lblHolidayOrNot_H" runat="server" Text="假日"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:70%" align="left"><asp:Image ID="imgHolidayOrNot" runat="server" ImageUrl="~/images/chkbox.gif" /><asp:TextBox ID="txtHolidayOrNot" runat="server" style="display:none"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="30%" align="center"><asp:Label ID="lblWeek_H" runat="server" Text="星期"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:70%" align="left"><asp:Label ID="lblWeek" runat="server" Font-Names="新細明體"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="30%" align="center"><asp:Label ID="lblNextBusDate_H" runat="server" Text="下一營業日"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:70%" align="left"><asp:Label ID="lblNextBusDate" runat="server" Font-Names="新細明體"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="30%" align="center"><asp:Label ID="lblLastBusDate_H" runat="server" Text="上一營業日"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:70%" align="left"><asp:Label ID="lblLastBusDate" runat="server" Font-Names="新細明體"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="30%" align="center"><asp:Label ID="lblNeNeBusDate_H" runat="server" Text="下下營業日"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:70%" align="left"><asp:Label ID="lblNeNeBusDate" runat="server" Font-Names="新細明體"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="30%" align="center"><asp:Label ID="lblLastEndDate_H" runat="server" Text="上月月底日"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:70%" align="left"><asp:Label ID="lblLastEndDate" runat="server" Font-Names="新細明體"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="30%" align="center"><asp:Label ID="lblThisEndDate_H" runat="server" Text="本月月底日"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:70%" align="left"><asp:Label ID="lblThisEndDate" runat="server" Font-Names="新細明體"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="30%" align="center"><asp:Label ID="lblMonEndDate_H" runat="server" Text="本月最終營業日"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:70%" align="left"><asp:Label ID="lblMonEndDate" runat="server" Font-Names="新細明體"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="30%" align="center"><asp:Label ID="lblNextDateDiff_H" runat="server" Text="下營業日差"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:70%" align="left"><asp:Label ID="lblNextDateDiff" runat="server" Font-Names="新細明體"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="30%" align="center"><asp:Label ID="lblNeNeDateDiff_H" runat="server" Text="下下營業日差"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:70%" align="left"><asp:Label ID="lblNeNeDateDiff" runat="server" Font-Names="新細明體"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="30%" align="center"><asp:Label ID="lblLastDateDiff_H" runat="server" Text="上營業日差"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:70%" align="left"><asp:Label ID="lblLastDateDiff" runat="server" Font-Names="新細明體"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="30%" align="center"><asp:Label ID="lblJulianDate_H" runat="server" Text="太陽日"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:70%" align="left"><asp:Label ID="lblJulianDate" runat="server" Font-Names="新細明體"></asp:Label>
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
