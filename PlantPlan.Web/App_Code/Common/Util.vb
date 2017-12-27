Imports System.Threading
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Data
Imports Newtonsoft.Json
Imports System.Linq
Imports System.Xml
Imports System.IO

Friend Class Util
    Private TimeLimit As Integer = 40

    Public Shared Function GetPolicy(ByVal p_AppID As Integer, ByVal p_UserID As Integer, ByVal p_MenuID As Integer, ByRef Title As String, ByRef Msg As String, Optional ByRef p_Creatable As Boolean = False, Optional ByRef p_Editable As Boolean = False, Optional ByRef p_Deletable As Boolean = False, Optional ByRef p_Viewable As Boolean = False, Optional ByRef p_ControlBranch As Boolean = False, Optional ByRef p_ControlOwner As Boolean = False) As Boolean
        Dim accress As Boolean = False

        Try
            Dim bdCon As New DBConnection()
            Dim Param As New List(Of SqlParameter)() From {New SqlParameter("@MApp_Id", p_AppID), New SqlParameter("@MUser_Id", p_UserID), New SqlParameter("@MMenu_Id", p_MenuID)}
            Dim ds As DataSet = bdCon.ExecuteDataset("dbo.MenusInRolesInApplications_M_Sel", Param)

            If ds.Tables(0).Rows.Count = 1 Then
                p_Creatable = ds.Tables(0).Rows(0)("MMra_Creatable")
                p_Editable = ds.Tables(0).Rows(0)("MMra_Editable")
                p_Deletable = ds.Tables(0).Rows(0)("MMra_Deletable")
                p_Viewable = ds.Tables(0).Rows(0)("MMra_Viewable")
                p_ControlBranch = ds.Tables(0).Rows(0)("MMra_Viewable")
                p_ControlBranch = ds.Tables(0).Rows(0)("MMra_ControlBranch")
                p_ControlOwner = ds.Tables(0).Rows(0)("MMra_ControlOwner")
                accress = True

            ElseIf ConfigurationManager.AppSettings("SSOType") = 0 Then
                p_Creatable = True
                p_Editable = True
                p_Deletable = True
                p_Viewable = True
                p_ControlBranch = False
                p_ControlOwner = False
                accress = True

            Else
                p_Creatable = False
                p_Editable = False
                p_Deletable = False
                p_ControlBranch = False
                p_ControlOwner = False

                Dim wrapper As New CryptoProvider("Error")
                Title = wrapper.EncryptData("Access denied")
                Msg = wrapper.EncryptData("Your permission can't access that page")
            End If

        Catch ex As Exception
            Throw New Exception("Fn-GetPolicy : [" & ex.Message & "]")
        End Try

        Return accress
    End Function

    Public Shared Function FillDateInName(FileName As String, ReviseName As String) As String
        Dim NewName As String = ""
        Try
            Dim str() As String = FileName.Split(".")

            If str.Count = 2 Then
                NewName = IIf(ReviseName.Trim <> "", ReviseName, str(0)) + Now.ToString("yyyyMMdd-hhmmss") + "." + str(1)
            Else
                Throw New Exception("Name has wrong format!!")
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return NewName
    End Function

    Public Shared Function ConvertDataTableToXmlString(ByVal c_Datatable As DataTable) As String
        Dim xmlString As String = String.Empty

        If c_Datatable IsNot Nothing Then
            Try
                'c_Datatable.TableName = "Item"
                'ds.Tables.Add(c_Datatable)
                'xmlString = ds.GetXml

                Dim Xmldoc As New XmlDocument
                Dim rootNode As XmlNode = Xmldoc.CreateElement("Items")

                Dim Data As String
                Dim ColumnName As String
                Dim ColumnType As Type

                For RowIndex As Integer = 0 To c_Datatable.Rows.Count - 1

                    Dim ItemNode As XmlNode = Xmldoc.CreateElement("Item")

                    For ColumnIdex As Integer = 0 To c_Datatable.Columns.Count - 1

                        If c_Datatable.Rows(RowIndex)(ColumnIdex) IsNot DBNull.Value Then

                            Data = c_Datatable.Rows(RowIndex)(ColumnIdex).ToString()
                            ColumnName = c_Datatable.Columns(ColumnIdex).ColumnName.ToString()
                            ColumnType = c_Datatable.Columns(ColumnIdex).DataType()


                            Dim DataNode As XmlNode = Xmldoc.CreateElement(ColumnName)

                            If ColumnType.Equals(GetType(String)) Then
                                Dim cdata As XmlCDataSection = Xmldoc.CreateCDataSection(Data)
                                DataNode.AppendChild(cdata)
                            Else
                                DataNode.InnerText = Data.ToString()
                            End If
                            ItemNode.AppendChild(DataNode)
                        Else


                        End If
                    Next
                    rootNode.AppendChild(ItemNode)
                Next

                Xmldoc.AppendChild(rootNode)

                xmlString = Xmldoc.InnerXml.ToString()

            Catch ex As Exception
                xmlString = String.Empty
            End Try
        End If

        Return xmlString
    End Function

    Public Shared Function FindMaxValueInDataTable(dt As DataTable, Column As String) As Integer
        Dim Max As Integer = 0
        Try
            Max = Convert.ToInt32(IIf(dt.Rows.Count = 0, 0, dt.Compute("Max(" & Column & ")", String.Empty)))
        Catch ex As Exception
            Throw ex
        End Try
        Return Max
    End Function

    Public Shared Function FindMinValueInDataTable(dt As DataTable, Column As String) As Integer
        Dim Min As Integer = 0
        Try
            Min = Convert.ToInt32(IIf(dt.Rows.Count = 0, 0, dt.Compute("Min(" & Column & ")", String.Empty)))
        Catch ex As Exception
            Throw ex
        End Try
        Return Min
    End Function

    Public Shared Function CheckNullToNumber(ByVal c_Value As String, ByVal c_DefaultVal As Integer) As Integer
        Dim rtn As String
        Try
            If IsNothing(c_Value) OrElse c_Value = String.Empty OrElse c_Value.ToString.Trim = "" Then
                rtn = c_DefaultVal
            Else
                rtn = c_Value
            End If
        Catch ex As Exception
            rtn = Nothing
        End Try

        Return rtn
    End Function

    Public Shared Function CheckNullToLong(ByVal c_Value As String, ByVal c_DefaultVal As Long) As Long
        Dim rtn As Long
        Try
            If IsNothing(c_Value) OrElse c_Value = String.Empty OrElse c_Value.ToString.Trim = "" Then
                rtn = c_DefaultVal
            Else
                rtn = c_Value
            End If
        Catch ex As Exception
            rtn = Nothing
        End Try

        Return rtn
    End Function

    Public Shared Function CheckNullToDecimal(ByVal c_Value As String, ByVal c_DefaultVal As Decimal) As Decimal
        Dim rtn As Decimal
        Try
            If IsNothing(c_Value) OrElse c_Value = String.Empty OrElse c_Value.ToString.Trim = "" Then
                rtn = c_DefaultVal
            Else
                rtn = c_Value
            End If
        Catch ex As Exception
            rtn = Nothing
        End Try

        Return rtn
    End Function

    Public Shared Function ConcatStringFromDataTable(ByVal p_col As String, ByVal p_table As DataTable) As String
        Dim str As String = ""
        Try
            Dim distinctDT As DataTable = p_table.DefaultView.ToTable(True, p_col)

            For Each dr As DataRow In distinctDT.Rows
                str = str + dr(p_col).ToString + ","
            Next

            str = str.Substring(0, str.Length - 1)
        Catch ex As Exception
            str = Nothing
        End Try

        Return str
    End Function

    Public Shared Function KeepDataInDataTable(ByVal p_criteria As String, ByVal p_table As DataTable) As DataTable
        Dim newDT As DataTable
        Try
            Dim drs() As DataRow = p_table.Select(p_criteria)

            newDT = drs.CopyToDataTable
        Catch ex As Exception
            newDT = Nothing
        End Try

        Return newDT
    End Function


    Public Function CheckNullString(ByVal c_Value As String) As String
        Dim rtn As String
        Try
            If c_Value = String.Empty OrElse IsNothing(c_Value) OrElse c_Value.ToString.Trim = "" OrElse c_Value.ToString.Trim = "-1" Then
                rtn = String.Empty
            Else
                rtn = c_Value
            End If
        Catch ex As Exception
            rtn = Nothing
        End Try

        Return rtn
    End Function

    'Public Function CheckNullDateTime(ByVal c_Value As DateTime, ByVal format As String, ByVal provider As Globalization.CultureInfo) As String
    '    Dim rtn As String
    '    Try
    '        If IsNothing(c_Value) OrElse c_Value = #12:00:00 AM# OrElse c_Value.ToString.Trim = "" OrElse c_Value = Date.MinValue Then
    '            rtn = Nothing
    '        Else
    '            rtn = c_Value.ToString(format, provider)
    '        End If
    '    Catch ex As Exception
    '        rtn = Nothing
    '    End Try

    '    Return rtn
    'End Function

    Public Function CheckDateTime(ByVal c_Value As Global.System.Nullable(Of Date)) As Global.System.Nullable(Of Date)
        Dim rtn As Global.System.Nullable(Of Date) = Nothing
        Try
            If IsNothing(c_Value) OrElse c_Value = #12:00:00 AM# OrElse c_Value.ToString.Trim = "" OrElse c_Value = Date.MinValue Then
                rtn = Nothing ' CDate("1/1/2443")
            Else
                rtn = c_Value
            End If
        Catch ex As Exception
            rtn = Nothing       'CDate("1/1/2443")
        End Try

        Return rtn
    End Function

    Public Function CheckNullNumber(ByVal c_Value As Integer) As String
        Dim rtn As String
        Try
            If IsNothing(c_Value) OrElse c_Value = 0 OrElse c_Value = 0.0 OrElse c_Value.ToString.Trim = "" Then
                rtn = String.Empty
            Else
                rtn = c_Value
            End If
        Catch ex As Exception
            rtn = Nothing
        End Try

        Return rtn
    End Function

    Public Function GetNumber(ByVal c_Value As String) As String
        Dim rtn As String
        Try
            If IsNothing(c_Value) OrElse c_Value = String.Empty OrElse c_Value.ToString.Trim = "" OrElse c_Value = "0" Then
                rtn = 0
            Else
                rtn = c_Value
            End If
        Catch ex As Exception
            rtn = 0
        End Try

        Return rtn
    End Function

    Public Function CheckNullBoolean(ByVal c_Value As Boolean) As String
        Dim rtn As String
        Try
            If IsNothing(c_Value) OrElse c_Value = False OrElse c_Value.ToString.Trim = "" Then
                rtn = String.Empty
            Else
                rtn = c_Value
            End If
        Catch ex As Exception
            rtn = Nothing
        End Try

        Return rtn
    End Function

    'Public Function CheckNullToDateTime(ByVal c_Value As String, ByVal format As String, ByVal provider As Globalization.CultureInfo) As DateTime
    '    Dim rtn As DateTime
    '    Try
    '        If IsNothing(c_Value) OrElse c_Value.ToString = String.Empty OrElse c_Value.ToString.Trim = "" _
    '                OrElse c_Value = "#12:00:00 AM#" OrElse c_Value = "0:00:00" Then
    '            rtn = DateTime.ParseExact("01/01/1900", format, provider)
    '        Else
    '            rtn = DateTime.ParseExact(c_Value, format, provider)
    '        End If
    '    Catch ex As Exception
    '        rtn = Nothing
    '    End Try

    '    Return rtn
    'End Function

    Public Function CheckNullToDateTimeControl(ByVal c_Value As String) As DateTime
        Dim rtn As DateTime
        Try
            If IsNothing(c_Value) OrElse c_Value = String.Empty OrElse c_Value.ToString.Trim = "" OrElse c_Value = "#12:00:00 AM#" OrElse c_Value = "0:00:00" Then
                rtn = Nothing
            Else
                rtn = c_Value
            End If
        Catch ex As Exception
            rtn = Nothing
        End Try

        Return rtn
    End Function

    Public Function UpdateXmlInfo(ByVal FileName As String, ByVal XMLpath As String, ByVal NodeKey As String, ByVal NodeUpdate As String, ByVal ValKey As String, ByVal ValUpdate As String) As Boolean

        UpdateXmlInfo = False
        'System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("en-US")

        Dim Timer As Integer = 0
        Try
            Dim ds As New DataSet
            Dim xmlDoc As XmlDocument = New XmlDocument()
            While isFileOpen(FileName)
                If Timer >= TimeLimit Then
                    Throw New Exception("Can not open file xml for loading to xmlDoc at path " & FileName)
                    'Throw New Exception(ex.Message)
                    'Exit Function
                End If
                Thread.Sleep(500)
                Timer = Timer + 1
            End While

            xmlDoc.Load(FileName)

            Dim nodeList As XmlNodeList = xmlDoc.SelectNodes(XMLpath)
            Dim node As XmlNode
            Dim i As Integer
            Dim j As Integer
            For Each node In nodeList
                For i = 0 To node.ChildNodes.Count - 1
                    'If node.ChildNodes(i).Name = NodeKey And node.ChildNodes(i).InnerText = ValKey Then
                    For j = 0 To node.ChildNodes.Count - 1
                        If node.ChildNodes(j).Name = NodeUpdate Then
                            node.ChildNodes(j).InnerText = ValUpdate
                        End If
                    Next
                    'End If
                Next
            Next
            'ds.ReadXml(New XmlNodeReader(xmlDoc))
            'ds.WriteXml(FileName)
            'TimeLimit = 1
            Timer = 0
            While isFileOpen(FileName)
                If Timer >= TimeLimit Then
                    Throw New Exception("Can not update information to file xml at path " & FileName)
                    'Exit Function
                End If
                Thread.Sleep(500)
                Timer = Timer + 1
            End While
            xmlDoc.Save(FileName)

            Return True
        Catch ex As Exception

        End Try
    End Function

    Public Shared Function ConvertDataTableToJsonString(ByVal c_Datatable As DataTable) As String
        Dim JsonString As String = String.Empty
        Try
            JsonString = JsonConvert.SerializeObject(c_Datatable)
        Catch ex As Exception
            JsonString = String.Empty
        End Try

        Return JsonString
    End Function

    Public Shared Function ConvertJsonStringToDataTable(ByVal c_str As String) As DataTable
        Dim dt As DataTable = New DataTable
        Try
            dt = JsonConvert.DeserializeObject(Of DataTable)(c_str)
        Catch ex As Exception
            dt = New DataTable
        End Try

        Return dt
    End Function

    Private Function IsFileOpen(ByVal filename As String) As Boolean
        'System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("en-US")
        Dim sf As System.IO.FileStream
        Try
            sf = System.IO.File.Open(filename, FileMode.Open)
            sf.Close()
            Return False
        Catch ex As System.IO.IOException
            Return True
            ex = Nothing
        Finally
            sf = Nothing
        End Try
    End Function

    Public Function IsDate(ByVal datevalue As String, ByVal dateformat As String) As Boolean

        Dim ret As Boolean = False

        Try
            If String.IsNullOrEmpty(datevalue) OrElse datevalue.Trim = "" Then
                ret = True
            Else
                Dim dtfi As System.Globalization.DateTimeFormatInfo = New System.Globalization.DateTimeFormatInfo()
                dtfi.ShortDatePattern = dateformat

                Dim dt As DateTime = DateTime.ParseExact(datevalue, dateformat, dtfi)
                ret = True
            End If

        Catch ex As Exception
            ret = False
        End Try

        Return ret

    End Function

    Public Function ToExactDate(ByVal datevalue As String, ByVal dateformat As String) As Nullable(Of Date)

        Dim ret As Nullable(Of Date) = Nothing

        Try
            Dim ci As System.Globalization.CultureInfo = New System.Globalization.CultureInfo("en-US")
            ci.DateTimeFormat.ShortDatePattern = dateformat

            ret = DateTime.ParseExact(datevalue, dateformat, ci)

        Catch ex As Exception
            Throw ex
        End Try

        Return ret

    End Function

    Public Function CheckNullLabel(ByVal c_Value As String, ByVal c_Sequence As String) As String
        Dim rtn As String
        Try
            If c_Value = String.Empty OrElse IsNothing(c_Value) OrElse c_Value.ToString.Trim = "" OrElse c_Value.ToString.Trim = "-1" Then
                rtn = "Field # " & c_Sequence
            Else
                rtn = c_Value
            End If
        Catch ex As Exception
            rtn = Nothing
        End Try

        Return rtn
    End Function

End Class
