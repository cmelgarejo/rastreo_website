<%@ Page Title="RASTREO Paraguay - Administración - Lista de Usuarios de la Persona" Language="VB" MasterPageFile="RASTREOMasterPage.master" AutoEventWireup="false" CodeFile="admin_cliente_usuarios.aspx.vb" Inherits="RASTREO_Administracion_admin_cliente_usuarios" %>
<asp:Content ID="Content2" ContentPlaceHolderID="main_ContentPlaceHolder" Runat="Server">

    <asp:UpdatePanel ID="updPanel_Usuarios" runat="server" EnableViewState="true" >
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
                            <input class="rastreo_botones" onclick="window.location.href='admin_personas_lista.aspx';window.close();return true;" 
                            type="button" value="Volver" />
                    </td>
                </tr>                
                <tr>
                    <td>
                    <asp:Button runat="server" ID="btnEdit_UsuarioSistema" Text="Agregar Usuario" CssClass="rastreo_botones" ToolTip="Crea el usuario del sistema para esta persona"/>
                    <asp:Panel runat="server" ID="panel_UsuarioSistema" Width="300px" CssClass="PopUp">
                        Informacion de acceso al sistema web
                        <table>
                            <tr>
                                <td>
                                    Nombre completo
                                </td>
                                <td>
                                    <asp:TextBox runat="server" CssClass="TextBox" ID="txtNOMBRE_usuario" ToolTip="Escriba el nombre que aparecera como nombre de usuario en el sistema"/>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Usuario
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtUSUARIO_usuario"/>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Password
                                </td>
                                <td>
                                    <asp:TextBox TextMode="Password" runat="server" ID="txtUSUARIO_password"/>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Button runat="server" ID="btnSave_Usuario" Text="Guardar" OnClientClick="DisableSave(this);"/>
                                    <asp:Button ID="btnCerrar_Usuario" runat="server" Text="Cerrar"/>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <ajaxToolkit:ModalPopupExtender ID="panel_UsuarioSistema_ModalPopupExtender" 
                        BackgroundCssClass="modalBackground"
                        runat="server" DropShadow="True" Enabled="True" 
                        OkControlID="btnCerrar_Usuario"
                        TargetControlID="btnEdit_UsuarioSistema" 
                        PopupControlID="panel_UsuarioSistema">
                    </ajaxToolkit:ModalPopupExtender>
                        <asp:GridView PageSize="30"  ID="gv_Usuarios" runat="server" 
                        DataSourceID="sqlds_usuarios" AllowPaging="True" 
                        AllowSorting="True" AutoGenerateColumns="False" BackColor="White" 
                        BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" 
                            GridLines="Vertical" Width="100%" 
                            PagerSettings-PageButtonCount="20" PagerSettings-Position="TopAndBottom">
                        <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                        <Columns>           
                        <asp:BoundField HeaderText="Nombre completo" DataField="nombre_completo" />
                        <asp:BoundField HeaderText="Usuario" DataField="usuario" />
                        <asp:CheckBoxField ReadOnly="true" DataField="su" HeaderText="Superusuario?" />
                        <asp:BoundField HeaderText="Ultimo ingreso al sistema" DataField="fech_login" />
                        <%--<asp:BoundField HeaderText="Intentos de ingreso" DataField="intentos_login" />--%>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" Font-Size="X-Small" ID="btnEditarUser" Text="Editar usuario" CssClass="rastreo_botones" />
                            </ItemTemplate>
                        </asp:TemplateField> 
                        <asp:TemplateField>
                            <ItemTemplate>
                                <a name="btnSabesLuegus" title="Manejar permisos del usuario <%#Eval("nombre_completo") %>" 
                                    visible="false" class="rastreo_botones"
                                    onclick="AbrirDialogo('admingps_opciones_usuario_lista.aspx?uid=<%#Eval("idrastreo_usuarios")%>&pid=<%=Request.QueryString("id")%>')">
                                Permisos del usuario
                                </a>
                                <asp:Panel runat="server" ID="PnlPOPI" Width="150px" BorderColor="Navy" BackColor="Gainsboro" BorderStyle="Solid" BorderWidth="3px" >
                                    Nombre<br />
                                    <asp:TextBox runat="server" CssClass="TextBox" ID="txtUPDNOM"
                                    Text='<%#Eval("nombre_completo") %>'
                                    />
                                    Cambiar password<br />
                                    <asp:TextBox runat="server" CssClass="TextBoxMAIL" ID="txtUPDPASS" ></asp:TextBox>
                                    <br />
                                    <asp:CheckBox runat="server" ID="chkSU" Text="Superusuario?" Checked='<%#Iif(Eval("su") Is DBNull.Value, False, Eval("su")) %>' />
                                    <asp:Button runat="server" ID="btnUpdUSER" CommandName="Editar" ToolTip='<%#Eval("idrastreo_usuarios") %>' CommandArgument='<%# Container.DataItemIndex %>' Text="Actualizar" CssClass="rastreo_botones" />
                                </asp:Panel>
                                 <ajaxToolkit:HoverMenuExtender runat="server" ID="masterHuver" TargetControlID="btnEditarUser"
                                  PopupControlID="PnlPOPI" PopupPosition="Left" OffsetY="-30">
                                </ajaxToolkit:HoverMenuExtender>
                            </ItemTemplate>
                        </asp:TemplateField> 
                        <asp:TemplateField>
                            <ItemTemplate>                                
                                <a class="rastreo_botones" title="Aqui haga click para ver la lista de los vehiculos los cuales este usuario puede visualizar posicion"
                                 href="admin_cliente_usuario_vehiculo_lista.aspx?id=<%# Eval("id_persona") %>&uid=<%# Eval("idrastreo_usuarios") %>">Ver vehiculos habilitados</a>
                            </ItemTemplate>
                        </asp:TemplateField>    
                        <asp:TemplateField>
                            <ItemTemplate>
                                 <asp:LinkButton ID="linkdele" runat="server" CssClass="rastreo_botones_cancel" 
                                        Font-Size="X-Small" Text="Eliminar" CommandName="Eliminar" CommandArgument='<%#Eval("idrastreo_usuarios") %>' OnClientClick="return seguro_que();" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        </Columns>
                        <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                        <PagerStyle BackColor="Navy" ForeColor="White" HorizontalAlign="Center" />
                        <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="Navy" ForeColor="White" HorizontalAlign="Center" 
                                Font-Bold="True" />
                        <AlternatingRowStyle BackColor="#BABABA" />
                        </asp:GridView>
                        <asp:SqlDataSource ID="sqlds_usuarios" runat="server" ConnectionString="<%$ ConnectionStrings:CS_RASTREOSystem %>" ProviderName="<%$ ConnectionStrings:CS_RASTREOSystem.ProviderName %>"
                        SelectCommand="select * from rastreo_usuarios where id_persona = @id_cliente"
                         >
                        <SelectParameters>
                            <asp:QueryStringParameter DbType="Int32" Name="id_cliente" QueryStringField="id" />
                        </SelectParameters>
                        </asp:SqlDataSource>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

