<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Masters/PageMaster.master" CodeBehind="Management.aspx.vb" Inherits="PlantPlan.Web.Management" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="dvMain" runat="server" style="margin-top:50px;">
        <asp:UpdateProgress ID="updateProgress" runat="server">
            <ProgressTemplate>
                <div style="position: fixed; text-align: center; vertical-align: middle; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 500; opacity: 0.5;">
                    <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="../../Images/Loading.gif" AlternateText="Loading ..." ToolTip="Loading ..." Style="padding: 10px; position: fixed; top: 45%; left: 45%;" />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:UpdatePanel ID="upnMain" runat="server">
            <ContentTemplate>
                <telerik:RadGrid ID="grdCrop" runat="server" PageSize="10" PagerStyle-PageButtonCount="5"
                    OnNeedDataSource="grdCrop_NeedDataSource" AllowCustomPaging="true"
                    AllowPaging="True" AllowSorting="true" RenderMode="Auto">
                    <GroupingSettings CaseSensitive="false" ShowUnGroupButton="true" />
                    <MasterTableView AutoGenerateColumns="False" AllowFilteringByColumn="true" TableLayout="Fixed"
                        DataKeyNames="TPgc_Id" CommandItemDisplay="Top" InsertItemPageIndexAction="ShowItemOnFirstPage">
                        <CommandItemTemplate>
                            <div style="color:white; text-align:center; height:30px; line-height:30px;">
                                Managemant
                            </div>
                        </CommandItemTemplate>
                        <Columns>
                            <telerik:GridButtonColumn UniqueName="ViewColumn" ImageUrl="~/Images/view.png" ButtonType="ImageButton" Text="View" CommandName="View">
                                <HeaderStyle HorizontalAlign="Center" Width="5%" />
                                <FooterStyle HorizontalAlign="Center" Width="5%" />
                                <ItemStyle HorizontalAlign="Center" Width="5%" />
                            </telerik:GridButtonColumn>
                            <telerik:GridBoundColumn DataField="SPntgp_Name" HeaderText="พืช" SortExpression="SPntgp_Name" FilterControlWidth="90%"
                                UniqueName="SPntgp_Name" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith" ShowFilterIcon="false">
                                <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                <FooterStyle HorizontalAlign="Center" Width="10%" />
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="10%" />
                                <FilterTemplate>
                                    <telerik:RadComboBox ID="cbxSPntgp" runat="server" MarkFirstMatch="true" RenderMode="Lightweight" AppendDataBoundItems="false" MaxHeight="300"
                                        SelectedValue='<%# TryCast(Container, GridItem).OwnerTableView.GetColumn("SPntgp_Name").CurrentFilterValue %>'
                                        OnInit="cbxSPntgp_Init" OnClientSelectedIndexChanged="cbxSPntgpIndexChanged" Width="90%">
                                    </telerik:RadComboBox>
                                    <telerik:RadScriptBlock ID="sbSPntgp" runat="server">
                                        <script type="text/javascript">
                                            function cbxSPntgpIndexChanged(sender, args) {
                                                var tableView = $find("<%# TryCast(Container, GridItem).OwnerTableView.ClientID %>");
                                                if (args.get_item().get_value() != "-- All --")
                                                    tableView.filter("SPntgp_Name", args.get_item().get_value(), "EqualTo");
                                                else
                                                    tableView.filter("SPntgp_Name", args.get_item().get_value(), "NotEqualTo");
                                            }
                                        </script>
                                    </telerik:RadScriptBlock>
                                </FilterTemplate>
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="SProv_Name" HeaderText="จังหวัด" SortExpression="SProv_Name" FilterControlWidth="90%"
                                UniqueName="SProv_Name" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith" ShowFilterIcon="false">
                                <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                <FooterStyle HorizontalAlign="Center" Width="10%" />
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="10%" />
                                <FilterTemplate>
                                    <telerik:RadComboBox ID="cbxSProv" runat="server" MarkFirstMatch="true" RenderMode="Lightweight" AppendDataBoundItems="false" MaxHeight="300"
                                        SelectedValue='<%# TryCast(Container, GridItem).OwnerTableView.GetColumn("SProv_Name").CurrentFilterValue %>'
                                        OnInit="cbxSProv_Init" OnClientSelectedIndexChanged="cbxSProvIndexChanged" Width="90%">
                                    </telerik:RadComboBox>
                                    <telerik:RadScriptBlock ID="sbSProv" runat="server">
                                        <script type="text/javascript">
                                            function cbxSProvIndexChanged(sender, args) {
                                                var tableView = $find("<%# TryCast(Container, GridItem).OwnerTableView.ClientID %>");
                                                if (args.get_item().get_value() != "-- All --")
                                                    tableView.filter("SProv_Name", args.get_item().get_value(), "EqualTo");
                                                else
                                                    tableView.filter("SProv_Name", args.get_item().get_value(), "NotEqualTo");
                                            }
                                        </script>
                                    </telerik:RadScriptBlock>
                                </FilterTemplate>
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="SAmp_Name" HeaderText="อำเภอ" SortExpression="SAmp_Name" FilterControlWidth="90%"
                                UniqueName="SAmp_Name" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith" ShowFilterIcon="false">
                                <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                <FooterStyle HorizontalAlign="Center" Width="10%" />
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="10%" />
                                <FilterTemplate>
                                    <telerik:RadComboBox ID="cbxSAmp" runat="server" MarkFirstMatch="true" RenderMode="Lightweight" AppendDataBoundItems="false" MaxHeight="300"
                                        SelectedValue='<%# TryCast(Container, GridItem).OwnerTableView.GetColumn("SAmp_Name").CurrentFilterValue %>'
                                        OnInit="cbxSAmp_Init" OnClientSelectedIndexChanged="cbxSAmpIndexChanged" Width="90%">
                                    </telerik:RadComboBox>
                                    <telerik:RadScriptBlock ID="sbSAmp" runat="server">
                                        <script type="text/javascript">
                                            function cbxSAmpIndexChanged(sender, args) {
                                                var tableView = $find("<%# TryCast(Container, GridItem).OwnerTableView.ClientID %>");
                                                if (args.get_item().get_value() != "-- All --")
                                                    tableView.filter("SAmp_Name", args.get_item().get_value(), "EqualTo");
                                                else
                                                    tableView.filter("SAmp_Name", args.get_item().get_value(), "NotEqualTo");
                                            }
                                        </script>
                                    </telerik:RadScriptBlock>
                                </FilterTemplate>
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="STamb_Name" HeaderText="ตำบล" SortExpression="STamb_Name" FilterControlWidth="90%"
                                UniqueName="STamb_Name" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith" ShowFilterIcon="false">
                                <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                <FooterStyle HorizontalAlign="Center" Width="10%" />
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="10%" />
                                <FilterTemplate>
                                    <telerik:RadComboBox ID="cbxSTamb" runat="server" MarkFirstMatch="true" RenderMode="Lightweight" AppendDataBoundItems="false" MaxHeight="300"
                                        SelectedValue='<%# TryCast(Container, GridItem).OwnerTableView.GetColumn("STamb_Name").CurrentFilterValue %>'
                                        OnInit="cbxSTamb_Init" OnClientSelectedIndexChanged="cbxSTambIndexChanged" Width="90%">
                                    </telerik:RadComboBox>
                                    <telerik:RadScriptBlock ID="sbSTamb" runat="server">
                                        <script type="text/javascript">
                                            function cbxSTambIndexChanged(sender, args) {
                                                var tableView = $find("<%# TryCast(Container, GridItem).OwnerTableView.ClientID %>");
                                                if (args.get_item().get_value() != "-- All --")
                                                    tableView.filter("STamb_Name", args.get_item().get_value(), "EqualTo");
                                                else
                                                    tableView.filter("STamb_Name", args.get_item().get_value(), "NotEqualTo");
                                            }
                                        </script>
                                    </telerik:RadScriptBlock>
                                </FilterTemplate>
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="TPgc_FarmerName" HeaderText="เกษตรกร" SortExpression="TPgc_FarmerName" FilterControlWidth="90%"
                                UniqueName="TPgc_FarmerName" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                <FooterStyle HorizontalAlign="Center" Width="10%" />
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="10%" />
                            </telerik:GridBoundColumn>
                            <telerik:GridNumericColumn DataField="TPgc_Area" HeaderText="พื้นที่ปลูก" SortExpression="TPgc_Area" FilterControlWidth="90%"
                                UniqueName="TPgc_Area" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" ShowFilterIcon="false">
                                <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                <FooterStyle HorizontalAlign="Center" Width="10%" />
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="10%" />
                            </telerik:GridNumericColumn>
                            <telerik:GridDateTimeColumn DataField="TPgc_StartDate" HeaderText="เริ่มปลูก" SortExpression="TPgc_StartDate" FilterControlWidth="90%"
                                UniqueName="TPgc_StartDate" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" ShowFilterIcon="false" FilterDateFormat="dd-MM-yyyy" DataFormatString="{0:dd-MM-yyyy}">
                                <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                <FooterStyle HorizontalAlign="Center" Width="10%" />
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="10%" />
                            </telerik:GridDateTimeColumn>
                            <telerik:GridDateTimeColumn DataField="TPgc_EndDate" HeaderText="เก็บเกี่ยว" SortExpression="TPgc_EndDate" FilterControlWidth="90%"
                                UniqueName="TPgc_EndDate" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" ShowFilterIcon="false" FilterDateFormat="dd-MM-yyyy" DataFormatString="{0:dd-MM-yyyy}">
                                <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                <FooterStyle HorizontalAlign="Center" Width="10%" />
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="10%" />
                            </telerik:GridDateTimeColumn>
                            <telerik:GridCheckBoxColumn DataField="TPgc_IsCompleted" HeaderText="สมบูรณ์" SortExpression="TPgc_IsCompleted" FilterControlWidth="90%"
                                UniqueName="TPgc_IsCompleted" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" ShowFilterIcon="false">
                                <HeaderStyle HorizontalAlign="Center" Width="8%" />
                                <FooterStyle HorizontalAlign="Center" Width="8%" />
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="8%" />
                            </telerik:GridCheckBoxColumn>
                            <telerik:GridEditCommandColumn UniqueName="EditColumn">
                                <HeaderStyle HorizontalAlign="Center" Width="5%" />
                                <FooterStyle HorizontalAlign="Center" Width="5%" />
                                <ItemStyle HorizontalAlign="Left" Width="5%" />
                            </telerik:GridEditCommandColumn>
                        </Columns>
                    </MasterTableView>
                    <ClientSettings AllowColumnsReorder="true" AllowColumnHide="true">
                        <Selecting AllowRowSelect="true" />
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                    </ClientSettings>
                </telerik:RadGrid>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
