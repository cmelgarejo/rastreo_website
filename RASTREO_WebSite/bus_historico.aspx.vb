Imports RASTREOmw
Imports System.IO
Imports System.Data
Imports System.Drawing.Color
Imports System.Runtime.InteropServices
Imports MyGeneration.dOOdads
Imports Npgsql

Partial Class Historico
    Inherits System.Web.UI.Page

    Public Oki As Boolean = False
    Public miUsuario As Usuario
    Public mi_usuario As String = String.Empty
    Public permiso_menu_usuarios As Boolean
    Public vehiculo_seleccionado As String = String.Empty
    Dim idvehiculo As Integer = 0
    Dim idequipogps As Integer = 0
    Dim g_RowCount As Integer = 0
    Private g_rsview_eventos_buses As String = String.Empty

    Private Sub set_g_rsview_eventos_buses(ByVal gFecha As DateTime)
        'Dim loli As String = "reportesgps" & gFecha.Month.ToString() & gFecha.Year.ToString() & ""
        g_rsview_eventos_buses = _
"         SELECT DISTINCT ON (reportesgps" & gFecha.Month.ToString() & gFecha.Year.ToString() & ".gps_fechahora_reporte) rastreogps_tipoevento.idrastreogps_tipo_evento AS id_evento, " & _
"        CASE" & _
"            WHEN rastreogps_equipo_eventos.evento IS NOT NULL THEN rastreogps_equipo_eventos.evento" & _
"            WHEN rastreogps_equipo_eventos.evento IS NULL THEN rastreogps_tipoevento.descripcion::text" & _
"            WHEN rastreogps_tipoevento.descripcion IS NULL THEN reportesgps" & gFecha.Month.ToString() & gFecha.Year.ToString() & ".gps_evento::text" & _
"            ELSE NULL::text" & _
"        END AS evento, rastreogps_tipoevento.color AS color_evento, rastreogps_equipo_eventos.habilitado AS evento_habilitado, reportesgps" & gFecha.Month.ToString() & gFecha.Year.ToString() & ".idreportesgps" & gFecha.Month.ToString() & gFecha.Year.ToString() & ", reportesgps" & gFecha.Month.ToString() & gFecha.Year.ToString() & ".id_equipogps, reportesgps" & gFecha.Month.ToString() & gFecha.Year.ToString() & ".gps_longitud, reportesgps" & gFecha.Month.ToString() & gFecha.Year.ToString() & ".gps_latitud, reportesgps" & gFecha.Month.ToString() & gFecha.Year.ToString() & ".gps_fechahora_reporte, reportesgps" & gFecha.Month.ToString() & gFecha.Year.ToString() & ".gps_velocidad, reportesgps" & gFecha.Month.ToString() & gFecha.Year.ToString() & ".gps_rumbo, " & _
"        CASE" & _
"            WHEN reportesgps" & gFecha.Month.ToString() & gFecha.Year.ToString() & ".gps_rumbo >= 0 AND reportesgps" & gFecha.Month.ToString() & gFecha.Year.ToString() & ".gps_rumbo <= 23 OR reportesgps" & gFecha.Month.ToString() & gFecha.Year.ToString() & ".gps_rumbo >= 345 AND reportesgps" & gFecha.Month.ToString() & gFecha.Year.ToString() & ".gps_rumbo <= 360 THEN 'NORTE'::text" & _
"            WHEN reportesgps" & gFecha.Month.ToString() & gFecha.Year.ToString() & ".gps_rumbo > 23 AND reportesgps" & gFecha.Month.ToString() & gFecha.Year.ToString() & ".gps_rumbo <= 69 THEN 'NORESTE'::text" & _
"            WHEN reportesgps" & gFecha.Month.ToString() & gFecha.Year.ToString() & ".gps_rumbo > 69 AND reportesgps" & gFecha.Month.ToString() & gFecha.Year.ToString() & ".gps_rumbo <= 115 THEN 'ESTE'::text" & _
"            WHEN reportesgps" & gFecha.Month.ToString() & gFecha.Year.ToString() & ".gps_rumbo > 115 AND reportesgps" & gFecha.Month.ToString() & gFecha.Year.ToString() & ".gps_rumbo <= 161 THEN 'SURESTE'::text" & _
"            WHEN reportesgps" & gFecha.Month.ToString() & gFecha.Year.ToString() & ".gps_rumbo > 161 AND reportesgps" & gFecha.Month.ToString() & gFecha.Year.ToString() & ".gps_rumbo <= 207 THEN 'SUR'::text" & _
"            WHEN reportesgps" & gFecha.Month.ToString() & gFecha.Year.ToString() & ".gps_rumbo > 207 AND reportesgps" & gFecha.Month.ToString() & gFecha.Year.ToString() & ".gps_rumbo <= 253 THEN 'SUROESTE'::text" & _
"            WHEN reportesgps" & gFecha.Month.ToString() & gFecha.Year.ToString() & ".gps_rumbo > 253 AND reportesgps" & gFecha.Month.ToString() & gFecha.Year.ToString() & ".gps_rumbo <= 299 THEN 'OESTE'::text" & _
"            WHEN reportesgps" & gFecha.Month.ToString() & gFecha.Year.ToString() & ".gps_rumbo > 299 AND reportesgps" & gFecha.Month.ToString() & gFecha.Year.ToString() & ".gps_rumbo <= 345 THEN 'NOROESTE'::text" & _
"            ELSE NULL::text" & _
"        END AS rumbo, reportesgps" & gFecha.Month.ToString() & gFecha.Year.ToString() & ".gps_evento, reportesgps" & gFecha.Month.ToString() & gFecha.Year.ToString() & ".gps_edaddeldato, reportesgps" & gFecha.Month.ToString() & gFecha.Year.ToString() & ".gps_hdop, reportesgps" & gFecha.Month.ToString() & gFecha.Year.ToString() & ".gps_satstatus, reportesgps" & gFecha.Month.ToString() & gFecha.Year.ToString() & ".gps_gsmstatus, reportesgps" & gFecha.Month.ToString() & gFecha.Year.ToString() & ".gps_estado_io, reportesgps" & gFecha.Month.ToString() & gFecha.Year.ToString() & ".gps_tipodeposicion, reportesgps" & gFecha.Month.ToString() & gFecha.Year.ToString() & ".gps_ip, reportesgps" & gFecha.Month.ToString() & gFecha.Year.ToString() & ".gps_obs, rastreo_persona.idrastreo_persona AS idcliente, rastreo_vehiculo.identificador_rastreo, rastreo_vehiculo.idrastreo_vehiculo, rastreo_vehiculo.consumo_aprox, rastreogps_equipogps.idrastreogps_equipogps AS idequipogps, rastreogps_tipoequipo.idrastreogps_tipoequipo AS id_tipoequipogps, rastreogps_tipoequipo.tipo_equipo AS tipoequipogps, rastreogps_equipogps.id_equipo_gps, rastreogps_equipogps.nro_serie_gps" & _
"        FROM reportesgps" & gFecha.Month.ToString() & gFecha.Year.ToString() & _
"   LEFT JOIN rastreo_vehiculo ON reportesgps" & gFecha.Month.ToString() & gFecha.Year.ToString() & ".id_equipogps = rastreo_vehiculo.id_equipogps" & _
"   LEFT JOIN rastreo_persona ON rastreo_vehiculo.id_cliente = rastreo_persona.idrastreo_persona" & _
"   LEFT JOIN rastreo_cliente ON rastreo_vehiculo.id_cliente = rastreo_cliente.id_cliente" & _
"   LEFT JOIN rastreogps_equipogps ON rastreo_vehiculo.id_equipogps = rastreogps_equipogps.idrastreogps_equipogps" & _
"   LEFT JOIN rastreogps_tipoequipo ON rastreogps_equipogps.tipoequipo = rastreogps_tipoequipo.idrastreogps_tipoequipo" & _
"   LEFT JOIN rastreogps_tipoevento ON rastreogps_tipoevento.id_tipoequipo = rastreogps_tipoequipo.idrastreogps_tipoequipo AND rastreogps_tipoevento.evento::text = reportesgps" & gFecha.Month.ToString() & gFecha.Year.ToString() & ".gps_evento::text" & _
"   LEFT JOIN rastreogps_equipo_eventos ON rastreogps_equipo_eventos.id_equipogps = rastreogps_equipogps.idrastreogps_equipogps AND rastreogps_tipoevento.idrastreogps_tipo_evento = rastreogps_equipo_eventos.id_tipoevento" & _
"  WHERE rastreogps_equipo_eventos.habilitado = true AND (rastreogps_equipo_eventos.evento ~~* '%CONTROL%'::text OR rastreogps_equipo_eventos.evento ~~* '%LLE%GADA%'::text OR rastreogps_equipo_eventos.evento ~~* '%SALIDA%'::text) " & _
"  AND rastreo_vehiculo.idrastreo_vehiculo = @idVehiculoSeleccionado AND gps_fechahora_reporte BETWEEN @fecha_inicio AND @fecha_fin"
    End Sub

    '" OR (rastreogps_tipoevento.descripcion ILIKE '%CONTROL%' OR rastreogps_tipoevento.descripcion ILIKE '%LLEGADA%' OR rastreogps_tipoevento.descripcion ILIKE '%SALIDA%'))"
    Protected Overloads Overrides Sub Render(ByVal writer As HtmlTextWriter)
        Utilidades.CompresionASPX(HttpContext.Current)
        MyBase.Render(writer)
    End Sub

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        miUsuario = CType(Session("session_UsuarioRASTREO"), Usuario)
        If Not miUsuario Is Nothing Then
            'If Not miUsuario.Cliente Then
            'Response.Redirect("Principal.aspx", False)
            'End If
            mi_usuario = CType(Session("session_UsuarioRASTREO"), Usuario).NombreUsuario
            If Not Utilidades.FindPermissionFor(miUsuario.idUsuario, "OPCION_BUSES") Then
                Response.Redirect("Login.aspx?to=" + Now.ToString(), False)
                'Injeccion??? LOGUEAR!
            End If
        Else
            Response.Redirect("Login.aspx?to=" + Now.ToString(), False)
            'Return
        End If

        If Request.QueryString.Keys.Count > 0 Then
            If Not Request.QueryString("vid") Is Nothing Then
                If Integer.TryParse(Request.QueryString("vid"), idvehiculo) Then
                    idVehiculoSeleccionado.Value = idvehiculo
                    Dim tblV As New rastreo_vehiculo(cnn_str.CadenaDeConexion)
                    With tblV
                        .Where.Idrastreo_vehiculo.Value = idvehiculo
                        If .Query.Load() Then
                            If .s_Identificador_rastreo <> String.Empty Then
                                vehiculo_seleccionado = .Identificador_rastreo
                                idVehiculoSeleccionado.Value = idvehiculo
                                idequipogps = .Id_equipogps
                                Page.Title = "RASTREO Paraguay - Recorrido del vehiculo " + vehiculo_seleccionado
                            End If
                        End If
                    End With
                End If
            End If
        Else
            Response.Redirect("./Login.aspx?to=" + Now.ToString(), False)
            Return
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Session.Timeout = 60
        Oki = CType(ViewState("IniciadoHistorico"), Boolean)
        Dim sbScript As New StringBuilder()
        sbScript.Append("<script language='JavaScript' type='text/javascript'>" + ControlChars.Lf)
        sbScript.Append("<!--" + ControlChars.Lf)
        sbScript.Append("" + ControlChars.Lf)
        sbScript.Append("// -->" + ControlChars.Lf)
        sbScript.Append("</script>" + ControlChars.Lf)
        If Request.QueryString("vid") Is Nothing Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Onload_closewindow", _
                                                                  sbScript.ToString(), False)
            Return
        End If
        miUsuario = CType(Session("session_UsuarioRASTREO"), Usuario)
        If Not miUsuario Is Nothing Then
            If Not miUsuario.Cliente And Not miUsuario.Empleado Then
                Response.Redirect("Principal.aspx", False)
            End If
            mi_usuario = CType(Session("session_UsuarioRASTREO"), Usuario).Nombre
            If Utilidades.FindPermissionFor(miUsuario.idUsuario, "MENU_USUARIOS") Then
                permiso_menu_usuarios = True
            End If
        Else
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Onload_closewindow", _
                                                                  sbScript.ToString(), False)
            Return
        End If

        If Not IsPostBack Then
            'Session("sqldsVehiculoSeleccionado.SelectCommand") = Nothing
            If txtFECHAINICIO.Text = String.Empty Then
                txtFECHAINICIO.Text = Now.ToString("dd/MM/yyyy 00:00:00")
            End If
            If txtFECHAFIN.Text = String.Empty Then
                txtFECHAFIN.Text = Now.ToString("dd/MM/yyyy HH:mm:ss")
            End If
            txtFECHAINICIO.Focus()
        End If

        set_g_rsview_eventos_buses(Convert.ToDateTime(txtFECHAINICIO.Text))

        idVehiculoSeleccionado.Value = idvehiculo
        lbStatusOpciones.Text = "Inicio de recorrido: " + txtFECHAINICIO.Text + " Fin de recorrido: " + _
                                txtFECHAFIN.Text

        sqldsVehiculoSeleccionado.SelectCommand = g_rsview_eventos_buses
        sqldsVehiculoSeleccionado.DataBind()
        gridVehiculoSeleccionado.DataBind()

        'If Not Session("sqldsVehiculoSeleccionado.SelectCommand") Is Nothing And Oki Then
        '    sqldsVehiculoSeleccionado.SelectCommand = _
        '        Session("sqldsVehiculoSeleccionado.SelectCommand")
        'End If
    End Sub

    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        Try
            ShowMeTheError()
            If Not IsPostBack Then
                'ajax_popuphistorico.Show()
            End If
        Catch myEX As Exception
            ViewState.Add("msjerror", myEX.Message + "<br>" + myEX.StackTrace)
        End Try
    End Sub
    Private Sub ShowMeTheError()
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
    End Sub

    Protected Sub getpos_makeposfile(ByVal mi_usuario_pos As Usuario)
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
        Dim tmpLONG As String = String.Empty
        Dim tmpLAT As String = String.Empty
        Dim posPOINT As String = String.Empty ' lat, long
        Dim posTITLE As String = String.Empty ' titulo - contraseña
        Dim posDESCRIPTION As String = String.Empty ' descripcion - informacion
        Dim posICON As String = String.Empty ' png

        Dim web_file As String = mi_usuario_pos.Usuario + "_" + idVehiculoSeleccionado.Value + _
                "_" + mi_usuario_pos.IP.Replace(".", "_") + "_" + Session.SessionID + ".txt"
        'Dim web_file As String = mi_usuario_pos.Usuario + "_" + Session.SessionID + ".pos"
        Dim web_directory As String = "~/tmp/hispostxt/"
        Dim web_filename As String = web_directory.Replace("~", ".") + web_file
        Dim DirectorioTMP As String = MapPath(web_directory)
        Dim web_directoryROOT As String = "~/tmp/"
        Dim DirectorioTMPROOT As String = MapPath(web_directoryROOT)
        If Not Directory.Exists(DirectorioTMPROOT) Then
            Directory.CreateDirectory(DirectorioTMPROOT)
        End If
        If Not Directory.Exists(DirectorioTMP) Then
            Directory.CreateDirectory(DirectorioTMP)
        End If
        Dim nom_arch As String = DirectorioTMP + web_file
        Dim total_posiciones As Integer = 0
        Dim total_en_marcha As Long = 0
        Dim total_detenido As Long = 0
        Dim total_velocidad_promedio As Long = 0
        Dim total_tiempo_detenido As Integer = 0
        Dim total_tiempo_marcha As Integer = 0
        Dim velmin As Integer = 0
        Dim velmax As Integer = 0
        Dim oldlat As Double = 0
        Dim oldlon As Double = 0
        Dim total_km As Double = 0
        Dim consumoaprox As Double = 0

        Dim rastrear_conexion As New NpgsqlConnection(cnn_str.CadenaDeConexion)
        rastrear_conexion.Open()

        Try
            If txtFECHAINICIO.Text.Trim() <> String.Empty And _
                txtFECHAFIN.Text.Trim() <> String.Empty And _
                idVehiculoSeleccionado.Value <> "0" Then
                Dim sbScript As New StringBuilder()
                Dim rastrear_commando As New NpgsqlCommand(g_rsview_eventos_buses, rastrear_conexion)
                Dim rastrear_reader As New NpgsqlDataAdapter(rastrear_commando)
                Dim tRecorrido As New DataSet()

                rastrear_commando.Parameters.Add(New NpgsqlParameter("idVehiculoSeleccionado", idvehiculo))
                rastrear_commando.Parameters.Add(New NpgsqlParameter("fecha_inicio", txtFECHAINICIO.Text))
                rastrear_commando.Parameters.Add(New NpgsqlParameter("fecha_fin", txtFECHAFIN.Text))

                rastrear_reader.Fill(tRecorrido)

                With tRecorrido.Tables(0)
                    If Not Directory.Exists(DirectorioTMP) Then
                        Directory.CreateDirectory(DirectorioTMP)
                    End If
                    If File.Exists(nom_arch) Then
                        File.Delete(nom_arch)
                    End If
                    g_RowCount = .Rows.Count
                    Dim index As Integer = 0
                    If g_RowCount > 0 Then
                        If Convert.ToString(.Rows(index).Item("Consumo_aprox")) <> String.Empty Then
                            consumoaprox = .Rows(index).Item("Consumo_aprox")
                        Else
                            consumoaprox = 0
                        End If
                        Dim archivoPOS As New StreamWriter(New FileStream(nom_arch, FileMode.Create, FileAccess.ReadWrite))
                        Dim primer_pase As Boolean = True
                        SyncLock archivoPOS
                            archivoPOS.WriteLine("point" + Chr(9) + "title" + Chr(9) + _
                                                    "description" + Chr(9) + "icon" + Chr(9) + _
                                                    "iconSize" + Chr(9) + "iconOffset")
                            While index < g_RowCount
                                tmpLONG = Convert.ToString(.Rows(index).Item("Gps_longitud")).Replace(",", ".")
                                tmpLAT = Convert.ToString(.Rows(index).Item("Gps_latitud")).Replace(",", ".")
                                posPOINT = tmpLAT + "," + tmpLONG
                                Dim iconoevento As String = "./img/center_normal.gif"
                                If .Rows(index).Item("Evento") Is System.DBNull.Value Then
                                    If .Rows(index).Item("Evento").Contains("MARCHA") Or _
                                        .Rows(index).Item("Evento").Contains("IDA") Or _
                                        .Rows(index).Item("Evento").Contains("VUELTA") Then
                                        iconoevento = "./img/center_green.gif"
                                    ElseIf .Rows(index).Item("Evento").Contains("APAGADO") Or .Rows(index).Item("Evento").Contains("DETENIDO") Then
                                        iconoevento = "./img/center_red.gif"
                                    End If
                                End If
                                archivoPOS.WriteLine(posPOINT + _
                                                     Chr(9) + _
                                                     .Rows(index).Item("Identificador_rastreo") + _
                                                     Chr(9) + _
                                                     "<div class=""rastreo_botones"">" + _
                                                     "<input type=""button"" value=""Agregar como referencia"" style=""font-size:xx-small"" class=""rastreo_botones_save"" onclick=""setLONLAT_Historico(" + tmpLONG + "," + tmpLAT + ");"" />" + _
                                                     "<table style=""font-size:smaler"" cellspacing=""0"" cellpadding=""0""><tr><td>" + _
                                                     "<tr><td style=""border:solid 1px grey;"">" + _
                                                     "Fecha y hora de posicion:<td/><td style=""border:solid 1px grey;"">" + Convert.ToString(.Rows(index).Item("Gps_fechahora_reporte")) + "<td/><tr/>" + _
                                                     "<tr><td style=""border:solid 1px grey;"">" + _
                                                     "Estado:<td/><td style=""border:solid 1px grey;"">" + .Rows(index).Item("Evento") + "<td/><tr/>" + _
                                                     "<tr><td style=""border:solid 1px grey;"">" + _
                                                     "Velocidad:<td/><td style=""border:solid 1px grey;"">" + Convert.ToString(.Rows(index).Item("Gps_velocidad")) + "<td/><tr/>" + _
                                                     "<tr><td style=""border:solid 1px grey;"">" + _
                                                     "Rumbo:<td/><td style=""border:solid 1px grey;"">" + Convert.ToString(.Rows(index).Item("Gps_rumbo")) + "° " + .Rows(index).Item("Rumbo") + "<td/><tr/>" + _
                                                     "</table>" + _
                                                     "</div>" + _
                                                     Chr(9) + _
                                                     iconoevento + _
                                                     Chr(9) + _
                                                     "15, 15" + _
                                                     Chr(9) + _
                                                     "-1, -1")
                                archivoPOS.Flush()
                                If primer_pase Then
                                    primer_pase = False
                                    velmin = .Rows(index).Item("Gps_velocidad")
                                    velmax = velmin
                                    oldlat = .Rows(index).Item("Gps_latitud")
                                    oldlon = .Rows(index).Item("Gps_longitud")
                                    sbScript.Append("<script language='JavaScript' type='text/javascript'>" + ControlChars.Lf)
                                    sbScript.Append("<!--" + ControlChars.Lf)
                                    sbScript.Append("map_panTo(" + _
                                                    tmpLONG + "," + tmpLAT + _
                                                        ");" + ControlChars.Lf)
                                    sbScript.Append("// -->" + ControlChars.Lf)
                                    sbScript.Append("</script>" + ControlChars.Lf)
                                End If


                                If .Rows(index).Item("Evento").Contains("DETENIDO") Then
                                    total_detenido += 1
                                ElseIf .Rows(index).Item("Evento").Contains("MARCHA") Then
                                    total_en_marcha += 1
                                End If

                                total_posiciones += 1

                                If .Rows(index).Item("Gps_velocidad") > velmax Then
                                    velmax = .Rows(index).Item("Gps_velocidad")
                                End If
                                If .Rows(index).Item("Gps_velocidad") < velmin Then
                                    velmin = .Rows(index).Item("Gps_velocidad")
                                End If

                                total_velocidad_promedio += .Rows(index).Item("Gps_velocidad")

                                total_km += _
                                Utilidades.dblCalcular_distancia_entre_dos_puntos_KM(oldlat, oldlon, _
                                .Rows(index).Item("Gps_latitud"), .Rows(index).Item("Gps_longitud"))
                                oldlat = .Rows(index).Item("Gps_latitud")
                                oldlon = .Rows(index).Item("Gps_longitud")

                                index += 1
                            End While
                        End SyncLock
                        archivoPOS.Close()
                        txtfile.Value = web_filename
                        ViewState.Add("txtfile", web_filename)
                        txtTOTPOS.Text = total_posiciones.ToString()

                        index = 0
                        ''Para el calculo de tiempo en marcha
                        .DefaultView.RowFilter = "Evento LIKE '%MARCHA%'"
                        Dim FiRsT As Boolean = True
                        If .DefaultView.Count > 2 Then
                            Dim oldmov As DateTime
                            For Each ROW As DataRowView In .DefaultView
                                If Not FiRsT Then
                                    If DateDiff(DateInterval.Second, oldmov, ROW("gps_fechahora_reporte")) < 60 Then
                                        total_tiempo_marcha += _
                                                DateDiff(DateInterval.Second, oldmov, ROW("gps_fechahora_reporte"))
                                    End If
                                    oldmov = ROW("gps_fechahora_reporte")
                                Else
                                    oldmov = ROW("gps_fechahora_reporte")
                                    FiRsT = False
                                End If
                            Next
                        End If
                        ''Tiempo en marcha
                        ''Descomposicion de segundos en dias, horas, minutos y segundos
                        Dim strmarcha As String = String.Empty
                        If total_tiempo_marcha > 86400 Then
                            strmarcha += (total_tiempo_marcha \ 86400).ToString() + " Dia(s),"
                        End If
                        If total_tiempo_marcha > 3600 Then
                            strmarcha += ((total_tiempo_marcha Mod 86400) \ 3600).ToString() + " Horas(s),"
                        End If
                        If total_tiempo_marcha > 60 Then
                            strmarcha += ((((total_tiempo_marcha Mod 86400)) Mod 3600) \ 60).ToString() + " Minutos(s),"
                        End If
                        If total_tiempo_marcha > 0 Then
                            strmarcha += ((((total_tiempo_marcha Mod 86400)) Mod 3600) Mod 60).ToString() + " Segundos."
                        End If

                        ''Para el calculo de tiempo detenido
                        .DefaultView.RowFilter = "Evento LIKE '%DETEN%'"
                        FiRsT = True
                        If .DefaultView.Count > 2 Then
                            Dim oldmov As DateTime
                            For Each ROW As DataRowView In .DefaultView
                                If Not FiRsT Then
                                    If DateDiff(DateInterval.Minute, oldmov, ROW("gps_fechahora_reporte")) <= 5 Then
                                        total_tiempo_detenido += _
                                                DateDiff(DateInterval.Second, oldmov, ROW("gps_fechahora_reporte"))
                                    End If
                                    oldmov = ROW("gps_fechahora_reporte")
                                Else
                                    oldmov = ROW("gps_fechahora_reporte")
                                    FiRsT = False
                                End If
                            Next
                        End If
                        ''Tiempo detenido
                        ''Descomposicion de segundos en dias, horas, minutos y segundos
                        Dim strdetenido As String = String.Empty
                        If total_tiempo_detenido > 86400 Then
                            strdetenido += (total_tiempo_detenido \ 86400).ToString() + " Dia(s),"
                        End If
                        If total_tiempo_detenido > 3600 Then
                            strdetenido += ((total_tiempo_detenido Mod 86400) \ 3600).ToString() + " Horas(s),"
                        End If
                        If total_tiempo_detenido > 60 Then
                            strdetenido += ((((total_tiempo_detenido Mod 86400)) Mod 3600) \ 60).ToString() + " Minutos(s),"
                        End If
                        If total_tiempo_detenido > 0 Then
                            strdetenido += ((((total_tiempo_detenido Mod 86400)) Mod 3600) Mod 60).ToString() + " Segundos."
                        End If

                        If total_velocidad_promedio > 0 Then
                            txtVELPROM.Text = Convert.ToInt32(total_velocidad_promedio / _
                                                              total_posiciones).ToString() + _
                                                " Km/h."
                        Else
                            txtVELPROM.Text = "0 Km/h."
                        End If

                        If velmin > 0 Then
                            txtVELMIN.Text = velmin.ToString() + " Km/h."
                        Else
                            txtVELMIN.Text = "0 Km/h."
                        End If

                        If velmax > 0 Then
                            txtVELMAX.Text = velmax.ToString() + " Km/h."
                        Else
                            txtVELMAX.Text = "0 Km/h."
                        End If

                        txtTOTENMOV.Text = total_en_marcha.ToString() + " - Tiempo: " + strmarcha
                        txtTOTDETENIDO.Text = total_detenido.ToString() + " - Tiempo: " + strdetenido
                        txtTOTKM.Text = total_km.ToString("F2") + " Km."
                        If consumoaprox > 0 Then
                            txtCONSUMO.Text = ((consumoaprox * total_km) / 100).ToString("F2") + " lts."
                        Else
                            txtCONSUMO.Text = "[No se ha configurado el consumo de combustible del vehiculo.]"
                        End If

                    End If
                    sbScript.Append("<script language='JavaScript' type='text/javascript'>" + ControlChars.Lf)
                    sbScript.Append("<!--" + ControlChars.Lf)
                    sbScript.Append("g_UpdateMap(false);" + ControlChars.Lf)
                    sbScript.Append("// -->" + ControlChars.Lf)
                    sbScript.Append("</script>" + ControlChars.Lf)
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Onload_mymapshit", _
                                                              sbScript.ToString(), False)
                End With
            End If
        Catch ex As Exception
            Session.Add("msjerror", ex.ToString)
            Return
        Finally
            'GC.Collect()
        End Try
    End Sub

    Protected Sub btnIniciarHistorico_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnIniciarHistorico.Click
        ViewState.Add("IniciadoHistorico", True)
        Oki = True
        Session("sqldsVehiculoSeleccionado.SelectCommand") = Nothing
        Limpiar_campos()
        'Calcular_descarga_recorrido()
        'ajax_popuphistorico.Hide()
        'Reload_Data()
    End Sub

    Private Sub Limpiar_campos()
        txtTOTDETENIDO.Text = String.Empty
        txtTOTENMOV.Text = String.Empty
        txtTOTKM.Text = String.Empty
        txtTOTPOS.Text = String.Empty
        txtVELPROM.Text = String.Empty
    End Sub

    Private Sub Reload_Data()
        getpos_makeposfile(miUsuario)
        If Oki Then
            sqldsVehiculoSeleccionado.SelectCommand = g_rsview_eventos_buses
            sqldsVehiculoSeleccionado.DataBind()
            gridVehiculoSeleccionado.DataBind()
            'ViewState.Remove("IniciadoHistorico")
        End If
        gridVehiculoSeleccionado.PageSize = 20000
        Session("sqldsVehiculoSeleccionado.SelectCommand") = sqldsVehiculoSeleccionado.SelectCommand
        lbStatusOpciones.Text = "Inicio de recorrido: " + txtFECHAINICIO.Text + " Fin de recorrido: " + _
                                txtFECHAFIN.Text
        If ViewState("txtfile") <> String.Empty Then
            txtfile.Value = ViewState("txtfile")
        End If
    End Sub

    Protected Sub btnDismiss_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDismiss.Click
        'ajax_popuphistorico.Hide()
    End Sub

    Protected Sub btnCalcular_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCalcular.Click
        Calcular_descarga_recorrido()
    End Sub

    Private Function Calcular_descarga_recorrido() As Integer
        Calcular_descarga_recorrido = 0
        Dim rastrear_conexion As New NpgsqlConnection(cnn_str.CadenaDeConexion)
        rastrear_conexion.Open()
        Dim SQL_count_rows As String = _
            g_rsview_eventos_buses
        Dim rastrear_commando As New NpgsqlCommand(SQL_count_rows, rastrear_conexion)
        Dim rastrear_reader As New NpgsqlDataAdapter(rastrear_commando)
        Dim rastrear_dataset As New DataSet()
        rastrear_commando.CommandTimeout = 120
        rastrear_commando.Parameters.Add(New NpgsqlParameter("idVehiculoSeleccionado", idvehiculo))
        rastrear_commando.Parameters.Add(New NpgsqlParameter("fecha_inicio", txtFECHAINICIO.Text))
        rastrear_commando.Parameters.Add(New NpgsqlParameter("fecha_fin", txtFECHAFIN.Text))
        rastrear_reader.Fill(rastrear_dataset)
        'MsgBox(rastrear_dataset.Tables(0).Rows.Count)
        Dim conteo As Integer = Integer.Parse(rastrear_dataset.Tables(0).Rows(0)(0))
        lbCalculo.Text = conteo.ToString() + " puntos de posicion, demorará " + Tiempo_en_minutos(conteo)
        Calcular_descarga_recorrido = rastrear_dataset.Tables(0).Rows.Count
    End Function

    Private Function Tiempo_en_minutos(ByVal rows As Integer) As String
        If rows > 0 Then
            Dim tempo As String = String.Empty
            Dim cant_pto_tiempo As Integer = rows '3 segundos para obtener direccion?
            Dim minutos As Integer = 0
            Dim segundos As Integer = 0
            minutos = cant_pto_tiempo \ 60
            segundos = cant_pto_tiempo - (minutos * 60)
            tempo = minutos.ToString() + " minutos " + segundos.ToString() + " segundos; aproximadamente..."
            Tiempo_en_minutos = tempo
        Else
            Tiempo_en_minutos = "0 segundos."
        End If
    End Function

    Protected Sub rblHoras_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rblHoras.SelectedIndexChanged
        rblDias.SelectedIndex = -1
        rblSemanas.SelectedIndex = -1
        rblMeses.SelectedIndex = -1
        txtFECHAINICIO.Text = Now.AddHours(-rblHoras.SelectedValue).ToString("dd/MM/yyyy HH:mm:ss")
        txtFECHAFIN.Text = Now.ToString("dd/MM/yyyy HH:mm:ss")
        Calcular_descarga_recorrido()
    End Sub
    Protected Sub rblDias_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rblDias.SelectedIndexChanged
        rblHoras.SelectedIndex = -1
        rblSemanas.SelectedIndex = -1
        rblMeses.SelectedIndex = -1
        txtFECHAINICIO.Text = Now.AddDays(-rblDias.SelectedValue).ToString("dd/MM/yyyy HH:mm:ss")
        txtFECHAFIN.Text = Now.ToString("dd/MM/yyyy HH:mm:ss")
        Calcular_descarga_recorrido()
    End Sub
    Protected Sub rblSemanas_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rblSemanas.SelectedIndexChanged
        rblHoras.SelectedIndex = -1
        rblDias.SelectedIndex = -1
        rblMeses.SelectedIndex = -1
        txtFECHAINICIO.Text = Now.AddDays(-rblSemanas.SelectedValue).ToString("dd/MM/yyyy HH:mm:ss")
        txtFECHAFIN.Text = Now.ToString("dd/MM/yyyy HH:mm:ss")
        Calcular_descarga_recorrido()
    End Sub

    Protected Sub rblMeses_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rblMeses.SelectedIndexChanged
        rblHoras.SelectedIndex = -1
        rblDias.SelectedIndex = -1
        rblSemanas.SelectedIndex = -1
        txtFECHAINICIO.Text = Now.AddMonths(-rblMeses.SelectedValue).ToString("dd/MM/yyyy HH:mm:ss")
        txtFECHAFIN.Text = Now.ToString("dd/MM/yyyy HH:mm:ss")
        Calcular_descarga_recorrido()
    End Sub

    Protected Sub btnExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExcel.Click
        'Dim recorrido As New rsview_recorridos(cnn_str.CadenaDeConexion)
        'With recorrido
        '.Where.Evento_habilitado.Value = True
        '.Where.Id_vehiculo.Value = idvehiculo
        '.Where.Gps_fechahora_reporte.Operator = WhereParameter.Operand.Between
        '.Where.Gps_fechahora_reporte.BetweenBeginValue = Convert.ToDateTime(txtFECHAINICIO.Text.Trim()).ToString("dd/MM/yyyy HH:mm:ss")
        '.Where.Gps_fechahora_reporte.BetweenEndValue = Convert.ToDateTime(txtFECHAFIN.Text.Trim()).ToString("dd/MM/yyyy HH:mm:ss")
        'If Not String.IsNullOrEmpty(ddlFiltroEventos.SelectedValue) Then
        '    .Where.Gps_evento.Value = ddlFiltroEventos.SelectedItem.Value
        'End If
        'If Not String.IsNullOrEmpty(txtSpdINI.Text) And Not String.IsNullOrEmpty(txtSpdFIN.Text) Then
        '    .Where.Gps_velocidad.Operator = MyGeneration.dOOdads.WhereParameter.Operand.Between
        '    .Where.Gps_velocidad.BetweenBeginValue = CInt(txtSpdINI.Text)
        '    .Where.Gps_velocidad.BetweenEndValue = CInt(txtSpdFIN.Text)
        'End If
        'End with
        ExportExcel()

    End Sub

    Protected Sub gridVehiculoSeleccionado_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridVehiculoSeleccionado.DataBound
        ViewState.Remove("IniciadoHistorico")
    End Sub

    Public Overloads Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)

    End Sub

    Public Sub ExportExcel()

        Dim sb As New StringBuilder()
        Dim sw As New StringWriter(sb)
        Dim htw As New HtmlTextWriter(sw)

        htw.Flush()
        lbStatusOpciones.RenderControl(htw)
        gridVehiculoSeleccionado.RenderControl(htw)
        pnlResultados.RenderControl(htw)

        HttpContext.Current.Response.Clear()
        HttpContext.Current.Response.Buffer = True
        HttpContext.Current.Response.ContentType = "application/vnd.ms-excel"
        HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=recorrido_" & vehiculo_seleccionado & Now.ToString("_ddMMyyy_HHmm_") & ".xls")
        HttpContext.Current.Response.Charset = "UTF-8"
        HttpContext.Current.Response.ContentEncoding = Encoding.Default '  New System.Text.UTF8Encoding(False)
        HttpContext.Current.Response.Write(sb.ToString())
        HttpContext.Current.Response.End()

    End Sub

    Public Function gGetReferenciaCercana(ByVal vlat As Double, ByVal vlon As Double) As String
        Return Utilidades.GetReferenciaCercana(miUsuario, vlat, vlon)
    End Function

    Public Function GeoPos(ByVal pLAT As String, ByVal pLON As String) As String
        Dim RET As String = ""
        'If Oki Then
        'If chkCalles.Checked Then
        RET = Utilidades.mapserver_ReverseGeocode(pLAT, pLON)
        'End If
        'End If
        Return RET
    End Function

    'Protected Sub ajax_popuphistorico_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles ajax_popuphistorico.Load
    '    ViewState.Remove("IniciadoHistorico")
    'End Sub

End Class
