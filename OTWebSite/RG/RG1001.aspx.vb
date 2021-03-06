'****************************************************
'功能說明：員工資料維護作業-新增
'建立人員：Micky Sung
'建立日期：2015.08.31
'****************************************************
Imports System.Data

Partial Class RG_RG1001
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then
            subGetData()
        End If
    End Sub

    Protected Overrides Sub BaseOnPageTransfer(ByVal ti As TransferInfo)
        If Not IsPostBack() Then

        End If
    End Sub

    Private Sub subGetData()
        Dim objHR As New HR()

        '公司代碼
        txtCompID.Text = UserProfile.SelectCompRoleName
        hidCompID.Value = UserProfile.SelectCompRoleID

        ViewState.Item("CheckInFileCompID") = objHR.funGetCheckInFileCompID(hidCompID.Value) '報到文件歸屬公司代碼
        ViewState.Item("IsRankIDMapFlag") = objHR.IsRankIDMapFlag(hidCompID.Value) '此公司是否有導入惠悅專案的註記
        ViewState.Item("PayrollCompID") = objHR.funGetPayrollCompID(hidCompID.Value) '計薪作業歸屬公司代碼

        '應試者編號UC
        ucSelectRecruit.WindowWidth = "1000"    '2015/11/09 Add
        ucSelectRecruit.SelectCompID = UserProfile.SelectCompRoleID

        '*部門/*科組課
        ucSelectHROrgan.LoadData(hidCompID.Value)

        '職等/職稱
        ucSelectRankAndTitle.LoadData(hidCompID.Value, "A")

        '簽核最小單位
        ucSelectFlowOrgan.LoadData(ucSelectHROrgan.SelectedOrganID)

        '身分別
        ddlNationID.SelectedValue = "1"

        '學歷
        Bsp.Utility.FillDDL(ddlEduID, "eHRMSDB", "EduDegree", "RTrim(EduID)", "EduName", Bsp.Utility.DisplayType.Full, "", "", "Order by EduID")
        ddlEduID.Items.Insert(0, New ListItem("---請選擇---", ""))
        ddlEduID.SelectedValue = "050"

        '任職狀況
        Bsp.Utility.FillDDL(ddlWorkStatus, "eHRMSDB", "WorkStatus", "RTrim(WorkCode)", "Remark", Bsp.Utility.DisplayType.Full, "", "", "Order by WorkCode")
        ddlWorkStatus.Items.Insert(0, New ListItem("---請選擇---", ""))
        ddlWorkStatus.SelectedValue = "1"

        '僱用類別
        Bsp.Utility.FillDDL(ddlEmpType, "eHRMSDB", "HRCodeMap", "RTrim(Code)", "CodeCName", Bsp.Utility.DisplayType.Full, "", " AND TabName = 'Personal' AND FldName = 'EmpType' AND NotShowFlag = '0'", "Order by SortFld")
        ddlEmpType.Items.Insert(0, New ListItem("---請選擇---", ""))
        ddlEmpType.SelectedValue = "1"

        '到職日
        txtEmpDate.DateText = Now.ToString("yyyy/MM/dd")

        '企業團到職日
        txtSinopacEmpDate.DateText = Now.ToString("yyyy/MM/dd")

        '試用期
        Bsp.Utility.FillDDL(ddlProbMonth, "eHRMSDB", "HRCodeMap", "RTrim(Code)", "RTrim(Code) + CodeCName", Bsp.Utility.DisplayType.OnlyName, "", " AND TabName = 'Personal' AND FldName = 'ProbMonth' AND NotShowFlag = '0'", "Order by SortFld")
        ddlProbMonth.SelectedValue = "3"

        '金控職等
        Bsp.Utility.FillDDL(ddlHoldingRankID, "eHRMSDB", "Rank", "RTrim(RankID)", "", Bsp.Utility.DisplayType.OnlyID, "", " AND CompID = 'SPHOLD'", "Order by RankID")
        ddlHoldingRankID.Items.Insert(0, New ListItem("---請選擇---", ""))

        '對外職稱
        Bsp.Utility.FillDDL(ddlPublicTitleID, "eHRMSDB", "PublicTitle", "RTrim(PublicTitleID)", "PublicTitleName", Bsp.Utility.DisplayType.Full, "", "", "Order by PublicTitleID")
        ddlPublicTitleID.Items.Insert(0, New ListItem("---請選擇---", ""))

        '最近升遷日\本階起始日
        txtRankBeginDate.DateText = Now.ToString("yyyy/MM/dd")

        '工作地點
        Bsp.Utility.FillDDL(ddlWorkSiteID, "eHRMSDB", "WorkSite", "RTrim(WorkSiteID)", "Remark", Bsp.Utility.DisplayType.Full, "", " AND CompID = " & Bsp.Utility.Quote(hidCompID.Value), "Order by WorkSiteID")
        ddlWorkSiteID.Items.Insert(0, New ListItem("---請選擇---", ""))

        '班別類型
        Bsp.Utility.FillDDL(ddlWTIDTypeFlag, "eHRMSDB", "HRCodeMap", "RTrim(Code)", "CodeCName", Bsp.Utility.DisplayType.Full, "", " AND TabName = 'WorkTime' AND FldName = 'WTIDTypeFlag' AND NotShowFlag = '0'", "Order By SortFld")
        ddlWTIDTypeFlag.Items.Insert(0, New ListItem("---請選擇---", ""))

        '班別
        'Bsp.Utility.FillDDL(ddlWTID, "eHRMSDB", "WorkTime", "RTrim(WTID)", "BeginTime + '-' + EndTime", Bsp.Utility.DisplayType.Full, "", " AND CompID = " & Bsp.Utility.Quote(hidCompID.Value), "Order by WTID")
        ddlWTID.Items.Insert(0, New ListItem("---請選擇---", ""))

        '族別
        Bsp.Utility.FillDDL(ddlAboriginalTribe, "eHRMSDB", "HRCodeMap", "RTrim(Code)", "CodeCName", Bsp.Utility.DisplayType.Full, "", " AND TabName = 'PersonalOther' AND FldName = 'AboriginalTribe' AND NotShowFlag = '0'", "Order by SortFld")
        ddlAboriginalTribe.Items.Insert(0, New ListItem("---請選擇---", ""))

        '職位
        ddlPositionID.Items.Insert(0, New ListItem("---請選擇---", ""))

        '工作性質
        ddlWorkTypeID.Items.Insert(0, New ListItem("---請選擇---", ""))

        '2015/12/14 Add 新增下拉選單:證件類型
        Bsp.Utility.FillDDL(ddlIDType, "eHRMSDB", "HRCodeMap", "RTrim(Code)", "CodeCName", Bsp.Utility.DisplayType.Full, "", " AND TabName = 'Personal' AND FldName = 'IDType' AND NotShowFlag = '0' ", "order by SortFld")
        ddlIDType.Items.Insert(0, New ListItem("---請選擇---", ""))
        ddlIDType.SelectedValue = "1"

        '原住民註記/族別
        ddlAboriginalTribe.Enabled = False
        ddlAboriginalTribe.SelectedValue = ""

        '身份證字號重複員工資料
        ReIDNo.Visible = False

        If Not ViewState.Item("IsRankIDMapFlag") Then
            ddlPositionID.Enabled = False
            ucSelectPosition.Enabled = False
        End If

        '需刷卡註記
        If ViewState.Item("PayrollCompID") = "SPHSC1" Then
            chkOfficeLoginFlag.Enabled = True
            chkOfficeLoginFlag.Checked = True
        Else
            chkOfficeLoginFlag.Enabled = False
            chkOfficeLoginFlag.Checked = False
        End If
    End Sub

    Public Overrides Sub DoAction(ByVal Param As String)
        Select Case Param
            Case "btnAdd"   '存檔返回
                If funCheckData() Then
                    If SaveData() Then
                        '導頁至subMenu
                        Dim a As New FlowBackInfo()
                        Dim objSC As New SC

                        a.URL = "~/RG/RG1000.aspx"
                        a.MenuNodeTitle = "回清單"

                        TransferFramePage(Bsp.MySettings.FlowRedirectPage, Nothing, "FlowID=EMPINFO", a, _
                            "SelectedCompID=" & hidCompID.Value, _
                            "SelectedCompName=" & objSC.GetCompName(hidCompID.Value).Rows(0).Item("CompName").ToString, _
                            "SelectedEmpID=" & txtEmpID.Text, _
                            "SelectedEmpName=" & txtNameN.Text, _
                            "SelectedIDNo=" & txtIDNo.Text, _
                            "DoQuery=" & Bsp.Utility.IsStringNull(ViewState.Item("DoQuery")))
                    End If
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
        Dim objHR As New HR()
        Dim objRG As New RG1()
        Dim objSC As New SC()
        Dim objRegistData As New RegistData()
        Dim strSQL As New StringBuilder()

        Dim bePersonal As New bePersonal.Row()
        Dim beCommunication As New beCommunication.Row()
        Dim bePersonalOther As New bePersonalOther.Row()
        Dim beEmpWorkTypeRows() As beEmpWorkType.Row = Nothing
        Dim beEmpPositionRows() As beEmpPosition.Row = Nothing
        Dim beCheckInFile_SPHBK1 As New beCheckInFile_SPHBK1.Row()
        Dim beCheckInFile_SPHSC1 As New beCheckInFile_SPHSC1.Row()
        Dim beEmployeeLog As New beEmployeeLog.Row()
        Dim beProbation As New beProbation.Row()
        Dim beProbationSPHSC1 As New beProbationSPHSC1.Row()
        Dim beSalaryData As New beSalaryData.Row()
        Dim beSalaryRows() As beSalary.Row = Nothing
        Dim beEmpRetireWait As New beEmpRetireWait.Row()
        Dim beEmpFlow As New beEmpFlow.Row()
        Dim beEmpSenRank As New beEmpSenRank.Row()
        Dim beEmpSenOrgType As New beEmpSenOrgType.Row()
        Dim beEmpSenOrgTypeFlow As New beEmpSenOrgTypeFlow.Row()
        Dim beEmpSenWorkType As New beEmpSenWorkType.Row()
        Dim beEmpSenPosition As New beEmpSenPosition.Row()
        Dim beEmpSenComp As New beEmpSenComp.Row()
        Dim beInsureWait As New beInsureWait.Row()
        Dim beGroupWait As New beGroupWait.Row()

        Dim intCount As Integer = 0
        Dim IsTypeA As Boolean = False
        Dim IsSalary As Boolean = False
        Dim intMonthOfAnnualPay As Integer = 0
        Dim strMonthOfAnnualPay As String = ""
        Dim strFinalAmount As String = ""
        Dim lngFinalAmount As Long = 0

        Try
            If txtRecID.Text <> "" Then
                '年薪月份
                strMonthOfAnnualPay = objRG.QueryDataRecruit("RE_SalaryData", " AND CompID = " & Bsp.Utility.Quote(hidReturnCompID.Value) & " AND RecID = " & Bsp.Utility.Quote(txtRecID.Text) & " AND CheckInDate = " & Bsp.Utility.Quote(hidCheckInDate.Value), "MonthOfAnnualPay")
                Integer.TryParse(strMonthOfAnnualPay, intMonthOfAnnualPay)

                '最終核定薪資
                strFinalAmount = objRG.Decryption("RE_SalaryData", " AND CompID = " & Bsp.Utility.Quote(hidReturnCompID.Value) & " AND RecID = " & Bsp.Utility.Quote(txtRecID.Text) & " AND CheckInDate = " & Bsp.Utility.Quote(hidCheckInDate.Value), "isnull(CONVERT(varchar(10), DecryptByKey(FinalAmount)), '0')")
                Long.TryParse(strFinalAmount, lngFinalAmount)
            End If

            '1. 新增員工基本資料[Personal]員工檔
            bePersonal.CompID.Value = hidCompID.Value
            bePersonal.EmpID.Value = txtEmpID.Text.ToUpper
            bePersonal.IDNo.Value = txtIDNo.Text.ToUpper
            bePersonal.Name.Value = txtName.Text
            bePersonal.NameN.Value = txtNameN.Text
            bePersonal.NameB.Value = txtNameB.Text
            bePersonal.EngName.Value = txtEngName.Text
            bePersonal.PassportName.Value = txtPassportName.Text
            bePersonal.BirthDate.Value = IIf(txtBirthDate.DateText = "", "1900/01/01", txtBirthDate.DateText)
            bePersonal.Sex.Value = ddlSex.SelectedValue
            bePersonal.NationID.Value = ddlNationID.SelectedValue
            bePersonal.IDExpireDate.Value = IIf(txtIDExpireDate.DateText = "", "1900/01/01", txtIDExpireDate.DateText)
            bePersonal.EduID.Value = ddlEduID.SelectedValue
            bePersonal.Marriage.Value = ddlMarriage.SelectedValue
            bePersonal.EmpType.Value = ddlEmpType.SelectedValue
            bePersonal.GroupID.Value = txtGroupID.Text.Trim
            bePersonal.DeptID.Value = ucSelectHROrgan.SelectedDeptID
            bePersonal.OrganID.Value = ucSelectHROrgan.SelectedOrganID
            bePersonal.WorkSiteID.Value = IIf(ddlWorkSiteID.SelectedValue <> "", ddlWorkSiteID.SelectedValue, ViewState.Item("WorkSiteID"))
            bePersonal.RankID.Value = ucSelectRankAndTitle.SelectedRankID
            bePersonal.RankIDMap.Value = objHR.FunGetRankIDMap(hidCompID.Value, ucSelectRankAndTitle.SelectedRankID)
            bePersonal.HoldingRankID.Value = ddlHoldingRankID.SelectedValue
            bePersonal.TitleID.Value = ucSelectRankAndTitle.SelectedTitleID
            bePersonal.PublicTitleID.Value = ddlPublicTitleID.SelectedValue
            bePersonal.EmpDate.Value = IIf(txtEmpDate.DateText = "", "1900/01/01", txtEmpDate.DateText)
            bePersonal.WorkStatus.Value = ddlWorkStatus.SelectedValue
            bePersonal.SinopacEmpDate.Value = IIf(txtSinopacEmpDate.DateText = "", "1900/01/01", txtSinopacEmpDate.DateText)
            'bePersonal.ProbDate.Value = IIf(txtProbDate.DateText = "", "1900/01/01", txtProbDate.DateText)
            bePersonal.ProbDate.Value = IIf(ddlProbMonth.SelectedValue = "0", txtEmpDate.DateText, "1900/01/01")
            bePersonal.ProbMonth.Value = ddlProbMonth.SelectedValue
            bePersonal.CheckInFlag.Value = "0"
            bePersonal.IsHLBFlag.Value = IIf(ViewState.Item("PayrollCompID") = "SPHBK1", IIf(objHR.FunGetRankIDMap(hidCompID.Value, ucSelectRankAndTitle.SelectedRankID) >= 10, "1", "0"), "0")
            bePersonal.PassExamFlag.Value = IIf(chkPassExamFlag.Checked, "1", "0")
            bePersonal.LocalHireFlag.Value = IIf(chkLocalHireFlag.Checked, "1", "0")
            bePersonal.RankBeginDate.Value = IIf(txtRankBeginDate.DateText = "", "1900/01/01", txtRankBeginDate.DateText)
            bePersonal.LastChgComp.Value = UserProfile.ActCompID
            bePersonal.LastChgID.Value = UserProfile.ActUserID
            bePersonal.LastChgDate.Value = Now
            bePersonal.IDType.Value = ddlIDType.SelectedValue   '2015/12/14 Add 證件類型

            '2. 新增員工特殊設定資料檔[PersonalOther]
            bePersonalOther.CompID.Value = hidCompID.Value
            bePersonalOther.EmpID.Value = txtEmpID.Text.ToUpper
            bePersonalOther.WTID.Value = ddlWTID.SelectedValue 'IIf(ViewState.Item("PayrollCompID") = "SPHSC1", ddlWTID.SelectedValue, "")
            bePersonalOther.OfficeLoginFlag.Value = IIf(chkOfficeLoginFlag.Checked, "1", "0") 'IIf(ViewState.Item("PayrollCompID") = "SPHSC1", IIf(chkOfficeLoginFlag.Checked, "1", "0"), "")
            bePersonalOther.AboriginalFlag.Value = IIf(chkAboriginalFlag.Checked, "1", "0")
            bePersonalOther.AboriginalTribe.Value = ddlAboriginalTribe.SelectedValue
            bePersonalOther.RecID.Value = txtRecID.Text
            bePersonalOther.CheckInDate.Value = IIf(hidCheckInDate.Value = "", "1900/01/01", hidCheckInDate.Value)
            bePersonalOther.LastChgComp.Value = UserProfile.ActCompID
            bePersonalOther.LastChgID.Value = UserProfile.ActUserID
            bePersonalOther.LastChgDate.Value = Now

            '3. 新增員工工作性質檔[EmpWorkType]
            Dim arrWorkTypeID() As String = lblSelectWorkTypeID.Text.Replace("'", "").Split(",")
            intCount = 0
            For Each strWorkTypeID In arrWorkTypeID
                ReDim Preserve beEmpWorkTypeRows(intCount)
                Dim beEmpWorkType As New beEmpWorkType.Row()

                beEmpWorkType.CompID.Value = hidCompID.Value
                beEmpWorkType.EmpID.Value = txtEmpID.Text.ToUpper

                beEmpWorkType.WorkTypeID.Value = strWorkTypeID
                If intCount = 0 Then
                    beEmpWorkType.PrincipalFlag.Value = "1"
                Else
                    beEmpWorkType.PrincipalFlag.Value = "0"
                End If

                beEmpWorkType.LastChgComp.Value = UserProfile.ActCompID
                beEmpWorkType.LastChgID.Value = UserProfile.ActUserID
                beEmpWorkType.LastChgDate.Value = Now

                beEmpWorkTypeRows(intCount) = beEmpWorkType
                intCount = intCount + 1
            Next

            '4. 新增員工職位檔[EmpPosition]
            If ViewState.Item("IsRankIDMapFlag") Then
                Dim arrPositionID() As String = lblSelectPosition.Text.Replace("'", "").Split(",")
                intCount = 0
                For Each strPositionID In arrPositionID
                    ReDim Preserve beEmpPositionRows(intCount)
                    Dim beEmpPosition As New beEmpPosition.Row()

                    beEmpPosition.CompID.Value = hidCompID.Value
                    beEmpPosition.EmpID.Value = txtEmpID.Text.ToUpper

                    beEmpPosition.PositionID.Value = strPositionID
                    If intCount = 0 Then
                        beEmpPosition.PrincipalFlag.Value = "1"
                    Else
                        beEmpPosition.PrincipalFlag.Value = "0"
                    End If

                    beEmpPosition.LastChgComp.Value = UserProfile.ActCompID
                    beEmpPosition.LastChgID.Value = UserProfile.ActUserID
                    beEmpPosition.LastChgDate.Value = Now

                    beEmpPositionRows(intCount) = beEmpPosition
                    intCount = intCount + 1
                Next
            End If

            '5. 新增員工報到文件檔[CheckInFile]
            If ViewState.Item("CheckInFileCompID") = "SPHBK1" Then
                beCheckInFile_SPHBK1.CompID.Value = hidCompID.Value
                beCheckInFile_SPHBK1.EmpID.Value = txtEmpID.Text.ToUpper
                Dim strFile9 As String
                If ddlSex.SelectedValue = "2" Or ddlNationID.SelectedValue = "2" Then
                    strFile9 = "1"
                Else
                    strFile9 = "0"
                End If
                beCheckInFile_SPHBK1.File9.Value = strFile9
                beCheckInFile_SPHBK1.File11.Value = "1"     '2016/09/22 add by Mandy 雅涵增加[201610060063]
                'beCheckInFile_SPHBK1.File12.Value = "1"     '2016/09/22 add by Mandy 雅涵增加[201610060063],11/3說恢復為不需預設
                beCheckInFile_SPHBK1.File14.Value = "1"
                beCheckInFile_SPHBK1.File15.Value = "1"
                'beCheckInFile_SPHBK1.File16.Value = IIf(objHR.FunGetRankIDMap(hidCompID.Value, ucSelectRankAndTitle.SelectedRankID) < "07", "1", "0")
                beCheckInFile_SPHBK1.File16.Value = "1"      '2016/09/22 modify by Mandy 雅涵說不用判斷職等[201610060063]
                beCheckInFile_SPHBK1.File18.Value = IIf(objRG.IsDataExists("EmpWorkType", " AND CompID = " & Bsp.Utility.Quote(hidCompID.Value) & " AND EmpID = " & Bsp.Utility.Quote(txtEmpID.Text.ToUpper) & " AND WorkTypeID in (select WorkTypeID from WorkType where CompID = " & Bsp.Utility.Quote(hidCompID.Value) & " AND PBFlag = '1') ") = True, "0", "1")
                '2015/11/30 Add Table新增欄位
                beCheckInFile_SPHBK1.LastChgComp.Value = UserProfile.ActCompID
                beCheckInFile_SPHBK1.LastChgID.Value = UserProfile.ActUserID
                beCheckInFile_SPHBK1.LastChgDate.Value = Now
            ElseIf ViewState.Item("CheckInFileCompID") = "SPHSC1" Then
                beCheckInFile_SPHSC1.CompID.Value = hidCompID.Value
                beCheckInFile_SPHSC1.EmpID.Value = txtEmpID.Text.ToUpper
                Dim strFile11 As String
                If ddlSex.SelectedValue = "2" Or ddlNationID.SelectedValue = "2" Then
                    strFile11 = "1"
                Else
                    strFile11 = "0"
                End If
                beCheckInFile_SPHSC1.File11.Value = strFile11
                '2015/11/30 Add Table新增欄位
                beCheckInFile_SPHSC1.LastChgComp.Value = UserProfile.ActCompID
                beCheckInFile_SPHSC1.LastChgID.Value = UserProfile.ActUserID
                beCheckInFile_SPHSC1.LastChgDate.Value = Now
            End If

            '6. 新增員工異動記錄檔[EmployeeLog]
            beEmployeeLog.IDNo.Value = txtIDNo.Text.ToUpper
            beEmployeeLog.ModifyDate.Value = IIf(txtEmpDate.DateText = "", "1900/01/01", txtEmpDate.DateText)
            beEmployeeLog.Reason.Value = "01"
            beEmployeeLog.Seq.Value = "1"
            beEmployeeLog.CompID.Value = hidCompID.Value
            beEmployeeLog.EmpID.Value = txtEmpID.Text.ToUpper
            beEmployeeLog.Company.Value = objSC.GetCompName(hidCompID.Value).Rows(0).Item("CompName").ToString
            beEmployeeLog.DeptID.Value = ucSelectHROrgan.SelectedDeptID
            beEmployeeLog.DeptName.Value = ucSelectHROrgan.SelectedDeptName
            beEmployeeLog.OrganID.Value = ucSelectHROrgan.SelectedOrganID
            beEmployeeLog.OrganName.Value = ucSelectHROrgan.SelectedOrganName
            beEmployeeLog.GroupID.Value = txtGroupID.Text.Trim
            beEmployeeLog.GroupName.Value = txtGroupName.Text.Trim
            beEmployeeLog.FlowOrganID.Value = ucSelectFlowOrgan.SelectedOrganID
            beEmployeeLog.FlowOrganName.Value = ucSelectFlowOrgan.SelectedOrganIDName
            beEmployeeLog.RankID.Value = ucSelectRankAndTitle.SelectedRankID
            beEmployeeLog.TitleID.Value = ucSelectRankAndTitle.SelectedTitleID
            beEmployeeLog.TitleName.Value = ucSelectRankAndTitle.SelectedTitleName
            beEmployeeLog.PositionID.Value = hidPositionID.Value
            beEmployeeLog.Position.Value = lblSelectPositionName.Text
            beEmployeeLog.WorkTypeID.Value = hidWorkTypeID.Value
            beEmployeeLog.WorkType.Value = lblSelectWorkTypeName.Text
            beEmployeeLog.WorkStatus.Value = ddlWorkStatus.SelectedValue
            Dim arrWorkStatus() As String
            arrWorkStatus = ddlWorkStatus.SelectedItem.Text.Split("-")
            beEmployeeLog.WorkStatusName.Value = arrWorkStatus(1)
            beEmployeeLog.Remark.Value = txtEmployeeLogRemark.Text
            beEmployeeLog.LastChgComp.Value = UserProfile.ActCompID
            beEmployeeLog.LastChgID.Value = UserProfile.ActUserID
            beEmployeeLog.LastChgDate.Value = Now

            '7. 新增試用考核檔[Probation](A類人員)
            If ddlProbMonth.SelectedValue <> "0" And ddlEmpType.SelectedValue <> "3" Then
                IsTypeA = True
                If ViewState.Item("PayrollCompID") <> "SPHSC1" Then
                    beProbation.CompID.Value = hidCompID.Value
                    beProbation.ApplyTime.Value = Now.ToString("yyyy/MM/dd hh:mm:ss")
                    beProbation.ApplyID.Value = txtEmpID.Text.ToUpper
                    beProbation.ProbSeq.Value = "1"
                    beProbation.ProbDate.Value = IIf(txtEmpDate.DateText = "", "1900/01/01", txtEmpDate.DateText)
                    beProbation.ProbDate2.Value = objHR.funGetProbationEndDate(txtEmpDate.DateText, ddlProbMonth.SelectedValue)
                    beProbation.LastChgComp.Value = UserProfile.ActCompID
                    beProbation.LastChgID.Value = UserProfile.ActUserID
                    beProbation.LastChgDate.Value = Now
                Else
                    beProbationSPHSC1.CompID.Value = hidCompID.Value
                    beProbationSPHSC1.ApplyTime.Value = Now.ToString("yyyy/MM/dd hh:mm:ss")
                    beProbationSPHSC1.ApplyID.Value = txtEmpID.Text.ToUpper
                    beProbationSPHSC1.ProbSeq.Value = "1"
                    beProbationSPHSC1.ProbDate.Value = IIf(txtEmpDate.DateText = "", "1900/01/01", txtEmpDate.DateText)
                    beProbationSPHSC1.ProbDate2.Value = objHR.funGetProbationEndDate(txtEmpDate.DateText, ddlProbMonth.SelectedValue)
                    beProbationSPHSC1.LastChgComp.Value = UserProfile.ActCompID
                    beProbationSPHSC1.LastChgID.Value = UserProfile.ActUserID
                    beProbationSPHSC1.LastChgDate.Value = Now
                End If
            End If

            '8. 新增員工薪資資料檔[SalaryData]
            beSalaryData.CompID.Value = hidCompID.Value
            beSalaryData.EmpID.Value = txtEmpID.Text.ToUpper
            Dim WelfareFlag As String = ""
            If hidCompID.Value = "SPHVCL" Or hidCompID.Value = "SPHOLD" Or hidCompID.Value = "SPHPIA" Or hidCompID.Value = "SPHPLA" Then
                WelfareFlag = "0"
            Else
                WelfareFlag = "1"
            End If
            beSalaryData.WelfareFlag.Value = WelfareFlag
            beSalaryData.SalaryDate.Value = IIf(txtEmpDate.DateText = "", "1900/01/01", txtEmpDate.DateText)
            beSalaryData.CreateUserComp.Value = UserProfile.ActCompID
            beSalaryData.CreateUserID.Value = UserProfile.ActUserID
            beSalaryData.CreateDate.Value = Now
            beSalaryData.LastChgComp.Value = UserProfile.ActCompID
            beSalaryData.LastChgID.Value = UserProfile.ActUserID
            beSalaryData.LastChgDate.Value = Now
            If txtRecID.Text <> "" Then '14.1.c 
                beSalaryData.MonthOfAnnualPay.Value = intMonthOfAnnualPay
            End If

            '9. 新增薪資主檔[Salary]
            Using dt = objRG.QueryTable("SalaryItem", " AND CompID = " & Bsp.Utility.Quote(hidCompID.Value) & " AND FixItem = '1' ", "SalaryID")
                If dt.Rows.Count > 0 Then
                    IsSalary = True
                    intCount = 0
                    Dim strAmount = "0"
                    For Each dr As DataRow In dt.Rows
                        ReDim Preserve beSalaryRows(intCount)
                        Dim beSalary As New beSalary.Row()

                        beSalary.CompID.Value = hidCompID.Value
                        beSalary.EmpID.Value = txtEmpID.Text.ToUpper
                        beSalary.SalaryID.Value = dr.Item(0)
                        beSalary.PayMethod.Value = "1"
                        beSalary.MethodRatio.Value = "0"
                        beSalary.MethodAmt.Value = "0"

                        If txtRecID.Text <> "" And dr.Item(0) = "A000" Then '14.1.d
                            strAmount = CStr(intMonthOfAnnualPay * lngFinalAmount)
                        End If

                        If dr.Item(0) = "B000" Then
                            strAmount = objRG.QueryData("Parameter", " AND CompID = " & Bsp.Utility.Quote(hidCompID.Value), "isnull(MealPay, 0)")
                        End If

                        beSalary.Amount.Value = objRegistData.funEncryptNumber(txtEmpID.Text.ToUpper, strAmount)
                        'beSalary.Amount.Value = strAmount
                        beSalary.PeriodFlag.Value = "1"
                        beSalary.ValidFrom.Value = "1900/01/01"
                        beSalary.ValidTo.Value = "1900/01/01"
                        beSalary.LastChgComp.Value = UserProfile.ActCompID
                        beSalary.LastChgID.Value = UserProfile.ActUserID
                        beSalary.LastChgDate.Value = Now

                        beSalaryRows(intCount) = beSalary
                        intCount = intCount + 1
                    Next
                End If
            End Using

            '10. 新增新進員工勞退新制放行作業檔[EmpRetireWait]
            beEmpRetireWait.ReleaseMark.Value = "0"
            beEmpRetireWait.CompID.Value = hidCompID.Value
            beEmpRetireWait.EmpID.Value = txtEmpID.Text.ToUpper
            beEmpRetireWait.ApplyDate.Value = IIf(txtEmpDate.DateText = "", "1900/01/01", txtEmpDate.DateText)
            beEmpRetireWait.Kind.Value = "1"
            beEmpRetireWait.EmpRatio.Value = "0"
            beEmpRetireWait.CompRatio.Value = "6"
            beEmpRetireWait.ManagerFlag.Value = "0"
            beEmpRetireWait.BossFlag.Value = "0"
            beEmpRetireWait.Source.Value = "1"
            beEmpRetireWait.ReleaseDate.Value = "1900/01/01"
            beEmpRetireWait.ReleaseComp.Value = "-"
            beEmpRetireWait.ReleaseEmpID.Value = "-"
            beEmpRetireWait.LastChgComp.Value = UserProfile.ActCompID
            beEmpRetireWait.LastChgID.Value = UserProfile.ActUserID
            beEmpRetireWait.LastChgDate.Value = Now

            '11. 新增員工最小簽核單位檔[EmpFlow]
            beEmpFlow.CompID.Value = hidCompID.Value
            beEmpFlow.EmpID.Value = txtEmpID.Text.ToUpper
            beEmpFlow.ActionID.Value = "01"
            beEmpFlow.OrganID.Value = ucSelectFlowOrgan.SelectedOrganID

            If ucSelectHROrgan.SelectedOrganName.ToString().EndsWith("(未生效)") Then
                Using dt As DataTable = objRG.GetOrgWaitData(hidCompID.Value, ucSelectHROrgan.SelectedOrganID, txtEmpDate.DateText, "2")
                    If dt.Rows.Count > 0 Then
                        beEmpFlow.GroupType.Value = dt.Rows(0).Item("GroupType")
                        beEmpFlow.GroupID.Value = dt.Rows(0).Item("GroupID")
                    Else
                        beEmpFlow.GroupType.Value = objRG.QueryData("OrganizationFlow", " AND VirtualFlag = '0' AND InValidFlag = '0' AND OrganID = " & Bsp.Utility.Quote(ucSelectHROrgan.SelectedOrganID), "GroupType")
                        beEmpFlow.GroupID.Value = objRG.QueryData("OrganizationFlow", "AND OrganID = " & Bsp.Utility.Quote(ucSelectHROrgan.SelectedOrganID), "GroupID")
                    End If
                End Using
            Else
                beEmpFlow.GroupType.Value = objRG.QueryData("OrganizationFlow", " AND VirtualFlag = '0' AND InValidFlag = '0' AND OrganID = " & Bsp.Utility.Quote(ucSelectHROrgan.SelectedOrganID), "GroupType")
                beEmpFlow.GroupID.Value = objRG.QueryData("OrganizationFlow", "AND OrganID = " & Bsp.Utility.Quote(ucSelectHROrgan.SelectedOrganID), "GroupID")
            End If

            beEmpFlow.LastChgComp.Value = UserProfile.ActCompID
            beEmpFlow.LastChgID.Value = UserProfile.ActUserID
            beEmpFlow.LastChgDate.Value = Now

            '12. 新增通訊資料檔[寫入Communication]
            beCommunication.IDNo.Value = txtIDNo.Text.ToUpper
            beCommunication.LastChgComp.Value = UserProfile.ActCompID
            beCommunication.LastChgID.Value = UserProfile.ActUserID
            beCommunication.LastChgDate.Value = Now

            If bePersonal.WorkSiteID.Value <> "" Then
                strSQL.Clear()
                strSQL.AppendLine("SELECT CountryCode, AreaCode, Telephone, ExtNo FROM WorkSite WHERE CompID = " & Bsp.Utility.Quote(hidCompID.Value) & " AND WorkSiteID = " & Bsp.Utility.Quote(ddlWorkSiteID.SelectedValue))
                Using dt As DataTable = Bsp.DB.ExecuteDataSet(CommandType.Text, strSQL.ToString(), "eHRMSDB").Tables(0)
                    If dt.Rows.Count > 0 Then
                        beCommunication.CompTelCode.Value = dt.Rows(0).Item(0).ToString()
                        beCommunication.AreaCode.Value = dt.Rows(0).Item(1).ToString()
                        beCommunication.CompTel.Value = dt.Rows(0).Item(2).ToString()
                        beCommunication.ExtNo.Value = dt.Rows(0).Item(3).ToString()
                    End If
                End Using
            End If

            '13.1 新增職等年資EmpSenRank
            beEmpSenRank.CompID.Value = hidCompID.Value
            beEmpSenRank.EmpID.Value = txtEmpID.Text.ToUpper
            beEmpSenRank.RankID.Value = ucSelectRankAndTitle.SelectedRankID
            beEmpSenRank.ValidDateB.Value = IIf(txtEmpDate.DateText = "", "1900/01/01", txtEmpDate.DateText)
            beEmpSenRank.LastChgComp.Value = UserProfile.ActCompID
            beEmpSenRank.LastChgID.Value = UserProfile.ActUserID
            beEmpSenRank.LastChgDate.Value = Now

            '13.2 新增單位年資EmpSenOrgType
            Dim strOrgType As String = ""   '單位類別
            Dim strOrgTypeName As String = "" '單位類別名稱

            If ucSelectHROrgan.SelectedOrganName.ToString().EndsWith("(未生效)") Then
                Using dt As DataTable = objRG.GetOrgWaitData(hidCompID.Value, ucSelectHROrgan.SelectedOrganID, txtEmpDate.DateText, "1")
                    If dt.Rows.Count > 0 Then
                        strOrgType = dt.Rows(0).Item("OrgType")
                        strOrgTypeName = IIf(strOrgType = ucSelectHROrgan.SelectedOrganID, dt.Rows(0).Item("OrganName"), objRG.QueryData("Organization", " AND OrganID = " & Bsp.Utility.Quote(strOrgType), "isnull(OrganName, '')"))
                    Else
                        strOrgType = objRG.QueryData("Organization", " AND OrganID in (select isnull(OrgType, '') from Organization where OrganID = " & Bsp.Utility.Quote(ucSelectHROrgan.SelectedDeptID) & ")", "isnull(OrgType, '')")
                        strOrgTypeName = objRG.QueryData("Organization", " AND OrganID = " & Bsp.Utility.Quote(strOrgType), "isnull(OrganName, '')")
                    End If
                End Using
            Else
                strOrgType = objRG.QueryData("Organization", " AND OrganID in (select isnull(OrgType, '') from Organization where OrganID = " & Bsp.Utility.Quote(ucSelectHROrgan.SelectedDeptID) & ")", "isnull(OrgType, '')")
                strOrgTypeName = objRG.QueryData("Organization", " AND OrganID = " & Bsp.Utility.Quote(strOrgType), "isnull(OrganName, '')")
            End If

            beEmpSenOrgType.CompID.Value = hidCompID.Value
            beEmpSenOrgType.Reason.Value = "01"
            beEmpSenOrgType.EmpID.Value = txtEmpID.Text.ToUpper
            beEmpSenOrgType.IDNo.Value = txtIDNo.Text.ToUpper
            beEmpSenOrgType.OrgCompID.Value = hidCompID.Value
            beEmpSenOrgType.OrgType.Value = strOrgType
            beEmpSenOrgType.OrgTypeName.Value = strOrgTypeName
            beEmpSenOrgType.OrganID.Value = ucSelectHROrgan.SelectedDeptID
            beEmpSenOrgType.OrgName.Value = ucSelectHROrgan.SelectedDeptName
            beEmpSenOrgType.ValidDateB.Value = IIf(txtEmpDate.DateText = "", "1900/01/01", txtEmpDate.DateText)
            beEmpSenOrgType.LastChgComp.Value = UserProfile.ActCompID
            beEmpSenOrgType.LastChgID.Value = UserProfile.ActUserID
            beEmpSenOrgType.LastChgDate.Value = Now

            '13.3 新增簽核單位年資EmpSenOrgTypeFlow
            beEmpSenOrgTypeFlow.CompID.Value = hidCompID.Value
            beEmpSenOrgTypeFlow.EmpID.Value = txtEmpID.Text.ToUpper
            beEmpSenOrgTypeFlow.OrgType.Value = strOrgType
            beEmpSenOrgTypeFlow.ValidDateB.Value = IIf(txtEmpDate.DateText = "", "1900/01/01", txtEmpDate.DateText)
            beEmpSenOrgTypeFlow.LastChgComp.Value = UserProfile.ActCompID
            beEmpSenOrgTypeFlow.LastChgID.Value = UserProfile.ActUserID
            beEmpSenOrgTypeFlow.LastChgDate.Value = Now

            '13.4 新增工作性質年資檔EmpSenWorkType
            Dim strWorkTypeName As String = ""  '主要的工作性質
            strWorkTypeName = objRG.QueryData("WorkType", " AND WorkTypeID = " & Bsp.Utility.Quote(ddlWorkTypeID.SelectedValue), "isnull(Remark, '')")
            beEmpSenWorkType.CompID.Value = hidCompID.Value
            beEmpSenWorkType.PreCompID.Value = hidCompID.Value
            beEmpSenWorkType.Reason.Value = "01"
            beEmpSenWorkType.EmpID.Value = txtEmpID.Text.ToUpper
            beEmpSenWorkType.IDNo.Value = txtIDNo.Text.ToUpper
            beEmpSenWorkType.WorkTypeID.Value = ddlWorkTypeID.SelectedValue
            beEmpSenWorkType.WorkType.Value = strWorkTypeName
            beEmpSenWorkType.ValidDateB.Value = IIf(txtEmpDate.DateText = "", "1900/01/01", txtEmpDate.DateText)
            beEmpSenWorkType.CategoryI.Value = objRG.QueryData("WorkType", " AND CompID = " & Bsp.Utility.Quote(hidCompID.Value & " AND WorkTypeID = " & Bsp.Utility.Quote(ddlWorkTypeID.SelectedValue)), "CategoryI")
            beEmpSenWorkType.CategoryII.Value = objRG.QueryData("WorkType", " AND CompID = " & Bsp.Utility.Quote(hidCompID.Value & " AND WorkTypeID = " & Bsp.Utility.Quote(ddlWorkTypeID.SelectedValue)), "CategoryII")
            beEmpSenWorkType.CategoryIII.Value = objRG.QueryData("WorkType", " AND CompID = " & Bsp.Utility.Quote(hidCompID.Value & " AND WorkTypeID = " & Bsp.Utility.Quote(ddlWorkTypeID.SelectedValue)), "CategoryIII")
            beEmpSenWorkType.LastChgComp.Value = UserProfile.ActCompID
            beEmpSenWorkType.LastChgID.Value = UserProfile.ActUserID
            beEmpSenWorkType.LastChgDate.Value = Now

            '13.5 新增職位年資檔EmpSenPosition
            Dim strPosition As String = ""
            strPosition = objRG.QueryData("Position", " AND PositionID = " & Bsp.Utility.Quote(ddlPositionID.SelectedValue), "isnull(Remark, '')")
            beEmpSenPosition.CompID.Value = hidCompID.Value
            beEmpSenPosition.PreCompID.Value = hidCompID.Value
            beEmpSenPosition.Reason.Value = "01"
            beEmpSenPosition.EmpID.Value = txtEmpID.Text.ToUpper
            beEmpSenPosition.IDNo.Value = txtIDNo.Text.ToUpper
            beEmpSenPosition.PositionID.Value = ddlPositionID.SelectedValue
            beEmpSenWorkType.CategoryI.Value = objRG.QueryData("Position", " AND CompID = " & Bsp.Utility.Quote(hidCompID.Value & " AND PositionID = " & Bsp.Utility.Quote(ddlPositionID.SelectedValue)), "CategoryI")
            beEmpSenWorkType.CategoryII.Value = objRG.QueryData("Position", " AND CompID = " & Bsp.Utility.Quote(hidCompID.Value & " AND PositionID = " & Bsp.Utility.Quote(ddlPositionID.SelectedValue)), "CategoryII")
            beEmpSenWorkType.CategoryIII.Value = objRG.QueryData("Position", " AND CompID = " & Bsp.Utility.Quote(hidCompID.Value & " AND PositionID = " & Bsp.Utility.Quote(ddlPositionID.SelectedValue)), "CategoryIII")
            beEmpSenPosition.LastChgComp.Value = UserProfile.ActCompID
            beEmpSenPosition.LastChgID.Value = UserProfile.ActUserID
            beEmpSenPosition.LastChgDate.Value = Now

            '13.6 新增公司年資檔EmpSenComp
            '2016/10/26 增加參數intEmpSenComp[201610270001]判斷存在的邏輯是要用CompID+EmpID+IDNo檢查EmpSenComp是否有資料by Mandy(因為婉渝設計年資，跨公司會在待異動就會用原公司EmpID+IDNO新增)
            Dim intEmpSenComp As Integer = 0
            Integer.TryParse(objRG.QueryData("EmpSenComp" _
                                            , " AND CompID = " & Bsp.Utility.Quote(hidCompID.Value) _
                                            & " AND EmpID = " & Bsp.Utility.Quote(txtEmpID.Text.ToUpper) _
                                            & " AND IDNo = " & Bsp.Utility.Quote(txtIDNo.Text.ToUpper) _
                                            , "ISNULL(COUNT(IDNo), 0) ") _
                                        , intEmpSenComp)

            If intEmpSenComp = 0 Then
                beEmpSenComp.CompID.Value = hidCompID.Value
                beEmpSenComp.EmpID.Value = txtEmpID.Text.ToUpper
                beEmpSenComp.IDNo.Value = txtIDNo.Text.ToUpper
                beEmpSenComp.ValidDateB.Value = IIf(txtEmpDate.DateText = "", "1900/01/01", txtEmpDate.DateText)
                beEmpSenComp.ValidDateB_Sinopac.Value = IIf(txtSinopacEmpDate.DateText = "", "1900/01/01", txtSinopacEmpDate.DateText)
                beEmpSenComp.LastChgComp.Value = UserProfile.ActCompID
                beEmpSenComp.LastChgID.Value = UserProfile.ActUserID
                beEmpSenComp.LastChgDate.Value = Now
            End If

            '14.
            If txtRecID.Text <> "" Then
                '14.2 新增勞健InsureWait保險待加退保檔
                beInsureWait.WaitType.Value = "1"
                beInsureWait.WaitDate.Value = IIf(txtEmpDate.DateText = "", "1900/01/01", txtEmpDate.DateText)
                beInsureWait.CompID.Value = hidCompID.Value
                beInsureWait.EmpID.Value = txtEmpID.Text.ToUpper
                beInsureWait.RelativeIDNo.Value = ""
                beInsureWait.Source.Value = "1"
                beInsureWait.Amount.Value = objRegistData.funEncryptNumber(txtEmpID.Text.ToUpper, objHR.funGetLabHeaRetireLevel(lngFinalAmount, "Amount", "Hea"))
                beInsureWait.LabAmount.Value = objRegistData.funEncryptNumber(txtEmpID.Text.ToUpper, objHR.funGetLabHeaRetireLevel(lngFinalAmount, "Amount", "Lab"))
                beInsureWait.RetireAmount.Value = objRegistData.funEncryptNumber(txtEmpID.Text.ToUpper, objHR.funGetLabHeaRetireLevel(lngFinalAmount, "Amount", "Retire"))
                beInsureWait.MthRealWages.Value = objRegistData.funEncryptNumber(txtEmpID.Text.ToUpper, CStr(lngFinalAmount))
                'beInsureWait.Amount.Value = objHR.funGetLabHeaRetireLevel(lngFinalAmount, "Amount", "Hea")
                'beInsureWait.LabAmount.Value = objHR.funGetLabHeaRetireLevel(lngFinalAmount, "Amount", "Lab")
                'beInsureWait.RetireAmount.Value = objHR.funGetLabHeaRetireLevel(lngFinalAmount, "Amount", "Retire")
                'beInsureWait.MthRealWages.Value = CStr(lngFinalAmount)
                beInsureWait.IdentityID.Value = "01"
                beInsureWait.RelativeID.Value = "00"
                beInsureWait.Reason.Value = "01"
                beInsureWait.LabDate.Value = IIf(txtEmpDate.DateText = "", "1900/01/01", txtEmpDate.DateText)
                beInsureWait.HeaDate.Value = IIf(txtEmpDate.DateText = "", "1900/01/01", txtEmpDate.DateText)
                beInsureWait.InsureState.Value = "0"
                '2015/11/30 Add Table新增欄位
                beInsureWait.LastChgComp.Value = UserProfile.ActCompID
                beInsureWait.LastChgID.Value = UserProfile.ActUserID
                beInsureWait.LastChgDate.Value = Now

                '14.3 修改EmpRetireWait新進員工勞退新制放行作業
                beEmpRetireWait.Amount.Value = objRegistData.funEncryptNumber(txtEmpID.Text.ToUpper, CStr(lngFinalAmount))
                'beEmpRetireWait.Amount.Value = CStr(lngFinalAmount)

                '14.4 新增團保GroupWait
                beGroupWait.WaitType.Value = "1"
                beGroupWait.WaitDate.Value = IIf(txtEmpDate.DateText = "", "1900/01/01", txtEmpDate.DateText)
                beGroupWait.CompID.Value = hidCompID.Value
                beGroupWait.EmpID.Value = txtEmpID.Text.ToUpper
                beGroupWait.RelativeIDNo.Value = ""
                beGroupWait.Source.Value = "1"
                beGroupWait.GrpLvl.Value = ucSelectRankAndTitle.SelectedRankID
                beGroupWait.RelativeID.Value = "00"
                beGroupWait.GrpDate.Value = IIf(txtEmpDate.DateText = "", "1900/01/01", txtEmpDate.DateText)
                beGroupWait.NotifyFlag.Value = IIf(objHR.FunGetRankIDMap(hidCompID.Value, ucSelectRankAndTitle.SelectedRankID) < "07", "0", "1")
                '2015/11/30 Add Table新增欄位
                beGroupWait.LastChgComp.Value = UserProfile.ActCompID
                beGroupWait.LastChgID.Value = UserProfile.ActUserID
                beGroupWait.LastChgDate.Value = Now

                '14.5.a 學歷資料 Education

                '14.5.b 前職資料 Experience

                '14.5.c 家庭資料 Family

                '14.5.d 通訊資料 Communication
            End If

            '檢查資料是否存在
            'If bsCompany.IsDataExists(beCompany) Then
            '    Bsp.Utility.ShowFormatMessage(Me, "W_00010", "")
            '    Return False
            'End If

            '儲存資料
            '2016/10/26 增加參數intEmpSenComp[201610270001]判斷存在的邏輯是要用CompID+EmpID+IDNo檢查EmpSenComp是否有資料by Mandy(因為婉渝設計年資，跨公司會在待異動就會用原公司EmpID+IDNO新增)
            Return objRG.AddPersonalSetting(bePersonal, bePersonalOther, beEmployeeLog, ViewState.Item("IsRankIDMapFlag"), beEmpWorkTypeRows, beEmpPositionRows, ViewState.Item("CheckInFileCompID"), beCheckInFile_SPHBK1, beCheckInFile_SPHSC1, _
                                            IsTypeA, ViewState.Item("PayrollCompID"), beProbation, beProbationSPHSC1, beSalaryData, IsSalary, beSalaryRows, beEmpRetireWait, beEmpFlow, beCommunication, beEmpSenRank, _
                                            beEmpSenOrgType, beEmpSenOrgTypeFlow, beEmpSenWorkType, beEmpSenPosition, beEmpSenComp, beInsureWait, beGroupWait, intEmpSenComp
                                            )
        Catch ex As Exception
            Dim errLine As Integer = Convert.ToInt32(ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(" ")))
            Bsp.Utility.ShowMessage(Me, "[SaveData]" & errLine & Bsp.Utility.getMessage("E_00000") & Bsp.Utility.getInnerException(Me.FunID, ex))
            Return False
        End Try
    End Function

    Private Function funCheckData() As Boolean
        Dim objRG As New RG1()
        Dim objHR As New HR()

        '*員工編號
        If txtEmpID.Text.Trim = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblEmpID.Text)
            txtEmpID.Focus()
            Return False
        Else
            If txtEmpID.Text.Trim.Length <> 6 Then
                Bsp.Utility.ShowMessage(Me, "『員工編號，需為6碼』")
                txtEmpID.Focus()
                Return False
            End If
        End If

        '*員工姓名(難字)
        If txtNameN.Text.Trim = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", "員工姓名(難字)")
            txtNameN.Focus()
            Return False
        Else
            If txtNameN.Text.ToString().Trim().Length > txtNameN.MaxLength Then
                Bsp.Utility.ShowFormatMessage(Me, "W_00040", "員工姓名(難字)", txtNameN.MaxLength.ToString())
                txtNameN.Focus()
                Return False
            End If
        End If

        '*員工姓名(拆字)
        If txtName.Text.Trim = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", "員工姓名(拆字)")
            txtNameN.Focus()
            Return False
        Else
            If txtNameN.Text.ToString().Trim() = "" Then
                Bsp.Utility.ShowFormatMessage(Me, "W_00030", "員工姓名(難字)")
                txtNameN.Focus()
                Return False
            Else
                If txtName.Text.ToString().Trim().Length > txtName.MaxLength Then
                    Bsp.Utility.ShowFormatMessage(Me, "W_00040", "員工姓名(拆字)", txtName.MaxLength.ToString())
                    txtName.Focus()
                    Return False
                End If

                For iLoop As Integer = 1 To Len(txtName.Text.ToString().Trim().Replace("?", ""))
                    If Bsp.Utility.getStringLength(Mid(txtName.Text.ToString().Trim(), iLoop, 1)) = 1 And Not Char.IsLower(Mid(txtName.Text.ToString().Trim(), iLoop, 1)) And Not Char.IsUpper(Mid(txtName.Text.ToString().Trim(), iLoop, 1)) Then
                        Bsp.Utility.ShowFormatMessage(Me, "W_00031", "員工姓名(拆字)請勿輸入難字!")
                        txtName.Focus()
                        Return False
                    End If
                Next
            End If
        End If

        '員工姓名(造字)
        If txtNameB.Text.Trim <> "" Then
            If txtNameB.Text.ToString().Trim().Length > txtNameB.MaxLength Then
                Bsp.Utility.ShowFormatMessage(Me, "W_00040", "員工姓名(造字)", txtNameB.MaxLength.ToString())
                txtNameB.Focus()
                Return False
            End If
        End If

        '*身分證字號
        If txtIDNo.Text.Trim = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblIDNo.Text)
            txtIDNo.Focus()
            Return False
        End If

        '英文姓名
        If Bsp.Utility.getStringLength(txtEngName.Text.Trim) > txtEngName.MaxLength Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00040", lblEngName.Text, txtEngName.MaxLength.ToString)
            txtEngName.Focus()
            Return False
        End If

        '護照英文名字
        If Bsp.Utility.getStringLength(txtPassportName.Text.Trim) > txtPassportName.MaxLength Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00040", lblPassportName.Text, txtPassportName.MaxLength.ToString)
            txtPassportName.Focus()
            Return False
        End If

        '*生日
        If txtBirthDate.DateText.Trim = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblBirthDate.Text)
            txtBirthDate.Focus()
            Return False
        End If

        '*性別
        If ddlSex.SelectedValue = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblSex.Text)
            ddlSex.Focus()
            Return False
        End If

        '*國籍
        If ddlNationID.SelectedValue = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblNationID.Text)
            ddlNationID.Focus()
            Return False
        End If

        '工作證期限(國籍為2時必填) '2016/03/09 移除檢核
        'If ddlIDType.SelectedValue = "2" Then '2015/12/14 Modify 修改檢核判斷
        '    'If ddlNationID.SelectedValue = "2" Then
        '    If txtIDExpireDate.DateText = "" Then
        '        Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblIDExpireDate.Text)
        '        txtIDExpireDate.Focus()
        '        Return False
        '    End If
        'End If

        '*婚姻狀況
        If ddlMarriage.SelectedValue = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblMarriage.Text)
            ddlMarriage.Focus()
            Return False
        End If

        '*任職狀況
        If ddlWorkStatus.SelectedValue = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblWorkStatus.Text)
            ddlWorkStatus.Focus()
            Return False
        End If

        '2016/11/24 Add
        If ddlWorkStatus.SelectedValue = "9" Then
            If Not (hidCompID.Value = "SPHSC1" Or hidCompID.Value = "SPHFC1" Or hidCompID.Value = "SPHICC") Then
                Bsp.Utility.ShowMessage(Me, "僅SPHSC1證券/SPHFC1期貨/SPHICC投顧，任職狀況才可以為9-待報到")
                ddlWorkStatus.Focus()
                Return False
            Else
                If txtEmpDate.DateText <= Now.Date Then
                    Bsp.Utility.ShowMessage(Me, "任職狀況為9-待報到，到職日需大於今日")
                    ddlWorkStatus.Focus()
                    Return False
                End If
            End If
        End If

        '*到職日
        If txtEmpDate.DateText.Trim = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblEmpDate.Text)
            txtEmpDate.Focus()
            Return False
        End If

        '*企業團到職日
        If txtSinopacEmpDate.DateText.Trim = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblSinopacEmpDate.Text)
            txtSinopacEmpDate.Focus()
            Return False
        End If

        '*職等/*職稱
        If ucSelectRankAndTitle.SelectedRankID = "" Or ucSelectRankAndTitle.SelectedTitleID = "" Then
            Bsp.Utility.ShowMessage(Me, "職等RankID/職稱TitleID為必要欄位，不可空白")
            ucSelectRankAndTitle.Focus()
            Return False
        End If

        '*金控職等
        If ddlHoldingRankID.SelectedValue.Trim = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblHoldingRankID.Text)
            ddlHoldingRankID.Focus()
            Return False
        End If

        '*最近升遷日\本階起始日
        If txtRankBeginDate.DateText.Trim = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblRankBeginDate.Text)
            txtRankBeginDate.Focus()
            Return False
        End If

        '*部門/*科組課
        If ucSelectHROrgan.SelectedDeptID = "" Or ucSelectHROrgan.SelectedOrganID = "" Then
            Bsp.Utility.ShowMessage(Me, "「部門DeptID/科組課為必要欄位，不可空白」")
            ucSelectHROrgan.Focus()
            Return False
        End If

        '職位
        If ViewState.Item("IsRankIDMapFlag") Then
            If ddlPositionID.SelectedValue = "" Then
                Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblPositionID.Text)
                ddlPositionID.Focus()
                Return False
            End If

            If ddlPositionID.Items.Count > 5 Then
                Bsp.Utility.ShowMessage(Me, lblPositionID.Text & "-下拉選單資料不可超過5個")
                ddlPositionID.Focus()
                Return False
            End If
        End If

        '*工作性質
        If ddlWorkTypeID.SelectedValue.Trim = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblWorkTypeID.Text)
            ddlWorkTypeID.Focus()
            Return False
        End If

        If ddlWorkTypeID.Items.Count > 5 Then
            Bsp.Utility.ShowMessage(Me, lblWorkTypeID.Text & "-下拉選單資料不可超過5個")
            ddlWorkTypeID.Focus()
            Return False
        End If

        '*最小簽核單位
        If ucSelectFlowOrgan.SelectedOrganID = "" Then
            Bsp.Utility.ShowMessage(Me, "「請挑選最小簽核單位」")
            ucSelectFlowOrgan.Focus()
            Return False
        End If

        '企業團經歷的備註
        If txtEmployeeLogRemark.Text.Trim.Length > txtEmployeeLogRemark.MaxLength Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00040", lblEmployeeLogRemark.Text, txtEmployeeLogRemark.MaxLength.ToString)
            txtEmployeeLogRemark.Focus()
            Return False
        End If

        '員編檢查
        If objRG.IsDataExists("Personal", " AND EmpID = " & Bsp.Utility.Quote(txtEmpID.Text.ToUpper) & " AND WorkStatus = '1' ") Then
            Bsp.Utility.ShowMessage(Me, "『Personal員工資料已存在此員編且為在職，請勿重複新增！』")
            Return False
        End If
        If objRG.IsDataExists("Personal", " AND CompID = " & Bsp.Utility.Quote(hidCompID.Value) & " AND EmpID = " & Bsp.Utility.Quote(txtEmpID.Text.ToUpper)) Then
            Bsp.Utility.ShowMessage(Me, "『Personal員工資料在此公司已存在此員編，請勿重複新增！』")
            Return False
        End If
        If objRG.IsDataExists("PersonalOutsourcing", " AND CompID = " & Bsp.Utility.Quote(hidCompID.Value) & " AND EmpID = " & Bsp.Utility.Quote(txtEmpID.Text.ToUpper)) Then
            Bsp.Utility.ShowMessage(Me, "『PersonalOutsourcing相同的員工編號資料已存在委外人員資料檔，請勿重複新增！』")
            Return False
        End If

        '身分證是否重複
        If objRG.IsDataExists("Personal", " AND IDNo = " & Bsp.Utility.Quote(txtIDNo.Text.ToUpper) & " AND CompID = " & Bsp.Utility.Quote(hidCompID.Value)) Then
            Bsp.Utility.ShowMessage(Me, "「若為同公司同身份證字號員工，請由待異動維護離職復職，不可新增！」")
            txtIDNo.Focus()
            Return False
        End If
        If objRG.IsDataExists("Personal", " AND IDNo = " & Bsp.Utility.Quote(txtIDNo.Text.ToUpper) & " AND WorkStatus = '1'") Then
            Bsp.Utility.ShowMessage(Me, "「身分證字號∕居留證號，已經存在，且為在職狀態，不可重複輸入。新增員工失敗！」")
            txtIDNo.Focus()
            Return False
        End If

        '2015/12/14 Add 新增下拉選單檢核:證件類型
        If ddlIDType.SelectedValue = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblIDType.Text)
            ddlIDType.Focus()
            Return False
        End If

        '身分證字號邏輯判斷 2015/12/14 Modify 變更檢核判斷
        If ddlIDType.SelectedValue = "1" Then
            'If ddlNationID.SelectedValue = "1" Then
            If objHR.funCheckIDNO(txtIDNo.Text.ToUpper) = False Then
                Bsp.Utility.RunClientScript(Me, "confirmAdd()")
                Return False
            End If
        ElseIf ddlIDType.SelectedValue = "2" Then
            'ElseIf ddlNationID.SelectedValue = "2" Then
            If objHR.funCheckResidentIDNo(txtIDNo.Text.ToUpper) = False Then
                Bsp.Utility.RunClientScript(Me, "confirmAdd()")
                Return False
            End If
        End If

        Return True
    End Function

    '身分證字號邏輯有誤，是否重新輸入→是
    Protected Sub btnYes_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnYes.Click
        txtIDNo.Focus()
    End Sub

    '身分證字號邏輯有誤，是否重新輸入→否
    Protected Sub btnNo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNo.Click
        If SaveData() Then
            '導頁至subMenu
            Dim a As New FlowBackInfo()
            Dim objSC As New SC

            a.URL = "~/RG/RG1000.aspx"
            a.MenuNodeTitle = "回清單"

            TransferFramePage(Bsp.MySettings.FlowRedirectPage, Nothing, "FlowID=EMPINFO", a, _
                "SelectedCompID=" & hidCompID.Value, _
                "SelectedCompName=" & objSC.GetCompName(hidCompID.Value).Rows(0).Item("CompName").ToString, _
                "SelectedEmpID=" & txtEmpID.Text, _
                "SelectedEmpName=" & txtNameN.Text, _
                "SelectedIDNo=" & txtIDNo.Text, _
                "DoQuery=" & Bsp.Utility.IsStringNull(ViewState.Item("DoQuery")))
        End If
    End Sub

    Private Sub ClearData()
        subGetData()

        txtEmpID.Text = ""
        txtRecID.Text = ""
        hidReturnCompID.Value = ""
        hidCheckInDate.Value = ""
        txtNameN.Text = ""
        txtName.Text = ""
        txtNameB.Text = ""
        txtIDNo.Text = ""
        txtEngName.Text = ""
        txtPassportName.Text = ""
        txtBirthDate.DateText = ""
        ddlSex.SelectedValue = ""
        'ddlNationID.SelectedValue = ""
        txtIDExpireDate.DateText = ""
        ddlMarriage.SelectedValue = ""
        'txtEmpDate.DateText = ""
        'txtSinopacEmpDate.DateText = ""
        'txtProbDate.Text = ""
        ViewState.Item("WorkSiteID") = ""
        txtHoldingTitle.Text = ""
        'txtRankBeginDate.DateText = ""
        txtGroupID.Text = ""
        txtGroupName.Text = ""

        '職位
        lblSelectPosition.Text = ""
        lblSelectPositionName.Text = ""
        ddlPositionID.Items.Clear()
        ddlPositionID.Items.Insert(0, New ListItem("---請選擇---", ""))
        hidPositionID.Value = ""

        ''*工作性質
        lblSelectWorkTypeID.Text = ""
        lblSelectWorkTypeName.Text = ""
        ddlWorkTypeID.Items.Clear()
        ddlWorkTypeID.Items.Insert(0, New ListItem("---請選擇---", ""))
        hidWorkTypeID.Value = ""

        '班別
        ddlWTID.Items.Clear()
        ddlWTID.Items.Insert(0, New ListItem("---請選擇---", ""))

        chkLocalHireFlag.Checked = False
        chkPassExamFlag.Checked = False
        chkAboriginalFlag.Checked = False

        txtEmployeeLogRemark.Text = ""

    End Sub

    Public Overrides Sub DoModalReturn(ByVal returnValue As String)
        Dim strSql As String = ""
        Dim strRstName1 As String = ""
        Dim strRstName2 As String = ""

        If returnValue <> "" Then
            Dim aryData() As String = returnValue.Split(":")

            Select Case aryData(0)
                Case "ucSelectRecruit"      '應試者編號
                    ClearData()

                    Dim aryValue() As String = Split(aryData(1), "|$|")
                    hidReturnCompID.Value = aryValue(0) '公司代碼
                    txtRecID.Text = aryValue(1)         '應試者編號
                    hidCheckInDate.Value = aryValue(2)  '預計到報到日

                    GetData_ucSelectRecruit()
                Case "ucSelectWorkType"     '工作性質
                    lblSelectWorkTypeID.Text = aryData(1)

                    If lblSelectWorkTypeID.Text <> "''" Then  '非必填時，回傳空值
                        '載入 工作性質 下拉式選單
                        strSql = " and WorkTypeID in (" + lblSelectWorkTypeID.Text + ") and CompID = '" + hidCompID.Value + "'"
                        Bsp.Utility.WorkType(ddlWorkTypeID, "WorkTypeID", , strSql)

                        '第一筆為主要工作性質
                        Dim strDefaultValue() As String = lblSelectWorkTypeID.Text.Replace("'", "").Split(",")
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
                    Else
                        ddlWorkTypeID.Items.Clear()
                        ddlWorkTypeID.Items.Insert(0, New ListItem("---請選擇---", ""))
                    End If

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
                Case "ucSelectPosition"     '職位
                    lblSelectPosition.Text = aryData(1)

                    If lblSelectPosition.Text <> "''" Then  '非必填時，回傳空值
                        '載入 職位 下拉式選單
                        strSql = " and PositionID in (" + lblSelectPosition.Text + ") and CompID = '" + hidCompID.Value + "'"
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
                    Else
                        ddlPositionID.Items.Clear()
                        ddlPositionID.Items.Insert(0, New ListItem("---請選擇---", ""))
                    End If

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
            End Select

        End If
    End Sub

    '20161108 ADD
    '隱藏的按鈕--生效日(組織待異動生效日期<=到職日)
    Protected Sub btnValidDate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnValidDate.Click
        If hidCompID.Value = "SPHSC1" Or hidCompID.Value = "SPHFC1" Or hidCompID.Value = "SPHICC" Then
            If txtEmpDate.DateText <> "" And txtEmpDate.DateText <> "____/__/__" And txtEmpDate.DateText <> Now.Date Then
                ucSelectHROrgan.HasWait = True
                ucSelectHROrgan.ValidDate = txtEmpDate.DateText
            Else
                ucSelectHROrgan.HasWait = False
                ucSelectHROrgan.ValidDate = ""
            End If
        End If
        ucSelectHROrgan.LoadData(hidCompID.Value)
    End Sub

    Protected Sub ucSelectRankAndTitle_ucSelectRankIDSelectedIndexChangedHandler_SelectChange(ByVal sender As Object, ByVal e As System.EventArgs) Handles ucSelectRankAndTitle.ucSelectRankIDSelectedIndexChangedHandler_SelectChange
        Dim objPA As New PA1()
        Dim strHoldingRankID As String = ""

        Using dt As DataTable = objPA.GetTitleByHolding2(hidCompID.Value, ucSelectRankAndTitle.SelectedRankID)
            If dt.Rows.Count > 0 Then
                strHoldingRankID = dt.Rows(0).Item(0)
            End If
        End Using

        Bsp.Utility.SetSelectedIndex(ddlHoldingRankID, strHoldingRankID)
    End Sub

    '部門change
    Protected Sub ucSelectHROrgan_ucSelectDeptIDSelectedIndexChangedHandler_SelectChange(ByVal sender As Object, ByVal e As System.EventArgs) Handles ucSelectHROrgan.ucSelectDeptIDSelectedIndexChangedHandler_SelectChange
        ddlWorkTypeID.Items.Clear()
        ddlWorkTypeID.Items.Insert(0, New ListItem("---請選擇---", ""))
        hidWorkTypeID.Value = ""
        lblSelectWorkTypeID.Text = ""
    End Sub

    '科組課change
    Protected Sub ucSelectHROrgan_ucSelectOrganIDSelectedIndexChangedHandler_SelectChange(ByVal sender As Object, ByVal e As System.EventArgs) Handles ucSelectHROrgan.ucSelectOrganIDSelectedIndexChangedHandler_SelectChange
        Dim objHR3000 As New HR3000
        Dim objRG As New RG1()
        Dim strMainWorkTypeID As String = ""
        Dim strMainPositionID As String = ""
        Dim bolWorkType As Boolean = False
        Dim bolPosition As Boolean = False

        ViewState.Item("WorkSiteID") = ""

        If ucSelectHROrgan.SelectedOrganName.ToString().EndsWith("(未生效)") Then

            Using dt As DataTable = objRG.GetOrgWaitData(hidCompID.Value, ucSelectHROrgan.SelectedOrganID, txtEmpDate.DateText, "1")
                If dt.Rows.Count > 0 Then
                    txtGroupID.Text = dt.Rows(0).Item("GroupID")
                    ViewState.Item("WorkSiteID") = dt.Rows(0).Item("WorkSiteID")
                Else
                    txtGroupID.Text = ""
                End If
            End Using

            '事業群
            txtGroupName.Text = objRG.QueryData("OrganizationFlow", "And OrganID = " & Bsp.Utility.Quote(txtGroupID.Text), "OrganName")

            '工作地點
            Bsp.Utility.SetSelectedIndex(ddlWorkSiteID, ViewState.Item("WorkSiteID"))

            '最小簽核單位
            If txtEmpDate.DateText <> "" And txtEmpDate.DateText <> "____/__/__" Then
                ucSelectFlowOrgan.IsWait = True
                ucSelectFlowOrgan.ValidDate = txtEmpDate.DateText
            Else
                ucSelectFlowOrgan.IsWait = False
                ucSelectFlowOrgan.ValidDate = ""
            End If
            ucSelectFlowOrgan.LoadData(ucSelectHROrgan.SelectedOrganID)
            UpdFlowOrgnaID.Update()
            ucSelectFlowOrgan.SetDefaultOrgan()
        Else
            '事業群
            txtGroupID.Text = objRG.QueryData("Organization", "And CompID = " & Bsp.Utility.Quote(hidCompID.Value) & " And OrganID = " & Bsp.Utility.Quote(ucSelectHROrgan.SelectedOrganID), "GroupID")
            txtGroupName.Text = objRG.QueryData("OrganizationFlow", "And OrganID = " & Bsp.Utility.Quote(txtGroupID.Text), "OrganName")

            '工作地點
            Using dt As DataTable = objRG.selectWorkSite(hidCompID.Value, ucSelectHROrgan.SelectedDeptID, ucSelectHROrgan.SelectedOrganID)
                If dt.Rows.Count > 0 Then
                    ViewState.Item("WorkSiteID") = dt.Rows(0).Item(0)
                End If
            End Using
            Bsp.Utility.SetSelectedIndex(ddlWorkSiteID, ViewState.Item("WorkSiteID"))

            '最小簽核單位
            ucSelectFlowOrgan.IsWait = False
            ucSelectFlowOrgan.ValidDate = ""
            ucSelectFlowOrgan.LoadData(ucSelectHROrgan.SelectedOrganID)
            UpdFlowOrgnaID.Update()
            ucSelectFlowOrgan.SetDefaultOrgan()
        End If

        '班別
        If ddlWTIDTypeFlag.SelectedValue = "1" Then
            ddlWTIDTypeFlag_SelectedIndexChanged(Nothing, Nothing)
        End If

        '職位
        lblSelectPosition.Text = ""
        lblSelectPositionName.Text = ""
        ddlPositionID.Items.Clear()
        ddlPositionID.Items.Insert(0, New ListItem("---請選擇---", ""))
        hidPositionID.Value = ""

        ''*工作性質
        lblSelectWorkTypeID.Text = ""
        lblSelectWorkTypeName.Text = ""
        ddlWorkTypeID.Items.Clear()
        ddlWorkTypeID.Items.Insert(0, New ListItem("---請選擇---", ""))
        hidWorkTypeID.Value = ""
    End Sub

    '員工姓名
    Protected Sub txtNameN_TextChanged(sender As Object, e As System.EventArgs) Handles txtNameN.TextChanged
        Dim objRG As New RG1
        Dim strNameN As String = txtNameN.Text.Trim.Replace(vbCrLf, "")
        Dim rtnName As String = ""

        If strNameN.Length > 0 Then
            For iLoop As Integer = 1 To Len(strNameN)
                If Bsp.Utility.getStringLength(Mid(strNameN, iLoop, 1)) = 1 And Not Char.IsLower(Mid(strNameN, iLoop, 1)) And Not Char.IsUpper(Mid(strNameN, iLoop, 1)) Then
                    Dim QName = objRG.QueryData("HRNameMap", "AND NameN = N" & Bsp.Utility.Quote(Mid(strNameN, iLoop, 1)), "Name")
                    If QName <> "" Then
                        rtnName = rtnName & QName
                    Else
                        rtnName = rtnName & "?"
                    End If
                Else
                    rtnName = rtnName & Mid(strNameN, iLoop, 1)
                End If
            Next
        End If

        If rtnName = "" Then
            txtName.Text = strNameN
        Else
            txtName.Text = rtnName
        End If
    End Sub

    '身分證字號IDNo
    Protected Sub btnIDNo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnIDNo.Click
        Dim strCapital As String
        Dim strSex As String
        Dim strSQL As New StringBuilder()

        If txtIDNo.Text <> "" Then
            '判斷性別
            If Len(txtIDNo.Text) >= 2 Then
                strCapital = Mid(txtIDNo.Text, 1, 1)
                strSex = Mid(txtIDNo.Text, 2, 1)

                If Asc(strCapital) >= Asc("A") And Asc(strCapital) <= Asc("Z") Then
                    If strSex = "1" Or strSex = "2" Then
                        ddlSex.SelectedValue = strSex
                    End If
                End If
            End If

            '身份證字號重複員工資料
            strSQL.AppendLine(" SELECT ")
            strSQL.AppendLine(" IDNo, P.EmpID, P.NameN as Name, P.WorkStatus, isnull(W.Remark, '') as WorkStatusName ")
            strSQL.AppendLine(" , P.CompID, isnull(C.CompName, '') as CompName ")
            strSQL.AppendLine(" , P.DeptID, isnull(O.OrganName, '') OrganName ")
            strSQL.AppendLine(" , EmpDate = Case When Convert(Char(10), P.EmpDate, 111) = '1900/01/01' Then '' ElSE Convert(Varchar, P.EmpDate, 120) End ")
            strSQL.AppendLine(" , QuitDate = Case When Convert(Char(10), P.QuitDate, 111) = '1900/01/01' Then '' ElSE Convert(Varchar, P.QuitDate, 120) End ")
            strSQL.AppendLine(" FROM Personal P")
            strSQL.AppendLine(" LEFT JOIN Organization O ON P.CompID = O.CompID AND P.DeptID = O.OrganID ")
            strSQL.AppendLine(" LEFT JOIN Company C ON P.CompID = C.CompID ")
            strSQL.AppendLine(" LEFT JOIN WorkStatus W ON P.WorkStatus = W.WorkCode ")
            strSQL.AppendLine(" WHERE ")
            strSQL.AppendLine(" P.IDNo = " & Bsp.Utility.Quote(txtIDNo.Text.ToUpper))
            strSQL.AppendLine(" ORDER BY P.EmpDate ")

            Using dt As DataTable = Bsp.DB.ExecuteDataSet(CommandType.Text, strSQL.ToString(), "eHRMSDB").Tables(0)
                If dt.Rows.Count > 0 Then
                    ReIDNo.Visible = True
                    gvMain.DataSource = dt
                    gvMain.DataBind()
                Else
                    ReIDNo.Visible = False
                End If
            End Using

            '取企業團到職日、學歷【最高學歷】、英文姓名、護照英文姓名欄位
            strSQL.Clear()
            strSQL.AppendLine(" SELECT ")
            strSQL.AppendLine(" SinopacEmpDate, EduID, EngName, PassportName ")
            strSQL.AppendLine(" FROM Personal P ")
            strSQL.AppendLine(" JOIN (SELECT Top 1 CompID, EmpID FROM EmployeeLog WHERE IDNo = " & Bsp.Utility.Quote(txtIDNo.Text.ToUpper) & " ORDER BY ModifyDate DESC) E ")
            strSQL.AppendLine(" ON P.CompID = E.CompID AND P.EmpID = E.EmpID ")
            Using dt As DataTable = Bsp.DB.ExecuteDataSet(CommandType.Text, strSQL.ToString(), "eHRMSDB").Tables(0)
                If dt.Rows.Count > 0 Then
                    '企業團到職日
                    If dt.Rows(0).Item(0).ToString().Trim <> "" Then
                        txtSinopacEmpDate.DateText = dt.Rows(0).Item(0).ToString()
                    End If

                    '學歷【最高學歷】
                    If dt.Rows(0).Item(1).ToString().Trim <> "" Then
                        ddlEduID.SelectedValue = dt.Rows(0).Item(1).ToString()
                    End If

                    '英文姓名
                    If dt.Rows(0).Item(2).ToString().Trim <> "" Then
                        txtEngName.Text = dt.Rows(0).Item(2).ToString()
                    End If

                    '護照英文姓名
                    If dt.Rows(0).Item(3).ToString().Trim <> "" Then
                        txtPassportName.Text = dt.Rows(0).Item(3).ToString()
                    End If
                End If
            End Using

            '查詢配偶狀態
            strSQL.Clear()
            strSQL.AppendLine(" SELECT RelativeID FROM Family WHERE IDNo = " & Bsp.Utility.Quote(txtIDNo.Text.ToUpper) & " AND RelativeID = '01' ")
            Using dt As DataTable = Bsp.DB.ExecuteDataSet(CommandType.Text, strSQL.ToString(), "eHRMSDB").Tables(0)
                If dt.Rows.Count > 0 Then
                    ddlMarriage.SelectedValue = "2"
                Else
                    ddlMarriage.SelectedValue = "1"
                End If
            End Using
        Else
            ReIDNo.Visible = False
        End If
    End Sub

    '試用期
    'Protected Sub ddlProbMonth_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlProbMonth.SelectedIndexChanged
    '    If ddlProbMonth.SelectedValue = "0" Then
    '        txtProbDate.Text = txtEmpDate.DateText
    '    Else
    '        txtProbDate.Text = ""
    '    End If
    'End Sub

    '金控職等
    Protected Sub ddlHoldingRankID_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlHoldingRankID.SelectedIndexChanged
        Dim strSQL As New StringBuilder()

        If ddlHoldingRankID.SelectedValue <> "" Then
            strSQL.AppendLine(" SELECT TitleName FROM TitleByHolding WHERE HoldingRankID = " & Bsp.Utility.Quote(ddlHoldingRankID.SelectedValue))

            Using dt As DataTable = Bsp.DB.ExecuteDataSet(CommandType.Text, strSQL.ToString(), "eHRMSDB").Tables(0)
                If dt.Rows.Count > 0 Then
                    txtHoldingTitle.Text = dt.Rows(0).Item(0).ToString()
                End If
            End Using
        Else
            txtHoldingTitle.Text = ""
        End If
    End Sub

    '金控職等
    Protected Sub chkAboriginalFlag_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkAboriginalFlag.CheckedChanged
        If chkAboriginalFlag.Checked Then
            ddlAboriginalTribe.Enabled = True
        Else
            ddlAboriginalTribe.Enabled = False
            ddlAboriginalTribe.SelectedValue = ""
        End If
    End Sub

    '班別
    Protected Sub ddlWTIDTypeFlag_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlWTIDTypeFlag.SelectedIndexChanged
        If ddlWTIDTypeFlag.SelectedValue = "0" Then
            Bsp.Utility.FillDDL(ddlWTID, "eHRMSDB", "WorkTime", "RTrim(WTID)", "BeginTime + '-' + EndTime", Bsp.Utility.DisplayType.Full, "", "AND CompID = " & Bsp.Utility.Quote(hidCompID.Value) & " AND WTIDTypeFlag='0' ")
        ElseIf ddlWTIDTypeFlag.SelectedValue = "1" Then
            Bsp.Utility.FillDDL(ddlWTID, "eHRMSDB", "OrgWorkTime RW", "RTrim(RW.WTID)", "ISNULL(W.BeginTime, '') + '-' + ISNULL(W.EndTime, '')", Bsp.Utility.DisplayType.Full, "Left Join WorkTime W ON RW.WTID = W.WTID AND RW.CompID = W.CompID", _
                                "AND RW.CompID = " & Bsp.Utility.Quote(hidCompID.Value) & " AND RW.DeptID = " & Bsp.Utility.Quote(ucSelectHROrgan.SelectedDeptID) & " AND RW.OrganID = " & Bsp.Utility.Quote(ucSelectHROrgan.SelectedOrganID) & " AND W.WTIDTypeFlag = '1'")
        Else
            ddlWTID.Items.Clear()
        End If
        ddlWTID.Items.Insert(0, New ListItem("---請選擇---", ""))
    End Sub

