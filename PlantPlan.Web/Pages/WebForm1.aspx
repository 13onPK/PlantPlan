<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Masters/PageMaster.master" CodeBehind="WebForm1.aspx.vb" Inherits="PlantPlan.Web.WebForm1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" type="text/css" href="../Styles/Menucomponent.css" />
    <link rel="stylesheet" type="text/css" href="../Styles/Menudefault.css" />
    <script src="../scripts/modernizr.custom.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="column">
        <asp:Panel runat="server">
            <iframe id="ifPopup" src="http://www.thaiwater.net/DATA/REPORT/php/rid_lgraph3.php?dam_id=41" height="600" width="1240"></iframe>
        </asp:Panel>
    </div>
</asp:Content>
