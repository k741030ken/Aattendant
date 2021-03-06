<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SC0601.aspx.vb" Inherits="SC_SC0601" %>

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
                <td align="center">
                    <table width="80%" class="tbl_Edit" cellpadding="1" cellspacing="1">
                        <tr>
                            <td class="td_EditHeader" width="10%" align="center">
                                <asp:Label ID="lblSysID" runat="server" ForeColor="blue" Text="系統別"></asp:Label>
                            </td>
                            <td class="td_Edit" style="width: 30%" align="left">
                                <asp:Label ID="lblSysName" runat="server" CssClass="InputTextStyle_ReadOnly" MaxLength="50" Width="360px"></asp:Label>
                                <asp:Label ID="lblSysNameD" runat="server" CssClass="InputTextStyle_ReadOnly" MaxLength="50" Width="360px" Visible="false" ></asp:Label>
                            </td>                                       
                        </tr>
                        <tr>
                            <td class="td_EditHeader" width="10%" align="center">
                                <asp:Label ID="lblEmpID" runat="server" ForeColor="blue" Text="*使用者"></asp:Label>
                            </td>
                            <td class="td_Edit" style="width: 30%" align="left">
                                <%--<asp:Label ID="lblCompRoleNameU" runat="server" CssClass="InputTextStyle_ReadOnly" MaxLength="50" Width="360px"></asp:Label>--%>
                                <%--<asp:Label ID="lblCompRoleNameU" runat="server" Width="100px" CssClass="InputTextStyle_Thin"></asp:Label>
                                <asp:Label ID="lblUserID" runat="server" Width="100px" CssClass="InputTextStyle_Thin"></asp:Label>
                                <asp:Label ID="lblUserName" runat="server" Width="100px" CssClass="InputTextStyle_Thin"></asp:Label>
                                <asp:DropDownList ID="ddlCompRoleNameU" runat="server" CssClass="DropDownListStyle" AutoPostBack ="true" ></asp:DropDownList>
                                <asp:Label ID="lblCompRoleCompIDU" runat="server"  Visible ="false"></asp:Label>--%>
                                <asp:DropDownList ID="ddlUserComp" runat="server" AutoPostBack="true" Font-Names="細明體"></asp:DropDownList>
                                <asp:TextBox ID="txtUserID" CssClass="InputTextStyle_Thin" runat="server" Width="80px" Enabled="false" AutoPostBack="true" MaxLength="6" style="TEXT-TRANSFORM:uppercase"></asp:TextBox>
                                <asp:Label ID="lblUserName" runat="server"></asp:Label>
                                <uc:ButtonQuerySelectUserID ID="ucSelectEmpID" runat="server" ButtonText="..." ButtonHint="選擇人員..." WindowHeight="550" WindowWidth="500" />
                            </td>                         
                        </tr>
                        <tr>
                            <td class="td_EditHeader" width="10%" align="center">
                                <asp:Label ID="lblCompRoleNameH" runat="server" ForeColor="blue" Text="授權公司"></asp:Label>
                            </td>
                            <td class="td_Edit" style="width: 30%" align="left">
                                <asp:DropDownList ID="ddlCompRoleName" runat="server" CssClass="DropDownListStyle" AutoPostBack ="true" ></asp:DropDownList>
                                <asp:Label ID="lblCompRoleName" runat="server" CssClass="InputTextStyle_ReadOnly" MaxLength="50" Width="360px"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_EditHeader" width="10%" align="center">
                                <asp:Label ID="lblGroup" ForeColor="Blue" runat="server" Text="群組"></asp:Label>
                            </td>
                            <td class="td_Edit" style="width:30%" align="left">
                               <asp:DropDownList ID="ddlGroup" runat="server" CssClass="DropDownListStyle" AutoPostBack="true"></asp:DropDownList>
                            </td>                        
                        </tr>
                    </table>     
                </td>
            </tr>
            <tr>
                <td>
                    <uc:PagerControl ID="pcMain" runat="server" GridViewName="gvMain" />
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:GridView ID="gvMain" runat="server" CssClass="GridViewStyle" AllowPaging="False" DataKeyNames="FunID" AutoGenerateColumns="False" CellPadding="2" Width="100%" PageSize="15">
                        <FooterStyle BackColor="#99CCCC" ForeColor="#003399" />
                        <Columns>
                        <asp:BoundField DataField="CompRoleID" HeaderText="授權公司" ReadOnly="True" SortExpression="CompRoleID" Visible="False">
                                <HeaderStyle Width="0%" CssClass="td_header" />
                                <ItemStyle CssClass="td_detail" />
                            </asp:BoundField>
                            <asp:BoundField DataField="CompName" HeaderText="授權公司" ReadOnly="True" SortExpression="CompName" >
                                <HeaderStyle Width="10%" CssClass="td_header" />
                                <ItemStyle CssClass="td_detail" />
                            </asp:BoundField>
                            <asp:BoundField DataField="GroupID" HeaderText="群組代碼" ReadOnly="True" SortExpression="GroupID" >
                                <HeaderStyle Width="5%" CssClass="td_header" />
                                <ItemStyle CssClass="td_detail" />
                            </asp:BoundField>
                            <asp:BoundField DataField="GroupName" HeaderText="組名" ReadOnly="True" SortExpression="GroupName" >
                                <HeaderStyle Width="10%" CssClass="td_header" />
                                <ItemStyle CssClass="td_detail" />
                            </asp:BoundField>
                            <asp:BoundField DataField="FunID" HeaderText="功能代碼" ReadOnly="True" SortExpression="FunID" >
                                <HeaderStyle Width="5%" CssClass="td_header" />
                                <ItemStyle CssClass="td_detail" />
                            </asp:BoundField>
                            <asp:BoundField DataField="FunName" HeaderText="功能名稱" ReadOnly="True" SortExpression="FunName" >
                                <HeaderStyle Width="10%" CssClass="td_header" />
                                <ItemStyle CssClass="td_detail" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="新增" SortExpression="RightA">
                                <ItemStyle CssClass="td_detail" />
                                <HeaderStyle CssClass="td_header" Width="4%" />
                                <ItemTemplate>
                                    <asp:Image id="imgRightA" runat="server" ImageUrl="~/images/chkbox.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightA")="V","True","False") %>' />
                                    <asp:Image id="imgRightA_E" runat="server" ImageUrl="~/images/chkboxE.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightA")="","True","False") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="修改" SortExpression="RightU">
                                <ItemStyle CssClass="td_detail" />
                                <HeaderStyle CssClass="td_header" Width="4%" />
                                <ItemTemplate>
                                    <asp:Image id="imgRightU" runat="server" ImageUrl="~/images/chkbox.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightU")="V","True","False") %>' />
                                    <asp:Image id="imgRightU_E" runat="server" ImageUrl="~/images/chkboxE.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightU")="","True","False") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="刪除" SortExpression="RightD">
                                <ItemStyle CssClass="td_detail" />
                                <HeaderStyle CssClass="td_header" Width="4%" />
                                <ItemTemplate>
                                    <asp:Image id="imgRightD" runat="server" ImageUrl="~/images/chkbox.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightD")="V","True","False") %>' />
                                    <asp:Image id="imgRightD_E" runat="server" ImageUrl="~/images/chkboxE.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightD")="","True","False") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="查詢" SortExpression="RightI">
                                <ItemStyle CssClass="td_detail" />
                                <HeaderStyle CssClass="td_header" Width="4%" />
                                <ItemTemplate>
                                    <asp:Image id="imgRightI" runat="server" ImageUrl="~/images/chkbox.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightI")="V","True","False") %>' />
                                    <asp:Image id="imgRightI_E" runat="server" ImageUrl="~/images/chkboxE.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightI")="","True","False") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="執行" SortExpression="RightE">
                                <ItemStyle CssClass="td_detail" />
                                <HeaderStyle CssClass="td_header" Width="4%" />
                                <ItemTemplate>
                                    <asp:Image id="imgRightE" runat="server" ImageUrl="~/images/chkbox.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightE")="V","True","False") %>' />
                                    <asp:Image id="imgRightE_E" runat="server" ImageUrl="~/images/chkboxE.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightE")="","True","False") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="確認" SortExpression="RightC">
                                <ItemStyle CssClass="td_detail" />
                                <HeaderStyle CssClass="td_header" Width="4%" />
                                <ItemTemplate>
                                    <asp:Image id="imgRightC" runat="server" ImageUrl="~/images/chkbox.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightC")="V","True","False") %>' />
                                    <asp:Image id="imgRightC_E" runat="server" ImageUrl="~/images/chkboxE.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightC")="","True","False") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="列印" SortExpression="RightP">
                                <ItemStyle CssClass="td_detail" />
                                <HeaderStyle CssClass="td_header" Width="4%" />
                                <ItemTemplate>
                                    <asp:Image id="imgRightP" runat="server" ImageUrl="~/images/chkbox.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightP")="V","True","False") %>' />
                                    <asp:Image id="imgRightP_E" runat="server" ImageUrl="~/images/chkboxE.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightP")="","True","False") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="下傳" SortExpression="RightP">
                                <ItemStyle CssClass="td_detail" />
                                <HeaderStyle CssClass="td_header" Width="4%" />
                                <ItemTemplate>
                                    <asp:Image id="imgRightL" runat="server" ImageUrl="~/images/chkbox.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightL")="V","True","False") %>' />
                                    <asp:Image id="imgRightL_E" runat="server" ImageUrl="~/images/chkboxE.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightL")="","True","False") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="上傳" SortExpression="RightF">
                                <ItemStyle CssClass="td_detail" />
                                <HeaderStyle CssClass="td_header" Width="4%" />
                                <ItemTemplate>
                                    <asp:Image id="imgRightF" runat="server" ImageUrl="~/images/chkbox.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightF")="V","True","False") %>' />
                                    <asp:Image id="imgRightF_E" runat="server" ImageUrl="~/images/chkboxE.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightF")="","True","False") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="放行" SortExpression="RightC">
                                <ItemStyle CssClass="td_detail" />
                                <HeaderStyle CssClass="td_header" Width="4%" />
                                <ItemTemplate>
                                    <asp:Image id="imgRightR" runat="server" ImageUrl="~/images/chkbox.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightR")="V","True","False") %>' />
                                    <asp:Image id="imgRightR_E" runat="server" ImageUrl="~/images/chkboxE.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightR")="","True","False") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="複製" SortExpression="RightG">
                                <ItemStyle CssClass="td_detail" />
                                <HeaderStyle CssClass="td_header" Width="4%" />
                                <ItemTemplate>
                                    <asp:Image id="imgRightG" runat="server" ImageUrl="~/images/chkbox.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightG")="V","True","False") %>' />
                                    <asp:Image id="imgRightG_E" runat="server" ImageUrl="~/images/chkboxE.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightG")="","True","False") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="駁回" SortExpression="RightB">
                                <ItemStyle CssClass="td_detail" />
                                <HeaderStyle CssClass="td_header" Width="4%" />
                                <ItemTemplate>
                                    <asp:Image id="imgRightB" runat="server" ImageUrl="~/images/chkbox.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightB")="V","True","False") %>' />
                                    <asp:Image id="imgRightB_E" runat="server" ImageUrl="~/images/chkboxE.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightB")="","True","False") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="清除" SortExpression="RightX">
                                <ItemStyle CssClass="td_detail" />
                                <HeaderStyle CssClass="td_header" Width="4%" />
                                <ItemTemplate>
                                    <asp:Image id="imgRightX" runat="server" ImageUrl="~/images/chkbox.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightX")="V","True","False") %>' />
                                    <asp:Image id="imgRightX_E" runat="server" ImageUrl="~/images/chkboxE.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightX")="","True","False") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="保留" SortExpression="RightH">
                                <ItemStyle CssClass="td_detail" />
                                <HeaderStyle CssClass="td_header" Width="4%" />
                                <ItemTemplate>
                                    <asp:Image id="imgRightH" runat="server" ImageUrl="~/images/chkbox.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightH")="V","True","False") %>' />
                                    <asp:Image id="imgRightH_E" runat="server" ImageUrl="~/images/chkboxE.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightH")="","True","False") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="保留" SortExpression="RightJ">
                                <ItemStyle CssClass="td_detail" />
                                <HeaderStyle CssClass="td_header" Width="4%" />
                                <ItemTemplate>
                                    <asp:Image id="imgRightJ" runat="server" ImageUrl="~/images/chkbox.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightJ")="V","True","False") %>' />
                                    <asp:Image id="imgRightJ_E" runat="server" ImageUrl="~/images/chkboxE.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightJ")="","True","False") %>' />
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
    </form>
</body>
</html>
