Imports Microsoft.VisualBasic

Public Class PageBase
    Inherits System.Web.UI.Page

    Public Function RelativePath() As String

        Dim strApplication As String = Me.Request.CurrentExecutionFilePath
        Dim FullPathUrl As String = Me.Request.Url.AbsoluteUri
        Dim ApplicationPath As String = Me.Request.ApplicationPath
        Dim pos As Integer = FullPathUrl.IndexOf(Me.Request.CurrentExecutionFilePath)
        Dim PreUrl As String = FullPathUrl.Substring(0, pos)

        If ApplicationPath.Trim <> "/" Then
            PreUrl = PreUrl & ApplicationPath
        End If

        Return PreUrl & "/"

    End Function

End Class
