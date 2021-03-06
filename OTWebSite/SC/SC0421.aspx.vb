'****************************************************
'功能說明：訊息定義檔-新增
'建立人員：Chung
'建立日期：2013/01/29
'****************************************************
Imports System.Data

Partial Class SC_SC0421
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Bsp.Utility.FillCommon(rblMsgKind, "02", Bsp.Enums.SelectCommonType.All)
            Bsp.Utility.FillCommon(rblOpenFlag, "03", Bsp.Enums.SelectCommonType.All)
            Page.SetFocus(txtMsgCode)
        End If
        rblOpenWSize.Attributes.Add("onclick", "EnableSizeBox();")
    End Sub

    Private Sub InitialScreen()
        txtMsgCode.Text = ""
        rblMsgKind.SelectedIndex = -1
        txtMsgUrl.Text = ""
        txtMsgReason.Text = ""
        rblOpenFlag.SelectedIndex = -1
        rblDelKind.SelectedIndex = -1
        rblOpenWSize.SelectedIndex = -1
    End Sub

    Public Overrides Sub DoAction(ByVal Param As String)
        Select Case Param
            Case "btnAdd"
                If funCheckData() Then
                    If SaveData() Then
                        InitialScreen()
                        txtMsgCode.Focus()
                    End If
                End If
            Case "btnUpdate"
                If funCheckData() Then
                    If SaveData() Then GoBack()
                End If
            Case "btnActionX"
                GoBack()
        End Select
    End Sub

    Private Sub GoBack()
        Dim ti As TransferInfo = Me.StateTransfer
        Me.TransferFramePage(ti.CallerUrl, Nothing, ti.Args)
    End Sub

    Private Function SaveData() As Boolean
        Dim beMsgDefine As New beWF_MsgDefine.Row()
        Dim bsMsgDefine As New beWF_MsgDefine.Service()

        With beMsgDefine
            .MsgCode.Value = txtMsgCode.Text
            .MsgReason.Value = txtMsgReason.Text
            .MsgUrl.Value = txtMsgUrl.Text
            .MsgKind.Value = rblMsgKind.SelectedValue
            .OpenFlag.Value = rblOpenFlag.SelectedValue
            If rblOpenWSize.SelectedValue = "C" Then
                .OpenWSize.Value = txtSize.Text
            Else
                .OpenWSize.Value = rblOpenWSize.SelectedValue
            End If
            .DelKind.Value = rblDelKind.SelectedValue
            .LastChgDate.Value = Now
            .LastChgID.Value = UserProfile.ActUserID
        End With

        If bsMsgDefine.IsDataExists(beMsgDefine) Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00010", "訊息代碼")
            Return False
        End If

        Try
            bsMsgDefine.Insert(beMsgDefine)
        Catch ex As Exception
            Bsp.Utility.ShowMessage(Me, Me.FunID & ".SaveData", ex)
            Return False
        End Try
        Return True
    End Function

    Private Function funCheckData() As Boolean
        Dim strValue As String

        strValue = txtMsgCode.Text.Trim()
        If strValue = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", "訊息代碼")
            txtMsgCode.Focus()
            Return False
        Else
            If Bsp.Utility.getStringLength(strValue) > txtMsgCode.MaxLength Then
                Bsp.Utility.ShowFormatMessage(Me, "W_00040", "訊息代碼", txtMsgCode.MaxLength.ToString())
                txtMsgCode.Focus()
                Return False
            End If
            txtMsgCode.Text = strValue
        End If

        strValue = txtMsgReason.Text.Trim()
        If strValue = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", "讯息描述")
            txtMsgReason.Focus()
            Return False
        Else
            If Bsp.Utility.getStringLength(strValue) > txtMsgReason.MaxLength Then
                Bsp.Utility.ShowFormatMessage(Me, "W_00040", "訊息描述", txtMsgReason.MaxLength.ToString())
                txtMsgReason.Focus()
                Return False
            End If
            txtMsgReason.Text = strValue
        End If

        strValue = txtMsgUrl.Text.Trim()
        If strValue = "" Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00030", "連結網頁")
            txtMsgUrl.Focus()
            Return False
        Else
            If Bsp.Utility.getStringLength(strValue) > txtMsgUrl.MaxLength Then
                Bsp.Utility.ShowFormatMessage(Me, "W_00040", "連結網頁", txtMsgUrl.MaxLength.ToString())
                txtMsgUrl.Focus()
                Return False
            End If
            txtMsgUrl.Text = strValue
        End If
        txtMsgUrl.Text = strValue

        If rblMsgKind.SelectedIndex < 0 Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00035", "訊息類別")
            rblMsgKind.Focus()
            Return False
        End If

        If rblOpenFlag.SelectedIndex < 0 Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00035", "網頁開啟方式")
            rblOpenFlag.Focus()
            Return False
        End If

        If rblOpenWSize.SelectedIndex < 0 Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00035", "開啟視窗大小")
            rblOpenFlag.Focus()
            Return False
        End If

        If rblOpenWSize.SelectedValue = "C" Then
            strValue = txtSize.Text.Trim()
            Dim arySize() As String = strValue.Split("*")
            If arySize.Length <> 2 Then
                Bsp.Utility.ShowFormatMessage(Me, "W_03200")
                txtSize.Focus()
                Return False
            End If
            'If (Not IsNumeric(arySize(0))) OrElse (Not IsNumeric(arySize(1))) Then
            '    Bsp.Utility.ShowFormatMessage(Me, "W_03200")
            '    txtSize.Focus()
            '    Return False
            'End If
            txtSize.Text = arySize(0).Trim() & "*" & arySize(1).Trim()
        End If

        If rblDelKind.SelectedIndex < 0 Then
            Bsp.Utility.ShowFormatMessage(Me, "W_00035", "開啟刪除註記")
            rblDelKind.Focus()
            Return False
        End If

        Return True
    End Function
End Class
