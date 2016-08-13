Imports RASTREOmw
Imports MyGeneration.dOOdads

Partial Class RASTREO_Administracion_admin_eventos_equipo_lista
    Inherits System.Web.UI.Page

    Dim EquipoEvento As Integer = 0
    Public mi_usuario As New Usuario()
    Private permisoAdd As Boolean = False
    Private permisoMod As Boolean = False
    Private permisoDel As Boolean = False
    Private permisoQry As Boolean = False

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Not Session("session_UsuarioRASTREO") Is Nothing Then
            mi_usuario = CType(Session("session_UsuarioRASTREO"), Usuario)
        Else
            Response.Redirect("../Login.aspx?rp=" + Me.Request.Url.AbsoluteUri(), False)
        End If
        permisoAdd = Utilidades.FindPermissionFor(mi_usuario.idUsuario, "EVENTOSGPS_ADMIN-A")
        permisoDel = Utilidades.FindPermissionFor(mi_usuario.idUsuario, "EVENTOSGPS_ADMIN-B")
        permisoMod = Utilidades.FindPermissionFor(mi_usuario.idUsuario, "EVENTOSGPS_ADMIN-M")
        permisoQry = Utilidades.FindPermissionFor(mi_usuario.idUsuario, "EVENTOSGPS_ADMIN-C")
        'Boton de "NUEVO"
        btnAddEvento.Visible = permisoAdd
        'Esto solo funciona si la grilla siempre tiene "Editar" en penultimo lugar.
        gv_Lista_EventosGPS.Columns(gv_Lista_EventosGPS.Columns.Count - 2).Visible = permisoMod
        'Esto solo funciona si la grilla siempre tiene "Eliminar" al ultimo.
        gv_Lista_EventosGPS.Columns(gv_Lista_EventosGPS.Columns.Count - 1).Visible = permisoDel
        'Aqui seteamos a NADA el datasource si no tiene permisos para consultar...
        If Not permisoQry Then gv_Lista_EventosGPS.Visible = False
        gv_Lista_EventosGPS.DataBind()
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Request.QueryString("id") <> String.Empty Then
            EquipoEvento = CType(Request.QueryString("id"), Integer)
        Else

        End If
        If Not IsPostBack Then
            bindDDL_EventosTipoEquipo()
            Utilidades.fillDDL(ddlEquiposGPS, New rs_select_equipogps(cnn_str.CadenaDeConexion), _
                               rs_select_equipogps.ColumnNames.Ddlequipogps_desc, _
                               rs_select_equipogps.ColumnNames.Idrastreogps_equipogps)
            If Not Request.UrlReferrer Is Nothing And ViewState("GO_BACK_URL") Is Nothing Then
                ViewState.Add("GO_BACK_URL", Request.UrlReferrer.ToString)
            End If
        End If
    End Sub

    Protected Sub bindDDL_EventosTipoEquipo()
        Try
            Dim IdTDEquipo As Integer = 0
            Dim tblEquipo As New rastreogps_equipogps(cnn_str.CadenaDeConexion)
            Dim tblTipoEventos As New rastreogps_tipoevento(cnn_str.CadenaDeConexion)
            With tblEquipo
                .LoadByPrimaryKey(EquipoEvento)
                If .RowCount > 0 Then
                    IdTDEquipo = .Tipoequipo
                End If
            End With
            If IdTDEquipo > 0 Then
                With tblTipoEventos
                    .Where.Id_tipoequipo.Value = IdTDEquipo
                    If .Query.Load Then
                        If .RowCount > 0 Then
                            ddlEventosDisponibles.DataSource = .DefaultView
                            ddlEventosDisponibles.DataValueField = rastreogps_tipoevento.ColumnNames.Idrastreogps_tipo_evento
                            ddlEventosDisponibles.DataTextField = rastreogps_tipoevento.ColumnNames.Descripcion
                            ddlEventosDisponibles.DataBind()
                        End If
                    End If
                End With
            End If
        Catch myex As Exception
            Session.Add("msjerror", myex.Message + "<br>" + myex.StackTrace)
        End Try
    End Sub


    Protected Sub gv_Lista_EventosGPS_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_Lista_EventosGPS.RowCommand
        Try
            If e.CommandName = "Editar" Then
                ViewState.Add("admin_equipoevento_update", True)
                chkHabilitado.Checked = True
                pnlAddEvento.Visible = True
                ddlEventosDisponibles.SelectedItem.Selected = False
                ddlEventosDisponibles.Items.FindByValue(e.CommandArgument).Selected = True
                Dim tblEE As New rastreogps_equipo_eventos(cnn_str.CadenaDeConexion)
                tblEE.LoadByPrimaryKey(EquipoEvento, e.CommandArgument)
                chkHabilitado.Checked = tblEE.Habilitado
            End If
        Catch myEX As Exception
            Session.Add("msjerror", myEX.Message + "<br>" + myEX.StackTrace)
        End Try
    End Sub

    Protected Sub gv_Lista_EventosGPS_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gv_Lista_EventosGPS.RowDeleting
        Dim Indx As Integer = 0
        Indx = CType(e.Values("id_tipoevento"), Integer)
        Dim tblEventosEquipos As New rastreogps_equipo_eventos(cnn_str.CadenaDeConexion)
        tblEventosEquipos.LoadByPrimaryKey(EquipoEvento, Indx)
        tblEventosEquipos.MarkAsDeleted()
        tblEventosEquipos.Save()
        Response.Redirect(Request.Url.ToString, False)
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
            Session.Add("msjerror", myEX.Message + "<br>" + myEX.StackTrace)
        End Try
    End Sub

    Protected Sub btnSeleccionarTDE_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSeleccionarEquipo.Click
        Response.Redirect("admingps_eventos_equipo_lista.aspx?id=" + ddlEquiposGPS.SelectedValue.ToString, False)
    End Sub

    Protected Sub btnGuardaEvento_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGuardaEvento.Click
        Try
            Dim idEvento As Integer = ddlEventosDisponibles.SelectedValue
            Dim tblEquipoEvento As New rastreogps_equipo_eventos(cnn_str.CadenaDeConexion)
            With tblEquipoEvento
                If Not ViewState("admin_equipoevento_update") Then
                    .Where.Id_equipogps.Value = EquipoEvento
                    .Where.Id_tipoevento.Value = idEvento
                    If .Query.Load Then
                        If .RowCount > 0 Then
                            Session.Add("msjerror", "El equipo ya posee este evento.")
                        End If
                    End If
                    .AddNew()
                    .Id_equipogps = EquipoEvento
                    .Id_tipoevento = idEvento
                    .Evento = txtEvento.Text.ToUpperInvariant().Trim()
                    .Habilitado = chkHabilitado.Checked
                    .User_ins = mi_usuario.idUsuario
                    .Fech_ins = Now
                    .Save()
                    Session.Add("msjerror", "Evento registrado exitosamente!")
                Else
                    .LoadByPrimaryKey(EquipoEvento, idEvento)
                    .Id_tipoevento = idEvento
                    .Evento = txtEvento.Text.ToUpperInvariant().Trim()
                    .Habilitado = chkHabilitado.Checked
                    .User_upd = mi_usuario.idUsuario
                    .Fech_upd = Now
                    .Save()
                    Session.Add("msjerror", "Evento actulizado exitosamente!")
                End If
            End With
            Response.Redirect(Request.Url.ToString, False)
        Catch myEX As Exception
            Session.Add("msjerror", myEX.Message + "<br>" + myEX.StackTrace)
        End Try
    End Sub

    Protected Sub btnAddEvento_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddEvento.Click
        pnlAddEvento.Visible = Not pnlAddEvento.Visible
    End Sub
End Class
