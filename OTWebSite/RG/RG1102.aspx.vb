'****************************************************
'功能說明：新進臨時人員資料輸入-修改
'建立人員：MickySung
'建立日期：2015.06.29
'****************************************************
Imports System.Data
Imports Newtonsoft.Json

Partial Class RG_RG1102
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then
            ViewState.Item("IDConfirm") = "N" '20150108 Beatrice Add

            '學歷
            Bsp.Utility.FillEduID(ddlEduID)
            ddlEduID.Items.Insert(0, New ListItem("---請選擇---", ""))

            '校名
            ucSelectSchool.QuerySQL = "Select SchoolID, Remark as School from School order by SchoolID"
            ucSelectSchool.Fields = New FieldState() {New FieldState("SchoolID", "校名代碼", True, True), New FieldState("School", "校名", True, True)}

            '科系
            ucSelectDepart.QuerySQL = "Select DepartID, Remark as Depart from Depart Order by DepartID"
            ucSelectDepart.Fields = New FieldState() {New FieldState("DepartID", "科系代碼", True, True), New FieldState("Depart", "科系", True, True)}

            '就學狀態
            Bsp.Utility.FillDDL(ddlSchoolStatus, "eHRMSDB", "HRCodeMap", "Code", "CodeCName", Bsp.Utility.DisplayType.Full, "", " AND TabName = 'PersonalOutsourcing' AND FldName = 'SchoolStatus' AND NotShowFlag = '0' ", "")
            ddlSchoolStatus.Items.Insert(0, New ListItem("---請選擇---", ""))

            '人員屬性
            Bsp.Utility.FillDDL(ddlEmpAttrib, "eHRMSDB", "HRCodeMap", "Code", "CodeCName", Bsp.Utility.DisplayType.Full, "", " AND TabName = 'PersonalOutsourcing' AND FldName = 'EmpAttrib' AND NotShowFlag = '0' ", "")
            ddlEmpAttrib.Items.Insert(0, New ListItem("---請選擇---", ""))

            '工作地點
            ddlWorkSiteID.Items.Insert(0, New ListItem("---請選擇---", ""))

            '計薪方式
            Bsp.Utility.FillDDL(ddlSalaryUnit, "eHRMSDB", "HRCodeMap", "Code", "CodeCName", Bsp.Utility.DisplayType.Full, "", " AND TabName = 'PersonalOutsourcing' AND FldName = 'SalaryUnit' AND NotShowFlag = '0' ", "")
            ddlSalaryUnit.Items.Insert(0, New ListItem("---請選擇---", ""))

            '人才來源
            Bsp.Utility.FillDDL(ddlEmpSource, "eHRMSDB", "HRCodeMap", "Code", "CodeCName", Bsp.Utility.DisplayType.Full, "", " AND TabName = 'PersonalOutsourcing' AND FldName = 'EmpSource' AND NotShowFlag = '0' ", "")
            ddlEmpSource.Items.Insert(0, New ListItem("---請選擇---", ""))

            '派遣公司
            Bsp.Utility.FillDDL(ddlOutsourcingComp, "eHRMSDB", "HRCodeMap", "Code", "CodeCName", Bsp.Utility.DisplayType.Full, "", " AND TabName = 'PersonalOutsourcing' AND FldName = 'OutsourcingComp' AND NotShowFlag = '0' ", "")
            ddlOutsourcingComp.Items.Insert(0, New ListItem("---請選擇---", ""))

        End If
    End Sub

    Protected Overrides Sub BaseOnPageTransfer(ByVal ti As TransferInfo)
        If Not IsPostBack() Then
            Dim ht As Hashtable = Bsp.Utility.getHashTableFromParam(ti.Args)

            If ht.ContainsKey("SelectedCompID") Then
                ViewState.Item("CompID") = ht("SelectedCompID").ToString()
                subGetData(ht("SelectedCompID").ToString(), ht("SelectedEmpID").ToString())
            Else
                Return
            End If
        End If
    End Sub

    Public Overrides Sub DoAction(ByVal Param As String)
        Select Case Param
            Case "btnUpdate"   '存檔返回
                If funCheckData() Then
                    If SaveData() Then
                        GoBack()
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

    Public Overrides Sub DoModalReturn(ByVal returnValue As String)
        Dim strSql As String = ""

        If returnValue <> "" Then
            Dim aryData = returnValue.Substring(0, returnValue.IndexOf(":"))
            Dim aryValue As Dictionary(Of String, String) = JsonConvert.DeserializeObject(Of Dictionary(Of String, String))(returnValue.Replace(aryData & ":", ""))

            Select Case aryData
                Case "ucSelectSchool"
                    '校名
                    txtHighSchool.Text = aryValue.Item("School")
                Case "ucSelectDepart"
                    '科系
                    txtHighDepart.Text = aryValue.Item("Depart")
            End Select
        End If
    End Sub

    Private Function SaveData() As Boolean
        Dim objRG As New RG1()
        Dim objRegistData As New RegistData '2015/11/16 Add
        Dim bePersonalOutsourcing As New bePersonalOutsourcing.Row()
        Dim bsPersonalOutsourcing As New bePersonalOutsourcing.Service()

        bePersonalOutsourcing.CompID.Value = hidCompID.Value
        bePersonalOutsourcing.EmpID.Value = txtEmpID.Text
        bePersonalOutsourcing.NameN.Value = txtName.Text
        bePersonalOutsourcing.Name.Value = txtName.Text
        bePersonalOutsourcing.Sex.Value = ddlSex.SelectedValue
        bePersonalOutsourcing.BirthDate.Value = txtBirthDate.DateText
        bePersonalOutsourcing.IDNo.Value = txtIDNo.Text.ToUpper()
        bePersonalOutsourcing.EduID.Value = ddlEduID.SelectedValue
        bePersonalOutsourcing.HighSchool.Value = txtHighSchool.Text
        bePersonalOutsourcing.HighDepart.Value = txtHighDepart.Text
        bePersonalOutsourcing.SchoolStatus.Value = ddlSchoolStatus.SelectedValue
        bePersonalOutsourcing.EmpAttrib.Value = ddlEmpAttrib.SelectedValue
        bePersonalOutsourcing.ContractStartDate.Value = txtContractStartDate.DateText
        bePersonalOutsourcing.ContractQuitDate.Value = txtContractQuitDate.DateText
        bePersonalOutsourcing.EmpDate.Value = txtEmpDate.DateText
        bePersonalOutsourcing.GroupID.Value = hidGroupID.Value
        bePersonalOutsourcing.DeptID.Value = ddlOrganDept.SelectedDeptID
        bePersonalOutsourcing.OrganID.Value = ddlOrganDept.SelectedOrganID
        bePersonalOutsourcing.WorkSiteID.Value = ddlWorkSiteID.SelectedValue
        bePersonalOutsourcing.CommTel.Value = txtCommTel.Text
        bePersonalOutsourcing.RelName.Value = txtRelName.Text
        bePersonalOutsourcing.CommAddr.Value = txtCommAddr.Text
        bePersonalOutsourcing.FeeShareDept.Value = ddlFeeShareDept.SelectedValue
        bePersonalOutsourcing.SalaryUnit.Value = ddlSalaryUnit.SelectedValue

        'bePersonalOutsourcing.Salary.Value = txtSalary.Text
        bePersonalOutsourcing.Salary.Value = objRegistData.funEncryptNumber(txtEmpID.Text, txtSalary.Text)  '2015/11/16 Modify 加入加密
        'bePersonalOutsourcing.Allowance.Value = txtAllowance.Text
        bePersonalOutsourcing.Allowance.Value = objRegistData.funEncryptNumber(txtEmpID.Text, txtAllowance.Text)  '2015/11/16 Modify 加入加密

        bePersonalOutsourcing.EmpSource.Value = ddlEmpSource.SelectedValue
        bePersonalOutsourcing.OutsourcingComp.Value = ddlOutsourcingComp.SelectedValue
        bePersonalOutsourcing.LastChgComp.Value = UserProfile.ActCompID
        bePersonalOutsourcing.LastChgID.Value = UserProfile.ActUserID
        bePersonalOutsourcing.LastChgDate.Value = Now

        '儲存資料
        Try
            Return objRG.UpdatePersonalOutsourcingSetting(bePersonalOutsourcing)
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, "[CC0201_99]" & Bsp.Utility.getMessage("E_00000") & Bsp.Utility.getInnerException(Me.FunID, ex))
            Return False
        End Try
    End Function

    Private Sub subGetData(ByVal CompID As String, ByVal EmpID As String)
        Dim objRG As New RG1()
        Dim objSC As New SC()
        Dim objRegistData As New RegistData '2015/11/17 Add
        Dim bePersonalOutsourcing As New bePersonalOutsourcing.Row()
        Dim bsPersonalOutsourcing As New bePersonalOutsourcing.Service()

        bePersonalOutsourcing.CompID.Value = CompID
        bePersonalOutsourcing.EmpID.Value = EmpID
        Try
            Using dt As DataTable = bsPersonalOutsourcing.QueryByKey(bePersonalOutsourcing).Tables(0)
                If dt.Rows.Count <= 0 Then Exit Sub
                bePersonalOutsourcing = New bePersonalOutsourcing.Row(dt.Rows(0))

                '公司代碼
                txtCompID.Text = bePersonalOutsourcing.CompID.Value
                hidCompID.Value = bePersonalOutsourcing.CompID.Value
                '員工編號
                txtEmpID.Text = bePersonalOutsourcing.EmpID.Value
                '員工姓名
                txtName.Text = bePersonalOutsourcing.NameN.Value.Trim
                '性別
                ddlSex.SelectedValue = bePersonalOutsourcing.Sex.Value.Trim
                '出生日期
                txtBirthDate.DateText = Format(bePersonalOutsourcing.BirthDate.Value, "yyyy/MM/dd")
                '身分證字號
                txtIDNo.Text = bePersonalOutsourcing.IDNo.Value.Trim
                hidIDNo.Value = bePersonalOutsourcing.IDNo.Value.Trim

                '學歷
                ddlEduID.SelectedValue = bePersonalOutsourcing.EduID.Value.Trim
                '最高學歷學校
                txtHighSchool.Text = bePersonalOutsourcing.HighSchool.Value.Trim
                '最高學歷科系
                txtHighDepart.Text = bePersonalOutsourcing.HighDepart.Value.Trim
                '就學狀態
                ddlSchoolStatus.SelectedValue = bePersonalOutsourcing.SchoolStatus.Value.Trim
                '人員屬性
                ddlEmpAttrib.SelectedValue = bePersonalOutsourcing.EmpAttrib.Value.Trim
                '契約起日
                txtContractStartDate.DateText = Format(bePersonalOutsourcing.ContractStartDate.Value, "yyyy/MM/dd")
                '契約迄日
                txtContractQuitDate.DateText = Format(bePersonalOutsourcing.ContractQuitDate.Value, "yyyy/MM/dd")
                '到職日
                txtEmpDate.DateText = Format(bePersonalOutsourcing.EmpDate.Value, "yyyy/MM/dd")

                '錄用部門、科/組/課
                ddlOrganDept.LoadData(hidCompID.Value)
                ddlOrganDept.setDeptID(hidCompID.Value, bePersonalOutsourcing.DeptID.Value.Trim, "")
                ddlOrganDept.setOrganID(hidCompID.Value, bePersonalOutsourcing.OrganID.Value.Trim, "")
                '工作地點
                Bsp.Utility.FillDDL(ddlWorkSiteID, "eHRMSDB", "WorkSite", "WorkSiteID", "Remark", Bsp.Utility.DisplayType.Full, "", " AND CompID = '" + hidCompID.Value + "' AND WorkSiteID = '" + bePersonalOutsourcing.WorkSiteID.Value.Trim + "' ", "")
                ddlWorkSiteID.Items.Insert(0, New ListItem("---請選擇---", ""))
                ddlWorkSiteID.SelectedValue = bePersonalOutsourcing.WorkSiteID.Value.Trim
                '事業單位
                hidGroupID.Value = bePersonalOutsourcing.GroupID.Value.Trim

                '聯絡電話
                txtCommTel.Text = bePersonalOutsourcing.CommTel.Value.Trim
                '緊急聯絡人
                txtRelName.Text = bePersonalOutsourcing.RelName.Value.Trim
                '聯絡地址
                txtCommAddr.Text = bePersonalOutsourcing.CommAddr.Value.Trim

                '費用歸屬單位
                Bsp.Utility.FillDDL(ddlFeeShareDept, "eHRMSDB", "Organization", "OrganID", "OrganName", Bsp.Utility.DisplayType.Full, "", " AND CompID = '" + hidCompID.Value + "' AND VirtualFlag = '0' ", "")
                ddlFeeShareDept.Items.Insert(0, New ListItem("---請選擇---", ""))
                ddlFeeShareDept.SelectedValue = bePersonalOutsourcing.FeeShareDept.Value.Trim

                '計薪方式
                ddlSalaryUnit.SelectedValue = bePersonalOutsourcing.SalaryUnit.Value.Trim
                '薪資
                'txtSalary.Text = bePersonalOutsourcing.Salary.Value
                txtSalary.Text = objRegistData.funDecryptNumber(bePersonalOutsourcing.EmpID.Value, bePersonalOutsourcing.Salary.Value.Trim) '2015/11/17 Modify 解密
                '津貼
                'txtAllowance.Text = bePersonalOutsourcing.Allowance.Value
                txtAllowance.Text = objRegistData.funDecryptNumber(bePersonalOutsourcing.EmpID.Value, bePersonalOutsourcing.Allowance.Value.Trim) '2015/11/17 Modify 解密
                '人才來源
                ddlEmpSource.SelectedValue = bePersonalOutsourcing.EmpSource.Value.Trim
                '派遣公司
                ddlOutsourcingComp.SelectedValue = bePersonalOutsourcing.OutsourcingComp.Value.Trim

                '最後異動公司
                Dim CompName As String = objSC.GetSC_CompName(bePersonalOutsourcing.LastChgComp.Value)
                lblLastChgComp.Text = bePersonalOutsourcing.LastChgComp.Value + IIf(CompName <> "", "-" + CompName, "")

                '最後異動人員
                Dim UserName As String = objSC.GetSC_UserName(bePersonalOutsourcing.LastChgComp.Value, bePersonalOutsourcing.LastChgID.Value)
                lblLastChgID.Text = bePersonalOutsourcing.LastChgID.Value + IIf(UserName <> "", "-" + UserName, "")

                '最後異動日期
                Dim boolDate As Boolean = Format(bePersonalOutsourcing.LastChgDate.Value, "yyyy/MM/dd") = "1900/01/01"
                lblLastChgDate.Text = IIf(boolDate, "", bePersonalOutsourcing.LastChgDate.Value.ToString("yyyy/MM/dd HH:mm:ss"))
            End Using
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & "subGetData", ex)
        End Try

    End Sub

    Private Function funCheckData() As Boolean
        Dim objRG As New RG1()
        Dim objHR As New HR()
        Dim bePersonalOutsourcing As New bePersonalOutsourcing.Row()
        Dim bsPersonalOutsourcing As New bePersonalOutsourcing.Service()

        '員工姓名
        If txtName.Text.Trim = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblName.Text)
            txtName.Focus()
            Return False
        End If
        If Bsp.Utility.getStringLength(txtName.Text.Trim) > txtName.MaxLength Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00040", lblName.Text, txtName.MaxLength.ToString)
            Return False
        End If

        '性別
        If ddlSex.SelectedValue.Trim = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblSex.Text)
            ddlSex.Focus()
            Return False
        End If

        '出生日期
        If txtBirthDate.DateText.Trim = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblBirthDate.Text)
            txtBirthDate.Focus()
            Return False
        End If

        '身分證字號
        If txtIDNo.Text.Trim = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblIDNo.Text)
            txtIDNo.Focus()
            Return False
        End If

        '身分證邏輯判斷 '20150108 Beatrice Modify
        If ViewState.Item("IDConfirm") = "N" Then
            If objHR.funCheckIDNO(txtIDNo.Text.Trim) = False Then
                'Bsp.Utility.ShowMessage(Me, "身分證字號邏輯有誤")
                Bsp.Utility.RunClientScript(Me.Page, "IDConfirm();")
                Return False
            End If

            '身分證字號是否存在
            'If hidIDNo.Value <> txtIDNo.Text Then
            '    If objRG.checkIDNoFromPersonalOutsourcing(txtIDNo.Text.Trim).Rows(0).Item(0) > 0 Or objRG.checkIDNoFromPersonal(txtIDNo.Text.Trim).Rows(0).Item(0) > 0 Then
            '        Bsp.Utility.ShowMessage(Me, "「身份證字號資料已存在，請重新輸入！」")
            '        txtIDNo.Focus()
            '        Return False
            '    End If
            'End If
        End If


        '最高學歷學校
        If Bsp.Utility.getStringLength(txtHighSchool.Text.Trim) > txtHighSchool.MaxLength Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00040", lblHighSchool.Text, txtHighSchool.MaxLength.ToString)
            txtHighSchool.Focus()
            Return False
        End If

        '最高學歷科系
        If Bsp.Utility.getStringLength(txtHighDepart.Text.Trim) > txtHighDepart.MaxLength Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00040", lblHighDepart.Text, txtHighDepart.MaxLength.ToString)
            txtHighDepart.Focus()
            Return False
        End If

        '契約起日
        If txtContractStartDate.DateText.Trim = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblContractStartDate.Text)
            txtContractStartDate.Focus()
            Return False
        End If

        '契約迄日
        If txtContractQuitDate.DateText.Trim = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblContractQuitDate.Text)
            txtContractQuitDate.Focus()
            Return False
        End If

        '到職日
        If txtEmpDate.DateText.Trim = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblEmpDate.Text)
            txtEmpDate.Focus()
            Return False
        End If

        '錄用部門、科/組/課
        If ddlOrganDept.SelectedDeptID.Trim = "" Or ddlOrganDept.SelectedOrganID.Trim = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblOrganDept.Text)
            ddlOrganDept.Focus()
            Return False
        End If

        '聯絡電話
        If Bsp.Utility.getStringLength(txtCommTel.Text.Trim) > txtCommTel.MaxLength Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00040", lblCommTel.Text, txtCommTel.MaxLength.ToString)
            txtCommTel.Focus()
            Return False
        End If

        '緊急聯絡人
        If Bsp.Utility.getStringLength(txtRelName.Text.Trim) > txtRelName.MaxLength Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00040", lblRelName.Text, txtRelName.MaxLength.ToString)
            txtRelName.Focus()
            Return False
        End If

        '緊急聯絡人
        If Bsp.Utility.getStringLength(txtRelName.Text.Trim) > txtRelName.MaxLength Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00040", lblRelName.Text, txtRelName.MaxLength.ToString)
            txtRelName.Focus()
            Return False
        End If

        '聯絡地址
        If Bsp.Utility.getStringLength(txtCommAddr.Text.Trim) > txtCommAddr.MaxLength Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00040", lblCommAddr.Text, txtCommAddr.MaxLength.ToString)
            txtCommAddr.Focus()
            Return False
        End If

        '薪資
        If txtSalary.Text.Trim = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblSalary.Text)
            txtSalary.Focus()
            Return False
        Else
            If Bsp.Utility.getStringLength(txtSalary.Text.Trim) > txtSalary.MaxLength Then
                Bsp.Utility.ShowFormatMessage(Me, "W_00040", lblSalary.Text, txtSalary.MaxLength.ToString)
                Return False
            End If

            If IsNumeric(txtSalary.Text.Trim) = False Then
                Bsp.Utility.ShowMessage(Me, "薪資請輸入數字")
                txtSalary.Focus()
                Return False
            End If
        End If

        '津貼
        If txtAllowance.Text.Trim = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblAllowance.Text)
            txtAllowance.Focus()
            Return False
        Else
            If Bsp.Utility.getStringLength(txtAllowance.Text.Trim) > txtAllowance.MaxLength Then
                Bsp.Utility.ShowFormatMessage(Me, "W_00040", lblAllowance.Text, txtAllowance.MaxLength.ToString)
                Return False
            End If

            If IsNumeric(txtAllowance.Text.Trim) = False Then
                Bsp.Utility.ShowMessage(Me, "津貼請輸入數字")
                txtAllowance.Focus()
                Return False
            End If
        End If

        Return True
    End Function

    Private Sub ClearData()
        ViewState.Item("IDConfirm") = "N" '20150108 Beatrice Add
        subGetData(hidCompID.Value, txtEmpID.Text)
    End Sub

    Protected Sub ddlOrganDept_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlOrganDept.ucSelectOrganIDSelectedIndexChangedHandler_SelectChange
        Dim objRG As New RG1()
        Dim WorkSiteID As String
        Dim GroupID As String

        '工作地點
        WorkSiteID = objRG.selectWorkSite(hidCompID.Value, ddlOrganDept.SelectedDeptID, ddlOrganDept.SelectedOrganID).Rows(0).Item(0)
        Bsp.Utility.FillDDL(ddlWorkSiteID, "eHRMSDB", "WorkSite", "WorkSiteID", "Remark", Bsp.Utility.DisplayType.Full, "", " AND CompID = '" + hidCompID.Value + "' AND WorkSiteID = '" + WorkSiteID + "' ", "")
        ddlWorkSiteID.Items.Insert(0, New ListItem("---請選擇---", ""))
        Bsp.Utility.SetSelectedIndex(ddlWorkSiteID, WorkSiteID)

        '事業群代碼
        GroupID = objRG.selectWorkSite(hidCompID.Value, ddlOrganDept.SelectedDeptID, ddlOrganDept.SelectedOrganID).Rows(0).Item(1)
        hidGroupID.Value = GroupID

    End Sub

    '20150108 Beatrice Add
    Protected Sub btnIDConfirm_Click(sender As Object, e As System.EventArgs) Handles btnIDConfirm.Click
        Dim objRG As New RG1()

        If hidIDNo.Value <> txtIDNo.Text Then
            '身分證字號是否存在
            If objRG.checkIDNoFromPersonalOutsourcing(txtIDNo.Text.Trim).Rows(0).Item(0) > 0 Then
                Bsp.Utility.ShowMessage(Me, "「身份證字號已存在，不可重複輸入！」")
                txtIDNo.Focus()
                Return
            Else
                If objRG.checkIDNoFromPersonal(txtIDNo.Text.Trim).Rows(0).Item(0) > 0 Then
                    Bsp.Utility.ShowMessage(Me, "「身份證字號已經存在員工資料主檔，且為在職狀態，不可重複輸入！」")
                    txtIDNo.Focus()
                    Return
                End If
            End If
        End If

        ViewState.Item("IDConfirm") = "Y"
        DoAction("btnUpdate")
    End Sub

    '20150108 Beatrice Add
    Protected Sub txtIDNo_TextChanged(sender As Object, e As System.EventArgs) Handles txtIDNo.TextChanged
        ViewState.Item("IDConfirm") = "N"
    End Sub
End Class
