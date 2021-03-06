'****************************************************
'功能說明：員工待異動紀錄(EmployeeLog)維護-新增
'建立人員：Weicheng
'建立日期：2014/08/18
'****************************************************
Imports System.Data
Imports Newtonsoft.Json

Partial Class HR_HR3002
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then
            '公司
            Bsp.Utility.FillHRCompany(ddlNewCompID, Bsp.Enums.FullNameType.CodeDefine)
            '20150713 wei add
            '任職狀況
            ViewState.Item("WorkStatus") = ""

            '異動原因
            HR3000.FillEmployeeReason(ddlReason, False)
            ddlReason.Items.Insert(0, New ListItem("---請選擇---", ""))
            ucSelectAddFlowOrgan.LoadData(ucSelectEmpAdditionHROrgan.SelectedOrganID, "Y")
            'ddlEmpAdditonFlowOrganID.Items.Insert(0, New ListItem("", ""))
            ddlReason.SelectedIndex = -1
            '離職原因
            HR3000.FillEmployeeQuitReason(ddlQuitReason)
            ddlQuitReason.Items.Insert(0, New ListItem("", ""))
            '部門、科組課
            'ucSelectHROrgan.LoadData(ddlNewCompID.SelectedItem.Value)
            ViewState.Item("WorkTypeID") = ""
            ViewState.Item("PositionID") = ""

            '主管任用方式
            ddlBossType.Items.Insert(0, New ListItem("---請選擇---", ""))
            ddlBossType.Items.Insert(1, New ListItem("主要", "1"))
            ddlBossType.Items.Insert(2, New ListItem("兼任", "2"))

            '兼任狀態
            HR3000.FillEmpAdditionReason(ddlEmpAdditionReason)

            '兼任公司
            Bsp.Utility.FillHRCompany(ddlEmpAdditionCompID,Bsp.Enums.FullNameType.CodeDefine)
            '兼任主管任用方式
            ddlEmpAdditionBossType.Items.Insert(0, New ListItem("---請選擇---", ""))
            ddlEmpAdditionBossType.Items.Insert(1, New ListItem("主要", "1"))
            ddlEmpAdditionBossType.Items.Insert(2, New ListItem("兼任", "2"))

            '常用備註 20150311 wei add
            ucSelectRemark.QuerySQL = "Select CodeCName From HRCodeMap Where TabName='EmployeeWait' and FldName='Remark' Order by CodeCName"
            ucSelectRemark.SelectStyle = Bsp.Enums.SelectStyle.CheckBox
            ucSelectRemark.Fields = New FieldState() { _
                    New FieldState("CodeCName", "名稱", True, True)}
        End If
    End Sub
    Protected Overrides Sub BaseOnPageTransfer(ByVal ti As TransferInfo)
        If Not IsPostBack() Then

            Dim ht As Hashtable = Bsp.Utility.getHashTableFromParam(ti.Args)

            ViewState.Item("CompID") = ht("SelectCompID").ToString()
            ViewState.Item("EmpID") = ht("SelectEmpID").ToString()
            ViewState.Item("ValidDate") = ht("SelectValidDate").ToString()
            ViewState.Item("Seq") = ht("SelectSeq").ToString()
            If ht.ContainsKey("Detail") Then
                ViewState.Item("Detail") = ht("Detail").ToString()
            Else
                ViewState.Item("Detail") = "N"
            End If
            If ViewState.Item("Detail") = "Y" Then
                btnAddEmpAddition.Enabled = False
                btnEmpAdditionCancel.Text = "關閉"
            End If
            ViewState.Item("PageNo") = ht("PageNo").ToString()
            hidCompID.Value = ViewState.Item("CompID")
            hidEmpID.Value = ViewState.Item("EmpID")
            hidValidDate.Value = ViewState.Item("ValidDate")
            hidSeq.Value = ViewState.Item("Seq")

            '工作性質
            ddlWorkType.Items.Insert(0, New ListItem("---請選擇---", ""))

            '職位
            ddlPosition.Items.Insert(0, New ListItem("---請選擇---", ""))

            CheckShow_Hide()


            subGetData(hidCompID.Value, hidEmpID.Value, hidValidDate.Value, hidSeq.Value)

            '調兼資料
            ddlEmpAdditionCompID.SelectedValue = ViewState.Item("NewCompID")
            '部門、科組課
            ucSelectEmpAdditionHROrgan.LoadData(ViewState.Item("NewCompID"))
            ucSelectAddFlowOrgan.LoadData(ucSelectEmpAdditionHROrgan.SelectedOrganID, "Y")
            'ddlEmpAdditonFlowOrganID.Items.Insert(0, New ListItem("---請選擇---", ""))

        End If
    End Sub
    Private Sub subGetData(ByVal CompID As String, ByVal EmpID As String, ByVal ValidDate As String, ByVal Seq As String)
        Dim strMainWorkTypeID As String
        Dim strMainPositionID As String

        Dim objSC As New SC
        Dim objHR As New HR
        Dim bsEmployeeWait As New beEmployeeWait.Service()
        Dim beEmployeeWait As New beEmployeeWait.Row()

        beEmployeeWait.CompID.Value = CompID
        beEmployeeWait.EmpID.Value = EmpID
        beEmployeeWait.ValidDate.Value = ValidDate
        beEmployeeWait.Seq.Value = Seq

        Try
            Using dt As DataTable = bsEmployeeWait.QueryByKey(beEmployeeWait).Tables(0)
                If dt.Rows.Count <= 0 Then Exit Sub
                beEmployeeWait = New beEmployeeWait.Row(dt.Rows(0))
                hidEmpID.Value = EmpID
                lblEmpID_O.Text = EmpID
                subGetPersonal(CompID, EmpID)
                ucValidDate.DateText = Format(beEmployeeWait.ValidDate.Value, "yyyy/MM/dd")
                If beEmployeeWait.DueDate.Value <> "1900/01/01" Then
                    ucDueDate.DateText = Format(beEmployeeWait.DueDate.Value, "yyyy/MM/dd")
                End If
                '異動原因
                Bsp.Utility.SetSelectedIndex(ddlReason, beEmployeeWait.Reason.Value)
                ViewState.Item("Reason") = beEmployeeWait.Reason.Value
                subSetControlEnable(ViewState.Item("Reason"))

                If beEmployeeWait.Reason.Value = "02" Or beEmployeeWait.Reason.Value = "03" Or beEmployeeWait.Reason.Value = "05" Or beEmployeeWait.Reason.Value = "06" Or beEmployeeWait.Reason.Value = "07" Or beEmployeeWait.Reason.Value = "33" Or beEmployeeWait.Reason.Value = "34" Or beEmployeeWait.Reason.Value = "50" Or beEmployeeWait.Reason.Value = "60" Or beEmployeeWait.Reason.Value = "70" Then
                    Bsp.Utility.RunClientScript(Me.Page, "show_tr('trBossType');")
                    Bsp.Utility.RunClientScript(Me.Page, "show_tr('trIsBoss');")
                Else
                    Bsp.Utility.RunClientScript(Me.Page, "hide_tr('trBossType');")
                    Bsp.Utility.RunClientScript(Me.Page, "hide_tr('trIsBoss');")
                End If
                If beEmployeeWait.Reason.Value = "12" Or beEmployeeWait.Reason.Value = "18" Or beEmployeeWait.Reason.Value = "19" Then    '20150721 wei modify 19留停延長
                    lblDueDate.Visible = True
                    ucDueDate.Visible = True
                Else
                    lblDueDate.Visible = False
                    ucDueDate.Visible = False
                End If

                If ViewState.Item("Reason") = "20" Or ViewState.Item("Reason") = "21" Or ViewState.Item("Reason") = "22" Or ViewState.Item("Reason") = "70" Then 'ViewState.Item("Reason") = "50"
                    ddlNewCompID.Enabled = True
                Else
                    ddlNewCompID.Enabled = False
                End If

                If ViewState.Item("Reason") = "40" Then
                    lblNotice.Text = "僅處理【職等職稱】/【工作性質】/【職位】"
                Else
                    lblNotice.Text = ""
                End If

                '離職原因
                Bsp.Utility.SetSelectedIndex(ddlQuitReason, beEmployeeWait.QuitReason.Value)
                '公司
                Bsp.Utility.SetSelectedIndex(ddlNewCompID, beEmployeeWait.NewCompID.Value)
                ViewState.Item("NewCompID") = beEmployeeWait.NewCompID.Value

                '部門
                ucSelectHROrgan.LoadData(beEmployeeWait.NewCompID.Value, "Y")
                ucSelectHROrgan.setDeptID(beEmployeeWait.NewCompID.Value, beEmployeeWait.DeptID.Value, "Y")

                '科組課
                ucSelectHROrgan.setOrganID(beEmployeeWait.NewCompID.Value, beEmployeeWait.OrganID.Value, "Y")

                '最小簽核單位
                If beEmployeeWait.FlowOrganID.Value <> "" Then
                    ucSelectFlowOrgan.LoadData(ucSelectHROrgan.SelectedOrganID, "Y")
                    ucSelectFlowOrgan.setOrganID(beEmployeeWait.FlowOrganID.Value, "Y")
                    'GetEmpFlowOrganID()
                    'Bsp.Utility.SetSelectedIndex(ddlFlowOrganID, beEmployeeWait.FlowOrganID.Value)
                End If

                '生效註記
                ViewState.Item("ValidMark") = beEmployeeWait.ValidMark.Value
                '20150714 wei add 畫面增加生效註記欄位
                If ViewState.Item("ValidMark") = "1" Then
                    chkValidMark.Checked = True
                Else
                    chkValidMark.Checked = False
                End If

                '計薪註計
                If beEmployeeWait.SalaryPaid.Value = "1" Then
                    chkSalaryPaid.Checked = True
                End If

                '主管
                Bsp.Utility.SetSelectedIndex(ddlBossType, beEmployeeWait.BossType.Value)
                If beEmployeeWait.IsBoss.Value = "1" Then
                    chkIsBoss.Checked = True
                Else
                    chkIsBoss.Checked = False
                End If
                If beEmployeeWait.IsSecBoss.Value = "1" Then
                    chkIsSecBoss.Checked = True
                Else
                    chkIsSecBoss.Checked = False
                End If
                If beEmployeeWait.IsGroupBoss.Value = "1" Then
                    chkIsGroupBoss.Checked = True
                Else
                    chkIsGroupBoss.Checked = False
                End If
                If beEmployeeWait.IsSecGroupBoss.Value = "1" Then
                    chkIsSecGroupBoss.Checked = True
                Else
                    chkIsSecGroupBoss.Checked = False
                End If

                '職等職稱
                Bsp.Utility.Rank(ddlRankID, ddlNewCompID.SelectedItem.Value)
                ddlRankID.Items.Insert(0, New ListItem("---請選擇---", ""))
                Bsp.Utility.SetSelectedIndex(ddlRankID, beEmployeeWait.RankID.Value)
                subLoadTitleIDData()
                Bsp.Utility.SetSelectedIndex(ddlTitleID, beEmployeeWait.TitleID.Value)

                '工作地點
                Bsp.Utility.WorkSite(ddlWorkSiteID, ddlNewCompID.SelectedItem.Value, "")
                ddlWorkSiteID.Items.Insert(0, New ListItem("---請選擇---", ""))
                Bsp.Utility.SetSelectedIndex(ddlWorkSiteID, beEmployeeWait.WorkSiteID.Value)

                '工作性質
                ViewState.Item("WorkTypeID") = beEmployeeWait.WorkTypeID.Value
                'hidWorkTypeID.Value = ViewState.Item("WorkTypeID")  '20150623 wei del
                strMainWorkTypeID = ""
                If ViewState.Item("WorkTypeID") <> "" Then  'hidWorkTypeID.Value 20150623 wei modify 
                    strMainWorkTypeID = GetWorkTypeID(ViewState.Item("WorkTypeID"))
                    Bsp.Utility.SetSelectedIndex(ddlWorkType, strMainWorkTypeID)
                    ddlWorkType.Items.Remove(ddlWorkType.Items.FindByText("------"))
                End If

                '職位
                ViewState.Item("PositionID") = beEmployeeWait.PositionID.Value
                'hidPositionID.Value = beEmployeeWait.PositionID.Value()    '20150623 wei del
                strMainPositionID = ""
                If ViewState.Item("PositionID") <> "" Then
                    strMainPositionID = GetPositionID(ViewState.Item("PositionID"))
                    Bsp.Utility.SetSelectedIndex(ddlPosition, strMainPositionID)
                    ddlPosition.Items.Remove(ddlPosition.Items.FindByText("------"))
                End If


                '應試者編號
                txtRecID.Text = beEmployeeWait.RecID.Value
                ucContractDate.DateText = beEmployeeWait.ContractDate.Value.ToShortDateString

                '班別
                HR3000.FillEmployeeWTID(ddlWTID, ddlNewCompID.SelectedItem.Value)
                ddlWTID.Items.Insert(0, New ListItem("---請選擇---", ""))
                Bsp.Utility.SetSelectedIndex(ddlWTID, beEmployeeWait.WTID.Value)

                txtRemark.Text = beEmployeeWait.Remark.Value

                If beEmployeeWait.ExistsEmployeeLog.Value = "1" Then
                    btnEmployeeLogWait.Enabled = True
                Else
                    btnEmployeeLogWait.Enabled = False
                End If

                '最後異動公司
                Using dt1 As DataTable = objHR.GetHRCompName(beEmployeeWait.LastChgComp.Value)
                    'lblLastChgCompS.Text = beEmployeeWait.LastChgComp.Value & " " & objHR.GetHRCompName(beEmployeeWait.LastChgComp.Value).Rows(0).Item("CompName").ToString()
                    If dt1.Rows.Count = 0 Then
                        lblLastChgCompS.Text = beEmployeeWait.LastChgComp.Value
                    Else
                        lblLastChgCompS.Text = beEmployeeWait.LastChgComp.Value & "-" & dt1.Rows(0).Item("CompName").ToString()
                    End If
                End Using


                '最後異動者
                Using dt1 As DataTable = objHR.GetHREmpName(beEmployeeWait.LastChgComp.Value, beEmployeeWait.LastChgID.Value)
                    'lblLastChgIDS.Text = beEmployeeWait.LastChgID.Value & "-" & objHR.GetHREmpName(beEmployeeWait.LastChgComp.Value, beEmployeeWait.LastChgID.Value).Rows(0).Item("NameN").ToString
                    If dt1.Rows.Count = 0 Then
                        lblLastChgIDS.Text = beEmployeeWait.LastChgID.Value
                    Else
                        lblLastChgIDS.Text = beEmployeeWait.LastChgID.Value & "-" & dt1.Rows(0).Item("NameN").ToString()
                    End If
                End Using


                '最後異動時間
                lblLastChgDateS.Text = beEmployeeWait.LastChgDate.Value.ToString("yyyy/MM/dd HH:mm:ss")

                '兼任資料
                EmpAddition_CopyTo_Tmp_EmpAdditionWait()
                GetEmpAdditionWait()

            End Using
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & ".GetData", ex)
        End Try
    End Sub
    Private Sub subGetPersonal(ByVal CompID As String, ByVal EmpID As String)
        Dim objHR3000 As New HR3000
        '個人資料
        Using dt As DataTable = objHR3000.GetEmpDataByHR3000(CompID, EmpID)
            If dt.Rows.Count > 0 Then

                hidIDNo.Value = dt.Rows.Item(0)("IDNo").ToString()
                lblUserName.Text = dt.Rows.Item(0)("Name").ToString()
                hidEmpDate.Value = dt.Rows(0)("EmpDate").ToString()
                ViewState.Item("WorkStatus") = dt.Rows.Item(0)("WorkStatus").ToString()  '20150713 wei 任職狀況
            End If
        End Using
    End Sub
    Public Overrides Sub DoAction(ByVal Param As String)
        Select Case Param
            Case "btnAdd"   '存檔返回
                If ViewState.Item("ValidMark") = "1" Then
                    Bsp.Utility.RunClientScript(Me.Page, "IsTOConfirm('Execute');")
                    'Release("btnExecute")
                Else
                    If funCheckData() Then
                        If SaveData() Then
                            If funCheckEmployeeLog(ucValidDate.DateText, hidCompID.Value, lblEmpID_O.Text.Trim) Then  '檢查是否有已生效企業團經歷
                                Bsp.Utility.RunClientScript(Me.Page, "IsTOConfirm('EmployeeLogWait');")
                                'If MsgBox("此員工生效日後，已有『已生效』的企業團經歷存在，請確認是否要調整資料？", MsgBoxStyle.OkCancel, "") = MsgBoxResult.Ok Then  '導向已生效企業團經歷待異動畫面
                                '    GoEmployeeLogWait()
                                'Else
                                '    GoBack()
                                'End If
                            Else
                                If funCheckEmployeeWait(ucValidDate.DateText, hidCompID.Value, lblEmpID_O.Text.Trim, hidSeq.Value) Then '檢查是否有未生效待異動紀錄
                                    Bsp.Utility.RunClientScript(Me.Page, "IsTOConfirm('BackQuery');")
                                    'If MsgBox("此員工生效日後，已有『未生效』的待異動資料存在，請確認是否要調整資料？", MsgBoxStyle.OkCancel, "") = MsgBoxResult.Ok Then  '導回查詢清單頁，並帶入查詢條件-公司代碼，員工編號，狀態-未生效
                                    '    GoBackQuery()
                                    'Else
                                    '    GoBack()
                                    'End If
                                Else
                                    GoBack()
                                End If
                            End If

                        End If
                    End If
                End If
            Case "btnActionX"   '返回
                'ucSelectPosition_Click()
                GoBack()
            Case "btnCancel"    '清除
                ClearData()
        End Select
    End Sub
    Private Sub Release(ByVal LogFunction As String)
        ucRelease.ShowCompRole = "True"
        ucRelease.FunID = "HR3000"
        ucRelease.LogFunction = LogFunction
        ucRelease.OpenSelect()
    End Sub
    '隱藏的按鈕--檢核主管註記，跳出提示視窗，確認後繼續儲存
    Protected Sub btnCheckSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCheckSave.Click
        If SaveData() Then
            If funCheckEmployeeLog(ucValidDate.DateText, hidCompID.Value, hidEmpID.Value) Then  '檢查是否有已生效企業團經歷  
                Bsp.Utility.RunClientScript(Me.Page, "IsTOConfirm('EmployeeLogWait');")
                'If MsgBox("此員工生效日後，已有『已生效』的企業團經歷存在，請確認是否要調整資料？", MsgBoxStyle.OkCancel, "") = MsgBoxResult.Ok Then  '導向已生效企業團經歷待異動畫面
                '    GoEmployeeLogWait()
                'Else
                '    GoBack()
                'End If
            Else
                If funCheckEmployeeWait(ucValidDate.DateText, hidCompID.Value, hidEmpID.Value, hidSeq.Value) Then '檢查是否有未生效待異動紀錄 
                    Bsp.Utility.RunClientScript(Me.Page, "IsTOConfirm('BackQuery');")
                    'If MsgBox("此員工生效日後，已有『未生效』的待異動資料存在，請確認是否要調整資料？", MsgBoxStyle.OkCancel, "") = MsgBoxResult.Ok Then  '導回查詢清單頁，並帶入查詢條件-公司代碼，員工編號，狀態-未生效
                    '    GoBackQuery()
                    'Else
                    '    GoBack()
                    'End If
                Else
                    GoBack()
                End If
            End If
        End If
    End Sub
    Private Sub GoBack()
        Dim ti As TransferInfo = Me.StateTransfer
        Me.TransferFramePage("~/HR3/HR3000.aspx", Nothing, ti.Args)

        'Dim ti As TransferInfo = Me.StateTransfer
        'Me.TransferFramePage("~/HR3/HR3000.aspx", Nothing, _
        '                     "chkValidOrNot=True", _
        '                     "ddlValidOrNot=0", _
        '                     "chkEmpID=True", _
        '                     "txtEmpID=" & lblEmpID_O.Text.Trim, _
        '                     "SelectCompID=" & ViewState.Item("CompID"), _
        '                     "PageNo=1", _
        '                     "DoQuery=Y")
    End Sub
    Private Sub GoBackQuery()
        Dim ti As TransferInfo = Me.StateTransfer
        Me.TransferFramePage("~/HR3/HR3000.aspx", Nothing, _
                             "chkValidOrNot=True", _
                             "ddlValidOrNot=0", _
                             "chkEmpID=True", _
                             "txtEmpID=" & lblEmpID_O.Text.Trim, _
                             "SelectCompID=" & ViewState.Item("CompID"), _
                             "PageNo=1", _
                             "DoQuery=Y")
    End Sub
    Private Sub GoEmployeeLogWait() '導向已生效企業團經歷待異動
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
            '                     "SelectEmpID=" & lblEmpID_O.Text.Trim, _
            '                     "SelectName=" & lblUserName.Text.Trim, _
            '                     "SelectValidDate=" & ucValidDate.DateText, _
            '                     "SelectSeq=" & hidSeq.Value, _
            '                     "PageNo=" & ViewState.Item("PageNo"), _
            '                     "DoDetail=Y", _
            '                     "DoQuery=Y")
        Else
            btnA.Caption = "輸入調整資料"
            btnD.Caption = "刪除"
            btnX.Caption = "返回"

            Me.TransferFramePage("~/HR3/HR3010.aspx", New ButtonState() {btnA, btnD, btnX}, ti.Args)
            'Me.TransferFramePage("~/HR3/HR3010.aspx", New ButtonState() {btnA, btnD, btnX}, _
            '                     "SelectCompRoleID=" & hidCompID.Value, _
            '                     "SelectCompID=" & hidCompID.Value, _
            '                     "SelectEmpID=" & lblEmpID_O.Text.Trim, _
            '                     "SelectName=" & lblUserName.Text.Trim, _
            '                     "SelectValidDate=" & ucValidDate.DateText, _
            '                     "SelectSeq=" & hidSeq.Value, _
            '                     "PageNo=" & ViewState.Item("PageNo"), _
            '                     "Detail=N", _
            '                     "DoQuery=Y")
        End If

    End Sub
    Private Function SaveData() As Boolean
        Dim strWhere As String = ""
        Dim beEmployeeWaitOld As New beEmployeeWait.Row()
        Dim beEmployeeWait As New beEmployeeWait.Row()
        Dim beEmployeeWaitDel As New beEmployeeWait.Row()
        Dim bsEmployeeWait As New beEmployeeWait.Service()
        Dim objHR As New HR
        Dim objHR3000 As New HR3000

        Dim intEmployeeWaitSeq As Integer = 0
        Dim strGroupID As String = ""
        Dim Seq As Integer



        beEmployeeWaitDel.CompID.Value = ViewState.Item("CompID")
        beEmployeeWaitDel.EmpID.Value = ViewState.Item("EmpID")
        beEmployeeWaitDel.ValidDate.Value = ViewState.Item("ValidDate")
        beEmployeeWaitDel.Seq.Value = ViewState.Item("Seq")

        ''取得輸入資料
        beEmployeeWait.CompID.Value = hidCompID.Value
        beEmployeeWait.EmpID.Value = lblEmpID_O.Text.ToUpper()
        beEmployeeWait.ValidDate.Value = Convert.ToDateTime(ucValidDate.DateText)
        If ucValidDate.DateText = ViewState.Item("ValidDate") Then
            Seq = hidSeq.Value
        Else
            Seq = objHR3000.GetEmployeeWaitSeq(beEmployeeWait.CompID.Value, beEmployeeWait.EmpID.Value, ucValidDate.DateText)
            hidSeq.Value = Seq
        End If
        beEmployeeWait.Seq.Value = Seq
        beEmployeeWait.Reason.Value = ddlReason.SelectedValue
        beEmployeeWait.NewCompID.Value = ddlNewCompID.SelectedValue

        beEmployeeWait.DeptID.Value = ucSelectHROrgan.SelectedDeptID
        If ucSelectHROrgan.SelectedOrganID = "" Then
            beEmployeeWait.OrganID.Value = ucSelectHROrgan.SelectedDeptID
            beEmployeeWait.GroupID.Value = ucSelectHROrgan.SelectedDeptID
        Else
            beEmployeeWait.OrganID.Value = ucSelectHROrgan.SelectedOrganID

            '2016/05/03 SPHBKC資料已併入OrganizationFlow中
            'If ddlNewCompID.SelectedValue = "SPHBKC" Then
            '    strGroupID = objHR3000.Get_CGroupID(beEmployeeWait.OrganID.Value)
            'Else
            If ucSelectHROrgan.SelectedOrganName.ToString().EndsWith("(未生效)") Then
                strGroupID = ViewState.Item("GroupID")
            Else
                strGroupID = objHR3000.Get_GroupID(beEmployeeWait.OrganID.Value)
                'End If
            End If
            beEmployeeWait.GroupID.Value = strGroupID
        End If

        beEmployeeWait.RankID.Value = ddlRankID.SelectedValue
        beEmployeeWait.TitleID.Value = ddlTitleID.SelectedValue
        beEmployeeWait.TitleName.Value = ddlTitleID.SelectedItem.Text.Split("-")(1).ToString
        beEmployeeWait.WorkTypeID.Value = ViewState.Item("WorkTypeID") 'hidWorkTypeID.Value 20150623 wei modify
        beEmployeeWait.PositionID.Value = ViewState.Item("PositionID")  'hidPositionID.Value 20150623 wei modify
        '20150316 wei modify
        If ddlReason.SelectedValue = "02" Or ddlReason.SelectedValue = "03" Or ddlReason.SelectedValue = "05" Or ddlReason.SelectedValue = "06" Or ddlReason.SelectedValue = "07" Or ddlReason.SelectedValue = "33" Or ddlReason.SelectedValue = "34" Or ddlReason.SelectedValue = "50" Or ddlReason.SelectedValue = "60" Or ddlReason.SelectedValue = "70" Then
            beEmployeeWait.BossType.Value = ddlBossType.SelectedValue
            beEmployeeWait.IsBoss.Value = IIf(chkIsBoss.Checked, "1", "0")
            beEmployeeWait.IsSecBoss.Value = IIf(chkIsSecBoss.Checked, "1", "0")
            beEmployeeWait.IsGroupBoss.Value = IIf(chkIsGroupBoss.Checked, "1", "0")
            beEmployeeWait.IsSecGroupBoss.Value = IIf(chkIsSecGroupBoss.Checked, "1", "0")
        Else
            beEmployeeWait.BossType.Value = ""
            beEmployeeWait.IsBoss.Value = "0"
            beEmployeeWait.IsSecBoss.Value = "0"
            beEmployeeWait.IsGroupBoss.Value = "0"
            beEmployeeWait.IsSecGroupBoss.Value = "0"
        End If

        beEmployeeWait.Remark.Value = txtRemark.Text.Trim()
        Using dt As DataTable = bsEmployeeWait.QueryByKey(beEmployeeWaitDel).Tables(0)
            beEmployeeWaitOld = New beEmployeeWait.Row(dt.Rows(0))
            beEmployeeWait.ValidMark.Value = beEmployeeWaitOld.ValidMark.Value
        End Using

        beEmployeeWait.FlowOrganID.Value = ucSelectFlowOrgan.SelectedOrganID
        beEmployeeWait.WorkSiteID.Value = ddlWorkSiteID.SelectedValue
        If ucDueDate.DateText = "" Then
            beEmployeeWait.DueDate.Value = Convert.ToDateTime("1900/01/01")
        Else
            beEmployeeWait.DueDate.Value = Convert.ToDateTime(ucDueDate.DateText)
        End If
        beEmployeeWait.QuitReason.Value = ddlQuitReason.SelectedValue.Trim
        beEmployeeWait.WTID.Value = ddlWTID.SelectedValue.Trim
        beEmployeeWait.RecID.Value = txtRecID.Text.Trim
        If ucContractDate.DateText <> "" Then
            beEmployeeWait.ContractDate.Value = Convert.ToDateTime(ucContractDate.DateText)
        Else
            beEmployeeWait.ContractDate.Value = Convert.ToDateTime("1900/01/01")
        End If
        '檢查是否有在生效日之後的企業團經歷
        'strWhere = " And CompID <> " & Bsp.Utility.Quote(hidCompID.Value) & " And EmpID = " & Bsp.Utility.Quote(lblEmpID_O.Text.ToUpper())
        'strWhere &= " And ModifyDate >= " & Bsp.Utility.Quote(ucValidDate.DateText)
        'If objHR.IsDataExists("EmployeeLog", strWhere) Then
        '    beEmployeeWait.ExistsEmployeeLog.Value = "1"
        'End If
        If funCheckEmployeeLog(ucValidDate.DateText, hidCompID.Value, lblEmpID_O.Text.Trim) Then  '檢查是否有已生效企業團經歷
            beEmployeeWait.ExistsEmployeeLog.Value = "1"
        End If
        beEmployeeWait.LastChgComp.Value = UserProfile.ActCompID
        beEmployeeWait.LastChgID.Value = UserProfile.ActUserID
        beEmployeeWait.LastChgDate.Value = Now

        '檢查資料是否存在
        If Not bsEmployeeWait.IsDataExists(beEmployeeWaitDel) Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00020", "")
            Return False
        End If
        '儲存資料
        Try
            Return objHR3000.UpdateEmployeeWait(beEmployeeWaitDel, beEmployeeWait, hdfEmpAdditionSeqNo.Value)

        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, "[CC0201_99]" & Bsp.Utility.getMessage("E_00000") & Bsp.Utility.getInnerException(Me.FunID, ex))
            Return False
        End Try
    End Function


