'****************************************************
'功能說明：員工訓練資料查詢-明細
'建立人員：BeatriceCheng
'建立日期：2015.06.11
'****************************************************
Imports System.Data

Partial Class ST_ST2302
    Inherits PageBase


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then

        End If
    End Sub

    Protected Overrides Sub BaseOnPageTransfer(ByVal ti As TransferInfo)
        If Not IsPostBack() Then
            Dim ht As Hashtable = Bsp.Utility.getHashTableFromParam(ti.Args)

            If ht.ContainsKey("SelectedIDNo") Then
                ViewState.Item("CompID") = ht("SelectedCompID").ToString()
                ViewState.Item("IDNo") = ht("SelectedIDNo").ToString()
                ViewState.Item("BeginDate") = ht("SelectedBeginDate").ToString()
                ViewState.Item("LessonName") = ht("SelectedLessonName").ToString()
                ViewState.Item("LessonID") = ht("SelectedLessonID").ToString()
                ViewState.Item("ActivityID") = ht("SelectedActivityID").ToString()
                subGetData(
                    ht("SelectedIDNo").ToString(), _
                    ht("SelectedBeginDate").ToString(), _
                    ht("SelectedLessonName").ToString(), _
                    ht("SelectedLessonID").ToString(), _
                    ht("SelectedActivityID").ToString())
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
        Me.TransferFramePage(ti.CallerUrl, Nothing, ti.Args)
    End Sub

    Private Sub subGetData(ByVal IDNo As String, ByVal BeginDate As String, ByVal LessonName As String, ByVal LessonID As String, ByVal ActivityID As String)
        Dim beTraining As New beTraining.Row()
        Dim bsTraining As New beTraining.Service()
        Dim objST2 As New ST2

        beTraining.IDNo.Value = IDNo
        beTraining.BeginDate.Value = BeginDate
        beTraining.LessonName.Value = LessonName
        beTraining.LessonID.Value = LessonID
        beTraining.ActivityID.Value = ActivityID
        Try
            Using dt As DataTable = objST2.GetEmpData(IDNo, ViewState.Item("CompID"))
                If dt.Rows.Count <= 0 Then Exit Sub

                txtCompID.Text = dt.Rows(0).Item("CompID")
                txtEmpID.Text = dt.Rows(0).Item("EmpID")
                txtName.Text = dt.Rows(0).Item("NameN")
            End Using

            Using dt As DataTable = bsTraining.QueryByKey(beTraining).Tables(0)
                If dt.Rows.Count <= 0 Then Exit Sub
                beTraining = New beTraining.Row(dt.Rows(0))

                txtBeginDate.Text = Format(beTraining.BeginDate.Value, "yyyy/MM/dd")
                txtEndDate.Text = Format(beTraining.EndDate.Value, "yyyy/MM/dd")
                txtLessonID.Text = beTraining.LessonID.Value
                txtLessonName.Text = beTraining.LessonName.Value
                txtActivityID.Text = beTraining.ActivityID.Value
                txtHours.Text = beTraining.Hours.Value
                txtFee.Text = beTraining.Fee.Value
                txtKindName.Text = beTraining.KindName.Value
                txtDeptName.Text = beTraining.DeptName.Value
            End Using
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & "subGetData", ex)
        End Try

    End Sub
End Class
