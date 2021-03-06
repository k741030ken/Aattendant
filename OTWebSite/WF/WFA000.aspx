<%@ Page Language="VB" AutoEventWireup="false" CodeFile="WFA000.aspx.vb" Inherits="WF_WFA000" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
    <title></title>
    <link type="text/css" rel="stylesheet" href="../StyleSheet.Css" />
    <link type="text/css" rel="stylesheet" href="../form.Css" />
    <script type="text/javascript">
    <!--
        function funAction(Param)
        {
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
        
        function WFStepCloseMenu() {
            window.parent.parent.frames[1].location = "WFA010.aspx";
            window.parent.parent.frames[2].imgSplit_WorkFlow.src = "";
            window.parent.parent.frames[2].imgSplit_WorkFlow.alt = "";
            window.parent.parent.frmSet.cols = "0,0,20,*";
            window.parent.parent.frames[2].imgSplit_WorkFlow.style.display = "none";
        }
       -->
    </script>
</head>

<body style="margin-top:5px; margin-left:5px; margin-right:5px; margin-bottom:0" >
    <form id="frmContent" runat="server">
        <table border="0" width="100%">
            <tr>
                <td style="width:100%" align="center">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 98%;">
                        <tr>
                             <td style="width:100%;">
                                <div class="divSepTitle">
                                    <div class="Head">待辦事項與通知</div>
                                    <div class="Body">
                                        <span style="float:left;">
                                            <asp:PlaceHolder ID="phToDoCount" runat="server">
                                                &nbsp;<asp:LinkButton ID="btnDispatchCnt" Font-Size="12px" runat="server" Text="急件(0)" CausesValidation="false" onmouseover="this.style.color='red';" onmouseout="this.style.color='blue';"></asp:LinkButton> 
                                            </asp:PlaceHolder>
                                        </span>
                                        <span style="float:right"> </span>
                                    </div>
                                </div>
                             </td>
                        </tr>
                        <tr>
                            <td style="width:100%">
                                <uc:PagerControl ID="pcMain" runat="server" GridViewName="gvMain" PerPageRecord="10" />
                            </td>
                        </tr>
                        <tr>
                            <td align="center" width="100%" >
                                <asp:GridView ID="gvMain" runat="server" AutoGenerateColumns="False"  
                                    AllowSorting="True"  CellPadding="2" Width="100%"  CssClass="GridViewStyle" 
                                    DataKeyNames="FlowType,FlowKeyStr,FlowStepID">
                                    <FooterStyle BackColor="#99CCCC" ForeColor="#003399" />
                                    <Columns>
                                        <asp:TemplateField ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnSelect" runat="server" CausesValidation="False" 
                                                    CommandName="Select" Text="處理"></asp:LinkButton>&nbsp;
                                                <asp:LinkButton ID="btnDelete" runat="server" CausesValidation="False" 
                                                    CommandName="Delete" Text="刪除" Enabled="False" OnClientClick="return confirm('確定刪除此筆資料？');"></asp:LinkButton>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="td_header" Width="9%" />
                                            <ItemStyle CssClass="td_detail" Height="24px" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Seq" HeaderText="序號" ReadOnly="True" SortExpression="Seq" >
                                            <HeaderStyle Width="4%" CssClass="td_header" />
                                            <ItemStyle CssClass="td_detail" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="FlowDispatchFlag" HeaderText="急件" ReadOnly="True" SortExpression="FlowDispatchFlag" >
                                            <HeaderStyle Width="4%" CssClass="td_header" />
                                            <ItemStyle CssClass="td_detail" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="FlowStepDesc" HeaderText="關卡" ReadOnly="True" SortExpression="FlowStepDesc" >
                                            <HeaderStyle Width="12%" CssClass="td_header" />
                                            <ItemStyle CssClass="td_detail" HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="KeyID" HeaderText="申請編號" ReadOnly="True" SortExpression="KeyID">
                                            <HeaderStyle Width="10%" CssClass="td_header" />
                                            <ItemStyle CssClass="td_detail" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="FlowShowValue" HeaderText="訊息摘要" ReadOnly="True">
                                            <HeaderStyle Width="25%" CssClass="td_header" />
                                            <ItemStyle CssClass="td_detail" HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="FromBrName" HeaderText="發出部門" ReadOnly="True" SortExpression="FromBrName" >
                                            <HeaderStyle Width="10%" CssClass="td_header" />
                                            <ItemStyle CssClass="td_detail" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="FromUserName" HeaderText="發出人" ReadOnly="True" SortExpression="FromUserName" >
                                            <HeaderStyle Width="10%" CssClass="td_header" />
                                            <ItemStyle CssClass="td_detail" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="FromDate" HeaderText="發出時間" ReadOnly="True" SortExpression="FromDate" DataFormatString="{0:yyyy/MM/dd HH:mm:ss}" >
                                            <HeaderStyle Width="16%" CssClass="td_header" />
                                            <ItemStyle CssClass="td_detail" />
                                        </asp:BoundField>
                                    </Columns>
                                    <EmptyDataRowStyle CssClass="GridView_EmptyRowStyle" />
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblNoData" runat="server" Text="無待辦事項！"></asp:Label>
                                    </EmptyDataTemplate>
                                    <RowStyle CssClass="tr_evenline" />
                                    <AlternatingRowStyle CssClass="tr_oddline" />
                                    <SelectedRowStyle CssClass="GridView_SelectedRowStyle" BackColor="Moccasin" />
                                    <PagerStyle CssClass="GridView_PagerStyle" />
                                    <PagerSettings Position="Top" />
                                </asp:GridView>
                            </td>
                        </tr>
                    </table> 
                    <hr /> 
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 98%;">
                        <tr>
                             <td align="left">
                                <div class="divSepTitle">
                                    <div class="Head">追蹤清單</div>
                                    <div class="Body">
                                        <span style="float:left"></span>
                                        <span style="float:right"></span>
                                    </div>
                                </div>
                             </td>
                        </tr>
                        <tr>
                            <td style="width:100%">
                                <uc:PagerControl ID="pcTraceList" runat="server" GridViewName="TraceGridView" PerPageRecord="5" />
                            </td>
                        </tr>
                        <tr>
                            <td align="center" width="100%" >
                                <asp:GridView ID="TraceGridView" runat="server" AllowPaging="False"  AllowSorting="True" AutoGenerateColumns="False" CellPadding="2" Width="100%" CssClass="GridViewStyle" meta:resourcekey="gvMainResource2"  DataKeyNames="FlowID,FlowCaseID,FlowLogID" PageSize="5">
                                    <FooterStyle BackColor="#99CCCC" ForeColor="#003399" />
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderStyle Width="7%" CssClass="td_header" />
                                            <ItemStyle CssClass="td_detail" HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnSelect" runat="server" CommandName="Edit" Text="追踪"></asp:LinkButton>
                                                <asp:LinkButton ID="btnDelete" runat="server" CommandName="Delete" Text="刪除" OnClientClick="return funAction('btnDelete')"></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Seq" HeaderText="序號" ReadOnly="True" SortExpression="Seq">
                                            <HeaderStyle Width="4%" CssClass="td_header" />
                                            <ItemStyle CssClass="td_detail" Height="24px"/>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="FlowDispatchFlag" HeaderText="急件" ReadOnly="True" SortExpression="FlowDispatchFlag" >
                                            <HeaderStyle Width="4%" CssClass="td_header" />
                                            <ItemStyle CssClass="td_detail" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="FlowStepDesc" HeaderText="關卡" ReadOnly="True" SortExpression="FlowStepDesc" >
                                            <HeaderStyle Width="10%" CssClass="td_header" />
                                            <ItemStyle CssClass="td_detail" HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="KeyID" HeaderText="申請編號" ReadOnly="True" SortExpression="KeyID" >
                                            <HeaderStyle Width="10%" CssClass="td_header" />
                                            <ItemStyle CssClass="td_detail" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="FlowShowValue" HeaderText="訊息摘要" ReadOnly="True" >
                                            <HeaderStyle Width="20%" CssClass="td_header" />
                                            <ItemStyle CssClass="td_detail" HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="AssignName" HeaderText="處理人" ReadOnly="True" SortExpression="AssignName" >
                                            <HeaderStyle Width="10%" CssClass="td_header" />
                                            <ItemStyle CssClass="td_detail" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="FromBrName" HeaderText="發出部門" ReadOnly="True" SortExpression="FromBrName" >
                                            <HeaderStyle Width="10%" CssClass="td_header" />
                                            <ItemStyle CssClass="td_detail" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="FromUserName" HeaderText="發出人" ReadOnly="True" SortExpression="FromUserName" >
                                            <HeaderStyle Width="10%" CssClass="td_header" />
                                            <ItemStyle CssClass="td_detail" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="FromDate" HeaderText="發出時間" ReadOnly="True" SortExpression="FromDate" >
                                            <HeaderStyle Width="15%" CssClass="td_header" />
                                            <ItemStyle CssClass="td_detail" />
                                        </asp:BoundField>
                                    </Columns>
                                    <EmptyDataRowStyle CssClass="GridView_EmptyRowStyle" />
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblNoData" runat="server" Text="無追蹤項目！"></asp:Label>
                                    </EmptyDataTemplate>
                                    <RowStyle CssClass="tr_evenline" />
                                    <AlternatingRowStyle CssClass="tr_oddline" />
                                    <SelectedRowStyle CssClass="GridView_SelectedRowStyle" BackColor="Moccasin" />
                                    <PagerStyle CssClass="GridView_PagerStyle" />
                                    <PagerSettings Position="Top" />
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <p>　</p>
    </form>
</body>
</html>