#Region "PositionID"    '職位
    '異動後資料-職位button
    Protected Sub ucPositionID_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ucSelectPosition.Load
        '載入按鈕-職位選單畫面
        ucSelectPosition.QueryCompID = hidCompID.Value
        ucSelectPosition.QueryEmpID = ""
        ucSelectPosition.DefaultPosition = lblSelectPosition.Text
        ucSelectPosition.QueryOrganID = ucSelectHROrgan.SelectedOrganID
        ucSelectPosition.Fields = New FieldState() { _
            New FieldState("PositionID", "職位代碼", True, True), _
            New FieldState("Remark", "職位名稱", True, True)}

        If (hidCompID.Value = "SPHSC1" Or hidCompID.Value = "SPHFC1" Or hidCompID.Value = "SPHICC") _
            And ucSelectHROrgan.SelectedOrganName.ToString().EndsWith("(未生效)") _
            And txtEmpDate.DateText <> "" And txtEmpDate.DateText <> "____/__/__" Then

            ucSelectPosition.IsWait = True
            ucSelectPosition.ValidDate = txtEmpDate.DateText
        Else
            ucSelectPosition.IsWait = False
            ucSelectPosition.ValidDate = ""
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
            lblSelectWorkTypeName.Text = strRstName1
        Else
            lblSelectPosition.Text = strRst1 + "," + strRst2
            hidPositionID.Value = strMainPosition + "|" + strPosition
            lblSelectPositionName.Text = strRstName1 + "," + strRstName2
        End If
    End Sub

    Public Function GetPositionID(ByVal strPositionID As String) As String
        Dim strWhere As String
        Dim strWherePosition As String
        Dim aryValue() As String = strPositionID.Split("|")
        Dim intCnt As Integer
        Dim strMainPosition As String '主要職位
        Dim objST As New ST1
        strMainPosition = ""
        Dim CompID As String = hidCompID.Value

        strWhere = "where CompID = " & Bsp.Utility.Quote(CompID)
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

        lblSelectPosition.Text = strWherePosition

        Try
            Using dt As Data.DataTable = objST.GetPositionID(strWhere).Tables(0)
                With ddlPositionID
                    .DataSource = dt
                    .DataTextField = "FullPositionName"
                    .DataValueField = "PositionID"
                    .DataBind()
                    .Items.Insert(0, New ListItem("---請選擇---", ""))
                End With
            End Using
            Return strMainPosition

        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me.Page, Bsp.Utility.getInnerException("ddlPositionID", ex))
            Return strMainPosition
        End Try
    End Function
