'****************************************************
'功能說明：其他代碼設定-新增
'建立人員：MickySung
'建立日期：2015.05.25
'****************************************************
Imports System.Data

Partial Class PA_PA5101
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then
            '代碼類別
            PA5.FillFldName_PA5100(ddlFldName)
            ddlFldName.Items.Insert(0, New ListItem("---請選擇---", ""))
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
        Dim beHRCodeMap As New beHRCodeMap.Row()
        Dim bsHRCodeMap As New beHRCodeMap.Service()
        Dim objPA As New PA5()

        '代碼類別
        Dim arrFldName(1) As String
        arrFldName = ddlFldName.SelectedValue.Split("\")
        beHRCodeMap.TabName.Value = arrFldName(0)
        beHRCodeMap.FldName.Value = arrFldName(1)
        '代碼
        beHRCodeMap.Code.Value = txtCode.Text
        '代碼名稱
        beHRCodeMap.CodeCName.Value = txtCodeCName.Text
        '排序順序
        beHRCodeMap.SortFld.Value = txtSortFld.Text
        '不顯示註記
        beHRCodeMap.NotShowFlag.Value = IIf(chkNotShowFlag.Checked, "1", "0")
        beHRCodeMap.LastChgComp.Value = UserProfile.ActCompID
        beHRCodeMap.LastChgID.Value = UserProfile.ActUserID
        beHRCodeMap.LastChgDate.Value = Now

        '檢查資料是否存在
        If bsHRCodeMap.IsDataExists(beHRCodeMap) Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00010", "")
            Return False
        End If

        '儲存資料
        Try
            Return objPA.AddHRCodeMapSetting(beHRCodeMap)
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, "[CC0201_99]" & Bsp.Utility.getMessage("E_00000") & Bsp.Utility.getInnerException(Me.FunID, ex))
            Return False
        End Try
    End Function

    Private Function funCheckData() As Boolean
        Dim objPA As New PA1()
        Dim beHRCodeMap As New beHRCodeMap.Row()
        Dim bsWorkSite As New beHRCodeMap.Service()

        '代碼類別
        If ddlFldName.SelectedValue = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblFldName.Text)
            ddlFldName.Focus()
            Return False
        End If

        '代碼
        If txtCode.Text = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblCode.Text)
            txtCode.Focus()
            Return False
        End If
        If Bsp.Utility.getStringLength(txtCode.Text.Trim) > txtCode.MaxLength Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00040", lblCode.Text, txtCode.MaxLength.ToString)
            txtCode.Focus()
            Return False
        End If

        '代碼名稱
        If txtCodeCName.Text = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblCodeCName.Text)
            txtCodeCName.Focus()
            Return False
        End If

        '排序順序
        If txtSortFld.Text = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblSortFld.Text)
            txtSortFld.Focus()
            Return False
        Else
            If IsNumeric(txtSortFld.Text.Trim) = False Then
                Bsp.Utility.ShowMessage(Me, "｢排序順序｣請輸入0~255之數字")
                txtSortFld.Focus()
                Return False
            Else
                If CInt(txtSortFld.Text.Trim) < 0 Or CInt(txtSortFld.Text.Trim) > 255 Then
                    Bsp.Utility.ShowMessage(Me, "｢排序順序｣請輸入0~255之數字")
                    txtSortFld.Focus()
                    Return False
                End If
            End If
        End If

        Return True
    End Function

    Private Sub ClearData()
        ddlFldName.SelectedValue = ""
        txtCode.Text = ""
        txtCodeCName.Text = ""
        lblSortFld.Text = ""
        chkNotShowFlag.Checked = False
    End Sub

End Class
