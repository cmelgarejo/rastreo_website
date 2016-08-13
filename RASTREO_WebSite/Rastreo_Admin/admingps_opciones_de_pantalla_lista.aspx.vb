Imports RASTREOmw
Imports MyGeneration.dOOdads


Public Class admingps_opciones_de_pantalla_lista
    Inherits System.Web.UI.Page

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
        permisoAdd = Utilidades.FindPermissionFor(mi_usuario.idUsuario, "OPCIONESPANTALLA_ADMIN-A")
        permisoDel = Utilidades.FindPermissionFor(mi_usuario.idUsuario, "OPCIONESPANTALLA_ADMIN-B")
        permisoMod = Utilidades.FindPermissionFor(mi_usuario.idUsuario, "OPCIONESPANTALLA_ADMIN-M")
        permisoQry = Utilidades.FindPermissionFor(mi_usuario.idUsuario, "OPCIONESPANTALLA_ADMIN-C")
        'Boton de "NUEVO"
        btnAddOpcion.Visible = permisoAdd
        'Esto solo funciona si la grilla siempre tiene "Editar" en penultimo lugar.
        gv_Lista_Opciones.Columns(gv_Lista_Opciones.Columns.Count - 2).Visible = permisoMod
        'Esto solo funciona si la grilla siempre tiene "Eliminar" al ultimo.
        gv_Lista_Opciones.Columns(gv_Lista_Opciones.Columns.Count - 1).Visible = permisoDel
        'Aqui seteamos a NADA el datasource si no tiene permisos para consultar...
        If Not permisoQry Then gv_Lista_Opciones.Visible = False
        gv_Lista_Opciones.DataBind()
    End Sub

    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        Try
            Dim mensaje As String = String.Empty
            mensaje = ViewState("msjerror")
            If mensaje = String.empty Then
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

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Not Request.UrlReferrer Is Nothing And ViewState("GO_BACK_URL") Is Nothing Then
                ViewState.Add("GO_BACK_URL", Request.UrlReferrer.ToString)
            End If
        End If
    End Sub

    Protected Sub btnAddOpcion_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddOpcion.Click
        Try
            Response.Redirect("admingps_opciones_de_pantalla.aspx", False)
        Catch X As Exception
            Response.Write(X.Message)
        End Try
    End Sub

    Protected Sub gv_Lista_Opciones_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gv_Lista_Opciones.RowDeleting
        Dim Indx As Integer = 0
        Indx = CType(e.Values("idrastreogps_equipogps"), Integer)
        Dim tblOpPan As New rastreogps_opciones_de_pantalla(cnn_str.CadenaDeConexion)
        With tblOpPan
            .LoadByPrimaryKey(Indx)
            .MarkAsDeleted()
            .Save()
        End With
        Response.Redirect(Request.Url.ToString)
    End Sub

End Class



