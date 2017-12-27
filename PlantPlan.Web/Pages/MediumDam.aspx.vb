Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Net
Imports PlantPlan.Web.DBConnection

Public Class MediumDam
    Inherits PageBase

    Private Sub Pages_MediumDam_Load(sender As Object, e As EventArgs) Handles Me.Load
        Me.GetData()
    End Sub

    Private Sub GetData()
        Dim p_Date As String = "2012-06-01"
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
            Dim bdCon As New DBConnection()
            Dim Param As New List(Of SqlParameter)() From {New SqlParameter("@Date", CDate(p_Date)),
                                                           New SqlParameter("@Data", ""),
                                                           New SqlParameter("@CrBy", 0)
                                                          }
            Dim ds As DataSet = bdCon.ExecuteDataset("dbo.IrrigationWater_T_ManpMedium", Param)
        End If
    End Sub


End Class