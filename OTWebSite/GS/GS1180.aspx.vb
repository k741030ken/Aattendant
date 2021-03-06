'****************************************************
'功能說明：排序調整
'建立人員：Weicheng
'建立日期：2016.05.16
'****************************************************
Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.Common
Imports Microsoft.Practices.EnterpriseLibrary.Common
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports System.IO
Imports System
Imports System.Web.Services

Partial Class GS_GS1180
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
                Dim strScript As String = ""
                strScript = "window.onload = function () {saveOrder();};"
                If Not Page.ClientScript.IsClientScriptBlockRegistered(Me.GetType(), "WindowOnload") Then
                    Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "WindowOnload", strScript, True)
                End If

            Else
                Return
            End If
        End If
    End Sub

    Public Overrides Sub DoAction(ByVal Param As String)
        Select Case Param
            Case "btnUpdate"   '儲存返回
                SaveData()
                GoBack()
                'subGetData(
                '    ViewState.Item("CompID"), _
                '    ViewState.Item("ApplyID"), _
                '    ViewState.Item("ApplyTime"), _
                '    ViewState.Item("Seq"), _
                '    ViewState.Item("MainFlag"), _
                '    ViewState.Item("GradeYear"), _
                '    ViewState.Item("GradeSeq"), _
                '    ViewState.Item("EvaluationSeq"), _
                '    ViewState.Item("DeptEX"))
            Case "btnActionX"   '返回
                'Bsp.Utility.RunClientScript(Me, "window.top.close();")
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

    Private Sub subGetData(ByVal CompID As String, ByVal ApplyID As String, ByVal ApplyTime As String, ByVal Seq As String, ByVal MainFlag As String, ByVal GradeYear As String, ByVal GradeSeq As String _
                           , ByVal EvaluationSeq As String, ByVal DeptEx As String)
        Dim objGS1 As New GS1()
        Dim objHR As New HR()

        Try
            Using dt As DataTable = objHR.GetHROrganName(CompID, ApplyID)
                If dt.Rows.Count > 0 Then
                    txtDeptID.Text = "部門：" & dt.Rows(0)(0).ToString()
                End If
            End Using
            '特優
            Using dt As DataTable = objGS1.GS1180Query(CompID, ApplyID, ApplyTime, Seq, MainFlag, GradeYear, GradeSeq, EvaluationSeq, "9", ViewState.Item("DeptEX"), ViewState.Item("GroupID"))
                If dt.Rows.Count > 0 Then
                    Grade9.DataSource = dt
                    Grade9.DataBind()
                Else
                    tdGrade9.Style("display") = "none"
                End If
            End Using
            '優
            Using dt As DataTable = objGS1.GS1180Query(CompID, ApplyID, ApplyTime, Seq, MainFlag, GradeYear, GradeSeq, EvaluationSeq, "1", ViewState.Item("DeptEX"), ViewState.Item("GroupID"))
                If dt.Rows.Count > 0 Then
                    Grade1.DataSource = dt
                    Grade1.DataBind()
                Else
                    tdGrade1.Style("display") = "none"
                End If
            End Using
            '甲上
            Using dt As DataTable = objGS1.GS1180Query(CompID, ApplyID, ApplyTime, Seq, MainFlag, GradeYear, GradeSeq, EvaluationSeq, "6", ViewState.Item("DeptEX"), ViewState.Item("GroupID"))
                If dt.Rows.Count > 0 Then
                    Grade6.DataSource = dt
                    Grade6.DataBind()
                Else
                    tdGrade6.Style("display") = "none"
                End If
            End Using
            '甲
            Using dt As DataTable = objGS1.GS1180Query(CompID, ApplyID, ApplyTime, Seq, MainFlag, GradeYear, GradeSeq, EvaluationSeq, "2", ViewState.Item("DeptEX"), ViewState.Item("GroupID"))
                If dt.Rows.Count > 0 Then
                    Grade2.DataSource = dt
                    Grade2.DataBind()
                Else
                    tdGrade2.Style("display") = "none"
                End If
            End Using
            '甲下
            Using dt As DataTable = objGS1.GS1180Query(CompID, ApplyID, ApplyTime, Seq, MainFlag, GradeYear, GradeSeq, EvaluationSeq, "7", ViewState.Item("DeptEX"), ViewState.Item("GroupID"))
                If dt.Rows.Count > 0 Then
                    Grade7.DataSource = dt
                    Grade7.DataBind()
                Else
                    tdGrade7.Style("display") = "none"
                End If
            End Using
            '乙
            Using dt As DataTable = objGS1.GS1180Query(CompID, ApplyID, ApplyTime, Seq, MainFlag, GradeYear, GradeSeq, EvaluationSeq, "3", ViewState.Item("DeptEX"), ViewState.Item("GroupID"))
                If dt.Rows.Count > 0 Then
                    Grade3.DataSource = dt
                    Grade3.DataBind()
                Else
                    tdGrade3.Style("display") = "none"
                End If
            End Using
            '丙
            Using dt As DataTable = objGS1.GS1180Query(CompID, ApplyID, ApplyTime, Seq, MainFlag, GradeYear, GradeSeq, EvaluationSeq, "4", ViewState.Item("DeptEX"), ViewState.Item("GroupID"))
                If dt.Rows.Count > 0 Then
                    Grade4.DataSource = dt
                    Grade4.DataBind()
                Else
                    tdGrade4.Style("display") = "none"
                End If
            End Using
            
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & ".DoQuery", ex)
        End Try
    End Sub
    
    Private Function SaveData() As Boolean
        Dim objGS1 As New GS1()

        '儲存資料
        Try
            Dim strValue As String
            strValue = hidValue9.Value + "," + hidValue1.Value + "," + hidValue6.Value + "," + hidValue2.Value + "," + hidValue7.Value + "," + hidValue3.Value + "," + hidValue4.Value
            Dim aryEmpID() As String = strValue.Replace(",,", ",").Split(",")
            Return objGS1.GS1180Update(aryEmpID, ViewState.Item("CompID"), ViewState.Item("ApplyID"), ViewState.Item("ApplyTime"), ViewState.Item("Seq"), ViewState.Item("GradeSeq"), ViewState.Item("GradeYear"), ViewState.Item("IsSignNext"), ViewState.Item("MainFlag"))

        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, "[CC0201_99]" & Bsp.Utility.getMessage("E_00000") & Bsp.Utility.getInnerException(Me.FunID, ex))
            Return False
        End Try
    End Function

    <WebMethod()> _
    Public Shared Function SaveListOrder(EmpID As Integer()) As String
        Dim strKeyValue As String
        strKeyValue = ""
        For i As Integer = 0 To EmpID.Length - 1
            'Dim id As Integer = ids(i)
            '...
            'Dim ordinal As Integer = i
            If strKeyValue = "" Then
                strKeyValue = EmpID(i)
            Else
                strKeyValue = strKeyValue & "," & EmpID(i)
            End If
        Next

        Return strKeyValue

    End Function
   
End Class
