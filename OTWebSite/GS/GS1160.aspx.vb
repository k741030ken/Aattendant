'****************************************************
'功能說明：考核補充說明畫面
'建立人員：BeatriceCheng
'建立日期：2015.11.03
'****************************************************
Imports System.Data

Partial Class GS_GS1160
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
            ViewState.Item("GroupID") = ht("GroupID").ToString()
            Select Case ViewState.Item("GroupID")
                Case "1"
                    txtGroup.Text = "群組：非管理職"
                Case "2"
                    txtGroup.Text = "群組：科主管"
                Case "3"
                    txtGroup.Text = "群組：單位主管"
                Case Else
                    txtGroup.Text = ""
            End Select
            Dim topDataCount As String = "0"
            Dim LastDataCount As String = "0"
            topDataCount = CInt(CSng(ht("DataCount").ToString()) * 0.2).ToString()
            LastDataCount = CInt(CSng(ht("DataCount").ToString()) * 0.15).ToString()
            Dim objHR As New HR()

            If ht.ContainsKey("CompID") Then
                Using dt As DataTable = objHR.GetHROrganName(ViewState.Item("CompID"), ViewState.Item("ApplyID"))
                    If dt.Rows.Count > 0 Then
                        txtDeptID.Text = "部門：" & dt.Rows(0)(0).ToString()
                    End If
                End Using

                subGetData(
                    ViewState.Item("CompID"), _
                    ViewState.Item("ApplyID"), _
                    ViewState.Item("ApplyTime"), _
                    ViewState.Item("Seq"), _
                    ViewState.Item("MainFlag"), _
                    ViewState.Item("GradeYear"), _
                    ViewState.Item("GradeSeq"), _
                    ViewState.Item("EvaluationSeq"), _
                    ViewState.Item("DeptEX"), _
                    topDataCount, _
                    LastDataCount)
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
            btnU.Caption = "排序上傳(專用格式)"
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

    Private Sub subGetData(ByVal CompID As String, ByVal ApplyID As String, ByVal ApplyTime As String, ByVal Seq As String, ByVal MainFlag As String, ByVal GradeYear As String, ByVal GradeSeq As String _
                           , ByVal EvaluationSeq As String, ByVal DeptEx As String, ByVal topDataCount As String, ByVal LastDataCount As String)
        Dim objGS1 As New GS1()
        Dim showComment As String = "1"
        ViewState("SelectSeq") = "-1"
        Try           
            If Not chkShowComment.Checked Then
                showComment = ""
            End If
            Using dt As DataTable = objGS1.GS1160Query(CompID, ApplyID, ApplyTime, Seq, GradeYear, GradeSeq, EvaluationSeq, MainFlag, DeptEx, topDataCount, LastDataCount, ViewState.Item("GroupID"), showComment)
                gvMain.DataSource = dt
                gvMain.DataBind()

            End Using
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & ".DoQuery", ex)
        End Try
    End Sub
    Protected Sub gvMain_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvMain.RowCommand

        If ViewState("SelectSeq") <> selectedRow(gvMain) And ViewState("SelectSeq") <> "-1" Then
            Dim lblPreComment As Label = CType(gvMain.Rows(ViewState("SelectSeq")).FindControl("lblComment"), Label)
            Dim txtPreComment As TextBox = CType(gvMain.Rows(ViewState("SelectSeq")).FindControl("txtComment"), TextBox)

            If txtPreComment.Visible = True Then
                Dim ibtnPreEdit As ImageButton = CType(gvMain.Rows(ViewState("SelectSeq")).FindControl("ibtnEdit"), ImageButton)
                Dim ibtnPreSave As ImageButton = CType(gvMain.Rows(ViewState("SelectSeq")).FindControl("ibtnSave"), ImageButton)
                Dim ibtnPreCancel As ImageButton = CType(gvMain.Rows(ViewState("SelectSeq")).FindControl("ibtnCancel"), ImageButton)

                Dim PreCompID As String = gvMain.DataKeys(ViewState("SelectSeq"))("CompID").ToString()
                Dim PreEmpID As String = gvMain.DataKeys(ViewState("SelectSeq"))("EmpID").ToString()
                Dim PreApplyID As String = gvMain.DataKeys(selectedRow(gvMain))("ApplyID").ToString()
                Dim PreIsExit As String = gvMain.DataKeys(selectedRow(gvMain))("IsExit").ToString()
                Dim PreGrade1 As String = gvMain.DataKeys(selectedRow(gvMain))("Grade1").ToString()
                Dim PreGrade2 As String = gvMain.DataKeys(selectedRow(gvMain))("Grade2").ToString()
                Dim PreIsDel As Boolean = False
                If (PreGrade1 = PreGrade2 And (PreGrade2 = "6" Or PreGrade2 = "2")) Or (PreGrade1 = "0" And (PreGrade2 = "6" Or PreGrade2 = "2")) Then
                    PreIsDel = True
                End If

                If lblPreComment.Text <> "" Then
                    PreIsExit = "Y"
                End If

                If SaveData(PreCompID, PreApplyID, PreEmpID, txtPreComment.Text, PreIsExit, PreIsDel) Then
                    lblPreComment.Text = txtPreComment.Text

                    lblPreComment.Visible = True
                    txtPreComment.Visible = False

                    ibtnPreEdit.Visible = True
                    ibtnPreSave.Visible = False
                    ibtnPreCancel.Visible = False
                Else
                    Return
                End If
            End If
        End If
        ViewState("SelectSeq") = selectedRow(gvMain)


        Dim ibtnEdit As ImageButton = CType(gvMain.Rows(selectedRow(gvMain)).FindControl("ibtnEdit"), ImageButton)
        Dim ibtnSave As ImageButton = CType(gvMain.Rows(selectedRow(gvMain)).FindControl("ibtnSave"), ImageButton)
        Dim ibtnCancel As ImageButton = CType(gvMain.Rows(selectedRow(gvMain)).FindControl("ibtnCancel"), ImageButton)

        Dim lblComment As Label = CType(gvMain.Rows(selectedRow(gvMain)).FindControl("lblComment"), Label)
        Dim txtComment As TextBox = CType(gvMain.Rows(selectedRow(gvMain)).FindControl("txtComment"), TextBox)

        Dim CompID As String = gvMain.DataKeys(selectedRow(gvMain))("CompID").ToString()
        Dim EmpID As String = gvMain.DataKeys(selectedRow(gvMain))("EmpID").ToString()
        Dim ApplyID As String = gvMain.DataKeys(selectedRow(gvMain))("ApplyID").ToString()
        Dim IsExit As String = gvMain.DataKeys(selectedRow(gvMain))("IsExit").ToString()
        Dim Grade1 As String = gvMain.DataKeys(selectedRow(gvMain))("Grade1").ToString()
        Dim Grade2 As String = gvMain.DataKeys(selectedRow(gvMain))("Grade2").ToString()
        Dim IsDel As Boolean = False
        If (Grade1 = Grade2 And (Grade2 = "6" Or Grade2 = "2")) Or (Grade1 = "0" And (Grade2 = "6" Or Grade2 = "2")) Then
            IsDel = True
        End If

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
            If SaveData(CompID, ApplyID, EmpID, txtComment.Text, IsExit, IsDel) Then
                lblComment.Text = txtComment.Text

                lblComment.Visible = True
                txtComment.Visible = False

                ibtnEdit.Visible = True
                ibtnSave.Visible = False
                ibtnCancel.Visible = False

                If IsDel And txtComment.Text.Trim <> " Then" Then
                    Dim topDataCount As String = "0"
                    Dim LastDataCount As String = "0"
                    topDataCount = 0
                    LastDataCount = 0
                    subGetData(
                    ViewState.Item("CompID"), _
                    ViewState.Item("ApplyID"), _
                    ViewState.Item("ApplyTime"), _
                    ViewState.Item("Seq"), _
                    ViewState.Item("MainFlag"), _
                    ViewState.Item("GradeYear"), _
                    ViewState.Item("GradeSeq"), _
                    ViewState.Item("EvaluationSeq"), _
                    ViewState.Item("DeptEX"), _
                    topDataCount, _
                    LastDataCount)
                End If
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
    Private Function SaveData(ByVal CompID As String, ByVal ApplyID As String, ByVal EmpID As String, ByVal Comment As String, ByVal IsExit As String, ByVal IsDel As Boolean) As Boolean
        Dim objGS1 As New GS1()

        If Comment.Trim() = "" And Not IsDel Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", "補充說明")
            Return False
        End If

        '儲存資料
        Try
            If IsExit = "Y" Then
                Return objGS1.GS1160Update(ViewState.Item("GradeYear"), ViewState.Item("ApplyTime"), ApplyID, CompID, EmpID, ViewState.Item("Seq"), ViewState.Item("GradeSeq"), ViewState.Item("MainFlag"), Comment)
            Else
                Return objGS1.GS1160Insert(ViewState.Item("GradeYear"), ViewState.Item("ApplyTime"), ApplyID, CompID, EmpID, ViewState.Item("Seq"), ViewState.Item("GradeSeq"), ViewState.Item("MainFlag"), Comment)
            End If

        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, "[CC0201_99]" & Bsp.Utility.getMessage("E_00000") & Bsp.Utility.getInnerException(Me.FunID, ex))
            Return False
        End Try
    End Function
    Protected Sub gvMain_RowCreated(sender As Object, e As GridViewRowEventArgs) Handles gvMain.RowCreated
        If e.Row.RowType = DataControlRowType.Header Then
            If ViewState.Item("MainFlag") = "2" Then
                e.Row.Cells(4).Text = "單位排序整體評量"
            End If
        End If
    End Sub
    Protected Sub gvMain_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvMain.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim tmpBtn As New ImageButton
            tmpBtn = CType(e.Row.FindControl("ibtnEdit"), ImageButton)
            If ViewState.Item("Result") = "1" Then
                tmpBtn.Visible = False
            End If



            For i = 4 To 5
                Select Case e.Row.Cells(i).Text
                    Case "1"
                        e.Row.Cells(i).Text = "優"
                    Case "2"
                        e.Row.Cells(i).Text = "甲"
                    Case "3"
                        e.Row.Cells(i).Text = "乙"
                    Case "4"
                        e.Row.Cells(i).Text = "丙"
                    Case "6"
                        e.Row.Cells(i).Text = "甲上"
                    Case "7"
                        e.Row.Cells(i).Text = "甲下"
                    Case "9"
                        e.Row.Cells(i).Text = "特優"
                    Case Else
                        e.Row.Cells(i).Text = ""
                End Select
            Next i

        End If
    End Sub
    Protected Sub chkShowComment_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkShowComment.CheckedChanged
        subGetData(
                    ViewState.Item("CompID"), _
                    ViewState.Item("ApplyID"), _
                    ViewState.Item("ApplyTime"), _
                    ViewState.Item("Seq"), _
                    ViewState.Item("MainFlag"), _
                    ViewState.Item("GradeYear"), _
                    ViewState.Item("GradeSeq"), _
                    ViewState.Item("EvaluationSeq"), _
                    ViewState.Item("DeptEX"), _
                    "0", _
                    "0")
    End Sub
End Class
