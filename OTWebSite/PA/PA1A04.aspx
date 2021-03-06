<%@ Page Language="VB" AutoEventWireup="false" CodeFile="PA1A04.aspx.vb" Inherits="PA_PA1A04" %>

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
    <style type="text/css">
        .style1
        {
            font-size: 14px;
            width: 20%;
            border: 1px solid #5384e6;
            background-color: #e2e9fe;
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
                                        <td class="style1" align="center">
                                            <asp:Label ID="lblCopyYear" ForeColor="Blue" runat="server" Text="*複製年度"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left" colspan="3">
                                            <asp:TextBox ID="txtCopyYear" CssClass="InputTextStyle_Thin" runat="server" MaxLength="4"></asp:TextBox>
                                            <asp:Label ID="Label1" runat="server" Text="(西元格式:yyyy)"></asp:Label>
                                        </td>                                       
                                    </tr> 
                                    <tr style="height:20px">
                                        <td class="style1" align="center">
                                            <asp:Label ID="lblSource" ForeColor="Blue" runat="server" Text="*年曆檔複製來源公司"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left" colspan="3">
                                            <asp:DropDownList ID="ddlSource" runat="server" Font-Names="細明體"></asp:DropDownList>
                                        </td>
                                    </tr>       
                                    <tr style="height:20px">
                                        <td class="style1" align="center">
                                            <asp:Label ID="lblCopyTo" runat="server" ForeColor="Blue" Text="*複製至授權公司"></asp:Label>
                                        </td>
                                        <td class="td_Edit" width="35%" align="left" colspan="3">
                                           <asp:DropDownList ID="ddlCopyTo" runat="server" Font-Names="細明體"></asp:DropDownList>
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
