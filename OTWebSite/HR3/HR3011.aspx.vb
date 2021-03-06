'****************************************************
'功能說明：員工待異動紀錄(EmployeeLog)維護-新增
'建立人員：Weicheng
'建立日期：2014/08/18
'****************************************************
Imports System.Data

Partial Class HR_HR3011
    Inherits PageBase


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then
            '公司
            Bsp.Utility.FillHRCompany(ddlCompIDOld)
            '任職狀況
            HR.FillWorkStatus(ddlWorkStatusOld)
            HR.FillWorkStatus(ddlWorkStatusNew)
            ViewState.Item("WorkTypeIDOld") = ""
            ViewState.Item("PositionIDOld") = ""
            ViewState.Item("WorkTypeIDNew") = ""
            ViewState.Item("PositionIDNew") = ""

            '主管任用方式
            ddlBossTypeNewData.Items.Insert(0, New ListItem("---請選擇---", ""))
            ddlBossTypeNewData.Items.Insert(1, New ListItem("主要", "1"))
            ddlBossTypeNewData.Items.Insert(2, New ListItem("兼任", "2"))
        End If
    End Sub
    Protected Overrides Sub BaseOnPageTransfer(ByVal ti As TransferInfo)
        If Not IsPostBack() Then

            Dim objHR As New HR
            Dim objHR3000 As New HR3000
            Dim ht As Hashtable = Bsp.Utility.getHashTableFromParam(ti.Args)

            'CompID,EmpID,Wait_ValidDate,Wait_Seq,IDNo,ModifyDate,Reason

            If ht.ContainsKey("SelectCompID") Then
                ViewState.Item("CompID") = ht("SelectCompID").ToString()
                hidCompID.Value = ViewState.Item("CompID")
            End If
            If ht.ContainsKey("SelectEmpID") Then
                ViewState.Item("EmpID") = ht("SelectEmpID").ToString()
                hidEmpID.Value = ViewState.Item("EmpID")
            End If
            If ht.ContainsKey("SelectName") Then
                ViewState.Item("Name") = ht("SelectName").ToString()
            End If
            If ht.ContainsKey("SelectValidDate") Then
                ViewState.Item("Wait_ValidDate") = ht("SelectValidDate").ToString()
                hidValidDate.Value = ViewState.Item("Wait_ValidDate")
            End If
            If ht.ContainsKey("SelectSeq") Then
                ViewState.Item("Wait_Seq") = ht("SelectSeq").ToString()
                hidSeq.Value = ViewState.Item("Wait_Seq")
            End If
            If ht.ContainsKey("IDNo") Then
                ViewState.Item("IDNo") = ht("IDNo").ToString()
                hidIDNo.Value = ViewState.Item("IDNo")
            End If
            If ht.ContainsKey("ModifyDate") Then
                ViewState.Item("ModifyDate") = ht("ModifyDate").ToString()
                hidModifyDate.Value = ViewState.Item("ModifyDate")
            End If
            If ht.ContainsKey("Reason") Then
                ViewState.Item("Reason") = ht("Reason").ToString()
                hidReason.Value = ViewState.Item("Reason")
            End If
            If ht.ContainsKey("Detail") Then
                ViewState.Item("Detail") = ht("Detail").ToString()
            End If

            lblHeadMsg.Text = "公司名稱：" + objHR.GetHRCompName(ViewState.Item("CompID")).Rows(0).Item("CompName").ToString + "   員工編號：" + ViewState.Item("EmpID") + "-" + ViewState.Item("Name") + "   企業團生效日期：" + ViewState.Item("Wait_ValidDate") + "   異動原因：" + objHR.GetReasonName(ViewState.Item("Reason")).Rows(0).Item("ReasonName").ToString

            '部門、科組課
            ucSelectHROrganOld.LoadData(ViewState.Item("NewCompID"))
            ddlFlowOrganIDOld.Items.Insert(0, New ListItem("---請選擇---", ""))
            ucSelectHROrganNew.LoadData(ViewState.Item("NewCompID"))
            ddlFlowOrganIDNew.Items.Insert(0, New ListItem("---請選擇---", ""))

            '職等Old        
            Bsp.Utility.Rank(ddlRankIDOld, ddlCompIDOld.SelectedItem.Value)
            ddlRankIDOld.Items.Insert(0, New ListItem("---請選擇---", ""))
            '職稱Old
            ddlTitleIDOld.Items.Insert(0, "---請先選擇職等---")
            '職等New
            Bsp.Utility.Rank(ddlRankIDNew, hidCompID.Value)
            ddlRankIDNew.Items.Insert(0, New ListItem("---請選擇---", ""))
            '職稱New
            ddlTitleIDNew.Items.Insert(0, "---請先選擇職等---")

            '工作性質
            ddlWorkTypeOld.Items.Insert(0, New ListItem("---請選擇---", ""))
            ddlWorkTypeNew.Items.Insert(0, New ListItem("---請選擇---", ""))

            '職位
            ddlPositionOld.Items.Insert(0, New ListItem("---請選擇---", ""))
            ddlPositionNew.Items.Insert(0, New ListItem("---請選擇---", ""))


            subGetData(ViewState.Item("CompID"), ViewState.Item("EmpID"), ViewState.Item("Wait_ValidDate"), ViewState.Item("Wait_Seq"), ViewState.Item("IDNo"), ViewState.Item("ModifyDate"), ViewState.Item("Reason"))
        End If
    End Sub
    Private Sub subGetData(ByVal CompID As String, ByVal EmpID As String, ByVal ValidDate As String, ByVal Seq As String, ByVal IDNo As String, ByVal ModifyDate As String, ByVal Reason As String)
        Dim strWorkTypeID As String = ""
        Dim strPositionID As String = ""
        Dim objSC As New SC

        'EmployeeLog
        Dim bsEmployeeLog As New beEmployeeLog.Service()
        Dim beEmployeeLog As New beEmployeeLog.Row()

        beEmployeeLog.IDNo.Value = IDNo
        beEmployeeLog.ModifyDate.Value = ModifyDate
        beEmployeeLog.Reason.Value = Reason

        Try
            Using dt As DataTable = bsEmployeeLog.QueryByKey(beEmployeeLog).Tables(0)
                If dt.Rows.Count <= 0 Then Exit Sub
                beEmployeeLog = New beEmployeeLog.Row(dt.Rows(0))
                '異動前
                lblCompIDOldDate.Text = beEmployeeLog.CompIDOld.Value + " " + beEmployeeLog.CompanyOld.Value    '公司名稱
                lblGroupIDOldData.Text = beEmployeeLog.GroupIDOld.Value + " " + beEmployeeLog.GroupNameOld.Value    '事業群
                lblDeptIDOldData.Text = beEmployeeLog.DeptIDOld.Value + " " + beEmployeeLog.DeptNameOld.Value + "\" + beEmployeeLog.OrganIDOld.Value + " " + beEmployeeLog.OrganNameOld.Value   '部門\科組課
                lblFlowOrganIDOldData.Text = beEmployeeLog.OrganIDOld.Value + " " + beEmployeeLog.OrganNameOld.Value    '最小簽核單位
                lblRankIDOldData.Text = beEmployeeLog.RankIDOld.Value + "\" + beEmployeeLog.TitleIDOld.Value + " " + beEmployeeLog.TitleNameOld.Value   '職等\職稱
                '職位
                If beEmployeeLog.PositionIDOld.Value.Trim <> "" Then
                    Dim aryDataValue = Split(beEmployeeLog.PositionIDOld.Value.Trim, "|")
                    Dim aryData = Split(beEmployeeLog.PositionOld.Value.Trim, ",")
                    Dim intCount As Integer
                    strPositionID = ""
                    intCount = 0
                    For Each strKey As String In aryData
                        If strPositionID = "" Then
                            strPositionID = aryDataValue(intCount + 1) + " " + strKey
                        Else
                            strPositionID = strPositionID + "," + aryDataValue(intCount + 1) + " " + strKey
                        End If
                        intCount = intCount + 2
                        If intCount > aryData.GetUpperBound(0) Then
                            Exit For
                        End If
                    Next
                End If
                lblPositionIDOldData.Text = strPositionID
                '工作性質
                If beEmployeeLog.WorkTypeIDOld.Value.Trim <> "" Then
                    Dim aryDataValue = Split(beEmployeeLog.WorkTypeIDOld.Value.Trim, "|")
                    Dim aryData = Split(beEmployeeLog.WorkTypeOld.Value.Trim, ",")
                    Dim intCount As Integer
                    strWorkTypeID = ""
                    intCount = 0
                    For Each strKey As String In aryData
                        If strWorkTypeID = "" Then
                            strWorkTypeID = aryDataValue(intCount + 1) + " " + strKey
                        Else
                            strWorkTypeID = strPositionID + "," + aryDataValue(intCount + 1) + " " + strKey
                        End If
                        intCount = intCount + 2
                        If intCount > aryData.GetUpperBound(0) Then
                            Exit For
                        End If
                    Next
                End If
                lblWorkTypeIDOldData.Text = strWorkTypeID
                lblWorkStatusOldData.Text = beEmployeeLog.WorkStatusOld.Value + " " + beEmployeeLog.WorkStatusNameOld.Value '任職狀況
                '異動後
                lblGroupIDNewData.Text = beEmployeeLog.GroupID.Value + " " + beEmployeeLog.GroupName.Value  '事業群
                lblDeptIDNewData.Text = beEmployeeLog.DeptID.Value + " " + beEmployeeLog.DeptName.Value + "\" + beEmployeeLog.OrganID.Value + " " + beEmployeeLog.OrganName.Value '部門/科組課
                lblFlowOrganIDNewData.Text = beEmployeeLog.OrganID.Value + " " + beEmployeeLog.OrganName.Value  '最小簽核單位

                '主管任用方式
                If beEmployeeLog.BossType.Value = "1" Then
                    rbnBossType1Show.Checked = True
                End If
                If beEmployeeLog.BossType.Value = "2" Then
                    rbnBossType2Show.Checked = True
                End If
                '主管
                If beEmployeeLog.IsBoss.Value = "1" Then
                    chkIsBossShow.Checked = True
                End If
                If beEmployeeLog.IsSecBoss.Value = "1" Then
                    chkIsSecBossShow.Checked = True
                End If
                '簽核單位
                If beEmployeeLog.IsGroupBoss.Value = "1" Then
                    chkIsGroupBossShow.Checked = True
                End If
                If beEmployeeLog.IsSecGroupBoss.Value = "1" Then
                    chkIsSecGroupBossShow.Checked = True
                End If
                lblRankIDNewData.Text = beEmployeeLog.RankID.Value + "\" + beEmployeeLog.TitleID.Value + " " + beEmployeeLog.TitleName.Value    '職等\職稱
                '職位
                If beEmployeeLog.PositionID.Value.Trim <> "" Then
                    Dim aryDataValue = Split(beEmployeeLog.PositionID.Value.Trim, "|")
                    Dim aryData = Split(beEmployeeLog.Position.Value.Trim, ",")
                    Dim intCount As Integer
                    strPositionID = ""
                    intCount = 0
                    For Each strKey As String In aryData
                        If strPositionID = "" Then
                            strPositionID = aryDataValue(intCount + 1) + " " + strKey
                        Else
                            strPositionID = strPositionID + "," + aryDataValue(intCount + 1) + " " + strKey
                        End If
                        intCount = intCount + 2
                        If intCount > aryData.GetUpperBound(0) Then
                            Exit For
                        End If
                    Next
                End If
                lblPositionIDNewData.Text = strPositionID
                '工作性質
                If beEmployeeLog.WorkTypeID.Value.Trim <> "" Then
                    Dim aryDataValue = Split(beEmployeeLog.WorkTypeID.Value.Trim, "|")
                    Dim aryData = Split(beEmployeeLog.WorkType.Value.Trim, ",")
                    Dim intCount As Integer
                    strWorkTypeID = ""
                    intCount = 0
                    For Each strKey As String In aryData
                        If strWorkTypeID = "" Then
                            strWorkTypeID = aryDataValue(intCount + 1) + " " + strKey
                        Else
                            strWorkTypeID = strPositionID + "," + aryDataValue(intCount + 1) + " " + strKey
                        End If
                        intCount = intCount + 2
                        If intCount > aryData.GetUpperBound(0) Then
                            Exit For
                        End If
                    Next
                End If
                lblWorkTypeIDNewData.Text = strWorkTypeID
                lblWorkStatusNewData.Text = beEmployeeLog.WorkStatus.Value + " " + beEmployeeLog.WorkStatusName.Value   '任職狀況
                lblRemarkNewData.Text = beEmployeeLog.Remark.Value  '備註
            End Using


        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & ".subGetDataByEmployeeLog", ex)
        End Try

        'EmployeeLogWait
        Dim strMainPositionID As String = ""
        Dim strMainWorkTypeID As String = ""
        Dim bsEmployeeLogWait As New beEmployeeLogWait.Service()
        Dim beEmployeeLogWait As New beEmployeeLogWait.Row()
        beEmployeeLogWait.CompID.Value = CompID
        beEmployeeLogWait.EmpID.Value = EmpID
        beEmployeeLogWait.Wait_ValidDate.Value = ValidDate
        beEmployeeLogWait.Wait_Seq.Value = Seq
        beEmployeeLogWait.IDNo.Value = IDNo
        beEmployeeLogWait.ModifyDate.Value = ModifyDate
        beEmployeeLogWait.Reason.Value = Reason
        Try
            Using dt As DataTable = bsEmployeeLogWait.QueryByKey(beEmployeeLogWait).Tables(0)
                If dt.Rows.Count <= 0 Then
                    '異動前
                    Bsp.Utility.SetSelectedIndex(ddlCompIDOld, beEmployeeLog.CompIDOld.Value) '公司
                    lblGroupIDOldShow.Text = beEmployeeLog.GroupIDOld.Value + " " + beEmployeeLog.GroupNameOld.Value    '事業群
                    '部門
                    ucSelectHROrganOld.LoadData(beEmployeeLog.CompIDOld.Value)
                    ucSelectHROrganOld.setDeptID(beEmployeeLog.CompIDOld.Value, beEmployeeLog.DeptIDOld.Value, "N")
                    '科組課
                    ucSelectHROrganOld.setOrganID(beEmployeeLog.CompIDOld.Value, beEmployeeLog.OrganIDOld.Value, "N")
                    '最小簽核單位
                    GetEmpFlowOrganIDOld()
                    Bsp.Utility.SetSelectedIndex(ddlFlowOrganIDold, beEmployeeLog.FlowOrganIDOld.Value)
                    '職等職稱
                    Bsp.Utility.Rank(ddlRankIDOld, ddlCompIDOld.SelectedItem.Value)
                    ddlRankIDOld.Items.Insert(0, New ListItem("---請選擇---", ""))
                    Bsp.Utility.SetSelectedIndex(ddlRankIDOld, beEmployeeLog.RankIDOld.Value)
                    subLoadTitleIDDataOld()
                    Bsp.Utility.SetSelectedIndex(ddlTitleIDOld, beEmployeeLog.TitleIDOld.Value)
                    '職位
                    hidPositionIDOld.Value = beEmployeeLog.PositionIDOld.Value.Trim
                    If beEmployeeLog.PositionIDOld.Value.Trim <> "" Then
                        strMainPositionID = GetPositionIDOld(hidPositionIDOld.Value)
                    Else
                        strMainPositionID = ""
                    End If

                    Bsp.Utility.SetSelectedIndex(ddlPositionOld, strMainPositionID)
                    lblSelectPositionNameOld.Text = beEmployeeLog.PositionOld.Value.Trim
                    '工作性質
                    hidWorkTypeIDOld.Value = beEmployeeLog.WorkTypeIDOld.Value.Trim
                    strMainWorkTypeID = GetWorkTypeIDOld(hidWorkTypeIDOld.Value)
                    Bsp.Utility.SetSelectedIndex(ddlWorkTypeOld, strMainWorkTypeID)
                    lblSelectWorkTypeNameOld.Text = beEmployeeLog.WorkTypeOld.Value
                    Bsp.Utility.SetSelectedIndex(ddlWorkStatusOld, beEmployeeLog.WorkStatusOld.Value)   '任職狀況
                    '異動後
                    lblGroupIDNewShow.Text = beEmployeeLog.GroupID.Value + " " + beEmployeeLog.GroupName.Value  '事業群
                    '部門
                    ucSelectHROrganNew.LoadData(beEmployeeLog.CompID.Value)
                    ucSelectHROrganNew.setDeptID(beEmployeeLog.CompID.Value, beEmployeeLog.DeptID.Value, "N")
                    '科組課
                    ucSelectHROrganNew.setOrganID(beEmployeeLog.CompID.Value, beEmployeeLog.OrganID.Value, "N")
                    '最小簽核單位
                    If beEmployeeLog.FlowOrganID.Value <> "" Then
                        GetEmpFlowOrganIDNew()
                        Bsp.Utility.SetSelectedIndex(ddlFlowOrganIDNew, beEmployeeLog.FlowOrganID.Value)
                    End If
                    
                    '主管任用方式
                    Bsp.Utility.SetSelectedIndex(ddlBossTypeNewData, beEmployeeLog.BossType.Value)
                    '主管
                    If beEmployeeLog.IsBoss.Value = "1" Then
                        chkIsBoss.Checked = True
                    End If
                    If beEmployeeLog.IsSecBoss.Value = "1" Then
                        chkIsSecBoss.Checked = True
                    End If
                    '簽核單位
                    If beEmployeeLog.IsGroupBoss.Value = "1" Then
                        chkIsGroupBoss.Checked = True
                    End If
                    If beEmployeeLog.IsSecGroupBoss.Value = "1" Then
                        chkIsSecGroupBoss.Checked = True
                    End If
                    '職等職稱
                    Bsp.Utility.Rank(ddlRankIDNew, hidCompID.Value)
                    ddlRankIDOld.Items.Insert(0, New ListItem("---請選擇---", ""))
                    Bsp.Utility.SetSelectedIndex(ddlRankIDNew, beEmployeeLog.RankID.Value)
                    subLoadTitleIDDataNew()
                    Bsp.Utility.SetSelectedIndex(ddlTitleIDNew, beEmployeeLog.TitleID.Value)
                    '職位
                    hidPositionIDNew.Value = beEmployeeLog.PositionID.Value.Trim
                    If beEmployeeLog.PositionID.Value.Trim <> "" Then
                        strMainPositionID = GetPositionIDNew(hidPositionIDNew.Value)
                    Else
                        strMainPositionID = ""
                    End If
                    Bsp.Utility.SetSelectedIndex(ddlPositionNew, strMainPositionID)
                    lblSelectPositionNameNew.Text = beEmployeeLog.Position.Value.Trim
                    '工作性質
                    hidWorkTypeIDNew.Value = beEmployeeLog.WorkTypeID.Value.Trim
                    If beEmployeeLog.WorkTypeID.Value.Trim <> "" Then
                        strMainWorkTypeID = GetWorkTypeIDNew(hidWorkTypeIDNew.Value)
                    Else
                        strMainWorkTypeID = ""
                    End If
                    Bsp.Utility.SetSelectedIndex(ddlWorkTypeNew, strMainWorkTypeID)
                    lblSelectWorkTypeNameNew.Text = beEmployeeLog.WorkType.Value
                    Bsp.Utility.SetSelectedIndex(ddlWorkStatusNew, beEmployeeLog.WorkStatus.Value)   '任職狀況
                    '備註
                    txtRemarkNew.Text = beEmployeeLog.Remark.Value
                Else
                    beEmployeeLogWait = New beEmployeeLogWait.Row(dt.Rows(0))
                    '異動前
                    Bsp.Utility.SetSelectedIndex(ddlCompIDOld, beEmployeeLogWait.CompIDOld.Value) '公司
                    lblGroupIDOldShow.Text = beEmployeeLogWait.GroupIDOld.Value + " " + beEmployeeLogWait.GroupNameOld.Value    '事業群
                    '部門
                    ucSelectHROrganOld.LoadData(beEmployeeLogWait.CompIDOld.Value)
                    ucSelectHROrganOld.setDeptID(beEmployeeLogWait.CompIDOld.Value, beEmployeeLogWait.DeptIDOld.Value, "N")
                    '科組課
                    ucSelectHROrganOld.setOrganID(beEmployeeLogWait.CompIDOld.Value, beEmployeeLogWait.OrganIDOld.Value, "N")
                    '最小簽核單位
                    GetEmpFlowOrganIDOld()
                    Bsp.Utility.SetSelectedIndex(ddlFlowOrganIDOld, beEmployeeLogWait.FlowOrganIDOld.Value)
                    '職等職稱
                    Bsp.Utility.Rank(ddlRankIDOld, ddlCompIDOld.SelectedItem.Value)
                    ddlRankIDOld.Items.Insert(0, New ListItem("---請選擇---", ""))
                    Bsp.Utility.SetSelectedIndex(ddlRankIDOld, beEmployeeLogWait.RankIDOld.Value)
                    subLoadTitleIDDataOld()
                    Bsp.Utility.SetSelectedIndex(ddlTitleIDOld, beEmployeeLogWait.TitleIDOld.Value)
                    '職位
                    hidPositionIDOld.Value = beEmployeeLogWait.PositionIDOld.Value.Trim
                    If beEmployeeLogWait.PositionIDOld.Value.Trim <> "" Then
                        strMainPositionID = GetPositionIDOld(hidPositionIDOld.Value)
                    Else
                        strMainPositionID = ""
                    End If

                    Bsp.Utility.SetSelectedIndex(ddlPositionOld, strMainPositionID)
                    lblSelectPositionNameOld.Text = beEmployeeLogWait.PositionOld.Value.Trim
                    '工作性質
                    hidWorkTypeIDOld.Value = beEmployeeLogWait.WorkTypeIDOld.Value.Trim
                    strMainWorkTypeID = GetWorkTypeIDOld(hidWorkTypeIDOld.Value)
                    Bsp.Utility.SetSelectedIndex(ddlWorkTypeOld, strMainWorkTypeID)
                    lblSelectWorkTypeNameOld.Text = beEmployeeLogWait.WorkTypeOld.Value
                    Bsp.Utility.SetSelectedIndex(ddlWorkStatusOld, beEmployeeLogWait.WorkStatusOld.Value)   '任職狀況
                    '異動後
                    lblGroupIDNewShow.Text = beEmployeeLogWait.GroupID.Value + " " + beEmployeeLogWait.GroupName.Value  '事業群
                    '部門
                    ucSelectHROrganNew.LoadData(beEmployeeLogWait.CompID.Value)
                    ucSelectHROrganNew.setDeptID(beEmployeeLogWait.CompID.Value, beEmployeeLogWait.DeptID.Value, "N")
                    '科組課
                    ucSelectHROrganNew.setOrganID(beEmployeeLogWait.CompID.Value, beEmployeeLogWait.OrganID.Value, "N")
                    '最小簽核單位
                    If beEmployeeLogWait.FlowOrganID.Value <> "" Then
                        GetEmpFlowOrganIDNew()
                        Bsp.Utility.SetSelectedIndex(ddlFlowOrganIDNew, beEmployeeLogWait.FlowOrganID.Value)
                    End If
                    
                    '主管任用方式
                    Bsp.Utility.SetSelectedIndex(ddlBossTypeNewData, beEmployeeLogWait.BossType.Value)
                    '主管
                    If beEmployeeLogWait.IsBoss.Value = "1" Then
                        chkIsBoss.Checked = True
                    End If
                    If beEmployeeLogWait.IsSecBoss.Value = "1" Then
                        chkIsSecBoss.Checked = True
                    End If
                    '簽核單位
                    If beEmployeeLogWait.IsGroupBoss.Value = "1" Then
                        chkIsGroupBoss.Checked = True
                    End If
                    If beEmployeeLogWait.IsSecGroupBoss.Value = "1" Then
                        chkIsSecGroupBoss.Checked = True
                    End If
                    '職等職稱
                    Bsp.Utility.Rank(ddlRankIDNew, hidCompID.Value)
                    ddlRankIDOld.Items.Insert(0, New ListItem("---請選擇---", ""))
                    Bsp.Utility.SetSelectedIndex(ddlRankIDNew, beEmployeeLogWait.RankID.Value)
                    subLoadTitleIDDataNew()
                    Bsp.Utility.SetSelectedIndex(ddlTitleIDNew, beEmployeeLogWait.TitleID.Value)
                    '職位
                    hidPositionIDNew.Value = beEmployeeLogWait.PositionID.Value.Trim
                    If beEmployeeLogWait.PositionID.Value.Trim <> "" Then
                        strMainPositionID = GetPositionIDNew(hidPositionIDNew.Value)
                    Else
                        strMainPositionID = ""
                    End If

                    Bsp.Utility.SetSelectedIndex(ddlPositionNew, strMainPositionID)
                    lblSelectPositionNameNew.Text = beEmployeeLogWait.Position.Value.Trim
                    '工作性質
                    hidWorkTypeIDNew.Value = beEmployeeLogWait.WorkTypeID.Value.Trim
                    strMainWorkTypeID = GetWorkTypeIDNew(hidWorkTypeIDNew.Value)
                    Bsp.Utility.SetSelectedIndex(ddlWorkTypeNew, strMainWorkTypeID)
                    lblSelectWorkTypeNameNew.Text = beEmployeeLog.WorkType.Value
                    Bsp.Utility.SetSelectedIndex(ddlWorkStatusNew, beEmployeeLogWait.WorkStatus.Value)   '任職狀況
                    '備註
                    txtRemarkNew.Text = beEmployeeLogWait.Remark.Value
                End If

            End Using
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & ".subGetDataByEmployeeLogWait", ex)
        End Try
    End Sub

    Public Overrides Sub DoAction(ByVal Param As String)
        Select Case Param
            Case "btnAdd"   '存檔返回
                If funCheckData() Then
                    If SaveData() Then
                        GoBack()
                    End If
                End If
            Case "btnActionX"   '返回
                GoBack()
        End Select
    End Sub

    Private Sub GoBack()
        Dim ti As TransferInfo = Me.StateTransfer

        Dim btnA As New ButtonState(ButtonState.emButtonType.Add)
        Dim btnD As New ButtonState(ButtonState.emButtonType.Delete)
        Dim btnX As New ButtonState(ButtonState.emButtonType.Exit)

        If ViewState.Item("Detail") = "Y" Then
            btnX.Caption = "返回"

            Me.TransferFramePage("~/HR3/HR3010.aspx", New ButtonState() {btnX}, ti.Args)

            'Me.TransferFramePage("~/HR3/HR3010.aspx", New ButtonState() {btnX}, _
            '                     "SelectCompRoleID=" & hidCompID.Value, _
            '                     "SelectCompID=" & hidCompID.Value, _
            '                     "SelectEmpID=" & hidEmpID.Value.Trim, _
            '                     "SelectName=" & ViewState.Item("Name"), _
            '                     "SelectValidDate=" & hidValidDate.Value, _
            '                     "SelectSeq=" & hidSeq.Value, _
            '                     "PageNo=1", _
            '                     "DoQuery=Y")
        Else
            btnA.Caption = "輸入調整資料"
            btnD.Caption = "刪除"
            btnX.Caption = "返回"

            Me.TransferFramePage("~/HR3/HR3010.aspx", New ButtonState() {btnA, btnD, btnX}, ti.Args)

            'Me.TransferFramePage("~/HR3/HR3010.aspx", New ButtonState() {btnA, btnD, btnX}, _
            '                     "SelectCompRoleID=" & hidCompID.Value, _
            '                     "SelectCompID=" & hidCompID.Value, _
            '                     "SelectEmpID=" & hidEmpID.Value.Trim, _
            '                     "SelectName=" & ViewState.Item("Name"), _
            '                     "SelectValidDate=" & hidValidDate.Value, _
            '                     "SelectSeq=" & hidSeq.Value, _
            '                     "PageNo=1", _
            '                     "DoQuery=Y")
        End If

    End Sub


    Private Function SaveData() As Boolean
        Dim strWhere As String = ""
        Dim beEmployeeLogWait As New beEmployeeLogWait.Row()
        Dim bsEmployeeLogWait As New beEmployeeLogWait.Service()
        Dim objHR As New HR
        Dim objHR3000 As New HR3000

        Dim strGroupID As String = ""

        
        ''取得輸入資料
        beEmployeeLogWait.CompID.Value = hidCompID.Value
        beEmployeeLogWait.EmpID.Value = hidEmpID.Value
        beEmployeeLogWait.Wait_ValidDate.Value = hidValidDate.Value
        beEmployeeLogWait.Wait_Seq.Value = hidSeq.Value
        beEmployeeLogWait.IDNo.Value = hidIDNo.Value
        beEmployeeLogWait.ModifyDate.Value = hidModifyDate.Value
        beEmployeeLogWait.Reason.Value = hidReason.Value

        '異動後
        beEmployeeLogWait.Company.Value = objHR.GetHRCompName(hidCompID.Value).Rows(0).Item("CompName").ToString
        beEmployeeLogWait.DeptID.Value = ucSelectHROrganNew.SelectedDeptID
        beEmployeeLogWait.DeptName.Value = ucSelectHROrganNew.SelectedDeptName
        beEmployeeLogWait.OrganID.Value = ucSelectHROrganNew.SelectedDeptID
        beEmployeeLogWait.OrganName.Value = ucSelectHROrganNew.SelectedOrganName
        If ucSelectHROrganNew.SelectedOrganID = "" Then
            beEmployeeLogWait.GroupID.Value = ucSelectHROrganNew.SelectedDeptID
        Else
            '2016/05/03 SPHBKC資料已併入OrganizationFlow中
            'If hidCompID.Value = "SPHBKC" Then
            '    strGroupID = objHR3000.Get_CGroupID(beEmployeeLogWait.OrganID.Value)
            'Else
            strGroupID = objHR3000.Get_GroupID(beEmployeeLogWait.OrganID.Value)
            'End If
            beEmployeeLogWait.GroupID.Value = strGroupID
        End If

        '2016/05/03 SPHBKC資料已併入OrganizationFlow中
        'If hidCompID.Value = "SPHBKC" Then
        '    beEmployeeLogWait.GroupName.Value = objHR3000.Get_CGroupInfo(beEmployeeLogWait.GroupID.Value)
        'Else
        beEmployeeLogWait.GroupName.Value = objHR3000.Get_GroupInfo(beEmployeeLogWait.GroupID.Value)
        'End If

        beEmployeeLogWait.FlowOrganID.Value = ddlFlowOrganIDNew.SelectedValue
        If ddlFlowOrganIDNew.SelectedValue <> "" Then
            beEmployeeLogWait.FlowOrganName.Value = ddlFlowOrganIDNew.SelectedItem.Text.Trim.Split(" ")(1).ToString
        End If
        beEmployeeLogWait.RankID.Value = ddlRankIDNew.SelectedValue
        beEmployeeLogWait.TitleID.Value = ddlTitleIDNew.SelectedValue
        If ddlTitleIDNew.SelectedValue <> "" Then
            beEmployeeLogWait.TitleName.Value = ddlTitleIDNew.SelectedItem.Text.Trim.Split("-")(1).ToString
        End If
        beEmployeeLogWait.PositionID.Value = hidPositionIDNew.Value
        beEmployeeLogWait.Position.Value = lblSelectPositionNameNew.Text.Trim
        beEmployeeLogWait.WorkTypeID.Value = hidWorkTypeIDNew.Value
        beEmployeeLogWait.WorkType.Value = lblSelectWorkTypeNameNew.Text.Trim
        beEmployeeLogWait.WorkStatus.Value = ddlWorkStatusNew.SelectedValue
        If ddlWorkStatusNew.SelectedValue <> "" Then
            beEmployeeLogWait.WorkStatusName.Value = ddlWorkStatusNew.SelectedItem.Text.Trim.Split(" ")(1).ToString
        End If
        beEmployeeLogWait.Remark.Value = txtRemarkNew.Text.Trim
        beEmployeeLogWait.BossType.Value = ddlBossTypeNewData.SelectedValue
        beEmployeeLogWait.IsBoss.Value = IIf(chkIsBoss.Checked, "1", "0")
        beEmployeeLogWait.IsSecBoss.Value = IIf(chkIsSecBoss.Checked, "1", "0")
        beEmployeeLogWait.IsGroupBoss.Value = IIf(chkIsGroupBoss.Checked, "1", "0")
        beEmployeeLogWait.IsSecGroupBoss.Value = IIf(chkIsSecGroupBoss.Checked, "1", "0")

        '異動前
        beEmployeeLogWait.CompIDOld.Value = ddlCompIDOld.SelectedValue
        If ddlCompIDOld.SelectedValue <> "" Then
            beEmployeeLogWait.CompanyOld.Value = ddlCompIDOld.SelectedItem.Text.Trim
        End If
        beEmployeeLogWait.DeptIDOld.Value = ucSelectHROrganOld.SelectedDeptID
        beEmployeeLogWait.DeptNameOld.Value = ucSelectHROrganOld.SelectedDeptName
        beEmployeeLogWait.OrganIDOld.Value = ucSelectHROrganOld.SelectedOrganID
        beEmployeeLogWait.OrganNameOld.Value = ucSelectHROrganOld.SelectedOrganName
        If ucSelectHROrganOld.SelectedOrganID = "" Then
            beEmployeeLogWait.GroupIDOld.Value = ucSelectHROrganOld.SelectedDeptID
        Else
            '2016/05/03 SPHBKC資料已併入OrganizationFlow中
            'If ddlCompIDOld.SelectedValue = "SPHBKC" Then
            '    strGroupID = objHR3000.Get_CGroupID(beEmployeeLogWait.OrganIDOld.Value)
            'Else
            strGroupID = objHR3000.Get_GroupID(beEmployeeLogWait.OrganIDOld.Value)
            'End If

            beEmployeeLogWait.GroupIDOld.Value = strGroupID
        End If

        '2016/05/03 SPHBKC資料已併入OrganizationFlow中
        'If ddlCompIDOld.SelectedValue = "SPHBKC" Then
        '    beEmployeeLogWait.GroupNameOld.Value = objHR3000.Get_CGroupInfo(beEmployeeLogWait.GroupIDOld.Value)
        'Else
        beEmployeeLogWait.GroupNameOld.Value = objHR3000.Get_GroupInfo(beEmployeeLogWait.GroupIDOld.Value)
        'End If

        beEmployeeLogWait.FlowOrganIDOld.Value = ddlFlowOrganIDOld.SelectedValue
        If ddlFlowOrganIDOld.SelectedValue <> "" Then
            beEmployeeLogWait.FlowOrganNameOld.Value = ddlFlowOrganIDOld.SelectedItem.Text.Trim.Split(" ")(1).ToString
        End If
        beEmployeeLogWait.RankIDOld.Value = ddlRankIDOld.SelectedValue
        beEmployeeLogWait.TitleIDOld.Value = ddlTitleIDOld.SelectedValue
        If ddlTitleIDOld.SelectedValue <> "" Then
            beEmployeeLogWait.TitleNameOld.Value = ddlTitleIDOld.SelectedItem.Text.Trim.Split("-")(1).ToString
        End If
        beEmployeeLogWait.PositionIDOld.Value = hidPositionIDOld.Value
        beEmployeeLogWait.PositionOld.Value = lblSelectPositionNameOld.Text.Trim
        beEmployeeLogWait.WorkTypeIDOld.Value = hidWorkTypeIDOld.Value
        beEmployeeLogWait.WorkTypeOld.Value = lblSelectWorkTypeNameOld.Text.Trim
        beEmployeeLogWait.WorkStatusOld.Value = ddlWorkStatusOld.SelectedValue
        If ddlWorkStatusOld.SelectedValue <> "" Then
            beEmployeeLogWait.WorkStatusNameOld.Value = ddlWorkStatusOld.SelectedItem.Text.Trim.Split(" ")(1).ToString
        End If
        beEmployeeLogWait.LastChgComp.Value = UserProfile.ActCompID
        beEmployeeLogWait.LastChgID.Value = UserProfile.ActUserID
        beEmployeeLogWait.LastChgDate.Value = Now

        '儲存資料
        Try
            '檢查資料是否存在
            If bsEmployeeLogWait.IsDataExists(beEmployeeLogWait) Then
                Return objHR3000.UpdateEmployeeLogWati(beEmployeeLogWait)
            Else
                Return objHR3000.AddEmployeeLogWati(beEmployeeLogWait)
            End If


        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, "[CC0201_99]" & Bsp.Utility.getMessage("E_00000") & Bsp.Utility.getInnerException(Me.FunID, ex))
            Return False
        End Try
    End Function


