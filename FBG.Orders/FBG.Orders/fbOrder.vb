
Imports System
Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.Globalization
Imports System.IO
Imports System.Linq
Imports System.Reflection
Imports System.Runtime.CompilerServices
Imports System.Security
Imports System.Text
Imports System.Threading.Tasks
Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports RA.Platform
Imports System.ComponentModel
Imports System.Data.OleDb

Public Class fbOrder
    Private _OrderID As Integer
    Private _EmpID As Integer
    Private _ClientID As Integer
    Private _OrderDate As DateTime
    Private _ShippedDate As DateTime
    Private _ClientLocation As String
    Private _Taxes As String
    Private _PaymentType As String
    Private _PaidDate As DateTime
    Private _Notes As String
    Private _StatusID As Integer
    Private _ExSource As String
    Private _ExSourceID As String


    Public ReadOnly Property OrderID As System.Int32
        Get
            Return _OrderID
        End Get
    End Property

    Public Property EmpID As Integer
        Get
            Return _EmpID
        End Get
        Set(ByVal value As Integer)
            _EmpID = value
        End Set
    End Property

    Public Property ClientID As Integer
        Get
            Return _ClientID
        End Get
        Set(ByVal value As Integer)
            _ClientID = value
        End Set
    End Property

    Public Property OrderDate As DateTime
        Get
            Return _OrderDate
        End Get
        Set(ByVal value As DateTime)
            _OrderDate = value
        End Set
    End Property

    Public Property ShippedDate As DateTime
        Get
            Return _ShippedDate
        End Get
        Set(ByVal value As DateTime)
            _ShippedDate = value
        End Set
    End Property

    Public Property ClientLocation As String
        Get
            Return _ClientLocation
        End Get
        Set(ByVal value As String)
            _ClientLocation = value
        End Set
    End Property

    Public Property Taxes As String
        Get
            Return _Taxes
        End Get
        Set(ByVal value As String)
            _Taxes = value
        End Set
    End Property

    Public Property PaymentType As String
        Get
            Return _PaymentType
        End Get
        Set(ByVal value As String)
            _PaymentType = value
        End Set
    End Property

    Public Property PaidDate As DateTime
        Get
            Return _PaidDate
        End Get
        Set(ByVal value As DateTime)
            _PaidDate = value
        End Set
    End Property

    Public Property Notes As String
        Get
            Return _Notes
        End Get
        Set(ByVal value As String)
            _Notes = value
        End Set
    End Property

    Public Property StatusID As System.Int32
        Get
            Return _StatusID
        End Get
        Set(ByVal value As System.Int32)
            _StatusID = value
        End Set
    End Property

    Public Property ExSource As String
        Get
            Return _ExSource
        End Get
        Set(ByVal value As String)
            _ExSource = value
        End Set
    End Property
    Public Property ExSourceID As String
        Get
            Return _ExSourceID
        End Get
        Set(ByVal value As String)
            _ExSourceID = value
        End Set
    End Property

    Public Sub Save()
        Try

            Dim conn As OleDbConnection = Common.DBConnection()

            Using cmd As OleDbCommand = conn.CreateCommand()
                ' cmd.CommandText = "INSERT INTO T_Orders1 " & "([OEmpID], [OClientID], [ODate], [OShippedDate], [OClientLocation], [OTaxes], [OPaymentType], [OPaidDate], [ONotes], [OStatus ID]) " & "VALUES(@EmpID, @ClientID, @OrderDate, @ShippedDate, @ClientLocation, @Taxes, @PaymentType, @PaidDate, @Notes, @StatusID)"
                cmd.CommandText = "INSERT INTO T_Orders " & "([OEmpID], [OClientID], [ODate],[OShippedDate], [OClientLocation], [OTaxes], [OPaymentType], [OPaidDate],[ONotes],[OStatusID], [OExSource], [OExSourceID]) " & _
                                  "VALUES(@EmpID, @ClientID, @OrderDate,@ShippedDate, @ClientLocation, @Taxes, @PaymentType, @PaidDateD,@Notes,@StatusID,@ExSource, @ExSourceID)"


                'cmd.CommandText = "INSERT INTO T_Clients " & "([CName]) " & "VALUES('AAA')"

                cmd.Parameters.Add("@EmpID", OleDbType.Integer).Value = EmpID
                cmd.Parameters.Add("@ClientID", OleDbType.Integer).Value = ClientID
                cmd.Parameters.Add("@OrderDate", OleDbType.DBDate).Value = OrderDate
                cmd.Parameters.Add("@ShippedDate", OleDbType.DBDate).Value = ShippedDate
                cmd.Parameters.Add("@ClientLocation", OleDbType.Integer).Value = ClientLocation
                cmd.Parameters.Add("@Taxes", OleDbType.VarChar).Value = Taxes
                cmd.Parameters.Add("@PaymentType", OleDbType.VarChar).Value = PaymentType
                cmd.Parameters.Add("@PaidDate", OleDbType.DBDate).Value = PaidDate
                cmd.Parameters.Add("@Notes", OleDbType.LongVarChar).Value = Notes
                cmd.Parameters.Add("@StatusID", OleDbType.VarChar).Value = StatusID
                cmd.Parameters.Add("@ExSource", OleDbType.VarChar).Value = ExSource
                cmd.Parameters.Add("@ExSourceID", OleDbType.VarChar).Value = ExSourceID
                cmd.ExecuteNonQuery()

                Dim QryID As String = "Select @@Identity"
                cmd.CommandText = QryID

                Dim iResult As Integer = cmd.ExecuteScalar()



                If iResult > 0 Then
                    _OrderID = iResult
                End If
            End Using

            conn.Close()
        Catch ex As Exception
            AddToLog("Error occured in fbOrder.save: " & ex.Message, True)
        End Try




    End Sub

    Public Function CheckDuplicateOrder(ExSource As String, ExSourceID As String) As Boolean
        Dim conn As OleDbConnection = Common.DBConnection()
        Dim cmd As New OleDbCommand
        Dim dr As OleDbDataReader

        Try
            cmd = conn.CreateCommand()
            cmd.CommandText = "SELECT OID FROM T_Orders WHERE OExSource=@pExSource and OExSourceID=@pExSourceID"
            cmd.Parameters.Add("@pExSource", OleDbType.VarChar).Value = ExSource
            cmd.Parameters.Add("@pExSourceID", OleDbType.VarChar).Value = ExSourceID


            dr = cmd.ExecuteReader()

            Dim iOID As Integer
            While (dr.Read())
                iOID = dr(0)
            End While


            dr.Close()
            cmd.Dispose()
            conn.Close()

            Return iOID



        Catch ex As Exception
            AddToLog("Error occured in CheckDuplicateOrder: " & ex.Message, True)
            Return 0
        End Try


    End Function


    Public Function GetOrderProcessDT() As Date
        Dim conn As OleDbConnection = Common.DBConnection()
        Dim cmd As New OleDbCommand
        Dim dr As OleDbDataReader

        Try
            cmd = conn.CreateCommand()
            cmd.CommandText = "SELECT Max(T_OrderProcessLog.OrderFetchDT) AS MaxOfOrderFetchDT FROM T_OrderProcessLog"
            dr = cmd.ExecuteReader()

            Dim dtLOD As Date
            While (dr.Read())
                dtLOD = dr(0)
            End While


            dr.Close()
            cmd.Dispose()
            conn.Close()

            Return dtLOD


        Catch ex As Exception
            AddToLog("Error occured in GetOrderProcessDT: " & ex.Message, True)
            Return "1/1/1900"
        End Try
    End Function

    Public Sub UpdateOrderProcLog(dtOPL As Date, Status As String, orderCout As Integer, ProcessStartDT As Date, ProcessEndDT As Date)
        Try

            Dim conn As OleDbConnection = Common.DBConnection()

            Using cmd As OleDbCommand = conn.CreateCommand()

                cmd.CommandText = "INSERT INTO T_OrderProcessLog ([OrderFetchDT],[OrderFetchStatus], [OrderCount], [ProcessStartDT], [ProcessEndDT] " & _
                "VALUES (@OrderFetchDT, @OrderFetchStatus, @OrderCount, @ProcessStartDT, @ProcessEndDT)"


                cmd.Parameters.Add("@OrderFetchDT", OleDbType.DBDate).Value = dtOPL
                cmd.Parameters.Add("@OrderFetchStatus", OleDbType.VarChar).Value = Status
                cmd.Parameters.Add("@OrderCount", OleDbType.Integer).Value = orderCout
                cmd.Parameters.Add("@ProcessStartDT", OleDbType.DBDate).Value = ProcessStartDT
                cmd.Parameters.Add("@ProcessEndDT", OleDbType.Integer).Value = ProcessEndDT

                cmd.ExecuteNonQuery()

                
            End Using

            conn.Close()
        Catch ex As Exception
            AddToLog("Error occured in fbOrder.UpdateOrderProcLog: " & ex.Message, True)
        End Try

    End Sub
End Class


