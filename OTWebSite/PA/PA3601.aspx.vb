'****************************************************
'功能說明：簽證國家設定-新增
'建立人員：BeatriceCheng
'建立日期：2015.05.08
'****************************************************
Imports System.Data

Partial Class PA_PA3601
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
        Dim beVisaCountry As New beVisaCountry.Row()
        Dim bsVisaCountry As New beVisaCountry.Service()
        Dim objPA3 As New PA3()

        '取得輸入資料
        beVisaCountry.Country.Value = txtCountry.Text
        beVisaCountry.CountryName.Value = txtCountryName.Text
        beVisaCountry.OfficeName.Value = txtOfficeName.Text
        beVisaCountry.Address.Value = txtAddress.Text

        beVisaCountry.LastChgComp.Value = UserProfile.ActCompID
        beVisaCountry.LastChgID.Value = UserProfile.ActUserID
        beVisaCountry.LastChgDate.Value = Now

        '檢查資料是否存在
        If bsVisaCountry.IsDataExists(beVisaCountry) Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00010", "")
            Return False
        End If

        '儲存資料
        Try
            Return objPA3.VisaCountryAdd(beVisaCountry)
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, "[CC0201_99]" & Bsp.Utility.getMessage("E_00000") & Bsp.Utility.getInnerException(Me.FunID, ex))
            Return False
        End Try
    End Function

    Private Function funCheckData() As Boolean

        Dim strValue As String = ""

        '簽證國(英文)
        strValue = txtCountry.Text
        If strValue.Trim = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblCountry.Text)
            txtCountry.Focus()
            Return False
        End If
        If Bsp.Utility.getStringLength(strValue) > txtCountry.MaxLength Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00040", lblCountry.Text, txtCountry.MaxLength.ToString())
            txtCountry.Focus()
            Return False
        End If

        '簽證國(中文)
        'strValue = txtCountryName.Text
        'If Bsp.Utility.getStringLength(strValue) > txtCountryName.MaxLength Then
        '    Bsp.Utility.ShowFormatMessage(Me, "W_00040", lblCountryName.Text, txtCountryName.MaxLength.ToString())
        '    txtCountryName.Focus()
        '    Return False
        'End If

        '在台辦事處名稱(英文)
        strValue = txtOfficeName.Text
        If Bsp.Utility.getStringLength(strValue) > txtOfficeName.MaxLength Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00040", lblOfficeName.Text, txtOfficeName.MaxLength.ToString())
            txtOfficeName.Focus()
            Return False
        End If

        '在台辦事處地址(英文)
        strValue = txtAddress.Text
        If Bsp.Utility.getStringLength(strValue) > txtAddress.MaxLength Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00040", lblAddress.Text, txtAddress.MaxLength.ToString())
            txtAddress.Focus()
            Return False
        End If

        Return True
    End Function

    Private Sub ClearData()
        txtCountry.Text = ""
        txtCountryName.Text = ""
        txtOfficeName.Text = ""
        txtAddress.Text = ""
    End Sub

End Class
