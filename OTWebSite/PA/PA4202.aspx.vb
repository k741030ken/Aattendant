'****************************************************
'功能說明：Web我們的同仁組織圖列印-修改
'建立人員：BeatriceCheng
'建立日期：2015.05.12
'****************************************************
Imports System.Data

Partial Class PA_PA4202
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
                ViewState.Item("EmpID") = ht("SelectedEmpID").ToString()
                subGetData(ViewState.Item("CompID"), ViewState.Item("EmpID"))
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
        Dim beExOrganPrint As New beExOrganPrint.Row()
        Dim bsExOrganPrint As New beExOrganPrint.Service()
        Dim objPA4 As New PA4()

        '取得輸入資料
        beExOrganPrint.CompID.Value = txtComID.Text
        beExOrganPrint.EmpID.Value = txtEmpID.Text
        beExOrganPrint.OrganPrintFlag.Value = ddlOrganPrintFlag.SelectedValue
        beExOrganPrint.PrintType.Value = ddlPrintType.SelectedValue

        beExOrganPrint.LastChgComp.Value = UserProfile.ActCompID
        beExOrganPrint.LastChgID.Value = UserProfile.ActUserID
        beExOrganPrint.LastChgDate.Value = Now

        '儲存資料
        Try
            Return objPA4.ExOrganPrintUpdate(beExOrganPrint)
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, "[CC0201_99]" & Bsp.Utility.getMessage("E_00000") & Bsp.Utility.getInnerException(Me.FunID, ex))
            Return False
        End Try
    End Function

    Private Sub subGetData(ByVal CompID As String, ByVal EmpID As String)
        Dim objSC As New SC
        Dim beExOrganPrint As New beExOrganPrint.Row()
        Dim bsExOrganPrint As New beExOrganPrint.Service()
        Dim objPA3 As New PA3()

        beExOrganPrint.CompID.Value = CompID
        beExOrganPrint.EmpID.Value = EmpID
        Try
            Using dt As DataTable = bsExOrganPrint.QueryByKey(beExOrganPrint).Tables(0)
                If dt.Rows.Count <= 0 Then Exit Sub
                beExOrganPrint = New beExOrganPrint.Row(dt.Rows(0))

                txtComID.Text = beExOrganPrint.CompID.Value
                txtComName.Text = objSC.GetSC_CompName(beExOrganPrint.CompID.Value)
                txtEmpID.Text = beExOrganPrint.EmpID.Value
                txtEmpName.Text = objSC.GetSC_UserName(beExOrganPrint.CompID.Value, beExOrganPrint.EmpID.Value)
                ddlOrganPrintFlag.SelectedValue = beExOrganPrint.OrganPrintFlag.Value
                ddlPrintType.SelectedValue = beExOrganPrint.PrintType.Value

                '最後異動公司
                Dim CompName As String = objSC.GetSC_CompName(beExOrganPrint.LastChgComp.Value)
                txtLastChgComp.Text = beExOrganPrint.LastChgComp.Value + IIf(CompName <> "", "-" + CompName, "")
                '最後異動人員
                Dim UserName As String = objSC.GetSC_UserName(beExOrganPrint.LastChgComp.Value, beExOrganPrint.LastChgID.Value)
                txtLastChgID.Text = beExOrganPrint.LastChgID.Value + IIf(UserName <> "", "-" + UserName, "")
                '最後異動日期
                Dim boolDate As Boolean = Format(beExOrganPrint.LastChgDate.Value, "yyyy/MM/dd") = "1900/01/01"
                txtLastChgDate.Text = IIf(boolDate, "", beExOrganPrint.LastChgDate.Value.ToString("yyyy/MM/dd HH:mm:ss"))
            End Using
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & "subGetData", ex)
        End Try

    End Sub

    Private Function funCheckData() As Boolean

        Dim strValue As String = ""

        '權限註記
        strValue = ddlOrganPrintFlag.SelectedValue
        If strValue.Trim = "" Or strValue = "---請選擇---" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblOrganPrintFlag.Text)
            ddlOrganPrintFlag.Focus()
            Return False
        End If

        '列印範圍
        strValue = ddlPrintType.SelectedValue
        If strValue.Trim = "" Or strValue = "---請選擇---" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblPrintType.Text)
            ddlPrintType.Focus()
            Return False
        End If

        Return True
    End Function

    Private Sub ClearData()
        subGetData(ViewState.Item("CompID"), ViewState.Item("EmpID"))
    End Sub

End Class
