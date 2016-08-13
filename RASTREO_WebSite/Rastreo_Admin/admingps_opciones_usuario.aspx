<%@ Page Language="VB" Title="RASTREO Paraguay - Administración - ABM de Opciones de pantalla del usuario" MasterPageFile="RASTREOMasterPage.master" CodeFile="admingps_opciones_usuario.aspx.vb" Inherits="admingps_opciones_usuario" %>

<asp:Content ID="contenido_rastreoadmin" ContentPlaceHolderID="main_ContentPlaceHolder" Runat="Server">
    <asp:Label ID="errMsgs" runat="server" Width="100%" CssClass="Mensaje_de_error" 
                Visible="False"></asp:Label>
    <asp:Panel runat="server" ID="panelOPCIONES" BackColor="LightSlateGray" 
                            ForeColor="White" Font-Bold="true"  >
        <asp:Label ID="Label1" BackColor="#024579" ForeColor="White" Font-Bold="True" 
                            runat="server"  Text="Opciones de Usuario"/>
        <table width="100%">
            <tr>
                    <td colspan="2" style="text-align:left">
                        <asp:Button ID="btnGUARDAR" runat="server" 
                            CssClass="rastreo_botones_save" Text="Guardar" OnClientClick="DisableSave(this);" />
                        <asp:Button ID="btnCANCELAR" runat="server" CausesValidation="false" 
                            CssClass="rastreo_botones_cancel" Text="Cancelar" />
                    </td>
                </tr>
            <tr>
                <td style="width:30%">
                                        Opcion de Pantalla
                                    </td>
                <td>
                    <asp:DropDownList ID="ddlOPCIONESGPS" runat="server" 
                                            CssClass="DropDownList" Width="99%" Visible="False" />
                    <asp:CheckBoxList ID="chkOpciones" runat="server" RepeatColumns="2" ToolTip='A = ALTA || B = BAJA || M = MODIFICACIONES || C - CONSULTAS'>
                    </asp:CheckBoxList>
                </td>
            </tr>
            
        </table>
    </asp:Panel>

</asp:Content>
	
	