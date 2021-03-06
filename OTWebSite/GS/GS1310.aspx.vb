'****************************************************
'功能說明：考核補充說明
'建立人員：Weicheng
'建立日期：2015.11.23
'****************************************************
Imports System.Data

Partial Class GS_GS1310
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then

        End If
    End Sub

    Protected Overrides Sub BaseOnPageCall(ByVal ti As TransferInfo)
        If Not IsPostBack() Then
            Dim objSC As New SC()
            Dim objHR As New HR()
            Dim ht As Hashtable = Bsp.Utility.getHashTableFromParam(ti.Args)

            If ht.ContainsKey("CompID") Then
                Using dt As DataTable = objHR.GetHREmpName(ht("Comment_SignCompID").ToString(), ht("Comment_SignID").ToString())
                    If dt.Rows.Count > 0 Then
                        lblSignName.Text = ht("Comment_SignID").ToString() & "-" & dt.Rows(0)(0).ToString()
                    End If
                End Using
                lblComment.Text = ht("Comment").ToString()
                'lblComment_Adjust.Text = ht("Comment_Adjust").ToString()
                Using dt As DataTable = objHR.GetHREmpName(ht("Comment_SignCompID1").ToString(), ht("Comment_SignID1").ToString())
                    If dt.Rows.Count > 0 Then
                        lblSignName1.Text = ht("Comment_SignID1").ToString() & "-" & dt.Rows(0)(0).ToString()
                    End If
                End Using
                lblComment1.Text = ht("Comment1").ToString()
                'lblComment_Adjust1.Text = ht("Comment_Adjust1").ToString()
                If ht("MainFlag").ToString() = "1" And ht("IsSignNext").ToString() = "N" Then
                    Bsp.Utility.RunClientScript(Me.Page, "hide_tr('DeptBoss');")
                End If
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

End Class
