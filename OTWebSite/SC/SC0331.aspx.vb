'****************************************************
'功能說明：SC_BatchRule維護-新增
'建立人員：Chung
'建立日期：2013/05/09
'****************************************************
Imports System.Data

Partial Class SC_SC0331
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            subLoadDeptID(ddlDeptID, "1")
            subLoadWorkType()
            subLoadGroup()
        End If
    End Sub

    ''' <summary>
    ''' 撈取部門資料
    ''' </summary>
    ''' <param name="objDDL"></param>
    ''' <param name="OrganType">1:部門 2:科組課</param>
    ''' <remarks></remarks>
    Private Sub subLoadDeptID(ByVal objDDL As DropDownList, ByVal OrganType As String, Optional ByVal DeptID As String = "")
        Dim strSQL As New StringBuilder()

        Dim objSC As New SC()

        strSQL.AppendLine("Select OrganID, OrganID + '-' + OrganName + Case When InValidFlag = '1' Then '(無效)' else '' End as OrganName")
        strSQL.AppendLine("From SC_Organization")
        strSQL.AppendLine("Where 1 = 1")
        strSQL.AppendLine("And OrganType = " & Bsp.Utility.Quote(OrganType))
        If DeptID <> "" Then
            strSQL.AppendLine("And DeptID = " & Bsp.Utility.Quote(DeptID))
        End If
        strSQL.AppendLine("Order by OrganID ")

        Using dt As DataTable = Bsp.DB.ExecuteDataSet(CommandType.Text, strSQL.ToString()).Tables(0)
            With objDDL
                .Items.Clear()
                .DataTextField = "OrganName"
                .DataValueField = "OrganID"
                .DataSource = dt
                .DataBind()

                .Items.Insert(0, New ListItem("---不指定---", ""))
            End With
        End Using
    End Sub

    Private Sub subLoadWorkType(Optional ByVal DeptID As String = "", Optional ByVal OrganID As String = "")
        Dim strSQL As New StringBuilder()

        strSQL.AppendLine("Select distinct WorkTypeID, WorkTypeID + '-' + WorkType as WorkType")
        strSQL.AppendLine("From SC_User")
        strSQL.AppendLine("Where WorkType <> ''")
        strSQL.AppendLine("And WorkStatus = '1'")

        If DeptID <> "" Then strSQL.AppendLine("And DeptID = " & Bsp.Utility.Quote(DeptID))
        If OrganID <> "" Then strSQL.AppendLine("And OrganID = " & Bsp.Utility.Quote(OrganID))

        Using dt As DataTable = Bsp.DB.ExecuteDataSet(CommandType.Text, strSQL.ToString()).Tables(0)
            With ddlWorkTypeID
                .Items.Clear()
                .DataTextField = "WorkType"
                .DataValueField = "WorkTypeID"
                .DataSource = dt
                .DataBind()
                .Items.Insert(0, New ListItem("---不指定---", ""))
            End With
        End Using
    End Sub

    Private Sub subLoadGroup()
        Dim strSQL As New StringBuilder()

        strSQL.AppendLine("Select GroupID, GroupID + '-' + GroupName as GroupName")
        strSQL.AppendLine("From SC_Group")
        strSQL.AppendLine("Order by GroupID")

        Using dt As DataTable = Bsp.DB.ExecuteDataSet(CommandType.Text, strSQL.ToString()).Tables(0)
            With ddlGroupID
                .Items.Clear()
                .DataTextField = "GroupName"
                .DataValueField = "GroupID"
                .DataSource = dt
                .DataBind()
                .Items.Insert(0, New ListItem("---不指定---", ""))
            End With
        End Using
    End Sub

    Public Overrides Sub DoAction(ByVal Param As String)
        Select Case Param
            Case "btnAdd"
                If funCheckData() Then
                    If SaveData() Then
                        GoBack()
                    End If
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
        Dim objSC As New SC
        Dim beBatchRule As New beSC_BatchRule.Row()


        beBatchRule.DeptID.Value = ddlDeptID.SelectedValue
        beBatchRule.OrganID.Value = ddlOrganID.SelectedValue
        beBatchRule.WorkTypeID.Value = ddlWorkTypeID.SelectedValue
        beBatchRule.UpdateSeq.Value = CInt(txtUpdateSeq.Text)
        beBatchRule.BanMark.Value = rblBanMark.SelectedValue
        beBatchRule.BusinessFlag.Value = rblBusinessFlag.SelectedValue
        beBatchRule.GroupID.Value = ddlGroupID.SelectedValue
        beBatchRule.Description.Value = txtDescription.Text


        Try
            '判斷要Update的資料是否存在, 存在Update, 不存在則Insert
            If objSC.IsBatchRuleExist(beBatchRule) Then
                objSC.UpdateBatchRule(beBatchRule)
            Else
                objSC.AddBatchRule(beBatchRule)
            End If
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & ".SaveData", ex)
            Return False
        End Try

        Return True
    End Function

    Private Function funCheckData() As Boolean
        Dim strValue As String

        'If txtDeptID.Text.Trim = "" AndAlso txtOrganID.Text.Trim = "" AndAlso txtWorkTypeID.Text.Trim = "" Then
        '    Bsp.Utility.ShowFormatMessage(Me, "W_04200")
        '    txtDeptID.Focus()
        '    Return False
        'End If

        'strValue = txtDeptID.Text.ToString()
        'If strValue <> "" Then
        '    If Bsp.Utility.getStringLength(strValue) > txtDeptID.MaxLength Then
        '        Bsp.Utility.ShowFormatMessage(Me, "W_00040", "部门", txtDeptID.MaxLength.ToString())
        '        txtDeptID.Focus()
        '        Return False
        '    End If
        '    txtDeptID.Text = strValue
        'End If

        'strValue = txtOrganID.Text.ToString()
        'If strValue <> "" Then
        '    If Bsp.Utility.getStringLength(strValue) > txtOrganID.MaxLength Then
        '        Bsp.Utility.ShowFormatMessage(Me, "W_00040", "科组课", txtOrganID.MaxLength.ToString())
        '        txtOrganID.Focus()
        '        Return False
        '    End If
        '    txtOrganID.Text = strValue
        'End If

        'strValue = txtWorkTypeID.Text.ToString()
        'If strValue <> "" Then
        '    If Bsp.Utility.getStringLength(strValue) > txtWorkTypeID.MaxLength Then
        '        Bsp.Utility.ShowFormatMessage(Me, "W_00040", "工作性质", txtWorkTypeID.MaxLength.ToString())
        '        txtWorkTypeID.Focus()
        '        Return False
        '    End If
        '    txtWorkTypeID.Text = strValue
        'End If

        strValue = txtUpdateSeq.Text.ToString.Trim()
        If strValue = "" Then
            strValue = "0"
        Else
            If Not IsNumeric(strValue) Then
                strValue = "0"
            End If
        End If
        txtUpdateSeq.Text = strValue

        'strValue = txtBanMark.Text.ToString()
        'If strValue <> "" Then
        '    If Bsp.Utility.getStringLength(strValue) > txtBanMark.MaxLength Then
        '        Bsp.Utility.ShowFormatMessage(Me, "W_00040", "可使用注记", txtBanMark.MaxLength.ToString())
        '        txtBanMark.Focus()
        '        Return False
        '    End If
        '    txtBanMark.Text = strValue
        'End If

        'strValue = txtGroupID.Text.ToString()
        'If strValue <> "" Then
        '    If Bsp.Utility.getStringLength(strValue) > txtGroupID.MaxLength Then
        '        Bsp.Utility.ShowFormatMessage(Me, "W_00040", "群组", txtGroupID.MaxLength.ToString())
        '        txtGroupID.Focus()
        '        Return False
        '    End If
        '    txtGroupID.Text = strValue
        'End If

        'strValue = txtBusinessFlag.Text.ToString()
        'If strValue <> "" Then
        '    If Bsp.Utility.getStringLength(strValue) > txtBusinessFlag.MaxLength Then
        '        Bsp.Utility.ShowFormatMessage(Me, "W_00040", "业务注记", txtBusinessFlag.MaxLength.ToString())
        '        txtBusinessFlag.Focus()
        '        Return False
        '    End If
        '    txtBusinessFlag.Text = strValue
        'End If

        strValue = txtDescription.Text.ToString()
        If strValue <> "" Then
            If Bsp.Utility.getStringLength(strValue) > txtDescription.MaxLength Then
                Bsp.Utility.ShowFormatMessage(Me, "W_00040", "說明", txtDescription.MaxLength.ToString())
                txtDescription.Focus()
                Return False
            End If
            txtDescription.Text = strValue
        End If

        Return True
    End Function

    Protected Sub ddlDeptID_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlDeptID.SelectedIndexChanged
        subLoadDeptID(ddlOrganID, "2", ddlDeptID.SelectedValue)
        subLoadWorkType(ddlDeptID.SelectedValue)
    End Sub

    Protected Sub ddlOrganID_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlOrganID.SelectedIndexChanged
        subLoadWorkType(ddlDeptID.SelectedValue, ddlOrganID.SelectedValue)
    End Sub
End Class
