Public Class JavaScript
    Inherits System.Web.UI.Page

    Shared Sub Alert(ByVal P As System.Web.UI.Page, ByVal msg As String)

        msg = EscapeSequence(msg)

        Dim jscript As String = "alert(""" & Replace(Replace(msg, """", "'"), vbCrLf, "\n") & """);" & vbCrLf
        ScriptManager.RegisterStartupScript(P, P.GetType(), "key", jscript, True)

    End Sub

    Shared Function EscapeSequence(ByVal Original As String) As String
        Dim local0 As String
        Dim local1 As Boolean

        local1 = Microsoft.VisualBasic.Information.IsNothing(Original)
        If local1 Then
            local0 = ""
        Else
            local0 = Original.Replace("\", "\\").Replace("'", "\'").Replace("""", "\""").Replace(vbCr, "\r").Replace(vbLf, "\n").Replace(vbTab, "\t")
        End If
        Return local0
    End Function

End Class
