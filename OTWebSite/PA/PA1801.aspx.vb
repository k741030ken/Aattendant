'****************************************************
'功能說明：工作性質設定-新增
'建立人員：MickySung
'建立日期：2015.05.11
'****************************************************
Imports System.Data

Partial Class PA_PA1801
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then
            Dim objSC As New SC()
            '2015/05/28 公司代碼-名稱改寫法
            lbltxtCompID.Text = UserProfile.SelectCompRoleName
            'lbltxtCompID.Text = UserProfile.SelectCompRoleID + "-" + objSC.GetCompName(UserProfile.SelectCompRoleID).Rows(0).Item("CompName").ToString

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
        Dim bePosition As New bePosition.Row()
        Dim bsPosition As New bePosition.Service()
        Dim objPA As New PA1()

        '取得輸入資料
        bePosition.CompID.Value = UserProfile.SelectCompRoleID
        bePosition.PositionID.Value = txtPositionID.Text
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

        '檢查資料是否存在
        If bsPosition.IsDataExists(bePosition) Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00010", "")
            Return False
        End If

        '儲存資料
        Try
            Return objPA.AddPositionSetting(bePosition)
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, "[CC0201_99]" & Bsp.Utility.getMessage("E_00000") & Bsp.Utility.getInnerException(Me.FunID, ex))
            Return False
        End Try
    End Function

    Private Function funCheckData() As Boolean
        Dim objPA As New PA1()
        Dim bePosition As New bePosition.Row()
        Dim bsPosition As New bePosition.Service()

        '職位代碼
        If txtPositionID.Text = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblPositionID.Text)
            txtPositionID.Focus()
            Return False
        End If
        If bsPosition.IsDataExists(bePosition) Then
            Bsp.Utility.ShowFormatMessage(Me, "職位代碼：「" + txtPositionID.Text + "」資料已存在，不可新增！", "")
            txtPositionID.Focus()
            Return False
        End If
        If Bsp.Utility.getStringLength(txtPositionID.Text.Trim) > txtPositionID.MaxLength Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00040", lblPositionID.Text, txtPositionID.MaxLength.ToString)
            txtPositionID.Focus()
            Return False
        End If

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
        '職位代碼
        txtPositionID.Text = ""

        '職位名稱
        txtRemark.Text = ""

        '無效註記
        chkInValidFlag.Checked = False

        '排序
        txtSortOrder.Text = ""

        '部門列印註記
        ddlOrganPrintFlag.SelectedValue = ""

        '工作性質類別
        chkIsEVManager.Checked = False

        '大類
        ddlCategoryI.SelectedValue = ""

        '中類
        ddlCategoryII.SelectedValue = ""

        '細類
        ddlCategoryIII.SelectedValue = ""
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
