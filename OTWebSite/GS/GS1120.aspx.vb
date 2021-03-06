'****************************************************
'功能說明：考核表綜合量
'建立人員：Micky Sung
'建立日期：2015.11.02
'****************************************************
Imports System.Data

Partial Class GS_GS1120
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then

        End If
    End Sub

    Protected Overrides Sub BaseOnPageCall(ByVal ti As TransferInfo)
        If Not IsPostBack() Then
            Dim objSC As New SC()
            Dim ht As Hashtable = Bsp.Utility.getHashTableFromParam(ti.Args)

            If ht.ContainsKey("CompID") Then
                hidCompID.Value = ht("CompID").ToString()
                hidEmpID.Value = ht("EmpID").ToString()
                hidEvaluationYear.Value = ht("EvaluationYear").ToString()
                hidEvaluationSeq.Value = ht("EvaluationSeq").ToString()

                title.Text = "員工[" + objSC.GetSC_UserName(hidCompID.Value, hidEmpID.Value) + "]" + hidEvaluationYear.Value + "半年度績效考核-單位主管綜合評量："
                GetData()
            Else
                Return
            End If
        End If
    End Sub

    Public Overrides Sub DoAction(ByVal Param As String)
        Select Case Param
            Case "btnActionX"    '返回
                Bsp.Utility.RunClientScript(Me, "window.top.close();")
        End Select
    End Sub

    Private Sub GetData()
        Dim strSQL As New StringBuilder()

        strSQL.AppendLine(" SELECT Top 1 ")
        strSQL.AppendLine(" ISNULL(EA.Content, '') AS Content, ")
        strSQL.AppendLine(" CONVERT(nchar(19), ISNULL(ES.SignTime, ''), 120) AS SignTime, ")
        strSQL.AppendLine(" ISNULL(ES.SignIDName, '') AS SignIDName ")
        strSQL.AppendLine(" FROM EvaluationSignLog ES ")
        strSQL.AppendLine(" INNER JOIN EvaluationCommentAssess EA ON ES.CompID = EA.CompID AND ES.EvaluationYear = EA.EvaluationYear AND ES.EvaluationSeq = EA.EvaluationSeq AND ES.Seq = EA.Seq AND ES.ApplyID = EA.ApplyID ")
        strSQL.AppendLine(" INNER JOIN EvaluationCommentH EC ON EC.CompID = EA.CompID AND EC.EvaluationYear = EA.EvaluationYear AND EC.EvaluationSeq = EA.EvaluationSeq AND EC.Seq = EA.CommentSeq ")
        strSQL.AppendLine(" WHERE ")
        strSQL.AppendLine(" ES.CompID = " & Bsp.Utility.Quote(hidCompID.Value))
        strSQL.AppendLine(" AND ES.EvaluationYear = " & Bsp.Utility.Quote(hidEvaluationYear.Value))
        strSQL.AppendLine(" AND ES.EvaluationSeq = " & Bsp.Utility.Quote(hidEvaluationSeq.Value))
        strSQL.AppendLine(" AND ES.Result = '1' ")
        strSQL.AppendLine(" AND StepID = '3' ")
        strSQL.AppendLine(" AND IdentityID = '4' ")
        strSQL.AppendLine(" AND EC.Type LIKE '%0%' ")
        strSQL.AppendLine(" AND ES.ApplyID = " & Bsp.Utility.Quote(hidEmpID.Value))
        strSQL.AppendLine(" AND ES.Seq = (SELECT Max(Seq) FROM EvaluationSignLog WHERE CompID = ES.CompID AND EvaluationYear = ES.EvaluationYear AND EvaluationSeq = ES.EvaluationSeq AND ApplyID = ES.ApplyID AND Result = '1' AND StepID = '3' AND IdentityID = '4') ")
        strSQL.AppendLine(" ORDER BY CommentSeq DESC ")

        Using dt As DataTable = Bsp.DB.ExecuteDataSet(CommandType.Text, strSQL.ToString(), "eHRMSDB").Tables(0)
            gvMain.DataSource = dt
            gvMain.DataBind()
        End Using

    End Sub
End Class
