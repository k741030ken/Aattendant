'****************************************************
'功能说明：结案退一關
'建立/轉移人员：A02976  /  Tsao
'建立日期：2008/07/14  /  20130913
'' 20120906 (U) 104751 201208230023 申请企审系统[结案退关]流程中,开放[覆审结案]退回前一關卡项目之權限
'****************************************************
Imports System.Data

Partial Class WF_WFA090
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            PageInit()
        End If
    End Sub

    Private Sub PageInit()
        Bsp.Utility.FillOrganization(ddlOrgan, Bsp.Enums.OrganType.Business, Bsp.Enums.FullNameType.CodeDefine)
        Dim strOrgan As String = ""
        If ddlOrgan.Items.Count > 0 Then
            For Each li As ListItem In ddlOrgan.Items
                strOrgan &= "," & li.Value
            Next
            strOrgan = strOrgan.Substring(1)
            If ddlOrgan.Items.Count > 1 Then
                ddlOrgan.Items.Insert(0, New ListItem("---所有部門---", ""))
                ddlOrgan.SelectedIndex = 0
            End If
        End If
        ViewState.Item("AllOrgan") = strOrgan
        Bsp.Utility.fillUserBySelectedValue(ddlAO, strOrgan, "")
        If ddlAO.Items.Count > 1 Then
            ddlAO.Items.Insert(0, New ListItem("---所有AO---", ""))
            ddlAO.SelectedIndex = 0
        End If
        '2012.10.11 (U) Chung 征信组长排除授信及覆审项目
        With ddlFlowType
            .Items.Clear()
            'If UserProfile.GroupID.ContainsKey("21") Then
            If UserProfile.GroupID.Contains("22") Then
                .Items.Add(New ListItem("授信结案-S999", "S999"))
                .Items.Add(New ListItem("徵信結案-CC99", "CC99"))
                .Items.Add(New ListItem("徵信結案-CC98", "CC98"))
                'ElseIf UserProfile.GroupID.Contains("21") Then
                '    .Items.Add(New ListItem("单笔利费率优惠申请结案-RA99", "RA99"))
                '    '.Items.Add(New ListItem("覆审结案-RV99", "RV99"))
                '    '.Items.Add(New ListItem("覆审销案-RV98", "RV98"))
                '    '.Items.Add(New ListItem("单笔利费率优惠申请结案-RA99", "RA99"))
                '    .SelectedIndex = 0
                'ElseIf UserProfile.GroupID.Contains("23") OrElse UserProfile.GroupID.Contains("24") Then
                '    .Items.Add(New ListItem("徵信結案-CC99", "CC99"))
                '    .Items.Add(New ListItem("徵信結案-CC98", "CC98"))
                '    .SelectedIndex = 0
            End If
        End With
    End Sub

    Protected Overrides Sub BaseOnPageTransfer(ByVal ti As TransferInfo)

    End Sub

    Public Overrides Sub DoAction(ByVal Param As String)
        Select Case Param
            Case "btnAdd"       '新增
                DoAdd()
            Case "btnUpdate"    '修改
                DoUpdate()
            Case "btnQuery"     '查询
                If (ddlOrgan.SelectedIndex < 0 OrElse ddlOrgan.SelectedValue = "") AndAlso _
                   (ddlAO.SelectedIndex < 0 OrElse ddlAO.SelectedValue = "") AndAlso txtAppID.Text.Trim() = "" AndAlso txtCName.Text.Trim() = "" AndAlso txtCustomerID.Text.Trim() = "" Then
                    Bsp.Utility.ShowMessage(Me, "请输入至少一个条件！")
                    Return
                End If
                DoQuery()
            Case "btnDelete"    '删除
                DoDelete()
            Case Else
                DoOtherAction()   '其他功能動作
        End Select
    End Sub

    Private Sub DoAdd()

    End Sub

    Private Sub DoUpdate()

    End Sub

    Private Sub DoQuery()
        Dim objWF As New WF
        Dim ht As New Hashtable

        Try
            ViewState.Item("FlowType") = ddlFlowType.SelectedValue

            If ddlOrgan.SelectedValue = "" Then
                ht.Add(ddlOrgan.ID, ViewState.Item("AllOrgan"))
            Else
                ht.Add(ddlOrgan.ID, ddlOrgan.SelectedValue)
            End If
            ht.Add(ddlAO.ID, ddlAO.SelectedValue)
            ht.Add(ddlAppID.ID, ddlAppID.SelectedValue)
            ht.Add(txtAppID.ID, txtAppID.Text.ToUpper())
            ht.Add(ddlCustomerID.ID, ddlCustomerID.SelectedValue)
            ht.Add(txtCustomerID.ID, txtCustomerID.Text.ToUpper)
            ht.Add(ddlCName.ID, ddlCName.SelectedValue)
            ht.Add(txtCName.ID, txtCName.Text)

            gvMain.Visible = False
            gvMain_CC99.Visible = False
            gvMain_CR.Visible = False

            Select Case ddlFlowType.SelectedValue
                Case "S999" '授信结案
                    Using dt As DataTable = objWF.getFinishCase(ht)
                        gvMain.DataSource = dt
                        gvMain.DataBind()
                        gvMain.Visible = True
                    End Using
                Case "CC99" '徵信結案
                    Using dt As DataTable = objWF.getCC99Case(ht)
                        gvMain_CC99.DataSource = dt
                        gvMain_CC99.DataBind()
                        gvMain_CC99.Visible = True
                    End Using
                Case "CC98" '徵信銷案
                    Using dt As DataTable = objWF.getCC98Case(ht)
                        gvMain_CC99.DataSource = dt
                        gvMain_CC99.DataBind()
                        gvMain_CC99.Visible = True
                    End Using
                    'Case "RV99" '覆审结案
                    '    ht.Add("Status", "9")
                    '    Using dt As DataTable = objWF.getCRCase(ht)
                    '        gvMain_CR.DataSource = dt
                    '        gvMain_CR.DataBind()
                    '        gvMain_CR.Visible = True
                    '    End Using
                    'Case "RV98" '覆审销案
                    '    ht.Add("Status", "0")
                    '    Using dt As DataTable = objWF.getCRCase(ht)
                    '        gvMain_CR.DataSource = dt
                    '        gvMain_CR.DataBind()
                    '        gvMain_CR.Visible = True
                    '    End Using
                    'Case "RA99" '单笔利费率优惠申请结案退關卡
                    '    Using dt As DataTable = objWF.GetRACase(ht)
                    '        gvMain_RA.DataSource = dt
                    '        gvMain_RA.DataBind()
                    '        gvMain_RA.Visible = True
                    '    End Using
            End Select
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & ".DoQuery", ex, ht.ToString())
        End Try
    End Sub

    Private Sub DoDelete()

    End Sub

    Private Sub DoOtherAction()

    End Sub

    Protected Sub ddlOrgan_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlOrgan.SelectedIndexChanged
        Bsp.Utility.fillUserBySelectedValue(ddlAO, ViewState.Item("AllOrgan"), ddlOrgan.SelectedValue)
        If ddlAO.Items.Count > 1 Then
            ddlAO.Items.Insert(0, New ListItem("---所有AO---", ""))
            ddlAO.SelectedIndex = 0
        End If
    End Sub

    Protected Sub gvMain_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles gvMain.RowEditing, gvMain_CC99.RowEditing, gvMain_CR.RowEditing, gvMain_RA.RowEditing
        Dim btnU As New ButtonState(ButtonState.emButtonType.Update)
        Dim btnX As New ButtonState(ButtonState.emButtonType.Exit)
        Dim objGV As GridView = CType(sender, GridView)
        Dim strParam_1 As String = ""
        Dim strFlowTypeNm As String = ""

        Select Case Bsp.Utility.IsStringNull(ViewState.Item("FlowType"))
            Case "S999"
                strFlowTypeNm = Bsp.Utility.IsStringNull(ViewState.Item("FlowType")) & "-授信结案"
            Case "CC99"
                strFlowTypeNm = Bsp.Utility.IsStringNull(ViewState.Item("FlowType")) & "-徵信結案"
                strParam_1 = "CCID=" & objGV.DataKeys(e.NewEditIndex)("CCID").ToString()
            Case "CC98"
                strFlowTypeNm = Bsp.Utility.IsStringNull(ViewState.Item("FlowType")) & "-徵信銷案"
                strParam_1 = "CCID=" & objGV.DataKeys(e.NewEditIndex)("CCID").ToString()
                'Case "RV99"
                '    strFlowTypeNm = ViewState.Item("FlowType") & "-覆审结案"
                '    strParam_1 = "CRID=" & objGV.DataKeys(e.NewEditIndex)("CRID").ToString()
                'Case "RV98"
                '    strFlowTypeNm = ViewState.Item("FlowType") & "-覆审销案"
                '    strParam_1 = "CRID=" & objGV.DataKeys(e.NewEditIndex)("CRID").ToString()
                'Case "RA99"
                '    strFlowTypeNm = ViewState.Item("FlowType") & "-单笔利费率优惠申请结案"
                '    strParam_1 = "RAID=" & objGV.DataKeys(e.NewEditIndex)("RAID").ToString()
        End Select

        btnU.Caption = "退一關"
        CallSmallPage("/WF/WFA091.aspx", New ButtonState() {btnU, btnX}, _
            "FlowType=" & ViewState.Item("FlowType"), _
            "FlowTypeNm=" & strFlowTypeNm, _
            "AppID=" & objGV.DataKeys(e.NewEditIndex)("AppID").ToString(), _
            "Customer=" & objGV.DataKeys(e.NewEditIndex)("CustomerID").ToString() & "-" & objGV.DataKeys(e.NewEditIndex)("CName").ToString(), _
            "AO=" & objGV.DataKeys(e.NewEditIndex)("OfficerNm").ToString(), _
            strParam_1)
    End Sub

    Public Overrides Sub DoModalReturn(ByVal returnValue As String)
        If returnValue = "OK" Then
            DoQuery()
        End If
    End Sub
End Class