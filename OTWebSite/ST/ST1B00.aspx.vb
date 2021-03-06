'****************************************************
'功能說明：員工待異動紀錄維護
'建立人員：Weicheng
'建立日期：2014/08/18
'****************************************************
Imports System.Data

Partial Class ST_ST1B00
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim objSC As New SC
            lblCompRoleID.Text = UserProfile.SelectCompRoleName '20150527 wei modify UserProfile.SelectCompRoleID + "-" + objSC.GetCompName(UserProfile.SelectCompRoleID).Rows(0).Item("CompName").ToString


        End If
    End Sub

    Public Overrides Sub DoAction(ByVal Param As String)
        ViewState.Item("Action") = ""
        Select Case Param
            Case "btnAdd"       '新增
                DoAdd()
            Case "btnUpdate"    '修改
                DoUpdate()
            Case "btnQuery"     '查詢
                ViewState.Item("DoQuery") = "Y"
                DoQuery()
            Case "btnDelete"    '刪除
                DoDelete()
            Case "btnDownload"  '下傳
                DoDownload()
        End Select
    End Sub

    Protected Overrides Sub BaseOnPageTransfer(ByVal ti As TransferInfo)
        If Not IsPostBack() Then
            Dim ht As Hashtable = Bsp.Utility.getHashTableFromParam(ti.Args)
            If ht.ContainsKey("SelectedCompID") Then
                ViewState.Item("CompID") = ht("SelectedCompID").ToString()
                ViewState.Item("CompName") = ht("SelectedCompName").ToString()
                ViewState.Item("EmpID") = ht("SelectedEmpID").ToString()
                ViewState.Item("EmpName") = ht("SelectedEmpName").ToString()
                ViewState.Item("IDNo") = ht("SelectedIDNo").ToString()

                lblCompRoleID.Text = ViewState.Item("CompID").ToString + "-" + ViewState.Item("CompName")
                txtEmpID.Text = ViewState.Item("EmpID").ToString
                txtEmpName.Text = ViewState.Item("EmpName").ToString
            Else
                Return
            End If
            DoQuery()
        End If
    End Sub

    Private Sub DoAdd()
        Dim objHR3000 As New HR3000
        

        Dim objSC As New SC
        Dim btnA As New ButtonState(ButtonState.emButtonType.Add)
        Dim btnX As New ButtonState(ButtonState.emButtonType.Exit)
        Dim btnC As New ButtonState(ButtonState.emButtonType.Cancel)

        btnA.Caption = "存檔返回"
        btnX.Caption = "返回"
        btnC.Caption = "清除"


        Dim strCompID As String
        If UserProfile.SelectCompRoleID = "ALL" Then
            strCompID = ViewState.Item("CompID")
        Else
            strCompID = UserProfile.SelectCompRoleID
        End If

        Using dt As DataTable = objHR3000.GetEmpDataByHR3000(strCompID, ViewState.Item("EmpID"))
            If dt.Rows.Count > 0 Then
                If dt.Rows(0)("WorkStatus") <> "1" Then
                    Bsp.Utility.ShowFormatMessage(Me, "該員工不在職，不可新增調兼資料")
                    Return
                End If
            End If
        End Using

        Me.TransferFramePage("~/ST/ST1B01.aspx", New ButtonState() {btnA, btnX, btnC}, _
                             rbEmpAddition.ID & "=" & CStr(rbEmpAddition.Checked), _
                             rbEmpAdditionDetail.ID & "=" & CStr(rbEmpAdditionDetail.Checked), _
                             lblCompRoleID.ID & "=" & UserProfile.SelectCompRoleID, _
                             txtEmpID.ID & "=" & txtEmpID.Text, _
                             txtEmpName.ID & "=" & txtEmpName.Text, _
                             "SelectCompRoleID=" & strCompID, _
                             "SelectedCompID=" & strCompID, _
                             "SelectedCompName=" & objSC.GetCompName(UserProfile.SelectCompRoleID).Rows(0).Item("CompName").ToString, _
                             "SelectedEmpName=" & txtEmpName.Text, _
                             "SelectedIDNo=" & ViewState.Item("IDNo"), _
                             "SelectedEmpID=" & ViewState.Item("EmpID"), _
                             "PageNo=" & pcMain.PageNo.ToString(), _
                             "DoQuery=" & Bsp.Utility.IsStringNull(ViewState.Item("DoQuery")))
    End Sub

    Private Sub DoUpdate()

        If rbEmpAddition.Checked Then
            Bsp.Utility.ShowFormatMessage(Me, "員工調兼現況不可修改，請選擇員工調兼資料")
            Return
        End If
        
        If selectedRow(gvMain) >= 0 Then
            Dim objSC As New SC
            Dim btnA As New ButtonState(ButtonState.emButtonType.Add)
            Dim btnX As New ButtonState(ButtonState.emButtonType.Exit)
            Dim btnC As New ButtonState(ButtonState.emButtonType.Cancel)

            btnA.Caption = "存檔返回"
            btnX.Caption = "返回"
            btnC.Caption = "清除"

            Dim strCompID As String
            If UserProfile.SelectCompRoleID = "ALL" Then
                strCompID = ViewState.Item("CompID")
            Else
                strCompID = UserProfile.SelectCompRoleID
            End If

            Me.TransferFramePage("~/ST/ST1B02.aspx", New ButtonState() {btnA, btnX, btnC}, _
                                 rbEmpAddition.ID & "=" & CStr(rbEmpAddition.Checked), _
                                 rbEmpAdditionDetail.ID & "=" & CStr(rbEmpAdditionDetail.Checked), _
                                 lblCompRoleID.ID & "=" & UserProfile.SelectCompRoleID, _
                                 txtEmpID.ID & "=" & txtEmpID.Text, _
                                 txtEmpName.ID & "=" & txtEmpName.Text, _
                                 "SelectCompRoleID=" & strCompID, _
                                 "SelectedCompID=" & gvMain.DataKeys(Me.selectedRow(gvMain))("CompID").ToString(), _
                                 "SelectedEmpID=" & gvMain.DataKeys(Me.selectedRow(gvMain))("EmpID").ToString(), _
                                 "SelectValidDate=" & gvMain.DataKeys(Me.selectedRow(gvMain))("ValidDate").ToString(), _
                                 "SelectAddCompID=" & gvMain.DataKeys(Me.selectedRow(gvMain))("AddCompID").ToString(), _
                                 "SelectAddDeptID=" & gvMain.DataKeys(Me.selectedRow(gvMain))("AddDeptID").ToString(), _
                                 "SelectAddOrganID=" & gvMain.DataKeys(Me.selectedRow(gvMain))("AddOrganID").ToString(), _
                                 "SelectedCompName=" & objSC.GetCompName(UserProfile.SelectCompRoleID).Rows(0).Item("CompName").ToString, _
                                 "SelectedEmpName=" & txtEmpName.Text, _
                                 "SelectedIDNo=" & ViewState.Item("IDNo"), _
                                 "PageNo=" & pcMain.PageNo.ToString(), _
                                 "DoQuery=" & Bsp.Utility.IsStringNull(ViewState.Item("DoQuery")))


        End If
    End Sub

    Private Sub DoQuery()
        Dim objHR1600 As New HR1600()
        Dim strCompID As String
        If UserProfile.SelectCompRoleID = "ALL" Then
            strCompID = ViewState.Item("CompID")
        Else
            strCompID = UserProfile.SelectCompRoleID
        End If

        Try
            gvMain.Visible = True
            If rbEmpAddition.Checked Then
                pcMain.DataTable = objHR1600.QueryEmpAddition( _
                "CompID=" & strCompID, _
                "EmpID=True;" & txtEmpID.Text.Trim())
            End If

            If rbEmpAdditionDetail.Checked Then
                pcMain.DataTable = objHR1600.QueryEmpAdditionDetail( _
                "CompID=" & strCompID, _
                "EmpID=True;" & txtEmpID.Text.Trim())
            End If

        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & ".DoQuery", ex)
        End Try


    End Sub

    Private Sub DoDelete()
        If rbEmpAddition.Checked Then
            Bsp.Utility.ShowFormatMessage(Me, "員工調兼現況不可直接刪除，請選擇員工調兼資料")
            Return
        End If
        If selectedRow(gvMain) < 0 Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00000")
        Else
            Dim beEmpAddition As New beEmpAddition.Row()
            Dim beEmpAdditionDetail As New beEmpAdditionDetail.Row()
            Dim beEmpAdditionLog As New beEmpAdditionLog.Row()
            Dim objHR1600 As New HR1600

            'EmpAdditionDetail
            beEmpAdditionDetail.ValidDate.Value = gvMain.DataKeys(Me.selectedRow(gvMain))("ValidDate").ToString()
            beEmpAdditionDetail.CompID.Value = gvMain.DataKeys(Me.selectedRow(gvMain))("CompID").ToString()
            beEmpAdditionDetail.EmpID.Value = gvMain.DataKeys(Me.selectedRow(gvMain))("EmpID").ToString()
            beEmpAdditionDetail.AddCompID.Value = gvMain.DataKeys(Me.selectedRow(gvMain))("AddCompID").ToString()
            beEmpAdditionDetail.AddDeptID.Value = gvMain.DataKeys(Me.selectedRow(gvMain))("AddDeptID").ToString()
            beEmpAdditionDetail.AddOrganID.Value = gvMain.DataKeys(Me.selectedRow(gvMain))("AddOrganID").ToString()

            'EmpAddition
            beEmpAddition.ValidDate.Value = gvMain.DataKeys(Me.selectedRow(gvMain))("ValidDate").ToString()
            beEmpAddition.CompID.Value = gvMain.DataKeys(Me.selectedRow(gvMain))("CompID").ToString()
            beEmpAddition.EmpID.Value = gvMain.DataKeys(Me.selectedRow(gvMain))("EmpID").ToString()
            beEmpAddition.AddCompID.Value = gvMain.DataKeys(Me.selectedRow(gvMain))("AddCompID").ToString()
            beEmpAddition.AddDeptID.Value = gvMain.DataKeys(Me.selectedRow(gvMain))("AddDeptID").ToString()
            beEmpAddition.AddOrganID.Value = gvMain.DataKeys(Me.selectedRow(gvMain))("AddOrganID").ToString()

            'EmpAdditionLog
            beEmpAdditionLog.ValidDate.Value = gvMain.DataKeys(Me.selectedRow(gvMain))("ValidDate").ToString()
            beEmpAdditionLog.CompID.Value = gvMain.DataKeys(Me.selectedRow(gvMain))("CompID").ToString()
            beEmpAdditionLog.EmpID.Value = gvMain.DataKeys(Me.selectedRow(gvMain))("EmpID").ToString()
            beEmpAdditionLog.AddCompID.Value = gvMain.DataKeys(Me.selectedRow(gvMain))("AddCompID").ToString()
            beEmpAdditionLog.AddDeptID.Value = gvMain.DataKeys(Me.selectedRow(gvMain))("AddDeptID").ToString()
            beEmpAdditionLog.AddOrganID.Value = gvMain.DataKeys(Me.selectedRow(gvMain))("AddOrganID").ToString()

            Try
                objHR1600.DeleteEmpAddition(beEmpAddition, beEmpAdditionDetail, beEmpAdditionLog)
            Catch ex As Exception
                Bsp.Utility.ShowMessage(Me, Me.FunID & ".DoDelete", ex)
            End Try
            DoQuery()
        End If

    End Sub


    Public Overrides Sub DoModalReturn(ByVal returnValue As String)

    End Sub
    

    Protected Sub gvMain_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvMain.RowCommand
        If e.CommandName = "Detail" Then
            Dim btnX As New ButtonState(ButtonState.emButtonType.Exit)
            Dim objSC As New SC
            btnX.Caption = "返回"

            Dim strCompID As String
            If UserProfile.SelectCompRoleID = "ALL" Then
                strCompID = ViewState.Item("CompID")
            Else
                strCompID = UserProfile.SelectCompRoleID
            End If

            If rbEmpAddition.Checked Then
                Me.TransferFramePage("~/ST/ST1B12.aspx", New ButtonState() {btnX}, _
                                 rbEmpAddition.ID & "=" & CStr(rbEmpAddition.Checked), _
                                 rbEmpAdditionDetail.ID & "=" & CStr(rbEmpAdditionDetail.Checked), _
                                 lblCompRoleID.ID & "=" & UserProfile.SelectCompRoleID, _
                                 txtEmpID.ID & "=" & txtEmpID.Text, _
                                 txtEmpName.ID & "=" & txtEmpName.Text, _
                                 "SelectCompRoleID=" & strCompID, _
                                 "SelectedCompID=" & gvMain.DataKeys(Me.selectedRow(gvMain))("CompID").ToString(), _
                                 "SelectedEmpID=" & gvMain.DataKeys(Me.selectedRow(gvMain))("EmpID").ToString(), _
                                 "SelectValidDate=" & gvMain.DataKeys(Me.selectedRow(gvMain))("ValidDate").ToString(), _
                                 "SelectAddCompID=" & gvMain.DataKeys(Me.selectedRow(gvMain))("AddCompID").ToString(), _
                                 "SelectAddDeptID=" & gvMain.DataKeys(Me.selectedRow(gvMain))("AddDeptID").ToString(), _
                                 "SelectAddOrganID=" & gvMain.DataKeys(Me.selectedRow(gvMain))("AddOrganID").ToString(), _
                                 "SelectedCompName=" & objSC.GetCompName(gvMain.DataKeys(Me.selectedRow(gvMain))("CompID").ToString()).Rows(0).Item("CompName").ToString, _
                                 "SelectedEmpName=" & txtEmpName.Text, _
                                 "SelectedIDNo=" & ViewState.Item("IDNo"), _
                                 "PageNo=" & pcMain.PageNo.ToString(), _
                                 "DoQuery=" & Bsp.Utility.IsStringNull(ViewState.Item("DoQuery")))
            End If
            If rbEmpAdditionDetail.Checked Then
                Me.TransferFramePage("~/ST/ST1B02.aspx", New ButtonState() {btnX}, _
                                 rbEmpAddition.ID & "=" & CStr(rbEmpAddition.Checked), _
                                 rbEmpAdditionDetail.ID & "=" & CStr(rbEmpAdditionDetail.Checked), _
                                 lblCompRoleID.ID & "=" & UserProfile.SelectCompRoleID, _
                                 txtEmpID.ID & "=" & txtEmpID.Text, _
                                 txtEmpName.ID & "=" & txtEmpName.Text, _
                                 "SelectCompRoleID=" & strCompID, _
                                 "SelectedCompID=" & gvMain.DataKeys(Me.selectedRow(gvMain))("CompID").ToString(), _
                                 "SelectedEmpID=" & gvMain.DataKeys(Me.selectedRow(gvMain))("EmpID").ToString(), _
                                 "SelectValidDate=" & gvMain.DataKeys(Me.selectedRow(gvMain))("ValidDate").ToString(), _
                                 "SelectAddCompID=" & gvMain.DataKeys(Me.selectedRow(gvMain))("AddCompID").ToString(), _
                                 "SelectAddDeptID=" & gvMain.DataKeys(Me.selectedRow(gvMain))("AddDeptID").ToString(), _
                                 "SelectAddOrganID=" & gvMain.DataKeys(Me.selectedRow(gvMain))("AddOrganID").ToString(), _
                                 "SelectedCompName=" & objSC.GetCompName(gvMain.DataKeys(Me.selectedRow(gvMain))("CompID").ToString()).Rows(0).Item("CompName").ToString, _
                                 "SelectedEmpName=" & txtEmpName.Text, _
                                 "SelectedIDNo=" & ViewState.Item("IDNo"), _
                                 "PageNo=" & pcMain.PageNo.ToString(), _
                                 "DoQuery=" & Bsp.Utility.IsStringNull(ViewState.Item("DoQuery")))
            End If



        End If
    End Sub
    Private Sub DoDownload()
        Try

            Dim strCompID As String = ""
            Dim objHR1600 As New HR1600
            If pcMain.DataTable.Rows.Count > 0 Then
                '產出檔頭
                Dim strFileName As String = ""


                '動態產生GridView以便匯出成EXCEL
                Dim gvExport As GridView = New GridView()
                gvExport.AllowPaging = False
                gvExport.AllowSorting = False
                gvExport.FooterStyle.BackColor = Drawing.ColorTranslator.FromHtml("#99CCCC")
                gvExport.FooterStyle.ForeColor = Drawing.ColorTranslator.FromHtml("#003399")
                gvExport.RowStyle.CssClass = "tr_evenline"
                gvExport.AlternatingRowStyle.CssClass = "tr_oddline"
                gvExport.EmptyDataRowStyle.CssClass = "GridView_EmptyRowStyle"

                'gvExport = gvMain  '寫GridViewName-gvMain，會變成下載GridView畫面，會出下換頁CSS等怪東西
                If UserProfile.SelectCompRoleID = "ALL" Then
                    strCompID = ViewState.Item("CompID")
                Else
                    strCompID = UserProfile.SelectCompRoleID
                End If

                If rbEmpAddition.Checked Then
                    strFileName = Bsp.Utility.GetNewFileName("調兼現況資料下載-") & ".xls"
                    gvExport.DataSource = objHR1600.QueryEmpAdditionByDownload( _
                    "CompID=" & strCompID, _
                    "EmpID=True;" & txtEmpID.Text.Trim())
                End If
                If rbEmpAdditionDetail.Checked Then
                    strFileName = Bsp.Utility.GetNewFileName("調兼資料下載-") & ".xls"
                    gvExport.DataSource = objHR1600.QueryEmpAdditionDetailByDownload( _
                    "CompID=" & strCompID, _
                    "EmpID=True;" & txtEmpID.Text.Trim())
                End If

                'AddHandler gvExport.RowDataBound, AddressOf gvExport_RowDataBound   '20140103 wei add 增加自訂事件
                gvExport.DataBind()

                Response.ClearContent()
                Response.BufferOutput = True
                Response.Charset = "utf-8"
                ''Response.ContentType = "application/ms-excel"      '只寫ms-excel不OK，會變成程式碼下載@@
                'Response.ContentType = "application/vnd.ms-excel"
                Response.ContentType = "application/save-as"         '隱藏檔案網址路逕的下載
                Response.AddHeader("Content-Transfer-Encoding", "binary")
                Response.ContentEncoding = System.Text.Encoding.UTF8
                Response.AddHeader("content-disposition", "attachment; filename=" & Server.UrlPathEncode(strFileName))

                Dim oStringWriter As New System.IO.StringWriter()
                Dim oHtmlTextWriter As New System.Web.UI.HtmlTextWriter(oStringWriter)

                Response.Write("<meta http-equiv=Content-Type content=text/html charset=utf-8>")
                Dim style As String = "<style>td{font-size:9pt} a{font-size:9pt} tr{page-break-after: always}</style>"

                gvExport.Attributes.Add("style", "vnd.ms-excel.numberformat:@")
                gvExport.RenderControl(oHtmlTextWriter)
                Response.Write(style)
                Response.Write(oStringWriter.ToString())
                Response.End()
            Else
                Bsp.Utility.ShowFormatMessage(Me, "請先查詢有資料，才能下傳!")
            End If

        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & ".DoDownload", ex)
        End Try

    End Sub
    Protected Sub gvExport_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)

        Select Case e.Row.RowType
            Case DataControlRowType.Header

                ''取得該GridView的表頭  
                'Dim tcHeader As TableCellCollection = e.Row.Cells
                ''清除先前設定的表頭  
                'tcHeader.Clear()

                ''新增第一層表頭()
                'tcHeader.Add(New TableHeaderCell())
                'tcHeader(0).Attributes.Add("colspan", "10")
                ''該表頭所要顯示的內容
                ''功用是用來第一個表頭的結尾  做為下一行的開始  
                ''若未在表頭內容加上就會看到下一層表頭"資料"出現在"全部資訊"後方  
                'tcHeader(0).Text = "招募晉用狀況報表-" & txtCheckInDateS.Text.Trim() & "~" & txtCheckInDateE.Text.Trim()

                'tcHeader.Add(New TableHeaderCell())
                'tcHeader(1).Attributes.Add("colspan", "4")
                'tcHeader(1).Text = "來源/招募方式</tr><tr>"

                'tcHeader.Add(New TableHeaderCell())
                'tcHeader(2).Attributes.Add("rowspan", "2")
                'tcHeader(2).Text = "3P"

                'tcHeader.Add(New TableHeaderCell())
                'tcHeader(3).Attributes.Add("rowspan", "2")
                'tcHeader(3).Text = "職位"

                'tcHeader.Add(New TableHeaderCell())
                'tcHeader(4).Attributes.Add("rowspan", "2")
                'tcHeader(4).Text = "收件數"

                'tcHeader.Add(New TableHeaderCell())
                'tcHeader(5).Attributes.Add("rowspan", "2")
                'tcHeader(5).Text = "轉核薪"

                'tcHeader.Add(New TableHeaderCell())
                'tcHeader(6).Text = "&nbsp;"

                'tcHeader.Add(New TableHeaderCell())
                'tcHeader(7).Text = "&nbsp;"

                'tcHeader.Add(New TableHeaderCell())
                'tcHeader(8).Text = "&nbsp;"

                'tcHeader.Add(New TableHeaderCell())
                'tcHeader(9).Text = "&nbsp;"

                'tcHeader.Add(New TableHeaderCell())
                'tcHeader(10).Text = "&nbsp;"

                'tcHeader.Add(New TableHeaderCell())
                'tcHeader(11).Text = "&nbsp;"

                'tcHeader.Add(New TableHeaderCell())
                'tcHeader(12).Attributes.Add("rowspan", "2")
                'tcHeader(12).Attributes.Add("bgcolor", "#FFFF00")
                'tcHeader(12).Text = "HR轉介"

                'tcHeader.Add(New TableHeaderCell())
                'tcHeader(13).Attributes.Add("rowspan", "2")
                'tcHeader(13).Attributes.Add("bgcolor", "#FFFF00")
                'tcHeader(13).Text = "統召"

                'tcHeader.Add(New TableHeaderCell())
                'tcHeader(14).Attributes.Add("rowspan", "2")
                'tcHeader(14).Attributes.Add("bgcolor", "#FFFF00")
                'tcHeader(14).Text = "自召"

                'tcHeader.Add(New TableHeaderCell())
                'tcHeader(15).Attributes.Add("rowspan", "2")
                'tcHeader(15).Attributes.Add("bgcolor", "#FFFF00")
                'tcHeader(15).Text = "其他</tr><tr>"

                'tcHeader.Add(New TableHeaderCell())
                'tcHeader(16).Text = "暫緩"

                'tcHeader.Add(New TableHeaderCell())
                'tcHeader(17).Text = "核薪中"

                'tcHeader.Add(New TableHeaderCell())
                'tcHeader(18).Text = "拒絶"

                'tcHeader.Add(New TableHeaderCell())
                'tcHeader(19).Text = "待報到"

                'tcHeader.Add(New TableHeaderCell())
                'tcHeader(20).Text = "待報後拒絶"

                'tcHeader.Add(New TableHeaderCell())
                'tcHeader(21).Text = "已報到</tr>"
                'e.Row.Cells(7).Visible = False
            Case DataControlRowType.DataRow
                'e.Row.Cells(7).Visible = False
                'e.Row.Cells(20).Text = GetPosition(DataBinder.Eval(e.Row.DataItem, "NewCompID").ToString(), DataBinder.Eval(e.Row.DataItem, "主要職位").ToString(), True)
                'e.Row.Cells(21).Text = GetPosition(DataBinder.Eval(e.Row.DataItem, "NewCompID").ToString(), DataBinder.Eval(e.Row.DataItem, "主要職位").ToString(), False)
                'e.Row.Cells(22).Text = GetWorkType(DataBinder.Eval(e.Row.DataItem, "NewCompID").ToString(), DataBinder.Eval(e.Row.DataItem, "主要工作性質").ToString(), True)
                'e.Row.Cells(23).Text = GetWorkType(DataBinder.Eval(e.Row.DataItem, "NewCompID").ToString(), DataBinder.Eval(e.Row.DataItem, "主要工作性質").ToString(), False)

        End Select
    End Sub
    Public Sub GroupRows(ByVal GridView1 As GridView, ByVal cellNum As Integer, ByVal comparCellNum As Integer)
        Dim i As Integer = 0
        Dim rowSpanNum As Integer = 1
        While i < GridView1.Rows.Count - 1
            Dim gvr As GridViewRow = GridView1.Rows(i)
            For i = i + 1 To GridView1.Rows.Count - 1
                Dim gvrNext As GridViewRow = GridView1.Rows(i)
                If gvr.Cells(comparCellNum).Text = gvrNext.Cells(comparCellNum).Text Then
                    gvrNext.Cells(cellNum).Visible = False
                    rowSpanNum = rowSpanNum + 1
                Else
                    gvr.Cells(cellNum).RowSpan = rowSpanNum
                    rowSpanNum = 1
                    Exit For
                End If
                If i = GridView1.Rows.Count - 1 Then
                    gvr.Cells(cellNum).RowSpan = rowSpanNum
                End If
            Next
            gvr.Cells(cellNum).BackColor = Drawing.Color.White
        End While
    End Sub
End Class
