'****************************************************
'功能说明：银行维护-修改
'建立人员：A02976
'建立日期：2007/04/06
'****************************************************
Imports System.Data

Partial Class SC_SC0352

    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Page.SetFocus(txtBankName)
        End If
    End Sub

    Protected Overrides Sub BaseOnPageTransfer(ByVal ti As TransferInfo)
        If Not IsPostBack() Then
            GetData(ti.Args(3))
        End If
    End Sub

    Public Overrides Sub DoAction(ByVal Param As String)
        Select Case Param
            Case "btnUpdate"
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

    Private Sub GetData(ByVal strBankID As String)
        Dim objSC As New SC
        Dim strSQL As New StringBuilder
        Dim dt As DataTable

        If strBankID = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00110")
            Return
        End If

        strSQL.AppendLine("SELECT * FROM SC_Bank WHERE BankID = " & Bsp.Utility.Quote(strBankID))
        Try
            dt = Bsp.DB.ExecuteDataSet(CommandType.Text, strSQL.ToString()).Tables(0)

            With dt.Rows(0)
                lblBankID.Text = .Item("BankID")
                txtBankName.Text = .Item("BankName")
                txtWorldRank.Text = .Item("WorldRank")
                txtMoodyGrade.Text = .Item("MoodyGrade")
                lblCreateDate.Text = Convert.ToDateTime(.Item("CreateDate")).ToString("yyyy/MM/dd HH:mm:ss")
                lblLastChgDate.Text = Convert.ToDateTime(.Item("LastChgDate")).ToString("yyyy/MM/dd HH:mm:ss")
                lblLastChgID.Text = .Item("LastChgID")
            End With
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & ".GetData", ex)
        End Try
    End Sub

    Private Function SaveData() As Boolean

        Dim objSC As New SC
        Dim strSQL As New StringBuilder
        strSQL.AppendLine("UPDATE SC_Bank")
        strSQL.AppendLine("SET BankName = " & Bsp.Utility.Quote(txtBankName.Text.Trim()))
        strSQL.AppendLine("  , WorldRank = " & Bsp.Utility.Quote(txtWorldRank.Text))
        strSQL.AppendLine("  , MoodyGrade = " & Bsp.Utility.Quote(txtMoodyGrade.Text))
        strSQL.AppendLine("  , LastChgID = " & Bsp.Utility.Quote(UserProfile.UserID))
        strSQL.AppendLine("  , LastChgDate = GETDATE()")
        strSQL.AppendLine("WHERE BankID = " & Bsp.Utility.Quote(lblBankID.Text))


        Try
            Bsp.DB.ExecuteNonQuery(CommandType.Text, strSQL.ToString())
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID, ex)
            Return False
        End Try

        Return True
    End Function

    Private Function funCheckData() As Boolean
        Dim strValue As String

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
            Bsp.Utility.ShowFormatMessage(Me, "W_00040", "Moody's & S&PP評等", txtMoodyGrade.MaxLength.ToString())
            txtMoodyGrade.Focus()
            Return False
        End If
        txtMoodyGrade.Text = strValue

        Return True
    End Function
End Class