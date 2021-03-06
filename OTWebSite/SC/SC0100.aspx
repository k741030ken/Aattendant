<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SC0100.aspx.vb" Inherits="SC_SC0100" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title></title>
    <link type="text/css" rel="stylesheet" href="../StyleSheet.Css" />
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

        //另開員工Pop視窗
        function funCallSelectEmp() {
            var obj = document.getElementById('ucSelecEmp_btnSelect');
            if (obj == null)
                alert('Can not find object!');
            else {
                obj.click();
            }
        }
       -->
    </script>
</head>

<body style="margin-top:5px; margin-left:5px; margin-right:5px; margin-bottom:0" >
    <form id="frmContent" runat="server">
    <ajaxToolkit:ToolkitScriptManager runat="Server" EnableScriptGlobalization="true"
        EnableScriptLocalization="true" ID="ScriptManager1" />
    <%--<tr>    
                <td align="center" width="100%">
                <table width="100%" class="tbl_Edit" cellpadding="1" cellspacing="1">
                <td class="td_EditHeader" width="15%" align="center">
                                <asp:Label ID="lblSysID" runat="server" ForeColor="blue" Text="系統別"></asp:Label>
                </td>
                <td class="td_Edit" style="width: 35%" align="left">
                                <asp:Label ID="lblSysName" runat="server" CssClass="InputTextStyle_ReadOnly" MaxLength="50" Width="360px"></asp:Label>
                                <asp:Label ID="lblSysNameD" runat="server" CssClass="InputTextStyle_ReadOnly" MaxLength="50" Width="360px" Visible="false" ></asp:Label>
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
        </tr>--%>
        <table width="100%" class="tbl_Condition" cellpadding="1" cellspacing="1">
            <tr>
                <td width="5%"></td>
                <td align="left">
                    <asp:Label ID="lblSysID" ForeColor="blue" Font-Size="15px" runat="server" Text="系統別："></asp:Label>
                </td>
                <td align="left">
                    <asp:Label ID="lblSysName" runat="server" Font-Size="15px" CssClass="InputTextStyle_Thin" MaxLength="50" Width="200px"></asp:Label>
                    <asp:Label ID="lblSysNameD" runat="server" Font-Size="15px" CssClass="InputTextStyle_Thin" MaxLength="50" Width="200px" Visible="false" ></asp:Label>
                </td>
                <td align="left">
                    <asp:Label ID="lblCompRoleID" ForeColor="blue" Font-Size="15px" runat="server" Text="授權公司："></asp:Label>
                </td>
                <td align="left">
                    <asp:Label ID="lblCompRoleName" runat="server" Font-Size="15px" CssClass="InputTextStyle_Thin" MaxLength="50" Width="300px"></asp:Label>
                    <asp:DropDownList ID="ddlCompRoleName" runat="server" CssClass="DropDownList"></asp:DropDownList>
                </td>
                <td width="5%"></td>
            </tr>
            <tr>
                <td width="5%"></td>
                <td align="left">
                    <asp:Label ID="lblUserID" Font-Size="15px" runat="server" Text="員工編號："/>
                </td>
                <td align="left">
                    <asp:TextBox ID="txtUserID" CssClass="InputTextStyle_Thin" MaxLength="6" runat="server"></asp:TextBox>
                    <uc:ButtonQuerySelectUserID ID="ucSelectEmpID" runat="server" ButtonText="..." ButtonHint="選擇人員..." WindowHeight="550" WindowWidth="500" />
                </td>
                <td align="left">
                    <asp:Label ID="lblName" Font-Size="15px" runat="server" Text="員工姓名："/>
                </td>
                <td align="left">
                    <asp:TextBox ID="txtUserName" CssClass="InputTextStyle_Thin" MaxLength="15" runat="server"></asp:TextBox>
                </td>
                <td width="5%"></td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 100%">            
            <tr>
                <td align="center" width="100%">
                    <table width="100%" height="100%" class="tbl_Content">                      
                        <tr>
                            <td style="width:100%">
                                <uc:PagerControl ID="pcMain" runat="server" GridViewName="gvMain" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 100%">
                                <asp:GridView ID="gvMain" runat="server" CssClass="GridViewStyle" AllowPaging="False" DataKeyNames="CompID,UserID" AutoGenerateColumns="False" CellPadding="2" Width="100%" PageSize="15">
                                    <FooterStyle BackColor="#99CCCC" ForeColor="#003399" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="">
                                            <ItemStyle CssClass="td_detail" Height="15px" />
                                            <HeaderStyle CssClass="td_header" Width="3%" />
                                            <ItemTemplate>
                                                <asp:RadioButton ID="rdo_gvMain" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="CompID" HeaderText="授權公司代碼" ReadOnly="True" SortExpression="CompID" Visible="false"  >
                                            <HeaderStyle Width="8%" CssClass="td_header" />
                                            <ItemStyle CssClass="td_detail" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CompName" HeaderText="授權公司" ReadOnly="True" SortExpression="CompName" >
                                            <HeaderStyle Width="8%" CssClass="td_header" />
                                            <ItemStyle CssClass="td_detail" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="UserID" HeaderText="員工編號" ReadOnly="True" SortExpression="UserID" >
                                            <HeaderStyle Width="5%" CssClass="td_header" />
                                            <ItemStyle CssClass="td_detail" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="UserName" HeaderText="員工姓名" ReadOnly="True" SortExpression="UserName" >
                                            <HeaderStyle Width="8%" CssClass="td_header" />
                                            <ItemStyle CssClass="td_detail" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="WorkStatus" HeaderText="狀態" ReadOnly="True" SortExpression="WorkStatus" >
                                            <HeaderStyle Width="4%" CssClass="td_header" />
                                            <ItemStyle CssClass="td_detail" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="DeptID" HeaderText="部門" ReadOnly="True" SortExpression="DeptID" >
                                            <HeaderStyle Width="8%" CssClass="td_header" />
                                            <ItemStyle CssClass="td_detail" />
                                        </asp:BoundField>
                                         <asp:BoundField DataField="OrganID" HeaderText="科組課" ReadOnly="True" SortExpression="OrganID" >
                                            <HeaderStyle Width="8%" CssClass="td_header" />
                                            <ItemStyle CssClass="td_detail" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="TitleName" HeaderText="職稱" ReadOnly="True" SortExpression="TitleName" >
                                            <HeaderStyle Width="8%" CssClass="td_header" />
                                            <ItemStyle CssClass="td_detail" />
                                        </asp:BoundField>                                       
                                        <asp:TemplateField HeaderText="禁用註記" SortExpression="BanMark">
                                            <ItemStyle CssClass="td_detail" />
                                            <HeaderStyle CssClass="td_header" Width="3%" />
                                            <ItemTemplate>
                                                <asp:Image ID="imgBanMark" runat="server" ImageUrl="~/Images/chkbox.gif" Visible='<% # Iif(DataBinder.Eval(Container.DataItem, "BanMark")="1","True","False") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="BanMarkValidDate" HeaderText="禁用生效日" ReadOnly="True" SortExpression="BanMarkValidDate" >
                                            <HeaderStyle Width="8%" CssClass="td_header" />
                                            <ItemStyle CssClass="td_detail" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="LastChgComp" HeaderText="最後異動公司" ReadOnly="True" SortExpression="LastChgComp">
                                            <HeaderStyle Width="7%" CssClass="td_header" />
                                            <ItemStyle CssClass="td_detail" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="LastChgID" HeaderText="最後異動者" ReadOnly="True" SortExpression="LastChgID">
                                            <HeaderStyle Width="6%" CssClass="td_header" />
                                            <ItemStyle CssClass="td_detail" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="LastChgDate" HeaderText="最後異動日期" ReadOnly="True" SortExpression="LastChgDate">
                                            <HeaderStyle Width="8%" CssClass="td_header" />
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
