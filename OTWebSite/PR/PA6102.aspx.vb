'****************************************************
'功能說明：薪資所得扣繳稅額繳款書參數維護-修改
'建立人員：BeatriceCheng
'建立日期：2016.05.13
'****************************************************
Imports System.Data

Partial Class PA_PA6102
    Inherits PageBase


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then
            Bsp.Utility.FillDDL(ddlTaxCityCode, "eHRMSDB", "HRCodeMap", "Code", "CodeCName", Bsp.Utility.DisplayType.Full, "", "And TabName = 'TaxParameterOrgan' And FldName = 'TaxCityCode' And NotShowFlag = '0'")
            ddlTaxCityCode.Items.Insert(0, New ListItem("---請選擇---", ""))

            ddlTaxUnitNo.Items.Insert(0, New ListItem("---請選擇---", ""))
        End If
    End Sub

    Protected Overrides Sub BaseOnPageTransfer(ByVal ti As TransferInfo)
        If Not IsPostBack() Then
            Dim ht As Hashtable = Bsp.Utility.getHashTableFromParam(ti.Args)
            Dim objSC As New SC

            If ht.ContainsKey("SelectedCompID") Then
                ViewState.Item("CompID") = ht("SelectedCompID").ToString()
                ViewState.Item("InvoiceNo") = ht("SelectedInvoiceNo").ToString()
                ViewState.Item("CompName") = objSC.GetSC_CompName(ht("SelectedCompID").ToString())

                Bsp.Utility.FillDDL(ddlInvoiceOrganID, "eHRMSDB", "Organization", "RTRIM(OrganID)", "OrganName", Bsp.Utility.DisplayType.Full, "", "And CompID = " & Bsp.Utility.Quote(ViewState.Item("CompID")) & " And OrganID = DeptID And VirtualFlag = '0' And InValidFlag = '0'")
                ddlInvoiceOrganID.Items.Insert(0, New ListItem("---請選擇---", ""))

                subGetData(ViewState.Item("CompID"), ViewState.Item("InvoiceNo"))
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

    Private Function SaveData() As Boolean
        Dim beTaxParameterOrgan As New beTaxParameterOrgan.Row()
        Dim bsTaxParameterOrgan As New beTaxParameterOrgan.Service()
        Dim objPA6 As New PA6()

        '取得輸入資料
        beTaxParameterOrgan.CompID.Value = ViewState.Item("CompID")
        beTaxParameterOrgan.InvoiceNo.OldValue = ViewState.Item("InvoiceNo")
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
        If txtInvoiceNo.Text <> ViewState.Item("InvoiceNo") And bsTaxParameterOrgan.IsDataExists(beTaxParameterOrgan) Then
            Bsp.Utility.ShowMessage(Me, "統一編號「" & txtInvoiceNo.Text & "」資料已存在，不可修改")
            Return False
        End If

        '儲存資料
        Try
            Return objPA6.TaxParameterOrganUpdate(beTaxParameterOrgan)
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, "[CC0201_99]" & Bsp.Utility.getMessage("E_00000") & Bsp.Utility.getInnerException(Me.FunID, ex))
            Return False
        End Try
    End Function

    Private Sub subGetData(ByVal CompID As String, ByVal InvoiceNo As String)
        Dim objSC As New SC
        Dim beTaxParameterOrgan As New beTaxParameterOrgan.Row()
        Dim bsTaxParameterOrgan As New beTaxParameterOrgan.Service()
        Dim objPA6 As New PA6()

        beTaxParameterOrgan.CompID.Value = CompID
        beTaxParameterOrgan.InvoiceNo.Value = InvoiceNo
        Try
            Using dt As DataTable = bsTaxParameterOrgan.QueryByKey(beTaxParameterOrgan).Tables(0)
                If dt.Rows.Count <= 0 Then Exit Sub
                beTaxParameterOrgan = New beTaxParameterOrgan.Row(dt.Rows(0))

                txtCompID.Text = CompID + ViewState.Item("CompName")
                txtInvoiceNo.Text = beTaxParameterOrgan.InvoiceNo.Value

                ddlInvoiceOrganID.SelectedValue = beTaxParameterOrgan.InvoiceOrganID.Value
                ddlInvoiceOrganID_Changed(Nothing, Nothing)

                txtInvoiceName.Text = beTaxParameterOrgan.InvoiceName.Value
                cbHeadOfficeFlag.Checked = IIf(beTaxParameterOrgan.HeadOfficeFlag.Value = "1", True, False)

                ddlTaxCityCode.SelectedValue = beTaxParameterOrgan.TaxCityCode.Value
                ddlTaxCityCode_Changed(Nothing, Nothing)

                ddlTaxUnitNo.SelectedValue = beTaxParameterOrgan.TaxUnitNo.Value

                '最後異動公司
                Dim CompName As String = objSC.GetSC_CompName(beTaxParameterOrgan.LastChgComp.Value)
                txtLastChgComp.Text = beTaxParameterOrgan.LastChgComp.Value + IIf(CompName <> "", "-" + CompName, "")
                '最後異動人員
                Dim UserName As String = objSC.GetSC_UserName(beTaxParameterOrgan.LastChgComp.Value, beTaxParameterOrgan.LastChgID.Value)
                txtLastChgID.Text = beTaxParameterOrgan.LastChgID.Value + IIf(UserName <> "", "-" + UserName, "")
                '最後異動日期
                Dim boolDate As Boolean = Format(beTaxParameterOrgan.LastChgDate.Value, "yyyy/MM/dd") = "1900/01/01"
                txtLastChgDate.Text = IIf(boolDate, "", beTaxParameterOrgan.LastChgDate.Value.ToString("yyyy/MM/dd HH:mm:ss"))

            End Using
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & "subGetData", ex)
        End Try

    End Sub

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
        subGetData(ViewState.Item("CompID"), ViewState.Item("InvoiceNo"))
    End Sub

    Protected Sub ddlInvoiceOrganID_Changed(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlInvoiceOrganID.SelectedIndexChanged
        If ddlInvoiceOrganID.SelectedValue <> "" Then
            txtInvoiceName.Text = ViewState.Item("CompName") + ddlInvoiceOrganID.SelectedItem.Text.Split("-")(1)

            Dim objPA6 As New PA6
            Using dt As DataTable = objPA6.ChangeOrgan(ViewState.Item("CompID"), ddlInvoiceOrganID.SelectedValue)
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
End Class
