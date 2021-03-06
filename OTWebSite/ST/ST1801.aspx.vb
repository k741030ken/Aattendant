'****************************************************
'功能說明：員工企業團經歷維護-新增
'建立人員：Micky Sung
'建立日期：2015.07.14
'****************************************************
Imports System.Data
Imports Newtonsoft.Json

Partial Class ST_ST1801
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then
            '異動原因
            Bsp.Utility.FillDDL(ddlReason, "eHRMSDB", "EmployeeReason", "RTrim(Reason)", "Remark")
            ddlReason.Items.Insert(0, New ListItem("---請選擇---", ""))

            '部門名稱
            Bsp.Utility.FillHRCompany(ddlCompIDOld)
            ddlCompIDOld.Items.Insert(0, New ListItem("---請選擇---", ""))
            Bsp.Utility.FillHRCompany(ddlCompID)
            ddlCompID.Items.Insert(0, New ListItem("---請選擇---", ""))
        End If
    End Sub

    Protected Overrides Sub BaseOnPageTransfer(ByVal ti As TransferInfo)
        If Not IsPostBack() Then
            Dim objSC As New SC
            Dim objST As New ST1
            Dim ht As Hashtable = Bsp.Utility.getHashTableFromParam(ti.Args)

            '公司代碼
            If ht.ContainsKey("SelectedCompID") Then
                ViewState.Item("CompID") = ht("SelectedCompID").ToString()
            Else
                Return
            End If

            '員工編號
            If ht.ContainsKey("SelectedEmpID") Then
                ViewState.Item("EmpID") = ht("SelectedEmpID").ToString()
            Else
                Return
            End If

            '員工姓名
            If ht.ContainsKey("SelectedEmpName") Then
                ViewState.Item("EmpName") = ht("SelectedEmpName").ToString()
            Else
                Return
            End If

            '員工身分證字號
            If ht.ContainsKey("SelectedIDNo") Then
                ViewState.Item("IDNo") = ht("SelectedIDNo").ToString()
            Else
                Return
            End If

            txtCompID.Text = ViewState.Item("CompID").ToString() + "-" + objSC.GetCompName(ViewState.Item("CompID").ToString()).Rows(0).Item("CompName").ToString
            txtEmpID.Text = ViewState.Item("EmpID").ToString()
            txtEmpName.Text = ViewState.Item("EmpName").ToString()

            '到職日
            hidEmpDate.Value = objST.selectEmpDate(ViewState.Item("CompID"), txtEmpID.Text).Rows(0).Item(0)

            '職位
            ddlPositionIDOld.Items.Insert(0, New ListItem("---請選擇---", ""))
            ddlPositionID.Items.Insert(0, New ListItem("---請選擇---", ""))

            '部門/科組課
            ucSelectHROrganOld.LoadData(ddlCompIDOld.SelectedValue, "Y")
            ucSelectHROrgan.LoadData(ddlCompID.SelectedValue, "Y")

            '工作性質
            ddlWorkTypeIDOld.Items.Insert(0, New ListItem("---請選擇---", ""))
            ddlWorkTypeID.Items.Insert(0, New ListItem("---請選擇---", ""))

            '職等/職稱
            ucSelectRankAndTitleOld.LoadData(ddlCompIDOld.SelectedValue, "A")
            ucSelectRankAndTitle.LoadData(ddlCompID.SelectedValue, "A")

            '簽核最小單位
            ucSelectFlowOrganOld.LoadData(ucSelectHROrganOld.SelectedOrganID, "Y")
            ucSelectFlowOrgan.LoadData(ucSelectHROrganOld.SelectedOrganID, "Y")

            '任職狀況
            HR.FillWorkStatus(ddlWorkStatusOld)
            ddlWorkStatusOld.Items.Insert(0, New ListItem("---請選擇---", ""))
        End If
    End Sub

    Public Overrides Sub DoAction(ByVal Param As String)
        Select Case Param
            Case "btnAdd"   '存檔返回
                If funCheckData() Then
                    ViewState.Item("Action") = "btnAdd"
                    Release("btnAdd")
                End If
            Case "btnActionX"   '返回
                GoBack()
            Case "btnCancel"    '清除
                ClearData()
        End Select
    End Sub

    Private Sub GoBack()
        Dim ti As TransferInfo = Me.StateTransfer
        Me.TransferFramePage(ti.CallerUrl, Nothing, ti.Args)
    End Sub

    Private Function SaveData() As Boolean
        Dim beEmployeeLog As New beEmployeeLog.Row()
        Dim bsEmployeeLog As New beEmployeeLog.Service()
        Dim objST As New ST1
        Dim objHR As New HR
        Dim objHR3000 As New HR3000

        '儲存資料
        Try
            '2015/08/03 Add 查詢序號
            Dim SeqTable As DataTable
            SeqTable = objST.selectSeq(ViewState.Item("IDNo"), ucValidDate.DateText)
            If SeqTable.Rows().Count > 0 Then
                beEmployeeLog.Seq.Value = Int(SeqTable.Rows(0).Item(0).ToString) + 1
            Else
                beEmployeeLog.Seq.Value = "1"
            End If

            beEmployeeLog.IDNo.Value = ViewState.Item("IDNo")
            beEmployeeLog.ModifyDate.Value = ucValidDate.DateText
            beEmployeeLog.Reason.Value = ddlReason.SelectedValue
            beEmployeeLog.Remark.Value = txtRemark.Text
            If txtDueDate.DateText <> "" And txtDueDate.DateText <> "____/__/__" Then
                beEmployeeLog.DueDate.Value = txtDueDate.DateText
            End If
            beEmployeeLog.PWID.Value = ""
            beEmployeeLog.PW.Value = ""
            beEmployeeLog.IsBoss.Value = IIf(chkIsBoss.Checked, "1", "0")
            beEmployeeLog.IsSecBoss.Value = IIf(chkIsSecBoss.Checked, "1", "0")
            beEmployeeLog.IsGroupBoss.Value = IIf(chkIsGroupBoss.Checked, "1", "0")
            beEmployeeLog.IsSecGroupBoss.Value = IIf(chkIsSecGroupBoss.Checked, "1", "0")
            beEmployeeLog.BossType.Value = ddlBossType.SelectedValue
            beEmployeeLog.LastChgComp.Value = UserProfile.ActCompID
            beEmployeeLog.LastChgID.Value = UserProfile.ActUserID
            beEmployeeLog.LastChgDate.Value = Now

            '異動後
            beEmployeeLog.CompID.Value = ddlCompID.SelectedValue
            beEmployeeLog.EmpID.Value = txtEmpID.Text
            beEmployeeLog.Company.Value = ddlCompID.SelectedItem.Text
            beEmployeeLog.DeptID.Value = ucSelectHROrgan.SelectedDeptID
            beEmployeeLog.DeptName.Value = ucSelectHROrgan.SelectedDeptName
            beEmployeeLog.OrganID.Value = ucSelectHROrgan.SelectedOrganID
            beEmployeeLog.OrganName.Value = ucSelectHROrgan.SelectedOrganName
            beEmployeeLog.GroupID.Value = objST.QueryData("Organization", "And CompID = " & Bsp.Utility.Quote(ddlCompID.SelectedValue) & " And OrganID = " & Bsp.Utility.Quote(ucSelectHROrgan.SelectedOrganID), "GroupID")
            beEmployeeLog.GroupName.Value = objST.QueryData("OrganizationFlow", "And OrganID = " & Bsp.Utility.Quote(ucSelectHROrgan.SelectedOrganID), "OrganName")
            beEmployeeLog.FlowOrganID.Value = ucSelectFlowOrgan.SelectedOrganID
            beEmployeeLog.FlowOrganName.Value = ucSelectFlowOrgan.SelectedOrganIDName
            beEmployeeLog.RankID.Value = ucSelectRankAndTitle.SelectedRankID
            beEmployeeLog.TitleID.Value = ucSelectRankAndTitle.SelectedTitleID
            beEmployeeLog.TitleName.Value = ucSelectRankAndTitle.SelectedTitleName

            beEmployeeLog.PositionID.Value = hidPositionID.Value
            beEmployeeLog.Position.Value = lblSelectPositionName.Text

            beEmployeeLog.WorkTypeID.Value = hidWorkTypeID.Value
            beEmployeeLog.WorkType.Value = lblSelectWorkTypeName.Text

            beEmployeeLog.WorkStatus.Value = hidWorkStatus.Value
            beEmployeeLog.WorkStatusName.Value = hidWorkStatusName.Value

            '異動前
            beEmployeeLog.CompIDOld.Value = ddlCompIDOld.SelectedValue
            beEmployeeLog.CompanyOld.Value = IIf(ddlCompIDOld.SelectedValue <> "", ddlCompIDOld.SelectedItem.Text, "")
            beEmployeeLog.DeptIDOld.Value = ucSelectHROrganOld.SelectedDeptID
            beEmployeeLog.DeptNameOld.Value = ucSelectHROrganOld.SelectedDeptName
            beEmployeeLog.OrganIDOld.Value = ucSelectHROrganOld.SelectedOrganID
            beEmployeeLog.OrganNameOld.Value = ucSelectHROrganOld.SelectedOrganName
            beEmployeeLog.GroupIDOld.Value = objST.QueryData("Organization", "And CompID = " & Bsp.Utility.Quote(ddlCompIDOld.SelectedValue) & " And OrganID = " & Bsp.Utility.Quote(ucSelectHROrganOld.SelectedOrganID), "GroupID")
            beEmployeeLog.GroupNameOld.Value = objST.QueryData("OrganizationFlow", "And OrganID = " & Bsp.Utility.Quote(ucSelectHROrganOld.SelectedOrganID), "OrganName")
            beEmployeeLog.FlowOrganIDOld.Value = ucSelectFlowOrganOld.SelectedOrganID
            beEmployeeLog.FlowOrganNameOld.Value = IIf(ucSelectFlowOrganOld.SelectedOrganID <> "", ucSelectFlowOrganOld.SelectedOrganIDName, "")
            beEmployeeLog.RankIDOld.Value = ucSelectRankAndTitleOld.SelectedRankID
            beEmployeeLog.TitleIDOld.Value = ucSelectRankAndTitleOld.SelectedTitleID
            beEmployeeLog.TitleNameOld.Value = IIf(ucSelectRankAndTitleOld.SelectedTitleID <> "", ucSelectRankAndTitleOld.SelectedTitleName, "")

            beEmployeeLog.PositionIDOld.Value = hidPositionIDOld.Value
            beEmployeeLog.PositionOld.Value = IIf(hidPositionIDOld.Value <> "", lblSelectPositionNameOld.Text, "")

            beEmployeeLog.WorkTypeIDOld.Value = hidWorkTypeIDOld.Value
            beEmployeeLog.WorkTypeOld.Value = IIf(hidWorkTypeIDOld.Value <> "", lblSelectWorkTypeNameOld.Text, "")

            beEmployeeLog.WorkStatusOld.Value = ddlWorkStatusOld.SelectedValue
            If ddlWorkStatusOld.SelectedValue <> "" Then
                Dim arrWorkStatus As String()
                arrWorkStatus = ddlWorkStatusOld.SelectedItem.Text.Split(" ")
                beEmployeeLog.WorkStatusNameOld.Value = arrWorkStatus(1)
            Else
                beEmployeeLog.WorkStatusNameOld.Value = ""
            End If

            Return objST.AddEmployeeLogSetting(beEmployeeLog)
        Catch ex As Exception
            Dim errLine As Integer = Convert.ToInt32(ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(" ")))
            Bsp.Utility.ShowMessage(Me, "[SaveData]" & errLine & Bsp.Utility.getMessage("E_00000") & Bsp.Utility.getInnerException(Me.FunID, ex))
            Return False
        End Try
    End Function

    Private Function funCheckData() As Boolean
        Dim objHR As New HR()
        Dim objST As New ST1()
        Dim beEmployeeLog As New beEmployeeLog.Row()
        Dim bsEmployeeLog As New beEmployeeLog.Service()
        Dim strReason As String = ddlReason.SelectedValue '異動原因
        Dim strWorkStatus As String = hidWorkStatus.Value '異動後任職狀況

        '異動原因
        If strReason = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblReason.Text)
            ddlReason.Focus()
            Return False
        End If

        '生效日期
        If ucValidDate.DateText = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblModifyDate.Text)
            ucValidDate.Focus()
            Return False
        Else
            If Not IsDate(ucValidDate.DateText) Then
                Bsp.Utility.ShowFormatMessage(Me, "W_00070", lblModifyDate.Text)
                ucValidDate.Focus()
                Return False
            Else
                If CDate(ucValidDate.DateText) < CDate(hidEmpDate.Value) Then
                    Bsp.Utility.ShowMessage(Me, "生效日期：輸入錯誤，必須大於到職日")
                    ucValidDate.Focus()
                    Return False
                End If
            End If
        End If

        '生效迄日
        If strReason = "12" Or strReason = "18" Then
            If txtDueDate.DateText = "" Then
                Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblDueDate.Text)
                txtDueDate.Focus()
                Return False
            Else
                If CDate(ucValidDate.DateText) >= CDate(txtDueDate.DateText) Then
                    Bsp.Utility.ShowMessage(Me, "「生效迄日」必須大於生效日期")
                    txtDueDate.Focus()
                    Return False
                End If
            End If
        End If

        '備註
        If txtRemark.Text.Trim.Length > txtRemark.MaxLength Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00040", lblRemark.Text, txtRemark.MaxLength.ToString)
            txtRemark.Focus()
            Return False
        End If

        If strReason <> "01" Then
            '(前)公司名稱
            If ddlCompIDOld.SelectedValue = "" Then
                Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblBeforeInfo.Text & "-" & lblCompIDOld.Text)
                ddlCompIDOld.Focus()
                Return False
            End If

            '(前)部門
            If ucSelectHROrganOld.SelectedDeptID = "" Then
                Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblBeforeInfo.Text & "-" & "部門")
                ucSelectHROrganOld.Focus()
                Return False
            End If

            '(前)科組課
            If ucSelectHROrganOld.SelectedOrganID = "" Then
                Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblBeforeInfo.Text & "-" & "科/組/課")
                ucSelectHROrganOld.Focus()
                Return False
            End If

            '(前)簽核最小單位
            If ucSelectFlowOrganOld.SelectedOrganID = "" Then
                Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblBeforeInfo.Text & "-" & lblFlowOrganIDOld.Text)
                ucSelectFlowOrganOld.Focus()
                Return False
            End If

            '(前)任職狀況
            If ddlWorkStatusOld.SelectedValue = "" Then
                Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblBeforeInfo.Text & "-" & lblWorkStatusOld.Text)
                ddlWorkStatusOld.Focus()
                Return False
            End If

            '(前)職等/職稱
            If ucSelectRankAndTitleOld.SelectedRankID <> "" Then
                If ucSelectRankAndTitleOld.SelectedTitleID = "" And ucSelectRankAndTitleOld.SelectedTitleName = "" Then
                    Bsp.Utility.ShowMessage(Me, lblBeforeInfo.Text & "-" & "職等不為空白時，職稱不可為空白")
                    ucSelectRankAndTitleOld.Focus()
                    Return False
                End If
            End If

            '(前)職位  2015/08/14 Add
            Dim RankIDMapValidDateOld As String = objST.QueryData("Company", " AND CompID = " & Bsp.Utility.Quote(ddlCompIDOld.SelectedValue), "RankIDMapValidDate")
            If objHR.IsRankIDMapFlag(ddlCompIDOld.SelectedValue) And hidBeforeDueDateOld.Value >= RankIDMapValidDateOld And ddlReason.SelectedValue <> "70" And hidPositionIDOld.Value = "" Then
                Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblBeforeInfo.Text & "-" & lblPositionIDOld.Text)
                ddlPositionIDOld.Focus()
                Return False
            End If
            If ddlPositionIDOld.Items.Count > 5 Then
                Bsp.Utility.ShowMessage(Me, lblBeforeInfo.Text & "-" & "職位不可超過5個")
                ddlPositionIDOld.Focus()
                Return False
            End If

            '(前)工作性質  2015/08/14 Add
            If ddlWorkTypeIDOld.Items.Count > 5 Then
                Bsp.Utility.ShowMessage(Me, lblBeforeInfo.Text & "-" & "工作性質不可超過5個")
                ddlWorkTypeIDOld.Focus()
                Return False
            End If

        End If

        '(後)公司名稱
        If ddlCompID.SelectedValue = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblAfterInfo.Text & "-" & lblCompID.Text)
            ddlCompID.Focus()
            Return False
        End If
        If strReason = "70" Then
            If ddlCompIDOld.SelectedValue = ddlCompID.SelectedValue Then
                Bsp.Utility.ShowMessage(Me, lblAfterInfo.Text & "-" & "公司：輸入錯誤，異動原因為70時，前後公司不得相同！")
                ddlCompID.Focus()
                Return False
            End If
        End If
        If strReason = "33" Or strReason = "50" Then
            If ddlCompIDOld.SelectedValue <> ddlCompID.SelectedValue Then
                Bsp.Utility.ShowMessage(Me, lblAfterInfo.Text & "-" & "公司：輸入錯誤，異動原因為33或50時，前後公司必須相同！")
                ddlCompID.Focus()
                Return False
            End If
        End If

        '(後)職位
        '2015/11/27 Modify 改檢核
        'If ucValidDate.DateText > "2013/10/01" And objHR.IsRankIDMapFlag(ddlCompID.SelectedValue) Then
        '    If Len(hidPositionID.Value) = 0 And strReason <> "70" And ddlPositionID.SelectedValue = "" Then
        '        Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblAfterInfo.Text & "-" & lblPositionID.Text)
        '        ddlPositionID.Focus()
        '        Return False
        '    Else
        '        If ddlPositionID.Items.Count > 10 Then
        '            Bsp.Utility.ShowMessage(Me, lblAfterInfo.Text & "-" & "職位不可超過10個")
        '            ddlPositionID.Focus()
        '            Return False
        '        End If
        '        If Not (strWorkStatus = "2" Or strWorkStatus = "3" Or strWorkStatus = "6") Then
        '            For Each listItmValue As ListItem In ddlPositionID.Items
        '                If Not objHR.ChkPositionIsVlaid(ddlCompID.SelectedValue.Trim, listItmValue.Value) And listItmValue.Value <> "" Then
        '                    Bsp.Utility.ShowMessage(Me, lblAfterInfo.Text & "-" & "職位：所選職位中有無效職位！")
        '                    ddlPositionID.Focus()
        '                    Return False
        '                End If
        '            Next
        '        End If
        '    End If
        'End If
        Dim RankIDMapValidDate As String = objST.QueryData("Company", " AND CompID = " & Bsp.Utility.Quote(ddlCompID.SelectedValue), "RankIDMapValidDate")
        If objHR.IsRankIDMapFlag(ddlCompID.SelectedValue) And ucValidDate.DateText >= RankIDMapValidDate And ddlReason.SelectedValue <> "70" And hidPositionID.Value = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblAfterInfo.Text & "-" & lblPositionID.Text)
            ddlPositionID.Focus()
            Return False
        End If
        If ddlPositionID.Items.Count > 5 Then
            Bsp.Utility.ShowMessage(Me, lblAfterInfo.Text & "-" & "職位不可超過5個")
            ddlPositionID.Focus()
            Return False
        End If


        '(後)部門
        If ucSelectHROrgan.SelectedDeptID = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblAfterInfo.Text & "-" & "部門")
            ucSelectHROrgan.Focus()
            Return False
        Else
            '2015/11/27 Modify
            'If Not (strWorkStatus = "2" Or strWorkStatus = "3" Or strWorkStatus = "6") Then
            '    If Not objHR.ChkOrganIsVlaid(ddlCompID.SelectedValue, ucSelectHROrgan.SelectedDeptID) Then
            '        Bsp.Utility.ShowMessage(Me, lblAfterInfo.Text & "-" & "部門：所選擇的部門是無效單位！")
            '        Return False
            '    End If
            'End If
            If strReason = "33" Then
                If ucSelectHROrgan.SelectedDeptID <> ucSelectHROrganOld.SelectedDeptID Then
                    Bsp.Utility.ShowMessage(Me, lblAfterInfo.Text & "-" & "部門：輸入錯誤，異動原因為33時，前後部門需相同！")
                    Return False
                End If
            End If
            If strReason = "50" Then
                If ucSelectHROrgan.SelectedDeptID = ucSelectHROrganOld.SelectedDeptID Then
                    Bsp.Utility.ShowMessage(Me, lblAfterInfo.Text & "-" & "部門：輸入錯誤，異動原因為50時，前後部門不得相同！")
                    Return False
                End If
            End If
        End If

        '(後)科組課
        If ucSelectHROrgan.SelectedOrganID = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblAfterInfo.Text & "-" & "科/組/課")
            ucSelectHROrgan.Focus()
            Return False
        Else
            '2015/11/27 Modify
            'If Not (strWorkStatus = "2" Or strWorkStatus = "3" Or strWorkStatus = "6") Then
            '    If Not objHR.ChkOrganIsVlaid(ddlCompID.SelectedValue, ucSelectHROrgan.SelectedOrganID) Then
            '        Bsp.Utility.ShowMessage(Me, lblAfterInfo.Text & "-" & "科/組/課：所選擇科/組/課的是無效單位！")
            '        Return False
            '    End If
            'End If
        End If

        '(後)工作性質
        If Len(hidWorkTypeID.Value) = 0 And strReason <> "70" And ddlWorkTypeID.SelectedValue = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblAfterInfo.Text & "-" & lblWorkTypeID.Text)
            ddlWorkTypeID.Focus()
            Return False
        Else
            If ddlWorkTypeID.Items.Count > 5 Then
                Bsp.Utility.ShowMessage(Me, lblAfterInfo.Text & "-" & "工作性質不可超過5個")
                ddlWorkTypeID.Focus()
                Return False
            End If
            '2015/11/27 Modify
            'For Each listItmValue As ListItem In ddlWorkTypeID.Items
            '    If Not (strWorkStatus = "2" Or strWorkStatus = "3" Or strWorkStatus = "6") Then
            '        If Not objHR.ChkWorkTypeIsVlaid(ddlCompID.SelectedValue.Trim, listItmValue.Value) And listItmValue.Value <> "" Then
            '            Bsp.Utility.ShowMessage(Me, lblAfterInfo.Text & "-" & "工作性質：所選工作性質中有無效工作性質！")
            '            ddlWorkTypeID.Focus()
            '            Return False
            '        End If
            '    End If
            'Next
        End If

        '(後)職等/職稱
        If hidWorkStatus.Value = "1" Or hidWorkStatus.Value = "7" Then
            If ucSelectRankAndTitle.SelectedRankID = "" Then
                Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblAfterInfo.Text & "-" & "職等")
                ucSelectRankAndTitle.Focus()
                Return False
            End If
            If ucSelectRankAndTitle.SelectedTitleID = "" Then
                Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblAfterInfo.Text & "-" & "職稱")
                ucSelectRankAndTitle.Focus()
                Return False
            End If
        End If
        If ucSelectRankAndTitle.SelectedRankID <> "" Then
            If ucSelectRankAndTitle.SelectedTitleID = "" And ucSelectRankAndTitle.SelectedTitleName = "" Then
                Bsp.Utility.ShowMessage(Me, lblAfterInfo.Text & "-" & "職等不為空白時，職稱不可為空白")
                ucSelectRankAndTitle.Focus()
                Return False
            End If
        End If

        '簽核最小單位
        If ucSelectFlowOrgan.SelectedOrganID = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblAfterInfo.Text & "-" & lblFlowOrganID.Text)
            ucSelectFlowOrgan.Focus()
            Return False
        Else
            '2015/11/27 Modify
            'If Not (strWorkStatus = "2" Or strWorkStatus = "3" Or strWorkStatus = "6") Then
            '    If Not objHR.ChkOrganFlowIsVlaid(ddlCompID.SelectedValue, ucSelectFlowOrgan.SelectedOrganID) Then
            '        Bsp.Utility.ShowMessage(Me, lblAfterInfo.Text & "-" & "最小簽核單位：所選擇的最小簽核部門是無效單位！")
            '        ucSelectFlowOrgan.Focus()
            '        Return False
            '    End If
            'End If
        End If

        '主管任用方式
        If ddlBossType.SelectedValue <> "" Then
            If chkIsBoss.Checked = False And chkIsGroupBoss.Checked = False Then
                Bsp.Utility.ShowFormatMessage(Me, "H_00000", "已選擇主管任用方式，請至少選擇一個單位主管或簽核單位主管")
                ddlBossType.Focus()
                Return False
            End If
        End If
        If Not (chkIsGroupBoss.Checked Or chkIsBoss.Checked) Then
            If chkIsBoss.Checked Or chkIsGroupBoss.Checked Then
                Bsp.Utility.ShowFormatMessage(Me, "H_00000", "已選擇單位主管或簽核單位主管，請選擇主管任用方式")
                ddlBossType.Focus()
                Return False
            End If
        End If

        '單位
        If ddlWorkTypeID.SelectedValue = "BA0001" And chkIsBoss.Checked = False Then
            Bsp.Utility.ShowFormatMessage(Me, "H_00000", "異動後工作性質為單位主管，區督導，分行經理時，必須要勾選主管註記&簽核單位主管註記")
            chkIsBoss.Focus()
            Return False
        End If

        '簽核單位
        If ddlWorkTypeID.SelectedValue = "BA0001" And chkIsGroupBoss.Checked = False Then
            Bsp.Utility.ShowFormatMessage(Me, "H_00000", "異動後工作性質為單位主管，區督導，分行經理時，必須要勾選主管註記&簽核單位主管註記")
            chkIsGroupBoss.Focus()
            Return False
        End If

        Return True
    End Function

    Private Sub ClearData()
        ddlReason.SelectedValue = ""
        ucValidDate.DateText = ""
        txtDueDate.DateText = ""
        txtRemark.Text = ""
        ddlCompIDOld.SelectedValue = ""
        ddlPositionIDOld.Items.Clear()
        ddlPositionIDOld.Items.Insert(0, New ListItem("---請選擇---", ""))
        ucSelectHROrganOld.LoadData(ddlCompIDOld.SelectedValue, "Y")
        ddlWorkTypeIDOld.Items.Clear()
        ddlWorkTypeIDOld.Items.Insert(0, New ListItem("---請選擇---", ""))
        ucSelectRankAndTitleOld.LoadData(ddlCompIDOld.SelectedValue, "A")
        ucSelectFlowOrganOld.LoadData(ucSelectHROrganOld.SelectedOrganID, "Y")
        ddlWorkStatusOld.SelectedValue = ""
        ddlCompID.SelectedValue = ""
        ddlPositionID.Items.Clear()
        ddlPositionID.Items.Insert(0, New ListItem("---請選擇---", ""))
        ucSelectHROrgan.LoadData(ddlCompID.SelectedValue, "Y")
        ddlWorkTypeID.Items.Clear()
        ddlWorkTypeID.Items.Insert(0, New ListItem("---請選擇---", ""))
        ucSelectRankAndTitle.LoadData(ddlCompID.SelectedValue, "A")
        ucSelectFlowOrgan.LoadData(ucSelectHROrganOld.SelectedOrganID, "Y")
        txtWorkStatus.Text = ""
        hidWorkStatus.Value = ""
        hidWorkStatusName.Value = ""
        hidPositionID.Value = ""
        hidWorkTypeID.Value = ""
        hidPositionIDOld.Value = ""
        hidWorkTypeIDOld.Value = ""
        ddlBossType.SelectedValue = ""
        chkIsBoss.Checked = False
        chkIsSecBoss.Checked = False
        chkIsGroupBoss.Checked = False
        chkIsSecGroupBoss.Checked = False
    End Sub

    Public Overrides Sub DoModalReturn(ByVal returnValue As String)
        Dim strSql As String = ""
        Dim strRstName1 As String = ""
        Dim strRstName2 As String = ""

        If returnValue <> "" Then
            Dim aryData() As String = returnValue.Split(":")

            Select Case aryData(0)
                Case "ucRelease"
                    Dim aryValue() As String = Split(aryData(1), "|$|")
                    If aryValue(0) = "Y" Then
                        Select Case ViewState.Item("Action")
                            Case "btnAdd"
                                If SaveData() Then
                                    GoBack()
                                End If
                        End Select
                    End If
                Case "ucPositionIDOld"
                    lblSelectPositionOld.Text = aryData(1)
                    If lblSelectPositionOld.Text <> "''" Then  '非必填時，回傳空值
                        '載入 職位 下拉式選單
                        strSql = " and PositionID in (" + lblSelectPositionOld.Text + ") and CompID = '" + ddlCompIDOld.SelectedValue + "'"
                        Bsp.Utility.Position(ddlPositionIDOld, "PositionID", , strSql)
                        '第一筆為主要職位
                        Dim strDefaultValue() As String = lblSelectPositionOld.Text.Replace("'", "").Split(",")
                        Dim strPosition As String = ""
                        Bsp.Utility.SetSelectedIndex(ddlPositionIDOld, strDefaultValue(0))
                        For intLoop As Integer = 0 To strDefaultValue.GetUpperBound(0)
                            If intLoop = 0 Then
                                strPosition = "1|" + strDefaultValue(intLoop)
                            Else
                                strPosition = strPosition + "|0|" + strDefaultValue(intLoop)
                            End If
                        Next
                        hidPositionIDOld.Value = strPosition

                        For i As Integer = 0 To ddlPositionIDOld.Items.Count - 1
                            If ddlPositionIDOld.Items(i).Selected Then
                                strRstName1 = ddlPositionIDOld.Items(i).Text.Trim.Split("-")(1).ToString
                            Else
                                If strRstName2 <> "" Then strRstName2 += ","
                                strRstName2 += ddlPositionIDOld.Items(i).Text.Trim.Split("-")(1).ToString
                            End If
                        Next
                        If strRstName2 = "" Then
                            lblSelectPositionNameOld.Text = strRstName1
                        Else
                            lblSelectPositionNameOld.Text = strRstName1 + "," + strRstName2
                        End If
                    Else
                        ddlPositionIDOld.Items.Clear()
                        ddlPositionIDOld.Items.Insert(0, New ListItem("---請選擇---", ""))

                        lblSelectPositionOld.Text = ""
                        lblSelectPositionNameOld.Text = ""
                        hidPositionIDOld.Value = ""
                    End If

                Case "ucPositionID"
                    lblSelectPosition.Text = aryData(1)
                    If lblSelectPosition.Text <> "''" Then  '非必填時，回傳空值
                        '載入 職位 下拉式選單
                        strSql = " and PositionID in (" + lblSelectPosition.Text + ") and CompID = '" + ddlCompID.SelectedValue + "'"
                        Bsp.Utility.Position(ddlPositionID, "PositionID", , strSql)
                        '第一筆為主要職位
                        Dim strDefaultValue() As String = lblSelectPosition.Text.Replace("'", "").Split(",")
                        Dim strPosition As String = ""
                        Bsp.Utility.SetSelectedIndex(ddlPositionID, strDefaultValue(0))
                        For intLoop As Integer = 0 To strDefaultValue.GetUpperBound(0)
                            If intLoop = 0 Then
                                strPosition = "1|" + strDefaultValue(intLoop)
                            Else
                                strPosition = strPosition + "|0|" + strDefaultValue(intLoop)
                            End If
                        Next
                        hidPositionID.Value = strPosition

                        For i As Integer = 0 To ddlPositionID.Items.Count - 1
                            If ddlPositionID.Items(i).Selected Then
                                strRstName1 = ddlPositionID.Items(i).Text.Trim.Split("-")(1).ToString
                            Else
                                If strRstName2 <> "" Then strRstName2 += ","
                                strRstName2 += ddlPositionID.Items(i).Text.Trim.Split("-")(1).ToString
                            End If
                        Next
                        If strRstName2 = "" Then
                            lblSelectPositionName.Text = strRstName1
                        Else
                            lblSelectPositionName.Text = strRstName1 + "," + strRstName2
                        End If
                    Else
                        ddlPositionID.Items.Clear()
                        ddlPositionID.Items.Insert(0, New ListItem("---請選擇---", ""))

                        lblSelectPosition.Text = ""
                        lblSelectPositionName.Text = ""
                        hidPositionID.Value = ""
                    End If

                Case "ucSelectWorkTypeOld"
                    lblSelectWorkTypeOld.Text = aryData(1)

                    If lblSelectWorkTypeOld.Text <> "''" Then  '非必填時，回傳空值
                        '載入 工作性質 下拉式選單
                        strSql = " and WorkTypeID in (" + lblSelectWorkTypeOld.Text + ") and CompID = '" + ddlCompIDOld.SelectedValue + "'"
                        Bsp.Utility.WorkType(ddlWorkTypeIDOld, "WorkTypeID", , strSql)

                        '第一筆為主要工作性質
                        Dim strDefaultValue() As String = lblSelectWorkTypeOld.Text.Replace("'", "").Split(",")
                        Dim strWorkType As String = ""
                        Bsp.Utility.SetSelectedIndex(ddlWorkTypeIDOld, strDefaultValue(0))
                        For intLoop As Integer = 0 To strDefaultValue.GetUpperBound(0)
                            If intLoop = 0 Then
                                strWorkType = "1|" + strDefaultValue(intLoop)
                            Else
                                strWorkType = strWorkType + "|0|" + strDefaultValue(intLoop)
                            End If
                        Next
                        hidWorkTypeIDOld.Value = strWorkType

                        For i As Integer = 0 To ddlWorkTypeIDOld.Items.Count - 1
                            If ddlWorkTypeIDOld.Items(i).Selected Then
                                strRstName1 = ddlWorkTypeIDOld.Items(i).Text.Trim.Split("-")(1).ToString
                            Else
                                If strRstName2 <> "" Then strRstName2 += ","
                                strRstName2 += ddlWorkTypeIDOld.Items(i).Text.Trim.Split("-")(1).ToString
                            End If
                        Next
                        If strRstName2 = "" Then
                            lblSelectWorkTypeNameOld.Text = strRstName1
                        Else
                            lblSelectWorkTypeNameOld.Text = strRstName1 + "," + strRstName2
                        End If
                    Else
                        ddlWorkTypeIDOld.Items.Clear()
                        ddlWorkTypeIDOld.Items.Insert(0, New ListItem("---請選擇---", ""))

                        lblSelectWorkTypeOld.Text = ""
                        lblSelectWorkTypeNameOld.Text = ""
                        hidWorkTypeIDOld.Value = ""
                    End If

                Case "ucSelectWorkType"
                    lblSelectWorkType.Text = aryData(1)

                    If lblSelectWorkType.Text <> "''" Then  '非必填時，回傳空值
                        '載入 工作性質 下拉式選單
                        strSql = " and WorkTypeID in (" + lblSelectWorkType.Text + ") and CompID = '" + ddlCompID.SelectedValue + "'"
                        Bsp.Utility.WorkType(ddlWorkTypeID, "WorkTypeID", , strSql)

                        '第一筆為主要工作性質
                        Dim strDefaultValue() As String = lblSelectWorkType.Text.Replace("'", "").Split(",")
                        Dim strWorkType As String = ""
                        Bsp.Utility.SetSelectedIndex(ddlWorkTypeID, strDefaultValue(0))
                        For intLoop As Integer = 0 To strDefaultValue.GetUpperBound(0)
                            If intLoop = 0 Then
                                strWorkType = "1|" + strDefaultValue(intLoop)
                            Else
                                strWorkType = strWorkType + "|0|" + strDefaultValue(intLoop)
                            End If
                        Next
                        hidWorkTypeID.Value = strWorkType

                        For i As Integer = 0 To ddlWorkTypeID.Items.Count - 1
                            If ddlWorkTypeID.Items(i).Selected Then
                                strRstName1 = ddlWorkTypeID.Items(i).Text.Trim.Split("-")(1).ToString
                            Else
                                If strRstName2 <> "" Then strRstName2 += ","
                                strRstName2 += ddlWorkTypeID.Items(i).Text.Trim.Split("-")(1).ToString
                            End If
                        Next
                        If strRstName2 = "" Then
                            lblSelectWorkTypeName.Text = strRstName1
                        Else
                            lblSelectWorkTypeName.Text = strRstName1 + "," + strRstName2
                        End If
                    Else
                        ddlWorkTypeID.Items.Clear()
                        ddlWorkTypeID.Items.Insert(0, New ListItem("---請選擇---", ""))

                        lblSelectWorkType.Text = ""
                        lblSelectWorkTypeName.Text = ""
                        hidWorkTypeID.Value = ""
                    End If

            End Select
        End If
    End Sub

    '異動原因變動時改變 異動後的任職狀況
    Protected Sub ddlReason_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlReason.SelectedIndexChanged
        Dim objST As New ST1()
        Dim ReasonTable As DataTable
        ReasonTable = objST.selectWorkStatus(ddlReason.SelectedValue)
        If ReasonTable.Rows().Count > 0 Then
            hidWorkStatus.Value = ReasonTable.Rows(0).Item(0)
            hidWorkStatusName.Value = ReasonTable.Rows(0).Item(1)
            txtWorkStatus.Text = ReasonTable.Rows(0).Item(2)
        Else
            hidWorkStatus.Value = ""
            hidWorkStatusName.Value = ""
            txtWorkStatus.Text = ""
        End If
    End Sub

    '生效日期查詢異動前、後資料
    Protected Sub btnEmployeeLog_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEmployeeLog.Click
        Dim objST As New ST1()
        Dim EmployeeLogOldTable As DataTable
        Dim EmployeeLogNewTable As DataTable

        ddlPositionIDOld.Items.Clear()
        lblSelectPositionOld.Text = ""
        lblSelectPositionNameOld.Text = ""

        ddlWorkTypeIDOld.Items.Clear()
        lblSelectWorkTypeOld.Text = ""
        lblSelectWorkTypeNameOld.Text = ""

        ddlPositionID.Items.Clear()
        lblSelectPosition.Text = ""
        lblSelectPositionName.Text = ""

        ddlWorkTypeID.Items.Clear()
        lblSelectWorkType.Text = ""
        lblSelectWorkTypeName.Text = ""

        EmployeeLogOldTable = objST.QueryEmployeeLogOld(ViewState.Item("IDNo"), ucValidDate.DateText)
        If EmployeeLogOldTable.Rows().Count <> 0 Then
            '(前)部門名稱
            ddlCompIDOld.SelectedValue = EmployeeLogOldTable.Rows(0).Item(0).ToString()

            '(前)職位
            'ddlPositionIDOld.Items.Insert(0, New ListItem(EmployeeLogOldTable.Rows(0).Item(3).ToString(), EmployeeLogOldTable.Rows(0).Item(1).ToString()))
            'ddlPositionIDOld.SelectedValue = EmployeeLogOldTable.Rows(0).Item(1).ToString()
            'If lblSelectPositionOld.Text = "" Then
            '    lblSelectPositionOld.Text = "'" + EmployeeLogOldTable.Rows(0).Item(1).ToString() + "'"
            'Else
            '    lblSelectPositionOld.Text += ",'" + EmployeeLogOldTable.Rows(0).Item(1).ToString() + "'"
            'End If
            'hidPositionIDOld.Value = EmployeeLogOldTable.Rows(0).Item(1).ToString()
            'lblSelectPositionNameOld.Text = EmployeeLogOldTable.Rows(0).Item(12).ToString()
            If EmployeeLogOldTable.Rows(0).Item(1).ToString() <> "" Then
                Dim strPositionIDOld As String()
                Dim strPositionNameOld As String()
                strPositionIDOld = EmployeeLogOldTable.Rows(0).Item(1).ToString().Split("|")
                strPositionNameOld = EmployeeLogOldTable.Rows(0).Item(2).ToString().Split(",")

                For PositionIDOldCount As Integer = 0 To strPositionIDOld.Count / 2 - 1
                    ddlPositionIDOld.Items.Insert(PositionIDOldCount, New ListItem(strPositionIDOld(PositionIDOldCount * 2 + 1) + "-" + strPositionNameOld(PositionIDOldCount), strPositionIDOld(PositionIDOldCount * 2 + 1)))

                    If lblSelectPositionOld.Text = "" Then
                        lblSelectPositionOld.Text = "'" + strPositionIDOld(PositionIDOldCount * 2 + 1) + "'"
                    Else
                        lblSelectPositionOld.Text += ",'" + strPositionIDOld(PositionIDOldCount * 2 + 1) + "'"
                    End If
                Next

                ddlPositionIDOld.SelectedValue = strPositionIDOld(1)
                hidPositionIDOld.Value = EmployeeLogOldTable.Rows(0).Item(1).ToString()
                lblSelectPositionNameOld.Text = EmployeeLogOldTable.Rows(0).Item(2).ToString()
            Else
                lblSelectPositionOld.Text = ""
                hidPositionIDOld.Value = ""
                ddlPositionIDOld.Items.Insert(0, New ListItem("---請選擇---", ""))
            End If

            '(前)部門/科組課
            ucSelectHROrganOld.LoadData(EmployeeLogOldTable.Rows(0).Item(0).ToString(), "Y")
            ucSelectHROrganOld.setDeptID(EmployeeLogOldTable.Rows(0).Item(0).ToString(), EmployeeLogOldTable.Rows(0).Item(3).ToString(), "Y")
            ucSelectHROrganOld.setOrganID(EmployeeLogOldTable.Rows(0).Item(0).ToString(), EmployeeLogOldTable.Rows(0).Item(4).ToString(), "Y")

            '(前)工作性質
            'ddlWorkTypeIDOld.Items.Insert(0, New ListItem(EmployeeLogOldTable.Rows(0).Item(6).ToString(), EmployeeLogOldTable.Rows(0).Item(5).ToString()))
            'ddlWorkTypeIDOld.SelectedValue = EmployeeLogOldTable.Rows(0).Item(5).ToString()
            'hidWorkTypeIDOld.Value = EmployeeLogOldTable.Rows(0).Item(5).ToString()
            'If lblSelectWorkTypeOld.Text = "" Then
            '    lblSelectWorkTypeOld.Text = "'" + EmployeeLogOldTable.Rows(0).Item(5).ToString() + "'"
            'Else
            '    lblSelectWorkTypeOld.Text += ",'" + EmployeeLogOldTable.Rows(0).Item(5).ToString() + "'"
            'End If
            'lblSelectWorkTypeNameOld.Text = EmployeeLogOldTable.Rows(0).Item(12).ToString()
            If EmployeeLogOldTable.Rows(0).Item(6).ToString() <> "" Then
                Dim strWorkTypeIDOld As String()
                Dim strWorkTypeNameOld As String()
                strWorkTypeIDOld = EmployeeLogOldTable.Rows(0).Item(5).ToString().Split("|")
                strWorkTypeNameOld = EmployeeLogOldTable.Rows(0).Item(6).ToString().Split(",")

                For WorkTypeIDOldCount As Integer = 0 To strWorkTypeIDOld.Count / 2 - 1
                    ddlWorkTypeIDOld.Items.Insert(WorkTypeIDOldCount, New ListItem(strWorkTypeIDOld(WorkTypeIDOldCount * 2 + 1) + "-" + strWorkTypeNameOld(WorkTypeIDOldCount), strWorkTypeIDOld(WorkTypeIDOldCount * 2 + 1)))

                    If lblSelectWorkTypeOld.Text = "" Then
                        lblSelectWorkTypeOld.Text = "'" + strWorkTypeIDOld(WorkTypeIDOldCount * 2 + 1) + "'"
                    Else
                        lblSelectWorkTypeOld.Text += ",'" + strWorkTypeIDOld(WorkTypeIDOldCount * 2 + 1) + "'"
                    End If
                Next

                ddlWorkTypeIDOld.SelectedValue = strWorkTypeIDOld(1)
                hidWorkTypeIDOld.Value = EmployeeLogOldTable.Rows(0).Item(5).ToString()
                lblSelectWorkTypeNameOld.Text = EmployeeLogOldTable.Rows(0).Item(6).ToString()
            Else
                lblSelectWorkTypeOld.Text = ""
                hidWorkTypeIDOld.Value = ""
                ddlWorkTypeIDOld.Items.Insert(0, New ListItem("---請選擇---", ""))
            End If

            '(前)職等/職稱
            If EmployeeLogOldTable.Rows(0).Item(7).ToString() <> "" Then
                ucSelectRankAndTitleOld.setRankID(EmployeeLogOldTable.Rows(0).Item(0).ToString(), EmployeeLogOldTable.Rows(0).Item(7).ToString(), "U")
                ucSelectRankAndTitleOld.setTitleID(EmployeeLogOldTable.Rows(0).Item(0).ToString(), EmployeeLogOldTable.Rows(0).Item(7).ToString(), EmployeeLogOldTable.Rows(0).Item(8).ToString(), "U")
            Else
                ucSelectRankAndTitleOld.setRankID(EmployeeLogOldTable.Rows(0).Item(0).ToString(), EmployeeLogOldTable.Rows(0).Item(7).ToString(), "A")
                ucSelectRankAndTitleOld.setTitleID(EmployeeLogOldTable.Rows(0).Item(0).ToString(), EmployeeLogOldTable.Rows(0).Item(7).ToString(), EmployeeLogOldTable.Rows(0).Item(8).ToString(), "A")
            End If

            '(前)簽核最小單位
            ucSelectFlowOrganOld.LoadData(ucSelectHROrganOld.SelectedOrganID, "Y")
            ucSelectFlowOrganOld.setOrganID(EmployeeLogOldTable.Rows(0).Item(9).ToString(), "Y")

            '(前)任職狀況
            ddlWorkStatusOld.SelectedValue = EmployeeLogOldTable.Rows(0).Item(10).ToString()

            '2015/11/27 Add 生效日期
            hidBeforeDueDateOld.Value = EmployeeLogOldTable.Rows(0).Item(11).ToString()
        Else
            '(前)部門名稱
            ddlCompIDOld.SelectedValue = ""

            '(前)部門/科組課
            ucSelectHROrganOld.LoadData(ddlCompIDOld.SelectedValue, "Y")

            '(前)職位
            ddlPositionIDOld.Items.Insert(0, New ListItem("---請選擇---", ""))
            lblSelectPositionOld.Text = ""
            lblSelectPositionNameOld.Text = ""
            hidPositionIDOld.Value = ""

            '(前)工作性質
            ddlWorkTypeIDOld.Items.Insert(0, New ListItem("---請選擇---", ""))
            lblSelectWorkTypeOld.Text = ""
            lblSelectWorkTypeNameOld.Text = ""
            hidWorkTypeIDOld.Value = ""

            '(前)職等/職稱
            ucSelectRankAndTitleOld.LoadData(ddlCompIDOld.SelectedValue, "A")

            '(前)簽核最小單位
            ucSelectFlowOrganOld.LoadData(ucSelectHROrganOld.SelectedOrganID, "Y")

            '2015/11/27 Add 生效日期
            hidBeforeDueDateOld.Value = ""
        End If

        EmployeeLogNewTable = objST.QueryEmployeeLogNew(ViewState.Item("IDNo"), ucValidDate.DateText)
        If EmployeeLogNewTable.Rows().Count <> 0 Then
            '(後)部門名稱
            ddlCompID.SelectedValue = EmployeeLogNewTable.Rows(0).Item(0).ToString()

            '(後)職位
            'ddlPositionID.Items.Insert(0, New ListItem(EmployeeLogNewTable.Rows(0).Item(2).ToString(), EmployeeLogNewTable.Rows(0).Item(1).ToString()))
            'ddlPositionID.SelectedValue = EmployeeLogNewTable.Rows(0).Item(1).ToString()
            'If lblSelectPosition.Text = "" Then
            '    lblSelectPosition.Text = "'" + EmployeeLogNewTable.Rows(0).Item(1).ToString() + "'"
            'Else
            '    lblSelectPosition.Text += ",'" + EmployeeLogNewTable.Rows(0).Item(1).ToString() + "'"
            'End If
            'hidPositionID.Value = EmployeeLogNewTable.Rows(0).Item(1).ToString()
            'lblSelectPositionName.Text = EmployeeLogNewTable.Rows(0).Item(13).ToString()
            If EmployeeLogNewTable.Rows(0).Item(1).ToString() <> "" Then
                Dim strPositionID As String()
                Dim strPositionName As String()
                strPositionID = EmployeeLogNewTable.Rows(0).Item(1).ToString().Split("|")
                strPositionName = EmployeeLogNewTable.Rows(0).Item(2).ToString().Split(",")

                For PositionIDCount As Integer = 0 To strPositionID.Count / 2 - 1
                    ddlPositionID.Items.Insert(PositionIDCount, New ListItem(strPositionID(PositionIDCount * 2 + 1) + "-" + strPositionName(PositionIDCount), strPositionID(PositionIDCount * 2 + 1)))

                    If lblSelectPosition.Text = "" Then
                        lblSelectPosition.Text = "'" + strPositionID(PositionIDCount * 2 + 1) + "'"
                    Else
                        lblSelectPosition.Text += ",'" + strPositionID(PositionIDCount * 2 + 1) + "'"
                    End If
                Next

                ddlPositionID.SelectedValue = strPositionID(1)
                hidPositionID.Value = EmployeeLogNewTable.Rows(0).Item(1).ToString()
                lblSelectPositionName.Text = EmployeeLogNewTable.Rows(0).Item(2).ToString()
            Else
                lblSelectPosition.Text = ""
                hidPositionID.Value = ""
                ddlPositionID.Items.Insert(0, New ListItem("---請選擇---", ""))
            End If

            '(後)部門/科組課
            ucSelectHROrgan.LoadData(EmployeeLogNewTable.Rows(0).Item(0).ToString(), "Y")
            ucSelectHROrgan.setDeptID(EmployeeLogNewTable.Rows(0).Item(0).ToString(), EmployeeLogNewTable.Rows(0).Item(3).ToString(), "Y")
            ucSelectHROrgan.setOrganID(EmployeeLogNewTable.Rows(0).Item(0).ToString(), EmployeeLogNewTable.Rows(0).Item(4).ToString(), "Y")

            '(後)工作性質
            'ddlWorkTypeID.Items.Insert(0, New ListItem(EmployeeLogNewTable.Rows(0).Item(6).ToString(), EmployeeLogNewTable.Rows(0).Item(5).ToString()))
            'ddlWorkTypeID.SelectedValue = EmployeeLogNewTable.Rows(0).Item(5).ToString()
            'hidWorkTypeID.Value = EmployeeLogNewTable.Rows(0).Item(5).ToString()
            'If lblSelectWorkType.Text = "" Then
            '    lblSelectWorkType.Text = "'" + EmployeeLogNewTable.Rows(0).Item(5).ToString() + "'"
            'Else
            '    lblSelectWorkType.Text += ",'" + EmployeeLogNewTable.Rows(0).Item(5).ToString() + "'"
            'End If
            'lblSelectWorkTypeName.Text = EmployeeLogNewTable.Rows(0).Item(13).ToString()
            If EmployeeLogNewTable.Rows(0).Item(6).ToString() <> "" Then
                Dim strWorkTypeID As String()
                Dim strWorkTypeName As String()
                strWorkTypeID = EmployeeLogNewTable.Rows(0).Item(5).ToString().Split("|")
                strWorkTypeName = EmployeeLogNewTable.Rows(0).Item(6).ToString().Split(",")

                For WorkTypeIDCount As Integer = 0 To strWorkTypeID.Count / 2 - 1
                    ddlWorkTypeID.Items.Insert(WorkTypeIDCount, New ListItem(strWorkTypeID(WorkTypeIDCount * 2 + 1) + "-" + strWorkTypeName(WorkTypeIDCount), strWorkTypeID(WorkTypeIDCount * 2 + 1)))

                    If lblSelectWorkType.Text = "" Then
                        lblSelectWorkType.Text = "'" + strWorkTypeID(WorkTypeIDCount * 2 + 1) + "'"
                    Else
                        lblSelectWorkType.Text += ",'" + strWorkTypeID(WorkTypeIDCount * 2 + 1) + "'"
                    End If
                Next

                ddlWorkTypeID.SelectedValue = strWorkTypeID(1)
                hidWorkTypeID.Value = EmployeeLogNewTable.Rows(0).Item(5).ToString()
                lblSelectWorkTypeName.Text = EmployeeLogNewTable.Rows(0).Item(6).ToString()
            Else
                lblSelectWorkType.Text = ""
                hidWorkTypeID.Value = ""
                ddlWorkTypeID.Items.Insert(0, New ListItem("---請選擇---", ""))
            End If

            '(後)職等/職稱
            If EmployeeLogNewTable.Rows(0).Item(7).ToString() <> "" Then
                ucSelectRankAndTitle.setRankID(EmployeeLogNewTable.Rows(0).Item(0).ToString(), EmployeeLogNewTable.Rows(0).Item(7).ToString(), "U")
                ucSelectRankAndTitle.setTitleID(EmployeeLogNewTable.Rows(0).Item(0).ToString(), EmployeeLogNewTable.Rows(0).Item(7).ToString(), EmployeeLogNewTable.Rows(0).Item(8).ToString(), "U")
            Else
                ucSelectRankAndTitle.setRankID(EmployeeLogNewTable.Rows(0).Item(0).ToString(), EmployeeLogNewTable.Rows(0).Item(7).ToString(), "A")
                ucSelectRankAndTitle.setTitleID(EmployeeLogNewTable.Rows(0).Item(0).ToString(), EmployeeLogNewTable.Rows(0).Item(7).ToString(), EmployeeLogNewTable.Rows(0).Item(8).ToString(), "A")
            End If

            '(後)簽核最小單位
            ucSelectFlowOrgan.LoadData(ucSelectHROrgan.SelectedOrganID, "Y")
            ucSelectFlowOrgan.setOrganID(EmployeeLogNewTable.Rows(0).Item(9).ToString(), "Y")
        Else
            '(後)部門名稱
            ddlCompID.SelectedValue = ""

            '(後)部門/科組課
            ucSelectHROrgan.LoadData(ddlCompID.SelectedValue, "Y")

            '(後)職位
            ddlPositionID.Items.Insert(0, New ListItem("---請選擇---", ""))
            lblSelectPosition.Text = ""
            lblSelectPositionName.Text = ""
            hidPositionID.Value = ""

            '(後)工作性質
            ddlWorkTypeID.Items.Insert(0, New ListItem("---請選擇---", ""))
            lblSelectWorkType.Text = ""
            lblSelectWorkTypeName.Text = ""
            hidWorkTypeID.Value = ""

            '(後)職等/職稱
            ucSelectRankAndTitle.LoadData(ddlCompID.SelectedValue, "A")

            '(後)簽核最小單位
            ucSelectFlowOrgan.LoadData(ucSelectHROrgan.SelectedOrganID, "Y")
        End If

    End Sub

    '異動前資料-公司
    Protected Sub ddlCompIDOld_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlCompIDOld.SelectedIndexChanged
        ucSelectHROrganOld.LoadData(ddlCompIDOld.SelectedValue, "Y")
        ucSelectRankAndTitleOld.LoadData(ddlCompIDOld.SelectedValue, "A")
        ucSelectHROrganOld_ucSelectOrganIDSelectedIndexChangedHandler_SelectChange(Nothing, Nothing)
    End Sub

    '異動後資料-公司
    Protected Sub ddlCompID_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlCompID.SelectedIndexChanged
        ucSelectHROrgan.LoadData(ddlCompID.SelectedValue, "Y")
        ucSelectRankAndTitle.LoadData(ddlCompID.SelectedValue, "A")
        ucSelectHROrgan_ucSelectOrganIDSelectedIndexChangedHandler_SelectChange(Nothing, Nothing)
    End Sub

    '異動前資料-職位button
    Protected Sub ucPositionIDOld_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ucPositionIDOld.Load
        '載入按鈕-職位選單畫面
        ucPositionIDOld.QueryCompID = ddlCompIDOld.SelectedValue
        ucPositionIDOld.QueryEmpID = ""
        ucPositionIDOld.DefaultPosition = lblSelectPositionOld.Text
        ucPositionIDOld.QueryOrganID = ucSelectHROrganOld.SelectedOrganID
        ucPositionIDOld.Fields = New FieldState() { _
            New FieldState("PositionID", "職位代碼", True, True), _
            New FieldState("Remark", "職位名稱", True, True)}
    End Sub

    '異動後資料-職位button
    Protected Sub ucPositionID_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ucPositionID.Load
        '載入按鈕-職位選單畫面
        ucPositionID.QueryCompID = ddlCompID.SelectedValue
        ucPositionID.QueryEmpID = ""
        ucPositionID.DefaultPosition = lblSelectPosition.Text
        ucPositionID.QueryOrganID = ucSelectHROrgan.SelectedOrganID
        ucPositionID.Fields = New FieldState() { _
            New FieldState("PositionID", "職位代碼", True, True), _
            New FieldState("Remark", "職位名稱", True, True)}
    End Sub

    '異動前資料-職位下拉選單:將選擇那筆 改為 第一筆為主要職位
    Protected Sub ddlPositionIDOld_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlPositionIDOld.SelectedIndexChanged
        Dim strRst1 As String = ""
        Dim strRst2 As String = ""
        Dim strMainPosition As String = ""
        Dim strPosition As String = ""
        Dim strRstName1 As String = ""
        Dim strRstName2 As String = ""
        For i As Integer = 0 To ddlPositionIDOld.Items.Count - 1

            If ddlPositionIDOld.Items(i).Selected Then
                strRst1 = "'" + ddlPositionIDOld.Items(i).Value + "'"
                strMainPosition = "1|" + ddlPositionIDOld.Items(i).Value
                strRstName1 = ddlPositionIDOld.Items(i).Text.Trim.Split("-")(1).ToString
            Else
                If strRst2 <> "" Then strRst2 += ","
                strRst2 += "'" + ddlPositionIDOld.Items(i).Value + "'"
                If strPosition <> "" Then strPosition += "|"
                strPosition += "0|" + ddlPositionIDOld.Items(i).Value
                If strRstName2 <> "" Then strRstName2 += ","
                strRstName2 += ddlPositionIDOld.Items(i).Text.Trim.Split("-")(1).ToString
            End If
        Next
        If strRst2 = "" Then
            lblSelectPositionOld.Text = strRst1
            hidPositionIDOld.Value = strMainPosition
            lblSelectPositionNameOld.Text = strRstName1
        Else
            lblSelectPositionOld.Text = strRst1 + "," + strRst2
            hidPositionIDOld.Value = strMainPosition + "|" + strPosition
            lblSelectPositionNameOld.Text = strRstName1 + "," + strRstName2
        End If
    End Sub

    '異動後資料-職位下拉選單:將選擇那筆 改為 第一筆為主要職位
    Protected Sub ddlPositionID_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlPositionID.SelectedIndexChanged
        Dim strRst1 As String = ""
        Dim strRst2 As String = ""
        Dim strMainPosition As String = ""
        Dim strPosition As String = ""
        Dim strRstName1 As String = ""
        Dim strRstName2 As String = ""
        For i As Integer = 0 To ddlPositionID.Items.Count - 1

            If ddlPositionID.Items(i).Selected Then
                strRst1 = "'" + ddlPositionID.Items(i).Value + "'"
                strMainPosition = "1|" + ddlPositionID.Items(i).Value
                strRstName1 = ddlPositionID.Items(i).Text.Trim.Split("-")(1).ToString
            Else
                If strRst2 <> "" Then strRst2 += ","
                strRst2 += "'" + ddlPositionID.Items(i).Value + "'"
                If strPosition <> "" Then strPosition += "|"
                strPosition += "0|" + ddlPositionID.Items(i).Value
                If strRstName2 <> "" Then strRstName2 += ","
                strRstName2 += ddlPositionID.Items(i).Text.Trim.Split("-")(1).ToString
            End If
        Next
        If strRst2 = "" Then
            lblSelectPosition.Text = strRst1
            hidPositionID.Value = strMainPosition
            lblSelectPositionName.Text = strRstName1
        Else
            lblSelectPosition.Text = strRst1 + "," + strRst2
            hidPositionID.Value = strMainPosition + "|" + strPosition
            lblSelectPositionName.Text = strRstName1 + "," + strRstName2
        End If
    End Sub

    '異動前資料-工作性質button
    Protected Sub ucSelectWorkTypeOld_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ucSelectWorkTypeOld.Load
        '載入按鈕-工作性質選單畫面
        ucSelectWorkTypeOld.QueryCompID = ddlCompIDOld.SelectedValue
        ucSelectWorkTypeOld.QueryEmpID = ""
        ucSelectWorkTypeOld.DefaultWorkType = lblSelectWorkTypeOld.Text
        ucSelectWorkTypeOld.QueryOrganID = ucSelectHROrganOld.SelectedDeptID
        ucSelectWorkTypeOld.Fields = New FieldState() { _
            New FieldState("WorkTypeID", "工作性質代碼", True, True), _
            New FieldState("Remark", "工作性質名稱", True, True)}
    End Sub

    '異動後資料-工作性質button
    Protected Sub ucSelectWorkType_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ucSelectWorkType.Load
        '載入按鈕-工作性質選單畫面
        ucSelectWorkType.QueryCompID = ddlCompID.SelectedValue
        ucSelectWorkType.QueryEmpID = ""
        ucSelectWorkType.DefaultWorkType = lblSelectWorkType.Text
        ucSelectWorkType.QueryOrganID = ucSelectHROrgan.SelectedDeptID
        ucSelectWorkType.Fields = New FieldState() { _
            New FieldState("WorkTypeID", "工作性質代碼", True, True), _
            New FieldState("Remark", "工作性質名稱", True, True)}
    End Sub

    '異動前資料-工作性質選單:將選擇那筆 改為 第一筆為主要工作性質
    Protected Sub ddlWorkTypeOld_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlWorkTypeIDOld.SelectedIndexChanged
        Dim strRst1 As String = ""
        Dim strRst2 As String = ""
        Dim strMainWorkType As String = ""
        Dim strWorkType As String = ""
        Dim strRstName1 As String = ""
        Dim strRstName2 As String = ""
        For i As Integer = 0 To ddlWorkTypeIDOld.Items.Count - 1

            If ddlWorkTypeIDOld.Items(i).Selected Then
                strRst1 = "'" + ddlWorkTypeIDOld.Items(i).Value + "'"
                strMainWorkType = "1|" + ddlWorkTypeIDOld.Items(i).Value
                strRstName1 = ddlWorkTypeIDOld.Items(i).Text.Trim.Split("-")(1).ToString
            Else
                If strRst2 <> "" Then strRst2 += ","
                strRst2 += "'" + ddlWorkTypeIDOld.Items(i).Value + "'"
                If strWorkType <> "" Then strWorkType += "|"
                strWorkType += "0|" + ddlWorkTypeIDOld.Items(i).Value
                If strRstName2 <> "" Then strRstName2 += ","
                strRstName2 += ddlWorkTypeIDOld.Items(i).Text.Trim.Split("-")(1).ToString
            End If
        Next
        If strRst2 = "" Then
            lblSelectWorkTypeOld.Text = strRst1
            hidWorkTypeIDOld.Value = strMainWorkType
            lblSelectWorkTypeNameOld.Text = strRstName1
        Else
            lblSelectWorkTypeOld.Text = strRst1 + "," + strRst2
            hidWorkTypeIDOld.Value = strMainWorkType + "|" + strWorkType
            lblSelectWorkTypeNameOld.Text = strRstName1 + "," + strRstName2
        End If
    End Sub

    '異動後資料-工作性質選單:將選擇那筆 改為 第一筆為主要工作性質
    Protected Sub ddlWorkType_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlWorkTypeID.SelectedIndexChanged
        Dim strRst1 As String = ""
        Dim strRst2 As String = ""
        Dim strMainWorkType As String = ""
        Dim strWorkType As String = ""
        Dim strRstName1 As String = ""
        Dim strRstName2 As String = ""
        For i As Integer = 0 To ddlWorkTypeID.Items.Count - 1

            If ddlWorkTypeID.Items(i).Selected Then
                strRst1 = "'" + ddlWorkTypeID.Items(i).Value + "'"
                strMainWorkType = "1|" + ddlWorkTypeID.Items(i).Value
                strRstName1 = ddlWorkTypeID.Items(i).Text.Trim.Split("-")(1).ToString
            Else
                If strRst2 <> "" Then strRst2 += ","
                strRst2 += "'" + ddlWorkTypeID.Items(i).Value + "'"
                If strWorkType <> "" Then strWorkType += "|"
                strWorkType += "0|" + ddlWorkTypeID.Items(i).Value
                If strRstName2 <> "" Then strRstName2 += ","
                strRstName2 += ddlWorkTypeID.Items(i).Text.Trim.Split("-")(1).ToString
            End If
        Next
        If strRst2 = "" Then
            lblSelectWorkType.Text = strRst1
            hidWorkTypeID.Value = strMainWorkType
            lblSelectWorkTypeName.Text = strRstName1
        Else
            lblSelectWorkType.Text = strRst1 + "," + strRst2
            hidWorkTypeID.Value = strMainWorkType + "|" + strWorkType
            lblSelectWorkTypeName.Text = strRstName1 + "," + strRstName2
        End If
    End Sub

    '異動前部門change
    Protected Sub ucSelectHROrganOld_ucSelectDeptIDSelectedIndexChangedHandler_SelectChange(ByVal sender As Object, ByVal e As System.EventArgs) Handles ucSelectHROrganOld.ucSelectDeptIDSelectedIndexChangedHandler_SelectChange
        lblSelectWorkTypeOld.Text = ""
        hidWorkTypeIDOld.Value = ""
        lblSelectWorkTypeNameOld.Text = ""
        ddlWorkTypeIDOld.Items.Clear()
        ddlWorkTypeIDOld.Items.Insert(0, New ListItem("---請選擇---", ""))
    End Sub

    '異動前科組課change
    Protected Sub ucSelectHROrganOld_ucSelectOrganIDSelectedIndexChangedHandler_SelectChange(ByVal sender As Object, ByVal e As System.EventArgs) Handles ucSelectHROrganOld.ucSelectOrganIDSelectedIndexChangedHandler_SelectChange
        '最小簽核單位
        ucSelectFlowOrganOld.LoadData(ucSelectHROrganOld.SelectedOrganID, "Y")
        ucSelectFlowOrganOld.SetDefaultOrgan()

        lblSelectPositionOld.Text = ""
        hidPositionIDOld.Value = ""
        lblSelectPositionNameOld.Text = ""
        ddlPositionIDOld.Items.Clear()
        ddlPositionIDOld.Items.Insert(0, New ListItem("---請選擇---", ""))
    End Sub

    '異動後部門change
    Protected Sub ucSelectHROrgan_ucSelectDeptIDSelectedIndexChangedHandler_SelectChange(ByVal sender As Object, ByVal e As System.EventArgs) Handles ucSelectHROrgan.ucSelectDeptIDSelectedIndexChangedHandler_SelectChange
        lblSelectWorkType.Text = ""
        hidWorkTypeID.Value = ""
        lblSelectWorkTypeName.Text = ""
        ddlWorkTypeID.Items.Clear()
        ddlWorkTypeID.Items.Insert(0, New ListItem("---請選擇---", ""))
    End Sub

    '異動後科組課change
    Protected Sub ucSelectHROrgan_ucSelectOrganIDSelectedIndexChangedHandler_SelectChange(ByVal sender As Object, ByVal e As System.EventArgs) Handles ucSelectHROrgan.ucSelectOrganIDSelectedIndexChangedHandler_SelectChange
        '最小簽核單位
        ucSelectFlowOrgan.LoadData(ucSelectHROrgan.SelectedOrganID, "Y")
        ucSelectFlowOrgan.SetDefaultOrgan()

        lblSelectPosition.Text = ""
        hidPositionID.Value = ""
        lblSelectPositionName.Text = ""
        ddlPositionID.Items.Clear()
        ddlPositionID.Items.Insert(0, New ListItem("---請選擇---", ""))
    End Sub

    Private Sub Release(ByVal LogFunction As String)
        ucRelease.ShowCompRole = "True"
        ucRelease.FunID = "ST1800"
        ucRelease.LogFunction = LogFunction
        ucRelease.OpenSelect()
    End Sub
End Class