#Region "CheckData:EmployeeLogWait"
    Private Function funCheckData() As Boolean
        Dim objHR As New HR
        Dim objHR3000 As New HR3000

        Dim strValue As String = ""
        Dim strReason As String = hidReason.Value

        Dim strWhere As String = ""

        '異動前
        '公司名稱
        strValue = ddlCompIDOld.SelectedValue
        If strValue = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", "異動前公司名稱")
            ddlCompIDOld.Focus()
            Return False
        End If
        '事業群
        '2016/05/03 SPHBKC資料已併入OrganizationFlow中
        'If ddlCompIDOld.SelectedValue = "SPHBKC" Then
        '    If objHR3000.Get_CGroupID(ucSelectHROrganOld.SelectedOrganID) = "" Then
        '        Bsp.Utility.ShowFormatMessage(Me, "W_00030", "異動前事業群")
        '        ucSelectHROrganOld.Focus()
        '        Return False
        '    End If
        'Else
        If objHR3000.Get_GroupID(ucSelectHROrganOld.SelectedOrganID) = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", "異動前事業群")
            ucSelectHROrganOld.Focus()
            Return False
        End If
        'End If

        '部門\科組課
        If ucSelectHROrganOld.SelectedDeptID = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", "異動前部門")
            Return False
        Else
            If Not objHR.ChkOrganIsVlaid(ddlCompIDOld.SelectedValue, ucSelectHROrganOld.SelectedDeptID) Then
                Bsp.Utility.ShowMessage(Me, "異動前部門：所選擇部門的是無效單位！")
                Return False
            End If
        End If

        If ucSelectHROrganOld.SelectedOrganID = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", "異動前科/組/課")
            Return False
        Else
            If Not objHR.ChkOrganIsVlaid(ddlCompIDOld.SelectedValue, ucSelectHROrganOld.SelectedOrganID) Then
                Bsp.Utility.ShowMessage(Me, "異動前科/組/課：所選擇科/組/課的是無效單位！")
                Return False
            End If
        End If
        '最小簽核單位
        '職等\職稱
        strValue = ddlRankIDOld.SelectedValue
        If strValue = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", "異動前職等")
            ddlRankIDOld.Focus()
            Return False
        End If
        strValue = ddlTitleIDOld.SelectedValue
        If strValue = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", "異動前職稱")
            ddlTitleIDOld.Focus()
            Return False
        End If
        '職位
        If objHR.IsRankIDMapFlag(ddlCompIDOld.SelectedValue) Then
            If Len(hidPositionIDOld.Value) = 0 Then
                Bsp.Utility.ShowFormatMessage(Me, "W_00030", "異動前職位")
                ddlPositionOld.Focus()
                Return False
            Else
                If ddlPositionOld.Items.Count > 7 Then '職位不可超過7個
                    Bsp.Utility.ShowFormatMessage(Me, "異動前職位不可超過7個")
                    ddlPositionOld.Focus()
                    Return False
                End If
            End If
        End If
        '工作性質
        If Len(hidWorkTypeIDOld.Value) = 0 Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", "異動前工作性質")
            ddlWorkTypeOld.Focus()
            Return False
        Else
            If ddlWorkTypeOld.Items.Count > 7 Then '工作性質不可超過7個
                Bsp.Utility.ShowFormatMessage(Me, "異動前工作性質不可超過7個")
                ddlWorkTypeOld.Focus()
                Return False
            End If
        End If
        '任職狀況
        strValue = ddlWorkStatusOld.SelectedValue()
        If strValue = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", "異動前任職狀況")
            ddlTitleIDOld.Focus()
            Return False
        End If

        '異動後
        '公司名稱
        If ddlWorkStatusNew.SelectedValue = "1" Or ddlWorkStatusNew.SelectedValue = "4" Or ddlWorkStatusNew.SelectedValue = "5" Or ddlWorkStatusNew.SelectedValue = "7" Then
            '事業群
            '2016/05/03 SPHBKC資料已併入OrganizationFlow中
            'If hidCompID.Value = "SPHBKC" Then
            '    If objHR3000.Get_CGroupID(ucSelectHROrganNew.SelectedOrganID) = "" Then
            '        Bsp.Utility.ShowFormatMessage(Me, "W_00030", "異動前事業群")
            '        ucSelectHROrganNew.Focus()
            '        Return False
            '    End If
            'Else
            If objHR3000.Get_GroupID(ucSelectHROrganNew.SelectedOrganID) = "" Then
                Bsp.Utility.ShowFormatMessage(Me, "W_00030", "異動前事業群")
                ucSelectHROrganNew.Focus()
                Return False
            End If
            'End If

            '部門\科組課
            If ucSelectHROrganNew.SelectedDeptID = "" Then
                Bsp.Utility.ShowFormatMessage(Me, "W_00030", "異動後部門")
                Return False
            Else
                If Not objHR.ChkOrganIsVlaid(hidCompID.Value, ucSelectHROrganNew.SelectedDeptID) Then
                    Bsp.Utility.ShowMessage(Me, "異動後部門：所選擇部門的是無效單位！")
                    Return False
                End If
            End If

            If ucSelectHROrganNew.SelectedOrganID = "" Then
                Bsp.Utility.ShowFormatMessage(Me, "W_00030", "異動後科/組/課")
                Return False
            Else
                If Not objHR.ChkOrganIsVlaid(hidCompID.Value, ucSelectHROrganNew.SelectedOrganID) Then
                    Bsp.Utility.ShowMessage(Me, "異動後科/組/課：所選擇科/組/課的是無效單位！")
                    Return False
                End If
            End If
            '最小簽核單位
            '主管
            If ddlBossTypeNewData.SelectedValue <> "" Then
                If Not chkIsBoss.Checked And Not chkIsSecBoss.Checked And Not chkIsGroupBoss.Checked And Not chkIsSecGroupBoss.Checked Then
                    Bsp.Utility.ShowFormatMessage(Me, "H_00000", "已選擇異動後主管任用方式，請至少選擇一個單位(副)主或簽核單位(副)主管")
                    chkIsBoss.Focus()
                    Return False
                End If
            End If
            '職等\職稱
            strValue = ddlRankIDNew.SelectedValue
            If strValue = "" Then
                Bsp.Utility.ShowFormatMessage(Me, "W_00030", "異動後職等")
                ddlRankIDNew.Focus()
                Return False
            End If
            strValue = ddlTitleIDNew.SelectedValue
            If strValue = "" Then
                Bsp.Utility.ShowFormatMessage(Me, "W_00030", "異動後職稱")
                ddlTitleIDNew.Focus()
                Return False
            End If
            '職位
            If objHR.IsRankIDMapFlag(hidCompID.Value) Then
                If Len(hidPositionIDNew.Value) = 0 Then
                    Bsp.Utility.ShowFormatMessage(Me, "W_00030", "異動後職位")
                    ddlPositionNew.Focus()
                    Return False
                Else
                    If ddlPositionNew.Items.Count > 7 Then '職位不可超過7個
                        Bsp.Utility.ShowFormatMessage(Me, "異動後職位不可超過7個")
                        ddlPositionNew.Focus()
                        Return False
                    End If
                End If
            End If
            '工作性質
            If Len(hidWorkTypeIDNew.Value) = 0 Then
                Bsp.Utility.ShowFormatMessage(Me, "W_00030", "異動後工作性質")
                ddlWorkTypeNew.Focus()
                Return False
            Else
                If ddlWorkTypeNew.Items.Count > 7 Then '工作性質不可超過7個
                    Bsp.Utility.ShowFormatMessage(Me, "異動後工作性質不可超過7個")
                    ddlWorkTypeNew.Focus()
                    Return False
                End If
            End If
        End If
        '任職狀況
        strValue = ddlWorkStatusNew.SelectedValue()
        If strValue = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", "異動後任職狀況")
            ddlTitleIDOld.Focus()
            Return False
        End If

        Return True
    End Function
