Imports System.Data.SqlClient

Public Class WebForm2
    Inherits System.Web.UI.Page

#Region "Properties"

    Private Property dtProvince As DataTable
        Get
            Return ViewState("dtProvince")
        End Get
        Set(ByVal value As DataTable)
            ViewState("dtProvince") = value
        End Set
    End Property

    Private Property dtAmphoe As DataTable
        Get
            Return ViewState("dtAmphoe")
        End Get
        Set(ByVal value As DataTable)
            ViewState("dtAmphoe") = value
        End Set
    End Property

    Private Property dtTambon As DataTable
        Get
            Return ViewState("dtAmphoe")
        End Get
        Set(ByVal value As DataTable)
            ViewState("dtAmphoe") = value
        End Set
    End Property

#End Region

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Me.LoadDDProvince()
            Me.LoadDDAmphoe()
            Me.LoadDDTambon()
        End If
    End Sub
#End Region

#Region "Functions"

    Private Sub LoadDDProvince()
        Try
            Dim bdCon As New DBConnection()
            Dim ds As DataSet = bdCon.ExecuteDataset("dbo.Province_S_SelDDL", Nothing)

            Me.dtProvince = ds.Tables(0)
            Me.ddProvince.DataSource = Me.dtProvince
            Me.ddProvince.DataTextField = "Name"
            Me.ddProvince.DataValueField = "ID"
            Me.ddProvince.DataBind()
        Catch ex As Exception
            Throw New Exception("Fn-LoadDDProvince:[" & ex.Message & "]")
        End Try
    End Sub

    Private Sub LoadDDAmphoe()
        Try
            Dim bdCon As New DBConnection()
            Dim Param As New List(Of SqlParameter)() From {New SqlParameter("@SProv_Id", Me.ddProvince.SelectedValue)}

            Dim ds As DataSet = bdCon.ExecuteDataset("dbo.Amphoe_S_SelDDL", Param)

            Me.dtAmphoe = ds.Tables(0)
            Me.ddAmphoe.DataSource = Me.dtAmphoe
            Me.ddAmphoe.DataTextField = "Name"
            Me.ddAmphoe.DataValueField = "ID"
            Me.ddAmphoe.DataBind()
        Catch ex As Exception
            Throw New Exception("Fn-LoadDDAmphoe:[" & ex.Message & "]")
        End Try
    End Sub

    Private Sub LoadDDTambon()
        Try
            Dim bdCon As New DBConnection()
            Dim Param As New List(Of SqlParameter)() From {New SqlParameter("@SAmp_Id", Me.ddAmphoe.SelectedValue)}

            Dim ds As DataSet = bdCon.ExecuteDataset("dbo.Tambon_S_SelDDL", Param)

            Me.dtTambon = ds.Tables(0)
            Me.ddTambon.DataSource = Me.dtTambon
            Me.ddTambon.DataTextField = "Name"
            Me.ddTambon.DataValueField = "ID"
            Me.ddTambon.DataBind()

        Catch ex As Exception
            Throw New Exception("Fn-LoadDDTambon:[" & ex.Message & "]")
        End Try
    End Sub

#End Region
End Class