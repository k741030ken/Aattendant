'****************************************************
'功能說明：年曆檔維護-年曆檔複製
'建立人員：MickySung
'建立日期：2015.05.18
'****************************************************
Imports System.Data

Partial Class PA_PA1A04
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then
            '年曆檔複製來源公司
            PA1.FillCompName_PA1A04(ddlSource)
            ddlSource.Items.Insert(0, New ListItem("---請選擇---", ""))

            '複製至授權公司
            PA1.subLoadCompRoleID(ddlCopyTo)
            ddlCopyTo.Items.Insert(0, New ListItem("---請選擇---", ""))
        End If
    End Sub
    Protected Overrides Sub BaseOnPageTransfer(ByVal ti As TransferInfo)
        If Not IsPostBack() Then

        End If
    End Sub
    Public Overrides Sub DoAction(ByVal Param As String)
        Select Case Param
            Case "btnPrint"   '存檔返回
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
        Dim objPA As New PA1()
        Dim CopyYear As String = txtCopyYear.Text
        Dim Source As String = ddlSource.SelectedValue
        Dim CopyTo As String = ddlCopyTo.SelectedValue
        Dim LastChgComp As String = UserProfile.ActCompID
        Dim LastChgID As String = UserProfile.ActUserID
        Dim LastChgDate As String = Now

        Dim OldData As DataTable
        OldData = objPA.checkData(txtCopyYear.Text, ddlSource.SelectedValue)
        If OldData.Rows.Count = 0 Then
            Bsp.Utility.ShowMessage(Me, "該【年曆檔複製來源公司】的【複製年度】並無資料")
            Return False
        End If

        '儲存資料
        Try
            Return objPA.CopyCalendarSetting(CopyYear, Source, CopyTo, LastChgComp, LastChgID, LastChgDate)
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, "[CC0201_99]" & Bsp.Utility.getMessage("E_00000") & Bsp.Utility.getInnerException(Me.FunID, ex))
            Return False
        End Try
    End Function

    Private Function funCheckData() As Boolean
        Dim objPA As New PA1()
        Dim beCalendar As New beCalendar.Row()
        Dim bsCalendar As New beCalendar.Service()

        '複製年度
        If txtCopyYear.Text = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblCopyYear.Text)
            txtCopyYear.Focus()
            Return False
        End If
        If IsNumeric(txtCopyYear.Text.Trim) = False Then
            Bsp.Utility.ShowMessage(Me, "｢複製年度｣請輸入數字")
            txtCopyYear.Focus()
            Return False
        End If

        '年曆檔複製來源公司
        If ddlSource.SelectedValue = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblSource.Text)
            ddlSource.Focus()
            Return False
        End If

        '複製至授權公司
        If ddlCopyTo.SelectedValue = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblCopyTo.Text)
            ddlCopyTo.Focus()
            Return False
        End If

        Return True
    End Function

    Private Sub ClearData()
        txtCopyYear.Text = ""
        ddlSource.SelectedValue = ""
        ddlCopyTo.SelectedValue = ""
    End Sub

End Class
