'****************************************************
'功能說明：Web我們的同仁組織色塊設定-新增
'建立人員：BeatriceCheng
'建立日期：2015.05.15
'****************************************************
Imports System.Data

Partial Class PA_PA4401
    Inherits PageBase


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then
            Dim objSC As New SC
            txtComID.Text = UserProfile.SelectCompRoleName
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
        Dim beOrganColor_Web As New beOrganColor_Web.Row()
        Dim bsOrganColor_Web As New beOrganColor_Web.Service()
        Dim objPA4 As New PA4()

        '取得輸入資料
        beOrganColor_Web.CompID.Value = UserProfile.SelectCompRoleID
        beOrganColor_Web.SortOrder.Value = txtSortOrder.Text

        If hldColor.Value = "" Then hldColor.Value = "#F0F8FF"
        beOrganColor_Web.Color.Value = hldColor.Value

        beOrganColor_Web.LastChgComp.Value = UserProfile.ActCompID
        beOrganColor_Web.LastChgID.Value = UserProfile.ActUserID
        beOrganColor_Web.LastChgDate.Value = Now

        '檢查資料是否存在
        If bsOrganColor_Web.IsDataExists(beOrganColor_Web) Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00010", "")
            Return False
        End If

        '儲存資料
        Try
            Return objPA4.OrganColor_WebAdd(beOrganColor_Web)
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, "[CC0201_99]" & Bsp.Utility.getMessage("E_00000") & Bsp.Utility.getInnerException(Me.FunID, ex))
            Return False
        End Try
    End Function

    Private Function funCheckData() As Boolean

        Dim strValue As String = ""

        '排序
        strValue = txtSortOrder.Text
        If strValue.Trim = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblSortOrder.Text)
            txtSortOrder.Focus()
            Return False
        End If
        If Bsp.Utility.getStringLength(strValue) > txtSortOrder.MaxLength Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00040", lblSortOrder.Text, txtSortOrder.MaxLength.ToString())
            txtSortOrder.Focus()
            Return False
        End If

        Return True
    End Function

    Private Sub ClearData()
        txtSortOrder.Text = ""
        txtColor.Text = ""
        hldColor.Value = ""
    End Sub
End Class
