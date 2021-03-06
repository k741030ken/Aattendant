<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SC0500.aspx.vb" Inherits="SC_SC0500" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <link type="text/css" rel="stylesheet" href="../StyleSheet.Css" />
    <script type="text/javascript">
    <!--
        function funAction(Param)
        {
            switch(Param)
            {
                case "btnUpdate":
                case "btnDelete":
                    if (!hasSelectedRow(''))
                    {
                        alert("未選取資料列！");
                        return false;
                    }
            }

            switch(Param)
            {
                case "btnDelete":
                    if (!confirm('確定刪除此筆資料？'))
                        return false;
                    break;
            }
        }

        function EntertoSubmit()
        {
            if (window.event.keyCode == 13)
            {
                try
                {
                    window.parent.frames[0].document.getElementById("ucButtonPermission_btnQuery").click();
                }
                catch(ex)
                {}
            }
        }
        
        //另開員工Pop視窗
        function funCallSelectEmp()
        {
            var obj = document.getElementById('ucSelecEmp_btnSelect');
            if (obj == null)
                alert('Can not find object!');
            else
            {
                obj.click();
            }
        }
       -->
    </script>
</head>

<body style="margin-top:5px; margin-left:5px; margin-right:5px; margin-bottom:0" >
    <form id="frmContent" runat="server">
        <%--<table width="100%" class="tbl_Edit" cellpadding="1" cellspacing="1">
            <tr>
                <td class="td_EditHeader" width="15%" align="center">
                    <asp:Label ID="lblSysID" runat="server" ForeColor="blue" Text="系統別"></asp:Label>
                </td>
                <td class="td_Edit" style="width: 35%" align="left">
                    <asp:Label ID="lblSysName" runat="server" CssClass="InputTextStyle_ReadOnly" MaxLength="50" Width="360px"></asp:Label>
                </td>
                <td class="td_EditHeader" width="15%" align="center">
                    <asp:Label ID="lblCompRoleID" runat="server" ForeColor="blue" Text="授權公司"></asp:Label>
                </td>
                <td class="td_Edit" style="width: 35%" align="left">
                    <asp:Label ID="lblCompRoleName" runat="server" CssClass="InputTextStyle_ReadOnly" MaxLength="50" Width="360px"></asp:Label>
                    <asp:DropDownList ID="ddlCompRoleName" runat="server" CssClass="DropDownList" AutoPostBack="true"></asp:DropDownList>
                </td>
            </tr>
        </table>--%>
        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 100%">
            <tr>
                <td align="center" style="height: 30px;">
                    <table cellpadding="1" cellspacing="1" border="0" class="tbl_Condition" height="100%" width="100%">
                        <tr>
                            <td width="5%"></td>
                            <td align="left">
                                <asp:Label ID="lblSysID" runat="server" Font-Size="15px" ForeColor="blue" Text="系統別："></asp:Label>
                            </td>
                            <td align="left">
                                <asp:Label ID="lblSysName" runat="server" Font-Size="15px" CssClass="InputTextStyle_Thin" MaxLength="50" Width="200px"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:Label ID="lblCompRoleID" runat="server" Font-Size="15px" ForeColor="blue" Text="授權公司："></asp:Label>
                            </td>
                            <td align="left">
                                <asp:Label ID="lblCompRoleName" runat="server" Font-Size="15px" CssClass="InputTextStyle_Thin" MaxLength="50" Width="300px"></asp:Label>
                                <asp:DropDownList ID="ddlCompRoleName" runat="server" Font-Size="15px" CssClass="DropDownList" AutoPostBack="true"></asp:DropDownList>
                            </td>
                            <td width="5%"></td>
                        </tr>                        
                        <tr>
                            <td width="5%"></td>
                            <td align="left" width="10%">
                                <asp:Label ID="lblGroupID" Font-Size="15px" runat="server" Text="群組："></asp:Label>
                            </td>
                            <td align="left" width="35%">
                                <asp:DropDownList ID="ddlGroup" runat="Server" CssClass="DropDownList" Width="300px"></asp:DropDownList>
                                <asp:LinkButton ID="btnChangeToFun" runat="server" Font-Size="12px" Text="切換以功能設定" Visible="false"></asp:LinkButton>
                            </td>
                            <td align="left" width="10%">
                                <asp:Label ID="lblFunID" Font-Size="15px" runat="server" Text="功能："></asp:Label>
                            </td>
                            <td align="left" width="35%">
                                <asp:DropDownList ID="ddlFun" runat="Server" CssClass="DropDownList" Width="300px"></asp:DropDownList>
                            </td>
                            <td width="5%"></td>
                        </tr>
                        
                         <tr style="display:none">
                            <td>
                                <uc:ButtonQuerySelect ID="ucSelecEmp" runat="server" Visible="True" 
                                    WindowHeight="580" WindowWidth="500" />
                            </td>
                        </tr>
                        
                    </table>
                </td>
            </tr>
            <tr>
                <td align="center" width="100%">
                    <table width="100%" height="100%" class="tbl_Content">
                        <tr style="display:none">
                            <td align="left">
                                <asp:Label ID="Label1" runat="server" ForeColor="blue" Font-Size="12px" Text="查詢群組："></asp:Label><asp:Label ID="lblGroup" runat="server" ForeColor="blue" Font-Size="12px"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:100%">
                                <uc:PagerControl ID="pcMain" runat="server" GridViewName="gvMain" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 100%">
                                <asp:GridView ID="gvMain" runat="server" CssClass="GridViewStyle" AllowPaging="False" DataKeyNames="SysID,GroupID,CompRoleID,FunID" AutoGenerateColumns="False" CellPadding="2" Width="100%" PageSize="15">
                                    <FooterStyle BackColor="#99CCCC" ForeColor="#003399" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="">
                                            <ItemStyle CssClass="td_detail" Height="15px" />
                                            <HeaderStyle CssClass="td_header" Width="2%" />
                                            <ItemTemplate>
                                                <asp:RadioButton ID="rdo_gvMain" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%--<asp:BoundField DataField="CompRoleID" HeaderText="權限" ReadOnly="True" SortExpression="RightID" Visible="False">
                                            <HeaderStyle Width="15%" CssClass="td_header" />
                                            <ItemStyle CssClass="td_detail" />
                                        </asp:BoundField>--%>
                                        <asp:BoundField DataField="CompRoleID" HeaderText="授權公司" ReadOnly="True" SortExpression="CompRoleID" Visible="False">
                                            <HeaderStyle Width="0%" CssClass="td_header" />
                                            <ItemStyle CssClass="td_detail" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CompName" HeaderText="授權公司" ReadOnly="True" SortExpression="CompName" >
                                            <HeaderStyle Width="5%" CssClass="td_header" />
                                            <ItemStyle CssClass="td_detail" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="GroupID" HeaderText="群組代碼" ReadOnly="True" SortExpression="GroupID" >
                                            <HeaderStyle Width="4%" CssClass="td_header" />
                                            <ItemStyle CssClass="td_detail" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="GroupName" HeaderText="組名" ReadOnly="True" SortExpression="GroupName" >
                                            <HeaderStyle Width="9%" CssClass="td_header" />
                                            <ItemStyle CssClass="td_detail" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="FunID" HeaderText="功能代碼" ReadOnly="True" SortExpression="FunID" >
                                            <HeaderStyle Width="4%" CssClass="td_header" />
                                            <ItemStyle CssClass="td_detail" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="FunName" HeaderText="功能名稱" ReadOnly="True" SortExpression="FunName" >
                                            <HeaderStyle Width="10%" CssClass="td_header" />
                                            <ItemStyle CssClass="td_detail" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="新增" SortExpression="RightA">
                                            <ItemStyle CssClass="td_detail" />
                                            <HeaderStyle CssClass="td_header" Width="3%" />
                                            <ItemTemplate>
                                                <asp:Image id="imgRightA" runat="server" ImageUrl="~/images/chkbox.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightA")="V","True","False") %>' />
                                                <asp:Image id="imgRightA_E" runat="server" ImageUrl="~/images/chkboxE.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightA")="","True","False") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="修改" SortExpression="RightU">
                                            <ItemStyle CssClass="td_detail" />
                                            <HeaderStyle CssClass="td_header" Width="3%" />
                                            <ItemTemplate>
                                                <asp:Image id="imgRightU" runat="server" ImageUrl="~/images/chkbox.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightU")="V","True","False") %>' />
                                                <asp:Image id="imgRightU_E" runat="server" ImageUrl="~/images/chkboxE.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightU")="","True","False") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="刪除" SortExpression="RightD">
                                            <ItemStyle CssClass="td_detail" />
                                            <HeaderStyle CssClass="td_header" Width="3%" />
                                            <ItemTemplate>
                                                <asp:Image id="imgRightD" runat="server" ImageUrl="~/images/chkbox.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightD")="V","True","False") %>' />
                                                <asp:Image id="imgRightD_E" runat="server" ImageUrl="~/images/chkboxE.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightD")="","True","False") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="查詢" SortExpression="RightI">
                                            <ItemStyle CssClass="td_detail" />
                                            <HeaderStyle CssClass="td_header" Width="3%" />
                                            <ItemTemplate>
                                                <asp:Image id="imgRightI" runat="server" ImageUrl="~/images/chkbox.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightI")="V","True","False") %>' />
                                                <asp:Image id="imgRightI_E" runat="server" ImageUrl="~/images/chkboxE.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightI")="","True","False") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="執行" SortExpression="RightE">
                                            <ItemStyle CssClass="td_detail" />
                                            <HeaderStyle CssClass="td_header" Width="3%" />
                                            <ItemTemplate>
                                                <asp:Image id="imgRightE" runat="server" ImageUrl="~/images/chkbox.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightE")="V","True","False") %>' />
                                                <asp:Image id="imgRightE_E" runat="server" ImageUrl="~/images/chkboxE.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightE")="","True","False") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="確認" SortExpression="RightC">
                                            <ItemStyle CssClass="td_detail" />
                                            <HeaderStyle CssClass="td_header" Width="3%" />
                                            <ItemTemplate>
                                                <asp:Image id="imgRightC" runat="server" ImageUrl="~/images/chkbox.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightC")="V","True","False") %>' />
                                                <asp:Image id="imgRightC_E" runat="server" ImageUrl="~/images/chkboxE.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightC")="","True","False") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="列印" SortExpression="RightP">
                                            <ItemStyle CssClass="td_detail" />
                                            <HeaderStyle CssClass="td_header" Width="3%" />
                                            <ItemTemplate>
                                                <asp:Image id="imgRightP" runat="server" ImageUrl="~/images/chkbox.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightP")="V","True","False") %>' />
                                                <asp:Image id="imgRightP_E" runat="server" ImageUrl="~/images/chkboxE.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightP")="","True","False") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="下傳" SortExpression="RightL">
                                            <ItemStyle CssClass="td_detail" />
                                            <HeaderStyle CssClass="td_header" Width="3%" />
                                            <ItemTemplate>
                                                <asp:Image id="imgRightL" runat="server" ImageUrl="~/images/chkbox.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightL")="V","True","False") %>' />
                                                <asp:Image id="imgRightL_E" runat="server" ImageUrl="~/images/chkboxE.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightL")="","True","False") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="上傳" SortExpression="RightF">
                                            <ItemStyle CssClass="td_detail" />
                                            <HeaderStyle CssClass="td_header" Width="3%" />
                                            <ItemTemplate>
                                                <asp:Image id="imgRightF" runat="server" ImageUrl="~/images/chkbox.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightF")="V","True","False") %>' />
                                                <asp:Image id="imgRightF_E" runat="server" ImageUrl="~/images/chkboxE.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightF")="","True","False") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="放行" SortExpression="RightR">
                                            <ItemStyle CssClass="td_detail" />
                                            <HeaderStyle CssClass="td_header" Width="3%" />
                                            <ItemTemplate>
                                                <asp:Image id="imgRightR" runat="server" ImageUrl="~/images/chkbox.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightR")="V","True","False") %>' />
                                                <asp:Image id="imgRightR_E" runat="server" ImageUrl="~/images/chkboxE.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightR")="","True","False") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="複製" SortExpression="RightG">
                                            <ItemStyle CssClass="td_detail" />
                                            <HeaderStyle CssClass="td_header" Width="3%" />
                                            <ItemTemplate>
                                                <asp:Image id="imgRightG" runat="server" ImageUrl="~/images/chkbox.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightG")="V","True","False") %>' />
                                                <asp:Image id="imgRightG_E" runat="server" ImageUrl="~/images/chkboxE.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightG")="","True","False") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="駁回" SortExpression="RightB">
                                            <ItemStyle CssClass="td_detail" />
                                            <HeaderStyle CssClass="td_header" Width="3%" />
                                            <ItemTemplate>
                                                <asp:Image id="imgRightB" runat="server" ImageUrl="~/images/chkbox.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightB")="V","True","False") %>' />
                                                <asp:Image id="imgRightB_E" runat="server" ImageUrl="~/images/chkboxE.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightB")="","True","False") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="清除" SortExpression="RightX">
                                            <ItemStyle CssClass="td_detail" />
                                            <HeaderStyle CssClass="td_header" Width="3%" />
                                            <ItemTemplate>
                                                <asp:Image id="imgRightX" runat="server" ImageUrl="~/images/chkbox.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightX")="V","True","False") %>' />
                                                <asp:Image id="imgRightX_E" runat="server" ImageUrl="~/images/chkboxE.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightX")="","True","False") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="保留" SortExpression="RightH">
                                            <ItemStyle CssClass="td_detail" />
                                            <HeaderStyle CssClass="td_header" Width="3%" />
                                            <ItemTemplate>
                                                <asp:Image id="imgRightH" runat="server" ImageUrl="~/images/chkbox.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightH")="V","True","False") %>' />
                                                <asp:Image id="imgRightH_E" runat="server" ImageUrl="~/images/chkboxE.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightH")="","True","False") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="保留" SortExpression="RightJ">
                                            <ItemStyle CssClass="td_detail" />
                                            <HeaderStyle CssClass="td_header" Width="3%" />
                                            <ItemTemplate>
                                                <asp:Image id="imgRightJ" runat="server" ImageUrl="~/images/chkbox.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightJ")="V","True","False") %>' />
                                                <asp:Image id="imgRightJ_E" runat="server" ImageUrl="~/images/chkboxE.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightJ")="","True","False") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="LastChgComp" HeaderText="最後異動公司" ReadOnly="True" SortExpression="LastChgComp">
                                            <HeaderStyle Width="6%" CssClass="td_header" />
                                            <ItemStyle CssClass="td_detail" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="LastChgID" HeaderText="最後異動者" ReadOnly="True" SortExpression="LastChgID">
                                            <HeaderStyle Width="5%" CssClass="td_header" />
                                            <ItemStyle CssClass="td_detail" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="LastChgDate" HeaderText="最後異動日期" ReadOnly="True" SortExpression="LastChgDate">
                                            <HeaderStyle Width="10%" CssClass="td_header" />
                                            <ItemStyle CssClass="td_detail" />
                                        </asp:BoundField>  
                                    </Columns>
                                    <EmptyDataRowStyle CssClass="GridView_EmptyRowStyle" />
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblNoData" runat="server" Text="無資料顯示！"></asp:Label>
                                    </EmptyDataTemplate>
                                    <RowStyle CssClass="tr_evenline" />
                                    <AlternatingRowStyle CssClass="tr_oddline" />
                                    <SelectedRowStyle CssClass="GridView_SelectedRowStyle" BackColor="#b5efff" />
                                    <PagerStyle CssClass="GridView_PagerStyle" />
                                    <PagerSettings Position="Top" />
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
