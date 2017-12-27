<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="MediumDam.aspx.vb" Inherits="PlantPlan.Web.MediumDam" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="refresh" content="10"/>

    <script type="text/javascript" src="../Scripts/csvparse.js" ></script>
    <script type="text/javascript" src="../Scripts/convertJSON.js"></script>
    <script type="text/javascript" src="../Scripts/csvup.js"></script>
    <script type="text/javascript" src="../Scripts/underscore-min.js"></script>
    <script type="text/javascript" src="../Scripts/strsup.js"></script>
    <script type="text/javascript" src="https://code.jquery.com/jquery-3.1.1.min.js" integrity="sha256-hVVnYaiADRTO2PzUGmuLJr8BLUSjGIZsDYGmIJLv2b8=" crossorigin="anonymous" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
            <Scripts>
                
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js" />
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js" />
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js" />
            </Scripts>
        </telerik:RadScriptManager>
        <script type="text/javascript">
            function getWaterFlow() {
                $.ajax({
                    type: "POST",
                    url: "KeepDataWater.aspx/GetMediumIrrList",
                    data: "",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        var objdata = $.parseJSON(data.d);
                        $.each(objdata, function () {
                            console.log(this['MaxDate'])
                            var MaxDate = new Date(this['MaxDate']);
                            var now = new Date();
                            var daysOfYear = [];
                            var md = MaxDate.format('yyyy-MM-dd');

                            //for (var d = MaxDate ; d <= now ; d.setDate(d.getDate() + 1)) {
                                //console.log(d)
                                
                                $.ajax({
                                    type: "POST",
                                    url: "KeepDataWater.aspx/GetDataMedium",
                                    data: "{p_Date: '" + md + "'}",
                                    contentType: "application/json; charset=utf-8",
                                    dataType: "json",
                                    success: function (data) {
                                        var input = data.d;
                                        var output = run(1, input, output);
                                        //console.log(input)
                                        console.log(output)

                                        var param = new Object();
                                        param.p_Date = MaxDate.format('yyyy-MM-dd');
                                        param.p_Data = output;

                                        $.ajax({
                                            type: "POST",
                                            url: "KeepDataWater.aspx/KeepMediumIrrWater",
                                            data:JSON.stringify(param),
                                            contentType: "application/json; charset=utf-8",
                                            dataType: "json",
                                            success: function (msg) {
                                                //console.log("OK");
                                                console.log(msg.d[0].msg);
                                            }
                                        });

                                    },
                                    error: function (xhr, status, error) {
                                        console.log("An error has occurred during processing: " + error);
                                    }
                                });

                            //}
                            
                        });
                    }
                });
            }

            window.onload = getWaterFlow;


        </script>
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        </telerik:RadAjaxManager>
        <div>
            <asp:HiddenField ID="hdData" runat="server" />
            <asp:TextBox ID="textArea_input" ClientIDMode="Static" runat="server" TextMode="MultiLine" Rows="10"></asp:TextBox>
            <asp:TextBox ID="textArea_output" ClientIDMode="Static" runat="server" TextMode="MultiLine" Rows="10"></asp:TextBox>
        </div>
        <div id="divHtml" style="display:none; visibility: hidden; height: 5px"></div>
    </form>
</body>
</html>
