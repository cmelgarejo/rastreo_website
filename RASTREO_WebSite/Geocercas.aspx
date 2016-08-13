<%@ Page Title="RASTREO Paraguay - GEOCERCA - " Language="VB" AutoEventWireup="false" CodeFile="Geocercas.aspx.vb" Inherits="GeoCercas" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">

<head id="GPSadminHead" runat="server">

    <link rel="shortcut icon" href="~/RASTREO.ico" type="image/x-icon"/>
    <link rel="icon" href="~/RASTREO.ico" type="image/x-icon"/>
    <title>RASTREO WebSystem - Bienvenido!<%=vehiculo_seleccionado%></title>
    <%--    <%If Request.ServerVariables("REMOTE_HOST") = "192.168.1.2" Then%>--%>
    <%--<script src="http://maps.google.com/maps?file=api&amp;v=2&amp;sensor=false&amp;key=ABQIAAAA5OEr6wr1CP_TFa6oFKhdERTrReK3P99mKBTfLTiMIdvPR6kVWRTshY6odSRy6ukvH8fz-9K6FrZu1Q" type="text/javascript"></script>--%>
    <%--    <%If Request.ServerVariables("REMOTE_HOST") = "192.168.1.2" Then%>--%>
    <%--<script src="http://maps.google.com/maps?file=api&amp;v=2&amp;sensor=false&amp;key=ABQIAAAA5OEr6wr1CP_TFa6oFKhdERTrReK3P99mKBTfLTiMIdvPR6kVWRTshY6odSRy6ukvH8fz-9K6FrZu1Q" type="text/javascript"></script>--%>
    <script src="http://maps.google.com/maps/api/js?v=3.2&sensor=false"></script>
    <script type="text/javascript" src="FuncionesGenerales.js"></script>    
    <script type="text/javascript" src="OpenLayers.js"></script>
    <script type="text/javascript" src="FuncionesGPS.js"></script>