#Region "CheckData:EmployeeWait"
    Private Function funCheckData() As Boolean
        Dim objHR As New HR
        Dim objHR3000 As New HR3000

        Dim strValue As String
        Dim strReason As String = ""

        Dim bolBoss As Boolean = False '20150601 wei add 判斷工作性質是否符合BA0001單位主管，BSN021區督導，BSN000分行經理
        Dim bolOPBoss As Boolean = False    '20150601 wei add 判斷工作性質是否符合 BO0001作業主管

        Dim strWhere As String = ""

        '異動原因
        strReason = ddlReason.SelectedValue
        If strReason = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblReason.Text)
            ddlReason.Focus()
            Return False
        Else
            If strReason <> ViewState.Item("Reason") Then
                If Not objHR3000.ChkEmployeeReasonIsVlaid(strReason) Then
                    Bsp.Utility.ShowMessage(Me, "異動原因：所選擇異動原因已無效！")
                    Return False
                End If
            End If
        End If

        '20150713 wei add 增加任職狀況vs異動原因的檢核
        strWhere = " And BeforeWorkStatus = " & Bsp.Utility.Quote(ViewState.Item("WorkStatus")) & " And Reason = " & Bsp.Utility.Quote(strReason)
        If Not objHR.IsDataExists("WorkStatus_EmployeeReason", strWhere) Then
            Bsp.Utility.ShowMessage(Me, "人員任職狀態與輸入異動原因不符，請重新輸入！")
            ddlReason.Focus()
            Return False
        End If

        '20150528 wei add 02,03,07跨公司復職，要檢核非在職(<>1)
        If strReason = "02" Or strReason = "03" Or strReason = "07" Then
            strWhere = " And CompID = " & Bsp.Utility.Quote(hidCompID.Value) & " And EmpID = " & Bsp.Utility.Quote(hidEmpID.Value)
            strWhere &= " And WorkStatus<>'1'"
            If Not objHR.IsDataExists("Personal", strWhere) Then
                Bsp.Utility.ShowMessage(Me, "異動原因：輸入錯誤，人員需為不在職")
                ddlReason.Focus()
                Return False
            End If
        End If
        '20150528 wei add	70跨公司調動，要檢核異動前公司為在職(=1)，防呆輸入07
        '20150528 wei add	11，要檢核在職(=1)
        If strReason = "11" Or strReason = "70" Then
            strWhere = " And CompID = " & Bsp.Utility.Quote(hidCompID.Value) & " And EmpID = " & Bsp.Utility.Quote(hidEmpID.Value)
            strWhere &= " And WorkStatus='1'"
            If Not objHR.IsDataExists("Personal", strWhere) Then
                Bsp.Utility.ShowMessage(Me, "異動原因：輸入錯誤，人員需為在職")
                ddlReason.Focus()
                Return False
            End If
        End If
        '20150528 wei add	12留停會延長,13，要檢核在職或留停(=1,2)
        If strReason = "12" Or strReason = " 13" Or strReason = " 19" Then   '20150721 wei modify 19留停延長
            strWhere = " And CompID = " & Bsp.Utility.Quote(hidCompID.Value) & " And EmpID = " & Bsp.Utility.Quote(hidEmpID.Value)
            strWhere &= " And WorkStatus in ('1','2')"
            If Not objHR.IsDataExists("Personal", strWhere) Then
                Bsp.Utility.ShowMessage(Me, "異動原因：輸入錯誤，人員需為在職或留停")
                ddlReason.Focus()
                Return False
            End If
        End If

        '生效日
        strValue = ucValidDate.DateText
        If strValue = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblModifyDate.Text)
            ucValidDate.Focus()
            Return False
        Else
            If Not IsDate(strValue) Then
                Bsp.Utility.ShowFormatMessage(Me, "W_00070", lblModifyDate.Text)
                ucValidDate.Focus()
                Return False
            Else
                If CDate(strValue) < CDate(hidEmpDate.Value) Then
                    Bsp.Utility.ShowMessage(Me, "生效日期：輸入錯誤，必須大於到職日")
                    ucValidDate.Focus()
                    Return False
                End If
            End If
        End If
        If strReason = "12" Or strReason = "18" Or strReason = "19" Then    '20150721 wei modify 19留停延長
            strValue = ucDueDate.DateText
            If strValue = "" Then
                Bsp.Utility.ShowFormatMessage(Me, "W_00030", "迄日")
                ucDueDate.Focus()
                Return False
            Else
                If Not IsDate(strValue) Then
                    Bsp.Utility.ShowFormatMessage(Me, "W_00070", "迄日")
                    ucValidDate.Focus()
                    Return False
                Else
                    If CDate(strValue) < CDate(ucValidDate.DateText) Then
                        Bsp.Utility.ShowMessage(Me, "迄日：輸入錯誤，必須大於生效日期")
                        ucValidDate.Focus()
                        Return False
                    End If
                End If
            End If
        End If

        '離職原因
        If strReason = "11" Then
            If ddlQuitReason.SelectedValue = "" Then
                Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblQuitReason.Text)
                ddlQuitReason.Focus()
                Return False
            End If
        End If

        '異動後公司
        If ddlNewCompID.SelectedValue = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblNewCompID.Text)
            ddlNewCompID.Focus()
            Return False
        End If
        '20150529 wei add 70跨公司調動，增加檢核異動前後公司需不相同
        If strReason = "70" Then
            If ddlNewCompID.SelectedValue = hidCompID.Value Then
                Bsp.Utility.ShowMessage(Me, "異動後公司：輸入錯誤，異動原因為70時，前後公司不得相同！")
                ddlNewCompID.Focus()
                Return False
            End If
        Else    '20150806 wei 除70跨公司調動外，其餘異動前後公司需相同
            If ddlNewCompID.SelectedValue <> hidCompID.Value Then
                Bsp.Utility.ShowMessage(Me, "異動後公司：輸入錯誤，異動原因不為70時，前後公司需相同！")
                ddlNewCompID.Focus()
                Return False
            End If
        End If

        '職等職稱
        '20150529 wei modify 異動後非在職原因(11,12,13,14,15,16,17,18)改非必填，除70跨公司調動
        If objHR3000.GetAfterWorkStatusTypeByWorkStatus_EmployeeReason(ViewState.Item("WorkStatus"), strReason) = "1" Or objHR3000.GetAfterWorkStatusTypeByWorkStatus_EmployeeReason(ViewState.Item("WorkStatus"), strReason) = "7" Then '20150731 wei modify
            'If Not (strReason = "11" Or strReason = "12" Or strReason = "13" Or strReason = "14" Or strReason = "15" Or strReason = "16" Or strReason = "17" Or strReason = "18" Or strReason = "19") Then  '20150721 wei modify 19留停延長    20150731 wei del
            If ddlRankID.SelectedValue = "" And (ddlNewCompID.SelectedValue.Trim <> hidCompID.Value) Then
                Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblRankID.Text)
                ddlRankID.Focus()
                Return False
            End If
            If ddlTitleID.SelectedValue = "" And (ddlNewCompID.SelectedValue.Trim <> hidCompID.Value) Then
                Bsp.Utility.ShowFormatMessage(Me, "W_00030", "職稱")
                ddlTitleID.Focus()
                Return False
            End If
        End If

        'T類人員(EmpType = 3)只需輸入生效日期、異動原因、公司名稱
        strValue = objHR3000.GetEmpType(hidCompID.Value, lblEmpID_O.Text.Trim)
        If Not strValue = "3" Then
            '20外派、21外調、22外派轉任或16外派不回任不需輸入
            'If Not (strReason = "20" Or strReason = "21" Or strReason = "22" Or strReason = "16" Or strReason = "14" Or strReason = "50") Then 20150529 wei del
            '20150529 wei add---------------------------------------
            '1-1.系統檢核部門、科組課都必填 (HR已知異動後單位)
            '1-2.若異動後任職狀況=2,3,6的單位可輸入無效單位EX:留停延長 (11,12,13,14,15,16,17,18)
            '1-4.33內部輪調-要同部門，50部門調動-要不同部門
            '1-5.33,50不可選其他公司(現況已有)
            '20150529 wei add---------------------------------------

            '2016/05/03 SPHBKC資料已併入OrganizationFlow中
            'If ddlNewCompID.SelectedValue = "SPHBKC" Then
            '    If objHR3000.Get_CGroupID(ucSelectHROrgan.SelectedOrganID) = "" Then
            '        Bsp.Utility.ShowFormatMessage(Me, "W_00030", "事業群")
            '        ucSelectHROrgan.Focus()
            '        Return False
            '    End If
            'Else
            If objHR3000.Get_GroupID(ucSelectHROrgan.SelectedOrganID) = "" Then
                Bsp.Utility.ShowFormatMessage(Me, "W_00030", "事業群")
                ucSelectHROrgan.Focus()
                Return False
            End If
            'End If

            If ucSelectHROrgan.SelectedDeptID = "" Then
                Bsp.Utility.ShowFormatMessage(Me, "W_00030", "部門")
                Return False
            Else
                If Not (strReason = "11" Or strReason = "12" Or strReason = "13" Or strReason = "14" Or strReason = "15" Or strReason = "16" Or strReason = "17" Or strReason = "18" Or strReason = "19") Then  '20150529 wei add   '20150721 wei modify 19留停延長
                    If Not objHR.ChkOrganIsVlaid(ddlNewCompID.SelectedValue, ucSelectHROrgan.SelectedDeptID) Then
                        Bsp.Utility.ShowMessage(Me, "部門：所選擇的部門是無效單位！")
                        Return False
                    End If
                End If
                '20150529 wei add
                If strReason = "33" Then
                    If ucSelectHROrgan.SelectedDeptID <> objHR.GetHREmpDeptID(hidCompID.Value, hidEmpID.Value).Rows(0)("DeptID").ToString Then
                        Bsp.Utility.ShowMessage(Me, "部門：輸入錯誤，異動原因為33時，前後部門需相同！")
                        Return False
                    End If
                End If
                '20150529 wei add
                If strReason = "50" Then
                    If ucSelectHROrgan.SelectedDeptID = objHR.GetHREmpDeptID(hidCompID.Value, hidEmpID.Value).Rows(0)("DeptID").ToString Then
                        Bsp.Utility.ShowMessage(Me, "部門：輸入錯誤，異動原因為50時，前後部門不得相同！")
                        Return False
                    End If
                End If
            End If

            If ucSelectHROrgan.SelectedOrganID = "" Then
                Bsp.Utility.ShowFormatMessage(Me, "W_00030", "科/組/課")
                Return False
            Else
                If Not (strReason = "11" Or strReason = "12" Or strReason = "13" Or strReason = "14" Or strReason = "15" Or strReason = "16" Or strReason = "17" Or strReason = "18" Or strReason = "19") Then  '20150529 wei add   '20150721 wei modify 19留停延長
                    If Not objHR.ChkOrganIsVlaid(ddlNewCompID.SelectedValue, ucSelectHROrgan.SelectedOrganID) Then
                        Bsp.Utility.ShowMessage(Me, "科/組/課：所選擇科/組/課的是無效單位！")
                        Return False
                    End If
                End If
            End If

            '以下異動原因必需選取[最小簽核單位]
            '1-1.系統檢核最小簽核單位都必填
            '1-2.若異動後任職狀況=2,3,6的單位可輸入無效單位EX:留停延長
            'If strReason = "02" Or strReason = "03" Or strReason = "05" Or strReason = "06" Or strReason = "33" Or strReason = "34" Or strReason = "35" Or strReason = "50" Then   20150601 wei del
            If ucSelectFlowOrgan.SelectedOrganID = "" Then
                Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblFlowOrganID.Text)
                ucSelectFlowOrgan.Focus()
                Return False
            Else
                If Not (strReason = "11" Or strReason = "12" Or strReason = "13" Or strReason = "14" Or strReason = "15" Or strReason = "16" Or strReason = "17" Or strReason = "18" Or strReason = "19") Then  '20150601 wei add   '20150721 wei modify 19留停延長
                    If Not objHR.ChkOrganFlowIsVlaid(ddlNewCompID.SelectedValue, ucSelectFlowOrgan.SelectedOrganID) Then
                        Bsp.Utility.ShowMessage(Me, "最小簽核單位：所選擇的最小簽核部門是無效單位！")
                        ucSelectFlowOrgan.Focus()
                        Return False
                    End If
                End If
            End If
            'End If

            '正式員工必須選擇工作性質，'11離職,12留停,13退休,18育嬰留停時不需輸入工作性質
            '20150529 wei add-------------------------------------------------------
            '1-1.系統檢核工作性質都必填 (HR已知異動後單位) ，除70跨公司調動
            '1-2.若異動後任職狀況=2,3,6的工作性質可輸入無效工作性質EX:留停延長
            '20150529 wei add-------------------------------------------------------
            'If strValue = "1" And strReason <> "70" Then '20150529 wei modify And strReason <> "12" And strReason <> "13" And strReason <> "18"
            '高階主管和一般員工必須選擇工作性質
            If Len(lblSelectWorkType.Text.Trim) = 0 And strReason <> "70" Then 'And (ddlNewCompID.SelectedValue.Trim <> hidCompID.Value) 20150623 wei modify
                Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblWorkTypeID.Text)
                ddlWorkType.Focus()
                Return False
            Else
                If ddlWorkType.Items.Count > 6 Then '工作性質不可超過10個   20150529 wei modify  20150814 wei modify 10個配合南京子行整併上線時調整
                    Bsp.Utility.ShowFormatMessage(Me, "工作性質不可超過6個")
                    ddlWorkType.Focus()
                    Return False
                End If
                If Not (strReason = "11" Or strReason = "12" Or strReason = "14" Or strReason = "15" Or strReason = "16" Or strReason = "17" Or strReason = "18" Or strReason = "19") Then  '20150529 wei add   '20150721 wei modify 19留停延長
                    For Each listItmValue As ListItem In ddlWorkType.Items
                        If Not objHR.ChkWorkTypeIsVlaid(ddlNewCompID.SelectedValue.Trim, listItmValue.Value) And listItmValue.Value <> "" Then
                            Bsp.Utility.ShowMessage(Me, "工作性質：所選工作性質中有無效工作性質！")
                            ddlWorkType.Focus()
                            Return False
                        End If
                        ''20150601 wei add 判斷工作性質是否符合BA0001單位主管，BSN021區督導，BSN000分行經理
                        If listItmValue.Value = "BA0001" Or listItmValue.Value = "BSN021" Or listItmValue.Value = "BSN000" Then
                            bolBoss = True
                        End If
                        If listItmValue.Value = "BO0001" Then
                            bolOPBoss = True
                        End If
                    Next
                End If
            End If
            '高階主管和一般員工且異動後公司有導入惠悅必須選擇職位
            If objHR.IsRankIDMapFlag(ddlNewCompID.SelectedValue) Then
                If Len(lblSelectPosition.Text.Trim) = 0 And strReason <> "70" Then 'And (ddlNewCompID.SelectedValue.Trim <> hidCompID.Value) 20150623 wei modify
                    Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblPositionID.Text)
                    ddlPosition.Focus()
                    Return False
                Else
                    If ddlPosition.Items.Count > 6 Then '職位不可超過10個 20150529 wei modify  20150814 wei modify 10個配合南京子行整併上線時調整
                        Bsp.Utility.ShowFormatMessage(Me, "職位不可超過6個")
                        ddlPosition.Focus()
                        Return False
                    End If
                    If Not (strReason = "11" Or strReason = "12" Or strReason = "14" Or strReason = "15" Or strReason = "16" Or strReason = "17" Or strReason = "18" Or strReason = "19") Then  '20150529 wei add   '20150721 wei modify 19留停延長
                        For Each listItmValue As ListItem In ddlPosition.Items
                            If Not objHR.ChkPositionIsVlaid(ddlNewCompID.SelectedValue.Trim, listItmValue.Value) And listItmValue.Value <> "" Then
                                Bsp.Utility.ShowMessage(Me, "職位：所選職位中有無效職位！")
                                ddlPosition.Focus()
                                Return False
                            End If
                        Next
                    End If
                End If
            End If
            'End If
            'End If 20150529 wei del
        End If

        '主管
        If ddlBossType.SelectedValue <> "" Then
            If Not chkIsBoss.Checked And Not chkIsGroupBoss.Checked Then  '20150601 wei modify And Not chkIsSecBoss.Checked And Not chkIsSecGroupBoss.Checked
                Bsp.Utility.ShowFormatMessage(Me, "H_00000", "已選擇主管任用方式，請至少選擇一個單位主管或簽核單位主管")
                chkIsBoss.Focus()
                Return False
            End If
            '檢查是否有同單位的主管註記
            If chkIsBoss.Checked Then
                If hidCompID.Value = ddlNewCompID.SelectedValue And objHR3000.GetAfterWorkStatusTypeByWorkStatus_EmployeeReason(ViewState.Item("WorkStatus"), strReason) = "1" Then '20160113 wei modify 增加判斷若異動後在職才檢查
                    strWhere = " And CompID = " & Bsp.Utility.Quote(ddlNewCompID.SelectedValue) & " And OrganID = " & Bsp.Utility.Quote(ucSelectHROrgan.SelectedOrganID) & " And EmpID<>" & Bsp.Utility.Quote(hidEmpID.Value)
                    strWhere &= " And ValidMark='0' And IsBoss='1' And CompID=NewCompID"
                    strWhere &= " And ValidDate = " & Bsp.Utility.Quote(ucValidDate.DateText)   '20160104 wei add 增加生效日判斷
                    strWhere &= " And Reason in (Select Reason From WorkStatus_EmployeeReason where AfterWorkStatusType in ('1')) " '20160104 wei add 增加判斷異動後需為在職的原因
                    If objHR.IsDataExists("EmployeeWait", strWhere) Then
                        Bsp.Utility.ShowMessage(Me, "待生效資料已有相同單位的單位主管註記！")
                        chkIsBoss.Focus()
                        Return False
                    End If
                    strWhere = " And AddCompID = " & Bsp.Utility.Quote(ddlNewCompID.SelectedValue) & " And AddOrganID = " & Bsp.Utility.Quote(ucSelectHROrgan.SelectedOrganID) & " And EmpID<>" & Bsp.Utility.Quote(hidEmpID.Value)
                    strWhere &= " And ValidMark='0' And IsBoss='1' "
                    strWhere &= " And ValidDate = " & Bsp.Utility.Quote(ucValidDate.DateText)   '20160104 wei add 增加生效日判斷
                    strWhere &= " And Reason in ('1') " '20160104 wei add 增加判斷異動後為調兼的原因
                    If objHR.IsDataExists("EmpAdditionWait", strWhere) Then
                        Bsp.Utility.ShowMessage(Me, "待生效資料已有相同單位的單位主管註記！")
                        chkIsBoss.Focus()
                        Return False
                    End If
                End If
            Else
                '20150601 wei add 檢核若異動後工作性質為BA0001單位主管，區督導，分行經理時，必須要勾選主管註記&簽核單位主管註記
                If bolBoss And objHR3000.GetAfterWorkStatusTypeByWorkStatus_EmployeeReason(ViewState.Item("WorkStatus"), strReason) = "1" Then '20160113 wei modify 增加判斷若異動後在職才檢查
                    Bsp.Utility.ShowFormatMessage(Me, "H_00000", "異動後工作性質為單位主管，區督導，分行經理時，必須要勾選主管註記&簽核單位主管註記")
                    chkIsBoss.Focus()
                    Return False
                End If
            End If
            If chkIsGroupBoss.Checked Then
                If hidCompID.Value = ddlNewCompID.SelectedValue And objHR3000.GetAfterWorkStatusTypeByWorkStatus_EmployeeReason(ViewState.Item("WorkStatus"), strReason) = "1" Then '20160113 wei modify 增加判斷若異動後在職才檢查
                    strWhere = " And CompID = " & Bsp.Utility.Quote(ddlNewCompID.SelectedValue) & " And FlowOrganID = " & Bsp.Utility.Quote(ucSelectFlowOrgan.SelectedOrganID) & " And EmpID<>" & Bsp.Utility.Quote(hidEmpID.Value)
                    strWhere &= " And ValidMark='0' And IsGroupBoss='1' And CompID=NewCompID"
                    strWhere &= " And ValidDate = " & Bsp.Utility.Quote(ucValidDate.DateText)   '20160104 wei add 增加生效日判斷
                    strWhere &= " And Reason in (Select Reason From WorkStatus_EmployeeReason where AfterWorkStatusType in ('1')) " '20160104 wei add 增加判斷異動後需為在職的原因
                    If objHR.IsDataExists("EmployeeWait", strWhere) Then
                        Bsp.Utility.ShowMessage(Me, "待生效資料已有相同最小簽核單位的單位主管註記！")
                        chkIsGroupBoss.Focus()
                        Return False
                    End If
                    strWhere = " And AddCompID = " & Bsp.Utility.Quote(ddlNewCompID.SelectedValue) & " And AddFlowOrganID = " & Bsp.Utility.Quote(ucSelectFlowOrgan.SelectedOrganID) & " And EmpID<>" & Bsp.Utility.Quote(hidEmpID.Value)
                    strWhere &= " And ValidMark='0' And IsGroupBoss='1' "
                    strWhere &= " And ValidDate = " & Bsp.Utility.Quote(ucValidDate.DateText)   '20160104 wei add 增加生效日判斷
                    strWhere &= " And Reason in ('1') " '20160104 wei add 增加判斷異動後為調兼的原因
                    If objHR.IsDataExists("EmpAdditionWait", strWhere) Then
                        Bsp.Utility.ShowMessage(Me, "待生效資料已有相同最小簽核單位的單位主管註記！")
                        chkIsGroupBoss.Focus()
                        Return False
                    End If
                End If
            Else
                '20150601 wei add 檢核若異動後工作性質為BA0001單位主管，區督導，分行經理時，必須要勾選主管註記&簽核單位主管註記
                If bolBoss And objHR3000.GetAfterWorkStatusTypeByWorkStatus_EmployeeReason(ViewState.Item("WorkStatus"), strReason) = "1" Then '20160113 wei modify 增加判斷若異動後在職才檢查
                    Bsp.Utility.ShowFormatMessage(Me, "H_00000", "異動後工作性質為單位主管，區督導，分行經理時，必須要勾選主管註記&簽核單位主管註記")
                    chkIsGroupBoss.Focus()
                    Return False
                End If
            End If
        Else
            If chkIsBoss.Checked Or chkIsGroupBoss.Checked Then '20150601 wei modify Or chkIsSecBoss.Checked Or chkIsSecGroupBoss.Checked
                Bsp.Utility.ShowFormatMessage(Me, "H_00000", "已選擇單位(副)主或簽核單位(副)主管，請選擇主管任用方式")
                ddlBossType.Focus()
                Return False
            Else
                '20150601 wei add 檢核若異動後工作性質為BA0001單位主管，區督導，分行經理時，必須要勾選主管註記&簽核單位主管註記
                If bolBoss And objHR3000.GetAfterWorkStatusTypeByWorkStatus_EmployeeReason(ViewState.Item("WorkStatus"), strReason) = "1" Then '20160113 wei modify 增加判斷若異動後在職才檢查
                    Bsp.Utility.ShowFormatMessage(Me, "H_00000", "異動後工作性質為單位主管，區督導，分行經理時，必須要勾選主管註記&簽核單位主管註記")
                    chkIsBoss.Focus()
                    Return False
                End If
            End If
        End If

        If Not objHR3000.CheckOrgan_IsBoss("Ins", hidCompID.Value, hidEmpID.Value, ucValidDate.DateText, ddlReason.SelectedValue, hdfEmpAdditionSeqNo.Value) Then
            Bsp.Utility.ShowFormatMessage(Me, "H_00000", "待異動調兼紀錄中有重覆單位(簽核單位)的主管註記!")
            Return False
        End If

        '工作地點
        If ddlWorkSiteID.SelectedValue = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblWorkSiteID.Text)
            ddlWorkSiteID.Focus()
            Return False
        End If

        '備註
        strValue = txtRemark.Text.Trim
        If Bsp.Utility.getStringLength(strValue) > 100 Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00040", lblRemark.Text, "100")
            txtRemark.Focus()
            Return False
        End If
        txtRemark.Text = strValue

        '預計報到日
        strValue = ucContractDate.DateText()
        If strValue <> "" And Not IsDate(strValue) Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00070", lblContractDate.Text)
            ucContractDate.Focus()
            Return False
        End If
        '應試者編號
        If txtRecID.Text.Trim <> "" Or ucContractDate.DateText <> "" Then
            If Not objHR3000.CheckRecID(ddlNewCompID.SelectedValue, txtRecID.Text.Trim(), ucContractDate.DateText) Then
                Bsp.Utility.ShowMessage(Me, "查無應試者編號及預計報到日！")
                txtRecID.Focus()
                Return False
            End If
        End If

        '跨公司異動檢核
        '生效後為【在職】的異動原因(02,03,05,06,07,50)的異動原因，都先檢查【人員主檔】在【異動後公司以外】的公司資料是否在職
        '若在職，再檢查檢查【待異動檔】在【異動後公司以外】的公司資料是否有異動後【非在職】(12,14,18,11,15,16,17,13,50)的待異動
        '若有，才能建檔，否則不能建檔(HR2300到職建檔亦同)
        Dim strMsg As String = ""
        If objHR3000.GetAfterWorkStatusTypeByWorkStatus_EmployeeReason(ViewState.Item("WorkStatus"), strReason) = "1" Then  '20150729 wei modify 改為依任職狀況vs異動原因表來判斷異動後是否在職
            'If strReason = "02" Or strReason = "03" Or strReason = "05" Or strReason = "06" Or strReason = "07" Or (strReason = "50" And hidCompID.Value = ddlNewCompID.SelectedValue) Then    20150729 wei del
            strMsg = objHR3000.funCheckWorkStatus(ucValidDate.DateText, hidCompID.Value, ddlNewCompID.SelectedValue, lblEmpID_O.Text.Trim, hidIDNo.Value, strReason)
            If strMsg <> "" Then
                Bsp.Utility.ShowMessage(Me, strMsg)
                Return False
            End If
        End If

        strMsg = funCheckWorkStatus(ucValidDate.DateText, hidCompID.Value, ddlNewCompID.SelectedValue, lblEmpID_O.Text.Trim, hidIDNo.Value, strReason)
        If strMsg <> "" Then
            Bsp.Utility.ShowMessage(Me, strMsg)
            Return False
        End If


        '檢核是否有相同生效日期及異動原因的紀錄存在 
        If strReason <> ViewState.Item("Reason") Or ucValidDate.DateText <> hidValidDate.Value Then
            strWhere = " And CompID = " & Bsp.Utility.Quote(hidCompID.Value) & " And EmpID = " & Bsp.Utility.Quote(lblEmpID_O.Text.Trim)
            strWhere &= " And ValidDate = " & Bsp.Utility.Quote(ucValidDate.DateText) & " And Reason = " & Bsp.Utility.Quote(ddlReason.SelectedValue)
            If objHR.IsDataExists("EmployeeWait", strWhere) Then
                Bsp.Utility.ShowMessage(Me, "資料重複：待異動紀錄已有相同生效日期及異動原因的資料存在")
                Return False
            Else
                strWhere = " And CompID = " & Bsp.Utility.Quote(hidCompID.Value) & " And EmpID = " & Bsp.Utility.Quote(lblEmpID_O.Text.Trim)
                strWhere &= " And ModifyDate = " & Bsp.Utility.Quote(ucValidDate.DateText) & " And Reason = " & Bsp.Utility.Quote(ddlReason.SelectedValue)
                '避免批次sHR3000執行時，發生失敗，故在前端輸入時檢核是否重複
                If objHR.IsDataExists("EmployeeLog", strWhere) Then
                    Bsp.Utility.ShowMessage(Me, "資料重複：企業團經歷紀錄已有相同生效日期及異動原因的資料存在")
                    Return False
                End If
            End If
        End If

        '20150602 wei add 檢核若工作性質是分行作業主管BO0001，但未勾選簽核主管註記，就跳出提醒確認是否勾選簽核單位主管註記，若確認新增，則儲存
        If bolOPBoss And Not chkIsGroupBoss.Checked Then
            Bsp.Utility.RunClientScript(Me.Page, "IsTOConfirm('OPBoss');")
            Return False
        End If

        '20150602 wei add 異動前A單位為A主管(主管or簽核主管)，待異動輸入A單位換B主管，跳出提醒視窗確認【A單位將更換為B主管，請確認是否繼續儲存?】，按鈕有【確認，取消】，若確認，則繼續儲存
        '主管
        If chkIsBoss.Checked Then
            strWhere = " And CompID = " & Bsp.Utility.Quote(ddlNewCompID.SelectedValue) & " And OrganID = " & Bsp.Utility.Quote(ucSelectHROrgan.SelectedOrganID)
            strWhere &= " And BossCompID = " & Bsp.Utility.Quote(hidCompID.Value) & " And Boss = " & Bsp.Utility.Quote(lblEmpID_O.Text.Trim)
            If Not objHR.IsDataExists("Organization", strWhere) Then
                If ddlNewCompID.SelectedValue = hidCompID.Value Then    '20160104 wei add 增加判斷為同公司異動才顯示
                    Bsp.Utility.RunClientScript(Me.Page, "IsTOConfirm1('" & ucSelectHROrgan.SelectedOrganName & "主管將更換為" & lblUserName.Text.Trim & "，請確認是否繼續儲存?');")
                    Return False
                End If

            End If
        End If
        '簽核主管
        If chkIsGroupBoss.Checked Then
            strWhere = " And OrganID = " & Bsp.Utility.Quote(ucSelectFlowOrgan.SelectedOrganID)
            strWhere &= " And BossCompID = " & Bsp.Utility.Quote(hidCompID.Value) & " And Boss = " & Bsp.Utility.Quote(lblEmpID_O.Text.Trim)
            If Not objHR.IsDataExists("OrganizationFlow", strWhere) Then
                If ddlNewCompID.SelectedValue = hidCompID.Value Then    '20160104 wei add 增加判斷為同公司異動才顯示
                    Bsp.Utility.RunClientScript(Me.Page, "IsTOConfirm1('" & ucSelectFlowOrgan.SelectedOrganIDName & "主管將更換為" & lblUserName.Text.Trim & "，請確認是否繼續儲存?');")
                    Return False
                End If

            End If
        End If

        ''若為證券體系，增加檢核筆記型電腦貸款    
        ''若離職員工仍有『貸款未結案、離職但公司補助款期限未到期不列入所得』，則出訊息提醒
        'If strPayrollComp = "SPHSC1" Then
        '    If gfunSelectCount("HRCodeMap", " where TabName='NBQuitFlag' and FldName='Reason' and NotShowFlag='0' and Code = '" & gfunComboGetKey(cmbReason1.Text, Chr$(9)) & "' ") > 0 Then
        '        strSqlEW = " where CompID = '" & strCompID & "' and EmpID = '" & txtEmpID1.Text & "' "
        '        strSqlEW = strSqlEW & " and (CloseFlag='0' "
        '        strSqlEW = strSqlEW & " or (Convert(Char(8)," & mskValidDate.Text & ",112) < Convert(Char(8),DATEADD(Month,CompTerm,NBDate),112)) "
        '        strSqlEW = strSqlEW & " ) "

        '        If gfunSelectCount("EmpNoteBook", strSqlEW) > 0 Then
        '            MsgBox("該員工的筆記型電腦貸款未結案 或 公司補助款期限未到期不列入所得", vbInformation, App.Title)
        '        End If
        '    End If
        'End If

        Return True
    End Function

    '跨公司異動檢核
    '生效後為【在職】的異動原因(02,03,05,06,07,50)的異動原因，都先檢查【人員主檔】在【異動後公司以外】的公司資料是否在職
    '若在職，再檢查檢查【待異動檔】在【異動後公司以外】的公司資料是否有異動後【非在職】(12,14,18,11,15,16,17,13,50)的待異動
    '若有，才能建檔，否則不能建檔(HR2300到職建檔亦同)
    Private Function funCheckWorkStatus(ByVal strValidDate As String, ByVal strOldCompID As String, ByVal strNewCompID As String, ByVal strEmpID As String, ByVal strIDNo As String, ByVal strReason As String) As String
        Dim objHR As New HR()
        Dim objHR3000 As New HR3000()
        Dim strWhere As New StringBuilder

        Dim strMsgComp As String = ""
        Dim strMsgValidDate As String = ""
        If objHR3000.GetAfterWorkStatusTypeByWorkStatus_EmployeeReason(ViewState.Item("WorkStatus"), strReason) = "1" Then  '20150729 wei modify 改為依任職狀況vs異動原因表來判斷異動後是否在職
            'If ViewState.Item("Reason") = "11" Or ViewState.Item("Reason") = "12" Or ViewState.Item("Reason") = "13" Or ViewState.Item("Reason") = "14" Or ViewState.Item("Reason") = "15" Or ViewState.Item("Reason") = "16" Or ViewState.Item("Reason") = "17" Or ViewState.Item("Reason") = "18" Or ViewState.Item("Reason") = "19" Or (ViewState.Item("Reason") = "50" And strOldCompID <> ViewState.Item("NewCompID")) Then    '20150721 wei modify 19留停延長    20150729 wei del
            '檢查是否有待異動後在職紀錄
            strWhere.Length = 0
            strWhere.AppendLine("And P.CompID <> " & Bsp.Utility.Quote(strOldCompID) & " And P.IDNo = " & Bsp.Utility.Quote(strIDNo))
            'strWhere.AppendLine("And (E.Reason in ('02','03','05','06','07')") '20150729 wei del
            'strWhere.AppendLine("or (E.Reason ='50' and E.CompID=E.NewCompID) )")  '20150729 wei del
            strWhere.AppendLine(" And (E.Reason in (select Reason From WorkStatus_EmployeeReason where AfterWorkStatusType='1' ) )")  '20150729 wei modify 改為依任職狀況vs異動原因表來判斷異動後是否在職 '20150903 wei modify
            strWhere.AppendLine("And E.ValidMark = '0' And ValidDate<=" & Bsp.Utility.Quote(ViewState.Item("ValidDate")))
            If objHR.IsDataExists("EmployeeWait E Inner Join Personal P On E.CompID=P.CompID and E.EmpID=P.EmpID", strWhere.ToString) Then
                If (strReason <> "11" And strReason <> "12" And strReason <> "13" And strReason <> "14" And strReason <> "15" And strReason <> "16" And strReason <> "17" And strReason <> "18" And strReason <> "19" And Not (strReason = "50" And strOldCompID <> strNewCompID)) Or strValidDate >= ViewState.Item("ValidDate") Then    '20150721 wei modify 19留停延長
                    Using dt1 As DataTable = objHR3000.GetCheckMsg("P.CompID,C.CompName,Convert(char(10),E.ValidDate,111) as ValidDate", _
                                                 "EmployeeWait E Inner Join Personal P On E.CompID=P.CompID and E.EmpID=P.EmpID Inner Join Company C On P.CompID=C.CompID", _
                                                 strWhere.ToString)
                        strMsgComp = dt1.Rows(0).Item("CompName").ToString
                        strMsgValidDate = dt1.Rows(0).Item("ValidDate").ToString
                    End Using
                    Return "資料重複：該員將於" & strMsgComp & strMsgValidDate & "復職，不得重覆兩公司在職"
                End If
            End If
        End If


        Return ""
    End Function

    Private Function funCheckEmployeeWait(ByVal strValidDate As String, ByVal strCompID As String, ByVal strEmpID As String, ByVal strSeq As String) As Boolean
        Dim strWhere As String = ""
        Dim objHR As New HR
        strWhere = " And CompID = " & Bsp.Utility.Quote(strCompID) & " And EmpID = " & Bsp.Utility.Quote(strEmpID)
        strWhere &= " And ValidDate >= " & Bsp.Utility.Quote(ucValidDate.DateText) & " And ValidMark='0' And Seq<>" & Bsp.Utility.Quote(strSeq)
        If objHR.IsDataExists("EmployeeWait", strWhere.ToString) Then '存在則檢查是否有待異動離職紀錄
            Return True
        End If
        Return False

    End Function

    Private Function funCheckEmployeeLog(ByVal strValidDate As String, ByVal strCompID As String, ByVal strEmpID As String) As Boolean
        Dim strWhere As String = ""
        Dim objHR As New HR
        strWhere = " And CompID = " & Bsp.Utility.Quote(strCompID) & " And EmpID = " & Bsp.Utility.Quote(strEmpID)
        strWhere &= " And ModifyDate >= " & Bsp.Utility.Quote(ucValidDate.DateText)
        If objHR.IsDataExists("EmployeeLog", strWhere.ToString) Then
            Return True
        End If
        Return False
    End Function

