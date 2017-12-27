Public Class ManagementSession
    Private Const Const_PageAction As String = "ManagementSession_PageAction"
    Public Shared Property PageAction As PageAction_Enum
        Get
            Dim retval As PageAction_Enum = 0

            If Not IsNothing(HttpContext.Current.Session(Const_PageAction)) Then
                retval = HttpContext.Current.Session(Const_PageAction)
            End If

            Return retval

        End Get
        Set(ByVal value As PageAction_Enum)
            HttpContext.Current.Session(Const_PageAction) = value
        End Set
    End Property

    Private Const Const_TPgc_Id As String = "ManagementSession_TPgc_Id"
    Public Shared Property TPgc_Id As Integer
        Get
            Dim retval As Integer = 0

            If Not IsNothing(HttpContext.Current.Session(Const_TPgc_Id)) Then
                retval = HttpContext.Current.Session(Const_TPgc_Id)
            End If

            Return retval

        End Get
        Set(ByVal value As Integer)
            HttpContext.Current.Session(Const_TPgc_Id) = value
        End Set
    End Property

    Public Shared Sub ClearAll()
        ManagementSession.PageAction = Nothing
        ManagementSession.TPgc_Id = Nothing
    End Sub
End Class
