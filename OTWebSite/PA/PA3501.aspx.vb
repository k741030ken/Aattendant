'****************************************************
'功能說明：科系設定-新增
'建立人員：BeatriceCheng
'建立日期：2015.05.07
'****************************************************
Imports System.Data

Partial Class PA_PA3501
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
        Dim beDepart As New beDepart.Row()
        Dim bsDepart As New beDepart.Service()
        Dim objPA3 As New PA3()

        '取得輸入資料
        beDepart.DepartID.Value = txtDepartID.Text
        beDepart.Remark.Value = txtRemark.Text

        beDepart.LastChgComp.Value = UserProfile.ActCompID
        beDepart.LastChgID.Value = UserProfile.ActUserID
        beDepart.LastChgDate.Value = Now

        '檢查資料是否存在
        If bsDepart.IsDataExists(beDepart) Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00010", "")
            Return False
        End If

        '儲存資料
        Try
            Return objPA3.DepartAdd(beDepart)
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, "[CC0201_99]" & Bsp.Utility.getMessage("E_00000") & Bsp.Utility.getInnerException(Me.FunID, ex))
            Return False
        End Try
    End Function

    Private Function funCheckData() As Boolean

        Dim strValue As String = ""

        '科系代碼
        strValue = txtDepartID.Text
        If strValue.Trim = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblDepartID.Text)
            txtDepartID.Focus()
            Return False
        End If
        If Bsp.Utility.getStringLength(strValue) > txtDepartID.MaxLength Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00040", lblDepartID.Text, txtDepartID.MaxLength.ToString())
            txtDepartID.Focus()
            Return False
        End If

        '科系名稱
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
        txtDepartID.Text = ""
        txtRemark.Text = ""
    End Sub

End Class
