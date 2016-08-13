Imports RASTREOmw
Imports System.IO
Imports System.Data
Imports System.Drawing.Color
Imports System.Drawing.Imaging
Imports System.Runtime.InteropServices
Imports MyGeneration.dOOdads
Imports Npgsql
Imports System.Web.UI.DataVisualization.Charting
Imports System.Xml
Imports System.Text

Partial Class Historico
    Inherits System.Web.UI.Page

    Dim Indexex As Integer = 0
    Public Oki As Boolean = False
    Public miUsuario As Usuario
    Public mi_usuario As String = String.Empty
    Public permiso_menu_usuarios As Boolean
    Public vehiculo_seleccionado As String = String.Empty
    Dim idvehiculo As Integer = 0
    Dim idequipogps As Integer = 0
    Dim g_RowCount As Integer = 0
    Public filtroRefOnly As String = String.Empty
    Private g_selectcommand As String = String.Empty

    Private Sub setSQLCMD()
        Try
            g_selectcommand = _
                " SELECT DISTINCT ON (reportesgps" + Convert.ToDateTime(txtFECHAINICIO.Value).Month.ToString() + Convert.ToDateTime(txtFECHAINICIO.Value).Year.ToString() & ".gps_fechahora_reporte) " & _
                "	rastreogps_tipoevento.idrastreogps_tipo_evento as id_evento," & _
                "	CASE " & _
                "       WHEN rastreogps_equipo_eventos.evento IS NULL THEN " & _
                "        rastreogps_tipoevento.descripcion " & _
                "	    WHEN rastreogps_tipoevento.descripcion IS NULL THEN " & _
                "        reportesgps" + Convert.ToDateTime(txtFECHAINICIO.Value).Month.ToString() + Convert.ToDateTime(txtFECHAINICIO.Value).Year.ToString() & ".Gps_evento " & _
                "	    WHEN rastreogps_equipo_eventos.evento IS NOT NULL THEN" & _
                "        rastreogps_equipo_eventos.evento " & _
                "	END as evento," & _
                "	rastreogps_tipoevento.color as color_evento," & _
                "	rastreogps_equipo_eventos.habilitado as evento_habilitado," & _
                "	reportesgps" + Convert.ToDateTime(txtFECHAINICIO.Value).Month.ToString() + Convert.ToDateTime(txtFECHAINICIO.Value).Year.ToString() & ".idreportesgps" + Convert.ToDateTime(txtFECHAINICIO.Value).Month.ToString() + Convert.ToDateTime(txtFECHAINICIO.Value).Year.ToString() & ", reportesgps" + Convert.ToDateTime(txtFECHAINICIO.Value).Month.ToString() + Convert.ToDateTime(txtFECHAINICIO.Value).Year.ToString() & ".id_equipogps, " & _
                "	reportesgps" + Convert.ToDateTime(txtFECHAINICIO.Value).Month.ToString() + Convert.ToDateTime(txtFECHAINICIO.Value).Year.ToString() & ".gps_longitud, reportesgps" + Convert.ToDateTime(txtFECHAINICIO.Value).Month.ToString() + Convert.ToDateTime(txtFECHAINICIO.Value).Year.ToString() & ".gps_latitud, " & _
                "	reportesgps" + Convert.ToDateTime(txtFECHAINICIO.Value).Month.ToString() + Convert.ToDateTime(txtFECHAINICIO.Value).Year.ToString() & ".gps_fechahora_reporte, reportesgps" + Convert.ToDateTime(txtFECHAINICIO.Value).Month.ToString() + Convert.ToDateTime(txtFECHAINICIO.Value).Year.ToString() & ".gps_velocidad," & _
                "	reportesgps" + Convert.ToDateTime(txtFECHAINICIO.Value).Month.ToString() + Convert.ToDateTime(txtFECHAINICIO.Value).Year.ToString() & ".gps_rumbo, " & _
                "	CASE" & _
                "	    WHEN (reportesgps" + Convert.ToDateTime(txtFECHAINICIO.Value).Month.ToString() + Convert.ToDateTime(txtFECHAINICIO.Value).Year.ToString() & ".gps_rumbo >= 0   AND reportesgps" + Convert.ToDateTime(txtFECHAINICIO.Value).Month.ToString() + Convert.ToDateTime(txtFECHAINICIO.Value).Year.ToString() & ".gps_rumbo <= 23 ) OR" & _
                "		 (reportesgps" + Convert.ToDateTime(txtFECHAINICIO.Value).Month.ToString() + Convert.ToDateTime(txtFECHAINICIO.Value).Year.ToString() & ".gps_rumbo >= 345 AND reportesgps" + Convert.ToDateTime(txtFECHAINICIO.Value).Month.ToString() + Convert.ToDateTime(txtFECHAINICIO.Value).Year.ToString() & ".gps_rumbo <= 360) THEN " & _
                "		('NORTE'::text)" & _
                "	    WHEN reportesgps" + Convert.ToDateTime(txtFECHAINICIO.Value).Month.ToString() + Convert.ToDateTime(txtFECHAINICIO.Value).Year.ToString() & ".gps_rumbo > 23 AND reportesgps" + Convert.ToDateTime(txtFECHAINICIO.Value).Month.ToString() + Convert.ToDateTime(txtFECHAINICIO.Value).Year.ToString() & ".gps_rumbo <= 69 THEN " & _
                "		('NORESTE'::text)" & _
                "	    WHEN reportesgps" + Convert.ToDateTime(txtFECHAINICIO.Value).Month.ToString() + Convert.ToDateTime(txtFECHAINICIO.Value).Year.ToString() & ".gps_rumbo > 69 AND reportesgps" + Convert.ToDateTime(txtFECHAINICIO.Value).Month.ToString() + Convert.ToDateTime(txtFECHAINICIO.Value).Year.ToString() & ".gps_rumbo <= 115 THEN " & _
                "	    ('ESTE'::text)" & _
                "	    WHEN reportesgps" + Convert.ToDateTime(txtFECHAINICIO.Value).Month.ToString() + Convert.ToDateTime(txtFECHAINICIO.Value).Year.ToString() & ".gps_rumbo > 115 AND reportesgps" + Convert.ToDateTime(txtFECHAINICIO.Value).Month.ToString() + Convert.ToDateTime(txtFECHAINICIO.Value).Year.ToString() & ".gps_rumbo <= 161 THEN " & _
                "	    ('SURESTE'::text)" & _
                "	    WHEN reportesgps" + Convert.ToDateTime(txtFECHAINICIO.Value).Month.ToString() + Convert.ToDateTime(txtFECHAINICIO.Value).Year.ToString() & ".gps_rumbo > 161 AND reportesgps" + Convert.ToDateTime(txtFECHAINICIO.Value).Month.ToString() + Convert.ToDateTime(txtFECHAINICIO.Value).Year.ToString() & ".gps_rumbo <= 207 THEN " & _
                "	    ('SUR'::text)" & _
                "	    WHEN reportesgps" + Convert.ToDateTime(txtFECHAINICIO.Value).Month.ToString() + Convert.ToDateTime(txtFECHAINICIO.Value).Year.ToString() & ".gps_rumbo > 207 AND reportesgps" + Convert.ToDateTime(txtFECHAINICIO.Value).Month.ToString() + Convert.ToDateTime(txtFECHAINICIO.Value).Year.ToString() & ".gps_rumbo <= 253 THEN " & _
                "	    ('SUROESTE'::text)" & _
                "	    WHEN reportesgps" + Convert.ToDateTime(txtFECHAINICIO.Value).Month.ToString() + Convert.ToDateTime(txtFECHAINICIO.Value).Year.ToString() & ".gps_rumbo > 253 AND reportesgps" + Convert.ToDateTime(txtFECHAINICIO.Value).Month.ToString() + Convert.ToDateTime(txtFECHAINICIO.Value).Year.ToString() & ".gps_rumbo <= 299 THEN " & _
                "	    ('OESTE'::text)	" & _
                "	    WHEN reportesgps" + Convert.ToDateTime(txtFECHAINICIO.Value).Month.ToString() + Convert.ToDateTime(txtFECHAINICIO.Value).Year.ToString() & ".gps_rumbo > 299 AND reportesgps" + Convert.ToDateTime(txtFECHAINICIO.Value).Month.ToString() + Convert.ToDateTime(txtFECHAINICIO.Value).Year.ToString() & ".gps_rumbo <= 345 THEN " & _
                "	    ('NOROESTE'::text)" & _
                "	END as rumbo, " & _
                "	reportesgps" + Convert.ToDateTime(txtFECHAINICIO.Value).Month.ToString() + Convert.ToDateTime(txtFECHAINICIO.Value).Year.ToString() & ".gps_evento," & _
                "	reportesgps" + Convert.ToDateTime(txtFECHAINICIO.Value).Month.ToString() + Convert.ToDateTime(txtFECHAINICIO.Value).Year.ToString() & ".gps_edaddeldato, reportesgps" + Convert.ToDateTime(txtFECHAINICIO.Value).Month.ToString() + Convert.ToDateTime(txtFECHAINICIO.Value).Year.ToString() & ".gps_hdop, " & _
                "	reportesgps" + Convert.ToDateTime(txtFECHAINICIO.Value).Month.ToString() + Convert.ToDateTime(txtFECHAINICIO.Value).Year.ToString() & ".gps_satstatus, reportesgps" + Convert.ToDateTime(txtFECHAINICIO.Value).Month.ToString() + Convert.ToDateTime(txtFECHAINICIO.Value).Year.ToString() & ".gps_gsmstatus, " & _
                "	reportesgps" + Convert.ToDateTime(txtFECHAINICIO.Value).Month.ToString() + Convert.ToDateTime(txtFECHAINICIO.Value).Year.ToString() & ".gps_estado_io, reportesgps" + Convert.ToDateTime(txtFECHAINICIO.Value).Month.ToString() + Convert.ToDateTime(txtFECHAINICIO.Value).Year.ToString() & ".gps_tipodeposicion, " & _
                "	reportesgps" + Convert.ToDateTime(txtFECHAINICIO.Value).Month.ToString() + Convert.ToDateTime(txtFECHAINICIO.Value).Year.ToString() & ".gps_ip, reportesgps" + Convert.ToDateTime(txtFECHAINICIO.Value).Month.ToString() + Convert.ToDateTime(txtFECHAINICIO.Value).Year.ToString() & ".gps_obs," & _
                "	reportesgps" + Convert.ToDateTime(txtFECHAINICIO.Value).Month.ToString() + Convert.ToDateTime(txtFECHAINICIO.Value).Year.ToString() & ".gps_ip, reportesgps" + Convert.ToDateTime(txtFECHAINICIO.Value).Month.ToString() + Convert.ToDateTime(txtFECHAINICIO.Value).Year.ToString() & ".gps_dir," & _
                "   rastreo_persona.idrastreo_persona AS idcliente, " & _
                "   rastreo_vehiculo.identificador_rastreo, rastreo_vehiculo.idrastreo_vehiculo AS id_vehiculo, " & _
                "   rastreo_vehiculo.consumo_aprox, " & _
                "   rastreogps_equipogps.idrastreogps_equipogps AS idequipogps, " & _
                "   rastreogps_tipoequipo.idrastreogps_tipoequipo AS id_tipoequipogps, rastreogps_tipoequipo.tipo_equipo AS tipoequipogps, " & _
                "   rastreogps_equipogps.id_equipo_gps, " & _
                "   rastreogps_equipogps.Nro_serie_gps, rastreogps_tipoequipo.tipo_de_reporte as tipo_de_reporte" & _
                "   FROM reportesgps" + Convert.ToDateTime(txtFECHAINICIO.Value).Month.ToString() + Convert.ToDateTime(txtFECHAINICIO.Value).Year.ToString() & " " & _
                " LEFT JOIN rastreo_vehiculo 			ON reportesgps" + Convert.ToDateTime(txtFECHAINICIO.Value).Month.ToString() + Convert.ToDateTime(txtFECHAINICIO.Value).Year.ToString() & ".id_equipogps  = rastreo_vehiculo.id_equipogps " & _
                " LEFT JOIN rastreo_persona 			ON rastreo_vehiculo.id_cliente       = rastreo_persona.idrastreo_persona " & _
                " LEFT JOIN rastreo_cliente 			ON rastreo_vehiculo.id_cliente = rastreo_cliente.id_cliente " & _
                " LEFT JOIN rastreogps_equipogps 		ON rastreo_vehiculo.id_equipogps = rastreogps_equipogps.idrastreogps_equipogps " & _
                " LEFT JOIN rastreogps_tipoequipo 		ON rastreogps_equipogps.tipoequipo = rastreogps_tipoequipo.idrastreogps_tipoequipo " & _
                " LEFT JOIN rastreogps_tipoevento ON rastreogps_tipoevento.id_tipoequipo = rastreogps_tipoequipo.idrastreogps_tipoequipo " & _
                "                               AND rastreogps_tipoevento.evento = reportesgps" + Convert.ToDateTime(txtFECHAINICIO.Value).Month.ToString() + Convert.ToDateTime(txtFECHAINICIO.Value).Year.ToString() & ".gps_evento " & _
                " LEFT JOIN rastreogps_equipo_eventos ON rastreogps_equipo_eventos.id_equipogps = rastreogps_equipogps.idrastreogps_equipogps " & _
                "				AND rastreogps_tipoevento.idrastreogps_tipo_evento = rastreogps_equipo_eventos.id_tipoevento "

        Catch ex As Exception

        End Try
    End Sub

    'Protected Overloads Overrides Sub Render(ByVal writer As HtmlTextWriter)
    '    Utilidades.CompresionASPX(HttpContext.Current)
    '    MyBase.Render(writer)
    'End Sub

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        miUsuario = CType(Session("session_UsuarioRASTREO"), Usuario)
        If Not miUsuario Is Nothing Then
            'If Not miUsuario.Cliente Then
            'Response.Redirect("Principal.aspx", False)
            'End If
            mi_usuario = CType(Session("session_UsuarioRASTREO"), Usuario).NombreUsuario
        Else
            'Response.Redirect("./Login.aspx?to=" + Now.ToString(), False)
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

        Dim sbScript As New StringBuilder()
        sbScript.Append("<script language='JavaScript' type='text/javascript'>" + ControlChars.Lf)
        sbScript.Append("<!--" + ControlChars.Lf)
        'sbScript.Append("window.close();" + ControlChars.Lf)
        sbScript.Append("// -->" + ControlChars.Lf)
        sbScript.Append("</script>" + ControlChars.Lf)
        If Request.QueryString("vid") Is Nothing Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Onload_closewindow", _
                                                                  sbScript.ToString(), False)
            Return
        End If

        'Session.Timeout = 60
        If gridVehiculoSeleccionado.Rows.Count > 0 Then
            'MsgBox(Now())
            Session.Add("msjerror", "")
        End If

        Oki = CType(ViewState("IniciadoHistorico"), Boolean)
        
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

        idVehiculoSeleccionado.Value = idvehiculo
        If Not IsPostBack Then
            For mes As Integer = 1 To 12
                Dim itemes As New ListItem
                Select Case mes
                    Case 1
                        itemes.Text = "Enero"
                    Case 2
                        itemes.Text = "Febrero"
                    Case 3
                        itemes.Text = "Marzo"
                    Case 4
                        itemes.Text = "Abril"
                    Case 5
                        itemes.Text = "Mayo"
                    Case 6
                        itemes.Text = "Junio"
                    Case 7
                        itemes.Text = "Julio"
                    Case 8
                        itemes.Text = "Agosto"
                    Case 9
                        itemes.Text = "Setiembre"
                    Case 10
                        itemes.Text = "Octubre"
                    Case 11
                        itemes.Text = "Noviembre"
                    Case 12
                        itemes.Text = "Diciembre"
                End Select
                itemes.Value = mes
                ddlMonth.Items.Add(itemes)
            Next
            ddlMonth.SelectedValue = Now.Month
            Dim itemyear(1) As ListItem
            itemyear(0) = New ListItem(Now.Year.ToString(), Now.Year.ToString())
            itemyear(1) = New ListItem(Now.AddYears(-1).Year.ToString(), Now.AddYears(-1).Year.ToString())
            ddlYear.Items.AddRange(itemyear)
            txtDiaInicio.Text = Now.Day.ToString()
            txtDiaFin.Text = Now.Day.ToString()
            txtHoraInicio.Text = Now.AddHours(-1).Hour.ToString()
            txtHoraFin.Text = Now.Hour.ToString()
            txtMinutoInicio.Text = Now.Minute.ToString()
            txtMinutoFin.Text = Now.Minute.ToString()
            Session("sqldsVehiculoSeleccionado.SelectCommand") = Nothing
        End If
        NUD1.Maximum = DateTime.DaysInMonth(ddlYear.SelectedValue, ddlMonth.SelectedValue)
        NUD2.Maximum = DateTime.DaysInMonth(ddlYear.SelectedValue, ddlMonth.SelectedValue)
        If Convert.ToInt32(txtDiaInicio.Text) > DateTime.DaysInMonth(ddlYear.SelectedValue, ddlMonth.SelectedValue) Then
            txtDiaInicio.Text = DateTime.DaysInMonth(ddlYear.SelectedValue, ddlMonth.SelectedValue)
        End If
        If Convert.ToInt32(txtDiaFin.Text) > DateTime.DaysInMonth(ddlYear.SelectedValue, ddlMonth.SelectedValue) Then
            txtDiaFin.Text = DateTime.DaysInMonth(ddlYear.SelectedValue, ddlMonth.SelectedValue)
        End If
        'txtFECHAINICIO.Value = Convert.ToDateTime(txtDiaInicio.Text.PadLeft(2, "0") + "/" + ddlMonth.SelectedValue().PadLeft(2, "0") + "/" + ddlYear.SelectedValue + " " + txtHoraInicio.Text.PadLeft(2, "0") + ":" + txtMinutoInicio.Text.PadLeft(2, "0"))
        txtFECHAINICIO.Value = Convert.ToDateTime(ddlYear.SelectedValue + "-" + ddlMonth.SelectedValue().PadLeft(2, "0") + "-" + txtDiaInicio.Text.PadLeft(2, "0") + " " + txtHoraInicio.Text.PadLeft(2, "0") + ":" + txtMinutoInicio.Text.PadLeft(2, "0"))
        'txtFECHAFIN.Value = Convert.ToDateTime(txtDiaFin.Text.PadLeft(2, "0") + "/" + ddlMonth.SelectedValue().PadLeft(2, "0") + "/" + ddlYear.SelectedValue + " " + txtHoraFin.Text.PadLeft(2, "0") + ":" + txtMinutoFin.Text.PadLeft(2, "0"))
        txtFECHAFIN.Value = Convert.ToDateTime(ddlYear.SelectedValue + "-" + ddlMonth.SelectedValue().PadLeft(2, "0") + "-" + txtDiaFin.Text.PadLeft(2, "0") + " " + txtHoraFin.Text.PadLeft(2, "0") + ":" + txtMinutoFin.Text.PadLeft(2, "0"))
        setSQLCMD()

        If Not IsPostBack Then
            Calcular_descarga_recorrido()
        End If

        If Not Session("sqldsVehiculoSeleccionado.SelectCommand") Is Nothing And Oki Then
            sqldsVehiculoSeleccionado.SelectCommand = _
                Session("sqldsVehiculoSeleccionado.SelectCommand")
        End If

        'If pnlPuntoReferencia.Visible Then
        Bind_DDLHojasDeReferencia(miUsuario)
        'End If

        lbStatusOpciones.Text = "Inicio de recorrido: " + txtFECHAINICIO.Value + " Fin de recorrido: " + _
                        txtFECHAFIN.Value

    End Sub

    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        Try
            ShowMeTheError()
            If Not IsPostBack Then
                ajax_popuphistorico.Show()
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

    Protected Function getRecorridoPOS(ByVal mi_usuario_pos As Usuario, _
                                     Optional ByVal evento As String = "SIN EVENTO", _
                                     Optional ByVal spdini As String = "", _
                                     Optional ByVal spdfin As String = "") As String
        'Aqui tengo q buscar todos lo vehicuos del usuario y tirarlos en el txt, formato:
        'separacion de los items: TAB
        'point	title	description	icon
        '-2903050.588772871,-6382304.855050304	auto	texto	./App_Themes/CENTRAL/Imagenes/IconosTransporte/auto.png
        '-2903050.588752871,-6682304.855050304	camion	texto	./App_Themes/CENTRAL/Imagenes/IconosTransporte/camion.png
        '-2903050.588752871,-6882304.855050304	bus	texto	./App_Themes/CENTRAL/Imagenes/IconosTransporte/bus.png
        'guardar en un texto y asignarlo a txtfile.value, hiddencontrol
        'dependiendo del login
        getRecorridoPOS = String.Empty
        If mi_usuario_pos Is Nothing Then
            Return getRecorridoPOS
        End If
        Dim tmpLONG As String = String.Empty
        Dim tmpLAT As String = String.Empty
        Dim posPOINT As String = String.Empty ' lat, long
        Dim posTITLE As String = String.Empty ' titulo - contraseña
        Dim posDESCRIPTION As String = String.Empty ' descripcion - informacion
        Dim posICON As String = String.Empty ' png

        Dim web_file As String = mi_usuario_pos.Usuario + "_" + idVehiculoSeleccionado.Value + _
                "_" + mi_usuario_pos.IP.Replace(".", "_").Replace(":", "_") + "_" + Session.SessionID + ".txt"
        'Dim web_file As String = mi_usuario_pos.Usuario + "_" + Session.SessionID + ".pos"
        Dim web_directory As String = "~/tmp/hispostxt/"
        Dim web_filename As String = web_directory.Replace("~", ".") + web_file
        Dim DirectorioTMP As String = MapPath(web_directory)
        Dim nom_arch As String = DirectorioTMP + web_file
        Dim web_directoryROOT As String = "~/tmp/"
        Dim DirectorioTMPROOT As String = MapPath(web_directoryROOT)
        If Not Directory.Exists(DirectorioTMPROOT) Then
            Directory.CreateDirectory(DirectorioTMPROOT)
        End If
        If Not Directory.Exists(DirectorioTMP) Then
            Directory.CreateDirectory(DirectorioTMP)
        End If
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
        Dim precioGslt As Integer = 0

        Dim comienzo_stop As Boolean = False
        Dim Old_idreportegps As String = String.Empty
        Dim sql_where_reportes As String = String.Empty
        Dim rastrear_conexion As New NpgsqlConnection(cnn_str.CadenaDeConexion)
        rastrear_conexion.Open()

        '' Create new data series and set it's visual attributes
        Dim serieVelocidad As New Series("Velocidad")
        serieVelocidad.ChartType = SeriesChartType.Range
        serieVelocidad.XValueType = ChartValueType.DateTime
        serieVelocidad.BorderWidth = 3
        serieVelocidad.ShadowOffset = 2

        Try
            If Convert.ToDateTime(txtFECHAINICIO.Value).ToString("yyyy-MM-dd HH:mm:ss") <> String.Empty And _
               Convert.ToDateTime(txtFECHAFIN.Value).ToString("yyyy-MM-dd HH:mm:ss") <> String.Empty And _
                idVehiculoSeleccionado.Value <> "0" Then
                Dim sbScript As New StringBuilder()
                sql_where_reportes = " WHERE Idrastreo_vehiculo = " & idVehiculoSeleccionado.Value & _
                " AND Gps_fechahora_reporte BETWEEN '" & Convert.ToDateTime(txtFECHAINICIO.Value).ToString("yyyy-MM-dd HH:mm:ss") & _
                "' AND '" & Convert.ToDateTime(txtFECHAFIN.Value).ToString("yyyy-MM-dd HH:mm:ss") & "' "
                If evento <> "SIN EVENTO" Then
                    sql_where_reportes += " AND Gps_evento = '" & evento & "' "
                End If
                If spdini <> "" And spdfin <> "" Then
                    sql_where_reportes += " AND (Gps_velocidad BETWEEN " & spdini & " AND " & spdfin & ") "
                End If
                sql_where_reportes = sql_where_reportes + " AND gps_latitud <> 0 AND gps_longitud <> 0 "
                Dim rastrear_commando As New NpgsqlCommand(g_selectcommand + sql_where_reportes, rastrear_conexion)
                rastrear_commando.CommandTimeout = 120
                Dim rastrear_reader As New NpgsqlDataAdapter(rastrear_commando)
                Dim tRecorrido As New DataSet()
                rastrear_reader.Fill(tRecorrido)

                With tRecorrido.Tables(0)
                    If Not Directory.Exists(DirectorioTMP) Then
                        Directory.CreateDirectory(DirectorioTMP)
                    End If
                    If File.Exists(nom_arch) Then
                        File.Delete(nom_arch)
                    End If
                    g_RowCount = .Rows.Count
                    Indexex = 0
                    Dim EventoAnterior As String = String.Empty
                    If g_RowCount > 0 Then
                        If Convert.ToString(.Rows(Indexex).Item("Consumo_aprox")) <> String.Empty Then
                            consumoaprox = .Rows(Indexex).Item("Consumo_aprox")
                        Else
                            consumoaprox = 0
                        End If
                        Dim archivoPOS As New StreamWriter(New FileStream(nom_arch, FileMode.Create, FileAccess.ReadWrite))
                        Dim primer_pase As Boolean = True
                        'Static MothaFucka As String = "LOLFUCK"
                        'SyncLock MothaFucka
                        archivoPOS.WriteLine("point" + Chr(9) + "title" + Chr(9) + _
                                                "description" + Chr(9) + "icon" + Chr(9) + _
                                                "iconSize" + Chr(9) + "iconOffset")
                        Dim conta_paradas As Integer = 0
                        Dim oldconta As Integer = 0

                        Dim firstEvento As Boolean = False

                        While Indexex < g_RowCount

                            tmpLONG = Convert.ToString(.Rows(Indexex).Item("Gps_longitud")).Replace(",", ".")
                            tmpLAT = Convert.ToString(.Rows(Indexex).Item("Gps_latitud")).Replace(",", ".")
                            posPOINT = tmpLAT + "," + tmpLONG
                            Dim puede_insertar As Boolean = True

                            If chkRefOnly.Checked Then
                                If gGetReferenciaCercana(Double.Parse(.Rows(Indexex).Item("Gps_latitud")), Double.Parse(.Rows(Indexex).Item("Gps_longitud"))).Contains("NINGUNA") Then
                                    If Indexex + 1 < g_RowCount Then
                                        Indexex += 1
                                        Continue While
                                    Else
                                        Indexex = g_RowCount
                                        Exit While
                                    End If
                                Else
                                    getRecorridoPOS = getRecorridoPOS & Convert.ToString(.Rows(Indexex).Item("idreportesgps" + Convert.ToDateTime(txtFECHAINICIO.Value).Month.ToString() + Convert.ToDateTime(txtFECHAINICIO.Value).Year.ToString())) & ","
                                    If Not .Rows(Indexex).Item("Evento") Is DBNull.Value Then
                                        If String.IsNullOrEmpty(EventoAnterior) Or EventoAnterior <> .Rows(Indexex).Item("Evento") Then
                                            EventoAnterior = .Rows(Indexex).Item("Evento")
                                            firstEvento = True
                                        ElseIf EventoAnterior = .Rows(Indexex).Item("Evento") Then
                                            'Indexex += 1
                                            'Continue While
                                            firstEvento = False
                                        End If
                                    End If
                                    If Not firstEvento Then
                                        Indexex += 1
                                        Continue While
                                    End If
                                End If
                            Else
                                If (chkResumenParada.Checked) Then
                                    If Convert.ToString(.Rows(Indexex).Item("Evento")).ToUpperInvariant().Contains("DETENIDO") Then
                                        Old_idreportegps = Convert.ToString(.Rows(Indexex).Item("idreportesgps" + Convert.ToDateTime(txtFECHAINICIO.Value).Month.ToString() + Convert.ToDateTime(txtFECHAINICIO.Value).Year.ToString()))
                                        If Indexex + 1 = g_RowCount Then
                                            getRecorridoPOS = getRecorridoPOS & Old_idreportegps & ","
                                        End If
                                        If comienzo_stop = False Then
                                            conta_paradas += 1
                                            comienzo_stop = True
                                        Else
                                            puede_insertar = False
                                        End If
                                    Else
                                        comienzo_stop = False
                                    End If
                                End If
                                If puede_insertar Then
                                    If Not comienzo_stop And puede_insertar Then
                                        'Old_idreportegps = String.Empty
                                    End If
                                    If Not String.IsNullOrEmpty(Old_idreportegps) Then
                                        getRecorridoPOS = getRecorridoPOS & Old_idreportegps & ","
                                    End If
                                    getRecorridoPOS = getRecorridoPOS & Convert.ToString(.Rows(Indexex).Item("idreportesgps" + Convert.ToDateTime(txtFECHAINICIO.Value).Month.ToString() + Convert.ToDateTime(txtFECHAINICIO.Value).Year.ToString())) & ","
                                End If
                            End If
                            Old_idreportegps = Convert.ToString(.Rows(Indexex).Item("idreportesgps" + Convert.ToDateTime(txtFECHAINICIO.Value).Month.ToString() + Convert.ToDateTime(txtFECHAINICIO.Value).Year.ToString()))

                            Dim iconoevento As String = "./img/center_normal.gif"
                            If Not .Rows(Indexex).Item("Evento") Is DBNull.Value Then
                                If .Rows(Indexex).Item("Evento").Contains("MARCHA") Or _
                                    .Rows(Indexex).Item("Evento").Contains("IDA") Or _
                                    .Rows(Indexex).Item("Evento").Contains("VUELTA") Then
                                    'iconoevento = "./img/center_green.gif"
                                    iconoevento = "./img/" & .Rows(Indexex).Item("Rumbo") & ".png"
                                ElseIf .Rows(Indexex).Item("Evento").Contains("APAGADO") Or .Rows(Indexex).Item("Evento").Contains("DETENIDO") Then
                                    iconoevento = "./img/stop_red.png"
                                ElseIf .Rows(Indexex).Item("Evento").Contains("DERECHA") Or .Rows(Indexex).Item("Evento").Contains("IZQ") Then
                                    If .Rows(Indexex).Item("Rumbo") <> String.Empty Then
                                        If .Rows(Indexex).Item("Evento").Contains("DERECHA") Then
                                            iconoevento = "./img/vder" & Convert.ToString(.Rows(Indexex).Item("Rumbo")).ToLowerInvariant() & ".png"
                                        ElseIf .Rows(Indexex).Item("Evento").Contains("IZQ") Then
                                            iconoevento = "./img/vizq" & Convert.ToString(.Rows(Indexex).Item("Rumbo")).ToLowerInvariant() & ".png"
                                        End If
                                    Else
                                        If .Rows(Indexex).Item("Evento").Contains("DERECHA") Then
                                            iconoevento = "./img/vder.png"
                                        ElseIf .Rows(Indexex).Item("Evento").Contains("IZQ") Then
                                            iconoevento = "./img/vizq.png"
                                        End If
                                    End If
                                End If
                            Else
                                iconoevento = "./img/stop_red.png"
                            End If
                            If puede_insertar Then

                                archivoPOS.WriteLine(posPOINT + _
                                                     Chr(9) + _
                                                     .Rows(Indexex).Item("Identificador_rastreo") + _
                                                     Chr(9) + _
                                                     "<div class=""rastreo_botones"">" + _
                                                     "<table width=""100%"" style=""font-size:smaller"" cellspacing=""0"" cellpadding=""0""><tr><td>" + _
                                                     "<tr><td colspan=""3"" style=""border:solid 1px grey;"">" + _
                                                     "<input type=""button"" value=""Agregar posicion como referencia"" style=""font-size:x-small"" class=""rastreo_botones_save"" onclick=""setLONLAT_Historico(" + tmpLONG + "," + tmpLAT + ");"" /></td></tr>" + _
                                                     "<tr><td style=""border:solid 1px grey;"">" + _
                                                     "Fecha y hora:<td/><td style=""border:solid 1px grey;"">" + Convert.ToString(.Rows(Indexex).Item("Gps_fechahora_reporte")) + "<td/><tr/>" + _
                                                     "<tr><td style=""border:solid 1px grey;"">" + _
                                                     "Estado:<td/><td style=""border:solid 1px grey;"">" + .Rows(Indexex).Item("Evento") + "<td/><tr/>" + _
                                                     "<tr><td style=""border:solid 1px grey;"">" + _
                                                     "Velocidad:<td/><td style=""border:solid 1px grey;"">" + Convert.ToString(.Rows(Indexex).Item("Gps_velocidad")) + "<td/><tr/>" + _
                                                     "<tr><td style=""border:solid 1px grey;"">" + _
                                                     "Rumbo:<td/><td style=""border:solid 1px grey;"">" + Convert.ToString(.Rows(Indexex).Item("Gps_rumbo")) + "° " + .Rows(Indexex).Item("Rumbo") + "<td/><tr/>" + _
                                                     "</table>" + _
                                                     "</div>" + _
                                                     Chr(9) + _
                                                     iconoevento + _
                                                     Chr(9) + _
                                                     "15, 15" + _
                                                     Chr(9) + _
                                                     "-1, -1")

                                If conta_paradas > 0 And oldconta <> conta_paradas Then
                                    oldconta = conta_paradas
                                    archivoPOS.WriteLine(posPOINT + _
                                                     Chr(9) + _
                                                     .Rows(Indexex).Item("Identificador_rastreo") + _
                                                     Chr(9) + _
                                                     "<div class=""rastreo_botones"">" + _
                                                     "<table width=""100%"" style=""font-size:smaller"" cellspacing=""0"" cellpadding=""0""><tr><td>" + _
                                                     "<tr><td colspan=""3"" style=""border:solid 1px grey;"">" + _
                                                     "<input type=""button"" value=""Agregar parada " & conta_paradas.ToString() & " de referencia"" style=""font-size:x-small"" class=""rastreo_botones_save"" onclick=""setLONLAT_Historico(" + tmpLONG + "," + tmpLAT + ");"" /></td></tr>" + _
                                                     "<tr><td style=""border:solid 1px grey;"">" + _
                                                     "Fecha y hora:<td/><td style=""border:solid 1px grey;"">" + Convert.ToString(.Rows(Indexex).Item("Gps_fechahora_reporte")) + "<td/><tr/>" + _
                                                     "<tr><td style=""border:solid 1px grey;"">" + _
                                                     "Estado:<td/><td style=""border:solid 1px grey;"">" + .Rows(Indexex).Item("Evento") + "<td/><tr/>" + _
                                                     "<tr><td style=""border:solid 1px grey;"">" + _
                                                     "Velocidad:<td/><td style=""border:solid 1px grey;"">" + Convert.ToString(.Rows(Indexex).Item("Gps_velocidad")) + "<td/><tr/>" + _
                                                     "<tr><td style=""border:solid 1px grey;"">" + _
                                                     "Rumbo:<td/><td style=""border:solid 1px grey;"">" + Convert.ToString(.Rows(Indexex).Item("Gps_rumbo")) + "° " + .Rows(Indexex).Item("Rumbo") + "<td/><tr/>" + _
                                                     "</table>" + _
                                                     "</div>" + _
                                                     Chr(9) + _
                                                     Utilidades.Texto_a_Imagen(conta_paradas.ToString(), _
                                                        MapPath(Utilidades.web_directory_txtimgs), _
                                                        conta_paradas.ToString(), _
                                                        ImageFormat.Png, "Black", "Trasparent", 12, 12, 12) + _
                                                     Chr(9) + _
                                                     "12, 20" + _
                                                     Chr(9) + _
                                                     "15, -1")
                                End If
                            End If
                            archivoPOS.Flush()
                            If primer_pase Then
                                primer_pase = False
                                velmin = .Rows(Indexex).Item("Gps_velocidad")
                                velmax = velmin
                                oldlat = .Rows(Indexex).Item("Gps_latitud")
                                oldlon = .Rows(Indexex).Item("Gps_longitud")
                                sbScript.Append("<script language='JavaScript' type='text/javascript'>" + ControlChars.Lf)
                                sbScript.Append("<!--" + ControlChars.Lf)
                                sbScript.Append("map_panTo(" + _
                                                tmpLONG + "," + tmpLAT + _
                                                    ");" + ControlChars.Lf)
                                sbScript.Append("// -->" + ControlChars.Lf)
                                sbScript.Append("</script>" + ControlChars.Lf)
                            End If


                            If Not .Rows(Indexex).Item("Evento") Is DBNull.Value Then
                                If .Rows(Indexex).Item("Evento").Contains("DETENIDO") Then
                                    total_detenido += 1
                                ElseIf .Rows(Indexex).Item("Evento").Contains("MARCHA") Then
                                    total_en_marcha += 1
                                End If
                            End If

                            total_posiciones += 1

                            If .Rows(Indexex).Item("Gps_velocidad") > velmax Then
                                velmax = .Rows(Indexex).Item("Gps_velocidad")
                            End If
                            If .Rows(Indexex).Item("Gps_velocidad") < velmin Then
                                velmin = .Rows(Indexex).Item("Gps_velocidad")
                            End If

                            total_velocidad_promedio += .Rows(Indexex).Item("Gps_velocidad")


                            ' Populate new series with data
                            'serieVelocidad.Points.AddY(
                            If (Indexex > 0) Then
                                serieVelocidad.Points.AddXY(Convert.ToString(.Rows(Indexex).Item("Gps_fechahora_reporte")), .Rows(Indexex).Item("Gps_velocidad"))
                            End If
                            'serieVelocidad.Points.AddY(57)
                            'serieVelocidad.Points.AddY(83)
                            'serieVelocidad.Points.AddY(23)
                            'serieVelocidad.Points.AddY(70)
                            'serieVelocidad.Points.AddY(60)
                            'serieVelocidad.Points.AddY(90)
                            'serieVelocidad.Points.AddY(20)

                            '' Add series into the chart's series collection

                            total_km += _
                            Utilidades.dblCalcular_distancia_entre_dos_puntos_KM(oldlat, oldlon, _
                            .Rows(Indexex).Item("Gps_latitud"), .Rows(Indexex).Item("Gps_longitud"))
                            oldlat = .Rows(Indexex).Item("Gps_latitud")
                            oldlon = .Rows(Indexex).Item("Gps_longitud")

                            Indexex += 1
                        End While
                        'End SyncLock
                        chartVelocidad.Series.Add(serieVelocidad)

                        archivoPOS.Close()
                        If Not chkResumenOnly.Checked Then
                            txtfile.Value = web_filename
                            ViewState.Add("txtfile", web_filename)
                        End If
                        txtTOTPOS.Text = total_posiciones.ToString()

                        Indexex = 0
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

                        If txtPrecioGslt.Text > 0 Then
                            precioGslt = Convert.ToInt16(txtPrecioGslt.Text)
                        Else
                            precioGslt = 6200
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

                            Try
                                txtCONSUMO.Text = ((consumoaprox * total_km) / 100).ToString("F2") + " lts."
                                txtConsumoGs.Text = (((Math.Round(consumoaprox * total_km) / 100)) * precioGslt).ToString("F2") + "Gs."

                            Catch ex As Exception

                            End Try

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
            If getRecorridoPOS.Length > 0 Then
                getRecorridoPOS = getRecorridoPOS.Substring(0, getRecorridoPOS.Length - 1)
            End If
            Return getRecorridoPOS
        Catch ex As Exception
            Session.Add("msjerror", ex.ToString)
            If getRecorridoPOS.Length > 0 Then
                Return getRecorridoPOS.Substring(0, getRecorridoPOS.Length - 1)
            End If
        Finally
            'GC.Collect()
        End Try
    End Function

    Protected Sub btnIniciarHistorico_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnIniciarHistorico.Click
        ViewState.Add("IniciadoHistorico", True)
        Oki = True
        Session("sqldsVehiculoSeleccionado.SelectCommand") = Nothing
        Limpiar_campos()
        'Calcular_descarga_recorrido()
        ajax_popuphistorico.Hide()
        If chkBusqueda.Checked Then
            Reload_Data(ddlFiltroEventos.SelectedValue, txtSpdINI.Text, txtSpdFIN.Text)
        Else
            Reload_Data()
        End If
    End Sub

    Private Sub Limpiar_campos()
        txtTOTDETENIDO.Text = String.Empty
        txtTOTENMOV.Text = String.Empty
        txtTOTKM.Text = String.Empty
        txtTOTPOS.Text = String.Empty
        txtVELPROM.Text = String.Empty
    End Sub

    Private Sub Reload_Data(Optional ByVal evento As String = "SIN EVENTO", Optional ByVal SpdIni As String = "", _
                            Optional ByVal SpdFin As String = "")
        'If SpdIni <> "" And SpdFin <> "" And evento <> "SIN EVENTO" Then
        '    sqldsVehiculoSeleccionado.SelectCommand = g_selectcommand & " WHERE gps_evento = @evento AND rastreogps_equipo_eventos.habilitado = TRUE AND rastreo_vehiculo.idrastreo_vehiculo = @idVehiculoSeleccionado AND gps_fechahora_reporte BETWEEN @fecha_inicio AND @fecha_fin AND gps_velocidad BETWEEN @SpdIni AND @SpdFin  AND gps_latitud <> 0 AND gps_longitud <> 0 "
        'ElseIf evento <> "SIN EVENTO" Then
        '    sqldsVehiculoSeleccionado.SelectCommand = g_selectcommand & " WHERE gps_evento = @evento AND rastreogps_equipo_eventos.habilitado = TRUE AND rastreo_vehiculo.idrastreo_vehiculo = @idVehiculoSeleccionado AND gps_fechahora_reporte BETWEEN @fecha_inicio AND @fecha_fin AND gps_latitud <> 0 AND gps_longitud <> 0 "
        'End If

        filtroRefOnly = getRecorridoPOS(miUsuario, evento, SpdIni, SpdFin)

        If filtroRefOnly <> String.Empty Then
            Dim wherestring As String = String.Empty
            If sqldsVehiculoSeleccionado.SelectCommand.Contains("WHERE") Then
                wherestring = " AND reportesgps"
            Else
                wherestring = " WHERE reportesgps"
            End If
            sqldsVehiculoSeleccionado.SelectCommand = _
            g_selectcommand + wherestring + _
            Convert.ToDateTime(txtFECHAINICIO.Value).Month.ToString() + Convert.ToDateTime(txtFECHAINICIO.Value).Year.ToString() & ".idreportesgps" + Convert.ToDateTime(txtFECHAINICIO.Value).Month.ToString() + Convert.ToDateTime(txtFECHAINICIO.Value).Year.ToString() & " IN (" & filtroRefOnly & ");"
        End If

        If chkResumenOnly.Checked Then
            sqldsVehiculoSeleccionado.SelectCommand = String.Empty
        End If

        If Oki Then
            sqldsVehiculoSeleccionado.DataBind()
            gridVehiculoSeleccionado.DataBind()
            'sqldsVehiculoSeleccionado.SelectCommand = ""
            'ViewState.Remove("IniciadoHistorico")
        End If
        If g_RowCount > 0 Then
            gridVehiculoSeleccionado.PageSize = g_RowCount
        Else
            gridVehiculoSeleccionado.PageSize = 20
        End If
        'sqldsVehiculoSeleccionado.SelectCommand = ""
        Session("sqldsVehiculoSeleccionado.SelectCommand") = sqldsVehiculoSeleccionado.SelectCommand
        lbStatusOpciones.Text = "Inicio de recorrido: " + txtFECHAINICIO.Value + " Fin de recorrido: " + _
                                txtFECHAFIN.Value
        If ViewState("txtfile") <> String.Empty Then
            txtfile.Value = ViewState("txtfile")
        End If
    End Sub

    Protected Sub btnDismiss_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDismiss.Click
        ajax_popuphistorico.Hide()
    End Sub

    Protected Sub btnCalcular_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCalcular.Click
        Calcular_descarga_recorrido()
    End Sub

    Private Function Calcular_descarga_recorrido() As Integer
        Try
            Calcular_descarga_recorrido = 0
            Dim SQL_count_reportes As String = g_selectcommand & _
                "   WHERE reportesgps" + Convert.ToDateTime(txtFECHAINICIO.Value).Month.ToString() + Convert.ToDateTime(txtFECHAINICIO.Value).Year.ToString() & ".id_equipogps = " & idequipogps.ToString() & _
                "   AND reportesgps" + Convert.ToDateTime(txtFECHAINICIO.Value).Month.ToString() + Convert.ToDateTime(txtFECHAINICIO.Value).Year.ToString() & ".gps_fechahora_reporte BETWEEN '" & Convert.ToDateTime(txtFECHAINICIO.Value).ToString("yyyy-MM-dd HH:mm:ss") & "' AND '" & _
                Convert.ToDateTime(txtFECHAFIN.Value).ToString("yyyy-MM-dd HH:mm:ss") & "'"
            Dim rastrear_conexion As New NpgsqlConnection(cnn_str.CadenaDeConexion)
            rastrear_conexion.Open()
            Dim rastrear_commando As New NpgsqlCommand(SQL_count_reportes, rastrear_conexion)
            Dim rastrear_reader As New NpgsqlDataAdapter(rastrear_commando)
            Dim rastrear_dataset As New DataSet()
            rastrear_commando.CommandTimeout = 120
            Try
                rastrear_reader.Fill(rastrear_dataset)
            Catch
                lbCalculo.Text = "No hay posiciones del rango seleccionado"
                Calcular_descarga_recorrido = 0
                Return Calcular_descarga_recorrido
            End Try
            'MsgBox(rastrear_dataset.Tables(0).Rows.Count)
            lbCalculo.Text = rastrear_dataset.Tables(0).Rows.Count.ToString() + " puntos de posición, demorará " + Tiempo_en_minutos(rastrear_dataset.Tables(0).Rows.Count)
            Calcular_descarga_recorrido = rastrear_dataset.Tables(0).Rows.Count
        Catch ex As Exception

        End Try

    End Function

    Private Function Tiempo_en_minutos(ByVal rows As Integer) As String
        If rows > 0 Then
            Dim tempo As String = String.Empty
            Dim cant_pto_tiempo As Integer = rows * 0.6 '3 segundos para obtener direccion?
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

    Protected Sub btnExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExcel.Click
        'Dim recorrido As New rsview_recorridos(cnn_str.CadenaDeConexion)
        'With recorrido
        '.Where.Evento_habilitado.Value = True
        '.Where.Id_vehiculo.Value = idvehiculo
        '.Where.Gps_fechahora_reporte.Operator = WhereParameter.Operand.Between
        '.Where.Gps_fechahora_reporte.BetweenBeginValue = Convert.ToDateTime(txtFECHAINICIO.Trim()).ToString("yyyy-MM-dd HH:mm:ss")
        '.Where.Gps_fechahora_reporte.BetweenEndValue = Convert.ToDateTime(txtFECHAFIN.Trim()).ToString("yyyy-MM-dd HH:mm:ss")
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

    'Private Sub BindToDataSet(ByVal gridVehiculoSeleccionado As DataGrid)
    '    ' Create a DataSet.
    '    Dim myDataSet As New DataSet("myDataSet")
    '    ' Insert code to populate DataSet with several tables.
    '    gridVehiculoSeleccionado.DataSource = myDataSet
    '    ' Use the DataMember property to specify the DataTable.
    '    gridVehiculoSeleccionado.DataMember = "Suppliers"
    'End Sub 'BindToDataSet


    Protected Sub btnKml_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnKml.Click

        Dim spdini As String = ""
        Dim spdfin As String = ""
        Dim evento As String = "SIN EVENTO"
        Dim lat As String = ""
        Dim lon As String = ""
        Dim rastrear_url = "http://190.104.156.142:8080/"
        'Indexex = 0
        ''cambiar
        Dim sql_where_reportes As String = String.Empty
        Dim rastrear_conexion As New NpgsqlConnection(cnn_str.CadenaDeConexion)
        rastrear_conexion.Open()

        Dim sbScript As New StringBuilder()
        sql_where_reportes = " WHERE Idrastreo_vehiculo = " & idVehiculoSeleccionado.Value & _
                             " AND Gps_fechahora_reporte BETWEEN '" & Convert.ToDateTime(txtFECHAINICIO.Value).ToString("yyyy-MM-dd HH:mm:ss") & _
                             "' AND '" & Convert.ToDateTime(txtFECHAFIN.Value).ToString("yyyy-MM-dd HH:mm:ss") & "' "
        If evento <> "SIN EVENTO" Then
            sql_where_reportes += " AND Gps_evento = '" & evento & "' "
        End If
        If spdini <> "" And spdfin <> "" Then
            sql_where_reportes += " AND (Gps_velocidad BETWEEN " & spdini & " AND " & spdfin & ") "
        End If
        sql_where_reportes = sql_where_reportes + " AND gps_latitud <> 0 AND gps_longitud <> 0 "
        Dim rastrear_commando As New NpgsqlCommand(g_selectcommand + sql_where_reportes, rastrear_conexion)
        rastrear_commando.CommandTimeout = 120
        Dim rastrear_reader As New NpgsqlDataAdapter(rastrear_commando)
        Dim tRecorrido As New DataSet()
        rastrear_reader.Fill(tRecorrido)

        HttpContext.Current.Response.Clear()
        HttpContext.Current.Response.ContentType() = "application/vnd.google-earth.kml+xml"
        ''uncomment following line to view raw xml
        ''My.Response.ContentType = "plain/text"
        HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8
        Dim stream As New System.IO.MemoryStream
        Dim XMLwrite As New XmlTextWriter(stream, System.Text.Encoding.UTF8)

        XMLwrite.WriteStartDocument()
        XMLwrite.WriteWhitespace(Environment.NewLine)
        XMLwrite.WriteStartElement("kml", "http://earth.google.com/kml/2.1")
        XMLwrite.WriteWhitespace(Environment.NewLine)
        XMLwrite.WriteStartElement("Document") 'Hay que encerrar a todo el documento con un <Document></Document>
        XMLwrite.WriteWhitespace(Environment.NewLine)
        XMLwrite.WriteElementString("Name", "Recorrido")
        XMLwrite.WriteWhitespace(Environment.NewLine)
        XMLwrite.WriteElementString("description", Page.Title.Replace("RASTREO Paraguay - ", "").Trim())
        XMLwrite.WriteWhitespace(Environment.NewLine)
        Dim lineRecorrido As String = String.Empty

        With tRecorrido.Tables(0)
            g_RowCount = .Rows.Count
            Indexex = 0
            While Indexex < g_RowCount
                Dim iconoevento As String = rastrear_url + "img/imgkml/center_normal.gif"
                If Not .Rows(Indexex).Item("Evento") Is DBNull.Value Then
                    If .Rows(Indexex).Item("Evento").Contains("MARCHA") Or _
                        .Rows(Indexex).Item("Evento").Contains("IDA") Or _
                        .Rows(Indexex).Item("Evento").Contains("VUELTA") Or _
                        .Rows(Indexex).Item("Evento").Contains("POSICION") Then
                        iconoevento = rastrear_url + "img/imgkml/" & .Rows(Indexex).Item("Rumbo") & ".png"
                    ElseIf .Rows(Indexex).Item("Evento").Contains("APAGADO") Or .Rows(Indexex).Item("Evento").Contains("DETENIDO") Then
                        iconoevento = rastrear_url + "img/imgkml/stop_red.png"
                    ElseIf .Rows(Indexex).Item("Evento").Contains("DERECHA") Or .Rows(Indexex).Item("Evento").Contains("IZQ") Then
                        If .Rows(Indexex).Item("Rumbo") <> String.Empty Then
                            If .Rows(Indexex).Item("Evento").Contains("DERECHA") Then
                                iconoevento = rastrear_url + "img/imgkml/vder" & Convert.ToString(.Rows(Indexex).Item("Rumbo")).ToLowerInvariant() & ".png"
                            ElseIf .Rows(Indexex).Item("Evento").Contains("IZQ") Then
                                iconoevento = rastrear_url + "img/imgkml/vizq" & Convert.ToString(.Rows(Indexex).Item("Rumbo")).ToLowerInvariant() & ".png"
                            End If
                        Else
                            If .Rows(Indexex).Item("Evento").Contains("DERECHA") Then
                                iconoevento = rastrear_url + "img/imgkml/vder.png"
                            ElseIf .Rows(Indexex).Item("Evento").Contains("IZQ") Then
                                iconoevento = rastrear_url + "img/imgkml/vizq.png"
                            End If
                        End If
                    End If
                Else
                    iconoevento = rastrear_url + "img/imgkml/stop_red.png"
                End If
                lon = Convert.ToString(.Rows(Indexex).Item("Gps_longitud")).Replace(",", ".")
                lat = Convert.ToString(.Rows(Indexex).Item("Gps_latitud")).Replace(",", ".")
                XMLwrite.WriteStartElement("Placemark")
                XMLwrite.WriteWhitespace(Environment.NewLine)
                'No hace falta sacar nada de la grilla, en la tabla tenes todo fijate el query para saber que item de cada row necesitas.

                XMLwrite.WriteElementString("description", Convert.ToString(.Rows(Indexex).Item("gps_fechahora_reporte")) & " , " & Convert.ToString(.Rows(Indexex).Item("Gps_velocidad")) & "km/h" & " , " & Convert.ToString(.Rows(Indexex).Item("gps_dir"))) ' aqui quiero que aparezca la Fecha Completa, Hora,minutos y segundos con su  Direccion Cercana que aparece en gridVehiculoSeleccionado
                XMLwrite.WriteElementString("name", Convert.ToString(.Rows(Indexex).Item("Evento"))) ' Aqui le quite el titulo de las posiciones porque en el mapa se juntan todos y no es bien legible
                XMLwrite.WriteWhitespace(Environment.NewLine)
                XMLwrite.WriteStartElement("LookAt")
                XMLwrite.WriteElementString("longitude", lon) ' Esta es la parte que mas me cuesta, aqui quiero leer la LONGITUD de donde estuvo el movil  en gridVehiculoSeleccionado
                XMLwrite.WriteWhitespace(Environment.NewLine)
                XMLwrite.WriteElementString("latitude", lat) ' Esta es la parte que mas me cuesta, aqui quiero leer la LATITUD de donde estuvo el movil en gridVehiculoSeleccionado
                XMLwrite.WriteWhitespace(Environment.NewLine)
                XMLwrite.WriteElementString("range", "300")
                XMLwrite.WriteWhitespace(Environment.NewLine)
                XMLwrite.WriteElementString("tilt", "20")
                XMLwrite.WriteWhitespace(Environment.NewLine)
                XMLwrite.WriteElementString("heading", Convert.ToString(.Rows(Indexex).Item("Gps_rumbo")))
                XMLwrite.WriteWhitespace(Environment.NewLine)
                XMLwrite.WriteEndElement()
                XMLwrite.WriteWhitespace(Environment.NewLine)
                XMLwrite.WriteElementString("visibility", "1")
                XMLwrite.WriteWhitespace(Environment.NewLine)
                XMLwrite.WriteStartElement("Style")
                XMLwrite.WriteWhitespace(Environment.NewLine)
                XMLwrite.WriteStartElement("IconStyle")
                XMLwrite.WriteWhitespace(Environment.NewLine)
                XMLwrite.WriteStartElement("Icon")
                XMLwrite.WriteWhitespace(Environment.NewLine)
                XMLwrite.WriteElementString("href", iconoevento)
                XMLwrite.WriteWhitespace(Environment.NewLine)
                XMLwrite.WriteElementString("w", "-1")
                XMLwrite.WriteWhitespace(Environment.NewLine)
                XMLwrite.WriteElementString("h", "-1")
                XMLwrite.WriteWhitespace(Environment.NewLine)
                XMLwrite.WriteEndElement()
                XMLwrite.WriteWhitespace(Environment.NewLine)
                XMLwrite.WriteEndElement()
                XMLwrite.WriteWhitespace(Environment.NewLine)
                XMLwrite.WriteEndElement()

                XMLwrite.WriteStartElement("Point")
                XMLwrite.WriteWhitespace(Environment.NewLine)
                XMLwrite.WriteElementString("extrude", "1")
                XMLwrite.WriteWhitespace(Environment.NewLine)
                XMLwrite.WriteElementString("altitudeMode", "relativeToGround")
                XMLwrite.WriteWhitespace(Environment.NewLine)
                XMLwrite.WriteElementString("coordinates", lon & ", " & lat & ", 0") 'aqui es una consecuencia luego de haber obtenido LATITUD y LONGITUD

                lineRecorrido += lon & ", " & lat + Environment.NewLine 'guardamos el punto para ir formando la linea

                XMLwrite.WriteWhitespace(Environment.NewLine)
                XMLwrite.WriteEndElement()
                XMLwrite.WriteWhitespace(Environment.NewLine)
                XMLwrite.WriteEndElement()
                XMLwrite.WriteWhitespace(Environment.NewLine)
                XMLwrite.Flush()
                Indexex += 1
            End While

        End With
        'Creamos el Style y La linea en si
        '<Style id="RASTREARLine">
        ' <LineStyle>
        '  <color>7f075fff</color>
        '  <width>4</width>
        ' </LineStyle>
        ' <PolyStyle>
        '  <color>7f00ff00</color>
        ' </PolyStyle>
        '</Style>
        XMLwrite.WriteStartElement("Style")
        XMLwrite.WriteAttributeString("id", "RASTREARLinePoly")
        XMLwrite.WriteStartElement("LineStyle")
        XMLwrite.WriteElementString("color", "7f075fff")
        XMLwrite.WriteElementString("width", "6")
        XMLwrite.WriteEndElement()
        XMLwrite.WriteStartElement("PolyStyle")
        XMLwrite.WriteElementString("color", "7f00ff00")
        XMLwrite.WriteEndElement()
        XMLwrite.WriteEndElement()
        XMLwrite.WriteStartElement("Placemark")
        XMLwrite.WriteElementString("name", Page.Title.Replace("RASTREO Paraguay - ", "").Trim())
        XMLwrite.WriteElementString("styleUrl", "#RASTREARLinePoly")
        XMLwrite.WriteStartElement("LineString")
        XMLwrite.WriteElementString("extrude", "1")
        XMLwrite.WriteElementString("tessellate", "1")
        XMLwrite.WriteElementString("coordinates", lineRecorrido)
        XMLwrite.WriteEndElement()
        XMLwrite.WriteEndElement()
        XMLwrite.WriteEndElement()

        XMLwrite.WriteEndElement()
        XMLwrite.WriteWhitespace(Environment.NewLine)
        'XMLwrite.WriteEndElement()

        XMLwrite.Flush()

        Dim reader As IO.StreamReader
        stream.Position = 0
        reader = New IO.StreamReader(stream)
        Dim bytes() As Byte = System.Text.Encoding.UTF8.GetBytes(reader.ReadToEnd())
        HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=recorrido_" & vehiculo_seleccionado & Now.ToString("_ddMMyyy_HHmm_") & ".kml")
        HttpContext.Current.Response.BinaryWrite(bytes)
        HttpContext.Current.Response.End()

    End Sub

    Protected Sub ddlFiltroEventos_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlFiltroEventos.SelectedIndexChanged
        'FiltrarRecorido()
    End Sub

    Private Sub Bind_ddlFiltroEventos()
        Dim tipoquipo As Integer = 0

        Dim tblTipoEventos As New rsview_equipo_eventos(cnn_str.CadenaDeConexion)
        Dim tblVista As New rsview_cliente_vehiculo_equipogps(cnn_str.CadenaDeConexion)
        With tblVista
            .Where.Idrastreo_vehiculo.Value = idvehiculo
            If .Query.Load() Then
                If .RowCount > 0 Then
                    If .s_Id_tipoequipogps <> String.Empty Then
                        tipoquipo = .Idequipogps
                    End If
                End If
            End If
        End With
        If tipoquipo > 0 Then
            With tblTipoEventos
                .Where.Id_rastreoequipogps.Value = tipoquipo
                If .Query.Load() Then
                    If .RowCount > 0 Then
                        ddlFiltroEventos.Items.Clear()
                        While Not .EOF
                            Dim itm As New ListItem(.Evento, .Evento_raw)
                            ddlFiltroEventos.Items.Add(itm)
                            .MoveNext()
                        End While
                    End If
                End If
            End With
        End If
    End Sub

    Private Sub FiltrarRecorido()
        Oki = True
        If chkBusqueda.Checked Or chkSpeed.Checked Then
            If ddlFiltroEventos.SelectedValue <> "" Then
                Reload_Data(ddlFiltroEventos.SelectedValue, txtSpdINI.Text, txtSpdFIN.Text)
            Else
                Reload_Data("SIN EVENTO", txtSpdINI.Text, txtSpdFIN.Text)
            End If
            sqldsVehiculoSeleccionado.DataBind()
            gridVehiculoSeleccionado.DataBind()
        Else
            Reload_Data()
            sqldsVehiculoSeleccionado.DataBind()
            gridVehiculoSeleccionado.DataBind()
        End If
    End Sub

    Protected Sub chkBusqueda_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkBusqueda.CheckedChanged
        If chkBusqueda.Checked Then
            Bind_ddlFiltroEventos()
            pnlBusqueda.Visible = True
        Else
            pnlBusqueda.Visible = False
        End If
        'FiltrarRecorido()
    End Sub

    Protected Sub gridVehiculoSeleccionado_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridVehiculoSeleccionado.DataBound
        ViewState.Remove("IniciadoHistorico")
    End Sub

    Protected Sub gridVehiculoSeleccionado_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gridVehiculoSeleccionado.RowCommand
        '"PUNTO DE REFERENCIA"
        'Todo: Query el boton agregue a la tabla hoja_de_ruta si no existe 
        'un registro general para el usuario q sera PUBLICO para todos los usuarios del cliente/empresa
        'Y a los puntos de la hoja creada "pUNTOS DE REFERENCIA" O NO SE SERAN INSERTADOS CON LA LAT 
        'LON DE ESTE REPORTE Q SE PASA POR COMMANDARGUMENT
        If e.CommandName = "PUNTO DE REFERENCIA" Then
            pnlPuntoReferencia.Visible = True
        End If
    End Sub

    Protected Sub OLDbtnSAVEPTO_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If txtLAT.Text <> String.Empty And txtLON.Text <> String.Empty And _
            txtNombre_pto.Text <> String.Empty And txtDescripcion_pto.Text <> String.Empty Then
            Dim tblHojaRef As New rastreo_template_hoja_de_ruta(cnn_str.CadenaDeConexion)
            Dim tblPTOSHojaRef As New rastreo_template_puntos_hoja_de_ruta(cnn_str.CadenaDeConexion)
            With tblHojaRef
                .Where.Descripcion_recorrido.Value = "REFERENCIAS DE " + miUsuario.NombreUsuario
                .Where.Id_usuarios.Value = miUsuario.idUsuario
                .Where.Id_persona.Value = miUsuario.idPersona
                If Not .Query.Load() Then
                    .AddNew()
                    .Descripcion_recorrido = "REFERENCIAS DE  " + miUsuario.NombreUsuario
                    .Id_usuarios = miUsuario.idUsuario
                    .Id_persona = miUsuario.idPersona
                    .Publico = True
                    .Fech_ins = Now
                    .User_ins = miUsuario.idUsuario
                    .Save()
                End If
                If .s_Id_recorridotemplate <> String.Empty Then
                    tblPTOSHojaRef.Where.Id_recorridotemplate.Value = .Id_recorridotemplate
                    tblPTOSHojaRef.Where.Lat.Value = Convert.ToDouble(txtLAT.Text.Replace(".", ","))
                    tblPTOSHojaRef.Where.Lon.Value = Convert.ToDouble(txtLON.Text.Replace(".", ","))
                    If tblPTOSHojaRef.Query.Load() Then
                        'Que hacemos si ya existe el mismo exacto punto de lat y long?
                    Else
                        tblPTOSHojaRef.AddNew()
                        tblPTOSHojaRef.Lat = Convert.ToDouble(txtLAT.Text.Replace(".", ","))
                        tblPTOSHojaRef.Lon = Convert.ToDouble(txtLON.Text.Replace(".", ","))
                        tblPTOSHojaRef.Id_recorridotemplate = .Id_recorridotemplate
                    End If
                    tblPTOSHojaRef.Nombre = txtNombre_pto.Text.ToUpperInvariant().Trim()
                    tblPTOSHojaRef.Descripcion = txtDescripcion_pto.Text.ToUpperInvariant().Trim()

                    If chkReferencia_ptoref.Checked Then
                        tblPTOSHojaRef.Avisar_ingreso = True
                        tblPTOSHojaRef.Emails = txtMailReferencias_ptoref.Text.Replace(" ", ";").Replace(",", ";").Trim()
                    Else
                        tblPTOSHojaRef.Avisar_ingreso = False
                        tblPTOSHojaRef.SetColumnNull(rastreo_template_puntos_hoja_de_ruta.ColumnNames.Emails)
                    End If
                    tblPTOSHojaRef.Save()
                    Session.Add("msjerror", "Punto de referencia agregado con exito!")
                    'ShowMeTheError()
                End If
            End With
        End If
    End Sub

    Protected Sub btnSAVEPTO_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSAVEPTO.Click
        If txtLAT.Text <> String.Empty And txtLON.Text <> String.Empty And _
            txtNombre_pto.Text <> String.Empty And txtDescripcion_pto.Text <> String.Empty Then
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
                    tblPTOSHojaRef.Nombre = txtNombre_pto.Text.ToUpperInvariant().Trim()
                    tblPTOSHojaRef.Descripcion = txtDescripcion_pto.Text.ToUpperInvariant().Trim()
                    If txtMETROS_REDONDA_ptorefref.Text.Trim() <> String.Empty Then
                        tblPTOSHojaRef.Metros = Convert.ToInt32(txtMETROS_REDONDA_ptorefref.Text.Trim())
                    Else
                        tblPTOSHojaRef.Metros = 100
                    End If
                    If chkReferencia_ptoref.Checked Then
                        tblPTOSHojaRef.Avisar_ingreso \= True
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

    Protected Sub chkReferencia_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkReferencia.CheckedChanged
        If chkReferencia.Visible Then
            pnlPuntoReferencia.Visible = True
        Else
            pnlPuntoReferencia.Visible = False
        End If
    End Sub

    Protected Sub chkSpeed_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkSpeed.CheckedChanged
        pnlVelocidad.Visible = Not pnlVelocidad.Visible
        If chkSpeed.Checked Then
            txtSpdINI.Text = String.Empty
            txtSpdFIN.Text = String.Empty
        End If
    End Sub

    Protected Sub btnFiltroSpd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFiltroSpd.Click
        FiltrarRecorido()
    End Sub


    Public Overloads Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)

    End Sub

    Public Sub ExportExcel()

        Dim sb As New StringBuilder()
        Dim sw As New StringWriter(sb)
        Dim htw As New HtmlTextWriter(sw)

        gridVehiculoSeleccionado.Columns(5).Visible = False
        txtPrecioGslt.Visible = False

        'For Each row As GridViewRow In gridVehiculoSeleccionado.Rows
        ' row.Cells(5).Controls.RemoveAt(5)
        ' row.Cells(5).Controls.RemoveAt(1)
        ' row.Cells(5).Controls.RemoveAt(2)
        'Next

        htw.Flush()
        lbStatusOpciones.RenderControl(htw)
        pnlResultados.RenderControl(htw)
        'pnlGraphs.RenderControl(htw)
        gridVehiculoSeleccionado.RenderControl(htw)

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

    Protected Sub ajax_popuphistorico_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles ajax_popuphistorico.Load
        ViewState.Remove("IniciadoHistorico")
    End Sub

    Protected Sub sqldsVehiculoSeleccionado_Selecting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.SqlDataSourceSelectingEventArgs) Handles sqldsVehiculoSeleccionado.Selecting
        e.Command.CommandTimeout = 7200
    End Sub

    Protected Sub gridVehiculoSeleccionado_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gridVehiculoSeleccionado.RowDataBound
        'MsgBox("row: " & e.Row.ID)
        'DirectCast(e.Row.DataItem, System.Data.DataRowView)("gps_longitud")
    End Sub

    Protected Sub gridVehiculoSeleccionado_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles gridVehiculoSeleccionado.RowUpdating

    End Sub

    Protected Sub ddlYear_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlYear.SelectedIndexChanged
        Calcular_descarga_recorrido()
    End Sub

    Protected Sub ddlMonth_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlMonth.SelectedIndexChanged
        Calcular_descarga_recorrido()
    End Sub

    Public Function GetStatusIO(ByVal IO As String, ByVal EdadDelDato As String, ByVal TipoDePosicion As String) As String
        GetStatusIO = String.Empty
        If IO <> String.Empty And EdadDelDato <> String.Empty And TipoDePosicion <> String.Empty Then
            Dim statusBIN As String = Utilidades.GetIO(IO)
            GetStatusIO = String.Empty
            GetStatusIO = GetStatusIO & "<table cellpadding='0' cellspacing='0' width='100%'>"
            GetStatusIO = GetStatusIO & "<tr><td>Ignición:</td><td>"
            GetStatusIO = GetStatusIO & IIf(statusBIN.Substring(0, 1) = "1", "<img src='App_Themes/CENTRAL/Imagenes/okidoki.png' alt='Ok' />", "<img src='App_Themes/CENTRAL/Imagenes/process-stop.png'  alt='X'/>")
            GetStatusIO = GetStatusIO & "</td>"
            GetStatusIO = GetStatusIO & "<td>Batería:</td><td>"
            GetStatusIO = GetStatusIO & IIf(statusBIN.Substring(1, 1) = "1", "<img src='App_Themes/CENTRAL/Imagenes/okidoki.png' alt='Ok' />", "<img src='App_Themes/CENTRAL/Imagenes/process-stop.png'  alt='X'/>")
            GetStatusIO = GetStatusIO & "</td></tr>"
            GetStatusIO = GetStatusIO & "<tr><td>Entrada 5:</td><td>"
            GetStatusIO = GetStatusIO & IIf(statusBIN.Substring(2, 1) = "1", "<img src='App_Themes/CENTRAL/Imagenes/okidoki.png' alt='Ok' />", "<img src='App_Themes/CENTRAL/Imagenes/process-stop.png'  alt='X'/>")
            GetStatusIO = GetStatusIO & "</td>"
            GetStatusIO = GetStatusIO & "<td>Entrada 4:</td><td>"
            GetStatusIO = GetStatusIO & IIf(statusBIN.Substring(3, 1) = "1", "<img src='App_Themes/CENTRAL/Imagenes/okidoki.png' alt='Ok' />", "<img src='App_Themes/CENTRAL/Imagenes/process-stop.png'  alt='X'/>")
            GetStatusIO = GetStatusIO & "</td></tr>"
            GetStatusIO = GetStatusIO & "<tr><td>Entrada 3:</td><td>"
            GetStatusIO = GetStatusIO & IIf(statusBIN.Substring(4, 1) = "1", "<img src='App_Themes/CENTRAL/Imagenes/okidoki.png' alt='Ok' />", "<img src='App_Themes/CENTRAL/Imagenes/process-stop.png'  alt='X'/>")
            GetStatusIO = GetStatusIO & "</td>"
            GetStatusIO = GetStatusIO & "<td>Entrada 2:</td><td>"
            GetStatusIO = GetStatusIO & IIf(statusBIN.Substring(5, 1) = "1", "<img src='App_Themes/CENTRAL/Imagenes/okidoki.png' alt='Ok' />", "<img src='App_Themes/CENTRAL/Imagenes/process-stop.png'  alt='X'/>")
            GetStatusIO = GetStatusIO & "</td></tr>"
            GetStatusIO = GetStatusIO & "<tr><td>Entrada 1:</td><td>"
            GetStatusIO = GetStatusIO & IIf(statusBIN.Substring(6, 1) = "1", "<img src='App_Themes/CENTRAL/Imagenes/okidoki.png' alt='Ok' />", "<img src='App_Themes/CENTRAL/Imagenes/process-stop.png'  alt='X'/>")
            GetStatusIO = GetStatusIO & "</td>"
            GetStatusIO = GetStatusIO & "<td>Entrada 0:</td><td>"
            GetStatusIO = GetStatusIO & IIf(statusBIN.Substring(7, 1) = "1", "<img src='App_Themes/CENTRAL/Imagenes/okidoki.png' alt='Ok' />", "<img src='App_Themes/CENTRAL/Imagenes/process-stop.png'  alt='X'/>")
            GetStatusIO = GetStatusIO & "</td></tr>"
            GetStatusIO = GetStatusIO & "</td>"
            Dim EDD As Integer = Integer.Parse(EdadDelDato, System.Globalization.NumberStyles.HexNumber)
            GetStatusIO = GetStatusIO & "<td>Edad del Dato: " & EDD.ToString() & "</td><td>"
            If EDD <= 85 Then
                GetStatusIO = GetStatusIO & "<img src='App_Themes/CENTRAL/Imagenes/okidoki.png' alt='Ok' />"
            ElseIf EDD > 85 And EDD <= 170 Then
                GetStatusIO = GetStatusIO & "<img src='App_Themes/CENTRAL/Imagenes/edit-undo.png' alt='-'/>"
            ElseIf EDD > 170 Then
                GetStatusIO = GetStatusIO & "<img src='App_Themes/CENTRAL/Imagenes/process-stop.png'  alt='X'/>"
            End If
            GetStatusIO = GetStatusIO & "<td>Posicion " & TipoDePosicion & "D</td><td>"
            Select Case (TipoDePosicion)
                Case "3"
                    GetStatusIO = GetStatusIO & "<img src='App_Themes/CENTRAL/Imagenes/okidoki.png' alt='Ok' />"
                Case "2"
                    GetStatusIO = GetStatusIO & "<img src='App_Themes/CENTRAL/Imagenes/edit-undo.png' alt='-' />"
                Case Else
                    GetStatusIO = GetStatusIO & "<img src='App_Themes/CENTRAL/Imagenes/process-stop.png'  alt='X'/>"
            End Select
            GetStatusIO = GetStatusIO & "</td></tr>"
            GetStatusIO = GetStatusIO & "</table>"
        End If
        Return GetStatusIO
    End Function

    Protected Sub txtPrecioGslt_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPrecioGslt.TextChanged
        Dim precioGslt As Double = 0
        Dim consumo As Double = 0

        If txtPrecioGslt.Text > 0 Then
            precioGslt = Convert.ToInt16(txtPrecioGslt.Text)
        Else
            precioGslt = 6200
        End If


        If "" = Trim(txtCONSUMO.Text) Then
            txtCONSUMO.Text = "0"
        Else
            consumo = CDbl(txtCONSUMO.Text.Substring(1, txtCONSUMO.Text.Length - " lts.".Length))
        End If

        txtConsumoGs.Text = (consumo * precioGslt).ToString + "Gs."


    End Sub
End Class
