'****************************************************
'功能說明：綜合查詢授權設定-新增
'建立人員：BeatriceCheng
'建立日期：2015.11.24
'****************************************************
Imports System.Data

Partial Class SC_SC1001
    Inherits PageBase


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then
            Dim objSC As New SC
            Dim strCompID As String = ""

            If UserProfile.IsSysAdmin = True Then
                Bsp.Utility.FillCompany(ddlCompRoleID)
                ddlCompRoleID.Items.Insert(0, New ListItem("---請選擇---", ""))
                strCompID = ddlCompRoleID.SelectedValue
                txtCompRoleID.Visible = False
            Else
                strCompID = UserProfile.SelectCompRoleID
                txtCompRoleID.Text = UserProfile.SelectCompRoleName
                ddlCompRoleID.Visible = False
            End If

            Bsp.Utility.FillDDL(ddlGroupID, "HRDB", "SC_Group S", "S.GroupID", "S.GroupName", Bsp.Utility.DisplayType.Full, "", _
                                "And S.GroupID Not In (Select A.GroupID From SC_GroupMutiQry A Where A.SysID = S.SysID And A.CompRoleID = S.CompRoleID And A.GroupID = S.GroupID)" & _
                                "And S.CompRoleID = " & Bsp.Utility.Quote(strCompID))
            ddlGroupID.Items.Insert(0, New ListItem("---請選擇---", ""))


            Using dt As DataTable = Bsp.DB.ExecuteDataSet(CommandType.Text, _
                "Select Code, CodeCName From HRCodeMap Where TabName = 'SC_GroupMutiQry' And FldName = 'QueryID' And NotShowFlag = '0' Order By Code", "eHRMSDB").Tables(0)
                With chkQueryID
                    .Items.Clear()
                    .DataSource = dt
                    .DataTextField = "CodeCName"
                    .DataValueField = "Code"
                    .DataBind()
                End With
            End Using
        End If
    End Sub
    Protected Overrides Sub BaseOnPageTransfer(ByVal ti As TransferInfo)
        If Not IsPostBack() Then

        End If
    End Sub
    Public Overrides Sub DoAction(ByVal Param As String)
        Select Case Param
            Case "btnAdd"   '存檔返回
                If funCheckData() Then
                    If SaveData() Then
                        GoBack()
                    End If
                End If
            Case "btnActionX"   '返回
                GoBack()
            Case "btnCancel"    '清除
                ClearData()
        End Select
    End Sub

    Private Sub GoBack()
        Dim ti As TransferInfo = Me.StateTransfer
        Me.TransferFramePage(ti.CallerUrl, Nothing, ti.Args)
    End Sub


    Private Function SaveData() As Boolean
        Dim beSC_GroupMutiQrys() As beSC_GroupMutiQry.Row = Nothing
        Dim bsSC_GroupMutiQry As New beSC_GroupMutiQry.Service()
        Dim objSC As New SC()

        Dim strCompID As String = ddlCompRoleID.SelectedValue
        If strCompID = "" Then
            strCompID = UserProfile.SelectCompRoleID
        End If

        Dim intCount As Integer = 0
        For i As Integer = 0 To chkQueryID.Items.Count - 1
            If chkQueryID.Items(i).Selected = True Then
                ReDim Preserve beSC_GroupMutiQrys(intCount)

                '取得輸入資料
                Dim beSC_GroupMutiQry As New beSC_GroupMutiQry.Row()
                beSC_GroupMutiQry.SysID.Value = UserProfile.LoginSysID
                beSC_GroupMutiQry.CompRoleID.Value = strCompID
                beSC_GroupMutiQry.GroupID.Value = ddlGroupID.SelectedValue
                beSC_GroupMutiQry.QueryID.Value = chkQueryID.Items(i).Value
                beSC_GroupMutiQry.CreateDate.Value = Now
                beSC_GroupMutiQry.LastChgComp.Value = UserProfile.ActCompID
                beSC_GroupMutiQry.LastChgID.Value = UserProfile.ActUserID
                beSC_GroupMutiQry.LastChgDate.Value = Now

                '檢查資料是否存在
                If bsSC_GroupMutiQry.IsDataExists(beSC_GroupMutiQry) Then
                    Bsp.Utility.ShowFormatMessage(Me, "W_00010", "")
                    Return False
                End If

                beSC_GroupMutiQrys(intCount) = beSC_GroupMutiQry
                intCount += 1
            End If
        Next

        '儲存資料
        Try
            Return objSC.SC1000_Add(beSC_GroupMutiQrys)
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, "[CC0201_99]" & Bsp.Utility.getMessage("E_00000") & Bsp.Utility.getInnerException(Me.FunID, ex))
            Return False
        End Try
    End Function

    Private Function funCheckData() As Boolean

        Dim strValue As String = ""

        '授權群組
        strValue = ddlGroupID.SelectedValue
        If strValue.Trim = "" Or strValue = "---請選擇---" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblGroupID.Text)
            ddlGroupID.Focus()
            Return False
        End If

        '查詢權限
        strValue = Bsp.Utility.JoinListValue(chkQueryID)
        If strValue = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblQueryID.Text)
            chkQueryID.Focus()
            Return False
        End If

        Return True
    End Function

    Private Sub ClearData()
        If ddlCompRoleID.SelectedValue = "" Then
            ddlGroupID.SelectedIndex = 0
        Else
            ddlCompRoleID.SelectedIndex = 0
            ddlGroupID.Items.Clear()
            ddlGroupID.Items.Insert(0, New ListItem("---請選擇---", ""))
        End If

        For i As Integer = 0 To chkQueryID.Items.Count - 1
            chkQueryID.Items(i).Selected = False
        Next
    End Sub

    Protected Sub ddlCompRoleID_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlCompRoleID.SelectedIndexChanged
        Dim objSC As New SC

        Bsp.Utility.FillDDL(ddlGroupID, "HRDB", "SC_Group S", "S.GroupID", "S.GroupName", Bsp.Utility.DisplayType.Full, "", _
                            "And S.GroupID Not In (Select A.GroupID From SC_GroupMutiQry A Where A.SysID = S.SysID And A.CompRoleID = S.CompRoleID And A.GroupID = S.GroupID)" & _
                            "And S.CompRoleID = " & Bsp.Utility.Quote(ddlCompRoleID.SelectedValue))
        ddlGroupID.Items.Insert(0, New ListItem("---請選擇---", ""))
    End Sub
End Class
