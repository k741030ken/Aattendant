'****************************************************
'功能說明：查詢畫面顯示
'建立人員：A02976
'建立日期：2007/06/15
'****************************************************
Imports System.Data
Imports System.Data.SqlClient
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Common

Partial Class SC_SC0092
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'If txtSQL.Text.Trim() <> "" Then
        '    sdsMain.SelectCommand = CType(StateMain, String)
        'Else
        '    StateMain = ""
        'End If
    End Sub

    Public Overrides Sub DoAction(ByVal Param As String)
        Select Case Param
            Case "btnAdd"       '新增
                DoAdd()
            Case "btnUpdate"    '修改
                DoUpdate()
            Case "btnQuery"     '查詢
                DoQuery()
            Case "btnDelete"    '刪除
                DoDelete()
            Case Else
                DoOtherAction()   '其他功能動作
        End Select
    End Sub

    Private Sub DoAdd()

    End Sub

    Private Sub DoUpdate()

    End Sub

    Private Sub DoQuery()

    End Sub

    Private Sub DoDelete()

    End Sub

    Private Sub DoOtherAction()

    End Sub

    Protected Sub btnQuery_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnQuery.Click
        '判斷是否是Update或是Insert，則執行另一段
        Dim strSQL As String = funGetSQL(txtSQL.Text.Trim())
        Dim connStr As String = ""
        Dim cn As SqlClient.SqlConnection

        If strSQL = "" Then
            Bsp.Utility.ShowMessage(Me, "未輸入查詢字串！")
            Return
        End If

        If Session("sys_ConnectionObject") Is Nothing Then
            connStr = DatabaseFactory.CreateDatabase.CreateConnection.ConnectionString
            cn = DatabaseFactory.CreateDatabase.CreateConnection
        Else
            connStr = CType(Session("sys_ConnectionObject"), SqlClient.SqlConnection).ConnectionString
            cn = CType(Session("sys_ConnectionObject"), SqlClient.SqlConnection)
        End If

        If strSQL.ToString.ToUpper.IndexOf("DELETE ") >= 0 Or _
           strSQL.ToString.ToUpper.IndexOf("INSERT ") >= 0 Or _
           strSQL.ToString.ToUpper.IndexOf("UPDATE ") >= 0 Or _
           strSQL.ToString.ToUpper.IndexOf("EXEC ") >= 0 Then
            subRunScript(cn)
            Exit Sub
        End If

        Try
            pcMain.DataTable = MyDB.gfunExecuteQuery(strSQL, cn).Tables(0)
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, ex.ToString())
        End Try
        'pcMain.BindGridView()
        'StateMain = strSQL
        'sdsMain.ConnectionString = connStr
        'sdsMain.SelectCommand = strSQL

        'gvMain.DataBind()
    End Sub

    Private Sub subRunScript(ByVal cn As SqlConnection)
        Dim strSQL As String = funGetSQL(txtSQL.Text.Trim())
        Try
            If cn.State = ConnectionState.Closed Then cn.Open()
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, "subRunScript_1", ex)
            Return
        End Try

        Dim objTxn As SqlTransaction = cn.BeginTransaction

        Try
            MyDB.gfunExecuteNonQuery(strSQL, objTxn)
            objTxn.Commit()
            Bsp.Utility.ShowMessage(Me, "指令執行成功！")
        Catch ex As Exception
            objTxn.Rollback()
            Bsp.Utility.ShowMessage(Me, "subRunScript_2", ex)
        Finally
            objTxn.Dispose()
        End Try
    End Sub

    Private Function funGetSQL(ByVal strOSQL As String) As String
        Dim arySQL() As String = Split(strOSQL.Trim, vbCrLf)
        Dim intLoop As Integer
        Dim strSQL As String = ""
        Dim bolMark As Boolean = False

        For intLoop = 0 To arySQL.GetUpperBound(0)
            If arySQL(intLoop).ToString.Trim.Length > 0 Then
                If Not bolMark Then
                    If arySQL(intLoop).ToString().Trim().Length >= 2 Then
                        If arySQL(intLoop).ToString().Trim().Substring(0, 2) = "/*" Then
                            bolMark = True
                        Else
                            If arySQL(intLoop).ToString().Trim().Substring(0, 2) <> "--" Then
                                strSQL = strSQL & arySQL(intLoop).ToString().Trim() & vbCrLf
                            End If
                        End If
                    Else
                        strSQL = strSQL & arySQL(intLoop).ToString().Trim() & vbCrLf
                    End If
                Else
                    If Right(arySQL(intLoop).Trim(), 2) = "*/" Then
                        bolMark = False
                    End If
                End If
            End If
        Next
        Return strSQL
    End Function

    Protected Sub gvMain_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvMain.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            For intLoop As Integer = 0 To e.Row.Cells.Count - 1
                e.Row.Cells(intLoop).Attributes.Add("class", "td_detail")
                e.Row.Cells(intLoop).Style.Add("height", "15px")
            Next
        ElseIf e.Row.RowType = DataControlRowType.Header Then
            For intLoop As Integer = 0 To e.Row.Cells.Count - 1
                e.Row.Cells(intLoop).Attributes.Add("class", "td_header")
                e.Row.Cells(intLoop).Style.Add("height", "16px")
            Next
        End If
    End Sub
End Class
