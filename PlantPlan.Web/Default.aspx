<%@ Page Language="VB" AutoEventWireup="false" CodeBehind="Default.aspx.vb" Inherits="PlantPlan.Web._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="../Styles/bootstrap.min.css" rel="stylesheet" />
    <title></title>
    <telerik:RadStyleSheetManager ID="RadStyleSheetManager1" runat="server" />
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
        <div class="col-xs-12">
            <div class="col-xs-12" style="text-align: center; padding-top:10px;">
                <asp:Image runat="server" ImageUrl="Images/Plant-Plan-AHP.png" />
            </div>
            <div class="col-xs-12">
                <div class="col-sm-3 col-xs-12">
                </div>
                <div class="col-sm-6 col-xs-12" style="align-content: center;">                    
                    <telerik:RadTextBox ID="txtUserName" runat="server" Skin="MetroTouch" Width="30%" MaxLength="20" placeholder="ชื่อผู้ใช้งาน">
                    </telerik:RadTextBox>
                    <asp:RequiredFieldValidator ID="rqUserName" runat="server" Display="Dynamic" ValidationGroup="Login"
                        ControlToValidate="txtUserName" ErrorMessage="โปรดใส่ชื่อผู้ใช้งาน" ForeColor="Red"></asp:RequiredFieldValidator>
                    <telerik:RadTextBox ID="txtPassword" runat="server" Skin="MetroTouch" TextMode="Password" Width="30%" MaxLength="20" placeholder="รหัสผ่าน">
                    </telerik:RadTextBox>
                    <asp:RequiredFieldValidator ID="rqPassword" runat="server" Display="Dynamic" ValidationGroup="Login"
                        ControlToValidate="txtPassword" ErrorMessage="โปรดใส่รหัสผ่าน" ForeColor="Red"></asp:RequiredFieldValidator>
                    <telerik:RadButton ID="btnSignin" runat="server" Skin="BlackMetroTouch" Text="เข้าสู่ระบบ" ValidationGroup="Login" Width="30%"></telerik:RadButton>
                </div>
                <div class="col-sm-3 col-xs-12">
                </div>
            </div>
            <div class="col-xs-12">
                <div class="col-sm-3 col-xs-12">
                </div>
                <div class="col-sm-6 col-xs-12" style="align-content: center;">
                    <asp:Label runat="server" Text="" ID="lbMsg" ForeColor="Red"></asp:Label>
                </div>
                <div class="col-sm-3 col-xs-12">
                </div>
            </div>
        </div>
    </form>
</body>
</html>
