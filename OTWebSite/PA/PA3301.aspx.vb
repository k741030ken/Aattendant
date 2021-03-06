'****************************************************
'功能說明：家屬稱謂狀況設定-新增
'建立人員：BeatriceCheng
'建立日期：2015.05.05
'****************************************************
Imports System.Data

Partial Class PA_PA3201
    Inherits PageBase


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then

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
        Dim beRelationship As New beRelationship.Row()
        Dim bsRelationship As New beRelationship.Service()
        Dim objPA3 As New PA3()

        '取得輸入資料
        beRelationship.RelativeID.Value = txtRelativeID.Text
        beRelationship.Remark.Value = txtRemark.Text
        beRelationship.RemarkCN.Value = txtRemarkCN.Text
        beRelationship.HeaRelID.Value = txtHeaRelID.Text
        beRelationship.CompRelID.Value = txtCompRelID.Text
        beRelationship.DeathPayID.Value = txtDeathPayID.Text
        beRelationship.TaxFamilyID.Value = ddlTaxFamilyID.SelectedValue
        beRelationship.RelTypeID.Value = ddlRelTypeID.SelectedValue
        beRelationship.RelClassID.Value = txtRelClassID.Text

        beRelationship.LastChgComp.Value = UserProfile.ActCompID
        beRelationship.LastChgID.Value = UserProfile.ActUserID
        beRelationship.LastChgDate.Value = Now

        '檢查資料是否存在
        If bsRelationship.IsDataExists(beRelationship) Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00010", "")
            Return False
        End If

        '儲存資料
        Try
            Return objPA3.RelationshipAdd(beRelationship)
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, "[CC0201_99]" & Bsp.Utility.getMessage("E_00000") & Bsp.Utility.getInnerException(Me.FunID, ex))
            Return False
        End Try
    End Function

    Private Function funCheckData() As Boolean

        Dim strValue As String = ""

        '家屬稱謂代碼
        strValue = txtRelativeID.Text
        If strValue.Trim = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblRelativeID.Text)
            txtRelativeID.Focus()
            Return False
        End If
        If Bsp.Utility.getStringLength(strValue) > txtRelativeID.MaxLength Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00040", lblRelativeID.Text, txtRelativeID.MaxLength.ToString())
            txtRelativeID.Focus()
            Return False
        End If

        '家屬稱謂名稱(繁)
        strValue = txtRemark.Text
        If strValue.Trim = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblRemark.Text)
            txtRemark.Focus()
            Return False
        End If
        'If Bsp.Utility.getStringLength(strValue) > txtRemark.MaxLength Then
        '    Bsp.Utility.ShowFormatMessage(Me, "W_00040", lblRemark.Text, txtRemark.MaxLength.ToString())
        '    txtRemark.Focus()
        '    Return False
        'End If

        '家屬稱謂名稱(簡)
        strValue = txtRemarkCN.Text
        If strValue.Trim = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblRemarkCN.Text)
            txtRemarkCN.Focus()
            Return False
        End If
        'If Bsp.Utility.getStringLength(strValue) > txtRemarkCN.MaxLength Then
        '    Bsp.Utility.ShowFormatMessage(Me, "W_00040", lblRemarkCN.Text, txtRemarkCN.MaxLength.ToString())
        '    txtRemarkCN.Focus()
        '    Return False
        'End If

        '健保家屬稱謂代碼
        strValue = txtHeaRelID.Text
        If Bsp.Utility.getStringLength(strValue) > txtHeaRelID.MaxLength Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00040", lblHeaRelID.Text, txtHeaRelID.MaxLength.ToString())
            txtHeaRelID.Focus()
            Return False
        End If

        '團保家屬稱謂代碼
        strValue = txtCompRelID.Text
        If Bsp.Utility.getStringLength(strValue) > txtCompRelID.MaxLength Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00040", lblCompRelID.Text, txtCompRelID.MaxLength.ToString())
            txtCompRelID.Focus()
            Return False
        End If

        '喪葬補助稱謂代碼
        strValue = txtDeathPayID.Text
        If Bsp.Utility.getStringLength(strValue) > txtDeathPayID.MaxLength Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00040", lblDeathPayID.Text, txtDeathPayID.MaxLength.ToString())
            txtDeathPayID.Focus()
            Return False
        End If

        '幾等親代碼
        strValue = txtRelClassID.Text
        If Bsp.Utility.getStringLength(strValue) > txtRelClassID.MaxLength Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00040", lblRelClassID.Text, txtRelClassID.MaxLength.ToString())
            txtRelClassID.Focus()
            Return False
        End If

        Return True
    End Function

    Private Sub ClearData()
        txtRelativeID.Text = ""
        txtRemark.Text = ""
        txtRemarkCN.Text = ""
        txtHeaRelID.Text = ""
        txtCompRelID.Text = ""
        txtDeathPayID.Text = ""
        ddlTaxFamilyID.SelectedIndex = 0
        ddlRelTypeID.SelectedIndex = 0
        txtRelClassID.Text = ""
    End Sub

End Class