#End Region
    '233
    '20161108 ADD
    '隱藏的按鈕--生效日(組織待異動生效日期<=人員待異動生效日)
    Protected Sub btnDateChange_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDateChange.Click
        If ucValidDate.DateText <> "" And ucValidDate.DateText <> "____/__/__" And ucValidDate.DateText <> Now.Date Then
            ucSelectHROrgan.HasWait = True
            ucSelectHROrgan.ValidDate = ucValidDate.DateText
            ucSelectEmpAdditionHROrgan.HasWait = True
            ucSelectEmpAdditionHROrgan.ValidDate = ucValidDate.DateText
        Else
            ucSelectHROrgan.HasWait = False
            ucSelectHROrgan.ValidDate = ""
            ucSelectEmpAdditionHROrgan.HasWait = False
            ucSelectEmpAdditionHROrgan.ValidDate = ""
        End If
        ucSelectHROrgan.LoadData(ViewState.Item("NewCompID"), "Y")
        ucSelectEmpAdditionHROrgan.LoadData(ddlNewCompID.SelectedValue)
    End Sub
#Region "ddlComp"    '公司
    Protected Sub ddlNewCompID_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlNewCompID.SelectedIndexChanged
        If ViewState.Item("NewCompID") <> ddlNewCompID.SelectedValue Then
            ddlWorkType.Items.Clear()
            'hidWorkTypeID.Value = ""    '20150623 wei del
            ViewState.Item("WorkTypeID") = ""   '20150623 wei modify
            ddlPosition.Items.Clear()
            'hidPositionID.Value = ""    20150623 wei del
            ViewState.Item("PositionID") = ""     '20150623 wei modify
        End If
        ViewState.Item("NewCompID") = ddlNewCompID.SelectedValue
        '部門、科組課
        ucSelectHROrgan.LoadData(ViewState.Item("NewCompID"))
        ucSelectFlowOrgan.LoadData(ucSelectHROrgan.SelectedOrganID, "Y")
        'ddlFlowOrganID.Items.Clear()
        'ddlFlowOrganID.Items.Insert(0, New ListItem("---請選擇---", ""))

        '職等           
        Bsp.Utility.Rank(ddlRankID, ViewState.Item("NewCompID"))
        ddlRankID.Items.Insert(0, New ListItem("---請選擇---", ""))
        ddlTitleID.Items.Clear()
        ddlTitleID.Items.Insert(0, "---請先選擇職等---")

        '工作地點
        Bsp.Utility.WorkSite(ddlWorkSiteID, ViewState.Item("NewCompID"), "")
        ddlWorkSiteID.Items.Insert(0, New ListItem("---請選擇---", ""))

        '班別
        HR3000.FillEmployeeWTID(ddlWTID, ViewState.Item("NewCompID"))
        ddlWTID.Items.Insert(0, New ListItem("---請選擇---", ""))
        CheckShow_Hide()
    End Sub
    Protected Sub ddlEmpAdditionCompID_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlEmpAdditionCompID.SelectedIndexChanged
        '部門、科組課
        ucSelectEmpAdditionHROrgan.LoadData(ddlEmpAdditionCompID.SelectedValue)
        ucSelectAddFlowOrgan.LoadData(ucSelectEmpAdditionHROrgan.SelectedOrganID, "Y")
        'ddlEmpAdditonFlowOrganID.Items.Clear()
        'ddlEmpAdditonFlowOrganID.Items.Insert(0, New ListItem("---請選擇---", ""))
        UdpEmpAdditionDeptID.Update()
        UpdEmpAdditionFlowOrgnaID.Update()

    End Sub
