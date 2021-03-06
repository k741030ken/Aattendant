<%@ Page Language="VB" AutoEventWireup="false" CodeFile="PA1202.aspx.vb" Inherits="PA_PA1202" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link type="text/css" rel="stylesheet" href="../StyleSheet.Css" />
    <script type="text/javascript">
    <!--

    -->
    </script>
</head>
<body style="margin-top: 5px; margin-left: 5px; margin-right: 5px; margin-bottom: 0">
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
                                <tr style="height: 20px">
                                    <td class="td_EditHeader" width="15%" align="center">
                                        <asp:Label ID="lblCompID" ForeColor="Blue" runat="server" Text="*公司代碼"></asp:Label>
                                    </td>
                                    <td class="td_Edit" width="35%" align="left" colspan="3">
                                        <asp:Label ID="lblCompIDtxt" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr style="height: 20px">
                                    <td class="td_EditHeader" width="15%" align="center">
                                        <asp:Label ID="lblCompName" ForeColor="Blue" runat="server" Text="*公司名稱"></asp:Label>
                                    </td>
                                    <td class="td_Edit" width="35%" align="left">
                                        <asp:TextBox ID="txtCompName" CssClass="InputTextStyle_Thin" runat="server" MaxLength="20"
                                            Width="300px"></asp:TextBox>
                                    </td>
                                    <td class="td_EditHeader" width="15%" align="center">
                                        <asp:Label ID="lblCompNameCN" runat="server" Text="公司名稱(簡體)"></asp:Label>
                                    </td>
                                    <td class="td_Edit" width="35%" align="left">
                                        <asp:TextBox ID="txtCompNameCN" CssClass="InputTextStyle_Thin" runat="server" MaxLength="20"
                                            Width="300px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr style="height: 20px">
                                    <td class="td_EditHeader" width="15%" align="center">
                                        <asp:Label ID="lblCompChnName" runat="server" Text="中文名稱"></asp:Label>
                                    </td>
                                    <td class="td_Edit" width="35%" align="left">
                                        <asp:TextBox ID="txtCompChnName" CssClass="InputTextStyle_Thin" runat="server" MaxLength="50"
                                            TextMode="MultiLine" Width="300px" Height="60px"></asp:TextBox>
                                    </td>
                                    <td class="td_EditHeader" width="15%" align="center">
                                        <asp:Label ID="lblCompChnNameCN" runat="server" Text="中文名稱(簡體)"></asp:Label>
                                    </td>
                                    <td class="td_Edit" width="35%" align="left">
                                        <asp:TextBox ID="txtCompChnNameCN" CssClass="InputTextStyle_Thin" runat="server"
                                            MaxLength="50" TextMode="MultiLine" Width="300px" Height="60px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr style="height: 20px">
                                    <td class="td_EditHeader" width="15%" align="center">
                                        <asp:Label ID="lblAddress" runat="server" Text="中文地址"></asp:Label>
                                    </td>
                                    <td class="td_Edit" width="35%" align="left">
                                        <asp:TextBox ID="txtAddress" CssClass="InputTextStyle_Thin" runat="server" MaxLength="80"
                                            TextMode="MultiLine" Width="300px" Height="60px"></asp:TextBox>
                                    </td>
                                    <td class="td_EditHeader" width="15%" align="center">
                                        <asp:Label ID="lblAddressCN" runat="server" Text="中文地址(簡體)"></asp:Label>
                                    </td>
                                    <td class="td_Edit" width="35%" align="left">
                                        <asp:TextBox ID="txtAddressCN" CssClass="InputTextStyle_Thin" runat="server" MaxLength="80"
                                            TextMode="MultiLine" Width="300px" Height="60px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr style="height: 20px">
                                    <td class="td_EditHeader" width="15%" align="center">
                                        <asp:Label ID="lblCompEngName" runat="server" Text="英文名稱"></asp:Label>
                                    </td>
                                    <td class="td_Edit" width="35%" align="left">
                                        <asp:TextBox ID="txtCompEngName" CssClass="InputTextStyle_Thin" runat="server" MaxLength="50"
                                            TextMode="MultiLine" Width="300px" Height="75px"></asp:TextBox>
                                    </td>
                                    <td class="td_EditHeader" width="15%" align="center">
                                        <asp:Label ID="lblEngAddress" runat="server" Text="英文地址"></asp:Label>
                                    </td>
                                    <td class="td_Edit" width="35%" align="left">
                                        <asp:TextBox ID="txtEngAddress" CssClass="InputTextStyle_Thin" runat="server" MaxLength="80"
                                            TextMode="MultiLine" Width="300px" Height="75px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr style="height: 20px">
                                    <td class="td_EditHeader" width="15%" align="center">
                                        <asp:Label ID="lblInValidFlag" runat="server" Text="無效註記"></asp:Label>
                                    </td>
                                    <td class="td_Edit" width="35%" align="left">
                                        <asp:CheckBox ID="chkInValidFlag" runat="server" />
                                    </td>
                                    <td class="td_EditHeader" width="15%" align="center">
                                        <asp:Label ID="lblNotShowFlag" runat="server" Text="不顯示註記"></asp:Label>
                                    </td>
                                    <td class="td_Edit" width="35%" align="left">
                                        <asp:CheckBox ID="chkNotShowFlag" runat="server" />
                                    </td>
                                </tr>
                                <tr style="height: 20px">
                                    <td class="td_EditHeader" width="15%" align="center">
                                        <asp:Label ID="lblFeeShareFlag" runat="server" Text="費用分攤註記"></asp:Label>
                                    </td>
                                    <td class="td_Edit" width="35%" align="left">
                                        <asp:CheckBox ID="chkFeeShareFlag" runat="server" />
                                    </td>
                                    <td class="td_EditHeader" width="15%" align="center">
                                        <asp:Label ID="lblSPHSC1GrpFlag" runat="server" Text="證券團保公司註記"></asp:Label>
                                    </td>
                                    <td class="td_Edit" width="35%" align="left">
                                        <asp:CheckBox ID="chkSPHSC1GrpFlag" runat="server" />
                                    </td>
                                </tr>
                                <tr style="height: 20px">
                                    <td class="td_EditHeader" width="15%" align="center">
                                        <asp:Label ID="lblRankIDMapFlag" runat="server" Text="導入惠悅"></asp:Label>
                                    </td>
                                    <td class="td_Edit" width="35%" align="left">
                                        <asp:CheckBox ID="chkRankIDMapFlag" AutoPostBack="true" runat="server" />
                                    </td>
                                    <td class="td_EditHeader" width="15%" align="center">
                                        <asp:Label ID="lblRankIDMapValidDate" runat="server" Text="導入惠悅生效日"></asp:Label>
                                    </td>
                                    <td class="td_Edit" width="35%" align="left">
                                        <uc:uccalender ID="txtRankIDMapValidDate" runat="server" Enabled="false" />
                                    </td>
                                </tr>
                                <tr style="height: 20px">
                                    <td class="td_EditHeader" width="15%" align="center">
                                        <asp:Label ID="lblNotShowRankID" runat="server" Text="不顯示職等"></asp:Label>
                                    </td>
                                    <td class="td_Edit" width="35%" align="left">
                                        <asp:CheckBox ID="chkNotShowRankID" runat="server" />
                                    </td>
                                    <td class="td_EditHeader" width="15%" align="center">
                                        <asp:Label ID="lblNotShowWorkType" runat="server" Text="不顯示工作性質"></asp:Label>
                                    </td>
                                    <td class="td_Edit" width="35%" align="left">
                                        <asp:CheckBox ID="chkNotShowWorkType" runat="server" />
                                    </td>
                                </tr>
                                <tr style="height: 20px">
                                    <td class="td_EditHeader" width="15%" align="center">
                                        <asp:Label ID="lblHRISFlag" runat="server" Text="資料轉入HRISDB"></asp:Label>
                                    </td>
                                    <td class="td_Edit" width="35%" align="left">
                                        <asp:CheckBox ID="chkHRISFlag" runat="server" />
                                    </td>
                                    <td class="td_EditHeader" width="15%" align="center">
                                        <asp:Label ID="lblCNFlag" runat="server" Text="簡體註記"></asp:Label>
                                    </td>
                                    <td class="td_Edit" width="35%" align="left">
                                        <asp:CheckBox ID="chkCNFlag2" runat="server" />
                                    </td>
                                </tr>
                                <tr style="height: 20px">
                                    <td class="td_EditHeader" width="15%" align="center">
                                        <asp:Label ID="lblPayroll" runat="server" Text="計薪作業歸屬體系"></asp:Label>
                                    </td>
                                    <td class="td_Edit" width="35%" align="left">
                                        <asp:DropDownList ID="ddlPayroll" runat="server" Font-Names="細明體">
                                            <asp:ListItem Value="" Text="---請選擇---" Selected="true"></asp:ListItem>
                                            <asp:ListItem Value="SPHBK1" Text="SPHBK1-永豐銀行"></asp:ListItem>
                                            <asp:ListItem Value="SPHSC1" Text="SPHSC1-永豐金證券"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td class="td_EditHeader" width="15%" align="center">
                                        <asp:Label ID="lblCalendar" runat="server" Text="年曆檔歸屬體系"></asp:Label>
                                    </td>
                                    <td class="td_Edit" width="35%" align="left">
                                        <asp:DropDownList ID="ddlCalendar" runat="server" Font-Names="細明體">
                                            <asp:ListItem Value="" Text="---請選擇---" Selected="true"></asp:ListItem>
                                            <asp:ListItem Value="SPHBK1" Text="SPHBK1-永豐銀行"></asp:ListItem>
                                            <asp:ListItem Value="SPHSC1" Text="SPHSC1-永豐金證券"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr style="height: 20px">
                                    <td class="td_EditHeader" width="15%" align="center">
                                        <asp:Label ID="lblCheckInFile" runat="server" Text="報到文件歸屬體系"></asp:Label>
                                    </td>
                                    <td class="td_Edit" width="35%" align="left">
                                        <asp:DropDownList ID="ddlCheckInFile" runat="server" Font-Names="細明體">
                                            <asp:ListItem Value="" Text="---請選擇---" Selected="true"></asp:ListItem>
                                            <asp:ListItem Value="SPHBK1" Text="SPHBK1-永豐銀行"></asp:ListItem>
                                            <asp:ListItem Value="SPHSC1" Text="SPHSC1-永豐金證券"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td class="td_EditHeader" width="15%" align="center">
                                        <asp:Label ID="lblProbation" runat="server" Text="試用考核歸屬公司體系"></asp:Label>
                                    </td>
                                    <td class="td_Edit" width="35%" align="left">
                                        <asp:DropDownList ID="ddlProbation" runat="server" Font-Names="細明體"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr style="height: 20px">
                                    <td class="td_EditHeader" width="15%" align="center">
                                        <asp:Label ID="lblPayrollMaintain" runat="server" Text="可維護計薪作業公司代碼"></asp:Label>
                                    </td>
                                    <td class="td_Edit" width="35%" align="left">
                                        <asp:DropDownList ID="ddlPayrollMaintain" runat="server" Font-Names="細明體">
                                        </asp:DropDownList>
                                    </td>
                                    <td class="td_EditHeader" width="15%" align="center">
                                        <asp:Label ID="lblEmpSource" runat="server" Text="員工資料來源"></asp:Label>
                                    </td>
                                    <td class="td_Edit" width="35%" align="left">
                                        <asp:DropDownList ID="ddlEmpSource" runat="server" Font-Names="細明體">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr style="height: 20px">
                                    <td class="td_EditHeader" width="15%" align="center">
                                        <asp:Label ID="lblLastChgComp1" runat="server" Text="最後異動公司"></asp:Label>
                                    </td>
                                    <td class="td_Edit" width="35%" align="left">
                                        <asp:Label ID="lblLastChgComp" runat="server"></asp:Label>
                                    </td>
                                    <td class="td_EditHeader" width="15%" align="center">
                                        <asp:Label ID="lblLastChgID1" runat="server" Text="最後異動人員"></asp:Label>
                                    </td>
                                    <td class="td_Edit" width="35%" align="left">
                                        <asp:Label ID="lblLastChgID" runat="server"> </asp:Label>
                                    </td>
                                </tr>
                                <tr style="height: 20px">
                                    <td class="td_EditHeader" width="15%" align="center">
                                        <asp:Label ID="lblLastChgDate1" runat="server" Text="最後異動日期"></asp:Label>
                                    </td>
                                    <td class="td_Edit" width="35%" align="left">
                                        <asp:Label ID="lblLastChgDate" runat="server"></asp:Label>
                                    </td>
                                    <td class="td_EditHeader" width="15%" align="center"></td>
                                    <td class="td_Edit" width="35%" align="left"></td>
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
