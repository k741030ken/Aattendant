<%@ Page Language="VB" AutoEventWireup="false" CodeFile="PA4101.aspx.vb" Inherits="PA_PA4101" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <link type="text/css" rel="stylesheet" href="../StyleSheet.Css" />
    <script type="text/javascript" src="../ClientFun/jquery-1.8.3.min.js"></script>
    <style type="text/css">
        .listbox  
        {
            height: 192px;
            width: 250px;
            margin: 2px;
            display: inline-block;
            overflow-x: hidden;
            overflow-y: auto;
            background-color: White;
            border: 1px solid gray;
        }
        .orgbox  
        {
            margin-left: 15px;
        }
        .chkbox
        {
        	display: inline-block;
        	vertical-align: -50%;
        	padding-bottom: 5px;
        	border-bottom: 1px dashed black;
        	width: 200px;
        }
    </style>
    <script type="text/javascript">
    <!--
        function confirmAdd() {
            if (confirm('已有相同權限，是否全部刪除重新新增？')) {
                document.getElementById('btnConfirmAdd').click();
            }
            else return false;
        }

        function DoClear() {
            $("#hidAllCompIDFlag").val("");
            $("#hidUseCompID").val("");
            $("#hidAllCompGroup").val("");
            $("#hidUseGroupID").val("");
            $("#hidAllGroupIDFlag").val("");
            $("#hidAllCompGroupOrgan").val("");
            $("#hidUseOrganID").val("");
            $("#hidAllOrganIDFlag").val("");
            $("#hidAllCompOrganFlag").val("");
            $("#hidUseAllOrganIDFlag").val("");
            $("#hidUseGroupOrganID").val("");
            $('#divUseGroup').empty();
            $('#divUseOrgan').empty();
            $('#divUseGroupOrgan').empty();
        }
    -->
    </script>
