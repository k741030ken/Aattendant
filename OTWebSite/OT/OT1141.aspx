<%@ Page Language="VB" AutoEventWireup="false" CodeFile="OT1141.aspx.vb" Inherits="OT_OT1141" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <link type="text/css" rel="stylesheet" href="../StyleSheet.Css" />
    <script type="text/javascript" src="../ClientFun/jquery-1.8.3.min.js"></script>
    <script type="text/javascript">
    <!--
        $(function () {
            $("#txtEmpID").change(function () {
                $("#btnEmpID").click();
            });

        });

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
            width: 17%;
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
                                            <asp:Label ID="lblCompID" runat="server" Text="公司代碼"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left">
                                            <asp:Label ID="txtCompID" runat="server"></asp:Label>
                                            <asp:HiddenField ID="hidCompID" runat="server"></asp:HiddenField>
                                        </td>
                                    </tr> 
                                    <tr style="height:20px">
                                        <td class="style1" align="center">
                                            <asp:Label ID="lblEmpID" ForeColor="Blue" runat="server" Text="*員工編號"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left">
                                            <asp:TextBox ID="txtEmpID" CssClass="InputTextStyle_Thin" runat="server" MaxLength="6"></asp:TextBox>
                                            <asp:Label ID="lblName" runat="server"></asp:Label>
                                            <uc:ButtonQuerySelectUserID ID="ucQueryEmp" runat="server" WindowHeight="800" WindowWidth="600" ButtonText="..." />
                                            <asp:Button ID="btnEmpID" runat="server" style="display:none" />
                                        </td>
                                    </tr>
                                    <tr style="height:20px;DISPLAY:table-row" id="tr4">
                                        <td class="style1" align="center">
                                            <asp:Label ID="lblUAmount" ForeColor="Blue" runat="server" Text="*上限金額"></asp:Label>
                                        </td>
                                        <td class="td_Edit" width="35%" align="left">
                                             <asp:TextBox ID="txtUAmount" CssClass="InputTextStyle_Thin" runat="server"></asp:TextBox>
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
