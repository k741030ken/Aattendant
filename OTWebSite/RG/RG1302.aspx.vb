'****************************************************
'功能說明：新進員工文件繳交作業_銀行-修改
'建立人員：Micky Sung
'建立日期：2015.07.06
'****************************************************
Imports System.Data

Partial Class RG_RG1302
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then

        End If
    End Sub

    Protected Overrides Sub BaseOnPageTransfer(ByVal ti As TransferInfo)
        If Not IsPostBack() Then
            Dim ht As Hashtable = Bsp.Utility.getHashTableFromParam(ti.Args)

            If ht.ContainsKey("SelectedCompID") Then
                ViewState.Item("CompID") = ht("SelectedCompID").ToString()
                ViewState.Item("EmpType") = "0"
                subGetData(ht("SelectedCompID").ToString(), ht("SelectedEmpID").ToString())
            Else
                Return
            End If
        End If
    End Sub

    Public Overrides Sub DoAction(ByVal Param As String)
        Select Case Param
            Case "btnUpdate"   '存檔返回
                ViewState.Item("Action") = "btnUpdate"
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
        Dim objHR As New HR()
        Dim beCheckInFile_SPHBK1 As New beCheckInFile_SPHBK1.Row()
        Dim bsCheckInFile_SPHBK1 As New beCheckInFile_SPHBK1.Service()
        Dim strCheckInFlag As String = ""
        Dim strSalaryFlag As String = ""

        beCheckInFile_SPHBK1.CompID.Value = ViewState.Item("CompID")
        beCheckInFile_SPHBK1.EmpID.Value = txtEmpID.Text
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

        If chkFile1.Checked = True And chkFile2.Checked = True And chkFile3.Checked = True _
            And chkFile4.Checked = True And chkFile5.Checked = True And chkFile6.Checked = True _
            And chkFile7.Checked = True And chkFile8.Checked = True And chkFile9.Checked = True _
            And chkFile10.Checked = True And chkFile11.Checked = True And chkFile12.Checked = True _
            And chkFile13.Checked = True And chkFile14.Checked = True And chkFile15.Checked = True _
            And chkFile16.Checked = True And chkFile17.Checked = True And chkFile18.Checked = True _
            And chkFile19.Checked = True Then
            strCheckInFlag = "3"
        Else
            If ViewState.Item("EmpType") = "1" Then
                If chkFile1.Checked = True And chkFile2.Checked = True And chkFile3.Checked = True _
                    And chkFile4.Checked = True And chkFile5.Checked = True And chkFile6.Checked = True _
                    And chkFile7.Checked = True And chkFile8.Checked = True And chkFile9.Checked = True _
                    And chkFile10.Checked = True And chkFile11.Checked = True And chkFile17.Checked = True _
                    And chkFile18.Checked = True And chkFile19.Checked = True Then
                    strCheckInFlag = "2"
                Else
                    strCheckInFlag = "1"
                End If
            ElseIf ViewState.Item("EmpType") = "3" Then
                If chkFile1.Checked = True And chkFile2.Checked = True And chkFile5.Checked = True _
                    And chkFile10.Checked = True And chkFile14.Checked = True And chkFile15.Checked = True _
                    And chkFile17.Checked = True And chkFile19.Checked = True Then
                    strCheckInFlag = "2"
                Else
                    strCheckInFlag = "1"
                End If
            End If
        End If

        '20160823 Beatrice mod
        'If strCheckInFlag = "2" Or strCheckInFlag = "3" Then
        '    strSalaryFlag = "1"
        'Else
        '    strSalaryFlag = "0"
        'End If

        If chkFile7.Checked = False And chkFile8.Checked = False Then
            strSalaryFlag = "0"
        Else
            If chkFile1.Checked = True And chkFile2.Checked = True And chkFile3.Checked = True _
                And chkFile4.Checked = True And chkFile5.Checked = True And chkFile6.Checked = True _
                And chkFile10.Checked = True And chkFile11.Checked = True And chkFile17.Checked = True _
                And chkFile19.Checked = True Then
                strSalaryFlag = "1"
            Else
                strSalaryFlag = "0"
            End If
        End If

        If objRG.IsSWorkType(ViewState.Item("CompID"), txtEmpID.Text) And chkFile18.Checked = True Then
            strSalaryFlag = "1"
        End If

        '儲存資料
        Try
            Return objRG.UpdateCheckInFile_SPHBK1(beCheckInFile_SPHBK1, UserProfile.ActCompID, UserProfile.ActUserID, strCheckInFlag, strSalaryFlag)
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, "[CC0201_99]" & Bsp.Utility.getMessage("E_00000") & Bsp.Utility.getInnerException(Me.FunID, ex))
            Return False
        End Try
    End Function

    Private Sub subGetData(ByVal CompID As String, ByVal EmpID As String)
        Dim objRG As New RG1()
        Dim objHR As New HR()
        Dim objSC As New SC()

        Dim beCheckInFile_SPHBK1 As New beCheckInFile_SPHBK1.Row()
        Dim bsCheckInFile_SPHBK1 As New beCheckInFile_SPHBK1.Service()

        beCheckInFile_SPHBK1.CompID.Value = CompID
        beCheckInFile_SPHBK1.EmpID.Value = EmpID
        Try
            Using dt As DataTable = bsCheckInFile_SPHBK1.QueryByKey(beCheckInFile_SPHBK1).Tables(0)
                If dt.Rows.Count <= 0 Then Exit Sub
                beCheckInFile_SPHBK1 = New beCheckInFile_SPHBK1.Row(dt.Rows(0))

                txtCompID.Text = beCheckInFile_SPHBK1.CompID.Value
                txtEmpID.Text = beCheckInFile_SPHBK1.EmpID.Value
                txtRemark.Text = beCheckInFile_SPHBK1.Remark.Value

                Dim PersonalTable As DataTable = objRG.GetEmpData(beCheckInFile_SPHBK1.CompID.Value, beCheckInFile_SPHBK1.EmpID.Value)

                If PersonalTable.Rows.Count > 0 Then
                    txtName.Text = PersonalTable.Rows(0).Item("NameN")
                    txtOrgan.Text = PersonalTable.Rows(0).Item("OrganName")
                    txtEmpDate.Text = PersonalTable.Rows(0).Item("EmpDate")

                    ViewState.Item("EmpType") = PersonalTable.Rows(0).Item("EmpType")

                    '欄位disable控制
                    If ViewState.Item("EmpType") = "1" Then '正式員工
                        If PersonalTable.Rows(0).Item("Sex") = "2" Or PersonalTable.Rows(0).Item("NationID") = "2" Then
                            chkFile9.Enabled = False
                            chkFile9.Checked = True
                        End If

                        chkFile14.Enabled = False
                        chkFile15.Enabled = False

                        If objHR.FunGetRankIDMap(beCheckInFile_SPHBK1.CompID.Value, PersonalTable.Rows(0).Item("RankID")) < "07" Then
                            chkFile16.Enabled = False
                            chkFile16.Checked = True
                        End If

                        If Not objRG.IsCredit(beCheckInFile_SPHBK1.CompID.Value, beCheckInFile_SPHBK1.EmpID.Value, "") Then
                            chkFile18.Enabled = False
                            chkFile18.Checked = True
                        End If

                    ElseIf ViewState.Item("EmpType") = "3" Then '臨時人員
                        chkFile3.Enabled = False
                        chkFile4.Enabled = False
                        chkFile6.Enabled = False
                        chkFile7.Enabled = False
                        chkFile8.Enabled = False
                        chkFile9.Enabled = False
                        chkFile11.Enabled = False
                        chkFile12.Enabled = False
                        chkFile13.Enabled = False
                        chkFile16.Enabled = False
                    End If
                End If

                chkFile1.Checked = IIf(beCheckInFile_SPHBK1.File1.Value = "1", True, False)
                chkFile2.Checked = IIf(beCheckInFile_SPHBK1.File2.Value = "1", True, False)
                chkFile3.Checked = IIf(beCheckInFile_SPHBK1.File3.Value = "1", True, False)
                chkFile4.Checked = IIf(beCheckInFile_SPHBK1.File4.Value = "1", True, False)
                chkFile5.Checked = IIf(beCheckInFile_SPHBK1.File5.Value = "1", True, False)
                chkFile6.Checked = IIf(beCheckInFile_SPHBK1.File6.Value = "1", True, False)
                chkFile7.Checked = IIf(beCheckInFile_SPHBK1.File7.Value = "1", True, False)
                chkFile8.Checked = IIf(beCheckInFile_SPHBK1.File8.Value = "1", True, False)
                chkFile9.Checked = IIf(beCheckInFile_SPHBK1.File9.Value = "1", True, False)
                chkFile10.Checked = IIf(beCheckInFile_SPHBK1.File10.Value = "1", True, False)
                chkFile11.Checked = IIf(beCheckInFile_SPHBK1.File11.Value = "1", True, False)
                chkFile12.Checked = IIf(beCheckInFile_SPHBK1.File12.Value = "1", True, False)
                chkFile13.Checked = IIf(beCheckInFile_SPHBK1.File13.Value = "1", True, False)
                chkFile14.Checked = IIf(beCheckInFile_SPHBK1.File14.Value = "1", True, False)
                chkFile15.Checked = IIf(beCheckInFile_SPHBK1.File15.Value = "1", True, False)
                chkFile16.Checked = IIf(beCheckInFile_SPHBK1.File16.Value = "1", True, False)
                chkFile17.Checked = IIf(beCheckInFile_SPHBK1.File17.Value = "1", True, False)
                chkFile18.Checked = IIf(beCheckInFile_SPHBK1.File18.Value = "1", True, False)
                chkFile19.Checked = IIf(beCheckInFile_SPHBK1.File19.Value = "1", True, False)

                '2015/12/01 Add 增加欄位:最後異動公司,最後異動人員,最後異動日期
                '最後異動公司
                Dim CompName As String = objSC.GetSC_CompName(beCheckInFile_SPHBK1.LastChgComp.Value)
                txtLastChgComp.Text = beCheckInFile_SPHBK1.LastChgComp.Value + IIf(CompName <> "", "-" + CompName, "")

                '最後異動人員
                Dim UserName As String = objSC.GetSC_UserName(beCheckInFile_SPHBK1.LastChgComp.Value, beCheckInFile_SPHBK1.LastChgID.Value)
                txtLastChgID.Text = beCheckInFile_SPHBK1.LastChgID.Value + IIf(UserName <> "", "-" + UserName, "")

                '最後異動日期
                Dim boolDate As Boolean = Format(beCheckInFile_SPHBK1.LastChgDate.Value, "yyyy/MM/dd") = "1900/01/01"
                txtLastChgDate.Text = IIf(boolDate, "", beCheckInFile_SPHBK1.LastChgDate.Value.ToString("yyyy/MM/dd HH:mm:ss"))


            End Using
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & "subGetData", ex)
        End Try

    End Sub

    Private Function funCheckData() As Boolean
        '備註
        If txtRemark.Text.Trim.Length > txtRemark.MaxLength Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00040", lblRemark.Text, txtRemark.MaxLength.ToString)
            txtRemark.Focus()
            Return False
        End If

        Return True
    End Function

    Private Sub ClearData()
        subGetData(ViewState.Item("CompID"), txtEmpID.Text)
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
            End Select
        End If
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
