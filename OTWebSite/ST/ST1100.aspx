<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ST1100.aspx.vb" Inherits="ST_ST1100" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <link type="text/css" rel="stylesheet" href="../StyleSheet.Css" />
    <style type="text/css">
        .NoPic
        {
            color: Silver;
            font-size: 30px;
            font-family: Arial;
            font-weight: bolder;
        }
        .PhotoPanel
        {
        	width: 150px;
        	height: 150px;
        	text-align: center;
        	line-height: 150px;
        	vertical-align: middle;
        	position: relative;
        }
        .imgPhoto
        {
            position: absolute;
            top: 0;
            left: 0;
            right: 0;
            bottom: 0;
            margin: auto;
            max-height: 150px;
            max-width: 150px;
        }        
    </style>
    <script type="text/javascript" src="../ClientFun/jquery-1.8.3.min.js"></script>
    <script type="text/javascript">
    <!--
        function changeIDNo() {
            var strIDNo = $("#txtIDNo").val();
            if (strIDNo.length >= 2) {
                var regExp = /^[a-zA-z]/;
                if (regExp.test(strIDNo.substring(0, 1))) {
                    if (strIDNo.substring(1, 2) == "1") {
                        $("#ddlSex").val("1");
                    } else if (strIDNo.substring(1, 2) == "2") {
                        $("#ddlSex").val("2");
                    }
                }
            } 
        }

        function confirmIDNo() {
            var msg = ""
            if ($("#ddlIDType").val() == "1") { //2015/12/16 Modify
                msg = '「身分證字號邏輯有誤，是否重新輸入？【確定】表示重新輸入，【取消】表示忽略且同意修改」'
            }
            else if ($("#ddlIDType").val() == "2") { //2015/12/16 Modify
                msg = '「居留證字號邏輯有誤，是否重新輸入？【確定】表示重新輸入，【取消】表示忽略且同意修改」'
            }

            if (confirm(msg)) {
                document.getElementById('btnYes').click();
            }
            else {
                document.getElementById('btnNo').click();
            }
        }

        function confirmIDNo2() {
            var msg = "身分證字號∕居留證號為系統資料KEY值，確定修改？【確定】，主管放行後，系統將一併修改相關KEY值；【取消】，請重新輸入。"

            if (confirm(msg)) {
                document.getElementById('btnNo').click();
            }
            else {
                document.getElementById('btnYes').click();
            }
        }

        function confirmUpdate() {
            var msg = "「確定修改資料？」"

            if (confirm(msg)) {
                document.getElementById('confirmUpdate_Yes').click();
            }
            else {
                document.getElementById('confirmUpdate_No').click();
            }
        }

        //2015/12/17 Add 其他證件號碼判斷-新增
        function confirmAdd() {
            var msg = ""
            if ($("#ddlOtherIDType").val() == "1") {
                msg = '「其他證件號碼-身分證字號邏輯有誤，是否重新輸入？【確定】表示重新輸入，【取消】表示忽略且同意修改」'
            }
            else if ($("#ddlOtherIDType").val() == "2") {
                msg = '「其他證件號碼-居留證字號邏輯有誤，是否重新輸入？【確定】表示重新輸入，【取消】表示忽略且同意修改」'
            }

            if (confirm(msg)) {
                document.getElementById('btnOtherIDNoY').click();
            }
            else {
                document.getElementById('btnOtherIDNoN').click();
            }
        }

        //2015/12/17 Add 其他證件號碼判斷-修改
        function confirmUpd() {
            var msg = ""
            if ($("#ddlOtherIDType").val() == "1") {
                msg = '「其他證件號碼-身分證字號邏輯有誤，是否重新輸入？【確定】表示重新輸入，【取消】表示忽略且同意修改」'
            }
            else if ($("#ddlOtherIDType").val() == "2") {
                msg = '「其他證件號碼-居留證字號邏輯有誤，是否重新輸入？【確定】表示重新輸入，【取消】表示忽略且同意修改」'
            }

            if (confirm(msg)) {
                document.getElementById('btnOtherIDNoY_Upd').click();
            }
            else {
                document.getElementById('btnOtherIDNoN_Upd').click();
            }
        }
        -->
    </script>
