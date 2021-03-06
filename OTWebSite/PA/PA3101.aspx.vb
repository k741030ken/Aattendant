'****************************************************
'功能說明：學歷代碼設定-新增
'建立人員：BeatriceCheng
'建立日期：2015.05.04
'****************************************************
Imports System.Data

Partial Class PA_PA3101
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
        Dim beEduDegree As New beEduDegree.Row()
        Dim bsEduDegree As New beEduDegree.Service()
        Dim objPA3 As New PA3()

        '取得輸入資料
        beEduDegree.EduID.Value = txtEduID.Text
        beEduDegree.EduName.Value = txtEduName.Text
        beEduDegree.EduNameCN.Value = txtEduNameCN.Text

        beEduDegree.LastChgComp.Value = UserProfile.ActCompID
        beEduDegree.LastChgID.Value = UserProfile.ActUserID
        beEduDegree.LastChgDate.Value = Now

        '檢查資料是否存在
        If bsEduDegree.IsDataExists(beEduDegree) Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00010", "")
            Return False
        End If

        '儲存資料
        Try
            Return objPA3.EduDegreeAdd(beEduDegree)
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, "[CC0201_99]" & Bsp.Utility.getMessage("E_00000") & Bsp.Utility.getInnerException(Me.FunID, ex))
            Return False
        End Try
    End Function

    Private Function funCheckData() As Boolean

        Dim strValue As String = ""

        '學歷代碼
        strValue = txtEduID.Text
        If strValue.Trim = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblEduID.Text)
            txtEduID.Focus()
            Return False
        End If
        If Bsp.Utility.getStringLength(strValue) > txtEduID.MaxLength Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00040", lblEduID.Text, txtEduID.MaxLength.ToString())
            txtEduID.Focus()
            Return False
        End If

        '學歷名稱(繁)
        strValue = txtEduName.Text
        If strValue.Trim = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblEduName.Text)
            txtEduName.Focus()
            Return False
        End If
        'If Bsp.Utility.getStringLength(strValue) > txtEduName.MaxLength Then
        '    Bsp.Utility.ShowFormatMessage(Me, "W_00040", lblEduName.Text, txtEduName.MaxLength.ToString())
        '    txtEduName.Focus()
        '    Return False
        'End If

        '學歷名稱(簡)
        strValue = txtEduNameCN.Text
        If strValue.Trim = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblEduNameCN.Text)
            txtEduNameCN.Focus()
            Return False
        End If
        'If Bsp.Utility.getStringLength(strValue) > txtEduNameCN.MaxLength Then
        '    Bsp.Utility.ShowFormatMessage(Me, "W_00040", lblEduNameCN.Text, txtEduNameCN.MaxLength.ToString())
        '    txtEduNameCN.Focus()
        '    Return False
        'End If

        Return True
    End Function

    Private Sub ClearData()
        txtEduID.Text = ""
        txtEduName.Text = ""
        txtEduNameCN.Text = ""
    End Sub

End Class
