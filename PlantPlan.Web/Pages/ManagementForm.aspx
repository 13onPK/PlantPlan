<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Masters/PageMaster.Master" CodeBehind="ManagementForm.aspx.vb" Inherits="PlantPlan.Web.ManagementForm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="dvMain" runat="server" style="margin-top: 50px;">
        <telerik:RadToolBar RenderMode="Lightweight" ID="tbMain" runat="server" Width="100%">
            <Items>
                <telerik:RadToolBarButton Text="Management" Font-Bold="true">
                </telerik:RadToolBarButton>
            </Items>
        </telerik:RadToolBar>
        <asp:UpdatePanel ID="upnMain" runat="server">
            <ContentTemplate>
                <div id="dvUpdateMain" runat="server" class="styleEditablePanel">
                    <div class="col-xs-12">
                        <div class="col-sm-6 col-xs-12">
                            <div class="col-xs-12">
                                <div class="col-sm-4 col-xs-12 styleLabel">
                                    <asp:Label ID="lbPlant" runat="server" Text="พืช"></asp:Label><span>:</span>
                                </div>
                                <div class="col-sm-8 col-xs-12">
                                    <telerik:RadComboBox ID="ddPlantGroup" runat="server" Width="100%" MarkFirstMatch="true" Enabled="false"></telerik:RadComboBox>
                                </div>
                            </div>
                            <div class="col-xs-12">
                                <div class="col-sm-4 col-xs-12 styleLabel">
                                    <asp:Label ID="lbProvince" runat="server" Text="จังหวัด"></asp:Label><span>:</span>
                                </div>
                                <div class="col-sm-8 col-xs-12">
                                    <telerik:RadComboBox ID="ddProvince" runat="server" Width="100%" MarkFirstMatch="true" Enabled="false"></telerik:RadComboBox>
                                </div>
                            </div>
                            <div class="col-xs-12">
                                <div class="col-sm-4 col-xs-12 styleLabel">
                                    <asp:Label ID="lbAmphoe" runat="server" Text="อำเภอ"></asp:Label><span>:</span>
                                </div>
                                <div class="col-sm-8 col-xs-12">
                                    <telerik:RadComboBox ID="ddAmphoe" runat="server" Width="100%" MarkFirstMatch="true" Enabled="false"></telerik:RadComboBox>
                                </div>
                            </div>
                            <div class="col-xs-12">
                                <div class="col-sm-4 col-xs-12 styleLabel">
                                    <asp:Label ID="lbTambon" runat="server" Text="ตำบล"></asp:Label><span>:</span>
                                </div>
                                <div class="col-sm-8 col-xs-12">
                                    <telerik:RadComboBox ID="ddTambon" runat="server" Width="100%" MarkFirstMatch="true" Enabled="false"></telerik:RadComboBox>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-6 col-xs-12">
                        </div>
                    </div>
                    <div class="col-xs-12">
                        <div class="col-sm-2 col-xs-12 styleLabel">
                            <asp:Label ID="lbFarmer" runat="server" Text="เกษตรกร"></asp:Label><span>:</span>
                        </div>
                        <div class="col-sm-4 col-xs-12">
                            <telerik:RadTextBox ID="txtFarmer" runat="server" Width="100%" ReadOnly="true"></telerik:RadTextBox>
                        </div>
                        <div class="col-sm-2 col-xs-12 styleLabel">
                            <asp:Label ID="lbArea" runat="server" Text="พื้นที่"></asp:Label><span>:</span>
                        </div>
                        <div class="col-sm-4 col-xs-12">
                            <telerik:RadNumericTextBox ID="txtArea" runat="server" Width="100%" ReadOnly="true" NumberFormat-DecimalDigits="3"></telerik:RadNumericTextBox>
                        </div>
                    </div>
                    <div class="col-xs-12">
                        <div class="col-sm-2 col-xs-12 styleLabel">
                            <asp:Label ID="lbStartDate" runat="server" Text="เริ่มปลูก"></asp:Label><span>:</span>
                        </div>
                        <div class="col-sm-4 col-xs-12">
                            <telerik:RadDatePicker ID="dpStartDate" runat="server" Width="100%" Enabled="false" DateInput-DateFormat="dd-MM-yyyy" DateInput-DisplayDateFormat="dd-MM-yyyy"></telerik:RadDatePicker>
                        </div>
                        <div class="col-sm-2 col-xs-12 styleLabel">
                            <asp:Label ID="lbEndDate" runat="server" Text="เก็บเกี่ยว"></asp:Label><span>:</span>
                        </div>
                        <div class="col-sm-4 col-xs-12">
                            <telerik:RadDatePicker ID="dpEndDate" runat="server" Width="100%" DateInput-DateFormat="dd-MM-yyyy" DateInput-DisplayDateFormat="dd-MM-yyyy"></telerik:RadDatePicker>
                            <asp:RequiredFieldValidator ID="rqdpEndDate" runat="server" Display="Dynamic" ControlToValidate="dpEndDate" ErrorMessage="Harvest date can't be Empty" ForeColor="Red" ValidationGroup="Save"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="col-xs-12">
                        <div class="col-sm-2 col-xs-12 styleLabel">
                            <asp:Label ID="lbSuite" runat="server" Text="เริ่มปลูก"></asp:Label><span>:</span>
                        </div>
                        <div class="col-sm-4 col-xs-12">
                            <telerik:RadTextBox ID="txtSuite" runat="server" Width="100%" ReadOnly="true"></telerik:RadTextBox>
                        </div>
                        <div class="col-sm-2 col-xs-12 styleLabel">
                            <asp:Label ID="lbWater" runat="server" Text="ปริมาณน้ำที่ใช้"></asp:Label><span>:</span>
                        </div>
                        <div class="col-sm-4 col-xs-12">
                            <telerik:RadNumericTextBox ID="txtWater" runat="server" Width="100%" ReadOnly="true"></telerik:RadNumericTextBox>
                        </div>
                    </div>
                    <div class="col-xs-12">
                        <div class="col-sm-2 col-xs-12 styleLabel">
                            <asp:Label ID="lbDemand" runat="server" Text="ความต้องการ"></asp:Label><span>:</span>
                        </div>
                        <div class="col-sm-4 col-xs-12">
                            <telerik:RadNumericTextBox ID="txtDemand" runat="server" Width="100%" ReadOnly="true"></telerik:RadNumericTextBox>
                        </div>
                        <div class="col-sm-2 col-xs-12 styleLabel">
                            <asp:Label ID="lbScore" runat="server" Text="คะแนน"></asp:Label><span>:</span>
                        </div>
                        <div class="col-sm-4 col-xs-12">
                            <telerik:RadNumericTextBox ID="txtScore" runat="server" Width="100%" ReadOnly="true"></telerik:RadNumericTextBox>
                        </div>
                    </div>
                    <div class="col-xs-12">
                        <div class="col-sm-2 col-xs-12">
                        </div>
                        <div class="col-sm-4 col-xs-12" style="text-align:center;">
                            <asp:Label ID="lbForecast" runat="server" Text="คาดการณ์"></asp:Label>
                        </div>
                        <div class="col-sm-4 col-xs-12" style="text-align:center;">
                            <asp:Label ID="lbReal" runat="server" Text="ผลลัพธ์"></asp:Label>
                        </div>
                        <div class="col-sm-2 col-xs-12">
                        </div>
                    </div>
                    <div class="col-xs-12">
                        <div class="col-sm-2 col-xs-12 styleLabel">
                            <asp:Label ID="lbCost" runat="server" Text="ต้นทุน"></asp:Label><span>:</span>
                        </div>
                        <div class="col-sm-4 col-xs-12">
                            <telerik:RadNumericTextBox ID="txtForecastCost" runat="server" Width="100%" NumberFormat-DecimalDigits="2" ReadOnly="true"></telerik:RadNumericTextBox>
                        </div>
                        <div class="col-sm-4 col-xs-12">
                            <telerik:RadNumericTextBox ID="txtRealCost" runat="server" Width="100%" NumberFormat-DecimalDigits="2" MinValue="0" AutoPostBack="true" OnTextChanged="CalProfit_TextChanged"></telerik:RadNumericTextBox>
                        </div>
                        <div class="col-sm-2 col-xs-12">
                        </div>
                    </div>
                    <div class="col-xs-12">
                        <div class="col-sm-2 col-xs-12 styleLabel">
                            <asp:Label ID="lbProduct" runat="server" Text="ผลผลิต"></asp:Label><span>:</span>
                        </div>
                        <div class="col-sm-4 col-xs-12">
                            <telerik:RadNumericTextBox ID="txtForecastProduct" runat="server" Width="100%" NumberFormat-DecimalDigits="2" ReadOnly="true"></telerik:RadNumericTextBox>
                        </div>
                        <div class="col-sm-4 col-xs-12">
                            <telerik:RadNumericTextBox ID="txtRealProduct" runat="server" Width="100%" NumberFormat-DecimalDigits="2" MinValue="0" AutoPostBack="true" OnTextChanged="CalProfit_TextChanged"></telerik:RadNumericTextBox>
                            <asp:RequiredFieldValidator ID="rqtxtRealProduct" runat="server" Display="Dynamic" ControlToValidate="txtRealProduct" ErrorMessage="Product can't be Empty" ForeColor="Red" ValidationGroup="Save"></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-sm-2 col-xs-12">
                        </div>
                    </div>
                    <div class="col-xs-12">
                        <div class="col-sm-2 col-xs-12 styleLabel">
                            <asp:Label ID="lbPrice" runat="server" Text="ราคาขาย"></asp:Label><span>:</span>
                        </div>
                        <div class="col-sm-4 col-xs-12">
                            <telerik:RadNumericTextBox ID="txtForecastPrice" runat="server" Width="100%" NumberFormat-DecimalDigits="2" ReadOnly="true"></telerik:RadNumericTextBox>
                        </div>
                        <div class="col-sm-4 col-xs-12">
                            <telerik:RadNumericTextBox ID="txtRealPrice" runat="server" Width="100%" NumberFormat-DecimalDigits="2" MinValue="0" AutoPostBack="true" OnTextChanged="CalProfit_TextChanged"></telerik:RadNumericTextBox>
                            <asp:RequiredFieldValidator ID="rqtxtRealPrice" runat="server" Display="Dynamic" ControlToValidate="txtRealPrice" ErrorMessage="Price can't be Empty" ForeColor="Red" ValidationGroup="Save"></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-sm-2 col-xs-12">
                        </div>
                    </div>
                    <div class="col-xs-12">
                        <div class="col-sm-2 col-xs-12 styleLabel">
                            <asp:Label ID="lbProfit" runat="server" Text="กำไร"></asp:Label><span>:</span>
                        </div>
                        <div class="col-sm-4 col-xs-12">
                            <telerik:RadNumericTextBox ID="txtForecastProfit" runat="server" Width="100%" NumberFormat-DecimalDigits="2" ReadOnly="true"></telerik:RadNumericTextBox>
                        </div>
                        <div class="col-sm-4 col-xs-12">
                            <telerik:RadNumericTextBox ID="txtRealProfit" runat="server" Width="100%" NumberFormat-DecimalDigits="2" ReadOnly="true"></telerik:RadNumericTextBox>
                            <asp:RequiredFieldValidator ID="rqtxtRealProfit" runat="server" Display="Dynamic" ControlToValidate="txtRealProfit" ErrorMessage="Profit can't be Empty" ForeColor="Red" ValidationGroup="Save"></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-sm-2 col-xs-12">
                        </div>
                    </div>
                    <div class="col-xs-12">
                        <div class="col-sm-2 col-xs-12 styleLabel">
                            <asp:Label ID="lbComplete" runat="server" Text="สมบูรณ์"></asp:Label><span>:</span>
                        </div>
                        <div class="col-sm-4 col-xs-12">
                            <telerik:RadCheckBox ID="cbCompleted" runat="server"></telerik:RadCheckBox>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="dvControl" class="styleFormatControl">
        <telerik:RadButton ID="btnSave" runat="server" Text="Save" Width="100px" ValidationGroup="Save"></telerik:RadButton>
        <telerik:RadButton ID="btnBack" runat="server" Text="Back" Width="100px"></telerik:RadButton>
    </div>
</asp:Content>
