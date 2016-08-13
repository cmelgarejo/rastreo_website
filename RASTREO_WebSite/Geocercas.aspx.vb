Imports RASTREOmw
Imports System.IO
Imports System.Drawing.Imaging
Imports MyGeneration.dOOdads

Partial Class GeoCercas
    Inherits System.Web.UI.Page

    Public idPersona As Integer
    Public idVehiculo As Integer

    Public miUsuario As Usuario
    Public mi_usuario As String = String.Empty
    Public mi_vehiculo As String = String.Empty

    Public permiso_menu_admin As Boolean
    Public permiso_menu_usuarios As Boolean

    Private bTodoElDia As Boolean = False
    Public vehiculo_seleccionado As String = String.Empty

    Protected Overloads Overrides Sub Render(ByVal writer As HtmlTextWriter)
        Utilidades.CompresionASPX(HttpContext.Current)
        MyBase.Render(writer)
    End Sub
    

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim sbScript As New StringBuilder()
        sbScript.Append("<script language='JavaScript' type='text/javascript'>" + ControlChars.Lf)
        sbScript.Append("<!--" + ControlChars.Lf)
        'sbScript.Append("window.close();" + ControlChars.Lf)
        sbScript.Append("// -->" + ControlChars.Lf)
        sbScript.Append("</script>" + ControlChars.Lf)
        If Request.QueryString("cid") Is Nothing Or Request.QueryString("uid") Is Nothing Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Onload_closewindow", _
                                                                  sbScript.ToString(), False)
            Return
        End If
        miUsuario = CType(Session("session_UsuarioRASTREO"), Usuario)
        If Not miUsuario Is Nothing Then
            'If Not miUsuario.Empleado Then
            '    Response.Redirect("Principal.aspx", False)
            'End If
            mi_usuario = CType(Session("session_UsuarioRASTREO"), Usuario).Nombre
            If miUsuario.idUsuario = 0 Or Utilidades.FindPermissionFor(miUsuario.idUsuario, "MENU_ADMINISTRADOR") Then
                permiso_menu_admin = True
            ElseIf miUsuario.idUsuario = 0 Or Utilidades.FindPermissionFor(miUsuario.idUsuario, "MENU_USUARIOS") Then
                permiso_menu_usuarios = True
            End If
        Else
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Onload_closewindow", _
                                                                  sbScript.ToString(), False)
            Return
        End If
        idVehiculo = Request.QueryString("vid")
        idPersona = Request.QueryString("cid")
        If Integer.TryParse(Request.QueryString("vid"), idVehiculo) Then
            Dim tblV As New rastreo_vehiculo(cnn_str.CadenaDeConexion)
            tblV.LoadByPrimaryKey(idVehiculo, idPersona)
            Me.Title += tblV.Identificador_rastreo
        End If
        If IsPostBack And geocerca_puntos.Value <> String.Empty Then
            Reload_Data()
            geocerca_puntos.Value = String.Empty
        End If
        Select Case rblOPCIONESACTIVACION.SelectedValue
            Case 1
                pnlLimitHora.Visible = True
                pnlDiaDeActivacion.Visible = False
            Case 2
                bTodoElDia = True
                pnlLimitHora.Visible = False
                pnlDiaDeActivacion.Visible = False
            Case 3
                pnlLimitHora.Visible = False
                pnlDiaDeActivacion.Visible = True
        End Select
        If Not Session("puntos_geocerca") Is Nothing Then
            geocerca_puntos.Value = Session("puntos_geocerca")
            mensajegeocerca.Value = "Sin cambios en la geocerca."
        End If
        InitPosiciones_Referencias()
    End Sub

    Protected Sub InitPosiciones_Referencias()
        Dim sbScript As New StringBuilder()
        sbScript.Append("<script language='JavaScript' type='text/javascript'>" + ControlChars.Lf)
        sbScript.Append("//<!--" + ControlChars.Lf)
        sbScript.Append("g_UpdateMap(false);" + ControlChars.Lf)
        sbScript.Append("g_UpdateRef(false);" + ControlChars.Lf)
        sbScript.Append("// -->" + ControlChars.Lf)
        sbScript.Append("</script>" + ControlChars.Lf)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Onload_center_reloaddata", _
                                                      sbScript.ToString(), False)
        getPosiciones(miUsuario)
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
                    .Where.Id_usuarios.Value = mi_usuario_pos.idUsuario
                    .Where.Id_vehiculo.Value = idVehiculo
                    .Where.Visible.Value = True
                    .Query.Load()
                End If
                If .RowCount > 0 Then
                    If .s_Identificador_rastreo <> String.Empty Then
                        'Dim web_file As String = mi_usuario_pos.Usuario + mi_usuario_pos.IP.Replace(".", "") + ".pos"
                        Dim web_file As String = mi_usuario_pos.Usuario + _
                                                "_" + mi_usuario_pos.IP.Replace(".", "_") + _
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
                                '"./img/marker-blue.png" + _
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
            Session.Add("msjerror", ex.ToString)
            Return
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

    Private Sub Reload_Data()
        'MakeGeoCercaFile(miUsuario)
        Dim sbScript As New StringBuilder()
        sbScript.Append("<script language='JavaScript' type='text/javascript'>" + ControlChars.Lf)
        sbScript.Append("<!--" + ControlChars.Lf)
        sbScript.Append("my_deserialize_geocerca();" + ControlChars.Lf)
        sbScript.Append("// -->" + ControlChars.Lf)
        sbScript.Append("</script>" + ControlChars.Lf)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Onload_updatemap", _
                                                              sbScript.ToString(), False)
        'sbScript = New StringBuilder()
        'If gv_Lista_Vehiculos.SelectedIndex >= 0 Then
        '    sbScript.Append("<script language='JavaScript' type='text/javascript'>" + ControlChars.Lf)
        '    sbScript.Append("<!--" + ControlChars.Lf)
        '    sbScript.Append("map_panTo(" + _
        '                    gv_Lista_Vehiculos.SelectedDataKey("gps_longitud").ToString().Replace(",", ".") + _
        '                        "," + gv_Lista_Vehiculos.SelectedDataKey("gps_latitud").ToString().Replace(",", ".") + _
        '                        ");" + ControlChars.Lf)
        '    sbScript.Append("// -->" + ControlChars.Lf)
        '    sbScript.Append("</script>" + ControlChars.Lf)
        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Onload_centermap", _
        '                                              sbScript.ToString(), False)
        'End If
    End Sub


    Protected Sub gv_GeoCercas_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gv_GeoCerca.SelectedIndexChanged
        If gv_GeoCerca.SelectedIndex >= 0 Then
            If Not gv_GeoCerca.SelectedDataKey("puntos") Is DBNull.Value Then
                geocerca_puntos.Value = gv_GeoCerca.SelectedDataKey("puntos")
                Reload_Data()
            End If
        End If
    End Sub
    Protected Sub btnCargaGeoCerca_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCargaGeoCerca.Click
        pnlGeoCerca.Visible = Not pnlGeoCerca.Visible
        chkActiva.Checked = True
        chkInversa.Checked = False
        Session.Add("msjerror", "Puede dibujar la geozona en el mapa que desee el vehiculo no abandone.")
    End Sub
    Private Sub LimpiarGeoCerca()
        rec_txtDESC.Text = String.Empty
        chkActiva.Checked = False
        chkInversa.Checked = False
    End Sub
    Private Sub GuardarGeoCerca()
        Dim tblGeoCerca As New rastreo_geocercas(cnn_str.CadenaDeConexion)
        Dim FU As Boolean = False
        Try
            If insGeocercaPuntos.Value <> String.Empty Then
                With tblGeoCerca
                    If Session("updGeoCerca") Then
                        Dim indice As Integer = 0
                        If Integer.TryParse(Session("updGeoCerca_index"), indice) Then
                            .LoadByPrimaryKey(indice)
                            'If .Query.Load() Then
                            If .s_Idrastreo_geocercas <> String.Empty Then
                                .User_upd = miUsuario.idUsuario
                                .Fech_upd = Now
                                Session.Remove("updGeoCerca")
                                Session.Remove("updGeoCerca_index")
                                FU = True
                            End If
                            'End If
                        End If
                    Else
                        .AddNew()
                        .Id_cliente = idPersona
                        .Id_vehiculo = idVehiculo
                        .Id_usuario = miUsuario.idUsuario
                        .User_ins = miUsuario.idUsuario
                        .Fech_ins = Now
                    End If
                    .s_Descripcion = rec_txtDESC.Text.Trim().ToUpperInvariant()
                    .Activa = chkActiva.Checked
                    .Activo_siempre = bTodoElDia
                    If rblOPCIONESACTIVACION.SelectedValue = 3 Then
                        .Dia_activacion = Convert.ToDateTime(txtDiaDeActivacion.Text.Trim())
                    Else
                        .SetColumnNull(rastreo_geocercas.ColumnNames.Dia_activacion)
                    End If
                    .Avisar_entrada = chkInversa.Checked
                    .Aviso_entradasalida = chkIngresoEgreso.Checked
                    .s_Puntos = insGeocercaPuntos.Value
                    If (txtHoraInicio.Text <> String.Empty) Then
                        .s_Hora_activacion = IIf(txtHoraInicio.Text <> String.Empty, txtHoraInicio.Text, Now.ToShortTimeString())
                    End If
                    If (txtHorasActiva.Text <> String.Empty) Then
                        .s_Horas_activa = IIf(txtHorasActiva.Text <> String.Empty, txtHorasActiva.Text, 1)
                    End If
                    .Mails = txtMails.Text.Replace(" ", ";").Replace(",", ";").Trim()
                    .Tel_movil = txtTelMoviles.Text.Replace(" ", ";").Replace(",", ";").Trim()
                    .Save()
                End With
                If chkCopyAllGEOCERCA.Checked Then
                    Dim myvehiculos As New rsview_vehiculo_bandejaentrada_cliente_equipogps(cnn_str.CadenaDeConexion)
                    With myvehiculos
                        .Query.Distinct = True
                        .Where.Id_usuarios.Value = miUsuario.idUsuario
                        If .Query.Load() Then
                            If .RowCount > 0 Then
                                While Not .EOF
                                    If .Id_vehiculo = idVehiculo Then
                                        .MoveNext()
                                        Continue While
                                    End If
                                    tblGeoCerca.FlushData()
                                    If FU Then
                                        tblGeoCerca.Where.Id_usuario.Value = miUsuario.idUsuario
                                        tblGeoCerca.Where.Id_vehiculo.Value = .Id_vehiculo
                                        tblGeoCerca.Where.Id_cliente.Value = idPersona
                                        tblGeoCerca.Where.Puntos.Value = Convert.ToString(Session("old_puntos_geocerca"))
                                        If tblGeoCerca.Query.Load() Then
                                            tblGeoCerca.User_upd = miUsuario.idUsuario
                                            tblGeoCerca.Fech_upd = Now
                                        End If
                                    Else
                                        tblGeoCerca.AddNew()
                                    End If
                                    tblGeoCerca.Id_usuario = miUsuario.idUsuario
                                    tblGeoCerca.Id_cliente = idPersona
                                    tblGeoCerca.Id_vehiculo = .Id_vehiculo
                                    tblGeoCerca.User_ins = miUsuario.idUsuario
                                    tblGeoCerca.Fech_ins = Now
                                    tblGeoCerca.Descripcion = rec_txtDESC.Text.Trim().ToUpperInvariant()
                                    tblGeoCerca.Activa = chkActiva.Checked
                                    tblGeoCerca.Activo_siempre = bTodoElDia
                                    If rblOPCIONESACTIVACION.SelectedValue = 3 Then
                                        tblGeoCerca.Dia_activacion = Convert.ToDateTime(txtDiaDeActivacion.Text.Trim())
                                    Else
                                        tblGeoCerca.SetColumnNull(rastreo_geocercas.ColumnNames.Dia_activacion)
                                    End If
                                    tblGeoCerca.Avisar_entrada = chkInversa.Checked
                                    tblGeoCerca.Aviso_entradasalida = chkIngresoEgreso.Checked
                                    tblGeoCerca.s_Puntos = insGeocercaPuntos.Value
                                    tblGeoCerca.s_Hora_activacion = IIf(txtHoraInicio.Text <> String.Empty, txtHoraInicio.Text, Now.ToShortTimeString())
                                    tblGeoCerca.s_Horas_activa = IIf(txtHorasActiva.Text <> String.Empty, txtHorasActiva.Text, 1)
                                    tblGeoCerca.Mails = txtMails.Text.Replace(" ", ";").Replace(",", ";").Trim()
                                    tblGeoCerca.Tel_movil = txtTelMoviles.Text.Replace(" ", ";").Replace(",", ";").Trim()
                                    tblGeoCerca.Save()
                                    .MoveNext()
                                End While
                            End If
                        End If
                    End With
                End If
                sqlds_GeoCerca.DataBind()
                gv_GeoCerca.DataBind()
                LimpiarGeoCerca()
                Session.Add("msjerror", "Geocerca guardada.")
                pnlGeoCerca.Visible = Not pnlGeoCerca.Visible
            Else
                Session.Add("msjerror", "Por favor, diseñe una geocerca y luego seleccionela para guardarla.")
            End If
        Catch ex As Exception
            Session.Add("msjerror", ex.ToString())
        End Try
    End Sub
    Protected Sub gv_GeoCerca_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_GeoCerca.RowCommand
        Try
            If e.CommandName = "Editar" Then
                Session.Add("updGeoCerca", True)
                Session.Add("updGeoCerca_index", e.CommandArgument)
                Dim tblLoad As New rastreo_geocercas(cnn_str.CadenaDeConexion)
                With tblLoad
                    .LoadByPrimaryKey(e.CommandArgument)
                    If Not .Puntos Is DBNull.Value Then
                        geocerca_puntos.Value = .Puntos
                        If Session("puntos_geocerca") Is Nothing Then
                            Session.Add("puntos_geocerca", .Puntos)
                            Session.Add("old_puntos_geocerca", .Puntos)
                        Else
                            Session("puntos_geocerca") = .Puntos
                            Session("old_puntos_geocerca") = .Puntos
                        End If
                    End If
                    rec_txtDESC.Text = .s_Descripcion
                    chkActiva.Checked = .Activa
                    chkInversa.Checked = .Avisar_entrada
                    chkIngresoEgreso.Checked = .Aviso_entradasalida
                    Try
                        txtHoraInicio.Text = .Hora_activacion.ToShortTimeString()
                    Catch ex As Exception
                        txtHoraInicio.Text = String.Empty
                    End Try
                    Try
                        txtHorasActiva.Text = .Horas_activa.ToString()
                    Catch ex As Exception
                        txtHorasActiva.Text = String.Empty
                    End Try
                    txtMails.Text = .Mails
                    txtTelMoviles.Text = .Tel_movil
                End With

                pnlGeoCerca.Visible = True
            ElseIf e.CommandName = "Eliminar" Then
                Dim tblDel As New rastreo_geocercas(cnn_str.CadenaDeConexion)
                With tblDel
                    .LoadByPrimaryKey(e.CommandArgument)
                    .MarkAsDeleted()
                    .Save()
                End With
            End If
            sqlds_GeoCerca.DataBind()
            gv_GeoCerca.DataBind()
            Reload_Data()
        Catch ex As Exception
            Session.Add("msjerror", ex.ToString())
        End Try
    End Sub
    Protected Sub btnSAVEREC_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSAVEREC.Click
        GuardarGeoCerca()
        pnlGeoCerca.Visible = False
    End Sub
    Protected Sub btnCANCELREC_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCANCELREC.Click
        pnlGeoCerca.Visible = False
    End Sub

    Protected Sub gv_GeoCerca_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_GeoCerca.RowDataBound
        If gv_GeoCerca.SelectedIndex >= 0 Then
            If Not gv_GeoCerca.SelectedDataKey Is Nothing Then
                If Not gv_GeoCerca.SelectedDataKey("puntos") Is DBNull.Value Then
                    geocerca_puntos.Value = gv_GeoCerca.SelectedDataKey("puntos")
                End If
            End If
            Reload_Data()
        End If
    End Sub

    'Public Overloads Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)

    'End Sub

End Class