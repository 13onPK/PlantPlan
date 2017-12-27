Imports System.Data

Public Class PageMaster
    Inherits MasterPageBase

    Public Property MenuID As Integer
        Get
            Return ViewState("MenuID")
        End Get
        Set(ByVal value As Integer)
            ViewState("MenuID") = value
        End Set
    End Property

#Region "Event"

    Private Sub Masters_Page_Init(sender As Object, e As EventArgs) Handles Me.Init
        Try
            If ConfigurationManager.AppSettings("SSOType") = 0 Then
                MySession.IsLogin = 1
                MySession.UserID = ConfigurationManager.AppSettings("TestUserID")
                MySession.UserName = ConfigurationManager.AppSettings("TestUserName")
                MySession.RoleID = ConfigurationManager.AppSettings("TestRoleID")
            End If
        Catch ex As Exception
            JavaScript.Alert(Me.Page, "Ev-Masters_Page_Init : [" & ex.Message & "]")
        End Try
    End Sub

    Private Sub Masters_Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                If MySession.IsLogin Then
                    Me.lkCompany.HRef = WebConfig.UrlDefaultPage
                    Me.lbUserName.Text = "USER : " + MySession.UserName
                    Me.BindMenu()
                Else
                    Dim CurrentPage = HttpUtility.UrlEncode(Request.Url.AbsolutePath.Trim())
                    Dim url = WebConfig.UrlLoginForm.ToString() + "?Url=" + CurrentPage
                    Me.Page.Response.Redirect(url, False)
                End If
            ElseIf MySession.IsLogin = False Then
                Dim CurrentPage = HttpUtility.UrlEncode(Request.Url.AbsolutePath.Trim())
                Dim url = WebConfig.UrlLoginForm.ToString()
                Me.Page.Response.Redirect(url, False)
            End If
        Catch ex As Exception
            JavaScript.Alert(Me.Page, "Ev-Pages_MasterPage_Load : [" & ex.Message & "]")
        End Try

    End Sub

    Private Sub lnkLogoff_Click(sender As Object, e As EventArgs) Handles lnkLogoff.Click
        Try
            MySession.ClearAll()
            Dim url = WebConfig.UrlLoginForm.ToString()
            Me.Page.Response.Redirect(url, False)
        Catch ex As Exception
            JavaScript.Alert(Me.Page, "Ev-Pages_MasterPage_Load : [" & ex.Message & "]")
        End Try
    End Sub

#End Region

#Region "Function"

    Private Sub BindMenu()
        Try
            Dim dt As New DataTable
            dt.Columns.Add("MMENU_ID", System.Type.GetType("System.Int32"))
            dt.Columns.Add("MMENU_NAME")
            dt.Columns.Add("MMENU_PARENTMENUID", System.Type.GetType("System.Int32"))
            dt.Columns.Add("MMENU_URL")

            Dim drSuggestion As DataRow = dt.NewRow
            drSuggestion("MMENU_ID") = 1
            drSuggestion("MMENU_NAME") = "Suggestion"
            drSuggestion("MMENU_URL") = "~/Pages/Suggestion.aspx"
            dt.Rows.Add(drSuggestion)

            If MySession.RoleID = 1 Then
                Dim drManagement As DataRow = dt.NewRow
                drManagement("MMENU_ID") = 2
                drManagement("MMENU_NAME") = "Management"
                drManagement("MMENU_URL") = "~/Pages/Management.aspx"
                dt.Rows.Add(drManagement)
            End If

            mnuMain.DataFieldID = "MMENU_ID"
            mnuMain.DataTextField = "MMENU_NAME"
            mnuMain.DataValueField = "MMENU_ID"
            mnuMain.DataFieldParentID = "MMENU_PARENTMENUID"
            mnuMain.DataNavigateUrlField = "MMENU_URL"
            mnuMain.DataSource = dt
            mnuMain.DataBind()
        Catch ex As Exception
            Throw New Exception("Fn-BindMenu : [" & ex.Message & "]")
        End Try
    End Sub

#End Region

End Class