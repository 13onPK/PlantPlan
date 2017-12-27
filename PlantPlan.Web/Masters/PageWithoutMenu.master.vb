Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Net

Partial Class Masters_Page
    Inherits MasterPageBase

#Region "Event"

    Private Sub Masters_Page_Init(sender As Object, e As EventArgs) Handles Me.Init
        Try
            If ConfigurationManager.AppSettings("SSOType") = 0 Then
                MySession.IsLogin = 1
                MySession.UserID = ConfigurationManager.AppSettings("TestUserID")
                MySession.UserName = ConfigurationManager.AppSettings("TestUserName")
                MySession.UserEmail = ConfigurationManager.AppSettings("TestUserEmail")
                MySession.RoleID = ConfigurationManager.AppSettings("TestRoleID")
            End If
        Catch ex As Exception
            JavaScript.Alert("Ev-Masters_Page_Init : [" & ex.Message & "]")
        End Try
    End Sub

    Private Sub Masters_Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                Me.lbCompanyName.Text = WebConfig.FullNameCompany
                Me.lkCompany.HRef = WebConfig.UrlDefaultPage
            End If
        Catch ex As Exception
            JavaScript.Alert("Ev-Pages_MasterPage_Load : [" & ex.Message & "]")
        End Try

    End Sub

#End Region

End Class

