'****************************************************
'功能說明：維護/檢視關卡選取後的執行SQL
'建立人員：Chung
'建立日期：2013/10/25
'****************************************************
Imports System.Data
Imports Newtonsoft.Json

Partial Class SC_SC0413
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then
        End If
    End Sub

    Protected Overrides Sub BaseOnPageCall(ByVal ti As TransferInfo)
        Dim ht As Hashtable = CType(ti.Args(0), Hashtable)

        For Each strKey As String In ht.Keys
            Select Case strKey
                Case "SeqNo"
                    ViewState.Item(strKey) = ht(strKey).ToString()
                Case "AfterSQL"
                    txtAfterSQL.Text = ht(strKey).ToString()
                Case "ButtonName"
                    lblButtonName.Text = ht(strKey).ToString()
                Case "Action"
                    If ht(strKey).ToString() = "Edit" Then
                        txtAfterSQL.ReadOnly = False
                    End If
            End Select
        Next
    End Sub

    Public Overrides Sub DoAction(ByVal Param As String)
        Select Case Param
            Case "btnUpdate"    '驗證後離開
                If ValidateSQL(txtAfterSQL.Text) Then
                    Dim returnValue As New Dictionary(Of String, String)

                    returnValue.Add("SeqNo", Bsp.Utility.IsStringNull(ViewState.Item("SeqNo")))
                    returnValue.Add("AfterSQL", txtAfterSQL.Text.Trim())

                    GoBack(JsonConvert.SerializeObject(returnValue))
                End If
            Case "btnActionC" '驗證SQL
                ValidateSQL(txtAfterSQL.Text)
            Case "btnDelete" '清除後離開
                txtAfterSQL.Text = ""
                Dim returnValue As New Dictionary(Of String, String)

                returnValue.Add("SeqNo", Bsp.Utility.IsStringNull(ViewState.Item("SeqNo")))
                returnValue.Add("AfterSQL", txtAfterSQL.Text)

                GoBack(JsonConvert.SerializeObject(returnValue))

            Case "btnActionX"
                GoBack("")
        End Select
    End Sub

    Private Sub GoBack(ByVal ReturnValue As String)
        Bsp.Utility.RunClientScript(Me, "window.top.returnValue='" & Replace(ReturnValue, "'", "\'") & "';window.top.close();")
    End Sub

    '驗證SQL
    Private Function ValidateSQL(ByVal SQL As String) As Boolean
        SQL = SQL.Replace(vbCrLf, " ").Replace(vbTab, " ")

        If SQL.Trim() = "" Then
            Bsp.Utility.ShowMessage(Me, "未輸入SQL!")
            Return False
        End If

        Dim strValidateSQL As String = ""
        Dim bolLeftOpen As Boolean = False

        For intLoop As Integer = 0 To SQL.Length - 1
            Dim a As String = SQL.Substring(intLoop, 1)

            If bolLeftOpen Then
                If a = " " Then
                    strValidateSQL &= "' "
                    bolLeftOpen = False
                Else
                    strValidateSQL &= a
                End If
            Else
                If a = "@" Then
                    strValidateSQL &= "'" & a
                    bolLeftOpen = True
                Else
                    strValidateSQL &= a
                End If
            End If
        Next
        If bolLeftOpen Then strValidateSQL &= "'"
        strValidateSQL = "Set Parseonly on" & vbCrLf & strValidateSQL & vbCrLf & "Set Parseonly off"
        Try
            Bsp.DB.ExecuteNonQuery(CommandType.Text, strValidateSQL)
            Bsp.Utility.ShowMessage(Me, "語法驗證OK!")
            Return True
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, "語法驗證失敗!" & vbCrLf & ex.ToString())
            Return False
        End Try
    End Function

    Private Sub GoBack()
        Dim ti As TransferInfo = Me.StateTransfer
        Me.TransferFramePage(ti.CallerUrl, Nothing, ti.Args)
    End Sub

    Private Function SaveData() As Boolean

    End Function

    Private Function funCheckData() As Boolean
        Dim strValue As String

        strValue = txtAfterSQL.Text.Trim()
        If strValue = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", txtAfterSQL.Text)
            txtAfterSQL.Focus()
            Return False
        Else
            If strValue.Length > Bsp.Utility.getStringLength(txtAfterSQL.Text) Then
                Bsp.Utility.ShowFormatMessage(Me, "W_00040", txtAfterSQL.Text, txtAfterSQL.MaxLength.ToString())
                txtAfterSQL.Focus()
                Return False
            End If
            txtAfterSQL.Text = strValue
        End If
        Return True
    End Function
End Class
