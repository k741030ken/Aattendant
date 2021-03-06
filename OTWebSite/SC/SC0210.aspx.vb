'****************************************************
'功能說明：部門維護
'建立人員：Chung
'建立日期：2011/05/16
'****************************************************
Imports System.Data

Partial Class SC_SC0210
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        txtOrganID.Attributes.Add("onkeypress", "EntertoSubmit();")
        txtOrganName.Attributes.Add("onkeypress", "EntertoSubmit();")

        Page.SetFocus(txtOrganID)
    End Sub

    Protected Overrides Sub BaseOnPageTransfer(ByVal ti As TransferInfo)
        If Not IsPostBack() Then
            Dim ht As Hashtable = Bsp.Utility.getHashTableFromParam(ti.Args)

            For Each strKey As String In ht.Keys
                Dim ctl As Control = Me.FindControl(strKey)

                If TypeOf ctl Is TextBox Then
                    CType(ctl, TextBox).Text = ht(strKey).ToString()
                ElseIf TypeOf ctl Is DropDownList Then
                    Bsp.Utility.SetSelectedIndex(CType(ctl, DropDownList), ht(strKey).ToString())
                End If
            Next
            If ht.ContainsKey("PageNo") Then
                pcMain.PageNo = Convert.ToInt32(ht("PageNo"))
            End If
            If ht.ContainsKey("DoQuery") Then
                If ht("DoQuery").ToString() = "Y" Then
                    ViewState.Item("DoQuery") = "Y"
                    DoQuery()
                End If
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
                ViewState.Item("DoQuery") = "Y"
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

        Me.TransferFramePage("~/SC/SC0211.aspx", New ButtonState() {btnA, btnX}, _
                             txtOrganID.ID & "=" & txtOrganID.Text, _
                             txtOrganName.ID & "=" & txtOrganName.Text, _
                             ddlOrganType.ID & "=" & ddlOrganType.SelectedIndex, _
                             "PageNo=" & pcMain.PageNo.ToString(), _
                             "DoQuery=" & Bsp.Utility.IsStringNull(ViewState.Item("DoQuery")))
    End Sub

    Private Sub DoUpdate()
        Dim btnU As New ButtonState(ButtonState.emButtonType.Update)
        Dim btnX As New ButtonState(ButtonState.emButtonType.Exit)

        btnU.Caption = "存檔返回"
        btnX.Caption = "返回"

        Me.TransferFramePage("~/SC/SC0212.aspx", New ButtonState() {btnU, btnX}, _
                             txtOrganID.ID & "=" & txtOrganID.Text, _
                             txtOrganName.ID & "=" & txtOrganName.Text, _
                             ddlOrganType.ID & "=" & ddlOrganType.SelectedValue, _
                             "PageNo=" & pcMain.PageNo.ToString(), _
                             "SelectedOrganID=" & gvMain.DataKeys(selectedRow(gvMain))("OrganID").ToString(), _
                             "DoQuery=Y")
    End Sub

    Private Sub DoQuery()
        Dim objSC As New SC()

        Try
            pcMain.DataTable = objSC.QueryOrganization( _
                    "OrganID=" & txtOrganID.Text.Trim().ToUpper(), _
                    "OrganName=" & txtOrganName.Text.Trim().ToUpper(), _
                    "OrganType=" & ddlOrganType.SelectedValue)
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, ex.Message)
        End Try
    End Sub

    Private Sub DoDelete()
        If selectedRow(gvMain) < 0 Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00000")
        Else
            Dim beOrgan As New beSC_Organization.Row()
            Dim objSC As New SC()

            beOrgan.OrganID.Value = gvMain.DataKeys(Me.selectedRow(gvMain))("OrganID").ToString()

            Try
                objSC.DeleteOrganization(beOrgan)
                gvMain.DataBind()
            Catch ex As Exception
                Bsp.Utility.ShowMessage(Me, Me.FunID & ".DoDelete", ex)
            End Try
        End If
    End Sub

    Private Sub DoOtherAction()

    End Sub
End Class
