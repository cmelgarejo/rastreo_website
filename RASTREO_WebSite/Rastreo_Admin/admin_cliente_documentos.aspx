<%@ Page Title="RASTREO Paraguay - Administración - Lista documentos de los vehiculos" Language="VB" MasterPageFile="RASTREOMasterPage.master" AutoEventWireup="false" CodeFile="admin_cliente_documentos.aspx.vb" Inherits="Rastreo_admin_clientes_documento" %>
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
                    return true;00
            }
            elem.value = null;
            alert('La extensión ' + ext.toUpperCase() + ' no está permitida, elija otro archivo e intentelo de nuevo. \n Utilice solo imágenes por favor.');
            return false;
        }
    </script>
    <asp:UpdatePanel ID="updPanel_Clientes_documentos" runat="server" EnableViewState="true" >
        <Triggers>
            <asp:PostBackTrigger ControlID="btnUpDocumento" />
            <asp:PostBackTrigger ControlID="btnAdddocumento" />
            <asp:PostBackTrigger ControlID="gv_Lista_Vehiculos_Documentos" />
        </Triggers>
        <ContentTemplate>
            <asp:Label ID="errMsgs" runat="server" Width="99%" CssClass="Mensaje_de_error" 
                Visible="False"></asp:Label>
            <table style="width:100%">
                <tr>
                    <td colspan="5">
                        <asp:Label runat="server" ID="lbCTAG" Text="Cliente:" /> <asp:Label runat="server" ID="lblCliente"/>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button runat="server" ID="btnAdddocumento" Text="Agregar Documento" 
                            CssClass="rastreo_botones" />
                    </td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td>
                        <% If Request.QueryString.Count > 0 Then%>                   
                        <input class="rastreo_botones" onclick="window.location.href='admin_personas_lista.aspx?id=<%= Request.QueryString("cid") %>';window.close();return true;" 
                            type="button" value="Volver" />
                        <% End If%>
                    </td>

                </tr>
                <tr>
                    <td colspan="5">
                        <asp:Panel runat="server" ID="pnlAddDocumento" Visible="false" BorderColor="Navy" BorderStyle="Solid">
                            <asp:Label runat="server" Text="Descripción del Documento" CssClass="Label"/><br /><asp:TextBox Width="80%" runat="server" ID="txtDocumentoDesc" CssClass="TextBox"></asp:TextBox>
                            <br /><br />
                            <asp:FileUpload runat="server" ID="fuAddDocumento" />
                            <asp:Button runat="server" ID="btnUpDocumento" Text="Subir Documento" CssClass="rastreo_botones_save" OnClientClick="DisableSave(this);"/>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td colspan="5">
                        <asp:GridView PageSize="30"  ID="gv_Lista_Vehiculos_Documentos" runat="server" 
                        DataSourceID="sqlds_Lista_Vehiculo_Documentos" AllowPaging="True" 
                        AllowSorting="True" AutoGenerateColumns="False" BackColor="White" 
                        BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" 
                            GridLines="Vertical" Width="100%">
                        <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                        <Columns>
                        <asp:BoundField HeaderText="Id" DataField="Idrastreo_cliente_documentos" />
                        <asp:BoundField HeaderText="Descripción" DataField="descripcion" />
                        <asp:BoundField HeaderText="Nombre del archivo" DataField="nombre_archivo" />
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Button runat="server" ID="btnVehiculo" CssClass="rastreo_botones" OnClick="DownloadFile" Text="Descargar archivo" CommandArgument='<%# Eval("idrastreo_cliente_documentos") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>   
                        <asp:CommandField ControlStyle-CssClass="rastreo_botones_cancel" ControlStyle-Font-Size="X-Small" ButtonType="Button" DeleteText="Eliminar" ShowDeleteButton="true" />
                        </Columns>
                        <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                        <PagerStyle BackColor="Navy" ForeColor="White" HorizontalAlign="Center" />
                        <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
                        <AlternatingRowStyle BackColor="#BABABA" />
                        </asp:GridView>

                        <asp:SqlDataSource ID="sqlds_Lista_Vehiculo_Documentos" runat="server" ConnectionString="<%$ ConnectionStrings:CS_RASTREOSystem %>" ProviderName="<%$ ConnectionStrings:CS_RASTREOSystem.ProviderName %>"
                        SelectCommand="select * from rastreo_cliente_documentos where idcliente = @id_cliente">
                        <SelectParameters>
                            <asp:QueryStringParameter DbType="Int32" Name="id_cliente" QueryStringField="cid" />
                        </SelectParameters>
                        </asp:SqlDataSource>

                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>



