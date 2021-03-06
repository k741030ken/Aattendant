'****************************************************
'功能說明：新進員工文件繳交作業_銀行-新增
'建立人員：Micky Sung
'建立日期：2015.07.06
'****************************************************
Imports System.Data

Partial Class RG_RG1301
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then
            '公司代碼
            txtCompID.Text = UserProfile.SelectCompRoleName

            '員工編號、員工姓名
            'ucQueryEmp.ShowCompRole = False
            ucQueryEmp.ShowCompRole = "False"
            ucQueryEmp.InValidFlag = "N"
            ucQueryEmp.SelectCompID = UserProfile.SelectCompRoleID
        End If
    End Sub

    Protected Overrides Sub BaseOnPageTransfer(ByVal ti As TransferInfo)
        If Not IsPostBack() Then

        End If
    End Sub

    Public Overrides Sub DoAction(ByVal Param As String)
        Select Case Param
            Case "btnAdd"   '存檔返回
                ViewState.Item("Action") = "btnAdd"
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
        Dim objRG As New RG1()
        Dim beCheckInFile_SPHBK1 As New beCheckInFile_SPHBK1.Row()
        Dim bsCheckInFile_SPHBK1 As New beCheckInFile_SPHBK1.Service()

        beCheckInFile_SPHBK1.CompID.Value = UserProfile.SelectCompRoleID
        beCheckInFile_SPHBK1.EmpID.Value = txtEmpID.Text.ToUpper
        beCheckInFile_SPHBK1.Remark.Value = txtRemark.Text
        beCheckInFile_SPHBK1.File1.Value = IIf(chkFile1.Checked, "1", "0")
        beCheckInFile_SPHBK1.File2.Value = IIf(chkFile2.Checked, "1", "0")
        beCheckInFile_SPHBK1.File3.Value = IIf(chkFile3.Checked, "1", "0")
        beCheckInFile_SPHBK1.File4.Value = IIf(chkFile4.Checked, "1", "0")
        beCheckInFile_SPHBK1.File5.Value = IIf(chkFile5.Checked, "1", "0")
        beCheckInFile_SPHBK1.File6.Value = IIf(chkFile6.Checked, "1", "0")
        beCheckInFile_SPHBK1.File7.Value = IIf(chkFile7.Checked, "1", "0")
        beCheckInFile_SPHBK1.File8.Value = IIf(chkFile8.Checked, "1", "0")
        beCheckInFile_SPHBK1.File9.Value = IIf(chkFile9.Checked, "1", "0")
        beCheckInFile_SPHBK1.File10.Value = IIf(chkFile10.Checked, "1", "0")
        beCheckInFile_SPHBK1.File11.Value = IIf(chkFile11.Checked, "1", "0")
        beCheckInFile_SPHBK1.File12.Value = IIf(chkFile12.Checked, "1", "0")
        beCheckInFile_SPHBK1.File13.Value = IIf(chkFile13.Checked, "1", "0")
        beCheckInFile_SPHBK1.File14.Value = IIf(chkFile14.Checked, "1", "0")
        beCheckInFile_SPHBK1.File15.Value = IIf(chkFile15.Checked, "1", "0")
        beCheckInFile_SPHBK1.File16.Value = IIf(chkFile16.Checked, "1", "0")
        beCheckInFile_SPHBK1.File17.Value = IIf(chkFile17.Checked, "1", "0")
        beCheckInFile_SPHBK1.File18.Value = IIf(chkFile18.Checked, "1", "0")
        beCheckInFile_SPHBK1.File19.Value = IIf(chkFile19.Checked, "1", "0")
        '2015/12/01 Add
        beCheckInFile_SPHBK1.LastChgComp.Value = UserProfile.ActCompID
        beCheckInFile_SPHBK1.LastChgID.Value = UserProfile.ActUserID
        beCheckInFile_SPHBK1.LastChgDate.Value = Now

        '檢查資料是否存在
        If bsCheckInFile_SPHBK1.IsDataExists(beCheckInFile_SPHBK1) Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00010", "")
            Return False
        End If

        '儲存資料
        Try
            Return objRG.AddCheckInFile_SPHBK1(beCheckInFile_SPHBK1, UserProfile.ActCompID, UserProfile.ActUserID)
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, "[CC0201_99]" & Bsp.Utility.getMessage("E_00000") & Bsp.Utility.getInnerException(Me.FunID, ex))
            Return False
        End Try
    End Function

    Private Function funCheckData() As Boolean
        Dim objRG As New RG1()
        Dim objHR As New HR()
        Dim beCheckInFile_SPHBK1 As New beCheckInFile_SPHBK1.Row()
        Dim bsCheckInFile_SPHBK1 As New beCheckInFile_SPHBK1.Service()

        '員工編號
        If txtEmpID.Text.Trim = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblEmpID.Text)
            txtEmpID.Focus()
            Return False
        End If
        If Bsp.Utility.getStringLength(txtEmpID.Text.Trim) > txtEmpID.MaxLength Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00040", lblEmpID.Text, txtEmpID.MaxLength.ToString)
            Return False
        End If

        '員工編號不存在
        If Not CheckEmpData(False) Then
            Return False
        End If

        '備註
        If txtRemark.Text.Trim.Length > txtRemark.MaxLength Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00040", lblRemark.Text, txtRemark.MaxLength.ToString)
            txtRemark.Focus()
            Return False
        End If

        Return True
    End Function

    Private Sub ClearData()
        txtEmpID.Text = ""

        ClearCheckBox()
    End Sub

    Private Sub ClearCheckBox()
        txtName.Text = ""
        txtOrgan.Text = ""
        txtEmpDate.Text = ""
        txtRemark.Text = ""
        chkFileAll.Checked = False
        chkFileAll.Enabled = False

        For i As Integer = 1 To 19
            Dim chkFile As CheckBox = Me.FindControl("chkFile" + i.ToString)
            chkFile.Checked = False
            chkFile.Enabled = True
        Next
    End Sub

    Public Overrides Sub DoModalReturn(ByVal returnValue As String)
        Dim strSql As String = ""

        If returnValue <> "" Then
            Dim aryData() As String = returnValue.Split(":")

            Select Case aryData(0)
                Case "ucQueryEmp"
                    Dim aryValue() As String = Split(aryData(1), "|$|")
                    '員工編號
                    txtEmpID.Text = aryValue(1)
                    ''員工姓名
                    'txtName.Text = aryValue(2)
                    CheckEmpData(True)
            End Select
        End If
    End Sub

    Private Function CheckEmpData(ByVal IsEnableFile As Boolean) As Boolean
        Dim objRG As New RG1
        Dim objHR As New HR()
        Dim PersonalTable As DataTable = objRG.GetEmpData(UserProfile.SelectCompRoleID, txtEmpID.Text.ToUpper)

        If PersonalTable.Rows.Count > 0 Then
            txtName.Text = PersonalTable.Rows(0).Item("NameN")
            txtOrgan.Text = PersonalTable.Rows(0).Item("OrganName")
            txtEmpDate.Text = PersonalTable.Rows(0).Item("EmpDate")
            chkFileAll.Enabled = True

            If IsEnableFile Then
                If PersonalTable.Rows(0).Item("EmpType") = "1" Then '正式員工
                    If PersonalTable.Rows(0).Item("Sex") = "2" Or PersonalTable.Rows(0).Item("NationID") = "2" Then
                        chkFile9.Enabled = False
                        chkFile9.Checked = True
                    End If

                    chkFile14.Enabled = False
                    chkFile14.Checked = True
                    chkFile15.Enabled = False
                    chkFile15.Checked = True

                    If objHR.FunGetRankIDMap(UserProfile.SelectCompRoleID, PersonalTable.Rows(0).Item("RankID")) < "07" Then
                        chkFile16.Enabled = False
                        chkFile16.Checked = True
                    End If

                    If Not objRG.IsCredit(UserProfile.SelectCompRoleID, txtEmpID.Text.ToUpper, "") Then
                        chkFile18.Enabled = False
                        chkFile18.Checked = True
                    End If

                ElseIf PersonalTable.Rows(0).Item("EmpType") = "3" Then '臨時人員
                    chkFile3.Enabled = False
                    chkFile3.Checked = True
                    chkFile4.Enabled = False
                    chkFile4.Checked = True
                    chkFile6.Enabled = False
                    chkFile6.Checked = True
                    chkFile7.Enabled = False
                    chkFile7.Checked = True
                    chkFile8.Enabled = False
                    chkFile8.Checked = True
                    chkFile9.Enabled = False
                    chkFile9.Checked = True
                    chkFile11.Enabled = False
                    chkFile11.Checked = True
                    chkFile12.Enabled = False
                    chkFile12.Checked = True
                    chkFile13.Enabled = False
                    chkFile13.Checked = True
                    chkFile16.Enabled = False
                    chkFile16.Checked = True
                End If
            End If

            Return True
        Else
            Bsp.Utility.ShowMessage(Me, "員工編號不存在！")
            txtEmpID.Focus()
            Return False
        End If
    End Function

    Protected Sub txtEmpID_TextChanged(sender As Object, e As System.EventArgs) Handles txtEmpID.TextChanged
        ClearCheckBox()
        CheckEmpData(True)
    End Sub

    Protected Sub chkFileAll_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkFileAll.CheckedChanged
        If chkFileAll.Checked = True Then
            For i As Integer = 1 To 19
                Dim chkFile As CheckBox = Me.FindControl("chkFile" + i.ToString)
                If chkFile.Enabled = True Then
                    chkFile.Checked = True
                End If
            Next
        Else
            For i As Integer = 1 To 19
                Dim chkFile As CheckBox = Me.FindControl("chkFile" + i.ToString)
                If chkFile.Enabled = True Then
                    chkFile.Checked = False
                End If
            Next
        End If
    End Sub
End Class
