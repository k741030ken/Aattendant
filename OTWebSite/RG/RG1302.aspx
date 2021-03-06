<%@ Page Language="VB" AutoEventWireup="false" CodeFile="RG1302.aspx.vb" Inherits="RG_RG1302" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <link type="text/css" rel="stylesheet" href="../StyleSheet.Css" />
    <script type="text/javascript" src="../ClientFun/jquery-1.8.3.min.js"></script>
    <script type="text/javascript">
    <!--
        //備註
        function displaycount_Remark() {
            var total = parseInt($('#txtRemark').val().length);
            $("#lblCount_Remark").html('(最多25字，已輸入 <b>' + total + '</b> 字 / 25字)');
        }
    -->
    </script>
    <style type="text/css">
        .style1
        {
            font-size: 14px;
            width: 15%;
            border: 1px solid #5384e6;
            background-color: #e2e9fe;
        }
        .style2
        {
            width: 15%;
        }
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
                                        <td align="center" class="style1">
                                            <asp:Label ID="lblCompID" runat="server" Text="公司代碼"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left" colspan="3">
                                            <asp:Label ID="txtCompID" runat="server"></asp:Label>
                                        </td>
                                    </tr> 
                                    <tr style="height:20px">
                                        <td class="style1" align="center">
                                            <asp:Label ID="lblEmpID" ForeColor="Blue" runat="server" Text="員工編號"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left">
                                            <asp:Label ID="txtEmpID" runat="server"></asp:Label>
                                        </td>                    
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblName" runat="server" Text="員工姓名"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left">
                                            <asp:Label ID="txtName" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr style="height:20px;DISPLAY:table-row" id="tr4">
                                        <td class="style1" align="center">
                                            <asp:Label ID="lblOrgan" runat="server" Text="部門"></asp:Label>
                                        </td>
                                        <td class="td_Edit" width="35%" align="left">
                                            <asp:Label ID="txtOrgan" runat="server"></asp:Label>
                                        </td>
                                         <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblEmpDate" runat="server" Text="到職日"></asp:Label>
                                        </td>
                                        <td class="td_Edit" width="35%" align="left">
                                            <asp:Label ID="txtEmpDate" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblFileAll" runat="server" Text="繳交項目全選"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left" colspan="3">
                                            <asp:CheckBox ID="chkFileAll" AutoPostBack="true" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <table width="80%" class="tbl_Edit" cellpadding="1" cellspacing="1">
                                    <tr style="height:20px">
                                        <td class="td_Edit" style="width:50%" align="left">
                                            <asp:CheckBox ID="chkFile1" runat="server" Text="1.基本資料" />
                                        </td>
                                        <td class="td_Edit" style="width:50%" align="left">
                                            <asp:CheckBox ID="chkFile2" runat="server" Text="2.倫理規範同意書" />
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_Edit" style="width:50%" align="left">
                                            <asp:CheckBox ID="chkFile3" runat="server" Text="3.扶養親屬表" />
                                        </td>
                                        <td class="td_Edit" style="width:50%" align="left">
                                            <asp:CheckBox ID="chkFile4" runat="server" Text="4.工作紀律表" />
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_Edit" style="width:50%" align="left">
                                            <asp:CheckBox ID="chkFile5" runat="server" Text="5.身分證影本" />
                                        </td>
                                        <td class="td_Edit" style="width:50%" align="left">
                                            <asp:CheckBox ID="chkFile6" runat="server" Text="6.學歷證件影本" />
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_Edit" style="width:50%" align="left">
                                            <asp:CheckBox ID="chkFile7" runat="server" Text="7.原服務單位離職證明書" />
                                        </td>
                                        <td class="td_Edit" style="width:50%" align="left">
                                            <asp:CheckBox ID="chkFile8" runat="server" Text="8.健保轉出申報表" />
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_Edit" style="width:50%" align="left">
                                            <asp:CheckBox ID="chkFile9" runat="server" Text="9.退伍令" />
                                        </td>
                                        <td class="td_Edit" style="width:50%" align="left">
                                            <asp:CheckBox ID="chkFile10" runat="server" Text="10.永豐帳戶影本" />
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_Edit" style="width:50%" align="left">
                                            <asp:CheckBox ID="chkFile11" runat="server" Text="11.彰銀帳戶影本" />
                                        </td>
                                        <td class="td_Edit" style="width:50%" align="left">
                                            <asp:CheckBox ID="chkFile12" runat="server" Text="12.健康檢查報告" />
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_Edit" style="width:50%" align="left">
                                            <asp:CheckBox ID="chkFile13" runat="server" Text="13.兩吋半身正面照片兩張" />
                                        </td>
                                        <td class="td_Edit" style="width:50%" align="left">
                                            <asp:CheckBox ID="chkFile14" runat="server" Text="14.契約書" />
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_Edit" style="width:50%" align="left">
                                            <asp:CheckBox ID="chkFile15" runat="server" Text="15.學生證 Or 在學證明" />
                                        </td>
                                        <td class="td_Edit" style="width:50%" align="left">
                                            <asp:CheckBox ID="chkFile16" runat="server" Text="16.健康聲明書" />
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_Edit" style="width:50%" align="left">
                                            <asp:CheckBox ID="chkFile17" runat="server" Text="17.勞工退休金制度選擇意願徵詢表" />
                                        </td>
                                        <td class="td_Edit" style="width:50%" align="left">
                                            <asp:CheckBox ID="chkFile18" runat="server" Text="18.刑事紀錄證明" />
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_Edit" style="width:50%" align="left">
                                            <asp:CheckBox ID="chkFile19" runat="server" Text="19.保密切結書" />
                                        </td>
                                        <td class="td_Edit" style="width:50%" align="left">
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_Edit" style="width:50%" align="left" colspan="2">
                                            <asp:Label ID="lblRemark" runat="server" Text="備註："></asp:Label>
                                            <asp:TextBox ID="txtRemark" CssClass="InputTextStyle_Thin" runat="server" MaxLength="25" Width="400px" onkeyup="displaycount_Remark()"></asp:TextBox>
                                            <asp:Label ID="lblCount_Remark" runat="server" ForeColor="Red" Font-Size="12px" ></asp:Label>
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_Edit" style="width:50%" align="left">
                                            <asp:Label ID="lblLastChgID" runat="server" Text="最後異動人員："></asp:Label>
                                            <asp:Label ID="txtLastChgID" runat="server" > </asp:Label>
                                        </td>
                                        <td class="td_Edit" width="50%" align="left">
                                            <asp:Label ID="lblLastChgDate" runat="server" Text="最後異動日期："></asp:Label>
                                            <asp:Label ID="txtLastChgDate" runat="server" ></asp:Label>
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_Edit" width="50%" align="left" colspan="2">
                                            <asp:Label ID="lblLastChgComp" runat="server" Text="最後異動公司："></asp:Label>
                                            <asp:Label ID="txtLastChgComp" runat="server" ></asp:Label>
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
