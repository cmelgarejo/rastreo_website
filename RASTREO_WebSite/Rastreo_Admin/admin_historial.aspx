<%@ Page Title="RASTREO Paraguay - Administración - Lista de Eventos historiales" Language="VB" MasterPageFile="RASTREOMasterPage.master" AutoEventWireup="false" CodeFile="admin_historial.aspx.vb" Inherits="RASTREO_Administracion_admin_historial" %>
<asp:Content ID="Content2" ContentPlaceHolderID="main_ContentPlaceHolder" Runat="Server">
    <asp:UpdatePanel ID="updPanel_Contactos" runat="server" EnableViewState="true" >
        <ContentTemplate>
            <asp:Label ID="errMsgs" runat="server" Width="99%" CssClass="Mensaje_de_error" 
                Visible="False"></asp:Label>
            <table style="width:100%">
                <tr><td colspan="6" style="border: solid 3px navy">
                    Cliente:
                    <asp:Label runat="server" CssClass="Label" ID="lblCliente" />
                </td></tr>
                <tr>
                    <td>
                        <asp:Button runat="server" ID="btnAddEvento" Text="Agregar evento" CssClass="rastreo_botones" ToolTip="Haga click aqui para agregar un evento" />
                    </td>
                    <td>Busqueda:</td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlBuscar" CssClass="DropDownList"></asp:DropDownList>
                    </td>
                    <td><asp:TextBox runat="server" CssClass="TextBox" ID="txtBuscar"></asp:TextBox></td>
                    <td><asp:Button runat="server" ID="btnBuscar" Text="Buscar!" CssClass="rastreo_botones" /><asp:Button runat="server" ID="btnLimpiarBusqueda" Text="Limpiar busqueda" CssClass="rastreo_botones" /></td>
                    <td>
                        <% If Request.QueryString.Count > 0 Then%>                   
                        <input class="rastreo_botones" onclick="window.location.href='admin_personas_lista.aspx';window.close();return true;" 
                            type="button" value="Volver" />
                        <% End If%>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <asp:UpdatePanel runat="server" ID="updpnlAddHistorial" Visible="false">
                            <ContentTemplate>
                                <asp:Panel ID="pnlAddHistorial" runat="server" Width="100%" 
                                                 BackColor="#778899" ForeColor="White" BorderColor="Silver" BorderWidth="3px" >
                                    <table width="100%">
                                                    <tr>
                                                                <td style="text-align:right">
                                                                    <asp:Button ID="btnGUARDAR" runat="server" OnClientClick="DisableSave(this);"
                                                                        CssClass="rastreo_botones_save" Text="Guardar" />
                                                                    <asp:Button ID="btnCANCELAR" runat="server" CausesValidation="false" 
                                                                        CssClass="rastreo_botones_cancel" Text="Cancelar" />
                                                                </td>
                                                            </tr>
                                                    <tr>
                                                    <td>
                                                            Vehiculos<br /><asp:DropDownList CssClass="DropDownList" runat="server" ID="ddl_Vehiculos" Width="50%" />
                                                            <asp:Button runat="server" ID="btnAgregarVehiculo" Text="Agregar vehiculo a la lista" CssClass="rastreo_botones"/>
                                                            <br /><br />
                                                            Vehiculos Involucrados<br />
                                                            <asp:ListBox runat="server" ID="listVehiculosInvolucrados" Width="50%" CssClass="DropDownList"/>
                                                            <asp:Button runat="server" ID="btnEliminarVehiculo" Text="Eliminar vehiculo de la lista" CssClass="rastreo_botones" OnClientClick="return seguro_que();" />
                                                            <br />
                                                            Fecha y hora del evento: 
                                                            <asp:TextBox CssClass="TextBox" AutoPostBack="False" runat="server" 
                                                            ID="txtHISTORIAL_fechaevento" ToolTip="Ingrese la fecha de instalación" 
                                                            Width="160px"/>
                                                            <!--
                                                            <ajaxToolkit:CalendarExtender CssClass="MyCalendar" ID="CalendarExtender1" runat="server" Format="dd/MM/yyyy hh:mm:ss" PopupPosition="TopRight" Animated="true" TargetControlID="txtHISTORIAL_fechaevento" />
                                                            -->
                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtHISTORIAL_fechaevento"
                                                            ErrorMessage="Fecha y/o hora inválidas"
                                                            ValidationExpression ="^((((([0-1]?\d)|(2[0-8]))\/((0?\d)|(1[0-2])))|(29\/((0?[1,3-9])|(1[0-2])))|(30\/((0?[1,3-9])|(1[0-2])))|(31\/((0?[13578])|(1[0-2]))))\/((19\d{2})|([2-9]\d{3}))|(29\/0?2\/(((([2468][048])|([3579][26]))00)|(((19)|([2-9]\d))(([2468]0)|([02468][48])|([13579][26]))))))\s(([01]?\d)|(2[0-3]))(:[0-5]?\d){2}.*$" 
                                                            SetFocusOnError = "true" ></asp:RegularExpressionValidator>                                                                        
                                                            <ajaxToolkit:MaskedEditExtender ID="mask_fechaevento" runat="server" 
                                                            AcceptAMPM="false" ClearMaskOnLostFocus="true" 
                                                            TargetControlID="txtHISTORIAL_fechaevento" Mask="99/99/9999 99:99:99" 
                                                            MaskType="DateTime" ClearTextOnInvalid="True" UserDateFormat="DayMonthYear" />
                                                            <br />
                                                            Observaciones<br />
                                                            <asp:TextBox runat="server" Width="99%" Height="100px" CssClass="TextBox" ID="txtObservacion" />
                                                        </td>
                                                    </tr>                            
                                                </table>
                                </asp:Panel>
                           </ContentTemplate>
                        </asp:UpdatePanel>
                        <asp:GridView PageSize="30"  ID="gv_Lista_Historial" runat="server" 
                        DataSourceID="sqlds_Lista_Historial" AllowPaging="True" 
                        AllowSorting="True" AutoGenerateColumns="False" BackColor="White" 
                        BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" 
                            GridLines="Vertical" Width="100%"
                            PagerSettings-PageButtonCount="20" PagerSettings-Position="TopAndBottom">
                        <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                        <Columns>           
                        <asp:BoundField HeaderText="ID" DataField="idrastreo_historial"/>             
                        <asp:BoundField HeaderText="Vehiculos involucrados" DataField="vehiculos_involucrados" />
                        <asp:BoundField HeaderText="Observaciones" DataField="observacion" />
                        <asp:BoundField HeaderText="Fecha" DataField="fecha_verificacion" />
                        <asp:BoundField HeaderText="Ingresado por" DataField="empleado_ins" />
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Button ID="btnEditHistorial" runat="server" Text="Editar" CssClass="rastreo_botones" CommandArgument='<%#Eval("idrastreo_historial")%>' CommandName="Editar" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Button ID="btnDeleteHistorial" runat="server" Text="Eliminar" CssClass="rastreo_botones_cancel" CommandArgument='<%#Eval("idrastreo_historial")%>' CommandName="Eliminar" OnClientClick="return seguro_que();" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        </Columns>
                        <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                        <PagerStyle BackColor="Navy" ForeColor="White" HorizontalAlign="Center" />
                        <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
                        <AlternatingRowStyle BackColor="#BABABA" />
                        </asp:GridView>
                        <asp:SqlDataSource ID="sqlds_Lista_Historial" runat="server" ConnectionString="<%$ ConnectionStrings:CS_RASTREOSystem %>" ProviderName="<%$ ConnectionStrings:CS_RASTREOSystem.ProviderName %>"
                        SelectCommand="SELECT 
                                        rastreo_historial.*, 
                                        CASE
                                            WHEN rastreo_persona.tipo_persona = 'F'::bpchar THEN ((rastreo_persona.nombre::text || ' '::text) || rastreo_persona.apellido::text)::character varying
                                            ELSE rastreo_persona.razon_social
                                        END AS empleado_ins
                                        FROM rastreo_historial 
                                        LEFT JOIN rastreo_usuarios ON rastreo_usuarios.idrastreo_usuarios = rastreo_historial.user_ins
                                        LEFT JOIN rastreo_persona ON rastreo_persona .idrastreo_persona  = rastreo_usuarios.id_persona
                                        WHERE id_cliente = @id_cliente">
                        <SelectParameters>
                            <asp:QueryStringParameter DbType="Int32" Name="id_cliente" QueryStringField="cid" />
                        </SelectParameters>
                        </asp:SqlDataSource>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

