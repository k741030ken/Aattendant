'****************************************************
'功能說明：代理人維護-修改
'建立人員：Chung
'建立日期：2013/01/28
'****************************************************
Imports System.Data

Partial Class SC_SC02A2
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Page.SetFocus(ddlAgencyType)
        End If
    End Sub

    Protected Overrides Sub BaseOnPageTransfer(ByVal ti As TransferInfo)
        If Not IsPostBack() Then
            GetData(ti.Args(0))
        End If
    End Sub

    Public Overrides Sub DoAction(ByVal Param As String)
        Select Case Param
            Case "btnUpdate"
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

    Private Sub GetData(ByVal strAgentUserID As String)
        Dim objSC As New SC

        Try
            Using dt As DataTable = objSC.GetAgency(UserProfile.UserID, strAgentUserID, "*, dbo.funGetAOrgDefine('3', AgentUserID) AgentUserName, dbo.funGetAOrgDefine('3', UserID) UserName")
                If dt.Rows.Count = 0 Then
                    Bsp.Utility.ShowFormatMessage(Me, "W_00020", "代理人")
                    Return
                End If
                Dim beAgency As New beSC_Agency.Row(dt.Rows(0))

                With beAgency
                    lblAgentUserName.Text = .AgentUserID.Value & "-" & dt.Rows(0).Item("AgentUserName").ToString()
                    ddlAgencyType.SelectedValue = .AgencyType.Value
                    txtValidFrom.Text = .ValidFrom.Value.ToString("yyyy/MM/dd")
                    txtValidTo.Text = .ValidTo.Value.ToString("yyyy/MM/dd")
                    chkValidFlag.Checked = IIf(.ValidFlag.Value = "1", True, False)
                    lblCreateDate.Text = .CreateDate.Value.ToString("yyyy/MM/dd HH:mm:ss")
                    lblLastChgDate.Text = .LastChgDate.Value.ToString("yyyy/MM/dd HH:mm:ss")
                    lblLastChgID.Text = .LastChgID.Value
                End With
            End Using
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & ".GetData", ex)
        End Try
    End Sub

    Private Function SaveData() As Boolean
        Dim beAgency As New beSC_Agency.Row()
        Dim objSC As New SC

        With beAgency
            .UserID.Value = UserProfile.UserID
            .AgentUserID.Value = lblAgentUserName.Text.Split("-")(0)
            .AgencyType.Value = ddlAgencyType.SelectedValue
            .ValidFlag.Value = IIf(chkValidFlag.Checked, "1", "0")
            .ValidFrom.Value = CDate(txtValidFrom.Text)
            .ValidTo.Value = Convert.ToDateTime(txtValidTo.Text & " 23:59:59")
            .LastChgDate.Value = Now
            .LastChgID.Value = UserProfile.ActUserID
        End With

        If Not objSC.IsAgencyExists(beAgency) Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00020", "代理人")
            Return False
        End If

        Try
            objSC.UpdateAgency(beAgency)
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & ".SaveData", ex)
            Return False
        End Try

        Return True
    End Function

    Private Function funCheckData() As Boolean
        Dim strValue As String

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