#End Region
#Region "ddlComp"    '公司
    Protected Sub ddlCompIDOld_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlCompIDOld.SelectedIndexChanged
        ViewState.Item("CompIDOld") = ddlCompIDOld.SelectedValue
        '部門、科組課
        ucSelectHROrganOld.LoadData(ViewState.Item("CompIDOld"))
        ddlFlowOrganIDOld.Items.Clear()
        ddlFlowOrganIDOld.Items.Insert(0, New ListItem("---請選擇---", ""))

        '職等           
        Bsp.Utility.Rank(ddlRankIDOld, ViewState.Item("CompIDOld"))
        ddlRankIDOld.Items.Insert(0, New ListItem("---請選擇---", ""))

    End Sub
#End Region

    Protected Sub ucSelectPositionOld_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ucSelectPositionOld.Load
        '載入按鈕-職位選單畫面
        If ddlCompIDOld.SelectedItem.Value <> "" Then
            ViewState.Item("DataType") = "Old"
            ucSelectPositionOld.QueryCompID = ddlCompIDOld.SelectedItem.Value
            ucSelectPositionOld.QueryEmpID = ""
            ucSelectPositionOld.QueryOrganID = ucSelectHROrganOld.SelectedOrganID
            ucSelectPositionOld.DefaultPosition = lblSelectPositionOld.Text
            ucSelectPositionOld.Fields = New FieldState() { _
                    New FieldState("PositionID", "職位代碼", True, True), _
                    New FieldState("Remark", "職位名稱", True, True)}
        End If

    End Sub
    Protected Sub ucSelectPositionNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ucSelectPositionNew.Load
        '載入按鈕-職位選單畫面
        If hidCompID.Value <> "" Then
            ucSelectPositionNew.QueryCompID = hidCompID.Value
            ucSelectPositionNew.QueryEmpID = ""
            ucSelectPositionNew.QueryOrganID = ucSelectHROrganNew.SelectedOrganID
            ucSelectPositionNew.DefaultPosition = lblSelectPositionNew.Text
            ucSelectPositionNew.Fields = New FieldState() { _
                    New FieldState("PositionID", "職位代碼", True, True), _
                    New FieldState("Remark", "職位名稱", True, True)}
        End If

    End Sub

    Protected Sub ucSelectWorkTypeOld_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ucSelectWorkTypeOld.Load
        '載入按鈕-工作性質選單畫面
        If ddlCompIDOld.SelectedItem.Value <> "" Then
            ucSelectWorkTypeOld.QueryCompID = ddlCompIDOld.SelectedItem.Value
            ucSelectWorkTypeOld.QueryEmpID = ""
            ucSelectWorkTypeOld.QueryOrganID = ucSelectHROrganOld.SelectedOrganID
            ucSelectWorkTypeOld.DefaultWorkType = lblSelectWorkTypeOld.Text
            ucSelectWorkTypeOld.Fields = New FieldState() { _
                    New FieldState("WorkTypeID", "工作性質代碼", True, True), _
                    New FieldState("Remark", "工作性質名稱", True, True)}
        End If

    End Sub
    Protected Sub ucSelectWorkTypeNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ucSelectWorkTypeNew.Load
        '載入按鈕-工作性質選單畫面
        If hidCompID.Value <> "" Then
            ViewState.Item("DataType") = "New"
            ucSelectWorkTypeNew.QueryCompID = hidCompID.Value
            ucSelectWorkTypeNew.QueryEmpID = ""
            ucSelectWorkTypeNew.QueryOrganID = ucSelectHROrganNew.SelectedOrganID
            ucSelectWorkTypeNew.DefaultWorkType = lblSelectWorkTypeNew.Text
            ucSelectWorkTypeNew.Fields = New FieldState() { _
                    New FieldState("WorkTypeID", "工作性質代碼", True, True), _
                    New FieldState("Remark", "工作性質名稱", True, True)}
        End If

    End Sub

    '將選擇那筆 改為 第一筆為主要職位
    Protected Sub ddlPositionOld_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlPositionOld.SelectedIndexChanged
        Dim strRst1 As String = ""
        Dim strRst2 As String = ""
        Dim strMainPosition As String = ""
        Dim strPosition As String = ""
        Dim strRstName1 As String = ""
        Dim strRstName2 As String = ""
        For i As Integer = 0 To ddlPositionOld.Items.Count - 1

            If ddlPositionOld.Items(i).Selected Then
                strRst1 = "'" + ddlPositionOld.Items(i).Value + "'"
                strMainPosition = "1|" + ddlPositionOld.Items(i).Value
                strRstName1 = ddlPositionOld.Items(i).Text.Trim.Split("-")(1).ToString
            Else
                If strRst2 <> "" Then strRst2 += ","
                strRst2 += "'" + ddlPositionOld.Items(i).Value + "'"
                If strPosition <> "" Then strPosition += "|"
                strPosition += "0|" + ddlPositionOld.Items(i).Value
                If strRstName2 <> "" Then strRstName2 += ","
                strRstName2 += ddlPositionOld.Items(i).Text.Trim.Split("-")(1).ToString
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
    Protected Sub ddlPositionNew_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlPositionNew.SelectedIndexChanged
        Dim strRst1 As String = ""
        Dim strRst2 As String = ""
        Dim strMainPosition As String = ""
        Dim strPosition As String = ""
        Dim strRstName1 As String = ""
        Dim strRstName2 As String = ""
        For i As Integer = 0 To ddlPositionNew.Items.Count - 1

            If ddlPositionNew.Items(i).Selected Then
                strRst1 = "'" + ddlPositionNew.Items(i).Value + "'"
                strMainPosition = "1|" + ddlPositionNew.Items(i).Value
                strRstName1 = ddlPositionNew.Items(i).Text.Trim.Split("-")(1).ToString
            Else
                If strRst2 <> "" Then strRst2 += ","
                strRst2 += "'" + ddlPositionNew.Items(i).Value + "'"
                If strPosition <> "" Then strPosition += "|"
                strPosition += "0|" + ddlPositionNew.Items(i).Value
                If strRstName2 <> "" Then strRstName2 += ","
                strRstName2 += ddlPositionNew.Items(i).Text.Trim.Split("-")(1).ToString
            End If
        Next
        If strRst2 = "" Then
            lblSelectPositionNew.Text = strRst1
            hidPositionIDNew.Value = strMainPosition
            lblSelectPositionNameNew.Text = strRstName1
        Else
            lblSelectPositionNew.Text = strRst1 + "," + strRst2
            hidPositionIDNew.Value = strMainPosition + "|" + strPosition
            lblSelectPositionNameNew.Text = strRstName1 + "," + strRstName2
        End If
    End Sub
    '將選擇那筆 改為 第一筆為主要工作性質
    Protected Sub ddlWorkTypeOld_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlWorkTypeOld.SelectedIndexChanged
        Dim strRst1 As String = ""
        Dim strRst2 As String = ""
        Dim strMainWorkType As String = ""
        Dim strWorkType As String = ""
        Dim strRstName1 As String = ""
        Dim strRstName2 As String = ""
        For i As Integer = 0 To ddlWorkTypeOld.Items.Count - 1

            If ddlWorkTypeOld.Items(i).Selected Then
                strRst1 = "'" + ddlWorkTypeOld.Items(i).Value + "'"
                strMainWorkType = "1|" + ddlWorkTypeOld.Items(i).Value
                strRstName1 = ddlWorkTypeOld.Items(i).Text.Trim.Split("-")(1).ToString
            Else
                If strRst2 <> "" Then strRst2 += ","
                strRst2 += "'" + ddlWorkTypeOld.Items(i).Value + "'"
                If strWorkType <> "" Then strWorkType += "|"
                strWorkType += "0|" + ddlWorkTypeOld.Items(i).Value
                If strRstName2 <> "" Then strRstName2 += ","
                strRstName2 += ddlWorkTypeOld.Items(i).Text.Trim.Split("-")(1).ToString
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
    Protected Sub ddlWorkTypeNew_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlWorkTypeNew.SelectedIndexChanged
        Dim strRst1 As String = ""
        Dim strRst2 As String = ""
        Dim strMainWorkType As String = ""
        Dim strWorkType As String = ""
        Dim strRstName1 As String = ""
        Dim strRstName2 As String = ""
        For i As Integer = 0 To ddlWorkTypeNew.Items.Count - 1

            If ddlWorkTypeNew.Items(i).Selected Then
                strRst1 = "'" + ddlWorkTypeNew.Items(i).Value + "'"
                strMainWorkType = "1|" + ddlWorkTypeNew.Items(i).Value
                strRstName1 = ddlWorkTypeNew.Items(i).Text.Trim.Split("-")(1).ToString
            Else
                If strRst2 <> "" Then strRst2 += ","
                strRst2 += "'" + ddlWorkTypeNew.Items(i).Value + "'"
                If strWorkType <> "" Then strWorkType += "|"
                strWorkType += "0|" + ddlWorkTypeNew.Items(i).Value
                If strRstName2 <> "" Then strRstName2 += ","
                strRstName2 += ddlWorkTypeNew.Items(i).Text.Trim.Split("-")(1).ToString
            End If
        Next
        If strRst2 = "" Then
            lblSelectWorkTypeNew.Text = strRst1
            hidWorkTypeIDNew.Value = strMainWorkType
            lblSelectWorkTypeNameNew.Text = strRstName1
        Else
            lblSelectWorkTypeNew.Text = strRst1 + "," + strRst2
            hidWorkTypeIDNew.Value = strMainWorkType + "|" + strWorkType
            lblSelectWorkTypeNameNew.Text = strRstName1 + "," + strRstName2
        End If
    End Sub
