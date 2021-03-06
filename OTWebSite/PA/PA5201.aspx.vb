'****************************************************
'功能說明：功能負責人維護-新增
'建立人員：MickySung
'建立日期：2015.05.25
'****************************************************
Imports System.Data

Partial Class PA_PA5201
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then
            Dim objSC As New SC
            '2015/05/28 公司代碼-名稱改寫法
            lbltxtCompID.Text = UserProfile.SelectCompRoleName
            'lbltxtCompID.Text = UserProfile.SelectCompRoleID + "-" + objSC.GetCompName(UserProfile.SelectCompRoleID).Rows(0).Item("CompName").ToString

            '主管公司代碼、主管編號
            ucQueryEmp.ShowCompRole = "False"
            ucQueryEmp.InValidFlag = "N"

            '功能代碼
            PA5.FillDDL(ddlFunctionID, "HRCodeMap", "Code", "CodeCName", PA5.DisplayType.Full, " AND TabName = 'Maintain' AND FldName = 'FunctionID'", "", "")
            ddlFunctionID.Items.Insert(0, New ListItem("---請選擇---", ""))

            '2015/07/31 公司代碼/員工編號-改寫
            Bsp.Utility.FillHRCompany(ddlCompID)
            ddlCompID.Items.Insert(0, New ListItem("---請選擇---", ""))
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
        Dim beMaintain As New beMaintain.Row()
        Dim bsMaintain As New beMaintain.Service()
        Dim objPA As New PA5

        '取得輸入資料
        beMaintain.CompID.Value = UserProfile.SelectCompRoleID
        beMaintain.FunctionID.Value = ddlFunctionID.SelectedValue
        beMaintain.Role.Value = ddlRole.SelectedValue
        beMaintain.EmpComp.Value = ddlCompID.SelectedValue
        beMaintain.EmpID.Value = txtEmpID.Text
        beMaintain.Telephone.Value = txtTelephone.Text
        beMaintain.Fax.Value = txtFax.Text
        beMaintain.LastChgComp.Value = UserProfile.ActCompID
        beMaintain.LastChgID.Value = UserProfile.ActUserID
        beMaintain.LastChgDate.Value = Now

        '檢查資料是否存在
        If bsMaintain.IsDataExists(beMaintain) Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00010", "")
            Return False
        End If

        '儲存資料
        Try
            Return objPA.AddMaintainSetting(beMaintain)
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, "[CC0201_99]" & Bsp.Utility.getMessage("E_00000") & Bsp.Utility.getInnerException(Me.FunID, ex))
            Return False
        End Try
    End Function

    Private Function funCheckData() As Boolean
        Dim objPA As New PA1()
        Dim beMaintain As New beMaintain.Row()
        Dim bsMaintain As New beMaintain.Service()

        '功能代碼
        If ddlFunctionID.SelectedValue = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblFunctionID.Text)
            ddlFunctionID.Focus()
            Return False
        End If

        '角色
        If ddlRole.SelectedValue = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblRole.Text)
            ddlRole.Focus()
            Return False
        End If

        '公司代碼
        If ddlCompID.SelectedValue = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", "公司代碼")
            ddlCompID.Focus()
            Return False
        End If

        '員工編號
        If txtEmpID.Text = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", "員工編號")
            Return False
        End If

        '電話
        If txtTelephone.Text = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblTelephone.Text)
            txtTelephone.Focus()
            Return False
        End If
        If Bsp.Utility.getStringLength(txtTelephone.Text.Trim) > txtTelephone.MaxLength Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00040", lblTelephone.Text, txtTelephone.MaxLength.ToString)
            Return False
        End If

        '傳真
        If txtFax.Text = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblFax.Text)
            txtFax.Focus()
            Return False
        End If
        If Bsp.Utility.getStringLength(txtFax.Text.Trim) > txtFax.MaxLength Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00040", lblFax.Text, txtFax.MaxLength.ToString)
            Return False
        End If

        Dim objHR As New HR
        Dim rtnTable As DataTable = objHR.GetHREmpName(ddlCompID.SelectedValue, txtEmpID.Text)
        If rtnTable.Rows.Count <= 0 Then
            Bsp.Utility.ShowFormatMessage(Me, "H_00000", "人事資料尚未建檔")
            txtEmpID.Focus()
            Return False
        End If

        Return True
    End Function

    Private Sub ClearData()
        ddlFunctionID.SelectedValue = ""
        ddlRole.SelectedValue = ""
        ddlCompID.SelectedValue = ""
        txtEmpID.Text = ""
        txtEmpID.Enabled = False
        lblNameN.Text = ""
        txtTelephone.Text = ""
        txtFax.Text = ""
    End Sub

    Public Overrides Sub DoModalReturn(ByVal returnValue As String)
        Dim strSql As String = ""

        If returnValue <> "" Then
            Dim aryData() As String = returnValue.Split(":")

            Select Case aryData(0)
                Case "ucQueryEmp"
                    Dim aryValue() As String = Split(aryData(1), "|$|")
                    '主管公司代碼
                    ddlCompID.SelectedValue = aryValue(3)
                    '主管編號
                    txtEmpID.Text = aryValue(1)
                    lblNameN.Text = aryValue(2)
                    txtEmpID.Enabled = True
            End Select
        End If
    End Sub

    Protected Sub ddlCompID_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlCompID.SelectedIndexChanged
        If ddlCompID.SelectedValue = "" Then
            txtEmpID.Text = ""
            txtEmpID.Enabled = False
            lblNameN.Text = ""
        Else
            txtEmpID.Text = ""
            txtEmpID.Enabled = True
            lblNameN.Text = ""
        End If
    End Sub

    Protected Sub txtEmpID_TextChanged(sender As Object, e As System.EventArgs) Handles txtEmpID.TextChanged
        If ddlCompID.SelectedValue <> "" And txtEmpID.Text <> "" Then
            Dim objHR As New HR
            Dim rtnTable As DataTable = objHR.GetHREmpName(ddlCompID.SelectedValue, txtEmpID.Text)
            If rtnTable.Rows.Count <= 0 Then
                lblNameN.Text = ""
                Bsp.Utility.ShowFormatMessage(Me, "H_00000", "人事資料尚未建檔")
            Else
                lblNameN.Text = rtnTable.Rows(0).Item(0)
            End If
        Else
            lblNameN.Text = ""
        End If
    End Sub
End Class
