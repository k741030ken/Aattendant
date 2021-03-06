'****************************************************
'功能說明：家屬稱謂狀況設定-修改
'建立人員：BeatriceCheng
'建立日期：2015.05.05
'****************************************************
Imports System.Data

Partial Class PA_PA3202
    Inherits PageBase


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then

        End If
    End Sub
    Protected Overrides Sub BaseOnPageTransfer(ByVal ti As TransferInfo)
        If Not IsPostBack() Then
            Dim ht As Hashtable = Bsp.Utility.getHashTableFromParam(ti.Args)

            If ht.ContainsKey("SelectedRelativeID") Then
                ViewState.Item("RelativeID") = ht("SelectedRelativeID").ToString()
                subGetData(ht("SelectedRelativeID").ToString())
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
        Dim beRelationship As New beRelationship.Row()
        Dim bsRelationship As New beRelationship.Service()
        Dim objPA3 As New PA3()

        '取得輸入資料
        beRelationship.RelativeID.Value = txtRelativeID.Text
        beRelationship.Remark.Value = txtRemark.Text
        beRelationship.RemarkCN.Value = txtRemarkCN.Text
        beRelationship.HeaRelID.Value = txtHeaRelID.Text
        beRelationship.CompRelID.Value = txtCompRelID.Text
        beRelationship.DeathPayID.Value = txtDeathPayID.Text
        beRelationship.TaxFamilyID.Value = ddlTaxFamilyID.SelectedValue
        beRelationship.RelTypeID.Value = ddlRelTypeID.SelectedValue
        beRelationship.RelClassID.Value = txtRelClassID.Text

        beRelationship.LastChgComp.Value = UserProfile.ActCompID
        beRelationship.LastChgID.Value = UserProfile.ActUserID
        beRelationship.LastChgDate.Value = Now

        '儲存資料
        Try
            Return objPA3.RelationshipUpdate(beRelationship)
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, "[CC0201_99]" & Bsp.Utility.getMessage("E_00000") & Bsp.Utility.getInnerException(Me.FunID, ex))
            Return False
        End Try
    End Function


    Private Sub subGetData(ByVal RelativeID As String)
        Dim objSC As New SC
        Dim beRelationship As New beRelationship.Row()
        Dim bsRelationship As New beRelationship.Service()
        Dim objPA3 As New PA3()

        beRelationship.RelativeID.Value = RelativeID
        Try
            Using dt As DataTable = bsRelationship.QueryByKey(beRelationship).Tables(0)
                If dt.Rows.Count <= 0 Then Exit Sub
                beRelationship = New beRelationship.Row(dt.Rows(0))

                txtRelativeID.Text = beRelationship.RelativeID.Value
                txtRemark.Text = beRelationship.Remark.Value
                txtRemarkCN.Text = beRelationship.RemarkCN.Value
                txtHeaRelID.Text = beRelationship.HeaRelID.Value
                txtCompRelID.Text = beRelationship.CompRelID.Value
                txtDeathPayID.Text = beRelationship.DeathPayID.Value
                ddlTaxFamilyID.SelectedValue = beRelationship.TaxFamilyID.Value
                ddlRelTypeID.SelectedValue = beRelationship.RelTypeID.Value
                txtRelClassID.Text = beRelationship.RelClassID.Value

                '最後異動公司
                Dim CompName As String = objSC.GetSC_CompName(beRelationship.LastChgComp.Value)
                txtLastChgComp.Text = beRelationship.LastChgComp.Value + IIf(CompName <> "", "-" + CompName, "")
                '最後異動人員
                Dim UserName As String = objSC.GetSC_UserName(beRelationship.LastChgComp.Value, beRelationship.LastChgID.Value)
                txtLastChgID.Text = beRelationship.LastChgID.Value + IIf(UserName <> "", "-" + UserName, "")
                '最後異動日期
                Dim boolDate As Boolean = Format(beRelationship.LastChgDate.Value, "yyyy/MM/dd") = "1900/01/01"
                txtLastChgDate.Text = IIf(boolDate, "", beRelationship.LastChgDate.Value.ToString("yyyy/MM/dd HH:mm:ss"))
            End Using
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & "subGetData", ex)
        End Try

    End Sub

    Private Function funCheckData() As Boolean

        Dim strValue As String = ""

        '家屬稱謂名稱(繁)
        strValue = txtRemark.Text
        If strValue.Trim = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblRemark.Text)
            txtRemark.Focus()
            Return False
        End If
        'If Bsp.Utility.getStringLength(strValue) > txtRemark.MaxLength Then
        '    Bsp.Utility.ShowFormatMessage(Me, "W_00040", lblRemark.Text, txtRemark.MaxLength.ToString())
        '    txtRemark.Focus()
        '    Return False
        'End If

        '家屬稱謂名稱(簡)
        strValue = txtRemarkCN.Text
        If strValue.Trim = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblRemarkCN.Text)
            txtRemarkCN.Focus()
            Return False
        End If
        'If Bsp.Utility.getStringLength(strValue) > txtRemarkCN.MaxLength Then
        '    Bsp.Utility.ShowFormatMessage(Me, "W_00040", lblRemarkCN.Text, txtRemarkCN.MaxLength.ToString())
        '    txtRemarkCN.Focus()
        '    Return False
        'End If

        '健保家屬稱謂代碼
        strValue = txtHeaRelID.Text
        If Bsp.Utility.getStringLength(strValue) > txtHeaRelID.MaxLength Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00040", lblHeaRelID.Text, txtHeaRelID.MaxLength.ToString())
            txtHeaRelID.Focus()
            Return False
        End If

        '團保家屬稱謂代碼
        strValue = txtCompRelID.Text
        If Bsp.Utility.getStringLength(strValue) > txtCompRelID.MaxLength Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00040", lblCompRelID.Text, txtCompRelID.MaxLength.ToString())
            txtCompRelID.Focus()
            Return False
        End If

        '喪葬補助稱謂代碼
        strValue = txtDeathPayID.Text
        If Bsp.Utility.getStringLength(strValue) > txtDeathPayID.MaxLength Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00040", lblDeathPayID.Text, txtDeathPayID.MaxLength.ToString())
            txtDeathPayID.Focus()
            Return False
        End If

        '幾等親代碼
        strValue = txtRelClassID.Text
        If Bsp.Utility.getStringLength(strValue) > txtRelClassID.MaxLength Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00040", lblRelClassID.Text, txtRelClassID.MaxLength.ToString())
            txtRelClassID.Focus()
            Return False
        End If

        Return True
    End Function

    Private Sub ClearData()
        subGetData(ViewState.Item("RelativeID"))
    End Sub

End Class
