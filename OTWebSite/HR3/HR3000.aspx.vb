'****************************************************
'功能說明：員工待異動紀錄維護
'建立人員：Weicheng
'建立日期：2014/08/18
'****************************************************
Imports System.Data

Partial Class HR_HR3000
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim objSC As New SC


            '/*Commom/UserProfile.選擇權限公司代碼SelectCompRoleID*/'
            If UserProfile.SelectCompRoleID = "ALL" Then
                lblCompRoleID.Visible = False '不呈現
                Bsp.Utility.FillHRCompany(ddlCompID)
                '//Common/Utility.vb->class Utility中的FillHRCompany(1引數)
                '//FillHRCompany(1引數){1個中自動填表成3個在呼叫另一個多型的FillHRCompany(3個引數)}
                Page.SetFocus(ddlCompID)
            Else
                lblCompRoleID.Text = UserProfile.SelectCompRoleName '20150527 wei modify UserProfile.SelectCompRoleID + "-" + objSC.GetCompName(UserProfile.SelectCompRoleID).Rows(0).Item("CompName").ToString
                ddlCompID.Visible = False
                Page.SetFocus(txtEmpID)
            End If

            ucQueryEmp.ShowCompRole = "False"
            ucQueryEmp.InValidFlag = "N"
            ucQueryEmp.SelectCompID = UserProfile.SelectCompRoleID

            HR3000.FillEmployeeReason(ddlReason,False)

        End If
    End Sub
    ''20161121 leo modify ActionCheck
    'Private Function ActionCheck(ByVal ValidDate As Boolean) As Boolean
    '    If selectedRows(gvMain) <> "" Then
    '        Dim HR As New HR
    '        Dim intSelectRow As Integer
    '        Dim intSelectCount As Integer = 0
    '        For intRow As Integer = 0 To gvMain.Rows.Count - 1
    '            Dim objChk As CheckBox = gvMain.Rows(intRow).FindControl("chk_gvMain")
    '            If objChk.Checked Then
    '                intSelectRow = intRow
    '                intSelectCount = intSelectCount + 1

    '                Dim strFrom As String = " EmployeeWait E" & _
    '                    " inner join  Personal P on E.CompID=P.CompID and E.EmpID=P.EmpID  and E.OrganID != P.OrganID " & _
    '                    " inner join OrganizationWait O on P.CompID=O.CompID and P.OrganID=O.OrganID " & _
    '                    " and O.OrganReason = '2' and O.OrganType in ('1','3')  and WaitStatus='0' " & _
    '                    IIf(ValidDate, " and O.ValidDate=E.ValidDate", "and O.ValidDate<=E.ValidDate ")
    '                Dim strWhere As String = _
    '                    " and E.CompID=" + Bsp.Utility.Quote(gvMain.DataKeys(intSelectRow)("CompID").ToString()) + _
    '                    " and E.EmpID=" + Bsp.Utility.Quote(gvMain.DataKeys(intSelectRow)("EmpID").ToString()) + _
    '                    " and E.ValidDate=" + Bsp.Utility.Quote(gvMain.DataKeys(intSelectRow)("ValidDate").ToString()) + _
    '                    " and E.Seq=" + Bsp.Utility.Quote(gvMain.DataKeys(intSelectRow)("Seq").ToString()) + " "

    '                If HR.IsDataExists(strFrom, strWhere) Then
    '                    Return True
    '                End If
    '            End If
    '        Next
    '        Return False
    '    End If
    '    Bsp.Utility.ShowMessage(Me, "請先查詢資料")
    '    Return False
    'End Function
    Public Overrides Sub DoAction(ByVal Param As String)
        ViewState.Item("Action") = ""
        Select Case Param
            Case "btnAdd"       '新增
                DoAdd()
            Case "btnUpdate"    '修改
                DoUpdate()
            Case "btnQuery"     '查詢
                ViewState.Item("DoQuery") = "Y"
                DoQuery()
            Case "btnDelete"    '刪除
                    ViewState.Item("Action") = "btnDelete"
                    Release("btnDelete")
                'DoDelete()
            Case "btnExecutes"   '執行 20150716 wei modify btnActionC 
                ViewState.Item("Action") = "btnExecute"
                Release("btnExecute")
            Case "btnUpload" '上傳 20150716 wei modify btnPrint
                DoUpload()
            Case "btnDownload"  '下傳
                DoDownload()
            Case "btnActionX"   '清除
                DoClear()
        End Select
    End Sub

    Protected Overrides Sub BaseOnPageTransfer(ByVal ti As TransferInfo)
        If Not IsPostBack() Then
            Dim ht As Hashtable = Bsp.Utility.getHashTableFromParam(ti.Args)

            For Each strKey As String In ht.Keys
                Dim ctl As Control = Page.FindControl(strKey)

                If TypeOf ctl Is TextBox Then
                    CType(ctl, TextBox).Text = ht(strKey).ToString()
                End If
                If TypeOf ctl Is CheckBox Then
                    CType(ctl, CheckBox).Checked = CBool(ht(strKey).ToString())
                End If
                If TypeOf ctl Is DropDownList Then
                    CType(ctl, DropDownList).SelectedValue = ht(strKey).ToString
                End If
            Next
            If ht.ContainsKey("PageNo") Then
                pcMain.PageNo = Convert.ToInt32(ht("PageNo"))
            End If
            If ht.ContainsKey("DoQuery") Then
                If ht("DoQuery").ToString() = "Y" Then
                    ViewState.Item("DoQuery") = "Y"
                    DoQuery()
                End If
            End If
        End If
    End Sub
    Private Sub DoClear()
        IsDoQuery.Value = ""
        ViewState.Item("DoQuery") = ""

        chkValidOrNot.Checked = False
        ddlValidOrNot.SelectedIndex = 0

        chkEmpID.Checked = False
        txtEmpID.Text = ""

        chkName.Checked = False
        txtName.Text = ""

        chkReason.Checked = False
        ddlReason.SelectedIndex = 0

        chkValidDate.Checked = False
        txtValidDateB.Text = ""
        txtValidDateE.Text = ""

        gvMain.Visible = False

    End Sub
    Private Sub DoAdd()
        Dim btnA As New ButtonState(ButtonState.emButtonType.Add)
        Dim btnX As New ButtonState(ButtonState.emButtonType.Exit)
        Dim btnC As New ButtonState(ButtonState.emButtonType.Cancel)

        btnA.Caption = "存檔返回"
        btnX.Caption = "返回"
        btnC.Caption = "清除"


        Dim strCompID As String
        If UserProfile.SelectCompRoleID = "ALL" Then
            strCompID = ddlCompID.SelectedValue
        Else
            strCompID = UserProfile.SelectCompRoleID
        End If

        Me.TransferFramePage("~/HR3/HR3001.aspx", New ButtonState() {btnA, btnX, btnC}, _
                             chkValidOrNot.ID & "=" & CStr(chkValidOrNot.Checked), _
                             ddlValidOrNot.ID & "=" & ddlValidOrNot.SelectedValue, _
                             chkEmpID.ID & "=" & CStr(chkEmpID.Checked), _
                             txtEmpID.ID & "=" & txtEmpID.Text, _
                             chkName.ID & "=" & CStr(chkName.Checked), _
                             txtName.ID & "=" & txtName.Text, _
                             chkValidDate.ID & "=" & CStr(chkValidDate.Checked), _
                             txtValidDateB.ID & "=" & txtValidDateB.Text, _
                             txtValidDateE.ID & "=" & txtValidDateE.Text, _
                             chkReason.ID & "=" & CStr(chkReason.Checked), _
                             ddlReason.ID & "=" & ddlReason.SelectedValue, _
                             "SelectCompRoleID=" & strCompID, _
                             "SelectCompID=" & strCompID, _
                             "PageNo=" & pcMain.PageNo.ToString(), _
                             "DoQuery=" & Bsp.Utility.IsStringNull(ViewState.Item("DoQuery")))
    End Sub

    Private Sub DoUpdate()
        If selectedRows(gvMain) <> "" Then
            Dim intSelectRow As Integer
            Dim intSelectCount As Integer = 0
            For intRow As Integer = 0 To gvMain.Rows.Count - 1
                Dim objChk As CheckBox = gvMain.Rows(intRow).FindControl("chk_gvMain")
                If objChk.Checked Then
                    intSelectRow = intRow
                    intSelectCount = intSelectCount + 1
                End If
            Next
            If intSelectCount = 1 Then
                Dim btnA As New ButtonState(ButtonState.emButtonType.Add)
                Dim btnX As New ButtonState(ButtonState.emButtonType.Exit)
                Dim btnC As New ButtonState(ButtonState.emButtonType.Cancel)

                btnA.Caption = "存檔返回"
                btnX.Caption = "返回"
                btnC.Caption = "清除"

                Dim strCompID As String
                If UserProfile.SelectCompRoleID = "ALL" Then
                    strCompID = ddlCompID.SelectedValue
                Else
                    strCompID = UserProfile.SelectCompRoleID
                End If

                Me.TransferFramePage("~/HR3/HR3002.aspx", New ButtonState() {btnA, btnX, btnC}, _
                                 chkValidOrNot.ID & "=" & CStr(chkValidOrNot.Checked), _
                                 ddlValidOrNot.ID & "=" & ddlValidOrNot.SelectedValue, _
                                 chkEmpID.ID & "=" & CStr(chkEmpID.Checked), _
                                 txtEmpID.ID & "=" & txtEmpID.Text, _
                                 chkName.ID & "=" & CStr(chkName.Checked), _
                                 txtName.ID & "=" & txtName.Text, _
                                 chkValidDate.ID & "=" & CStr(chkValidDate.Checked), _
                                 txtValidDateB.ID & "=" & txtValidDateB.Text, _
                                 txtValidDateE.ID & "=" & txtValidDateE.Text, _
                                 chkReason.ID & "=" & CStr(chkReason.Checked), _
                                 ddlReason.ID & "=" & ddlReason.SelectedValue, _
                                 "SelectCompRoleID=" & strCompID, _
                                 "SelectCompID=" & gvMain.DataKeys(intSelectRow)("CompID").ToString(), _
                                 "SelectEmpID=" & gvMain.DataKeys(intSelectRow)("EmpID").ToString(), _
                                 "SelectName=" & gvMain.DataKeys(intSelectRow)("NameN").ToString(), _
                                 "SelectValidDate=" & gvMain.DataKeys(intSelectRow)("ValidDate").ToString(), _
                                 "SelectSeq=" & gvMain.DataKeys(intSelectRow)("Seq").ToString(), _
                                 "PageNo=" & pcMain.PageNo.ToString(), _
                                 "DoQuery=Y")
            Else
                Bsp.Utility.ShowMessage(Me, "修改只能選擇一筆資料")
            End If
        End If
    End Sub

    Private Sub DoQuery()
        Dim objHR3000 As New HR3000()
        Dim strCompID As String

        If funCheckData() Then
            If UserProfile.SelectCompRoleID = "ALL" Then
                strCompID = ddlCompID.SelectedValue
            Else
                strCompID = UserProfile.SelectCompRoleID
            End If

            Try
                IsDoQuery.Value = "Y"
                gvMain.Visible = True
                pcMain.DataTable = objHR3000.QueryEmployeeWait( _
                    "CompID=" & strCompID, _
                    "ValidOrNot=" & chkValidOrNot.Checked.ToString() & ";" & ddlValidOrNot.SelectedValue, _
                    "EmpID=" & chkEmpID.Checked.ToString() & ";" & txtEmpID.Text.ToUpper.Trim(), _
                    "Name=" & chkName.Checked.ToString() & ";" & txtName.Text.Trim(), _
                    "ValidDate=" & chkValidDate.Checked.ToString() & ";" & txtValidDateB.Text.Trim() & ";" & txtValidDateE.Text.Trim(), _
                    "Reason=" & chkReason.Checked.ToString() & ";" & ddlReason.SelectedValue)
            Catch ex As Exception
                Bsp.Utility.ShowMessage(Me, Me.FunID & ".DoQuery", ex)
            End Try
        End If
        
    End Sub

    Private Sub DoDelete()
        If selectedRows(gvMain) = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00000")
        Else
            Dim bsEmployeeWait As New beEmployeeWait.Service()
            Dim beEmployeeWait As New beEmployeeWait.Row()
            Dim beEmpAdditionWait As New beEmpAdditionWait.Row()
            Dim beEmployeeLogWait As New beEmployeeLogWait.Row()
            Dim objHR3000 As New HR3000
            Dim objHR As New HR

            Dim strWhere As New StringBuilder
            Dim strIDNo As String = ""

            Dim strMsgComp As String = ""
            Dim strMsgValidDate As String = ""

            'beSC_Sys.SysID.Value = gvMain.DataKeys(Me.selectedRow(gvMain))("SysID").ToString()

            Try
                For intRow As Integer = 0 To gvMain.Rows.Count - 1
                    Dim objChk As CheckBox = gvMain.Rows(intRow).FindControl("chk_gvMain")
                    If objChk.Checked Then
                        '20161123-leo modify Start
                        Dim strFrom As String = " EmployeeWait E" & _
                            " inner join  Personal P on E.CompID=P.CompID and E.EmpID=P.EmpID  and E.OrganID != P.OrganID " & _
                            " inner join OrganizationWait O on P.CompID=O.CompID and P.OrganID=O.OrganID " & _
                            " and O.OrganReason = '2' and O.OrganType in ('1','3')  and WaitStatus='0' " & _
                            " and O.ValidDate=E.ValidDate"
                        strWhere.AppendLine(" and E.CompID=" + Bsp.Utility.Quote(gvMain.DataKeys(intRow)("CompID").ToString()))
                        strWhere.AppendLine("and E.EmpID=" + Bsp.Utility.Quote(gvMain.DataKeys(intRow)("EmpID").ToString()))
                        strWhere.AppendLine(" and E.ValidDate=" + Bsp.Utility.Quote(gvMain.DataKeys(intRow)("ValidDate").ToString()))
                        strWhere.AppendLine(" and E.Seq=" + Bsp.Utility.Quote(gvMain.DataKeys(intRow)("Seq").ToString()))
                        If objHR.IsDataExists(strFrom, strWhere.ToString) Then
                            Bsp.Utility.ShowMessage(Me, "刪除失敗：此人員所屬的部門，在組織待異動為2-組織無效，需先刪除組織待異動的資料後，才可刪除此筆人員待異動紀錄")
                            Return
                        End If
                        strFrom = " EmployeeWait E" & _
                            " inner join  Personal P on E.CompID=P.CompID and E.EmpID=P.EmpID  and E.DeptID != P.DeptID " & _
                            " inner join OrganizationWait O on P.CompID=O.CompID and P.DeptID=O.OrganID " & _
                            " and O.OrganReason = '2' and O.OrganType in ('1','3')  and WaitStatus='0' " & _
                            " and O.ValidDate=E.ValidDate"
                        If objHR.IsDataExists(strFrom, strWhere.ToString) Then
                            Bsp.Utility.ShowMessage(Me, "刪除失敗：人員待異動的科組課，在組織待異動為2-組織無效，需先刪除組織待異動的資料後，才可刪除此筆人員待異動紀錄")
                            Return
                        End If
                        strWhere.Clear()
                        '20161123-leo modify End

                        'EmployeeWait
                        beEmployeeWait.CompID.Value = gvMain.DataKeys(intRow)("CompID").ToString()
                        beEmployeeWait.EmpID.Value = gvMain.DataKeys(intRow)("EmpID").ToString()
                        beEmployeeWait.ValidDate.Value = gvMain.DataKeys(intRow)("ValidDate").ToString()
                        beEmployeeWait.Seq.Value = gvMain.DataKeys(intRow)("Seq").ToString()

                        Using dt As DataTable = bsEmployeeWait.QueryByKey(beEmployeeWait).Tables(0)
                            If dt.Rows.Count <= 0 Then Exit Sub
                            beEmployeeWait = New beEmployeeWait.Row(dt.Rows(0))
                            If beEmployeeWait.Reason.Value = "11" Or beEmployeeWait.Reason.Value = "12" Or beEmployeeWait.Reason.Value = "13" Or beEmployeeWait.Reason.Value = "14" Or beEmployeeWait.Reason.Value = "15" Or beEmployeeWait.Reason.Value = "16" Or beEmployeeWait.Reason.Value = "17" Or beEmployeeWait.Reason.Value = "18" Or beEmployeeWait.Reason.Value = "50" Then
                                strIDNo = objHR.GetIDNo(beEmployeeWait.CompID.Value, beEmployeeWait.EmpID.Value)
                                '檢查是否有待異動後在職紀錄
                                strWhere.Length = 0
                                strWhere.AppendLine(" And P.CompID <> " & Bsp.Utility.Quote(beEmployeeWait.CompID.Value) & " And P.IDNo = " & Bsp.Utility.Quote(strIDNo))
                                strWhere.AppendLine(" And (E.Reason in ('02','03','05','06','07') ")
                                strWhere.AppendLine(" or (E.Reason ='50' and E.CompID=E.NewCompID) )")
                                strWhere.AppendLine(" And E.ValidMark = '0' ")
                                If objHR.IsDataExists("EmployeeWait E Inner Join Personal P On E.CompID=P.CompID and E.EmpID=P.EmpID", strWhere.ToString) Then
                                    Using dt1 As DataTable = objHR3000.GetCheckMsg("P.CompID,C.CompName,Convert(char(10),E.ValidDate,111) as ValidDate", _
                                                                                    "EmployeeWait E Inner Join Personal P On E.CompID=P.CompID and E.EmpID=P.EmpID Inner Join Company C On P.CompID=C.CompID", _
                                                                                    strWhere.ToString)

                                        strMsgComp = dt1.Rows(0).Item("CompName").ToString
                                        strMsgValidDate = dt1.Rows(0).Item("ValidDate").ToString
                                    End Using
                                    Bsp.Utility.ShowMessage(Me, beEmployeeWait.CompID.Value & " " & beEmployeeWait.EmpID.Value & " " & beEmployeeWait.ValidDate.Value & "資料綁定：該員將於" + strMsgComp + strMsgValidDate + "復職，不得重覆兩公司在職")
                                    Return
                                End If
                            End If
                        End Using
                        objHR3000.DeleteEmployeeWait(beEmployeeWait)

                        'EmpAdditionWait
                        beEmpAdditionWait.CompID.Value = gvMain.DataKeys(intRow)("CompID").ToString()
                        beEmpAdditionWait.EmpID.Value = gvMain.DataKeys(intRow)("EmpID").ToString()
                        beEmpAdditionWait.ValidDate.Value = gvMain.DataKeys(intRow)("ValidDate").ToString()
                        beEmpAdditionWait.Seq.Value = gvMain.DataKeys(intRow)("Seq").ToString()
                        objHR3000.DeleteEmpAdditionWait(beEmpAdditionWait)

                        'EmployeeLogWait
                        beEmployeeLogWait.CompID.Value = gvMain.DataKeys(intRow)("CompID").ToString()
                        beEmployeeLogWait.EmpID.Value = gvMain.DataKeys(intRow)("EmpID").ToString()
                        beEmployeeLogWait.Wait_ValidDate.Value = gvMain.DataKeys(intRow)("ValidDate").ToString()
                        beEmployeeLogWait.Wait_Seq.Value = gvMain.DataKeys(intRow)("Seq").ToString()
                        objHR3000.DeleteEmployeeLogWaitByEmployeeWait(beEmployeeLogWait)


                    End If
                Next
                Bsp.Utility.ShowMessage(Me, "資料刪除成功")
            Catch ex As Exception
                Bsp.Utility.ShowMessage(Me, Me.FunID & ".DoDelete", ex)
            End Try
            DoQuery()
        End If
    End Sub
    Private Sub DoExecute()
        If selectedRows(gvMain) = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00000")
        Else
            Dim bsEmployeeWait As New beEmployeeWait.Service()
            Dim beEmployeeWait As New beEmployeeWait.Row()

            Dim objHR3000 As New HR3000
            Dim objHR As New HR

            Dim strCompID As String = ""
            Dim strValidDate As String = ""
            Dim strEmpID As String = ""
            Dim strSeq As String = ""
            Dim strProbation_Comp As String = ""
            Dim strClerk As String = ""
            Dim strClerkTel As String = ""

            Try
                For intRow As Integer = 0 To gvMain.Rows.Count - 1
                    Dim objChk As CheckBox = gvMain.Rows(intRow).FindControl("chk_gvMain")

                    If objChk.Checked Then

                        '20161123-leo modify Start
                        Dim strFrom As String = " EmployeeWait E" & _
                            " inner join  Personal P on E.CompID=P.CompID and E.EmpID=P.EmpID  and E.OrganID != P.OrganID " & _
                            " inner join OrganizationWait O on P.CompID=O.CompID and P.OrganID=O.OrganID " & _
                            " and O.OrganReason = '1' and O.OrganType in ('1','3')  and WaitStatus='0' " & _
                            " and O.ValidDate<=E.ValidDate"
                        Dim strWhere As New StringBuilder
                        strWhere.AppendLine(" and E.CompID=" + Bsp.Utility.Quote(gvMain.DataKeys(intRow)("CompID").ToString()))
                        strWhere.AppendLine("and E.EmpID=" + Bsp.Utility.Quote(gvMain.DataKeys(intRow)("EmpID").ToString()))
                        strWhere.AppendLine(" and E.ValidDate=" + Bsp.Utility.Quote(gvMain.DataKeys(intRow)("ValidDate").ToString()))
                        strWhere.AppendLine(" and E.Seq=" + Bsp.Utility.Quote(gvMain.DataKeys(intRow)("Seq").ToString()))
                        If objHR.IsDataExists(strFrom, strWhere.ToString) Then
                            Bsp.Utility.ShowMessage(Me, "執行失敗：此人員待異動登打的部門，在組織待異動為1組織新增且尚未生效")
                            Return
                        End If
                        strFrom = " EmployeeWait E" & _
                            " inner join  Personal P on E.CompID=P.CompID and E.EmpID=P.EmpID  and E.DeptID != P.DeptID " & _
                            " inner join OrganizationWait O on P.CompID=O.CompID and P.DeptID=O.OrganID " & _
                            " and O.OrganReason = '1' and O.OrganType in ('1','3')  and WaitStatus='0' " & _
                            " and E.ValidDate>=O.ValidDate"
                        If objHR.IsDataExists(strFrom, strWhere.ToString) Then
                            Bsp.Utility.ShowMessage(Me, "執行失敗：此人員待異動登打的科組課，在組織待異動為1組織新增且尚未生效")
                            Return
                        End If
                        strWhere.Clear()
                        '20161123-leo modify End

                        strCompID = gvMain.DataKeys(intRow)("CompID").ToString()
                        strEmpID = gvMain.DataKeys(intRow)("EmpID").ToString()
                        strValidDate = gvMain.DataKeys(intRow)("ValidDate").ToString()
                        strSeq = gvMain.DataKeys(intRow)("Seq").ToString()
                        beEmployeeWait.CompID.Value = gvMain.DataKeys(intRow)("CompID").ToString()
                        beEmployeeWait.EmpID.Value = gvMain.DataKeys(intRow)("EmpID").ToString()
                        beEmployeeWait.ValidDate.Value = gvMain.DataKeys(intRow)("ValidDate").ToString()
                        beEmployeeWait.Seq.Value = gvMain.DataKeys(intRow)("Seq").ToString()

                        strProbation_Comp = objHR.GetProbation_Comp(strCompID).Rows(0).Item("Probation").ToString

                        Using dt As DataTable = objHR3000.GetClerk(strCompID)
                            For intLoop As Integer = 0 To dt.Rows.Count - 1
                                If Len(strClerk) > 0 Then strClerk = strClerk + ";"
                                If Len(strClerkTel) > 0 Then strClerkTel = strClerkTel + ";"
                                strClerk = strClerk + dt.Rows(intLoop).Item("Clerk").ToString
                                strClerkTel = strClerkTel + dt.Rows(intLoop).Item("ClerkTel").ToString
                            Next
                        End Using

                        If strClerk = "" Or strClerkTel = "" Then
                            Bsp.Utility.ShowMessage(Me, strCompID & "功能負責人(團保作業)：資料未維護！")
                            Return
                        End If

                        Using dt As DataTable = bsEmployeeWait.QueryByKey(beEmployeeWait).Tables(0)
                            If dt.Rows.Count <= 0 Then Exit Sub
                            beEmployeeWait = New beEmployeeWait.Row(dt.Rows(0))
                            If beEmployeeWait.ValidMark.Value = "1" Then
                                Bsp.Utility.ShowMessage(Me, strCompID & " " & strEmpID & " " & strValidDate & "該記錄已生效！")
                                Return
                            End If
                        End Using

                        If CDate(strValidDate) > Now() Then
                            Bsp.Utility.ShowMessage(Me, strCompID & " " & strEmpID & " " & strValidDate & "生效日期：執行生效日期不能大於目前系統日！")
                            Return
                        End If

                        Dim Result = objHR3000.Execute_sHR3001(strCompID, strEmpID, strValidDate, strSeq, strProbation_Comp, strClerk, strClerkTel).Split(";")
                        If Result(0) = "1" Then
                            Bsp.Utility.ShowMessage(Me, strCompID & "批次執行失敗，失敗訊息：" & Result(1))
                            Return
                        End If

                    End If
                Next
                Bsp.Utility.ShowMessage(Me, "批次執行成功")
            Catch ex As Exception
                Bsp.Utility.ShowMessage(Me, Me.FunID & ".DoExecute", ex)
            End Try
            DoQuery()
        End If
    End Sub
    Private Sub Release(ByVal LogFunction As String)
        ucRelease.ShowCompRole = "True"
        ucRelease.FunID = "HR3000"
        ucRelease.LogFunction = LogFunction
        ucRelease.OpenSelect()
    End Sub
    Public Overrides Sub DoModalReturn(ByVal returnValue As String)
        Dim strSql As String = ""

        If returnValue <> "" Then
            Dim aryData() As String = returnValue.Split(":")

            Select Case aryData(0)

                Case "ucRelease"
                    lblReleaseResult.Text = ""
                    Dim aryValue() As String = Split(aryData(1), "|$|")
                    lblReleaseResult.Text = aryValue(0)
                    If lblReleaseResult.Text = "Y" Then
                        Select Case ViewState.Item("Action")
                            Case "btnDelete"
                                DoDelete()
                            Case "btnExecute"
                                DoExecute()
                        End Select
                    End If
                Case "ucQueryEmp"
                    Dim aryValue() As String = Split(aryData(1), "|$|")
                    txtEmpID.Text = aryValue(1)

            End Select
        End If
    End Sub
    Private Function funCheckData() As Boolean
        '查詢條件必須選擇
        If Not chkValidOrNot.Checked And Not chkEmpID.Checked And Not chkName.Checked And Not chkValidDate.Checked And Not chkReason.Checked Then
            Bsp.Utility.ShowFormatMessage(Me, "W_C1A00")
            Return False
        End If

        '狀態
        If chkValidOrNot.Checked Then
            If ddlValidOrNot.SelectedValue = "" Then
                Bsp.Utility.ShowFormatMessage(Me, "W_00035", "狀態")
                ddlValidOrNot.Focus()
                Return False
            End If
        End If

        '員工編號
        If chkEmpID.Checked Then
            If txtEmpID.Text.Trim() = "" Then
                Bsp.Utility.ShowFormatMessage(Me, "W_00030", "員工編號")
                txtEmpID.Focus()
                Return False
            Else
                If Bsp.Utility.getStringLength(txtEmpID.Text) <> txtEmpID.MaxLength Then
                    Bsp.Utility.ShowFormatMessage(Me, "W_00040", "員工編號", txtEmpID.MaxLength.ToString())
                    txtEmpID.Focus()
                    Return False
                End If
            End If
        End If

        '員工姓名
        If chkName.Checked Then
            If txtName.Text.Trim() = "" Then
                Bsp.Utility.ShowFormatMessage(Me, "W_00030", "員工姓名")
                txtName.Focus()
            Else
                If Bsp.Utility.getStringLength(txtName.Text) > txtName.MaxLength Then
                    Bsp.Utility.ShowFormatMessage(Me, "W_00040", "員工姓名", txtName.MaxLength.ToString())
                    txtName.Focus()
                    Return False
                End If
            End If
        End If

        '生效日期
        If chkValidDate.Checked Then
            If txtValidDateB.Text.Trim() = "" Then
                Bsp.Utility.ShowFormatMessage(Me, "W_00030", "生效日期(起)")
                txtValidDateB.Focus()
                Return False
            Else
                If Bsp.Utility.getStringLength(txtValidDateB.Text) > txtValidDateB.MaxLength Then
                    Bsp.Utility.ShowFormatMessage(Me, "W_00040", "生效日期(起)", txtValidDateB.MaxLength.ToString())
                    txtValidDateB.Focus()
                    Return False
                Else
                    If Not IsDate(txtValidDateB.Text) Then
                        Bsp.Utility.ShowFormatMessage(Me, "W_00070", "生效日期(起)")
                        txtValidDateB.Focus()
                        Return False
                    End If
                End If
            End If

            If txtValidDateE.Text.Trim() <> "" Then
                If Bsp.Utility.getStringLength(txtValidDateE.Text) > txtValidDateE.MaxLength Then
                    Bsp.Utility.ShowFormatMessage(Me, "W_00040", "生效日期(迄)", txtValidDateE.MaxLength.ToString())
                    txtValidDateE.Focus()
                    Return False
                Else
                    If Not IsDate(txtValidDateE.Text) Then
                        Bsp.Utility.ShowFormatMessage(Me, "W_00070", "生效日期(迄)")
                        txtValidDateE.Focus()
                        Return False
                    End If
                End If
                If CDate(txtValidDateB.Text) > CDate(txtValidDateE.Text) Then
                    Bsp.Utility.ShowFormatMessage(Me, "W_00130", "")
                    txtValidDateE.Focus()
                    Return False
                End If
            End If
        End If

        '異動原因
        If chkReason.Checked Then
            If ddlReason.SelectedValue = "" Then
                Bsp.Utility.ShowFormatMessage(Me, "W_00035", "異動原因")
                ddlReason.Focus()
                Return False
            End If
        End If

        Return True
    End Function

    Protected Sub gvMain_RowCreated(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs)
        Select Case e.Row.RowType
            Case DataControlRowType.Header
                'Dim tcRow As New GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Normal)
                'Dim tcHeader1 As New TableCell
                'tcHeader1.ColumnSpan = 8
                'tcHeader1.Text = ""
                'tcHeader1.CssClass = "td_header"
                'tcRow.Cells.Add(tcHeader1)

                'Dim tcHeader2 As New TableCell
                'tcHeader2.ColumnSpan = 1
                'tcHeader2.Text = ""
                'tcHeader2.CssClass = "td_header"
                'tcRow.Cells.Add(tcHeader2)

                'Dim tcHeader3 As New TableCell
                'tcHeader3.ColumnSpan = 1
                'tcHeader3.Text = ""
                'tcHeader3.CssClass = "td_header"
                'tcRow.Cells.Add(tcHeader3)

                'Dim tcHeader4 As New TableCell
                'tcHeader4.ColumnSpan = 1
                'tcHeader4.Text = ""
                'tcHeader4.CssClass = "td_header"
                'tcRow.Cells.Add(tcHeader4)

                'Dim tcHeader5 As New TableCell
                'tcHeader5.ColumnSpan = 1
                'tcHeader5.Text = ""
                'tcHeader5.CssClass = "td_header"
                'tcRow.Cells.Add(tcHeader5)

                'Dim tcHeader6 As New TableCell
                'tcHeader6.ColumnSpan = 1
                'tcHeader6.Text = ""
                'tcHeader6.CssClass = "td_header"
                'tcRow.Cells.Add(tcHeader6)

                'Dim tcHeader7 As New TableCell
                'tcHeader7.ColumnSpan = 1
                'tcHeader7.Text = ""
                'tcHeader7.CssClass = "td_header"
                'tcRow.Cells.Add(tcHeader7)

                'Dim tcHeader8 As New TableCell
                'tcHeader8.ColumnSpan = 1
                'tcHeader8.Text = ""
                'tcHeader8.CssClass = "td_header"
                'tcRow.Cells.Add(tcHeader8)

                'Dim tcHeader9 As New TableCell
                'tcHeader9.ColumnSpan = 4
                'tcHeader9.Text = "異動後資料"
                'tcHeader9.CssClass = "td_header"
                ''tcHeader9.BackColor = Drawing.Color.LightYellow

                'tcRow.Cells.Add(tcHeader9)

                'gvMain.Controls(0).Controls.AddAt(0, tcRow)

                ''取得該GridView的表頭()
                'Dim tcHeader As TableCellCollection = e.Row.Cells
                ''清除先前設定的表頭  
                'tcHeader.Clear()

                ''新增第一層表頭()
                'tcHeader.Add(New TableHeaderCell())
                'tcHeader(0).Attributes.Add("rowspan", "2")
                'tcHeader(0).Attributes.Add("Class", "td_header")
                ''該表頭所要顯示的內容
                ''功用是用來第一個表頭的結尾  做為下一行的開始  
                ''若未在表頭內容加上就會看到下一層表頭"資料"出現在"全部資訊"後方  
                'tcHeader(0).Text = "&nbsp;"

                'tcHeader.Add(New TableHeaderCell())
                'tcHeader(1).Attributes.Add("rowspan", "2")
                'tcHeader(1).Attributes.Add("Class", "td_header")
                'tcHeader(1).Text = "明細"

                'tcHeader.Add(New TableHeaderCell())
                'tcHeader(2).Attributes.Add("rowspan", "2")
                'tcHeader(2).Attributes.Add("Class", "td_header")
                'tcHeader(2).Text = "生效"

                'tcHeader.Add(New TableHeaderCell())
                'tcHeader(3).Attributes.Add("rowspan", "2")
                'tcHeader(3).Attributes.Add("Class", "td_header")
                'tcHeader(3).Text = "公司名稱"

                'tcHeader.Add(New TableHeaderCell())
                'tcHeader(4).Attributes.Add("rowspan", "2")
                'tcHeader(4).Attributes.Add("Class", "td_header")
                'tcHeader(4).Text = "員工編號"

                'tcHeader.Add(New TableHeaderCell())
                'tcHeader(5).Attributes.Add("rowspan", "2")
                'tcHeader(5).Attributes.Add("Class", "td_header")
                'tcHeader(5).Text = "姓名"

                'tcHeader.Add(New TableHeaderCell())
                'tcHeader(6).Attributes.Add("rowspan", "2")
                'tcHeader(6).Attributes.Add("Class", "td_header")
                'tcHeader(6).Text = "生效日期"

                'tcHeader.Add(New TableHeaderCell())
                'tcHeader(7).Attributes.Add("rowspan", "2")
                'tcHeader(7).Attributes.Add("Class", "td_header")
                'tcHeader(7).Text = "異動原因"

                'tcHeader.Add(New TableHeaderCell())
                'tcHeader(8).Attributes.Add("colspan", "4")
                'tcHeader(8).Attributes.Add("Class", "td_header")
                'tcHeader(8).Text = "異動後資料</tr><tr>"

                'tcHeader.Add(New TableHeaderCell())
                'tcHeader(9).Attributes.Add("Class", "td_header")
                'tcHeader(9).Text = "公司名稱"

                'tcHeader.Add(New TableHeaderCell())
                'tcHeader(10).Attributes.Add("Class", "td_header")
                'tcHeader(10).Text = "事業群"

                'tcHeader.Add(New TableHeaderCell())
                'tcHeader(11).Attributes.Add("Class", "td_header")
                'tcHeader(11).Text = "部門"

                'tcHeader.Add(New TableHeaderCell())
                'tcHeader(12).Attributes.Add("Class", "td_header")
                'tcHeader(12).Text = "科組課</tr>"

        End Select
    End Sub

    Protected Sub gvMain_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvMain.RowCommand
        If e.CommandName = "Detail" Then
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            Dim btnX As New ButtonState(ButtonState.emButtonType.Exit)

            btnX.Caption = "返回"

            Dim strCompID As String
            If UserProfile.SelectCompRoleID = "ALL" Then
                strCompID = ddlCompID.SelectedValue
            Else
                strCompID = UserProfile.SelectCompRoleID
            End If

            Me.TransferFramePage("~/HR3/HR3002.aspx", New ButtonState() {btnX}, _
                             chkValidOrNot.ID & "=" & CStr(chkValidOrNot.Checked), _
                             ddlValidOrNot.ID & "=" & ddlValidOrNot.SelectedValue, _
                             chkEmpID.ID & "=" & CStr(chkEmpID.Checked), _
                             txtEmpID.ID & "=" & txtEmpID.Text, _
                             chkName.ID & "=" & CStr(chkName.Checked), _
                             txtName.ID & "=" & txtName.Text, _
                             chkValidDate.ID & "=" & CStr(chkValidDate.Checked), _
                             txtValidDateB.ID & "=" & txtValidDateB.Text, _
                             txtValidDateE.ID & "=" & txtValidDateE.Text, _
                             chkReason.ID & "=" & CStr(chkReason.Checked), _
                             ddlReason.ID & "=" & ddlReason.SelectedValue, _
                             "SelectCompRoleID=" & strCompID, _
                             "SelectCompID=" & gvMain.DataKeys(index)("CompID").ToString(), _
                             "SelectEmpID=" & gvMain.DataKeys(index)("EmpID").ToString(), _
                             "SelectName=" & gvMain.DataKeys(index)("NameN").ToString(), _
                             "SelectValidDate=" & gvMain.DataKeys(index)("ValidDate").ToString(), _
                             "SelectSeq=" & gvMain.DataKeys(index)("Seq").ToString(), _
                             "PageNo=" & pcMain.PageNo.ToString(), _
                             "Detail=Y", _
                             "DoQuery=Y")



        End If
    End Sub
    '檔案上傳
    Private Sub DoUpload()
        Dim btnC As New ButtonState(ButtonState.emButtonType.Confirm)
        Dim btnX As New ButtonState(ButtonState.emButtonType.Exit)

        btnC.Caption = "確定上傳"
        btnX.Caption = "關閉離開"

        Me.CallSmallPage("~/HR3/HR3003.aspx", New ButtonState() {btnC, btnX})

    End Sub
    Private Sub DoDownload()
        Try
            If IsDoQuery.Value = "Y" Then
                Dim strCompID As String = ""
                Dim objHR3000 As New HR3000
                If pcMain.DataTable.Rows.Count > 0 Then
                    '產出檔頭
                    Dim strFileName As String = ""
                    strFileName = Bsp.Utility.GetNewFileName("資料下載-") & ".xls"



                    '動態產生GridView以便匯出成EXCEL
                    Dim gvExport As GridView = New GridView()
                    gvExport.AllowPaging = False
                    gvExport.AllowSorting = False
                    gvExport.FooterStyle.BackColor = Drawing.ColorTranslator.FromHtml("#99CCCC")
                    gvExport.FooterStyle.ForeColor = Drawing.ColorTranslator.FromHtml("#003399")
                    gvExport.RowStyle.CssClass = "tr_evenline"
                    gvExport.AlternatingRowStyle.CssClass = "tr_oddline"
                    gvExport.EmptyDataRowStyle.CssClass = "GridView_EmptyRowStyle"

                    'gvExport = gvMain  '寫GridViewName-gvMain，會變成下載GridView畫面，會出下換頁CSS等怪東西
                    If UserProfile.SelectCompRoleID = "ALL" Then
                        strCompID = ddlCompID.SelectedValue
                    Else
                        strCompID = UserProfile.SelectCompRoleID
                    End If
                    gvExport.DataSource = objHR3000.QueryEmployeeWaitByDownload( _
                    "CompID=" & strCompID, _
                    "ValidOrNot=" & chkValidOrNot.Checked.ToString() & ";" & ddlValidOrNot.SelectedValue, _
                    "EmpID=" & chkEmpID.Checked.ToString() & ";" & txtEmpID.Text.Trim(), _
                    "Name=" & chkName.Checked.ToString() & ";" & txtName.Text.Trim(), _
                    "ValidDate=" & chkValidDate.Checked.ToString() & ";" & txtValidDateB.Text.Trim() & ";" & txtValidDateE.Text.Trim(), _
                    "Reason=" & chkReason.Checked.ToString() & ";" & ddlReason.SelectedValue)
                    AddHandler gvExport.RowDataBound, AddressOf gvExport_RowDataBound   '20140103 wei add 增加自訂事件
                    gvExport.DataBind()
                    'GroupRows(gvExport, 1, 1)
                    'GroupRows(gvExport, 2, 2)
                    'GroupRows(gvExport, 3, 3)

                    Response.ClearContent()
                    Response.BufferOutput = True
                    Response.Charset = "utf-8"
                    ''Response.ContentType = "application/ms-excel"      '只寫ms-excel不OK，會變成程式碼下載@@
                    'Response.ContentType = "application/vnd.ms-excel"
                    Response.ContentType = "application/save-as"         '隱藏檔案網址路逕的下載
                    Response.AddHeader("Content-Transfer-Encoding", "binary")
                    Response.ContentEncoding = System.Text.Encoding.UTF8
                    Response.AddHeader("content-disposition", "attachment; filename=" & Server.UrlPathEncode(strFileName))

                    Dim oStringWriter As New System.IO.StringWriter()
                    Dim oHtmlTextWriter As New System.Web.UI.HtmlTextWriter(oStringWriter)

                    Response.Write("<meta http-equiv=Content-Type content=text/html charset=utf-8>")
                    Dim style As String = "<style>td{font-size:9pt} a{font-size:9pt} tr{page-break-after: always}</style>"

                    gvExport.Attributes.Add("style", "vnd.ms-excel.numberformat:@")
                    gvExport.RenderControl(oHtmlTextWriter)
                    Response.Write(style)
                    Response.Write(oStringWriter.ToString())
                    Response.End()
                Else
                    Bsp.Utility.ShowFormatMessage(Me, "W_00030", "請先查詢有資料，才能下傳!")
                End If
            Else
                Bsp.Utility.ShowFormatMessage(Me, "W_00030", "請先查詢有資料，才能下傳!")
            End If

        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & ".DoDownload", ex)
        End Try

    End Sub
    Protected Sub gvExport_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)

        Select Case e.Row.RowType
            Case DataControlRowType.Header

                ''取得該GridView的表頭  
                'Dim tcHeader As TableCellCollection = e.Row.Cells
                ''清除先前設定的表頭  
                'tcHeader.Clear()

                ''新增第一層表頭()
                'tcHeader.Add(New TableHeaderCell())
                'tcHeader(0).Attributes.Add("colspan", "10")
                ''該表頭所要顯示的內容
                ''功用是用來第一個表頭的結尾  做為下一行的開始  
                ''若未在表頭內容加上就會看到下一層表頭"資料"出現在"全部資訊"後方  
                'tcHeader(0).Text = "招募晉用狀況報表-" & txtCheckInDateS.Text.Trim() & "~" & txtCheckInDateE.Text.Trim()

                'tcHeader.Add(New TableHeaderCell())
                'tcHeader(1).Attributes.Add("colspan", "4")
                'tcHeader(1).Text = "來源/招募方式</tr><tr>"

                'tcHeader.Add(New TableHeaderCell())
                'tcHeader(2).Attributes.Add("rowspan", "2")
                'tcHeader(2).Text = "3P"

                'tcHeader.Add(New TableHeaderCell())
                'tcHeader(3).Attributes.Add("rowspan", "2")
                'tcHeader(3).Text = "職位"

                'tcHeader.Add(New TableHeaderCell())
                'tcHeader(4).Attributes.Add("rowspan", "2")
                'tcHeader(4).Text = "收件數"

                'tcHeader.Add(New TableHeaderCell())
                'tcHeader(5).Attributes.Add("rowspan", "2")
                'tcHeader(5).Text = "轉核薪"

                'tcHeader.Add(New TableHeaderCell())
                'tcHeader(6).Text = "&nbsp;"

                'tcHeader.Add(New TableHeaderCell())
                'tcHeader(7).Text = "&nbsp;"

                'tcHeader.Add(New TableHeaderCell())
                'tcHeader(8).Text = "&nbsp;"

                'tcHeader.Add(New TableHeaderCell())
                'tcHeader(9).Text = "&nbsp;"

                'tcHeader.Add(New TableHeaderCell())
                'tcHeader(10).Text = "&nbsp;"

                'tcHeader.Add(New TableHeaderCell())
                'tcHeader(11).Text = "&nbsp;"

                'tcHeader.Add(New TableHeaderCell())
                'tcHeader(12).Attributes.Add("rowspan", "2")
                'tcHeader(12).Attributes.Add("bgcolor", "#FFFF00")
                'tcHeader(12).Text = "HR轉介"

                'tcHeader.Add(New TableHeaderCell())
                'tcHeader(13).Attributes.Add("rowspan", "2")
                'tcHeader(13).Attributes.Add("bgcolor", "#FFFF00")
                'tcHeader(13).Text = "統召"

                'tcHeader.Add(New TableHeaderCell())
                'tcHeader(14).Attributes.Add("rowspan", "2")
                'tcHeader(14).Attributes.Add("bgcolor", "#FFFF00")
                'tcHeader(14).Text = "自召"

                'tcHeader.Add(New TableHeaderCell())
                'tcHeader(15).Attributes.Add("rowspan", "2")
                'tcHeader(15).Attributes.Add("bgcolor", "#FFFF00")
                'tcHeader(15).Text = "其他</tr><tr>"

                'tcHeader.Add(New TableHeaderCell())
                'tcHeader(16).Text = "暫緩"

                'tcHeader.Add(New TableHeaderCell())
                'tcHeader(17).Text = "核薪中"

                'tcHeader.Add(New TableHeaderCell())
                'tcHeader(18).Text = "拒絶"

                'tcHeader.Add(New TableHeaderCell())
                'tcHeader(19).Text = "待報到"

                'tcHeader.Add(New TableHeaderCell())
                'tcHeader(20).Text = "待報後拒絶"

                'tcHeader.Add(New TableHeaderCell())
                'tcHeader(21).Text = "已報到</tr>"
                e.Row.Cells(7).Visible = False
            Case DataControlRowType.DataRow
                e.Row.Cells(7).Visible = False
                e.Row.Cells(20).Text = GetPosition(DataBinder.Eval(e.Row.DataItem, "NewCompID").ToString(), DataBinder.Eval(e.Row.DataItem, "主要職位").ToString(), True)
                e.Row.Cells(21).Text = GetPosition(DataBinder.Eval(e.Row.DataItem, "NewCompID").ToString(), DataBinder.Eval(e.Row.DataItem, "主要職位").ToString(), False)
                e.Row.Cells(22).Text = GetWorkType(DataBinder.Eval(e.Row.DataItem, "NewCompID").ToString(), DataBinder.Eval(e.Row.DataItem, "主要工作性質").ToString(), True)
                e.Row.Cells(23).Text = GetWorkType(DataBinder.Eval(e.Row.DataItem, "NewCompID").ToString(), DataBinder.Eval(e.Row.DataItem, "主要工作性質").ToString(), False)

        End Select
    End Sub
    Public Sub GroupRows(ByVal GridView1 As GridView, ByVal cellNum As Integer, ByVal comparCellNum As Integer)
        Dim i As Integer = 0
        Dim rowSpanNum As Integer = 1
        While i < GridView1.Rows.Count - 1
            Dim gvr As GridViewRow = GridView1.Rows(i)
            For i = i + 1 To GridView1.Rows.Count - 1
                Dim gvrNext As GridViewRow = GridView1.Rows(i)
                If gvr.Cells(comparCellNum).Text = gvrNext.Cells(comparCellNum).Text Then
                    gvrNext.Cells(cellNum).Visible = False
                    rowSpanNum = rowSpanNum + 1
                Else
                    gvr.Cells(cellNum).RowSpan = rowSpanNum
                    rowSpanNum = 1
                    Exit For
                End If
                If i = GridView1.Rows.Count - 1 Then
                    gvr.Cells(cellNum).RowSpan = rowSpanNum
                End If
            Next
            gvr.Cells(cellNum).BackColor = Drawing.Color.White
        End While
    End Sub
    Public Function GetPosition(ByVal strCompID As String, ByVal strPositionID As String, bolMain As Boolean) As String
        Dim aryValue() As String = strPositionID.Split("|")
        Dim intCnt As Integer
        Dim objHR3000 As New HR3000
        Dim strMainPositionName As String = ""
        Dim strPositionName As String = ""

        For intCnt = 0 To UBound(aryValue) - 1 Step 2
            If aryValue(intCnt) = "1" Then
                strMainPositionName = objHR3000.funGetPosition(strCompID, aryValue(intCnt + 1))
                If bolMain Then Exit For
            Else
                strPositionName = strPositionName & objHR3000.funGetPosition(strCompID, aryValue(intCnt + 1)) & ","
            End If
        Next

        If bolMain Then
            Return strMainPositionName
        Else
            If Len(strPositionName) > 0 Then
                Return (Left(strPositionName, Len(strPositionName) - 1))
            Else
                Return strPositionName
            End If

        End If

    End Function
    Public Function GetWorkType(ByVal strCompID As String, ByVal strWorkTypeID As String, bolMain As Boolean) As String
        Dim aryValue() As String = strWorkTypeID.Split("|")
        Dim intCnt As Integer
        Dim objHR3000 As New HR3000
        Dim strMainWorkTypeName As String = ""
        Dim strWorkTypeName As String = ""

        For intCnt = 0 To UBound(aryValue) - 1 Step 2
            If aryValue(intCnt) = "1" Then
                strMainWorkTypeName = objHR3000.funGetWorkType(strCompID, aryValue(intCnt + 1))
                If bolMain Then Exit For
            Else
                strWorkTypeName = strWorkTypeName & objHR3000.funGetWorkType(strCompID, aryValue(intCnt + 1)) & ","
            End If
        Next

        If bolMain Then
            Return strMainWorkTypeName
        Else
            If Len(strWorkTypeName) > 0 Then
                Return Left(strWorkTypeName, Len(strWorkTypeName) - 1)
            Else
                Return strWorkTypeName
            End If

        End If

    End Function

    'Private Function funCheckData() As Boolean
    '    Dim objHR As New HR
    '    Dim objHR3000 As New HR3000

    '    Dim strCompID As String = ""

    '    Dim strClerk As String = ""
    '    Dim strClerkTel As String = ""

    '    If UserProfile.SelectCompRoleID = "ALL" Then
    '        strCompID = ddlCompID.SelectedValue
    '    Else
    '        strCompID = UserProfile.SelectCompRoleID
    '    End If

    '    Using dt As DataTable = objHR3000.GetClerk(strCompID)
    '        For intLoop As Integer = 0 To dt.Rows.Count - 1
    '            If Len(strClerk) > 0 Then strClerk = strClerk + ";"
    '            If Len(strClerkTel) > 0 Then strClerkTel = strClerkTel + ";"
    '            strClerk = strClerk + dt.Rows(intLoop).Item("Clerk").ToString
    '            strClerkTel = strClerkTel + dt.Rows(intLoop).Item("ClerkTel").ToString
    '        Next
    '    End Using

    '    If strClerk = "" Or strClerkTel = "" Then
    '        Bsp.Utility.ShowMessage(Me, "功能負責人(團保作業)：資料未維護！")
    '        Return False
    '    End If

    '    Return True
    'End Function
End Class
