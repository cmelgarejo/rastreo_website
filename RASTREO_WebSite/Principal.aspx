<%@ Page Title="RASTREO Paraguay - Página Principal" Language="VB" MasterPageFile="RASTREOMasterPage.master" AutoEventWireup="false" CodeFile="Principal.aspx.vb" Inherits="_Principal" %>

<asp:Content ID="Content1" runat="server" 
    contentplaceholderid="main_ContentPlaceHolder">
    <asp:UpdatePanel runat="server" ID="paneli">
    <ContentTemplate>
    <div style="text-align:center; width:100% ">
    <asp:Image ID="imgRASTREO" runat="server" 
        ImageUrl="~/App_Themes/CENTRAL/Imagenes/logo_rastreo_original.jpg" />
    </div>
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


