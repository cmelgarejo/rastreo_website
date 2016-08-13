<%@ Page Title="RASTREO Paraguay - Administración - ABM de Eventos de Tipo Equipo GPS" Language="VB" MasterPageFile="RASTREOMasterPage.master" AutoEventWireup="false" CodeFile="admingps_eventos.aspx.vb" Inherits="RASTREO_Administracion_eventosgps" %>
<%@ Register Assembly="ColorPicker" Namespace="ColorPicker" TagPrefix="cp" %>

<asp:Content ID="Content2" ContentPlaceHolderID="main_ContentPlaceHolder" Runat="Server">
    <asp:UpdatePanel ID="updPanel_EquipoGPS" runat="server">
        <ContentTemplate>
            <asp:Label ID="errMsgs" runat="server" Width="100%" CssClass="Mensaje_de_error" 
                Visible="False"></asp:Label>
            <table style="width:100%;">
                <tr>
                    <td colspan="2" style="text-align:right">
                        <asp:Button ID="btnGUARDAR" runat="server" 
                            CssClass="rastreo_botones_save" Text="Guardar" OnClientClick="DisableSave(this);"/>
                        <asp:Button ID="btnCANCELAR" runat="server" CausesValidation="false" 
                            CssClass="rastreo_botones_cancel" Text="Cancelar" OnClientClick="history.back();" />
                    </td>
                </tr>            
                <tr>
                    <td>
                        <asp:Panel runat="server" ID="panelEventoGPSData" BackColor="LightSlateGray" ForeColor="White" Font-Bold="true"  >
                            <asp:Label ID="lbTipoEquipo" BackColor="#024579" ForeColor="White" Font-Bold="True" 
                            runat="server"  Text="Tipo Equipo: "/>
                            <table width="100%">
                                <tr>
                                    <td style="width:30%">
                                        Descripcion del evento
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEVENTO_DESC" runat="server" 
                                            CssClass="TextBox" Width="99%"></asp:TextBox>
                                    </td>
                                </tr>                                
                                <tr>
                                    <td>
                                        Evento <br /><span style="font-size:x-small">(Tal cual lo indica el manual)</span></td>
                                    <td>
                                        <asp:TextBox ID="txtEVENTO_evento" runat="server" 
                                            CssClass="TextBox" Width="99%"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Color <br />
                                    <td>
                                        <cp:ColorPicker ID="cpColor_Evento" runat="server"></cp:ColorPicker>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Aviso sonoro en el software? <br />
                                    <td>
                                        <asp:CheckBox ID="checkSonoro" runat="server" Checked="false" />
                                    </td>
                                </tr>
<%--                                
                                <tr>
                                    <td>
                                        Archivo de sonido para el aviso <br />
                                    <td>
                                        <asp:FileUpload ID="FileUpLoadSound" runat="server"  />
                                    </td>
                                </tr>
--%>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>            
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
