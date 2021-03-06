'****************************************************
'功能說明：WF_FlowToDoList(待辦事項處理)
'建立人員：Chung
'建立日期：2013/01/28
'****************************************************
Imports System.Data
Imports Newtonsoft.Json

Partial Class WF_WFA000
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                '取得資料
                'Bsp.Utility.RunClientScript(Me, "WFStepCloseMenu();")
                Try
                    pcMain.DataTable = WF.GetToDoList(UserProfile.UserID)
                    pcTraceList.DataTable = WF.GetTraceList(UserProfile.UserID)
                Catch ex As Exception
                    Bsp.Utility.ShowMessage(Me, Me.FunID & ".Page_Load", ex)
                    Return
                End Try
                'setCustomer()
            End If
            CreateToDoCountButton()
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & ".PageLoad", ex)
        End Try
    End Sub

    'Private Sub setCustomer()
    '    ucQueryCustomer.SelectStyle = Bsp.Enums.SelectStyle.RadioButton
    '    ucQueryCustomer.WindowWidth = "650"
    '    ucQueryCustomer.WindowHeight = "500"

    '    ucQueryCustomer.Fields = New FieldState() { _
    '        New FieldState("CustomerID", "客戶ID", True, True, Unit.Parse("80px")), _
    '        New FieldState("CName", "客戶名稱", True, True, Unit.Parse("220px")), _
    '        New FieldState("AO_CODE", "", False, True), _
    '        New FieldState("AOName", "AO", True, False, Unit.Parse("80px")), _
    '        New FieldState("CustTypeDesc", "客户類別", True, True, Unit.Parse("100px")), _
    '        New FieldState("CustType", "", False, True), _
    '        New FieldState("T24ID", "T24 ID", True, True, Unit.Parse("80px"))}

    '    Dim objAP As New AP()

    '    ucQueryCustomer.QuerySQL = objAP.GetCustomerSQL("105751", "")
    'End Sub

    Public Overrides Sub DoModalReturn(ByVal returnValue As String)

        pcMain.DataTable = WF.GetToDoList(UserProfile.UserID)

        'pcMain.DataTable = WF.GetToDoList(UserProfile.UserID)
        'If returnValue <> "" AndAlso returnValue <> "undefined" Then
        '    Dim intPos As Integer = returnValue.IndexOf(":")
        '    Dim strValue As String = returnValue.Substring(intPos + 1)
        '    Dim strResult As String = ""

        '    'CheckBox 測試
        '    'Dim ltAA As List(Of Dictionary(Of String, String)) = JsonConvert.DeserializeObject(Of List(Of Dictionary(Of String, String)))(strValue)

        '    'For Each a As Dictionary(Of String, String) In ltAA
        '    '    For Each s As String In a.Keys
        '    '        strResult &= String.Format("{0}={1}{2}", s, a(s), vbCrLf)
        '    '    Next
        '    'Next

        '    'RadioButton測試
        '    Dim dicData As Dictionary(Of String, String) = JsonConvert.DeserializeObject(Of Dictionary(Of String, String))(strValue)
        '    For Each s As String In dicData.Keys
        '        strResult &= String.Format("{0}={1}{2}", s, dicData(s), vbCrLf)
        '    Next

        '    Bsp.Utility.ShowMessage(Me, strResult)
        'End If

    End Sub

    Private Sub CreateToDoCountButton()
        Using dt As DataTable = WF.GetToDoListCount(UserProfile.UserID)
            For Each dr As DataRow In dt.Rows
                Dim strButtonID As String = String.Format("btn{0}", dr.Item("Code").ToString())
                Dim strButtonText As String = String.Format("{0}({1})", dr.Item("Define").ToString(), dr.Item("Cnt").ToString())

                Dim ctl As Control = phToDoCount.FindControl(strButtonID)
                If ctl IsNot Nothing Then
                    CType(ctl, LinkButton).Text = strButtonText
                Else
                    Dim NewButton As New LinkButton()
                    Dim lblSpace As New Label()

                    lblSpace.Text = " "
                    NewButton.ID = strButtonID
                    NewButton.Text = strButtonText
                    NewButton.Style.Item("font-size") = "12px"
                    NewButton.Attributes.Add("onmouseover", "this.style.color='red';")
                    NewButton.Attributes.Add("onmouseout", "this.style.color='blue';")
                    AddHandler NewButton.Click, AddressOf ToDoCountButtonClick

                    phToDoCount.Controls.Add(lblSpace)
                    phToDoCount.Controls.Add(NewButton)
                End If
            Next
        End Using
    End Sub

    Protected Sub ToDoCountButtonClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDispatchCnt.Click
        Dim CondStr As String
        Dim strButtonID As String = CType(sender, LinkButton).ID

        If strButtonID = "btnDispatchCnt" Then
            CondStr = "And FlowDispatchFlag = 'Y'"
        Else
            CondStr = "And FlowID = " & Bsp.Utility.Quote(strButtonID.Replace("btn", ""))
        End If
        Try
            pcMain.DataTable = WF.GetToDoList(UserProfile.UserID, CondStr)
        Catch ex As Exception
            Throw
        End Try
    End Sub


    Public Overrides Sub DoAction(ByVal Param As String)
        Select Case Param
            Case "btnAdd"       '新增
                DoAdd()
            Case "btnUpdate"    '修改
                DoUpdate()
            Case "btnQuery"     '查詢
                DoQuery()
            Case "btnDelete"    '刪除
                DoDelete()
            Case Else
                DoOtherAction(Param)   '其他功能動作
        End Select
    End Sub

    Private Sub DoAdd()

    End Sub

    Private Sub DoUpdate()

    End Sub

    Private Sub DoQuery()
        Try
            pcMain.DataTable = WF.GetToDoList(UserProfile.UserID)
            pcTraceList.DataTable = WF.GetTraceList(UserProfile.UserID)
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & ".Page_Load", ex)
            Return
        End Try
    End Sub

    Private Sub DoDelete()

    End Sub

    Private Sub DoOtherAction(ByVal Param As String)
        Select Case Param
            Case "btnActionC"
                If selectedRow(gvMain) < 0 Then
                    '尚未選取資料列！
                    Bsp.Utility.ShowFormatMessage(Me, "W_00000")
                Else
                    Dim intRow As Integer = selectedRow(gvMain)
                    Dim strFlowType As String = gvMain.DataKeys(intRow)("FlowType").ToString()
                    Dim strFlowKeyStr As String = gvMain.DataKeys(intRow)("FlowKeyStr").ToString()
                    Dim strFlowStepID As String = gvMain.DataKeys(intRow)("FlowStepID").ToString()
                    Dim FlowKeyArr() As String = strFlowKeyStr.Split(";")
                    Try
                        Select Case strFlowType
                            Case "F"
                                Me.TransferFramePage("~/WF/WFA011.aspx", Nothing, "FlowID=" & FlowKeyArr(0), "FlowCaseID=" & FlowKeyArr(1), "FlowLogID=" & FlowKeyArr(2))
                            Case "M"
                                RunToDoMessage(strFlowKeyStr)
                        End Select
                    Catch ex As Exception
                        Bsp.Utility.ShowMessage(Me, Me.FunID & ".DoOtherAction", ex)
                    End Try
                End If
        End Select
    End Sub

    Protected Sub gvMain_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvMain.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            If e.Row.DataItem("MsgKind") = "M" Then
                Dim objDel As LinkButton = e.Row.FindControl("btnDelete")

                If objDel IsNot Nothing Then
                    objDel.Enabled = True
                End If
            End If
        End If
    End Sub

    Protected Sub gvMain_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gvMain.RowDeleting
        Try
            Dim objWF As New WF()
            objWF.UpdateToDoMessage(gvMain.DataKeys(e.RowIndex)("FlowKeyStr").ToString(), "D")
            pcMain.DataTable = WF.GetToDoList(UserProfile.UserID)
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & ".gvMain_RowDeleting", ex)
        End Try
    End Sub

    Protected Sub gvMain_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvMain.SelectedIndexChanged
        DoOtherAction("btnActionC")
    End Sub

    Private Sub RunToDoMessage(ByVal MsgCaseID As String)
        Using dt As DataTable = WF.GetToDoMessage(MsgCaseID)
            If dt.Rows.Count = 0 Then Throw New Exception("[WFA000.RunToDoMessage]：查無該筆待辦事項!(MsgID=" & MsgCaseID & ")")

            Dim beToDoMessage As New beWF_ToDoMessage.Row(dt.Rows(0))
            Dim beMsgDefine As beWF_MsgDefine.Row = Nothing

            Using dtMsgDefine As DataTable = WF.GetMsgDefine(beToDoMessage.MsgCode.Value)
                If dtMsgDefine.Rows.Count = 0 Then Throw New Exception("[WFA000.RunToDoMessage]：無此待辦事項定義!(MsgCode=" & MsgCaseID & ")")
                beMsgDefine = New beWF_MsgDefine.Row(dtMsgDefine.Rows(0))
            End Using

            Dim objWF As New WF()

            '判斷是否設定為開啟刪除待辦
            If beMsgDefine.DelKind.Value = "Y" Then
                objWF.UpdateToDoMessage(beToDoMessage.MsgCaseID.Value, "D")
            Else
                objWF.UpdateToDoMessage(beToDoMessage.MsgCaseID.Value, "Y")
            End If

            Dim aryParam() As String = beToDoMessage.KeyValue.Value.Split("&")
            '若只有一個參數則帶上KeyID過去
            If aryParam.Length = 1 Then aryParam(0) = "KeyID=" & aryParam(0)
            ReDim Preserve aryParam(aryParam.Length)
            aryParam(aryParam.GetUpperBound(0)) = "MsgCaseID=" & MsgCaseID

            '開啟視窗方式判斷
            If beMsgDefine.OpenFlag.Value = "R" Then
                TransferFramePage(beMsgDefine.MsgUrl.Value, Nothing, aryParam)
            Else
                Select Case beMsgDefine.OpenWSize.Value
                    Case "S"
                        CallSmallPage(beMsgDefine.MsgUrl.Value, False, Nothing, aryParam)
                    Case "M"
                        CallMiddlePage(beMsgDefine.MsgUrl.Value, False, Nothing, aryParam)
                    Case "L"
                        CallLargePage(beMsgDefine.MsgUrl.Value, False, Nothing, aryParam)
                    Case Else
                        If beMsgDefine.OpenWSize.Value.IndexOf("*") > 0 Then
                            Dim arySize() As String = beMsgDefine.OpenWSize.Value.Split("*")
                            CallModalPage(beMsgDefine.MsgUrl.Value, arySize(0), arySize(1), True, Nothing, aryParam)
                        Else
                            CallMiddlePage(beMsgDefine.MsgUrl.Value, Nothing, aryParam)
                        End If
                End Select
            End If
        End Using
    End Sub

    Protected Sub TraceGridView_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles TraceGridView.RowDeleting
        Try
            Dim objWF As New WF()
            Dim strFlowID As String = TraceGridView.DataKeys(e.RowIndex)("FlowID").ToString()
            Dim strFlowCaseID As String = TraceGridView.DataKeys(e.RowIndex)("FlowCaseID").ToString()
            objWF.DeleteTraceList(strFlowID, strFlowCaseID, UserProfile.UserID)
            pcTraceList.DataTable = WF.GetTraceList(UserProfile.UserID)
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & ".TraceGridView_RowDeleting", ex)
        End Try
    End Sub

    Protected Sub TraceGridView_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles TraceGridView.RowEditing
        Try
            Dim strFlowID As String = TraceGridView.DataKeys(e.NewEditIndex)("FlowID").ToString()
            Dim strFlowCaseID As String = TraceGridView.DataKeys(e.NewEditIndex)("FlowCaseID").ToString()
            Dim strFlowLogID As String = TraceGridView.DataKeys(e.NewEditIndex)("FlowLogID").ToString()

            Me.TransferFramePage(ResolveUrl("~/WF/WFA011.aspx"), Nothing, "FlowID=" & strFlowID, "FlowCaseID=" & strFlowCaseID, "FlowLogID=" & strFlowLogID, "ShowMode=Y")
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & ".TraceGridView_RowEditing", ex)
        End Try
    End Sub

    'Protected Sub btnTest_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnTest.Click
    '    Dim a As New FlowBackInfo()

    '    a.MenuNodeTitle = "回客戶清單"
    '    a.URL = "~/BR/BR0000.aspx"

    '    TransferFramePage(Bsp.MySettings.FlowRedirectPage, Nothing, "FlowID=CUSDATA", a, "CID=CI0201054004")
    '    TransferFramePage("~/AL/AL0000.aspx", Nothing, "AppID=")
    'End Sub

    'Protected Sub btnAL0000_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAL0000.Click
    '    Me.TransferFramePage(ResolveUrl("~/AL/AL0000.aspx"), New ButtonState() {}, "AppID=AP1304120003")

    'End Sub

    'Protected Sub btnAL0100_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAL0100.Click
    '    Me.TransferFramePage(ResolveUrl("~/AL/AL0100.aspx"), New ButtonState() {}, "AppID=AP1304120003")
    'End Sub

    'Protected Sub btnAL0500_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAL0500.Click
    '    Me.TransferFramePage(ResolveUrl("~/AL/AL0500.aspx?AppID=AP1304120003"), New ButtonState() {})
    'End Sub

    'Protected Sub btnCC0700_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCC0070.Click
    '    Me.TransferFramePage(ResolveUrl("~/CC/CC0070.aspx?AppID=AP1306260001"), New ButtonState() {})
    'End Sub

    'Protected Sub btnAL0730_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAL0730.Click
    '    Me.TransferFramePage(ResolveUrl("~/AL/AL0730.aspx?AppID=AP1306260001"), New ButtonState() {})
    'End Sub

    'Protected Sub btnAL0400_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAL0400.Click
    '    Me.TransferFramePage(ResolveUrl("~/AL/AL0400.aspx?AppID=DI0907070015&SourceAppID=AP0906100021&PrintUserID=" & UserProfile.UserID), New ButtonState() {})
    'End Sub

    'Protected Sub btnCC0030_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCC0030.Click
    '    Me.TransferFramePage(ResolveUrl("~/CC/CC0030.aspx?AppID=AP1306260001&CustomerID=CP00000013&CCID=CC1306260001"), New ButtonState() {})
    'End Sub

End Class
