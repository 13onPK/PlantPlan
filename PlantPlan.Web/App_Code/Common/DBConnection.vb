Imports System.Data
Imports System.Data.SqlClient
Imports System.Transactions

Public Class DBConnection
    Public Property connectionString() As String
        Get
            Return m_connectionString
        End Get
        Set
            m_connectionString = Value
        End Set
    End Property
    Private m_connectionString As String

    Public Sub New(connectionString As String)
        Me.connectionString = connectionString
    End Sub

    Public Sub New()
        Me.connectionString = ConfigurationManager.ConnectionStrings("DBConnection").ConnectionString
    End Sub

    Public Function ExecuteDatasetWithTrans(spName As String, ltParameter As List(Of SqlParameter)) As DataSet
        Dim ds As New DataSet()
        Using transactionScope As New TransactionScope()
            Try
                ds = ExecuteDataset(spName, ltParameter)

                transactionScope.Complete()
                transactionScope.Dispose()
            Catch e As Exception
                transactionScope.Dispose()
            End Try
        End Using

        Return ds
    End Function
    'end ExecuteDatasetWithTrans
    Public Function ExecuteDataset(spName As String, ltParameter As List(Of SqlParameter)) As DataSet
        Dim ds As New DataSet()
        Dim sqlCon As SqlConnection = Nothing

        If Not [String].IsNullOrEmpty(connectionString) AndAlso Not [String].IsNullOrEmpty(spName) Then
            Try
                sqlCon = New SqlConnection(connectionString)

                Dim sqlCom As SqlCommand = sqlCon.CreateCommand()
                sqlCom.CommandType = CommandType.StoredProcedure
                sqlCom.CommandText = spName

                GenerateParameter(sqlCom, ltParameter)

                If sqlCon.State <> ConnectionState.Open Then
                    sqlCon.Open()
                End If
                Try
                    Dim da As New SqlDataAdapter(sqlCom)
                    da.Fill(ds)

                    If sqlCon.State = ConnectionState.Open Then
                        sqlCon.Close()
                    End If
                Catch ex2 As Exception
                    If sqlCon.State = ConnectionState.Open Then
                        sqlCon.Close()
                    End If
                    Throw ex2
                End Try
            Catch ex As Exception
                If sqlCon.State = ConnectionState.Open Then
                    sqlCon.Close()
                End If
                Throw ex
            End Try
        End If



        Return ds
    End Function
    'end ExecuteDataset
    Public Sub ExecuteNonQueryWithTrans(spName As String, ltParameter As List(Of SqlParameter))
        Using transactionScope As New TransactionScope()
            Try
                ExecuteNonQuery(spName, ltParameter)

                transactionScope.Complete()
                transactionScope.Dispose()
            Catch e As Exception
                transactionScope.Dispose()
            End Try
        End Using
    End Sub
    'end ExecuteDatasetWithTrans
    Public Sub ExecuteNonQuery(spName As String, ltParameter As List(Of SqlParameter))
        Dim sqlCon As SqlConnection = Nothing

        If Not [String].IsNullOrEmpty(connectionString) AndAlso Not [String].IsNullOrEmpty(spName) Then
            Try
                sqlCon = New SqlConnection(connectionString)

                Dim sqlCom As SqlCommand = sqlCon.CreateCommand()
                sqlCom.CommandType = CommandType.StoredProcedure
                sqlCom.CommandText = spName

                GenerateParameter(sqlCom, ltParameter)

                If sqlCon.State <> ConnectionState.Open Then
                    sqlCon.Open()
                End If
                Try
                    sqlCom.ExecuteNonQuery()

                    If sqlCon.State = ConnectionState.Open Then
                        sqlCon.Close()
                    End If
                Catch ex2 As Exception
                    If sqlCon.State = ConnectionState.Open Then
                        sqlCon.Close()
                    End If
                    Throw ex2
                End Try
            Catch ex As Exception
                If sqlCon.State = ConnectionState.Open Then
                    sqlCon.Close()
                End If
                Throw ex
            End Try
        End If
    End Sub
    'end ExecuteDataset
    Private Sub GenerateParameter(sqlCom As SqlCommand, ltParameter As List(Of SqlParameter))
        If ltParameter IsNot Nothing Then
            If ltParameter.Count > 0 Then
                For Each p As SqlParameter In ltParameter
                    If p.Value Is Nothing Then
                        p.Value = DBNull.Value
                    ElseIf [String].IsNullOrEmpty(p.Value.ToString()) Then
                        p.Value = DBNull.Value
                    End If
                    sqlCom.Parameters.Add(p)
                Next
            End If
        End If

    End Sub
    'end GenerateParameter
End Class
