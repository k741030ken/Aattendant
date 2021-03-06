'****************************************************
'功能說明：職等對照設定-修改
'建立人員：MickySung
'建立日期：2015.05.04
'****************************************************
Imports System.Data

Partial Class PA_PA1602
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then
            Dim objSC As New SC
            '公司職等
            PA1.FillRankID_PA1600(ddlRankID, UserProfile.SelectCompRoleID)
            ddlRankID.Items.Insert(0, New ListItem("---請選擇---", ""))

            '2015/07/21 下拉選單改成textbox
            ''對照職等
            'PA1.FillRankIDMap_PA1600(ddlRankIDMap, UserProfile.SelectCompRoleID)
            'ddlRankIDMap.Items.Insert(0, New ListItem("---請選擇---", ""))
        End If
    End Sub

    Protected Overrides Sub BaseOnPageTransfer(ByVal ti As TransferInfo)
        If Not IsPostBack() Then
            Dim ht As Hashtable = Bsp.Utility.getHashTableFromParam(ti.Args)

            If ht.ContainsKey("SelectedRankIDMap") Then
                ViewState.Item("RankIDMap") = ht("SelectedRankIDMap").ToString()
                subGetData(ht("SelectedRankID").ToString(), ht("SelectedCompID").ToString(), ht("SelectedRankIDMap").ToString())
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
        Dim beRankMapping As New beRankMapping.Row()
        Dim bsRankMapping As New beRankMapping.Service()
        Dim objPA As New PA1()
        Dim count As Integer

        '取得輸入資料
        beRankMapping.CompID.Value = UserProfile.SelectCompRoleID
        beRankMapping.RankID.Value = ddlRankID.SelectedValue
        beRankMapping.RankIDMap.Value = txtRankIDMap.Text
        beRankMapping.LastChgComp.Value = UserProfile.ActCompID
        beRankMapping.LastChgID.Value = UserProfile.ActUserID
        beRankMapping.LastChgDate.Value = Now

        '2015/07/21新增防呆，【一個公司職等】只能對照【一個金控職等】
        count = objPA.SelectRankID(saveCompID.Value, ddlRankID.SelectedValue).Rows(0).Item(0).ToString()
        If saveRankID.Value = ddlRankID.SelectedValue Then
            If count >= 2 Then
                Bsp.Utility.ShowMessage(Me, "公司職等已經存在，【一個公司職等】只能對照【一個比對職等】")
                Return False
            End If
        Else
            If count >= 1 Then
                Bsp.Utility.ShowMessage(Me, "公司職等已經存在，【一個公司職等】只能對照【一個比對職等】")
                Return False
            End If
        End If
        

        '檢查資料是否存在
        If bsRankMapping.IsDataExists(beRankMapping) Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00010", "")
            Return False
        End If

        '儲存資料
        Try
            Return objPA.UpdateRankMappingSetting(beRankMapping, saveCompID.Value, saveRankID.Value, saveRankIDMap.Value)
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, "[CC0201_99]" & Bsp.Utility.getMessage("E_00000") & Bsp.Utility.getInnerException(Me.FunID, ex))
            Return False
        End Try
    End Function

    Private Sub subGetData(ByVal RankID As String, ByVal CompID As String, ByVal RankIDMap As String)
        Dim objPA As New PA1
        Dim objSC As New SC
        Dim beRankMapping As New beRankMapping.Row()
        Dim bsRankMapping As New beRankMapping.Service()

        beRankMapping.RankID.Value = RankID
        beRankMapping.CompID.Value = CompID
        beRankMapping.RankIDMap.Value = RankIDMap
        Try
            Using dt As DataTable = bsRankMapping.QueryByKey(beRankMapping).Tables(0)
                If dt.Rows.Count <= 0 Then Exit Sub
                beRankMapping = New beRankMapping.Row(dt.Rows(0))

                '2015/05/28 公司代碼-名稱改寫法
                lbltxtCompID.Text = UserProfile.SelectCompRoleName
                'lbltxtCompID.Text = beRankMapping.CompID.Value + "-" + objSC.GetCompName(beRankMapping.CompID.Value).Rows(0).Item("CompName").ToString
                '公司職等
                ddlRankID.SelectedValue = beRankMapping.RankID.Value
                '金控職等
                txtRankIDMap.Text = beRankMapping.RankIDMap.Value
                '最後異動公司
                If beRankMapping.LastChgComp.Value.Trim <> "" Then
                    lblLastChgComp.Text = beRankMapping.LastChgComp.Value + "-" + objSC.GetCompName(beRankMapping.LastChgComp.Value).Rows(0).Item("CompName").ToString
                Else
                    lblLastChgComp.Text = ""
                End If
                '最後異動人員
                If beRankMapping.LastChgID.Value.Trim <> "" Then
                    Dim UserName As String = objSC.GetSC_UserName(beRankMapping.LastChgComp.Value, beRankMapping.LastChgID.Value)
                    lblLastChgID.Text = beRankMapping.LastChgID.Value + IIf(UserName <> "", "-" + UserName, "")
                Else
                    lblLastChgID.Text = ""
                End If
                '最後異動日期
                lblLastChgDate.Text = IIf(Format(beRankMapping.LastChgDate.Value, "yyyy/MM/dd") = "1900/01/01", "", beRankMapping.LastChgDate.Value.ToString("yyyy/MM/dd HH:mm:ss"))

                '隱藏欄位
                saveCompID.Value = beRankMapping.CompID.Value
                saveRankID.Value = beRankMapping.RankID.Value
                saveRankIDMap.Value = beRankMapping.RankIDMap.Value

            End Using
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & "subGetData", ex)
        End Try
    End Sub

    Private Function funCheckData() As Boolean
        Dim objPA As New PA1()
        Dim beRankMapping As New beRankMapping.Row()
        Dim bsRankMapping As New beRankMapping.Service()

        '公司職等(數字/文字)
        If ddlRankID.SelectedValue = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblRankID.Text)
            ddlRankID.Focus()
            Return False
        End If

        '對照職等(數字)
        If txtRankIDMap.Text = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblRankIDMap.Text)
            txtRankIDMap.Focus()
            Return False
        End If

        Return True
    End Function

    Private Sub ClearData()
        subGetData(saveRankID.Value, saveCompID.Value, saveRankIDMap.Value)
    End Sub

End Class
