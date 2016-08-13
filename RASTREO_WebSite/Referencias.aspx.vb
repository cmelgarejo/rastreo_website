Imports RASTREOmw
Imports System.IO

Partial Class Referencias
    Inherits System.Web.UI.Page

    Public idPersona As Integer
    Public idUsuario As Integer

    Public miUsuario As Usuario
    Public mi_usuario As String = String.Empty

    Public permiso_menu_admin As Boolean
    Public permiso_menu_usuarios As Boolean

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
        idUsuario = Request.QueryString("uid")
        idPersona = Request.QueryString("cid")
        If IsPostBack Then
            Reload_Data()
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
        Catch myEX As Exception
            ViewState.Add("msjerror", myEX.Message + "<br>" + myEX.StackTrace)
        End Try
    End Sub

    Public Function MakeREFFile(ByVal mi_usuario_pos As Usuario) As String
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
        Dim puntos_hojaderuta As New rastreo_template_puntos_hoja_de_ruta(cnn_str.CadenaDeConexion)
        Try
            Dim web_file As String = mi_usuario_pos.Usuario + "_" + mi_usuario_pos.IP.Replace(".", "_") + _
                        "_" + Session.SessionID + ".txt"
            Dim web_directory As String = "~/tmp/template_postxt/"
            Dim web_filename As String = web_directory.Replace("~", ".") + web_file
            With puntos_hojaderuta
                .Where.Id_recorridotemplate.Value = id_recorridotemplate.Value
                .Query.Load()
                If .RowCount > 0 Then
                    If .s_Id_puntostemplate <> String.Empty Then
                        'Dim web_file As String = mi_usuario_pos.Usuario + mi_usuario_pos.IP.Replace(".", "") + ".pos"
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
                                                     "./img/center_normal.gif" + _
                                                     Chr(9) + _
                                                     " 15,  15" + _
                                                     Chr(9) + _
                                                     "0, 0")
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
        'Makehoja_de_rutaFile(miUsuario)
        Dim sbScript As New StringBuilder()
        sbScript.Append("<script language='JavaScript' type='text/javascript'>" + ControlChars.Lf)
        sbScript.Append("<!--" + ControlChars.Lf)
        sbScript.Append("hojaderuta_UpdateMap(false, """ + MakeREFFile(miUsuario) + """);" + ControlChars.Lf)
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


    Protected Sub gv_hoja_de_rutas_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gv_Hoja_de_Ruta_template.SelectedIndexChanged
        If gv_Hoja_de_Ruta_template.SelectedIndex >= 0 Then
            'Form1.Visible = True
            pnlSelechoja_de_ruta.Visible = True
            id_recorridotemplate.Value = gv_Hoja_de_Ruta_template.SelectedDataKey("id_recorridotemplate")
            Reload_Data()
        End If
    End Sub
    Protected Sub btnCargahoja_de_ruta_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCargahoja_de_ruta.Click
        pnlhoja_de_ruta.Visible = Not pnlhoja_de_ruta.Visible
        Session.Add("msjerror", "Haga click en el mapa para determinar la latitud y longitud del punto de hoja_de_ruta.")
    End Sub
    Private Sub Limpiarhoja_de_ruta()
        rec_chkACT.Checked = False
        rec_txtDESC.Text = String.Empty
    End Sub
    Private Sub Guardarhoja_de_ruta()
        Dim tblhoja_de_ruta As New rastreo_template_hoja_de_ruta(cnn_str.CadenaDeConexion)
        Try
            With tblhoja_de_ruta
                If ViewState("updhoja_de_ruta") Then
                    Dim indice As Integer = 0
                    If Integer.TryParse(ViewState("updhoja_de_ruta_index"), indice) Then
                        .LoadByPrimaryKey(indice)
                        If .s_Id_recorridotemplate <> String.Empty Then
                            .User_upd = miUsuario.idUsuario
                            .Fech_upd = Now
                            ViewState.Remove("updhoja_de_ruta")
                            ViewState.Remove("updhoja_de_ruta_index")
                        End If
                    End If
                Else
                    .AddNew()
                    .Id_persona = idPersona
                    .Id_usuarios = idUsuario
                    .User_ins = miUsuario.idUsuario
                    .Fech_ins = Now
                End If
                .s_Descripcion_recorrido = rec_txtDESC.Text.Trim().ToUpperInvariant()
                .s_Publico = rec_chkACT.Checked
                .Save()
                sqlds_hoja_de_ruta.DataBind()
                gv_Hoja_de_Ruta_template.DataBind()
                Limpiarhoja_de_ruta()
                Session.Add("msjerror", "hoja_de_ruta guardado.")
            End With
        Catch ex As Exception
            Session.Add("msjerror", ex.ToString())
        End Try
    End Sub
    Protected Sub gv_Hoja_de_Ruta_template_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_Hoja_de_Ruta_template.RowCommand
        Try
            If e.CommandName = "Editar" Then
                ViewState.Add("updhoja_de_ruta", True)
                ViewState.Add("updhoja_de_ruta_index", e.CommandArgument)
                Dim tblLoad As New rastreo_template_hoja_de_ruta(cnn_str.CadenaDeConexion)
                With tblLoad
                    .LoadByPrimaryKey(e.CommandArgument)
                    rec_chkACT.Checked = .Publico
                    rec_txtDESC.Text = .s_Descripcion_recorrido
                End With
                ' aqui le agregue Form1 09022011 - Pedro Silva
                'Form1.Visible = True
                pnlhoja_de_ruta.Visible = True
            ElseIf e.CommandName = "Eliminar" Then
                Dim tblDel As New rastreo_template_hoja_de_ruta(cnn_str.CadenaDeConexion)
                With tblDel
                    If .LoadByPrimaryKey(e.CommandArgument) Then
                        .MarkAsDeleted()
                        .Save()
                    End If
                End With
            End If
            'aqui le agregue Form1 09022011 - Pedro Silva
            ' Form1.DataBind()
            sqlds_hoja_de_ruta.DataBind()
            gv_Hoja_de_Ruta_template.DataBind()
            sqlds_ListaPuntoshoja_de_ruta.DataBind()
            gv_Lista_Puntos_hoja_de_ruta.DataBind()
            Reload_Data()
        Catch ex As Exception
            Session.Add("msjerror", ex.ToString())
        End Try
    End Sub
    Protected Sub btnSAVEREC_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSAVEREC.Click
        Guardarhoja_de_ruta()
        pnlhoja_de_ruta.Visible = False
    End Sub
    Protected Sub btnCANCELREC_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCANCELREC.Click
        pnlhoja_de_ruta.Visible = False
    End Sub

    Protected Sub btnCargaPuntoshoja_de_ruta_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCargaPuntoshoja_de_ruta.Click
        pnlPuntos.Visible = Not pnlPuntos.Visible
    End Sub
    Protected Sub btnSAVEPTO_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSAVEPTO.Click
        GuardarPuntoshoja_de_ruta()
        pnlPuntos.Visible = False
    End Sub
    Protected Sub btnCANCELPTO_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCANCELPTO.Click
        pnlPuntos.Visible = False
    End Sub
    Private Sub LimpiarPuntoshoja_de_ruta()
        pto_txtDesc.Text = String.Empty
        pto_txtLAT.Text = String.Empty
        pto_txtLON.Text = String.Empty
        pto_txtNom.Text = String.Empty
        pto_txtMETROS_REDONDA.Text = String.Empty
        pto_chkReferencia.Checked = False
    End Sub
    Private Sub GuardarPuntoshoja_de_ruta()
        Dim tblhoja_de_rutaPtos As New rastreo_template_puntos_hoja_de_ruta(cnn_str.CadenaDeConexion)
        Try
            Dim Latitud As String
            Dim Longitud As String
            Latitud = pto_txtLAT.Text.Trim().Replace(".", ",")
            Longitud = pto_txtLON.Text.Trim().Replace(".", ",")
            If Latitud <> "" Or _
                 Longitud <> "" Then
                With tblhoja_de_rutaPtos
                    If ViewState("updhoja_de_rutaPTOS") Then
                        Dim indice As Integer = 0
                        If Integer.TryParse(ViewState("updhoja_de_rutaPTOS_index"), indice) Then
                            If .LoadByPrimaryKey(indice) Then
                                If .s_Id_puntostemplate <> String.Empty Then
                                    ViewState.Remove("updhoja_de_rutaPTOS")
                                    ViewState.Remove("updhoja_de_rutaPTOS_index")
                                Else
                                    Return
                                End If
                            End If
                        End If
                    Else
                        .AddNew()
                        .s_Id_recorridotemplate = gv_Hoja_de_Ruta_template.SelectedDataKey("id_recorridotemplate")
                        '.User_ins = miUsuario.idUsuario
                        '.Fech_ins = Now
                    End If
                    .s_Descripcion = pto_txtDesc.Text.Trim().ToUpperInvariant()
                    '.s_ = pto_txtHoraLlegada.Text.Trim()
                    If My.Computer.Info.InstalledUICulture.NumberFormat.NumberDecimalSeparator = "." Then
                        .s_Lat = pto_txtLAT.Text.Trim()
                        .s_Lon = pto_txtLON.Text.Trim()
                    Else
                        .s_Lat = pto_txtLAT.Text.Trim().Replace(".", ",")
                        .s_Lon = pto_txtLON.Text.Trim().Replace(".", ",")
                    End If
                    '.s_R_minutos_demora = pto_txtTOL.Text.Trim()
                    .s_Nombre = pto_txtNom.Text.Trim().ToUpperInvariant()
                    '.s_R_orden = pto_NroOrden.Text.Trim()
                    If pto_txtMETROS_REDONDA.Text.Trim() <> String.Empty Then
                        .Metros = Convert.ToInt32(pto_txtMETROS_REDONDA.Text.Trim())
                    Else
                        .Metros = 100
                    End If
                    If pto_chkReferencia.Checked Then
                        .Avisar_ingreso = True
                        .Emails = txtMailReferencias_ptoref.Text.Replace(" ", ";").Replace(",", ";").Trim()
                    Else
                        .Avisar_ingreso = False
                        '.SetColumnNull(rastreo_template_puntos_hoja_de_ruta.ColumnNames.Emails)
                    End If
                    .Save()
                    sqlds_ListaPuntoshoja_de_ruta.DataBind()
                    gv_Lista_Puntos_hoja_de_ruta.DataBind()
                    LimpiarPuntoshoja_de_ruta()

                    Session.Add("msjerror", "Punto de hoja de ruta guardado.")

                    Reload_Data()
                End With
            Else
                Session.Add("msjerror", "Debe hacer click en el mapa donde este el punto de su referencia.")
            End If

        Catch ex As Exception
            Session.Add("msjerror", ex.ToString())
        End Try
    End Sub
    Protected Sub gv_Lista_Puntos_hoja_de_ruta_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_Lista_Puntos_hoja_de_ruta.RowCommand
        Try
            If e.CommandName = "Editar" Then
                ViewState.Add("updhoja_de_rutaPTOS", True)
                ViewState.Add("updhoja_de_rutaPTOS_index", e.CommandArgument)
                Dim tblLoad As New rastreo_template_puntos_hoja_de_ruta(cnn_str.CadenaDeConexion)
                With tblLoad
                    .LoadByPrimaryKey(e.CommandArgument)
                    'pto_NroOrden.Text = .s_R_orden
                    pto_txtDesc.Text = .s_Descripcion
                    'pto_txtHoraLlegada.Text = .R_hora_llegada.ToShortTimeString()
                    pto_txtLAT.Text = .s_Lat
                    pto_txtLON.Text = .s_Lon
                    pto_txtNom.Text = .s_Nombre
                    pto_txtMETROS_REDONDA.Text = .s_Metros
                    txtMailReferencias_ptoref.Text = .Emails
                    pto_chkReferencia.Checked = .Avisar_ingreso
                    'pto_txtTOL.Text = .s_R_minutos_demora
                End With
                pnlPuntos.Visible = True
            ElseIf e.CommandName = "Eliminar" Then
                Dim tblDel As New rastreo_template_puntos_hoja_de_ruta(cnn_str.CadenaDeConexion)
                With tblDel
                    .LoadByPrimaryKey(e.CommandArgument)
                    .MarkAsDeleted()
                    .Save()
                End With
            End If
            sqlds_hoja_de_ruta.DataBind()
            gv_Hoja_de_Ruta_template.DataBind()
            sqlds_ListaPuntoshoja_de_ruta.DataBind()
            gv_Lista_Puntos_hoja_de_ruta.DataBind()
            Reload_Data()
        Catch ex As Exception
            Session.Add("msjerror", ex.ToString())
        End Try
    End Sub

    Protected Sub gv_Hoja_de_Ruta_template_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_Hoja_de_Ruta_template.RowDataBound
        If gv_Hoja_de_Ruta_template.SelectedIndex >= 0 Then
            If Not gv_Hoja_de_Ruta_template.SelectedDataKey Is Nothing Then
                id_recorridotemplate.Value = gv_Hoja_de_Ruta_template.SelectedDataKey("id_recorridotemplate")
            End If
            Reload_Data()
        End If
        'Form1.Visible = True
        sqlds_ListaPuntoshoja_de_ruta.DataBind()
        gv_Lista_Puntos_hoja_de_ruta.DataBind()
    End Sub
    'Creamos un procedimiento para controlar las descargas de las referencias 07022011 - Pedro Silva
    Protected Sub btnExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExcel.Click
        '    'Aca llamamos a la funcion ExportExcel para luego descargar las referencias.
        ExportExcel()
        'Dim sbScript As New StringBuilder()
        'sbScript.Append("<script language='JavaScript' type='text/javascript'>" + ControlChars.Lf)
        'sbScript.Append("<!--" + ControlChars.Lf)
        ''sbScript.Append("window.close();" + ControlChars.Lf)
        'sbScript.Append("// -->" + ControlChars.Lf)
        'sbScript.Append("</script>" + ControlChars.Lf)
    End Sub

    Public Sub ExportExcel()

        'Dim sb As StringBuilder = New StringBuilder()
        'Dim sw As StringWriter = New StringWriter(sb)
        'Dim htw As HtmlTextWriter = New HtmlTextWriter(sw)
        'Dim pagina As Page = New Page
        'Dim form = New HtmlForm
        'gv_Lista_Puntos_hoja_de_ruta.EnableViewState = False
        'pagina.EnableEventValidation = False
        'pagina.DesignerInitialize()
        'pagina.Controls.Add(form)
        'form.Controls.Add(gv_Lista_Puntos_hoja_de_ruta)
        'pagina.RenderControl(htw)

        'HttpContext.Current.Response.Clear()
        'HttpContext.Current.Response.Buffer = True
        'HttpContext.Current.Response.ContentType = "application/vnd.ms-excel"
        'HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=recorrido_" & mi_usuario & Now.ToString("_ddMMyyy_HHmm_") & "data.xls")
        'HttpContext.Current.Response.Charset = "UTF-8"
        'HttpContext.Current.Response.ContentEncoding = Encoding.Default
        'HttpContext.Current.Response.Write(sb.ToString())
        'HttpContext.Current.Response.End()

        'Dim sb As New StringBuilder()
        'Dim sw As New StringWriter(sb)
        'Dim htw As New HtmlTextWriter(sw)

        ''gv_Lista_Puntos_hoja_de_ruta.Visible = False

        ''For Each row As GridViewRow In gridVehiculoSeleccionado.Rows
        '' row.Cells(5).Controls.RemoveAt(5)
        '' row.Cells(5).Controls.RemoveAt(1)
        '' row.Cells(5).Controls.RemoveAt(2)
        ''Next
        'htw.Flush()
        ''lbhoja_de_rutaPanel.RenderControl(htw)
        ''pnlSelechoja_de_ruta.RenderControl(htw)
        ''pnlPuntos.RenderControl(htw)
        ''btnCargahoja_de_ruta.Visible = False
        'gv_Lista_Puntos_hoja_de_ruta.RenderControl(htw)

        'HttpContext.Current.Response.Clear()
        'HttpContext.Current.Response.Buffer = True
        'HttpContext.Current.Response.ContentType = "application/vnd.ms-excel"
        'HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=recorrido_" & mi_usuario & Now.ToString("_ddMMyyy_HHmm_") & ".xls")
        'HttpContext.Current.Response.Charset = "UTF-8"
        'HttpContext.Current.Response.ContentEncoding = Encoding.Default '  New System.Text.UTF8Encoding(False)
        'HttpContext.Current.Response.Write(sb.ToString())
        'HttpContext.Current.Response.End()

        '  If (gv_Lista_Puntos_hoja_de_ruta.Rows.Count > 0) Then

        '      'StringBuilder sb = new StringBuilder();
        '      Dim sb As New StringBuilder()
        '      'StringWriter sw = new StringWriter(sb);
        '      Dim sw As New StringWriter(sb)
        '      'HtmlTextWriter htw = new HtmlTextWriter(sw);
        '      Dim htw As New HtmlTextWriter(sw)
        '      Page pagina = new Page();
        '  HtmlForm form = new HtmlForm();
        '  GridView dg = new GridView();
        '  dg.EnableViewState = false;
        '  dg.DataSource = tabla;
        '  dg.DataBind();
        '  pagina.EnableEventValidation = false;
        '  pagina.DesignerInitialize();
        '  pagina.Controls.Add(form);
        '  form.Controls.Add(dg);
        '  pagina.RenderControl(htw);
        '  Response.Clear();
        '  Response.Buffer = true;
        '  Response.ContentType = "application/vnd.ms-excel";
        '  Response.AddHeader("Content-Disposition", "attachment;filename=nombedetufichero.xls");
        '  Response.Charset = "UTF-8";
        '  Response.ContentEncoding = Encoding.Default;
        '  Response.Write(sb.ToString());
        '  Response.End();

        '  End If
        '}


    End Sub
End Class