'****************************************************
'功能說明：員工待異動紀錄維護
'建立人員：Weicheng
'建立日期：2014/08/18
'****************************************************
Imports System.Data

Partial Class HR_HR3010
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then

        End If
    End Sub

    Public Overrides Sub DoAction(ByVal Param As String)
        ViewState.Item("Action") = ""
        Select Case Param
            Case "btnAdd"       '輸入調整資料
                DoAdd()
            Case "btnDelete"    '刪除
                DoDelete()
            Case "btnActionX"   '返回
                GoBack()
        End Select
    End Sub

    Protected Overrides Sub BaseOnPageTransfer(ByVal ti As TransferInfo)
        If Not IsPostBack() Then
            Dim objHR As New HR
            Dim ht As Hashtable = Bsp.Utility.getHashTableFromParam(ti.Args)

            If ht.ContainsKey("SelectCompRoleID") Then
                ViewState.Item("SelectCompRoleID") = ht("SelectCompRoleID").ToString()
            End If

            If ht.ContainsKey("SelectCompID") Then
                ViewState.Item("CompID") = ht("SelectCompID").ToString()
                lblCompRoleID.Text = ViewState.Item("CompID") + "-" + objHR.GetHRCompName(ViewState.Item("CompID")).Rows(0).Item("CompName").ToString
            End If

            If ht.ContainsKey("SelectEmpID") And ht.ContainsKey("SelectName") Then
                ViewState.Item("EmpID") = ht("SelectEmpID").ToString()
                ViewState.Item("Name") = ht("SelectName").ToString()
                lblName.Text = ht("SelectEmpID").ToString() & "-" & ht("SelectName").ToString()
            End If
            If ht.ContainsKey("SelectValidDate") Then
                ViewState.Item("ValidDate") = ht("SelectValidDate").ToString()
                lblMsg1.Text = "此員工待異動生效日『" & ht("SelectValidDate").ToString() & "』後,已有『已生效』的企業團經歷資料存在(如下)，請確認是否要調整資料？"
            End If
            If ht.ContainsKey("SelectSeq") Then
                ViewState.Item("SelectSeq") = ht("SelectSeq").ToString()
            End If

            If ht.ContainsKey("PageNo") Then
                ViewState.Item("PageNo") = ht.ContainsKey("PageNo")
            End If
            If ht.ContainsKey("DoQuery") Then
                If ht("DoQuery").ToString() = "Y" Then
                    ViewState.Item("DoQuery") = "Y"
                    DoQuery()
                End If
            End If
            If ht.ContainsKey("Detail") Then
                ViewState.Item("Detail") = ht("Detail").ToString()
            End If
        End If
    End Sub

    Private Sub DoAdd()
        If selectedRows(gvMain) <> "" Then
            Dim intSelectRow As Integer
            Dim intSelectCount As Integer = 0
            For intRow As Integer = 0 To gvMain.Rows.Count - 1
                Dim objChk As CheckBox = gvMain.Rows(intRow).FindControl("chk_gvMain")
                If objChk.Checked Then
                    intSelectRow = intRow
                    intSelectCount = intSelectCount + 1
                End If
            Next
            If intSelectCount = 1 Then
                Dim ti As TransferInfo = Me.StateTransfer

                Dim btnA As New ButtonState(ButtonState.emButtonType.Add)
                Dim btnX As New ButtonState(ButtonState.emButtonType.Exit)

                btnA.Caption = "存檔返回"
                btnX.Caption = "返回"

                Dim aryArg() As Object = Nothing
                ReDim aryArg(ti.Args.GetUpperBound(0) + 3)
                Dim tiCount As Integer = ti.Args.GetUpperBound(0)
                For i = 0 To ti.Args.Length - 1
                    aryArg(i) = ti.Args(i)
                Next
                aryArg(tiCount + 1) = "IDNo=" & gvMain.DataKeys(intSelectRow)("IDNo").ToString()
                aryArg(tiCount + 2) = "ModifyDate=" & gvMain.DataKeys(intSelectRow)("ModifyDate").ToString()
                aryArg(tiCount + 3) = "Reason=" & gvMain.DataKeys(intSelectRow)("Reason").ToString()

                Me.TransferFramePage("~/HR3/HR3011.aspx", New ButtonState() {btnA, btnX}, aryArg)

                'Me.TransferFramePage("~/HR3/HR3011.aspx", New ButtonState() {btnA, btnX}, _
                '                     "SelectCompRoleID=" & ViewState.Item("SelectCompRoleID"), _
                '                     "CompID=" & ViewState.Item("CompID"), _
                '                     "EmpID=" & ViewState.Item("EmpID"), _
                '                     "Name=" & ViewState.Item("Name"), _
                '                     "SelectValidDate=" & ViewState.Item("ValidDate"), _
                '                     "SelectSeq=" & ViewState.Item("SelectSeq"), _
                '                     "IDNo=" & gvMain.DataKeys(intSelectRow)("IDNo").ToString(), _
                '                     "ModifyDate=" & gvMain.DataKeys(intSelectRow)("ModifyDate").ToString(), _
                '                     "Reason=" & gvMain.DataKeys(intSelectRow)("Reason").ToString(), _
                '                     "PageNo=" & ViewState.Item("PageNo"), _
                '                     "DoQuery=" & Bsp.Utility.IsStringNull(ViewState.Item("DoQuery")))
            Else
                Bsp.Utility.ShowMessage(Me, "調整資料只能選擇一筆資料")
            End If
            
        End If
        
    End Sub

    Private Sub DoQuery()
        Dim objHR3000 As New HR3000()

        Try
            pcMain.DataTable = objHR3000.QueryEmployeeLogWait( _
                "CompID=" & ViewState.Item("CompID"), _
                "EmpID=" & ViewState.Item("EmpID"), _
                "ValidDate=" & ViewState.Item("ValidDate"), _
                "Seq=" & ViewState.Item("SelectSeq"))
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & ".DoQuery", ex)
        End Try

    End Sub

    Private Sub DoDelete()
        If selectedRows(gvMain) = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00000")
        Else
            Dim beEmployeeLogWait As New beEmployeeLogWait.Row()
            Dim objHR3000 As New HR3000
            Dim objHR As New HR


            Try
                For intRow As Integer = 0 To gvMain.Rows.Count - 1
                    Dim objChk As CheckBox = gvMain.Rows(intRow).FindControl("chk_gvMain")
                    If objChk.Checked Then
                        beEmployeeLogWait.CompID.Value = gvMain.DataKeys(intRow)("CompID").ToString()
                        beEmployeeLogWait.EmpID.Value = gvMain.DataKeys(intRow)("EmpID").ToString()
                        beEmployeeLogWait.Wait_ValidDate.Value = gvMain.DataKeys(intRow)("Wait_ValidDate").ToString()
                        beEmployeeLogWait.Wait_Seq.Value = gvMain.DataKeys(intRow)("Wait_Seq").ToString()
                        beEmployeeLogWait.IDNo.Value = gvMain.DataKeys(intRow)("IDNo").ToString()
                        beEmployeeLogWait.ModifyDate.Value = gvMain.DataKeys(intRow)("ModifyDate").ToString()
                        beEmployeeLogWait.Reason.Value = gvMain.DataKeys(intRow)("Reason").ToString()
                        objHR3000.DeleteEmployeeLogWait(beEmployeeLogWait)

                    End If
                Next
                Bsp.Utility.ShowMessage(Me, "資料刪除成功")
            Catch ex As Exception
                Bsp.Utility.ShowMessage(Me, Me.FunID & ".DoDelete", ex)
            End Try
            DoQuery()
        End If
    End Sub
    
    Private Sub GoBack()
        Dim ti As TransferInfo = Me.StateTransfer
        Dim btnA As New ButtonState(ButtonState.emButtonType.Add)
        Dim btnX As New ButtonState(ButtonState.emButtonType.Exit)
        Dim btnC As New ButtonState(ButtonState.emButtonType.Cancel)

        If ViewState.Item("Detail") = "Y" Then
            btnX.Caption = "返回"
            Me.TransferFramePage("~/HR3/HR3002.aspx", New ButtonState() {btnX}, ti.Args)
            'Me.TransferFramePage("~/HR3/HR3002.aspx", New ButtonState() {btnA, btnX, btnC}, _
            '                     "SelectCompRoleID=" & ViewState.Item("SelectCompRoleID"), _
            '                     "SelectCompID=" & ViewState.Item("CompID"), _
            '                     "SelectEmpID=" & ViewState.Item("EmpID"), _
            '                     "SelectName=" & ViewState.Item("Name"), _
            '                     "SelectValidDate=" & ViewState.Item("ValidDate"), _
            '                     "SelectSeq=" & ViewState.Item("SelectSeq"), _
            '                     "PageNo=" & ViewState.Item("PageNo"), _
            '                     "DoDetail=Y", _
            '                     "DoQuery=Y")
        Else
            btnA.Caption = "存檔返回"
            btnX.Caption = "返回"
            btnC.Caption = "清除"
            Me.TransferFramePage("~/HR3/HR3002.aspx", New ButtonState() {btnA, btnX, btnC}, ti.Args)
            'Me.TransferFramePage("~/HR3/HR3002.aspx", New ButtonState() {btnA, btnX, btnC}, _
            '                     "SelectCompRoleID=" & ViewState.Item("SelectCompRoleID"), _
            '                     "SelectCompID=" & ViewState.Item("CompID"), _
            '                     "SelectEmpID=" & ViewState.Item("EmpID"), _
            '                     "SelectName=" & ViewState.Item("Name"), _
            '                     "SelectValidDate=" & ViewState.Item("ValidDate"), _
            '                     "SelectSeq=" & ViewState.Item("SelectSeq"), _
            '                     "PageNo=" & ViewState.Item("PageNo"), _
            '                     "DoDetail=N", _
            '                     "DoQuery=Y")
        End If
        
    End Sub

    Protected Sub gvMain_RowCreated(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs)
        Select Case e.Row.RowType
            Case DataControlRowType.Header
                'Dim tcRow As New GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Normal)
                'Dim tcHeader1 As New TableCell
                'tcHeader1.ColumnSpan = 8
                'tcHeader1.Text = ""
                'tcHeader1.CssClass = "td_header"
                'tcRow.Cells.Add(tcHeader1)

                'Dim tcHeader2 As New TableCell
                'tcHeader2.ColumnSpan = 1
                'tcHeader2.Text = ""
                'tcHeader2.CssClass = "td_header"
                'tcRow.Cells.Add(tcHeader2)

                'Dim tcHeader3 As New TableCell
                'tcHeader3.ColumnSpan = 1
                'tcHeader3.Text = ""
                'tcHeader3.CssClass = "td_header"
                'tcRow.Cells.Add(tcHeader3)

                'Dim tcHeader4 As New TableCell
                'tcHeader4.ColumnSpan = 1
                'tcHeader4.Text = ""
                'tcHeader4.CssClass = "td_header"
                'tcRow.Cells.Add(tcHeader4)

                'Dim tcHeader5 As New TableCell
                'tcHeader5.ColumnSpan = 1
                'tcHeader5.Text = ""
                'tcHeader5.CssClass = "td_header"
                'tcRow.Cells.Add(tcHeader5)

                'Dim tcHeader6 As New TableCell
                'tcHeader6.ColumnSpan = 1
                'tcHeader6.Text = ""
                'tcHeader6.CssClass = "td_header"
                'tcRow.Cells.Add(tcHeader6)

                'Dim tcHeader7 As New TableCell
                'tcHeader7.ColumnSpan = 1
                'tcHeader7.Text = ""
                'tcHeader7.CssClass = "td_header"
                'tcRow.Cells.Add(tcHeader7)

                'Dim tcHeader8 As New TableCell
                'tcHeader8.ColumnSpan = 1
                'tcHeader8.Text = ""
                'tcHeader8.CssClass = "td_header"
                'tcRow.Cells.Add(tcHeader8)

                'Dim tcHeader9 As New TableCell
                'tcHeader9.ColumnSpan = 4
                'tcHeader9.Text = "異動後資料"
                'tcHeader9.CssClass = "td_header"
                ''tcHeader9.BackColor = Drawing.Color.LightYellow

                'tcRow.Cells.Add(tcHeader9)

                'gvMain.Controls(0).Controls.AddAt(0, tcRow)

                ''取得該GridView的表頭()
                'Dim tcHeader As TableCellCollection = e.Row.Cells
                ''清除先前設定的表頭  
                'tcHeader.Clear()

                ''新增第一層表頭()
                'tcHeader.Add(New TableHeaderCell())
                'tcHeader(0).Attributes.Add("rowspan", "2")
                'tcHeader(0).Attributes.Add("Class", "td_header")
                ''該表頭所要顯示的內容
                ''功用是用來第一個表頭的結尾  做為下一行的開始  
                ''若未在表頭內容加上就會看到下一層表頭"資料"出現在"全部資訊"後方  
                'tcHeader(0).Text = "&nbsp;"

                'tcHeader.Add(New TableHeaderCell())
                'tcHeader(1).Attributes.Add("rowspan", "2")
                'tcHeader(1).Attributes.Add("Class", "td_header")
                'tcHeader(1).Text = "明細"

                'tcHeader.Add(New TableHeaderCell())
                'tcHeader(2).Attributes.Add("rowspan", "2")
                'tcHeader(2).Attributes.Add("Class", "td_header")
                'tcHeader(2).Text = "生效"

                'tcHeader.Add(New TableHeaderCell())
                'tcHeader(3).Attributes.Add("rowspan", "2")
                'tcHeader(3).Attributes.Add("Class", "td_header")
                'tcHeader(3).Text = "公司名稱"

                'tcHeader.Add(New TableHeaderCell())
                'tcHeader(4).Attributes.Add("rowspan", "2")
                'tcHeader(4).Attributes.Add("Class", "td_header")
                'tcHeader(4).Text = "員工編號"

                'tcHeader.Add(New TableHeaderCell())
                'tcHeader(5).Attributes.Add("rowspan", "2")
                'tcHeader(5).Attributes.Add("Class", "td_header")
                'tcHeader(5).Text = "姓名"

                'tcHeader.Add(New TableHeaderCell())
                'tcHeader(6).Attributes.Add("rowspan", "2")
                'tcHeader(6).Attributes.Add("Class", "td_header")
                'tcHeader(6).Text = "生效日期"

                'tcHeader.Add(New TableHeaderCell())
                'tcHeader(7).Attributes.Add("rowspan", "2")
                'tcHeader(7).Attributes.Add("Class", "td_header")
                'tcHeader(7).Text = "異動原因"

                'tcHeader.Add(New TableHeaderCell())
                'tcHeader(8).Attributes.Add("colspan", "4")
                'tcHeader(8).Attributes.Add("Class", "td_header")
                'tcHeader(8).Text = "異動後資料</tr><tr>"

                'tcHeader.Add(New TableHeaderCell())
                'tcHeader(9).Attributes.Add("Class", "td_header")
                'tcHeader(9).Text = "公司名稱"

                'tcHeader.Add(New TableHeaderCell())
                'tcHeader(10).Attributes.Add("Class", "td_header")
                'tcHeader(10).Text = "事業群"

                'tcHeader.Add(New TableHeaderCell())
                'tcHeader(11).Attributes.Add("Class", "td_header")
                'tcHeader(11).Text = "部門"

                'tcHeader.Add(New TableHeaderCell())
                'tcHeader(12).Attributes.Add("Class", "td_header")
                'tcHeader(12).Text = "科組課</tr>"

        End Select
    End Sub

    Protected Sub gvMain_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvMain.RowCommand
        If e.CommandName = "Detail" Then
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            Dim btnX As New ButtonState(ButtonState.emButtonType.Exit)

            Dim ti As TransferInfo = Me.StateTransfer

            btnX.Caption = "返回"

            Dim aryArg() As Object = Nothing
            ReDim aryArg(ti.Args.GetUpperBound(0) + 3)
            Dim tiCount As Integer = ti.Args.GetUpperBound(0)
            For i = 0 To ti.Args.Length - 1
                aryArg(i) = ti.Args(i)
            Next
            aryArg(tiCount + 1) = "IDNo=" & gvMain.DataKeys(index)("IDNo").ToString()
            aryArg(tiCount + 2) = "ModifyDate=" & gvMain.DataKeys(index)("ModifyDate").ToString()
            aryArg(tiCount + 3) = "Reason=" & gvMain.DataKeys(index)("Reason").ToString()

            Me.TransferFramePage("~/HR3/HR3011.aspx", New ButtonState() {btnX}, aryArg)

            'Me.TransferFramePage("~/HR3/HR3011.aspx", New ButtonState() {btnX}, _
            '                         "SelectCompRoleID=" & ViewState.Item("SelectCompRoleID"), _
            '                         "CompID=" & ViewState.Item("CompID"), _
            '                         "EmpID=" & ViewState.Item("EmpID"), _
            '                         "Name=" & ViewState.Item("Name"), _
            '                         "SelectValidDate=" & ViewState.Item("ValidDate"), _
            '                         "SelectSeq=" & ViewState.Item("SelectSeq"), _
            '                         "IDNo=" & gvMain.DataKeys(index)("IDNo").ToString(), _
            '                         "ModifyDate=" & gvMain.DataKeys(index)("ModifyDate").ToString(), _
            '                         "Reason=" & gvMain.DataKeys(index)("Reason").ToString(), _
            '                         "PageNo=" & ViewState.Item("PageNo"), _
            '                         "DoQuery=" & Bsp.Utility.IsStringNull(ViewState.Item("DoQuery")))

        End If
    End Sub
End Class
