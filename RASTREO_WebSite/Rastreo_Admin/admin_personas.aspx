<%@ Page Title="RASTREO Paraguay - Administración - Ingreso de Clientes|Empleados"
    Language="VB" EnableEventValidation="false" MasterPageFile="RASTREOMasterPage.master"
    AutoEventWireup="false" CodeFile="admin_personas.aspx.vb" Inherits="RASTREO_admin_personas" %>

<asp:Content ID="Content2" ContentPlaceHolderID="main_ContentPlaceHolder" runat="Server">
    <asp:UpdatePanel ID="updPanel_Clientes" runat="server" EnableViewState="true" RenderMode="Block">
        <ContentTemplate>
            <asp:Label ID="errMsgs" runat="server" Width="100%" CssClass="Mensaje_de_error" Visible="False"></asp:Label>
            <table width="100%" id="mainTable" cellpadding="0" cellspacing="0" border="black 3px">
                <thead>
                    <tr>
                        <td colspan="2" style="text-align: right">
                            <asp:Button ID="btnNUEVO" runat="server" OnClientClick="DisableSave(this);" CssClass="rastreo_botones"
                                Text="Nueva Persona" ToolTip="Nueva persona" />
                            <asp:Button ID="btnGUARDAR" runat="server" CausesValidation="true" CssClass="rastreo_botones_save"
                                Text="Guardar" ToolTip="Guardar Cambios" />
                            <asp:Button ID="btnCANCELAR" runat="server" CausesValidation="false" CssClass="rastreo_botones_cancel"
                                Text="Cancelar" ToolTip="Volver a la lista" />
                        </td>
                    </tr>
                </thead>
                <tr>
                    <td colspan="2" style="font-family: Arial Narrow, Arial, Verdana; font-size: small;
                        font-weight: bold; vertical-align: top">
                        Carga de
                        <asp:DropDownList CssClass="DropDownList" ID="ddl_Cliente_o_Empleado" runat="server"
                            AutoPostBack="True">
                            <asp:ListItem Value="1">Cliente</asp:ListItem>
                            <asp:ListItem Value="2">Empleado</asp:ListItem>
                        </asp:DropDownList>
                        <% If Request.QueryString("id") <> String.Empty And personaExiste Then%>
                        <input type="button" name="btnUsuarius" value="Agregar usuarios" visible="false"
                            class="rastreo_botones" onclick="AbrirDialogo('admin_cliente_usuarios.aspx?id=<%=Request.QueryString("id")%>')"
                            title="Agrega usuarios del cliente." />
                        <input type="button" name="btnContactus" value="Agregar contacto" visible="false"
                            class="rastreo_botones" onclick="AbrirDialogo('admin_cliente_contacto_lista.aspx?id=<%=Request.QueryString("id")%>')"
                            title="Agrega contacto de la persona." />
                        <input type="button" name="btnVehiculus" value="Agregar vehiculo" visible="false"
                            class="rastreo_botones" onclick="AbrirDialogo('admin_cliente_vehiculo_lista.aspx?id=<%=Request.QueryString("id")%>')"
                            title="Agrega vehiculos a la persona." />
                        <% End If%>
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: baseline; background-color: #F5F5DC">
                        <asp:Panel ID="panel_TipoPersona" ToolTip="Tipo de Persona" runat="server" Width="100%"
                            BackColor="#F5F5DC">
                            <asp:Label ID="Label1" BackColor="#024579" ForeColor="White" Font-Bold="true" runat="server"
                                Text="Tipo de Persona" />
                            <asp:Panel ID="panelSelect_TipoPersona" runat="server" Width="100%">
                                <asp:DropDownList CssClass="DropDownList" ID="ddlTipoPersona" runat="server" Width="80%"
                                    AutoPostBack="True">
                                    <asp:ListItem Selected="True" Value="F">Fisica</asp:ListItem>
                                    <asp:ListItem Value="J">Juridica</asp:ListItem>
                                </asp:DropDownList>
                                <%--                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" 
                                ErrorMessage="Este campo es obligatorio." SetFocusOnError="true" 
                                ControlToValidate="ddlTipoPersona" CssClass="rastreo_botones"/>--%></asp:Panel>
                        </asp:Panel>
                    </td>
                    <td>
                        <asp:Panel ID="panel_TipoDocumento" ToolTip="Tipo de Documento" runat="server" Width="100%"
                            BackColor="#F5F5DC">
                            <asp:Label ID="Label2" BackColor="#024579" ForeColor="White" Font-Bold="true" runat="server"
                                Text="Tipo de Documento" />
                            <asp:Panel ID="panelSelect_TipoDocumento" runat="server" Width="100%">
                                <asp:DropDownList CssClass="DropDownList" ID="ddlTipoDocumento" runat="server" Width="80%"
                                    AutoPostBack="True" />
                                <asp:LinkButton CssClass="rastreo_linkbuttons" ID="link_new_tipodocumento" runat="server"
                                    ToolTip="Nuevo" CausesValidation="False"><img src="../App_Themes/CENTRAL/Imagenes/NewDocumentHS.png" alt="New"/></asp:LinkButton>
                                <asp:LinkButton CssClass="rastreo_linkbuttons" ID="link_edit_tipodocumento" runat="server"
                                    ToolTip="Editar" CausesValidation="False"><img src="../App_Themes/CENTRAL/Imagenes/EditTableHS.png" alt="edit"/></asp:LinkButton>
                                <%--                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" 
                                ErrorMessage="Este campo es obligatorio." SetFocusOnError="true" 
                                ControlToValidate="ddlTipoDocumento" CssClass="rastreo_botones"/>
