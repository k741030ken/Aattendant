<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SC0101.aspx.vb" Inherits="SC_SC0101" %>

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
<body style="margin-top:5px; margin-left:5px; margin-right:5px; margin-bottom:0" >
    <form id="frmContent" runat="server">
        <ajaxToolkit:ToolkitScriptManager runat="Server" EnableScriptGlobalization="true"
            EnableScriptLocalization="true" ID="ScriptManager1" />
        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 100%">
            <tr>
                <td align="center">
                    <table width="80%" class="tbl_Edit" cellpadding="1" cellspacing="1">
                        <tr style="height:20px">
                            <td class="td_EditHeader" width="30%" align="center"><asp:Label ID="lblUserID" ForeColor="Blue" runat="server" Text="*員工編號"></asp:Label>
                            </td>
                            <td class="td_Edit" style="width:70%" align="left"><asp:TextBox ID="txtUserID" CssClass="InputTextStyle_Thin" style="text-transform: uppercase" runat="server" MaxLength="6"></asp:TextBox>
                            </td>
                        </tr>
                        <tr style="height:20px">
                            <td class="td_EditHeader" width="30%" align="center"><asp:Label ID="lblUserName" ForeColor="blue" runat="server" Text="員工姓名"></asp:Label>
                            </td>
                            <td class="td_Edit" style="width:70%" align="left"><asp:TextBox ID="txtUserName" CssClass="InputTextStyle_Thin" runat="server" MaxLength="42" Width="300px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr style="height:20px">
                            <td class="td_EditHeader" width="30%" align="center"><asp:Label ID="lblEngName" runat="server" Text="員工英文姓名"></asp:Label>
                            </td>
                            <td class="td_Edit" style="width:70%" align="left"><asp:TextBox ID="txtEngName" CssClass="InputTextStyle_Thin" runat="server" MaxLength="42" Width="300px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr style="height:20px">
                            <td class="td_EditHeader" width="30%" align="center"><asp:Label ID="lblCompID" runat="server" Text="公司代碼"></asp:Label>
                            </td>
                            <td class="td_Edit" style="width:70%" align="left"><asp:TextBox ID="txtCompID" CssClass="InputTextStyle_Thin" runat="server" MaxLength="6"></asp:TextBox>
                            </td>
                        </tr>
                        <tr style="height:20px">
                            <td class="td_EditHeader" width="30%" align="center"><asp:Label ID="lblDeptID" ForeColor="blue" runat="server" Text="部門/科組課"></asp:Label>
                            </td>
                            <td class="td_Edit" style="width:70%" align="left">
                                <asp:UpdatePanel ID="UdpDeptID" runat="server">
                                    <ContentTemplate>
                                        <uc:SelectOrgan ID="ucSelectOrgan" runat="server" />
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="ucSelectOrgan" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr style="height:20px">
                            <td class="td_EditHeader" width="30%" align="center"><asp:Label ID="lblBusinessFlag" runat="server" Text="業務人員註記"></asp:Label>
                            </td>
                            <td class="td_Edit" style="width:70%" align="left"><asp:CheckBox ID="chkBusinessFlag" runat="server" Text="" />
                            </td>
                        </tr>
                        <tr style="height:20px">
                            <td class="td_EditHeader" width="30%" align="center"><asp:Label ID="lblBusinessID" runat="server" Text="業務單位"></asp:Label>
                            </td>
                            <td class="td_Edit" style="width:70%" align="left"><uc:SelectOrgan ID="ucSelectBusiness" ShowOrgan="false" DeptType="Bussiness" MustSelect="false" runat="server" />
                            </td>
                        </tr>
                        <tr style="height:20px">
                            <td class="td_EditHeader" width="30%" align="center"><asp:Label ID="lblWorkTypeID" runat="server" Text="工作性質"></asp:Label>
                            </td>
                            <td class="td_Edit" style="width:70%" align="left"><asp:TextBox ID="txtWorkTypeID" CssClass="InputTextStyle_Thin" runat="server" MaxLength="6"></asp:TextBox>
                            </td>
                        </tr>
                        <tr style="height:20px">
                            <td class="td_EditHeader" width="30%" align="center"><asp:Label ID="lblRankID" runat="server" Text="職等"></asp:Label>
                            </td>
                            <td class="td_Edit" style="width:70%" align="left"><asp:TextBox ID="txtRankID" CssClass="InputTextStyle_Thin" runat="server" MaxLength="2"></asp:TextBox>
                            </td>
                        </tr>
                        <tr style="height:20px">
                            <td class="td_EditHeader" width="30%" align="center"><asp:Label ID="lblPasswordErrorCount" runat="server" Text="密碼錯誤次數"></asp:Label>
                            </td>
                            <td class="td_Edit" style="width:70%" align="left"><asp:TextBox ID="txtPasswordErrorCount" CssClass="InputTextStyle_Thin" runat="server" MaxLength="1"></asp:TextBox>
                            </td>
                        </tr>
                        <tr style="height:20px">
                            <td class="td_EditHeader" width="30%" align="center"><asp:Label ID="lblExpireDate" ForeColor="blue" runat="server" Text="使用期限"></asp:Label>
                            </td>
                            <td class="td_Edit" style="width:70%" align="left"><asp:TextBox ID="txtExpireDate" CssClass="InputTextStyle_Thin" runat="server" MaxLength="10"></asp:TextBox><!--<uc:Date ID="ucExpireDate" runat="server" DateControlName="txtExpireDate" />-->
                                <asp:ImageButton runat="Server" ID="imgExpireDate" ImageUrl="~/images/Calendar.gif" AlternateText="Click to show calendar" />
                                <ajaxToolkit:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtExpireDate" PopupButtonID="imgExpireDate" Format="yyyy/MM/dd" />
                            </td>
                        </tr>
                        <tr style="height:20px">
                            <td class="td_EditHeader" width="30%" align="center"><asp:Label ID="lblEMail" runat="server" Text="E-Mail"></asp:Label>
                            </td>
                            <td class="td_Edit" style="width:70%" align="left"><asp:TextBox ID="txtEMail" CssClass="InputTextStyle_Thin" runat="server" MaxLength="60" Width="300px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr style="height:20px">
                            <td class="td_EditHeader" width="30%" align="center"><asp:Label ID="lblBanMark" runat="server" Text="禁用註記"></asp:Label>
                            </td>
                            <td class="td_Edit" style="width:70%" align="left"><asp:CheckBox ID="chkBanMark" runat="server" />
                            </td>
                        </tr>
                        <tr style="height:20px">
                            <td class="td_EditHeader" width="30%" align="center"><asp:Label ID="lblUpdateFlag" runat="server" Text="異動註記"></asp:Label>
                            </td>
                            <td class="td_Edit" style="width:70%" align="left"><asp:CheckBox ID="chkUpdateFlag" runat="server" Checked="true" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>

        </table>
        </form>
</body>
</html>
