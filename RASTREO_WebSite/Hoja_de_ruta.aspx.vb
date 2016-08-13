Imports RASTREOmw
Imports System.IO
Imports MyGeneration.dOOdads

Partial Class Hoja_de_Ruta
    Inherits System.Web.UI.Page

    Public idPersona As Integer
    Public idVehiculo As Integer
    Public idUsuario As Integer
    Public vehiculo_seleccionado As String
    Public miUsuario As Usuario
    Public mi_usuario As String = String.Empty
    Public permiso_menu_admin As Boolean
    Public permiso_menu_usuarios As Boolean

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
        If Request.QueryString("uid") Is Nothing Or Request.QueryString("cid") Is Nothing Or _
            Request.QueryString("vid") Is Nothing Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Onload_closewindow", _
                                                                  sbScript.ToString(), False)
            Return
        End If
        miUsuario = CType(Session("session_UsuarioRASTREO"), Usuario)
        If Not miUsuario Is Nothing Then
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
        If Not Request.QueryString("vid") Is Nothing Then
            If Integer.TryParse(Request.QueryString("vid"), idVehiculo) Then
                Dim tblV As New rastreo_vehiculo(cnn_str.CadenaDeConexion)
                With tblV
                    .Where.Idrastreo_vehiculo.Value = idVehiculo
                    If .Query.Load() Then
                        If .s_Identificador_rastreo <> String.Empty Then
                            vehiculo_seleccionado = .Identificador_rastreo
                            Page.Title = "RASTREO Paraguay - Hoja de ruta para el vehiculo: " + vehiculo_seleccionado
                        End If
                    End If
                End With
            End If
        End If
        idPersona = Request.QueryString("cid")
        idUsuario = Request.QueryString("uid")
        If IsPostBack Then
            Reload_Data()
        Else
            BindDDLBusqueda()
        End If
    End Sub

    Private Function BindearGrilla_PUNTOS(Optional ByVal TodosLosRegistros As Boolean = False) As Boolean
        Dim tblSearch As New rastreo_hoja_de_ruta_puntos(cnn_str.CadenaDeConexion)
        Try
            With tblSearch
                If TodosLosRegistros Then
                    gv_Lista_Puntos_Recorrido.DataSourceID = sqlds_ListaPuntosRecorrido.ID
                    Return True
                Else
                    Dim Columna As String = Me.ddlBuscar.SelectedItem.Value.ToString().Trim()
                    Dim Valor As String = Me.txtBuscar.Text
                    If Valor <> String.Empty And Columna <> String.Empty Then
                        If Columna <> String.Empty Then ' Se ha seleccionado alguna columna por la cual buscar?
                            Dim MyWhere As New WhereParameter(Columna, _
                                New Npgsql.NpgsqlParameter("@" & Columna, Valor))
                            MyWhere.Value = "%" & Valor & "%"
                            MyWhere.Operator = WhereParameter.Operand.ILike
                            .Query.AddWhereParameter(MyWhere)
                            .Where.Idhoja_de_ruta.Value = idhoja_de_ruta.Value
                            .Query.Load()
                            gv_Lista_Puntos_Recorrido.DataSourceID = String.Empty
                            gv_Lista_Puntos_Recorrido.DataSource = .DefaultView
                        End If
                    End If
                End If


                If .RowCount < 1 Then
                    gv_Lista_Puntos_Recorrido.DataBind()
                    Session.Add("msjerror", "La busqueda no produjo ningun resultado.")
                    Return False
                End If
                Session.Add("msjerror", .DefaultView.Count.ToString() + " registro(s) encontrado(s).")
                gv_Lista_Puntos_Recorrido.DataBind()
                Return True
            End With


        Catch MYEX As Exception
            Session.Add("msjerror", MYEX.ToString())
            Return False
        End Try
    End Function

    Private Sub BindDDLBusqueda()
        Try
            With ddlBuscar
                .Items.Add(New ListItem("Nombre", rastreo_hoja_de_ruta_puntos.ColumnNames.Nombre))
                .Items.Add(New ListItem("Descripcion", rastreo_hoja_de_ruta_puntos.ColumnNames.Descripcion))
            End With
        Catch MYEX As Exception
            Session.Add("msjerror", MYEX.Message + "<br>" + MYEX.StackTrace)
        End Try
    End Sub

    Protected Sub btnBuscar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        BindearGrilla_PUNTOS()
    End Sub

    Protected Sub btnLimpiarBusqueda_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLimpiarBusqueda.Click
        BindearGrilla_PUNTOS(True)
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

    Public Function Makehoja_de_rutaFile(ByVal mi_usuario_pos As Usuario) As String
        'Aqui tengo q buscar todos lo vehicuos del usuario y tirarlos en el txt, formato:
        'separacion de los items: TAB
        'point	title	description	icon
        '-2903050.588772871,-6382304.855050304	auto	texto	./App_Themes/CENTRAL/Imagenes/IconosTransporte/auto.png
        '-2903050.588752871,-6682304.855050304	camion	texto	./App_Themes/CENTRAL/Imagenes/IconosTransporte/camion.png
        '-2903050.588752871,-6882304.855050304	bus	texto	./App_Themes/CENTRAL/Imagenes/IconosTransporte/bus.png
        'guardar en un texto y asignarlo a txtfile.value, hiddencontrol
        'dependiendo del login
        If mi_usuario_pos Is Nothing Then
            Return String.Empty
        End If
        Dim tmpLONG As String = String.Empty
        Dim tmpLAT As String = String.Empty
        Dim posPOINT As String = String.Empty ' lat, long
        Dim posTITLE As String = String.Empty ' titulo - contraseña
        Dim posDESCRIPTION As String = String.Empty ' descripcion - informacion
        Dim posICON As String = String.Empty ' png
        Dim puntos_hojaderuta As New rastreo_hoja_de_ruta_puntos(cnn_str.CadenaDeConexion)
        Try
            Dim web_file As String = mi_usuario_pos.Usuario + "_" + mi_usuario_pos.IP.Replace(".", "_") + _
                        "_" + Session.SessionID + ".txt"
            Dim web_directory As String = "~/tmp/hdr_postxt/"
            Dim web_filename As String = web_directory.Replace("~", ".") + web_file
            With puntos_hojaderuta
                .Where.Idhoja_de_ruta.Value = idhoja_de_ruta.Value
                .Query.Load()
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
                If File.Exists(nom_arch) Then
                    File.Delete(nom_arch)
                End If
                If .RowCount > 0 Then
                    If .s_Id_punto <> String.Empty Then
                        'Dim web_file As String = mi_usuario_pos.Usuario + mi_usuario_pos.IP.Replace(".", "") + ".pos"
                        Dim archivoPOS As New StreamWriter(New FileStream(nom_arch, FileMode.Create, FileAccess.ReadWrite))
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
                                                     "Orden:<td/><td style=""border:solid 1px grey;"">" + .s_Orden + "<td/><tr/>" + _
                                                     "<tr><td style=""border:solid 1px grey;"">" + _
                                                     "Hora prevista de llegada:<td/><td style=""border:solid 1px grey;"">" + .Hora_llegada.ToShortTimeString() + "<td/><tr/>" + _
                                                     "<tr><td style=""border:solid 1px grey;"">" + _
                                                     "Tolerancia:<td/><td style=""border:solid 1px grey;"">" + .s_Minutos_demora + "<td/><tr/>" + _
                                                     "<tr><td style=""border:solid 1px grey;"">" + _
                                                     "Descripcion:<td/><td style=""border:solid 1px grey;"">" + .Descripcion + "<td/><tr/>" + _
                                                     "</table>" + _
                                                     "</div>" + _
                                                     Chr(9) + _
                                                     "./img/marker.png" + _
                                                     Chr(9) + _
                                                     " 20,  22" + _
                                                     Chr(9) + _
                                                     "-10, -16")
                                archivoPOS.Flush()
                                .MoveNext()
                            End While
                        End SyncLock
                        archivoPOS.Close()
                        txtfile.Value = web_filename
                    End If
                End If
            End With
            Return web_filename
        Catch ex As Exception
            Session.Add("msjerror", ex.ToString)
            Return String.Empty
        End Try
    End Function

    Private Sub Reload_Data()
        Dim sbScript As New StringBuilder()
        sbScript.Append("<script language='JavaScript' type='text/javascript'>" + ControlChars.Lf)
        sbScript.Append("<!--" + ControlChars.Lf)
        sbScript.Append("hojaderuta_UpdateMap(false, """ + Makehoja_de_rutaFile(miUsuario) + """);" + ControlChars.Lf)
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

    Protected Sub gv_Hoja_de_Ruta_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gv_Hoja_de_Ruta.SelectedIndexChanged
        If gv_Hoja_de_Ruta.SelectedIndex >= 0 Then
            pnlSelecRecorrido.Visible = True
            idhoja_de_ruta.Value = gv_Hoja_de_Ruta.SelectedDataKey("idhoja_de_ruta")
            Reload_Data()
        End If
    End Sub
    Protected Sub btnCargaRecorrido_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCargaRecorrido.Click
        pnlRecorrido.Visible = Not pnlRecorrido.Visible
        Session.Add("msjerror", "Haga click en el mapa para determinar la latitud y longitud del punto de recorrido.")
    End Sub
    Private Sub LimpiarRecorrido()
        rec_chkACT.Checked = False
        rec_txtDESC.Text = String.Empty
        rec_txtFIN.Text = String.Empty
        rec_txtINI.Text = String.Empty
    End Sub
    Private Sub GuardarRecorrido()
        Dim tblRecorrido As New rastreo_hoja_de_ruta(cnn_str.CadenaDeConexion)
        Dim tblVehiculoHasRecorrido As New rastreo_hojaderuta_has_vehiculo(cnn_str.CadenaDeConexion)
        Try
            With tblRecorrido
                If Session("updRECORRIDO") Then
                    Dim indice As Integer = 0
                    If Integer.TryParse(Session("updRECORRIDO_index"), indice) Then
                        .LoadByPrimaryKey(indice)
                        If .Query.Load() Then
                            If .s_Idhoja_de_ruta <> String.Empty Then
                                tblVehiculoHasRecorrido.LoadByPrimaryKey(idPersona, idVehiculo, .Idhoja_de_ruta)
                                .User_upd = miUsuario.idUsuario
                                .Fech_upd = Now
                                Session.Remove("updRECORRIDO")
                                Session.Remove("updRECORRIDO_index")
                            End If
                        End If
                    End If
                Else
                    .AddNew()
                    tblVehiculoHasRecorrido.AddNew()
                    .User_ins = miUsuario.idUsuario
                    .Fech_ins = Now
                End If
                .s_Id_usuario = idUsuario
                .s_Descripcion = rec_txtDESC.Text.Trim().ToUpperInvariant()
                .Activa = rec_chkACT.Checked
                .Hora_activacion = rec_txtINI.Text.Trim()
                .Hora_desactivacion = rec_txtFIN.Text.Trim()
                .Mails = txtMails.Text.Replace(" ", ";").Trim()
                .Tel_movil = txtTelMoviles.Text.Replace(" ", ";").Trim()
                .Save()
                If Not Session("updRECORRIDO") Then
                    tblVehiculoHasRecorrido.Id_cliente = idPersona
                    tblVehiculoHasRecorrido.Id_vehiculo = idVehiculo
                    tblVehiculoHasRecorrido.Idhoja_de_ruta = .Idhoja_de_ruta
                    tblVehiculoHasRecorrido.Save()
                End If
                sqlds_Recorrido.DataBind()
                gv_Hoja_de_Ruta.DataBind()
                LimpiarRecorrido()
                Session.Add("msjerror", "Recorrido guardado.")
            End With
        Catch ex As Exception
            Session.Add("msjerror", ex.ToString())
        End Try
    End Sub
    Protected Sub gv_Hoja_de_Ruta_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_Hoja_de_Ruta.RowCommand
        Try
            If e.CommandName = "Editar" Then
                Session.Add("updRECORRIDO", True)
                Session.Add("updRECORRIDO_index", e.CommandArgument)
                Dim tblLoad As New rastreo_hoja_de_ruta(cnn_str.CadenaDeConexion)
                With tblLoad
                    .LoadByPrimaryKey(e.CommandArgument)
                    rec_chkACT.Checked = .Activa
                    rec_txtDESC.Text = .Descripcion
                    rec_txtFIN.Text = .Hora_activacion.ToShortTimeString()
                    rec_txtFIN.Text = .Hora_desactivacion.ToShortTimeString()
                    txtMails.Text = .Mails
                    txtTelMoviles.Text = .Tel_movil
                End With
                pnlRecorrido.Visible = True
            ElseIf e.CommandName = "Eliminar" Then
                Dim tblDel As New rastreo_hoja_de_ruta(cnn_str.CadenaDeConexion)
                With tblDel
                    .LoadByPrimaryKey(e.CommandArgument)
                    .MarkAsDeleted()
                    .Save()
                End With
            End If
            sqlds_Recorrido.DataBind()
            gv_Hoja_de_Ruta.DataBind()
            sqlds_ListaPuntosRecorrido.DataBind()
            gv_Lista_Puntos_Recorrido.DataBind()
            Reload_Data()
            Reload_Data()
        Catch ex As Exception
            Session.Add("msjerror", ex.ToString())
        End Try
    End Sub
    Protected Sub btnSAVEREC_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSAVEREC.Click
        GuardarRecorrido()
        pnlRecorrido.Visible = False
    End Sub
    Protected Sub btnCANCELREC_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCANCELREC.Click
        pnlRecorrido.Visible = False
    End Sub

    Protected Sub btnCargaPuntosRecorrido_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCargaPuntosRecorrido.Click
        LimpiarPuntosRecorrido()
        BindDDL_HDR()
        pnlPuntos.Visible = Not pnlPuntos.Visible
    End Sub

    Private Sub BindDDL_HDR()
        ddlHDRg.Items.Clear()
        Dim HDR As New rastreo_template_hoja_de_ruta(cnn_str.CadenaDeConexion)
        With HDR
            .Where.Id_persona.Value = idPersona
            .Where.Publico.Value = True
            If .Query.Load() Then
                If .RowCount > 0 Then
                    If .s_Id_recorridotemplate <> String.Empty Then
                        While Not .EOF
                            Dim Itm As New ListItem(.Descripcion_recorrido, .Id_recorridotemplate.ToString())
                            ddlHDRg.Items.Add(Itm)
                            .MoveNext()
                        End While
                    End If
                End If
            End If
            .FlushData()
            .Where.WhereClauseReset()
            .Where.Id_usuarios.Value = idUsuario
            If .Query.Load() Then
                If .RowCount > 0 Then
                    If .s_Id_recorridotemplate <> String.Empty Then
                        While Not .EOF
                            Dim Itm As New ListItem(.Descripcion_recorrido, .Id_recorridotemplate.ToString())
                            If ddlHDRg.Items.FindByValue(Itm.Value) Is Nothing Then
                                ddlHDRg.Items.Add(Itm)
                            End If
                            .MoveNext()
                        End While
                    End If
                End If
            End If
            If ddlHDRg.Items.Count > 0 Then
                BindDDL_PTO(ddlHDRg.SelectedValue)
            End If
            'HDR.Where.Publico.Value = True
        End With
    End Sub

    Private Sub BindDDL_PTO(ByVal id_recorrido As Integer)
        ddlPTOg.Items.Clear()
        Dim PTO As New rastreo_template_puntos_hoja_de_ruta(cnn_str.CadenaDeConexion)
        With PTO
            .Where.Id_recorridotemplate.Value = id_recorrido
            If .Query.Load() Then
                If .RowCount > 0 Then
                    If .s_Id_recorridotemplate <> String.Empty Then
                        While Not .EOF
                            Dim Itm As New ListItem(.Nombre, .Id_puntostemplate.ToString())
                            ddlPTOg.Items.Add(Itm)
                            .MoveNext()
                        End While
                        PinPointPTO()
                    End If
                End If
            End If
        End With
    End Sub

    Protected Sub btnSAVEPTO_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSAVEPTO.Click
        GuardarPuntosRecorrido()
        pnlPuntos.Visible = False
        Reload_Data()
    End Sub
    Protected Sub btnCANCELPTO_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCANCELPTO.Click
        pnlPuntos.Visible = False
    End Sub
    Private Sub LimpiarPuntosRecorrido()
        pto_NroOrden.Text = String.Empty
        pto_txtDesc.Text = String.Empty
        pto_txtHoraLlegada.Text = String.Empty
        pto_txtLAT.Text = String.Empty
        pto_txtLON.Text = String.Empty
        pto_txtNom.Text = String.Empty
        pto_txtTOL.Text = String.Empty
    End Sub
    Private Sub GuardarPuntosRecorrido()
        Dim tblRecorridoPtos As New rastreo_hoja_de_ruta_puntos(cnn_str.CadenaDeConexion)
        Try
            With tblRecorridoPtos
                If Session("updRECORRIDOPTOS") Then
                    Dim indice As Integer = 0
                    If Integer.TryParse(Session("updRECORRIDOPTOS_index"), indice) Then
                        .LoadByPrimaryKey(indice)
                        If .Query.Load() Then
                            If .s_Id_punto <> String.Empty Then
                                '.User_upd = miUsuario.idUsuario
                                '.Fech_upd = Now
                                Session.Remove("updRECORRIDOPTOS")
                                Session.Remove("updRECORRIDOPTOS_index")
                            End If
                        End If
                    End If
                Else
                    .Where.Idhoja_de_ruta.Value = gv_Hoja_de_Ruta.SelectedDataKey("idhoja_de_ruta")
                    .Where.Orden.Value = pto_NroOrden.Text
                    If .Query.Load() Then
                        Session.Add("msjerror", "El orden que se quiere asignar a este punto ya existe")
                        Return
                    End If
                    .FlushData()
                    .AddNew()
                    .s_Idhoja_de_ruta = gv_Hoja_de_Ruta.SelectedDataKey("idhoja_de_ruta")
                    '.User_ins = miUsuario.idUsuario
                    '.Fech_ins = Now
                End If
                .s_Descripcion = pto_txtDesc.Text.Trim().ToUpperInvariant()
                .s_Hora_llegada = pto_txtHoraLlegada.Text.Trim()
                If My.Computer.Info.InstalledUICulture.NumberFormat.NumberDecimalSeparator = "." Then
                    .s_Lat = pto_txtLAT.Text.Trim()
                    .s_Lon = pto_txtLON.Text.Trim()
                Else
                    .s_Lat = pto_txtLAT.Text.Trim().Replace(".", ",")
                    .s_Lon = pto_txtLON.Text.Trim().Replace(".", ",")
                End If
                .s_Minutos_demora = pto_txtTOL.Text.Trim()
                .s_Nombre = pto_txtNom.Text.Trim().ToUpperInvariant()
                .s_Orden = pto_NroOrden.Text.Trim()
                .Save()
                sqlds_ListaPuntosRecorrido.DataBind()
                gv_Lista_Puntos_Recorrido.DataBind()
                LimpiarPuntosRecorrido()
                Session.Add("msjerror", "Punto de Recorrido guardado.")
            End With
        Catch ex As Exception
            Session.Add("msjerror", ex.ToString())
        End Try
    End Sub
    Protected Sub gv_Lista_Puntos_Recorrido_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_Lista_Puntos_Recorrido.RowCommand
        Try
            If e.CommandName = "Editar" Then
                Session.Add("updRECORRIDOPTOS", True)
                Session.Add("updRECORRIDOPTOS_index", e.CommandArgument)
                Dim tblLoad As New rastreo_hoja_de_ruta_puntos(cnn_str.CadenaDeConexion)
                With tblLoad
                    .LoadByPrimaryKey(e.CommandArgument)
                    pto_NroOrden.Text = .s_Orden
                    pto_txtDesc.Text = .s_Descripcion
                    pto_txtHoraLlegada.Text = .Hora_llegada.ToShortTimeString()
                    pto_txtLAT.Text = .s_Lat
                    pto_txtLON.Text = .s_Lon
                    pto_txtNom.Text = .s_Nombre
                    pto_txtTOL.Text = .s_Minutos_demora
                End With
                BindDDL_HDR()
                pnlPuntos.Visible = True
            ElseIf e.CommandName = "Eliminar" Then
                Dim tblDel As New rastreo_hoja_de_ruta_puntos(cnn_str.CadenaDeConexion)
                With tblDel
                    .LoadByPrimaryKey(e.CommandArgument)
                    .MarkAsDeleted()
                    .Save()
                End With
            End If
            sqlds_Recorrido.DataBind()
            gv_Hoja_de_Ruta.DataBind()
            sqlds_ListaPuntosRecorrido.DataBind()
            gv_Lista_Puntos_Recorrido.DataBind()
            Reload_Data()
        Catch ex As Exception
            Session.Add("msjerror", ex.ToString())
        End Try
    End Sub

    Protected Sub gv_Hoja_de_Ruta_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_Hoja_de_Ruta.RowDataBound
        If gv_Hoja_de_Ruta.SelectedIndex >= 0 Then
            If Not gv_Hoja_de_Ruta.SelectedDataKey Is Nothing Then
                idhoja_de_ruta.Value = gv_Hoja_de_Ruta.SelectedDataKey("idhoja_de_ruta")
                'Reload_Data()
            End If
        End If
        sqlds_ListaPuntosRecorrido.DataBind()
        gv_Lista_Puntos_Recorrido.DataBind()
    End Sub

    Protected Sub rec_txtINI_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rec_txtINI.TextChanged
        checkTIME()
    End Sub

    Protected Sub rec_txtFIN_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rec_txtFIN.TextChanged
        checkTIME()
    End Sub

    Private Sub checkTIME()
        'Dim TF As DateTime
        'Dim TI As DateTime
        'If rec_txtINI.Text <> "" And rec_txtFIN.Text <> "" Then
        '    TF = Convert.ToDateTime(rec_txtFIN.Text)
        '    TI = Convert.ToDateTime(rec_txtINI.Text)
        'End If
        'If TF > TI Then
        '    Session.Add("msjerror", "La hora de inicio no puede ser mayor a la final.")
        '    rec_txtFIN.Text = String.Empty
        '    rec_txtINI.Text = String.Empty
        '    rec_txtINI.Focus()
        'ElseIf TI > TF Then
        '    Session.Add("msjerror", "La hora de final no puede ser menor a la inicial.")
        '    rec_txtFIN.Text = String.Empty
        '    rec_txtINI.Text = String.Empty
        '    rec_txtINI.Focus()
        'End If

    End Sub

    Protected Sub ddlHDRg_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlHDRg.SelectedIndexChanged
        BindDDL_PTO(ddlHDRg.SelectedValue)
    End Sub

    Protected Sub ddlPTOg_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPTOg.DataBound
        PinPointPTO()
    End Sub

    Protected Sub ddlPTOg_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPTOg.SelectedIndexChanged
        PinPointPTO()
    End Sub

    Private Sub PinPointPTO()
        Dim PTO As New rastreo_template_puntos_hoja_de_ruta(cnn_str.CadenaDeConexion)
        With PTO
            .LoadByPrimaryKey(ddlPTOg.SelectedValue)
            If .RowCount > 0 Then
                If .s_Id_recorridotemplate <> String.Empty Then
                    pto_txtLAT.Text = .s_Lat
                    pto_txtLON.Text = .s_Lon
                    Dim sbScript As StringBuilder = New StringBuilder()
                    sbScript.Append("<script language='JavaScript' type='text/javascript'>" + ControlChars.Lf)
                    sbScript.Append("<!--" + ControlChars.Lf)
                    sbScript.Append("map_panTo(" + .s_Lon.Replace(",", ".") + _
                                        "," + .s_Lat.Replace(",", ".") + _
                                        ");" + ControlChars.Lf)
                    sbScript.Append("// -->" + ControlChars.Lf)
                    sbScript.Append("</script>" + ControlChars.Lf)
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Onload_centermap", _
                                                              sbScript.ToString(), False)
                    sbScript = New StringBuilder()
                    sbScript.Append("<script language='JavaScript' type='text/javascript'>" + ControlChars.Lf)
                    sbScript.Append("<!--" + ControlChars.Lf)
                    sbScript.Append("hojaderuta_UpdateMark(" + .s_Lon.Replace(",", ".") + _
                    "," + .s_Lat.Replace(",", ".") + _
                    ");" + ControlChars.Lf)
                    sbScript.Append("// -->" + ControlChars.Lf)
                    sbScript.Append("</script>" + ControlChars.Lf)
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Onload_pointmap", _
                                                              sbScript.ToString(), False)
                End If
            End If
        End With
    End Sub
End Class
