<%@ Page Title="RASTREO Paraguay - Hojas de Ruta" Language="VB" AutoEventWireup="false" CodeFile="Hoja_de_ruta.aspx.vb" Inherits="Hoja_de_Ruta" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<!-- Autor: Ing. Adolfo Bresanovich || Director de R&D RASTREO Paraguay -->
<head id="GPSadminHead" runat="server">

    <link rel="shortcut icon" href="~/RASTREO.ico" type="image/x-icon"/>
    <link rel="icon" href="~/RASTREO.ico" type="image/x-icon"/>
    <title>RASTREO Paraguay - Hoja de ruta para el vehiculo <%=vehiculo_seleccionado%></title>

</head>
<body onload="hojaderuta_init_map();">
    <form id="RASTREOmainForm" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="RASTREOToolScriptManager" ScriptMode="Release" runat="server">
        <Scripts>
            <asp:ScriptReference Path="~/WebKit.js" />
        </Scripts>    
    </ajaxToolkit:ToolkitScriptManager>    
    <asp:UpdatePanel runat="server" ID="updpnl_map">
        <ContentTemplate>
            <asp:HiddenField ID="txtfile" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    
    <asp:UpdateProgress runat="server" ID="updprogressHojaDeRuta" AssociatedUpdatePanelID="updpnlHojaDeRuta">
    <ProgressTemplate>
        <div style="position:absolute;left:0px;top:0px">
            <img src="./App_Themes/CENTRAL/Imagenes/ajax-loader.gif" alt="-"/>
            <label class="rastreo_nowloading">Actualizando...</label>
        </div>
    </ProgressTemplate>
    </asp:UpdateProgress>

    <div id="RASTREO_mainwrapper">
        <asp:Label ID="errMsgs" runat="server" Width="99%" CssClass="Mensaje_de_error" 
        Visible="false"></asp:Label>
        <table width="100%" cellpadding="0" cellspacing="0">
        <tr>
        <td>
            <div id="map"></div>            
        </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:UpdatePanel runat="server" ID="updpnlHojaDeRuta">
                <ContentTemplate>
                    <asp:Panel runat="server" ID="pnlSetupRecorrido" BorderStyle="Solid" BorderWidth="2px" BorderColor="Navy" Width="99%">
                    <asp:Label runat="server" ID="lbRecorridoPanel" Text="Hojas de ruta" CssClass="rastreo_linkbuttons" />
                    <asp:HiddenField runat="server" ID="idhoja_de_ruta" Value="0" />                    
                    <table>
                        <tr>
                            <td valign="top">
                                <asp:Button ID="btnCargaRecorrido" runat="server" CssClass="rastreo_botones" CausesValidation="false"
                                    Text="Agregar hoja de ruta" ToolTip="Carga de hojas de ruta del vehiculo seleccionado" />
                                <asp:Panel ID="pnlRecorrido" runat="server" BackColor="AliceBlue" 
                                    Visible="false">
                                    <table cellpadding="0" cellspacing="0">
                                        <tr align="center">
                                            <td align="center" colspan="2">
                                                <asp:Button ID="btnSAVEREC" runat="server" CssClass="rastreo_botones_save" CausesValidation="true"
                                                    Text="Guardar hoja de ruta" />
                                                <asp:Button ID="btnCANCELREC" runat="server" CssClass="rastreo_botones_cancel" CausesValidation="false"
                                                    Text="Cancelar" />
                                            </td>
                                        </tr>
                                        <tr align="center">
                                            <td colspan="2">
                                                <asp:CheckBox ID="rec_chkACT" runat="server" Text="Activa?" Checked="true"
                                                    ToolTip="Determina si la hoja de ruta esta activa para el vehiculo." />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Descripcion:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="rec_txtDESC" runat="server" CssClass="TextBox" 
                                                    ToolTip="Aqui debe ingresar la descripcion del hoja de ruta. Ej: 'Recorrido de entrega del primer semestre.' " />
                                                
                                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="rec_txtDESC" SetFocusOnError="true" Text="*" ToolTip="Este campo es obligatorio"/>
                                                
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Hora de inicio:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="rec_txtINI" runat="server" CssClass="TextBox"/>
                                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ControlToValidate="rec_txtINI" SetFocusOnError="true" Text="*" ToolTip="Este campo es obligatorio"/>
                                                    <ajaxToolkit:MaskedEditExtender ID="mask_HINI" runat="server" 
                                                    AcceptAMPM="false"  Mask="99:99" MaskType="Time" 
                                                    TargetControlID="rec_txtINI" UserTimeFormat="None" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Hora de fin:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="rec_txtFIN" runat="server" CssClass="TextBox"/>
                                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3" ControlToValidate="rec_txtFIN" SetFocusOnError="true" Text="*" ToolTip="Este campo es obligatorio"/>                                                
                                                <ajaxToolkit:MaskedEditExtender ID="mask_HFIN" runat="server" 
                                                    AcceptAMPM="false"  Mask="99:99" MaskType="Time" 
                                                    TargetControlID="rec_txtFIN" UserTimeFormat="None" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Telefonos móviles a enviar aviso (separar con <b>;</b>)</td>
                                            <td colspan="2"><asp:TextBox TextMode="MultiLine" CssClass="TextBox" 
                                                    runat="server" ID="txtTelMoviles" 
                                                    ToolTip="Ingrese los teléfonos separándolos por punto y coma (;)" 
                                                    Width="100%"/>
                                            </td>
                                        </tr>                                        
                                        <tr>
                                            <td>e-mails a enviar aviso (separar con <b>;</b>)</td>
                                            <td colspan="2"><asp:TextBox TextMode="MultiLine" runat="server" 
                                            CssClass="TextBoxMAIL" ID="txtMails" Width="100%" 
                                            ToolTip="Ingrese los e-mails separándolos por punto y coma (;)" 
                                            onchange="if (this.value.contains(' ')) this.value.replace(' ',';');"/>
                                            </td>                                            
                                        </tr>

                                    </table>
                                </asp:Panel>
                                <asp:GridView ID="gv_Hoja_de_Ruta" runat="server" AllowPaging="True" 
                                    AllowSorting="True" AutoGenerateColumns="False" BackColor="White"
                                    DataKeyNames="idhoja_de_ruta, hora_activacion, hora_desactivacion, activa, id_usuario, descripcion" 
                                    BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" 
                                    DataSourceID="sqlds_Recorrido" Font-Names="Nina, Segoe, Arial Narrow, Arial, Verdana" Font-Size="XX-Small" 
                                    GridLines="Vertical" PageSize="10" Width="100%">
                                    <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                                    <Columns>
                                        <asp:CommandField ShowSelectButton="true" SelectText="Seleccionar" ItemStyle-ForeColor="White"
                         ControlStyle-CssClass="rastreo_botones_save" HeaderText="Seleccionar" />
                                        <asp:BoundField DataField="descripcion" HeaderText="Descripcion" />
                                        <asp:BoundField DataField="hora_activacion" HeaderText="Hora de inicio" DataFormatString="{0:HH:mm}" />
                                        <asp:BoundField DataField="hora_desactivacion" HeaderText="Hora de fin" DataFormatString="{0:HH:mm}"/>
                                        <asp:CheckBoxField DataField="activa" HeaderText ="Activa?" ReadOnly="true" />
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button runat="server" Text="Editar" Font-Size="X-Small" CssClass="rastreo_linkbuttons" CommandArgument='<%#Eval("idhoja_de_ruta")%>' CommandName="Editar"  />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button runat="server" Font-Size="X-Small" CssClass="rastreo_botones_cancel" Text="Eliminar" ToolTip="Eliminar recorrido" CommandArgument='<%#Eval("idhoja_de_ruta") %>' CommandName="Eliminar" OnClientClick="return seguro_que();" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                                    <PagerStyle BackColor="Navy" ForeColor="White" HorizontalAlign="Center" />
                                    <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                                    <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
                                    <AlternatingRowStyle BackColor="#BABABA" />
                                </asp:GridView>
                                <asp:SqlDataSource ID="sqlds_Recorrido" runat="server" 
                                    ConnectionString="<%$ ConnectionStrings:CS_RASTREOSystem %>" 
                                    ProviderName="<%$ ConnectionStrings:CS_RASTREOSystem.ProviderName %>" 
                                    SelectCommand="SELECT * FROM rastreo_hoja_de_ruta WHERE idhoja_de_ruta IN (SELECT idhoja_de_ruta FROM rastreo_hojaderuta_has_vehiculo WHERE id_cliente = @id_cliente AND id_vehiculo = @id_vehiculo) AND id_usuario = @id_usuario">
                                    <SelectParameters>
                                        <asp:QueryStringParameter QueryStringField="uid" ConvertEmptyStringToNull="false" DefaultValue="" Name="id_usuario" />
                                        <asp:QueryStringParameter QueryStringField="cid" ConvertEmptyStringToNull="false" DefaultValue="" Name="id_cliente" />
                                        <asp:QueryStringParameter QueryStringField="vid" ConvertEmptyStringToNull="false" DefaultValue="" Name="id_vehiculo" />
                                    </SelectParameters>
                                </asp:SqlDataSource>                                
                            </td>
                            <td valign="top">
                                <asp:Panel runat="server" ID="pnlSelecRecorrido" Visible="false" Width="90%">
                                <asp:Button ID="btnCargaPuntosRecorrido" runat="server" 
                                    CssClass="rastreo_botones" Text="Agregar puntos al recorrido" CausesValidation="false"
                                    ToolTip="Carga de Puntos de Recorrido del vehiculo seleccionado" />
                                <asp:Panel ID="pnlPuntos" Font-Names="Nina, Segoe, Arial Narrow, Arial, Verdana" Font-Size="X-Small" runat="server" BackColor="AliceBlue" Visible="false">
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td align="center" colspan="2">
                                                <asp:Button ID="btnSAVEPTO" runat="server" CssClass="rastreo_botones_save" CausesValidation="true"
                                                    Text="Guardar punto" />
                                                <asp:Button ID="btnCANCELPTO" runat="server" CssClass="rastreo_botones_cancel" CausesValidation="false"
                                                    Text="Cancelar" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Referencias de:
                                            </td>
                                            <td>
                                               <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlHDRg" AutoPostBack="true" Width="128px"></asp:DropDownList>
                                            </td>
                                            <td>
                                                Puntos                                                
                                            </td>
                                            <td>
                                                <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlPTOg" AutoPostBack="true" Width="128px"></asp:DropDownList>                                                
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4" align="center" style="background-color:Olive;color:White">
                                                    Datos del punto de la hoja de ruta:
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Nro. de Orden:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="pto_NroOrden" runat="server" CssClass="TextBox" 
                                                 onchange="if (isNaN(this.value)) { this.value='0'; this.focus()}" />
                                                <ajaxToolkit:MaskedEditExtender ID="mask_ptoorden" runat="server" 
                                                    Mask="999" MaskType="Number" InputDirection="RightToLeft"
                                                    TargetControlID="pto_NroOrden"/>
                                                    <asp:RequiredFieldValidator runat="server" 
                                                    ID="RequiredFieldValidator4" ControlToValidate="pto_NroOrden" 
                                                    SetFocusOnError="true" Text="*" 
                                                    ToolTip="Este campo es obligatorio"/>                                                
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Nombre del punto de referencia:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="pto_txtNom" runat="server" CssClass="TextBox"
                                                    ToolTip="Aqui debe ingresar el nombre abreviado para el punto. Ej: 'Estacion Nro.1' " />
                                                <asp:RequiredFieldValidator runat="server" 
                                                    ID="RequiredFieldValidator5" ControlToValidate="pto_txtNom" 
                                                    SetFocusOnError="true" Text="*" 
                                                    ToolTip="Este campo es obligatorio"/>   
                                            </td>
                                            <td>
                                                Descripcion del punto:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="pto_txtDesc" runat="server" CssClass="TextBox" />
                                                <asp:RequiredFieldValidator runat="server" 
                                                    ID="RequiredFieldValidator6" ControlToValidate="pto_txtDesc" 
                                                    SetFocusOnError="true" Text="*" 
                                                    ToolTip="Este campo es obligatorio"/> 
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Latitud:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="pto_txtLAT" Enabled="false" runat="server" CssClass="TextBox" />
                                                <asp:RequiredFieldValidator runat="server" 
                                                    ID="RequiredFieldValidator7" ControlToValidate="pto_txtLAT" 
                                                    SetFocusOnError="true" Text="*" 
                                                    ToolTip="Este campo es obligatorio"/> 
                                            </td>
                                            <td>
                                                Longitud:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="pto_txtLON" Enabled="false" runat="server" CssClass="TextBox" />
                                                <asp:RequiredFieldValidator runat="server" 
                                                    ID="RequiredFieldValidator8" ControlToValidate="pto_txtLON" 
                                                    SetFocusOnError="true" Text="*" 
                                                    ToolTip="Este campo es obligatorio"/> 
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Hora de llegada al punto
                                            </td>
                                            <td>
                                                <asp:TextBox ID="pto_txtHoraLlegada" runat="server" CssClass="TextBox" />
                                                <ajaxToolkit:MaskedEditExtender ID="mask_horallegada" runat="server" 
                                                    AcceptAMPM="false"  Mask="99:99" MaskType="Time" 
                                                    TargetControlID="pto_txtHoraLlegada" UserTimeFormat="TwentyFourHour" />
                                                <asp:RequiredFieldValidator runat="server" 
                                                    ID="RequiredFieldValidator9" ControlToValidate="pto_txtHoraLlegada" 
                                                    SetFocusOnError="true" Text="*" 
                                                    ToolTip="Este campo es obligatorio"/> 
                                            </td>
                                            <td>
                                                Tolerancia de minutos de llegada
                                            </td>
                                            <td>
                                                <asp:TextBox ID="pto_txtTOL" runat="server" CssClass="TextBox" />
                                                <ajaxToolkit:MaskedEditExtender ID="mask_TOL" runat="server" 
                                                    Mask="99" MaskType="Number"  InputDirection="RightToLeft" 
                                                    TargetControlID="pto_txtTOL"/>
                                                <asp:RequiredFieldValidator runat="server" 
                                                    ID="RequiredFieldValidator10" ControlToValidate="pto_txtTOL" 
                                                    SetFocusOnError="true" Text="*" 
                                                    ToolTip="Este campo es obligatorio"/> 
                                            </td>
                                        </tr>
                                        
                                    </table>
                                </asp:Panel>
