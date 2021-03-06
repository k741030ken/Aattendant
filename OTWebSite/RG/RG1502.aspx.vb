'****************************************************
'功能說明：不報到員工資料輸入-修改
'建立人員：BeatriceCheng
'建立日期：2015.08.04
'****************************************************
Imports System.Data

Partial Class RG_RG1502
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then
            txtCompID.Text = UserProfile.SelectCompRoleName
        End If
    End Sub

    Protected Overrides Sub BaseOnPageTransfer(ByVal ti As TransferInfo)
        If Not IsPostBack() Then
            Dim ht As Hashtable = Bsp.Utility.getHashTableFromParam(ti.Args)

            If ht.ContainsKey("SelectedCompID") Then
                ViewState.Item("CompID") = ht("SelectedCompID").ToString()
                ViewState.Item("RecID") = ht("SelectedRecID").ToString()
                ViewState.Item("CheckInDate") = ht("SelectedCheckInDate").ToString()
                subGetData(ViewState.Item("CompID"), ViewState.Item("RecID"), ViewState.Item("CheckInDate"))
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

    Private Sub subGetData(ByVal CompID As String, ByVal RecID As String, ByVal CheckInDate As String)
        Dim objRG1 As New RG1()

        Try
            Using dt As DataTable = objRG1.QueryRE_ContractData("CompID=" & CompID, "RecID=" & RecID, "CheckInDate=" & CheckInDate)
                If dt.Rows.Count <= 0 Then Exit Sub

                Dim dr As DataRow = dt.Rows(0)

                txtCompID.Text = dr.Item("CompID") & IIf(dr.Item("CompName") = "", "", "-" & dr.Item("CompName"))
                txtRecID.Text = dr.Item("RecID")
                txtName.Text = dr.Item("Name")
                cbCheckInFlag.Checked = IIf(dr.Item("CheckInFlag") = "1", True, False)
                txtNotCheckInRemark.Text = dr.Item("NotCheckInRemark")

                '最後異動公司
                txtChkLastChgComp.Text = dr.Item("ChkLastChgComp") & IIf(dr.Item("LastChgComp") = "", "", "-" & dr.Item("LastChgComp"))
                '最後異動者
                txtChkLastChgID.Text = dr.Item("ChkLastChgID") & IIf(dr.Item("LastChgID") = "", "", "-" & dr.Item("LastChgID"))
                '最後異動時間
                txtChkLastChgDate.Text = dr.Item("LastChgDate")

            End Using
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & "subGetData", ex)
        End Try

    End Sub

    Private Function SaveData() As Boolean
        Dim objRG1 As New RG1()

        '儲存資料
        Try
            Return objRG1.UpdateRE_ContractData( _
                "CompID=" & ViewState.Item("CompID"), _
                "RecID=" & txtRecID.Text, _
                "CheckInDate=" & ViewState.Item("CheckInDate"), _
                "CheckInFlag=" & IIf(cbCheckInFlag.Checked, "N", ""), _
                "NotCheckInRemark=" & txtNotCheckInRemark.Text, _
                "ChkLastChgComp=" & UserProfile.ActCompID, _
                "ChkLastChgID=" & UserProfile.ActUserID)

        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, "[CC0201_99]" & Bsp.Utility.getMessage("E_00000") & Bsp.Utility.getInnerException(Me.FunID, ex))
            Return False
        End Try
    End Function

    Private Function funCheckData() As Boolean

        Dim strValue As String = ""

        strValue = txtNotCheckInRemark.Text
        If strValue.Length > txtNotCheckInRemark.MaxLength Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00040", lblNotCheckInRemark.Text, txtNotCheckInRemark.MaxLength.ToString())
            txtNotCheckInRemark.Focus()
            Return False
        End If

        Return True
    End Function

    Private Sub ClearData()
        subGetData(ViewState.Item("CompID"), ViewState.Item("RecID"), ViewState.Item("CheckInDate"))
    End Sub
End Class
