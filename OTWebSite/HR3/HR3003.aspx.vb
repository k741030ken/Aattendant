'****************************************************
'功能說明：待生效員工資料上傳-->檔案上傳
'建立人員：wei
'建立日期：2013/12/10
'****************************************************
Imports System.Data
Imports System.Data.Common
Imports Microsoft.Practices.EnterpriseLibrary.Common
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports System.IO
Imports System.IO.Compression
Imports System.Data.OleDb
Imports System.Data.SqlClient

Partial Class HR_HR3003
    Inherits PageBase

    Public Shared intSuccessCount As Integer = 0
    Public Shared intErrorCount As Integer = 0
    Public Shared intTotalCount As Integer = 0
    Public Shared intWarningCount As Integer = 0

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then

        End If
    End Sub

    Public Overrides Sub DoAction(ByVal Param As String)
        Select Case Param

            Case "btnActionC"   '上傳
                If funCheckData() Then

                    '畫面輸入的條件

                    'cs:btnActionC = > Server 判斷DB有資料(先儲存上傳檔案&路徑) => 再回cs:IsTOConfirm('') => cs:btnUpd.click() => 再回Server讀取檔案內容寫入DB
                    'cs:btnActionC = > Server 判斷DB無資料(先儲存上傳檔案&路徑) =>讀取檔案內容寫入DB
                    '要先儲存檔案~不然CS<-->Server來回後~FileUpload資料會遺失
                    Dim strFileName As String = Bsp.Utility.GetNewFileName("HR3000")

                    Dim strFilePath As String = Server.MapPath(Bsp.Utility.getAppSetting("TempPath")) & "\" & strFileName & ".txt"
                    '先儲存上傳檔案
                    FileUpload.PostedFile.SaveAs(strFilePath)
                    'Server上 上傳檔案SaveAs路徑
                    hidFileUploadFileName.Value = strFileName & ".txt"

                    '要從IIS Server上直接產生USER LOCAL電腦的檔案是不行的，有安全問題，沒有權限，在LOCAL開發OK，但上SERVER不行
                    '將在Server先產生ERR檔案，再傳過去USER LOCAL CS端

                    'hidLogFileName.Value = strFileName & ".err"
                    'ERR LOG檔案的檔名同上傳檔
                    Dim strLogFile As String = FileUpload.PostedFile.FileName
                    strLogFile = strLogFile.Substring(strLogFile.LastIndexOf("\") + 1)
                    strLogFile = strLogFile.Substring(0, strLogFile.LastIndexOf("."))
                    hidLogFileName.Value = strLogFile & strFileName & ".err"

                    If IsDataExist.Value = "Y" Then
                        '若DB有資料，再回到CS，請USER由畫面確認 "相同资料已经存在，确定全部删除，重新上传?!?"
                        Bsp.Utility.RunClientScript(Me.Page, "IsTOConfirm('');")
                    Else
                        '若DB無資料，直接上傳
                        DoUpload()
                    End If
                End If
            Case Else
                'DoOtherAction(Param)   '其他功能動作
        End Select
    End Sub

    '隱藏的按鈕--上傳檔案
    Protected Sub btnUpd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpd.Click
        DoUpload()
    End Sub

    '隱藏的按鈕--下傳檔案
    Protected Sub btnUpd1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpd1.Click
        subDownloadLogFile()
    End Sub

    Private Sub GoBack()
        Dim ti As TransferInfo = Me.StateTransfer
        Me.TransferFramePage(ti.CallerUrl, Nothing, ti.Args)
    End Sub

    Private Function funCheckData() As Boolean
        Dim strValue As String = ""

        '上傳檔案路徑
        If FileUpload.PostedFile Is Nothing Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00120", "上傳檔案路徑")
            FileUpload.Focus()
            Return False
        Else
            If FileUpload.PostedFile.FileName = "" Then
                Bsp.Utility.ShowFormatMessage(Me, "W_00030", "上傳檔案路徑")
                FileUpload.Focus()
                Return False
            End If
        End If

        Return True
    End Function
    Private Sub DoUpload()
        'Dim objSC As New SC
        Dim strMessage As String = ""
        Dim strUpload_TableName As String = ""
        Dim btnX As New ButtonState(ButtonState.emButtonType.Exit)
        btnX.Caption = "關閉離開"

        Try
            Dim strFileName As String = Server.MapPath(Bsp.Utility.getAppSetting("TempPath")) & "\" & hidFileUploadFileName.Value

            Dim objHR As New HR

            '20150609 wei add
            Dim strTabName As String = ""
            If rbEmployeeWait.Checked Then
                strTabName = "HR3000$"
            End If
            If rbEmpAdditionWait.Checked Then
                strTabName = "HR3000_EmpAddition$"
            End If

            strUpload_TableName = objHR.funCheck_UploadTableName(strFileName).Replace("'", "").ToString

            If strUpload_TableName <> strTabName Then   '201540609 wei modify "HR3000$"
                File.Delete(strFileName)
                strMessage = "上傳檔案的工作表(WorkSheet)-" & Mid(strUpload_TableName, 1, Len(strUpload_TableName) - 1) & " 與選擇上傳類型-" & strTabName.Substring(0, Len(strTabName) - 1) & "不一致!" '20150609 wei modify
                Bsp.Utility.RunClientScript(Me, "alert('" & String.Format(Bsp.Utility.getMessage("E_00050"), strMessage) & "');" & vbCrLf & _
                                "window.top.returnValue = 'OK';")
            End If


            If objHR.funCheck_UploadCount(strFileName, "[" & strTabName & "]") > CInt(ConfigurationManager.AppSettings("Upload_MaxCount").ToString) Then '20150609 wei modify
                File.Delete(strFileName)
                strMessage = "上傳資料筆數過大，系統允許最大上傳筆數：" & ConfigurationManager.AppSettings("Upload_MaxCount").ToString
                Bsp.Utility.RunClientScript(Me, "alert('" & String.Format(Bsp.Utility.getMessage("E_00050"), strMessage) & "');" & vbCrLf & _
                                "window.top.returnValue = 'OK';")
            End If

            If rbEmployeeWait.Checked Then
                If funCheckData1(strFileName) Then
                    File.Delete(strFileName)
                    strMessage = "HR3000資料上傳總筆數：" & intTotalCount & "，成功筆數：" & intSuccessCount & "，警告筆數：" & intWarningCount & "，失敗筆數：" & intErrorCount
                    Bsp.Utility.RunClientScript(Me, "alert('" & String.Format(Bsp.Utility.getMessage("I_00020"), strMessage) & "');" & vbCrLf & _
                                "window.top.returnValue = 'OK';" & vbCrLf & "window.top.close();")
                Else
                    '刪除上傳檔案
                    File.Delete(strFileName)
                    strMessage = "HR3000資料上傳總筆數：" & intTotalCount & "，成功筆數：" & intSuccessCount & "，警告筆數：" & intWarningCount & "，失敗筆數：" & intErrorCount & "，請下載錯誤紀錄log檔案!"
                    '顯示上傳失敗錯誤訊息，並下載ERR LOG檔案
                    Bsp.Utility.RunClientScript(Me.Page, "IsTODownload('" & strMessage & "');")

                End If
            End If
            '20150609 wei add
            If rbEmpAdditionWait.Checked Then
                If funCheckData_ByEmpAdditionWait(strFileName) Then
                    File.Delete(strFileName)
                    strMessage = "HR3000_EmpAddition資料上傳總筆數：" & intTotalCount & "，成功筆數：" & intSuccessCount & "，警告筆數：" & intWarningCount & "，失敗筆數：" & intErrorCount
                    Bsp.Utility.RunClientScript(Me, "alert('" & String.Format(Bsp.Utility.getMessage("I_00020"), strMessage) & "');" & vbCrLf & _
                                "window.top.returnValue = 'OK';" & vbCrLf & "window.top.close();")
                Else
                    '刪除上傳檔案
                    File.Delete(strFileName)
                    strMessage = "HR3000_EmpAddition資料上傳總筆數：" & intTotalCount & "，成功筆數：" & intSuccessCount & "，警告筆數：" & intWarningCount & "，失敗筆數：" & intErrorCount & "，請下載錯誤紀錄log檔案!"
                    '顯示上傳失敗錯誤訊息，並下載ERR LOG檔案
                    Bsp.Utility.RunClientScript(Me.Page, "IsTODownload('" & strMessage & "');")

                End If
            End If
            


        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & ".DoUpload", ex)
        End Try

    End Sub

    'Check
    '20150609 wei modify HR3000-EmployeeWait
    Private Function funCheckData1(ByVal FileName As String) As Boolean

        Dim objHR As New HR
        Dim objHR3000 As New HR3000
        Dim bolInputStatus As Boolean = True
        Dim strErrMsg As String = ""
        Dim strSqlWhere As String = ""
        Dim strValidDate As String = ""

        Dim intLoop As Integer = 0

        Dim bolCheckData As Boolean = False  '檢核資料

        '畫面輸入的條件

        '"HR3000上傳作業 _分隔符號『  |  』)"			
        'NO.	名稱	        格式    長度    必填	說明
        '1	公司代碼	        字串    6       Y
        '2	員工編號	        字串    6       Y	
        '3	生效日期	        日期    10      Y   YYYY/MM/DD
        '4	異動原因代碼	    字串    6       Y        	
        '5	異動後公司代碼	字串    6        	
        '6	異動後部門代碼	字串    12        	
        '7	異動後科組課代碼 	字串    12        	
        '8	職等代碼       	字串    2       Y        	
        '9	職稱代碼        	字串    1       Y 	
        '10	職位代碼        	字串    50        	以"|"符號隔開,1:主要|0:兼任,只有一筆主要職位 ex:1|03002,有一筆主要職位&多筆兼任職位 EX:1|030002|0|03003|0|030004
        '11	工作性質代碼    	字串    50        	以"|"符號隔開,1:主要|0:兼任,只有一筆主要工作性質 ex:1|03002,有一筆主要工作性質&多筆兼任工作性質 EX:1|030002|0|03003|0|030004
        '12	主管註記        	字串    1        	0:否，1:是
        '13	簽核單位主管註記	字串    1   	        0:否，1:是
        '14	最小簽核單位代碼 字串    8        	
        '15	工作地點代碼  	字串    3        	
        '16	班別代碼        	字串    2        	
        '17	備註          	字串    100         最多可輸入50個中文字或100個英數字    	
        '18	主管任用方式  	字串    1        	1 主要 2 兼任
        '19	副主管         	字串    1        	0:否，1:是
        '20	簽核單位副主管  	字串    1        	0:否，1:是
        '21 應試者編號       字串    14          異動(復職)員工需與招募系統待報到應試者資料勾稽用       
        '22 預計報到日       日期    10          YYYY/MM/DD

        '補充說明：欄位非必填且放空白	
        Dim nintCompID As Integer = 0
        Dim nintEmpID As Integer = 1
        Dim nintValidDate As Integer = 2
        Dim nintReason As Integer = 3
        Dim nintCompIDNew As Integer = 4
        Dim nintDeptID As Integer = 5
        Dim nintOrganID As Integer = 6
        Dim nintRankID As Integer = 7
        Dim nintTitleID As Integer = 8
        Dim nintPositionID As Integer = 9
        Dim nintWorkTypeID As Integer = 10
        Dim nintIsBoss As Integer = 11
        Dim nintIsGroupBoss As Integer = 12
        Dim nintFlowOrganID As Integer = 13
        Dim nintEmpFlowRemarkID As Integer = 14
        Dim nintWorkSiteID As Integer = 15
        Dim nintWTID As Integer = 16
        Dim nintRemark As Integer = 17
        Dim nintBossType As Integer = 18
        Dim nintIsSecBoss As Integer = 19
        Dim nintIsSecGroupBoss As Integer = 20
        Dim nintRecID As Integer = 21
        Dim nintContractDate As Integer = 22

        intTotalCount = 0
        intSuccessCount = 0
        intErrorCount = 0
        intWarningCount = 0

        Dim strExcelConn As String = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & FileName & ";Extended Properties='EXCEL 8.0;HDR=Yes;IMEX=1;'"    '20150401 wei modify

        Dim strExcelWorkSheet As String = "[HR3000$]"
        Dim strExcelSelect As String = "SELECT * FROM " & strExcelWorkSheet

        Dim myExcelConn As OleDbConnection = Nothing
        myExcelConn = New OleDbConnection(strExcelConn)
        Dim myExcelCommand As OleDbCommand = New OleDbCommand(strExcelSelect, myExcelConn)
        Try
            myExcelConn.Open()
            Dim strIDNo As String = ""
            Dim strEmpDate As String = ""       '20150605 wei add
            Dim strReason As String = ""        '20150605 wei add
            Dim strCompIDNew As String = ""     '20160824 Beatrice Add
            Dim strRankID As String = ""        '20160920 Beatrice Add
            Dim bolBoss As Boolean = False      '20150601 wei add 判斷工作性質是否符合BA0001單位主管，BSN021區督導，BSN000分行經理
            Dim bolOPBoss As Boolean = False    '20150601 wei add 判斷工作性質是否符合 BO0001作業主管
            Using myDataRead As OleDbDataReader = myExcelCommand.ExecuteReader
                While myDataRead.Read

                    '檢核上傳資料
                    strErrMsg = ""
                    bolCheckData = True
                    strValidDate = ""
                    strIDNo = ""
                    strEmpDate = ""                     '20150605 wei add
                    strReason = ""                      '20150605 wei add
                    strCompIDNew = ""                   '20160824 Beatrice Add
                    strRankID = ""                      '20160920 Beatrice Add
                    bolBoss = False
                    bolOPBoss = False
                    If Trim(myDataRead(nintCompID).ToString) <> "" And Trim(myDataRead(nintEmpID).ToString) <> "" Then
                        Using dt As DataTable = objHR3000.GetEmpDataByHR3000(Trim(myDataRead(nintCompID).ToString), Trim(myDataRead(nintEmpID).ToString))
                            If dt.Rows.Count > 0 Then
                                strIDNo = dt.Rows.Item(0)("IDNo").ToString()
                                strEmpDate = dt.Rows.Item(0)("EmpDate").ToString()
                                strRankID = dt.Rows.Item(0)("RankID").ToString()
                            End If
                        End Using
                    End If

                    '先判斷是否上傳欄位個數正確
                    If myDataRead.FieldCount <> 23 Then
                        strErrMsg = strErrMsg & "==> 上傳資料欄位個數不正確！"
                        intErrorCount = intErrorCount + 1
                    Else
                        '公司代碼
                        strSqlWhere = ""
                        strSqlWhere = strSqlWhere & " and CompID =" & Bsp.Utility.Quote(Trim(myDataRead(nintCompID).ToString))
                        If Not objHR.IsDataExists("Company", strSqlWhere) Then
                            strErrMsg = strErrMsg & "==> " & Trim(myDataRead(nintCompID).ToString) & " 查無公司代碼!"
                            bolCheckData = False
                        End If
                        '員工編號
                        strSqlWhere = ""
                        strSqlWhere = strSqlWhere & " and CompID =" & Bsp.Utility.Quote(Trim(myDataRead(nintCompID).ToString))
                        strSqlWhere = strSqlWhere & " and EmpID =" & Bsp.Utility.Quote(Trim(myDataRead(nintEmpID).ToString))
                        If Not objHR.IsDataExists("Personal", strSqlWhere) Then
                            strErrMsg = strErrMsg & "==> " & Trim(myDataRead(nintEmpID).ToString) & " 員工主檔不存在!"
                            bolCheckData = False
                        End If

                        '異動原因代碼
                        strSqlWhere = ""
                        strSqlWhere = strSqlWhere & " and EmployeeWaitFlag='1' and Reason =" & Bsp.Utility.Quote(Trim(myDataRead(nintReason).ToString))
                        If Not objHR.IsDataExists("EmployeeReason", strSqlWhere) Then
                            strErrMsg = strErrMsg & "==> " & Trim(myDataRead(nintReason).ToString) & " 查無異動原因代碼!"
                            bolCheckData = False
                        Else
                            strReason = Trim(myDataRead(nintReason).ToString)
                        End If

                        '20150528 wei add 02,03,07跨公司復職，要檢核非在職(<>1)
                        If bolCheckData And (strReason = "02" Or strReason = "03" Or strReason = "07") Then
                            strSqlWhere = " And CompID = " & Bsp.Utility.Quote(Trim(myDataRead(nintCompID).ToString)) & " And EmpID = " & Bsp.Utility.Quote(Trim(myDataRead(nintEmpID).ToString))
                            strSqlWhere &= " And WorkStatus<>'1'"
                            If Not objHR.IsDataExists("Personal", strSqlWhere) Then
                                strErrMsg = strErrMsg & "==> 異動原因：" & Trim(myDataRead(nintReason).ToString) & " 輸入錯誤，人員需為不在職!"
                                bolCheckData = False
                            End If
                        End If

                        '20150528 wei add	70跨公司調動，要檢核異動前公司為在職(=1)，防呆輸入07
                        '20150528 wei add	11，要檢核在職(=1)
                        If bolCheckData And (strReason = "11" Or strReason = "70") Then
                            strSqlWhere = " And CompID = " & Bsp.Utility.Quote(Trim(myDataRead(nintCompID).ToString)) & " And EmpID = " & Bsp.Utility.Quote(Trim(myDataRead(nintEmpID).ToString))
                            strSqlWhere &= " And WorkStatus='1'"
                            If Not objHR.IsDataExists("Personal", strSqlWhere) Then
                                strErrMsg = strErrMsg & "==> 異動原因：" & Trim(myDataRead(nintReason).ToString) & " 輸入錯誤，人員需為在職!"
                                bolCheckData = False
                            End If
                        End If

                        '20150528 wei add	12留停會延長,13，要檢核在職或留停(=1,2)
                        If bolCheckData And (strReason = "12" Or strReason = " 13") Then
                            strSqlWhere = " And CompID = " & Bsp.Utility.Quote(Trim(myDataRead(nintCompID).ToString)) & " And EmpID = " & Bsp.Utility.Quote(Trim(myDataRead(nintEmpID).ToString))
                            strSqlWhere &= " And WorkStatus in ('1','2')"
                            If Not objHR.IsDataExists("Personal", strSqlWhere) Then
                                strErrMsg = strErrMsg & "==> 異動原因：" & Trim(myDataRead(nintReason).ToString) & " 輸入錯誤，人員需為在職或留停!"
                                bolCheckData = False
                            End If
                        End If

                        '生效日期
                        strValidDate = Trim(myDataRead(nintValidDate).ToString)
                        If Trim(myDataRead(nintValidDate).ToString) = "" Then
                            strErrMsg = strErrMsg & "==> 生效日期不能為空白！"
                            bolCheckData = False
                            strValidDate = ""
                        Else
                            If Not IsDate(Trim(myDataRead(nintValidDate).ToString)) Then
                                strErrMsg = strErrMsg & "==> " & Trim(myDataRead(nintValidDate).ToString) & " 生效日期格式錯誤！"
                                bolCheckData = False
                                strValidDate = ""
                            Else
                                If strEmpDate <> "" Then
                                    If CDate(strValidDate) < CDate(strEmpDate) Then
                                        strErrMsg = strErrMsg & "==> 生效日期：" & Trim(myDataRead(nintValidDate).ToString) & " 輸入錯誤，必須大於到職日！"
                                        bolCheckData = False
                                        strValidDate = ""
                                    End If
                                End If
                            End If
                        End If

                        '異動後公司代碼
                        If strReason = "40" Then '20160824 Beatrice Add
                            strCompIDNew = Trim(myDataRead(nintCompID).ToString)
                            If Trim(myDataRead(nintCompIDNew).ToString) <> "" Then
                                strErrMsg = strErrMsg & "==> 異動後公司：" & Trim(myDataRead(nintCompIDNew).ToString) & " 異動原因為40升等，僅處理【職等職稱】/【工作性質】/【職位】"
                                bolCheckData = False
                            End If
                        Else
                            strCompIDNew = Trim(myDataRead(nintCompIDNew).ToString)
                            strSqlWhere = ""
                            strSqlWhere = strSqlWhere & " and CompID =" & Bsp.Utility.Quote(Trim(myDataRead(nintCompIDNew).ToString))
                            If Not objHR.IsDataExists("Company", strSqlWhere) Then
                                strErrMsg = strErrMsg & "==> " & Trim(myDataRead(nintCompIDNew).ToString) & " 查無異動後公司代碼!"
                                bolCheckData = False
                            End If
                            '20150529 wei add 70跨公司調動，增加檢核異動前後公司需不相同
                            If strReason = "70" Then
                                If Trim(myDataRead(nintCompIDNew).ToString) = Trim(myDataRead(nintCompID).ToString) Then
                                    strErrMsg = strErrMsg & "==> 異動後公司：" & Trim(myDataRead(nintCompIDNew).ToString) & " 輸入錯誤，異動原因為70時，前後公司不得相同!"
                                    bolCheckData = False
                                End If
                            End If
                        End If

                        '異動後部門代碼
                        If strReason = "40" Then '20160824 Beatrice Add
                            If Trim(myDataRead(nintDeptID).ToString) <> "" Then
                                strErrMsg = strErrMsg & "==> 部門：" & Trim(myDataRead(nintDeptID).ToString) & " 異動原因為40升等，僅處理【職等職稱】/【工作性質】/【職位】"
                                bolCheckData = False
                            End If
                        Else
                            strSqlWhere = ""
                            strSqlWhere = strSqlWhere & " and CompID =" & Bsp.Utility.Quote(Trim(myDataRead(nintCompIDNew).ToString))
                            strSqlWhere = strSqlWhere & " and OrganID =" & Bsp.Utility.Quote(Trim(myDataRead(nintDeptID).ToString))
                            If Not objHR.IsDataExists("Organization", strSqlWhere) Then
                                '20161121 leo modify IsDataExists(OrganizationWait&EmployeeWait) Start
                                strSqlWhere = ""
                                strSqlWhere = strSqlWhere & " and CompID =" & Bsp.Utility.Quote(Trim(myDataRead(nintCompIDNew).ToString))
                                strSqlWhere = strSqlWhere & " and OrganID =" & Bsp.Utility.Quote(Trim(myDataRead(nintDeptID).ToString))
                                strSqlWhere = strSqlWhere & " and ValidDate <=" & Bsp.Utility.Quote(Trim(myDataRead(nintValidDate).ToString))
                                strSqlWhere = strSqlWhere & " and OrganType in ('1','3')  and WaitStatus='0' "
                                If Not objHR.IsDataExists("OrganizationWait", strSqlWhere & "and OrganReason = '1'") Then
                                    strErrMsg = strErrMsg & "==> 部門：" & Trim(myDataRead(nintCompIDNew).ToString) & "/" & _
                                      Trim(myDataRead(nintDeptID).ToString) & " 查無異動後部門代碼!"
                                    bolCheckData = False
                                End If
                                
                                '20161121 leo modify IsDataExists(OrganizationWait&EmployeeWait) End
                            Else
                                If Not (strReason = "11" Or strReason = "12" Or strReason = "13" Or strReason = "14" Or strReason = "15" Or strReason = "16" Or strReason = "17" Or strReason = "18") Then  '20150529 wei add
                                    If Not objHR.ChkOrganIsVlaid(Trim(myDataRead(nintCompIDNew).ToString), Trim(myDataRead(nintDeptID).ToString)) Then
                                        strErrMsg = strErrMsg & "==> 部門：" & Trim(myDataRead(nintCompIDNew).ToString) & "/" & Trim(myDataRead(nintDeptID).ToString) & " 上傳部門是無效單位!"
                                        bolCheckData = False
                                    End If
                                End If
                                '20150529 wei add
                                If strReason = "33" Then
                                    If Trim(myDataRead(nintDeptID).ToString) <> objHR.GetHREmpDeptID(Trim(myDataRead(nintCompID).ToString), Trim(myDataRead(nintEmpID).ToString)).Rows(0)("DeptID").ToString Then
                                        strErrMsg = strErrMsg & "==> 部門：" & Trim(myDataRead(nintCompIDNew).ToString) & "/" & Trim(myDataRead(nintDeptID).ToString) & " 輸入錯誤，異動原因為33時，前後部門需相同!"
                                        bolCheckData = False
                                    End If
                                End If
                                '20150529 wei add
                                If strReason = "50" Then
                                    If Trim(myDataRead(nintDeptID).ToString) = objHR.GetHREmpDeptID(Trim(myDataRead(nintCompID).ToString), Trim(myDataRead(nintEmpID).ToString)).Rows(0)("DeptID").ToString Then
                                        strErrMsg = strErrMsg & "==> 部門：" & Trim(myDataRead(nintCompIDNew).ToString) & "/" & Trim(myDataRead(nintDeptID).ToString) & " 輸入錯誤，異動原因為50時，前後部門不得相同!"
                                        bolCheckData = False
                                    End If
                                End If
                            End If
                            If objHR.IsDataExists("OrganizationWait", strSqlWhere & "and OrganReason = '2'") Then
                                strErrMsg = strErrMsg & "==> 部門：" & Trim(myDataRead(nintCompIDNew).ToString) & "/" & _
                                    Trim(myDataRead(nintDeptID).ToString) & "異動後部門代碼，已在組織待異動設定無效!"
                                bolCheckData = False
                            End If
                        End If


                        '異動後科組課代碼
                        If strReason = "40" Then '20160824 Beatrice Add
                            If Trim(myDataRead(nintOrganID).ToString) <> "" Then
                                strErrMsg = strErrMsg & "==> 科/組/課：" & Trim(myDataRead(nintOrganID).ToString) & " 異動原因為40升等，僅處理【職等職稱】/【工作性質】/【職位】"
                                bolCheckData = False
                            End If
                        Else
                            strSqlWhere = ""
                            strSqlWhere = strSqlWhere & " and CompID =" & Bsp.Utility.Quote(Trim(myDataRead(nintCompIDNew).ToString))
                            strSqlWhere = strSqlWhere & " and DeptID =" & Bsp.Utility.Quote(Trim(myDataRead(nintDeptID).ToString))
                            strSqlWhere = strSqlWhere & " and OrganID =" & Bsp.Utility.Quote(Trim(myDataRead(nintOrganID).ToString))
                            If Not objHR.IsDataExists("Organization", strSqlWhere) Then
                                '20161121 leo modify IsDataExists(OrganizationWait&EmployeeWait) Start
                                strSqlWhere = ""
                                strSqlWhere = strSqlWhere & " and CompID =" & Bsp.Utility.Quote(Trim(myDataRead(nintCompIDNew).ToString))
                                strSqlWhere = strSqlWhere & " and OrganID =" & Bsp.Utility.Quote(Trim(myDataRead(nintOrganID).ToString))
                                strSqlWhere = strSqlWhere & " and ValidDate <=" & Bsp.Utility.Quote(Trim(myDataRead(nintValidDate).ToString))
                                strSqlWhere = strSqlWhere & " and OrganType in ('1','3')  and WaitStatus='0' "
                                If Not objHR.IsDataExists("OrganizationWait", strSqlWhere & "and OrganReason = '1'") Then
                                    strErrMsg = strErrMsg & "==> 科/組/課：" & Trim(myDataRead(nintCompIDNew).ToString) & "/" & _
                                      Trim(myDataRead(nintOrganID).ToString) & " 查無異動後科組課代碼!"
                                    bolCheckData = False
                                End If
                                
                                '20161124 leo modify IsDataExists(OrganizationWait&EmployeeWait) End
                                'strErrMsg = strErrMsg & "==> " & Trim(myDataRead(nintCompIDNew).ToString) & "/" & Trim(myDataRead(nintOrganID).ToString) & " 查無異動後科組課代碼!"
                                'bolCheckData = False
                            Else
                                If Not (strReason = "11" Or strReason = "12" Or strReason = "13" Or strReason = "14" Or strReason = "15" Or strReason = "16" Or strReason = "17" Or strReason = "18") Then  '20150529 wei add
                                    If Not objHR.ChkOrganIsVlaid(Trim(myDataRead(nintCompIDNew).ToString), Trim(myDataRead(nintOrganID).ToString)) Then
                                        strErrMsg = strErrMsg & "==> 科/組/課：" & Trim(myDataRead(nintCompIDNew).ToString) & "/" & Trim(myDataRead(nintOrganID).ToString) & " 上傳的科/組/課是無效單位!"
                                        bolCheckData = False
                                    End If
                                End If
                            End If
                            If objHR.IsDataExists("OrganizationWait", strSqlWhere & "and OrganReason = '2'") Then
                                strErrMsg = strErrMsg & "==> 科/組/課：" & Trim(myDataRead(nintCompIDNew).ToString) & "/" & _
                                    Trim(myDataRead(nintOrganID).ToString) & "異動後科組課代碼，已在組織待異動設定無效!"
                                bolCheckData = False
                            End If
                        End If

                        '20150609 wei add 事業群
                        '2016/05/03 SPHBKC資料已併入OrganizationFlow中
                        'If Trim(myDataRead(nintCompIDNew).ToString) = "SPHBKC" Then
                        '    If objHR3000.Get_CGroupID(Trim(myDataRead(nintOrganID).ToString)) = "" Then
                        '        strErrMsg = strErrMsg & "==> " & Trim(myDataRead(nintCompIDNew).ToString) & "/" & Trim(myDataRead(nintOrganID).ToString) & " 查無異動後事業群代碼!"
                        '        bolCheckData = False
                        '    End If
                        'Else
                        If strReason <> "40" Then '20160824 Beatrice Add
                            If objHR3000.Get_GroupID(Trim(myDataRead(nintOrganID).ToString)) = "" Then
                                strErrMsg = strErrMsg & "==> " & Trim(myDataRead(nintCompIDNew).ToString) & "/" & Trim(myDataRead(nintOrganID).ToString) & " 查無異動後事業群代碼!"
                                bolCheckData = False
                            End If
                        End If
                        'End If

                        '職等職稱
                        '20150529 wei modify 異動後非在職原因(11,12,13,14,15,16,17,18)改非必填，除70跨公司調動
                        If Not (strReason = "11" Or strReason = "12" Or strReason = "13" Or strReason = "14" Or strReason = "15" Or strReason = "16" Or strReason = "17" Or strReason = "18") Then
                            If strReason = "40" And Trim(myDataRead(nintRankID).ToString) = "" Then
                                strErrMsg = strErrMsg & "==> " & Trim(myDataRead(nintRankID).ToString) & " 職等代碼不能空白!"
                                bolCheckData = False
                            End If
                            If strReason = "40" And Trim(myDataRead(nintTitleID).ToString) = "" Then
                                strErrMsg = strErrMsg & "==> " & Trim(myDataRead(nintTitleID).ToString) & " 職稱代碼不能空白!"
                                bolCheckData = False
                            End If
                            If strReason = "40" And strCompIDNew = "SPHBK1" And strRankID = Trim(myDataRead(nintRankID).ToString) Then
                                strErrMsg = strErrMsg & "==> 職等：" & Trim(myDataRead(nintRankID).ToString) & " (銀行)異動原因為40升等，職等不可與現有的相同!"
                                bolCheckData = False
                            End If
                            '職等代碼
                            strSqlWhere = ""
                            strSqlWhere = strSqlWhere & " and CompID =" & Bsp.Utility.Quote(strCompIDNew)
                            strSqlWhere = strSqlWhere & " and RankID =" & Bsp.Utility.Quote(Trim(myDataRead(nintRankID).ToString))
                            If Not objHR.IsDataExists("Rank", strSqlWhere) And (strCompIDNew <> Trim(myDataRead(nintCompID).ToString)) Then
                                strErrMsg = strErrMsg & "==> " & strCompIDNew & "/" & Trim(myDataRead(nintRankID).ToString) & " 查無職等代碼!"
                                bolCheckData = False
                            End If
                            '職稱代碼
                            strSqlWhere = ""
                            strSqlWhere = strSqlWhere & " and CompID =" & Bsp.Utility.Quote(strCompIDNew)
                            strSqlWhere = strSqlWhere & " and RankID =" & Bsp.Utility.Quote(Trim(myDataRead(nintRankID).ToString))
                            strSqlWhere = strSqlWhere & " and TitleID =" & Bsp.Utility.Quote(Trim(myDataRead(nintTitleID).ToString))
                            If Not objHR.IsDataExists("Title", strSqlWhere) And (strCompIDNew <> Trim(myDataRead(nintCompID).ToString)) Then
                                strErrMsg = strErrMsg & "==> " & strCompIDNew & "/" & Trim(myDataRead(nintRankID).ToString) & "/" & Trim(myDataRead(nintTitleID).ToString) & " 查無職稱代碼!"
                                bolCheckData = False
                            End If
                        End If

                        If strReason <> "70" Then
                            '職位代碼
                            '高階主管和一般員工且異動後公司有導入惠悅必須選擇職位
                            If strReason = "40" And Trim(myDataRead(nintPositionID).ToString) = "" Then '20160824 Beatrice Add
                                strErrMsg = strErrMsg & "==> " & Trim(myDataRead(nintPositionID).ToString) & " 職位代碼不能空白!"
                                bolCheckData = False
                            Else
                                If objHR.IsRankIDMapFlag(strCompIDNew) Then
                                    If Trim(myDataRead(nintPositionID).ToString) <> "" Then
                                        Dim aryPositionID = Split(Trim(myDataRead(nintPositionID).ToString), "|", -1, CompareMethod.Text)
                                        If UBound(aryPositionID) = 0 Or ((UBound(aryPositionID) + 1) Mod 2) <> 0 Then
                                            strErrMsg = strErrMsg & "==> " & Trim(myDataRead(nintPositionID).ToString) & " 上傳職位格式不正確ex:1|主要|0|兼任!"
                                            bolCheckData = False
                                        Else
                                            If UBound(aryPositionID) > 19 Then
                                                strErrMsg = strErrMsg & "==> " & Trim(myDataRead(nintPositionID).ToString) & " 職位不可超過10個!"
                                                bolCheckData = False
                                            End If
                                            For iPosition As Integer = 1 To aryPositionID.GetUpperBound(0) Step 2
                                                strSqlWhere = ""
                                                strSqlWhere = strSqlWhere & " and CompID =" & Bsp.Utility.Quote(strCompIDNew)
                                                strSqlWhere = strSqlWhere & " and PositionID =" & Bsp.Utility.Quote(Trim(aryPositionID(iPosition)))
                                                If Not objHR.IsDataExists("Position", strSqlWhere) Then
                                                    strErrMsg = strErrMsg & "==> " & Trim(myDataRead(nintPositionID).ToString) & " 職位代碼不存在!"
                                                    bolCheckData = False
                                                Else
                                                    If Not (strReason = "11" Or strReason = "12" Or strReason = "14" Or strReason = "15" Or strReason = "16" Or strReason = "17" Or strReason = "18") Then  '20150529 wei add
                                                        If Not objHR.ChkPositionIsVlaid(strCompIDNew, Trim(aryPositionID(iPosition))) And Trim(aryPositionID(iPosition)) <> "" Then
                                                            strErrMsg = strErrMsg & "==> 職位：" & Trim(myDataRead(nintPositionID).ToString) & " 上傳職位中有無效職位!"
                                                            bolCheckData = False
                                                        End If
                                                    End If
                                                End If
                                            Next
                                        End If
                                    End If
                                Else
                                    If (strCompIDNew <> Trim(myDataRead(nintCompID).ToString)) Then
                                        strErrMsg = strErrMsg & "==> " & Trim(myDataRead(nintPositionID).ToString) & " 職位代碼不能空白!"
                                        bolCheckData = False
                                    End If
                                End If
                            End If

                            '工作性質代碼
                            '高階主管和一般員工必須選擇工作性質
                            If Trim(myDataRead(nintWorkTypeID).ToString) <> "" Then
                                Dim aryWorkTypeID = Split(Trim(myDataRead(nintWorkTypeID).ToString), "|", -1, CompareMethod.Text)
                                If UBound(aryWorkTypeID) = 0 Or ((UBound(aryWorkTypeID) + 1) Mod 2) <> 0 Then
                                    strErrMsg = strErrMsg & "==> " & Trim(myDataRead(nintWorkTypeID).ToString) & " 上傳工作性質格式不正確ex:1|主要|0|兼任!"
                                    bolCheckData = False
                                Else
                                    If UBound(aryWorkTypeID) > 19 Then
                                        strErrMsg = strErrMsg & "==> " & Trim(myDataRead(nintPositionID).ToString) & " 工作性質不可超過10個!"
                                        bolCheckData = False
                                    End If
                                    For iWorkType As Integer = 1 To aryWorkTypeID.GetUpperBound(0) Step 2
                                        strSqlWhere = ""
                                        strSqlWhere = strSqlWhere & " and CompID =" & Bsp.Utility.Quote(strCompIDNew)
                                        strSqlWhere = strSqlWhere & " and WorkTypeID =" & Bsp.Utility.Quote(Trim(aryWorkTypeID(iWorkType)))
                                        If Not objHR.IsDataExists("WorkType", strSqlWhere) Then
                                            strErrMsg = strErrMsg & "==> " & Trim(myDataRead(nintWorkTypeID).ToString) & " 工作性質代碼不存在!"
                                            bolCheckData = False
                                        Else
                                            If Not (strReason = "11" Or strReason = "12" Or strReason = "14" Or strReason = "15" Or strReason = "16" Or strReason = "17" Or strReason = "18") Then  '20150529 wei add
                                                If Not objHR.ChkWorkTypeIsVlaid(strCompIDNew, Trim(aryWorkTypeID(iWorkType))) And Trim(aryWorkTypeID(iWorkType)) <> "" Then
                                                    strErrMsg = strErrMsg & "==> 工作性質：" & Trim(myDataRead(nintWorkTypeID).ToString) & " 上傳工作性質中有無效工作性質!"
                                                    bolCheckData = False
                                                End If
                                            End If
                                            ''20150601 wei add 判斷工作性質是否符合BA0001單位主管，BSN021區督導，BSN000分行經理
                                            If Trim(aryWorkTypeID(iWorkType)) = "BA0001" Or Trim(aryWorkTypeID(iWorkType)) = "BSN021" Or Trim(aryWorkTypeID(iWorkType)) = "BSN000" Then
                                                bolBoss = True
                                            End If
                                            If Trim(aryWorkTypeID(iWorkType)) = "BO0001" Then
                                                bolOPBoss = True
                                            End If
                                        End If
                                    Next
                                End If
                            Else
                                If strReason = "40" Or (strCompIDNew <> Trim(myDataRead(nintCompID).ToString)) Then
                                    strErrMsg = strErrMsg & "==> " & Trim(myDataRead(nintWorkTypeID).ToString) & " 工作性質代碼不能空白!"
                                    bolCheckData = False
                                End If
                            End If
                        End If

                        '主管註記
                        If strReason = "40" Then '20160824 Beatrice Add
                            If Trim(myDataRead(nintIsBoss).ToString) <> "" Then
                                strErrMsg = strErrMsg & "==> 主管註記：" & Trim(myDataRead(nintIsBoss).ToString) & " 異動原因為40升等，僅處理【職等職稱】/【工作性質】/【職位】"
                                bolCheckData = False
                            End If
                        Else
                            If Trim(myDataRead(nintIsBoss).ToString) <> "" And Trim(myDataRead(nintIsBoss).ToString) <> "0" And Trim(myDataRead(nintIsBoss).ToString) <> "1" Then
                                strErrMsg = strErrMsg & "==> " & Trim(myDataRead(nintIsBoss).ToString) & " 主管註記(0:否，1:是)資料錯誤!"
                                bolCheckData = False
                            End If
                        End If

                        '簽核單位主管註記
                        If strReason = "40" Then '20160824 Beatrice Add
                            If Trim(myDataRead(nintIsGroupBoss).ToString) <> "" Then
                                strErrMsg = strErrMsg & "==> 簽核單位主管註記：" & Trim(myDataRead(nintIsGroupBoss).ToString) & " 異動原因為40升等，僅處理【職等職稱】/【工作性質】/【職位】"
                                bolCheckData = False
                            End If
                        Else
                            If Trim(myDataRead(nintIsGroupBoss).ToString) <> "" And Trim(myDataRead(nintIsGroupBoss).ToString) <> "0" And Trim(myDataRead(nintIsGroupBoss).ToString) <> "1" Then
                                strErrMsg = strErrMsg & "==> " & Trim(myDataRead(nintIsGroupBoss).ToString) & " 簽核單位主管註記(0:否，1:是)資料錯誤!"
                                bolCheckData = False
                            End If
                        End If
                        '以下異動原因必需選取[最小簽核單位]
                        '1-1.系統檢核最小簽核單位都必填
                        '1-2.若異動後任職狀況=2,3,6的單位可輸入無效單位EX:留停延長
                        '最小簽核單位代碼
                        If strReason = "40" Then '20160824 Beatrice Add
                            If Trim(myDataRead(nintFlowOrganID).ToString) <> "" Then
                                strErrMsg = strErrMsg & "==> 最小簽核單位代碼：" & Trim(myDataRead(nintFlowOrganID).ToString) & " 異動原因為40升等，僅處理【職等職稱】/【工作性質】/【職位】"
                                bolCheckData = False
                            End If
                        Else
                            strSqlWhere = ""
                            strSqlWhere = strSqlWhere & " and OrganID =" & Bsp.Utility.Quote(Trim(myDataRead(nintFlowOrganID).ToString))
                            If Not objHR.IsDataExists("OrganizationFlow", strSqlWhere) Then
                                strErrMsg = strErrMsg & "==> " & Trim(myDataRead(nintFlowOrganID).ToString) & " 查無最小簽核單位代碼!"
                                bolCheckData = False
                            Else
                                If Not (strReason = "11" Or strReason = "12" Or strReason = "13" Or strReason = "14" Or strReason = "15" Or strReason = "16" Or strReason = "17" Or strReason = "18") Then  '20150601 wei add
                                    If Not objHR.ChkOrganFlowIsVlaid(Trim(myDataRead(nintCompIDNew).ToString), Trim(myDataRead(nintFlowOrganID).ToString)) Then
                                        strErrMsg = strErrMsg & "==> 最小簽核單位：" & Trim(myDataRead(nintFlowOrganID).ToString) & " 上傳的最小簽核部門是無效單位!"
                                        bolCheckData = False
                                    End If
                                End If
                            End If
                        End If

                        If strReason = "40" Then '20161104 Beatrice Add
                            If Trim(myDataRead(nintEmpFlowRemarkID).ToString) <> "" Then
                                strErrMsg = strErrMsg & "==> 功能備註代碼：" & Trim(myDataRead(nintFlowOrganID).ToString) & " 異動原因為40升等，僅處理【職等職稱】/【工作性質】/【職位】"
                                bolCheckData = False
                            End If
                        Else
                            Dim strBusiness As String = objHR3000.GetBusinessType(Trim(myDataRead(nintCompIDNew).ToString), Trim(myDataRead(nintFlowOrganID).ToString)).Trim()

                            If strBusiness <> "" And Trim(myDataRead(nintEmpFlowRemarkID).ToString) = "" Then
                                strErrMsg = strErrMsg & "==> 功能備註代碼不能空白!"
                                bolCheckData = False
                            ElseIf strBusiness = "" And Trim(myDataRead(nintEmpFlowRemarkID).ToString) <> "" Then
                                strErrMsg = strErrMsg & "==> 功能備註代碼不可上傳!"
                                bolCheckData = False
                            ElseIf strBusiness <> "" And Trim(myDataRead(nintEmpFlowRemarkID).ToString) <> "" Then
                                strSqlWhere = ""
                                strSqlWhere = strSqlWhere & " and Code =" & Bsp.Utility.Quote(Trim(myDataRead(nintEmpFlowRemarkID).ToString))
                                strSqlWhere = strSqlWhere & " and TabName = 'EmpFlowRemark' and FldName = '" + strBusiness + "' and NotShowFlag = '0'"

                                If Not objHR.IsDataExists("HRCodeMap", strSqlWhere) Then
                                    strErrMsg = strErrMsg & "==> 業務類別：" & strBusiness & "，功能備註代碼：" & Trim(myDataRead(nintEmpFlowRemarkID).ToString) & " 查無功能備註代碼!"
                                    bolCheckData = False
                                End If
                            End If
                        End If

                        '工作地點代碼
                        If strReason = "40" Then '20160824 Beatrice Add
                            If Trim(myDataRead(nintWorkSiteID).ToString) <> "" Then
                                strErrMsg = strErrMsg & "==> 工作地點代碼：" & Trim(myDataRead(nintWorkSiteID).ToString) & " 異動原因為40升等，僅處理【職等職稱】/【工作性質】/【職位】"
                                bolCheckData = False
                            End If
                        Else
                            If Trim(myDataRead(nintWorkSiteID).ToString) <> "" Then
                                strSqlWhere = ""
                                strSqlWhere = strSqlWhere & " and CompID =" & Bsp.Utility.Quote(Trim(myDataRead(nintCompIDNew).ToString))
                                strSqlWhere = strSqlWhere & " and WorkSiteID =" & Bsp.Utility.Quote(Trim(myDataRead(nintWorkSiteID).ToString))
                                If Not objHR.IsDataExists("WorkSite", strSqlWhere) Then
                                    strErrMsg = strErrMsg & "==> " & Trim(myDataRead(nintCompIDNew).ToString) & "/" & Trim(myDataRead(nintWorkSiteID).ToString) & " 查無工作地點代碼!"
                                    bolCheckData = False
                                End If
                            End If
                        End If

                        '班別代碼
                        If strReason = "40" Then '20160824 Beatrice Add
                            If Trim(myDataRead(nintWTID).ToString) <> "" Then
                                strErrMsg = strErrMsg & "==> 班別代碼：" & Trim(myDataRead(nintWTID).ToString) & " 異動原因為40升等，僅處理【職等職稱】/【工作性質】/【職位】"
                                bolCheckData = False
                            End If
                        Else
                            If Trim(myDataRead(nintWTID).ToString) <> "" Then
                                strSqlWhere = ""
                                strSqlWhere = strSqlWhere & " and CompID =" & Bsp.Utility.Quote(Trim(myDataRead(nintCompIDNew).ToString))
                                strSqlWhere = strSqlWhere & " and WTID =" & Bsp.Utility.Quote(Trim(myDataRead(nintWTID).ToString))
                                If Not objHR.IsDataExists("WorkTime", strSqlWhere) Then
                                    strErrMsg = strErrMsg & "==> " & Trim(myDataRead(nintWTID).ToString) & " 查無班別代碼!"
                                    bolCheckData = False
                                End If
                            End If
                        End If

                        '備註
                        If Trim(myDataRead(nintRemark).ToString) <> "" And Len(Trim(myDataRead(nintRemark).ToString)) > 100 Then
                            strErrMsg = strErrMsg & "==> " & Trim(myDataRead(nintRemark).ToString) & " 備註，最多可輸入50個中文字或100個英數字!"
                            bolCheckData = False
                        End If

                        '主管任用方式
                        If strReason = "40" Then '20160824 Beatrice Add
                            If Trim(myDataRead(nintBossType).ToString) <> "" Then
                                strErrMsg = strErrMsg & "==> 主管任用方式：" & Trim(myDataRead(nintBossType).ToString) & " 異動原因為40升等，僅處理【職等職稱】/【工作性質】/【職位】"
                                bolCheckData = False
                            End If
                        Else
                            If Trim(myDataRead(nintBossType).ToString) <> "" And Trim(myDataRead(nintBossType).ToString) <> "1" And Trim(myDataRead(nintBossType).ToString) <> "2" Then
                                strErrMsg = strErrMsg & "==> " & Trim(myDataRead(nintBossType).ToString) & " 主管任用方式(1:主要，2:兼任)資料錯誤!"
                                bolCheckData = False
                            End If
                        End If

                        '副主管
                        If strReason = "40" Then '20160824 Beatrice Add
                            If Trim(myDataRead(nintIsSecBoss).ToString) <> "" Then
                                strErrMsg = strErrMsg & "==> 副主管註記：" & Trim(myDataRead(nintIsSecBoss).ToString) & " 異動原因為40升等，僅處理【職等職稱】/【工作性質】/【職位】"
                                bolCheckData = False
                            End If
                        Else
                            If Trim(myDataRead(nintIsSecBoss).ToString) <> "" And Trim(myDataRead(nintIsSecBoss).ToString) <> "0" And Trim(myDataRead(nintIsSecBoss).ToString) <> "1" Then
                                strErrMsg = strErrMsg & "==> " & Trim(myDataRead(nintIsSecBoss).ToString) & " 副主管註記(0:否，1:是)資料錯誤!"
                                bolCheckData = False
                            End If
                        End If

                        '簽核單位副主管
                        If strReason = "40" Then '20160824 Beatrice Add
                            If Trim(myDataRead(nintIsSecGroupBoss).ToString) <> "" Then
                                strErrMsg = strErrMsg & "==> 簽核單位副主管註記：" & Trim(myDataRead(nintIsSecGroupBoss).ToString) & " 異動原因為40升等，僅處理【職等職稱】/【工作性質】/【職位】"
                                bolCheckData = False
                            End If
                        Else
                            If Trim(myDataRead(nintIsSecGroupBoss).ToString) <> "" And Trim(myDataRead(nintIsSecGroupBoss).ToString) <> "0" And Trim(myDataRead(nintIsSecGroupBoss).ToString) <> "1" Then
                                strErrMsg = strErrMsg & "==> " & Trim(myDataRead(nintIsGroupBoss).ToString) & " 簽核單位副主管註記(0:否，1:是)資料錯誤!"
                                bolCheckData = False
                            End If
                        End If

                        '主管
                        If strReason <> "40" Then '20160824 Beatrice Add
                            If Trim(myDataRead(nintBossType).ToString) <> "" And (Trim(myDataRead(nintBossType).ToString) = "1" Or Trim(myDataRead(nintBossType).ToString) = "2") Then
                                If Trim(myDataRead(nintIsBoss).ToString) <> "1" And Trim(myDataRead(nintIsGroupBoss).ToString) <> "1" Then  '20150601 wei modify Trim(myDataRead(nintIsSecBoss).ToString) Trim(myDataRead(nintIsSecGroupBoss).ToString)
                                    strErrMsg = strErrMsg & "==> " & Trim(myDataRead(nintIsGroupBoss).ToString) & " 已上傳主管任用方式，請至少上傳一個單位主管或簽核單位主管註記!"
                                    bolCheckData = False
                                End If
                                If Trim(myDataRead(nintBossType).ToString) = "1" Then
                                    strSqlWhere = " And CompID = " & Bsp.Utility.Quote(Trim(myDataRead(nintCompID).ToString)) & " And EmpID = " & Bsp.Utility.Quote(Trim(myDataRead(nintEmpID).ToString))
                                    strSqlWhere &= " And ValidMark='0' And BossType = '1'"
                                    If objHR.IsDataExists("EmployeeWait", strSqlWhere) Then
                                        strErrMsg = strErrMsg & "==> " & Trim(myDataRead(nintBossType).ToString) & " 待生效資料已有主要主管資料存在!"
                                        bolCheckData = False
                                    End If
                                    strSqlWhere = " And CompID = " & Bsp.Utility.Quote(Trim(myDataRead(nintCompID).ToString)) & " And EmpID = " & Bsp.Utility.Quote(Trim(myDataRead(nintEmpID).ToString))
                                    strSqlWhere &= " And ValidMark='0' And BossType = '1'"
                                    If objHR.IsDataExists("Tmp_EmpAdditionWait", strSqlWhere) Then
                                        strErrMsg = strErrMsg & "==> " & Trim(myDataRead(nintBossType).ToString) & " 待生效資料已有主要主管資料存在!"
                                        bolCheckData = False
                                    End If
                                    strSqlWhere = " And CompID = " & Bsp.Utility.Quote(Trim(myDataRead(nintCompID).ToString)) & " And EmpID = " & Bsp.Utility.Quote(Trim(myDataRead(nintEmpID).ToString))
                                    strSqlWhere &= " And ValidMark='0' And BossType = '1'"
                                    If objHR.IsDataExists("EmpAdditionWait", strSqlWhere) Then
                                        strErrMsg = strErrMsg & "==> " & Trim(myDataRead(nintBossType).ToString) & " 待生效資料已有主要主管資料存在!"
                                        bolCheckData = False
                                    End If
                                End If
                                '檢查是否有同單位的主管註記
                                If Trim(myDataRead(nintIsBoss).ToString) = "1" Then
                                    If Trim(myDataRead(nintCompID).ToString) = Trim(myDataRead(nintCompIDNew).ToString) Then
                                        strSqlWhere = " And CompID = " & Bsp.Utility.Quote(Trim(myDataRead(nintCompIDNew).ToString)) & " And OrganID = " & Bsp.Utility.Quote(Trim(myDataRead(nintOrganID).ToString)) & " And EmpID<>" & Bsp.Utility.Quote(Trim(myDataRead(nintEmpID).ToString))
                                        strSqlWhere &= " And ValidMark='0' And IsBoss='1' "
                                        strSqlWhere &= " And ValidDate = " & Bsp.Utility.Quote(FormatDateTime(strValidDate, DateFormat.ShortDate))   '20160104 wei add 增加生效日判斷
                                        strSqlWhere &= " And Reason in (Select Reason From WorkStatus_EmployeeReason where AfterWorkStatusType in ('1')) " '20160104 wei add 增加判斷異動後需為在職的原因
                                        If objHR.IsDataExists("EmployeeWait", strSqlWhere) Then
                                            strErrMsg = strErrMsg & "==> " & Trim(myDataRead(nintOrganID).ToString) & " 待生效資料已有相同單位的單位主管註記!"
                                            bolCheckData = False
                                        End If
                                        strSqlWhere = " And AddCompID = " & Bsp.Utility.Quote(Trim(myDataRead(nintCompIDNew).ToString)) & " And AddOrganID = " & Bsp.Utility.Quote(Trim(myDataRead(nintOrganID).ToString)) & " And EmpID<>" & Bsp.Utility.Quote(Trim(myDataRead(nintEmpID).ToString))
                                        strSqlWhere &= " And ValidMark='0' And IsBoss='1' "
                                        strSqlWhere &= " And ValidDate = " & Bsp.Utility.Quote(FormatDateTime(strValidDate, DateFormat.ShortDate))   '20160104 wei add 增加生效日判斷
                                        strSqlWhere &= " And Reason in ('1') " '20160104 wei add 增加判斷異動後為調兼的原因
                                        If objHR.IsDataExists("EmpAdditionWait", strSqlWhere) Then
                                            strErrMsg = strErrMsg & "==> " & Trim(myDataRead(nintOrganID).ToString) & " 待生效資料已有相同單位的單位主管註記!"
                                            bolCheckData = False
                                        End If
                                    End If
                                Else
                                    '20150601 wei add 檢核若異動後工作性質為BA0001單位主管，區督導，分行經理時，必須要勾選主管註記&簽核單位主管註記
                                    If bolBoss Then
                                        strErrMsg = strErrMsg & "==>  異動後工作性質為單位主管，區督導，分行經理時，必須要勾選主管註記&簽核單位主管註記!"
                                        bolCheckData = False
                                    End If
                                End If
                                If Trim(myDataRead(nintIsGroupBoss).ToString) = "1" Then
                                    If Trim(myDataRead(nintCompID).ToString) = Trim(myDataRead(nintCompIDNew).ToString) Then
                                        strSqlWhere = " And CompID = " & Bsp.Utility.Quote(Trim(myDataRead(nintCompIDNew).ToString)) & " And FlowOrganID = " & Bsp.Utility.Quote(Trim(myDataRead(nintFlowOrganID).ToString)) & " And EmpID<>" & Bsp.Utility.Quote(Trim(myDataRead(nintEmpID).ToString))
                                        strSqlWhere &= " And ValidMark='0' And IsGroupBoss='1' "
                                        strSqlWhere &= " And ValidDate = " & Bsp.Utility.Quote(FormatDateTime(strValidDate, DateFormat.ShortDate))   '20160104 wei add 增加生效日判斷
                                        strSqlWhere &= " And Reason in (Select Reason From WorkStatus_EmployeeReason where AfterWorkStatusType in ('1')) " '20160104 wei add 增加判斷異動後需為在職的原因
                                        If objHR.IsDataExists("EmployeeWait", strSqlWhere) Then
                                            strErrMsg = strErrMsg & "==> " & Trim(myDataRead(nintFlowOrganID).ToString) & " 待生效資料已有相同最小簽核單位的單位主管註記!"
                                            bolCheckData = False
                                        End If
                                        strSqlWhere = " And AddCompID = " & Bsp.Utility.Quote(Trim(myDataRead(nintCompIDNew).ToString)) & " And AddFlowOrganID = " & Bsp.Utility.Quote(Trim(myDataRead(nintFlowOrganID).ToString)) & " And EmpID<>" & Bsp.Utility.Quote(Trim(myDataRead(nintEmpID).ToString))
                                        strSqlWhere &= " And ValidMark='0' And IsGroupBoss='1' "
                                        strSqlWhere &= " And ValidDate = " & Bsp.Utility.Quote(FormatDateTime(strValidDate, DateFormat.ShortDate))   '20160104 wei add 增加生效日判斷
                                        strSqlWhere &= " And Reason in ('1') " '20160104 wei add 增加判斷異動後為調兼的原因
                                        If objHR.IsDataExists("EmpAdditionWait", strSqlWhere) Then
                                            strErrMsg = strErrMsg & "==> " & Trim(myDataRead(nintFlowOrganID).ToString) & " 待生效資料已有相同最小簽核單位的單位主管註記!"
                                            bolCheckData = False
                                        End If
                                    End If
                                Else
                                    '20150601 wei add 檢核若異動後工作性質為BA0001單位主管，區督導，分行經理時，必須要勾選主管註記&簽核單位主管註記
                                    If bolBoss Then
                                        strErrMsg = strErrMsg & "==>  異動後工作性質為單位主管，區督導，分行經理時，必須要勾選主管註記&簽核單位主管註記!"
                                        bolCheckData = False
                                    End If
                                End If

                            Else
                                If Trim(myDataRead(nintIsBoss).ToString) <> "" Or Trim(myDataRead(nintIsGroupBoss).ToString) <> "" Then '20150601 wei modify Or chkIsSecBoss.Checked Or chkIsSecGroupBoss.Checked
                                    strErrMsg = strErrMsg & "==> " & Trim(myDataRead(nintBossType).ToString) & " 已選擇單位主管或簽核單位主管，請選擇主管任用方式!"
                                    bolCheckData = False
                                Else
                                    '20150601 wei add 檢核若異動後工作性質為BA0001單位主管，區督導，分行經理時，必須要勾選主管註記&簽核單位主管註記
                                    If bolBoss Then
                                        strErrMsg = strErrMsg & "==>  異動後工作性質為單位主管，區督導，分行經理時，必須要勾選主管註記&簽核單位主管註記!"
                                        bolCheckData = False
                                    End If
                                End If
                            End If
                        End If


                        '應試者編號
                        If strReason = "40" Then '20160824 Beatrice Add
                            If Trim(myDataRead(nintRecID).ToString) <> "" Then
                                strErrMsg = strErrMsg & "==> 應試者編號：" & Trim(myDataRead(nintRecID).ToString) & " 異動原因為40升等，僅處理【職等職稱】/【工作性質】/【職位】"
                                bolCheckData = False
                            End If
                            If Trim(myDataRead(nintContractDate).ToString) <> "" Then
                                strErrMsg = strErrMsg & "==> 預計報到日：" & Trim(myDataRead(nintContractDate).ToString) & " 異動原因為40升等，僅處理【職等職稱】/【工作性質】/【職位】"
                                bolCheckData = False
                            End If
                        Else
                            If Trim(myDataRead(nintRecID).ToString) <> "" And Trim(myDataRead(nintContractDate).ToString) <> "" And IsDate(Trim(myDataRead(nintContractDate).ToString)) Then
                                If Not objHR3000.CheckRecID(Trim(myDataRead(nintCompIDNew).ToString), Trim(myDataRead(nintRecID).ToString), Trim(myDataRead(nintContractDate).ToString)) Then
                                    strErrMsg = strErrMsg & "==> " & Trim(myDataRead(nintRecID).ToString) & "/" & Trim(myDataRead(nintContractDate).ToString) & " 查無應試者編號及預計報到日!"
                                    bolCheckData = False
                                End If
                            Else
                                '預計報到日
                                If Trim(myDataRead(nintContractDate).ToString) <> "" And Not IsDate(Trim(myDataRead(nintContractDate).ToString)) Then
                                    strErrMsg = strErrMsg & "==> " & Trim(myDataRead(nintContractDate).ToString) & " 預計報到日格式錯誤！"
                                    bolCheckData = False
                                End If
                                If Trim(myDataRead(nintContractDate).ToString) <> "" Then
                                    strErrMsg = strErrMsg & "==> " & Trim(myDataRead(nintRecID).ToString) & "/" & Trim(myDataRead(nintContractDate).ToString) & " 有應試者編號無預計報到日!"
                                    bolCheckData = False
                                End If
                            End If
                        End If

                        If Trim(myDataRead(nintCompID).ToString) <> "" And Trim(myDataRead(nintEmpID).ToString) <> "" And strValidDate <> "" And Trim(myDataRead(nintReason).ToString) <> "" Then

                            '跨公司異動檢核
                            '生效後為【在職】的異動原因(02,03,05,06,07,50)的異動原因，都先檢查【人員主檔】在【異動後公司以外】的公司資料是否在職
                            '若在職，再檢查檢查【待異動檔】在【異動後公司以外】的公司資料是否有異動後【非在職】(12,14,18,11,15,16,17,13,50)的待異動
                            '若有，才能建檔，否則不能建檔(HR2300到職建檔亦同)
                            If strReason = "02" Or strReason = "03" Or strReason = "05" Or strReason = "06" Or strReason = "07" Or (strReason = "50" And Trim(myDataRead(nintCompID).ToString) = Trim(myDataRead(nintCompIDNew).ToString)) Then
                                If objHR3000.funCheckWorkStatus(strValidDate, Trim(myDataRead(nintCompID).ToString), Trim(myDataRead(nintCompIDNew).ToString), Trim(myDataRead(nintEmpID).ToString), strIDNo, Trim(myDataRead(nintReason).ToString)) <> "" Then
                                    strErrMsg = strErrMsg & "==> 該員尚在其它公司在職，且未有離職相關待異動，請先確認並通知該公司經辦再行鍵入待異動！"
                                    bolCheckData = False
                                End If
                            End If

                            '檢核是否有相同生效日期及異動原因的紀錄存在 
                            strSqlWhere = ""
                            strSqlWhere = strSqlWhere & " and CompID=" & Bsp.Utility.Quote(Trim(myDataRead(nintCompID).ToString))
                            strSqlWhere = strSqlWhere & " and EmpID=" & Bsp.Utility.Quote(Trim(myDataRead(nintEmpID).ToString))
                            strSqlWhere = strSqlWhere & " and ValidDate=" & Bsp.Utility.Quote(FormatDateTime(strValidDate, DateFormat.ShortDate))
                            strSqlWhere = strSqlWhere & " and Reason=" & Bsp.Utility.Quote(Trim(myDataRead(nintReason).ToString))
                            If objHR.IsDataExists("EmployeeWait", strSqlWhere) Then
                                strErrMsg = strErrMsg & "==> 資料重複：待異動紀錄已有相同生效日期及異動原因的資料存在！"
                                bolCheckData = False
                            End If

                            strSqlWhere = " And IDNo = " & Bsp.Utility.Quote(strIDNo)
                            strSqlWhere = strSqlWhere & " And ModifyDate = " & Bsp.Utility.Quote(FormatDateTime(strValidDate, DateFormat.ShortDate)) & " And Reason = " & Bsp.Utility.Quote(Trim(myDataRead(nintReason).ToString))
                            '避免批次sHR3000執行時，發生失敗，故在前端輸入時檢核是否重複
                            If objHR.IsDataExists("EmployeeLog", strSqlWhere) Then
                                strErrMsg = strErrMsg & "==> 資料重複：企業團經歷紀錄已有相同生效日期及異動原因的資料存在！"
                                bolCheckData = False
                            End If

                            If strReason <> "40" Then
                                '20150602 wei add 檢核若工作性質是分行作業主管BO0001，但未勾選簽核主管註記，就跳出提醒確認是否勾選簽核單位主管註記，若確認新增，則儲存
                                If bolOPBoss And Trim(myDataRead(nintIsBoss).ToString) <> "1" Then
                                    strErrMsg = strErrMsg & "==> 警告:工作性質是分行作業主管，但未勾選簽核主管註記！"
                                End If

                                '20150602 wei add 異動前A單位為A主管(主管or簽核主管)，待異動輸入A單位換B主管，跳出提醒視窗確認【A單位將更換為B主管，請確認是否繼續儲存?】，按鈕有【確認，取消】，若確認，則繼續儲存
                                '主管
                                If Trim(myDataRead(nintIsBoss).ToString) <> "" Then
                                    strSqlWhere = " And CompID = " & Bsp.Utility.Quote(Trim(myDataRead(nintCompIDNew).ToString)) & " And OrganID = " & Bsp.Utility.Quote(Trim(myDataRead(nintOrganID).ToString))
                                    strSqlWhere &= " And BossCompID = " & Bsp.Utility.Quote(Trim(myDataRead(nintCompID).ToString)) & " And Boss = " & Bsp.Utility.Quote(Trim(myDataRead(nintEmpID).ToString))
                                    If Not objHR.IsDataExists("Organization", strSqlWhere) Then
                                        If Trim(myDataRead(nintCompIDNew).ToString) = Trim(myDataRead(nintCompID).ToString) Then    '20160104 wei add 增加判斷為同公司異動才顯示
                                            strErrMsg = strErrMsg & "==> 警告:" & Trim(myDataRead(nintOrganID).ToString) & "主管將更換為" & Trim(myDataRead(nintEmpID).ToString) & "！"
                                        End If
                                    End If
                                End If
                                '簽核主管
                                If Trim(myDataRead(nintIsGroupBoss).ToString) <> "" Then
                                    strSqlWhere = " And OrganID = " & Bsp.Utility.Quote(Trim(myDataRead(nintFlowOrganID).ToString))
                                    strSqlWhere &= " And BossCompID = " & Bsp.Utility.Quote(Trim(myDataRead(nintCompID).ToString)) & " And Boss = " & Bsp.Utility.Quote(Trim(myDataRead(nintEmpID).ToString))
                                    If Not objHR.IsDataExists("OrganizationFlow", strSqlWhere) Then
                                        If Trim(myDataRead(nintCompIDNew).ToString) = Trim(myDataRead(nintCompID).ToString) Then    '20160104 wei add 增加判斷為同公司異動才顯示
                                            strErrMsg = strErrMsg & "==> 警告:" & Trim(myDataRead(nintFlowOrganID).ToString) & "主管將更換為" & Trim(myDataRead(nintEmpID).ToString) & "！"
                                        End If
                                    End If
                                End If
                            End If
                        End If

                        If bolCheckData Then
                            If funUploadDataSingle(myDataRead) Then
                                If strErrMsg = "" Then
                                    intSuccessCount = intSuccessCount + 1
                                Else
                                    intWarningCount = intWarningCount + 1
                                End If
                            Else
                                intErrorCount = intErrorCount + 1
                            End If
                        Else
                            intErrorCount = intErrorCount + 1
                        End If
                    End If

                        If strErrMsg <> "" Then
                            subWriteLog("資料列:" & intTotalCount + 1 & " 公司代碼：" & Trim(myDataRead(nintCompID).ToString) & " 員工編號：" & Trim(myDataRead(nintEmpID).ToString) & " 生效日期：" & strValidDate & " 異動原因：" & Trim(myDataRead(nintReason).ToString) & strErrMsg)
                            bolInputStatus = False
                        End If

                        intTotalCount = intTotalCount + 1
                End While
            End Using

            myExcelConn.Close()

            If bolInputStatus = False Then
                Return False
            Else
                Return True
            End If

        Catch ex As Exception
            myExcelConn.Close()
            Bsp.Utility.ShowMessage(Me, Me.FunID & ".funCheckData1", ex)
            Return False
        End Try

    End Function
    '資料上傳 單筆 '20150609 wei mdoify HR3000-EmployeeWait
    Private Function funUploadDataSingle(ByVal myDataRead As OleDbDataReader) As Boolean
        Dim objHR As New HR
        Dim objHR3000 As New HR3000
        Dim bsEmployeeWait As New beEmployeeWait.Service()
        Dim beEmployeeWait As New beEmployeeWait.Row
        Dim dtNow = DateTime.Now

        '畫面輸入的條件

        Using cn As DbConnection = Bsp.DB.getConnection("eHRMSDB")
            cn.Open()

            Dim tran As DbTransaction = cn.BeginTransaction
            Dim inTrans As Boolean = True

            '"HR3000上傳作業 _分隔符號『  |  』)"			
            'NO.	名稱	        格式    長度    必填	說明
            '1	公司代碼	        字串    6       Y
            '2	員工編號	        字串    6       Y	
            '3	生效日期	        日期    10      Y   YYYY/MM/DD
            '4	異動原因代碼	    字串    6       Y        	
            '5	異動後公司代碼	字串    6        	
            '6	異動後部門代碼	字串    12        	
            '7	異動後科組課代碼 	字串    12        	
            '8	職等代碼       	字串    2       Y        	
            '9	職稱代碼        	字串    1       Y 	
            '10	職位代碼        	字串    50        	以"|"符號隔開,1:主要|0:兼任,只有一筆主要職位 ex:1|03002,有一筆主要職位&多筆兼任職位 EX:1|030002|0|03003|0|030004
            '11	工作性質代碼    	字串    50        	以"|"符號隔開,1:主要|0:兼任,只有一筆主要工作性質 ex:1|03002,有一筆主要工作性質&多筆兼任工作性質 EX:1|030002|0|03003|0|030004
            '12	主管註記        	字串    1        	0:否，1:是
            '13	簽核單位主管註記	字串    1   	        0:否，1:是
            '14	最小簽核單位代碼 字串    8        	
            '15	工作地點代碼  	字串    3        	
            '16	班別代碼        	字串    2        	
            '17	備註          	字串    100         最多可輸入50個中文字或100個英數字    	
            '18	主管任用方式  	字串    1        	1 主要 2 兼任
            '19	副主管         	字串    1        	0:否，1:是
            '20	簽核單位副主管  	字串    1        	0:否，1:是
            '21 應試者編號       字串    14          異動(復職)員工需與招募系統待報到應試者資料勾稽用       
            '22 預計報到日       日期    10          YYYY/MM/DD

            '補充說明：欄位非必填且放空白	
            Dim nintCompID As Integer = 0
            Dim nintEmpID As Integer = 1
            Dim nintValidDate As Integer = 2
            Dim nintReason As Integer = 3
            Dim nintCompIDNew As Integer = 4
            Dim nintDeptID As Integer = 5
            Dim nintOrganID As Integer = 6
            Dim nintRankID As Integer = 7
            Dim nintTitleID As Integer = 8
            Dim nintPositionID As Integer = 9
            Dim nintWorkTypeID As Integer = 10
            Dim nintIsBoss As Integer = 11
            Dim nintIsGroupBoss As Integer = 12
            Dim nintFlowOrganID As Integer = 13
            Dim nintEmpFlowRemarkID As Integer = 14
            Dim nintWorkSiteID As Integer = 15
            Dim nintWTID As Integer = 16
            Dim nintRemark As Integer = 17
            Dim nintBossType As Integer = 18
            Dim nintIsSecBoss As Integer = 19
            Dim nintIsSecGroupBoss As Integer = 20
            Dim nintRecID As Integer = 21
            Dim nintContractDate As Integer = 22



            Dim intLoop As Integer = 0

            Try

                '公司代碼
                beEmployeeWait.CompID.Value = myDataRead(nintCompID).ToString
                '員工編號
                beEmployeeWait.EmpID.Value = myDataRead(nintEmpID).ToString
                '生效日期
                beEmployeeWait.ValidDate.Value = myDataRead(nintValidDate).ToString
                beEmployeeWait.Seq.Value = objHR3000.GetEmployeeWaitSeq(beEmployeeWait.CompID.Value, beEmployeeWait.EmpID.Value, beEmployeeWait.ValidDate.Value.ToString("yyyy/MM/dd")) '20150529 wei modify 修正無法取得Seq
                '異動原因代碼
                beEmployeeWait.Reason.Value = myDataRead(nintReason).ToString

                '20160825 Add
                If myDataRead(nintReason).ToString = "40" Then
                    Using dt As DataTable = objHR3000.GetEmpDataByHR3000(Trim(myDataRead(nintCompID).ToString), Trim(myDataRead(nintEmpID).ToString))
                        If dt.Rows.Count > 0 Then
                            beEmployeeWait.NewCompID.Value = dt.Rows(0).Item("CompID").ToString
                            beEmployeeWait.DeptID.Value = dt.Rows(0).Item("DeptID").ToString
                            beEmployeeWait.OrganID.Value = dt.Rows(0).Item("OrganID").ToString
                            beEmployeeWait.FlowOrganID.Value = dt.Rows(0).Item("FlowOrganID").ToString
                            beEmployeeWait.BusinessType.Value = dt.Rows(0).Item("BusinessType").ToString
                            beEmployeeWait.EmpFlowRemarkID.Value = dt.Rows(0).Item("EmpFlowRemarkID").ToString
                            beEmployeeWait.WorkSiteID.Value = dt.Rows(0).Item("WorkSiteID").ToString
                            beEmployeeWait.WTID.Value = dt.Rows(0).Item("WTID").ToString
                            beEmployeeWait.BossType.Value = ""
                            beEmployeeWait.IsBoss.Value = "0"
                            beEmployeeWait.IsSecBoss.Value = "0"
                            beEmployeeWait.IsGroupBoss.Value = "0"
                            beEmployeeWait.IsSecGroupBoss.Value = "0"
                        End If
                    End Using
                Else
                    '異動後公司代碼
                    beEmployeeWait.NewCompID.Value = myDataRead(nintCompIDNew).ToString
                    '異動後部門代碼
                    beEmployeeWait.DeptID.Value = myDataRead(nintDeptID).ToString
                    '異動後科組課代碼
                    beEmployeeWait.OrganID.Value = myDataRead(nintOrganID).ToString
                    '最小簽核單位代碼
                    beEmployeeWait.FlowOrganID.Value = myDataRead(nintFlowOrganID).ToString
                    '業務代碼 20161104 Beatrice Add
                    beEmployeeWait.BusinessType.Value = objHR3000.GetBusinessType(myDataRead(nintCompIDNew).ToString, myDataRead(nintFlowOrganID).ToString)
                    '功能備註代碼 20161104 Beatrice Add
                    beEmployeeWait.EmpFlowRemarkID.Value = myDataRead(nintEmpFlowRemarkID).ToString
                    '工作地點代碼
                    beEmployeeWait.WorkSiteID.Value = myDataRead(nintWorkSiteID).ToString
                    '班別代碼
                    beEmployeeWait.WTID.Value = myDataRead(nintWTID).ToString
                    '主管任用方式
                    beEmployeeWait.BossType.Value = myDataRead(nintBossType).ToString
                    '主管註記
                    beEmployeeWait.IsBoss.Value = myDataRead(nintIsBoss).ToString
                    '簽核單位主管註記
                    beEmployeeWait.IsGroupBoss.Value = myDataRead(nintIsGroupBoss).ToString
                    '副主管
                    beEmployeeWait.IsSecBoss.Value = myDataRead(nintIsSecBoss).ToString
                    '簽核單位副主管
                    beEmployeeWait.IsSecGroupBoss.Value = myDataRead(nintIsSecGroupBoss).ToString
                End If

                If beEmployeeWait.OrganID.Value = "" Then
                    beEmployeeWait.GroupID.Value = beEmployeeWait.DeptID.Value
                Else
                    '2016/05/03 SPHBKC資料已併入OrganizationFlow中
                    'If myDataRead(nintCompIDNew).ToString = "SPHBKC" Then
                    '    beEmployeeWait.GroupID.Value = objHR3000.Get_CGroupID(beEmployeeWait.OrganID.Value)
                    'Else
                    beEmployeeWait.GroupID.Value = objHR3000.Get_GroupID(beEmployeeWait.OrganID.Value)
                    'End If
                End If
                '職等代碼
                beEmployeeWait.RankID.Value = myDataRead(nintRankID).ToString
                '職稱代碼
                beEmployeeWait.TitleID.Value = myDataRead(nintTitleID).ToString

                '2016/05/03 SPHBKC資料已併入Title中
                'If beEmployeeWait.NewCompID.Value = "SPHBKC" Then
                '    beEmployeeWait.TitleName.Value = objHR.GetCTitleName(beEmployeeWait.NewCompID.Value, beEmployeeWait.RankID.Value, beEmployeeWait.TitleID.Value).Rows(0)("TitleName").ToString
                'Else
                beEmployeeWait.TitleName.Value = objHR.GetTitleName(beEmployeeWait.NewCompID.Value, beEmployeeWait.RankID.Value, beEmployeeWait.TitleID.Value).Rows(0)("TitleName").ToString
                'End If

                '職位代碼
                beEmployeeWait.PositionID.Value = myDataRead(nintPositionID).ToString
                '工作性質代碼
                beEmployeeWait.WorkTypeID.Value = myDataRead(nintWorkTypeID).ToString
                '備註
                beEmployeeWait.Remark.Value = myDataRead(nintRemark).ToString
                '應試者編號
                beEmployeeWait.RecID.Value = myDataRead(nintRecID).ToString
                '預計報到日
                If Trim(myDataRead(nintContractDate).ToString) <> "" Then
                    beEmployeeWait.ContractDate.Value = Convert.ToDateTime(myDataRead(nintContractDate).ToString)
                Else
                    beEmployeeWait.ContractDate.Value = Convert.ToDateTime("1900/01/01")
                End If

                beEmployeeWait.ValidMark.Value = "0"

                beEmployeeWait.LastChgComp.Value = UserProfile.ActCompID
                beEmployeeWait.LastChgID.Value = UserProfile.ActUserID
                beEmployeeWait.LastChgDate.Value = Now

                bsEmployeeWait.Insert(beEmployeeWait, tran)

                '20150604 wei add 增加上傳寫入EmpAdditionWait
                EmpAddition_CopyTo_EmpAdditionWait(beEmployeeWait.CompID.Value, beEmployeeWait.EmpID.Value, beEmployeeWait.ValidDate.Value.ToString("yyyy/MM/dd"), beEmployeeWait.Seq.Value, tran)

                tran.Commit()
                inTrans = False
                Return True

            Catch ex As Exception

                If inTrans Then tran.Rollback()
                Return False

                Bsp.Utility.ShowMessage(Me, Me.FunID & ".funUploadDataSingle", ex)
            End Try
        End Using
    End Function
    '20150609 wei add HR3000-EmpAdditionWait
    Private Function funCheckData_ByEmpAdditionWait(ByVal FileName As String) As Boolean

        Dim objHR As New HR
        Dim objHR3000 As New HR3000
        Dim bolInputStatus As Boolean = True
        Dim strErrMsg As String = ""
        Dim strSqlWhere As String = ""
        Dim strValidDate As String = ""

        Dim intLoop As Integer = 0

        Dim bolCheckData As Boolean = False  '檢核資料

        '畫面輸入的條件

        '"HR3000 EmpAdditionWait 上傳作業 _分隔符號『  |  』)"			
        'NO.	名稱          格式    長度    必填	說明
        '1	公司代碼          字串    6       Y
        '2	員工編號          字串    6       Y	
        '3	生效日期          日期    10      Y   YYYY/MM/DD
        '4	人令              字串    50      Y        	
        '5	兼任狀態          字串    1       Y       	
        '6	兼任公司          字串    6       Y 	
        '7	兼任部門          字串    12      Y  	
        '8	兼任科組課        字串    12      Y        	
        '9	兼任最小簽核單位   字串    12       	
        '10	主管任用方式       字串    1      1 主要 2 兼任
        '11	主管註記        	  字串    1      0:否，1:是
        '12	簽核單位主管註記	  字串    1   	 0:否，1:是
        '13	副主管         	  字串    1      0:否，1:是
        '14	簽核單位副主管  	  字串    1      0:否，1:是
        '15 備註              字串    500       


        '補充說明：欄位非必填且放空白	
        Dim nintCompID As Integer = 0
        Dim nintEmpID As Integer = 1
        Dim nintValidDate As Integer = 2
        Dim nintFileNo As Integer = 3
        Dim nintReason As Integer = 4
        Dim nintAddCompID As Integer = 5
        Dim nintAddDeptID As Integer = 6
        Dim nintAddOrganID As Integer = 7
        Dim nintAddFlowOrganID As Integer = 8
        Dim nintBossType As Integer = 9
        Dim nintIsBoss As Integer = 10
        Dim nintIsGroupBoss As Integer = 11
        Dim nintIsSecBoss As Integer = 12
        Dim nintIsSecGroupBoss As Integer = 13
        Dim nintRemark As Integer = 14

        intTotalCount = 0
        intSuccessCount = 0
        intErrorCount = 0
        intWarningCount = 0

        Dim strExcelConn As String = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & FileName & ";Extended Properties='EXCEL 8.0;HDR=Yes;IMEX=1;'"    '20150401 wei modify

        Dim strExcelWorkSheet As String = "[HR3000_EmpAddition$]"
        Dim strExcelSelect As String = "SELECT * FROM " & strExcelWorkSheet

        Dim myExcelConn As OleDbConnection = Nothing
        myExcelConn = New OleDbConnection(strExcelConn)
        Dim myExcelCommand As OleDbCommand = New OleDbCommand(strExcelSelect, myExcelConn)
        Try
            myExcelConn.Open()
            Dim intSeq As Integer   'EmployeeWait Seq
            Dim intAddSeq As Integer    'EmpAdditionWait Seq
            Using myDataRead As OleDbDataReader = myExcelCommand.ExecuteReader
                While myDataRead.Read

                    '檢核上傳資料
                    strErrMsg = ""
                    bolCheckData = True
                    intSeq = 0
                    intAddSeq = 0
                    strValidDate = ""
                    '先判斷是否上傳欄位個數正確
                    If myDataRead.FieldCount <> 15 Then
                        strErrMsg = strErrMsg & "==> 上傳資料欄位個數不正確！"
                        intErrorCount = intErrorCount + 1
                    Else
                        '公司代碼
                        strSqlWhere = ""
                        strSqlWhere = strSqlWhere & " and CompID =" & Bsp.Utility.Quote(Trim(myDataRead(nintCompID).ToString))
                        If Not objHR.IsDataExists("Company", strSqlWhere) Then
                            strErrMsg = strErrMsg & "==> " & Trim(myDataRead(nintCompID).ToString) & " 查無公司代碼!"
                            bolCheckData = False
                        End If
                        '員工編號
                        strSqlWhere = ""
                        strSqlWhere = strSqlWhere & " and CompID =" & Bsp.Utility.Quote(Trim(myDataRead(nintCompID).ToString))
                        strSqlWhere = strSqlWhere & " and EmpID =" & Bsp.Utility.Quote(Trim(myDataRead(nintEmpID).ToString))
                        If Not objHR.IsDataExists("Personal", strSqlWhere) Then
                            strErrMsg = strErrMsg & "==> " & Trim(myDataRead(nintEmpID).ToString) & " 員工主檔不存在!"
                            bolCheckData = False
                        End If

                        '生效日期
                        strValidDate = Trim(myDataRead(nintValidDate).ToString)
                        If Trim(myDataRead(nintValidDate).ToString) = "" Then
                            strErrMsg = strErrMsg & "==> 生效日期不能為空白！"
                            bolCheckData = False
                            strValidDate = ""
                        Else
                            If Not IsDate(Trim(myDataRead(nintValidDate).ToString)) Then
                                strErrMsg = strErrMsg & "==> " & Trim(myDataRead(nintValidDate).ToString) & " 生效日期格式錯誤！"
                                bolCheckData = False
                                strValidDate = ""
                            End If
                        End If

                        If Trim(myDataRead(nintCompID).ToString) <> "" And Trim(myDataRead(nintEmpID).ToString) <> "" And Trim(myDataRead(nintValidDate).ToString) <> "" Then
                            Using dt As DataTable = objHR3000.GetEmployeeWaitData(Trim(myDataRead(nintCompID).ToString), Trim(myDataRead(nintEmpID).ToString), Trim(myDataRead(nintValidDate).ToString))
                                If dt.Rows.Count > 0 Then
                                    intSeq = dt.Rows.Item(0)("Seq").ToString()
                                Else
                                    strErrMsg = strErrMsg & "==> 查無待異動主檔！"
                                    bolCheckData = False
                                End If
                            End Using
                        End If

                        '調兼狀態代碼
                        If Trim(myDataRead(nintReason).ToString) <> "1" Then   'And Trim(myDataRead(nintReason).ToString) <> "2" And Trim(myDataRead(nintReason).ToString) <> "3"
                            strErrMsg = strErrMsg & "==> " & Trim(myDataRead(nintReason).ToString) & " 調兼狀態代碼錯誤，待異動調兼上傳只允許調兼!"
                            bolCheckData = False
                        End If

                        '調兼公司代碼
                        strSqlWhere = ""
                        strSqlWhere = strSqlWhere & " and CompID =" & Bsp.Utility.Quote(Trim(myDataRead(nintAddCompID).ToString))
                        If Not objHR.IsDataExists("Company", strSqlWhere) Then
                            strErrMsg = strErrMsg & "==> " & Trim(myDataRead(nintAddCompID).ToString) & " 查無調兼公司代碼!"
                            bolCheckData = False
                        End If

                        '調兼部門代碼
                        strSqlWhere = ""
                        strSqlWhere = strSqlWhere & " and CompID =" & Bsp.Utility.Quote(Trim(myDataRead(nintAddCompID).ToString))
                        strSqlWhere = strSqlWhere & " and OrganID =" & Bsp.Utility.Quote(Trim(myDataRead(nintAddDeptID).ToString))
                        If Not objHR.IsDataExists("Organization", strSqlWhere) Then
                            strErrMsg = strErrMsg & "==> " & Trim(myDataRead(nintAddCompID).ToString) & "/" & Trim(myDataRead(nintAddDeptID).ToString) & " 查無調兼部門代碼!"
                            bolCheckData = False
                        Else
                            If Not objHR.ChkOrganIsVlaid(Trim(myDataRead(nintAddCompID).ToString), Trim(myDataRead(nintAddDeptID).ToString)) Then
                                strErrMsg = strErrMsg & "==> 部門：" & Trim(myDataRead(nintAddCompID).ToString) & "/" & Trim(myDataRead(nintAddDeptID).ToString) & " 上傳的調兼部門是無效單位!"
                                bolCheckData = False
                            End If
                        End If

                        '調兼科組課代碼
                        strSqlWhere = ""
                        strSqlWhere = strSqlWhere & " and CompID =" & Bsp.Utility.Quote(Trim(myDataRead(nintAddCompID).ToString))
                        strSqlWhere = strSqlWhere & " and DeptID =" & Bsp.Utility.Quote(Trim(myDataRead(nintAddDeptID).ToString))
                        strSqlWhere = strSqlWhere & " and OrganID =" & Bsp.Utility.Quote(Trim(myDataRead(nintAddOrganID).ToString))
                        If Not objHR.IsDataExists("Organization", strSqlWhere) Then
                            strErrMsg = strErrMsg & "==> " & Trim(myDataRead(nintAddCompID).ToString) & "/" & Trim(myDataRead(nintAddOrganID).ToString) & " 查無調兼科組課代碼!"
                            bolCheckData = False
                        Else
                            If Not objHR.ChkOrganIsVlaid(Trim(myDataRead(nintAddCompID).ToString), Trim(myDataRead(nintAddOrganID).ToString)) Then
                                strErrMsg = strErrMsg & "==> 科/組/課：" & Trim(myDataRead(nintAddCompID).ToString) & "/" & Trim(myDataRead(nintAddOrganID).ToString) & " 上傳的調兼科/組/課是無效單位!"
                                bolCheckData = False
                            End If
                        End If

                        '調兼最小簽核單位代碼
                        strSqlWhere = ""
                        strSqlWhere = strSqlWhere & " and OrganID =" & Bsp.Utility.Quote(Trim(myDataRead(nintAddFlowOrganID).ToString))
                        If Not objHR.IsDataExists("OrganizationFlow", strSqlWhere) Then
                            strErrMsg = strErrMsg & "==> " & Trim(myDataRead(nintAddFlowOrganID).ToString) & " 查無調兼最小簽核單位代碼!"
                            bolCheckData = False
                        Else
                            If Not objHR.ChkOrganFlowIsVlaid(Trim(myDataRead(nintAddCompID).ToString), Trim(myDataRead(nintAddFlowOrganID).ToString)) Then
                                strErrMsg = strErrMsg & "==> 最小簽核單位：" & Trim(myDataRead(nintAddFlowOrganID).ToString) & " 上傳的調兼最小簽核部門是無效單位!"
                                bolCheckData = False
                            End If
                        End If

                        '主管任用方式
                        If Trim(myDataRead(nintBossType).ToString) <> "" And Trim(myDataRead(nintBossType).ToString) <> "1" And Trim(myDataRead(nintBossType).ToString) <> "2" Then
                            strErrMsg = strErrMsg & "==> " & Trim(myDataRead(nintBossType).ToString) & " 主管任用方式(1:主要，2:兼任)資料錯誤!"
                            bolCheckData = False
                        End If
                        '主管註記
                        If Trim(myDataRead(nintIsBoss).ToString) <> "" And Trim(myDataRead(nintIsBoss).ToString) <> "0" And Trim(myDataRead(nintIsBoss).ToString) <> "1" Then
                            strErrMsg = strErrMsg & "==> " & Trim(myDataRead(nintIsBoss).ToString) & " 主管註記(0:否，1:是)資料錯誤!"
                            bolCheckData = False
                        End If
                        '簽核單位主管註記
                        If Trim(myDataRead(nintIsGroupBoss).ToString) <> "" And Trim(myDataRead(nintIsGroupBoss).ToString) <> "0" And Trim(myDataRead(nintIsGroupBoss).ToString) <> "1" Then
                            strErrMsg = strErrMsg & "==> " & Trim(myDataRead(nintIsGroupBoss).ToString) & " 簽核單位主管註記(0:否，1:是)資料錯誤!"
                            bolCheckData = False
                        End If

                        '副主管
                        If Trim(myDataRead(nintIsSecBoss).ToString) <> "" And Trim(myDataRead(nintIsSecBoss).ToString) <> "0" And Trim(myDataRead(nintIsSecBoss).ToString) <> "1" Then
                            strErrMsg = strErrMsg & "==> " & Trim(myDataRead(nintIsSecBoss).ToString) & " 副主管註記(0:否，1:是)資料錯誤!"
                            bolCheckData = False
                        End If
                        '簽核單位副主管
                        If Trim(myDataRead(nintIsSecGroupBoss).ToString) <> "" And Trim(myDataRead(nintIsSecGroupBoss).ToString) <> "0" And Trim(myDataRead(nintIsSecGroupBoss).ToString) <> "1" Then
                            strErrMsg = strErrMsg & "==> " & Trim(myDataRead(nintIsGroupBoss).ToString) & " 簽核單位副主管註記(0:否，1:是)資料錯誤!"
                            bolCheckData = False
                        End If
                        
                        '主管
                        If Trim(myDataRead(nintBossType).ToString) <> "" And (Trim(myDataRead(nintBossType).ToString) = "1" Or Trim(myDataRead(nintBossType).ToString) = "2") Then
                            If Trim(myDataRead(nintIsBoss).ToString) <> "1" And Trim(myDataRead(nintIsGroupBoss).ToString) <> "1" Then
                                strErrMsg = strErrMsg & "==> " & Trim(myDataRead(nintIsGroupBoss).ToString) & " 已上傳主管任用方式，請至少上傳一個單位主管或簽核單位主管註記!"
                                bolCheckData = False
                            End If
                            If Trim(myDataRead(nintBossType).ToString) = "1" Then
                                strSqlWhere = " And CompID = " & Bsp.Utility.Quote(Trim(myDataRead(nintCompID).ToString)) & " And EmpID = " & Bsp.Utility.Quote(Trim(myDataRead(nintEmpID).ToString))
                                strSqlWhere &= " And ValidMark='0' And BossType = '1'"
                                If objHR.IsDataExists("EmployeeWait", strSqlWhere) Then
                                    strErrMsg = strErrMsg & "==> " & Trim(myDataRead(nintBossType).ToString) & " 待生效資料已有主要主管資料存在!"
                                    bolCheckData = False
                                End If
                                strSqlWhere = " And CompID = " & Bsp.Utility.Quote(Trim(myDataRead(nintCompID).ToString)) & " And EmpID = " & Bsp.Utility.Quote(Trim(myDataRead(nintEmpID).ToString))
                                strSqlWhere &= " And ValidMark='0' And BossType = '1'"
                                If objHR.IsDataExists("Tmp_EmpAdditionWait", strSqlWhere) Then
                                    strErrMsg = strErrMsg & "==> " & Trim(myDataRead(nintBossType).ToString) & " 待生效資料已有主要主管資料存在!"
                                    bolCheckData = False
                                End If
                                strSqlWhere = " And CompID = " & Bsp.Utility.Quote(Trim(myDataRead(nintCompID).ToString)) & " And EmpID = " & Bsp.Utility.Quote(Trim(myDataRead(nintEmpID).ToString))
                                strSqlWhere &= " And ValidMark='0' And BossType = '1'"
                                If objHR.IsDataExists("EmpAdditionWait", strSqlWhere) Then
                                    strErrMsg = strErrMsg & "==> " & Trim(myDataRead(nintBossType).ToString) & " 待生效資料已有主要主管資料存在!"
                                    bolCheckData = False
                                End If
                            End If
                            '檢查是否有同單位的主管註記
                            If Trim(myDataRead(nintIsBoss).ToString) = "1" Then
                                If Trim(myDataRead(nintCompID).ToString) = Trim(myDataRead(nintAddCompID).ToString) Then
                                    strSqlWhere = " And CompID = " & Bsp.Utility.Quote(Trim(myDataRead(nintAddCompID).ToString)) & " And OrganID = " & Bsp.Utility.Quote(Trim(myDataRead(nintAddOrganID).ToString)) & " And EmpID<>" & Bsp.Utility.Quote(Trim(myDataRead(nintEmpID).ToString))
                                    strSqlWhere &= " And ValidMark='0' And IsBoss='1' "
                                    strSqlWhere &= " And ValidDate = " & Bsp.Utility.Quote(FormatDateTime(strValidDate, DateFormat.ShortDate))   '20160104 wei add 增加生效日判斷
                                    strSqlWhere &= " And Reason in (Select Reason From WorkStatus_EmployeeReason where AfterWorkStatusType in ('1')) " '20160104 wei add 增加判斷異動後需為在職的原因
                                    If objHR.IsDataExists("EmployeeWait", strSqlWhere) Then
                                        strErrMsg = strErrMsg & "==> " & Trim(myDataRead(nintAddOrganID).ToString) & " 待生效資料已有相同單位的單位主管註記!"
                                        bolCheckData = False
                                    End If
                                    strSqlWhere = " And AddCompID = " & Bsp.Utility.Quote(Trim(myDataRead(nintAddCompID).ToString)) & " And AddOrganID = " & Bsp.Utility.Quote(Trim(myDataRead(nintAddOrganID).ToString)) & " And EmpID<>" & Bsp.Utility.Quote(Trim(myDataRead(nintEmpID).ToString))
                                    strSqlWhere &= " And ValidMark='0' And IsBoss='1' "
                                    strSqlWhere &= " And ValidDate = " & Bsp.Utility.Quote(FormatDateTime(strValidDate, DateFormat.ShortDate))   '20160104 wei add 增加生效日判斷
                                    strSqlWhere &= " And Reason in ('1') " '20160104 wei add 增加判斷異動後需為在職的原因
                                    If objHR.IsDataExists("EmpAdditionWait", strSqlWhere) Then
                                        strErrMsg = strErrMsg & "==> " & Trim(myDataRead(nintAddOrganID).ToString) & " 待生效資料已有相同單位的單位主管註記!"
                                        bolCheckData = False
                                    End If
                                End If
                            End If
                            If Trim(myDataRead(nintIsGroupBoss).ToString) = "1" Then
                                If Trim(myDataRead(nintCompID).ToString) = Trim(myDataRead(nintAddCompID).ToString) Then
                                    strSqlWhere = " And CompID = " & Bsp.Utility.Quote(Trim(myDataRead(nintAddCompID).ToString)) & " And FlowOrganID = " & Bsp.Utility.Quote(Trim(myDataRead(nintAddFlowOrganID).ToString)) & " And EmpID<>" & Bsp.Utility.Quote(Trim(myDataRead(nintEmpID).ToString))
                                    strSqlWhere &= " And ValidMark='0' And IsGroupBoss='1' "
                                    strSqlWhere &= " And ValidDate = " & Bsp.Utility.Quote(FormatDateTime(strValidDate, DateFormat.ShortDate))   '20160104 wei add 增加生效日判斷
                                    strSqlWhere &= " And Reason in (Select Reason From WorkStatus_EmployeeReason where AfterWorkStatusType in ('1')) " '20160104 wei add 增加判斷異動後需為在職的原因
                                    If objHR.IsDataExists("EmployeeWait", strSqlWhere) Then
                                        strErrMsg = strErrMsg & "==> " & Trim(myDataRead(nintAddFlowOrganID).ToString) & " 待生效資料已有相同最小簽核單位的單位主管註記!"
                                        bolCheckData = False
                                    End If
                                    strSqlWhere = " And AddCompID = " & Bsp.Utility.Quote(Trim(myDataRead(nintAddCompID).ToString)) & " And AddFlowOrganID = " & Bsp.Utility.Quote(Trim(myDataRead(nintAddFlowOrganID).ToString)) & " And EmpID<>" & Bsp.Utility.Quote(Trim(myDataRead(nintEmpID).ToString))
                                    strSqlWhere &= " And ValidMark='0' And IsGroupBoss='1' "
                                    strSqlWhere &= " And ValidDate = " & Bsp.Utility.Quote(FormatDateTime(strValidDate, DateFormat.ShortDate))   '20160104 wei add 增加生效日判斷
                                    strSqlWhere &= " And Reason in ('1') " '20160104 wei add 增加判斷異動後需為在職的原因
                                    If objHR.IsDataExists("EmpAdditionWait", strSqlWhere) Then
                                        strErrMsg = strErrMsg & "==> " & Trim(myDataRead(nintAddFlowOrganID).ToString) & " 待生效資料已有相同最小簽核單位的單位主管註記!"
                                        bolCheckData = False
                                    End If
                                End If
                            End If
                        Else
                            If Trim(myDataRead(nintIsBoss).ToString) <> "" Or Trim(myDataRead(nintIsGroupBoss).ToString) <> "" Then
                                strErrMsg = strErrMsg & "==> " & Trim(myDataRead(nintBossType).ToString) & " 已選擇單位主管或簽核單位主管，請選擇主管任用方式!"
                                bolCheckData = False
                            End If
                        End If
                        
                        '備註
                        If Trim(myDataRead(nintRemark).ToString) <> "" And Len(Trim(myDataRead(nintRemark).ToString)) > 500 Then
                            strErrMsg = strErrMsg & "==> " & Trim(myDataRead(nintRemark).ToString) & " 備註，最多可輸入250個中文字或500個英數字!"
                            bolCheckData = False
                        End If

                        If Trim(myDataRead(nintCompID).ToString) <> "" And Trim(myDataRead(nintEmpID).ToString) <> "" And strValidDate <> "" And Trim(myDataRead(nintAddCompID).ToString) <> "" And Trim(myDataRead(nintAddDeptID).ToString) <> "" And Trim(myDataRead(nintAddOrganID).ToString) <> "" Then

                            '檢核是否有相同單位的待異動調兼檔 
                            strSqlWhere = ""
                            strSqlWhere = strSqlWhere & " and CompID=" & Bsp.Utility.Quote(Trim(myDataRead(nintCompID).ToString))
                            strSqlWhere = strSqlWhere & " and EmpID=" & Bsp.Utility.Quote(Trim(myDataRead(nintEmpID).ToString))
                            strSqlWhere = strSqlWhere & " and AddCompID=" & Bsp.Utility.Quote(Trim(myDataRead(nintAddCompID).ToString))
                            strSqlWhere = strSqlWhere & " and AddDeptID=" & Bsp.Utility.Quote(Trim(myDataRead(nintAddDeptID).ToString))
                            strSqlWhere = strSqlWhere & " and AddOrganID=" & Bsp.Utility.Quote(Trim(myDataRead(nintAddOrganID).ToString))
                            strSqlWhere = strSqlWhere & " and ValidMark='0'"
                            If objHR.IsDataExists("EmpAdditionWait", strSqlWhere) Then
                                strErrMsg = strErrMsg & "==> 資料重複：待異動調兼紀錄已有相同單位的調兼資料存在！"
                                bolCheckData = False
                            End If


                            strSqlWhere = ""
                            strSqlWhere = strSqlWhere & " and CompID=" & Bsp.Utility.Quote(Trim(myDataRead(nintCompID).ToString))
                            strSqlWhere = strSqlWhere & " and EmpID=" & Bsp.Utility.Quote(Trim(myDataRead(nintEmpID).ToString))
                            strSqlWhere = strSqlWhere & " and AddCompID=" & Bsp.Utility.Quote(Trim(myDataRead(nintAddCompID).ToString))
                            strSqlWhere = strSqlWhere & " and AddDeptID=" & Bsp.Utility.Quote(Trim(myDataRead(nintAddDeptID).ToString))
                            strSqlWhere = strSqlWhere & " and AddOrganID=" & Bsp.Utility.Quote(Trim(myDataRead(nintAddOrganID).ToString))
                            If objHR.IsDataExists("EmpAddition", strSqlWhere) Then
                                strErrMsg = strErrMsg & "==> 資料重複：調兼紀錄已有相同單位的調兼資料存在！"
                                bolCheckData = False
                            End If



                            '20150602 wei add 異動前A單位為A主管(主管or簽核主管)，待異動輸入A單位換B主管，跳出提醒視窗確認【A單位將更換為B主管，請確認是否繼續儲存?】，按鈕有【確認，取消】，若確認，則繼續儲存
                            '主管
                            If Trim(myDataRead(nintIsBoss).ToString) <> "" Then
                                strSqlWhere = " And CompID = " & Bsp.Utility.Quote(Trim(myDataRead(nintAddCompID).ToString)) & " And OrganID = " & Bsp.Utility.Quote(Trim(myDataRead(nintAddOrganID).ToString))
                                strSqlWhere &= " And BossCompID = " & Bsp.Utility.Quote(Trim(myDataRead(nintCompID).ToString)) & " And Boss = " & Bsp.Utility.Quote(Trim(myDataRead(nintEmpID).ToString))
                                If Not objHR.IsDataExists("Organization", strSqlWhere) Then

                                    strErrMsg = strErrMsg & "==> 警告:" & Trim(myDataRead(nintAddOrganID).ToString) & "主管將更換為" & Trim(myDataRead(nintEmpID).ToString) & "！"
                                End If
                            End If
                            '簽核主管
                            If Trim(myDataRead(nintIsGroupBoss).ToString) <> "" Then
                                strSqlWhere = " And OrganID = " & Bsp.Utility.Quote(Trim(myDataRead(nintAddFlowOrganID).ToString))
                                strSqlWhere &= " And BossCompID = " & Bsp.Utility.Quote(Trim(myDataRead(nintCompID).ToString)) & " And Boss = " & Bsp.Utility.Quote(Trim(myDataRead(nintEmpID).ToString))
                                If Not objHR.IsDataExists("OrganizationFlow", strSqlWhere) Then
                                    strErrMsg = strErrMsg & "==> 警告:" & Trim(myDataRead(nintAddFlowOrganID).ToString) & "主管將更換為" & Trim(myDataRead(nintEmpID).ToString) & "！"
                                End If
                            End If

                        End If

                        If bolCheckData Then
                            If funUploadDataSingle_ByEmpAdditionWait(myDataRead, intSeq) Then
                                If strErrMsg = "" Then
                                    intSuccessCount = intSuccessCount + 1
                                Else
                                    intWarningCount = intWarningCount + 1
                                End If
                            Else
                                intErrorCount = intErrorCount + 1
                            End If
                        Else
                            intErrorCount = intErrorCount + 1
                        End If
                    End If

                    If strErrMsg <> "" Then
                        subWriteLog("資料列:" & intTotalCount + 1 & " 公司代碼：" & Trim(myDataRead(nintCompID).ToString) & " 員工編號：" & Trim(myDataRead(nintEmpID).ToString) & " 生效日期：" & strValidDate & " 異動原因：" & Trim(myDataRead(nintReason).ToString) & strErrMsg)
                        bolInputStatus = False
                    End If

                    intTotalCount = intTotalCount + 1
                End While
            End Using

            myExcelConn.Close()

            If bolInputStatus = False Then
                Return False
            Else
                Return True
            End If

        Catch ex As Exception
            myExcelConn.Close()
            Bsp.Utility.ShowMessage(Me, Me.FunID & ".funCheckData1", ex)
            Return False
        End Try

    End Function
    '20150609 wei add HR3000-EmpAdditionWait
    Private Function funUploadDataSingle_ByEmpAdditionWait(ByVal myDataRead As OleDbDataReader,ByVal intSeq As Integer) As Boolean
        Dim objHR As New HR
        Dim objHR3000 As New HR3000
        Dim bsEmpAdditionWait As New beEmpAdditionWait.Service()
        Dim beEmpAdditionWait As New beEmpAdditionWait.Row
        Dim dtNow = DateTime.Now

        '畫面輸入的條件

        Using cn As DbConnection = Bsp.DB.getConnection("eHRMSDB")
            cn.Open()

            Dim tran As DbTransaction = cn.BeginTransaction
            Dim inTrans As Boolean = True

            '"HR3000 EmpAdditionWait 上傳作業 _分隔符號『  |  』)"			
            'NO.	名稱          格式    長度    必填	說明
            '1	公司代碼          字串    6       Y
            '2	員工編號          字串    6       Y	
            '3	生效日期          日期    10      Y   YYYY/MM/DD
            '4	人令              字串    50      Y        	
            '5	兼任狀態          字串    1       Y       	
            '6	兼任公司          字串    6       Y 	
            '7	兼任部門          字串    12      Y  	
            '8	兼任科組課        字串    12      Y        	
            '9	兼任最小簽核單位   字串    12       	
            '10	主管任用方式       字串    1      1 主要 2 兼任
            '11	主管註記        	  字串    1      0:否，1:是
            '12	簽核單位主管註記	  字串    1   	 0:否，1:是
            '13	副主管         	  字串    1      0:否，1:是
            '14	簽核單位副主管  	  字串    1      0:否，1:是
            '15 備註              字串    500       


            '補充說明：欄位非必填且放空白	
            Dim nintCompID As Integer = 0
            Dim nintEmpID As Integer = 1
            Dim nintValidDate As Integer = 2
            Dim nintFileNo As Integer = 3
            Dim nintReason As Integer = 4
            Dim nintAddCompID As Integer = 5
            Dim nintAddDeptID As Integer = 6
            Dim nintAddOrganID As Integer = 7
            Dim nintAddFlowOrganID As Integer = 8
            Dim nintBossType As Integer = 9
            Dim nintIsBoss As Integer = 10
            Dim nintIsGroupBoss As Integer = 11
            Dim nintIsSecBoss As Integer = 12
            Dim nintIsSecGroupBoss As Integer = 13
            Dim nintRemark As Integer = 14

            Dim intLoop As Integer = 0

            Try

                '公司代碼
                beEmpAdditionWait.CompID.Value = myDataRead(nintCompID).ToString
                '員工編號
                beEmpAdditionWait.EmpID.Value = myDataRead(nintEmpID).ToString
                '生效日期
                beEmpAdditionWait.ValidDate.Value = myDataRead(nintValidDate).ToString
                '待異動序號
                beEmpAdditionWait.Seq.Value = intSeq
                '待異動調兼序號
                beEmpAdditionWait.AddSeq.Value = objHR3000.GetEmpAdditionWaitSeq(myDataRead(nintCompID).ToString, myDataRead(nintEmpID).ToString, myDataRead(nintValidDate).ToString, intSeq)
                '人令
                beEmpAdditionWait.FileNo.Value = myDataRead(nintFileNo).ToString
                '兼任狀況代碼
                beEmpAdditionWait.Reason.Value = myDataRead(nintReason).ToString
                '兼任公司代碼
                beEmpAdditionWait.AddCompID.Value = myDataRead(nintAddCompID).ToString
                '兼任部門代碼
                beEmpAdditionWait.AddDeptID.Value = myDataRead(nintAddDeptID).ToString
                '調兼科組課代碼
                beEmpAdditionWait.AddOrganID.Value = myDataRead(nintAddOrganID).ToString
                '兼任最小簽核單位代碼
                beEmpAdditionWait.AddFlowOrganID.Value = myDataRead(nintAddFlowOrganID).ToString
                '主管任用方式
                beEmpAdditionWait.BossType.Value = myDataRead(nintBossType).ToString
                '主管註記
                beEmpAdditionWait.IsBoss.Value = myDataRead(nintIsBoss).ToString
                '簽核單位主管註記
                beEmpAdditionWait.IsGroupBoss.Value = myDataRead(nintIsGroupBoss).ToString
                '副主管
                beEmpAdditionWait.IsSecBoss.Value = myDataRead(nintIsSecBoss).ToString
                '簽核單位副主管
                beEmpAdditionWait.IsSecGroupBoss.Value = myDataRead(nintIsSecGroupBoss).ToString
                '備註
                beEmpAdditionWait.Remark.Value = myDataRead(nintRemark).ToString

                beEmpAdditionWait.ValidMark.Value = "0"
                beEmpAdditionWait.ExistsEmpAddition.Value = "0"

                beEmpAdditionWait.CreateComp.Value = UserProfile.ActCompID
                beEmpAdditionWait.CreateID.Value = UserProfile.ActUserID
                beEmpAdditionWait.CreateDate.Value = Now

                beEmpAdditionWait.LastChgComp.Value = UserProfile.ActCompID
                beEmpAdditionWait.LastChgID.Value = UserProfile.ActUserID
                beEmpAdditionWait.LastChgDate.Value = Now

                If bsEmpAdditionWait.IsDataExists(beEmpAdditionWait) Then
                    tran.Rollback()
                    inTrans = False
                    Return False
                End If

                bsEmpAdditionWait.Insert(beEmpAdditionWait, tran)

                tran.Commit()
                inTrans = False
                Return True

            Catch ex As Exception

                If inTrans Then tran.Rollback()
                Return False

                Bsp.Utility.ShowMessage(Me, Me.FunID & ".funUploadDataSingle_ByEmpAddition", ex)
            End Try
        End Using
    End Function
    Private Sub subWriteLog(ByVal strLogString As String, Optional ByVal strFileformat As String = "Unicode")
        '產生檔案寫法一
        'Dim FileNum As Integer
        'FileNum = FreeFile()
        'FileOpen(FileNum, strLogFileName, OpenMode.Append)
        'PrintLine(FileNum, strLogString)
        'FileClose(FileNum)

        '產生檔案寫法二---若要存成Unicode TXT檔，需改這種寫法，指定要輸出別的編碼方式
        Dim strFileName As String = Server.MapPath(Bsp.Utility.getAppSetting("TempPath")) & "\" & hidLogFileName.Value
        Using Writer As System.IO.StreamWriter = New System.IO.StreamWriter(strFileName, True, System.Text.Encoding.Unicode)
            Writer.WriteLine(strLogString)
        End Using

    End Sub

    '將ERR LOG檔案立即提供USER端下載，完成後刪除檔案
    '若發生ERR LOG檔案在SERVER有產生，但無法提供下載(USER端無跳出檔案下載視窗畫面)~~WHY??=>IE\工具\網際網路選項\安全性\加入信任的網站 即可!!!!!
    Private Sub subDownloadLogFile()

        '若有多個ShowFormatMessage時，只會跳最後一個
        'Bsp.Utility.ShowFormatMessage(Me, "W_00031", "subDownloadLogFile!")

        Dim strFileName As String = Server.MapPath(Bsp.Utility.getAppSetting("TempPath")) & "\" & hidLogFileName.Value
        If System.IO.File.Exists(strFileName) Then
            Response.ClearContent()
            Response.BufferOutput = True
            Response.Charset = "utf-8"
            '設定MIME類型  
            'Response.ContentType = "application/ms-excel"       '只寫ms-excel不OK，會變成程式碼下載@@
            'Response.ContentType = "application/vnd.ms-excel"
            'Response.ContentType = "application/save-as"
            'Response.ContentType = "Application/unknown"
            Response.ContentType = "application/octet-stream"

            Response.AddHeader("Content-Transfer-Encoding", "binary")
            Response.ContentEncoding = System.Text.Encoding.Default
            '跳出視窗，讓用戶端選擇要儲存的地方     '=>使用Server.UrlPathEncode()編碼中文字才不會下載時，檔名為亂碼
            Response.AddHeader("Content-Disposition", "attachment; filename=" & Server.UrlPathEncode(hidLogFileName.Value))
            '=>使用Server.UrlEncode()還是亂碼
            'http://blog.miniasp.com/post/2008/04/20/ASPNET-Force-Download-File-and-deal-with-Chinese-Filename-correctly.aspx
            '「網址的路徑(Path)」與「網址的參數(QueryString)」編碼方式不一樣! 
            '路徑包括目錄名稱與檔案名稱的部分，要用 HttpUtility.UrlPathEncode 編碼。 
            '參數的部分才用 HttpUtility.UrlEncode 編碼。 
            '例如：空白字元( )用 HttpUtility.UrlPathEncode 會變成(%20)，但用 HttpUtility.UrlEncode 卻會變成加號(+)，而檔名中空白的部分用 %20 才是對的，否則存檔後檔名空白的部分會變成加號(+)那檔名就不對了

            '檔案有各式各樣，所以用BinaryWrite 
            Response.BinaryWrite(File.ReadAllBytes(strFileName))
            'Response.WriteFile(strFileName)                     '這寫法也可

            '刪除Server上log檔
            File.Delete(strFileName)

            Response.End()      '加這行後續的程式執行程式碼才不會寫入檔案，但後續處理就都不會進行了
        Else
            Response.Write("無檔案")
        End If

        '----------------------------------------------------
        '這寫法也可
        'Dim default_file_root As String = Server.MapPath(Bsp.Utility.getAppSetting("TempPath")) & "\"
        'Dim user_request As String = "test.txt"

        'Dim filep As String = default_file_root & user_request
        'If System.IO.File.Exists(filep) Then
        '    With Response
        '        .ContentType = "application/save-as"
        '        .AddHeader("content-disposition", "attachment; filename=" & user_request)
        '        .WriteFile(filep)
        '        .End()
        '    End With
        'Else
        '    Response.Write("無檔案")
        'End If

    End Sub
    '如同VB 6.0的LenB函數，傳回字串aStr的位元組長度    
    Public Shared Function StrLenB(ByVal vstrValue As String) As Integer
        Dim i, k As Integer
        For i = 1 To Len(vstrValue)
            k += CharByte(Mid(vstrValue, i, 1))
        Next
        Return k

    End Function
    Public Shared Function CharByte(ByVal vstrWord As String) As Integer
        If Len(vstrWord) = 0 Then
            Return 0
        Else
            Select Case Asc(vstrWord)
                Case 0 To 255
                    Return 1
                Case Else
                    Return 2
            End Select
        End If
    End Function
    Private Sub subDownloadSampleFile(ByVal strFile As String)

        '若有多個ShowFormatMessage時，只會跳最後一個
        'Bsp.Utility.ShowFormatMessage(Me, "W_00031", "subDownloadLogFile!")

        Dim strFileName As String = Server.MapPath(Bsp.Utility.getAppSetting("DownloadPath")) & "\" & strFile
        If System.IO.File.Exists(strFileName) Then
            Response.ClearContent()
            Response.BufferOutput = True
            Response.Charset = "utf-8"
            '設定MIME類型  
            'Response.ContentType = "application/ms-excel"       '只寫ms-excel不OK，會變成程式碼下載@@
            'Response.ContentType = "application/vnd.ms-excel"
            'Response.ContentType = "application/save-as"
            'Response.ContentType = "Application/unknown"
            Response.ContentType = "application/octet-stream"

            Response.AddHeader("Content-Transfer-Encoding", "binary")
            Response.ContentEncoding = System.Text.Encoding.Default
            '跳出視窗，讓用戶端選擇要儲存的地方     '=>使用Server.UrlPathEncode()編碼中文字才不會下載時，檔名為亂碼
            Response.AddHeader("Content-Disposition", "attachment; filename=" & Server.UrlPathEncode(strFile))
            '=>使用Server.UrlEncode()還是亂碼
            'http://blog.miniasp.com/post/2008/04/20/ASPNET-Force-Download-File-and-deal-with-Chinese-Filename-correctly.aspx
            '「網址的路徑(Path)」與「網址的參數(QueryString)」編碼方式不一樣! 
            '路徑包括目錄名稱與檔案名稱的部分，要用 HttpUtility.UrlPathEncode 編碼。 
            '參數的部分才用 HttpUtility.UrlEncode 編碼。 
            '例如：空白字元( )用 HttpUtility.UrlPathEncode 會變成(%20)，但用 HttpUtility.UrlEncode 卻會變成加號(+)，而檔名中空白的部分用 %20 才是對的，否則存檔後檔名空白的部分會變成加號(+)那檔名就不對了

            '檔案有各式各樣，所以用BinaryWrite 
            Response.BinaryWrite(File.ReadAllBytes(strFileName))
            'Response.WriteFile(strFileName)                     '這寫法也可

            Response.End()      '加這行後續的程式執行程式碼才不會寫入檔案，但後續處理就都不會進行了
        Else
            Response.Write("無檔案")
        End If

        '----------------------------------------------------
        '這寫法也可
        'Dim default_file_root As String = Server.MapPath(Bsp.Utility.getAppSetting("TempPath")) & "\"
        'Dim user_request As String = "test.txt"

        'Dim filep As String = default_file_root & user_request
        'If System.IO.File.Exists(filep) Then
        '    With Response
        '        .ContentType = "application/save-as"
        '        .AddHeader("content-disposition", "attachment; filename=" & user_request)
        '        .WriteFile(filep)
        '        .End()
        '    End With
        'Else
        '    Response.Write("無檔案")
        'End If

    End Sub
    Protected Sub btnDownload_Click(sender As Object, e As System.EventArgs) Handles btnDownload.Click
        subDownloadSampleFile("HR3000上傳格式說明.xls")
    End Sub
    '20150604 wei add 增加上傳同時寫入EmpAdditionWati
    Private Sub EmpAddition_CopyTo_EmpAdditionWait(ByVal strCompID As String, ByVal strEmpID As String, ByVal strModifyDate As String, ByVal intSeq As Integer, ByVal tran As DbTransaction)
        Dim strSQL As New StringBuilder()

        strSQL.AppendLine("Insert Into EmpAdditionWait (CompID,EmpID,ValidDate,Seq,AddSeq,AddCompID,AddDeptID,AddOrganID,AddFlowOrganID,Reason," & _
                                                                    "FileNo,Remark,IsBoss,IsSecBoss,IsGroupBoss,IsSecGroupBoss,BossType,ValidMark,ExistsEmpAddition," & _
                                                                    "CreateDate,CreateComp,CreateID,LastChgDate,LastChgComp,LastChgID)")
        strSQL.AppendLine("Select CompID,EmpID," & Bsp.Utility.Quote(strModifyDate) & "," & intSeq & " as Seq,Row_number() OVER (ORDER BY EmpID DESC) AS AddSeq")
        strSQL.AppendLine(",AddCompID,AddDeptID,AddOrganID,AddFlowOrganID,Reason")
        strSQL.AppendLine(",FileNo,Remark,IsBoss,IsSecBoss,IsGroupBoss,IsSecGroupBoss,BossType")
        strSQL.AppendLine(",'0' as ValidMark,'1' as ExistsEmpAddition")
        strSQL.AppendLine(",getdate()")
        strSQL.AppendLine("," & Bsp.Utility.Quote(UserProfile.ActCompID))
        strSQL.AppendLine("," & Bsp.Utility.Quote(UserProfile.ActUserID))
        strSQL.AppendLine(",getdate()")
        strSQL.AppendLine("," & Bsp.Utility.Quote(UserProfile.ActCompID))
        strSQL.AppendLine("," & Bsp.Utility.Quote(UserProfile.ActUserID))
        strSQL.AppendLine("From EmpAddition")
        strSQL.AppendLine("Where CompID = " & Bsp.Utility.Quote(strCompID))
        strSQL.AppendLine("And EmpID = " & Bsp.Utility.Quote(strEmpID))
        strSQL.AppendLine("Order by ValidDate")

        Bsp.DB.ExecuteNonQuery(CommandType.Text, strSQL.ToString(), tran, "eHRMSDB")

    End Sub
End Class




