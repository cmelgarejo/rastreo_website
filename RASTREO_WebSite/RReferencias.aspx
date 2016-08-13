<%@ Page Title="RASTREO Paraguay - Puntos de Referencia" Language="VB" AutoEventWireup="false" CodeFile="~/RReferencias.aspx.vb" Inherits="Referencias" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<!-- Autor: Ing. Adolfo Bresanovich || Director de R&D RASTREO Paraguay -->

<head id="GPSadminHead" runat="server">

    <link rel="shortcut icon" href="~/RASTREO.ico" type="image/x-icon"/>
    <link rel="icon" href="~/RASTREO.ico" type="image/x-icon"/>
    <title>RASTREO WebSystem - Bienvenido!</title>

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

    <div id="RASTREO_mainwrapper">
        <asp:UpdateProgress runat="server" ID="updpg_pnlGPS" AssociatedUpdatePanelID="updpnlControlhoja_de_ruta">
            <ProgressTemplate>
                <div style="position:absolute;left:0px;top:0px;width:100%;height:1000%" class="modalBackground">
                    <img src="./App_Themes/CENTRAL/Imagenes/ajax-loader.gif" alt="-"/>
                    <label class="rastreo_nowloading">Actualizando...</label>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>    
        <asp:Label ID="errMsgs" runat="server" Width="99%" CssClass="Mensaje_de_error" 
        Visible="false"></asp:Label>
        <table width="100%" cellpadding="0" cellspacing="0">
        <tr>
        <td colspan="2">
            <table width="100%"  cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <div id="map"></div>            
                    </td>
                </tr>
            </table>

        </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:UpdatePanel runat="server" ID="updpnlControlhoja_de_ruta" Visible="true">
                <ContentTemplate>
                    <asp:Panel runat="server" ID="pnlSetuphoja_de_ruta" BorderStyle="Solid" BorderWidth="2px" BorderColor="Navy">
                    <asp:Label runat="server" ID="lbhoja_de_rutaPanel" Text="Hojas de Referencias" CssClass="rastreo_linkbuttons" />
                    <asp:HiddenField runat="server" ID="id_recorridotemplate" Value="0" />                    
                    <table>
                        <tr valign="top">
                            <td>
                                <asp:Button ID="btnCargahoja_de_ruta" runat="server" CssClass="rastreo_botones" 
                                    Text="Agregar Hoja de Referencia" ToolTip="Carga de hoja de referencia del usuario seleccionado" />
                                <asp:Panel ID="pnlhoja_de_ruta" runat="server" BackColor="AliceBlue" 
                                    Visible="false">
                                    <table>
                                        <tr>
                                            <td colspan="2">
                                                <asp:CheckBox ID="rec_chkACT" runat="server" Text="Publico?" Checked="true"
                                                    ToolTip="Determina si la hoja de referencia sera visible para los demas usuarios de su cuenta." />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Descripcion:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="rec_txtDESC" runat="server" CssClass="TextBox" 
                                                    ToolTip="Aqui debe ingresar la descripcion de la hoja de referencia. Ej: 'Farmacias' o 'Supermercados' " />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="2">
                                                <asp:Button ID="btnSAVEREC" runat="server" CssClass="rastreo_botones_save" 
                                                    OnClientClick="DisableSaveControl(this);" Text="Guardar hoja de referencia" />
                                                <asp:Button ID="btnCANCELREC" runat="server" CssClass="rastreo_botones_cancel" 
                                                    Text="Cancelar" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:GridView ID="gv_Hoja_de_Ruta_template" runat="server" AllowPaging="True" 
                                    AllowSorting="True" AutoGenerateColumns="False" BackColor="White"
                                    DataKeyNames="id_recorridotemplate, publico" 
                                    BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" 
                                    DataSourceID="sqlds_hoja_de_ruta" Font-Names="Arial Narrow, Arial, Verdana" Font-Size="XX-Small" 
                                    GridLines="Vertical" PageSize="10" Width="100%">
                                    <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                                    <Columns>
                                        <asp:CommandField ShowSelectButton="true" SelectText="Seleccionar" ItemStyle-ForeColor="White"
                         ControlStyle-CssClass="rastreo_botones_save" HeaderText="Seleccionar" />
                                        <asp:BoundField DataField="descripcion_recorrido" HeaderText="Descripcion" />
                                        <asp:CheckBoxField DataField="publico" HeaderText ="Publico?" ReadOnly="true" />
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button runat="server" Text="Editar" Font-Size="X-Small" CssClass="rastreo_linkbuttons" CommandArgument='<%#Eval("id_recorridotemplate")%>' CommandName="Editar"  />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button runat="server" Font-Size="X-Small" CssClass="rastreo_botones_cancel" Text="Eliminar" ToolTip="Eliminar hoja de referencia" CommandArgument='<%#Eval("id_recorridotemplate") %>' CommandName="Eliminar" OnClientClick="return seguro_que();" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                                    <PagerStyle BackColor="Navy" ForeColor="White" HorizontalAlign="Center" />
                                    <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                                    <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
                                    <AlternatingRowStyle BackColor="#BABABA" />
                                </asp:GridView>
                                <asp:SqlDataSource ID="sqlds_hoja_de_ruta" runat="server" 
                                    ConnectionString="<%$ ConnectionStrings:CS_RASTREOSystem %>" 
                                    ProviderName="<%$ ConnectionStrings:CS_RASTREOSystem.ProviderName %>" 
                                    
                                    SelectCommand="SELECT * FROM rastreo_template_hoja_de_ruta WHERE rastreoref = TRUE AND publico = TRUE order by descripcion_recorrido">
                                </asp:SqlDataSource>                                
                            </td>
                            <td>
                                <asp:Panel runat="server" ID="pnlSelechoja_de_ruta" Visible="false">
                                <asp:Button ID="btnCargaPuntoshoja_de_ruta" runat="server" 
                                    CssClass="rastreo_botones" Text="Agregar puntos a la hoja de referencia" 
                                    ToolTip="Agrega de puntos de hoja de referencia seleccionada" />
                                <asp:Panel ID="pnlPuntos" runat="server" BackColor="AliceBlue" Visible="false">
                                    <table>
                                        <tr>
                                            <td align="center" colspan="2">
                                                <asp:Button ID="btnSAVEPTO" runat="server" CssClass="rastreo_botones_save" 
                                                    OnClientClick="DisableSaveControl(this);" Text="Guardar punto" />
                                                <asp:Button ID="btnCANCELPTO" runat="server" CssClass="rastreo_botones_cancel" 
                                                    Text="Cancelar" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Nombre del punto de referencia:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="pto_txtNom" runat="server" CssClass="TextBox" 
                                                    ToolTip="Aqui debe ingresar el nombre abreviado para el punto. Ej: 'Estacion Nro.1' " />
                                            </td>
                                            <td>
                                                Descripcion del punto:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="pto_txtDesc" runat="server" CssClass="TextBox" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Latitud:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="pto_txtLAT" runat="server" CssClass="TextBox" />
                                            </td>
                                            <td>
                                                Longitud:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="pto_txtLON" runat="server" CssClass="TextBox" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Metros a la redonda de detección de la referencia
                                            </td>
                                            <td colspan="2">
                                                <asp:TextBox ID="pto_txtMETROS_REDONDA" runat="server" CssClass="TextBox" onchange="if (isNaN(this.value)) { this.value='100'; this.focus()}" Width="100%" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:CheckBox runat="server" ID="pto_chkReferencia" Text="Deteccion de ingreso a la referencia <br />[separar los mail con <b>;</b> o <b>,</b>]"
                                                    ToolTip="Chequee esta casilla si desea que se le avise cuando un vehiculo ingrese a este punto de referencia." Checked="false" />
                                            </td>
                                            <td colspan="3">
                                                <asp:TextBox ID="txtMailReferencias_ptoref" runat="server" CssClass="TextBoxMAIL" TextMode="MultiLine" ToolTip="Ingrese en esta casilla los mail a donde será enviados los avisos cuando ingrese un vehículo a esta referencia." Height="100%" Width="100%" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:GridView ID="gv_Lista_Puntos_hoja_de_ruta" runat="server" AllowPaging="True" 
                                    DataKeyNames="id_puntostemplate, id_recorridotemplate, nombre, descripcion, lon, lat"
                                    AllowSorting="True" AutoGenerateColumns="False" BackColor="White" 
                                    BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" 
                                    DataSourceID="sqlds_ListaPuntoshoja_de_ruta" Font-Names="Arial Narrow, Arial, Verdana"
                                    Font-Size="XX-Small" GridLines="Vertical" PageSize="10" Width="100%">
                                    <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                                    <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <a onclick="map_panTo(<%# Convert.ToString(Eval("lon")).Replace(",",".") %>,<%# Convert.ToString(Eval("lat")).Replace(",",".") %>);">
                                            <%#Eval("nombre")%>
                                            </a>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="descripcion" HeaderText="Descripcion"/>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Button ID="btnEditarPunto" runat="server" Text="Editar" Font-Size="X-Small" CssClass="rastreo_linkbuttons" CommandArgument='<%#Eval("id_puntostemplate")%>' CommandName="Editar"  />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>                                            
                                            <asp:Button ID="btnEliminarPunto" runat="server" Font-Size="X-Small" CssClass="rastreo_botones_cancel" Text="Eliminar" ToolTip="Eliminar hoja_de_ruta" CommandArgument='<%#Eval("id_puntostemplate") %>' CommandName="Eliminar" OnClientClick="return seguro_que();" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    </Columns>
                                    <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                                    <PagerStyle BackColor="Navy" ForeColor="White" HorizontalAlign="Center" />
                                    <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                                    <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
                                    <AlternatingRowStyle BackColor="#BABABA" />
                                </asp:GridView>
                                <asp:SqlDataSource ID="sqlds_ListaPuntoshoja_de_ruta" runat="server" 
                                    ConnectionString="<%$ ConnectionStrings:CS_RASTREOSystem %>" 
                                    ProviderName="<%$ ConnectionStrings:CS_RASTREOSystem.ProviderName %>" 
                                    
                                        SelectCommand="SELECT * FROM rastreo_template_puntos_hoja_de_ruta WHERE id_recorridotemplate = @id_recorridotemplate order by nombre">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="id_recorridotemplate" ConvertEmptyStringToNull="false" 
                                            DefaultValue="" Name="id_recorridotemplate" />
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
    <script type="text/javascript" src="FuncionesGenerales.js"></script>    
    <script src="http://maps.google.com/maps/api/js?v=3.2&sensor=false"></script>
    <script type="text/javascript" src="OpenLayers.js"></script>
    <script type="text/javascript" src="FuncionesGPS.js"></script>
    <%--<script src='http://dev.virtualearth.net/mapcontrol/mapcontrol.ashx?v=6.1'></script>--%>
    <%--<script src="http://api.maps.yahoo.com/ajaxymap?v=3.0&appid=euzuro-openlayers"></script>--%>
    <%--<script type="text/javascript" src="http://www.openlayers.net/api/OpenLayers.js"></script>--%>    
</body>
</html>




