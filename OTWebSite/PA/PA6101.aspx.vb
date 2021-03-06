'****************************************************
'功能說明：薪資所得扣繳稅額繳款書參數維護-新增
'建立人員：BeatriceCheng
'建立日期：2016.05.12
'****************************************************
Imports System.Data

Partial Class PA_PA6101
    Inherits PageBase


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then
            Dim objSC As New SC
            ViewState.Item("CompName") = objSC.GetSC_CompName(UserProfile.SelectCompRoleID)

            txtCompID.Text = UserProfile.SelectCompRoleName

            Bsp.Utility.FillDDL(ddlInvoiceOrganID, "eHRMSDB", "Organization", "RTRIM(OrganID)", "OrganName", Bsp.Utility.DisplayType.Full, "", "And CompID = " & Bsp.Utility.Quote(UserProfile.SelectCompRoleID) & " And OrganID = DeptID And VirtualFlag = '0' And InValidFlag = '0'")
            ddlInvoiceOrganID.Items.Insert(0, New ListItem("---請選擇---", ""))

            Bsp.Utility.FillDDL(ddlTaxCityCode, "eHRMSDB", "HRCodeMap", "Code", "CodeCName", Bsp.Utility.DisplayType.Full, "", "And TabName = 'TaxParameterOrgan' And FldName = 'TaxCityCode' And NotShowFlag = '0'")
            ddlTaxCityCode.Items.Insert(0, New ListItem("---請選擇---", ""))

            ddlTaxUnitNo.Items.Insert(0, New ListItem("---請選擇---", ""))
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
        Dim beTaxParameterOrgan As New beTaxParameterOrgan.Row()
        Dim bsTaxParameterOrgan As New beTaxParameterOrgan.Service()
        Dim objPA6 As New PA6()

        '取得輸入資料
        beTaxParameterOrgan.CompID.Value = UserProfile.SelectCompRoleID
        beTaxParameterOrgan.InvoiceNo.Value = txtInvoiceNo.Text
        beTaxParameterOrgan.InvoiceOrganID.Value = ddlInvoiceOrganID.SelectedValue
        beTaxParameterOrgan.InvoiceName.Value = txtInvoiceName.Text
        beTaxParameterOrgan.HeadOfficeFlag.Value = IIf(cbHeadOfficeFlag.Checked = True, "1", "0")
        beTaxParameterOrgan.TaxCityCode.Value = ddlTaxCityCode.SelectedValue
        beTaxParameterOrgan.TaxUnitNo.Value = ddlTaxUnitNo.SelectedValue
        beTaxParameterOrgan.LastChgComp.Value = UserProfile.ActCompID
        beTaxParameterOrgan.LastChgID.Value = UserProfile.ActUserID
        beTaxParameterOrgan.LastChgDate.Value = Now

        '檢查資料是否存在
        If bsTaxParameterOrgan.IsDataExists(beTaxParameterOrgan) Then
            Bsp.Utility.ShowMessage(Me, "統一編號「" & txtInvoiceNo.Text & "」資料已存在，不可新增")
            Return False
        End If

        '儲存資料
        Try
            Return objPA6.TaxParameterOrganAdd(beTaxParameterOrgan)
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, "[CC0201_99]" & Bsp.Utility.getMessage("E_00000") & Bsp.Utility.getInnerException(Me.FunID, ex))
            Return False
        End Try
    End Function

    Private Function funCheckData() As Boolean
        '統一編號
        If txtInvoiceNo.Text.Trim = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblInvoiceNo.Text)
            txtInvoiceNo.Focus()
            Return False
        End If

        '部門代號
        If ddlInvoiceOrganID.SelectedValue = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblInvoiceOrganID.Text)
            ddlInvoiceOrganID.Focus()
            Return False
        End If

        '名稱
        If txtInvoiceName.Text.Trim = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblInvoiceName.Text)
            txtInvoiceName.Focus()
            Return False
        End If
        If Bsp.Utility.getStringLength(txtInvoiceName.Text.Trim) > txtInvoiceName.MaxLength Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00040", lblInvoiceName.Text, txtInvoiceName.MaxLength.ToString())
            txtInvoiceName.Focus()
            Return False
        End If

        '縣市代碼
        If ddlTaxCityCode.SelectedValue = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblTaxCityCode.Text)
            ddlTaxCityCode.Focus()
            Return False
        End If

        '稽徵單位代碼
        If ddlTaxUnitNo.SelectedValue = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblTaxUnitNo.Text)
            ddlTaxUnitNo.Focus()
            Return False
        End If

        Return True
    End Function

    Private Sub ClearData()
        txtInvoiceNo.Text = ""
        ddlInvoiceOrganID.SelectedValue = ""
        txtInvoiceName.Text = ""
        cbHeadOfficeFlag.Checked = False
        txtAddress.Text = ""
        txtObligor.Text = ""
        ddlTaxCityCode.SelectedValue = ""
        ddlTaxCityCode_Changed(Nothing, Nothing)
    End Sub

    Protected Sub ddlInvoiceOrganID_Changed(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlInvoiceOrganID.SelectedIndexChanged
        If ddlInvoiceOrganID.SelectedValue <> "" Then
            txtInvoiceName.Text = ViewState.Item("CompName") + ddlInvoiceOrganID.SelectedItem.Text.Split("-")(1)

            Dim objPA6 As New PA6
            Using dt As DataTable = objPA6.ChangeOrgan(UserProfile.SelectCompRoleID, ddlInvoiceOrganID.SelectedValue)
                If dt.Rows.Count > 0 Then
                    txtAddress.Text = dt.Rows(0).Item(0)
                    txtObligor.Text = dt.Rows(0).Item(1)
                End If
            End Using
        End If
    End Sub

    Protected Sub ddlTaxCityCode_Changed(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlTaxCityCode.SelectedIndexChanged
        If ddlTaxCityCode.SelectedValue <> "" Then
            Bsp.Utility.FillDDL(ddlTaxUnitNo, "eHRMSDB", "TaxUnit", "TaxUnitNo", "TaxUnitName", Bsp.Utility.DisplayType.Full, "", "And TaxCityCode = " & Bsp.Utility.Quote(ddlTaxCityCode.SelectedValue))
        Else
            ddlTaxUnitNo.Items.Clear()
        End If
        ddlTaxUnitNo.Items.Insert(0, New ListItem("---請選擇---", ""))
    End Sub

    Protected Sub ddlTaxUnitNo_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlTaxUnitNo.SelectedIndexChanged

    End Sub
End Class
