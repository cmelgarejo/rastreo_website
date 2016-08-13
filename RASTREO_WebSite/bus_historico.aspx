<%@ Page Language="VB" EnableEventValidation = "false" AutoEventWireup="true" CodeFile="bus_historico.aspx.vb" Inherits="Historico" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<!-- Autor: Ing. Adolfo Bresanovich || Director de R&D RASTREO Paraguay -->
<head id="HistoricoHead" runat="server">

    <link rel="shortcut icon" href="~/RASTREO.ico" type="image/x-icon"/>
    <link rel="icon" href="~/RASTREO.ico" type="image/x-icon"/>
    <title>RASTREO Paraguay - Recorrido del vehiculo <%=vehiculo_seleccionado%></title>
    <script type="text/javascript" src="FuncionesGenerales.js"></script>    
    
</head>
<body> <%-- onload="Historico_init_map();">--%>
    <div>
    <form id="RASTREOmainForm" runat="server">
    <ajaxToolkit:ToolkitScriptManager AsyncPostBackTimeout="3600" ID="RASTREOToolScriptManager" ScriptMode="Release" runat="server">
            <Scripts>
            <asp:ScriptReference Path="~/WebKit.js" />
        </Scripts>
    </ajaxToolkit:ToolkitScriptManager>    
    <div id="RASTREO_mainwrapper">
        <asp:UpdateProgress runat="server" ID="updpg_pnlGPS" AssociatedUpdatePanelID="updpnl_popuphistorico">
            <ProgressTemplate>
                <div style="position:absolute;left:0px;top:0px;width:100%;height:1000%" class="modalBackground">
                    <img src="./App_Themes/CENTRAL/Imagenes/ajax-loader.gif" alt="-"/>
                    <label class="rastreo_nowloading">Actualizando...</label>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <a id="toptop"></a>
        <%--<asp:Button CssClass="rastreo_botones_cancel" runat="server" ID="btnSetOpHistorico" Text="Opciones del recorrido historico" ToolTip="Haga click aqui para seleccionar las opciones del recorrido e iniciar la vista del mismo" />--%>
        <asp:Button CssClass="rastreo_botones_save" runat="server" ID="btnExcel" Text="Descargar planilla de recorrido" ToolTip="Haga click aqui para descargar el archivo de la planilla de recorrido"/>
        <asp:Panel runat="server" ID="pnl_popuphistorico" Visible="false" CssClass="PopUp" Width="480px">
            <asp:UpdatePanel runat="server" ID="updpnl_popuphistorico">                
                <ContentTemplate>
                    <asp:HiddenField ID="txtfile" runat="server" EnableViewState="true" />
                    <asp:HiddenField ID="idVehiculoSeleccionado" runat="server" Value="0"/>
                    <table width="100%"  >
<%--                <tr>
                    <td colspan="2">
                        <asp:CheckBox runat="server" ID="check_Calles" Text="Obtener informacion extra de la posicion" ToolTip="Obtiene Pais, Departamento, ciudades y barrios de cada posicion del recorrido, --Esto ralentiza la generacion del recorrido--" />
                    </td>
                    </tr>
