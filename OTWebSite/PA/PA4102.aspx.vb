'****************************************************
'功能說明：Web人員資料查詢
'建立人員：BeatriceCheng
'建立日期：2015.08.11
'****************************************************
Imports System.Data
Imports Newtonsoft.Json

Partial Class PA_PA4102
    Inherits PageBase


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then
            Dim objSC As New SC

            ucSelectEmpID.ShowCompRole = False
            ddlCompID.Visible = False

            If UserProfile.SelectCompRoleID = "ALL" Then
                ddlCompID.Visible = True
                Bsp.Utility.FillHRCompany(ddlCompID)
                'ddlCompID.Items.Insert(0, New ListItem("全金控", "0"))
                lblCompRoleID.Visible = False
            Else
                ddlCompID.Visible = False
                lblCompRoleID.Text = UserProfile.SelectCompRoleName
                lblCompRoleID.Visible = True

                ucSelectEmpID.SelectCompID = UserProfile.SelectCompRoleID
            End If

            subLoadDropDownList()
        End If
    End Sub

    Protected Overrides Sub BaseOnPageTransfer(ByVal ti As TransferInfo)

    End Sub

    Public Overrides Sub DoAction(ByVal Param As String)
        Select Case Param
            Case "btnQuery"     '查詢
                ViewState.Item("DoQuery") = "Y"
                DoQuery()
            Case "btnActionX"   '返回
                GoBack()
            Case "btnCancel"    '清除
                DoClear()
        End Select
    End Sub

    Private Sub GoBack()
        Dim ti As TransferInfo = Me.StateTransfer
        Me.TransferFramePage(ti.CallerUrl, Nothing, ti.Args)
    End Sub

    Private Sub subLoadDropDownList()
        PA4.FillDDL(ddlUseCompID, "Company", "RTRIM(CompID)", "CompName + case when InValidFlag = '1' then '(無效)' else '' end", PA4.DisplayType.Full, "", "", "Order By InValidFlag, CompID")
        ddlUseCompID.Items.Insert(0, New ListItem("---請選擇---", ""))

        PA4.FillDDL(ddlUseGroupID, "OrganizationFlow", "RTRIM(GroupID)", "OrganName", PA4.DisplayType.Full, "", "And GroupID = OrganID")
        ddlUseGroupID.Items.Insert(0, New ListItem("---請選擇---", ""))

        PA4.FillDDL(ddlUseOrganID, "Organization", "RTRIM(OrganID)", "OrganName + case when InValidFlag = '1' then '(無效)' else '' end", PA4.DisplayType.Full, "", "And OrganID = DeptID And VirtualFlag = '0'", "Order By InValidFlag, OrganID")
        ddlUseOrganID.Items.Insert(0, New ListItem("---請選擇---", ""))
    End Sub

    Private Sub DoQuery()
        Dim objPA4 As New PA4()

        Dim strCompID As String = ddlCompID.SelectedValue
        If strCompID = "" Then
            strCompID = UserProfile.SelectCompRoleID
        End If

        Try
            pcMain.DataTable = objPA4.VIPQuery(
                "CompID=" & strCompID, _
                "EmpID=" & txtEmpID.Text.ToUpper, _
                "Name=" & txtName.Text, _
                "UseCompID=" & ddlUseCompID.SelectedValue, _
                "UseGroupID=" & ddlUseGroupID.SelectedValue, _
                "UseOrganID=" & ddlUseOrganID.SelectedValue)

            gvMain.Visible = True
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & ".DoQuery", ex)
        End Try
    End Sub

    Private Sub DoClear()
        ViewState.Item("DoQuery") = ""
        ddlCompID.SelectedValue = ""
        txtEmpID.Text = ""
        txtName.Text = ""
        gvMain.Visible = False
        subLoadDropDownList()
    End Sub

    Public Overrides Sub DoModalReturn(ByVal returnValue As String)
        Dim strSql As String = ""
        Dim result As Boolean = True

        If returnValue <> "" Then
            Dim aryData() As String = returnValue.Split(":")
            Select Case aryData(0)
                '員工編號
                Case "ucSelectEmpID"
                    Dim aryValue() As String = Split(aryData(1), "|$|")
                    txtEmpID.Text = aryValue(1)
                    'txtName.Text = aryValue(2)
                    If UserProfile.SelectCompRoleID = "ALL" Then
                        ddlCompID.SelectedValue = aryValue(0)
                    End If

            End Select
        End If
    End Sub

    Protected Sub gvMain_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvMain.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim UseOurColleagues As String() = DataBinder.Eval(e.Row.DataItem, "UseOurColleagues").ToString.Split("|")

            If UseOurColleagues.Length > 0 Then
                Dim strColleague As String = ""
                For Each item In UseOurColleagues
                    Select Case item
                        Case "01"
                            strColleague = strColleague & "、基本資料"
                        Case "02"
                            strColleague = strColleague & "、進階資料"
                        Case "03"
                            strColleague = strColleague & "、學歷資料"
                        Case "04"
                            strColleague = strColleague & "、前職經歷"
                        Case "05"
                            strColleague = strColleague & "、家庭狀況"
                        Case "06"
                            strColleague = strColleague & "、企業團經歷"
                        Case "07"
                            strColleague = strColleague & "、證照"
                        Case "08"
                            strColleague = strColleague & "、訓練紀錄"
                    End Select
                Next
                If strColleague.Length > 0 Then
                    e.Row.Cells(8).Text = strColleague.Substring(1)
                End If
            End If
        End If
    End Sub

    Protected Sub ddlUseCompID_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlUseCompID.SelectedIndexChanged
        If ddlUseCompID.SelectedValue <> "" Then
            PA4.FillDDL(ddlUseGroupID, "OrganizationFlow", "RTRIM(GroupID)", "OrganName", PA4.DisplayType.Full, "", "And GroupID = OrganID And GroupID In (Select Distinct GroupID From Organization Where CompID = " & Bsp.Utility.Quote(ddlUseCompID.SelectedValue) & ")")
            ddlUseGroupID.Items.Insert(0, New ListItem("---請選擇---", ""))

            PA4.FillDDL(ddlUseOrganID, "Organization", "RTRIM(OrganID)", "OrganName + case when InValidFlag = '1' then '(無效)' else '' end", PA4.DisplayType.Full, "", "And OrganID = DeptID And VirtualFlag = '0' And CompID = " & Bsp.Utility.Quote(ddlUseCompID.SelectedValue), "Order By InValidFlag, OrganID")
            ddlUseOrganID.Items.Insert(0, New ListItem("---請選擇---", ""))
        Else
            PA4.FillDDL(ddlUseGroupID, "OrganizationFlow", "RTRIM(GroupID)", "OrganName", PA4.DisplayType.Full, "", "And GroupID = OrganID")
            ddlUseGroupID.Items.Insert(0, New ListItem("---請選擇---", ""))

            PA4.FillDDL(ddlUseOrganID, "Organization", "RTRIM(OrganID)", "OrganName + case when InValidFlag = '1' then '(無效)' else '' end", PA4.DisplayType.Full, "", "And OrganID = DeptID And VirtualFlag = '0'", "Order By InValidFlag, OrganID")
            ddlUseOrganID.Items.Insert(0, New ListItem("---請選擇---", ""))
        End If
    End Sub

    Protected Sub ddlUseGroupID_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlUseGroupID.SelectedIndexChanged
        If ddlUseCompID.SelectedValue <> "" Then
            If ddlUseGroupID.SelectedValue <> "" Then
                PA4.FillDDL(ddlUseOrganID, "Organization", "RTRIM(OrganID)", "OrganName + case when InValidFlag = '1' then '(無效)' else '' end", PA4.DisplayType.Full, "", "And OrganID = DeptID And VirtualFlag = '0' And CompID = " & Bsp.Utility.Quote(ddlUseCompID.SelectedValue) & _
                            " And GroupID In (Select GroupID From OrganizationFlow Where OrganID = GroupID And GroupID In ( Select Distinct GroupID From Organization Where CompID = " & Bsp.Utility.Quote(ddlUseCompID.SelectedValue) & " )) And GroupID = " & Bsp.Utility.Quote(ddlUseGroupID.SelectedValue), "Order By InValidFlag, OrganID")
                ddlUseOrganID.Items.Insert(0, New ListItem("---請選擇---", ""))
            Else
                PA4.FillDDL(ddlUseOrganID, "Organization", "RTRIM(OrganID)", "OrganName + case when InValidFlag = '1' then '(無效)' else '' end", PA4.DisplayType.Full, "", "And OrganID = DeptID And VirtualFlag = '0' And CompID = " & Bsp.Utility.Quote(ddlUseCompID.SelectedValue), "Order By InValidFlag, OrganID")
                ddlUseOrganID.Items.Insert(0, New ListItem("---請選擇---", ""))
            End If
        Else
            PA4.FillDDL(ddlUseOrganID, "Organization", "RTRIM(OrganID)", "OrganName + case when InValidFlag = '1' then '(無效)' else '' end", PA4.DisplayType.Full, "", "And OrganID = DeptID And VirtualFlag = '0'", "Order By InValidFlag, OrganID")
            ddlUseOrganID.Items.Insert(0, New ListItem("---請選擇---", ""))
        End If
    End Sub

    Protected Sub txtEmpID_TextChanged(sender As Object, e As System.EventArgs) Handles txtEmpID.TextChanged

    End Sub
End Class
