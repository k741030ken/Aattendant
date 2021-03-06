'****************************************************
'功能說明：代理人維護-新增
'建立人員：Chung
'建立日期：2013/01/28
'****************************************************
Imports System.Data

Partial Class SC_SC02C1
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Page.SetFocus(ucAgentUserID)
        End If
    End Sub

    Protected Overrides Sub BaseOnPageTransfer(ByVal ti As TransferInfo)
        If Not IsPostBack Then
            If ti.Args.Length > 0 Then
                ucUserID.setDeptID(ti.Args(0))
                ucUserID.setUserID(ti.Args(1))
                ucAgentUserID.setDeptID(ti.Args(0))
            End If
        End If
    End Sub

    Public Overrides Sub DoAction(ByVal Param As String)
        Select Case Param
            Case "btnAdd"
                If funCheckData() Then
                    If SaveData() Then GoBack()
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
        Dim beAgency As New beSC_Agency.Row()
        Dim objSC As New SC

        With beAgency
            .UserID.Value = ucUserID.SelectedUserID
            .AgentUserID.Value = ucAgentUserID.SelectedUserID
            .AgencyType.Value = ddlAgencyType.SelectedValue
            .ValidFlag.Value = IIf(chkValidFlag.Checked, "1", "0")
            .ValidFrom.Value = CDate(txtValidFrom.Text)
            .ValidTo.Value = CDate(txtValidTo.Text & " 23:59:59")
            .CreateDate.Value = Now
            .LastChgDate.Value = Now
            .LastChgID.Value = UserProfile.ActUserID
        End With

        If objSC.IsAgencyExists(beAgency) Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00010", "代理人")
            Return False
        End If

        Try
            objSC.AddAgency(beAgency)
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & ".SaveData", ex)
            Return False
        End Try

        Return True
    End Function

    Private Function funCheckData() As Boolean
        Dim strValue As String

        If ucUserID.SelectedUserID = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00035", "員工")
            Return False
        End If

        If ucAgentUserID.SelectedUserID = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00035", "代理人")
            Return False
        End If

        strValue = txtValidFrom.Text.ToString()
        If strValue = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", "有效起日")
            txtValidFrom.Focus()
            Return False
        Else
            If Bsp.Utility.getStringLength(strValue) > txtValidFrom.MaxLength Then
                Bsp.Utility.ShowFormatMessage(Me, "W_00040", "有效起日", txtValidFrom.MaxLength.ToString())
                txtValidFrom.Focus()
                Return False
            End If
            strValue = Bsp.Utility.CheckDate(strValue)
            If strValue = "" Then
                Bsp.Utility.ShowFormatMessage(Me, "W_00060", "有效起日")
                txtValidFrom.Focus()
                Return False
            End If
            txtValidFrom.Text = strValue
        End If

        strValue = txtValidTo.Text.ToString()
        If strValue = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", "有效迄日")
            txtValidTo.Focus()
            Return False
        Else
            If Bsp.Utility.getStringLength(strValue) > txtValidTo.MaxLength Then
                Bsp.Utility.ShowFormatMessage(Me, "W_00040", "有效迄日", txtValidTo.MaxLength.ToString())
                txtValidTo.Focus()
                Return False
            End If
            strValue = Bsp.Utility.CheckDate(strValue)
            If strValue = "" Then
                Bsp.Utility.ShowFormatMessage(Me, "W_00060", "有效迄日")
                txtValidTo.Focus()
                Return False
            End If
            txtValidTo.Text = strValue
        End If

        If txtValidTo.Text < txtValidFrom.Text Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00130")
            txtValidFrom.Focus()
            Return False
        End If

        Return True
    End Function
End Class