<table>
<tr>
    <td>Busqueda:</td>
    <td>
        <asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlBuscar" Font-Size="X-Small"></asp:DropDownList>
    </td>
    <td><asp:TextBox runat="server" CssClass="TextBox" ID="txtBuscar"></asp:TextBox></td>
    <td><asp:Button runat="server" ID="btnBuscar" Text="Buscar!" CssClass="rastreo_botones" CausesValidation="false" />
    <asp:Button runat="server" ID="btnLimpiarBusqueda" Text="Limpiar busqueda" CssClass="rastreo_botones" CausesValidation="false" /></td>
</tr>  
</table>                                      

                                <asp:GridView ID="gv_Lista_Puntos_Recorrido" runat="server" AllowPaging="True" 
                                    DataKeyNames="id_punto, orden, hora_llegada, nombre, descripcion, lat, lon, minutos_demora"
                                    AllowSorting="True" AutoGenerateColumns="False" BackColor="White" 
                                    BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" 
                                    DataSourceID="sqlds_ListaPuntosRecorrido" Font-Names="Nina, Segoe, Arial Narrow, Arial, Verdana"
                                    Font-Size="XX-Small" GridLines="Vertical" PageSize="10" Width="100%">
                                    <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                                    <Columns>
                                    <asp:BoundField DataField="orden" HeaderText="Orden"/>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <a onclick="map_panTo(<%# Convert.ToString(Eval("lon")).Replace(",",".") %>,<%# Convert.ToString(Eval("lat")).Replace(",",".") %>);">
                                            <%#Eval("nombre")%>
                                            </a>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="descripcion" HeaderText="Descripcion"/>
                                    <asp:BoundField DataField="hora_llegada" HeaderText="Hora de llegada al punto" DataFormatString="{0:HH:mm}"/>
                                    <asp:BoundField DataField="minutos_demora" HeaderText="Tolerancia"/>
                                    <%--
                                    r_minutos_demora 
                                    r_lon
                                    r_lat
                                    --%>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Button ID="btnEditarPunto" runat="server" Text="Editar" Font-Size="X-Small" CssClass="rastreo_linkbuttons" CommandArgument='<%#Eval("id_punto")%>' CommandName="Editar"  />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Button ID="btnEliminarPunto" runat="server" Font-Size="X-Small" CssClass="rastreo_botones_cancel" Text="Eliminar" ToolTip="Eliminar recorrido" CommandArgument='<%#Eval("id_punto") %>' CommandName="Eliminar" OnClientClick="return seguro_que();" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    </Columns>
                                    <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                                    <PagerStyle BackColor="Navy" ForeColor="White" HorizontalAlign="Center" />
                                    <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                                    <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
                                    <AlternatingRowStyle BackColor="#BABABA" />
                                </asp:GridView>
                                <asp:SqlDataSource ID="sqlds_ListaPuntosRecorrido" runat="server" 
                                    ConnectionString="<%$ ConnectionStrings:CS_RASTREOSystem %>" 
                                    ProviderName="<%$ ConnectionStrings:CS_RASTREOSystem.ProviderName %>" 
                                    SelectCommand="SELECT * FROM rastreo_hoja_de_ruta_puntos WHERE idhoja_de_ruta = @idhoja_de_ruta ORDER BY orden">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="idhoja_de_ruta" ConvertEmptyStringToNull="false" 
                                            DefaultValue="" Name="idhoja_de_ruta" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                                </asp:Panel>
                            </td>
                        </tr>
                     </table>
                    </asp:Panel>
                </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        </table>
    </div>
    </form>
    <script src="http://maps.google.com/maps/api/js?v=3.2&sensor=false"></script>
    <script type="text/javascript" src="FuncionesGenerales.js"></script>    
    <script type="text/javascript" src="OpenLayers.js"></script>
    <script type="text/javascript" src="FuncionesGPS.js"></script>  
</body>
</html>