'****************************************************
'功能說明：Web我們的同仁組織圖列印-新增
'建立人員：BeatriceCheng
'建立日期：2015.05.11
'****************************************************
Imports System.Data

Partial Class PA_PA4201
    Inherits PageBase


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then
            txtComID.Text = UserProfile.SelectCompRoleName
            ucSelectEmpID.ShowCompRole = False
            ucSelectEmpID.SelectCompID = UserProfile.SelectCompRoleID
        End If
    End Sub
    Protected Overrides Sub BaseOnPageTransfer(ByVal ti As TransferInfo)
        If Not IsPostBack() Then

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
        Dim beExOrganPrint As New beExOrganPrint.Row()
        Dim bsExOrganPrint As New beExOrganPrint.Service()
        Dim objPA4 As New PA4()

        '取得輸入資料
        beExOrganPrint.CompID.Value = UserProfile.SelectCompRoleID
        beExOrganPrint.EmpID.Value = txtEmpID.Text
        beExOrganPrint.OrganPrintFlag.Value = ddlOrganPrintFlag.SelectedValue
        beExOrganPrint.PrintType.Value = ddlPrintType.SelectedValue

        beExOrganPrint.CreateComp.Value = UserProfile.ActCompID
        beExOrganPrint.CreateID.Value = UserProfile.ActUserID
        beExOrganPrint.CreateTime.Value = Now

        beExOrganPrint.LastChgComp.Value = UserProfile.ActCompID
        beExOrganPrint.LastChgID.Value = UserProfile.ActUserID
        beExOrganPrint.LastChgDate.Value = Now

        '檢查資料是否存在
        If bsExOrganPrint.IsDataExists(beExOrganPrint) Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00010", "")
            Return False
        End If

        '儲存資料
        Try
            Return objPA4.ExOrganPrintAdd(beExOrganPrint)
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, "[CC0201_99]" & Bsp.Utility.getMessage("E_00000") & Bsp.Utility.getInnerException(Me.FunID, ex))
            Return False
        End Try
    End Function

    Private Function funCheckData() As Boolean
        Dim objPA As New PA4()
        Dim strValue As String = ""

        '員工編號
        strValue = txtEmpID.Text
        If strValue.Trim = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblEmpID.Text)
            Return False
        Else
            Dim EmpNameTable As DataTable = objPA.GetEmpName(UserProfile.SelectCompRoleID, txtEmpID.Text)
            If EmpNameTable.Rows().Count <= 0 Then
                Bsp.Utility.ShowMessage(Me, "人事資料尚未建檔")
                txtEmpID.Focus()
                Return False
            End If
        End If

        '權限註記
        strValue = ddlOrganPrintFlag.SelectedValue
        If strValue.Trim = "" Or strValue = "---請選擇---" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblOrganPrintFlag.Text)
            ddlOrganPrintFlag.Focus()
            Return False
        End If

        '列印範圍
        strValue = ddlPrintType.SelectedValue
        If strValue.Trim = "" Or strValue = "---請選擇---" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblPrintType.Text)
            ddlPrintType.Focus()
            Return False
        End If

        Return True
    End Function

    Private Sub ClearData()
        txtEmpID.Text = ""
        lblEmpName.Text = ""
        ddlOrganPrintFlag.SelectedIndex = 0
        ddlPrintType.SelectedIndex = 0
    End Sub

    Public Overrides Sub DoModalReturn(ByVal returnValue As String)
        Dim strSql As String = ""

        If returnValue <> "" Then
            Dim aryData() As String = returnValue.Split(":")
            Select Case aryData(0)
                '員工編號
                Case "ucSelectEmpID"
                    Dim aryValue() As String = Split(aryData(1), "|$|")
                    txtEmpID.Text = aryValue(1)
                    lblEmpName.Text = aryValue(2)
            End Select
        End If
    End Sub

    Protected Sub btnEmpID_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEmpID.Click
        If txtEmpID.Text <> "" Then
            Dim objPA As New PA4()
            Dim EmpNameTable As DataTable = objPA.GetEmpName(UserProfile.SelectCompRoleID, txtEmpID.Text)
            If EmpNameTable.Rows().Count > 0 Then
                lblEmpName.Text = EmpNameTable.Rows(0).Item(0)
            Else
                lblEmpName.Text = ""
                Bsp.Utility.ShowMessage(Me, "人事資料尚未建檔")
            End If
        Else
            lblEmpName.Text = ""
        End If
    End Sub

End Class
