<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Masters/PageMaster.Master" CodeBehind="WebForm2.aspx.vb" Inherits="PlantPlan.Web.WebForm2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="col-xs-12">
        <div class="col-sm-4 col-xs-12">
            จังหวัด :<telerik:RadComboBox ID="ddProvince" ClientIDMode="Static" runat="server" Width="100%" MarkFirstMatch="true" AutoPostBack="true"></telerik:RadComboBox>
        </div>
        <div class="col-sm-4 col-xs-12">
            อำเภอ :<telerik:RadComboBox ID="ddAmphoe" ClientIDMode="Static" runat="server" Width="100%" MarkFirstMatch="true" AutoPostBack="true"></telerik:RadComboBox>
        </div>
        <div class="col-sm-4 col-xs-12">
            ตำบล :<telerik:RadComboBox ID="ddTambon" ClientIDMode="Static" runat="server" Width="100%" MarkFirstMatch="true" AutoPostBack="true"></telerik:RadComboBox>
        </div>
    </div>
    <div class="col-xs-12">
        <div class="col-sm-6 col-xs-12">
        </div>
        <div class="col-sm-6 col-xs-12">
            <asp:Button runat="server" Text="Button" />
            <telerik:RadButton runat="server" Text="Button" Skin="" />
        </div>
    </div>
</asp:Content>
