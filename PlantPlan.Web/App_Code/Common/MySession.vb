Imports System.Web
Imports Microsoft.VisualBasic

Public Class MySession

    Public Const Const_IsLogin As String = "MySession_IsLogin"
    Public Shared Property IsLogin() As Boolean
        Get
            Dim retval As Boolean = False

            If Not IsNothing(HttpContext.Current.Session(Const_IsLogin)) Then
                retval = HttpContext.Current.Session(Const_IsLogin)
            End If

            Return retval

        End Get
        Set(ByVal value As Boolean)
            HttpContext.Current.Session(Const_IsLogin) = value
        End Set
    End Property

    Public Const Const_ApplicationID As String = "MySession_ApplicationID"
    Public Shared Property ApplicationID() As Integer
        Get
            Dim retval As Integer = 0

            If Not IsNothing(HttpContext.Current.Session(Const_ApplicationID)) Then
                retval = HttpContext.Current.Session(Const_ApplicationID)
            End If

            Return retval

        End Get
        Set(ByVal value As Integer)
            HttpContext.Current.Session(Const_ApplicationID) = value
        End Set
    End Property

    Public Const Const_ApplicationName As String = "MySession_ApplicationName"
    Public Shared Property ApplicationName() As String
        Get
            Dim retval As String = ""

            If Not IsNothing(HttpContext.Current.Session(Const_ApplicationName)) Then
                retval = HttpContext.Current.Session(Const_ApplicationName)
            End If

            Return retval

        End Get
        Set(ByVal value As String)
            HttpContext.Current.Session(Const_ApplicationName) = value
        End Set
    End Property

    Public Const Const_CompanyID As String = "MySession_CompanyID"
    Public Shared Property CompanyID() As Integer
        Get
            Dim retval As Integer = 0

            If Not IsNothing(HttpContext.Current.Session(Const_CompanyID)) Then
                retval = HttpContext.Current.Session(Const_CompanyID)
            End If

            Return retval

        End Get
        Set(ByVal value As Integer)
            HttpContext.Current.Session(Const_CompanyID) = value
        End Set
    End Property

    Public Const Const_MainLanguageID As String = "MySession_MainLanguageID"
    Public Shared Property MainLanguageID() As Integer
        Get
            Dim retval As Integer = 0

            If Not IsNothing(HttpContext.Current.Session(Const_MainLanguageID)) Then
                retval = HttpContext.Current.Session(Const_MainLanguageID)
            End If

            Return retval

        End Get
        Set(ByVal value As Integer)
            HttpContext.Current.Session(Const_MainLanguageID) = value
        End Set
    End Property

    Public Const Const_MainCurrencyID As String = "MySession_MainCurrencyID"
    Public Shared Property MainCurrencyID() As Integer
        Get
            Dim retval As Integer = 0

            If Not IsNothing(HttpContext.Current.Session(Const_MainCurrencyID)) Then
                retval = HttpContext.Current.Session(Const_MainCurrencyID)
            End If

            Return retval

        End Get
        Set(ByVal value As Integer)
            HttpContext.Current.Session(Const_MainCurrencyID) = value
        End Set
    End Property

    Public Const Const_UserID As String = "MySession_UserID"
    Public Shared Property UserID() As Integer
        Get
            Dim retval As Integer = 0

            If Not IsNothing(HttpContext.Current.Session(Const_UserID)) Then
                retval = HttpContext.Current.Session(Const_UserID)
            End If

            Return retval

        End Get
        Set(ByVal value As Integer)
            HttpContext.Current.Session(Const_UserID) = value
        End Set
    End Property

    Public Const Const_UserName As String = "MySession_UserName"
    Public Shared Property UserName() As String
        Get
            Dim retval As String = ""

            If Not IsNothing(HttpContext.Current.Session(Const_UserName)) Then
                retval = HttpContext.Current.Session(Const_UserName)
            End If

            Return retval

        End Get
        Set(ByVal value As String)
            HttpContext.Current.Session(Const_UserName) = value
        End Set
    End Property

    Public Const Const_UserEmail As String = "MySession_UserEmail"
    Public Shared Property UserEmail() As String
        Get
            Dim retval As String = ""

            If Not IsNothing(HttpContext.Current.Session(Const_UserEmail)) Then
                retval = HttpContext.Current.Session(Const_UserEmail)
            End If

            Return retval

        End Get
        Set(ByVal value As String)
            HttpContext.Current.Session(Const_UserEmail) = value
        End Set
    End Property

    Public Const Const_RoleID As String = "MySession_RoleID"
    Public Shared Property RoleID() As Integer
        Get
            Dim retval As Integer = 0

            If Not IsNothing(HttpContext.Current.Session(Const_RoleID)) Then
                retval = HttpContext.Current.Session(Const_RoleID)
            End If

            Return retval

        End Get
        Set(ByVal value As Integer)
            HttpContext.Current.Session(Const_RoleID) = value
        End Set
    End Property

    Public Const Const_SessionLastActive As String = "MySession_SessionLastActive"
    Public Shared Property SessionLastActive() As DateTime
        Get
            Dim retval As DateTime = Now.AddDays(-1)

            If Not IsNothing(HttpContext.Current.Session(Const_SessionLastActive)) Then
                retval = HttpContext.Current.Session(Const_SessionLastActive)
            End If

            Return retval

        End Get
        Set(ByVal value As DateTime)
            HttpContext.Current.Session(Const_SessionLastActive) = value
        End Set
    End Property

    Public Shared Sub SetSessionLastActive()
        SessionLastActive = Now
    End Sub

    Public Shared Function GetSessionTimeout() As Integer
        Return CInt(DateDiff(DateInterval.Minute, SessionLastActive, Now))
    End Function

    Public Shared Sub KeepLogin(ByVal AppName As String, ByVal LoginUsername As String)
        MySession.ApplicationName = AppName
        MySession.UserName = LoginUsername
        MySession.IsLogin = True
    End Sub

    Public Shared Sub ClearAll()

        HttpContext.Current.Session.RemoveAll()

    End Sub

End Class
