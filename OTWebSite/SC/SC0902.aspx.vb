'****************************************************
'功能說明：資料授權設定-新增
'建立人員：BeatriceCheng
'建立日期：2015.05.28
'****************************************************
Imports System.Data

Partial Class SC_SC0902
    Inherits PageBase


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then
            Dim objSC As New SC
            txtComID.Text = UserProfile.SelectCompRoleName
            ucSelectEmpID.ShowCompRole = False
            ucSelectEmpID.ConnType = "SC"

            objSC.SC0900_FillTabName(ddlTabName)
            ddlTabName.Items.Insert(0, New ListItem("---請選擇---", ""))
            ddlFldName2.Items.Insert(0, New ListItem("---請選擇---", ""))
        End If
    End Sub
    Protected Overrides Sub BaseOnPageTransfer(ByVal ti As TransferInfo)
        If Not IsPostBack() Then
            Dim ht As Hashtable = Bsp.Utility.getHashTableFromParam(ti.Args)

            If ht.ContainsKey("SelectedTabName") Then
                ViewState.Item("TabName") = ht("SelectedTabName").ToString()
                ViewState.Item("FldName") = ht("SelectedFldName").ToString()
                ViewState.Item("Code") = ht("SelectedCode").ToString()
                subGetData(ViewState.Item("TabName"), ViewState.Item("FldName"), ViewState.Item("Code"))
            Else
                Return
            End If
        End If
    End Sub
    Public Overrides Sub DoAction(ByVal Param As String)
        Select Case Param
            Case "btnActionX"   '返回
                GoBack()
        End Select
    End Sub

    Private Sub GoBack()
        Dim ti As TransferInfo = Me.StateTransfer
        Me.TransferFramePage(ti.CallerUrl, Nothing, ti.Args)
    End Sub

    Private Sub subGetData(ByVal TabName As String, ByVal FldName As String, ByVal Code As String)
        Dim objSC As New SC
        Dim beHRCodeMap As New beHRCodeMap.Row()
        Dim bsHRCodeMap As New beHRCodeMap.Service()

        beHRCodeMap.TabName.Value = TabName
        beHRCodeMap.FldName.Value = FldName
        beHRCodeMap.Code.Value = Code
        Try
            Using dt As DataTable = bsHRCodeMap.QueryByKey(beHRCodeMap).Tables(0)
                If dt.Rows.Count <= 0 Then Exit Sub
                beHRCodeMap = New beHRCodeMap.Row(dt.Rows(0))

                ddlTabName.SelectedValue = beHRCodeMap.TabName.Value
                ddlTabName_SelectedIndexChanged()

                If ddlTabName.SelectedValue = "HRSupport_WorkType" And beHRCodeMap.CodeCName.Value = "Company" Then
                    ddlFldName1.SelectedValue = beHRCodeMap.FldName.Value
                Else
                    ddlFldName2.SelectedValue = beHRCodeMap.FldName.Value
                End If

                txtEmpID.Text = beHRCodeMap.Code.Value
                hldEmpID.Value = beHRCodeMap.Code.Value

                '最後異動公司
                Dim CompName As String = objSC.GetSC_CompName(beHRCodeMap.LastChgComp.Value)
                txtLastChgComp.Text = beHRCodeMap.LastChgComp.Value + IIf(CompName <> "", "-" + CompName, "")
                '最後異動人員
                Dim UserName As String = objSC.GetSC_UserName(beHRCodeMap.LastChgComp.Value, beHRCodeMap.LastChgID.Value)
                txtLastChgID.Text = beHRCodeMap.LastChgID.Value + IIf(UserName <> "", "-" + UserName, "")
                '最後異動日期
                Dim boolDate As Boolean = Format(beHRCodeMap.LastChgDate.Value, "yyyy/MM/dd") = "1900/01/01"
                txtLastChgDate.Text = IIf(boolDate, "", beHRCodeMap.LastChgDate.Value.ToString("yyyy/MM/dd HH:mm:ss"))
            End Using
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & "subGetData", ex)
        End Try

    End Sub

    Public Overrides Sub DoModalReturn(ByVal returnValue As String)
        Dim strSql As String = ""

        If returnValue <> "" Then
            Dim aryData() As String = returnValue.Split(":")
            Select Case aryData(0)
                '員工編號
                Case "ucSelectEmpID"
                    Dim objSC As New SC
                    Dim aryValue() As String = Split(aryData(1), "|$|")

                    txtEmpID.Text = aryValue(0) + "-" + aryValue(1) + "-" + aryValue(2)
                    hldEmpID.Value = aryValue(1)
            End Select
        End If
    End Sub

    Protected Sub ddlTabName_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlTabName.SelectedIndexChanged
        ddlTabName_SelectedIndexChanged()
    End Sub

    Private Sub ddlTabName_SelectedIndexChanged()
        Dim objSC As New SC
        If ddlTabName.SelectedValue = "" Then
            ddlFldName1.Visible = False
            ddlFldName1.Items.Clear()

            ddlFldName2.Items.Clear()
            ddlFldName2.Items.Insert(0, New ListItem("---請選擇---", ""))

        ElseIf ddlTabName.SelectedValue = "HRSupport_WorkType" Then
            ddlFldName1.Visible = True
            ddlFldName1.Items.Add(New ListItem("---請選擇---", ""))
            ddlFldName1.Items.Add(New ListItem("_SPHOLD-全金控", "_SPHOLD"))
            ddlFldName1.Items.Add(New ListItem("SPHBK1-永豐銀行", "SPHBK1"))

            PA4.FillDDL(ddlFldName2, "WorkType", "WorkTypeID", "Remark", PA4.DisplayType.Full, "", "And CompID = 'SPHBK1' and InValidFlag = '0'")
            ddlFldName2.Items.Insert(0, New ListItem("Other-其他人員", "Other"))
            ddlFldName2.Items.Insert(0, New ListItem("SelectOP-分行作業類人員", "SelectOP"))
            ddlFldName2.Items.Insert(0, New ListItem("---請選擇---", ""))

        Else
            ddlFldName1.Visible = False
            ddlFldName1.Items.Clear()

            Bsp.Utility.FillCompany(ddlFldName2, Bsp.Enums.FullNameType.CodeDefine)
            ddlFldName2.Items.Insert(0, New ListItem("All-所有公司別", "All"))
            ddlFldName2.Items.Insert(0, New ListItem("_SPHOLD-全金控", "_SPHOLD"))
            ddlFldName2.Items.Insert(0, New ListItem("---請選擇---", ""))
        End If
    End Sub
End Class
