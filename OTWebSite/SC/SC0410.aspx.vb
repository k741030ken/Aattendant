'****************************************************
'功能說明：WF_FlowStep維護
'建立人員：Chung
'建立日期：2013/01/29
'****************************************************
Imports System.Data

Partial Class SC_SC0410
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then
            Bsp.Utility.FillCommon(ddlFlowID, "003", Bsp.Enums.SelectCommonType.All)
            ddlFlowID.Items.Insert(0, New ListItem("----All----", ""))
            Page.SetFocus(ddlFlowID)
        End If
    End Sub

    Protected Overrides Sub BaseOnPageTransfer(ByVal ti As TransferInfo)
        If ti.Args.Length > 0 Then
            Dim ht As Hashtable = Bsp.Utility.getHashTableFromParam(ti.Args)

            For Each strKey As String In ht.Keys
                Select Case strKey
                    Case "PageIndex"
                        pcMain.PageNo = CInt(ht(strKey))
                    Case Else
                        Dim ctl As Control = Page.Form.FindControl(strKey)
                        If ctl IsNot Nothing Then
                            If TypeOf ctl Is DropDownList Then
                                Bsp.Utility.IniListWithValue(CType(ctl, DropDownList), ht(strKey).ToString())
                            End If
                            If TypeOf ctl Is TextBox Then
                                CType(ctl, TextBox).Text = ht(strKey)
                            End If
                        End If
                End Select
            Next
            If ht.ContainsKey("PageIndex") Then
                If CInt(ht("PageIndex")) > 0 Then
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
        Dim btnX As New ButtonState(ButtonState.emButtonType.Exit)

        btnA.Caption = "存檔返回"
        btnX.Caption = "返回"

        Me.TransferFramePage("SC0411.aspx", New ButtonState() {btnA, btnX}, _
            Bsp.Utility.FormatToParam(ddlFlowID), Bsp.Utility.FormatToParam(txtFlowStepID), _
            Bsp.Utility.FormatToParam(txtDescription), "PageIndex=" & pcMain.PageNo.ToString())
    End Sub

    Private Sub DoUpdate()
        Dim btnU As New ButtonState(ButtonState.emButtonType.Update)
        Dim btnX As New ButtonState(ButtonState.emButtonType.Exit)
        Dim intRow As Integer = selectedRow(gvMain)

        btnU.Caption = "存檔返回"
        btnX.Caption = "返回"

        Me.TransferFramePage("SC0412.aspx", New ButtonState() {btnU, btnX}, _
            Bsp.Utility.FormatToParam(ddlFlowID), Bsp.Utility.FormatToParam(txtFlowStepID), _
            Bsp.Utility.FormatToParam(txtDescription), "PageIndex=" & pcMain.PageNo.ToString(), _
            "SelectedFlowID=" & gvMain.DataKeys(intRow)("FlowID").ToString(), _
            "SelectedFlowVer=" & gvMain.DataKeys(intRow)("FlowVer").ToString(), _
            "SelectedFlowStepID=" & gvMain.DataKeys(intRow)("FlowStepID").ToString())
    End Sub

    Private Sub DoQuery()
        Dim objSC As New SC()
        Dim ht As New Hashtable()

        ht.Add("FlowID", ddlFlowID.SelectedValue)
        ht.Add("FlowStepID", txtFlowStepID.Text)
        ht.Add("Description", txtDescription.Text)

        Try
            pcMain.DataTable = objSC.GetFlowStepM(ht)
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & ".DoQuery", ex)
        End Try
    End Sub

    Private Sub DoDelete()
        If selectedRow(gvMain) < 0 Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00000")
        Else
            Dim beFlowStep As New beWF_FlowStepM.Row()
            Dim objSC As New SC()

            Try
                beFlowStep.FlowID.Value = gvMain.DataKeys(selectedRow(gvMain))("FlowID").ToString()
                beFlowStep.FlowVer.Value = Convert.ToDecimal(gvMain.DataKeys(selectedRow(gvMain))("FlowVer"))
                beFlowStep.FlowStepID.Value = gvMain.DataKeys(selectedRow(gvMain))("FlowStepID").ToString()

                objSC.DeleteFlowStep(beFlowStep)
            Catch ex As Exception
                Bsp.Utility.ShowMessage(Me, Me.FunID & ".DoDelete", ex, beFlowStep.FlowID.Value, beFlowStep.FlowVer.Value, beFlowStep.FlowStepID.Value)
            End Try
            DoQuery()
        End If
    End Sub

    Private Sub DoOtherAction()

    End Sub

End Class
