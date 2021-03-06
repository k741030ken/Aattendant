'****************************************************
'功能說明：處別維護-修改
'建立人員：Chung
'建立日期：2011/05/17
'****************************************************
Imports System.Data

Partial Class SC_SC0222
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        txtBoss.Attributes.Add("onkeypress", "clearOnKeypress('txtBossName');")

        If Not IsPostBack() Then
            ucButtonQuerySelect.QuerySQL = "Select UserID, UserName From SC_User Where BanMark = '0' Order by UserID"
            ucButtonQuerySelect.Fields = New FieldState() { _
                    New FieldState("UserID", "員工編號", True, True), _
                    New FieldState("UserName", "員工姓名", True, True)}

            Bsp.Utility.FillRegion(ddlUpRegionID)
            ddlUpRegionID.Items.Insert(0, New ListItem("---不指定---", ""))
        End If
    End Sub

    Protected Overrides Sub BaseOnPageTransfer(ByVal ti As TransferInfo)
        If Not IsPostBack() Then
            Dim ht As Hashtable = Bsp.Utility.getHashTableFromParam(ti.Args)

            If ht.ContainsKey("SelectedRegionID") Then
                GetData(ht("SelectedRegionID").ToString())
                ddlUpRegionID.Items.RemoveAt(ddlUpRegionID.Items.IndexOf(ddlUpRegionID.Items.FindByValue(ht("SelectedRegionID").ToString())))
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

    Private Sub GetData(ByVal RegionID As String)
        Dim beRegion As New beSC_Region.Row()
        Dim objSC As New SC()

        Try
            Using dt As DataTable = objSC.GetRegionInfo(RegionID, "*, dbo.funGetAOrgDefine('3', Boss) BossName")
                If dt.Rows.Count <= 0 Then Exit Sub
                beRegion = New beSC_Region.Row(dt.Rows(0))

                lblRegionID.Text = beRegion.RegionID.Value
                txtRegionName.Text = beRegion.RegionName.Value
                txtBoss.Text = beRegion.Boss.Value
                txtBossName.Text = dt.Rows(0).Item("BossName").ToString()
                Bsp.Utility.SetSelectedIndex(ddlUpRegionID, beRegion.UpRegionID.Value)
                lblCreateDate.Text = beRegion.CreateDate.Value.ToString("yyyy/MM/dd HH:mm:ss")
                lblLastChgID.Text = beRegion.LastChgID.Value
                lblLastChgDate.Text = beRegion.LastChgDate.Value.ToString("yyyy/MM/dd HH:mm:ss")
            End Using
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & ".GetData", ex)
        End Try
    End Sub

    Private Function SaveData() As Boolean
        Dim beRegion As New beSC_Region.Row()
        Dim bsRegion As New beSC_Region.Service()
        Dim beUser As New beSC_User.Row()
        Dim bsUser As New beSC_User.Service()
        Dim objSC As New SC()

        beRegion.RegionID.Value = lblRegionID.Text
        beRegion.RegionName.Value = txtRegionName.Text.Trim()
        beRegion.Boss.Value = txtBoss.Text.Trim()
        beRegion.UpRegionID.Value = ddlUpRegionID.SelectedValue
        beRegion.LastChgDate.Value = Now
        beRegion.LastChgID.Value = UserProfile.ActUserID

        '判斷Boss是否存在於SC_User內
        beUser.UserID.Value = txtBoss.Text
        If Not bsUser.IsDataExists(beUser) Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00020", "處别主管")
            Return False
        End If

        '判斷資料是否存在
        If Not bsRegion.IsDataExists(beRegion) Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00020", "處别")
            Return False
        End If

        Try
            objSC.UpdateRegion(beRegion)
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & ".SaveData", ex)
            Return False
        End Try
        Return True
    End Function

    Private Function funCheckData() As Boolean
        Dim strValue As String

        strValue = txtRegionName.Text.ToString()
        If strValue = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", "處别名稱")
            txtRegionName.Focus()
            Return False
        Else
            If Bsp.Utility.getStringLength(strValue) > txtRegionName.MaxLength Then
                Bsp.Utility.ShowFormatMessage(Me, "W_00040", "處别名稱", txtRegionName.MaxLength.ToString())
                txtRegionName.Focus()
                Return False
            End If
            txtRegionName.Text = strValue
        End If

        strValue = txtBoss.Text.ToString()
        If strValue = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", "處别主管")
            txtBoss.Focus()
            Return False
        Else
            If Bsp.Utility.getStringLength(strValue) > txtBoss.MaxLength Then
                Bsp.Utility.ShowFormatMessage(Me, "W_00040", "處别主管", txtBoss.MaxLength.ToString())
                txtBoss.Focus()
                Return False
            End If
            txtBoss.Text = strValue
        End If

        Return True
    End Function

    Public Overrides Sub DoModalReturn(ByVal returnValue As String)
        If returnValue <> "" Then
            Dim aryData() As String = returnValue.Split(":")

            Select Case aryData(0)
                Case "ucButtonQuerySelect"
                    Dim aryValue() As String = Split(aryData(1), "|$|")

                    txtBoss.Text = aryValue(0)
                    txtBossName.Text = aryValue(1)
            End Select
        End If
    End Sub
End Class
