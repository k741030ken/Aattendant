﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AppLogQry.aspx.cs" Inherits="Util_AppLogQry" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Util/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc1" %>
<%@ Register Src="~/Util/ucTextBox.ascx" TagName="ucTextBox" TagPrefix="uc1" %>
<%@ Register Src="~/Util/ucCommSingleSelect.ascx" TagName="ucCommSingleSelect" TagPrefix="uc1" %>
<%@ Register Src="ucGridView.ascx" TagName="ucGridView" TagPrefix="uc1" %>
<!DOCTYPE html>
<html>
<head runat="server">
    <title>AppLogQry</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </asp:ToolkitScriptManager>
        <asp:Label ID="labErrMsg" runat="server" Text="" Visible="false"></asp:Label>
        <div runat="server" id="DivQryCondition" visible="true">
            <fieldset class='Util_Fieldset'>
                <legend class="Util_Legend">查詢條件
                    <asp:LinkButton ID="btnPurgeTable" runat="server" OnClick="btnPurgeTable_Click">
                        <asp:Image ID="imgPurce" runat="server" BorderStyle="None" ImageUrl="~/Util/WebClient/Icon_Delete.png" /></asp:LinkButton></legend>
                <div class="Util_clsRow1" style="padding: 3px;">
                    <uc1:ucTextBox ID="LogApp" ucCaption="LogApp" ucWidth="200" runat="server" />
                    <uc1:ucCommSingleSelect ID="LogType" ucCaption="LogType" runat="server" ucIsHasEmptyItem="true"
                        ucIsSearchEnabled="false" />
                    <uc1:ucTextBox ID="LogUser" ucCaption="LogUser" ucWidth="200" runat="server" />
                </div>
                <div class="Util_clsRow2" style="padding: 3px;">
                    <uc1:ucDatePicker ID="LogDate1" ucCaption="LogDate" runat="server" />
                    ～
                    <uc1:ucDatePicker ID="LogDate2" runat="server" />
                </div>
                <div style="text-align: center; height: 50px;">
                    <hr class="Util_clsHR" />
                    <asp:Button runat="server" ID="btnQry" CssClass="Util_clsBtnGray" Width="80" Text="查　　詢"
                        OnClick="btnQry_Click" />
                    <asp:Button runat="server" ID="btnClear" CssClass="Util_clsBtnGray" Width="80"
                        Text="清　　除" OnClick="btnClear_Click" />
                </div>
            </fieldset>
        </div>
        <br />
        <div runat="server" id="DivQryResult" visible="false">
            <fieldset class='Util_Fieldset'>
                <legend class="Util_Legend">查詢結果</legend>
                <div class="Util_Frame">
                    <uc1:ucGridView ID="ucGridView1" runat="server" />
                </div>
            </fieldset>
        </div>
    </div>
    </form>
</body>
</html>
