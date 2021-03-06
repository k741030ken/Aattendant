'****************************************************
'功能說明：工作性質設定-修改
'建立人員：MickySung
'建立日期：2015.05.11
'****************************************************
Imports System.Data

Partial Class PA_PA1802
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then
            Dim objSC As New SC
            '大類
            PA1.FillCategoryI_PA1800(ddlCategoryI)
            ddlCategoryI.Items.Insert(0, New ListItem("---請選擇---", ""))

            '中類
            PA1.FillCategoryII_PA1800(ddlCategoryII, ddlCategoryI.SelectedValue)
            ddlCategoryII.Items.Insert(0, New ListItem("---請選擇---", ""))

            '細類
            PA1.FillCategoryIII_PA1800(ddlCategoryIII, ddlCategoryI.SelectedValue, ddlCategoryII.SelectedValue)
            ddlCategoryIII.Items.Insert(0, New ListItem("---請選擇---", ""))
        End If
    End Sub

    Protected Overrides Sub BaseOnPageTransfer(ByVal ti As TransferInfo)
        If Not IsPostBack() Then
            Dim ht As Hashtable = Bsp.Utility.getHashTableFromParam(ti.Args)

            If ht.ContainsKey("SelectedPositionID") Then
                ViewState.Item("PositionID") = ht("SelectedPositionID").ToString()
                subGetData(ht("SelectedPositionID").ToString())
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
        Dim bePosition As New bePosition.Row()
        Dim bsPosition As New bePosition.Service()
        Dim objPA As New PA1()

        '取得輸入資料
        bePosition.CompID.Value = UserProfile.SelectCompRoleID
        bePosition.PositionID.Value = lbltxtPositionID.Text
        bePosition.Remark.Value = txtRemark.Text
        bePosition.InValidFlag.Value = IIf(chkInValidFlag.Checked, "1", "0")
        bePosition.SortOrder.Value = txtSortOrder.Text
        bePosition.OrganPrintFlag.Value = ddlOrganPrintFlag.SelectedValue
        bePosition.IsEVManager.Value = IIf(chkIsEVManager.Checked, "1", "0")
        bePosition.CategoryI.Value = ddlCategoryI.SelectedValue
        bePosition.CategoryII.Value = ddlCategoryII.SelectedValue
        bePosition.CategoryIII.Value = ddlCategoryIII.SelectedValue
        bePosition.LastChgComp.Value = UserProfile.ActCompID
        bePosition.LastChgID.Value = UserProfile.ActUserID
        bePosition.LastChgDate.Value = Now

        '儲存資料
        Try
            Return objPA.UpdatePositionSetting(bePosition)
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, "[CC0201_99]" & Bsp.Utility.getMessage("E_00000") & Bsp.Utility.getInnerException(Me.FunID, ex))
            Return False
        End Try
    End Function

    Private Sub subGetData(ByVal PositionID As String)
        Dim objPA As New PA1()
        Dim objSC As New SC()
        Dim bePosition As New bePosition.Row()
        Dim bsPosition As New bePosition.Service()

        bePosition.CompID.Value = UserProfile.SelectCompRoleID
        bePosition.PositionID.Value = PositionID
        Try
            Using dt As DataTable = bsPosition.QueryByKey(bePosition).Tables(0)
                If dt.Rows.Count <= 0 Then Exit Sub
                bePosition = New bePosition.Row(dt.Rows(0))

                '2015/05/28 公司代碼-名稱改寫法
                lbltxtCompID.Text = UserProfile.SelectCompRoleName
                'lbltxtCompID.Text = bePosition.CompID.Value + "-" + objSC.GetCompName(bePosition.CompID.Value).Rows(0).Item("CompName").ToString
                '職位代碼
                lbltxtPositionID.Text = bePosition.PositionID.Value
                '職位名稱
                txtRemark.Text = bePosition.Remark.Value
                '無效註記
                chkInValidFlag.Checked = IIf(bePosition.InValidFlag.Value = "1", True, False)
                '排序
                txtSortOrder.Text = bePosition.SortOrder.Value
                '部門列印註記
                ddlOrganPrintFlag.SelectedValue = bePosition.OrganPrintFlag.Value
                '績效考核表主管註記
                chkIsEVManager.Checked = bePosition.IsEVManager.Value
                '大類
                ddlCategoryI.SelectedValue = bePosition.CategoryI.Value
                '中類
                PA1.FillCategoryII_PA1800(ddlCategoryII, ddlCategoryI.SelectedValue)
                ddlCategoryII.Items.Insert(0, New ListItem("---請選擇---", ""))
                ddlCategoryII.SelectedValue = bePosition.CategoryII.Value
                '細類
                PA1.FillCategoryIII_PA1800(ddlCategoryIII, ddlCategoryI.SelectedValue, ddlCategoryII.SelectedValue)
                ddlCategoryIII.Items.Insert(0, New ListItem("---請選擇---", ""))
                ddlCategoryIII.SelectedValue = bePosition.CategoryIII.Value
                '最後異動公司
                If bePosition.LastChgComp.Value.Trim <> "" Then
                    lblLastChgComp.Text = bePosition.LastChgComp.Value + "-" + objSC.GetCompName(bePosition.LastChgComp.Value).Rows(0).Item("CompName").ToString
                Else
                    lblLastChgComp.Text = ""
                End If
                '最後異動人員
                If bePosition.LastChgID.Value.Trim <> "" Then
                    Dim UserName As String = objSC.GetSC_UserName(bePosition.LastChgComp.Value, bePosition.LastChgID.Value)
                    lblLastChgID.Text = bePosition.LastChgID.Value + IIf(UserName <> "", "-" + UserName, "")
                Else
                    lblLastChgID.Text = ""
                End If
                '最後異動日期
                lblLastChgDate.Text = IIf(Format(bePosition.LastChgDate.Value, "yyyy/MM/dd") = "1900/01/01", "", bePosition.LastChgDate.Value.ToString("yyyy/MM/dd HH:mm:ss"))

            End Using
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & "subGetData", ex)
        End Try
    End Sub

    Private Function funCheckData() As Boolean
        Dim objPA As New PA1()
        Dim bePosition As New bePosition.Row()
        Dim bsPosition As New bePosition.Service()

        '職位名稱
        If txtRemark.Text = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblRemark.Text)
            txtRemark.Focus()
            Return False
        End If
        If txtRemark.Text.Trim.Length > txtRemark.MaxLength Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00040", lblRemark.Text, txtRemark.MaxLength.ToString)
            txtRemark.Focus()
            Return False
        End If

        '排序
        If Bsp.Utility.getStringLength(txtSortOrder.Text.Trim) > txtSortOrder.MaxLength Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00040", lblSortOrder.Text, txtSortOrder.MaxLength.ToString)
            txtSortOrder.Focus()
            Return False
        End If

        Return True
    End Function

    Private Sub ClearData()
        subGetData(lbltxtPositionID.Text)
    End Sub

    Protected Sub ddlCategoryI_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlCategoryI.SelectedIndexChanged
        PA1.FillCategoryII_PA1800(ddlCategoryII, ddlCategoryI.SelectedItem.Value)
        ddlCategoryII.Items.Insert(0, New ListItem("---請選擇---", ""))
        UpdCategoryII.Update()
    End Sub

    Protected Sub ddlCategoryII_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlCategoryII.SelectedIndexChanged
        PA1.FillCategoryIII_PA1800(ddlCategoryIII, ddlCategoryI.SelectedItem.Value, ddlCategoryII.SelectedItem.Value)
        ddlCategoryIII.Items.Insert(0, New ListItem("---請選擇---", ""))
        UpdCategoryIII.Update()
    End Sub

End Class
