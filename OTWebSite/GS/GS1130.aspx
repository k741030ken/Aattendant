<%@ Page Language="VB" AutoEventWireup="false" CodeFile="GS1130.aspx.vb" Inherits="GS_GS1130" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <link type="text/css" rel="stylesheet" href="../StyleSheet.Css" />
    <script type="text/javascript" src="../ClientFun/jquery-1.8.3.min.js"></script>
    <script type="text/javascript">
    <!--
        
        //        隱藏TR
        function hide_tr(strID) {
            var result_style = document.getElementById(strID).style; result_style.display = 'none';
        }
        //        顯示TR
        function show_tr(strID) {
            var result_style = document.getElementById(strID).style; result_style.display = 'table-row';
        }
        function ChangeValue(e) {
            alert(e.id);         
        }
    -->
    </script>
    <style type="text/css">
        .style1
        {
            height: 20px;
        }
    </style>
</head>
<body style="margin-top:5px; margin-left:5px; margin-right:5px; margin-bottom:0" >
    <form id="frmContent" runat="server">
        <ajaxToolkit:ToolkitScriptManager runat="Server" EnableScriptGlobalization="true"
            EnableScriptLocalization="true" ID="ScriptManager1" />
        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 100%">
            <tr>
                <td>            
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 100%">
                        <tr>
                            <td align="center">
                                <table width="80%" class="tbl_Edit" cellpadding="1" cellspacing="1">
                                    <tr style="height:20px">
                                        <td width="80%" align="left">
                                            <asp:Label ID="title" runat="server" Font-Names="微軟正黑體"></asp:Label>
                                            <asp:HiddenField ID="hidCompID" runat="server"></asp:HiddenField>                                            
                                            <asp:HiddenField ID="hidEmpID" runat="server"></asp:HiddenField>
                                            <asp:HiddenField ID="hidValidYear" runat="server"></asp:HiddenField>
                                        </td>
                                        <td width="20%" align="right">
                                            <!--<asp:Button ID="btnclose" Text="關閉" runat="server" />-->
                                        </td>
                                    </tr> 
                                    <tr>
                                        <td align="center" width="100%" colspan="2">
                                            <table width="100%" height="100%" class="tbl_Content">
                                                <tr>
                                                    <td style="width: 100%">
                                                        <asp:GridView ID="gvMain" runat="server" AllowSorting="False" AutoGenerateColumns="False" CssClass="GridViewStyle" CellPadding="2" DataKeyNames="ValidDate,DeptName,Kind,RecordCnt,Reason" Width="100%" PageSize="15" HeaderStyle-ForeColor="dimgray" Font-Names="微軟正黑體">
                                                            <FooterStyle BackColor="#99CCCC" ForeColor="#003399" Font-Names="微軟正黑體" />
                                                            <Columns>
                                                                <asp:BoundField DataField="ValidDate" HeaderText="日期" ReadOnly="True" SortExpression="ValidDate" >
                                                                    <HeaderStyle Width="10%" CssClass="td_header" Font-Names="微軟正黑體" />
                                                                    <ItemStyle CssClass="td_detail" Font-Names="微軟正黑體" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="DeptName" HeaderText="單位" ReadOnly="True" SortExpression="DeptName" >
                                                                    <HeaderStyle Width="12%" CssClass="td_header" Font-Names="微軟正黑體" />
                                                                    <ItemStyle CssClass="td_detail" HorizontalAlign="Left" Font-Names="微軟正黑體" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Kind" HeaderText="核定獎懲" ReadOnly="True" SortExpression="Kind" >
                                                                    <HeaderStyle Width="12%" CssClass="td_header" Font-Names="微軟正黑體" />
                                                                    <ItemStyle CssClass="td_detail" HorizontalAlign="Left" Font-Names="微軟正黑體" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="RecordCnt" HeaderText="獎懲次數" ReadOnly="True" SortExpression="RecordCnt" >
                                                                    <HeaderStyle Width="12%" CssClass="td_header" Font-Names="微軟正黑體" />
                                                                    <ItemStyle CssClass="td_detail" HorizontalAlign="Left" Font-Names="微軟正黑體" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Reason" HeaderText="獎懲事由" ReadOnly="True" SortExpression="Reason" >
                                                                    <HeaderStyle Width="54%" CssClass="td_header" Font-Names="微軟正黑體" />
                                                                    <ItemStyle CssClass="td_detail" HorizontalAlign="Left" Font-Names="微軟正黑體" />
                                                                </asp:BoundField>
                                                            </Columns>
                                                            <EmptyDataRowStyle CssClass="GridView_EmptyRowStyle" Font-Names="微軟正黑體" />
                                                            <EmptyDataTemplate>
                                                                <asp:Label ID="lblNoData" runat="server" Text="無資料顯示！" Font-Names="微軟正黑體"></asp:Label>
                                                            </EmptyDataTemplate>
                                                            <RowStyle CssClass="tr_evenline" Font-Names="微軟正黑體" />
                                                            <AlternatingRowStyle CssClass="tr_oddline" Font-Names="微軟正黑體" />
                                                            <SelectedRowStyle CssClass="GridView_SelectedRowStyle" BackColor="#b5efff" Font-Names="微軟正黑體" />
                                                            <PagerStyle CssClass="GridView_PagerStyle" Font-Names="微軟正黑體" />
                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
