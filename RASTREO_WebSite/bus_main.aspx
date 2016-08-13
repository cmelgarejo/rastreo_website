<%@ Page Title="RASTREO Paraguay - Control de flota de Autobuses" EnableViewStateMac="false" Language="VB" Async="false" AutoEventWireup="false" CodeFile="bus_main.aspx.vb" Inherits="bus_main" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<!-- Autor: Ing. Adolfo Bresanovich || Director de R&D RASTREO Paraguay -->

<head id="GPSadminHead" runat="server">

    <link rel="shortcut icon" href="~/RASTREO.ico" type="image/x-icon"/>
    <link rel="icon" href="~/RASTREO.ico" type="image/x-icon"/>
    <title>RASTREO WebSystem - Bienvenido!</title>

    <script type="text/javascript" src="FuncionesGenerales.js"></script>    
    
</head>
<body onload='busmain_init_map();countdown_timer(<%= txtTimeout.text %>);'>

    <form id="RASTREOmainForm" runat="server" >
        
    <ajaxToolkit:ToolkitScriptManager AsyncPostBackTimeout="3600" ID="RASTREOToolScriptManager" ScriptMode="Release" runat="server" >
        <Scripts>
            <asp:ScriptReference Path="~/WebKit.js" />
        </Scripts>
    </ajaxToolkit:ToolkitScriptManager>
    
    
    <asp:HiddenField ID="idUsuario" runat="server" Value="0"/>
    <asp:HiddenField ID="idPersona" runat="server" Value="0"/>
    <%--<asp:HiddenField ID="idVehiculoSeleccionado" runat="server" Value="0"/>--%>

    <div id="RASTREO_myHeader_RASTREAR" >       
            <asp:UpdateProgress runat="server" ID="updpg_pnlGPS" AssociatedUpdatePanelID="pnlbus_main">
            <ProgressTemplate>
                <div style="position:absolute;left:0px;top:0px;width:100%;height:1000%" class="modalBackground">
                    <img src="./App_Themes/CENTRAL/Imagenes/ajax-loader.gif" alt="-"/>
                    <label class="rastreo_nowloading">Actualizando...</label>
                </div>
            </ProgressTemplate>
            </asp:UpdateProgress>
            <table style="text-align:right;width:100%" cellpadding="0" cellspacing="0">
                <tr>  
                    <td style="width:20%" align="center">
                        <span style="font-size:x-small;left:110px;position:relative">ϐeta</span>
                    </td>
                    <td style="width:50%">Cliente: <% Response.Write(mi_cliente)%><br />
                    Usuario: <% Response.Write(mi_usuario) %></td>
                    <td style="width:30%; text-align:center; border: double 2px #024579; vertical-align:middle">
                        <asp:LinkButton CssClass="rastreo_linkbuttons" runat="server" ID="RASTREO_LOGOUT">Salir del sistema</asp:LinkButton>
                        <asp:Image ID="imgsyslogout" runat="server" ImageUrl="~/App_Themes/CENTRAL/Imagenes/system-log-out.png" />    
                    </td>
                </tr>
                <tr align="right">
                <td style="width:30%">
                </td>
                <td valign="top" colspan="2">            
                    <input id="cd_timer" class="countdown" value="Bienvenido!" readonly="readonly" />
                    <asp:CheckBox runat="server" ID="chkCalles" Text="Ver direcciones cercanas" 
                    ToolTip="Chequee esta casilla si desea ver la calle cercana a la posicion de sus moviles, tenga en cuenta que esta opcion degrada la velocidad de carga de datos del sistema." Checked="true" />
                    <asp:TextBox runat="server" ID="txtTimeout" CssClass="TextBox" 
                    onchange="if (isNaN(this.value)) { this.value='10'; this.focus()}" Width="30px"
                    Text="10"
                    MaxLength="3" >
                    </asp:TextBox>
                    <asp:Button runat="server" CssClass="rastreo_botones" ID="btnAjustar"
                     Text="Ajustar intérvalo" OnClientClick="DisableSave(this);" />
                    <table width="100%" cellpadding="0" cellspacing="0" class="PopUp">
                        <tr>
                            <% If permiso_REFERENCIAS Then%>
                            <td>
                                <input value="Administrar Referencias" type="button" 
                                title="Haga click aqui para crear hojas de referencia que contendran puntos de referencia a ser utilizados en las hojas de ruta"
                                class="rastreo_botones" 
                                onclick="AbrirDialogo('Referencias.aspx?uid=<%= idUsuario.Value %>&cid=<%= idPersona.Value %>')"/>
                            </td>
                            <% End If%>
                        </tr>
                    </table>
                 </td>
            </tr>
            </table>    
    </div>

    <div id="RASTREO_mainwrapper">
        <div id="toptop" visible="false" />
        <asp:Label ID="errMsgs" runat="server" Width="99%" CssClass="Mensaje_de_error" 
        Visible="false"></asp:Label>
        <table width="100%" cellpadding="0" cellspacing="0">
        <tr>
            <td valign="top" align="center">
                <asp:Label ID="Label1" runat="server" Width="100%" CssClass="Mensaje_de_error" Text="Vehiculos en viaje de IDA" />
                <div id="mapbus_ida" class="PopUp">
                </div>
            </td>
            <td valign="top" align="center">
                <asp:Label ID="Label2" runat="server" Width="100%" CssClass="Mensaje_de_error" Text="Vehiculos en viaje de VUELTA" />
                <div id="mapbus_vuelta" class="PopUp">
                </div>
            </td>
        </tr>
        <tr>
            <td valign="top" colspan="2">
                <asp:UpdatePanel runat="server" ID="pnlbus_main">
                <ContentTemplate>        
                    <asp:HiddenField ID="txtfile_ida" runat="server" />
                    <asp:HiddenField ID="txtfile_vuelta" runat="server" />
                    <asp:HiddenField ID="reffile" runat="server" />
                    <asp:Timer runat="server" ID="tmr_Reload" Interval="10000" />
                    <div class="divBusIDA">
                    <asp:GridView PageSize="20"  ID="gv_ListaVehiculos_IDA" runat="server" 
                            DataSourceID="sqlVehiculosIDA" AllowPaging="True" 
                            DataKeyNames="identificador_rastreo, id_vehiculo, gps_longitud, gps_latitud"
                            AllowSorting="True" AutoGenerateColumns="False" BackColor="White" 
                            BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" 
                            GridLines="Vertical" Width="995px" Font-Names="Nina, Segoe, Arial Narrow, Arial, Verdana"
                            HeaderStyle-Font-Names="Nina, Segoe, Arial Narrow, Arial, Verdana"
                            Font-Size="Small" EmptyDataText="No tiene vehiculos asignados o en viaje de IDA."
                            EmptyDataRowStyle-CssClass="Mensaje_de_error">
                            <EmptyDataRowStyle CssClass="Mensaje_de_error" HorizontalAlign="Center" />
                            <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                            <Columns>
                            <asp:TemplateField> 
                                <ItemStyle Width="70px" />
                                <HeaderTemplate> Vehiculo </HeaderTemplate>
                                <ItemTemplate>
                                    <ajaxToolkit:HoverMenuExtender runat="server" ID="masterHuverTareas" TargetControlID="lnkTareas" PopupControlID="pnlTareas" PopupPosition="Right" OffsetX="5" OffsetY="-15">
                                    </ajaxToolkit:HoverMenuExtender>
                                    <asp:Label runat="server" ID="lnkTareas">
                                    <a title="Centrar el móvil <%#Eval("identificador_rastreo")%> " href="#toptop" onclick="busida_map_panTo(<%# Convert.ToString(Eval("gps_longitud")).Replace(",",".") %>,<%# Convert.ToString(Eval("gps_latitud")).Replace(",",".") %>);">
                                      <%#Eval("identificador_rastreo")%> 
                                    </a>
                                    </asp:Label>
                                    <asp:Panel BorderColor="Black" BorderWidth="2px" BackColor="Navy" Font-Size="X-Small" ForeColor="White" ID="pnlTareas" runat="server">
                                        <a title="Haga click aqui para obtener los recorridos del dia del móvil <%#Eval("identificador_rastreo")%> " class="OpcionTarea" onclick="AbrirDialogo('bus_historico.aspx?vid=<%#Eval("id_vehiculo")%>')">
                                         Recorridos del día
                                        </a>
                                        <br />
                                        <a title="Haga click aqui para obtener el recorrido histórico del móvil <%#Eval("identificador_rastreo")%> " class="OpcionTarea" onclick="AbrirDialogo('historico.aspx?vid=<%#Eval("id_vehiculo")%>')">
                                         Historicos
                                        </a>
                                        <br />
                                        <% If permiso_GEOCERCA Then%>
                                        <a title="Haga click aqui para crear geocercas para el vehiculo <%#Eval("identificador_rastreo")%> " class="OpcionTarea" onclick="AbrirDialogo('Geocercas.aspx?uid=<%#Eval("id_usuarios")%>&cid=<%#Eval("id_cliente") %>&vid=<%#Eval("id_vehiculo") %>')">
                                         GeoCerca
                                        </a>
                                        <br />
                                        <% End if %></asp:Panel>
                                    <ajaxToolkit:HoverMenuExtender runat="server" ID="masterHuverDatos" TargetControlID="lnkTareas" PopupControlID="pnlDatos" PopupPosition="Bottom" OffsetX="150" OffsetY="-30">
                                    </ajaxToolkit:HoverMenuExtender>
                                    <asp:Panel BorderColor="Black" BorderWidth="2px" BackColor="DarkGreen" Font-Size="X-Small" ForeColor="White" ID="pnlDatos" runat="server">
                                        <table cellpadding="0" cellspacing="0">
                                            <tr><td>Modelo:</td><td><%#Eval("marca")%></td></tr>
                                            <tr><td>Marca:</td><td><%#Eval("modelo")%></td></tr>
                                            <tr><td>Patente:</td><td><%#Eval("matricula")%></td></tr>
                                        </table>
                                    </asp:Panel>
                                </ItemTemplate>
                            </asp:TemplateField>                        
                            <asp:BoundField HeaderText="Fecha y hora" DataField="gps_fecha" ItemStyle-Width="100px" />
                            <asp:TemplateField>
                                <ItemStyle Width="350px" />
                                <HeaderTemplate>
                                    Direccion cercana  
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%#GeoPos(Convert.ToString(Eval("gps_latitud")), Convert.ToString(Eval("gps_longitud")))%>
                                    <br />
                                    <%#Convert.ToString(Eval("gps_latitud")).Replace(",", ".")%>,<%#Convert.ToString(Eval("gps_longitud")).Replace(",", ".")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Width="20px" />
                                <HeaderTemplate>
                                    Vel.
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%#IIf(Convert.ToString(Eval("evento")).ToLowerInvariant().Contains("detenido") Or Convert.ToString(Eval("evento")).ToLowerInvariant().Contains("apagado"), "0", Eval("gps_velocidad"))%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Width="100px" />
                                <HeaderTemplate> Rumbo </HeaderTemplate>
                                <ItemTemplate>
                                    <%#Eval("gps_rumbo") %>° <%# Eval("rumbo") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Width="120px" />
                                <HeaderTemplate>
                                    Evento
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <label style='background-color:<%#Eval("color_evento") %>'><%#Eval("evento")%></label>
                                </ItemTemplate>
                                <ItemStyle ForeColor="White"/>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Width="120px" />
                                <HeaderTemplate>
                                    Referencia cercana  
                                </HeaderTemplate>
                                <ItemTemplate>
                                        <%#gGetReferenciaCercana(Convert.ToDouble(Eval("gps_latitud")), Convert.ToDouble(Eval("gps_longitud")))%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            </Columns>
                            <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                            <PagerStyle BackColor="Navy" ForeColor="White" HorizontalAlign="Center" />
                            <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                            <HeaderStyle BackColor="#008400" Font-Bold="True" ForeColor="White" Font-Size="Smaller" />
                            <AlternatingRowStyle BackColor="#BABABA" />
                            </asp:GridView>
                    </div>
                    <div class="divBusVUELTA">
                    <asp:GridView PageSize="20"  ID="gv_ListaVehiculos_VUELTA" runat="server" 
                           DataSourceID="sqlVehiculosVuelta" AllowPaging="True" 
                            DataKeyNames="identificador_rastreo, id_vehiculo, gps_longitud, gps_latitud"
                            AllowSorting="True" AutoGenerateColumns="False" BackColor="White" 
                            BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" 
                            GridLines="Vertical" Width="995px" Font-Names="Nina, Segoe, Arial Narrow, Arial, Verdana"
                            HeaderStyle-Font-Names="Nina, Segoe, Arial Narrow, Arial, Verdana"
                            Font-Size="Small" EmptyDataText="No tiene vehiculos asignados o en viaje de IDA."
                            EmptyDataRowStyle-CssClass="Mensaje_de_error">
                            <EmptyDataRowStyle CssClass="Mensaje_de_error" HorizontalAlign="Center" />
                            <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                            <Columns>
                            <asp:TemplateField> 
                                <ItemStyle Width="70px" />
                                <HeaderTemplate> Vehiculo </HeaderTemplate>
                                <ItemTemplate>
                                    <ajaxToolkit:HoverMenuExtender runat="server" ID="masterHuverTareas" TargetControlID="lnkTareas" PopupControlID="pnlTareas" PopupPosition="Right" OffsetX="5" OffsetY="-15">
                                    </ajaxToolkit:HoverMenuExtender>
                                    <asp:Label runat="server" ID="lnkTareas">
                                    <a title="Centrar el móvil <%#Eval("identificador_rastreo")%> " href="#toptop" onclick="busvuelta_map_panTo(<%# Convert.ToString(Eval("gps_longitud")).Replace(",",".") %>,<%# Convert.ToString(Eval("gps_latitud")).Replace(",",".") %>);">
                                      <%#Eval("identificador_rastreo")%> 
                                    </a>
                                    </asp:Label>
                                    <asp:Panel BorderColor="Black" BorderWidth="2px" BackColor="Navy" Font-Size="X-Small" ForeColor="White" ID="pnlTareas" runat="server">
                                        <a title="Haga click aqui para obtener los recorridos del dia del móvil <%#Eval("identificador_rastreo")%> " class="OpcionTarea" onclick="AbrirDialogo('bus_historico.aspx?vid=<%#Eval("id_vehiculo")%>')">
                                         Recorridos del día
                                        </a>
                                        <br />
                                        <a title="Haga click aqui para obtener el recorrido histórico del móvil <%#Eval("identificador_rastreo")%> " class="OpcionTarea" onclick="AbrirDialogo('historico.aspx?vid=<%#Eval("id_vehiculo")%>')">
                                         Historicos
                                        </a>
                                        <br />
                                        <% If permiso_GEOCERCA Then%>
                                        <a title="Haga click aqui para crear geocercas para el vehiculo <%#Eval("identificador_rastreo")%> " class="OpcionTarea" onclick="AbrirDialogo('Geocercas.aspx?uid=<%#Eval("id_usuarios")%>&cid=<%#Eval("id_cliente") %>&vid=<%#Eval("id_vehiculo") %>')">
                                         GeoCerca
                                        </a>
                                        <br />
                                        <% End if %></asp:Panel>
                                    <ajaxToolkit:HoverMenuExtender runat="server" ID="masterHuverDatos" TargetControlID="lnkTareas" PopupControlID="pnlDatos" PopupPosition="Bottom" OffsetX="150" OffsetY="-30">
                                    </ajaxToolkit:HoverMenuExtender>
                                    <asp:Panel BorderColor="Black" BorderWidth="2px" BackColor="DarkGreen" Font-Size="X-Small" ForeColor="White" ID="pnlDatos" runat="server">
                                        <table cellpadding="0" cellspacing="0">
                                            <tr><td>Modelo:</td><td><%#Eval("marca")%></td></tr>
                                            <tr><td>Marca:</td><td><%#Eval("modelo")%></td></tr>
                                            <tr><td>Patente:</td><td><%#Eval("matricula")%></td></tr>
                                        </table>
                                    </asp:Panel>
                                </ItemTemplate>
                            </asp:TemplateField>                        
                            <asp:BoundField HeaderText="Fecha y hora" DataField="gps_fecha" ItemStyle-Width="100px" />
                            <asp:TemplateField>
                                <ItemStyle Width="350px" />
                                <HeaderTemplate>
                                    Direccion cercana  
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%#GeoPos(Convert.ToString(Eval("gps_latitud")), Convert.ToString(Eval("gps_longitud")))%>
                                    <br />
                                    <%#Convert.ToString(Eval("gps_latitud")).Replace(",", ".")%>,<%#Convert.ToString(Eval("gps_longitud")).Replace(",", ".")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Width="20px" />
                                <HeaderTemplate>
                                    Vel.
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%#IIf(Convert.ToString(Eval("evento")).ToLowerInvariant().Contains("detenido") Or Convert.ToString(Eval("evento")).ToLowerInvariant().Contains("apagado"), "0", Eval("gps_velocidad"))%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Width="100px" />
                                <HeaderTemplate> Rumbo </HeaderTemplate>
                                <ItemTemplate>
                                    <%#Eval("gps_rumbo") %>° <%# Eval("rumbo") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Width="120px" />
                                <HeaderTemplate>
                                    Evento
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <label style='background-color:<%#Eval("color_evento") %>'><%#Eval("evento")%></label>
                                </ItemTemplate>
                                <ItemStyle ForeColor="White"/>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Width="120px" />
                                <HeaderTemplate>
                                    Referencia cercana  
                                </HeaderTemplate>
                                <ItemTemplate>
                                        <%#gGetReferenciaCercana(Convert.ToDouble(Eval("gps_latitud")), Convert.ToDouble(Eval("gps_longitud")))%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            </Columns>
                            <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                            <PagerStyle BackColor="Navy" ForeColor="White" HorizontalAlign="Center" />
                            <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                            <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" Font-Size="Smaller" />
                            <AlternatingRowStyle BackColor="#BABABA" />
                         </asp:GridView>
                    </div>
                    <asp:SqlDataSource ID="sqlVehiculosIDA" runat="server" ConnectionString="<%$ ConnectionStrings:CS_RASTREOSystem %>" ProviderName="<%$ ConnectionStrings:CS_RASTREOSystem.ProviderName %>"
                        SelectCommand="SELECT DISTINCT ON (identificador_rastreo) * FROM rsview_vehiculo_bandejaentrada_cliente_equipogps WHERE idrastreo_usuarios = @id_usuarios AND gps_obs ILIKE '%IDA%'">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="idUsuario" Name="id_usuarios"/>
                        </SelectParameters>
                    </asp:SqlDataSource>
                    <asp:SqlDataSource ID="sqlVehiculosVUELTA" runat="server" ConnectionString="<%$ ConnectionStrings:CS_RASTREOSystem %>" ProviderName="<%$ ConnectionStrings:CS_RASTREOSystem.ProviderName %>"
                        SelectCommand="SELECT DISTINCT ON (identificador_rastreo) * FROM rsview_vehiculo_bandejaentrada_cliente_equipogps WHERE idrastreo_usuarios = @id_usuarios AND gps_obs ILIKE '%VUELTA%'">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="idUsuario" Name="id_usuarios"/>
                        </SelectParameters>
                    </asp:SqlDataSource>
                </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        </table>
    </div>

    <%--<%If Request.ServerVariables("REMOTE_HOST") = "190.104.156.142" Then%>--%>
        <script src="http://maps.google.com/maps?file=api&amp;v=2&amp;sensor=false&amp;key=ABQIAAAA_r2VgoolhB6iO9xSBULQFxTPNvAASRBEw_TMHTynVLwSexQB8hQ4RTPwWf589F08aFbkKIvY434rxQ" type="text/javascript"></script> 
    <%--<%ElseIf Request.ServerVariables("REMOTE_HOST") = "190.104.153.61" Then%>
        <script src="http://maps.google.com/maps?file=api&amp;v=2&amp;sensor=false&amp;key=ABQIAAAA_r2VgoolhB6iO9xSBULQFxQjCxOHmMAjVGNZYZATNfcVIoBu7hQt5fsrP_aWckxRC585DEwI98L-ow" type="text/javascript"></script>
    <%ElseIf Request.ServerVariables("REMOTE_HOST") = "127.0.0.1" Then%>
        <script src="http://maps.google.com/maps?file=api&amp;v=2&amp;sensor=false&amp;key=ABQIAAAA_r2VgoolhB6iO9xSBULQFxQ9pzGUvE4e-uE6JwppIOBVvhvAmRQysIA9otMz1DX9na4y4LGsV3YqDA" type="text/javascript"></script>
    <%End If%>--%>
    <script type="text/javascript" src="OpenLayers.js"></script>
    <script type="text/javascript" src="FuncionesGPS.js"></script>
    <%--<script type="text/javascript" src="google_corners/popup.js"></script>--%>
    <%--<script type="text/javascript" src="http://www.openlayers.net/api/OpenLayers.js"></script>--%>
    <%--<script src='http://dev.virtualearth.net/mapcontrol/mapcontrol.ashx?v=6.1'></script>--%>
    <%--<script src="http://api.maps.yahoo.com/ajaxymap?v=3.0&appid=euzuro-openlayers"></script>--%>

    </form>

</body>
</html>




