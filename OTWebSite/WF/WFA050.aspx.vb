'****************************************************
'功能說明：流程改派處理
'建立人員：Chung
'建立日期：2013/01/29
'****************************************************
Imports System.Data.SqlClient
Imports System.Data
Imports System.Data.Common

Partial Class WF_WFA050
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If UserProfile.GroupID.Contains("S1") Then
            If Not IsPostBack Then
                UserSelect.setDeptID(UserProfile.ActDeptID)
                OneUserSelectAssign.setDeptID(UserProfile.ActDeptID)
            End If
        Else
            UserSelect.FreezeDeptID = UserProfile.DeptID
        End If

        If Not IsPostBack() Then
            PageInit()
        End If

        Dim ToDoUser As String = UserSelect.SelectedUserID
        ViewState.Item("ToDoUser") = ToDoUser

        '取得資料
        Dim objWF As New WF()

        Dim SelectCommandStr As String = objWF.GetUserToDoList(ViewState.Item("ToDoUser"))
        If ddlFlowName.SelectedValue.Trim() <> "" Then
            SelectCommandStr &= " And FlowID = " & Bsp.Utility.Quote(ddlFlowName.SelectedValue.Trim())
        End If
        SelectCommandStr &= " And FlowCaseStatus = 'Open' Order by FromDate"
        sdsMain.SelectCommand = SelectCommandStr
    End Sub

    Private Sub PageInit()
        Bsp.Utility.FillCommon(ddlFlowName, "003", Bsp.Enums.SelectCommonType.All)
        ddlFlowName.Items.Insert(0, New ListItem("請選擇", ""))
    End Sub

    Public Overrides Sub DoAction(ByVal Param As String)
        Select Case Param
            Case "btnAdd"       '新增
                DoAdd()
            Case "btnUpdate"    '修改
                If funCheckData() Then
                    DoUpdate()
                End If
            Case "btnQuery"     '查詢
                DoQuery()
            Case "btnDelete"    '刪除
                DoDelete()
            Case Else
                DoOtherAction(Param)   '其他功能動作
        End Select
    End Sub

    Private Sub DoAdd()

    End Sub

    Private Sub DoUpdate()
        Dim lblobj As Label
        Dim Applblobj As Label
        Dim selectedRowStr As String = selectedRows(gvMain)
        Dim selectedRowArr() As String = selectedRowStr.Split(",")
        Dim objWF As New WF
        Dim FlowID As String
        Dim FlowCaseID As String
        Dim FlowLogBatNo As String
        Dim FlowLogID As String
        Dim ReAssign As String = ""
        Dim MessageStr As String = ""
        Dim FlowStepID As String
        Dim ErrMsg As String = ""
        Dim FlowVer As String
        Dim strFlowKeyValue As String = ""

        Using cn As DbConnection = Bsp.DB.getConnection
            cn.Open()
            Dim tran As DbTransaction = cn.BeginTransaction
            Try
                For intLoop As Integer = 0 To selectedRowArr.GetUpperBound(0)
                    If Trim(selectedRowArr(intLoop).ToString) <> "" Then
                        FlowStepID = ""
                        ErrMsg = ""
                        lblobj = gvMain.Rows(selectedRowArr(intLoop).Split(".")(1)).FindControl("lblFlowKeyStr")
                        Applblobj = gvMain.Rows(selectedRowArr(intLoop).Split(".")(1)).FindControl("APPIDStr")
                        FlowID = lblobj.Text.ToString.Split(";")(0).ToString
                        FlowCaseID = lblobj.Text.ToString.Split(";")(1).ToString
                        FlowLogBatNo = lblobj.Text.ToString.Split(";")(2).ToString
                        FlowLogID = lblobj.Text.ToString.Split(";")(3).ToString

                        Try
                            FlowVer = getFlowVer(FlowID, FlowCaseID, tran)
                        Catch ex As Exception
                            FlowVer = "1"
                        End Try

                        Using dt As DataTable = objWF.GetFlowToDoList(FlowID, FlowCaseID, FlowLogID, tran)
                            If dt.Rows.Count > 0 Then
                                FlowStepID = dt.Rows(0).Item("FlowStepID").ToString
                                strFlowKeyValue = dt.Rows(0).Item("FlowKeyValue").ToString()
                            End If
                        End Using

                        ReAssign = objWF.FlowReAssign(FlowID, FlowVer, FlowCaseID, FlowLogBatNo, FlowLogID, OneUserSelectAssign.SelectedUserID, TxtAreaOption.Value, tran)

                        MessageStr &= "<table><tr><th>選取的案件:<td>【" & Applblobj.Text & "】"
                        MessageStr &= "<tr><th>流程主鍵值：<td>" & lblobj.Text & ""
                        MessageStr &= "<tr><th>改派意見：<td>" & TxtAreaOption.Value.ToString
                        MessageStr &= "<tr><th>原案處理人員：<td>" & UserSelect.SelectedUserID
                        If ReAssign = "" Then
                            MessageStr &= "<tr><th>執行動作：<td>沒有任何改派人員" & ReAssign & "，無改派動作! " & ErrMsg & "</table>"
                        Else
                            MessageStr &= "<tr><th>執行動作：<td>改派改派人員予【" & ReAssign & "】</table><br>"
                        End If
                    End If
                Next
                tran.Commit()
                Me.TransferFramePage("~/WF/WFA021.aspx", Nothing, "InforPath=改派處理完成", "NextWebPage=~/WF/WFA050.aspx", "Message=" & MessageStr.Replace("=", ":"))
            Catch ex As Exception
                tran.Rollback()
                Bsp.Utility.ShowMessage(Me, Me.FunID & ".ItemCommand", ex)
                Me.TransferFramePage("~/WF/WFA021.aspx", Nothing, "MessageType=ERROR", "InforPath=改派處理", "NextWebPage=~/WF/WFA050.aspx", "Message=" & ex.ToString.Replace("=", ":"))
            Finally
                If cn.State = ConnectionState.Open Then cn.Close()
            End Try
        End Using
    End Sub

    Private Function funCheckData() As Boolean
        Dim strValue As String

        strValue = UserSelect.SelectedUserID
        If strValue = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", "原處理人員不得為空值")
            Return False
        End If

        strValue = OneUserSelectAssign.SelectedUserID
        If strValue = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", "欲改派之處理人員不得為空值")
            Return False
        End If

        strValue = TxtAreaOption.Value
        If strValue = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", "改派意見")
            Return False
        Else
            If Bsp.Utility.getStringLength(strValue) > 100 Then
                Bsp.Utility.ShowFormatMessage(Me, "W_00040", "備註说明", "100")
            End If
        End If
        strValue = selectedRows(gvMain).ToString
        strValue = strValue.Replace(",", "").Trim()
        If strValue = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", "改派之案件未選取")
            Return False
        End If
        Return True
    End Function

    Private Sub DoQuery()
        Dim Applblobj As Label
        Dim lblobj As Label
        Dim selectedRowStr As String = selectedRows(gvMain)
        Dim selectedRowArr() As String = selectedRowStr.Split(",")
        For intLoopA As Integer = 0 To selectedRowArr.GetUpperBound(0)
            If Trim(selectedRowArr(intLoopA).ToString) <> "" Then
                lblobj = gvMain.Rows(selectedRowArr(intLoopA).Split(".")(1)).FindControl("lblFlowKeyStr")
                Applblobj = gvMain.Rows(selectedRowArr(intLoopA).Split(".")(1)).FindControl("APPIDStr")
            End If
        Next
    End Sub

    Private Sub DoDelete()

    End Sub


    Private Sub DoOtherAction(ByVal Param As String)

    End Sub


    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If Request.Params("__EVENTTARGET") IsNot Nothing Then
            Select Case Request.Params("__EVENTTARGET")
                Case "UserSelect_ddlUser", "UserSelect$ddlOrgan", "FlowNameDrpList"
                    clearSelectedRows(gvMain)
            End Select
        End If
        gvMain.DataBind()
    End Sub

    Protected Sub gvMain_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles gvMain.RowEditing
        Dim btnP As New ButtonState(ButtonState.emButtonType.Print)
        Dim btnX As New ButtonState(ButtonState.emButtonType.Exit)
        Dim strURL As String = "~/WF/WFA030.aspx"

        strURL = String.Format("{0}?FlowID={1}", strURL, gvMain.DataKeys(e.NewEditIndex)("FlowID").ToString())
        strURL = String.Format("{0}&FlowCaseID={1}", strURL, gvMain.DataKeys(e.NewEditIndex)("FlowCaseID").ToString())
        strURL = String.Format("{0}&FlowLogID={1}", strURL, gvMain.DataKeys(e.NewEditIndex)("FlowLogID").ToString())

        Me.CallPage(strURL, New ButtonState() {btnP, btnX})
    End Sub

    Private Function getFlowVer(ByVal FlowID As String, ByVal FlowCaseID As String, ByVal tran As DbTransaction) As String
        Dim objWF As New WF

        Using dt As DataTable = objWF.GetFlowCase(FlowID, FlowCaseID, tran)
            If dt.Rows.Count > 0 Then
                Return dt.Rows(0).Item("FlowVer").ToString()
            Else
                Return "1"
            End If
        End Using
    End Function
End Class
