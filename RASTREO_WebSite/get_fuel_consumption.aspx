<%@ Page Language="VB" AutoEventWireup="false" CodeFile="get_fuel_consumption.aspx.vb" Inherits="get_fuel_consumption" %>

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
        <asp:Button CssClass="rastreo_botones_save" runat="server" ID="btnExcel" Text="Descargar planilla de consumo" ToolTip="Haga click aqui para descargar el archivo de la planilla de recorrido"/>
        <table width="100%">
        <tr>
        <td>
        <asp:UpdatePanel runat="server" ID="updpnl_VehiculoSeleccionado">
        <ContentTemplate>
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
                        <asp:TemplateField>
                            <HeaderTemplate>
                                Resumen de consumo
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#Resumen_vehiculo(Convert.ToString(Eval("id_vehiculo")))%>
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




