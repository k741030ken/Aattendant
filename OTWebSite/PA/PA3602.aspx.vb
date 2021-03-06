'****************************************************
'功能說明：簽證國家設定-修改
'建立人員：BeatriceCheng
'建立日期：2015.05.08
'****************************************************
Imports System.Data

Partial Class PA_PA3602
    Inherits PageBase


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then

        End If
    End Sub

    Protected Overrides Sub BaseOnPageTransfer(ByVal ti As TransferInfo)
        If Not IsPostBack() Then
            Dim ht As Hashtable = Bsp.Utility.getHashTableFromParam(ti.Args)

            If ht.ContainsKey("SelectedCountry") Then
                ViewState.Item("Country") = ht("SelectedCountry").ToString()
                subGetData(ht("SelectedCountry").ToString())
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

        '儲存資料
        Try
            Return objPA3.VisaCountryUpdate(beVisaCountry)
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, "[CC0201_99]" & Bsp.Utility.getMessage("E_00000") & Bsp.Utility.getInnerException(Me.FunID, ex))
            Return False
        End Try
    End Function

    Private Sub subGetData(ByVal Country As String)
        Dim objSC As New SC
        Dim beVisaCountry As New beVisaCountry.Row()
        Dim bsVisaCountry As New beVisaCountry.Service()
        Dim objPA3 As New PA3()

        beVisaCountry.Country.Value = Country
        Try
            Using dt As DataTable = bsVisaCountry.QueryByKey(beVisaCountry).Tables(0)
                If dt.Rows.Count <= 0 Then Exit Sub
                beVisaCountry = New beVisaCountry.Row(dt.Rows(0))

                txtCountry.Text = beVisaCountry.Country.Value
                txtCountryName.Text = beVisaCountry.CountryName.Value
                txtOfficeName.Text = beVisaCountry.OfficeName.Value
                txtAddress.Text = beVisaCountry.Address.Value

                '最後異動公司
                Dim CompName As String = objSC.GetSC_CompName(beVisaCountry.LastChgComp.Value)
                txtLastChgComp.Text = beVisaCountry.LastChgComp.Value + IIf(CompName <> "", "-" + CompName, "")
                '最後異動人員
                Dim UserName As String = objSC.GetSC_UserName(beVisaCountry.LastChgComp.Value, beVisaCountry.LastChgID.Value)
                txtLastChgID.Text = beVisaCountry.LastChgID.Value + IIf(UserName <> "", "-" + UserName, "")
                '最後異動日期
                Dim boolDate As Boolean = Format(beVisaCountry.LastChgDate.Value, "yyyy/MM/dd") = "1900/01/01"
                txtLastChgDate.Text = IIf(boolDate, "", beVisaCountry.LastChgDate.Value.ToString("yyyy/MM/dd HH:mm:ss"))
            End Using
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & "subGetData", ex)
        End Try

    End Sub

    Private Function funCheckData() As Boolean
        Dim strValue As String = ""

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
        subGetData(ViewState.Item("Country"))
    End Sub

End Class
