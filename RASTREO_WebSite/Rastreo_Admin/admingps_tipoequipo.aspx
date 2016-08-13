<%@ Page Title="" Language="VB" MasterPageFile="~/Rastreo_Admin/RASTREOMasterPage.master" AutoEventWireup="false" CodeFile="admingps_tipoequipo.aspx.vb" Inherits="Rastreo_Admin_Default" %>

<asp:Content ID="Content2" ContentPlaceHolderID="main_ContentPlaceHolder" Runat="Server">
<table width="100%" cellspacing="0" cellpadding="0">
    <tr>
        <td>
            <asp:Label ID="errMsgs" runat="server" Width="100%" CssClass="Mensaje_de_error" 
            Visible="False"></asp:Label>
            <asp:Panel runat="server" ID="panel_TipoDeEquipoGPS_select" Width="100%" Height="100%" ForeColor="White" BackColor="LightSlateGray">
                                            Tipo de EquipoGPS<br />
                                            <asp:DropDownList CssClass="DropDownList" runat="server" 
                                                ID="ddlTipoDeEquipoGPS" AutoPostBack="true" Width="100%"/>
                                            <br />
                                            <asp:LinkButton CssClass="rastreo_linkbuttons" runat="server" ID="lnkTipoDeEquipoGPS_edit" Text="Editar"/><br />
                                            <asp:LinkButton CssClass="rastreo_linkbuttons" runat="server" ID="lnkTipoDeEquipoGPS_nueva" Text="Nueva"/></asp:Panel>                                    
            <asp:Panel runat="server" ID="panel_TipoDeEquipoGPSs_newedit" Width="100%" 
                                            Height="100%" Visible="false" BackColor="LightSlateGray" ForeColor="White" BorderColor="#CC0000" 
                                            BorderStyle="Solid" BorderWidth="2px">
                                            Tipo de EquipoGPS<br />                        
                                            <asp:TextBox CssClass="TextBox" runat="server" ID="txtTipoDeEquipoGPS_Add" Width="100%"/>
                                            <br />
                                            Tipo de Reporte del equipo<br />                        
                                            <asp:TextBox CssClass="TextBox" runat="server" ID="txtTipoReporte_Add" 
                                                Width="100%" 
                                                ToolTip="Aqui ingrese el tipo de reporte del equipo, ej: RGP, RGPTRAXS4, TK1000, G381"/>
                                            <br />
                                            <asp:ImageButton runat="server" ID="imgbtn_TipoDeEquipoGPSsave" OnClientClick="DisableSave(this);" AlternateText="Guardar" ToolTip="Guardar"  
                                                ImageUrl="~/App_Themes/CENTRAL/Imagenes/saveHS.png"/> &nbsp;
                                            <asp:ImageButton runat="server" ID="imgbtn_TipoDeEquipoGPScancel" AlternateText="Cancelar" ToolTip="Cancelar" ImageUrl="~/App_Themes/CENTRAL/Imagenes/edit-undo.png"/> &nbsp;</asp:Panel>
        </td>
    </tr>
</table>
</asp:Content>