#End Region
    Protected Sub ucSelectPosition_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ucSelectPosition.Load
        '載入按鈕-職位選單畫面
        If ddlNewCompID.SelectedItem.Value <> "" Then
            ucSelectPosition.QueryCompID = ddlNewCompID.SelectedItem.Value
            ucSelectPosition.QueryEmpID = ""
            ucSelectPosition.DefaultPosition = lblSelectPosition.Text
            ucSelectPosition.QueryOrganID = ucSelectHROrgan.SelectedOrganID
            ucSelectPosition.Fields = New FieldState() { _
                    New FieldState("PositionID", "職位代碼", True, True), _
                    New FieldState("Remark", "職位名稱", True, True)}

            '2016/11/21 For組織待異動
            If ucSelectHROrgan.SelectedOrganName.ToString().EndsWith("(未生效)") And ucValidDate.DateText <> "" And ucValidDate.DateText <> "____/__/__" Then
                ucSelectPosition.IsWait = True
                ucSelectPosition.ValidDate = ucValidDate.DateText
            Else
                ucSelectPosition.IsWait = False
                ucSelectPosition.ValidDate = ""
            End If
        End If
        CheckShow_Hide()
    End Sub
    'Protected Sub ucSelectPosition_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ucSelectPosition.Load
    '    '載入按鈕-職位選單畫面
    '    If ddlNewCompID.SelectedItem.Value <> "" Then
    '        ucSelectPosition.QueryCompID = ddlNewCompID.SelectedItem.Value
    '        ucSelectPosition.QueryEmpID = ""
    '        ucSelectPosition.QueryOrganID = ucSelectHROrgan.SelectedOrganID
    '        ucSelectPosition.DefaultPosition = lblSelectPosition.Text
    '        ucSelectPosition.Fields = New FieldState() { _
    '                New FieldState("PositionID", "職位代碼", True, True), _
    '                New FieldState("Remark", "職位名稱", True, True)}
    '    End If
    '    CheckShow_Hide()
    'End Sub
    Protected Sub ucSelectWorkType_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ucSelectWorkType.Load
        '載入按鈕-工作性質選單畫面
        If ddlNewCompID.SelectedItem.Value <> "" Then
            ucSelectWorkType.QueryCompID = ddlNewCompID.SelectedItem.Value
            ucSelectWorkType.QueryEmpID = ""
            ucSelectWorkType.DefaultWorkType = lblSelectWorkType.Text
            ucSelectWorkType.QueryOrganID = ucSelectHROrgan.SelectedDeptID

            '2016/05/03 SPHBKC資料已併入WorkTypeID中
            'If ddlNewCompID.SelectedItem.Value = "SPHBKC" Then
            '    ucSelectWorkType.Fields = New FieldState() { _
            '        New FieldState("Code", "工作性質代碼", True, True), _
            '        New FieldState("CodeName", "工作性質名稱", True, True)}
            'Else
            ucSelectWorkType.Fields = New FieldState() { _
                New FieldState("WorkTypeID", "工作性質代碼", True, True), _
                New FieldState("Remark", "工作性質名稱", True, True)}
            'End If

            '2016/11/21 For組織待異動
            If ucSelectHROrgan.SelectedOrganName.ToString().EndsWith("(未生效)") And ucValidDate.DateText <> "" And ucValidDate.DateText <> "____/__/__" Then
                ucSelectWorkType.IsWait = True
                ucSelectWorkType.ValidDate = ucValidDate.DateText
            Else
                ucSelectWorkType.IsWait = False
                ucSelectWorkType.ValidDate = ""
            End If
        End If
        CheckShow_Hide()
    End Sub
    'Protected Sub ucSelectWorkType_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ucSelectWorkType.Load
    '    '載入按鈕-工作性質選單畫面
    '    If ddlNewCompID.SelectedItem.Value <> "" Then
    '        ucSelectWorkType.QueryCompID = ddlNewCompID.SelectedItem.Value
    '        ucSelectWorkType.QueryEmpID = ""
    '        ucSelectWorkType.QueryOrganID = ucSelectHROrgan.SelectedDeptID
    '        ucSelectWorkType.DefaultWorkType = lblSelectWorkType.Text
    '        '2016/05/03 SPHBKC資料已併入WorkTypeID中
    '        'If ddlNewCompID.SelectedItem.Value = "SPHBKC" Then
    '        '    ucSelectWorkType.Fields = New FieldState() { _
    '        '        New FieldState("Code", "工作性質代碼", True, True), _
    '        '        New FieldState("CodeName", "工作性質名稱", True, True)}
    '        'Else
    '        ucSelectWorkType.Fields = New FieldState() { _
    '            New FieldState("WorkTypeID", "工作性質代碼", True, True), _
    '            New FieldState("Remark", "工作性質名稱", True, True)}
    '        'End If
    '    End If
    '    CheckShow_Hide()
    'End Sub

    '將選擇那筆 改為 第一筆為主要職位
    Protected Sub ddlPosition_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlPosition.SelectedIndexChanged
        Dim strRst1 As String = ""
        Dim strRst2 As String = ""
        Dim strMainPosition As String = ""
        Dim strPosition As String = ""
        For i As Integer = 0 To ddlPosition.Items.Count - 1

            If ddlPosition.Items(i).Selected Then
                strRst1 = "'" + ddlPosition.Items(i).Value + "'"
                strMainPosition = "1|" + ddlPosition.Items(i).Value
            Else
                If strRst2 <> "" Then strRst2 += ","
                strRst2 += "'" + ddlPosition.Items(i).Value + "'"
                If strPosition <> "" Then strPosition += "|"
                strPosition += "0|" + ddlPosition.Items(i).Value
            End If
        Next
        If strRst2 = "" Then
            lblSelectPosition.Text = strRst1
            'hidPositionID.Value = strMainPosition   '20150623 wei del
            ViewState.Item("PositionID") = strMainPosition  '20150623 wei modify
        Else
            lblSelectPosition.Text = strRst1 + "," + strRst2
            'hidPositionID.Value = strMainPosition + "|" + strPosition   '20150623 wei del
            ViewState.Item("PositionID") = strMainPosition + "|" + strPosition  '20150623 wei modify
        End If
    End Sub
    '將選擇那筆 改為 第一筆為主要工作性質
    Protected Sub ddlWorkType_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlWorkType.SelectedIndexChanged
        Dim strRst1 As String = ""
        Dim strRst2 As String = ""
        Dim strMainWorkType As String = ""
        Dim strWorkType As String = ""
        For i As Integer = 0 To ddlWorkType.Items.Count - 1

            If ddlWorkType.Items(i).Selected Then
                strRst1 = "'" + ddlWorkType.Items(i).Value + "'"
                strMainWorkType = "1|" + ddlWorkType.Items(i).Value
            Else
                If strRst2 <> "" Then strRst2 += ","
                strRst2 += "'" + ddlWorkType.Items(i).Value + "'"
                If strWorkType <> "" Then strWorkType += "|"
                strWorkType += "0|" + ddlWorkType.Items(i).Value
            End If
        Next
        If strRst2 = "" Then
            lblSelectWorkType.Text = strRst1
            'hidWorkTypeID.Value = strMainWorkType  '20150623 wei del
            ViewState.Item("WorkTypeID") = strMainWorkType  '20150623 wei modify
        Else
            lblSelectWorkType.Text = strRst1 + "," + strRst2
            'hidWorkTypeID.Value = strMainWorkType + "|" + strWorkType  20150623 wei del
            ViewState.Item("WorkTypeID") = strMainWorkType + "|" + strWorkType  '20150623 wei modify
        End If
    End Sub
