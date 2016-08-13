<%@ Page Language="VB" Title="RASTREO Paraguay - Administración - ABM de Opciones de pantalla" MasterPageFile="RASTREOMasterPage.master" CodeFile="admingps_opciones_de_pantalla.aspx.vb" AutoEventWireup="false" Inherits="admingps_opciones_de_pantalla" %>

<asp:Content ID="contenido_rastreoadmin" ContentPlaceHolderID="main_ContentPlaceHolder" Runat="Server">
<asp:Label ID="errMsgs" runat="server" Width="100%" CssClass="Mensaje_de_error" 
                Visible="False"></asp:Label>
    <asp:Panel runat="server" ID="panelOPCIONES" BackColor="LightSlateGray" 
                            ForeColor="White" Font-Bold="true"  >
        <asp:Label ID="Label1" BackColor="#024579" ForeColor="White" Font-Bold="True" 
                            runat="server"  Text="Datos del Equipo GPS"/>
        <table width="100%">
                <tr>
                    <td colspan="2" style="text-align:right">
                        <asp:Button ID="btnGUARDAR" runat="server" 
                            CssClass="rastreo_botones_save" Text="Guardar" OnClientClick="DisableSave(this);"/>
                        <asp:Button ID="btnCANCELAR" runat="server" CausesValidation="false" 
                            CssClass="rastreo_botones_cancel" Text="Cancelar" />
                    </td>
                </tr>        
            <tr>
                <td style="width:30%">
                                        Opcion de Pantalla
                                    </td>
                <td>
                    <asp:TextBox ID="txtOPCIONESGPS" runat="server" 
                                            CssClass="TextBox" Width="99%"></asp:TextBox>
                </td>
            </tr>

        </table>
    </asp:Panel>

</asp:Content>
	
	