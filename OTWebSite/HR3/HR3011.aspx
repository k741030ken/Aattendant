<%@ Page Language="VB" AutoEventWireup="false" CodeFile="HR3011.aspx.vb" Inherits="HR_HR3011" %>

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
                                <table width="80%">
                                    <tr>
                                        <td align="left">
                                            <asp:Label ID="lblHeadMsg" runat="server"></asp:Label>
                                            <asp:HiddenField ID="hidCompID" runat="server" />
                                            <asp:HiddenField ID="hidEmpID" runat="server" />
                                            <asp:HiddenField ID="hidIDNo" runat="server" />                                          
                                            <asp:HiddenField ID="hidWorkTypeIDOld" runat="server" />
                                            <asp:HiddenField ID="hidPositionIDOld" runat="server" />
                                            <asp:HiddenField ID="hidWorkTypeIDNew" runat="server" />
                                            <asp:HiddenField ID="hidPositionIDNew" runat="server" />
                                            <asp:HiddenField ID="hidReason" runat="server" />
                                            <asp:HiddenField ID="hidModifyDate" runat="server" />
                                            <asp:HiddenField ID="hidValidDate" runat="server" />
                                            <asp:HiddenField ID="hidSeq" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <table width="80%" class="tbl_Edit" cellpadding="1" cellspacing="1">
                                    <tr style="height:20px">
                                        <td width="50%" class="td_EditHeader" align="center" colspan="3">
                                            已生效企業團經歷
                                        </td>
                                        <td width="50%" class="td_EditHeader" align="center">
                                            已生效企業團經歷-待異動
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="1%" align="left" rowspan="8">
                                            異動前
                                        </td>
                                        <td class="td_EditHeader" width="14%" align="left">
                                            <asp:Label ID="lblCompIDOld" runat="server" ForeColor="Blue" Text="*公司名稱"></asp:Label>
                                        </td>
                                        <td class="td_EditHeader" width="45%" align="left">
                                            <asp:Label ID="lblCompIDOldDate" runat="server"></asp:Label>
                                        </td>
                                        <td class="td_Edit" width="40%" align="left">
                                            <asp:DropDownList ID="ddlCompIDOld" runat="server"></asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="14%" align="left">
                                            <asp:Label ID="lblGroupIDOld" runat="server" Text="事業群"></asp:Label>
                                        </td>
                                        <td class="td_EditHeader" width="45%" align="left">
                                            <asp:Label ID="lblGroupIDOldData" runat="server"></asp:Label>
                                        </td>
                                        <td class="td_Edit" width="40%" align="left">
                                            <asp:UpdatePanel ID="UpdGroupIDOld" runat="server" ChildrenAsTriggers="False" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:Label ID="lblGroupIDOldShow" runat="server"></asp:Label>
                                            </ContentTemplate>                                                          
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="14%" align="left">
                                            <asp:Label ID="lblDeptIDOld" runat="server" ForeColor="Blue" Text="*部門\科組課"></asp:Label>
                                        </td>
                                        <td class="td_EditHeader" width="45%" align="left">
                                            <asp:Label ID="lblDeptIDOldData" runat="server"></asp:Label>
                                        </td>
                                        <td class="td_Edit" width="40%" align="left">
                                            <asp:UpdatePanel ID="UdpDeptIDOld" runat="server" ChildrenAsTriggers="False" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <uc:SelectHROrgan ID="ucSelectHROrganOld" runat="server" />
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="ucSelectHROrganOld" />
                                            </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="14%" align="left">
                                            <asp:Label ID="lblFlowOrganIDOld" runat="server" Text="簽核最小單位"></asp:Label>
                                        </td>
                                        <td class="td_EditHeader" width="45%" align="left">
                                            <asp:Label ID="lblFlowOrganIDOldData" runat="server"></asp:Label>
                                        </td>
                                        <td class="td_Edit" width="40%" align="left">
                                            <asp:UpdatePanel ID="UpdFlowOrgnaIDOld" runat="server" ChildrenAsTriggers="False" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:DropDownList ID="ddlFlowOrganIDOld" runat="server"></asp:DropDownList>
                                            </ContentTemplate>                                                          
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="14%" align="left">
                                            <asp:Label ID="lblRankIDOld" runat="server" ForeColor="Blue" Text="*職等\職稱"></asp:Label>
                                        </td>
                                        <td class="td_EditHeader" width="45%" align="left">
                                            <asp:Label ID="lblRankIDOldData" runat="server"></asp:Label>
                                        </td>
                                        <td class="td_Edit" width="40%" align="left">
                                            <asp:UpdatePanel ID="UpdRankIDOld" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:DropDownList ID="ddlRankIDOld" runat="server" AutoPostBack="True"></asp:DropDownList>
                                                <asp:DropDownList ID="ddlTitleIDOld" runat="server" AutoPostBack="True"></asp:DropDownList>
                                                <asp:TextBox ID="txtRankIDMapOld" CssClass="InputTextStyle_Thin" Style="text-transform: uppercase"
                                                    runat="server" MaxLength="1" Height="5px" Width="5px" Visible ="false"  ></asp:TextBox>
                                            </ContentTemplate>                                
                                            </asp:UpdatePanel> 
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="14%" align="left">
                                            <asp:Label ID="lblPositionIDOld" runat="server" Text="職位"></asp:Label>
                                        </td>
                                        <td class="td_EditHeader" width="45%" align="left">
                                            <asp:Label ID="lblPositionIDOldData" runat="server"></asp:Label>
                                        </td>
                                        <td class="td_Edit" width="40%" align="left">
                                            <asp:DropDownList ID="ddlPositionOld" runat="server" AutoPostBack="True"></asp:DropDownList>                       
                                            <uc:ButtonPosition ID="ucSelectPositionOld" runat="server" ButtonText="..." ButtonHint="選取"
                                            WindowHeight="550" WindowWidth="1000" />
                                            <asp:Label ID="lblSelectPositionOld" runat="server" ForeColor="Blue" Text="" style="display:none"></asp:Label> 
                                            <asp:Label ID="lblSelectPositionNameOld" runat="server" ForeColor="Blue" Text="" style="display:none"></asp:Label> 
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="14%" align="left">
                                            <asp:Label ID="lblWorkTypeIDOld" runat="server" ForeColor="Blue" Text="*工作性質"></asp:Label>
                                        </td>
                                        <td class="td_EditHeader" width="45%" align="left">
                                            <asp:Label ID="lblWorkTypeIDOldData" runat="server"></asp:Label>
                                        </td>
                                        <td class="td_Edit" width="40%" align="left">
                                            <asp:DropDownList ID="ddlWorkTypeOld" runat="server" AutoPostBack="True"></asp:DropDownList>                     
                                            <uc:ButtonWorkType ID="ucSelectWorkTypeOld" runat="server" ButtonText="..." ButtonHint="選取"
                                            WindowHeight="550" WindowWidth="1000" />
                                            <asp:Label ID="lblSelectWorkTypeOld" runat="server" ForeColor="Blue" Text="" style="display:none"></asp:Label>
                                            <asp:Label ID="lblSelectWorkTypeNameOld" runat="server" ForeColor="Blue" Text="" style="display:none"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="14%" align="left">
                                            <asp:Label ID="lblWorkStatusOld" runat="server" ForeColor="Blue" Text="*任職狀況"></asp:Label>
                                        </td>
                                        <td class="td_EditHeader" width="45%" align="left">
                                            <asp:Label ID="lblWorkStatusOldData" runat="server"></asp:Label>
                                        </td>
                                        <td class="td_Edit" width="40%" align="left">
                                            <asp:DropDownList ID="ddlWorkStatusOld" runat="server"></asp:DropDownList>
                                        </td>
                                    </tr>

                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="1%" align="left" rowspan="11">
                                            異動後
                                        </td>
                                        <td class="td_EditHeader" width="14%" align="left">
                                            <asp:Label ID="lblGroupIDNew" runat="server" Text="事業群"></asp:Label>
                                        </td>
                                        <td class="td_EditHeader" width="45%" align="left">
                                            <asp:Label ID="lblGroupIDNewData" runat="server"></asp:Label>
                                        </td>
                                        <td class="td_Edit" width="40%" align="left">
                                            <asp:UpdatePanel ID="UpdGroupIDNew" runat="server" ChildrenAsTriggers="False" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:Label ID="lblGroupIDNewShow" runat="server"></asp:Label>
                                            </ContentTemplate>                                                          
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="14%" align="left">
                                            <asp:Label ID="lblDeptIDNew" runat="server" Text="部門\科組課"></asp:Label>
                                        </td>
                                        <td class="td_EditHeader" width="45%" align="left">
                                            <asp:Label ID="lblDeptIDNewData" runat="server"></asp:Label>
                                        </td>
                                        <td class="td_Edit" width="40%" align="left">
                                            <asp:UpdatePanel ID="UdpDeptIDNew" runat="server" ChildrenAsTriggers="False" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <uc:SelectHROrgan ID="ucSelectHROrganNew" runat="server" />
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="ucSelectHROrganNew" />
                                            </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="14%" align="left">
                                            <asp:Label ID="lblFlowOrganIDNew" runat="server" Text="簽核最小單位"></asp:Label>
                                        </td>
                                        <td class="td_EditHeader" width="45%" align="left">
                                            <asp:Label ID="lblFlowOrganIDNewData" runat="server"></asp:Label>
                                        </td>
                                        <td class="td_Edit" width="40%" align="left">
                                            <asp:UpdatePanel ID="UpdFlowOrgnaIDNew" runat="server" ChildrenAsTriggers="False" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:DropDownList ID="ddlFlowOrganIDNew" runat="server"></asp:DropDownList>
                                            </ContentTemplate>                                                          
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="14%" align="left">
                                            <asp:Label ID="lblBossTypeNew" runat="server" Text="主管任用方式"></asp:Label>
                                        </td>
                                        <td class="td_EditHeader" width="45%" align="left">
                                            <asp:RadioButton ID="rbnBossType1Show" GroupName="rbnBossType" runat="server" Text="主要" Enabled="false" />&nbsp;&nbsp;&nbsp;
                                            <asp:RadioButton ID="rbnBossType2Show" GroupName="rbnBossType" runat="server" Text="兼任" Enabled="false" />      
                                        </td>
                                        <td class="td_Edit" width="40%" align="left">
                                            <asp:DropDownList ID="ddlBossTypeNewData" runat="server"></asp:DropDownList>     
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="14%" align="left">
                                            <asp:Label ID="lblOrganBossNew" runat="server" Text="主管"></asp:Label>
                                        </td>
                                        <td class="td_EditHeader" width="45%" align="left">
                                            <asp:CheckBox ID="chkIsBossShow" Text="主管" runat="server" Enabled="false" />&nbsp;&nbsp;&nbsp;
                                            <asp:CheckBox ID="chkIsSecBossShow" Text="副主管" runat="server" Enabled="false" />
                                        </td>
                                        <td class="td_Edit" width="40%" align="left">
                                            <asp:CheckBox ID="chkIsBoss" Text="主管" runat="server" />&nbsp;&nbsp;&nbsp;
                                            <asp:CheckBox ID="chkIsSecBoss" Text="副主管" runat="server" />
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="14%" align="left">
                                            <asp:Label ID="lblOrganFlowBossNew" runat="server" Text="簽核單位"></asp:Label>
                                        </td>
                                        <td class="td_EditHeader" width="45%" align="left">
                                            <asp:CheckBox ID="chkIsGroupBossShow" Text="主管" runat="server" Enabled="false" />&nbsp;&nbsp;&nbsp;
                                            <asp:CheckBox ID="chkIsSecGroupBossShow" Text="副主管" runat="server" Enabled="false" />
                                        </td>
                                        <td class="td_Edit" width="40%" align="left">
                                            <asp:CheckBox ID="chkIsGroupBoss" Text="主管" runat="server" />&nbsp;&nbsp;&nbsp;
                                            <asp:CheckBox ID="chkIsSecGroupBoss" Text="副主管" runat="server" />
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="14%" align="left">
                                            <asp:Label ID="lblRankIDNew" runat="server" Text="職等\職稱"></asp:Label>
                                        </td>
                                        <td class="td_EditHeader" width="45%" align="left">
                                            <asp:Label ID="lblRankIDNewData" runat="server"></asp:Label>
                                        </td>
                                        <td class="td_Edit" width="40%" align="left">
                                            <asp:UpdatePanel ID="UpdRankIDNew" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:DropDownList ID="ddlRankIDNew" runat="server" AutoPostBack="True"></asp:DropDownList>
                                                <asp:DropDownList ID="ddlTitleIDNew" runat="server" AutoPostBack="True"></asp:DropDownList>
                                                <asp:TextBox ID="txtRankIDMapNew" CssClass="InputTextStyle_Thin" Style="text-transform: uppercase"
                                                    runat="server" MaxLength="1" Height="5px" Width="5px" Visible ="false"  ></asp:TextBox>
                                            </ContentTemplate>                                
                                            </asp:UpdatePanel> 
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="14%" align="left">
                                            <asp:Label ID="lblPositionIDNew" runat="server" Text="職位"></asp:Label>
                                        </td>
                                        <td class="td_EditHeader" width="45%" align="left">
                                            <asp:Label ID="lblPositionIDNewData" runat="server"></asp:Label>
                                        </td>
                                        <td class="td_Edit" width="40%" align="left">
                                            <asp:DropDownList ID="ddlPositionNew" runat="server" AutoPostBack="True"></asp:DropDownList>                       
                                            <uc:ButtonPosition ID="ucSelectPositionNew" runat="server" ButtonText="..." ButtonHint="選取"
                                            WindowHeight="550" WindowWidth="1000" />
                                            <asp:Label ID="lblSelectPositionNew" runat="server" ForeColor="Blue" Text="" style="display:none"></asp:Label> 
                                            <asp:Label ID="lblSelectPositionNameNew" runat="server" ForeColor="Blue" Text="" style="display:none"></asp:Label> 
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="14%" align="left">
                                            <asp:Label ID="lblWorkTypeIDNew" runat="server" Text="工作性質"></asp:Label>
                                        </td>
                                        <td class="td_EditHeader" width="45%" align="left">
                                            <asp:Label ID="lblWorkTypeIDNewData" runat="server"></asp:Label>
                                        </td>
                                        <td class="td_Edit" width="40%" align="left">
                                            <asp:DropDownList ID="ddlWorkTypeNew" runat="server" AutoPostBack="True"></asp:DropDownList>                     
                                            <uc:ButtonWorkType ID="ucSelectWorkTypeNew" runat="server" ButtonText="..." ButtonHint="選取"
                                            WindowHeight="550" WindowWidth="1000" />
                                            <asp:Label ID="lblSelectWorkTypeNew" runat="server" ForeColor="Blue" Text="" style="display:none"></asp:Label>
                                            <asp:Label ID="lblSelectWorkTypeNameNew" runat="server" ForeColor="Blue" Text="" style="display:none"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="14%" align="left">
                                            <asp:Label ID="lblWorkStatusNew" runat="server" ForeColor="Blue" Text="*任職狀況"></asp:Label>
                                        </td>
                                        <td class="td_EditHeader" width="45%" align="left">
                                            <asp:Label ID="lblWorkStatusNewData" runat="server"></asp:Label>
                                        </td>
                                        <td class="td_Edit" width="40%" align="left">
                                            <asp:DropDownList ID="ddlWorkStatusNew" runat="server"></asp:DropDownList>
                                        </td>
                                    </tr>                                    
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="14%" align="left">
                                            <asp:Label ID="lblRemarkNew" runat="server" Text="備註"></asp:Label>
                                        </td>
                                        <td class="td_EditHeader" width="45%" align="left">
                                            <asp:Label ID="lblRemarkNewData" runat="server"></asp:Label>
                                        </td>
                                        <td class="td_Edit" width="40%" align="left">
                                            <asp:TextBox ID="txtRemarkNew" CssClass="InputTextStyle_Thin" runat="server" 
                                                MaxLength="100" Width="600px"></asp:TextBox>
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
