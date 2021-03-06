'****************************************************
'功能說明：學校設定-新增
'建立人員：BeatriceCheng
'建立日期：2015.05.06
'****************************************************
Imports System.Data

Partial Class PA_PA3401
    Inherits PageBase


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then

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
        Dim beSchool As New beSchool.Row()
        Dim bsSchool As New beSchool.Service()
        Dim objPA3 As New PA3()

        '取得輸入資料
        beSchool.SchoolID.Value = txtSchoolID.Text
        beSchool.Remark.Value = txtRemark.Text
        beSchool.PrimaryFlag.Value = IIf(cbPrimaryFlag.Checked, "1", "0")

        beSchool.LastChgComp.Value = UserProfile.ActCompID
        beSchool.LastChgID.Value = UserProfile.ActUserID
        beSchool.LastChgDate.Value = Now

        '檢查資料是否存在
        If bsSchool.IsDataExists(beSchool) Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00010", "")
            Return False
        End If

        '儲存資料
        Try
            Return objPA3.SchoolAdd(beSchool)
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, "[CC0201_99]" & Bsp.Utility.getMessage("E_00000") & Bsp.Utility.getInnerException(Me.FunID, ex))
            Return False
        End Try
    End Function

    Private Function funCheckData() As Boolean

        Dim strValue As String = ""

        '學校代碼
        strValue = txtSchoolID.Text
        If strValue.Trim = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblSchoolID.Text)
            txtSchoolID.Focus()
            Return False
        End If
        If Bsp.Utility.getStringLength(strValue) > txtSchoolID.MaxLength Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00040", lblSchoolID.Text, txtSchoolID.MaxLength.ToString())
            txtSchoolID.Focus()
            Return False
        End If

        '學校名稱
        strValue = txtRemark.Text
        If strValue.Trim = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblRemark.Text)
            txtRemark.Focus()
            Return False
        End If
        If strValue.Length > txtRemark.MaxLength Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00040", lblRemark.Text, txtRemark.MaxLength.ToString())
            txtRemark.Focus()
            Return False
        End If

        Return True
    End Function

    Private Sub ClearData()
        txtSchoolID.Text = ""
        txtRemark.Text = ""
        cbPrimaryFlag.Checked = False
    End Sub

End Class
