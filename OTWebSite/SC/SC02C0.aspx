<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SC02C0.aspx.vb" Inherits="SC_SC02C0" %>

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
        <asp:ScriptManager ID="MyScriptManager" runat="server"></asp:ScriptManager>
        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 100%">
            <tr>
                <td align="center" style="height: 30px;">
                    <table cellpadding="1" cellspacing="1" border="0" class="tbl_Condition" height="100%" width="100%">
                        <tr>
                            <td width="25%"></td>
                            <td align="right">
                                <asp:Label ID="lblUserID" Font-Size="15px" runat="server" Text="員工：" CssClass="MustInputCaption"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:UpdatePanel ID="udpUser" runat="server" RenderMode="Inline">
                                    <ContentTemplate>
                                        <uc:OneUserSelect ID="ousUser" runat="server" DeptType="Dept" DisplayType="full" MustSelect="True" UserType="validuser" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
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
                            <td style="width: 100%">
                                <asp:GridView ID="gvMain" CssClass="GridViewStyle" runat="server" AllowPaging="False" AutoGenerateColumns="False" DataSourceID="sdsMain" CellPadding="2" Width="100%">
                                    <FooterStyle BackColor="#99CCCC" ForeColor="#003399" />
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemStyle CssClass="td_detail" Height="15px" />
                                            <HeaderStyle CssClass="td_header" Width="5%" />
                                            <ItemTemplate>
                                                <asp:RadioButton ID="rdo_gvMain" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="員工" SortExpression="UserID">
                                            <ItemStyle CssClass="td_detail" />
                                            <HeaderStyle CssClass="td_header" Width="20%" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblUserID" runat="server" Text='<%# Eval("UserID") %>'></asp:Label>-
                                                <asp:Label ID="lblUserName" runat="server" Text='<%# Eval("UserName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="代理人" SortExpression="AgentUserID">
                                            <ItemStyle CssClass="td_detail" />
                                            <HeaderStyle CssClass="td_header" Width="20%" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblAgentUserID" runat="server" Text='<%# Eval("AgentUserID") %>'></asp:Label>-
                                                <asp:Label ID="lblAgentUserName" runat="server" Text='<%# Eval("AgentUserName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="代理種類" SortExpression="AgencyType">
                                            <ItemStyle CssClass="td_detail" />
                                            <HeaderStyle CssClass="td_header" Width="15%" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblAgencyType" runat="server" Text='<%# IIf(Eval("AgencyType")="0","待辦事項","使用權限") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="代理起日" SortExpression="ValidFrom">
                                            <ItemStyle CssClass="td_detail" />
                                            <HeaderStyle CssClass="td_header" Width="15%" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblValidFrom" runat="server" Text='<%# Bind("ValidFrom", "{0:yyyy/MM/dd}") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="代理迄日" SortExpression="ValidTo">
                                            <ItemStyle CssClass="td_detail" />
                                            <HeaderStyle CssClass="td_header" Width="15%" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblValidTo" runat="server" Text='<%# Bind("ValidTo", "{0:yyyy/MM/dd}") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="有效註記" SortExpression="ValidFlag">
                                            <ItemStyle CssClass="td_detail" />
                                            <HeaderStyle CssClass="td_header" Width="10%" />
                                            <ItemTemplate>
                                                <asp:Image ID="imgValidFlag" runat="server" ImageUrl="~/images/chkbox.gif" Visible='<%# iif(Databinder.Eval(Container.DataItem, "ValidFlag")="1","True","False") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataRowStyle CssClass="GridView_EmptyRowStyle" />
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblNoData" runat="server" Text="無資料顯示！"></asp:Label>
                                    </EmptyDataTemplate>
                                    <RowStyle CssClass="tr_evenline" />
                                    <AlternatingRowStyle CssClass="tr_oddline" />
                                    <SelectedRowStyle CssClass="GridView_SelectedRowStyle" BackColor="Moccasin" />
                                    <PagerStyle CssClass="GridView_PagerStyle" />
                                    <PagerSettings Position="Top" />
                                </asp:GridView>
                                <asp:SqlDataSource ID="sdsMain" runat="server" ConnectionString="<%$ ConnectionStrings:DB_CCNJ %>">
                                </asp:SqlDataSource>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
