'****************************************************
'功能說明：使用者權限維護-權限複製
'建立人員：Ann
'建立日期：2014/10/24
'****************************************************
Imports System.Data
Imports System.Data.Common

Partial Class SC_SC0602
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then

            Dim objSC As New SC
            ucSelectEmpID.ShowCompRole = False
            ucSelectEmpIDCopy.ShowCompRole = False
            ucSelectEmpID.ConnType = "SC"
            ucSelectEmpIDCopy.ConnType = "SC"

            '系統別
            Dim strSysID As String
            strSysID = Bsp.Utility.subGetSysID(UserProfile.LoginSysID)
            Dim arySysID() As String = Split(strSysID, "-")
            lblSysName.Text = strSysID

            '來源使用者
            Bsp.Utility.FillCompany(ddlUserComp) '20151013 Beatrice Add
            ddlUserComp.Items.Insert(0, New ListItem("---請選擇---", "")) '20151013 Beatrice Add

            '來源授權公司
            'Bsp.Utility.FillCompany(ddlCompRoleID)
            ddlCompRoleID.Items.Insert(0, New ListItem("---請選擇---", "")) '20151015 Beatrice modify

            '來源群組
            'Bsp.Utility.FillGroup(ddlGroup)
            ddlGroup.Items.Insert(0, New ListItem("---請選擇---", "")) '20151015 Beatrice modify

            '複製至使用者授權公司
            'lblCompIDCopy.Text = objSC.GetCompName(UserProfile.SelectCompRoleID).Rows(0).Item("CompName").ToString()
            ddlCompRoleIDCopy.Items.Insert(0, New ListItem("---請選擇---", "")) '20151014 Beatrice Add

            '複製至使用者
            Bsp.Utility.FillCompany(ddlUserCompCopy) '20151014 Beatrice Add
            ddlUserCompCopy.Items.Insert(0, New ListItem("---請選擇---", "")) '20151014 Beatrice Add

            'If lblCompRoleName.Text <> "" Then
            '    subLoadtxtGroup()
            'End If

        End If
    End Sub

    Public Overrides Sub DoAction(ByVal Param As String)
        Select Case Param
            Case "btnUpdate"
                If funCheckData() Then
                    If SaveData() Then GoBack()
                End If
            Case "btnActionX"
                GoBack()
        End Select
    End Sub

    Private Sub GoBack()
        Dim ti As TransferInfo = Me.StateTransfer
        Me.TransferFramePage(ti.CallerUrl, Nothing, ti.Args)
    End Sub

    Private Function SaveData() As Boolean
        Dim objSC As New SC
        Dim beUserGroup As New beSC_UserGroup.Row()
        Dim bsUserGroup As New beSC_UserGroup.Service()
        Dim strSQL As New StringBuilder()

        '1A 來源授權公司全選 + 來源群組全選(請選擇=全選)
        If ddlCompRoleID.SelectedValue = "" And ddlGroup.SelectedValue = "" Then
            '先刪除「複製至使用者」的所有權限，再複製「來源使用者」的權限
            strSQL = New StringBuilder()
            strSQL.AppendLine(" delete from SC_UserGroup ")
            strSQL.AppendLine(" where CompID = '" & ddlUserCompCopy.SelectedValue & "' and UserID = '" & txtUserIDCopy.Text & "' ")

            strSQL.AppendLine(" insert into SC_UserGroup (CompID, UserID, SysID, CompRoleID, GroupID, CreateDate, LastChgComp, LastChgID, LastChgDate) ")
            strSQL.AppendLine(" select distinct ")
            strSQL.AppendLine(" '" & ddlUserCompCopy.SelectedValue & "' ")     '使用者公司
            strSQL.AppendLine(" ,'" & txtUserIDCopy.Text & "' ")    '使用者員編
            strSQL.AppendLine(" ,'" & UserProfile.LoginSysID & "' ")      '使用者系統別
            'strSQL.AppendLine(" ,'" & ddlCompRoleID.SelectedValue & "' ")    '來源授權公司
            strSQL.AppendLine(" , U.CompRoleID, U.GroupID, getdate() ") '來源授權公司全選
            strSQL.AppendLine(" ,'" & UserProfile.ActCompID & "' ")
            strSQL.AppendLine(" ,'" & UserProfile.UserID & "' ")
            strSQL.AppendLine(" ,'" & Format(Now, "yyyy/MM/dd HH:mm:ss") & "' ")
            strSQL.AppendLine(" from SC_UserGroup U left join SC_Group G on U.CompRoleID = G.CompRoleID and U.SysID = G.SysID and U.GroupID = G.GroupID ")
            strSQL.AppendLine(" where U.UserID = '" & txtUserID.Text & "'")
            strSQL.AppendLine(" and U.CompID = '" & ddlUserComp.SelectedValue & "'") '20151015 Beatrice Add 綁定來源使用者
            'and CompRoleID = '" & ddlCompRoleID.SelectedValue & "' ")    '來源使用者與來源授權公司
            'Bsp.DB.ExecuteScalar(strSQL.ToString())
        End If

        '1B 來源授權公司全選 + 來源群組單選 '(2015/10/12 Beatrice del 不會進入此情況)
        'If ddlCompRoleID.SelectedValue = "" And ddlGroup.SelectedValue <> "" Then
        '    strSQL = New StringBuilder()
        '    strSQL.AppendLine(" delete from SC_UserGroup ")
        '    strSQL.AppendLine(" where CompID = '" & lblCompIDCopyH.Text & "' and UserID = '" & lblUserIDCopy.Text & "' and GroupID  = '" & ddlGroup.SelectedValue & "' ")

        '    strSQL.AppendLine(" insert into SC_UserGroup (CompID, UserID, SysID, CompRoleID, GroupID, CreateDate, LastChgComp, LastChgID, LastChgDate) ")
        '    strSQL.AppendLine(" select distinct ")
        '    strSQL.AppendLine(" '" & lblCompIDCopyH.Text & "' ")     '使用者公司
        '    strSQL.AppendLine(" ,'" & lblUserIDCopy.Text & "' ")    '使用者員編
        '    strSQL.AppendLine(" ,'" & UserProfile.LoginSysID & "' ")      '使用者系統別
        '    strSQL.AppendLine(" , U.CompRoleID, U.GroupID, getdate() ") '來源授權公司全選
        '    strSQL.AppendLine(" ,'" & UserProfile.ActCompID & "' ")
        '    strSQL.AppendLine(" ,'" & UserProfile.UserID & "' ")
        '    strSQL.AppendLine(" ,'" & Format(Now, "yyyy/MM/dd HH:mm:ss") & "' ")
        '    strSQL.AppendLine(" from SC_UserGroup U left join SC_Group G on U.CompRoleID = G.CompRoleID and U.SysID = G.SysID and U.GroupID = G.GroupID ")
        '    strSQL.AppendLine(" where U.GroupID = '" & ddlGroup.SelectedValue & "'")
        'End If

        '1C 來源授權公司單選 + 來源群組全選
        If ddlCompRoleID.SelectedValue <> "" And ddlGroup.SelectedValue = "" Then
            strSQL = New StringBuilder()
            strSQL.AppendLine(" delete from SC_UserGroup ")
            strSQL.AppendLine(" where CompID = '" & ddlUserCompCopy.SelectedValue & "' and UserID = '" & txtUserIDCopy.Text & "' and CompRoleID  = '" & ddlCompRoleID.SelectedValue & "' ")

            strSQL.AppendLine(" insert into SC_UserGroup (CompID, UserID, SysID, CompRoleID, GroupID, CreateDate, LastChgComp, LastChgID, LastChgDate) ")
            strSQL.AppendLine(" select distinct ")
            strSQL.AppendLine(" '" & ddlUserCompCopy.SelectedValue & "' ")     '使用者公司
            strSQL.AppendLine(" ,'" & txtUserIDCopy.Text & "' ")    '使用者員編
            strSQL.AppendLine(" ,'" & UserProfile.LoginSysID & "' ")      '使用者系統別
            strSQL.AppendLine(" , U.CompRoleID, U.GroupID, getdate() ") '來源授權公司全選
            strSQL.AppendLine(" ,'" & UserProfile.ActCompID & "' ")
            strSQL.AppendLine(" ,'" & UserProfile.UserID & "' ")
            strSQL.AppendLine(" ,'" & Format(Now, "yyyy/MM/dd HH:mm:ss") & "' ")
            strSQL.AppendLine(" from SC_UserGroup U left join SC_Group G on U.CompRoleID = G.CompRoleID and U.SysID = G.SysID and U.GroupID = G.GroupID ")
            strSQL.AppendLine(" where U.CompRoleID = '" & ddlCompRoleID.SelectedValue & "'")
            strSQL.AppendLine(" and U.CompID = '" & ddlUserComp.SelectedValue & "'") '20151015 Beatrice Add 綁定來源使用者
            strSQL.AppendLine(" and U.UserID = '" & txtUserID.Text & "'") '20151015 Beatrice Add 綁定來源使用者
        End If

        '1D 來源授權公司單選 + 來源群組單選
        If ddlCompRoleID.SelectedValue <> "" And ddlGroup.SelectedValue <> "" Then
            If ddlCompRoleIDCopy.SelectedValue <> "" Then
                '20151014 Beatrice Add
                strSQL = New StringBuilder()
                strSQL.AppendLine(" select count(*) from SC_Group ")
                strSQL.AppendLine(" where SysID = '" & UserProfile.LoginSysID & "' ")
                strSQL.AppendLine(" and CompRoleID = '" & ddlCompRoleIDCopy.SelectedValue & "' ")
                strSQL.AppendLine(" and GroupID = '" & ddlGroup.SelectedValue & "'")

                If Bsp.DB.ExecuteScalar(strSQL.ToString()) <= 0 Then
                    Bsp.Utility.ShowFormatMessage(Me, "H_00000", "該公司尚無「" + ddlGroup.SelectedItem.Text + "」群組，請先新增群組，再做使用者權限複製")
                    Return False
                Else
                    strSQL = New StringBuilder()
                    strSQL.AppendLine(" delete from SC_UserGroup ")
                    strSQL.AppendLine(" where CompID = '" & ddlUserCompCopy.SelectedValue & "' and UserID = '" & txtUserIDCopy.Text & "' and CompRoleID  = '" & ddlCompRoleIDCopy.SelectedValue & "' and GroupID = '" & ddlGroup.SelectedValue & "' ")

                    strSQL.AppendLine(" insert into SC_UserGroup (CompID, UserID, SysID, CompRoleID, GroupID, CreateDate, LastChgComp, LastChgID, LastChgDate) ")
                    strSQL.AppendLine(" select distinct ")
                    strSQL.AppendLine(" '" & ddlUserCompCopy.SelectedValue & "' ")
                    strSQL.AppendLine(" ,'" & txtUserIDCopy.Text & "' ")
                    strSQL.AppendLine(" ,'" & UserProfile.LoginSysID & "' ")
                    strSQL.AppendLine(" , G.CompRoleID, G.GroupID, getdate() ") '來源授權公司全選
                    strSQL.AppendLine(" ,'" & UserProfile.ActCompID & "' ")
                    strSQL.AppendLine(" ,'" & UserProfile.UserID & "' ")
                    strSQL.AppendLine(" ,'" & Format(Now, "yyyy/MM/dd HH:mm:ss") & "' ")
                    strSQL.AppendLine(" from SC_UserGroup U ")
                    strSQL.AppendLine(" full outer join SC_Group G on U.CompRoleID = G.CompRoleID and U.SysID = G.SysID and U.GroupID = G.GroupID ")
                    strSQL.AppendLine(" where G.CompRoleID = '" & ddlCompRoleIDCopy.SelectedValue & "'")
                    strSQL.AppendLine(" and G.GroupID = '" & ddlGroup.SelectedValue & "'")
                End If
            Else
                strSQL = New StringBuilder()
                strSQL.AppendLine(" delete from SC_UserGroup ")
                strSQL.AppendLine(" where CompID = '" & ddlUserCompCopy.SelectedValue & "' and UserID = '" & txtUserIDCopy.Text & "' and CompRoleID  = '" & ddlCompRoleID.SelectedValue & "' and GroupID = '" & ddlGroup.SelectedValue & "' ")

                strSQL.AppendLine(" insert into SC_UserGroup (CompID, UserID, SysID, CompRoleID, GroupID, CreateDate, LastChgComp, LastChgID, LastChgDate) ")
                strSQL.AppendLine(" select distinct ")
                strSQL.AppendLine(" '" & ddlUserCompCopy.SelectedValue & "' ")
                strSQL.AppendLine(" ,'" & txtUserIDCopy.Text & "' ")
                strSQL.AppendLine(" ,'" & UserProfile.LoginSysID & "' ")
                strSQL.AppendLine(" , CompRoleID, GroupID, getdate() ") '來源授權公司全選
                strSQL.AppendLine(" ,'" & UserProfile.ActCompID & "' ")
                strSQL.AppendLine(" ,'" & UserProfile.UserID & "' ")
                strSQL.AppendLine(" ,'" & Format(Now, "yyyy/MM/dd HH:mm:ss") & "' ")
                'strSQL.AppendLine(" from SC_UserGroup U left join SC_Group G on U.CompRoleID = G.CompRoleID and U.SysID = G.SysID and U.GroupID = G.GroupID ")
                strSQL.AppendLine(" from SC_UserGroup ")
                strSQL.AppendLine(" where CompRoleID = '" & ddlCompRoleID.SelectedValue & "'")
                strSQL.AppendLine(" and GroupID = '" & ddlGroup.SelectedValue & "'")
                strSQL.AppendLine(" and CompID = '" & ddlUserComp.SelectedValue & "'") '20151015 Beatrice Add 綁定來源使用者
                strSQL.AppendLine(" and UserID = '" & txtUserID.Text & "'") '20151015 Beatrice Add 綁定來源使用者
            End If
        End If

        Try
            Using cn As DbConnection = Bsp.DB.getConnection()
                cn.Open()
                Dim tran As DbTransaction = cn.BeginTransaction

                Try
                    Bsp.DB.ExecuteNonQuery(CommandType.Text, strSQL.ToString(), tran)
                    tran.Commit()
                Catch ex As Exception
                    tran.Rollback()
                    Throw
                Finally
                    If tran IsNot Nothing Then tran.Dispose()
                End Try
            End Using

        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, "[CC0201_99]" & Bsp.Utility.getMessage("E_00000") & Bsp.Utility.getInnerException(Me.FunID, ex))
            Return False
        End Try

        Return True
    End Function

    Private Function funCheckData() As Boolean
        '檢查功能代碼
        Dim strValue As String

        '20151013 Beatrice Add
        strValue = ddlUserComp.SelectedValue
        If strValue = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", "來源使用者公司代碼")
            ddlUserComp.Focus()
            Return False
        End If

        '20151013 Beatrice modify
        strValue = txtUserID.Text.Trim()
        If strValue = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", "來源使用者")
            txtUserID.Focus()
            Return False
        End If

        '20151013 Beatrice Add
        Dim objSC As New SC
        Dim WorkStatus As String = objSC.GetUserWorkStatus(ddlUserComp.SelectedValue, txtUserID.Text)
        If WorkStatus = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "H_00000", "來源使用者-人事資料尚未建檔")
            txtUserID.Focus()
            Return False
        ElseIf WorkStatus <> "1" Then
            Bsp.Utility.ShowFormatMessage(Me, "H_00000", "來源使用者-人員非在職狀態")
            txtUserID.Focus()
            Return False
        End If

        '20151016 Beatrice Add
        If ddlCompRoleID.Items.Count <= 1 Then
            Bsp.Utility.ShowFormatMessage(Me, "H_00000", "來源使用者無群組資料，請重新選擇")
            txtUserID.Focus()
            Return False
        End If

        '20151013 Beatrice Add
        strValue = ddlUserCompCopy.SelectedValue
        If strValue = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", "複製至使用者公司代碼")
            ddlUserCompCopy.Focus()
            Return False
        End If

        '20151013 Beatrice modify
        strValue = txtUserIDCopy.Text.Trim()
        If strValue = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", "複製至使用者")
            txtUserIDCopy.Focus()
            Return False
        End If

        '20151013 Beatrice Add
        WorkStatus = objSC.GetUserWorkStatus(ddlUserCompCopy.SelectedValue, txtUserIDCopy.Text)
        If WorkStatus = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "H_00000", "複製至使用者-人事資料尚未建檔")
            txtUserIDCopy.Focus()
            Return False
        ElseIf WorkStatus <> "1" Then
            Bsp.Utility.ShowFormatMessage(Me, "H_00000", "複製至使用者-人員非在職狀態")
            txtUserIDCopy.Focus()
            Return False
        End If

        Return True
    End Function

    Protected Sub ddlCompRoleID_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCompRoleID.SelectedIndexChanged
        'subLoadGroup()
        '20151015 Beatrice modify
        Bsp.Utility.FillDDL(ddlGroup, "HRDB", "SC_Group G", "G.GroupID", " G.GroupName", Bsp.Utility.DisplayType.Full, _
                            "left join SC_UserGroup U on U.CompRoleID = G.CompRoleID and U.SysID = G.SysID and U.GroupID = G.GroupID", _
                            "and U.SysID = '" + UserProfile.LoginSysID + "' and U.CompID = '" + ddlUserComp.SelectedValue + "' and U.UserID = '" + txtUserID.Text + "' and U.CompRoleID = '" + ddlCompRoleID.SelectedValue + "'")
        ddlGroup.Items.Insert(0, New ListItem("---請選擇---", ""))
        subQueryGroup()
    End Sub

    Protected Sub ddlGroup_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlGroup.SelectedIndexChanged
        subQueryGroup()
    End Sub

    '20140905 Ann add 來源群組依據來源授權公司帶入
    Private Sub subLoadGroup()
        Dim strGroup As String = ""
        Dim objSC As New SC
        Try
            Using dt As Data.DataTable = objSC.GetGroup(UserProfile.LoginSysID, ddlCompRoleID.SelectedValue)
                With ddlGroup
                    .DataSource = dt
                    .DataTextField = "GroupName"
                    .DataValueField = "GroupID"
                    .DataBind()

                    .Items.Insert(0, New ListItem("---請選擇---", ""))

                    'If dt.Rows.Count > 0 Then
                    '    strGroup = dt.Rows(0).Item(0).ToString
                    '    Bsp.Utility.SetSelectedIndex(ddlGroup, strGroup)
                    '    'UpdGroup.Update()
                    'End If

                End With
            End Using

        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Bsp.Utility.getInnerException("subLoadGroup：", ex))
            Return
        End Try
    End Sub

    Private Sub subQueryGroup()  '20140903 Ann add
        If ddlGroup.SelectedIndex < 0 Then Return
        Dim objSC As New SC

        Try
            pcMain.DataTable = objSC.GetGroupFun_0500(ddlGroup.SelectedValue, "", ddlCompRoleID.SelectedValue) '20150401 Beatrice modify
            'objSC.GetGroupFunCopy(Bsp.Enums.GroupFunType.Group, ddlGroup.SelectedItem.Value)
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & ".QueryData", ex)
        End Try
    End Sub

    Public Overrides Sub DoModalReturn(ByVal returnValue As String)
        Dim strSql As String = ""

        If returnValue <> "" Then
            Dim aryData() As String = returnValue.Split(":")
            Select Case aryData(0)

                Case "ucSelectEmpID" '來源使用者
                    Dim aryValue() As String = Split(aryData(1), "|$|")
                    'lblCompRoleName.Text = aryValue(0)  '公司名稱
                    'txtUserID.Text = aryValue(1)        '員工編號
                    'lblUserName.Text = aryValue(2)      '員工姓名
                    'lblCompRoleIDH.Text = aryValue(3)   '公司代碼

                    '20151013 Beatrice modify
                    Bsp.Utility.SetSelectedIndex(ddlUserComp, aryValue(3)) '公司代碼
                    txtUserID.Text = aryValue(1) '員工編號
                    lblUserName.Text = aryValue(2) '員工姓名

                    txtUserID.Enabled = True
                    subLoadCompRole(txtUserID.Text, ddlCompRoleID)
                    ddlCompRoleID.Enabled = True
                    ddlGroup.Enabled = True

                Case "ucSelectEmpIDCopy" '複製來源使用者
                    Dim aryValue() As String = Split(aryData(1), "|$|")
                    'lblCompIDCopy.Text = aryValue(0)
                    'lblUserIDCopy.Text = aryValue(1)
                    'lblUserNameCopy.Text = aryValue(2)
                    'lblCompIDCopyH.Text = aryValue(3)

                    '20151013 Beatrice modify
                    Bsp.Utility.SetSelectedIndex(ddlUserCompCopy, aryValue(3)) '公司代碼
                    txtUserIDCopy.Text = aryValue(1) '員工編號
                    lblUserNameCopy.Text = aryValue(2) '員工姓名

                    txtUserIDCopy.Enabled = True
                    subLoadCompRole(txtUserIDCopy.Text, ddlCompRoleIDCopy)
                    ddlCompRoleIDCopy.Enabled = True
            End Select
        End If
    End Sub

    '20151013 Beatrice Add
    Protected Sub ddlUserComp_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlUserComp.SelectedIndexChanged
        If ddlUserComp.SelectedValue = "" Then
            txtUserID.Text = ""
            txtUserID.Enabled = False
            lblUserName.Text = ""
        Else
            txtUserID.Text = ""
            txtUserID.Enabled = True
            lblUserName.Text = ""
        End If
    End Sub

    '20151013 Beatrice Add
    Protected Sub ddlUserCompCopy_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlUserCompCopy.SelectedIndexChanged
        If ddlUserCompCopy.SelectedValue = "" Then
            txtUserIDCopy.Text = ""
            txtUserIDCopy.Enabled = False
            lblUserNameCopy.Text = ""
        Else
            txtUserIDCopy.Text = ""
            txtUserIDCopy.Enabled = True
            lblUserNameCopy.Text = ""
        End If
    End Sub

    '20151013 Beatrice Add 檢核員編
    Protected Sub txtUserID_TextChanged(sender As Object, e As System.EventArgs) Handles txtUserID.TextChanged
        ddlCompRoleID.Items.Clear()
        ddlCompRoleID.Items.Insert(0, New ListItem("---請選擇---", ""))
        ddlGroup.Items.Clear()
        ddlGroup.Items.Insert(0, New ListItem("---請選擇---", ""))

        If ddlUserComp.SelectedValue <> "" And txtUserID.Text <> "" Then
            Dim objSC As New SC
            Dim WorkStatus As String = objSC.GetUserWorkStatus(ddlUserComp.SelectedValue, txtUserID.Text)
            If WorkStatus = "" Then
                lblUserName.Text = ""
                ddlCompRoleID.Enabled = False
                ddlGroup.Enabled = False
                Bsp.Utility.ShowFormatMessage(Me, "H_00000", "人事資料尚未建檔")
            ElseIf WorkStatus <> "1" Then
                lblUserName.Text = ""
                ddlCompRoleID.Enabled = False
                ddlGroup.Enabled = False
                Bsp.Utility.ShowFormatMessage(Me, "H_00000", "人員非在職狀態")
            Else
                lblUserName.Text = objSC.GetSC_UserName(ddlUserComp.SelectedValue, txtUserID.Text)
                subLoadCompRole(txtUserID.Text, ddlCompRoleID)
                ddlCompRoleID.Enabled = True
                ddlGroup.Enabled = True
            End If
        Else
            lblUserName.Text = ""
            ddlCompRoleID.Enabled = False
            ddlGroup.Enabled = False
        End If
    End Sub

    '20151013 Beatrice Add 檢核員編
    Protected Sub txtUserIDCopy_TextChanged(sender As Object, e As System.EventArgs) Handles txtUserIDCopy.TextChanged
        ddlCompRoleIDCopy.Items.Clear()
        ddlCompRoleIDCopy.Items.Insert(0, New ListItem("---請選擇---", ""))

        If ddlUserCompCopy.SelectedValue <> "" And txtUserIDCopy.Text <> "" Then
            Dim objSC As New SC
            Dim WorkStatus As String = objSC.GetUserWorkStatus(ddlUserCompCopy.SelectedValue, txtUserIDCopy.Text)
            If WorkStatus = "" Then
                lblUserNameCopy.Text = ""
                ddlCompRoleIDCopy.Enabled = False
                Bsp.Utility.ShowFormatMessage(Me, "H_00000", "人事資料尚未建檔")
            ElseIf WorkStatus <> "1" Then
                lblUserNameCopy.Text = ""
                ddlCompRoleIDCopy.Enabled = False
                Bsp.Utility.ShowFormatMessage(Me, "H_00000", "人員非在職狀態")
            Else
                lblUserNameCopy.Text = objSC.GetSC_UserName(ddlUserCompCopy.SelectedValue, txtUserIDCopy.Text)
                subLoadCompRole(txtUserIDCopy.Text, ddlCompRoleIDCopy)
                ddlCompRoleIDCopy.Enabled = True
            End If
        Else
            lblUserNameCopy.Text = ""
            ddlCompRoleIDCopy.Enabled = False
        End If
    End Sub

    '20151008 Beatrice Add 載入授權公司
    Private Sub subLoadCompRole(ByVal UserID As String, ByVal ddlCompRole As DropDownList)
        Dim objSC As New SC()

        ddlCompRole.Items.Clear()
        Try
            Using dt As Data.DataTable = objSC.GetCompRoleID(UserID, UserProfile.LoginSysID)
                With ddlCompRole
                    .DataSource = dt
                    .DataTextField = "CompName"
                    .DataValueField = "CompRoleID"
                    .DataBind()
                    .Items.Insert(0, New ListItem("---請選擇---", ""))
                End With
            End Using

        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, "subLoadCompRoleID", ex)
            Throw
            Return
        End Try
    End Sub
End Class
