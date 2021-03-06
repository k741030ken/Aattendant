'****************************************************
'功能說明：職位設定_中類-新增
'建立人員：MickySung
'建立日期：2015.05.13
'****************************************************
Imports System.Data

Partial Class PA_PA1821
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then
            '大類
            PA1.FillCategoryI_PA1800(ddlCategoryI)
            ddlCategoryI.Items.Insert(0, New ListItem("---請選擇---", ""))
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
        Dim bePosition_CategoryII As New bePosition_CategoryII.Row()
        Dim bsPosition_CategoryII As New bePosition_CategoryII.Service()
        Dim objPA As New PA1()

        '取得輸入資料
        bePosition_CategoryII.CategoryI.Value = ddlCategoryI.SelectedValue
        bePosition_CategoryII.CategoryII.Value = txtCategoryII.Text
        bePosition_CategoryII.CategoryIIName.Value = txtCategoryIIName.Text
        bePosition_CategoryII.LastChgComp.Value = UserProfile.ActCompID
        bePosition_CategoryII.LastChgID.Value = UserProfile.ActUserID
        bePosition_CategoryII.LastChgDate.Value = Now

        '檢查資料是否存在
        If bsPosition_CategoryII.IsDataExists(bePosition_CategoryII) Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00010", "")
            Return False
        End If

        '儲存資料
        Try
            Return objPA.AddPosition_CategoryII(bePosition_CategoryII)
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, "[CC0201_99]" & Bsp.Utility.getMessage("E_00000") & Bsp.Utility.getInnerException(Me.FunID, ex))
            Return False
        End Try
    End Function

    Private Function funCheckData() As Boolean
        Dim objPA As New PA1()
        Dim bePosition_CategoryII As New bePosition_CategoryII.Row()
        Dim bsPosition_CategoryII As New bePosition_CategoryII.Service()

        '大類代碼
        If ddlCategoryI.SelectedValue = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblCategoryI.Text)
            ddlCategoryI.Focus()
            Return False
        End If

        '中類代碼
        If txtCategoryII.Text = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblCategoryII.Text)
            txtCategoryII.Focus()
            Return False
        End If
        If bsPosition_CategoryII.IsDataExists(bePosition_CategoryII) Then
            Bsp.Utility.ShowFormatMessage(Me, "H_00000", "中類代碼：「" + txtCategoryII.Text + "」資料已存在，不可新增！")
            txtCategoryII.Focus()
            Return False
        End If
        If Bsp.Utility.getStringLength(txtCategoryII.Text.Trim) > txtCategoryII.MaxLength Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00040", lblCategoryII.Text, txtCategoryII.MaxLength.ToString)
            txtCategoryII.Focus()
            Return False
        End If

        '中類名稱
        If txtCategoryIIName.Text = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblCategoryIIName.Text)
            txtCategoryIIName.Focus()
            Return False
        End If
        'If Bsp.Utility.getStringLength(txtCategoryIIName.Text.Trim) > txtCategoryIIName.MaxLength Then
        '    Bsp.Utility.ShowFormatMessage(Me, "W_00040", lblCategoryIIName.Text, txtCategoryIIName.MaxLength.ToString)
        '    txtCategoryIIName.Focus()
        '    Return False
        'End If

        Return True
    End Function

    Private Sub ClearData()
        '大類代碼
        ddlCategoryI.SelectedValue = ""

        '中類代碼
        txtCategoryII.Text = ""

        '中類名稱
        txtCategoryIIName.Text = ""
    End Sub

End Class
