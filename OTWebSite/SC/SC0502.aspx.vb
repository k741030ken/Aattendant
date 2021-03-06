'****************************************************
'功能說明：SC_GroupFun維護-修改
'建立人員：Ann
'建立日期：2014/09/01
'****************************************************
Imports System.Data

Partial Class SC_SC0502
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then
            'Bsp.Utility.FillGroup(ddlGroup)
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
                    ddlCompRoleName.Visible = True
                    Bsp.Utility.FillCompany(ddlCompRoleName)
                    'ddlCompRoleName.Items.Insert(0, "ALL-全金控")
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

    Protected Overrides Sub BaseOnPageTransfer(ByVal ti As TransferInfo)
        Dim ht As Hashtable = Bsp.Utility.getHashTableFromParam(ti.Args)
        Dim objSC As New SC()

        For Each strKey As String In ht.Keys
            Select Case strKey
                Case "SelectedGroupID"
                    ViewState.Item("GroupID") = ht("SelectedGroupID").ToString()  'ht(strKey).ToString()
                    ViewState.Item("SysID") = ht("SelectedSysID").ToString()
                    ViewState.Item("CompRoleID") = ht("SelectedCompRoleID").ToString()
                    'Using dt As DataTable = objSC.GetGroupInfo(ht(strKey).ToString(), ht(strKey).ToString(), ht(strKey).ToString(), "GroupName")
                    'Using dt As DataTable = objSC.GetGroupInfo_0500(ViewState.Item("GroupID"), ViewState.Item("SysID"), ViewState.Item("CompRoleID"), "GroupName")
                    Using dt As DataTable = objSC.GetGroupInfo_0500(ViewState.Item("GroupID"), ViewState.Item("SysID"), ViewState.Item("CompRoleID"))
                        If dt.Rows.Count > 0 Then
                            lblGroup.Text = String.Format("{0}-{1}", ht(strKey).ToString(), dt.Rows(0).Item(0).ToString())
                            Bsp.Utility.SetSelectedIndex(ddlCompRoleName, ht("SelectedCompRoleID").ToString())
                        End If
                    End Using
                Case "SelectedFunID"
                    ViewState.Item("FunID") = ht(strKey).ToString()
                    ViewState.Item("SysID") = ht("SelectedSysID").ToString()
                    'Using dt As DataTable = objSC.GetFunInfo(ht(strKey).ToString(), "FunName")
                    Using dt As DataTable = objSC.GetFunInfo_0500(ht(strKey).ToString(), ViewState.Item("SysID"), "FunName")
                        If dt.Rows.Count > 0 Then
                            lblFun.Text = String.Format("{0}-{1}", ht(strKey).ToString(), dt.Rows(0).Item(0).ToString())
                        End If
                    End Using
            End Select
        Next

        LoadData(Bsp.Utility.IsStringNull(ViewState.Item("GroupID")), Bsp.Utility.IsStringNull(ViewState.Item("FunID")), Bsp.Utility.IsStringNull(ViewState.Item("SysID")), Bsp.Utility.IsStringNull(ViewState.Item("CompRoleID")))
    End Sub

    Private Sub LoadData(ByVal GroupID As String, ByVal sFunID As String, ByVal SysID As String, ByVal CompRoleID As String)
        Dim objSC As New SC
        Dim bsRight As New beSC_Right.Service()
        Dim bsGroupFun As New beSC_GroupFun.Service()
        Dim ht As New Hashtable
        Dim strGroupName As String

        If UserProfile.IsSysAdmin = True Then
            ddlCompRoleName.Visible = True
            Bsp.Utility.FillCompany(ddlCompRoleName)
            'ddlCompRoleName.Items.Insert(0, "ALL-全金控")
            Bsp.Utility.SetSelectedIndex(ddlCompRoleName, CompRoleID)
            'ddlCompRoleName.SelectedIndex = 0
            lblCompRoleName.Visible = False
        Else
            '非系統管理者
            ddlCompRoleName.Visible = False
            lblCompRoleName.Text = objSC.GetCompName(UserProfile.SelectCompRoleID).Rows(0).Item("CompName").ToString()
            lblCompRoleName.Visible = True
        End If
        lblGroup.Text = GroupID
        strGroupName = Bsp.Utility.subGetGroupName(CompRoleID, GroupID)
        lblGroupName.Text = strGroupName
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
                'Using dt As DataTable = objSC.GetGroupFunInfo(GroupID, sFunID, CompRoleID, "RightID")
                Using dt As DataTable = objSC.GetGroupFunInfo(GroupID, sFunID, "", "And CompRoleID = " & Bsp.Utility.Quote(ddlCompRoleName.SelectedValue)) '20150720 Beatrice modify
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

        '20150309 Beatrice modify
        Dim strWhere As String = " "
        strWhere &= "Where SysID = " & Bsp.Utility.Quote(SysID)
        strWhere &= "And CompRoleID = " & Bsp.Utility.Quote(CompRoleID)
        strWhere &= "And GroupID = " & Bsp.Utility.Quote(GroupID)
        strWhere &= "And FunID = " & Bsp.Utility.Quote(sFunID)
        strWhere &= "Order By LastChgDate Desc"
        Using dt As DataTable = bsGroupFun.QuerybyWhere(strWhere).Tables(0)
            If dt.Rows.Count > 0 Then
                '20150312 Beatrice modify
                Dim CompName As String = objSC.GetSC_CompName(dt.Rows(0).Item("LastChgComp").ToString())
                lblLastChgComp.Text = dt.Rows(0).Item("LastChgComp").ToString() + IIf(CompName <> "", "-" + CompName, "")
                Dim UserName As String = objSC.GetSC_UserName(dt.Rows(0).Item("LastChgComp").ToString(), dt.Rows(0).Item("LastChgID").ToString())
                lblLastChgID.Text = dt.Rows(0).Item("LastChgID").ToString() + IIf(UserName <> "", "-" + UserName, "")
                '20150312 Beatrice modify End
                lblLastChgDate.Text = Convert.ToDateTime(dt.Rows(0).Item("LastChgDate")).ToString("yyyy/MM/dd HH:mm:ss")
            End If
        End Using
        '20150309 Beatrice modify End
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

    Private Sub GetData(ByVal SysID As String, ByVal CompRoleID As String, ByVal GroupID As String, ByVal FunID As String)
        Dim objSC As New SC
        Dim bsGF As New beSC_GroupFun.Service()
        Dim beGF As New beSC_GroupFun.Row()

        beGF.SysID.Value = SysID
        beGF.CompRoleID.Value = CompRoleID
        beGF.GroupID.Value = GroupID
        Try
            Using dt As DataTable = bsGF.QueryByKey(beGF).Tables(0)
                If dt.Rows.Count <= 0 Then Exit Sub
                beGF = New beSC_GroupFun.Row(dt.Rows(0))
                lblSysName.Text = beGF.SysID.Value
                Bsp.Utility.SetSelectedIndex(ddlCompRoleName, beGF.CompRoleID.Value)
                lblGroup.Text = beGF.GroupID.Value
                lblFun.Text = beGF.FunID.Value
            End Using
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & "subGetData", ex)
        End Try
    End Sub

    Private Function SaveData() As Boolean
        Dim beGroupFun() As beSC_GroupFun.Row = Nothing

        For Each ctl As Control In Me.Page.Form.Controls
            If TypeOf ctl Is CheckBox Then
                If Left(ctl.ID, 8) = "chkRight" Then
                    If CType(ctl, CheckBox).Enabled AndAlso CType(ctl, CheckBox).Checked Then
                        Dim beGF As New beSC_GroupFun.Row()
                        beGF.SysID.Value = Bsp.Utility.IsStringNull(ViewState.Item("SysID"))
                        beGF.CompRoleID.Value = Bsp.Utility.IsStringNull(ViewState.Item("CompRoleID"))
                        beGF.GroupID.Value = Bsp.Utility.IsStringNull(ViewState.Item("GroupID"))
                        beGF.FunID.Value = Bsp.Utility.IsStringNull(ViewState.Item("FunID"))
                        beGF.RightID.Value = Right(ctl.ID, 1)
                        beGF.CreateDate.Value = Now
                        beGF.LastChgDate.Value = Now
                        beGF.LastChgID.Value = UserProfile.ActUserID
                        beGF.LastChgComp.Value = UserProfile.ActCompID '20150312 Beatrice modify

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
End Class
