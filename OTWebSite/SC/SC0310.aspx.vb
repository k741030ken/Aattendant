'****************************************************
'功能說明：年曆檔維護
'建立人員：Chung
'建立日期：2011/05/24
'****************************************************
Imports System.Data

Partial Class SC_SC0310
    Inherits PageBase

    Private ChineseWeek() As String = {"一", "二", "三", "四", "五", "六", "日"}

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Calendar1.SelectedDate = Today
            LoadCalendarDate(Calendar1.SelectedDate)
            Page.SetFocus(ddlAreaID)
        End If
    End Sub

    Public Overrides Sub DoAction(ByVal Param As String)
        Select Case Param
            Case "btnAdd"       '新增
                DoAdd()
            Case "btnUpdate"    '修改
                DoUpdate()
            Case "btnQuery"     '下載
                'DoQuery()
            Case "btnDelete"    '刪除
                DoDelete()
            Case Else
                DoOtherAction()   '其他功能動作
        End Select
    End Sub

    Private Sub DoAdd()
        Dim btnC As New ButtonState(ButtonState.emButtonType.Confirm)
        Dim btnA As New ButtonState(ButtonState.emButtonType.Add)
        Dim btnX As New ButtonState(ButtonState.emButtonType.Exit)

        btnC.Caption = "確定上傳"
        btnX.Caption = "關閉離開"
        btnA.Caption = "匯入年曆檔"

        Me.CallSmallPage("~/SC/SC0311.aspx", New ButtonState() {btnC, btnA, btnX})
    End Sub

    Public Overrides Sub DoModalReturn(ByVal returnValue As String)
        LoadCalendarDate(Calendar1.SelectedDate)
    End Sub

    Private Sub DoUpdate()
        Dim objSC As New SC

        Try
            objSC.UpdateCalendar(Calendar1.SelectedDate, lblAreaID.Text.Split("-")(0), _
                IIf(txtHolidayOrNot.Text = "1", "0", "1"))
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & ".DoUpdate", ex)
        Finally
            LoadCalendarDate(Calendar1.SelectedDate)
        End Try
    End Sub

    Private Sub LoadCalendarDate(ByVal sDate As DateTime)
        Dim objSC As New SC
        Dim beCalendar As beSC_Calendar.Row = Nothing

        Try
            Using dt As DataTable = objSC.GetCalendar(sDate, ddlAreaID.SelectedValue)
                If dt.Rows.Count = 0 Then
                    Bsp.Utility.ShowFormatMessage(Me, "W_00020", sDate.ToString("yyyy/MM/dd") & " ")
                    Return
                End If
                beCalendar = New beSC_Calendar.Row(dt.Rows(0))
            End Using

            With beCalendar
                lblAreaID.Text = ddlAreaID.SelectedItem.Text
                lblSysDate.Text = .SysDate.Value.ToString("yyyy/MM/dd")
                txtSysDate.Text = .SysDate.Value.ToString("yyyy/MM/dd")
                imgHolidayOrNot.ImageUrl = IIf(.HolidayOrNot.Value = "1", "~/images/chkbox.gif", "~/images/chkboxE.gif")
                txtHolidayOrNot.Text = .HolidayOrNot.Value
                lblWeek.Text = ChineseWeek(CInt(.Week.Value) - 1)
                lblNextBusDate.Text = .NextBusDate.Value.ToString("yyyy/MM/dd")
                lblLastBusDate.Text = .LastBusDate.Value.ToString("yyyy/MM/dd")
                lblNeNeBusDate.Text = .NeNeBusDate.Value.ToString("yyyy/MM/dd")
                lblLastEndDate.Text = .LastEndDate.Value.ToString("yyyy/MM/dd")
                lblThisEndDate.Text = .ThisEndDate.Value.ToString("yyyy/MM/dd")
                lblMonEndDate.Text = .MonEndDate.Value.ToString("yyyy/MM/dd")
                lblNextDateDiff.Text = .NextDateDiff.Value.ToString()
                lblNeNeDateDiff.Text = .NeNeDateDiff.Value.ToString()
                lblLastDateDiff.Text = .LastDateDiff.Value.ToString()
                lblJulianDate.Text = .JulianDate.Value.ToString()
            End With
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & ".LoadCalendarDate", ex)
        End Try
    End Sub

    Private Sub DoQuery()
       
    End Sub

    Private Sub DoDelete()

    End Sub

    Private Sub DoOtherAction()

    End Sub

    Protected Sub Calendar1_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Calendar1.SelectionChanged
        LoadCalendarDate(Calendar1.SelectedDate)
    End Sub

    Protected Sub ddlAreaID_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlAreaID.SelectedIndexChanged
        LoadCalendarDate(Calendar1.SelectedDate)
    End Sub
End Class
