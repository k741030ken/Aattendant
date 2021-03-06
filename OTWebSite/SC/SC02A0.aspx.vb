'****************************************************
'功能說明：代理人維護
'建立人員：Chung
'建立日期：2013/01/28
'****************************************************
Imports System.Data

Partial Class SC_SC02A0
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then
            DoQuery()
        End If
    End Sub

    Public Overrides Sub DoAction(ByVal Param As String)
        Select Case Param
            Case "btnAdd"       '新增
                DoAdd()
            Case "btnUpdate"    '修改
                DoUpdate()
            Case "btnQuery"     '查詢
                DoQuery()
            Case "btnDelete"    '刪除
                DoDelete()
            Case Else
                DoOtherAction()   '其他功能動作
        End Select
    End Sub

    Private Sub DoAdd()
        Dim btnA As New ButtonState(ButtonState.emButtonType.Add)
        Dim btnX As New ButtonState(ButtonState.emButtonType.Exit)

        btnA.Caption = "存檔返回"
        btnX.Caption = "返回"

        Me.TransferFramePage("~/SC/SC02A1.aspx", New ButtonState() {btnA, btnX})
    End Sub

    Private Sub DoUpdate()
        Dim btnU As New ButtonState(ButtonState.emButtonType.Update)
        Dim btnX As New ButtonState(ButtonState.emButtonType.Exit)
        Dim intRow As Integer = selectedRow(gvMain)

        btnU.Caption = "存檔返回"
        btnX.Caption = "返回"

        Me.TransferFramePage("~/SC/SC02A2.aspx", New ButtonState() {btnU, btnX}, _
            CType(gvMain.Rows(intRow).FindControl("lblAgentUserID"), Label).Text)
    End Sub

    Private Sub DoQuery()
        Dim objSC As New SC()

        Try
            pcMain.DataTable = objSC.GetAgency(UserProfile.UserID, "", "*, dbo.funGetAOrgDefine('3', AgentUserID) AgentUserName, dbo.funGetAOrgDefine('3', UserID) UserName")
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & ".DoQuery", ex)
        End Try
    End Sub

    Private Sub DoDelete()
        Dim beAgency As New beSC_Agency.Row()
        Dim objSC As New SC

        Try
            With beAgency
                .UserID.Value = UserProfile.UserID
                .AgentUserID.Value = CType(gvMain.Rows(selectedRow(gvMain)).FindControl("lblAgentUserID"), Label).Text
            End With
            objSC.DeleteAgency(beAgency)
            'gvMain.DataBind()
            DoQuery()
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & ".DoDelete", ex)
        End Try
    End Sub

    Private Sub DoOtherAction()

    End Sub

    Protected Sub gvMain_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvMain.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim objValidFrom As Label = e.Row.FindControl("lblValidFrom")
            Dim objValidTo As Label = e.Row.FindControl("lblValidTo")

            If objValidFrom IsNot Nothing AndAlso objValidTo IsNot Nothing Then
                If objValidFrom.Text > Today.ToString("yyyy/MM/dd") OrElse _
                    objValidTo.Text < Today.ToString("yyyy/MM/dd") Then
                    e.Row.Style.Item("Color") = "Red"
                End If
            End If
        End If
    End Sub
End Class
