'****************************************************
'功能說明：學歷代碼設定-修改
'建立人員：BeatriceCheng
'建立日期：2015.05.04
'****************************************************
Imports System.Data

Partial Class PA_PA3102
    Inherits PageBase


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then

        End If
    End Sub

    Protected Overrides Sub BaseOnPageTransfer(ByVal ti As TransferInfo)
        If Not IsPostBack() Then
            Dim ht As Hashtable = Bsp.Utility.getHashTableFromParam(ti.Args)

            If ht.ContainsKey("SelectedEduID") Then
                ViewState.Item("EduID") = ht("SelectedEduID").ToString()
                subGetData(ht("SelectedEduID").ToString())
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
        Dim beEduDegree As New beEduDegree.Row()
        Dim bsEduDegree As New beEduDegree.Service()
        Dim objPA3 As New PA3()

        '取得輸入資料
        beEduDegree.EduID.Value = txtEduID.Text
        beEduDegree.EduName.Value = txtEduName.Text
        beEduDegree.EduNameCN.Value = txtEduNameCN.Text

        beEduDegree.LastChgComp.Value = UserProfile.ActCompID
        beEduDegree.LastChgID.Value = UserProfile.ActUserID
        beEduDegree.LastChgDate.Value = Now

        '儲存資料
        Try
            Return objPA3.EduDegreeUpdate(beEduDegree)
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, "[CC0201_99]" & Bsp.Utility.getMessage("E_00000") & Bsp.Utility.getInnerException(Me.FunID, ex))
            Return False
        End Try
    End Function

    Private Sub subGetData(ByVal EduID As String)
        Dim objSC As New SC
        Dim beEduDegree As New beEduDegree.Row()
        Dim bsEduDegree As New beEduDegree.Service()
        Dim objPA3 As New PA3()

        beEduDegree.EduID.Value = EduID
        Try
            Using dt As DataTable = bsEduDegree.QueryByKey(beEduDegree).Tables(0)
                If dt.Rows.Count <= 0 Then Exit Sub
                beEduDegree = New beEduDegree.Row(dt.Rows(0))

                txtEduID.Text = beEduDegree.EduID.Value
                txtEduName.Text = beEduDegree.EduName.Value
                txtEduNameCN.Text = beEduDegree.EduNameCN.Value
                '最後異動公司
                Dim CompName As String = objSC.GetSC_CompName(beEduDegree.LastChgComp.Value)
                txtLastChgComp.Text = beEduDegree.LastChgComp.Value + IIf(CompName <> "", "-" + CompName, "")
                '最後異動人員
                Dim UserName As String = objSC.GetSC_UserName(beEduDegree.LastChgComp.Value, beEduDegree.LastChgID.Value)
                txtLastChgID.Text = beEduDegree.LastChgID.Value + IIf(UserName <> "", "-" + UserName, "")
                '最後異動日期
                Dim boolDate As Boolean = Format(beEduDegree.LastChgDate.Value, "yyyy/MM/dd") = "1900/01/01"
                txtLastChgDate.Text = IIf(boolDate, "", beEduDegree.LastChgDate.Value.ToString("yyyy/MM/dd HH:mm:ss"))
            End Using
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & "subGetData", ex)
        End Try

    End Sub

    Private Function funCheckData() As Boolean
        Dim strValue As String = ""

        '學歷名稱(繁)
        strValue = txtEduName.Text
        If strValue.Trim = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblEduName.Text)
            txtEduName.Focus()
            Return False
        End If
        'If Bsp.Utility.getStringLength(strValue) > txtEduName.MaxLength Then
        '    Bsp.Utility.ShowFormatMessage(Me, "W_00040", lblEduName.Text, txtEduName.MaxLength.ToString())
        '    txtEduName.Focus()
        '    Return False
        'End If

        '學歷名稱(簡)
        strValue = txtEduNameCN.Text
        If strValue.Trim = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblEduNameCN.Text)
            txtEduNameCN.Focus()
            Return False
        End If
        'If Bsp.Utility.getStringLength(strValue) > txtEduNameCN.MaxLength Then
        '    Bsp.Utility.ShowFormatMessage(Me, "W_00040", lblEduNameCN.Text, txtEduNameCN.MaxLength.ToString())
        '    txtEduNameCN.Focus()
        '    Return False
        'End If

        Return True
    End Function

    Private Sub ClearData()
        subGetData(ViewState.Item("EduID"))
    End Sub

End Class
