'****************************************************
'功能說明：員工學歷資料維護-新增
'建立人員：Micky Sung
'建立日期：2015.05.25
'****************************************************
Imports System.Data
Imports Newtonsoft.Json

Partial Class ST_ST1301
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then
            '公司代碼
            'txtCompID.Text = UserProfile.SelectCompRoleName

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


            If ht.ContainsKey("SelectedCompID") Then
                ViewState.Item("CompID") = ht("SelectedCompID").ToString() '公司代碼
                ViewState.Item("EmpID") = ht("SelectedEmpID").ToString() '員工編號
                ViewState.Item("EmpName") = ht("SelectedEmpName").ToString() '員工姓名
                ViewState.Item("IDNo") = ht("SelectedIDNo").ToString() '員工身分證字號

                txtCompID.Text = ViewState.Item("CompID").ToString() + "-" + objSC.GetCompName(ViewState.Item("CompID").ToString()).Rows(0).Item("CompName").ToString
                txtEmpID.Text = ViewState.Item("EmpID").ToString()
                txtEmpName.Text = ViewState.Item("EmpName").ToString()
            End If
        End If
    End Sub

    Public Overrides Sub DoAction(ByVal Param As String)
        Select Case Param
            Case "btnAdd"   '存檔返回
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

        beEducation.IDNo.Value = ViewState.Item("IDNo")
        beEducation.EduID.Value = ddlEduID.SelectedValue
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
        beEducation.Seq.Value = objST.QueryEducationMaxSeq(ViewState.Item("IDNo"))

        '檢查資料是否存在
        If bsEducation.IsDataExists(beEducation) Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00010", "")
            Return False
        End If

        '儲存資料
        Try
            Return objST.AddEducationSetting(beEducation, ViewState.Item("CompID"), ViewState.Item("EmpID"))
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, "[CC0201_99]" & Bsp.Utility.getMessage("E_00000") & Bsp.Utility.getInnerException(Me.FunID, ex))
            Return False
        End Try
    End Function

    Private Function funCheckData() As Boolean
        Dim objPA As New PA1()
        Dim beEducation As New beEducation.Row()
        Dim bsEducation As New beEducation.Service()

        '學歷
        If ddlEduID.SelectedValue = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblEduID.Text)
            ddlEduID.Focus()
            Return False
        End If

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
        If ddlEduID.SelectedValue > "030" Then
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

    Private Sub ClearData()
        ddlEduID.SelectedValue = ""
        ddlEduStatus.SelectedValue = ""
        txtGraduateYear.Text = ""
        ddlSchoolType.SelectedValue = ""
        txtSchoolID.Text = ""
        txtSchool.Text = ""
        txtDepartID.Text = ""
        txtDepart.Text = ""
        txtSecDepartID.Text = ""
        txtSecDepart.Text = ""
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
