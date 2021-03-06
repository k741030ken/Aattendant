<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SC0503.aspx.vb" Inherits="SC_SC0503" %>

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
       -->
    </script>
</head>

<body style="margin-top:5px; margin-left:5px; margin-right:5px; margin-bottom:0" >
    <form id="frmContent" runat="server">
    <tr>
                <td align="center" width="100%">
                <table width="100%" class="tbl_Edit" cellpadding="1" cellspacing="1">
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
                                <asp:DropDownList ID="ddlCompRoleName" runat="server" CssClass="DropDownListStyle"></asp:DropDownList>
                </td>
                </table>
                </td>
            </tr>
        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 100%">
            <tr>
                <td align="center" style="height: 30px;">
                    <table cellpadding="1" cellspacing="1" border="0" class="tbl_Condition" height="100%" width="100%">
                        <tr>
                            <td width="15%"></td>
                            <td align="right" width="25%">
                                <asp:Label ID="lblFunID" Font-Size="15px" runat="server" CssClass="MustInputCaption" Text="功能："></asp:Label>
                            </td>
                            <td align="left" width="45%">
                                <asp:DropDownList ID="ddlFun" runat="Server" Font-Names="calibri, 新細明體" Font-Size="12px" Width="200px"></asp:DropDownList>
                                <asp:LinkButton ID="btnChangeToGroup" runat="server" Font-Size="12px" Text="切換以群組設定"></asp:LinkButton>
                            </td>
                            <td width="15%"></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="center" width="100%">
                    <table width="100%" height="100%" class="tbl_Content">
                        <tr>
                            <td align="left">
                                <asp:Label ID="Label1" runat="server" ForeColor="blue" Font-Size="12px" Text="查詢功能："></asp:Label><asp:Label ID="lblFun" runat="server" ForeColor="blue" Font-Size="12px"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:100%">
                                <uc:PagerControl ID="pcMain" runat="server" GridViewName="gvMain" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 100%">
                                <asp:GridView ID="gvMain" runat="server" CssClass="GridViewStyle" AllowPaging="False" DataKeyNames="SysID,GroupID,CompID,FunID" AutoGenerateColumns="False" CellPadding="2" Width="100%" PageSize="15">
                                    <FooterStyle BackColor="#99CCCC" ForeColor="#003399" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="">
                                            <ItemStyle CssClass="td_detail" Height="15px" />
                                            <HeaderStyle CssClass="td_header" Width="3%" />
                                            <ItemTemplate>
                                                <asp:RadioButton ID="rdo_gvMain" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="CompID" HeaderText="授權公司" ReadOnly="True" SortExpression="CompID" Visible="False">
                                            <HeaderStyle Width="15%" CssClass="td_header" />
                                            <ItemStyle CssClass="td_detail" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CompName" HeaderText="授權公司" ReadOnly="True" SortExpression="CompName" >
                                            <HeaderStyle Width="8%" CssClass="td_header" />
                                            <ItemStyle CssClass="td_detail" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="GroupID" HeaderText="群組代碼" ReadOnly="True" SortExpression="GroupID" >
                                            <HeaderStyle Width="5%" CssClass="td_header" />
                                            <ItemStyle CssClass="td_detail" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="GroupName" HeaderText="組名" ReadOnly="True" SortExpression="GroupName" >
                                            <HeaderStyle Width="8%" CssClass="td_header" />
                                            <ItemStyle CssClass="td_detail" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="FunID" HeaderText="功能代碼" ReadOnly="True" SortExpression="FunID" >
                                            <HeaderStyle Width="5%" CssClass="td_header" />
                                            <ItemStyle CssClass="td_detail" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="FunName" HeaderText="功能名稱" ReadOnly="True" SortExpression="FunName" >
                                            <HeaderStyle Width="8%" CssClass="td_header" />
                                            <ItemStyle CssClass="td_detail" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="新增" SortExpression="RightA">
                                            <ItemStyle CssClass="td_detail" />
                                            <HeaderStyle CssClass="td_header" Width="6%" />
                                            <ItemTemplate>
                                                <asp:Image id="imgRightA" runat="server" ImageUrl="~/images/chkbox.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightA")="V","True","False") %>' />
                                                <asp:Image id="imgRightA_E" runat="server" ImageUrl="~/images/chkboxE.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightA")="","True","False") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="修改" SortExpression="RightU">
                                            <ItemStyle CssClass="td_detail" />
                                            <HeaderStyle CssClass="td_header" Width="6%" />
                                            <ItemTemplate>
                                                <asp:Image id="imgRightU" runat="server" ImageUrl="~/images/chkbox.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightU")="V","True","False") %>' />
                                                <asp:Image id="imgRightU_E" runat="server" ImageUrl="~/images/chkboxE.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightU")="","True","False") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="刪除" SortExpression="RightD">
                                            <ItemStyle CssClass="td_detail" />
                                            <HeaderStyle CssClass="td_header" Width="8%" />
                                            <ItemTemplate>
                                                <asp:Image id="imgRightD" runat="server" ImageUrl="~/images/chkbox.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightD")="V","True","False") %>' />
                                                <asp:Image id="imgRightD_E" runat="server" ImageUrl="~/images/chkboxE.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightD")="","True","False") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="查詢" SortExpression="RightI">
                                            <ItemStyle CssClass="td_detail" />
                                            <HeaderStyle CssClass="td_header" Width="6%" />
                                            <ItemTemplate>
                                                <asp:Image id="imgRightI" runat="server" ImageUrl="~/images/chkbox.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightI")="V","True","False") %>' />
                                                <asp:Image id="imgRightI_E" runat="server" ImageUrl="~/images/chkboxE.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightI")="","True","False") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="確認" SortExpression="RightC">
                                            <ItemStyle CssClass="td_detail" />
                                            <HeaderStyle CssClass="td_header" Width="6%" />
                                            <ItemTemplate>
                                                <asp:Image id="imgRightC" runat="server" ImageUrl="~/images/chkbox.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightC")="V","True","False") %>' />
                                                <asp:Image id="imgRightC_E" runat="server" ImageUrl="~/images/chkboxE.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightC")="","True","False") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="放行" SortExpression="RightC">
                                            <ItemStyle CssClass="td_detail" />
                                            <HeaderStyle CssClass="td_header" Width="6%" />
                                            <ItemTemplate>
                                                <asp:Image id="imgRightR" runat="server" ImageUrl="~/images/chkbox.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightR")="V","True","False") %>' />
                                                <asp:Image id="imgRightR_E" runat="server" ImageUrl="~/images/chkboxE.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightR")="","True","False") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="列印" SortExpression="RightP">
                                            <ItemStyle CssClass="td_detail" />
                                            <HeaderStyle CssClass="td_header" Width="6%" />
                                            <ItemTemplate>
                                                <asp:Image id="imgRightP" runat="server" ImageUrl="~/images/chkbox.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightP")="V","True","False") %>' />
                                                <asp:Image id="imgRightP_E" runat="server" ImageUrl="~/images/chkboxE.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightP")="","True","False") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="下傳" SortExpression="RightP">
                                            <ItemStyle CssClass="td_detail" />
                                            <HeaderStyle CssClass="td_header" Width="6%" />
                                            <ItemTemplate>
                                                <asp:Image id="imgRightL" runat="server" ImageUrl="~/images/chkbox.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightL")="V","True","False") %>' />
                                                <asp:Image id="imgRightL_E" runat="server" ImageUrl="~/images/chkboxE.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightL")="","True","False") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="關閉離開" SortExpression="RightX">
                                            <ItemStyle CssClass="td_detail" />
                                            <HeaderStyle CssClass="td_header" Width="8%" />
                                            <ItemTemplate>
                                                <asp:Image id="imgRightX" runat="server" ImageUrl="~/images/chkbox.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightX")="V","True","False") %>' />
                                                <asp:Image id="imgRightX_E" runat="server" ImageUrl="~/images/chkboxE.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightX")="","True","False") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
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
