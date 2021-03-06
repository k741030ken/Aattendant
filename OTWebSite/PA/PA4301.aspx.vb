'****************************************************
'功能說明：Web功能權限維護-新增
'建立人員：BeatriceCheng
'建立日期：2015.05.13
'****************************************************
Imports System.Data

Partial Class PA_PA4301
    Inherits PageBase


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then
            Dim objSC As New SC
            txtComID.Text = UserProfile.SelectCompRoleName
            PA4.FillDDL(ddlWebID, "FormWeb", "WebID", "WebName", PA4.DisplayType.Full)
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
                    Release("btnAdd")
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


    Private Function SaveData(ByVal ReleaseComp As String, ByVal ReleaseID As String) As Boolean
        Dim beUserFormWeb As New beUserFormWeb.Row()
        Dim bsUserFormWeb As New beUserFormWeb.Service()
        Dim objPA4 As New PA4()

        '取得輸入資料
        beUserFormWeb.CompID.Value = UserProfile.SelectCompRoleID
        beUserFormWeb.WebID.Value = ddlWebID.SelectedValue

        beUserFormWeb.ReleaseComp.Value = ReleaseComp
        beUserFormWeb.ReleaseID.Value = ReleaseID

        beUserFormWeb.LastChgComp.Value = UserProfile.ActCompID
        beUserFormWeb.LastChgID.Value = UserProfile.ActUserID
        beUserFormWeb.LastChgDate.Value = Now

        '檢查資料是否存在
        If bsUserFormWeb.IsDataExists(beUserFormWeb) Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00010", "")
            Return False
        End If

        '儲存資料
        Try
            Return objPA4.UserFormWebAdd(beUserFormWeb)
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, "[CC0201_99]" & Bsp.Utility.getMessage("E_00000") & Bsp.Utility.getInnerException(Me.FunID, ex))
            Return False
        End Try
    End Function

    Private Function funCheckData() As Boolean

        Dim strValue As String = ""

        'Web功能代碼
        strValue = ddlWebID.SelectedValue
        If strValue.Trim = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblWebID.Text)
            ddlWebID.Focus()
            Return False
        End If

        Return True
    End Function

    Private Sub ClearData()
        ddlWebID.SelectedIndex = 0
    End Sub

    Private Sub Release(ByVal LogFunction As String)
        ucRelease.ShowCompRole = "True"
        ucRelease.FunID = "PA4300"
        ucRelease.LogFunction = LogFunction
        ucRelease.OpenSelect()
    End Sub

    Public Overrides Sub DoModalReturn(ByVal returnValue As String)
        Dim strSql As String = ""

        If returnValue <> "" Then
            Dim aryData() As String = returnValue.Split(":")

            Select Case aryData(0)
                Case "ucRelease"
                    Dim aryValue() As String = Split(aryData(1), "|$|")
                    If aryValue(0) = "Y" Then
                        If SaveData(aryValue(1), aryValue(2)) Then
                            GoBack()
                        End If
                    End If
            End Select
        End If
    End Sub

End Class
