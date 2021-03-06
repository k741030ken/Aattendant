'****************************************************
'功能說明：群組功能維護－by群組
'建立人員：Ann
'建立日期：2014/09/01
'****************************************************
Imports System.Data

Partial Class SC_SC0500
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then
            Dim objSC As New SC
            Dim strSysID As String
            strSysID = Bsp.Utility.subGetSysID(UserProfile.LoginSysID)
            Dim arySysID() As String = Split(strSysID, "-")
            lblSysName.Text = strSysID
            ddlCompRoleName.Visible = False

            If UserProfile.SelectCompRoleID = "ALL" Then
                ddlCompRoleName.Visible = True
                Bsp.Utility.FillCompany(ddlCompRoleName)
                ddlCompRoleName.Items.Insert(0, New ListItem("全金控", "0")) '20150306 Beatrice modify
                lblCompRoleName.Visible = False
            Else
                '系統管理者
                If UserProfile.IsSysAdmin = True Then
                    '系統管理者選擇全金控
                    If UserProfile.SelectCompRoleID = "ALL" Then
                        ddlCompRoleName.Visible = True
                        Bsp.Utility.FillCompany(ddlCompRoleName)
                        ddlCompRoleName.Items.Insert(0, New ListItem("全金控", "0")) '20150306 Beatrice modify
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

            If Not IsPostBack Then
                SetDropDownData()
            End If
        End If
        '設定員工PopSelect 視窗
        'ucSelecEmp.QuerySQL = "Select UserID, UserName From SC_User Where BanMark = '0' And WorkTypeID not in ('', 'ZZZZZZ') Order by UserID"
        'ucSelecEmp.Fields = New FieldState() { _
        '        New FieldState("UserID", "員工編號", True, True), _
        '        New FieldState("UserName", "員工姓名", True, True)}

    End Sub

    Protected Overrides Sub BaseOnPageTransfer(ByVal ti As TransferInfo)
        If Not IsPostBack() Then
            Dim ht As Hashtable = Bsp.Utility.getHashTableFromParam(ti.Args)

            '20150304 Beatrice modify
            If ht.ContainsKey("ddlCompRoleName") Then
                Bsp.Utility.SetSelectedIndex(ddlCompRoleName, ht("ddlCompRoleName").ToString())

                If ht("ddlCompRoleName").ToString() <> "" Then
                    If ht("ddlCompRoleName").ToString() = "0" Then
                        subGetAllGroupID()
                        Bsp.Utility.SetSelectedIndex(ddlGroup, ht("ddlGroup").ToString())
                    Else
                        SetDropDownData()
                    End If
                End If
            End If
            If ht.ContainsKey("ddlFun") Then
                Bsp.Utility.SetSelectedIndex(ddlFun, ht("ddlFun").ToString())
            End If
            '20150304 Beatrice modify End

            If ht.ContainsKey("ddlGroup") Then
                Bsp.Utility.SetSelectedIndex(ddlGroup, ht("ddlGroup").ToString())
            End If
            If ht.ContainsKey("SelectedGroupID") Then
                ViewState.Item("SelectedGroupID") = ht("SelectedGroupID").ToString()

                Dim item As ListItem = ddlGroup.Items.FindByValue(ht("SelectedGroupID").ToString())
                If item IsNot Nothing Then
                    lblGroup.Text = item.Text
                End If
            End If
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
            Case "btnAdd"       '新增
                DoAdd()
            Case "btnUpdate"    '修改
                DoUpdate()
            Case "btnQuery"     '查詢
                ViewState.Item("DoQuery") = "Y"
                DoQuery()
            Case "btnDelete"    '刪除
                DoDelete()
            Case "btnCopy"      '權限複製 20150720 Beatrice modify
                DoRightCopy()
            Case Else
                DoOtherAction()   '其他功能動作
        End Select
    End Sub

    Private Sub DoAdd()
        Dim btnA As New ButtonState(ButtonState.emButtonType.Add)
        Dim btnU As New ButtonState(ButtonState.emButtonType.Update)
        Dim btnX As New ButtonState(ButtonState.emButtonType.Exit)

        btnA.Caption = "存檔繼續"
        btnU.Caption = "存檔返回"
        btnX.Caption = "返回"

        Me.TransferFramePage("~/SC/SC0501.aspx", New ButtonState() {btnA, btnU, btnX}, _
                             Bsp.Utility.FormatToParam(ddlCompRoleName), _
                             Bsp.Utility.FormatToParam(ddlGroup), _
                             Bsp.Utility.FormatToParam(ddlFun), _
                             "SelectedGroupID=" & Bsp.Utility.IsStringNull(ViewState.Item("SelectedGroupID")), _
                             "PageNo=" & pcMain.PageNo.ToString(), _
                             "DoQuery=" & Bsp.Utility.IsStringNull(ViewState.Item("DoQuery")))
    End Sub

    Private Sub DoUpdate()
        Dim btnU As New ButtonState(ButtonState.emButtonType.Update)
        Dim btnX As New ButtonState(ButtonState.emButtonType.Exit)

        btnU.Caption = "存檔返回"
        btnX.Caption = "返回"

        Me.TransferFramePage("~/SC/SC0502.aspx", New ButtonState() {btnU, btnX}, _
                             Bsp.Utility.FormatToParam(ddlCompRoleName), _
                             Bsp.Utility.FormatToParam(ddlGroup), _
                             Bsp.Utility.FormatToParam(ddlFun), _
                             "SelectedGroupID=" & gvMain.DataKeys(selectedRow(gvMain))("GroupID").ToString(), _
                             "SelectedFunID=" & gvMain.DataKeys(selectedRow(gvMain))("FunID").ToString(), _
                             "SelectedSysID=" & gvMain.DataKeys(selectedRow(gvMain))("SysID").ToString(), _
                             "SelectedCompRoleID=" & gvMain.DataKeys(selectedRow(gvMain))("CompRoleID").ToString(), _
                             "PageNo=" & pcMain.PageNo.ToString(), _
                             "DoQuery=" & Bsp.Utility.IsStringNull(ViewState.Item("DoQuery")))
        '"SelectedGroupID=" & Bsp.Utility.IsStringNull(ViewState.Item("SelectedGroupID")), _
        '"SelectedGroupID=" & gvMain.DataKeys(selectedRow(gvMain))("GroupID").ToString(), _
        '"SelectedCompRoleID=" & gvMain.DataKeys(selectedRow(gvMain))("CompRoleID").ToString(), _
    End Sub

    Private Sub DoRightCopy()
        Dim btnU As New ButtonState(ButtonState.emButtonType.Update)
        Dim btnX As New ButtonState(ButtonState.emButtonType.Exit)

        btnU.Caption = "存檔返回"
        btnX.Caption = "返回"

        Me.TransferFramePage("~/SC/SC0505.aspx", New ButtonState() {btnU, btnX}, _
                             Bsp.Utility.FormatToParam(ddlCompRoleName), _
                             Bsp.Utility.FormatToParam(ddlGroup), _
                             Bsp.Utility.FormatToParam(ddlFun), _
                             "PageNo=" & pcMain.PageNo.ToString(), _
                             "DoQuery=" & Bsp.Utility.IsStringNull(ViewState.Item("DoQuery")))
        
        '"SelectedGroupID=" & Bsp.Utility.IsStringNull(ViewState.Item("SelectedGroupID")), _
        '                     "SelectedFunID=" & gvMain.DataKeys(selectedRow(gvMain))("FunID").ToString(), _
        '                     "SelectedSysID=" & gvMain.DataKeys(selectedRow(gvMain))("SysID").ToString(), _
        '                     "SelectedCompRoleID=" & gvMain.DataKeys(selectedRow(gvMain))("CompRoleID").ToString(), _
    End Sub

    Private Sub DoQuery()
        If ddlGroup.SelectedIndex < 0 Then Return
        Dim objSC As New SC()

        'If ddlGroup.SelectedItem.Value.Equals("Emp") Then
        '    Bsp.Utility.RunClientScript(Me, "funCallSelectEmp();")
        '    Return
        'End If

        ViewState.Item("SelectedSysID") = UserProfile.LoginSysID
        ViewState.Item("SelectedGroupID") = ddlGroup.SelectedValue
        ViewState.Item("SelectedCompRoleID") = UserProfile.SelectCompRoleID
        'QueryData(ddlGroup.SelectedItem.Value)
        'QueryData(ddlGroup.SelectedItem.Value, UserProfile.SelectCompRoleID, UserProfile.LoginSysID) '20140903 Ann add

        '20150304 Beatrice modify
        Dim strCompRoleID As String = ddlCompRoleName.SelectedValue
        If strCompRoleID = "" Then
            strCompRoleID = UserProfile.SelectCompRoleID
        End If

        If strCompRoleID = "0" Then
            If ddlGroup.SelectedValue = "" Then
                Bsp.Utility.ShowFormatMessage(Me, "W_00035", "群組")
                ddlGroup.Focus()
                Return
            End If
            pcMain.DataTable = objSC.GetGroupFun_0500(ddlGroup.SelectedValue.Split("-")(1), ddlFun.SelectedValue, ddlGroup.SelectedValue.Split("-")(0))
        Else
            pcMain.DataTable = objSC.GetGroupFun_0500(ddlGroup.SelectedValue, ddlFun.SelectedValue, strCompRoleID)
        End If
        '20150304 Beatrice modify End
    End Sub

    Private Sub DoDelete()
        Dim objSC As New SC()

        Try
            objSC.DeleteGroupFun(ViewState.Item("SelectedGroupID").ToString(), gvMain.DataKeys(selectedRow(gvMain))("FunID").ToString(), gvMain.DataKeys(selectedRow(gvMain))("SysID").ToString(), gvMain.DataKeys(selectedRow(gvMain))("CompRoleID").ToString())
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & ".DoDelete", ex)
        End Try
        gvMain.DataBind()
        'QueryData(ddlGroup.SelectedItem.Value, UserProfile.SelectCompRoleID, UserProfile.LoginSysID) '20150112 Ann add
        DoQuery() '20150401 Beatrice modify
    End Sub

    Private Sub DoOtherAction()

    End Sub

    Protected Sub btnChangeToFun_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnChangeToFun.Click
        Me.TransferFramePage("~/SC/SC0503.aspx", Nothing)
    End Sub

    Private Sub SetDropDownData()
        '20150302 Beatrice modify
        Dim strCompRoleID As String = ddlCompRoleName.SelectedValue
        If strCompRoleID = "" Then
            strCompRoleID = UserProfile.SelectCompRoleID
        End If

        If strCompRoleID = "0" Then
            subGetAllGroupID()
        Else
            Bsp.Utility.FillGroup_0501(ddlGroup, strCompRoleID)
            ddlGroup.Items.Insert(0, New ListItem("---請選擇---", ""))
        End If
        'ddlGroup.Items.Add(New ListItem("個人群組...", "Emp"))

        '20150302 Beatrice modify
        Bsp.Utility.FillFun(ddlFun, Bsp.Enums.SelectFunType.FunHasRight, UserProfile.LoginSysID, strCompRoleID)
        ddlFun.Items.Insert(0, New ListItem("---請選擇---", ""))
    End Sub
    ''' <summary>
    ''' 員工pop視窗關閉選擇後處理
    ''' </summary>
    ''' <param name="returnValue"></param>
    ''' <remarks></remarks>
    Public Overrides Sub DoModalReturn(ByVal returnValue As String)
        Dim aryData() As String = returnValue.Split(":")

        Select Case aryData(0)
            Case "ucSelecEmp"
                Dim aryValue() As String = Split(aryData(1), "|$|")
                If SaveData(aryValue(0), aryValue(1)) Then
                    SetDropDownData()
                    Bsp.Utility.SetSelectedIndex(ddlGroup, aryValue(0))
                    QueryData(aryValue(0), aryValue(1), aryValue(2))
                End If

        End Select

    End Sub
    ''' <summary>
    '''  Save 個人群組 資料
    ''' </summary>
    ''' <param name="GroupID">員工ID</param>
    ''' <param name="GroupName">員工姓名</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function SaveData(ByVal GroupID As String, ByVal GroupName As String) As Boolean
        'Private Function SaveData(ByVal GroupID As String, ByVal CompRoleID As String, ByVal SysID As String, ByVal GroupName As String) As Boolean
        Dim beGroup As New beSC_Group.Row()
        Dim bsGroup As New beSC_Group.Service()
        Dim objSC As New SC

        beGroup.GroupID.Value = GroupID
        beGroup.GroupName.Value = GroupName
        beGroup.LastChgDate.Value = Now
        beGroup.CreateDate.Value = Now
        beGroup.LastChgID.Value = UserProfile.ActUserID

        '判斷資料是否存在
        If bsGroup.IsDataExists(beGroup) Then
            'QueryData(GroupID)    '20140903. Ann add
            QueryData(GroupID, UserProfile.SelectCompRoleID, UserProfile.LoginSysID)    '20140903. Ann add
            ddlGroup.SelectedValue = GroupID
            Return False
        End If

        Try
            objSC.AddGroup(beGroup)
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & ".SaveData", ex)
            Return False
        End Try

        Return True

    End Function
    'Private Sub QueryData(ByVal GroupID As String)
    Private Sub QueryData(ByVal GroupID As String, ByVal CompRoleID As String, ByVal SysID As String)  '20140903 Ann add
        If ddlGroup.SelectedIndex < 0 Then Return
        Dim objSC As New SC
        Try
            pcMain.DataTable = objSC.GetGroupFun(Bsp.Enums.GroupFunType.Group, GroupID, CompRoleID)
            'Using dt As DataTable = objSC.GetGroupInfo_0500(GroupID, "GroupID + '-' + GroupName FullName")

            Using dt As DataTable = objSC.GetGroupInfo_0500(GroupID, CompRoleID, SysID, "GroupID + '-' + GroupName FullName")  '20140903 Ann add
                If dt.Rows.Count > 0 Then
                    lblGroup.Text = dt.Rows(0).Item("FullName").ToString()
                End If
            End Using
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & ".QueryData", ex)
        End Try
    End Sub

    '20150304 Beatrice Add
    Protected Sub ddlCompRoleName_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCompRoleName.SelectedIndexChanged
        Dim strCompRoleID As String = ddlCompRoleName.SelectedValue
        If strCompRoleID = "" Then
            strCompRoleID = UserProfile.SelectCompRoleID
        End If

        If strCompRoleID = "0" Then
            subGetAllGroupID()
        Else
            Bsp.Utility.FillGroup_0501(ddlGroup, strCompRoleID)
            ddlGroup.Items.Insert(0, New ListItem("---請選擇---", ""))
        End If

        Bsp.Utility.FillFun(ddlFun, Bsp.Enums.SelectFunType.FunHasRight, UserProfile.LoginSysID, strCompRoleID)
        ddlFun.Items.Insert(0, New ListItem("---請選擇---", ""))
    End Sub

    Private Sub subGetAllGroupID()
        Dim objSC As New SC
        Try
            ddlGroup.Items.Clear()
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

                ddlGroup.Items.Insert(i, New ListItem(strText, strValue))
            Next
            ddlGroup.Items.Insert(0, New ListItem("---請選擇---", ""))
        Catch ex As Exception
            ddlGroup.Items.Insert(0, New ListItem("---請選擇---", ""))
        End Try
    End Sub
    '20150304 Beatrice Add End
End Class
