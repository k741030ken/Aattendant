'****************************************************
'功能說明：獎懲資料
'建立人員：Micky Sung
'建立日期：2015.11.03
'****************************************************
Imports System.Data

Partial Class GS_GS1130
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
                hidValidYear.Value = ht("ValidYear").ToString()

                title.Text = "員工[" + objSC.GetSC_UserName(hidCompID.Value, hidEmpID.Value) + "]" + hidValidYear.Value + "年度獎懲紀錄："
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

        strSQL.AppendLine(" SELECT ")
        strSQL.AppendLine(" ISNULL(P.Name, '') AS Name, CONVERT(char(10), R.ValidDate, 111) AS ValidDate ")
        strSQL.AppendLine(" , R.DeptName, R.Reason, R.Kind, R.RecordCnt ")
        strSQL.AppendLine(" FROM EmpReward R ")
        strSQL.AppendLine(" LEFT JOIN Personal P ON P.CompID = R.CompID AND P.EmpID = R.EmpID ")
        strSQL.AppendLine(" WHERE ")
        strSQL.AppendLine(" R.CompID = " & Bsp.Utility.Quote(hidCompID.Value))
        strSQL.AppendLine(" AND year(R.ValidDate) = " & Bsp.Utility.Quote(hidValidYear.Value))
        strSQL.AppendLine(" AND R.EmpID = " & Bsp.Utility.Quote(hidEmpID.Value))
        strSQL.AppendLine(" ORDER BY R.ValidDate ")

        Using dt As DataTable = Bsp.DB.ExecuteDataSet(CommandType.Text, strSQL.ToString(), "eHRMSDB").Tables(0)
            gvMain.DataSource = dt
            gvMain.DataBind()
        End Using

    End Sub

End Class
