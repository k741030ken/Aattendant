'****************************************************
'功能說明：員工家庭狀況維護-新增
'建立人員：Micky Sung
'建立日期：2015.06.04
'****************************************************
Imports System.Data

Partial Class ST_ST1401
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then
            '稱謂
            Bsp.Utility.FillRelativeID(ddlRelativeID)
            ddlRelativeID.Items.Insert(0, New ListItem("---請選擇---", ""))

            '眷屬產業別 
            Bsp.Utility.FillIndustryType(ddlIndustryType)
            ddlIndustryType.Items.Insert(0, New ListItem("---請選擇---", ""))
        End If
    End Sub

    Protected Overrides Sub BaseOnPageTransfer(ByVal ti As TransferInfo)
        If Not IsPostBack() Then
            Dim objSC As New SC
            Dim ht As Hashtable = Bsp.Utility.getHashTableFromParam(ti.Args)


            If ht.ContainsKey("SelectedCompID") Then
                ViewState.Item("CompID") = ht("SelectedCompID").ToString() '公司代碼
                ViewState.Item("EmpID") = ht("SelectedEmpID").ToString() '員工編號
                ViewState.Item("EmpName") = ht("SelectedEmpName").ToString() '員工姓名
                ViewState.Item("IDNo") = ht("SelectedIDNo").ToString() '員工身分證字號

                txtCompID.Text = ViewState.Item("CompID").ToString() + "-" + objSC.GetCompName(ViewState.Item("CompID").ToString()).Rows(0).Item("CompName").ToString
                txtEmpID.Text = ViewState.Item("EmpID").ToString()
                txtEmpName.Text = ViewState.Item("EmpName").ToString()
            End If
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
        Dim beFamily As New beFamily.Row()
        Dim bsFamily As New beFamily.Service()
        Dim objST As New ST1

        beFamily.IDNo.Value = ViewState.Item("IDNo")
        beFamily.RelativeID.Value = ddlRelativeID.SelectedValue
        beFamily.NameN.Value = txtNameN.Text
        beFamily.Name.Value = txtName.Text
        beFamily.RelativeIDNo.Value = txtRelativeIDNo.Text.ToUpper
        beFamily.BirthDate.Value = IIf(txtBirthDate.DateText <> "" And txtBirthDate.DateText <> "____/__/__", txtBirthDate.DateText, "1900/01/01")
        beFamily.Occupation.Value = txtOccupation.Text
        beFamily.IndustryType.Value = ddlIndustryType.SelectedValue
        beFamily.Company.Value = txtCompany.Text
        beFamily.IsBSPEmp.Value = ddlIsBSPEmp.SelectedValue
        beFamily.DeleteMark.Value = ddlDeleteMark.SelectedValue
        beFamily.LastChgComp.Value = UserProfile.ActCompID
        beFamily.LastChgID.Value = UserProfile.ActUserID
        beFamily.LastChgDate.Value = Now

        '檢查資料是否存在
        If bsFamily.IsDataExists(beFamily) Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00010", "")
            Return False
        End If

        '儲存資料
        Try
            Return objST.AddFamilySetting(beFamily, ViewState.Item("CompID"), ViewState.Item("EmpID"))
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, "[CC0201_99]" & Bsp.Utility.getMessage("E_00000") & Bsp.Utility.getInnerException(Me.FunID, ex))
            Return False
        End Try
    End Function

    Private Function funCheckData() As Boolean
        Dim objHR As New HR
        Dim objST As New ST1
        Dim beFamily As New beFamily.Row()
        Dim bsFamily As New beFamily.Service()

        '稱謂
        If ddlRelativeID.SelectedValue = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblRelativeID.Text)
            ddlRelativeID.Focus()
            Return False
        End If

        '眷屬姓名
        Dim NameN As String = txtNameN.Text.ToString().Trim()
        If NameN = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblNameN.Text)
            txtNameN.Focus()
            Return False
        Else
            If Bsp.Utility.getStringLength(NameN) > txtNameN.MaxLength Then
                Bsp.Utility.ShowFormatMessage(Me, "W_00040", lblNameN.Text, txtNameN.MaxLength.ToString())
                txtNameN.Focus()
                Return False
            End If
            txtNameN.Text = NameN
        End If

        '眷屬姓名(拆字)
        Dim Name As String = txtName.Text.ToString().Trim()
        If txtName.Text = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblNameN.Text)
            txtName.Focus()
            Return False
        Else
            If Bsp.Utility.getStringLength(Name) > txtName.MaxLength Then
                Bsp.Utility.ShowFormatMessage(Me, "W_00040", "眷屬姓名(拆字)", txtName.MaxLength.ToString())
                txtName.Focus()
                Return False
            End If

            For iLoop As Integer = 1 To Len(Name.Replace("?", ""))
                If Bsp.Utility.getStringLength(Mid(Name, iLoop, 1)) = 1 And Not Char.IsLower(Mid(Name, iLoop, 1)) And Not Char.IsUpper(Mid(Name, iLoop, 1)) Then
                    Bsp.Utility.ShowFormatMessage(Me, "W_00031", "眷屬姓名(拆字)請勿輸入難字!")
                    txtName.Focus()
                    Return False
                End If
            Next
            txtName.Text = Name
        End If

        '眷屬身分證字號
        If txtRelativeIDNo.Text = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblRelativeIDNo.Text)
            txtRelativeIDNo.Focus()
            Return False
        Else
            '眷屬身分證是否重複
            If objST.checkRelativeIDNo(ViewState.Item("CompID"), ViewState.Item("EmpID"), ViewState.Item("IDNo"), txtRelativeIDNo.Text.ToUpper).Rows(0).Item(0) > 0 Then
                Bsp.Utility.ShowMessage(Me, "「相同員工或眷屬身份證字號資料已存在，請重新輸入！」")
                txtRelativeIDNo.Focus()
                Return False
            End If

            '身分證邏輯判斷
            If objHR.funCheckIDNO(txtRelativeIDNo.Text.ToUpper) = False Then
                Bsp.Utility.RunClientScript(Me, "confirmAdd()")
                Return False
            End If
        End If

        Return True
    End Function

    Private Sub ClearData()
        ddlRelativeID.SelectedValue = ""
        txtNameN.Text = ""
        txtName.Text = ""
        txtRelativeIDNo.Text = ""
        txtBirthDate.DateText = ""
        txtOccupation.Text = ""
        ddlIndustryType.SelectedValue = ""
        txtCompany.Text = ""
        ddlIsBSPEmp.SelectedValue = "0"
        ddlDeleteMark.SelectedValue = "0"
    End Sub

    Protected Sub btnYes_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnYes.Click

    End Sub

    Protected Sub btnNo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNo.Click
        If SaveData() Then
            GoBack()
        End If
    End Sub

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
End Class
