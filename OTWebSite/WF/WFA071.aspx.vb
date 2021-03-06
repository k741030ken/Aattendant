'****************************************************
'功能說明：流程查詢-顯示頁
'建立人員：Chung
'建立日期：2013/01/29
'****************************************************
Imports System.Data

Partial Class WF_WFA071
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Overrides Sub BaseOnPageTransfer(ByVal ti As TransferInfo)
        If Not IsPostBack Then
            Dim objWF As New WF()

            pcMain.DataTable = objWF.GetToDoListForWFA070(ti.Args)
        End If
    End Sub

    Private Sub GoBack()
        Dim ti As TransferInfo = Me.StateTransfer
        Me.TransferFramePage(ti.CallerUrl, Nothing, ti.Args)
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
            Case "btnActionX"
                GoBack()
            Case Else
                DoOtherAction()   '其他功能動作
        End Select
    End Sub

    Private Sub DoAdd()

    End Sub

    Private Sub DoUpdate()
        Dim intRow As Integer = SelectedRow(pcMain.GridViewName)
        Dim objGV As GridView = Page.Form.FindControl(pcMain.GridViewName)

        If intRow < 0 Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00000")
            Return
        End If

        Try
            Dim args() As Object = StateTransfer.Args

            Array.Resize(args, args.Length + 4)
            args(args.Length - 4) = "FlowID=" & objGV.DataKeys(intRow)("FlowID").ToString()
            args(args.Length - 3) = "FlowCaseID=" & objGV.DataKeys(intRow)("FlowCaseID").ToString()
            args(args.Length - 2) = "FlowLogBatNo=" & objGV.DataKeys(intRow)("FlowLogBatNo").ToString()
            args(args.Length - 1) = "FlowLogID=" & objGV.DataKeys(intRow)("FlowLogID").ToString()

            Me.TransferFramePage(Bsp.MySettings.FlowRedirectPage, Nothing, args)
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & ".DoUpdate", ex)
        End Try

    End Sub

    Private Sub DoQuery()

    End Sub

    Private Sub DoDelete()

    End Sub

    Private Sub DoOtherAction()

    End Sub

    Protected Sub gvMain_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvMain.RowCommand
        If e.CommandName = "FlowDetail" Then
            Dim intRow As Integer = Convert.ToInt32(e.CommandArgument)

            Dim btnP As New ButtonState(ButtonState.emButtonType.Print)
            Dim btnX As New ButtonState(ButtonState.emButtonType.Exit)
            Dim strURL As String = "~/WF/WFA030.aspx"

            strURL &= "?FlowID=" & CType(sender, GridView).DataKeys(intRow)("FlowID").ToString()
            strURL &= "&FlowCaseID=" & CType(sender, GridView).DataKeys(intRow)("FlowCaseID").ToString()
            strURL &= "&FlowLogID=" & CType(sender, GridView).DataKeys(intRow)("FlowLogID").ToString()

            Me.CallMiddlePage(strURL, New ButtonState() {btnP, btnX})
        End If
    End Sub

    Protected Sub gvMain_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvMain.RowCreated
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim objLB As LinkButton = e.Row.FindControl("lbnFlowDetail")

            objLB.CommandArgument = e.Row.RowIndex.ToString()
        End If
    End Sub
End Class