#Region "ucSelectHROrgan"    '
    Protected Sub ucSelectHROrganOld_ucSelectOrganIDSelectedIndexChangedHandler_SelectChange(ByVal sender As Object, ByVal e As System.EventArgs) Handles ucSelectHROrganOld.ucSelectOrganIDSelectedIndexChangedHandler_SelectChange
        Dim objHR As New HR
        Dim objHR3000 As New HR3000
        Dim strGroupID As String = ""

        '2016/05/03 SPHBKC資料已併入OrganizationFlow中
        'If ddlCompIDOld.SelectedValue = "SPHBKC" Then
        '    strGroupID = objHR3000.Get_CGroupID(ucSelectHROrganOld.SelectedOrganID)
        'Else
        strGroupID = objHR3000.Get_GroupID(ucSelectHROrganOld.SelectedOrganID)
        'End If

        '事業群
        If strGroupID <> "" Then
            '2016/05/03 SPHBKC資料已併入OrganizationFlow中
            'If ddlCompIDOld.SelectedValue = "SPHBKC" Then
            '    lblGroupIDOldShow.Text = strGroupID + " " + objHR3000.Get_CGroupInfo(strGroupID)
            'Else
            lblGroupIDOldShow.Text = strGroupID + " " + objHR3000.Get_GroupInfo(strGroupID)
            'End If

            UpdGroupIDOld.Update()
        End If

        '最小簽核單位
        GetEmpFlowOrganIDOld()

    End Sub
    Protected Sub ucSelectHROrganNew_ucSelectOrganIDSelectedIndexChangedHandler_SelectChange(ByVal sender As Object, ByVal e As System.EventArgs) Handles ucSelectHROrganNew.ucSelectOrganIDSelectedIndexChangedHandler_SelectChange
        Dim objHR As New HR
        Dim objHR3000 As New HR3000

        Dim strGroupID As String = ""
        '2016/05/03 SPHBKC資料已併入OrganizationFlow中
        'If hidCompID.Value = "SPHBKC" Then
        '    strGroupID = objHR3000.Get_CGroupID(ucSelectHROrganNew.SelectedOrganID)
        'Else
        strGroupID = objHR3000.Get_GroupID(ucSelectHROrganNew.SelectedOrganID)
        'End If

        '事業群
        If strGroupID <> "" Then
            '2016/05/03 SPHBKC資料已併入OrganizationFlow中
            'If hidCompID.Value = "SPHBKC" Then
            '    lblGroupIDNewShow.Text = strGroupID + " " + objHR3000.Get_CGroupInfo(strGroupID)
            'Else
            lblGroupIDNewShow.Text = strGroupID + " " + objHR3000.Get_GroupInfo(strGroupID)
            'End If

            UpdGroupIDNew.Update()
        End If
        '最小簽核單位
        GetEmpFlowOrganIDNew()

    End Sub
