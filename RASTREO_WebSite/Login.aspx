<%@ Page Title="RASTREO Paraguay - Bienvenido al sistema web de RASTREO Paraguay" 
         Language="VB" 
         MasterPageFile="RASTREOMasterPage.master" 
         AutoEventWireup="false" 
         CodeFile="Login.aspx.vb" 
         Inherits="_RASTREOLogin" 
         culture="auto" 
         meta:resourcekey="PageResource1" 
         UICulture="auto" %>
<asp:Content ID="ContentHead" ContentPlaceHolderID="main_ContentPlaceHolder" Runat="Server">
        <br /><br /><br />
        <asp:Label ID="errMsgs" runat="server" Width="100%" CssClass="Mensaje_de_error" 
            Visible="False" meta:resourcekey="errMsgsResource1"></asp:Label>
        <table width="100%">
        <tr>
        <td align="center">
            <asp:Image ID="Image1" runat="server" AlternateText="Bienvenido a RASTREO!" 
                ImageUrl="~/App_Themes/RASTREO/Imagenes/login.jpg" 
                meta:resourcekey="Image1Resource1" />
        </td>
        <td align="center">
                <asp:Login 
                    ID="Login_RASTREO" runat="server" BackColor="#F7F6F3" BorderColor="#E6E2D8" 
                    BorderPadding="4" BorderStyle="Solid" BorderWidth="1px" Font-Names="Nina,Segoe,Arial Narrow,Verdana"
                    Font-Size="0.8em" ForeColor="#333333"
                    FailureText="Usuario y/o Password incorrecto(s)." 
                    PasswordRequiredErrorMessage="El password es requerido para ingresar." 
                    RememberMeText="No cerrar sesión"
                    RememberMeSet ="true"
                    TitleText="Bienvenido a RASTREO Paraguay"
                    LabelStyle-HorizontalAlign="Left"
                    UserNameLabelText='<img src="App_Themes/RASTREO/Imagenes/system-users.png" />Usuario:' 
                    PasswordLabelText='<img src="App_Themes/RASTREO/Imagenes/system-lock-screen.png" />Contraseña:' 
                    UserNameRequiredErrorMessage="Nombre de usuario es requerido." 
                    LoginButtonText="RASTREAR" 
                    CreateUserText="Grupo TRES S.R.L. © Paraguay - 2010 "
                    CreateUserUrl="http://www.rastreo.com.py" 
                    meta:resourcekey="Login_RASTREOResource1">                    
                    <TextBoxStyle Font-Size="1.8em" Width="200px" Font-Names="Nina, Segoe, Arial Narrow, Verdana, Arial" CssClass="TextBoxMAIL"/>
                    <LoginButtonStyle BackColor="#FFFBFF" BorderColor="#CCCCCC" BorderStyle="Solid" 
                        BorderWidth="3px"  Font-Names="Nina, Segoe, Arial Narrow, Verdana" Font-Size="1.1em" ForeColor="#024579" />
                    <InstructionTextStyle Font-Italic="True" CssClass="rastreo_botones" BorderStyle="None" />

<LabelStyle HorizontalAlign="Left"></LabelStyle>

                    <TitleTextStyle BackColor="#5D7B9D" Font-Bold="True" Font-Size="1.1em" 
                        ForeColor="White" />
                </asp:Login>
        </td>                                                                                 
        <td align="center">
            <asp:Image ID="Image2" runat="server" AlternateText="Bienvenido a RASTREO!" 
                ImageUrl="~/App_Themes/RASTREO/Imagenes/login.jpg" 
                meta:resourcekey="Image2Resource1" />
        </td>
        </tr>
        <tr>
            <td colspan="3" align="center">
                <asp:Label CssClass="DropDownList" ID="lbDisclaimer" runat="server" 
                    meta:resourcekey="lbDisclaimerResource1">
                 Cuidamos lo tuyo.</asp:Label>
                <br />
                <asp:Panel ID="pnlDisclaimer" runat="server" BackColor="Silver" 
                 BorderColor="Navy" BorderStyle="Dotted" BorderWidth="2px"
                 Width="300px" Font-Size="Smaller" meta:resourcekey="pnlDisclaimerResource1">
                 <div style="text-align:justify">
                    <asp:Label ID="aviso_Rastrear" runat="server" meta:resourcekey="aviso_Rastrear">Estimado Cliente: Por seguridad de lo suyo, todos los intentos de acceso son 
                    almacenados en nuestro sistema y posteriormente auditados incluyendo los datos 
                    de origen de la conexión a continuación:</asp:Label>
                    <br /><br />
                    <b>IP</b>: <%= Request.UserHostAddress() %> 
                    <br />
                    <b>Browser</b>: <%= Request.Browser.Browser + " " + Request.Browser.Version %>
                    <br />
                </div>
                </asp:Panel>
                <ajaxToolkit:HoverMenuExtender runat="server" ID="hoverDisclaimer"
                 TargetControlID="lbDisclaimer" PopupPosition="Bottom" OffsetX="-95" PopupControlID="pnlDisclaimer"/>
            </td>
        </tr>
        <tr>
            <td colspan="3" align="center">
                Recomendamos utilizar
                <a href="http://www.google.com/chrome/index.html?hl=es" >
                Google Chrome * </a> 
                ||<a href="http://es-ar.www.mozilla.com/es-AR/">
                Firefox 3.5</a> 
                || <a href="http://www.opera.com/download/" >
                Opera 9+</a>              
                <%--|| <a href="http://www.microsoft.com/spain/windows/internet-explorer/download-ie.aspx">
                 IE 8.0+  </a> --%>
            </td>
        </tr>
        </table>
        <br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br />
</asp:Content>




