'****************************************************
'功能說明：使用者(SC_User)維護-新增
'建立人員：Ann
'建立日期：2014/10/30
'****************************************************
Imports System.Data
Imports System.Data.Common

Partial Class SC_SC0101
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then
            ucSelectBusiness.LoadData()
            ucSelectOrgan.LoadData()
        End If
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
        Dim beUser As New beSC_User.Row()
        Dim bsUser As New beSC_User.Service()
        Dim objSC As New SC

        If funCheckData() Then
            Using cn As DbConnection = Bsp.DB.getConnection()
                cn.Open()

                Dim tran As DbTransaction = cn.BeginTransaction
                Dim inTrans As Boolean = True

                Try
                    '取得輸入資料
                    beUser.UserID.Value = txtUserID.Text.ToUpper()
                    beUser.UserName.Value = txtUserName.Text.Trim()
                    beUser.EngName.Value = txtEngName.Text
                    beUser.BanMark.Value = IIf(chkBanMark.Checked, "1", "0")

                    beUser.CompID.Value = txtCompID.Text

                    beUser.DeptID.Value = ucSelectOrgan.SelectedDeptID
                    beUser.OrganID.Value = ucSelectOrgan.SelectedOrganID
                    If beUser.OrganID.Value = "" Then beUser.OrganID.Value = beUser.DeptID.Value
                    beUser.EMail.Value = txtEMail.Text
                    beUser.ExpireDate.Value = Convert.ToDateTime(txtExpireDate.Text)
                    If txtPasswordErrorCount.Text.Trim() = "" OrElse Not IsNumeric(txtPasswordErrorCount.Text) Then
                        beUser.PasswordErrorCount.Value = 0
                    Else
                        beUser.PasswordErrorCount.Value = Convert.ToInt32(txtPasswordErrorCount.Text)
                    End If
                    beUser.RankID.Value = txtRankID.Text

                    beUser.WorkStatus.Value = "1"
                    beUser.WorkTypeID.Value = txtWorkTypeID.Text
                    beUser.CreateDate.Value = Now
                    beUser.LastChgDate.Value = Now
                    beUser.LastChgID.Value = UserProfile.ActUserID

                    '檢查資料是否存在
                    If bsUser.IsDataExists(beUser) Then
                        Bsp.Utility.ShowFormatMessage(Me, "W_00010", "")
                        Return False
                    End If

                    '儲存資料
                    Try
                        Return objSC.AddUser(beUser)
                    Catch ex As Exception
                        Bsp.Utility.ShowMessage(Me, "[CC0201_99]" & Bsp.Utility.getMessage("E_00000") & Bsp.Utility.getInnerException(Me.FunID, ex))
                        Return False
                    End Try
                    tran.Commit()
                Catch ex As Exception
                    If inTrans Then tran.Rollback()
                    Return False
                End Try
            End Using
        End If
        Return True
    End Function

    Private Function funCheckData() As Boolean
        Dim strValue As String

        strValue = txtUserID.Text.ToString().Trim().ToUpper()
        If strValue = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", "員工編號")
            txtUserID.Focus()
            Return False
        Else
            If Bsp.Utility.getStringLength(strValue) > txtUserID.MaxLength Then
                Bsp.Utility.ShowFormatMessage(Me, "W_00040", "員工編號", txtUserID.MaxLength.ToString())
                txtUserID.Focus()
                Return False
            End If
            txtUserID.Text = strValue
        End If

        strValue = txtUserName.Text.ToString().Trim()
        If strValue = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", "員工姓名")
            txtUserName.Focus()
            Return False
        Else
            If Bsp.Utility.getStringLength(strValue) > txtUserName.MaxLength Then
                Bsp.Utility.ShowFormatMessage(Me, "W_00040", "員工姓名", txtUserName.MaxLength.ToString())
                txtUserName.Focus()
                Return False
            End If
            txtUserName.Text = strValue
        End If

        strValue = txtEngName.Text.ToString().Trim()
        If Bsp.Utility.getStringLength(strValue) > txtEngName.MaxLength Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00040", "員工英文姓名", txtUserName.MaxLength.ToString())
            txtEngName.Focus()
            Return False
        End If
        txtEngName.Text = strValue

        strValue = txtCompID.Text.ToString()
        If Bsp.Utility.getStringLength(strValue) > txtCompID.MaxLength Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00040", "公司代碼", txtCompID.MaxLength.ToString())
            txtCompID.Focus()
            Return False
        End If
        txtCompID.Text = strValue

        strValue = txtWorkTypeID.Text.ToString()
        If Bsp.Utility.getStringLength(strValue) > txtWorkTypeID.MaxLength Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00040", "工作性質", txtUserName.MaxLength.ToString())
            txtWorkTypeID.Focus()
            Return False
        End If
        txtWorkTypeID.Text = strValue

        strValue = txtRankID.Text.ToString()
        If Bsp.Utility.getStringLength(strValue) > txtRankID.MaxLength Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00040", "職等", txtRankID.MaxLength.ToString())
            txtRankID.Focus()
            Return False
        End If
        txtRankID.Text = strValue

        strValue = txtPasswordErrorCount.Text.ToString()
        If strValue <> "" AndAlso Not IsNumeric(strValue) Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00050", "密碼錯誤次數")
            txtPasswordErrorCount.Focus()
            Return False
        End If
        txtPasswordErrorCount.Text = strValue

        strValue = txtExpireDate.Text.ToString()
        If strValue = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", "使用期限")
            txtExpireDate.Focus()
            Return False
        Else
            If Bsp.Utility.getStringLength(strValue) > txtExpireDate.MaxLength Then
                Bsp.Utility.ShowFormatMessage(Me, "W_00040", "使用期限", txtExpireDate.MaxLength.ToString())
                txtExpireDate.Focus()
                Return False
            End If
            If Not IsDate(strValue) Then
                Bsp.Utility.ShowFormatMessage(Me, "W_00070", "使用期限")
                txtExpireDate.Focus()
                Return False
            End If
            txtExpireDate.Text = strValue
        End If

        strValue = txtEMail.Text.ToString()
        If Bsp.Utility.getStringLength(strValue) > txtEMail.MaxLength Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00040", "E-Mail", txtEMail.MaxLength.ToString())
            txtEMail.Focus()
            Return False
        End If
        txtEMail.Text = strValue

        '檢核若勾選業務註記時，需同步選取業務單位
        If chkBusinessFlag.Checked AndAlso ucSelectBusiness.SelectedDeptID = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_02000")
            ucSelectBusiness.Focus()
            Return False
        End If

        Return True
    End Function
End Class
