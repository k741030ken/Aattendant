'****************************************************
'功能說明：員工持股信託特例人員處理-修改
'建立人員：Micky Sung
'建立日期：2015.08.13
'****************************************************
Imports System.Data

Partial Class OT_OT1142
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then
            '公司代碼
            txtCompID.Text = UserProfile.SelectCompRoleName
            hidCompID.Value = UserProfile.SelectCompRoleID

        End If
    End Sub

    Protected Overrides Sub BaseOnPageTransfer(ByVal ti As TransferInfo)
        If Not IsPostBack() Then
            Dim ht As Hashtable = Bsp.Utility.getHashTableFromParam(ti.Args)

            '公司代碼
            If ht.ContainsKey("SelectedCompID") Then
                ViewState.Item("CompID") = ht("SelectedCompID").ToString()
                ViewState.Item("EmpID") = ht("SelectedEmpID").ToString()
                ViewState.Item("NameN") = ht("SelectedNameN").ToString()

                txtCompID.Text = UserProfile.SelectCompRoleName
                hidCompID.Value = UserProfile.SelectCompRoleID
                lblEmpName.Text = ViewState.Item("EmpID") + "-" + ViewState.Item("NameN")
                hidEmpID.Value = ViewState.Item("EmpID")

                subGetData(ht("SelectedCompID").ToString(), ht("SelectedEmpID").ToString())
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
        Dim objOT As New OT1()
        Dim beEmpStockTrustExLevel As New beEmpStockTrustExLevel.Row()
        Dim bsEmpStockTrustExLevel As New beEmpStockTrustExLevel.Service()

        beEmpStockTrustExLevel.CompID.Value = hidCompID.Value
        beEmpStockTrustExLevel.EmpID.Value = hidEmpID.Value
        beEmpStockTrustExLevel.UAmount.Value = CInt(txtUAmount.Text)
        beEmpStockTrustExLevel.LastChgComp.Value = UserProfile.ActCompID
        beEmpStockTrustExLevel.LastChgID.Value = UserProfile.ActUserID
        beEmpStockTrustExLevel.LastChgDate.Value = Now

        '儲存資料
        Try
            Return objOT.UpdateEmpStockTrustExLevelSetting(beEmpStockTrustExLevel)
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, "[CC0201_99]" & Bsp.Utility.getMessage("E_00000") & Bsp.Utility.getInnerException(Me.FunID, ex))
            Return False
        End Try
    End Function

    Private Sub subGetData(ByVal CompID As String, ByVal EmpID As String)
        Dim objSC As New SC
        Dim beEmpStockTrustExLevel As New beEmpStockTrustExLevel.Row()
        Dim bsEmpStockTrustExLevel As New beEmpStockTrustExLevel.Service()

        beEmpStockTrustExLevel.CompID.Value = CompID
        beEmpStockTrustExLevel.EmpID.Value = EmpID

        Try
            Using dt As DataTable = bsEmpStockTrustExLevel.QueryByKey(beEmpStockTrustExLevel).Tables(0)
                If dt.Rows.Count <= 0 Then Exit Sub
                beEmpStockTrustExLevel = New beEmpStockTrustExLevel.Row(dt.Rows(0))

                txtUAmount.Text = beEmpStockTrustExLevel.UAmount.Value

                '最後異動公司
                If beEmpStockTrustExLevel.LastChgComp.Value.Trim <> "" Then
                    lblLastChgComp.Text = beEmpStockTrustExLevel.LastChgComp.Value + "-" + objSC.GetCompName(beEmpStockTrustExLevel.LastChgComp.Value).Rows(0).Item("CompName").ToString
                Else
                    lblLastChgComp.Text = ""
                End If
                '最後異動人員
                If beEmpStockTrustExLevel.LastChgID.Value.Trim <> "" Then
                    Dim UserName As String = objSC.GetSC_UserName(beEmpStockTrustExLevel.LastChgComp.Value, beEmpStockTrustExLevel.LastChgID.Value)
                    lblLastChgID.Text = beEmpStockTrustExLevel.LastChgID.Value + IIf(UserName <> "", "-" + UserName, "")
                Else
                    lblLastChgID.Text = ""
                End If
                '最後異動日期
                lblLastChgDate.Text = IIf(Format(beEmpStockTrustExLevel.LastChgDate.Value, "yyyy/MM/dd") = "1900/01/01", "", beEmpStockTrustExLevel.LastChgDate.Value.ToString("yyyy/MM/dd HH:mm:ss"))

            End Using
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & "subGetData", ex)
        End Try
    End Sub

    Private Function funCheckData() As Boolean
        Dim objOT As New OT1()
        Dim beEmpStockTrustExLevel As New beEmpStockTrustExLevel.Row()
        Dim bsEmpStockTrustExLevel As New beEmpStockTrustExLevel.Service()

        '上限金額
        If txtUAmount.Text.Trim = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblUAmount.Text)
            txtUAmount.Focus()
            Return False
        Else
            If IsNumeric(txtUAmount.Text.Trim) And InStr(1, txtUAmount.Text.Trim, ".") = 0 Then
                '2015/08/20 Modify 修改數字檢核範圍、錯誤訊息
                If txtUAmount.Text.Trim < 0 Or txtUAmount.Text.Trim > 24000 Then
                    Bsp.Utility.ShowMessage(Me, "欄位[上限金額]不可小於0或大於24000")
                    txtUAmount.Focus()
                    Return False
                End If
            Else
                Bsp.Utility.ShowMessage(Me, "欄位[上限金額]請輸入數字")
                txtUAmount.Focus()
                Return False
            End If
        End If

        Return True
    End Function

    Private Sub ClearData()
        subGetData(hidCompID.Value, hidEmpID.Value)
    End Sub

End Class
