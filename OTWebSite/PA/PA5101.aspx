<%@ Page Language="VB" AutoEventWireup="false" CodeFile="PA5101.aspx.vb" Inherits="PA_PA5101" %>

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
        function ChangeValue(e) {
            alert(e.id);         
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
                                            <asp:Label ID="lblFldName" ForeColor="Blue" runat="server" Text="*代碼類別"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left" colspan="3">
                                            <asp:DropDownList ID="ddlFldName" runat="server" Font-Names="微軟正黑體"></asp:DropDownList>
                                        </td>
                                    </tr> 
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblCode" ForeColor="Blue" runat="server" Text="*代碼"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left" colspan="3">
                                            <asp:TextBox ID="txtCode" CssClass="InputTextStyle_Thin" runat="server" MaxLength="30" Width="300px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblCodeCName" ForeColor="Blue" runat="server" Text="*代碼名稱"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left" colspan="3">
                                            <asp:TextBox ID="txtCodeCName" CssClass="InputTextStyle_Thin" runat="server" MaxLength="1000" TextMode="MultiLine" Width="300px" Height="50px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblSortFld" ForeColor="Blue" runat="server" Text="*排序順序"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left" colspan="3">
                                            <asp:TextBox ID="txtSortFld" CssClass="InputTextStyle_Thin" runat="server" MaxLength="3"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblNotShowFlag" runat="server" Text="不顯示註記"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left" colspan="3">
                                            <asp:CheckBox ID="chkNotShowFlag" runat="server" />
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
