'****************************************************
'功能說明：年曆檔上傳
'建立人員：Chung
'建立日期：2013/04/10
'****************************************************
Imports System.Data
Imports System.Data.Common
Imports Microsoft.Practices.EnterpriseLibrary.Common
Imports Microsoft.Practices.EnterpriseLibrary.Data

Partial Class SC_SC0311
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            getYear()
            ddlYear.SelectedIndex = 1
            Page.SetFocus(ddlAreaID)
        End If
    End Sub

    Public Overrides Sub DoAction(ByVal Param As String)
        Select Case Param
            Case "btnAdd"       '新增
                DoAdd()
            Case "btnUpdate"    '修改
                DoUpdate()
            Case "btnQuery"     '查詢
                DoQuery()
            Case "btnDelete"    '刪除
                DoDelete()
            Case "btnActionC"   '上傳
                DoUpload()
            Case Else
                DoOtherAction(Param)   '其他功能動作
        End Select
    End Sub

    Private Sub getYear()
        Dim intThisYear As Integer = Today.Year

        ddlYear.Items.Clear()
        For intLoop As Integer = -1 To 2
            ddlYear.Items.Add(New ListItem((intThisYear + intLoop).ToString()))
        Next
    End Sub

    Private Sub DoAdd()
        Dim objSC As New SC()

        Try
            objSC.ImportCalendar(ddlAreaID.SelectedValue, ddlYear.SelectedValue)

            Bsp.Utility.RunClientScript(Me, "alert('" & String.Format(Bsp.Utility.getMessage("I_00020"), "年曆檔") & "');" & vbCrLf & _
                            "window.top.returnValue = 'OK';" & vbCrLf & "window.top.close();")
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & ".DoAdd", ex)
        End Try
    End Sub

    Private Sub DoUpdate()

    End Sub

    Private Sub DoQuery()

    End Sub

    Private Sub DoDelete()

    End Sub

    Private Sub DoUpload()
        If calendarUpload.PostedFile Is Nothing Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00120")
            Return
        End If
        Dim strFileName As String = Server.MapPath(Bsp.Utility.getAppSetting("TempPath")) & "\" & _
                Bsp.Utility.GetNewFileName("CALENDAR") & ".txt"

        calendarUpload.PostedFile.SaveAs(strFileName)

        ReadForAfter100(strFileName)
        'Select Case ddlAreaID.SelectedValue
        '    Case "TW"
        '        ReadForAfter100(strFileName)
        '    Case Else
        '        ReadForOverseas(strFileName)
        'End Select
    End Sub

    ''' <summary>
    ''' 台灣年曆使用
    ''' </summary>
    ''' <param name="FileName"></param>
    ''' <remarks></remarks>
    Private Sub ReadForAfter100(ByVal FileName As String)
        Dim strDateString As String
        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim objSC As New SC

        Dim intHolidayOrNot As Integer = 1
        Dim intWeek As Integer = 1
        Dim intSysDate As Integer = 8
        Dim intNextBusDate As Integer = 8
        Dim intLastBusDate As Integer = 8
        Dim intNeNeBusDate As Integer = 8
        Dim intLastEndDate As Integer = 8
        Dim intThisEndDate As Integer = 8
        Dim intMonEndDate As Integer = 8
        Dim intNextDateDiff As Integer = 2
        Dim intNeNeDateDiff As Integer = 2
        Dim intLastDateDiff As Integer = 2
        Dim intJulianDate As Integer = 3

        Using cn As DbConnection = db.CreateConnection
            cn.Open()
            Dim tran As DbTransaction = cn.BeginTransaction
            Dim inTrans As Boolean = True
            Dim bolFirstRow As Boolean = True
            Dim intCount As Integer = 0

            Try
                Using sr As System.IO.StreamReader = New System.IO.StreamReader(FileName, System.Text.Encoding.ASCII)
                    While sr.Peek >= -1
                        strDateString = sr.ReadLine
                        If strDateString Is Nothing OrElse strDateString = "" Then Exit While

                        Dim beCalendar As New beSC_Calendar.Row()
                        Dim intPos As Integer = 0
                        'Mark by Chung 2009.07.20  配合百年專案
                        'Dim intPos As Integer = 11

                        With beCalendar
                            '假日
                            'Update by Chung 2012.07.30 配合NCB修改年曆檔
                            'Mark by Chung 2009.07.20 配合百年專案
                            '.HolidayOrNot = strDateString.Substring(5, .LEN_HolidayOrNot)
                            .HolidayOrNot.Value = strDateString.Substring(intPos, intHolidayOrNot)
                            intPos += intHolidayOrNot
                            .Week.Value = strDateString.Substring(intPos, intWeek)
                            intPos += intWeek
                            .SysDate.Value = Bsp.Utility.CheckDate(strDateString.Substring(intPos, intSysDate))
                            intPos += intSysDate
                            .NextBusDate.Value = Bsp.Utility.CheckDate(strDateString.Substring(intPos, intNextBusDate))
                            intPos += intNextBusDate
                            .NeNeBusDate.Value = Bsp.Utility.CheckDate(strDateString.Substring(intPos, intNeNeBusDate))
                            intPos += intNeNeBusDate
                            .LastBusDate.Value = Bsp.Utility.CheckDate(strDateString.Substring(intPos, intLastBusDate))
                            intPos += intLastBusDate
                            .LastEndDate.Value = Bsp.Utility.CheckDate(strDateString.Substring(intPos, intLastEndDate))
                            intPos += intLastEndDate
                            .ThisEndDate.Value = Bsp.Utility.CheckDate(strDateString.Substring(intPos, intThisEndDate))
                            intPos += intThisEndDate
                            .MonEndDate.Value = Bsp.Utility.CheckDate(strDateString.Substring(intPos, intThisEndDate))
                            intPos += intThisEndDate
                            .NextDateDiff.Value = CInt(strDateString.Substring(intPos, intNextDateDiff))
                            intPos += intNextDateDiff
                            .NeNeDateDiff.Value = CInt(strDateString.Substring(intPos, intNeNeDateDiff))
                            intPos += intNeNeDateDiff
                            .LastDateDiff.Value = CInt(strDateString.Substring(intPos, intLastDateDiff))
                            intPos += intLastDateDiff
                            .JulianDate.Value = CInt(strDateString.Substring(intPos, intJulianDate))
                            .AreaID.Value = ddlAreaID.SelectedValue
                            .CreateDate.Value = Now
                            .LastChgDate.Value = Now
                            .LastChgID.Value = UserProfile.ActUserID
                        End With

                        '第一列檢查年度是否正確併刪除資料庫內此年度資料
                        If bolFirstRow Then
                            If beCalendar.SysDate.Value.Year.ToString() <> ddlYear.SelectedValue Then
                                Bsp.Utility.ShowFormatMessage(Me, "W_03100")
                                Return
                            End If

                            objSC.DeleteCalendarbyYear(beCalendar.AreaID.Value, beCalendar.SysDate.Value.Year.ToString(), tran)
                            bolFirstRow = False
                        End If
                        objSC.InsertCalendar(beCalendar, tran)
                    End While
                End Using

                tran.Commit()
                inTrans = False
                Bsp.Utility.RunClientScript(Me, "alert('" & String.Format(Bsp.Utility.getMessage("I_00020"), "年曆檔") & "');" & vbCrLf & _
                            "window.top.returnValue = 'OK';" & vbCrLf & "window.top.close();")
            Catch ex As Exception
                If inTrans Then tran.Rollback()
                Bsp.Utility.ShowMessage(Me, Me.FunID & ".DoUpload", ex)
            End Try
        End Using
    End Sub

    '''' <summary>
    '''' 海外年曆使用
    '''' </summary>
    '''' <param name="FileName"></param>
    '''' <remarks></remarks>
    'Private Sub ReadForOverseas(ByVal FileName As String)
    '    Dim strDateString As String
    '    Dim db As Database = DatabaseFactory.CreateDatabase
    '    Dim objSC As New SC

    '    Using cn As DbConnection = db.CreateConnection
    '        cn.Open()
    '        Dim tran As DbTransaction = cn.BeginTransaction
    '        Dim inTrans As Boolean = True
    '        Dim bolFirstRow As Boolean = True
    '        Dim intCount As Integer = 0

    '        Try
    '            Using sr As System.IO.StreamReader = New System.IO.StreamReader(FileName, System.Text.Encoding.ASCII)
    '                While sr.Peek >= -1
    '                    strDateString = sr.ReadLine
    '                    If strDateString Is Nothing OrElse strDateString = "" Then Exit While

    '                    Dim beCalendar As New SC_Entities.beSC_Calendar
    '                    Dim intPos As Integer = 2

    '                    With beCalendar
    '                        '假日
    '                        .HolidayOrNot = CInt(strDateString.Substring(0, SC_Entities.beSC_Calendar.LEN_HolidayOrNot2)).ToString()
    '                        .Week = CInt(strDateString.Substring(intPos, SC_Entities.beSC_Calendar.LEN_Week2)).ToString()
    '                        intPos += SC_Entities.beSC_Calendar.LEN_Week2
    '                        .SysDate = PubFun.ChiDatetoSysDate(strDateString.Substring(intPos, SC_Entities.beSC_Calendar.LEN_SysDate2))
    '                        intPos += SC_Entities.beSC_Calendar.LEN_SysDate2
    '                        .NextBusDate = PubFun.ChiDatetoSysDate(strDateString.Substring(intPos, SC_Entities.beSC_Calendar.LEN_NextBusDate2))
    '                        intPos += SC_Entities.beSC_Calendar.LEN_NextBusDate2
    '                        .NeNeBusDate = PubFun.ChiDatetoSysDate(strDateString.Substring(intPos, SC_Entities.beSC_Calendar.LEN_NeNeBusDate2))
    '                        intPos += SC_Entities.beSC_Calendar.LEN_NeNeBusDate2
    '                        .LastBusDate = PubFun.ChiDatetoSysDate(strDateString.Substring(intPos, SC_Entities.beSC_Calendar.LEN_LastBusDate2))
    '                        intPos += SC_Entities.beSC_Calendar.LEN_LastBusDate2
    '                        .LastEndDate = PubFun.ChiDatetoSysDate(strDateString.Substring(intPos, SC_Entities.beSC_Calendar.LEN_LastEndDate2))
    '                        intPos += SC_Entities.beSC_Calendar.LEN_LastEndDate2
    '                        .ThisEndDate = PubFun.ChiDatetoSysDate(strDateString.Substring(intPos, SC_Entities.beSC_Calendar.LEN_ThisEndDate2))
    '                        intPos += SC_Entities.beSC_Calendar.LEN_ThisEndDate2
    '                        .MonEndDate = PubFun.ChiDatetoSysDate(strDateString.Substring(intPos, SC_Entities.beSC_Calendar.LEN_ThisEndDate2))
    '                        intPos += SC_Entities.beSC_Calendar.LEN_ThisEndDate2
    '                        .NextDateDiff = CInt(strDateString.Substring(intPos, SC_Entities.beSC_Calendar.LEN_NextDateDiff2))
    '                        intPos += SC_Entities.beSC_Calendar.LEN_NextDateDiff2
    '                        .NeNeDateDiff = CInt(strDateString.Substring(intPos, SC_Entities.beSC_Calendar.LEN_NeNeDateDiff2))
    '                        intPos += SC_Entities.beSC_Calendar.LEN_NeNeDateDiff2
    '                        .LastDateDiff = CInt(strDateString.Substring(intPos, SC_Entities.beSC_Calendar.LEN_LastDateDiff2))
    '                        intPos += SC_Entities.beSC_Calendar.LEN_LastDateDiff2
    '                        .JulianDate = CInt(strDateString.Substring(intPos, SC_Entities.beSC_Calendar.LEN_JulianDate2))


    '                        .AreaID = ddlAreaID.SelectedValue
    '                        .CreateDate = Now
    '                        .LastChgDate = Now
    '                        .LastChgID = UserProfile.ActUserID
    '                    End With

    '                    '第一列檢查年度是否正確併刪除資料庫內此年度資料
    '                    If bolFirstRow Then
    '                        If beCalendar.SysDate.Year.ToString() <> ddlYear.SelectedValue Then
    '                            WebFun.ShowFormatMessage(Me, "W_03100")
    '                            Return
    '                        End If

    '                        objSC.DeleteCalendarbyYear(beCalendar.AreaID, beCalendar.SysDate.Year.ToString(), tran)
    '                        bolFirstRow = False
    '                    End If
    '                    objSC.InsertCalendar(beCalendar, tran)
    '                End While
    '            End Using

    '            tran.Commit()
    '            inTrans = False
    '            WebFun.RunClientScript(Me, "alert('" & String.Format(WebFun.getMessage("I_00020"), "年曆檔") & "');" & vbCrLf & _
    '                        "window.top.returnValue = 'OK';" & vbCrLf & "window.top.close();")
    '        Catch ex As Exception
    '            If inTrans Then tran.Rollback()
    '            WebFun.ShowMessage(Me, Me.FunID & ".DoUpload", ex)
    '        End Try
    '    End Using
    'End Sub

    Private Sub DoOtherAction(ByVal Param As String)

    End Sub

End Class
