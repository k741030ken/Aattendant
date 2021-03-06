<%@ Page Language="VB" AutoEventWireup="false" CodeFile="RG1402.aspx.vb" Inherits="RG_RG1402" %>

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
                                    <tr>
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblCompID" runat="server" Text="公司代碼"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left" colspan="3">
                                            <asp:Label ID="txtCompID" runat="server"></asp:Label>
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
                                            <asp:Label ID="lblName" runat="server" Text="員工姓名"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left">
                                            <asp:Label ID="txtName" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblOrgan" runat="server" Text="部門"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left">
                                            <asp:Label ID="txtOrgan" runat="server"></asp:Label>
                                        </td>
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblEmpDate" runat="server" Text="到職日"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left">
                                            <asp:Label ID="txtEmpDate" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblFileAll" runat="server" Text="繳交項目全選"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left" colspan="3">
                                            <asp:CheckBox ID="cbFileAll" AutoPostBack="true" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td style="height:20px"></td>
                        </tr>
                        <tr>
                            <td align="center">
                                <table width="80%" class="tbl_Edit" cellpadding="1" cellspacing="1">
                                    <tr style="height:20px">
                                        <td class="td_Edit" style="width:50%" align="left">
                                            <asp:CheckBox ID="cbFile1" Text="1.身份證影本乙份" runat="server" />
                                        </td>
                                        <td class="td_Edit" width="50%" align="left">
                                            <asp:CheckBox ID="cbFile2" Text="2.學歷證件影本乙份" runat="server" />
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_Edit" style="width:50%" align="left">
                                            <asp:CheckBox ID="cbFile3" Text="3.戶籍謄本影本或戶口名簿影本乙份" runat="server" />
                                        </td>
                                        <td class="td_Edit" width="50%" align="left">
                                            <asp:CheckBox ID="cbFile4" Text="4.照片五張" runat="server" />
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_Edit" style="width:50%" align="left">
                                            <asp:CheckBox ID="cbFile5" Text="5.保證書二份" runat="server" />
                                        </td>
                                        <td class="td_Edit" width="50%" align="left">
                                            <asp:CheckBox ID="cbFile6" Text="6.承諾書乙份" runat="server" />
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_Edit" style="width:50%" align="left">
                                            <asp:CheckBox ID="cbFile7" Text="7.客戶資料保密切結書乙份" runat="server" Enabled="false" />
                                        </td>
                                        <td class="td_Edit" width="50%" align="left">
                                            <asp:CheckBox ID="cbFile8" Text="8.扶養親屬表" runat="server" />
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_Edit" style="width:50%" align="left">
                                            <asp:CheckBox ID="cbFile9" Text="9.人事資料表" runat="server" />
                                        </td>
                                        <td class="td_Edit" width="50%" align="left">
                                            <asp:CheckBox ID="cbFile10" Text="10.原公司離職證明乙份" runat="server" />
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_Edit" style="width:50%" align="left">
                                            <asp:CheckBox ID="cbFile11" Text="11.退伍令" runat="server" Enabled="false" />
                                        </td>
                                        <td class="td_Edit" width="50%" align="left">
                                            <asp:CheckBox ID="cbFile12" Text="12.永豐銀行存摺影本" runat="server" />
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_Edit" style="width:50%" align="left">
                                            <asp:CheckBox ID="cbFile13" Text="13.團體醫療保險申請表" runat="server" />
                                        </td>
                                        <td class="td_Edit" width="50%" align="left">
                                            <asp:CheckBox ID="cbFile14" Text="14.勞、健保加保申請書" runat="server" />
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_Edit" style="width:50%" align="left">
                                            <asp:CheckBox ID="cbFile15" Text="15.識別證製作申請書" runat="server" />
                                        </td>
                                        <td class="td_Edit" width="50%" align="left">
                                            <asp:CheckBox ID="cbFile16" Text="16.員工二親等眷屬資料表" runat="server" />
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_Edit" style="width:50%" align="left">
                                            <asp:CheckBox ID="cbFile17" Text="17.健康檢查報告" runat="server" />
                                        </td>
                                        <td class="td_Edit" width="50%" align="left">
                                            <asp:CheckBox ID="cbFile18" Text="18.同意書及員工基本工作規範" runat="server" />
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_Edit" align="left" colspan="2">
                                            <asp:Label ID="lblRemark" runat="server" Text="備註："></asp:Label>
                                            <asp:TextBox ID="txtRemark" runat="server" CssClass="InputTextStyle_Thin" MaxLength="25" Width="400px" onkeyup="displaycount_Remark()"></asp:TextBox>
                                            <asp:Label ID="lblCount_Remark" runat="server" ForeColor="Red" Font-Size="12px" ></asp:Label>
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_Edit" width="50%" align="left">
                                            <asp:Label ID="lblLastChgComp" runat="server" Text="最後異動公司："></asp:Label>
                                            <asp:Label ID="txtLastChgComp" runat="server" ></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:50%" align="left">
                                            <asp:Label ID="lblLastChgID" runat="server" Text="最後異動人員："></asp:Label>
                                            <asp:Label ID="txtLastChgID" runat="server" > </asp:Label>
                                        </td>
                                    </tr>
                                    <tr style="height:20px">                                        
                                        <td class="td_Edit" width="50%" align="left" colspan="2">
                                            <asp:Label ID="lblLastChgDate" runat="server" Text="最後異動日期："></asp:Label>
                                            <asp:Label ID="txtLastChgDate" runat="server" ></asp:Label>
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
