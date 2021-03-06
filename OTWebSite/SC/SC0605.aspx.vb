'****************************************************
'功能說明：SC_GroupFun維護-權限複製
'建立人員：Chung
'建立日期：2011/05/20
'****************************************************
Imports System.Data

Partial Class SC_SC0505
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then
            'Bsp.Utility.FillGroup(ddlGroup)
            Dim objSC As New SC

            '來源群組
            ddlGroup.Items.Insert(0, "---請先選擇來源授權公司---")
            ddlCopyGroup.Items.Insert(0, "---請先選擇來源授權公司---")

            If lblCompRoleName.Text <> "" Then
                subLoadtxtGroup()
            End If

            lblCopyCompID.Text = objSC.GetCompName(UserProfile.SelectCompRoleID).Rows(0).Item("CompName").ToString()

            'ddlGroupType.Attributes.Add("onchange", "funGroupChange();")
            Dim strSysID As String
            strSysID = Bsp.Utility.subGetSysID(UserProfile.LoginSysID)
            Dim arySysID() As String = Split(strSysID, "-")
            lblSysName.Text = strSysID
            ddlCompRoleName.Visible = False

            If UserProfile.SelectCompRoleID = "ALL" Then
                ddlCompRoleName.Visible = True
                Bsp.Utility.FillCompany(ddlCompRoleName)
                ddlCompRoleName.SelectedIndex = 0
                lblCompRoleName.Visible = False
            Else
                '系統管理者
                If UserProfile.IsSysAdmin = True Then
                    ddlCompRoleName.Visible = True
                    Bsp.Utility.FillCompany(ddlCompRoleName)
                    ddlCompRoleName.Items.Insert(0, "ALL-全金控")
                    ddlCompRoleName.SelectedIndex = 0
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

    'Protected Overrides Sub BaseOnPageTransfer(ByVal ti As TransferInfo)
    '    Dim ht As Hashtable = Bsp.Utility.getHashTableFromParam(ti.Args)
    '    Dim objSC As New SC()

    '    For Each strKey As String In ht.Keys
    '        Select Case strKey
    '            Case "SelectedGroupID"
    '                ViewState.Item("GroupID") = ht(strKey).ToString()
    '                ViewState.Item("CompRoleID") = ht(strKey).ToString()
    '                ViewState.Item("SysID") = ht(strKey).ToString()
    '                Using dt As DataTable = objSC.GetGroupInfo(ht(strKey).ToString(), ht(strKey).ToString(), ht(strKey).ToString(), "GroupName")
    '                    If dt.Rows.Count > 0 Then
    '                        lblGroup.Text = String.Format("{0}-{1}", ht(strKey).ToString(), dt.Rows(0).Item(0).ToString())
    '                    End If
    '                End Using
    '            Case "SelectedFunID"
    '                ViewState.Item("FunID") = ht(strKey).ToString()
    '                Using dt As DataTable = objSC.GetFunInfo(ht(strKey).ToString(), "FunName")
    '                    If dt.Rows.Count > 0 Then
    '                        lblFun.Text = String.Format("{0}-{1}", ht(strKey).ToString(), dt.Rows(0).Item(0).ToString())
    '                    End If
    '                End Using
    '        End Select
    '    Next

    '    LoadData(Bsp.Utility.IsStringNull(ViewState.Item("GroupID")), Bsp.Utility.IsStringNull(ViewState.Item("FunID")))
    'End Sub

    Private Sub LoadData(ByVal GroupID As String, ByVal sFunID As String)
        Dim objSC As New SC
        Dim bsRight As New beSC_Right.Service()
        Dim ht As New Hashtable

        Try
            Using dtRight As DataTable = bsRight.QuerybyWhere("").Tables(0)
                For Each dr As DataRow In dtRight.Rows
                    ht.Add(dr.Item("RightID").ToString(), dr.Item("RightName").ToString())
                Next
            End Using

            For Each ctl As Control In Page.Form.Controls
                If TypeOf ctl Is CheckBox Then
                    If Left(ctl.ID, 8) = "chkRight" Then
                        CType(ctl, CheckBox).Enabled = False
                        CType(ctl, CheckBox).Checked = False
                        Dim a As New Hashtable
                        CType(ctl, CheckBox).Text = ht.Item(Right(ctl.ID, 1))
                    End If
                End If
            Next
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & ".LoadData.1", ex)
            Return
        Finally
            ht.Clear()
        End Try

        If sFunID IsNot Nothing AndAlso sFunID <> "" Then
            Try
                '撈取FunRight
                Using dt As DataTable = objSC.GetFunRightInfo(sFunID, "FR.RightID, FR.Caption, RT.RightName, RT.OrderSeq", "Order by RT.OrderSeq")
                    For Each dr As DataRow In dt.Rows
                        Dim chkbox As CheckBox = Me.Page.Form.FindControl("chkRight" & dr.Item("RightID").ToString())

                        If chkbox IsNot Nothing Then
                            chkbox.Enabled = True
                            chkbox.Text = IIf(dr.Item("Caption").ToString() = "", dr.Item("RightName").ToString(), dr.Item("Caption").ToString())
                        End If
                    Next
                End Using

                '撈取GroupFun
                Using dt As DataTable = objSC.GetGroupFunInfo(GroupID, sFunID, "RightID")
                    For Each dr As DataRow In dt.Rows
                        Dim chkbox As CheckBox = Me.Page.Form.FindControl("chkRight" & dr.Item("RightID").ToString())

                        If chkbox IsNot Nothing AndAlso chkbox.Enabled Then
                            chkbox.Checked = True
                        End If
                    Next
                End Using
            Catch ex As Exception
                Bsp.Utility.ShowMessage(Me, Me.FunID & ".LoadData.2", ex)
            End Try
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

    Private Sub GetData()
    End Sub

    Private Function SaveData() As Boolean
        Dim beGroupFun() As beSC_GroupFun.Row = Nothing

        For Each ctl As Control In Me.Page.Form.Controls
            If TypeOf ctl Is CheckBox Then
                If Left(ctl.ID, 8) = "chkRight" Then
                    If CType(ctl, CheckBox).Enabled AndAlso CType(ctl, CheckBox).Checked Then
                        Dim beGF As New beSC_GroupFun.Row()

                        beGF.GroupID.Value = Bsp.Utility.IsStringNull(ViewState.Item("GroupID"))
                        beGF.FunID.Value = Bsp.Utility.IsStringNull(ViewState.Item("FunID"))
                        beGF.RightID.Value = Right(ctl.ID, 1)
                        beGF.CreateDate.Value = Now
                        beGF.LastChgDate.Value = Now
                        beGF.LastChgID.Value = UserProfile.UserID

                        If beGroupFun Is Nothing Then
                            ReDim beGroupFun(0)
                        Else
                            ReDim Preserve beGroupFun(beGroupFun.Length)
                        End If
                        beGroupFun(beGroupFun.GetUpperBound(0)) = beGF
                    End If
                End If
            End If
        Next

        Dim objSC As New SC
        Try
            If beGroupFun Is Nothing Then
                objSC.DeleteGroupFun(Bsp.Utility.IsStringNull(ViewState.Item("GroupID")), Bsp.Utility.IsStringNull(ViewState.Item("FunID")), Bsp.Utility.IsStringNull(ViewState.Item("SysID")), Bsp.Utility.IsStringNull(ViewState.Item("CompRoleID")))
            Else
                objSC.UpdateGroupFun(beGroupFun)
            End If
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & ".SaveData", ex)
            Return False
        End Try

        Return True
    End Function

    Private Function funCheckData() As Boolean

        Return True
    End Function

    Protected Sub ddlCompRoleName_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCompRoleName.SelectedIndexChanged
        subLoadGroup()
    End Sub

    Protected Sub ddlGroup_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlGroup.SelectedIndexChanged
        subLoadCopyGroup()
        subQueryGroup()
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

                    If dt.Rows.Count > 0 Then
                        strGroup = dt.Rows(0).Item(0).ToString
                        Bsp.Utility.SetSelectedIndex(ddlGroup, strGroup)
                        'UpdGroup.Update()
                    End If

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

                    If dt.Rows.Count > 0 Then
                        strGroup = dt.Rows(0).Item(0).ToString
                        Bsp.Utility.SetSelectedIndex(ddlGroup, strGroup)
                        'UpdGroup.Update()
                    End If

                End With
            End Using

        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Bsp.Utility.getInnerException("subLoadGroup：", ex))
            Return
        End Try
    End Sub

    '20140905 Ann add 複製至群組依據來源授權公司帶入
    Private Sub subLoadCopyGroup()
        Dim strCopyGroup As String = ""
        Dim objSC As New SC
        Try
            Using dt As Data.DataTable = objSC.GetGroup(UserProfile.LoginSysID, lblCopyCompID.Text)
                With ddlCopyGroup
                    .DataSource = dt
                    .DataTextField = "GroupName"
                    .DataValueField = "GroupID"
                    .DataBind()

                    .Items.Insert(0, New ListItem("---請選擇---", ""))

                    If dt.Rows.Count > 0 Then
                        strCopyGroup = dt.Rows(0).Item(0).ToString
                        Bsp.Utility.SetSelectedIndex(ddlGroup, strCopyGroup)
                        'UpdGroup.Update()
                    End If

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
            pcMain.DataTable = objSC.GetGroupFunCopy(Bsp.Enums.GroupFunType.Group, ddlGroup.SelectedItem.Value)
            ''Using dt As DataTable = objSC.GetGroupInfo_0500(GroupID, "GroupID + '-' + GroupName FullName")
            'Using dt As DataTable = objSC.GetGroupInfo_0500(ddlGroup.SelectedValue, ddlCompRoleName.SelectedValue, UserProfile.LoginSysID, "GroupID + '-' + GroupName FullName")  '20140903 Ann add
            '    If dt.Rows.Count > 0 Then
            '        'lblGroup.Text = dt.Rows(0).Item("FullName").ToString()
            '    End If
            'End Using
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & ".QueryData", ex)
        End Try
    End Sub
End Class
