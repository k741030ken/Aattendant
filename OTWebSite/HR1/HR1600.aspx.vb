'****************************************************
'功能說明：員工待異動紀錄維護
'建立人員：Weicheng
'建立日期：2014/08/18
'****************************************************
Imports System.Data

Partial Class HR_HR1600
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim objSC As New SC



            If UserProfile.SelectCompRoleID = "ALL" Then
                lblCompRoleID.Visible = False
                Bsp.Utility.FillHRCompany(ddlCompID)
                Page.SetFocus(ddlCompID)
                '現任部門
                Bsp.Utility.FillOrganization(ddlDeptID, ddlCompID.SelectedValue, "N")
                ddlDeptID.Items.Insert(0, New ListItem("", ""))
            Else
                lblCompRoleID.Text = UserProfile.SelectCompRoleName '20150527 wei modify UserProfile.SelectCompRoleID + "-" + objSC.GetCompName(UserProfile.SelectCompRoleID).Rows(0).Item("CompName").ToString
                ddlCompID.Visible = False
                Page.SetFocus(txtEmpID)
                '現任部門
                Bsp.Utility.FillOrganization(ddlDeptID, UserProfile.SelectCompRoleID, "N")
                ddlDeptID.Items.Insert(0, New ListItem("", ""))
            End If

            '狀態
            HR3000.FillEmpAdditionReason(ddlReason)

            '兼任公司
            Bsp.Utility.FillHRCompany(ddlAdditionCompID)
            ddlAdditionCompID.Items.Insert(0, New ListItem("--請選擇---", ""))


        End If
    End Sub

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
                DoDelete()
            Case "btnActionX"   '清除
                DoClear()
            Case "btnDownload"  '下傳
                DoDownload()
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
                    If ht(strKey).ToString <> "" Then
                        CType(ctl, DropDownList).SelectedValue = ht(strKey).ToString
                    End If

                End If
            Next

            '兼任部門
            If ddlAdditionCompID.SelectedValue <> "" Then
                Bsp.Utility.FillOrganization(ddlAdditionDeptID, ddlAdditionCompID.SelectedValue, "N")
                ddlAdditionDeptID.Items.Insert(0, New ListItem("", ""))
                UpdAdditionDeptID.Update()
            End If


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

        Me.TransferFramePage("~/HR1/HR1601.aspx", New ButtonState() {btnA, btnX, btnC}, _
                             rbEmpAddition.ID & "=" & CStr(rbEmpAddition.Checked), _
                             rbEmpAdditionDetail.ID & "=" & CStr(rbEmpAdditionDetail.Checked), _
                             chkDeptID.ID & "=" & CStr(chkDeptID.Checked), _
                             ddlDeptID.ID & "=" & ddlDeptID.SelectedValue, _
                             chkEmpID.ID & "=" & CStr(chkEmpID.Checked), _
                             txtEmpID.ID & "=" & txtEmpID.Text, _
                             chkName.ID & "=" & CStr(chkName.Checked), _
                             txtName.ID & "=" & txtName.Text, _
                             chkReason.ID & "=" & CStr(chkReason.Checked), _
                             ddlReason.ID & "=" & ddlReason.SelectedValue, _
                             chkValidDate.ID & "=" & CStr(chkValidDate.Checked), _
                             txtValidDateB.ID & "=" & txtValidDateB.Text, _
                             txtValidDateE.ID & "=" & txtValidDateE.Text, _
                             chkAdditionCompID.ID & "=" & CStr(chkAdditionCompID.Checked), _
                             ddlAdditionCompID.ID & "=" & ddlAdditionCompID.SelectedValue, _
                             chkAdditionDeptID.ID & "=" & CStr(chkAdditionDeptID.Checked), _
                             ddlAdditionDeptID.ID & "=" & ddlAdditionDeptID.SelectedValue, _
                             "SelectCompRoleID=" & strCompID, _
                             "SelectCompID=" & strCompID, _
                             "PageNo=" & pcMain.PageNo.ToString(), _
                             "DoQuery=" & Bsp.Utility.IsStringNull(ViewState.Item("DoQuery")))
    End Sub

    Private Sub DoUpdate()
        If rbEmpAddition.Checked Then
            Bsp.Utility.ShowFormatMessage(Me, "員工調兼現況不可修改，請選擇員工調兼資料")
            Return
        End If
        If selectedRow(gvMain) >= 0 Then
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

            Me.TransferFramePage("~/HR1/HR1602.aspx", New ButtonState() {btnA, btnX, btnC}, _
                                 rbEmpAddition.ID & "=" & CStr(rbEmpAddition.Checked), _
                                 rbEmpAdditionDetail.ID & "=" & CStr(rbEmpAdditionDetail.Checked), _
                                 chkDeptID.ID & "=" & CStr(chkDeptID.Checked), _
                                 ddlDeptID.ID & "=" & ddlDeptID.SelectedValue, _
                                 chkEmpID.ID & "=" & CStr(chkEmpID.Checked), _
                                 txtEmpID.ID & "=" & txtEmpID.Text, _
                                 chkName.ID & "=" & CStr(chkName.Checked), _
                                 txtName.ID & "=" & txtName.Text, _
                                 chkReason.ID & "=" & CStr(chkReason.Checked), _
                                 ddlReason.ID & "=" & ddlReason.SelectedValue, _
                                 chkValidDate.ID & "=" & CStr(chkValidDate.Checked), _
                                 txtValidDateB.ID & "=" & txtValidDateB.Text, _
                                 txtValidDateE.ID & "=" & txtValidDateE.Text, _
                                 chkAdditionCompID.ID & "=" & CStr(chkAdditionCompID.Checked), _
                                 ddlAdditionCompID.ID & "=" & ddlAdditionCompID.SelectedValue, _
                                 chkAdditionDeptID.ID & "=" & CStr(chkAdditionDeptID.Checked), _
                                 ddlAdditionDeptID.ID & "=" & ddlAdditionDeptID.SelectedValue, _
                                 "SelectCompRoleID=" & strCompID, _
                                 "SelectCompID=" & gvMain.DataKeys(Me.selectedRow(gvMain))("CompID").ToString(), _
                                 "SelectEmpID=" & gvMain.DataKeys(Me.selectedRow(gvMain))("EmpID").ToString(), _
                                 "SelectValidDate=" & gvMain.DataKeys(Me.selectedRow(gvMain))("ValidDate").ToString(), _
                                 "SelectAddCompID=" & gvMain.DataKeys(Me.selectedRow(gvMain))("AddCompID").ToString(), _
                                 "SelectAddDeptID=" & gvMain.DataKeys(Me.selectedRow(gvMain))("AddDeptID").ToString(), _
                                 "SelectAddOrganID=" & gvMain.DataKeys(Me.selectedRow(gvMain))("AddOrganID").ToString(), _
                                 "PageNo=" & pcMain.PageNo.ToString(), _
                                 "DoQuery=" & Bsp.Utility.IsStringNull(ViewState.Item("DoQuery")))
        End If
    End Sub
    Private Sub DoClear()
        IsDoQuery.Value = ""
        ViewState.Item("DoQuery") = ""
        'rbEmpAdditionDetail.Checked = True

        chkDeptID.Checked = False
        ddlDeptID.SelectedValue = ""

        chkEmpID.Checked = False
        txtEmpID.Text = ""

        chkName.Checked = False
        txtName.Text = ""

        chkReason.Checked = False
        ddlReason.SelectedIndex = 0

        chkValidDate.Checked = False
        txtValidDateB.Text = ""
        txtValidDateE.Text = ""

        chkAdditionCompID.Checked = False
        ddlAdditionCompID.SelectedValue = ""

        chkAdditionDeptID.Checked = False
        ddlAdditionDeptID.SelectedValue = ""
        gvMain.Visible = False

    End Sub

    Private Sub DoQuery()
        Dim objHR1600 As New HR1600()
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
                If rbEmpAddition.Checked Then
                    pcMain.DataTable = objHR1600.QueryEmpAddition( _
                    "CompID=" & strCompID, _
                    "DeptID=" & chkDeptID.Checked.ToString() & ";" & ddlDeptID.SelectedValue, _
                    "EmpID=" & chkEmpID.Checked.ToString() & ";" & txtEmpID.Text.Trim(), _
                    "Name=" & chkName.Checked.ToString() & ";" & txtName.Text.Trim(), _
                    "Reason=" & chkReason.Checked.ToString() & ";" & ddlReason.SelectedValue, _
                    "ValidDate=" & chkValidDate.Checked.ToString() & ";" & txtValidDateB.Text.Trim() & ";" & txtValidDateE.Text.Trim(), _
                    "AdditiionCompID=" & chkAdditionCompID.Checked.ToString() & ";" & ddlAdditionCompID.SelectedValue, _
                    "AdditiionDeptID=" & chkAdditionDeptID.Checked.ToString() & ";" & ddlAdditionDeptID.SelectedValue)
                End If

                If rbEmpAdditionDetail.Checked Then
                    pcMain.DataTable = objHR1600.QueryEmpAdditionDetail( _
                    "CompID=" & strCompID, _
                    "DeptID=" & chkDeptID.Checked.ToString() & ";" & ddlDeptID.SelectedValue, _
                    "EmpID=" & chkEmpID.Checked.ToString() & ";" & txtEmpID.Text.Trim(), _
                    "Name=" & chkName.Checked.ToString() & ";" & txtName.Text.Trim(), _
                    "Reason=" & chkReason.Checked.ToString() & ";" & ddlReason.SelectedValue, _
                    "ValidDate=" & chkValidDate.Checked.ToString() & ";" & txtValidDateB.Text.Trim() & ";" & txtValidDateE.Text.Trim(), _
                    "AdditiionCompID=" & chkAdditionCompID.Checked.ToString() & ";" & ddlAdditionCompID.SelectedValue, _
                    "AdditiionDeptID=" & chkAdditionDeptID.Checked.ToString() & ";" & ddlAdditionDeptID.SelectedValue)
                End If
                
            Catch ex As Exception
                Bsp.Utility.ShowMessage(Me, Me.FunID & ".DoQuery", ex)
            End Try
        End If

    End Sub

    Private Sub DoDelete()
        If rbEmpAddition.Checked Then
            Bsp.Utility.ShowFormatMessage(Me, "員工調兼現況不可直接刪除，請選擇員工調兼資料")
            Return
        End If
        If selectedRow(gvMain) < 0 Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00000")
        Else
            Dim beEmpAddition As New beEmpAddition.Row()
            Dim beEmpAdditionDetail As New beEmpAdditionDetail.Row()
            Dim beEmpAdditionLog As New beEmpAdditionLog.Row()
            Dim objHR1600 As New HR1600

            'EmpAdditionDetail
            beEmpAdditionDetail.ValidDate.Value = gvMain.DataKeys(Me.selectedRow(gvMain))("ValidDate").ToString()
            beEmpAdditionDetail.CompID.Value = gvMain.DataKeys(Me.selectedRow(gvMain))("CompID").ToString()
            beEmpAdditionDetail.EmpID.Value = gvMain.DataKeys(Me.selectedRow(gvMain))("EmpID").ToString()
            beEmpAdditionDetail.AddCompID.Value = gvMain.DataKeys(Me.selectedRow(gvMain))("AddCompID").ToString()
            beEmpAdditionDetail.AddDeptID.Value = gvMain.DataKeys(Me.selectedRow(gvMain))("AddDeptID").ToString()
            beEmpAdditionDetail.AddOrganID.Value = gvMain.DataKeys(Me.selectedRow(gvMain))("AddOrganID").ToString()

            'EmpAddition
            beEmpAddition.ValidDate.Value = gvMain.DataKeys(Me.selectedRow(gvMain))("ValidDate").ToString()
            beEmpAddition.CompID.Value = gvMain.DataKeys(Me.selectedRow(gvMain))("CompID").ToString()
            beEmpAddition.EmpID.Value = gvMain.DataKeys(Me.selectedRow(gvMain))("EmpID").ToString()
            beEmpAddition.AddCompID.Value = gvMain.DataKeys(Me.selectedRow(gvMain))("AddCompID").ToString()
            beEmpAddition.AddDeptID.Value = gvMain.DataKeys(Me.selectedRow(gvMain))("AddDeptID").ToString()
            beEmpAddition.AddOrganID.Value = gvMain.DataKeys(Me.selectedRow(gvMain))("AddOrganID").ToString()

            'EmpAdditionLog
            beEmpAdditionLog.ValidDate.Value = gvMain.DataKeys(Me.selectedRow(gvMain))("ValidDate").ToString()
            beEmpAdditionLog.CompID.Value = gvMain.DataKeys(Me.selectedRow(gvMain))("CompID").ToString()
            beEmpAdditionLog.EmpID.Value = gvMain.DataKeys(Me.selectedRow(gvMain))("EmpID").ToString()
            beEmpAdditionLog.AddCompID.Value = gvMain.DataKeys(Me.selectedRow(gvMain))("AddCompID").ToString()
            beEmpAdditionLog.AddDeptID.Value = gvMain.DataKeys(Me.selectedRow(gvMain))("AddDeptID").ToString()
            beEmpAdditionLog.AddOrganID.Value = gvMain.DataKeys(Me.selectedRow(gvMain))("AddOrganID").ToString()

            Try
                objHR1600.DeleteEmpAddition(beEmpAddition, beEmpAdditionDetail, beEmpAdditionLog)
            Catch ex As Exception
                Bsp.Utility.ShowMessage(Me, Me.FunID & ".DoDelete", ex)
            End Try
            DoQuery()
        End If

    End Sub
    
    
    Public Overrides Sub DoModalReturn(ByVal returnValue As String)
        
    End Sub
    Private Function funCheckData() As Boolean
        '查詢條件必須選擇
        If Not chkDeptID.Checked And Not chkEmpID.Checked And Not chkName.Checked And Not chkReason.Checked And Not chkValidDate.Checked And Not chkAdditionCompID.Checked And Not chkAdditionDeptID.Checked Then
            Bsp.Utility.ShowFormatMessage(Me, "W_C1A00")
            Return False
        End If

        '現任部門
        If chkDeptID.Checked Then
            If ddlDeptID.SelectedValue = "" Then
                Bsp.Utility.ShowFormatMessage(Me, "W_00035", "現任部門")
                ddlDeptID.Focus()
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

        '狀態
        If chkReason.Checked Then
            If ddlReason.SelectedValue = "" Then
                Bsp.Utility.ShowFormatMessage(Me, "W_00035", "狀態")
                ddlReason.Focus()
                Return False
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
                If txtValidDateB.Text > txtValidDateE.Text Then
                    Bsp.Utility.ShowFormatMessage(Me, "W_00130", "")
                    txtValidDateE.Focus()
                    Return False
                End If
            End If
        End If

        '兼任公司
        If chkAdditionCompID.Checked Then
            If ddlAdditionCompID.SelectedValue = "" Then
                Bsp.Utility.ShowFormatMessage(Me, "W_00035", "兼任公司")
                ddlAdditionCompID.Focus()
                Return False
            End If
        End If

        '兼任部門
        If chkAdditionDeptID.Checked Then
            If ddlAdditionDeptID.SelectedValue = "" Then
                Bsp.Utility.ShowFormatMessage(Me, "W_00035", "兼任部門")
                ddlAdditionDeptID.Focus()
                Return False
            End If
        End If

        Return True
    End Function

    Protected Sub gvMain_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvMain.RowCommand
        If e.CommandName = "Detail" Then
            Dim btnX As New ButtonState(ButtonState.emButtonType.Exit)

            btnX.Caption = "返回"

            Dim strCompID As String
            If UserProfile.SelectCompRoleID = "ALL" Then
                strCompID = ddlCompID.SelectedValue
            Else
                strCompID = UserProfile.SelectCompRoleID
            End If

            If rbEmpAddition.Checked Then
                Me.TransferFramePage("~/HR1/HR1612.aspx", New ButtonState() {btnX}, _
                                 rbEmpAddition.ID & "=" & CStr(rbEmpAddition.Checked), _
                                 rbEmpAdditionDetail.ID & "=" & CStr(rbEmpAdditionDetail.Checked), _
                                 chkDeptID.ID & "=" & CStr(chkDeptID.Checked), _
                                 ddlDeptID.ID & "=" & ddlDeptID.SelectedValue, _
                                 chkEmpID.ID & "=" & CStr(chkEmpID.Checked), _
                                 txtEmpID.ID & "=" & txtEmpID.Text, _
                                 chkName.ID & "=" & CStr(chkName.Checked), _
                                 txtName.ID & "=" & txtName.Text, _
                                 chkReason.ID & "=" & CStr(chkReason.Checked), _
                                 ddlReason.ID & "=" & ddlReason.SelectedValue, _
                                 chkValidDate.ID & "=" & CStr(chkValidDate.Checked), _
                                 txtValidDateB.ID & "=" & txtValidDateB.Text, _
                                 txtValidDateE.ID & "=" & txtValidDateE.Text, _
                                 chkAdditionCompID.ID & "=" & CStr(chkAdditionCompID.Checked), _
                                 ddlAdditionCompID.ID & "=" & ddlAdditionCompID.SelectedValue, _
                                 chkAdditionDeptID.ID & "=" & CStr(chkAdditionDeptID.Checked), _
                                 ddlAdditionDeptID.ID & "=" & ddlAdditionDeptID.SelectedValue, _
                                 "SelectCompRoleID=" & strCompID, _
                                 "SelectCompID=" & gvMain.DataKeys(Me.selectedRow(gvMain))("CompID").ToString(), _
                                 "SelectEmpID=" & gvMain.DataKeys(Me.selectedRow(gvMain))("EmpID").ToString(), _
                                 "SelectValidDate=" & gvMain.DataKeys(Me.selectedRow(gvMain))("ValidDate").ToString(), _
                                 "SelectAddCompID=" & gvMain.DataKeys(Me.selectedRow(gvMain))("AddCompID").ToString(), _
                                 "SelectAddDeptID=" & gvMain.DataKeys(Me.selectedRow(gvMain))("AddDeptID").ToString(), _
                                 "SelectAddOrganID=" & gvMain.DataKeys(Me.selectedRow(gvMain))("AddOrganID").ToString(), _
                                 "PageNo=" & pcMain.PageNo.ToString(), _
                                 "DoQuery=" & Bsp.Utility.IsStringNull(ViewState.Item("DoQuery")))
            End If
            If rbEmpAdditionDetail.Checked Then
                Me.TransferFramePage("~/HR1/HR1602.aspx", New ButtonState() {btnX}, _
                                 rbEmpAddition.ID & "=" & CStr(rbEmpAddition.Checked), _
                                 rbEmpAdditionDetail.ID & "=" & CStr(rbEmpAdditionDetail.Checked), _
                                 chkDeptID.ID & "=" & CStr(chkDeptID.Checked), _
                                 ddlDeptID.ID & "=" & ddlDeptID.SelectedValue, _
                                 chkEmpID.ID & "=" & CStr(chkEmpID.Checked), _
                                 txtEmpID.ID & "=" & txtEmpID.Text, _
                                 chkName.ID & "=" & CStr(chkName.Checked), _
                                 txtName.ID & "=" & txtName.Text, _
                                 chkReason.ID & "=" & CStr(chkReason.Checked), _
                                 ddlReason.ID & "=" & ddlReason.SelectedValue, _
                                 chkValidDate.ID & "=" & CStr(chkValidDate.Checked), _
                                 txtValidDateB.ID & "=" & txtValidDateB.Text, _
                                 txtValidDateE.ID & "=" & txtValidDateE.Text, _
                                 chkAdditionCompID.ID & "=" & CStr(chkAdditionCompID.Checked), _
                                 ddlAdditionCompID.ID & "=" & ddlAdditionCompID.SelectedValue, _
                                 chkAdditionDeptID.ID & "=" & CStr(chkAdditionDeptID.Checked), _
                                 ddlAdditionDeptID.ID & "=" & ddlAdditionDeptID.SelectedValue, _
                                 "SelectCompRoleID=" & strCompID, _
                                 "SelectCompID=" & gvMain.DataKeys(Me.selectedRow(gvMain))("CompID").ToString(), _
                                 "SelectEmpID=" & gvMain.DataKeys(Me.selectedRow(gvMain))("EmpID").ToString(), _
                                 "SelectValidDate=" & gvMain.DataKeys(Me.selectedRow(gvMain))("ValidDate").ToString(), _
                                 "SelectAddCompID=" & gvMain.DataKeys(Me.selectedRow(gvMain))("AddCompID").ToString(), _
                                 "SelectAddDeptID=" & gvMain.DataKeys(Me.selectedRow(gvMain))("AddDeptID").ToString(), _
                                 "SelectAddOrganID=" & gvMain.DataKeys(Me.selectedRow(gvMain))("AddOrganID").ToString(), _
                                 "PageNo=" & pcMain.PageNo.ToString(), _
                                 "DoQuery=" & Bsp.Utility.IsStringNull(ViewState.Item("DoQuery")))
            End If



        End If
    End Sub
    Private Sub DoDownload()
        Try
            If IsDoQuery.Value = "Y" Then
                Dim strCompID As String = ""
                Dim objHR1600 As New HR1600
                If pcMain.DataTable.Rows.Count > 0 Then
                    '產出檔頭
                    Dim strFileName As String = ""


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

                    If rbEmpAddition.Checked Then
                        strFileName = Bsp.Utility.GetNewFileName("調兼現況資料下載-") & ".xls"
                        gvExport.DataSource = objHR1600.QueryEmpAdditionByDownload( _
                        "CompID=" & strCompID, _
                        "DeptID=" & chkDeptID.Checked.ToString() & ";" & ddlDeptID.SelectedValue, _
                        "EmpID=" & chkEmpID.Checked.ToString() & ";" & txtEmpID.Text.Trim(), _
                        "Name=" & chkName.Checked.ToString() & ";" & txtName.Text.Trim(), _
                        "Reason=" & chkReason.Checked.ToString() & ";" & ddlReason.SelectedValue, _
                        "ValidDate=" & chkValidDate.Checked.ToString() & ";" & txtValidDateB.Text.Trim() & ";" & txtValidDateE.Text.Trim(), _
                        "AdditiionCompID=" & chkAdditionCompID.Checked.ToString() & ";" & ddlAdditionCompID.SelectedValue, _
                        "AdditiionDeptID=" & chkAdditionDeptID.Checked.ToString() & ";" & ddlAdditionDeptID.SelectedValue)
                    End If
                    If rbEmpAdditionDetail.Checked Then
                        strFileName = Bsp.Utility.GetNewFileName("調兼資料下載-") & ".xls"
                        gvExport.DataSource = objHR1600.QueryEmpAdditionDetailByDownload( _
                        "CompID=" & strCompID, _
                        "DeptID=" & chkDeptID.Checked.ToString() & ";" & ddlDeptID.SelectedValue, _
                        "EmpID=" & chkEmpID.Checked.ToString() & ";" & txtEmpID.Text.Trim(), _
                        "Name=" & chkName.Checked.ToString() & ";" & txtName.Text.Trim(), _
                        "Reason=" & chkReason.Checked.ToString() & ";" & ddlReason.SelectedValue, _
                        "ValidDate=" & chkValidDate.Checked.ToString() & ";" & txtValidDateB.Text.Trim() & ";" & txtValidDateE.Text.Trim(), _
                        "AdditiionCompID=" & chkAdditionCompID.Checked.ToString() & ";" & ddlAdditionCompID.SelectedValue, _
                        "AdditiionDeptID=" & chkAdditionDeptID.Checked.ToString() & ";" & ddlAdditionDeptID.SelectedValue)
                    End If
                    
                    'AddHandler gvExport.RowDataBound, AddressOf gvExport_RowDataBound   '20140103 wei add 增加自訂事件
                    gvExport.DataBind()

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
                    Bsp.Utility.ShowFormatMessage(Me, "請先查詢有資料，才能下傳!")
                End If
            Else
                Bsp.Utility.ShowFormatMessage(Me, "請先查詢有資料，才能下傳!")
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
                'e.Row.Cells(7).Visible = False
            Case DataControlRowType.DataRow
                'e.Row.Cells(7).Visible = False
                'e.Row.Cells(20).Text = GetPosition(DataBinder.Eval(e.Row.DataItem, "NewCompID").ToString(), DataBinder.Eval(e.Row.DataItem, "主要職位").ToString(), True)
                'e.Row.Cells(21).Text = GetPosition(DataBinder.Eval(e.Row.DataItem, "NewCompID").ToString(), DataBinder.Eval(e.Row.DataItem, "主要職位").ToString(), False)
                'e.Row.Cells(22).Text = GetWorkType(DataBinder.Eval(e.Row.DataItem, "NewCompID").ToString(), DataBinder.Eval(e.Row.DataItem, "主要工作性質").ToString(), True)
                'e.Row.Cells(23).Text = GetWorkType(DataBinder.Eval(e.Row.DataItem, "NewCompID").ToString(), DataBinder.Eval(e.Row.DataItem, "主要工作性質").ToString(), False)

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
    
    Protected Sub ddlAdditionCompID_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlAdditionCompID.SelectedIndexChanged
        '兼任部門
        Bsp.Utility.FillOrganization(ddlAdditionDeptID, ddlAdditionCompID.SelectedValue, "N")
        ddlAdditionDeptID.Items.Insert(0, New ListItem("", ""))
        UpdAdditionDeptID.Update()
    End Sub

    Protected Sub ddlCompID_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlCompID.SelectedIndexChanged
        '現任部門
        Bsp.Utility.FillOrganization(ddlDeptID, ddlCompID.SelectedValue, "N")
        ddlDeptID.Items.Insert(0, New ListItem("", ""))
        UpdDeptID.Update()
    End Sub

    Protected Sub rbEmpAddition_CheckedChanged(sender As Object, e As System.EventArgs) Handles rbEmpAddition.CheckedChanged
        DoClear()
    End Sub

    Protected Sub rbEmpAdditionDetail_CheckedChanged(sender As Object, e As System.EventArgs) Handles rbEmpAdditionDetail.CheckedChanged
        DoClear()
    End Sub
End Class
