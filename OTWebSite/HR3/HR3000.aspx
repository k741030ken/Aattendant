<%@ Page Language="VB" AutoEventWireup="false" CodeFile="HR3000.aspx.vb" Inherits="HR_HR3000" %>

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
                case "btnExecutes":
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
    <style type="text/css">
        .style1
        {
            height: 37px;
        }
    </style>
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
                            <td width="5%" class="style1"></td>
                            <td align="left" class="style1">
                                <asp:Label ID="lblCompID" ForeColor="blue" Font-Size="15px" runat="server" Text="公司代碼："></asp:Label>
                            </td>
                            <td align="left" class="style1">
                                <asp:DropDownList ID="ddlCompID" runat="server" AutoPostBack="true" ></asp:DropDownList>
                                <asp:Label ID="lblCompRoleID" Font-Size="15px" runat="server" CssClass="InputTextStyle_Thin" Width="200px"></asp:Label>
                                <uc:Release ID="ucRelease" runat="server" WindowHeight="350" WindowWidth="350" style="display:none" />
                                <asp:Label ID="lblReleaseResult" runat="server" ForeColor="Blue" Text="" style="display:none"></asp:Label>
                                <asp:HiddenField ID="IsDoQuery" runat="server"></asp:HiddenField>
                            </td>
                            <td align="left" class="style1">
                                <asp:CheckBox ID="chkValidOrNot" runat="server" />
                                <asp:Label ID="lblValidOrNot" Font-Size="15px" runat="server" Text="狀態："></asp:Label>
                            </td>
                            <td align="left" class="style1">
                                <asp:DropDownList ID="ddlValidOrNot" runat="server" AutoPostBack="true" >
                                    <asp:ListItem Selected="True" Value="0">未生效</asp:ListItem>
                                    <asp:ListItem Value="1">已生效</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td width="5%" class="style1"></td>
                        </tr>
                        <tr>
                            <td width="5%"></td>
                            <td align="left">
                                <asp:CheckBox ID="chkEmpID" runat="server" />
                                <asp:Label ID="lblEmpID" Font-Size="15px" runat="server" Text="員工編號："></asp:Label>
                            </td>
                            <td align="left">
                                <asp:TextBox CssClass="InputTextStyle_Thin" ID="txtEmpID" MaxLength="6" runat="server"></asp:TextBox>
                                <uc:ButtonQuerySelectUserID ID="ucQueryEmp" runat="server" WindowHeight="800" WindowWidth="600" ButtonText="..." />
                            </td>
                            <td align="left">
                                <asp:CheckBox ID="chkName" runat="server" />
                                <asp:Label ID="lblName" Font-Size="15px" runat="server" Text="員工姓名："></asp:Label>
                            </td>
                            <td align="left">
                                <asp:TextBox CssClass="InputTextStyle_Thin" ID="txtName" MaxLength="12" runat="server"></asp:TextBox>
                            </td>
                            <td width="5%"></td>
                        </tr>
                        <tr>
                            <td width="5%"></td>
                            <td align="left">
                                <asp:CheckBox ID="chkValidDate" runat="server" />
                                <asp:Label ID="lblValidDate" Font-Size="15px" runat="server" Text="生效日期："></asp:Label>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtValidDateB" CssClass="InputTextStyle_Thin" runat="server" MaxLength="10"></asp:TextBox>
                                <asp:ImageButton runat="Server" ID="imgValidDateB" ImageUrl="~/images/Calendar.gif" AlternateText="Click to show calendar" />
                                <ajaxToolkit:CalendarExtender ID="CalendarValidDateB" runat="server" TargetControlID="txtValidDateB" PopupButtonID="imgValidDateB" Format="yyyy/MM/dd"  />
                                <asp:Label ID="Label1" ForeColor="blue" runat="server" Text="～"></asp:Label>
                                <asp:TextBox ID="txtValidDateE" CssClass="InputTextStyle_Thin" runat="server" MaxLength="10"></asp:TextBox>
                                <asp:ImageButton runat="Server" ID="imgValidDateE" ImageUrl="~/images/Calendar.gif" AlternateText="Click to show calendar" />
                                <ajaxToolkit:CalendarExtender ID="CalendarValidDateE" runat="server" TargetControlID="txtValidDateE" PopupButtonID="imgValidDateE" Format="yyyy/MM/dd" />                                
                            </td>
                            <td align="left">
                                <asp:CheckBox ID="chkReason" runat="server" />
                                <asp:Label ID="lblReason" Font-Size="15px" runat="server" Text="異動原因："></asp:Label>
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="ddlReason" runat="server" AutoPostBack="true" ></asp:DropDownList>
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
                                <asp:GridView ID="gvMain" runat="server" AllowPaging="False" AutoGenerateColumns="False" CellPadding="2" Width="100%" PageSize="15" CssClass="GridViewStyle" DataKeyNames="CompID,EmpID,ValidDate,Seq,NameN,ValidMark">
                                    <FooterStyle BackColor="#99CCCC" ForeColor="#003399" />
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemStyle CssClass="td_detail" Height="15px" />
                                            <HeaderStyle CssClass="td_header" Width="2%" />
                                            <HeaderTemplate>
                                                <uc:ucGridViewChoiceAll ID="ucGridViewChoiceAll" CheckBoxName="chk_gvMain" HeaderText="全選" runat="server"  />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chk_gvMain" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        
                                        <asp:ButtonField Text="明細" HeaderText="明細" CommandName="Detail" ItemStyle-Width ="2%" ItemStyle-Font-Size ="12px" 
                                            HeaderStyle-CssClass ="td_header"  ItemStyle-CssClass ="td_detail" />

                                        <asp:CheckBoxField DataField="ValidMark" HeaderText="生效" 
                                            InsertVisible="False" ReadOnly="True" SortExpression="ValidMark">
                                            <HeaderStyle CssClass="td_header" Width="2%" />
                                            <ItemStyle CssClass="td_detail" />
                                        </asp:CheckBoxField>
                                        <asp:BoundField DataField="CompName" HeaderText="公司名稱" ReadOnly="True" SortExpression="CompName">
                                            <HeaderStyle Width="8%" CssClass="td_header" />
                                            <ItemStyle CssClass="td_detail" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="EmpID" HeaderText="員工編號" ReadOnly="True" SortExpression="EmpID">
                                            <HeaderStyle Width="3%" CssClass="td_header" />
                                            <ItemStyle CssClass="td_detail" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="NameN" HeaderText="姓名" ReadOnly="True" SortExpression="NameN">
                                            <HeaderStyle Width="6%" CssClass="td_header" />
                                            <ItemStyle CssClass="td_detail" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ValidDate" HeaderText="生效日期" ReadOnly="True" SortExpression="ValidDate">
                                            <HeaderStyle Width="5%" CssClass="td_header" />
                                            <ItemStyle CssClass="td_detail" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ReasonName" HeaderText="異動原因" ReadOnly="True" SortExpression="ReasonName">
                                            <HeaderStyle Width="3%" CssClass="td_header" />
                                            <ItemStyle CssClass="td_detail" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="NewCompName" HeaderText="異動後公司名稱" ReadOnly="True" SortExpression="NewCompName">
                                            <HeaderStyle Width="8%" CssClass="td_header"/>
                                            <ItemStyle CssClass="td_detail" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="NewGroupName" HeaderText="異動後事業群" ReadOnly="True" SortExpression="NewGroupName">
                                            <HeaderStyle Width="10%" CssClass="td_header"/>
                                            <ItemStyle CssClass="td_detail" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="NewDeptName" HeaderText="異動後部門" ReadOnly="True" SortExpression="NewDeptName">
                                            <HeaderStyle Width="10%" CssClass="td_header"/>
                                            <ItemStyle CssClass="td_detail" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="NewOrganName" HeaderText="異動後科組課" ReadOnly="True" SortExpression="NewOrganName">
                                            <HeaderStyle Width="10%" CssClass="td_header" />
                                            <ItemStyle CssClass="td_detail" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="LastChgCompName" HeaderText="最後異動公司" ReadOnly="True" SortExpression="LastChgComp">
                                            <HeaderStyle Width="8%" CssClass="td_header" />
                                            <ItemStyle CssClass="td_detail" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="LastChgName" HeaderText="最後異動者" ReadOnly="True" SortExpression="LastChgID">
                                            <HeaderStyle Width="6%" CssClass="td_header" />
                                            <ItemStyle CssClass="td_detail" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="LastChgDate" HeaderText="最後異動日期" ReadOnly="True" SortExpression="LastChgDate">
                                            <HeaderStyle Width="5%" CssClass="td_header" />
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
