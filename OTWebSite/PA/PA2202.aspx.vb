'****************************************************
'功能說明：單位班別設定-修改
'建立人員：MickySung
'建立日期：2015.05.20
'****************************************************
Imports System.Data

Partial Class PA_PA2202
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then
            '班別代碼
            PA2.FillWTID_PA2200(ddlWTID, UserProfile.SelectCompRoleID)
            ddlWTID.Items.Insert(0, New ListItem("---請選擇---", ""))

            '單位代碼1
            PA2.FillOrganID_PA2201(ddlDeptID, UserProfile.SelectCompRoleID, "", "1", PA2.DisplayType.Full)
            ddlDeptID.Items.Insert(0, New ListItem("---請選擇---", ""))
        End If
    End Sub

    Protected Overrides Sub BaseOnPageTransfer(ByVal ti As TransferInfo)
        If Not IsPostBack() Then
            Dim ht As Hashtable = Bsp.Utility.getHashTableFromParam(ti.Args)

            If ht.ContainsKey("SelectedCompID") Then
                ViewState.Item("CompID") = ht("SelectedCompID").ToString()
                subGetData(ht("SelectedCompID").ToString(), ht("SelectedWTID").ToString(), ht("SelectedDeptID").ToString(), ht("SelectedOrganID").ToString())
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
        Dim beOrgWorkTime As New beOrgWorkTime.Row()
        Dim bsOrgWorkTime As New beOrgWorkTime.Service()
        Dim objPA As New PA2()

        '取得輸入資料
        beOrgWorkTime.CompID.Value = UserProfile.SelectCompRoleID
        beOrgWorkTime.WTID.Value = ddlWTID.SelectedValue
        beOrgWorkTime.DeptID.Value = ddlDeptID.SelectedValue
        beOrgWorkTime.OrganID.Value = ddlOrganID.SelectedValue
        beOrgWorkTime.LastChgComp.Value = UserProfile.ActCompID
        beOrgWorkTime.LastChgID.Value = UserProfile.ActUserID
        beOrgWorkTime.LastChgDate.Value = Now

        '檢查資料是否存在
        If bsOrgWorkTime.IsDataExists(beOrgWorkTime) Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00010", "")
            Return False
        End If

        '儲存資料
        Try
            Return objPA.UpdateOrgWorkTimeSetting(beOrgWorkTime, saveWTID.Value, saveDeptID.Value, saveOrganID.Value)
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, "[CC0201_99]" & Bsp.Utility.getMessage("E_00000") & Bsp.Utility.getInnerException(Me.FunID, ex))
            Return False
        End Try
    End Function

    Private Sub subGetData(ByVal CompID As String, ByVal WTID As String, ByVal DeptID As String, ByVal OrganID As String)
        Dim objPA As New PA2()
        Dim objSC As New SC()
        Dim beOrgWorkTime As New beOrgWorkTime.Row()
        Dim bsOrgWorkTime As New beOrgWorkTime.Service()

        beOrgWorkTime.CompID.Value = CompID
        beOrgWorkTime.WTID.Value = WTID
        beOrgWorkTime.DeptID.Value = DeptID
        beOrgWorkTime.OrganID.Value = OrganID
        Try
            Using dt As DataTable = bsOrgWorkTime.QueryByKey(beOrgWorkTime).Tables(0)
                If dt.Rows.Count <= 0 Then Exit Sub
                beOrgWorkTime = New beOrgWorkTime.Row(dt.Rows(0))

                '2015/05/28 公司代碼-名稱改寫法
                lbltxtCompID.Text = UserProfile.SelectCompRoleName
                'lbltxtCompID.Text = beOrgWorkTime.CompID.Value + "-" + objSC.GetCompName(beOrgWorkTime.CompID.Value).Rows(0).Item("CompName").ToString
                '班別代碼
                ddlWTID.SelectedValue = beOrgWorkTime.WTID.Value
                '單位代碼1
                ddlDeptID.SelectedValue = beOrgWorkTime.DeptID.Value
                '單位代碼2
                PA2.FillOrganID_PA2201(ddlOrganID, UserProfile.SelectCompRoleID, ddlDeptID.SelectedValue, "2", PA2.DisplayType.Full)
                ddlOrganID.SelectedValue = beOrgWorkTime.OrganID.Value
                '最後異動公司
                If beOrgWorkTime.LastChgComp.Value.Trim <> "" Then
                    lblLastChgComp.Text = beOrgWorkTime.LastChgComp.Value + "-" + objSC.GetCompName(beOrgWorkTime.LastChgComp.Value).Rows(0).Item("CompName").ToString
                Else
                    lblLastChgComp.Text = ""
                End If
                '最後異動人員
                If beOrgWorkTime.LastChgID.Value.Trim <> "" Then
                    Dim UserName As String = objSC.GetSC_UserName(beOrgWorkTime.LastChgComp.Value, beOrgWorkTime.LastChgID.Value)
                    lblLastChgID.Text = beOrgWorkTime.LastChgID.Value + IIf(UserName <> "", "-" + UserName, "")
                Else
                    lblLastChgID.Text = ""
                End If
                '最後異動日期
                lblLastChgDate.Text = IIf(Format(beOrgWorkTime.LastChgDate.Value, "yyyy/MM/dd") = "1900/01/01", "", beOrgWorkTime.LastChgDate.Value.ToString("yyyy/MM/dd HH:mm:ss"))

                '隱藏欄位
                saveWTID.Value = beOrgWorkTime.WTID.Value
                saveDeptID.Value = beOrgWorkTime.DeptID.Value
                saveOrganID.Value = beOrgWorkTime.OrganID.Value

            End Using
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & "subGetData", ex)
        End Try
    End Sub

    Private Function funCheckData() As Boolean
        Dim objPA As New PA2()
        Dim beOrgWorkTime As New beOrgWorkTime.Row()
        Dim bsOrgWorkTime As New beOrgWorkTime.Service()

        '班別代碼
        If ddlWTID.SelectedValue = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblWTID.Text)
            ddlWTID.Focus()
            Return False
        End If

        '單位代碼1
        If ddlDeptID.SelectedValue = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblOrgID.Text)
            ddlDeptID.Focus()
            Return False
        End If

        '單位代碼2
        If ddlOrganID.SelectedValue = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblOrgID.Text)
            ddlOrganID.Focus()
            Return False
        End If

        Return True
    End Function

    Private Sub ClearData()
        subGetData(UserProfile.SelectCompRoleID, saveWTID.Value, saveDeptID.Value, saveOrganID.Value)
    End Sub

    Protected Sub ddlDeptID_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlDeptID.SelectedIndexChanged
        '單位代碼2
        PA2.FillOrganID_PA2201(ddlOrganID, UserProfile.SelectCompRoleID, ddlDeptID.SelectedValue, "2", PA2.DisplayType.Full)
        ddlOrganID.Items.Insert(0, New ListItem("---請選擇---", ""))
        UpdOrganID.Update()
    End Sub

End Class
