<%@ Page Language="VB" AutoEventWireup="false" CodeFile="PA1901.aspx.vb" Inherits="PA_PA1901" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <link type="text/css" rel="stylesheet" href="../StyleSheet.Css" />
    <script type="text/javascript" src="../ClientFun/jquery-1.8.3.min.js"></script>
    <script type="text/javascript">
    <!--
        //工作地點
        function displaycount_Remark() {
            var total = parseInt($('#txtRemark').val().length);
            $("#lblCount_Remark").html('(最多20字，已輸入 <b>' + total + '</b> 字 / 20字)');
        }

        //地址
        function displaycount_Address() {
            var total = parseInt($('#txtAddress').val().length);
            $("#lblCount_Address").html('(最多60字，已輸入 <b>' + total + '</b> 字 / 60字)');
        }

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
                                            <asp:Label ID="lblCompID" ForeColor="Blue" runat="server" Text="*公司代碼"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left" colspan="3">
                                            <asp:Label ID="lbltxtCompID" runat="server" ></asp:Label>
                                        </td>
                                    </tr> 
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblWorkSiteID" ForeColor="Blue" runat="server" Text="*工作地點代碼"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left" colspan="3">
                                            <asp:TextBox ID="txtWorkSiteID" CssClass="InputTextStyle_Thin" runat="server" MaxLength="3"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblRemark" ForeColor="Blue" runat="server" Text="*工作地點"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left" colspan="3">
                                            <asp:TextBox ID="txtRemark" CssClass="InputTextStyle_Thin" runat="server" MaxLength="20" Width="300px" onkeyup="displaycount_Remark()"></asp:TextBox>
                                            <asp:Label ID="lblCount_Remark" runat="server" ForeColor="Red" Font-Size="12px" ></asp:Label>
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblEmpCount" runat="server" Text="人數"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left" colspan="3">
                                            <asp:TextBox ID="txtEmpCount" CssClass="InputTextStyle_Thin" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblBranchFlag" runat="server" Text="分行註記"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left" colspan="3">
                                            <asp:CheckBox ID="chkBranchFlag" runat="server" />
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblBuildingFlag" runat="server" Text="大樓註記"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left" colspan="3">
                                            <asp:CheckBox ID="chkBuildingFlag" runat="server" />
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblCityCode" runat="server" Text="縣市代碼"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left" colspan="3">
                                            <asp:DropDownList ID="ddlCityCode" runat="server" Font-Names="細明體"></asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblWorkSiteCode" runat="server" Text="郵遞區號"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left" colspan="3">
                                            <asp:TextBox ID="txtWorkSiteCode" CssClass="InputTextStyle_Thin" runat="server" MaxLength="5"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblAddress" runat="server" Text="地址"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left" colspan="3">
                                            <asp:TextBox ID="txtAddress" CssClass="InputTextStyle_Thin" runat="server" MaxLength="60" TextMode="MultiLine" Width="400px" Height="60px" onkeyup="displaycount_Address()"></asp:TextBox>
                                            <asp:Label ID="lblCount_Address" runat="server" ForeColor="Red" Font-Size="12px" ></asp:Label>
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblTelephone" runat="server" Text="電話"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left" colspan="3">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label1" runat="server" Text="(國別)"></asp:Label>
                                                    </td>
                                                    <td>                                                        
                                                        <asp:DropDownList ID="ddlCodeNo" runat="server" AutoPostBack="true" Font-Names="細明體"></asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Label2" runat="server" Text="(國碼)"></asp:Label>
                                                    </td>
                                                    <td>                                                        
                                                        <asp:UpdatePanel ID="UpdCountryCode" runat="server" ChildrenAsTriggers="False" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:TextBox ID="txtCountryCode" CssClass="InputTextStyle_Thin" runat="server" MaxLength="6" Width="50px"></asp:TextBox>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="ddlCodeNo" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Label3" runat="server" Text="(區碼)"></asp:Label>
                                                    </td>
                                                    <td>                                                        
                                                        <asp:TextBox ID="txtAreaCode" CssClass="InputTextStyle_Thin" runat="server" MaxLength="3" Width="50px"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Label4" runat="server" Text="(電話)"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtTelephone" CssClass="InputTextStyle_Thin" runat="server" MaxLength="20" Width="100px"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Label5" runat="server" Text="(分機)"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtExtNo" CssClass="InputTextStyle_Thin" runat="server" MaxLength="5" Width="50px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblDialIn" runat="server" Text="撥入類別"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left" colspan="3">
                                            <asp:DropDownList ID="ddlDialIn" runat="server" Font-Names="細明體"></asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblDialOut" runat="server" Text="撥出類別"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left" colspan="3">
                                            <asp:DropDownList ID="ddlDialOut" runat="server" Font-Names="細明體"></asp:DropDownList>
                                        </td>
                                    </tr>
                                    <%-- 20160419 wei del
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblInvoiceNo" runat="server" Text="統一編號"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left" colspan="3">
                                            <asp:TextBox ID="txtInvoiceNo" CssClass="InputTextStyle_Thin" runat="server" MaxLength="8"></asp:TextBox>
                                        </td>
                                    </tr>--%>
                                    <%--2016 wei add 分機長度--%>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblExtYards" runat="server" Text="分機長度"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left" colspan="3">
                                            <asp:DropDownList ID="ddlExtYards" runat="server" Font-Names="細明體"></asp:DropDownList>
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
