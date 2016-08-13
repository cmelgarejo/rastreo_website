<%@ Page Title="RASTREO Paraguay - Administración - ABM de Equipo GPS" Language="VB" MasterPageFile="RASTREOMasterPage.master" AutoEventWireup="false" CodeFile="admingps_equipogps.aspx.vb" Inherits="RASTREO_admin_equipogps" %>

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
                            CssClass="rastreo_botones_cancel" Text="Cancelar" OnClientClick="window.close();history.back();" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel runat="server" ID="panelEQUIPOGPSData" BackColor="LightSlateGray" ForeColor="White" Font-Bold="true"  >
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
                                <tr>
                                    <td colspan="2">
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
                                                        Height="100%" Visible="false" BackColor="#FFFF66" ForeColor="Black">
                                                        Tipo de EquipoGPS<br />                        
                                                        <asp:TextBox CssClass="TextBox" runat="server" ID="txtTipoDeEquipoGPS_Add" Width="100%"/>
                                                        <br />
                                                        <asp:ImageButton runat="server" ID="imgbtn_TipoDeEquipoGPSsave" AlternateText="Guardar" ToolTip="Guardar"  OnClientClick="DisableSave(this);"
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
                                                        Visible="false" BackColor="#FFFF66" ForeColor="Black">
                                                        SIM card<br />
                                                        <table width="100%">
                                                            <tr>
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
                                                                <td><asp:TextBox CssClass="TextBox" runat="server" ID="txtSIM_NRO" Width="100%"/></td>
                                                             </tr>   
                                                             <tr>
                                                                <td>PIN</td><td><asp:TextBox CssClass="TextBox" runat="server" ID="txtSIM_PIN" Width="100%"/></td>
                                                             </tr>
                                                             <tr>
                                                                <td>PIN2:</td><td><asp:TextBox CssClass="TextBox" runat="server" ID="txtSIM_PIN2" Width="100%"/></td>
                                                             </tr>
                                                             <tr>
                                                                <td>PUK:</td><td><asp:TextBox CssClass="TextBox"  runat="server" ID="txtSIM_PUK" Width="100%" /></td>
                                                             </tr>
                                                             <tr>
                                                                <td>PUK2:</td><td><asp:TextBox CssClass="TextBox"  runat="server" ID="txtSIM_PUK2" Width="50%"/></td>
                                                             </tr>
                                                        </table>
                                                        <br />
                                                        <asp:ImageButton runat="server" ID="imgbtn_SIMsave" AlternateText="Guardar" ToolTip="Guardar" OnClientClick="DisableSave(this);"  ImageUrl="~/App_Themes/CENTRAL/Imagenes/saveHS.png"/> &nbsp;
                                                        <asp:ImageButton runat="server" ID="imgbtn_SIMcancel" 
                                                            AlternateText="Cancelar" ToolTip="Cancelar" ImageUrl="~/App_Themes/CENTRAL/Imagenes/edit-undo.png" 
                                                            /> &nbsp;</asp:Panel>
                                                </td>
                                            </tr>
                                            
                                        </table>
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