--%>                
                    <tr>
                    <td>
                        <asp:CheckBox runat="server" ID="chkCalles" Text="Ver direcciones cercanas"  Checked="true"
                                    ToolTip="Chequee esta casilla si desea ver la calle cercana a la posicion de sus moviles, tenga en cuenta que esta opcion degrada la velocidad de carga de datos del sistema." />                    
                    </td>
                    </tr>
                    <tr>
                    <td> Fecha de inicio: </td>
                    <td>
                    <asp:TextBox CssClass="TextBox" AutoPostBack="false" runat="server" 
                    ID="txtFECHAINICIO" ToolTip="Ingrese la fecha de inicio del recorrido" 
                    Width="160px"/>
                    <%--<!--
                    <ajaxToolkit:CalendarExtender CssClass="MyCalendar" ID="CalendarExtender1" runat="server" Format="dd/MM/yyyy hh:mm:ss" PopupPosition="TopRight" Animated="true" TargetControlID="txtFECHAINICIO" />
                    -->--%>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtFECHAINICIO"
                    
                    ErrorMessage="Fecha y/o hora inválidas"
                    ValidationExpression ="^((((([0-1]?\d)|(2[0-8]))\/((0?\d)|(1[0-2])))|(29\/((0?[1,3-9])|(1[0-2])))|(30\/((0?[1,3-9])|(1[0-2])))|(31\/((0?[13578])|(1[0-2]))))\/((19\d{2})|([2-9]\d{3}))|(29\/0?2\/(((([2468][048])|([3579][26]))00)|(((19)|([2-9]\d))(([2468]0)|([02468][48])|([13579][26]))))))\s(([01]?\d)|(2[0-3]))(:[0-5]?\d){2}.*$" 
                    SetFocusOnError = "true" ></asp:RegularExpressionValidator>                                                                        
                    <ajaxToolkit:MaskedEditExtender ID="mask_fechainstalacion" runat="server" 
                    AcceptAMPM="false"
                    TargetControlID="txtFECHAINICIO" Mask="99/99/9999 99:99:99" 
                    MaskType="DateTime" ClearTextOnInvalid="True" UserDateFormat="DayMonthYear" />
                    </td>
                    </tr>
                    <tr>
                    <td> Fecha de fin: </td>
                    <td>
                    <asp:TextBox CssClass="TextBox" AutoPostBack="false" runat="server" 
                    ID="txtFECHAFIN" ToolTip="Ingrese la fecha de inicio del recorrido" 
                    Width="160px"/>
                    <%--<!--
                    <ajaxToolkit:CalendarExtender CssClass="MyCalendar" ID="CalendarExtender2" runat="server" Format="dd/MM/yyyy hh:mm:ss" PopupPosition="TopRight" Animated="true" TargetControlID="txtFECHAFIN" />
                    -->--%>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtFECHAFIN"
                    ErrorMessage="Fecha y/o hora inválidas"
                    ValidationExpression ="^((((([0-1]?\d)|(2[0-8]))\/((0?\d)|(1[0-2])))|(29\/((0?[1,3-9])|(1[0-2])))|(30\/((0?[1,3-9])|(1[0-2])))|(31\/((0?[13578])|(1[0-2]))))\/((19\d{2})|([2-9]\d{3}))|(29\/0?2\/(((([2468][048])|([3579][26]))00)|(((19)|([2-9]\d))(([2468]0)|([02468][48])|([13579][26]))))))\s(([01]?\d)|(2[0-3]))(:[0-5]?\d){2}.*$" 
                    SetFocusOnError = "true" ></asp:RegularExpressionValidator>                                                                        
                    <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server" 
                    AcceptAMPM="false"  
                    TargetControlID="txtFECHAFIN" Mask="99/99/9999 99:99:99" 
                    MaskType="DateTime" ClearTextOnInvalid="True" UserDateFormat="DayMonthYear" />
                    </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label runat="server" ID="lbCalculo" CssClass="Mensaje_de_error" Font-Size="X-Small"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Panel runat="server" ID="pnlHoras" Width="100%">
                                <asp:RadioButtonList runat="server" ID="rblHoras" 
                                    CausesValidation="false" RepeatDirection="Horizontal" RepeatLayout="Table"
                                     CssClass="DropDownList" AutoPostBack="true" >
                                    <asp:ListItem Value="1">1 Hora</asp:ListItem>
                                    <asp:ListItem Value="6">6 Horas</asp:ListItem>
                                    <asp:ListItem Value="12">12 Horas</asp:ListItem>
                                </asp:RadioButtonList>
                                <asp:RadioButtonList runat="server" ID="rblDias" 
                                    CausesValidation="false" RepeatDirection="Horizontal" RepeatLayout="Table"
                                     CssClass="DropDownList" AutoPostBack="true" >
                                    <asp:ListItem Value="1">1 Dia</asp:ListItem>
                                    <asp:ListItem Value="3">3 Dias</asp:ListItem>
                                    <asp:ListItem Value="5">5 Dias</asp:ListItem>
                                </asp:RadioButtonList>    
                                <asp:RadioButtonList runat="server" ID="rblSemanas" 
                                    CausesValidation="false" RepeatDirection="Horizontal" RepeatLayout="Table"
                                     CssClass="DropDownList" AutoPostBack="true" >
                                    <asp:ListItem Value="7">1 Semana </asp:ListItem>
                                    <asp:ListItem Value="14">2 Semanas </asp:ListItem>
                                    <asp:ListItem Value="21">3 Semanas </asp:ListItem>
                                </asp:RadioButtonList>
                                <asp:RadioButtonList runat="server" ID="rblMeses" 
                                    CausesValidation="false" RepeatDirection="Horizontal" RepeatLayout="Table"
                                     CssClass="DropDownList" AutoPostBack="true" >
                                    <asp:ListItem Value="1">1 Mes</asp:ListItem>
                                    <asp:ListItem Value="3">3 Meses</asp:ListItem>
                                </asp:RadioButtonList>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                        <asp:Button runat="server" ID="btnCalcular" Text="Calcular tiempo de descarga de recorrido" CssClass="rastreo_botones" ToolTip="Haga click aqui para calular cuantos registros y tiempo tomara para la descarga de su recorrido" />
                        </td>
                    </tr>                    
                    <tr>
                    <td>
                    <asp:Button CssClass="rastreo_botones_save" runat="server" ID="btnIniciarHistorico" Text="Mostrar recorrido histórico" ToolTip="Haga click aqui para mostrar el recorrido entre las dos fechas y horas seleccionadas" OnClientClick="DisableSave(this);"/>
                    </td><td>
                    <asp:Button CssClass="rastreo_botones_cancel" runat="server" ID="btnDismiss" Text="Cerrar ventana" ToolTip="Cierra esta pantalla"/>
                    </td>
                    </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>
        <%--<ajaxToolkit:ModalPopupExtender runat="server" ID="ajax_popuphistorico" BackgroundCssClass="modalBackground" TargetControlID="btnSetOpHistorico" PopupControlID="pnl_popuphistorico"/>--%>
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <asp:Label runat="server" ID="lbStatusOpciones" CssClass="Label"></asp:Label>
                <asp:Label ID="errMsgs" runat="server" Width="100%" CssClass="Mensaje_de_error" 
                Visible="false"></asp:Label>
            </ContentTemplate>
        </asp:UpdatePanel>
        <table width="100%">
        <tr>
        <td >
            <asp:UpdateProgress runat="server" ID="updpg_pnlMainGPS" AssociatedUpdatePanelID="updpnl_VehiculoSeleccionado">
            <ProgressTemplate>
                <div style="position:absolute;left:0px;top:0px;width:100%;height:1000%" class="modalBackground">
                    <img src="./App_Themes/CENTRAL/Imagenes/ajax-loader.gif" alt="-"/>
                    <label class="rastreo_nowloading">Actualizando...</label>
                </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <div><%-- style="height:250px; overflow:scroll"> --%>
                <asp:UpdatePanel runat="server" ID="updpnl_VehiculoSeleccionado">
                    <ContentTemplate>
                    <asp:Panel Visible="false" runat="server" ID="pnlResultados" BorderColor="Navy" Width="100%" BackColor="Silver">
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
                    <asp:GridView ID="gridVehiculoSeleccionado" runat="server" 
                        DataSourceID="sqldsVehiculoSeleccionado" AllowPaging="True" 
                        AllowSorting="True" AutoGenerateColumns="False" BackColor="White" 
                        BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" 
                        GridLines="Vertical" Width="100%" Font-Names="Nina, Segoe, Arial Narrow, Arial, Verdana" 
                        HeaderStyle-Font-Names="Nina, Segoe, Arial Narrow, Arial, Verdana" PageSize="500"
                        Font-Size="Small">
                        <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                        <Columns>
                        <asp:TemplateField> 
                            <HeaderTemplate> Fecha y hora </HeaderTemplate>
                            <ItemTemplate>
                                <span style='background-color:<%#Eval("color_evento") %>; color:White;' align="center">
                                    <%#Eval("gps_fechahora_reporte")%> 
                                </span>                                  
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                Evento
                            </HeaderTemplate>
                            <ItemTemplate>
                                <span style='background-color:<%#Eval("color_evento") %>; color:White;' align="center">
                                    <%#Eval("evento")%>
                                </span>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                    Direccion cercana  
                            </HeaderTemplate>
                            <ItemTemplate>
                                    <%#GeoPos(Convert.ToString(Eval("gps_latitud")), Convert.ToString(Eval("gps_longitud")))%>
                                    <br />
                                    <%#Convert.ToString(Eval("gps_latitud")).Replace(",", ".")%>&nbsp;<%#Convert.ToString(Eval("gps_longitud")).Replace(",", ".")%>
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
                        <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
                        <AlternatingRowStyle BackColor="#BABABA" />
                        </asp:GridView>
                        <asp:SqlDataSource ID="sqldsVehiculoSeleccionado"  runat="server" ConnectionString="<%$ ConnectionStrings:CS_RASTREOSystem %>" ProviderName="<%$ ConnectionStrings:CS_RASTREOSystem.ProviderName %>" 
                            SelectCommand=""
                            EnableCaching="true" CacheDuration="Infinite">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="idVehiculoSeleccionado" Name="idVehiculoSeleccionado" ConvertEmptyStringToNull="false" DbType="Int32" />
                                <asp:ControlParameter ControlID="txtFECHAINICIO" Name="fecha_inicio" DbType="DateTime" />
                                <asp:ControlParameter ControlID="txtFECHAFIN"  Name="fecha_fin" DbType="DateTime"/>
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
        <%--<script src="http://maps.google.com/maps?file=api&amp;v=2&amp;sensor=false&amp;key=ABQIAAAA_r2VgoolhB6iO9xSBULQFxTPNvAASRBEw_TMHTynVLwSexQB8hQ4RTPwWf589F08aFbkKIvY434rxQ" type="text/javascript"></script> --%>
