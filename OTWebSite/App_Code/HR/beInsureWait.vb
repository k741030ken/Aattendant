﻿'****************************************************************
' Table:InsureWait
' Created Date: 2015.11.27
'****************************************************************/
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports Microsoft.Practices.EnterpriseLibrary.Common
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports System.Data.Common
Imports System.Data

Namespace beInsureWait
    Public Class Table
        Private m_Rows As Rows = New Rows()
        Private m_Fields As String() = { "CompID", "WaitType", "WaitDate", "EmpID", "RelativeIDNo", "Source", "Name", "BirthDate", "Amount", "LabAmount" _
                                    , "RetireAmount", "MthRealWages", "IdentityID", "RelativeID", "Reason", "LabDate", "HeaDate", "InsureUnitID", "InsureUnitIDHea", "TransferDate", "InsureState" _
                                    , "WelfareFlag", "Remark1", "LastChgComp", "LastChgID", "LastChgDate" }
        Private m_Types As System.Type() = { GetType(String), GetType(String), GetType(Date), GetType(String), GetType(String), GetType(String), GetType(String), GetType(Date), GetType(String), GetType(String) _
                                    , GetType(String), GetType(String), GetType(String), GetType(String), GetType(String), GetType(Date), GetType(Date), GetType(Integer), GetType(Integer), GetType(Date), GetType(String) _
                                    , GetType(String), GetType(String), GetType(String), GetType(String), GetType(Date) }
        Private m_PrimaryFields As String() = { "CompID", "WaitType", "WaitDate", "EmpID", "RelativeIDNo" }

        Public ReadOnly Property Rows() As beInsureWait.Rows 
            Get
                Return m_Rows
            End Get
        End Property

        Public ReadOnly Property FieldNames() As String()
            Get
                Return m_Fields
            End Get
        End Property

        Public ReadOnly Property PrimaryFieldNames() As String()
            Get
                Return m_PrimaryFields
            End Get
        End Property

        Public Function IsPrimaryKey(ByVal fieldName As String) As Boolean
            Dim iKeys As IEnumerable(Of String) = From s In m_PrimaryFields Where s.ToString().Equals(fieldName) Select s
            Return IIf(iKeys.Count() > 0, True, False)
        End Function

        Public Sub Dispose()
            m_Rows.Dispose()
        End Sub

        ''' <summary>
        ''' 將DataTable資料轉成entity
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub Transfer2Row(InsureWaitTable As DataTable)
            For Each dr As DataRow In InsureWaitTable.Rows
                m_Rows.Add(New Row(dr))
            Next
        End Sub

        ''' <summary>
        ''' 將Entity的資料轉成DataTable
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Transfer2DataTable() As DataTable
            Dim dt As DataTable = New DataTable()
            Dim dcPrimary As DataColumn() = New DataColumn() {}

            For i As Integer = 0 To m_Fields.Length - 1
                Dim dc As DataColumn = New DataColumn(m_Fields(i), m_Types(i))
                If IsPrimaryKey(m_Fields(i)) Then
                    Array.Resize(Of DataColumn)(dcPrimary, dcPrimary.Length + 1)
                    dcPrimary(dcPrimary.Length - 1) = dc
                End If
            Next

            For i As Integer = 0 To m_Rows.Count - 1
                Dim dr As DataRow = dt.NewRow()

                dr(m_Rows(i).CompID.FieldName) = m_Rows(i).CompID.Value
                dr(m_Rows(i).WaitType.FieldName) = m_Rows(i).WaitType.Value
                dr(m_Rows(i).WaitDate.FieldName) = m_Rows(i).WaitDate.Value
                dr(m_Rows(i).EmpID.FieldName) = m_Rows(i).EmpID.Value
                dr(m_Rows(i).RelativeIDNo.FieldName) = m_Rows(i).RelativeIDNo.Value
                dr(m_Rows(i).Source.FieldName) = m_Rows(i).Source.Value
                dr(m_Rows(i).Name.FieldName) = m_Rows(i).Name.Value
                dr(m_Rows(i).BirthDate.FieldName) = m_Rows(i).BirthDate.Value
                dr(m_Rows(i).Amount.FieldName) = m_Rows(i).Amount.Value
                dr(m_Rows(i).LabAmount.FieldName) = m_Rows(i).LabAmount.Value
                dr(m_Rows(i).RetireAmount.FieldName) = m_Rows(i).RetireAmount.Value
                dr(m_Rows(i).MthRealWages.FieldName) = m_Rows(i).MthRealWages.Value
                dr(m_Rows(i).IdentityID.FieldName) = m_Rows(i).IdentityID.Value
                dr(m_Rows(i).RelativeID.FieldName) = m_Rows(i).RelativeID.Value
                dr(m_Rows(i).Reason.FieldName) = m_Rows(i).Reason.Value
                dr(m_Rows(i).LabDate.FieldName) = m_Rows(i).LabDate.Value
                dr(m_Rows(i).HeaDate.FieldName) = m_Rows(i).HeaDate.Value
                dr(m_Rows(i).InsureUnitID.FieldName) = m_Rows(i).InsureUnitID.Value
                dr(m_Rows(i).InsureUnitIDHea.FieldName) = m_Rows(i).InsureUnitIDHea.Value
                dr(m_Rows(i).TransferDate.FieldName) = m_Rows(i).TransferDate.Value
                dr(m_Rows(i).InsureState.FieldName) = m_Rows(i).InsureState.Value
                dr(m_Rows(i).WelfareFlag.FieldName) = m_Rows(i).WelfareFlag.Value
                dr(m_Rows(i).Remark1.FieldName) = m_Rows(i).Remark1.Value
                dr(m_Rows(i).LastChgComp.FieldName) = m_Rows(i).LastChgComp.Value
                dr(m_Rows(i).LastChgID.FieldName) = m_Rows(i).LastChgID.Value
                dr(m_Rows(i).LastChgDate.FieldName) = m_Rows(i).LastChgDate.Value

                dt.Rows.Add(dr)
            Next

            Return dt
        End Function

    End Class

    Public Class Rows
        Private m_Rows As List(Of Row) = New List(Of Row)()

        Default Public ReadOnly Property Rows(ByVal i As Integer) As Row
            Get
                Return m_Rows(i)
            End Get
        End Property

        Public ReadOnly Property Count() As Integer
            Get
                Return m_Rows.Count
            End Get
        End Property

        Public Sub Add(InsureWaitRow As Row)
            m_Rows.Add(InsureWaitRow)
        End Sub

        Public Sub Remove(InsureWaitRow As Row)
            If m_Rows.IndexOf(InsureWaitRow) >= 0 Then
                m_Rows.Remove(InsureWaitRow)
            End If
        End Sub

        Public Sub Dispose()
            m_Rows.Clear()
        End Sub

    End Class

    Public Class Row

        Private FI_CompID As Field(Of String) = new Field(Of String)("CompID", true)
        Private FI_WaitType As Field(Of String) = new Field(Of String)("WaitType", true)
        Private FI_WaitDate As Field(Of Date) = new Field(Of Date)("WaitDate", true)
        Private FI_EmpID As Field(Of String) = new Field(Of String)("EmpID", true)
        Private FI_RelativeIDNo As Field(Of String) = new Field(Of String)("RelativeIDNo", true)
        Private FI_Source As Field(Of String) = new Field(Of String)("Source", true)
        Private FI_Name As Field(Of String) = new Field(Of String)("Name", true)
        Private FI_BirthDate As Field(Of Date) = new Field(Of Date)("BirthDate", true)
        Private FI_Amount As Field(Of String) = new Field(Of String)("Amount", true)
        Private FI_LabAmount As Field(Of String) = new Field(Of String)("LabAmount", true)
        Private FI_RetireAmount As Field(Of String) = new Field(Of String)("RetireAmount", true)
        Private FI_MthRealWages As Field(Of String) = new Field(Of String)("MthRealWages", true)
        Private FI_IdentityID As Field(Of String) = new Field(Of String)("IdentityID", true)
        Private FI_RelativeID As Field(Of String) = new Field(Of String)("RelativeID", true)
        Private FI_Reason As Field(Of String) = new Field(Of String)("Reason", true)
        Private FI_LabDate As Field(Of Date) = new Field(Of Date)("LabDate", true)
        Private FI_HeaDate As Field(Of Date) = new Field(Of Date)("HeaDate", true)
        Private FI_InsureUnitID As Field(Of Integer) = new Field(Of Integer)("InsureUnitID", true)
        Private FI_InsureUnitIDHea As Field(Of Integer) = new Field(Of Integer)("InsureUnitIDHea", true)
        Private FI_TransferDate As Field(Of Date) = new Field(Of Date)("TransferDate", true)
        Private FI_InsureState As Field(Of String) = new Field(Of String)("InsureState", true)
        Private FI_WelfareFlag As Field(Of String) = new Field(Of String)("WelfareFlag", true)
        Private FI_Remark1 As Field(Of String) = new Field(Of String)("Remark1", true)
        Private FI_LastChgComp As Field(Of String) = new Field(Of String)("LastChgComp", true)
        Private FI_LastChgID As Field(Of String) = new Field(Of String)("LastChgID", true)
        Private FI_LastChgDate As Field(Of Date) = new Field(Of Date)("LastChgDate", true)
        Private m_FieldNames As String() = { "CompID", "WaitType", "WaitDate", "EmpID", "RelativeIDNo", "Source", "Name", "BirthDate", "Amount", "LabAmount" _
                                    , "RetireAmount", "MthRealWages", "IdentityID", "RelativeID", "Reason", "LabDate", "HeaDate", "InsureUnitID", "InsureUnitIDHea", "TransferDate", "InsureState" _
                                    , "WelfareFlag", "Remark1", "LastChgComp", "LastChgID", "LastChgDate" }
        Private m_PrimaryFields As String() = { "CompID", "WaitType", "WaitDate", "EmpID", "RelativeIDNo" }
        Private m_IdentityFields As String() = {  }
        Private m_LoadFromDataRow As Boolean = False

        Private Function GetFieldValue(ByVal fieldName As String) As Object
            Select Case fieldName
                Case "CompID"
                    Return FI_CompID.Value
                Case "WaitType"
                    Return FI_WaitType.Value
                Case "WaitDate"
                    Return FI_WaitDate.Value
                Case "EmpID"
                    Return FI_EmpID.Value
                Case "RelativeIDNo"
                    Return FI_RelativeIDNo.Value
                Case "Source"
                    Return FI_Source.Value
                Case "Name"
                    Return FI_Name.Value
                Case "BirthDate"
                    Return FI_BirthDate.Value
                Case "Amount"
                    Return FI_Amount.Value
                Case "LabAmount"
                    Return FI_LabAmount.Value
                Case "RetireAmount"
                    Return FI_RetireAmount.Value
                Case "MthRealWages"
                    Return FI_MthRealWages.Value
                Case "IdentityID"
                    Return FI_IdentityID.Value
                Case "RelativeID"
                    Return FI_RelativeID.Value
                Case "Reason"
                    Return FI_Reason.Value
                Case "LabDate"
                    Return FI_LabDate.Value
                Case "HeaDate"
                    Return FI_HeaDate.Value
                Case "InsureUnitID"
                    Return FI_InsureUnitID.Value
                Case "InsureUnitIDHea"
                    Return FI_InsureUnitIDHea.Value
                Case "TransferDate"
                    Return FI_TransferDate.Value
                Case "InsureState"
                    Return FI_InsureState.Value
                Case "WelfareFlag"
                    Return FI_WelfareFlag.Value
                Case "Remark1"
                    Return FI_Remark1.Value
                Case "LastChgComp"
                    Return FI_LastChgComp.Value
                Case "LastChgID"
                    Return FI_LastChgID.Value
                Case "LastChgDate"
                    Return FI_LastChgDate.Value
                Case Else
                    Return Nothing
            End Select
        End Function

        Private Sub SetFieldValue(ByVal fieldName As String, ByVal value As Object)
            Select Case fieldName
                Case "CompID"
                    FI_CompID.SetValue(value)
                Case "WaitType"
                    FI_WaitType.SetValue(value)
                Case "WaitDate"
                    FI_WaitDate.SetValue(value)
                Case "EmpID"
                    FI_EmpID.SetValue(value)
                Case "RelativeIDNo"
                    FI_RelativeIDNo.SetValue(value)
                Case "Source"
                    FI_Source.SetValue(value)
                Case "Name"
                    FI_Name.SetValue(value)
                Case "BirthDate"
                    FI_BirthDate.SetValue(value)
                Case "Amount"
                    FI_Amount.SetValue(value)
                Case "LabAmount"
                    FI_LabAmount.SetValue(value)
                Case "RetireAmount"
                    FI_RetireAmount.SetValue(value)
                Case "MthRealWages"
                    FI_MthRealWages.SetValue(value)
                Case "IdentityID"
                    FI_IdentityID.SetValue(value)
                Case "RelativeID"
                    FI_RelativeID.SetValue(value)
                Case "Reason"
                    FI_Reason.SetValue(value)
                Case "LabDate"
                    FI_LabDate.SetValue(value)
                Case "HeaDate"
                    FI_HeaDate.SetValue(value)
                Case "InsureUnitID"
                    FI_InsureUnitID.SetValue(value)
                Case "InsureUnitIDHea"
                    FI_InsureUnitIDHea.SetValue(value)
                Case "TransferDate"
                    FI_TransferDate.SetValue(value)
                Case "InsureState"
                    FI_InsureState.SetValue(value)
                Case "WelfareFlag"
                    FI_WelfareFlag.SetValue(value)
                Case "Remark1"
                    FI_Remark1.SetValue(value)
                Case "LastChgComp"
                    FI_LastChgComp.SetValue(value)
                Case "LastChgID"
                    FI_LastChgID.SetValue(value)
                Case "LastChgDate"
                    FI_LastChgDate.SetValue(value)
            End Select
        End Sub

        Default Public Property Row(ByVal fieldName As String) As Object
            Get
                Return GetFieldValue(fieldName)
            End Get
            Set(ByVal value As Object)
                SetFieldValue(fieldName, value)
            End Set
        End Property

        Default Public Property Row(ByVal idx As Integer) As Object
            Get
                Return GetFieldValue(m_FieldNames(idx))
            End Get
            Set(ByVal value As Object)
                SetFieldValue(m_FieldNames(idx), value)
            End Set
        End Property

        Public ReadOnly Property FieldNames() As String()
            Get
                Return m_FieldNames
            End Get
        End Property

        Public ReadOnly Property FieldCount() As Integer
            Get
                Return m_FieldNames.Length
            End Get
        End Property

        Public Function IsUpdated(ByVal fieldName As String) As Boolean
            Select Case fieldName
                Case "CompID"
                    return FI_CompID.Updated
                Case "WaitType"
                    return FI_WaitType.Updated
                Case "WaitDate"
                    return FI_WaitDate.Updated
                Case "EmpID"
                    return FI_EmpID.Updated
                Case "RelativeIDNo"
                    return FI_RelativeIDNo.Updated
                Case "Source"
                    return FI_Source.Updated
                Case "Name"
                    return FI_Name.Updated
                Case "BirthDate"
                    return FI_BirthDate.Updated
                Case "Amount"
                    return FI_Amount.Updated
                Case "LabAmount"
                    return FI_LabAmount.Updated
                Case "RetireAmount"
                    return FI_RetireAmount.Updated
                Case "MthRealWages"
                    return FI_MthRealWages.Updated
                Case "IdentityID"
                    return FI_IdentityID.Updated
                Case "RelativeID"
                    return FI_RelativeID.Updated
                Case "Reason"
                    return FI_Reason.Updated
                Case "LabDate"
                    return FI_LabDate.Updated
                Case "HeaDate"
                    return FI_HeaDate.Updated
                Case "InsureUnitID"
                    return FI_InsureUnitID.Updated
                Case "InsureUnitIDHea"
                    return FI_InsureUnitIDHea.Updated
                Case "TransferDate"
                    return FI_TransferDate.Updated
                Case "InsureState"
                    return FI_InsureState.Updated
                Case "WelfareFlag"
                    return FI_WelfareFlag.Updated
                Case "Remark1"
                    return FI_Remark1.Updated
                Case "LastChgComp"
                    return FI_LastChgComp.Updated
                Case "LastChgID"
                    return FI_LastChgID.Updated
                Case "LastChgDate"
                    return FI_LastChgDate.Updated
                Case Else
                    Throw New Exception("無此欄位！")
            End Select
        End Function

        Public Function CreateUpdateSQL(ByVal fieldName As String) As Boolean
            Select Case fieldName
                Case "CompID"
                    return FI_CompID.CreateUpdateSQL
                Case "WaitType"
                    return FI_WaitType.CreateUpdateSQL
                Case "WaitDate"
                    return FI_WaitDate.CreateUpdateSQL
                Case "EmpID"
                    return FI_EmpID.CreateUpdateSQL
                Case "RelativeIDNo"
                    return FI_RelativeIDNo.CreateUpdateSQL
                Case "Source"
                    return FI_Source.CreateUpdateSQL
                Case "Name"
                    return FI_Name.CreateUpdateSQL
                Case "BirthDate"
                    return FI_BirthDate.CreateUpdateSQL
                Case "Amount"
                    return FI_Amount.CreateUpdateSQL
                Case "LabAmount"
                    return FI_LabAmount.CreateUpdateSQL
                Case "RetireAmount"
                    return FI_RetireAmount.CreateUpdateSQL
                Case "MthRealWages"
                    return FI_MthRealWages.CreateUpdateSQL
                Case "IdentityID"
                    return FI_IdentityID.CreateUpdateSQL
                Case "RelativeID"
                    return FI_RelativeID.CreateUpdateSQL
                Case "Reason"
                    return FI_Reason.CreateUpdateSQL
                Case "LabDate"
                    return FI_LabDate.CreateUpdateSQL
                Case "HeaDate"
                    return FI_HeaDate.CreateUpdateSQL
                Case "InsureUnitID"
                    return FI_InsureUnitID.CreateUpdateSQL
                Case "InsureUnitIDHea"
                    return FI_InsureUnitIDHea.CreateUpdateSQL
                Case "TransferDate"
                    return FI_TransferDate.CreateUpdateSQL
                Case "InsureState"
                    return FI_InsureState.CreateUpdateSQL
                Case "WelfareFlag"
                    return FI_WelfareFlag.CreateUpdateSQL
                Case "Remark1"
                    return FI_Remark1.CreateUpdateSQL
                Case "LastChgComp"
                    return FI_LastChgComp.CreateUpdateSQL
                Case "LastChgID"
                    return FI_LastChgID.CreateUpdateSQL
                Case "LastChgDate"
                    return FI_LastChgDate.CreateUpdateSQL
                Case Else
                    Throw New Exception("無此欄位！")
            End Select
        End Function

        Public ReadOnly Property PrimaryFieldNames() As String()
            Get
                Return m_PrimaryFields
            End Get
        End Property

        Public Function IsPrimaryKey(ByVal fieldName As String) As Boolean
            Dim iKeys As IEnumerable(Of String) = From s In m_PrimaryFields Where s.ToString().Equals(fieldName) Select s
            Return IIf(iKeys.Count() > 0, True, False)
        End Function

        Public ReadOnly Property IdentityFields()
            Get
                Return m_IdentityFields
            End Get
        End Property

        Public Function IsIdentityField(ByVal fieldName As String) As Boolean
            Dim iKeys As IEnumerable(Of String) = From s In m_IdentityFields Where s.ToString().Equals(fieldName) Select s
            Return IIf(iKeys.Count() > 0, True, False)
        End Function

        Public ReadOnly Property LoadFromDataRow() As Boolean
            Get
                Return m_LoadFromDataRow
            End Get
        End Property

        Public Sub New()
            FI_CompID.SetInitValue("SPHBK1")
            FI_WaitType.SetInitValue("")
            FI_WaitDate.SetInitValue(Convert.ToDateTime("1900/01/01"))
            FI_EmpID.SetInitValue("")
            FI_RelativeIDNo.SetInitValue("")
            FI_Source.SetInitValue(" ")
            FI_Name.SetInitValue(" ")
            FI_BirthDate.SetInitValue(Convert.ToDateTime("1900/01/01"))
            FI_Amount.SetInitValue("")
            FI_LabAmount.SetInitValue("")
            FI_RetireAmount.SetInitValue("")
            FI_MthRealWages.SetInitValue("")
            FI_IdentityID.SetInitValue(" ")
            FI_RelativeID.SetInitValue(" ")
            FI_Reason.SetInitValue(" ")
            FI_LabDate.SetInitValue(Convert.ToDateTime("1900/01/01"))
            FI_HeaDate.SetInitValue(Convert.ToDateTime("1900/01/01"))
            FI_InsureUnitID.SetInitValue(0)
            FI_InsureUnitIDHea.SetInitValue(0)
            FI_TransferDate.SetInitValue(Convert.ToDateTime("1900/01/01"))
            FI_InsureState.SetInitValue("0")
            FI_WelfareFlag.SetInitValue("0")
            FI_Remark1.SetInitValue("")
            FI_LastChgComp.SetInitValue("")
            FI_LastChgID.SetInitValue("")
            FI_LastChgDate.SetInitValue(Convert.ToDateTime("1900/01/01"))
        End Sub

        Public Sub New(ByVal dr As System.Data.DataRow)
            FI_CompID.SetInitValue(dr("CompID"))
            FI_WaitType.SetInitValue(dr("WaitType"))
            FI_WaitDate.SetInitValue(dr("WaitDate"))
            FI_EmpID.SetInitValue(dr("EmpID"))
            FI_RelativeIDNo.SetInitValue(dr("RelativeIDNo"))
            FI_Source.SetInitValue(dr("Source"))
            FI_Name.SetInitValue(dr("Name"))
            FI_BirthDate.SetInitValue(dr("BirthDate"))
            FI_Amount.SetInitValue(dr("Amount"))
            FI_LabAmount.SetInitValue(dr("LabAmount"))
            FI_RetireAmount.SetInitValue(dr("RetireAmount"))
            FI_MthRealWages.SetInitValue(dr("MthRealWages"))
            FI_IdentityID.SetInitValue(dr("IdentityID"))
            FI_RelativeID.SetInitValue(dr("RelativeID"))
            FI_Reason.SetInitValue(dr("Reason"))
            FI_LabDate.SetInitValue(dr("LabDate"))
            FI_HeaDate.SetInitValue(dr("HeaDate"))
            FI_InsureUnitID.SetInitValue(dr("InsureUnitID"))
            FI_InsureUnitIDHea.SetInitValue(dr("InsureUnitIDHea"))
            FI_TransferDate.SetInitValue(dr("TransferDate"))
            FI_InsureState.SetInitValue(dr("InsureState"))
            FI_WelfareFlag.SetInitValue(dr("WelfareFlag"))
            FI_Remark1.SetInitValue(dr("Remark1"))
            FI_LastChgComp.SetInitValue(dr("LastChgComp"))
            FI_LastChgID.SetInitValue(dr("LastChgID"))
            FI_LastChgDate.SetInitValue(dr("LastChgDate"))

            m_LoadFromDataRow = True
        End Sub

        Private Sub ClearUpdated()
            FI_CompID.Updated = False
            FI_WaitType.Updated = False
            FI_WaitDate.Updated = False
            FI_EmpID.Updated = False
            FI_RelativeIDNo.Updated = False
            FI_Source.Updated = False
            FI_Name.Updated = False
            FI_BirthDate.Updated = False
            FI_Amount.Updated = False
            FI_LabAmount.Updated = False
            FI_RetireAmount.Updated = False
            FI_MthRealWages.Updated = False
            FI_IdentityID.Updated = False
            FI_RelativeID.Updated = False
            FI_Reason.Updated = False
            FI_LabDate.Updated = False
            FI_HeaDate.Updated = False
            FI_InsureUnitID.Updated = False
            FI_InsureUnitIDHea.Updated = False
            FI_TransferDate.Updated = False
            FI_InsureState.Updated = False
            FI_WelfareFlag.Updated = False
            FI_Remark1.Updated = False
            FI_LastChgComp.Updated = False
            FI_LastChgID.Updated = False
            FI_LastChgDate.Updated = False
        End Sub

        Public ReadOnly Property CompID As Field(Of String) 
            Get
                Return FI_CompID
            End Get
        End Property

        Public ReadOnly Property WaitType As Field(Of String) 
            Get
                Return FI_WaitType
            End Get
        End Property

        Public ReadOnly Property WaitDate As Field(Of Date) 
            Get
                Return FI_WaitDate
            End Get
        End Property

        Public ReadOnly Property EmpID As Field(Of String) 
            Get
                Return FI_EmpID
            End Get
        End Property

        Public ReadOnly Property RelativeIDNo As Field(Of String) 
            Get
                Return FI_RelativeIDNo
            End Get
        End Property

        Public ReadOnly Property Source As Field(Of String) 
            Get
                Return FI_Source
            End Get
        End Property

        Public ReadOnly Property Name As Field(Of String) 
            Get
                Return FI_Name
            End Get
        End Property

        Public ReadOnly Property BirthDate As Field(Of Date) 
            Get
                Return FI_BirthDate
            End Get
        End Property

        Public ReadOnly Property Amount As Field(Of String) 
            Get
                Return FI_Amount
            End Get
        End Property

        Public ReadOnly Property LabAmount As Field(Of String) 
            Get
                Return FI_LabAmount
            End Get
        End Property

        Public ReadOnly Property RetireAmount As Field(Of String) 
            Get
                Return FI_RetireAmount
            End Get
        End Property

        Public ReadOnly Property MthRealWages As Field(Of String) 
            Get
                Return FI_MthRealWages
            End Get
        End Property

        Public ReadOnly Property IdentityID As Field(Of String) 
            Get
                Return FI_IdentityID
            End Get
        End Property

        Public ReadOnly Property RelativeID As Field(Of String) 
            Get
                Return FI_RelativeID
            End Get
        End Property

        Public ReadOnly Property Reason As Field(Of String) 
            Get
                Return FI_Reason
            End Get
        End Property

        Public ReadOnly Property LabDate As Field(Of Date) 
            Get
                Return FI_LabDate
            End Get
        End Property

        Public ReadOnly Property HeaDate As Field(Of Date) 
            Get
                Return FI_HeaDate
            End Get
        End Property

        Public ReadOnly Property InsureUnitID As Field(Of Integer) 
            Get
                Return FI_InsureUnitID
            End Get
        End Property

        Public ReadOnly Property InsureUnitIDHea As Field(Of Integer) 
            Get
                Return FI_InsureUnitIDHea
            End Get
        End Property

        Public ReadOnly Property TransferDate As Field(Of Date) 
            Get
                Return FI_TransferDate
            End Get
        End Property

        Public ReadOnly Property InsureState As Field(Of String) 
            Get
                Return FI_InsureState
            End Get
        End Property

        Public ReadOnly Property WelfareFlag As Field(Of String) 
            Get
                Return FI_WelfareFlag
            End Get
        End Property

        Public ReadOnly Property Remark1 As Field(Of String) 
            Get
                Return FI_Remark1
            End Get
        End Property

        Public ReadOnly Property LastChgComp As Field(Of String) 
            Get
                Return FI_LastChgComp
            End Get
        End Property

        Public ReadOnly Property LastChgID As Field(Of String) 
            Get
                Return FI_LastChgID
            End Get
        End Property

        Public ReadOnly Property LastChgDate As Field(Of Date) 
            Get
                Return FI_LastChgDate
            End Get
        End Property

    End Class

    Public Class Service
        Public Function DeleteRowByPrimaryKey(ByVal InsureWaitRow As Row) As Integer
            Dim db As Database = DatabaseFactory.CreateDatabase("eHRMSDB")
            Dim dbcmd As DbCommand
            Dim strSQL As StringBuilder = New StringBuilder()

            strSQL.AppendLine("Delete From InsureWait")
            strSQL.AppendLine("Where CompID = @CompID")
            strSQL.AppendLine("And WaitType = @WaitType")
            strSQL.AppendLine("And WaitDate = @WaitDate")
            strSQL.AppendLine("And EmpID = @EmpID")
            strSQL.AppendLine("And RelativeIDNo = @RelativeIDNo")

            dbcmd = db.GetSqlStringCommand(strSQL.ToString())
            db.AddInParameter(dbcmd, "@CompID", DbType.String, InsureWaitRow.CompID.Value)
            db.AddInParameter(dbcmd, "@WaitType", DbType.String, InsureWaitRow.WaitType.Value)
            db.AddInParameter(dbcmd, "@WaitDate", DbType.Date, InsureWaitRow.WaitDate.Value)
            db.AddInParameter(dbcmd, "@EmpID", DbType.String, InsureWaitRow.EmpID.Value)
            db.AddInParameter(dbcmd, "@RelativeIDNo", DbType.String, InsureWaitRow.RelativeIDNo.Value)

            return db.ExecuteNonQuery(dbcmd)
        End Function

        public Function DeleteRowByPrimaryKey(ByVal InsureWaitRow As Row, ByVal tran As DbTransaction) As Integer
            Dim db As Database = DatabaseFactory.CreateDatabase("eHRMSDB")
            Dim dbcmd As DbCommand
            Dim strSQL As StringBuilder = New StringBuilder()

            strSQL.AppendLine("Delete From InsureWait")
            strSQL.AppendLine("Where CompID = @CompID")
            strSQL.AppendLine("And WaitType = @WaitType")
            strSQL.AppendLine("And WaitDate = @WaitDate")
            strSQL.AppendLine("And EmpID = @EmpID")
            strSQL.AppendLine("And RelativeIDNo = @RelativeIDNo")

            dbcmd = db.GetSqlStringCommand(strSQL.ToString())
            db.AddInParameter(dbcmd, "@CompID", DbType.String, InsureWaitRow.CompID.Value)
            db.AddInParameter(dbcmd, "@WaitType", DbType.String, InsureWaitRow.WaitType.Value)
            db.AddInParameter(dbcmd, "@WaitDate", DbType.Date, InsureWaitRow.WaitDate.Value)
            db.AddInParameter(dbcmd, "@EmpID", DbType.String, InsureWaitRow.EmpID.Value)
            db.AddInParameter(dbcmd, "@RelativeIDNo", DbType.String, InsureWaitRow.RelativeIDNo.Value)

            return db.ExecuteNonQuery(dbcmd, tran)
        End Function

        Public Function DeleteRowByPrimaryKey(ByVal InsureWaitRow As Row()) As Integer
            Dim db As Database = DatabaseFactory.CreateDatabase("eHRMSDB")
            Dim dbcmd As DbCommand
            Dim strSQL As StringBuilder = New StringBuilder()
            Dim intRowsAffected As Integer = 0

            strSQL.AppendLine("Delete From InsureWait")
            strSQL.AppendLine("Where CompID = @CompID")
            strSQL.AppendLine("And WaitType = @WaitType")
            strSQL.AppendLine("And WaitDate = @WaitDate")
            strSQL.AppendLine("And EmpID = @EmpID")
            strSQL.AppendLine("And RelativeIDNo = @RelativeIDNo")

            dbcmd = db.GetSqlStringCommand(strSQL.ToString())
            using cn As DbConnection = db.CreateConnection()
                cn.Open()
                Dim tran As DbTransaction = cn.BeginTransaction()
                Dim inTrans As Boolean = True

                Try
                    For Each r As Row In InsureWaitRow
                        dbcmd.Parameters.Clear()
                        db.AddInParameter(dbcmd, "@CompID", DbType.String, r.CompID.Value)
                        db.AddInParameter(dbcmd, "@WaitType", DbType.String, r.WaitType.Value)
                        db.AddInParameter(dbcmd, "@WaitDate", DbType.Date, r.WaitDate.Value)
                        db.AddInParameter(dbcmd, "@EmpID", DbType.String, r.EmpID.Value)
                        db.AddInParameter(dbcmd, "@RelativeIDNo", DbType.String, r.RelativeIDNo.Value)

                        intRowsAffected += db.ExecuteNonQuery(dbcmd, tran)
                    Next
                    tran.Commit()
                    inTrans = false
                Catch ex As Exception
                    If inTrans Then tran.Rollback()
                    Throw
                Finally
                    tran.Dispose()
                    If cn.State = ConnectionState.Open Then cn.Close()
                End Try
            End Using
            Return intRowsAffected
        End Function

        Public Function DeleteRowByPrimaryKey(ByVal InsureWaitRow As Row(), ByVal tran As DbTransaction) As Integer
            Dim db As Database = DatabaseFactory.CreateDatabase("eHRMSDB")
            Dim dbcmd As DbCommand
            Dim strSQL As StringBuilder = New StringBuilder()
            Dim intRowsAffected As Integer = 0

            strSQL.AppendLine("Delete From InsureWait")
            strSQL.AppendLine("Where CompID = @CompID")
            strSQL.AppendLine("And WaitType = @WaitType")
            strSQL.AppendLine("And WaitDate = @WaitDate")
            strSQL.AppendLine("And EmpID = @EmpID")
            strSQL.AppendLine("And RelativeIDNo = @RelativeIDNo")

            dbcmd = db.GetSqlStringCommand(strSQL.ToString())

            For Each r As Row In InsureWaitRow
                dbcmd.Parameters.Clear()
                db.AddInParameter(dbcmd, "@CompID", DbType.String, r.CompID.Value)
                db.AddInParameter(dbcmd, "@WaitType", DbType.String, r.WaitType.Value)
                db.AddInParameter(dbcmd, "@WaitDate", DbType.Date, r.WaitDate.Value)
                db.AddInParameter(dbcmd, "@EmpID", DbType.String, r.EmpID.Value)
                db.AddInParameter(dbcmd, "@RelativeIDNo", DbType.String, r.RelativeIDNo.Value)

                intRowsAffected += db.ExecuteNonQuery(dbcmd, tran)
            Next
            Return intRowsAffected
        End Function

        Public Function QueryByKey(ByVal InsureWaitRow As Row) As DataSet
            Dim db As Database = DatabaseFactory.CreateDatabase("eHRMSDB")
            Dim dbcmd As DbCommand
            Dim strSQL As StringBuilder = New StringBuilder()

            strSQL.AppendLine("Select * From InsureWait")
            strSQL.AppendLine("Where CompID = @CompID")
            strSQL.AppendLine("And WaitType = @WaitType")
            strSQL.AppendLine("And WaitDate = @WaitDate")
            strSQL.AppendLine("And EmpID = @EmpID")
            strSQL.AppendLine("And RelativeIDNo = @RelativeIDNo")

            dbcmd = db.GetSqlStringCommand(strSQL.ToString())
            db.AddInParameter(dbcmd, "@CompID", DbType.String, InsureWaitRow.CompID.Value)
            db.AddInParameter(dbcmd, "@WaitType", DbType.String, InsureWaitRow.WaitType.Value)
            db.AddInParameter(dbcmd, "@WaitDate", DbType.Date, InsureWaitRow.WaitDate.Value)
            db.AddInParameter(dbcmd, "@EmpID", DbType.String, InsureWaitRow.EmpID.Value)
            db.AddInParameter(dbcmd, "@RelativeIDNo", DbType.String, InsureWaitRow.RelativeIDNo.Value)

            Return db.ExecuteDataSet(dbcmd)
        End Function

        Public Function QueryByKey(InsureWaitRow As Row, tran As DbTransaction) As DataSet
            Dim db As Database = DatabaseFactory.CreateDatabase("eHRMSDB")
            Dim dbcmd As DbCommand
            Dim strSQL As StringBuilder = New StringBuilder()

            strSQL.AppendLine("Select * From InsureWait")
            strSQL.AppendLine("Where CompID = @CompID")
            strSQL.AppendLine("And WaitType = @WaitType")
            strSQL.AppendLine("And WaitDate = @WaitDate")
            strSQL.AppendLine("And EmpID = @EmpID")
            strSQL.AppendLine("And RelativeIDNo = @RelativeIDNo")

            dbcmd = db.GetSqlStringCommand(strSQL.ToString())
            db.AddInParameter(dbcmd, "@CompID", DbType.String, InsureWaitRow.CompID.Value)
            db.AddInParameter(dbcmd, "@WaitType", DbType.String, InsureWaitRow.WaitType.Value)
            db.AddInParameter(dbcmd, "@WaitDate", DbType.Date, InsureWaitRow.WaitDate.Value)
            db.AddInParameter(dbcmd, "@EmpID", DbType.String, InsureWaitRow.EmpID.Value)
            db.AddInParameter(dbcmd, "@RelativeIDNo", DbType.String, InsureWaitRow.RelativeIDNo.Value)

            Return db.ExecuteDataSet(dbcmd, tran)
        End Function

        Public Function Update(ByVal InsureWaitRow As Row) As Integer
            Dim db As Database = DatabaseFactory.CreateDatabase("eHRMSDB")
            Dim dbcmd As DbCommand
            Dim strSQL As StringBuilder = New StringBuilder()
            Dim strDot As String = String.Empty

            strSQL.AppendLine("Update InsureWait Set")
            For i As Integer = 0 To InsureWaitRow.FieldNames.Length - 1
                If Not InsureWaitRow.IsIdentityField(InsureWaitRow.FieldNames(i)) AndAlso InsureWaitRow.IsUpdated(InsureWaitRow.FieldNames(i)) AndAlso InsureWaitRow.CreateUpdateSQL(InsureWaitRow.FieldNames(i)) Then
                    strSQL.AppendLine(string.Format("{0}{1} = @{1}", strDot, InsureWaitRow.FieldNames(i)))
                    strDot = ","
                End If
            Next
            If strDot = String.Empty Then Throw New Exception("未異動資料欄位！")
            strSQL.AppendLine("Where CompID = @PKCompID")
            strSQL.AppendLine("And WaitType = @PKWaitType")
            strSQL.AppendLine("And WaitDate = @PKWaitDate")
            strSQL.AppendLine("And EmpID = @PKEmpID")
            strSQL.AppendLine("And RelativeIDNo = @PKRelativeIDNo")

            dbcmd = db.GetSqlStringCommand(strSQL.ToString())
            If InsureWaitRow.CompID.Updated Then db.AddInParameter(dbcmd, "@CompID", DbType.String, InsureWaitRow.CompID.Value)
            If InsureWaitRow.WaitType.Updated Then db.AddInParameter(dbcmd, "@WaitType", DbType.String, InsureWaitRow.WaitType.Value)
            If InsureWaitRow.WaitDate.Updated Then db.AddInParameter(dbcmd, "@WaitDate", DbType.Date, IIf(IsDateTimeNull(InsureWaitRow.WaitDate.Value), Convert.ToDateTime("1900/1/1"), InsureWaitRow.WaitDate.Value))
            If InsureWaitRow.EmpID.Updated Then db.AddInParameter(dbcmd, "@EmpID", DbType.String, InsureWaitRow.EmpID.Value)
            If InsureWaitRow.RelativeIDNo.Updated Then db.AddInParameter(dbcmd, "@RelativeIDNo", DbType.String, InsureWaitRow.RelativeIDNo.Value)
            If InsureWaitRow.Source.Updated Then db.AddInParameter(dbcmd, "@Source", DbType.String, InsureWaitRow.Source.Value)
            If InsureWaitRow.Name.Updated Then db.AddInParameter(dbcmd, "@Name", DbType.String, InsureWaitRow.Name.Value)
            If InsureWaitRow.BirthDate.Updated Then db.AddInParameter(dbcmd, "@BirthDate", DbType.Date, IIf(IsDateTimeNull(InsureWaitRow.BirthDate.Value), Convert.ToDateTime("1900/1/1"), InsureWaitRow.BirthDate.Value))
            If InsureWaitRow.Amount.Updated Then db.AddInParameter(dbcmd, "@Amount", DbType.String, InsureWaitRow.Amount.Value)
            If InsureWaitRow.LabAmount.Updated Then db.AddInParameter(dbcmd, "@LabAmount", DbType.String, InsureWaitRow.LabAmount.Value)
            If InsureWaitRow.RetireAmount.Updated Then db.AddInParameter(dbcmd, "@RetireAmount", DbType.String, InsureWaitRow.RetireAmount.Value)
            If InsureWaitRow.MthRealWages.Updated Then db.AddInParameter(dbcmd, "@MthRealWages", DbType.String, InsureWaitRow.MthRealWages.Value)
            If InsureWaitRow.IdentityID.Updated Then db.AddInParameter(dbcmd, "@IdentityID", DbType.String, InsureWaitRow.IdentityID.Value)
            If InsureWaitRow.RelativeID.Updated Then db.AddInParameter(dbcmd, "@RelativeID", DbType.String, InsureWaitRow.RelativeID.Value)
            If InsureWaitRow.Reason.Updated Then db.AddInParameter(dbcmd, "@Reason", DbType.String, InsureWaitRow.Reason.Value)
            If InsureWaitRow.LabDate.Updated Then db.AddInParameter(dbcmd, "@LabDate", DbType.Date, IIf(IsDateTimeNull(InsureWaitRow.LabDate.Value), Convert.ToDateTime("1900/1/1"), InsureWaitRow.LabDate.Value))
            If InsureWaitRow.HeaDate.Updated Then db.AddInParameter(dbcmd, "@HeaDate", DbType.Date, IIf(IsDateTimeNull(InsureWaitRow.HeaDate.Value), Convert.ToDateTime("1900/1/1"), InsureWaitRow.HeaDate.Value))
            If InsureWaitRow.InsureUnitID.Updated Then db.AddInParameter(dbcmd, "@InsureUnitID", DbType.Int32, InsureWaitRow.InsureUnitID.Value)
            If InsureWaitRow.InsureUnitIDHea.Updated Then db.AddInParameter(dbcmd, "@InsureUnitIDHea", DbType.Int32, InsureWaitRow.InsureUnitIDHea.Value)
            If InsureWaitRow.TransferDate.Updated Then db.AddInParameter(dbcmd, "@TransferDate", DbType.Date, IIf(IsDateTimeNull(InsureWaitRow.TransferDate.Value), Convert.ToDateTime("1900/1/1"), InsureWaitRow.TransferDate.Value))
            If InsureWaitRow.InsureState.Updated Then db.AddInParameter(dbcmd, "@InsureState", DbType.String, InsureWaitRow.InsureState.Value)
            If InsureWaitRow.WelfareFlag.Updated Then db.AddInParameter(dbcmd, "@WelfareFlag", DbType.String, InsureWaitRow.WelfareFlag.Value)
            If InsureWaitRow.Remark1.Updated Then db.AddInParameter(dbcmd, "@Remark1", DbType.String, InsureWaitRow.Remark1.Value)
            If InsureWaitRow.LastChgComp.Updated Then db.AddInParameter(dbcmd, "@LastChgComp", DbType.String, InsureWaitRow.LastChgComp.Value)
            If InsureWaitRow.LastChgID.Updated Then db.AddInParameter(dbcmd, "@LastChgID", DbType.String, InsureWaitRow.LastChgID.Value)
            If InsureWaitRow.LastChgDate.Updated Then db.AddInParameter(dbcmd, "@LastChgDate", DbType.Date, IIf(IsDateTimeNull(InsureWaitRow.LastChgDate.Value), Convert.ToDateTime("1900/1/1"), InsureWaitRow.LastChgDate.Value))
            db.AddInParameter(dbcmd, "@PKCompID", DbType.String, IIf(InsureWaitRow.LoadFromDataRow, InsureWaitRow.CompID.OldValue, InsureWaitRow.CompID.Value))
            db.AddInParameter(dbcmd, "@PKWaitType", DbType.String, IIf(InsureWaitRow.LoadFromDataRow, InsureWaitRow.WaitType.OldValue, InsureWaitRow.WaitType.Value))
            db.AddInParameter(dbcmd, "@PKWaitDate", DbType.Date, IIf(InsureWaitRow.LoadFromDataRow, InsureWaitRow.WaitDate.OldValue, InsureWaitRow.WaitDate.Value))
            db.AddInParameter(dbcmd, "@PKEmpID", DbType.String, IIf(InsureWaitRow.LoadFromDataRow, InsureWaitRow.EmpID.OldValue, InsureWaitRow.EmpID.Value))
            db.AddInParameter(dbcmd, "@PKRelativeIDNo", DbType.String, IIf(InsureWaitRow.LoadFromDataRow, InsureWaitRow.RelativeIDNo.OldValue, InsureWaitRow.RelativeIDNo.Value))

            Return db.ExecuteNonQuery(dbcmd)
        End Function

        Public Function Update(ByVal InsureWaitRow As Row, ByVal tran As DbTransaction) As Integer
            Dim db As Database = DatabaseFactory.CreateDatabase("eHRMSDB")
            Dim dbcmd As DbCommand
            Dim strSQL As StringBuilder = New StringBuilder()
            Dim strDot As String = String.Empty

            strSQL.AppendLine("Update InsureWait Set")
            For i As Integer = 0 To InsureWaitRow.FieldNames.Length - 1
                If Not InsureWaitRow.IsIdentityField(InsureWaitRow.FieldNames(i)) AndAlso InsureWaitRow.IsUpdated(InsureWaitRow.FieldNames(i)) AndAlso InsureWaitRow.CreateUpdateSQL(InsureWaitRow.FieldNames(i)) Then
                    strSQL.AppendLine(string.Format("{0}{1} = @{1}", strDot, InsureWaitRow.FieldNames(i)))
                    strDot = ","
                End If
            Next 
            If strDot = String.Empty Then Throw New Exception("未異動資料欄位！")
            strSQL.AppendLine("Where CompID = @PKCompID")
            strSQL.AppendLine("And WaitType = @PKWaitType")
            strSQL.AppendLine("And WaitDate = @PKWaitDate")
            strSQL.AppendLine("And EmpID = @PKEmpID")
            strSQL.AppendLine("And RelativeIDNo = @PKRelativeIDNo")

            dbcmd = db.GetSqlStringCommand(strSQL.ToString())
            If InsureWaitRow.CompID.Updated Then db.AddInParameter(dbcmd, "@CompID", DbType.String, InsureWaitRow.CompID.Value)
            If InsureWaitRow.WaitType.Updated Then db.AddInParameter(dbcmd, "@WaitType", DbType.String, InsureWaitRow.WaitType.Value)
            If InsureWaitRow.WaitDate.Updated Then db.AddInParameter(dbcmd, "@WaitDate", DbType.Date, IIf(IsDateTimeNull(InsureWaitRow.WaitDate.Value), Convert.ToDateTime("1900/1/1"), InsureWaitRow.WaitDate.Value))
            If InsureWaitRow.EmpID.Updated Then db.AddInParameter(dbcmd, "@EmpID", DbType.String, InsureWaitRow.EmpID.Value)
            If InsureWaitRow.RelativeIDNo.Updated Then db.AddInParameter(dbcmd, "@RelativeIDNo", DbType.String, InsureWaitRow.RelativeIDNo.Value)
            If InsureWaitRow.Source.Updated Then db.AddInParameter(dbcmd, "@Source", DbType.String, InsureWaitRow.Source.Value)
            If InsureWaitRow.Name.Updated Then db.AddInParameter(dbcmd, "@Name", DbType.String, InsureWaitRow.Name.Value)
            If InsureWaitRow.BirthDate.Updated Then db.AddInParameter(dbcmd, "@BirthDate", DbType.Date, IIf(IsDateTimeNull(InsureWaitRow.BirthDate.Value), Convert.ToDateTime("1900/1/1"), InsureWaitRow.BirthDate.Value))
            If InsureWaitRow.Amount.Updated Then db.AddInParameter(dbcmd, "@Amount", DbType.String, InsureWaitRow.Amount.Value)
            If InsureWaitRow.LabAmount.Updated Then db.AddInParameter(dbcmd, "@LabAmount", DbType.String, InsureWaitRow.LabAmount.Value)
            If InsureWaitRow.RetireAmount.Updated Then db.AddInParameter(dbcmd, "@RetireAmount", DbType.String, InsureWaitRow.RetireAmount.Value)
            If InsureWaitRow.MthRealWages.Updated Then db.AddInParameter(dbcmd, "@MthRealWages", DbType.String, InsureWaitRow.MthRealWages.Value)
            If InsureWaitRow.IdentityID.Updated Then db.AddInParameter(dbcmd, "@IdentityID", DbType.String, InsureWaitRow.IdentityID.Value)
            If InsureWaitRow.RelativeID.Updated Then db.AddInParameter(dbcmd, "@RelativeID", DbType.String, InsureWaitRow.RelativeID.Value)
            If InsureWaitRow.Reason.Updated Then db.AddInParameter(dbcmd, "@Reason", DbType.String, InsureWaitRow.Reason.Value)
            If InsureWaitRow.LabDate.Updated Then db.AddInParameter(dbcmd, "@LabDate", DbType.Date, IIf(IsDateTimeNull(InsureWaitRow.LabDate.Value), Convert.ToDateTime("1900/1/1"), InsureWaitRow.LabDate.Value))
            If InsureWaitRow.HeaDate.Updated Then db.AddInParameter(dbcmd, "@HeaDate", DbType.Date, IIf(IsDateTimeNull(InsureWaitRow.HeaDate.Value), Convert.ToDateTime("1900/1/1"), InsureWaitRow.HeaDate.Value))
            If InsureWaitRow.InsureUnitID.Updated Then db.AddInParameter(dbcmd, "@InsureUnitID", DbType.Int32, InsureWaitRow.InsureUnitID.Value)
            If InsureWaitRow.InsureUnitIDHea.Updated Then db.AddInParameter(dbcmd, "@InsureUnitIDHea", DbType.Int32, InsureWaitRow.InsureUnitIDHea.Value)
            If InsureWaitRow.TransferDate.Updated Then db.AddInParameter(dbcmd, "@TransferDate", DbType.Date, IIf(IsDateTimeNull(InsureWaitRow.TransferDate.Value), Convert.ToDateTime("1900/1/1"), InsureWaitRow.TransferDate.Value))
            If InsureWaitRow.InsureState.Updated Then db.AddInParameter(dbcmd, "@InsureState", DbType.String, InsureWaitRow.InsureState.Value)
            If InsureWaitRow.WelfareFlag.Updated Then db.AddInParameter(dbcmd, "@WelfareFlag", DbType.String, InsureWaitRow.WelfareFlag.Value)
            If InsureWaitRow.Remark1.Updated Then db.AddInParameter(dbcmd, "@Remark1", DbType.String, InsureWaitRow.Remark1.Value)
            If InsureWaitRow.LastChgComp.Updated Then db.AddInParameter(dbcmd, "@LastChgComp", DbType.String, InsureWaitRow.LastChgComp.Value)
            If InsureWaitRow.LastChgID.Updated Then db.AddInParameter(dbcmd, "@LastChgID", DbType.String, InsureWaitRow.LastChgID.Value)
            If InsureWaitRow.LastChgDate.Updated Then db.AddInParameter(dbcmd, "@LastChgDate", DbType.Date, IIf(IsDateTimeNull(InsureWaitRow.LastChgDate.Value), Convert.ToDateTime("1900/1/1"), InsureWaitRow.LastChgDate.Value))
            db.AddInParameter(dbcmd, "@PKCompID", DbType.String, IIf(InsureWaitRow.LoadFromDataRow, InsureWaitRow.CompID.OldValue, InsureWaitRow.CompID.Value))
            db.AddInParameter(dbcmd, "@PKWaitType", DbType.String, IIf(InsureWaitRow.LoadFromDataRow, InsureWaitRow.WaitType.OldValue, InsureWaitRow.WaitType.Value))
            db.AddInParameter(dbcmd, "@PKWaitDate", DbType.Date, IIf(InsureWaitRow.LoadFromDataRow, InsureWaitRow.WaitDate.OldValue, InsureWaitRow.WaitDate.Value))
            db.AddInParameter(dbcmd, "@PKEmpID", DbType.String, IIf(InsureWaitRow.LoadFromDataRow, InsureWaitRow.EmpID.OldValue, InsureWaitRow.EmpID.Value))
            db.AddInParameter(dbcmd, "@PKRelativeIDNo", DbType.String, IIf(InsureWaitRow.LoadFromDataRow, InsureWaitRow.RelativeIDNo.OldValue, InsureWaitRow.RelativeIDNo.Value))

            Return db.ExecuteNonQuery(dbcmd, tran)
        End Function

        Public Function Update(ByVal InsureWaitRow As Row()) As Integer
            Dim db As Database = DatabaseFactory.CreateDatabase("eHRMSDB")
            Dim dbcmd As DbCommand
            Dim strSQL As StringBuilder = New StringBuilder()
            Dim intRowsAffected As Integer = 0
            Dim strDot As String

            Using cn As DbConnection = db.CreateConnection()
                cn.Open()
                Dim tran As DbTransaction = cn.BeginTransaction()
                Dim inTrans As Boolean = True

                Try
                    For Each r As Row In InsureWaitRow
                        strDot = String.Empty
                        If strSQL.Length > 0 Then strSQL.Remove(0, strSQL.Length)
                        strSQL.AppendLine("Update InsureWait Set")
                        For i As Integer = 0 To r.FieldNames.Length - 1
                            If Not r.IsIdentityField(r.FieldNames(i)) AndAlso r.IsUpdated(r.FieldNames(i)) AndAlso r.CreateUpdateSQL(r.FieldNames(i)) Then
                                strSQL.AppendLine(String.Format("{0}{1} = @{1}", strDot, r.FieldNames(i)))
                                strDot = ","
                            End If
                        Next
                        If strDot = String.Empty Then Continue For
                        strSQL.AppendLine("Where CompID = @PKCompID")
                        strSQL.AppendLine("And WaitType = @PKWaitType")
                        strSQL.AppendLine("And WaitDate = @PKWaitDate")
                        strSQL.AppendLine("And EmpID = @PKEmpID")
                        strSQL.AppendLine("And RelativeIDNo = @PKRelativeIDNo")

                        dbcmd = db.GetSqlStringCommand(strSQL.ToString())
                        If r.CompID.Updated Then db.AddInParameter(dbcmd, "@CompID", DbType.String, r.CompID.Value)
                        If r.WaitType.Updated Then db.AddInParameter(dbcmd, "@WaitType", DbType.String, r.WaitType.Value)
                        If r.WaitDate.Updated Then db.AddInParameter(dbcmd, "@WaitDate", DbType.Date, IIf(IsDateTimeNull(r.WaitDate.Value), Convert.ToDateTime("1900/1/1"), r.WaitDate.Value))
                        If r.EmpID.Updated Then db.AddInParameter(dbcmd, "@EmpID", DbType.String, r.EmpID.Value)
                        If r.RelativeIDNo.Updated Then db.AddInParameter(dbcmd, "@RelativeIDNo", DbType.String, r.RelativeIDNo.Value)
                        If r.Source.Updated Then db.AddInParameter(dbcmd, "@Source", DbType.String, r.Source.Value)
                        If r.Name.Updated Then db.AddInParameter(dbcmd, "@Name", DbType.String, r.Name.Value)
                        If r.BirthDate.Updated Then db.AddInParameter(dbcmd, "@BirthDate", DbType.Date, IIf(IsDateTimeNull(r.BirthDate.Value), Convert.ToDateTime("1900/1/1"), r.BirthDate.Value))
                        If r.Amount.Updated Then db.AddInParameter(dbcmd, "@Amount", DbType.String, r.Amount.Value)
                        If r.LabAmount.Updated Then db.AddInParameter(dbcmd, "@LabAmount", DbType.String, r.LabAmount.Value)
                        If r.RetireAmount.Updated Then db.AddInParameter(dbcmd, "@RetireAmount", DbType.String, r.RetireAmount.Value)
                        If r.MthRealWages.Updated Then db.AddInParameter(dbcmd, "@MthRealWages", DbType.String, r.MthRealWages.Value)
                        If r.IdentityID.Updated Then db.AddInParameter(dbcmd, "@IdentityID", DbType.String, r.IdentityID.Value)
                        If r.RelativeID.Updated Then db.AddInParameter(dbcmd, "@RelativeID", DbType.String, r.RelativeID.Value)
                        If r.Reason.Updated Then db.AddInParameter(dbcmd, "@Reason", DbType.String, r.Reason.Value)
                        If r.LabDate.Updated Then db.AddInParameter(dbcmd, "@LabDate", DbType.Date, IIf(IsDateTimeNull(r.LabDate.Value), Convert.ToDateTime("1900/1/1"), r.LabDate.Value))
                        If r.HeaDate.Updated Then db.AddInParameter(dbcmd, "@HeaDate", DbType.Date, IIf(IsDateTimeNull(r.HeaDate.Value), Convert.ToDateTime("1900/1/1"), r.HeaDate.Value))
                        If r.InsureUnitID.Updated Then db.AddInParameter(dbcmd, "@InsureUnitID", DbType.Int32, r.InsureUnitID.Value)
                        If r.InsureUnitIDHea.Updated Then db.AddInParameter(dbcmd, "@InsureUnitIDHea", DbType.Int32, r.InsureUnitIDHea.Value)
                        If r.TransferDate.Updated Then db.AddInParameter(dbcmd, "@TransferDate", DbType.Date, IIf(IsDateTimeNull(r.TransferDate.Value), Convert.ToDateTime("1900/1/1"), r.TransferDate.Value))
                        If r.InsureState.Updated Then db.AddInParameter(dbcmd, "@InsureState", DbType.String, r.InsureState.Value)
                        If r.WelfareFlag.Updated Then db.AddInParameter(dbcmd, "@WelfareFlag", DbType.String, r.WelfareFlag.Value)
                        If r.Remark1.Updated Then db.AddInParameter(dbcmd, "@Remark1", DbType.String, r.Remark1.Value)
                        If r.LastChgComp.Updated Then db.AddInParameter(dbcmd, "@LastChgComp", DbType.String, r.LastChgComp.Value)
                        If r.LastChgID.Updated Then db.AddInParameter(dbcmd, "@LastChgID", DbType.String, r.LastChgID.Value)
                        If r.LastChgDate.Updated Then db.AddInParameter(dbcmd, "@LastChgDate", DbType.Date, IIf(IsDateTimeNull(r.LastChgDate.Value), Convert.ToDateTime("1900/1/1"), r.LastChgDate.Value))
                        db.AddInParameter(dbcmd, "@PKCompID", DbType.String, IIf(r.LoadFromDataRow, r.CompID.OldValue, r.CompID.Value))
                        db.AddInParameter(dbcmd, "@PKWaitType", DbType.String, IIf(r.LoadFromDataRow, r.WaitType.OldValue, r.WaitType.Value))
                        db.AddInParameter(dbcmd, "@PKWaitDate", DbType.Date, IIf(r.LoadFromDataRow, r.WaitDate.OldValue, r.WaitDate.Value))
                        db.AddInParameter(dbcmd, "@PKEmpID", DbType.String, IIf(r.LoadFromDataRow, r.EmpID.OldValue, r.EmpID.Value))
                        db.AddInParameter(dbcmd, "@PKRelativeIDNo", DbType.String, IIf(r.LoadFromDataRow, r.RelativeIDNo.OldValue, r.RelativeIDNo.Value))

                        intRowsAffected += db.ExecuteNonQuery(dbcmd, tran)
                    Next
                    tran.Commit()
                Catch ex As Exception
                    If inTrans Then tran.Rollback()
                    Throw
                Finally
                    tran.Dispose()
                    If cn.State = ConnectionState.Open Then cn.Close()
                End Try
            End Using
            Return intRowsAffected
        End Function

        Public Function Update(ByVal InsureWaitRow As Row(), ByVal tran As DbTransaction) As Integer
            Dim db As Database = DatabaseFactory.CreateDatabase("eHRMSDB")
            Dim dbcmd As DbCommand
            Dim strSQL As StringBuilder = New StringBuilder()
            Dim intRowsAffected As Integer = 0
            Dim strDot As String

            For Each r As Row In InsureWaitRow
                strDot = String.Empty
                If strSQL.Length > 0 Then strSQL.Remove(0, strSQL.Length)
                strSQL.AppendLine("Update InsureWait Set")
                For i As Integer = 0 To r.FieldNames.Length - 1
                    If Not r.IsIdentityField(r.FieldNames(i)) AndAlso r.IsUpdated(r.FieldNames(i)) AndAlso r.CreateUpdateSQL(r.FieldNames(i))
                        strSQL.AppendLine(String.Format("{0}{1} = @{1}", strDot, r.FieldNames(i)))
                        strDot = ","
                    End If
                Next
                If strDot = String.Empty Then Continue For
                strSQL.AppendLine("Where CompID = @PKCompID")
                strSQL.AppendLine("And WaitType = @PKWaitType")
                strSQL.AppendLine("And WaitDate = @PKWaitDate")
                strSQL.AppendLine("And EmpID = @PKEmpID")
                strSQL.AppendLine("And RelativeIDNo = @PKRelativeIDNo")

                dbcmd = db.GetSqlStringCommand(strSQL.ToString())
                If r.CompID.Updated Then db.AddInParameter(dbcmd, "@CompID", DbType.String, r.CompID.Value)
                If r.WaitType.Updated Then db.AddInParameter(dbcmd, "@WaitType", DbType.String, r.WaitType.Value)
                If r.WaitDate.Updated Then db.AddInParameter(dbcmd, "@WaitDate", DbType.Date, IIf(IsDateTimeNull(r.WaitDate.Value), Convert.ToDateTime("1900/1/1"), r.WaitDate.Value))
                If r.EmpID.Updated Then db.AddInParameter(dbcmd, "@EmpID", DbType.String, r.EmpID.Value)
                If r.RelativeIDNo.Updated Then db.AddInParameter(dbcmd, "@RelativeIDNo", DbType.String, r.RelativeIDNo.Value)
                If r.Source.Updated Then db.AddInParameter(dbcmd, "@Source", DbType.String, r.Source.Value)
                If r.Name.Updated Then db.AddInParameter(dbcmd, "@Name", DbType.String, r.Name.Value)
                If r.BirthDate.Updated Then db.AddInParameter(dbcmd, "@BirthDate", DbType.Date, IIf(IsDateTimeNull(r.BirthDate.Value), Convert.ToDateTime("1900/1/1"), r.BirthDate.Value))
                If r.Amount.Updated Then db.AddInParameter(dbcmd, "@Amount", DbType.String, r.Amount.Value)
                If r.LabAmount.Updated Then db.AddInParameter(dbcmd, "@LabAmount", DbType.String, r.LabAmount.Value)
                If r.RetireAmount.Updated Then db.AddInParameter(dbcmd, "@RetireAmount", DbType.String, r.RetireAmount.Value)
                If r.MthRealWages.Updated Then db.AddInParameter(dbcmd, "@MthRealWages", DbType.String, r.MthRealWages.Value)
                If r.IdentityID.Updated Then db.AddInParameter(dbcmd, "@IdentityID", DbType.String, r.IdentityID.Value)
                If r.RelativeID.Updated Then db.AddInParameter(dbcmd, "@RelativeID", DbType.String, r.RelativeID.Value)
                If r.Reason.Updated Then db.AddInParameter(dbcmd, "@Reason", DbType.String, r.Reason.Value)
                If r.LabDate.Updated Then db.AddInParameter(dbcmd, "@LabDate", DbType.Date, IIf(IsDateTimeNull(r.LabDate.Value), Convert.ToDateTime("1900/1/1"), r.LabDate.Value))
                If r.HeaDate.Updated Then db.AddInParameter(dbcmd, "@HeaDate", DbType.Date, IIf(IsDateTimeNull(r.HeaDate.Value), Convert.ToDateTime("1900/1/1"), r.HeaDate.Value))
                If r.InsureUnitID.Updated Then db.AddInParameter(dbcmd, "@InsureUnitID", DbType.Int32, r.InsureUnitID.Value)
                If r.InsureUnitIDHea.Updated Then db.AddInParameter(dbcmd, "@InsureUnitIDHea", DbType.Int32, r.InsureUnitIDHea.Value)
                If r.TransferDate.Updated Then db.AddInParameter(dbcmd, "@TransferDate", DbType.Date, IIf(IsDateTimeNull(r.TransferDate.Value), Convert.ToDateTime("1900/1/1"), r.TransferDate.Value))
                If r.InsureState.Updated Then db.AddInParameter(dbcmd, "@InsureState", DbType.String, r.InsureState.Value)
                If r.WelfareFlag.Updated Then db.AddInParameter(dbcmd, "@WelfareFlag", DbType.String, r.WelfareFlag.Value)
                If r.Remark1.Updated Then db.AddInParameter(dbcmd, "@Remark1", DbType.String, r.Remark1.Value)
                If r.LastChgComp.Updated Then db.AddInParameter(dbcmd, "@LastChgComp", DbType.String, r.LastChgComp.Value)
                If r.LastChgID.Updated Then db.AddInParameter(dbcmd, "@LastChgID", DbType.String, r.LastChgID.Value)
                If r.LastChgDate.Updated Then db.AddInParameter(dbcmd, "@LastChgDate", DbType.Date, IIf(IsDateTimeNull(r.LastChgDate.Value), Convert.ToDateTime("1900/1/1"), r.LastChgDate.Value))
                db.AddInParameter(dbcmd, "@PKCompID", DbType.String, IIf(r.LoadFromDataRow, r.CompID.OldValue, r.CompID.Value))
                db.AddInParameter(dbcmd, "@PKWaitType", DbType.String, IIf(r.LoadFromDataRow, r.WaitType.OldValue, r.WaitType.Value))
                db.AddInParameter(dbcmd, "@PKWaitDate", DbType.Date, IIf(r.LoadFromDataRow, r.WaitDate.OldValue, r.WaitDate.Value))
                db.AddInParameter(dbcmd, "@PKEmpID", DbType.String, IIf(r.LoadFromDataRow, r.EmpID.OldValue, r.EmpID.Value))
                db.AddInParameter(dbcmd, "@PKRelativeIDNo", DbType.String, IIf(r.LoadFromDataRow, r.RelativeIDNo.OldValue, r.RelativeIDNo.Value))

                intRowsAffected += db.ExecuteNonQuery(dbcmd, tran)
            Next
            Return intRowsAffected
        End Function

        Public Function IsDataExists(ByVal InsureWaitRow As Row) As Boolean
            Dim db As Database = DatabaseFactory.CreateDatabase("eHRMSDB")
            Dim dbcmd As DbCommand
            Dim strSQL As StringBuilder = New StringBuilder()
            Dim intCount As Integer = 0

            strSQL.AppendLine("Select Count(*) Cnt From InsureWait")
            strSQL.AppendLine("Where CompID = @CompID")
            strSQL.AppendLine("And WaitType = @WaitType")
            strSQL.AppendLine("And WaitDate = @WaitDate")
            strSQL.AppendLine("And EmpID = @EmpID")
            strSQL.AppendLine("And RelativeIDNo = @RelativeIDNo")

            dbcmd = db.GetSqlStringCommand(strSQL.ToString())
            db.AddInParameter(dbcmd, "@CompID", DbType.String, InsureWaitRow.CompID.Value)
            db.AddInParameter(dbcmd, "@WaitType", DbType.String, InsureWaitRow.WaitType.Value)
            db.AddInParameter(dbcmd, "@WaitDate", DbType.Date, InsureWaitRow.WaitDate.Value)
            db.AddInParameter(dbcmd, "@EmpID", DbType.String, InsureWaitRow.EmpID.Value)
            db.AddInParameter(dbcmd, "@RelativeIDNo", DbType.String, InsureWaitRow.RelativeIDNo.Value)

            intCount = Convert.ToInt32(db.ExecuteScalar(dbcmd))
            Return IIf(intCount > 0, True, False)
        End Function

        Public Function IsDataExists(ByVal InsureWaitRow As Row, ByVal tran As DbTransaction) As Boolean
            Dim db As Database = DatabaseFactory.CreateDatabase("eHRMSDB")
            Dim dbcmd As DbCommand
            Dim strSQL As StringBuilder = New StringBuilder()
            Dim intCount As Integer = 0

            strSQL.AppendLine("Select Count(*) Cnt From InsureWait")
            strSQL.AppendLine("Where CompID = @CompID")
            strSQL.AppendLine("And WaitType = @WaitType")
            strSQL.AppendLine("And WaitDate = @WaitDate")
            strSQL.AppendLine("And EmpID = @EmpID")
            strSQL.AppendLine("And RelativeIDNo = @RelativeIDNo")

            dbcmd = db.GetSqlStringCommand(strSQL.ToString())
            db.AddInParameter(dbcmd, "@CompID", DbType.String, InsureWaitRow.CompID.Value)
            db.AddInParameter(dbcmd, "@WaitType", DbType.String, InsureWaitRow.WaitType.Value)
            db.AddInParameter(dbcmd, "@WaitDate", DbType.Date, InsureWaitRow.WaitDate.Value)
            db.AddInParameter(dbcmd, "@EmpID", DbType.String, InsureWaitRow.EmpID.Value)
            db.AddInParameter(dbcmd, "@RelativeIDNo", DbType.String, InsureWaitRow.RelativeIDNo.Value)

            intCount = Convert.ToInt32(db.ExecuteScalar(dbcmd, tran))
            Return IIf(intCount > 0, True, False)
        End Function

        Public Function QuerybyWhere(WhereCondition As String) As DataSet
            Dim db As Database = DatabaseFactory.CreateDatabase("eHRMSDB")
            Dim strSQL As StringBuilder = New StringBuilder()

            strSQL.AppendLine("Select * From InsureWait")
            If WhereCondition <> String.Empty Then strSQL.AppendLine(WhereCondition)

            Return db.ExecuteDataSet(CommandType.Text, strSQL.ToString())
        End Function

        Public Function Insert(ByVal InsureWaitRow As Row) As Integer
            Dim db As Database = DatabaseFactory.CreateDatabase("eHRMSDB")
            Dim dbcmd As DbCommand
            Dim strSQL As StringBuilder = New StringBuilder()

            strSQL.AppendLine("Insert into InsureWait")
            strSQL.AppendLine("(")
            strSQL.AppendLine("    CompID, WaitType, WaitDate, EmpID, RelativeIDNo, Source, Name, BirthDate, Amount,")
            strSQL.AppendLine("    LabAmount, RetireAmount, MthRealWages, IdentityID, RelativeID, Reason, LabDate, HeaDate,")
            strSQL.AppendLine("    InsureUnitID, InsureUnitIDHea, TransferDate, InsureState, WelfareFlag, Remark1, LastChgComp,")
            strSQL.AppendLine("    LastChgID, LastChgDate")
            strSQL.AppendLine(")")
            strSQL.AppendLine("Values")
            strSQL.AppendLine("(")
            strSQL.AppendLine("    @CompID, @WaitType, @WaitDate, @EmpID, @RelativeIDNo, @Source, @Name, @BirthDate, @Amount,")
            strSQL.AppendLine("    @LabAmount, @RetireAmount, @MthRealWages, @IdentityID, @RelativeID, @Reason, @LabDate, @HeaDate,")
            strSQL.AppendLine("    @InsureUnitID, @InsureUnitIDHea, @TransferDate, @InsureState, @WelfareFlag, @Remark1, @LastChgComp,")
            strSQL.AppendLine("    @LastChgID, @LastChgDate")
            strSQL.AppendLine(")")

            dbcmd = db.GetSqlStringCommand(strSQL.ToString())
            db.AddInParameter(dbcmd, "@CompID", DbType.String, InsureWaitRow.CompID.Value)
            db.AddInParameter(dbcmd, "@WaitType", DbType.String, InsureWaitRow.WaitType.Value)
            db.AddInParameter(dbcmd, "@WaitDate", DbType.Date, IIf(IsDateTimeNull(InsureWaitRow.WaitDate.Value), Convert.ToDateTime("1900/1/1"), InsureWaitRow.WaitDate.Value))
            db.AddInParameter(dbcmd, "@EmpID", DbType.String, InsureWaitRow.EmpID.Value)
            db.AddInParameter(dbcmd, "@RelativeIDNo", DbType.String, InsureWaitRow.RelativeIDNo.Value)
            db.AddInParameter(dbcmd, "@Source", DbType.String, InsureWaitRow.Source.Value)
            db.AddInParameter(dbcmd, "@Name", DbType.String, InsureWaitRow.Name.Value)
            db.AddInParameter(dbcmd, "@BirthDate", DbType.Date, IIf(IsDateTimeNull(InsureWaitRow.BirthDate.Value), Convert.ToDateTime("1900/1/1"), InsureWaitRow.BirthDate.Value))
            db.AddInParameter(dbcmd, "@Amount", DbType.String, InsureWaitRow.Amount.Value)
            db.AddInParameter(dbcmd, "@LabAmount", DbType.String, InsureWaitRow.LabAmount.Value)
            db.AddInParameter(dbcmd, "@RetireAmount", DbType.String, InsureWaitRow.RetireAmount.Value)
            db.AddInParameter(dbcmd, "@MthRealWages", DbType.String, InsureWaitRow.MthRealWages.Value)
            db.AddInParameter(dbcmd, "@IdentityID", DbType.String, InsureWaitRow.IdentityID.Value)
            db.AddInParameter(dbcmd, "@RelativeID", DbType.String, InsureWaitRow.RelativeID.Value)
            db.AddInParameter(dbcmd, "@Reason", DbType.String, InsureWaitRow.Reason.Value)
            db.AddInParameter(dbcmd, "@LabDate", DbType.Date, IIf(IsDateTimeNull(InsureWaitRow.LabDate.Value), Convert.ToDateTime("1900/1/1"), InsureWaitRow.LabDate.Value))
            db.AddInParameter(dbcmd, "@HeaDate", DbType.Date, IIf(IsDateTimeNull(InsureWaitRow.HeaDate.Value), Convert.ToDateTime("1900/1/1"), InsureWaitRow.HeaDate.Value))
            db.AddInParameter(dbcmd, "@InsureUnitID", DbType.Int32, InsureWaitRow.InsureUnitID.Value)
            db.AddInParameter(dbcmd, "@InsureUnitIDHea", DbType.Int32, InsureWaitRow.InsureUnitIDHea.Value)
            db.AddInParameter(dbcmd, "@TransferDate", DbType.Date, IIf(IsDateTimeNull(InsureWaitRow.TransferDate.Value), Convert.ToDateTime("1900/1/1"), InsureWaitRow.TransferDate.Value))
            db.AddInParameter(dbcmd, "@InsureState", DbType.String, InsureWaitRow.InsureState.Value)
            db.AddInParameter(dbcmd, "@WelfareFlag", DbType.String, InsureWaitRow.WelfareFlag.Value)
            db.AddInParameter(dbcmd, "@Remark1", DbType.String, InsureWaitRow.Remark1.Value)
            db.AddInParameter(dbcmd, "@LastChgComp", DbType.String, InsureWaitRow.LastChgComp.Value)
            db.AddInParameter(dbcmd, "@LastChgID", DbType.String, InsureWaitRow.LastChgID.Value)
            db.AddInParameter(dbcmd, "@LastChgDate", DbType.Date, IIf(IsDateTimeNull(InsureWaitRow.LastChgDate.Value), Convert.ToDateTime("1900/1/1"), InsureWaitRow.LastChgDate.Value))

            Return db.ExecuteNonQuery(dbcmd)
        End Function

        Public Function Insert(ByVal InsureWaitRow As Row, ByVal tran As DbTransaction) As Integer
            Dim db As Database = DatabaseFactory.CreateDatabase("eHRMSDB")
            Dim dbcmd As DbCommand
            Dim strSQL As StringBuilder = New StringBuilder()

            strSQL.AppendLine("Insert into InsureWait")
            strSQL.AppendLine("(")
            strSQL.AppendLine("    CompID, WaitType, WaitDate, EmpID, RelativeIDNo, Source, Name, BirthDate, Amount,")
            strSQL.AppendLine("    LabAmount, RetireAmount, MthRealWages, IdentityID, RelativeID, Reason, LabDate, HeaDate,")
            strSQL.AppendLine("    InsureUnitID, InsureUnitIDHea, TransferDate, InsureState, WelfareFlag, Remark1, LastChgComp,")
            strSQL.AppendLine("    LastChgID, LastChgDate")
            strSQL.AppendLine(")")
            strSQL.AppendLine("Values")
            strSQL.AppendLine("(")
            strSQL.AppendLine("    @CompID, @WaitType, @WaitDate, @EmpID, @RelativeIDNo, @Source, @Name, @BirthDate, @Amount,")
            strSQL.AppendLine("    @LabAmount, @RetireAmount, @MthRealWages, @IdentityID, @RelativeID, @Reason, @LabDate, @HeaDate,")
            strSQL.AppendLine("    @InsureUnitID, @InsureUnitIDHea, @TransferDate, @InsureState, @WelfareFlag, @Remark1, @LastChgComp,")
            strSQL.AppendLine("    @LastChgID, @LastChgDate")
            strSQL.AppendLine(")")

            dbcmd = db.GetSqlStringCommand(strSQL.ToString())
            db.AddInParameter(dbcmd, "@CompID", DbType.String, InsureWaitRow.CompID.Value)
            db.AddInParameter(dbcmd, "@WaitType", DbType.String, InsureWaitRow.WaitType.Value)
            db.AddInParameter(dbcmd, "@WaitDate", DbType.Date, IIf(IsDateTimeNull(InsureWaitRow.WaitDate.Value), Convert.ToDateTime("1900/1/1"), InsureWaitRow.WaitDate.Value))
            db.AddInParameter(dbcmd, "@EmpID", DbType.String, InsureWaitRow.EmpID.Value)
            db.AddInParameter(dbcmd, "@RelativeIDNo", DbType.String, InsureWaitRow.RelativeIDNo.Value)
            db.AddInParameter(dbcmd, "@Source", DbType.String, InsureWaitRow.Source.Value)
            db.AddInParameter(dbcmd, "@Name", DbType.String, InsureWaitRow.Name.Value)
            db.AddInParameter(dbcmd, "@BirthDate", DbType.Date, IIf(IsDateTimeNull(InsureWaitRow.BirthDate.Value), Convert.ToDateTime("1900/1/1"), InsureWaitRow.BirthDate.Value))
            db.AddInParameter(dbcmd, "@Amount", DbType.String, InsureWaitRow.Amount.Value)
            db.AddInParameter(dbcmd, "@LabAmount", DbType.String, InsureWaitRow.LabAmount.Value)
            db.AddInParameter(dbcmd, "@RetireAmount", DbType.String, InsureWaitRow.RetireAmount.Value)
            db.AddInParameter(dbcmd, "@MthRealWages", DbType.String, InsureWaitRow.MthRealWages.Value)
            db.AddInParameter(dbcmd, "@IdentityID", DbType.String, InsureWaitRow.IdentityID.Value)
            db.AddInParameter(dbcmd, "@RelativeID", DbType.String, InsureWaitRow.RelativeID.Value)
            db.AddInParameter(dbcmd, "@Reason", DbType.String, InsureWaitRow.Reason.Value)
            db.AddInParameter(dbcmd, "@LabDate", DbType.Date, IIf(IsDateTimeNull(InsureWaitRow.LabDate.Value), Convert.ToDateTime("1900/1/1"), InsureWaitRow.LabDate.Value))
            db.AddInParameter(dbcmd, "@HeaDate", DbType.Date, IIf(IsDateTimeNull(InsureWaitRow.HeaDate.Value), Convert.ToDateTime("1900/1/1"), InsureWaitRow.HeaDate.Value))
            db.AddInParameter(dbcmd, "@InsureUnitID", DbType.Int32, InsureWaitRow.InsureUnitID.Value)
            db.AddInParameter(dbcmd, "@InsureUnitIDHea", DbType.Int32, InsureWaitRow.InsureUnitIDHea.Value)
            db.AddInParameter(dbcmd, "@TransferDate", DbType.Date, IIf(IsDateTimeNull(InsureWaitRow.TransferDate.Value), Convert.ToDateTime("1900/1/1"), InsureWaitRow.TransferDate.Value))
            db.AddInParameter(dbcmd, "@InsureState", DbType.String, InsureWaitRow.InsureState.Value)
            db.AddInParameter(dbcmd, "@WelfareFlag", DbType.String, InsureWaitRow.WelfareFlag.Value)
            db.AddInParameter(dbcmd, "@Remark1", DbType.String, InsureWaitRow.Remark1.Value)
            db.AddInParameter(dbcmd, "@LastChgComp", DbType.String, InsureWaitRow.LastChgComp.Value)
            db.AddInParameter(dbcmd, "@LastChgID", DbType.String, InsureWaitRow.LastChgID.Value)
            db.AddInParameter(dbcmd, "@LastChgDate", DbType.Date, IIf(IsDateTimeNull(InsureWaitRow.LastChgDate.Value), Convert.ToDateTime("1900/1/1"), InsureWaitRow.LastChgDate.Value))

            Return db.ExecuteNonQuery(dbcmd, tran)
        End Function

        Public Function Insert(ByVal InsureWaitRow As Row()) As Integer
            Dim db As Database = DatabaseFactory.CreateDatabase("eHRMSDB")
            Dim dbcmd As DbCommand
            Dim strSQL As StringBuilder = New StringBuilder()
            Dim intRowsAffected As Integer = 0

            strSQL.AppendLine("Insert into InsureWait")
            strSQL.AppendLine("(")
            strSQL.AppendLine("    CompID, WaitType, WaitDate, EmpID, RelativeIDNo, Source, Name, BirthDate, Amount,")
            strSQL.AppendLine("    LabAmount, RetireAmount, MthRealWages, IdentityID, RelativeID, Reason, LabDate, HeaDate,")
            strSQL.AppendLine("    InsureUnitID, InsureUnitIDHea, TransferDate, InsureState, WelfareFlag, Remark1, LastChgComp,")
            strSQL.AppendLine("    LastChgID, LastChgDate")
            strSQL.AppendLine(")")
            strSQL.AppendLine("Values")
            strSQL.AppendLine("(")
            strSQL.AppendLine("    @CompID, @WaitType, @WaitDate, @EmpID, @RelativeIDNo, @Source, @Name, @BirthDate, @Amount,")
            strSQL.AppendLine("    @LabAmount, @RetireAmount, @MthRealWages, @IdentityID, @RelativeID, @Reason, @LabDate, @HeaDate,")
            strSQL.AppendLine("    @InsureUnitID, @InsureUnitIDHea, @TransferDate, @InsureState, @WelfareFlag, @Remark1, @LastChgComp,")
            strSQL.AppendLine("    @LastChgID, @LastChgDate")
            strSQL.AppendLine(")")

            dbcmd = db.GetSqlStringCommand(strSQL.ToString())
            Using cn As DbConnection = db.CreateConnection()
                cn.Open()
                Dim tran As DbTransaction = cn.BeginTransaction()
                Dim inTrans As Boolean = True

                Try
                    For Each r As Row In InsureWaitRow
                        dbcmd.Parameters.Clear()
                        db.AddInParameter(dbcmd, "@CompID", DbType.String, r.CompID.Value)
                        db.AddInParameter(dbcmd, "@WaitType", DbType.String, r.WaitType.Value)
                        db.AddInParameter(dbcmd, "@WaitDate", DbType.Date, IIf(IsDateTimeNull(r.WaitDate.Value), Convert.ToDateTime("1900/1/1"), r.WaitDate.Value))
                        db.AddInParameter(dbcmd, "@EmpID", DbType.String, r.EmpID.Value)
                        db.AddInParameter(dbcmd, "@RelativeIDNo", DbType.String, r.RelativeIDNo.Value)
                        db.AddInParameter(dbcmd, "@Source", DbType.String, r.Source.Value)
                        db.AddInParameter(dbcmd, "@Name", DbType.String, r.Name.Value)
                        db.AddInParameter(dbcmd, "@BirthDate", DbType.Date, IIf(IsDateTimeNull(r.BirthDate.Value), Convert.ToDateTime("1900/1/1"), r.BirthDate.Value))
                        db.AddInParameter(dbcmd, "@Amount", DbType.String, r.Amount.Value)
                        db.AddInParameter(dbcmd, "@LabAmount", DbType.String, r.LabAmount.Value)
                        db.AddInParameter(dbcmd, "@RetireAmount", DbType.String, r.RetireAmount.Value)
                        db.AddInParameter(dbcmd, "@MthRealWages", DbType.String, r.MthRealWages.Value)
                        db.AddInParameter(dbcmd, "@IdentityID", DbType.String, r.IdentityID.Value)
                        db.AddInParameter(dbcmd, "@RelativeID", DbType.String, r.RelativeID.Value)
                        db.AddInParameter(dbcmd, "@Reason", DbType.String, r.Reason.Value)
                        db.AddInParameter(dbcmd, "@LabDate", DbType.Date, IIf(IsDateTimeNull(r.LabDate.Value), Convert.ToDateTime("1900/1/1"), r.LabDate.Value))
                        db.AddInParameter(dbcmd, "@HeaDate", DbType.Date, IIf(IsDateTimeNull(r.HeaDate.Value), Convert.ToDateTime("1900/1/1"), r.HeaDate.Value))
                        db.AddInParameter(dbcmd, "@InsureUnitID", DbType.Int32, r.InsureUnitID.Value)
                        db.AddInParameter(dbcmd, "@InsureUnitIDHea", DbType.Int32, r.InsureUnitIDHea.Value)
                        db.AddInParameter(dbcmd, "@TransferDate", DbType.Date, IIf(IsDateTimeNull(r.TransferDate.Value), Convert.ToDateTime("1900/1/1"), r.TransferDate.Value))
                        db.AddInParameter(dbcmd, "@InsureState", DbType.String, r.InsureState.Value)
                        db.AddInParameter(dbcmd, "@WelfareFlag", DbType.String, r.WelfareFlag.Value)
                        db.AddInParameter(dbcmd, "@Remark1", DbType.String, r.Remark1.Value)
                        db.AddInParameter(dbcmd, "@LastChgComp", DbType.String, r.LastChgComp.Value)
                        db.AddInParameter(dbcmd, "@LastChgID", DbType.String, r.LastChgID.Value)
                        db.AddInParameter(dbcmd, "@LastChgDate", DbType.Date, IIf(IsDateTimeNull(r.LastChgDate.Value), Convert.ToDateTime("1900/1/1"), r.LastChgDate.Value))

                        intRowsAffected += db.ExecuteNonQuery(dbcmd, tran)
                    Next
                    tran.Commit()
                Catch ex As Exception
                    If inTrans Then tran.Rollback()
                    Throw
                Finally
                    tran.Dispose()
                    If cn.State = ConnectionState.Open Then cn.Close()
                End Try
            End Using
            Return intRowsAffected
        End Function

        Public Function Insert(ByVal InsureWaitRow As Row(), ByVal tran As DbTransaction) As Integer
            Dim db As Database = DatabaseFactory.CreateDatabase("eHRMSDB")
            Dim dbcmd As DbCommand
            Dim strSQL As StringBuilder = New StringBuilder()
            Dim intRowsAffected As Integer = 0

            strSQL.AppendLine("Insert into InsureWait")
            strSQL.AppendLine("(")
            strSQL.AppendLine("    CompID, WaitType, WaitDate, EmpID, RelativeIDNo, Source, Name, BirthDate, Amount,")
            strSQL.AppendLine("    LabAmount, RetireAmount, MthRealWages, IdentityID, RelativeID, Reason, LabDate, HeaDate,")
            strSQL.AppendLine("    InsureUnitID, InsureUnitIDHea, TransferDate, InsureState, WelfareFlag, Remark1, LastChgComp,")
            strSQL.AppendLine("    LastChgID, LastChgDate")
            strSQL.AppendLine(")")
            strSQL.AppendLine("Values")
            strSQL.AppendLine("(")
            strSQL.AppendLine("    @CompID, @WaitType, @WaitDate, @EmpID, @RelativeIDNo, @Source, @Name, @BirthDate, @Amount,")
            strSQL.AppendLine("    @LabAmount, @RetireAmount, @MthRealWages, @IdentityID, @RelativeID, @Reason, @LabDate, @HeaDate,")
            strSQL.AppendLine("    @InsureUnitID, @InsureUnitIDHea, @TransferDate, @InsureState, @WelfareFlag, @Remark1, @LastChgComp,")
            strSQL.AppendLine("    @LastChgID, @LastChgDate")
            strSQL.AppendLine(")")

            dbcmd = db.GetSqlStringCommand(strSQL.ToString())

            For Each r As Row In InsureWaitRow
                dbcmd.Parameters.Clear()
                db.AddInParameter(dbcmd, "@CompID", DbType.String, r.CompID.Value)
                db.AddInParameter(dbcmd, "@WaitType", DbType.String, r.WaitType.Value)
                db.AddInParameter(dbcmd, "@WaitDate", DbType.Date, IIf(IsDateTimeNull(r.WaitDate.Value), Convert.ToDateTime("1900/1/1"), r.WaitDate.Value))
                db.AddInParameter(dbcmd, "@EmpID", DbType.String, r.EmpID.Value)
                db.AddInParameter(dbcmd, "@RelativeIDNo", DbType.String, r.RelativeIDNo.Value)
                db.AddInParameter(dbcmd, "@Source", DbType.String, r.Source.Value)
                db.AddInParameter(dbcmd, "@Name", DbType.String, r.Name.Value)
                db.AddInParameter(dbcmd, "@BirthDate", DbType.Date, IIf(IsDateTimeNull(r.BirthDate.Value), Convert.ToDateTime("1900/1/1"), r.BirthDate.Value))
                db.AddInParameter(dbcmd, "@Amount", DbType.String, r.Amount.Value)
                db.AddInParameter(dbcmd, "@LabAmount", DbType.String, r.LabAmount.Value)
                db.AddInParameter(dbcmd, "@RetireAmount", DbType.String, r.RetireAmount.Value)
                db.AddInParameter(dbcmd, "@MthRealWages", DbType.String, r.MthRealWages.Value)
                db.AddInParameter(dbcmd, "@IdentityID", DbType.String, r.IdentityID.Value)
                db.AddInParameter(dbcmd, "@RelativeID", DbType.String, r.RelativeID.Value)
                db.AddInParameter(dbcmd, "@Reason", DbType.String, r.Reason.Value)
                db.AddInParameter(dbcmd, "@LabDate", DbType.Date, IIf(IsDateTimeNull(r.LabDate.Value), Convert.ToDateTime("1900/1/1"), r.LabDate.Value))
                db.AddInParameter(dbcmd, "@HeaDate", DbType.Date, IIf(IsDateTimeNull(r.HeaDate.Value), Convert.ToDateTime("1900/1/1"), r.HeaDate.Value))
                db.AddInParameter(dbcmd, "@InsureUnitID", DbType.Int32, r.InsureUnitID.Value)
                db.AddInParameter(dbcmd, "@InsureUnitIDHea", DbType.Int32, r.InsureUnitIDHea.Value)
                db.AddInParameter(dbcmd, "@TransferDate", DbType.Date, IIf(IsDateTimeNull(r.TransferDate.Value), Convert.ToDateTime("1900/1/1"), r.TransferDate.Value))
                db.AddInParameter(dbcmd, "@InsureState", DbType.String, r.InsureState.Value)
                db.AddInParameter(dbcmd, "@WelfareFlag", DbType.String, r.WelfareFlag.Value)
                db.AddInParameter(dbcmd, "@Remark1", DbType.String, r.Remark1.Value)
                db.AddInParameter(dbcmd, "@LastChgComp", DbType.String, r.LastChgComp.Value)
                db.AddInParameter(dbcmd, "@LastChgID", DbType.String, r.LastChgID.Value)
                db.AddInParameter(dbcmd, "@LastChgDate", DbType.Date, IIf(IsDateTimeNull(r.LastChgDate.Value), Convert.ToDateTime("1900/1/1"), r.LastChgDate.Value))

                intRowsAffected += db.ExecuteNonQuery(dbcmd, tran)
            Next
            Return intRowsAffected
        End Function

        Private Function IsDateTimeNull(ByVal Src As DateTime) As Boolean
            If Src = Convert.ToDateTime("1900/1/1") OrElse _
               Src = Convert.ToDateTime("0001/1/1") Then
                Return True
            Else
                Return False
            End If
        End Function
    End Class
End Namespace

