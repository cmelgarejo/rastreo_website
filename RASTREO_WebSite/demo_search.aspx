<%@ Page Language="VB" AutoEventWireup="false" CodeFile="demo_search.aspx.vb" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<!-- Autor: Ing. Christian Melgarejo Bresanovich || CLRSoft.net -->

  <head id="mainHEAD" runat="server">
    <script type="text/javascript" src="http://www.openlayers.org/api/OpenLayers.js"></script>
    <title>Search school demo</title>
  </head>

<body onload="init();">
<form id="mainFORM" runat="server" >

<ajaxToolkit:ToolkitScriptManager ID="mainToolScriptManager" ScriptMode="Release" runat="server" >
</ajaxToolkit:ToolkitScriptManager>

<table width="100%">
<tr>
<td>
<div id="map" style="width:100%;height:400px;border: 1px solid black;"></div>
</td>
</tr>
<tr>
<td>

<asp:UpdatePanel ID="updPnlSchools" runat="server">
<ContentTemplate>
<asp:HiddenField ID="reffile" runat="server" Value="demosearch.txt" />
<asp:DropDownList ID="ddlSearchBy" runat="server" CssClass="DropDownList"></asp:DropDownList>
<asp:TextBox runat="server" ID="txtSearch" CssClass="TextBox" ></asp:TextBox>
<asp:Button ID="btnSUBMIT" runat="server" Text="Search"/>
<asp:Button ID="btnCLEAR" runat="server" Text="Clear Search"/>
<asp:DataGrid runat="server" ID="grdDEMO" DataSourceID="SqlDataSource1" AutoGenerateColumns="false"
        Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None">
    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
    <EditItemStyle BackColor="#2461BF" />
    <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
    <AlternatingItemStyle BackColor="White" />
    <ItemStyle BackColor="#EFF3FB" />
    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
    <Columns>
        <asp:TemplateColumn>
            <HeaderTemplate>School</HeaderTemplate>
            <ItemTemplate>
                <a href='#' onclick='move_map(<%#Eval("lon") %>, <%#Eval("lat") %>)'><%#Eval("SchoolName")%></a>
            </ItemTemplate>
        </asp:TemplateColumn>
        <asp:BoundColumn DataField="Address" HeaderText="Address" />
        <asp:BoundColumn DataField="PhoneNo" HeaderText="Phone No." />
        <asp:TemplateColumn>
            <HeaderTemplate>Website</HeaderTemplate>
            <ItemTemplate>
                <a href='http://<%#Eval("WebSite")%>' target="_blank">Click</a>
            </ItemTemplate>
        </asp:TemplateColumn>
        <asp:BoundColumn DataField="lon" HeaderText="Longitude" />
        <asp:BoundColumn DataField="lat" HeaderText="Latitude" />
    </Columns>
</asp:DataGrid>
    <%--<asp:SqlDataSource SelectCommand="SELECT * FROM [Schools]" ID="SqlDataSource1" 
        runat="server" ConnectionString="<%$ ConnectionStrings:testConnectionString %>">
    </asp:SqlDataSource>--%>
    <asp:SqlDataSource SelectCommand="" ID="SqlDataSource1" 
        runat="server" ConnectionString="">
    </asp:SqlDataSource>
</ContentTemplate>
</asp:UpdatePanel>
</td>
</tr>
</table>
    <script type="text/javascript" src="demo.js"></script>
</form>
</body>
</html>