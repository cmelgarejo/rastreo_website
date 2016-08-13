<%@ Page Title="RASTREO Paraguay - Administración - Lista de Eventos de Tipo Equipos GPS" Language="VB" MasterPageFile="RASTREOMasterPage.master" AutoEventWireup="false" CodeFile="admingps_eventos_equipo_lista.aspx.vb" Inherits="RASTREO_admin_eventos_equipo_lista" %>
<asp:Content ID="Content2" ContentPlaceHolderID="main_ContentPlaceHolder" Runat="Server">
    <asp:UpdatePanel ID="updPanel_Vehiculos" runat="server" EnableViewState="true" >
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="gv_Lista_EventosGPS" EventName="RowDeleting" />
        </Triggers>
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
                                        <asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlEventosDisponibles" AutoPostBack="true" />
                                        <asp:CheckBox runat="server" ID="chkHabilitado" Text="Habilitado?" />
                                        <asp:CheckBox runat="server" ID="chkSonoro" Text="Sonoro?" />
                                        <br />
                                        <asp:TextBox ID="txtEvento" runat="server" CssClass="TextBox"></asp:TextBox>
                                        <br />
                                        Emails a enviar si el evento sucede: (separarlos por <b>;</b> o <b>,</b>)
                                        <asp:TextBox ID="txtMails" runat="server" CssClass="TextBoxMAIL" Width="100%" Height="100px">
                                        </asp:TextBox>
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
                        SelectCommand="select * from rsview_equipo_eventos WHERE id_rastreoequipogps = @equipogps order by evento">
                        <SelectParameters>
                            <asp:QueryStringParameter DbType="Int32" Name="equipogps" QueryStringField="id" />
                        </SelectParameters>
                        </asp:SqlDataSource>

                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>



