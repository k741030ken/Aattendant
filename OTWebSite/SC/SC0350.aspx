<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SC0350.aspx.vb" Inherits="SC_SC0350" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title></title>
    <link type="text/css" rel="stylesheet" href="/StyleSheet.Css" />
    <script type="text/javascript">
    <!--
        function funAction(Param) {
            switch (Param) {
                case "btnUpdate":
                case "btnDelete":
                    if (!hasSelectedRow('')) {
                        alert("未選取資料列！");
                        return false;
                    }
            }

            switch (Param) {
                case "btnDelete":
                    if (!confirm('確定刪除此筆資料？'))
                        return false;
                    break;
            }
        }

        function EntertoSubmit() {
            if (window.event.keyCode == 13) {
                try {
                    window.parent.frames[0].document.getElementById("ucButtonPermission_btnQuery").click();
                }
                catch (ex)
                { }
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
                            <td width="15%"></td>
                            <td align="right">
                                <asp:Label ID="lblBankID" Font-Size="15px" runat="server" Text="銀行代號：" CssClass="MustInputCaption"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:TextBox CssClass="InputTextStyle_Thin" ID="txtBankID" runat="server"></asp:TextBox>
                            </td>
                            <td align="right">
                                <asp:Label ID="lblBankName" Font-Size="15px" runat="server" Text="銀行名稱：" CssClass="MustInputCaption"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:TextBox CssClass="InputTextStyle_Thin" ID="txtBankName" runat="server"></asp:TextBox>
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
                                <asp:GridView ID="gvMain" CssClass="GridViewStyle" runat="server" AllowPaging="True" AutoGenerateColumns="False" DataSourceID="sdsMain" CellPadding="2" DataKeyNames="" Width="100%" PageSize="15">
                                    <FooterStyle BackColor="#99CCCC" ForeColor="#003399" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="">
                                            <ItemStyle CssClass="td_detail" Height="15px" />
                                            <HeaderStyle CssClass="td_header" Width="5%" />
                                            <ItemTemplate>
                                                <asp:RadioButton ID="rdo_gvMain" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="BankID" HeaderText="銀行代號" ReadOnly="True" SortExpression="BankID" >
                                            <HeaderStyle Width="8%" CssClass="td_header" />
                                            <ItemStyle CssClass="td_detail" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="BankName" HeaderText="銀行名稱" ReadOnly="True" SortExpression="BankName" >
                                            <HeaderStyle Width="50%" CssClass="td_header" />
                                            <ItemStyle CssClass="td_detail" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="WorldRank" HeaderText="世界排名" ReadOnly="True" SortExpression="WorldRank" >
                                            <HeaderStyle Width="15%" CssClass="td_header" />
                                            <ItemStyle CssClass="td_detail" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="MoodyGrade" HeaderText="Moody's / S&P" ReadOnly="True" SortExpression="MoodyGrade" >
                                            <HeaderStyle Width="22%" CssClass="td_header" />
                                            <ItemStyle CssClass="td_detail" />
                                        </asp:BoundField>
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
