'****************************************************
'功能說明：結案退關卡查詢
'建立轉移人員：A02976  /  Tsao
'建立轉移日期：2008/07/25  /  20130917
'****************************************************
Imports System.Data

Partial Class WF_WFA092
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        txtCustomerID.Attributes.Add("onkeypress", "EntertoSubmit();")
        txtCName.Attributes.Add("onkeypress", "EntertoSubmit();")
        If Not IsPostBack Then
            PageInit()
        End If
    End Sub

    Private Sub PageInit()
        Dim objWF As New WF
        Using dt As DataTable = objWF.getFlowBackExecuteUser()
            ddlUserID.DataSource = dt
            ddlUserID.DataTextField = "UserNm"
            ddlUserID.DataValueField = "UserID"
            ddlUserID.DataBind()

            ddlUserID.Items.Insert(0, New ListItem("---不指定---", ""))
            ddlUserID.SelectedIndex = 0
        End Using
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
            If funCheckData() Then
                ht.Add(ddlCustomerID.ID, ddlCustomerID.SelectedValue)
                ht.Add(txtCustomerID.ID, txtCustomerID.Text.ToUpper)
                ht.Add(ddlCName.ID, ddlCName.SelectedValue)
                ht.Add(txtCName.ID, txtCName.Text)
                ht.Add(ddlAppID.ID, ddlAppID.SelectedValue)
                ht.Add(txtAppID.ID, txtAppID.Text)
                ht.Add(ddlUserID.ID, ddlUserID.SelectedValue)

                pcMain.DataTable = objWF.getFlowBackLog(ht)
                If pcMain.DataTable.Rows.Count > CInt(Bsp.Utility.QueryLimit()) Then
                    Bsp.Utility.ShowMessage(Me, "查詢結果筆數超過撈取筆數上限，請輸入更精確的條件  ", True)
                End If
                'Using dt As DataTable = objWF.getFlowBackLog(ht)
                '    gvMain.DataSource = dt
                '    gvMain.DataBind()
                'End Using
            End If
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & ".DoQuery", ex, ht.ToString())
        End Try
    End Sub

    Private Function funCheckData() As Boolean
        If txtCustomerID.Text.Trim() = "" AndAlso txtCName.Text.Trim() = "" _
            AndAlso txtAppID.Text.Trim() = "" AndAlso ddlUserID.SelectedValue = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_C1A00")
            Return False
        End If
        Return True
    End Function

    Private Sub DoDelete()

    End Sub

    Private Sub DoOtherAction()

    End Sub
End Class
