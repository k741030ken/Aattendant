'****************************************************
'功能說明：員工調兼資料維護(EmpAddition)維護-修改
'建立人員：Weicheng
'建立日期：2014/11/07
'****************************************************
Imports System.Data

Partial Class ST_ST1B12
    Inherits PageBase


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then

        End If
    End Sub
    Protected Overrides Sub BaseOnPageTransfer(ByVal ti As TransferInfo)
        If Not IsPostBack() Then

            Dim ht As Hashtable = Bsp.Utility.getHashTableFromParam(ti.Args)

            ViewState.Item("CompID") = ht("SelectedCompID").ToString()
            ViewState.Item("EmpID") = ht("SelectedEmpID").ToString()
            ViewState.Item("ValidDate") = ht("SelectValidDate").ToString()
            ViewState.Item("AddCompID") = ht("SelectAddCompID").ToString()
            ViewState.Item("AddDeptID") = ht("SelectAddDeptID").ToString()
            ViewState.Item("AddOrganID") = ht("SelectAddOrganID").ToString()

            ViewState.Item("PageNo") = ht("PageNo").ToString()

            subGetData(ViewState.Item("CompID"), ViewState.Item("EmpID"), ViewState.Item("ValidDate"), ViewState.Item("AddCompID"), ViewState.Item("AddDeptID"), ViewState.Item("AddOrganID"))


        End If
    End Sub
    Private Sub subGetData(ByVal CompID As String, ByVal EmpID As String, ByVal ValidDate As String, ByVal AddCompID As String, ByVal AddDeptID As String, ByVal AddOrganID As String)
        Dim objHR1600 As New HR1600()
        Dim bsEmpAddition As New beEmpAddition.Service()
        Dim beEmpAddition As New beEmpAddition.Row()

        beEmpAddition.CompID.Value = CompID
        beEmpAddition.EmpID.Value = EmpID
        beEmpAddition.ValidDate.Value = ValidDate
        beEmpAddition.AddCompID.Value = AddCompID
        beEmpAddition.AddDeptID.Value = AddDeptID
        beEmpAddition.AddOrganID.Value = AddOrganID

        Try
            Using dt As DataTable = objHR1600.QueryEmpAdditionByDetail(CompID, EmpID, ValidDate, AddCompID, AddDeptID, AddOrganID)
                If dt.Rows.Count <= 0 Then Exit Sub

                '員工編號
                lblEmpID_S.Text = dt.Rows(0).Item("EmpID").ToString
                '姓名
                lblUserName_S.Text = dt.Rows(0).Item("NameN").ToString
                '現任公司
                lblCompID_S.Text = dt.Rows(0).Item("CompID").ToString & " " & dt.Rows(0).Item("CompName").ToString
                '現任部門/科組課
                lblDeptID_S.Text = dt.Rows(0).Item("DeptID").ToString & " " & dt.Rows(0).Item("DeptName").ToString
                lblOrganID_S.Text = dt.Rows(0).Item("OrganID").ToString & " " & dt.Rows(0).Item("OrganName").ToString
                '子公司職稱(現況)
                lblTitle_S.Text = dt.Rows(0).Item("Title").ToString
                '職位(現況)
                lblPosition_S.Text = dt.Rows(0).Item("Position").ToString
                '工作性質(現況)
                lblWorkType_S.Text = dt.Rows(0).Item("WorkType").ToString
                '金控職等(現況)
                lblHoldingRank_S.Text = dt.Rows(0).Item("HoldingRank").ToString
                '金控職級(現況)
                lblHoldingTitle_S.Text = dt.Rows(0).Item("HoldingTitle").ToString
                '兼任部門主管(現況)
                If dt.Rows(0).Item("AddDeptBoss").ToString = "1" Then
                    chkAdditionDeptBoss.Checked = True
                End If
                '兼任科組課主管(現況)
                If dt.Rows(0).Item("AddOrganBoss").ToString = "1" Then
                    chkAdditionOrganBoss.Checked = True
                End If
                '生效日
                lblValidDate_S.Text = dt.Rows(0).Item("ValidDate").ToString
                '狀態
                lblReason_S.Text = dt.Rows(0).Item("Reason").ToString & " " & dt.Rows(0).Item("ReasonName").ToString
                '人令
                lblFileNO_S.Text = dt.Rows(0).Item("FileNo").ToString
                '兼任公司
                lblAdditionCompID_S.Text = dt.Rows(0).Item("AddCompID").ToString & " " & dt.Rows(0).Item("AddCompName").ToString
                '兼任部門/科組課
                lblAdditionDeptID_S.Text = dt.Rows(0).Item("AddDeptID").ToString & " " & dt.Rows(0).Item("AddDeptName").ToString
                lblAdditionOrganID_S.Text = dt.Rows(0).Item("AddOrganID").ToString & " " & dt.Rows(0).Item("AddOrganName").ToString
                '兼任簽核最小單位
                lblAdditionFlowOrganID_S.Text = dt.Rows(0).Item("AddFlowOrganID").ToString & " " & dt.Rows(0).Item("AddFlowOrganName").ToString
                '主管任用方式
                If dt.Rows(0).Item("BossType").ToString = "1" Then
                    rbnEmpAdditionBossType1.Checked = True
                End If
                If dt.Rows(0).Item("BossType").ToString = "2" Then
                    rbnEmpAdditionBossType2.Checked = True
                End If
                '單位主管
                If dt.Rows(0).Item("IsBoss").ToString = "1" Then
                    chkEmpAdditionIsBoss.Checked = True
                End If
                '單位副主管
                If dt.Rows(0).Item("IsSecBoss").ToString = "1" Then
                    chkEmpAdditionIsSecBoss.Checked = True
                End If
                '簽核單位主管
                If dt.Rows(0).Item("IsGroupBoss").ToString = "1" Then
                    chkEmpAdditionIsGroupBoss.Checked = True
                End If
                '簽核單位副主管
                If dt.Rows(0).Item("IsSecGroupBoss").ToString = "1" Then
                    chkEmpAdditionIsSecGroupBoss.Checked = True
                End If
                '備註
                txtEmpAdditionRemark.Text = dt.Rows(0).Item("Remark").ToString
                ViewState.Item("Remark") = dt.Rows(0).Item("Remark").ToString
                '建檔日期
                lblCreateDate_S.Text = dt.Rows(0).Item("CreateDate").ToString
                '建檔人員
                lblCreateID_S.Text = dt.Rows(0).Item("CreateID").ToString
                '最後異動日期
                lblLastChgDate_S.Text = dt.Rows(0).Item("LastChgDate").ToString
                '最後異動人員
                lblLastChgID_S.Text = dt.Rows(0).Item("LastChgID").ToString

            End Using


        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & ".GetData", ex)
        End Try

    End Sub
    Public Overrides Sub DoAction(ByVal Param As String)
        Select Case Param
            Case "btnActionX"   '返回
                GoBack()
        End Select
    End Sub

    Private Sub GoBack()
        Dim ti As TransferInfo = Me.StateTransfer
        Me.TransferFramePage(ti.CallerUrl, Nothing, ti.Args)
    End Sub





End Class
