﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OvertimePreOrder_Add.aspx.cs" Inherits="OverTime_OvertimePreOrder_Add" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../Util/ucDate.ascx" TagName="ucDate" TagPrefix="asp" %>
<%@ Register Src="../Util/ucTimePickerNew.ascx" TagName="ucTimePickerNew" TagPrefix="asp" %>
<%@ Register src="../Util/ucCascadingDropDown.ascx" tagname="ucCascadingDropDown" tagprefix="uc1" %>
<%@ Register src="../Util/ucModalPopup.ascx" tagname="ucModalPopup" tagprefix="uc1" %>
<%@ Register Src="../Util/ucGridView.ascx" TagName="ucGridView" TagPrefix="uc1" %>
<%@ Register Src="../Util/ucTextBox.ascx" TagName="ucTextBox" TagPrefix="uc1" %>
<%@ Register Src="~/Util/ucLightBox.ascx" TagPrefix="uc1" TagName="ucLightBox" %>
<!DOCTYPE>
<html>
<head id="Head1" runat="server">
    <title>預先申請</title>
    
    <style type="text/css">
    body
    {
        font-family: "微軟正黑體", "微软雅黑", "Microsoft JhengHei", Arial, sans-serif;
        color :#000000;
    }
    /* 唯讀下拉選單 */
    .clsDropDownListReadOnly {
    color: #000000;
    background-color: white;
    -webkit-appearance: none;
    -moz-appearance: none;
}
    /* 下拉選單 */
    .clsDropDownList {
    color: #000000;
    background-color: white;
    -webkit-appearance: none;
    -moz-appearance: none;
}
</style>
</head>

<script type="text/javascript" src="../Util/WebClient/jquery-1.8.3.min.js"></script>
    <script type="text/javascript">
        function SelectAllCheckboxes(spanChk) {
            elm = document.forms[0];
            for (i = 0; i <= elm.length - 1; i++) {
                if (elm[i].type == "checkbox" && elm[i].id != spanChk.id && elm[i].id != "chkMealFlag" && elm[i].id != "chkCopyAtt") {
                    if (elm.elements[i].checked != spanChk.checked)
                        elm.elements[i].click();
                }
            }
        }
        function funHolidayOrNot(msg) {
            alert(msg);
        }
        function funYesOrNoTempSave(msg) {
            if (confirm(msg)) {
                document.getElementById('btnTempSaveInvisible').click();
            }
        }
        function funYesOrNoTempSave_Reminder(msg) {
            alert(msg);
            document.getElementById('btnTempSaveInvisible_Reminder').click();
        }
        function funYesOrNoSubmit(msg) {
            if (confirm(msg)) {
//                document.getElementById('btnSubmit').setAttribute("disabled", "disabled");
//                document.getElementById('btnSubmitInvisible').click();
                document.getElementById('btnSubmitInvisible').click();
            }
        }
        function funYesOrNoSubmit_Reminder(msg) {
//            if (confirm(msg)) {
//                document.getElementById('btnSubmit').setAttribute("disabled", "disabled");
//                document.getElementById('btnSubmitInvisible').click();
//            }
            alert(msg);
            document.getElementById('btnSubmitInvisible_Reminder').click();

        }
        function funYesOrNoDelete() {
            if (confirm('是否要刪除？')) {
                document.getElementById('btnDeleteInvisible').click();
            }
        }

        //上傳附件時，另開子視窗
        function openUploadWindow() {
            var url = document.getElementById("frameAttach").value;
            var popup = window.open(url, '檔案上傳', 'width=650,height=500');
            this.pollForWindowClosure(popup); //檢查子視窗是否被關，被關時觸發事件
            //            popup.onbeforeunload = function () {
            //                window.parent.document.getElementById('updateAttachName').click();
            //            };
        }

        //        //檢查子視窗是否被關，被關時觸發事件
        //        function pollForWindowClosure(popup) {
        //            if (popup.closed) {
        //                document.getElementById('updateAttachName').click();
        //                return;
        //            }
        //            setTimeout(function () { this.pollForWindowClosure(popup) }, 10);
        //        }
        //檢查子視窗是否被關，被關時觸發事件
        function pollForWindowClosure(popup) {
            if (popup.closed) {
                queryFileNameCount = 0;
                queryFileName();
                return;
            }
            setTimeout(function () { this.pollForWindowClosure(popup) }, 10);
        }

        //查詢附件名稱
        var queryFileNameCount = 0; //目前查詢次數
        var queryFileNameMaxCount = 10; //最多查詢次數
        function queryFileName() {
            queryFileNameCount++;
            var filename = $('#lblAttachName').text(); //附件名
            var searchstr = "無附件"; //要搜索的字串
            var start = 0; //開始搜索的位置
            var checkFilename = filename.indexOf(searchstr, start); //搜索是否有休關字串
            if (queryFileNameCount > queryFileNameMaxCount) { //有顯示附件名或查詢次達到最高次數就不再查詢
                return;
            }
            else {
                $('#updateAttachName').click(); //查詢
            }
            setTimeout(function () { this.queryFileName() }, 100); //0.1秒後再次查詢
        }
        
    </script>