</head>
<body style="margin-top:5px; margin-left:5px; margin-right:5px; margin-bottom:0" >
    <form id="frmContent" runat="server">
        <ajaxToolkit:ToolkitScriptManager runat="Server" EnableScriptGlobalization="true" EnableScriptLocalization="true" ID="ScriptManager1" />
        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 100%">
            <tr>
                <td>            
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 100%">
                        <tr>
                            <td align="center">
                                <table width="80%" class="tbl_Edit" cellpadding="1" cellspacing="1">
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblCompID" runat="server" Text="公司代碼"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left">                                
                                            <asp:Label ID="txtCompID" runat="server" ></asp:Label>
                                            <asp:HiddenField ID="hidCompID" runat="server"></asp:HiddenField>
                                            <asp:HiddenField ID="hidRecID" runat="server"></asp:HiddenField>
                                            <uc:Release ID="ucRelease" runat="server" WindowHeight="350" WindowWidth="350" style="display:none" />
                                        </td>
                                        <td class="td_EditHeader" width="15%" align="center" rowspan="3">
                                            <asp:Label ID="lblPhoto" runat="server" Text="照片"></asp:Label>
                                        </td>
                                        <td class="td_Edit" width="35%" align="center" rowspan="3">
                                            <asp:Panel ID="pnlPhoto" runat="server" CssClass="PhotoPanel">
                                                <asp:Label ID="lblPhoto_NoPic" CssClass="NoPic" runat="server" Text="NoPicture"></asp:Label>
                                                <asp:Image ID="imgPhoto" CssClass="imgPhoto" Visible="false" runat="server" />
                                            </asp:Panel>                                           
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblEmpID" runat="server" Text="員工編號"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left">
                                            <asp:Label ID="txtEmpID" runat="server" ></asp:Label>
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblEmpName" ForeColor="Blue" runat="server" Text="*員工姓名"></asp:Label>
                                        </td>
                                        <td class="td_Edit" width="35%" align="left">
                                            <asp:TextBox ID="txtNameN" CssClass="InputTextStyle_Thin" runat="server" MaxLength="12" AutoPostBack="true"></asp:TextBox>
                                            <asp:Label ID="lblNameN" runat="server" Text="(難字)"></asp:Label>
                                            <asp:TextBox ID="txtName" CssClass="InputTextStyle_Thin" runat="server" MaxLength="12"></asp:TextBox>
                                            <asp:Label ID="lblName" runat="server" Text="(拆字)"></asp:Label>
                                            <asp:TextBox ID="txtNameB" CssClass="InputTextStyle_Thin" runat="server" MaxLength="12"></asp:TextBox>
                                            <asp:Label ID="lblNameB" runat="server" Text="(造字)"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblIDNo" ForeColor="Blue" runat="server" Text="*身分證字號"></asp:Label>
                                        </td>
                                        <td class="td_Edit" width="35%" align="left">
                                            <asp:TextBox ID="txtIDNo" CssClass="InputTextStyle_Thin" runat="server" MaxLength="30" Style="text-transform: uppercase"></asp:TextBox>
                                            <asp:Button ID="btnIDNo" runat="server" style="display:none" />
                                            <asp:Button ID="btnYes" Text="是" runat="server" style="display:none" />
                                            <asp:Button ID="btnNo" Text="否" runat="server" style="display:none" />
                                            <asp:Button ID="confirmUpdate_Yes" Text="是" runat="server" style="display:none" />
                                            <asp:Button ID="confirmUpdate_No" Text="否" runat="server" style="display:none" />
                                            <asp:HiddenField ID="hidIDNo" runat="server"></asp:HiddenField>
                                        </td>
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblIDType" runat="server" ForeColor="Blue" Text="*證件類型"></asp:Label>
                                        </td>
                                        <td class="td_Edit" width="35%" align="left"> 
                                            <asp:DropDownList ID="ddlIDType" runat="server" Font-Names="細明體"></asp:DropDownList>
                                            <asp:HiddenField ID="hidIDType" runat="server"></asp:HiddenField>
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblEmpOtherIDNo" runat="server" Text="其他證件號碼"></asp:Label>
                                        </td>
                                        <td class="td_Edit" width="35%" align="left" colspan="3">
                                            <asp:Label ID="lblOtherIDNo" runat="server" Text="證件號碼"></asp:Label>
                                            <asp:TextBox ID="txtOtherIDNo" CssClass="InputTextStyle_Thin" runat="server" MaxLength="30" Style="text-transform: uppercase"></asp:TextBox>
                                            <asp:Label ID="lblOtherIDType" runat="server" Text="證件類型"></asp:Label>
                                            <asp:DropDownList ID="ddlOtherIDType" runat="server" Font-Names="細明體"></asp:DropDownList>
                                            <asp:Label ID="lblOtherIDExpireDate" runat="server" Text="工作證期限"></asp:Label>
                                            <uc:uccalender ID="txtOtherIDExpireDate" runat="server" Enabled="True" />

                                            <asp:Button ID="btnOtherIDNo" runat="server" CssClass="button" Text="新增" onclick="btnOtherIDNo_Click" />
                                            <asp:Button ID="btnOtherIDNoInsert" runat="server" Text="確定" CssClass="button" onclick="btnOtherIDNoInsert_Click" />
                                            <asp:Button ID="btnOtherIDNoUpdate" runat="server" Text="確定" CssClass="button" onclick="btnOtherIDNoUpdate_Click" />
                                            <asp:Button ID="btnOtherIDNoCancel" runat="server" Text="取消" CssClass="button" onclick="btnOtherIDNoCancel_Click" />
                                            <asp:Button ID="btnOtherIDNoY" Text="是" runat="server" style="display:none" />
                                            <asp:Button ID="btnOtherIDNoN" Text="否" runat="server" style="display:none" />
                                            <asp:Button ID="btnOtherIDNoY_Upd" Text="是" runat="server" style="display:none" />
                                            <asp:Button ID="btnOtherIDNoN_Upd" Text="否" runat="server" style="display:none" />

                                            <asp:GridView ID="gvOtherIDNo" runat="server" AutoGenerateColumns="False" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" 
                                                CellPadding="3" Width="100%" DataKeyNames="OtherIDNo,OtherIDTypeName,OtherIDExpireDate,OtherIDType"
                                                 onrowcommand="gvOtherIDNo_RowCommand" onrowupdating="gvOtherIDNo_RowUpdating" onrowdeleting="gvOtherIDNo_RowDeleting" onrowdatabound="gvOtherIDNo_RowDataBound">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="動作" ShowHeader="False">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="ibnUpdate" runat="server" CausesValidation="false" CommandName="Update" ImageUrl="~/images/edit.gif" Text="編輯" ToolTip="編輯" />
                                                        <asp:ImageButton ID="ibnDelete" runat="server" CausesValidation="false" OnClientClick="if (confirm('確定要刪除?') == false) return false;" CommandName="Delete" ImageUrl="~/images/delete.gif" Text="刪除" ToolTip="刪除" />
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="10%" />
                                                    </asp:TemplateField>
                                                    <asp:BoundField HeaderText="證件號碼" DataField="OtherIDNo">
                                                        <HeaderStyle Width="30%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField HeaderText="證件類型" DataField="OtherIDTypeName">
                                                        <HeaderStyle Width="30%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField HeaderText="工作證期限" DataField="OtherIDExpireDate">
                                                        <HeaderStyle Width="30%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="OtherIDType" HeaderText="證件類型(代碼)" ReadOnly="True" Visible="false">
                                                        <HeaderStyle Width="0%" />
                                                    </asp:BoundField>
                                                </Columns>
                                                <FooterStyle BackColor="White" ForeColor="#000066" />
                                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                                <RowStyle ForeColor="#000066" />
                                                <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                                <sortedascendingcellstyle backcolor="#F1F1F1" />
                                                <sortedascendingheaderstyle backcolor="#007DBB" />
                                                <sorteddescendingcellstyle backcolor="#CAC9C9" />
                                                <sorteddescendingheaderstyle backcolor="#00547E" />
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblEngName" runat="server" Text="英文姓名"></asp:Label>
                                        </td>
                                        <td class="td_Edit" width="35%" align="left">
                                            <asp:TextBox ID="txtEngName" CssClass="InputTextStyle_Thin" runat="server" MaxLength="20"></asp:TextBox>
                                        </td>
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblPassportName" runat="server" Text="護照英文名字"></asp:Label>
                                        </td>
                                        <td class="td_Edit" width="35%" align="left">
                                            <asp:TextBox ID="txtPassportName" CssClass="InputTextStyle_Thin" runat="server" MaxLength="20"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblBirthDate" runat="server" ForeColor="Blue" Text="*生日"></asp:Label>
                                        </td>
                                        <td class="td_Edit" width="35%" align="left">
                                            <uc:uccalender ID="txtBirthDate" runat="server" Enabled="True" />
                                            <asp:HiddenField ID="hidBirthDate" runat="server"></asp:HiddenField>
                                        </td>
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblSex" runat="server" ForeColor="Blue" Text="*性別"></asp:Label>
                                        </td>
                                        <td class="td_Edit" width="35%" align="left">
                                            <asp:DropDownList ID="ddlSex" runat="server" Font-Names="細明體">
                                                <asp:ListItem Value="" Text="---請選擇---" Selected="true"></asp:ListItem>
                                                <asp:ListItem Value="1" Text="1-男"></asp:ListItem>
                                                <asp:ListItem Value="2" Text="2-女"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblNationID" runat="server" ForeColor="Blue" Text="*身分別"></asp:Label>
                                        </td>
                                        <td class="td_Edit" width="35%" align="left">
                                            <asp:DropDownList ID="ddlNationID" runat="server" Font-Names="細明體">
                                                <asp:ListItem Value="" Text="---請選擇---" Selected="true"></asp:ListItem>
                                                <asp:ListItem Value="1" Text="1-本國人"></asp:ListItem>
                                                <asp:ListItem Value="2" Text="2-外國人"></asp:ListItem>
                                                <asp:ListItem Value="3" Text="3-大陸人"></asp:ListItem>
                                                <asp:ListItem Value="4" Text="4-香港人"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:HiddenField ID="hidNationID_Old" runat="server"></asp:HiddenField>
                                        </td>
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblIDExpireDate" runat="server" Text="工作證期限"></asp:Label>
                                        </td>
                                        <td class="td_Edit" width="35%" align="left">
                                            <uc:uccalender ID="txtIDExpireDate" runat="server" Enabled="True" />
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblEduID" runat="server" Text="學歷"></asp:Label>
                                        </td>
                                        <td class="td_Edit" width="35%" align="left">
                                            <asp:DropDownList ID="ddlEduID" runat="server" Font-Names="細明體"></asp:DropDownList>
                                        </td>
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblMarriage" runat="server" ForeColor="Blue" Text="*婚姻狀況"></asp:Label>
                                        </td>
                                        <td class="td_Edit" width="35%" align="left">
                                            <asp:DropDownList ID="ddlMarriage" runat="server" Font-Names="細明體">
                                                <asp:ListItem Value="" Text="---請選擇---" Selected="true"></asp:ListItem>
                                                <asp:ListItem Value="1" Text="1-未婚"></asp:ListItem>
                                                <asp:ListItem Value="2" Text="2-已婚"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblWorkStatus" runat="server" ForeColor="Blue" Text="*任職狀況"></asp:Label>
                                        </td>
                                        <td class="td_Edit" width="35%" align="left">
                                            <asp:DropDownList ID="ddlWorkStatus" runat="server" Font-Names="細明體"></asp:DropDownList>
                                        </td>
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblEmpType" runat="server" Text="僱用類別"></asp:Label>
                                        </td>
                                        <td class="td_Edit" width="35%" align="left">
                                            <asp:DropDownList ID="ddlEmpType" runat="server" Font-Names="細明體">
                                                <asp:ListItem Value="" Text="---請選擇---" Selected="true"></asp:ListItem>
                                                <asp:ListItem Value="1" Text="1-已婚"></asp:ListItem>
                                                <asp:ListItem Value="2" Text="2-未婚"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblEmpTotSen" runat="server" Text="公司年資"></asp:Label>
                                        </td>
                                        <td class="td_Edit" width="35%" align="left">
                                            <asp:Label ID="txtEmpTotSen" runat="server" ></asp:Label>
                                        </td>
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblEmpTotSen_SPHOLD" runat="server" Text="企業團年資"></asp:Label>
                                        </td>
                                        <td class="td_Edit" width="35%" align="left">
                                            <asp:Label ID="txtEmpTotSen_SPHOLD" runat="server" ></asp:Label>
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblEmpDate" runat="server" ForeColor="Blue" Text="*到職日"></asp:Label>
                                        </td>
                                        <td class="td_Edit" width="35%" align="left">
                                            <uc:uccalender ID="txtEmpDate" runat="server" Enabled="True" />
                                        </td>
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblSinopacEmpDate" runat="server" ForeColor="Blue" Text="*企業團到職日"></asp:Label>
                                        </td>
                                        <td class="td_Edit" width="35%" align="left">
                                            <uc:uccalender ID="txtSinopacEmpDate" runat="server" Enabled="True" />
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblProbMonth" runat="server" Text="試用期"></asp:Label>
                                        </td>
                                        <td class="td_Edit" width="35%" align="left">
                                            <asp:DropDownList ID="ddlProbMonth" runat="server" Font-Names="細明體"></asp:DropDownList>
                                        </td>
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblProbDate" runat="server" Text="試用考核試滿日"></asp:Label>
                                        </td>
                                        <td class="td_Edit" width="35%" align="left">
                                            <%--<asp:Label ID="txtProbDate" runat="server" Text=""></asp:Label>--%>
                                            <uc:uccalender ID="txtProbDate" runat="server" Enabled="False" />
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblQuitDate" runat="server" Text="離職日(公司)"></asp:Label>
                                        </td>
                                        <td class="td_Edit" width="35%" align="left">
                                            <uc:uccalender ID="txtQuitDate" runat="server" Enabled="True" />
                                        </td>
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblSinopacQuitDate" runat="server" Text="離職日(企業團)"></asp:Label>
                                        </td>
                                        <td class="td_Edit" width="35%" align="left">
                                            <uc:uccalender ID="txtSinopacQuitDate" runat="server" Enabled="True" />
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="Label3" runat="server" ForeColor="Blue" Text="*職等/*職稱"></asp:Label>
                                        </td>
                                        <td class="td_Edit" width="35%" align="left" colspan="3">
                                            <uc:ucSelectRankAndTitle ID="ucSelectRankAndTitle" runat="server"/>
                                            <asp:HiddenField ID="hidRankID" runat="server"></asp:HiddenField>
                                            <asp:HiddenField ID="hidTitleID" runat="server"></asp:HiddenField>
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblHoldingRankID" runat="server" ForeColor="Blue" Text="*金控職等"></asp:Label>
                                        </td>
                                        <td class="td_Edit" width="35%" align="left">
                                            <asp:DropDownList ID="ddlHoldingRankID" runat="server" AutoPostBack="true" Font-Names="細明體"></asp:DropDownList>
                                        </td>
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblHoldingTitle" runat="server" Text="金控職稱"></asp:Label>
                                        </td>
                                        <td class="td_Edit" width="35%" align="left">
                                            <asp:Label ID="txtHoldingTitle" runat="server" ></asp:Label>
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblPublicTitleID" runat="server" Text="對外職稱"></asp:Label>
                                        </td>
                                        <td class="td_Edit" width="35%" align="left">
                                            <asp:DropDownList ID="ddlPublicTitleID" runat="server" Font-Names="細明體"></asp:DropDownList>
                                        </td>
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblRankBeginDate" runat="server" ForeColor="Blue" Text="*最近升遷日\本階起始日"></asp:Label>
                                        </td>
                                        <td class="td_Edit" width="35%" align="left">
                                            <uc:uccalender ID="txtRankBeginDate" runat="server" Enabled="True" />
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblGroupID" runat="server" Text="事業群"></asp:Label>
                                        </td>
                                        <td class="td_Edit" width="35%" align="left">
                                            <asp:Label ID="txtGroupID" runat="server" ></asp:Label>
                                            <asp:Label ID="txtGroupName" runat="server"></asp:Label>
                                        </td>
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblDeptID" runat="server" ForeColor="Blue" Text="*部門/*科組課"></asp:Label>
                                        </td>
                                        <td class="td_Edit" width="35%" align="left">
                                            <uc:SelectHROrgan ID="ucSelectHROrgan" runat="server" />
                                            <asp:HiddenField ID="hidDeptID" runat="server"></asp:HiddenField>
                                            <asp:HiddenField ID="hidOrganID" runat="server"></asp:HiddenField>
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblPositionID" runat="server" Text="職位"></asp:Label>
                                        </td>
                                        <td class="td_Edit" width="35%" align="left">
                                            <asp:DropDownList ID="ddlPositionID" runat="server" AutoPostBack="True" Font-Names="細明體"></asp:DropDownList>
                                            <asp:Label ID="lblSelectPositionID" runat="server" ForeColor="Blue" Text="" style="display:none"></asp:Label>
                                            <asp:Label ID="lblSelectPositionName" runat="server" ForeColor="Blue" Text="" style="display:none"></asp:Label>
                                            <asp:HiddenField ID="hidPositionID" runat="server" />
                                            <asp:HiddenField ID="hidPositionID_Old" runat="server" />
                                            <uc:ButtonPosition ID="ucSelectPosition" runat="server" ButtonText="..." ButtonHint="選取" WindowHeight="550" WindowWidth="1000" />
                                        </td>
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblWorkTypeID" runat="server" ForeColor="Blue" Text="*工作性質"></asp:Label>
                                        </td>
                                        <td class="td_Edit" width="35%" align="left">
                                            <asp:DropDownList ID="ddlWorkTypeID" runat="server" Font-Names="細明體"></asp:DropDownList>
                                            <asp:Label ID="lblSelectWorkTypeID" runat="server" ForeColor="Blue" Text="" style="display:none"></asp:Label>
                                            <asp:Label ID="lblSelectWorkTypeName" runat="server" ForeColor="Blue" Text="" style="display:none"></asp:Label>
                                            <asp:HiddenField ID="hidWorkTypeID" runat="server" />
                                            <asp:HiddenField ID="hidWorkTypeID_Old" runat="server" />
                                            <uc:ButtonWorkType ID="ucSelectWorkType" runat="server" ButtonText="..." ButtonHint="選取" WindowHeight="550" WindowWidth="1000" />
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblWorkSiteID" runat="server" ForeColor="Blue" Text="*工作地點"></asp:Label>
                                        </td>
                                        <td class="td_Edit" width="35%" align="left">
                                            <asp:DropDownList ID="ddlWorkSiteID" runat="server" Font-Names="細明體"></asp:DropDownList>
                                            <asp:HiddenField ID="hidWorkSiteID" runat="server" />
                                        </td>
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblWTID" runat="server" Text="班別類型/班別"></asp:Label>
                                        </td>
                                        <td class="td_Edit" width="35%" align="left">
                                            <asp:DropDownList ID="ddlWTIDTypeFlag" runat="server" Font-Names="細明體" AutoPostBack="true"></asp:DropDownList>
                                            <asp:DropDownList ID="ddlWTID" runat="server" Font-Names="細明體"></asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblLocalHireFlag" runat="server" Text="LocalHire註記"></asp:Label>
                                        </td>
                                        <td class="td_Edit" width="35%" align="left">
                                            <asp:CheckBox ID="chkLocalHireFlag" runat="server" />
                                        </td>
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblPassExamFlag" runat="server" Text="新員招考註記"></asp:Label>
                                        </td>
                                        <td class="td_Edit" width="35%" align="left">
                                            <asp:CheckBox ID="chkPassExamFlag" runat="server" />
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblOfficeLoginFlag" runat="server" Text="需刷卡註記"></asp:Label>
                                        </td>
                                        <td class="td_Edit" width="35%" align="left">
                                            <asp:CheckBox ID="chkOfficeLoginFlag" runat="server" />
                                        </td>
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblAboriginalFlag" runat="server" Text="原住民註記/族別"></asp:Label>
                                        </td>
                                        <td class="td_Edit" width="35%" align="left">
                                            <asp:CheckBox ID="chkAboriginalFlag" AutoPostBack="true" runat="server" />
                                            <asp:DropDownList ID="ddlAboriginalTribe" runat="server" Font-Names="細明體"></asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblEmpFlowOrganID" runat="server" ForeColor="Blue" Text="*最小簽核單位"></asp:Label>
                                        </td>
                                        <td class="td_Edit" width="35%" align="left" colspan="3">
                                            <asp:UpdatePanel ID="UpdFlowOrgnaID" runat="server" ChildrenAsTriggers="False" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <uc:ucSelectFlowOrgan ID="ucSelectFlowOrgan" runat="server" Enabled="True" AutoPostBack="true" />
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                            <asp:HiddenField ID="hidFlowOrganID_Old" runat="server" />
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblLastChgComp1" runat="server" Text="最後異動公司"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left">
                                            <asp:Label ID="lblLastChgComp" runat="server" ></asp:Label>
                                        </td>
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblLastChgID1" runat="server" Text="最後異動人員"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left">
                                            <asp:Label ID="lblLastChgID" runat="server" > </asp:Label>
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblLastChgDate1" runat="server" Text="最後異動日期"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left" colspan="3">
                                            <asp:Label ID="lblLastChgDate" runat="server" ></asp:Label>
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
