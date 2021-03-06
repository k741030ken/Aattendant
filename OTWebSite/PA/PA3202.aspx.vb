'****************************************************
'功能說明：任職狀況設定-修改
'建立人員：BeatriceCheng
'建立日期：2015.05.04
'****************************************************
Imports System.Data

Partial Class PA_PA3202
    Inherits PageBase


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then

        End If
    End Sub

    Protected Overrides Sub BaseOnPageTransfer(ByVal ti As TransferInfo)
        If Not IsPostBack() Then
            Dim ht As Hashtable = Bsp.Utility.getHashTableFromParam(ti.Args)

            If ht.ContainsKey("SelectedWorkCode") Then
                ViewState.Item("WorkCode") = ht("SelectedWorkCode").ToString()
                subGetData(ht("SelectedWorkCode").ToString())
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
        Dim beWorkStatus As New beWorkStatus.Row()
        Dim bsWorkStatus As New beWorkStatus.Service()
        Dim objPA3 As New PA3()

        '取得輸入資料
        beWorkStatus.WorkCode.Value = txtWorkCode.Text
        beWorkStatus.Remark.Value = txtRemark.Text
        beWorkStatus.RemarkCN.Value = txtRemarkCN.Text

        beWorkStatus.LastChgComp.Value = UserProfile.ActCompID
        beWorkStatus.LastChgID.Value = UserProfile.ActUserID
        beWorkStatus.LastChgDate.Value = Now

        '儲存資料
        Try
            Return objPA3.WorkStatusUpdate(beWorkStatus)
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, "[CC0201_99]" & Bsp.Utility.getMessage("E_00000") & Bsp.Utility.getInnerException(Me.FunID, ex))
            Return False
        End Try
    End Function

    Private Sub subGetData(ByVal WorkCode As String)
        Dim objSC As New SC
        Dim beWorkStatus As New beWorkStatus.Row()
        Dim bsWorkStatus As New beWorkStatus.Service()
        Dim objPA3 As New PA3()

        beWorkStatus.WorkCode.Value = WorkCode
        Try
            Using dt As DataTable = bsWorkStatus.QueryByKey(beWorkStatus).Tables(0)
                If dt.Rows.Count <= 0 Then Exit Sub
                beWorkStatus = New beWorkStatus.Row(dt.Rows(0))

                txtWorkCode.Text = beWorkStatus.WorkCode.Value
                txtRemark.Text = beWorkStatus.Remark.Value
                txtRemarkCN.Text = beWorkStatus.RemarkCN.Value
                '最後異動公司
                Dim CompName As String = objSC.GetSC_CompName(beWorkStatus.LastChgComp.Value)
                txtLastChgComp.Text = beWorkStatus.LastChgComp.Value + IIf(CompName <> "", "-" + CompName, "")
                '最後異動人員
                Dim UserName As String = objSC.GetSC_UserName(beWorkStatus.LastChgComp.Value, beWorkStatus.LastChgID.Value)
                txtLastChgID.Text = beWorkStatus.LastChgID.Value + IIf(UserName <> "", "-" + UserName, "")
                '最後異動日期
                Dim boolDate As Boolean = Format(beWorkStatus.LastChgDate.Value, "yyyy/MM/dd") = "1900/01/01"
                txtLastChgDate.Text = IIf(boolDate, "", beWorkStatus.LastChgDate.Value.ToString("yyyy/MM/dd HH:mm:ss"))
            End Using
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & "subGetData", ex)
        End Try

    End Sub

    Private Function funCheckData() As Boolean
        Dim strValue As String = ""

        '任職名稱(繁)
        strValue = txtRemark.Text
        If strValue.Trim = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblRemark.Text)
            txtRemark.Focus()
            Return False
        End If
        'If Bsp.Utility.getStringLength(strValue) > txtRemark.MaxLength Then
        '    Bsp.Utility.ShowFormatMessage(Me, "W_00040", lblRemark.Text, txtRemark.MaxLength.ToString())
        '    txtRemark.Focus()
        '    Return False
        'End If

        '任職名稱(簡)
        strValue = txtRemarkCN.Text
        If strValue.Trim = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblRemarkCN.Text)
            txtRemarkCN.Focus()
            Return False
        End If
        'If Bsp.Utility.getStringLength(strValue) > txtRemarkCN.MaxLength Then
        '    Bsp.Utility.ShowFormatMessage(Me, "W_00040", lblRemarkCN.Text, txtRemarkCN.MaxLength.ToString())
        '    txtRemarkCN.Focus()
        '    Return False
        'End If

        Return True
    End Function

    Private Sub ClearData()
        subGetData(ViewState.Item("WorkCode"))
    End Sub

End Class
