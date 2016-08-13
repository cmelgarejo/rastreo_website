<%@ Page Title="RASTREO Paraguay - Administración - Lista de Personas" Language="VB" MasterPageFile="RASTREOMasterPage.master" EnableEventValidation="false" AutoEventWireup="true" EnableViewState="false" CodeFile="admin_personas_lista.aspx.vb" Inherits="RASTREO_Administracion_admin_personas_lista" %>
<asp:Content ID="Content2" ContentPlaceHolderID="main_ContentPlaceHolder" Runat="Server">
        <script language="javascript" type="text/javascript">
            function seguro_que_cliente()
            {
                 var seguroque = confirm("Seguro que vas a eliminar al cliente? \n Se eliminarán sus vehiculos, contactos, usuario y opciones.");
                 if (seguroque  == true)
                 {
                     return true;
                 }
                 else
                 {
                     return false;
                 }
             }
             function set_COLVAL(value) {
                 if (document.getElementById("main_ContentPlaceHolder_ColVal") != null) {
                     var OHYEAH = document.getElementById("main_ContentPlaceHolder_ColVal");
                     OHYEAH.value = value;
                 }
             }
             function set_ExportaExcel(value) {
                 if (document.getElementById("main_ContentPlaceHolder_ExportaExcel") != null) {
                     var OHFACK = document.getElementById("main_ContentPlaceHolder_ExportaExcel");
                     OHFACK.value = value;
                 }
             }
        </script>

            <asp:Button CssClass="rastreo_botones_save" runat="server" ID="btnExcel" 
            Text="Descargar planilla" OnClientClick="set_ExportaExcel(true);"
            ToolTip="Haga click aqui para descargar la lista visible en uan planilla" Visible="true"/>

    <asp:UpdateProgress runat="server" ID="updpg_Personas" AssociatedUpdatePanelID="updPanel_Personas" >
        <ProgressTemplate>
            <img src="../App_Themes/CENTRAL/Imagenes/ajax-loader.gif" alt="X"/>
            <asp:Label runat="server" ID="lbActualizando" CssClass="rastreo_nowloading" Font-Size="Smaller">
            Actualizando...
            </asp:Label>
        </ProgressTemplate>
    </asp:UpdateProgress>
        
    <asp:UpdatePanel ID="updPanel_Personas" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="ColVal" runat="server" Value=""/>
            <asp:HiddenField ID="ExportaExcel" runat="server" Value="false"/>
            <asp:Label ID="errMsgs" runat="server" Width="99%" CssClass="Mensaje_de_error" 
                Visible="False"></asp:Label>
            <table style="width:100%">
                <tr>
                    <td>
                        <asp:Button runat="server" ID="btnAddPersona" Text=" Agregar Persona " CssClass="btnplus2" Width="100%" />
                    </td>
                    <td>Busqueda:</td>
                    <td>
                        <asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlBuscar" Font-Size="X-Small" Width="250px"></asp:DropDownList>
                    </td>
                    <td><asp:TextBox runat="server" CssClass="TextBox" ID="txtBuscar"></asp:TextBox></td>
                    <td><asp:Button runat="server" ID="btnBuscar" Text="Buscar!" CssClass="rastreo_botones" Width="60px"   OnClientClick="set_COLVAL(ctl00$main_ContentPlaceHolder$ddlBuscar[ctl00$main_ContentPlaceHolder$ddlBuscar.selectedIndex].value);"/></td>
                    <td><asp:Button runat="server" ID="btnLimpiarBusqueda" Text="Limpiar" CssClass="rastreo_botones" Width="70px" /></td>
                </tr>
                <tr>
                    <td><asp:CheckBox AutoPostBack="true" runat="server" ID="chkFiltro" Text="Mostrar empleados" CssClass="CheckBoxes" Font-Size="Smaller"/></td>
                    <td><asp:CheckBox AutoPostBack="true" runat="server" ID="chkTipoPersona_FISICA" Text="Personas fisicas" CssClass="CheckBoxes" Font-Size="Smaller"/></td>
                    <td><asp:CheckBox AutoPostBack="true" runat="server" ID="chkTipoPersona_JURIDICA" Text="Personas juridicas" CssClass="CheckBoxes" Font-Size="Smaller"/></td>
                    <td><label style="font-size:smaller">Tamaño de pagina:&nbsp;</label><asp:TextBox runat="server" CssClass="TextBoxMAIL" ID="txtPageSize" ToolTip="Tamaño de la lista" MaxLength="5" Width="70px" Text="20" 
                    onchange="if (isNaN(this.value)) { this.value='0'; this.focus()}"></asp:TextBox></td>
                    <td><asp:ImageButton ID="btn_refresh_lista" runat="server" ImageUrl="~/App_Themes/CENTRAL/Imagenes/view-refresh.png" /></td>
                    <td></td>
                </tr>
                <tr>
                    <td colspan="6">
                        <asp:GridView PageSize="20"  ID="gv_Lista_Personas" runat="server" 
                        DataSourceID="sqlds_Lista_Personas" AllowPaging="True" 
                        AllowSorting="True" AutoGenerateColumns="False" BackColor="White" 
                        BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" 
                            GridLines="Vertical" Width="100%" Font-Names="Arial Narrow, Arial, Verdana"
                            Font-Size="XX-Small">
                        <RowStyle BackColor="#EEEEEE" ForeColor="Black"  />
                        <Columns>
                        <%--<asp:BoundField HeaderText="Id" DataField="idrastreo_persona" />--%>
                        <asp:TemplateField> 
                            <HeaderTemplate> Nombre </HeaderTemplate>
                            <ItemTemplate>
                                <a class="Label" href="admin_personas.aspx?id=<%# Eval("idrastreo_persona") %>">
                                  <%#IIf(Not Eval("cliente") Is DBNull.Value, Eval("cliente"), "(Completar nombre de cliente...)")%> 
                                </a>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Nro. Documento" DataField="nro_documento" />
                        <asp:BoundField HeaderText="Cant. moviles" DataField="cantidad_de_vehiculos" HeaderStyle-Width="70px" />
                        <asp:TemplateField>
                            <ItemTemplate>
                                <ajaxToolkit:HoverMenuExtender runat="server" ID="masterHuver" TargetControlID="lnkDatos" PopupControlID="pnlDatos" PopupPosition="Top">
                                </ajaxToolkit:HoverMenuExtender>
                                <asp:Label runat="server" ID="lnkDatos" CssClass="DropDownList" >Datos</asp:Label>
                                <asp:Panel BorderColor="Black" BorderWidth="2px" BackColor="Black" ForeColor="White" Font-Size="X-Small" ID="pnlDatos" runat="server">
                                   <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            Cliente:
                                        </td>
                                        <td>
                                            <%#Eval("cliente")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Pais:</td> 
                                        <td><%#Eval("pais")%></td>
                                    </tr>
                                    <tr>
                                        <td>Distrito:</td> 
                                        <td><%#Eval("distrito")%></td>
                                    </tr>
                                    <tr>
                                        <td>Ciudad:</td> 
                                        <td><%#Eval("ciudad")%></td>
                                    </tr>            
                                    <tr>
                                        <td>Telefono movil:</td> 
                                        <td><%#Eval("tel_movil")%></td>
                                    </tr>
                                    <tr>
                                        <td>Telefono particular:</td> 
                                        <td><%#Eval("tel_part")%></td>
                                    </tr>
                                    <tr>
                                        <td>Telefono oficina:</td> 
                                        <td><%#Eval("tel_ofi")%></td>
                                    </tr>
                                    <tr>
                                        <td>e-mails:</td> 
                                        <td><%#Eval("email")%></td>
                                    </tr>              
                                   </table>
                                </asp:Panel>
                            </ItemTemplate>
                        </asp:TemplateField>                  
                        <asp:TemplateField>
                            <HeaderTemplate>Ver</HeaderTemplate>
                            <ItemTemplate>
                                <ajaxToolkit:HoverMenuExtender runat="server" ID="waskaHover" TargetControlID="lnkVer" PopupControlID="pnlVer" PopupPosition="Top">
                                </ajaxToolkit:HoverMenuExtender>
                                <span title='Administrar a: <%# Eval("cliente") %>' >
                                <asp:Label runat="server" ID="lnkVer" CssClass="btn1">
                                Ver...
                                </asp:Label>
                                <asp:Panel BorderColor="Black" BorderWidth="2px" BackColor="#024579" ID="pnlVer" runat="server">
                                    <%--<a href="admin_cliente_contacto_lista.aspx?id=<%# Eval("idrastreo_persona") %>">Contactos</a>
                                    <br />--%>
                                    <a href="admin_cliente_vehiculo_lista.aspx?id=<%#Eval("idrastreo_persona")%>">Vehiculos</a>
                                    <br />
                                    <a href="admin_cliente_documentos.aspx?cid=<%# Eval("idrastreo_persona") %>">Documentos</a>
                                    <br />
                                    <a href="admingps_opciones_usuario_lista.aspx?pid=<%# Eval("idrastreo_persona") %>">Permisos</a>
                                    <br />
                                    <a href="admin_historial.aspx?cid=<%# Eval("idrastreo_persona") %>">Historial</a></asp:Panel>
                                </span>
                            </ItemTemplate>
                        </asp:TemplateField>   
                        <asp:TemplateField>
                            <ItemTemplate>
                                <ajaxToolkit:HoverMenuExtender runat="server" ID="hoverContactos" TargetControlID="lnkVerC" PopupControlID="pnlVerC" PopupPosition="Right">
                                </ajaxToolkit:HoverMenuExtender>
                                <span>
                                <asp:Label runat="server" ID="lnkVerC" CssClass="btn" >
                                Contactos
                                </asp:Label>
                                <asp:Panel BackColor="Black" ID="pnlVerC" runat="server">
                                <%# GetContactos(Convert.ToString(Eval("idrastreo_persona"))) %>
                                </asp:Panel>
                            </ItemTemplate>
                        </asp:TemplateField>               
                        <asp:TemplateField>
                            <ItemTemplate>
                                 <a class="btn1" onclick="AbrirDialogo('admin_cliente_usuarios.aspx?id=<%# Eval("idrastreo_persona") %>')" title="Agrega usuarios de <%#Eval("cliente") %>">
                                 Administrar usuarios
                                 </a>
                            </ItemTemplate>
                        </asp:TemplateField>                        
                        <asp:TemplateField>
                            <ItemTemplate>
                                <div title='Eliminar a <%# Eval("cliente") %>'>
                                 <asp:LinkButton runat="server" CssClass="rastreo_botones_cancel"
                                        Font-Size="X-Small" Text="Eliminar" CommandName="Eliminar" CommandArgument='<%#Eval("idrastreo_persona") %>' OnClientClick='return seguro_que_cliente();' />
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        </Columns>
                        <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                        <PagerStyle BackColor="Navy" ForeColor="White" HorizontalAlign="Center" 
                                Font-Bold="True" />
                        <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
                        <AlternatingRowStyle BackColor="#BABABA" />
                        </asp:GridView>
                        <asp:SqlDataSource ID="sqlds_Lista_Personas" runat="server" ConnectionString="<%$ ConnectionStrings:CS_RASTREOSystem %>" ProviderName="<%$ ConnectionStrings:CS_RASTREOSystem.ProviderName %>">
                            <%--SelectCommand="SELECT * FROM rsview_datos_personas WHERE idrastreo_persona NOT IN (SELECT id_empleado FROM rastreo_empleados WHERE id_empleado NOT IN (SELECT id_cliente FROM rastreo_cliente))">--%>
                        </asp:SqlDataSource>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
