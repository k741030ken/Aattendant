'****************************************************
'功能說明：工作性質設定-新增
'建立人員：MickySung
'建立日期：2015.05.05
'****************************************************
Imports System.Data

Partial Class PA_PA1701
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then
            Dim objSC As New SC()
            '2015/05/28 公司代碼-名稱改寫法
            lbltxtCompID.Text = UserProfile.SelectCompRoleName
            'lbltxtCompID.Text = UserProfile.SelectCompRoleID + "-" + objSC.GetCompName(UserProfile.SelectCompRoleID).Rows(0).Item("CompName").ToString

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
        Dim beWorkType As New beWorkType.Row()
        Dim bsWorkType As New beWorkType.Service()
        Dim objPA As New PA1()

        '取得輸入資料
        beWorkType.CompID.Value = UserProfile.SelectCompRoleID
        beWorkType.WorkTypeID.Value = txtWorkTypeID.Text
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

        '檢查資料是否存在
        If bsWorkType.IsDataExists(beWorkType) Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00010", "")
            Return False
        End If

        '儲存資料
        Try
            Return objPA.AddWorkTypeSetting(beWorkType)
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, "[CC0201_99]" & Bsp.Utility.getMessage("E_00000") & Bsp.Utility.getInnerException(Me.FunID, ex))
            Return False
        End Try
    End Function

    Private Function funCheckData() As Boolean
        Dim objPA As New PA1()
        Dim beWorkType As New beWorkType.Row()
        Dim bsWorkType As New beWorkType.Service()

        '工作性質代碼
        If txtWorkTypeID.Text = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblWorkTypeID.Text)
            txtWorkTypeID.Focus()
            Return False
        End If
        If bsWorkType.IsDataExists(beWorkType) Then
            Bsp.Utility.ShowFormatMessage(Me, "工作性質代碼：「" + txtWorkTypeID.Text + "」資料已存在，不可新增！", "")
            txtWorkTypeID.Focus()
            Return False
        End If
        If Bsp.Utility.getStringLength(txtWorkTypeID.Text.Trim) > txtWorkTypeID.MaxLength Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00040", lblWorkTypeID.Text, txtWorkTypeID.MaxLength.ToString)
            txtWorkTypeID.Focus()
            Return False
        End If

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
        '工作性質代碼
        txtWorkTypeID.Text = ""

        '工作性質名稱
        txtRemark.Text = ""

        '無效註記
        chkInValidFlag.Checked = False

        'AO/OP
        ddlAOFlag.SelectedValue = ""

        'PB類
        ChkPBFlag.Checked = False

        '排序
        txtSortOrder.Text = ""

        '部門列印註記
        ddlOrganPrintFlag.SelectedValue = ""

        '工作性質類別
        ddlClass.SelectedValue = ""

        '大類
        ddlCategoryI.SelectedValue = ""

        '中類
        ddlCategoryII.SelectedValue = ""

        '細類
        ddlCategoryIII.SelectedValue = ""
    End Sub

    Protected Sub ddlCategoryI_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlCategoryI.SelectedIndexChanged
        PA1.FillCategoryII_PA1700(ddlCategoryII, ddlCategoryI.SelectedItem.Value)
        ddlCategoryII.Items.Insert(0, New ListItem("---請選擇---", ""))
        UpdCategoryII.Update()
    End Sub

    Protected Sub ddlCategoryII_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlCategoryII.SelectedIndexChanged
        PA1.FillCategoryIII_PA1700(ddlCategoryIII, ddlCategoryI.SelectedItem.Value, ddlCategoryII.SelectedItem.Value)
        ddlCategoryIII.Items.Insert(0, New ListItem("---請選擇---", ""))
        UpdCategoryIII.Update()
    End Sub

End Class