#End Region


    Public Overrides Sub DoModalReturn(ByVal returnValue As String)

        Dim strSql As String = ""
        Dim strRstName1 As String = ""
        Dim strRstName2 As String = ""

        If returnValue <> "" Then
            Dim aryData() As String = returnValue.Split(":")

            Select Case aryData(0)
                Case "ucSelectPositionNew"
                    lblSelectPositionNew.Text = aryData(1)

                    If lblSelectPositionNew.Text <> "''" Then  '非必填時，回傳空值
                        '載入 職位 下拉式選單
                        'strSql = " and PositionID in (" + lblSelectPosition.Text + ") "
                        strSql = " and PositionID in (" + lblSelectPositionNew.Text + ") and CompID = '" + hidCompID.Value + "'"
                        Bsp.Utility.Position(ddlPositionNew, "PositionID", , strSql)
                        'Bsp.Utility.RE_PositionU(ddlPosition, "PositionID")

                        
                        '第一筆為主要職位
                        Dim strDefaultValue() As String = lblSelectPositionNew.Text.Replace("'", "").Split(",")
                        Dim strPosition As String = ""
                        Bsp.Utility.SetSelectedIndex(ddlPositionNew, strDefaultValue(0))
                        For intLoop As Integer = 0 To strDefaultValue.GetUpperBound(0)
                            If intLoop = 0 Then
                                strPosition = "1|" + strDefaultValue(intLoop)
                            Else
                                strPosition = strPosition + "|0|" + strDefaultValue(intLoop)
                            End If
                        Next
                        hidPositionIDNew.Value = strPosition
                        For i As Integer = 0 To ddlPositionNew.Items.Count - 1
                            If ddlPositionNew.Items(i).Selected Then
                                strRstName1 = ddlPositionNew.Items(i).Text.Trim.Split("-")(1).ToString
                            Else
                                If strRstName2 <> "" Then strRstName2 += ","
                                strRstName2 += ddlPositionNew.Items(i).Text.Trim.Split("-")(1).ToString
                            End If
                        Next
                        If strRstName2 = "" Then
                            lblSelectWorkTypeNameNew.Text = strRstName1
                        Else
                            lblSelectPositionNameNew.Text = strRstName1 + "," + strRstName2
                        End If
                    Else
                        ddlPositionNew.Items.Clear()
                        ddlPositionNew.Items.Insert(0, New ListItem("---請選擇---", ""))
                    End If
                Case "ucSelectPositionOld"
                    lblSelectPositionOld.Text = aryData(1)

                    If lblSelectPositionOld.Text <> "''" Then  '非必填時，回傳空值
                        '載入 職位 下拉式選單
                        'strSql = " and PositionID in (" + lblSelectPosition.Text + ") "
                        strSql = " and PositionID in (" + lblSelectPositionOld.Text + ") and CompID = '" + ddlCompIDOld.SelectedItem.Value + "'"
                        Bsp.Utility.Position(ddlPositionOld, "PositionID", , strSql)
                        'Bsp.Utility.RE_PositionU(ddlPosition, "PositionID")
                        '第一筆為主要職位
                        Dim strDefaultValue() As String = lblSelectPositionOld.Text.Replace("'", "").Split(",")
                        Dim strPosition As String = ""
                        Bsp.Utility.SetSelectedIndex(ddlPositionOld, strDefaultValue(0))
                        For intLoop As Integer = 0 To strDefaultValue.GetUpperBound(0)
                            If intLoop = 0 Then
                                strPosition = "1|" + strDefaultValue(intLoop)
                            Else
                                strPosition = strPosition + "|0|" + strDefaultValue(intLoop)
                            End If
                        Next
                        hidPositionIDOld.Value = strPosition
                    Else
                        ddlPositionOld.Items.Clear()
                        ddlPositionOld.Items.Insert(0, New ListItem("---請選擇---", ""))
                    End If
                    For i As Integer = 0 To ddlPositionOld.Items.Count - 1
                        If ddlPositionOld.Items(i).Selected Then
                            strRstName1 = ddlPositionOld.Items(i).Text.Trim.Split("-")(1).ToString
                        Else
                            If strRstName2 <> "" Then strRstName2 += ","
                            strRstName2 += ddlPositionOld.Items(i).Text.Trim.Split("-")(1).ToString
                        End If
                    Next
                    If strRstName2 = "" Then
                        lblSelectPositionNameOld.Text = strRstName1
                    Else
                        lblSelectPositionNameOld.Text = strRstName1 + "," + strRstName2
                    End If
                Case "ucSelectWorkTypeNew"
                    lblSelectWorkTypeNew.Text = aryData(1)

                    If lblSelectWorkTypeNew.Text <> "''" Then  '非必填時，回傳空值
                        '載入 工作性質 下拉式選單
                        strSql = " and WorkTypeID in (" + lblSelectWorkTypeNew.Text + ") and CompID = '" + hidCompID.Value + "'"
                        Bsp.Utility.WorkType(ddlWorkTypeNew, "WorkTypeID", , strSql)
                        '第一筆為主要工作性質
                        Dim strDefaultValue() As String = lblSelectWorkTypeNew.Text.Replace("'", "").Split(",")
                        Dim strWorkType As String = ""
                        Bsp.Utility.SetSelectedIndex(ddlWorkTypeNew, strDefaultValue(0))
                        For intLoop As Integer = 0 To strDefaultValue.GetUpperBound(0)
                            If intLoop = 0 Then
                                strWorkType = "1|" + strDefaultValue(intLoop)
                            Else
                                strWorkType = strWorkType + "|0|" + strDefaultValue(intLoop)
                            End If
                        Next
                        hidWorkTypeIDNew.Value = strWorkType
                    Else
                        ddlWorkTypeNew.Items.Clear()
                        ddlWorkTypeNew.Items.Insert(0, New ListItem("---請選擇---", ""))
                    End If
                    For i As Integer = 0 To ddlWorkTypeNew.Items.Count - 1
                        If ddlWorkTypeNew.Items(i).Selected Then
                            strRstName1 = ddlWorkTypeNew.Items(i).Text.Trim.Split("-")(1).ToString
                        Else
                            If strRstName2 <> "" Then strRstName2 += ","
                            strRstName2 += ddlWorkTypeNew.Items(i).Text.Trim.Split("-")(1).ToString
                        End If
                    Next
                    If strRstName2 = "" Then
                        lblSelectWorkTypeNameNew.Text = strRstName1
                    Else
                        lblSelectWorkTypeNameNew.Text = strRstName1 + "," + strRstName2
                    End If
                Case "ucSelectWorkTypeOld"

                    lblSelectWorkTypeOld.Text = aryData(1)

                    If lblSelectWorkTypeOld.Text <> "''" Then  '非必填時，回傳空值
                        '載入 工作性質 下拉式選單
                        strSql = " and WorkTypeID in (" + lblSelectWorkTypeOld.Text + ") and CompID = '" + ddlCompIDOld.SelectedItem.Value + "'"
                        Bsp.Utility.WorkType(ddlWorkTypeOld, "WorkTypeID", , strSql)

                        '第一筆為主要工作性質
                        Dim strDefaultValue() As String = lblSelectWorkTypeOld.Text.Replace("'", "").Split(",")
                        Dim strWorkType As String = ""
                        Bsp.Utility.SetSelectedIndex(ddlWorkTypeOld, strDefaultValue(0))
                        For intLoop As Integer = 0 To strDefaultValue.GetUpperBound(0)
                            If intLoop = 0 Then
                                strWorkType = "1|" + strDefaultValue(intLoop)
                            Else
                                strWorkType = strWorkType + "|0|" + strDefaultValue(intLoop)
                            End If
                        Next
                        hidWorkTypeIDOld.Value = strWorkType
                    Else
                        ddlWorkTypeOld.Items.Clear()
                        ddlWorkTypeOld.Items.Insert(0, New ListItem("---請選擇---", ""))
                    End If
                    For i As Integer = 0 To ddlWorkTypeOld.Items.Count - 1
                        If ddlWorkTypeOld.Items(i).Selected Then
                            strRstName1 = ddlWorkTypeOld.Items(i).Text.Trim.Split("-")(1).ToString
                        Else
                            If strRstName2 <> "" Then strRstName2 += ","
                            strRstName2 += ddlWorkTypeOld.Items(i).Text.Trim.Split("-")(1).ToString
                        End If
                    Next
                    If strRstName2 = "" Then
                        lblSelectWorkTypeNameOld.Text = strRstName1
                    Else
                        lblSelectWorkTypeNameOld.Text = strRstName1 + "," + strRstName2
                    End If
            End Select
        End If
    End Sub


    Protected Sub ddlRankIDOld_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlRankIDOld.SelectedIndexChanged
        subLoadTitleIDDataOld()
    End Sub
    Protected Sub ddlRankIDNew_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlRankIDNew.SelectedIndexChanged
        subLoadTitleIDDataNew()
    End Sub
    Private Sub subLoadTitleIDDataOld()
        '職等
        If ddlRankIDOld.SelectedIndex < 0 Then Return
        ddlTitleIDOld.Items.Clear()

        Dim objHR As New HR()
        Try
            Using dt As Data.DataTable = objHR.GetTitleInfo(ddlRankIDOld.SelectedValue, "TitleID, TitleName, TitleID + '-' + TitleName as FullName", "And CompID=" & Bsp.Utility.Quote(ddlCompIDOld.SelectedValue))
                With ddlTitleIDOld
                    .DataSource = dt
                    .DataTextField = "FullName"
                    .DataValueField = "TitleID"
                    .DataBind()
                    .Items.Insert(0, New ListItem("---請選擇---", ""))

                End With
            End Using

        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Bsp.Utility.getInnerException("subLoadTitleIDData：", ex))
            Return
        End Try
    End Sub
    Private Sub subLoadTitleIDDataNew()
        '職等
        If ddlRankIDNew.SelectedIndex < 0 Then Return
        ddlTitleIDNew.Items.Clear()

        Dim objHR As New HR()
        Try
            Using dt As Data.DataTable = objHR.GetTitleInfo(ddlRankIDNew.SelectedValue, "TitleID, TitleName, TitleID + '-' + TitleName as FullName", "And CompID=" & Bsp.Utility.Quote(hidCompID.Value))
                With ddlTitleIDNew
                    .DataSource = dt
                    .DataTextField = "FullName"
                    .DataValueField = "TitleID"
                    .DataBind()
                    .Items.Insert(0, New ListItem("---請選擇---", ""))

                End With
            End Using

        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Bsp.Utility.getInnerException("subLoadTitleIDData：", ex))
            Return
        End Try
    End Sub

