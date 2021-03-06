'****************************************************
'功能說明：員工最小簽核單位維護-新增
'建立人員：Micky Sung
'建立日期：2015.06.11
'****************************************************
Imports System.Data
Imports Newtonsoft.Json

Partial Class ST_ST1201
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then
            '作業代碼
            ST1.FillDDL(ddlActionID, "HRCodeMap", "Code", "CodeCName", ST1.DisplayType.Full, " AND TabName = 'EmpFlow' AND FldName = 'ActionID' AND NotShowFlag = '0' ", "", "")
            ddlActionID.Items.Insert(0, New ListItem("---請選擇---", ""))
        End If
    End Sub

    Protected Overrides Sub BaseOnPageTransfer(ByVal ti As TransferInfo)
        If Not IsPostBack() Then
            Dim objSC As New SC
            Dim objST As New ST1
            Dim ht As Hashtable = Bsp.Utility.getHashTableFromParam(ti.Args)

            '公司代碼
            If ht.ContainsKey("SelectedCompID") Then
                ViewState.Item("CompID") = ht("SelectedCompID").ToString()
            Else
                Return
            End If

            '員工編號
            If ht.ContainsKey("SelectedEmpID") Then
                ViewState.Item("EmpID") = ht("SelectedEmpID").ToString()
            Else
                Return
            End If

            '員工姓名
            If ht.ContainsKey("SelectedEmpName") Then
                ViewState.Item("EmpName") = ht("SelectedEmpName").ToString()
            Else
                Return
            End If

            '員工身分證字號
            If ht.ContainsKey("SelectedIDNo") Then
                ViewState.Item("IDNo") = ht("SelectedIDNo").ToString()
            Else
                Return
            End If

            txtCompID.Text = ViewState.Item("CompID").ToString() + "-" + objSC.GetCompName(ViewState.Item("CompID").ToString()).Rows(0).Item("CompName").ToString
            hidCompID.Value = ViewState.Item("CompID").ToString()
            txtEmpID.Text = ViewState.Item("EmpID").ToString()
            txtEmpName.Text = ViewState.Item("EmpName").ToString()
            hidIDNo.Value = ViewState.Item("IDNo").ToString()

            Dim OrganID As String = objST.QueryOrganID(ViewState.Item("CompID").ToString(), ViewState.Item("EmpID").ToString()).Rows(0).Item(0)
            hidOrganID.Value = OrganID

            txtOrganID.LoadData(hidOrganID.Value, "0")
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
        Dim beEmpFlow As New beEmpFlow.Row()
        Dim bsEmpFlow As New beEmpFlow.Service()
        Dim objST As New ST1

        beEmpFlow.CompID.Value = hidCompID.Value
        beEmpFlow.EmpID.Value = txtEmpID.Text
        beEmpFlow.ActionID.Value = ddlActionID.SelectedValue
        beEmpFlow.OrganID.Value = txtOrganID.SelectedOrganID
        beEmpFlow.GroupType.Value = objST.QueryData("OrganizationFlow", "AND OrganID = " & Bsp.Utility.Quote(hidOrganID.Value), "GroupType")
        beEmpFlow.GroupID.Value = objST.QueryData("OrganizationFlow", "AND OrganID = " & Bsp.Utility.Quote(hidOrganID.Value), "GroupID")
        beEmpFlow.LastChgComp.Value = UserProfile.ActCompID
        beEmpFlow.LastChgID.Value = UserProfile.ActUserID
        beEmpFlow.LastChgDate.Value = Now

        '檢查資料是否存在
        If bsEmpFlow.IsDataExists(beEmpFlow) Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00010", "")
            Return False
        End If

        '儲存資料
        Try
            Return objST.AddEmpFlowSetting(beEmpFlow)
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, "[CC0201_99]" & Bsp.Utility.getMessage("E_00000") & Bsp.Utility.getInnerException(Me.FunID, ex))
            Return False
        End Try
    End Function

    Private Function funCheckData() As Boolean
        Dim objHR As New HR
        Dim objST As New ST1
        Dim beEmpFlow As New beEmpFlow.Row()
        Dim bsEmpFlow As New beEmpFlow.Service()

        '作業代碼
        If ddlActionID.SelectedValue = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblActionID.Text)
            ddlActionID.Focus()
            Return False
        End If

        '最小單位
        If txtOrganID.SelectedOrganID = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblOrganID.Text)
            txtOrganID.Focus()
            Return False
        End If

        Return True
    End Function

    Private Sub ClearData()
        ddlActionID.SelectedValue = ""
        txtOrganID.LoadData(hidOrganID.Value, "0")
    End Sub

End Class
