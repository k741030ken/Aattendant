'****************************************************
'功能說明：Web我們的同仁組織色塊設定-多筆修改
'建立人員：BeatriceCheng
'建立日期：2015.05.19
'****************************************************
Imports System.Data

Partial Class PA_PA4403
    Inherits PageBase


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then

        End If
    End Sub
    Protected Overrides Sub BaseOnPageTransfer(ByVal ti As TransferInfo)
        If Not IsPostBack() Then
            Dim ht As Hashtable = Bsp.Utility.getHashTableFromParam(ti.Args)

            If ht.ContainsKey("SelectedCompID") Then
                ViewState.Item("CompID") = ht("SelectedCompID").ToString()
                ViewState.Item("SortOrder") = ht("SelectedSortOrder").ToString()
                subGetData(ViewState.Item("CompID"), ViewState.Item("SortOrder"))
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
        Dim beOrganColor_Web As New beOrganColor_Web.Row()
        Dim bsOrganColor_Web As New beOrganColor_Web.Service()
        Dim objPA4 As New PA4()

        '取得輸入資料
        beOrganColor_Web.CompID.Value = txtComID.Text
        beOrganColor_Web.Color.OldValue = txtOldColor.Text

        If hldColor.Value = "" Then hldColor.Value = "#F0F8FF"
        beOrganColor_Web.Color.Value = hldColor.Value

        beOrganColor_Web.LastChgComp.Value = UserProfile.ActCompID
        beOrganColor_Web.LastChgID.Value = UserProfile.ActUserID
        beOrganColor_Web.LastChgDate.Value = Now

        '儲存資料
        Try
            Return objPA4.OrganColor_WebMutiUpdate(beOrganColor_Web)
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, "[CC0201_99]" & Bsp.Utility.getMessage("E_00000") & Bsp.Utility.getInnerException(Me.FunID, ex))
            Return False
        End Try
    End Function

    Private Sub subGetData(ByVal CompID As String, ByVal SortOrder As String)
        Dim objSC As New SC
        Dim beOrganColor_Web As New beOrganColor_Web.Row()
        Dim bsOrganColor_Web As New beOrganColor_Web.Service()

        beOrganColor_Web.CompID.Value = CompID
        beOrganColor_Web.SortOrder.Value = SortOrder
        Try
            Using dt As DataTable = bsOrganColor_Web.QueryByKey(beOrganColor_Web).Tables(0)
                If dt.Rows.Count <= 0 Then Exit Sub
                beOrganColor_Web = New beOrganColor_Web.Row(dt.Rows(0))

                txtComID.Text = beOrganColor_Web.CompID.Value
                txtComName.Text = objSC.GetSC_CompName(beOrganColor_Web.CompID.Value)
                txtOldColor.Text = beOrganColor_Web.Color.Value
                txtNewColor.Text = beOrganColor_Web.Color.Value
                hldColor.Value = beOrganColor_Web.Color.Value

                divOldColor.BackColor = Drawing.ColorTranslator.FromHtml(beOrganColor_Web.Color.Value)

                '最後異動公司
                Dim CompName As String = objSC.GetSC_CompName(beOrganColor_Web.LastChgComp.Value)
                txtLastChgComp.Text = beOrganColor_Web.LastChgComp.Value + IIf(CompName <> "", "-" + CompName, "")
                '最後異動人員
                Dim UserName As String = objSC.GetSC_UserName(beOrganColor_Web.LastChgComp.Value, beOrganColor_Web.LastChgID.Value)
                txtLastChgID.Text = beOrganColor_Web.LastChgID.Value + IIf(UserName <> "", "-" + UserName, "")
                '最後異動日期
                Dim boolDate As Boolean = Format(beOrganColor_Web.LastChgDate.Value, "yyyy/MM/dd") = "1900/01/01"
                txtLastChgDate.Text = IIf(boolDate, "", beOrganColor_Web.LastChgDate.Value.ToString("yyyy/MM/dd HH:mm:ss"))
            End Using
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & "subGetData", ex)
        End Try

    End Sub

    Private Function funCheckData() As Boolean

        Dim strValue As String = ""

        Return True
    End Function

    Private Sub ClearData()
        subGetData(ViewState.Item("CompID"), ViewState.Item("SortOrder"))
    End Sub

End Class
