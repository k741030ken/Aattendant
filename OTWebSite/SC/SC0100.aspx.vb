'****************************************************
'功能說明：使用者資料維護
'建立人員：Ann
'建立日期：2014/10/30
'****************************************************
Imports System.Data
Imports System.Data.Common

Partial Class SC_SC0100
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then
            Dim objSC As New SC
            Dim strSysID As String
            strSysID = Bsp.Utility.subGetSysID(UserProfile.LoginSysID)
            Dim arySysID() As String = Split(strSysID, "-")
            lblSysName.Text = strSysID

            ddlCompRoleName.Visible = False
            ucSelectEmpID.ShowCompRole = False
            ucSelectEmpID.ConnType = "SC"

            If UserProfile.SelectCompRoleID = "ALL" Then
                ddlCompRoleName.Visible = True
                Bsp.Utility.FillCompany(ddlCompRoleName)
                lblCompRoleName.Visible = False
            Else
                '系統管理者
                If UserProfile.IsSysAdmin = True Then
                    '系統管理者選擇全金控
                    If UserProfile.SelectCompRoleID = "ALL" Then
                        ddlCompRoleName.Visible = True
                        Bsp.Utility.FillCompany(ddlCompRoleName)
                        lblCompRoleName.Visible = False
                    Else
                        ddlCompRoleName.Visible = False
                        lblCompRoleName.Text = objSC.GetCompName(UserProfile.SelectCompRoleID).Rows(0).Item("CompName").ToString()
                        lblCompRoleName.Visible = True
                    End If
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
        If Not IsPostBack() Then
            Dim ht As Hashtable = Bsp.Utility.getHashTableFromParam(ti.Args)

            'If ht.ContainsKey("ddlGroup") Then
            '    Bsp.Utility.SetSelectedIndex(ddlGroup, ht("ddlGroup").ToString())
            'End If
            'If ht.ContainsKey("SelectedGroupID") Then
            '    ViewState.Item("SelectedGroupID") = ht("SelectedGroupID").ToString()
            '    Dim item As ListItem = ddlGroup.Items.FindByValue(ht("SelectedGroupID").ToString())

            '    If item IsNot Nothing Then
            '        lblGroup.Text = item.Text
            '    End If
            'End If
            'If ht.ContainsKey("PageNo") Then
            '    pcMain.PageNo = Convert.ToInt32(ht("PageNo"))
            '    DoQuery()
            'End If
            If ht.ContainsKey("PageNo") Then pcMain.PageNo = Convert.ToInt32(ht("PageNo"))
            If ht.ContainsKey("DoQuery") Then
                If ht("DoQuery").ToString() = "Y" Then
                    ViewState.Item("DoQuery") = "Y"
                    DoQuery()
                End If
            End If
        End If
    End Sub

    Public Overrides Sub DoAction(ByVal Param As String)
        Select Case Param
            Case "btnQuery"     '查詢
                ViewState.Item("DoQuery") = "N"
                DoQuery()
             Case "btnUpdate"    '修改
                DoUpdate()
            Case Else
                'DoOtherAction()   '其他功能動作
        End Select
    End Sub

    Private Sub DoUpdate()
        Dim btnU As New ButtonState(ButtonState.emButtonType.Update)
        Dim btnX As New ButtonState(ButtonState.emButtonType.Exit)

        btnU.Caption = "存檔返回"
        btnX.Caption = "返回"

        subGetSysIDs(UserProfile.ActUserID)
        Dim strCompRoleID As String
        If ddlCompRoleName.Visible = True Then
            strCompRoleID = ddlCompRoleName.SelectedValue
        Else
            strCompRoleID = UserProfile.SelectCompRoleID
        End If
        Me.TransferFramePage("~/SC/SC0102.aspx", New ButtonState() {btnU, btnX}, _
                           Bsp.Utility.FormatToParam(txtUserID), _
                           Bsp.Utility.FormatToParam(txtUserName), _
                            "CompID=" & strCompRoleID, _
                            "UserID=" & gvMain.DataKeys(selectedRow(gvMain))("UserID").ToString(), _
                             "PageNo=" & pcMain.PageNo.ToString(), _
                             "DoQuery=" & Bsp.Utility.IsStringNull(ViewState.Item("DoQuery")))
    End Sub

    Private Sub DoQuery()
        Dim objSC As New SC()
        Dim btnU As New ButtonState(ButtonState.emButtonType.Update)
        Dim btnX As New ButtonState(ButtonState.emButtonType.Exit)

        btnU.Caption = "存檔返回"
        btnX.Caption = "返回"

        Try
            subGetSysIDs(UserProfile.ActUserID)
            Dim strCompRoleID As String

            If ddlCompRoleName.Visible = True Then
                strCompRoleID = ddlCompRoleName.SelectedValue
            Else
                strCompRoleID = UserProfile.SelectCompRoleID
            End If

            Dim bsUser As New beSC_User.Service()
            Dim beUser As New beSC_User.Row()

            pcMain.DataTable = objSC.QuerySCUser( _
               "CompID=" & strCompRoleID, _
               "UserID=" & txtUserID.Text.Trim().ToUpper(), _
               "UserName=" & txtUserName.Text.Trim().ToUpper())
            
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & ".DoQuery", ex)
        End Try

    End Sub

    Private Sub DoDelete()
        Dim objSC As New SC()

        Try
            'objSC.DeleteGroupFun(ViewState.Item("SelectedGroupID").ToString(), gvMain.DataKeys(selectedRow(gvMain))("FunID").ToString())
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & ".DoDelete", ex)
        End Try
        gvMain.DataBind()
    End Sub

    Private Sub subGetSysIDs(ByVal ActUserID As String)
        Dim strSysIDs As String
        strSysIDs = Bsp.Utility.subGetSysIDs(UserProfile.ActUserID)
        lblSysNameD.Text = strSysIDs
    End Sub

    Private Sub SetDropDownData()
        'Bsp.Utility.FillGroup(ddlGroup)
        'ddlGroup.Items.Add(New ListItem("個人群組...", "Emp"))
    End Sub

    Public Overrides Sub DoModalReturn(ByVal returnValue As String)
        Dim strSql As String = ""

        If returnValue <> "" Then
            Dim aryData() As String = returnValue.Split(":")
            Select Case aryData(0)
                '員工編號
                Case "ucSelectEmpID"
                    Dim aryValue() As String = Split(aryData(1), "|$|")
                    txtUserID.Text = aryValue(1)
                    'txtUserName.Text = aryValue(2) '20150727 Beatrice del
            End Select
        End If
    End Sub

End Class
