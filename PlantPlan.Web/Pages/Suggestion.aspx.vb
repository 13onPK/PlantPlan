Imports Telerik.Web.UI
Imports System.Data.SqlClient
Imports System.IO
Imports System.Net
Imports System.Linq
Imports Newtonsoft.Json

Public Class Suggestion
    Inherits PageBase

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

    Private Property dtIrrigation As DataTable
        Get
            Return ViewState("dtIrrigation")
        End Get
        Set(ByVal value As DataTable)
            ViewState("dtIrrigation") = value
        End Set
    End Property

    Private Property MaxRiceCrop As Integer
        Get
            Return ViewState("MaxRiceCrop")
        End Get
        Set(ByVal value As Integer)
            ViewState("MaxRiceCrop") = value
        End Set
    End Property

    Private Property MaxCornCrop As Integer
        Get
            Return ViewState("MaxCornCrop")
        End Get
        Set(ByVal value As Integer)
            ViewState("MaxCornCrop") = value
        End Set
    End Property

    Private Property MaxCaneCrop As Integer
        Get
            Return ViewState("MaxCaneCrop")
        End Get
        Set(ByVal value As Integer)
            ViewState("MaxCaneCrop") = value
        End Set
    End Property

    Private Property MaxCassavaCrop As Integer
        Get
            Return ViewState("MaxCassavaCrop")
        End Get
        Set(ByVal value As Integer)
            ViewState("MaxCassavaCrop") = value
        End Set
    End Property

    Private Property UseWater As Boolean
        Get
            Return ViewState("UseWater")
        End Get
        Set(ByVal value As Boolean)
            ViewState("UseWater") = value
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
                    Me.LoadDDProvince()
                    Me.LoadDDAmphoe()
                    Me.LoadDDTambon()
                    Me.LoadDDPlantGroup()
                    Me.BindMenu()
                End If
            Else
                If Me.dtIrrigation IsNot Nothing AndAlso Me.dtIrrigation.Rows.Count > 0 Then
                    'Me.SetLink()
                End If
            End If
        Catch ex As Exception
            JavaScript.Alert(Me.Page, "Ev-Page_Load:[" & ex.Message & "]")
        End Try
    End Sub

    Private Sub ddProvince_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles ddProvince.SelectedIndexChanged
        Try
            Me.LoadDDAmphoe()
            Me.LoadDDTambon()
        Catch ex As Exception
            JavaScript.Alert(Me.Page, "Ev-ddProvince_SelectedIndexChanged:[" & ex.Message & "]")
        End Try
    End Sub

    Private Sub ddAmphoe_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles ddAmphoe.SelectedIndexChanged
        Try
            Me.LoadDDTambon()
        Catch ex As Exception
            JavaScript.Alert(Me.Page, "Ev-ddAmphoe_SelectedIndexChanged:[" & ex.Message & "]")
        End Try
    End Sub

    Private Sub ddTambon_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles ddTambon.SelectedIndexChanged
        Try
            Me.SetValue()
        Catch ex As Exception
            JavaScript.Alert(Me.Page, "Ev-ddTambon_SelectedIndexChanged:[" & ex.Message & "]")
        End Try
    End Sub

    Protected Sub cbWater_CheckedChanged(sender As Object, e As EventArgs)
        Try
            Dim cb As CheckBox = DirectCast(sender, CheckBox)

            If cb.Checked = False Then
                Me.hdWaterWeight.Value = 0
            End If

            Me.UseWater = cb.Checked
        Catch ex As Exception
            JavaScript.Alert(Me.Page, "Ev-cbWater_CheckedChanged:[" & ex.Message & "]")
        End Try
    End Sub

    Private Sub btnView_Click(sender As Object, e As EventArgs) Handles btnView.Click
        Try
            Me.txtlat.Value = Me.lat_value.Value
            Me.txtlon.Value = Me.lon_value.Value
            Me.GetAreaDetail()
            Me.GetSuitDetail()
            Me.GetPlantDetail()
            Me.GetDemandDetail()
            Me.GetPriceDetail()
            Me.GetWaterDetail()

            Me.GetCalProfit()
            Me.GetCalScore()

            Me.btnChoosed.Enabled = True
        Catch ex As Exception
            JavaScript.Alert(Me.Page, "Ev-btnView_Click:[" & ex.Message & "]")
        End Try
    End Sub

    Private Sub btnChoosed_Click(sender As Object, e As EventArgs) Handles btnChoosed.Click
        Try

            Dim dtDetail As DataTable = Me.GetDetail()
            Dim DetailJson As String = Util.ConvertDataTableToJsonString(dtDetail)

            Dim bdCon As New DBConnection()
            Dim Param As New List(Of SqlParameter)() From {New SqlParameter("@TPgc_SProv_Id", Me.ddProvince.SelectedValue),
                                                            New SqlParameter("@TPgc_SAmp_Id", Me.ddAmphoe.SelectedValue),
                                                            New SqlParameter("@TPgc_STamb_Id", Me.ddTambon.SelectedValue),
                                                            New SqlParameter("@TPgc_FarmerName", Me.txtFarmer.Text),
                                                            New SqlParameter("@TPgc_Latitude", Me.txtlat.Value),
                                                            New SqlParameter("@TPgc_Longitude", Me.txtlon.Value),
                                                            New SqlParameter("@TPgc_Area", Me.txtCrop.Value),
                                                            New SqlParameter("@TPgc_StartDate", Me.dpStart.SelectedDate),
                                                            New SqlParameter("@TPgc_DemandWeight", Me.hdDemandWeight.Value),
                                                            New SqlParameter("@TPgc_ProfitWeight", Me.hdProfitWeight.Value),
                                                            New SqlParameter("@TPgc_SuitWeight", Me.hdSuitWeight.Value),
                                                            New SqlParameter("@TPgc_WaterWeight", Me.hdWaterWeight.Value),
                                                            New SqlParameter("@Choosed", Me.ddPlantGroup.SelectedValue),
                                                            New SqlParameter("@TPgc_Remarks", ""),
                                                            New SqlParameter("@DetailJson", DetailJson),
                                                            New SqlParameter("@CrBy", MySession.UserID)
                                                            }
            Dim str As String = JsonConvert.SerializeObject(Param)
            Dim Result As DataSet = bdCon.ExecuteDataset("dbo.PlantGroupCrop_T_ManpByJson", Param)

            If Result.Tables.Count = 1 Then
                If Result.Tables(0).Rows(0)("Msg") = "Success" Then
                    Me.Page.Response.Redirect("Suggestion.aspx")
                Else
                    JavaScript.Alert(Me.Page, "Save error : [" & Result.Tables(0).Rows(0)("Msg") & "]")
                End If

            Else
                JavaScript.Alert(Me.Page, "Save failed!!")
            End If

        Catch ex As Exception
            JavaScript.Alert(Me.Page, "Ev-btnChoosed_Click:[" & ex.Message & "]")
        End Try
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

            Me.SetValue()
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

    Private Sub SetValue()
        Try
            Me.namePlace.Value = Me.ddTambon.SelectedItem.Text & " " & Me.ddAmphoe.SelectedItem.Text & " " & Me.ddProvince.SelectedItem.Text

            Dim drTamb() As DataRow = Me.dtTambon.Select("ID = " & Me.ddTambon.SelectedValue)
            Dim drAmp() As DataRow = Me.dtAmphoe.Select("ID = " & Me.ddAmphoe.SelectedValue)
            Dim drProv() As DataRow = Me.dtProvince.Select("ID = " & Me.ddProvince.SelectedValue)

            If drTamb.Count = 1 Then
                Me.criteria.Value = "WHERE tambon_idn = " & drTamb(0)("Code")
            End If

            If drAmp.Count = 1 Then
                If Me.criteria.Value.Length = 0 Then
                    Me.criteria.Value &= "AND amphoe_idn = " & drAmp(0)("Code")
                Else
                    Me.criteria.Value = "WHERE amphoe_idn = " & drAmp(0)("Code")
                End If
            End If

            If drProv.Count = 1 Then
                If Me.criteria.Value.Length = 0 Then
                    Me.criteria.Value &= "AND prov_code = " & drProv(0)("Code")
                Else
                    Me.criteria.Value = "WHERE prov_code = " & drProv(0)("Code")
                End If

                Me.url.Value = drProv(0)("API")
            End If

            Me.txtlat.Value = 0
            Me.txtlon.Value = 0
        Catch ex As Exception
            Throw New Exception("Fn-SetValue:[" & ex.Message & "]")
        End Try
    End Sub

    Private Sub GetAreaDetail()
        Try
            Dim url As String = "https://nectec-agrimap.carto.com/api/v2/sql?api_key=a9a22db331435326cc06d7583eb29c14907f9004&q=SELECT drainage, area_desc, soil_desc, limitation, slope FROM ""nectec-admin"".thailand_land_description WHERE ST_Contains(the_geom, ST_GeomFromText('POINT(" & Me.lon_value.Value & " " & Me.lat_value.Value & ")', 4326))"
            Dim Contect As String = Me.RequestAPI(url)
            Dim dt As DataTable = Util.ConvertJsonStringToDataTable(Contect)
            Dim A As PlantPlan.Web.Util = New Util


            If dt.Rows.Count > 0 Then
                Me.lbarea_desc.Text = dt.Rows(0)("area_desc").ToString
                Me.lbdrainage.Text = dt.Rows(0)("drainage").ToString
                Me.lblimitation.Text = dt.Rows(0)("limitation").ToString
                Me.lbslope.Text = dt.Rows(0)("slope").ToString
                Me.lbsoil_desc.Text = dt.Rows(0)("soil_desc").ToString
            Else
                Me.lbarea_desc.Text = "N/A"
                Me.lbdrainage.Text = "N/A"
                Me.lblimitation.Text = "N/A"
                Me.lbslope.Text = "N/A"
                Me.lbsoil_desc.Text = "N/A"
            End If

        Catch ex As Exception
            Throw New Exception("Fn-SetValue:[" & ex.Message & "]")
        End Try
    End Sub

    Private Sub GetSuitDetail()
        Try
            Dim urlRice As String = "https://nectec-agrimap.carto.com/api/v2/sql?api_key=a9a22db331435326cc06d7583eb29c14907f9004&q=SELECT suit_info FROM ""nectec-admin"".agri_landsuit_rice_th WHERE ST_Contains(the_geom, ST_GeomFromText('POINT(" & Me.lon_value.Value & " " & Me.lat_value.Value & ")', 4326))"
            Dim urlCorn As String = "https://nectec-agrimap.carto.com/api/v2/sql?api_key=a9a22db331435326cc06d7583eb29c14907f9004&q=SELECT suit_info FROM ""nectec-admin"".agri_landsuit_corn_th WHERE ST_Contains(the_geom, ST_GeomFromText('POINT(" & Me.lon_value.Value & " " & Me.lat_value.Value & ")', 4326))"
            Dim urlCane As String = "https://nectec-agrimap.carto.com/api/v2/sql?api_key=a9a22db331435326cc06d7583eb29c14907f9004&q=SELECT suit_info FROM ""nectec-admin"".agri_landsuit_cane_th WHERE ST_Contains(the_geom, ST_GeomFromText('POINT(" & Me.lon_value.Value & " " & Me.lat_value.Value & ")', 4326))"
            Dim urlCassava As String = "https://nectec-agrimap.carto.com/api/v2/sql?api_key=a9a22db331435326cc06d7583eb29c14907f9004&q=SELECT suit_info FROM ""nectec-admin"".agri_landsuit_cassava_th WHERE ST_Contains(the_geom, ST_GeomFromText('POINT(" & Me.lon_value.Value & " " & Me.lat_value.Value & ")', 4326))"

            Dim ContectRice As String = Me.RequestAPI(urlRice)
            Dim ContectCorn As String = Me.RequestAPI(urlCorn)
            Dim ContectCane As String = Me.RequestAPI(urlCane)
            Dim ContectCassava As String = Me.RequestAPI(urlCassava)

            Dim dtRice As DataTable = Util.ConvertJsonStringToDataTable(ContectRice)
            Dim dtCorn As DataTable = Util.ConvertJsonStringToDataTable(ContectCorn)
            Dim dtCane As DataTable = Util.ConvertJsonStringToDataTable(ContectCane)
            Dim dtCassava As DataTable = Util.ConvertJsonStringToDataTable(ContectCassava)

            If dtRice.Rows.Count > 0 Then
                Me.lbRiceSuit.Text = dtRice.Rows(0)("suit_info").ToString
                Me.lbRiceSuit.ForeColor = System.Drawing.Color.Black
            Else
                Me.lbRiceSuit.Text = "พท.ไม่เหมาะสม"
                Me.lbRiceSuit.ForeColor = System.Drawing.Color.Red
            End If

            If dtCorn.Rows.Count > 0 Then
                Me.lbCornSuit.Text = dtCorn.Rows(0)("suit_info").ToString
                Me.lbCornSuit.ForeColor = System.Drawing.Color.Black
            Else
                Me.lbCornSuit.Text = "พท.ไม่เหมาะสม"
                Me.lbCornSuit.ForeColor = System.Drawing.Color.Red
            End If

            If dtCane.Rows.Count > 0 Then
                Me.lbCaneSuit.Text = dtCane.Rows(0)("suit_info").ToString
                Me.lbCaneSuit.ForeColor = System.Drawing.Color.Black
            Else
                Me.lbCaneSuit.Text = "พท.ไม่เหมาะสม"
                Me.lbCaneSuit.ForeColor = System.Drawing.Color.Red
            End If

            If dtCassava.Rows.Count > 0 Then
                Me.lbCassavaSuit.Text = dtCassava.Rows(0)("suit_info").ToString
                Me.lbCassavaSuit.ForeColor = System.Drawing.Color.Black
            Else
                Me.lbCassavaSuit.Text = "พท.ไม่เหมาะสม"
                Me.lbCassavaSuit.ForeColor = System.Drawing.Color.Red
            End If
        Catch ex As Exception
            Throw New Exception("Fn-SetValue:[" & ex.Message & "]")
        End Try
    End Sub

    Private Sub GetPlantDetail()
        Try
            Dim bdCon As New DBConnection()

            'Rice
            Dim ParamRice As New List(Of SqlParameter)() From {New SqlParameter("@SPntgp_Id", Plant_Enum._Rice), New SqlParameter("@SProv_Id", Me.ddProvince.SelectedValue)}
            Dim dsRice As DataSet = bdCon.ExecuteDataset("dbo.PlantGroup_S_SelById", ParamRice)

            If dsRice IsNot Nothing AndAlso dsRice.Tables.Count = 1 Then
                Me.lbRiceWater.Text = CDec(dsRice.Tables(0).Rows(0)("TPwu_AllUse")).ToString("##,###.##")

                If IsNumeric(Me.hdRiceCrop.Value) AndAlso Me.hdRiceCrop.Value > 0 Then
                    Me.lbRiceCrop.Text = Me.hdRiceCrop.Value
                    Me.MaxRiceCrop = Me.hdRiceCrop.Value
                Else
                    Me.lbRiceCrop.Text = dsRice.Tables(0).Rows(0)("SPntgp_CropPeriodMin").ToString & "-" & dsRice.Tables(0).Rows(0)("SPntgp_CropPeriodMax").ToString
                    Me.MaxRiceCrop = dsRice.Tables(0).Rows(0)("SPntgp_CropPeriodMax")
                End If

                If IsNumeric(Me.hdRiceYield.Value) AndAlso Me.hdRiceYield.Value > 0 Then
                    Me.lbRiceYield.Text = CDec(Me.hdRiceYield.Value).ToString("##,###.##")
                Else
                    Me.lbRiceYield.Text = CDec(dsRice.Tables(0).Rows(0)("SPntgp_YieldKGAvg")).ToString("##,###.##")
                End If

                If IsNumeric(Me.hdRiceCost.Value) AndAlso Me.hdRiceCost.Value > 0 Then
                    Me.lbRiceCost.Text = CDec(Me.hdRiceCost.Value).ToString("##,###.##")
                Else
                    Me.lbRiceCost.Text = CDec(dsRice.Tables(0).Rows(0)("SPntgp_Cost")).ToString("##,###.##")
                End If
            Else
                Me.lbRiceWater.Text = "N/A"
                Me.lbRiceCrop.Text = "N/A"
                Me.lbRiceYield.Text = "N/A"
            End If

            'Corn
            Dim ParamCorn As New List(Of SqlParameter)() From {New SqlParameter("@SPntgp_Id", Plant_Enum._Corn), New SqlParameter("@SProv_Id", Me.ddProvince.SelectedValue)}
            Dim dsCorn As DataSet = bdCon.ExecuteDataset("dbo.PlantGroup_S_SelById", ParamCorn)

            If dsCorn IsNot Nothing AndAlso dsCorn.Tables.Count = 1 Then
                Me.lbCornWater.Text = CDec(dsCorn.Tables(0).Rows(0)("TPwu_AllUse")).ToString("##,###.##")

                If IsNumeric(Me.hdCornCrop.Value) AndAlso Me.hdCornCrop.Value > 0 Then
                    Me.lbCornCrop.Text = Me.hdCornCrop.Value
                    Me.MaxCornCrop = Me.hdCornCrop.Value
                Else
                    Me.lbCornCrop.Text = dsCorn.Tables(0).Rows(0)("SPntgp_CropPeriodMin").ToString & "-" & dsCorn.Tables(0).Rows(0)("SPntgp_CropPeriodMax").ToString
                    Me.MaxCornCrop = dsCorn.Tables(0).Rows(0)("SPntgp_CropPeriodMax")
                End If

                If IsNumeric(Me.hdCornYield.Value) AndAlso Me.hdCornYield.Value > 0 Then
                    Me.lbCornYield.Text = CDec(Me.hdCornYield.Value).ToString("##,###.##")
                Else
                    Me.lbCornYield.Text = CDec(dsCorn.Tables(0).Rows(0)("SPntgp_YieldKGAvg")).ToString("##,###.##")
                End If

                If IsNumeric(Me.hdCornCost.Value) AndAlso Me.hdCornCost.Value > 0 Then
                    Me.lbCornCost.Text = CDec(Me.hdCornCost.Value).ToString("##,###.##")
                Else
                    Me.lbCornCost.Text = CDec(dsCorn.Tables(0).Rows(0)("SPntgp_Cost")).ToString("##,###.##")
                End If
            Else
                Me.lbCornWater.Text = "N/A"
                Me.lbCornCrop.Text = "N/A"
                Me.lbCornYield.Text = "N/A"
                Me.lbCornCost.Text = "N/A"
            End If

            'Cane
            Dim ParamCane As New List(Of SqlParameter)() From {New SqlParameter("@SPntgp_Id", Plant_Enum._Cane), New SqlParameter("@SProv_Id", Me.ddProvince.SelectedValue)}
            Dim dsCane As DataSet = bdCon.ExecuteDataset("dbo.PlantGroup_S_SelById", ParamCane)

            If dsCane IsNot Nothing AndAlso dsCane.Tables.Count = 1 Then
                Me.lbCaneWater.Text = CDec(dsCane.Tables(0).Rows(0)("TPwu_AllUse")).ToString("##,###.##")

                If IsNumeric(Me.hdCaneCrop.Value) AndAlso Me.hdCaneCrop.Value > 0 Then
                    Me.lbCaneCrop.Text = Me.hdCaneCrop.Value
                    Me.MaxCaneCrop = Me.hdCaneCrop.Value
                Else
                    Me.lbCaneCrop.Text = dsCane.Tables(0).Rows(0)("SPntgp_CropPeriodMin").ToString & "-" & dsCane.Tables(0).Rows(0)("SPntgp_CropPeriodMax").ToString
                    Me.MaxCaneCrop = dsCane.Tables(0).Rows(0)("SPntgp_CropPeriodMax")
                End If

                If IsNumeric(Me.hdCaneYield.Value) AndAlso Me.hdCaneYield.Value > 0 Then
                    Me.lbCaneYield.Text = CDec(Me.hdCaneYield.Value).ToString("##,###.##")
                Else
                    Me.lbCaneYield.Text = CDec(dsCane.Tables(0).Rows(0)("SPntgp_YieldKGAvg")).ToString("##,###.##")
                End If

                If IsNumeric(Me.hdCaneCost.Value) AndAlso Me.hdCaneCost.Value > 0 Then
                    Me.lbCaneCost.Text = CDec(Me.hdCaneCost.Value).ToString("##,###.##")
                Else
                    Me.lbCaneCost.Text = CDec(dsCane.Tables(0).Rows(0)("SPntgp_Cost")).ToString("##,###.##")
                End If
            Else
                Me.lbCaneWater.Text = "N/A"
                Me.lbCaneCrop.Text = "N/A"
                Me.lbCaneYield.Text = "N/A"
                Me.lbCaneCost.Text = "N/A"
            End If

            'Cassava
            Dim ParamCassava As New List(Of SqlParameter)() From {New SqlParameter("@SPntgp_Id", Plant_Enum._Cassava), New SqlParameter("@SProv_Id", Me.ddProvince.SelectedValue)}
            Dim dsCassava As DataSet = bdCon.ExecuteDataset("dbo.PlantGroup_S_SelById", ParamCassava)

            If dsCassava IsNot Nothing AndAlso dsCassava.Tables.Count = 1 Then
                Me.lbCassavaWater.Text = CDec(dsCassava.Tables(0).Rows(0)("TPwu_AllUse")).ToString("##,###.##")

                If IsNumeric(Me.hdCassavaCrop.Value) AndAlso Me.hdCassavaCrop.Value > 0 Then
                    Me.lbCassavaCrop.Text = Me.hdCassavaCrop.Value
                    Me.MaxCassavaCrop = Me.hdCassavaCrop.Value
                Else
                    Me.lbCassavaCrop.Text = dsCassava.Tables(0).Rows(0)("SPntgp_CropPeriodMin").ToString & "-" & dsCassava.Tables(0).Rows(0)("SPntgp_CropPeriodMax").ToString
                    Me.MaxCassavaCrop = dsCassava.Tables(0).Rows(0)("SPntgp_CropPeriodMax")
                End If

                If IsNumeric(Me.hdCassavaYield.Value) AndAlso Me.hdCassavaYield.Value > 0 Then
                    Me.lbCassavaYield.Text = CDec(Me.hdCassavaYield.Value).ToString("##,###.##")
                Else
                    Me.lbCassavaYield.Text = CDec(dsCassava.Tables(0).Rows(0)("SPntgp_YieldKGAvg")).ToString("##,###.##")
                End If

                If IsNumeric(Me.hdCassavaCost.Value) AndAlso Me.hdCassavaCost.Value > 0 Then
                    Me.lbCassavaCost.Text = CDec(Me.hdCassavaCost.Value).ToString("##,###.##")
                Else
                    Me.lbCassavaCost.Text = CDec(dsCassava.Tables(0).Rows(0)("SPntgp_Cost")).ToString("##,###.##")
                End If
            Else
                Me.lbCassavaWater.Text = "N/A"
                Me.lbCassavaCrop.Text = "N/A"
                Me.lbCassavaYield.Text = "N/A"
                Me.lbCassavaCost.Text = "N/A"
            End If

        Catch ex As Exception
            Throw New Exception("Fn-GetPlantDetail:[" & ex.Message & "]")
        End Try
    End Sub

    Private Sub GetDemandDetail()
        Try
            Dim bdCon As New DBConnection()

            'Rice
            Dim ParamRice As New List(Of SqlParameter)() From {New SqlParameter("@SPntgp_Id", Plant_Enum._Rice),
                                                                New SqlParameter("@DateCorp", DateAndTime.DateAdd(DateInterval.Day, Me.MaxRiceCrop, CDate(Me.dpStart.SelectedDate)))}

            Dim p_RiceDemand = New SqlParameter("@AllDemand", SqlDbType.Float)
            p_RiceDemand.Direction = ParameterDirection.Output
            ParamRice.Add(p_RiceDemand)

            Dim p_RiceRemain = New SqlParameter("@RemainDemand", SqlDbType.Float)
            p_RiceRemain.Direction = ParameterDirection.Output
            ParamRice.Add(p_RiceRemain)

            bdCon.ExecuteDatasetWithTrans("dbo.PlantGroupDemand_T_SelById", ParamRice)
            Me.lbRiceDemand.Text = CDec(p_RiceDemand.Value).ToString("##,###.##")
            Me.lbRiceRemain.Text = CDec(p_RiceRemain.Value).ToString("##,###.##")

            'Corn
            Dim ParamCorn As New List(Of SqlParameter)() From {New SqlParameter("@SPntgp_Id", Plant_Enum._Corn),
                                                                New SqlParameter("@DateCorp", DateAndTime.DateAdd(DateInterval.Day, Me.MaxRiceCrop, CDate(Me.dpStart.SelectedDate)))}

            Dim p_CornDemand = New SqlParameter("@AllDemand", SqlDbType.Float)
            p_CornDemand.Direction = ParameterDirection.Output
            ParamCorn.Add(p_CornDemand)

            Dim p_CornRemain = New SqlParameter("@RemainDemand", SqlDbType.Float)
            p_CornRemain.Direction = ParameterDirection.Output
            ParamCorn.Add(p_CornRemain)

            bdCon.ExecuteDatasetWithTrans("dbo.PlantGroupDemand_T_SelById", ParamCorn)
            Me.lbCornDemand.Text = CDec(p_CornDemand.Value).ToString("##,###.##")
            Me.lbCornRemain.Text = CDec(p_CornRemain.Value).ToString("##,###.##")

            'Cane
            Dim ParamCane As New List(Of SqlParameter)() From {New SqlParameter("@SPntgp_Id", Plant_Enum._Cane),
                                                                New SqlParameter("@DateCorp", DateAndTime.DateAdd(DateInterval.Day, Me.MaxRiceCrop, CDate(Me.dpStart.SelectedDate)))}

            Dim p_CaneDemand = New SqlParameter("@AllDemand", SqlDbType.Float)
            p_CaneDemand.Direction = ParameterDirection.Output
            ParamCane.Add(p_CaneDemand)

            Dim p_CaneRemain = New SqlParameter("@RemainDemand", SqlDbType.Float)
            p_CaneRemain.Direction = ParameterDirection.Output
            ParamCane.Add(p_CaneRemain)

            bdCon.ExecuteDatasetWithTrans("dbo.PlantGroupDemand_T_SelById", ParamCane)
            Me.lbCaneDemand.Text = CDec(p_CaneDemand.Value).ToString("##,###.##")
            Me.lbCaneRemain.Text = CDec(p_CaneRemain.Value).ToString("##,###.##")

            'Cassava
            Dim ParamCassava As New List(Of SqlParameter)() From {New SqlParameter("@SPntgp_Id", Plant_Enum._Cassava),
                                                                New SqlParameter("@DateCorp", DateAndTime.DateAdd(DateInterval.Day, Me.MaxRiceCrop, CDate(Me.dpStart.SelectedDate)))}

            Dim p_CassavaDemand = New SqlParameter("@AllDemand", SqlDbType.Float)
            p_CassavaDemand.Direction = ParameterDirection.Output
            ParamCassava.Add(p_CassavaDemand)

            Dim p_CassavaRemain = New SqlParameter("@RemainDemand", SqlDbType.Float)
            p_CassavaRemain.Direction = ParameterDirection.Output
            ParamCassava.Add(p_CassavaRemain)

            bdCon.ExecuteDatasetWithTrans("dbo.PlantGroupDemand_T_SelById", ParamCassava)
            Me.lbCassavaDemand.Text = CDec(p_CassavaDemand.Value).ToString("##,###.##")
            Me.lbCassavaRemain.Text = CDec(p_CassavaRemain.Value).ToString("##,###.##")

        Catch ex As Exception
            Throw New Exception("Fn-GetPriceDetail:[" & ex.Message & "]")
        End Try
    End Sub

    Private Sub GetPriceDetail()
        Try
            Dim bdCon As New DBConnection()

            'Rice
            Dim ParamRice As New List(Of SqlParameter)() From {New SqlParameter("@SPntgp_Id", Plant_Enum._Rice),
                                                                New SqlParameter("@StartCorp", Me.dpStart.SelectedDate),
                                                                New SqlParameter("@CropDay", Me.MaxRiceCrop)
                                                                }

            Dim p_RicePrice = New SqlParameter("@Price", SqlDbType.Float)
            p_RicePrice.Direction = ParameterDirection.Output
            ParamRice.Add(p_RicePrice)
            bdCon.ExecuteDatasetWithTrans("dbo.PlantGroupPrice_T_SelById", ParamRice)
            Me.lbRicePrice.Text = CDec(p_RicePrice.Value).ToString("##,###.##")

            'Corn
            Dim ParamCorn As New List(Of SqlParameter)() From {New SqlParameter("@SPntgp_Id", Plant_Enum._Corn),
                                                                New SqlParameter("@StartCorp", Me.dpStart.SelectedDate),
                                                                New SqlParameter("@CropDay", Me.MaxCornCrop)
                                                                }

            Dim p_CornPrice = New SqlParameter("@Price", SqlDbType.Float)
            p_CornPrice.Direction = ParameterDirection.Output
            ParamCorn.Add(p_CornPrice)
            bdCon.ExecuteDatasetWithTrans("dbo.PlantGroupPrice_T_SelById", ParamCorn)
            Me.lbCornPrice.Text = CDec(p_CornPrice.Value).ToString("##,###.##")

            'Cane
            Dim ParamCane As New List(Of SqlParameter)() From {New SqlParameter("@SPntgp_Id", Plant_Enum._Cane),
                                                                New SqlParameter("@StartCorp", Me.dpStart.SelectedDate),
                                                                New SqlParameter("@CropDay", Me.MaxCaneCrop)
                                                                }

            Dim p_CanePrice = New SqlParameter("@Price", SqlDbType.Float)
            p_CanePrice.Direction = ParameterDirection.Output
            ParamCane.Add(p_CanePrice)
            bdCon.ExecuteDatasetWithTrans("dbo.PlantGroupPrice_T_SelById", ParamCane)
            Me.lbCanePrice.Text = CDec(p_CanePrice.Value).ToString("##,###.##")

            'Cassava
            Dim ParamCassava As New List(Of SqlParameter)() From {New SqlParameter("@SPntgp_Id", Plant_Enum._Cassava),
                                                                New SqlParameter("@StartCorp", Me.dpStart.SelectedDate),
                                                                New SqlParameter("@CropDay", Me.MaxCassavaCrop)
                                                                }

            Dim p_CassavaPrice = New SqlParameter("@Price", SqlDbType.Float)
            p_CassavaPrice.Direction = ParameterDirection.Output
            ParamCassava.Add(p_CassavaPrice)
            bdCon.ExecuteDatasetWithTrans("dbo.PlantGroupPrice_T_SelById", ParamCassava)
            Me.lbCassavaPrice.Text = CDec(p_CassavaPrice.Value).ToString("##,###.##")

        Catch ex As Exception
            Throw New Exception("Fn-GetPriceDetail:[" & ex.Message & "]")
        End Try
    End Sub

    Private Sub GetWaterDetail()
        Try
            Dim bdCon As New DBConnection()

            Dim p_date As Date = IIf(Me.dpStart.SelectedDate > Now, Now, Me.dpStart.SelectedDate)
            Dim Param As New List(Of SqlParameter)() From {New SqlParameter("@Lat", Me.txtlat.Value), New SqlParameter("@Lng", Me.txtlon.Value), New SqlParameter("@Date", Me.dpStart.SelectedDate)}
            Dim ds As DataSet = bdCon.ExecuteDataset("dbo.IrrigationWater_T_SelInfo", Param)

            If ds IsNot Nothing AndAlso ds.Tables.Count = 1 Then
                Me.dtIrrigation = ds.Tables(0)
                Me.SetLink()
            End If

        Catch ex As Exception
            Throw New Exception("Fn-GetWaterDetail:[" & ex.Message & "]")
        End Try
    End Sub

    Private Sub GetCalProfit()
        Try
            Me.lbRiceProfit.Text = CDec((CDec((Me.txtCrop.Value * CDec(Me.lbRiceYield.Text) * CDec(Me.lbRicePrice.Text)) / 1000) - Me.txtCrop.Value * CDec(Me.lbRiceCost.Text)) / (MaxRiceCrop / 30)).ToString("##,###.##")
            Me.lbCornProfit.Text = CDec((CDec((Me.txtCrop.Value * CDec(Me.lbCornYield.Text) * CDec(Me.lbCornPrice.Text)) / 1000) - Me.txtCrop.Value * CDec(Me.lbCornCost.Text)) / (MaxCornCrop / 30)).ToString("##,###.##")
            Me.lbCaneProfit.Text = CDec((CDec((Me.txtCrop.Value * CDec(Me.lbCaneYield.Text) * CDec(Me.lbCanePrice.Text)) / 1000) - Me.txtCrop.Value * CDec(Me.lbCaneCost.Text)) / (MaxCaneCrop / 30)).ToString("##,###.##")
            Me.lbCassavaProfit.Text = CDec((CDec((Me.txtCrop.Value * CDec(Me.lbCassavaYield.Text) * CDec(Me.lbCassavaPrice.Text)) / 1000) - Me.txtCrop.Value * CDec(Me.lbCassavaCost.Text)) / (MaxCassavaCrop / 30)).ToString("##,###.##")
        Catch ex As Exception
            Throw New Exception("Fn-GetCalProfit:[" & ex.Message & "]")
        End Try
    End Sub

    Private Sub GetCalScore()
        Try
            Dim c_Factors As Integer = 4
            Dim c_Crops As Integer = 4

            If Me.UseWater = 0 Then
                c_Factors = 3
            End If

            Dim matrixFactor(c_Factors - 1, c_Factors - 1) As Decimal
            Me.AddFactorValue(matrixFactor)

            Dim matrixDemand(c_Crops - 1, c_Crops - 1) As Decimal
            Me.AddDemandValue(matrixDemand)

            Dim matrixProfit(c_Crops - 1, c_Crops - 1) As Decimal
            Me.AddProfitValue(matrixProfit)

            Dim matrixSuit(c_Crops - 1, c_Crops - 1) As Decimal
            Me.AddSuitValue(matrixSuit)

            Dim matrixWater(c_Crops - 1, c_Crops - 1) As Decimal
            Me.AddWaterValue(matrixWater)

            Dim Factor As Decimal() = Me.CalculateWeight(matrixFactor)

            Dim Demand As Decimal() = Me.CalculateWeight(matrixDemand)
            Dim Profit As Decimal() = Me.CalculateWeight(matrixProfit)
            Dim Suit As Decimal() = Me.CalculateWeight(matrixSuit)
            Dim Water As Decimal() = Me.CalculateWeight(matrixWater)


            Dim MatrixA(c_Crops, c_Factors) As Decimal
            Dim MatrixB(c_Factors, 1) As Decimal

            For i As Integer = 0 To c_Crops - 1
                For j As Integer = 0 To c_Factors - 1
                    If j = Factor_Enum._Demand - 1 Then
                        MatrixA(i, j) = Demand(i)

                    ElseIf j = Factor_Enum._Profit - 1 Then
                        MatrixA(i, j) = Profit(i)

                    ElseIf j = Factor_Enum._Suit - 1 Then
                        MatrixA(i, j) = Suit(i)

                    ElseIf j = Factor_Enum._Water - 1 Then
                        MatrixA(i, j) = Water(i)

                    End If
                Next
            Next

            For i As Integer = 0 To c_Factors - 1
                MatrixB(i, 1) = Factor(i)
            Next

            Dim Score(,) As Decimal = Me.Multiply(MatrixA, MatrixB)
            For i As Integer = 0 To c_Crops - 1
                If i = Plant_Enum._Rice - 1 Then
                    Me.lbRiceScore.Text = CDec(Score(i, 1) * 100).ToString("##,###.##")
                ElseIf i = Plant_Enum._Corn - 1 Then
                    Me.lbCornScore.Text = CDec(Score(i, 1) * 100).ToString("##,###.##")

                ElseIf i = Plant_Enum._Cane - 1 Then
                    Me.lbCaneScore.Text = CDec(Score(i, 1) * 100).ToString("##,###.##")

                ElseIf i = Plant_Enum._Cassava - 1 Then
                    Me.lbCassavaScore.Text = CDec(Score(i, 1) * 100).ToString("##,###.##")

                End If
            Next
        Catch ex As Exception
            Throw New Exception("Fn-GetCalProfit:[" & ex.Message & "]")
        End Try
    End Sub

    Private Function RequestAPI(ByVal url As String) As String
        Dim response As String = ""
        Try
            Dim inStream As StreamReader
            Dim webRequest As WebRequest
            Dim webresponse As WebResponse

            Dim Contect As String = ""
            webRequest = WebRequest.Create(url)
            webresponse = webRequest.GetResponse()
            inStream = New StreamReader(webresponse.GetResponseStream())
            Contect = inStream.ReadToEnd()

            response = Contect.Substring(Contect.IndexOf("["), Contect.IndexOf("]") + 1 - Contect.IndexOf("["))
        Catch ex As Exception
            response = ex.Message
        End Try
        Return response
    End Function

    Private Sub AddFactorValue(ByRef mtx(,) As Decimal)
        Try
            Dim FactorVal = New Decimal() {Me.hdDemandWeight.Value, Me.hdProfitWeight.Value, Me.hdSuitWeight.Value, Me.hdWaterWeight.Value}

            For i As Integer = 0 To mtx.GetLength(0) - 1
                For j = 0 To mtx.GetLength(1) - 1
                    mtx(i, j) = FactorVal(i) / FactorVal(j)
                Next
            Next
        Catch ex As Exception
            Throw New Exception("Fn-AddFactorValue:[" & ex.Message & "]")
        End Try
    End Sub

    Private Sub AddDemandValue(ByRef mtx(,) As Decimal)
        Try
            Dim DemandVal = New Decimal() {CDec(Me.lbRiceRemain.Text) / CDec(Me.lbRiceDemand.Text), CDec(Me.lbCornRemain.Text) / CDec(Me.lbCornDemand.Text), CDec(Me.lbCaneRemain.Text) / CDec(Me.lbCaneDemand.Text), CDec(Me.lbCassavaRemain.Text) / CDec(Me.lbCassavaDemand.Text)}

            For i As Integer = 0 To mtx.GetLength(0) - 1
                For j = 0 To mtx.GetLength(1) - 1
                    mtx(i, j) = DemandVal(i) / DemandVal(j)
                Next
            Next
        Catch ex As Exception
            Throw New Exception("Fn-AddDemandValue:[" & ex.Message & "]")
        End Try
    End Sub

    Private Sub AddProfitValue(ByRef mtx(,) As Decimal)
        Try
            Dim ProfitVal = New Decimal() {CDec(Me.lbRiceProfit.Text), CDec(Me.lbCornProfit.Text), CDec(Me.lbCaneProfit.Text), CDec(Me.lbCassavaProfit.Text)}

            Dim min As Decimal = ProfitVal.Min()
            If min < 1 Then
                min = (min * -1) + 1
            Else
                min = 0
            End If

            For i As Integer = 0 To mtx.GetLength(0) - 1
                For j = 0 To mtx.GetLength(1) - 1
                    mtx(i, j) = (ProfitVal(i) + min) / (ProfitVal(j) + min)
                Next
            Next
        Catch ex As Exception
            Throw New Exception("Fn-AddProfitValue:[" & ex.Message & "]")
        End Try
    End Sub

    Private Sub AddSuitValue(ByRef mtx(,) As Decimal)
        Try
            Dim SuitVal = New Decimal() {Me.ConvertSuit(Me.lbRiceSuit.Text), Me.ConvertSuit(Me.lbCornSuit.Text), Me.ConvertSuit(Me.lbCaneSuit.Text), Me.ConvertSuit(Me.lbCassavaSuit.Text)}

            For i As Integer = 0 To mtx.GetLength(0) - 1
                For j = 0 To mtx.GetLength(1) - 1
                    mtx(i, j) = SuitVal(i) / SuitVal(j)
                Next
            Next
        Catch ex As Exception
            Throw New Exception("Fn-AddSuitValue:[" & ex.Message & "]")
        End Try
    End Sub

    Private Sub AddWaterValue(ByRef mtx(,) As Decimal)
        Try
            Dim rice As Decimal = 0
            Dim corn As Decimal = 0
            Dim cane As Decimal = 0
            Dim cassava As Decimal = 0

            If CDec(Me.lbRiceWater.Text) < CDec(Me.lbCornWater.Text) Then
                rice += 2
                corn += 1
            Else
                rice += 1
                corn += 2
            End If

            If CDec(Me.lbRiceWater.Text) < CDec(Me.lbCaneWater.Text) Then
                rice += 2
                cane += 1
            Else
                rice += 1
                cane += 2
            End If

            If CDec(Me.lbRiceWater.Text) < CDec(Me.lbCassavaWater.Text) Then
                rice += 2
                cassava += 1
            Else
                rice += 1
                cassava += 2
            End If

            If CDec(Me.lbCornWater.Text) < CDec(Me.lbCaneWater.Text) Then
                corn += 2
                cane += 1
            Else
                corn += 1
                cane += 2
            End If

            If CDec(Me.lbCornWater.Text) < CDec(Me.lbCassavaWater.Text) Then
                corn += 2
                cassava += 1
            Else
                corn += 1
                cassava += 2
            End If

            If CDec(Me.lbCaneWater.Text) < CDec(Me.lbCassavaWater.Text) Then
                cane += 2
                cassava += 1
            Else
                cane += 1
                cassava += 2
            End If

            Dim WaterVal = New Decimal() {rice, corn, cane, cassava}
            Dim div As Decimal = 1

            If Me.dtIrrigation.Rows.Count > 0 Then
                Dim PercentQty As Decimal = 0
                Dim PercentQty3 As Decimal = 0
                Dim PercentQty5 As Decimal = 0
                Dim PercentQty7 As Decimal = 0
                For Each dr As DataRow In Me.dtIrrigation.Rows
                    PercentQty += dr("TIrrgw_QtyPercent")
                    PercentQty3 += dr("QtyPercent3AVG")
                    PercentQty5 += dr("QtyPercent5AVG")
                    PercentQty7 += dr("QtyPercent7AVG")
                Next
                PercentQty = PercentQty / Me.dtIrrigation.Rows.Count
                PercentQty3 = PercentQty3 / Me.dtIrrigation.Rows.Count
                PercentQty5 = PercentQty5 / Me.dtIrrigation.Rows.Count
                PercentQty7 = PercentQty7 / Me.dtIrrigation.Rows.Count

                If PercentQty < PercentQty3 And PercentQty < PercentQty5 Then
                    div = 100
                ElseIf PercentQty > PercentQty3 Or PercentQty > PercentQty5 Then
                    div = 10
                End If
            End If

            For i As Integer = 0 To mtx.GetLength(0) - 1
                For j = 0 To mtx.GetLength(1) - 1
                    mtx(i, j) = (WaterVal(i) / WaterVal(j)) / div
                Next
            Next
        Catch ex As Exception
            Throw New Exception("Fn-AddSuitValue:[" & ex.Message & "]")
        End Try
    End Sub

    Private Function ConvertSuit(ByVal txt As String) As Decimal
        Dim Result As Decimal = 0.1

        If txt = "พท.ไม่เหมาะสม" Then
            Result = 1
        ElseIf txt = "พท.เหมาะสมปานกลาง" Then
            Result = 5
        ElseIf txt = "พท.เหมาะสมสูง" Then
            Result = 10
        End If

        Return Result
    End Function

    Private Function CalculateWeight(ByVal mtx(,) As Decimal) As Decimal()
        Dim Threshold As Single
        Threshold = 0.005

        Dim OrgMTX = mtx.Clone
        Dim PrevMTX = mtx.Clone
        Dim CurrMTX = mtx.Clone

        Dim Result(OrgMTX.GetLength(0)) As Decimal

        Try
            'Current data
            CurrMTX = Me.CalCurrent(PrevMTX)

            While CheckThershold(Threshold, PrevMTX, CurrMTX, Result) = True
                PrevMTX = CurrMTX
                Me.CalCurrent(PrevMTX)
            End While
        Catch ex As Exception
            Throw New Exception("Fn-CalculateWeight:[" & ex.Message & "]")
        End Try

        Return Result
    End Function

    Private Function CalCurrent(ByVal mtx(,) As Decimal) As Decimal(,)
        Return Me.Multiply(mtx, mtx)
    End Function

    Private Function CheckThershold(ByVal t As Decimal, ByVal Prev(,) As Decimal, ByVal Curr(,) As Decimal, ByRef rVaule() As Decimal) As Boolean
        Dim Result As Boolean
        Result = False

        Try
            Dim SumPrev(Prev.GetLength(0)) As Decimal
            Dim TotalPrev As Decimal = 0
            For i As Integer = 0 To Prev.GetLength(0) - 1
                For j = 0 To Prev.GetLength(1) - 1
                    SumPrev(i) += Prev(i, j)
                    TotalPrev += Prev(i, j)
                Next
            Next

            Dim SumCurr(Curr.GetLength(0)) As Decimal
            Dim TotalCurr As Decimal = 0
            For i As Integer = 0 To Curr.GetLength(0) - 1
                For j = 0 To Curr.GetLength(1) - 1
                    SumCurr(i) += Curr(i, j)
                    TotalCurr += Curr(i, j)
                Next
            Next

            For i As Integer = 0 To SumPrev.Length - 1
                SumPrev(i) = SumPrev(i) / TotalPrev
                SumCurr(i) = SumCurr(i) / TotalCurr

                If Math.Abs(SumPrev(i) - SumCurr(i)) > t Then
                    Result = True
                End If
            Next

            rVaule = SumCurr

        Catch ex As Exception
            Throw New Exception("Fn-CheckThershold:[" & ex.Message & "]")
        End Try

        Return Result
    End Function

    Private Function Multiply(ByVal matrixA(,) As Decimal, ByVal matrixB(,) As Decimal) As Decimal(,)

        If matrixA.GetLength(1) <> matrixB.GetLength(0) Then
            JavaScript.Alert(Me.Page, "Illegal matrix dimensions!")
            Return Nothing
        End If

        Dim result(matrixA.GetLength(0) - 1, matrixB.GetLength(1) - 1) As Decimal

        Try
            For i = 0 To result.GetLength(0) - 1
                For j = 0 To result.GetLength(1) - 1
                    For k = 0 To matrixA.GetLength(1) - 1

                        result(i, j) += matrixA(i, k) * matrixB(k, j)

                    Next
                Next
            Next
        Catch ex As Exception
            Throw New Exception("Fn-Multiply:[" & ex.Message & "]")
        End Try

        Return result

    End Function

    Private Function GetDetail() As DataTable
        Dim dt As DataTable = New DataTable
        dt.Columns.Add("TPgc_SPntgp_Id", System.Type.GetType("System.Int32"))
        dt.Columns.Add("TPgc_EndDate", System.Type.GetType("System.Int32"))
        dt.Columns.Add("TPgc_ForecastCost", System.Type.GetType("System.Decimal"))
        dt.Columns.Add("TPgc_ForecastProduct", System.Type.GetType("System.Decimal"))
        dt.Columns.Add("TPgc_ForecastPrice", System.Type.GetType("System.Decimal"))
        dt.Columns.Add("TPgc_ForecastProfit", System.Type.GetType("System.Decimal"))
        dt.Columns.Add("TPgc_SuitValue")
        dt.Columns.Add("TPgc_UseWaterValue", System.Type.GetType("System.Decimal"))
        dt.Columns.Add("TPgc_DemandValue", System.Type.GetType("System.Decimal"))
        dt.Columns.Add("TPgc_ScoreValue", System.Type.GetType("System.Decimal"))

        Try
            Dim drRice As DataRow = dt.NewRow
            drRice("TPgc_SPntgp_Id") = Plant_Enum._Rice
            drRice("TPgc_EndDate") = Me.MaxRiceCrop
            drRice("TPgc_ForecastCost") = CDec(Me.lbRiceCost.Text)
            drRice("TPgc_ForecastProduct") = CDec(Me.lbRiceYield.Text)
            drRice("TPgc_ForecastPrice") = CDec(Me.lbRicePrice.Text)
            drRice("TPgc_ForecastProfit") = CDec(Me.lbRiceProfit.Text)
            drRice("TPgc_SuitValue") = Me.lbRiceSuit.Text
            drRice("TPgc_UseWaterValue") = CDec(Me.lbRiceWater.Text)
            drRice("TPgc_DemandValue") = CDec(Me.lbRiceDemand.Text)
            drRice("TPgc_ScoreValue") = CDec(Me.lbRiceScore.Text)
            dt.Rows.Add(drRice)

            Dim drCorn As DataRow = dt.NewRow
            drCorn("TPgc_SPntgp_Id") = Plant_Enum._Corn
            drCorn("TPgc_EndDate") = Me.MaxCornCrop
            drCorn("TPgc_ForecastCost") = CDec(Me.lbCornCost.Text)
            drCorn("TPgc_ForecastProduct") = CDec(Me.lbCornYield.Text)
            drCorn("TPgc_ForecastPrice") = CDec(Me.lbCornPrice.Text)
            drCorn("TPgc_ForecastProfit") = CDec(Me.lbCornProfit.Text)
            drCorn("TPgc_SuitValue") = Me.lbCornSuit.Text
            drCorn("TPgc_UseWaterValue") = CDec(Me.lbCornWater.Text)
            drCorn("TPgc_DemandValue") = CDec(Me.lbCornDemand.Text)
            drCorn("TPgc_ScoreValue") = CDec(Me.lbCornScore.Text)
            dt.Rows.Add(drCorn)

            Dim drCane As DataRow = dt.NewRow
            drCane("TPgc_SPntgp_Id") = Plant_Enum._Cane
            drCane("TPgc_EndDate") = Me.MaxCaneCrop
            drCane("TPgc_ForecastCost") = CDec(Me.lbCaneCost.Text)
            drCane("TPgc_ForecastProduct") = CDec(Me.lbCaneYield.Text)
            drCane("TPgc_ForecastPrice") = CDec(Me.lbCanePrice.Text)
            drCane("TPgc_ForecastProfit") = CDec(Me.lbCaneProfit.Text)
            drCane("TPgc_SuitValue") = Me.lbCaneSuit.Text
            drCane("TPgc_UseWaterValue") = CDec(Me.lbCaneWater.Text)
            drCane("TPgc_DemandValue") = CDec(Me.lbCaneDemand.Text)
            drCane("TPgc_ScoreValue") = CDec(Me.lbCaneScore.Text)
            dt.Rows.Add(drCane)

            Dim drCassava As DataRow = dt.NewRow
            drCassava("TPgc_SPntgp_Id") = Plant_Enum._Cassava
            drCassava("TPgc_EndDate") = Me.MaxCassavaCrop
            drCassava("TPgc_ForecastCost") = CDec(Me.lbCassavaCost.Text)
            drCassava("TPgc_ForecastProduct") = CDec(Me.lbCassavaYield.Text)
            drCassava("TPgc_ForecastPrice") = CDec(Me.lbCassavaPrice.Text)
            drCassava("TPgc_ForecastProfit") = CDec(Me.lbCassavaProfit.Text)
            drCassava("TPgc_SuitValue") = Me.lbCassavaSuit.Text
            drCassava("TPgc_UseWaterValue") = CDec(Me.lbCassavaWater.Text)
            drCassava("TPgc_DemandValue") = CDec(Me.lbCassavaDemand.Text)
            drCassava("TPgc_ScoreValue") = CDec(Me.lbCassavaScore.Text)
            dt.Rows.Add(drCassava)

        Catch ex As Exception
            Throw New Exception("Fn-GetDataItem:[" & ex.Message & "]")
        End Try

        Return dt
    End Function

    Private Sub SetLink()
        Try
            Dim i As Integer = 1
            Me.lbt1.Text = ""
            Me.lbt2.Text = ""
            Me.lbt3.Text = ""
            Me.lbt4.Text = ""
            Me.lbt5.Text = ""
            Me.lbt6.Text = ""
            Me.lbt7.Text = ""
            Me.lbt8.Text = ""
            Me.lbt9.Text = ""
            Me.lbt10.Text = ""
            Me.lbt1.Visible = False
            Me.lbt2.Visible = False
            Me.lbt3.Visible = False
            Me.lbt4.Visible = False
            Me.lbt5.Visible = False
            Me.lbt6.Visible = False
            Me.lbt7.Visible = False
            Me.lbt8.Visible = False
            Me.lbt9.Visible = False
            Me.lbt10.Visible = False
            Me.bc1.Visible = False
            Me.bc2.Visible = False

            For Each dr As DataRow In Me.dtIrrigation.Rows
                Select Case i
                    Case 1
                        Me.lbt1.Text = dr("SIrrg_Name")
                        Me.lbt1.Visible = True
                        Me.SetGraph(Me.bc1, 1)
                        Me.bc1.Visible = True

                    Case 2
                        Me.lbt2.Text = dr("SIrrg_Name")
                        Me.lbt2.Visible = True
                        Me.SetGraph(Me.bc2, 2)
                        Me.bc2.Visible = True

                    Case 3
                        Me.lbt3.Text = dr("SIrrg_Name")
                        Me.lbt3.Visible = True
                        Me.SetGraph(Me.bc3, 3)
                        Me.bc3.Visible = True

                    Case 4
                        Me.lbt4.Text = dr("SIrrg_Name")
                        Me.lbt4.Visible = True
                        Me.SetGraph(Me.bc4, 4)
                        Me.bc4.Visible = True

                    Case 5
                        Me.lbt5.Text = dr("SIrrg_Name")
                        Me.lbt5.Visible = True
                        Me.SetGraph(Me.bc5, 5)
                        Me.bc5.Visible = True

                    Case 6
                        Me.lbt6.Text = dr("SIrrg_Name")
                        Me.lbt6.Visible = True
                        Me.SetGraph(Me.bc6, 6)
                        Me.bc6.Visible = True

                    Case 7
                        Me.lbt7.Text = dr("SIrrg_Name")
                        Me.lbt7.Visible = True
                        Me.SetGraph(Me.bc7, 7)
                        Me.bc7.Visible = True

                    Case 8
                        Me.lbt8.Text = dr("SIrrg_Name")
                        Me.lbt8.Visible = True
                        Me.SetGraph(Me.bc8, 8)
                        Me.bc8.Visible = True

                    Case 9
                        Me.lbt9.Text = dr("SIrrg_Name")
                        Me.lbt9.Visible = True
                        Me.SetGraph(Me.bc9, 9)
                        Me.bc9.Visible = True

                    Case 10
                        Me.lbt10.Text = dr("SIrrg_Name")
                        Me.lbt10.Visible = True
                        Me.SetGraph(Me.bc10, 10)
                        Me.bc10.Visible = True

                    Case Else
                        Exit For
                End Select
                i += 1
            Next
        Catch ex As Exception
            Throw New Exception("Fn-SetLink:[" & ex.Message & "]")
        End Try
    End Sub

    Private Sub SetGraph(ByRef bc As RadHtmlChart, ByVal i As Integer)
        Try
            bc.ChartTitle.Text = Me.dtIrrigation.Rows(i - 1)("SIrrg_Name")
            Dim xD As AxisItem = New AxisItem
            xD.LabelText = CDate(Me.dtIrrigation.Rows(i - 1)("TIrrgw_Date")).ToString("dd-MM-yyyy")
            Dim xAVG3 As AxisItem = New AxisItem
            xAVG3.LabelText = "เฉลี่ย 7 วัน"
            Dim xAVG5 As AxisItem = New AxisItem
            xAVG5.LabelText = "เฉลี่ย 3 ปี"
            Dim xAVG7 As AxisItem = New AxisItem
            xAVG7.LabelText = "เฉลี่ย 5 ปี"
            bc.PlotArea.XAxis.Items.Clear()
            bc.PlotArea.XAxis.Items.Add(xD)
            bc.PlotArea.XAxis.Items.Add(xAVG3)
            bc.PlotArea.XAxis.Items.Add(xAVG5)
            bc.PlotArea.XAxis.Items.Add(xAVG7)
            bc.PlotArea.XAxis.TitleAppearance.Text = Me.GetThaiMonth(Month(CDate(Me.dtIrrigation.Rows(i - 1)("TIrrgw_Date"))))

            Dim Qty As BarSeries = DirectCast(bc.PlotArea.Series.Item(0), BarSeries)
            Dim QtyC As CategorySeriesItem = New CategorySeriesItem
            QtyC.Y = CDec(Me.dtIrrigation.Rows(i - 1)("TIrrgw_Qty"))
            Dim Qty7AVG As CategorySeriesItem = New CategorySeriesItem
            Qty7AVG.Y = CDec(Me.dtIrrigation.Rows(i - 1)("Qty7AVG"))
            Dim Qty3AVG As CategorySeriesItem = New CategorySeriesItem
            Qty3AVG.Y = CDec(Me.dtIrrigation.Rows(i - 1)("Qty3AVG"))
            Dim Qty5AVG As CategorySeriesItem = New CategorySeriesItem
            Qty5AVG.Y = CDec(Me.dtIrrigation.Rows(i - 1)("Qty5AVG"))
            Qty.SeriesItems.Clear()
            Qty.SeriesItems.Add(QtyC)
            Qty.SeriesItems.Add(Qty7AVG)
            Qty.SeriesItems.Add(Qty3AVG)
            Qty.SeriesItems.Add(Qty5AVG)

            Dim Percent As BarSeries = DirectCast(bc.PlotArea.Series.Item(1), BarSeries)
            Dim QtyPC As CategorySeriesItem = New CategorySeriesItem
            QtyPC.Y = CDec(Me.dtIrrigation.Rows(i - 1)("TIrrgw_QtyPercent"))
            Dim QtyPercent7AVG As CategorySeriesItem = New CategorySeriesItem
            QtyPercent7AVG.Y = CDec(Me.dtIrrigation.Rows(i - 1)("QtyPercent7AVG"))
            Dim QtyPercent3AVG As CategorySeriesItem = New CategorySeriesItem
            QtyPercent3AVG.Y = CDec(Me.dtIrrigation.Rows(i - 1)("QtyPercent3AVG"))
            Dim QtyPercent5AVG As CategorySeriesItem = New CategorySeriesItem
            QtyPercent5AVG.Y = CDec(Me.dtIrrigation.Rows(i - 1)("QtyPercent5AVG"))
            Percent.SeriesItems.Clear()
            Percent.SeriesItems.Add(QtyPC)
            Percent.SeriesItems.Add(QtyPercent7AVG)
            Percent.SeriesItems.Add(QtyPercent3AVG)
            Percent.SeriesItems.Add(QtyPercent5AVG)
        Catch ex As Exception
            Throw New Exception("Fn-BindMenu : [" & ex.Message & "]")
        End Try
    End Sub

    Private Function GetThaiMonth(ByVal m As Integer) As String
        Dim str As String = ""
        Try
            Select Case m
                Case 1
                    str = "มกราคม"
                Case 2
                    str = "กุมภาพันธ์"
                Case 3
                    str = "มีนาคม"
                Case 4
                    str = "เมษายน"
                Case 5
                    str = "พฤษภาคม"
                Case 6
                    str = "มิถุนายน"
                Case 7
                    str = "กรกฎาคม"
                Case 8
                    str = "สิงหาคม"
                Case 9
                    str = "กันยายน"
                Case 10
                    str = "ตุลาคม"
                Case 11
                    str = "พฤษจิกายน"
                Case 12
                    str = "ธันวาคม"
            End Select
        Catch ex As Exception
            Throw New Exception("Fn-GetThaiMonth : [" & ex.Message & "]")
        End Try

        Return str
    End Function

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