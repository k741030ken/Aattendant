'****************************************************
'功能说明：银行维护-新增
'建立人员：A02976
'建立日期：2007/04/06
'****************************************************
Imports System.Data

Partial Class SC_SC0351
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Page.SetFocus(txtBankID)
        End If
    End Sub

    Public Overrides Sub DoAction(ByVal Param As String)
        Select Case Param
            Case "btnAdd"
                If funCheckData() Then
                    If SaveData() Then GoBack()
                End If
            Case "btnActionX"
                GoBack()
        End Select
    End Sub

    Private Sub GoBack()
        Dim ti As TransferInfo = Me.StateTransfer
        Me.TransferFramePage(ti.CallerUrl, Nothing, ti.Args)
    End Sub

    Private Function SaveData() As Boolean
        Dim objSC As New SC
        Dim strSQL As New StringBuilder
        strSQL.AppendLine("INSERT INTO SC_Bank (BankID, BankName, WorldRank, MoodyGrade, CreateDate, LastChgID, LastChgDate)")
        strSQL.AppendLine("SELECT " & Bsp.Utility.Quote(txtBankID.Text))
        strSQL.AppendLine("     , " & Bsp.Utility.Quote(txtBankName.Text))
        strSQL.AppendLine("     , " & IIf(txtWorldRank.Text.Trim = "", "NULL", Bsp.Utility.Quote(txtWorldRank.Text)))
        strSQL.AppendLine("     , " & Bsp.Utility.Quote(txtMoodyGrade.Text))
        strSQL.AppendLine("     , GETDATE()")
        strSQL.AppendLine("     , " & Bsp.Utility.Quote(UserProfile.UserID))
        strSQL.AppendLine("     , GETDATE()")

        Try
            Bsp.DB.ExecuteNonQuery(CommandType.Text, strSQL.ToString())
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & ".SaveData", ex)
            Return False
        End Try

        Return True
    End Function

    Private Function funCheckData() As Boolean
        Dim strValue As String

        strValue = txtBankID.Text.ToString()
        If strValue = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", "銀行代號")
            txtBankID.Focus()
            Return False
        Else
            If Bsp.Utility.getStringLength(strValue) > txtBankID.MaxLength Then
                Bsp.Utility.ShowFormatMessage(Me, "W_00040", "銀行代號", txtBankID.MaxLength.ToString())
                txtBankID.Focus()
                Return False
            End If
            txtBankID.Text = strValue
        End If

        strValue = txtBankName.Text.ToString()
        If strValue = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", "銀行名稱")
            txtBankName.Focus()
            Return False
        Else
            If Bsp.Utility.getStringLength(strValue) > txtBankName.MaxLength Then
                Bsp.Utility.ShowFormatMessage(Me, "W_00040", "銀行名稱", txtBankName.MaxLength.ToString())
                txtBankName.Focus()
                Return False
            End If
            txtBankName.Text = strValue
        End If

        strValue = txtWorldRank.Text.ToString()
        If Bsp.Utility.getStringLength(strValue) > txtWorldRank.MaxLength Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00040", "世界排名", txtWorldRank.MaxLength.ToString())
            txtWorldRank.Focus()
            Return False
        End If
        txtWorldRank.Text = strValue

        strValue = txtMoodyGrade.Text.ToString()
        If Bsp.Utility.getStringLength(strValue) > txtMoodyGrade.MaxLength Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00040", "Moody's & S&P評等", txtMoodyGrade.MaxLength.ToString())
            txtMoodyGrade.Focus()
            Return False
        End If
        txtMoodyGrade.Text = strValue

        If Bsp.DB.ExecuteScalar("SELECT COUNT(*) FROM SC_Bank WHERE BankID = " & Bsp.Utility.Quote(txtBankID.Text.Trim()) & " OR BankName = " & Bsp.Utility.Quote(txtBankName.Text.Trim())) > 0 Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00010", "銀行")
            Return False
        End If

        Return True
    End Function
End Class