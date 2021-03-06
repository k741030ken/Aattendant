'****************************************************
'功能說明：年曆檔維護-修改
'建立人員：MickySung
'建立日期：2015.05.18
'****************************************************
Imports System.Data

Partial Class PA_PA1A02
    Inherits PageBase


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then
            
        End If
    End Sub
    Protected Overrides Sub BaseOnPageTransfer(ByVal ti As TransferInfo)
        If Not IsPostBack() Then
            Dim ht As Hashtable = Bsp.Utility.getHashTableFromParam(ti.Args)

            If ht.ContainsKey("SelectedSysDate") Then
                ViewState.Item("SysDate") = ht("SelectedSysDate").ToString()
                subGetData(ht("SelectedSysDate").ToString())
            Else
                Return
            End If
        End If
    End Sub
    Public Overrides Sub DoAction(ByVal Param As String)
        Select Case Param
            Case "btnUpdate"   '存檔返回
                If funCheckData() Then
                    ViewState.Item("Action") = "btnUpdate"
                    Release("btnUpdate")
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
        beCalendar.SysDate.Value = lbltxtSysDate.Text
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

        '儲存資料
        Try
            Return objPA.UpdateCalendarSetting(beCalendar)
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, "[CC0201_99]" & Bsp.Utility.getMessage("E_00000") & Bsp.Utility.getInnerException(Me.FunID, ex))
            Return False
        End Try
    End Function

    Private Sub subGetData(ByVal SysDate As String)
        Dim objPA As New PA1()
        Dim objSC As New SC()
        Dim beCalendar As New beCalendar.Row()
        Dim bsCalendar As New beCalendar.Service()

        beCalendar.CompID.Value = UserProfile.SelectCompRoleID
        beCalendar.SysDate.Value = SysDate
        Try
            Using dt As DataTable = bsCalendar.QueryByKey(beCalendar).Tables(0)
                If dt.Rows.Count <= 0 Then Exit Sub
                beCalendar = New beCalendar.Row(dt.Rows(0))

                '2015/05/28 公司代碼-名稱改寫法
                lbltxtCompID.Text = UserProfile.SelectCompRoleName
                'lbltxtCompID.Text = beCalendar.CompID.Value + "-" + objSC.GetCompName(beCalendar.CompID.Value).Rows(0).Item("CompName").ToString
                '系統日期
                lbltxtSysDate.Text = beCalendar.SysDate.Value
                '星期
                ddlWeek.SelectedValue = beCalendar.Week.Value
                '假日註記
                rbnHolidayOrNot1Show.Checked = IIf(beCalendar.HolidayOrNot.Value = "1", True, False)
                rbnHolidayOrNot2Show.Checked = IIf(beCalendar.HolidayOrNot.Value = "0", True, False)
                '上一營業日
                txtLastBusDate.DateText = Format(beCalendar.LastBusDate.Value, "yyyy/MM/dd")
                '下一營業日
                txtNextBusDate.DateText = Format(beCalendar.NextBusDate.Value, "yyyy/MM/dd")
                '下下營業日
                txtNeNeBusDate.DateText = Format(beCalendar.NeNeBusDate.Value, "yyyy/MM/dd")
                '上營業日差
                txtLastDateDiff.Text = beCalendar.LastDateDiff.Value
                '下營業日差
                txtNextDateDiff.Text = beCalendar.NextDateDiff.Value
                '下下營業日差
                txtNeNeDateDiff.Text = beCalendar.NeNeDateDiff.Value
                '上月月底日
                txtLastEndDate.DateText = Format(beCalendar.LastEndDate.Value, "yyyy/MM/dd")
                '本月月底日
                txtThisEndDate.DateText = Format(beCalendar.ThisEndDate.Value, "yyyy/MM/dd")
                '本月最終營業日
                txtMonEndDate.DateText = Format(beCalendar.MonEndDate.Value, "yyyy/MM/dd")
                '太陽日
                txtJulianDate.Text = beCalendar.JulianDate.Value
                '最後異動公司
                If beCalendar.LastChgComp.Value.Trim <> "" Then
                    lblLastChgComp.Text = beCalendar.LastChgComp.Value + "-" + objSC.GetCompName(beCalendar.LastChgComp.Value).Rows(0).Item("CompName").ToString
                Else
                    lblLastChgComp.Text = ""
                End If
                '最後異動人員
                If beCalendar.LastChgID.Value.Trim <> "" Then
                    Dim UserName As String = objSC.GetSC_UserName(beCalendar.LastChgComp.Value, beCalendar.LastChgID.Value)
                    lblLastChgID.Text = beCalendar.LastChgID.Value + IIf(UserName <> "", "-" + UserName, "")
                Else
                    lblLastChgID.Text = ""
                End If
                '最後異動日期
                lblLastChgDate.Text = IIf(Format(beCalendar.LastChgDate.Value, "yyyy/MM/dd") = "1900/01/01", "", beCalendar.LastChgDate.Value.ToString("yyyy/MM/dd HH:mm:ss"))

            End Using
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & "subGetData", ex)
        End Try
    End Sub

    Private Function funCheckData() As Boolean
        Dim objPA As New PA1()
        Dim beCalendar As New beCalendar.Row()
        Dim bsCalendar As New beCalendar.Service()

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
        subGetData(lbltxtSysDate.Text)
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
                            Case "btnUpdate"
                                If SaveData() Then
                                    GoBack()
                                End If
                        End Select
                    End If
            End Select

        End If
    End Sub


End Class
