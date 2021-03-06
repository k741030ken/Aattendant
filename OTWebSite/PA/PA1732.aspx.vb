'****************************************************
'功能說明：工作性質設定_細類-修改
'建立人員：MickySung
'建立日期：2015.05.08
'****************************************************
Imports System.Data

Partial Class PA_PA1732
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then
            
        End If
    End Sub

    Protected Overrides Sub BaseOnPageTransfer(ByVal ti As TransferInfo)
        If Not IsPostBack() Then
            Dim ht As Hashtable = Bsp.Utility.getHashTableFromParam(ti.Args)

            If ht.ContainsKey("SelectedCategoryII") Then
                ViewState.Item("CategoryII") = ht("SelectedCategoryII").ToString()
                subGetData(ht("SelectedCategoryI").ToString(), ht("SelectedCategoryII").ToString(), ht("SelectedCategoryIII").ToString(), ht("SelectedCategoryIName").ToString(), ht("SelectedCategoryIIName").ToString())
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
        Dim beWorkType_CategoryIII As New beWorkType_CategoryIII.Row()
        Dim bsWorkType_CategoryIII As New beWorkType_CategoryIII.Service()
        Dim objPA As New PA1()

        '取得輸入資料
        beWorkType_CategoryIII.CategoryI.Value = lbltxtCategoryI.Text
        beWorkType_CategoryIII.CategoryII.Value = lbltxtCategoryII.Text
        beWorkType_CategoryIII.CategoryIII.Value = lbltxtCategoryIII.Text
        beWorkType_CategoryIII.CategoryIIIName.Value = txtCategoryIIIName.Text
        beWorkType_CategoryIII.LastChgComp.Value = UserProfile.ActCompID
        beWorkType_CategoryIII.LastChgID.Value = UserProfile.ActUserID
        beWorkType_CategoryIII.LastChgDate.Value = Now
        
        '儲存資料
        Try
            Return objPA.UpdateWorkType_CategoryII(beWorkType_CategoryIII)
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, "[CC0201_99]" & Bsp.Utility.getMessage("E_00000") & Bsp.Utility.getInnerException(Me.FunID, ex))
            Return False
        End Try
    End Function

    Private Sub subGetData(ByVal CategoryI As String, ByVal CategoryII As String, ByVal CategoryIII As String, ByVal CategoryIName As String, ByVal CategoryIIName As String)
        Dim objPA As New PA1()
        Dim objSC As New SC()
        Dim beWorkType_CategoryIII As New beWorkType_CategoryIII.Row()
        Dim bsWorkType_CategoryIII As New beWorkType_CategoryIII.Service()

        beWorkType_CategoryIII.CategoryI.Value = CategoryI
        beWorkType_CategoryIII.CategoryII.Value = CategoryII
        beWorkType_CategoryIII.CategoryIII.Value = CategoryIII
        Try
            Using dt As DataTable = bsWorkType_CategoryIII.QueryByKey(beWorkType_CategoryIII).Tables(0)
                If dt.Rows.Count <= 0 Then Exit Sub
                beWorkType_CategoryIII = New beWorkType_CategoryIII.Row(dt.Rows(0))

                '大類代碼+名稱
                lbltxtCategoryI.Text = beWorkType_CategoryIII.CategoryI.Value
                lbltxtCategoryIName.Text = CategoryIName
                '中類代碼+名稱
                lbltxtCategoryII.Text = beWorkType_CategoryIII.CategoryII.Value
                lbltxtCategoryIIName.Text = CategoryIIName
                '細類代碼
                lbltxtCategoryIII.Text = beWorkType_CategoryIII.CategoryIII.Value
                '細類名稱
                txtCategoryIIIName.Text = beWorkType_CategoryIII.CategoryIIIName.Value
                '最後異動公司
                If beWorkType_CategoryIII.LastChgComp.Value.Trim <> "" Then
                    lblLastChgComp.Text = beWorkType_CategoryIII.LastChgComp.Value + "-" + objSC.GetCompName(beWorkType_CategoryIII.LastChgComp.Value).Rows(0).Item("CompName").ToString
                Else
                    lblLastChgComp.Text = ""
                End If
                '最後異動人員
                If beWorkType_CategoryIII.LastChgID.Value.Trim <> "" Then
                    Dim UserName As String = objSC.GetSC_UserName(beWorkType_CategoryIII.LastChgComp.Value, beWorkType_CategoryIII.LastChgID.Value)
                    lblLastChgID.Text = beWorkType_CategoryIII.LastChgID.Value + IIf(UserName <> "", "-" + UserName, "")
                Else
                    lblLastChgID.Text = ""
                End If
                '最後異動日期
                lblLastChgDate.Text = IIf(Format(beWorkType_CategoryIII.LastChgDate.Value, "yyyy/MM/dd") = "1900/01/01", "", beWorkType_CategoryIII.LastChgDate.Value.ToString("yyyy/MM/dd HH:mm:ss"))

            End Using
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & "subGetData", ex)
        End Try
    End Sub

    Private Function funCheckData() As Boolean
        Dim objPA As New PA1()
        Dim beWorkType_CategoryIII As New beWorkType_CategoryIII.Row()
        Dim bsWorkType_CategoryIII As New beWorkType_CategoryIII.Service()

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
        subGetData(lbltxtCategoryI.Text, lbltxtCategoryII.Text, lbltxtCategoryIII.Text, lbltxtCategoryIName.Text, lbltxtCategoryIIName.Text)
    End Sub
    
End Class
