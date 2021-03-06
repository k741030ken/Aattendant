'****************************************************
'功能說明：排序資料上傳-->檔案上傳
'建立人員：wei
'建立日期：2015/11/11
'****************************************************
Imports System.Data
Imports System.Data.Common
Imports Microsoft.Practices.EnterpriseLibrary.Common
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports System.IO
Imports System.IO.Compression
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports SinoPac.WebExpress.Common   '20160513 wei add NetAP dll匯出xlsx使用
Imports OfficeOpenXml   '20160513 wei add
Imports OfficeOpenXml.Style '20160513 wei add

Partial Class GS_GS1201
    Inherits PageBase

    Public Shared intSuccessCount As Integer = 0
    Public Shared intErrorCount As Integer = 0
    Public Shared intTotalCount As Integer = 0
    Public Shared intWarningCount As Integer = 0

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then

        End If
    End Sub
    Protected Overrides Sub BaseOnPageCall(ByVal ti As TransferInfo)
        If Not IsPostBack() Then
            Dim ht As Hashtable = Bsp.Utility.getHashTableFromParam(ti.Args)

            ViewState.Item("CompID") = ht("CompID").ToString()
            ViewState.Item("ApplyID") = ht("ApplyID").ToString()
            ViewState.Item("ApplyTime") = ht("ApplyTime").ToString()
            ViewState.Item("Seq") = ht("Seq").ToString()
            ViewState.Item("MainFlag") = ht("MainFlag").ToString()
            ViewState.Item("GradeYear") = ht("GradeYear").ToString()
            ViewState.Item("GradeSeq") = ht("GradeSeq").ToString()
            ViewState.Item("EvaluationSeq") = ht("EvaluationSeq").ToString()
            ViewState.Item("DeptEX") = ht("DeptEX").ToString()
            ViewState.Item("IsSignNext") = ht("IsSignNext").ToString()
            ViewState.Item("Result") = ht("Result").ToString()
            ViewState.Item("GroupID") = ht("GroupID").ToString()

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
                    Dim strFileName As String = Bsp.Utility.GetNewFileName("GradeOrder_Upload")

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
                    hidLogFileName.Value = strLogFile & strFileName & ".txt"

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
            strTabName = "GradeOrder_Upload$"

            strUpload_TableName = objHR.funCheck_UploadTableName_Xlsx(strFileName).Replace("'", "").ToString

            If strUpload_TableName <> strTabName Then   '201540609 wei modify "HR3000$"
                File.Delete(strFileName)
                strMessage = "上傳檔案的工作表(WorkSheet)-" & Mid(strUpload_TableName, 1, Len(strUpload_TableName) - 1) & " 與選擇上傳類型-" & strTabName.Substring(0, Len(strTabName) - 1) & "不一致!" '20150609 wei modify
                Bsp.Utility.RunClientScript(Me, "alert('" & String.Format(Bsp.Utility.getMessage("E_00050"), strMessage) & "');" & vbCrLf & _
                                "window.top.returnValue = 'OK';")
            End If

            If objHR.funCheck_UploadCount_Xlsx(strFileName, "[" & strTabName & "]") > CInt(ConfigurationManager.AppSettings("Upload_MaxCount").ToString) Then '20150609 wei modify
                File.Delete(strFileName)
                strMessage = "上傳資料筆數過大，系統允許最大上傳筆數：" & ConfigurationManager.AppSettings("Upload_MaxCount").ToString
                Bsp.Utility.RunClientScript(Me, "alert('" & String.Format(Bsp.Utility.getMessage("E_00050"), strMessage) & "');" & vbCrLf & _
                                "window.top.returnValue = 'OK';")
            End If

            If funCheckData1(strFileName) Then
                File.Delete(strFileName)
                strMessage = "排序資料上傳總筆數：" & intTotalCount & " 成功筆數：" & intSuccessCount & " 警告筆數：" & intWarningCount & " 失敗筆數：" & intErrorCount
                Bsp.Utility.RunClientScript(Me, "alert('" & String.Format(Bsp.Utility.getMessage("I_00020"), strMessage) & "');" & vbCrLf & _
                            "window.top.returnValue = 'OK';" & vbCrLf & "window.top.close();")
            Else
                '刪除上傳檔案
                File.Delete(strFileName)
                strMessage = "排序資料上傳總筆數：" & intTotalCount & " 成功筆數：" & intSuccessCount & " 警告筆數：" & intWarningCount & " 失敗筆數：" & intErrorCount & "，請下載錯誤紀錄log檔案!"
                '顯示上傳失敗錯誤訊息，並下載ERR LOG檔案
                Bsp.Utility.RunClientScript(Me.Page, "IsTODownload('" & strMessage & "');")

            End If

        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & ".DoUpload", ex)
        End Try

    End Sub

    Private Function funCheckData1(ByVal FileName As String) As Boolean

        Dim objHR As New HR
        Dim objGS As New GS1
        Dim bolInputStatus As Boolean = True
        Dim strErrMsg As String = ""
        Dim strSqlWhere As String = ""
        Dim strValidDate As String = ""

        Dim intLoop As Integer = 0

        Dim bolCheckData As Boolean = False  '檢核資料

        '畫面輸入的條件

        '"GradeOrder_Upload上傳作業 _分隔符號『  |  』)"			
        'NO.	名稱	        格式    長度    必填	說明
        '1	員工編號	        字串    6       Y 	
        '2	姓名	            日期             
        '3	單位	            字串                  	
        '4	到職日	        字串              	YYYY/MM/DD
        '5	職位	            字串            	
        '6	職等 	        字串  
        '7	單位考績           	字串            	
        '8	單位排序        	字串    1       Y 	
        '9	考績           	字串            	
        '10	年度排序        	字串    1       Y 	
        '11	補充說明        	字串    1       Y 	
        '補充說明：欄位非必填且放空白	
        Dim nintEmpID As Integer = 0
        Dim nintName As Integer = 1
        Dim nintOrganName As Integer = 2
        Dim nintEmpDate As Integer = 3
        Dim nintPosition As Integer = 4
        Dim nintRankID As Integer = 5
        Dim nintGradeDept As Integer = 6
        Dim nintGradeOrderDept As Integer = 7
        Dim nintGrade As Integer = 8
        Dim nintGradeOrder As Integer = 9
        Dim nintGradeComment As Integer = 10

        intTotalCount = 0
        intSuccessCount = 0
        intErrorCount = 0
        intWarningCount = 0

        Dim strExcelConn As String = "Provider=Microsoft.ACE.OLEDB.12.0;;Data Source=" & FileName & ";Extended Properties='EXCEL 8.0;HDR=Yes;IMEX=1;'"    '20160513 wei modify
        'Dim strExcelConn As String = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & FileName & ";Extended Properties='EXCEL 8.0;HDR=Yes;IMEX=1;'"    '20150401 wei modify

        Dim strExcelWorkSheet As String = "[GradeOrder_Upload$]"
        Dim strExcelSelect As String = "SELECT * FROM " & strExcelWorkSheet

        Dim myExcelConn As OleDbConnection = Nothing
        myExcelConn = New OleDbConnection(strExcelConn)
        Dim myExcelCommand As OleDbCommand = New OleDbCommand(strExcelSelect, myExcelConn)
        Try
            myExcelConn.Open()
            Using myDataRead As OleDbDataReader = myExcelCommand.ExecuteReader
                While myDataRead.Read

                    '檢核上傳資料
                    strErrMsg = ""
                    bolCheckData = True

                    '先判斷是否上傳欄位個數正確
                    If myDataRead.FieldCount <> 11 Then
                        strErrMsg = strErrMsg & "==> 上傳資料欄位個數不正確！"
                        intErrorCount = intErrorCount + 1
                    Else
                        '員工編號
                        '20160713 wei del
                        'strSqlWhere = ""
                        'strSqlWhere = strSqlWhere & " and CompID =" & Bsp.Utility.Quote(Trim(ViewState.Item("CompID")))
                        'strSqlWhere = strSqlWhere & " and EmpID =" & Bsp.Utility.Quote(Trim(myDataRead(nintEmpID).ToString))
                        'strSqlWhere = strSqlWhere & " and WorkStatus='1' "
                        'If Not objHR.IsDataExists("Personal", strSqlWhere) Then
                        '    strErrMsg = strErrMsg & "==> " & Trim(myDataRead(nintEmpID).ToString) & " 員工主檔不存在!"
                        '    bolCheckData = False
                        'End If
                        strSqlWhere = ""
                        strSqlWhere = strSqlWhere & " and GradeYear =" & Bsp.Utility.Quote(Trim(ViewState.Item("GradeYear")))
                        strSqlWhere = strSqlWhere & " and GradeSeq =" & Bsp.Utility.Quote(Trim(ViewState.Item("GradeSeq")))
                        strSqlWhere = strSqlWhere & " and CompID =" & Bsp.Utility.Quote(Trim(ViewState.Item("CompID")))
                        strSqlWhere = strSqlWhere & " and EmpID =" & Bsp.Utility.Quote(Trim(myDataRead(nintEmpID).ToString))
                        strSqlWhere = strSqlWhere & " and Online='1' "
                        If Not objHR.IsDataExists("GradeBase", strSqlWhere) Then
                            strErrMsg = strErrMsg & "==> " & Trim(myDataRead(nintEmpID).ToString) & " 考核名單檔無此員工!"
                            bolCheckData = False
                        End If

                        If Not IsNumeric(Trim(myDataRead(nintGradeOrder).ToString)) And Trim(myDataRead(nintGradeOrder).ToString) <> "" Then
                            strErrMsg = strErrMsg & "==> " & Trim(myDataRead(nintGradeOrder).ToString) & " 排序為非數字!"
                            bolCheckData = False
                        End If

                        If Trim(myDataRead(nintGradeComment).ToString).Length > 100 Then
                            strErrMsg = strErrMsg & "==> " & Trim(myDataRead(nintGradeComment).ToString) & " 考績補充說明請勿超過100個字!"
                            bolCheckData = False
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
                        subWriteLog("資料列:" & intTotalCount + 1 & " 員工編號：" & Trim(myDataRead(nintEmpID).ToString) & " 姓名：" & Trim(myDataRead(nintName).ToString) & " 單位：" & Trim(myDataRead(nintOrganName).ToString) & strErrMsg)
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
    '資料上傳 單筆 
    Private Function funUploadDataSingle(ByVal myDataRead As OleDbDataReader) As Boolean
        Dim objHR As New HR
        Dim objGS As New GS1
        Dim strSQL As New StringBuilder()
        '畫面輸入的條件

        Using cn As DbConnection = Bsp.DB.getConnection("eHRMSDB")
            cn.Open()

            Dim tran As DbTransaction = cn.BeginTransaction
            Dim inTrans As Boolean = True

            '"GradeOrder_Upload上傳作業 _分隔符號『  |  』)"	
            '1	員工編號	        字串    6       Y 	
            '2	姓名	            日期             
            '3	單位	            字串                  	
            '4	到職日	        字串              	YYYY/MM/DD
            '5	職位	            字串            	
            '6	職等 	        字串  
            '7	單位考績           	字串            	
            '8	單位排序        	字串    1       Y 	
            '9	考績           	字串            	
            '10	年度排序        	字串    1       Y 	
            '11	補充說明        	字串    1       Y 	
            '補充說明：欄位非必填且放空白	
            Dim nintEmpID As Integer = 0
            Dim nintName As Integer = 1
            Dim nintOrganName As Integer = 2
            Dim nintEmpDate As Integer = 3
            Dim nintPosition As Integer = 4
            Dim nintRankID As Integer = 5
            Dim nintGradeDept As Integer = 6
            Dim nintGradeOrderDept As Integer = 7
            Dim nintGrade As Integer = 8
            Dim nintGradeOrder As Integer = 9
            Dim nintGradeComment As Integer = 10

            Dim Grade As String = ""


            Try
                Select Case Trim(myDataRead(nintGrade).ToString)
                    Case "特優"
                        Grade = "9"
                    Case "優"
                        Grade = "1"
                    Case "甲上"
                        Grade = "6"
                    Case "甲"
                        Grade = "2"
                    Case "甲下"
                        Grade = "7"
                    Case "乙"
                        Grade = "3"
                    Case "丙"
                        Grade = "4"
                End Select

                strSQL.AppendLine(ConfigurationManager.AppSettings("eHRMSDBDES").ToString)
                strSQL.AppendLine(" Update GradeSignLog ")
                strSQL.AppendLine(" Set GradeOrder = " & IIf(Trim(myDataRead(nintGradeOrder).ToString) = "", 0, Trim(myDataRead(nintGradeOrder).ToString)))
                strSQL.AppendLine(" ,Grade = EncryptByKey(Key_GUID('eHRMSDBDES'),'" & Grade & "')")    '20160607 wei add
                strSQL.AppendLine(" ,LastChgComp = " & Bsp.Utility.Quote(UserProfile.CompID))
                strSQL.AppendLine(" ,LastChgID = " & Bsp.Utility.Quote(UserProfile.UserID))
                strSQL.AppendLine(" ,LastChgDate = getdate()")
                strSQL.AppendLine(" Where CompID = " & Bsp.Utility.Quote(ViewState.Item("CompID")))
                strSQL.AppendLine(" and ApplyTime = " & Bsp.Utility.Quote(ViewState.Item("ApplyTime")))
                strSQL.AppendLine(" and EmpID = " & Bsp.Utility.Quote(myDataRead(nintEmpID).ToString))
                strSQL.AppendLine(" and Seq = " & Bsp.Utility.Quote(ViewState.Item("Seq")))
                strSQL.AppendLine(" and GradeSeq = " & Bsp.Utility.Quote(ViewState.Item("GradeSeq")) & ";")

                If Trim(myDataRead(nintGradeComment).ToString) <> "" Then
                    strSQL.AppendLine(" Delete From GradeEmpComment ")
                    strSQL.AppendLine(" Where CompID = " & Bsp.Utility.Quote(ViewState.Item("CompID")))
                    strSQL.AppendLine(" and ApplyTime = " & Bsp.Utility.Quote(ViewState.Item("ApplyTime")))
                    strSQL.AppendLine(" and EmpID = " & Bsp.Utility.Quote(myDataRead(nintEmpID).ToString))
                    strSQL.AppendLine(" and Seq = " & Bsp.Utility.Quote(ViewState.Item("Seq")))
                    strSQL.AppendLine(" and GradeSeq = " & Bsp.Utility.Quote(ViewState.Item("GradeSeq")) & ";")

                    strSQL.AppendLine(" Insert into GradeEmpComment (GradeYear,ApplyTime,ApplyID,CompID,EmpID,Seq,GradeSeq,MainFlag,Comment,LastChgComp,LastChgID,LastChgDate) ")
                    strSQL.AppendLine(" Select " & Bsp.Utility.Quote(ViewState.Item("GradeYear")))
                    strSQL.AppendLine(" ,ApplyTime, ApplyID, CompID, EmpID, Seq, GradeSeq")
                    strSQL.AppendLine(" ," & Bsp.Utility.Quote(ViewState.Item("MainFlag")))
                    strSQL.AppendLine(" ," & Bsp.Utility.Quote(Trim(myDataRead(nintGradeComment).ToString)))
                    strSQL.AppendLine(" ," & Bsp.Utility.Quote(UserProfile.CompID))
                    strSQL.AppendLine(" ," & Bsp.Utility.Quote(UserProfile.UserID))
                    strSQL.AppendLine(" ,getdate()")
                    strSQL.AppendLine(" From GradeSignLog")
                    strSQL.AppendLine(" Where CompID = " & Bsp.Utility.Quote(ViewState.Item("CompID")))
                    strSQL.AppendLine(" and ApplyTime = " & Bsp.Utility.Quote(ViewState.Item("ApplyTime")))
                    strSQL.AppendLine(" and EmpID = " & Bsp.Utility.Quote(myDataRead(nintEmpID).ToString))
                    strSQL.AppendLine(" and Seq = " & Bsp.Utility.Quote(ViewState.Item("Seq")))
                    strSQL.AppendLine(" and GradeSeq = " & Bsp.Utility.Quote(ViewState.Item("GradeSeq")) & ";")
                End If

                Bsp.DB.ExecuteNonQuery(CommandType.Text, strSQL.ToString(), tran, "eHRMSDB")

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
    Private Sub subDownloadSampleFile()

        Dim objGS1 As New GS1()

        Try

            '產出檔頭
            Dim strFileName As String = "GradeOrder_Upload.xlsx"
            Using dt As DataTable = objGS1.GS1200DownloadSample(ViewState.Item("CompID"), _
                ViewState.Item("ApplyID"), _
                ViewState.Item("ApplyTime"), _
                ViewState.Item("Seq"), _
                ViewState.Item("Status"), _
                ViewState.Item("MainFlag"), _
                ViewState.Item("GradeYear"), _
                ViewState.Item("GradeSeq"), _
                ViewState.Item("IsSignNext"), _
                ViewState.Item("DeptEX"), _
                    ViewState.Item("GroupID"))

                Using pck As New ExcelPackage(Util.getExcelOpenXml(dt, "GradeOrder_Upload"))
                    Dim ws As ExcelWorksheet = pck.Workbook.Worksheets("GradeOrder_Upload")
                    ws.Cells("A1:K1").Style.Font.Color.SetColor(System.Drawing.Color.Black)
                    'ws.Cells.Style.Border.Left.Style = ExcelBorderStyle.Thin
                    'ws.Cells.Style.Border.Right.Style = ExcelBorderStyle.Thin
                    'ws.Cells.Style.Border.Top.Style = ExcelBorderStyle.Thin
                    'ws.Cells.Style.Border.Bottom.Style = ExcelBorderStyle.Thin
                    ws.Cells.Style.ShrinkToFit = True

                    '修改指定 Cell 樣式
                    'ws.Cells("A1:C1").Style.Font.Color.SetColor(System.Drawing.Color.Red)
                    ws.Cells("I1:I" & Convert.ToInt32(dt.Rows.Count.ToString) + 1).Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow)
                    ws.Cells("J1:J" & Convert.ToInt32(dt.Rows.Count.ToString) + 1).Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow)
                    ws.Cells("K1:K" & Convert.ToInt32(dt.Rows.Count.ToString) + 1).Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow)

                    Dim val = ws.DataValidations.AddListValidation(ws.Cells("I1:I" & Convert.ToInt32(dt.Rows.Count.ToString) + 1).Address)
                    '设置下拉框显示的数据区域
                    val.Formula.Values.Add("特優")
                    val.Formula.Values.Add("優")
                    val.Formula.Values.Add("甲上")
                    val.Formula.Values.Add("甲")
                    val.Formula.Values.Add("甲下")
                    val.Formula.Values.Add("乙")
                    val.Formula.Values.Add("丙")
                    val.Error = "請輸入正確考績"
                    val.ShowErrorMessage = True


                    ''数据区域的名称
                    'val.Prompt = "下拉选择参数"
                    ''下拉提示
                    'val.ShowInputMessage = True
                    ''显示提示内容


                    '匯出
                    Util.ExportBinary(pck.GetAsByteArray(), strFileName)
                End Using
            End Using

            ''動態產生GridView以便匯出成EXCEL
            'Dim gvExport As GridView = New GridView()
            'gvExport.AllowPaging = False
            'gvExport.AllowSorting = False
            'gvExport.FooterStyle.BackColor = Drawing.ColorTranslator.FromHtml("#99CCCC")
            'gvExport.FooterStyle.ForeColor = Drawing.ColorTranslator.FromHtml("#003399")
            'gvExport.RowStyle.CssClass = "tr_evenline"
            'gvExport.AlternatingRowStyle.CssClass = "tr_oddline"
            'gvExport.EmptyDataRowStyle.CssClass = "GridView_EmptyRowStyle"

            'gvExport.DataSource = objGS1.GS1200DownloadSample(ViewState.Item("CompID"), _
            '    ViewState.Item("ApplyID"), _
            '    ViewState.Item("ApplyTime"), _
            '    ViewState.Item("Seq"), _
            '    ViewState.Item("Status"), _
            '    ViewState.Item("MainFlag"), _
            '    ViewState.Item("GradeYear"), _
            '    ViewState.Item("GradeSeq"), _
            '    ViewState.Item("IsSignNext"), _
            '    ViewState.Item("DeptEX"))
            'AddHandler gvExport.RowDataBound, AddressOf gvExport_RowDataBound   '20140103 wei add 增加自訂事件
            'gvExport.DataBind()

            'Response.ClearContent()
            'Response.BufferOutput = True
            'Response.Charset = "utf-8"
            'Response.ContentType = "application/save-as"         '隱藏檔案網址路逕的下載
            'Response.AddHeader("Content-Transfer-Encoding", "binary")
            'Response.ContentEncoding = System.Text.Encoding.UTF8
            'Response.AddHeader("content-disposition", "attachment; filename=" & Server.UrlPathEncode(strFileName))

            'Dim oStringWriter As New System.IO.StringWriter()
            'Dim oHtmlTextWriter As New System.Web.UI.HtmlTextWriter(oStringWriter)
            'Response.Write("<meta http-equiv=Content-Type content=text/html charset=utf-8>")
            'Dim style As String = "<style>td{font-size:9pt} a{font-size:9pt} tr{page-break-after: always}</style>"

            'gvExport.Attributes.Add("style", "vnd.ms-excel.numberformat:@")
            'gvExport.RenderControl(oHtmlTextWriter)
            'Response.Write(style)
            'Response.Write(oStringWriter.ToString())
            'Response.End()

        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & ".DoDownload", ex)
        End Try

    End Sub
    Protected Sub btnDownload_Click(sender As Object, e As System.EventArgs) Handles btnDownload.Click
        subDownloadSampleFile()
    End Sub
    Protected Sub gvExport_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        Select Case e.Row.RowType
            Case DataControlRowType.Header
                e.Row.Cells(8).BackColor = System.Drawing.Color.Yellow()
            Case DataControlRowType.DataRow
                e.Row.Cells(8).BackColor = System.Drawing.Color.Yellow
        End Select
    End Sub
End Class




