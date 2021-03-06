'****************************************************
'功能說明：工作地點設定-新增
'建立人員：MickySung
'建立日期：2015.05.14
'****************************************************
Imports System.Data

Partial Class PA_PA1901
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then
            Dim objSC As New SC
            '2015/05/28 公司代碼-名稱改寫法
            lbltxtCompID.Text = UserProfile.SelectCompRoleName
            'lbltxtCompID.Text = UserProfile.SelectCompRoleID + "-" + objSC.GetCompName(UserProfile.SelectCompRoleID).Rows(0).Item("CompName").ToString

            '2015/05/22 取消公司代碼、主管編號
            ''主管公司代碼、主管編號
            'ucQueryEmp.ShowCompRole = "False"
            'ucQueryEmp.InValidFlag = "N"

            '國別
            PA1.FillCodeNo_PA1901(ddlCodeNo)
            ddlCodeNo.Items.Insert(0, New ListItem("---請選擇---", ""))
            ddlCodeNo.Items.Insert(1, New ListItem("其他", "0"))

            '縣市代碼
            PA1.FillCityCode_PA1901(ddlCityCode)
            ddlCityCode.Items.Insert(0, New ListItem("---請選擇---", ""))

            '撥入類別   20151218 wei add
            PA1.FillDialIn_PA1901(ddlDialIn)
            ddlDialIn.Items.Insert(0, New ListItem("---請選擇---", ""))

            '撥出類別   20151218 wei add
            PA1.FillDialOut_PA1901(ddlDialOut)
            ddlDialOut.Items.Insert(0, New ListItem("---請選擇---", ""))

            '20160419 wei add 分機長度
            ddlExtYards.Items.Insert(0, New ListItem("0", "0"))
            ddlExtYards.Items.Insert(1, New ListItem("1", "1"))
            ddlExtYards.Items.Insert(2, New ListItem("2", "2"))
            ddlExtYards.Items.Insert(3, New ListItem("3", "3"))
            ddlExtYards.Items.Insert(4, New ListItem("4", "4"))
            ddlExtYards.Items.Insert(5, New ListItem("5", "5"))
            ddlExtYards.SelectedValue = 4

        End If
    End Sub

    Protected Overrides Sub BaseOnPageTransfer(ByVal ti As TransferInfo)
        If Not IsPostBack() Then

        End If
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
            Case "btnCancel"    '清除
                ClearData()
        End Select
    End Sub

    Private Sub GoBack()
        Dim ti As TransferInfo = Me.StateTransfer
        Me.TransferFramePage(ti.CallerUrl, Nothing, ti.Args)
    End Sub

    Private Function SaveData() As Boolean
        Dim beWorkSite As New beWorkSite.Row()
        Dim bsWorkSite As New beWorkSite.Service()
        Dim objPA As New PA1()

        '取得輸入資料
        beWorkSite.CompID.Value = UserProfile.SelectCompRoleID
        beWorkSite.WorkSiteID.Value = txtWorkSiteID.Text
        beWorkSite.Remark.Value = txtRemark.Text
        beWorkSite.EmpCount.Value = IIf(txtEmpCount.Text <> "", txtEmpCount.Text, "0")
        beWorkSite.BranchFlag.Value = IIf(chkBranchFlag.Checked, "1", "0")
        beWorkSite.BuildingFlag.Value = IIf(chkBuildingFlag.Checked, "1", "0")
        '2015/05/22 取消公司代碼、主管編號
        'beWorkSite.BossID.Value = txtBossID.Text
        'beWorkSite.BossCompID.Value = txtBossCompID.Text
        beWorkSite.WorkSiteCode.Value = txtWorkSiteCode.Text
        beWorkSite.CityCode.Value = ddlCityCode.SelectedValue
        beWorkSite.Address.Value = txtAddress.Text

        beWorkSite.CountryCode.Value = txtCountryCode.Text
        beWorkSite.AreaCode.Value = txtAreaCode.Text
        beWorkSite.Telephone.Value = txtTelephone.Text
        beWorkSite.ExtNo.Value = txtExtNo.Text

        '20151218 wei add 撥入類別
        beWorkSite.DialIn.Value = ddlDialIn.SelectedValue

        '20151218 wei add 撥出類別
        beWorkSite.DialOut.Value = ddlDialOut.SelectedValue

        ''電話
        'Dim Telephone As String
        'If txtCodeNo.Text.Trim <> "" Or txtTelephone1.Text <> "" Or txtTelephone2.Text <> "" Then
        '    Telephone = txtCodeNo.Text + "-" + txtTelephone1.Text + "-" + txtTelephone2.Text
        '    If txtTelephone3.Text.Trim <> "" Then
        '        Telephone += "-" + txtTelephone3.Text
        '    End If
        'Else
        '    Telephone = ""
        'End If
        'beWorkSite.Telephone.Value = Telephone
        'beWorkSite.InvoiceNo.Value = txtInvoiceNo.Text '20160419 wei del
        beWorkSite.ExtYards.Value = ddlExtYards.SelectedValue   '20160419 wei add 分機長度
        beWorkSite.LastChgComp.Value = UserProfile.ActCompID
        beWorkSite.LastChgID.Value = UserProfile.ActUserID
        beWorkSite.LastChgDate.Value = Now

        '檢查資料是否存在
        If bsWorkSite.IsDataExists(beWorkSite) Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00010", "")
            Return False
        End If

        '儲存資料
        Try
            Return objPA.AddWorkSiteSetting(beWorkSite)
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, "[CC0201_99]" & Bsp.Utility.getMessage("E_00000") & Bsp.Utility.getInnerException(Me.FunID, ex))
            Return False
        End Try
    End Function

    Private Function funCheckData() As Boolean
        Dim objPA As New PA1()
        Dim beWorkSite As New beWorkSite.Row()
        Dim bsWorkSite As New beWorkSite.Service()

        '工作地點代碼
        If txtWorkSiteID.Text = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblWorkSiteID.Text)
            txtWorkSiteID.Focus()
            Return False
        End If
        If bsWorkSite.IsDataExists(beWorkSite) Then
            Bsp.Utility.ShowFormatMessage(Me, "工作地點代碼：「" + txtWorkSiteID.Text + "」資料已存在，不可新增！", "")
            txtWorkSiteID.Focus()
            Return False
        End If
        If Bsp.Utility.getStringLength(txtWorkSiteID.Text.Trim) > txtWorkSiteID.MaxLength Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00040", lblWorkSiteID.Text, txtWorkSiteID.MaxLength.ToString)
            Return False
        End If

        '工作地點
        If txtRemark.Text = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblRemark.Text)
            txtRemark.Focus()
            Return False
        End If
        'If Bsp.Utility.getStringLength(txtRemark.Text.Trim) > txtRemark.MaxLength Then
        '    Bsp.Utility.ShowFormatMessage(Me, "W_00040", lblRemark.Text, txtRemark.MaxLength.ToString)
        '    Return False
        'End If

        '2015/05/22 取消公司代碼、主管編號
        ''主管公司代碼
        'If txtBossCompID.Text = "" Then
        '    Bsp.Utility.ShowFormatMessage(Me, "W_00030", "主管公司代碼")
        '    txtBossCompID.Focus()
        '    Return False
        'End If

        ''主管編號
        'If txtBossID.Text = "" Then
        '    Bsp.Utility.ShowFormatMessage(Me, "W_00030", "主管編號")
        '    txtBossID.Focus()
        '    Return False
        'End If

        '地址
        If Bsp.Utility.getStringLength(txtAddress.Text.Trim) > txtAddress.MaxLength Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00040", lblAddress.Text, txtAddress.MaxLength.ToString)
            Return False
        End If
        '2015/07/30 add 地址長度檢核
        If txtAddress.Text.Trim.Length > txtAddress.MaxLength Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00040", lblAddress.Text, txtAddress.MaxLength.ToString)
            txtAddress.Focus()
            Return False
        End If

        '人數
        If txtEmpCount.Text.Trim <> "" Then
            If IsNumeric(txtEmpCount.Text.Trim) = False Then
                Bsp.Utility.ShowMessage(Me, "欄位[人數]請輸入數字")
                txtEmpCount.Focus()
                Return False
            Else
                If txtEmpCount.Text.Trim < -2147483648 Or txtEmpCount.Text.Trim > 2147483647 Then
                    Bsp.Utility.ShowMessage(Me, "欄位[人數]超過最大長度")
                    txtEmpCount.Focus()
                    Return False
                End If
            End If
        End If

        Return True
    End Function

    Private Sub ClearData()
        txtWorkSiteID.Text = ""
        txtRemark.Text = ""
        txtEmpCount.Text = ""
        chkBranchFlag.Checked = False
        chkBuildingFlag.Checked = False
        '2015/05/22 取消公司代碼、主管編號
        'txtBossCompID.Text = ""
        'txtBossID.Text = ""
        ddlCityCode.SelectedValue = ""
        txtWorkSiteCode.Text = ""
        txtAddress.Text = ""
        ddlCodeNo.SelectedValue = ""
        txtCountryCode.Text = ""
        txtAreaCode.Text = ""
        txtTelephone.Text = ""
        txtExtNo.Text = ""
        'txtInvoiceNo.Text = "" '20160419 wei del
        ddlExtYards.SelectedValue = 4 '20160419 wei add 分機長度
    End Sub

    '2015/05/22 取消公司代碼、主管編號
    'Public Overrides Sub DoModalReturn(ByVal returnValue As String)
    '    Dim strSql As String = ""

    '    If returnValue <> "" Then
    '        Dim aryData() As String = returnValue.Split(":")

    '        Select Case aryData(0)
    '            Case "ucQueryEmp"
    '                Dim aryValue() As String = Split(aryData(1), "|$|")
    '                '主管公司代碼
    '                txtBossCompID.Text = aryValue(3)
    '                '主管編號
    '                txtBossID.Text = aryValue(1)
    '        End Select
    '    End If
    'End Sub

    Protected Sub ddlCodeNo_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlCodeNo.SelectedIndexChanged
        If ddlCodeNo.SelectedValue = "0" Then
            txtCountryCode.Text = ""
        Else
            txtCountryCode.Text = ddlCodeNo.SelectedValue
        End If

        UpdCountryCode.Update()
    End Sub

End Class
