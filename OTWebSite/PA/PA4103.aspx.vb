'****************************************************
'功能說明：Web人員資料查詢設定
'建立人員：BeatriceCheng
'建立日期：2015.08.11
'****************************************************
Imports System.Data
Imports Newtonsoft.Json

Partial Class PA_PA4103
    Inherits PageBase


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then
            Dim objSC As New SC
            Dim objPA4 As New PA4

            txtComID.Text = UserProfile.SelectCompRoleName
            ucSelectEmpID.ShowCompRole = False
            ucSelectEmpID.SelectCompID = UserProfile.SelectCompRoleID

            subLoadDropDownList()
        End If
    End Sub
    Protected Overrides Sub BaseOnPageTransfer(ByVal ti As TransferInfo)
        If Not IsPostBack() Then
            Dim ht As Hashtable = Bsp.Utility.getHashTableFromParam(ti.Args)
            ViewState.Item("PageNo") = ht("PageNo").ToString()
            ViewState.Item("DoQuery") = ht("DoQuery").ToString()
            ViewState.Item("txtEmpID") = ht("txtEmpID").ToString()
            ViewState.Item("txtName") = ht("txtName").ToString()
        End If
    End Sub
    Public Overrides Sub DoAction(ByVal Param As String)
        Select Case Param
            Case "btnAdd"   '存檔返回
                If rbGrantFlag0.Checked = True Then
                    funCheckData0()
                ElseIf rbGrantFlag1.Checked = True Then
                    funCheckData1()
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

    Private Sub subLoadDropDownList()
        tabGrantFlag1.Visible = True
        tabGrantFlag0.Visible = False

        PA4.FillDDL(ddlUseCompID, "Company", "RTRIM(CompID)", "CompName + case when InValidFlag = '1' then '(無效)' else '' end", PA4.DisplayType.Full, "", "And CompID = 'SPHBK1'", "Order by InValidFlag, CompID")
        ddlUseCompID.Items.Insert(0, New ListItem("---請選擇---", ""))
    End Sub

    Private Function funCheckData1() As Boolean
        Dim bsVIPFlow As New beVIPFlow.Service()
        Dim SqlWhere As String = ""

        Dim arrAllGroupIDFlag() As String = hidAllGroupIDFlag.Value.Split(",")
        Dim arrAllCompOrganFlag() As String = hidAllCompOrganFlag.Value.Split(",")
        Dim arrAllOrganIDFlag() As String = hidAllOrganIDFlag.Value.Split(",")
        Dim arrUseOrganID() As String = hidUseOrganID.Value.Split(",")

        'A.111 
        If hidAllCompIDFlag.Value = "1" Then
            SqlWhere = " Where CompID = " & Bsp.Utility.Quote(UserProfile.SelectCompRoleID)
            SqlWhere += " And EmpID = " & Bsp.Utility.Quote(txtEmpID.Text)

            '檢查資料是否存在
            If bsVIPFlow.QuerybyWhere(SqlWhere).Tables(0).Rows.Count > 0 Then
                Bsp.Utility.RunClientScript(Me, "confirmAdd()")
                Return False
            End If
        End If

        'B.011
        If hidAllCompIDFlag.Value = "0" Then
            If arrAllGroupIDFlag.Length > 0 Then
                For i As Integer = 0 To arrAllGroupIDFlag.Length - 1
                    If arrAllGroupIDFlag(i) <> "" Then
                        For j As Integer = 0 To arrAllCompOrganFlag.Length - 1
                            If arrAllCompOrganFlag(j) <> "" Then
                                If arrAllCompOrganFlag(j) = arrAllGroupIDFlag(i) Then
                                    SqlWhere = " Where CompID = " & Bsp.Utility.Quote(UserProfile.SelectCompRoleID)
                                    SqlWhere += " And EmpID = " & Bsp.Utility.Quote(txtEmpID.Text)
                                    SqlWhere += " And UseCompID = " & Bsp.Utility.Quote(arrAllGroupIDFlag(i))

                                    '檢查資料是否存在
                                    If bsVIPFlow.QuerybyWhere(SqlWhere).Tables(0).Rows.Count > 0 Then
                                        Bsp.Utility.RunClientScript(Me, "confirmAdd()")
                                        Return False
                                    End If

                                    Dim newUseOrganID As List(Of String) = arrUseOrganID.ToList()
                                    For k As Integer = 0 To arrUseOrganID.Length - 1
                                        If arrUseOrganID(k).StartsWith(arrAllGroupIDFlag(i)) Then
                                            newUseOrganID.Remove(arrUseOrganID(k))
                                        End If
                                    Next
                                    arrUseOrganID = newUseOrganID.ToArray()
                                End If
                            End If
                        Next
                    End If
                Next
            End If
        End If

        'C.001
        Dim HasComp As Boolean = False
        If hidAllCompIDFlag.Value = "0" Then
            If arrAllOrganIDFlag.Length > 0 Then
                For i As Integer = 0 To arrAllOrganIDFlag.Length - 1
                    If arrAllOrganIDFlag(i) <> "" Then
                        HasComp = False
                        For j As Integer = 0 To arrAllGroupIDFlag.Length - 1
                            If arrAllGroupIDFlag(j) <> "" Then
                                If arrAllOrganIDFlag(i).StartsWith(arrAllGroupIDFlag(j)) Then
                                    HasComp = True
                                    Exit For
                                End If
                            End If
                        Next
                        If Not HasComp Then
                            SqlWhere = " Where CompID = " & Bsp.Utility.Quote(UserProfile.SelectCompRoleID)
                            SqlWhere += " And EmpID = " & Bsp.Utility.Quote(txtEmpID.Text)
                            SqlWhere += " And UseCompID = " & Bsp.Utility.Quote(arrAllOrganIDFlag(i).Split("_")(0))
                            SqlWhere += " And UseBusinessType = " & Bsp.Utility.Quote(arrAllOrganIDFlag(i).Split("_")(1))

                            '檢查資料是否存在
                            If bsVIPFlow.QuerybyWhere(SqlWhere).Tables(0).Rows.Count > 0 Then
                                Bsp.Utility.RunClientScript(Me, "confirmAdd()")
                                Return False
                            End If

                            Dim newUseOrganID As List(Of String) = arrUseOrganID.ToList()
                            For k As Integer = 0 To arrUseOrganID.Length - 1
                                If arrUseOrganID(k).StartsWith(arrAllOrganIDFlag(i)) Then
                                    newUseOrganID.Remove(arrUseOrganID(k))
                                End If
                            Next
                            arrUseOrganID = newUseOrganID.ToArray()
                        End If
                    End If
                Next
            End If
        End If

        'D.000
        If hidAllCompIDFlag.Value = "0" Then
            For Each item In arrUseOrganID
                SqlWhere = " Where CompID = " & Bsp.Utility.Quote(UserProfile.SelectCompRoleID)
                SqlWhere += " And EmpID = " & Bsp.Utility.Quote(txtEmpID.Text)
                SqlWhere += " And UseCompID = " & Bsp.Utility.Quote(item.Split("_")(0))
                SqlWhere += " And UseBusinessType = " & Bsp.Utility.Quote(item.Split("_")(1))
                SqlWhere += " And UseOrganID = " & Bsp.Utility.Quote(item.Split("_")(3))

                '檢查資料是否存在
                If bsVIPFlow.QuerybyWhere(SqlWhere).Tables(0).Rows.Count > 0 Then
                    'Bsp.Utility.ShowMessage(Me, "已有相同的特殊權限設定存在，不可重複新增！")
                    Bsp.Utility.RunClientScript(Me, "confirmAdd()")
                    Return False
                End If
            Next
        End If

        Release("btnAdd")
        Return True
    End Function

    Protected Sub btnConfirmAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnConfirmAdd.Click
        Release("btnAdd")
    End Sub

    '授權
    Private Function SaveData1(ByVal ReleaseComp As String, ByVal ReleaseID As String) As Boolean
        Dim beVIPParameterFlowRows() As beVIPParameterFlow.Row = Nothing
        Dim objPA4 As New PA4()
        Dim intCount As Integer = 0

        Dim arrAllGroupIDFlag() As String = hidAllGroupIDFlag.Value.Split(",")
        Dim arrAllCompOrganFlag() As String = hidAllCompOrganFlag.Value.Split(",")
        Dim arrAllOrganIDFlag() As String = hidAllOrganIDFlag.Value.Split(",")
        Dim arrUseOrganID() As String = hidUseOrganID.Value.Split(",")

        'A.111 
        If hidAllCompIDFlag.Value = "1" Then
            ReDim Preserve beVIPParameterFlowRows(intCount)

            Dim beVIPParameterFlow As New beVIPParameterFlow.Row()
            beVIPParameterFlow.CompID.Value = UserProfile.SelectCompRoleID
            beVIPParameterFlow.EmpID.Value = txtEmpID.Text
            beVIPParameterFlow.GrantFlag.Value = "1"

            'beVIPParameterFlow.AllCompIDFlag.Value = "1"
            beVIPParameterFlow.AllBusinessType.Value = "1"
            beVIPParameterFlow.AllOrganIDFlag.Value = "1"

            beVIPParameterFlow.UseCompID.Value = ""
            beVIPParameterFlow.UseBusinessType.Value = ""
            beVIPParameterFlow.UseOurColleagues.Value = GetOurColleagues()

            beVIPParameterFlow.BeginDate.Value = txtBeginDate.DateText
            beVIPParameterFlow.EndDate.Value = txtEndDate.DateText

            beVIPParameterFlow.CreateComp.Value = UserProfile.ActCompID
            beVIPParameterFlow.CreateID.Value = UserProfile.ActUserID
            beVIPParameterFlow.CreateDate.Value = Now

            beVIPParameterFlow.ReleaseComp.Value = ReleaseComp
            beVIPParameterFlow.ReleaseID.Value = ReleaseID
            beVIPParameterFlow.ReleaseDate.Value = Now

            beVIPParameterFlow.LastChgComp.Value = UserProfile.ActCompID
            beVIPParameterFlow.LastChgID.Value = UserProfile.ActUserID
            beVIPParameterFlow.LastChgDate.Value = Now

            beVIPParameterFlowRows(intCount) = beVIPParameterFlow
            intCount = intCount + 1
        End If

        'B.011
        If hidAllCompIDFlag.Value = "0" Then
            If arrAllGroupIDFlag.Length > 0 Then
                For i As Integer = 0 To arrAllGroupIDFlag.Length - 1
                    If arrAllGroupIDFlag(i) <> "" Then
                        For j As Integer = 0 To arrAllCompOrganFlag.Length - 1
                            If arrAllCompOrganFlag(j) <> "" Then
                                If arrAllCompOrganFlag(j) = arrAllGroupIDFlag(i) Then
                                    ReDim Preserve beVIPParameterFlowRows(intCount)

                                    Dim beVIPParameterFlow As New beVIPParameterFlow.Row()
                                    beVIPParameterFlow.CompID.Value = UserProfile.SelectCompRoleID
                                    beVIPParameterFlow.EmpID.Value = txtEmpID.Text
                                    beVIPParameterFlow.GrantFlag.Value = "1"

                                    'beVIPParameterFlow.AllCompIDFlag.Value = "0"
                                    beVIPParameterFlow.AllBusinessType.Value = "1"
                                    beVIPParameterFlow.AllOrganIDFlag.Value = "1"

                                    beVIPParameterFlow.UseCompID.Value = arrAllGroupIDFlag(i)
                                    beVIPParameterFlow.UseBusinessType.Value = ""
                                    beVIPParameterFlow.UseOrganID.Value = ""
                                    beVIPParameterFlow.UseOurColleagues.Value = GetOurColleagues()

                                    beVIPParameterFlow.BeginDate.Value = txtBeginDate.DateText
                                    beVIPParameterFlow.EndDate.Value = txtEndDate.DateText

                                    beVIPParameterFlow.CreateComp.Value = UserProfile.ActCompID
                                    beVIPParameterFlow.CreateID.Value = UserProfile.ActUserID
                                    beVIPParameterFlow.CreateDate.Value = Now

                                    beVIPParameterFlow.ReleaseComp.Value = ReleaseComp
                                    beVIPParameterFlow.ReleaseID.Value = ReleaseID
                                    beVIPParameterFlow.ReleaseDate.Value = Now

                                    beVIPParameterFlow.LastChgComp.Value = UserProfile.ActCompID
                                    beVIPParameterFlow.LastChgID.Value = UserProfile.ActUserID
                                    beVIPParameterFlow.LastChgDate.Value = Now

                                    beVIPParameterFlowRows(intCount) = beVIPParameterFlow
                                    intCount = intCount + 1

                                    Dim newUseOrganID As List(Of String) = arrUseOrganID.ToList()
                                    For k As Integer = 0 To arrUseOrganID.Length - 1
                                        If arrUseOrganID(k).StartsWith(arrAllGroupIDFlag(i)) Then
                                            newUseOrganID.Remove(arrUseOrganID(k))
                                        End If
                                    Next
                                    arrUseOrganID = newUseOrganID.ToArray()
                                End If
                            End If

                        Next
                    End If

                Next
            End If
        End If

        'C.001
        Dim HasComp As Boolean = False
        If hidAllCompIDFlag.Value = "0" Then
            If arrAllOrganIDFlag.Length > 0 Then
                For i As Integer = 0 To arrAllOrganIDFlag.Length - 1
                    If arrAllOrganIDFlag(i) <> "" Then
                        HasComp = False
                        For j As Integer = 0 To arrAllGroupIDFlag.Length - 1
                            If arrAllGroupIDFlag(j) <> "" Then
                                If arrAllOrganIDFlag(i).StartsWith(arrAllGroupIDFlag(j)) Then
                                    HasComp = True
                                    Exit For
                                End If
                            End If
                        Next
                        If Not HasComp Then
                            ReDim Preserve beVIPParameterFlowRows(intCount)

                            Dim beVIPParameterFlow As New beVIPParameterFlow.Row()
                            beVIPParameterFlow.CompID.Value = UserProfile.SelectCompRoleID
                            beVIPParameterFlow.EmpID.Value = txtEmpID.Text
                            beVIPParameterFlow.GrantFlag.Value = "1"

                            'beVIPParameterFlow.AllCompIDFlag.Value = "0"
                            beVIPParameterFlow.AllBusinessType.Value = "0"
                            beVIPParameterFlow.AllOrganIDFlag.Value = "1"

                            beVIPParameterFlow.UseCompID.Value = arrAllOrganIDFlag(i).Split("_")(0)
                            beVIPParameterFlow.UseBusinessType.Value = arrAllOrganIDFlag(i).Split("_")(1)
                            beVIPParameterFlow.UseOrganID.Value = ""
                            beVIPParameterFlow.UseOurColleagues.Value = GetOurColleagues()

                            beVIPParameterFlow.BeginDate.Value = txtBeginDate.DateText
                            beVIPParameterFlow.EndDate.Value = txtEndDate.DateText

                            beVIPParameterFlow.CreateComp.Value = UserProfile.ActCompID
                            beVIPParameterFlow.CreateID.Value = UserProfile.ActUserID
                            beVIPParameterFlow.CreateDate.Value = Now

                            beVIPParameterFlow.ReleaseComp.Value = ReleaseComp
                            beVIPParameterFlow.ReleaseID.Value = ReleaseID
                            beVIPParameterFlow.ReleaseDate.Value = Now

                            beVIPParameterFlow.LastChgComp.Value = UserProfile.ActCompID
                            beVIPParameterFlow.LastChgID.Value = UserProfile.ActUserID
                            beVIPParameterFlow.LastChgDate.Value = Now

                            beVIPParameterFlowRows(intCount) = beVIPParameterFlow
                            intCount = intCount + 1

                            Dim newUseOrganID As List(Of String) = arrUseOrganID.ToList()
                            For k As Integer = 0 To arrUseOrganID.Length - 1
                                If arrUseOrganID(k).StartsWith(arrAllOrganIDFlag(i)) Then
                                    newUseOrganID.Remove(arrUseOrganID(k))
                                End If
                            Next
                            arrUseOrganID = newUseOrganID.ToArray()
                        End If
                    End If
                Next
            End If
        End If

        'D.000
        If hidAllCompIDFlag.Value = "0" Then
            For Each item In arrUseOrganID
                ReDim Preserve beVIPParameterFlowRows(intCount)

                Dim beVIPParameterFlow As New beVIPParameterFlow.Row()
                beVIPParameterFlow.CompID.Value = UserProfile.SelectCompRoleID
                beVIPParameterFlow.EmpID.Value = txtEmpID.Text
                beVIPParameterFlow.GrantFlag.Value = "1"

                'beVIPParameterFlow.AllCompIDFlag.Value = "0"
                beVIPParameterFlow.AllBusinessType.Value = "0"
                beVIPParameterFlow.AllOrganIDFlag.Value = "0"

                beVIPParameterFlow.UseCompID.Value = item.Split("_")(0)
                beVIPParameterFlow.UseBusinessType.Value = item.Split("_")(1)
                beVIPParameterFlow.UseOrganID.Value = item.Split("_")(3)
                beVIPParameterFlow.UseOurColleagues.Value = GetOurColleagues()

                beVIPParameterFlow.BeginDate.Value = txtBeginDate.DateText
                beVIPParameterFlow.EndDate.Value = txtEndDate.DateText

                beVIPParameterFlow.CreateComp.Value = UserProfile.ActCompID
                beVIPParameterFlow.CreateID.Value = UserProfile.ActUserID
                beVIPParameterFlow.CreateDate.Value = Now

                beVIPParameterFlow.ReleaseComp.Value = ReleaseComp
                beVIPParameterFlow.ReleaseID.Value = ReleaseID
                beVIPParameterFlow.ReleaseDate.Value = Now

                beVIPParameterFlow.LastChgComp.Value = UserProfile.ActCompID
                beVIPParameterFlow.LastChgID.Value = UserProfile.ActUserID
                beVIPParameterFlow.LastChgDate.Value = Now

                beVIPParameterFlowRows(intCount) = beVIPParameterFlow
                intCount = intCount + 1
            Next
        End If

        '儲存資料
        Try
            Return objPA4.VIPFlowAdd(beVIPParameterFlowRows)
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, "[CC0201_99]" & Bsp.Utility.getMessage("E_00000") & Bsp.Utility.getInnerException(Me.FunID, ex))
            Return False
        End Try

        Return False
    End Function

    Private Function funCheckData0() As Boolean
        Dim bsVIPParameterFlow As New beVIPParameterFlow.Service()

        Dim arrUseGroupID() As String = hidUseGroupOrganID.Value.Split(",")
        Dim UseGroupID As String = ""
        Dim UseOrganID As String = ""

        '取得輸入資料
        For i As Integer = 0 To arrUseGroupID.Length - 1
            Dim beVIPParameterFlow As New beVIPParameterFlow.Row()
            UseGroupID = arrUseGroupID(i).Split("_")(1)
            UseOrganID = arrUseGroupID(i).Split("_")(2)

            beVIPParameterFlow.CompID.Value = UserProfile.SelectCompRoleID
            beVIPParameterFlow.EmpID.Value = txtEmpID.Text
            beVIPParameterFlow.GrantFlag.Value = "0"

            beVIPParameterFlow.UseCompID.Value = ddlUseCompID.SelectedValue
            beVIPParameterFlow.UseBusinessType.Value = UseGroupID
            beVIPParameterFlow.UseOrganID.Value = UseOrganID

            '檢查資料是否存在
            If bsVIPParameterFlow.IsDataExists(beVIPParameterFlow) Then
                'Bsp.Utility.ShowMessage(Me, "已有相同的特殊權限設定存在，不可重複排除！")
                Bsp.Utility.RunClientScript(Me, "confirmAdd()")
                Return False
            End If
        Next

        Release("btnAdd")
        Return True
    End Function

    '排除授權
    Private Function SaveData0(ByVal ReleaseComp As String, ByVal ReleaseID As String) As Boolean
        Dim beVIPParameterFlowRows() As beVIPParameterFlow.Row = Nothing
        Dim objPA4 As New PA4()

        Dim arrUseGroupID() As String = hidUseGroupOrganID.Value.Split(",")
        Dim UseGroupID As String = ""
        Dim UseOrganID As String = ""

        '取得輸入資料
        For i As Integer = 0 To arrUseGroupID.Length - 1
            ReDim Preserve beVIPParameterFlowRows(i)

            Dim beVIPParameterFlow As New beVIPParameterFlow.Row()
            UseGroupID = arrUseGroupID(i).Split("_")(1)
            UseOrganID = arrUseGroupID(i).Split("_")(3)

            beVIPParameterFlow.CompID.Value = UserProfile.SelectCompRoleID
            beVIPParameterFlow.EmpID.Value = txtEmpID.Text
            beVIPParameterFlow.GrantFlag.Value = "0"

            'beVIPParameterFlow.AllCompIDFlag.Value = "0"
            beVIPParameterFlow.AllBusinessType.Value = "0"
            beVIPParameterFlow.AllOrganIDFlag.Value = "0"

            beVIPParameterFlow.UseCompID.Value = ddlUseCompID.SelectedValue
            beVIPParameterFlow.UseBusinessType.Value = UseGroupID
            beVIPParameterFlow.UseOrganID.Value = UseOrganID

            beVIPParameterFlow.BeginDate.Value = "1900/01/01"
            beVIPParameterFlow.EndDate.Value = "1900/01/01"

            beVIPParameterFlow.CreateComp.Value = UserProfile.ActCompID
            beVIPParameterFlow.CreateID.Value = UserProfile.ActUserID
            beVIPParameterFlow.CreateDate.Value = Now

            beVIPParameterFlow.ReleaseComp.Value = ReleaseComp
            beVIPParameterFlow.ReleaseID.Value = ReleaseID
            beVIPParameterFlow.ReleaseDate.Value = Now

            beVIPParameterFlow.LastChgComp.Value = UserProfile.ActCompID
            beVIPParameterFlow.LastChgID.Value = UserProfile.ActUserID
            beVIPParameterFlow.LastChgDate.Value = Now

            beVIPParameterFlowRows(i) = beVIPParameterFlow
        Next

        '儲存資料
        Try
            Return objPA4.VIPFlowAdd(beVIPParameterFlowRows)
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, "[CC0201_99]" & Bsp.Utility.getMessage("E_00000") & Bsp.Utility.getInnerException(Me.FunID, ex))
            Return False
        End Try

        Return False
    End Function

    Private Sub ClearData()
        txtEmpID.Text = ""
        lblName.Text = ""

        ddlUseCompID.SelectedIndex = 0

        cb01.Checked = False
        cb02.Checked = False
        cb03.Checked = False
        cb04.Checked = False
        cb05.Checked = False
        cb06.Checked = False
        cb07.Checked = False

        txtBeginDate.DateText = ""
        txtEndDate.DateText = ""

        Bsp.Utility.RunClientScript(Me, "DoClear()")
    End Sub

    Private Sub Release(ByVal LogFunction As String)
        ucRelease.ShowCompRole = "True"
        ucRelease.FunID = "PA4100"
        ucRelease.LogFunction = LogFunction
        ucRelease.OpenSelect()
    End Sub

    Public Overrides Sub DoModalReturn(ByVal returnValue As String)
        Dim strSql As String = ""
        Dim result As Boolean = True

        If returnValue <> "" Then
            Dim aryData() As String = returnValue.Split(":")
            Select Case aryData(0)
                '員工編號
                Case "ucSelectEmpID"
                    Dim aryValue() As String = Split(aryData(1), "|$|")
                    txtEmpID.Text = aryValue(1)
                    lblName.Text = aryValue(2)

                Case "ucRelease"
                    Dim aryValue() As String = Split(aryData(1), "|$|")
                    If aryValue(0) = "Y" Then
                        If rbGrantFlag0.Checked = True Then
                            result = SaveData0(aryValue(1), aryValue(2))
                        ElseIf rbGrantFlag1.Checked = True Then
                            result = SaveData1(aryValue(1), aryValue(2))
                        End If

                        If result Then
                            GoBack()
                        End If
                    End If
            End Select
        End If
    End Sub

    Protected Sub txtEmpID_TextChanged(sender As Object, e As System.EventArgs) Handles txtEmpID.TextChanged
        If txtEmpID.Text <> "" Then
            Dim objHR As New HR
            Dim rtnTable As DataTable = objHR.GetHREmpName(UserProfile.SelectCompRoleID, txtEmpID.Text)
            If rtnTable.Rows.Count <= 0 Then
                lblName.Text = ""
                Bsp.Utility.ShowFormatMessage(Me, "H_00000", "人事資料尚未建檔")
            Else
                lblName.Text = rtnTable.Rows(0).Item(0)
            End If
        Else
            lblName.Text = ""
        End If
    End Sub

    '排除授權
    Protected Sub rbGrantFlag0_CheckedChanged(sender As Object, e As System.EventArgs) Handles rbGrantFlag0.CheckedChanged
        If rbGrantFlag0.Checked = True Then
            tabGrantFlag0.Visible = True
            tabGrantFlag1.Visible = False
        End If
    End Sub

    '授權
    Protected Sub rbGrantFlag1_CheckedChanged(sender As Object, e As System.EventArgs) Handles rbGrantFlag1.CheckedChanged
        If rbGrantFlag1.Checked = True Then
            tabGrantFlag1.Visible = True
            tabGrantFlag0.Visible = False
        End If
    End Sub

    Protected Sub cbAllOC_CheckedChanged(sender As Object, e As System.EventArgs) Handles cbAllOC.CheckedChanged
        If cbAllOC.Checked = True Then
            cb02.Checked = True
            cb03.Checked = True
            cb04.Checked = True
            cb05.Checked = True
            cb06.Checked = True
            cb07.Checked = True
            cb08.Checked = True
        Else
            cb02.Checked = False
            cb03.Checked = False
            cb04.Checked = False
            cb05.Checked = False
            cb06.Checked = False
            cb07.Checked = False
            cb08.Checked = False
        End If
    End Sub

    Private Function GetOurColleagues() As String
        Dim value As String = ""

        value = value & IIf(cb01.Checked, "|01", "")
        value = value & IIf(cb02.Checked, "|02", "")
        value = value & IIf(cb03.Checked, "|03", "")
        value = value & IIf(cb04.Checked, "|04", "")
        value = value & IIf(cb05.Checked, "|05", "")
        value = value & IIf(cb06.Checked, "|06", "")
        value = value & IIf(cb07.Checked, "|07", "")

        If value.Length > 0 Then
            value = value.Substring(1)
        End If

        Return value
    End Function

