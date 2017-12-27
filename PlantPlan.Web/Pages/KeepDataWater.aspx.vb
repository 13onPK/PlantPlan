Imports System.Data
Imports System.Data.SqlClient
Imports System.Net
Imports System.Web.Services
Imports PlantPlan.Web.DBConnection

Public Class KeepDataWater
    Inherits PageBase

    <WebMethod>
    Public Shared Function GetBigIrrList() As String
        Dim dt As String = Nothing
        Try
            Dim bdCon As New DBConnection()
            Dim Param As New List(Of SqlParameter)() From {New SqlParameter("@SIrrg_Type", "Big")}

            Dim ds As DataSet = bdCon.ExecuteDataset("dbo.Irrigation_S_SelList", Param)
            dt = Util.ConvertDataTableToJsonString(ds.Tables(0))
        Catch ex As Exception
            dt = Nothing
        End Try
        Return dt
    End Function

    <WebMethod>
    Public Shared Function GetMediumIrrList() As String
        Dim dt As String = Nothing
        Try
            Dim bdCon As New DBConnection()
            Dim Param As New List(Of SqlParameter)() From {New SqlParameter("@SIrrg_Type", "Medium")}

            Dim ds As DataSet = bdCon.ExecuteDataset("dbo.Irrigation_S_SelList", Param)
            dt = Util.ConvertDataTableToJsonString(ds.Tables(0))
        Catch ex As Exception
            dt = Nothing
        End Try
        Return dt
    End Function

    <WebMethod>
    Public Shared Function GetDataMedium(ByVal p_Date As String) As String
        Dim Response As String = Nothing
        Try
            Response = "OK"
            Dim url = "http://water.rid.go.th/flood/rsvmiddle/rsvmiddle" + IIf(CDate(p_Date).Day.ToString.Length = 1, "0" + CDate(p_Date).Day.ToString, CDate(p_Date).Day.ToString) + IIf(CDate(p_Date).Month.ToString.Length = 1, "0" + CDate(p_Date).Month.ToString, CDate(p_Date).Month.ToString) + CDate(p_Date).Year.ToString + "_files/sheet001.htm"
            Dim str = New System.Net.WebClient().DownloadString(url)
            str = str.Substring(str.IndexOf("<table") - 1, str.IndexOf("</table>") - str.IndexOf("<table") + 9)
            Response = str
        Catch ex As Exception
            Response = ""
        End Try

        If Response = "" Then
            Try
                Response = "OK"
                Dim url = "http://water.rid.go.th/flood/rsvmiddle/rsvmiddle" + IIf(CDate(p_Date).Day.ToString.Length = 1, "0" + CDate(p_Date).Day.ToString, CDate(p_Date).Day.ToString) + IIf(CDate(p_Date).Month.ToString.Length = 1, "0" + CDate(p_Date).Month.ToString, CDate(p_Date).Month.ToString) + CDate(p_Date).Year.ToString + ".htm"
                Dim str = New System.Net.WebClient().DownloadString(url)
                str = str.Substring(str.IndexOf("<table") - 1, str.IndexOf("</table>") - str.IndexOf("<table") + 9)
                Response = str
            Catch ex As Exception
                Response = ""
            End Try
        End If


        If Response = "" Then
            Dim bdCon As New DBConnection()
            Dim Param As New List(Of SqlParameter)() From {New SqlParameter("@Date", CDate(p_Date)),
                                                           New SqlParameter("@Data", ""),
                                                           New SqlParameter("@CrBy", 0)
                                                          }
            Dim ds As DataSet = bdCon.ExecuteDataset("dbo.IrrigationWater_T_ManpMedium", Param)
        End If
        Return Response
    End Function

    <WebMethod>
    Public Shared Function KeepBigIrrWater(ByVal p_SIrrg_Id As String, ByVal p_Date As Date, ByVal p_Qty As Decimal, ByVal p_QtyPercent As Decimal, ByVal p_FlowInQty As Decimal, ByVal p_FlowOutQty As Decimal, ByVal p_UselessQty As Decimal) As String
        Dim msg As String = Nothing
        Try
            Dim bdCon As New DBConnection()
            Dim Param As New List(Of SqlParameter)() From {New SqlParameter("@TIrrgw_SIrrg_Id", p_SIrrg_Id),
                                                           New SqlParameter("@TIrrgw_Date", p_Date),
                                                           New SqlParameter("@TIrrgw_Qty", p_Qty),
                                                           New SqlParameter("@TIrrgw_QtyPercent", p_QtyPercent),
                                                           New SqlParameter("@TIrrgw_FlowInQty", p_FlowInQty),
                                                           New SqlParameter("@TIrrgw_FlowOutQty", p_FlowOutQty),
                                                           New SqlParameter("@TIrrgw_UselessQty", p_UselessQty),
                                                           New SqlParameter("@CrBy", 0)
                                                          }


            Dim ds As DataSet = bdCon.ExecuteDataset("dbo.IrrigationWater_T_ManpBig", Param)
            msg = Util.ConvertDataTableToJsonString(ds.Tables(0))

        Catch ex As Exception
            msg = Nothing
        End Try
        Return msg
    End Function

    <WebMethod>
    Public Shared Function KeepMediumIrrWater(ByVal p_Date As Date, ByVal p_Data As String) As String
        Dim msg As String = Nothing
        Try
            Dim bdCon As New DBConnection()
            Dim Param As New List(Of SqlParameter)() From {New SqlParameter("@Date", p_Date),
                                                           New SqlParameter("@Data", p_Data),
                                                           New SqlParameter("@CrBy", 0)
                                                          }

            Dim ds As DataSet = bdCon.ExecuteDataset("dbo.IrrigationWater_T_ManpMedium", Param)
            msg = Util.ConvertDataTableToJsonString(ds.Tables(0))

        Catch ex As Exception
            msg = Nothing
        End Try
        Return msg
    End Function

End Class