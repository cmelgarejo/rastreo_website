<%@ Page Title="RASTREO Paraguay - Administración - Lista de Eventos de Tipo Equipos GPS" Language="VB" MasterPageFile="RASTREOMasterPage.master" AutoEventWireup="false" CodeFile="admingps_eventos_lista.aspx.vb" Inherits="RASTREO_Administracion_admin_eventos_lista" %>
<asp:Content ID="Content2" ContentPlaceHolderID="main_ContentPlaceHolder" Runat="Server">
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
                            <asp:DropDownList ID="ddlTipoEquiposGPS" CssClass="DropDownList" runat="server"   />
                            <asp:Button runat="server" ID="btnSeleccionarTDE" Text="Seleccionar Tipo de Equipo GPS" CssClass="rastreo_botones" />
                        <%End If%>
                    </td>
                    <td>                        
                    </td>
                    <td></td>
                    <td></td>
                    <td>
                        <% If Request.QueryString.Count > 0 Then%>                   
                        <input class="rastreo_botones" onclick="window.location.href='admingps_eventos.aspx';window.close();return true;" 
                            type="button" value="Volver" />
                        <% End If%>
                    </td>
                </tr>
                <tr>
                    <td colspan="5">
                        <asp:GridView PageSize="30"  ID="gv_Lista_EventosGPS" runat="server" 
                        DataSourceID="sqlds_Lista_equipogps" AllowPaging="True" 
                        AllowSorting="True" AutoGenerateColumns="False" BackColor="White" 
                        BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" 
                            GridLines="Vertical" Width="100%">
                        <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                        <Columns>
                        <asp:BoundField HeaderText="Id" DataField="idrastreogps_tipo_evento" />                        
                        <asp:BoundField HeaderText="Evento" DataField="evento" />
                        <asp:TemplateField>
                            <ItemTemplate>
                                <label style='background-color:<%#Eval("color") %>'><%#Eval("descripcion")%></label>
                            </ItemTemplate>
                            <ItemStyle ForeColor="White" />
                        </asp:TemplateField>                        
                        <asp:TemplateField>
                            <ItemTemplate>
                                <a href="admingps_eventos.aspx?tdeid=<%=Request.QueryString("id")%>&id=<%# Eval("idrastreogps_tipo_evento") %>">Editar</a>
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
                        SelectCommand="select * from rastreogps_tipoevento WHERE id_tipoequipo = @id_tipoequipo order by evento">
                        <SelectParameters>
                            <asp:QueryStringParameter DbType="Int32" Name="id_tipoequipo" QueryStringField="id" />
                        </SelectParameters>
                        </asp:SqlDataSource>

                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>



