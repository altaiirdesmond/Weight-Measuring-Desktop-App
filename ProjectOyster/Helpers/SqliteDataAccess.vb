﻿Imports System.Data.SQLite
Imports Dapper
Imports ProjectOyster.Models

Namespace Helpers

    Public Class SqliteDataAccess

        Public Shared Sub AddOyster(oyster As Oyster)
            Using cnn As IDbConnection = New SQLiteConnection(LoadConnectionString())
                Try
                    cnn.Execute("INSERT INTO Oyster VALUES(@Time, @Weight, @User)", oyster)
                    MsgBox("added to database!")
                Catch ex As Exception
                    MessageBox.Show(ex.Message)
                End Try
            End Using
        End Sub

        Public Shared Function GetAll() As List(Of Oyster)
            Using cnn As IDbConnection = New SQLiteConnection(LoadConnectionString())
                Return cnn.Query(Of Oyster)("SELECT * FROM Oyster").ToList()
            End Using
        End Function

        Public Shared Function GetUserCount(username As String, password As String) As Integer
            Using cnn As IDbConnection = New SQLiteConnection(LoadConnectionString())
                Return cnn.Query(Of User)("SELECT * FROM User WHERE Username=@username AND Password=@password", New With {
                                             username,
                                             password
                                             }).Count()
            End Using
        End Function

        Public Shared Function IsAdmin(username As String, password As String) As Boolean
            Dim admin = False
            Using cnn As IDbConnection = New SQLiteConnection(LoadConnectionString())
                For Each user In cnn.Query(Of User)("SELECT Admin FROM User WHERE Username=@username AND Password=@password", New With {
                                                       username,
                                                       password
                                                       }).ToList().Take(1)
                    If user.Admin = 1 Then
                        admin = True
                    End If
                Next
            End Using

            Return admin
        End Function

        Private Shared Function LoadConnectionString() As String
            Return Configuration.ConfigurationManager.ConnectionStrings("Default").ConnectionString
        End Function
    End Class
End Namespace