--%></asp:Panel>
                            <asp:Panel ID="panelInsert_TipoDocumento" runat="server" Visible="false" Width="100%">
                                <asp:TextBox CssClass="TextBox" ID="txtAdd_tipodocumento" runat="server" Width="100%" /><br />
                                <asp:ImageButton ID="link_save_tipodocumento" runat="server" OnClientClick="DisableSave(this);"
                                    AlternateText="Guardar" ToolTip="Guardar" ImageUrl="~/App_Themes/CENTRAL/Imagenes/saveHS.png"
                                    CausesValidation="False" />&nbsp;
                                <asp:ImageButton ID="link_cancel_tipodocumento" runat="server" AlternateText="Cancelar"
                                    ToolTip="Cancelar" ImageUrl="~/App_Themes/CENTRAL/Imagenes/edit-undo.png" CausesValidation="False" />
                            </asp:Panel>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Panel ID="panelCLIENTE" runat="server" Width="100%" BackColor="LightSlateGray"
                            ForeColor="White" Font-Bold="true">
                            <asp:Label ID="Label3" BackColor="#024579" ForeColor="White" Font-Bold="true" runat="server"
                                Text="Cliente" />
                            <table width="100%">
                                <tr>
                                    <td style="width: 20%">
                                        Clave Personal
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtCLIENTE_ClavePersonal" runat="server" CssClass="TextBox" Width="50%" />
                                        <%--<%If panelCLIENTE.Visible Then%>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" 
                                        ControlToValidate="txtCLIENTE_ClavePersonal" 
                                        ErrorMessage="Este campo es obligatorio." SetFocusOnError="true" />
                                        <%End If%>--%>
                                        <asp:CheckBox ID="chkCLIENTE_activo" runat="server" Checked="true" Text="Cliente activo" />
                                        <asp:CheckBox ID="chkCLIENTE_acceso" runat="server" Checked="true" Text="Acceso al sistema web" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="panelEMPLEADO" runat="server" Width="100%" BackColor="LightSlateGray"
                            ForeColor="White" Font-Bold="true">
                            <asp:Label ID="Label4" BackColor="#024579" ForeColor="White" Font-Bold="true" runat="server"
                                Text="Empleado" />
                            <table width="100%">
                                <tr>
                                    <td style="width: 33%">
                                        <asp:CheckBox ID="chkEMPLEADO_activo" Checked="true" runat="server" Text="Activo" />
                                    </td>
                                    <td style="width: 33%">
                                        <asp:CheckBox ID="chkEMPLEADO_acceso" runat="server" Checked="true" Text="Acceso al sistema web" />
                                    </td>
                                    <td style="width: 33%">
                                        <asp:CheckBox ID="chkEMPLEADO_instalador" runat="server" Checked="true" Text="Instalador de GPS" />
                                    </td>
                                    <td style="width: 33%">
                                        <asp:CheckBox ID="chkEMPLEADO_Vendedor" runat="server" Checked="true" Text="Vendedor" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <asp:CheckBox ID="chkSeguro" CssClass="PopUp" AutoPostBack="true" Checked="false"
                            runat="server" Text="Representa una empresa de Seguros???" Width="100%" />
                        <asp:Panel ID="panelSEGURO" runat="server" Visible="false" Width="100%" ForeColor="White"
                            Font-Bold="true" CssClass="PopUp">
                            <asp:Label ID="lbseguroz" BackColor="#024579" ForeColor="White" Font-Bold="true"
                                runat="server" Text="Representa una empresa de Seguros" />
                            <table width="100%">
                                <tr>
                                    <td colspan="3">
                                        <asp:DropDownList ID="ddlRepresentaEsteSeguro" runat="server" CssClass="DropDownList">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Panel runat="server" ID="panelPERSONA_DATOSCONCRETOS" Width="100%" BackColor="LightSlateGray"
                            ForeColor="White" Font-Bold="true">
                            <asp:Label BackColor="#024579" ForeColor="White" Font-Bold="true" runat="server"
                                Text="Datos de la persona" />
                            <table width="100%">
                                <tr>
                                    <td style="width: 20%">
                                        *Nro. de Documento\RUC
                                    </td>
                                    <td>
                                        <asp:UpdatePanel runat="server" ID="updpnlNRODOC">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txtPERSONA_NroDocumento" runat="server" CssClass="TextBox" Width="35%"
                                                    AutoPostBack="true" />
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtPERSONA_NroDocumento"
                                                    ErrorMessage="Este campo es obligatorio." SetFocusOnError="true" CssClass="rastreo_botones" />
                                                <br />
                                                <asp:Label runat="server" ID="lbREPE" CssClass="Mensaje_de_error" Font-Size="X-Small"></asp:Label>
                                                <asp:UpdateProgress runat="server" AssociatedUpdatePanelID="updpnlNRODOC">
                                                    <ProgressTemplate>
                                                        <asp:Label runat="server" ID="lbCheq" CssClass="rastreo_botones_save" Font-Size="X-Small">
                                                        Chequeando..
                                                        </asp:Label>
                                                    </ProgressTemplate>
                                                </asp:UpdateProgress>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="width: 100%">
                                        <asp:Panel ID="panelPERSONAFISICA" runat="server" Visible="true" Width="100%">
                                            <table width="100%">
                                                <tr>
                                                    <td style="width: 20%">
                                                        *Nombre
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtPERSONA_Nombre" runat="server" CssClass="TextBox" Width="80%" />
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtPERSONA_Nombre"
                                                            CssClass="rastreo_botones" ErrorMessage="Este campo es obligatorio." SetFocusOnError="true" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 20%">
                                                        *Apellido
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtPERSONA_Apellido" runat="server" CssClass="TextBox" Width="80%" />
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txtPERSONA_Apellido"
                                                            CssClass="rastreo_botones" ErrorMessage="Este campo es obligatorio." SetFocusOnError="true" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                        <asp:Panel runat="server" ID="panelPERSONAJURIDICA" Visible="false" Width="100%">
                                            <table width="100%">
                                                <tr>
                                                    <td style="width: 20%">
                                                        *Razon Social
                                                    </td>
                                                    <td>
                                                        <asp:TextBox CssClass="TextBox" runat="server" Width="80%" ID="txtPERSONA_RazonSocial" />
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txtPERSONA_RazonSocial"
                                                            ErrorMessage="Este campo es obligatorio." SetFocusOnError="true" CssClass="rastreo_botones" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 20%">
                                        *Direccion
                                    </td>
                                    <td>
                                        <asp:TextBox CssClass="TextBox" runat="server" Width="97%" ID="txtPERSONA_Direccion" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtPERSONA_Direccion"
                                            ErrorMessage="Este campo es obligatorio." SetFocusOnError="true" CssClass="rastreo_botones" />
                                    </td>
                                </tr>
                            </table>
                            <asp:Panel runat="server">
                                <table width="100%">
                                    <tr>
                                        <td style="width: 20%">
                                            *Telefono móvil
                                        </td>
                                        <td>
                                            <asp:TextBox TextMode="MultiLine" CssClass="TextBox" runat="server" ID="txtPERSONA_tel_movil"
                                                ToolTip="Ingrese los teléfonos separándolos por punto y coma (;)" Width="100%" />
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtPERSONA_tel_movil"
                                                ErrorMessage="Este campo es obligatorio." SetFocusOnError="true" CssClass="rastreo_botones" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%">
                                            *Telefono Particular
                                        </td>
                                        <td>
                                            <asp:TextBox TextMode="MultiLine" CssClass="TextBox" runat="server" ID="txtPERSONA_tel_part"
                                                ToolTip="Ingrese los teléfonos separándolos por punto y coma (;)" Width="100%" />
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="txtPERSONA_tel_part"
                                                ErrorMessage="Este campo es obligatorio." SetFocusOnError="true" CssClass="rastreo_botones" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%">
                                            *Telefono Oficina
                                        </td>
                                        <td>
                                            <asp:TextBox TextMode="MultiLine" CssClass="TextBox" runat="server" ID="txtPERSONA_tel_ofi"
                                                ToolTip="Ingrese los teléfonos separándolos por punto y coma (;)" Width="100%" />
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="txtPERSONA_tel_ofi"
                                                ErrorMessage="Este campo es obligatorio." SetFocusOnError="true" CssClass="rastreo_botones" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%">
                                            *e-mails
                                        </td>
                                        <td>
                                            <asp:TextBox TextMode="MultiLine" runat="server" CssClass="TextBoxMAIL" ID="txtPERSONA_email"
                                                Width="100%" ToolTip="Ingrese los e-mails separándolos por punto y coma (;)" />
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="txtPERSONA_email"
                                                ErrorMessage="Este campo es obligatorio." SetFocusOnError="true" CssClass="rastreo_botones" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Panel ID="panel_Ubicacion" runat="server" Width="100%" BackColor="#F5F5DC">
                            <asp:Label BackColor="#024579" ForeColor="White" Font-Bold="true" runat="server"
                                Text="Ubicacion" />
                            <table width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="border: black 1px">
                                        Pais:
                                        <asp:Label runat="server" ID="lblPais" BackColor="#FFFF99" />
                                        <asp:Panel ID="panelSelect_pais" runat="server">
                                            <asp:DropDownList CssClass="DropDownList" ID="ddlPais" runat="server" AutoPostBack="True"
                                                Width="99%">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Este campo es obligatorio."
                                                SetFocusOnError="true" ControlToValidate="ddlPais" CssClass="rastreo_botones" />
                                            <br />
                                            <asp:LinkButton ID="link_new_pais" runat="server" ToolTip="Nuevo pais" CausesValidation="False">
                                                    <img src="../App_Themes/CENTRAL/Imagenes/NewDocumentHS.png" alt="New"/>
                                            </asp:LinkButton>
                                            <asp:LinkButton CssClass="rastreo_linkbuttons" ID="link_edit_pais" runat="server"
                                                ToolTip="Editar Pais" CausesValidation="False">
                                                    <img src="../App_Themes/CENTRAL/Imagenes/EditTableHS.png" alt="Edit"/></asp:LinkButton>
                                        </asp:Panel>
                                        <asp:Panel ID="panelInsert_pais" runat="server" Visible="false">
                                            <asp:TextBox CssClass="TextBox" ID="txtAdd_pais" runat="server" Width="100%" ToolTip="Aqui ingresa el distrito que quieres insertar al pais seleccionado."></asp:TextBox>
                                            <br />
                                            <asp:ImageButton ID="link_save_pais" ImageUrl="~/App_Themes/CENTRAL/Imagenes/saveHS.png"
                                                OnClientClick="DisableSave(this);" runat="server" AlternateText="Guardar" ToolTip="Guardar"
                                                CausesValidation="False" />
                                            &nbsp;<asp:ImageButton ID="link_cancel_pais" ImageUrl="~/App_Themes/CENTRAL/Imagenes/edit-undo.png"
                                                runat="server" AlternateText="Cancelar" ToolTip="Cancelar" CausesValidation="False" />
                                        </asp:Panel>
                                    </td>
                                    <td style="border: black 1px">
                                        Distrito(Departamento):
                                        <asp:Label runat="server" ID="lblDistrito" BackColor="#FFFF99" />
                                        <asp:Panel ID="panelSelect_distrito" runat="server">
                                            <asp:DropDownList CssClass="DropDownList" ID="ddlDistrito" runat="server" AutoPostBack="True"
                                                Width="99%">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Este campo es obligatorio."
                                                SetFocusOnError="true" ControlToValidate="ddlDistrito" CssClass="rastreo_botones" />
                                            <br />
                                            <asp:LinkButton CssClass="rastreo_linkbuttons" ID="link_new_distrito" runat="server"
                                                ToolTip="Nuevo Distrito" CausesValidation="False"><img src="../App_Themes/CENTRAL/Imagenes/NewDocumentHS.png" alt="New"/></asp:LinkButton>
                                            <asp:LinkButton CssClass="rastreo_linkbuttons" ID="link_edit_distrito" runat="server"
                                                ToolTip="Editar Distrito" CausesValidation="False"><img src="../App_Themes/CENTRAL/Imagenes/EditTableHS.png" alt="Edit"/></asp:LinkButton>
                                        </asp:Panel>
                                        <asp:Panel ID="panelInsert_Distrito" runat="server" Visible="false">
                                            <asp:TextBox CssClass="TextBox" ID="txtAdd_distrito" runat="server" Width="100%"
                                                ToolTip="Aqui ingresa el distrito que quieres insertar al pais seleccionado."></asp:TextBox>
                                            <asp:ImageButton ImageUrl="~/App_Themes/CENTRAL/Imagenes/saveHS.png" ID="link_save_distrito"
                                                OnClientClick="DisableSave(this);" runat="server" AlternateText="Guardar" ToolTip="Guardar"
                                                CausesValidation="False" />
                                            &nbsp;
                                            <asp:ImageButton ImageUrl="~/App_Themes/CENTRAL/Imagenes/edit-undo.png" ID="link_cancel_distrito"
                                                runat="server" AlternateText="Cancelar" ToolTip="Cancelar" CausesValidation="False" />
                                        </asp:Panel>
                                    </td>
                                    <td style="border: black 1px">
                                        Ciudad:
                                        <asp:Label runat="server" ID="lblCiudad" BackColor="#FFFF99" />
                                        <asp:Panel ID="panelSelect_ciudad" runat="server">
                                            <asp:DropDownList CssClass="DropDownList" ID="ddlCiudad" runat="server" AutoPostBack="True"
                                                Width="99%">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Este campo es obligatorio."
                                                SetFocusOnError="true" ControlToValidate="ddlCiudad" CssClass="rastreo_botones" />
                                            <br />
                                            <asp:LinkButton CssClass="rastreo_linkbuttons" ID="link_new_ciudad" runat="server"
                                                ToolTip="Nueva Ciudad" CausesValidation="False"><img src="../App_Themes/CENTRAL/Imagenes/NewDocumentHS.png" alt="New"/></asp:LinkButton>
                                            <asp:LinkButton CssClass="rastreo_linkbuttons" ID="link_edit_ciudad" runat="server"
                                                ToolTip="Editar Ciudad" CausesValidation="False"><img src="../App_Themes/CENTRAL/Imagenes/EditTableHS.png" alt="Edit"/></asp:LinkButton>
                                        </asp:Panel>
                                        <asp:Panel ID="panelInsert_ciudad" runat="server" Visible="false">
                                            <asp:TextBox CssClass="TextBox" ID="txtAdd_ciudad" runat="server" Width="100%" ToolTip="Aqui ingresa la ciudad que quieres crear con el Pais y Distrito seleccionado."></asp:TextBox>
                                            <asp:ImageButton ID="link_save_ciudad" runat="server" AlternateText="Guardar" ToolTip="Guardar"
                                                OnClientClick="DisableSave(this);" ImageUrl="~/App_Themes/CENTRAL/Imagenes/saveHS.png"
                                                CausesValidation="False" />
                                            &nbsp;
                                            <asp:ImageButton ID="link_cancel_ciudad" runat="server" AlternateText="Cancelar"
                                                ToolTip="Cancelar" ImageUrl="~/App_Themes/CENTRAL/Imagenes/edit-undo.png" CausesValidation="False" />
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
