<%@ Page Language="VB" AutoEventWireup="false" CodeFile="HR3010.aspx.vb" Inherits="HR_HR3010" %>

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
                case "btnAdd":
                case "btnDelete":
                    if (!hasSelectedRows(''))
                    {
                        alert("未選取資料列！");
                        return false;
                    }
            }

            switch(Param)
            {
                case "btnDelete":
                    if (!confirm('注意：確定刪除選取資料列？'))
                        return false;
                    break;
            }
        }

        function EntertoSubmit()
        {
//            if (window.event.keyCode == 13)
//            {
//                try
//                {
////                    window.parent.frames[0].document.getElementById("ucButtonPermission_btnQuery").click();
//                }
//                catch(ex)
//                {}
//            }
        }
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
                            <td align="left" width="5%">
                                <asp:Label ID="lblCompID" ForeColor="blue" Font-Size="15px" runat="server" Text="公司名稱："></asp:Label>
                            </td>
                            <td align="left" width="10%">
                                <asp:Label ID="lblCompRoleID" Font-Size="15px" runat="server" CssClass="InputTextStyle_Thin" Width="300px"></asp:Label>
                            </td>
                            <td align="left" width="5%">
                                <asp:Label ID="lblEmpID" Font-Size="15px" runat="server" Text="員工編號："></asp:Label>
                            </td>
                            <td align="left">
                                <asp:Label ID="lblName" Font-Size="15px" runat="server" CssClass="InputTextStyle_Thin" Width="300px"></asp:Label>
                            </td>
                            <td width="25%"></td>
                            <td width="25%"></td>
                        </tr>
                        <tr>
                            <td align="left" colspan="6">
                                <asp:Label ID="lblMsg1" Font-Size="15px" runat="server" Width="800px"></asp:Label>                                
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="6">
                                <asp:Label ID="lblMsg2" Font-Size="15px" runat="server" Text="若欲調整，該員工待異動生效時，將一併連動調整到【已生效的企業團經歷】" Width="800px"></asp:Label>                                
                            </td>
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
                                <asp:GridView ID="gvMain" runat="server" AllowPaging="False" AutoGenerateColumns="False" CellPadding="2" Width="100%" PageSize="15" CssClass="GridViewStyle" DataKeyNames="CompID,EmpID,Wait_ValidDate,Wait_Seq,IDNo,ModifyDate,Reason">
                                    <FooterStyle BackColor="#99CCCC" ForeColor="#003399" />
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemStyle CssClass="td_detail" Height="15px" />
                                            <HeaderStyle CssClass="td_header" Width="5%" />
                                            <HeaderTemplate>
                                                <uc:ucGridViewChoiceAll ID="ucGridViewChoiceAll" CheckBoxName="chk_gvMain" HeaderText="全選" runat="server"  />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chk_gvMain" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                         <asp:BoundField DataField="ExitEmployeeLogWait" HeaderText="是否連動調整" ReadOnly="True" SortExpression="ExitEmployeeLogWait">
                                            <HeaderStyle Width="3%" CssClass="td_header" />
                                            <ItemStyle CssClass="td_detail" />
                                        </asp:BoundField>
                                        <asp:ButtonField Text="明細" HeaderText="明細" CommandName="Detail" ItemStyle-Width ="3%" ItemStyle-Font-Size ="12px" 
                                            HeaderStyle-CssClass ="td_header"  ItemStyle-CssClass ="td_detail" />
                                        <asp:BoundField DataField="ModifyDateShow" HeaderText="企業團生效日期" ReadOnly="True" SortExpression="ModifyDate">
                                            <HeaderStyle Width="5%" CssClass="td_header" />
                                            <ItemStyle CssClass="td_detail" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ReasonName" HeaderText="異動原因" ReadOnly="True" SortExpression="ReasonName">
                                            <HeaderStyle Width="5%" CssClass="td_header" />
                                            <ItemStyle CssClass="td_detail" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Company" HeaderText="異動後公司名稱" ReadOnly="True" SortExpression="Company">
                                            <HeaderStyle Width="10%" CssClass="td_header"/>
                                            <ItemStyle CssClass="td_detail" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="DeptName" HeaderText="異動後部門" ReadOnly="True" SortExpression="DeptName">
                                            <HeaderStyle Width="10%" CssClass="td_header"/>
                                            <ItemStyle CssClass="td_detail" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="OrganName" HeaderText="異動後科組課" ReadOnly="True" SortExpression="OrganName">
                                            <HeaderStyle Width="10%" CssClass="td_header" />
                                            <ItemStyle CssClass="td_detail" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Position" HeaderText="異動後職位" ReadOnly="True" SortExpression="Position">
                                            <HeaderStyle Width="10%" CssClass="td_header" />
                                            <ItemStyle CssClass="td_detail" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="WorkType" HeaderText="異動後工作性質" ReadOnly="True" SortExpression="WorkType">
                                            <HeaderStyle Width="10%" CssClass="td_header" />
                                            <ItemStyle CssClass="td_detail" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="TitleName" HeaderText="異動後職稱" ReadOnly="True" SortExpression="TitleName">
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
