<%@ Page Title="RASTREO Paraguay - Administración - Lista Fotos del Vehiculo" Language="VB" MasterPageFile="RASTREOMasterPage.master" AutoEventWireup="false" CodeFile="admin_cliente_vehiculo_fotos.aspx.vb" Inherits="Rastreo_admin_vehiculo_fotos" %>
<asp:Content ID="Content2" ContentPlaceHolderID="main_ContentPlaceHolder" Runat="Server">
    <script language="javascript" type="text/javascript">
        function checkFileExtension(elem) {
            var filePath = elem.value;

            if (filePath.indexOf('.') == -1)
                return false;

            var validExtensions = new Array();
            var ext = filePath.substring(filePath.lastIndexOf('.') + 1).toLowerCase();

            validExtensions[0] = 'jpg';
            validExtensions[1] = 'jpeg';
            validExtensions[2] = 'bmp';
            validExtensions[3] = 'png';
            validExtensions[4] = 'gif';
            validExtensions[5] = 'tif';
            validExtensions[6] = 'tiff';
            validExtensions[7] = 'txt';
            validExtensions[8] = 'doc';
            validExtensions[9] = 'xls';
            validExtensions[10] = 'pdf';

            for (var i = 0; i < validExtensions.length; i++) {
                if (ext == validExtensions[i])
                    return true;
            }
            elem.value = null;
            alert('La extensión ' + ext.toUpperCase() + ' no está permitida, elija otro archivo e intentelo de nuevo. \n Utilice solo imágenes por favor.');
            return false;
        }
    </script>
    <asp:UpdatePanel ID="updPanel_Vehiculos" runat="server" EnableViewState="true" >
        <Triggers>
            <asp:PostBackTrigger ControlID="btnUpfoto" />
            <asp:PostBackTrigger ControlID="btnAddFotos" />
        </Triggers>
        <ContentTemplate>
            <asp:Label ID="errMsgs" runat="server" Width="99%" CssClass="Mensaje_de_error" 
                Visible="False"></asp:Label>
            <table style="width:100%">
                <tr>
                    <td colspan="5">
                        <asp:Label runat="server" ID="lbCTAG" Text="Cliente:" /> <asp:Label runat="server" ID="lblCliente"/>
                        <br />
                        <asp:Label runat="server" ID="lbVTAG" Text="Vehiculo:" /> <asp:Label runat="server" ID="lblVehiculo"/>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button runat="server" ID="btnAddFotos" Text="Agregar foto" 
                            CssClass="rastreo_botones" />
                    </td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td>
                        <% If Request.QueryString.Count > 0 Then%>                   
                        <input class="rastreo_botones" onclick="window.location.href='admin_cliente_vehiculo_lista.aspx?id=<%= Request.QueryString("cid") %>';window.close();return true;" 
                            type="button" value="Volver" />
                        <% End If%>
                    </td>

                </tr>
                <tr>
                    <td colspan="5">
                        <asp:Panel runat="server" ID="pnlAddFoto" Visible="false" BorderColor="Navy" BorderStyle="Solid">
                            <asp:Label runat="server" Text="Descripción de la fotografía" CssClass="Label"/><br /><asp:TextBox Width="80%" runat="server" ID="txtFotoDesc" CssClass="TextBox"></asp:TextBox>
                            <br /><br />
                            <asp:FileUpload runat="server" ID="fuAddFoto" />
                            <asp:Button runat="server" ID="btnUpfoto" Text="Subir foto" CssClass="rastreo_botones_save" OnClientClick="DisableSave(this);"/>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td colspan="5">
                        <asp:GridView PageSize="30"  ID="gv_Lista_Vehiculos_Fotos" runat="server" 
                        DataSourceID="sqlds_Lista_Vehiculo_Fotos" AllowPaging="True" 
                        AllowSorting="True" AutoGenerateColumns="False" BackColor="White" 
                        BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" 
                            GridLines="Vertical" Width="100%">
                        <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                        <Columns>
                        <asp:BoundField HeaderText="Id" DataField="idrastreo_vehiculo_fotos" />
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Panel runat="server" ID="pnlViewFoto" >
                                    <img src='<%# Eval("idrastreo_vehiculo_fotos") %>_<%# Eval("nombre_archivo") %>' alt="" width="320px" height="240px" /></asp:Panel>
                                <asp:Label runat="server" ID="lbVehiculo" CssClass="rastreo_linkbuttons"><%#Eval("descripcion_foto")%></asp:Label>
                                <ajaxToolkit:HoverMenuExtender runat="server" PopupControlID="pnlViewFoto" TargetControlID="lbVehiculo"></ajaxToolkit:HoverMenuExtender>
                                <%--<ajaxToolkit:PopupControlExtender runat="server" PopupControlID="pnlViewFoto" TargetControlID="lbVehiculo" Position="Center"  />--%>
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

                        <asp:SqlDataSource ID="sqlds_Lista_Vehiculo_Fotos" runat="server" ConnectionString="<%$ ConnectionStrings:CS_RASTREOSystem %>" ProviderName="<%$ ConnectionStrings:CS_RASTREOSystem.ProviderName %>"
                        SelectCommand="select * from rastreo_vehiculo_fotos where idcliente = @id_cliente and idvehiculo = @id_vehiculo"
                         >
                        <SelectParameters>
                            <asp:QueryStringParameter DbType="Int32" Name="id_cliente" QueryStringField="cid" />
                            <asp:QueryStringParameter DbType="Int32" Name="id_vehiculo" QueryStringField="vid" />
                        </SelectParameters>
                        </asp:SqlDataSource>

                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>



