<%@ Page Title="RASTREO Paraguay - Administración - ABM de Contacto" Language="VB" MasterPageFile="RASTREOMasterPage.master" AutoEventWireup="false" CodeFile="admin_cliente_contacto.aspx.vb" Inherits="RASTREO_Administracion_admin_cliente_contacto" %>

<asp:Content ID="Content2" ContentPlaceHolderID="main_ContentPlaceHolder" Runat="Server">
    <asp:UpdatePanel ID="updPanel_Vehiculos" runat="server">
        <ContentTemplate>
            <asp:Label ID="errMsgs" runat="server" Width="99%" CssClass="Mensaje_de_error" 
                Visible="False"></asp:Label>
            <table style="width:100%; background-color:Silver; font-weight: bold; color:White;">
                <tr>
                    <td colspan="2" style="text-align:right">
                        <asp:Button ID="btnGUARDAR" runat="server"
                            CssClass="rastreo_botones_save" Text="Guardar" 
                            onclick="btnGUARDAR_Click" />
                        <asp:Button ID="btnCANCELAR" runat="server" CausesValidation="false" 
                            CssClass="rastreo_botones_cancel" Text="Cancelar" OnClientClick="window.close();history.back();" />
                    </td>
                </tr>
                <tr>
                    <td style="width:30%">
                        Nombre, Apellido o Razon social del contacto</td>
                    <td>
                        <asp:TextBox CssClass="TextBox" ID="txtCONTACTO_nomb" runat="server" Width="99%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Nro. de Documento (si aplica)</td>
                    <td>
                        <asp:TextBox CssClass="TextBox" ID="txtCONTACTO_nrodoc" runat="server" Width="99%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Relación con el cliente</td>
                    <td>
                        <asp:TextBox ID="txtCONTACTO_relacion" runat="server" CssClass="TextBox" 
                            Width="99%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Teléfono(s)</td>
                    <td>
                        <asp:TextBox CssClass="TextBox" ID="txtCONTACTO_telefonos" runat="server" Width="99%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        e-mail</td>
                    <td>
                        <asp:TextBox CssClass="TextBoxMAIL" ID="txtCONTACTO_mail" runat="server" Width="99%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>Autorizado para solicitar datos</td>
                    <td> <asp:CheckBox runat="server" ID="chkAutorizado" Checked="false" /> </td>
                </tr>
                <tr>
                    <td>Titular secundario</td>
                    <td> <asp:CheckBox runat="server" ID="chkTitularSecundario" Checked="false" /> </td>
                </tr>

            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

