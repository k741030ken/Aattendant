'****************************************************
'功能說明：WF_LoanChecker維護
'建立人員：Tsao
'建立日期：2013/03/XX
'****************************************************
Imports System.Data.SqlClient
Imports System.Data
Imports System.Data.Common

Partial Class WF_WFA080
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Overrides Sub BaseOnPageTransfer(ti As TransferInfo)
        If Not IsPostBack Then

            Object2ViewState(ti.Args)

            ViewState("SetupStepID") = ViewState("FlowStepID")
            If ViewState("ReLoadLeftFlag") <> "1" Then
                'ReLoadLeftListBox(ViewState("FlowCaseID").ToString(), ViewState("FlowID").ToString())
            End If
            ViewState("ReLoadLeftFlag") = "0"

        End If

    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If Not IsPostBack Then
            BindData()
        End If
    End Sub

    Private Sub BindData()
        Dim objWF As New WF
        Dim strSQL As New StringBuilder
        strSQL.AppendLine("Select Case Isnull(a.LoanCheckdateEnd, '') ")
        strSQL.AppendLine("     When '' Then Case Isnull(a.LoanCheckdateBeg, '') When '' Then '' Else '審查中' End ")
        strSQL.AppendLine("     Else '已審查' End as CASEState, a.FlowCaseID, a.LoanCheckID + '-' + Isnull(b.UserName, '') UserName,")
        strSQL.AppendLine("     dbo.funGetAOrgDefine('2', b.DeptID) OrganName, a.LoanCheckID ")
        strSQL.AppendLine("From WF_LoanChecker a left outer join SC_User b on a.LoanCheckID = b.UserID ")
        strSQL.AppendLine("Where FlowID = " & Bsp.Utility.Quote(ViewState("FlowID")))
        strSQL.AppendLine("  And FlowCaseID = " & Bsp.Utility.Quote(ViewState("FlowCaseID")))
        strSQL.AppendLine("  And a.SetupStepID = " & Bsp.Utility.Quote(ViewState("SetupStepID")))
        strSQL.AppendLine(" Order by a.FlowSeq")
        StateMain = strSQL.ToString()
        If StateMain IsNot Nothing Then
            sdsMain.SelectCommand = StateMain
        End If

        LoadListBox(ViewState("FlowCaseID").ToString(), ViewState("FlowID").ToString())
        LogGridView.DataSource = objWF.WF_LoanCheckerLogList(ViewState("FlowID"), ViewState("FlowCaseID"), ViewState("SetupStepID")).Tables(0)
        LogGridView.DataBind()

        WF.fillOrganByValue(ddlOrgan, "", "", " and OrganID in (SELECT  DeptID  FROM  SC_User Where BanMark = '0') ")
        ddlOrgan.Items.Insert(0, New ListItem("---所有部門---", ""))
        Bsp.Utility.IniListWithValue(ddlOrgan, UserProfile.OrganID)
        ReLoadLeftListBox(ViewState("FlowCaseID").ToString(), ViewState("FlowID").ToString())

        Try
            '權限核委設定
            'Dim objWF As New WF
            'If ti.Args.Length > 0 Then
            '    For intLoop As Integer = 0 To ti.Args.Length - 1
            '        ViewState(ti.Args(intLoop).ToString().Split("=")(0)) = ti.Args(intLoop).ToString().Split("=")(1)
            '    Next
            'End If
            Using dt As DataTable = objWF.ToDisUserQTable(ViewState("FlowID"), ViewState("FlowCaseID"), ViewState("FlowLogBatNo"), ViewState("FlowLogID"))
                If dt.Rows.Count > 0 Then
                    ViewState("AppID") = dt.Rows(0).Item("FlowKeyValue").ToString.Split("&")(0).Split("=")(1)
                Else
                    If ViewState("FlowCaseID").ToString().Substring(0, 2) = "BT" Then
                        ViewState("AppID") = ViewState("FlowCaseID")
                    End If
                End If
            End Using
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & ".BaseOnPageTransfer", ex)
        End Try
    End Sub

    Private Sub LoadListBox(ByVal FlowCaseID As String, ByVal FlowID As String)
        Dim objWF As New WF
        Dim dt As DataTable = Nothing
        Dim dt1 As DataTable = Nothing
        Try

            'dt = objWF.getSC_LoanChecker(FlowCaseID, ViewState("SetupStepID"), ddlOrgan.SelectedValue, "", FlowID).Tables(0)
            'dt1 = objWF.getSC_LoanChecker_Out(FlowCaseID, ViewState("SetupStepID"), FlowID).Tables(0)

            dt = objWF.getSC_LoanChecker_Out(FlowCaseID, ViewState("SetupStepID"), ddlOrgan.SelectedValue, FlowID).Tables(0)
            dt1 = objWF.getSC_LoanChecker_In(FlowCaseID, ViewState("SetupStepID"), ddlOrgan.SelectedValue, FlowID).Tables(0)

            UcMultiSelect1.LoadLeftData(dt)
            UcMultiSelect1.LoadRightData(dt1)
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, "WFA080", ex)
        Finally
            If dt IsNot Nothing Then dt.Dispose()
            If dt1 IsNot Nothing Then dt1.Dispose()
        End Try
    End Sub

    Private Sub ReLoadLeftListBox(ByVal strCaseNo As String, ByVal FlowID As String)
        Dim objWF As New WF
        Dim dt As DataTable = Nothing
        Dim dt1 As DataTable = Nothing
        Try
            dt = objWF.getSC_LoanChecker_Out(strCaseNo, ViewState("SetupStepID"), ddlOrgan.SelectedValue, FlowID).Tables(0)
            dt1 = objWF.getSC_LoanChecker_In(strCaseNo, ViewState("SetupStepID"), ddlOrgan.SelectedValue, FlowID).Tables(0)

            UcMultiSelect1.LoadLeftData(dt)
            UcMultiSelect1.LoadRightData(dt1)
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, "WFA080", ex)
        Finally
            If dt IsNot Nothing Then dt.Dispose()
        End Try
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
                DoOtherAction(Param)   '其他功能動作
        End Select
    End Sub

    Private Sub DoAdd()
        'Me.TransferFramePage("~/WF/CCA051.aspx", "btnAdd")
    End Sub

    Private Sub DoUpdate()
        'Me.TransferFramePage("~/WF/CCA052.aspx", "btnUpdate")
    End Sub

    Private Sub DoQuery()

    End Sub

    Private Sub DoDelete()

    End Sub

    Private Sub DoOtherAction(ByVal Param As String)
        Select Case Param
            Case "btnActionC"
                Dim objWF As New WF
                Dim LoanChecker As String = UcMultiSelect1.ValueResult

                Try
                    objWF.ReBuileLoanChecker(ViewState("FlowCaseID"), "1", ViewState("SetupStepID"), LoanChecker, ViewState("FlowID").ToString())
                    'gvMain.DataBind()
                    If StateMain IsNot Nothing Then
                        sdsMain.SelectCommand = StateMain
                    End If
                    LogGridView.DataSource = objWF.WF_LoanCheckerLogList(ViewState("FlowID"), ViewState("FlowCaseID"), ViewState("SetupStepID")).Tables(0)
                    LogGridView.DataBind()
                Catch ex As Exception
                    Bsp.Utility.ShowMessage(Me, "WFA080.DoOtherAction", ex)
                End Try
        End Select
    End Sub

    Protected Sub gvMain_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvMain.RowCommand

    End Sub


    Protected Sub gvMain_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvMain.SelectedIndexChanged

    End Sub

    Sub WFSortACt(ByVal sender As Object, ByVal e As CommandEventArgs)
        Select Case e.CommandName
            Case "WF_SortDown"
                Response.Write(e.CommandArgument)
            Case "WF_SortUp"
                Response.Write(e.CommandArgument)
        End Select

    End Sub

    Protected Sub ddlOrgan_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlOrgan.SelectedIndexChanged
        ReLoadLeftListBox(ViewState("FlowCaseID").ToString(), ViewState("FlowID").ToString())
        ViewState("ReLoadLeftFlag") = "1"
    End Sub
End Class
