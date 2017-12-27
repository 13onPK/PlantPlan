Imports Telerik.Web.UI
Imports System.Data.SqlClient
Imports System.IO
Imports System.Net
Imports System.Linq

Public Class ManagementForm
    Inherits PageBase

#Region "Properties"

    Private Property PageAction As PageAction_Enum
        Get
            Return ViewState("PageAction")
        End Get
        Set(ByVal value As PageAction_Enum)
            ViewState("PageAction") = value
        End Set
    End Property

    Private Property TPgc_Id As Integer
        Get
            Return ViewState("TPgc_Id")
        End Get
        Set(ByVal value As Integer)
            ViewState("TPgc_Id") = value
        End Set
    End Property

    Private Property lat As Decimal
        Get
            Return ViewState("lat")
        End Get
        Set(ByVal value As Decimal)
            ViewState("lat") = value
        End Set
    End Property

    Private Property lng As Decimal
        Get
            Return ViewState("lng")
        End Get
        Set(ByVal value As Decimal)
            ViewState("lng") = value
        End Set
    End Property

#End Region

#Region "Events"

    Private Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                If MySession.IsLogin = False Then
                    Me.Page.Response.Redirect("../Default.aspx")

                Else
                    Me.PageAction = ManagementSession.PageAction
                    Me.TPgc_Id = ManagementSession.TPgc_Id

                    Me.LoadDDPlantGroup()

                    If Me.TPgc_Id > 0 Then
                        Me.LoadData()
                    End If

                    If Me.PageAction = PageAction_Enum.View Then
                        Me.btnSave.Visible = False
                    End If
                End If

                ManagementSession.ClearAll()
            End If
        Catch ex As Exception
            JavaScript.Alert(Me.Page, "Ev-Page_Load:[" & ex.Message & "]")
        End Try
    End Sub

    Protected Sub CalProfit_TextChanged(sender As Object, e As EventArgs)
        Try
            If Me.txtRealCost.Text <> "" AndAlso Me.txtRealProduct.Text <> "" AndAlso Me.txtRealPrice.Text <> "" Then
                Me.txtRealProfit.Value = CDec((CDec((CDec(Me.txtRealProduct.Value) * CDec(Me.txtArea.Value) * CDec(Me.txtRealPrice.Value)) / 1000) - CDec(Me.txtArea.Value) * CDec(Me.txtRealCost.Value)) / (DateDiff(DateInterval.Day, CDate(Me.dpStartDate.SelectedDate), CDate(Me.dpEndDate.SelectedDate)) / 30)).ToString("##,###.##")
            End If
        Catch ex As Exception
            JavaScript.Alert(Me.Page, "Ev-CalProfit_TextChanged:[" & ex.Message & "]")
        End Try
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Try
            Dim bdCon As New DBConnection()
            Dim Param As New List(Of SqlParameter)() From {New SqlParameter("@TPgc_Id", Me.TPgc_Id),
                                                           New SqlParameter("@TPgc_HarvestDate", Me.dpEndDate.SelectedDate),
                                                           New SqlParameter("@TPgc_RealCost", Me.txtRealCost.Value),
                                                           New SqlParameter("@TPgc_RealProduct", Me.txtRealProduct.Value),
                                                           New SqlParameter("@TPgc_RealPrice", Me.txtRealPrice.Value),
                                                           New SqlParameter("@TPgc_RealProfit", Me.txtRealProfit.Value),
                                                           New SqlParameter("@TPgc_IsCompleted", Me.cbCompleted.Checked),
                                                           New SqlParameter("@CrBy", 0)
                                                          }

            Dim ds As DataSet = bdCon.ExecuteDataset("dbo.PlantGroupCrop_T_UpdateCompleted", Param)

            If ds.Tables(0).Rows.Count = 1 AndAlso ds.Tables(0).Rows(0)("Msg") = "Success" Then
                JavaScript.Alert(Me.Page, "Save successed.")
                Me.Page.Response.Redirect("Management.aspx")

            Else
                JavaScript.Alert(Me.Page, "Save failed!!")
            End If
        Catch ex As Exception
            JavaScript.Alert(Me.Page, "Ev-btnSave_Click:[" & ex.Message & "]")
        End Try
    End Sub

    Private Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
        Try
            Me.Page.Response.Redirect("Management.aspx")
        Catch ex As Exception
            JavaScript.Alert(Me.Page, "Ev-btnBack_Click:[" & ex.Message & "]")
        End Try
    End Sub

#End Region

