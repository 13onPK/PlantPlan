<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="BigDam.aspx.vb" Inherits="PlantPlan.Web.BigDam" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
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
                    url: "KeepDataWater.aspx/GetBigIrrList",
                    data: "",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        var objdata = $.parseJSON(data.d);
                        $.each(objdata, function () {
                            console.log(this['SIrrg_Code'])
                            console.log(this['SIrrg_Name'])
                            console.log(this['MaxDate'])

                            var StoreQ = this['SIrrg_StoreQty'];
                            var MaxDate = new Date(this['MaxDate']);
                            var DamID = this['SIrrg_Id'];

                            var now = new Date();
                            var daysOfYear = [];
                            for (var d = MaxDate ; d <= now ; d.setDate(d.getDate() + 1)) {
                                
                                console.log(d);
                                var WMSCService = new Object();
                                WMSCService.sDamID = this['SIrrg_Code'];
                                WMSCService.sDate = new Date(d).toJSON().slice(0, 10);
                                var DTO = { 'wmscsv': WMSCService };

                                $.ajax({
                                    type: "POST",
                                    //url: "http://app.rid.go.th/Service.svc/getDamDataAuto",
                                    url: "http://app.rid.go.th/Service.svc/getDamData",
                                    //url: "http://app.rid.go.th/Service.svc/getDamDataInfo",
                                    contentType: "application/json; charset=utf-8",
                                    dataType: "json",
                                    data: JSON.stringify(DTO),
                                    crossDomain: true,
                                    async: false,
                                    success: function (dm) {
                                        if (dm.d) {
                                            if (dm.d.length > 0) {
                                                $.each(dm.d, function () {

                                                    var DateString = dm.d[0].DateString;
                                                    var num = parseInt((dm.d[0].Date.replace("+0700", "")).replace(/[^0-9]/g, ""));
                                                    var CurrDate = new Date(num).toJSON().slice(0, 10);
                                                    var Q = dm.d[0].Q;
                                                    var Inflow = dm.d[0].Inflow;
                                                    var Outflow = dm.d[0].Outflow;
                                                    var DamInfo = dm.d[0].DamInfo;
                                                    var DUL_Useless = DamInfo['QUseless'];

                                                    var param = new Object();
                                                    param.p_SIrrg_Id = DamID;
                                                    param.p_Date = CurrDate;
                                                    param.p_Qty = Q;
                                                    param.p_QtyPercent = ((Q + DUL_Useless) * 100) / StoreQ;
                                                    param.p_FlowInQty = Inflow;
                                                    param.p_FlowOutQty = Outflow;
                                                    param.p_UselessQty = DUL_Useless;
                                                    console.log(param)

                                                    $.ajax({
                                                        type: "POST",
                                                        url: "KeepDataWater.aspx/KeepBigIrrWater",
                                                        data: JSON.stringify(param),
                                                        contentType: "application/json; charset=utf-8",
                                                        dataType: "json",
                                                        success: function (msg) {
                                                            console.log(msg.d[0].msg);
                                                        }
                                                    });
                                                });
                                            }//lenght>0
                                            else {
                                                console.log("no data");
                                            }
                                        }//if
                                        else {
                                            console.log("no data");
                                        }
                                    },//success
                                    error: function (xhr, status, error) {
                                        console.log("An error has occurred during processing: " + error);
                                    }
                                });

                            }
                            
                        });
                    }
                });
            }

            window.onload = getWaterFlow;

        </script>
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        </telerik:RadAjaxManager>
        <div>
        </div>
    </form>
</body>
</html>
