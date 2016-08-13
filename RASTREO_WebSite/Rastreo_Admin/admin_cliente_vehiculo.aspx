<%@ Page Title="RASTREO Paraguay - Administración- ABM de Vehiculo" Language="VB"
    MasterPageFile="RASTREOMasterPage.master" AutoEventWireup="false" CodeFile="admin_cliente_vehiculo.aspx.vb"
    Inherits="RASTREO_Administracion_admin_cliente_vehiculo" %>

<%--<%@ Register Assembly="ColorPicker" Namespace="ColorPicker" TagPrefix="cp" %>--%>
<asp:Content ID="Content2" ContentPlaceHolderID="main_ContentPlaceHolder" runat="Server">
    <asp:UpdatePanel ID="updPanel_Vehiculos" runat="server">
        <ContentTemplate>
            <asp:Label ID="errMsgs" runat="server" Width="100%" CssClass="Mensaje_de_error" Visible="False"></asp:Label>
            <table style="width: 100%;">
                <tr>
                    <td colspan="2" style="text-align: right">
                        <asp:Button ID="btnGUARDAR" runat="server" OnClientClick="DisableSave(this);" CssClass="rastreo_botones_save"
                            Text="Guardar" ToolTip="Guardar" />
                        <asp:Button ID="btnCANCELAR" runat="server" CausesValidation="false" CssClass="rastreo_botones_cancel"
                            Text="Cancelar" ToolTip="Cancelar" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="panelVEHICULO" runat="server" Width="100%" BackColor="Beige">
                            <asp:Label BackColor="#024579" ForeColor="White" Font-Bold="True" runat="server"
                                Text="Datos de la Instalacion" />
                            <% If Integer.TryParse(Request.QueryString("id"), result:=0) Then%>
                            <input type="button" name="btnManageFotos" value="Agregar Fotografia del Vehiculo"
                                class="rastreo_botones" onclick="AbrirDialogo('admin_cliente_vehiculo_fotos.aspx?vid=<%=Request.QueryString("id")%>&cid=<%=Request.QueryString("cid")%>')"
                                title="Haga click aqui para gestionar las fotos de este vehiculo" />
                            <% End If%>
                            <table width="100%">
                                <tr>
                                    <td>
                                        Sucursal de Instalacion
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlSucursales" CssClass="DropDownList">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        Nro. de Ficha
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFicha" runat="server" CssClass="TextBox" Width="99%"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Este campo es obligatorio."
                                            SetFocusOnError="true" ControlToValidate="txtFicha" CssClass="rastreo_botones" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        MOVIL
                                        <br />
                                        o <strong>Contraseña</strong>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtVEHICULO_identificador_rastreo" runat="server" CssClass="TextBox"
                                            Width="99%"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator_txtVEHICULO_identificador_rastreo"
                                            runat="server" ErrorMessage="Este campo es obligatorio." SetFocusOnError="true"
                                            ControlToValidate="txtVEHICULO_identificador_rastreo" CssClass="rastreo_botones" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 10%">
                                        Nro. de Poliza
                                    </td>
                                    <td style="width: 10%">
                                        <asp:TextBox CssClass="TextBox" runat="server" ID="txtVEHICULO_nropoliza" ToolTip="Ingrese el nro. de poliza aqui"
                                            Width="150px" />
                                    </td>
                                    <td>
                                        Emisión &nbsp;
                                        <asp:TextBox CssClass="TextBox" runat="server" ID="txtVEHICULO_fechaemisionpoliza"
                                            ToolTip="Ingrese el vencimiento de poliza aqui" Width="70px" />
                                        <asp:Image runat="server" ID="CalendarioEmision" ImageUrl="~/App_Themes/CENTRAL/Imagenes/Calendar_scheduleHS.png"
                                            ImageAlign="AbsMiddle" />
                                        <ajaxToolkit:MaskedEditExtender runat="server" AcceptAMPM="false" ID="mask_fechaemision"
                                            ClearMaskOnLostFocus="true" TargetControlID="txtVEHICULO_fechaemisionpoliza"
                                            Mask="99/99/9999" MaskType="Date" UserDateFormat="DayMonthYear" />
                                        <ajaxToolkit:CalendarExtender CssClass="MyCalendar" ID="CalendarExtender_polizaemision"
                                            runat="server" PopupButtonID="CalendarioEmision" Format="dd/MM/yyyy" PopupPosition="TopRight"
                                            Animated="true" TargetControlID="txtVEHICULO_fechaemisionpoliza" />
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtVEHICULO_fechaemisionpoliza"
                                            ErrorMessage="Fecha inválida" ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((1[6-9]|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((1[6-9]|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((1[6-9]|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"
                                            SetFocusOnError="true"></asp:RegularExpressionValidator>
                                    </td>
                                    <td>
                                        Vencimiento &nbsp;
                                        <asp:TextBox CssClass="TextBox" runat="server" ID="txtVEHICULO_fechavencimientopoliza"
                                            ToolTip="Ingrese el vencimiento de poliza aqui" Width="70px" />
                                        <asp:Image runat="server" ID="CalendarioVencimiento" ImageUrl="~/App_Themes/CENTRAL/Imagenes/Calendar_scheduleHS.png"
                                            ImageAlign="AbsMiddle" />
                                        <ajaxToolkit:MaskedEditExtender ID="mask_fechavencimiento" runat="server" AcceptAMPM="false"
                                            ClearMaskOnLostFocus="true" TargetControlID="txtVEHICULO_fechavencimientopoliza"
                                            Mask="99/99/9999" MaskType="Date" ClearTextOnInvalid="True" UserDateFormat="DayMonthYear" />
                                        <ajaxToolkit:CalendarExtender CssClass="MyCalendar" ID="CalendarExtender_polizavencimiento"
                                            runat="server" PopupButtonID="CalendarioVencimiento" Format="dd/MM/yyyy" PopupPosition="TopRight"
                                            Animated="true" TargetControlID="txtVEHICULO_fechavencimientopoliza" />
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtVEHICULO_fechavencimientopoliza"
                                            ErrorMessage="Fecha inválida" ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((1[6-9]|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((1[6-9]|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((1[6-9]|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"
                                            SetFocusOnError="true"></asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 15%">
                                        Proviene de
                                    </td>
                                    <td colspan="3">
                                        <asp:Panel runat="server" ID="pnlProvieneSelect" Width="70%" Height="100%">
                                            <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlVEHICULO_provienede"
                                                ToolTip="Elija aqui seguro viene el vehiculo si aplica." Width="60%" AutoPostBack="True" />
                                            <asp:LinkButton CssClass="rastreo_linkbuttons" runat="server" ID="lnkproviene_edit"
                                                ToolTip="Editar">
                                                        <img src="../App_Themes/CENTRAL/Imagenes/EditTableHS.png" alt="Edit" />
                                            </asp:LinkButton>
                                            <asp:LinkButton runat="server" ID="lnkproviene_new" ToolTip="Nuevito">
                                                        <img src="../App_Themes/CENTRAL/Imagenes/NewDocumentHS.png" alt="New" />
                                            </asp:LinkButton>
                                        </asp:Panel>
                                        <asp:Panel runat="server" ID="pnlProvieneNew" Width="70%" Height="100%" Visible="false">
                                            <asp:TextBox runat="server" ID="txtPROVIENEDE" Width="70%" />
                                            <br />
                                            <asp:ImageButton runat="server" ID="imgbtn_provienesave" AlternateText="Guardar"
                                                ToolTip="Guardar" OnClientClick="DisableSave(this);" ImageUrl="~/App_Themes/CENTRAL/Imagenes/saveHS.png"
                                                Style="width: 16px" />
                                            &nbsp;
                                            <asp:ImageButton runat="server" ID="imgbtn_provieneback" AlternateText="Cancelar"
                                                ToolTip="Cancelar" ImageUrl="~/App_Themes/CENTRAL/Imagenes/edit-undo.png" />
                                            &nbsp;</asp:Panel>
                                    </td>
                                    <!-- se agrega Nro Orden de Instalacion 06122010 -->
                                    <td>
                                        <asp:Panel runat="server" ID="pnlNroOrden" RenderMode="Inline" Visible="False">
                                            <td style="width: 99%">
                                                Nro. de Orden
                                                <asp:TextBox CssClass="TextBox" runat="server" ID="txtNroOrdenInstal" ToolTip="Nro de Orden El Productor S.A."
                                                    Width="40px" Height="19px" />
                                                <!--<asp:RequiredFieldValidator ID="ReqFieldValidNroOrden" runat="server"
                                        ControlToValidate="txtNroOrdenInstal"
                                        ErrorMessage="Obligatorio para El Productor SA." SetFocusOnError="true"
                                        CssClass="rastreo_botones" />-->
                                            </td>
                                        </asp:Panel>
                                    </td>
                                    <!-- se agrega Nro Orden de Instalacion 06122010 -->
                                </tr>
                                <tr>
                                    <td>
                                        Fecha de Instalación
                                    </td>
                                    <td>
                                        <asp:TextBox CssClass="TextBox" runat="server" ID="txtVEHICULO_fechainstalacion"
                                            ToolTip="Ingrese la fecha de instalación" Width="150px" />
                                        <%--<ajaxToolkit:CalendarExtender CssClass="MyCalendar" ID="myCalendarExtender1" runat="server" PopupPosition="TopRight" Animated="true" TargetControlID="txtVEHICULO_fechainstalacion" Format="dd/MM/yyyy HH:mm:ss"/>--%>
                                        <%--<asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtVEHICULO_fechainstalacion"
                                        ErrorMessage="Fecha y/o hora inválidas"
                                        ValidationExpression ="^((((([0-1]?\d)|(2[0-8]))\/((0?\d)|(1[0-2])))|(29\/((0?[1,3-9])|(1[0-2])))|(30\/((0?[1,3-9])|(1[0-2])))|(31\/((0?[13578])|(1[0-2]))))\/((19\d{2})|([2-9]\d{3}))|(29\/0?2\/(((([2468][048])|([3579][26]))00)|(((19)|([2-9]\d))(([2468]0)|([02468][48])|([13579][26]))))))\s(([01]?\d)|(2[0-3]))(:[0-5]?\d){2}.*$"
                                        ></asp:RegularExpressionValidator>--%>
                                        <ajaxToolkit:MaskedEditExtender ID="mask_fechainstalacion" runat="server" TargetControlID="txtVEHICULO_fechainstalacion"
                                            Mask="99/99/9999 99:99" MaskType="DateTime" />
                                    </td>
                                    <td colspan="1" style="text-align: center">
                                        Fecha de Re-Instalación
                                    </td>
                                    <td>
                                        <asp:TextBox CssClass="TextBox" runat="server" ID="txtVEHICULO_refechainstalacion"
                                            ToolTip="Ingrese la fecha de Re-instalación" Width="150px" />
                                        <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server" TargetControlID="txtVEHICULO_Refechainstalacion"
                                            Mask="99/99/9999 99:99" MaskType="DateTime" />
                                    </td>
                                    <td colspan="1" style="text-align: center">
                                        Fecha de DesInstalación
                                    </td>
                                    <td>
                                        <asp:TextBox CssClass="TextBox" runat="server" ID="txtVEHICULO_desfechainstalacion"
                                            ToolTip="Ingrese la fecha de Re-instalación" Width="150px" />
                                        <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender2" runat="server" TargetControlID="txtVEHICULO_desfechainstalacion"
                                            Mask="99/99/9999 99:99" MaskType="DateTime" />
                                    </td>
                                    <td colspan="1" style="text-align: left">
                                        <asp:CheckBox ID="chkVEHICULO_activo" runat="server" Checked="true" Text="Vehiculo activo" />
                                        <br />
                                        <asp:CheckBox ID="chkVEHICULO_demo" runat="server" Checked="false" Text="Vehiculo DEMO" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel runat="server" ID="panelVehiculoData" BackColor="LightSlateGray" ForeColor="White"
                            Font-Bold="true">
                            <asp:Label ID="Label1" BackColor="#024579" ForeColor="White" Font-Bold="True" runat="server"
                                Text="Datos del Vehiculo" />
                            <table width="100%">
                                <tr>
                                    <td colspan="2">
                                        <table width="100%" cellspacing="0" cellpadding="0">
                                            <tr>
                                                <td>
                                                    Tipo de Vehiculo
                                                    <asp:Image runat="server" ID="Icono_de_TipoVehiculo" Width="32px" Height="32px" AlternateText="ImagenTransporte" />
                                                </td>
                                                <td>
                                                    <asp:Panel runat="server" ID="panel_select_tipodevehiculo" Width="100%" Height="100%">
                                                        <asp:DropDownList CssClass="DropDownList" ID="ddlTipoDeVehiculo" runat="server" Width="80%"
                                                            AutoPostBack="true">
                                                        </asp:DropDownList>
                                                        <asp:LinkButton CssClass="rastreo_linkbuttons" runat="server" ID="lnk_edit_tipovehiculo"
                                                            ToolTip="Editar">
                                                        <img src="../App_Themes/CENTRAL/Imagenes/EditTableHS.png" alt="Edit" />
                                                        </asp:LinkButton>
                                                        <asp:LinkButton runat="server" ID="lnk_new_tipovehiculo" ToolTip="Nuevito">
                                                        <img src="../App_Themes/CENTRAL/Imagenes/NewDocumentHS.png" alt="New" />
                                                        </asp:LinkButton>
                                                    </asp:Panel>
                                                    <asp:Panel runat="server" BackColor="Navy" ID="panel_newedit_tipodevehiculo" Width="100%"
                                                        Height="100%" Visible="false">
                                                        Tipo de vehiculo<br />
                                                        <asp:TextBox runat="server" ID="txtTIPOVEHICULO_Add" Width="100%" CssClass="TextBox" />
                                                        <br />
                                                        Icono identificador<br />
                                                        <asp:DropDownList runat="server" ID="ddlIconosTV" AutoPostBack="True">
                                                        </asp:DropDownList>
                                                        <asp:Image runat="server" ID="imgtempTV" />
                                                        <br />
                                                        <asp:ImageButton runat="server" ID="imgbtn_tipovehiculo_save" AlternateText="Guardar"
                                                            ToolTip="Guardar" OnClientClick="DisableSave(this);" ImageUrl="~/App_Themes/CENTRAL/Imagenes/saveHS.png" />
                                                        &nbsp;
                                                        <asp:ImageButton runat="server" ID="imgbtn_tipovehiculo_cancel" AlternateText="Cancelar"
                                                            ToolTip="Cancelar" ImageUrl="~/App_Themes/CENTRAL/Imagenes/edit-undo.png" />
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Marca
                                                </td>
                                                <td>
                                                    <asp:Panel runat="server" ID="panel_Marca_select" Width="100%" Height="100%">
                                                        <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlMarcas" AutoPostBack="true"
                                                            Width="80%" />
                                                        <asp:LinkButton CssClass="rastreo_linkbuttons" runat="server" ID="lnkmarca_edit"
                                                            ToolTip="Editar">
                                                        <img src="../App_Themes/CENTRAL/Imagenes/EditTableHS.png" alt="Edit" />
                                                        </asp:LinkButton>
                                                        <asp:LinkButton runat="server" ID="lnkmarca_nueva" ToolTip="Nueva">
                                                        <img src="../App_Themes/CENTRAL/Imagenes/NewDocumentHS.png" alt="New" />
                                                        </asp:LinkButton>
                                                    </asp:Panel>
                                                    <asp:Panel runat="server" BackColor="Navy" ID="panel_Marcas_newedit" Width="100%"
                                                        Height="100%" Visible="false">
                                                        Marca<br />
                                                        <asp:TextBox runat="server" ID="txtMARCA_Add" Width="100%" CssClass="TextBox" />
                                                        <br />
                                                        <asp:ImageButton runat="server" ID="imgbtn_marcasave" AlternateText="Guardar" ToolTip="Guardar"
                                                            OnClientClick="DisableSave(this);" ImageUrl="~/App_Themes/CENTRAL/Imagenes/saveHS.png" />
                                                        &nbsp;
                                                        <asp:ImageButton runat="server" ID="imgbtn_marcacancel" AlternateText="Cancelar"
                                                            ToolTip="Cancelar" ImageUrl="~/App_Themes/CENTRAL/Imagenes/edit-undo.png" />
                                                        &nbsp;</asp:Panel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Modelo
                                                </td>
                                                <td>
                                                    <asp:Panel runat="server" ID="panel_Modelo_select" Width="100%" Height="100%">
                                                        <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlModelos" AutoPostBack="true"
                                                            Width="80%" />
                                                        <asp:LinkButton CssClass="rastreo_linkbuttons" runat="server" ID="lnkmodelo_edit"
                                                            ToolTip="Editar">
                                                        <img src="../App_Themes/CENTRAL/Imagenes/EditTableHS.png" alt="Edit" />
                                                        </asp:LinkButton>
                                                        <asp:LinkButton runat="server" ID="lnkmodelo_nuevo" ToolTip="Nuevito">
                                                        <img src="../App_Themes/CENTRAL/Imagenes/NewDocumentHS.png" alt="New" />
                                                        </asp:LinkButton>
                                                    </asp:Panel>
                                                    <asp:Panel runat="server" BackColor="Navy" ID="panel_Modelo_newedit" Width="100%"
                                                        Height="100%" Visible="false">
                                                        Modelo<br />
                                                        <asp:TextBox runat="server" ID="txtMODELO_Add" Width="100%" CssClass="TextBox" />
                                                        <br />
                                                        <asp:ImageButton runat="server" ID="imgbtn_modelosave" AlternateText="Guardar" ToolTip="Guardar"
                                                            ImageUrl="~/App_Themes/CENTRAL/Imagenes/saveHS.png" OnClientClick="DisableSave(this);" />
                                                        &nbsp;
                                                        <asp:ImageButton runat="server" ID="imgbtn_modelocancel" AlternateText="Cancelar"
                                                            ToolTip="Cancelar" ImageUrl="~/App_Themes/CENTRAL/Imagenes/edit-undo.png" />
                                                        &nbsp;</asp:Panel>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Año
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtVEHICULO_anho" runat="server" CssClass="TextBox" Width="99%"
                                            onchange="if (isNaN(this.value)) { this.value='0'; this.focus()}"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Chasis
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtVEHICULO_chasis" runat="server" CssClass="TextBox" Width="99%"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator_txtVEHICULO_chasis" runat="server"
                                            ErrorMessage="Este campo es obligatorio." SetFocusOnError="true" ControlToValidate="txtVEHICULO_chasis"
                                            CssClass="rastreo_botones" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Matricula
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtVEHICULO_matricula" runat="server" CssClass="TextBox" AutoPostBack="true"
                                            Width="99%"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Color
                                    </td>
                                    <td>
                                        <%--<cp:ColorPicker ID="cpVEHICULO_color" runat="server"></cp:ColorPicker>--%>
                                        <asp:TextBox runat="server" ID="cpVEHICULO_color" CssClass="TextBox"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Kilometraje
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtVEHICULO_kilometraje" runat="server" CssClass="TextBox" Width="99%"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Motor
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtVEHICULO_motor" runat="server" CssClass="TextBox" Width="99%"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Consumo combustible aprox. cada 100Km.
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtVEHICULO_consumo" runat="server" CssClass="TextBox" Width="50%"
                                            onchange="if (isNaN(this.value)) { this.value='0'; this.focus()}"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Chofer
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtVEHICULO_chofer" runat="server" CssClass="TextBox" Width="90%"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Contacto del chofer
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtVEHICULO_chofer_contacto" runat="server" CssClass="TextBox" Width="50%"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Observaciones del vehiculo
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtVEHICULO_observaciones" runat="server" CssClass="TextBox" Width="100%"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="border: solid 3px silver; background-color: #024579">
                                        Equipo GPS
                                    </td>
                                    <td style="border: solid 3px silver; vertical-align: baseline">
                                        <% If Request.Browser.Frames Then%>
                                        <asp:UpdatePanel runat="server" ID="updpnlFRAME">
                                            <ContentTemplate>
                                                <asp:Panel runat="server" ID="pnlIFRAME" Width="100%">
                                                    <iframe id="ifFRAME1" frameborder="0" src="iadmingps_equipogps.aspx?id=<%=ddlEquipoGPS.SelectedValue %>"
                                                        style="width: 550px; height: 200px" scrolling="no"></iframe>
                                                    <br />
                                                    <asp:Button CssClass="rastreo_botones_cancel" runat="server" ID="btnClose_equipo"
                                                        Text="Cerrar ventana" />
                                                </asp:Panel>
                                                <ajaxToolkit:ModalPopupExtender runat="server" ID="ajax_popuphistorico" BackgroundCssClass="modalBackground"
                                                    TargetControlID="btnFRAMEequipoGPS" DropShadow="True" Enabled="True" OkControlID="btnClose_equipo"
                                                    PopupControlID="pnlIFRAME" />
                                                <asp:Button runat="server" ID="btnFRAMEequipoGPS" CausesValidation="false" Text="Administrar Equipos GPS"
                                                    CssClass="rastreo_botones" TabIndex="1" />
                                                <br />
                                                <asp:DropDownList AutoPostBack="true" ID="ddlEquipoGPS" runat="server">
                                                </asp:DropDownList>
                                                <asp:ImageButton ID="imgRefreshListaEquiposGPS" runat="server" CausesValidation="False"
                                                    ImageUrl="~/App_Themes/CENTRAL/Imagenes/view-refresh.png" ToolTip="Haga click aqui para refrescar la lista de equipos GPS" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                        <%  Else%>
                                        <a href="admingps_equipogps.aspx" class="rastreo_linkbuttons">
                                            <img src="../App_Themes/CENTRAL/Imagenes/EditTableHS.png" alt="Nuevo" />
                                        </a><a href="admingps_equipogps.aspx?id=<%=ddlEquipoGPS.SelectedValue %>" class="rastreo_linkbuttons">
                                            <img src="../App_Themes/CENTRAL/Imagenes/NewDocumentHS.png" alt="Editar" /></a>
                                        <%  End If%>
                                        <asp:Label runat="server" ID="lbEquipoUsado" CssClass="Mensaje_de_error" />
                                        <% If Request.Browser.Frames Then%>
                                        <br />
                                        <asp:Panel runat="server" ID="pmlFRAMEEquipo" Visible="False" Width="100%">
                                            <iframe id="IframeEventos" scrolling="auto" frameborder="0" src='iadmingps_eventos_equipo_lista.aspx?id=<%= ddlEquipoGPS.SelectedValue %>'
                                                style="width: 550px; height: 200px"></iframe>
                                        </asp:Panel>
                                        <asp:Button runat="server" ID="btnEventosEquipo" Text="Eventos del equipo GPS" CssClass="rastreo_botones"
                                            TabIndex="1" />
                                        <%  End If%>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <%--Aqui hay q hacer acople con INSTALADORES, es decir EMPLEADOS!--%>
                                        Vendedores:
                                        <br />
                                        <asp:DropDownList runat="server" ID="ddl_Vendedor" Width="100%" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <%--Aqui hay q hacer acople con INSTALADORES, es decir EMPLEADOS!--%>
                                        Instalador:
                                        <br />
                                        <asp:DropDownList ID="ddl_INSTALADORES" runat="server" Width="100%" />
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