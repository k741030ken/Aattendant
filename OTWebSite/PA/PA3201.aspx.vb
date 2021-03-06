'****************************************************
'功能說明：任職狀況設定-新增
'建立人員：BeatriceCheng
'建立日期：2015.05.04
'****************************************************
Imports System.Data

Partial Class PA_PA3201
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
        Dim beWorkStatus As New beWorkStatus.Row()
        Dim bsWorkStatus As New beWorkStatus.Service()
        Dim objPA3 As New PA3()

        '取得輸入資料
        beWorkStatus.WorkCode.Value = txtWorkCode.Text
        beWorkStatus.Remark.Value = txtRemark.Text
        beWorkStatus.RemarkCN.Value = txtRemarkCN.Text

        beWorkStatus.LastChgComp.Value = UserProfile.ActCompID
        beWorkStatus.LastChgID.Value = UserProfile.ActUserID
        beWorkStatus.LastChgDate.Value = Now

        '檢查資料是否存在
        If bsWorkStatus.IsDataExists(beWorkStatus) Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00010", "")
            Return False
        End If

        '儲存資料
        Try
            Return objPA3.WorkStatusAdd(beWorkStatus)
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, "[CC0201_99]" & Bsp.Utility.getMessage("E_00000") & Bsp.Utility.getInnerException(Me.FunID, ex))
            Return False
        End Try
    End Function

    Private Function funCheckData() As Boolean

        Dim strValue As String = ""

        '任職代碼
        strValue = txtWorkCode.Text
        If strValue.Trim = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblWorkCode.Text)
            txtWorkCode.Focus()
            Return False
        End If
        If Bsp.Utility.getStringLength(strValue) > txtWorkCode.MaxLength Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00040", lblWorkCode.Text, txtWorkCode.MaxLength.ToString())
            txtWorkCode.Focus()
            Return False
        End If

        '任職名稱(繁)
        strValue = txtRemark.Text
        If strValue.Trim = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblRemark.Text)
            txtRemark.Focus()
            Return False
        End If
        'If Bsp.Utility.getStringLength(strValue) > txtRemark.MaxLength Then
        '    Bsp.Utility.ShowFormatMessage(Me, "W_00040", lblRemark.Text, txtRemark.MaxLength.ToString())
        '    txtRemark.Focus()
        '    Return False
        'End If

        '任職名稱(簡)
        strValue = txtRemarkCN.Text
        If strValue.Trim = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblRemarkCN.Text)
            txtRemarkCN.Focus()
            Return False
        End If
        'If Bsp.Utility.getStringLength(strValue) > txtRemarkCN.MaxLength Then
        '    Bsp.Utility.ShowFormatMessage(Me, "W_00040", lblRemarkCN.Text, txtRemarkCN.MaxLength.ToString())
        '    txtRemarkCN.Focus()
        '    Return False
        'End If

        Return True
    End Function

    Private Sub ClearData()
        txtWorkCode.Text = ""
        txtRemark.Text = ""
        txtRemarkCN.Text = ""
    End Sub

End Class
