'****************************************************
'功能說明：結案退一關處理
'建立/轉移人員：A02976 / Tsao
'建立日期：2008/07/15  /  20130913
'' 20120906 (U) 104751 201208230023 申請企審系統[結案退關]流程中,開放[覆審結案]退回前一關卡項目之權限
'****************************************************
Imports System.Data

Partial Class WF_WFA091
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then
        End If
    End Sub

    Protected Overrides Sub BaseOnPageCall(ByVal ti As TransferInfo)
        Dim ht As Hashtable = Bsp.Utility.getHashTableFromParam(ti.Args)

        For Each strKey As String In ht.Keys
            Dim ctl As Control = Me.Form.FindControl("lbl" & strKey)

            If ctl IsNot Nothing Then
                CType(ctl, Label).Text = ht(strKey)
            End If
        Next
    End Sub

    Public Overrides Sub DoAction(ByVal Param As String)
        Select Case Param
            Case "btnUpdate"
                If funCheckData() Then
                    If SaveData() Then
                        GoBack()
                    End If
                End If
                'Case "btnActionX"
                '    GoBack()
        End Select
    End Sub

    Private Sub GoBack()
        'Dim ti As TransferInfo = Me.StateTransfer
        'Me.TransferFramePage(ti.CallerUrl, Nothing, ti.Args)
    End Sub

    Private Function SaveData() As Boolean
        Dim objWF As New WF
        'Dim objAP As New AP()
        Dim strFlowStepID As String = lblFlowTypeNm.Text.Split("-")(0)
        Dim strID As String = ""

        Select Case strFlowStepID
            Case "S999"
                strID = lblAppID.Text
            Case "CC99"
                strID = lblCCID.Text
            Case "CC98"
                strID = lblCCID.Text
                'Case "RV99", "RV98"
                '    strID = lblCRID.Text
                'Case "RA99"
                '    strID = lblRAID.Text
        End Select

        Try
            objWF.ExecuteBackStep(strFlowStepID, strID, txtRemark.Text)
            'If Bsp.Utility.InStr(strFlowStepID, "CC99", "CC98") Then
            '    objWF.DeleteCCPDF(lblAppID.Text, lblCustomer.Text.Trim().Split("-")(0))
            'End If
            Bsp.Utility.RunClientScript(Me, "funClose(""退關卡作業完成！"");")
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & ".SaveData", ex, lblAppID.Text, txtRemark.Text)
        End Try
    End Function

    Private Function funCheckData() As Boolean
        Dim strValue As String = txtRemark.Text.Trim()

        If strValue = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblRemark.Text)
            txtRemark.Focus()
            Return False
        Else
            If strValue.Length > txtRemark.MaxLength Then
                Bsp.Utility.ShowFormatMessage(Me, "W_00040", lblRemark.Text, txtRemark.MaxLength.ToString())
                txtRemark.Focus()
                Return False
            End If
            txtRemark.Text = strValue
        End If
        Return True
    End Function
End Class
