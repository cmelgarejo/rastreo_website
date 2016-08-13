Imports RASTREOmw
Imports System.IO
Imports System.Data
Imports System.Drawing.Color
Imports System.Collections.Generic
Imports System.Runtime.InteropServices
Imports MyGeneration.dOOdads
Imports Npgsql

Partial Class get_fuel_consumption
    Inherits System.Web.UI.Page

    Public miUsuario As Usuario
    Public mi_usuario As String = String.Empty
    Public permiso_menu_usuarios As Boolean
    Public vehiculo_seleccionado As String = String.Empty
    Dim idvehiculo As New List(Of Integer)
    Dim idequipogps As Integer = 0
    Dim g_RowCount As Integer = 0
    Private g_selectcommand As String = _
                "SELECT DISTINCT ON (identificador_rastreo, rastreogps_reportes.gps_fechahora_reporte) " & _
                "	rastreogps_tipoevento.idrastreogps_tipo_evento as id_evento," & _
                "	CASE " & _
                "	    WHEN rastreogps_tipoevento.descripcion IS NULL THEN" & _
                "        rastreogps_reportes.Gps_evento" & _
                "	    WHEN rastreogps_tipoevento.descripcion IS NOT NULL THEN" & _
                "        rastreogps_tipoevento.Descripcion" & _
                "	END as evento," & _
                "	rastreogps_tipoevento.color as color_evento," & _
                "	rastreogps_equipo_eventos.habilitado as evento_habilitado," & _
                "	rastreogps_reportes.idrastreogps_reportes, rastreogps_reportes.id_equipogps, " & _
                "	rastreogps_reportes.gps_longitud, rastreogps_reportes.gps_latitud, " & _
                "	rastreogps_reportes.gps_fechahora_reporte, rastreogps_reportes.gps_velocidad," & _
                "	rastreogps_reportes.gps_rumbo, " & _
                "	CASE" & _
                "	    WHEN (rastreogps_reportes.gps_rumbo >= 0   AND rastreogps_reportes.gps_rumbo <= 23 ) OR" & _
                "		 (rastreogps_reportes.gps_rumbo >= 345 AND rastreogps_reportes.gps_rumbo <= 360) THEN " & _
                "		('NORTE'::text)" & _
                "	    WHEN rastreogps_reportes.gps_rumbo > 23 AND rastreogps_reportes.gps_rumbo <= 69 THEN " & _
                "		('NORESTE'::text)" & _
                "	    WHEN rastreogps_reportes.gps_rumbo > 69 AND rastreogps_reportes.gps_rumbo <= 115 THEN " & _
                "	    ('ESTE'::text)" & _
                "	    WHEN rastreogps_reportes.gps_rumbo > 115 AND rastreogps_reportes.gps_rumbo <= 161 THEN " & _
                "	    ('SURESTE'::text)" & _
                "	    WHEN rastreogps_reportes.gps_rumbo > 161 AND rastreogps_reportes.gps_rumbo <= 207 THEN " & _
                "	    ('SUR'::text)" & _
                "	    WHEN rastreogps_reportes.gps_rumbo > 207 AND rastreogps_reportes.gps_rumbo <= 253 THEN " & _
                "	    ('SUROESTE'::text)" & _
                "	    WHEN rastreogps_reportes.gps_rumbo > 253 AND rastreogps_reportes.gps_rumbo <= 299 THEN " & _
                "	    ('OESTE'::text)	" & _
                "	    WHEN rastreogps_reportes.gps_rumbo > 299 AND rastreogps_reportes.gps_rumbo <= 345 THEN " & _
                "	    ('NOROESTE'::text)" & _
                "	END as rumbo, " & _
                "	rastreogps_reportes.gps_evento," & _
                "	rastreogps_reportes.gps_edaddeldato, rastreogps_reportes.gps_hdop, " & _
                "	rastreogps_reportes.gps_satstatus, rastreogps_reportes.gps_gsmstatus, " & _
                "	rastreogps_reportes.gps_estado_io, rastreogps_reportes.gps_tipodeposicion, " & _
                "	rastreogps_reportes.gps_ip, rastreogps_reportes.gps_obs," & _
                "   rastreo_persona.idrastreo_persona AS idcliente, " & _
                "   rastreo_vehiculo.identificador_rastreo, rastreo_vehiculo.idrastreo_vehiculo AS id_vehiculo, " & _
                "   rastreo_vehiculo.consumo_aprox, " & _
                "   rastreogps_equipogps.idrastreogps_equipogps AS idequipogps, " & _
                "   rastreogps_tipoequipo.idrastreogps_tipoequipo AS id_tipoequipogps, rastreogps_tipoequipo.tipo_equipo AS tipoequipogps, " & _
                "   rastreogps_equipogps.id_equipo_gps, " & _
                "   rastreogps_equipogps.Nro_serie_gps" & _
                "   FROM rastreogps_reportes " & _
                "LEFT JOIN rastreo_vehiculo 			ON rastreogps_reportes.id_equipogps  = rastreo_vehiculo.id_equipogps " & _
                "LEFT JOIN rastreo_persona 			ON rastreo_vehiculo.id_cliente       = rastreo_persona.idrastreo_persona " & _
                "LEFT JOIN rastreo_cliente 			ON rastreo_vehiculo.id_cliente = rastreo_cliente.id_cliente " & _
                "LEFT JOIN rastreogps_equipogps 		ON rastreo_vehiculo.id_equipogps = rastreogps_equipogps.idrastreogps_equipogps " & _
                "LEFT JOIN rastreogps_tipoequipo 		ON rastreogps_equipogps.tipoequipo = rastreogps_tipoequipo.idrastreogps_tipoequipo " & _
                "LEFT JOIN rastreogps_tipoevento     ON rastreogps_tipoevento.id_tipoequipo = rastreogps_equipogps.tipoequipo " & _
                "   AND rastreogps_tipoevento.evento = rastreogps_reportes.gps_evento " & _
                "LEFT JOIN rastreogps_equipo_eventos ON rastreogps_equipo_eventos.id_equipogps = rastreogps_equipogps.idrastreogps_equipogps " & _
                "   AND rastreogps_tipoevento.id_tipoequipo = rastreogps_equipogps.tipoequipo "

    Protected Overloads Overrides Sub Render(ByVal writer As HtmlTextWriter)
        Utilidades.CompresionASPX(HttpContext.Current)
        MyBase.Render(writer)
    End Sub

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        miUsuario = CType(Session("session_UsuarioRASTREO"), Usuario)
        If Not miUsuario Is Nothing Then
            mi_usuario = CType(Session("session_UsuarioRASTREO"), Usuario).NombreUsuario
        Else
            Response.Redirect("./Login.aspx?to=" + Now.ToString(), False)
            Return
        End If

        If Request.QueryString.Keys.Count > 0 Then

        Else
            'Response.Redirect("./Login.aspx?to=" + Now.ToString(), False)
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
        sqldsVehiculoSeleccionado.SelectCommand = g_selectcommand & _
        " WHERE rastreogps_equipo_eventos.habilitado = TRUE AND rastreo_vehiculo.idrastreo_vehiculo IN (SELECT idrastreo_vehiculo FROM rastreo_vehiculo WHERE id_cliente = " & miUsuario.idPersona & ") AND gps_fechahora_reporte BETWEEN @fecha_de_inicio AND @fecha_de_fin ORDER BY rastreo_vehiculo.identificador_rastreo, rastreogps_reportes.gps_fechahora_reporte"
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
        HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" & miUsuario.Usuario & "todos_los_moviles_" & Now.ToString("_ddMMyyy_HHmm_") & ".xls")
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

    Public Function Resumen_vehiculo(ByVal id_Vehiclo As String) As String
        Resumen_vehiculo = String.Empty
        Return Resumen_vehiculo
    End Function

End Class
