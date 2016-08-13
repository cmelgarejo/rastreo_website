<%@ Page Title="RASTREO Paraguay - Administración - Lista de Equipos GPS" Language="VB" MasterPageFile="RASTREOMasterPage.master" AutoEventWireup="false" CodeFile="admingps_equipogps_lista.aspx.vb" Inherits="RASTREO_Administracion_admin_equipogps_lista" %>
<asp:Content ID="Content2" ContentPlaceHolderID="main_ContentPlaceHolder" Runat="Server">
    <asp:UpdatePanel ID="updPanel_Vehiculos" runat="server" EnableViewState="true" >
        <ContentTemplate>
            <asp:Label ID="errMsgs" runat="server" Width="99%" CssClass="Mensaje_de_error" 
                Visible="False"></asp:Label>
            <table style="width:100%">
                <tr>
                    <td>
                        <asp:Button runat="server" ID="btnAddEquipoGPS" Text="Agregar equipo gps" CssClass="rastreo_botones" />
                    </td>
                    <td>
                        Busqueda:
                    </td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlBuscar"></asp:DropDownList>
                    </td>
                    <td><asp:TextBox runat="server" CssClass="TextBox" ID="txtBuscar"></asp:TextBox></td>
                    <td><asp:Button runat="server" ID="btnBuscar" Text="Buscar!" CssClass="rastreo_botones" /><asp:Button runat="server" ID="btnLimpiarBusqueda" Text="Limpiar busqueda" CssClass="rastreo_botones" /></td>
                </tr>
                <tr>
                    <td colspan="5">
                        <asp:GridView PageSize="30"  ID="gv_Lista_EquiposGPS" runat="server" 
                        DataSourceID="sqlds_Lista_equipogps" AllowPaging="True" 
                        AllowSorting="True" AutoGenerateColumns="False" BackColor="White" 
                        BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" 
                            GridLines="Vertical" Width="100%">
                        <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                        <Columns>
                        <asp:BoundField HeaderText="Vehiculo asociado" DataField="identificador_rastreo" />
                        <asp:BoundField HeaderText="ID del Equipo" DataField="id_equipo_gps" />
                        <asp:BoundField HeaderText="Tipo de Equipo GPS" DataField="tipo_de_equipo" />
                        <asp:BoundField HeaderText="SIM" DataField="sim_nro" />
                        <asp:BoundField HeaderText="Nro. de Serie" DataField="nro_serie_gps" />
                        <asp:TemplateField>
                            <ItemTemplate>
                                <a href="admingps_eventos_equipo_lista.aspx?id=<%# Eval("idrastreogps_equipogps") %>">Editar eventos del equipo</a>
                            </ItemTemplate>
                        </asp:TemplateField>      
                        <asp:TemplateField>
                            <ItemTemplate>
                                <a href="admingps_equipogps.aspx?id=<%# Eval("idrastreogps_equipogps") %>">Editar</a>
                            </ItemTemplate>
                        </asp:TemplateField>                        
                        <asp:TemplateField>
                            <ItemTemplate>
                                 <asp:LinkButton ID="linkdel" runat="server" CssClass="rastreo_botones_cancel"  
                                        Font-Size="X-Small" Text="Eliminar" CommandName="Eliminar" CommandArgument='<%#Eval("idrastreogps_equipogps") %>' OnClientClick="return seguro_que();" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        </Columns>
                        <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                        <PagerStyle BackColor="Navy" ForeColor="White" HorizontalAlign="Center" />
                        <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="Navy" ForeColor="White" HorizontalAlign="Center" 
                                Font-Bold="True" />
                        <AlternatingRowStyle BackColor="#BABABA" />
                        </asp:GridView>

                        <asp:SqlDataSource ID="sqlds_Lista_equipogps" runat="server" ConnectionString="<%$ ConnectionStrings:CS_RASTREOSystem %>" ProviderName="<%$ ConnectionStrings:CS_RASTREOSystem.ProviderName %>"
                        SelectCommand="select * from rsview_equipo_tipoequipo order by id_equipo_gps">
                        </asp:SqlDataSource>

                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>