<%--    <%ElseIf Request.ServerVariables("REMOTE_HOST") = "190.104.153.61" Then%>
        <script src="http://maps.google.com/maps?file=api&amp;v=2&amp;sensor=false&amp;key=ABQIAAAA_r2VgoolhB6iO9xSBULQFxQjCxOHmMAjVGNZYZATNfcVIoBu7hQt5fsrP_aWckxRC585DEwI98L-ow" type="text/javascript"></script>
    <%ElseIf Request.ServerVariables("REMOTE_HOST") = "127.0.0.1" Then%>
        <script src="http://maps.google.com/maps?file=api&amp;v=2&amp;sensor=false&amp;key=ABQIAAAA_r2VgoolhB6iO9xSBULQFxQ9pzGUvE4e-uE6JwppIOBVvhvAmRQysIA9otMz1DX9na4y4LGsV3YqDA" type="text/javascript"></script>
    <%End If%>--%>
    <%--<script type="text/javascript" src="http://www.openlayers.net/api/OpenLayers.js?q=1"></script>--%>
    <%--<script type="text/javascript" src="OpenLayers.js"></script>
    <script type="text/javascript" src="FuncionesGPS.js"></script>--%>
    <%--<script src='http://dev.virtualearth.net/mapcontrol/mapcontrol.ashx?v=6.1'></script>--%>
    <%--<script src="http://api.maps.yahoo.com/ajaxymap?v=3.0&appid=euzuro-openlayers"></script>--%>

</body>
</html>




