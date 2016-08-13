<%@ Page Title="RASTREO Paraguay - Administración - Lista de Vehiculos" Language="VB" EnableEventValidation="false" AutoEventWireup="true" MasterPageFile="RASTREOMasterPage.master" CodeFile="admin_cliente_vehiculo_lista.aspx.vb" Inherits="admin_cliente_vehiculo_lista" %>
<asp:Content ID="Content2" ContentPlaceHolderID="main_ContentPlaceHolder" Runat="Server">
            <asp:Button CssClass="rastreo_botones_save" runat="server" ID="btnExcel" 
            Text="Descargar planilla" 
            ToolTip="Haga click aqui para descargar la lista visible en una planilla"/>
    <asp:UpdatePanel ID="updPanel_Vehiculos" runat="server" EnableViewState="true" >
        <ContentTemplate>
            <asp:Label ID="errMsgs" runat="server" Width="99%" CssClass="Mensaje_de_error" 
                Visible="False"></asp:Label>
            <table style="width:100%">
                <tr>
                    <td colspan="5">
                        <asp:UpdateProgress runat="server" AssociatedUpdatePanelID="updPanel_Vehiculos">
                            <ProgressTemplate>
                                <img src="../App_Themes/CENTRAL/Imagenes/ajax-loader.gif" alt="-"/>
                                <label class="rastreo_nowloading">Actualizando...</label>
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                    </td>
                </tr>
                <tr>
                    <td colspan="5">
                        <% If Request.QueryString.HasKeys Then%> 
                            <% If  Request.QueryString("id") <> String.Empty Then %>
                                <asp:Label runat="server" ID="lbTag" Text="Cliente:" />
                                <asp:Label runat="server" ID="lblCliente"/>
                            <% End If %>
                        <% Else  %>
                            <asp:DropDownList AutoPostBack="true" runat="server" ID="ddl_PickCliente" CssClass="DropDownList" />
                            <asp:Button CssClass="rastreo_botones" runat="server" ID="btnSelectCliente" Text="Seleccionar este cliente"  />
                            <asp:Button CssClass="rastreo_botones" runat="server" ID="btnTodos" Text="Ver TODOS los vehiculos"  />
                        <% End If %>
                    </td>
                </tr>
                <tr>
                    <td>
                        <% If Request.QueryString.Count > 0 Then%> 
                            <asp:Button runat="server" ID="btnAddVehiculo" Text="Agregar vehiculo" CssClass="rastreo_botones" />
                        <% End If %>
                    </td>
                    <td>
                        <asp:CheckBox runat="server" ID="chkInactivos" Text="Ver vehiculos inactivos" AutoPostBack="true" />
                    </td>
                    <td></td>
                    <td></td>
                    <td>
                        <% If Request.QueryString.Count > 0 Then%>                   
                        <input class="rastreo_botones" onclick="window.location.href='admin_personas_lista.aspx';window.close();return true;" 
                            type="button" value="Volver" />
                        <% End If%>
                    </td>

                </tr>
                <tr>
                    <td colspan="5">
                        <asp:GridView PageSize="30"  ID="gv_Lista_Vehiculos" runat="server" 
                        DataSourceID="sqlds_Lista_Cliente_Vehiculo" AllowPaging="True" 
                        AllowSorting="True" AutoGenerateColumns="False" BackColor="White" 
                        BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="1" 
                            GridLines="Vertical" Width="100%">
                        <HeaderStyle Font-Size="X-Small" />
                        <RowStyle BackColor="#EEEEEE" ForeColor="Black" Font-Size="Smaller" Font-Names="Arial Narrow, Verdana" />
                        <Columns>
                        <%--<asp:BoundField HeaderText="Id" DataField="idrastreo_vehiculo" />--%>
                        <%--<asp:TemplateField>
                            <ItemTemplate>
                                <a href="admin_cliente_vehiculo_fotos.aspx?cid=<%# Eval("idrastreo_persona") %>&vid=<%# Eval("idrastreo_vehiculo") %>">Ver fotos</a>
                            </ItemTemplate>
                        </asp:TemplateField>--%>  
                            <%--26082010-- Pedro Silva --%>
                        <%--  <asp:BoundField DataField="nombre" HeaderText="Nombre Persona" />--%> 
                                                       
                            <%--<asp:BoundField DataField="razon_social" HeaderText="Razon_Social" />--%>
                            <%--26082010-- Pedro Silva --%>
                        <asp:BoundField HeaderText="Id Movil" DataField="identificador_rastreo" 
                                ItemStyle-Font-Size="XX-Small" >
                            <ItemStyle Font-Size="XX-Small" />
                            </asp:BoundField>
                           <%--Mis cambios 28082010-- Pedro Silva --%>
                        <asp:BoundField HeaderText="Cliente" DataField="cliente" 
                                ItemStyle-Font-Size="XX-Small" >
                            <ItemStyle Font-Size="XX-Small" />
                            </asp:BoundField>   
                            <%--Mis cambios 28082010-- Pedro Silva --%>
                            <%--Mis cambios 01092010-- Pedro Silva --%>
                        <asp:BoundField HeaderText="Nro. Doc/RUC" DataField="nro_documento" 
                                ItemStyle-Font-Size="XX-Small" >
                            <ItemStyle Font-Size="XX-Small" />
                            </asp:BoundField>
                            <%--Mis cambios 01092010-- Pedro Silva --%>
                        <asp:BoundField HeaderText="ID Equipo" DataField="id_equipo_gps" 
                                ItemStyle-Font-Size="XX-Small" >
                            <ItemStyle Font-Size="XX-Small" />
                            </asp:BoundField>
                               
                        <asp:BoundField HeaderText="Tipo Equipo" DataField="tipoequipogps"  
                                ItemStyle-Font-Size="XX-Small" >
                            <ItemStyle Font-Size="XX-Small" />
                            </asp:BoundField>
                        <asp:BoundField HeaderText="Nro. Poliza" DataField="poliza_nroorden" 
                                ItemStyle-Font-Size="XX-Small" >
                            <ItemStyle Font-Size="XX-Small" />
                            </asp:BoundField>
                        <asp:BoundField HeaderText="Patente" DataField="matricula"  
                                ItemStyle-Font-Size="XX-Small" >
                            <ItemStyle Font-Size="XX-Small" />
                            </asp:BoundField>
                        <asp:BoundField HeaderText="Marca" DataField="marca"  
                                ItemStyle-Font-Size="XX-Small" >
                            <ItemStyle Font-Size="XX-Small" />
                            </asp:BoundField>
                        <asp:BoundField HeaderText="Modelo" DataField="modelo"  
                                ItemStyle-Font-Size="XX-Small" >
                            <ItemStyle Font-Size="XX-Small" />
                            <%--<ItemStyle Font-Size="XX-Small" />--%>
                            </asp:BoundField>
                        <asp:BoundField HeaderText="Año" DataField="anho" 
                                ItemStyle-Font-Size="XX-Small" >
                            <ItemStyle Font-Size="XX-Small" />
                            </asp:BoundField>
                        <asp:BoundField HeaderText="Color" DataField="color" 
                                ItemStyle-Font-Size="XX-Small" >
                            <ItemStyle Font-Size="XX-Small" />
                            </asp:BoundField>                                                      
                        <asp:BoundField HeaderText="Chasis" DataField="chasis" 
                                ItemStyle-Font-Size="XX-Small" >
                            <ItemStyle Font-Size="XX-Small" />
                            </asp:BoundField>
                        <asp:BoundField HeaderText="Fecha Instalacion" DataField="instalacion_fechahora" 
                                ItemStyle-Font-Size="XX-Small" >
                            <ItemStyle Font-Size="XX-Small" />
                            </asp:BoundField>
                        <asp:BoundField HeaderText="Reinstalacion" DataField="reinstalacion_fechahora" 
                                 ItemStyle-Font-Size = "XX-Small" >
                             <ItemStyle Font-Size="XX-Small" />
                             </asp:BoundField>
                        <asp:BoundField HeaderText="Sucursal de Instalacion" DataField="sucursal" 
                                ItemStyle-Font-Size="XX-Small" >
                            <ItemStyle Font-Size="XX-Small" />
                            </asp:BoundField>
                        <asp:BoundField HeaderText="Instalador" DataField="instalador" 
                                ItemStyle-Font-Size="XX-Small" >
                            <ItemStyle Font-Size="XX-Small" />
                            </asp:BoundField>
                        <asp:BoundField HeaderText="Vendedor" DataField="vendedor" 
                                ItemStyle-Font-Size="XX-Small" >
                            <ItemStyle Font-Size="XX-Small" />
                            </asp:BoundField>
                        <asp:CheckBoxField HeaderText="Demo" DataField="demo" />
                        <asp:CheckBoxField HeaderText="Activo" DataField="activo" />
                        <asp:BoundField HeaderText="SIM" DataField="sim_nro" 
                                ItemStyle-Font-Size="XX-Small" >
                            <ItemStyle Font-Size="XX-Small" />
                            </asp:BoundField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <a href="admin_cliente_vehiculo.aspx?cid=<%# Eval("idrastreo_persona")%>&id=<%# Eval("idrastreo_vehiculo") %>">Editar</a>
                            </ItemTemplate>
                        </asp:TemplateField>                        
                        <asp:TemplateField>
                            <ItemTemplate>
                                 <asp:LinkButton ID="btnEliminarVehiculo" runat="server" CssClass="rastreo_botones_cancel" 
                                        Font-Size="X-Small" Text="Eliminar" CommandName="Eliminar" CommandArgument='<%#Eval("idrastreo_vehiculo") %>' OnClientClick="return seguro_que();" />
                            </ItemTemplate>
                        </asp:TemplateField>
                            <asp:BoundField />
                        </Columns>
                        <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                        <PagerStyle BackColor="Navy" ForeColor="White" HorizontalAlign="Center" />
                        <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" Font-Size="Smaller" />
                        <AlternatingRowStyle BackColor="#BABABA" />
                        </asp:GridView>
                        <asp:SqlDataSource ID="sqlds_Lista_Cliente_Vehiculo" runat="server" 
                            ConnectionString="<%$ ConnectionStrings:CS_RASTREOSystem %>" 
                            ProviderName="<%$ ConnectionStrings:CS_RASTREOSystem.ProviderName %>">
                        <SelectParameters>
                            <asp:QueryStringParameter DbType="Int32" Name="id_cliente" QueryStringField="id" />
                            <asp:ControlParameter DbType="Boolean" Name="activo" ControlID="chkInactivos" PropertyName="Checked" />
                        </SelectParameters>
                        </asp:SqlDataSource>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>



