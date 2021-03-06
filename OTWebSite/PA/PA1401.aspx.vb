'****************************************************
'功能說明：職稱代碼設定-新增
'建立人員：MickySung
'建立日期：2015.04.29
'****************************************************
Imports System.Data

Partial Class PA_PA1401
    Inherits PageBase


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then
            Dim objSC As New SC
            '2015/05/28 公司代碼-名稱改寫法
            lbltxtCompID.Text = UserProfile.SelectCompRoleName
            'lbltxtCompID.Text = UserProfile.SelectCompRoleID + "-" + objSC.GetCompName(UserProfile.SelectCompRoleID).Rows(0).Item("CompName").ToString

            '職等代碼
            'PA1.FillRankID(ddlRankID, UserProfile.SelectCompRoleID)
            Bsp.Utility.Rank(ddlRankID, UserProfile.SelectCompRoleID)
            ddlRankID.Items.Insert(0, New ListItem("---請選擇---", ""))
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
        Dim beTitle As New beTitle.Row()
        Dim bsTitle As New beTitle.Service()
        Dim objPA As New PA1()

        '取得輸入資料
        beTitle.CompID.Value = UserProfile.SelectCompRoleID
        beTitle.RankID.Value = ddlRankID.SelectedValue
        beTitle.TitleID.Value = txtTitleID.Text
        beTitle.TitleName.Value = txtTitleName.Text
        beTitle.TitleEngName.Value = txtTitleEngName.Text
        beTitle.LastChgComp.Value = UserProfile.ActCompID
        beTitle.LastChgID.Value = UserProfile.ActUserID
        beTitle.LastChgDate.Value = Now

        '檢查資料是否存在
        If bsTitle.IsDataExists(beTitle) Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00010", "")
            Return False
        End If

        '儲存資料
        Try
            Return objPA.AddTitleSetting(beTitle)
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, "[CC0201_99]" & Bsp.Utility.getMessage("E_00000") & Bsp.Utility.getInnerException(Me.FunID, ex))
            Return False
        End Try
    End Function

    Private Function funCheckData() As Boolean
        Dim objPA As New PA1()
        Dim strWhere As String = ""
        Dim beTitle As New beTitle.Row()
        Dim bsTitle As New beTitle.Service()

        '職等代碼
        If ddlRankID.SelectedValue = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblRankID.Text)
            ddlRankID.Focus()
            Return False
        End If

        '職稱代碼
        If txtTitleID.Text = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblTitleID.Text)
            txtTitleID.Focus()
            Return False
        End If
        If Bsp.Utility.getStringLength(txtTitleID.Text.Trim) > txtTitleID.MaxLength Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00040", lblTitleID.Text, txtTitleID.MaxLength.ToString)
            Return False
        End If

        '職稱名稱
        'If Bsp.Utility.getStringLength(txtTitleName.Text.Trim) > txtTitleName.MaxLength Then
        '    Bsp.Utility.ShowFormatMessage(Me, "W_00040", lblTitleName.Text, txtTitleName.MaxLength.ToString)
        '    Return False
        'End If

        '職稱英文名稱
        If txtTitleEngName.Text.Trim.Length > txtTitleEngName.MaxLength Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00040", lblTitleEngName.Text, txtTitleEngName.MaxLength.ToString)
            Return False
        End If

        Return True
    End Function

    Private Sub ClearData()
        '職等代碼
        ddlRankID.SelectedValue = ""

        '職稱代碼
        txtTitleID.Text = ""

        '職稱名稱
        txtTitleName.Text = ""

        '職稱英文名稱
        txtTitleEngName.Text = ""
    End Sub

End Class
