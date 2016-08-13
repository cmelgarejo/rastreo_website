Imports RASTREOmw
Imports System.IO
Imports System.Drawing.Imaging
Imports System.IO.Compression
Imports MyGeneration.dOOdads

Partial Class bus_main
    Inherits System.Web.UI.Page

    Public miUsuario As Usuario
    Public mi_usuario As String = String.Empty
    Public mi_cliente As String = String.Empty

    Public permiso_menu_usuarios As Boolean
    Public permiso_HOJADERUTA As Boolean
    Public permiso_GEOCERCA As Boolean
    Public permiso_REFERENCIAS As Boolean
    Public permiso_BUSES As Boolean

    Protected Overloads Overrides Sub Render(ByVal writer As HtmlTextWriter)
        Utilidades.CompresionASPX(HttpContext.Current)
        MyBase.Render(writer)
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
                    Response.Redirect("Principal.aspx", False)
                End If
                mi_usuario = CType(Session("session_UsuarioRASTREO"), Usuario).NombreUsuario
                mi_cliente = CType(Session("session_UsuarioRASTREO"), Usuario).Nombre
                If Utilidades.FindPermissionFor(miUsuario.idUsuario, "MENU_USUARIOS") Then
                    permiso_menu_usuarios = True
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
            Else
                Response.Redirect("./Login.aspx?to=" + Now.ToString(), False)
                Return
            End If

            idUsuario.Value = miUsuario.idUsuario
            idPersona.Value = miUsuario.idPersona
            If IsPostBack Then
                If Not Session("selected_vehiculo_main") Is Nothing Then
                    'idVehiculoSeleccionado.Value = Session("selected_vehiculo_main")
                    Reload_Data(True)
                Else
                    Reload_Data()
                End If
            End If
            Dim sbScript As New StringBuilder()
            sbScript.Append("<script language='JavaScript' type='text/javascript'>" + ControlChars.Lf)
            sbScript.Append("//<!--" + ControlChars.Lf)
            sbScript.Append("countdown_timer(" & txtTimeout.Text & ");" & ControlChars.Lf)
            sbScript.Append("// -->" + ControlChars.Lf)
            sbScript.Append("</script>" + ControlChars.Lf)
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Onload_updatecountdown", _
                                                                  sbScript.ToString(), False)

            txtfile_ida.Value = bus_getPosiciones(miUsuario, True)
            txtfile_vuelta.Value = bus_getPosiciones(miUsuario)

            reffile.Value = LoadReferencias(miUsuario)

            tmr_Reload.Interval = Convert.ToInt32(txtTimeout.Text) * 1000
        Catch ex As Exception

        Finally
            'GC.Collect()
        End Try
    End Sub

    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        Try
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
            Session.Add("msjerror", myEX.Message + "<br>" + myEX.StackTrace)
        End Try
    End Sub

    Private Sub LogOUT()
        Session.Abandon()
        Response.Redirect("Login.aspx?rp=" + Me.Request.Url.AbsoluteUri(), False)
    End Sub

    Protected Sub RASTREO_LOGOUT_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RASTREO_LOGOUT.Click
        LogOUT()
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="mi_usuario_pos"></param>
    ''' <param name="IDA">Si son las posiciones de IDA lo que se necesita, 
    ''' llamar con este valor en TRUE, si son las vuelta, dejarla en FALSE</param>
    ''' <remarks></remarks>
    Private Function bus_getPosiciones(ByVal mi_usuario_pos As Usuario, Optional ByRef IDA As Boolean = False) As String
        'Aqui tengo q buscar todos lo vehicuos del usuario y tirarlos en el txt, formato:
        'separacion de los items: TAB
        'point	title	description	icon
        '-2903050.588772871,-6382304.855050304	auto	texto	./App_Themes/CENTRAL/Imagenes/IconosTransporte/auto.png
        '-2903050.588752871,-6682304.855050304	camion	texto	./App_Themes/CENTRAL/Imagenes/IconosTransporte/camion.png
        '-2903050.588752871,-6882304.855050304	bus	texto	./App_Themes/CENTRAL/Imagenes/IconosTransporte/bus.png
        'guardar en un texto y asignarlo a txtfile.value, hiddencontrol
        'dependiendo del login
        bus_getPosiciones = String.Empty
        If mi_usuario_pos Is Nothing Then
            Return bus_getPosiciones
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
                    ''Esto hay que internacionalizar.
                    .Where.Gps_obs.Operator = WhereParameter.Operand.ILike
                    If IDA Then
                        .Where.Gps_obs.Value = "%" & "IDA" & "%"
                    Else
                        .Where.Gps_obs.Value = "%" & "VUELTA" & "%"
                    End If
                    .Query.Load()
                End If
                If .RowCount > 0 Then
                    If .s_Identificador_rastreo <> String.Empty Then
                        Dim web_file As String = mi_usuario_pos.Usuario + _
                                                "_" + mi_usuario_pos.IP.Replace(".", "_") + _
                                                "_" + Session.SessionID + Convert.ToString(IIf(IDA, "ida", "vuelta")) + _
                                                ".txt"
                        Dim web_directory As String = "~/tmp/bus_postxt/"
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
                                tmpLONG = Convert.ToString(.Gps_longitud).Replace(",", ".")
                                tmpLAT = Convert.ToString(.Gps_latitud).Replace(",", ".")
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
                                Dim WIDTH As Integer = .s_Identificador_rastreo.Length * 10
                                Dim HEIGHT As Integer = 16
                                archivoPOS.WriteLine(posPOINT + Chr(9) + _
                                                   .s_Identificador_rastreo + "<img width=""32px"" Height=""32px"" src=""" + .s_Icono.Replace("~", ".") + """>" + _
                                                   Chr(9) + _
                                                   "<div class=""rastreo_botones"">" + _
                                                   "<table width=""100%"" style=""font-size:smaller"" cellspacing=""0"" cellpadding=""0""><tr><td>" + _
                                                   "<tr><td style=""border:solid 1px grey;"">" + _
                                                   "Fecha y hora:<td/><td style=""border:solid 1px grey;"">" + .Gps_fecha.ToString() + "<td/><tr/>" + _
                                                   "<tr><td style=""border:solid 1px grey;"">" + _
                                                   "Estado:<td/><td style=""border:solid 1px grey;"">" + .s_Evento + "<td/><tr/>" + _
                                                   "<tr><td style=""border:solid 1px grey;"">" + _
                                                   "Velocidad:<td/><td style=""border:solid 1px grey;"">" + .s_Gps_velocidad + "<td/><tr/>" + _
                                                   "<tr><td style=""border:solid 1px grey;"">" + _
                                                   "Rumbo:<td/><td style=""border:solid 1px grey;"">" + .s_Gps_rumbo.ToString() + "° " + .s_Rumbo + "<td/><tr/>" + _
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
                        End SyncLock
                        archivoPOS.Close()
                        bus_getPosiciones = web_filename ' + "?reload=1"
                    End If
                End If
            End With
            Return bus_getPosiciones
        Catch ex As Exception
            Session.Add("msjerror", ex.ToString)
            Return bus_getPosiciones
        End Try
    End Function

    Public Function LoadReferencias(ByVal mi_usuario_pos As Usuario) As String
        LoadReferencias = String.Empty
        If mi_usuario_pos Is Nothing Then
            Return LoadReferencias
        End If
        Dim tmpLONG As String = String.Empty
        Dim tmpLAT As String = String.Empty
        Dim posPOINT As String = String.Empty ' lat, long
        Dim posTITLE As String = String.Empty ' titulo - contraseña
        Dim posDESCRIPTION As String = String.Empty ' descripcion - informacion
        Dim posICON As String = String.Empty ' png
        Dim puntos_hojaderuta As New rastreo_template_puntos_hoja_de_ruta(cnn_str.CadenaDeConexion)
        Try
            Dim web_file As String = mi_usuario_pos.Usuario + "_" + mi_usuario_pos.IP.Replace(".", "_") + _
                        "_" + Session.SessionID + ".txt"
            Dim web_directory As String = "~/tmp/bus_ref_postxt/"
            Dim web_filename As String = web_directory.Replace("~", ".") + web_file
            Dim hoja_template As New rastreo_template_hoja_de_ruta(cnn_str.CadenaDeConexion)
            Dim DirectorioTMP As String = MapPath(web_directory)
            Dim nom_arch As String = DirectorioTMP + "\" + web_file
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
                                                             "</table>" + _
                                                             "</div>" + _
                                                             Chr(9) + _
                                                             "./img/marker-green.png" + _
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
                    LoadReferencias = web_filename '+ "?reload=1"

                End If
            End If
            Return LoadReferencias
        Catch ex As Exception
            Session.Add("msjerror", ex.ToString)
            Return String.Empty
        End Try
    End Function

    Protected Sub tmr_Reload_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles tmr_Reload.Tick
    End Sub

    Private Sub Centrar_Mapa(ByVal lat As Double, ByVal lon As Double)
        Dim sbScript As New StringBuilder()
        sbScript.Append("<script language='JavaScript' type='text/javascript'>" + ControlChars.Lf)
        sbScript.Append("//<!--" + ControlChars.Lf)
        sbScript.Append("busida_map_panTo(" + _
                                      lon.ToString().Replace(",", ".") + _
                                "," + lat.ToString().Replace(",", ".") + _
                                ");" + ControlChars.Lf)
        sbScript.Append("busvuelta_map_panTo(" + _
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
        'sqlVehiculosIDA.DataBind()
        'gv_ListaVehiculos_IDA.DataBind()
        'If seguimiento Then
        '    'sqldsVehiculoSeleccionado.DataBind()
        '    'gridVehiculoSeleccionado.DataBind()
        '    gv_ListaVehiculos_IDA.DataSourceID = Nothing
        '    gv_ListaVehiculos_IDA.DataBind()
        '    gv_ListaVehiculos_IDA.EmptyDataText = "Para volver a ver los demas vehiculos, detenga el seguimiento actual."
        '    gv_ListaVehiculos_VUELTA.DataSourceID = Nothing
        '    gv_ListaVehiculos_VUELTA.DataBind()
        '    gv_ListaVehiculos_VUELTA.EmptyDataText = "Para volver a ver los demas vehiculos, detenga el seguimiento actual."
        '    'If gridVehiculoSeleccionado.Rows.Count > 0 Then
        '    '    gridVehiculoSeleccionado.SelectedIndex = 0
        '    '    Centrar_Mapa(gridVehiculoSeleccionado.SelectedDataKey("gps_latitud"), _
        '    '                 gridVehiculoSeleccionado.SelectedDataKey("gps_longitud"))
        '    'End If
        'Else
        Try
            If IsPostBack Then
                sqlVehiculosIDA.DataBind()
                gv_ListaVehiculos_IDA.DataSourceID = "sqlVehiculosIDA"
                gv_ListaVehiculos_IDA.DataBind()
                gv_ListaVehiculos_IDA.EmptyDataText = "No tiene vehiculos asignados o en viaje de IDA."
                gv_ListaVehiculos_VUELTA.DataSourceID = "sqlVehiculosVUELTA"
                gv_ListaVehiculos_VUELTA.DataBind()
                gv_ListaVehiculos_VUELTA.EmptyDataText = "No tiene vehiculos asignados o en viaje de VUELTA."
                'idVehiculoSeleccionado.Value = "0"
                'sqldsVehiculoSeleccionado.DataBind()
                'gridVehiculoSeleccionado.DataBind()
            End If
            'End If
            Dim sbScript As New StringBuilder()
            sbScript.Append("<script language='JavaScript' type='text/javascript'>" + ControlChars.Lf)
            sbScript.Append("//<!--" + ControlChars.Lf)
            sbScript.Append("gbus_UpdateMap(false);" + ControlChars.Lf)
            sbScript.Append("// -->" + ControlChars.Lf)
            sbScript.Append("</script>" + ControlChars.Lf)
            sbScript.Append("<script language='JavaScript' type='text/javascript'>" + ControlChars.Lf)
            sbScript.Append("//<!--" + ControlChars.Lf)
            sbScript.Append("gbus_UpdateRef(false);" + ControlChars.Lf)
            sbScript.Append("// -->" + ControlChars.Lf)
            sbScript.Append("</script>" + ControlChars.Lf)
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Onload_center_reloaddata", _
                                                          sbScript.ToString(), False)

        Catch ex As Exception

        End Try
    End Sub

    Protected Sub gv_ListaVehiculos_IDA_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gv_ListaVehiculos_IDA.SelectedIndexChanged
        'If gv_ListaVehiculos_IDA.SelectedIndex >= 0 Then
        '    idVehiculoSeleccionado.Value = gv_ListaVehiculos_IDA.SelectedDataKey("id_vehiculo")
        '    If Session("old_selected_vehiculo_main") Is Nothing Then
        '        Session.Add("old_selected_vehiculo_main", gv_ListaVehiculos_IDA.SelectedDataKey("id_vehiculo"))
        '        Session.Add("msjerror", "Para volver a activar la actualizacion de los demas moviles, finalize el seguimiento actual.")
        '    Else
        '        If gv_ListaVehiculos_IDA.SelectedDataKey("id_vehiculo") = Session("old_selected_vehiculo_main") Then
        '            gv_ListaVehiculos_IDA.SelectedIndex = -1
        '            idVehiculoSeleccionado.Value = "0"
        '            Session("old_selected_vehiculo_main") = 0
        '        Else
        '            Session.Add("msjerror", "Para volver a activar la actualizacion de los demas moviles, finalize el seguimiento actual.")
        '            Session("old_selected_vehiculo_main") = gv_ListaVehiculos_IDA.SelectedDataKey("id_vehiculo")
        '            SetMapCenter()
        '        End If
        '    End If
        'Else
        '    Session("old_selected_vehiculo_main") = 0
        '    idVehiculoSeleccionado.Value = "0"
        'End If
        'If gv_ListaVehiculos_IDA.SelectedIndex >= 0 And gv_ListaVehiculos_IDA.Rows.Count > 0 Then
        '    Session.Add("selected_vehiculo_main", gv_ListaVehiculos_IDA.SelectedDataKey("id_vehiculo"))
        '    gv_ListaVehiculos_IDA.SelectedIndex = -1
        '    'Reload_Data(True)
        '    tmr_Reload.Interval = 1
        'End If
    End Sub

    Public Function gGetReferenciaCercana(ByVal vlat As Double, ByVal vlon As Double) As String
        Return Utilidades.GetReferenciaCercana(miUsuario, vlat, vlon)
    End Function

    Public Function GeoPos(ByVal pLAT As String, ByVal pLON As String) As String
        Dim RET As String = "--"
        If chkCalles.Checked Then
            RET = Utilidades.mapserver_ReverseGeocode(pLAT, pLON)
        End If
        Return RET
    End Function

    'Protected Sub gridVehiculoSeleccionado_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gridVehiculoSeleccionado.RowCommand
    '    If e.CommandName = "selected_vehiculo_main" Then
    '        Session.Remove("selected_vehiculo_main")
    '        gv_ListaVehiculos_IDA.SelectedIndex = -1
    '        'Reload_Data()
    '        tmr_Reload.Interval = 1
    '    End If
    'End Sub
End Class