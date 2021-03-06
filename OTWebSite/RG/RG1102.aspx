<%@ Page Language="VB" AutoEventWireup="false" CodeFile="RG1102.aspx.vb" Inherits="RG_RG1102" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <link type="text/css" rel="stylesheet" href="../StyleSheet.Css" />
    <script type="text/javascript">
    <!--
        function IDConfirm() {
            if (confirm("身分證字號邏輯有誤，是否重新輸入？【確定】表示重新輸入，【取消】表示忽略且同意修改")) {
                document.getElementById("txtIDNo").focus();
            } else {
                document.getElementById("btnIDConfirm").click();
            }
        }
    -->
    </script>
    <style type="text/css">

    </style>
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
                                            <asp:Label ID="lblCompID" runat="server" Text="公司代碼"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left" colspan="3">
                                            <asp:Label ID="txtCompID" runat="server"></asp:Label>
                                            <asp:HiddenField ID="hidCompID" runat="server"></asp:HiddenField>
                                            <asp:HiddenField ID="hidGroupID" runat="server"></asp:HiddenField>
                                            <asp:Button ID = "btnIDConfirm" runat="server" Text="" Width = "0px" style="display:none;"></asp:Button>
                                        </td>
                                    </tr> 
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblEmpID" ForeColor="Blue" runat="server" Text="*員工編號"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left">
                                            <asp:Label ID="txtEmpID" runat="server"></asp:Label>
                                        </td>                    
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblName" ForeColor="Blue" runat="server" Text="*員工姓名"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left">
                                            <asp:TextBox ID="txtName" CssClass="InputTextStyle_Thin" runat="server" MaxLength="12"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblSex" ForeColor="Blue" runat="server" Text="*性別"></asp:Label>
                                        </td>
                                        <td class="td_Edit" width="35%" align="left">
                                             <asp:DropDownList ID="ddlSex" runat="server" Font-Names="細明體">
                                                <asp:ListItem Value="" Text="---請選擇---" Selected="true"></asp:ListItem>
                                                <asp:ListItem Value="1" Text="1-男"></asp:ListItem>
                                                <asp:ListItem Value="2" Text="2-女"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                         <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblBirthDate" ForeColor="Blue" runat="server" Text="*出生日期"></asp:Label>
                                        </td>
                                        <td class="td_Edit" width="35%" align="left">
                                             <uc:uccalender ID="txtBirthDate" runat="server" Enabled="True" />
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblIDNo" ForeColor="Blue" runat="server" Text="*身分證字號"></asp:Label>
                                        </td>
                                        <td class="td_Edit" width="35%" align="left">
                                            <asp:TextBox ID="txtIDNo" CssClass="InputTextStyle_Thin" runat="server" MaxLength="10" AutoPostBack="true" style="TEXT-TRANSFORM:uppercase"></asp:TextBox>
                                            <asp:HiddenField ID="hidIDNo" runat="server"></asp:HiddenField>
                                        </td>
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblEduID" runat="server" Text="學歷"></asp:Label>
                                        </td>
                                        <td class="td_Edit" width="35%" align="left">
                                             <asp:DropDownList ID="ddlEduID" runat="server" Font-Names="細明體"></asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblHighSchool" runat="server" Text="最高學歷學校"></asp:Label>
                                        </td>
                                        <td class="td_Edit" width="35%" align="left">
                                            <asp:TextBox ID="txtHighSchool" CssClass="InputTextStyle_Thin" runat="server" MaxLength="20"></asp:TextBox>
                                            <uc:ButtonQuerySelectHR ID="ucSelectSchool" runat="server" ButtonText="..." ButtonHint="校名..." WindowHeight="550" WindowWidth="500" />
                                        </td>
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblHighDepart" runat="server" Text="最高學歷科系"></asp:Label>
                                        </td>
                                        <td class="td_Edit" width="35%" align="left">
                                             <asp:TextBox ID="txtHighDepart" CssClass="InputTextStyle_Thin" runat="server" MaxLength="20"></asp:TextBox>
                                             <uc:ButtonQuerySelectHR ID="ucSelectDepart" runat="server" ButtonText="..." ButtonHint="科系..." WindowHeight="550" WindowWidth="500" />
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblSchoolStatus" runat="server" Text="就學狀態"></asp:Label>
                                        </td>
                                        <td class="td_Edit" width="35%" align="left">
                                            <asp:DropDownList ID="ddlSchoolStatus" runat="server" Font-Names="細明體"></asp:DropDownList>
                                        </td>
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblEmpAttrib" runat="server" Text="人員屬性"></asp:Label>
                                        </td>
                                        <td class="td_Edit" width="35%" align="left">
                                             <asp:DropDownList ID="ddlEmpAttrib" runat="server" Font-Names="細明體"></asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblContractStartDate" ForeColor="Blue" runat="server" Text="*契約起日"></asp:Label>
                                        </td>
                                        <td class="td_Edit" width="35%" align="left">
                                            <uc:uccalender ID="txtContractStartDate" runat="server" Enabled="True" />
                                        </td>
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblContractQuitDate" ForeColor="Blue" runat="server" Text="*契約迄日"></asp:Label>
                                        </td>
                                        <td class="td_Edit" width="35%" align="left">
                                             <uc:uccalender ID="txtContractQuitDate" runat="server" Enabled="True" />
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblEmpDate" ForeColor="Blue" runat="server" Text="*到職日"></asp:Label>
                                        </td>
                                        <td class="td_Edit" width="35%" align="left" colspan="3">
                                            <uc:uccalender ID="txtEmpDate" runat="server" Enabled="True" />
                                        </td>                                        
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblOrganDept" ForeColor="Blue" runat="server" Text="*錄用部門、科/組/課"></asp:Label>
                                        </td>
                                        <td class="td_Edit" width="35%" align="left">
                                            <uc:SelectHROrgan ID="ddlOrganDept" runat="server" />
                                        </td>
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblWorkSiteID" runat="server" Text="工作地點"></asp:Label>
                                        </td>
                                        <td class="td_Edit" width="35%" align="left">
                                            <asp:DropDownList ID="ddlWorkSiteID" AutoPostBack="true" runat="server" Font-Names="細明體"></asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblCommTel" runat="server" Text="聯絡電話"></asp:Label>
                                        </td>
                                        <td class="td_Edit" width="35%" align="left">
                                            <asp:TextBox ID="txtCommTel" CssClass="InputTextStyle_Thin" runat="server" MaxLength="20"></asp:TextBox>
                                        </td>
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblRelName" runat="server" Text="緊急聯絡人"></asp:Label>
                                        </td>
                                        <td class="td_Edit" width="35%" align="left">
                                             <asp:TextBox ID="txtRelName" CssClass="InputTextStyle_Thin" runat="server" MaxLength="12"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblCommAddr" runat="server" Text="聯絡地址"></asp:Label>
                                        </td>
                                        <td class="td_Edit" width="35%" align="left" colspan="3">
                                            <asp:TextBox ID="txtCommAddr" CssClass="InputTextStyle_Thin" runat="server" MaxLength="60" Width="600px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblFeeShareDept" runat="server" Text="費用歸屬單位"></asp:Label>
                                        </td>
                                        <td class="td_Edit" width="35%" align="left">
                                            <asp:DropDownList ID="ddlFeeShareDept" runat="server" Font-Names="細明體"></asp:DropDownList>
                                        </td>
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblSalaryUnit" runat="server" Text="計薪方式"></asp:Label>
                                        </td>
                                        <td class="td_Edit" width="35%" align="left">
                                             <asp:DropDownList ID="ddlSalaryUnit" runat="server" Font-Names="細明體"></asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblSalary" ForeColor="Blue" runat="server" Text="*薪資"></asp:Label>
                                        </td>
                                        <td class="td_Edit" width="35%" align="left">
                                            <asp:TextBox ID="txtSalary" CssClass="InputTextStyle_Thin" runat="server" MaxLength="32"></asp:TextBox>
                                        </td>
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblAllowance" ForeColor="Blue" runat="server" Text="*津貼"></asp:Label>
                                        </td>
                                        <td class="td_Edit" width="35%" align="left">
                                             <asp:TextBox ID="txtAllowance" CssClass="InputTextStyle_Thin" runat="server" MaxLength="32"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblEmpSource" runat="server" Text="人才來源"></asp:Label>
                                        </td>
                                        <td class="td_Edit" width="35%" align="left">
                                            <asp:DropDownList ID="ddlEmpSource" runat="server" Font-Names="細明體"></asp:DropDownList>
                                        </td>
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblOutsourcingComp" runat="server" Text="派遣公司"></asp:Label>
                                        </td>
                                        <td class="td_Edit" width="35%" align="left">
                                             <asp:DropDownList ID="ddlOutsourcingComp" runat="server" Font-Names="細明體"></asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblLastChgComp1" runat="server" Text="最後異動公司"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left">
                                            <asp:Label ID="lblLastChgComp" runat="server" ></asp:Label>
                                        </td>
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblLastChgID1" runat="server" Text="最後異動人員"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left">
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
