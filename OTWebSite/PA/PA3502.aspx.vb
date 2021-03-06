'****************************************************
'功能說明：科系設定-修改
'建立人員：BeatriceCheng
'建立日期：2015.05.07
'****************************************************
Imports System.Data

Partial Class PA_PA3502
    Inherits PageBase


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then

        End If
    End Sub

    Protected Overrides Sub BaseOnPageTransfer(ByVal ti As TransferInfo)
        If Not IsPostBack() Then
            Dim ht As Hashtable = Bsp.Utility.getHashTableFromParam(ti.Args)

            If ht.ContainsKey("SelectedDepartID") Then
                ViewState.Item("DepartID") = ht("SelectedDepartID").ToString()
                subGetData(ht("SelectedDepartID").ToString())
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

    Private Function SaveData() As Boolean
        Dim beDepart As New beDepart.Row()
        Dim bsDepart As New beDepart.Service()
        Dim objPA3 As New PA3()

        '取得輸入資料
        beDepart.DepartID.Value = txtDepartID.Text
        beDepart.Remark.Value = txtRemark.Text

        beDepart.LastChgComp.Value = UserProfile.ActCompID
        beDepart.LastChgID.Value = UserProfile.ActUserID
        beDepart.LastChgDate.Value = Now

        '儲存資料
        Try
            Return objPA3.DepartUpdate(beDepart)
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, "[CC0201_99]" & Bsp.Utility.getMessage("E_00000") & Bsp.Utility.getInnerException(Me.FunID, ex))
            Return False
        End Try
    End Function

    Private Sub subGetData(ByVal DepartID As String)
        Dim objSC As New SC
        Dim beDepart As New beDepart.Row()
        Dim bsDepart As New beDepart.Service()
        Dim objPA3 As New PA3()

        beDepart.DepartID.Value = DepartID
        Try
            Using dt As DataTable = bsDepart.QueryByKey(beDepart).Tables(0)
                If dt.Rows.Count <= 0 Then Exit Sub
                beDepart = New beDepart.Row(dt.Rows(0))

                txtDepartID.Text = beDepart.DepartID.Value
                txtRemark.Text = beDepart.Remark.Value

                '最後異動公司
                Dim CompName As String = objSC.GetSC_CompName(beDepart.LastChgComp.Value)
                txtLastChgComp.Text = beDepart.LastChgComp.Value + IIf(CompName <> "", "-" + CompName, "")
                '最後異動人員
                Dim UserName As String = objSC.GetSC_UserName(beDepart.LastChgComp.Value, beDepart.LastChgID.Value)
                txtLastChgID.Text = beDepart.LastChgID.Value + IIf(UserName <> "", "-" + UserName, "")
                '最後異動日期
                Dim boolDate As Boolean = Format(beDepart.LastChgDate.Value, "yyyy/MM/dd") = "1900/01/01"
                txtLastChgDate.Text = IIf(boolDate, "", beDepart.LastChgDate.Value.ToString("yyyy/MM/dd HH:mm:ss"))
            End Using
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & "subGetData", ex)
        End Try

    End Sub

    Private Function funCheckData() As Boolean
        Dim strValue As String = ""

        '科系名稱
        strValue = txtRemark.Text
        If strValue.Trim = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblRemark.Text)
            txtRemark.Focus()
            Return False
        End If
        If strValue.Length > txtRemark.MaxLength Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00040", lblRemark.Text, txtRemark.MaxLength.ToString())
            txtRemark.Focus()
            Return False
        End If

        Return True
    End Function

    Private Sub ClearData()
        subGetData(ViewState.Item("DepartID"))
    End Sub

End Class
