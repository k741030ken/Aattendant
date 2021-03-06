'****************************************************
'功能說明：員工學歷資料維護-修改
'建立人員：Micky Sung
'建立日期：2015.06.04
'****************************************************
Imports System.Data
Imports Newtonsoft.Json

Partial Class ST_ST1302
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then
            '學歷
            Bsp.Utility.FillEduID(ddlEduID)
            ddlEduID.Items.Insert(0, New ListItem("---請選擇---", ""))

            '學校類別
            Bsp.Utility.FillSchoolType(ddlSchoolType)
            ddlSchoolType.Items.Insert(0, New ListItem("---請選擇---", ""))

            '校名
            ucSelectSchool.QuerySQL = "Select Remark as School, SchoolID from School order by SchoolID"
            ucSelectSchool.Fields = New FieldState() {New FieldState("School", "校名", True, True), New FieldState("SchoolID", "校名代碼", True, True)}

            '科系
            ucSelectDepart.QuerySQL = "Select Remark as Depart, DepartID from Depart Order by DepartID"
            ucSelectDepart.Fields = New FieldState() {New FieldState("Depart", "科系名稱", True, True), New FieldState("DepartID", "科系代碼", True, True)}

            '輔系
            ucSelectSecDepart.QuerySQL = "Select Remark as SecDepart, DepartID as SecDepartID from Depart Order by SecDepartID"
            ucSelectSecDepart.Fields = New FieldState() {New FieldState("SecDepart", "輔系名稱", True, True), New FieldState("SecDepartID", "輔系代碼", True, True)}
        End If
    End Sub

    Protected Overrides Sub BaseOnPageTransfer(ByVal ti As TransferInfo)
        If Not IsPostBack() Then
            Dim objSC As New SC
            Dim ht As Hashtable = Bsp.Utility.getHashTableFromParam(ti.Args)

            If ht.ContainsKey("SelectedIDNo") Then
                ViewState.Item("IDNo") = ht("SelectedIDNo").ToString()
                ViewState.Item("EduID") = ht("SelectedEduID").ToString()
                ViewState.Item("Seq") = ht("SelectedSeq").ToString()
                ViewState.Item("CompID") = ht("SelectedCompID").ToString()
                ViewState.Item("EmpID") = ht("SelectedEmpID").ToString()
                ViewState.Item("EmpName") = ht("SelectedEmpName").ToString()

                txtCompID.Text = ViewState.Item("CompID").ToString() + "-" + objSC.GetCompName(ViewState.Item("CompID").ToString()).Rows(0).Item("CompName").ToString
                txtEmpID.Text = ViewState.Item("EmpID").ToString()
                txtEmpName.Text = ViewState.Item("EmpName").ToString()
                subGetData(ViewState.Item("IDNo"), ViewState.Item("EduID"), ViewState.Item("Seq"))
            Else
                Return
            End If
        End If
    End Sub

    Public Overrides Sub DoAction(ByVal Param As String)
        Select Case Param
            Case "btnUpdate"   '存檔返回
                If funCheckData() Then
                    If SaveData() Then
                        GoBack()
                    End If
                End If
            Case "btnActionX"   '返回
                GoBack()
            Case "btnCancel"    '清除
                ClearData()
        End Select
    End Sub

    Private Sub GoBack()
        Dim ti As TransferInfo = Me.StateTransfer
        Me.TransferFramePage(ti.CallerUrl, Nothing, ti.Args)
    End Sub

    Private Function SaveData() As Boolean
        Dim beEducation As New beEducation.Row()
        Dim bsEducation As New beEducation.Service()
        Dim objST As New ST1

        beEducation.IDNo.Value = ViewState.Item("IDNo").ToString
        beEducation.EduID.OldValue = ViewState.Item("EduID")
        beEducation.EduID.Value = ddlEduID.SelectedValue
        beEducation.Seq.Value = ViewState.Item("Seq").ToString  '2015/11/24 Add
        beEducation.GraduateYear.Value = IIf(txtGraduateYear.Text <> "", txtGraduateYear.Text, "0")
        beEducation.SchoolType.Value = IIf(ddlSchoolType.SelectedValue <> "", ddlSchoolType.SelectedValue, "0")
        beEducation.SchoolID.Value = txtSchoolID.Text
        beEducation.School.Value = txtSchool.Text
        beEducation.DepartID.Value = txtDepartID.Text
        beEducation.Depart.Value = txtDepart.Text
        beEducation.SecDepartID.Value = txtSecDepartID.Text
        beEducation.SecDepart.Value = txtSecDepart.Text
        beEducation.EduStatus.Value = ddlEduStatus.SelectedValue
        beEducation.LastChgComp.Value = UserProfile.ActCompID
        beEducation.LastChgID.Value = UserProfile.ActUserID
        beEducation.LastChgDate.Value = Now

        '儲存資料
        Try
            Return objST.UpdateEducationSetting(beEducation, ViewState.Item("CompID").ToString(), ViewState.Item("EmpID"))
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, "[CC0201_99]" & Bsp.Utility.getMessage("E_00000") & Bsp.Utility.getInnerException(Me.FunID, ex))
            Return False
        End Try
    End Function

    Private Function funCheckData() As Boolean
        Dim objPA As New PA1()
        Dim beEducation As New beEducation.Row()
        Dim bsEducation As New beEducation.Service()

        '學歷狀態
        If ddlEduStatus.SelectedValue = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblEduStatus.Text)
            ddlEduStatus.Focus()
            Return False
        End If

        '畢業年度
        If txtGraduateYear.Text <> "" Then
            If IsNumeric(txtGraduateYear.Text.Trim) = False Then
                Bsp.Utility.ShowMessage(Me, "｢畢業年度｣需為四位數西元年度")
                txtGraduateYear.Focus()
                Return False
            ElseIf txtGraduateYear.Text.Trim.Length <> 4 Then
                Bsp.Utility.ShowMessage(Me, "｢畢業年度｣需為四位數西元年度")
                txtGraduateYear.Focus()
                Return False
            Else
                If CInt(txtGraduateYear.Text.Trim) < 1900 Or CInt(txtGraduateYear.Text.Trim) > Year(Now) Then
                    Bsp.Utility.ShowMessage(Me, "｢畢業年度｣不可大於目前的年度")
                    txtGraduateYear.Focus()
                    Return False
                End If
            End If
        End If

        '校名代碼
        If txtSchoolID.Text = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", "校名代碼")
            txtSchoolID.Focus()
            Return False
        End If

        '校名
        If txtSchool.Text = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", "校名")
            txtSchool.Focus()
            Return False
        End If

        '科系代碼
        If ddlEduID.Text > "030" Then
            If txtDepartID.Text = "" Then
                Bsp.Utility.ShowFormatMessage(Me, "W_00030", "科系代碼")
                txtDepartID.Focus()
                Return False
            End If

            '科系
            If txtDepart.Text = "" Then
                Bsp.Utility.ShowFormatMessage(Me, "W_00030", "科系")
                txtDepart.Focus()
                Return False
            End If
        End If

        Return True
    End Function

    Private Sub subGetData(ByVal IDNo As String, ByVal EduID As String, ByVal Seq As String)
        Dim objST As New ST1()
        Dim objSC As New SC
        Dim beEducation As New beEducation.Row()
        Dim bsEducation As New beEducation.Service()

        beEducation.IDNo.Value = IDNo
        beEducation.EduID.Value = EduID
        beEducation.Seq.Value = Seq

        Try
            Using dt As DataTable = bsEducation.QueryByKey(beEducation).Tables(0)
                If dt.Rows.Count <= 0 Then Exit Sub
                beEducation = New beEducation.Row(dt.Rows(0))
                ddlEduID.SelectedValue = beEducation.EduID.Value
                ddlEduStatus.SelectedValue = beEducation.EduStatus.Value
                txtGraduateYear.Text = beEducation.GraduateYear.Value
                txtGraduateYear.Text = IIf(beEducation.GraduateYear.Value <> 0, beEducation.GraduateYear.Value, "")
                ddlSchoolType.SelectedValue = beEducation.SchoolType.Value
                txtSchoolID.Text = beEducation.SchoolID.Value
                txtSchool.Text = beEducation.School.Value
                txtDepartID.Text = beEducation.DepartID.Value
                txtDepart.Text = beEducation.Depart.Value
                txtSecDepartID.Text = beEducation.SecDepartID.Value
                txtSecDepart.Text = beEducation.SecDepart.Value

                '最後異動公司
                Dim CompName As String = objSC.GetSC_CompName(beEducation.LastChgComp.Value)
                lblLastChgComp.Text = beEducation.LastChgComp.Value + IIf(CompName <> "", "-" + CompName, "")

                '最後異動人員
                Dim UserName As String = objSC.GetSC_UserName(beEducation.LastChgComp.Value, beEducation.LastChgID.Value)
                lblLastChgID.Text = beEducation.LastChgID.Value + IIf(UserName <> "", "-" + UserName, "")

                '最後異動日期
                Dim boolDate As Boolean = Format(beEducation.LastChgDate.Value, "yyyy/MM/dd") = "1900/01/01"
                lblLastChgDate.Text = IIf(boolDate, "", beEducation.LastChgDate.Value.ToString("yyyy/MM/dd HH:mm:ss"))

            End Using
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & "subGetData", ex)
        End Try

    End Sub

    Private Sub ClearData()
        subGetData(ViewState.Item("IDNo"), ViewState.Item("EduID"), ViewState.Item("Seq"))
    End Sub

    Public Overrides Sub DoModalReturn(ByVal returnValue As String)
        Dim strSql As String = ""

        If returnValue <> "" Then
            Dim aryData = returnValue.Substring(0, returnValue.IndexOf(":"))
            Dim aryValue As Dictionary(Of String, String) = JsonConvert.DeserializeObject(Of Dictionary(Of String, String))(returnValue.Replace(aryData & ":", ""))

            Select Case aryData
                Case "ucSelectSchool"
                    '校名
                    txtSchoolID.Text = aryValue.Item("SchoolID")
                    txtSchool.Text = aryValue.Item("School")
                Case "ucSelectDepart"
                    '科系
                    txtDepartID.Text = aryValue.Item("DepartID")
                    txtDepart.Text = aryValue.Item("Depart")
                Case "ucSelectSecDepart"
                    '輔系
                    txtSecDepartID.Text = aryValue.Item("SecDepartID")
                    txtSecDepart.Text = aryValue.Item("SecDepart")
            End Select
        End If
    End Sub

End Class
