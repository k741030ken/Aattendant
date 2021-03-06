'****************************************************
'功能說明：查詢部門設定
'建立人員：Chung
'建立日期：2013/08/07
'****************************************************
Imports System.Data

Partial Class SC_SC0340
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        txtOrganID.Attributes.Add("onkeypress", "EntertoSubmit();")
        txtUserID.Attributes.Add("onkeypress", "EntertoSubmit();")
        txtGroupID.Attributes.Add("onkeypress", "EntertoSubmit();")

        If Not IsPostBack() Then
            Page.SetFocus(txtOrganID)
        End If
    End Sub

    Protected Overrides Sub BaseOnPageTransfer(ByVal ti As TransferInfo)
        If Not IsPostBack Then
            If ti.Args.Length > 0 Then
                Dim ht As Hashtable = Bsp.Utility.getHashTableFromParam(ti.Args)

                For Each s In ht.Keys
                    Dim ctl As Control = Me.FindControl(s)

                    If ctl IsNot Nothing Then
                        CType(ctl, TextBox).Text = ht(s).ToString()
                    End If
                Next
                DoQuery()
            End If
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
        Dim btnU As New ButtonState(ButtonState.emButtonType.Update)
        Dim btnX As New ButtonState(ButtonState.emButtonType.Exit)

        btnA.Caption = "存檔繼續"
        btnU.Caption = "存檔返回"
        btnX.Caption = "返回"

        Me.TransferFramePage("~/SC/SC0341.aspx", New ButtonState() {btnA, btnU, btnX}, _
            Bsp.Utility.FormatToParam(txtOrganID), _
            Bsp.Utility.FormatToParam(txtUserID), _
            Bsp.Utility.FormatToParam(txtGroupID))
    End Sub

    Private Sub DoUpdate()
        Dim btnU As New ButtonState(ButtonState.emButtonType.Update)
        Dim btnX As New ButtonState(ButtonState.emButtonType.Exit)
        Dim intRow As Integer = selectedRow(gvMain)

        Dim objUserID As Label = gvMain.Rows(intRow).FindControl("lblUserID")
        Dim objOrganID As Label = gvMain.Rows(intRow).FindControl("lblOrganID")
        Dim objGroupID As Label = gvMain.Rows(intRow).FindControl("lblGroupID")

        btnU.Caption = "存檔返回"
        btnX.Caption = "返回"

        Me.TransferFramePage("~/SC/SC0342.aspx", New ButtonState() {btnU, btnX}, _
                Bsp.Utility.FormatToParam(txtOrganID), _
                Bsp.Utility.FormatToParam(txtUserID), _
                Bsp.Utility.FormatToParam(txtGroupID), _
                "OrganID=" & objOrganID.Text, "UserID=" & objUserID.Text, "GroupID=" & objGroupID.Text)
    End Sub

    Private Sub DoQuery()
        Dim ht As New Hashtable()
        Dim objSC As New SC()

        ht.Add("OrganID", txtOrganID.Text.Trim())
        ht.Add("UserID", txtUserID.Text.Trim())
        ht.Add("GroupID", txtGroupID.Text.Trim())

        Try
            pcMain.DataTable = objSC.GetSC0340Data(ht)
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & ".DoQuery", ex)
        End Try
    End Sub

    Private Sub DoDelete()
        Dim beDeptQuerySetting As New beSC_DeptQuerySetting.Row()
        Dim objSC As New SC
        Dim intRow As Integer = selectedRow(gvMain)

        Dim objUserID As Label = gvMain.Rows(intRow).FindControl("lblUserID")
        Dim objOrganID As Label = gvMain.Rows(intRow).FindControl("lblOrganID")
        Dim objGroupID As Label = gvMain.Rows(intRow).FindControl("lblGroupID")

        With beDeptQuerySetting
            .OrganID.Value = objOrganID.Text
            .UserID.Value = objUserID.Text
            .GroupID.Value = objGroupID.Text
        End With
        Try
            objSC.deleteDeptQuerySetting(beDeptQuerySetting)
            DoQuery()
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & ".DoDelete", ex)
        End Try
    End Sub

    Private Sub DoOtherAction()

    End Sub

End Class
