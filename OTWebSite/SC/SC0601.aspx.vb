'****************************************************
'功能說明：使用者權限維護-新增
'建立人員：Ann
'建立日期：2014/10/24
'****************************************************
Imports System.Data
Imports System.Data.Common

Partial Class SC_SC0601
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then

            ucSelectEmpID.ShowCompRole = False
            ucSelectEmpID.ConnType = "SC"

            Bsp.Utility.FillGroup(ddlGroup)
            Dim objSC As New SC
            Dim strSysID As String
            strSysID = Bsp.Utility.subGetSysID(UserProfile.LoginSysID)
            Dim arySysID() As String = Split(strSysID, "-")
            lblSysName.Text = strSysID
            'ddlCompRoleNameU.Visible = False
            ddlGroup.Items.Insert(0, "---請選擇---")
            ddlGroup.Items.Insert(1, New ListItem("全選", "0"))
            
            '使用者公司
            Bsp.Utility.FillCompany(ddlUserComp) '20151008 Beatrice Add
            ddlUserComp.Items.Insert(0, "---請選擇---")
            ddlUserComp.SelectedIndex = 0

            If UserProfile.SelectCompRoleID = "ALL" Then
                ddlCompRoleName.Visible = True
                Bsp.Utility.FillCompany(ddlCompRoleName)
                ddlCompRoleName.SelectedIndex = 0
                lblCompRoleName.Visible = False
            Else
                '系統管理者
                If UserProfile.IsSysAdmin = True Then
                    ''系統管理者選擇全金控
                    'If UserProfile.SelectCompRoleID = "ALL" Then
                    '    ddlCompRoleName.Visible = True
                    '    Bsp.Utility.FillCompany(ddlCompRoleName)
                    '    lblCompRoleName.Visible = False
                    'Else
                    '    ddlCompRoleName.Visible = False
                    '    lblCompRoleName.Text = objSC.GetCompName(UserProfile.SelectCompRoleID).Rows(0).Item("CompName").ToString()
                    '    lblCompRoleName.Visible = True
                    'End If
                    ddlCompRoleName.Visible = True
                    Bsp.Utility.FillCompany(ddlCompRoleName)
                    ddlCompRoleName.Items.Insert(0, "---請選擇---")
                    ddlCompRoleName.Items.Insert(1, New ListItem("全金控", "0"))
                    lblCompRoleName.Visible = False
                Else
                    '非系統管理者
                    ddlCompRoleName.Visible = False
                    lblCompRoleName.Text = objSC.GetCompName(UserProfile.SelectCompRoleID).Rows(0).Item("CompName").ToString()
                    lblCompRoleName.Visible = True
                End If
            End If
        End If
    End Sub

    Protected Overrides Sub BaseOnPageTransfer(ByVal ti As TransferInfo)
        Dim ht As Hashtable = Bsp.Utility.getHashTableFromParam(ti.Args)
        Dim objSC As New SC()

        For Each strKey As String In ht.Keys
            Select Case strKey
                Case "SelectedGroupID"
                    ViewState.Item("GroupID") = ht(strKey).ToString()
                    ViewState.Item("CompRoleID") = ht(strKey).ToString()
                    ViewState.Item("SysID") = ht(strKey).ToString()
                    ViewState.Item("FunID") = ht(strKey).ToString()
            End Select
        Next
    End Sub

    Public Overrides Sub DoAction(ByVal Param As String)
        Select Case Param
            Case "btnAdd"
                If funCheckData() Then
                    If SaveData() Then
                    End If
                End If
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

        If funCheckData() Then
            Using cn As DbConnection = Bsp.DB.getConnection()
                cn.Open()
                Dim tran As DbTransaction = cn.BeginTransaction
                Dim inTrans As Boolean = True

                Try
                    If ddlCompRoleName.Visible = True Then
                        ''系統管理者
                        ''1A 授權公司全選 + 群組全選(請選擇=全選)
                        'If ddlCompRoleName.SelectedValue = "---請選擇---" And ddlGroup.SelectedItem.ToString = "---請選擇---" Then

                        '    strSQL.AppendLine(" delete from SC_UserGroup ")
                        '    strSQL.AppendLine(" where CompID = '" & UserProfile.CompID & "' and UserID = '" & lblUserID.Text & "' ")

                        '    strSQL.AppendLine(" insert into SC_UserGroup (CompID, UserID, SysID, CompRoleID, GroupID, CreateDate, LastChgComp, LastChgID, LastChgDate) ")
                        '    strSQL.AppendLine(" select distinct ")
                        '    strSQL.AppendLine(" '" & lblCompRoleCompIDU.Text & "' ")
                        '    strSQL.AppendLine(" ,'" & lblUserID.Text & "' ")
                        '    strSQL.AppendLine(" ,'" & UserProfile.LoginSysID & "' ")
                        '    strSQL.AppendLine(" , CompRoleID, GroupID, getdate() ")
                        '    strSQL.AppendLine(" ,'" & UserProfile.ActCompID & "' ")
                        '    strSQL.AppendLine(" ,'" & UserProfile.UserID & "' ")
                        '    strSQL.AppendLine(" ,'" & Format(Now, "yyyy/MM/dd HH:mm:ss") & "' ")
                        '    strSQL.AppendLine(" from SC_Group ")
                        '    'Bsp.DB.ExecuteScalar(strSQL.ToString())
                        '    Try
                        '        Bsp.DB.ExecuteNonQuery(CommandType.Text, strSQL.ToString(), tran)
                        '        tran.Commit()
                        '        inTrans = False
                        '    Catch ex As Exception
                        '        If inTrans Then tran.Rollback()
                        '        Throw
                        '    Finally
                        '        tran.Dispose()
                        '        If cn.State = ConnectionState.Open Then cn.Close()
                        '    End Try
                        'End If

                        ''1B 授權公司全選 + 群組單選
                        'If ddlCompRoleName.SelectedValue = "---請選擇---" And ddlGroup.SelectedItem.ToString <> "---請選擇---" Then
                        '    strSQL.AppendLine(" delete from SC_UserGroup ")
                        '    strSQL.AppendLine(" where CompID = '" & UserProfile.CompID & "' and UserID = '" & lblUserID.Text & "' and GroupID  = '" & ddlGroup.SelectedItem.Value & "' ")

                        '    strSQL.AppendLine(" insert into SC_UserGroup (CompID, UserID, SysID, CompRoleID, GroupID, CreateDate, LastChgComp, LastChgID, LastChgDate) ")
                        '    strSQL.AppendLine(" select distinct ")
                        '    strSQL.AppendLine(" '" & lblCompRoleCompIDU.Text & "' ")
                        '    strSQL.AppendLine(" ,'" & lblUserID.Text & "' ")
                        '    strSQL.AppendLine(" ,'" & UserProfile.LoginSysID & "' ")
                        '    strSQL.AppendLine(" , CompRoleID, GroupID, getdate() ")
                        '    strSQL.AppendLine(" ,'" & UserProfile.ActCompID & "' ")
                        '    strSQL.AppendLine(" ,'" & UserProfile.UserID & "' ")
                        '    strSQL.AppendLine(" ,'" & Format(Now, "yyyy/MM/dd HH:mm:ss") & "' ")
                        '    strSQL.AppendLine(" from SC_Group ")
                        '    strSQL.AppendLine(" where GroupID = '" & ddlGroup.SelectedItem.Value & "'")
                        '    'Bsp.DB.ExecuteScalar(strSQL.ToString())
                        '    Try
                        '        Bsp.DB.ExecuteNonQuery(CommandType.Text, strSQL.ToString(), tran)
                        '        tran.Commit()
                        '        inTrans = False
                        '    Catch ex As Exception
                        '        If inTrans Then tran.Rollback()
                        '        Throw
                        '    Finally
                        '        tran.Dispose()
                        '        If cn.State = ConnectionState.Open Then cn.Close()
                        '    End Try
                        'End If

                        '1C 授權公司單選 + 群組全選
                        If ddlCompRoleName.SelectedValue <> "---請選擇---" And ddlCompRoleName.SelectedValue <> "0" And ddlGroup.SelectedItem.Value = "0" Then
                            If ddlCompRoleName.SelectedValue <> "0" Then
                                strSQL.AppendLine(" delete from SC_UserGroup ")
                                strSQL.AppendLine(" where CompID = '" & UserProfile.CompID & "' and UserID = '" & txtUserID.Text & "' and CompRoleID  = '" & ddlCompRoleName.SelectedValue & "' ")

                                strSQL.AppendLine(" insert into SC_UserGroup (CompID, UserID, SysID, CompRoleID, GroupID, CreateDate, LastChgComp, LastChgID, LastChgDate) ")
                                strSQL.AppendLine(" select distinct ")
                                strSQL.AppendLine(" '" & ddlUserComp.SelectedValue & "' ")
                                strSQL.AppendLine(" ,'" & txtUserID.Text & "' ")
                                strSQL.AppendLine(" ,'" & UserProfile.LoginSysID & "' ")
                                strSQL.AppendLine(" , CompRoleID, GroupID, getdate() ")
                                strSQL.AppendLine(" ,'" & UserProfile.ActCompID & "' ")
                                strSQL.AppendLine(" ,'" & UserProfile.UserID & "' ")
                                strSQL.AppendLine(" ,'" & Format(Now, "yyyy/MM/dd HH:mm:ss") & "' ")
                                strSQL.AppendLine(" from SC_Group ")
                                strSQL.AppendLine(" where CompRoleID = '" & ddlCompRoleName.SelectedItem.Value & "'")
                            Else
                                strSQL.AppendLine(" delete from SC_UserGroup ")
                                strSQL.AppendLine(" where CompID = '" & UserProfile.CompID & "' and UserID = '" & txtUserID.Text & "' and CompRoleID  = '" & ddlCompRoleName.SelectedValue & "' ")

                                strSQL.AppendLine(" insert into SC_UserGroup (CompID, UserID, SysID, CompRoleID, GroupID, CreateDate, LastChgComp, LastChgID, LastChgDate) ")
                                strSQL.AppendLine(" select distinct ")
                                strSQL.AppendLine(" '" & ddlUserComp.SelectedValue & "' ")
                                strSQL.AppendLine(" ,'" & txtUserID.Text & "' ")
                                strSQL.AppendLine(" ,'" & UserProfile.LoginSysID & "' ")
                                strSQL.AppendLine(" , CompRoleID, GroupID, getdate() ")
                                strSQL.AppendLine(" ,'" & UserProfile.ActCompID & "' ")
                                strSQL.AppendLine(" ,'" & UserProfile.UserID & "' ")
                                strSQL.AppendLine(" ,'" & Format(Now, "yyyy/MM/dd HH:mm:ss") & "' ")
                                strSQL.AppendLine(" from SC_Group ")
                                strSQL.AppendLine(" where CompRoleID = '" & ddlCompRoleName.SelectedItem.Value & "'")
                                strSQL.AppendLine(" union ")
                                strSQL.AppendLine(" select distinct ")
                                strSQL.AppendLine(" '" & ddlUserComp.SelectedValue & "' ")
                                strSQL.AppendLine(" ,'" & txtUserID.Text & "' ")
                                strSQL.AppendLine(" ,'" & UserProfile.LoginSysID & "' ")
                                strSQL.AppendLine(" , 'ALL', GroupID, getdate() ")
                                strSQL.AppendLine(" ,'" & UserProfile.ActCompID & "' ")
                                strSQL.AppendLine(" ,'" & UserProfile.UserID & "' ")
                                strSQL.AppendLine(" ,'" & Format(Now, "yyyy/MM/dd HH:mm:ss") & "' ")
                                strSQL.AppendLine(" from SC_Group ")
                            End If
                            'Bsp.DB.ExecuteScalar(strSQL.ToString())
                            Try
                                Bsp.DB.ExecuteNonQuery(CommandType.Text, strSQL.ToString(), tran)
                                tran.Commit()
                                inTrans = False
                            Catch ex As Exception
                                If inTrans Then tran.Rollback()
                                Throw
                            Finally
                                tran.Dispose()
                                If cn.State = ConnectionState.Open Then cn.Close()
                            End Try
                        End If

                        '1D 授權公司單選 + 群組單選
                        If ddlCompRoleName.SelectedValue <> "---請選擇---" And ddlCompRoleName.SelectedValue <> "0" And ddlGroup.SelectedItem.Value <> "0" Then
                            If ddlCompRoleName.SelectedValue <> "0" Then
                                strSQL.AppendLine(" delete from SC_UserGroup ")
                                strSQL.AppendLine(" where CompID = '" & UserProfile.CompID & "' and UserID = '" & txtUserID.Text & "' and CompRoleID  = '" & ddlCompRoleName.SelectedValue & "' and GroupID = '" & ddlGroup.SelectedItem.Value & "' ")

                                strSQL.AppendLine(" insert into SC_UserGroup (CompID, UserID, SysID, CompRoleID, GroupID, CreateDate, LastChgComp, LastChgID, LastChgDate) ")
                                strSQL.AppendLine(" select distinct ")
                                strSQL.AppendLine(" '" & ddlUserComp.SelectedValue & "' ")
                                strSQL.AppendLine(" ,'" & txtUserID.Text & "' ")
                                strSQL.AppendLine(" ,'" & UserProfile.LoginSysID & "' ")
                                strSQL.AppendLine(" , CompRoleID, GroupID, getdate() ")
                                strSQL.AppendLine(" ,'" & UserProfile.ActCompID & "' ")
                                strSQL.AppendLine(" ,'" & UserProfile.UserID & "' ")
                                strSQL.AppendLine(" ,'" & Format(Now, "yyyy/MM/dd HH:mm:ss") & "' ")
                                strSQL.AppendLine(" from SC_Group ")
                                strSQL.AppendLine(" where CompRoleID = '" & ddlCompRoleName.SelectedItem.Value & "'")
                                strSQL.AppendLine(" and GroupID = '" & ddlGroup.SelectedItem.Value & "'")
                                'Bsp.DB.ExecuteScalar(strSQL.ToString())
                            Else
                                strSQL.AppendLine(" delete from SC_UserGroup ")
                                strSQL.AppendLine(" where CompID = '" & UserProfile.CompID & "' and UserID = '" & txtUserID.Text & "' and CompRoleID  = '" & ddlCompRoleName.SelectedValue & "' and GroupID = '" & ddlGroup.SelectedItem.Value & "' ")

                                strSQL.AppendLine(" insert into SC_UserGroup (CompID, UserID, SysID, CompRoleID, GroupID, CreateDate, LastChgComp, LastChgID, LastChgDate) ")
                                strSQL.AppendLine(" select distinct ")
                                strSQL.AppendLine(" '" & ddlUserComp.SelectedValue & "' ")
                                strSQL.AppendLine(" ,'" & txtUserID.Text & "' ")
                                strSQL.AppendLine(" ,'" & UserProfile.LoginSysID & "' ")
                                strSQL.AppendLine(" , CompRoleID, GroupID, getdate() ")
                                strSQL.AppendLine(" ,'" & UserProfile.ActCompID & "' ")
                                strSQL.AppendLine(" ,'" & UserProfile.UserID & "' ")
                                strSQL.AppendLine(" ,'" & Format(Now, "yyyy/MM/dd HH:mm:ss") & "' ")
                                strSQL.AppendLine(" from SC_Group ")
                                strSQL.AppendLine(" where CompRoleID = '" & ddlCompRoleName.SelectedItem.Value & "'")
                                strSQL.AppendLine(" and GroupID = '" & ddlGroup.SelectedItem.Value & "'")
                                strSQL.AppendLine(" union ")
                                strSQL.AppendLine(" select distinct ")
                                strSQL.AppendLine(" '" & ddlUserComp.SelectedValue & "' ")
                                strSQL.AppendLine(" ,'" & txtUserID.Text & "' ")
                                strSQL.AppendLine(" ,'" & UserProfile.LoginSysID & "' ")
                                strSQL.AppendLine(" , 'ALL', GroupID, getdate() ")
                                strSQL.AppendLine(" ,'" & UserProfile.ActCompID & "' ")
                                strSQL.AppendLine(" ,'" & UserProfile.UserID & "' ")
                                strSQL.AppendLine(" ,'" & Format(Now, "yyyy/MM/dd HH:mm:ss") & "' ")
                                strSQL.AppendLine(" from SC_Group ")
                                'strSQL.AppendLine(" where CompRoleID = '" & ddlCompRoleName.SelectedItem.Value & "'")
                                'strSQL.AppendLine(" and GroupID = '" & ddlGroup.SelectedItem.Value & "'")
                                'Bsp.DB.ExecuteScalar(strSQL.ToString())
                            End If
                            Try
                                Bsp.DB.ExecuteNonQuery(CommandType.Text, strSQL.ToString(), tran)
                                tran.Commit()
                                inTrans = False
                            Catch ex As Exception
                                If inTrans Then tran.Rollback()
                                Throw
                            Finally
                                tran.Dispose()
                                If cn.State = ConnectionState.Open Then cn.Close()
                            End Try
                        End If
                    Else
                        '非系統管理者
                        '群組全選
                        If ddlGroup.SelectedItem.Value = "0" Then
                            strSQL.AppendLine(" delete from SC_UserGroup ")
                            strSQL.AppendLine(" where CompID = '" & UserProfile.CompID & "' and UserID = '" & txtUserID.Text & "' and CompRoleID  = '" & UserProfile.SelectCompRoleID & "' ")

                            strSQL.AppendLine(" insert into SC_UserGroup (CompID, UserID, SysID, CompRoleID, GroupID, CreateDate, LastChgComp, LastChgID, LastChgDate) ")
                            strSQL.AppendLine(" select distinct ")
                            strSQL.AppendLine(" '" & ddlUserComp.SelectedValue & "' ")
                            strSQL.AppendLine(" ,'" & txtUserID.Text & "' ")
                            strSQL.AppendLine(" ,'" & UserProfile.LoginSysID & "' ")
                            strSQL.AppendLine(" , CompRoleID, GroupID, getdate() ")
                            strSQL.AppendLine(" ,'" & UserProfile.ActCompID & "' ")
                            strSQL.AppendLine(" ,'" & UserProfile.UserID & "' ")
                            strSQL.AppendLine(" ,'" & Format(Now, "yyyy/MM/dd HH:mm:ss") & "' ")
                            strSQL.AppendLine(" from SC_Group ")
                            strSQL.AppendLine(" where CompRoleID = '" & UserProfile.SelectCompRoleID & "'")
                            'Bsp.DB.ExecuteScalar(strSQL.ToString())
                            Try
                                Bsp.DB.ExecuteNonQuery(CommandType.Text, strSQL.ToString(), tran)
                                tran.Commit()
                                inTrans = False
                            Catch ex As Exception
                                If inTrans Then tran.Rollback()
                                Throw
                            Finally
                                tran.Dispose()
                                If cn.State = ConnectionState.Open Then cn.Close()
                            End Try
                        End If

                        '1D 授權公司單選 + 群組單選
                        If ddlGroup.SelectedItem.Value <> "0" Then
                            strSQL.AppendLine(" delete from SC_UserGroup ")
                            strSQL.AppendLine(" where CompID = '" & UserProfile.CompID & "' and UserID = '" & txtUserID.Text & "' and CompRoleID  = '" & UserProfile.SelectCompRoleID & "' and GroupID = '" & ddlGroup.SelectedItem.Value & "' ")

                            strSQL.AppendLine(" insert into SC_UserGroup (CompID, UserID, SysID, CompRoleID, GroupID, CreateDate, LastChgComp, LastChgID, LastChgDate) ")
                            strSQL.AppendLine(" select distinct ")
                            strSQL.AppendLine(" '" & ddlUserComp.SelectedValue & "' ")
                            strSQL.AppendLine(" ,'" & txtUserID.Text & "' ")
                            strSQL.AppendLine(" ,'" & UserProfile.LoginSysID & "' ")
                            strSQL.AppendLine(" , CompRoleID, GroupID, getdate() ")
                            strSQL.AppendLine(" ,'" & UserProfile.ActCompID & "' ")
                            strSQL.AppendLine(" ,'" & UserProfile.UserID & "' ")
                            strSQL.AppendLine(" ,'" & Format(Now, "yyyy/MM/dd HH:mm:ss") & "' ")
                            strSQL.AppendLine(" from SC_Group ")
                            strSQL.AppendLine(" where CompRoleID = '" & UserProfile.SelectCompRoleID & "'")
                            strSQL.AppendLine(" and GroupID = '" & ddlGroup.SelectedItem.Value & "'")
                            'Bsp.DB.ExecuteScalar(strSQL.ToString())
                            Try
                                Bsp.DB.ExecuteNonQuery(CommandType.Text, strSQL.ToString(), tran)
                                tran.Commit()
                                inTrans = False
                            Catch ex As Exception
                                If inTrans Then tran.Rollback()
                                Throw
                            Finally
                                tran.Dispose()
                                If cn.State = ConnectionState.Open Then cn.Close()
                            End Try
                        End If
                    End If
                    '檢查有無重複資料，有，顯示提示訊息
                    'Try
                    '    Using dt As DataTable = GetSC_UserGroup( _
                    '        "CompID=" & UserProfile.CompID, _
                    '        "UserID=" & lblUserID.Text, _
                    '        "CompRoleID=" & ddlCompRoleName.SelectedValue, _
                    '        "GroupID=" & ddlGroup.SelectedItem.Value _
                    '        )

                    '        If dt.Rows.Count > 0 Then
                    '            Bsp.Utility.ShowFormatMessage(Me, "W_00010", "")
                    '            Return False
                    '        Else
                    '            strSQL.AppendLine(" insert into SC_UserGroup (CompID, UserID, SysID, CompRoleID, GroupID, CreateDate, LastChgComp, LastChgID, LastChgDate) ")
                    '            strSQL.AppendLine(" select ")
                    '            strSQL.AppendLine(" '" & UserProfile.CompID & "' ")
                    '            strSQL.AppendLine(" ,'" & lblUserID.Text & "' ")
                    '            strSQL.AppendLine(" ,'" & lblSysNameD.Text & "' ")
                    '            strSQL.AppendLine(" , CompRoleID, GroupID, getdate() ")
                    '            strSQL.AppendLine(" ,'" & UserProfile.ActCompID & "' ")
                    '            strSQL.AppendLine(" ,'" & UserProfile.UserID & "' ")
                    '            strSQL.AppendLine(" ,'" & Format(Now, "yyyy/MM/dd HH:mm:ss") & "' ")
                    '            strSQL.AppendLine(" from SC_Group ")
                    '            strSQL.AppendLine(" where CompRoleID = '" & ddlCompRoleName.SelectedItem.Value & "' and GroupID = '" & ddlGroup.SelectedItem.Value & "'")

                    '            Try
                    '                Bsp.DB.ExecuteNonQuery(CommandType.Text, strSQL.ToString(), tran)
                    '                tran.Commit()
                    '                inTrans = False
                    '            Catch ex As Exception
                    '                If inTrans Then tran.Rollback()
                    '                Throw
                    '            Finally
                    '                tran.Dispose()
                    '                If cn.State = ConnectionState.Open Then cn.Close()
                    '            End Try
                    '        End If
                    '    End Using
                    'Catch ex As Exception
                    '    If inTrans Then tran.Rollback()
                    '    Return False
                    'End Try

                Catch ex As Exception
                    If inTrans Then tran.Rollback()
                    Return False
                End Try
            End Using
        End If
        Return True
    End Function

    Private Function funCheckData() As Boolean
        '檢查功能代碼
        Dim strValue As String

        '公司代碼 '20151008 Beatrice Add
        strValue = ddlUserComp.SelectedValue
        If strValue = "" Or strValue = "---請選擇---" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", "使用者公司代碼")
            ddlUserComp.Focus()
            Return False
        End If

        '20151008 Beatrice Modify
        strValue = txtUserID.Text.Trim()
        If strValue = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", "使用者")
            txtUserID.Focus()
            Return False
        End If

        If ddlCompRoleName.Visible = True Then
            strValue = ddlCompRoleName.SelectedValue
            'If strValue = "" And strValue <> "---請選擇---" Then
            If strValue = "" Or strValue = "---請選擇---" Then
                Bsp.Utility.ShowFormatMessage(Me, "W_00030", "授權公司")
                ddlCompRoleName.Focus()
                Return False
            End If
        End If

        strValue = ddlGroup.SelectedValue
        'If strValue = "" And strValue <> "---請選擇---" Then
        If strValue = "" Or strValue = "---請選擇---" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", "群組")
            ddlGroup.Focus()
            Return False
        End If

        '20151008 Beatrice Add
        Dim objSC As New SC
        Dim WorkStatus As String = objSC.GetUserWorkStatus(ddlUserComp.SelectedValue, txtUserID.Text)
        If WorkStatus = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "H_00000", "人事資料尚未建檔")
            txtUserID.Focus()
            Return False
        ElseIf WorkStatus <> "1" Then
            Bsp.Utility.ShowFormatMessage(Me, "H_00000", "人員非在職狀態")
            txtUserID.Focus()
            Return False
        End If

        Return True
    End Function
    Protected Sub ddlCompRoleName_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCompRoleName.SelectedIndexChanged
        subLoadCopyGroup()
        subQueryGroup()
    End Sub
    Protected Sub ddlGroup_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlGroup.SelectedIndexChanged
        subQueryGroup()
    End Sub
    '20140905 Ann add 複製至群組依據來源授權公司帶入
    Private Sub subLoadCopyGroup()
        Dim strCopyGroup As String = ""
        Dim objSC As New SC
        Try
            'Using dt As Data.DataTable = objSC.GetGroup(UserProfile.LoginSysID, UserProfile.SelectCompRoleID)
            Using dt As Data.DataTable = objSC.GetGroup(UserProfile.LoginSysID, ddlCompRoleName.SelectedItem.Value)
                With ddlGroup
                    .DataSource = dt
                    .DataTextField = "GroupName"
                    .DataValueField = "GroupID"
                    .DataBind()

                    .Items.Insert(0, New ListItem("---請選擇---", ""))
                    ddlGroup.Items.Insert(1, New ListItem("全選", "0"))

                    'If dt.Rows.Count > 0 Then
                    'strCopyGroup = dt.Rows(0).Item(0).ToString
                    'Bsp.Utility.SetSelectedIndex(ddlGroup, strCopyGroup)
                    'End If

                End With
            End Using

        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Bsp.Utility.getInnerException("subLoadCopyGroup：", ex))
            Return
        End Try
    End Sub

    Private Sub subQueryGroup()  '20140903 Ann add
        If ddlGroup.SelectedIndex < 0 Then Return
        Dim objSC As New SC

        Try
            '20150527 Beatrice Add
            Dim strCompRoleID As String
            If ddlCompRoleName.Visible = True Then
                strCompRoleID = ddlCompRoleName.SelectedValue
            Else
                strCompRoleID = UserProfile.SelectCompRoleID
            End If

            'pcMain.DataTable = objSC.GetUserGroupFunadd(ddlGroup.SelectedItem.Value, ddlCompRoleName.SelectedItem.Value)
            pcMain.DataTable = objSC.GetUserGroupFunadd(ddlGroup.SelectedItem.Value, strCompRoleID)
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & ".subQueryGroup", ex)
        End Try
    End Sub
    Public Overrides Sub DoModalReturn(ByVal returnValue As String)
        Dim strSql As String = ""

        If returnValue <> "" Then
            Dim aryData() As String = returnValue.Split(":")
            Select Case aryData(0)
                '員工編號
                Case "ucSelectEmpID"
                    Dim aryValue() As String = Split(aryData(1), "|$|")
                    'lblCompRoleNameU.Text = aryValue(0)
                    'lblUserID.Text = aryValue(1)
                    'lblUserName.Text = aryValue(2)
                    'lblCompRoleCompIDU.Text = aryValue(3)

                    '20151008 Beatrice Modify
                    Bsp.Utility.SetSelectedIndex(ddlUserComp, aryValue(3))
                    txtUserID.Text = aryValue(1)
                    lblUserName.Text = aryValue(2)

                    txtUserID.Enabled = True
            End Select
        End If
    End Sub

    Public Function GetSC_UserGroup(ByVal ParamArray Params() As String) As DataTable
        Dim strSQL As New StringBuilder()
        Dim strWhere As New StringBuilder()
        Dim ht As Hashtable = Bsp.Utility.getHashTableFromParam(Params)

        For Each strKey As String In ht.Keys
            If Bsp.Utility.IsStringNull(ht(strKey)) <> "" Then
                Select Case strKey
                    Case "CompID"
                        strWhere.AppendLine("And CompID = " & Bsp.Utility.Quote(UserProfile.CompID))
                    Case "UserID"
                        strWhere.AppendLine("And UserID = " & Bsp.Utility.Quote(txtUserID.Text))
                    Case "CompRoleID"
                        strWhere.AppendLine("And CompRoleID = " & Bsp.Utility.Quote(ddlCompRoleName.SelectedValue))
                    Case "GroupID"
                        strWhere.AppendLine("And GroupID = " & Bsp.Utility.Quote(ddlGroup.SelectedItem.Value))
                End Select
            End If
        Next

        strSQL.AppendLine(" select * from SC_UserGroup ")
        'strSQL.AppendLine(" where CompID = '" & UserProfile.CompID & "' and UserID = '" & lblUserID.Text & "' and CompRoleID  = '" & ddlCompRoleName.SelectedValue & "' and GroupID  = '" & ddlGroup.SelectedItem.Value & "' ")
        strSQL.AppendLine("Where 1 = 1")
        If strWhere.ToString() <> "" Then strSQL.AppendLine(strWhere.ToString())

        Return Bsp.DB.ExecuteDataSet(CommandType.Text, strSQL.ToString()).Tables(0)

    End Function

    '20151008 Beatrice Add
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

    '20151008 Beatrice Add
    Protected Sub txtUserID_TextChanged(sender As Object, e As System.EventArgs) Handles txtUserID.TextChanged
        If ddlUserComp.SelectedValue <> "" And txtUserID.Text <> "" Then
            Dim objSC As New SC
            Dim WorkStatus As String = objSC.GetUserWorkStatus(ddlUserComp.SelectedValue, txtUserID.Text)
            If WorkStatus = "" Then
                lblUserName.Text = ""
                Bsp.Utility.ShowFormatMessage(Me, "H_00000", "人事資料尚未建檔")
            ElseIf WorkStatus <> "1" Then
                lblUserName.Text = ""
                Bsp.Utility.ShowFormatMessage(Me, "H_00000", "人員非在職狀態")
            Else
                lblUserName.Text = objSC.GetSC_UserName(ddlUserComp.SelectedValue, txtUserID.Text)
            End If
        Else
            lblUserName.Text = ""
        End If
    End Sub
End Class
