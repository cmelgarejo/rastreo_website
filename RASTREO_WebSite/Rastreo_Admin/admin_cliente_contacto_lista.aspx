<%@ Page Title="RASTREO Paraguay - Administración - Lista de Contacto" Language="VB" MasterPageFile="RASTREOMasterPage.master" AutoEventWireup="false" CodeFile="admin_cliente_contacto_lista.aspx.vb" Inherits="RASTREO_Administracion_admin_cliente_contacto_lista" %>
<asp:Content ID="Content2" ContentPlaceHolderID="main_ContentPlaceHolder" Runat="Server">

    <asp:UpdatePanel ID="updPanel_Contactos" runat="server" EnableViewState="true" >
        <ContentTemplate>
            <asp:Label ID="errMsgs" runat="server" Width="99%" CssClass="Mensaje_de_error" 
                Visible="False"></asp:Label>
            <table style="width:100%">
                <tr><td colspan="6" style="border: solid 3px navy">
                    Cliente:
                    <asp:Label runat="server" CssClass="Label" ID="lblCliente" />
                </td></tr>
                <tr>
                    <td>
                        <asp:Button runat="server" ID="btnAddContacto" Text="Agregar contacto" CssClass="rastreo_botones" />
                    </td>
                    <td>Busqueda:</td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlBuscar"></asp:DropDownList>
                    </td>
                    <td><asp:TextBox runat="server" CssClass="TextBox" ID="txtBuscar"></asp:TextBox></td>
                    <td><asp:Button runat="server" ID="btnBuscar" Text="Buscar!" CssClass="rastreo_botones" /><asp:Button runat="server" ID="btnLimpiarBusqueda" Text="Limpiar busqueda" CssClass="rastreo_botones" /></td>
                    <td>
                        <% If Request.QueryString.Count > 0 Then%>                   
                        <input class="rastreo_botones" onclick="window.location.href='admin_personas_lista.aspx';window.close();return true;" 
                            type="button" value="Volver" />
                        <% End If%>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <asp:GridView PageSize="30"  ID="gv_Lista_Contactos" runat="server" 
                        DataSourceID="sqlds_Lista_Cliente_Contacto" AllowPaging="True" 
                        AllowSorting="True" AutoGenerateColumns="False" BackColor="White" 
                        BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" 
                            GridLines="Vertical" Width="100%" 
                            PagerSettings-PageButtonCount="20" PagerSettings-Position="TopAndBottom">
                        <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                        <Columns>           
                        <asp:BoundField HeaderText="Nombre" DataField="nombre_apellido_razonsocial" />
                        <asp:BoundField HeaderText="Relacion" DataField="relacion"/>             
                        <asp:BoundField HeaderText="N° Documento" DataField="nrodoc_ruc" />
                        <asp:BoundField HeaderText="Telefono" DataField="telefono" />
                        <asp:BoundField HeaderText="e-mail" DataField="email" />
                        <asp:CheckBoxField HeaderText="Puede pedir datos?" DataField="Autorizado" />
                        <asp:TemplateField>
                            <ItemTemplate>
                                <a href="admin_cliente_contacto.aspx?cid=<%=Request.QueryString("id")%>&id=<%# Eval("idrastreo_cliente_contactos") %>">Editar</a>
                            </ItemTemplate>
                        </asp:TemplateField>    
                        <asp:TemplateField>
                            <ItemTemplate>
                                 <asp:LinkButton ID="LinkButton1" runat="server" CssClass="rastreo_botones_cancel" 
                                        Font-Size="X-Small" Text="Eliminar" CommandName="Eliminar" CommandArgument='<%#Eval("idrastreo_cliente_contactos") %>' OnClientClick="return seguro_que();" />
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
                        <asp:SqlDataSource ID="sqlds_Lista_Cliente_Contacto" runat="server" ConnectionString="<%$ ConnectionStrings:CS_RASTREOSystem %>" ProviderName="<%$ ConnectionStrings:CS_RASTREOSystem.ProviderName %>"
                        SelectCommand="select * from rastreo_cliente_contactos where rastreo_cliente_id_cliente = @id_cliente"
                         >
                        <SelectParameters>
                            <asp:QueryStringParameter DbType="Int32" Name="id_cliente" QueryStringField="id" />
                        </SelectParameters>
                        </asp:SqlDataSource>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

