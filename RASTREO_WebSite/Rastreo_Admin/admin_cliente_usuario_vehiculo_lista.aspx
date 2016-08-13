<%@ Page Title="RASTREO Paraguay - Administración - Lista de Vehiculos" Language="VB" MasterPageFile="RASTREOMasterPage.master" AutoEventWireup="false" CodeFile="admin_cliente_usuario_vehiculo_lista.aspx.vb" Inherits="RASTREO_Administracion_admin_cliente_vehiculo_lista" %>
<asp:Content ID="Content2" ContentPlaceHolderID="main_ContentPlaceHolder" Runat="Server">

    <asp:UpdatePanel ID="updPanel_Vehiculos" runat="server" EnableViewState="true" >
        <ContentTemplate>
            Cliente: <asp:Label ID="lblCliente" runat="server"></asp:Label>
            <br />
            Usuario: <asp:Label ID="lblUser" runat="server"></asp:Label>
            <br />
            <asp:Label ID="errMsgs" runat="server" Width="99%" CssClass="Mensaje_de_error" 
                Visible="False"></asp:Label>
            <table style="width:100%">
                <tr>
                    <td align="right">
                            <input class="rastreo_botones" onclick="history.back();" 
                            type="button" value="Volver" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:GridView PageSize="50"  ID="gvListaUser" runat="server" ShowHeader="true" Caption="Lista de vehiculos visibles por el usuario" Font-Names="Nina, Segoe, Arial Narrow, Arial, Verdana"
                        DataSourceID="sqlds_listauser" AllowPaging="True" 
                        AllowSorting="True" AutoGenerateColumns="False" BackColor="White" 
                        BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" 
                            GridLines="Vertical" Width="100%">
                        <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                        <Columns>
                        <asp:BoundField HeaderText="Contraseña" DataField="identificador_rastreo" />
                        <asp:BoundField HeaderText="Patente" DataField="matricula" />
                        <asp:BoundField HeaderText="Marca" DataField="marca" />
                        <asp:BoundField HeaderText="Modelo" DataField="modelo" />
                        <asp:BoundField HeaderText="Año" DataField="anho" />
                        <asp:BoundField HeaderText="Color" DataField="color" />
                        <asp:BoundField HeaderText="Chasis" DataField="chasis" />
                        <asp:CheckBoxField HeaderText="Activo" DataField="activo" />
                        <asp:TemplateField>
                            <ItemTemplate>
                                <a href="admin_cliente_vehiculo_fotos.aspx?cid=<%# Eval("id_cliente") %>&vid=<%# Eval("id_vehiculo") %>">Ver fotos</a>
                            </ItemTemplate>
                        </asp:TemplateField>  
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Button Text="Eliminar" CommandArgument='<%#Eval("id_vehiculo") %>' CommandName="Eliminar" CssClass="rastreo_botones_cancel" ID="btnEliminarV" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>                                                  
                        
                        </Columns>
                        <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                        <PagerStyle BackColor="Navy" ForeColor="White" HorizontalAlign="Center" />
                        <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
                        <AlternatingRowStyle BackColor="#BABABA" />
                        </asp:GridView>
                        <asp:SqlDataSource ID="sqlds_listauser" runat="server" ConnectionString="<%$ ConnectionStrings:CS_RASTREOSystem %>" ProviderName="<%$ ConnectionStrings:CS_RASTREOSystem.ProviderName %>"
                        SelectCommand="select * from rsview_vehiculos_asignados_a_usuarios where activo = TRUE AND id_usuarios = @id_usuarios AND id_cliente = @id_cliente">
                        <SelectParameters>
                            <asp:QueryStringParameter DbType="Int32" Name="id_cliente" QueryStringField="id" />
                        </SelectParameters>
                        <SelectParameters>
                            <asp:QueryStringParameter DbType="Int32" Name="id_usuarios" QueryStringField="uid" />
                        </SelectParameters>
                        
                        </asp:SqlDataSource>
                    </td>
                </tr>            
                <tr>
                    <td>
                        <asp:GridView PageSize="50"  ID="gv_Lista_Vehiculos" runat="server" ShowHeader="true" Caption="Lista de vehiculos disponibles" Font-Names="Arial Narrow, Arial, Verdana"
                        DataSourceID="sqlds_Lista_Cliente_Vehiculo" AllowPaging="True" 
                        AllowSorting="True" AutoGenerateColumns="False" BackColor="White" 
                        BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" 
                            GridLines="Vertical" Width="100%">
                        <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                        <Columns>
                        <%--<asp:BoundField HeaderText="Id" DataField="idrastreo_vehiculo" />--%>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Button Text="Agregar" CommandArgument='<%#Eval("idrastreo_vehiculo") %>' CommandName="Agregar" CssClass="rastreo_botones" ID="btnAgregarV" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>                          
                        <asp:BoundField HeaderText="Contraseña" DataField="identificador_rastreo" />
                        <asp:BoundField HeaderText="Patente" DataField="matricula" />
                        <asp:BoundField HeaderText="Marca" DataField="marca" />
                        <asp:BoundField HeaderText="Modelo" DataField="modelo" />
                        <asp:BoundField HeaderText="Año" DataField="anho" />
                        <asp:BoundField HeaderText="Color" DataField="color" />
                        <asp:BoundField HeaderText="Chasis" DataField="chasis" />
                        <asp:CheckBoxField HeaderText="Activo" DataField="activo" />
                        <asp:TemplateField>
                            <ItemTemplate>
                                <a href="admin_cliente_vehiculo_fotos.aspx?cid=<%# Eval("idrastreo_persona") %>&vid=<%# Eval("idrastreo_vehiculo") %>">Ver fotos</a>
                            </ItemTemplate>
                        </asp:TemplateField>  
                        </Columns>
                        <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                        <PagerStyle BackColor="Navy" ForeColor="White" HorizontalAlign="Center" />
                        <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
                        <AlternatingRowStyle BackColor="#BABABA" />
                        </asp:GridView>
                        <asp:SqlDataSource ID="sqlds_Lista_Cliente_Vehiculo" runat="server" ConnectionString="<%$ ConnectionStrings:CS_RASTREOSystem %>" ProviderName="<%$ ConnectionStrings:CS_RASTREOSystem.ProviderName %>"
                        SelectCommand="SELECT DISTINCT ON (identificador_rastreo) * FROM rsview_cliente_vehiculo_equipogps WHERE activo = TRUE AND idrastreo_persona = @id_cliente AND idrastreo_vehiculo NOT IN (SELECT id_vehiculo FROM rsview_vehiculos_asignados_a_usuarios WHERE activo = TRUE AND id_usuarios = @id_usuarios AND id_cliente = @id_cliente)">
                        <SelectParameters>
                            <asp:QueryStringParameter DbType="Int32" Name="id_cliente" QueryStringField="id" />
                        </SelectParameters>
                        <SelectParameters>
                            <asp:QueryStringParameter DbType="Int32" Name="id_usuarios" QueryStringField="uid" />
                        </SelectParameters>
                        </asp:SqlDataSource>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>