</head>
<body style="margin-top:5px; margin-left:5px; margin-right:5px; margin-bottom:0">
    <form id="frmContent" runat="server">
        <ajaxToolkit:ToolkitScriptManager runat="Server" EnableScriptGlobalization="true" EnableScriptLocalization="true" ID="ScriptManager1" />
        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 100%">
            <tr>
                <td align="center">
                    <table width="85%" class="tbl_Edit" cellpadding="1" cellspacing="1">
                        <tr>
                            <td class="td_EditHeader" width="18%" align="center">
                                <asp:Label ID="lblComID" ForeColor="Blue" runat="server" Text="*公司代碼"></asp:Label>
                            </td>
                            <td class="td_Edit" align="left" colspan="3">
                                <asp:Label ID="txtComID" runat="server"></asp:Label>
                                <uc:Release ID="ucRelease" runat="server" WindowHeight="350" WindowWidth="350" style="display:none" />
                                <asp:Button ID="btnConfirmAdd" Text="確認新增" runat="server" style="display:none" />
                            </td>
                        </tr> 
                        <tr style="height:20px">
                            <td class="td_EditHeader" width="18%" align="center">
                                <asp:Label ID="lblEmpID" ForeColor="Blue" runat="server" Text="*員工編號"></asp:Label>
                            </td>
                            <td class="td_Edit" align="left" colspan="3">
                                <asp:TextBox ID="txtEmpID" CssClass="InputTextStyle_Thin" runat="server" AutoPostBack="true" MaxLength="6" Style="text-transform: uppercase"></asp:TextBox>
                                <asp:Label ID="lblName" runat="server"></asp:Label>
                                <uc:ButtonQuerySelectUserID ID="ucSelectEmpID" runat="server" ButtonText="..." ButtonHint="選擇人員..." WindowHeight="550" WindowWidth="500" />
                            </td>
                        </tr>
                        <tr style="height:20px">
                            <td class="td_EditHeader" width="18%" align="center">
                                <asp:Label ID="lblGrantFlag" ForeColor="Blue" runat="server" Text="*授權/排除授權"></asp:Label>
                            </td>
                            <td class="td_Edit" align="left" colspan="3">
                                <asp:RadioButton ID="rbGrantFlag1" GroupName="GrantFlag" AutoPostBack="true" Text="授權" Checked="true" runat="server" />
                                <asp:RadioButton ID="rbGrantFlag0" GroupName="GrantFlag" AutoPostBack="true" Text="排除授權" runat="server" />
                            </td>
                        </tr>
                    </table>
                    <table width="85%" class="tbl_Edit" cellpadding="1" cellspacing="1" id="tabGrantFlag1" runat="server">
                        <tr style="height:20px">
                            <td class="td_EditHeader" width="18%" align="center">
                                <asp:Label ID="lblOurColleagues" ForeColor="Blue" runat="server" Text="*頁籤授權"></asp:Label>
                            </td>
                            <td class="td_Edit" align="left" colspan="3">
                                <asp:UpdatePanel ID="upOurColleagues" runat="server" ChildrenAsTriggers="False" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:CheckBox ID="cbAllOC" Text="全選" runat="server" AutoPostBack="true" />
                                        <asp:CheckBox ID="cb01" Text="基本資料" ForeColor="Red" Checked="true" Enabled="false" runat="server" />
                                        <asp:CheckBox ID="cb02" Text="進階資料" runat="server" />
                                        <asp:CheckBox ID="cb03" Text="學歷資料" runat="server" />
                                        <asp:CheckBox ID="cb04" Text="前職經歷" runat="server" />
                                        <asp:CheckBox ID="cb05" Text="家庭狀況" runat="server" />
                                        <asp:CheckBox ID="cb06" Text="企業團經歷" runat="server" />
                                        <asp:CheckBox ID="cb07" Text="證照" runat="server" />
                                        <asp:CheckBox ID="cb08" Text="訓練紀錄" runat="server" />                                        
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="cbAllOC" EventName="CheckedChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>                                
                            </td>
                        </tr>
                        <tr style="height:20px">
                            <td class="td_EditHeader" width="18%" align="center">
                                <asp:Label ID="lblAllFag" ForeColor="Blue" runat="server" Text="公司/事業群/部門全選"></asp:Label>
                            </td>
                            <td class="td_Edit" align="left" colspan="3">
                                <input type="checkbox" id="cbAllCompIDFlag" name="cbAllCompIDFlag" onclick="AllComp_Change(this)" value="1" />
                                <asp:HiddenField ID="hidAllCompIDFlag" runat="server" />
                            </td>
                        </tr>
                        <tr style="height:20px">
                            <td class="td_EditHeader" width="18%" align="center">
                                <asp:Label ID="lblUseCompID" ForeColor="Blue" runat="server" Text="*公司授權"></asp:Label>
                            </td>
                            <td class="td_Edit" align="left" colspan="3">
                                <div id="divUseComp" class="listbox"></div>
                                <asp:HiddenField ID="hidUseCompID" runat="server" />
                            </td>
                        </tr>
                        <tr style="height:20px">
                            <td class="td_EditHeader" width="18%" align="center">
                                <asp:Label ID="lblUseGroupID" ForeColor="Blue" runat="server" Text="*事業群授權"></asp:Label>
                                <br />
                                <asp:Label ID="lblUseGroupIDNotice" ForeColor="Red" runat="server" Text="(需先選擇公司)"></asp:Label>
                            </td>
                            <td class="td_Edit" align="left" colspan="3">
                                <asp:HiddenField ID="hidAllCompGroup" runat="server" />
                                <input type="checkbox" id="cbAllCompGroup" name="cbAllCompGroup" onclick="AllCompGroup_Change(this)" value="1" />
                                <label for="cbAllCompGroup" style="font-size:12px;color:Red;">多公司/全事業群</label>
                                <div id="divUseGroup"></div>
                                <asp:HiddenField ID="hidUseGroupID" runat="server" />
                                <asp:HiddenField ID="hidAllGroupIDFlag" runat="server" />
                            </td>
                        </tr>
                        <tr style="height:20px">
                            <td class="td_EditHeader" width="18%" align="center">
                                <asp:Label ID="lblUseOrganID" ForeColor="Blue" runat="server" Text="*部門授權"></asp:Label>
                                <br />
                                <asp:Label ID="lblUseOrganIDNotice" ForeColor="Red" runat="server" Text="(需先選擇事業群)"></asp:Label>
                            </td>
                            <td class="td_Edit" align="left" colspan="3">
                                <asp:HiddenField ID="hidAllCompGroupOrgan" runat="server" />
                                <input type="checkbox" id="cbAllCompGroupOrgan" name="cbAllCompGroupOrgan" onclick="AllCompGroupOrgan_Change(this)" value="1" />
                                <label for="cbAllCompGroupOrgan" style="font-size:12px;color:Red;">多公司/多事業群/全部門</label>
                                <div id="divUseOrgan"></div>
                                <asp:HiddenField ID="hidUseOrganID" runat="server" />
                                <asp:HiddenField ID="hidAllOrganIDFlag" runat="server" />
                                <asp:HiddenField ID="hidAllCompOrganFlag" runat="server" />
                            </td>
                        </tr>
                        <tr style="height:20px">
                            <td class="td_EditHeader" width="18%" align="center">
                                <asp:Label ID="lblUseRankID" ForeColor="Blue" runat="server" Text="*金控職等"></asp:Label>
                            </td>
                            <td class="td_Edit" align="left" colspan="3">
                                <asp:DropDownList ID="ddlUseRankID" runat="server" Font-Size="12px"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr style="height:20px">
                            <td class="td_EditHeader" width="18%" align="center">
                                <asp:Label ID="lblBeginDate" ForeColor="Blue" runat="server" Text="*查詢起日"></asp:Label>
                            </td>
                            <td class="td_Edit" align="left" colspan="3">
                                <uc:uccalender ID="txtBeginDate" runat="server" Enabled="True" />
                            </td>
                        </tr>
                        <tr style="height:20px">
                            <td class="td_EditHeader" width="18%" align="center">
                                <asp:Label ID="lblEndDate" ForeColor="Blue" runat="server" Text="*查詢迄日"></asp:Label>
                            </td>
                            <td class="td_Edit" align="left" colspan="3">
                                <uc:uccalender ID="txtEndDate" runat="server" Enabled="True" />
                            </td>
                        </tr>
                    </table>
                    <table width="85%" class="tbl_Edit" cellpadding="1" cellspacing="1" id="tabGrantFlag0" runat="server">
                        <tr style="height:20px">
                            <td class="td_EditHeader" width="18%" align="center">
                                <asp:Label ID="lblUseCompID_0" ForeColor="Blue" runat="server" Text="*查詢權限公司"></asp:Label>
                            </td>
                            <td class="td_Edit" align="left" colspan="3">
                                <asp:DropDownList ID="ddlUseCompID" runat="server" Font-Size="12px" onchange="CompChange(this)"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr style="height:20px">
                            <td class="td_EditHeader" width="18%" align="center">
                                <asp:Label ID="lblUseOrganID_0" ForeColor="Blue" runat="server" Text="*查詢權限部門"></asp:Label>
                                <br />
                                <asp:Label ID="lblUseOrganIDNotice_0" ForeColor="Red" runat="server" Text="(需先選擇公司)"></asp:Label>
                            </td>
                            <td class="td_Edit" align="left" colspan="3">
                                <div id="divUseGroupOrgan" class="listbox"></div>
                                <asp:HiddenField ID="hidUseAllOrganIDFlag" runat="server" />
                                <asp:HiddenField ID="hidUseGroupOrganID" runat="server" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </form>
