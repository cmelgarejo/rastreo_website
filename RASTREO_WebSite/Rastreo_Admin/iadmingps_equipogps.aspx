<%@ Page Title="ABM Equipo GPS" Language="VB" AutoEventWireup="false" CodeFile="iadmingps_equipogps.aspx.vb" Inherits="RASTREO_Administracion_admin_equipogps" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<!-- Autor: Ing. Adolfo Bresanovich || Director de R&D RASTREO Paraguay -->

<head id="Head1" runat="server">
<link rel="shortcut icon" href="~/RASTREO.ico" type="image/x-icon"/>
<link rel="icon" href="~/RASTREO.ico" type="image/x-icon"/>
<title>RASTREO WebSystem - EquipoGPS Manager</title>
</head>
<body class="body">
<form id="RASTREOmainForm" runat="server">
<ajaxToolkit:ToolkitScriptManager ID="RASTREOToolScriptManager" ScriptMode="Release" runat="server">
        <Scripts>
            <asp:ScriptReference Path="~/WebKit.js" />
        </Scripts>
</ajaxToolkit:ToolkitScriptManager>    
<div>       
<asp:UpdatePanel ID="updPanel_EquipoGPS" runat="server">
<ContentTemplate>
<table style="width:525px;">
    <tr>
        <td align="center">
            <asp:Button ID="btnNuevo" CssClass="rastreo_botones_save"  runat="server" Text="Nuevo" />
            <asp:Button ID="btnGUARDAR" CssClass="rastreo_botones_save" runat="server" Text="Guardar" />
        </td>

        <td>
        <asp:Label ID="errMsgs" runat="server" Width="100%" CssClass="Mensaje_de_error" 
                Visible="False"></asp:Label>
            <asp:Panel runat="server" ID="panelEQUIPOGPSData" BackColor="LightSlateGray" ForeColor="White" Font-Bold="true"  >
                <asp:Panel runat="server" ID="pnlINFOEQUIPOPS" BackColor="LightSlateGray" ForeColor="White" Font-Bold="true"  >
                <asp:Label ID="Label1" BackColor="#024579" ForeColor="White" Font-Bold="True" 
                runat="server"  Text="Datos del Equipo GPS"/>
                <table width="100%">
                    <tr>
                        <td style="width:30%">
                            ID del Equipo
                        </td>
                        <td>
                            <asp:TextBox ID="txtEQUIPOGPS_id_equipo_gps" runat="server" 
                                CssClass="TextBox" Width="99%"></asp:TextBox>
                        </td>
                    </tr>                                
                    <tr>
                        <td>
                            Nro. de Serie</td>
                        <td>
                            <asp:TextBox ID="txtEQUIPOGPS_nro_serie" runat="server" 
                                CssClass="TextBox" Width="99%"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                </asp:Panel>
                <table width="100%">
                    <tr>
                        <td>
                            <table width="100%" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td> 
                                        <asp:Panel runat="server" ID="panel_TipoDeEquipoGPS_select" Width="100%" Height="100%">
                                            Tipo de EquipoGPS<br />
                                            <asp:DropDownList CssClass="DropDownList" runat="server" 
                                                ID="ddlTipoDeEquipoGPS" AutoPostBack="true" Width="100%"/>
                                            <br />
                                            <asp:LinkButton CssClass="rastreo_linkbuttons" runat="server" ID="lnkTipoDeEquipoGPS_edit" Text="Editar"/><br />
                                            <asp:LinkButton CssClass="rastreo_linkbuttons" runat="server" ID="lnkTipoDeEquipoGPS_nueva" Text="Nueva"/></asp:Panel>                                    
                                        <asp:Panel runat="server" ID="panel_TipoDeEquipoGPSs_newedit" Width="100%" 
                                            Height="100%" Visible="false" ForeColor="Black" BorderColor="#CC0000" 
                                            BorderStyle="Solid" BorderWidth="2px">
                                            Tipo de EquipoGPS<br />                        
                                            <asp:TextBox CssClass="TextBox" runat="server" ID="txtTipoDeEquipoGPS_Add" Width="100%"/>
                                            <br />
                                            Tipo de Reporte del equipo<br />                        
                                            <asp:TextBox CssClass="TextBox" runat="server" ID="txtTipoReporte_Add" 
                                                Width="100%" 
                                                ToolTip="Aqui ingrese el tipo de reporte del equipo, ej: RGP, RGPTRAXS4, TK1000, G381"/>
                                            <br />
                                            <asp:ImageButton runat="server" ID="imgbtn_TipoDeEquipoGPSsave" OnClientClick="DisableSave(this);"  AlternateText="Guardar" ToolTip="Guardar"  
                                                ImageUrl="~/App_Themes/CENTRAL/Imagenes/saveHS.png"/> &nbsp;
                                            <asp:ImageButton runat="server" ID="imgbtn_TipoDeEquipoGPScancel" AlternateText="Cancelar" ToolTip="Cancelar" ImageUrl="~/App_Themes/CENTRAL/Imagenes/edit-undo.png"/> &nbsp;</asp:Panel>
                                    </td>
                                    <td>
                                        <asp:Panel runat="server" ID="panel_SIM_select" Width="100%" Height="100%">
                                            SIM card<br />
                                            <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlSIMs" AutoPostBack="true"  Width="100%"/><br />
                                             <asp:CheckBox runat="server" ID="dualSIM" Checked="false" Text="SIM card 2 (opcional)"/>
                                                        <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlSIMs2" AutoPostBack="true"  Width="100%"/>
                                                        <br />
                                            <asp:LinkButton CssClass="rastreo_linkbuttons" runat="server" ID="lnkSIM_edit" Text="Editar"/><br />
                                            <asp:LinkButton CssClass="rastreo_linkbuttons" runat="server" ID="lnkSIM_nuevo" Text="Nuevo"/></asp:Panel>
                                        <asp:Panel runat="server" ID="panel_SIM_newedit" Width="100%" Height="100%" 
                                            Visible="false" ForeColor="Black" BorderColor="#CC0000" 
                                            BorderStyle="Solid" BorderWidth="2px">
                                            
                                            <table width="100%">
                                                <tr>
                                                    <td>SIM Card</td>
                                                    <td>Prefijo</td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlPrefijos" runat="server" CssClass="DropDownList">
                                                        <asp:ListItem>097</asp:ListItem>
                                                        <asp:ListItem>099</asp:ListItem>
                                                        <asp:ListItem>098</asp:ListItem>
                                                        <asp:ListItem>095</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                
                                                <tr>
                                                    <td>Nro</td>
                                                    <td colspan="2"><asp:TextBox CssClass="TextBox" runat="server" ID="txtSIM_NRO" Width="100%"/></td>
                                                 </tr>   
                                                 <tr>
                                                    <td colspan="2">PIN: <asp:TextBox CssClass="TextBox" runat="server" ID="txtSIM_PIN" Width="100%"/></td>
                                                    <td>PIN2: <asp:TextBox CssClass="TextBox" runat="server" ID="txtSIM_PIN2" Width="100%"/></td>
                                                 </tr>
                                                 <tr>
                                                    <td colspan="2">PUK: <asp:TextBox CssClass="TextBox"  runat="server" ID="txtSIM_PUK" Width="100%" /></td>
                                                    <td>PUK2: <asp:TextBox CssClass="TextBox"  runat="server" ID="txtSIM_PUK2" Width="100%"/></td>
                                                 </tr>
                                            </table>
                                            <br />
                                            <asp:ImageButton runat="server" ID="imgbtn_SIMsave" OnClientClick="DisableSave(this);"  AlternateText="Guardar" ToolTip="Guardar"  ImageUrl="~/App_Themes/CENTRAL/Imagenes/saveHS.png"/> &nbsp;
                                            <asp:ImageButton runat="server" ID="imgbtn_SIMcancel" 
                                                AlternateText="Cancelar" ToolTip="Cancelar" ImageUrl="~/App_Themes/CENTRAL/Imagenes/edit-undo.png" 
                                                /> &nbsp;</asp:Panel></td>
                                     </tr>
                                     </table>
                                     </td></tr></table></asp:Panel></td></tr><tr>
    </tr>
</table>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</form>
</body>
</html>