#Region "ucSelectHROrgan"    '233
    Protected Sub ucSelectHROrgan_ucSelectOrganIDSelectedIndexChangedHandler_SelectChange(ByVal sender As Object, ByVal e As System.EventArgs) Handles ucSelectHROrgan.ucSelectOrganIDSelectedIndexChangedHandler_SelectChange
        Dim objHR As New HR
        Dim objHR3000 As New HR3000
        Dim objRG As New RG1
        Dim strMainWorkTypeID As String = ""
        Dim strMainPositionID As String = ""
        Dim bolWorkType As Boolean = False
        Dim bolPosition As Boolean = False

        If ucSelectHROrgan.SelectedOrganName.ToString().EndsWith("(未生效)") Then
            Using dt As DataTable = objRG.GetOrgWaitData(hidCompID.Value, ucSelectHROrgan.SelectedOrganID, ucValidDate.DateText, "1")
                If dt.Rows.Count > 0 Then
                    '工作地點
                    Bsp.Utility.SetSelectedIndex(ddlWorkSiteID, dt.Rows(0).Item("WorkSiteID").ToString.Trim)
                    UpdWorkSite.Update()

                    ViewState.Item("GroupID") = dt.Rows(0).Item("GroupID").ToString.Trim
                End If
            End Using

            '最小簽核單位
            If ucValidDate.DateText <> "" And ucValidDate.DateText <> "____/__/__" Then
                ucSelectFlowOrgan.IsWait = True
                ucSelectFlowOrgan.ValidDate = ucValidDate.DateText
            Else
                ucSelectFlowOrgan.IsWait = False
                ucSelectFlowOrgan.ValidDate = ""
            End If
            ucSelectFlowOrgan.LoadData(ucSelectHROrgan.SelectedOrganID, "Y")
            UpdFlowOrgnaID.Update()
            ucSelectFlowOrgan.SetDefaultOrgan()
        Else
            '工作地點
            Dim strWorkSiteID As String = ""
            Using dt As DataTable = objHR.GetWorkSite(ddlNewCompID.SelectedItem.Value, ucSelectHROrgan.SelectedOrganID)
                If dt.Rows.Count > 0 Then
                    strWorkSiteID = dt.Rows(0).Item(0).ToString
                End If
            End Using
            Bsp.Utility.SetSelectedIndex(ddlWorkSiteID, strWorkSiteID)
            UpdWorkSite.Update()

            '最小簽核單位
            'GetEmpFlowOrganID()
            ucSelectFlowOrgan.IsWait = False
            ucSelectFlowOrgan.ValidDate = ""
            ucSelectFlowOrgan.LoadData(ucSelectHROrgan.SelectedOrganID, "Y")
            UpdFlowOrgnaID.Update()
            ucSelectFlowOrgan.SetDefaultOrgan()
        End If

        '20161104 Beatrice Add
        ddlBusinessType.SelectedValue = ""
        ddlEmpFlowRemarkID.Items.Clear()
        ddlEmpFlowRemarkID.Items.Insert(0, New ListItem("---請選擇---", ""))
        UpdEmpFlowRemark.Update()

        '工作性質
        ViewState.Item("WorkTypeID") = ""
        Using dt As DataTable = objHR3000.GetEmpWorkTypeByEmpOrgan(ddlNewCompID.SelectedItem.Value, lblEmpID_O.Text.Trim, ucSelectHROrgan.SelectedOrganID)
            If dt.Rows.Count > 0 Then
                bolWorkType = True
                For intCnt = 0 To dt.Rows.Count - 1
                    ViewState.Item("WorkTypeID") = ViewState.Item("WorkTypeID") & dt.Rows(intCnt)("PrincipalFlag").ToString() & "|" & dt.Rows(intCnt)("WorkTypeID").ToString() & "|"
                Next
                ViewState.Item("WorkTypeID") = Left(ViewState.Item("WorkTypeID"), Len(ViewState.Item("WorkTypeID")) - 1)
            End If
        End Using

        If bolWorkType Then
            'hidWorkTypeID.Value = ViewState.Item("WorkTypeID")

            If ViewState.Item("WorkTypeID") <> "" Then  'hidWorkTypeID.Value '20150623 wei mdoify
                strMainWorkTypeID = GetWorkTypeID(ViewState.Item("WorkTypeID"))
                UpdWorkType.Update()
                Bsp.Utility.SetSelectedIndex(ddlWorkType, strMainWorkTypeID)
            End If
        Else
            ddlWorkType.Items.Clear()
            ddlWorkType.Items.Insert(0, New ListItem("------", ""))
            UpdWorkType.Update()
            'hidWorkTypeID.Value = ""   '20150623 wei del
            ViewState.Item("WorkTypeID") = ""   '20150623 wei modify
            lblSelectWorkType.Text = ""
            '20150309 wei del
            'Using dt As DataTable = objHR3000.GetWorkTypeIDByOrgan(ddlNewCompID.SelectedItem.Value, ucSelectHROrgan.SelectedOrganID)
            '    If dt.Rows.Count > 0 Then
            '        ViewState.Item("WorkTypeID") = "1|" & dt.Rows(0).Item(0).ToString
            '    End If
            'End Using

            'hidWorkTypeID.Value = ViewState.Item("WorkTypeID")

            'If hidWorkTypeID.Value <> "" Then
            '    strMainWorkTypeID = GetWorkTypeID(ViewState.Item("WorkTypeID"))
            '    UpdWorkType.Update()
            '    Bsp.Utility.SetSelectedIndex(ddlWorkType, strMainWorkTypeID)
            'End If
        End If

        '職位
        ViewState.Item("PositionID") = ""
        Using dt As DataTable = objHR3000.GetEmpPositionByEmpOrgan(ddlNewCompID.SelectedItem.Value, lblEmpID_O.Text.Trim, ucSelectHROrgan.SelectedOrganID)
            If dt.Rows.Count > 0 Then
                bolPosition = True
                For intCnt = 0 To dt.Rows.Count - 1
                    ViewState.Item("PositionID") = ViewState.Item("PositionID") & dt.Rows(intCnt)("PrincipalFlag").ToString() & "|" & dt.Rows(intCnt)("PositionID").ToString() & "|"
                Next
                ViewState.Item("PositionID") = Left(ViewState.Item("PositionID"), Len(ViewState.Item("PositionID")) - 1)
            End If
        End Using

        If bolPosition Then
            'hidPositionID.Value = ViewState.Item("PositionID")

            If ViewState.Item("PositionID") <> "" Then  'hidPositionID.Value 20150623 wei modify
                strMainPositionID = GetPositionID(ViewState.Item("PositionID"))
                UpdPosition.Update()
                Bsp.Utility.SetSelectedIndex(ddlPosition, strMainPositionID)
            End If
        Else
            ddlPosition.Items.Clear()
            ddlPosition.Items.Insert(0, New ListItem("------", ""))
            UpdPosition.Update()
            'hidPositionID.Value = ""   20150623 wei del
            ViewState.Item("PositionID") = ""   '20150623 wei modify
            lblSelectPosition.Text = ""

            '20150309 wei del
            'Using dt As DataTable = objHR3000.GetPositionIDByOrgan(ddlNewCompID.SelectedItem.Value, ucSelectHROrgan.SelectedOrganID)
            '    If dt.Rows.Count > 0 Then
            '        ViewState.Item("PositionID") = "1|" & dt.Rows(0).Item(0).ToString
            '    End If
            'End Using

            'hidPositionID.Value = ViewState.Item("PositionID")

            'If hidPositionID.Value <> "" Then
            '    strMainPositionID = GetPositionID(ViewState.Item("PositionID"))
            '    UpdPosition.Update()
            '    Bsp.Utility.SetSelectedIndex(ddlPosition, strMainPositionID)
            'End If
        End If

    End Sub
    Protected Sub ucSelectEmpadditionHROrgan_ucSelectOrganIDSelectedIndexChangedHandler_SelectChange(ByVal sender As Object, ByVal e As System.EventArgs) Handles ucSelectEmpAdditionHROrgan.ucSelectOrganIDSelectedIndexChangedHandler_SelectChange
        Dim objHR As New HR
        Dim objHR3000 As New HR3000

        If ucSelectEmpAdditionHROrgan.SelectedOrganName.ToString().EndsWith("(未生效)") Then
            '最小簽核單位
            If ucValidDate.DateText <> "" And ucValidDate.DateText <> "____/__/__" Then
                ucSelectAddFlowOrgan.IsWait = True
                ucSelectAddFlowOrgan.ValidDate = ucValidDate.DateText
            Else
                ucSelectAddFlowOrgan.IsWait = False
                ucSelectAddFlowOrgan.ValidDate = ""
            End If
            ucSelectAddFlowOrgan.LoadData(ucSelectEmpAdditionHROrgan.SelectedOrganID)
            UpdEmpAdditionFlowOrgnaID.Update()
            ucSelectAddFlowOrgan.SetDefaultOrgan()
        Else
            '最小簽核單位
            'GetEmpAdditionFlowOrganID()
            ucSelectAddFlowOrgan.LoadData(ucSelectEmpAdditionHROrgan.SelectedOrganID)
            UpdEmpAdditionFlowOrgnaID.Update()
            ucSelectAddFlowOrgan.SetDefaultOrgan()
        End If
    End Sub
#End Region


    Public Overrides Sub DoModalReturn(ByVal returnValue As String)

        Dim strSql As String = ""

        If returnValue <> "" Then
            Dim aryData() As String = returnValue.Split(":")

            Select Case aryData(0)
                Case "ucSelectPosition"
                    lblSelectPosition.Text = aryData(1)

                    If lblSelectPosition.Text <> "''" Then  '非必填時，回傳空值
                        '載入 職位 下拉式選單
                        'strSql = " and PositionID in (" + lblSelectPosition.Text + ") "
                        strSql = " and PositionID in (" + lblSelectPosition.Text + ") and CompID = '" + ddlNewCompID.SelectedItem.Value + "'"
                        Bsp.Utility.Position(ddlPosition, "PositionID", , strSql)
                        'Bsp.Utility.RE_PositionU(ddlPosition, "PositionID")

                        '第一筆為主要職位
                        Dim strDefaultValue() As String = lblSelectPosition.Text.Replace("'", "").Split(",")
                        Dim strPosition As String = ""
                        Bsp.Utility.SetSelectedIndex(ddlPosition, strDefaultValue(0))
                        For intLoop As Integer = 0 To strDefaultValue.GetUpperBound(0)
                            If intLoop = 0 Then
                                strPosition = "1|" + strDefaultValue(intLoop)
                            Else
                                strPosition = strPosition + "|0|" + strDefaultValue(intLoop)
                            End If
                        Next
                        'hidPositionID.Value = strPosition   '20150623 wei del
                        ViewState.Item("PositionID") = strPosition  '20150623 wei modify
                    Else
                        ddlPosition.Items.Clear()
                        ddlPosition.Items.Insert(0, New ListItem("---請選擇---", ""))
                        ViewState.Item("PositionID") = ""
                    End If

                Case "ucSelectWorkType"
                    lblSelectWorkType.Text = aryData(1)

                    If lblSelectWorkType.Text <> "''" Then  '非必填時，回傳空值
                        '載入 工作性質 下拉式選單
                        '2016/05/03 SPHBKC資料已併入WorkTypeID中
                        'If ddlNewCompID.SelectedValue = "SPHBKC" Then
                        '    strSql = " and Code in (" + lblSelectWorkType.Text + ") "
                        '    Bsp.Utility.CWorkType(ddlWorkType, "Code as WorkTypeID", , strSql)
                        'Else
                        strSql = " and WorkTypeID in (" + lblSelectWorkType.Text + ") and CompID = '" + ddlNewCompID.SelectedItem.Value + "'"
                        Bsp.Utility.WorkType(ddlWorkType, "WorkTypeID", , strSql)
                        'End If

                        '第一筆為主要工作性質
                        Dim strDefaultValue() As String = lblSelectWorkType.Text.Replace("'", "").Split(",")
                        Dim strWorkType As String = ""
                        Bsp.Utility.SetSelectedIndex(ddlWorkType, strDefaultValue(0))
                        For intLoop As Integer = 0 To strDefaultValue.GetUpperBound(0)
                            If intLoop = 0 Then
                                strWorkType = "1|" + strDefaultValue(intLoop)
                            Else
                                strWorkType = strWorkType + "|0|" + strDefaultValue(intLoop)
                            End If
                        Next
                        'hidWorkTypeID.Value = strWorkType '20150623 wei del
                        ViewState.Item("WorkTypeID") = strWorkType  '20150623 wei modify
                    Else
                        ddlWorkType.Items.Clear()
                        ddlWorkType.Items.Insert(0, New ListItem("---請選擇---", ""))
                        ViewState.Item("WorkTypeID") = ""
                    End If

                Case "ucRelease"
                    lblReleaseResult.Text = ""
                    Dim aryValue() As String = Split(aryData(1), "|$|")
                    lblReleaseResult.Text = aryValue(0)
                    If lblReleaseResult.Text = "Y" Then
                        If funCheckData() Then
                            If SaveData() Then
                                If funCheckEmployeeLog(ucValidDate.DateText, hidCompID.Value, lblEmpID_O.Text.Trim) Then  '檢查是否有已生效企業團經歷
                                    Bsp.Utility.RunClientScript(Me.Page, "IsTOConfirm('EmployeeLogWait');")
                                    'If MsgBox("此員工生效日後，已有『已生效』的企業團經歷存在，請確認是否要調整資料？", MsgBoxStyle.OkCancel, "") = MsgBoxResult.Ok Then  '導向已生效企業團經歷待異動畫面
                                    '    GoEmployeeLogWait()
                                    'Else
                                    '    GoBack()
                                    'End If
                                Else
                                    If funCheckEmployeeWait(ucValidDate.DateText, hidCompID.Value, lblEmpID_O.Text.Trim, hidSeq.Value) Then '檢查是否有未生效待異動紀錄
                                        Bsp.Utility.RunClientScript(Me.Page, "IsTOConfirm('BackQuery');")
                                        'If MsgBox("此員工生效日後，已有『未生效』的待異動資料存在，請確認是否要調整資料？", MsgBoxStyle.OkCancel, "") = MsgBoxResult.Ok Then  '導回查詢清單頁，並帶入查詢條件-公司代碼，員工編號，狀態-未生效
                                        '    GoBackQuery()
                                        'Else
                                        '    GoBack()
                                        'End If
                                    Else
                                        GoBack()
                                    End If
                                End If

                            End If
                        End If
                    End If
                Case "ucSelectRemark" '20150311 wei add 增加常用備註
                    Dim intPos As Integer = returnValue.IndexOf(":")
                    Dim strWho As String = returnValue.Substring(0, intPos)
                    Dim strValue As String = ""
                    Dim ltCustomer As List(Of Dictionary(Of String, String)) = JsonConvert.DeserializeObject(Of List(Of Dictionary(Of String, String)))(returnValue.Substring(intPos + 1))
                    For Each r As Dictionary(Of String, String) In ltCustomer
                        For Each s As String In r.Keys
                            Select Case s
                                Case "CodeCName"
                                    If strValue = "" Then
                                        strValue = r(s)
                                    Else
                                        strValue = strValue & "," & r(s)
                                    End If
                            End Select
                        Next
                    Next

                    If txtRemark.Text.Trim = "" Then
                        txtRemark.Text = strValue
                    Else
                        txtRemark.Text = txtRemark.Text.Trim & "," & strValue
                    End If
            End Select
        End If
        CheckShow_Hide()
    End Sub
    Private Sub CheckShow_Hide()
        Dim objHR As New HR
        Dim strCompID As String
        If ddlNewCompID.SelectedValue = hidCompID.Value Then
            strCompID = hidCompID.Value
        Else
            strCompID = ddlNewCompID.SelectedValue
        End If
        '20160104 wei del 不分公司別,都顯示班別
        'If strCompID = "SPHSC1" Then
        '    Bsp.Utility.RunClientScript(Me.Page, "show_tr('trWTID');")
        'Else
        '    Bsp.Utility.RunClientScript(Me.Page, "hide_tr('trWTID');")
        'End If

        If objHR.IsRankIDMapFlag(strCompID) Then
            Bsp.Utility.RunClientScript(Me.Page, "show_tr('trPosition');")
        Else
            Bsp.Utility.RunClientScript(Me.Page, "hide_tr('trPosition');")
        End If


    End Sub

    Protected Sub ddlReason_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlReason.SelectedIndexChanged
        Dim strReason = ddlReason.SelectedValue
        lblNotice.Text = ""

        '外派(20)或外調(21)或外調轉任(22)或外調轉任(22)或部門調動(50)或金控調兼(35)
        If strReason = "70" Then    'strReason = "50"   strReason = "20" Or strReason = "21" Or strReason = "22" Or 　'20150806 wei modify 調整為只有70跨公司調動可選擇不同公司
            ddlNewCompID.Enabled = True
            'ucSelectHROrgan.LoadData(ddlNewCompID.SelectedValue)
            'ddlFlowOrganID.Items.Clear()
            'ddlFlowOrganID.Items.Insert(0, New ListItem("---請選擇---", ""))
        Else
            ddlNewCompID.Enabled = False
            If ddlNewCompID.SelectedValue <> ViewState.Item("CompID") Then
                ddlNewCompID.SelectedValue = ViewState.Item("CompID")
                ucSelectHROrgan.LoadData(ddlNewCompID.SelectedValue)
                ucSelectFlowOrgan.LoadData(ucSelectHROrgan.SelectedOrganID, "Y")
                'ddlFlowOrganID.Items.Clear()
                'ddlFlowOrganID.Items.Insert(0, New ListItem("---請選擇---", ""))
            End If

        End If

        If strReason = "02" Or strReason = "03" Or strReason = "05" Or strReason = "06" Or strReason = "07" Or strReason = "33" Or strReason = "34" Or strReason = "50" Or strReason = "60" Or strReason = "70" Then
            Bsp.Utility.RunClientScript(Me.Page, "show_tr('trBossType');")
            Bsp.Utility.RunClientScript(Me.Page, "show_tr('trIsBoss');")
            'chkIsBoss.Visible = True
            'chkIsGroupBoss.Visible = True
        Else
            Bsp.Utility.RunClientScript(Me.Page, "hide_tr('trBossType');")
            Bsp.Utility.RunClientScript(Me.Page, "hide_tr('trIsBoss');")
            'chkIsBoss.Visible = False
            'chkIsGroupBoss.Visible = False
        End If

        '留停或育嬰留停
        If strReason = "12" Or strReason = "18" Or strReason = "19" Then    '20150721 wei modify 19留停延長
            lblDueDate.Visible = True
            ucDueDate.Visible = True
        Else
            lblDueDate.Visible = False
            ucDueDate.Visible = False
        End If

        '20160817 Beatrice Add 異動原因為40升等，畫面直接帶出目前人員檔的【部門/科組課】跟【簽核最小單位】，並鎖住該欄位不可修改。
        If strReason = "40" Then
            Dim strModifyDate As String = ucValidDate.DateText
            ClearData()
            ucValidDate.DateText = strModifyDate
            ddlReason.SelectedValue = "40"
            lblNotice.Text = "僅處理【職等職稱】/【工作性質】/【職位】"
        End If

        CheckShow_Hide()
        subSetControlEnable(strReason)

    End Sub

    Protected Sub ddlRankID_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlRankID.SelectedIndexChanged
        subLoadTitleIDData()
    End Sub
    Private Sub subLoadTitleIDData()
        '職等
        If ddlRankID.SelectedIndex < 0 Then Return
        ddlTitleID.Items.Clear()

        Dim objHR As New HR()
        Try
            '2016/04/29 SPHBKC資料已併入Title中
            'If ddlNewCompID.SelectedValue = "SPHBKC" Then
            '    Using dt As Data.DataTable = objHR.GetCTitleInfo(ddlRankID.SelectedValue, "TitleID, TitleName, TitleID + '-' + TitleName as FullName", "And CompID=" & Bsp.Utility.Quote(ddlNewCompID.SelectedValue))
            '        With ddlTitleID
            '            .DataSource = dt
            '            .DataTextField = "FullName"
            '            .DataValueField = "TitleID"
            '            .DataBind()
            '            .Items.Insert(0, New ListItem("---請選擇---", ""))

            '        End With
            '    End Using
            'Else
            Using dt As Data.DataTable = objHR.GetTitleInfo(ddlRankID.SelectedValue, "TitleID, TitleName, TitleID + '-' + TitleName as FullName", "And CompID=" & Bsp.Utility.Quote(ddlNewCompID.SelectedValue))
                With ddlTitleID
                    .DataSource = dt
                    .DataTextField = "FullName"
                    .DataValueField = "TitleID"
                    .DataBind()
                    .Items.Insert(0, New ListItem("---請選擇---", ""))

                End With
            End Using
            'End If


        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Bsp.Utility.getInnerException("subLoadTitleIDData：", ex))
            Return
        End Try
    End Sub

