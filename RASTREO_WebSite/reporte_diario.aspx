<%@ Page Language="VB" EnableEventValidation = "false" AutoEventWireup="true" CodeFile="reporte_diario.aspx.vb" Inherits="Historico" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="ajaxToolkit" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<!-- Autor: Ing. Adolfo Bresanovich || Director de R&D RASTREO Paraguay -->

<head id="HistoricoHead" runat="server">    

    <link rel="shortcut icon" href="~/RASTREO.ico" type="image/x-icon"/>
    <link rel="icon" href="~/RASTREO.ico" type="image/x-icon"/>
    <title>RASTREO Paraguay - Recorrido del vehiculo <%=vehiculo_seleccionado%></title>
    <script type="text/javascript" src="FuncionesGenerales.js"></script>    
    
</head>
<body onload="Historico_init_map();">
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
        <a id="toptop">
                    <asp:GridView ID="gridVehiculoSeleccionado" runat="server"
                        DataSourceID="sqldsVehiculoSeleccionado" AllowPaging="false" 
                        AllowSorting="False" AutoGenerateColumns="False" BackColor="White" 
                        BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" 
                        GridLines="Vertical" Width="100%" Font-Names="Arial Narrow, Verdana" 
                        HeaderStyle-Font-Names="Arial Narrow, Verdana"
                        Font-Size="Smaller">
                        <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                        <Columns>
                        <asp:TemplateField> 
                            <HeaderTemplate> Fecha y hora de posición </HeaderTemplate>
                            <ItemTemplate>

                                <a href="http://maps.google.com/staticmap?center=<%#Convert.ToString(Eval("gps_latitud")).Replace(",",".") %>,<%#Convert.ToString(Eval("gps_longitud")).Replace(",",".") %>&zoom=16&size=640x480&maptype=hybrid&markers=<%#Convert.ToString(Eval("gps_latitud")).Replace(",",".") %>,<%#Convert.ToString(Eval("gps_longitud")).Replace(",",".") %>,bluer&sensor=false&key=ABQIAAAAzr2EBOXUKnm_jVnk0OJI7xSsTL4WIgxhMZ0ZK_kHjwHeQuOD4xQJpBVbSrqNn69S6DOTv203MQ5ufA" />

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
                                    <%#GeoPos(Convert.ToString(Eval("gps_latitud")), Convert.ToString(Eval("gps_longitud")), Convert.ToString(Eval("gps_dir")))%> | 
                                    <%#Convert.ToString(Eval("gps_latitud")).Replace(",", ".")%>&nbsp;<%#Convert.ToString(Eval("gps_longitud")).Replace(",", ".")%>
                                <%--<%End If%>--%>
                            </ItemTemplate>
                        </asp:TemplateField>                        
                        <asp:BoundField  HeaderText="Velocidad" DataField="gps_velocidad" />
                        <asp:TemplateField>
                            <HeaderTemplate> Rumbo </HeaderTemplate>
                            <ItemTemplate>
                                <%#Eval("gps_rumbo") %> grados <%# Eval("rumbo") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                Evento
                            </HeaderTemplate>
                            <ItemTemplate>
                                <table>
                                    <tr>
                                        <td style='background-color:<%#Eval("color_evento") %>; color:White;' 
                                            align="center">
                                            <%#Eval("evento")%>
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </asp:TemplateField>
                      <asp:TemplateField>
                            <HeaderTemplate>
                                Referencia cercana
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#gGetReferenciaCercana(Convert.ToDouble(Eval("gps_latitud")), Convert.ToDouble(Eval("gps_longitud")), vehiculo_seleccionado)%>
                            </ItemTemplate>
                        </asp:TemplateField>                           
                        </Columns>
                        <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                        <PagerStyle BackColor="Navy" ForeColor="White" HorizontalAlign="Center" />
                        <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
                        <AlternatingRowStyle BackColor="#BABABA" />
                        </asp:GridView>
                        </a>
        <asp:Button CssClass="rastreo_botones_cancel" runat="server" ID="btnSetOpHistorico" Text="Opciones del recorrido historico" ToolTip="Haga click aqui para seleccionar las opciones del recorrido e iniciar la vista del mismo" />
        <asp:Button CssClass="rastreo_botones_save" runat="server" ID="btnExcel" Text="Descargar planilla de recorrido" ToolTip="Haga click aqui para descargar el archivo de la planilla de recorrido"/>
        <asp:Panel runat="server" ID="pnl_popuphistorico" Visible="true" CssClass="PopUp" Width="480px">
            <asp:UpdateProgress runat="server" ID="updpg_popuphistorico" AssociatedUpdatePanelID="updpnl_popuphistorico">
                <ProgressTemplate>
                <img src="./App_Themes/CENTRAL/Imagenes/ajax-loader.gif" alt="-"/>
                <asp:Label runat="server" ID="lbActualizandopopuphistorico" Font-Size="XX-Small" CssClass="rastreo_nowloading">
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
                        <asp:CheckBox runat="server" ID="chkResumenParada" Text="Resumir paradas"  Checked="false"
                                        ToolTip="Chequee esta casilla no se te para el pito." />
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
                        <asp:Button runat="server" ID="btnCalcular" Text="Calcular tiempo de descarga de recorrido" CssClass="rastreo_botones" ToolTip="Haga click aqui para calcular cuantos registros y tiempo tomara para la descarga de su recorrido" OnClientClick="cronometro(1);" />
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
        <ajaxToolkit:ModalPopupExtender runat="server" ID="ajax_popuphistorico" 
            BackgroundCssClass="modalBackground" TargetControlID="btnSetOpHistorico" 
            PopupControlID="pnl_popuphistorico"/>        
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <asp:Label runat="server" ID="lbStatusOpciones" CssClass="Label"></asp:Label>
                <asp:Label ID="errMsgs" runat="server" Width="100%" CssClass="Mensaje_de_error" Visible="false"></asp:Label>
            </ContentTemplate>
        </asp:UpdatePanel>
        <table width="100%">
            <tr>
                <td>
                    <div id="map" style="height:250px"></div>            
                </td>
            </tr>
        <tr>
        <td >
            <div style="height:300px; overflow:auto"> 
                <asp:UpdatePanel runat="server" ID="updpnl_VehiculoSeleccionado">
                    <ContentTemplate>
                    <asp:Panel runat="server" ID="pnlResultados" BorderColor="Navy" Width="100%" BackColor="Silver">
                        <table width="100%" style="border:solid 1px grey;font-size:smaller" cellspacing="0" cellpadding="0">
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
                                <asp:Label ID="txtCONSUMO" runat="server" />
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
        </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </td>
        </tr>
        </table>
        </div>
    </form>
    </div>
    <%--    <%If Request.ServerVariables("REMOTE_HOST") = "190.104.156.142" Then%>--%>
        <script src="http://maps.google.com/maps?file=api&amp;v=2&amp;sensor=false&amp;key=ABQIAAAA_r2VgoolhB6iO9xSBULQFxTPNvAASRBEw_TMHTynVLwSexQB8hQ4RTPwWf589F08aFbkKIvY434rxQ" type="text/javascript"></script> 
