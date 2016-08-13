<%@ Page Language="VB" AutoEventWireup="false" CodeFile="get_recorrido.aspx.vb" Inherits="get_recorrido" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<!-- Autor: Ing. Adolfo Bresanovich || Director de R&D RASTREO Paraguay -->
<head id="get_recorridoHead" runat="server">

    <link rel="shortcut icon" href="~/RASTREO.ico" type="image/x-icon"/>
    <link rel="icon" href="~/RASTREO.ico" type="image/x-icon"/>
    <title>RASTREO Paraguay - Recorrido del vehiculo <%=vehiculo_seleccionado%></title>
    <%--<script type="text/javascript" src="FuncionesGenerales.js"></script>--%>
    
</head>
<body>
    <div>
    <form id="RASTREOmainForm" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="RASTREOToolScriptManager" ScriptMode="Release" runat="server">
            <Scripts>
            <asp:ScriptReference Path="~/WebKit.js" />
        </Scripts>
    </ajaxToolkit:ToolkitScriptManager>    
    <div id="RASTREO_mainwrapper">
        <asp:UpdateProgress runat="server" ID="updpg_pnlGPS" AssociatedUpdatePanelID="updpnl_VehiculoSeleccionado">
            <ProgressTemplate>
                <div style="position:absolute;left:0px;top:0px">
                    <img src="./App_Themes/CENTRAL/Imagenes/ajax-loader.gif" alt="-"/>
                    <label class="rastreo_nowloading">Actualizando...</label>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:Label ID="errMsgs" runat="server" Width="100%" CssClass="Mensaje_de_error" ></asp:Label>
        <asp:Button CssClass="rastreo_botones_save" runat="server" ID="btnExcel" Text="Descargar planilla de recorrido" ToolTip="Haga click aqui para descargar el archivo de la planilla de recorrido"/>
        <table width="100%">
        <tr>
        <td>
        <asp:UpdatePanel runat="server" ID="updpnl_VehiculoSeleccionado">
        <ContentTemplate>
            <asp:HiddenField ID="txtFECHAINICIO" runat="server" />
            <asp:HiddenField ID="HiddenField1" runat="server" />
                    <table>
                    <tr>
                    <td>
                    <asp:GridView ID="gridVehiculoSeleccionado" runat="server" 
                        DataSourceID="sqldsVehiculoSeleccionado" AllowPaging="True"
                        AllowSorting="True" AutoGenerateColumns="False" BackColor="White" 
                        BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" 
                        GridLines="Vertical" Width="100%" Font-Names="Nina, Segoe, Arial Narrow, Arial, Verdana"
                        Font-Size="XX-Small">
                        <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                        <Columns>
                        <asp:BoundField HeaderText="Identificador" DataField="identificador_rastreo" />
                        <asp:BoundField HeaderText="Fecha y hora" DataField="gps_fechahora_reporte" />
                        <asp:BoundField  HeaderText="Latitud" DataField="gps_latitud" />
                        <asp:BoundField  HeaderText="Longitud" DataField="gps_longitud" />
                        <asp:BoundField  HeaderText="Velocidad" DataField="gps_velocidad" />
                        <asp:TemplateField>
                            <HeaderTemplate> Rumbo </HeaderTemplate>
                            <ItemTemplate>
                                <%#Eval("gps_rumbo") %>° <%# Eval("rumbo") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <table>
                                    <tr>
                                        <td style='background-color:<%#Eval("color_evento") %>; color:White;' align="center">
                                            <%#Eval("evento")%>
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <%#gGetReferenciaCercana(Convert.ToDouble(Eval("gps_latitud")), Convert.ToDouble(Eval("gps_longitud")))%>
                            </ItemTemplate>
                        </asp:TemplateField>                        
                        <asp:TemplateField>
                            <HeaderTemplate>
                                Direccion cercana
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#GeoPos(Convert.ToString(Eval("gps_latitud")), Convert.ToString(Eval("gps_longitud")))%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        </Columns>
                        <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                        <PagerStyle BackColor="Navy" ForeColor="White" HorizontalAlign="Center" />
                        <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
                        <AlternatingRowStyle BackColor="#BABABA" />
                        </asp:GridView>
                        <asp:SqlDataSource ID="sqldsVehiculoSeleccionado"  runat="server" ConnectionString="<%$ ConnectionStrings:CS_RASTREOSystem %>" ProviderName="<%$ ConnectionStrings:CS_RASTREOSystem.ProviderName %>"
                            SelectCommand="" EnableCaching="true" CacheDuration="Infinite">
                            <SelectParameters>
                                <asp:QueryStringParameter QueryStringField="fchini" Name="fecha_de_inicio" DbType="DateTime" />
                                <asp:QueryStringParameter QueryStringField="fchfin"  Name="fecha_de_fin" DbType="DateTime"/>
                                <asp:QueryStringParameter QueryStringField="evento" Name="evento" DbType="String" />
                            </SelectParameters>
                        </asp:SqlDataSource>
        </ContentTemplate>
        </asp:UpdatePanel>
        </td>
        </tr>
        </table>
        </div>
    </form>
    </div>
</body>
</html>




