'****************************************************
'功能說明：員工最小簽核單位維護-新增
'建立人員：Micky Sung
'建立日期：2015.06.11
'****************************************************
Imports System.Data
Imports Newtonsoft.Json

Partial Class ST_ST1202
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then

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
                ViewState.Item("EmpID") = ht("SelectedEmpID").ToString()
                ViewState.Item("EmpName") = ht("SelectedEmpName").ToString()
                ViewState.Item("IDNo") = ht("SelectedIDNo").ToString()
                ViewState.Item("ActionID") = ht("SelectedActionID").ToString()
                ViewState.Item("ActionName") = ht("SelectedActionName").ToString()

                txtCompID.Text = ViewState.Item("CompID").ToString() + "-" + objSC.GetCompName(ViewState.Item("CompID").ToString()).Rows(0).Item("CompName").ToString
                hidCompID.Value = ViewState.Item("CompID").ToString()
                txtEmpID.Text = ViewState.Item("EmpID").ToString()
                txtEmpName.Text = ViewState.Item("EmpName").ToString()
                hidIDNo.Value = ViewState.Item("IDNo").ToString()
                hidActionID.Value = ViewState.Item("ActionID").ToString()
                hidActionName.Value = ViewState.Item("ActionName").ToString()

                Dim OrganID As String = objST.QueryOrganID(ViewState.Item("CompID").ToString(), ViewState.Item("EmpID").ToString()).Rows(0).Item(0)
                hidOrganID.Value = OrganID
                txtOrganID.LoadData(hidOrganID.Value, "0")
                subGetData(ht("SelectedCompID").ToString(), ht("SelectedEmpID").ToString(), ht("SelectedActionID").ToString())
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
        Dim beEmpFlow As New beEmpFlow.Row()
        Dim bsEmpFlow As New beEmpFlow.Service()
        Dim objST As New ST1

        beEmpFlow.CompID.Value = hidCompID.Value
        beEmpFlow.EmpID.Value = txtEmpID.Text
        beEmpFlow.ActionID.Value = hidActionID.Value
        beEmpFlow.OrganID.Value = txtOrganID.SelectedOrganID
        beEmpFlow.GroupType.Value = objST.QueryData("OrganizationFlow", "AND OrganID = " & Bsp.Utility.Quote(hidOrganID.Value), "GroupType")
        beEmpFlow.GroupID.Value = objST.QueryData("OrganizationFlow", "AND OrganID = " & Bsp.Utility.Quote(hidOrganID.Value), "GroupID")
        beEmpFlow.LastChgComp.Value = UserProfile.ActCompID
        beEmpFlow.LastChgID.Value = UserProfile.ActUserID
        beEmpFlow.LastChgDate.Value = Now

        '儲存資料
        Try
            Return objST.UpdateEmpFlowSetting(beEmpFlow)
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, "[CC0201_99]" & Bsp.Utility.getMessage("E_00000") & Bsp.Utility.getInnerException(Me.FunID, ex))
            Return False
        End Try
    End Function

    Private Sub subGetData(ByVal CompID As String, ByVal EmpID As String, ByVal ActionID As String)
        Dim objST As New ST1
        Dim objSC As New SC
        Dim objHR As New HR
        Dim beEmpFlow As New beEmpFlow.Row()
        Dim bsEmpFlow As New beEmpFlow.Service()

        beEmpFlow.CompID.Value = CompID
        beEmpFlow.EmpID.Value = EmpID
        beEmpFlow.ActionID.Value = ActionID

        Try
            Using dt As DataTable = bsEmpFlow.QueryByKey(beEmpFlow).Tables(0)
                If dt.Rows.Count <= 0 Then Exit Sub
                beEmpFlow = New beEmpFlow.Row(dt.Rows(0))

                ddlActionID.Text = beEmpFlow.ActionID.Value + IIf(hidActionName.Value <> "", "-" + hidActionName.Value, "")
                txtOrganID.setOrganID(beEmpFlow.OrganID.Value, "0")

                '最後異動公司
                Dim CompName As String = objSC.GetSC_CompName(beEmpFlow.LastChgComp.Value)
                lblLastChgComp.Text = beEmpFlow.LastChgComp.Value + IIf(CompName <> "", "-" + CompName, "")

                '最後異動人員
                Dim UserName As String = objSC.GetSC_UserName(beEmpFlow.LastChgComp.Value, beEmpFlow.LastChgID.Value)
                lblLastChgID.Text = beEmpFlow.LastChgID.Value + IIf(UserName <> "", "-" + UserName, "")

                '最後異動日期
                Dim boolDate As Boolean = Format(beEmpFlow.LastChgDate.Value, "yyyy/MM/dd") = "1900/01/01"
                lblLastChgDate.Text = IIf(boolDate, "", beEmpFlow.LastChgDate.Value.ToString("yyyy/MM/dd HH:mm:ss"))

            End Using
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & "subGetData", ex)
        End Try

    End Sub

    Private Function funCheckData() As Boolean
        Dim objHR As New HR
        Dim objST As New ST1
        Dim beEmpFlow As New beEmpFlow.Row()
        Dim bsEmpFlow As New beEmpFlow.Service()

        '最小單位
        If txtOrganID.SelectedOrganID = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblOrganID.Text)
            txtOrganID.Focus()
            Return False
        End If

        Return True
    End Function

    Private Sub ClearData()
        subGetData(hidCompID.Value, txtEmpID.Text, hidActionID.Value)
    End Sub

End Class
