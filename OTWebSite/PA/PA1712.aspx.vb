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
            Dim ht As Hashtable = Bsp.Utility.getHashTableFromParam(ti.Args)

            If ht.ContainsKey("SelectedCategoryI") Then
                ViewState.Item("CategoryI") = ht("SelectedCategoryI").ToString()
                subGetData(ht("SelectedCategoryI").ToString())
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
        Dim beWorkType_CategoryI As New beWorkType_CategoryI.Row()
        Dim bsWorkType_CategoryI As New beWorkType_CategoryI.Service()
        Dim objPA As New PA1()

        '取得輸入資料
        beWorkType_CategoryI.CategoryI.Value = lbltxtCategoryI.Text
        beWorkType_CategoryI.CategoryIName.Value = txtCategoryIName.Text
        beWorkType_CategoryI.LastChgComp.Value = UserProfile.ActCompID
        beWorkType_CategoryI.LastChgID.Value = UserProfile.ActUserID
        beWorkType_CategoryI.LastChgDate.Value = Now

        '儲存資料
        Try
            Return objPA.UpdateWorkType_CategoryI(beWorkType_CategoryI)
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, "[CC0201_99]" & Bsp.Utility.getMessage("E_00000") & Bsp.Utility.getInnerException(Me.FunID, ex))
            Return False
        End Try
    End Function

    Private Sub subGetData(ByVal CategoryI As String)
        Dim objPA As New PA1()
        Dim objSC As New SC()
        Dim beWorkType_CategoryI As New beWorkType_CategoryI.Row()
        Dim bsWorkType_CategoryI As New beWorkType_CategoryI.Service()

        beWorkType_CategoryI.CategoryI.Value = CategoryI
        Try
            Using dt As DataTable = bsWorkType_CategoryI.QueryByKey(beWorkType_CategoryI).Tables(0)
                If dt.Rows.Count <= 0 Then Exit Sub
                beWorkType_CategoryI = New beWorkType_CategoryI.Row(dt.Rows(0))

                '大類代碼
                lbltxtCategoryI.Text = beWorkType_CategoryI.CategoryI.Value
                '大類名稱
                txtCategoryIName.Text = beWorkType_CategoryI.CategoryIName.Value
                '最後異動公司
                If beWorkType_CategoryI.LastChgComp.Value.Trim <> "" Then
                    lblLastChgComp.Text = beWorkType_CategoryI.LastChgComp.Value + "-" + objSC.GetCompName(beWorkType_CategoryI.LastChgComp.Value).Rows(0).Item("CompName").ToString
                Else
                    lblLastChgComp.Text = ""
                End If
                '最後異動人員
                If beWorkType_CategoryI.LastChgID.Value.Trim <> "" Then
                    Dim UserName As String = objSC.GetSC_UserName(beWorkType_CategoryI.LastChgComp.Value, beWorkType_CategoryI.LastChgID.Value)
                    lblLastChgID.Text = beWorkType_CategoryI.LastChgID.Value + IIf(UserName <> "", "-" + UserName, "")
                Else
                    lblLastChgID.Text = ""
                End If
                '最後異動日期
                lblLastChgDate.Text = IIf(Format(beWorkType_CategoryI.LastChgDate.Value, "yyyy/MM/dd") = "1900/01/01", "", beWorkType_CategoryI.LastChgDate.Value.ToString("yyyy/MM/dd HH:mm:ss"))

            End Using
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & "subGetData", ex)
        End Try
    End Sub

    Private Function funCheckData() As Boolean
        Dim objPA As New PA1()
        Dim beWorkType_CategoryI As New beWorkType_CategoryI.Row()
        Dim bsWorkType_CategoryI As New beWorkType_CategoryI.Service()

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
        subGetData(lbltxtCategoryI.Text)
    End Sub

End Class
