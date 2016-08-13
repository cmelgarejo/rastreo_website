Imports RASTREOmw
Imports System.IO
Imports System.Drawing.Imaging
Imports System.IO.Compression
Imports MyGeneration.dOOdads
Imports System.Web.Mail
Imports Resources
Imports System.Threading
Imports System.Globalization
Imports System.Web.UI.HtmlControls.HtmlGenericControl

Partial Class mainGPS
    Inherits System.Web.UI.Page

    Public miUsuario As Usuario
    Public mi_usuario As String = String.Empty
    Public mi_cliente As String = String.Empty

    Public permiso_MENU_USUARIOS As Boolean = False
    Public permiso_REPORTEAUTOMATICO As Boolean = False
    Public permiso_HOJADERUTA As Boolean = False
    Public permiso_GEOCERCA As Boolean = False
    Public permiso_REFERENCIAS As Boolean = False
    Public permiso_BUSES As Boolean = False

    'Protected Overloads Overrides Sub Render(ByVal writer As HtmlTextWriter)
    '    Utilidades.CompresionASPX(HttpContext.Current)
    '    MyBase.Render(writer)
    'End Sub

    Protected Sub ddlLang_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlLang.SelectedIndexChanged
        If Not (Session.Item("LANG") Is Nothing) Then
            Session("LANG") = ddlLang.SelectedValue
        Else
            Session.Add("LANG", ddlLang.SelectedValue)
        End If
    End Sub

    Protected Overrides Sub InitializeCulture()
        If Session("LANG") IsNot Nothing Then
            Dim selectedLanguage As String = _
                    (Session("LANG"))
            UICulture = Session("LANG")
            Culture = Session("LANG")
            Thread.CurrentThread.CurrentCulture = _
                CultureInfo.CreateSpecificCulture(selectedLanguage)
            Thread.CurrentThread.CurrentUICulture = New  _
                CultureInfo(selectedLanguage)
        End If
        MyBase.InitializeCulture()
    End Sub

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Try
            If Session("LANG") IsNot Nothing Then
                ddlLang.SelectedValue = Session("LANG")
            End If
        Catch ex As Exception
            ErrHandler.WriteError(ex.Message)
            'System.Windows.Forms.MessageBox.Show("EERORRRR!!")
        End Try
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'Context.Session.Timeout = 60
            miUsuario = CType(Session("session_UsuarioRASTREO"), Usuario)
            If Not miUsuario Is Nothing Then
                'Session.Add("msjerror", miUsuario.idUsuario)
            End If
            If Not miUsuario Is Nothing Then
                If Not miUsuario.Cliente Then
                    Response.Redirect("Login.aspx?blk=1", False)
                End If
                mi_usuario = CType(Session("session_UsuarioRASTREO"), Usuario).NombreUsuario
                mi_cliente = CType(Session("session_UsuarioRASTREO"), Usuario).Nombre
                If Utilidades.FindPermissionFor(miUsuario.idUsuario, "MENU_USUARIOS") Then
                    permiso_MENU_USUARIOS = True
                End If
                If Utilidades.FindPermissionFor(miUsuario.idUsuario, "OPCION_HOJADERUTA") Then
                    permiso_HOJADERUTA = True
                End If
                If Utilidades.FindPermissionFor(miUsuario.idUsuario, "OPCION_GEOCERCA") Then
                    permiso_GEOCERCA = True
                End If
                If Utilidades.FindPermissionFor(miUsuario.idUsuario, "OPCION_REFERENCIAS") Then
                    permiso_REFERENCIAS = True
                End If
                If Utilidades.FindPermissionFor(miUsuario.idUsuario, "OPCION_BUSES") Then
                    permiso_BUSES = True
                End If
                If Utilidades.FindPermissionFor(miUsuario.idUsuario, "OPCION_REPORTEAUTOMATICO") Then
                    permiso_REPORTEAUTOMATICO = True
                End If
            Else
                Response.Redirect("./Login.aspx?to=" + Now.ToString(), False)
                Return
            End If

            idUsuario.Value = miUsuario.idUsuario
            idPersona.Value = miUsuario.idPersona

            If chkReferencias.Checked Then
                If Not pnlPuntoReferencia.Visible Then
                    Bind_DDLHojasDeReferencia(miUsuario)
                End If
                pnlPuntoReferencia.Visible = True
            Else
                pnlPuntoReferencia.Visible = False
            End If

            Dim sbScript As New StringBuilder()
            sbScript.Append("<script language='JavaScript' type='text/javascript'>" + ControlChars.Lf)
            sbScript.Append("//<!--" + ControlChars.Lf)
            sbScript.Append("countdown_timer(" & txtTimeout.Text & ");" & ControlChars.Lf)
            sbScript.Append("// -->" + ControlChars.Lf)
            sbScript.Append("</script>" + ControlChars.Lf)
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Onload_updatecountdown", _
                                                                  sbScript.ToString(), False)


            If Not Convert.ToBoolean(sendingmail.Value) Then
                'tmr_Reload.Enabled = True
                If IsPostBack Then
                    If idVehiculoSeleccionado.Value <> 0 And Convert.ToBoolean(seguimiento_activo.Value) Then
                        txtTimeout.Text = Session("TimeoutUpdate")
                        Reload_Data(True)
                    Else
                        Session("TimeoutUpdate") = txtTimeout.Text
                        'txtTimeout.Text = Session("TimeoutUpdate")
                        Reload_Data()
                    End If
                Else
                    Session("TimeoutUpdate") = "60"
                    txtTimeout.Text = "60"
                    Dim rpersona As New rastreo_persona(cnn_str.CadenaDeConexion)
                    Dim rcliente As New rastreo_cliente(cnn_str.CadenaDeConexion)
                    If rpersona.LoadByPrimaryKey(miUsuario.idPersona) Then
                        If rcliente.LoadByPrimaryKey(miUsuario.idPersona) Then
                            txtMailSettings.Text = rpersona.Email
                            If (rcliente.s_Ra_horaenvio <> String.Empty) Then txtHORA_ENVIO_REPORTE.Text = rcliente.Ra_horaenvio.ToString("HH:mm")
                            If rcliente.s_Reporte_automatico <> String.Empty Then
                                chk_RA_OnOff.Checked = rcliente.Reporte_automatico
                            Else
                                chk_RA_OnOff.Checked = False
                            End If
                        End If
                    End If
                End If

                getPosiciones(miUsuario)
                LoadReferencias(miUsuario)
                Bind_DDLSendMailTo(miUsuario)
                tmr_Reload.Interval = Convert.ToInt32(txtTimeout.Text) * 1000
            Else
                'tmr_Reload.Enabled = False5
            End If

        Catch ex As Exception
            ErrHandler.WriteError(ex.Message)
        Finally
            'GC.Collect()
        End Try
    End Sub

    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        Try
            If Convert.ToBoolean(sendingmail.Value) Then
                PopupMAIL.Show()
            End If
            Dim mensaje As String = String.Empty
            mensaje = Session("msjerror")
            If mensaje = String.Empty Then
                mensaje = Session("msjerror")
                Session.Remove("msjerror")
            End If
            If mensaje <> String.Empty Then
                errMsgs.Text = mensaje
                errMsgs.Visible = True
                Session.Remove("msjerror")
            Else
                errMsgs.Visible = False
            End If
        Catch myEX As Exception
            ErrHandler.WriteError(myEX.Message)
            Session.Add("msjerror", myEX.Message + "<br>" + myEX.StackTrace)
        End Try
    End Sub

    Private Sub LogOUT()
        'Session.Abandon()
        ''Intento hacer que expire el COOKIE del RastreoLOGIN si el tipo le da salir....
        If (Not Request.Cookies("RastreoCOOKIE") Is Nothing) Then
            Dim myCookie As HttpCookie
            myCookie = New HttpCookie("RastreoCOOKIE")
            myCookie.Expires = DateTime.Now.AddDays(-1)
            Response.Cookies.Add(myCookie)
        End If
        'Response.Redirect("Login.aspx?rp=" + Me.Request.Url.AbsoluteUri(), False)
        Session.Abandon()
        Response.Redirect("Login.aspx?rp=" + Me.Request.Url.AbsoluteUri(), False)
    End Sub

    Protected Sub RASTREO_LOGOUT_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RASTREO_LOGOUT.Click
        LogOUT()
    End Sub

    Protected Sub Bind_DDLHojasDeReferencia(ByVal miuser As Usuario)
        Dim tblHDR As New rastreo_template_hoja_de_ruta(cnn_str.CadenaDeConexion)
        With tblHDR
            .Where.Publico.Value = True
            .Where.Id_persona.Value = miuser.idPersona
            .Where.Id_usuarios.Conjuction = WhereParameter.Conj.Or
            .Where.Id_usuarios.Value = miuser.idUsuario
            If .Query.Load() Then
                ddlHojaDeReferencia_ptoref.DataSource = .DefaultView
                ddlHojaDeReferencia_ptoref.DataTextField = rastreo_template_hoja_de_ruta.ColumnNames.Descripcion_recorrido
                ddlHojaDeReferencia_ptoref.DataValueField = rastreo_template_hoja_de_ruta.ColumnNames.Id_recorridotemplate
                ddlHojaDeReferencia_ptoref.DataBind()
            End If
        End With
    End Sub

    Protected Sub Bind_DDLSendMailTo(ByVal miuser As Usuario)
        ddlSendMailTo.Items.Clear()
        Dim vehiculos As New rsview_vehiculos_asignados_a_usuarios(cnn_str.CadenaDeConexion)
        With vehiculos
            .Where.Activo.Value = True
            '.Where.Id_cliente.Value = miuser.idPersona
            .Where.Id_usuarios.Value = miuser.idUsuario
            If .Query.Load() Then
                If .RowCount > 0 Then
                    ddlSendMailTo.DataSource = .DefaultView
                    ddlSendMailTo.DataTextField = rsview_vehiculos_asignados_a_usuarios.ColumnNames.Identificador_rastreo
                    ddlSendMailTo.DataValueField = rsview_vehiculos_asignados_a_usuarios.ColumnNames.Id_vehiculo
                    ddlSendMailTo.DataBind()
                End If
            End If
        End With
    End Sub

    Protected Sub getPosiciones(ByVal mi_usuario_pos As Usuario)
        'Aqui tengo q buscar todos lo vehicuos del usuario y tirarlos en el txt, formato:
        'separacion de los items: TAB
        'point	title	description	icon
        '-2903050.588772871,-6382304.855050304	auto	texto	./App_Themes/CENTRAL/Imagenes/IconosTransporte/auto.png
        '-2903050.588752871,-6682304.855050304	camion	texto	./App_Themes/CENTRAL/Imagenes/IconosTransporte/camion.png
        '-2903050.588752871,-6882304.855050304	bus	texto	./App_Themes/CENTRAL/Imagenes/IconosTransporte/bus.png
        'guardar en un texto y asignarlo a txtfile.value, hiddencontrol
        'dependiendo del login
        If mi_usuario_pos Is Nothing Then
            Return
        End If
        Dim centerLAT As Double = 0
        Dim centerLON As Double = 0
        Dim tmpLONG As String = String.Empty
        Dim tmpLAT As String = String.Empty
        Dim posPOINT As String = String.Empty ' lat, long
        Dim posTITLE As String = String.Empty ' titulo - contraseña
        Dim posDESCRIPTION As String = String.Empty ' descripcion - informacion
        Dim posICON As String = String.Empty ' png
        Dim ViewVehiculos As New rsview_vehiculo_bandejaentrada_cliente_equipogps(cnn_str.CadenaDeConexion)
        Try
            With ViewVehiculos
                If mi_usuario_pos.Cliente Then
                    .Where.Id_usuarios.Value = idUsuario.Value
                    .Where.Visible.Value = True
                    .Query.Load()
                End If
                If .RowCount > 0 Then
                    If .s_Identificador_rastreo <> String.Empty Then
                        'Dim web_file As String = mi_usuario_pos.Usuario + mi_usuario_pos.IP.Replace(".", "") + ".pos"
                        Dim web_file As String = mi_usuario_pos.Usuario + _
                                                "_" + mi_usuario_pos.IP.Replace(".", "_").Replace(":", "_") + _
                                                "_" + Session.SessionID + ".txt"
                        'Now.ToString("_yyyyMMdd_HHmmss_") + _
                        Dim web_directory As String = "~/tmp/postxt/"
                        Dim web_filename As String = web_directory.Replace("~", ".") + web_file
                        Dim DirectorioTMP As String = MapPath(web_directory)
                        Dim nom_arch As String = DirectorioTMP + "\" + web_file
                        Dim web_directoryROOT As String = "~/tmp/"
                        Dim DirectorioTMPROOT As String = MapPath(web_directoryROOT)
                        If Not Directory.Exists(DirectorioTMPROOT) Then
                            Directory.CreateDirectory(DirectorioTMPROOT)
                        End If
                        If Not Directory.Exists(DirectorioTMP) Then
                            Directory.CreateDirectory(DirectorioTMP)
                        End If
                        Dim archivoPOS As New StreamWriter(New FileStream(nom_arch, FileMode.Create, FileAccess.ReadWrite))
                        SyncLock archivoPOS
                            archivoPOS.WriteLine("point" + Chr(9) + "title" + Chr(9) + _
                                                    "description" + Chr(9) + "icon" + Chr(9) + _
                                                    "iconSize" + Chr(9) + "iconOffset")
                            While Not .EOF
                                'If gv_Lista_de_Todos_los_Vehiculos.SelectedIndex >= 0 Then
                                '    If gv_Lista_de_Todos_los_Vehiculos.SelectedDataKey("identificador_rastreo") = .Identificador_rastreo Then
                                '        centerLAT = .Gps_latitud
                                '        centerLON = .Gps_longitud
                                '    End If
                                'End If
                                Try
                                    If .s_Gps_longitud <> String.Empty And .s_Gps_latitud <> String.Empty Then
                                        tmpLONG = Convert.ToString(.Gps_longitud).Replace(",", ".")
                                        tmpLAT = Convert.ToString(.Gps_latitud).Replace(",", ".")
                                    Else
                                        tmpLONG = "0"
                                        tmpLAT = "0"
                                    End If
                                Catch EX As Exception
                                    tmpLONG = "0"
                                    tmpLAT = "0"
                                End Try
                                posPOINT = tmpLAT + "," + tmpLONG
                                archivoPOS.WriteLine(posPOINT + Chr(9) + _
                                                   "" + _
                                                   Chr(9) + _
                                                   "" + _
                                                   Chr(9) + _
                                                   .s_Icono.Replace("~", ".") + _
                                                   Chr(9) + _
                                                   "24, 20" + _
                                                   Chr(9) + _
                                                   "-5, -14")
                                '"./img/marker-blue.png" + _
                                Dim WIDTH As Integer = .s_Identificador_rastreo.Length * 10
                                Dim HEIGHT As Integer = 16
                                Dim FECHA As String = String.Empty
                                If .s_Gps_fecha <> String.Empty Then
                                    FECHA = .Gps_fecha.ToString()
                                End If

                                Dim RefIngresar As String = String.Empty
                                If chkReferencias.Checked Then
                                    RefIngresar = "<tr><td colspan=""3"" style=""border:solid 1px grey;"">" + _
                                               "<input type=""button"" value=""Agregar posicion como referencia"" style=""font-size:x-small;width:100%;"" class=""rastreo_botones_save"" onclick=""setLONLAT_Historico(" + tmpLONG + "," + tmpLAT + ");"" /></td></tr>" + _
                                               "<tr><td style=""border:solid 1px grey;"">"
                                End If
                                archivoPOS.WriteLine(posPOINT + Chr(9) + _
                                                   .s_Identificador_rastreo + "<img width=""32px"" Height=""32px"" src=""" + .s_Icono.Replace("~", ".") + """>" + _
                                                   Chr(9) + _
                                                   "<div class=""rastreo_botones"">" + _
                                                   "<table width=""100%"" style=""font-size:smaller"" cellspacing=""0"" cellpadding=""0""><tr><td>" + _
                                                   RefIngresar + _
                                                   "Fecha y hora:<td/><td style=""border:solid 1px grey;"">" + FECHA + "<td/><tr/>" + _
                                                   "<tr><td style=""border:solid 1px grey;"">" + _
                                                   "Estado:<td/><td style=""border:solid 1px grey;"">" + .s_Evento + "<td/><tr/>" + _
                                                   "<tr><td style=""border:solid 1px grey;"">" + _
                                                   "Velocidad:<td/><td style=""border:solid 1px grey;"">" + .s_Gps_velocidad + "<td/><tr/>" + _
                                                   "<tr><td style=""border:solid 1px grey;"">" + _
                                                   "Rumbo:<td/><td style=""border:solid 1px grey;"">" + .s_Gps_rumbo + "° " + .s_Rumbo + "<td/><tr/>" + _
                                                   "<tr><td style=""border:solid 1px grey;"">" + _
                                                   "Matricula:<td/><td style=""border:solid 1px grey;"">" + .s_Matricula + "<td/></tr>" + _
                                                   "<tr><td style=""border:solid 1px grey;"">" + _
                                                   "Marca:<td/><td style=""border:solid 1px grey;"">" + .s_Marca + "<td/><tr/>" + _
                                                   "<tr><td style=""border:solid 1px grey;"">" + _
                                                   "Modelo:<td/><td style=""border:solid 1px grey;"">" + .s_Modelo + "<td/><tr/>" + _
                                                   "<tr><td style=""border:solid 1px grey;"" >" + _
                                                   "Año:<td/><td style=""border:solid 1px grey;"">" + .s_Anho + "<td/><tr/>" + _
                                                   "<tr><td style=""border:solid 1px grey;"">" + _
                                                   "Color:<td/><td style=""border:solid 1px grey;"">" + .s_Color + "<td/><tr/>" + _
                                                   "</table>" + _
                                                   "</div>" + _
                                                   Chr(9) + _
                                                   Utilidades.Texto_a_Imagen(.s_Identificador_rastreo, _
                                                   MapPath(Utilidades.web_directory_txtimgs), _
                                                   .s_Identificador_rastreo, _
                                                   ImageFormat.Png, "White", "Black", WIDTH, HEIGHT) + _
                                                   Chr(9) + _
                                                   WIDTH.ToString() + "," + HEIGHT.ToString() + _
                                                   Chr(9) + _
                                                   (-WIDTH / 2).ToString() + "," + (-(HEIGHT + 18)).ToString())
                                archivoPOS.Flush()
                                .MoveNext()
                            End While
                            archivoPOS.Write(vbCrLf)
                            archivoPOS.Flush()
                        End SyncLock
                        archivoPOS.Close()
                        txtfile.Value = web_filename ' + "?reload=1"
                        'If IsPostBack Then
                        '    If gv_Lista_de_Todos_los_Vehiculos.SelectedIndex >= 0 Then
                        '        Centrar_Mapa(centerLAT, centerLON)
                        '    End If
                        'End If
                    End If
                End If
            End With
        Catch ex As Exception
            ErrHandler.WriteError(ex.Message)
            Session.Add("msjerror", ex.ToString)
            Return
        End Try
    End Sub

    Public Function LoadReferencias(ByVal mi_usuario_pos As Usuario) As String
        If mi_usuario_pos Is Nothing Then
            Return String.Empty
        End If
        Dim tmpLONG As String = String.Empty
        Dim tmpLAT As String = String.Empty
        Dim posPOINT As String = String.Empty ' lat, long
        Dim posTITLE As String = String.Empty ' titulo - contraseña
        Dim posDESCRIPTION As String = String.Empty ' descripcion - informacion
        Dim posICON As String = String.Empty ' png
        Dim puntos_hojaderuta As New rastreo_template_puntos_hoja_de_ruta(cnn_str.CadenaDeConexion)
        Dim DirectorioTMP As String
        Dim nom_arch As String
        Try
            Dim web_file As String = mi_usuario_pos.Usuario + "_" + mi_usuario_pos.IP.Replace(".", "_").Replace(":", "_") + _
                        "_" + Session.SessionID + ".txt"
            Dim web_directory As String = "~/tmp/ref_postxt/"
            Dim web_filename As String = web_directory.Replace("~", ".") + web_file
            Dim hoja_template As New rastreo_template_hoja_de_ruta(cnn_str.CadenaDeConexion)
            DirectorioTMP = MapPath(web_directory)
            nom_arch = DirectorioTMP + "\" + web_file
            If Not Directory.Exists(DirectorioTMP) Then
                Directory.CreateDirectory(DirectorioTMP)
            End If
            Dim archivoPOS As New StreamWriter(New FileStream(nom_arch, FileMode.Create, FileAccess.ReadWrite))
            hoja_template.Where.Id_usuarios.Value = mi_usuario_pos.idUsuario
            hoja_template.Query.AddConjunction(WhereParameter.Conj.Or)
            hoja_template.Query.OpenParenthesis()
            hoja_template.Where.Id_persona.Value = miUsuario.idPersona
            hoja_template.Where.Publico.Value = True
            hoja_template.Query.CloseParenthesis()
            If hoja_template.Query.Load() Then
                If hoja_template.s_Id_recorridotemplate <> String.Empty Then
                    While Not hoja_template.EOF
                        With puntos_hojaderuta
                            .Where.Id_recorridotemplate.Value = hoja_template.Id_recorridotemplate
                            .Query.Load()
                            If .RowCount > 0 Then
                                SyncLock archivoPOS
                                    archivoPOS.WriteLine("point" + Chr(9) + "title" + Chr(9) + _
                                                            "description" + Chr(9) + "icon" + Chr(9) + _
                                                            "iconSize" + Chr(9) + "iconOffset")
                                    While Not .EOF
                                        tmpLONG = Convert.ToString(.Lon).Replace(",", ".")
                                        tmpLAT = Convert.ToString(.Lat).Replace(",", ".")
                                        posPOINT = tmpLAT + "," + tmpLONG
                                        archivoPOS.WriteLine(posPOINT + Chr(9) + _
                                                             .Nombre + _
                                                             Chr(9) + _
                                                             "<div class=""rastreo_botones"">" + _
                                                             "<table style=""font-size:smaller"" cellspacing=""0"" cellpadding=""0"">" + _
                                                             "<tr><td style=""border:solid 1px grey;"">" + _
                                                             "Descripcion:<td/><td style=""border:solid 1px grey;"">" + .Descripcion + "<td/><tr/>" + _
                                                             "<tr><td style=""border:solid 1px grey;"">" + _
                                                             "Metros de detección de referencia:<td/><td style=""border:solid 1px grey;"">" + .s_Metros + "<td/><tr/>" + _
                                                             "</table>" + _
                                                             "</div>" + _
                                                             Chr(9) + _
                                                             "./img/info.png" + _
                                                             Chr(9) + _
                                                             "15,  15" + _
                                                             Chr(9) + _
                                                             "-7, -7")
                                        archivoPOS.Flush()
                                        .MoveNext()
                                    End While
                                End SyncLock
                            End If
                        End With
                        hoja_template.MoveNext()
                    End While
                    archivoPOS.Close()
                    reffile.Value = web_filename '+ "?reload=1"
                    Return web_filename
                End If
            End If
            Return String.Empty
        Catch ex As Exception
            ErrHandler.WriteError(ex.Message)
            Session.Add("msjerror", ex.ToString)
            Return String.Empty
        End Try
    End Function

    Protected Sub tmr_Reload_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles tmr_Reload.Tick
        Dim sbScript As New StringBuilder()
        sbScript.Append("<script language='JavaScript' type='text/javascript'>" + ControlChars.Lf)
        sbScript.Append("//<!--" + ControlChars.Lf)
        sbScript.Append("countdown_timer(" & txtTimeout.Text & ");" & ControlChars.Lf)
        sbScript.Append("// -->" + ControlChars.Lf)
        sbScript.Append("</script>" + ControlChars.Lf)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Onload_updatecountdown", _
                                                              sbScript.ToString(), False)
    End Sub

    Private Sub Centrar_Mapa(ByVal lat As Double, ByVal lon As Double)
        Dim sbScript As New StringBuilder()
        sbScript.Append("<script language='JavaScript' type='text/javascript'>" + ControlChars.Lf)
        sbScript.Append("//<!--" + ControlChars.Lf)
        sbScript.Append("map_panTo(" + _
                                      lon.ToString().Replace(",", ".") + _
                                "," + lat.ToString().Replace(",", ".") + _
                                ");" + ControlChars.Lf)
        sbScript.Append("// -->" + ControlChars.Lf)
        sbScript.Append("</script>" + ControlChars.Lf)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Onload_g_setcentermap", _
                                                              sbScript.ToString(), False)
    End Sub

    Private Sub Reload_Data(Optional ByVal seguimiento As Boolean = False)
        'If Not seguimiento Then
        'sqldsTodosLosVehiculos.DataBind()
        'gv_Lista_de_Todos_los_Vehiculos.DataBind()
        If seguimiento Then
            sqldsVehiculoSeleccionado.DataBind()
            gridVehiculoSeleccionado.DataBind()
            gv_Lista_de_Todos_los_Vehiculos.DataSourceID = Nothing
            gv_Lista_de_Todos_los_Vehiculos.DataBind()
            gv_Lista_de_Todos_los_Vehiculos.EmptyDataText = "Para volver a ver los demas vehiculos, detenga el seguimiento actual."
            If gridVehiculoSeleccionado.Rows.Count > 0 Then
                gridVehiculoSeleccionado.SelectedIndex = 0
                Centrar_Mapa(gridVehiculoSeleccionado.SelectedDataKey("gps_latitud"), _
                             gridVehiculoSeleccionado.SelectedDataKey("gps_longitud"))
            End If
        Else
            If IsPostBack Then
                sqldsTodosLosVehiculos.DataBind()
                gv_Lista_de_Todos_los_Vehiculos.DataSourceID = "sqldsTodosLosVehiculos"
                gv_Lista_de_Todos_los_Vehiculos.DataBind()
                If Not seguimiento Then
                    gv_Lista_de_Todos_los_Vehiculos.EmptyDataText = "No tiene vehiculos asignados."
                Else
                    gv_Lista_de_Todos_los_Vehiculos.EmptyDataText = "-Realizando seguimiento-"
                End If
                idVehiculoSeleccionado.Value = "0"
                sqldsVehiculoSeleccionado.DataBind()
                gridVehiculoSeleccionado.DataBind()
            End If
        End If
        Dim sbScript As New StringBuilder()
        sbScript.Append("<script language='JavaScript' type='text/javascript'>" + ControlChars.Lf)
        sbScript.Append("//<!--" + ControlChars.Lf)
        sbScript.Append("g_UpdateMap(false);" + ControlChars.Lf)
        sbScript.Append("// -->" + ControlChars.Lf)
        sbScript.Append("</script>" + ControlChars.Lf)
        sbScript.Append("<script language='JavaScript' type='text/javascript'>" + ControlChars.Lf)
        sbScript.Append("//<!--" + ControlChars.Lf)
        sbScript.Append("g_UpdateRef(false);" + ControlChars.Lf)
        sbScript.Append("// -->" + ControlChars.Lf)
        sbScript.Append("</script>" + ControlChars.Lf)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Onload_center_reloaddata", _
                                                      sbScript.ToString(), False)
    End Sub

    'Protected Sub gv_Lista_de_Todos_los_Vehiculos_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_Lista_de_Todos_los_Vehiculos.RowCommand

    'End Sub

    Protected Sub gv_Lista_de_Todos_los_Vehiculos_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gv_Lista_de_Todos_los_Vehiculos.SelectedIndexChanged
        'If gv_Lista_de_Todos_los_Vehiculos.SelectedIndex >= 0 Then
        '    idVehiculoSeleccionado.Value = gv_Lista_de_Todos_los_Vehiculos.SelectedDataKey("id_vehiculo")
        '    If Session("old_selected_vehiculo_main") Is Nothing Then
        '        Session.Add("old_selected_vehiculo_main", gv_Lista_de_Todos_los_Vehiculos.SelectedDataKey("id_vehiculo"))
        '        Session.Add("msjerror", "Para volver a activar la actualizacion de los demas moviles, finalize el seguimiento actual.")
        '    Else
        '        If gv_Lista_de_Todos_los_Vehiculos.SelectedDataKey("id_vehiculo") = Session("old_selected_vehiculo_main") Then
        '            gv_Lista_de_Todos_los_Vehiculos.SelectedIndex = -1
        '            idVehiculoSeleccionado.Value = "0"
        '            Session("old_selected_vehiculo_main") = 0
        '        Else
        '            Session.Add("msjerror", "Para volver a activar la actualizacion de los demas moviles, finalize el seguimiento actual.")
        '            Session("old_selected_vehiculo_main") = gv_Lista_de_Todos_los_Vehiculos.SelectedDataKey("id_vehiculo")
        '            SetMapCenter()
        '        End If
        '    End If
        'Else
        '    Session("old_selected_vehiculo_main") = 0
        '    idVehiculoSeleccionado.Value = "0"
        'End If
        If gv_Lista_de_Todos_los_Vehiculos.SelectedIndex >= 0 And gv_Lista_de_Todos_los_Vehiculos.Rows.Count > 0 Then
            Session.Add("selected_vehiculo_main", gv_Lista_de_Todos_los_Vehiculos.SelectedDataKey("id_vehiculo"))
            gv_Lista_de_Todos_los_Vehiculos.SelectedIndex = -1
            'Reload_Data(True)
            tmr_Reload.Interval = 1
            gv_Lista_de_Todos_los_Vehiculos.EmptyDataText = "-Realizando seguimiento-"
            Session("TimeoutUpdate") = txtTimeout.Text
        Else
            txtTimeout.Text = Session("TimeoutUpdate")
        End If
    End Sub

    Public Function mainGetReferenciaCercana(ByVal vlat As String, ByVal vlon As String) As String
        mainGetReferenciaCercana = String.Empty
        If vlat <> String.Empty And vlon <> String.Empty Then
            Dim lat As Double = Double.Parse(vlat)
            Dim lon As Double = Double.Parse(vlon)
            mainGetReferenciaCercana = Utilidades.GetReferenciaCercana(miUsuario, lat, lon)
        End If
        Return mainGetReferenciaCercana
    End Function

    Public Function GeoPos(ByVal pLAT As String, ByVal pLON As String, Optional ByVal DIR As String = "") As String
        If Not String.IsNullOrEmpty(DIR) Then
            Return DIR
        End If
        Dim RET As String = "--"
        'If chkCalles.Checked Then
        If pLAT <> String.Empty And pLON <> String.Empty Then
            RET = Utilidades.mapserver_ReverseGeocode(pLAT, pLON)
        End If
        'End If
        Return RET
    End Function

    Protected Sub gridVehiculoSeleccionado_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gridVehiculoSeleccionado.RowCommand
        If e.CommandName = "selected_vehiculo_main" Then
            Session.Remove("selected_vehiculo_main")
            'txtTimeout.Text = "30"
            gv_Lista_de_Todos_los_Vehiculos.SelectedIndex = -1
            'Reload_Data()
            tmr_Reload.Interval = 1
        End If
    End Sub

    Protected Sub btnAgregarVehiculo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAgregarVehiculo.Click
        Try
            Dim myError As String = String.Empty
            For Each thisItem As ListItem In ddlSendMailTo.Items
                If thisItem.Selected Then
                    If Not listVehiculosInvolucrados.Items.FindByText(thisItem.Text) Is Nothing Then
                        myError += "El vehiculo " + thisItem.Text + " ya se encuentra en la lista" + vbCrLf
                        Continue For
                    End If
                    Session.Add("msjerror", myError)
                    Dim myItem As New ListItem(thisItem.Text, thisItem.Value)
                    listVehiculosInvolucrados.Items.Add(myItem)
                End If
            Next
        Catch MYEX As Exception
            ErrHandler.WriteError(MYEX.Message)
            Session.Add("msjerror", MYEX.Message + "<br>" + MYEX.StackTrace)
        End Try
    End Sub

    Protected Sub btnEliminarVehiculo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEliminarVehiculo.Click
        Try
            For Each thisItem As ListItem In listVehiculosInvolucrados.Items
                If thisItem.Selected Then
                    listVehiculosInvolucrados.Items.Remove(thisItem)
                End If
            Next
        Catch MYEX As Exception
            ErrHandler.WriteError(MYEX.Message)
            Session.Add("msjerror", MYEX.Message + "<br>" + MYEX.StackTrace)
        End Try
    End Sub

    Protected Sub btnSendMail_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSendMail.Click
        Enviar_Mails(miUsuario, txtMensajeCustom.Text)
        PopupMAIL.Hide()
    End Sub

    Private Function Enviar_Mails(ByVal miUser As Usuario, Optional ByVal MensajeCustom As String = "") As Boolean
        Enviar_Mails = False
        If Not String.IsNullOrEmpty(mailsSEND.Text) And listVehiculosInvolucrados.Items.Count > 0 Then
            Dim Correos As String()
            Dim datosVehiculos As New rsview_vehiculo_bandejaentrada_cliente_equipogps(cnn_str.CadenaDeConexion)
            Dim Resultados_de_Envio As String = String.Empty
            mailsSEND.Text = mailsSEND.Text.Trim().Replace(" ", ";").Replace(",", ";")
            Correos = mailsSEND.Text.Split(Convert.ToChar(";"))
            If Correos.Length > 0 Then
                For Each email As String In Correos
                    If Not String.IsNullOrEmpty(email) Then
                        For Each Vehiculo As ListItem In listVehiculosInvolucrados.Items
                            With datosVehiculos
                                .Where.Id_vehiculo.Value = Vehiculo.Value
                                .Where.Activo.Value = True
                                '.Where.Id_cliente.Value = miUser.idPersona
                                .Where.Id_usuarios.Value = miUser.idUsuario
                                If .Query.Load() Then
                                    Dim googlemaps_staticpos As String = _
                                    "http://maps.google.com/staticmap?" & _
                                        "center=" & .s_Gps_latitud.Replace(",", ".") & "," & .s_Gps_longitud.Replace(",", ".") & _
                                        "&zoom=" & g_zoom_value.Value & "" & _
                                        "&size=450x350" & _
                                        "&maptype=mobile" & _
                                        "&markers=" & .s_Gps_latitud.Replace(",", ".") & "," & .s_Gps_longitud.Replace(",", ".") & ",bluer" & _
                                        "&sensor=false" & _
                                        "&key=""" & RASTREAR._GOOGLE_MAPS_APIKEY_
                                    Dim mensaje As String = _
                                        "<a href=""" & googlemaps_staticpos & """>Ver imagen de posicion en ventana nueva</a><br />" & _
                                        "<iframe width=""425"" height=""350"" frameborder=""0"" scrolling=""no"" marginheight=""0"" marginwidth=""0"" " & _
                                        "src=""" & googlemaps_staticpos & """" & _
                                        "style=""color:#0000FF;text-align:left""></iframe><br />" & _
                                        "El vehiculo " & .Identificador_rastreo & " se localiza en " & _
                                        Utilidades.mapserver_ReverseGeocode(.s_Gps_latitud.Replace(",", "."), .s_Gps_longitud.Replace(",", ".")) & _
                                        "(" & .s_Gps_latitud.Replace(",", ".") & "," & .s_Gps_longitud.Replace(",", ".") & ")" & _
                                        " en estado de " & _
                                        "<label style=""color:white;background-color:" & .Color_evento & """>" & .Evento & "</label>" & _
                                        "<br />Velocidad: " & _
                                        IIf(.Evento.ToLowerInvariant().Contains("detenido") Or .Evento.ToLowerInvariant().Contains("apagado"), "0", .s_Gps_velocidad) & _
                                        "<br />Rumbo: " & .Gps_rumbo & "°" & .Rumbo
                                    If Not String.IsNullOrEmpty(Resultados_de_Envio) Then
                                        Resultados_de_Envio = Resultados_de_Envio & "<br />-"
                                    Else
                                        Resultados_de_Envio = Resultados_de_Envio & "-"
                                    End If
                                    Resultados_de_Envio = Resultados_de_Envio & _
                                        Utilidades.SendEmail(email, RASTREAR.g_MailSENDER, _
                                            "Posicion del Vehiculo " & .Identificador_rastreo, mensaje & vbCrLf & MensajeCustom, _
                                        RASTREAR.g_MailSERVER, RASTREAR.g_MailSTMPPORT, _
                                        RASTREAR.g_MailUSER, RASTREAR.g_MailUSERPWD, False) & _
                                        "Asunto: Posicion del Vehiculo " & .Identificador_rastreo
                                End If
                            End With
                        Next
                    End If
                Next
                Session.Add("msjerror", Resultados_de_Envio)
            Else
                Session.Add("msjerror", "Coloque direccion(es) validas de correo electronico a enviar.")
            End If
        End If
        Return Enviar_Mails
    End Function

    Protected Sub btnSAVEPTO_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSAVEPTO.Click
        If txtLAT.Text <> String.Empty And txtLON.Text <> String.Empty And _
            txtNombre_ptoref.Text <> String.Empty And txtDescripcion_ptoref.Text <> String.Empty Then
            Dim tblHojaRef As New rastreo_template_hoja_de_ruta(cnn_str.CadenaDeConexion)
            Dim tblPTOSHojaRef As New rastreo_template_puntos_hoja_de_ruta(cnn_str.CadenaDeConexion)
            Dim idhoja_de_referencia As Integer = 0
            If ddlHojaDeReferencia_ptoref.SelectedValue Is Nothing Then
                With tblHojaRef
                    .Where.Descripcion_recorrido.Value = "REFERENCIAS DE " + miUsuario.NombreUsuario
                    .Where.Id_usuarios.Value = miUsuario.idUsuario
                    .Where.Id_persona.Value = miUsuario.idPersona
                    If Not .Query.Load() Then
                        .AddNew()
                        .Descripcion_recorrido = "REFERENCIAS DE " + miUsuario.NombreUsuario
                        .Id_usuarios = miUsuario.idUsuario
                        .Id_persona = miUsuario.idPersona
                        .Publico = True
                        .Fech_ins = Now
                        .User_ins = miUsuario.idUsuario
                        .Save()
                    Else
                        idhoja_de_referencia = .Id_recorridotemplate
                    End If
                End With
            Else
                idhoja_de_referencia = Convert.ToInt32(ddlHojaDeReferencia_ptoref.SelectedValue)
                If idhoja_de_referencia > 0 Then
                    tblPTOSHojaRef.Where.Id_recorridotemplate.Value = idhoja_de_referencia
                    tblPTOSHojaRef.Where.Lat.Value = Convert.ToDouble(txtLAT.Text.Replace(".", ","))
                    tblPTOSHojaRef.Where.Lon.Value = Convert.ToDouble(txtLON.Text.Replace(".", ","))
                    If tblPTOSHojaRef.Query.Load() Then
                        'Que hacemos si ya existe el mismo exacto punto de lat y long?
                    Else
                        tblPTOSHojaRef.AddNew()
                        tblPTOSHojaRef.Lat = Convert.ToDouble(txtLAT.Text.Replace(".", ","))
                        tblPTOSHojaRef.Lon = Convert.ToDouble(txtLON.Text.Replace(".", ","))
                        tblPTOSHojaRef.Id_recorridotemplate = idhoja_de_referencia
                    End If
                    tblPTOSHojaRef.Nombre = txtNombre_ptoref.Text.ToUpperInvariant().Trim()
                    tblPTOSHojaRef.Descripcion = txtDescripcion_ptoref.Text.ToUpperInvariant().Trim()
                    If txtMETROS_REDONDA_ptorefref.Text.Trim() <> String.Empty Then
                        tblPTOSHojaRef.Metros = Convert.ToInt32(txtMETROS_REDONDA_ptorefref.Text.Trim())
                    Else
                        tblPTOSHojaRef.Metros = 100
                    End If
                    If chkReferencia_ptoref.Checked Then
                        tblPTOSHojaRef.Avisar_ingreso = True
                        tblPTOSHojaRef.Emails = txtMailReferencias_ptoref.Text.Replace(" ", ";").Replace(",", ";").Trim()
                    Else
                        tblPTOSHojaRef.Avisar_ingreso = False
                        tblPTOSHojaRef.SetColumnNull(rastreo_template_puntos_hoja_de_ruta.ColumnNames.Emails)
                    End If
                    tblPTOSHojaRef.Save()
                    'txtDescripcion_ptoref.Text = String.Empty
                    'txtLAT.Text = String.Empty
                    'txtLON.Text = String.Empty
                    'txtNombre_ptoref.Text = String.Empty
                    'txtMETROS_REDONDA_ptorefref.Text = String.Empty
                    'txtMailReferencias_ptoref.Text = String.Empty
                    Session.Add("msjerror", "Punto de referencia agregado con exito! Puede seguir agregando...")
                    'ShowMeTheError()
                End If
            End If
        End If
    End Sub

    Protected Sub btnCANCELPTO_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCANCELPTO.Click
        'chkReferencias.Checked = False
        'pnlPuntoReferencia.Visible = False
    End Sub

    Public Function GetStatusIO(ByVal IO As String, ByVal EdadDelDato As String, ByVal TipoDePosicion As String) As String
        GetStatusIO = String.Empty
        If IO <> String.Empty And EdadDelDato <> String.Empty And TipoDePosicion <> String.Empty Then
            Dim statusBIN As String = Utilidades.GetIO(IO)
            GetStatusIO = String.Empty
            GetStatusIO = GetStatusIO & "<table cellpadding='0' cellspacing='0' width='100%'>"
            GetStatusIO = GetStatusIO & "<tr><td>Ignición:</td><td>"
            GetStatusIO = GetStatusIO & IIf(statusBIN.Substring(0, 1) = "1", "<img src='App_Themes/CENTRAL/Imagenes/okidoki.png' />", "<img src='App_Themes/CENTRAL/Imagenes/process-stop.png' />")
            GetStatusIO = GetStatusIO & "</td>"
            GetStatusIO = GetStatusIO & "<td>Batería:</td><td>"
            GetStatusIO = GetStatusIO & IIf(statusBIN.Substring(1, 1) = "1", "<img src='App_Themes/CENTRAL/Imagenes/okidoki.png' />", "<img src='App_Themes/CENTRAL/Imagenes/process-stop.png' />")
            GetStatusIO = GetStatusIO & "</td></tr>"
            GetStatusIO = GetStatusIO & "<tr><td>Entrada 5:</td><td>"
            GetStatusIO = GetStatusIO & IIf(statusBIN.Substring(2, 1) = "1", "<img src='App_Themes/CENTRAL/Imagenes/okidoki.png' />", "<img src='App_Themes/CENTRAL/Imagenes/process-stop.png' />")
            GetStatusIO = GetStatusIO & "</td>"
            GetStatusIO = GetStatusIO & "<td>Entrada 4:</td><td>"
            GetStatusIO = GetStatusIO & IIf(statusBIN.Substring(3, 1) = "1", "<img src='App_Themes/CENTRAL/Imagenes/okidoki.png' />", "<img src='App_Themes/CENTRAL/Imagenes/process-stop.png' />")
            GetStatusIO = GetStatusIO & "</td></tr>"
            GetStatusIO = GetStatusIO & "<tr><td>Entrada 3:</td><td>"
            GetStatusIO = GetStatusIO & IIf(statusBIN.Substring(4, 1) = "1", "<img src='App_Themes/CENTRAL/Imagenes/okidoki.png' />", "<img src='App_Themes/CENTRAL/Imagenes/process-stop.png' />")
            GetStatusIO = GetStatusIO & "</td>"
            GetStatusIO = GetStatusIO & "<td>Entrada 2:</td><td>"
            GetStatusIO = GetStatusIO & IIf(statusBIN.Substring(5, 1) = "1", "<img src='App_Themes/CENTRAL/Imagenes/okidoki.png' />", "<img src='App_Themes/CENTRAL/Imagenes/process-stop.png' />")
            GetStatusIO = GetStatusIO & "</td></tr>"
            GetStatusIO = GetStatusIO & "<tr><td>Entrada 1:</td><td>"
            GetStatusIO = GetStatusIO & IIf(statusBIN.Substring(6, 1) = "1", "<img src='App_Themes/CENTRAL/Imagenes/okidoki.png' />", "<img src='App_Themes/CENTRAL/Imagenes/process-stop.png' />")
            GetStatusIO = GetStatusIO & "</td>"
            GetStatusIO = GetStatusIO & "<td>Entrada 0:</td><td>"
            GetStatusIO = GetStatusIO & IIf(statusBIN.Substring(7, 1) = "1", "<img src='App_Themes/CENTRAL/Imagenes/okidoki.png' />", "<img src='App_Themes/CENTRAL/Imagenes/process-stop.png' />")
            GetStatusIO = GetStatusIO & "</td></tr>"
            GetStatusIO = GetStatusIO & "</td>"
            Dim EDD As Integer = Integer.Parse(EdadDelDato, System.Globalization.NumberStyles.HexNumber)
            GetStatusIO = GetStatusIO & "<td>Edad del Dato: " & EDD.ToString() & "</td><td>"
            If EDD <= 85 Then
                GetStatusIO = GetStatusIO & "<img src='App_Themes/CENTRAL/Imagenes/okidoki.png' />"
            ElseIf EDD > 85 And EDD <= 170 Then
                GetStatusIO = GetStatusIO & "<img src='App_Themes/CENTRAL/Imagenes/edit-undo.png' />"
            ElseIf EDD > 170 Then
                GetStatusIO = GetStatusIO & "<img src='App_Themes/CENTRAL/Imagenes/process-stop.png' />"
            End If
            GetStatusIO = GetStatusIO & "<td>Posicion " & TipoDePosicion & "D</td><td>"
            Select Case (TipoDePosicion)
                Case "3"
                    GetStatusIO = GetStatusIO & "<img src='App_Themes/CENTRAL/Imagenes/okidoki.png' />"
                Case "2"
                    GetStatusIO = GetStatusIO & "<img src='App_Themes/CENTRAL/Imagenes/edit-undo.png' />"
                Case Else
                    GetStatusIO = GetStatusIO & "<img src='App_Themes/CENTRAL/Imagenes/process-stop.png' />"
            End Select
            GetStatusIO = GetStatusIO & "</td></tr>"
            GetStatusIO = GetStatusIO & "</table>"
        End If
        Return GetStatusIO
    End Function


    '<% If permiso_REPORTEAUTOMATICO Then%>
    '                            <asp:UpdatePanel runat="server" ID="updpnl_pref">
    '                                <ContentTemplate>                                        
    '                                    <asp:Button runat="server" ID="btnPreferencias" CssClass="rastreo_botones" Text="Reporte Automatico" />
    '                                    <ajaxToolkit:ModalPopupExtender ID="modalPopiSETTINGS" runat="server" BackgroundCssClass="modalBackground" TargetControlID="btnPreferencias" PopupControlID="pnlPrefs" CancelControlID="btnCloseSettings" />
    '                                    <asp:Panel CssClass="PopUp" ID="pnlPrefs" runat="server" Width="300px">
    '                                        <asp:UpdateProgress runat="server" ID="updprg_pref" AssociatedUpdatePanelID="updpnl_pref">
    '                                            <ProgressTemplate>
    '                                                <img src="./App_Themes/CENTRAL/Imagenes/ajax-loader.gif" alt="-"/>
    '                                                <label class="rastreo_nowloading">Aguarde...</label>
    '                                            </ProgressTemplate>
    '                                        </asp:UpdateProgress>
    '                                        <table width="100%" cellpadding="0" cellspacing="0" class="Label">
    '                                            <tr><td><asp:CheckBox runat="server" ID="chk_RA_OnOff" Text="Activar reportes automaticos" /></td></tr>
    '                                            <tr>
    '                                                <td>Hora de envio: 
    '                                                    <asp:TextBox runat="server" ID="txtHORA_ENVIO_REPORTE" CssClass="TextBox"></asp:TextBox>
    '       <%--<ajaxToolkit:MaskedEditExtender runat="server" InputDirection="LeftToRight" AcceptAMPM="false" UserTimeFormat="TwentyFourHour" ID="mymaskHour" TargetControlID="txtHORA_ENVIO_REPORTE" MaskType="Time" Mask="99:99"></ajaxToolkit:MaskedEditExtender>--%>
    '                                                </td>
    '                                            </tr>
    '                                            <tr>
    '                                                <td>
    '                                                    Ingrese los mail a donde desea que se envie reportes automaticos:
    '                                                    <asp:TextBox runat="server" ID="txtMailSettings" CssClass="TextBoxMAIL" TextMode="MultiLine" Width="90%" Height="100px" ToolTip="Puede ingresar sus mail separados por ',' o ':'"></asp:TextBox>
    '                                                </td>
    '                                            </tr>
    '                                            <tr>
    '                                                <td>
    '                                                    <asp:Button runat="server" ID="btnSaveSettings" Text="Guardar preferencias" CssClass="rastreo_botones_save"/><asp:Button  runat="server" ID="btnCloseSettings" Text="Cancelar" CssClass="rastreo_botones_cancel" />
    '                                                </td>
    '                                            </tr>
    '                                        </table>
    '                                    </asp:Panel>             
    '                                </ContentTemplate>
    '                            </asp:UpdatePanel>       
    '                    <% End If%>  
    '    Protected Sub btnSaveSettings_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveSettings.Click
    '        Dim rpersona As New rastreo_persona(cnn_str.CadenaDeConexion)
    '        With rpersona
    '            If (miUsuario.idPersona > 0) Then
    '                If .LoadByPrimaryKey(miUsuario.idPersona) Then
    '                    If txtMailSettings.Text.Trim() <> String.Empty Then
    '                        Dim rcliente As New rastreo_cliente(cnn_str.CadenaDeConexion)
    '                        If rcliente.LoadByPrimaryKey(miUsuario.idPersona) Then
    '                            rcliente.s_Ra_horaenvio = Now.ToString("dd/MM/yyyy ") + txtHORA_ENVIO_REPORTE.Text.Trim()
    '                            rcliente.Reporte_automatico = True
    '                            rcliente.Save()
    '                            .Email = txtMailSettings.Text.Trim().Replace(",", ";").Replace(" ", ";")
    '                            .Save()
    '                            txtMailSettings.Text = .Email
    '                        End If
    '                    End If
    '                End If
    '            End If
    '        End With
    '        modalPopiSETTINGS.Hide()
    '    End Sub

    Protected Sub btnAjustar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAjustar.Click

    End Sub

    Protected Sub btnSaveSettings_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveSettings.Click
        Dim tblCliente As New rastreo_cliente(cnn_str.CadenaDeConexion)
        Dim tblPersona As New rastreo_persona(cnn_str.CadenaDeConexion)
        Dim tblVehiculo As New rastreo_vehiculo(cnn_str.CadenaDeConexion) ' le agregue para la entidad vehiculo 07112010


        If tblCliente.LoadByPrimaryKey(miUsuario.idPersona) Then
            If tblPersona.LoadByPrimaryKey(miUsuario.idPersona) Then
                tblCliente.Ra_horaenvio = Convert.ToDateTime(Now.ToString("dd/MM/yyyy") + " " + txtHORA_ENVIO_REPORTE.Text)
                tblCliente.Reporte_automatico = chk_RA_OnOff.Checked

                tblVehiculo.Where.Id_cliente.Value = tblCliente.Id_cliente
                If tblVehiculo.Query.Load() Then
                    While Not tblVehiculo.EOF
                        'tblVehiculo.Ra_sgte_envio = Convert.ToDateTime(Now.ToString("dd/MM/yyyy"))
                        tblVehiculo.MoveNext()
                    End While
                End If
                tblVehiculo.Save()

                'Dim Emails() As String = txtMailSettings.Text.Replace(",", ";").Replace(" ", ";").Split(";")
                'For Each email As String In Emails
                tblPersona.s_Email = txtMailSettings.Text.Replace(",", ";").Replace(" ", ";")
                tblCliente.Save()
                tblPersona.Save()
            End If
        End If
        modalPopiSETTINGS.Hide()
    End Sub

    Protected Sub btnCloseSettings_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCloseSettings.Click
        modalPopiSETTINGS.Hide()
    End Sub

    Protected Sub btnPreferencias_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPreferencias.Click
        modalPopiSETTINGS.Show()
    End Sub


End Class