#Region "function"

    Private Sub LoadDDProvince()
        Try
            Dim bdCon As New DBConnection()
            Dim ds As DataSet = bdCon.ExecuteDataset("dbo.Province_S_SelDDL", Nothing)

            Me.ddProvince.DataSource = ds.Tables(0)
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

            Me.ddAmphoe.DataSource = ds.Tables(0)
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

            Me.ddTambon.DataSource = ds.Tables(0)
            Me.ddTambon.DataTextField = "Name"
            Me.ddTambon.DataValueField = "ID"
            Me.ddTambon.DataBind()

        Catch ex As Exception
            Throw New Exception("Fn-LoadDDTambon:[" & ex.Message & "]")
        End Try
    End Sub

    Private Sub LoadDDPlantGroup()
        Try
            Dim bdCon As New DBConnection()
            Dim Param As New List(Of SqlParameter)() From {}

            Dim ds As DataSet = bdCon.ExecuteDataset("dbo.PlantGroup_S_SelDDL", Param)

            Me.ddPlantGroup.DataSource = ds.Tables(0)
            Me.ddPlantGroup.DataTextField = "Name"
            Me.ddPlantGroup.DataValueField = "ID"
            Me.ddPlantGroup.DataBind()
        Catch ex As Exception
            Throw New Exception("Fn-LoadDDPlantGroup:[" & ex.Message & "]")
        End Try
    End Sub

    Private Sub LoadData()
        Try
            Dim bdCon As New DBConnection()
            Dim Param As New List(Of SqlParameter)() From {New SqlParameter("@TPgc_Id", Me.TPgc_Id)}
            Dim ds As DataSet = bdCon.ExecuteDataset("dbo.PlantGroupCrop_T_SelById", Param)

            If ds.Tables.Count = 1 AndAlso ds.Tables(0).Rows.Count = 1 Then
                Me.ddPlantGroup.SelectedValue = ds.Tables(0).Rows(0)("TPgc_SPntgp_Id")
                Me.LoadDDProvince()
                Me.ddProvince.SelectedValue = ds.Tables(0).Rows(0)("TPgc_SProv_Id")
                Me.LoadDDAmphoe()
                Me.ddAmphoe.SelectedValue = ds.Tables(0).Rows(0)("TPgc_SAmp_Id")
                Me.LoadDDTambon()
                Me.ddTambon.SelectedValue = ds.Tables(0).Rows(0)("TPgc_STamb_Id")
                Me.lat = ds.Tables(0).Rows(0)("TPgc_Latitude")
                Me.lng = ds.Tables(0).Rows(0)("TPgc_Longitude")
                Me.txtArea.Value = CDec(ds.Tables(0).Rows(0)("TPgc_Area"))
                Me.txtFarmer.Text = ds.Tables(0).Rows(0)("TPgc_FarmerName").ToString
                Me.dpStartDate.SelectedDate = CDate(ds.Tables(0).Rows(0)("TPgc_StartDate"))
                Me.dpEndDate.SelectedDate = CDate(ds.Tables(0).Rows(0)("TPgc_EndDate"))
                Me.txtSuite.Text = ds.Tables(0).Rows(0)("TPgc_SuitValue")
                Me.txtWater.Value = CDec(ds.Tables(0).Rows(0)("TPgc_UseWaterValue"))
                Me.txtDemand.Value = CDec(ds.Tables(0).Rows(0)("TPgc_DemandValue"))
                Me.txtScore.Value = CDec(ds.Tables(0).Rows(0)("TPgc_ScoreValue"))
                Me.cbCompleted.Checked = CBool(ds.Tables(0).Rows(0)("TPgc_IsCompleted"))
                Me.txtForecastCost.Value = CDec(ds.Tables(0).Rows(0)("TPgc_ForecastCost"))
                Me.txtRealCost.Value = CDec(ds.Tables(0).Rows(0)("TPgc_RealCost"))
                Me.txtForecastProduct.Value = CDec(ds.Tables(0).Rows(0)("TPgc_ForecastProduct"))
                Me.txtRealProduct.Value = CDec(ds.Tables(0).Rows(0)("TPgc_RealProduct"))
                Me.txtForecastPrice.Value = CDec(ds.Tables(0).Rows(0)("TPgc_ForecastPrice"))
                Me.txtRealPrice.Value = CDec(ds.Tables(0).Rows(0)("TPgc_RealPrice"))
                Me.txtForecastProfit.Value = CDec(ds.Tables(0).Rows(0)("TPgc_ForecastProfit"))
                Me.txtRealProfit.Value = CDec(ds.Tables(0).Rows(0)("TPgc_RealProfit"))
            End If
        Catch ex As Exception
            Throw New Exception("Fn-LoadData:[" & ex.Message & "]")
        End Try
    End Sub

#End Region

End Class