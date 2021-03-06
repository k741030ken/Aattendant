'****************************************************
'功能說明：整體評量調整說明畫面
'建立人員：BeatriceCheng
'建立日期：2015.11.03
'****************************************************
Imports System.Data

Partial Class GS_GS1150
    Inherits PageBase


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then

        End If
    End Sub

    Protected Overrides Sub BaseOnPageTransfer(ByVal ti As TransferInfo)
        If Not IsPostBack() Then
            Dim ht As Hashtable = Bsp.Utility.getHashTableFromParam(ti.Args)

            ViewState.Item("CompID") = ht("CompID").ToString()
            ViewState.Item("ApplyID") = ht("ApplyID").ToString()
            ViewState.Item("ApplyTime") = ht("ApplyTime").ToString()
            ViewState.Item("Seq") = ht("Seq").ToString()
            ViewState.Item("MainFlag") = ht("MainFlag").ToString()
            ViewState.Item("GradeYear") = ht("GradeYear").ToString()
            ViewState.Item("GradeSeq") = ht("GradeSeq").ToString()
            ViewState.Item("EvaluationSeq") = ht("EvaluationSeq").ToString()
            ViewState.Item("DeptEX") = ht("DeptEX").ToString()
            ViewState.Item("IsSignNext") = ht("IsSignNext").ToString()
            ViewState.Item("Result") = ht("Result").ToString()

            If ht.ContainsKey("CompID") Then
                subGetData(
                    ViewState.Item("CompID"), _
                    ViewState.Item("ApplyID"), _
                    ViewState.Item("ApplyTime"), _
                    ViewState.Item("Seq"), _
                    ViewState.Item("MainFlag"), _
                    ViewState.Item("GradeYear"), _
                    ViewState.Item("GradeSeq"), _
                    ViewState.Item("EvaluationSeq"), _
                    ViewState.Item("DeptEX"))
            Else
                Return
            End If
        End If
    End Sub

    Public Overrides Sub DoAction(ByVal Param As String)
        Select Case Param
            Case "btnActionX"   '返回
                GoBack()
        End Select
    End Sub

    Private Sub GoBack()
        Dim ti As TransferInfo = Me.StateTransfer
        If ViewState.Item("MainFlag") = "1" And ViewState.Item("IsSignNext") = "Y" Then
            Dim btnD As New ButtonState(ButtonState.emButtonType.Download)
            Dim btnU As New ButtonState(ButtonState.emButtonType.Upload)
            Dim btnS As New ButtonState(ButtonState.emButtonType.Update)
            Dim btnE As New ButtonState(ButtonState.emButtonType.Executes)
            Dim btnO As New ButtonState(ButtonState.emButtonType.OK)
            Dim btnC As New ButtonState(ButtonState.emButtonType.Confirm)
            btnD.Caption = "結果檔下傳"
            btnU.Caption = "上傳(專用格式)"
            btnS.Caption = "考績暫存"
            btnE.Caption = "排序作業"
            btnO.Caption = "考績補充說明"
            btnC.Caption = "呈上階主管審核"
            If ViewState.Item("Result") = "1" Then
                btnU.Visible = False
                btnS.Visible = False
                btnE.Visible = False
                btnC.Visible = False
            End If
            Me.TransferFramePage(ti.CallerUrl, New ButtonState() {btnD, btnU, btnS, btnE, btnO, btnC}, ti.Args)
        End If
        If ViewState.Item("MainFlag") = "1" And ViewState.Item("IsSignNext") = "N" Then
            Dim btnD As New ButtonState(ButtonState.emButtonType.Download)
            Dim btnU As New ButtonState(ButtonState.emButtonType.Upload)
            Dim btnS As New ButtonState(ButtonState.emButtonType.Update)
            Dim btnE As New ButtonState(ButtonState.emButtonType.Executes)
            Dim btnO As New ButtonState(ButtonState.emButtonType.OK)
            Dim btnC As New ButtonState(ButtonState.emButtonType.Confirm)
            Dim btnC1 As New ButtonState(ButtonState.emButtonType.Copy)
            btnD.Caption = "結果檔下傳"
            btnU.Caption = "上傳(專用格式)"
            btnS.Caption = "考績暫存"
            btnE.Caption = "排序作業"
            btnO.Caption = "考績補充說明"
            btnC.Caption = "送出"
            btnC1.Caption = "統計表"
            If ViewState.Item("Result") = "1" Then
                btnU.Visible = False
                btnS.Visible = False
                btnE.Visible = False
                btnC.Visible = False
            End If
            Me.TransferFramePage(ti.CallerUrl, New ButtonState() {btnD, btnU, btnS, btnE, btnO, btnC, btnC1}, ti.Args)
        End If
        If ViewState.Item("MainFlag") = "2" Then
            Dim btnD As New ButtonState(ButtonState.emButtonType.Download)
            Dim btnU As New ButtonState(ButtonState.emButtonType.Upload)
            Dim btnS As New ButtonState(ButtonState.emButtonType.Update)
            Dim btnE As New ButtonState(ButtonState.emButtonType.Executes)
            Dim btnO As New ButtonState(ButtonState.emButtonType.OK)
            Dim btnC As New ButtonState(ButtonState.emButtonType.Confirm)
            Dim btnC1 As New ButtonState(ButtonState.emButtonType.Copy)
            btnD.Caption = "結果檔下傳"
            btnU.Caption = "上傳(專用格式)"
            btnS.Caption = "考績暫存"
            btnE.Caption = "排序作業"
            btnO.Caption = "考績補充說明"
            btnC.Caption = "送出"
            btnC1.Caption = "統計表"

            If ViewState.Item("Result") = "1" Then
                btnU.Visible = False
                btnS.Visible = False
                btnE.Visible = False
                btnC.Visible = False
            End If
            Me.TransferFramePage(ti.CallerUrl, New ButtonState() {btnD, btnU, btnS, btnE, btnO, btnC, btnC1}, ti.Args)
        End If

    End Sub

    Private Sub subGetData(ByVal CompID As String, ByVal ApplyID As String, ByVal ApplyTime As String, ByVal Seq As String, ByVal MainFlag As String, ByVal GradeYear As String, ByVal GradeSeq As String, ByVal EvaluationSeq As String, ByVal DeptEx As String)
        Dim objGS1 As New GS1()
        Dim objHR As New HR()

        Try
            Using dt As DataTable = objHR.GetHROrganName(CompID, ApplyID)
                If dt.Rows.Count > 0 Then
                    txtDeptID.Text = "部門：" & dt.Rows(0)(0).ToString()
                End If
            End Using

            Using dt As DataTable = objGS1.GS1150Query(CompID, ApplyID, ApplyTime, Seq, GradeYear, GradeSeq, EvaluationSeq, MainFlag, DeptEx)
                gvMain.DataSource = dt
                gvMain.DataBind()

            End Using
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & ".DoQuery", ex)
        End Try
    End Sub
    Protected Sub gvMain_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvMain.RowCommand
        Dim ibtnEdit As ImageButton = CType(gvMain.Rows(selectedRow(gvMain)).FindControl("ibtnEdit"), ImageButton)
        Dim ibtnSave As ImageButton = CType(gvMain.Rows(selectedRow(gvMain)).FindControl("ibtnSave"), ImageButton)
        Dim ibtnCancel As ImageButton = CType(gvMain.Rows(selectedRow(gvMain)).FindControl("ibtnCancel"), ImageButton)

        Dim lblComment As Label = CType(gvMain.Rows(selectedRow(gvMain)).FindControl("lblComment"), Label)
        Dim txtComment As TextBox = CType(gvMain.Rows(selectedRow(gvMain)).FindControl("txtComment"), TextBox)

        Dim CompID As String = gvMain.DataKeys(selectedRow(gvMain))("CompID").ToString()
        Dim EmpID As String = gvMain.DataKeys(selectedRow(gvMain))("EmpID").ToString()
        Dim ApplyID As String = gvMain.DataKeys(selectedRow(gvMain))("ApplyID").ToString()
        Dim IsExit As String = gvMain.DataKeys(selectedRow(gvMain))("IsExit").ToString()
        If lblComment.Text <> "" Then
            IsExit = "Y"
        End If

        If e.CommandName = "btnEdit" Then
            lblComment.Visible = False
            txtComment.Visible = True

            ibtnEdit.Visible = False
            ibtnSave.Visible = True
            ibtnCancel.Visible = True

        ElseIf e.CommandName = "btnSave" Then
            If SaveData(CompID, ApplyID, EmpID, txtComment.Text, IsExit) Then
                lblComment.Text = txtComment.Text

                lblComment.Visible = True
                txtComment.Visible = False

                ibtnEdit.Visible = True
                ibtnSave.Visible = False
                ibtnCancel.Visible = False
            End If

        ElseIf e.CommandName = "btnCancel" Then
            txtComment.Text = lblComment.Text

            lblComment.Visible = True
            txtComment.Visible = False

            ibtnEdit.Visible = True
            ibtnSave.Visible = False
            ibtnCancel.Visible = False
        End If
    End Sub

    Private Function SaveData(ByVal CompID As String, ByVal ApplyID As String, ByVal EmpID As String, ByVal Comment As String, ByVal IsExit As String) As Boolean
        Dim objGS1 As New GS1()

        If Comment.Trim() = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", "調整原因")
            Return False
        End If

        '儲存資料
        Try
            If IsExit = "Y" Then
                Return objGS1.GS1150Update(ViewState.Item("GradeYear"), ViewState.Item("ApplyTime"), ApplyID, CompID, EmpID, ViewState.Item("Seq"), ViewState.Item("GradeSeq"), ViewState.Item("MainFlag"), Comment)
            Else
                Return objGS1.GS1150Insert(ViewState.Item("GradeYear"), ViewState.Item("ApplyTime"), ApplyID, CompID, EmpID, ViewState.Item("Seq"), ViewState.Item("GradeSeq"), ViewState.Item("MainFlag"), Comment)
            End If

        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, "[CC0201_99]" & Bsp.Utility.getMessage("E_00000") & Bsp.Utility.getInnerException(Me.FunID, ex))
            Return False
        End Try
    End Function
    Protected Sub gvMain_RowCreated(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.Header Then
            Select Case ViewState.Item("MainFlag")
                Case "1"
                    e.Row.Cells(3).Visible = False
                Case "2"
                    e.Row.Cells(4).Text = "單位主管整體評量"
                    e.Row.Cells(5).Text = "區處主管整體評量"
            End Select
        End If
        If e.Row.RowType = DataControlRowType.DataRow Then
            Select Case ViewState.Item("MainFlag")
                Case "1"
                    e.Row.Cells(3).Visible = False
            End Select
        End If
    End Sub
    Protected Sub gvMain_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvMain.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Then
            Select Case ViewState.Item("MainFlag")
                Case "1"
                    e.Row.Cells(3).Visible = False
            End Select
        End If
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim tmpBtn As New ImageButton
            tmpBtn = CType(e.Row.FindControl("ibtnEdit"), ImageButton)
            If ViewState.Item("Result") = "1" Then
                tmpBtn.Visible = False
            End If

            Select Case ViewState.Item("MainFlag")
                Case "1"
                    e.Row.Cells(3).Visible = False
            End Select
        End If

    End Sub
End Class
