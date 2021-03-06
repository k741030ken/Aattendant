<%@ Page Language="VB" AutoEventWireup="false" CodeFile="PA4402.aspx.vb" Inherits="PA_PA4402" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <link type="text/css" rel="stylesheet" href="../StyleSheet.Css" />
    <link type="text/css" rel="stylesheet" href="../css/spectrum/spectrum.css" />
    <style type="text/css">
    .full-spectrum
    { 
	    border-right: #5384e6 1px solid; 
	    border-top: #5384e6 1px solid; 
	    border-left: #5384e6 1px solid; 
	    border-bottom: #5384e6 1px solid; 
	    background-color: #e2e9fe;
    }
    .tr-hide
    {
    	display: none;
    }
    </style>
    <script type="text/javascript" src="../ClientFun/jquery-1.8.3.min.js"></script>
    <script type="text/javascript" src="../css/spectrum/spectrum.js"></script>
    <script type="text/javascript">
    <!--
        $(function () {
            $("#full_S, #full_M").spectrum({
                allowEmpty: true,
                color: initColor(),
                replacerClassName: "full-spectrum",
                containerClassName: "full-spectrum",
                showInput: true,
                showPalette: true,
                preferredFormat: "hex",
                hide: function (color) {
                    updateColor(color);
                },
                palette: [
                ["rgb(0, 0, 0)", "rgb(67, 67, 67)", "rgb(102, 102, 102)", "rgb(153, 153, 153)", "rgb(183, 183, 183)",
                "rgb(204, 204, 204)", "rgb(217, 217, 217)", "rgb(239, 239, 239)", "rgb(243, 243, 243)", "rgb(255, 255, 255)"],
                ["rgb(152, 0, 0)", "rgb(255, 0, 0)", "rgb(255, 153, 0)", "rgb(255, 255, 0)", "rgb(0, 255, 0)",
                "rgb(0, 255, 255)", "rgb(74, 134, 232)", "rgb(0, 0, 255)", "rgb(153, 0, 255)", "rgb(255, 0, 255)"],
                ["rgb(230, 184, 175)", "rgb(244, 204, 204)", "rgb(252, 229, 205)", "rgb(255, 242, 204)", "rgb(217, 234, 211)",
                "rgb(208, 224, 227)", "rgb(201, 218, 248)", "rgb(207, 226, 243)", "rgb(217, 210, 233)", "rgb(234, 209, 220)"],
                ["rgb(221, 126, 107)", "rgb(234, 153, 153)", "rgb(249, 203, 156)", "rgb(255, 229, 153)", "rgb(182, 215, 168)",
                "rgb(162, 196, 201)", "rgb(164, 194, 244)", "rgb(159, 197, 232)", "rgb(180, 167, 214)", "rgb(213, 166, 189)"],
                ["rgb(204, 65, 37)", "rgb(224, 102, 102)", "rgb(246, 178, 107)", "rgb(255, 217, 102)", "rgb(147, 196, 125)",
                "rgb(118, 165, 175)", "rgb(109, 158, 235)", "rgb(111, 168, 220)", "rgb(142, 124, 195)", "rgb(194, 123, 160)"],
                ["rgb(166, 28, 0)", "rgb(204, 0, 0)", "rgb(230, 145, 56)", "rgb(241, 194, 50)", "rgb(106, 168, 79)",
                "rgb(69, 129, 142)", "rgb(60, 120, 216)", "rgb(61, 133, 198)", "rgb(103, 78, 167)", "rgb(166, 77, 121)"],
                ["rgb(133, 32, 12)", "rgb(153, 0, 0)", "rgb(180, 95, 6)", "rgb(191, 144, 0)", "rgb(56, 118, 29)",
                "rgb(19, 79, 92)", "rgb(17, 85, 204)", "rgb(11, 83, 148)", "rgb(53, 28, 117)", "rgb(116, 27, 71)"],
                ["rgb(91, 15, 0)", "rgb(102, 0, 0)", "rgb(120, 63, 4)", "rgb(127, 96, 0)", "rgb(39, 78, 19)",
                "rgb(12, 52, 61)", "rgb(28, 69, 135)", "rgb(7, 55, 99)", "rgb(32, 18, 77)", "rgb(76, 17, 48)"]
                ]
            });
        })

        function initColor() {
            var hexColor = "";

            if ($("#SingleEdit").prop("checked")) {
                hexColor = $("#txtColor").val();
            }
            else {
                hexColor = $("#txtNewColor").val();
            }

            if (hexColor == "") {
                hexColor = "#F0F8FF";
            }
            return hexColor;
        }

        function updateColor(color) {
            var hexColor = "";
            if (color) {
                hexColor = color.toHexString();
            }

            if ($("#SingleEdit").prop("checked")) {
                $("#txtColor").val(hexColor.toUpperCase());
                $("#hldColor").val(hexColor.toUpperCase());
            }
            else {
                $("#txtNewColor").val(hexColor.toUpperCase());
                $("#hldNewColor").val(hexColor.toUpperCase());
            }
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
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 100%">
                        <tr>
                            <td align="center">
                                <table width="80%" class="tbl_Edit" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="Label2" ForeColor="Blue" runat="server" Text="*請選擇修改方式"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left" colspan="3">
                                            <asp:RadioButton ID="SingleEdit" Text="單筆修改" GroupName="Edit" runat="server" AutoPostBack="true" Checked="true" />
                                            <asp:RadioButton ID="MultiEdit" Text="多筆修改" GroupName="Edit" runat="server" AutoPostBack="true" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblComID" ForeColor="Blue" runat="server" Text="*公司代碼"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left" colspan="3">
                                            <asp:Label ID="txtComID" runat="server"></asp:Label>
                                            <asp:Label ID="Label1" runat="server" Text="-"></asp:Label>
                                            <asp:Label ID="txtComName" runat="server"></asp:Label>
                                        </td>
                                    </tr> 
                                    <tr id="trSortOrder" class="" style="height:20px" runat="server">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblSortOrder" ForeColor="Blue" runat="server" Text="*排序(SortOrder)"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left" colspan="3">
                                            <asp:TextBox ID="txtSortOrder" runat="server" CssClass="InputTextStyle_Thin" MaxLength="6"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr id="trColor" class="" style="height:20px" runat="server">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblColor" runat="server" Text="顏色代碼"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left" colspan="3">
                                            <asp:TextBox ID="txtColor" CssClass="InputTextStyle_Thin" MaxLength="7" Enabled="false" runat="server"></asp:TextBox>
                                            <asp:HiddenField ID="hldColor" Value="" runat="server" />
                                            <input id="full_S" />
                                        </td>
                                    </tr>
                                    <tr id="trOldColor" class="tr-hide" style="height:20px" runat="server">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblOldColor" runat="server" Text="修改前顏色代碼"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left" colspan="3">
                                            <asp:Label ID="txtOldColor" runat="server"></asp:Label>
                                            <asp:Panel ID="divOldColor" Width="40" Height="30" style="display:inline-block; margin:5px; border:1px solid black;" runat="server"></asp:Panel>
                                        </td>
                                    </tr>
                                    <tr id="trNewColor" class="tr-hide" style="height:20px" runat="server">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblNewColor" runat="server" Text="修改後顏色代碼"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left" colspan="3">
                                            <asp:TextBox ID="txtNewColor" runat="server" CssClass="InputTextStyle_Thin" Enabled="false" MaxLength="7"></asp:TextBox>
                                            <asp:HiddenField ID="hldNewColor" Value="" runat="server" />
                                            <input id="full_M" />
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblLastChgComp" runat="server" Text="最後異動公司"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left" colspan="3">
                                            <asp:Label ID="txtLastChgComp" runat="server" ></asp:Label>
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblLastChgID" runat="server" Text="最後異動人員"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left" colspan="3">
                                            <asp:Label ID="txtLastChgID" runat="server" > </asp:Label>
                                        </td>
                                    </tr>
                                    <tr style="height:20px">
                                        <td class="td_EditHeader" width="15%" align="center">
                                            <asp:Label ID="lblLastChgDate" runat="server" Text="最後異動日期"></asp:Label>
                                        </td>
                                        <td class="td_Edit" style="width:35%" align="left" colspan="3">
                                            <asp:Label ID="txtLastChgDate" runat="server" ></asp:Label>
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
