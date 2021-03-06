'****************************************************
'功能說明：工作性質設定_大類-新增
'建立人員：MickySung
'建立日期：2015.05.07
'****************************************************
Imports System.Data

Partial Class PA_PA1711
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
        Dim beWorkType_CategoryI As New beWorkType_CategoryI.Row()
        Dim bsWorkType_CategoryI As New beWorkType_CategoryI.Service()
        Dim objPA As New PA1()

        '取得輸入資料
        beWorkType_CategoryI.CategoryI.Value = txtCategoryI.Text
        beWorkType_CategoryI.CategoryIName.Value = txtCategoryIName.Text
        beWorkType_CategoryI.LastChgComp.Value = UserProfile.ActCompID
        beWorkType_CategoryI.LastChgID.Value = UserProfile.ActUserID
        beWorkType_CategoryI.LastChgDate.Value = Now

        '檢查資料是否存在
        If bsWorkType_CategoryI.IsDataExists(beWorkType_CategoryI) Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00010", "")
            Return False
        End If

        '儲存資料
        Try
            Return objPA.AddWorkType_CategoryI(beWorkType_CategoryI)
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, "[CC0201_99]" & Bsp.Utility.getMessage("E_00000") & Bsp.Utility.getInnerException(Me.FunID, ex))
            Return False
        End Try
    End Function

    Private Function funCheckData() As Boolean
        Dim objPA As New PA1()
        Dim beWorkType_CategoryI As New beWorkType_CategoryI.Row()
        Dim bsWorkType_CategoryI As New beWorkType_CategoryI.Service()

        '大類代碼
        If txtCategoryI.Text = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblCategoryI.Text)
            txtCategoryI.Focus()
            Return False
        End If
        If bsWorkType_CategoryI.IsDataExists(beWorkType_CategoryI) Then
            Bsp.Utility.ShowFormatMessage(Me, "大類代碼：「" + txtCategoryI.Text + "」資料已存在，不可新增！", "")
            txtCategoryI.Focus()
            Return False
        End If
        If Bsp.Utility.getStringLength(txtCategoryI.Text.Trim) > txtCategoryI.MaxLength Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00040", lblCategoryI.Text, txtCategoryI.MaxLength.ToString)
            txtCategoryI.Focus()
            Return False
        End If

        '大類名稱
        If txtCategoryIName.Text = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblCategoryIName.Text)
            txtCategoryIName.Focus()
            Return False
        End If
        'If Bsp.Utility.getStringLength(txtCategoryIName.Text.Trim) > txtCategoryIName.MaxLength Then
        '    Bsp.Utility.ShowFormatMessage(Me, "W_00040", lblCategoryIName.Text, txtCategoryIName.MaxLength.ToString)
        '    txtCategoryIName.Focus()
        '    Return False
        'End If

        Return True
    End Function

    Private Sub ClearData()
        '大類代碼
        txtCategoryI.Text = ""

        '大類名稱
        txtCategoryIName.Text = ""
    End Sub

End Class
