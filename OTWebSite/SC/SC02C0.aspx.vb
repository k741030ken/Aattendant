'****************************************************
'功能說明：SC_Agency維護-授管處使用
'建立人員：Chung
'建立日期：2013/01/28
'****************************************************
Imports System.Data

Partial Class SC_SC02C0
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If StateMain IsNot Nothing Then
            sdsMain.SelectCommand = CType(StateMain, String)
        End If

        If Not IsPostBack() Then

        End If
    End Sub

    Protected Overrides Sub BaseOnPageTransfer(ByVal ti As TransferInfo)
        If Not IsPostBack Then
            ousUser.setDeptID(ti.Args(0))
            ousUser.setUserID(ti.Args(1))
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

        Me.TransferFramePage("~/SC/SC02C1.aspx", New ButtonState() {btnA, btnX}, _
            ousUser.SelectDeptID, ousUser.SelectedUserID)
    End Sub

    Private Sub DoUpdate()
        Dim btnU As New ButtonState(ButtonState.emButtonType.Update)
        Dim btnX As New ButtonState(ButtonState.emButtonType.Exit)
        Dim intRow As Integer = selectedRow(gvMain)

        btnU.Caption = "存檔返回"
        btnX.Caption = "返回"

        Me.TransferFramePage("~/SC/SC02C2.aspx", New ButtonState() {btnU, btnX}, _
            ousUser.SelectDeptID, ousUser.SelectedUserID, _
            CType(gvMain.Rows(intRow).FindControl("lblUserID"), Label).Text, _
            CType(gvMain.Rows(intRow).FindControl("lblAgentUserID"), Label).Text)
    End Sub

    Private Sub DoQuery()
        If ousUser.SelectedUserID = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00035", "員工")
            Return
        End If
        Dim objSC As New SC()

        Try
            StateMain = objSC.GetSC02C0QueryString(ousUser.SelectedUserID)
            sdsMain.SelectCommand = StateMain
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & ".DoQuery", ex)
        End Try
    End Sub

    Private Sub DoDelete()
        Dim beSC_Agency As New beSC_Agency.Row()
        Dim bsSC_Agency As New beSC_Agency.Service()
        Dim objSC As New SC
        Dim intRow As Integer = selectedRow(gvMain)

        beSC_Agency.UserID.Value = CType(gvMain.Rows(intRow).FindControl("lblUserID"), Label).Text
        beSC_Agency.AgentUserID.Value = CType(gvMain.Rows(intRow).FindControl("lblAgentUserID"), Label).Text
        Try
            bsSC_Agency.DeleteRowByPrimaryKey(beSC_Agency)
            gvMain.DataBind()
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
