Imports RASTREOmw
Imports MyGeneration.dOOdads

Partial Class RASTREO_Administracion_admin_eventos_lista
    Inherits System.Web.UI.Page

    Dim tde_EVENTO As Integer = 0
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
            tde_EVENTO = CType(Request.QueryString("id"), Integer)
        Else

        End If
        If Not IsPostBack Then
            Utilidades.fillDDL(ddlTipoEquiposGPS, New rastreogps_tipoequipo(cnn_str.CadenaDeConexion), _
                               rastreogps_tipoequipo.ColumnNames.Tipo_equipo, _
                               rastreogps_tipoequipo.ColumnNames.Idrastreogps_tipoequipo)
            If Not Request.UrlReferrer Is Nothing And ViewState("GO_BACK_URL") Is Nothing Then
                ViewState.Add("GO_BACK_URL", Request.UrlReferrer.ToString)
            End If
        End If
    End Sub

    Protected Sub btnAddEvento_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddEvento.Click
        Try
            Response.Redirect("admingps_eventos.aspx?tdeid=" + tde_EVENTO.ToString, False)
        Catch X As Exception
            Response.Write(X.Message)
        End Try

    End Sub

    Protected Sub gv_Lista_EventosGPS_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gv_Lista_EventosGPS.RowDeleting
        Dim Indx As Integer = 0
        Indx = CType(e.Values("idrastreogps_tipo_evento"), Integer)
        Dim tblTipoEventosEquipos As New rastreogps_tipoevento(cnn_str.CadenaDeConexion)
        tblTipoEventosEquipos.LoadByPrimaryKey(Indx)
        tblTipoEventosEquipos.MarkAsDeleted()
        tblTipoEventosEquipos.Save()
        Response.Redirect(Request.Url.ToString)
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

    Protected Sub btnSeleccionarTDE_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSeleccionarTDE.Click
        Response.Redirect("admingps_eventos_lista.aspx?id=" + ddlTipoEquiposGPS.SelectedValue.ToString, False)
    End Sub
End Class
