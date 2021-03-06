'****************************************************
'功能說明：業績資料
'建立人員：Micky Sung
'建立日期：2015.11.03
'****************************************************
Imports System.Data

Partial Class GS_GS1140
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
                lblEPyear.Text = ht("EPYear").ToString()

                title.Text = "員工[" + objSC.GetSC_UserName(hidCompID.Value, hidEmpID.Value) + "]" + lblEPyear.Text + "年度業績資料："
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
        Dim EmpPerformanceTable As DataTable
        Dim objGS As New GS1()

        EmpPerformanceTable = objGS.QueryEmpPerformance(hidCompID.Value, hidEmpID.Value, lblEPyear.Text)
        If EmpPerformanceTable.Rows().Count <> 0 Then
            Dim lblPerformance As Label
            For i = 0 To 35
                lblPerformance = FindControl("lblPerformance_" & i)
                lblPerformance.Text = EmpPerformanceTable.Rows(0).Item(i).ToString()
            Next
        End If
    End Sub

End Class
