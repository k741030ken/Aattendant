'****************************************************
'功能說明：系統別維護
'建立人員：Weicheng
'建立日期：2014/08/18
'****************************************************
Imports System.Data

Partial Class SC_SC0200
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        txtSysID.Attributes.Add("onkeypress", "EntertoSubmit();")
        txtSysName.Attributes.Add("onkeypress", "EntertoSubmit();")

        If Not IsPostBack Then
            Page.SetFocus(txtSysID)
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
        End Select
    End Sub

    Protected Overrides Sub BaseOnPageTransfer(ByVal ti As TransferInfo)
        If Not IsPostBack() Then
            Dim ht As Hashtable = Bsp.Utility.getHashTableFromParam(ti.Args)

            For Each strKey As String In ht.Keys
                Dim ctl As Control = Page.FindControl(strKey)

                If TypeOf ctl Is TextBox Then
                    CType(ctl, TextBox).Text = ht(strKey).ToString()
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

    Private Sub DoAdd()
        Dim btnA As New ButtonState(ButtonState.emButtonType.Add)
        Dim btnX As New ButtonState(ButtonState.emButtonType.Exit)

        btnA.Caption = "存檔返回"
        btnX.Caption = "返回"

        Me.TransferFramePage("~/SC/SC0201.aspx", New ButtonState() {btnA, btnX}, _
                             txtSysID.ID & "=" & txtSysID.Text.ToUpper(), _
                             txtSysName.ID & "=" & txtSysName.Text, _
                             "PageNo=" & pcMain.PageNo.ToString(), _
                             "DoQuery=" & Bsp.Utility.IsStringNull(ViewState.Item("DoQuery")))
    End Sub

    Private Sub DoUpdate()
        If selectedRow(gvMain) >= 0 Then
            Dim btnU As New ButtonState(ButtonState.emButtonType.Update)
            Dim btnX As New ButtonState(ButtonState.emButtonType.Exit)

            btnU.Caption = "存檔返回"
            btnX.Caption = "返回"

            Me.TransferFramePage("~/SC/SC0202.aspx", New ButtonState() {btnU, btnX}, _
                             txtSysID.ID & "=" & txtSysID.Text.ToUpper(), _
                             txtSysName.ID & "=" & txtSysName.Text, _
                             "PageNo=" & pcMain.PageNo.ToString(), _
                             "SelectedSysID=" & gvMain.DataKeys(Me.selectedRow(gvMain))("SysID").ToString(), _
                             "DoQuery=Y")
        End If
    End Sub

    Private Sub DoQuery()
        Dim objSC As New SC()

        Try
            pcMain.DataTable = objSC.QuerySys( _
                "SysID=" & txtSysID.Text.Trim(), _
                "SysName=" & txtSysName.Text.Trim())
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & ".DoQuery", ex)
        End Try
    End Sub

    Private Sub DoDelete()
        If selectedRow(gvMain) < 0 Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00000")
        Else
            Dim beSC_Sys As New beSC_Sys.Row()
            Dim objSC As New SC

            beSC_Sys.SysID.Value = gvMain.DataKeys(Me.selectedRow(gvMain))("SysID").ToString()

            Try
                objSC.DeleteSys(beSC_Sys)
            Catch ex As Exception
                Bsp.Utility.ShowMessage(Me, Me.FunID & ".DoDelete", ex)
            End Try
            DoQuery()
        End If
    End Sub
End Class
