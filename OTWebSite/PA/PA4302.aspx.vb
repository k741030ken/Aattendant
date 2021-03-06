'****************************************************
'功能說明：Web功能權限維護-明細
'建立人員：BeatriceCheng
'建立日期：2015.05.13
'****************************************************
Imports System.Data

Partial Class PA_PA4302
    Inherits PageBase


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then
            Dim objSC As New SC
            txtComID.Text = UserProfile.SelectCompRoleName
            PA4.FillDDL(ddlWebID, "FormWeb", "WebID", "WebName", PA4.DisplayType.Full)
        End If
    End Sub
    Protected Overrides Sub BaseOnPageTransfer(ByVal ti As TransferInfo)
        If Not IsPostBack() Then
            Dim ht As Hashtable = Bsp.Utility.getHashTableFromParam(ti.Args)

            If ht.ContainsKey("SelectedCompID") Then
                ViewState.Item("CompID") = ht("SelectedCompID").ToString()
                ViewState.Item("WebID") = ht("SelectedWebID").ToString()
                subGetData(ViewState.Item("CompID"), ViewState.Item("WebID"))
            Else
                Return
            End If
        End If
    End Sub
    Public Overrides Sub DoAction(ByVal Param As String)
        Select Case Param
            Case "btnActionX"   '返回
                GoBack()
        End Select
    End Sub

    Private Sub GoBack()
        Dim ti As TransferInfo = Me.StateTransfer
        Me.TransferFramePage(ti.CallerUrl, Nothing, ti.Args)
    End Sub

    Private Sub subGetData(ByVal CompID As String, ByVal WebID As String)
        Dim objSC As New SC
        Dim beUserFormWeb As New beUserFormWeb.Row()
        Dim bsUserFormWeb As New beUserFormWeb.Service()
        Dim objPA3 As New PA3()

        beUserFormWeb.CompID.Value = CompID
        beUserFormWeb.WebID.Value = WebID
        Try
            Using dt As DataTable = bsUserFormWeb.QueryByKey(beUserFormWeb).Tables(0)
                If dt.Rows.Count <= 0 Then Exit Sub
                beUserFormWeb = New beUserFormWeb.Row(dt.Rows(0))

                txtComID.Text = beUserFormWeb.CompID.Value
                txtComName.Text = objSC.GetSC_CompName(beUserFormWeb.CompID.Value)
                ddlWebID.SelectedValue = beUserFormWeb.WebID.Value

                '最後異動公司
                Dim CompName As String = objSC.GetSC_CompName(beUserFormWeb.LastChgComp.Value)
                txtLastChgComp.Text = beUserFormWeb.LastChgComp.Value + IIf(CompName <> "", "-" + CompName, "")
                '最後異動人員
                Dim UserName As String = objSC.GetSC_UserName(beUserFormWeb.LastChgComp.Value, beUserFormWeb.LastChgID.Value)
                txtLastChgID.Text = beUserFormWeb.LastChgID.Value + IIf(UserName <> "", "-" + UserName, "")
                '最後異動日期
                Dim boolDate As Boolean = Format(beUserFormWeb.LastChgDate.Value, "yyyy/MM/dd") = "1900/01/01"
                txtLastChgDate.Text = IIf(boolDate, "", beUserFormWeb.LastChgDate.Value.ToString("yyyy/MM/dd HH:mm:ss"))
            End Using
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & "subGetData", ex)
        End Try

    End Sub

End Class
