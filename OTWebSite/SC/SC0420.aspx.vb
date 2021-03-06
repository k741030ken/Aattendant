'****************************************************
'功能說明：訊息定義檔
'建立人員：Chung
'建立日期：2013/01/29
'****************************************************
Imports System.Data

Partial Class SC_SC0420
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then
            Bsp.Utility.FillCommon(ddlMsgKind, "02", Bsp.Enums.SelectCommonType.All, Bsp.Enums.FullNameType.OnlyDefine)
            ddlMsgKind.Items.Insert(0, New ListItem("---所有---", ""))
            Page.SetFocus(txtMsgCode)
        End If
    End Sub

    Protected Overrides Sub BaseOnPageTransfer(ByVal ti As TransferInfo)
        If Not IsPostBack Then
            Dim ht As Hashtable = Bsp.Utility.getHashTableFromParam(ti.Args)

            For Each strKey As String In ht.Keys
                Select Case strKey
                    Case "PageIndex"
                        pcMain.PageNo = CType(ht(strKey), Integer)
                    Case Else
                        Dim ctl As Control = Page.Form.FindControl(strKey)
                        If TypeOf ctl Is TextBox Then CType(ctl, TextBox).Text = ht(strKey).ToString()
                        If TypeOf ctl Is DropDownList Then Bsp.Utility.IniListWithValue(ddlMsgKind, ht(strKey).ToString())
                End Select
            Next
            If ht.ContainsKey("PageIndex") Then
                If CInt(ht("PageIndex")) > 0 Then DoQuery()
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

        Me.TransferFramePage("SC0421.aspx", New ButtonState() {btnA, btnU, btnX}, _
            Bsp.Utility.FormatToParam(ddlMsgKind), Bsp.Utility.FormatToParam(txtMsgCode), _
            "PageIndex=" & pcMain.PageNo.ToString())
    End Sub

    Private Sub DoUpdate()
        Dim btnU As New ButtonState(ButtonState.emButtonType.Update)
        Dim btnX As New ButtonState(ButtonState.emButtonType.Exit)
        Dim intSelectedRow As Integer = selectedRow(gvMain)

        btnU.Caption = "存檔返回"
        btnX.Caption = "返回"

        Me.TransferFramePage("SC0422.aspx", New ButtonState() {btnU, btnX}, _
            Bsp.Utility.FormatToParam(ddlMsgKind), Bsp.Utility.FormatToParam(txtMsgCode), _
            "PageIndex=" & pcMain.PageNo.ToString(), _
            "SelectedMsgCode=" & gvMain.DataKeys(intSelectedRow)("MsgCode").ToString())
    End Sub

    Private Sub DoQuery()
        Dim objSC As New SC()
        Dim ht As New Hashtable()

        ht.Add("MsgCode", txtMsgCode.Text)
        ht.Add("MsgKind", ddlMsgKind.SelectedValue)

        Try
            pcMain.DataTable = objSC.GetMsgDefineData(ht)
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, FunID & ".DoQuery", ex)
        End Try
    End Sub

    Private Sub DoDelete()
        Dim objSC As New SC()
        Dim intSelectedRow As Integer = selectedRow(gvMain)

        Dim beMsgDefine As New beWF_MsgDefine.Row()
        Dim bsMsgDefine As New beWF_MsgDefine.Service()

        beMsgDefine.MsgCode.Value = gvMain.DataKeys(intSelectedRow)("MsgCode").ToString()
        Try
            bsMsgDefine.DeleteRowByPrimaryKey(beMsgDefine)
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, FunID & ".DoDelete", ex)
        End Try
        DoQuery()
    End Sub

    Private Sub DoOtherAction()

    End Sub

End Class
