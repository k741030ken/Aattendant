'****************************************************
'功能說明：員工前職經歷維護-修改
'建立人員：Micky Sung
'建立日期：2015.06.09
'****************************************************
Imports System.Data

Partial Class ST_ST1502
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then
            '產業別
            Bsp.Utility.FillIndustryType(ddlIndustryType)
            ddlIndustryType.Items.Insert(0, New ListItem("---請選擇---", ""))

            '專業記號
            ddlProfession.SelectedValue = "1"
        End If
    End Sub

    Protected Overrides Sub BaseOnPageTransfer(ByVal ti As TransferInfo)
        If Not IsPostBack() Then
            Dim ht As Hashtable = Bsp.Utility.getHashTableFromParam(ti.Args)
                If ht.ContainsKey("SelectedIDNo") Then
                    ViewState.Item("CompID") = ht("SelectedCompID").ToString()
                    ViewState.Item("CompName") = ht("SelectedCompName").ToString()
                    ViewState.Item("EmpID") = ht("SelectedEmpID").ToString()
                    ViewState.Item("EmpName") = ht("SelectedEmpName").ToString()
                    ViewState.Item("IDNo") = ht("SelectedIDNo").ToString()
                    ViewState.Item("Seq") = ht("SelectedSeq").ToString()
                    subGetData(ht("SelectedIDNo").ToString(), ht("SelectedSeq").ToString())
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
        Dim beExperience As New beExperience.Row()
        Dim bsExperience As New beExperience.Service()
        Dim objST As New ST1
        
        beExperience.IDNo.Value = hidIDNo.Value
        beExperience.Seq.Value = ViewState.Item("Seq")
        beExperience.BeginDate.Value = txtBeginDate.DateText
        beExperience.EndDate.Value = txtEndDate.DateText
        beExperience.WorkYear.Value = CDec(txtWorkYear.Text)   '2015/11/24 Modify
        beExperience.Company.Value = txtCompany.Text
        beExperience.Department.Value = txtDepartment.Text
        beExperience.IndustryType.Value = ddlIndustryType.SelectedValue
        beExperience.Title.Value = txtTitle.Text
        beExperience.WorkType.Value = txtWorkType.Text
        beExperience.Profession.Value = ddlProfession.SelectedValue
        beExperience.LastChgComp.Value = UserProfile.ActCompID
        beExperience.LastChgID.Value = UserProfile.ActUserID
        beExperience.LastChgDate.Value = Now

        '儲存資料
        Try
            Return objST.UpdateExperienceSetting(beExperience)
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, "[CC0201_99]" & Bsp.Utility.getMessage("E_00000") & Bsp.Utility.getInnerException(Me.FunID, ex))
            Return False
        End Try
    End Function

    Private Function funCheckData() As Boolean
        Dim objHR As New HR
        Dim objST As New ST1
        Dim beExperience As New beExperience.Row()
        Dim bsExperience As New beExperience.Service()

        '起日
        If txtBeginDate.DateText = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblBeginDate.Text)
            txtBeginDate.Focus()
            Return False
        End If

        '迄日
        If txtEndDate.DateText = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblEndDate.Text)
            txtEndDate.Focus()
            Return False
        End If

        If txtEndDate.DateText < txtBeginDate.DateText Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00130")
            txtBeginDate.Focus()
            Return False
        End If

        '公司名稱
        If txtCompany.Text = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblCompany.Text)
            txtCompany.Focus()
            Return False
        End If

        '部門
        If txtDepartment.Text = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblDepartment.Text)
            txtDepartment.Focus()
            Return False
        End If

        '專業記號
        If ddlProfession.SelectedValue = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblProfession.Text)
            ddlProfession.Focus()
            Return False
        End If

        Return True
    End Function

    Private Sub subGetData(ByVal IDNo As String, ByVal Seq As String)
        Dim objSR As New ST1
        Dim objSC As New SC
        Dim beExperience As New beExperience.Row()
        Dim bsExperience As New beExperience.Service()

        beExperience.IDNo.Value = IDNo
        beExperience.Seq.Value = CInt(Seq)

        Try
            Using dt As DataTable = bsExperience.QueryByKey(beExperience).Tables(0)
                If dt.Rows.Count <= 0 Then Exit Sub
                beExperience = New beExperience.Row(dt.Rows(0))

                txtCompID.Text = ViewState.Item("CompID").ToString() + "-" + ViewState.Item("CompName").ToString()
                hidCompID.Value = ViewState.Item("CompID").ToString()
                txtEmpID.Text = ViewState.Item("EmpID").ToString()
                txtEmpName.Text = ViewState.Item("EmpName").ToString()
                hidIDNo.Value = ViewState.Item("IDNo").ToString()
                hidSeq.Value = ViewState.Item("Seq").ToString()
                txtBeginDate.DateText = Format(beExperience.BeginDate.Value, "yyyy/MM/dd")
                txtEndDate.DateText = Format(beExperience.EndDate.Value, "yyyy/MM/dd")
                txtWorkYear.Text = beExperience.WorkYear.Value
                txtCompany.Text = beExperience.Company.Value
                txtDepartment.Text = beExperience.Department.Value
                ddlIndustryType.SelectedValue = beExperience.IndustryType.Value
                txtTitle.Text = beExperience.Title.Value
                txtWorkType.Text = beExperience.WorkType.Value
                ddlProfession.SelectedValue = beExperience.Profession.Value

                '最後異動公司
                Dim CompName As String = objSC.GetSC_CompName(beExperience.LastChgComp.Value)
                lblLastChgComp.Text = beExperience.LastChgComp.Value + IIf(CompName <> "", "-" + CompName, "")

                '最後異動人員
                Dim UserName As String = objSC.GetSC_UserName(beExperience.LastChgComp.Value, beExperience.LastChgID.Value)
                lblLastChgID.Text = beExperience.LastChgID.Value + IIf(UserName <> "", "-" + UserName, "")

                '最後異動日期
                Dim boolDate As Boolean = Format(beExperience.LastChgDate.Value, "yyyy/MM/dd") = "1900/01/01"
                lblLastChgDate.Text = IIf(boolDate, "", beExperience.LastChgDate.Value.ToString("yyyy/MM/dd HH:mm:ss"))

            End Using
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & "subGetData", ex)
        End Try
    End Sub

    Private Sub ClearData()
        subGetData(hidIDNo.Value, hidSeq.Value)
    End Sub

    '年資
    Protected Sub btnWorkYear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnWorkYear.Click
        Dim year As Decimal = 0
        If txtBeginDate.DateText <> "" And txtEndDate.DateText <> "" Then
            year = DateDiff("d", txtBeginDate.DateText, txtEndDate.DateText) / 365
            txtWorkYear.Text = Math.Round(year, 1)
        End If
    End Sub

End Class
