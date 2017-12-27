<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Suggestion.aspx.vb" Inherits="PlantPlan.Web.Suggestion" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="content-type" content="text/html; charset=UTF-8" />
    <link href="../Styles/bootstrap.min.css" rel="stylesheet" />
    <style>
        html, body, #map {
            height: 550px;
            padding: 0;
            margin: 5px;
        }
    </style>
    <link rel="stylesheet" href="http://libs.cartocdn.com/cartodb.js/v3/3.15/themes/css/cartodb.css" />
    <script src="https://maps.googleapis.com/maps/api/js?key=AIzaSyD-LwBO6bcKfBm4E4mdATi39HdbaTfFcSk" type="text/javascript"></script>
    <script src="http://libs.cartocdn.com/cartodb.js/v3/3.15/cartodb.js"></script>

    <link rel="stylesheet" href="../Styles/mb.slider.css" media="all" type="text/css" />
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/2.1.3/jquery.js"></script>
    <script type="text/javascript" src="../scripts/jquery.mb.slider.js"></script>
    <script type="text/javascript" src="../scripts/formatDate.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
        <telerik:RadScriptBlock ID="sb" runat="server">
            <script type="text/javascript">
                var map;

                function initMap() {
                    GGM = new Object(google.maps);
                    geocoder = new GGM.Geocoder();

                    if (document.getElementById('txtlat').value == 0) {
                        var AddressSearch = $("#namePlace").val();
                        if (geocoder) {
                            geocoder.geocode({
                                address: AddressSearch
                            }, function (results, status) {
                                if (status == GGM.GeocoderStatus.OK) {
                                    var my_Point = results[0].geometry.location;
                                    map.setCenter(my_Point);
                                    my_Marker.setMap(map);
                                    my_Marker.setPosition(my_Point);
                                    $("#lat_value").val(my_Point.lat());
                                    $("#lon_value").val(my_Point.lng());

                                } else {
                                    alert("Not Found !: " + status);
                                    $("#namePlace").val("");
                                }
                            });
                        }
                    }

                    var my_Latlng = new GGM.LatLng(document.getElementById('lat_value').value, document.getElementById('lon_value').value);
                    var my_mapTypeId = GGM.MapTypeId.ROADMAP;
                    var my_DivObj = $("#map")[0];
                    var myOptions = {
                        zoom: 15,
                        center: my_Latlng,
                        mapTypeId: my_mapTypeId
                    };
                    map = new GGM.Map(my_DivObj, myOptions);
                    var image = '<%: ResolveUrl("~/Images/Pin/GreenPin.png") %>';
                    my_Marker = new GGM.Marker({
                        position: my_Latlng,
                        map: map,
                        icon: image,
                        draggable: true,

                    });

                    GGM.event.addListener(my_Marker, 'dragend', function () {
                        var my_Point = my_Marker.getPosition();
                        map.panTo(my_Point);
                        $("#lat_value").val(my_Point.lat());
                        $("#lon_value").val(my_Point.lng());
                    });

                    callCarto();
                }


                function callCarto() {
                    var style = [
                        '#world_borders{',
                        ' polygon-fill: #1a9850;',
                        ' polygon-opacity: 0.8;',
                        ' line-color: #FFF;',
                        ' line-width: 0.5;',
                        ' line-opacity: 1;',
                        '}'
                    ].join('\n')

                    var url = document.getElementById('url').value;
                    var criteria = document.getElementById('criteria').value;

                    //WHERE ST_Contains(the_geom, ST_GeomFromText('POINT(99.920654296875 14.31628419997645)', 4326))
                    var subLayerOptions0 = {
                        sql: "SELECT * FROM rice " + criteria,
                        cartocss: '#populated_places_test{ polygon-fill: #FFFF99; polygon-opacity: 0.5;}',
                        interactivity: 'suit_info'
                    }

                    var subLayerOptions1 = {
                        sql: "SELECT * FROM corn " + criteria,
                        cartocss: '#populated_places_test{ polygon-fill: #FFCC99; polygon-opacity: 0.5;}',
                        interactivity: 'suit_info'
                    }

                    var subLayerOptions2 = {
                        sql: "SELECT * FROM cane " + criteria,
                        cartocss: '#populated_places_test{ polygon-fill: #99FF99; polygon-opacity: 0.5;}',
                        interactivity: 'suit_info'
                    }

                    var subLayerOptions3 = {
                        sql: "SELECT * FROM cassava " + criteria,
                        cartocss: '#populated_places_test{ polygon-fill: #CC99FF; polygon-opacity: 0.5;}',
                        interactivity: 'suit_info'
                    }

                    cartodb.createLayer(map, url)
                        .addTo(map)
                        .on('done', function (layer) {
                            layer.getSubLayer(0).set(subLayerOptions0);
                            layer.getSubLayer(1).set(subLayerOptions1);
                            layer.getSubLayer(2).set(subLayerOptions2);
                            layer.getSubLayer(3).set(subLayerOptions3);
                            console.log(layer.getSubLayer(0))
                        });
                }

                window.onload = initMap;

            </script>

            <script type="text/javascript">
                function RiceCostChanged(sender, args) {
                    $get("<%= hdRiceCost.ClientID %>").value = args.get_newValue();
                }

                function CornCostChanged(sender, args) {
                    $get("<%= hdCornCost.ClientID %>").value = args.get_newValue();
                }

                function CaneCostChanged(sender, args) {
                    $get("<%= hdCaneCost.ClientID %>").value = args.get_newValue();
                }

                function CassavaCostChanged(sender, args) {
                    $get("<%= hdCassavaCost.ClientID %>").value = args.get_newValue();
                }

                function RiceYieldChanged(sender, args) {
                    $get("<%= hdRiceYield.ClientID %>").value = args.get_newValue();
                }

                function CornYieldChanged(sender, args) {
                    $get("<%= hdCornYield.ClientID %>").value = args.get_newValue();
                }

                function CaneYieldChanged(sender, args) {
                    $get("<%= hdCaneYield.ClientID %>").value = args.get_newValue();
                }

                function CassavaYieldChanged(sender, args) {
                    $get("<%= hdCassavaYield.ClientID %>").value = args.get_newValue();
                }

                function RiceCropChanged(sender, args) {
                    $get("<%= hdRiceCrop.ClientID %>").value = args.get_newValue();
                }

                function CornCropChanged(sender, args) {
                    $get("<%= hdCornCrop.ClientID %>").value = args.get_newValue();
                }

                function CaneCropChanged(sender, args) {
                    $get("<%= hdCaneCrop.ClientID %>").value = args.get_newValue();
                }

                function CassavaCropChanged(sender, args) {
                    $get("<%= hdCassavaCrop.ClientID %>").value = args.get_newValue();
                }
            </script>

            <script type="text/javascript">
                function CallWeight() {
                    $("#dvSuit .mb_slider").mbSlider({
                        onSlide: function (o) { $("#" + o.id + "_val").find(".val").val($(o).mbgetVal()); },
                        onSlideLoad: function (o) {
                            $(o).mbsetVal($get("<%= hdSuitWeight.ClientID %>").value);
                            $("#" + o.id + "_val").find(".val").val($(o).mbgetVal());
                        },
                        onStop: function (o) {
                            $get("<%= hdSuitWeight.ClientID %>").value = $(o).mbgetVal();
                        }
                    });

                    $("#dvWater .mb_slider").mbSlider({
                        onSlide: function (o) { $("#" + o.id + "_val").find(".val").val($(o).mbgetVal()); },
                        onSlideLoad: function (o) {
                            $(o).mbsetVal($get("<%= hdWaterWeight.ClientID %>").value);
                            $("#" + o.id + "_val").find(".val").val($(o).mbgetVal());
                        },
                        onStop: function (o) {
                            var cb = document.getElementById('cbWater');
                            if (cb.checked == true) {
                                $get("<%= hdWaterWeight.ClientID %>").value = $(o).mbgetVal();
                            }
                            else {
                                $get("<%= hdWaterWeight.ClientID %>").value = 1;
                                $(o).mbsetVal(1);
                                $("#" + o.id + "_val").find(".val").val($(o).mbgetVal());
                            }
                        }
                    });

                    $("#dvProfit .mb_slider").mbSlider({
                        onSlide: function (o) { $("#" + o.id + "_val").find(".val").val($(o).mbgetVal()); },
                        onSlideLoad: function (o) {
                            $(o).mbsetVal($get("<%= hdProfitWeight.ClientID %>").value);
                            $("#" + o.id + "_val").find(".val").val($(o).mbgetVal());
                        },
                        onStop: function (o) {
                            $get("<%= hdProfitWeight.ClientID %>").value = $(o).mbgetVal();
                        }
                    });


                    $("#dvDemand .mb_slider").mbSlider({
                        onSlide: function (o) { $("#" + o.id + "_val").find(".val").val($(o).mbgetVal()); },
                        onSlideLoad: function (o) {
                            $(o).mbsetVal($get("<%= hdDemandWeight.ClientID %>").value);
                            $("#" + o.id + "_val").find(".val").val($(o).mbgetVal());
                        },
                        onStop: function (o) {
                            $get("<%= hdDemandWeight.ClientID %>").value = $(o).mbgetVal();
                        }
                    });
                }
            </script>

            <script type="text/javascript">

