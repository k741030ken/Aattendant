<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ST1802.aspx.vb" Inherits="ST_ST1802" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <link type="text/css" rel="stylesheet" href="../StyleSheet.Css" />
    <script type="text/javascript" src="../ClientFun/jquery-1.8.3.min.js"></script>
    <script type="text/javascript">
    <!--
        $(function () {
            $("#ucValidDate_txtDate").change(function () {
                $("#btnEmployeeLog").click()
            });
        });
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
                                            <asp:Label ID="lbl1" runat="server" Text="公司代碼"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left" colspan="3">
                                            <asp:Label ID="txtCompID" runat="server" ></asp:Label>
                                            <asp:Button ID="btnEmployeeLog" runat="server" style="display:none" />
                                            <uc:Release ID="ucRelease" runat="server" WindowHeight="350" WindowWidth="350" style="display:none" />
                                        </td>
                                    </tr> 
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblEmpID" runat="server" Text="員工編號"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left">
                                            <asp:Label ID="txtEmpID" runat="server" ></asp:Label>
                                        </td>
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblEmpName" runat="server" Text="員工姓名"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left">
                                            <asp:Label ID="txtEmpName" runat="server" ></asp:Label>
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblReason" ForeColor="Blue" runat="server" Text="*異動原因"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left">
                                            <asp:DropDownList ID="ddlReason" runat="server" AutoPostBack="True" Font-Names="細明體"></asp:DropDownList>
                                            <asp:HiddenField ID="hidReason" runat="server"></asp:HiddenField>
                                        </td>
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblSeq" ForeColor="Blue" runat="server" Text="*序號"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left">
                                            <asp:TextBox ID="txtSeq" CssClass="InputTextStyle_Thin" runat="server" MaxLength="1" ></asp:TextBox>
                                            <asp:HiddenField ID="hidSeq" runat="server"></asp:HiddenField>
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblModifyDate" runat="server" ForeColor="Blue" Text="*生效日期"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left">
                                            <uc:uccalender ID="ucValidDate" runat="server" Enabled="True"  />
                                            <asp:HiddenField ID="hidEmpDate" runat="server"></asp:HiddenField>
                                            <asp:HiddenField ID="hidModifyDate" runat="server"></asp:HiddenField>
                                        </td>
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblDueDate" runat="server" Text="生效迄日"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left">
                                            <uc:uccalender ID="txtDueDate" runat="server" Enabled="True" />
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblRemark" runat="server" Text="備註"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left" colspan="3">
                                            <asp:TextBox ID="txtRemark" CssClass="InputTextStyle_Thin" runat="server" MaxLength="100" TextMode="MultiLine" Width="650px" Height="50px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr style="height:20px; background-color:#FFFFFF">
                                        <td width="15%" align="left" colspan="4">
                                            <asp:Label ID="lblBeforeInfo" runat="server" Text="異動前資料"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblCompIDOld" runat="server" ForeColor="Blue" Text="*公司名稱"></asp:Label>
                                            <asp:HiddenField ID="hidBeforeDueDateOld" runat="server"></asp:HiddenField>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left">
                                            <asp:DropDownList ID="ddlCompIDOld" runat="server" Font-Names="細明體" AutoPostBack="true"></asp:DropDownList>
                                        </td>
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblDeptIDOrganIDOld" runat="server" ForeColor="Blue" Text="*部門/科組課"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left">
                                            <uc:SelectHROrgan ID="ucSelectHROrganOld" runat="server" />
                                        </td>
                                    </tr> 
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblPositionIDOld" runat="server" Text="職位"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left">
                                            <asp:DropDownList ID="ddlPositionIDOld" runat="server" AutoPostBack="True" Font-Names="細明體"></asp:DropDownList>
                                            <asp:Label ID="lblSelectPositionOld" runat="server" ForeColor="Blue" Text="" style="display:none"></asp:Label>
                                            <asp:Label ID="lblSelectPositionNameOld" runat="server" ForeColor="Blue" Text="" style="display:none"></asp:Label>
                                            <asp:HiddenField ID="hidPositionIDOld" runat="server" />
                                            <uc:ButtonPosition ID="ucPositionIDOld" runat="server" ButtonText="..." ButtonHint="選取" WindowHeight="550" WindowWidth="1000" />
                                        </td>
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblWorkTypeIDOld" runat="server" Text="工作性質"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left">
                                            <asp:DropDownList ID="ddlWorkTypeIDOld" runat="server" AutoPostBack="true" Font-Names="細明體"></asp:DropDownList>
                                            <asp:Label ID="lblSelectWorkTypeOld" runat="server" ForeColor="Blue" Text="" style="display:none"></asp:Label>
                                            <asp:Label ID="lblSelectWorkTypeNameOld" runat="server" ForeColor="Blue" Text="" style="display:none"></asp:Label>
                                            <asp:HiddenField ID="hidWorkTypeIDOld" runat="server" />
                                            <uc:ButtonWorkType ID="ucSelectWorkTypeOld" runat="server" ButtonText="..." ButtonHint="選取" WindowHeight="550" WindowWidth="1000" />
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblRankIDOld" runat="server" Text="職等/職稱"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left" colspan="3">
                                            <uc:ucSelectRankAndTitle ID="ucSelectRankAndTitleOld" runat="server" />
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblFlowOrganIDOld" runat="server" ForeColor="Blue" Text="*簽核最小單位"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left">
                                            <uc:ucSelectFlowOrgan ID="ucSelectFlowOrganOld" runat="server" Enabled="True" AutoPostBack="true" />
                                        </td>
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblWorkStatusOld" runat="server" ForeColor="Blue" Text="*任職狀況"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left">
                                            <asp:DropDownList ID="ddlWorkStatusOld" runat="server" Font-Names="細明體"></asp:DropDownList>
                                            <asp:HiddenField ID="hidWorkStatusNameOld" runat="server"></asp:HiddenField>
                                        </td>
                                    </tr>
                                    <tr style="height:20px; background-color:#FFFFFF">
                                        <td width="15%" align="left" colspan="4">
                                            <asp:Label ID="lblAfterInfo" runat="server" Text="異動後資料"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblCompID" runat="server" ForeColor="Blue" Text="*公司名稱"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left">
                                            <asp:DropDownList ID="ddlCompID" runat="server" Font-Names="細明體" AutoPostBack="true"></asp:DropDownList>
                                        </td>
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="Label5" runat="server" ForeColor="Blue" Text="*部門/科組課"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left">                                            
                                            <uc:SelectHROrgan ID="ucSelectHROrgan" runat="server" />
                                        </td>
                                    </tr> 
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblPositionID" runat="server" Text="職位"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left">
                                            <asp:DropDownList ID="ddlPositionID" runat="server" AutoPostBack="true" Font-Names="細明體"></asp:DropDownList>
                                            <asp:Label ID="lblSelectPosition" runat="server" ForeColor="Blue" Text="" style="display:none"></asp:Label>
                                            <asp:Label ID="lblSelectPositionName" runat="server" ForeColor="Blue" Text="" style="display:none"></asp:Label>
                                            <asp:HiddenField ID="hidPositionID" runat="server" />
                                            <uc:ButtonPosition ID="ucPositionID" runat="server" ButtonText="..." ButtonHint="選取" WindowHeight="550" WindowWidth="1000" />
                                        </td>
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblWorkTypeID" runat="server" Text="工作性質"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left">
                                            <asp:DropDownList ID="ddlWorkTypeID" runat="server" AutoPostBack="true" Font-Names="細明體"></asp:DropDownList>
                                            <asp:Label ID="lblSelectWorkType" runat="server" ForeColor="Blue" Text="" style="display:none"></asp:Label>
                                            <asp:Label ID="lblSelectWorkTypeName" runat="server" ForeColor="Blue" Text="" style="display:none"></asp:Label>
                                            <asp:HiddenField ID="hidWorkTypeID" runat="server" />
                                            <uc:ButtonWorkType ID="ucSelectWorkType" runat="server" ButtonText="..." ButtonHint="選取" WindowHeight="550" WindowWidth="1000" />
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblRankID" runat="server" Text="職等/職稱"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left" colspan="3">
                                            <uc:ucSelectRankAndTitle ID="ucSelectRankAndTitle" runat="server" />
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblFlowOrganID" runat="server" ForeColor="Blue" Text="*簽核最小單位"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left">
                                            <uc:ucSelectFlowOrgan ID="ucSelectFlowOrgan" runat="server" Enabled="True" AutoPostBack="true" />
                                        </td>
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblWorkStatus" runat="server" Text="任職狀況"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left">
                                            <asp:Label ID="txtWorkStatus" runat="server" ></asp:Label>
                                            <asp:HiddenField ID="hidWorkStatus" runat="server"></asp:HiddenField>
                                            <asp:HiddenField ID="hidWorkStatusName" runat="server"></asp:HiddenField>
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblBossType" runat="server" Text="主管任用方式"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left" colspan="3">
                                            <asp:DropDownList ID="ddlBossType" runat="server" Font-Names="細明體">
                                                <asp:ListItem Value="" Text="---請選擇---" Selected="true"></asp:ListItem>
                                                <asp:ListItem Value="1" Text="1-主要主管"></asp:ListItem>
                                                <asp:ListItem Value="2" Text="2-兼任主管"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblBoss" runat="server" Text="單位"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left">
                                            <asp:CheckBox ID="chkIsBoss" runat="server" Text="主管" />
                                            <asp:CheckBox ID="chkIsSecBoss" runat="server" Text="副主管" />
                                        </td>
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblGroupBoss" runat="server" Text="簽核單位"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left">
                                            <asp:CheckBox ID="chkIsGroupBoss" runat="server" Text="主管" />
                                            <asp:CheckBox ID="chkIsSecGroupBoss" runat="server" Text="副主管" />
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblLastChgComp1" runat="server" Text="最後異動公司"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left" colspan="3">
                                            <asp:Label ID="lblLastChgComp" runat="server" ></asp:Label>
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblLastChgID1" runat="server" Text="最後異動人員"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left" colspan="3">
                                            <asp:Label ID="lblLastChgID" runat="server" > </asp:Label>
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblLastChgDate1" runat="server" Text="最後異動日期"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left" colspan="3">
                                            <asp:Label ID="lblLastChgDate" runat="server" ></asp:Label>
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