#Region "WorkType"    '工作性質
    Public Function GetWorkTypeID(ByVal strWorkTypeID As String) As String
        Dim strWhere As String = ""
        Dim strWhereWorkType As String
        Dim aryValue() As String = strWorkTypeID.Split("|")
        Dim intCnt As Integer
        Dim strMainWorkType As String '主要工作性質
        strMainWorkType = ""

        '2016/05/03 SPHBKC資料已併入WorkTypeID中
        'If ddlNewCompID.SelectedValue = "SPHBKC" Then
        '    strWhere = "where TabName='EmpWorkType' and FldName='WorkTypeID' "
        'Else
        strWhere = "where CompID = " & Bsp.Utility.Quote(ddlNewCompID.SelectedValue)
        'End If
        strWhereWorkType = ""
        If aryValue.GetUpperBound(0) < 1 Then
            strWhereWorkType = Bsp.Utility.Quote(aryValue(0).ToString().Trim)
            strMainWorkType = aryValue(0).ToString().Trim
            intCnt = 1
        Else
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
        End If

        If intCnt > 0 Then
            '2016/05/03 SPHBKC資料已併入WorkTypeID中
            'If ddlNewCompID.SelectedValue = "SPHBKC" Then
            '    strWhere = strWhere & "And Code In (" & strWhereWorkType & ")"
            'Else
            strWhere = strWhere & "And WorkTypeID In (" & strWhereWorkType & ")"
            'End If
        End If
        lblSelectWorkType.Text = strWhereWorkType

        Dim objHR3000 As New HR3000

        Try
            '2016/05/03 SPHBKC資料已併入WorkTypeID中
            'If ddlNewCompID.SelectedValue = "SPHBKC" Then
            '    Using dt As Data.DataTable = objHR3000.GetCWorkTypeID(strWhere).Tables(0)
            '        With ddlWorkType
            '            .DataSource = dt
            '            .DataTextField = "FullWorkTypeName"
            '            .DataValueField = "WorkTypeID"
            '            .DataBind()
            '            .Items.Insert(0, New ListItem("------", ""))
            '        End With
            '    End Using
            'Else
            Using dt As Data.DataTable = objHR3000.GetWorkTypeID(strWhere).Tables(0)
                With ddlWorkType
                    .DataSource = dt
                    .DataTextField = "FullWorkTypeName"
                    .DataValueField = "WorkTypeID"
                    .DataBind()
                    .Items.Insert(0, New ListItem("------", ""))
                End With
            End Using
            'End If


            Return strMainWorkType

        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me.Page, Bsp.Utility.getInnerException("ddlWorkType", ex))
            Return strMainWorkType
        End Try
    End Function
#End Region
#Region "Position"    '職位
    Public Function GetPositionID(ByVal strPositionID As String) As String
        Dim strWhere As String
        Dim strWherePosition As String
        Dim aryValue() As String = strPositionID.Split("|")
        Dim intCnt As Integer
        Dim strMainPosition As String '主要職位
        strMainPosition = ""

        strWhere = "where CompID = " & Bsp.Utility.Quote(ddlNewCompID.SelectedValue)
        strWherePosition = ""
        If aryValue.GetUpperBound(0) < 1 Then
            strWherePosition = Bsp.Utility.Quote(aryValue(0).ToString().Trim)
            strMainPosition = aryValue(0).ToString().Trim
            intCnt = 1
        Else
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
        End If

        If intCnt > 0 Then
            strWhere = strWhere & "And PositionID In (" & strWherePosition & ")"
        End If
        lblSelectPosition.Text = strWherePosition

        Dim objHR3000 As New HR3000

        Try
            Using dt As Data.DataTable = objHR3000.GetPositionID(strWhere).Tables(0)
                With ddlPosition
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
    '#Region "最小簽核單位"
    '    Public Sub GetEmpFlowOrganID()
    '        Dim objHR3000 As New HR3000
    '        Dim strWhere As String
    '        strWhere = "where OrganID = '" & ucSelectHROrgan.SelectedOrganID & "'"
    '        Dim intCnt As Integer
    '        Using dt As Data.DataTable = objHR3000.GetFlowOrganID(ddlNewCompID.SelectedValue, strWhere).Tables(0)
    '            If dt.Rows.Count > 0 Then
    '                With ddlFlowOrganID
    '                    If dt.Rows.Item(0)("FlowOrganID").ToString.Trim = "" Then
    '                        .DataSource = dt
    '                        .DataTextField = "FullOrganName"
    '                        .DataValueField = "OrganID"
    '                        .DataBind()
    '                    Else
    '                        Dim aryValue() As String = dt.Rows(0)("FlowOrganID").ToString().Trim.Split("|")
    '                        For intCnt = 0 To UBound(aryValue)
    '                            If intCnt = 0 Then
    '                                strWhere = Bsp.Utility.Quote(aryValue(intCnt).ToString().Trim)
    '                            Else
    '                                strWhere = strWhere & "," & Bsp.Utility.Quote(aryValue(intCnt).ToString().Trim)
    '                            End If
    '                        Next
    '                        strWhere = "Where OrganID In (" & strWhere & ")"
    '                    End If
    '                End With
    '            End If

    '        End Using
    '        If intCnt > 0 Then
    '            Using dt As Data.DataTable = objHR3000.GetFlowOrganID(ddlNewCompID.SelectedValue, strWhere).Tables(0)
    '                With ddlFlowOrganID
    '                    .DataSource = dt
    '                    .DataTextField = "FullOrganName"
    '                    .DataValueField = "OrganID"
    '                    .DataBind()
    '                End With
    '            End Using
    '        End If
    '        ddlFlowOrganID.Items.Insert(0, New ListItem("---請選擇---", ""))
    '        UpdFlowOrgnaID.Update()
    '    End Sub
    '    Public Sub GetEmpAdditionFlowOrganID()
    '        Dim objHR3000 As New HR3000
    '        Dim strWhere As String
    '        strWhere = "where OrganID = '" & ucSelectEmpAdditionHROrgan.SelectedOrganID & "'"
    '        Dim intCnt As Integer = 0
    '        Using dt As Data.DataTable = objHR3000.GetFlowOrganID(ddlEmpAdditionCompID.SelectedValue, strWhere).Tables(0)
    '            If dt.Rows.Count > 0 Then
    '                With ddlEmpAdditonFlowOrganID
    '                    If dt.Rows.Item(0)("FlowOrganID").ToString.Trim = "" Then
    '                        .DataSource = dt
    '                        .DataTextField = "FullOrganName"
    '                        .DataValueField = "OrganID"
    '                        .DataBind()
    '                    Else
    '                        Dim aryValue() As String = dt.Rows(0)("FlowOrganID").ToString().Trim.Split("|")
    '                        For intCnt = 0 To UBound(aryValue)
    '                            If intCnt = 0 Then
    '                                strWhere = Bsp.Utility.Quote(aryValue(intCnt).ToString().Trim)
    '                            Else
    '                                strWhere = strWhere & "," & Bsp.Utility.Quote(aryValue(intCnt).ToString().Trim)
    '                            End If
    '                        Next
    '                        strWhere = "Where OrganID In (" & strWhere & ")"
    '                    End If
    '                End With
    '            End If
    '        End Using
    '        If intCnt > 0 Then
    '            Using dt As Data.DataTable = objHR3000.GetFlowOrganID(ddlEmpAdditionCompID.SelectedValue, strWhere).Tables(0)
    '                With ddlEmpAdditonFlowOrganID
    '                    .DataSource = dt
    '                    .DataTextField = "FullOrganName"
    '                    .DataValueField = "OrganID"
    '                    .DataBind()
    '                End With
    '            End Using
    '        End If
    '        ddlEmpAdditonFlowOrganID.Items.Insert(0, New ListItem("---請選擇---", ""))
    '        UpdEmpAdditionFlowOrgnaID.Update()
    '    End Sub
    '#End Region

