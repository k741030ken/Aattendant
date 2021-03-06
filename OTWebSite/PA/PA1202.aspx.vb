'****************************************************
'功能說明：公司名稱設定-修改
'建立人員：MickySung
'建立日期：2015.04.23
'****************************************************
Imports System.Data

Partial Class PA_PA1202
    Inherits PageBase


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then
            Dim objPA As New PA1()
            '可維護計薪作業公司代碼 下拉選單
            PA1.FillEmpSource(ddlEmpSource)
            ddlEmpSource.Items.Insert(0, New ListItem("---請選擇---", ""))

            '員工資料來源 下拉選單
            Bsp.Utility.FillCompany(ddlPayrollMaintain, Bsp.Enums.FullNameType.CodeDefine)
            ddlPayrollMaintain.Items.Insert(0, New ListItem("---請選擇---", ""))

            '試用考核歸屬公司體系 下拉選單 '20160104 Beatrice Add
            Bsp.Utility.FillDDL(ddlProbation, "eHRMSDB", "HRCodeMap", "Code", "CodeCName", Bsp.Utility.DisplayType.Full, "", "And TabName = 'Company' and FldName = 'Probation'")
            ddlProbation.Items.Insert(0, New ListItem("---請選擇---", ""))
        End If
    End Sub
    Protected Overrides Sub BaseOnPageTransfer(ByVal ti As TransferInfo)
        If Not IsPostBack() Then
            Dim ht As Hashtable = Bsp.Utility.getHashTableFromParam(ti.Args)

            If ht.ContainsKey("SelectedCompID") Then
                ViewState.Item("CompID") = ht("SelectedCompID").ToString()
                subGetData(ht("SelectedCompID").ToString())
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

    Private Sub subGetData(ByVal CompID As String)
        Dim objPA As New PA1()
        Dim objSC As New SC()
        Dim beCompany As New beCompany.Row()
        Dim bsCompany As New beCompany.Service()

        beCompany.CompID.Value = CompID
        Try
            Using dt As DataTable = bsCompany.QueryByKey(beCompany).Tables(0)
                If dt.Rows.Count <= 0 Then Exit Sub
                beCompany = New beCompany.Row(dt.Rows(0))

                '公司代碼
                lblCompIDtxt.Text = beCompany.CompID.Value
                '公司名稱
                txtCompName.Text = beCompany.CompName.Value
                '公司名稱(簡)
                txtCompNameCN.Text = beCompany.CompNameCN.Value
                '中文名稱
                txtCompChnName.Text = beCompany.CompChnName.Value
                '公司中文地址
                txtAddress.Text = beCompany.Address.Value
                '中文名稱(簡)
                txtCompChnNameCN.Text = beCompany.CompChnNameCN.Value
                '公司中文地址(簡)
                txtAddressCN.Text = beCompany.AddressCN.Value
                '英文名稱
                txtCompEngName.Text = beCompany.CompEngName.Value
                '公司英文地址
                txtEngAddress.Text = beCompany.EngAddress.Value
                '無效註記
                chkInValidFlag.Checked = IIf(beCompany.InValidFlag.Value = "1", True, False)
                '不顯示註記
                chkNotShowFlag.Checked = IIf(beCompany.NotShowFlag.Value = "1", True, False)
                '費用分攤註記
                chkFeeShareFlag.Checked = IIf(beCompany.FeeShareFlag.Value = "1", True, False)
                '證券團保公司註記
                chkSPHSC1GrpFlag.Checked = IIf(beCompany.SPHSC1GrpFlag.Value = "1", True, False)
                '導入惠悅
                chkRankIDMapFlag.Checked = IIf(beCompany.RankIDMapFlag.Value = "1", True, False)

                '導入惠悅生效日  20151204 Beatrice Add
                txtRankIDMapValidDate.DateText = IIf(Format(beCompany.RankIDMapValidDate.Value, "yyyy/MM/dd") = "1900/01/01", "", beCompany.RankIDMapValidDate.Value.ToString("yyyy/MM/dd HH:mm:ss"))

                If chkRankIDMapFlag.Checked = True Then
                    txtRankIDMapValidDate.Enabled = True
                End If

                '不顯示職等
                chkNotShowRankID.Checked = IIf(beCompany.NotShowRankID.Value = "1", True, False)
                '不顯示工作性質
                chkNotShowWorkType.Checked = IIf(beCompany.NotShowWorkType.Value = "1", True, False)
                '資料轉入HRISDB
                chkHRISFlag.Checked = IIf(beCompany.HRISFlag.Value = "1", True, False)

                '2015/05/19 規格變更:取消繁體的checkbox
                '繁/簡體註記
                'chkCNFlag1.Checked = IIf(beCompany.CNFlag.Value = "0", True, False)
                chkCNFlag2.Checked = IIf(beCompany.CNFlag.Value = "1", True, False)
                '計薪作業歸屬體系
                Bsp.Utility.SetSelectedIndex(ddlPayroll, beCompany.Payroll.Value)
                '年曆檔歸屬體系
                Bsp.Utility.SetSelectedIndex(ddlCalendar, beCompany.Calendar.Value)
                '報到文件歸屬體系
                Bsp.Utility.SetSelectedIndex(ddlCheckInFile, beCompany.CheckInFile.Value)
                '試用考核歸屬公司體系 '20160104 Beatrice Add
                Bsp.Utility.SetSelectedIndex(ddlProbation, beCompany.Probation.Value)
                '可維護計薪作業公司代碼
                Bsp.Utility.SetSelectedIndex(ddlPayrollMaintain, beCompany.PayrollMaintain.Value)
                '員工資料來源
                Bsp.Utility.SetSelectedIndex(ddlEmpSource, beCompany.EmpSource.Value)
                '最後異動公司
                If beCompany.LastChgComp.Value.Trim <> "" Then
                    lblLastChgComp.Text = beCompany.LastChgComp.Value + "-" + objSC.GetCompName(beCompany.LastChgComp.Value).Rows(0).Item("CompName").ToString
                Else
                    lblLastChgComp.Text = ""
                End If
                '最後異動人員
                If beCompany.LastChgID.Value.Trim <> "" Then
                    Dim UserName As String = objSC.GetSC_UserName(beCompany.LastChgComp.Value, beCompany.LastChgID.Value)
                    lblLastChgID.Text = beCompany.LastChgID.Value + IIf(UserName <> "", "-" + UserName, "")
                Else
                    lblLastChgID.Text = ""
                End If
                '最後異動日期
                lblLastChgDate.Text = IIf(Format(beCompany.LastChgDate.Value, "yyyy/MM/dd") = "1900/01/01", "", beCompany.LastChgDate.Value.ToString("yyyy/MM/dd HH:mm:ss"))
            End Using
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & "subGetData", ex)
        End Try

    End Sub

    Private Function SaveData() As Boolean
        Dim beCompany As New beCompany.Row()
        Dim bsCompany As New beCompany.Service()
        Dim beSC_Company As New beSC_Company.Row()
        Dim objPA As New PA1()

        'beCompany
        beCompany.CompID.Value = lblCompIDtxt.Text
        beCompany.CompName.Value = txtCompName.Text
        beCompany.CompNameCN.Value = txtCompNameCN.Text
        beCompany.CompEngName.Value = txtCompEngName.Text
        beCompany.CompChnName.Value = txtCompChnName.Text
        beCompany.CompChnNameCN.Value = txtCompChnNameCN.Text
        beCompany.FeeShareFlag.Value = IIf(chkFeeShareFlag.Checked, "1", "0")
        'beCompany.GroupID.Value = "" '20160104 Beatrice Modify
        beCompany.CheckInFile.Value = ddlCheckInFile.SelectedValue
        beCompany.Calendar.Value = ddlCalendar.SelectedValue
        beCompany.Payroll.Value = ddlPayroll.SelectedValue
        beCompany.PayrollMaintain.Value = ddlPayrollMaintain.SelectedValue
        beCompany.SPHSC1GrpFlag.Value = IIf(chkSPHSC1GrpFlag.Checked, "1", "0")
        beCompany.Probation.Value = ddlProbation.SelectedValue '20160104 Beatrice Modify
        beCompany.InValidFlag.Value = IIf(chkInValidFlag.Checked, "1", "0")
        beCompany.NotShowFlag.Value = IIf(chkNotShowFlag.Checked, "1", "0")
        beCompany.HRISFlag.Value = IIf(chkHRISFlag.Checked, "1", "0")
        beCompany.Address.Value = txtAddress.Text
        beCompany.AddressCN.Value = txtAddressCN.Text
        beCompany.EngAddress.Value = txtEngAddress.Text
        'beCompany.NotQueryFlag.Value = "0" '20160104 Beatrice Modify
        beCompany.RankIDMapFlag.Value = IIf(chkRankIDMapFlag.Checked, "1", "0")

        '20151204 Beatrice Add
        If chkRankIDMapFlag.Checked = True Then
            beCompany.RankIDMapValidDate.Value = txtRankIDMapValidDate.DateText
        Else
            beCompany.RankIDMapValidDate.Value = "1900/01/01"
        End If

        beCompany.NotShowWorkType.Value = IIf(chkNotShowWorkType.Checked, "1", "0")
        beCompany.NotShowRankID.Value = IIf(chkNotShowRankID.Checked, "1", "0")
        beCompany.EmpSource.Value = ddlEmpSource.SelectedValue
        '2015/05/19 規格變更:取消繁體的checkbox
        'If chkCNFlag1.Checked = False And chkCNFlag2.Checked = False Then
        '    beCompany.CNFlag.Value = ""
        'ElseIf chkCNFlag1.Checked = True And chkCNFlag2.Checked = False Then
        '    beCompany.CNFlag.Value = "0"
        'ElseIf chkCNFlag1.Checked = False And chkCNFlag2.Checked = True Then
        '    beCompany.CNFlag.Value = "1"
        'End If
        If chkCNFlag2.Checked = True Then
            beCompany.CNFlag.Value = "1"
        Else
            beCompany.CNFlag.Value = "0"
        End If
        beCompany.LastChgComp.Value = UserProfile.ActCompID
        beCompany.LastChgID.Value = UserProfile.ActUserID
        beCompany.LastChgDate.Value = Now

        '2015/05/25 修改beSC_Company
        beSC_Company.CompID.Value = lblCompIDtxt.Text
        beSC_Company.CompName.Value = txtCompName.Text
        beSC_Company.CompFullName.Value = txtCompChnName.Text
        beSC_Company.InValidFlag.Value = IIf(chkInValidFlag.Checked, "1", "0")
        'beSC_Company.SortOrder.Value = "" '20160104 Beatrice Modify
        beSC_Company.LastChgComp.Value = UserProfile.ActCompID
        beSC_Company.LastChgID.Value = UserProfile.ActUserID
        beSC_Company.LastChgDate.Value = Now

        '儲存資料
        Try
            Return objPA.UpdateCompanySetting(beCompany, beSC_Company)
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, "[CC0201_99]" & Bsp.Utility.getMessage("E_00000") & Bsp.Utility.getInnerException(Me.FunID, ex))
            Return False
        End Try
    End Function

    Private Function funCheckData() As Boolean
        Dim objPA As New PA1()
        Dim beCompany As New beCompany.Row()
        Dim bsCompany As New beCompany.Service()

        '公司名稱
        If txtCompName.Text.Trim = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblCompName.Text)
            txtCompName.Focus()
            Return False
        End If
        'If Bsp.Utility.getStringLength(txtCompName.Text.Trim) > txtCompName.MaxLength Then
        '    Bsp.Utility.ShowFormatMessage(Me, "W_00040", lblCompName.Text, txtCompName.MaxLength.ToString)
        '    txtCompName.Focus()
        '    Return False
        'End If

        '公司名稱(簡)
        'If Bsp.Utility.getStringLength(txtCompNameCN.Text.Trim) > txtCompNameCN.MaxLength Then
        '    Bsp.Utility.ShowFormatMessage(Me, "W_00040", lblCompNameCN.Text, txtCompNameCN.MaxLength.ToString)
        '    txtCompNameCN.Focus()
        '    Return False
        'End If

        '中文名稱
        If txtCompChnName.Text.Trim.Length > txtCompChnName.MaxLength Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00040", lblCompChnName.Text, txtCompChnName.MaxLength.ToString)
            txtCompChnName.Focus()
            Return False
        End If

        '中文名稱(簡)
        If txtCompChnNameCN.Text.Trim.Length > txtCompChnNameCN.MaxLength Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00040", lblCompChnNameCN.Text, txtCompChnNameCN.MaxLength.ToString)
            txtCompChnNameCN.Focus()
            Return False
        End If

        '公司中文地址
        If txtAddress.Text.Trim.Length > txtAddress.MaxLength Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00040", lblEngAddress.Text, txtAddress.MaxLength.ToString)
            txtCompEngName.Focus()
            Return False
        End If

        '中文地址(簡)
        If txtAddressCN.Text.Trim.Length > txtAddressCN.MaxLength Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00040", lblAddressCN.Text, txtAddressCN.MaxLength.ToString)
            txtAddressCN.Focus()
            Return False
        End If

        '公司英文名稱
        If txtCompEngName.Text.Trim.Length > txtCompEngName.MaxLength Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00040", lblCompEngName.Text, txtCompEngName.MaxLength.ToString)
            txtCompEngName.Focus()
            Return False
        End If

        '公司英文地址
        If txtEngAddress.Text.Trim.Length > txtEngAddress.MaxLength Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00040", lblEngAddress.Text, txtEngAddress.MaxLength.ToString)
            txtCompEngName.Focus()
            Return False
        End If

        '導入惠悅生效日 20151204 Beatrice Add
        If chkRankIDMapFlag.Checked = True Then
            If txtRankIDMapValidDate.DateText = "" Then
                Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblRankIDMapValidDate.Text)
                txtRankIDMapValidDate.Focus()
                Return False
            End If

            If Bsp.Utility.CheckDate(txtRankIDMapValidDate.DateText) = "" Then
                Bsp.Utility.ShowFormatMessage(Me, "W_00070", lblRankIDMapValidDate.Text)
                txtRankIDMapValidDate.Focus()
                Return False
            End If
        End If

        Return True
    End Function

    Private Sub ClearData()
        subGetData(lblCompIDtxt.Text)
    End Sub

    '2015/05/19 規格變更:取消繁體的checkbox
    '#Region "繁/簡體註記 checkbox檢核"
    '    Protected Sub chkCNFlag1_Changed(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkCNFlag1.CheckedChanged
    '        If chkCNFlag1.Checked = True Then
    '            chkCNFlag2.Checked = False
    '        End If
    '    End Sub

    '    Protected Sub chkCNFlag2_Changed(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkCNFlag2.CheckedChanged
    '        If chkCNFlag2.Checked = True Then
    '            chkCNFlag1.Checked = False
    '        End If
    '    End Sub
    '#End Region

    '20151204 Beatrice Add
    Protected Sub chkRankIDMapFlag_Changed(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkRankIDMapFlag.CheckedChanged
        If chkRankIDMapFlag.Checked = True Then
            txtRankIDMapValidDate.Enabled = True
            txtRankIDMapValidDate.DateText = DateTime.Now.ToString("yyyy/MM/dd")
        Else
            txtRankIDMapValidDate.Enabled = False
            txtRankIDMapValidDate.DateText = ""
        End If
    End Sub
End Class
