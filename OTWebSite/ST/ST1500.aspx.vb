﻿'****************************************************
'功能說明：員工前職經歷維護
'建立人員：Micky Sung
'建立日期：2015.06.09
'****************************************************
Imports System.Data

Partial Class ST_ST1500
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then

        End If
    End Sub

    Public Overrides Sub DoAction(ByVal Param As String)
        Select Case Param
            Case "btnAdd"       '新增
                DoAdd()
            Case "btnUpdate"    '修改
                DoUpdate()
            Case "btnDelete"    '刪除
                DoDelete()
        End Select
    End Sub

    Protected Overrides Sub BaseOnPageTransfer(ByVal ti As TransferInfo)
        If Not IsPostBack() Then
            Dim ht As Hashtable = Bsp.Utility.getHashTableFromParam(ti.Args)
            If ht.ContainsKey("SelectedCompID") Then
                ViewState.Item("CompID") = ht("SelectedCompID").ToString()
                ViewState.Item("CompName") = ht("SelectedCompName").ToString()
                ViewState.Item("EmpID") = ht("SelectedEmpID").ToString()
                ViewState.Item("EmpName") = ht("SelectedEmpName").ToString()
                ViewState.Item("IDNo") = ht("SelectedIDNo").ToString()

                lblCompRoleID.Text = ViewState.Item("CompID").ToString + "-" + ViewState.Item("CompName")
                txtEmpID.Text = ViewState.Item("EmpID").ToString
                txtEmpName.Text = ViewState.Item("EmpName").ToString
            Else
                Return
            End If
            DoQuery()
        End If
    End Sub

    Private Sub DoAdd()
        Dim objSC As New SC
        Dim btnA As New ButtonState(ButtonState.emButtonType.Add)
        Dim btnX As New ButtonState(ButtonState.emButtonType.Exit)
        Dim btnC As New ButtonState(ButtonState.emButtonType.Cancel)

        btnA.Caption = "存檔返回"
        btnX.Caption = "返回"
        btnC.Caption = "清除"

        Me.TransferFramePage("~/ST/ST1501.aspx", New ButtonState() {btnA, btnX, btnC}, _
            lblCompRoleID.ID & "=" & ViewState.Item("CompID"), _
            txtEmpID.ID & "=" & txtEmpID.Text, _
            txtEmpName.ID & "=" & txtEmpName.Text, _
            "SelectedCompID=" & ViewState.Item("CompID"), _
            "SelectedCompName=" & objSC.GetCompName(ViewState.Item("CompID")).Rows(0).Item("CompName").ToString, _
            "SelectedEmpID=" & txtEmpID.Text, _
            "SelectedEmpName=" & txtEmpName.Text, _
            "SelectedIDNo=" & ViewState.Item("IDNo"), _
            "SelectedSeq=" & ViewState.Item("Seq"), _
            "PageNo=" & pcMain.PageNo.ToString(), _
            "DoQuery=" & Bsp.Utility.IsStringNull(ViewState.Item("DoQuery")))
    End Sub

    Private Sub DoUpdate()
        Dim objSC As New SC
        Dim btnU As New ButtonState(ButtonState.emButtonType.Update)
        Dim btnX As New ButtonState(ButtonState.emButtonType.Exit)
        Dim btnC As New ButtonState(ButtonState.emButtonType.Cancel)

        btnU.Caption = "存檔返回"
        btnX.Caption = "返回"
        btnC.Caption = "清除"

        Me.TransferFramePage("~/ST/ST1502.aspx", New ButtonState() {btnU, btnX, btnC}, _
            lblCompRoleID.ID & "=" & ViewState.Item("CompID"), _
            txtEmpID.ID & "=" & txtEmpID.Text, _
            txtEmpName.ID & "=" & txtEmpName.Text, _
            "SelectedCompID=" & ViewState.Item("CompID"), _
            "SelectedCompName=" & objSC.GetCompName(ViewState.Item("CompID")).Rows(0).Item("CompName").ToString, _
            "SelectedEmpID=" & txtEmpID.Text, _
            "SelectedEmpName=" & txtEmpName.Text, _
            "SelectedIDNo=" & gvMain.DataKeys(selectedRow(gvMain))("IDNo").ToString(), _
            "SelectedSeq=" & gvMain.DataKeys(selectedRow(gvMain))("Seq").ToString(), _
            "PageNo=" & pcMain.PageNo.ToString(), _
            "DoQuery=" & Bsp.Utility.IsStringNull(ViewState.Item("DoQuery")))
    End Sub

    Private Sub DoQuery()
        Dim objST As New ST1
        gvMain.Visible = True

        Try
            pcMain.DataTable = objST.QueryExperienceSetting(
                "CompID=" & ViewState.Item("CompID"), _
                "EmpID=" & txtEmpID.Text, _
                "EmpName=" & txtEmpName.Text)

        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & ".DoQuery", ex)
        End Try
    End Sub

    Private Sub DoDelete()
        If selectedRow(gvMain) < 0 Then
            '尚未選取資料列！
            Bsp.Utility.ShowFormatMessage(Me, "W_00000")
        Else
            Dim beExperience As New beExperience.Row()
            Dim objST As New ST1

            beExperience.IDNo.Value = gvMain.DataKeys(Me.selectedRow(gvMain))("IDNo").ToString()
            beExperience.Seq.Value = gvMain.DataKeys(Me.selectedRow(gvMain))("Seq").ToString()

            Try
                objST.DeleteExperienceSetting(beExperience, ViewState.Item("CompID"), txtEmpID.Text)
            Catch ex As Exception
                Bsp.Utility.ShowMessage(Me, Me.FunID & ".DoDelete", ex)
            End Try
            gvMain.DataBind()

            DoQuery()
        End If
    End Sub

    Protected Sub gvMain_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvMain.RowCommand
        If e.CommandName = "Detail" Then
            Dim objSC As New SC
            Dim btnX As New ButtonState(ButtonState.emButtonType.Exit)

            btnX.Caption = "返回"

            Me.TransferFramePage("~/ST/ST1502.aspx", New ButtonState() {btnX}, _
                lblCompRoleID.ID & "=" & ViewState.Item("CompID"), _
                txtEmpID.ID & "=" & txtEmpID.Text, _
                txtEmpName.ID & "=" & txtEmpName.Text, _
                "SelectedCompID=" & ViewState.Item("CompID"), _
                "SelectedCompName=" & objSC.GetCompName(ViewState.Item("CompID")).Rows(0).Item("CompName").ToString, _
                "SelectedEmpID=" & txtEmpID.Text, _
                "SelectedEmpName=" & txtEmpName.Text, _
                "SelectedIDNo=" & gvMain.DataKeys(selectedRow(gvMain))("IDNo").ToString(), _
                "SelectedSeq=" & gvMain.DataKeys(selectedRow(gvMain))("Seq").ToString(), _
                "PageNo=" & pcMain.PageNo.ToString(), _
                "DoQuery=" & Bsp.Utility.IsStringNull(ViewState.Item("DoQuery")))
        End If
    End Sub

End Class