#Region "WorkType"    '工作性質
    Public Function GetWorkTypeIDOld(ByVal strWorkTypeID As String) As String
        Dim strWhere As String
        Dim strWhereWorkType As String
        Dim aryValue() As String = strWorkTypeID.Split("|")
        Dim intCnt As Integer
        Dim strMainWorkType As String '主要工作性質
        strMainWorkType = ""

        strWhere = "where CompID = " & Bsp.Utility.Quote(ddlCompIDOld.SelectedValue)
        strWhereWorkType = ""
        For intCnt = 0 To UBound(aryValue) Step 2
            If intCnt = 0 Then
                strWhereWorkType = Bsp.Utility.Quote(aryValue(intCnt + 1).ToString().Trim)
            Else
                strWhereWorkType = strWhereWorkType & "," & Bsp.Utility.Quote(aryValue(intCnt + 1).ToString().Trim)
            End If
            If aryValue(intCnt) = "1" Then
                strMainWorkType = aryValue(intCnt + 1)
            End If
        Next
        If intCnt > 0 Then
            strWhere = strWhere & "And WorkTypeID In (" & strWhereWorkType & ")"
        End If
        lblSelectWorkTypeOld.Text = strWhereWorkType

        Dim objHR3000 As New HR3000

        Try
            Using dt As Data.DataTable = objHR3000.GetWorkTypeID(strWhere).Tables(0)
                With ddlWorkTypeOld
                    .DataSource = dt
                    .DataTextField = "FullWorkTypeName"
                    .DataValueField = "WorkTypeID"
                    .DataBind()
                    .Items.Insert(0, New ListItem("------", ""))
                End With
            End Using

            Return strMainWorkType

        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me.Page, Bsp.Utility.getInnerException("ddlWorkType", ex))
            Return strMainWorkType
        End Try
    End Function
    Public Function GetWorkTypeIDNew(ByVal strWorkTypeID As String) As String
        Dim strWhere As String
        Dim strWhereWorkType As String
        Dim aryValue() As String = strWorkTypeID.Split("|")
        Dim intCnt As Integer
        Dim strMainWorkType As String '主要工作性質
        strMainWorkType = ""

        strWhere = "where CompID = " & Bsp.Utility.Quote(hidCompID.Value)
        strWhereWorkType = ""
        For intCnt = 0 To UBound(aryValue) Step 2
            If intCnt = 0 Then
                strWhereWorkType = Bsp.Utility.Quote(aryValue(intCnt + 1).ToString().Trim)
            Else
                strWhereWorkType = strWhereWorkType & "," & Bsp.Utility.Quote(aryValue(intCnt + 1).ToString().Trim)
            End If
            If aryValue(intCnt) = "1" Then
                strMainWorkType = aryValue(intCnt + 1)
            End If
        Next
        If intCnt > 0 Then
            strWhere = strWhere & "And WorkTypeID In (" & strWhereWorkType & ")"
        End If
        lblSelectWorkTypeNew.Text = strWhereWorkType

        Dim objHR3000 As New HR3000

        Try
            Using dt As Data.DataTable = objHR3000.GetWorkTypeID(strWhere).Tables(0)
                With ddlWorkTypeNew
                    .DataSource = dt
                    .DataTextField = "FullWorkTypeName"
                    .DataValueField = "WorkTypeID"
                    .DataBind()
                    .Items.Insert(0, New ListItem("------", ""))
                End With
            End Using

            Return strMainWorkType

        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me.Page, Bsp.Utility.getInnerException("ddlWorkType", ex))
            Return strMainWorkType
        End Try
    End Function
