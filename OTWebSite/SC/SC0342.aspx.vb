'****************************************************
'功能說明：查詢部門設定-修改
'建立人員：Chung
'建立日期：2013/08/07
'****************************************************
Imports System.Data

Partial Class SC_SC0342
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            LoadDeptID()
        End If
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

    Protected Overrides Sub BaseOnPageTransfer(ByVal ti As TransferInfo)
        If Not IsPostBack() Then
            Dim ht As Hashtable = Bsp.Utility.getHashTableFromParam(ti.Args)
            Dim strOrganID As String = ""
            Dim strUserID As String = ""
            Dim strGroupID As String = ""

            If ht.ContainsKey("OrganID") Then strOrganID = ht("OrganID").ToString()
            If ht.ContainsKey("UserID") Then strUserID = ht("UserID").ToString()
            If ht.ContainsKey("GroupID") Then strGroupID = ht("GroupID").ToString()
            
            GetData(strOrganID, strUserID, strGroupID)
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

    Private Sub GetData(ByVal strOrganID As String, ByVal strUserID As String, ByVal strGroupID As String)
        Dim objSC As New SC
        Dim aryQueryOrganID() As String
        Dim beDeptQuerySetting As beSC_DeptQuerySetting.Row

        Try
            Using dt As DataTable = objSC.getDeptQuerySetting(strOrganID, strUserID, strGroupID)
                beDeptQuerySetting = New beSC_DeptQuerySetting.Row(dt.Rows(0))

                lblOrganID.Text = beDeptQuerySetting.OrganID.Value
                lblUserID.Text = beDeptQuerySetting.UserID.Value
                lblGroupID.Text = beDeptQuerySetting.GroupID.Value

                aryQueryOrganID = beDeptQuerySetting.QueryOrganID.Value.Split(",")
                Dim strQueryOrganID As String

                For intLoop As Integer = 0 To aryQueryOrganID.GetUpperBound(0)
                    strQueryOrganID = aryQueryOrganID(intLoop).Trim()

                    Dim lstItem As ListItem = chkQueryOrganID.Items.FindByValue(strQueryOrganID)
                    If lstItem IsNot Nothing Then
                        lstItem.Selected = True
                    End If
                Next
            End Using
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & ".GetData", ex)
        End Try
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
            .OrganID.Value = lblOrganID.Text
            .UserID.Value = lblUserID.Text
            .GroupID.Value = lblGroupID.Text
            .QueryOrganID.Value = strOrganID
        End With

        If Not objSC.IsDeptQuerySettingExists(beDeptQuerySetting) Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00020", "")
            Return False
        End If
        Try
            objSC.UpdateDeptQuerySetting(beDeptQuerySetting)
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & ".SaveData", ex)
            Return False
        End Try
        Return True
    End Function

    Private Function funCheckData() As Boolean
        If chkQueryOrganID.SelectedValue = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00035", "可查詢部門")
            Return False
        End If

        Return True
    End Function

    Protected Sub btnSelectAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelectAll.Click, btnUnSelectAll.Click
        Dim selected As Boolean = IIf(CType(sender, LinkButton).ID = "btnSelectAll", True, False)
        For intLoop As Integer = 0 To chkQueryOrganID.Items.Count - 1
            chkQueryOrganID.Items(intLoop).Selected = selected
        Next
    End Sub
End Class
