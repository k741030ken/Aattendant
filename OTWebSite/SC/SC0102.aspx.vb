'****************************************************
'功能說明：使用者(SC_User)維護-修改
'建立人員：Chung
'建立日期：2011/05/16
'****************************************************
Imports System.Data
Imports System.Data.Common

Partial Class SC_SC0102
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then
            Dim objSC As New SC

            'If UserProfile.SelectCompRoleID = "ALL" Then
            '    ddlCompRoleName.Visible = True
            '    Bsp.Utility.FillCompany(ddlCompRoleName)
            '    lblCompRoleName.Visible = False
            'Else
            '    '系統管理者
            '    If UserProfile.IsSysAdmin = True Then
            '        ddlCompRoleName.Visible = True
            '        Bsp.Utility.FillCompany(ddlCompRoleName)
            '        lblCompRoleName.Visible = False
            '    Else
            '        '非系統管理者
            '        ddlCompRoleName.Visible = False
            '        lblCompRoleName.Text = objSC.GetCompName(UserProfile.SelectCompRoleID).Rows(0).Item("CompName").ToString()
            '        lblCompRoleName.Visible = True
            '    End If
            'End If
        End If
    End Sub

    Protected Overrides Sub BaseOnPageTransfer(ByVal ti As TransferInfo)
        If Not IsPostBack() Then
            Dim ht As Hashtable = Bsp.Utility.getHashTableFromParam(ti.Args)

            If ht.ContainsKey("CompID") Then
                ViewState.Item("CompID") = ht("CompID").ToString()
                ViewState.Item("UserID") = ht("UserID").ToString()
                GetData(ht("CompID").ToString(), ht("UserID").ToString())
                'GetData(ViewState.Item("SysID").ToString(), ViewState.Item("CompID").ToString(), ViewState.Item("UserID").ToString())
            Else
                Return
            End If
        End If
    End Sub

    Public Overrides Sub DoAction(ByVal Param As String)
        Select Case Param
            Case "btnUpdate"
                If funCheckData() Then
                    If SaveData() Then
                        GoBack()
                    End If
                End If
            Case "btnActionX"
                GoBack()
        End Select
    End Sub

    Private Sub GoBack()
        Dim ti As TransferInfo = Me.StateTransfer
        Me.TransferFramePage(ti.CallerUrl, Nothing, ti.Args)
    End Sub

    Private Sub GetData(ByVal CompID As String, ByVal UserID As String)
        Dim objSC As New SC
        Dim bsUser As New beSC_User.Service()
        Dim beUser As New beSC_User.Row()

        Dim strSysID As String
        'Dim strUserName As String
        'Dim strBanMark As String
        Dim strTitle As String
        Dim strWorkStatus As String
        Dim strDeptName As String
        Dim strOrganName As String

        beUser.CompID.Value = CompID
        beUser.UserID.Value = UserID
        'strUserName = Bsp.Utility.subUserName(beUser.CompID.Value, beUser.UserID.Value)
        'strSysID = Bsp.Utility.subGetSysID(UserProfile.ActUserID)
        strSysID = Bsp.Utility.subGetSysID(UserProfile.LoginSysID)
        strWorkStatus = Bsp.Utility.subWorkStatus(beUser.CompID.Value, beUser.UserID.Value)
        strDeptName = Bsp.Utility.subDeptName(beUser.CompID.Value, beUser.UserID.Value)
        strOrganName = Bsp.Utility.subOrganName(beUser.CompID.Value, beUser.UserID.Value)
        strTitle = Bsp.Utility.subTitle(beUser.CompID.Value, beUser.UserID.Value)
        'strBanMark = Bsp.Utility.subBankMark(beUser.CompID.Value, beUser.UserID.Value)

        Using dt As DataTable = bsUser.QueryByKey(beUser).Tables(0)
            If dt.Rows.Count <= 0 Then Exit Sub
            beUser = New beSC_User.Row(dt.Rows(0))
            Try
                lblSysName.Text = strSysID
                lblUserID.Text = UserID
                lblUserName.Text = beUser.UserName.Value
                lblCompRoleName.Text = objSC.GetCompName(CompID).Rows(0).Item("CompName").ToString()
                lblWorkStatus.Text = strWorkStatus
                lblDeptID.Text = strDeptName
                lblOrganID.Text = strOrganName
                lblTitleID.Text = strTitle
                chkBanMark.Checked = IIf(beUser.BanMark.Value = "1", True, False)

                '20151116 Beatrice Modify
                If chkBanMark.Checked Then
                    txtBanMarkValidDate.Text = beUser.BanMarkValidDate.Value.ToString("yyyy/MM/dd")
                Else
                    txtBanMarkValidDate.Enabled = False
                End If
                'If txtBanMarkValidDate.Text <> "" Then
                '    txtBanMarkValidDate.Text = beUser.BanMarkValidDate.Value.ToString("yyyy/MM/dd")
                'Else
                '    txtBanMarkValidDate.Text = Format(Now, "yyyy/MM/dd")
                'End If
                lblCreateDate.Text = beUser.CreateDate.Value.ToString("yyyy/MM/dd HH:mm:ss")

                '20150312 Beatrice modify
                Dim CompName As String = objSC.GetSC_CompName(beUser.LastChgComp.Value)
                lblLastChgComp.Text = beUser.LastChgComp.Value + IIf(CompName <> "", "-" + CompName, "")
                Dim UserName As String = objSC.GetSC_UserName(beUser.LastChgComp.Value, beUser.LastChgID.Value)
                lblLastChgID.Text = beUser.LastChgID.Value + IIf(UserName <> "", "-" + UserName, "")
                '20150312 Beatrice modify End
                lblLastChgDate.Text = beUser.LastChgDate.Value.ToString("yyyy/MM/dd HH:mm:ss")
            Catch ex As Exception
                Bsp.Utility.ShowMessage(Me, Me.FunID & ".GetData", ex)
            End Try
        End Using
    End Sub

    Private Function SaveData() As Boolean
        Dim bsUser As New beSC_User.Service()
        Dim beUser As New beSC_User.Row()
        Dim objSC As New SC
        Dim strSQL As New StringBuilder

        Try
            beUser.UserID.Value = lblUserID.Text
            beUser.UserName.Value = lblUserName.Text
            beUser.CompID.Value = ViewState.Item("CompID")
            beUser.WorkStatus.Value = lblWorkStatus.Text
            beUser.DeptID.Value = lblDeptID.Text
            beUser.OrganID.Value = lblOrganID.Text
            beUser.TitleID.Value = lblTitleID.Text

            beUser.BanMark.Value = IIf(chkBanMark.Checked, "1", "0")
            If beUser.BanMark.Value = "1" Then
                beUser.BanMarkValidDate.Value = Bsp.Utility.CheckDate(txtBanMarkValidDate.Text)
            Else
                beUser.BanMarkValidDate.Value = "1900/01/01"
            End If
            beUser.LastChgComp.Value = UserProfile.ActCompID
            beUser.LastChgID.Value = UserProfile.ActUserID
            beUser.LastChgDate.Value = Format(Now, "yyyy/MM/dd HH:mm:ss")

            If Not bsUser.IsDataExists(beUser) Then
                Bsp.Utility.ShowFormatMessage(Me, "W_00020", "")
                Return False
            End If

            Using cn As DbConnection = Bsp.DB.getConnection()
                cn.Open()

                Dim tran As DbTransaction = cn.BeginTransaction
                Dim inTrans As Boolean = True
                Try
                    'Return objSC.UpdateUser(beUser)
                    strSQL.AppendLine("UPDATE SC_User")
                    strSQL.AppendLine("SET BanMark = " & Bsp.Utility.Quote(beUser.BanMark.Value))
                    strSQL.AppendLine(", BanMarkValidDate = " & Bsp.Utility.Quote(beUser.BanMarkValidDate.Value))
                    strSQL.AppendLine(", LastChgComp = " & Bsp.Utility.Quote(beUser.LastChgComp.Value))
                    strSQL.AppendLine(", LastChgID = " & Bsp.Utility.Quote(beUser.LastChgID.Value))
                    strSQL.AppendLine(", LastChgDate = GETDATE()")
                    '20150225 Beatrice modify 若勾選重設放行密碼，放行密碼錯誤次數更新為0且密碼重設為NEWUSER --20150323 Beatrice del
                    'If chkResetPwd.Checked Then
                    '    strSQL.AppendLine(", PasswordErrorCount = 0, Password = 'NEWUSER'")
                    'End If
                    '20150225 Beatrice modify End
                    strSQL.AppendLine("WHERE CompID = " & Bsp.Utility.Quote(ViewState.Item("CompID")))
                    strSQL.AppendLine("AND UserID = " & Bsp.Utility.Quote(ViewState.Item("UserID")))
                    Bsp.DB.ExecuteNonQuery(CommandType.Text, strSQL.ToString())
                    tran.Commit()
                Catch ex As Exception
                    If inTrans Then tran.Rollback()
                    Return False
                End Try
            End Using
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & ".SaveData", ex)
            Return False
        End Try
        Return True
    End Function

    '20160316 Beatrice modify
    Private Function funCheckData() As Boolean
        If chkBanMark.Checked = True And txtBanMarkValidDate.Text.Trim() = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", "禁用生效日")
            Return False
        Else
            If txtBanMarkValidDate.Text.Trim() <> "" Then
                'If Bsp.Utility.getStringLength(txtBanMarkValidDate.Text) > txtBanMarkValidDate.MaxLength Then
                '    Bsp.Utility.ShowFormatMessage(Me, "W_00040", "禁用生效日", txtBanMarkValidDate.MaxLength.ToString())
                '    txtBanMarkValidDate.Focus()
                '    Return False
                'Else
                If Bsp.Utility.CheckDate(txtBanMarkValidDate.Text) = "" Then
                    Bsp.Utility.ShowFormatMessage(Me, "W_00070", "禁用生效日")
                    txtBanMarkValidDate.Focus()
                    Return False
                End If
                'End If
                'If txtBanMarkValidDate.Text > txtBanMarkValidDate.Text Then
                '    Bsp.Utility.ShowFormatMessage(Me, "W_00130", "")
                '    txtBanMarkValidDate.Focus()
                '    Return False
                'End If
            End If
        End If
        Return True
    End Function

    '20150225 Beatrice modify 重設放行密碼欄位改用Button
    Protected Sub btnResetPwd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnResetPwd.Click
        Dim strSQL As New StringBuilder
        Try
            strSQL.AppendLine("UPDATE SC_User")
            strSQL.AppendLine("SET PasswordErrorCount = 0")
            strSQL.AppendLine(", Password = 'NEWUSER'")
            strSQL.AppendLine(", LastChgComp = " & Bsp.Utility.Quote(UserProfile.ActCompID))
            strSQL.AppendLine(", LastChgID = " & Bsp.Utility.Quote(UserProfile.ActUserID))
            strSQL.AppendLine(", LastChgDate = GETDATE()")
            strSQL.AppendLine("WHERE CompID = " & Bsp.Utility.Quote(ViewState.Item("CompID")))
            strSQL.AppendLine("AND UserID = " & Bsp.Utility.Quote(ViewState.Item("UserID")))
            Bsp.DB.ExecuteNonQuery(CommandType.Text, strSQL.ToString())
            Dim result As Integer = Bsp.DB.ExecuteNonQuery(CommandType.Text, strSQL.ToString())

            If result = 1 Then
                Bsp.Utility.ShowFormatMessage(Me, "H_00000", "放行密碼重設成功")
            Else
                Bsp.Utility.ShowFormatMessage(Me, "H_00000", "放行密碼重設失敗")
            End If
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & ".btnResetPwd_Click", ex)
        End Try
    End Sub

    '20151116 Beatrice Add
    Protected Sub chkBanMark_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkBanMark.CheckedChanged
        If chkBanMark.Checked Then
            txtBanMarkValidDate.Enabled = True
            txtBanMarkValidDate.Text = Format(Now, "yyyy/MM/dd")
        Else
            txtBanMarkValidDate.Enabled = False
            txtBanMarkValidDate.Text = ""
        End If
    End Sub
End Class