#Region "查詢員工姓名 Ajax"
    <System.Web.Services.WebMethod()> _
    Public Shared Function SelectEmpName(ByVal EmpID As String) As String
        Dim objHR As New HR
        Dim returnValue As New List(Of Dictionary(Of String, String))()
        Using dt = objHR.GetHREmpName(UserProfile.SelectCompRoleID, EmpID)
            If dt.Rows.Count > 0 Then
                Dim Dictionary As New Dictionary(Of String, String)
                Dictionary.Add("Name", dt.Rows(0).Item(0).ToString().Trim())
                returnValue.Add(Dictionary)
            End If
        End Using
        Return JsonConvert.SerializeObject(returnValue)
    End Function
#End Region

#Region "公司、業務類別、部門 連動Ajax"
    <System.Web.Services.WebMethod()> _
    Public Shared Function CreateCompCBL() As String
        Dim returnValue As New List(Of Dictionary(Of String, String))()
        Using dt = Bsp.DB.ExecuteDataSet(CommandType.Text, "Select CompID, CompName + case when InValidFlag = '1' then '(無效)' else '' end From Company Where CompID = 'SPHBK1' Order by InValidFlag, CompID", "eHRMSDB").Tables(0)
            For i As Integer = 0 To dt.Rows.Count - 1
                Dim Dictionary As New Dictionary(Of String, String)
                Dictionary.Add("CompID", dt.Rows(i).Item(0).ToString().Trim())
                Dictionary.Add("CompName", dt.Rows(i).Item(0).ToString().Trim() & "-" & dt.Rows(i).Item(1).ToString().Trim())
                returnValue.Add(Dictionary)
            Next
        End Using
        Return JsonConvert.SerializeObject(returnValue)
    End Function

    <System.Web.Services.WebMethod()> _
    Public Shared Function CreateGroupCBL(ByVal CompID As String) As String
        Dim strSQL As String = "Select Code, CodeCName From HRCodeMap Where TabName = 'Business' And FldName = 'BusinessType'"

        Dim returnValue As New List(Of Dictionary(Of String, String))()
        Using dt = Bsp.DB.ExecuteDataSet(CommandType.Text, strSQL, "eHRMSDB").Tables(0)
            For i As Integer = 0 To dt.Rows.Count - 1
                Dim Dictionary As New Dictionary(Of String, String)
                Dictionary.Add("GroupID", dt.Rows(i).Item(0).ToString().Trim())
                Dictionary.Add("OrganName", dt.Rows(i).Item(0).ToString().Trim() & "-" & dt.Rows(i).Item(1).ToString().Trim())
                returnValue.Add(Dictionary)
            Next
        End Using
        Return JsonConvert.SerializeObject(returnValue)
    End Function

    <System.Web.Services.WebMethod()> _
    Public Shared Function CreateOrganCBL(ByVal CompID As String, ByVal GroupID As String) As String
        Dim returnValue As New List(Of Dictionary(Of String, String))()

        Dim strSQL As String = "Select RoleCode, OrganID, OrganName + case when InValidFlag = '1' then '(無效)' else '' end From OrganizationFlow"
        strSQL &= " Where CompID = " & Bsp.Utility.Quote(CompID) & " And BusinessType =" & Bsp.Utility.Quote(GroupID)
        strSQL &= " Order By RoleCode Desc, OrganID"

        Using dt = Bsp.DB.ExecuteDataSet(CommandType.Text, strSQL, "eHRMSDB").Tables(0)
            For i As Integer = 0 To dt.Rows.Count - 1
                Dim Dictionary As New Dictionary(Of String, String)
                Dictionary.Add("RoleCode", dt.Rows(i).Item(0).ToString().Trim())
                Dictionary.Add("OrganID", dt.Rows(i).Item(1).ToString().Trim())
                Dictionary.Add("OrganName", dt.Rows(i).Item(1).ToString().Trim() & "-" & dt.Rows(i).Item(2).ToString().Trim())
                returnValue.Add(Dictionary)
            Next
        End Using
        Return JsonConvert.SerializeObject(returnValue)
    End Function

    <System.Web.Services.WebMethod()> _
    Public Shared Function CreateGroupOrganCBL(ByVal CompID As String) As String
        Dim returnValue As New List(Of Dictionary(Of String, String))()

        Dim strSQL As New StringBuilder
        strSQL.AppendLine("Select Org.GroupID, Flow.OrganName As GroupName, Org.OrganID, Org.OrganName + case when Org.InValidFlag = '1' then '(無效)' else '' end As OrganName")
        strSQL.AppendLine("From Organization Org")
        strSQL.AppendLine("Left Join (")
        strSQL.AppendLine("Select GroupID, OrganName From OrganizationFlow Where OrganID = GroupID And GroupID In (")
        strSQL.AppendLine("Select Distinct GroupID From Organization Where CompID = " & Bsp.Utility.Quote(CompID))
        strSQL.AppendLine(")) Flow On Flow.GroupID = Org.GroupID")
        strSQL.AppendLine("Where Org.CompID = " & Bsp.Utility.Quote(CompID) & " And Org.OrganID = Org.DeptID And Org.VirtualFlag = '0'")
        strSQL.AppendLine("And Org.GroupID In (")
        strSQL.AppendLine("Select GroupID From OrganizationFlow Where OrganID = GroupID And GroupID In (")
        strSQL.AppendLine("Select Distinct GroupID From Organization Where CompID = " & Bsp.Utility.Quote(CompID))
        strSQL.AppendLine(")) Order By Org.GroupID, Org.InValidFlag")

        Using dt = Bsp.DB.ExecuteDataSet(CommandType.Text, strSQL.ToString(), "eHRMSDB").Tables(0)
            For i As Integer = 0 To dt.Rows.Count - 1
                Dim Dictionary As New Dictionary(Of String, String)
                Dictionary.Add("GroupID", dt.Rows(i).Item(0).ToString().Trim())
                Dictionary.Add("GroupName", dt.Rows(i).Item(0).ToString().Trim() & "-" & dt.Rows(i).Item(1).ToString().Trim())
                Dictionary.Add("OrganID", dt.Rows(i).Item(2).ToString().Trim())
                Dictionary.Add("OrganName", dt.Rows(i).Item(2).ToString().Trim() & "-" & dt.Rows(i).Item(3).ToString().Trim())
                returnValue.Add(Dictionary)
            Next
        End Using

        Return JsonConvert.SerializeObject(returnValue)
    End Function
#End Region
End Class
