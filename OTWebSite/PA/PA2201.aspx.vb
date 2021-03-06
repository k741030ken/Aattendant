'****************************************************
'功能說明：單位班別設定-新增
'建立人員：MickySung
'建立日期：2015.05.20
'****************************************************
Imports System.Data

Partial Class PA_PA2201
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then
            Dim objSC As New SC()
            '2015/05/28 公司代碼-名稱改寫法
            lbltxtCompID.Text = UserProfile.SelectCompRoleName
            'lbltxtCompID.Text = UserProfile.SelectCompRoleID + "-" + objSC.GetCompName(UserProfile.SelectCompRoleID).Rows(0).Item("CompName").ToString

            '班別代碼
            PA2.FillWTID_PA2200(ddlWTID, UserProfile.SelectCompRoleID)
            ddlWTID.Items.Insert(0, New ListItem("---請選擇---", ""))

            '單位代碼1
            PA2.FillOrganID_PA2201(ddlDeptID, UserProfile.SelectCompRoleID, "", "1", PA2.DisplayType.Full)
            ddlDeptID.Items.Insert(0, New ListItem("---請選擇---", ""))

            '單位代碼2
            ddlOrganID.Items.Insert(0, New ListItem("---請選擇---", ""))
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
        Dim beOrgWorkTime As New beOrgWorkTime.Row()
        Dim bsOrgWorkTime As New beOrgWorkTime.Service()
        Dim beOrgWorkTimeRows() As beOrgWorkTime.Row = Nothing

        Dim objPA As New PA2()
        Dim intCount As Integer = 0

        '取得輸入資料 '2016/03/30 增加全選選項
        If ddlOrganID.SelectedValue = "ALL" Then
            For i As Integer = 2 To ddlOrganID.Items.Count - 1
                ReDim Preserve beOrgWorkTimeRows(intCount)
                beOrgWorkTime = New beOrgWorkTime.Row()
                beOrgWorkTime.CompID.Value = UserProfile.SelectCompRoleID
                beOrgWorkTime.WTID.Value = ddlWTID.SelectedValue
                beOrgWorkTime.DeptID.Value = ddlDeptID.SelectedValue
                beOrgWorkTime.OrganID.Value = ddlOrganID.Items(i).Value

                '檢查資料是否存在
                If bsOrgWorkTime.IsDataExists(beOrgWorkTime) Then
                    Bsp.Utility.ShowMessage(Me, "[" & ddlOrganID.Items(i).Text & "]資料已存在")
                    ddlDeptID_SelectedIndexChanged(Nothing, Nothing)
                    Return False
                End If

                beOrgWorkTime.LastChgComp.Value = UserProfile.ActCompID
                beOrgWorkTime.LastChgID.Value = UserProfile.ActUserID
                beOrgWorkTime.LastChgDate.Value = Now

                beOrgWorkTimeRows(intCount) = beOrgWorkTime
                intCount = intCount + 1
            Next
        Else
            ReDim Preserve beOrgWorkTimeRows(intCount)
            beOrgWorkTime.CompID.Value = UserProfile.SelectCompRoleID
            beOrgWorkTime.WTID.Value = ddlWTID.SelectedValue
            beOrgWorkTime.DeptID.Value = ddlDeptID.SelectedValue
            beOrgWorkTime.OrganID.Value = ddlOrganID.SelectedValue
            '檢查資料是否存在
            If bsOrgWorkTime.IsDataExists(beOrgWorkTime) Then
                Bsp.Utility.ShowFormatMessage(Me, "[" & ddlOrganID.SelectedItem.Text & "]資料已存在")
                ddlDeptID_SelectedIndexChanged(Nothing, Nothing)
                Return False
            End If

            beOrgWorkTime.LastChgComp.Value = UserProfile.ActCompID
            beOrgWorkTime.LastChgID.Value = UserProfile.ActUserID
            beOrgWorkTime.LastChgDate.Value = Now

            beOrgWorkTimeRows(intCount) = beOrgWorkTime
        End If

        '儲存資料
        Try
            Return objPA.AddOrgWorkTimeSetting(beOrgWorkTimeRows)
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, "[CC0201_99]" & Bsp.Utility.getMessage("E_00000") & Bsp.Utility.getInnerException(Me.FunID, ex))
            Return False
        End Try
    End Function

    Private Function funCheckData() As Boolean
        Dim objPA As New PA2()
        Dim beOrgWorkTime As New beOrgWorkTime.Row()
        Dim bsOrgWorkTime As New beOrgWorkTime.Service()

        '班別代碼
        If ddlWTID.SelectedValue = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblWTID.Text)
            ddlWTID.Focus()
            ddlDeptID_SelectedIndexChanged(Nothing, Nothing)
            Return False
        End If

        '單位代碼1
        If ddlDeptID.SelectedValue = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblOrgID.Text)
            ddlDeptID.Focus()
            ddlDeptID_SelectedIndexChanged(Nothing, Nothing)
            Return False
        End If

        '單位代碼2
        If ddlOrganID.SelectedValue = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblOrgID.Text)
            ddlOrganID.Focus()
            ddlDeptID_SelectedIndexChanged(Nothing, Nothing)
            Return False
        End If

        Return True
    End Function

    Private Sub ClearData()
        ddlWTID.SelectedValue = ""
        ddlDeptID.SelectedValue = ""
        ddlDeptID_SelectedIndexChanged(Nothing, Nothing)
        ddlOrganID.SelectedValue = ""
    End Sub

    '2016/03/30 增加全選選項 & 已新增單位變為灰字
    Protected Sub ddlDeptID_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlDeptID.SelectedIndexChanged
        '單位代碼2
        'PA2.FillOrganID_PA2201(ddlOrganID, UserProfile.SelectCompRoleID, ddlDeptID.SelectedValue, "2", PA2.DisplayType.Full)

        Dim strOrganID As String = ddlOrganID.SelectedValue

        Dim strSQL As New StringBuilder
        strSQL.AppendLine("SELECT DISTINCT O.OrganID")
        strSQL.AppendLine(", O.OrganID + '-' + O.OrganName AS OrganName")
        If ddlWTID.SelectedValue <> "" Then
            strSQL.AppendLine(", IsNull(W.WTID, '') AS WTID")
        Else
            strSQL.AppendLine(", '' AS WTID")
        End If
        strSQL.AppendLine("FROM Organization O")
        strSQL.AppendLine("LEFT JOIN OrgWorkTime W On O.CompID = W.CompID And O.DeptID = W.DeptID And O.OrganID = W.OrganID")
        If ddlWTID.SelectedValue <> "" Then
            strSQL.AppendLine("And W.WTID = " & Bsp.Utility.Quote(ddlWTID.SelectedValue))
        End If
        strSQL.AppendLine("WHERE O.CompID = " & Bsp.Utility.Quote(UserProfile.SelectCompRoleID) & " AND O.DeptID = " & Bsp.Utility.Quote(ddlDeptID.SelectedValue))
        strSQL.AppendLine("AND O.InValidFlag = '0' and O.VirtualFlag = '0' ")
        strSQL.AppendLine("ORDER BY O.OrganID")

        Using dt As DataTable = Bsp.DB.ExecuteDataSet(CommandType.Text, strSQL.ToString(), "eHRMSDB").Tables(0)
            ddlOrganID.Items.Clear()

            If dt.Rows.Count > 0 Then
                For Each item As DataRow In dt.Rows
                    Dim ListOpt As ListItem = New ListItem()
                    ListOpt.Value = item("OrganID").ToString()
                    ListOpt.Text = item("OrganName").ToString()

                    If item("WTID").ToString().Trim() <> "" Then
                        ListOpt.Attributes.Add("style", "color:gray")
                    End If
                    ddlOrganID.Items.Add(ListOpt)
                Next

                ddlOrganID.Items.Insert(0, New ListItem("ALL-全選", "ALL"))
            End If
        End Using
        ddlOrganID.Items.Insert(0, New ListItem("---請選擇---", ""))

        Bsp.Utility.SetSelectedIndex(ddlOrganID, strOrganID)
    End Sub

End Class
