'****************************************************
'功能說明：流程查詢
'建立人員：Chung
'建立日期：2013/01/29
'****************************************************
Imports System.Data.SqlClient
Imports System.Data
Imports System.Data.Common

Partial Class WF_WFA070
    Inherits PageBase

    Enum enumDropDownListType
        Organ
        AO
    End Enum

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                subInitScreen()
            End If
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & ".PageLoad", ex)
        End Try
    End Sub

    Protected Overrides Sub BaseOnPageTransfer(ByVal ti As TransferInfo)
        If ti.CallerPageID = "WFA071" Then
            For intLoop As Integer = 0 To ti.Args.Length - 1
                Dim intPos As Integer = ti.Args(intLoop).ToString().IndexOf("=")

                If intPos >= 0 Then
                    Dim aryItem(1) As String
                    aryItem(0) = ti.Args(intLoop).ToString().Substring(0, intPos)
                    aryItem(1) = ti.Args(intLoop).ToString().Substring(intPos + 1)

                    Dim obj As Control = Page.Form.FindControl(aryItem(0))
                    If obj IsNot Nothing Then
                        If TypeOf obj Is TextBox Then
                            CType(obj, TextBox).Text = aryItem(1)
                        ElseIf TypeOf obj Is RadioButtonList Then
                            CType(obj, RadioButtonList).SelectedValue = aryItem(1)
                        ElseIf TypeOf obj Is DropDownList Then
                            Bsp.Utility.IniListWithValue(CType(obj, DropDownList), aryItem(1))

                            If aryItem(0) = "ddlOrgan" Then
                                OrganSelectedIndexChanged()
                            End If
                        End If
                    End If
                End If
            Next
        Else
            Transfer(ti.Args)
        End If
    End Sub

    Private Sub subInitScreen()
        Bsp.Utility.FillOrganization(ddlOrgan, Bsp.Enums.OrganType.Dept, Bsp.Enums.FullNameType.CodeDefine)
        subSetAllListItem(enumDropDownListType.Organ)
        ddlOrgan.SelectedIndex = 0

        Bsp.Utility.FillUser(ddlAO, "And BanMark = '0'")
        subSetAllListItem(enumDropDownListType.AO)
        ddlAO.SelectedIndex = 0

        txtApplyCaseS.Text = Format(Now, "yyyy/MM/01")
        txtApplyCaseE.Text = Format(Now, "yyyy/MM/dd")

        Bsp.Utility.FillCommon(ddlFlowName, "04", Bsp.Enums.SelectCommonType.All)
    End Sub

    Private Sub subSetAllListItem(ByVal sType As enumDropDownListType)
        Select Case sType
            Case enumDropDownListType.Organ
                If ddlOrgan.Items.Count > 1 Then
                    ddlOrgan.Items.Insert(0, New ListItem("---All---", ""))
                    ddlOrgan.SelectedIndex = 0
                End If
            Case enumDropDownListType.AO
                If ddlAO.Items.Count > 1 Then
                    ddlAO.Items.Insert(0, New ListItem("---All---", ""))
                    ddlAO.SelectedIndex = 0
                End If
        End Select
    End Sub

    Public Overrides Sub DoAction(ByVal Param As String)
        If funCheckData() Then
            Select Case Param
                Case "btnAdd"       '新增

                Case "btnUpdate"    '修改
                    DoUpdate()
                Case "btnQuery"     '查詢
                    DoQuery()
                Case "btnActionC"    '
                    'DoComf()
                Case Else
                    DoOtherAction(Param)   '其他功能動作
            End Select
        End If
    End Sub

    Private Sub DoUpdate()

    End Sub

    Private Function funCheckData() As Boolean
        Dim strValue As String

        strValue = txtApplyCaseS.Text.Trim()
        If strValue <> "" Then
            strValue = Bsp.Utility.CheckDate(strValue)
            If strValue = "" Then
                Bsp.Utility.ShowFormatMessage(Me, "W_00060", lblApplyCase.Text)
                txtApplyCaseS.Focus()
                Return False
            End If
            txtApplyCaseS.Text = strValue
        End If

        strValue = txtApplyCaseE.Text.Trim()
        If strValue <> "" Then
            strValue = Bsp.Utility.CheckDate(strValue)
            If strValue = "" Then
                Bsp.Utility.ShowFormatMessage(Me, "W_00060", lblApplyCase.Text)
                txtApplyCaseE.Focus()
                Return False
            End If
            txtApplyCaseE.Text = strValue
        End If

        If txtApplyCaseS.Text <> "" AndAlso txtApplyCaseE.Text <> "" Then
            If txtApplyCaseS.Text > txtApplyCaseE.Text Then
                Bsp.Utility.ShowFormatMessage(Me, "W_00130")
                txtApplyCaseS.Focus()
                Return False
            End If
        End If

        Return True
    End Function

    Private Sub DoQuery()
        If funCheckData() Then
            Transfer( _
                "ddlOrgan=" & ddlOrgan.SelectedValue, _
                "ddlAO=" & ddlAO.SelectedValue, _
                "txtApplyCaseS=" & txtApplyCaseS.Text, _
                "txtApplyCaseE=" & txtApplyCaseE.Text, _
                "ddlFlowName=" & ddlFlowName.SelectedValue)
        End If
    End Sub


    Private Sub Transfer(ByVal ParamArray Args() As Object)
        Dim btnU As New ButtonState(ButtonState.emButtonType.Update)
        Dim btnX As New ButtonState(ButtonState.emButtonType.Exit)

        btnU.Caption = "連結案件"
        btnX.Caption = "返回"

        Me.TransferFramePage("~/WF/WFA071.aspx", New ButtonState() {btnU, btnX}, Args)
    End Sub

    Private Sub DoDelete()

    End Sub

    Private Sub DoOtherAction(ByVal Param As String)

    End Sub

    Private Sub OrganSelectedIndexChanged()
        Bsp.Utility.FillUser(ddlAO, "And BanMark = '0' And DeptID = " & Bsp.Utility.Quote(ddlOrgan.SelectedValue))
        subSetAllListItem(enumDropDownListType.AO)
    End Sub

    Protected Sub ddlOrgan_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlOrgan.SelectedIndexChanged
        OrganSelectedIndexChanged()
    End Sub
End Class
