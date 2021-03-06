'****************************************************
'功能說明：員工基本資料修改
'建立人員：Micky Sung
'建立日期：2015.09.24
'****************************************************
Imports System.Data

Partial Class ST_ST1100
    Inherits PageBase
    Public Shared arr_All As New List(Of ArrayList)()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then

        End If
    End Sub
    Protected Overrides Sub BaseOnPageTransfer(ByVal ti As TransferInfo)
        If Not IsPostBack() Then
            Dim ht As Hashtable = Bsp.Utility.getHashTableFromParam(ti.Args)
            If ht.ContainsKey("SelectedCompID") Then
                ViewState.Item("CompID") = ht("SelectedCompID").ToString()
                ViewState.Item("CompName") = ht("SelectedCompName").ToString()
                ViewState.Item("EmpID") = ht("SelectedEmpID").ToString()
                ViewState.Item("EmpName") = ht("SelectedEmpName").ToString()
                ViewState.Item("IDNo") = ht("SelectedIDNo").ToString()

                txtCompID.Text = ViewState.Item("CompID").ToString + "-" + ViewState.Item("CompName")
                txtEmpID.Text = ViewState.Item("EmpID").ToString
                hidCompID.Value = ViewState.Item("CompID").ToString

                txtIDNo.Attributes.Add("onchange", "changeIDNo()")

                subLoadData()
                subGetData(ViewState.Item("CompID").ToString, ViewState.Item("EmpID").ToString)
            Else
                Return
            End If
        End If
    End Sub

    Public Overrides Sub DoAction(ByVal Param As String)
        Select Case Param
            Case "btnUpdate"   '存檔返回
                If funCheckData() Then
                    If ViewState.Item("needRelease") Then
                        Release("btnUpdate")
                    Else
                        Bsp.Utility.RunClientScript(Me, "confirmUpdate()")
                    End If
                End If
        End Select
    End Sub

    Private Sub subLoadData()
        Dim objHR As New HR()

        ViewState.Item("CheckInFileCompID") = objHR.funGetCheckInFileCompID(hidCompID.Value) '報到文件歸屬公司代碼
        ViewState.Item("IsRankIDMapFlag") = objHR.IsRankIDMapFlag(hidCompID.Value) '此公司是否有導入惠悅專案的註記
        ViewState.Item("PayrollCompID") = objHR.funGetPayrollCompID(hidCompID.Value) '計薪作業歸屬公司代碼

        '*部門/*科組課
        ucSelectHROrgan.LoadData(hidCompID.Value, "Y")

        '職等/職稱
        ucSelectRankAndTitle.LoadData(hidCompID.Value, "U")

        '學歷
        Bsp.Utility.FillDDL(ddlEduID, "eHRMSDB", "EduDegree", "RTrim(EduID)", "EduName", Bsp.Utility.DisplayType.Full, "", "", "Order by EduID")
        ddlEduID.Items.Insert(0, New ListItem("---請選擇---", ""))

        '任職狀況
        Bsp.Utility.FillDDL(ddlWorkStatus, "eHRMSDB", "WorkStatus", "RTrim(WorkCode)", "Remark", Bsp.Utility.DisplayType.Full, "", "", "Order by WorkCode")
        ddlWorkStatus.Items.Insert(0, New ListItem("---請選擇---", ""))

        '僱用類別
        Bsp.Utility.FillDDL(ddlEmpType, "eHRMSDB", "HRCodeMap", "RTrim(Code)", "CodeCName", Bsp.Utility.DisplayType.Full, "", " AND TabName = 'Personal' and FldName = 'EmpType' and NotShowFlag = '0'", "Order by SortFld")
        ddlEmpType.Items.Insert(0, New ListItem("---請選擇---", ""))

        '試用期
        Bsp.Utility.FillDDL(ddlProbMonth, "eHRMSDB", "HRCodeMap", "RTrim(Code)", "RTrim(Code) + CodeCName", Bsp.Utility.DisplayType.OnlyName, "", " AND TabName = 'Personal' and FldName = 'ProbMonth' and NotShowFlag = '0'", "Order by SortFld")

        '金控職等
        Bsp.Utility.FillDDL(ddlHoldingRankID, "eHRMSDB", "Rank", "RTrim(RankID)", "", Bsp.Utility.DisplayType.OnlyID, "", " AND CompID = 'SPHOLD'", "Order by RankID")
        ddlHoldingRankID.Items.Insert(0, New ListItem("---請選擇---", ""))

        '對外職稱
        Bsp.Utility.FillDDL(ddlPublicTitleID, "eHRMSDB", "PublicTitle", "RTrim(PublicTitleID)", "PublicTitleName", Bsp.Utility.DisplayType.Full, "", "", "Order by PublicTitleID")
        ddlPublicTitleID.Items.Insert(0, New ListItem("---請選擇---", ""))

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
        Bsp.Utility.FillDDL(ddlAboriginalTribe, "eHRMSDB", "HRCodeMap", "RTrim(Code)", "CodeCName", Bsp.Utility.DisplayType.Full, "", " AND TabName = 'PersonalOther' and FldName = 'AboriginalTribe' and NotShowFlag = '0'", "Order by SortFld")
        ddlAboriginalTribe.Items.Insert(0, New ListItem("---請選擇---", ""))

        '職位
        ddlPositionID.Items.Insert(0, New ListItem("---請選擇---", ""))

        '工作性質
        ddlWorkTypeID.Items.Insert(0, New ListItem("---請選擇---", ""))

        '2015/12/14 Add 新增下拉選單:證件類型
        Bsp.Utility.FillDDL(ddlIDType, "eHRMSDB", "HRCodeMap", "RTrim(Code)", "CodeCName", Bsp.Utility.DisplayType.Full, "", " AND TabName = 'Personal' and FldName = 'IDType' and NotShowFlag = '0' ", "order by SortFld")
        ddlIDType.Items.Insert(0, New ListItem("---請選擇---", ""))

        If Not ViewState.Item("IsRankIDMapFlag") Then
            ddlPositionID.Enabled = False
            ucSelectPosition.Enabled = False
        End If

        If ViewState.Item("CompID") = "SPHSC1" Or ViewState.Item("CompID") = "SPHFC1" Or ViewState.Item("CompID") = "SPHICC" Then
            txtProbDate.Enabled = True
        End If

        '需刷卡註記
        If ViewState.Item("PayrollCompID") = "SPHSC1" Then
            chkOfficeLoginFlag.Enabled = True
        Else
            chkOfficeLoginFlag.Enabled = False
        End If

        '2015/12/14 Add 其他證件號碼
        btnOtherIDNo.Visible = True
        btnOtherIDNoInsert.Visible = False
        btnOtherIDNoUpdate.Visible = False
        btnOtherIDNoCancel.Visible = False
        lblOtherIDNo.Visible = False
        txtOtherIDNo.Visible = False
        lblOtherIDType.Visible = False
        ddlOtherIDType.Visible = False
        lblOtherIDExpireDate.Visible = False
        txtOtherIDExpireDate.Visible = False
        arr_All.Clear()

        '2015/12/14 Add 新增下拉選單:其他證件號碼 證件類型
        Bsp.Utility.FillDDL(ddlOtherIDType, "eHRMSDB", "HRCodeMap", "RTrim(Code)", "CodeCName", Bsp.Utility.DisplayType.Full, "", " AND TabName = 'Personal' and FldName = 'IDType' and NotShowFlag = '0' ", "order by SortFld")
        ddlOtherIDType.Items.Insert(0, New ListItem("---請選擇---", ""))
    End Sub

    Private Sub subGetData(ByVal CompID As String, ByVal EmpID As String)
        ViewState.Item("IsChkIDNo") = False
        ViewState.Item("needRelease") = False '是否需要放行
        ViewState.Item("OtherIDNoOld") = ""   '其他證件代號
        ViewState.Item("OtherIDTypeOld") = ""   '其他證件類型

        Dim objST As New ST1()
        Dim objSC As New SC()
        Dim objHR As New HR()

        Dim strSQL As New StringBuilder()

        Try
            imgPhoto.ImageUrl = New ST2().EmpPhotoQuery(CompID, EmpID)
            imgPhoto.Visible = True
            lblPhoto_NoPic.Visible = False
        Catch ex As Exception
            imgPhoto.ImageUrl = ""
            imgPhoto.Visible = False
            lblPhoto_NoPic.Visible = True
        End Try

        Try
            Using dt As DataTable = objST.subGetData_ST1100(hidCompID.Value, txtEmpID.Text)
                If dt.Rows().Count > 0 Then
                    For Each dr As DataRow In dt.Rows
                        hidRecID.Value = dr.Item("RecID").ToString.Trim
                        txtEmpID.Text = dr.Item("EmpID").ToString.Trim
                        txtNameN.Text = dr.Item("NameN").ToString.Trim
                        txtName.Text = dr.Item("Name").ToString.Trim
                        txtNameB.Text = dr.Item("NameB").ToString.Trim

                        '身分證字號
                        txtIDNo.Text = dr.Item("IDNo").ToString.Trim
                        hidIDNo.Value = dr.Item("IDNo").ToString.Trim

                        txtEngName.Text = dr.Item("EngName").ToString.Trim
                        txtPassportName.Text = dr.Item("PassportName").ToString.Trim

                        '生日
                        txtBirthDate.DateText = dr.Item("BirthDate").ToString.Trim
                        hidBirthDate.Value = dr.Item("BirthDate").ToString.Trim

                        ddlSex.SelectedValue = dr.Item("Sex").ToString.Trim

                        '國籍
                        ddlNationID.SelectedValue = dr.Item("NationID").ToString.Trim
                        hidNationID_Old.Value = dr.Item("NationID").ToString.Trim

                        '工作證期限
                        txtIDExpireDate.DateText = IIf(dr.Item("IDExpireDate").ToString.Trim = "1900/01/01", "", dr.Item("IDExpireDate").ToString.Trim)

                        ddlEduID.SelectedValue = dr.Item("EduID").ToString.Trim
                        ddlMarriage.SelectedValue = dr.Item("Marriage").ToString.Trim

                        '離職日(公司)
                        If dr.Item("QuitDate").ToString.Trim = "1900/01/01" Or dr.Item("WorkStatus").ToString.Trim = "1" Then
                            txtQuitDate.DateText = ""
                        Else
                            txtQuitDate.DateText = dr.Item("QuitDate").ToString.Trim()
                        End If

                        '離職日(企業團)
                        If dr.Item("SinopacQuitDate").ToString.Trim = "1900/01/01" Or dr.Item("WorkStatus").ToString.Trim = "1" Then
                            txtSinopacQuitDate.DateText = ""
                        Else
                            txtSinopacQuitDate.DateText = dr.Item("SinopacQuitDate").ToString.Trim()
                        End If

                        ddlWorkStatus.SelectedValue = dr.Item("WorkStatus").ToString.Trim
                        ddlEmpType.SelectedValue = dr.Item("EmpType").ToString.Trim

                        '年資(公司)
                        txtEmpTotSen.Text = dr.Item("TotSen").ToString.Trim
                        '年資(企業團)
                        txtEmpTotSen_SPHOLD.Text = dr.Item("TotSen_SPHOLD").ToString.Trim

                        '*到職日
                        txtEmpDate.DateText = IIf(dr.Item("EmpDate").ToString.Trim = "1900/01/01", "", dr.Item("EmpDate").ToString.Trim)

                        txtSinopacEmpDate.DateText = dr.Item("SinopacEmpDate").ToString.Trim
                        ddlProbMonth.SelectedValue = dr.Item("ProbMonth").ToString.Trim

                        '試用考核試滿日
                        txtProbDate.DateText = IIf(dr.Item("ProbDate").ToString.Trim = "1900/01/01", "", dr.Item("ProbDate").ToString.Trim)

                        '*職等/*職稱
                        ucSelectRankAndTitle.LoadData(hidCompID.Value, "U")
                        ucSelectRankAndTitle.setRankID(hidCompID.Value, dr.Item("RankID").ToString.Trim, "U")
                        ucSelectRankAndTitle.setTitleID(hidCompID.Value, dr.Item("RankID").ToString.Trim, dr.Item("TitleID").ToString.Trim, "U")
                        hidRankID.Value = dr.Item("RankID").ToString.Trim
                        hidTitleID.Value = dr.Item("TitleID").ToString.Trim

                        ddlHoldingRankID.SelectedValue = dr.Item("HoldingRankID").ToString.Trim
                        txtHoldingTitle.Text = dr.Item("HoldingTitle").ToString.Trim
                        ddlPublicTitleID.SelectedValue = dr.Item("PublicTitleID").ToString.Trim

                        '*最近升遷日\本階起始日
                        txtRankBeginDate.DateText = IIf(dr.Item("RankBeginDate").ToString.Trim = "1900/01/01", "", dr.Item("RankBeginDate").ToString.Trim)

                        '事業群
                        txtGroupID.Text = dr.Item("GroupID").ToString.Trim
                        txtGroupName.Text = dr.Item("GroupName").ToString.Trim

                        '*部門/*科組課
                        ucSelectHROrgan.LoadData(hidCompID.Value, "Y")
                        ucSelectHROrgan.setDeptID(hidCompID.Value, dr.Item("DeptID").ToString.Trim, "Y")
                        ucSelectHROrgan.setOrganID(hidCompID.Value, dr.Item("OrganID").ToString.Trim, "Y")
                        hidDeptID.Value = dr.Item("DeptID").ToString.Trim
                        hidOrganID.Value = dr.Item("OrganID").ToString.Trim

                        '職位
                        strSQL = New StringBuilder()
                        strSQL.AppendLine("SELECT E.PositionID, ISNULL(P.Remark, '') Position")
                        strSQL.AppendLine("FROM EmpPosition E ")
                        strSQL.AppendLine("LEFT JOIN Position P on P.CompID = E.CompID and P.PositionID = E.PositionID")
                        strSQL.AppendLine("WHERE 1 = 1 ")
                        strSQL.AppendLine("AND E.CompID = " & Bsp.Utility.Quote(hidCompID.Value))
                        strSQL.AppendLine("AND E.EmpID = " & Bsp.Utility.Quote(txtEmpID.Text))
                        strSQL.AppendLine("ORDER BY E.PrincipalFlag DESC, E.PositionID")

                        Using dtPos As DataTable = Bsp.DB.ExecuteDataSet(CommandType.Text, strSQL.ToString(), "eHRMSDB").Tables(0)
                            If dtPos.Rows.Count > 0 Then
                                ddlPositionID.Items.Clear()
                                hidPositionID.Value = ""
                                lblSelectPositionID.Text = ""
                                lblSelectPositionName.Text = ""

                                For i As Integer = 0 To dtPos.Rows.Count - 1
                                    ddlPositionID.Items.Add(New ListItem(dtPos.Rows(i).Item(0).ToString() + "-" + dtPos.Rows(i).Item(1).ToString(), dtPos.Rows(i).Item(0).ToString()))

                                    If i = 0 Then
                                        hidPositionID.Value += "|1|"
                                    Else
                                        hidPositionID.Value += "|0|"
                                    End If
                                    hidPositionID.Value += dtPos.Rows(i).Item(0).ToString()
                                    lblSelectPositionID.Text += ",'" + dtPos.Rows(i).Item(0).ToString() + "'"
                                    lblSelectPositionName.Text += "," + dtPos.Rows(i).Item(1).ToString()
                                Next

                                hidPositionID.Value = hidPositionID.Value.Substring(1)
                                hidPositionID_Old.Value = hidPositionID.Value
                                lblSelectPositionID.Text = lblSelectPositionID.Text.Substring(1)
                                lblSelectPositionName.Text = lblSelectPositionName.Text.Substring(1)
                            Else
                                ddlPositionID.Items.Clear()
                                ddlPositionID.Items.Insert(0, New ListItem("---請選擇---", ""))
                                hidPositionID.Value = ""
                                hidPositionID_Old.Value = ""
                            End If
                        End Using


                        '*工作性質
                        strSQL = New StringBuilder()
                        strSQL.AppendLine("SELECT E.WorkTypeID, ISNULL(P.Remark, '') WorkType")
                        strSQL.AppendLine("FROM EmpWorkType E ")
                        strSQL.AppendLine("LEFT JOIN WorkType P on P.CompID = E.CompID and P.WorkTypeID = E.WorkTypeID")
                        strSQL.AppendLine("WHERE 1 = 1 ")
                        strSQL.AppendLine("AND E.CompID = " & Bsp.Utility.Quote(hidCompID.Value))
                        strSQL.AppendLine("AND E.EmpID = " & Bsp.Utility.Quote(txtEmpID.Text))
                        strSQL.AppendLine("ORDER BY E.PrincipalFlag DESC, E.WorkTypeID")

                        Using dtWor As DataTable = Bsp.DB.ExecuteDataSet(CommandType.Text, strSQL.ToString(), "eHRMSDB").Tables(0)
                            If dtWor.Rows.Count > 0 Then
                                ddlWorkTypeID.Items.Clear()
                                hidWorkTypeID.Value = ""
                                lblSelectWorkTypeID.Text = ""
                                lblSelectWorkTypeName.Text = ""

                                For i As Integer = 0 To dtWor.Rows.Count - 1
                                    ddlWorkTypeID.Items.Add(New ListItem(dtWor.Rows(i).Item(0).ToString() + "-" + dtWor.Rows(i).Item(1).ToString(), dtWor.Rows(i).Item(0).ToString()))

                                    If i = 0 Then
                                        hidWorkTypeID.Value += "|1|"
                                    Else
                                        hidWorkTypeID.Value += "|0|"
                                    End If
                                    hidWorkTypeID.Value += dtWor.Rows(i).Item(0).ToString()
                                    lblSelectWorkTypeID.Text += ",'" + dtWor.Rows(i).Item(0).ToString() + "'"
                                    lblSelectWorkTypeName.Text += "," + dtWor.Rows(i).Item(1).ToString()
                                Next

                                hidWorkTypeID.Value = hidWorkTypeID.Value.Substring(1)
                                hidWorkTypeID_Old.Value = hidWorkTypeID.Value
                                lblSelectWorkTypeID.Text = lblSelectWorkTypeID.Text.Substring(1)
                                lblSelectWorkTypeName.Text = lblSelectWorkTypeName.Text.Substring(1)
                            Else
                                ddlWorkTypeID.Items.Clear()
                                ddlWorkTypeID.Items.Insert(0, New ListItem("---請選擇---", ""))
                                hidWorkTypeID.Value = ""
                                hidWorkTypeID_Old.Value = ""
                            End If
                        End Using

                        '工作地點
                        ddlWorkSiteID.SelectedValue = dr.Item("WorkSiteID").ToString.Trim
                        hidWorkSiteID.Value = dr.Item("WorkSiteID").ToString.Trim

                        '班別
                        ddlWTIDTypeFlag.SelectedValue = dr.Item("WTIDTypeFlag").ToString.Trim
                        ddlWTIDTypeFlag_SelectedIndexChanged(Nothing, Nothing)
                        Bsp.Utility.SetSelectedIndex(ddlWTID, dr.Item("WTID").ToString.Trim)

                        'LocalHire註記
                        If dr.Item("LocalHireFlag").ToString.Trim = "1" Then
                            chkLocalHireFlag.Checked = True
                        Else
                            chkLocalHireFlag.Checked = False
                        End If

                        '新員招考註記
                        If dr.Item("PassExamFlag").ToString.Trim = "1" Then
                            chkPassExamFlag.Checked = True
                        Else
                            chkPassExamFlag.Checked = False
                        End If

                        '需刷卡註記
                        If dr.Item("OfficeLoginFlag").ToString.Trim = "1" Then
                            chkOfficeLoginFlag.Checked = True
                        Else
                            chkOfficeLoginFlag.Checked = False
                        End If

                        '原住民註記/族別
                        If dr.Item("AboriginalFlag").ToString.Trim = "1" Then
                            chkAboriginalFlag.Checked = True
                            ddlAboriginalTribe.SelectedValue = dr.Item("AboriginalTribe").ToString.Trim
                        Else
                            chkAboriginalFlag.Checked = False
                            ddlAboriginalTribe.SelectedValue = ""
                            ddlAboriginalTribe.Enabled = False
                        End If

                        '*最小簽核單位
                        ucSelectFlowOrgan.LoadData(ucSelectHROrgan.SelectedOrganID, "Y")
                        ucSelectFlowOrgan.setOrganID(dr.Item("FlowOrganID").ToString.Trim, "Y")
                        hidFlowOrganID_Old.Value = dr.Item("FlowOrganID").ToString.Trim

                        '最後異動公司
                        Using dt1 As DataTable = objHR.GetHRCompName(dr.Item("LastChgComp").ToString.Trim)
                            If dt1.Rows.Count = 0 Then
                                lblLastChgComp.Text = dr.Item("LastChgComp").ToString.Trim
                            Else
                                lblLastChgComp.Text = dr.Item("LastChgComp").ToString.Trim & "-" & dt1.Rows(0).Item("CompName").ToString()
                            End If
                        End Using

                        '最後異動者
                        Using dt1 As DataTable = objHR.GetHREmpName(dr.Item("LastChgComp").ToString.Trim, dr.Item("LastChgID").ToString.Trim)
                            If dt1.Rows.Count = 0 Then
                                lblLastChgID.Text = dr.Item("LastChgID").ToString.Trim
                            Else
                                lblLastChgID.Text = dr.Item("LastChgID").ToString.Trim & "-" & dt1.Rows(0).Item("NameN").ToString()
                            End If
                        End Using

                        '最後異動日期
                        lblLastChgDate.Text = IIf(Format(dr.Item("LastChgDate").ToString.Trim, "yyyy/MM/dd") = "1900/01/01", "", dr.Item("LastChgDate").ToString.Replace("-", "/"))

                        '最後異動人員
                        If dr.Item("LastChgID").ToString.Trim <> "" Then
                            Dim UserName As String = objSC.GetSC_UserName(dr.Item("LastChgComp").ToString.Trim, dr.Item("LastChgID").ToString.Trim)
                            lblLastChgID.Text = dr.Item("LastChgID").ToString.Trim + IIf(UserName <> "", "-" + UserName, "")
                        Else
                            lblLastChgID.Text = ""
                        End If

                        '2015/12/14 Add 證件類型
                        ddlIDType.SelectedValue = dr.Item("IDType").ToString.Trim
                        hidIDType.Value = dr.Item("IDType").ToString.Trim
                    Next
                End If
            End Using


            '2015/12/14 Add 其他證件號碼
            gvOtherIDNo.Visible = True

            strSQL = New StringBuilder()
            strSQL.AppendLine(" SELECT E.OtherIDNo AS OtherIDNo, E.IDType + '-' + H.CodeCName AS OtherIDTypeName, Convert(varchar(10),E.IDExpireDate,111) AS OtherIDExpireDate, E.IDType AS OtherIDType ")
            strSQL.AppendLine(" FROM EmpOtherIDNo E ")
            strSQL.AppendLine(" LEFT JOIN HRCodeMap H ON H.Code = E.IDType AND H.TabName = 'Personal' and H.FldName = 'IDType' and H.NotShowFlag = '0' ")
            strSQL.AppendLine(" WHERE 1 = 1 ")
            strSQL.AppendLine(" AND IDNo = " & Bsp.Utility.Quote(hidIDNo.Value.ToString()))

            Using dt As DataTable = Bsp.DB.ExecuteDataSet(CommandType.Text, strSQL.ToString(), "eHRMSDB").Tables(0)
                gvOtherIDNo.DataSource = dt
                gvOtherIDNo.DataBind()

                If dt.Rows.Count > 0 Then
                    For Each dr As DataRow In dt.Rows
                        Dim arr As New ArrayList()
                        arr.Add(dr.Item(0).ToString())
                        arr.Add(dr.Item(1).ToString())
                        arr.Add(dr.Item(2).ToString())
                        arr.Add(dr.Item(3).ToString())
                        arr_All.Add(arr)
                    Next
                End If
            End Using

        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & "subGetData", ex)
        End Try
    End Sub

    Private Function SaveData() As Boolean
        Dim objHR As New HR()
        Dim objST As New ST1()
        Dim objSC As New SC()
        Dim objRegistData As New RegistData()
        Dim strSQL As New StringBuilder()

        Dim bePersonal As New bePersonal.Row()
        Dim bsPersonal As New bePersonal.Service()
        Dim bePersonalOther As New bePersonalOther.Row()
        Dim beEmpWorkTypeRows() As beEmpWorkType.Row = Nothing
        Dim beEmpPositionRows() As beEmpPosition.Row = Nothing
        Dim beEmpFlow As New beEmpFlow.Row()
        Dim beCommunication As New beCommunication.Row()
        Dim beEmpOtherIDNoRows() As beEmpOtherIDNo.Row = Nothing '2015/12/15 Add 新增Table

        Dim intCount As Integer = 0
        Dim IsUpdPersonalOther As Boolean = False

        '儲存資料
        Try
            '1. 新增員工基本資料[Personal]員工檔
            bePersonal.CompID.Value = hidCompID.Value
            bePersonal.EmpID.Value = txtEmpID.Text
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
            bePersonal.WorkStatus.Value = ddlWorkStatus.SelectedValue
            bePersonal.EmpType.Value = ddlEmpType.SelectedValue
            bePersonal.GroupID.Value = txtGroupID.Text.Trim
            bePersonal.DeptID.Value = ucSelectHROrgan.SelectedDeptID
            bePersonal.OrganID.Value = ucSelectHROrgan.SelectedOrganID
            bePersonal.WorkSiteID.Value = ddlWorkSiteID.SelectedValue
            bePersonal.RankID.Value = ucSelectRankAndTitle.SelectedRankID
            bePersonal.RankIDMap.Value = objHR.FunGetRankIDMap(hidCompID.Value, ucSelectRankAndTitle.SelectedRankID)
            bePersonal.HoldingRankID.Value = ddlHoldingRankID.SelectedValue
            bePersonal.TitleID.Value = ucSelectRankAndTitle.SelectedTitleID
            bePersonal.PublicTitleID.Value = ddlPublicTitleID.SelectedValue
            bePersonal.EmpDate.Value = IIf(txtEmpDate.DateText = "", "1900/01/01", txtEmpDate.DateText)
            bePersonal.SinopacEmpDate.Value = IIf(txtSinopacEmpDate.DateText = "", "1900/01/01", txtSinopacEmpDate.DateText)
            bePersonal.ProbDate.Value = IIf(txtProbDate.DateText = "", "1900/01/01", txtProbDate.DateText)
            bePersonal.ProbMonth.Value = ddlProbMonth.SelectedValue
            bePersonal.IsHLBFlag.Value = 0  '2015/11/30 Add
            bePersonal.PassExamFlag.Value = IIf(chkPassExamFlag.Checked, "1", "0")
            bePersonal.LocalHireFlag.Value = IIf(chkLocalHireFlag.Checked, "1", "0")
            bePersonal.RankBeginDate.Value = IIf(txtRankBeginDate.DateText = "", "1900/01/01", txtRankBeginDate.DateText)
            bePersonal.LastChgComp.Value = UserProfile.ActCompID
            bePersonal.LastChgID.Value = UserProfile.ActUserID
            bePersonal.LastChgDate.Value = Now
            bePersonal.QuitDate.Value = IIf(txtQuitDate.DateText = "", "1900/01/01", txtQuitDate.DateText)
            bePersonal.SinopacQuitDate.Value = IIf(txtSinopacQuitDate.DateText = "", "1900/01/01", txtSinopacQuitDate.DateText)
            bePersonal.IDType.Value = ddlIDType.SelectedValue   '2015/12/15 Add 證件類型

            '2015/12/15 Add EmpOtherIDNo 員工次要證號檔
            If arr_All.Count > 0 Then
                intCount = 0
                For Each item In arr_All
                    ReDim Preserve beEmpOtherIDNoRows(intCount)
                    Dim beEmpOtherIDNo As New beEmpOtherIDNo.Row()
                    beEmpOtherIDNo.IDNo.Value = txtIDNo.Text.ToUpper
                    beEmpOtherIDNo.OtherIDNo.Value = IIf(item(3) = "1", item(0).ToUpper, item(0))
                    beEmpOtherIDNo.IDType.Value = item(3)
                    beEmpOtherIDNo.IDExpireDate.Value = IIf(item(2).ToString.Trim = "", "1900/01/01", item(2).ToString.Trim)
                    beEmpOtherIDNo.LastChgComp.Value = UserProfile.ActCompID
                    beEmpOtherIDNo.LastChgID.Value = UserProfile.ActUserID
                    beEmpOtherIDNo.LastChgDate.Value = Now

                    beEmpOtherIDNoRows(intCount) = beEmpOtherIDNo
                    intCount = intCount + 1
                Next
            End If

            '2. 如果有異動身分證IDNo(IDNo <> IDNo_Old)，需同步修改跟IDNo相關的所有Table的IDNo欄位值
            Dim UserName As String = objSC.GetSC_UserName(UserProfile.ActCompID, UserProfile.ActUserID)

            '3. 新增/修改員工特殊設定資料檔[PersonalOther]
            'If ViewState.Item("PayrollCompID") = "SPHSC1" Or chkAboriginalFlag.Checked = True Then
            If ddlWTID.SelectedValue <> "" Or chkAboriginalFlag.Checked = True Then
                IsUpdPersonalOther = True
                bePersonalOther.CompID.Value = hidCompID.Value
                bePersonalOther.EmpID.Value = txtEmpID.Text
                bePersonalOther.WTID.Value = ddlWTID.SelectedValue '2015/11/25 Modify 
                bePersonalOther.OfficeLoginFlag.Value = IIf(chkOfficeLoginFlag.Checked, "1", "0") '2015/11/25 Modify 
                bePersonalOther.AboriginalFlag.Value = IIf(chkAboriginalFlag.Checked, "1", "0")
                bePersonalOther.AboriginalTribe.Value = ddlAboriginalTribe.SelectedValue
                bePersonalOther.LastChgComp.Value = UserProfile.ActCompID
                bePersonalOther.LastChgID.Value = UserProfile.ActUserID
                bePersonalOther.LastChgDate.Value = Now
            End If

            '4. 新增員工工作性質檔[EmpWorkType]
            If hidWorkTypeID.Value <> hidWorkTypeID_Old.Value Then
                Dim arrWorkTypeID() As String = lblSelectWorkTypeID.Text.Replace("'", "").Split(",")
                intCount = 0
                For Each strWorkTypeID In arrWorkTypeID
                    ReDim Preserve beEmpWorkTypeRows(intCount)
                    Dim beEmpWorkType As New beEmpWorkType.Row()

                    beEmpWorkType.CompID.Value = hidCompID.Value
                    beEmpWorkType.EmpID.Value = txtEmpID.Text

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
            End If


            '5. 新增員工職位檔[EmpPosition]
            If hidPositionID.Value <> hidPositionID_Old.Value Then
                Dim arrPositionID() As String = lblSelectPositionID.Text.Replace("'", "").Split(",")
                intCount = 0
                For Each strPositionID In arrPositionID
                    ReDim Preserve beEmpPositionRows(intCount)
                    Dim beEmpPosition As New beEmpPosition.Row()

                    beEmpPosition.CompID.Value = hidCompID.Value
                    beEmpPosition.EmpID.Value = txtEmpID.Text

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

            '6. 新增員工最小簽核單位檔[EmpFlow]
            If ucSelectFlowOrgan.SelectedOrganID <> hidFlowOrganID_Old.Value Then
                beEmpFlow.CompID.Value = hidCompID.Value
                beEmpFlow.EmpID.Value = txtEmpID.Text
                beEmpFlow.ActionID.Value = "01"
                beEmpFlow.OrganID.Value = ucSelectFlowOrgan.SelectedOrganID
                beEmpFlow.GroupType.Value = objST.QueryData("OrganizationFlow", " AND VirtualFlag = '0' AND InValidFlag = '0' AND OrganID = " & Bsp.Utility.Quote(ucSelectHROrgan.SelectedOrganID), "GroupType")
                beEmpFlow.GroupID.Value = objST.QueryData("OrganizationFlow", "AND OrganID = " & Bsp.Utility.Quote(ucSelectHROrgan.SelectedOrganID), "GroupID")
                beEmpFlow.LastChgComp.Value = UserProfile.ActCompID '2015/11/30 Add
                beEmpFlow.LastChgID.Value = UserProfile.ActUserID   '2015/11/30 Add
                beEmpFlow.LastChgDate.Value = Now
            End If

            '7. 修改通訊資料檔[寫入Communication]
            If ddlWorkSiteID.SelectedValue <> hidWorkSiteID.Value Then
                beCommunication.IDNo.Value = txtIDNo.Text.ToUpper
                beCommunication.LastChgComp.Value = UserProfile.ActCompID
                beCommunication.LastChgID.Value = UserProfile.ActUserID
                beCommunication.LastChgDate.Value = Now

                strSQL.Clear()
                strSQL.AppendLine(" SELECT CountryCode, AreaCode, Telephone, ExtNo FROM WorkSite WHERE CompID = " & Bsp.Utility.Quote(hidCompID.Value) & " AND WorkSiteID = " & Bsp.Utility.Quote(ddlWorkSiteID.SelectedValue))
                Using dt As DataTable = Bsp.DB.ExecuteDataSet(CommandType.Text, strSQL.ToString(), "eHRMSDB").Tables(0)
                    If dt.Rows.Count > 0 Then
                        beCommunication.CompTelCode.Value = dt.Rows(0).Item(0).ToString()
                        beCommunication.AreaCode.Value = dt.Rows(0).Item(1).ToString()
                        beCommunication.CompTel.Value = dt.Rows(0).Item(2).ToString()
                        beCommunication.ExtNo.Value = dt.Rows(0).Item(3).ToString()
                    Else
                        beCommunication.CompTelCode.Value = ""
                        beCommunication.AreaCode.Value = ""
                        beCommunication.CompTel.Value = ""
                        beCommunication.ExtNo.Value = ""
                    End If
                End Using
            End If

            Return objST.UpdatePersonalSetting(bePersonal, hidIDNo.Value, UserName, IsUpdPersonalOther, bePersonalOther, beEmpWorkTypeRows, beEmpPositionRows, beEmpFlow, beCommunication, beEmpOtherIDNoRows)
        Catch ex As Exception
            Dim errLine As Integer = Convert.ToInt32(ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(" ")))
            Bsp.Utility.ShowMessage(Me, "[SaveData]" & errLine & Bsp.Utility.getMessage("E_00000") & Bsp.Utility.getInnerException(Me.FunID, ex))
            Return False
        End Try
    End Function

    Private Function funCheckData() As Boolean
        Dim objST As New ST1()
        Dim objHR As New HR()

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

        '身分證字號
        If txtIDNo.Text.Trim = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblIDNo.Text)
            txtIDNo.Focus()
            Return False
        Else
            If txtIDNo.Text <> hidIDNo.Value Then
                ViewState.Item("needRelease") = True
            End If
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
        Else
            If txtBirthDate.DateText <> hidBirthDate.Value Then
                ViewState.Item("needRelease") = True
            End If
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

        '工作證期限(證件類型為2時必填)
        'If ddlIDType.SelectedValue = "2" Then '2015/12/15 Modify 修改檢核判斷
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

        '離職日(公司)
        If ddlWorkStatus.SelectedValue <> "1" Then
            If txtQuitDate.DateText = "" Then
                Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblQuitDate.Text)
                txtQuitDate.Focus()
                Return False
            Else
                If txtEmpDate.DateText <> "" Then
                    If txtQuitDate.DateText < txtEmpDate.DateText Then
                        Bsp.Utility.ShowMessage(Me, "離職日不得早於到職日")
                        txtQuitDate.Focus()
                        Return False
                    End If
                End If
            End If
        End If

        '離職日(企業團)
        If txtSinopacQuitDate.DateText <> "" Then
            If txtSinopacQuitDate.DateText < txtEmpDate.DateText Then
                Bsp.Utility.ShowMessage(Me, "企業團離職日不得早於到職日")
                txtSinopacQuitDate.Focus()
                Return False
            End If
        End If

        '*職等/*職稱
        If ucSelectRankAndTitle.SelectedRankID = "" Or ucSelectRankAndTitle.SelectedTitleID = "" Then
            Bsp.Utility.ShowMessage(Me, "職等RankID/職稱TitleID為必要欄位，不可空白")
            ucSelectRankAndTitle.Focus()
            Return False
        Else
            If ucSelectRankAndTitle.SelectedRankID <> hidRankID.Value Or ucSelectRankAndTitle.SelectedTitleID <> hidTitleID.Value Then
                ViewState.Item("needRelease") = True
            End If
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
            Bsp.Utility.ShowMessage(Me, "「部門/科組課為必要欄位，不可空白」")
            ucSelectHROrgan.Focus()
            Return False
        End If
        If ucSelectHROrgan.SelectedDeptID <> hidDeptID.Value Or ucSelectHROrgan.SelectedOrganID <> hidOrganID.Value Then
            ViewState.Item("needRelease") = True
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

            If hidPositionID.Value <> hidPositionID_Old.Value Then
                ViewState.Item("needRelease") = True
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

        If hidPositionID.Value <> hidPositionID_Old.Value Then
            ViewState.Item("needRelease") = True
        End If

        '工作地點
        If ddlWorkSiteID.SelectedValue.Trim = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblWorkSiteID.Text)
            ddlWorkSiteID.Focus()
            Return False
        End If

        '*最小簽核單位 
        If ucSelectFlowOrgan.SelectedOrganID = "" Then
            Bsp.Utility.ShowMessage(Me, "「請挑選最小簽核單位」")
            ucSelectFlowOrgan.Focus()
            Return False
        End If

        '2015/12/15 Add 身分證與其他證件號碼是否有重覆
        For i As Integer = 0 To arr_All.Count - 1
            If arr_All(i)(0) = txtIDNo.Text.ToUpper Then
                Bsp.Utility.ShowMessage(Me, "「身分證與其他證件號碼重複，請更正」")
                txtIDNo.Focus()
                Return False
            End If
        Next

        '2015/12/15 Add 新增下拉選單檢核:證件類型
        If ddlIDType.SelectedValue = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblIDType.Text)
            ddlIDType.Focus()
            Return False
        End If

        If txtIDNo.Text <> "" Then
            If txtIDNo.Text.ToUpper <> hidIDNo.Value Or ddlIDType.SelectedValue <> hidIDType.Value Then
                '身分證是否重複
                'If objST.IsDataExists("Personal", " AND IDNo = " & Bsp.Utility.Quote(txtIDNo.Text.ToUpper) & " AND CompID = " & Bsp.Utility.Quote(hidCompID.Value)) Then
                '    Bsp.Utility.ShowMessage(Me, "「若為同公司同身份證字號員工，請由待異動維護離職復職，不可新增！」")
                '    txtIDNo.Focus()
                '    Return False
                'End If
                'If objST.IsDataExists("Personal", " AND IDNo = " & Bsp.Utility.Quote(txtIDNo.Text.ToUpper) & " AND WorkStatus = '1'") Then
                '    Bsp.Utility.ShowMessage(Me, "「身分證字號∕居留證號，已經存在，且為在職狀態，不可重複輸入。新增員工失敗！」")
                '    txtIDNo.Focus()
                '    Return False
                'End If

                ViewState.Item("IsChkIDNo") = False
                '身分證字號邏輯判斷 2015/12/15 Modify 變更檢核判斷
                If ddlIDType.SelectedValue = "1" Then
                    If objHR.funCheckIDNO(txtIDNo.Text.ToUpper) = False Then
                        Bsp.Utility.RunClientScript(Me, "confirmIDNo()")
                        Return False
                    End If
                ElseIf ddlIDType.SelectedValue = "2" Then
                    If objHR.funCheckResidentIDNo(txtIDNo.Text.ToUpper) = False Then
                        Bsp.Utility.RunClientScript(Me, "confirmIDNo()")
                        Return False
                    End If
                End If

                Return funCheckIDNo()
            End If
        End If

        Return True
    End Function

    Private Function funCheckIDNo() As Boolean
        ViewState.Item("IsChkIDNo") = True

        Dim objST As New ST1()
        If txtIDNo.Text.ToUpper <> hidIDNo.Value And objST.funCheckIDNo(txtIDNo.Text.ToUpper, hidIDNo.Value) = True Then
            Bsp.Utility.ShowMessage(Me, "「已有員工相同的身份證字號資料存在 或 與所屬眷屬相同身份證字號，請重新輸入！修改員工失敗！」")
            txtIDNo.Focus()
            Return False
        Else
            Bsp.Utility.RunClientScript(Me, "confirmIDNo2()")
            Return False
        End If
    End Function

    '身分證字號邏輯有誤，是否重新輸入→是
    Protected Sub btnYes_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnYes.Click
        txtIDNo.Focus()
    End Sub

    '身分證字號邏輯有誤，是否重新輸入→否
    Protected Sub btnNo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNo.Click
        If ViewState.Item("IsChkIDNo") = True Then
            Release("btnUpdate")
        Else
            funCheckIDNo()
        End If
    End Sub

    Public Overrides Sub DoModalReturn(ByVal returnValue As String)
        Dim strSql As String = ""
        Dim strRstName1 As String = ""
        Dim strRstName2 As String = ""

        If returnValue <> "" Then
            Dim aryData() As String = returnValue.Split(":")

            Select Case aryData(0)
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
                    lblSelectPositionID.Text = aryData(1)

                    If lblSelectPositionID.Text <> "''" Then  '非必填時，回傳空值
                        '載入 職位 下拉式選單
                        strSql = " and PositionID in (" + lblSelectPositionID.Text + ") and CompID = '" + hidCompID.Value + "'"
                        Bsp.Utility.Position(ddlPositionID, "PositionID", , strSql)
                        '第一筆為主要職位
                        Dim strDefaultValue() As String = lblSelectPositionID.Text.Replace("'", "").Split(",")
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
                Case "ucRelease"
                    Dim aryValue() As String = Split(aryData(1), "|$|")
                    If aryValue(0) = "Y" Then
                        Bsp.Utility.RunClientScript(Me, "confirmUpdate()")
                    End If
            End Select

        End If
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
        Dim strWorkSiteID As String = ""
        Dim strMainWorkTypeID As String = ""
        Dim strMainPositionID As String = ""
        Dim bolWorkType As Boolean = False
        Dim bolPosition As Boolean = False

        '事業群
        txtGroupID.Text = objRG.QueryData("Organization", "And CompID = " & Bsp.Utility.Quote(hidCompID.Value) & " And OrganID = " & Bsp.Utility.Quote(ucSelectHROrgan.SelectedOrganID), "GroupID")
        txtGroupName.Text = objRG.QueryData("OrganizationFlow", "And OrganID = " & Bsp.Utility.Quote(txtGroupID.Text), "OrganName")

        '最小簽核單位
        ucSelectFlowOrgan.LoadData(ucSelectHROrgan.SelectedOrganID, "Y")
        UpdFlowOrgnaID.Update()
        ucSelectFlowOrgan.SetDefaultOrgan()

        '班別
        If ddlWTIDTypeFlag.SelectedValue = "1" Then
            ddlWTIDTypeFlag_SelectedIndexChanged(Nothing, Nothing)
        End If

        '工作地點
        Using dt As DataTable = objRG.selectWorkSite(hidCompID.Value, ucSelectHROrgan.SelectedDeptID, ucSelectHROrgan.SelectedOrganID)
            If dt.Rows.Count > 0 Then
                strWorkSiteID = dt.Rows(0).Item(0)
            End If
        End Using
        Bsp.Utility.SetSelectedIndex(ddlWorkSiteID, strWorkSiteID)

        '職位
        hidPositionID.Value = ""
        Using dt As DataTable = objHR3000.GetEmpPositionByEmpOrgan(hidCompID.Value, txtEmpID.Text.Trim, ucSelectHROrgan.SelectedOrganID)
            If dt.Rows.Count > 0 Then
                bolPosition = True
                For intCnt = 0 To dt.Rows.Count - 1
                    hidPositionID.Value = hidPositionID.Value & dt.Rows(intCnt)("PrincipalFlag").ToString() & "|" & dt.Rows(intCnt)("PositionID").ToString() & "|"
                Next
                hidPositionID.Value = Left(hidPositionID.Value, Len(hidPositionID.Value) - 1)
                lblSelectPositionID.Text = hidPositionID.Value
            End If
        End Using

        If bolPosition Then
            If hidPositionID.Value <> "" Then
                strMainPositionID = GetPositionID(hidPositionID.Value)
                Bsp.Utility.SetSelectedIndex(ddlPositionID, strMainPositionID)

                Dim str As String()
                str = ddlPositionID.SelectedItem.Text.Split("-")
                lblSelectPositionName.Text = str(1)
            End If
        Else
            ddlPositionID.Items.Clear()
            ddlPositionID.Items.Insert(0, New ListItem("---請選擇---", ""))
            hidPositionID.Value = ""
            lblSelectPositionID.Text = ""
        End If

        '工作性質
        hidWorkTypeID.Value = ""
        Using dt As DataTable = objHR3000.GetEmpWorkTypeByEmpOrgan(hidCompID.Value, txtEmpID.Text.Trim, ucSelectHROrgan.SelectedOrganID)
            If dt.Rows.Count > 0 Then
                bolWorkType = True
                For intCnt = 0 To dt.Rows.Count - 1
                    hidWorkTypeID.Value = hidWorkTypeID.Value & dt.Rows(intCnt)("PrincipalFlag").ToString() & "|" & dt.Rows(intCnt)("WorkTypeID").ToString() & "|"
                Next
                hidWorkTypeID.Value = Left(hidWorkTypeID.Value, Len(hidWorkTypeID.Value) - 1)
                lblSelectWorkTypeID.Text = hidWorkTypeID.Value
            End If
        End Using

        If bolWorkType Then
            If hidWorkTypeID.Value <> "" Then
                strMainWorkTypeID = GetWorkTypeID(hidWorkTypeID.Value)
                Bsp.Utility.SetSelectedIndex(ddlWorkTypeID, strMainWorkTypeID)

                Dim str As String()
                str = ddlWorkTypeID.SelectedItem.Text.Split("-")
                lblSelectWorkTypeName.Text = str(1)
            End If
        End If
    End Sub

    '員工姓名
    Protected Sub txtNameN_TextChanged(sender As Object, e As System.EventArgs) Handles txtNameN.TextChanged
        Dim objRG As New RG1
        Dim strNameN As String = txtNameN.Text.Trim.Replace(vbCrLf, "")
        Dim rtnName As String = ""

        If strNameN.Length > 0 Then
            For iLoop As Integer = 1 To Len(strNameN)
                If Bsp.Utility.getStringLength(Mid(strNameN, iLoop, 1)) = 1 And Not Char.IsLower(Mid(strNameN, iLoop, 1)) And Not Char.IsUpper(Mid(strNameN, iLoop, 1)) Then
                    Dim QName = objRG.QueryData("HRNameMap", "And NameN = N" & Bsp.Utility.Quote(Mid(strNameN, iLoop, 1)), "Name")
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
                strCapital = Mid(txtIDNo.Text.ToUpper, 1, 1)
                strSex = Mid(txtIDNo.Text, 2, 1)

                If Asc(strCapital) >= Asc("A") And Asc(strCapital) <= Asc("Z") Then
                    If strSex = "1" Or strSex = "2" Then
                        ddlSex.SelectedValue = strSex
                    End If
                End If
            End If

            '2015/12/15 Modify 取消此功能！
            ''取企業團到職日、學歷【最高學歷】、英文姓名、護照英文姓名欄位
            'strSQL.Clear()
            'strSQL.AppendLine(" SELECT ")
            'strSQL.AppendLine(" SinopacEmpDate, EduID, EngName, PassportName ")
            'strSQL.AppendLine(" FROM Personal P ")
            'strSQL.AppendLine(" JOIN (SELECT Top 1 CompID, EmpID FROM EmployeeLog WHERE IDNo = " & Bsp.Utility.Quote(txtIDNo.Text) & " ORDER BY ModifyDate DESC) E ")
            'strSQL.AppendLine(" ON P.CompID = E.CompID AND P.EmpID = E.EmpID ")
            'Using dt As DataTable = Bsp.DB.ExecuteDataSet(CommandType.Text, strSQL.ToString(), "eHRMSDB").Tables(0)
            '    If dt.Rows.Count > 0 Then
            '        '企業團到職日
            '        If dt.Rows(0).Item(0).ToString().Trim <> "" Then
            '            txtSinopacEmpDate.DateText = dt.Rows(0).Item(0).ToString()
            '        End If

            '        '學歷【最高學歷】
            '        If dt.Rows(0).Item(1).ToString().Trim <> "" Then
            '            ddlEduID.SelectedValue = dt.Rows(0).Item(1).ToString()
            '        End If

            '        '英文姓名
            '        If dt.Rows(0).Item(2).ToString().Trim <> "" Then
            '            txtEngName.Text = dt.Rows(0).Item(2).ToString()
            '        End If

            '        '護照英文姓名
            '        If dt.Rows(0).Item(3).ToString().Trim <> "" Then
            '            txtPassportName.Text = dt.Rows(0).Item(3).ToString()
            '        End If
            '    End If
            'End Using

            '2015/12/15 Modify 取消此功能！
            ''查詢配偶狀態
            'strSQL.Clear()
            'strSQL.AppendLine(" SELECT RelativeID FROM Family WHERE IDNo = " & Bsp.Utility.Quote(txtIDNo.Text) & " AND RelativeID = '01' ")
            'Using dt As DataTable = Bsp.DB.ExecuteDataSet(CommandType.Text, strSQL.ToString(), "eHRMSDB").Tables(0)
            '    If dt.Rows.Count > 0 Then
            '        ddlMarriage.SelectedValue = "2"
            '    Else
            '        ddlMarriage.SelectedValue = "1"
            '    End If
            'End Using
        End If
    End Sub

    '試用期
    'Protected Sub ddlProbMonth_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlProbMonth.SelectedIndexChanged
    '    If ddlProbMonth.SelectedValue = "0" Then
    '        txtProbDate.DateText = txtEmpDate.DateText
    '    Else
    '        txtProbDate.DateText = ""
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

    '原住民註記
    Protected Sub chkAboriginalFlag_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkAboriginalFlag.CheckedChanged
        If chkAboriginalFlag.Checked Then
            ddlAboriginalTribe.Enabled = True
        Else
            ddlAboriginalTribe.Enabled = False
            ddlAboriginalTribe.SelectedValue = ""
        End If
    End Sub

