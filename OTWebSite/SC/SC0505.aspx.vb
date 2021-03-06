'****************************************************
'功能說明：SC_GroupFun維護-權限複製
'建立人員：Ann
'建立日期：2014/09/01
'****************************************************
Imports System.Data
Imports System.Data.Common


Partial Class SC_SC0505
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then
            'Bsp.Utility.FillGroup(ddlGroup)
            Dim objSC As New SC

            '來源群組
            ddlGroup.Items.Insert(0, "---請先選擇來源授權公司---")
            ddlCopyGroup.Items.Insert(0, "---請先選擇複製至授權公司---") '20150401 Beatrice modify
            'subLoadCopyGroup()

            'If lblCompRoleName.Text <> "" Then
            '    subLoadtxtGroup()
            'End If

            'lblCopyCompID.Text = objSC.GetCompName(UserProfile.SelectCompRoleID).Rows(0).Item("CompName").ToString()

            'ddlGroupType.Attributes.Add("onchange", "funGroupChange();")
            Dim strSysID As String
            strSysID = Bsp.Utility.subGetSysID(UserProfile.LoginSysID)
            Dim arySysID() As String = Split(strSysID, "-")
            lblSysName.Text = strSysID
            'ddlCompRoleName.Visible = False

            'If UserProfile.SelectCompRoleID = "ALL" Then
            'ddlCompRoleName.Visible = True
            Bsp.Utility.FillCompany(ddlCompRoleName)
            ddlCompRoleName.Items.Insert(0, New ListItem("---請選擇---", "")) '20150401 Beatrice modify
            ddlCompRoleName.SelectedIndex = 0
            'lblCompRoleName.Visible = False
            'Else
            '系統管理者
            If UserProfile.IsSysAdmin = True Then
                '20150401 Beatrice modify
                ddlCopyCompID.Visible = True
                Bsp.Utility.FillCompany(ddlCopyCompID)
                ddlCopyCompID.Items.Insert(0, New ListItem("---請選擇---", ""))
                ddlCopyCompID.SelectedIndex = 0
                lblCopyCompID.Visible = False

                'ddlCompRoleName.Visible = True
                'Bsp.Utility.FillCompany(ddlCompRoleName)
                'ddlCompRoleName.Items.Insert(0, "ALL-全金控")
                'ddlCompRoleName.SelectedIndex = 0
                'lblCompRoleName.Visible = False
            Else
                '非系統管理者
                '20150401 Beatrice modify
                ddlCopyCompID.Visible = False
                lblCopyCompID.Text = objSC.GetCompName(UserProfile.SelectCompRoleID).Rows(0).Item("CompName").ToString()
                lblCopyCompID.Visible = True
                subLoadCopyGroup(UserProfile.SelectCompRoleID)

                'ddlCompRoleName.Visible = False
                'lblCompRoleName.Text = objSC.GetCompName(UserProfile.SelectCompRoleID).Rows(0).Item("CompName").ToString()
                'lblCompRoleName.Visible = True
            End If
            'End If
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

    Private Sub GetData()
    End Sub

    Private Function SaveData() As Boolean
        Dim strCopyCompID As String = ""
        strCopyCompID = ddlCopyCompID.SelectedValue
        If strCopyCompID = "" Then
            strCopyCompID = UserProfile.SelectCompRoleID
        End If

        Dim strSQL As New StringBuilder()

        '20150401 Beatrice modify
        strSQL.AppendLine("Delete From SC_GroupFun Where CompRoleID = " & Bsp.Utility.Quote(strCopyCompID) & " AND GroupID = " & Bsp.Utility.Quote(ddlCopyGroup.SelectedValue))
        strSQL.AppendLine("Insert into SC_GroupFun (SysID, CompRoleID, GroupID, FunID, RightID, CreateDate, LastChgComp, LastChgID, LastChgDate)")
        strSQL.AppendLine("select SysID, " & Bsp.Utility.Quote(strCopyCompID) & ", " & Bsp.Utility.Quote(ddlCopyGroup.SelectedValue) & ", FunID, RightID, CreateDate")
        strSQL.AppendLine(", " & Bsp.Utility.Quote(UserProfile.ActCompID) & ", " & Bsp.Utility.Quote(UserProfile.ActUserID) & ", " & Bsp.Utility.Quote(Format(Now, "yyyy/MM/dd HH:mm:ss")) & " From SC_GroupFun")
        strSQL.AppendLine("Where CompRoleID = " & Bsp.Utility.Quote(ddlCompRoleName.SelectedValue) & " AND GroupID = " & Bsp.Utility.Quote(ddlGroup.SelectedValue))
        '20150401 Beatrice modify End

        Using cn As DbConnection = Bsp.DB.getConnection()
            cn.Open()
            Dim tran As Data.Common.DbTransaction = cn.BeginTransaction
            Dim inTrans As Boolean = True

            Try
                Bsp.DB.ExecuteNonQuery(CommandType.Text, strSQL.ToString(), tran, "eHRMSDB")
                tran.Commit()
                inTrans = False
            Catch ex As Exception
                If inTrans Then tran.Rollback()
                Throw
            Finally
                tran.Dispose()
                If cn.State = ConnectionState.Open Then cn.Close()
            End Try
        End Using
        Return True
    End Function

    Private Function funCheckData() As Boolean
        '20150401 Beatrice Add
        If ddlCompRoleName.SelectedValue = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00035", "來源授權公司")
            ddlCompRoleName.Focus()
            Return False
        End If

        If ddlGroup.SelectedValue = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00035", "來源群組")
            ddlGroup.Focus()
            Return False
        End If

        If ddlCopyCompID.SelectedValue = "" And lblCopyCompID.Text = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00035", "複製至授權公司")
            ddlCopyCompID.Focus()
            Return False
        End If

        If ddlCopyGroup.SelectedValue = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00035", "複製至群組")
            ddlCopyGroup.Focus()
            Return False
        End If
        '20150401 Beatrice Add End
        Return True
    End Function

    Protected Sub ddlCompRoleName_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCompRoleName.SelectedIndexChanged
        subLoadGroup()
    End Sub

    Protected Sub ddlGroup_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlGroup.SelectedIndexChanged
        subQueryGroup()
    End Sub

    '20150401 Beatrice Add
    Protected Sub ddlCopyCompID_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCopyCompID.SelectedIndexChanged
        subLoadCopyGroup(ddlCopyCompID.SelectedValue)
    End Sub

    '20140905 Ann add 來源群組依據來源授權公司帶入
    Private Sub subLoadGroup()
        Dim strGroup As String = ""
        Dim objSC As New SC
        Try
            Using dt As Data.DataTable = objSC.GetGroup(UserProfile.LoginSysID, ddlCompRoleName.SelectedValue)
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

    '20140905 Ann add 來源群組依據來源授權公司帶入
    Private Sub subLoadtxtGroup()
        Dim strGroup As String = ""
        Dim objSC As New SC
        Try
            Using dt As Data.DataTable = objSC.GetGroup(UserProfile.LoginSysID, lblCompRoleName.Text)
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
            Bsp.Utility.ShowMessage(Me, Bsp.Utility.getInnerException("subLoadtxtGroup：", ex))
            Return
        End Try
    End Sub

    '20140905 Ann add 複製至群組依據來源授權公司帶入
    'Private Sub subLoadCopyGroup() '20150401 Beatrice modify
    Private Sub subLoadCopyGroup(ByVal strCompID As String)
        Dim strCopyGroup As String = ""
        Dim objSC As New SC
        Try
            Using dt As Data.DataTable = objSC.GetGroup(UserProfile.LoginSysID, strCompID) 'UserProfile.SelectCompRoleID
                With ddlCopyGroup
                    .DataSource = dt
                    .DataTextField = "GroupName"
                    .DataValueField = "GroupID"
                    .DataBind()

                    .Items.Insert(0, New ListItem("---請選擇---", ""))

                    'If dt.Rows.Count > 0 Then
                    '    strCopyGroup = dt.Rows(0).Item(0).ToString
                    '    Bsp.Utility.SetSelectedIndex(ddlGroup, strCopyGroup)
                    '    'UpdGroup.Update()
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
            pcMain.DataTable = objSC.GetGroupFun_0500(ddlGroup.SelectedValue, "", ddlCompRoleName.SelectedValue) '20150401 Beatrice modify
            'objSC.GetGroupFunCopy(Bsp.Enums.GroupFunType.Group, ddlGroup.SelectedItem.Value)
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & ".QueryData", ex)
        End Try
    End Sub
End Class
