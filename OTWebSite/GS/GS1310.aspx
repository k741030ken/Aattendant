<%@ Page Language="VB" AutoEventWireup="false" CodeFile="GS1310.aspx.vb" Inherits="GS_GS1310" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <link type="text/css" rel="stylesheet" href="../StyleSheet.Css" />
    <script type="text/javascript" src="../ClientFun/jquery-1.8.3.min.js"></script>
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
        function ChangeValue(e) {
            alert(e.id);         
        }
    -->
    </script>
    <style type="text/css">
        .style1
        {
            height: 20px;
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
                                    <tr>
                                        <td align="center" width="100%" colspan="2">
                                            <table width="100%" height="100%" class="tbl_Content">
                                                <tr>
                                                    <td style="width: 100%">
                                                        <table class="tbl_Edit" cellpadding="1" cellspacing="1" width="100%" style="font-family:@微軟正黑體">
                                                            <tr style="height:10px">
                                                                <td width="10%" class="td_EditHeader"></td>
                                                                <td width="20%" class="td_EditHeader" align="center" style="font-family:@微軟正黑體"><asp:Label ID="Label1" runat="server" Font-Names="微軟正黑體" Text="簽核主管"></asp:Label></td>
                                                                <td width="70%" class="td_EditHeader" align="center" style="font-family:@微軟正黑體"><asp:Label ID="Label2" runat="server" Font-Names="微軟正黑體" Text="考績補充說明"></asp:Label></td>
                                                                <%--<td width="42%" class="td_EditHeader" align="center">整體評量調整說明</td>--%>
                                                            </tr>
                                                            <tr style="height:20px">
                                                                <td width="10%" class="td_EditHeader" align="left" style="font-family:@微軟正黑體"><asp:Label ID="Label3" runat="server" Font-Names="微軟正黑體" Text="單位主管"></asp:Label></td>
                                                                <td width="20%" class="td_Edit" align="left"><asp:Label ID="lblSignName" runat="server" Font-Names="微軟正黑體"></asp:Label></td>
                                                                <td width="70%" class="td_Edit" align="left"><asp:Label ID="lblComment" runat="server" Font-Names="微軟正黑體"></asp:Label></td>
                                                                <%--<td width="42%" class="td_Edit" align="left"><asp:Label ID="lblComment_Adjust" runat="server" Font-Names="微軟正黑體"></asp:Label></td>--%>
                                                            </tr>
                                                            <tr style="height:20px" id="DeptBoss">
                                                                <td width="10%" class="td_EditHeader" align="left" style="font-family:@微軟正黑體"><asp:Label ID="Label4" runat="server" Font-Names="微軟正黑體" Text="區/處主管"></asp:Label></td>
                                                                <td width="20%" class="td_Edit" align="left"><asp:Label ID="lblSignName1" runat="server" Font-Names="微軟正黑體"></asp:Label></td>
                                                                <td width="70%" class="td_Edit" align="left"><asp:Label ID="lblComment1" runat="server" Font-Names="微軟正黑體"></asp:Label></td>
                                                                <%--<td width="42%" class="td_Edit" align="left"><asp:Label ID="lblComment_Adjust1" runat="server" Font-Names="微軟正黑體"></asp:Label></td>--%>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
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
