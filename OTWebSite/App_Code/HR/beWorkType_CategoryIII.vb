﻿'****************************************************************
' Table:WorkType_CategoryIII
' Created Date: 2015.04.29
'****************************************************************/
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports Microsoft.Practices.EnterpriseLibrary.Common
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports System.Data.Common
Imports System.Data

Namespace beWorkType_CategoryIII
    Public Class Table
        Private m_Rows As Rows = New Rows()
        Private m_Fields As String() = { "CategoryI", "CategoryII", "CategoryIII", "CategoryIIIName", "LastChgComp", "LastChgID", "LastChgDate" }
        Private m_Types As System.Type() = { GetType(String), GetType(String), GetType(String), GetType(String), GetType(String), GetType(String), GetType(Date) }
        Private m_PrimaryFields As String() = { "CategoryI", "CategoryII", "CategoryIII" }

        Public ReadOnly Property Rows() As beWorkType_CategoryIII.Rows 
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
        Public Sub Transfer2Row(WorkType_CategoryIIITable As DataTable)
            For Each dr As DataRow In WorkType_CategoryIIITable.Rows
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

                dr(m_Rows(i).CategoryI.FieldName) = m_Rows(i).CategoryI.Value
                dr(m_Rows(i).CategoryII.FieldName) = m_Rows(i).CategoryII.Value
                dr(m_Rows(i).CategoryIII.FieldName) = m_Rows(i).CategoryIII.Value
                dr(m_Rows(i).CategoryIIIName.FieldName) = m_Rows(i).CategoryIIIName.Value
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

        Public Sub Add(WorkType_CategoryIIIRow As Row)
            m_Rows.Add(WorkType_CategoryIIIRow)
        End Sub

        Public Sub Remove(WorkType_CategoryIIIRow As Row)
            If m_Rows.IndexOf(WorkType_CategoryIIIRow) >= 0 Then
                m_Rows.Remove(WorkType_CategoryIIIRow)
            End If
        End Sub

        Public Sub Dispose()
            m_Rows.Clear()
        End Sub

    End Class

    Public Class Row

        Private FI_CategoryI As Field(Of String) = new Field(Of String)("CategoryI", true)
        Private FI_CategoryII As Field(Of String) = new Field(Of String)("CategoryII", true)
        Private FI_CategoryIII As Field(Of String) = new Field(Of String)("CategoryIII", true)
        Private FI_CategoryIIIName As Field(Of String) = new Field(Of String)("CategoryIIIName", true)
        Private FI_LastChgComp As Field(Of String) = new Field(Of String)("LastChgComp", true)
        Private FI_LastChgID As Field(Of String) = new Field(Of String)("LastChgID", true)
        Private FI_LastChgDate As Field(Of Date) = new Field(Of Date)("LastChgDate", true)
        Private m_FieldNames As String() = { "CategoryI", "CategoryII", "CategoryIII", "CategoryIIIName", "LastChgComp", "LastChgID", "LastChgDate" }
        Private m_PrimaryFields As String() = { "CategoryI", "CategoryII", "CategoryIII" }
        Private m_IdentityFields As String() = {  }
        Private m_LoadFromDataRow As Boolean = False

        Private Function GetFieldValue(ByVal fieldName As String) As Object
            Select Case fieldName
                Case "CategoryI"
                    Return FI_CategoryI.Value
                Case "CategoryII"
                    Return FI_CategoryII.Value
                Case "CategoryIII"
                    Return FI_CategoryIII.Value
                Case "CategoryIIIName"
                    Return FI_CategoryIIIName.Value
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
                Case "CategoryI"
                    FI_CategoryI.SetValue(value)
                Case "CategoryII"
                    FI_CategoryII.SetValue(value)
                Case "CategoryIII"
                    FI_CategoryIII.SetValue(value)
                Case "CategoryIIIName"
                    FI_CategoryIIIName.SetValue(value)
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
                Case "CategoryI"
                    return FI_CategoryI.Updated
                Case "CategoryII"
                    return FI_CategoryII.Updated
                Case "CategoryIII"
                    return FI_CategoryIII.Updated
                Case "CategoryIIIName"
                    return FI_CategoryIIIName.Updated
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
                Case "CategoryI"
                    return FI_CategoryI.CreateUpdateSQL
                Case "CategoryII"
                    return FI_CategoryII.CreateUpdateSQL
                Case "CategoryIII"
                    return FI_CategoryIII.CreateUpdateSQL
                Case "CategoryIIIName"
                    return FI_CategoryIIIName.CreateUpdateSQL
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
            FI_CategoryI.SetInitValue("""")
            FI_CategoryII.SetInitValue("")
            FI_CategoryIII.SetInitValue("")
            FI_CategoryIIIName.SetInitValue("")
            FI_LastChgComp.SetInitValue("")
            FI_LastChgID.SetInitValue("")
            FI_LastChgDate.SetInitValue(Convert.ToDateTime("1900/01/01"))
        End Sub

        Public Sub New(ByVal dr As System.Data.DataRow)
            FI_CategoryI.SetInitValue(dr("CategoryI"))
            FI_CategoryII.SetInitValue(dr("CategoryII"))
            FI_CategoryIII.SetInitValue(dr("CategoryIII"))
            FI_CategoryIIIName.SetInitValue(dr("CategoryIIIName"))
            FI_LastChgComp.SetInitValue(dr("LastChgComp"))
            FI_LastChgID.SetInitValue(dr("LastChgID"))
            FI_LastChgDate.SetInitValue(dr("LastChgDate"))

            m_LoadFromDataRow = True
        End Sub

        Private Sub ClearUpdated()
            FI_CategoryI.Updated = False
            FI_CategoryII.Updated = False
            FI_CategoryIII.Updated = False
            FI_CategoryIIIName.Updated = False
            FI_LastChgComp.Updated = False
            FI_LastChgID.Updated = False
            FI_LastChgDate.Updated = False
        End Sub

        Public ReadOnly Property CategoryI As Field(Of String) 
            Get
                Return FI_CategoryI
            End Get
        End Property

        Public ReadOnly Property CategoryII As Field(Of String) 
            Get
                Return FI_CategoryII
            End Get
        End Property

        Public ReadOnly Property CategoryIII As Field(Of String) 
            Get
                Return FI_CategoryIII
            End Get
        End Property

        Public ReadOnly Property CategoryIIIName As Field(Of String) 
            Get
                Return FI_CategoryIIIName
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
        Public Function DeleteRowByPrimaryKey(ByVal WorkType_CategoryIIIRow As Row) As Integer
            Dim db As Database = DatabaseFactory.CreateDatabase("eHRMSDB")
            Dim dbcmd As DbCommand
            Dim strSQL As StringBuilder = New StringBuilder()

            strSQL.AppendLine("Delete From WorkType_CategoryIII")
            strSQL.AppendLine("Where CategoryI = @CategoryI")
            strSQL.AppendLine("And CategoryII = @CategoryII")
            strSQL.AppendLine("And CategoryIII = @CategoryIII")

            dbcmd = db.GetSqlStringCommand(strSQL.ToString())
            db.AddInParameter(dbcmd, "@CategoryI", DbType.String, WorkType_CategoryIIIRow.CategoryI.Value)
            db.AddInParameter(dbcmd, "@CategoryII", DbType.String, WorkType_CategoryIIIRow.CategoryII.Value)
            db.AddInParameter(dbcmd, "@CategoryIII", DbType.String, WorkType_CategoryIIIRow.CategoryIII.Value)

            return db.ExecuteNonQuery(dbcmd)
        End Function

        public Function DeleteRowByPrimaryKey(ByVal WorkType_CategoryIIIRow As Row, ByVal tran As DbTransaction) As Integer
            Dim db As Database = DatabaseFactory.CreateDatabase("eHRMSDB")
            Dim dbcmd As DbCommand
            Dim strSQL As StringBuilder = New StringBuilder()

            strSQL.AppendLine("Delete From WorkType_CategoryIII")
            strSQL.AppendLine("Where CategoryI = @CategoryI")
            strSQL.AppendLine("And CategoryII = @CategoryII")
            strSQL.AppendLine("And CategoryIII = @CategoryIII")

            dbcmd = db.GetSqlStringCommand(strSQL.ToString())
            db.AddInParameter(dbcmd, "@CategoryI", DbType.String, WorkType_CategoryIIIRow.CategoryI.Value)
            db.AddInParameter(dbcmd, "@CategoryII", DbType.String, WorkType_CategoryIIIRow.CategoryII.Value)
            db.AddInParameter(dbcmd, "@CategoryIII", DbType.String, WorkType_CategoryIIIRow.CategoryIII.Value)

            return db.ExecuteNonQuery(dbcmd, tran)
        End Function

        Public Function DeleteRowByPrimaryKey(ByVal WorkType_CategoryIIIRow As Row()) As Integer
            Dim db As Database = DatabaseFactory.CreateDatabase("eHRMSDB")
            Dim dbcmd As DbCommand
            Dim strSQL As StringBuilder = New StringBuilder()
            Dim intRowsAffected As Integer = 0

            strSQL.AppendLine("Delete From WorkType_CategoryIII")
            strSQL.AppendLine("Where CategoryI = @CategoryI")
            strSQL.AppendLine("And CategoryII = @CategoryII")
            strSQL.AppendLine("And CategoryIII = @CategoryIII")

            dbcmd = db.GetSqlStringCommand(strSQL.ToString())
            using cn As DbConnection = db.CreateConnection()
                cn.Open()
                Dim tran As DbTransaction = cn.BeginTransaction()
                Dim inTrans As Boolean = True

                Try
                    For Each r As Row In WorkType_CategoryIIIRow
                        dbcmd.Parameters.Clear()
                        db.AddInParameter(dbcmd, "@CategoryI", DbType.String, r.CategoryI.Value)
                        db.AddInParameter(dbcmd, "@CategoryII", DbType.String, r.CategoryII.Value)
                        db.AddInParameter(dbcmd, "@CategoryIII", DbType.String, r.CategoryIII.Value)

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

        Public Function DeleteRowByPrimaryKey(ByVal WorkType_CategoryIIIRow As Row(), ByVal tran As DbTransaction) As Integer
            Dim db As Database = DatabaseFactory.CreateDatabase("eHRMSDB")
            Dim dbcmd As DbCommand
            Dim strSQL As StringBuilder = New StringBuilder()
            Dim intRowsAffected As Integer = 0

            strSQL.AppendLine("Delete From WorkType_CategoryIII")
            strSQL.AppendLine("Where CategoryI = @CategoryI")
            strSQL.AppendLine("And CategoryII = @CategoryII")
            strSQL.AppendLine("And CategoryIII = @CategoryIII")

            dbcmd = db.GetSqlStringCommand(strSQL.ToString())

            For Each r As Row In WorkType_CategoryIIIRow
                dbcmd.Parameters.Clear()
                db.AddInParameter(dbcmd, "@CategoryI", DbType.String, r.CategoryI.Value)
                db.AddInParameter(dbcmd, "@CategoryII", DbType.String, r.CategoryII.Value)
                db.AddInParameter(dbcmd, "@CategoryIII", DbType.String, r.CategoryIII.Value)

                intRowsAffected += db.ExecuteNonQuery(dbcmd, tran)
            Next
            Return intRowsAffected
        End Function

        Public Function QueryByKey(ByVal WorkType_CategoryIIIRow As Row) As DataSet
            Dim db As Database = DatabaseFactory.CreateDatabase("eHRMSDB")
            Dim dbcmd As DbCommand
            Dim strSQL As StringBuilder = New StringBuilder()

            strSQL.AppendLine("Select * From WorkType_CategoryIII")
            strSQL.AppendLine("Where CategoryI = @CategoryI")
            strSQL.AppendLine("And CategoryII = @CategoryII")
            strSQL.AppendLine("And CategoryIII = @CategoryIII")

            dbcmd = db.GetSqlStringCommand(strSQL.ToString())
            db.AddInParameter(dbcmd, "@CategoryI", DbType.String, WorkType_CategoryIIIRow.CategoryI.Value)
            db.AddInParameter(dbcmd, "@CategoryII", DbType.String, WorkType_CategoryIIIRow.CategoryII.Value)
            db.AddInParameter(dbcmd, "@CategoryIII", DbType.String, WorkType_CategoryIIIRow.CategoryIII.Value)

            Return db.ExecuteDataSet(dbcmd)
        End Function

        Public Function QueryByKey(WorkType_CategoryIIIRow As Row, tran As DbTransaction) As DataSet
            Dim db As Database = DatabaseFactory.CreateDatabase("eHRMSDB")
            Dim dbcmd As DbCommand
            Dim strSQL As StringBuilder = New StringBuilder()

            strSQL.AppendLine("Select * From WorkType_CategoryIII")
            strSQL.AppendLine("Where CategoryI = @CategoryI")
            strSQL.AppendLine("And CategoryII = @CategoryII")
            strSQL.AppendLine("And CategoryIII = @CategoryIII")

            dbcmd = db.GetSqlStringCommand(strSQL.ToString())
            db.AddInParameter(dbcmd, "@CategoryI", DbType.String, WorkType_CategoryIIIRow.CategoryI.Value)
            db.AddInParameter(dbcmd, "@CategoryII", DbType.String, WorkType_CategoryIIIRow.CategoryII.Value)
            db.AddInParameter(dbcmd, "@CategoryIII", DbType.String, WorkType_CategoryIIIRow.CategoryIII.Value)

            Return db.ExecuteDataSet(dbcmd, tran)
        End Function

        Public Function Update(ByVal WorkType_CategoryIIIRow As Row) As Integer
            Dim db As Database = DatabaseFactory.CreateDatabase("eHRMSDB")
            Dim dbcmd As DbCommand
            Dim strSQL As StringBuilder = New StringBuilder()
            Dim strDot As String = String.Empty

            strSQL.AppendLine("Update WorkType_CategoryIII Set")
            For i As Integer = 0 To WorkType_CategoryIIIRow.FieldNames.Length - 1
                If Not WorkType_CategoryIIIRow.IsIdentityField(WorkType_CategoryIIIRow.FieldNames(i)) AndAlso WorkType_CategoryIIIRow.IsUpdated(WorkType_CategoryIIIRow.FieldNames(i)) AndAlso WorkType_CategoryIIIRow.CreateUpdateSQL(WorkType_CategoryIIIRow.FieldNames(i)) Then
                    strSQL.AppendLine(string.Format("{0}{1} = @{1}", strDot, WorkType_CategoryIIIRow.FieldNames(i)))
                    strDot = ","
                End If
            Next
            If strDot = String.Empty Then Throw New Exception("未異動資料欄位！")
            strSQL.AppendLine("Where CategoryI = @PKCategoryI")
            strSQL.AppendLine("And CategoryII = @PKCategoryII")
            strSQL.AppendLine("And CategoryIII = @PKCategoryIII")

            dbcmd = db.GetSqlStringCommand(strSQL.ToString())
            If WorkType_CategoryIIIRow.CategoryI.Updated Then db.AddInParameter(dbcmd, "@CategoryI", DbType.String, WorkType_CategoryIIIRow.CategoryI.Value)
            If WorkType_CategoryIIIRow.CategoryII.Updated Then db.AddInParameter(dbcmd, "@CategoryII", DbType.String, WorkType_CategoryIIIRow.CategoryII.Value)
            If WorkType_CategoryIIIRow.CategoryIII.Updated Then db.AddInParameter(dbcmd, "@CategoryIII", DbType.String, WorkType_CategoryIIIRow.CategoryIII.Value)
            If WorkType_CategoryIIIRow.CategoryIIIName.Updated Then db.AddInParameter(dbcmd, "@CategoryIIIName", DbType.String, WorkType_CategoryIIIRow.CategoryIIIName.Value)
            If WorkType_CategoryIIIRow.LastChgComp.Updated Then db.AddInParameter(dbcmd, "@LastChgComp", DbType.String, WorkType_CategoryIIIRow.LastChgComp.Value)
            If WorkType_CategoryIIIRow.LastChgID.Updated Then db.AddInParameter(dbcmd, "@LastChgID", DbType.String, WorkType_CategoryIIIRow.LastChgID.Value)
            If WorkType_CategoryIIIRow.LastChgDate.Updated Then db.AddInParameter(dbcmd, "@LastChgDate", DbType.Date, IIf(IsDateTimeNull(WorkType_CategoryIIIRow.LastChgDate.Value), Convert.ToDateTime("1900/1/1"), WorkType_CategoryIIIRow.LastChgDate.Value))
            db.AddInParameter(dbcmd, "@PKCategoryI", DbType.String, IIf(WorkType_CategoryIIIRow.LoadFromDataRow, WorkType_CategoryIIIRow.CategoryI.OldValue, WorkType_CategoryIIIRow.CategoryI.Value))
            db.AddInParameter(dbcmd, "@PKCategoryII", DbType.String, IIf(WorkType_CategoryIIIRow.LoadFromDataRow, WorkType_CategoryIIIRow.CategoryII.OldValue, WorkType_CategoryIIIRow.CategoryII.Value))
            db.AddInParameter(dbcmd, "@PKCategoryIII", DbType.String, IIf(WorkType_CategoryIIIRow.LoadFromDataRow, WorkType_CategoryIIIRow.CategoryIII.OldValue, WorkType_CategoryIIIRow.CategoryIII.Value))

            Return db.ExecuteNonQuery(dbcmd)
        End Function

        Public Function Update(ByVal WorkType_CategoryIIIRow As Row, ByVal tran As DbTransaction) As Integer
            Dim db As Database = DatabaseFactory.CreateDatabase("eHRMSDB")
            Dim dbcmd As DbCommand
            Dim strSQL As StringBuilder = New StringBuilder()
            Dim strDot As String = String.Empty

            strSQL.AppendLine("Update WorkType_CategoryIII Set")
            For i As Integer = 0 To WorkType_CategoryIIIRow.FieldNames.Length - 1
                If Not WorkType_CategoryIIIRow.IsIdentityField(WorkType_CategoryIIIRow.FieldNames(i)) AndAlso WorkType_CategoryIIIRow.IsUpdated(WorkType_CategoryIIIRow.FieldNames(i)) AndAlso WorkType_CategoryIIIRow.CreateUpdateSQL(WorkType_CategoryIIIRow.FieldNames(i)) Then
                    strSQL.AppendLine(string.Format("{0}{1} = @{1}", strDot, WorkType_CategoryIIIRow.FieldNames(i)))
                    strDot = ","
                End If
            Next 
            If strDot = String.Empty Then Throw New Exception("未異動資料欄位！")
            strSQL.AppendLine("Where CategoryI = @PKCategoryI")
            strSQL.AppendLine("And CategoryII = @PKCategoryII")
            strSQL.AppendLine("And CategoryIII = @PKCategoryIII")

            dbcmd = db.GetSqlStringCommand(strSQL.ToString())
            If WorkType_CategoryIIIRow.CategoryI.Updated Then db.AddInParameter(dbcmd, "@CategoryI", DbType.String, WorkType_CategoryIIIRow.CategoryI.Value)
            If WorkType_CategoryIIIRow.CategoryII.Updated Then db.AddInParameter(dbcmd, "@CategoryII", DbType.String, WorkType_CategoryIIIRow.CategoryII.Value)
            If WorkType_CategoryIIIRow.CategoryIII.Updated Then db.AddInParameter(dbcmd, "@CategoryIII", DbType.String, WorkType_CategoryIIIRow.CategoryIII.Value)
            If WorkType_CategoryIIIRow.CategoryIIIName.Updated Then db.AddInParameter(dbcmd, "@CategoryIIIName", DbType.String, WorkType_CategoryIIIRow.CategoryIIIName.Value)
            If WorkType_CategoryIIIRow.LastChgComp.Updated Then db.AddInParameter(dbcmd, "@LastChgComp", DbType.String, WorkType_CategoryIIIRow.LastChgComp.Value)
            If WorkType_CategoryIIIRow.LastChgID.Updated Then db.AddInParameter(dbcmd, "@LastChgID", DbType.String, WorkType_CategoryIIIRow.LastChgID.Value)
            If WorkType_CategoryIIIRow.LastChgDate.Updated Then db.AddInParameter(dbcmd, "@LastChgDate", DbType.Date, IIf(IsDateTimeNull(WorkType_CategoryIIIRow.LastChgDate.Value), Convert.ToDateTime("1900/1/1"), WorkType_CategoryIIIRow.LastChgDate.Value))
            db.AddInParameter(dbcmd, "@PKCategoryI", DbType.String, IIf(WorkType_CategoryIIIRow.LoadFromDataRow, WorkType_CategoryIIIRow.CategoryI.OldValue, WorkType_CategoryIIIRow.CategoryI.Value))
            db.AddInParameter(dbcmd, "@PKCategoryII", DbType.String, IIf(WorkType_CategoryIIIRow.LoadFromDataRow, WorkType_CategoryIIIRow.CategoryII.OldValue, WorkType_CategoryIIIRow.CategoryII.Value))
            db.AddInParameter(dbcmd, "@PKCategoryIII", DbType.String, IIf(WorkType_CategoryIIIRow.LoadFromDataRow, WorkType_CategoryIIIRow.CategoryIII.OldValue, WorkType_CategoryIIIRow.CategoryIII.Value))

            Return db.ExecuteNonQuery(dbcmd, tran)
        End Function

        Public Function Update(ByVal WorkType_CategoryIIIRow As Row()) As Integer
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
                    For Each r As Row In WorkType_CategoryIIIRow
                        strDot = String.Empty
                        If strSQL.Length > 0 Then strSQL.Remove(0, strSQL.Length)
                        strSQL.AppendLine("Update WorkType_CategoryIII Set")
                        For i As Integer = 0 To r.FieldNames.Length - 1
                            If Not r.IsIdentityField(r.FieldNames(i)) AndAlso r.IsUpdated(r.FieldNames(i)) AndAlso r.CreateUpdateSQL(r.FieldNames(i)) Then
                                strSQL.AppendLine(String.Format("{0}{1} = @{1}", strDot, r.FieldNames(i)))
                                strDot = ","
                            End If
                        Next
                        If strDot = String.Empty Then Continue For
                        strSQL.AppendLine("Where CategoryI = @PKCategoryI")
                        strSQL.AppendLine("And CategoryII = @PKCategoryII")
                        strSQL.AppendLine("And CategoryIII = @PKCategoryIII")

                        dbcmd = db.GetSqlStringCommand(strSQL.ToString())
                        If r.CategoryI.Updated Then db.AddInParameter(dbcmd, "@CategoryI", DbType.String, r.CategoryI.Value)
                        If r.CategoryII.Updated Then db.AddInParameter(dbcmd, "@CategoryII", DbType.String, r.CategoryII.Value)
                        If r.CategoryIII.Updated Then db.AddInParameter(dbcmd, "@CategoryIII", DbType.String, r.CategoryIII.Value)
                        If r.CategoryIIIName.Updated Then db.AddInParameter(dbcmd, "@CategoryIIIName", DbType.String, r.CategoryIIIName.Value)
                        If r.LastChgComp.Updated Then db.AddInParameter(dbcmd, "@LastChgComp", DbType.String, r.LastChgComp.Value)
                        If r.LastChgID.Updated Then db.AddInParameter(dbcmd, "@LastChgID", DbType.String, r.LastChgID.Value)
                        If r.LastChgDate.Updated Then db.AddInParameter(dbcmd, "@LastChgDate", DbType.Date, IIf(IsDateTimeNull(r.LastChgDate.Value), Convert.ToDateTime("1900/1/1"), r.LastChgDate.Value))
                        db.AddInParameter(dbcmd, "@PKCategoryI", DbType.String, IIf(r.LoadFromDataRow, r.CategoryI.OldValue, r.CategoryI.Value))
                        db.AddInParameter(dbcmd, "@PKCategoryII", DbType.String, IIf(r.LoadFromDataRow, r.CategoryII.OldValue, r.CategoryII.Value))
                        db.AddInParameter(dbcmd, "@PKCategoryIII", DbType.String, IIf(r.LoadFromDataRow, r.CategoryIII.OldValue, r.CategoryIII.Value))

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

        Public Function Update(ByVal WorkType_CategoryIIIRow As Row(), ByVal tran As DbTransaction) As Integer
            Dim db As Database = DatabaseFactory.CreateDatabase("eHRMSDB")
            Dim dbcmd As DbCommand
            Dim strSQL As StringBuilder = New StringBuilder()
            Dim intRowsAffected As Integer = 0
            Dim strDot As String

            For Each r As Row In WorkType_CategoryIIIRow
                strDot = String.Empty
                If strSQL.Length > 0 Then strSQL.Remove(0, strSQL.Length)
                strSQL.AppendLine("Update WorkType_CategoryIII Set")
                For i As Integer = 0 To r.FieldNames.Length - 1
                    If Not r.IsIdentityField(r.FieldNames(i)) AndAlso r.IsUpdated(r.FieldNames(i)) AndAlso r.CreateUpdateSQL(r.FieldNames(i))
                        strSQL.AppendLine(String.Format("{0}{1} = @{1}", strDot, r.FieldNames(i)))
                        strDot = ","
                    End If
                Next
                If strDot = String.Empty Then Continue For
                strSQL.AppendLine("Where CategoryI = @PKCategoryI")
                strSQL.AppendLine("And CategoryII = @PKCategoryII")
                strSQL.AppendLine("And CategoryIII = @PKCategoryIII")

                dbcmd = db.GetSqlStringCommand(strSQL.ToString())
                If r.CategoryI.Updated Then db.AddInParameter(dbcmd, "@CategoryI", DbType.String, r.CategoryI.Value)
                If r.CategoryII.Updated Then db.AddInParameter(dbcmd, "@CategoryII", DbType.String, r.CategoryII.Value)
                If r.CategoryIII.Updated Then db.AddInParameter(dbcmd, "@CategoryIII", DbType.String, r.CategoryIII.Value)
                If r.CategoryIIIName.Updated Then db.AddInParameter(dbcmd, "@CategoryIIIName", DbType.String, r.CategoryIIIName.Value)
                If r.LastChgComp.Updated Then db.AddInParameter(dbcmd, "@LastChgComp", DbType.String, r.LastChgComp.Value)
                If r.LastChgID.Updated Then db.AddInParameter(dbcmd, "@LastChgID", DbType.String, r.LastChgID.Value)
                If r.LastChgDate.Updated Then db.AddInParameter(dbcmd, "@LastChgDate", DbType.Date, IIf(IsDateTimeNull(r.LastChgDate.Value), Convert.ToDateTime("1900/1/1"), r.LastChgDate.Value))
                db.AddInParameter(dbcmd, "@PKCategoryI", DbType.String, IIf(r.LoadFromDataRow, r.CategoryI.OldValue, r.CategoryI.Value))
                db.AddInParameter(dbcmd, "@PKCategoryII", DbType.String, IIf(r.LoadFromDataRow, r.CategoryII.OldValue, r.CategoryII.Value))
                db.AddInParameter(dbcmd, "@PKCategoryIII", DbType.String, IIf(r.LoadFromDataRow, r.CategoryIII.OldValue, r.CategoryIII.Value))

                intRowsAffected += db.ExecuteNonQuery(dbcmd, tran)
            Next
            Return intRowsAffected
        End Function

        Public Function IsDataExists(ByVal WorkType_CategoryIIIRow As Row) As Boolean
            Dim db As Database = DatabaseFactory.CreateDatabase("eHRMSDB")
            Dim dbcmd As DbCommand
            Dim strSQL As StringBuilder = New StringBuilder()
            Dim intCount As Integer = 0

            strSQL.AppendLine("Select Count(*) Cnt From WorkType_CategoryIII")
            strSQL.AppendLine("Where CategoryI = @CategoryI")
            strSQL.AppendLine("And CategoryII = @CategoryII")
            strSQL.AppendLine("And CategoryIII = @CategoryIII")

            dbcmd = db.GetSqlStringCommand(strSQL.ToString())
            db.AddInParameter(dbcmd, "@CategoryI", DbType.String, WorkType_CategoryIIIRow.CategoryI.Value)
            db.AddInParameter(dbcmd, "@CategoryII", DbType.String, WorkType_CategoryIIIRow.CategoryII.Value)
            db.AddInParameter(dbcmd, "@CategoryIII", DbType.String, WorkType_CategoryIIIRow.CategoryIII.Value)

            intCount = Convert.ToInt32(db.ExecuteScalar(dbcmd))
            Return IIf(intCount > 0, True, False)
        End Function

        Public Function IsDataExists(ByVal WorkType_CategoryIIIRow As Row, ByVal tran As DbTransaction) As Boolean
            Dim db As Database = DatabaseFactory.CreateDatabase("eHRMSDB")
            Dim dbcmd As DbCommand
            Dim strSQL As StringBuilder = New StringBuilder()
            Dim intCount As Integer = 0

            strSQL.AppendLine("Select Count(*) Cnt From WorkType_CategoryIII")
            strSQL.AppendLine("Where CategoryI = @CategoryI")
            strSQL.AppendLine("And CategoryII = @CategoryII")
            strSQL.AppendLine("And CategoryIII = @CategoryIII")

            dbcmd = db.GetSqlStringCommand(strSQL.ToString())
            db.AddInParameter(dbcmd, "@CategoryI", DbType.String, WorkType_CategoryIIIRow.CategoryI.Value)
            db.AddInParameter(dbcmd, "@CategoryII", DbType.String, WorkType_CategoryIIIRow.CategoryII.Value)
            db.AddInParameter(dbcmd, "@CategoryIII", DbType.String, WorkType_CategoryIIIRow.CategoryIII.Value)

            intCount = Convert.ToInt32(db.ExecuteScalar(dbcmd, tran))
            Return IIf(intCount > 0, True, False)
        End Function

        Public Function QuerybyWhere(WhereCondition As String) As DataSet
            Dim db As Database = DatabaseFactory.CreateDatabase("eHRMSDB")
            Dim strSQL As StringBuilder = New StringBuilder()

            strSQL.AppendLine("Select * From WorkType_CategoryIII")
            If WhereCondition <> String.Empty Then strSQL.AppendLine(WhereCondition)

            Return db.ExecuteDataSet(CommandType.Text, strSQL.ToString())
        End Function

        Public Function Insert(ByVal WorkType_CategoryIIIRow As Row) As Integer
            Dim db As Database = DatabaseFactory.CreateDatabase("eHRMSDB")
            Dim dbcmd As DbCommand
            Dim strSQL As StringBuilder = New StringBuilder()

            strSQL.AppendLine("Insert into WorkType_CategoryIII")
            strSQL.AppendLine("(")
            strSQL.AppendLine("    CategoryI, CategoryII, CategoryIII, CategoryIIIName, LastChgComp, LastChgID, LastChgDate")
            strSQL.AppendLine(")")
            strSQL.AppendLine("Values")
            strSQL.AppendLine("(")
            strSQL.AppendLine("    @CategoryI, @CategoryII, @CategoryIII, @CategoryIIIName, @LastChgComp, @LastChgID, @LastChgDate")
            strSQL.AppendLine(")")

            dbcmd = db.GetSqlStringCommand(strSQL.ToString())
            db.AddInParameter(dbcmd, "@CategoryI", DbType.String, WorkType_CategoryIIIRow.CategoryI.Value)
            db.AddInParameter(dbcmd, "@CategoryII", DbType.String, WorkType_CategoryIIIRow.CategoryII.Value)
            db.AddInParameter(dbcmd, "@CategoryIII", DbType.String, WorkType_CategoryIIIRow.CategoryIII.Value)
            db.AddInParameter(dbcmd, "@CategoryIIIName", DbType.String, WorkType_CategoryIIIRow.CategoryIIIName.Value)
            db.AddInParameter(dbcmd, "@LastChgComp", DbType.String, WorkType_CategoryIIIRow.LastChgComp.Value)
            db.AddInParameter(dbcmd, "@LastChgID", DbType.String, WorkType_CategoryIIIRow.LastChgID.Value)
            db.AddInParameter(dbcmd, "@LastChgDate", DbType.Date, IIf(IsDateTimeNull(WorkType_CategoryIIIRow.LastChgDate.Value), Convert.ToDateTime("1900/1/1"), WorkType_CategoryIIIRow.LastChgDate.Value))

            Return db.ExecuteNonQuery(dbcmd)
        End Function

        Public Function Insert(ByVal WorkType_CategoryIIIRow As Row, ByVal tran As DbTransaction) As Integer
            Dim db As Database = DatabaseFactory.CreateDatabase("eHRMSDB")
            Dim dbcmd As DbCommand
            Dim strSQL As StringBuilder = New StringBuilder()

            strSQL.AppendLine("Insert into WorkType_CategoryIII")
            strSQL.AppendLine("(")
            strSQL.AppendLine("    CategoryI, CategoryII, CategoryIII, CategoryIIIName, LastChgComp, LastChgID, LastChgDate")
            strSQL.AppendLine(")")
            strSQL.AppendLine("Values")
            strSQL.AppendLine("(")
            strSQL.AppendLine("    @CategoryI, @CategoryII, @CategoryIII, @CategoryIIIName, @LastChgComp, @LastChgID, @LastChgDate")
            strSQL.AppendLine(")")

            dbcmd = db.GetSqlStringCommand(strSQL.ToString())
            db.AddInParameter(dbcmd, "@CategoryI", DbType.String, WorkType_CategoryIIIRow.CategoryI.Value)
            db.AddInParameter(dbcmd, "@CategoryII", DbType.String, WorkType_CategoryIIIRow.CategoryII.Value)
            db.AddInParameter(dbcmd, "@CategoryIII", DbType.String, WorkType_CategoryIIIRow.CategoryIII.Value)
            db.AddInParameter(dbcmd, "@CategoryIIIName", DbType.String, WorkType_CategoryIIIRow.CategoryIIIName.Value)
            db.AddInParameter(dbcmd, "@LastChgComp", DbType.String, WorkType_CategoryIIIRow.LastChgComp.Value)
            db.AddInParameter(dbcmd, "@LastChgID", DbType.String, WorkType_CategoryIIIRow.LastChgID.Value)
            db.AddInParameter(dbcmd, "@LastChgDate", DbType.Date, IIf(IsDateTimeNull(WorkType_CategoryIIIRow.LastChgDate.Value), Convert.ToDateTime("1900/1/1"), WorkType_CategoryIIIRow.LastChgDate.Value))

            Return db.ExecuteNonQuery(dbcmd, tran)
        End Function

        Public Function Insert(ByVal WorkType_CategoryIIIRow As Row()) As Integer
            Dim db As Database = DatabaseFactory.CreateDatabase("eHRMSDB")
            Dim dbcmd As DbCommand
            Dim strSQL As StringBuilder = New StringBuilder()
            Dim intRowsAffected As Integer = 0

            strSQL.AppendLine("Insert into WorkType_CategoryIII")
            strSQL.AppendLine("(")
            strSQL.AppendLine("    CategoryI, CategoryII, CategoryIII, CategoryIIIName, LastChgComp, LastChgID, LastChgDate")
            strSQL.AppendLine(")")
            strSQL.AppendLine("Values")
            strSQL.AppendLine("(")
            strSQL.AppendLine("    @CategoryI, @CategoryII, @CategoryIII, @CategoryIIIName, @LastChgComp, @LastChgID, @LastChgDate")
            strSQL.AppendLine(")")

            dbcmd = db.GetSqlStringCommand(strSQL.ToString())
            Using cn As DbConnection = db.CreateConnection()
                cn.Open()
                Dim tran As DbTransaction = cn.BeginTransaction()
                Dim inTrans As Boolean = True

                Try
                    For Each r As Row In WorkType_CategoryIIIRow
                        dbcmd.Parameters.Clear()
                        db.AddInParameter(dbcmd, "@CategoryI", DbType.String, r.CategoryI.Value)
                        db.AddInParameter(dbcmd, "@CategoryII", DbType.String, r.CategoryII.Value)
                        db.AddInParameter(dbcmd, "@CategoryIII", DbType.String, r.CategoryIII.Value)
                        db.AddInParameter(dbcmd, "@CategoryIIIName", DbType.String, r.CategoryIIIName.Value)
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

        Public Function Insert(ByVal WorkType_CategoryIIIRow As Row(), ByVal tran As DbTransaction) As Integer
            Dim db As Database = DatabaseFactory.CreateDatabase("eHRMSDB")
            Dim dbcmd As DbCommand
            Dim strSQL As StringBuilder = New StringBuilder()
            Dim intRowsAffected As Integer = 0

            strSQL.AppendLine("Insert into WorkType_CategoryIII")
            strSQL.AppendLine("(")
            strSQL.AppendLine("    CategoryI, CategoryII, CategoryIII, CategoryIIIName, LastChgComp, LastChgID, LastChgDate")
            strSQL.AppendLine(")")
            strSQL.AppendLine("Values")
            strSQL.AppendLine("(")
            strSQL.AppendLine("    @CategoryI, @CategoryII, @CategoryIII, @CategoryIIIName, @LastChgComp, @LastChgID, @LastChgDate")
            strSQL.AppendLine(")")

            dbcmd = db.GetSqlStringCommand(strSQL.ToString())

            For Each r As Row In WorkType_CategoryIIIRow
                dbcmd.Parameters.Clear()
                db.AddInParameter(dbcmd, "@CategoryI", DbType.String, r.CategoryI.Value)
                db.AddInParameter(dbcmd, "@CategoryII", DbType.String, r.CategoryII.Value)
                db.AddInParameter(dbcmd, "@CategoryIII", DbType.String, r.CategoryIII.Value)
                db.AddInParameter(dbcmd, "@CategoryIIIName", DbType.String, r.CategoryIIIName.Value)
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

