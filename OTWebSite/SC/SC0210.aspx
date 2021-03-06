<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SC0210.aspx.vb" Inherits="SC_SC0210" %>

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
        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 100%">
            <tr>
                <td align="center" style="height: 30px;">
                    <table cellpadding="1" cellspacing="1" border="0" class="tbl_Condition" height="100%" width="100%">
                        <tr>
                            <td width="5%"></td>
                            <td align="right">
                                <asp:Label ID="lblOrganID" Font-Size="15px" runat="server" Text="部門代碼："></asp:Label>
                            </td>
                            <td align="left">
                                <asp:TextBox CssClass="InputTextStyle_Thin" ID="txtOrganID" runat="server" style="text-transform:uppercase"></asp:TextBox>
                            </td>
                            <td align="right">
                                <asp:Label ID="lblOrganName" Font-Size="15px" runat="server" Text="部門名稱："></asp:Label>
                            </td>
                            <td align="left">
                                <asp:TextBox CssClass="InputTextStyle_Thin" ID="txtOrganName" runat="server"></asp:TextBox>
                            </td>
                            <td align="right">
                                <asp:Label ID="Label1" Font-Size="15px" runat="server" Text="部門類別："></asp:Label>
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="ddlOrganType" runat="server" Font-Size="12px" Width="115px">
                                    <asp:ListItem Value="All" Text="所有部門" Selected="true"></asp:ListItem>
                                    <asp:ListItem Value="Dept" Text="部門"></asp:ListItem>
                                    <asp:ListItem Value="Branch" Text="分行單位"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td width="5%"></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="center" width="100%">
                    <table width="100%" height="100%" class="tbl_Content">
                        <tr>
                            <td style="width: 100%">
                                <uc:PagerControl ID="pcMain" runat="server" GridViewName="gvMain" />
                            </td> 
                        </tr>
                        <tr>
                            <td style="width: 100%">
                                <asp:GridView ID="gvMain" runat="server" AllowPaging="False" AutoGenerateColumns="False" CellPadding="2" Width="100%" PageSize="15" CssClass="GridViewStyle" DataKeyNames="OrganID" >
                                    <FooterStyle BackColor="#99CCCC" ForeColor="#003399" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="">
                                            <ItemStyle CssClass="td_detail" Height="15px" />
                                            <HeaderStyle CssClass="td_header" Width="6%" />
                                            <ItemTemplate>
                                                <asp:RadioButton ID="rdo_gvMain" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="OrganID" HeaderText="部門代碼" ReadOnly="True" SortExpression="OrganID" >
                                            <HeaderStyle Width="8%" CssClass="td_header" />
                                            <ItemStyle CssClass="td_detail" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="OrganName" HeaderText="部門名稱" ReadOnly="True" SortExpression="OrganName" >
                                            <HeaderStyle Width="20%" CssClass="td_header" />
                                            <ItemStyle CssClass="td_detail" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="部門類別" SortExpression="OrganType">
                                            <ItemStyle CssClass="td_detail" />
                                            <HeaderStyle CssClass="td_header" Width="8%" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblOrganType" runat="server" Text='<%# iif(DataBinder.Eval(Container.DataItem, "OrganType")="1","部門","科組課") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="BranchNo" HeaderText="分行代碼" ReadOnly="True" SortExpression="BranchNo" >
                                            <HeaderStyle Width="7%" CssClass="td_header" />
                                            <ItemStyle CssClass="td_detail" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="DeptID" HeaderText="所屬單位" ReadOnly="True" SortExpression="DeptID" >
                                            <HeaderStyle Width="9%" CssClass="td_header" />
                                            <ItemStyle CssClass="td_detail" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="UpOrganID" HeaderText="上階部門" ReadOnly="True" SortExpression="UpOrganID" >
                                            <HeaderStyle Width="9%" CssClass="td_header" />
                                            <ItemStyle CssClass="td_detail" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="分行註記" SortExpression="BranchFlag">
                                            <ItemStyle CssClass="td_detail" />
                                            <HeaderStyle CssClass="td_header" Width="6%" />
                                            <ItemTemplate>
                                                <asp:Image ID="imgBranchFlag" runat="server" ImageUrl="~/images/chkbox.gif" Visible='<%# iif(DataBinder.Eval(Container.DataItem, "BranchFlag")="1","true","false") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="有效註記" SortExpression="InValidFlag">
                                            <ItemStyle CssClass="td_detail" />
                                            <HeaderStyle CssClass="td_header" Width="6%" />
                                            <ItemTemplate>
                                                <asp:Image ID="imgInValidFlag" runat="server" ImageUrl="~/images/chkbox.gif" Visible='<%# iif(DataBinder.Eval(Container.DataItem, "InValidFlag")="0","true","false") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="異動註記" SortExpression="UpdateFlag">
                                            <ItemStyle CssClass="td_detail" />
                                            <HeaderStyle CssClass="td_header" Width="6%" />
                                            <ItemTemplate>
                                                <asp:Image ID="imgUpdateFlag" runat="server" ImageUrl="~/images/chkbox.gif" Visible='<%# iif(DataBinder.Eval(Container.DataItem, "UpdateFlag")="1","true","false") %>' />
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
