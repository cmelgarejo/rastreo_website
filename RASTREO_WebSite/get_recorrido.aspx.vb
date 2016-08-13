Imports RASTREOmw
Imports System.IO
Imports System.Data
Imports System.Drawing.Color
Imports System.Collections.Generic
Imports System.Runtime.InteropServices
Imports MyGeneration.dOOdads
Imports Npgsql

Partial Class get_recorrido
    Inherits System.Web.UI.Page

    Public miUsuario As Usuario
    Public mi_usuario As String = String.Empty
    Public permiso_menu_usuarios As Boolean
    Public vehiculo_seleccionado As String = String.Empty
    Dim idvehiculo As New List(Of Integer)
    Dim idequipogps As Integer = 0
    Dim g_RowCount As Integer = 0
    Private g_selectcommand As String = String.Empty

    Private Sub setSQLCMD()
        txtFECHAINICIO.Value = Request.QueryString("fchini")
        g_selectcommand = _
                "SELECT DISTINCT ON (rastreo_vehiculo.identificador_rastreo, reportesgps" + Convert.ToDateTime(txtFECHAINICIO.Value).Month.ToString() + Convert.ToDateTime(txtFECHAINICIO.Value).Year.ToString() & ".gps_fechahora_reporte) " & _
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
                "LEFT JOIN rastreo_vehiculo 			ON reportesgps" + Convert.ToDateTime(txtFECHAINICIO.Value).Month.ToString() + Convert.ToDateTime(txtFECHAINICIO.Value).Year.ToString() & ".id_equipogps  = rastreo_vehiculo.id_equipogps " & _
                "LEFT JOIN rastreo_persona 			ON rastreo_vehiculo.id_cliente       = rastreo_persona.idrastreo_persona " & _
                "LEFT JOIN rastreo_cliente 			ON rastreo_vehiculo.id_cliente = rastreo_cliente.id_cliente " & _
                "LEFT JOIN rastreogps_equipogps 		ON rastreo_vehiculo.id_equipogps = rastreogps_equipogps.idrastreogps_equipogps " & _
                "LEFT JOIN rastreogps_tipoequipo 		ON rastreogps_equipogps.tipoequipo = rastreogps_tipoequipo.idrastreogps_tipoequipo " & _
                "LEFT JOIN rastreogps_tipoevento ON rastreogps_tipoevento.id_tipoequipo = rastreogps_tipoequipo.idrastreogps_tipoequipo " & _
                "                               AND rastreogps_tipoevento.evento = reportesgps" + Convert.ToDateTime(txtFECHAINICIO.Value).Month.ToString() + Convert.ToDateTime(txtFECHAINICIO.Value).Year.ToString() & ".gps_evento " & _
                "LEFT JOIN rastreogps_equipo_eventos ON rastreogps_equipo_eventos.id_equipogps = rastreogps_equipogps.idrastreogps_equipogps " & _
                "				AND rastreogps_tipoevento.idrastreogps_tipo_evento = rastreogps_equipo_eventos.id_tipoevento "
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
            Response.Redirect("./Login.aspx?to=" + Now.ToString(), False)
            'Return
        End If

        If Request.QueryString.Keys.Count > 0 Then
            'If Not Request.QueryString("vid") Is Nothing Then
            '    If Integer.TryParse(Request.QueryString("vid"), idvehiculo) Then
            '        idVehiculoSeleccionado.Value = idvehiculo
            '        Dim tblV As New rastreo_vehiculo(cnn_str.CadenaDeConexion)
            '        With tblV
            '            .Where.Idrastreo_vehiculo.Value = idvehiculo
            '            If .Query.Load() Then
            '                If .s_Identificador_rastreo <> String.Empty Then
            '                    vehiculo_seleccionado = .Identificador_rastreo
            '                    idVehiculoSeleccionado.Value = idvehiculo
            '                    idequipogps = .Id_equipogps
            '                    Page.Title = "RASTREO Paraguay - Recorrido del vehiculo " + vehiculo_seleccionado
            '                End If
            '            End If
            '        End With
            '    End If
            'End If
        Else
            Response.Redirect("./Login.aspx?to=" + Now.ToString(), False)
            Return
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Session.Timeout = 60
        Dim sbScript As New StringBuilder()
        sbScript.Append("<script language='JavaScript' type='text/javascript'>" + ControlChars.Lf)
        sbScript.Append("<!--" + ControlChars.Lf)
        'sbScript.Append("window.close();" + ControlChars.Lf)
        sbScript.Append("// -->" + ControlChars.Lf)
        sbScript.Append("</script>" + ControlChars.Lf)
        'If Request.QueryString("vid") Is Nothing Then
        'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Onload_closewindow", _
        '                                                      sbScript.ToString(), False)
        'Return
        'End If
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

        'idVehiculoSeleccionado.Value = idvehiculo
        'lbStatusOpciones.Text = "Inicio de recorrido: " + txtFECHAINICIO.Text + " Fin de recorrido: " + _
        '                        txtFECHAFIN.Text
        'If Not IsPostBack Then
        '    If txtFECHAINICIO.Text = String.Empty Then
        '        txtFECHAINICIO.Text = Now.AddHours(-1).ToString("dd/MM/yyyy HH:mm:ss")
        '    End If
        '    If txtFECHAFIN.Text = String.Empty Then
        '        txtFECHAFIN.Text = Now.ToString("dd/MM/yyyy HH:mm:ss")
        '    End If
        '    txtFECHAINICIO.Focus()
        'End If
        setSQLCMD()
        Reload_Data()

    End Sub

    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        Try
            ShowMeTheError()
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

    Private Sub Reload_Data()
        sqldsVehiculoSeleccionado.SelectCommand = g_selectcommand & " WHERE rastreogps_tipoevento.descripcion ILIKE '%" & _
                Request.QueryString("evento") & "%' AND rastreogps_equipo_eventos.habilitado = TRUE " & _
                " AND rastreo_vehiculo.idrastreo_vehiculo IN (SELECT idrastreo_vehiculo FROM rastreo_vehiculo " & _
                " WHERE id_cliente = " & miUsuario.idPersona & ") AND gps_fechahora_reporte BETWEEN @fecha_de_inicio " & _
                " AND @fecha_de_fin ORDER BY rastreo_vehiculo.identificador_rastreo, reportesgps" + Convert.ToDateTime(txtFECHAINICIO.Value).Month.ToString() + Convert.ToDateTime(txtFECHAINICIO.Value).Year.ToString() & ".gps_fechahora_reporte"
        'If Oki Then
        sqldsVehiculoSeleccionado.DataBind()
        gridVehiculoSeleccionado.DataBind()
        ViewState.Remove("IniciadoHistorico")
        'End If
        gridVehiculoSeleccionado.PageSize = 10000
        'ExportExcel()
        'lbStatusOpciones.Text = "Inicio de recorrido: " + txtFECHAINICIO.Text + " Fin de recorrido: " + _
        '                        txtFECHAFIN.Text
    End Sub

    Private Function Tiempo_en_minutos(ByVal rows As Integer) As String
        If rows > 0 Then
            Dim tempo As String = String.Empty
            Dim cant_pto_tiempo As Integer = rows * 3 '3 segundos para obtener direccion?
            Dim minutos As Integer = 0
            Dim segundos As Integer = 0
            minutos = cant_pto_tiempo \ 60
            segundos = cant_pto_tiempo - (minutos * 60)
            tempo = minutos.ToString() + " minutos " + segundos.ToString() + " segundos. aproximadamente"
            Tiempo_en_minutos = tempo
        Else
            Tiempo_en_minutos = "0 segundos."
        End If
    End Function

    Protected Sub btnExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExcel.Click

        ExportExcel()

    End Sub

    Public Overloads Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)

    End Sub

    Public Sub ExportExcel()

        Dim sb As New StringBuilder()
        Dim sw As New StringWriter(sb)
        Dim htw As New HtmlTextWriter(sw)


        htw.Flush()
        'lbStatusOpciones.RenderControl(htw)
        gridVehiculoSeleccionado.RenderControl(htw)
        'pnlResultados.RenderControl(htw)

        HttpContext.Current.Response.Clear()
        HttpContext.Current.Response.Buffer = True
        HttpContext.Current.Response.ContentType = "application/vnd.ms-excel"
        HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" & _
                                               miUsuario.Usuario & "todos_los_moviles_" & Now.ToString("_ddMMyyy_HHmm_") & ".xls")
        HttpContext.Current.Response.Charset = "UTF-8"
        HttpContext.Current.Response.ContentEncoding = Encoding.Default '  New System.Text.UTF8Encoding(False)
        HttpContext.Current.Response.Write(sb.ToString())
        HttpContext.Current.Response.End()

    End Sub

    Public Function gGetReferenciaCercana(ByVal vlat As Double, ByVal vlon As Double) As String
        If Request.QueryString("ref") = "s" Then
            Return Utilidades.GetReferenciaCercana(miUsuario, vlat, vlon)
        Else
            Return String.Empty
        End If
    End Function

    Public Function GeoPos(ByVal pLAT As String, ByVal pLON As String) As String
        Dim RET As String = "--"
        'If chkCallesGOOGLE.Checked Then
        RET = Utilidades.mapserver_ReverseGeocode(pLAT, pLON)
        'End If
        Return RET
    End Function

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

End Class
