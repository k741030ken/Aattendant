<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SC0600.aspx.vb" Inherits="SC_SC0600" %>

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
                    if (!confirm('將刪除選取群組「全部」資料，確定刪除？'))
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
        
        //另開員工Pop視窗
        function funCallSelectEmp()
        {
            var obj = document.getElementById('ucSelecEmp_btnSelect');
            if (obj == null)
                alert('Can not find object!');
            else
            {
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
                    <asp:DropDownList ID="ddlCompRoleName" runat="server" AutoPostBack="true" CssClass="DropDownList"></asp:DropDownList>
                </td>
                <td width="5%"></td>
            </tr>            
            <tr>
                <td width="5%"></td>
                <td align="left">
                    <asp:Label ID="lblEmpID" Font-Size="15px" runat="server" Text="員工編號："/>
                </td>
                <td align="left">
                    <asp:TextBox ID="txtEmpID" CssClass="InputTextStyle_Thin" MaxLength="6" runat="server"></asp:TextBox>
                    <uc:ButtonQuerySelectUserID ID="ucSelectEmpID" runat="server" ButtonText="..." ButtonHint="選擇人員..." WindowHeight="550" WindowWidth="500" />
                </td>
                <td align="left">
                    <asp:Label ID="lblName" Font-Size="15px" runat="server" Text="員工姓名："/>
                </td>
                <td align="left">
                    <asp:TextBox ID="txtName" CssClass="InputTextStyle_Thin" MaxLength="15" runat="server"></asp:TextBox>
                </td>
                <td width="5%"></td>
            </tr>
            <tr>
                <td width="5%"></td>
                <td align="left">
                    <asp:Label ID="lblGroupID" Font-Size="15px" runat="server" Text="群組："/>
                </td>
                <td align="left" colspan="3">
                    <asp:DropDownList ID="ddlGroupID" Width="300px" runat="server" CssClass="DropDownList"></asp:DropDownList>
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
                                <asp:GridView ID="gvMain" runat="server" CssClass="GridViewStyle" AllowPaging="False" DataKeyNames="CompID,UserID,SysID,CompRoleID,GroupID" AutoGenerateColumns="False" CellPadding="2" Width="100%" PageSize="15">
                                    <FooterStyle BackColor="#99CCCC" ForeColor="#003399" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="">
                                            <ItemStyle CssClass="td_detail" Height="15px" />
                                            <HeaderStyle CssClass="td_header" Width="3%" />
                                            <ItemTemplate>
                                                <asp:RadioButton ID="rdo_gvMain" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="CompID" HeaderText="公司" ReadOnly="True" SortExpression="CompID" Visible="false" >
                                            <HeaderStyle Width="8%" CssClass="td_header" />
                                            <ItemStyle CssClass="td_detail" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CompName" HeaderText="公司" ReadOnly="True" SortExpression="CompName" >
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
                                        <asp:BoundField DataField="UserID" HeaderText="使用者代碼" ReadOnly="True" SortExpression="UserID" Visible="false"  >
                                            <HeaderStyle Width="8%" CssClass="td_header" />
                                            <ItemStyle CssClass="td_detail" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="UserName" HeaderText="使用者" ReadOnly="True" SortExpression="UserName" >
                                            <HeaderStyle Width="8%" CssClass="td_header" />
                                            <ItemStyle CssClass="td_detail" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CompRoleName" HeaderText="被授權公司" ReadOnly="True" SortExpression="CompRoleName" >
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
                                            <HeaderStyle CssClass="td_header" Width="3%" />
                                            <ItemTemplate>
                                                <asp:Image id="imgRightA" runat="server" ImageUrl="~/images/chkbox.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightA")="V","True","False") %>' />
                                                <asp:Image id="imgRightA_E" runat="server" ImageUrl="~/images/chkboxE.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightA")="","True","False") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="修改" SortExpression="RightU">
                                            <ItemStyle CssClass="td_detail" />
                                            <HeaderStyle CssClass="td_header" Width="3%" />
                                            <ItemTemplate>
                                                <asp:Image id="imgRightU" runat="server" ImageUrl="~/images/chkbox.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightU")="V","True","False") %>' />
                                                <asp:Image id="imgRightU_E" runat="server" ImageUrl="~/images/chkboxE.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightU")="","True","False") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="刪除" SortExpression="RightD">
                                            <ItemStyle CssClass="td_detail" />
                                            <HeaderStyle CssClass="td_header" Width="3%" />
                                            <ItemTemplate>
                                                <asp:Image id="imgRightD" runat="server" ImageUrl="~/images/chkbox.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightD")="V","True","False") %>' />
                                                <asp:Image id="imgRightD_E" runat="server" ImageUrl="~/images/chkboxE.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightD")="","True","False") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="查詢" SortExpression="RightI">
                                            <ItemStyle CssClass="td_detail" />
                                            <HeaderStyle CssClass="td_header" Width="3%" />
                                            <ItemTemplate>
                                                <asp:Image id="imgRightI" runat="server" ImageUrl="~/images/chkbox.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightI")="V","True","False") %>' />
                                                <asp:Image id="imgRightI_E" runat="server" ImageUrl="~/images/chkboxE.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightI")="","True","False") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="執行" SortExpression="RightE">
                                            <ItemStyle CssClass="td_detail" />
                                            <HeaderStyle CssClass="td_header" Width="3%" />
                                            <ItemTemplate>
                                                <asp:Image id="imgRightE" runat="server" ImageUrl="~/images/chkbox.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightE")="V","True","False") %>' />
                                                <asp:Image id="imgRightE_E" runat="server" ImageUrl="~/images/chkboxE.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightE")="","True","False") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="確認" SortExpression="RightC">
                                            <ItemStyle CssClass="td_detail" />
                                            <HeaderStyle CssClass="td_header" Width="3%" />
                                            <ItemTemplate>
                                                <asp:Image id="imgRightC" runat="server" ImageUrl="~/images/chkbox.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightC")="V","True","False") %>' />
                                                <asp:Image id="imgRightC_E" runat="server" ImageUrl="~/images/chkboxE.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightC")="","True","False") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="列印" SortExpression="RightP">
                                            <ItemStyle CssClass="td_detail" />
                                            <HeaderStyle CssClass="td_header" Width="3%" />
                                            <ItemTemplate>
                                                <asp:Image id="imgRightP" runat="server" ImageUrl="~/images/chkbox.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightP")="V","True","False") %>' />
                                                <asp:Image id="imgRightP_E" runat="server" ImageUrl="~/images/chkboxE.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightP")="","True","False") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="下傳" SortExpression="RightL">
                                            <ItemStyle CssClass="td_detail" />
                                            <HeaderStyle CssClass="td_header" Width="3%" />
                                            <ItemTemplate>
                                                <asp:Image id="imgRightL" runat="server" ImageUrl="~/images/chkbox.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightL")="V","True","False") %>' />
                                                <asp:Image id="imgRightL_E" runat="server" ImageUrl="~/images/chkboxE.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightL")="","True","False") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="上傳" SortExpression="RightF">
                                            <ItemStyle CssClass="td_detail" />
                                            <HeaderStyle CssClass="td_header" Width="3%" />
                                            <ItemTemplate>
                                                <asp:Image id="imgRightF" runat="server" ImageUrl="~/images/chkbox.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightF")="V","True","False") %>' />
                                                <asp:Image id="imgRightF_E" runat="server" ImageUrl="~/images/chkboxE.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightF")="","True","False") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="放行" SortExpression="RightR">
                                            <ItemStyle CssClass="td_detail" />
                                            <HeaderStyle CssClass="td_header" Width="3%" />
                                            <ItemTemplate>
                                                <asp:Image id="imgRightR" runat="server" ImageUrl="~/images/chkbox.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightR")="V","True","False") %>' />
                                                <asp:Image id="imgRightR_E" runat="server" ImageUrl="~/images/chkboxE.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightR")="","True","False") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="複製" SortExpression="RightG">
                                            <ItemStyle CssClass="td_detail" />
                                            <HeaderStyle CssClass="td_header" Width="3%" />
                                            <ItemTemplate>
                                                <asp:Image id="imgRightG" runat="server" ImageUrl="~/images/chkbox.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightG")="V","True","False") %>' />
                                                <asp:Image id="imgRightG_E" runat="server" ImageUrl="~/images/chkboxE.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightG")="","True","False") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="駁回" SortExpression="RightB">
                                            <ItemStyle CssClass="td_detail" />
                                            <HeaderStyle CssClass="td_header" Width="3%" />
                                            <ItemTemplate>
                                                <asp:Image id="imgRightB" runat="server" ImageUrl="~/images/chkbox.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightB")="V","True","False") %>' />
                                                <asp:Image id="imgRightB_E" runat="server" ImageUrl="~/images/chkboxE.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightB")="","True","False") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="清除" SortExpression="RightX">
                                            <ItemStyle CssClass="td_detail" />
                                            <HeaderStyle CssClass="td_header" Width="3%" />
                                            <ItemTemplate>
                                                <asp:Image id="imgRightX" runat="server" ImageUrl="~/images/chkbox.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightX")="V","True","False") %>' />
                                                <asp:Image id="imgRightX_E" runat="server" ImageUrl="~/images/chkboxE.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightX")="","True","False") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="保留" SortExpression="RightH">
                                            <ItemStyle CssClass="td_detail" />
                                            <HeaderStyle CssClass="td_header" Width="3%" />
                                            <ItemTemplate>
                                                <asp:Image id="imgRightH" runat="server" ImageUrl="~/images/chkbox.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightH")="V","True","False") %>' />
                                                <asp:Image id="imgRightH_E" runat="server" ImageUrl="~/images/chkboxE.gif" Visible='<%# Iif(Databinder.Eval(Container.DataItem, "RightH")="","True","False") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="保留" SortExpression="RightJ">
                                            <ItemStyle CssClass="td_detail" />
                                            <HeaderStyle CssClass="td_header" Width="3%" />
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
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
