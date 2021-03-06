'****************************************************
'功能說明：年曆檔維護-新增
'建立人員：MickySung
'建立日期：2015.05.18
'****************************************************
Imports System.Data

Partial Class PA_PA1A01
    Inherits PageBase


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then
            Dim objSC As New SC()
            '2015/05/28 公司代碼-名稱改寫法
            lbltxtCompID.Text = UserProfile.SelectCompRoleName
            'lbltxtCompID.Text = UserProfile.SelectCompRoleID + "-" + objSC.GetCompName(UserProfile.SelectCompRoleID).Rows(0).Item("CompName").ToString
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
                    ViewState.Item("Action") = "btnAdd"
                    Release("btnAdd")
                    'If SaveData() Then
                    '    GoBack()
                    'End If
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
        Dim beCalendar As New beCalendar.Row()
        Dim bsCalendar As New beCalendar.Service()
        Dim objPA As New PA1()

        beCalendar.CompID.Value = UserProfile.SelectCompRoleID
        beCalendar.SysDate.Value = txtSysDate.DateText
        beCalendar.Week.Value = ddlWeek.SelectedValue
        If rbnHolidayOrNot1Show.Checked Then
            '假日
            beCalendar.HolidayOrNot.Value = "1"
        ElseIf rbnHolidayOrNot2Show.Checked Then
            '營業日
            beCalendar.HolidayOrNot.Value = "0"
        Else
            beCalendar.HolidayOrNot.Value = ""
        End If
        beCalendar.LastBusDate.Value = txtLastBusDate.DateText
        beCalendar.NextBusDate.Value = txtNextBusDate.DateText
        beCalendar.NeNeBusDate.Value = txtNeNeBusDate.DateText
        '上營業日差
        If txtLastDateDiff.Text <> "" Then
            beCalendar.LastDateDiff.Value = txtLastDateDiff.Text
        Else
            beCalendar.LastDateDiff.Value = "0"
        End If
        '下營業日差
        If txtNextDateDiff.Text <> "" Then
            beCalendar.NextDateDiff.Value = txtNextDateDiff.Text
        Else
            beCalendar.NextDateDiff.Value = "0"
        End If
        '下下營業日差
        If txtNeNeDateDiff.Text <> "" Then
            beCalendar.NeNeDateDiff.Value = txtNeNeDateDiff.Text
        Else
            beCalendar.NeNeDateDiff.Value = "0"
        End If
        beCalendar.LastEndDate.Value = txtLastEndDate.DateText
        beCalendar.ThisEndDate.Value = txtThisEndDate.DateText
        beCalendar.MonEndDate.Value = txtMonEndDate.DateText
        '太陽日
        If txtJulianDate.Text <> "" Then
            beCalendar.JulianDate.Value = txtJulianDate.Text
        Else
            beCalendar.JulianDate.Value = "1"
        End If
        beCalendar.LastChgComp.Value = UserProfile.ActCompID
        beCalendar.LastChgID.Value = UserProfile.ActUserID
        beCalendar.LastChgDate.Value = Now

        '檢查資料是否存在
        If bsCalendar.IsDataExists(beCalendar) Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00010", "")
            Return False
        End If

        '儲存資料
        Try
            Return objPA.AddCalendarSetting(beCalendar)
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, "[CC0201_99]" & Bsp.Utility.getMessage("E_00000") & Bsp.Utility.getInnerException(Me.FunID, ex))
            Return False
        End Try
    End Function

    Private Function funCheckData() As Boolean
        Dim objPA As New PA1()
        Dim beCalendar As New beCalendar.Row()
        Dim bsCalendar As New beCalendar.Service()

        '系統日期        
        If txtSysDate.DateText = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblSysDate.Text)
            txtSysDate.Focus()
            Return False
        Else
            If Not IsDate(txtSysDate.DateText) Then
                Bsp.Utility.ShowFormatMessage(Me, "W_00070", lblSysDate.Text)
                txtSysDate.Focus()
                Return False
            End If
        End If

        '上一營業日
        If txtLastBusDate.DateText = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblLastBusDate.Text)
            txtLastBusDate.Focus()
            Return False
        Else
            If Not IsDate(txtLastBusDate.DateText) Then
                Bsp.Utility.ShowFormatMessage(Me, "W_00070", lblLastBusDate.Text)
                txtLastBusDate.Focus()
                Return False
            End If
        End If

        '下一營業日
        If txtNextBusDate.DateText = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblNextBusDate.Text)
            txtNextBusDate.Focus()
            Return False
        Else
            If Not IsDate(txtNextBusDate.DateText) Then
                Bsp.Utility.ShowFormatMessage(Me, "W_00070", lblNextBusDate.Text)
                txtNextBusDate.Focus()
                Return False
            End If
        End If

        '下下營業日
        If txtNeNeBusDate.DateText = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblNeNeBusDate.Text)
            txtNeNeBusDate.Focus()
            Return False
        Else
            If Not IsDate(txtNeNeBusDate.DateText) Then
                Bsp.Utility.ShowFormatMessage(Me, "W_00070", lblNeNeBusDate.Text)
                txtNeNeBusDate.Focus()
                Return False
            End If
        End If

        '上營業日差
        If txtLastDateDiff.Text.Trim <> "" Then
            If IsNumeric(txtLastDateDiff.Text.Trim) = False Then
                Bsp.Utility.ShowMessage(Me, "｢上營業日差｣請輸入0~255之數字")
                txtLastDateDiff.Focus()
                Return False
            Else
                If CInt(txtLastDateDiff.Text.Trim) < 0 Or CInt(txtLastDateDiff.Text.Trim) > 255 Then
                    Bsp.Utility.ShowMessage(Me, "｢上營業日差｣請輸入0~255之數字")
                    txtLastDateDiff.Focus()
                    Return False
                End If
            End If
        End If

        '下營業日差
        If txtNextDateDiff.Text.Trim <> "" Then
            If IsNumeric(txtNextDateDiff.Text.Trim) = False Then
                Bsp.Utility.ShowMessage(Me, "｢下營業日差｣請輸入0~255之數字")
                txtNextDateDiff.Focus()
                Return False
            Else
                If CInt(txtNextDateDiff.Text.Trim) < 0 Or CInt(txtNextDateDiff.Text.Trim) > 255 Then
                    Bsp.Utility.ShowMessage(Me, "｢下營業日差｣請輸入0~255之數字")
                    txtNextDateDiff.Focus()
                    Return False
                End If
            End If
        End If

        '下下營業日差
        If txtNeNeDateDiff.Text.Trim <> "" Then
            If IsNumeric(txtNeNeDateDiff.Text.Trim) = False Then
                Bsp.Utility.ShowMessage(Me, "｢下下營業日差｣請輸入0~255之數字")
                txtNeNeDateDiff.Focus()
                Return False
            Else
                If CInt(txtNeNeDateDiff.Text.Trim) < 0 Or CInt(txtNeNeDateDiff.Text.Trim) > 255 Then
                    Bsp.Utility.ShowMessage(Me, "｢下下營業日差｣請輸入0~255之數字")
                    txtNeNeDateDiff.Focus()
                    Return False
                End If
            End If
        End If

        '上月月底日
        If txtLastEndDate.DateText = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblLastEndDate.Text)
            txtLastEndDate.Focus()
            Return False
        Else
            If Not IsDate(txtLastEndDate.DateText) Then
                Bsp.Utility.ShowFormatMessage(Me, "W_00070", lblLastEndDate.Text)
                txtLastEndDate.Focus()
                Return False
            End If
        End If

        '本月月底日
        If txtThisEndDate.DateText = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblThisEndDate.Text)
            txtThisEndDate.Focus()
            Return False
        Else
            If Not IsDate(txtThisEndDate.DateText) Then
                Bsp.Utility.ShowFormatMessage(Me, "W_00070", lblThisEndDate.Text)
                txtThisEndDate.Focus()
                Return False
            End If
        End If

        '本月最終營業日
        If txtMonEndDate.DateText = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", lblMonEndDate.Text)
            txtMonEndDate.Focus()
            Return False
        Else
            If Not IsDate(txtMonEndDate.DateText) Then
                Bsp.Utility.ShowFormatMessage(Me, "W_00070", lblMonEndDate.Text)
                txtMonEndDate.Focus()
                Return False
            End If
        End If

        '太陽日
        If txtJulianDate.Text.Trim <> "" Then
            If IsNumeric(txtJulianDate.Text.Trim) = False Then
                Bsp.Utility.ShowMessage(Me, "｢太陽日｣請輸入1~366之數字")
                txtJulianDate.Focus()
                Return False
            Else
                If CInt(txtJulianDate.Text.Trim) < 1 Or CInt(txtJulianDate.Text.Trim) > 366 Then
                    Bsp.Utility.ShowMessage(Me, "｢太陽日｣請輸入1~366之數字")
                    txtJulianDate.Focus()
                    Return False
                End If
            End If
        End If

        Return True
    End Function

    Private Sub ClearData()
        'txtSysDate.Text = ""
        ddlWeek.SelectedValue = ""
        rbnHolidayOrNot1Show.Checked = False
        rbnHolidayOrNot2Show.Checked = False
        txtLastBusDate.DateText = ""
        txtNextBusDate.DateText = ""
        txtNeNeBusDate.DateText = ""
        txtLastDateDiff.Text = ""
        txtNextDateDiff.Text = ""
        txtNeNeDateDiff.Text = ""
        txtLastEndDate.DateText = ""
        txtThisEndDate.DateText = ""
        txtMonEndDate.DateText = ""
        txtJulianDate.Text = ""
    End Sub

    Private Sub Release(ByVal LogFunction As String)
        ucRelease.ShowCompRole = "True"
        ucRelease.FunID = "PA1A00"
        ucRelease.LogFunction = LogFunction
        ucRelease.OpenSelect()
    End Sub

    Public Overrides Sub DoModalReturn(ByVal returnValue As String)
        Dim strSql As String = ""

        If returnValue <> "" Then
            Dim aryData() As String = returnValue.Split(":")

            Select Case aryData(0)
                Case "ucRelease"
                    lblReleaseResult.Text = ""
                    Dim aryValue() As String = Split(aryData(1), "|$|")
                    lblReleaseResult.Text = aryValue(0)
                    If lblReleaseResult.Text = "Y" Then
                        Select Case ViewState.Item("Action")
                            Case "btnAdd"
                                If SaveData() Then
                                    GoBack()
                                End If
                        End Select
                    End If
            End Select

        End If
    End Sub


End Class