</body>
<script type="text/javascript">
<!--
    $(function () {
        AllComp_Load();
        Page_Load();

        $("#txtEmpID").change(function () {
            SaveCheckValue();
        });

        $("#ucSelectEmpID_btnSelect").click(function () {
            SaveCheckValue();
        });
    });

    function Page_Load() {
        //授權
        if ($("#rbGrantFlag1").prop("checked")) {
            if ($("#hidAllCompIDFlag").val() == "1") {
                $("#cbAllCompIDFlag").prop("checked", true);
                $("#divUseComp input:checkbox").prop("checked", true).prop("disabled", true);
                $("#cbAllCompGroup").prop("checked", true).prop("disabled", true);
                $("#cbAllCompGroupOrgan").prop("checked", true).prop("disabled", true);
            }
            else {
                showSubmitting();
                setTimeout(function () {

                    if ($("#hidUseCompID").val() != "") {
                        var arrValue = $("#hidUseCompID").val().split(",").sort();
                        for (var item in arrValue) {
                            $("#divUseComp input[id='cb_" + arrValue[item] + "']").prop("checked", true);
                            var CompID = arrValue[item];
                            var CompName = $("#divUseComp input[id='cb_" + arrValue[item] + "']").val();
                            AllGroup_Load(CompID, CompName);
                        }
                    }

                    if ($("#hidUseGroupID").val() != "") {
                        var arrValue = $("#hidUseGroupID").val().split(",").sort();
                        for (var item in arrValue) {
                            var CompID = arrValue[item].split("_")[0];
                            var paran = $("#divUseGroup input[id='cb_" + arrValue[item] + "']").val();
                            var CompName = paran.split(",")[0];
                            var GroupID = paran.split(",")[1];
                            var GroupName = paran.split(",")[2];
                            AllOrgan_Load(CompID, CompName, GroupID, GroupName);
                            $("#divUseGroup input[id='cb_" + arrValue[item] + "']").prop("checked", true);
                        }
                    }

                    if ($("#hidAllGroupIDFlag").val() != "") {
                        var arrValue = $("#hidAllGroupIDFlag").val().split(",").sort();
                        for (var item in arrValue) {
                            $("#divUseGroup input[id='cbAllGroupIDFlag_" + arrValue[item] + "']").prop("checked", true);
                        }
                    }

                    if ($("#hidUseOrganID").val() != "") {
                        $("#divUseOrgan input:checkbox").prop("checked", false);
                        var arrValue = $("#hidUseOrganID").val().split(",");
                        for (var item in arrValue) {
                            $("#divUseOrgan input[id='cb_" + arrValue[item] + "']").prop("checked", true);
                        }
                    }

                    if ($("#hidAllOrganIDFlag").val() != "") {
                        var arrValue = $("#hidAllOrganIDFlag").val().split(",");
                        for (var item in arrValue) {
                            $("#divUseOrgan input[id='cbAllOrganIDFlag_" + arrValue[item] + "']").prop("checked", true);
                        }
                    }

                    if ($("#hidAllCompOrganFlag").val() != "") {
                        var arrValue = $("#hidAllCompOrganFlag").val().split(",");
                        for (var item in arrValue) {
                            $("#divUseOrgan input[id='cbAllCompOrganFlag_" + arrValue[item] + "']").prop("checked", true);
                        }
                    }

                    if ($("#hidAllCompGroup").val() == "1") {
                        $("#cbAllCompGroup").prop("checked", true);
                    }

                    if ($("#hidAllCompGroupOrgan").val() == "1") {
                        $("#cbAllCompGroupOrgan").prop("checked", true);
                    }

                    if ($("#divUseGroup div[id^='divUseGroup_']").length == 0) {
                        $("#cbAllCompGroup").prop("checked", false).prop("disabled", true);
                    }

                    if ($("#divUseOrgan div[id^='divUseOrgan_']").length == 0) {
                        $("#cbAllCompGroupOrgan").prop("checked", false).prop("disabled", true);
                    }

                    hidePopupWindow();
                }, 50);
            }
        }
        //排除授權
        if ($("#rbGrantFlag0").prop("checked")) {
            if ($("#ddlUseCompID").val() != "") {
                showSubmitting();
                setTimeout(function () {
                    AllGroupOrgan_Load($("#ddlUseCompID").val());

                    if ($("#hidUseGroupOrganID").val() != "") {
                        var arrValue = $("#hidUseGroupOrganID").val().split(",");
                        for (var item in arrValue) {
                            $("#divUseGroupOrgan input[id='cb_" + arrValue[item] + "']").click();
                        }
                    }

                    if ($("#hidUseAllOrganIDFlag").val() != "") {
                        var arrValue = $("#hidUseAllOrganIDFlag").val().split(",");
                        for (var item in arrValue) {
                            $("#divUseGroupOrgan input[id='cbAllOrganIDFlag_" + arrValue[item] + "']").click();
                        }
                    }

                    hidePopupWindow();
                }, 50);
            }
        }
    }

    function SelectEmpName() {
        var result = true;
        $.ajax({
            type: "POST",
            url: "PA4101.aspx/SelectEmpName",
            data: "{EmpID: '" + $("#txtEmpID").val() + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (datas) {
                var data = $.parseJSON(datas.d);
                if (data.length > 0) {
                    $("#lblName").text(data[0].Name);
                }
                else {
                    $("#lblName").text("");
                    alert("[H_00000]：人事資料尚未建檔");
                    result = false;
                }
            }
        });
        return result;
    }

    function AllComp_Load() {
        $.ajax({
            type: "POST",
            url: "PA4101.aspx/CreateCompCBL",
            data: "{}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (data) {
                //公司總個數
                $.each($.parseJSON(data.d), function (i, item) {
                    var cbkbox = "<input type='checkbox' id='cb_" + item.CompID + "' name='cb_" + item.CompID + "' onclick='ChkComp_Change(\"" + item.CompID + "\", \"" + item.CompName + "\")' value='" + item.CompName + "' />";
                    cbkbox += "<label for='cb_" + item.CompID + "'>" + item.CompName + "</label><br />";
                    $("#divUseComp").append(cbkbox);
                });
            }
        });
    }

    function AllGroup_Load(CompID, CompName) {
        $.ajax({
            type: "POST",
            url: "PA4101.aspx/CreateGroupCBL",
            data: "{CompID: '" + CompID + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (datas) {
                var data = $.parseJSON(datas.d);
                if (data.length > 0) {
                    $("#cbAllCompGroup").prop("checked", false);
                    //公司事業群DIV
                    $("#divUseGroup").append("<div id='divUseGroup_" + CompID + "' class='listbox'></div>");

                    //事業群全選CheckBox
                    var cbkbox = "<input type='checkbox' id='cbAllGroupIDFlag_" + CompID + "' name='cbAllGroupIDFlag_" + CompID + "' onclick='AllGroup_Change(\"" + CompID + "\")' value='1' />";
                    cbkbox += "<label for='cbAllGroupIDFlag_" + CompID + "' style='background-color:yellow'>" + CompName + "-全事業群</label><br />";
                    $("#divUseGroup_" + CompID).append(cbkbox);

                    $.each(data, function (i, item) {
                        cbkbox = "<input type='checkbox' id='cb_" + CompID + "_" + item.GroupID + "' name='cb_" + CompID + "_" + item.GroupID + "' onclick='ChkGroup_Change(\"" + CompID + "\", \"" + CompName + "\", \"" + item.GroupID + "\", \"" + item.OrganName + "\")' value='" + CompName + "," + item.GroupID + "," + item.OrganName + "' />";
                        cbkbox += "<label for='cb_" + CompID + "_" + item.GroupID + "'>" + item.OrganName + "</label><br />";
                        $("#divUseGroup_" + CompID).append(cbkbox);
                    });
                }
                else {
                    //公司事業群DIV
                    $("#divUseGroup").append("<div id='divUseGroup_" + CompID + "' class='listbox nogroup' style='display:none;'></div>");

                    //事業群全選CheckBox
                    var cbkbox = "<input type='checkbox' id='cb_" + CompID + "_Null' name='cb_" + CompID + "_Null' value='1' checked='checked' />";
                    cbkbox += "<label for='cb_" + CompID + "_Null'>" + CompID + "-Null</label><br />";
                    $("#divUseGroup_" + CompID).append(cbkbox);

                    AllOrgan_Load(CompID, CompName, "Null", "Null");
                }
            }
        });
    }

    function AllOrgan_Load(CompID, CompName, GroupID, GroupName) {
        $.ajax({
            type: "POST",
            url: "PA4101.aspx/CreateOrganCBL",
            data: "{CompID: '" + CompID + "', GroupID: '" + GroupID + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (datas) {
                var data = $.parseJSON(datas.d);
                if (data.length > 0) {
                    $("#cbAllCompGroupOrgan").prop("checked", false);

                    if ($("#divUseOrgan_" + CompID).length == 0) {

                        //公司部門DIV
                        if (GroupID == "Null") {
                            $("#divUseOrgan").append("<div id='divUseOrgan_" + CompID + "' class='listbox nogroup'></div>");
                        } else {
                            $("#divUseOrgan").append("<div id='divUseOrgan_" + CompID + "' class='listbox'></div>");
                        }

                        //公司部門全選CheckBox
                        var cbkbox = "<input type='checkbox' id='cbAllCompOrganFlag_" + CompID + "' name='cbAllCompOrganFlag_" + CompID + "' onclick='AllOrgan_Change(\"" + CompID + "\", \"\")' value='1' />";
                        cbkbox += "<label for='cbAllCompOrganFlag_" + CompID + "'><span class='chkbox'>選單部門全選(輔助勾選)<br />" + CompName + "</span></label><br />";
                        $("#divUseOrgan_" + CompID).append(cbkbox);
                    }
                    else {
                        $("#cbAllCompOrganFlag_" + CompID).prop("checked", false);
                    }

                    //事業群部門DIV
                    $("#divUseOrgan_" + CompID).append("<div id='divUseOrgan_" + CompID + "_" + GroupID + "'></div>");

                    //事業群部門全選CheckBox
                    var cbkbox = "<input type='checkbox' id='cbAllOrganIDFlag_" + CompID + "_" + GroupID + "' name='cbAllOrganIDFlag_" + CompID + "_" + GroupID + "' onclick='AllOrgan_Change(\"" + CompID + "\", \"" + GroupID + "\")' value='1' />";
                    cbkbox += "<label for='cbAllOrganIDFlag_" + CompID + "_" + GroupID + "' style='background-color:yellow'>" + GroupName + "</label><br />";
                    if (GroupID == "Null") {
                        cbkbox = "<div style='display:none'>" + cbkbox + "</div>";
                    }
                    $("#divUseOrgan_" + CompID + "_" + GroupID).append(cbkbox + "<div class='orgbox'></div>");

                    $.each(data, function (i, item) {
                        cbkbox = "<input type='checkbox' id='cb_" + CompID + "_" + GroupID + "_" + item.OrganID + "' name='cb_" + CompID + "_" + GroupID + "_" + item.OrganID + "' onclick='ChkOrgan_Change(\"" + CompID + "\", \"" + GroupID + "\", \"" + item.OrganID + "\")' />";
                        cbkbox += "<label for='cb_" + CompID + "_" + GroupID + "_" + item.OrganID + "'>" + item.OrganName + "</label><br />";
                        $("#divUseOrgan_" + CompID + "_" + GroupID + " div.orgbox").append(cbkbox);
                    });
                }
            }
        });
    }

    //排除授權
    function AllGroupOrgan_Load(CompID) {
        if (CompID != "") {
            $.ajax({
                type: "POST",
                url: "PA4101.aspx/CreateGroupOrganCBL",
                data: "{CompID: '" + CompID + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (datas) {
                    var data = $.parseJSON(datas.d);
                    $("#divUseGroupOrgan").empty();
                    if (data.length > 0) {
                        var GroupID = "";
                        var LenOrgan = 0;
                        $.each(data, function (i, item) {
                            if (GroupID != item.GroupID) {
                                LenOrgan = 0;
                                GroupID = item.GroupID;

                                //事業群部門DIV
                                $("#divUseGroupOrgan").append("<div id='divUseOrgan_" + CompID + "_" + GroupID + "'></div>");

                                //事業群部門全選CheckBox
                                var cbkbox = "<input type='checkbox' id='cbAllOrganIDFlag_" + CompID + "_" + GroupID + "' name='cbAllOrganIDFlag_" + CompID + "_" + GroupID + "' onclick='AllOrgan_Change(\"" + CompID + "\", \"" + GroupID + "\")' value='1' />";
                                
                                if (GroupID == "Null") {
                                    cbkbox += "<label for='cbAllOrganIDFlag_" + CompID + "_" + GroupID + "'>全選</label><br />";
                                } else {
                                    cbkbox += "<label for='cbAllOrganIDFlag_" + CompID + "_" + GroupID + "'>全選 " + item.GroupName + "</label><br />";
                                }
                                $("#divUseOrgan_" + CompID + "_" + GroupID).append(cbkbox + "<div class='orgbox'></div>");
                            }

                            cbkbox = "<input type='checkbox' id='cb_" + CompID + "_" + GroupID + "_" + item.OrganID + "' name='cb_" + CompID + "_" + GroupID + "_" + item.OrganID + "' onclick='ChkOrgan_Change(\"" + CompID + "\", \"" + GroupID + "\", \"" + item.OrganID + "\")' />";
                            cbkbox += "<label for='cb_" + CompID + "_" + GroupID + "_" + item.OrganID + "'>" + item.OrganName + "</label><br />";
                            $("#divUseOrgan_" + CompID + "_" + GroupID + " div.orgbox").append(cbkbox);
                        });
                    }
                }
            });
        } else $("#divUseGroupOrgan").empty();

    }

    //排除授權
    function CompChange(e) {
        showSubmitting();
        setTimeout(function () {
            AllGroupOrgan_Load($(e).val());
            hidePopupWindow();
        }, 50);
    }

    //公司全選
    function AllComp_Change(e) {
        if ($(e).prop("checked")) {
            $("#divUseComp input:checkbox").prop("checked", true).prop("disabled", true);
            $("#cbAllCompGroup").prop("checked", true).prop("disabled", true);
            $("#cbAllCompGroupOrgan").prop("checked", true).prop("disabled", true);
            $("#divUseGroup").empty();
            $("#divUseOrgan").empty();
        } else {
            $("#divUseComp input:checkbox").prop("checked", false).prop("disabled", false);
            $("#cbAllCompGroup").prop("checked", false).prop("disabled", false);
            $("#cbAllCompGroupOrgan").prop("checked", false).prop("disabled", false);
        }
    }

    //公司個別選
    function ChkComp_Change(CompID, CompName) {
        if ($("#cb_" + CompID).prop("checked")) {
            showSubmitting();
            setTimeout(function () {
                AllGroup_Load(CompID, CompName);
                $("#cbAllCompGroup").prop("disabled", false);
                if ($("#divUseOrgan div[id^='divUseOrgan_']").length > 0) {
                    $("#cbAllCompGroupOrgan").prop("disabled", false);
                }
                hidePopupWindow();
            }, 50);
        } else {
            $("#divUseGroup_" + CompID).remove();
            $("#divUseOrgan_" + CompID).remove();
            if ($("#divUseGroup input").length == 0) {
                $("#cbAllCompGroup").prop("checked", false);
            }
            if ($("#divUseOrgan input").length == 0) {
                $("#cbAllCompGroupOrgan").prop("checked", false);
            }
            if ($("#divUseGroup div[id^='divUseGroup_']").length == 0) {
                $("#cbAllCompGroup").prop("disabled", true);
            }
            if ($("#divUseOrgan div[id^='divUseOrgan_']").length == 0) {
                $("#cbAllCompGroupOrgan").prop("disabled", true);
            }
        }
    }

    //跨公司事業群全選
    function AllCompGroup_Change(e) {
        if ($("#divUseGroup input:checkbox").length > 0) {
            if ($(e).prop("checked")) {
                showSubmitting();
                setTimeout(function () {
                    $("#divUseGroup input[id^='cbAllGroupIDFlag_']").not(":checked").each(function () {
                        $(this).prop("checked", true);
                        var CompID = $(this).attr("id").replace("cbAllGroupIDFlag_", "");

                        $("#divUseGroup_" + CompID + " input[id^='cb_']").not(":checked").each(function () {
                            var paran = $(this).val();
                            var CompName = paran.split(",")[0];
                            var GroupID = paran.split(",")[1];
                            var GroupName = paran.split(",")[2];

                            AllOrgan_Load(CompID, CompName, GroupID, GroupName);
                        });
                        $("#divUseGroup_" + CompID + " input:checkbox").prop("checked", true);
                        $("#divUseOrgan_" + CompID + " input:checkbox").prop("checked", true);
                    });
                    $("#cbAllCompGroupOrgan").prop("checked", true).prop("disabled", false);
                    AllCompGroupOrgan_Change($("#cbAllCompGroupOrgan"));
                    hidePopupWindow();
                }, 50);
            } else {
                $("#divUseGroup div.listbox").not(".nogroup").find("input").prop("checked", false);
                $("#divUseOrgan div.listbox").not(".nogroup").remove();
                $("#cbAllCompGroupOrgan").prop("checked", false);

                //【跨公司部門】自動disable
                if ($("#divUseOrgan div[id^='divUseOrgan_']").length == 0) {
                    $("#cbAllCompGroupOrgan").prop("disabled", true);
                }
            }
        }
    }

    //事業群全選
    function AllGroup_Change(CompID) {
        showSubmitting();
        setTimeout(function () {
            if ($("#cbAllGroupIDFlag_" + CompID).prop("checked")) {
                $("#divUseGroup_" + CompID + " input[id^='cb_']").not(":checked").each(function () {
                    var paran = $(this).val();
                    var CompName = paran.split(",")[0];
                    var GroupID = paran.split(",")[1];
                    var GroupName = paran.split(",")[2];

                    AllOrgan_Load(CompID, CompName, GroupID, GroupName);
                });
                $("#divUseGroup_" + CompID + " input:checkbox").prop("checked", true);
                $("#divUseOrgan_" + CompID + " input:checkbox").prop("checked", true);
                $("#cbAllCompGroupOrgan").prop("disabled", false);
            } else {
                $("#divUseGroup_" + CompID + " input:checkbox").prop("checked", false);
                $("#divUseOrgan_" + CompID).remove();
            }

            //【跨公司事業群】自動取消全選
            if ($("#divUseGroup input:checkbox").not(":checked").length > 0) {
                $("#cbAllCompGroup").prop("checked", false);
            }

            //【跨公司部門】自動disable
            if ($("#divUseOrgan div[id^='divUseOrgan_']").length == 0) {
                $("#cbAllCompGroupOrgan").prop("disabled", true);
            }

            hidePopupWindow();
        }, 50);
    }

    //事業群個別選
    function ChkGroup_Change(CompID, CompName, GroupID, GroupName) {
        showSubmitting();
        setTimeout(function () { 
            if ($("#cb_" + CompID + "_" + GroupID).prop("checked")) {
                AllOrgan_Load(CompID, CompName, GroupID, GroupName);
                $("#cbAllCompGroupOrgan").prop("disabled", false);
            } else {
                if ($("#divUseGroup_" + CompID + " input:checked").length == 0 && $("div[id^='divUseOrgan_" + CompID + "_']").length == 1) {
                    $("#divUseOrgan_" + CompID).remove();
                } else {
                    $("#divUseOrgan_" + CompID + "_" + GroupID).remove();
                }
            }

            //【公司事業群】自動取消全選
            if ($("#divUseGroup_" + CompID + " input:checkbox").not(":checked").length > 0) {
                $("#cbAllGroupIDFlag_" + CompID).prop("checked", false);
            }

            //【跨公司事業群】自動勾全選或取消全選
            if ($("#divUseGroup input:checkbox").not(":checked").length > 0) {
                $("#cbAllCompGroup").prop("checked", false);
            }

            //【跨公司部門】自動取消全選
            if ($("#divUseOrgan input:checkbox").length == 0 || $("#divUseOrgan input:checkbox").not(":checked").length > 0) {
                $("#cbAllCompGroupOrgan").prop("checked", false);
            }

            //【跨公司部門】自動disable
            if ($("#divUseOrgan div[id^='divUseOrgan_']").length == 0) {
                $("#cbAllCompGroupOrgan").prop("disabled", true);
            }

            hidePopupWindow();
        }, 50);
    }


    //跨公司部門全選
    function AllCompGroupOrgan_Change(e) {
        if ($(e).prop("checked")) {
            $("#divUseOrgan input:checkbox").prop("checked", true);
        } else {
            $("#divUseOrgan input:checkbox").prop("checked", false);
        }
    }

    //公司部門全選
    function AllOrgan_Change(CompID, GroupID) {
        if (GroupID == "") {
            if ($("#cbAllCompOrganFlag_" + CompID).prop("checked")) {
                $("#divUseOrgan_" + CompID + " input:checkbox").prop("checked", true);
            } else {
                $("#divUseOrgan_" + CompID + " input:checkbox").prop("checked", false);
                $("#cbAllGroupIDFlag_" + CompID).prop("checked", false);
                $("#cbAllCompGroup").prop("checked", false);
            }
        } else {
            //事業群全部門
            if ($("#cbAllOrganIDFlag_" + CompID + "_" + GroupID).prop("checked")) {
                $("#divUseOrgan_" + CompID + "_" + GroupID + " input:checkbox").prop("checked", true);
            } else {
                $("#divUseOrgan_" + CompID + "_" + GroupID + " input:checkbox").prop("checked", false);
                $("#cbAllGroupIDFlag_" + CompID).prop("checked", false);
                $("#cbAllCompGroup").prop("checked", false);
            }
            //公司全部門
            if ($("#divUseOrgan_" + CompID + " input:checkbox").not(":checked").length> 1) {
                $("#cbAllCompOrganFlag_" + CompID).prop("checked", false);
            }
        }

        //跨公司全部門
        if ($("#divUseOrgan input:checkbox").not(":checked").length > 0) {
            $("#cbAllCompGroupOrgan").prop("checked", false);
        }
    }

    //部門個別選
    function ChkOrgan_Change(CompID, GroupID, OrganID) {
        //【事業群全部門】自動取消全選
        $("#cbAllOrganIDFlag_" + CompID + "_" + GroupID).prop("checked", false);
        $("#cbAllGroupIDFlag_" + CompID).prop("checked", false);
        $("#cbAllCompGroup").prop("checked", false);

        //【公司全部門】自動取消全選
        if ($("#divUseOrgan_" + CompID + " input[id^='cb_" + CompID + "']").not(":checked").length > 0) {
            $("#cbAllCompOrganFlag_" + CompID).prop("checked", false);
        }

        //【跨公司】自動取消全選
        if ($("#divUseOrgan input:checkbox").not(":checked").length > 0) {
            $("#cbAllCompGroupOrgan").prop("checked", false);
        }
    }

    function SaveCheckValue() {
        //授權
        var hid = "";
        if ($("#rbGrantFlag1").prop("checked")) {

            //公司/事業群/部門全選 
            if ($("#cbAllCompIDFlag").prop("checked")) {
                $("#hidAllCompIDFlag").val("1");
            } else {
                $("#hidAllCompIDFlag").val("0");

                //跨公司事業群全選 
                if ($("#cbAllCompGroup").prop("checked")) {
                    $("#hidAllCompGroup").val("1");
                } else {
                    $("#hidAllCompGroup").val("0");
                }

                //跨公司部門全選
                if ($("#cbAllCompGroupOrgan").prop("checked")) {
                    $("#hidAllCompGroupOrgan").val("1");
                } else {
                    $("#hidAllCompGroupOrgan").val("0");
                }

                //事業群全選
                hid = "";
                $("#divUseGroup input[id^='cbAllGroupIDFlag_']:checked").each(function () {
                    hid += "," + $(this).attr("id").substring(17);
                });
                $("#hidAllGroupIDFlag").val(hid.substring(1));

                //公司部門全選
                hid = "";
                $("#divUseOrgan input[id^='cbAllCompOrganFlag_']:checked").each(function () {
                    hid += "," + $(this).attr("id").substring(19);
                });
                $("#hidAllCompOrganFlag").val(hid.substring(1));

                //部門全選
                hid = "";
                $("#divUseOrgan input[id^='cbAllOrganIDFlag_']:checked").each(function () {
                    hid += "," + $(this).attr("id").substring(17);
                });
                $("#hidAllOrganIDFlag").val(hid.substring(1));

                //公司
                hid = "";
                $("#divUseComp input[id^='cb_']:checked").each(function () {
                    hid += "," + $(this).attr("id").substring(3);
                });
                $("#hidUseCompID").val(hid.substring(1));

                //事業群
                hid = "";
                $("#divUseGroup input[id^='cb_']:checked").each(function () {
                    hid += "," + $(this).attr("id").substring(3);
                });
                $("#hidUseGroupID").val(hid.substring(1));

                //部門
                hid = "";
                $("#divUseOrgan input[id^='cb_']:checked").each(function () {
                    hid += "," + $(this).attr("id").substring(3);
                });
                $("#hidUseOrganID").val(hid.substring(1));
            }
        }
        //排除授權
        if ($("#rbGrantFlag0").prop("checked")) {
            hid = "";
            $("#divUseGroupOrgan input[id^='cbAllOrganIDFlag_']:checked").each(function () {
                hid += "," + $(this).attr("id").substring(17);
            });
            $("#hidUseAllOrganIDFlag").val(hid.substring(1));

            hid = "";
            $("#divUseGroupOrgan input[id^='cb_']:checked").each(function () {
                hid += "," + $(this).attr("id").substring(3);
            });
            $("#hidUseGroupOrganID").val(hid.substring(1));
        }
    }

    function funAction(Param) {
        if (Param == "btnAdd") {
            var error = 0;
            if ($("#txtEmpID").val() == "") {
                alert("[W_00030]：欄位[*員工編號]未輸入！");
                return false;
            }

            if (!SelectEmpName()) {
                return false;
            }

            //授權
            if ($("#rbGrantFlag1").prop("checked")) {
                if (!$("#cbAllCompIDFlag").prop("checked")) {
                    if ($("#divUseComp input:checked").length == 0) {
                        alert("[W_00030]：欄位[*查詢權限公司]未輸入！");
                        return false;
                    }

                    if ($("#divUseGroup input:checked").length == 0) {
                        alert("[W_00030]：欄位[*查詢權限事業群]未輸入！");
                        return false;
                    }

                    if ($("#divUseOrgan input:checked").length == 0) {
                        alert("[W_00030]：欄位[*查詢權限部門]未輸入！");
                        return false;
                    }

                    error = 0;
                    $("div[id^='divUseGroup_']").each(function () {
                        if ($(this).find("input:checked").length == 0) {
                            var id = $(this).attr("id").replace("divUseGroup", "cbAllGroupIDFlag");
                            var comp = $(this).find("label[for='" + id + "']").text().replace("-全事業群", "").trim();
                            alert("請勾選[" + comp + "]公司對應事業群！");
                            error++;
                            return false;
                        }
                    });

                    if (error > 0) {
                        return false;
                    }

                    error = 0;
                    $("div.listbox[id^='divUseOrgan_']").each(function () {
                        var compid = $(this).attr("id").replace("divUseOrgan_", "cbAllCompOrganFlag_");
                        var comp = $(this).find("label[for='" + compid + "']").text().replace("選單部門全選(輔助勾選)", "").trim();
                        $(this).find("div[id^='divUseOrgan_']").each(function () {
                            if ($(this).find("input:checked").length == 0) {
                                var groupid = $(this).attr("id").replace("divUseOrgan", "cbAllOrganIDFlag");
                                var group = $(this).find("label[for='" + groupid + "']").text().replace("全選", "").trim();
                                alert("請勾選[" + comp + "][" + group + "]事業群對應部門！");
                                error++;
                                return false;
                            }
                        });
                        if (error > 0) {
                            return false;
                        }
                    });

                    if (error > 0) {
                        return false;
                    }
                }

                if ($("#ddlUseRankID").val() == "") {
                    alert("[W_00030]：欄位[*金控職等]未輸入！");
                    return false;
                }

                if ($("#txtBeginDate_txtDate").val() == "") {
                    alert("[W_00030]：欄位[*查詢起日]未輸入！");
                    return false;
                }

                if (!IsValidYYYYsMMsDD($("#txtBeginDate_txtDate").val())) {
                    alert("[W_00060]：欄位[*查詢起日]請輸入日期型態！");
                    return false;
                }

                if ($("#txtEndDate_txtDate").val() == "") {
                    alert("[W_00030]：欄位[*查詢迄日]未輸入！");
                    return false;
                }

                if (!IsValidYYYYsMMsDD($("#txtEndDate_txtDate").val())) {
                    alert("[W_00060]：欄位[*查詢起日]請輸入日期型態！");
                    return false;
                }

                if ($("#txtBeginDate_txtDate").val() > $("#txtEndDate_txtDate").val()) {
                    alert("[W_00130]：起日不可晚於迄日！");
                    return false;
                }

            } else if ($("#rbGrantFlag0").prop("checked")) {
                //排除授權
                if ($("#ddlUseCompID").val() == "") {
                    alert("[W_00030]：欄位[*查詢權限公司]未輸入！");
                    return false;
                }
                if ($("#divUseGroupOrgan input:checked").length == 0) {
                    alert("[W_00030]：欄位[*查詢權限部門]未輸入！");
                    return false;
                }
            }
            SaveCheckValue();
        }
    }
-->
</script>
</html>