</script>
            <style type="text/css">
                .setVal {
                    padding: 5px;
                    position: relative;
                    top: -5px;
                    margin: 3px;
                    width: 20px;
                    color: #AAA;
                }

                    .setVal input {
                        font: 18px/14px Arial, sans-serif;
                        border: 2px solid #ccc;
                        background: none;
                        padding: 5px;
                        -moz-border-radius: 4px;
                        -webkit-border-radius: 4px;
                    }
            </style>
        </telerik:RadScriptBlock>
        <div class="menu-masterPage">
            <telerik:RadMenu ID="mnuMain" runat="server" Skin="Glow" Width="100%" ClickToOpen="True"
                EnableRootItemScroll="True">
                <DefaultGroupSettings RepeatDirection="Vertical" RepeatColumns="2" />
            </telerik:RadMenu>
        </div>
        <asp:UpdateProgress ID="updateProgress" runat="server">
            <ProgressTemplate>
                <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999; background-color: #000000; opacity: 0.2;">
                </div>
                <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="../Images/Loading.gif" AlternateText="Loading ..." ToolTip="Loading ..." Style="padding: 10px; position: fixed; top: 45%; left: 45%; opacity: 0.5;" />
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:UpdatePanel ID="upnHead" runat="server">
            <ContentTemplate>
                <script type="text/javascript">
                    Sys.Application.add_load(initMap);
                    Sys.Application.add_load(CallWeight);
                </script>
                <div id="dvHead" runat="server" class="styleEditablePanel">
                    <div class="col-xs-12">
                        <div class="col-sm-5 col-xs-12">
                            <div class="col-xs-12">
                                <div class="col-sm-4 col-xs-12">
                                    จังหวัด :
                                    <telerik:RadComboBox ID="ddProvince" ClientIDMode="Static" runat="server" Width="100%" MarkFirstMatch="true" AutoPostBack="true"></telerik:RadComboBox>
                                </div>
                                <div class="col-sm-4 col-xs-12">
                                    อำเภอ :
                                    <telerik:RadComboBox ID="ddAmphoe" ClientIDMode="Static" runat="server" Width="100%" MarkFirstMatch="true" AutoPostBack="true"></telerik:RadComboBox>
                                </div>
                                <div class="col-sm-4 col-xs-12">
                                    ตำบล :
                                    <telerik:RadComboBox ID="ddTambon" ClientIDMode="Static" runat="server" Width="100%" MarkFirstMatch="true" AutoPostBack="true"></telerik:RadComboBox>
                                </div>
                                <asp:HiddenField ID="namePlace" runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="url" runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="criteria" runat="server"></asp:HiddenField>
                            </div>
                            <div class="col-xs-12">
                                <div id="map"></div>
                            </div>
                            <div class="col-xs-12">
                                <div class="col-sm-2 col-xs-12" style="align-content: center">
                                    <asp:Label ID="lbllatitude" runat="server" Text="latitude"></asp:Label><span>:</span>
                                </div>
                                <div class="col-sm-4 col-xs-12" style="align-content: center">
                                    <asp:HiddenField ID="txtlat" runat="server" ClientIDMode="Static"></asp:HiddenField>
                                    <input name="lat_value" type="text" id="lat_value" runat="server" style="width: 100%" value="0" readonly="readonly" />
                                </div>
                                <div class="col-sm-2 col-xs-12" style="align-content: center">
                                    <asp:Label ID="lblongtitude" runat="server" Text="longtitude"></asp:Label><span>:</span>
                                </div>
                                <div class="col-sm-4 col-xs-12" style="align-content: center">
                                    <asp:HiddenField ID="txtlon" runat="server"></asp:HiddenField>
                                    <input name="lon_value" type="text" id="lon_value" runat="server" style="width: 100%" value="0" readonly="readonly" />
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-7 col-xs-12">
                            <div class="col-xs-12">
                                <div class="col-xs-12" style="font-size: large; background-color: aliceblue; text-align: center;">
                                    ข้อมูลพื้นที่
                                </div>
                                <div class="col-xs-12" style="padding-right: 10px; padding-left: 10px">
                                    <div class="col-sm-2 col-xs-12" style="padding-right: 10px; padding-left: 10px">
                                        ลักษณะพื้นที่:
                                    </div>
                                    <div class="col-sm-10 col-xs-12" style="padding-right: 10px; padding-left: 10px">
                                        <asp:Label ID="lbarea_desc" runat="server"></asp:Label>
                                    </div>
                                </div>
                                <div class="col-xs-12" style="padding-right: 10px; padding-left: 10px">
                                    <div class="col-sm-2 col-xs-12" style="padding-right: 10px; padding-left: 10px">
                                        ประเภทดิน:
                                    </div>
                                    <div class="col-sm-4 col-xs-12" style="padding-right: 10px; padding-left: 10px">
                                        <asp:Label ID="lbsoil_desc" runat="server"></asp:Label>
                                    </div>
                                    <div class="col-sm-2 col-xs-12" style="padding-right: 10px; padding-left: 10px">
                                        การระบายน้ำ:
                                    </div>
                                    <div class="col-sm-4 col-xs-12" style="padding-right: 10px; padding-left: 10px">
                                        <asp:Label ID="lbdrainage" runat="server"></asp:Label>
                                    </div>
                                </div>
                                <div class="col-xs-12" style="padding-right: 10px; padding-left: 10px">
                                    <div class="col-sm-2 col-xs-12" style="padding-right: 10px; padding-left: 10px">
                                        ข้อจำกัด:
                                    </div>
                                    <div class="col-sm-4 col-xs-12" style="padding-right: 10px; padding-left: 10px">
                                        <asp:Label ID="lblimitation" runat="server"></asp:Label>
                                    </div>
                                    <div class="col-sm-2 col-xs-12" style="padding-right: 10px; padding-left: 10px">
                                        ความลาดชัน:
                                    </div>
                                    <div class="col-sm-4 col-xs-12" style="padding-right: 10px; padding-left: 10px">
                                        <asp:Label ID="lbslope" runat="server"></asp:Label>
                                    </div>
                                </div>
                                <div class="col-xs-12" style="padding-right: 10px; padding-left: 10px">
                                    <div class="col-sm-2 col-xs-12" style="padding-right: 10px; padding-left: 10px">
                                        แหล่งน้ำ:
                                    </div>
                                    <div class="col-sm-10 col-xs-12" style="padding-right: 10px; padding-left: 10px">
                                        <asp:LinkButton runat="server" Visible="false" PostBackUrl="#dialog" ID="lbt1"></asp:LinkButton>
                                        <asp:LinkButton runat="server" Visible="false" PostBackUrl="#dialog" ID="lbt2"></asp:LinkButton>
                                        <asp:LinkButton runat="server" Visible="false" PostBackUrl="#dialog" ID="lbt3"></asp:LinkButton>
                                        <asp:LinkButton runat="server" Visible="false" PostBackUrl="#dialog" ID="lbt4"></asp:LinkButton>
                                        <asp:LinkButton runat="server" Visible="false" PostBackUrl="#dialog" ID="lbt5"></asp:LinkButton>
                                        <asp:LinkButton runat="server" Visible="false" PostBackUrl="#dialog" ID="lbt6"></asp:LinkButton>
                                        <asp:LinkButton runat="server" Visible="false" PostBackUrl="#dialog" ID="lbt7"></asp:LinkButton>
                                        <asp:LinkButton runat="server" Visible="false" PostBackUrl="#dialog" ID="lbt8"></asp:LinkButton>
                                        <asp:LinkButton runat="server" Visible="false" PostBackUrl="#dialog" ID="lbt9"></asp:LinkButton>
                                        <asp:LinkButton runat="server" Visible="false" PostBackUrl="#dialog" ID="lbt10"></asp:LinkButton>
                                    </div>
                                </div>
                                <div class="col-xs-12" style="padding-right: 10px; padding-left: 10px">
                                    <div class="col-sm-2 col-xs-12" style="padding-right: 10px; padding-left: 10px;">
                                        พื้นที่เพาะปลูก:
                                    </div>
                                    <div class="col-sm-4 col-xs-12" style="padding-right: 10px; padding-left: 10px;">
                                        <telerik:RadNumericTextBox ID="txtCrop" runat="server" Width="50%" NumberFormat-DecimalDigits="2"></telerik:RadNumericTextBox>
                                        &nbsp;(ไร่)
                                        <span style="color: red;">*</span>
                                        <asp:RequiredFieldValidator ID="rqtxtCrop" runat="server" Display="Dynamic" ControlToValidate="txtCrop" ErrorMessage="โปรดใส่จำนวนพื้นที่" ForeColor="Red" ValidationGroup="View"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-sm-2 col-xs-12" style="padding-right: 10px; padding-left: 10px;">
                                        วันเริ่มเพาะปลูก:
                                    </div>
                                    <div class="col-sm-4 col-xs-12" style="padding-right: 10px; padding-left: 10px;">
                                        <telerik:RadDatePicker ID="dpStart" runat="server" Width="80%" DateInput-DateFormat="dd/MM/yyyy" DateInput-DisplayDateFormat="dd/MM/yyyy"></telerik:RadDatePicker>
                                        <span style="color: red;">*</span>
                                        <asp:RequiredFieldValidator ID="rpdpStart" runat="server" Display="Dynamic" ControlToValidate="dpStart" ErrorMessage="โปรดเลือกวันเพาะปลูก" ForeColor="Red" ValidationGroup="View"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="col-xs-12">
                                    <telerik:RadPanelBar RenderMode="Lightweight" runat="server" ID="pbAdvance" Width="100%">
                                        <Items>
                                            <telerik:RadPanelItem Text="ค่าน้ำหนัก" runat="server">
                                                <Items>
                                                    <telerik:RadPanelItem Value="Weight" runat="server">
                                                        <ItemTemplate>
                                                            <div class="col-xs-12">
                                                                <div class="col-sm-3 col-xs-12" style="padding-right: 10px; padding-left: 10px; height: 50px; line-height: 50px;">
                                                                    ความต้องการผลผลิต
                                                                </div>
                                                                <div class="col-sm-9 col-xs-12" style="padding-right: 10px; padding-left: 10px; text-align: center;">
                                                                    <div id="dvDemand" style="">
                                                                        <div id="sl4" class="mb_slider" data-property="{rangeColor:'green',negativeColor:'#ffcc00', startAt:1, grid:1, minVal:1, maxVal:10}"></div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-xs-12">
                                                                <div class="col-sm-3 col-xs-12" style="padding-right: 10px; padding-left: 10px; height: 50px; line-height: 50px;">
                                                                    ผลกำไร
                                                                </div>
                                                                <div class="col-sm-9 col-xs-12" style="padding-right: 10px; padding-left: 10px; text-align: center;">
                                                                    <div id="dvProfit" style="">
                                                                        <div id="sl3" class="mb_slider" data-property="{rangeColor:'orange',negativeColor:'#ffcc00', startAt:1, grid:1, minVal:1, maxVal:10}"></div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-xs-12">
                                                                <div class="col-sm-3 col-xs-12" style="padding-right: 10px; padding-left: 10px; height: 50px; line-height: 50px;">
                                                                    ความเหมาะสมของพื้นที่
                                                                </div>
                                                                <div class="col-sm-9 col-xs-12" style="padding-right: 10px; padding-left: 10px; text-align: center;">
                                                                    <div id="dvSuit" style="">
                                                                        <div id="sl1" class="mb_slider" data-property="{rangeColor:'brown',negativeColor:'#ffcc00', startAt:1, grid:1, minVal:1, maxVal:10}"></div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-xs-12">
                                                                <div class="col-sm-3 col-xs-12" style="padding-right: 10px; padding-left: 10px; height: 50px; line-height: 50px;">
                                                                    ปริมาณน้ำเพียงพอ
                                                                    <asp:CheckBox ID="cbWater" runat="server" Checked="true" ClientIDMode="Static" AutoPostBack="true" OnCheckedChanged="cbWater_CheckedChanged"></asp:CheckBox>
                                                                </div>
                                                                <div class="col-sm-9 col-xs-12" style="padding-right: 10px; padding-left: 10px; text-align: center;">
                                                                    <div id="dvWater" style="">
                                                                        <div id="sl2" class="mb_slider" data-property="{rangeColor:'blue',negativeColor:'#ffcc00', startAt:1, grid:1, minVal:1, maxVal:10}"></div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </ItemTemplate>
                                                    </telerik:RadPanelItem>
                                                </Items>
                                            </telerik:RadPanelItem>
                                            <telerik:RadPanelItem Text="ตั้งค่าขั้นสูง" runat="server">
                                                <Items>
                                                    <telerik:RadPanelItem Value="Advance" runat="server">
                                                        <ItemTemplate>
                                                            <div class="col-xs-12">
                                                                <div class="col-sm-4 col-xs-12" style="padding-right: 10px; padding-left: 10px;">
                                                                    &nbsp;
                                                                </div>
                                                                <div class="col-sm-2 col-xs-12" style="padding-right: 10px; padding-left: 10px; text-align: center;">
                                                                    ข้าว
                                                                </div>
                                                                <div class="col-sm-2 col-xs-12" style="padding-right: 10px; padding-left: 10px; text-align: center;">
                                                                    ข้าวโพด 
                                                                </div>
                                                                <div class="col-sm-2 col-xs-12" style="padding-right: 10px; padding-left: 10px; text-align: center;">
                                                                    อ้อย
                                                                </div>
                                                                <div class="col-sm-2 col-xs-12" style="padding-right: 10px; padding-left: 10px; text-align: center;">
                                                                    มันสำปะหลัง 
                                                                </div>
                                                            </div>
                                                            <div class="col-xs-12">
                                                                <div class="col-sm-4 col-xs-12" style="padding-right: 10px; padding-left: 10px; height: 30px; line-height: 30px;">
                                                                    ต้นทุน: (ไร่)
                                                                </div>
                                                                <div class="col-sm-2 col-xs-12" style="padding-right: 10px; padding-left: 10px; text-align: center;">
                                                                    <telerik:RadNumericTextBox ID="txtRiceCost" runat="server" Width="90%" NumberFormat-DecimalDigits="2" ClientEvents-OnValueChanged="RiceCostChanged"></telerik:RadNumericTextBox>
                                                                </div>
                                                                <div class="col-sm-2 col-xs-12" style="padding-right: 10px; padding-left: 10px; text-align: center;">
                                                                    <telerik:RadNumericTextBox ID="txtCornCost" runat="server" Width="90%" NumberFormat-DecimalDigits="2" ClientEvents-OnValueChanged="CornCostChanged"></telerik:RadNumericTextBox>
                                                                </div>
                                                                <div class="col-sm-2 col-xs-12" style="padding-right: 10px; padding-left: 10px; text-align: center;">
                                                                    <telerik:RadNumericTextBox ID="txtCaneCost" runat="server" Width="90%" NumberFormat-DecimalDigits="2" ClientEvents-OnValueChanged="CaneCostChanged"></telerik:RadNumericTextBox>
                                                                </div>
                                                                <div class="col-sm-2 col-xs-12" style="padding-right: 10px; padding-left: 10px; text-align: center;">
                                                                    <telerik:RadNumericTextBox ID="txtCassavaCost" runat="server" Width="90%" NumberFormat-DecimalDigits="2" ClientEvents-OnValueChanged="CassavaCostChanged"></telerik:RadNumericTextBox>
                                                                </div>
                                                            </div>
                                                            <div class="col-xs-12">
                                                                <div class="col-sm-4 col-xs-12" style="padding-right: 10px; padding-left: 10px; height: 30px; line-height: 30px;">
                                                                    ผลผลิตที่เคยปลูกได้: (กก.:ไร่)
                                                                </div>
                                                                <div class="col-sm-2 col-xs-12" style="padding-right: 10px; padding-left: 10px; text-align: center;">
                                                                    <telerik:RadNumericTextBox ID="txtRiceYield" runat="server" Width="90%" NumberFormat-DecimalDigits="2" ClientEvents-OnValueChanged="RiceYieldChanged"></telerik:RadNumericTextBox>
                                                                </div>
                                                                <div class="col-sm-2 col-xs-12" style="padding-right: 10px; padding-left: 10px; text-align: center;">
                                                                    <telerik:RadNumericTextBox ID="txtCornYield" runat="server" Width="90%" NumberFormat-DecimalDigits="2" ClientEvents-OnValueChanged="CornYieldChanged"></telerik:RadNumericTextBox>
                                                                </div>
                                                                <div class="col-sm-2 col-xs-12" style="padding-right: 10px; padding-left: 10px; text-align: center;">
                                                                    <telerik:RadNumericTextBox ID="txtCaneYield" runat="server" Width="90%" NumberFormat-DecimalDigits="2" ClientEvents-OnValueChanged="CaneYieldChanged"></telerik:RadNumericTextBox>
                                                                </div>
                                                                <div class="col-sm-2 col-xs-12" style="padding-right: 10px; padding-left: 10px; text-align: center;">
                                                                    <telerik:RadNumericTextBox ID="txtCassavaYield" runat="server" Width="90%" NumberFormat-DecimalDigits="2" ClientEvents-OnValueChanged="CassavaYieldChanged"></telerik:RadNumericTextBox>
                                                                </div>
                                                            </div>
                                                            <div class="col-xs-12">
                                                                <div class="col-sm-4 col-xs-12" style="padding-right: 10px; padding-left: 10px; height: 30px; line-height: 30px;">
                                                                    จำนวนวันเพาะปลูก: (วัน)
                                                                </div>
                                                                <div class="col-sm-2 col-xs-12" style="padding-right: 10px; padding-left: 10px; text-align: center;">
                                                                    <telerik:RadNumericTextBox ID="txtRiceCrop" runat="server" Width="90%" NumberFormat-DecimalDigits="0" ClientEvents-OnValueChanged="RiceCropChanged"></telerik:RadNumericTextBox>
                                                                </div>
                                                                <div class="col-sm-2 col-xs-12" style="padding-right: 10px; padding-left: 10px; text-align: center;">
                                                                    <telerik:RadNumericTextBox ID="txtCornCrop" runat="server" Width="90%" NumberFormat-DecimalDigits="0" ClientEvents-OnValueChanged="CornCropChanged"></telerik:RadNumericTextBox>
                                                                </div>
                                                                <div class="col-sm-2 col-xs-12" style="padding-right: 10px; padding-left: 10px; text-align: center;">
                                                                    <telerik:RadNumericTextBox ID="txtCaneCrop" runat="server" Width="90%" NumberFormat-DecimalDigits="0" ClientEvents-OnValueChanged="CaneCropChanged"></telerik:RadNumericTextBox>
                                                                </div>
                                                                <div class="col-sm-2 col-xs-12" style="padding-right: 10px; padding-left: 10px; text-align: center;">
                                                                    <telerik:RadNumericTextBox ID="txtCassavaCrop" runat="server" Width="90%" NumberFormat-DecimalDigits="0" ClientEvents-OnValueChanged="CassavaCropChanged"></telerik:RadNumericTextBox>
                                                                </div>
                                                            </div>
                                                        </ItemTemplate>
                                                    </telerik:RadPanelItem>
                                                </Items>
                                            </telerik:RadPanelItem>
                                        </Items>
                                    </telerik:RadPanelBar>
                                    <asp:HiddenField ID="hdSuitWeight" runat="server" Value="1" />
                                    <asp:HiddenField ID="hdWaterWeight" runat="server" Value="1" />
                                    <asp:HiddenField ID="hdProfitWeight" runat="server" Value="1" />
                                    <asp:HiddenField ID="hdDemandWeight" runat="server" Value="1" />

                                    <asp:HiddenField ID="hdRiceCost" runat="server" />
                                    <asp:HiddenField ID="hdCornCost" runat="server" />
                                    <asp:HiddenField ID="hdCaneCost" runat="server" />
                                    <asp:HiddenField ID="hdCassavaCost" runat="server" />

                                    <asp:HiddenField ID="hdRiceYield" runat="server" />
                                    <asp:HiddenField ID="hdCornYield" runat="server" />
                                    <asp:HiddenField ID="hdCaneYield" runat="server" />
                                    <asp:HiddenField ID="hdCassavaYield" runat="server" />

                                    <asp:HiddenField ID="hdRiceCrop" runat="server" />
                                    <asp:HiddenField ID="hdCornCrop" runat="server" />
                                    <asp:HiddenField ID="hdCaneCrop" runat="server" />
                                    <asp:HiddenField ID="hdCassavaCrop" runat="server" />
                                </div>
                            </div>
                            <div class="col-xs-12" style="text-align: center;">
                                <telerik:RadButton ID="btnView" runat="server" Text="วิเคราะห์ผล" Width="100px" ValidationGroup="View"></telerik:RadButton>
                            </div>
                            <div class="col-xs-12">
                                <div class="col-xs-12">
                                    <div class="col-sm-4 col-xs-12" style="padding-right: 10px; padding-left: 10px">
                                        &nbsp;
                                    </div>
                                    <div class="col-sm-2 col-xs-12" style="padding-right: 10px; padding-left: 10px; text-align: center;">
                                        ข้าว
                                    </div>
                                    <div class="col-sm-2 col-xs-12" style="padding-right: 10px; padding-left: 10px; text-align: center;">
                                        ข้าวโพด
                                    </div>
                                    <div class="col-sm-2 col-xs-12" style="padding-right: 10px; padding-left: 10px; text-align: center;">
                                        อ้อย
                                    </div>
                                    <div class="col-sm-2 col-xs-12" style="padding-right: 10px; padding-left: 10px; text-align: center;">
                                        มันสำปะหลัง
                                    </div>
                                </div>
                                <div class="col-xs-12">
                                    <div class="col-sm-4 col-xs-12" style="padding-right: 10px; padding-left: 10px">
                                        ความเหมาะสมของพื้นที่
                                    </div>
                                    <div class="col-sm-2 col-xs-12" style="padding-right: 10px; padding-left: 10px; text-align: center;">
                                        <asp:Label ID="lbRiceSuit" runat="server"></asp:Label>
                                    </div>
                                    <div class="col-sm-2 col-xs-12" style="padding-right: 10px; padding-left: 10px; text-align: center;">
                                        <asp:Label ID="lbCornSuit" runat="server"></asp:Label>
                                    </div>
                                    <div class="col-sm-2 col-xs-12" style="padding-right: 10px; padding-left: 10px; text-align: center;">
                                        <asp:Label ID="lbCaneSuit" runat="server"></asp:Label>
                                    </div>
                                    <div class="col-sm-2 col-xs-12" style="padding-right: 10px; padding-left: 10px; text-align: center;">
                                        <asp:Label ID="lbCassavaSuit" runat="server"></asp:Label>
                                    </div>
                                </div>
                                <div class="col-xs-12">
                                    <div class="col-sm-4 col-xs-12" style="padding-right: 10px; padding-left: 10px">
                                        ปริมาณน้ำที่ใช้ในการเพาะปลูก
                                    </div>
                                    <div class="col-sm-2 col-xs-12" style="padding-right: 10px; padding-left: 10px; text-align: right;">
                                        <asp:Label ID="lbRiceWater" runat="server"></asp:Label>
                                    </div>
                                    <div class="col-sm-2 col-xs-12" style="padding-right: 10px; padding-left: 10px; text-align: right;">
                                        <asp:Label ID="lbCornWater" runat="server"></asp:Label>
                                    </div>
                                    <div class="col-sm-2 col-xs-12" style="padding-right: 10px; padding-left: 10px; text-align: right;">
                                        <asp:Label ID="lbCaneWater" runat="server"></asp:Label>
                                    </div>
                                    <div class="col-sm-2 col-xs-12" style="padding-right: 10px; padding-left: 10px; text-align: right;">
                                        <asp:Label ID="lbCassavaWater" runat="server"></asp:Label>
                                    </div>
                                </div>
                                <div class="col-xs-12">
                                    <div class="col-sm-4 col-xs-12" style="padding-right: 10px; padding-left: 10px">
                                        ระยะเวลาในการเพาะปลูก (วัน)
                                    </div>
                                    <div class="col-sm-2 col-xs-12" style="padding-right: 10px; padding-left: 10px; text-align: right;">
                                        <asp:Label ID="lbRiceCrop" runat="server"></asp:Label>
                                    </div>
                                    <div class="col-sm-2 col-xs-12" style="padding-right: 10px; padding-left: 10px; text-align: right;">
                                        <asp:Label ID="lbCornCrop" runat="server"></asp:Label>
                                    </div>
                                    <div class="col-sm-2 col-xs-12" style="padding-right: 10px; padding-left: 10px; text-align: right;">
                                        <asp:Label ID="lbCaneCrop" runat="server"></asp:Label>
                                    </div>
                                    <div class="col-sm-2 col-xs-12" style="padding-right: 10px; padding-left: 10px; text-align: right;">
                                        <asp:Label ID="lbCassavaCrop" runat="server"></asp:Label>
                                    </div>
                                </div>
                                <div class="col-xs-12">
                                    <div class="col-sm-4 col-xs-12" style="padding-right: 10px; padding-left: 10px">
                                        ความต้องการทั้งหมด (ล้านตัน)
                                    </div>
                                    <div class="col-sm-2 col-xs-12" style="padding-right: 10px; padding-left: 10px; text-align: right;">
                                        <asp:Label ID="lbRiceDemand" runat="server"></asp:Label>
                                    </div>
                                    <div class="col-sm-2 col-xs-12" style="padding-right: 10px; padding-left: 10px; text-align: right;">
                                        <asp:Label ID="lbCornDemand" runat="server"></asp:Label>
                                    </div>
                                    <div class="col-sm-2 col-xs-12" style="padding-right: 10px; padding-left: 10px; text-align: right;">
                                        <asp:Label ID="lbCaneDemand" runat="server"></asp:Label>
                                    </div>
                                    <div class="col-sm-2 col-xs-12" style="padding-right: 10px; padding-left: 10px; text-align: right;">
                                        <asp:Label ID="lbCassavaDemand" runat="server"></asp:Label>
                                    </div>
                                </div>
                                <div class="col-xs-12">
                                    <div class="col-sm-4 col-xs-12" style="padding-right: 10px; padding-left: 10px">
                                        ความต้องการคงเหลือ (ล้านตัน)
                                    </div>
                                    <div class="col-sm-2 col-xs-12" style="padding-right: 10px; padding-left: 10px; text-align: right;">
                                        <asp:Label ID="lbRiceRemain" runat="server"></asp:Label>
                                    </div>
                                    <div class="col-sm-2 col-xs-12" style="padding-right: 10px; padding-left: 10px; text-align: right;">
                                        <asp:Label ID="lbCornRemain" runat="server"></asp:Label>
                                    </div>
                                    <div class="col-sm-2 col-xs-12" style="padding-right: 10px; padding-left: 10px; text-align: right;">
                                        <asp:Label ID="lbCaneRemain" runat="server"></asp:Label>
                                    </div>
                                    <div class="col-sm-2 col-xs-12" style="padding-right: 10px; padding-left: 10px; text-align: right;">
                                        <asp:Label ID="lbCassavaRemain" runat="server"></asp:Label>
                                    </div>
                                </div>
                                <div class="col-xs-12">
                                    <div class="col-sm-4 col-xs-12" style="padding-right: 10px; padding-left: 10px">
                                        ปริมาณผลผลิตคาดหมาย (กก.:ไร่)
                                    </div>
                                    <div class="col-sm-2 col-xs-12" style="padding-right: 10px; padding-left: 10px; text-align: right;">
                                        <asp:Label ID="lbRiceYield" runat="server"></asp:Label>
                                    </div>
                                    <div class="col-sm-2 col-xs-12" style="padding-right: 10px; padding-left: 10px; text-align: right;">
                                        <asp:Label ID="lbCornYield" runat="server"></asp:Label>
                                    </div>
                                    <div class="col-sm-2 col-xs-12" style="padding-right: 10px; padding-left: 10px; text-align: right;">
                                        <asp:Label ID="lbCaneYield" runat="server"></asp:Label>
                                    </div>
                                    <div class="col-sm-2 col-xs-12" style="padding-right: 10px; padding-left: 10px; text-align: right;">
                                        <asp:Label ID="lbCassavaYield" runat="server"></asp:Label>
                                    </div>
                                </div>
                                <div class="col-xs-12">
                                    <div class="col-sm-4 col-xs-12" style="padding-right: 10px; padding-left: 10px">
                                        ต้นทุนในการเพาะปลูก (บาท:ไร่)
                                    </div>
                                    <div class="col-sm-2 col-xs-12" style="padding-right: 10px; padding-left: 10px; text-align: right;">
                                        <asp:Label ID="lbRiceCost" runat="server"></asp:Label>
                                    </div>
                                    <div class="col-sm-2 col-xs-12" style="padding-right: 10px; padding-left: 10px; text-align: right;">
                                        <asp:Label ID="lbCornCost" runat="server"></asp:Label>
                                    </div>
                                    <div class="col-sm-2 col-xs-12" style="padding-right: 10px; padding-left: 10px; text-align: right;">
                                        <asp:Label ID="lbCaneCost" runat="server"></asp:Label>
                                    </div>
                                    <div class="col-sm-2 col-xs-12" style="padding-right: 10px; padding-left: 10px; text-align: right;">
                                        <asp:Label ID="lbCassavaCost" runat="server"></asp:Label>
                                    </div>
                                </div>
                                <div class="col-xs-12">
                                    <div class="col-sm-4 col-xs-12" style="padding-right: 10px; padding-left: 10px">
                                        ราคาขายในอนาคต (บาท:ตัน)
                                    </div>
                                    <div class="col-sm-2 col-xs-12" style="padding-right: 10px; padding-left: 10px; text-align: right;">
                                        <asp:Label ID="lbRicePrice" runat="server"></asp:Label>
                                    </div>
                                    <div class="col-sm-2 col-xs-12" style="padding-right: 10px; padding-left: 10px; text-align: right;">
                                        <asp:Label ID="lbCornPrice" runat="server"></asp:Label>
                                    </div>
                                    <div class="col-sm-2 col-xs-12" style="padding-right: 10px; padding-left: 10px; text-align: right;">
                                        <asp:Label ID="lbCanePrice" runat="server"></asp:Label>
                                    </div>
                                    <div class="col-sm-2 col-xs-12" style="padding-right: 10px; padding-left: 10px; text-align: right;">
                                        <asp:Label ID="lbCassavaPrice" runat="server"></asp:Label>
                                    </div>
                                </div>
                                <div class="col-xs-12">
                                    <div class="col-sm-4 col-xs-12" style="padding-right: 10px; padding-left: 10px">
                                        ผลกำไร (บาท:เดือน)
                                    </div>
                                    <div class="col-sm-2 col-xs-12" style="padding-right: 10px; padding-left: 10px; text-align: right;">
                                        <asp:Label ID="lbRiceProfit" runat="server"></asp:Label>
                                    </div>
                                    <div class="col-sm-2 col-xs-12" style="padding-right: 10px; padding-left: 10px; text-align: right;">
                                        <asp:Label ID="lbCornProfit" runat="server"></asp:Label>
                                    </div>
                                    <div class="col-sm-2 col-xs-12" style="padding-right: 10px; padding-left: 10px; text-align: right;">
                                        <asp:Label ID="lbCaneProfit" runat="server"></asp:Label>
                                    </div>
                                    <div class="col-sm-2 col-xs-12" style="padding-right: 10px; padding-left: 10px; text-align: right;">
                                        <asp:Label ID="lbCassavaProfit" runat="server"></asp:Label>
                                    </div>
                                </div>
                                <div class="col-xs-12">
                                    <div class="col-sm-4 col-xs-12" style="padding-right: 10px; padding-left: 10px">
                                        คะแนน (%)
                                    </div>
                                    <div class="col-sm-2 col-xs-12" style="padding-right: 10px; padding-left: 10px; text-align: right;">
                                        <asp:Label ID="lbRiceScore" runat="server"></asp:Label>
                                    </div>
                                    <div class="col-sm-2 col-xs-12" style="padding-right: 10px; padding-left: 10px; text-align: right;">
                                        <asp:Label ID="lbCornScore" runat="server"></asp:Label>
                                    </div>
                                    <div class="col-sm-2 col-xs-12" style="padding-right: 10px; padding-left: 10px; text-align: right;">
                                        <asp:Label ID="lbCaneScore" runat="server"></asp:Label>
                                    </div>
                                    <div class="col-sm-2 col-xs-12" style="padding-right: 10px; padding-left: 10px; text-align: right;">
                                        <asp:Label ID="lbCassavaScore" runat="server"></asp:Label>
                                    </div>
                                </div>
                            </div>
                            <div class="col-xs-12">
                                <div class="col-xs-12">
                                    <div class="col-sm-5 col-xs-12" style="padding-right: 10px; padding-left: 10px">
                                        ชื่อเกษตรกร:
                                        <telerik:RadTextBox ID="txtFarmer" runat="server" Width="80%" MaxLength="500"></telerik:RadTextBox>
                                    </div>
                                    <div class="col-sm-5 col-xs-12" style="padding-right: 10px; padding-left: 10px">
                                        พืชที่เลือก:
                                        <telerik:RadComboBox ID="ddPlantGroup" runat="server" Width="100%" MarkFirstMatch="true"></telerik:RadComboBox>
                                    </div>
                                    <div class="col-sm-2 col-xs-12" style="padding-right: 10px; padding-left: 10px;">
                                        <telerik:RadButton ID="btnChoosed" runat="server" Text="บันทึก" Enabled="false" Height="50px"></telerik:RadButton>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div>
                    <div class="col-xs-12">
                        <div class="col-sm-6 col-xs-12" style="padding-right: 10px; padding-left: 10px">
                            <telerik:RadHtmlChart runat="server" ID="bc1" Width="550" Height="500" Transitions="true" Skin="Silk" Visible="false">
                                <ChartTitle Text="Revenue">
                                    <Appearance Align="Center" BackgroundColor="Transparent" Position="Top"></Appearance>
                                </ChartTitle>
                                <PlotArea>
                                    <Series>
                                        <telerik:BarSeries Name="ปริมาณเฉลี่ย" Stacked="false" Gap="1.5" Spacing="0.4">
                                            <Appearance>
                                                <FillStyle BackgroundColor="#c5d291"></FillStyle>
                                            </Appearance>
                                            <LabelsAppearance DataFormatString="#,###.##" Position="Center">
                                            </LabelsAppearance>
                                            <TooltipsAppearance BackgroundColor="#c5d291" DataFormatString="#,###.##" Color="White"></TooltipsAppearance>
                                            <SeriesItems>
                                            </SeriesItems>
                                        </telerik:BarSeries>
                                        <telerik:BarSeries Name="ปริมาณเฉลี่ย(%)">
                                            <Appearance>
                                                <FillStyle BackgroundColor="#92b622"></FillStyle>
                                            </Appearance>
                                            <LabelsAppearance DataFormatString="{0}%" Position="Center"></LabelsAppearance>
                                            <TooltipsAppearance BackgroundColor="#92b622" DataFormatString="{0}%" Color="White"></TooltipsAppearance>
                                            <SeriesItems>
                                            </SeriesItems>
                                        </telerik:BarSeries>
                                    </Series>
                                    <Appearance>
                                        <FillStyle BackgroundColor="Transparent"></FillStyle>
                                    </Appearance>
                                    <XAxis AxisCrossingValue="0" Color="black" MajorTickType="Outside" MinorTickType="Outside"
                                        Reversed="false">
                                        <Items>
                                        </Items>
                                        <LabelsAppearance DataFormatString="{0}" RotationAngle="0" Skip="0" Step="1"></LabelsAppearance>
                                        <TitleAppearance Position="Center" RotationAngle="0" Text="ปี"></TitleAppearance>
                                    </XAxis>
                                    <YAxis AxisCrossingValue="0" Color="black" MajorTickSize="1" MajorTickType="Outside" MinorTickType="None" Reversed="false">
                                        <LabelsAppearance DataFormatString="{0}" RotationAngle="0" Skip="0" Step="1"></LabelsAppearance>
                                        <TitleAppearance Position="Center" RotationAngle="0" Text="ล้าน ลบ.ม."></TitleAppearance>
                                    </YAxis>
                                </PlotArea>
                                <Appearance>
                                    <FillStyle BackgroundColor="Transparent"></FillStyle>
                                </Appearance>
                                <Legend>
                                    <Appearance BackgroundColor="Transparent" Position="Bottom"></Appearance>
                                </Legend>
                            </telerik:RadHtmlChart>
                        </div>
                        <div class="col-sm-6 col-xs-12" style="padding-right: 10px; padding-left: 10px">
                            <telerik:RadHtmlChart runat="server" ID="bc2" Width="550" Height="500" Transitions="true" Skin="Silk" Visible="false">
                                <ChartTitle Text="Revenue">
                                    <Appearance Align="Center" BackgroundColor="Transparent" Position="Top"></Appearance>
                                </ChartTitle>
                                <PlotArea>
                                    <Series>
                                        <telerik:BarSeries Name="ปริมาณเฉลี่ย">
                                            <Appearance>
                                                <FillStyle BackgroundColor="#c5d291"></FillStyle>
                                            </Appearance>
                                            <LabelsAppearance DataFormatString="#,###.##" Position="Center">
                                            </LabelsAppearance>
                                            <TooltipsAppearance BackgroundColor="#c5d291" DataFormatString="#,###.##" Color="White"></TooltipsAppearance>
                                            <SeriesItems>
                                            </SeriesItems>
                                        </telerik:BarSeries>
                                        <telerik:BarSeries Name="ปริมาณเฉลี่ย(%)">
                                            <Appearance>
                                                <FillStyle BackgroundColor="#92b622"></FillStyle>
                                            </Appearance>
                                            <LabelsAppearance DataFormatString="{0}%" Position="Center"></LabelsAppearance>
                                            <TooltipsAppearance BackgroundColor="#92b622" DataFormatString="{0}%" Color="White"></TooltipsAppearance>
                                            <SeriesItems>
                                            </SeriesItems>
                                        </telerik:BarSeries>
                                    </Series>
                                    <Appearance>
                                        <FillStyle BackgroundColor="Transparent"></FillStyle>
                                    </Appearance>
                                    <XAxis AxisCrossingValue="0" Color="black" MajorTickType="Outside" MinorTickType="Outside"
                                        Reversed="false">
                                        <Items>
                                        </Items>
                                        <LabelsAppearance DataFormatString="{0}" RotationAngle="0" Skip="0" Step="1"></LabelsAppearance>
                                        <TitleAppearance Position="Center" RotationAngle="0" Text="ปี"></TitleAppearance>
                                    </XAxis>
                                    <YAxis AxisCrossingValue="0" Color="black" MajorTickSize="1" MajorTickType="Outside" MinorTickType="None" Reversed="false">
                                        <LabelsAppearance DataFormatString="{0}" RotationAngle="0" Skip="0" Step="1"></LabelsAppearance>
                                        <TitleAppearance Position="Center" RotationAngle="0" Text="ล้าน ลบ.ม."></TitleAppearance>
                                    </YAxis>
                                </PlotArea>
                                <Appearance>
                                    <FillStyle BackgroundColor="Transparent"></FillStyle>
                                </Appearance>
                                <Legend>
                                    <Appearance BackgroundColor="Transparent" Position="Bottom"></Appearance>
                                </Legend>
                            </telerik:RadHtmlChart>
                        </div>
                    </div>
                    <div class="col-xs-12">
                        <div class="col-sm-6 col-xs-12" style="padding-right: 10px; padding-left: 10px">
                            <telerik:RadHtmlChart runat="server" ID="bc3" Width="550" Height="500" Transitions="true" Skin="Silk" Visible="false">
                                <ChartTitle Text="Revenue">
                                    <Appearance Align="Center" BackgroundColor="Transparent" Position="Top"></Appearance>
                                </ChartTitle>
                                <PlotArea>
                                    <Series>
                                        <telerik:BarSeries Name="ปริมาณเฉลี่ย" Stacked="false" Gap="1.5" Spacing="0.4">
                                            <Appearance>
                                                <FillStyle BackgroundColor="#c5d291"></FillStyle>
                                            </Appearance>
                                            <LabelsAppearance DataFormatString="#,###.##" Position="Center">
                                            </LabelsAppearance>
                                            <TooltipsAppearance BackgroundColor="#c5d291" DataFormatString="#,###.##" Color="White"></TooltipsAppearance>
                                            <SeriesItems>
                                            </SeriesItems>
                                        </telerik:BarSeries>
                                        <telerik:BarSeries Name="ปริมาณเฉลี่ย(%)">
                                            <Appearance>
                                                <FillStyle BackgroundColor="#92b622"></FillStyle>
                                            </Appearance>
                                            <LabelsAppearance DataFormatString="{0}%" Position="Center"></LabelsAppearance>
                                            <TooltipsAppearance BackgroundColor="#92b622" DataFormatString="{0}%" Color="White"></TooltipsAppearance>
                                            <SeriesItems>
                                            </SeriesItems>
                                        </telerik:BarSeries>
                                    </Series>
                                    <Appearance>
                                        <FillStyle BackgroundColor="Transparent"></FillStyle>
                                    </Appearance>
                                    <XAxis AxisCrossingValue="0" Color="black" MajorTickType="Outside" MinorTickType="Outside"
                                        Reversed="false">
                                        <Items>
                                        </Items>
                                        <LabelsAppearance DataFormatString="{0}" RotationAngle="0" Skip="0" Step="1"></LabelsAppearance>
                                        <TitleAppearance Position="Center" RotationAngle="0" Text="ปี"></TitleAppearance>
                                    </XAxis>
                                    <YAxis AxisCrossingValue="0" Color="black" MajorTickSize="1" MajorTickType="Outside" MinorTickType="None" Reversed="false">
                                        <LabelsAppearance DataFormatString="{0}" RotationAngle="0" Skip="0" Step="1"></LabelsAppearance>
                                        <TitleAppearance Position="Center" RotationAngle="0" Text="ล้าน ลบ.ม."></TitleAppearance>
                                    </YAxis>
                                </PlotArea>
                                <Appearance>
                                    <FillStyle BackgroundColor="Transparent"></FillStyle>
                                </Appearance>
                                <Legend>
                                    <Appearance BackgroundColor="Transparent" Position="Bottom"></Appearance>
                                </Legend>
                            </telerik:RadHtmlChart>
                        </div>
                        <div class="col-sm-6 col-xs-12" style="padding-right: 10px; padding-left: 10px">
                            <telerik:RadHtmlChart runat="server" ID="bc4" Width="550" Height="500" Transitions="true" Skin="Silk" Visible="false">
                                <ChartTitle Text="Revenue">
                                    <Appearance Align="Center" BackgroundColor="Transparent" Position="Top"></Appearance>
                                </ChartTitle>
                                <PlotArea>
                                    <Series>
                                        <telerik:BarSeries Name="ปริมาณเฉลี่ย">
                                            <Appearance>
                                                <FillStyle BackgroundColor="#c5d291"></FillStyle>
                                            </Appearance>
                                            <LabelsAppearance DataFormatString="#,###.##" Position="Center">
                                            </LabelsAppearance>
                                            <TooltipsAppearance BackgroundColor="#c5d291" DataFormatString="#,###.##" Color="White"></TooltipsAppearance>
                                            <SeriesItems>
                                            </SeriesItems>
                                        </telerik:BarSeries>
                                        <telerik:BarSeries Name="ปริมาณเฉลี่ย(%)">
                                            <Appearance>
                                                <FillStyle BackgroundColor="#92b622"></FillStyle>
                                            </Appearance>
                                            <LabelsAppearance DataFormatString="{0}%" Position="Center"></LabelsAppearance>
                                            <TooltipsAppearance BackgroundColor="#92b622" DataFormatString="{0}%" Color="White"></TooltipsAppearance>
                                            <SeriesItems>
                                            </SeriesItems>
                                        </telerik:BarSeries>
                                    </Series>
                                    <Appearance>
                                        <FillStyle BackgroundColor="Transparent"></FillStyle>
                                    </Appearance>
                                    <XAxis AxisCrossingValue="0" Color="black" MajorTickType="Outside" MinorTickType="Outside"
                                        Reversed="false">
                                        <Items>
                                        </Items>
                                        <LabelsAppearance DataFormatString="{0}" RotationAngle="0" Skip="0" Step="1"></LabelsAppearance>
                                        <TitleAppearance Position="Center" RotationAngle="0" Text="ปี"></TitleAppearance>
                                    </XAxis>
                                    <YAxis AxisCrossingValue="0" Color="black" MajorTickSize="1" MajorTickType="Outside" MinorTickType="None" Reversed="false">
                                        <LabelsAppearance DataFormatString="{0}" RotationAngle="0" Skip="0" Step="1"></LabelsAppearance>
                                        <TitleAppearance Position="Center" RotationAngle="0" Text="ล้าน ลบ.ม."></TitleAppearance>
                                    </YAxis>
                                </PlotArea>
                                <Appearance>
                                    <FillStyle BackgroundColor="Transparent"></FillStyle>
                                </Appearance>
                                <Legend>
                                    <Appearance BackgroundColor="Transparent" Position="Bottom"></Appearance>
                                </Legend>
                            </telerik:RadHtmlChart>
                        </div>
                    </div>
                    <div class="col-xs-12">
                        <div class="col-sm-6 col-xs-12" style="padding-right: 10px; padding-left: 10px">
                            <telerik:RadHtmlChart runat="server" ID="bc5" Width="550" Height="500" Transitions="true" Skin="Silk" Visible="false">
                                <ChartTitle Text="Revenue">
                                    <Appearance Align="Center" BackgroundColor="Transparent" Position="Top"></Appearance>
                                </ChartTitle>
                                <PlotArea>
                                    <Series>
                                        <telerik:BarSeries Name="ปริมาณเฉลี่ย" Stacked="false" Gap="1.5" Spacing="0.4">
                                            <Appearance>
                                                <FillStyle BackgroundColor="#c5d291"></FillStyle>
                                            </Appearance>
                                            <LabelsAppearance DataFormatString="#,###.##" Position="Center">
                                            </LabelsAppearance>
                                            <TooltipsAppearance BackgroundColor="#c5d291" DataFormatString="#,###.##" Color="White"></TooltipsAppearance>
                                            <SeriesItems>
                                            </SeriesItems>
                                        </telerik:BarSeries>
                                        <telerik:BarSeries Name="ปริมาณเฉลี่ย(%)">
                                            <Appearance>
                                                <FillStyle BackgroundColor="#92b622"></FillStyle>
                                            </Appearance>
                                            <LabelsAppearance DataFormatString="{0}%" Position="Center"></LabelsAppearance>
                                            <TooltipsAppearance BackgroundColor="#92b622" DataFormatString="{0}%" Color="White"></TooltipsAppearance>
                                            <SeriesItems>
                                            </SeriesItems>
                                        </telerik:BarSeries>
                                    </Series>
                                    <Appearance>
                                        <FillStyle BackgroundColor="Transparent"></FillStyle>
                                    </Appearance>
                                    <XAxis AxisCrossingValue="0" Color="black" MajorTickType="Outside" MinorTickType="Outside"
                                        Reversed="false">
                                        <Items>
                                        </Items>
                                        <LabelsAppearance DataFormatString="{0}" RotationAngle="0" Skip="0" Step="1"></LabelsAppearance>
                                        <TitleAppearance Position="Center" RotationAngle="0" Text="ปี"></TitleAppearance>
                                    </XAxis>
                                    <YAxis AxisCrossingValue="0" Color="black" MajorTickSize="1" MajorTickType="Outside" MinorTickType="None" Reversed="false">
                                        <LabelsAppearance DataFormatString="{0}" RotationAngle="0" Skip="0" Step="1"></LabelsAppearance>
                                        <TitleAppearance Position="Center" RotationAngle="0" Text="ล้าน ลบ.ม."></TitleAppearance>
                                    </YAxis>
                                </PlotArea>
                                <Appearance>
                                    <FillStyle BackgroundColor="Transparent"></FillStyle>
                                </Appearance>
                                <Legend>
                                    <Appearance BackgroundColor="Transparent" Position="Bottom"></Appearance>
                                </Legend>
                            </telerik:RadHtmlChart>
                        </div>
                        <div class="col-sm-6 col-xs-12" style="padding-right: 10px; padding-left: 10px">
                            <telerik:RadHtmlChart runat="server" ID="bc6" Width="550" Height="500" Transitions="true" Skin="Silk" Visible="false">
                                <ChartTitle Text="Revenue">
                                    <Appearance Align="Center" BackgroundColor="Transparent" Position="Top"></Appearance>
                                </ChartTitle>
                                <PlotArea>
                                    <Series>
                                        <telerik:BarSeries Name="ปริมาณเฉลี่ย">
                                            <Appearance>
                                                <FillStyle BackgroundColor="#c5d291"></FillStyle>
                                            </Appearance>
                                            <LabelsAppearance DataFormatString="#,###.##" Position="Center">
                                            </LabelsAppearance>
                                            <TooltipsAppearance BackgroundColor="#c5d291" DataFormatString="#,###.##" Color="White"></TooltipsAppearance>
                                            <SeriesItems>
                                            </SeriesItems>
                                        </telerik:BarSeries>
                                        <telerik:BarSeries Name="ปริมาณเฉลี่ย(%)">
                                            <Appearance>
                                                <FillStyle BackgroundColor="#92b622"></FillStyle>
                                            </Appearance>
                                            <LabelsAppearance DataFormatString="{0}%" Position="Center"></LabelsAppearance>
                                            <TooltipsAppearance BackgroundColor="#92b622" DataFormatString="{0}%" Color="White"></TooltipsAppearance>
                                            <SeriesItems>
                                            </SeriesItems>
                                        </telerik:BarSeries>
                                    </Series>
                                    <Appearance>
                                        <FillStyle BackgroundColor="Transparent"></FillStyle>
                                    </Appearance>
                                    <XAxis AxisCrossingValue="0" Color="black" MajorTickType="Outside" MinorTickType="Outside"
                                        Reversed="false">
                                        <Items>
                                        </Items>
                                        <LabelsAppearance DataFormatString="{0}" RotationAngle="0" Skip="0" Step="1"></LabelsAppearance>
                                        <TitleAppearance Position="Center" RotationAngle="0" Text="ปี"></TitleAppearance>
                                    </XAxis>
                                    <YAxis AxisCrossingValue="0" Color="black" MajorTickSize="1" MajorTickType="Outside" MinorTickType="None" Reversed="false">
                                        <LabelsAppearance DataFormatString="{0}" RotationAngle="0" Skip="0" Step="1"></LabelsAppearance>
                                        <TitleAppearance Position="Center" RotationAngle="0" Text="ล้าน ลบ.ม."></TitleAppearance>
                                    </YAxis>
                                </PlotArea>
                                <Appearance>
                                    <FillStyle BackgroundColor="Transparent"></FillStyle>
                                </Appearance>
                                <Legend>
                                    <Appearance BackgroundColor="Transparent" Position="Bottom"></Appearance>
                                </Legend>
                            </telerik:RadHtmlChart>
                        </div>
                    </div>
                    <div class="col-xs-12">
                        <div class="col-sm-6 col-xs-12" style="padding-right: 10px; padding-left: 10px">
                            <telerik:RadHtmlChart runat="server" ID="bc7" Width="550" Height="500" Transitions="true" Skin="Silk" Visible="false">
                                <ChartTitle Text="Revenue">
                                    <Appearance Align="Center" BackgroundColor="Transparent" Position="Top"></Appearance>
                                </ChartTitle>
                                <PlotArea>
                                    <Series>
                                        <telerik:BarSeries Name="ปริมาณเฉลี่ย" Stacked="false" Gap="1.5" Spacing="0.4">
                                            <Appearance>
                                                <FillStyle BackgroundColor="#c5d291"></FillStyle>
                                            </Appearance>
                                            <LabelsAppearance DataFormatString="#,###.##" Position="Center">
                                            </LabelsAppearance>
                                            <TooltipsAppearance BackgroundColor="#c5d291" DataFormatString="#,###.##" Color="White"></TooltipsAppearance>
                                            <SeriesItems>
                                            </SeriesItems>
                                        </telerik:BarSeries>
                                        <telerik:BarSeries Name="ปริมาณเฉลี่ย(%)">
                                            <Appearance>
                                                <FillStyle BackgroundColor="#92b622"></FillStyle>
                                            </Appearance>
                                            <LabelsAppearance DataFormatString="{0}%" Position="Center"></LabelsAppearance>
                                            <TooltipsAppearance BackgroundColor="#92b622" DataFormatString="{0}%" Color="White"></TooltipsAppearance>
                                            <SeriesItems>
                                            </SeriesItems>
                                        </telerik:BarSeries>
                                    </Series>
                                    <Appearance>
                                        <FillStyle BackgroundColor="Transparent"></FillStyle>
                                    </Appearance>
                                    <XAxis AxisCrossingValue="0" Color="black" MajorTickType="Outside" MinorTickType="Outside"
                                        Reversed="false">
                                        <Items>
                                        </Items>
                                        <LabelsAppearance DataFormatString="{0}" RotationAngle="0" Skip="0" Step="1"></LabelsAppearance>
                                        <TitleAppearance Position="Center" RotationAngle="0" Text="ปี"></TitleAppearance>
                                    </XAxis>
                                    <YAxis AxisCrossingValue="0" Color="black" MajorTickSize="1" MajorTickType="Outside" MinorTickType="None" Reversed="false">
                                        <LabelsAppearance DataFormatString="{0}" RotationAngle="0" Skip="0" Step="1"></LabelsAppearance>
                                        <TitleAppearance Position="Center" RotationAngle="0" Text="ล้าน ลบ.ม."></TitleAppearance>
                                    </YAxis>
                                </PlotArea>
                                <Appearance>
                                    <FillStyle BackgroundColor="Transparent"></FillStyle>
                                </Appearance>
                                <Legend>
                                    <Appearance BackgroundColor="Transparent" Position="Bottom"></Appearance>
                                </Legend>
                            </telerik:RadHtmlChart>
                        </div>
                        <div class="col-sm-6 col-xs-12" style="padding-right: 10px; padding-left: 10px">
                            <telerik:RadHtmlChart runat="server" ID="bc8" Width="550" Height="500" Transitions="true" Skin="Silk" Visible="false">
                                <ChartTitle Text="Revenue">
                                    <Appearance Align="Center" BackgroundColor="Transparent" Position="Top"></Appearance>
                                </ChartTitle>
                                <PlotArea>
                                    <Series>
                                        <telerik:BarSeries Name="ปริมาณเฉลี่ย">
                                            <Appearance>
                                                <FillStyle BackgroundColor="#c5d291"></FillStyle>
                                            </Appearance>
                                            <LabelsAppearance DataFormatString="#,###.##" Position="Center">
                                            </LabelsAppearance>
                                            <TooltipsAppearance BackgroundColor="#c5d291" DataFormatString="#,###.##" Color="White"></TooltipsAppearance>
                                            <SeriesItems>
                                            </SeriesItems>
                                        </telerik:BarSeries>
                                        <telerik:BarSeries Name="ปริมาณเฉลี่ย(%)">
                                            <Appearance>
                                                <FillStyle BackgroundColor="#92b622"></FillStyle>
                                            </Appearance>
                                            <LabelsAppearance DataFormatString="{0}%" Position="Center"></LabelsAppearance>
                                            <TooltipsAppearance BackgroundColor="#92b622" DataFormatString="{0}%" Color="White"></TooltipsAppearance>
                                            <SeriesItems>
                                            </SeriesItems>
                                        </telerik:BarSeries>
                                    </Series>
                                    <Appearance>
                                        <FillStyle BackgroundColor="Transparent"></FillStyle>
                                    </Appearance>
                                    <XAxis AxisCrossingValue="0" Color="black" MajorTickType="Outside" MinorTickType="Outside"
                                        Reversed="false">
                                        <Items>
                                        </Items>
                                        <LabelsAppearance DataFormatString="{0}" RotationAngle="0" Skip="0" Step="1"></LabelsAppearance>
                                        <TitleAppearance Position="Center" RotationAngle="0" Text="ปี"></TitleAppearance>
                                    </XAxis>
                                    <YAxis AxisCrossingValue="0" Color="black" MajorTickSize="1" MajorTickType="Outside" MinorTickType="None" Reversed="false">
                                        <LabelsAppearance DataFormatString="{0}" RotationAngle="0" Skip="0" Step="1"></LabelsAppearance>
                                        <TitleAppearance Position="Center" RotationAngle="0" Text="ล้าน ลบ.ม."></TitleAppearance>
                                    </YAxis>
                                </PlotArea>
                                <Appearance>
                                    <FillStyle BackgroundColor="Transparent"></FillStyle>
                                </Appearance>
                                <Legend>
                                    <Appearance BackgroundColor="Transparent" Position="Bottom"></Appearance>
                                </Legend>
                            </telerik:RadHtmlChart>
                        </div>
                    </div>
                    <div class="col-xs-12">
                        <div class="col-sm-6 col-xs-12" style="padding-right: 10px; padding-left: 10px">
                            <telerik:RadHtmlChart runat="server" ID="bc9" Width="550" Height="500" Transitions="true" Skin="Silk" Visible="false">
                                <ChartTitle Text="Revenue">
                                    <Appearance Align="Center" BackgroundColor="Transparent" Position="Top"></Appearance>
                                </ChartTitle>
                                <PlotArea>
                                    <Series>
                                        <telerik:BarSeries Name="ปริมาณเฉลี่ย" Stacked="false" Gap="1.5" Spacing="0.4">
                                            <Appearance>
                                                <FillStyle BackgroundColor="#c5d291"></FillStyle>
                                            </Appearance>
                                            <LabelsAppearance DataFormatString="#,###.##" Position="Center">
                                            </LabelsAppearance>
                                            <TooltipsAppearance BackgroundColor="#c5d291" DataFormatString="#,###.##" Color="White"></TooltipsAppearance>
                                            <SeriesItems>
                                            </SeriesItems>
                                        </telerik:BarSeries>
                                        <telerik:BarSeries Name="ปริมาณเฉลี่ย(%)">
                                            <Appearance>
                                                <FillStyle BackgroundColor="#92b622"></FillStyle>
                                            </Appearance>
                                            <LabelsAppearance DataFormatString="{0}%" Position="Center"></LabelsAppearance>
                                            <TooltipsAppearance BackgroundColor="#92b622" DataFormatString="{0}%" Color="White"></TooltipsAppearance>
                                            <SeriesItems>
                                            </SeriesItems>
                                        </telerik:BarSeries>
                                    </Series>
                                    <Appearance>
                                        <FillStyle BackgroundColor="Transparent"></FillStyle>
                                    </Appearance>
                                    <XAxis AxisCrossingValue="0" Color="black" MajorTickType="Outside" MinorTickType="Outside"
                                        Reversed="false">
                                        <Items>
                                        </Items>
                                        <LabelsAppearance DataFormatString="{0}" RotationAngle="0" Skip="0" Step="1"></LabelsAppearance>
                                        <TitleAppearance Position="Center" RotationAngle="0" Text="ปี"></TitleAppearance>
                                    </XAxis>
                                    <YAxis AxisCrossingValue="0" Color="black" MajorTickSize="1" MajorTickType="Outside" MinorTickType="None" Reversed="false">
                                        <LabelsAppearance DataFormatString="{0}" RotationAngle="0" Skip="0" Step="1"></LabelsAppearance>
                                        <TitleAppearance Position="Center" RotationAngle="0" Text="ล้าน ลบ.ม."></TitleAppearance>
                                    </YAxis>
                                </PlotArea>
                                <Appearance>
                                    <FillStyle BackgroundColor="Transparent"></FillStyle>
                                </Appearance>
                                <Legend>
                                    <Appearance BackgroundColor="Transparent" Position="Bottom"></Appearance>
                                </Legend>
                            </telerik:RadHtmlChart>
                        </div>
                        <div class="col-sm-6 col-xs-12" style="padding-right: 10px; padding-left: 10px">
                            <telerik:RadHtmlChart runat="server" ID="bc10" Width="550" Height="500" Transitions="true" Skin="Silk" Visible="false">
                                <ChartTitle Text="Revenue">
                                    <Appearance Align="Center" BackgroundColor="Transparent" Position="Top"></Appearance>
                                </ChartTitle>
                                <PlotArea>
                                    <Series>
                                        <telerik:BarSeries Name="ปริมาณเฉลี่ย">
                                            <Appearance>
                                                <FillStyle BackgroundColor="#c5d291"></FillStyle>
                                            </Appearance>
                                            <LabelsAppearance DataFormatString="#,###.##" Position="Center">
                                            </LabelsAppearance>
                                            <TooltipsAppearance BackgroundColor="#c5d291" DataFormatString="#,###.##" Color="White"></TooltipsAppearance>
                                            <SeriesItems>
                                            </SeriesItems>
                                        </telerik:BarSeries>
                                        <telerik:BarSeries Name="ปริมาณเฉลี่ย(%)">
                                            <Appearance>
                                                <FillStyle BackgroundColor="#92b622"></FillStyle>
                                            </Appearance>
                                            <LabelsAppearance DataFormatString="{0}%" Position="Center"></LabelsAppearance>
                                            <TooltipsAppearance BackgroundColor="#92b622" DataFormatString="{0}%" Color="White"></TooltipsAppearance>
                                            <SeriesItems>
                                            </SeriesItems>
                                        </telerik:BarSeries>
                                    </Series>
                                    <Appearance>
                                        <FillStyle BackgroundColor="Transparent"></FillStyle>
                                    </Appearance>
                                    <XAxis AxisCrossingValue="0" Color="black" MajorTickType="Outside" MinorTickType="Outside"
                                        Reversed="false">
                                        <Items>
                                        </Items>
                                        <LabelsAppearance DataFormatString="{0}" RotationAngle="0" Skip="0" Step="1"></LabelsAppearance>
                                        <TitleAppearance Position="Center" RotationAngle="0" Text="ปี"></TitleAppearance>
                                    </XAxis>
                                    <YAxis AxisCrossingValue="0" Color="black" MajorTickSize="1" MajorTickType="Outside" MinorTickType="None" Reversed="false">
                                        <LabelsAppearance DataFormatString="{0}" RotationAngle="0" Skip="0" Step="1"></LabelsAppearance>
                                        <TitleAppearance Position="Center" RotationAngle="0" Text="ล้าน ลบ.ม."></TitleAppearance>
                                    </YAxis>
                                </PlotArea>
                                <Appearance>
                                    <FillStyle BackgroundColor="Transparent"></FillStyle>
                                </Appearance>
                                <Legend>
                                    <Appearance BackgroundColor="Transparent" Position="Bottom"></Appearance>
                                </Legend>
                            </telerik:RadHtmlChart>
                        </div>
                    </div>
                </div>
                <div id="dialog">
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
