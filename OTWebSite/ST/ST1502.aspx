<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ST1502.aspx.vb" Inherits="ST_ST1502" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <link type="text/css" rel="stylesheet" href="../StyleSheet.Css" />
    <script type="text/javascript" src="../ClientFun/jquery-1.8.3.min.js"></script>
    <script type="text/javascript">
    <!--
        $(function () {
            $("#txtBeginDate_txtDate").blur(function () {
                var StartDate = document.getElementById('txtBeginDate_txtDate').value
                var EndDate = document.getElementById('txtEndDate_txtDate').value
                var myStartDate = new Date(StartDate)
                var myEndDate = new Date(EndDate)
//                var workyear = new Number()

                if (myStartDate != "NaN" && myEndDate != "NaN") {
                    if ((myEndDate - myStartDate) < 0) {
                        $("#txtWorkYear").html("")
                        $("#txtWorkYear").val("")
                        alert("迄日不得大於等於起日");
                    } else {
//                        workyear = ((myEndDate - myStartDate) / 86400000 / 365) //2015/11/24 Add
//                        $("#txtWorkYear").html(workyear.toFixed(1))
//                        $("#txtWorkYear").val(workyear.toFixed(1))
//                        $("#hldWorkYear").val(workyear.toFixed(1));
                    }
                }
            });

            $("#txtEndDate_txtDate").blur(function () {
                var StartDate = document.getElementById('txtBeginDate_txtDate').value
                var EndDate = document.getElementById('txtEndDate_txtDate').value
                var myStartDate = new Date(StartDate)
                var myEndDate = new Date(EndDate)

                if (myStartDate != "NaN" && myEndDate != "NaN") {
                    if ((myEndDate - myStartDate) < 0) {
                        $("#txtWorkYear").html("")
                        $("#txtWorkYear").val("")
                        alert("迄日不得大於等於起日");
                    } else {
//                        workyear = ((myEndDate - myStartDate) / 86400000 / 365)  //2015/11/24 Add
//                        $("#txtWorkYear").html(workyear.toFixed(1));
//                        $("#txtWorkYear").val(workyear.toFixed(1));
//                        $("#hldWorkYear").val(workyear.toFixed(1));
                    }
                }
            });
        });

        //2015/11/24 Add 
        $(function () {
            $("#txtBeginDate_txtDate").change(function () {
                $("#btnWorkYear").click();
            });
        });

        //2015/11/24 Add 
        $(function () {
            $("#txtEndDate_txtDate").change(function () {
                $("#btnWorkYear").click();
            });
        });

        //        隱藏TR
        function hide_tr(strID) {
            var result_style = document.getElementById(strID).style; result_style.display = 'none';
        }
        //        顯示TR
        function show_tr(strID) {
            var result_style = document.getElementById(strID).style; result_style.display = 'table-row';
        }
        function ChangeValue(e) {
            alert(e.id);         
        }
    -->
    </script>
    <style type="text/css">
        .style1
        {
            border: 1px solid #5384e6;
            font-size: 14px;
            background-color: #e2e9fe;
            height: 20px;
        }
        .style2
        {
            border: 1px solid #89b3f5;
            font-size: 14px;
            width: 35%;
            height: 20px;
        }
    </style>
</head>
<body style="margin-top:5px; margin-left:5px; margin-right:5px; margin-bottom:0" >
    <form id="frmContent" runat="server">
        <ajaxToolkit:ToolkitScriptManager runat="Server" EnableScriptGlobalization="true"
            EnableScriptLocalization="true" ID="ScriptManager1" />
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
                                        <td class="td_Edit" style="width:35%" align="left" colspan="3">
                                            <asp:Label ID="txtCompID" runat="server" ></asp:Label>
                                            <asp:HiddenField ID="hidCompID" runat="server"></asp:HiddenField>
                                            <asp:HiddenField ID="hidIDNo" runat="server"></asp:HiddenField>
                                            <asp:HiddenField ID="hidSeq" runat="server"></asp:HiddenField>
                                        </td>
                                    </tr> 
                                    <tr>
                                        <td class="style1" width="15%" align="center">
                                            <asp:Label ID="lblEmpID" runat="server" Text="員工編號"></asp:Label>
                                        </td>
                                        <td class="style2" align="left" colspan="3">
                                            <asp:Label ID="txtEmpID" runat="server" ></asp:Label>
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblEmpName" runat="server" Text="員工姓名"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left" colspan="3">
                                            <asp:Label ID="txtEmpName" runat="server" ></asp:Label>
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblBeginDate" ForeColor="Blue" runat="server" Text="*起日"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left" colspan="3">
                                            <uc:uccalender ID="txtBeginDate" runat="server" Enabled="True" />
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblEndDate" runat="server" ForeColor="Blue" Text="*迄日"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left" colspan="3">
                                            <uc:uccalender ID="txtEndDate" runat="server" Enabled="True" />
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblWorkYear" runat="server" Text="年資"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left" colspan="3">
                                            <asp:Label ID="txtWorkYear" runat="server" ></asp:Label>
                                            <asp:Button ID="btnWorkYear" runat="server" style="display:none" />
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblCompany" runat="server" ForeColor="Blue" Text="*公司名稱"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left" colspan="3">
                                            <asp:TextBox ID="txtCompany" CssClass="InputTextStyle_Thin" runat="server" MaxLength="50"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblDepartment" runat="server" ForeColor="Blue" Text="*部門"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left" colspan="3">
                                            <asp:TextBox ID="txtDepartment" CssClass="InputTextStyle_Thin" runat="server" MaxLength="50"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="Label1" runat="server" Text="產業別"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left" colspan="3">
                                            <asp:DropDownList ID="ddlIndustryType" runat="server" Font-Names="細明體"></asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblTitle" runat="server" Text="職稱"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left" colspan="3">
                                            <asp:TextBox ID="txtTitle" CssClass="InputTextStyle_Thin" runat="server" MaxLength="20"></asp:TextBox>                                            
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblWorkType" runat="server" Text="工作性質"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left" colspan="3">
                                            <asp:TextBox ID="txtWorkType" CssClass="InputTextStyle_Thin" runat="server" MaxLength="50"></asp:TextBox>                                            
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblProfession" runat="server" ForeColor="Blue" Text="*專業記號"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left" colspan="3">
                                            <asp:DropDownList ID="ddlProfession" runat="server" Font-Names="細明體">
                                                <asp:ListItem Value="0" Text="0-非專業"></asp:ListItem>
                                                <asp:ListItem Value="1" Text="1-專業"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblLastChgComp1" runat="server" Text="最後異動公司"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left" colspan="3">
                                            <asp:Label ID="lblLastChgComp" runat="server" ></asp:Label>
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblLastChgID1" runat="server" Text="最後異動人員"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left" colspan="3">
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
