'****************************************************
'功能說明：工作性質設定-修改
'建立人員：MickySung
'建立日期：2015.05.05
'****************************************************
Imports System.Data

Partial Class PA_PA1702
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then
            Dim objSC As New SC
            '大類
            PA1.FillCategoryI_PA1700(ddlCategoryI)
            ddlCategoryI.Items.Insert(0, New ListItem("---請選擇---", ""))

            '中類
            PA1.FillCategoryII_PA1700(ddlCategoryII, ddlCategoryI.SelectedValue)
            ddlCategoryII.Items.Insert(0, New ListItem("---請選擇---", ""))

            '細類
            PA1.FillCategoryIII_PA1700(ddlCategoryIII, ddlCategoryI.SelectedValue, ddlCategoryII.SelectedValue)
            ddlCategoryIII.Items.Insert(0, New ListItem("---請選擇---", ""))
        End If
    End Sub

    Protected Overrides Sub BaseOnPageTransfer(ByVal ti As TransferInfo)
        If Not IsPostBack() Then
            Dim ht As Hashtable = Bsp.Utility.getHashTableFromParam(ti.Args)

            If ht.ContainsKey("SelectedWorkTypeID") Then
                ViewState.Item("WorkTypeID") = ht("SelectedWorkTypeID").ToString()
                subGetData(ht("SelectedWorkTypeID").ToString())
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
        Dim beWorkType As New beWorkType.Row()
        Dim bsWorkType As New beWorkType.Service()
        Dim objPA As New PA1()

        '取得輸入資料
        beWorkType.CompID.Value = UserProfile.SelectCompRoleID
        beWorkType.WorkTypeID.Value = lbltxtWorkTypeID.Text
        beWorkType.Remark.Value = txtRemark.Text
        beWorkType.InValidFlag.Value = IIf(chkInValidFlag.Checked, "1", "0")
        beWorkType.AOFlag.Value = ddlAOFlag.SelectedValue
        beWorkType.PBFlag.Value = IIf(ChkPBFlag.Checked, "1", "0")
        beWorkType.SortOrder.Value = txtSortOrder.Text
        beWorkType.OrganPrintFlag.Value = ddlOrganPrintFlag.SelectedValue
        beWorkType.Class.Value = ddlClass.SelectedValue
        beWorkType.CategoryI.Value = ddlCategoryI.SelectedValue
        beWorkType.CategoryII.Value = ddlCategoryII.SelectedValue
        beWorkType.CategoryIII.Value = ddlCategoryIII.SelectedValue
        beWorkType.LastChgComp.Value = UserProfile.ActCompID
        beWorkType.LastChgID.Value = UserProfile.ActUserID
        beWorkType.LastChgDate.Value = Now

        '儲存資料
        Try
            Return objPA.UpdateWorkTypeSetting(beWorkType)
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, "[CC0201_99]" & Bsp.Utility.getMessage("E_00000") & Bsp.Utility.getInnerException(Me.FunID, ex))
            Return False
        End Try
    End Function

    Private Sub subGetData(ByVal WorkTypeID As String)
        Dim objPA As New PA1()
        Dim objSC As New SC()
        Dim beWorkType As New beWorkType.Row()
        Dim bsWorkType As New beWorkType.Service()

        beWorkType.CompID.Value = UserProfile.SelectCompRoleID
        beWorkType.WorkTypeID.Value = WorkTypeID
        Try
            Using dt As DataTable = bsWorkType.QueryByKey(beWorkType).Tables(0)
                If dt.Rows.Count <= 0 Then Exit Sub
                beWorkType = New beWorkType.Row(dt.Rows(0))

                '2015/05/28 公司代碼-名稱改寫法
                lbltxtCompID.Text = UserProfile.SelectCompRoleName
                'lbltxtCompID.Text = beWorkType.CompID.Value + "-" + objSC.GetCompName(beWorkType.CompID.Value).Rows(0).Item("CompName").ToString
                '工作性質代碼
                lbltxtWorkTypeID.Text = beWorkType.WorkTypeID.Value
                '工作性質名稱
                txtRemark.Text = beWorkType.Remark.Value
                '無效註記
                chkInValidFlag.Checked = IIf(beWorkType.InValidFlag.Value = "1", True, False)
                'AO/OP
                ddlAOFlag.SelectedValue = beWorkType.AOFlag.Value
                'PB類
                ChkPBFlag.Checked = IIf(beWorkType.PBFlag.Value = "1", True, False)
                '排序
                txtSortOrder.Text = beWorkType.SortOrder.Value
                '部門列印註記
                ddlOrganPrintFlag.SelectedValue = beWorkType.OrganPrintFlag.Value
                '工作性質類別
                ddlClass.SelectedValue = beWorkType.Class.Value
                '大類
                ddlCategoryI.SelectedValue = beWorkType.CategoryI.Value
                '中類
                PA1.FillCategoryII_PA1700(ddlCategoryII, ddlCategoryI.SelectedValue)
                ddlCategoryII.Items.Insert(0, New ListItem("---請選擇---", ""))
                ddlCategoryII.SelectedValue = beWorkType.CategoryII.Value
                '細類
                PA1.FillCategoryIII_PA1700(ddlCategoryIII, ddlCategoryI.SelectedValue, ddlCategoryII.SelectedValue)
                ddlCategoryIII.Items.Insert(0, New ListItem("---請選擇---", ""))
                ddlCategoryIII.SelectedValue = beWorkType.CategoryIII.Value
                '最後異動公司
                If beWorkType.LastChgComp.Value.Trim <> "" Then
                    lblLastChgComp.Text = beWorkType.LastChgComp.Value + "-" + objSC.GetCompName(beWorkType.LastChgComp.Value).Rows(0).Item("CompName").ToString
                Else
                    lblLastChgComp.Text = ""
                End If
                '最後異動人員
                If beWorkType.LastChgID.Value.Trim <> "" Then
                    Dim UserName As String = objSC.GetSC_UserName(beWorkType.LastChgComp.Value, beWorkType.LastChgID.Value)
                    lblLastChgID.Text = beWorkType.LastChgID.Value + IIf(UserName <> "", "-" + UserName, "")
                Else
                    lblLastChgID.Text = ""
                End If
                '最後異動日期
                lblLastChgDate.Text = IIf(Format(beWorkType.LastChgDate.Value, "yyyy/MM/dd") = "1900/01/01", "", beWorkType.LastChgDate.Value.ToString("yyyy/MM/dd HH:mm:ss"))

            End Using
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & "subGetData", ex)
        End Try
    End Sub

    Private Function funCheckData() As Boolean
        Dim objPA As New PA1()
        Dim beWorkType As New beWorkType.Row()
        Dim bsWorkType As New beWorkType.Service()

        '工作性質名稱
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
        subGetData(lbltxtWorkTypeID.Text)
    End Sub

    Protected Sub ddlCategoryI_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlCategoryI.SelectedIndexChanged
        PA1.FillCategoryII_PA1700(ddlCategoryII, ddlCategoryI.SelectedItem.Value)
        ddlCategoryII.Items.Insert(0, New ListItem("---請選擇---", ""))
        ddlCategoryII.SelectedIndex = 0
        UpdCategoryII.Update()

        PA1.FillCategoryIII_PA1700(ddlCategoryIII, ddlCategoryI.SelectedItem.Value, ddlCategoryII.SelectedItem.Value)
        ddlCategoryIII.Items.Insert(0, New ListItem("---請選擇---", ""))
        ddlCategoryIII.SelectedIndex = 0
        UpdCategoryIII.Update()
    End Sub

    Protected Sub ddlCategoryII_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlCategoryII.SelectedIndexChanged
        PA1.FillCategoryIII_PA1700(ddlCategoryIII, ddlCategoryI.SelectedItem.Value, ddlCategoryII.SelectedItem.Value)
        ddlCategoryIII.Items.Insert(0, New ListItem("---請選擇---", ""))
        ddlCategoryIII.SelectedIndex = 0
        UpdCategoryIII.Update()
    End Sub

End Class
