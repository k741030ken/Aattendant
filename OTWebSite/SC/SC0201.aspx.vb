'****************************************************
'功能說明：系統別(SC_Sys)維護-新增
'建立人員：Weicheng
'建立日期：2014/08/18
'****************************************************
Imports System.Data

Partial Class SC_SC0201
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then
        End If
    End Sub

    Public Overrides Sub DoAction(ByVal Param As String)
        Select Case Param
            Case "btnAdd"
                If funCheckData() Then
                    If SaveData() Then
                        GoBack()
                    End If
                End If
            Case "btnActionX"
                GoBack()
        End Select
    End Sub

    Private Sub GoBack()
        Dim ti As TransferInfo = Me.StateTransfer
        Me.TransferFramePage(ti.CallerUrl, Nothing, ti.Args)
    End Sub

    Private Function SaveData() As Boolean
        Dim beSys As New beSC_Sys.Row()
        Dim bsSys As New beSC_Sys.Service()
        Dim objSC As New SC

        '取得輸入資料
        beSys.SysID.Value = txtSysID.Text.Trim()
        beSys.SysName.Value = txtSysName.Text.Trim()

        beSys.CreateDate.Value = Now
        beSys.LastChgDate.Value = Now
        beSys.LastChgComp.Value = UserProfile.ActCompID
        beSys.LastChgID.Value = UserProfile.ActUserID

        '檢查資料是否存在
        If bsSys.IsDataExists(beSys) Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00010", "")
            Return False
        End If

        '儲存資料
        Try
            Return objSC.AddSys(beSys)
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, "[CC0201_99]" & Bsp.Utility.getMessage("E_00000") & Bsp.Utility.getInnerException(Me.FunID, ex))
            Return False
        End Try
    End Function

    Private Function funCheckData() As Boolean
        Dim strValue As String

        strValue = txtSysID.Text.ToString().Trim()
        If strValue = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", "系統別代碼")
            txtSysID.Focus()
            Return False
        Else
            If Bsp.Utility.getStringLength(strValue) > txtSysID.MaxLength Then
                Bsp.Utility.ShowFormatMessage(Me, "W_00040", "系統別代碼", txtSysID.MaxLength.ToString())
                txtSysID.Focus()
                Return False
            End If
            txtSysID.Text = strValue
        End If

        strValue = txtSysName.Text.ToString().Trim()
        If strValue = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", "系統別代碼")
            txtSysName.Focus()
            Return False
        Else
            If Bsp.Utility.getStringLength(strValue) > txtSysName.MaxLength Then
                Bsp.Utility.ShowFormatMessage(Me, "W_00040", "系統別代碼", txtSysName.MaxLength.ToString())
                txtSysName.Focus()
                Return False
            End If
            txtSysName.Text = strValue
        End If

        Return True
    End Function
End Class