#End Region
#Region "Position"    '職位
    Public Function GetPositionIDOld(ByVal strPositionID As String) As String
        Dim strWhere As String
        Dim strWherePosition As String
        Dim aryValue() As String = strPositionID.Split("|")
        Dim intCnt As Integer
        Dim strMainPosition As String '主要職位
        strMainPosition = ""

        strWhere = "where CompID = " & Bsp.Utility.Quote(ddlCompIDOld.SelectedValue)
        strWherePosition = ""
        For intCnt = 0 To UBound(aryValue) Step 2
            If intCnt = 0 Then
                strWherePosition = Bsp.Utility.Quote(aryValue(intCnt + 1).ToString().Trim)
            Else
                strWherePosition = strWherePosition & "," & Bsp.Utility.Quote(aryValue(intCnt + 1).ToString().Trim)
            End If
            If aryValue(intCnt) = "1" Then
                strMainPosition = aryValue(intCnt + 1)
            End If
        Next
        If intCnt > 0 Then
            strWhere = strWhere & "And PositionID In (" & strWherePosition & ")"
        End If
        lblSelectPositionOld.Text = strWherePosition

        Dim objHR3000 As New HR3000

        Try
            Using dt As Data.DataTable = objHR3000.GetPositionID(strWhere).Tables(0)
                With ddlPositionOld
                    .DataSource = dt
                    .DataTextField = "FullPositionName"
                    .DataValueField = "PositionID"
                    .DataBind()
                    .Items.Insert(0, New ListItem("------", ""))
                End With
            End Using

            Return strMainPosition

        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me.Page, Bsp.Utility.getInnerException("ddlPosition", ex))
            Return strMainPosition
        End Try
    End Function
    Public Function GetPositionIDNew(ByVal strPositionID As String) As String
        Dim strWhere As String
        Dim strWherePosition As String
        Dim aryValue() As String = strPositionID.Split("|")
        Dim intCnt As Integer
        Dim strMainPosition As String '主要職位
        strMainPosition = ""

        strWhere = "where CompID = " & Bsp.Utility.Quote(hidCompID.Value)
        strWherePosition = ""
        For intCnt = 0 To UBound(aryValue) Step 2
            If intCnt = 0 Then
                strWherePosition = Bsp.Utility.Quote(aryValue(intCnt + 1).ToString().Trim)
            Else
                strWherePosition = strWherePosition & "," & Bsp.Utility.Quote(aryValue(intCnt + 1).ToString().Trim)
            End If
            If aryValue(intCnt) = "1" Then
                strMainPosition = aryValue(intCnt + 1)
            End If
        Next
        If intCnt > 0 Then
            strWhere = strWhere & "And PositionID In (" & strWherePosition & ")"
        End If
        lblSelectPositionNew.Text = strWherePosition

        Dim objHR3000 As New HR3000

        Try
            Using dt As Data.DataTable = objHR3000.GetPositionID(strWhere).Tables(0)
                With ddlPositionNew
                    .DataSource = dt
                    .DataTextField = "FullPositionName"
                    .DataValueField = "PositionID"
                    .DataBind()
                    .Items.Insert(0, New ListItem("------", ""))
                End With
            End Using

            Return strMainPosition

        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me.Page, Bsp.Utility.getInnerException("ddlPosition", ex))
            Return strMainPosition
        End Try
    End Function
