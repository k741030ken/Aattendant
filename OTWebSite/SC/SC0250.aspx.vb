'****************************************************
'功能說明：使用者群組維護
'建立人員：Chung
'建立日期：2011/05/19
'****************************************************
Imports System.Data

Partial Class SC_SC0250
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Public Overrides Sub DoAction(ByVal Param As String)
        Select Case Param
            Case "btnAdd"       '新增
                DoAdd()
            Case "btnUpdate"    '修改
                DoUpdate()
            Case "btnQuery"     '查詢
                DoQuery()
            Case "btnDelete"    '刪除
                DoDelete()
            Case Else
                DoOtherAction()   '其他功能動作
        End Select
    End Sub

    Protected Overrides Sub BaseOnPageTransfer(ByVal ti As TransferInfo)
        If ti.Args.Length > 0 Then
            Dim ht As Hashtable = Bsp.Utility.getHashTableFromParam(ti.Args)

            For Each strKey As String In ht.Keys
                Select Case strKey
                    Case "ucDeptID"
                        ucSelectedUser.setDeptID(ht(strKey).ToString())
                    Case "ucUserID"
                        ucSelectedUser.setUserID(ht(strKey).ToString())
                    Case "SelectedUserID"
                        GetData(ht(strKey).ToString())
                End Select
            Next
        End If
    End Sub

    Private Sub DoAdd()

    End Sub

    Private Sub DoUpdate()
        If lblUserID.Text = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00000")
            Return
        End If
        Dim btnU As New ButtonState(ButtonState.emButtonType.Update)
        Dim btnX As New ButtonState(ButtonState.emButtonType.Exit)

        btnU.Caption = "存檔返回"
        btnX.Caption = "返回"

        Me.TransferFramePage("~/SC/SC0251.aspx", New ButtonState() {btnU, btnX}, _
                             "ucDeptID=" & ucSelectedUser.SelectDeptID, _
                             "ucUserID=" & ucSelectedUser.SelectedUserID, _
                             "SelectedUserID=" & lblUserID.Text.ToUpper())
    End Sub

    Private Sub DoQuery()
        If ucSelectedUser.SelectedUserID = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00000")
            Return
        End If
        GetData(ucSelectedUser.SelectedUserID)
    End Sub

    Private Sub GetData(ByVal UserID As String)
        Dim objSC As New SC()

        Try
            '取得使用者基本資料
            Using dt As DataTable = objSC.GetUserInfo(UserID, "UserID, UserName, dbo.funGetAOrgDefine('2', DeptID) DeptName")
                If dt.Rows.Count > 0 Then
                    lblUserID.Text = dt.Rows(0).Item("UserID").ToString()
                    lblUserName.Text = dt.Rows(0).Item("UserName").ToString()
                    lblDeptID.Text = dt.Rows(0).Item("DeptName").ToString()
                End If
            End Using

            '取得群組資訊
            lstGroup.Items.Clear()
            Using dt As DataTable = objSC.GetGroupInfo("", "GroupID, GroupID + '-' + GroupName GroupName, GroupType", _
                                                       "And Exists (Select GroupID From SC_UserGroup Where GroupID = SC_Group.GroupID And UserID = " & Bsp.Utility.Quote(UserID) & ")")
                If dt.Rows.Count = 0 Then
                    lstGroup.Items.Add(New ListItem("(尚無群組資料)", ""))
                Else
                    lstGroup.DataSource = dt
                    lstGroup.DataTextField = "GroupName"
                    lstGroup.DataValueField = "GroupID"
                    lstGroup.DataBind()
                End If
            End Using
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & ".GetDate", ex)
        End Try
    End Sub

    Private Sub DoDelete()

    End Sub

    Private Sub DoOtherAction()

    End Sub

    Protected Sub btnChangeToGroup_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnChangeToGroup.Click
        Me.TransferFramePage("~/SC/SC0252.aspx", Nothing)
    End Sub
End Class
