Imports Microsoft.VisualBasic

Public Class WebConfig

    Public Shared ReadOnly Property DomainName() As String
        Get
            If Not IsNothing(ConfigurationManager.AppSettings("DomainName")) Then
                Return ConfigurationManager.AppSettings("DomainName")
            Else
                Return ""
            End If
        End Get
    End Property

    Public Shared ReadOnly Property ApplicationId() As String
        Get
            If Not IsNothing(ConfigurationManager.AppSettings("ApplicationId")) Then
                Return ConfigurationManager.AppSettings("ApplicationId")
            Else
                Return ""
            End If
        End Get
    End Property

    Public Shared ReadOnly Property ApplicationName() As String
        Get
            If Not IsNothing(ConfigurationManager.AppSettings("ApplicationName")) Then
                Return ConfigurationManager.AppSettings("ApplicationName")
            Else
                Return ""
            End If
        End Get
    End Property

    Public Shared ReadOnly Property CompanyId() As String
        Get
            If Not IsNothing(ConfigurationManager.AppSettings("CompanyId")) Then
                Return ConfigurationManager.AppSettings("CompanyId")
            Else
                Return ""
            End If
        End Get
    End Property

    Public Shared ReadOnly Property AppVersion() As String
        Get
            If Not IsNothing(ConfigurationManager.AppSettings("AppVersion")) Then
                Return ConfigurationManager.AppSettings("AppVersion")
            Else
                Return ""
            End If
        End Get
    End Property

    Public Shared ReadOnly Property HasSSL() As String
        Get
            If Not IsNothing(ConfigurationManager.AppSettings("HasSSL")) Then
                Return ConfigurationManager.AppSettings("HasSSL")
            Else
                Return ""
            End If
        End Get
    End Property

    Public Shared ReadOnly Property ReportServerUrl() As String
        Get
            If Not IsNothing(ConfigurationManager.AppSettings("ReportServerUrl")) Then
                Return ConfigurationManager.AppSettings("ReportServerUrl")
            Else
                Return ""
            End If
        End Get
    End Property

    Public Shared ReadOnly Property DateTimeCulture() As String
        Get
            If Not IsNothing(ConfigurationManager.AppSettings("DateTimeCulture")) Then
                Return ConfigurationManager.AppSettings("DateTimeCulture")
            Else
                Return ""
            End If
        End Get
    End Property

    Public Shared ReadOnly Property DateFormat() As String
        Get
            If Not IsNothing(ConfigurationManager.AppSettings("DateFormat")) Then
                Return ConfigurationManager.AppSettings("DateFormat")
            Else
                Return ""
            End If
        End Get
    End Property

    Public Shared ReadOnly Property encryptKey() As String
        Get
            If Not IsNothing(ConfigurationManager.AppSettings("encryptKey")) Then
                Return ConfigurationManager.AppSettings("encryptKey")
            Else
                Return ""
            End If
        End Get
    End Property


    Public Shared ReadOnly Property ReportServerDomain() As String
        Get
            If Not IsNothing(ConfigurationManager.AppSettings("ReportServerDomain")) Then
                Return ConfigurationManager.AppSettings("ReportServerDomain")
            Else
                Return ""
            End If
        End Get
    End Property

    Public Shared ReadOnly Property ReportServerUserName() As String
        Get
            If Not IsNothing(ConfigurationManager.AppSettings("ReportServerUserName")) Then
                Return ConfigurationManager.AppSettings("ReportServerUserName")
            Else
                Return ""
            End If
        End Get
    End Property

    Public Shared ReadOnly Property ReportServerPassword() As String
        Get
            If Not IsNothing(ConfigurationManager.AppSettings("ReportServerPassword")) Then
                Return ConfigurationManager.AppSettings("ReportServerPassword")
            Else
                Return ""
            End If
        End Get
    End Property

    Public Shared ReadOnly Property DownloadFolder() As String
        Get
            If Not IsNothing(ConfigurationManager.AppSettings("DownloadFolder")) Then
                Return ConfigurationManager.AppSettings("DownloadFolder")
            Else
                Throw New Exception("Class:WebConfig; Method:DownloadFolder; Error:Not found download folder in config file;")
            End If
        End Get
    End Property

    Public Shared ReadOnly Property SSOType() As SSO_Type_Enum
        Get
            Dim chk As String = "0"
            Dim rst As SSO_Type_Enum = SSO_Type_Enum.None

            If Not IsNothing(ConfigurationManager.AppSettings("SSOType")) Then
                chk = ConfigurationManager.AppSettings("SSOType")

                If chk.Trim <> "" Then rst = chk

            End If

            Return rst
        End Get
    End Property

    Public Shared ReadOnly Property ConnectionString() As String
        Get
            If Not IsNothing(ConfigurationManager.ConnectionStrings("tw.salesanalysis.dal.My.MySettings.ConnectionString1")) Then
                Return ConfigurationManager.ConnectionStrings("tw.salesanalysis.dal.My.MySettings.ConnectionString1").ToString

            Else
                Throw New Exception("Class:WebConfig; Method:ConnectionString; Error:Not found Parameter ConnectionStrings;")
            End If
        End Get
    End Property

    Public Shared ReadOnly Property MailHost() As String
        Get
            If Not IsNothing(ConfigurationManager.AppSettings("MailHost")) Then
                Return ConfigurationManager.AppSettings("MailHost")
            Else
                Return ""
            End If
        End Get
    End Property

    Public Shared ReadOnly Property MailPort() As String
        Get
            If Not IsNothing(ConfigurationManager.AppSettings("MailPort")) Then
                Return ConfigurationManager.AppSettings("MailPort")
            Else
                Return ""
            End If
        End Get
    End Property

    Public Shared ReadOnly Property MailEnableSsl() As Boolean
        Get
            If Not IsNothing(ConfigurationManager.AppSettings("MailEnableSsl")) Then
                Return ConfigurationManager.AppSettings("MailEnableSsl")
            Else
                Return ""
            End If
        End Get
    End Property

    Public Shared ReadOnly Property MailLoginName() As String
        Get
            If Not IsNothing(ConfigurationManager.AppSettings("MailLoginName")) Then
                Return ConfigurationManager.AppSettings("MailLoginName")
            Else
                Return ""
            End If
        End Get
    End Property

    Public Shared ReadOnly Property MailLoginPassword() As String
        Get
            If Not IsNothing(ConfigurationManager.AppSettings("MailLoginPassword")) Then
                Return ConfigurationManager.AppSettings("MailLoginPassword")
            Else
                Return ""
            End If
        End Get
    End Property

    Public Shared ReadOnly Property MailDeliveryMethod() As Integer
        Get
            If Not IsNothing(ConfigurationManager.AppSettings("MailDeliveryMethod")) Then
                Return ConfigurationManager.AppSettings("MailDeliveryMethod")
            Else
                Return ""
            End If
        End Get
    End Property

    Public Shared ReadOnly Property MailUseDefaultCredentials() As Boolean
        Get
            If Not IsNothing(ConfigurationManager.AppSettings("MailUseDefaultCredentials")) Then
                Return ConfigurationManager.AppSettings("MailUseDefaultCredentials")
            Else
                Return ""
            End If
        End Get
    End Property

    Public Shared ReadOnly Property MailIsBodyHtml() As Boolean
        Get
            If Not IsNothing(ConfigurationManager.AppSettings("MailIsBodyHtml")) Then
                Return ConfigurationManager.AppSettings("MailIsBodyHtml")
            Else
                Return ""
            End If
        End Get
    End Property

    Public Shared ReadOnly Property MailDefaultSender() As String
        Get
            If Not IsNothing(ConfigurationManager.AppSettings("MailDefaultSender")) Then
                Return ConfigurationManager.AppSettings("MailDefaultSender")
            Else
                Return ""
            End If
        End Get
    End Property

    Public Shared ReadOnly Property ImagePath() As String
        Get
            Dim url As String = ""

            If Not IsNothing(ConfigurationManager.AppSettings("ImagePath")) Then
                url = ConfigurationManager.AppSettings("ImagePath")

                If Not url.Trim.EndsWith("/") Then
                    url = url.Trim
                End If
            Else
                Throw New Exception("Class:WebConfig; Method:DefaultImagePath; Error:Not found DefaultImagePath in config file;")
            End If

            Return url
        End Get
    End Property

    Public Shared ReadOnly Property DefaultImagePath() As String
        Get
            Dim url As String = ""

            If Not IsNothing(ConfigurationManager.AppSettings("DefaultImagePath")) Then
                url = ConfigurationManager.AppSettings("DefaultImagePath")

                If Not url.Trim.EndsWith("/") Then
                    url = url.Trim
                End If
            Else
                Throw New Exception("Class:WebConfig; Method:DefaultImagePath; Error:Not found DefaultImagePath in config file;")
            End If

            Return url
        End Get
    End Property

    Public Shared ReadOnly Property DefaultNoImageOnServerPath() As String
        Get
            Dim url As String = ""

            If Not IsNothing(ConfigurationManager.AppSettings("DefaultNoImageOnServerPath")) Then
                url = ConfigurationManager.AppSettings("DefaultNoImageOnServerPath")

                If Not url.Trim.EndsWith("/") Then
                    url = url.Trim
                End If
            Else
                Throw New Exception("Class:WebConfig; Method:DefaultNoImageOnServerPath; Error:Not found DefaultNoImageOnServerPath in config file;")
            End If

            Return url
        End Get
    End Property

    Public Shared ReadOnly Property UrlSite() As String
        Get
            If Not IsNothing(ConfigurationManager.AppSettings("UrlSite")) Then
                Return ConfigurationManager.AppSettings("UrlSite")
            Else
                Return ""
            End If
        End Get
    End Property

    Public Shared ReadOnly Property UrlDefaultPage() As String
        Get
            If Not IsNothing(ConfigurationManager.AppSettings("UrlDefaultPage")) Then
                Return ConfigurationManager.AppSettings("UrlDefaultPage")
            Else
                Return ""
            End If
        End Get
    End Property

    Public Shared ReadOnly Property UrlLoginForm() As String
        Get
            If Not IsNothing(ConfigurationManager.AppSettings("UrlLogin")) Then
                Return ConfigurationManager.AppSettings("UrlLogin")
            Else
                Return ""
            End If
        End Get
    End Property

    Public Shared ReadOnly Property UrlForgotPasswordForm() As String
        Get
            If Not IsNothing(ConfigurationManager.AppSettings("UrlForgotPwd")) Then
                Return ConfigurationManager.AppSettings("UrlForgotPwd")
            Else
                Return ""
            End If
        End Get
    End Property

    Public Shared ReadOnly Property UrlChangePasswordForm() As String
        Get
            If Not IsNothing(ConfigurationManager.AppSettings("UrlChgPwd")) Then
                Return ConfigurationManager.AppSettings("UrlChgPwd")
            Else
                Return ""
            End If
        End Get
    End Property

    Public Shared ReadOnly Property SENDGRID_APIKEY() As String
        Get
            If Not IsNothing(ConfigurationManager.AppSettings("SENDGRID_APIKEY")) Then
                Return ConfigurationManager.AppSettings("SENDGRID_APIKEY")
            Else
                Return ""
            End If
        End Get
    End Property

End Class
