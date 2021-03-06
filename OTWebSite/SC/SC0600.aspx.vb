'****************************************************
'功能說明：使用者權限維護
'1.	使用者權限維護，使用者的【系統 -授權公司-群組-功能(按鍵) 關係】
'2.	依授權公司的權限，admin系統管理者可授權各子公司HR各自授權維護
'
'建立人員：Ann
'建立日期：2014/10/24
'****************************************************
Imports System.Data
Imports System.Data.Common

Partial Class SC_SC0600
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then
            Dim objSC As New SC
            Dim strSysID As String
            strSysID = Bsp.Utility.subGetSysID(UserProfile.LoginSysID)
            Dim arySysID() As String = Split(strSysID, "-")
            lblSysName.Text = strSysID
            ddlCompRoleName.Visible = False

            ucSelectEmpID.ConnType = "SC"
            ucSelectEmpID.ShowCompRole = False

            If UserProfile.SelectCompRoleID = "ALL" Then
                ddlCompRoleName.Visible = True
                Bsp.Utility.FillCompany(ddlCompRoleName)
                ddlCompRoleName.Items.Insert(0, New ListItem("全金控", "0"))   '20150112 Ann modify
                lblCompRoleName.Visible = False

                '20150313 Beatrice modify 增加群組代碼選單
                subGetAllGroupID()
            Else
                '系統管理者
                If UserProfile.IsSysAdmin = True Then
                    '系統管理者選擇全金控
                    If UserProfile.SelectCompRoleID = "ALL" Then
                        ddlCompRoleName.Visible = True
                        Bsp.Utility.FillCompany(ddlCompRoleName)
                        lblCompRoleName.Visible = False

                        '20150313 Beatrice modify 增加群組代碼選單
                        Bsp.Utility.FillGroup_0501(ddlGroupID, ddlCompRoleName.SelectedValue)
                        ddlGroupID.Items.Insert(0, New ListItem("---請選擇---", ""))
                    Else
                        ddlCompRoleName.Visible = False
                        lblCompRoleName.Text = objSC.GetCompName(UserProfile.SelectCompRoleID).Rows(0).Item("CompName").ToString()
                        lblCompRoleName.Visible = True

                        '20150313 Beatrice modify 增加群組代碼選單
                        Bsp.Utility.FillGroup(ddlGroupID)
                        ddlGroupID.Items.Insert(0, New ListItem("---請選擇---", ""))
                    End If
                Else
                    '非系統管理者
                    ddlCompRoleName.Visible = False
                    lblCompRoleName.Text = objSC.GetCompName(UserProfile.SelectCompRoleID).Rows(0).Item("CompName").ToString()
                    lblCompRoleName.Visible = True

                    '20150313 Beatrice modify 增加群組代碼選單
                    Bsp.Utility.FillGroup(ddlGroupID)
                    ddlGroupID.Items.Insert(0, New ListItem("---請選擇---", ""))
                End If
            End If

            If Not IsPostBack Then
                SetDropDownData()
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

            '20150313 Beatrice Add
            For Each strKey As String In ht.Keys
                Dim ctl As Control = Me.FindControl(strKey)

                If TypeOf ctl Is TextBox Then
                    CType(ctl, TextBox).Text = ht(strKey).ToString()
                ElseIf TypeOf ctl Is DropDownList Then
                    CType(ctl, DropDownList).SelectedValue = ht(strKey).ToString()

                    If ctl.ID.Equals("ddlCompRoleName") Then
                        If ht(strKey).ToString() <> "" Then
                            If ht(strKey).ToString() = "0" Then
                                subGetAllGroupID()
                                ddlGroupID.SelectedValue = ht("ddlGroupID").ToString()
                            Else
                                Try
                                    Bsp.Utility.FillGroup_0501(ddlGroupID, ht(strKey).ToString())
                                    ddlGroupID.Items.Insert(0, New ListItem("---請選擇---", ""))
                                Catch ex As Exception
                                    ddlGroupID.Items.Insert(0, New ListItem("---請選擇---", ""))
                                End Try
                            End If
                        End If
                    End If

                End If
            Next
            '20150313 Beatrice Add End

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
            Case "btnAdd"       '新增
                DoAdd()
            Case "btnQuery"     '查詢
                ViewState.Item("DoQuery") = "Y"
                DoQuery()
            Case "btnDelete"    '刪除
                DoDelete()
            Case "btnCopy"     '權限複製 20150721 Beatrice modify
                DoPrint()
            Case Else
                DoOtherAction()   '其他功能動作
        End Select
    End Sub

    Private Sub DoAdd()
        Dim btnU As New ButtonState(ButtonState.emButtonType.Update)
        Dim btnX As New ButtonState(ButtonState.emButtonType.Exit)

        btnU.Caption = "存檔返回"
        btnX.Caption = "返回"

        Me.TransferFramePage("~/SC/SC0601.aspx", New ButtonState() {btnU, btnX}, _
                             "PageNo=" & pcMain.PageNo.ToString(), _
                             "DoQuery=" & Bsp.Utility.IsStringNull(ViewState.Item("DoQuery")))
    End Sub
    '權限複製
    Private Sub DoPrint()
        Dim btnU As New ButtonState(ButtonState.emButtonType.Update)
        Dim btnX As New ButtonState(ButtonState.emButtonType.Exit)

        btnU.Caption = "存檔返回"
        btnX.Caption = "返回"

        Me.TransferFramePage("~/SC/SC0602.aspx", New ButtonState() {btnU, btnX}, _
                             "PageNo=" & pcMain.PageNo.ToString(), _
                             "DoQuery=" & Bsp.Utility.IsStringNull(ViewState.Item("DoQuery")))
    End Sub

    Private Sub DoQuery()
        Dim objSC As New SC()
        Dim btnA As New ButtonState(ButtonState.emButtonType.Add)
        Dim btnU As New ButtonState(ButtonState.emButtonType.Update)
        Dim btnD As New ButtonState(ButtonState.emButtonType.Delete)

        Try
            'subGetSysIDs(UserProfile.ActUserID)
            Dim strCompRoleID As String
            If ddlCompRoleName.Visible = True Then
                strCompRoleID = ddlCompRoleName.SelectedValue
            Else
                strCompRoleID = UserProfile.SelectCompRoleID
            End If

            '20150313 Beatrice modify
            If strCompRoleID <> "0" Then
                pcMain.DataTable = objSC.GetUserGroup( _
                    strCompRoleID, _
                    txtEmpID.Text.Trim(), _
                    txtName.Text.Trim(), _
                    ddlGroupID.SelectedValue)
            Else
                If ddlGroupID.SelectedValue = "" Then
                    pcMain.DataTable = objSC.GetUserGroup( _
                        "0", _
                        txtEmpID.Text.Trim(), _
                        txtName.Text.Trim(), _
                        "")
                Else
                    pcMain.DataTable = objSC.GetUserGroup( _
                        ddlGroupID.SelectedValue.Split("-")(0), _
                        txtEmpID.Text.Trim(), _
                        txtName.Text.Trim(), _
                        ddlGroupID.SelectedValue.Split("-")(1))
                End If
            End If
            '20150313 Beatrice modify End

        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & ".DoQuery", ex)
        End Try
    End Sub

    Private Sub DoDelete()
        'Dim objSC As New SC()
        'Try
        '    objSC.DeleteUserGroup(Bsp.Utility.IsStringNull(ViewState.Item("CompID")), Bsp.Utility.IsStringNull(ViewState.Item("UserID")), Bsp.Utility.IsStringNull(ViewState.Item("SysID")), Bsp.Utility.IsStringNull(ViewState.Item("CompRoleID")), ViewState.Item("SelectedGroupID").ToString())
        'Catch ex As Exception
        '    Bsp.Utility.ShowMessage(Me, Me.FunID & ".DoDelete", ex)
        'End Try
        'gvMain.DataBind()

        If selectedRow(gvMain) < 0 Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00000")
        Else
            Dim beSC_UserGroup As New beSC_UserGroup.Row()
            Dim objSC As New SC
            beSC_UserGroup.CompID.Value = gvMain.DataKeys(Me.selectedRow(gvMain))("CompID").ToString()
            beSC_UserGroup.UserID.Value = gvMain.DataKeys(Me.selectedRow(gvMain))("UserID").ToString()
            beSC_UserGroup.SysID.Value = gvMain.DataKeys(Me.selectedRow(gvMain))("SysID").ToString()
            beSC_UserGroup.CompRoleID.Value = gvMain.DataKeys(Me.selectedRow(gvMain))("CompRoleID").ToString()
            beSC_UserGroup.GroupID.Value = gvMain.DataKeys(Me.selectedRow(gvMain))("GroupID").ToString()

            Try
                objSC.DeleteUserGroup(beSC_UserGroup)
            Catch ex As Exception
                Bsp.Utility.ShowMessage(Me, Me.FunID & ".DoDelete", ex)
            End Try
            gvMain.DataBind()

            DoQuery() '20151012 Beatrice Add
        End If
    End Sub

    Private Sub DoOtherAction()

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
                    txtEmpID.Text = aryValue(1)
                    'txtName.Text = aryValue(2) '20150727 Beatrice del
            End Select
        End If
    End Sub

    '20150313 Beatrice Add
    Protected Sub ddlCompRoleName_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlCompRoleName.SelectedIndexChanged
        If ddlCompRoleName.SelectedValue = "0" Then
            subGetAllGroupID()
        Else
            Bsp.Utility.FillGroup_0501(ddlGroupID, ddlCompRoleName.SelectedValue)
            ddlGroupID.Items.Insert(0, New ListItem("---請選擇---", ""))
        End If
    End Sub

    Private Sub subGetAllGroupID()
        Dim objSC As New SC
        Try
            ddlGroupID.Items.Clear()
            Dim dt As DataTable = objSC.GetGroupInfo("", UserProfile.LoginSysID, "", "Order by G.CompRoleID, G.GroupID")
            For i = 0 To dt.Rows.Count - 1
                Dim strText As String = ""
                Dim strValue As String = ""

                If dt.Rows(i).Item("CompRoleID").ToString() = "ALL" Then
                    strText = dt.Rows(i).Item("CompRoleID").ToString() + "-" + "全金控" + "-" + dt.Rows(i).Item("GroupID").ToString() + "-" + dt.Rows(i).Item("GroupName").ToString()
                Else
                    strText = dt.Rows(i).Item("CompRoleID").ToString() + "-" + dt.Rows(i).Item("CompRoleName").ToString() + "-" + dt.Rows(i).Item("GroupID").ToString() + "-" + dt.Rows(i).Item("GroupName").ToString()
                End If
                strValue = dt.Rows(i).Item("CompRoleID").ToString() + "-" + dt.Rows(i).Item("GroupID").ToString()

                ddlGroupID.Items.Insert(i, New ListItem(strText, strValue))
            Next
            ddlGroupID.Items.Insert(0, New ListItem("---請選擇---", ""))
        Catch ex As Exception
            ddlGroupID.Items.Insert(0, New ListItem("---請選擇---", ""))
        End Try
    End Sub
    '20150313 Beatrice Add End

End Class
