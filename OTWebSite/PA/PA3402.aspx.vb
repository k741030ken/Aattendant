'****************************************************
'功能說明：學校設定-修改
'建立人員：BeatriceCheng
'建立日期：2015.05.06
'****************************************************
Imports System.Data

Partial Class PA_PA3402
    Inherits PageBase


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then

        End If
    End Sub

    Protected Overrides Sub BaseOnPageTransfer(ByVal ti As TransferInfo)
        If Not IsPostBack() Then
            Dim ht As Hashtable = Bsp.Utility.getHashTableFromParam(ti.Args)

            If ht.ContainsKey("SelectedSchoolID") Then
                ViewState.Item("SchoolID") = ht("SelectedSchoolID").ToString()
                subGetData(ht("SelectedSchoolID").ToString())
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
        Dim beSchool As New beSchool.Row()
        Dim bsSchool As New beSchool.Service()
        Dim objPA3 As New PA3()

        '取得輸入資料
        beSchool.SchoolID.Value = txtSchoolID.Text
        beSchool.Remark.Value = txtRemark.Text
        beSchool.PrimaryFlag.Value = IIf(cbPrimaryFlag.Checked, "1", "0")

        beSchool.LastChgComp.Value = UserProfile.ActCompID
        beSchool.LastChgID.Value = UserProfile.ActUserID
        beSchool.LastChgDate.Value = Now

        '儲存資料
        Try
            Return objPA3.SchoolUpdate(beSchool)
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, "[CC0201_99]" & Bsp.Utility.getMessage("E_00000") & Bsp.Utility.getInnerException(Me.FunID, ex))
            Return False
        End Try
    End Function

    Private Sub subGetData(ByVal SchoolID As String)
        Dim objSC As New SC
        Dim beSchool As New beSchool.Row()
        Dim bsSchool As New beSchool.Service()
        Dim objPA3 As New PA3()

        beSchool.SchoolID.Value = SchoolID
        Try
            Using dt As DataTable = bsSchool.QueryByKey(beSchool).Tables(0)
                If dt.Rows.Count <= 0 Then Exit Sub
                beSchool = New beSchool.Row(dt.Rows(0))

                txtSchoolID.Text = beSchool.SchoolID.Value
                txtRemark.Text = beSchool.Remark.Value
                cbPrimaryFlag.Checked = IIf(beSchool.PrimaryFlag.Value.Equals("1"), True, False)
                '最後異動公司
                Dim CompName As String = objSC.GetSC_CompName(beSchool.LastChgComp.Value)
                txtLastChgComp.Text = beSchool.LastChgComp.Value + IIf(CompName <> "", "-" + CompName, "")
                '最後異動人員
                Dim UserName As String = objSC.GetSC_UserName(beSchool.LastChgComp.Value, beSchool.LastChgID.Value)
                txtLastChgID.Text = beSchool.LastChgID.Value + IIf(UserName <> "", "-" + UserName, "")
                '最後異動日期
                Dim boolDate As Boolean = Format(beSchool.LastChgDate.Value, "yyyy/MM/dd") = "1900/01/01"
                txtLastChgDate.Text = IIf(boolDate, "", beSchool.LastChgDate.Value.ToString("yyyy/MM/dd HH:mm:ss"))
            End Using
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & "subGetData", ex)
        End Try

    End Sub

    Private Function funCheckData() As Boolean
        Dim strValue As String = ""

        '學校名稱
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
        subGetData(ViewState.Item("SchoolID"))
    End Sub

End Class
