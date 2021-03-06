<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ST1B12.aspx.vb" Inherits="ST_ST1B12" %>

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
        function funAction(Param) {
            //debugger;
            if (Param == 'btnQueryEmp') {
                fromField = document.getElementById("btnQueryEmp");
                fromField.click()
            }            
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
                                            <asp:Label ID="lblEmpID" ForeColor="Blue" runat="server" Text="*員工編號"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left">  
                                            <asp:Label ID="lblEmpID_S" CssClass="InputTextStyle_Thin" runat="server"></asp:Label> 
                                            <asp:Label ID="lblUserName_S" CssClass="InputTextStyle_Thin" runat="server"></asp:Label>                            
                                            <asp:HiddenField ID="hidIDNo" runat="server" />
                                            <asp:HiddenField ID="hidEmpDate" runat="server" />
                                        </td>   
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblCompID" ForeColor="Blue" runat="server" Text="*現任公司"></asp:Label>
                                        </td>    
                                        <td class="td_Edit" style="width:35%" align="left">
                                            <asp:Label ID="lblCompID_S" CssClass="InputTextStyle_Thin" runat="server"></asp:Label> 
                                        </td>
                                    </tr> 
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblDeptID" runat="server" Text="現任部門/科組課"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left">
                                            <asp:Label ID="lblDeptID_S" CssClass="InputTextStyle_Thin" runat="server"></asp:Label>
                                            <asp:Label ID="lblOrganID_S" CssClass="InputTextStyle_Thin" runat="server"></asp:Label>
                                        </td>
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblTitle" runat="server" Text="子公司職稱(現況)"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left">
                                            <asp:Label ID="lblTitle_S" CssClass="InputTextStyle_Thin" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblPosition" runat="server" Text="職位(現況)"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left">
                                            <asp:Label ID="lblPosition_S" CssClass="InputTextStyle_Thin" runat="server"></asp:Label>
                                        </td>
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblWorkType" runat="server" Text="工作性質(現況)"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left">
                                            <asp:Label ID="lblWorkType_S" CssClass="InputTextStyle_Thin" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblHoldingRank" runat="server" Text="金控職等(現況)"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left">
                                            <asp:Label ID="lblHoldingRank_S" CssClass="InputTextStyle_Thin" runat="server"></asp:Label>
                                        </td>
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblHoldingTitle" runat="server" Text="金控職級(現況)"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left">
                                            <asp:Label ID="lblHoldingTitle_S" CssClass="InputTextStyle_Thin" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblAdditionDeptBoss" runat="server" Text="兼任部門主管(現況)"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left">
                                            <asp:CheckBox ID="chkAdditionDeptBoss" runat="server" Enabled="false" />
                                        </td>
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblAdditionOrganBoss" runat="server" Text="兼任科組課主管(現況)"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left">
                                            <asp:CheckBox ID="chkAdditionOrganBoss" runat="server" Enabled="false" />
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblModifyDate" ForeColor="Blue" runat="server" Text="*生效日"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left">
                                            <asp:Label ID="lblValidDate_S" CssClass="InputTextStyle_Thin" runat="server"></asp:Label>                                         
                                        </td>                    
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblEmpAdditionReason" ForeColor="Blue" runat="server" Text="*狀態"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left">
                                            <asp:Label ID="lblReason_S" CssClass="InputTextStyle_Thin" runat="server"></asp:Label> 
                                        </td>
                                    </tr>       
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblFileNO" ForeColor="Blue" runat="server" Text="*人令"></asp:Label>
                                        </td>
                                        <td class="td_Edit" width="85%" align="left" colspan="3">
                                            <asp:Label ID="lblFileNO_S" CssClass="InputTextStyle_Thin" runat="server"></asp:Label> 
                                        </td>    
                                    </tr>                                                
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblEmpAdditionCompID" ForeColor="Blue" runat="server" Text="*兼任公司"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left">    
                                            <asp:Label ID="lblAdditionCompID_S" CssClass="InputTextStyle_Thin" runat="server"></asp:Label>          
                                        </td>
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblEmpAdditionDeptID" ForeColor="Blue" runat="server" Text="*兼任部門/科組課"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left">
                                            <asp:Label ID="lblAdditionDeptID_S" CssClass="InputTextStyle_Thin" runat="server"></asp:Label> 
                                            <asp:Label ID="lblAdditionOrganID_S" CssClass="InputTextStyle_Thin" runat="server"></asp:Label> 
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblEmpAdditionFlowOrganID" runat="server" Text="兼任簽核最小單位"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left">
                                            <asp:Label ID="lblAdditionFlowOrganID_S" CssClass="InputTextStyle_Thin" runat="server"></asp:Label> 
                                        </td>
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblEmpAdditionBossType" runat="server" Text="主管任用方式"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left">
                                            <asp:RadioButton ID="rbnEmpAdditionBossType1" GroupName="rbnEmpAdditionBossType" runat="server" Text="主要" Enabled="false"/>&nbsp;&nbsp;&nbsp;
                                            <asp:RadioButton ID="rbnEmpAdditionBossType2" GroupName="rbnEmpAdditionBossType" runat="server" Text="兼任" Enabled="false" />                                                               
                                        </td>
                                    </tr>
                                    <tr style="height:20px;DISPLAY:table-row" id="trBossType">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblEmpAdditionOrganBoss" runat="server" Text="單位"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left">
                                            <asp:CheckBox ID="chkEmpAdditionIsBoss" Text="主管" runat="server" Enabled="false" />&nbsp;&nbsp;&nbsp;
                                            <asp:CheckBox ID="chkEmpAdditionIsSecBoss" Text="副主管" runat="server" Enabled="false" />
                                        </td>
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblEmpAdditionOrganFlowBoss" runat="server" Text="簽核單位"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left">
                                            <asp:CheckBox ID="chkEmpAdditionIsGroupBoss" Text="主管" runat="server" Enabled="false" />&nbsp;&nbsp;&nbsp;
                                            <asp:CheckBox ID="chkEmpAdditionIsSecGroupBoss" Text="副主管" runat="server" Enabled="false" />
                                        </td>
                                    </tr>
                                    <tr style="height:20px;DISPLAY:table-row" id="trIsBoss">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblEmpAdditionRemark" runat="server" Text="備註"></asp:Label>
                                        </td>
                                        <td class="td_Edit" width="85%" align="left" colspan="3">
                                            <asp:TextBox ID="txtEmpAdditionRemark" CssClass="InputTextStyle_Thin" runat="server" 
                                                MaxLength="500" Width="500px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblCreateDate" runat="server" Text="建檔日期"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left">
                                            <asp:Label ID="lblCreateDate_S" CssClass="InputTextStyle_Thin" runat="server"></asp:Label>
                                        </td>
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblCreateID" runat="server" Text="建檔人員"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left">
                                            <asp:Label ID="lblCreateID_S" CssClass="InputTextStyle_Thin" runat="server"></asp:Label>
                                        </td>
                                    </tr>                               
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblLastChgDate" runat="server" Text="最後異動日期"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left">
                                            <asp:Label ID="lblLastChgDate_S" CssClass="InputTextStyle_Thin" runat="server"></asp:Label>
                                        </td>
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblLastChgID" runat="server" Text="最後異動人員"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left">
                                            <asp:Label ID="lblLastChgID_S" CssClass="InputTextStyle_Thin" runat="server"></asp:Label>
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