#Region "EmpAdditionWait"
    Protected Sub btnAddEmpAddition_Click(sender As Object, e As System.EventArgs)
        Panel_EmpAddition.Visible = True
        btnEmpAdditionInsert.Visible = True
        btnEmpAdditionUpdate.Visible = False
        btnAddEmpAddition.Visible = False
        ViewState.Item("EmpAddition") = "Add"
        ddlEmpAdditionReason.Items.Clear()
        ddlEmpAdditionReason.Items.Insert(0, New ListItem("調兼", "1"))
    End Sub

    Protected Sub gvEmpAddition_RowEditing(sender As Object, e As System.Web.UI.WebControls.GridViewEditEventArgs)
        gvEmpAddition.EditIndex = e.NewEditIndex
        'GetDetailData();
        GetEmpAdditionWait()
    End Sub
    Protected Sub gvEmpAddition_RowDeleting(sender As Object, e As System.Web.UI.WebControls.GridViewDeleteEventArgs)
        '要寫delete code
        '==== 離開刪除模式 demo先不做====
        gvEmpAddition.EditIndex = -1
        'GetDetailData();
        'lblContectMsg.Text = "刪除模式 demo，先不做(無程式碼)";
    End Sub
    Protected Sub gvEmpAddition_RowUpdating(sender As Object, e As System.Web.UI.WebControls.GridViewUpdateEventArgs)
        '要寫update code
        '==== 離開編輯模式 ====
        gvEmpAddition.EditIndex = -1
        GetEmpAdditionWait()
    End Sub
    Protected Sub gvEmpAddition_RowCancelingEdit(sender As Object, e As System.Web.UI.WebControls.GridViewCancelEditEventArgs)
        gvEmpAddition.EditIndex = -1
        'GetDetailData()
        GetEmpAdditionWait()
    End Sub
    Protected Sub gvEmpAddition_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs)
        Dim Mode As String = String.Empty
        If Request("Mode") IsNot Nothing Then
            Mode = Request("Mode").ToString()
        Else
            Mode = "Edit"
        End If

        If Mode <> "Edit" Then
            If e.Row.RowType = DataControlRowType.DataRow Then
                e.Row.Cells(0).Visible = False
            ElseIf e.Row.RowType = DataControlRowType.Header Then
                e.Row.Cells(0).Visible = False
            End If
        End If

        '設定Seq,Reason,AddCompID,AddDeptID,AddOrganID,AddFlowOrganID,FileNo,Remark不顯示
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(1).Style.Add("display", "none")
            e.Row.Cells(4).Style.Add("display", "none")
            e.Row.Cells(6).Style.Add("display", "none")
            e.Row.Cells(8).Style.Add("display", "none")
            e.Row.Cells(10).Style.Add("display", "none")
            e.Row.Cells(12).Style.Add("display", "none")
            e.Row.Cells(14).Style.Add("display", "none")
            e.Row.Cells(16).Style.Add("display", "none")
            e.Row.Cells(18).Style.Add("display", "none")
            e.Row.Cells(20).Style.Add("display", "none")
            e.Row.Cells(22).Style.Add("display", "none")
            e.Row.Cells(24).Style.Add("display", "none")
            e.Row.Cells(25).Style.Add("display", "none")
            e.Row.Cells(26).Style.Add("display", "none")
            Dim tmpBtn As New ImageButton
            Dim tmpBtnE As New ImageButton
            tmpBtn = e.Row.Cells(0).FindControl("ibnDelete")
            tmpBtnE = e.Row.Cells(0).FindControl("ibnUpdate")
            If DirectCast(gvEmpAddition.DataSource, DataTable).Rows(e.Row.RowIndex)("ExistsEmpAddition").ToString() = "1" Then
                tmpBtn.Visible = False
            End If
            If ViewState.Item("Detail") = "Y" Then
                tmpBtnE.Visible = False
                tmpBtn.Visible = False
            End If
        ElseIf e.Row.RowType = DataControlRowType.Header Then
            e.Row.Cells(1).Style.Add("display", "none")
            e.Row.Cells(4).Style.Add("display", "none")
            e.Row.Cells(6).Style.Add("display", "none")
            e.Row.Cells(8).Style.Add("display", "none")
            e.Row.Cells(10).Style.Add("display", "none")
            e.Row.Cells(12).Style.Add("display", "none")
            e.Row.Cells(14).Style.Add("display", "none")
            e.Row.Cells(16).Style.Add("display", "none")
            e.Row.Cells(18).Style.Add("display", "none")
            e.Row.Cells(20).Style.Add("display", "none")
            e.Row.Cells(22).Style.Add("display", "none")
            e.Row.Cells(24).Style.Add("display", "none")
            e.Row.Cells(25).Style.Add("display", "none")
            e.Row.Cells(26).Style.Add("display", "none")
        End If
    End Sub
    Protected Sub gvEmpAddition_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        Dim ibn As ImageButton = DirectCast(e.CommandSource, ImageButton)
        Dim gvr As GridViewRow = DirectCast(ibn.NamingContainer, GridViewRow)

        Select Case e.CommandName
            Case "Update"
                gvEmpAddition_Update(gvr)
            Case "Delete"
                gvEmpAddition_Delete(gvr)
        End Select
    End Sub

    Private Sub gvEmpAddition_Update(ByVal gvr As GridViewRow)
        Dim AddSeq As String = ""
        Dim AddReason As String = ""
        Dim AddCompID As String = ""
        Dim AddDeptID As String = ""
        Dim AddOrganID As String = ""
        Dim AddFlowOrganID As String = ""
        Dim BossType As String = ""
        Dim IsBoss As Boolean = False
        Dim IsSecBoss As Boolean = False
        Dim IsGroupBoss As Boolean = False
        Dim IsSecGroupBoss As Boolean = False
        Dim FileNo As String = ""
        Dim AddRemark As String = ""


        AddSeq = gvr.Cells(1).Text.Replace("&nbsp;", "").Trim()
        AddReason = gvr.Cells(4).Text.Replace("&nbsp;", "").Trim()
        AddCompID = gvr.Cells(6).Text.Replace("&nbsp;", "").Trim()
        AddDeptID = gvr.Cells(8).Text.Replace("&nbsp;", "").Trim()
        AddOrganID = gvr.Cells(10).Text.Replace("&nbsp;", "").Trim()
        AddFlowOrganID = gvr.Cells(12).Text.Replace("&nbsp;", "").Trim()
        BossType = gvr.Cells(14).Text.Replace("&nbsp;", "").Trim()
        If gvr.Cells(16).Text.Replace("&nbsp;", "").Trim() = "1" Then
            IsBoss = True
        End If
        If gvr.Cells(18).Text.Replace("&nbsp;", "").Trim() = "1" Then
            IsSecBoss = True
        End If
        If gvr.Cells(20).Text.Replace("&nbsp;", "").Trim() = "1" Then
            IsGroupBoss = True
        End If
        If gvr.Cells(22).Text.Replace("&nbsp;", "").Trim() = "1" Then
            IsSecGroupBoss = True
        End If
        FileNo = gvr.Cells(23).Text.Replace("&nbsp;", "").Trim()
        AddRemark = gvr.Cells(24).Text.Replace("&nbsp;", "").Trim()

        hdfEmpAdditionAddSeqNo.Value = AddSeq
        hdfEmpAdditionSeqNo.Value = gvr.Cells(26).Text.Replace("&nbsp;", "").Trim()

        '兼任狀態
        If gvr.Cells(25).Text.Replace("&nbsp;", "").Trim() = "1" Then
            HR3000.FillEmpAdditionReason(ddlEmpAdditionReason)
        Else
            ddlEmpAdditionReason.Items.Clear()
            ddlEmpAdditionReason.Items.Insert(0, New ListItem("調兼", "1"))
        End If

        Bsp.Utility.SetSelectedIndex(ddlEmpAdditionReason, AddReason)
        Bsp.Utility.SetSelectedIndex(ddlEmpAdditionCompID, AddCompID)
        ucSelectEmpAdditionHROrgan.LoadData(AddCompID)
        ucSelectEmpAdditionHROrgan.setDeptID(AddCompID, AddDeptID, "N")
        ucSelectEmpAdditionHROrgan.setOrganID(AddCompID, AddOrganID, "N")
        ''最小簽核單位
        ucSelectAddFlowOrgan.LoadData(AddFlowOrganID)
        'GetEmpAdditionFlowOrganID()
        'Bsp.Utility.SetSelectedIndex(ddlEmpAdditonFlowOrganID, AddFlowOrganID)
        Bsp.Utility.SetSelectedIndex(ddlEmpAdditionBossType, BossType)
        chkEmpAdditionIsBoss.Checked = IsBoss
        chkEmpAdditionIsSecBoss.Checked = IsSecBoss
        chkEmpAdditionIsGroupBoss.Checked = IsGroupBoss
        chkEmpAdditionIsSecGroupBoss.Checked = IsSecGroupBoss

        txtFileNO.Text = FileNo
        txtEmpAdditionRemark.Text = AddRemark

        Panel_EmpAddition.Visible = True
        btnEmpAdditionInsert.Visible = False
        btnEmpAdditionUpdate.Visible = True
        If ViewState("Detail") = "Y" Then
            btnEmpAdditionUpdate.Visible = False
        End If
        btnAddEmpAddition.Visible = False
        ViewState.Item("EmpAddition") = "" = "Update"
    End Sub
    Private Sub gvEmpAddition_Delete(ByVal gvr As GridViewRow)
        Dim strSQL As New StringBuilder()
        Dim AddSeq As String = ""
        AddSeq = gvr.Cells(1).Text.Replace("&nbsp;", "")

        '執行delete資料的動作

        strSQL.AppendLine("Delete From Tmp_EmpAdditionWait")
        strSQL.AppendLine("Where CompID = " & Bsp.Utility.Quote(hidCompID.Value))
        strSQL.AppendLine("And EmpID = " & Bsp.Utility.Quote(lblEmpID_O.Text))
        strSQL.AppendLine("And Seq = " & hdfEmpAdditionSeqNo.Value)
        strSQL.AppendLine("And AddSeq = " & AddSeq)
        Bsp.DB.ExecuteNonQuery(CommandType.Text, strSQL.ToString(), "eHRMSDB")

        '重新refresh資料
        GetEmpAdditionWait()

    End Sub
    Protected Sub btnEmpAdditionInsert_Click(sender As Object, e As System.EventArgs)
        Dim strSQL As New StringBuilder()
        Dim strValue As String = ""
        Dim objHR As New HR

        'insert一筆新資料
        Dim AddSeqNo As String = hdfEmpAdditionMaxAddSeqNo.Value
        If AddSeqNo = "" Then
            AddSeqNo = "0"
        End If
        AddSeqNo = (Int32.Parse(AddSeqNo) + 1).ToString()

        If funCheckEmpAdditionWaitData() Then

            '檢查資料是否存在
            strSQL.AppendLine("And EmpID = " & Bsp.Utility.Quote(lblEmpID_O.Text))
            strSQL.AppendLine("And Seq = " & hdfEmpAdditionSeqNo.Value)
            strSQL.AppendLine("And AddCompID = " & Bsp.Utility.Quote(ddlEmpAdditionCompID.SelectedValue))
            strSQL.AppendLine("And AddDeptID = " & Bsp.Utility.Quote(ucSelectEmpAdditionHROrgan.SelectedDeptID))
            strSQL.AppendLine("And AddOrganID = " & Bsp.Utility.Quote(ucSelectEmpAdditionHROrgan.SelectedOrganID))
            If Not objHR.IsDataExists("Tmp_EmpAdditionWait", strSQL.ToString()) Then
                strSQL.Length = 0
                strSQL.AppendLine("Insert Into Tmp_EmpAdditionWait (CompID,EmpID,Seq,AddSeq,AddCompID,AddDeptID,AddOrganID,AddFlowOrganID,Reason," & _
                                                            "FileNo,Remark,IsBoss,IsSecBoss,IsGroupBoss,IsSecGroupBoss,BossType,ValidMark,ExistsEmpAddition)")
                strSQL.AppendLine("Values (" & Bsp.Utility.Quote(hidCompID.Value))
                strSQL.AppendLine("," & Bsp.Utility.Quote(lblEmpID_O.Text))
                strSQL.AppendLine("," & hdfEmpAdditionSeqNo.Value)
                strSQL.AppendLine("," & AddSeqNo)
                strSQL.AppendLine("," & Bsp.Utility.Quote(ddlEmpAdditionCompID.SelectedValue))
                strSQL.AppendLine("," & Bsp.Utility.Quote(ucSelectEmpAdditionHROrgan.SelectedDeptID))
                strSQL.AppendLine("," & Bsp.Utility.Quote(ucSelectEmpAdditionHROrgan.SelectedOrganID))
                strSQL.AppendLine("," & Bsp.Utility.Quote(ucSelectAddFlowOrgan.SelectedOrganID))
                strSQL.AppendLine("," & Bsp.Utility.Quote(ddlEmpAdditionReason.SelectedValue))
                strSQL.AppendLine("," & Bsp.Utility.Quote(txtFileNO.Text.Trim()))
                strSQL.AppendLine("," & Bsp.Utility.Quote(txtEmpAdditionRemark.Text.Trim()))
                If chkEmpAdditionIsBoss.Checked Then
                    strSQL.AppendLine(",'1'")
                Else
                    strSQL.AppendLine(",'0'")
                End If
                If chkEmpAdditionIsSecBoss.Checked Then
                    strSQL.AppendLine(",'1'")
                Else
                    strSQL.AppendLine(",'0'")
                End If
                If chkEmpAdditionIsGroupBoss.Checked Then
                    strSQL.AppendLine(",'1'")
                Else
                    strSQL.AppendLine(",'0'")
                End If
                If chkEmpAdditionIsSecGroupBoss.Checked Then
                    strSQL.AppendLine(",'1'")
                Else
                    strSQL.AppendLine(",'0'")
                End If
                strValue = ddlEmpAdditionBossType.SelectedValue
                strSQL.AppendLine("," & Bsp.Utility.Quote(strValue))
                strSQL.AppendLine(",'0','0')")

                Bsp.DB.ExecuteNonQuery(CommandType.Text, strSQL.ToString(), "eHRMSDB")
            Else
                Bsp.Utility.ShowFormatMessage(Me, "W_00010", "員工待異動調兼紀錄")
                Return
            End If


            '重新refresh資料
            GetEmpAdditionWait()

            Panel_EmpAddition.Visible = False
            btnAddEmpAddition.Visible = True


            hdfEmpAdditionMaxAddSeqNo.Value = AddSeqNo

            ddlEmpAdditionBossType.SelectedValue = ""
            chkEmpAdditionIsBoss.Checked = False
            chkEmpAdditionIsSecBoss.Checked = False
            chkEmpAdditionIsGroupBoss.Checked = False
            chkEmpAdditionIsSecGroupBoss.Checked = False
            txtFileNO.Text = ""
            txtEmpAdditionRemark.Text = ""

            ViewState.Item("EmpAddition") = ""
        End If

    End Sub
    Protected Sub btnEmpAdditionUpdate_Click(sender As Object, e As System.EventArgs)
        Dim strSQL As New StringBuilder()
        Dim strValue As String = ""
        Dim objHR As New HR

        '執行update資料的動作

        If funCheckEmpAdditionWaitData() Then

            strSQL.Length = 0
            strSQL.AppendLine("Update Tmp_EmpAdditionWait Set")
            strSQL.AppendLine("AddCompID=" & Bsp.Utility.Quote(ddlEmpAdditionCompID.SelectedValue))
            strSQL.AppendLine(",AddDeptID=" & Bsp.Utility.Quote(ucSelectEmpAdditionHROrgan.SelectedDeptID))
            strSQL.AppendLine(",AddOrganID=" & Bsp.Utility.Quote(ucSelectEmpAdditionHROrgan.SelectedOrganID))
            strSQL.AppendLine(",AddFlowOrganID=" & Bsp.Utility.Quote(ucSelectAddFlowOrgan.SelectedOrganID))
            strSQL.AppendLine(",Reason=" & Bsp.Utility.Quote(ddlEmpAdditionReason.SelectedValue))
            strSQL.AppendLine(",FileNo=" & Bsp.Utility.Quote(txtFileNO.Text.Trim()))
            strSQL.AppendLine(",Remark=" & Bsp.Utility.Quote(txtEmpAdditionRemark.Text.Trim()))
            If chkEmpAdditionIsBoss.Checked Then
                strSQL.AppendLine(",IsBoss='1'")
            Else
                strSQL.AppendLine(",IsBoss='0'")
            End If
            If chkEmpAdditionIsSecBoss.Checked Then
                strSQL.AppendLine(",IsSecBoss='1'")
            Else
                strSQL.AppendLine(",IsSecBoss='0'")
            End If
            If chkEmpAdditionIsGroupBoss.Checked Then
                strSQL.AppendLine(",IsGroupBoss='1'")
            Else
                strSQL.AppendLine(",IsGroupBoss='0'")
            End If
            If chkEmpAdditionIsSecGroupBoss.Checked Then
                strSQL.AppendLine(",IsSecGroupBoss='1'")
            Else
                strSQL.AppendLine(",IsSecGroupBoss='0'")
            End If
            strValue = ddlEmpAdditionBossType.SelectedValue
            strSQL.AppendLine(",BossType=" & Bsp.Utility.Quote(strValue))
            strSQL.AppendLine("Where CompID = " & Bsp.Utility.Quote(hidCompID.Value))
            strSQL.AppendLine("And EmpID = " & Bsp.Utility.Quote(lblEmpID_O.Text))
            strSQL.AppendLine("And Seq = " & hdfEmpAdditionSeqNo.Value)
            strSQL.AppendLine("And AddSeq = " & hdfEmpAdditionAddSeqNo.Value)
            Bsp.DB.ExecuteNonQuery(CommandType.Text, strSQL.ToString(), "eHRMSDB")

            '重新refresh資料
            GetEmpAdditionWait()

            Panel_EmpAddition.Visible = False
            btnAddEmpAddition.Visible = True

            ddlEmpAdditionBossType.SelectedValue = ""
            chkEmpAdditionIsBoss.Checked = False
            chkEmpAdditionIsSecBoss.Checked = False
            chkEmpAdditionIsGroupBoss.Checked = False
            chkEmpAdditionIsSecGroupBoss.Checked = False
            txtFileNO.Text = ""
            txtEmpAdditionRemark.Text = ""

            ViewState.Item("EmpAddition") = ""
        End If
    End Sub

    Protected Sub btnEmpAdditionCancel_Click(sender As Object, e As System.EventArgs)
        Panel_EmpAddition.Visible = False
        btnEmpAdditionInsert.Visible = True
        btnEmpAdditionUpdate.Visible = False
        btnAddEmpAddition.Visible = True

        ddlEmpAdditionBossType.SelectedValue = ""
        chkEmpAdditionIsBoss.Checked = False
        chkEmpAdditionIsSecBoss.Checked = False
        chkEmpAdditionIsGroupBoss.Checked = False
        chkEmpAdditionIsSecGroupBoss.Checked = False
        txtFileNO.Text = ""
        txtEmpAdditionRemark.Text = ""
        ViewState.Item("EmpAddition") = ""
    End Sub
    Private Function funCheckEmpAdditionWaitData() As Boolean
        Dim strWhere As String = ""
        Dim objHR As New HR
        '狀態
        If ddlEmpAdditionReason.SelectedValue = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", "兼任狀態")
            ddlEmpAdditionReason.Focus()
            Return False
        End If

        '人令
        If txtFileNO.Text.Trim() = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", "人令")
            txtFileNO.Focus()
            Return False
        Else
            If Bsp.Utility.getStringLength(txtFileNO.Text.Trim()) > txtFileNO.MaxLength Then
                Bsp.Utility.ShowFormatMessage(Me, "W_00040", lblFileNO.Text, txtFileNO.MaxLength)
                txtFileNO.Focus()
                Return False
            End If
        End If

        '兼任公司
        If ddlEmpAdditionCompID.SelectedValue = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", "兼任公司")
            ddlEmpAdditionCompID.Focus()
            Return False
        End If

        '兼任部門
        If ucSelectEmpAdditionHROrgan.SelectedDeptID = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", "兼任部門")
            ucSelectEmpAdditionHROrgan.Focus()
            Return False
        End If

        '兼任科組課
        If ucSelectEmpAdditionHROrgan.SelectedOrganID = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", "兼任科組課")
            ucSelectEmpAdditionHROrgan.Focus()
            Return False
        End If

        '最小簽核單位    
        If ucSelectAddFlowOrgan.SelectedOrganID = "" And chkEmpAdditionIsGroupBoss.Checked Then   '20150722 wei modify
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblEmpAdditionFlowOrganID.Text)
            ucSelectAddFlowOrgan.Focus()
            Return False
        End If

        '主管
        If ddlEmpAdditionBossType.SelectedValue <> "" Then
            If Not chkEmpAdditionIsBoss.Checked And Not chkEmpAdditionIsGroupBoss.Checked Then '20150605 wei modify And Not chkEmpAdditionIsSecBoss.Checked And Not chkEmpAdditionIsSecGroupBoss.Checked 
                Bsp.Utility.ShowFormatMessage(Me, "H_00000", "已選擇主管任用方式，請至少選擇一個單位(副)主或簽核單位(副)主管")
                chkEmpAdditionIsBoss.Focus()
                Return False
            End If
            '檢查是否有同單位的主管註記
            If chkEmpAdditionIsBoss.Checked Then
                strWhere = " And NewCompID = " & Bsp.Utility.Quote(ddlEmpAdditionCompID.SelectedValue) & " And OrganID = " & Bsp.Utility.Quote(ucSelectEmpAdditionHROrgan.SelectedOrganID) & " And EmpID<>" & Bsp.Utility.Quote(hidEmpID.Value)
                strWhere &= " And ValidMark='0' And IsBoss='1' And CompID=NewCompID"
                strWhere &= " And Reason in (Select Reason From WorkStatus_EmployeeReason where AfterWorkStatusType in ('1')) " '20160104 wei add 增加判斷異動後需為在職的原因
                If objHR.IsDataExists("EmployeeWait", strWhere) Then
                    Bsp.Utility.ShowMessage(Me, "待生效資料已有相同單位的單位主管註記！")
                    chkIsBoss.Focus()
                    Return False
                End If
                strWhere = " And AddCompID = " & Bsp.Utility.Quote(ddlEmpAdditionCompID.SelectedValue) & " And AddOrganID = " & Bsp.Utility.Quote(ucSelectEmpAdditionHROrgan.SelectedOrganID) & " And EmpID<>" & Bsp.Utility.Quote(hidEmpID.Value)
                strWhere &= " And ValidMark='0' And IsBoss='1' "
                strWhere &= " And Reason in ('1') " '20160104 wei add 增加判斷異動後為調兼的原因
                If objHR.IsDataExists("EmpAdditionWait", strWhere) Then
                    Bsp.Utility.ShowMessage(Me, "待生效資料已有相同單位的單位主管註記！")
                    chkIsBoss.Focus()
                    Return False
                End If
            End If
            If chkEmpAdditionIsGroupBoss.Checked Then
                strWhere = " And NewCompID = " & Bsp.Utility.Quote(ddlEmpAdditionCompID.SelectedValue) & " And FlowOrganID = " & Bsp.Utility.Quote(ucSelectAddFlowOrgan.SelectedOrganID) & " And EmpID<>" & Bsp.Utility.Quote(hidEmpID.Value)
                strWhere &= " And ValidMark='0' And IsGroupBoss='1' And CompID=NewCompID"
                strWhere &= " And Reason in (Select Reason From WorkStatus_EmployeeReason where AfterWorkStatusType in ('1')) " '20160104 wei add 增加判斷異動後需為在職的原因
                If objHR.IsDataExists("EmployeeWait", strWhere) Then
                    Bsp.Utility.ShowMessage(Me, "待生效資料已有相同最小簽核單位的單位主管註記！")
                    chkIsGroupBoss.Focus()
                    Return False
                End If
                strWhere = " And AddCompID = " & Bsp.Utility.Quote(ddlEmpAdditionCompID.SelectedValue) & " And AddFlowOrganID = " & Bsp.Utility.Quote(ucSelectAddFlowOrgan.SelectedOrganID) & " And EmpID<>" & Bsp.Utility.Quote(hidEmpID.Value)
                strWhere &= " And ValidMark='0' And IsGroupBoss='1' "
                strWhere &= " And Reason in ('1') " '20160104 wei add 增加判斷異動後為調兼的原因
                If objHR.IsDataExists("EmpAdditionWait", strWhere) Then
                    Bsp.Utility.ShowMessage(Me, "待生效資料已有相同最小簽核單位的單位主管註記！")
                    chkIsGroupBoss.Focus()
                    Return False
                End If
            End If
        Else
            If chkEmpAdditionIsBoss.Checked Or chkEmpAdditionIsGroupBoss.Checked Then   '20150605 wei modify Or chkEmpAdditionIsSecBoss.Checked Or chkEmpAdditionIsSecGroupBoss.Checked 
                Bsp.Utility.ShowFormatMessage(Me, "H_00000", "已選擇單位主管或簽核單位主管，請選擇主管任用方式")
                ddlEmpAdditionBossType.Focus()
                Return False
            End If
        End If

        '備註
        If Bsp.Utility.getStringLength(txtEmpAdditionRemark.Text.Trim()) > txtEmpAdditionRemark.MaxLength Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00040", lblEmpAdditionRemark.Text, txtEmpAdditionRemark.MaxLength)
            txtEmpAdditionRemark.Focus()
            Return False
        End If

        Return True
    End Function
    Private Sub EditMode()
        btnAddEmpAddition.Visible = True
    End Sub
    Private Sub ReadMode()
        btnAddEmpAddition.Visible = False
    End Sub

