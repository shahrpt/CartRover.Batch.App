
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

Public Class fbOrderDetail
    Private _OrderDetID As Integer
    Private _OrderID As Integer
    Private _ProductID As Integer
    Private _Quantity As Integer
    Private _UnitPrice As Decimal
    Private _Discount As Decimal




    Public ReadOnly Property OrderDetID As System.Int32
        Get
            Return _OrderDetID
        End Get
    End Property

    Public Property OrderID As System.Int32
        Get
            Return _OrderID
        End Get
        Set(ByVal value As System.Int32)
            _OrderID = value
        End Set
    End Property

    Public Property ProductID As System.Int32
        Get
            Return _ProductID
        End Get
        Set(ByVal value As System.Int32)
            _ProductID = value
        End Set
    End Property

    Public Property Quantity As System.Int32
        Get
            Return _Quantity
        End Get
        Set(ByVal value As System.Int32)
            _Quantity = value
        End Set
    End Property

    Public Property UnitPrice As System.Decimal
        Get
            Return _UnitPrice
        End Get
        Set(ByVal value As System.Decimal)
            _UnitPrice = value
        End Set
    End Property

    Public Property Discount As System.Decimal
        Get
            Return _Discount
        End Get
        Set(ByVal value As System.Decimal)
            _Discount = value
        End Set
    End Property

    Public Sub Save()
        Try
            Dim conn As OleDbConnection = Common.DBConnection()

            Using cmd As OleDbCommand = conn.CreateCommand()

                cmd.CommandText = "INSERT INTO T_OrderDetails " & "([ODOID],[ODProductID],[ODQuantity],[ODUnitPrice],[ODDiscount]) " & "VALUES(@OrderID,@ProductID,@Quantity,@UnitPrice,@Discount)"



                cmd.Parameters.Add("@OrderID", OleDbType.Integer).Value = OrderID
                cmd.Parameters.Add("@ProductID", OleDbType.Integer).Value = ProductID
                cmd.Parameters.Add("@Quantity", OleDbType.Integer).Value = Quantity
                cmd.Parameters.Add("@UnitPrice", OleDbType.Decimal).Value = UnitPrice
                cmd.Parameters.Add("@Discount", OleDbType.Decimal).Value = Discount
               
                cmd.ExecuteNonQuery()

                Dim QryID As String = "Select @@Identity"
                cmd.CommandText = QryID

                Dim iResult As Integer = cmd.ExecuteScalar()



                If iResult > 0 Then
                    _OrderDetID = iResult

                End If
                conn.Close()
            End Using

            conn.Close()
        Catch ex As Exception
            AddToLog("Error occured in fbOrderdetail.save: " & ex.Message, True)

        End Try


    End Sub

    Public Function GetProdID(sCode As String) As Integer
      
        Dim conn As OleDbConnection = Common.DBConnection()

        Dim cmd As New OleDbCommand
        Dim dr As OleDbDataReader



        Try

            cmd = conn.CreateCommand()
            cmd.CommandText = "SELECT PID, SKUCode, UPCCode FROM T_Products WHERE SKUCode=@pSKU OR UPCCode=@pUPC"
            cmd.Parameters.Add("@CodeSKU", OleDbType.VarChar).Value = sCode
            cmd.Parameters.Add("@CodeUPC", OleDbType.VarChar).Value = sCode


            dr = cmd.ExecuteReader()

            Dim iPID As Integer
            While (dr.Read())
                iPID = dr(0)
            End While


            dr.Close()
            cmd.Dispose()
            conn.Close()

            Return iPID



        Catch ex As Exception
            AddToLog("Error occured in fbOrderdetail.GetProdID: " & ex.Message, True)
            Return 0
        End Try


    End Function


End Class


