<%@ Page Language="VB" AutoEventWireup="false" CodeFile="GS1201.aspx.vb" Inherits="GS_GS1201" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <link type="text/css" rel="stylesheet" href="../StyleSheet.Css" />
    <script type="text/javascript">
    <!--
        //debugger;
        function funAction(Param) {
            //debugger;
            if (Param == 'btnActionX') {
                window.top.close();
            }
        }

        function IsTOConfirm(Param) {
            var strMsg;
            strMsg = "相同資料已經存在，確定全部刪除，重新上傳?!?";

            switch (Param) {
                case "":
                    if (!confirm(strMsg)) {
                        return false;
                        break;
                    }
                    fromField = document.getElementById("btnUpd");
                    fromField.click()
            }          
        }

        function IsTODownload(Param) {
            var strMsg;
            strMsg = "HR3000資料上傳-檔案檢核失敗，請下載錯誤紀錄log檔案!";
            alert(Param);
            fromField = document.getElementById("btnUpd1");
            fromField.click();
//            switch (Param) {
//                case "":
//                    alert(strMsg);
//                    fromField = document.getElementById("btnUpd1");
//                    fromField.click();
//            }
        }
                              
        function onShowModalReturn(returnValue)
        {
            if (returnValue == undefined || returnValue == '')
                return false
            else
                return true;
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
                <td align="center" width="100%">
                    <table width="95%" class="tbl_Edit" cellpadding="1" cellspacing="1">
                        <tr>                            
                            <td class="td_EditHeader" width="25%" align="center"><asp:Label ID="Label1" cssclass="MustInputCaption" runat="server" Text="說明" Font-Names="微軟正黑體"></asp:Label>
                            </td>
                            <td class="td_Edit" style="width:75%" align="left">
                                <asp:Label ID="Label2" cssclass="MustInputCaption" runat="server" Font-Names="微軟正黑體"
                                    Text="1.檔案格式XLS<br>2.<font color='red'>請保留標題欄位列<br>(系統判斷上傳檔案第一列資料為欄位標題)</font><br>3.工作表(WorkSheet)名稱為<font color='red'>GradeUpload</font>，請勿調整<br>4.請依專用格式下傳，調整黃底欄位後上傳"></asp:Label>  
                                <br />
                                <asp:Button ID="btnDownload" runat="server" Text="專用格式下傳" Font-Names="微軟正黑體" />
                                <asp:Button ID = "btnUpd" runat="server" Text="" Width = "0px" style="display:none;"></asp:Button>
                                <asp:Button ID = "btnUpd1" runat="server" Text="" Width = "0px" style="display:none;"></asp:Button>
                                <asp:HiddenField ID="IsDataExist" runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="hidFileUploadFileName" runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="hidLogFileName" runat="server"></asp:HiddenField>                             
                            </td>
                        </tr>        
                        <tr>
                            <td class="td_EditHeader" width="25%" align="center"><asp:Label ID="Labe3" cssclass="MustInputCaption" runat="server" Text="*檔案路徑" Font-Names="微軟正黑體"></asp:Label>
                            </td>
                            <td class="td_Edit" style="width:75%" align="left">
                                <asp:FileUpload ID="FileUpload" runat="server" Width="100%" CssClass="InputTextStyle_Thin" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