</head>
<body onload="geocerca_init_map();">
    <form id="RASTREOmainForm" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="RASTREOToolScriptManager" ScriptMode="Release" runat="server">
        <Scripts>
            <asp:ScriptReference Path="~/WebKit.js" />
        </Scripts>
    </ajaxToolkit:ToolkitScriptManager>    
    <div id="RASTREO_mainwrapper">
        <asp:UpdateProgress runat="server" ID="updpg_pnlGEOCERCAGPS" AssociatedUpdatePanelID="updpnlControlGeoCerca">
            <ProgressTemplate>
                <div style="position:absolute;left:0px;top:0px;width:100%;height:1000%" class="modalBackground">
                    <img src="./App_Themes/CENTRAL/Imagenes/ajax-loader.gif" alt="-"/>
                    <label class="rastreo_nowloading">Actualizando...</label>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>

        <table width="100%" cellpadding="0" cellspacing="0">
        <tr>
        <td>
            <div id="map"></div>
        </td>
        </tr>
        <tr>
            <td>
                <asp:UpdatePanel runat="server" ID="updpnlControlGeoCerca" Visible="true">
                <ContentTemplate>
                           
                    <input type="hidden" id="insGeocercaPuntos" runat="server" />
                    <asp:HiddenField ID="txtfile" runat="server" />
                    <asp:Label ID="errMsgs" runat="server" Width="99%" CssClass="Mensaje_de_error" Visible="false"></asp:Label>
                    <asp:Panel runat="server" ID="pnlSetupGeoCerca" BorderStyle="Solid" BorderWidth="2px" BorderColor="Navy">
                    <asp:Label runat="server" ID="lbGeoCercaPanel" Text="Geocercas" CssClass="rastreo_linkbuttons" />
                    <asp:HiddenField runat="server" ID="geocerca_puntos" Value="" />                    
                    <table>
                        <tr valign="top">
                            <td>
                                <asp:Button ID="btnCargaGeoCerca" runat="server" CssClass="rastreo_botones" 
                                    Text="Agregar geocerca del vehiculo" ToolTip="Agrega geocerca del vehiculo seleccionado" />
                                <asp:Panel ID="pnlGeoCerca" runat="server" BackColor="AliceBlue" 
                                    Visible="false">
                                    <table>
                                        <tr>
                                            <td align="center" colspan="2">
                                                <asp:Button ID="btnSAVEREC" runat="server" CssClass="rastreo_botones_save" 
                                                    Text="Guardar geocerca" />
                                                <asp:Button ID="btnCANCELREC" runat="server" CssClass="rastreo_botones_cancel" 
                                                    Text="Cancelar" />
                                            </td>
                                            <td colspan="2">
                                                <input id="mensajegeocerca" class="PopUp" type="text" runat="server" readonly="readonly" style="width:99%" value="Sin geocerca seleccionada"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Aplicar geocerca a todos mis vehiculos
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkCopyAllGEOCERCA" runat="server"
                                                    ToolTip="Con esto chequeado, esta geocerca se aplicará a todos sus vehiculos." />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Activa?:
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkActiva" runat="server"
                                                    ToolTip="Indique si esta activa o no la geocerca." />
                                            </td>
                                            <td>
                                                Descripcion:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="rec_txtDESC" runat="server" CssClass="TextBox" 
                                                    ToolTip="Aqui debe ingresar la descripcion de la geocerca. Ej: 'Zona permitida de movimiento' o 'Pais permitido' " />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:RadioButtonList runat="server" ID="rblOPCIONESACTIVACION" AutoPostBack="true">
                                                    <asp:ListItem Selected="True" Text="Hora de inicio/duracion" Value="1" />
                                                    <asp:ListItem Text="Todo el dia"  Value="2"/>
                                                    <asp:ListItem Text="Un día especifico" Value="3"/>
                                                </asp:RadioButtonList>
                                            </td>
                                            <td colspan="2">
                                                <asp:Panel runat="server" ID="pnlLimitHora" Visible="false" BackColor="RoyalBlue" ForeColor="White">
                                                <table cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            Hora de activacion (todos los dias)
                                                            <asp:TextBox ID="txtHoraInicio" runat="server" CssClass="TextBox"/>
                                                        </td>
                                                        <td>
                                                            Horas activa
                                                            <asp:TextBox ID="txtHorasActiva" runat="server" CssClass="TextBox"
                                                            onchange="if (isNaN(this.value)) { this.value='1'; this.focus();} else {if(this.value>=24||this.value<=0)this.value=23;}" />
                                                        </td>
                                                    </tr>
                                                </table>
                                                </asp:Panel>
                                                <asp:Panel runat="server" ID="pnlDiaDeActivacion" Visible="false" BackColor="DarkGreen" ForeColor="White">
                                                    Dia de activacion
                                                    <ajaxToolkit:MaskedEditExtender runat="server" AcceptAMPM="false" ID="mask_txtDiaDeActivacion" 
                                                        ClearMaskOnLostFocus="true" TargetControlID="txtDiaDeActivacion" 
                                                        Mask="99/99/9999" MaskType="Date" UserDateFormat="DayMonthYear"/>
                                                    <asp:TextBox ID="txtDiaDeActivacion" runat="server" CssClass="TextBox"/>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                Detectar ingreso a la geocerca.
                                            </td>
                                            <td colspan="2">
                                                <asp:CheckBox ID="chkInversa" runat="server"
                                                    ToolTip="Indique si esta geocerca será para detectar ingreso en lugar de salida de la zona, si la casilla esta chequeada significará que se detectará entrada, en caso contrario, salida de la misma." />
                                            </td>                                            
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                Detectar ingreso y egreso de a la geocerca.
                                            </td>
                                            <td colspan="2">
                                                <asp:CheckBox ID="chkIngresoEgreso" runat="server"
                                                    ToolTip="Indique si esta geocerca será para detectar ingreso y tambien su egreso de la zona." />
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
                                <asp:GridView ID="gv_GeoCerca" runat="server" AllowPaging="True" 
                                    AllowSorting="True" AutoGenerateColumns="False" BackColor="White"
                                    DataKeyNames="idrastreo_geocercas, descripcion, puntos" 
                                    BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" 
                                    DataSourceID="sqlds_GeoCerca" Font-Names="Nina, Segoe, Arial Narrow, Arial, Verdana" Font-Size="XX-Small" 
                                    GridLines="Vertical" PageSize="10" Width="100%">
                                    <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                                    <Columns>
                                        <asp:CommandField ShowSelectButton="true" SelectText="Seleccionar" 
                                        ItemStyle-ForeColor="White" ControlStyle-CssClass="rastreo_botones_save" 
                                        HeaderText="Seleccionar" />
                                        <asp:BoundField DataField="descripcion" HeaderText="Descripcion" />
                                        <asp:CheckBoxField ReadOnly="true" DataField="activa" HeaderText="Activa?" />
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button runat="server" Text="Editar" Font-Size="X-Small" CssClass="rastreo_linkbuttons" CommandArgument='<%#Eval("idrastreo_geocercas")%>' CommandName="Editar"  />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button runat="server" Font-Size="X-Small" CssClass="rastreo_botones_cancel" Text="Eliminar" ToolTip="Eliminar geocerca" CommandArgument='<%#Eval("idrastreo_geocercas") %>' CommandName="Eliminar" OnClientClick="return seguro_que();" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                                    <PagerStyle BackColor="Navy" ForeColor="White" HorizontalAlign="Center" />
                                    <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                                    <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
                                    <AlternatingRowStyle BackColor="#BABABA" />
                                </asp:GridView>
                                <asp:SqlDataSource ID="sqlds_GeoCerca" runat="server" 
                                    ConnectionString="<%$ ConnectionStrings:CS_RASTREOSystem %>" 
                                    ProviderName="<%$ ConnectionStrings:CS_RASTREOSystem.ProviderName %>" 
                                    SelectCommand="SELECT * FROM rastreo_geocercas WHERE id_vehiculo = @id_vehiculo AND id_cliente = @id_cliente AND id_usuario = @id_usuario">
                                    <SelectParameters>
                                        <asp:QueryStringParameter QueryStringField="vid" ConvertEmptyStringToNull="false" DefaultValue="" Name="id_vehiculo" />
                                        <asp:QueryStringParameter QueryStringField="cid" ConvertEmptyStringToNull="false" DefaultValue="" Name="id_cliente" />
                                    <asp:QueryStringParameter QueryStringField="uid" ConvertEmptyStringToNull="false" DefaultValue="" Name="id_usuario" />
                                    </SelectParameters>
                                </asp:SqlDataSource>                                
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

</body>
</html>