#End Region
#Region "最小簽核單位"
    Public Sub GetEmpFlowOrganIDOld()
        Dim objHR3000 As New HR3000
        Dim strWhere As String
        strWhere = "where OrganID = '" & ucSelectHROrganOld.SelectedOrganID & "'"
        Dim intCnt As Integer
        Using dt As Data.DataTable = objHR3000.GetFlowOrganID(ddlCompIDOld.SelectedValue, strWhere).Tables(0)
            With ddlFlowOrganIDOld
                If dt.Rows.Item(0)("FlowOrganID").ToString.Trim = "" Then
                    .DataSource = dt
                    .DataTextField = "FullOrganName"
                    .DataValueField = "OrganID"
                    .DataBind()
                Else
                    Dim aryValue() As String = dt.Rows(0)("FlowOrganID").ToString().Trim.Split("|")
                    For intCnt = 0 To UBound(aryValue)
                        If intCnt = 0 Then
                            strWhere = Bsp.Utility.Quote(aryValue(intCnt).ToString().Trim)
                        Else
                            strWhere = strWhere & "," & Bsp.Utility.Quote(aryValue(intCnt).ToString().Trim)
                        End If
                    Next
                    strWhere = "Where OrganID In (" & strWhere & ")"
                End If
            End With
        End Using
        If intCnt > 0 Then
            Using dt As Data.DataTable = objHR3000.GetFlowOrganID(ddlCompIDOld.SelectedValue, strWhere).Tables(0)
                With ddlFlowOrganIDOld
                    .DataSource = dt
                    .DataTextField = "FullOrganName"
                    .DataValueField = "OrganID"
                    .DataBind()
                End With
            End Using
        End If
        ddlFlowOrganIDOld.Items.Insert(0, New ListItem("---請選擇---", ""))
        UpdFlowOrgnaIDOld.Update()
    End Sub
    Public Sub GetEmpFlowOrganIDNew()
        Dim objHR3000 As New HR3000
        Dim strWhere As String
        strWhere = "where OrganID = '" & ucSelectHROrganNew.SelectedOrganID & "'"
        Dim intCnt As Integer
        Using dt As Data.DataTable = objHR3000.GetFlowOrganID(hidCompID.Value, strWhere).Tables(0)
            With ddlFlowOrganIDNew
                If dt.Rows.Item(0)("FlowOrganID").ToString.Trim = "" Then
                    .DataSource = dt
                    .DataTextField = "FullOrganName"
                    .DataValueField = "OrganID"
                    .DataBind()
                Else
                    Dim aryValue() As String = dt.Rows(0)("FlowOrganID").ToString().Trim.Split("|")
                    For intCnt = 0 To UBound(aryValue)
                        If intCnt = 0 Then
                            strWhere = Bsp.Utility.Quote(aryValue(intCnt).ToString().Trim)
                        Else
                            strWhere = strWhere & "," & Bsp.Utility.Quote(aryValue(intCnt).ToString().Trim)
                        End If
                    Next
                    strWhere = "Where OrganID In (" & strWhere & ")"
                End If
            End With
        End Using
        If intCnt > 0 Then
            Using dt As Data.DataTable = objHR3000.GetFlowOrganID(hidCompID.Value, strWhere).Tables(0)
                With ddlFlowOrganIDNew
                    .DataSource = dt
                    .DataTextField = "FullOrganName"
                    .DataValueField = "OrganID"
                    .DataBind()
                End With
            End Using
        End If
        ddlFlowOrganIDNew.Items.Insert(0, New ListItem("---請選擇---", ""))
        UpdFlowOrgnaIDNew.Update()
    End Sub
#End Region


End Class
