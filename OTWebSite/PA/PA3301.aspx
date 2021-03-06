<%@ Page Language="VB" AutoEventWireup="false" CodeFile="PA3301.aspx.vb" Inherits="PA_PA3201" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <link type="text/css" rel="stylesheet" href="../StyleSheet.Css" />
    <script type="text/javascript">
    <!--

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
                                            <asp:Label ID="lblRelativeID" ForeColor="Blue" runat="server" Text="*家屬稱謂代碼"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left" colspan="3">
                                            <asp:TextBox ID="txtRelativeID" CssClass="InputTextStyle_Thin" runat="server" MaxLength="2"></asp:TextBox>
                                        </td>
                                    </tr> 
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblRemark" ForeColor="Blue" runat="server" Text="*家屬稱謂名稱(繁)"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left" colspan="3">
                                            <asp:TextBox ID="txtRemark" CssClass="InputTextStyle_Thin" runat="server" MaxLength="20" Width="300px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblRemarkCN" ForeColor="Blue" runat="server" Text="*家屬稱謂名稱(簡)"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left" colspan="3">
                                            <asp:TextBox ID="txtRemarkCN" CssClass="InputTextStyle_Thin" runat="server" MaxLength="20" Width="300px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblHeaRelID" runat="server" Text="健保家屬稱謂代碼"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left" colspan="3">
                                            <asp:TextBox ID="txtHeaRelID" CssClass="InputTextStyle_Thin" runat="server" MaxLength="2"></asp:TextBox>
                                        </td>
                                    </tr> 
                                    <tr>
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblCompRelID" runat="server" Text="團保家屬稱謂代碼"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left" colspan="3">
                                            <asp:TextBox ID="txtCompRelID" CssClass="InputTextStyle_Thin" runat="server" MaxLength="1"></asp:TextBox>
                                        </td>
                                    </tr> 
                                    <tr>
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblDeathPayID" runat="server" Text="喪葬補助稱謂代碼"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left" colspan="3">
                                            <asp:TextBox ID="txtDeathPayID" CssClass="InputTextStyle_Thin" runat="server" MaxLength="2"></asp:TextBox>
                                        </td>
                                    </tr> 
                                    <tr>
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblTaxFamilyID" runat="server" Text="扶養親屬類別代碼"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left" colspan="3">
                                            <asp:DropDownList ID="ddlTaxFamilyID" runat="server" Font-Size="12px" Width="115px">
                                                <asp:ListItem Value="" Text="---請選擇---" Selected="true"></asp:ListItem>
                                                <asp:ListItem Value="0" Text="0-配偶"></asp:ListItem>
                                                <asp:ListItem Value="1" Text="1-尊親"></asp:ListItem>
                                                <asp:ListItem Value="2" Text="2-子女"></asp:ListItem>
                                                <asp:ListItem Value="3" Text="3-兄弟姐妹"></asp:ListItem>
                                                <asp:ListItem Value="4" Text="4-親戚"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr> 
                                    <tr>
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblRelTypeID" runat="server" Text="血親/姻親代碼"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left" colspan="3">
                                            <asp:DropDownList ID="ddlRelTypeID" runat="server" Font-Size="12px" Width="115px">
                                                <asp:ListItem Value="" Text="---請選擇---" Selected="true"></asp:ListItem>
                                                <asp:ListItem Value="B" Text="B-血親"></asp:ListItem>
                                                <asp:ListItem Value="M" Text="M-姻親"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblRelClassID" runat="server" Text="幾等親代碼"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left" colspan="3">
                                            <asp:TextBox ID="txtRelClassID" CssClass="InputTextStyle_Thin" runat="server" MaxLength="1"></asp:TextBox>
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