#End Region

#Region "WorkType" '工作性質
    '異動後資料-工作性質button
    Protected Sub ucSelectWorkType_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ucSelectWorkType.Load
        '載入按鈕-工作性質選單畫面
        ucSelectWorkType.QueryCompID = hidCompID.Value
        ucSelectWorkType.QueryEmpID = ""
        ucSelectWorkType.DefaultWorkType = lblSelectWorkTypeID.Text
        ucSelectWorkType.QueryOrganID = ucSelectHROrgan.SelectedDeptID
        ucSelectWorkType.Fields = New FieldState() { _
            New FieldState("WorkTypeID", "工作性質代碼", True, True), _
            New FieldState("Remark", "工作性質名稱", True, True)}

        If (hidCompID.Value = "SPHSC1" Or hidCompID.Value = "SPHFC1" Or hidCompID.Value = "SPHICC") _
            And ucSelectHROrgan.SelectedOrganName.ToString().EndsWith("(未生效)") _
            And txtEmpDate.DateText <> "" And txtEmpDate.DateText <> "____/__/__" Then

            ucSelectWorkType.IsWait = True
            ucSelectWorkType.ValidDate = txtEmpDate.DateText
        Else
            ucSelectWorkType.IsWait = False
            ucSelectWorkType.ValidDate = ""
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
            lblSelectWorkTypeID.Text = strRst1
            hidWorkTypeID.Value = strMainWorkType
            lblSelectWorkTypeName.Text = strRstName1
        Else
            lblSelectWorkTypeID.Text = strRst1 + "," + strRst2
            hidWorkTypeID.Value = strMainWorkType + "|" + strWorkType
            lblSelectWorkTypeName.Text = strRstName1 + "," + strRstName2
        End If
    End Sub

    Public Function GetWorkTypeID(ByVal strWorkTypeID As String) As String
        Dim strWhere As String = "where CompID = " & Bsp.Utility.Quote(hidCompID.Value)
        Dim strWhereWorkType As String = ""
        Dim aryValue() As String = strWorkTypeID.Split("|")
        Dim intCnt As Integer
        Dim strMainWorkType As String = "" '主要工作性質
        Dim objST As New ST1

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

        lblSelectWorkTypeID.Text = strWhereWorkType

        Try
            Using dt As Data.DataTable = objST.GetWorkTypeID(strWhere).Tables(0)
                With ddlWorkTypeID
                    .DataSource = dt
                    .DataTextField = "FullWorkTypeName"
                    .DataValueField = "WorkTypeID"
                    .DataBind()
                    .Items.Insert(0, New ListItem("---請選擇---", ""))
                End With
            End Using

            Return strMainWorkType

        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me.Page, Bsp.Utility.getInnerException("ddlWorkTypeID", ex))
            Return strMainWorkType
        End Try
    End Function
