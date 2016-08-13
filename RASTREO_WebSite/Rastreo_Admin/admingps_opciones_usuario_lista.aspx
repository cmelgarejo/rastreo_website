<%@ Page Language="VB" MasterPageFile="RASTREOMasterPage.master" Title="RASTREO Paraguay - Administración - Lista de Opciones de pantalla del Usuario" CodeFile="admingps_opciones_usuario_lista.aspx.vb" AutoEventWireup="false" Inherits="admingps_opciones_usuario_lista" %>


<asp:Content ID="rastreo_admincontent" ContentPlaceHolderID="main_ContentPlaceHolder" Runat="Server">
    <asp:UpdatePanel ID="updPanel_Opciones" runat="server" EnableViewState="true" >
        <ContentTemplate>
            <asp:Label ID="errMsgs" runat="server" Width="99%" CssClass="Mensaje_de_error" 
                Visible="False"></asp:Label>
                <% If Request.QueryString("pid") <> "" And Request.QueryString("uid") <> "" Then %>
                    Permisos del usuario: <% Response.Write( usuario ) %>
                <%Else %>
                    Debe crear un usuario para poder asignarle permisos.
                <%End If %>
            <table style="width:100%">
                <tr>
                    <td>
                        <% If Request.QueryString("uid") <> "" Then %>
                            <%--<asp:Button runat="server" ID="btnAddOpcion" Text="Agregar Opcion" CssClass="rastreo_botones" />--%>
                            <asp:HyperLink ID="linkAgregar" runat="server" CssClass="rastreo_linkbuttons" NavigateUrl="admingps_opciones_usuario.aspx?uid={0}" Text="Agregar Opcion" />
                        <% End If %>
                    </td>
                    <td>                        
                    </td>
                    <td></td>
                    <td>
                        <% If Request.QueryString.Count > 0 Then%>                   
                        <input class="rastreo_botones" onclick="history.back();return true;" 
                            type="button" value="Volver" />
                        <% End If%>
                    </td>
                </tr>
                <tr>
                    <td colspan="5">
                        <asp:GridView ID="gv_Lista_Opciones" runat="server" 
                        DataSourceID="sqlds_Lista_Opciones" AllowPaging="True" 
                        AllowSorting="True" AutoGenerateColumns="False" BackColor="White" 
                        BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" 
                            GridLines="Vertical" Width="100%" PageSize="50">
                        <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                        <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                        <PagerStyle BackColor="Navy" ForeColor="White" HorizontalAlign="Center" />
                        <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
                        <AlternatingRowStyle BackColor="#BABABA" />
                        <Columns>
                        <asp:BoundField HeaderText="Id" DataField="opcion_de_pantalla" />
                        <asp:BoundField HeaderText="Opcion" DataField="descripcion" />
                        <%--<asp:TemplateField>
                            <ItemTemplate>
                                <a class="rastreo_linkbuttons" href="admingps_opciones_usuario.aspx?uid=<%# Eval("id_usuario") %>&id=<%# Eval("opcion_de_pantalla") %>">Editar</a>
                            </ItemTemplate>
                        </asp:TemplateField>--%>                        
                        <asp:CommandField ControlStyle-CssClass="rastreo_botones_cancel" ControlStyle-Font-Size="X-Small" ButtonType="Button" DeleteText="Eliminar" ShowDeleteButton="true"/>
                        </Columns>
                        
                        </asp:GridView>

                        <asp:SqlDataSource ID="sqlds_Lista_Opciones" runat="server" ConnectionString="<%$ ConnectionStrings:CS_RASTREOSystem %>" ProviderName="<%$ ConnectionStrings:CS_RASTREOSystem.ProviderName %>"
                        SelectCommand="select * from rsview_opcionespantalla_usuario where id_usuario = @idUsuario order by descripcion">
                        <SelectParameters>
                        <asp:QueryStringParameter DbType="Int32" QueryStringField="uid" Name="idUsuario" /> </SelectParameters>
                        </asp:SqlDataSource>

                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content >
	
	