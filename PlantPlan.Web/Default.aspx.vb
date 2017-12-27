Imports Telerik.Web.UI

Partial Class _Default
    Inherits PageBase


#Region "Properties"

    ReadOnly Property AdminUserId As Integer = 0
    ReadOnly Property AdminUser As String = "Admin"
    ReadOnly Property AdminPassword As String = "admPassw0rd"
    ReadOnly Property AdminRole As Integer = 1


    ReadOnly Property DemoUserId As Integer = 1
    ReadOnly Property DemoUser As String = "Demo"
    ReadOnly Property DemoPassword As String = "demo"
    ReadOnly Property DemoRole As Integer = 2

#End Region

#Region "Events"

    Private Sub btnSignin_Click(sender As Object, e As EventArgs) Handles btnSignin.Click
        Try
            If (Me.AdminUser = Me.txtUserName.Text And Me.AdminPassword = Me.txtPassword.Text) Then
                MySession.UserID = Me.AdminUserId
                MySession.UserName = Me.AdminUser
                MySession.RoleID = Me.AdminRole
                MySession.IsLogin = True
                Me.Page.Response.Redirect("Pages/Suggestion.aspx")

            ElseIf (Me.DemoUser = Me.txtUserName.Text And Me.DemoPassword = Me.txtPassword.Text) Then
                MySession.UserID = Me.DemoUserId
                MySession.UserName = Me.DemoUser
                MySession.RoleID = Me.DemoRole
                MySession.IsLogin = True
                Me.Page.Response.Redirect("Pages/Suggestion.aspx")
            Else
                MySession.IsLogin = False
                Me.lbMsg.Text = "ชื่อผู้ใช้งาน หรือ รหัสผ่านไม่ถูกต้อง!"
            End If
        Catch ex As Exception
            JavaScript.Alert(Me.Page, "Ev-btnSignin_Click : [" & ex.Message & "]")
        End Try
    End Sub

#End Region

End Class
