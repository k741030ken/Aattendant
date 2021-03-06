<%@ Page Language="VB" AutoEventWireup="false" CodeFile="WFA060.aspx.vb" Inherits="WF_WFA060" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <link type="text/css" rel="stylesheet" href="../StyleSheet.Css" />
    <script type="text/javascript">
    <!--
        function funAction(Param)
        {
            return true;
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
                    <table width="90%" class="tbl_Edit" cellpadding="1" cellspacing="1">
                        <tr style="height:20px">
                            <td class="td_EditHeader" width="30%" align="center"><asp:Label ID="lblATID" CssClass="MustInputCaption" runat="server" Text="上傳檔案"></asp:Label>
                            </td>
                            <td class="td_Edit" style="width:70%" align="left"><asp:DropDownList ID="ddlDocType" runat="server" CssClass="DropDownListStyle" Visible="false"></asp:DropDownList><asp:FileUpload ID="myFileUpload" runat="server" CssClass="InputTextStyle_Thin" Width="100%" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="center" width="100%">
                    <table width="90%" height="100%" class="tbl_Content">
                        <tr>
                            <td style="width: 100%">
                                <asp:GridView ID="gvMain" CssClass="GridViewStyle" runat="server" AllowPaging="False" AutoGenerateColumns="False" DataSourceID="sdsMain" CellPadding="2" Width="100%" HeaderStyle-ForeColor="white" DataKeyNames="FlowID,FlowCaseID,PaperID,SeqNo">
                                    <FooterStyle BackColor="#99CCCC" ForeColor="#003399" />
                                    <Columns>
                                        <asp:TemplateField ShowHeader="False">
                                            <ItemStyle CssClass="td_detail" ForeColor="Blue" Height="18px" />
                                            <HeaderStyle CssClass="td_header" Width="5%" />
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnDelete" runat="server" Text="刪除" CommandName="DeleteUploadData"></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="FlowID" HeaderText="關卡" ReadOnly="True" SortExpression="FlowID" >
                                            <HeaderStyle Width="5%" CssClass="td_header" />
                                            <ItemStyle CssClass="td_detail" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CustomerNm" HeaderText="客户" ReadOnly="True" SortExpression="CustomerID" >
                                            <HeaderStyle Width="20%" CssClass="td_header" />
                                            <ItemStyle CssClass="td_detail" />
                                        </asp:BoundField>
                                       <%-- <asp:BoundField DataField="DocTypeNm" HeaderText="上传種類" ReadOnly="True" SortExpression="DocType" >
                                            <HeaderStyle Width="15%" CssClass="td_header" />
                                            <ItemStyle CssClass="td_detail" />
                                        </asp:BoundField>--%>
                                        <asp:TemplateField HeaderText="上傳檔案" SortExpression="FileName">
                                            <ItemStyle CssClass="td_detail" HorizontalAlign="Left" />
                                            <HeaderStyle CssClass="td_header" Width="40%" />
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnFileName" runat="server" ForeColor="Blue" Text='<%# Eval("FileName") %>' CommandName="OpenFile" CommandArgument='<%# Eval("FlowID") & "," & Eval("FlowCaseID") & "," & Eval("SeqNo").ToString() %>'></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="上傳日期" SortExpression="LastChgDate">
                                            <ItemStyle CssClass="td_detail" />
                                            <HeaderStyle CssClass="td_header" Width="15%" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblCreateDate" runat="server" Text='<%# Eval("LastChgDate", "{0:yyyy/MM/dd HH:mm:ss}") %>'></asp:Label>
                                                <asp:Label ID="LastChgID" runat="server" style="display:none" Text='<%# Eval("LastChgID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="UploadUserNm" HeaderText="上傳人員" ReadOnly="True" SortExpression="LastChgID" >
                                            <HeaderStyle Width="15%" CssClass="td_header" />
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
                                    <HeaderStyle ForeColor="White" />
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
