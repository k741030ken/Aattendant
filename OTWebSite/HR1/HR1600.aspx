<%@ Page Language="VB" AutoEventWireup="false" CodeFile="HR1600.aspx.vb" Inherits="HR_HR1600" %>

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
                case "btnActionC":
                    if (!hasSelectedRow(''))
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
                <td>
                    <asp:RadioButton ID="rbEmpAdditionDetail" GroupName="rbnEmpAdditionType" 
                        runat="server" Text="員工調兼資料" Checked="True" AutoPostBack="true" />
                    <asp:RadioButton ID="rbEmpAddition" GroupName="rbnEmpAdditionType" runat="server" Text="員工調兼現況" AutoPostBack="true" />
                </td>
            </tr>            
            <tr>
                <td align="center" style="height: 30px;">
                    <table cellpadding="1" cellspacing="1" border="0" class="tbl_Condition" height="100%" width="100%">
                        <tr>
                            <td width="5%"></td>
                            <td align="left">
                                <asp:Label ID="lblCompID" ForeColor="blue" Font-Size="15px" runat="server" Text="現任公司："></asp:Label>
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="ddlCompID" runat="server" AutoPostBack="true" Font-Names="細明體"></asp:DropDownList>
                                <asp:Label ID="lblCompRoleID" Font-Size="15px" runat="server" CssClass="InputTextStyle_Thin" Width="200px"></asp:Label>
                                <asp:HiddenField ID="IsDoQuery" runat="server"></asp:HiddenField>
                            </td>
                            <td align="left">
                                <asp:CheckBox ID="chkDeptID" runat="server" />
                                <asp:Label ID="lblDeptID" Font-Size="15px" runat="server" Text="現任部門："></asp:Label>
                            </td>
                            <td align="left">                                
                                <asp:UpdatePanel ID="UpdDeptID" runat="server" ChildrenAsTriggers="False" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="ddlDeptID" runat="server" Font-Names="細明體"></asp:DropDownList>
                                    </ContentTemplate>                                                          
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="ddlCompID" />
                                    </Triggers>
                                </asp:UpdatePanel>   
                            </td>
                            <td width="5%"></td>
                        </tr>
                        <tr>
                            <td width="5%"></td>
                            <td align="left">
                                <asp:CheckBox ID="chkEmpID" runat="server" />
                                <asp:Label ID="lblEmpID" Font-Size="15px" runat="server" Text="員工編號："></asp:Label>
                            </td>
                            <td align="left">
                                <asp:TextBox CssClass="InputTextStyle_Thin" ID="txtEmpID" MaxLength="6" runat="server"></asp:TextBox>
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
                                <asp:CheckBox ID="chkReason" runat="server" />
                                <asp:Label ID="lblReason" Font-Size="15px" runat="server" Text="狀態："></asp:Label>
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="ddlReason" runat="server" Font-Names="細明體"></asp:DropDownList>
                            </td>
                            <td align="left">
                                <asp:CheckBox ID="chkValidDate" runat="server" />
                                <asp:Label ID="lblValidDate" Font-Size="15px" runat="server" Text="生效日期："></asp:Label>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtValidDateB" CssClass="InputTextStyle_Thin" runat="server" MaxLength="10"></asp:TextBox>
                                <asp:ImageButton runat="Server" ID="imgValidDateB" ImageUrl="~/images/Calendar.gif" AlternateText="Click to show calendar" />
                                <ajaxToolkit:CalendarExtender ID="CalendarValidDateB" runat="server" TargetControlID="txtValidDateB" PopupButtonID="imgValidDateB" Format="yyyy/MM/dd" />
                                <asp:Label ID="Label1" ForeColor="blue" runat="server" Text="～"></asp:Label>
                                <asp:TextBox ID="txtValidDateE" CssClass="InputTextStyle_Thin" runat="server" MaxLength="10"></asp:TextBox>
                                <asp:ImageButton runat="Server" ID="imgValidDateE" ImageUrl="~/images/Calendar.gif" AlternateText="Click to show calendar" />
                                <ajaxToolkit:CalendarExtender ID="CalendarValidDateE" runat="server" TargetControlID="txtValidDateE" PopupButtonID="imgValidDateE" Format="yyyy/MM/dd" />
                            </td>
                            <td width="5%"></td>
                        </tr>
                        <tr>
                            <td width="5%"></td>
                            <td align="left">
                                <asp:CheckBox ID="chkAdditionCompID" runat="server" />
                                <asp:Label ID="lblAdditionCompID" ForeColor="blue" Font-Size="15px" runat="server" Text="兼任公司："></asp:Label>
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="ddlAdditionCompID" runat="server" Font-Names="細明體" 
                                    AutoPostBack="True"></asp:DropDownList>
                                          
                            </td>
                            <td align="left">
                                <asp:CheckBox ID="chkAdditionDeptID" runat="server" />
                                <asp:Label ID="lblAdditionDeptID" Font-Size="15px" runat="server" Text="兼任部門："></asp:Label>
                            </td>
                            <td align="left">
                                <asp:UpdatePanel ID="UpdAdditionDeptID" runat="server" ChildrenAsTriggers="False" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="ddlAdditionDeptID" runat="server" Font-Names="細明體"></asp:DropDownList>
                                    </ContentTemplate>                                                          
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="ddlAdditionCompID" />
                                    </Triggers>
                                </asp:UpdatePanel>                                
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
                                <asp:GridView ID="gvMain" runat="server" AutoGenerateColumns="False" 
                                    CellPadding="2" Width="100%" PageSize="15" CssClass="GridViewStyle" 
                                    DataKeyNames="CompID,EmpID,ValidDate,AddCompID,AddDeptID,AddOrganID">
                                    <FooterStyle BackColor="#99CCCC" ForeColor="#003399" />
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemStyle CssClass="td_detail" Height="15px" />
                                            <HeaderStyle CssClass="td_header" Width="5%" />
                                            <ItemTemplate>
                                                <asp:RadioButton ID="rdo_gvMain" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        
                                        <asp:TemplateField HeaderText="明細" ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lbtnDetail" runat="server" CausesValidation="false" 
                                                    CommandName="Detail" Text="明細"></asp:LinkButton>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="td_header" />
                                            <ItemStyle CssClass="td_detail" Font-Size="12px" Width="3%" />
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="AddCompName" HeaderText="調兼公司" ReadOnly="True" SortExpression="AddCompName">
                                            <HeaderStyle Width="10%" CssClass="td_header"/>
                                            <ItemStyle CssClass="td_detail" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="AddDeptName" HeaderText="調兼部門" ReadOnly="True" SortExpression="AddDeptName">
                                            <HeaderStyle Width="10%" CssClass="td_header"/>
                                            <ItemStyle CssClass="td_detail" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="AddOrganName" HeaderText="調兼科組課" ReadOnly="True" SortExpression="AddOrganName">
                                            <HeaderStyle Width="10%" CssClass="td_header" />
                                            <ItemStyle CssClass="td_detail" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CompName" HeaderText="現任公司" ReadOnly="True" SortExpression="CompName">
                                            <HeaderStyle Width="10%" CssClass="td_header" />
                                            <ItemStyle CssClass="td_detail" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="DeptName" HeaderText="現任部門" ReadOnly="True" SortExpression="DeptName">
                                            <HeaderStyle Width="10%" CssClass="td_header"/>
                                            <ItemStyle CssClass="td_detail" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="OrganName" HeaderText="現任科組課" ReadOnly="True" SortExpression="OrganName">
                                            <HeaderStyle Width="10%" CssClass="td_header" />
                                            <ItemStyle CssClass="td_detail" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="EmpID" HeaderText="員工編號" ReadOnly="True" SortExpression="EmpID">
                                            <HeaderStyle Width="5%" CssClass="td_header" />
                                            <ItemStyle CssClass="td_detail" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="NameN" HeaderText="員工姓名" ReadOnly="True" SortExpression="NameN">
                                            <HeaderStyle Width="8%" CssClass="td_header" />
                                            <ItemStyle CssClass="td_detail" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ReasonName" HeaderText="狀態" ReadOnly="True" SortExpression="ReasonName">
                                            <HeaderStyle Width="5%" CssClass="td_header" />
                                            <ItemStyle CssClass="td_detail" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ValidDate" HeaderText="生效日期" ReadOnly="True" SortExpression="ValidDate">
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
