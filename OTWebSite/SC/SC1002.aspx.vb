'****************************************************
'功能說明：綜合查詢授權設定-新增
'建立人員：BeatriceCheng
'建立日期：2015.11.24
'****************************************************
Imports System.Data

Partial Class SC_SC1002
    Inherits PageBase


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then
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
            Dim ht As Hashtable = Bsp.Utility.getHashTableFromParam(ti.Args)

            If ht.ContainsKey("SelectedCompRoleID") Then
                ViewState.Item("CompRoleID") = ht("SelectedCompRoleID").ToString()
                ViewState.Item("GroupID") = ht("SelectedGroupID").ToString()
                subGetData(ViewState.Item("CompRoleID"), ViewState.Item("GroupID"))
            Else
                Return
            End If
        End If
    End Sub
    Public Overrides Sub DoAction(ByVal Param As String)
        Select Case Param
            Case "btnUpdate"   '存檔返回
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

    Private Sub subGetData(ByVal CompRoleID As String, ByVal GroupID As String)
        Dim objSC As New SC

        Try
            Using dt As DataTable = objSC.SC1000_Query("CompRoleID=" & CompRoleID, "GroupID=" & GroupID)
                txtCompRoleID.Text = UserProfile.SelectCompRoleName
                txtGroupID.Text = ViewState.Item("GroupID") & "-" & ViewState.Item("GroupName")

                If dt.Rows.Count > 0 Then
                    txtCompRoleID.Text = dt.Rows(0).Item("CompRoleID") & "-" & dt.Rows(0).Item("CompRoleName")
                    txtGroupID.Text = dt.Rows(0).Item("GroupID") & "-" & dt.Rows(0).Item("GroupName")

                    For i As Integer = 0 To chkQueryID.Items.Count - 1
                        chkQueryID.Items(i).Selected = IIf(dt.Rows(0).Item("Query" & i).ToString() = "1", True, False)
                    Next

                    txtLastChgComp.Text = dt.Rows(0).Item("LastChgComp") & "-" & dt.Rows(0).Item("LastChgCompName")
                    txtLastChgID.Text = dt.Rows(0).Item("LastChgID") & "-" & dt.Rows(0).Item("LastChgName")
                    txtLastChgDate.Text = dt.Rows(0).Item("LastChgDate")
                End If
            End Using
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & "subGetData", ex)
        End Try
    End Sub

    Private Function SaveData() As Boolean
        Dim beSC_GroupMutiQrys() As beSC_GroupMutiQry.Row = Nothing
        Dim bsSC_GroupMutiQry As New beSC_GroupMutiQry.Service()
        Dim objSC As New SC()

        Dim intCount As Integer = 0
        For i As Integer = 0 To chkQueryID.Items.Count - 1
            If chkQueryID.Items(i).Selected = True Then
                ReDim Preserve beSC_GroupMutiQrys(intCount)

                '取得輸入資料
                Dim beSC_GroupMutiQry As New beSC_GroupMutiQry.Row()
                beSC_GroupMutiQry.SysID.Value = UserProfile.LoginSysID
                beSC_GroupMutiQry.CompRoleID.Value = ViewState("CompRoleID")
                beSC_GroupMutiQry.GroupID.Value = ViewState("GroupID")
                beSC_GroupMutiQry.QueryID.Value = chkQueryID.Items(i).Value
                beSC_GroupMutiQry.CreateDate.Value = Now
                beSC_GroupMutiQry.LastChgComp.Value = UserProfile.ActCompID
                beSC_GroupMutiQry.LastChgID.Value = UserProfile.ActUserID
                beSC_GroupMutiQry.LastChgDate.Value = Now

                beSC_GroupMutiQrys(intCount) = beSC_GroupMutiQry
                intCount += 1
            End If
        Next

        '儲存資料
        Try
            Return objSC.SC1000_Update(beSC_GroupMutiQrys)
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, "[CC0201_99]" & Bsp.Utility.getMessage("E_00000") & Bsp.Utility.getInnerException(Me.FunID, ex))
            Return False
        End Try
    End Function

    Private Function funCheckData() As Boolean

        Dim strValue As String = ""

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
        subGetData(ViewState.Item("CompRoleID"), ViewState.Item("GroupID"))
    End Sub
End Class
