'****************************************************
'功能說明：員工持股信託特例人員處理-新增
'建立人員：Micky Sung
'建立日期：2015.08.13
'****************************************************
Imports System.Data

Partial Class OT_OT1141
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then
            '公司代碼
            txtCompID.Text = UserProfile.SelectCompRoleName
            hidCompID.Value = UserProfile.SelectCompRoleID

            '員工編號
            ucQueryEmp.ShowCompRole = "False"
            ucQueryEmp.InValidFlag = "N"
            ucQueryEmp.SelectCompID = UserProfile.SelectCompRoleID
        End If
    End Sub

    Protected Overrides Sub BaseOnPageTransfer(ByVal ti As TransferInfo)
        If Not IsPostBack() Then

        End If
    End Sub

    Public Overrides Sub DoAction(ByVal Param As String)
        Select Case Param
            Case "btnAdd"   '存檔返回
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
        Dim objOT As New OT1()
        Dim beEmpStockTrustExLevel As New beEmpStockTrustExLevel.Row()
        Dim bsEmpStockTrustExLevel As New beEmpStockTrustExLevel.Service()

        beEmpStockTrustExLevel.CompID.Value = hidCompID.Value
        beEmpStockTrustExLevel.EmpID.Value = txtEmpID.Text
        beEmpStockTrustExLevel.UAmount.Value = CInt(txtUAmount.Text)
        beEmpStockTrustExLevel.LastChgComp.Value = UserProfile.ActCompID
        beEmpStockTrustExLevel.LastChgID.Value = UserProfile.ActUserID
        beEmpStockTrustExLevel.LastChgDate.Value = Now

        '檢查資料是否存在
        If bsEmpStockTrustExLevel.IsDataExists(beEmpStockTrustExLevel) Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00010", "")
            Return False
        End If

        '儲存資料
        Try
            Return objOT.AddEmpStockTrustExLevelSetting(beEmpStockTrustExLevel)
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, "[CC0201_99]" & Bsp.Utility.getMessage("E_00000") & Bsp.Utility.getInnerException(Me.FunID, ex))
            Return False
        End Try
    End Function

    Private Function funCheckData() As Boolean
        Dim objOT As New OT1()
        Dim beEmpStockTrustExLevel As New beEmpStockTrustExLevel.Row()
        Dim bsEmpStockTrustExLevel As New beEmpStockTrustExLevel.Service()

        '員工編號
        If txtEmpID.Text.Trim = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblEmpID.Text)
            txtEmpID.Focus()
            Return False
        Else
            If Bsp.Utility.getStringLength(txtEmpID.Text.Trim) > txtEmpID.MaxLength Then
                Bsp.Utility.ShowFormatMessage(Me, "W_00040", lblEmpID.Text, txtEmpID.MaxLength.ToString)
                Return False
            End If

            If Not objOT.IsDataExists("Personal", "AND CompID = " & Bsp.Utility.Quote(hidCompID.Value) & " AND EmpID = " & Bsp.Utility.Quote(txtEmpID.Text) & " AND WorkStatus = '1' ") Then
                Bsp.Utility.ShowMessage(Me, "欄位[員工編號]不存在，或非在職狀態")
                Return False
            End If
        End If

        '上限金額
        If txtUAmount.Text.Trim = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblUAmount.Text)
            txtUAmount.Focus()
            Return False
        Else
            If IsNumeric(txtUAmount.Text.Trim) And InStr(1, txtUAmount.Text.Trim, ".") = 0 Then
                '2015/08/20 Modify 修改數字檢核範圍、錯誤訊息
                If txtUAmount.Text.Trim < 0 Or txtUAmount.Text.Trim > 24000 Then
                    Bsp.Utility.ShowMessage(Me, "欄位[上限金額]不可小於0或大於24000")
                    txtUAmount.Focus()
                    Return False
                End If
            Else
                Bsp.Utility.ShowMessage(Me, "欄位[上限金額]請輸入數字")
                txtUAmount.Focus()
                Return False
            End If
        End If

        Return True
    End Function

    Private Sub ClearData()
        txtEmpID.Text = ""
        lblName.Text = ""
        txtUAmount.Text = ""
    End Sub

    Public Overrides Sub DoModalReturn(ByVal returnValue As String)
        Dim strSql As String = ""

        If returnValue <> "" Then
            Dim aryData() As String = returnValue.Split(":")
            Select Case aryData(0)
                Case "ucQueryEmp"
                    Dim aryValue() As String = Split(aryData(1), "|$|")
                    '員工編號
                    txtEmpID.Text = aryValue(1)
                    '員工姓名
                    lblName.Text = aryValue(2)
            End Select
        End If
    End Sub

    '員工編號帶出員工名稱
    Protected Sub btnEmpID_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEmpID.Click
        Dim objHR As New HR()
        Dim EmpNameTable As DataTable

        EmpNameTable = objHR.GetHREmpName(hidCompID.Value, txtEmpID.Text)
        If EmpNameTable.Rows().Count <> 0 Then
            lblName.Text = EmpNameTable.Rows(0).Item(0)
        End If
    End Sub

End Class
