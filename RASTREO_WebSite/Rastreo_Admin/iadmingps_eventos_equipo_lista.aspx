<%@ Page Title="ABM Eventos Equipo GPS" Language="VB" AutoEventWireup="false" CodeFile="~/Rastreo_Admin/iadmingps_eventos_equipo_lista.aspx.vb" Inherits="RASTREO_Administracion_admin_eventos_equipo_lista" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<!-- Autor: Ing. Adolfo Bresanovich || Director de R&D RASTREO Paraguay -->

<head id="Head1" runat="server">
<link rel="shortcut icon" href="~/RASTREO.ico" type="image/x-icon"/>
<link rel="icon" href="~/RASTREO.ico" type="image/x-icon />
<title>RASTREO WebSystem - Eventos EquipoGPS Manager</title>
</head>
<body class="body">
<form id="RASTREOmainForm" runat="server">
<ajaxToolkit:ToolkitScriptManager ID="RASTREOToolScriptManager" ScriptMode="Release" runat="server">
    <Scripts>
        <asp:ScriptReference Path="~/WebKit.js" />
    </Scripts>
</ajaxToolkit:ToolkitScriptManager>    
<div>       
    <asp:UpdatePanel ID="updPanel_Vehiculos" runat="server" EnableViewState="true" >
        <ContentTemplate>
            <asp:Label ID="errMsgs" runat="server" Width="99%" CssClass="Mensaje_de_error" 
                Visible="False"></asp:Label>
            <table style="width:100%">
                <tr>
                    <td>
                        <% If Request.QueryString.Count > 0 Then%>     
                            <asp:Button runat="server" ID="btnAddEvento" Text="Agregar evento gps" CssClass="rastreo_botones" />
                        <%Else%>
                            <asp:DropDownList ID="ddlEquiposGPS" CssClass="DropDownList" runat="server"   />
                            <asp:Button runat="server" ID="btnSeleccionarEquipo" Text="Seleccionar Equipo GPS" CssClass="rastreo_botones" />
                        <%End If%>
                    </td>
                    <td>                        
                    </td>
                    <td></td>
                    <td></td>
                    <td>
<%--                    <% If Request.QueryString.Count > 0 Then%>                   
                        <input class="rastreo_botones" onclick="window.location.href='admingps_eventos.aspx';window.close();return true;" 
                            type="button" value="Volver" />
                        <% End If%>--%>
                    </td>
                </tr>
                <tr>
                    <td colspan="5">
                        <asp:Panel runat="server" ID="pnlAddEvento" Visible="false" BackColor="AliceBlue" BorderColor="Black" BorderWidth="2">
                            <table style="width:100%">
                                <tr>
                                    <td>Eventos disponibles</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlEventosDisponibles" />
                                        <asp:CheckBox runat="server" ID="chkHabilitado" Text="Habilitado?" />
                                        <br />
                                        <asp:TextBox ID="txtEvento" runat="server" CssClass="TextBox"></asp:TextBox>
                                        <asp:Button runat="server" CssClass="rastreo_botones_save" OnClientClick="DisableSave(this);" ID="btnGuardaEvento" Text="Guardar evento" />
                                        
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        
                        <asp:GridView PageSize="30"  ID="gv_Lista_EventosGPS" runat="server" 
                        DataSourceID="sqlds_Lista_equipogps" AllowPaging="True" 
                        AllowSorting="True" AutoGenerateColumns="False" BackColor="White" 
                        BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" 
                            GridLines="Vertical" Width="100%">
                        <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                        <Columns>
                        <asp:BoundField HeaderText="Id" DataField="id_tipoevento" />
                        <asp:BoundField HeaderText="Descripcion" DataField="evento" />
                        <asp:BoundField HeaderText="Evento" DataField="evento_raw" />
                        <asp:CheckBoxField HeaderText="Habilitado?" DataField="habilitado" />   
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ID="lnkEditarEE" CommandName="Editar" CommandArgument='<%# Eval("id_tipoevento") %>'>Editar</asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>    
                                
                        <asp:CommandField ControlStyle-CssClass="rastreo_botones_cancel" ControlStyle-Font-Size="X-Small" ButtonType="Button" DeleteText="Eliminar" ShowDeleteButton="true"/>
                        </Columns>
                        <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                        <PagerStyle BackColor="Navy" ForeColor="White" HorizontalAlign="Center" />
                        <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
                        <AlternatingRowStyle BackColor="#BABABA" />
                        </asp:GridView>

                        <asp:SqlDataSource ID="sqlds_Lista_equipogps" runat="server" ConnectionString="<%$ ConnectionStrings:CS_RASTREOSystem %>" ProviderName="<%$ ConnectionStrings:CS_RASTREOSystem.ProviderName %>"
                        SelectCommand="select * from rsview_equipo_eventos WHERE id_rastreoequipogps = @equipogps">
                        <SelectParameters>
                            <asp:QueryStringParameter DbType="Int32" Name="equipogps" QueryStringField="id" />
                        </SelectParameters>
                        </asp:SqlDataSource>

                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
</form>
</body>
</html>