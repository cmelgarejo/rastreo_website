<%@ Page Title="RASTREO Paraguay - Posiciones GPS" EnableEventValidation="true" Language="VB"
    AutoEventWireup="false" CodeFile="GPSadmin.aspx.vb" Inherits="GPSadmin" %>

<%--<%@ Register Src="~/SoundPlayerI.ascx"
    TagPrefix="soundplayer"
    TagName="SoundPlayerI" %>--%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<!-- Autor: Ing. Adolfo Bresanovich || Director de R&D RASTREO Paraguay -->
<head id="GPSadminHead" runat="server">
    <link rel="shortcut icon" href="~/RASTREO.ico" type="image/x-icon" />
    <link rel="icon" href="~/RASTREO.ico" type="image/x-icon" />
    <title>RASTREO WebSystem - Bienvenid@!</title>
    <script type="text/javascript" src="http://maps.google.com/maps/api/js?v=3.2&sensor=false"></script>
    <script type="text/javascript" src="FuncionesGenerales.js"></script>
</head>
<body onload="GPSadmin_init_map();countdown_timer(120);">
    <div id="toptop" visible="false" />
    <form id="RASTREOmainForm" runat="server">
    <asp:UpdateProgress runat="server" ID="updpg_pnlGPS" AssociatedUpdatePanelID="pnlMainGPS">
        <ProgressTemplate>
            <div style="position: absolute; left: 0px; top: 0px; width: 100%; height: 1000%"
                class="modalBackground">
                <img src="./App_Themes/CENTRAL/Imagenes/ajax-loader.gif" alt="-" />
                <label class="rastreo_nowloading">
                    Actualizando...</label>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <ajaxToolkit:ToolkitScriptManager ID="RASTREOToolScriptManager" ScriptMode="Release"
        runat="server">
        <Scripts>
            <asp:ScriptReference Path="~/WebKit.js" />
        </Scripts>
    </ajaxToolkit:ToolkitScriptManager>
    <asp:HiddenField ID="idPersona" runat="server" Value="0" />
    <asp:HiddenField ID="idVehiculoSeleccionado" runat="server" Value="0" />
    <div id="RASTREO_myHeader_RASTREAR">
        <%--
            Calcula distancia entre dos puntos
            <a onclick="alert(OpenLayers.Util.distVincenty(new OpenLayers.LonLat(-25.33044,-57.59606),
            new OpenLayers.LonLat(-25.33979,-57.58446)));">calc</a>
        --%>
        <!-- CABECERA-->
        <div id="top">
            <div class="logo">
                <a href="#">
                    <img alt="RASTREAR - The tracking solution" src="App_Themes/CENTRAL/Imagenes/logo.png"
                        width="282" height="95" /></a></div>
            <div class="menutop">
                <div class="menubox1">
                    <div class="menuitem">
                        Usuario: <span class="menublanco">
                            <% Response.Write(mi_usuario)%></span></div>
                    <div class="menuitemLOGOUT">
                        <img alt=" X " src="App_Themes/CENTRAL/Imagenes/sailir.jpg" align="middle" />
                        <asp:LinkButton runat="server" ID="RASTREO_LOGOUT">Salir del sistema</asp:LinkButton>
                    </div>
                </div>
                <div class="menubox2">
                    <div>
                        <label style="font-size: smaller">
                            Clientes Activos:
                        </label>
                        <asp:Label runat="server" ID="lbCantidadClientes" ForeColor="Black" />
                        <label style="font-size: smaller">
                            Vehiculos Activos:
                        </label>
                        <asp:Label runat="server" ID="lbCantidadVehiculos" ForeColor="Black" />
                        <label style="font-size: smaller">
                            Vehiculos DEMO:
                        </label>
                        <asp:Label runat="server" ID="lbCantidadVehiculosDEMO" ForeColor="Black" />
                    </div>
                    <div class="menuitem2">
                        Próxima Actualización en
                        <input id="cd_timer" class="sec" value="" readonly="readonly" />
                        segundos |
                        <asp:TextBox runat="server" ID="txtTimeout" CssClass="sec2" onchange="if (isNaN(this.value)) { this.value='60'; this.focus()}"
                            Width="30px" Text="120" MaxLength="3" />
                        <asp:Button UseSubmitBehavior="false" runat="server" CssClass="btn" ID="btnAjustar"
                            Text="Ajustar intervalo" OnClientClick="DisableSave(this);" />
                        |
                        <asp:UpdatePanel runat="server" ID="updpnlListaSendMail">
                            <ContentTemplate>
                                <input value="Administrar Referencias" type="button" title="Haga click aqui para crear hojas de referencia que contendran puntos de referencia a ser utilizados en las hojas de ruta"
                                    class="btn" onclick="AbrirDialogo('RReferencias.aspx?uid=<%= _gidUsuario %>&cid=<%= _gidPersona %>')" />
                                <asp:Button UseSubmitBehavior="false" runat="server" ID="btn_OpenSendMail" CssClass="btn"
                                    Text="Enviar posicion por mail" OnClientClick="get_g_zoom();set_sendingmail(true);" />
                                <ajaxToolkit:ModalPopupExtender ID="PopupMAIL" runat="server" BackgroundCssClass="modalBackground"
                                    TargetControlID="btn_OpenSendMail" PopupControlID="pnlSendMail" OkControlID="btnCloseMail" />
                                <asp:Panel CssClass="PopUp" ID="pnlSendMail" runat="server">
                                    <asp:UpdateProgress runat="server" ID="updpnlListaSendMailprogress" AssociatedUpdatePanelID="updpnlListaSendMail">
                                        <ProgressTemplate>
                                            <img src="./App_Themes/CENTRAL/Imagenes/ajax-loader.gif" alt="-" />
                                            <label class="rastreo_nowloading">
                                                Aguarde...</label>
                                        </ProgressTemplate>
                                    </asp:UpdateProgress>
                                    <table width="100%" cellpadding="0" cellspacing="0" class="Label">
                                        <tr>
                                            <td>
                                                Destinatario(s)
                                                <br />
                                                [separarlos por <b>;</b> o <b>,</b>]:
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="mailsSEND" TextMode="MultiLine" CssClass="TextBoxMAIL"
                                                    Width="200px" Height="50px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                Vehiculos disponibles:<asp:DropDownList runat="server" ID="ddlSendMailTo" ToolTip="Elija que de vehiculo enviar posicion"
                                                    CssClass="DropDownList" />
                                                <asp:Button runat="server" ID="btnAgregarVehiculo" Text="Agregar vehiculo a la lista"
                                                    CssClass="btn1" />
                                                <br />
                                                Lista de vehiculos
                                                <br />
                                                <asp:ListBox runat="server" ID="listVehiculosInvolucrados" Width="100%" CssClass="DropDownList" />
                                                <asp:Button runat="server" ID="btnEliminarVehiculo" Text="Eliminar vehiculo de la lista"
                                                    CssClass="btn1" OnClientClick="return seguro_que();" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:Button runat="server" ID="btnSendMail" CssClass="btn" Text="Enviar posicion"
                                                    OnClientClick="set_sendingmail(false);" />
                                                <asp:Button runat="server" ID="btnCloseMail" CssClass="btn" Text="Cancelar" OnClientClick="set_sendingmail(false);" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
        <!-- CABECERA-->
        <script type="text/javascript" src="js/stmenu.js"></script>
        <div id="divMENU" style="position: absolute; top: 72px; left: 0px; width: 300px"
            align="center">
            <%  If permiso_menu_admin Then%>
            <%-- Menu administracion --%>
            <script type="text/javascript">
                        <!--
                stm_bm(["menu0eae", 900, "", "./App_Themes/CENTRAL/Imagenes/blank.gif", 0, "", "", 0, 0, 0, 0, 50, 1, 0, 0, "", "650px", 0, 0, 1, 1, "default", "hand", "", 1, 25], this);
                stm_bp("p0", [0, 4, 0, 0, 0, 2, 1, 0, 100, "progid:DXImageTransform.Microsoft.Wipe(GradientSize=1.0,wipeStyle=1,motion=forward,enabled=0,Duration=0.20)", 5, "", -2, 90, 0, 0, "#7F7F7F", "transparent", "./App_Themes/CENTRAL/Imagenes/050307.jpg", 3, 1, 1, "#999999"]);
                stm_ai("p0i0", [2, "", "./rastreo.ico", "./rastreo.ico", 16, 16, 0, "", "_self", "", "", "", "", 0, 0, 0, "", "", 0, 0, 0, 0, 1, "#FFFFFF", 1, "#FFFFFF", 1, "", "", 3, 3, 0, 0, "#FFFFFF", "#FFFFFF", "#333333", "#FFFFFF", "bold 9pt Arial", "bold 9pt Arial", 0, 0, "", "", "", "", 0, 0, 0], 30, 0);
                stm_aix("p0i1", "p0i0", [0, "Administración", "", "", -1, -1, 0, "", "_self", "", "", "", "", 1, 0, 0, "", "", 0, 0, 0, 1, 1, "#8D9CC4", 1, "#FFFFFF", 1, "", "", 0, 0, 0, 0, "#FFFFFF", "#FFFFFF", "#000000", "#FFFFFF", "bold 9pt 'Arial','Verdana'"], 80, 0);
                stm_bpx("p1", "p0", [1, 4, 0, 2, 0, 4, 1, 0, 100, "progid:DXImageTransform.Microsoft.Wipe(GradientSize=1.0,wipeStyle=1,motion=forward,enabled=0,Duration=0.50)", 5, "progid:DXImageTransform.Microsoft.Fade(overlap=.5,enabled=0,Duration=0.50)", -2, 60, 2, 6, "#CCCCCC", "#F3F2F9", "", 0]);
                stm_aix("p1i0", "p0i0", [0, "Personas", "", "", -1, -1, 0, "./rastreo_admin/admin_personas_lista.aspx", "_self", "", "Aqui puede dar de alta o ubicar una persona y editar todos los datos asociados a la misma.", "", "", 1, 0, 0, "", "", 0, 0, 0, 0, 1, "#0000FF", 1, "#DCDAED", 0, "", "", 0, 0, 0, 0, "#FFFFFF", "#FFFFFF", "#333333", "#0000FF"]);
                stm_aix("p1i1", "p1i0", [0, "Vehiculos", "", "", -1, -1, 0, "./rastreo_admin/admin_cliente_vehiculo_lista.aspx", "_self", "", "Aqui puede editar los vehiculos pertenecientes a un cliente especifico."]);
                stm_ep();
                stm_aix("p0i2", "p0i0", [0, "GPS", "", "", -1, -1, 0, "", "_self", "", "", "", "", 1, 0, 0, "", "", 0, 0, 0, 1, 1, "#8D9CC4", 1, "#FFFFFF", 1, "", "", 0, 0, 0, 0, "#FFFFFF", "#FFFFFF", "#000000"], 60, 0);
                stm_bpx("p2", "p1", []);
                stm_aix("p2i0", "p1i0", [0, "RASTREAR", "", "", -1, -1, 0, "./GPSadmin.aspx", "_self", "", "Haga click aqui para empezar a RASTREAR."]);
                stm_aix("p2i1", "p1i0", [0, "Tipos de Equipo GPS", "", "", -1, -1, 0, "./rastreo_admin/admingps_tipoequipo.aspx", "_self", "", "Es aqui donde puede dar de alta un equipo GPS."]);
                stm_aix("p2i2", "p1i0", [0, "Equipos GPS", "", "", -1, -1, 0, "./rastreo_admin/admingps_equipogps_lista.aspx", "_self", "", "Es aqui donde puede dar de alta un equipo GPS."]);
                stm_aix("p2i3", "p1i0", [0, "Eventos de Tipo Equipo GPS", "", "", -1, -1, 0, "./rastreo_admin/admingps_eventos_lista.aspx", "_self", "", "", "", "", 1, 0, 0, "", "", 0, 0, 0, 0, 1, "#FFFFFF"]);
                stm_aix("p2i4", "p1i0", [0, "Envio de comandos a equipos GPS", "", "", -1, -1, 0, "Envio_de_Comandos.aspx", "_blank", "", "Es aqui donde puede enviar comandos a los equipo GPS."]);
                stm_ep();
                stm_ep();
                stm_em();
                        //-->
            </script>
            <% End If%>
        </div>
    </div>
    <div id="RASTREO_mainwrapper">
        <asp:Label ID="errMsgs" runat="server" Width="99%" CssClass="Mensaje_de_error" Visible="false" />
        <div align="center">
            <input enableviewstate="true" class="TextBox" id="txtsrch" style="width: 65%" /><input
                class="btn1" value="Buscar direccion" style="font-size: smaller" onclick="googleFindAddress(GetValorFromObj('txtsrch'));return false;"
                type="submit" /></div>
        <div id="map">
        </div>
        <asp:UpdatePanel runat="server" ID="pnlMainGPS">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnForceUpdate" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnFiltrarGrilla_Por_Cliente" EventName="Click" />
            </Triggers>
            <ContentTemplate>
                <asp:HiddenField ID="txtfile" runat="server" />
                <asp:HiddenField ID="reffile" runat="server" />
                <asp:Timer runat="server" ID="tmr_Reload" Interval="120000">
                </asp:Timer>
                <asp:Button UseSubmitBehavior="false" runat="server" ID="btnForceUpdate" Text="Actualizar ahora."
                    Font-Size="X-Small" CssClass="rastreo_botones_save" />
                <asp:CheckBox runat="server" ID="chkFiltro_Todos_Los_Moviles" AutoPostBack="true"
                    Text="Ver todos los vehiculos" Checked="false" />
                <table width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <asp:CheckBox runat="server" ID="chkFiltro_Por_Cliente" AutoPostBack="true" Text="Filtro por Cliente"
                                Checked="true" />
                        </td>
                        <td>
                            <asp:Panel runat="server" ID="pnlFiltro_Por_Cliente" Visible="true">
                                <asp:DropDownList runat="server" ID="ddlClienteSeleccionado" CssClass="TextBox" AutoPostBack="true">
                                </asp:DropDownList>
                                <asp:ImageButton runat="server" ID="btnFiltrarGrilla_Por_Cliente" ImageUrl="~/App_Themes/CENTRAL/Imagenes/view-refresh.png" />
                            </asp:Panel>
                        </td>
                        <td>
                            <asp:CheckBox runat="server" ID="chkFiltro_Por_Identificador" AutoPostBack="true"
                                Text="Filtro por Identificador" />
                        </td>
                        <td>
                            <asp:Panel runat="server" ID="pnlFiltro_Por_Identificador" Visible="true">
                                <asp:DropDownList runat="server" ID="ddlIdentificadorSeleccionado" CssClass="TextBox"
                                    AutoPostBack="true">
                                </asp:DropDownList>
                                <asp:ImageButton runat="server" ID="btnFiltro_Por_Identificador" ImageUrl="~/App_Themes/CENTRAL/Imagenes/view-refresh.png" />
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <div id="div_mainGPS_grid">
                                <asp:Panel runat="server" ID="pnlVehiculoSeleccionado" BorderColor="Navy" BorderStyle="Solid"
                                    BorderWidth="2px">
                                    <asp:GridView PageSize="1" ID="gridVehiculoSeleccionado" runat="server" DataSourceID="sqldsVehiculoSeleccionado"
                                        AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" BackColor="White"
                                        BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Vertical"
                                        Width="100%" Font-Names="Nina, Segoe, Arial Narrow, Arial, Verdana" Font-Size="XX-Small"
                                        EmptyDataRowStyle-CssClass="Mensaje_de_error">
                                        <EmptyDataRowStyle CssClass="Mensaje_de_error" HorizontalAlign="Center" />
                                        <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                                        <Columns>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    Identificador
                                                </HeaderTemplate>
                                                <ItemStyle CssClass="rastreo_nowloading" Font-Size="X-Small" />
                                                <ItemTemplate>
                                                    <ajaxToolkit:HoverMenuExtender runat="server" ID="masterHuver" TargetControlID="lnkDatos"
                                                        PopupControlID="pnlDatos" PopupPosition="Right" OffsetY="-105" OffsetX="10">
                                                    </ajaxToolkit:HoverMenuExtender>
                                                    <asp:Panel BorderColor="Black" BorderWidth="2px" BackColor="Black" Font-Size="XX-Small"
                                                        ForeColor="White" ID="pnlDatos" runat="server">
                                                        <table cellpadding="0" cellspacing="0">
                                                            <tr>
                                                                <td>
                                                                    <a title="Contactos de <%#Eval("cliente") %>" onclick="AbrirDialogo('rastreo_admin/admin_cliente_contacto_lista.aspx?id=<%# Eval("idrastreo_persona") %>')">
                                                                        Contactos</a>
                                                                </td>
                                                                <td>
                                                                    <a title="Editar móvil <%#Eval("identificador_rastreo") %>" onclick="AbrirDialogo('rastreo_admin/admin_cliente_vehiculo.aspx?cid=<%# Eval("idrastreo_persona") %>&id=<%# Eval("idrastreo_vehiculo") %>')">
                                                                        Editar movil</a>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Chofer:
                                                                </td>
                                                                <td>
                                                                    <%#Eval("chofer")%>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Contacto:
                                                                </td>
                                                                <td>
                                                                    <%#Eval("chofer_contacto")%>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Titular:
                                                                </td>
                                                                <td>
                                                                    <%#Eval("cliente")%>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Telefono movil:
                                                                </td>
                                                                <td>
                                                                    <%#Eval("tel_movil")%>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Telefono particular:
                                                                </td>
                                                                <td>
                                                                    <%#Eval("tel_part")%>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Telefono oficina:
                                                                </td>
                                                                <td>
                                                                    <%#Eval("tel_ofi")%>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    e-mails:
                                                                </td>
                                                                <td>
                                                                    <%#Eval("email")%>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <%#Eval("marca")%>
                                                                </td>
                                                                <td>
                                                                    <%#Eval("modelo")%>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Patente:
                                                                </td>
                                                                <td>
                                                                    <%#Eval("matricula")%>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Tipo Equipo GPS:
                                                                </td>
                                                                <td>
                                                                    <%#Eval("tipo_equipo")%>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    ID Equipo GPS:
                                                                </td>
                                                                <td>
                                                                    <%#Eval("id_equipo_gps")%>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    SIM Nro:
                                                                </td>
                                                                <td>
                                                                    <%#Eval("sim_nro")%>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                    <a href="#toptop" onclick="map_panTo(<%# Convert.ToString(Eval("gps_longitud")).Replace(",",".") %>,<%# Convert.ToString(Eval("gps_latitud")).Replace(",",".") %>);">
                                                        <asp:Label CssClass="btn1" runat="server" ID="lnkDatos"><%#Eval("identificador_rastreo")%></asp:Label>
                                                    </a>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    Tareas
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <ajaxToolkit:HoverMenuExtender runat="server" ID="masterHuverTareas" TargetControlID="lnkTareas"
                                                        PopupControlID="pnlTareas" PopupPosition="Right" OffsetY="-108">
                                                    </ajaxToolkit:HoverMenuExtender>
                                                    <asp:Label runat="server" ID="lnkTareas" CssClass="btn1" Font-Size="X-Small">Tareas</asp:Label>
                                                    <asp:Panel BorderColor="Black" BorderWidth="2px" BackColor="RoyalBlue" Font-Size="X-Small"
                                                        ForeColor="White" ID="pnlTareas" runat="server">
                                                        <a class="OpcionTarea" href="#toptop" onclick="map_panTo(<%# Convert.ToString(Eval("gps_longitud")).Replace(",",".") %>,<%# Convert.ToString(Eval("gps_latitud")).Replace(",",".") %>)">
                                                            Posicionar </a>
                                                        <br />
                                                        <a title="Haga click aqui para obtener el recorrido histórico del vehiculo <%#Eval("identificador_rastreo")%>"
                                                            class="OpcionTarea" onclick="AbrirDialogo('Historico.aspx?vid=<%#Eval("idrastreo_vehiculo")%>')">
                                                            Histórico </a>
                                                        <br />
                                                        <a title="click aqui para enviar comando al vehiculo <%#Eval("identificador_rastreo")%>"
                                                            class="OpcionTarea" onclick="AbrirDialogo('Envio_de_Comandos.aspx?eqid=<%#Eval("id_equipogps")%>',800,600)">
                                                            Enviar comando al equipo </a>
                                                        <br />
                                                        <a title="Haga click aqui para crear hojas de ruta para el vehiculo <%#Eval("identificador_rastreo")%> "
                                                            class="OpcionTarea" onclick="AbrirDialogo('Hoja_de_ruta.aspx?uid=<%#Eval("id_usuarios")%>&cid=<%#Eval("id_cliente") %>&vid=<%#Eval("id_vehiculo") %>')">
                                                            Hoja de Ruta </a>
                                                        <br />
                                                        <a title="Haga click aqui para crear geocercas para el vehiculo <%#Eval("identificador_rastreo")%> "
                                                            class="OpcionTarea" onclick="AbrirDialogo('Geocercas.aspx?uid=<%#Eval("id_usuarios")%>&cid=<%#Eval("id_cliente") %>&vid=<%#Eval("id_vehiculo") %>')">
                                                            GeoCerca </a>
                                                        <br />
                                                    </asp:Panel>
                                                    <ajaxToolkit:HoverMenuExtender runat="server" ID="masterHover" TargetControlID="lnkTareas"
                                                        PopupControlID="pnlVer" PopupPosition="Top" OffsetX="-75">
                                                    </ajaxToolkit:HoverMenuExtender>
                                                    <asp:Panel BorderColor="Black" BorderWidth="2px" BackColor="#024579" ID="pnlVer"
                                                        runat="server">
                                                        <a title="Contactos de <%#Eval("cliente") %>" href="rastreo_admin/admin_cliente_contacto_lista.aspx?id=<%# Eval("idrastreo_persona") %>">
                                                            Contactos</a>
                                                        <br />
                                                        <a title="Documentos relacionados con <%#Eval("cliente") %>" href="rastreo_admin/admin_cliente_documentos.aspx?cid=<%# Eval("idrastreo_persona") %>">
                                                            Documentos</a>
                                                        <br />
                                                        <a title="Permisos del usuario <%#Eval("usuario") %>" href="rastreo_admin/admingps_opciones_usuario_lista.aspx?pid=<%# Eval("idrastreo_persona") %>">
                                                            Permisos</a>
                                                        <br />
                                                        <a title="Historial de eventos relacionados con <%#Eval("cliente") %>" href="rastreo_admin/admin_historial.aspx?cid=<%# Eval("idrastreo_persona") %>">
                                                            Historial de Eventos</a></asp:Panel>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField SortExpression="gps_fecha" HeaderText="Fecha y hora" DataField="gps_fecha"
                                                DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" />
                                            <%--<asp:BoundField  HeaderText="Latitud" DataField="gps_latitud" />
                                    <asp:BoundField  HeaderText="Longitud" DataField="gps_longitud" />--%>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    Direccion cercana
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <%#GeoPos(Convert.ToString(Eval("gps_latitud")), Convert.ToString(Eval("gps_longitud")), Convert.ToString(Eval("gps_dir")))%>
                                                    <br />
                                                    <%#Convert.ToString(Eval("gps_latitud")).Replace(",", ".")%>,<%#Convert.ToString(Eval("gps_longitud")).Replace(",", ".")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="Velocidad" DataField="gps_velocidad" />
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    Rumbo
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <%#Eval("gps_rumbo") %>°
                                                    <%# Eval("rumbo") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <label style='background-color: <%#Eval("color_evento") %>'>
                                                        <%#Eval("evento")%></label>
                                                </ItemTemplate>
                                                <ItemStyle ForeColor="White" />
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    Referencia cercana
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <%# mainGetReferenciaCercana(Convert.ToString(Eval("gps_latitud")), Convert.ToString(Eval("gps_longitud")))%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    Entradas I/O</HeaderTemplate>
                                                <ItemTemplate>
                                                    <ajaxToolkit:HoverMenuExtender runat="server" ID="statusIOHoverSEL" TargetControlID="lnkIOSEL"
                                                        PopupControlID="pnlStatusIOSEL" PopupPosition="Left" OffsetY="-45" OffsetX="0">
                                                    </ajaxToolkit:HoverMenuExtender>
                                                    <asp:Panel BorderColor="Black" BorderWidth="2px" BackColor="Black" Font-Size="X-Small"
                                                        ForeColor="White" ID="pnlStatusIOSEL" runat="server">
                                                        <%#IIf(Convert.ToString(Eval("tipo_de_reporte")).Contains("TRAX"), Convert.ToString(GetStatusIO(Convert.ToString(Eval("gps_estado_io")), Convert.ToString(Eval("gps_edaddeldato")), Convert.ToString(Eval("gps_tipodeposicion")))), String.Empty)%></asp:Panel>
                                                    <asp:Label runat="server" ID="lnkIOSEL" CssClass="btn1" ToolTip="">STATUS</asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                                        <PagerStyle BackColor="Navy" ForeColor="White" HorizontalAlign="Center" />
                                        <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                                        <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
                                        <AlternatingRowStyle BackColor="#BABABA" />
                                    </asp:GridView>
                                    <asp:SqlDataSource ID="sqldsVehiculoSeleccionado" runat="server" ConnectionString="<%$ ConnectionStrings:CS_RASTREOSystem %>"
                                        ProviderName="<%$ ConnectionStrings:CS_RASTREOSystem.ProviderName %>" SelectCommand="SELECT DISTINCT ON (identificador_rastreo) * FROM rsview_vehiculo_bandejaentrada_cliente_equipogps WHERE idrastreo_vehiculo = @idVehiculoSeleccionado">
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="idVehiculoSeleccionado" Name="idVehiculoSeleccionado"
                                                ConvertEmptyStringToNull="false" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                </asp:Panel>
                                <asp:GridView ID="gv_Lista_Vehiculos" runat="server" PageSize="30" DataKeyNames="idrastreo_vehiculo, idrastreo_persona, gps_longitud, gps_latitud"
                                    DataSourceID="sqlds_Lista_Vehiculos" AllowPaging="True" PagerSettings-Mode="NumericFirstLast"
                                    PagerSettings-Position="TopAndBottom" AllowSorting="True" AutoGenerateColumns="False"
                                    BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px"
                                    CellPadding="3" GridLines="Vertical" Width="100%" Font-Names="Nina, Segoe, Arial Narrow, Arial, Verdana"
                                    Font-Size="XX-Small" EmptyDataText="-= No existen vehiculos para la seleccion actual =-">
                                    <EmptyDataRowStyle CssClass="Mensaje_de_error" HorizontalAlign="Center" />
                                    <RowStyle BackColor="#EEEEEE" ForeColor="Black" VerticalAlign="Top" />
                                    <Columns>
                                        <asp:CommandField SelectText="+" ShowSelectButton="true" ButtonType="Button" ControlStyle-Font-Size="X-Small"
                                            ControlStyle-CssClass="btnplus" HeaderText="Seguimiento" />
                                        <asp:TemplateField>
                                            <ItemStyle Width="70px" HorizontalAlign="Center" />
                                            <HeaderTemplate>
                                                Identificador
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <ajaxToolkit:HoverMenuExtender runat="server" ID="masterHuver" TargetControlID="lnkDatos"
                                                    PopupControlID="pnlDatos" PopupPosition="Right" OffsetY="-70" OffsetX="5" HoverDelay="700">
                                                </ajaxToolkit:HoverMenuExtender>
                                                <asp:Panel BorderColor="Black" BorderWidth="2px" BackColor="RoyalBlue" Font-Size="XX-Small"
                                                    ForeColor="White" ID="pnlDatos" runat="server">
                                                    <table cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td>
                                                                <a title="Contactos de <%#Eval("cliente") %>" onclick="AbrirDialogo('rastreo_admin/admin_cliente_contacto_lista.aspx?id=<%# Eval("idrastreo_persona") %>')">
                                                                    Contactos</a>
                                                            </td>
                                                            <td>
                                                                <a title="Editar móvil <%#Eval("identificador_rastreo") %>" onclick="AbrirDialogo('rastreo_admin/admin_cliente_vehiculo.aspx?cid=<%# Eval("idrastreo_persona") %>&id=<%# Eval("idrastreo_vehiculo") %>')">
                                                                    Editar movil</a>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <%#Eval("marca")%>
                                                            </td>
                                                            <td>
                                                                <%#Eval("modelo")%>
                                                            </td>
                                                        </tr>
                                                        <td>
                                                            Patente:
                                                        </td>
                                                        <td>
                                                            <%#Eval("matricula")%>
                                                        </td>
                                                        <tr>
                                                            <td>
                                                                Chofer:
                                                            </td>
                                                            <td>
                                                                <%#Eval("chofer")%>
                                                            </td>
                                                            <td>
                                                                Contacto:
                                                            </td>
                                                            <td>
                                                                <%#Eval("chofer_contacto")%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                Titular:
                                                            </td>
                                                            <td>
                                                                <%#Eval("cliente")%>
                                                            </td>
                                                            <td>
                                                                Telefono movil:
                                                            </td>
                                                            <td>
                                                                <%#Eval("tel_movil")%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                Telefono particular:
                                                            </td>
                                                            <td>
                                                                <%#Eval("tel_part")%>
                                                            </td>
                                                            <td>
                                                                Telefono oficina:
                                                            </td>
                                                            <td>
                                                                <%#Eval("tel_ofi")%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                e-mails:
                                                            </td>
                                                            <td>
                                                                <%#Eval("email")%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                Tipo Equipo GPS:
                                                            </td>
                                                            <td>
                                                                <%#Eval("tipo_equipo")%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                ID Equipo GPS:
                                                            </td>
                                                            <td>
                                                                <%#Eval("id_equipo_gps")%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                SIM Nro:
                                                            </td>
                                                            <td>
                                                                <%#Eval("sim_nro")%>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                                <a href="#toptop" onclick="map_panTo(<%# Convert.ToString(Eval("gps_longitud")).Replace(",",".") %>,<%# Convert.ToString(Eval("gps_latitud")).Replace(",",".") %>);">
                                                    <asp:Label ID="lnkDatos" runat="server" CssClass="btn1" Font-Size="X-Small"><%#Eval("identificador_rastreo")%> </asp:Label>
                                                </a>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                Tareas
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <ajaxToolkit:HoverMenuExtender runat="server" ID="masterHuverTareas" TargetControlID="lnkTareas"
                                                    PopupControlID="pnlTareas" PopupPosition="Right" OffsetY="-70" HoverDelay="700">
                                                </ajaxToolkit:HoverMenuExtender>
                                                <asp:Label runat="server" ID="lnkTareas" CssClass="DropDownList" Font-Size="X-Small">Tareas</asp:Label>
                                                <asp:Panel BorderColor="Black" BorderWidth="1px" BackColor="Black" Font-Size="X-Small"
                                                    ForeColor="White" ID="pnlTareas" runat="server">
                                                    <a class="OpcionTarea" href="#toptop" onclick="map_panTo(<%# Convert.ToString(Eval("gps_longitud")).Replace(",",".") %>,<%# Convert.ToString(Eval("gps_latitud")).Replace(",",".") %>)">
                                                        Posicionar </a>
                                                    <br />
                                                    <a title="Haga click aqui para obtener el recorrido histórico del vehiculo <%#Eval("identificador_rastreo")%>"
                                                        class="OpcionTarea" onclick="AbrirDialogo('Historico.aspx?vid=<%#Eval("idrastreo_vehiculo")%>')">
                                                        Histórico </a>
                                                    <br />
                                                    <a title="click aqui para enviar comando al vehiculo <%#Eval("identificador_rastreo")%>"
                                                        class="OpcionTarea" onclick="AbrirDialogo('Envio_de_Comandos.aspx?eqid=<%#Eval("id_equipogps")%>',800,600)">
                                                        Enviar comando al equipo </a>
                                                    <br />
                                                    <a title="Haga click aqui para crear hojas de ruta para el vehiculo <%#Eval("identificador_rastreo")%> "
                                                        class="OpcionTarea" onclick="AbrirDialogo('Hoja_de_ruta.aspx?uid=<%#Eval("id_usuarios")%>&cid=<%#Eval("id_cliente") %>&vid=<%#Eval("id_vehiculo") %>')">
                                                        Hoja de Ruta </a>
                                                    <br />
                                                    <a title="Haga click aqui para crear geocercas para el vehiculo <%#Eval("identificador_rastreo")%> "
                                                        class="OpcionTarea" onclick="AbrirDialogo('Geocercas.aspx?uid=<%#Eval("id_usuarios")%>&cid=<%#Eval("id_cliente") %>&vid=<%#Eval("id_vehiculo") %>')">
                                                        GeoCerca </a>
                                                    <br />
                                                </asp:Panel>
                                                <ajaxToolkit:HoverMenuExtender runat="server" ID="masterHover" TargetControlID="lnkTareas"
                                                    PopupControlID="pnlVer" PopupPosition="Top" OffsetX="-75" HoverDelay="700">
                                                </ajaxToolkit:HoverMenuExtender>
                                                <asp:Panel BorderColor="Black" BorderWidth="2px" BackColor="#024579" ID="pnlVer"
                                                    runat="server">
                                                    <a title="Contactos de <%#Eval("cliente") %>" href="rastreo_admin/admin_cliente_contacto_lista.aspx?id=<%# Eval("idrastreo_persona") %>">
                                                        Contactos</a>
                                                    <br />
                                                    <a title="Documentos relacionados con <%#Eval("cliente") %>" href="rastreo_admin/admin_cliente_documentos.aspx?cid=<%# Eval("idrastreo_persona") %>">
                                                        Documentos</a>
                                                    <br />
                                                    <a title="Permisos del usuario <%#Eval("usuario") %>" href="rastreo_admin/admingps_opciones_usuario_lista.aspx?pid=<%# Eval("idrastreo_persona") %>">
                                                        Permisos</a>
                                                    <br />
                                                    <a title="Historial de eventos relacionados con <%#Eval("cliente") %>" href="rastreo_admin/admin_historial.aspx?cid=<%# Eval("idrastreo_persona") %>">
                                                        Historial de Eventos</a></asp:Panel>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Fecha y hora" DataField="gps_fecha" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" />
                                        <%--<asp:BoundField  HeaderText="Latitud" DataField="gps_latitud" />
                            <asp:BoundField  HeaderText="Longitud" DataField="gps_longitud" />--%>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                Direccion cercana
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%#GeoPos(Convert.ToString(Eval("gps_latitud")), Convert.ToString(Eval("gps_longitud")), Convert.ToString(Eval("gps_dir")))%>
                                                <br />
                                                <%#Convert.ToString(Eval("gps_latitud")).Replace(",", ".")%>,<%#Convert.ToString(Eval("gps_longitud")).Replace(",", ".")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Velocidad" DataField="gps_velocidad" />
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                Rumbo
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%#Eval("gps_rumbo") %>°
                                                <%# Eval("rumbo") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                Evento
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <label style='background-color: <%#Eval("color_evento") %>'>
                                                    <%#Eval("evento")%></label>
                                            </ItemTemplate>
                                            <ItemStyle ForeColor="White" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                Entradas I/O</HeaderTemplate>
                                            <ItemTemplate>
                                                <ajaxToolkit:HoverMenuExtender runat="server" ID="statusIOHover" TargetControlID="lnkIO"
                                                    PopupControlID="pnlStatusIO" PopupPosition="Left" OffsetY="-45" OffsetX="0">
                                                </ajaxToolkit:HoverMenuExtender>
                                                <asp:Panel BorderColor="Black" BorderWidth="2px" BackColor="Black" Font-Size="X-Small"
                                                    ForeColor="White" ID="pnlStatusIO" runat="server">
                                                    <%#IIf(Convert.ToString(Eval("tipo_de_reporte")).Contains("TRAX"), Convert.ToString(GetStatusIO(Convert.ToString(Eval("gps_estado_io")), Convert.ToString(Eval("gps_edaddeldato")), Convert.ToString(Eval("gps_tipodeposicion")))), String.Empty)%></asp:Panel>
                                                <asp:Label runat="server" ID="lnkIO" CssClass="btn" ToolTip="">STATUS</asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                                    <PagerStyle BackColor="Navy" ForeColor="White" HorizontalAlign="Center" />
                                    <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                                    <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
                                    <AlternatingRowStyle BackColor="#BABABA" />
                                </asp:GridView>
                            </div>
                            <asp:SqlDataSource ID="sqlds_Lista_Vehiculos" runat="server" ConnectionString="<%$ ConnectionStrings:CS_RASTREOSystem %>"
                                ProviderName="<%$ ConnectionStrings:CS_RASTREOSystem.ProviderName %>" SelectCommand="SELECT DISTINCT ON (identificador_rastreo) * FROM rsview_vehiculo_bandejaentrada_cliente_equipogps LIMIT 1">
                            </asp:SqlDataSource>
                        </td>
                    </tr>
                </table>
                <%--<% If gvEventos.Rows.Count > 0 Then%>
                <asp:Panel ID="pnlEventosCollapse" runat="server" Width="360px">
                    <asp:Panel ID="pnlEventos" runat="server" BackColor="Navy" BorderColor="Black" BorderStyle="Solid"
                        BorderWidth="1px" ForeColor="White" HorizontalAlign="Left" Width="350px">
                        <!--<soundplayer:SoundPlayerI runat="server" ID="sndEvento" />-->
                        <asp:Label ID="lbCollapse" runat="server" CssClass="rastreo_botones" Font-Bold="True"
                            Font-Names="Nina, Segoe, Arial Narrow, Arial, Verdana" Font-Size="Small" Text="Eventos"
                            ToolTip="Haga click aqui para ver las alertas" Width="50%"> </asp:Label>
                        <asp:Button UseSubmitBehavior="false" runat="server" ID="btnApagarTodosLosEventos"
                            Text="Atender todos los eventos." ToolTip="Bajo su propia responsabilidad, elimina todos los avisos de eventos."
                            CssClass="rastreo_botones" />
                        <asp:GridView runat="server" ID="gvEventos" DataSourceID="sqldsEventos" AllowPaging="True"
                            AllowSorting="True" AutoGenerateColumns="False" BackColor="White" BorderColor="#999999"
                            BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="99%"
                            Font-Names="Nina, Segoe, Arial Narrow, Arial, Verdana" Font-Size="XX-Small">
                            <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                            <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                            <PagerStyle BackColor="Navy" ForeColor="White" HorizontalAlign="Center" />
                            <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                            <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
                            <AlternatingRowStyle BackColor="#BABABA" />
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <label style='background-color: <%#Eval("color_evento") %>; color: <%#IIf(Convert.ToString(Eval("color_evento")) = "#000000" OR Convert.ToString(Eval("color_evento")) = "", "black", "white")%>'>
                                            <%#Convert.ToString((Eval("evento")))%>
                                        </label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Identificador" DataField="identificador_rastreo" />
                                <asp:BoundField HeaderText="Fecha y Hora" DataField="evento_fechahora" />
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" ID="linkAtenderEvento" CommandName="ATENDER EVENTO"
                                            CommandArgument='<%#Eval("idrastreogps_avisos") %>' CssClass="rastreo_botones_save"> Atender </asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <asp:SqlDataSource ID="sqldsEventos" runat="server" ConnectionString="<%$ ConnectionStrings:CS_RASTREOSystem %>"
                            ProviderName="<%$ ConnectionStrings:CS_RASTREOSystem.ProviderName %>" SelectCommand="SELECT * FROM rsview_avisos_equipo_vehiculo_cliente WHERE atendido = FALSE">
                        </asp:SqlDataSource>
                    </asp:Panel>
                </asp:Panel>
                <%End If%>
                <ajaxToolkit:AlwaysVisibleControlExtender ID="AlwaysVisibleControlExtenderAlertas"
                    runat="server" TargetControlID="pnlEventos" VerticalSide="Bottom" HorizontalSide="Right">
                </ajaxToolkit:AlwaysVisibleControlExtender>--%>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <!-- PIE-->
    <div id="pie" align="center">
        <div class="footnotes">
            &copy; 2010 Rastreo.com.py - <a href="http://www.rastreo.com.py/contactenos.php">Contactenos</a>
            - <a href="/manual/manual.pdf">Manual en linea<img src="App_Themes/CENTRAL/Imagenes/help-browser.png"
                alt="[?]" /></a></div>
    </div>
    <!-- PIE-->
    </form>
    <script type="text/javascript" src="OpenLayers.js"></script>
    <%--<script src='http://dev.virtualearth.net/mapcontrol/mapcontrol.ashx?v=6.1'></script>--%>
    <%--<script src="http://api.maps.yahoo.com/ajaxymap?v=3.0&appid=euzuro-openlayers"></script>--%>
    <script type="text/javascript" src="FuncionesGPS.js"></script>
</body>
</html>