#Region "PositionID"    '職位
    '異動後資料-職位button
    Protected Sub ucPositionID_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ucSelectPosition.Load
        '載入按鈕-職位選單畫面
        ucSelectPosition.QueryCompID = hidCompID.Value
        ucSelectPosition.QueryEmpID = ""
        ucSelectPosition.DefaultPosition = lblSelectPositionID.Text
        ucSelectPosition.QueryOrganID = ucSelectHROrgan.SelectedOrganID
        ucSelectPosition.Fields = New FieldState() { _
            New FieldState("PositionID", "職位代碼", True, True), _
            New FieldState("Remark", "職位名稱", True, True)}
    End Sub

    '異動後資料-職位下拉選單:將選擇那筆 改為 第一筆為主要職位
    Protected Sub ddlPositionID_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlPositionID.SelectedIndexChanged
        Dim strRst1 As String = ""
        Dim strRst2 As String = ""
        Dim strMainPosition As String = ""
        Dim strPosition As String = ""
        For i As Integer = 0 To ddlPositionID.Items.Count - 1

            If ddlPositionID.Items(i).Selected Then
                strRst1 = "'" + ddlPositionID.Items(i).Value + "'"
                strMainPosition = "1|" + ddlPositionID.Items(i).Value
            Else
                If strRst2 <> "" Then strRst2 += ","
                strRst2 += "'" + ddlPositionID.Items(i).Value + "'"
                If strPosition <> "" Then strPosition += "|"
                strPosition += "0|" + ddlPositionID.Items(i).Value
            End If
        Next
        If strRst2 = "" Then
            lblSelectPositionID.Text = strRst1
            hidPositionID.Value = strMainPosition
        Else
            lblSelectPositionID.Text = strRst1 + "," + strRst2
            hidPositionID.Value = strMainPosition + "|" + strPosition
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

        lblSelectPositionID.Text = strWherePosition

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

    Private Sub Release(ByVal LogFunction As String)
        ucRelease.ShowCompRole = "True"
        ucRelease.FunID = "ST1100"
        ucRelease.LogFunction = LogFunction
        ucRelease.OpenSelect()
    End Sub

    '任職狀態下拉選單change
    Protected Sub ddlWorkStatus_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlWorkStatus.SelectedIndexChanged
        If ddlWorkStatus.SelectedValue = "1" Then
            txtQuitDate.DateText = ""
            txtQuitDate.Enabled = False
        Else
            txtQuitDate.Enabled = True
        End If
    End Sub

    '確定修改資料？→是
    Protected Sub confirmUpdate_Yes_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles confirmUpdate_Yes.Click
        If SaveData() Then
            btnOtherIDNo.Visible = True
            btnOtherIDNoInsert.Visible = False
            btnOtherIDNoUpdate.Visible = False
            btnOtherIDNoCancel.Visible = False
            lblOtherIDNo.Visible = False
            txtOtherIDNo.Visible = False
            lblOtherIDType.Visible = False
            ddlOtherIDType.Visible = False
            lblOtherIDExpireDate.Visible = False
            txtOtherIDExpireDate.Visible = False
            arr_All.Clear()
            subGetData(ViewState.Item("CompID").ToString, ViewState.Item("EmpID").ToString)
            Bsp.Utility.ShowMessage(Me.Page, "存檔成功")
        End If
    End Sub

    '確定修改資料？→否
    Protected Sub confirmUpdate_No_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles confirmUpdate_No.Click

    End Sub

