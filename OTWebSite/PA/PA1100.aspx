<%@ Page Language="VB" AutoEventWireup="false" CodeFile="PA1100.aspx.vb" Inherits="PA_PA1100" %>

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
                <td align="center" style="height: 30px;">
                    <table cellpadding="1" cellspacing="1" border="0" class="tbl_Condition" height="100%" width="100%">
                         <tr>
                            <td width="10%"></td>
                            <td align="left" width="10%">
                                <asp:Label ID="lblCompID" ForeColor="blue" Font-Size="15px" runat="server" Text="公司代碼："></asp:Label>
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="ddlCompID" runat="server" AutoPostBack="true" Font-Names="細明體"></asp:DropDownList>
                                <asp:Label ID="lblCompRoleID" Font-Size="15px" runat="server" CssClass="InputTextStyle_Thin" Width="200px"></asp:Label>
                                <asp:HiddenField ID="IsDoQuery" runat="server"></asp:HiddenField>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="center" width="100%">
                    <table width="100%" height="100%" class="tbl_Content" id="tableControll" runat="server" visible="false">
                        <tr>
                            <td style="width:10%" align="left" />
                            <td style="width:40%" align="left">
                                <asp:Label ID="Label1" runat="server">1 .</asp:Label>
                                <asp:Image id="cb_UserGroupNotAdmin1" runat="server" ImageUrl="~/images/chkbox.gif" />
                                <asp:Image id="cb_UserGroupNotAdmin2" runat="server" ImageUrl="~/images/chkboxE.gif" />
                                <asp:Label runat="server">授權公司權限(非admin)</asp:Label>                                
                            </td>
                            <td style="width:50%" align="left">
                                <asp:Label ID="Label12" runat="server">11.</asp:Label> 
                                <asp:Image id="cb_Organization1" runat="server" ImageUrl="~/images/chkbox.gif" />
                                <asp:Image id="cb_Organization2" runat="server" ImageUrl="~/images/chkboxE.gif" />
                                <asp:Label ID="Label13" runat="server">HR1100部門組織資料維護</asp:Label>                                
                            </td>
                        </tr>
                        <tr>
                            <td style="width:10%" align="left" />
                            <td style="width:40%" align="left">
                                <asp:Label ID="Label4" runat="server">2 .</asp:Label>
                                <asp:Image id="cb_Rank1" runat="server" ImageUrl="~/images/chkbox.gif" />
                                <asp:Image id="cb_Rank2" runat="server" ImageUrl="~/images/chkboxE.gif" />
                                <asp:Label ID="Label31" runat="server">PA1300職等代碼設定</asp:Label>
                            </td>
                            <td style="width:50%" align="left">
                                <asp:Label ID="Label2" runat="server">12.</asp:Label> 
                                <asp:Image id="cb_OrganizationFlow1" runat="server" ImageUrl="~/images/chkbox.gif" />
                                <asp:Image id="cb_OrganizationFlow2" runat="server" ImageUrl="~/images/chkboxE.gif" />
                                <asp:Label ID="Label21" runat="server">HR1500簽核部門組織資料維護</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:10%" align="left" />
                            <td style="width:40%" align="left">
                                <asp:Label ID="Label3" runat="server">3 .</asp:Label> 
                                <asp:Image id="cb_Title1" runat="server" ImageUrl="~/images/chkbox.gif" />
                                <asp:Image id="cb_Title2" runat="server" ImageUrl="~/images/chkboxE.gif" />
                                <asp:Label ID="Label51" runat="server">PA1400職稱代碼設定</asp:Label>                                
                            </td>
                            <td style="width:50%" />
                        </tr>
                        <tr>
                            <td style="width:10%" align="left" />
                            <td style="width:40%" align="left">
                                <asp:Label ID="Label5" runat="server">4 .</asp:Label> 
                                <asp:Image id="cb_CompareRank1" runat="server" ImageUrl="~/images/chkbox.gif" />
                                <asp:Image id="cb_CompareRank2" runat="server" ImageUrl="~/images/chkboxE.gif" />
                                <asp:Label ID="Label61" runat="server">PA1500金控職等對照設定</asp:Label>                                
                            </td>
                            <td style="width:50%" />
                        </tr>
                        <tr>
                            <td style="width:10%" align="left" />
                            <td style="width:40%" align="left">
                                <asp:Label ID="Label6" runat="server">5 .</asp:Label> 
                                <asp:Image id="cb_RankMapping1" runat="server" ImageUrl="~/images/chkbox.gif" />
                                <asp:Image id="cb_RankMapping2" runat="server" ImageUrl="~/images/chkboxE.gif" />
                                <asp:Label ID="Label71" runat="server">PA1600職等比對設定</asp:Label>                                
                            </td>
                            <td style="width:50%" />
                        </tr>
                        <tr>
                            <td style="width:10%" align="left" />
                            <td style="width:40%" align="left">
                                <asp:Label ID="Label7" runat="server">6 .</asp:Label> 
                                <asp:Image id="cb_WorkType1" runat="server" ImageUrl="~/images/chkbox.gif" />
                                <asp:Image id="cb_WorkType2" runat="server" ImageUrl="~/images/chkboxE.gif" />
                                <asp:Label ID="Label81" runat="server">PA1700工作性質設定</asp:Label>                                
                            </td>
                            <td style="width:50%" />
                        </tr>
                        <tr>
                            <td style="width:10%" align="left" />
                            <td style="width:40%" align="left">
                                <asp:Label ID="Label8" runat="server">7 .</asp:Label> 
                                <asp:Image id="cb_Position1" runat="server" ImageUrl="~/images/chkbox.gif" />
                                <asp:Image id="cb_Position2" runat="server" ImageUrl="~/images/chkboxE.gif" />
                                <asp:Label ID="Label91" runat="server">PA1800職位設定</asp:Label>                                
                            </td>
                            <td style="width:50%" />
                        </tr>
                        <tr>
                            <td style="width:10%" align="left" />
                            <td style="width:40%" align="left">
                                <asp:Label ID="Label9" runat="server">8 .</asp:Label> 
                                <asp:Image id="cb_WorkSite1" runat="server" ImageUrl="~/images/chkbox.gif" />
                                <asp:Image id="cb_WorkSite2" runat="server" ImageUrl="~/images/chkboxE.gif" />
                                <asp:Label ID="Label101" runat="server">PA1900工作地點設定</asp:Label>                                
                            </td>
                            <td style="width:50%" />
                        </tr>
                        <tr>
                            <td style="width:10%" align="left" />
                            <td style="width:40%" align="left">
                                <asp:Label ID="Label10" runat="server">9 .</asp:Label> 
                                <asp:Image id="cb_Calendar1" runat="server" ImageUrl="~/images/chkbox.gif" />
                                <asp:Image id="cb_Calendar2" runat="server" ImageUrl="~/images/chkboxE.gif" />
                                <asp:Label ID="Label112" runat="server">PA1A00年曆檔維護</asp:Label>                                
                            </td>
                            <td style="width:50%" />
                        </tr>
                        <tr>
                            <td style="width:10%" align="left" />
                            <td style="width:40%" align="left">
                                <asp:Label ID="Label11" runat="server">10.</asp:Label>
                                <asp:Image id="cb_WorkTime1" runat="server" ImageUrl="~/images/chkbox.gif" />
                                <asp:Image id="cb_WorkTime2" runat="server" ImageUrl="~/images/chkboxE.gif" />
                                <asp:Label ID="Label41" runat="server">PA2100公司班別設定</asp:Label>
                            </td>
                            <td style="width:50%" />
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
