<%@ Page Title="RASTREO Paraguay - Envio de Comandos a los equipos GPS" Language="VB" AutoEventWireup="false" CodeFile="Envio_de_Comandos.aspx.vb" Inherits="Envio_De_Comandos" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<!-- Autor: Ing. Adolfo Bresanovich || Director de R&D RASTREO Paraguay -->

<head id="GPSadminHead" runat="server">

    <link rel="shortcut icon" href="~/RASTREO.ico" type="image/x-icon"/>
    <link rel="icon" href="~/RASTREO.ico" type="image/x-icon"/>
    <title>RASTREO WebSystem - Bienvenido!</title>
    <script type="text/javascript" language="javascript">
        function seguro_que_corte() {
            if (document.getElementById("MensajeEnvio") != null) {
                var mensaje = document.getElementById("MensajeEnvio");
            }
            //alert(mensaje.value);
            if (mensaje.value.indexOf("CORTE")>=0) {
                var seguroque = confirm("ATENCION: Seguro que va enviar un comando de corte?");
                if (seguroque == true) {
                    return true;
                }
                else {
                    return false;
                }
            }
            return true;
        }
    </script>
</head>
<body>
    <form id="RASTREOmainForm" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="RASTREOToolScriptManager" ScriptMode="Release" runat="server">
            <Scripts>
                <asp:ScriptReference Path="~/WebKit.js" />
            </Scripts>    
        </ajaxToolkit:ToolkitScriptManager>
                        <asp:UpdateProgress runat="server" ID="updpg_pnlGPS" AssociatedUpdatePanelID="updpnl_CMDS">
                    <ProgressTemplate>
                        <div style="position:absolute;left:0px;top:0px;width:100%;height:70%" class="modalBackground">
                            <img src="./App_Themes/CENTRAL/Imagenes/ajax-loader.gif" alt="-"/>
                            <label class="rastreo_nowloading">Actualizando...</label>
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>

                <asp:UpdatePanel runat="server" ID="updpnl_CMDS">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnActualizaLista" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnAddAll" EventName="Click" />
            </Triggers>
            <ContentTemplate>
                    <asp:HiddenField runat="server" ID="MensajeEnvio" />
                    <asp:HiddenField runat="server" ID="TipoDeReporte" />
                    <asp:Label ID="errMsgs" runat="server" Width="100%" CssClass="Mensaje_de_error" Visible="false" />
                    <asp:Panel runat="server" ID="updpnl_EnvioComandos" Width="100%" ForeColor="White"  BackColor="Silver">
                        <table cellpadding="0" cellspacing="0" width="100%" style="border: medium solid #808000">
                            <tr style="border-style: solid">
                                <td style="border:solid 2px black;" valign="top">
                                    Tipos de Equipo GPS
                                    <asp:DropDownList runat="server" ID="ddlTipoEquipo" CssClass="DropDownList" AutoPostBack="true"/>
                                    <asp:LinkButton runat="server" ID="btnAddEditCMDTpoEq" Text="Agregar | Editar comandos tipo de equipo" Width="250px" />
                                </td>
                                <td style="border: solid 2px black;" valign="top">
                                    EquipoGPS
                                    <asp:DropDownList runat="server" ID="ddlEquipoGPS" CssClass="DropDownList" AutoPostBack="true" />
                                    <asp:Button runat="server" ID="btnAddAll" Text="Agregar todos los equipos a la lista de envio" CssClass="btn" />
                                    <asp:Button runat="server" ID="btnAddGPSList" Text="Agregar a lista de envio" CssClass="btn1"/>
                                </td>
                            </tr>
                            <tr>
                                <td style="border: solid 2px black" colspan="2" valign="top">
                                    Comandos    
                                    <asp:DropDownList runat="server" ID="ddlComandos_de_TipoEquipo" CssClass="DropDownList" Width="60%" AutoPostBack="true" />
                                    <asp:LinkButton runat="server" ID="btnDELComandos_de_TipoEquipo" Text="- Eliminar comando" CssClass="btn" ForeColor="White" OnClientClick="return seguro_que();" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Panel runat="server" ID="pnlInsCMD" Visible="false" BackColor="Navy" BorderColor="YellowGreen" BorderWidth="3px" >
                                        <asp:Button runat="server" ID="btnNewCMD" CssClass="btnplus2" Text="   Nuevo comando" ForeColor="White"/>
                                        <table cellpadding="0" cellspacing="0" width="100%">
                                            <tr>
                                                <td valign="top">
                                                    Descripción:
                                                </td>
                                                <td valign="top">
                                                    <asp:TextBox runat="server" ID="txtTDEq_DESC" CssClass="TextBox" Width="98%" />
                                                </td>
                                                <td valign="top">
                                                    Comando:
                                                </td>
                                                <td valign="top">
                                                    <asp:TextBox runat="server" ID="txtTDEq_CMD" CssClass="TextBox" Width="98%"/>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="border: solid 2px black"  valign="top" align="center">
                                                    <asp:ImageButton runat="server" ID="btnSaveCMDTpoEq" 
                                                    AlternateText="Guardar" ToolTip="Guardar"
                                                    ImageUrl="~/App_Themes/CENTRAL/Imagenes/saveHS.png" style="height: 16px"/>
                                                    <asp:ImageButton runat="server" ID="btnCancelCMDTpoEq" 
                                                    AlternateText="Cancelar" ToolTip="Cancelar" 
                                                    ImageUrl="~/App_Themes/CENTRAL/Imagenes/edit-undo.png"/>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>                                            
                                </td>
                            </tr>
                            <tr>
                                <td style="border: solid 2px black" colspan="2" valign="top" align="center">
                                    <asp:LinkButton runat="server" ID="btnCustomComando" CssClass="DropDownList" BorderColor="Crimson" Text="Comando customizado" Width="100%" />
                                    <asp:Panel runat="server" ID="pnlCustomComando" Visible="false" BackColor="Goldenrod" BorderColor="Crimson" BorderWidth="3px" >
                                        <table cellpadding="0" cellspacing="0" width="100%">
                                            <tr>
                                                <td style="border: solid 2px black"  valign="top">
                                                    Comando:
                                                    <asp:TextBox runat="server" ID="txtCustomComando" CssClass="TextBox" Width="100%" />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>                                                
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    Equipos Seleccionados
                                </td>
                            </tr>
                            <tr>
                                <td style="border: solid 2px black;width:55%" align="right" valign="top">
                                    <asp:ListBox runat="server" ID="listaEQUIPOSGPS" Width="100%" CssClass="DropDownList" SelectionMode="Multiple"/>
                                </td>
                                <td>
                                    <asp:Button runat="server" ID="btnELIMINARLIST" Text="Eliminar selección" CssClass="btn" />
                                    <asp:Button runat="server" ID="btnCLEARLIST" Text="Limpiar lista" CssClass="btn" />
                                </td>
                            </tr>
                            <tr>
                                <td style="border: solid 2px black"  colspan="2">
                                    <asp:Button runat="server" ID="btnIngresarCMD_encola" Text="   Ingresar comando a la cola" CssClass="btnplus2" OnClientClick="return seguro_que_corte();" />
                                </td>                                            
                            </tr>
                            <tr>
                                <td style="border: solid 2px black" valign="top" colspan="2">
                                    <asp:Button runat="server" ID="btnActualizaLista" Text="Actualizar cola de envio" CssClass="btn" />
                                    <asp:GridView PageSize="1000"  ID="gv_ComandosEnCola" runat="server" 
                                        DataSourceID="sqldsComandosEnCola" AllowPaging="True" 
                                        AllowSorting="True" AutoGenerateColumns="False" BackColor="White" 
                                        BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" 
                                        GridLines="Vertical" Width="100%" Font-Names="Nina, Segoe, Arial Narrow, Arial, Verdana"
                                        EmptyDataRowStyle-CssClass="Mensaje_de_error"
                                        Font-Size="XX-Small" EmptyDataText="- No hay nada en la cola de envio de comandos. -">
                                        <EmptyDataRowStyle BackColor="#EEEEEE" ForeColor="Black" HorizontalAlign="Center"/>
                                        <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                                        <Columns>
                                            <asp:BoundField DataField="identificador_rastreo" HeaderText="Vehiculo" />
                                            <asp:BoundField DataField="id_equipo_gps" HeaderText="ID EquipoGPS" />
                                            <asp:BoundField DataField="comando" HeaderText="Comando" />
                                            <asp:BoundField DataField="descripcion" HeaderText="Descripcion del comando" />
                                            <asp:BoundField DataField="fech_ins" HeaderText="Fecha y hora de ingreso a la cola" />                                            
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:LinkButton runat="server" ID="btnEliminarCOLACMD" OnClientClick="return seguro_que();"
                                                        CommandArgument='<%#Eval("idrastreogps_cola_de_comandos") %>' CommandName="Eliminar">
                                                        Eliminar
                                                    </asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                                        <PagerStyle BackColor="Navy" ForeColor="White" HorizontalAlign="Center" />
                                        <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                                        <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
                                        <AlternatingRowStyle BackColor="#BABABA" />
                                        </asp:GridView>
                                    <asp:SqlDataSource ID="sqldsComandosEnCola" runat="server" ConnectionString="<%$ ConnectionStrings:CS_RASTREOSystem %>" ProviderName="<%$ ConnectionStrings:CS_RASTREOSystem.ProviderName %>"
                                    SelectCommand="SELECT * FROM rsview_equipogps_comandos WHERE Enviado = FALSE">
                                    </asp:SqlDataSource>                    
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
    <script type="text/javascript" src="FuncionesGenerales.js"></script>    
</body>
</html>


