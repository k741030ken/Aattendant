﻿<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SC0241.aspx.vb" Inherits="SC_SC0241" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <link type="text/css" rel="stylesheet" href="../StyleSheet.Css" />
    <script type="text/javascript">
    <!--
        function funAction(Param)
        {
            if (Param == 'btnAdd')
            {
                //檢查是否有選取權限
				var objs = document.documentElement.getElementsByTagName("input")
				var strRights = '';
				var objTxt, objTxtE, objChk;
				var strRightID;
				
				for(var i=0; i<objs.length; i++)
				{
				    if (objs[i].id.toString().substr(0, 4) == 'chk_')
				    {
					    if (objs[i].checked)
					    {
					        strRightID = objs[i].id.toString().substr(4,1);
						    if (strRights.toString().length != 0)
						    {
							    strRights = strRights + '|$|';
						    }
    							
						    objTxt = document.getElementById('txt_' + strRightID);
						    objTxtE = document.getElementById('txt_E' + strRightID);
						    objChk = document.getElementById('chkVisible_' + strRightID);
    						
						    if (objChk.checked)
						    {
							    strRights = strRights + strRightID + '||' + objTxt.value + '||' + objTxtE.value + '||0';
						    }
						    else
						    {
							    strRights = strRights + strRightID + '||' + objTxt.value + '||' + objTxtE.value + '||1';
						    }
					    }
					}
				}
				frmContent.txtFunRight.value = strRights;
            }
        }
      
        function funRightClick(obj, UID)
		{
			var objTxt = document.getElementById('txt_' + UID);
			var objTxtE = document.getElementById('txt_E' + UID);
			//var objChk = document.getElementById('chkVisible_' + UID);
			
			if (obj.checked)
			{
				objTxt.disabled = false;
				objTxtE.disabled = false;
				//objChk.disabled = false;
			}
			else
			{
				objTxt.value = '';
				//objChk.checked = false;
				objTxt.disabled = true;
				objTxtE.value = '';
				objTxtE.disabled = true
				//objChk.disabled = true;
			}
		}    
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
                            <td class="td_EditHeader" width="30%" align="center"><asp:Label ID="lblFunID" ForeColor="Blue" runat="server" Text="*功能代碼"></asp:Label>
                            </td>
                            <td class="td_Edit" style="width:70%" align="left"><asp:TextBox ID="txtFunID" CssClass="InputTextStyle_Thin" runat="server" MaxLength="8" style="text-transform:uppercase"></asp:TextBox>
                            </td>
                        </tr>
                        <tr style="height:20px">
                            <td class="td_EditHeader" width="30%" align="center">
                                <asp:Label ID="lblFunName" runat="server" ForeColor="blue" Text="*功能名稱"></asp:Label></td>
                            <td class="td_Edit" style="width: 70%" align="left">
                                <asp:TextBox ID="txtFunName" runat="server" CssClass="InputTextStyle_Thin" MaxLength="50"
                                    Width="360px"></asp:TextBox></td>
                            
                        </tr>
                        <tr style="height:20px">
                            <td class="td_EditHeader" width="30%" align="center">
                                <asp:Label ID="lblFunEngName" runat="server" Text="功能英文名稱"></asp:Label></td>
                            <td class="td_Edit" style="width: 70%; height: 20px;" align="left">
                                <asp:TextBox ID="txtFunEngName" runat="server" CssClass="InputTextStyle_Thin" MaxLength="50"
                                    Width="360px"></asp:TextBox></td>
                        </tr>
                        <tr style="height:20px">
                            <td class="td_EditHeader" width="30%" align="center">
                                <asp:Label ID="lblParentFormID" runat="server" Text="父功能代稱"></asp:Label></td>
                            <td class="td_Edit" style="width: 70%" align="left">
                                <asp:DropDownList ID="ddlParentFormID" runat="server" Width="360px" Font-Names="細明體" Font-Size="12px">
                                </asp:DropDownList></td>
                        </tr>
                        <tr style="height:20px">
                            <td class="td_EditHeader" width="30%" align="center">
                                <asp:Label ID="lblOrderSeq" runat="server" Text="Menu排序"></asp:Label></td>
                            <td class="td_Edit" style="width: 70%;" align="left">
                                <asp:TextBox ID="txtOrderSeq" runat="server" CssClass="InputTextStyle_Thin" MaxLength="4" Text="0"></asp:TextBox></td>
                        </tr>
                        <tr style="height:20px">
                            <td class="td_EditHeader" width="30%" align="center">
                                <asp:Label ID="lblIsMenu" runat="server" Text="是否為Menu"></asp:Label></td>
                            <td class="td_Edit" style="width: 70%;" align="left">
                                &nbsp;<asp:RadioButton ID="rbnIsMenu_Yes" GroupName="rbnIsMenu" runat="server" Checked="true" Text="是" />&nbsp;&nbsp;
                                <asp:RadioButton ID="rbnIsMenu_No" GroupName="rbnIsMenu" runat="server" Text="否" /></td>
                        </tr>
                        <tr style="height:20px">
                            <td class="td_EditHeader" width="30%" align="center">
                                <asp:Label ID="lblCheckRight" runat="server" Text="需授權"></asp:Label></td>
                            <td class="td_Edit" style="width: 70%;" align="left">
                                &nbsp;<asp:RadioButton ID="rdoCheckRightY" GroupName="rdoCheckRight" runat="server" Checked="true" Text="是" />&nbsp;&nbsp;
                                <asp:RadioButton ID="rdoCheckRightN" GroupName="rdoCheckRight" runat="server" Text="否" /></td>
                        </tr>
                        <tr style="height:20px">
                            <td class="td_EditHeader" width="30%" align="center">
                                <asp:Label ID="lblPath" runat="server" Text="網頁路徑"></asp:Label></td>
                            <td class="td_Edit" style="width: 70%;" align="left">
                                <asp:TextBox ID="txtPath" runat="server" CssClass="InputTextStyle_Thin" MaxLength="200"
                                    Width="360px"></asp:TextBox></td>
                        </tr>
                        <tr style="">
                            <td class="td_EditHeader" width="30%" align="center">
                                <asp:Label ID="lblFunRight" ForeColor="blue" runat="server" Text="*功能權限"></asp:Label></td>
                            <td class="td_Edit" style="width: 70%;" align="left">
                                <asp:Table ID="tblFunRight" EnableViewState="true" runat="server" Font-Size="12px" Width="100%">
                                </asp:Table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            
        </table>
        <asp:TextBox ID="txtFunRight" runat="server" style="display:none"></asp:TextBox>
        </form>
</body>
</html>
