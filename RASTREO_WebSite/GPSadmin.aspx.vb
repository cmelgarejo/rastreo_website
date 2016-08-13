Imports Npgsql
Imports System.Data
Imports RASTREOmw
Imports System.IO
Imports System.Drawing.Imaging
Imports MyGeneration.dOOdads

Partial Class GPSadmin
    Inherits System.Web.UI.Page

    Public TipoReporte As String = String.Empty

    Public miUsuario As Usuario = New Usuario
    Public mi_usuario As String = String.Empty
    Public permiso_menu_admin As Boolean = False
    Public permiso_menu_usuarios As Boolean = False
    Public _gidUsuario As String = String.Empty
    Public _gidPersona As String = String.Empty

    'Protected Overloads Overrides Sub Render(ByVal writer As HtmlTextWriter)
    '    Utilidades.CompresionASPX(HttpContext.Current)
    '    MyBase.Render(writer)
    'End Sub

    Protected Sub Page_Error(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Error
        'Dim ex As Exception = Server.GetLastError()
        'ErrHandler.WriteError(ex.Message + " - " + ex.StackTrace)
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            miUsuario = CType(Session("session_UsuarioRASTREO"), Usuario)
            If Not miUsuario Is Nothing Then
                If Not miUsuario.Empleado Then
                    Response.Redirect("Principal.aspx", False)
                End If
                mi_usuario = miUsuario.Nombre
                _gidUsuario = miUsuario.idUsuario
                _gidPersona = miUsuario.idPersona
                If Not IsPostBack Then
                    If miUsuario.idUsuario = 0 Or Utilidades.FindPermissionFor(miUsuario.idUsuario, "MENU_ADMINISTRADOR") Then
                        permiso_menu_admin = True
                        Load_Clientes_vehiculos()
                    ElseIf miUsuario.idUsuario = 0 Or Utilidades.FindPermissionFor(miUsuario.idUsuario, "MENU_USUARIOS") Then
                        permiso_menu_usuarios = True
                    End If
                End If
            Else
                Response.Redirect("Login.aspx?rp=" + Me.Request.Url.AbsoluteUri(), False)
            End If
            ''Esto se puede eliminar, es solo para que seleccione el primer 
            ''cliente y lo filtre para que no salga el chorizo en el mapa...
            If Not IsPostBack Then
                chkFiltro_Por_Cliente.Checked = True
                Bind_IdentificadorFiltro()
                Bind_ClienteFiltro()
            End If

            getPosiciones(miUsuario)
            Rebind_grillas()

            Dim sbScript As New StringBuilder()
            sbScript.Append("<script language='JavaScript' type='text/javascript'>" + ControlChars.Lf)
            sbScript.Append("<!--" + ControlChars.Lf)
            sbScript.Append("g_UpdateMap(false);" + ControlChars.Lf)
            sbScript.Append("// -->" + ControlChars.Lf)
            sbScript.Append("</script>" + ControlChars.Lf)
            sbScript.Append("<script language='JavaScript' type='text/javascript'>" + ControlChars.Lf)
            sbScript.Append("<!--" + ControlChars.Lf)
            sbScript.Append("countdown_timer(" & txtTimeout.Text & ");" + ControlChars.Lf)
            sbScript.Append("// -->" + ControlChars.Lf)
            sbScript.Append("</script>" + ControlChars.Lf)
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Onload_updatemap", _
                                                                  sbScript.ToString(), False)

            If Not gv_Lista_Vehiculos.SelectedValue Is Nothing Then
                idVehiculoSeleccionado.Value = gv_Lista_Vehiculos.SelectedDataKey("idrastreo_vehiculo")
                idPersona.Value = gv_Lista_Vehiculos.SelectedDataKey("idrastreo_persona")
            End If

            LoadReferencias(miUsuario)

        Catch ex As Exception

        Finally
            'GC.Collect()
        End Try
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

        Catch myEX As Exception
            ViewState.Add("msjerror", myEX.Message + "<br>" + myEX.StackTrace)
        End Try
    End Sub

    Private Sub Load_Clientes_vehiculos()
        Try
            Dim pg_Conn As NpgsqlConnection = New NpgsqlConnection(cnn_str.CadenaDeConexion)
            pg_Conn.Open()
            Dim count_Query As String = _
                "SELECT count(*) as conteo_clientes FROM rastreo_cliente WHERE estado_cliente = TRUE"
            Dim pg_SelectCMD As NpgsqlCommand = New NpgsqlCommand(count_Query, pg_Conn)
            pg_SelectCMD.CommandTimeout = 120
            Dim pg_RDR As NpgsqlDataReader = pg_SelectCMD.ExecuteReader(CommandBehavior.SingleRow)
            If pg_RDR.HasRows Then
                pg_RDR.Read()
                If Not lbCantidadClientes Is Nothing Then
                    lbCantidadClientes.Text = String.Empty + Convert.ToString(pg_RDR("conteo_clientes"))
                End If
            End If
            pg_RDR.Close()
            count_Query = _
                "SELECT count(*) as conteo_vehiculos FROM rastreo_vehiculo WHERE activo = TRUE and demo = FALSE"
            pg_SelectCMD.CommandText = count_Query
            pg_RDR = pg_SelectCMD.ExecuteReader(CommandBehavior.SingleRow)
            If pg_RDR.HasRows Then
                pg_RDR.Read()
                If Not lbCantidadVehiculos Is Nothing Then
                    lbCantidadVehiculos.Text = String.Empty + Convert.ToString(pg_RDR("conteo_vehiculos"))
                End If
            End If
            pg_RDR.Close()
            pg_RDR.Close()
            count_Query = _
                "SELECT count(*) as conteo_vehiculos_demo FROM rastreo_vehiculo WHERE activo = TRUE and demo = TRUE"
            pg_SelectCMD.CommandText = count_Query
            pg_RDR = pg_SelectCMD.ExecuteReader(CommandBehavior.SingleRow)
            If pg_RDR.HasRows Then
                pg_RDR.Read()
                If Not lbCantidadVehiculos Is Nothing Then
                    lbCantidadVehiculosDEMO.Text = String.Empty + Convert.ToString(pg_RDR("conteo_vehiculos_demo"))
                End If
            End If
            pg_Conn.Close()
            pg_SelectCMD.Dispose()
            pg_Conn.Dispose()
        Catch ex As Exception
            Session.Add("msjerror", ex.ToString())
        Finally
            'GC.Collect()
        End Try
    End Sub

    Private Sub LogOUT()
        Session.Abandon()
        Response.Redirect("Login.aspx?rp=" + Me.Request.Url.AbsoluteUri(), False)
    End Sub

    Protected Sub RASTREO_LOGOUT_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RASTREO_LOGOUT.Click
        LogOUT()
    End Sub

    Protected Sub getPosiciones(ByVal mi_usuario_pos As Usuario)
        If mi_usuario_pos Is Nothing Or chkFiltro_Todos_Los_Moviles.Checked Then
            txtfile.Value = String.Empty
            Return
        End If
        'Aqui tengo q buscar todos lo vehicuos del usuario y tirarlos en el txt, formato:
        'separacion de los items: TAB
        'point	title	description	icon
        '-2903050.588772871,-6382304.855050304	auto	texto	./App_Themes/CENTRAL/Imagenes/IconosTransporte/auto.png
        '-2903050.588752871,-6682304.855050304	camion	texto	./App_Themes/CENTRAL/Imagenes/IconosTransporte/camion.png
        '-2903050.588752871,-6882304.855050304	bus	texto	./App_Themes/CENTRAL/Imagenes/IconosTransporte/bus.png
        'guardar en un texto y asignarlo a txtfile.value, hiddencontrol
        'dependiendo del login
        Dim tmpLONG As String = String.Empty
        Dim tmpLAT As String = String.Empty
        Dim posPOINT As String = String.Empty ' lat, long
        Dim posTITLE As String = String.Empty ' titulo - contraseña
        Dim posDESCRIPTION As String = String.Empty ' descripcion - informacion
        Dim posICON As String = String.Empty ' png
        Dim ViewVehiculos As New rsview_vehiculo_bandejaentrada_cliente_equipogps(cnn_str.CadenaDeConexion)
        Dim web_file As String = mi_usuario_pos.Usuario + "_" + mi_usuario_pos.IP.Replace(".", "_").Replace(":", "_") + _
                                "_" + Session.SessionID + ".txt"
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
        Try
            With ViewVehiculos
                If chkFiltro_Por_Cliente.Checked Then
                    .Where.Idrastreo_persona.Value = ddlClienteSeleccionado.SelectedValue
                ElseIf chkFiltro_Por_Identificador.Checked Then
                    .Where.Idrastreo_vehiculo.Value = ddlIdentificadorSeleccionado.SelectedValue
                End If
                .Query.Distinct = True
                Dim archivoPOS As New StreamWriter(New FileStream(nom_arch, FileMode.Create, FileAccess.ReadWrite))
                If .Query.Load() Then
                    If .RowCount > 0 Then
                        If .s_Identificador_rastreo <> String.Empty Then
                            If Not Directory.Exists(DirectorioTMP) Then
                                Directory.CreateDirectory(DirectorioTMP)
                            End If
                            'SyncLock archivoPOS
                            archivoPOS.WriteLine("point" + Chr(9) + "title" + Chr(9) + _
                                                    "description" + Chr(9) + "icon" + Chr(9) + _
                                                    "iconSize" + Chr(9) + "iconOffset")
                            While Not .EOF
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
                                                     .s_Identificador_rastreo + _
                                                     Chr(9) + _
                                                     "" + _
                                                     Chr(9) + _
                                                     .s_Icono.Replace("~", ".").Replace("//", "/") + _
                                                     Chr(9) + _
                                                     "24, 20" + _
                                                     Chr(9) + _
                                                     "-6, -20")
                                Dim WIDTH As Integer = .s_Identificador_rastreo.Length * 10
                                Dim HEIGHT As Integer = 16
                                Dim FECHA As String = String.Empty
                                If .s_Gps_fecha <> String.Empty Then
                                    FECHA = .Gps_fecha.ToString()
                                End If
                                archivoPOS.WriteLine(posPOINT + Chr(9) + _
                                                   .s_Identificador_rastreo + _
                                                   Chr(9) + _
                                                   "<div class=""rastreo_botones"">" + _
                                                   "<table width=""100%"" style=""font-size:smaller"" cellspacing=""0"" cellpadding=""0""><tr><td>" + _
                                                   "<tr><td style=""border:solid 1px grey;"">" + _
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
                            'End SyncLock
                            archivoPOS.Close()
                        End If
                    End If
                Else

                End If
                txtfile.Value = web_filename '+ "?reload=1"
                If IsPostBack Then
                    Dim sbScript As New StringBuilder()
                    sbScript.Append("<script language='JavaScript' type='text/javascript'>" + ControlChars.Lf)
                    sbScript.Append("<!--" + ControlChars.Lf)
                    sbScript.Append("g_UpdateMap(false);" + ControlChars.Lf)
                    sbScript.Append("// -->" + ControlChars.Lf)
                    sbScript.Append("</script>" + ControlChars.Lf)
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Onload_updatemap", _
                                                                          sbScript.ToString(), False)
                End If
            End With
        Catch ex As Exception
            Session.Add("msjerror", ex.ToString)
            Return
        End Try
    End Sub

    Protected Sub tmr_Reload_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles tmr_Reload.Tick
        'getPosiciones(miUsuario)
        Reload_Data()
    End Sub

    Protected Sub gv_Lista_Vehiculos_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles gv_Lista_Vehiculos.DataBound
        Reload_Data()
    End Sub

    Protected Sub gv_Lista_Vehiculos_PageIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gv_Lista_Vehiculos.PageIndexChanged
        gv_Lista_Vehiculos.SelectedIndex = -1
        Reload_Data()
    End Sub

    Protected Sub gv_Lista_Vehiculos_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_Lista_Vehiculos.RowDataBound
    End Sub

    Protected Sub gv_Lista_Vehiculos_RowUpdated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdatedEventArgs) Handles gv_Lista_Vehiculos.RowUpdated
        Reload_Data()
    End Sub

    Protected Sub gv_Lista_Vehiculos_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gv_Lista_Vehiculos.SelectedIndexChanged
        Reload_Data()
    End Sub

    Private Sub Reload_Data()
        Try
            If Not gv_Lista_Vehiculos.SelectedDataKey Is Nothing Then
                If Not gv_Lista_Vehiculos.SelectedDataKey("idrastreo_vehiculo") Is Nothing Then
                    idVehiculoSeleccionado.Value = gv_Lista_Vehiculos.SelectedDataKey("idrastreo_vehiculo")
                    idPersona.Value = gv_Lista_Vehiculos.SelectedDataKey("idrastreo_persona")
                    Dim sbScript As New StringBuilder()
                    If gv_Lista_Vehiculos.SelectedIndex >= 0 Then
                        sbScript.Append("<script language='JavaScript' type='text/javascript'>" + ControlChars.Lf)
                        sbScript.Append("<!--" + ControlChars.Lf)
                        sbScript.Append("map_panTo(" + _
                                        gv_Lista_Vehiculos.SelectedDataKey("gps_longitud").ToString().Replace(",", ".") + _
                                            "," + gv_Lista_Vehiculos.SelectedDataKey("gps_latitud").ToString().Replace(",", ".") + _
                                            ");" + ControlChars.Lf)
                        sbScript.Append("// -->" + ControlChars.Lf)
                        sbScript.Append("</script>" + ControlChars.Lf)
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Onload_centermap", _
                                                                  sbScript.ToString(), False)
                    End If
                End If
            End If
        Catch ex As Exception
            Session.Add("msjerror", ex.ToString())
        End Try
    End Sub

    Private Sub Rebind_grillas()
        'sqldsEventos.DataBind()
        'gvEventos.DataBind()
        If chkFiltro_Por_Cliente.Checked And ddlClienteSeleccionado.SelectedValue <> String.Empty Then
            sqlds_Lista_Vehiculos.SelectCommand = _
                "SELECT DISTINCT ON (identificador_rastreo) * FROM rsview_vehiculo_bandejaentrada_cliente_equipogps " + _
                "WHERE idrastreo_persona = " + ddlClienteSeleccionado.SelectedValue
        ElseIf chkFiltro_Por_Identificador.Checked And ddlIdentificadorSeleccionado.SelectedValue <> String.Empty Then
            sqlds_Lista_Vehiculos.SelectCommand = _
                "SELECT DISTINCT ON (identificador_rastreo) * FROM rsview_vehiculo_bandejaentrada_cliente_equipogps " + _
                "WHERE idrastreo_vehiculo = " + ddlIdentificadorSeleccionado.SelectedValue
        ElseIf chkFiltro_Todos_Los_Moviles.Checked Then
            sqlds_Lista_Vehiculos.SelectCommand = _
                "SELECT DISTINCT ON (gps_fecha, identificador_rastreo) * FROM rsview_vehiculo_bandejaentrada_cliente_equipogps ORDER BY gps_fecha, identificador_rastreo;"
        End If
        sqlds_Lista_Vehiculos.DataBind()
        gv_Lista_Vehiculos.DataBind()
        sqldsVehiculoSeleccionado.DataBind()
        gridVehiculoSeleccionado.DataBind()
    End Sub

    Private Sub Bind_ClienteFiltro()
        Dim ListaFiltro As New rsview_datos_personas(cnn_str.CadenaDeConexion)
        With ListaFiltro
            .Query.Distinct = True
            .Query.AddOrderBy(rsview_datos_personas.ColumnNames.Cliente, MyGeneration.dOOdads.WhereParameter.Dir.ASC)
            .Query.Load()
            ddlClienteSeleccionado.DataSource = .DefaultView
            ddlClienteSeleccionado.DataTextField = rsview_datos_personas.ColumnNames.Cliente
            ddlClienteSeleccionado.DataValueField = rsview_datos_personas.ColumnNames.Idrastreo_persona
            ddlClienteSeleccionado.DataBind()
        End With
    End Sub

    Private Sub Bind_IdentificadorFiltro()
        Dim SQL_Query As String = _
            "SELECT DISTINCT ON (identificador_rastreo) * FROM rsview_cliente_vehiculo_equipogps"
        Dim rastrear_conexion As New NpgsqlConnection(cnn_str.CadenaDeConexion)
        rastrear_conexion.Open()
        Dim rastrear_commando As New NpgsqlCommand(SQL_Query, rastrear_conexion)
        Dim rastrear_reader As New NpgsqlDataAdapter(rastrear_commando)
        Dim rastrear_dataset As New DataSet()
        rastrear_reader.Fill(rastrear_dataset)
        With rastrear_dataset
            ddlIdentificadorSeleccionado.DataSource = rastrear_dataset
            ddlIdentificadorSeleccionado.DataTextField = rsview_cliente_vehiculo_equipogps.ColumnNames.Identificador_rastreo
            ddlIdentificadorSeleccionado.DataValueField = rsview_cliente_vehiculo_equipogps.ColumnNames.Idrastreo_vehiculo
            ddlIdentificadorSeleccionado.DataBind()
        End With
    End Sub

    Protected Sub chkFiltro_Todos_Los_Moviles_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkFiltro_Todos_Los_Moviles.CheckedChanged
        If chkFiltro_Todos_Los_Moviles.Checked Then
            chkFiltro_Por_Identificador.Checked = False
            chkFiltro_Por_Cliente.Checked = False
        End If
        Rebind_grillas()
        getPosiciones(miUsuario)
    End Sub

    Protected Sub chkFiltro_Por_Cliente_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkFiltro_Por_Cliente.CheckedChanged
        If chkFiltro_Por_Cliente.Checked Then
            chkFiltro_Por_Identificador.Checked = False
            chkFiltro_Todos_Los_Moviles.Checked = False
            'pnlFiltro_Por_Cliente.Visible = True
        Else
            'pnlFiltro_Por_Cliente.Visible = False
        End If
        Rebind_grillas()
        getPosiciones(miUsuario)
        If Not gv_Lista_Vehiculos.SelectedValue Is Nothing Then
            idVehiculoSeleccionado.Value = gv_Lista_Vehiculos.SelectedDataKey("idrastreo_vehiculo")
            idPersona.Value = gv_Lista_Vehiculos.SelectedDataKey("idrastreo_persona")
        End If
    End Sub

    Protected Sub chkFiltro_Por_Identificador_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkFiltro_Por_Identificador.CheckedChanged
        If chkFiltro_Por_Identificador.Checked Then
            chkFiltro_Por_Cliente.Checked = False
            chkFiltro_Todos_Los_Moviles.Checked = False
            'pnlFiltro_Por_Identificador.Visible = True
        Else
            'pnlFiltro_Por_Identificador.Visible = False
        End If
        Rebind_grillas()
        getPosiciones(miUsuario)
        If Not gv_Lista_Vehiculos.SelectedValue Is Nothing Then
            idVehiculoSeleccionado.Value = gv_Lista_Vehiculos.SelectedDataKey("idrastreo_vehiculo")
            idPersona.Value = gv_Lista_Vehiculos.SelectedDataKey("idrastreo_persona")
        End If
    End Sub

    'Protected Sub gvEventos_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvEventos.DataBound
    '    'llamar a sonidito
    '    'If gvEventos.Rows.Count > 0 Then
    '    '    Dim dir As String = "./sounds/"
    '    '    sndEvento.Archivo_de_Sonido = dir + "panic.mp3"
    '    'Else
    '    '    sndEvento.Archivo_de_Sonido = ""
    '    'End If
    'End Sub

    'Protected Sub gvEventos_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvEventos.RowCommand
    '    Dim idrastreo_avisos As Integer = 0
    '    Dim id_equipogps As Integer = 0
    '    Integer.TryParse(e.CommandArgument, idrastreo_avisos)
    '    If idrastreo_avisos > 0 Then
    '        Dim tblDeleteAviso As New rastreogps_avisos(cnn_str.CadenaDeConexion)
    '        With tblDeleteAviso
    '            .Where.Idrastreogps_avisos.Value = idrastreo_avisos
    '            If .Query.Load() Then
    '                .Atendido = True
    '                id_equipogps = .Id_equipogps
    '                .Save()
    '            End If
    '        End With
    '        Dim tblVehiPosi As New rsview_vehiculo_bandejaentrada_cliente_equipogps(cnn_str.CadenaDeConexion)
    '        With tblVehiPosi
    '            .Where.Id_equipogps.Value = id_equipogps
    '            If .Query.Load() Then
    '                idVehiculoSeleccionado.Value = .Idrastreo_vehiculo
    '                If Not ddlIdentificadorSeleccionado.SelectedItem Is Nothing Then
    '                    chkFiltro_Por_Cliente.Checked = False
    '                    chkFiltro_Por_Identificador.Checked = True
    '                    ddlIdentificadorSeleccionado.SelectedItem.Selected = False
    '                    ddlIdentificadorSeleccionado.Items.FindByValue(.Idrastreo_vehiculo).Selected = True
    '                    Rebind_grillas()
    '                    getPosiciones(miUsuario)
    '                End If
    '                sqldsEventos.DataBind()
    '                sqldsVehiculoSeleccionado.DataBind()
    '                gridVehiculoSeleccionado.DataBind()
    '                gvEventos.DataBind()
    '                Dim sbScript As New StringBuilder()
    '                sbScript.Append("<script language='JavaScript' type='text/javascript'>" + ControlChars.Lf)
    '                sbScript.Append("<!--" + ControlChars.Lf)
    '                sbScript.Append("map_panTo(" + _
    '                                .Gps_longitud.ToString().Replace(",", ".") + "," + _
    '                                .Gps_latitud.ToString().Replace(",", ".") + _
    '                                    ");" + ControlChars.Lf)
    '                sbScript.Append("// -->" + ControlChars.Lf)
    '                sbScript.Append("</script>" + ControlChars.Lf)
    '                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Onload_g_centermap", _
    '                                                          sbScript.ToString(), False)
    '            End If
    '        End With
    '    End If
    'End Sub

    'Protected Sub btnApagarTodosLosPanicos_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnApagarTodosLosEventos.Click
    '    Dim tblAvisos As New rastreogps_avisos(cnn_str.CadenaDeConexion)
    '    With tblAvisos
    '        .Where.Atendido.Value = False
    '        If .Query.Load() Then
    '            While Not .EOF
    '                .Atendido = True
    '                .MoveNext()
    '            End While
    '            .Save()
    '        End If
    '    End With
    '    Rebind_grillas()
    'End Sub

    Public Function GeoPos(ByVal pLAT As String, ByVal pLON As String, Optional ByVal DIR As String = "") As String
        If Not String.IsNullOrEmpty(DIR) Then
            Return DIR
        End If
        Dim RET As String = "--"
        If pLAT <> String.Empty And pLON <> String.Empty Then
            RET = Utilidades.mapserver_ReverseGeocode(pLAT, pLON)
        End If
        Return RET
    End Function

    Protected Sub btnFiltrarGrilla_Por_Cliente_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnFiltrarGrilla_Por_Cliente.Click
        Bind_ClienteFiltro()
    End Sub

    Protected Sub btnFiltro_Por_Identificador_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnFiltro_Por_Identificador.Click
        Bind_IdentificadorFiltro()
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
        Try
            Dim web_file As String = mi_usuario_pos.Usuario + "_" + mi_usuario_pos.IP.Replace(".", "_").Replace(":", "_") + _
                        "_" + Session.SessionID + ".txt"
            Dim web_directory As String = "~/tmp/ref_postxt/"
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
            Dim personadeusuarios As New rastreo_usuarios(cnn_str.CadenaDeConexion)
            personadeusuarios.Where.Su.Value = True
            'If personadeusuarios.Query.Load() Then
            '    If personadeusuarios.RowCount > 0 Then
            '        While Not personadeusuarios.EOF
            '            'hoja_template.Where.Id_persona.
            '            personadeusuarios.MoveNext()
            '        End While
            '    End If
            'Else
            '    hoja_template.Where.Id_persona.Value = miUsuario.idPersona
            'End If
            hoja_template.Where.Rastreoref.Value = True
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
            Session.Add("msjerror", ex.ToString)
            Return String.Empty
        End Try
    End Function

    Public Function mainGetReferenciaCercana(ByVal vlat As String, ByVal vlon As String) As String
        mainGetReferenciaCercana = String.Empty
        If vlat <> String.Empty And vlon <> String.Empty Then
            Dim lat As Double = Double.Parse(vlat)
            Dim lon As Double = Double.Parse(vlon)
            mainGetReferenciaCercana = Utilidades.GetReferenciaCercana(miUsuario, lat, lon)
        End If
        Return mainGetReferenciaCercana
    End Function
End Class