<%--    <%ElseIf Request.ServerVariables("REMOTE_HOST") = "190.104.153.61" Then%>
        <script src="http://maps.google.com/maps?file=api&amp;v=2&amp;sensor=false&amp;key=ABQIAAAA_r2VgoolhB6iO9xSBULQFxQjCxOHmMAjVGNZYZATNfcVIoBu7hQt5fsrP_aWckxRC585DEwI98L-ow" type="text/javascript"></script>
    <%ElseIf Request.ServerVariables("REMOTE_HOST") = "127.0.0.1" Then%>
        <script src="http://maps.google.com/maps?file=api&amp;v=2&amp;sensor=false&amp;key=ABQIAAAA_r2VgoolhB6iO9xSBULQFxQ9pzGUvE4e-uE6JwppIOBVvhvAmRQysIA9otMz1DX9na4y4LGsV3YqDA" type="text/javascript"></script>
    <%End If%>--%>
    <%--<script type="text/javascript" src="http://www.openlayers.net/api/OpenLayers.js?q=1"></script>--%>
    <script type="text/javascript" src="OpenLayers.js"></script>
    <script type="text/javascript" src="FuncionesGPS.js"></script>
    <%--<script src='http://dev.virtualearth.net/mapcontrol/mapcontrol.ashx?v=6.1'></script>--%>
    <%--<script src="http://api.maps.yahoo.com/ajaxymap?v=3.0&appid=euzuro-openlayers"></script>--%>

</body>
</html>