<body>
    <form id="form1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="True"></asp:ToolkitScriptManager>
    <uc1:ucLightBox runat="server" ID="ucLightBox" />
    <fieldset class="Util_Fieldset">
    <legend class="Util_Legend">預先申請-新增</legend>
    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
        <tr>
            <td align="left">
                <table width="100%" cellpadding="1" cellspacing="1">
                    <tr style="height:20px">
                    <td colspan="4" align="left">
                        <asp:Button ID="btnTempSave" runat="server" Text="暫存" onclick="btnTempSave_Click" CssClass="Util_clsBtn"/>&nbsp;&nbsp;
                        <asp:Button ID="btnTempSaveInvisible" runat="server" Text="暫存" CssClass="Util_clsBtn" style="display:none;" onclick="btnTempSaveInvisible_Click" />
                        <asp:Button ID="btnTempSaveInvisible_Reminder" runat="server" Text="暫存" CssClass="Util_clsBtn" style="display:none;" onclick="btnTempSaveInvisible_Reminder_Click" />
                        <asp:Button ID="btnSubmit"   runat="server" Text="送簽" onclick="btnSubmit_Click"   CssClass="Util_clsBtn" OnClientClick="this.disabled=true;" UseSubmitBehavior="false"/>&nbsp;&nbsp;
                        <asp:Button ID="btnSubmitInvisible" runat="server" Text="送簽"  CssClass="Util_clsBtn" style="display:none;" onclick="btnSubmitInvisible_Click"/>
                        <asp:Button ID="btnSubmitInvisible_Reminder" runat="server" Text="送簽" CssClass="Util_clsBtn" style="display:none;" onclick="btnSubmitInvisible_Reminder_Click" />
                        <asp:Button ID="btnClear"    runat="server" Text="清除" OnClientClick="return confirm('確定要清除？');"  onclick="btnClear_Click"    CssClass="Util_clsBtn" />&nbsp;&nbsp;
                        <asp:Button ID="btnDelete"   runat="server" Text="刪除" onclick="btnDelete_Click"   CssClass="Util_clsBtn" />&nbsp;&nbsp;
                        <asp:Button ID="btnDeleteInvisible" runat="server" Text="刪除"  CssClass="Util_clsBtn" style="display:none;" onclick="btnDeleteInvisible_Click" />
                        <asp:Button ID="btnExit"     runat="server" Text="返回" OnClientClick="return confirm('確定要返回？')"    onclick="btnExit_Click"    CssClass="Util_clsBtn" />
                    </td>
                    </tr>
                    <tr style="height:20px" class="Util_clsRow1">
                        <td width="10%" align="left">
                        <asp:Label ID="lblOTComp" runat="server" Text="*公司：" ForeColor="Blue"></asp:Label>
                        </td>
                        <td style="width:35%" align="left">  
                            <asp:Label ID="lblCompID" runat="server" Visible="false"></asp:Label>                              
                            <asp:Label ID="txtOTCompName" runat="server"></asp:Label>
                        </td>
                        <td width="10%" align="left">
                            <asp:Label ID="lblDept" runat="server" Text="*單位：" ForeColor="Blue"></asp:Label>
                        </td>
                        <td style="width:35%" align="left"> 
                            <asp:Label ID="lblDeptID" runat="server" Visible="false"></asp:Label>                               
                            <asp:Label ID="txtDeptName" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr style="height:20px" class="Util_clsRow2">
                        <td width="10%" align="left">
                            <asp:Label ID="lblOrgan" runat="server" Text="*科別：" ForeColor="Blue"></asp:Label>
                        </td>
                        <td style="width:35%" align="left">     
                            <asp:Label ID="lblOrganID" runat="server" Visible="false"></asp:Label>                           
                            <asp:Label ID="txtOrganName" runat="server"></asp:Label>

                        </td>
                        <td width="10%" align="left">
                            <asp:Label ID="lblEmp" runat="server" Text="*加班人：" ForeColor="Blue"></asp:Label>
                        </td>
                        <td style="width:35%" align="left">                                
                            <asp:RadioButton ID="rbtSingleEmpID" GroupName="OTEmpID" runat="server" Text="單筆加班人" Checked="true" oncheckedchanged="rbtSingleEmpID_CheckedChanged"  AutoPostBack="true"/>
                            <asp:RadioButton ID="rbtMultiEmpID" GroupName="OTEmpID" runat="server"  Text="多筆加班人" oncheckedchanged="rbtMultiEmpID_CheckedChanged" AutoPostBack="true"/>
                            <asp:ImageButton ID="imgLaunch" runat="server" BorderWidth="0" BorderStyle="None" ImageUrl="~/Util/WebClient/Icon_Group.png" ImageAlign="AbsBottom" onclick="imgLaunch_Click" style="height: 16px; width: 16px" />
                            <asp:TextBox ID="txtOTEmpID" runat="server" Width="50px" ontextchanged="txtOTEmpID_TextChanged" AutoPostBack="true" AutoComplete="off" MaxLength="6"></asp:TextBox>&nbsp;
                            <asp:TextBox ID="txtOTEmpName" runat="server" AutoComplete="off" Width="70px"  ></asp:TextBox>&nbsp;&nbsp;
                            <asp:ImageButton ID="imgQuestion" runat="server" BorderWidth="0" BorderStyle="None" ImageUrl="~/Util/WebClient/Icon_FAQ.png" ImageAlign="AbsBottom" onclick="imgQuestion_Click" style="height: 16px; width: 16px" />
                        </td>
                    </tr>
                    <tr style="height:20px" class="Util_clsRow1">
                        <td width="10%" align="left">
                            <asp:Label ID="lblOTRegister" runat="server" Text="*填單人：" ForeColor="Blue"></asp:Label>
                        </td>
                        <td style="width:35%" align="left">                                
                            <asp:Label ID="txtOTRegisterName" runat="server"></asp:Label>
                        </td>
                        <td width="10%" align="left">
                            <asp:Label ID="lblSalaryOrAdjust" runat="server" Text="*加班轉換方式：" ForeColor="blue"></asp:Label>
                        </td>
                        <td width="45%" align="left">
                            <asp:DropDownList ID="ddlSalaryOrAdjust" runat="server" AutoPostBack="true" onselectedindexchanged="ddlSalaryOrAdjust_SelectedIndexChanged" >
                            <asp:ListItem Value="0" Text="請選擇"></asp:ListItem>
                            <asp:ListItem Value="1" Text="轉薪資"></asp:ListItem>
                            <asp:ListItem Value="2" Text="轉補休"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:Label ID="lblAdjustInvalidDate" runat="server" Text="補休失效日：" Visible="false"></asp:Label>
                            <asp:Label ID="txtAdjustInvalidDate" runat="server" Visible="false" Text=""></asp:Label>
                        </td>
                        </tr>
                    <tr style="height:20px" class="Util_clsRow2">
                        <td width="10%" align="left">
                            <asp:Label ID="lblOvertimeDate" runat="server" Text="*加班日期：" ForeColor="Blue"></asp:Label>
                        </td>
                        <td style="width:35%" align="left" colspan="3">                                
                            <asp:ucDate ID="ucDateStart" runat="server" CssClass="InputTextStyle_Thin" Enabled="True" Width="80px"/>
                            <asp:Label ID="Label4" runat="server" Text="~"></asp:Label>
                            <asp:ucDate ID="ucDateEnd" runat="server" CssClass="InputTextStyle_Thin" Enabled="True" Width="80px" />
                        </td>
                    </tr>
                    <tr style="height:20px" class="Util_clsRow1">
                        <td width="10%" align="left">
                            <asp:Label ID="lblStartTime" runat="server" Text="*加班開始時間：" ForeColor="Blue"></asp:Label>
                        </td>
                        <td style="width:35%" align="left">                                
                            <asp:ucTimePickerNew ID="OTTimeStart" runat="server" CssClass="InputTextStyle_Thin" Enabled="True" Width="80px" AutoPostBack="true" />
                            <asp:Label ID="lblStartSex" runat="server" Text="女性不可以10點後加班" ForeColor="Red" Visible="false" ></asp:Label>
                        </td>
                        <td width="10%" align="left">
                            <asp:Label ID="lblEndTime" runat="server" Text="*加班結束時間：" ForeColor="Blue"></asp:Label>
                        </td>
                        <td width="45%" align="left">
                            <asp:ucTimePickerNew ID="OTTimeEnd" runat="server" CssClass="InputTextStyle_Thin" Enabled="True" Width="80px" />
                            <asp:Label ID="lblEndSex" runat="server" Text="女性不可以10點後加班" ForeColor="Red" Visible="false"></asp:Label>
                        </td>
                    </tr>
                    <tr style="height:20px" class="Util_clsRow2">
                        <td style="width:35%" align="left" colspan="2">   
                            <asp:Label ID="lblOTTotalTime" runat="server" Text="*加班時數：" ForeColor="Blue"></asp:Label>
                            <asp:Label ID="txtOTTotalTime" runat="server"></asp:Label>
                            <asp:Label ID="txtOTTotalTimeHour" runat="server" Text="小時"></asp:Label>
                            <asp:Label ID="txtTotalDescription" runat="server" Text="" ForeColor="Red"></asp:Label>                             
                            <br />
                            <asp:CheckBox ID="chkMealFlag" runat="server" Checked="true" AutoPostBack="true" oncheckedchanged="chkMealFlag_CheckedChanged" />
                            <asp:Label ID="lblMinusMealTime" runat="server" Text="扣除用餐時數：" ForeColor="Red"></asp:Label>&nbsp;&nbsp;
                            <asp:FilteredTextBoxExtender ID="fttxtMealTime" runat="server" TargetControlID="txtMealTime" FilterType="Numbers"></asp:FilteredTextBoxExtender>
                            <asp:TextBox ID="txtMealTime" runat="server" AutoPostBack="true" style="text-align:right;" width="50px" ontextchanged="txtMealTime_TextChanged" AutoComplete="off" MaxLength="3" ForeColor="Red"></asp:TextBox>
                            <asp:Label ID="lblMinute" runat="server" Text="分鐘" ForeColor="Red"></asp:Label>
                        </td>
                        <td width="45%" align="left" colspan="2">          
                            <asp:Label ID="lblPeriod" runat="server" Text="含本次累計時數如下：" Visible="false"></asp:Label><br />
                            <table width="40%" align="left" id="tbTime" visible="false" style="border: 1px solid black;" runat="server" rules="all" >
                            <tr align="center">
                                <td width="25%" rowspan="2">日期</td>
                                <td width="25%" colspan="3">小時</td>
                            </tr>
                            <tr align="center">
                                <td width="25%">時段(一)</td>
                                <td width="25%">時段(二)</td>
                                <td width="25%">時段(三)</td>
                            </tr> 
                            <tr id="trOne" visible="false" runat="server" align="center">
                                <td><asp:Label ID="lblDateOne" runat="server" ></asp:Label></td>
                                <td><asp:Label ID="lblDateOne_0" runat="server" ></asp:Label></td>
                                <td><asp:Label ID="lblDateOne_1" runat="server" ></asp:Label></td>
                                <td><asp:Label ID="lblDateOne_2" runat="server" ></asp:Label></td>
                            </tr>
                            <tr id="trTwo" visible="false" runat="server" align="center">
                                <td><asp:Label ID="lblDateTwo" runat="server" ></asp:Label></td>
                                <td><asp:Label ID="lblDateTwo_0" runat="server" ></asp:Label></td>
                                <td><asp:Label ID="lblDateTwo_1" runat="server" ></asp:Label></td>
                                <td><asp:Label ID="lblDateTwo_2" runat="server" ></asp:Label></td>
                            </tr>
                            </table>
                        </td>
                    </tr>
                    <tr style="height:20px" class="Util_clsRow1">
                        <td width="10%" align="left">
                            <asp:Label ID="lblOTTypeID" runat="server" Text="*加班類型：" ForeColor="Blue"></asp:Label>
                        </td>
                        <td style="width:35%" align="left" colspan="3">                              
                            <asp:DropDownList ID="ddlOTTypeID" runat="server"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr style="height:20px" class="Util_clsRow2">
                        <td width="10%" align="left">
                            <asp:Label ID="lblOTReasonID" runat="server" Text="*加班原因：" ForeColor="Blue"></asp:Label>
                        </td>
                        <td width="45%" align="left" colspan="3">
                            <uc1:ucTextBox runat="server" ID="txtOTReasonMemo" ucRows="2" ucMaxLength="200" ucIsDispEnteredWords="true" ucWidth="400" />
                            <asp:Label ID="lblCount_Remark" runat="server" ForeColor="Red" Font-Size="12px"></asp:Label>
                        </td>
                    </tr>
                    <tr style="height:20px" class="Util_clsRow1">
                        <td>
                            <asp:Label ID="lblFileUpload" runat="server" Text="上傳附件：" ></asp:Label>
                        </td>
                        <td width="45%" align="left" colspan="4">
                            <asp:Button ID="btnUploadAttach" runat="server" CausesValidation="false" 
                                CssClass="Util_clsBtn" OnClientClick="openUploadWindow();return false;" Text="附件上傳" />
                                <asp:Label ID="lblAttachName" runat="server" Text="(目前無附件)"></asp:Label>
                                <asp:Button ID="updateAttachName" runat="server" CausesValidation="false" 
                                CssClass="Util_clsBtn" OnClick="btnUpdateAttachName_Click" Text="更新附件檔名" style="display:none;" />
                                <asp:CheckBox ID="chkCopyAtt" runat="server" Text="同上筆附檔" AutoPostBack="true" oncheckedchanged="chkCopyAtt_CheckedChanged" Visible="false"/>
                        </td>
                    </tr>
                    <tr style="height:20px" class="Util_clsRow2">
                        <td width="45%" align="left" colspan="4">
                            <asp:Label ID="txtOTDateMonth" runat="server"></asp:Label>
                            <asp:Label ID="Label3" runat="server" Text="月已申請時數合計：  送簽"></asp:Label>
                            <asp:Label ID="lblSubmit" runat="server" Text="0.0"></asp:Label>
                            <asp:Label ID="Label5" runat="server" Text="小時  核准"></asp:Label>
                            <asp:Label ID="lblApproval" runat="server" Text="0.0"></asp:Label>
                            <asp:Label ID="Label8" runat="server" Text="小時  駁回"></asp:Label>
                            <asp:Label ID="lblReject" runat="server" Text="0.0"></asp:Label>
                            <asp:Label ID="Label10" runat="server" Text="小時"></asp:Label>
                        </td>
                    </tr>
                </table>
                </table>
                </fieldset>
                <div style ="height:75%; width:100%; overflow:auto; ">
                    <asp:GridView ID="gvMain" runat="server" AllowPaging="False" AllowSorting="true" AutoGenerateColumns="False" CssClass="GridViewStyle" 
                        CellPadding="2" DataKeyNames="OTCompID,OTFormNO,OTEmpID,OTDate,OTTime,OTTotalTime,OTTxnID" Width="100%" PageSize="15" HeaderStyle-ForeColor="dimgray" Visible="false" onrowdatabound="gvMain_RowDataBound">
                        <FooterStyle BackColor="#99CCCC" ForeColor="#003399" />
                        <Columns>
                            <asp:TemplateField HeaderText="全選">
                                <HeaderTemplate> 
                                    <asp:CheckBox ID="CheckAll" runat="server" onclick="javascript: SelectAllCheckboxes(gvMain_ctl01_CheckAll);" Text="全選" ToolTip="按一次全選，再按一次取消全選" /> 
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkChoose" runat="server" Enabled="true" />
                                </ItemTemplate>
                                <HeaderStyle Width="30px" Height="15px" CssClass="td_header" />
                                <ItemStyle CssClass="td_detail" Height="15px" HorizontalAlign="Center"/>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="序號">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex+1 %>
                                </ItemTemplate>
                                 <HeaderStyle Width="4%" Height="15px" CssClass="td_header" />
                                 <ItemStyle CssClass="td_detail" Height="15px" HorizontalAlign="Center"/>
                            </asp:TemplateField> 
                             <asp:BoundField HeaderText="表單編號" ItemStyle-Width="1px" ItemStyle-Wrap="true" SortExpression="OTFormNO" DataField="OTFormNO" HtmlEncode="false">
                                <HeaderStyle Width="80px" Height="15px"/>
                                <ItemStyle HorizontalAlign="Center" Width="25px"/>
                            </asp:BoundField>
                            <asp:BoundField DataField="OTEmpID" HeaderText="加班人員編">
                                <HeaderStyle Width="" CssClass="td_header" />
                                <ItemStyle CssClass="td_detail" HorizontalAlign="Center" />
                            </asp:BoundField>  
                            <asp:BoundField DataField="NameN" HeaderText="加班人">
                                <HeaderStyle Width="" CssClass="td_header" />
                                <ItemStyle CssClass="td_detail" HorizontalAlign="Center" />
                            </asp:BoundField>      
                            <asp:BoundField DataField="CodeCName" HeaderText="加班類型">
                                <HeaderStyle Width="" CssClass="td_header" />
                                <ItemStyle CssClass="td_detail" HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="OTDate" HeaderText="加班日期">
                                <HeaderStyle Width="" CssClass="td_header" />
                                <ItemStyle CssClass="td_detail" HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="OTTime" HeaderText="加班起迄時間">
                                <HeaderStyle Width="" CssClass="td_header" />
                                <ItemStyle CssClass="td_detail" HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="OTTotalTime" HeaderText="加班時數">
                                <HeaderStyle Width="" CssClass="td_header" />
                                <ItemStyle CssClass="td_detail" HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="上傳附件" DataField="FileName">
                                <HeaderStyle Width="" CssClass="td_header" />
                                <ItemStyle CssClass="td_detail" HorizontalAlign="Center" />
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
                    </asp:GridView>
                 </div>
                <!--人員選取-->
            <div id="DivHiddenArea" style="display: none; height: auto; overflow: scroll;">
            <asp:Panel ID="pnlVerify" runat="server">

            <asp:DropDownList ID="ddlOrg" runat="server" AutoPostBack="true" onselectedindexchanged="ddlOrg_SelectedIndexChanged" Width="195px" >
            <asp:ListItem Value="0" Text="請選擇"></asp:ListItem>
            <asp:ListItem Value="1" Text="行政組織"></asp:ListItem>
            <asp:ListItem Value="2" Text="功能組織"></asp:ListItem>
            </asp:DropDownList>
            <br />
            <uc1:ucCascadingDropDown ID="ucCascadingDropDown1" runat="server" ucServiceMethod="Util_GetUser" />
            <uc1:ucCascadingDropDown ID="ucCascadingDropDown2" runat="server" ucServiceMethod="Util_GetUser" />
            <br />
            <hr class="Util_clsHR" />
            <center>
              <asp:Button ID="btnOK" runat="server" Text="OK" CssClass="Util_clsBtn" OnClick="btnOK_Click" />
              <asp:Button ID="btnNo" runat="server" CausesValidation="False" Text="Cancel" CssClass="Util_clsBtn" OnClick="btnNo_Click" />
            </center>
            </asp:Panel>
     </div>
      <!--送審小視窗-->
    <%-- <div style="display: none;">
        <!--固定隱藏區-->
        <asp:Panel ID="pnlFlow" runat="server">
            <br />
            <asp:Label ID="labVerifyCaseInfo" runat="server" Text="" />
            <br />
            <asp:Label ID="labOTEmpID" runat="server" Text="" />
            <br />
            <asp:Label ID="labVerifyAssignTo" runat="server" Text="" /><asp:DropDownList ID="ddlAssignTo"
                runat="server" />
            <br />
            <hr class="Util_clsHR" />
            <center>
                <asp:Button ID="btnVerify" runat="server" Text="OK" CssClass="Util_clsBtn" OnClick="btnVerify_Click" />
            </center>
        </asp:Panel>
    </div>--%>
    <!--附件上傳-->
    <asp:HiddenField id="frameAttach" runat="server" />
    <%--<div id="Div1" style="display: none; height: auto; overflow: scroll">
            <asp:Panel runat="server" ID="pnlAttach" CssClass="Util_Frame" ScrollBars="Auto"
                Width="100%" Height="100%">
                <div style="height: auto; overflow: auto;">
                    <iframe id="frameAttach" runat="server" frameborder="0" width="100%" height="600"
                        clientidmode="Inherit"></iframe>
                </div>
            </asp:Panel>
    </div>--%>
        <uc1:ucModalPopup ID="ucModalPopup1" runat="server" />

    </form>
</body>
</html>
