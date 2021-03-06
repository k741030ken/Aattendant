'****************************************************
'功能說明：金控職等對照設定-修改
'建立人員：MickySung
'建立日期：2015.04.30
'****************************************************
Imports System.Data

Partial Class PA_PA1502
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then
            '公司職等
            PA1.FillRankID_PA1502(ddlRankID, UserProfile.SelectCompRoleID)
            ddlRankID.Items.Insert(0, New ListItem("---請選擇---", ""))

            '金控職等
            PA1.FillTitleByHolding(ddlHoldingRankID)
            ddlHoldingRankID.Items.Insert(0, New ListItem("---請選擇---", ""))
        End If
    End Sub

    Protected Overrides Sub BaseOnPageTransfer(ByVal ti As TransferInfo)
        If Not IsPostBack() Then
            Dim ht As Hashtable = Bsp.Utility.getHashTableFromParam(ti.Args)

            If ht.ContainsKey("SelectedHoldingRankID") Then
                ViewState.Item("HoldingRankID") = ht("SelectedHoldingRankID").ToString()
                subGetData(ht("SelectedRankID").ToString(), ht("SelectedCompID").ToString(), ht("SelectedHoldingRankID").ToString())
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

    Private Function SaveData() As Boolean
        Dim beCompareRank As New beCompareRank.Row()
        Dim bsCompareRank As New beCompareRank.Service()
        Dim objPA As New PA1()
        Dim count As Integer

        '取得輸入資料
        beCompareRank.CompID.Value = UserProfile.SelectCompRoleID
        beCompareRank.RankID.Value = ddlRankID.SelectedValue
        beCompareRank.HoldingRankID.Value = ddlHoldingRankID.SelectedValue
        beCompareRank.LastChgComp.Value = UserProfile.ActCompID
        beCompareRank.LastChgID.Value = UserProfile.ActUserID
        beCompareRank.LastChgDate.Value = Now

        '2015/07/21新增防呆，【一個公司職等】只能對照【一個金控職等】
        count = objPA.SelectHoldingRankID(saveCompID.Value, ddlRankID.SelectedValue).Rows(0).Item(0).ToString()
        If saveRankID.Value = ddlRankID.SelectedValue Then
            If count >= 2 Then
                Bsp.Utility.ShowMessage(Me, "公司職等已經存在，【一個公司職等】只能對照【一個金控職等】")
                Return False
            End If
        Else
            If count >= 1 Then
                Bsp.Utility.ShowMessage(Me, "公司職等已經存在，【一個公司職等】只能對照【一個金控職等】")
                Return False
            End If
        End If

        '檢查資料是否存在
        If bsCompareRank.IsDataExists(beCompareRank) Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00010", "")
            Return False
        End If

        '儲存資料
        Try
            Return objPA.UpdateCompareRankSetting(beCompareRank, saveCompID.Value, saveRankID.Value, saveHoldingRankID.Value)
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, "[CC0201_99]" & Bsp.Utility.getMessage("E_00000") & Bsp.Utility.getInnerException(Me.FunID, ex))
            Return False
        End Try
    End Function

    Private Sub subGetData(ByVal RankID As String, ByVal CompID As String, ByVal HoldingRankID As String)
        Dim objPA As New PA1()
        Dim objSC As New SC
        Dim beCompareRank As New beCompareRank.Row()
        Dim bsCompareRank As New beCompareRank.Service()


        beCompareRank.RankID.Value = RankID
        beCompareRank.CompID.Value = CompID
        beCompareRank.HoldingRankID.Value = HoldingRankID
        Try
            Using dt As DataTable = bsCompareRank.QueryByKey(beCompareRank).Tables(0)
                If dt.Rows.Count <= 0 Then Exit Sub
                beCompareRank = New beCompareRank.Row(dt.Rows(0))

                '2015/05/28 公司代碼-名稱改寫法
                lbltxtCompID.Text = UserProfile.SelectCompRoleName
                'lbltxtCompID.Text = beCompareRank.CompID.Value + "-" + objSC.GetCompName(beCompareRank.CompID.Value).Rows(0).Item("CompName").ToString
                '公司職等
                ddlRankID.SelectedValue = beCompareRank.RankID.Value
                '金控職等
                ddlHoldingRankID.SelectedValue = beCompareRank.HoldingRankID.Value
                '最後異動公司
                If beCompareRank.LastChgComp.Value.Trim <> "" Then
                    lblLastChgComp.Text = beCompareRank.LastChgComp.Value + "-" + objSC.GetCompName(beCompareRank.LastChgComp.Value).Rows(0).Item("CompName").ToString
                Else
                    lblLastChgComp.Text = ""
                End If
                '最後異動人員
                If beCompareRank.LastChgID.Value.Trim <> "" Then
                    Dim UserName As String = objSC.GetSC_UserName(beCompareRank.LastChgComp.Value, beCompareRank.LastChgID.Value)
                    lblLastChgID.Text = beCompareRank.LastChgID.Value + IIf(UserName <> "", "-" + UserName, "")
                Else
                    lblLastChgID.Text = ""
                End If
                '最後異動日期
                lblLastChgDate.Text = IIf(Format(beCompareRank.LastChgDate.Value, "yyyy/MM/dd") = "1900/01/01", "", beCompareRank.LastChgDate.Value.ToString("yyyy/MM/dd HH:mm:ss"))

                '隱藏欄位
                saveCompID.Value = beCompareRank.CompID.Value
                saveRankID.Value = beCompareRank.RankID.Value
                saveHoldingRankID.Value = beCompareRank.HoldingRankID.Value

            End Using
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & "subGetData", ex)
        End Try

    End Sub

    Private Function funCheckData() As Boolean
        Dim objPA As New PA1()
        Dim strWhere As String = ""
        Dim beTitle As New beTitle.Row()
        Dim bsTitle As New beTitle.Service()

        '公司職等
        If ddlRankID.SelectedValue = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblRankID.Text)
            ddlRankID.Focus()
            Return False
        End If

        '金控職等
        If ddlHoldingRankID.SelectedValue = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblHoldingRankID.Text)
            ddlHoldingRankID.Focus()
            Return False
        End If

        Return True
    End Function

    Private Sub ClearData()
        subGetData(saveRankID.Value, saveCompID.Value, saveHoldingRankID.Value)
    End Sub

End Class