#End Region

#Region "應試者編號帶出相關資料"
    Private Sub GetData_ucSelectRecruit()
        Dim objHR3000 As New HR3000
        Dim strSQL As New StringBuilder()

        strSQL.AppendLine("SELECT C.CompID")
        strSQL.AppendLine(", R.Name")
        strSQL.AppendLine(", R.NameN")
        strSQL.AppendLine(", R.NameB")
        strSQL.AppendLine(", R.IDNo")
        strSQL.AppendLine(", R.EngName")
        strSQL.AppendLine(", R.PassportName")
        strSQL.AppendLine(", BirthDate = Case When Convert(Char(10), R.BirthDate, 111) = '1900/01/01' Then '' ElSE Convert(Varchar, R.BirthDate, 111) END")
        strSQL.AppendLine(", R.Sex")
        strSQL.AppendLine(", R.NationID")
        strSQL.AppendLine(", IDExpireDate = Case When Convert(Char(10), R.IDExpireDate, 111) = '1900/01/01' Then '' ElSE Convert(Varchar, R.IDExpireDate, 111) END")
        strSQL.AppendLine(", R.EduID")
        strSQL.AppendLine(", R.Marriage")
        strSQL.AppendLine(", C.ProbMonth")
        strSQL.AppendLine(", ContractDate = Case When Convert(Char(10), C.ContractDate, 111) = '1900/01/01' Then '' ElSE Convert(Varchar, C.ContractDate, 111) END")
        strSQL.AppendLine(", ISNULL(S.FinalRankID, '') AS RankID")
        strSQL.AppendLine(", ISNULL(S.FinalTitleID, '') AS TitleID")
        strSQL.AppendLine(", C1.GroupID")
        strSQL.AppendLine(", C1.DeptID")
        strSQL.AppendLine(", C1.OrganID")
        strSQL.AppendLine(", ISNULL (STUFF ((SELECT ',''' + SEW.WorkTypeID + '''' FROM RE_EmpWorkType SEW")
        strSQL.AppendLine("WHERE C.CompID = SEW.CompID AND C.RecID = SEW.RecID AND C.CheckInDate = SEW.CheckInDate AND SEW.REType = '1'ORDER BY SEW.PrincipalFlag DESC, SEW.WorkTypeID")
        strSQL.AppendLine("FOR XML PATH('')), 1, 1, ''), '') AS WorkTypeID")
        strSQL.AppendLine(", ISNULL (STUFF ((SELECT ',' + SWT.Remark FROM RE_EmpWorkType SEW LEFT JOIN RE_WorkType SWT ON SWT.CompID = SEW.CompID AND SWT.WorkTypeID = SEW.WorkTypeID")
        strSQL.AppendLine("WHERE C.CompID = SEW.CompID AND C.RecID = SEW.RecID AND C.CheckInDate = SEW.CheckInDate AND SEW.REType = '1' ORDER BY SEW.PrincipalFlag DESC, SEW.WorkTypeID")
        strSQL.AppendLine("FOR XML PATH('')), 1, 1, ''), '') AS WorkType")
        strSQL.AppendLine(", ISNULL (STUFF ((SELECT ',''' + SEP.PositionID + '''' FROM RE_EmpPosition SEP")
        strSQL.AppendLine("WHERE C.CompID = SEP.CompID AND C.RecID = SEP.RecID AND C.CheckInDate = SEP.CheckInDate AND SEP.REType = 'F' ORDER BY SEP.PrincipalFlag DESC, SEP.PositionID")
        strSQL.AppendLine("FOR XML PATH('')), 1, 1, ''), '') AS PositionID")
        strSQL.AppendLine(", ISNULL (STUFF ((SELECT ',' + SP.Remark FROM RE_EmpPosition SEP LEFT JOIN RE_Position SP ON SP.CompID = SEP.CompID AND SP.PositionID = SEP.PositionID")
        strSQL.AppendLine("WHERE C.CompID = SEP.CompID AND C.RecID = SEP.RecID AND C.CheckInDate = SEP.CheckInDate AND SEP.REType = 'F' ORDER BY SEP.PrincipalFlag DESC, SEP.PositionID")
        strSQL.AppendLine("FOR XML PATH('')), 1, 1, ''), '') AS Position")
        strSQL.AppendLine(", C1.PassExamFlag")
        strSQL.AppendLine(", C1.WorkSiteID")
        strSQL.AppendLine("FROM RE_ContractData C")
        strSQL.AppendLine("LEFT JOIN RE_CheckInData C1 ON C.CompID = C1.CompID AND C.RecID = C1.RecID AND C.CheckInDate = C1.CheckInDate")
        strSQL.AppendLine("LEFT JOIN RE_Recruit R ON C.RecID = R.RecID")
        strSQL.AppendLine("LEFT JOIN RE_SalaryData S ON C.CompID = S.CompID AND C.RecID = S.RecID AND C.CheckInDate = S.CheckInDate")
        strSQL.AppendLine("WHERE 1 = 1 ")
        strSQL.AppendLine("AND C.CompID = " & Bsp.Utility.Quote(hidReturnCompID.Value))
        strSQL.AppendLine("AND C.RecID = " & Bsp.Utility.Quote(txtRecID.Text))
        strSQL.AppendLine("AND C.CheckInDate = " & Bsp.Utility.Quote(hidCheckInDate.Value))

        Using dt As DataTable = Bsp.DB.ExecuteDataSet(CommandType.Text, strSQL.ToString(), "Recruit").Tables(0)
            If dt.Rows.Count > 0 Then
                txtName.Text = dt.Rows(0).Item("Name").ToString().Trim
                txtNameN.Text = dt.Rows(0).Item("NameN").ToString().Trim
                txtNameB.Text = dt.Rows(0).Item("NameB").ToString().Trim
                txtEngName.Text = dt.Rows(0).Item("EngName").ToString().Trim
                txtPassportName.Text = dt.Rows(0).Item("PassportName").ToString().Trim
                ddlSex.SelectedValue = dt.Rows(0).Item("Sex").ToString().Trim
                txtIDNo.Text = dt.Rows(0).Item("IDNo").ToString().Trim
                txtBirthDate.DateText = dt.Rows(0).Item("BirthDate").ToString().Trim
                ddlNationID.SelectedValue = dt.Rows(0).Item("NationID").ToString().Trim
                txtIDExpireDate.DateText = dt.Rows(0).Item("IDExpireDate").ToString().Trim
                ddlMarriage.SelectedValue = dt.Rows(0).Item("Marriage").ToString().Trim
                ddlEduID.SelectedValue = dt.Rows(0).Item("EduID").ToString().Trim
                ddlProbMonth.SelectedValue = dt.Rows(0).Item("ProbMonth").ToString().Trim
                txtEmpDate.DateText = dt.Rows(0).Item("ContractDate").ToString().Trim
                txtSinopacEmpDate.DateText = dt.Rows(0).Item("ContractDate").ToString().Trim
                txtRankBeginDate.DateText = dt.Rows(0).Item("ContractDate").ToString().Trim
                '職等/職稱
                If dt.Rows(0).Item("RankID").ToString().Trim <> "" Then
                    ucSelectRankAndTitle.LoadData(hidCompID.Value, "U")
                    ucSelectRankAndTitle.setRankID(hidCompID.Value, dt.Rows(0).Item("RankID").ToString().Trim, "U")
                    ucSelectRankAndTitle.setTitleID(hidCompID.Value, dt.Rows(0).Item("RankID").ToString().Trim, dt.Rows(0).Item("TitleID").ToString().Trim, "U")
                End If
                txtGroupID.Text = dt.Rows(0).Item("GroupID").ToString().Trim
                txtGroupName.Text = objHR3000.Get_GroupInfo(dt.Rows(0).Item("GroupID").ToString().Trim)
                '部門/科組課
                ucSelectHROrgan.LoadData(hidCompID.Value)
                ucSelectHROrgan.setDeptID(hidCompID.Value, dt.Rows(0).Item("DeptID").ToString().Trim, "")
                ucSelectHROrgan.setOrganID(hidCompID.Value, dt.Rows(0).Item("OrganID").ToString().Trim, "")
                '工作性質
                If dt.Rows(0).Item("WorkTypeID").ToString().Trim <> "" Then
                    lblSelectWorkTypeID.Text = dt.Rows(0).Item("WorkTypeID").ToString().Trim
                    lblSelectWorkTypeName.Text = dt.Rows(0).Item("WorkType").ToString().Trim

                    Bsp.Utility.WorkType(ddlWorkTypeID, "WorkTypeID", , " and WorkTypeID in (" + lblSelectWorkTypeID.Text + ") and CompID = '" + hidCompID.Value + "'")

                    '第一筆為主要工作性質
                    Dim strDefaultValue() As String = lblSelectWorkTypeID.Text.Replace("'", "").Split(",")
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
                Else
                    ddlWorkTypeID.Items.Clear()
                    ddlWorkTypeID.Items.Insert(0, New ListItem("---請選擇---", ""))
                    hidWorkTypeID.Value = ""
                End If
                '職位
                If dt.Rows(0).Item("PositionID").ToString().Trim <> "" Then
                    lblSelectPosition.Text = dt.Rows(0).Item("PositionID").ToString().Trim
                    lblSelectPositionName.Text = dt.Rows(0).Item("Position").ToString().Trim

                    Bsp.Utility.Position(ddlPositionID, "PositionID", , " and PositionID in (" + lblSelectPosition.Text + ") and CompID = '" + hidCompID.Value + "'")
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
                Else
                    ddlPositionID.Items.Clear()
                    ddlPositionID.Items.Insert(0, New ListItem("---請選擇---", ""))
                    hidPositionID.Value = ""
                End If
                '新員招考註記
                If dt.Rows(0).Item("PassExamFlag").ToString().Trim = "1" Then
                    chkPassExamFlag.Checked = True
                ElseIf dt.Rows(0).Item("PassExamFlag").ToString().Trim = "0" Then
                    chkPassExamFlag.Checked = False
                End If

                '工作地點
                ddlWorkSiteID.SelectedValue = dt.Rows(0).Item("WorkSiteID").ToString().Trim   '2015/12/01 Modify

                '最小簽核單位
                ucSelectFlowOrgan.LoadData(ucSelectHROrgan.SelectedOrganID)
                UpdFlowOrgnaID.Update()
            End If
        End Using
    End Sub
#End Region
End Class
