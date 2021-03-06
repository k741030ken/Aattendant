﻿<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SC0030.aspx.vb" Inherits="SC_SC0030" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <link type="text/css" rel="stylesheet" href="../StyleSheet.Css" />
    <script type="text/javascript" language="javascript">
    <!--
        function redirectPage(Path) {
            var aryParam = new Array();
            aryParam = Path.split(',');

            if (aryParam[1].toString().toUpperCase().indexOf('SC0031.ASPX') >= 0) {
                OpenWindow(aryParam[1]);
            }
            else {
                //showSubmitting(window.top);
                try {
                    window.parent.frames[3].location = "../SC/SC0050.aspx?FunID=" + aryParam[0] + "&Path=" + aryParam[1] + "&CompRoleID=" + aryParam[2];
                    //window.parent.frames[2].imgSplit.src = "../images/mclose_r.png";  //20160721 Beatrice del
                    window.parent.frames[2].imgSplit.src = "../images/mclose.png";      //20160721 Beatrice modify
                    //window.parent.frames[2].imgSplit.alt = "Open menu"; //20160721 Beatrice del
                    window.parent.frames[2].imgSplit.alt = "Close menu";  //20160721 Beatrice modify
                    window.parent.frames[2].imgSplit_WorkFlow.style.display = 'none';
                    //window.parent.frmSet.cols = "0,0,20,*";  //20160721 Beatrice del
                    window.parent.frmSet.cols = "360,0,20,*";  //20160721 Beatrice modify
                    //window.parent.frames[2].document.bgColor = '#c10c0c'; //20150320 wei del
                    window.parent.frames[2].document.bgColor = '#f1f1f1';   //20150320 wei modify
                    showSubmitting();
                    //checkStatus();
                    window.setTimeout("checkStatus()", 500);
                }
                catch (ex) {
                    window.setTimeout("redirectPage('" + Path + "')", 500);
                }
            }
        }
        
        function checkStatus()
        {
            var msg;
			try
			{
			    //if (window.parent.frames[3].
				var obj = window.parent.frames[3].frames[1].document.getElementById('__ActionParam');
				msg = obj.value;
				hidePopupWindow();
			}
			catch(ex)
			{
				window.setTimeout("checkStatus()", 100);
			}
        }
        
        function funOnLoad() {
            if (frmContent.txtToDoList.value != '')
            {
                if (!((window.parent.frames[3].document.readyState == 'complete' || window.parent.frames[3].document.readyState == 'interactive') &&
                    (window.parent.frames[2].document.readyState == 'complete' || window.parent.frames[2].document.readyState == 'interactive')))
                {
                    window.setTimeout("funOnLoad();", 100);
                    return;
                }

                redirectPage(frmContent.txtToDoList.value);
            }
        }
    -->
    </script>
</head>
<body style="margin-right:10; margin-left:10; margin-top:10; margin-bottom:10; background: url(../images/menu_bg.gif)" onload="funOnLoad();">
    <form id="frmContent" runat="server" style="margin-left:10px">
    <div style="">
        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 100%">
            <tr>
                <td align="left">
                    <asp:Label ID="lblCompID" ForeColor="black" Font-Size="16px" runat="server" Text="授權公司"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="left" >
                    <asp:DropDownList ID="ddlCompRoleID" Font-Size="16px" runat="server" AutoPostBack="true" Font-Names="細明體"></asp:DropDownList>
                    <asp:Label ID="lblCompRoleID" Font-Size="16px" runat="server" CssClass="InputTextStyle_Thin"></asp:Label>
                </td> 
            </tr>
        </table>
    </div>
    <br />
    <div style=""><asp:PlaceHolder ID="phTree" runat="server">
        <asp:TreeView ID="tvFun" runat="server" ShowLines="true" AutoGenerateDataBindings="false" BackColor="transparent" Font-Size="16px" ForeColor="black">
        <NodeStyle Font-Size="16px" ForeColor="black" />
        <HoverNodeStyle Font-Underline="true" />
        <ParentNodeStyle ImageUrl="~/images/folder.gif" />
        <RootNodeStyle ImageUrl="~/images/folder.gif" />
        <LeafNodeStyle ImageUrl="~/images/html.gif" />
        </asp:TreeView>
    </asp:PlaceHolder>
    </div>
    <asp:TextBox ID="txtToDoList" runat="server" style="display:none"></asp:TextBox>
    </form>
</body>
</html>
