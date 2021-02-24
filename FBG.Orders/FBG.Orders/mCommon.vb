
Imports System.Data.OleDb

Module Common
    Public Function DBConnection()
        Try
            Dim conn As OleDbConnection = New OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\FBG\DB\FBG.accdb")
            conn.Open()

            Return conn
        Catch ex As Exception
            Dim errmessage As String = "Error Opening connection - mCommon.DBConnection - " & ex.Message

        End Try
    End Function

End Module
