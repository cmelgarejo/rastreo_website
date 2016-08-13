<%@ Page Title="RASTREO Paraguay - En construcción..." Language="VB" MasterPageFile="RASTREOMasterPage.master" AutoEventWireup="false" CodeFile="NotReady.aspx.vb" Inherits="_NOTREADY" %>

<asp:Content ID="contentNOTREADY" ContentPlaceHolderID="main_ContentPlaceHolder" Runat="Server">
    <script type="text/javascript" src="FuncionesGenerales.js"></script> 
<%--    <%If Request.ServerVariables("REMOTE_HOST") = "190.104.156.142" Then%>--%>
        <script src="http://maps.google.com/maps?file=api&amp;v=2&amp;sensor=false&amp;key=ABQIAAAA_r2VgoolhB6iO9xSBULQFxTPNvAASRBEw_TMHTynVLwSexQB8hQ4RTPwWf589F08aFbkKIvY434rxQ" type="text/javascript"></script> 
<%--    <%ElseIf Request.ServerVariables("REMOTE_HOST") = "190.104.153.61" Then%>
        <script src="http://maps.google.com/maps?file=api&amp;v=2&amp;sensor=false&amp;key=ABQIAAAA_r2VgoolhB6iO9xSBULQFxQjCxOHmMAjVGNZYZATNfcVIoBu7hQt5fsrP_aWckxRC585DEwI98L-ow" type="text/javascript"></script>
    <%ElseIf Request.ServerVariables("REMOTE_HOST") = "127.0.0.1" Then%>
        <script src="http://maps.google.com/maps?file=api&amp;v=2&amp;sensor=false&amp;key=ABQIAAAA_r2VgoolhB6iO9xSBULQFxQ9pzGUvE4e-uE6JwppIOBVvhvAmRQysIA9otMz1DX9na4y4LGsV3YqDA" type="text/javascript"></script>
    <%End If%>--%>
    <script type="text/javascript" src="OpenLayers.js"></script>
    <script type="text/javascript" src="FuncionesGPS.js"></script>
    <%--<script src='http://dev.virtualearth.net/mapcontrol/mapcontrol.ashx?v=6.1'></script>--%>
    <%--<script src="http://api.maps.yahoo.com/ajaxymap?v=3.0&appid=euzuro-openlayers"></script>--%>
    <%--<script type="text/javascript" src="http://www.openlayers.net/api/OpenLayers.js"></script>--%>    

<input type="button" value="Test reversegeo google" id="btnTest" onclick="google_ReverseGeocoding(this, -25.301200,-57.627411);" />
<asp:Panel runat="server" BackImageUrl="~/App_Themes/CENTRAL/Imagenes/matrixanim.gif">
<br /><br /><br /><br /><br /><br /><br />
<table width="100%">
<tr>
<td style="text-align:left; color: Lime; font-family:Arial, Impact, Arial Narrow; font-size: large;">Pagina en construcción</td>
<td style="text-align:right; color: Lime; font-family:Arial, Impact, Arial Narrow; font-size: large;">Pagina en construcción</td>
</tr>
<tr>
<td colspan="2" style="text-align:center" valign="middle">
<asp:Image ImageUrl="~/App_Themes/CENTRAL/Imagenes/under-construction.png" runat="server" />
</td>
</tr>
<tr>
<td style="text-align:left; color: Lime; font-family:Arial, Impact, Arial Narrow; font-size: large;">Pagina en construcción</td>
<td style="text-align:right; color: Lime; font-family:Arial, Impact, Arial Narrow; font-size: large;">Pagina en construcción</td>
</tr>
</table>
<br /><br /><br /><br /><br /><br /><br />
</asp:Panel>
</asp:Content>                    

