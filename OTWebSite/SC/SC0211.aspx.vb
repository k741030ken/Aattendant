'****************************************************
'功能說明：部門維護-新增
'建立人員：Chung
'建立日期：2011/05/17
'****************************************************
Imports System.Data
Imports Newtonsoft.Json

Partial Class SC_SC0211
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        txtBoss.Attributes.Add("onkeypress", "clearOnKeypress('txtBossName');")

        If Not IsPostBack() Then
            Bsp.Utility.FillOrganization(ddlDeptID, Bsp.Enums.OrganType.Dept)
            Bsp.Utility.FillOrganization(ddlUpOrganID, Bsp.Enums.OrganType.Dept)
            Bsp.Utility.FillRegion(ddlRegionID)
            ddlDeptID.Items.Insert(0, New ListItem("(此單位為一級部門)", ""))
            ddlUpOrganID.Items.Insert(0, New ListItem("(無上階部門)", ""))
            ddlRegionID.Items.Insert(0, New ListItem("--請選擇--", ""))
            ucSelectBoss.QuerySQL = "Select UserID, UserName From SC_User Where BanMark = '0' Order by UserID"
            ucSelectBoss.Fields = New FieldState() { _
                    New FieldState("UserID", "員工編號", True, True), _
                    New FieldState("UserName", "員工姓名", True, True)}
            ucSelectBusinessBoss.QuerySQL = "Select UserID, UserName From SC_User Where BanMark = '0' Order by UserID"
            ucSelectBusinessBoss.Fields = New FieldState() { _
                    New FieldState("UserID", "編號", True, True), _
                    New FieldState("UserName", "員工姓名", True, True)}
        End If
    End Sub

    Public Overrides Sub DoAction(ByVal Param As String)
        Select Case Param
            Case "btnAdd"
                If funCheckData() Then
                    If SaveData() Then GoBack()
                End If
            Case "btnActionX"
                GoBack()
        End Select
    End Sub

    Private Sub GoBack()
        Dim ti As TransferInfo = Me.StateTransfer
        Me.TransferFramePage(ti.CallerUrl, Nothing, ti.Args)
    End Sub

    Private Function SaveData() As Boolean
        Dim beOrgan As New beSC_Organization.Row()
        Dim bsOrgan As New beSC_Organization.Service()
        Dim objSC As New SC()

        beOrgan.OrganID.Value = txtOrganID.Text
        beOrgan.OrganName.Value = txtOrganName.Text
        beOrgan.DeptID.Value = IIf(ddlDeptID.SelectedValue = "", beOrgan.OrganID.Value, ddlDeptID.SelectedValue)
        beOrgan.UpOrganID.Value = ddlUpOrganID.SelectedValue

        beOrgan.Boss.Value = txtBoss.Text
        beOrgan.BranchFlag.Value = IIf(chkBranchFlag.Checked, "1", "0")

        beOrgan.InValidFlag.Value = IIf(chkInValidFlag.Checked, "0", "1")


        beOrgan.LastChgDate.Value = Now
        beOrgan.LastChgID.Value = UserProfile.ActUserID

        '檢查資料是否存在
        If bsOrgan.IsDataExists(beOrgan) Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00010", "")
            Return False
        End If
        Try
            Return objSC.AddOrganization(beOrgan)
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & ".SaveData", ex)
            Return False
        End Try

    End Function

    Private Function funCheckData() As Boolean
        Dim strValue As String

        strValue = txtOrganID.Text.ToString().Trim().ToUpper()
        If strValue = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", "部門代碼")
            txtOrganID.Focus()
            Return False
        Else
            If Bsp.Utility.getStringLength(strValue) > txtOrganID.MaxLength Then
                Bsp.Utility.ShowFormatMessage(Me, "W_00040", "部門代碼", txtOrganID.MaxLength.ToString())
                txtOrganID.Focus()
                Return False
            End If
            txtOrganID.Text = strValue
        End If

        strValue = txtOrganName.Text.ToString().Trim()
        If strValue = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", "部門名稱")
            txtOrganName.Focus()
            Return False
        Else
            If Bsp.Utility.getStringLength(strValue) > txtOrganName.MaxLength Then
                Bsp.Utility.ShowFormatMessage(Me, "W_00040", "部門名稱", txtOrganName.MaxLength.ToString())
                txtOrganName.Focus()
                Return False
            End If
            txtOrganName.Text = strValue
        End If

        strValue = txtBranchNo.Text.ToString()
        If Bsp.Utility.getStringLength(strValue) > txtBranchNo.MaxLength Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00040", "分行代碼", txtBranchNo.MaxLength.ToString())
            txtBranchNo.Focus()
            Return False
        End If
        txtBranchNo.Text = strValue

        strValue = txtBoss.Text.ToString()
        If Bsp.Utility.getStringLength(strValue) > txtBoss.MaxLength Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00040", "單位主管", txtBoss.MaxLength.ToString())
            txtBoss.Focus()
            Return False
        End If
        txtBoss.Text = strValue

        strValue = txtBusinessBoss.Text.ToString()
        If Bsp.Utility.getStringLength(strValue) > txtBusinessBoss.MaxLength Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00040", "業務主管", txtBusinessBoss.MaxLength.ToString())
            txtBusinessBoss.Focus()
            Return False
        End If
        txtBusinessBoss.Text = strValue

        Return True
    End Function

    Public Overrides Sub DoModalReturn(ByVal returnValue As String)
        If returnValue <> "" Then
            'Dim aryData() As String = returnValue.Split(":")
            Dim intPos As Integer = returnValue.IndexOf(":")
            Dim strWho As String = returnValue.Substring(0, intPos)
            Dim dicData As Dictionary(Of String, String) = JsonConvert.DeserializeObject(Of Dictionary(Of String, String))(returnValue.Substring(intPos + 1))

            Select Case strWho
                Case "ucSelectBoss"

                    For Each s As String In dicData.Keys
                        Select Case s
                            Case "UserID"
                                txtBoss.Text = dicData(s)
                            Case "UserName"
                                txtBossName.Text = dicData(s)
                        End Select
                    Next

                Case "ucSelectBusinessBoss"

                    For Each s As String In dicData.Keys
                        Select Case s
                            Case "UserID"
                                txtBusinessBoss.Text = dicData(s)
                            Case "UserName"
                                txtBusinessBossName.Text = dicData(s)
                        End Select
                    Next

            End Select
        End If
    End Sub
End Class