#End Region
#Region "處理調兼資料"
    '現況調兼資料複製至待調兼資料暫存檔Tmp_EmpAdditionWait
    Private Sub EmpAddition_CopyTo_Tmp_EmpAdditionWait()
        Dim objHR3000 As New HR3000
        Dim intSeq As Integer = 0
        Dim strSQL As New StringBuilder()


        hdfEmpAdditionSeqNo.Value = hidSeq.Value

        strSQL.AppendLine("Delete From Tmp_EmpAdditionWait")
        strSQL.AppendLine("Where CompID=" & Bsp.Utility.Quote(hidCompID.Value))
        strSQL.AppendLine("And EmpID = " & Bsp.Utility.Quote(lblEmpID_O.Text) & ";")

        strSQL.AppendLine("Insert Into Tmp_EmpAdditionWait (CompID,EmpID,Seq,AddSeq,AddCompID,AddDeptID,AddOrganID,AddFlowOrganID,Reason," & _
                                                            "FileNo,Remark,IsBoss,IsSecBoss,IsGroupBoss,IsSecGroupBoss,BossType,ValidMark,ExistsEmpAddition)")
        strSQL.AppendLine("Select CompID,EmpID,Seq,AddSeq")
        strSQL.AppendLine(",AddCompID,AddDeptID,AddOrganID,AddFlowOrganID,Reason")
        strSQL.AppendLine(",FileNo,Remark,IsBoss,IsSecBoss,IsGroupBoss,IsSecGroupBoss,BossType")
        strSQL.AppendLine(",ValidMark,ExistsEmpAddition")
        strSQL.AppendLine("From EmpAdditionWait")
        strSQL.AppendLine("Where CompID = " & Bsp.Utility.Quote(hidCompID.Value))
        strSQL.AppendLine("And EmpID = " & Bsp.Utility.Quote(lblEmpID_O.Text))
        strSQL.AppendLine("And ValidDate = " & Bsp.Utility.Quote(hidValidDate.Value))
        strSQL.AppendLine("And Seq = " & Bsp.Utility.Quote(hidSeq.Value))

        strSQL.AppendLine("Order by ValidDate")

        Bsp.DB.ExecuteNonQuery(CommandType.Text, strSQL.ToString(), "eHRMSDB")

    End Sub
    '待調兼資料-暫存檔Tmp_EmpAdditionWait
    Private Sub GetEmpAdditionWait()
        Dim strSQL As New StringBuilder()

        strSQL.AppendLine("Select A.Seq ,A.AddSeq")
        strSQL.AppendLine(",isnull(M.CodeCName,'') ReasonName,A.Reason")
        strSQL.AppendLine(",isnull(C1.CompName,'') AddCompName,A.AddCompID")
        'strSQL.AppendLine(",Case When A.AddCompID='SPHBKC' Then isnull(O3.OrganName,'') Else isnull(O1.OrganName,'') End COLLATE Chinese_Taiwan_Stroke_CS_AS as AddDeptName,A.AddDeptID")   '20150325 wei modify
        'strSQL.AppendLine(",Case When A.AddCompID='SPHBKC' Then isnull(O4.OrganName,'') Else isnull(O2.OrganName,'') End COLLATE Chinese_Taiwan_Stroke_CS_AS as AddOrganName,A.AddOrganID") '20150325 wei modify
        'strSQL.AppendLine(",Case When A.AddCompID='SPHBKC' Then isnull(F1.OrganName,'') Else isnull(F.OrganName,'') End COLLATE Chinese_Taiwan_Stroke_CS_AS as AddFlowOrganName,A.AddFlowOrganID")  '20150325 wei modify
        strSQL.AppendLine(",isnull(O1.OrganName,'') COLLATE Chinese_Taiwan_Stroke_CS_AS as AddDeptName,A.AddDeptID")            '2016/05/03 SPHBKC資料已併入Organization中
        strSQL.AppendLine(",isnull(O2.OrganName,'') COLLATE Chinese_Taiwan_Stroke_CS_AS as AddOrganName,A.AddOrganID")          '2016/05/03 SPHBKC資料已併入Organization中
        strSQL.AppendLine(",isnull(F.OrganName,'') COLLATE Chinese_Taiwan_Stroke_CS_AS as AddFlowOrganName,A.AddFlowOrganID")   '2016/05/03 SPHBKC資料已併入OrganizationFlow中
        strSQL.AppendLine(",Case When A.BossType='1' Then '主要' When A.BossType='2' Then '兼任' Else '' End as BossTypeName,A.BossType")
        strSQL.AppendLine(",Convert(bit,Case When A.IsBoss <>'1' Then '0' Else A.IsBoss End) as IsBossValue,A.IsBoss")
        strSQL.AppendLine(",Convert(bit,Case When A.IsSecBoss <>'1' Then '0' Else A.IsSecBoss End) as IsSecBossValue,A.IsSecBoss")
        strSQL.AppendLine(",Convert(bit,Case When A.IsGroupBoss <>'1' Then '0' Else A.IsGroupBoss End) as IsGroupBossValue,A.IsGroupBoss")
        strSQL.AppendLine(",Convert(bit,Case When A.IsSecGroupBoss <>'1' Then '0' Else A.IsSecGroupBoss End) as IsSecGroupBossValue,A.IsSecGroupBoss")
        strSQL.AppendLine(",A.FileNo")
        strSQL.AppendLine(",A.Remark")
        strSQL.AppendLine(",A.ExistsEmpAddition")
        strSQL.AppendLine(",Row_number()OVER(ORDER BY A.AddSeq) as ShowAddSeqNo")
        strSQL.AppendLine("From Tmp_EmpAdditionWait A")
        strSQL.AppendLine("LEFT JOIN HRCodeMap M ON A.Reason = M.Code and M.TabName = 'EmpAddition' and M.FldName = 'Reason'")
        strSQL.AppendLine("LEFT JOIN Company C1 ON C1.CompID = A.AddCompID")
        strSQL.AppendLine("LEFT JOIN Organization O1 ON O1.CompID = A.AddCompID and O1.OrganID = A.AddDeptID and O1.OrganID = O1.DeptID")
        strSQL.AppendLine("LEFT JOIN Organization O2 ON O2.CompID = A.AddCompID and O2.OrganID = A.AddOrganID")
        'strSQL.AppendLine("LEFT JOIN COrganization O3 ON O3.CompID = A.AddCompID COLLATE Chinese_Taiwan_Stroke_CS_AS and O3.OrganID = A.AddDeptID COLLATE Chinese_Taiwan_Stroke_CS_AS and O3.OrganID = O3.DeptID") '20150325 wei add '2016/05/03 SPHBKC資料已併入Organization中
        'strSQL.AppendLine("LEFT JOIN COrganization O4 ON O4.CompID = A.AddCompID COLLATE Chinese_Taiwan_Stroke_CS_AS and O4.OrganID = A.AddOrganID COLLATE Chinese_Taiwan_Stroke_CS_AS ") '20150325 wei add '2016/05/03 SPHBKC資料已併入Organization中
        strSQL.AppendLine("LEFT JOIN OrganizationFlow F ON F.OrganID = A.AddFlowOrganID")
        'strSQL.AppendLine("LEFT JOIN COrganizationFlow F1 ON F1.OrganID = A.AddFlowOrganID COLLATE Chinese_Taiwan_Stroke_CS_AS ") '20150325 wei add '2016/05/03 SPHBKC資料已併入OrganizationFlow中
        strSQL.AppendLine("Where A.CompID = " & Bsp.Utility.Quote(hidCompID.Value))
        strSQL.AppendLine("And A.EmpID = " & Bsp.Utility.Quote(lblEmpID_O.Text))
        strSQL.AppendLine("And Seq = " & Bsp.Utility.Quote(hidSeq.Value))

        strSQL.AppendLine("Order By A.AddSeq")

        Using dt As DataTable = Bsp.DB.ExecuteDataSet(CommandType.Text, strSQL.ToString(), "eHRMSDB").Tables(0)
            gvEmpAddition.DataSource = dt
            gvEmpAddition.DataBind()
        End Using

        '取得目前最大序號
        strSQL.Length = 0
        strSQL.AppendLine("Select Max(AddSeq) as MaxAddSeq")
        strSQL.AppendLine("From Tmp_EmpAdditionWait")
        strSQL.AppendLine("Where CompID = " & Bsp.Utility.Quote(hidCompID.Value))
        strSQL.AppendLine("And EmpID = " & Bsp.Utility.Quote(lblEmpID_O.Text))
        strSQL.AppendLine("And Seq = " & Bsp.Utility.Quote(hidSeq.Value))
        Using dt As DataTable = Bsp.DB.ExecuteDataSet(CommandType.Text, strSQL.ToString(), "eHRMSDB").Tables(0)
            hdfEmpAdditionMaxAddSeqNo.Value = dt.Rows(0)("MaxAddSeq").ToString()
        End Using

    End Sub
#End Region

    Private Sub ClearData()
        'EmployeeWait
        ucValidDate.DateText = ""
        ucDueDate.DateText = ""
        ddlReason.SelectedIndex = -1
        lblNotice.Text = ""
        ddlQuitReason.SelectedIndex = -1
        ddlNewCompID.SelectedValue = ViewState.Item("CompID") 'ViewState.Item("NewCompID")   '20150623 wei modify
        ddlNewCompID.Enabled = False
        ViewState.Item("WorkStatus") = ""  '20150713 wei 任職狀況
        '部門、科組課
        ucSelectHROrgan.LoadData(ViewState.Item("CompID"))   'ViewState.Item("NewCompID")  '20150623 wei modify
        ucSelectFlowOrgan.LoadData(ucSelectHROrgan.SelectedOrganID)
        'ddlFlowOrganID.Items.Clear()
        'ddlFlowOrganID.Items.Insert(0, New ListItem("---請選擇---", ""))
        chkSalaryPaid.Checked = False
        ddlBossType.SelectedValue = ""
        chkIsBoss.Checked = False
        chkIsSecBoss.Checked = False
        chkIsGroupBoss.Checked = False
        chkIsSecGroupBoss.Checked = False
        '職等           
        Bsp.Utility.Rank(ddlRankID, ddlNewCompID.SelectedItem.Value)
        ddlRankID.Items.Insert(0, New ListItem("---請選擇---", ""))
        '職稱
        ddlTitleID.Items.Clear()
        ddlTitleID.Items.Insert(0, "---請先選擇職等---")
        '工作地點
        Bsp.Utility.WorkSite(ddlWorkSiteID, ddlNewCompID.SelectedItem.Value, "")
        ddlWorkSiteID.Items.Insert(0, New ListItem("---請選擇---", ""))
        '工作性質
        ddlWorkType.Items.Clear()
        ddlWorkType.Items.Insert(0, New ListItem("---請選擇---", ""))
        '職位
        ddlPosition.Items.Clear()
        ddlPosition.Items.Insert(0, New ListItem("---請選擇---", ""))
        '班別
        ddlWTID.SelectedIndex = -1
        CheckShow_Hide()
        '備註
        txtRemark.Text = ""

        'EmpAdditionWait
        ddlEmpAdditionReason.SelectedIndex = -1
        txtFileNO.Text = ""
        ddlEmpAdditionCompID.SelectedValue = ViewState.Item("CompID")    'ViewState.Item("NewCompID")  '20150623 wei modify
        '部門、科組課
        ucSelectEmpAdditionHROrgan.LoadData(ViewState.Item("CompID"))   'ViewState.Item("NewCompID")  '20150623 wei modify
        ucSelectAddFlowOrgan.LoadData(ucSelectEmpAdditionHROrgan.SelectedOrganID, "Y")
        'ddlEmpAdditonFlowOrganID.Items.Clear()
        'ddlEmpAdditonFlowOrganID.Items.Insert(0, New ListItem("---請選擇---", ""))
        ddlEmpAdditionBossType.SelectedValue = ""
        chkEmpAdditionIsBoss.Checked = False
        chkEmpAdditionIsSecBoss.Checked = False
        chkEmpAdditionIsGroupBoss.Checked = False
        chkEmpAdditionIsSecGroupBoss.Checked = False
        txtEmpAdditionRemark.Text = ""
        '兼任資料
        GetEmpAdditionWait()
        Panel_EmpAddition.Visible = False
        btnEmpAdditionInsert.Visible = True
        btnEmpAdditionUpdate.Visible = False
        btnAddEmpAddition.Visible = True
        ViewState.Item("EmpAddition") = ""

        subGetData(hidCompID.Value, hidEmpID.Value, hidValidDate.Value, hidSeq.Value)
    End Sub
    '導向已生效企業團經歷待異動畫面
    Protected Sub btnEmployeeLogWait_Click(sender As Object, e As System.EventArgs) Handles btnEmployeeLogWait.Click
        GoEmployeeLogWait()
    End Sub
    Protected Sub btnEmployeeLogWait1_Click(sender As Object, e As System.EventArgs) Handles btnEmployeeLogWait1.Click
        GoEmployeeLogWait()
    End Sub
    '隱藏的按鈕--導回查詢清單頁，並帶入查詢條件-公司代碼，員工編號，狀態-未生效
    Protected Sub btnBackQuery_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBackQuery.Click
        GoBackQuery()
    End Sub
    '隱藏的按鈕--GoBack
    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
        GoBack()
    End Sub
    '隱藏的按鈕--Execute
    Protected Sub btnExecute_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExecute.Click
        Release("btnExecute")
    End Sub

    Private Sub subSetControlEnable(ByVal strReason As String)
        Select Case strReason
            Case "34"
                ddlQuitReason.Enabled = True
                ucSelectHROrgan.Enabled = False
                ucSelectFlowOrgan.Enabled = False
                'ddlFlowOrganID.Enabled = False
                chkSalaryPaid.Enabled = False
                ddlBossType.Enabled = False
                chkIsBoss.Enabled = False
                chkIsSecBoss.Enabled = False
                chkIsGroupBoss.Enabled = False
                chkIsSecGroupBoss.Enabled = False
                ddlRankID.Enabled = False
                ddlTitleID.Enabled = False
                ddlWorkSiteID.Enabled = False
                ddlWorkType.Enabled = False
                ucSelectWorkType.Enabled = False
                ddlPosition.Enabled = False
                ucSelectPosition.Enabled = False
                txtRecID.Enabled = False
                ucContractDate.Enabled = False
                ddlWTID.Enabled = False
            Case "40" '20160817 Beatrice Add 異動原因為40升等，僅開放【職等職稱】/【工作性質】/【職位】/【備註】等欄位可以修改。
                ddlQuitReason.Enabled = False
                ucSelectHROrgan.Enabled = False
                ucSelectFlowOrgan.Enabled = False
                'ddlFlowOrganID.Enabled = False
                chkSalaryPaid.Enabled = False
                ddlBossType.Enabled = False
                chkIsBoss.Enabled = False
                chkIsSecBoss.Enabled = False
                chkIsGroupBoss.Enabled = False
                chkIsSecGroupBoss.Enabled = False
                ddlRankID.Enabled = True
                ddlTitleID.Enabled = True
                ddlWorkSiteID.Enabled = False
                ddlWorkType.Enabled = True
                ucSelectWorkType.Enabled = True
                ddlPosition.Enabled = True
                ucSelectPosition.Enabled = True
                txtRecID.Enabled = False
                ucContractDate.Enabled = False
                ddlWTID.Enabled = False
            Case Else
                ddlQuitReason.Enabled = True
                ucSelectHROrgan.Enabled = True
                ucSelectFlowOrgan.Enabled = False
                'ddlFlowOrganID.Enabled = False
                chkSalaryPaid.Enabled = True
                ddlBossType.Enabled = True
                chkIsBoss.Enabled = True
                chkIsSecBoss.Enabled = True
                chkIsGroupBoss.Enabled = True
                chkIsSecGroupBoss.Enabled = True
                ddlRankID.Enabled = True
                ddlTitleID.Enabled = True
                ddlWorkSiteID.Enabled = True
                ddlWorkType.Enabled = True
                ucSelectWorkType.Enabled = True
                ddlPosition.Enabled = True
                ucSelectPosition.Enabled = True
                txtRecID.Enabled = True
                ucContractDate.Enabled = True
                ddlWTID.Enabled = True
        End Select
    End Sub
End Class
