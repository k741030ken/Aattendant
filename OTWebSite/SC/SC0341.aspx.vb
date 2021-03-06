'****************************************************
'功能說明：查詢部門設定-新增
'建立人員：Chung
'建立日期：2013/08/07
'****************************************************
Imports System.Data

Partial Class SC_SC0341
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            ucOrganID.LoadData()
            Bsp.Utility.FillGroup(ddlGroupID)
            ddlGroupID.Items.Insert(0, New ListItem("--未選取--", ""))
            ucUserID.QuerySQL = "Select UserID, UserName From SC_User Where BanMark = '0' Order by UserID"
            ucUserID.Fields = New FieldState() { _
                    New FieldState("UserID", "員工編號", True, True), _
                    New FieldState("UserName", "員工姓名", True, False)}
            'ucUserID.ReturnColumnIndex = "0"
            LoadDeptID()
        End If
    End Sub

    Private Sub ClearScreen()
        rdoOrganID.Checked = True
        ucOrganID.Enabled = True
        txtUserID.Enabled = False
        ucUserID.Enabled = False
        ddlGroupID.Enabled = False
        chkQueryOrganID.ClearSelection()
    End Sub

    Public Overrides Sub DoAction(ByVal Param As String)
        Select Case Param
            Case "btnAdd"
                If funCheckData() Then
                    If SaveData() Then
                        ClearScreen()
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
        Dim beDeptQuerySetting As New beSC_DeptQuerySetting.Row()
        Dim objSC As New SC
        Dim strOrganID As String = ""

        For intLoop As Integer = 0 To chkQueryOrganID.Items.Count - 1
            If chkQueryOrganID.Items(intLoop).Selected Then
                strOrganID &= chkQueryOrganID.Items(intLoop).Value & ", "
            End If
        Next
        strOrganID = strOrganID.Substring(0, strOrganID.Length - 2)

        With beDeptQuerySetting
            .OrganID.Value = ""
            .UserID.Value = ""
            .GroupID.Value = ""
            If rdoOrganID.Checked Then
                If ucOrganID.SelectedOrganID = "" Then
                    .OrganID.Value = ucOrganID.SelectedDeptID
                Else
                    .OrganID.Value = ucOrganID.SelectedOrganID
                End If
            ElseIf rdoUserID.Checked Then
                .UserID.Value = txtUserID.Text
            Else
                .GroupID.Value = ddlGroupID.SelectedValue
            End If
            .QueryOrganID.Value = strOrganID
        End With
        If objSC.IsDeptQuerySettingExists(beDeptQuerySetting) Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00010", "")
            Return False
        End If
        Try
            objSC.addDeptQuerySetting(beDeptQuerySetting)
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & ".SaveData", ex)
            Return False
        End Try
        Return True
    End Function

    Private Function funCheckData() As Boolean
        Dim strValue As String

        If rdoUserID.Checked Then
            strValue = txtUserID.Text.ToString()
            If strValue = "" Then
                Bsp.Utility.ShowFormatMessage(Me, "W_00030", "員工")
                txtUserID.Focus()
                Return False
            Else
                If Bsp.Utility.getStringLength(strValue) > txtUserID.MaxLength Then
                    Bsp.Utility.ShowFormatMessage(Me, "W_00040", "員工", txtUserID.MaxLength.ToString())
                    txtUserID.Focus()
                    Return False
                End If
                txtUserID.Text = strValue
            End If
        ElseIf rdoGroupID.Checked Then
            If ddlGroupID.SelectedIndex = 0 Then
                Bsp.Utility.ShowFormatMessage(Me, "W_00035", "群組")
                Return False
            End If
        End If
        If chkQueryOrganID.SelectedValue = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00035", "可查詢部門")
            Return False
        End If

        Return True
    End Function

    Protected Sub rdoGroupID_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdoGroupID.CheckedChanged, rdoOrganID.CheckedChanged, rdoUserID.CheckedChanged
        ucOrganID.Enabled = False
        txtUserID.Enabled = False
        ucUserID.Enabled = False
        ddlGroupID.Enabled = False

        Select Case CType(sender, RadioButton).ID
            Case "rdoOrganID"
                ucOrganID.Enabled = True
            Case "rdoUserID"
                txtUserID.Enabled = True
                ucUserID.Enabled = True
            Case "rdoGroupID"
                ddlGroupID.Enabled = True
        End Select
    End Sub

    Private Sub LoadDeptID()
        Dim objSC As New SC

        Try
            Using dt As DataTable = objSC.GetOrganForQuerySetting()
                With chkQueryOrganID
                    .DataTextField = "OrganFullName"
                    .DataValueField = "OrganID"
                    .DataSource = dt
                    .DataBind()
                End With
            End Using
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & ".LoadDeptID", ex)
        End Try
    End Sub

    Protected Sub btnSelectAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelectAll.Click, btnUnSelectAll.Click
        Dim selected As Boolean = IIf(CType(sender, LinkButton).ID = "btnSelectAll", True, False)
        For intLoop As Integer = 0 To chkQueryOrganID.Items.Count - 1
            chkQueryOrganID.Items(intLoop).Selected = selected
        Next
    End Sub
End Class
