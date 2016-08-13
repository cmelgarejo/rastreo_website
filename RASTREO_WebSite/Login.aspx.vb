Imports System.Collections.Generic
Imports RASTREOmw
Imports System.Diagnostics
Imports System.Threading
Imports System.Globalization

Partial Class _RASTREOLogin
    Inherits System.Web.UI.Page

    Const TOTAL_INTENTOS As Int16 = 5

    'Protected Overloads Overrides Sub Render(ByVal writer As HtmlTextWriter)
    '    Utilidades.CompresionASPX(HttpContext.Current)
    '    MyBase.Render(writer)
    'End Sub

    Protected Overrides Sub InitializeCulture()
        If Request.Form("ctl00$header_ContentPlaceHolder$ddlLang") IsNot Nothing Then
            Dim selectedLanguage As String = _
                (Request.Form("ctl00$header_ContentPlaceHolder$ddlLang"))
            UICulture = Request.Form("ctl00$header_ContentPlaceHolder$ddlLang")
            Culture = Request.Form("ctl00$header_ContentPlaceHolder$ddlLang")
            Thread.CurrentThread.CurrentCulture = _
                CultureInfo.CreateSpecificCulture(selectedLanguage)
            Thread.CurrentThread.CurrentUICulture = New  _
                CultureInfo(selectedLanguage)
        End If
        MyBase.InitializeCulture()
    End Sub

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Dim sbScript As New StringBuilder()
        sbScript.Append("<script language='JavaScript' type='text/javascript'>" + ControlChars.Lf)
        sbScript.Append("<!--" + ControlChars.Lf)
        'sbScript.Append("window.close();" + ControlChars.Lf)
        sbScript.Append("// -->" + ControlChars.Lf)
        sbScript.Append("</script>" + ControlChars.Lf)
        If Session("session_UsuarioRASTREO") Is Nothing Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Onload_closewindow", _
                                                                  sbScript.ToString(), False)
            Return
        End If
    End Sub

    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        Try
            Dim mensaje As String = String.Empty
            mensaje = ViewState("msjerror")
            If mensaje = String.Empty Then
                mensaje = Session("msjerror")
                Session.Remove("msjerror")
            End If
            If mensaje <> String.Empty Then
                errMsgs.Text = mensaje
                errMsgs.Visible = True
                ViewState.Remove("msjerror")
            Else
                errMsgs.Visible = False
            End If
            If Request.QueryString.Count > 0 Then
                If Not Request.QueryString("to") Is Nothing Then
                    errMsgs.Text = "La sesion ha expirado."
                    errMsgs.Visible = True
                End If
            End If

        Catch myEX As Exception
            ViewState.Add("msjerror", myEX.Message + "<br>" + myEX.StackTrace)
        End Try
    End Sub

    Protected Sub Login_RASTREO_Authenticate(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.AuthenticateEventArgs) Handles Login_RASTREO.Authenticate
        ''comienzo mi codigo
        ''check if remember me checkbox is checked on login
        If (Login_RASTREO.RememberMeSet) Then
            'Check if the browser support cookies
            If (Request.Browser.Cookies) Then
                'Check if the cookie with name RastreoLOGIN exist on user's machine
                'Create a cookie with expiry of 365 days
                Response.Cookies("RastreoCOOKIE").Expires = DateTime.Now.AddDays(365)
                'Write username to the cookie
                Response.Cookies("RastreoCOOKIE").Item("UNAME") = Login_RASTREO.UserName
                'Write password to the cookie
                Response.Cookies("RastreoCOOKIE").Item("UPASS") = Login_RASTREO.Password
                'If the cookie already exist then wirte the user name and password on the cookie
            End If
        Else
            If (Not (Request Is Nothing)) Then
                If (Request.Browser.Cookies) Then
                    'Check if the cookie with name RastreoLOGIN exist on user's machine
                    If (Not Request.Cookies("RastreoCOOKIE") Is Nothing) Then
                        Dim myCookie As HttpCookie
                        myCookie = New HttpCookie("RastreoCOOKIE")
                        myCookie.Expires = DateTime.Now.AddDays(-1)
                        Response.Cookies.Add(myCookie)
                    End If
                End If
            End If
        End If
        doAuthenticate(Login_RASTREO.UserName, Login_RASTREO.Password)
    End Sub

    'Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    '    If Not Request.Form("usu") Is Nothing And _
    '       Not Request.Form("contra") Is Nothing Then
    '        Dim USER As String = Convert.ToString(Request.Form("usu"))
    '        Dim PASS As String = Convert.ToString(Request.Form("contra"))
    '        doAuthenticate(USER, PASS)
    '    Else
    '        If Not IsPostBack Then
    '            ''comienzo de mi prueba
    '            'Check if the browser support cookies
    '            If Request.Browser.Cookies Then
    '                'Check if the cookies with name RastreoLOGIN exist on user's machine
    '                If Request.Cookies("RastreoLOGIN") IsNot Nothing Then
    '                    'Pass the user name and password to the VerifyLogin method
    '                    Me.doAuthenticate(Request.Cookies("RastreoLOGIN")("UNAME").ToString(), Request.Cookies("RastreoLOGIN")("UPASS").ToString())
    '                    Exit Sub
    '                End If
    '            End If
    '            ''fin de mi prueba
    '            Session.Abandon()
    '        End If
    '        Login_RASTREO.Focus()
    '    End If
    'End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'If Not Request.Form("usu") Is Nothing And _
        '   Not Request.Form("contra") Is Nothing Then
        '   Dim USER As String = Convert.ToString(Request.Form("usu"))
        '   Dim PASS As String = Convert.ToString(Request.Form("contra"))
        '   doAuthenticate(USER, PASS)
        'Else
        Try
            If (Request.Browser.Cookies) Then
                'Check if the cookie with name RastreoLOGIN exist on user's machine
                If (Not (Request.Cookies("RastreoCOOKIE") Is Nothing)) Then
                    'Response.Write(String.Format("{0} - {1}", Request.Cookies("RastreoCOOKIE")("UNAME").ToString(), Request.Cookies("RastreoCOOKIE")("UPASS").ToString()))
                    If (Not (Request.Cookies("RastreoCOOKIE")("UNAME") Is Nothing)) Then
                        doAuthenticate(Request.Cookies("RastreoCOOKIE")("UNAME").ToString(), Request.Cookies("RastreoCOOKIE")("UPASS").ToString())
                    End If
                    Return
                End If
            End If
        Catch ex As Exception
            Response.Write(ex.Message + " - " + ex.StackTrace)
        End Try

        If Not IsPostBack Then
            Session.Abandon()

        End If
        Login_RASTREO.Focus()

        'End If
    End Sub

    Private Sub doAuthenticate(ByVal USERNAME As String, ByVal PASSWORD As String)
        Dim tblUsuarios As New rastreo_usuarios(cnn_str.CadenaDeConexion)
        Dim IpAccesando As String = GetIPAddress()
        Dim autenticado As Boolean = False
        USERNAME = USERNAME.Trim()
        PASSWORD = PASSWORD.Trim()
        Try
            With tblUsuarios
                .Where.Usuario.Value = USERNAME
                If .Query.Load() Then
                    If .RowCount >= 1 Then ' esto tiene que ser igual a uno, pero no se porque me sale asi JMMZ le pongo > para mi bd local
                        If .Intentos_login < TOTAL_INTENTOS Then
                            If .Pass = Cryptografia.SHA256Hash(PASSWORD) Then
                                ''user pcsa passw "26152aaf5ec480a68361aa83fd5f20dd425c3e5b4e9830592838803f5a7f102b"
                                ''user elprod y su passw "4441090ff8407090cc9bd4f43a0433e3eca6c52c4577ec365f0b3f36c97a4141"
                                'If .Pass = "ec86533b5a6850201dc32587f9210cbc812ab5caa12063a3513bdcaa6e052915" Then 'user pdiazmeyer
                                'If .Pass = "27cc6052975bd671c1aac10d8908dcd1171d20ce5ab796723e7c7b0b6dd8a6fd" Then 'user diligencia
                                'If .Pass = "5bacee4b682eaebe643ebb0b0c95d22ae0137fec5983b8878ffd91ac1f6dbf57" Then 'user hierropar
                                'If .Pass = "f0260d77520dfa9b7260b8f7ee475db1068e6e5712833766b6b4fc396d88a85c" Then 'user fasesrl
                                'If .Pass = "5bacee4b682eaebe643ebb0b0c95d22ae0137fec5983b8878ffd91ac1f6dbf57" Then 'user hierropar
                                'If .Pass = "3758c63ae3e509f8a73f3ad125b9634020b948780bc22272c5713e1f6ea08b12" Then 'user chortitzer
                                'If .Pass = "3207c7b6793408ae5b1a30e218ed123bab252a8612755a193a1764f7276d80af" Then 'user horizonte
                                'If .Pass = "4ca663db94cd815f8cadd2215e5109323f41efc9017cda70be2cc4df282818e3" Then 'user gilcar

                                .Fech_login = Now
                                .Intentos_login = 0
                                .Save()
                                Utilidades.Informaciones_del_sistema("Se ha ingresado exitosamente al sistema con el usuario: [" + _
                                        USERNAME + "] " + Environment.NewLine + _
                                        "Desde la ip: " + IpAccesando + Environment.NewLine + _
                                        "Browser: " + Request.Browser.Browser + " " + Request.Browser.Version + Environment.NewLine + _
                                        "Fecha y hora del acceso: " + Now.ToString(), EventLogEntryType.Information)
                                Dim user As Usuario = Nothing
                                user = AsignarUser(USERNAME, .Id_persona, .Idrastreo_usuarios)
                                If Not user Is Nothing Then
                                    autenticado = True
                                    Dim URLStr As String = String.Empty
                                    URLStr = Request.QueryString("rp")
                                    If URLStr <> String.Empty And Not user.Cliente Then
                                        Response.Redirect(URLStr)
                                    Else
                                        If user.Empleado Then
                                            Session.Timeout = 4 * 60
                                            Response.Redirect("~/Principal.aspx", False)
                                            If (Not Request.Cookies("RastreoCOOKIE") Is Nothing) Then
                                                Dim myCookie As HttpCookie
                                                myCookie = New HttpCookie("RastreoCOOKIE")
                                                myCookie.Expires = DateTime.Now.AddDays(-1)
                                                Response.Cookies.Add(myCookie)
                                            End If
                                            'Response.Redirect("Login.aspx?rp=" + Me.Request.Url.AbsoluteUri(), False)
                                            'Session.Abandon()
                                            'Response.Redirect("./Login.aspx?blk=1")
                                        ElseIf user.Cliente Then
                                            If Utilidades.FindPermissionFor(user.idUsuario, "OPCION_BUSES") Then
                                                Response.Redirect("~/bus_main.aspx", False)
                                            ElseIf Utilidades.FindPermissionFor(user.idUsuario, "OPCION_SEGURO") Then
                                                Response.Redirect("~/insurance_mainGPS.aspx", False)
                                            ElseIf Utilidades.FindPermissionFor(user.idUsuario, "MENU_USUARIOS") Then
                                                Response.Redirect("~/mainGPS.aspx", False)
                                            End If
                                        End If
                                    End If
                                End If
                            Else
                                If .s_Intentos_login <> String.Empty Then
                                    If .Intentos_login < TOTAL_INTENTOS Then
                                        ViewState.Add("msjerror", "Tiene " & TOTAL_INTENTOS - .Intentos_login & " intentos restantes.")
                                        Utilidades.Informaciones_del_sistema("Se ha intentado accesar erroneamente al usuario [" + _
                                                USERNAME + "] " + Environment.NewLine + _
                                                "Desde la ip: " + IpAccesando + Environment.NewLine + _
                                                "Intentos: " + .Intentos_login.ToString() + Environment.NewLine + _
                                                "Browser: " + Request.Browser.Browser + " " + Request.Browser.Version + Environment.NewLine + _
                                                "Fecha y hora del acceso: " + Now.ToString(), EventLogEntryType.Warning)
                                    End If
                                    .Intentos_login += 1
                                    .Save()
                                End If
                            End If
                        Else
                            ViewState.Add("msjerror", "El usuario ha sido bloqueado por razones de seguridad, por favor comuniquese con la empresa.")
                            If .Intentos_login = TOTAL_INTENTOS Then
                                Utilidades.Informaciones_del_sistema("ATENCION! El usuario [" + _
                                        USERNAME + "] ha sido bloqueado por seguridad " + Environment.NewLine + _
                                        "Desde la ip: " + IpAccesando + Environment.NewLine + _
                                        "Intentos: " + .Intentos_login.ToString() + Environment.NewLine + _
                                        "Browser: " + Request.Browser.Browser + " " + Request.Browser.Version + Environment.NewLine + _
                                        "Fecha y hora del acceso: " + Now.ToString(), EventLogEntryType.Error)
                            End If
                            .Intentos_login += 1
                            .Save()
                        End If
                        End If
                End If
                'If Request.QueryString.Count > 0 And autenticado = False Then
                '    If Not Request.QueryString("to") Is Nothing Then
                '        Response.Redirect("Login.aspx")
                '    End If
                'End If
            End With
        Catch ex As Exception
            ViewState.Add("msjerror", ex.ToString())
        End Try
    End Sub

    Private Function AsignarUser(ByVal user As String, ByVal personaid As Integer, ByVal userid As Integer) As Usuario
        Dim tblPERSONA As New rastreo_persona(cnn_str.CadenaDeConexion)
        Dim tblCLIENTE As New rastreo_cliente(cnn_str.CadenaDeConexion)
        Dim tblEMPLEADO As New rastreo_empleados(cnn_str.CadenaDeConexion)
        Dim Nombre As String = String.Empty
        Dim idSEGURO As Integer = 0
        Dim Empleado As Boolean = False
        Dim Cliente As Boolean = False
        Dim superuser As Boolean = False
        Try
            With tblPERSONA
                .LoadByPrimaryKey(personaid)
                If .s_Id_seguro <> String.Empty Then
                    idSEGURO = .Id_seguro
                End If
                If .Nombre <> String.Empty Or .Apellido <> String.Empty Then
                    Nombre = .Nombre + " " + .Apellido
                ElseIf .Razon_social <> String.Empty Then
                    Nombre = .Razon_social
                End If
            End With

            With tblEMPLEADO
                .LoadByPrimaryKey(personaid)
                If .s_Acceso_sistema <> String.Empty Or .s_Estado_empleado <> String.Empty Then
                    If .Estado_empleado Then 'si estas activo
                        If .Acceso_sistema Then 'y tenes permiso de entrar al sistema
                            Empleado = True ' ingresas
                        Else
                            Return Nothing 'sino, salis
                        End If
                    Else
                        Return Nothing
                    End If
                End If
            End With

            With tblCLIENTE
                .LoadByPrimaryKey(personaid)
                If .s_Acceso_sistema <> String.Empty Then
                    If .Acceso_sistema Then
                        Cliente = True
                    Else
                        Return Nothing
                    End If
                    If Empleado Then
                        ' Empleado = False 'Si sos empleado te tratamos como cliente?
                    End If
                End If
            End With

            Dim NombrePersona As String = String.Empty
            Dim tblUSU As New rastreo_usuarios(cnn_str.CadenaDeConexion)
            With tblUSU
                If .LoadByPrimaryKey(userid) Then
                    If .s_Usuario <> String.Empty Then
                        If .s_Su <> String.Empty Then
                            superuser = .Su
                        End If
                        NombrePersona = .Nombre_completo
                    End If
                End If
            End With

            Dim myUser As New Usuario
            myUser.idSeguro = idSEGURO
            myUser.Usuario = user
            myUser.idUsuario = userid
            myUser.idPersona = personaid
            myUser.IP = GetIPAddress()
            myUser.NombreUsuario = NombrePersona
            myUser.Nombre = Nombre
            myUser.Cliente = Cliente
            myUser.Empleado = Empleado
            myUser.SU = superuser
            Session.Add("session_UsuarioRASTREO", myUser)
            'Utilidades.HazmeImagen(NombrePersona, "C:\usuario.png", System.Drawing.Imaging.ImageFormat.Png)
            Return myUser
        Catch ex As Exception
            ErrHandler.WriteError(ex.Message)
            Session.Add("msjerror", ex.ToString())
            Return Nothing
        End Try
    End Function

    Private Function GetIPAddress() As String
        Try
            Dim sIPAddress As String
            sIPAddress = Request.ServerVariables("HTTP_X_FORWARDED_FOR")
            If sIPAddress = String.Empty Then
                sIPAddress = Request.ServerVariables("REMOTE_ADDR")
            End If
            Return sIPAddress
        Catch ex As Exception
            Return String.Empty
            Session.Add("msjerror", ex.ToString())
        End Try
    End Function
End Class
