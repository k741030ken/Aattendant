'****************************************************
'功能說明：SC_Organization維護-修改
'建立人員：Chung
'建立日期：2011/05/17
'****************************************************
Imports System.Data
Imports Newtonsoft.Json

Partial Class SC_SC0212
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
                    New FieldState("UserID", "員工編號", True, True), _
                    New FieldState("UserName", "員工姓名", True, True)}
        End If
    End Sub

    Protected Overrides Sub BaseOnPageTransfer(ByVal ti As TransferInfo)
        If Not IsPostBack() Then
            Dim ht As Hashtable = Bsp.Utility.getHashTableFromParam(ti.Args)

            If ht.ContainsKey("SelectedOrganID") Then
                GetData(ht("SelectedOrganID").ToString())
            Else
                Return
            End If
        End If
    End Sub

    Public Overrides Sub DoAction(ByVal Param As String)
        Select Case Param
            Case "btnUpdate"
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

    Private Sub GetData(ByVal strOrganID As String)
        Dim objSC As New SC
        Dim bsOrgan As New beSC_Organization.Service()
        Dim beOrgan As New beSC_Organization.Row()

        beOrgan.OrganID.Value = strOrganID
        Try
            Using dt As DataTable = bsOrgan.QueryByKey(beOrgan).Tables(0)
                If dt.Rows.Count <= 0 Then Exit Sub
                beOrgan = New beSC_Organization.Row(dt.Rows(0))

                lblOrganID.Text = beOrgan.OrganID.Value
                txtOrganName.Text = beOrgan.OrganName.Value

                If beOrgan.OrganID.Value = beOrgan.DeptID.Value Then
                    ddlDeptID.SelectedIndex = 0
                Else
                    Bsp.Utility.SetSelectedIndex(ddlDeptID, beOrgan.DeptID.Value)
                End If
                Bsp.Utility.SetSelectedIndex(ddlUpOrganID, beOrgan.UpOrganID.Value)
                txtBoss.Text = beOrgan.Boss.Value
                Using dtBoss As DataTable = objSC.GetUserInfo(dt.Rows(0).Item("Boss").ToString(), "UserName")
                    If dtBoss.Rows.Count > 0 Then txtBossName.Text = dtBoss.Rows(0).Item("UserName").ToString
                End Using


               

                'txtBusinessBossName.Text = dt.Rows(0).Item("BusinessBossName").ToString()
                
                
 
                chkBranchFlag.Checked = IIf(beOrgan.BranchFlag.Value = "1", True, False)
                chkInValidFlag.Checked = IIf(beOrgan.InValidFlag.Value = "1", False, True)


                lblLastChgDate.Text = beOrgan.LastChgDate.Value.ToString("yyyy/MM/dd HH:mm:ss")
                lblLastChgID.Text = beOrgan.LastChgID.Value

            End Using
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & ".GetData", ex)
        End Try

    End Sub

    Private Function SaveData() As Boolean
        Dim beOrgan As New beSC_Organization.Row()
        Dim bsOrgan As New beSC_Organization.Service()
        Dim objSC As New SC

        beOrgan.OrganID.Value = lblOrganID.Text
        beOrgan.OrganName.Value = txtOrganName.Text

        beOrgan.DeptID.Value = IIf(ddlDeptID.SelectedValue = "", beOrgan.OrganID.Value, ddlDeptID.SelectedValue)
        beOrgan.UpOrganID.Value = ddlUpOrganID.SelectedValue

        beOrgan.Boss.Value = txtBoss.Text


        beOrgan.BranchFlag.Value = IIf(chkBranchFlag.Checked, "1", "0")


        beOrgan.InValidFlag.Value = IIf(chkInValidFlag.Checked, "0", "1")

        beOrgan.LastChgDate.Value = Now
        beOrgan.LastChgID.Value = UserProfile.ActUserID

        If Not bsOrgan.IsDataExists(beOrgan) Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00020", "")
            Return False
        End If

        Try
            Return objSC.UpdateOrganization(beOrgan)
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & ".SaveData", ex)
            Return False
        End Try

    End Function

    Private Function funCheckData() As Boolean
        Dim strValue As String

        strValue = txtOrganName.Text.ToString()
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
