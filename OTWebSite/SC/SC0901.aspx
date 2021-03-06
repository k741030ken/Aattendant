<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SC0901.aspx.vb" Inherits="SC_SC0901" %>

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
                                            <asp:Label ID="lblComID" ForeColor="Blue" runat="server" Text="*公司代碼"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left" colspan="3">
                                            <asp:Label ID="txtComID" runat="server"></asp:Label>
                                        </td>
                                    </tr> 
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblTabName" ForeColor="Blue" runat="server" Text="*授權功能類別"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left" colspan="3">
                                            <asp:DropDownList ID="ddlTabName" AutoPostBack="true" runat="server" Font-Size="12px"></asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblFldName" ForeColor="Blue" runat="server" Text="*授權項目"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left" colspan="3">
                                            <div>
                                                <asp:RadioButton ID="rbCompType" Text="可查詢公司別" GroupName="Type" runat="server" AutoPostBack="true" Visible="false" />
                                                <asp:DropDownList ID="ddlFldName1" runat="server" Font-Size="12px" Visible="false"></asp:DropDownList>                                            
                                            </div>
                                            <div>
                                                <asp:RadioButton ID="rbWorkType" Text="可查詢工作性質" GroupName="Type" runat="server" AutoPostBack="true" Visible="false" />
                                                <asp:DropDownList ID="ddlFldName2" runat="server" Font-Size="12px"></asp:DropDownList>                                            
                                            </div>
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblEmpID" ForeColor="Blue" runat="server" Text="*員工編號"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left" colspan="3">
                                            <%--<asp:Label ID="txtEmpID" runat="server"></asp:Label>
                                            <asp:HiddenField ID="hldEmpID" runat="server" />--%>
                                            <asp:TextBox ID="txtEmpID" CssClass="InputTextStyle_Thin" runat="server" MaxLength="6" AutoPostBack="true" Style="text-transform: uppercase"></asp:TextBox>
                                            <asp:Label ID="lblEmpName" runat="server"></asp:Label>
                                            <uc:ButtonQuerySelectUserID ID="ucSelectEmpID" runat="server" ButtonText="..." ButtonHint="選擇人員..." WindowHeight="550" WindowWidth="500" />
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
