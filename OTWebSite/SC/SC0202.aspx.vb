'****************************************************
'功能說明：系統別(SC_Sys)維護-修改
'建立人員：Weicheng
'建立日期：2014/08/18
'****************************************************
Imports System.Data

Partial Class SC_SC0202
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    End Sub

    Protected Overrides Sub BaseOnPageTransfer(ByVal ti As TransferInfo)
        If Not IsPostBack() Then
            Dim ht As Hashtable = Bsp.Utility.getHashTableFromParam(ti.Args)

            If ht.ContainsKey("SelectedSysID") Then
                ViewState.Item("SysID") = ht("SelectedSysID").ToString()
            Else
                Return
            End If
            GetData(ViewState.Item("SysID").ToString())
        End If
    End Sub

    Public Overrides Sub DoAction(ByVal Param As String)
        Select Case Param
            Case "btnUpdate"
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

    Private Sub GetData(ByVal SysID As String)
        Dim objSC As New SC
        Dim bsSys As New beSC_Sys.Service()
        Dim beSys As New beSC_Sys.Row()

        beSys.SysID.Value = SysID
        Try
            Using dt As DataTable = bsSys.QueryByKey(beSys).Tables(0)
                If dt.Rows.Count <= 0 Then Exit Sub
                beSys = New beSC_Sys.Row(dt.Rows(0))

                lblSysID.Text = beSys.SysID.Value
                txtSysName.Text = beSys.SysName.Value.Trim()
                lblCreateDate.Text = beSys.CreateDate.Value.ToString("yyyy/MM/dd HH:mm:ss")

                '20150306 Beatrice modify
                Dim CompName As String = ""
                If objSC.GetCompName(beSys.LastChgComp.Value).Rows.Count > 0 Then
                    CompName = objSC.GetCompName(beSys.LastChgComp.Value).Rows(0).Item("CompName").ToString()
                End If
                lblLastChgComp.Text = beSys.LastChgComp.Value + "-" + CompName
                '20150306 Beatrice modify End
                lblLastChgID.Text = beSys.LastChgID.Value
                lblLastChgDate.Text = beSys.LastChgDate.Value.ToString("yyyy/MM/dd HH:mm:ss")
            End Using
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & ".GetData", ex)
        End Try
    End Sub

    Private Function SaveData() As Boolean
        Dim beSys As New beSC_Sys.Row()
        Dim bsSys As New beSC_Sys.Service()
        Dim objSC As New SC

        With beSys
            .SysID.Value = lblSysID.Text
            .SysName.Value = txtSysName.Text

            .LastChgComp.Value = UserProfile.ActCompID
            .LastChgID.Value = UserProfile.ActUserID
            .LastChgDate.Value = Now
        End With

        If Not bsSys.IsDataExists(beSys) Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00020", "")
            Return False
        End If

        Try
            Return objSC.UpdateSys(beSys)
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & ".SaveData", ex)
            Return False
        End Try
    End Function

    Private Function funCheckData() As Boolean
        Dim strValue As String

        strValue = txtSysName.Text.ToString()
        If strValue = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", "系統別名稱")
            txtSysName.Focus()
            Return False
        Else
            If Bsp.Utility.getStringLength(strValue) > txtSysName.MaxLength Then
                Bsp.Utility.ShowFormatMessage(Me, "W_00040", "系統別名稱", txtSysName.MaxLength.ToString())
                txtSysName.Focus()
                Return False
            End If
            txtSysName.Text = strValue
        End If

        Return True
    End Function
End Class
