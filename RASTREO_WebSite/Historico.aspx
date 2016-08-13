<%@ page language="VB" 
         enableeventvalidation="false" 
         autoeventwireup="true" 
         codefile="Historico.aspx.vb"
         inherits="Historico" 
         stylesheettheme="RASTREO"%>

<%@ Register Assembly="System.Web.DataVisualization, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" 
    TagPrefix="aspchart" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
 <!-- Autor: Ing. Adolfo Bresanovich || Director de R&D RASTREO Paraguay -->
 
 <head id="HistoricoHead" runat="server">

    <link rel="shortcut icon" href="~/RASTREO.ico" type="image/x-icon"/>
    <link rel="icon" href="~/RASTREO.ico" type="image/x-icon"/>
    <title>RASTREO Paraguay - Recorrido del vehiculo <%=vehiculo_seleccionado%></title>
    <script type="text/javascript" src="FuncionesGenerales.js"></script>    
    <script type="text/javascript">

        function MapSizePlus() {
            map.div.style.position = "absolute";
            map.div.style.left = "0px";
            map.div.style.height = "700px";
            map.updateSize();
        }

        function MapSizeMinus() {
            map.div.style.position = "relative";
            map.div.style.height = "365px";
            map.updateSize();
        }
        function multiplicar(){
            m1 = document.getElementById("txtCONSUMO").value;
            m2 = document.getElementById("txtPrecioGslt").value;
            r = m1*m2;
            document.getElementById("txtConsumoGs").value = r;
        }
    
    </script>
  </head>
  <body onload="Historico_init_map();">
    <div id="toptop" visible="false" />
        <asp:HiddenField ID="txtFECHAINICIO" runat="server" />
        <asp:HiddenField ID="txtFECHAFIN" runat="server" />
    <div>
     <form id="RASTREOmainForm" runat="server">
        <ajaxToolkit:ToolkitScriptManager AsyncPostBackTimeout="7200" ID="RASTREOToolScriptManager" ScriptMode="Release" runat="server">
            <Scripts>
                <asp:ScriptReference Path="~/WebKit.js" />
            </Scripts>
        </ajaxToolkit:ToolkitScriptManager>    
      <div id="RASTREO_mainwrapper">
        <asp:Button CssClass="rastreo_botones_cancel" runat="server" ID="btnSetOpHistorico" Text="Opciones del recorrido historico" ToolTip="Haga click aqui para seleccionar las opciones del recorrido e iniciar la vista del mismo" />
        <asp:Button CssClass="rastreo_botones_save" runat="server" ID="btnExcel" Text="Descargar planilla de recorrido" ToolTip="Haga click aqui para descargar el archivo de la planilla de recorrido"/>
        <asp:Button CssClass="rastreo_botones_save" runat="server" ID="btnKml" Text="Descargar archivo Google Earth" ToolTip="Haga click aqui para descargar el archivo en formato KML"/>
        <img src="img/view_previous_on.png" style="cursor:pointer" title="Disminur tamaño de mapa" onclick="MapSizeMinus();" alt="Reducir mapa" id="reducemapa" />
        <img src="img/view_next_on.png" style="cursor:pointer" title="Aumentar tamaño de mapa" onclick="MapSizePlus();" alt="Aumentar mapa" id="aumentamapa" />
        <asp:Panel runat="server" ID="pnl_popuphistorico" Visible="true" CssClass="PopUp" Width="480px">
            <asp:UpdateProgress runat="server" ID="updpg_popuphistorico" AssociatedUpdatePanelID="updpnl_popuphistorico">
                <ProgressTemplate>
                <img src="./App_Themes/CENTRAL/Imagenes/ajax-loader.gif" alt="-"/>
                <asp:Label runat="server" ID="lbActualizandopopuphistorico" Font-Size="X-Small" CssClass="rastreo_nowloading">
                Actualizando, podria llevar unos minutos, favor aguarde....
                </asp:Label>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <asp:UpdatePanel runat="server" ID="updpnl_popuphistorico">                
                <ContentTemplate>
                    <asp:HiddenField ID="txtfile" runat="server" EnableViewState="true" />
                    <asp:HiddenField ID="idVehiculoSeleccionado" runat="server" Value="0"/>
                    <table width="100%" cellspacing="0" cellpadding="0">             
                    <tr>
                    <td>
                        <asp:CheckBox runat="server" ID="chkResumenParada" Text="Resumir paradas"  Checked="true"
                                        ToolTip="Chequee esta casilla para resumir paradas." />
                    </td>
                    <td>
                        <asp:CheckBox runat="server" ID="chkResumenOnly" Text="Solo cuadro de resumen"  Checked="false"
                                        ToolTip="Chequee esta casilla si desea solo ver el resumen sin necesidad de descargar el historial completo del periodo seleccionado." />
                    </td>
                    <td colspan="4">
                        <%--<input class="rastreo_botones" id="cronometro" onclick="cronometro(1);" onmouseover="cronometro(2);" />--%>
                        <asp:CheckBox runat="server" ID="chkRefOnly" Text="Filtrar posiciones cercanas a referencias."  Checked="false"
                                    ToolTip="Chequee esta casilla si desea ver solo las posiciones de su vehiculo cercanas a sus referencias ingresadas con el administrador de referencias." />                                                
                    </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td align="center" style="border:solid 2px Black">Dia</td>
                        <td align="center" style="border:solid 2px Black">Hora</td>
                        <td align="center" style="border:solid 2px Black">Minuto</td>
                        <td align="center" style="border:solid 2px Black">Mes y Año</td>
                    </tr>
                    <tr style="background-color:Navy;color:White">
                    <td>Inicio:</td>
                    <td style="border-right:solid 2px black;border-left:solid 2px black;">
                        <ajaxToolkit:NumericUpDownExtender ID="NUD1" runat="server" Maximum="31" Minimum="1" TargetControlID="txtDiaInicio" Width="50"></ajaxToolkit:NumericUpDownExtender>
                        <asp:TextBox runat="server" ID="txtDiaInicio" ToolTip="Dia de inicio del reporte" onchange="if (isNaN(this.value)) { this.value='1'; this.focus()}">
                        </asp:TextBox>
                    <td style="border-right:solid 2px black;border-left:solid 2px black;">
                        <ajaxToolkit:NumericUpDownExtender ID="NUDH1" runat="server" Maximum="23" Minimum="0" TargetControlID="txtHoraInicio" Width="50"></ajaxToolkit:NumericUpDownExtender>
                        <asp:TextBox ID="txtHoraInicio" runat="server" onchange="if (isNaN(this.value)) { this.value='0'; this.focus()}"></asp:TextBox>
                    </td>
                    <td style="border-right:solid 2px black;border-left:solid 2px black;">
                        <ajaxToolkit:NumericUpDownExtender ID="NUDM1" runat="server" Maximum="60" Minimum="0" TargetControlID="txtMinutoInicio" Width="50"></ajaxToolkit:NumericUpDownExtender>
                        <asp:TextBox ID="txtMinutoInicio" runat="server" onchange="if (isNaN(this.value)) { this.value='0'; this.focus()}"></asp:TextBox>
                    </td>
                    <td style="border-right:solid 2px black;border-left:solid 2px black;">
                        <asp:DropDownList runat="server" ID="ddlMonth" ToolTip="Mes del cual las fechas pertenecen" AutoPostBack="true">
                        </asp:DropDownList>
                    </td>
                    </tr>
                    <tr style="background-color:Green;color:White">
                    <td> Fin: </td>
                    <td style="border-right:solid 2px black;border-left:solid 2px black;">
                        <ajaxToolkit:NumericUpDownExtender ID="NUD2" runat="server" Maximum="31" Minimum="1" TargetControlID="txtDiaFin" Width="50"></ajaxToolkit:NumericUpDownExtender>
                        <asp:TextBox runat="server" ID="txtDiaFin" ToolTip="Dia de fin del reporte" onchange="if (isNaN(this.value)) { this.value='30'; this.focus()}">
                        </asp:TextBox>
                    </td>
                    <td style="border-right:solid 2px black;border-left:solid 2px black;">
                        <ajaxToolkit:NumericUpDownExtender ID="NUDH2" runat="server" Maximum="23" Minimum="0" TargetControlID="txtHoraFin" Width="50"></ajaxToolkit:NumericUpDownExtender>
                        <asp:TextBox ID="txtHoraFin" runat="server" onchange="if (isNaN(this.value)) { this.value='23'; this.focus()}"></asp:TextBox>
                    </td>
                    <td style="border-right:solid 2px black;border-left:solid 2px black;">
                        <ajaxToolkit:NumericUpDownExtender ID="NUDM2" runat="server" Maximum="60" Minimum="0" TargetControlID="txtMinutoFin" Width="50"></ajaxToolkit:NumericUpDownExtender>
                        <asp:TextBox ID="txtMinutoFin" runat="server" onchange="if (isNaN(this.value)) { this.value='59'; this.focus()}"></asp:TextBox>
                    </td>
                    <td style="border-right:solid 2px black;border-left:solid 2px black;">
                        <asp:DropDownList runat="server" ID="ddlYear" ToolTip="Año del cual las fechas pertenecen" AutoPostBack="true">
                        </asp:DropDownList>
                    </td>
                    </tr>
                    <tr>
                        <td colspan="5" style="border:solid 2px black">
                            <asp:Label runat="server" ID="lbCalculo" Width="100%" CssClass="Mensaje_de_error" Font-Size="X-Small"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                        <asp:Button runat="server" ID="btnCalcular" Text="Calcular tiempo de descarga de recorrido" CssClass="rastreo_botones" ToolTip="Haga click aqui para calular cuantos registros y tiempo tomara para la descarga de su recorrido" OnClientClick="cronometro(1);" />
                        </td>
                    </tr>
                    <tr>
                    <td colspan="4">
                    <asp:CheckBox runat="server" ID="chkBusqueda" AutoPostBack="true" Text="Filtrar posiciones por eventos" />
                    <asp:Panel runat="server" ID="pnlBusqueda" BackColor="Navy"
                        ForeColor="White" BorderColor="Black" Visible="false">
                        <asp:Label CssClass="rastreo_botones" ID="Label2" runat="server">
                            Filtros de reportes
                        </asp:Label>
                        <table cellpadding="0" cellspacing="0">
                            <tr>
                                <td>
                                    Filtro por eventos
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlFiltroEventos" AutoPostBack="true"
                                    CssClass="DropDownList">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                         </table>
                    </asp:Panel>
                    </td>
                    </tr>                    
                    <tr>
                    <td colspan="2">
                    <asp:Button CssClass="rastreo_botones_save" runat="server" ID="btnIniciarHistorico" Text="Mostrar recorrido histórico" ToolTip="Haga click aqui para mostrar el recorrido entre las dos fechas y horas seleccionadas" OnClientClick="cronometro(1);DisableSave(this);"/>
                    </td><td colspan="2">
                    <asp:Button CssClass="rastreo_botones_cancel" runat="server" ID="btnDismiss" Text="Cerrar ventana" ToolTip="Cierra esta pantalla"/>
                    </td>
                    </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>
        <ajaxToolkit:ModalPopupExtender runat="server" ID="ajax_popuphistorico" BackgroundCssClass="modalBackground" TargetControlID="btnSetOpHistorico" PopupControlID="pnl_popuphistorico"/>        
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:Label runat="server" ID="lbStatusOpciones" CssClass="Label"></asp:Label>
                <asp:Label ID="errMsgs" runat="server" Width="100%" CssClass="Mensaje_de_error" Visible="false"></asp:Label>
            </ContentTemplate>
        </asp:UpdatePanel>
        <table width="100%">
            <tr>
                <td>
                    <div id="map"></div>            
                </td>
            </tr>
        <tr>
         <td >
           <div style="height:300px; overflow:auto"> 
              <asp:UpdatePanel runat="server" ID="updpnl_VehiculoSeleccionado">
                 <ContentTemplate>
                    <asp:Panel runat="server" ID="pnlResultados" BorderColor="Navy" Width="100%" BackColor="Silver">
                        <table width="100%" style="border:solid 1px grey;" cellspacing="0" cellpadding="0">
                        <tr valign="top">
                            <td colspan="4">
                                Resumen del recorrido:
                            </td>
                        </tr>
                        <tr>
                            <td colspan="1">
                                Posiciones
                            </td>
                            <td colspan="3">
                                <asp:Label ID="txtTOTPOS" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                En Marcha
                            </td>
                            <td>
                                <asp:Label ID="txtTOTENMOV" runat="server" />
                            </td>
                            <td>
                                Detenido
                            </td>
                            <td>
                                <asp:Label ID="txtTOTDETENIDO" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Velocidad promedio
                            </td>
                            <td>
                                <asp:Label ID="txtVELPROM" runat="server" />
                            </td>
                            <td>
                                Distancia total recorrida
                            </td>
                            <td>
                                <asp:Label ID="txtTOTKM" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Velocidad minima en el recorrido
                            </td>
                            <td>
                                <asp:Label ID="txtVELMIN" runat="server" />
                            </td>
                            <td>
                                Velocidad maxima en el recorrido
                            </td>
                            <td>
                                <asp:Label ID="txtVELMAX" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Consumo de combustible aproximado en este recorrido
                            </td>
                            <td>
                                <asp:Label ID="txtCONSUMO" runat="server" onChange="multiplicar()"/>
                            </td>
                            <td>
                                Consumo de combustible en Gs.
                                <asp:Textbox ID="txtPrecioGslt" runat="server" value = "6200" BackColor="#CCCCCC" 
                                    Height="15px" Width="40px" onChange="multiplicar()" 
                                    ToolTip="Precio Gs/lt"/>
                                <asp:regularExpressionValidator ID="validarPrecioGs"
                                     validationExpression="[0-9]*"
                                     controlToValidate="txtPrecioGslt"
                                     errorMessage="No es numero"
                                runat="server"/>
                                
                            </td>
                            <td>
                                <asp:Label ID="txtConsumoGs" runat="server" value = "r"/>
                                
                                
                            </td>
                        </tr>
                        </table>
                        </asp:Panel>
                    <table>
                    <tr>
                    <td>
                    <asp:CheckBox runat="server" ID="chkSpeed" AutoPostBack="true" Text="Filtrar posiciones por velocidad" />
                    </td>                    
                    <td>
                    </td>
                    <td><asp:CheckBox runat="server" ID="chkReferencia" AutoPostBack="true" Text="Ingresar referencias" /></td>
                    </tr>
                    <asp:Panel runat="server" ID="pnlVelocidad" BackColor="Navy"
                        ForeColor="White" BorderColor="Black" Visible="false">
                        <asp:Label CssClass="rastreo_botones" ID="Label1" runat="server">
                            Filtros de reportes
                        </asp:Label>
                        <table cellpadding="0" cellspacing="0">
                            <tr>
                                <td colspan="2">
                                    Filtro por velocidad
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Inicio:
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtSpdINI" CssClass="TextBox"
                                    onchange="if (isNaN(this.value)) { this.value='0'; this.focus()}">
                                    </asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    Fin:
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtSpdFIN" CssClass="TextBox"
                                    onchange="if (isNaN(this.value)) { this.value='0'; this.focus()}">
                                    </asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Button runat="server" ID="btnFiltroSpd" Text="Filtrar" CssClass="rastreo_botones" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>                    
                    <%--<asp:Panel runat="server" ID="pnlBusqueda" BackColor="Navy"
                        ForeColor="White" BorderColor="Black" Visible="false">
                        <asp:Label CssClass="rastreo_botones" ID="lblmiBusqueda" runat="server">
                            Filtros de reportes
                        </asp:Label>
                        <table cellpadding="0" cellspacing="0">
                            
                        </table>
                    </asp:Panel>--%>
                    <asp:Panel runat="server" ID="pnlPuntoReferencia" BackColor="Beige" BorderColor="Black" Visible="false">
                        <asp:Label CssClass="rastreo_botones" ID="lbRefi" runat="server">
                            Referencias
                        </asp:Label>
                        <table cellpadding="0" cellspacing="0">
                            <tr>
                                <td>
                                    Hoja de Referencia <br /><i>[Donde será guardado el punto de referencia]</i>
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlHojaDeReferencia_ptoref" CssClass="DropDownList" Width="100%">
                                    </asp:DropDownList>
                                </td>
                            </tr>

                            <tr>
                                <td>
                                    Nombre del punto
                                </td>
                                <td>
                                    <asp:TextBox CssClass="TextBox" runat="server" ID="txtNombre_pto" />
                                </td>
                                <td>
                                    Descripcion
                                </td>
                                <td>
                                    <asp:TextBox CssClass="TextBox" runat="server" ID="txtDescripcion_pto" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Latitud
                                </td>
                                <td>
                                    <asp:TextBox CssClass="TextBox" runat="server" ID="txtLAT"/>
                                </td>
                                <td>
                                    Longitud
                                </td>
                                <td>
                                    <asp:TextBox CssClass="TextBox" runat="server" ID="txtLON"/>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Metros a la redonda de detección de la referencia
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtMETROS_REDONDA_ptorefref" runat="server" CssClass="TextBox" onchange="if (isNaN(this.value)) { this.value='100'; this.focus()}" Width="100%" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBox runat="server" ID="chkReferencia_ptoref" Text="Deteccion de ingreso a la referencia <br />[separar los mail con <b>;</b> o <b>,</b>]"
                                        ToolTip="Chequee esta casilla si desea que se le avise cuando un vehiculo ingrese a este punto de referencia." Checked="false" />
                                </td>
                                <td colspan="4">
                                    <asp:TextBox ID="txtMailReferencias_ptoref" runat="server" CssClass="TextBox" TextMode="MultiLine" ToolTip="Ingrese en esta casilla los mail a donde será enviados los avisos cuando ingrese un vehículo a esta referencia." Height="100%" Width="100%" />
                                </td>
                            </tr>
                            
                            <tr>
                                <td>
                                    <asp:Button runat="server" ID="btnSAVEPTO" 
                                    CssClass="rastreo_botones_save" Text="Guardar" />
                                </td>
                                <td>
                                    <asp:Button runat="server" ID="btnCANCELPTO" 
                                    CssClass="rastreo_botones_cancel" Text="Cancelar" />
                                </td>
                            </tr>

                        </table>
                    </asp:Panel>
                 <asp:GridView ID="gridVehiculoSeleccionado" runat="server"
                        DataSourceID="sqldsVehiculoSeleccionado" AllowPaging="True" 
                        DataKeyNames="identificador_rastreo, id_vehiculo, gps_longitud, gps_latitud"
                        AllowSorting="False" AutoGenerateColumns="False" BackColor="White" 
                        BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" 
                        GridLines="Vertical" Width="100%" Font-Names="Nina, Segoe, Arial Narrow, Arial, Verdana"
                        HeaderStyle-Font-Names="Nina, Segoe, Arial Narrow, Arial, Verdana"
                        Font-Size="Smaller">
                        <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                    <Columns>
                        <asp:TemplateField> 
                            <HeaderTemplate> Fecha y hora de posición </HeaderTemplate>
                            <ItemTemplate>                                
                                <a href="#toptop" onclick="map_panTo(<%# Convert.ToString(Eval("gps_longitud")).Replace(",",".") %>,<%# Convert.ToString(Eval("gps_latitud")).Replace(",",".") %>);">
                                  <%#Eval("gps_fechahora_reporte")%> 
                                </a>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <%--<% If chkCalles.Checked Then%>--%>
                                    Direccion cercana  
                                <%--<%End If%>--%>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%--<% If chkCalles.Checked Then%>--%>
                                    <%#GeoPos(Convert.ToString(Eval("gps_latitud")), Convert.ToString(Eval("gps_longitud")), Convert.ToString(Eval("gps_dir")))%>
                                    <br />
                                    <%#Convert.ToString(Eval("gps_latitud")).Replace(",", ".")%>&nbsp;<%#Convert.ToString(Eval("gps_longitud")).Replace(",", ".")%>
                                    
                                <%--<%End If%>--%>
                            </ItemTemplate>
                        </asp:TemplateField>                        
                        <asp:BoundField  HeaderText="Velocidad" DataField="gps_velocidad" />
                        <asp:TemplateField>
                            <HeaderTemplate> Rumbo </HeaderTemplate>
                            <ItemTemplate>
                                <%#Eval("gps_rumbo") %>° <%# Eval("rumbo") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                Evento
                            </HeaderTemplate>
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
                            <HeaderTemplate>
                                    Status
                            </HeaderTemplate>
                            <ItemTemplate>
                                <ajaxToolkit:HoverMenuExtender runat="server" ID="statusIOHoverSEL" TargetControlID="lnkIOSEL" PopupControlID="pnlStatusIOSEL" PopupPosition="Left" OffsetY="-30" OffsetX="0"  HoverDelay="700">
                                </ajaxToolkit:HoverMenuExtender>
                                <asp:Panel BorderColor="Black" BorderWidth="2px" BackColor="Black" Font-Size="X-Small" ForeColor="White" ID="pnlStatusIOSEL" runat="server">
                                    <%#IIf(Convert.ToString(Eval("tipo_de_reporte")).Contains("TRAX"), Convert.ToString(GetStatusIO(Convert.ToString(Eval("gps_estado_io")), Convert.ToString(Eval("gps_edaddeldato")), Convert.ToString(Eval("gps_tipodeposicion")))), String.Empty)%></asp:Panel>
                                <asp:Label runat="server" ID="lnkIOSEL" CssClass="btn" ToolTip="">STATUS</asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                Referencia cercana
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#gGetReferenciaCercana(Convert.ToDouble(Eval("gps_latitud")), Convert.ToDouble(Eval("gps_longitud")))%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <% If chkReferencia.Checked Then%>
                                    Ingresar referencia
                                <%End If%>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <% If chkReferencia.Checked Then%>
                                <a href="#toptop" onclick='setLONLAT_Historico(<%# Convert.ToString(Eval("gps_longitud")).Replace(",",".") %>,<%# Convert.ToString(Eval("gps_latitud")).Replace(",",".") %>);'
                                   class="rastreo_botones" style="font-size:small">
                                    Agregar como referencia
                                </a>
                                <% End If%>
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
                                <asp:ControlParameter ControlID="idVehiculoSeleccionado" Name="idVehiculoSeleccionado" ConvertEmptyStringToNull="false" DbType="Int32" />
                                <asp:ControlParameter ControlID="txtFECHAINICIO" Name="fecha_inicio" DbType="DateTime" />
                                <asp:ControlParameter ControlID="txtFECHAFIN"  Name="fecha_fin" DbType="DateTime"/>
                                <asp:ControlParameter ControlID="ddlFiltroEventos" PropertyName="SelectedValue"  Name="evento" DbType="String" />
                                <asp:ControlParameter ControlID="txtSpdINI" Name="SpdIni" DbType="Int32"/>
                                <asp:ControlParameter ControlID="txtSpdFIN" Name="SpdFin" DbType="Int32"/>
                            </SelectParameters>
                        </asp:SqlDataSource>
                        <asp:Panel runat="server" ID="pnlGraphs">
                            <aspchart:Chart id="chartVelocidad" runat="server" Height="300px" Width="960px" 
                                ImageLocation="~/tmp/ChartPic_#SEQ(300,3)" Palette="BrightPastel" imagetype="Png" AntiAliasing="All" BorderDashStyle="Solid" BackSecondaryColor="White" BackGradientStyle="TopBottom" BorderWidth="2" backcolor="#CCCCCC" BorderColor="26, 59, 105" ImageStorageMode="UseHttpHandler">
                                <Titles>
                                    <aspchart:Title Text="Estadisticas de velocidad en el tiempo del recorrido"></aspchart:Title>
                                </Titles>
                                <Legends>
                                    <aspchart:Legend IsTextAutoFit="False" Name="Default" BackColor="Transparent" Font="Trebuchet MS, 8.25pt, style=Bold"></aspchart:Legend>
                                </legends>
                                <borderskin skinstyle="Emboss"></borderskin>
                                <Series>
                                    <%--<asp:Series Name="Column" BorderColor="180, 26, 59, 105">
                                    <points>
                                        <asp:DataPoint YValues="45" />
                                        <asp:DataPoint YValues="34" />
                                        <asp:DataPoint YValues="67" />
                                        <asp:DataPoint YValues="31" />
                                        <asp:DataPoint YValues="27" />
                                        <asp:DataPoint YValues="87" />
                                        <asp:DataPoint YValues="45" />
                                        <asp:DataPoint YValues="32" />
                                    </points>
                                    </asp:Series>--%>
                                </series>
                                <ChartAreas>
                                    <aspchart:ChartArea Name="chartAreaVelocidad" BorderColor="88, 88, 88, 64" BorderDashStyle="Solid" BackSecondaryColor="White" BackColor="64, 165, 191, 228" ShadowColor="Transparent" BackGradientStyle="TopBottom">
                                        <area3dstyle Rotation="30" perspective="20" Inclination="20" IsRightAngleAxes="False" wallwidth="1" IsClustered="False"></area3dstyle>
                                            <axisy linecolor="64, 64, 64, 64">
                                                <LabelStyle font="Trebuchet MS, 8.25pt, style=Bold" />
                                                <MajorGrid linecolor="64, 64, 64, 64" />
                                            </axisy>
                                        <axisx linecolor="64, 64, 64, 64">
                                                <LabelStyle font="Trebuchet MS, 8.25pt, style=Bold" />
                                                <MajorGrid IntervalType="Minutes" Interval="NotSet" linecolor="64, 64, 64, 64" />
                                        </axisx>
                                    </aspchart:ChartArea>
                                </chartareas>
                              </aspchart:Chart>
                        </asp:Panel>
                   </ContentTemplate>
                   <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="txtPrecioGslt" EventName="TextChanged" />
                   </Triggers>

                </asp:UpdatePanel>
             </div>
           </td>
         </tr>
       </table>
     </div>
    </form>
   </div>
    <script src="http://maps.google.com/maps/api/js?v=3.2&sensor=false"></script>
    <script type="text/javascript" src="OpenLayers.js"></script>
    <script type="text/javascript" src="FuncionesGPS.js"></script>
    <%--<script src='http://dev.virtualearth.net/mapcontrol/mapcontrol.ashx?v=6.1'></script>--%>
    <%--<script src="http://api.maps.yahoo.com/ajaxymap?v=3.0&appid=euzuro-openlayers"></script>--%>
    <%--<script type="text/javascript" src="http://www.openlayers.net/api/OpenLayers.js"></script>--%>
    </body>
</html>




