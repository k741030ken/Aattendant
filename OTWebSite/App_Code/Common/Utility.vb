Imports Microsoft.VisualBasic
Imports eCredit
Imports System.Web
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Common
Imports System.Data
Imports System.Data.Common
Imports NLog

Namespace Bsp
    Public Class Utility
        Const myKey As String = "Bsp.Credit"

        Enum MessageType
            Errors = 0
            Information = 1
        End Enum

        Enum DisplayType
            OnlyName    '只顯示名字
            OnlyID           '顯示ID  
            Full        '顯示ID + 名字
        End Enum

        Public Shared MyLogger As Logger = LogManager.GetLogger("LoggerForNormal")

        Public Enum LogType
            Trace
            Debug
            Information
            Warn
            [Error]
        End Enum

#Region "ShowMessage:在Client端顯示Message"
        Public Shared Sub ShowMessage(ByVal obj As Object, ByVal Msg As String)
            Dim cMsgr As ClientScriptManager = CType(obj, Page).ClientScript
            Dim strKey As String = Bsp.Utility.GetNewFileName("Msg")
            cMsgr.RegisterStartupScript(obj.GetType, strKey, "<script type=""text/javascript"" language=""javascript"">alert(""" & Msg.Replace("\", "\\").Replace("""", "\""").Replace(vbCrLf, "\n\r") & """)</Script>")
        End Sub

        Public Shared Sub ShowMessageForAjax(ByVal obj As Object, ByVal Msg As String)
            Dim strKey As String = Bsp.Utility.GetNewFileName("Msg")
            ScriptManager.RegisterStartupScript(obj, obj.GetType, strKey, "alert(""" & Msg.Replace("\", "\\").Replace("""", "\""").Replace(vbCrLf, "\n\r") & """);", True)
        End Sub

        'Param:額外要寫出的錯誤訊息
        Public Shared Sub ShowMessage(ByVal obj As Object, ByVal Source As String, ByVal ex As Exception, ByVal ParamArray Param As Object())
            Array.Resize(Param, Param.Length + 1)
            Param(Param.Length - 1) = "UserID=" & UserProfile.UserID
            ShowMessage(obj, getInnerException(Source, ex, Param))
        End Sub

        Public Shared Sub ShowMessageForAjax(ByVal obj As Object, ByVal Source As String, ByVal ex As Exception, ByVal ParamArray Param As Object())
            Array.Resize(Param, Param.Length + 1)
            Param(Param.Length - 1) = "UserID=" & UserProfile.UserID
            ShowMessageForAjax(obj, getInnerException(Source, ex, Param))
        End Sub

        Public Shared Sub ShowFormatMessage(ByVal obj As Object, ByVal msgKey As String, ByVal ParamArray Args As Object())
            Dim cMsgr As ClientScriptManager = CType(obj, Page).ClientScript
            Dim strMsg As String = getMessage(msgKey)
            strMsg = String.Format(strMsg, Args)
            cMsgr.RegisterStartupScript(obj.GetType, "Startup", "<script type=""text/javascript"" language=""javascript"">alert(""" & strMsg & """)</Script>")
        End Sub

        Public Shared Sub ShowFormatMessageForAjax(ByVal obj As Object, ByVal msgKey As String, ByVal ParamArray Args As Object())
            Dim strMsg As String = getMessage(msgKey)
            Dim strKey As String = Bsp.Utility.GetNewFileName("Msg")
            strMsg = String.Format(strMsg, Args)

            ScriptManager.RegisterStartupScript(obj, obj.GetType, strKey, "alert(""" & strMsg & """);", True)
        End Sub

        Public Shared Sub ShowFormatMessage(ByVal obj As Object, ByVal msgKey As String, ByVal bolShowOnButtonLine As Boolean, ByVal ParamArray Args As Object())
            If bolShowOnButtonLine Then
                Dim strMsg As String = getMessage(msgKey)
                strMsg = String.Format(strMsg, Args)
                RunClientScript(obj, "showInformation(""" & Replace(strMsg, """", """""").Replace(vbCrLf, " ") & """);")
            Else
                ShowFormatMessage(obj, msgKey, Args)
            End If
        End Sub

        Public Shared Sub ShowFormatMessageForAjax(ByVal obj As Object, ByVal msgKey As String, ByVal bolShowOnButtonLine As Boolean, ByVal ParamArray Args As Object())
            If bolShowOnButtonLine Then
                Dim strMsg As String = getMessage(msgKey)
                strMsg = String.Format(strMsg, Args)
                RunClientScriptForAjax(obj, "showInformation(""" & Replace(strMsg, """", """""").Replace(vbCrLf, " ") & """);")
            Else
                ShowFormatMessageForAjax(obj, msgKey, Args)
            End If
        End Sub

        '顯示訊息在Button..主要用在存檔成功..等不需要特別警示User的訊息
        Public Shared Sub ShowMessage(ByVal obj As Object, ByVal Msg As String, ByVal bolShowOnButtonLine As Boolean)
            If bolShowOnButtonLine Then
                RunClientScript(obj, "showInformation(""" & Replace(Msg, """", """""") & """);")
            Else
                ShowMessage(obj, Msg)
            End If
        End Sub

        Public Shared Sub ShowMessageForAjax(ByVal obj As Object, ByVal Msg As String, ByVal bolShowOnButtonLine As Boolean)
            If bolShowOnButtonLine Then
                RunClientScriptForAjax(obj, "showInformation(""" & Replace(Msg, """", """""") & """);")
            Else
                ShowMessageForAjax(obj, Msg)
            End If
        End Sub
#End Region

#Region "runClientScript:從Server端寫出Client端執行的程式碼"
        Public Shared Sub RunClientScript(ByVal obj As Object, ByVal Script As String)
            Dim cMsgr As ClientScriptManager = CType(obj, Page).ClientScript
            Dim strKey As String = Bsp.Utility.GetNewFileName("Msg")

            cMsgr.RegisterStartupScript(obj.GetType, strKey, "<script type=""text/javascript"" language=""javascript"">" & Script & "</script>")
        End Sub

        Public Shared Sub RunClientScriptForAjax(ByVal obj As Page, ByVal Script As String)
            ScriptManager.RegisterStartupScript(obj, obj.GetType(), "ClientScriptForAjax", Script, True)
        End Sub
#End Region

#Region "getAppSetting:取得Web.config中的AppSetting變數的值"
        Public Shared Function getAppSetting(ByVal itemName As String) As String
            Return ConfigurationManager.AppSettings(itemName)
        End Function
#End Region

#Region "getMessagePageURL:取得顯示訊息至訊息頁的URL"
        Public Shared Function getMessagePageURL(ByVal InforPath As String, ByVal MessageType As Bsp.Enums.MessageType, ByVal Message As String, ByVal ButtonTitle As String, ByVal NextWebPage As String) As String
            Dim strURL As String

            strURL = getAppSetting("MessagePage") & "?" & _
                        "InforPath=" & System.Web.HttpContext.Current.Server.UrlEncode(InforPath) & _
                        "&MessageType=" & MessageType.ToString() & _
                        "&Message=" & System.Web.HttpContext.Current.Server.UrlEncode(Message) & _
                        "&ButtonTitle=" & System.Web.HttpContext.Current.Server.UrlEncode(ButtonTitle) & _
                        "&NextWebPage=" & System.Web.HttpContext.Current.Server.UrlEncode(NextWebPage)

            Return strURL
        End Function
#End Region

#Region "getInnerExcpetion：取得錯誤內部的訊息"
        'Param:額外要寫出的偵錯訊息
        Public Shared Function getInnerException(ByVal Source As String, ByVal ex As Exception, ByVal ParamArray Param As Object()) As String
            Return Bsp.Utility.ExtractErrMsg(ex, getAppSetting("LogFilePath"), Source, Param)
        End Function
#End Region

#Region "ExtractPageID:取得網頁檔名"
        Public Shared Function ExtractPageID(ByVal strURL As String) As String
            Dim strUrlLower As String = strURL.ToString()
            Dim intAspxIndex As Integer = strUrlLower.IndexOf(".aspx")

            If intAspxIndex >= 0 Then
                Dim intSlashIndex As Integer = strUrlLower.LastIndexOf("/", intAspxIndex)
                If intSlashIndex > 0 Then
                    Return strURL.Substring(intSlashIndex + 1, intAspxIndex - intSlashIndex - 1)
                Else
                    Return strURL.Substring(0, intAspxIndex)
                End If
            End If
            Return ""
        End Function
#End Region

#Region "fillUser：填入人員資料"
        Public Shared Sub FillUser(ByVal objDDL As DropDownList, ByVal DeptIDs As String, Optional ByVal CondStr As String = "")
            Dim objSC As New SC

            If CondStr <> "" Then CondStr &= vbCrLf
            CondStr &= "And BanMark = '0' And WorkTypeID <> 'ZZZZZZ'"
            Try
                Using dt As DataTable = objSC.GetUserInfobyDeptID(DeptIDs, "UserID, UserName, UserID + '-' + UserName FullName", CondStr)
                    With objDDL
                        .Items.Clear()
                        .DataTextField = "FullName"
                        .DataValueField = "UserID"
                        .DataSource = dt
                        .DataBind()
                    End With
                End Using
            Catch ex As Exception
                Throw
            End Try
        End Sub

        Public Shared Sub fillUserBySelectedValue(ByVal objDDL As DropDownList, ByVal QueryDept As String, ByVal sDeptID As String)
            Dim strSQL As New StringBuilder

            strSQL.AppendLine("Select Distinct UserID, UserID + '-' + UserName FullName")
            strSQL.AppendLine("From SC_User")
            strSQL.AppendLine("Where DeptID in ('" & QueryDept.Replace(",", "','") & "')")
            strSQL.AppendLine("And BanMark = '0'")
            strSQL.AppendLine("And BusinessFlag = '1'")
            'Add By Chung 2012.09.21 扣除代管AO
            strSQL.AppendLine("And WorkTypeID <> 'ZZZZZZ'")
            If sDeptID <> "" Then
                If sDeptID.IndexOf(",") >= 0 Then
                    strSQL.AppendLine("And DeptID in ('" & sDeptID.Replace(",", "','") & "')")
                Else
                    strSQL.AppendLine("And DeptID = " & Bsp.Utility.Quote(sDeptID))
                End If
            End If
            'If UserProfile.UserLevel = UserProfile.enumUserLevel.OnlyAO Then
            '    strSQL.AppendLine("And UserID = " & PubFun.Quote(UserProfile.UserID))
            'End If
            strSQL.AppendLine("Order by UserID")

            Try
                Using dt As DataTable = Bsp.DB.ExecuteDataSet(CommandType.Text, strSQL.ToString).Tables(0)
                    With objDDL
                        .Items.Clear()
                        .DataTextField = "FullName"
                        .DataValueField = "UserID"
                        .DataSource = dt
                        .DataBind()
                    End With
                End Using
            Catch ex As Exception
                Throw ex
            End Try
        End Sub
#End Region

#Region "fillOrganization：填入部門資料"
        Public Shared Sub FillOrganization(ByVal objDDL As DropDownList, ByVal SelectType As Bsp.Enums.OrganType)
            FillOrganization(objDDL, SelectType, Bsp.Enums.FullNameType.OnlyDefine)
        End Sub

        Public Shared Sub FillOrganization(ByVal objDDL As DropDownList, ByVal SelectType As Bsp.Enums.OrganType, ByVal ShowType As Bsp.Enums.FullNameType)
            Dim objSC As New SC
            Dim CondStr As String = ""

            Select Case SelectType
                Case Bsp.Enums.OrganType.Business
                    CondStr = "And InValidFlag = '0' And BusinessFlag = '1'" & vbCrLf & "Order by OrganID"
                Case Bsp.Enums.OrganType.Dept
                    CondStr = "And InValidFlag = '0' And OrganType = '1'" & vbCrLf & "Order by OrganID"
                Case Bsp.Enums.OrganType.Branch
                    CondStr = "And InValidFlag = '0' And BranchFlag = '1'" & vbCrLf & "Order by BranchNo"
            End Select

            Try
                Using dt As DataTable = objSC.GetOrganInfo("", "OrganID, OrganName, OrganID + '-' + OrganName OrganFullName", CondStr)
                    With objDDL
                        .Items.Clear()
                        .DataSource = dt
                        Select Case ShowType
                            Case Bsp.Enums.FullNameType.CodeDefine
                                .DataTextField = "OrganFullName"
                            Case Bsp.Enums.FullNameType.OnlyCode
                                .DataTextField = "OrganID"
                            Case Bsp.Enums.FullNameType.OnlyDefine
                                .DataTextField = "OrganName"
                        End Select
                        .DataValueField = "OrganID"
                        .DataBind()
                    End With
                End Using
            Catch ex As Exception
                Throw
            End Try
        End Sub

        Public Shared Sub fillOrganByValue(ByVal objDDL As DropDownList, ByVal QueryDept As String, Optional ByVal RegionID As String = "")
            Dim strSQL As New StringBuilder

            strSQL.AppendLine("Select distinct OrganID, OrganID + '-' + OrganName FullName")
            strSQL.AppendLine("From SC_Organization")
            strSQL.AppendLine("Where OrganID in ('" & QueryDept.Replace(",", "','") & "')")
            If RegionID <> "" Then
                strSQL.AppendLine("And RegionID = " & Bsp.Utility.Quote(RegionID))
            End If
            strSQL.AppendLine("Order by OrganID")

            Try
                Using dt As DataTable = Bsp.DB.ExecuteDataSet(CommandType.Text, strSQL.ToString()).Tables(0)
                    With objDDL
                        .Items.Clear()
                        .DataTextField = "FullName"
                        .DataValueField = "OrganID"
                        .DataSource = dt
                        .DataBind()
                    End With
                End Using
            Catch ex As Exception
                Throw ex
            End Try
        End Sub

#End Region

#Region "fillRegion：填入區域中心"
        Public Shared Sub FillRegion(ByVal objDDL As DropDownList, Optional ByVal CondStr As String = "")
            Dim objSC As New SC

            Try
                Using dt As DataTable = objSC.GetRegionInfo("", "RegionID, RegionName, RegionID + '-' + RegionName RegionFullName", CondStr)
                    With objDDL
                        .Items.Clear()
                        .DataSource = dt
                        .DataTextField = "RegionFullName"
                        .DataValueField = "RegionID"
                        .DataBind()
                    End With
                End Using
            Catch ex As Exception
                Throw
            End Try
        End Sub

#End Region

#Region "fillGroup：填入群組資料0500"
        Public Shared Sub FillGroup(ByVal objDDL As DropDownList)
            Dim objSC As New SC

            Try
                'Using dt As DataTable = objSC.GetGroupInfo_0500("", "GroupID, GroupID + '-' + GroupName as FullName, GroupName", "Order by GroupID")
                Using dt As DataTable = objSC.GetGroupInfo_0500("", UserProfile.SelectCompRoleID, UserProfile.LoginSysID, "GroupID, GroupID + '-' + GroupName as FullName, GroupName", "Order by GroupID")
                    With objDDL
                        .Items.Clear()
                        .DataSource = dt
                        .DataTextField = "FullName"
                        .DataValueField = "GroupID"
                        .DataBind()
                    End With
                End Using
            Catch ex As Exception
                Throw
            End Try
        End Sub
        Public Shared Sub FillGroup_0501(ByVal objDDL As DropDownList, ByVal CompRoleID As String)
            Dim objSC As New SC

            Try
                'Using dt As DataTable = objSC.GetGroupInfo_0500("", "GroupID, GroupID + '-' + GroupName as FullName, GroupName", "Order by GroupID")
                Using dt As DataTable = objSC.GetGroupInfo_0500("", CompRoleID, UserProfile.LoginSysID, "GroupID, GroupID + '-' + GroupName as FullName, GroupName", "Order by GroupID")
                    With objDDL
                        .Items.Clear()
                        .DataSource = dt
                        .DataTextField = "FullName"
                        .DataValueField = "GroupID"
                        .DataBind()
                    End With
                End Using
            Catch ex As Exception
                Throw
            End Try
        End Sub
        ''' <summary>
        ''' 填入群組資料
        ''' </summary>
        ''' <param name="objDDL"></param>
        ''' <param name="FillType"></param>
        ''' <param name="FunID">有/無包含此功能. 有/無以 FillType來看</param>
        ''' <remarks></remarks>
        Public Shared Sub FillGroup(ByVal objDDL As DropDownList, ByVal FillType As Bsp.Enums.SelectGroupType, ByVal FunID As String, ByVal CompRoleID As String)
            Dim objSC As New SC
            Dim CondStr As String = ""

            Select Case FillType
                Case Bsp.Enums.SelectGroupType.HasFun
                    If CompRoleID = "0" Then
                        CondStr = "And GroupID in (Select GroupID From SC_GroupFun Where FunID = " & Bsp.Utility.Quote(FunID) & ")"
                    Else
                        CondStr = "And GroupID in (Select GroupID From SC_GroupFun Where FunID = " & Bsp.Utility.Quote(FunID) & "and CompRoleID = " & Bsp.Utility.Quote(CompRoleID) & ")"
                    End If
                Case Bsp.Enums.SelectGroupType.NotHasFun
                    If CompRoleID = "0" Then
                        CondStr = "And GroupID not in (Select GroupID From SC_GroupFun Where FunID = " & Bsp.Utility.Quote(FunID) & ")"
                    Else
                        CondStr = "And GroupID not in (Select GroupID From SC_GroupFun Where FunID = " & Bsp.Utility.Quote(FunID) & "and CompRoleID = " & Bsp.Utility.Quote(CompRoleID) & ")"
                    End If
            End Select

            CondStr &= vbCrLf & "Order by GroupID"
            Try
                'Using dt As DataTable = objSC.GetGroupInfo("", "GroupID, GroupID + '-' + GroupName as FullName, GroupName", CondStr)GetGroupInfo_0500
                Using dt As DataTable = objSC.GetGroupInfo_0500("", "GroupID, GroupID + '-' + GroupName as FullName, GroupName", CondStr)
                    With objDDL
                        .Items.Clear()
                        .DataSource = dt
                        .DataTextField = "FullName"
                        .DataValueField = "GroupID"
                        .DataBind()
                    End With
                End Using
            Catch ex As Exception
                Throw ex
            End Try
        End Sub

        Public Shared Sub FillGroup_0504(ByVal objDDL As DropDownList, ByVal FillType As Bsp.Enums.SelectGroupType, ByVal FunID As String, ByVal CompRoleID As String)
            Dim objSC As New SC
            Dim CondStr As String = ""

            Select Case FillType
                Case Bsp.Enums.SelectGroupType.HasFun
                    If CompRoleID = "0" Then
                        CondStr = "And GroupID in (Select GroupID From SC_GroupFun Where FunID = " & Bsp.Utility.Quote(FunID) & ")"
                    Else
                        CondStr = "And GroupID in (Select GroupID From SC_GroupFun Where FunID = " & Bsp.Utility.Quote(FunID) & "and CompRoleID = " & Bsp.Utility.Quote(CompRoleID) & ")"
                    End If
                Case Bsp.Enums.SelectGroupType.NotHasFun
                    If CompRoleID = "0" Then
                        CondStr = "And GroupID not in (Select GroupID From SC_GroupFun Where FunID = " & Bsp.Utility.Quote(FunID) & ")"
                    Else
                        CondStr = "And GroupID not in (Select GroupID From SC_GroupFun Where FunID = " & Bsp.Utility.Quote(FunID) & "and CompRoleID = " & Bsp.Utility.Quote(CompRoleID) & ")"
                    End If
            End Select

            CondStr &= vbCrLf & "And CompRoleID = " & Bsp.Utility.Quote(CompRoleID) '20150305 Beatrice modify
            CondStr &= vbCrLf & "Order by GroupID"
            Try
                'Using dt As DataTable = objSC.GetGroupInfo("", "GroupID, GroupID + '-' + GroupName as FullName, GroupName", CondStr)GetGroupInfo_0500
                Using dt As DataTable = objSC.GetGroupInfo_0504("", " distinct GroupID, GroupID + '-' + GroupName as FullName, GroupName", CondStr)
                    With objDDL
                        .Items.Clear()
                        .DataSource = dt
                        .DataTextField = "FullName"
                        .DataValueField = "GroupID"
                        .DataBind()
                    End With
                End Using
            Catch ex As Exception
                Throw ex
            End Try
        End Sub
#End Region

#Region "fillFun：填入功能資料"
        ''' <summary>
        ''' 填入功能代碼
        ''' </summary>
        ''' <param name="objDDL"></param>
        ''' <param name="FillType"></param>
        ''' <param name="GroupID">若FillType=AssignToGroup, NotAssignToGroup則一定要傳入值</param>
        ''' <remarks></remarks>
        Public Shared Sub FillFun(ByVal objDDL As DropDownList, ByVal FillType As Bsp.Enums.SelectFunType, ByVal GroupID As String, ByVal CompRoleID As String)
            Dim objSC As New SC
            Dim CondStr As String = ""

            Select Case FillType
                Case Bsp.Enums.SelectFunType.ParentFun

                    CondStr = "And Path = '' And IsMenu = '1'"

                Case Bsp.Enums.SelectFunType.AssignToGroup

                    If GroupID = "" Then Return
                    If CompRoleID = "0" Then
                        CondStr = "And Exists (Select FunID From SC_GroupFun Where GroupID = " & Bsp.Utility.Quote(GroupID) & " And FunID = S.FunID)" '20150312 Beatrice modify
                    Else
                        CondStr = "And Exists (Select FunID From SC_GroupFun Where GroupID = " & Bsp.Utility.Quote(GroupID) & " And FunID = S.FunID and CompRoleID = " & Bsp.Utility.Quote(CompRoleID) & ")" '20150312 Beatrice modify
                    End If
                    
                Case Bsp.Enums.SelectFunType.FunHasRight

                    CondStr = "And Exists (Select FunID From SC_FunRight Where FunID = S.FunID)" '20150312 Beatrice modify
                    CondStr &= vbCrLf & "And CheckRight = '1'" '20160531 Beatrice modify

                Case Bsp.Enums.SelectFunType.NotAssignToGroup

                    If GroupID = "" Then Return

                    CondStr = "And Exists (Select FunID From SC_FunRight Where FunID = S.FunID)" '20150312 Beatrice modify
                    If CompRoleID = "0" Then
                        CondStr &= vbCrLf & "And Not Exists (Select FunID From SC_GroupFun Where GroupID = " & Bsp.Utility.Quote(GroupID) & " And FunID = S.FunID)" '20150312 Beatrice modify
                    Else
                        CondStr &= vbCrLf & "And Not Exists (Select FunID From SC_GroupFun Where GroupID = " & Bsp.Utility.Quote(GroupID) & " And FunID = S.FunID and CompRoleID = " & Bsp.Utility.Quote(CompRoleID) & ")" '20150312 Beatrice modify
                    End If
                    CondStr &= vbCrLf & "And CheckRight = '1'"
            End Select

            If CondStr <> "" Then CondStr &= vbCrLf
            CondStr &= "Order by FunID"

            Try
                Using dt As DataTable = objSC.GetFunInfo("", "FunID, FunName, FunID + '-' + FunName FullName", CondStr)
                    With objDDL
                        .Items.Clear()
                        .DataSource = dt
                        .DataTextField = "FullName"
                        .DataValueField = "FunID"
                        .DataBind()
                    End With
                End Using
            Catch ex As Exception
                Throw
            End Try
        End Sub

        Public Shared Sub FillFun_0504(ByVal objDDL As DropDownList, ByVal FillType As Bsp.Enums.SelectFunType, ByVal GroupID As String, ByVal CompRoleID As String)
            Dim objSC As New SC
            Dim CondStr As String = ""

            Select Case FillType
                Case Bsp.Enums.SelectFunType.ParentFun

                    CondStr = "And Path = '' And IsMenu = '1'"

                Case Bsp.Enums.SelectFunType.AssignToGroup

                    If GroupID = "" Then Return
                    If CompRoleID = "0" Then
                        CondStr = "And Exists (Select FunID From SC_GroupFun Where GroupID = " & Bsp.Utility.Quote(GroupID) & " And FunID = S.FunID)" '20150318 Beatrice modify
                    Else
                        CondStr = "And Exists (Select FunID From SC_GroupFun Where GroupID = " & Bsp.Utility.Quote(GroupID) & " And FunID = S.FunID and CompRoleID = " & Bsp.Utility.Quote(CompRoleID) & ")" '20150318 Beatrice modify
                    End If

                Case Bsp.Enums.SelectFunType.FunHasRight

                    CondStr = "And Exists (Select FunID From SC_FunRight Where FunID = S.FunID)" '20150318 Beatrice modify

                Case Bsp.Enums.SelectFunType.NotAssignToGroup

                    If GroupID = "" Then Return

                    CondStr = "And Exists (Select FunID From SC_FunRight Where FunID = S.FunID)" '20150318 Beatrice modify
                    If CompRoleID = "0" Then
                        CondStr &= vbCrLf & "And Exists (Select FunID From SC_GroupFun Where FunID = S.FunID)" '20150318 Beatrice modify
                    Else
                        CondStr &= vbCrLf & "And Exists (Select FunID From SC_GroupFun Where FunID = S.FunID and CompRoleID = " & Bsp.Utility.Quote(CompRoleID) & ")" '20150318 Beatrice modify
                    End If
                    CondStr &= vbCrLf & "And CheckRight = '1'"
            End Select

            If CondStr <> "" Then CondStr &= vbCrLf
            CondStr &= "Order by FunID"

            Try
                Using dt As DataTable = objSC.GetFunInfo("", "FunID, FunName, FunID + '-' + FunName FullName", CondStr)
                    With objDDL
                        .Items.Clear()
                        .DataSource = dt
                        .DataTextField = "FullName"
                        .DataValueField = "FunID"
                        .DataBind()
                    End With
                End Using
            Catch ex As Exception
                Throw
            End Try
        End Sub
#End Region

#Region "fillCommon：填入公用代碼"
        Public Shared Sub FillCommon(ByVal objList As Object, ByVal Type As String, ByVal FillType As Bsp.Enums.SelectCommonType)
            FillCommon(objList, Type, FillType, Bsp.Enums.FullNameType.CodeDefine)
        End Sub

        Public Shared Sub FillCommon(ByVal objList As Object, ByVal Type As String, ByVal FillType As Bsp.Enums.SelectCommonType, ByVal ShowType As Bsp.Enums.FullNameType)
            FillCommon(objList, Type, FillType, ShowType, "")
        End Sub

        ''' <summary>
        ''' 填入公用代碼
        ''' </summary>
        ''' <param name="objList">必須為DropDownList, CheckBoxList, RadioButtonList</param>
        ''' <param name="Type"></param>
        ''' <param name="FillType"></param>
        ''' <param name="ShowType"></param>
        ''' <param name="CondStr">僅能放Where條件式, 不允許放Order by語法</param>
        ''' <remarks></remarks>
        Public Shared Sub FillCommon(ByVal objList As Object, ByVal Type As String, ByVal FillType As Bsp.Enums.SelectCommonType, ByVal ShowType As Bsp.Enums.FullNameType, ByVal CondStr As String)
            Dim objSC As New SC()

            If CondStr.Trim <> "" Then CondStr &= vbCrLf

            Select Case FillType
                Case Bsp.Enums.SelectCommonType.InValid
                    CondStr &= " And ValidFlag = '0' Order by OrderSeq, Code"
                Case Bsp.Enums.SelectCommonType.Valid
                    CondStr &= " And ValidFlag = '1' Order by OrderSeq, Code"
                Case Else
                    CondStr &= " Order by OrderSeq, Code"
            End Select

            Try
                If TypeOf objList Is DropDownList OrElse TypeOf objList Is CheckBoxList OrElse TypeOf objList Is RadioButtonList Then
                    Using dt As DataTable = objSC.GetCommonInfo(Type, "", "Code, Define, Code + '-' + Define FullName", CondStr)
                        With objList
                            .Items.Clear()
                            .DataSource = dt
                            Select Case ShowType
                                Case Bsp.Enums.FullNameType.CodeDefine
                                    .DataTextField = "FullName"
                                Case Bsp.Enums.FullNameType.OnlyCode
                                    .DataTextField = "Code"
                                Case Bsp.Enums.FullNameType.OnlyDefine
                                    .DataTextField = "Define"
                            End Select
                            .DataValueField = "Code"
                            .DataBind()
                        End With
                    End Using
                End If
            Catch ex As Exception
                Throw
            End Try
        End Sub
#End Region

#Region "FillNation : 填入國家別資料"
        Public Shared Sub FillNation(ByVal ddlNation As DropDownList, ByVal ShowType As Bsp.Enums.FullNameType)
            Dim objSC As New SC()
            Dim strSQL As String = ""

            Select Case ShowType
                Case Enums.FullNameType.CodeDefine
                    strSQL = "Select NationID, NationID + '-' + ChnName ShowName From SC_Nation "
                Case Enums.FullNameType.OnlyCode
                    strSQL = "Select NationID, NationID as ShowName From SC_Nation "
                Case Enums.FullNameType.OnlyDefine
                    strSQL = "Select NationID, ChnName as ShowName From SC_Nation "
            End Select

            strSQL &= "Order by NationID"

            Using dt As DataTable = Bsp.DB.ExecuteDataSet(CommandType.Text, strSQL).Tables(0)
                With ddlNation
                    .DataTextField = "ShowName"
                    .DataValueField = "NationID"
                    .DataSource = dt
                    .DataBind()
                End With
            End Using
        End Sub
#End Region

#Region "FillGroup : 填入集團資料"
        Public Shared Sub FillGroup(ByVal ddlGroup As DropDownList, ByVal ShowType As Bsp.Enums.FullNameType)
            Dim objSC As New SC()
            Dim strSQL As String = ""

            Select Case ShowType
                Case Enums.FullNameType.CodeDefine
                    strSQL = "Select GroupNo, GroupNo + '-' + GroupName ShowName From AP_Group "
                Case Enums.FullNameType.OnlyCode
                    strSQL = "Select GroupNo, GroupNo as ShowName From AP_Group "
                Case Enums.FullNameType.OnlyDefine
                    strSQL = "Select GroupNo, GroupName as ShowName From AP_Group "
            End Select

            strSQL &= "Order by Cast(GroupNo as int)"

            Using dt As DataTable = Bsp.DB.ExecuteDataSet(CommandType.Text, strSQL).Tables(0)
                With ddlGroup
                    .DataTextField = "ShowName"
                    .DataValueField = "GroupNo"
                    .DataSource = dt
                    .DataBind()
                End With
            End Using
        End Sub
#End Region

#Region "FillBusType : 填入经齐类型代码"
        Public Shared Sub FillBusType(ByVal ddlGroup As DropDownList, ByVal ShowType As Bsp.Enums.FullNameType)
            Dim objSC As New SC()
            Dim strSQL As String = ""

            Select Case ShowType
                Case Enums.FullNameType.CodeDefine
                    strSQL = "Select BusTypeID, BusTypeID + '-' + BusTypeDesc ShowName From BR_BusType "
                Case Enums.FullNameType.OnlyCode
                    strSQL = "Select BusTypeID, BusTypeID as ShowName From BR_BusType "
                Case Enums.FullNameType.OnlyDefine
                    strSQL = "Select BusTypeID, BusTypeDesc as ShowName From BR_BusType "
            End Select

            strSQL &= "Order by Cast(BusTypeID as int)"

            Using dt As DataTable = Bsp.DB.ExecuteDataSet(CommandType.Text, strSQL).Tables(0)
                With ddlGroup
                    .DataTextField = "ShowName"
                    .DataValueField = "BusTypeID"
                    .DataSource = dt
                    .DataBind()
                End With
            End Using
        End Sub
#End Region

#Region "FillCurrency : 填入币种"
        Public Shared Sub FillCurrency(ByVal ddlGroup As DropDownList, ByVal ShowType As Bsp.Enums.FullNameType)
            Dim objSC As New SC()
            Dim strSQL As String = ""

            Select Case ShowType
                Case Enums.FullNameType.CodeDefine
                    strSQL = "Select CurrencyID, CurrencyID + '-' + CurrencyName ShowName From SC_Currency WHERE CurrencyNo <> '' "
                Case Enums.FullNameType.OnlyCode
                    strSQL = "Select CurrencyID, CurrencyID as ShowName From SC_Currency WHERE CurrencyNo <> '' "
                Case Enums.FullNameType.OnlyDefine
                    strSQL = "Select CurrencyID, CurrencyName as ShowName From SC_Currency WHERE CurrencyNo <> '' "
            End Select

            strSQL &= "Order by CurrencyID "

            Using dt As DataTable = Bsp.DB.ExecuteDataSet(CommandType.Text, strSQL).Tables(0)
                With ddlGroup
                    .DataTextField = "ShowName"
                    .DataValueField = "CurrencyID"
                    .DataSource = dt
                    .DataBind()
                End With
            End Using
        End Sub
#End Region

#Region "getCorp_Sub_Type：取得企业客户细类"
        'Corp_Sub_Type
        Public Shared Function getCorp_Sub_Type(ByVal Organization_Type As String) As DataTable
            Dim strSQL As String
            If Left(Organization_Type, 1) = "1" Then
                strSQL = "SELECT '' Code, '請選擇' Define UNION SELECT Code, Define FROM SC_Common WHERE [Type] = '217' AND RsvCol1 = '01'"
            Else
                strSQL = "SELECT '' Code, '請選擇' Define UNION SELECT Code, Define FROM SC_Common WHERE [Type] = '217' AND RsvCol1 != '01'"
            End If
            Return Bsp.DB.ExecuteDataSet(CommandType.Text, strSQL).Tables(0)
        End Function
#End Region

#Region "getOccInfo：取得行业類別"
        'OccType1
        Public Shared Function getOccInfo1(ByVal Type As Bsp.Enums.SelectCommonType) As DataTable
            Dim strSQL As String

            Select Case Type
                Case Bsp.Enums.SelectCommonType.InValid
                    strSQL = "SELECT OccType1, Type1Desc FROM BR_OccInfo1 WHERE DisplayFlag='N' "
                Case Bsp.Enums.SelectCommonType.Valid
                    strSQL = "SELECT OccType1, Type1Desc FROM BR_OccInfo1 WHERE DisplayFlag='Y' "
                Case Else
                    strSQL = "SELECT OccType1, Type1Desc FROM BR_OccInfo1 "
            End Select

            strSQL += " ORDER BY OccType1 "

            Return Bsp.DB.ExecuteDataSet(CommandType.Text, strSQL).Tables(0)
        End Function

        'OccType2
        Public Shared Function getOccInfo2(ByVal OccType1 As String, ByVal Type As Bsp.Enums.SelectCommonType) As DataTable
            Dim strSQL As String

            Select Case Type
                Case Bsp.Enums.SelectCommonType.InValid
                    strSQL = "SELECT OccType2, Type2Desc FROM BR_OccInfo2 WHERE OccType1 =" & Bsp.Utility.Quote(OccType1) & " AND DisplayFlag='N' "
                Case Bsp.Enums.SelectCommonType.Valid
                    strSQL = "SELECT OccType2, Type2Desc FROM BR_OccInfo2 WHERE OccType1 =" & Bsp.Utility.Quote(OccType1) & " AND DisplayFlag='Y' "
                Case Else
                    strSQL = "SELECT OccType2, Type2Desc FROM BR_OccInfo2 WHERE OccType1 =" & Bsp.Utility.Quote(OccType1)
            End Select

            strSQL += " ORDER BY Cast(OccType2 as int) "

            Return Bsp.DB.ExecuteDataSet(CommandType.Text, strSQL).Tables(0)
        End Function

        'OccType3
        Public Shared Function getOccInfo3(ByVal OccType2 As String, ByVal Type As Bsp.Enums.SelectCommonType) As DataTable
            Dim strSQL As String

            Select Case Type
                Case Bsp.Enums.SelectCommonType.InValid
                    strSQL = "SELECT OccType3, Type3Desc FROM BR_OccInfo3 WHERE OccType2 =" & Bsp.Utility.Quote(OccType2) & " AND DisplayFlag='N' "
                Case Bsp.Enums.SelectCommonType.Valid
                    strSQL = "SELECT OccType3, Type3Desc FROM BR_OccInfo3 WHERE OccType2 =" & Bsp.Utility.Quote(OccType2) & " AND DisplayFlag='Y' "
                Case Else
                    strSQL = "SELECT OccType3, Type3Desc FROM BR_OccInfo3 WHERE OccType2 =" & Bsp.Utility.Quote(OccType2)
            End Select

            strSQL += " ORDER BY Cast(OccType3 as int) "

            Return Bsp.DB.ExecuteDataSet(CommandType.Text, strSQL).Tables(0)
        End Function

        'OccType3
        Public Shared Function getOccInfo4(ByVal OccType4 As String, ByVal Type As Bsp.Enums.SelectCommonType) As DataTable
            Dim strSQL As String

            Select Case Type
                Case Bsp.Enums.SelectCommonType.InValid
                    strSQL = "SELECT OccType4, Type4Desc FROM BR_OccInfo4 WHERE OccType3 =" & Bsp.Utility.Quote(OccType4) & " AND DisplayFlag='N' "
                Case Bsp.Enums.SelectCommonType.Valid
                    strSQL = "SELECT OccType4, Type4Desc FROM BR_OccInfo4 WHERE OccType3 =" & Bsp.Utility.Quote(OccType4) & " AND DisplayFlag='Y' "
                Case Else
                    strSQL = "SELECT OccType4, Type4Desc FROM BR_OccInfo4 WHERE OccType3 =" & Bsp.Utility.Quote(OccType4)
            End Select

            strSQL += " ORDER BY Cast(OccType4 as int) "

            Return Bsp.DB.ExecuteDataSet(CommandType.Text, strSQL).Tables(0)
        End Function

        '透過OccType3取得相關的OccType1,OccType2
        Public Shared Function getParentOccType(ByVal OccType3 As String) As DataTable
            Dim strSQL As String

            strSQL = "SELECT OccType2 ,OccType1 FROM BR_OccInfo2 " _
                   + "WHERE OccType2 = (SELECT OccType2 FROM BR_OccInfo3 WHERE OccType3=" & Bsp.Utility.Quote(OccType3) & " ) "

            Return Bsp.DB.ExecuteDataSet(CommandType.Text, strSQL).Tables(0)
        End Function
#End Region

#Region "FillRC_Code：填入同一經濟關系代碼"
        ''' <summary>
        ''' 填入同一經濟關系代碼
        ''' 2013.04.09 Sharon Hung
        ''' </summary>
        ''' <param name="ddlGroup"></param>
        ''' <param name="ShowType"></param>
        ''' <remarks></remarks>
        Public Shared Sub FillRCCode(ByVal ddlGroup As DropDownList, ByVal ShowType As Bsp.Enums.FullNameType)
            Dim strSQL As String = ""

            Select Case ShowType
                Case Enums.FullNameType.CodeDefine
                    strSQL = " Select RCode,RCode+'-'+RCName ShowName "
                Case Enums.FullNameType.OnlyCode
                    strSQL = " Select RCode,RCode ShowName "
                Case Enums.FullNameType.OnlyDefine
                    strSQL = " Select RCode,RCName ShowName "
            End Select

            strSQL += "  From dbo.RL_Code where ValidFlag='Y' Order by RCode "

            Using dt As DataTable = Bsp.DB.ExecuteDataSet(CommandType.Text, strSQL).Tables(0)
                With ddlGroup
                    .DataTextField = "ShowName"
                    .DataValueField = "RCode"
                    .DataSource = dt
                    .DataBind()
                End With
            End Using
        End Sub
#End Region


#Region "IsCommonCodeExists : 代碼是否存在"
        Public Shared Function IsCommonCodeExists(ByVal Type As String, ByVal Code As String) As Boolean
            Dim strSQL As String

            strSQL = "Select Count(*) From SC_Common Where Type = " & Bsp.Utility.Quote(Type) & " And Code = " & Bsp.Utility.Quote(Code)
            If Convert.ToInt32(Bsp.DB.ExecuteScalar(strSQL)) > 0 Then
                Return True
            Else
                Return False
            End If
        End Function
#End Region

#Region "getMessage：取得Resource檔內的訊息字串"
        Public Shared Function getMessage(ByVal msgKey As String) As String
            Return "[" & msgKey & "]：" & Resources.Message.ResourceManager.GetString(msgKey)
        End Function
#End Region

#Region "getPaperCode：報告取號"
        Public Shared Function GetPaperCode(ByVal sPaperCode As String) As String
            If sPaperCode.Length <> 2 Then Return ""

            Dim db As Database = DatabaseFactory.CreateDatabase
            Dim DbCmd As DbCommand = db.GetStoredProcCommand("SP_GetPaperCode")
            db.AddInParameter(DbCmd, "@argPaperCodeType", DbType.String, sPaperCode)
            db.AddOutParameter(DbCmd, "@argID", DbType.String, 12)

            db.ExecuteNonQuery(DbCmd)
            Return db.GetParameterValue(DbCmd, "@argID")
        End Function
#End Region

#Region "parseValueToList：將','組合的集合字串值，指派給CheckBoxList物件"

        ''' <summary>
        ''' 將','組點字串值，指派給CheckBoxList物件
        ''' </summary>
        ''' <param name="objCBL">要指派值進去的CheckBoxList物件</param>
        ''' <param name="strValues">以','組合的集合字串值</param>
        ''' <remarks></remarks>
        Public Shared Sub parseValueToList(ByVal objCBL As CheckBoxList, ByVal strValues As String)
            Dim arrValues As String()
            If strValues IsNot Nothing Then
                arrValues = strValues.Split(",")
            Else
                arrValues = New String() {""}
            End If
            For Each strVal As String In arrValues
                If objCBL.Items.FindByValue(strVal) IsNot Nothing Then objCBL.Items.FindByValue(strVal).Selected = True
            Next
        End Sub
#End Region

#Region "IniListWithValue(多型)：將單一字串值，指派給RadioButtonList/DropDownList物件"

        ''' <summary>
        ''' 將單一字串值，指派給DropDownList物件
        ''' </summary>
        ''' <param name="objDDL">要指派值進去的DropDownList物件</param>
        ''' <param name="iniVal">要初始的指定字串值</param>
        ''' <remarks></remarks>
        Public Shared Sub IniListWithValue(ByVal objDDL As DropDownList, ByVal iniVal As String)
            objDDL.SelectedIndex = objDDL.Items.IndexOf(objDDL.Items.FindByValue(iniVal))
        End Sub

        ''' <summary>
        ''' 將單一字串值，指派給RadioButtonList物件
        ''' </summary>
        ''' <param name="objRBL">要指派值進去的RadioButtonList物件</param>
        ''' <param name="iniVal">要初始的指定字串值</param>
        ''' <remarks></remarks>
        Public Shared Sub IniListWithValue(ByVal objRBL As RadioButtonList, ByVal iniVal As String)
            objRBL.SelectedIndex = objRBL.Items.IndexOf(objRBL.Items.FindByValue(iniVal))
        End Sub
#End Region

#Region "JoinListValue：將CheckBoxList的選取值，組合成','分隔的字串"

        ''' <summary>
        ''' 將CheckBoxList的選取值，組合成','分隔的字串
        ''' </summary>
        ''' <param name="objCBL">要取出選取值的CheckBoxList物件</param>
        ''' <returns>以','分隔的選取值字串</returns>
        ''' <remarks></remarks>
        Public Shared Function JoinListValue(ByVal objCBL As CheckBoxList) As String
            Dim result As String = ""
            For i As Integer = 0 To objCBL.Items.Count - 1
                If objCBL.Items(i).Selected Then
                    If result <> "" Then result = result + ","
                    result = result + objCBL.Items(i).Value
                End If
            Next
            Return result
        End Function

#End Region

#Region "設定DropDownList的SelectedIndex值"
        Public Shared Sub SetSelectedIndex(ByRef objDDL As DropDownList, ByVal Value As String)
            objDDL.SelectedIndex = objDDL.Items.IndexOf(objDDL.Items.FindByValue(Value))
        End Sub

        Public Shared Sub SetSelectedIndex(ByVal objRB As RadioButtonList, ByVal Value As String)
            objRB.SelectedIndex = objRB.Items.IndexOf(objRB.Items.FindByValue(Value))
        End Sub
#End Region

#Region "檢查Decimal的Precision及Scale的長度"
        Public Shared Function DecimalCheck(ByVal strVal As Decimal, ByVal Precision As Integer, ByVal Scale As Integer) As String
            Return DecimalCheck(strVal.ToString(), Precision, Scale)
        End Function

        Public Shared Function DecimalCheck(ByVal strVal As String, ByVal Precision As Integer, ByVal Scale As Integer) As String
            Dim strResult As String = ""
            Dim regExpression As String = "^(-)?\d{1," + (Precision - Scale).ToString() + "}(\.\d{1," + Scale.ToString() + "})?$"
            Dim reg As New System.Text.RegularExpressions.Regex(regExpression)
            If reg.IsMatch(strVal) Then
                Return strResult
            Else
                For i As Integer = 1 To Precision - Scale
                    strResult += "9"
                Next
                strResult += "."
                For i As Integer = 1 To Scale
                    strResult += "9"
                Next
                Return strResult
            End If
        End Function
#End Region

#Region "getCommon：取得代碼資訊，以hashTable傳回"
        Public Shared Function getCommon(ByVal Type As String) As Hashtable
            Return getCommon(Type, "Code", "Define")
        End Function

        Public Shared Function getCommon(ByVal Type As String, ByVal KeyField As String, ByVal ValueField As String) As Hashtable
            Dim strSQL As String
            Dim ht As New Hashtable

            strSQL = "Select " & KeyField & ", " & ValueField & " From SC_Common Where Type = " & Bsp.Utility.Quote(Type) & " Order by Code"
            Using dt As DataTable = Bsp.DB.ExecuteDataSet(CommandType.Text, strSQL).Tables(0)
                For intLoop As Integer = 0 To dt.Rows.Count - 1
                    ht.Add(dt.Rows(intLoop).Item(0).ToString(), _
                           dt.Rows(intLoop).Item(1).ToString())
                Next
            End Using
            Return ht
        End Function

        Public Shared Function GetCommonData(ByVal Type As String, ByVal KeyField As String, ByVal ValueField As String) As DataTable
            Dim strSQL As New StringBuilder()

            strSQL.AppendFormat("Select {0}, {1} From SC_Common Where Type = " & Bsp.Utility.Quote(Type) & " Order by OrderSeq, Code", KeyField, ValueField)
            Return Bsp.DB.ExecuteDataSet(CommandType.Text, strSQL.ToString()).Tables(0)
        End Function
#End Region

#Region "Get Connection property"
        Enum ConnectionPart
            Server
            Database
            UserID
            Password
        End Enum

        Public Shared Function getConnectionProperty(ByVal ConStr As String, ByVal ConPart As ConnectionPart) As String
            Dim aryCon() As String = ConStr.Split(";")
            Dim ht As New Hashtable

            For intLoop As Integer = 0 To aryCon.GetUpperBound(0)
                If aryCon(intLoop).ToString().IndexOf("=") >= 0 Then
                    Dim aryPart() As String = aryCon(intLoop).Split("=")

                    ht.Add(aryPart(0).ToUpper(), aryPart(1))
                End If
            Next

            Select Case ConPart
                Case ConnectionPart.Server
                    Return ht.Item("SERVER").ToString()
                Case ConnectionPart.Database
                    Return ht.Item("DATABASE").ToString()
                Case ConnectionPart.UserID
                    Return ht.Item("UID").ToString()
                Case ConnectionPart.Password
                    Return ht.Item("PWD").ToString()
                Case Else
                    Return ""
            End Select
        End Function
#End Region

#Region "WaterMark"
        Enum WaterMarkType
            Query
            Print
            QueryPrint
        End Enum

        Public Shared Function getWaterMarkHeadline(ByVal wmType As WaterMarkType) As String
            Dim strWaterMark As String = ""

            Select Case wmType
                Case WaterMarkType.Query
                    strWaterMark &= String.Format("查询人员 : {0}({1}) {2}({3})", UserProfile.ActDeptName, UserProfile.ActDeptID, UserProfile.ActUserName, UserProfile.ActUserID)
                    strWaterMark &= String.Format("$$$查询时间 : {0} ", Now.ToString("yyyy/MM/dd HH:mm:ss"))
                    strWaterMark &= String.Format("$$$IP 地 址 : {0} ", Bsp.MySettings.ClientIP)
                Case WaterMarkType.Print
                    strWaterMark &= String.Format("打印人员 : {0}({1}) {2}({3})", UserProfile.ActDeptName, UserProfile.ActDeptID, UserProfile.ActUserName, UserProfile.ActUserID)
                    strWaterMark &= String.Format("$$$打印时间 : {0} ", Now.ToString("yyyy/MM/dd HH:mm:ss"))
                    strWaterMark &= String.Format("$$$IP 地 址 : {0} ", Bsp.MySettings.ClientIP)
            End Select

            Return strWaterMark
        End Function

        Public Shared Function getWaterMarkHeadline(ByVal wmType As WaterMarkType, ByVal htQueryInot As Hashtable) As String
            Dim strWaterMark As String = ""

            Select Case wmType
                Case WaterMarkType.Query
                    strWaterMark &= String.Format("查询人员 : {0}({1}) {2}({3})", UserProfile.ActDeptName, UserProfile.ActDeptID, UserProfile.ActUserName, UserProfile.ActUserID)
                    strWaterMark &= String.Format("$$$查询时间 : {0} ", Now.ToString("yyyy/MM/dd HH:mm:ss"))
                    strWaterMark &= String.Format("$$$IP 地 址 : {0} ", Bsp.MySettings.ClientIP)
                Case WaterMarkType.Print
                    strWaterMark &= String.Format("打印人员 : {0}({1}) {2}({3})", UserProfile.ActDeptName, UserProfile.ActDeptID, UserProfile.ActUserName, UserProfile.ActUserID)
                    strWaterMark &= String.Format("$$$打印时间 : {0} ", Now.ToString("yyyy/MM/dd HH:mm:ss"))
                    strWaterMark &= String.Format("$$$IP 地 址 : {0} ", Bsp.MySettings.ClientIP)
                Case WaterMarkType.QueryPrint
                    Dim strQueryID As String = ""
                    Dim strQueryUserName As String = ""
                    Dim strQueryDeptID As String = ""
                    Dim strQueryDeptName As String = ""
                    Dim strQueryDate As String = ""

                    For Each hashItem As DictionaryEntry In htQueryInot
                        Select Case hashItem.Key.ToString()
                            Case "QueryID"
                                strQueryID = hashItem.Value
                            Case "QueryUserName"
                                strQueryUserName = hashItem.Value
                            Case "QueryDeptID"
                                strQueryDeptID = hashItem.Value
                            Case "QueryDeptName"
                                strQueryDeptName = hashItem.Value
                            Case "QueryDate"
                                strQueryDate = hashItem.Value
                        End Select
                    Next

                    strWaterMark &= String.Format("查询人员 : {0}({1}) {2}({3}))", strQueryDeptName, strQueryDeptID, strQueryUserName, strQueryID)
                    strWaterMark &= String.Format("$$$查询时间 : {0} ", strQueryDate)
                    strWaterMark &= String.Format("$$$打印人员 : {0}({1}) {2}({3})", UserProfile.ActDeptName, UserProfile.ActDeptID, UserProfile.ActUserName, UserProfile.ActUserID)
                    strWaterMark &= String.Format("$$$打印时间 : {0} ", Now.ToString("yyyy/MM/dd HH:mm:ss"))
                    strWaterMark &= String.Format("$$$IP 地 址 : {0} ", Bsp.MySettings.ClientIP)
            End Select

            Return strWaterMark
        End Function
#End Region

#Region "將URL後面參數拆解、組合"
        'strParam格式需為A=1&B=2&C=3....
        Public Shared Function getHashTableFromParam(ByVal Params As String) As Hashtable
            Dim aryParam() As String = Params.Split("&")
            Dim ht As New Hashtable

            For intLoop As Integer = 0 To aryParam.GetUpperBound(0)
                Dim intPos As Integer = aryParam(intLoop).IndexOf("=")

                If intPos >= 0 Then ' AndAlso Not ht.ContainsKey(aryParam(intLoop).Substring(0, intPos)) Then
                    If ht.ContainsKey(aryParam(intLoop).Substring(0, intPos)) Then
                        ht(aryParam(intLoop).Substring(0, intPos)) = aryParam(intLoop).Substring(intPos + 1)
                    Else
                        ht.Add(aryParam(intLoop).Substring(0, intPos), aryParam(intLoop).Substring(intPos + 1))
                    End If
                End If
            Next
            Return ht
        End Function

        'strParam()為參數陣列,只回傳格式為XXX=yyy的資料
        Public Shared Function getHashTableFromParam(ByVal Params() As Object) As Hashtable
            Dim ht As New Hashtable

            For Each Param As Object In Params
                If TypeOf Param Is String Then
                    Dim intPos As Integer = Param.ToString().IndexOf("=")

                    If intPos >= 0 Then ' AndAlso Not ht.ContainsKey(Param.ToString().Substring(0, intPos)) Then
                        If ht.ContainsKey(Param.ToString().Substring(0, intPos)) Then
                            ht(Param.ToString().Substring(0, intPos)) = Param.ToString().Substring(intPos + 1)
                        Else
                            ht.Add(Param.ToString().Substring(0, intPos), Param.ToString().Substring(intPos + 1))
                        End If
                    End If
                ElseIf TypeOf Param Is FlowBackInfo Then
                    'FlowBackInfo不動作
                ElseIf TypeOf Param Is Object Then
                    'For Each pair As DictionaryEntry In Param
                    '    ht.Add(pair.Key, pair.Value)
                    'Next
                End If
            Next
            Return ht
        End Function

        Public Shared Function getParamFromHashTable(ByVal ht As Hashtable) As String
            Dim strRtn As String = ""
            For Each strKey As String In ht.Keys
                strRtn &= "&" & strKey & "=" & ht(strKey).ToString()
            Next
            If strRtn <> "" Then strRtn = strRtn.Substring(1)
            Return strRtn
        End Function

#End Region

#Region "將控制項轉成參數的Format"
        ''' <summary>
        ''' 將TextBox轉成 ID=Value的Format
        ''' </summary>
        ''' <param name="TB"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function FormatToParam(ByVal TB As TextBox, Optional ByVal DefaultName As String = "") As String
            If DefaultName = "" Then DefaultName = TB.ID
            Return String.Format("{0}={1}", DefaultName, TB.Text.Trim())
        End Function

        Public Shared Function FormatToParam(ByVal DDL As DropDownList, Optional ByVal DefaultName As String = "") As String
            If DefaultName = "" Then DefaultName = DDL.ID
            Return String.Format("{0}={1}", DefaultName, DDL.SelectedValue)
        End Function

        Public Shared Function FormatToParam(ByVal ChkBox As CheckBoxList, Optional ByVal DefaultName As String = "") As String
            If DefaultName = "" Then DefaultName = ChkBox.ID
            Return String.Format("{0}={1}", DefaultName, Bsp.Utility.JoinListValue(ChkBox))
        End Function

        Public Shared Function FormatToParam(ByVal LBL As Label, Optional ByVal DefaultName As String = "") As String
            If DefaultName = "" Then DefaultName = LBL.ID
            Return String.Format("{0}={1}", DefaultName, LBL.Text)
        End Function

        Public Shared Function FormatToParam(ByVal RBL As RadioButtonList, Optional ByVal DefaultName As String = "") As String
            If DefaultName = "" Then DefaultName = RBL.ID
            Return String.Format("{0}={1}", DefaultName, RBL.SelectedValue)
        End Function
#End Region

#Region "ToNumerber：將字串轉換成數值"
        Public Shared Function ToInteger(ByVal SrcValue As String, Optional ByVal DefaultValue As Integer = 0) As Integer
            Try
                Return Integer.Parse(SrcValue)
            Catch ex As Exception
                Return DefaultValue
            End Try
        End Function

        Public Shared Function ToDecimal(ByVal SrcValue As String, Optional ByVal DefaultValue As Decimal = 0) As Decimal
            Try
                Return Decimal.Parse(SrcValue)
            Catch ex As Exception
                Return DefaultValue
            End Try
        End Function
#End Region

#Region "InStr：判斷子字串是否存在母字串中"
        ''' <summary>
        ''' 判斷子字串是否存在母字串中
        ''' </summary>
        ''' <param name="Src">母字串</param>
        ''' <param name="SubStr">子字串</param>
        ''' <returns>True/False</returns>
        ''' <remarks></remarks>
        Public Shared Function InStr(ByVal Src As String, ByVal ParamArray SubStr() As String) As Boolean
            For Each s As String In SubStr
                If Src = s Then Return True
            Next
            Return False
        End Function
#End Region

#Region "取得列印人員列印日期Format"
        Public Shared Function GetPrintUserInfo() As String()
            Return New String() { _
                String.Format("{0} {1}", UserProfile.ActUserID, UserProfile.ActUserName), _
                Now.ToString("yyyy/MM/dd HH:mm:ss"), _
                Bsp.MySettings.ClientIP}
        End Function

#End Region


#Region "字串加解密"
        '字串加密
        Public Shared Function menuEncoding(ByVal strSrc As String) As String
            Dim strOutput As String = ""
            Dim strMsg As String = ""

            Dim bytes() As Byte = System.Text.Encoding.Unicode.GetBytes(strSrc)
            Dim strTmp As String

            For intLoop As Integer = 0 To bytes.GetUpperBound(0)
                strTmp = Hex(bytes(intLoop))
                If strTmp.Length = 1 Then strTmp = "0" & strTmp
                strOutput &= strTmp
            Next

            Return strOutput
        End Function

        Public Shared Function menuDecoding(ByVal strSrc As String) As String
            Dim strOutput As String

            Try
                If Len(strSrc) Mod 2 <> 0 Then
                    Throw New Exception("stringDecoding:This string is not an encrypted string")
                End If

                Dim bytes(Len(strSrc) \ 2 - 1) As Byte
                Dim strTmp As String = ""
                For intLoop As Integer = 0 To bytes.GetUpperBound(0)
                    bytes(intLoop) &= CInt("&H" & strSrc.Substring(intLoop * 2, 2))
                Next

                strOutput = System.Text.Encoding.Unicode.GetString(bytes)
                Return strOutput
            Catch ex As Exception
                Throw
            End Try
        End Function

        '字串加密
        Public Shared Function stringEncoding(ByVal Src As String) As String
            Return stringEncoding(Src, myKey)
        End Function

        Public Shared Function stringEncoding(ByVal strSrc As String, ByVal strKey As String) As String
            Dim objSec As New ECSecurity.clsEnCoder()
            Dim strOutput As String = ""
            Dim strMsg As String = ""

            Try
                If objSec.gfunEncryptString2W(strKey, strSrc, strOutput, strMsg) = True Then
                    Return strOutput
                Else
                    Throw New Exception(strMsg)
                End If
            Catch ex As Exception
                Throw ex
            End Try

        End Function

        '字串解密
        Public Shared Function stringDecoding(ByVal Src As String) As String
            Return stringDecoding(Src, myKey)
        End Function

        Public Shared Function stringDecoding(ByVal strSrc As String, ByVal strKey As String) As String
            Dim objSec As New ECSecurity.clsEnCoder()
            Dim strOutput As String = ""
            Dim strMsg As String = ""

            Try
                If objSec.gfunDecryptString2W(strKey, strSrc, strOutput, strMsg) = True Then
                    Return strOutput
                Else
                    Throw New Exception(strMsg)
                End If
            Catch ex As Exception
                Throw
            End Try
        End Function
#End Region

#Region "passwordEncrypt:密碼加密"
        Public Shared Function passwordEncrypt(ByVal strSrc As String) As String
            Dim objSec As New ECSecurity.clsEnCoder()
            Dim strOutput As String = ""
            Dim strMsg As String = ""

            Try
                If objSec.gfunEncryptString(strSrc, strOutput, strMsg) = True Then
                    Return strOutput
                Else
                    Throw New Exception(strMsg)
                End If
            Catch ex As Exception
                Throw
            End Try

        End Function
#End Region

#Region "String Function"
        Public Shared Function Quote(ByVal Src As String) As String
            Return "'" & Src.Replace("'", "''") & "'"
        End Function

        Public Shared Function getStringLength(ByVal Src As String) As Integer
            Return System.Text.Encoding.Default.GetBytes(Src).Length
        End Function

        ''' <summary>
        ''' 檢查是否為日期格式，若成功則回傳yyyy/mm/dd的日期字串，失敗回傳空字串
        ''' </summary>
        ''' <param name="sDate">請輸入8碼(yyyymmdd)或10碼(yyyy/mm/dd)</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function CheckDate(ByVal sDate As String) As String
            Dim strValue As String = ""

            If IsDate(sDate) Then
                strValue = CDate(sDate).ToString("yyyy/MM/dd")
            Else
                Select Case sDate.Length
                    Case 8
                        strValue = String.Format("{0}/{1}/{2}", sDate.Substring(0, 4), sDate.Substring(4, 2), sDate.Substring(6, 2))
                        If Not IsDate(strValue) Then strValue = ""
                    Case 10
                        If IsDate(sDate) Then strValue = sDate
                End Select
            End If

            Return strValue
        End Function

        ''' <summary>
        ''' 檢查是否為時間格式，若成功則回傳HH:mm:ss的時間字串，失敗回傳空字串
        ''' </summary>
        ''' <param name="sTime">請輸入4碼(HHmm)、5碼(HH:mm)、6碼(HHmmss)、8碼(HH:mm:ss)</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function CheckTime(ByVal sTime As String) As String
            Dim strValue As String = ""

            Select Case sTime.Length
                Case 4
                    strValue = String.Format("{0}:{1}:00", sTime.Substring(0, 2), sTime.Substring(2, 2))
                Case 5
                    strValue = sTime & ":00"
                Case 6
                    strValue = String.Format("{0}:{1}:{2}", sTime.Substring(0, 2), sTime.Substring(2, 2), sTime.Substring(4, 2))
                Case 8
                    strValue = sTime
            End Select
            If Not IsDate(String.Format("1900/01/01 {0}", strValue)) Then strValue = ""

            Return strValue
        End Function
#End Region

#Region "DataTime Function"
        '**********************************************************************************
        '說明：將日期型態轉換成字串，若日期為1900/1/1，表示為系統default時間，則回傳空字串
        '參數：dt：時間；dtType：Date則回傳yyyy/MM/dd, DateTime回傳yyyy/MM/dd HH:mm:ss
        '**********************************************************************************
        Public Shared Function DateTimeToString(ByVal dt As DateTime, ByVal dtType As Bsp.Enums.DateTimeType) As String
            Dim strValue As String = ""

            If Not IsDBNull(dt) Then
                If dt = CDate("1900/1/1") OrElse dt = CDate("0001/01/01") Then
                    strValue = ""
                Else
                    Select Case dtType
                        Case Bsp.Enums.DateTimeType.Date
                            strValue = dt.ToString("yyyy/MM/dd")
                        Case Bsp.Enums.DateTimeType.DateTime
                            strValue = dt.ToString("yyyy/MM/dd HH:mm:ss")
                    End Select
                End If
            End If

            Return strValue
        End Function
#End Region

#Region "IsNull：判斷傳入值是否為Nothing"
        ''' <summary>
        ''' 判斷傳入值是否為Null，若是回傳DefaultValue；若否則回傳Src本身
        ''' </summary>
        ''' <param name="Src">來源字串</param>
        ''' <param name="DefaultValue">預設值</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function IsStringNull(ByVal Src As Object, Optional ByVal DefaultValue As String = "") As String
            If Src Is Nothing OrElse IsDBNull(Src) Then
                Return DefaultValue
            Else
                Return Src.ToString()
            End If
        End Function

        ''' <summary>
        ''' 判斷傳入值是否為Decimal，若是回傳DefaultValue；若否則回傳Src本身
        ''' </summary>
        ''' <param name="Src">來源數值</param>
        ''' <param name="DefaultValue">預設值</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function IsDecimalNull(ByVal Src As Object, Optional ByVal DefaultValue As Decimal = 0) As Decimal
            If Src Is Nothing OrElse IsDBNull(Src) Then
                Return DefaultValue
            Else
                If TypeOf Src Is Decimal Then
                    Return Src
                Else
                    Return DefaultValue
                End If
            End If
        End Function
#End Region

#Region "ExtractErrMsg：錯誤訊息解析"
        'Param:額外要寫入Log的訊息
        Public Shared Function ExtractErrMsg(ByVal ex As Exception, ByVal LogFilePath As String, ByVal Source As String, ByVal ParamArray Param As Object()) As String
            Dim strMsg As New System.Text.StringBuilder
            Dim strLogMsg As New System.Text.StringBuilder
            Dim strOtherMsg As String = ""
            Dim strKey As String = Now.ToString("yyyyMMddHHmmss") & Now.Millisecond.ToString("000")

            If Param.Length > 0 Then
                For intLoop As Integer = 0 To Param.Length - 1
                    If strOtherMsg <> "" Then strOtherMsg &= ", "
                    strOtherMsg &= Param(intLoop).ToString()
                Next
            End If

            strLogMsg.AppendLine(String.Format("Source:[{0}], KeyID={1}", Source, strKey))
            If TypeOf ex Is System.Data.SqlClient.SqlException Then
                For Each er As SqlClient.SqlError In CType(ex, SqlClient.SqlException).Errors
                    strLogMsg.AppendLine(er.ToString())

                    strMsg.AppendLine(er.Message.ToString())
                Next
            Else
                strLogMsg.AppendLine(ex.ToString())
                strMsg.AppendLine(ex.Message)
            End If
            strMsg.AppendLine("(" & strKey & ")")
            WriteLog(IIf(strOtherMsg = "", strLogMsg.ToString(), strLogMsg.ToString() & strOtherMsg), LogFilePath)

            Return "[" & Source & "]：" & strMsg.ToString()
        End Function
#End Region

#Region "Write Log"
        Public Shared Sub WriteLog(ByVal LogMessage As String, Optional ByVal LogFilePath As String = "")
            WriteLog(LogMessage, LogType.Information, MyLogger)
        End Sub

        Public Shared Sub WriteLog(ByVal LogMessage As String, ByVal LT As LogType, ByVal LGR As Logger)
            Select Case LT
                Case LogType.Debug
                    LGR.Debug(LogMessage)
                Case LogType.Error
                    LGR.Error(LogMessage)
                Case LogType.Information
                    LGR.Info(LogMessage)
                Case LogType.Trace
                    LGR.Trace(LogMessage)
                Case LogType.Warn
                    LGR.Warn(LogMessage)
            End Select
        End Sub
#End Region

#Region "其他類別"
        ''' <summary>
        ''' 取得唯一的檔名, 以日期時間為依據來產生檔名
        ''' </summary>
        ''' <param name="sKind"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetNewFileName(ByVal sKind As String) As String
            Randomize()
            Dim intRandomNumber As Integer = Rnd() * 100000
            Return sKind & Now.ToString("yyyyMMddHHmmss") & Now.Millisecond.ToString("000") & intRandomNumber.ToString()
        End Function

#End Region
#Region "取得最大撈取筆數上限"
        ''' <summary>
        ''' 取得最大撈取筆數上限
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function QueryLimit() As String
            If HttpContext.Current.Application.Item("QueryLimit") Is Nothing OrElse _
                HttpContext.Current.Application.Item("QueryLimit").ToString() = "" Then
                'Dim objQR As New QR()
                Dim strLimit As String = ""

                'strLimit = objQR.GetQryLimit()

                If strLimit Is Nothing OrElse strLimit = "" Then
                    strLimit = "200"
                End If
                HttpContext.Current.Application.Item("QueryLimit") = String.Format("{0},{1}", Date.Now.ToString("yyyy/MM/dd HH:mm:ss"), strLimit)
                Return strLimit
            Else
                Dim aryLimit() As String = HttpContext.Current.Application.Item("QueryLimit").ToString().Split(",")

                Try
                    If DateDiff(DateInterval.Second, Convert.ToDateTime(aryLimit(0)), Date.Now) > 3600 Then
                        HttpContext.Current.Application.Remove("QueryLimit")
                        Return QueryLimit()
                    Else
                        Return aryLimit(1)
                    End If
                Catch ex As Exception
                    HttpContext.Current.Application.Remove("QueryLimit")
                    Return QueryLimit()
                End Try
            End If
        End Function
#End Region

#Region "系統別"
        Public Shared Function subGetSysID(ByVal LoginSysID As String) As String
            Dim strSQL As String
            Dim objSC As New SC

            'strSQL = "select S.SysID + ' - ' + S.SysName as SysID from SC_Admin A left join SC_Sys S on A.SysID = S.SysID  Where 1 = 1 And AdminID = " & UserProfile.ActUserID
            strSQL = "select SysID + '-' + SysName as SysName from SC_Sys where SysID = " & Bsp.Utility.Quote(UserProfile.LoginSysID)
            Return Bsp.DB.ExecuteScalar(strSQL.ToString())
            'Return Bsp.Utility.Quote(ScanID)

        End Function

        'SC0100_讀取員工姓名
        Public Shared Function subUserName(ByVal CompID As String, ByVal UserID As String) As String
            Dim strSQL As String
            Dim objSC As New SC

            strSQL = "select UserName from SC_User Where 1 = 1 and CompID = " & Bsp.Utility.Quote(CompID) & " And UserID = " & Bsp.Utility.Quote(UserID)
            Return Bsp.DB.ExecuteScalar(strSQL.ToString())
            'Return Bsp.Utility.Quote(ScanID)

        End Function

        'SC0100_讀取任職狀態
        Public Shared Function subWorkStatus(ByVal CompID As String, ByVal UserID As String) As String
            Dim strSQL As String
            Dim objSC As New SC

            strSQL = "Select W.Remark From SC_User S left join eHRMSDB_ITRD.dbo.WorkStatus W on S.WorkStatus = W.WorkCode where 1=1 and S.CompID = " & Bsp.Utility.Quote(CompID) & " and S.UserID = " & Bsp.Utility.Quote(UserID)

            Return Bsp.DB.ExecuteScalar(strSQL.ToString())
            'Return Bsp.Utility.Quote(ScanID)

        End Function

        'SC0100_讀取部門
        Public Shared Function subDeptName(ByVal CompID As String, ByVal UserID As String) As String
            Dim strSQL As String
            Dim objSC As New SC

            strSQL = " Select S.DeptID + '-' + O2.OrganName as DeptID "
            strSQL = strSQL & " From SC_User S "
            strSQL = strSQL & " left join SC_Company C on C.CompID = S.CompID "
            strSQL = strSQL & " left join SC_Organization O1 on S.CompID = O1.CompID and S.DeptID = O1.DeptID and S.OrganID = O1.OrganID "
            strSQL = strSQL & " left join SC_Organization O2 on S.CompID = O2.CompID and S.DeptID = O2.DeptID and S.DeptID = O2.OrganID "
            strSQL = strSQL & " where 1 = 1 And S.CompID = " & Bsp.Utility.Quote(CompID) & " And S.UserID = " & Bsp.Utility.Quote(UserID)

            Return Bsp.DB.ExecuteScalar(strSQL.ToString())
            'Return Bsp.Utility.Quote(ScanID)

        End Function

        'SC0100_讀取科組課
        Public Shared Function subOrganName(ByVal CompID As String, ByVal UserID As String) As String
            Dim strSQL As String
            Dim objSC As New SC

            strSQL = " Select S.OrganID + '-' + O1.OrganName as OrganID "
            strSQL = strSQL & " From SC_User S "
            strSQL = strSQL & " left join SC_Company C on C.CompID = S.CompID "
            strSQL = strSQL & " left join SC_Organization O1 on S.CompID = O1.CompID and S.DeptID = O1.DeptID and S.OrganID = O1.OrganID "
            strSQL = strSQL & " left join SC_Organization O2 on S.CompID = O2.CompID and S.DeptID = O2.DeptID and S.DeptID = O2.OrganID "
            strSQL = strSQL & " where 1 = 1 And S.CompID = " & Bsp.Utility.Quote(CompID) & " And S.UserID = " & Bsp.Utility.Quote(UserID)

            Return Bsp.DB.ExecuteScalar(strSQL.ToString())
            'Return Bsp.Utility.Quote(ScanID)

        End Function

        'SC0100_讀取職稱
        Public Shared Function subTitle(ByVal CompID As String, ByVal UserID As String) As String
            Dim strSQL As String
            Dim objSC As New SC

            strSQL = "Select IsNull(T.TitleName, '') From SC_User S left join eHRMSDB_ITRD.dbo.Title T on S.CompID = T.CompID and S.TitleID = T.TitleID and S.RankID = T.RankID where 1=1 and S.CompID = " & Bsp.Utility.Quote(CompID) & " and S.UserID = " & Bsp.Utility.Quote(UserID)

            Return Bsp.DB.ExecuteScalar(strSQL.ToString())
            'Return Bsp.Utility.Quote(ScanID)

        End Function

        'SC0100_讀取禁用註記
        Public Shared Function subBankMark(ByVal CompID As String, ByVal UserID As String) As String
            Dim strSQL As String
            Dim objSC As New SC

            strSQL = "select BanMark,BanMarkValidDate from SC_User Where 1 = 1 and CompID = " & Bsp.Utility.Quote(CompID) & " And UserID = " & Bsp.Utility.Quote(UserID)
            Return Bsp.DB.ExecuteScalar(strSQL.ToString())
            'Return Bsp.Utility.Quote(ScanID)

        End Function

        Public Shared Function subGetSysIDs(ByVal ActUserID As String) As String
            Dim strSQL As String
            Dim objSC As New SC

            strSQL = "select S.SysID from SC_Admin A left join SC_Sys S on A.SysID = S.SysID  Where 1 = 1 And AdminID = " & UserProfile.ActUserID
            Return Bsp.DB.ExecuteScalar(strSQL.ToString())
            'Return Bsp.Utility.Quote(ScanID)

        End Function

        Public Shared Function subGetGroupName(ByVal CompRoleID As String, ByVal GroupID As String) As String
            Dim strSQL As String
            Dim objSC As New SC
            strSQL = "select GroupID + '-' + GroupName from SC_Group where CompRoleID = " & Bsp.Utility.Quote(CompRoleID) & " and GroupID = " & Bsp.Utility.Quote(GroupID)
            Return Bsp.DB.ExecuteScalar(strSQL.ToString())
            'Return Bsp.Utility.Quote(ScanID)

        End Function
#End Region

#Region "公司別"
        Public Shared Function subGetFullComp(ByVal ActUserID As String) As String
            Dim strSQL As String
            Dim objSC As New SC

            strSQL = "select CompID + ' - ' + CompName as CompID from SC_Company"
            Return Bsp.DB.ExecuteScalar(strSQL.ToString())
            'Return Bsp.Utility.Quote(ScanID)

        End Function
#End Region
#Region "fillCompany：填入公司資料"
        Public Shared Sub FillCompany(ByVal objDDL As DropDownList)
            FillCompany(objDDL, Enums.FullNameType.OnlyDefine)

        End Sub

        Public Shared Sub FillCompany(ByVal objDDL As DropDownList, ByVal ShowType As Bsp.Enums.FullNameType)
            Dim objSC As New SC
            Dim CondStr As String = ""
            Try
                Using dt As DataTable = objSC.GetCompanyInfo("", "CompID, CompName, CompID + '-' + CompName CompFullName", CondStr)
                    With objDDL
                        .Items.Clear()
                        .DataSource = dt
                        Select Case ShowType
                            Case Bsp.Enums.FullNameType.CodeDefine
                                .DataTextField = "CompFullName"
                            Case Bsp.Enums.FullNameType.OnlyCode
                                .DataTextField = "CompID"
                            Case Bsp.Enums.FullNameType.OnlyDefine
                                .DataTextField = "CompName"
                        End Select
                        .DataValueField = "CompID"
                        .DataBind()
                    End With
                End Using
            Catch ex As Exception
                Throw
            End Try
        End Sub

#End Region

#Region "fillOrganization：填入部門資料"
        Public Shared Sub FillOrganization(ByVal objDDL As DropDownList, ByVal strCompID As String, ByVal strShowInValidOrgan As String)
            FillOrganization(objDDL, Enums.FullNameType.OnlyDefine, strCompID, strShowInValidOrgan)
        End Sub

        Public Shared Sub FillOrganization(ByVal objDDL As DropDownList, ByVal ShowType As Bsp.Enums.FullNameType, ByVal strCompID As String, ByVal strShowInValidOrgan As String)
            Dim objSC As New SC
            Dim objUC As New UC
            Dim CondStr As String = ""
            Dim strTableName As String = "Organization"
            Dim strField As String
            Dim strWhere As String

            strWhere = "And CompID = '" & strCompID & "' "
            strWhere &= "And OrganID = DeptID "

            '2016/04/29 SPHBKC資料已併入Organization中
            'If strCompID <> "SPHBKC" Then
            '    strTableName = "Organization"
            'Else
            '    strTableName = "COrganization"
            'End If

            If strShowInValidOrgan = "N" Then
                strField = "OrganID, OrganName, OrganID + '-' + OrganName as FullName"
                strWhere &= "And InValidFlag = '0'" '只顯示有效單位
            Else
                strField = "OrganID, OrganName + case when InValidFlag='0'then '' else '(無效)' end as OrganName, OrganID + '-' + OrganName + case when InValidFlag='0'then '' else '(無效)' end as FullName"
            End If

            Try
                Using dt As DataTable = objUC.GetHROrganInfo(strTableName, "", strField, strWhere)
                    With objDDL
                        .Items.Clear()
                        .DataSource = dt
                        Select Case ShowType
                            Case Bsp.Enums.FullNameType.CodeDefine
                                .DataTextField = "FullName"
                            Case Bsp.Enums.FullNameType.OnlyCode
                                .DataTextField = "OrganID"
                            Case Bsp.Enums.FullNameType.OnlyDefine
                                .DataTextField = "OrganName"
                        End Select
                        .DataValueField = "OrganID"
                        .DataBind()
                    End With
                End Using
            Catch ex As Exception
                Throw
            End Try
        End Sub
		Public Shared Sub FillHRCompany(ByVal objDDL As DropDownList)
            FillCompany(objDDL, Enums.FullNameType.OnlyDefine)
        End Sub

        Public Shared Sub FillHRCompany(ByVal objDDL As DropDownList, ByVal ShowType As Bsp.Enums.FullNameType)
            Dim objHR As New HR '//App_Code/Business/HR.vb的class HR
            Dim CondStr As String = ""

            Try
                Using dt As DataTable = objHR.GetHRCompanyInfo("", "CompID, CompName, CompID + '-' + CompName CompFullName", CondStr)
                    '//GetHRCompanyInfo的回傳值 Bsp.DB.ExecuteDataSet(CommandType.Text, strSQL.ToString(), "eHRMSDB").Tables(0)
                    '//ExecuteDataSet(3引數As String){return ExecuteDataSet(4引數)}   //有另一個(3引數 As 布林)
                    With objDDL
                        .Items.Clear()
                        .DataSource = dt
                        Select Case ShowType
                            Case Bsp.Enums.FullNameType.CodeDefine
                                .DataTextField = "CompFullName"
                            Case Bsp.Enums.FullNameType.OnlyCode
                                .DataTextField = "CompID"
                            Case Bsp.Enums.FullNameType.OnlyDefine
                                .DataTextField = "CompName"
                        End Select
                        .DataValueField = "CompID"
                        .DataBind()
                    End With
                End Using
            Catch ex As Exception
                Throw
            End Try
        End Sub
#End Region
#Region "Rank"
        Public Shared Sub Rank(ByVal objDDL As DropDownList, ByVal CompID As String, Optional ByVal Type As Bsp.Enums.DisplayType = Bsp.Enums.DisplayType.Full)
            Dim objHR As New HR

            Try
                Using dt As DataTable = objHR.GetRankIDInfo(CompID)
                    With objDDL
                        .Items.Clear()
                        .DataSource = dt
                        .DataTextField = "RankID"
                        .DataValueField = "RankID"
                        .DataBind()
                    End With
                End Using
            Catch ex As Exception
                Throw ex
            End Try
        End Sub

#End Region

#Region "WorkSite"
        Public Shared Sub WorkSite(ByVal objDDL As DropDownList, ByVal CompID As String, ByVal OrganID As String, Optional ByVal Type As Bsp.Enums.DisplayType = Bsp.Enums.DisplayType.Full)
            Dim obHR As New HR
            Try
                Using dt As DataTable = obHR.GetWorkSite(CompID, OrganID)
                    With objDDL
                        .Items.Clear()
                        .DataSource = dt
                        .DataTextField = "FullName"
                        .DataValueField = "WorkSiteID"
                        .DataBind()
                    End With
                End Using
            Catch ex As Exception
                Throw ex
            End Try
        End Sub
#End Region

#Region "WorType"
        Public Shared Sub WorkType(ByVal objDDL As DropDownList, ByVal WorkTypeID As String, Optional ByVal Type As Bsp.Enums.DisplayType = Bsp.Enums.DisplayType.Full, Optional ByVal CondStr As String = "")
            Dim objUC As New UC

            Try
                Using dt As DataTable = objUC.GetWorkTypeByUC("", "", CondStr)
                    With objDDL
                        .Items.Clear()
                        .DataSource = dt
                        Select Case Type
                            Case Bsp.Enums.DisplayType.OnlyID
                                .DataTextField = "WorkTypeID"
                            Case Bsp.Enums.DisplayType.OnlyName
                                .DataTextField = "Remark"
                            Case Else
                                .DataTextField = "FullName"
                        End Select
                        .DataValueField = "WorkTypeID"
                        .DataBind()
                    End With
                End Using
            Catch ex As Exception
                Throw ex
            End Try
        End Sub
        Public Shared Sub CWorkType(ByVal objDDL As DropDownList, ByVal WorkTypeID As String, Optional ByVal Type As Bsp.Enums.DisplayType = Bsp.Enums.DisplayType.Full, Optional ByVal CondStr As String = "")
            Dim objUC As New UC

            Try
                Using dt As DataTable = objUC.GetCWorkTypeByUC("", "", CondStr)
                    With objDDL
                        .Items.Clear()
                        .DataSource = dt
                        Select Case Type
                            Case Bsp.Enums.DisplayType.OnlyID
                                .DataTextField = "WorkTypeID"
                            Case Bsp.Enums.DisplayType.OnlyName
                                .DataTextField = "Remark"
                            Case Else
                                .DataTextField = "FullName"
                        End Select
                        .DataValueField = "WorkTypeID"
                        .DataBind()
                    End With
                End Using
            Catch ex As Exception
                Throw ex
            End Try
        End Sub
#End Region
#Region "Position"
        Public Shared Sub Position(ByVal objDDL As DropDownList, ByVal PositionID As String, Optional ByVal Type As Bsp.Enums.DisplayType = Bsp.Enums.DisplayType.Full, Optional ByVal CondStr As String = "")
            Dim objUC As New UC

            Try
                Using dt As DataTable = objUC.GetPositionByUC("", "", CondStr)
                    With objDDL
                        .Items.Clear()
                        .DataSource = dt
                        Select Case Type
                            Case Bsp.Enums.DisplayType.OnlyID
                                .DataTextField = "PositionID"
                            Case Bsp.Enums.DisplayType.OnlyName
                                .DataTextField = "Remark"
                            Case Else
                                .DataTextField = "FullName"
                        End Select
                        .DataValueField = "PositionID"
                        .DataBind()
                    End With
                End Using
            Catch ex As Exception
                Throw ex
            End Try
        End Sub
#End Region
#Region "fillEducation：填入學歷資料"
        Public Shared Sub FillEduID(ByVal objDDL As DropDownList)
            FillEduID(objDDL, Enums.FullNameType.OnlyDefine)
        End Sub

        Public Shared Sub FillEduID(ByVal objDDL As DropDownList, ByVal ShowType As Bsp.Enums.FullNameType)
           Dim strSQL As New StringBuilder

            strSQL.AppendLine("Select RTrim(EduID) as EduID,EduName,RTrim(EduID) + '-' + EduName as FullName from EduDegree Order by EduID")

            Try
                Using dt As DataTable = Bsp.DB.ExecuteDataSet(CommandType.Text, strSQL.ToString, "eHRMSDB").Tables(0)
                    With objDDL
                        .Items.Clear()
                        .DataTextField = "FullName"
                        .DataValueField = "EduID"
                        .DataSource = dt
                        .DataBind()
                    End With
                End Using
            Catch ex As Exception
                Throw ex
            End Try
        End Sub
#End Region
#Region "FillSchoolType：填入學校類別"
        Public Shared Sub FillSchoolType(ByVal objDDL As DropDownList)
            FillSchoolType(objDDL, Enums.FullNameType.OnlyDefine)
        End Sub

        Public Shared Sub FillSchoolType(ByVal objDDL As DropDownList, ByVal ShowType As Bsp.Enums.FullNameType)
            Dim strSQL As New StringBuilder

            strSQL.AppendLine("Select RTrim(Code) as Code,CodeCName,RTrim(Code) + '-' + CodeCName as FullName from HRCodeMap where TabName='Education' and FldName='SchoolType' and NotShowFlag='0' order by SortFld")

            Try
                Using dt As DataTable = Bsp.DB.ExecuteDataSet(CommandType.Text, strSQL.ToString, "eHRMSDB").Tables(0)
                    With objDDL
                        .Items.Clear()
                        .DataTextField = "FullName"
                        .DataValueField = "Code"
                        .DataSource = dt
                        .DataBind()
                    End With
                End Using
            Catch ex As Exception
                Throw ex
            End Try
        End Sub
#End Region
#Region "FillRelativeID：填入稱謂"
        Public Shared Sub FillRelativeID(ByVal objDDL As DropDownList)
            FillRelativeID(objDDL, Enums.FullNameType.OnlyDefine)
        End Sub

        Public Shared Sub FillRelativeID(ByVal objDDL As DropDownList, ByVal ShowType As Bsp.Enums.FullNameType)
            Dim strSQL As New StringBuilder

            strSQL.AppendLine("Select RelativeID,Remark,RelativeID + '-' + Remark as FullName  from Relationship where RelativeID <>'00' Order by RelativeID")

            Try
                Using dt As DataTable = Bsp.DB.ExecuteDataSet(CommandType.Text, strSQL.ToString, "eHRMSDB").Tables(0)
                    With objDDL
                        .Items.Clear()
                        .DataTextField = "FullName"
                        .DataValueField = "RelativeID"
                        .DataSource = dt
                        .DataBind()
                    End With
                End Using
            Catch ex As Exception
                Throw ex
            End Try
        End Sub
#End Region
#Region "FillIndustryType：填入眷屬產業別"
        Public Shared Sub FillIndustryType(ByVal objDDL As DropDownList)
            FillIndustryType(objDDL, Enums.FullNameType.OnlyDefine)
        End Sub

        Public Shared Sub FillIndustryType(ByVal objDDL As DropDownList, ByVal ShowType As Bsp.Enums.FullNameType)
            Dim strSQL As New StringBuilder

            strSQL.AppendLine("select RTrim(Code) as Code,CodeCName,RTrim(Code) + '-' + CodeCName as FullName from HRCodeMap where TabName='Experience' and FldName='IndustryType' and NotShowFlag='0' order by SortFld")

            Try
                Using dt As DataTable = Bsp.DB.ExecuteDataSet(CommandType.Text, strSQL.ToString, "eHRMSDB").Tables(0)
                    With objDDL
                        .Items.Clear()
                        .DataTextField = "FullName"
                        .DataValueField = "Code"
                        .DataSource = dt
                        .DataBind()
                    End With
                End Using
            Catch ex As Exception
                Throw ex
            End Try
        End Sub
#End Region

#Region "共用下拉選單"
        ''' <summary>組成下拉選單Function</summary>
        ''' <param name="objDDL">選單物件</param>
        ''' <param name="strConn">Datebase Connection名稱</param>
        ''' <param name="strTabName">Table名稱</param>
        ''' <param name="strValue">Value欄位名稱</param>
        ''' <param name="strText">Text欄位名稱</param>
        ''' <param name="DisplayType">(非必要參數) 選單文字呈現方式，預設為DisplayType.Full</param>
        ''' <param name="JoinStr">(非必要參數) 需額外Join的語法ex."Left Join XXX a On a.OOO = b.OOO"，預設為空值</param>
        ''' <param name="WhereStr">(非必要參數) 需額外Where的語法ex."And XXX=OOO"，預設為空值</param>
        ''' <param name="OrderByStr">(非必要參數) 需額外OrderBy的語法ex."Order By XXX"，預設為Order By strValue</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Sub FillDDL(ByVal objDDL As DropDownList, ByVal strConn As String, ByVal strTabName As String, ByVal strValue As String, ByVal strText As String, Optional ByVal Type As DisplayType = DisplayType.Full, Optional ByVal JoinStr As String = "", Optional ByVal WhereStr As String = "", Optional ByVal OrderByStr As String = "")
            Dim objUt As New Utility()
            Try
                Using dt As DataTable = objUt.GetDDLInfo(strConn, strTabName, strValue, strText, JoinStr, WhereStr, OrderByStr)
                    With objDDL
                        .Items.Clear()
                        .DataSource = dt
                        Select Case Type
                            Case DisplayType.OnlyID
                                .DataTextField = "Code"
                            Case DisplayType.OnlyName
                                .DataTextField = "CodeName"
                            Case Else
                                .DataTextField = "FullName"
                        End Select
                        .DataValueField = "Code"
                        .DataBind()
                    End With
                End Using
            Catch ex As Exception
                Throw
            End Try
        End Sub

        Public Function GetDDLInfo(ByVal strConn As String, ByVal strTabName As String, ByVal strValue As String, ByVal strText As String, Optional ByVal JoinStr As String = "", Optional ByVal WhereStr As String = "", Optional ByVal OrderByStr As String = "") As DataTable
            Dim strSQL As New StringBuilder
            strSQL.AppendLine("Select " & strValue & " AS Code")
            If strText <> "" Then strSQL.AppendLine(", " & strText & " AS CodeName, " & strValue.Replace("distinct", "") & " + '-' + " & strText & " AS FullName ")
            strSQL.AppendLine("FROM " & strTabName)
            If JoinStr <> "" Then strSQL.AppendLine(JoinStr)
            strSQL.AppendLine("Where 1=1")
            If WhereStr <> "" Then strSQL.AppendLine(WhereStr)
            If OrderByStr <> "" Then
                strSQL.AppendLine(OrderByStr)
            Else
                strSQL.AppendLine("Order By " & strValue.Replace("distinct", ""))
            End If
            Return Bsp.DB.ExecuteDataSet(CommandType.Text, strSQL.ToString, strConn).Tables(0)
        End Function
#End Region

    End Class
End Namespace
