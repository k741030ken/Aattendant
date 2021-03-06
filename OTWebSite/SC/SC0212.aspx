<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SC0212.aspx.vb" Inherits="SC_SC0212" %>

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
<body style="margin-top:5px; margin-left:5px; margin-right:5px; margin-bottom:0">
    <form id="frmContent" runat="server">
        <asp:ScriptManager ID="smBase" runat="server" EnablePartialRendering="true"></asp:ScriptManager>
        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 100%">
            <tr>
                <td align="center">
                    <table width="80%" class="tbl_Edit" cellpadding="1" cellspacing="1">
                        <tr style="height:20px">
                            <td class="td_EditHeader" width="30%" align="center"><asp:Label ID="lblOrganID_H" CssClass="MustInputCaption" runat="server" Text="*部門代碼"></asp:Label>
                            </td>
                            <td class="td_Edit" style="width:70%" align="left"><asp:Label ID="lblOrganID" runat="server" Width="115px" CssClass="InputTextStyle_Thin"></asp:Label> 
                            </td>
                        </tr>
                        <tr style="height:20px">
                            <td class="td_EditHeader" width="30%" align="center"><asp:Label ID="lblOrganName" runat="server" Text="部門名稱" CssClass="MustInputCaption"></asp:Label>
                            </td>
                            <td class="td_Edit" style="width:70%" align="left"><asp:TextBox ID="txtOrganName" CssClass="InputTextStyle_Thin" runat="server" MaxLength="30" Width="300px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr style="height:20px">
                            <td class="td_EditHeader" width="30%" align="center"><asp:Label ID="lblDeptID" runat="server" Text="所屬一級部門"></asp:Label>
                            </td>
                            <td class="td_Edit" style="width:70%" align="left"><asp:DropDownList ID="ddlDeptID" Font-Size="12px" Width="200px" runat="server" Font-Names="細明體"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr style="height:20px">
                            <td class="td_EditHeader" width="30%" align="center"><asp:Label ID="lblUpOrganID" runat="server" Text="上階部門"></asp:Label>
                            </td>
                            <td class="td_Edit" style="width:70%" align="left"><asp:DropDownList ID="ddlUpOrganID" Font-Size="12px" Width="200px" runat="server" Font-Names="細明體"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr style="height:20px">
                            <td class="td_EditHeader" width="30%" align="center"><asp:Label ID="lblRegionID" runat="server" Text="所屬區域中心"></asp:Label>
                            </td>
                            <td class="td_Edit" style="width:70%" align="left"><asp:DropDownList ID="ddlRegionID" Width="200px" runat="server" Font-Size="12px" Font-Names="細明體"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr style="height:20px">
                            <td class="td_EditHeader" width="30%" align="center"><asp:Label ID="lblBranchNo" runat="server" Text="分行代碼"></asp:Label>
                            </td>
                            <td class="td_Edit" style="width:70%" align="left"><asp:TextBox ID="txtBranchNo" CssClass="InputTextStyle_Thin" runat="server" MaxLength="3"></asp:TextBox>
                            </td>
                        </tr>
                        <tr style="height:20px">
                            <td class="td_EditHeader" width="30%" align="center"><asp:Label ID="lblBoss" runat="server" Text="單位主管"></asp:Label>
                            </td>
                            <td class="td_Edit" style="width:70%" align="left"><asp:TextBox ID="txtBoss" CssClass="InputTextStyle_Thin" runat="server" MaxLength="6" Width="60px"></asp:TextBox>&nbsp;<asp:TextBox ID="txtBossName" ReadOnly="true" runat="server" CssClass="InputTextStyle_Readonly"></asp:TextBox>
                            <uc:ButtonQuerySelect ID="ucSelectBoss" runat="server" ButtonText="..." ButtonHint="單位主管選取..." WindowHeight="550" WindowWidth="500" DataControlID="txtBoss" />
                            </td>
                        </tr>
                        <tr style="height:20px">
                            <td class="td_EditHeader" width="30%" align="center"><asp:Label ID="lblBusinessBoss" runat="server" Text="業務主管"></asp:Label>
                            </td>
                            <td class="td_Edit" style="width:70%" align="left"><asp:TextBox ID="txtBusinessBoss" CssClass="InputTextStyle_Thin" runat="server" MaxLength="6" Width="60px"></asp:TextBox>&nbsp;<asp:TextBox ID="txtBusinessBossName" ReadOnly="true" CssClass="InputTextStyle_Readonly" runat="server"></asp:TextBox>
                            <uc:ButtonQuerySelect ID="ucSelectBusinessBoss" runat="server" ButtonText="..." ButtonHint="業務主管選取..." WindowHeight="550" WindowWidth="500" />
                            </td>
                        </tr>
                        <tr style="height:20px">
                            <td class="td_EditHeader" width="30%" align="center"><asp:Label ID="lblControlWindow" runat="server" Text="控管科窗口"></asp:Label>
                            </td>
                            <td class="td_Edit" style="width:70%" align="left"><uc:OneUserSelect ID="ucControlWindow" runat="server" FreezeDeptID="R01000" MustSelect="false" />
                            </td>
                        </tr>
                        <tr style="height:20px">
                            <td class="td_EditHeader" width="30%" align="center"><asp:Label ID="lblAnalyzeWindow" runat="server" Text="審查科窗口"></asp:Label>
                            </td>
                            <td class="td_Edit" style="width:70%" align="left">
                                <asp:UpdatePanel ID="updAnalyzeWindow" runat="server">
                                    <ContentTemplate>
                                        <uc:OneUserSelect ID="ucAnalyzeWindow" runat="server" FreezeDeptID="R01000" MustSelect="false" />
                                    </ContentTemplate>
                                    <Triggers><asp:AsyncPostBackTrigger ControlID="ucAnalyzeWindow" /></Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr style="height:20px">
                            <td class="td_EditHeader" width="30%" align="center"><asp:Label ID="lblCreditWindow" runat="server" Text="徵信科窗口"></asp:Label>
                            </td>
                            <td class="td_Edit" style="width:70%" align="left"><uc:OneUserSelect ID="ucCreditWindow" runat="server" FreezeDeptID="R01000" MustSelect="false" />
                            </td>
                        </tr>
                        <tr style="height:20px">
                            <td class="td_EditHeader" width="30%" align="center"><asp:Label ID="Label2" runat="server" Text="覆審窗口"></asp:Label>
                            </td>
                            <td class="td_Edit" style="width:70%" align="left">
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                    <ContentTemplate>
                                        <uc:OneUserSelect ID="ucCRWindow" runat="server" FreezeDeptID="R01000" MustSelect="false" />
                                    </ContentTemplate>
                                    <Triggers><asp:AsyncPostBackTrigger ControlID="ucCRWindow" /></Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr style="height:20px">
                            <td class="td_EditHeader" width="30%" align="center"><asp:Label ID="Label1" runat="server" Text="業務單位註記"></asp:Label>
                            </td>
                            <td class="td_Edit" style="width:70%" align="left"><asp:CheckBox ID="chkBusinessFlag" runat="server" Checked="true" />
                            </td>
                        </tr>
                        <tr style="height:20px">
                            <td class="td_EditHeader" width="30%" align="center"><asp:Label ID="lblBranchFlag" runat="server" Text="分行註記"></asp:Label>
                            </td>
                            <td class="td_Edit" style="width:70%" align="left"><asp:CheckBox ID="chkBranchFlag" runat="server" Checked="true" />
                            </td>
                        </tr>
                        <tr style="height:20px">
                            <td class="td_EditHeader" width="30%" align="center"><asp:Label ID="lblInValidFlag" runat="server" Text="有效註記"></asp:Label>
                            </td>
                            <td class="td_Edit" style="width:70%" align="left"><asp:CheckBox ID="chkInValidFlag" runat="server" Checked="true" />
                            </td>
                        </tr>
                        <tr style="height:20px">
                            <td class="td_EditHeader" width="30%" align="center"><asp:Label ID="lblUpdateFlag" runat="server" Text="異動註記"></asp:Label>
                            </td>
                            <td class="td_Edit" style="width:70%" align="left"><asp:CheckBox ID="chkUpdateFlag" runat="server" Checked="true" />
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
                                <asp:Label ID="lblLastChgID_H" runat="server" Text="最後異動人員"></asp:Label></td>
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
