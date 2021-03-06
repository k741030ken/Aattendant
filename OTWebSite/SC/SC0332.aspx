<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SC0332.aspx.vb" Inherits="SC_SC0332" %>

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
<body style="margin-top:5px; margin-left:5px; margin-right:5px; margin-bottom:0">
    <form id="frmContent" runat="server">
        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 100%">
            <tr>
                <td align="center">
                    <table width="80%" class="tbl_Edit" cellpadding="1" cellspacing="1">
                        <tr style="height:20px">
                            <td class="td_EditHeader" width="30%" align="center"><asp:Label ID="lblDeptID_H" CssClass="MustInputCaption" runat="server" Text="部門"></asp:Label>
                            </td>
                            <td class="td_Edit" style="width:70%" align="left"><asp:DropDownList ID="ddlDeptID" runat="server" CssClass="DropDownListStyle" AutoPostBack="true"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr style="height:20px">
                            <td class="td_EditHeader" width="30%" align="center"><asp:Label ID="lblOrganID_H" CssClass="MustInputCaption" runat="server" Text="科組課"></asp:Label>
                            </td>
                            <td class="td_Edit" style="width:70%" align="left"><asp:DropDownList ID="ddlOrganID" runat="server" CssClass="DropDownListStyle" AutoPostBack="true"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr style="height:20px">
                            <td class="td_EditHeader" width="30%" align="center"><asp:Label ID="lblWorkTypeID_H" CssClass="MustInputCaption" runat="server" Text="工作性質"></asp:Label>
                            </td>
                            <td class="td_Edit" style="width:70%" align="left"><asp:DropDownList ID="ddlWorkTypeID" runat="server" CssClass="DropDownListStyle"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr style="height:20px">
                            <td class="td_EditHeader" width="30%" align="center"><asp:Label ID="lblUpdateSeq_H" CssClass="MustInputCaption" runat="server" Text="異動順序"></asp:Label>
                            </td>
                            <td class="td_Edit" style="width:70%" align="left"><asp:Textbox ID="txtUpdateSeq" CssClass="InputTextStyle_Thin" runat="server" Width="115px" MaxLength="6"></asp:Textbox>
                            </td>
                        </tr>
                        <tr style="height:20px">
                            <td class="td_EditHeader" width="30%" align="center"><asp:Label ID="lblBanMark" runat="server" Text="可使用註記"></asp:Label>
                            </td>
                            <td class="td_Edit" style="width:70%" align="left">
                                <asp:RadioButtonList ID="rblBanMark" runat="server">
                                    <asp:ListItem Text="有效" Selected="True" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="禁用" Value="1"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr style="height:20px">
                            <td class="td_EditHeader" width="30%" align="center"><asp:Label ID="lblGroupID" runat="server" Text="群組"></asp:Label>
                            </td>
                            <td class="td_Edit" style="width:70%" align="left"><asp:DropDownList ID="ddlGroupID" runat="server" CssClass="DropDownListStyle"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr style="height:20px">
                            <td class="td_EditHeader" width="30%" align="center"><asp:Label ID="lblBusinessFlag" runat="server" Text="業務註記"></asp:Label>
                            </td>
                            <td class="td_Edit" style="width:70%" align="left">
                                <asp:RadioButtonList ID="rblBusinessFlag" runat="server">
                                    <asp:ListItem Text="不指定" Selected="True" Value=""></asp:ListItem>
                                    <asp:ListItem Text="業務人員" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="非業務人員" Value="0"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr style="height:20px">
                            <td class="td_EditHeader" width="30%" align="center"><asp:Label ID="lblDescription" runat="server" Text="說明"></asp:Label>
                            </td>
                            <td class="td_Edit" style="width:70%" align="left"><asp:TextBox ID="txtDescription" CssClass="InputTextStyle_Thin" runat="server" MaxLength="80" Width="300px"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>

        </table>
        </form>
</body>
</html>
