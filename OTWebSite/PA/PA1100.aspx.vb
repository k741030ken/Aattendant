'****************************************************
'功能說明：新公司成立設定查詢
'建立人員：MickySung
'建立日期：2015/04/09
'****************************************************
Imports System.Data

Partial Class PA_PA1100
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim objSC As New SC
            '2015/05/15 規格變更
            'If UserProfile.IsSysAdmin = True Then
            '    lblCompRoleID.Visible = False
            '    Bsp.Utility.FillHRCompany(ddlCompID)
            'Else
            '    lblCompRoleID.Text = UserProfile.SelectCompRoleID + "-" + objSC.GetCompName(UserProfile.SelectCompRoleID).Rows(0).Item("CompName").ToString
            '    ddlCompID.Visible = False
            'End If
            '公司代碼下拉選單
            If UserProfile.SelectCompRoleID = "ALL" Then
                lblCompRoleID.Visible = False
                Bsp.Utility.FillHRCompany(ddlCompID)
                Page.SetFocus(ddlCompID)
            Else
                '2015/05/28 公司代碼-名稱改寫法
                'lblCompRoleID.Text = UserProfile.SelectCompRoleID + "-" + objSC.GetCompName(UserProfile.SelectCompRoleID).Rows(0).Item("CompName").ToString
                lblCompRoleID.Text = UserProfile.SelectCompRoleName
                ddlCompID.Visible = False
            End If

        End If
    End Sub

    Public Overrides Sub DoAction(ByVal Param As String)
        ViewState.Item("Action") = ""
        Select Case Param
            Case "btnQuery"     '查詢
                ViewState.Item("DoQuery") = "Y"
                DoQuery()
        End Select
    End Sub

    Protected Overrides Sub BaseOnPageTransfer(ByVal ti As TransferInfo)
        If Not IsPostBack() Then
            Dim ht As Hashtable = Bsp.Utility.getHashTableFromParam(ti.Args)

            For Each strKey As String In ht.Keys
                Dim ctl As Control = Page.FindControl(strKey)

                If TypeOf ctl Is TextBox Then
                    CType(ctl, TextBox).Text = ht(strKey).ToString()
                End If
                If TypeOf ctl Is CheckBox Then
                    CType(ctl, CheckBox).Checked = CBool(ht(strKey).ToString())
                End If
                If TypeOf ctl Is DropDownList Then
                    If ht(strKey).ToString <> "" Then
                        CType(ctl, DropDownList).SelectedValue = ht(strKey).ToString
                    End If

                End If
            Next

            If ht.ContainsKey("DoQuery") Then
                If ht("DoQuery").ToString() = "Y" Then
                    ViewState.Item("DoQuery") = "Y"
                    DoQuery()
                End If
            End If
        End If
    End Sub

    Private Sub DoQuery()
        Dim objPA1100 As New PA1()
        Dim strCompID As String
        Dim table1 As DataTable

        strCompID = ddlCompID.SelectedValue
        If strCompID = "" Then
            strCompID = UserProfile.SelectCompRoleID
        End If

        Try
            IsDoQuery.Value = "Y"

            tableControll.Visible = True '顯示table

            table1 = objPA1100.GetCompanySetting(strCompID, "HRDB")
            setVisible(table1, "HRDB")

            table1 = objPA1100.GetCompanySetting(strCompID, "eHRMSDB")
            setVisible(table1, "eHRMSDB")

        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & ".DoQuery", ex)
        End Try

    End Sub

    Protected Sub setVisible(ByVal table As DataTable, ByVal ConnStr As String)
        If ConnStr = "HRDB" Then
            If table.Rows.Count > 0 Then
                Dim Count As String = table.Rows(0).Item(0).ToString()
                If (CInt(Count) > 0) Then
                    cb_UserGroupNotAdmin1.Visible = True
                    cb_UserGroupNotAdmin2.Visible = False
                Else
                    cb_UserGroupNotAdmin1.Visible = False
                    cb_UserGroupNotAdmin2.Visible = True
                End If
            End If

        ElseIf ConnStr = "eHRMSDB" Then
            If table.Rows.Count > 0 Then
                For Each Cell As DataColumn In table.Columns
                    Dim ColumnName As String = Cell.ColumnName
                    Dim Count As String = table.Rows(0).Item(ColumnName).ToString()
                    If (CInt(Count) > 0) Then
                        CType(Page.FindControl(ColumnName + "1"), Image).Visible = True
                        CType(Page.FindControl(ColumnName + "2"), Image).Visible = False
                    Else
                        CType(Page.FindControl(ColumnName + "1"), Image).Visible = False
                        CType(Page.FindControl(ColumnName + "2"), Image).Visible = True
                    End If
                Next
            End If
        End If
    End Sub

End Class
