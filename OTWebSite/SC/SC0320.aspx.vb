'****************************************************
'功能說明：公用代碼維護
'建立人員：Chung
'建立日期：2007/04/04
'****************************************************
Imports System.Data

Partial Class SC_SC0320
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack() Then
            Bsp.Utility.FillCommon(ddlType, "000", Bsp.Enums.SelectCommonType.All)
            ddlType.Items.Insert(0, New ListItem("---所有類别---", ""))
        End If

        Page.SetFocus(ddlType)
    End Sub

    Protected Overrides Sub BaseOnPageTransfer(ByVal ti As TransferInfo)
        If Not IsPostBack Then
            Dim ht As Hashtable = Bsp.Utility.getHashTableFromParam(ti.Args)

            For Each strKey As String In ht.Keys
                Dim ctl As Control = Me.FindControl(strKey)

                If TypeOf ctl Is TextBox Then
                    CType(ctl, TextBox).Text = ht(strKey).ToString()
                ElseIf TypeOf ctl Is DropDownList Then
                    Bsp.Utility.SetSelectedIndex(CType(ctl, DropDownList), ht(strKey).ToString())
                End If
            Next
            If ht.ContainsKey("PageNo") Then pcMain.PageNo = Convert.ToInt32(ht("PageNo"))
            If ht.ContainsKey("DoQuery") Then
                If ht("DoQuery").ToString() = "Y" Then
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

        Me.TransferFramePage("~/SC/SC0321.aspx", New ButtonState() {btnA, btnU, btnX}, _
                Bsp.Utility.FormatToParam(ddlType), _
                Bsp.Utility.FormatToParam(txtCode), _
                Bsp.Utility.FormatToParam(txtDefine), _
                "PageNo=" & pcMain.PageNo.ToString(), _
                "DoQuery=" & Bsp.Utility.IsStringNull(ViewState.Item("DoQuery")))
    End Sub

    Private Sub DoUpdate()
        Dim btnU As New ButtonState(ButtonState.emButtonType.Update)
        Dim btnX As New ButtonState(ButtonState.emButtonType.Exit)
        Dim strType As String
        Dim strCode As String

        btnU.Caption = "存檔返回"
        btnX.Caption = "返回"

        strType = gvMain.DataKeys(selectedRow(gvMain))("Type").ToString()
        strCode = gvMain.DataKeys(selectedRow(gvMain))("Code").ToString()

        Me.TransferFramePage("~/SC/SC0322.aspx", New ButtonState() {btnU, btnX}, _
                Bsp.Utility.FormatToParam(ddlType), _
                Bsp.Utility.FormatToParam(txtCode), _
                Bsp.Utility.FormatToParam(txtDefine), _
                "PageNo=" & pcMain.PageNo.ToString(), _
                "SelectedType=" & strType, _
                "SelectedCode=" & strCode, _
                "DoQuery=Y")
    End Sub

    Private Sub DoQuery()
        Dim objSC As New SC()

        Try
            pcMain.DataTable = objSC.QueryCommon(ddlType.SelectedValue, txtCode.Text.Trim(), txtDefine.Text.Trim())
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & "DoQuery", ex)
        End Try
        ViewState.Item("DoQuery") = "Y"
    End Sub

    Private Sub DoDelete()
        Dim objSC As New SC
        Dim strType As String = gvMain.Rows(selectedRow(gvMain)).Cells(1).Text.ToString().Split("-")(0)
        Dim strCode As String = gvMain.Rows(selectedRow(gvMain)).Cells(2).Text

        If strType = "000" Then
            If strCode = "000" Then
                Bsp.Utility.ShowMessage(Me, "[DoDelete]:類別代碼不可刪除！")
                Return
            Else
                If objSC.HasChildCommon(strCode) Then
                    Bsp.Utility.ShowMessage(Me, "[DoDelete]:無法删除！已有建立此類別之代碼。")
                    Return
                End If
            End If
        End If
        Dim beCommon As New beSC_Common.Row()
        beCommon.Type.Value = strType
        beCommon.Code.Value = strCode
        Try
            objSC.DeleteCommon(beCommon)
            DoQuery()
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & ".DoDelete", ex)
        End Try
    End Sub

    Private Sub DoOtherAction()

    End Sub

End Class
