'****************************************************
'功能說明：群組功能維護－by群組－新增
'建立人員：Ann
'建立日期：2014/09/01
'****************************************************
Imports System.Data

Partial Class SC_SC0501
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then
            'Bsp.Utility.FillGroup(ddlGroup) '20150303 Beatrice del
            Dim objSC As New SC
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
                    'ddlCompRoleName.Items.Insert(0, New ListItem("全金控", "0"))
                    ddlCompRoleName.Items.Insert(0, New ListItem("全金控", "ALL")) '20150306 Beatrice modify
                    lblCompRoleName.Visible = False

                    ddlCompRoleName.SelectedValue = UserProfile.SelectCompRoleID '20150304 Beatrice modify
                Else
                    '非系統管理者
                    ddlCompRoleName.Visible = False
                    lblCompRoleName.Text = objSC.GetCompName(UserProfile.SelectCompRoleID).Rows(0).Item("CompName").ToString()
                    lblCompRoleName.Visible = True
                End If
            End If

            SetDropDownData() '20150303 Beatrice modify
        End If
    End Sub


    Protected Overrides Sub BaseOnPageTransfer(ByVal ti As TransferInfo)
        '20150303 Beatrice del
        'Dim ht As Hashtable = Bsp.Utility.getHashTableFromParam(ti.Args)

        'If ht.ContainsKey("SelectedGroupID") Then
        '    Bsp.Utility.SetSelectedIndex(ddlGroup, ht("SelectedGroupID").ToString())
        'End If

        'If ddlCompRoleName.SelectedValue <> "" Then
        '    Bsp.Utility.FillFun(ddlFun, Bsp.Enums.SelectFunType.NotAssignToGroup, ddlGroup.SelectedValue, ddlCompRoleName.SelectedValue)
        'Else
        '    Bsp.Utility.FillFun(ddlFun, Bsp.Enums.SelectFunType.NotAssignToGroup, ddlGroup.SelectedValue, UserProfile.SelectCompRoleID)
        'End If

        'If ddlFun.SelectedIndex >= 0 Then LoadData(ddlFun.SelectedValue)
        LoadData(ddlFun.SelectedValue)
    End Sub

    '20150303 Beatrice Add
    Private Sub SetDropDownData()
        Dim strCompRoleID As String
        strCompRoleID = ddlCompRoleName.SelectedValue
        If strCompRoleID = "" Then
            strCompRoleID = UserProfile.SelectCompRoleID
        End If

        Bsp.Utility.FillGroup_0501(ddlGroup, strCompRoleID)
        ddlGroup.Items.Insert(0, New ListItem("---請選擇---", ""))

        Bsp.Utility.FillFun(ddlFun, Bsp.Enums.SelectFunType.FunHasRight, UserProfile.LoginSysID, strCompRoleID)
        ddlFun.Items.Insert(0, New ListItem("---請選擇---", ""))
    End Sub
    '20150303 Beatrice Add End

    Public Overrides Sub DoAction(ByVal Param As String)
        Select Case Param
            Case "btnAdd"
                If funCheckData() Then
                    If SaveData() Then
                        If ddlCompRoleName.SelectedValue <> "" Then
                            Bsp.Utility.FillFun(ddlFun, Bsp.Enums.SelectFunType.NotAssignToGroup, ddlGroup.SelectedValue, ddlCompRoleName.SelectedValue)
                        Else
                            Bsp.Utility.FillFun(ddlFun, Bsp.Enums.SelectFunType.NotAssignToGroup, ddlGroup.SelectedValue, UserProfile.SelectCompRoleID)
                        End If
                        'If ddlFun.SelectedIndex >= 0 Then LoadData(ddlFun.SelectedValue)
                        '20150305 Beatrice modify
                        ddlFun.Items.Insert(0, New ListItem("---請選擇---", ""))
                        LoadData(ddlFun.SelectedValue)
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

    Private Sub LoadData(ByVal sFunID As String)
        Dim objSC As New SC
        Dim bsRight As New beSC_Right.Service()
        Dim ht As New Hashtable()

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
        If ddlFun.Items.Count = 0 Then Return

        If sFunID IsNot Nothing AndAlso sFunID <> "" Then
            Try
                Using dt As DataTable = objSC.GetFunRightInfo(sFunID, "FR.RightID, FR.Caption, RT.RightName, RT.OrderSeq", "Order by RT.OrderSeq")
                    For Each dr As DataRow In dt.Rows
                        Dim chkbox As CheckBox = Me.Page.Form.FindControl("chkRight" & dr.Item("RightID").ToString())

                        If chkbox IsNot Nothing Then
                            chkbox.Enabled = True
                            chkbox.Checked = True
                            chkbox.Text = IIf(dr.Item("Caption").ToString() = "", dr.Item("RightName").ToString(), dr.Item("Caption").ToString())
                        End If
                    Next
                End Using
            Catch ex As Exception
                Bsp.Utility.ShowMessage(Me, Me.FunID & ".LoadData.2", ex)
            End Try
        End If
    End Sub

    'Private Function SaveData() As Boolean
    '    If ddlFun.Items.Count = 0 Then
    '        Bsp.Utility.ShowFormatMessage(Me, "W_00090")
    '        Return False
    '    End If

    '    Dim beGroupFun() As beSC_GroupFun.Row = Nothing

    '    For Each ctl As Control In Me.Page.Form.Controls
    '        If TypeOf ctl Is CheckBox Then
    '            If Left(ctl.ID, 8) = "chkRight" Then
    '                If CType(ctl, CheckBox).Enabled AndAlso CType(ctl, CheckBox).Checked Then
    '                    Dim beGF As New beSC_GroupFun.Row()

    '                    beGF.SysID.Value = UserProfile.LoginSysID
    '                    If lblCompRoleName.Text <> "" Then
    '                        beGF.CompRoleID.Value = UserProfile.SelectCompRoleID
    '                    Else
    '                        beGF.CompRoleID.Value = ddlCompRoleName.SelectedValue
    '                    End If
    '                    beGF.GroupID.Value = ddlGroup.SelectedValue
    '                    beGF.FunID.Value = ddlFun.SelectedValue
    '                    beGF.RightID.Value = Right(ctl.ID, 1)
    '                    beGF.CreateDate.Value = Format(Now, "yyyy/MM/dd HH:mm:ss")
    '                    beGF.LastChgDate.Value = Format(Now, "yyyy/MM/dd HH:mm:ss")
    '                    beGF.LastChgID.Value = UserProfile.UserID

    '                    If beGroupFun Is Nothing Then
    '                        ReDim beGroupFun(0)
    '                    Else
    '                        ReDim Preserve beGroupFun(beGroupFun.Length)
    '                    End If
    '                    beGroupFun(beGroupFun.GetUpperBound(0)) = beGF
    '                End If
    '            End If
    '        End If
    '    Next

    '    If beGroupFun Is Nothing Then
    '        Bsp.Utility.ShowFormatMessage(Me, "W_02400")
    '        Return False
    '    End If

    '    Dim objSC As New SC
    '    Try
    '        If ddlCompRoleName.SelectedValue = "0" Then
    '            objSC.AddGroupFunAll(ddlCompRoleName.SelectedValue, ddlGroup.SelectedValue, ddlFun.SelectedValue, beGF.RightID.Value)
    '        Else
    '            objSC.AddGroupFun(beGroupFun)
    '        End If
    '    Catch ex As Exception
    '        Bsp.Utility.ShowMessage(Me, Me.FunID & ".SaveData", ex)
    '        Return False
    '    End Try

    '    Return True
    'End Function

    Private Function SaveData() As Boolean
        If ddlFun.Items.Count = 0 Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00090")
            Return False
        End If

        Dim objSC As New SC
        Dim beGroupFun() As beSC_GroupFun.Row = Nothing

        If lblCompRoleName.Text <> "" Then 'lblCompRoleName.Text <> "" 表示非admin身分
            For Each ctl As Control In Me.Page.Form.Controls
                If TypeOf ctl Is CheckBox Then
                    If Left(ctl.ID, 8) = "chkRight" Then
                        If CType(ctl, CheckBox).Enabled AndAlso CType(ctl, CheckBox).Checked Then
                            Dim beGF As New beSC_GroupFun.Row()

                            beGF.SysID.Value = UserProfile.LoginSysID
                            'If lblCompRoleName.Text <> "" Then
                            beGF.CompRoleID.Value = UserProfile.SelectCompRoleID
                            'Else
                            'beGF.CompRoleID.Value = ddlCompRoleName.SelectedValue
                            'End If
                            beGF.GroupID.Value = ddlGroup.SelectedValue
                            beGF.FunID.Value = ddlFun.SelectedValue
                            beGF.RightID.Value = Right(ctl.ID, 1)
                            beGF.CreateDate.Value = Format(Now, "yyyy/MM/dd HH:mm:ss")
                            beGF.LastChgDate.Value = Format(Now, "yyyy/MM/dd HH:mm:ss")
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
        Else
            'Admin 身分，選擇全金控
            'If ddlCompRoleName.SelectedValue = "0" Then
            If ddlCompRoleName.SelectedValue = "ALL" Then '20150306 Beatrice modify
                For i = 0 To ddlCompRoleName.Items.Count - 1
                    If ddlCompRoleName.Items(i).Value <> "0" Then
                        For Each ctl As Control In Me.Page.Form.Controls
                            If TypeOf ctl Is CheckBox Then
                                If Left(ctl.ID, 8) = "chkRight" Then
                                    If CType(ctl, CheckBox).Enabled AndAlso CType(ctl, CheckBox).Checked Then
                                        Dim beGF As New beSC_GroupFun.Row()

                                        beGF.SysID.Value = UserProfile.LoginSysID
                                        beGF.CompRoleID.Value = ddlCompRoleName.Items(i).Value
                                        beGF.GroupID.Value = ddlGroup.SelectedValue
                                        beGF.FunID.Value = ddlFun.SelectedValue
                                        beGF.RightID.Value = Right(ctl.ID, 1)
                                        beGF.CreateDate.Value = Format(Now, "yyyy/MM/dd HH:mm:ss")
                                        beGF.LastChgDate.Value = Format(Now, "yyyy/MM/dd HH:mm:ss")
                                        beGF.LastChgID.Value = UserProfile.UserID

                                        '20150310 Beatrice modify
                                        If Not objSC.HasGroupFun(beGF) Then
                                            If beGroupFun Is Nothing Then
                                                ReDim beGroupFun(0)
                                            Else
                                                ReDim Preserve beGroupFun(beGroupFun.Length)
                                            End If
                                            beGroupFun(beGroupFun.GetUpperBound(0)) = beGF
                                        End If
                                    End If
                                End If
                            End If
                        Next
                    End If

                Next
            Else
                'Admin 身分，選擇其他授權公司
                For Each ctl As Control In Me.Page.Form.Controls
                    If TypeOf ctl Is CheckBox Then
                        If Left(ctl.ID, 8) = "chkRight" Then
                            If CType(ctl, CheckBox).Enabled AndAlso CType(ctl, CheckBox).Checked Then
                                Dim beGF As New beSC_GroupFun.Row()

                                beGF.SysID.Value = UserProfile.LoginSysID
                                'If lblCompRoleName.Text <> "" Then
                                beGF.CompRoleID.Value = ddlCompRoleName.SelectedValue
                                'Else
                                'beGF.CompRoleID.Value = ddlCompRoleName.SelectedValue
                                'End If
                                beGF.GroupID.Value = ddlGroup.SelectedValue
                                beGF.FunID.Value = ddlFun.SelectedValue
                                beGF.RightID.Value = Right(ctl.ID, 1)
                                beGF.CreateDate.Value = Format(Now, "yyyy/MM/dd HH:mm:ss")
                                beGF.LastChgDate.Value = Format(Now, "yyyy/MM/dd HH:mm:ss")
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

            End If
        End If

        If beGroupFun Is Nothing Then
            Bsp.Utility.ShowFormatMessage(Me, "W_02400")
            Return False
        End If


        Try
            objSC.AddGroupFun(beGroupFun)
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & ".SaveData", ex)
            Return False
        End Try
        LoadData(ddlFun.SelectedValue)  '20150112 Ann modify
        Return True
    End Function

    Private Function funCheckData() As Boolean

        Return True
    End Function

    Protected Sub ddlCompRoleName_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCompRoleName.SelectedIndexChanged
        'LoadData(ddlFun.SelectedValue)
        'Bsp.Utility.FillGroup(ddlGroup)

        '20150303 Beatrice modify
        If ddlCompRoleName.SelectedValue = "0" Then
            Return
        End If

        If ddlCompRoleName.SelectedValue <> "" Then
            Bsp.Utility.FillGroup_0501(ddlGroup, ddlCompRoleName.SelectedValue)
            Bsp.Utility.FillFun(ddlFun, Bsp.Enums.SelectFunType.NotAssignToGroup, CType(sender, DropDownList).SelectedValue, ddlCompRoleName.SelectedValue)
        Else
            Bsp.Utility.FillGroup_0501(ddlGroup, UserProfile.SelectCompRoleID)
            Bsp.Utility.FillFun(ddlFun, Bsp.Enums.SelectFunType.NotAssignToGroup, CType(sender, DropDownList).SelectedValue, UserProfile.SelectCompRoleID)
        End If

        '20150303 Beatrice modify
        ddlGroup.Items.Insert(0, New ListItem("---請選擇---", ""))
        ddlFun.Items.Insert(0, New ListItem("---請選擇---", ""))

        'LoadData(ddlFun.SelectedValue) '20150303 Beatrice del
    End Sub

    Protected Sub ddlFun_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlFun.SelectedIndexChanged
        '20150303 Beatrice modify
        If ddlFun.SelectedValue = "" Then
            LoadData(ddlFun.SelectedValue)
            Return
        End If

        Dim ddlGroupSelectedValue As String = ddlGroup.SelectedValue

        If ddlCompRoleName.SelectedValue <> "" Then
            Bsp.Utility.FillGroup_0504(ddlGroup, Bsp.Enums.SelectGroupType.NotHasFun, ddlFun.SelectedValue, ddlCompRoleName.SelectedValue)
        Else
            Bsp.Utility.FillGroup_0504(ddlGroup, Bsp.Enums.SelectGroupType.NotHasFun, ddlFun.SelectedValue, UserProfile.SelectCompRoleID)
        End If

        ddlGroup.Items.Insert(0, New ListItem("---請選擇---", ""))
        Try
            ddlGroup.SelectedValue = ddlGroupSelectedValue
        Catch ex As Exception
            ddlGroup.SelectedValue = ""
        End Try
        '20150303 Beatrice modify End

        LoadData(ddlFun.SelectedValue)
    End Sub

    Protected Sub ddlGroup_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlGroup.SelectedIndexChanged
        '20150303 Beatrice modify
        If ddlGroup.SelectedValue = "" Then
            Return
        End If

        Dim ddlFunSelectedValue As String = ddlFun.SelectedValue
        '20150303 Beatrice modify End

        If ddlCompRoleName.SelectedValue <> "" Then
            Bsp.Utility.FillFun(ddlFun, Bsp.Enums.SelectFunType.NotAssignToGroup, CType(sender, DropDownList).SelectedValue, ddlCompRoleName.SelectedValue)
        Else
            Bsp.Utility.FillFun(ddlFun, Bsp.Enums.SelectFunType.NotAssignToGroup, CType(sender, DropDownList).SelectedValue, UserProfile.SelectCompRoleID)
        End If

        '20150303 Beatrice modify
        ddlFun.Items.Insert(0, New ListItem("---請選擇---", ""))
        Try
            ddlFun.SelectedValue = ddlFunSelectedValue
        Catch ex As Exception
            ddlFun.SelectedValue = ""
        End Try
        '20150303 Beatrice modify End

        'LoadData(ddlFun.SelectedValue) '20150303 Beatrice del
    End Sub
End Class
