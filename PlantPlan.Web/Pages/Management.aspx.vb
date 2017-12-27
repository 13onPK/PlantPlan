Imports Telerik.Web.UI
Imports System.Data.SqlClient
Imports System.Linq

Public Class Management
    Inherits PageBase

#Region "Properties"
    Private Property dtCrop As DataTable
        Get
            Return ViewState("dtCrop")
        End Get
        Set(ByVal value As DataTable)
            ViewState("dtCrop") = value
        End Set
    End Property
#End Region

#Region "Events"

    Private Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                If MySession.IsLogin = False Then
                    Me.Page.Response.Redirect("../Default.aspx")
                End If
            End If
        Catch ex As Exception
            JavaScript.Alert(Me.Page, "Ev-Page_Load:[" & ex.Message & "]")
        End Try
    End Sub

    Protected Sub grdCrop_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs)
        Try
            Dim grd As RadGrid = DirectCast(sender, RadGrid)

            Dim sorting As String = ""
            For Each ColSort As GridSortExpression In grd.MasterTableView.SortExpressions
                sorting = sorting + ColSort.ToString + ", "
            Next

            sorting = sorting.Substring(0, IIf(sorting.Length > 2, sorting.Length - 2, 0))

            Dim bdCon As New DBConnection()
            Dim Param As New List(Of SqlParameter)() From {New SqlParameter("@SPntgp_Name", grd.MasterTableView.Columns.FindByUniqueName("SPntgp_Name").CurrentFilterValue),
                                                           New SqlParameter("@SProv_Name", grd.MasterTableView.Columns.FindByUniqueName("SProv_Name").CurrentFilterValue),
                                                           New SqlParameter("@SAmp_Name", grd.MasterTableView.Columns.FindByUniqueName("SAmp_Name").CurrentFilterValue),
                                                           New SqlParameter("@STamb_Name", grd.MasterTableView.Columns.FindByUniqueName("STamb_Name").CurrentFilterValue),
                                                           New SqlParameter("@TPgc_FarmerName", grd.MasterTableView.Columns.FindByUniqueName("TPgc_FarmerName").CurrentFilterValue),
                                                           New SqlParameter("@TPgc_Area", grd.MasterTableView.Columns.FindByUniqueName("TPgc_Area").CurrentFilterValue),
                                                           New SqlParameter("@TPgc_StartDate", grd.MasterTableView.Columns.FindByUniqueName("TPgc_StartDate").CurrentFilterValue),
                                                           New SqlParameter("@TPgc_EndDate", grd.MasterTableView.Columns.FindByUniqueName("TPgc_EndDate").CurrentFilterValue),
                                                           New SqlParameter("@TPgc_IsCompleted", grd.MasterTableView.Columns.FindByUniqueName("TPgc_IsCompleted").CurrentFilterValue),
                                                           New SqlParameter("@PageSize", grd.PageSize),
                                                           New SqlParameter("@PageNum", grd.CurrentPageIndex),
                                                           New SqlParameter("@sorting", sorting)}
            Dim ds As DataSet = bdCon.ExecuteDataset("dbo.PlantGroupCrop_T_SelListCustom", Param)

            If ds.Tables.Count = 2 Then
                grd.VirtualItemCount = ds.Tables(0).Rows(0)("AllRow")
                grd.DataSource = ds.Tables(1)
                Me.dtCrop = ds.Tables(1)
            Else
                grd.DataSource = Nothing
            End If
        Catch ex As Exception
            JavaScript.Alert(Me.Page, "Ev-grdCrop_NeedDataSource : [" & ex.Message & "]")
        End Try
    End Sub

    Private Sub grdCrop_ItemCommand(sender As Object, e As GridCommandEventArgs) Handles grdCrop.ItemCommand
        Try
            If e.CommandName = RadGrid.EditCommandName Then
                Dim editedItem As GridEditableItem = CType(e.Item, GridEditableItem)
                Dim TPgc_Id = editedItem.OwnerTableView.DataKeyValues(editedItem.ItemIndex)("TPgc_Id")
                Dim dr() As DataRow = Me.dtCrop.Select("TPgc_Id =" & TPgc_Id)

                If dr.Count = 1 Then
                    If dr(0)("TPgc_IsCompleted") = True Then
                        ManagementSession.PageAction = PageAction_Enum.View
                    Else
                        ManagementSession.PageAction = PageAction_Enum.Update
                    End If

                    ManagementSession.TPgc_Id = TPgc_Id
                    Me.Page.Response.Redirect("ManagementForm.aspx")
                End If


            ElseIf e.CommandName = "View" Then
                Dim editedItem As GridEditableItem = CType(e.Item, GridEditableItem)
                Dim TPgc_Id = editedItem.OwnerTableView.DataKeyValues(editedItem.ItemIndex)("TPgc_Id")
                ManagementSession.PageAction = PageAction_Enum.View
                ManagementSession.TPgc_Id = TPgc_Id
                Me.Page.Response.Redirect("ManagementForm.aspx")
            End If
        Catch ex As Exception
            JavaScript.Alert(Me.Page, "Ev-grdCrop_ItemCommand : [" & ex.Message & "]")
        End Try
    End Sub

    Private Sub grdCrop_EditCommand(sender As Object, e As GridCommandEventArgs) Handles grdCrop.EditCommand
        Try
            e.Item.Edit = False
        Catch ex As Exception
            JavaScript.Alert(Me.Page, "Ev-grdCrop_EditCommand : [" & ex.Message & "]")
        End Try
    End Sub

    Protected Sub cbxSPntgp_Init(sender As Object, e As EventArgs)
        Try
            Dim ddPlantGroup As RadComboBox = TryCast(sender, RadComboBox)
            Dim bdCon As New DBConnection()
            Dim Param As New List(Of SqlParameter)() From {}

            Dim ds As DataSet = bdCon.ExecuteDataset("dbo.PlantGroup_S_SelDDL", Param)

            Dim dr = ds.Tables(0).NewRow
            dr("Name") = "-- All --"
            ds.Tables(0).Rows.InsertAt(dr, 0)

            ddPlantGroup.DataSource = ds.Tables(0)
            ddPlantGroup.DataTextField = "Name"
            ddPlantGroup.DataValueField = "ID"
            ddPlantGroup.DataBind()
        Catch ex As Exception
            JavaScript.Alert(Me.Page, "Ev-cbxSPntgp_Init:[" & ex.Message & "]")
        End Try
    End Sub

    Protected Sub cbxSProv_Init(sender As Object, e As EventArgs)
        Try
            Dim ddProvince As RadComboBox = TryCast(sender, RadComboBox)
            Dim bdCon As New DBConnection()
            Dim ds As DataSet = bdCon.ExecuteDataset("dbo.Province_S_SelDDL", Nothing)

            Dim dr = ds.Tables(0).NewRow
            dr("Name") = "-- All --"
            ds.Tables(0).Rows.InsertAt(dr, 0)

            ddProvince.DataSource = ds.Tables(0)
            ddProvince.DataTextField = "Name"
            ddProvince.DataValueField = "ID"
            ddProvince.DataBind()
        Catch ex As Exception
            JavaScript.Alert(Me.Page, "Ev-cbxSProv_Init:[" & ex.Message & "]")
        End Try
    End Sub

    Protected Sub cbxSAmp_Init(sender As Object, e As EventArgs)
        Try
            Dim ddAmphoe As RadComboBox = TryCast(sender, RadComboBox)
            Dim bdCon As New DBConnection()
            Dim Param As New List(Of SqlParameter)() From {New SqlParameter("@SProv_Id", Nothing)}
            Dim ds As DataSet = bdCon.ExecuteDataset("dbo.Amphoe_S_SelDDL", Param)

            Dim dr = ds.Tables(0).NewRow
            dr("Name") = "-- All --"
            ds.Tables(0).Rows.InsertAt(dr, 0)

            ddAmphoe.DataSource = ds.Tables(0)
            ddAmphoe.DataTextField = "Name"
            ddAmphoe.DataValueField = "ID"
            ddAmphoe.DataBind()
        Catch ex As Exception
            JavaScript.Alert(Me.Page, "Ev-cbxSAmp_Init:[" & ex.Message & "]")
        End Try
    End Sub

    Protected Sub cbxSTamb_Init(sender As Object, e As EventArgs)
        Try
            Dim ddTambon As RadComboBox = TryCast(sender, RadComboBox)
            Dim bdCon As New DBConnection()
            Dim Param As New List(Of SqlParameter)() From {New SqlParameter("@SAmp_Id", Nothing)}
            Dim ds As DataSet = bdCon.ExecuteDataset("dbo.Tambon_S_SelDDL", Param)

            Dim dr = ds.Tables(0).NewRow
            dr("Name") = "-- All --"
            ds.Tables(0).Rows.InsertAt(dr, 0)

            ddTambon.DataSource = ds.Tables(0)
            ddTambon.DataTextField = "Name"
            ddTambon.DataValueField = "ID"
            ddTambon.DataBind()
        Catch ex As Exception
            JavaScript.Alert(Me.Page, "Ev-cbxSTamb_Init:[" & ex.Message & "]")
        End Try
    End Sub

#End Region

End Class