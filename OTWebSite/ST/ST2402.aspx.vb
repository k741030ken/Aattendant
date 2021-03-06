'****************************************************
'功能說明：員工證照資料查詢-明細
'建立人員：BeatriceCheng
'建立日期：2015.06.11
'****************************************************
Imports System.Data

Partial Class ST_ST2402
    Inherits PageBase


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then

        End If
    End Sub

    Protected Overrides Sub BaseOnPageTransfer(ByVal ti As TransferInfo)
        If Not IsPostBack() Then
            Dim ht As Hashtable = Bsp.Utility.getHashTableFromParam(ti.Args)

            If ht.ContainsKey("SelectedIDNo") Then
                ViewState.Item("CompID") = ht("SelectedCompID").ToString()
                ViewState.Item("IDNo") = ht("SelectedIDNo").ToString()
                ViewState.Item("CertiDate") = ht("SelectedCertiDate").ToString()
                ViewState.Item("LicenseName") = ht("SelectedLicenseName").ToString()
                ViewState.Item("CategoryID") = ht("SelectedCategoryID").ToString()
                ViewState.Item("Institution") = ht("SelectedInstitution").ToString()
                subGetData(
                    ht("SelectedIDNo").ToString(), _
                    ht("SelectedCertiDate").ToString(), _
                    ht("SelectedLicenseName").ToString(), _
                    ht("SelectedCategoryID").ToString(), _
                    ht("SelectedInstitution").ToString())
            Else
                Return
            End If
        End If
    End Sub

    Public Overrides Sub DoAction(ByVal Param As String)
        Select Case Param
            Case "btnActionX"   '返回
                GoBack()
        End Select
    End Sub

    Private Sub GoBack()
        Dim ti As TransferInfo = Me.StateTransfer
        Me.TransferFramePage(ti.CallerUrl, Nothing, ti.Args)
    End Sub

    Private Sub subGetData(ByVal IDNo As String, ByVal CertiDate As String, ByVal LicenseName As String, ByVal CategoryID As String, ByVal Institution As String)
        Dim beCertification As New beCertification.Row()
        Dim bsCertification As New beCertification.Service()
        Dim objST2 As New ST2

        beCertification.IDNo.Value = IDNo
        beCertification.CertiDate.Value = CertiDate
        beCertification.LicenseName.Value = LicenseName
        beCertification.CategoryID.Value = CategoryID
        beCertification.Institution.Value = Institution
        Try
            Using dt As DataTable = objST2.GetEmpData(IDNo, ViewState.Item("CompID"))
                If dt.Rows.Count <= 0 Then Exit Sub

                txtCompID.Text = dt.Rows(0).Item("CompID")
                txtEmpID.Text = dt.Rows(0).Item("EmpID")
                txtName.Text = dt.Rows(0).Item("NameN")
            End Using

            Using dt As DataTable = bsCertification.QueryByKey(beCertification).Tables(0)
                If dt.Rows.Count <= 0 Then Exit Sub
                beCertification = New beCertification.Row(dt.Rows(0))

                txtCategoryID.Text = beCertification.CategoryID.Value
                txtCertiDate.Text = Format(beCertification.CertiDate.Value, "yyyy/MM/dd")

                Dim strCertiTo As String = Format(beCertification.CertiTo.Value, "yyyy/MM/dd")
                txtCertiTo.Text = IIf(strCertiTo = "1900/01/01", "永久", strCertiTo)

                txtLicenseName.Text = beCertification.LicenseName.Value
                txtInstitution.Text = beCertification.Institution.Value
                txtSerialNum.Text = beCertification.SerialNum.Value
                txtRemark.Text = beCertification.Remark.Value
            End Using
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & "subGetData", ex)
        End Try

    End Sub
End Class
