<%@ Page Title="RASTREO Paraguay - Control de vehiculos y posiciones GPS" EnableViewStateMac="false"
    Language="VB" Async="false" AutoEventWireup="false" CodeFile="mainGPS.aspx.vb"
    Inherits="mainGPS" Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<!-- Autor: Ing. Adolfo Bresanovich || Director de R&D RASTREO Paraguay -->
<head id="GPSadminHead" runat="server">
    <link rel="shortcut icon" href="~/RASTREO.ico" type="image/x-icon" />
    <link rel="icon" href="~/RASTREO.ico" type="image/x-icon" />
    <title>RASTREO WebSystem - Bienvenido!</title>
    <script type="text/javascript" src="FuncionesGenerales.js"></script>
</head>
<body onload='MainGPS_init_map();countdown_timer(<%= txtTimeout.text %>);'>
    <div id="toptop" visible="false" />
    <form id="RASTREOmainForm" runat="server">
    <!-- 10092010 Salia un error de ScripManager-->
    <ajaxToolkit:ToolkitScriptManager AsyncPostBackTimeout="3000" ID="RASTREOToolScriptManager"
        ScriptMode="Release" runat="server" CombineScripts="True">
        <Scripts>
            <asp:ScriptReference Path="~/WebKit.js" />
        </Scripts>
    </ajaxToolkit:ToolkitScriptManager>
    <!-- 10092010 Salia un error de ScripManager-->
    <script language="javascript" type="text/javascript">
        function set_sendingmail(value) {
            if (document.getElementById("sendingmail") != null) {
                var SM = document.getElementById("sendingmail");
                SM.value = value;
            }
        }
        function set_chkReferencias(value) {
            if (document.getElementById("chkReferencias") != null) {
                var CHKREF = document.getElementById("chkReferencias");
                CHKREF.checked = value;
            }
        }
        function set_seguimiento_activo(value) {
            if (document.getElementById("seguimiento_activo") != null) {
                var SEG = document.getElementById("seguimiento_activo");
                var pnlUpdate = document.getElementById("updpg_pnlGPS");
                SEG.value = value;
                pnlUpdate.style.display = "inline";
            }
        }
        function set_idVehiculoSeleccionado(value) {
            if (document.getElementById("idVehiculoSeleccionado") != null) {
                var VSEG = document.getElementById("idVehiculoSeleccionado");
                VSEG.value = value;
            }
        }
        // voy a probar un codigo para mantener estado de sesion valido segun http://support.microsoft.com/kb/829743/es

    </script>
    <!-- CABECERA-->
    <div id="top">
        <div class="logo">
            <a href="#">
                <img alt="RASTREAR: the tracking solution!" src="App_Themes/CENTRAL/Imagenes/logo.png"
                    width="282" height="95" /></a></div>
        <asp:DropDownList ID="ddlLang" AutoPostBack="false" runat="server">
            <asp:ListItem Text="Español" Value="es-PY"></asp:ListItem>
            <asp:ListItem Text="English" Value="en-US"></asp:ListItem>
            <asp:ListItem Text="Portugues" Value="pt-BR"></asp:ListItem>
        </asp:DropDownList>
        <asp:Button ID="btnChangeLang" runat="server" meta:resourcekey="btnlang" Text="Cambiar Idioma"
            CssClass="btn1" />
        <div class="menutop">
            <div class="menubox1">
                <div class="menuitem">
                    Cliente: <span class="menublanco">
                        <% Response.Write(mi_cliente)%></span></div>
                <div class="menuitem">
                    Usuario: <span class="menublanco">
                        <% Response.Write(mi_usuario)%></span></div>
                <div class="menuitemLOGOUT">
                    <img alt=" X " src="App_Themes/CENTRAL/Imagenes/sailir.jpg" align="absmiddle" />
                    <asp:LinkButton runat="server" ID="RASTREO_LOGOUT" meta:resourcekey="RASTREO_LOGOUTResource1">Salir del sistema</asp:LinkButton>
                </div>
            </div>
            <div class="menubox2">
                <div class="menuitem2">
                    Próxima Actualización en
                    <input id="cd_timer" class="sec" value="" readonly="readonly" />
                    segundos |
                    <asp:TextBox runat="server" ID="txtTimeout" CssClass="sec2" onchange="if (isNaN(this.value)) { this.value='60'; this.focus()}"
                        Width="30px" MaxLength="3" meta:resourcekey="txtTimeoutResource1" />
                    <asp:Button runat="server" CssClass="btn" ID="btnAjustar" UseSubmitBehavior="False"
                        Text="Ajustar intérvalo" OnClientClick="DisableSave(this);" meta:resourcekey="btnAjustarResource1" />
                    <% If permiso_REFERENCIAS Then%>
                    |
                    <asp:CheckBox runat="server" ID="chkReferencias" Text="Ingresar referencias" ToolTip="Chequee esta casilla para ingresar las posiciones actuales de sus vehiculos como referencias."
                        AutoPostBack="True" meta:resourcekey="chkReferenciasResource1" />
                    <%End If%>
                    <% If permiso_REFERENCIAS Then%>
                    |
                    <input value="Administrar Referencias" type="button" title="Haga click aqui para crear hojas de referencia que contendran puntos de referencia a ser utilizados en las hojas de ruta"
                        class="btn" onclick="AbrirDialogo('Referencias.aspx?uid=<%= idUsuario.Value %>&cid=<%= idPersona.Value %>')" />
                    <% End If%>
                    |
                    <asp:Button UseSubmitBehavior="False" runat="server" ID="btn_OpenSendMail" CssClass="btn"
                        Text="Enviar posicion por mail" OnClientClick="get_g_zoom();set_sendingmail(true);"
                        meta:resourcekey="btn_OpenSendMailResource1" />
                    <ajaxToolkit:ModalPopupExtender ID="PopupMAIL" runat="server" BackgroundCssClass="modalBackground"
                        TargetControlID="btn_OpenSendMail" PopupControlID="pnlSendMail" OkControlID="btnCloseMail"
                        DynamicServicePath="" Enabled="True" />
                    <asp:Panel CssClass="PopUp" ID="pnlSendMail" runat="server" meta:resourcekey="pnlSendMailResource1">
                        <img src="App_Themes/CENTRAL/Imagenes/enviartit.jpg" alt="Enviar posicion por mail." />
                        <table width="100%" cellpadding="0" cellspacing="0" class="Label">
                            <tr>
                                <td>
                                    Destinatario(s)
                                    <br />
                                    [separarlos por <b>;</b> o <b>,</b>]:
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="mailsSEND" TextMode="MultiLine" CssClass="TextBoxMAIL"
                                        Width="200px" Height="50px" meta:resourcekey="mailsSENDResource1" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    Vehiculos disponibles:<asp:ListBox runat="server" ID="ddlSendMailTo" ToolTip="Elija que de vehiculo enviar posicion"
                                        CssClass="btn" Width="100%" Height="100px" Rows="3" SelectionMode="Multiple"
                                        meta:resourcekey="ddlSendMailToResource1" />
                                    <br />
                                    <asp:Button runat="server" ID="btnAgregarVehiculo" Text="Agregar vehiculo(s) a la lista"
                                        CssClass="btn1" meta:resourcekey="btnAgregarVehiculoResource1" />
                                    <br />
                                    Lista de vehiculos
                                    <br />
                                    <asp:ListBox runat="server" ID="listVehiculosInvolucrados" Width="100%" CssClass="DropDownList"
                                        Height="100px" SelectionMode="Multiple" meta:resourcekey="listVehiculosInvolucradosResource1" />
                                    <asp:Button runat="server" ID="btnEliminarVehiculo" Text="Eliminar vehiculo(s) de la lista"
                                        CssClass="btn1" OnClientClick="return seguro_que();" meta:resourcekey="btnEliminarVehiculoResource1" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:TextBox runat="server" ID="txtMensajeCustom" CssClass="TextBoxMAIL" Width="98%"
                                        Height="150px" TextMode="MultiLine" meta:resourcekey="txtMensajeCustomResource1"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Button runat="server" ID="btnSendMail" CssClass="btn" Text="Enviar posicion"
                                        OnClientClick="set_sendingmail(false);" meta:resourcekey="btnSendMailResource1" />
                                    <asp:Button runat="server" ID="btnCloseMail" CssClass="btn" Text="Cancelar" OnClientClick="set_sendingmail(false);"
                                        meta:resourcekey="btnCloseMailResource1" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <% If permiso_REPORTEAUTOMATICO Then%>
                    |
                    <asp:Button UseSubmitBehavior="False" runat="server" ID="btnPreferencias" CssClass="btn"
                        Text="Reporte Automatico" OnClientClick="set_sendingmail(true);" meta:resourcekey="btnPreferenciasResource1" />
                    <ajaxToolkit:ModalPopupExtender ID="modalPopiSETTINGS" runat="server" BackgroundCssClass="modalBackground"
                        TargetControlID="btnPreferencias" PopupControlID="pnlPrefs" CancelControlID="btnCloseSettings"
                        DynamicServicePath="" Enabled="True" />
                    <asp:Panel CssClass="PopUp" ID="pnlPrefs" runat="server" Width="300px" meta:resourcekey="pnlPrefsResource1">
                        <table width="100%" cellpadding="0" cellspacing="0" class="Label">
                            <tr>
                                <td>
                                    <asp:CheckBox runat="server" ID="chk_RA_OnOff" Text="Activar reportes automaticos"
                                        meta:resourcekey="chk_RA_OnOffResource1" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Hora de envio:
                                    <asp:TextBox runat="server" ID="txtHORA_ENVIO_REPORTE" CssClass="TextBox" meta:resourcekey="txtHORA_ENVIO_REPORTEResource1"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Ingrese los mail a donde desea que se envie reportes automaticos:
                                    <asp:TextBox runat="server" ID="txtMailSettings" CssClass="TextBoxMAIL" TextMode="MultiLine"
                                        Width="90%" Height="100px" ToolTip="Puede ingresar sus mail separados por ',' o ':'"
                                        meta:resourcekey="txtMailSettingsResource1"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button runat="server" ID="btnSaveSettings" Text="Guardar preferencias" CssClass="btn1"
                                        OnClientClick="set_sendingmail(false);" meta:resourcekey="btnSaveSettingsResource1" /><asp:Button
                                            runat="server" ID="btnCloseSettings" Text="Cancelar" CssClass="btn1" OnClientClick="set_sendingmail(false);"
                                            meta:resourcekey="btnCloseSettingsResource1" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <% End If%>
                </div>
            </div>
        </div>
    </div>
    <!-- CABECERA-->
    <asp:HiddenField ID="idUsuario" runat="server" Value="0" />
    <asp:HiddenField ID="idPersona" runat="server" Value="0" />
    <asp:HiddenField ID="idVehiculoSeleccionado" runat="server" Value="0" />
    <asp:HiddenField ID="g_zoom_value" runat="server" Value="0" />
    <asp:HiddenField ID="sendingmail" runat="server" Value="false" />
    <asp:HiddenField ID="seguimiento_activo" runat="server" Value="false" />
    <div id="RASTREO_mainwrapper">
        <table width="100%" cellpadding="0" cellspacing="0">
            <tr>
                <td valign="top">
                    <div align="center">
                        <input enableviewstate="true" class="TextBox" id="txtsrch" style="width: 65%" /><input
                            class="btn1" value="Buscar direccion" style="font-size: smaller" onclick="googleFindAddress(GetValorFromObj('txtsrch'));return false;"
                            type="submit" /></div>
                    <div id="map">
                    </div>
                </td>
            </tr>
            <tr>
                <td valign="top">
                    <asp:UpdatePanel runat="server" ID="pnlMainGPS">
                        <ContentTemplate>
                            <asp:UpdateProgress runat="server" ID="updpg_pnlGPS" AssociatedUpdatePanelID="pnlMainGPS">
                                <ProgressTemplate>
                                    <div style="position: absolute; left: 0px; top: 0px; width: 100%; height: 15%" class="modalBackground">
                                        <img src="./App_Themes/CENTRAL/Imagenes/ajax-loader.gif" alt="-" />
                                        <label class="rastreo_nowloading">
                                            Actualizando...</label>
                                    </div>
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                            <asp:Label ID="errMsgs" runat="server" Width="100%" CssClass="Mensaje_de_error" Visible="False"></asp:Label>
                            <asp:HiddenField ID="txtfile" runat="server" />
                            <asp:HiddenField ID="reffile" runat="server" />
                            <asp:Timer runat="server" ID="tmr_Reload" Interval="60000" />
                            <asp:Panel runat="server" ID="pnlPuntoReferencia" BackColor="Beige" BorderColor="Black"
                                BorderWidth="2px" Visible="False" Width="99.8%" meta:resourcekey="pnlPuntoReferenciaResource1">
                                <asp:Label CssClass="rastreo_botones" ID="lbRefi" runat="server" meta:resourcekey="lbRefiResource1">
							Referencias
                                </asp:Label>
                                <table width="98%" cellpadding="1" cellspacing="0">
                                    <tr>
                                        <td colspan="4">
                                            <asp:Button runat="server" ID="btnSAVEPTO" CssClass="btn" Text="Guardar referencia"
                                                meta:resourcekey="btnSAVEPTOResource1" />
                                            <asp:Button runat="server" ID="btnCANCELPTO" OnClientClick="set_chkReferencias(false);"
                                                CssClass="btn" Text="Cancelar" meta:resourcekey="btnCANCELPTOResource1" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Hoja de Referencia
                                            <br />
                                            <i>[Donde será guardado el punto de referencia]</i>
                                        </td>
                                        <td>
                                            <asp:DropDownList runat="server" ID="ddlHojaDeReferencia_ptoref" CssClass="DropDownList"
                                                Width="100%" meta:resourcekey="ddlHojaDeReferencia_ptorefResource1">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Nombre del punto
                                        </td>
                                        <td>
                                            <asp:TextBox CssClass="TextBox" runat="server" ID="txtNombre_ptoref" meta:resourcekey="txtNombre_ptorefResource1" />
                                        </td>
                                        <td>
                                            Descripcion
                                        </td>
                                        <td>
                                            <asp:TextBox CssClass="TextBox" runat="server" ID="txtDescripcion_ptoref" meta:resourcekey="txtDescripcion_ptorefResource1" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Latitud
                                        </td>
                                        <td>
                                            <asp:TextBox CssClass="TextBox" runat="server" ID="txtLAT" Enabled="False" onchange="if (isNaN(this.value)) { this.value='0'; this.focus()}"
                                                meta:resourcekey="txtLATResource1" />
                                        </td>
                                        <td>
                                            Longitud
                                        </td>
                                        <td>
                                            <asp:TextBox CssClass="TextBox" runat="server" ID="txtLON" Enabled="False" onchange="if (isNaN(this.value)) { this.value='0'; this.focus()}"
                                                meta:resourcekey="txtLONResource1" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Metros a la redonda de detección de la referencia
                                        </td>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtMETROS_REDONDA_ptorefref" runat="server" CssClass="TextBox" onchange="if (isNaN(this.value)) { this.value='100'; this.focus()}"
                                                Width="100%" meta:resourcekey="txtMETROS_REDONDA_ptorefrefResource1" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox runat="server" ID="chkReferencia_ptoref" Text="Deteccion de ingreso a la referencia <br />[separar los mail con <b>;</b> o <b>,</b>]"
                                                ToolTip="Chequee esta casilla si desea que se le avise cuando un vehiculo ingrese a este punto de referencia."
                                                meta:resourcekey="chkReferencia_ptorefResource1" />
                                        </td>
                                        <td colspan="3">
                                            <asp:TextBox ID="txtMailReferencias_ptoref" runat="server" CssClass="TextBoxMAIL"
                                                TextMode="MultiLine" ToolTip="Ingrese en esta casilla los mail a donde será enviados los avisos cuando ingrese un vehículo a esta referencia."
                                                Height="100%" Width="100%" meta:resourcekey="txtMailReferencias_ptorefResource1" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <div id="div_mainGPS_grid">
                                <asp:SqlDataSource ID="sqldsVehiculoSeleccionado" runat="server" ConnectionString="<%$ ConnectionStrings:CS_RASTREOSystem %>"
                                    ProviderName="<%$ ConnectionStrings:CS_RASTREOSystem.ProviderName %>" SelectCommand="SELECT DISTINCT ON (identificador_rastreo) * FROM rsview_vehiculo_bandejaentrada_cliente_equipogps_sucu WHERE id_usuarios IS NOT NULL and id_usuarios = @id_usuarios AND id_vehiculo IS NOT NULL AND id_vehiculo = @idVehiculoSeleccionado">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="idVehiculoSeleccionado" Name="idVehiculoSeleccionado"
                                            ConvertEmptyStringToNull="False" />
                                        <asp:ControlParameter ControlID="idUsuario" Name="id_usuarios" ConvertEmptyStringToNull="False" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                                <asp:GridView PageSize="1" ID="gridVehiculoSeleccionado" runat="server" DataSourceID="sqldsVehiculoSeleccionado"
                                    AllowPaging="True" DataKeyNames="id_vehiculo,gps_latitud,gps_longitud" AllowSorting="True"
                                    AutoGenerateColumns="False" BackColor="White" BorderColor="#999999" BorderStyle="None"
                                    BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%" Font-Names="Segoe,Arial Narrow,Arial,Verdana"
                                    meta:resourcekey="gridVehiculoSeleccionadoResource1">
                                    <EmptyDataRowStyle CssClass="Mensaje_de_error" HorizontalAlign="Center" />
                                    <RowStyle BackColor="White" ForeColor="Black" />
                                    <Columns>
                                        <asp:TemplateField meta:resourcekey="TemplateFieldResource1">
                                            <ItemTemplate>
                                                <input type="button" onclick="set_seguimiento_activo(false);set_idVehiculoSeleccionado(0);__doPostBack('','');"
                                                    size="30px" class="btnclose" value="X" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField meta:resourcekey="TemplateFieldResource2">
                                            <ItemStyle Width="70px" HorizontalAlign="Center" />
                                            <HeaderTemplate>
                                                Vehiculo
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <ajaxToolkit:HoverMenuExtender runat="server" ID="masterHuverTareas" TargetControlID="lnkTareas"
                                                    PopupControlID="pnlTareas" PopupPosition="Right" OffsetY="-20" HoverDelay="700"
                                                    DynamicServicePath="" Enabled="True">
                                                </ajaxToolkit:HoverMenuExtender>
                                                <asp:Label runat="server" ID="lnkTareas">
								<a
									title='Haga click aqui para centrar el mapa en el móvil <%# Eval("identificador_rastreo") %> '
									href="#toptop"
									onclick="map_panTo(<%# Convert.ToString(Eval("gps_longitud")).Replace(",",".") %>,<%# Convert.ToString(Eval("gps_latitud")).Replace(",",".") %>);">
								  <%#Eval("identificador_rastreo")%>
								</a>
                                                </asp:Label>
                                                <asp:Panel BorderColor="Black" BorderWidth="2px" BackColor="RoyalBlue" Font-Size="X-Small"
                                                    ForeColor="White" ID="pnlTareas" runat="server" meta:resourcekey="pnlTareasResource1">
                                                    <a title='Haga click aqui para centrar el mapa en el móvil <%# Eval("identificador_rastreo") %> '
                                                        class="OpcionTarea" href="#toptop" onclick="map_panTo(<%# Convert.ToString(Eval("gps_longitud")).Replace(",",".") %>,<%# Convert.ToString(Eval("gps_latitud")).Replace(",",".") %>)">
                                                        Posicionar </a>
                                                    <br />
                                                    <a title='Haga click aqui para obtener el recorrido histórico del móvil <%# Eval("identificador_rastreo") %> '
                                                        class="OpcionTarea" onclick='AbrirDialogo(&#039;Historico.aspx?vid=<%# Eval("id_vehiculo") %>&#039;)'>
                                                        Histórico </a>
                                                    <% If permiso_HOJADERUTA Then%>
                                                    <br />
                                                    <a title='Haga click aqui para crear hojas de ruta para el vehiculo <%# Eval("identificador_rastreo") %> '
                                                        class="OpcionTarea" onclick='AbrirDialogo(&#039;Hoja_de_ruta.aspx?uid=<%# Eval("id_usuarios") %>&amp;cid=<%# Eval("id_cliente") %>&amp;vid=<%# Eval("id_vehiculo") %>&#039;)'>
                                                        Hoja de Ruta </a>
                                                    <% End If%><% If permiso_GEOCERCA Then%>
                                                    <br />
                                                    <a title='Haga click aqui para crear geocercas para el vehiculo <%# Eval("identificador_rastreo") %> '
                                                        class="OpcionTarea" onclick='AbrirDialogo(&#039;Geocercas.aspx?uid=<%# Eval("id_usuarios") %>&amp;cid=<%# Eval("id_cliente") %>&amp;vid=<%# Eval("id_vehiculo") %>&#039;)'>
                                                        GeoCerca </a>
                                                    <% End If%></asp:Panel>
                                                <ajaxToolkit:HoverMenuExtender runat="server" ID="masterHuverDatos" TargetControlID="lnkTareas"
                                                    PopupControlID="pnlDatos" PopupPosition="Right" OffsetX="90" OffsetY="-30" HoverDelay="700"
                                                    DynamicServicePath="" Enabled="True">
                                                </ajaxToolkit:HoverMenuExtender>
                                                <asp:Panel BorderColor="Black" BorderWidth="2px" BackColor="RoyalBlue" Font-Size="X-Small"
                                                    ForeColor="White" ID="pnlDatos" runat="server" meta:resourcekey="pnlDatosResource1">
                                                    <table cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td class="td">
                                                                Modelo:
                                                            </td>
                                                            <td class="td">
                                                                <%#Eval("marca")%>
                                                            </td>
                                                            <td>
                                                                Chofer:
                                                            </td>
                                                            <td class="td">
                                                                <%#Eval("chofer")%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="td">
                                                                Marca:
                                                            </td>
                                                            <td class="td">
                                                                <%#Eval("modelo")%>
                                                            </td>
                                                            <td>
                                                                Contacto:
                                                            </td>
                                                            <td class="td">
                                                                <%#Eval("chofer_contacto")%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="td">
                                                                Patente:
                                                            </td>
                                                            <td class="td">
                                                                <%#Eval("matricula")%>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Descripción" DataField="sucursalempresa" meta:resourcekey="BoundFieldResource3" />
                                        <asp:BoundField HeaderText="Fecha y hora" DataField="gps_fecha" meta:resourcekey="BoundFieldResource1" />
                                        <asp:TemplateField meta:resourcekey="TemplateFieldResource3">
                                            <HeaderTemplate>
                                                Direccion cercana
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%#GeoPos(Convert.ToString(Eval("gps_latitud")), Convert.ToString(Eval("gps_longitud")), Convert.ToString(Eval("gps_dir")))%>
                                                <br />
                                                <%#Convert.ToString(Eval("gps_latitud")).Replace(",", ".")%>,<%#Convert.ToString(Eval("gps_longitud")).Replace(",", ".")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField meta:resourcekey="TemplateFieldResource4">
                                            <HeaderTemplate>
                                                Vel.
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%#IIf(Convert.ToString(Eval("evento")).ToLowerInvariant().Contains("detenido") Or Convert.ToString(Eval("evento")).ToLowerInvariant().Contains("apagado"), "0", Eval("gps_velocidad"))%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField meta:resourcekey="TemplateFieldResource5">
                                            <HeaderTemplate>
                                                Rumbo
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%#Eval("gps_rumbo") %>°
                                                <%# Eval("rumbo") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField meta:resourcekey="TemplateFieldResource6">
                                            <HeaderTemplate>
                                                Evento
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <label style='background-color: <%# Eval("color_evento") %>'>
                                                    <%#Eval("evento")%></label>
                                            </ItemTemplate>
                                            <ItemStyle ForeColor="White" />
                                        </asp:TemplateField>
                                        <asp:TemplateField meta:resourcekey="TemplateFieldResource7">
                                            <HeaderTemplate>
                                                Referencia cercana
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%# mainGetReferenciaCercana(Convert.ToString(Eval("gps_latitud")), Convert.ToString(Eval("gps_longitud")))%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField meta:resourcekey="TemplateFieldResource8">
                                            <HeaderTemplate>
                                                Entradas I/O</HeaderTemplate>
                                            <ItemTemplate>
                                                <ajaxToolkit:HoverMenuExtender runat="server" ID="statusIOHoverSEL" TargetControlID="lnkIOSEL"
                                                    PopupControlID="pnlStatusIOSEL" PopupPosition="Left" OffsetY="-30" HoverDelay="1000"
                                                    DynamicServicePath="" Enabled="True">
                                                </ajaxToolkit:HoverMenuExtender>
                                                <asp:Panel BorderColor="Black" BorderWidth="2px" BackColor="Black" Font-Size="X-Small"
                                                    ForeColor="White" ID="pnlStatusIOSEL" runat="server" meta:resourcekey="pnlStatusIOSELResource1">
                                                    <%#IIf(Convert.ToString(Eval("tipo_de_reporte")).Contains("TRAX"), Convert.ToString(GetStatusIO(Convert.ToString(Eval("gps_estado_io")), Convert.ToString(Eval("gps_edaddeldato")), Convert.ToString(Eval("gps_tipodeposicion")))), String.Empty)%></asp:Panel>
                                                <asp:Label runat="server" ID="lnkIOSEL" CssClass="btn" meta:resourcekey="lnkIOSELResource1">STATUS</asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                                    <PagerStyle BackColor="Navy" ForeColor="White" HorizontalAlign="Center" />
                                    <SelectedRowStyle BackColor="#FFEE66" ForeColor="Black" />
                                    <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" Font-Size="Smaller"
                                        Font-Names="Nina,Segoe,Arial Narrow,Arial,Verdana" />
                                    <AlternatingRowStyle BackColor="#E5E5E5" />
                                </asp:GridView>
                                <asp:GridView PageSize="100" ID="gv_Lista_de_Todos_los_Vehiculos" runat="server"
                                    DataSourceID="sqldsTodosLosVehiculos" DataKeyNames="identificador_rastreo,id_vehiculo,gps_longitud,gps_latitud"
                                    AllowSorting="True" AutoGenerateColumns="False" BackColor="White" BorderColor="#999999"
                                    BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                                    Font-Names="Segoe,Nina,Arial,Arial Narrow,Verdana" meta:resourcekey="gv_Lista_de_Todos_los_VehiculosResource1">
                                    <EmptyDataRowStyle CssClass="Mensaje_de_error" HorizontalAlign="Center" />
                                    <RowStyle BackColor="White" ForeColor="Black" Font-Names="Segoe,Nina,Arial Narrow,Arial,Verdana" />
                                    <Columns>
                                        <asp:TemplateField meta:resourcekey="TemplateFieldResource9">
                                            <ItemTemplate>
                                                <input class="btnplus" value="+" type="button" onclick='set_seguimiento_activo(true);set_idVehiculoSeleccionado(<%# Eval("id_vehiculo") %>);__doPostBack(&#039;&#039;,&#039;&#039;);' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField meta:resourcekey="TemplateFieldResource10">
                                            <ItemStyle Width="70px" HorizontalAlign="Center" />
                                            <HeaderTemplate>
                                                Vehiculo
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <ajaxToolkit:HoverMenuExtender runat="server" ID="masterHuverTareas" TargetControlID="lnkTareas"
                                                    PopupControlID="pnlTareas" PopupPosition="Right" OffsetX="-5" OffsetY="-40" PopDelay="700"
                                                    DynamicServicePath="" Enabled="True">
                                                </ajaxToolkit:HoverMenuExtender>
                                                <asp:Label runat="server" ID="lnkTareas" Width="98%" Font-Size="Smaller">
								<a href="#toptop" onclick="map_panTo(<%# Convert.ToString(Eval("gps_longitud")).Replace(",",".") %>,<%# Convert.ToString(Eval("gps_latitud")).Replace(",",".") %>);" class="btn1" >
								  <%#Eval("identificador_rastreo")%>
								</a>
                                                </asp:Label>
                                                <asp:Panel BorderColor="#CCCCCC" BorderWidth="1px" BackColor="Black" Font-Size="X-Small"
                                                    ForeColor="White" ID="pnlTareas" runat="server" meta:resourcekey="pnlTareasResource2">
                                                    <a title='Haga click aqui para centrar el mapa en el móvil <%# Eval("identificador_rastreo") %> '
                                                        class="OpcionTarea" href="#toptop" onclick="map_panTo(<%# Convert.ToString(Eval("gps_longitud")).Replace(",",".") %>,<%# Convert.ToString(Eval("gps_latitud")).Replace(",",".") %>)">
                                                        Posicionar </a>
                                                    <br />
                                                    <a title='Haga click aqui para obtener el recorrido histórico del móvil <%# Eval("identificador_rastreo") %> '
                                                        class="OpcionTarea" onclick='AbrirDialogo(&#039;Historico.aspx?vid=<%# Eval("id_vehiculo") %>&#039;)'>
                                                        Histórico </a>
                                                    <% If permiso_HOJADERUTA Then%>
                                                    <br />
                                                    <a title='Haga click aqui para crear hojas de ruta para el vehiculo <%# Eval("identificador_rastreo") %> '
                                                        class="OpcionTarea" onclick='AbrirDialogo(&#039;Hoja_de_ruta.aspx?uid=<%# Eval("id_usuarios") %>&amp;cid=<%# Eval("id_cliente") %>&amp;vid=<%# Eval("id_vehiculo") %>&#039;)'>
                                                        Hoja de Ruta </a>
                                                    <% End If%><% If permiso_GEOCERCA Then%>
                                                    <br />
                                                    <a title='Haga click aqui para crear geocercas para el vehiculo <%# Eval("identificador_rastreo") %> '
                                                        class="OpcionTarea" onclick='AbrirDialogo(&#039;Geocercas.aspx?uid=<%# Eval("id_usuarios") %>&amp;cid=<%# Eval("id_cliente") %>&amp;vid=<%# Eval("id_vehiculo") %>&#039;)'>
                                                        GeoCerca </a>
                                                    <% End If%></asp:Panel>
                                                <ajaxToolkit:HoverMenuExtender runat="server" ID="masterHuverDatos" TargetControlID="lnkTareas"
                                                    PopupControlID="pnlDatos" PopupPosition="Right" OffsetX="-5" OffsetY="15" HoverDelay="100"
                                                    DynamicServicePath="" Enabled="True">
                                                </ajaxToolkit:HoverMenuExtender>
                                                <asp:Panel BorderColor="Black" BorderWidth="1px" BackColor="Black" Font-Size="X-Small"
                                                    ForeColor="White" ID="pnlDatos" runat="server" meta:resourcekey="pnlDatosResource2">
                                                    <table cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td class="td">
                                                                Modelo:
                                                            </td>
                                                            <td class="td">
                                                                <%#Eval("marca")%>
                                                            </td>
                                                            <td>
                                                                Chofer:
                                                            </td>
                                                            <td class="td">
                                                                <%#Eval("chofer")%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="td">
                                                                Marca:
                                                            </td>
                                                            <td class="td">
                                                                <%#Eval("modelo")%>
                                                            </td>
                                                            <td>
                                                                Contacto:
                                                            </td>
                                                            <td class="td">
                                                                <%#Eval("chofer_contacto")%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="td">
                                                                Patente:
                                                            </td>
                                                            <td class="td">
                                                                <%#Eval("matricula")%>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Descripción" DataField="sucursalempresa" meta:resourcekey="BoundFieldResource3" />
                                        <asp:BoundField HeaderText="Fecha y hora" DataField="gps_fecha" meta:resourcekey="BoundFieldResource2" />
                                        <asp:TemplateField meta:resourcekey="TemplateFieldResource11">
                                            <HeaderTemplate>
                                                Direccion cercana
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%#GeoPos(Convert.ToString(Eval("gps_latitud")), Convert.ToString(Eval("gps_longitud")), Convert.ToString(Eval("gps_dir")))%>
                                                <br />
                                                <%#Convert.ToString(Eval("gps_latitud")).Replace(",", ".")%>,<%#Convert.ToString(Eval("gps_longitud")).Replace(",", ".")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField meta:resourcekey="TemplateFieldResource12">
                                            <HeaderTemplate>
                                                Vel.
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%#IIf(Convert.ToString(Eval("evento")).ToLowerInvariant().Contains("detenido") Or Convert.ToString(Eval("evento")).ToLowerInvariant().Contains("apagado"), "0", Eval("gps_velocidad"))%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField meta:resourcekey="TemplateFieldResource13">
                                            <HeaderTemplate>
                                                Rumbo
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%#Eval("gps_rumbo") %>°
                                                <%# Eval("rumbo") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField meta:resourcekey="TemplateFieldResource14">
                                            <HeaderTemplate>
                                                Evento
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <label style='background-color: <%# Eval("color_evento") %>'>
                                                    <%#Eval("evento")%></label>
                                            </ItemTemplate>
                                            <ItemStyle ForeColor="White" />
                                        </asp:TemplateField>
                                        <asp:TemplateField meta:resourcekey="TemplateFieldResource15">
                                            <HeaderTemplate>
                                                Referencia cercana
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%# mainGetReferenciaCercana(Convert.ToString(Eval("gps_latitud")), Convert.ToString(Eval("gps_longitud")))%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField meta:resourcekey="TemplateFieldResource16">
                                            <HeaderTemplate>
                                                Entradas I/O</HeaderTemplate>
                                            <ItemTemplate>
                                                <ajaxToolkit:HoverMenuExtender runat="server" ID="statusIOHover" TargetControlID="lnkIO"
                                                    PopupControlID="pnlStatusIO" PopupPosition="Left" OffsetY="-30" DynamicServicePath=""
                                                    Enabled="True">
                                                </ajaxToolkit:HoverMenuExtender>
                                                <asp:Panel BorderColor="Black" BorderWidth="1px" BackColor="Black" Font-Size="X-Small"
                                                    ForeColor="White" ID="pnlStatusIO" runat="server" meta:resourcekey="pnlStatusIOResource1">
                                                    <%#IIf(Convert.ToString(Eval("tipo_de_reporte")).Contains("TRAX"), Convert.ToString(GetStatusIO(Convert.ToString(Eval("gps_estado_io")), Convert.ToString(Eval("gps_edaddeldato")), Convert.ToString(Eval("gps_tipodeposicion")))), String.Empty)%></asp:Panel>
                                                <asp:Label runat="server" ID="lnkIO" CssClass="btn" meta:resourcekey="lnkIOResource1">STATUS</asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                                    <PagerStyle BackColor="Navy" ForeColor="White" HorizontalAlign="Center" />
                                    <SelectedRowStyle BackColor="#008A8C" ForeColor="White" />
                                    <HeaderStyle BackColor="Black" ForeColor="White" Font-Size="Smaller" Font-Names="Segoe,Nina,Arial Narrow,Arial,Verdana" />
                                    <AlternatingRowStyle BackColor="#E5E5E5" />
                                </asp:GridView>
                            </div>
                            <asp:SqlDataSource ID="sqldsTodosLosVehiculos" runat="server" ConnectionString="<%$ ConnectionStrings:CS_RASTREOSystem %>"
                                ProviderName="<%$ ConnectionStrings:CS_RASTREOSystem.ProviderName %>" SelectCommand="SELECT DISTINCT ON (identificador_rastreo) * FROM rsview_vehiculo_bandejaentrada_cliente_equipogps_sucu WHERE idrastreo_usuarios = @id_usuarios">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="idUsuario" Name="id_usuarios" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
    </div>
    <!-- PIE-->
    <div id="pie" align="center">
        <div class="footnotes">
            &copy; 2011 Rastreo.com.py - <a href="http://www.rastreo.com.py/contactenos.php">Contactenos</a>
            - <a href="/manual/manual.pdf">Manual en linea<img src="App_Themes/CENTRAL/Imagenes/help-browser.png"
                alt="[?]" /></a></div>
    </div>
    <!-- PIE-->
    </form>
    <script src="http://maps.google.com/maps/api/js?v=3.2&sensor=false"></script>
    <script type="text/javascript" src="OpenLayers.js"></script>
    <script type="text/javascript" src="FuncionesGPS.js"></script>
    <%--<script type="text/javascript" src="http://www.openlayers.net/api/OpenLayers.js"></script>--%>
    <%--<script src='http://dev.virtualearth.net/mapcontrol/mapcontrol.ashx?v=6.1'></script>--%>
    <%--<script src="http://api.maps.yahoo.com/ajaxymap?v=3.0&appid=euzuro-openlayers"></script>--%>
</body>
</html>