#Region "其他證件號碼" '2015/12/14 Add
    Protected Sub btnOtherIDNo_Click(sender As Object, e As System.EventArgs)
        btnOtherIDNo.Visible = False
        btnOtherIDNoInsert.Visible = True
        btnOtherIDNoUpdate.Visible = False
        btnOtherIDNoCancel.Visible = True
        lblOtherIDNo.Visible = True
        txtOtherIDNo.Visible = True
        lblOtherIDType.Visible = True
        ddlOtherIDType.Visible = True
        lblOtherIDExpireDate.Visible = True
        txtOtherIDExpireDate.Visible = True
    End Sub

    Protected Sub gvOtherIDNo_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        Dim ibn As ImageButton = DirectCast(e.CommandSource, ImageButton)
        Dim gvr As GridViewRow = DirectCast(ibn.NamingContainer, GridViewRow)

        Select Case e.CommandName
            Case "Update"
                gvOtherIDNo_Update(gvr)
            Case "Delete"
                gvOtherIDNo_Delete(gvr)
        End Select
    End Sub

    Private Sub gvOtherIDNo_Update(ByVal gvr As GridViewRow)
        btnOtherIDNo.Visible = False
        btnOtherIDNoInsert.Visible = False
        btnOtherIDNoUpdate.Visible = True
        btnOtherIDNoCancel.Visible = True
        lblOtherIDNo.Visible = True
        txtOtherIDNo.Visible = True
        lblOtherIDType.Visible = True
        ddlOtherIDType.Visible = True
        lblOtherIDExpireDate.Visible = True
        txtOtherIDExpireDate.Visible = True


        txtOtherIDNo.Text = gvOtherIDNo.DataKeys(Me.selectedRow(gvOtherIDNo))("OtherIDNo").ToString()
        ddlOtherIDType.SelectedValue = gvOtherIDNo.DataKeys(Me.selectedRow(gvOtherIDNo))("OtherIDType").ToString()
        txtOtherIDExpireDate.DateText = gvOtherIDNo.DataKeys(Me.selectedRow(gvOtherIDNo))("OtherIDExpireDate").ToString()

        ViewState.Item("OtherIDNoOld") = txtOtherIDNo.Text
        ViewState.Item("OtherIDTypeOld") = ddlOtherIDType.SelectedValue
    End Sub

    Private Sub gvOtherIDNo_Delete(ByVal gvr As GridViewRow)
        btnOtherIDNo.Visible = True
        btnOtherIDNoInsert.Visible = False
        btnOtherIDNoUpdate.Visible = False
        btnOtherIDNoCancel.Visible = False
        lblOtherIDNo.Visible = False
        txtOtherIDNo.Visible = False
        lblOtherIDType.Visible = False
        ddlOtherIDType.Visible = False
        lblOtherIDExpireDate.Visible = False
        txtOtherIDExpireDate.Visible = False

        txtOtherIDNo.Text = ""
        ddlOtherIDType.SelectedValue = ""
        txtOtherIDExpireDate.DateText = ""
        Dim strOtherIDNo As String = gvOtherIDNo.DataKeys(Me.selectedRow(gvOtherIDNo))("OtherIDNo").ToString()
        For i As Integer = 0 To arr_All.Count - 1
            If arr_All(i)(0) = strOtherIDNo Then
                arr_All.RemoveAt(i)
                Exit For
            End If
        Next
        GetOtherIDNo()
    End Sub

    Protected Sub gvOtherIDNo_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs)

    End Sub

    Protected Sub gvOtherIDNo_RowUpdating(sender As Object, e As System.Web.UI.WebControls.GridViewUpdateEventArgs)
        gvOtherIDNo.EditIndex = -1
        GetOtherIDNo()
    End Sub

    Protected Sub gvOtherIDNo_RowDeleting(sender As Object, e As System.Web.UI.WebControls.GridViewDeleteEventArgs)
        gvOtherIDNo.EditIndex = -1
    End Sub

    Protected Sub btnOtherIDNoInsert_Click(sender As Object, e As System.EventArgs)
        If funCheckOtherIDNo("A") Then
            InsertOtherIDNo()
        End If
    End Sub

    Protected Sub InsertOtherIDNo()
        Dim arr As New ArrayList()
        arr.Add(txtOtherIDNo.Text.ToUpper)
        arr.Add(IIf(ddlOtherIDType.SelectedValue = "", "", ddlOtherIDType.SelectedItem.Text))
        arr.Add(txtOtherIDExpireDate.DateText)
        arr.Add(ddlOtherIDType.SelectedValue)
        arr_All.Add(arr)

        GetOtherIDNo()

        btnOtherIDNo.Visible = True
        btnOtherIDNoInsert.Visible = False
        btnOtherIDNoUpdate.Visible = False
        btnOtherIDNoCancel.Visible = False
        lblOtherIDNo.Visible = False
        txtOtherIDNo.Visible = False
        lblOtherIDType.Visible = False
        ddlOtherIDType.Visible = False
        lblOtherIDExpireDate.Visible = False
        txtOtherIDExpireDate.Visible = False

        txtOtherIDNo.Text = ""
        ddlOtherIDType.SelectedValue = ""
        txtOtherIDExpireDate.DateText = ""
    End Sub

    Protected Sub UpdOtherIDNo()
        For i As Integer = 0 To arr_All.Count - 1
            If arr_All(i)(0) = ViewState.Item("OtherIDNoOld") And arr_All(i)(3) = ViewState.Item("OtherIDTypeOld") Then
                arr_All(i)(0) = txtOtherIDNo.Text.ToUpper
                arr_All(i)(1) = ddlOtherIDType.SelectedItem.Text
                arr_All(i)(2) = txtOtherIDExpireDate.DateText
                arr_All(i)(3) = ddlOtherIDType.SelectedValue
                Exit For
            End If
        Next
        GetOtherIDNo()

        btnOtherIDNo.Visible = True
        btnOtherIDNoInsert.Visible = False
        btnOtherIDNoUpdate.Visible = False
        btnOtherIDNoCancel.Visible = False
        lblOtherIDNo.Visible = False
        txtOtherIDNo.Visible = False
        lblOtherIDType.Visible = False
        ddlOtherIDType.Visible = False
        lblOtherIDExpireDate.Visible = False
        txtOtherIDExpireDate.Visible = False

        txtOtherIDNo.Text = ""
        ddlOtherIDType.SelectedValue = ""
        txtOtherIDExpireDate.DateText = ""
    End Sub

    Protected Sub btnOtherIDNoUpdate_Click(sender As Object, e As System.EventArgs)
        If funCheckOtherIDNo("U") Then
            UpdOtherIDNo()
        End If
    End Sub

    '2015/12/18 Add 新增檢核 其他證件號碼(新增):身分證字號邏輯有誤，是否重新輸入→是
    Protected Sub btnOtherIDNoY_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOtherIDNoY.Click
        txtOtherIDNo.Focus()
    End Sub

    '2015/12/18 Add 新增檢核 其他證件號碼(新增):身分證字號邏輯有誤，是否重新輸入→否
    Protected Sub btnOtherIDNoN_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOtherIDNoN.Click
        InsertOtherIDNo()
    End Sub

    '2015/12/18 Add 新增檢核 其他證件號碼(修改):身分證字號邏輯有誤，是否重新輸入→是
    Protected Sub btnOtherIDNoY_Upd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOtherIDNoY_Upd.Click
        txtOtherIDNo.Focus()
    End Sub

    '2015/12/18 Add 新增檢核 其他證件號碼(修改):身分證字號邏輯有誤，是否重新輸入→否
    Protected Sub btnOtherIDNoN_Upd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOtherIDNoN_Upd.Click
        UpdOtherIDNo()
    End Sub

    Protected Sub btnOtherIDNoCancel_Click(sender As Object, e As System.EventArgs)
        btnOtherIDNo.Visible = True
        btnOtherIDNoInsert.Visible = False
        btnOtherIDNoUpdate.Visible = False
        btnOtherIDNoCancel.Visible = False
        lblOtherIDNo.Visible = False
        txtOtherIDNo.Visible = False
        lblOtherIDType.Visible = False
        ddlOtherIDType.Visible = False
        lblOtherIDExpireDate.Visible = False
        txtOtherIDExpireDate.Visible = False

        txtOtherIDNo.Text = ""
        ddlOtherIDType.SelectedValue = ""
        txtOtherIDExpireDate.DateText = ""
    End Sub

    Private Function funCheckOtherIDNo(ByVal Type As String) As Boolean
        Dim objHR As New HR

        '證件號碼
        If txtOtherIDNo.Text.Trim = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblEmpOtherIDNo.Text.Trim & "-" & lblOtherIDNo.Text)
            txtOtherIDNo.Focus()
            Return False
        Else
            If txtOtherIDNo.Text = txtIDNo.Text.ToUpper Then
                Bsp.Utility.ShowMessage(Me, lblEmpOtherIDNo.Text.Trim & "-" & "此證號" & txtOtherIDNo.Text & "與主檔身分證重複")
                txtOtherIDNo.Focus()
                Return False
            End If

            For i As Integer = 0 To arr_All.Count - 1
                If arr_All(i)(0) = txtOtherIDNo.Text And arr_All(i)(3) = ddlOtherIDType.SelectedValue Then
                    If Type = "A" Then
                        Bsp.Utility.ShowMessage(Me, lblEmpOtherIDNo.Text.Trim & "-" & "此證號" & txtOtherIDNo.Text & "+證件類型" & ddlOtherIDType.SelectedItem.Text & "已存在")
                        txtOtherIDNo.Focus()
                        Return False
                    ElseIf Type = "U" Then
                        If arr_All(i)(0) <> ViewState.Item("OtherIDNoOld") Or arr_All(i)(3) <> ViewState.Item("OtherIDTypeOld") Then
                            Bsp.Utility.ShowMessage(Me, lblEmpOtherIDNo.Text.Trim & "-" & "此證號" & txtOtherIDNo.Text & "+證件類型" & ddlOtherIDType.SelectedItem.Text & "已存在")
                            txtOtherIDNo.Focus()
                            Return False
                        End If
                    End If
                End If
            Next
        End If

        '工作證期限 '2015/12/31 修改時錯誤訊息要加上編號
        'If ddlOtherIDType.SelectedValue = "2" Then
        '    If txtOtherIDExpireDate.DateText = "" Or txtOtherIDExpireDate.DateText = "____/__/__" Then
        '        Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblEmpOtherIDNo.Text.Trim & "-" & lblOtherIDExpireDate.Text)
        '        txtOtherIDExpireDate.Focus()
        '        Return False
        '    End If
        'End If

        '2015/12/21 Add 證件類型:身分證字號邏輯判斷
        If ddlOtherIDType.SelectedValue = "1" Then
            If objHR.funCheckIDNO(txtIDNo.Text.ToUpper) = False Then
                If Type = "A" Then
                    Bsp.Utility.RunClientScript(Me, "confirmAdd()")
                ElseIf Type = "U" Then
                    Bsp.Utility.RunClientScript(Me, "confirmUpd()")
                End If
                Return False
            End If
        ElseIf ddlOtherIDType.SelectedValue = "2" Then
            If objHR.funCheckResidentIDNo(txtIDNo.Text.ToUpper) = False Then
                If Type = "A" Then
                    Bsp.Utility.RunClientScript(Me, "confirmAdd()")
                ElseIf Type = "U" Then
                    Bsp.Utility.RunClientScript(Me, "confirmUpd()")
                End If
                Return False
            End If
        End If

        Return True
    End Function

    Private Sub GetOtherIDNo()
        Dim newTable As New DataTable
        newTable.Columns.Add("OtherIDNo")
        newTable.Columns.Add("OtherIDTypeName")
        newTable.Columns.Add("OtherIDExpireDate")
        newTable.Columns.Add("OtherIDType")

        For i As Integer = 0 To arr_All.Count - 1
            newTable.Rows.Add(arr_All(i)(0), arr_All(i)(1), arr_All(i)(2), arr_All(i)(3))
        Next
        gvOtherIDNo.DataSource = newTable
        gvOtherIDNo.DataBind()
    End Sub
#End Region

End Class
