<%@ Control Language="VB" AutoEventWireup="false" CodeFile="SoundPlayerI.ascx.vb" Inherits="SoundPlayerI" %>
<asp:Label ID="lbSndLabel" runat="server" Text="SoundPlayer by Christian Melgarejo" Visible="false"></asp:Label>
<% If Not Archivo_de_Sonido = String.Empty  %>
    <embed src='<%= Archivo_de_Sonido  %>' autostart='true' loop='false' 
    visible='false' width='0' height='0'> 
<% End If %>
</embed>
