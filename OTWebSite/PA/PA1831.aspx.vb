'****************************************************
'功能說明：職位設定_細類-新增
'建立人員：MickySung
'建立日期：2015.05.14
'****************************************************
Imports System.Data

Partial Class PA_PA1831
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then
            '大類
            PA1.FillCategoryI_PA1800(ddlCategoryI)
            ddlCategoryI.Items.Insert(0, New ListItem("---請選擇---", ""))

            '中類
            PA1.FillCategoryII_PA1800(ddlCategoryII, ddlCategoryI.SelectedValue)
            ddlCategoryII.Items.Insert(0, New ListItem("---請選擇---", ""))
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
        Dim bePosition_CategoryIII As New bePosition_CategoryIII.Row()
        Dim bsPosition_CategoryIII As New bePosition_CategoryIII.Service()
        Dim objPA As New PA1()

        '取得輸入資料
        bePosition_CategoryIII.CategoryI.Value = ddlCategoryI.SelectedValue
        bePosition_CategoryIII.CategoryII.Value = ddlCategoryII.SelectedValue
        bePosition_CategoryIII.CategoryIII.Value = txtCategoryIII.Text
        bePosition_CategoryIII.CategoryIIIName.Value = txtCategoryIIIName.Text
        bePosition_CategoryIII.LastChgComp.Value = UserProfile.ActCompID
        bePosition_CategoryIII.LastChgID.Value = UserProfile.ActUserID
        bePosition_CategoryIII.LastChgDate.Value = Now

        '檢查資料是否存在
        If bsPosition_CategoryIII.IsDataExists(bePosition_CategoryIII) Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00010", "")
            Return False
        End If

        '儲存資料
        Try
            Return objPA.AddPosition_CategoryIII(bePosition_CategoryIII)
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, "[CC0201_99]" & Bsp.Utility.getMessage("E_00000") & Bsp.Utility.getInnerException(Me.FunID, ex))
            Return False
        End Try
    End Function

    Private Function funCheckData() As Boolean
        Dim objPA As New PA1()
        Dim bePosition_CategoryIII As New bePosition_CategoryIII.Row()
        Dim bsPosition_CategoryIII As New bePosition_CategoryIII.Service()

        '大類代碼
        If ddlCategoryI.SelectedValue = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblCategoryI.Text)
            ddlCategoryI.Focus()
            Return False
        End If

        '中類代碼
        If ddlCategoryII.SelectedValue = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblCategoryII.Text)
            ddlCategoryII.Focus()
            Return False
        End If

        '細類代碼
        If txtCategoryIII.Text = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblCategoryIII.Text)
            txtCategoryIII.Focus()
            Return False
        End If
        If bsPosition_CategoryIII.IsDataExists(bePosition_CategoryIII) Then
            Bsp.Utility.ShowFormatMessage(Me, "H_00000", "細類代碼：「" + txtCategoryIII.Text + "」資料已存在，不可新增！")
            txtCategoryIII.Focus()
            Return False
        End If
        If Bsp.Utility.getStringLength(txtCategoryIII.Text.Trim) > txtCategoryIII.MaxLength Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00040", lblCategoryII.Text, txtCategoryIII.MaxLength.ToString)
            txtCategoryIII.Focus()
            Return False
        End If

        '細類名稱
        If txtCategoryIIIName.Text = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblCategoryIIIName.Text)
            txtCategoryIIIName.Focus()
            Return False
        End If
        'If Bsp.Utility.getStringLength(txtCategoryIIIName.Text.Trim) > txtCategoryIIIName.MaxLength Then
        '    Bsp.Utility.ShowFormatMessage(Me, "W_00040", lblCategoryIIIName.Text, txtCategoryIIIName.MaxLength.ToString)
        '    txtCategoryIIIName.Focus()
        '    Return False
        'End If

        Return True
    End Function

    Private Sub ClearData()
        '大類代碼
        ddlCategoryI.SelectedValue = ""

        '中類代碼
        ddlCategoryII.SelectedValue = ""

        '細類代碼
        txtCategoryIII.Text = ""

        '細類名稱
        txtCategoryIIIName.Text = ""
    End Sub

    Protected Sub ddlCategoryI_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlCategoryI.SelectedIndexChanged
        PA1.FillCategoryII_PA1800(ddlCategoryII, ddlCategoryI.SelectedItem.Value)
        ddlCategoryII.Items.Insert(0, New ListItem("---請選擇---", ""))
        UpdCategoryII.Update()
    End Sub

End Class
