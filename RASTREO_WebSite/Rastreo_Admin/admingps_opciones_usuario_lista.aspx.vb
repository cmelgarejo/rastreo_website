Imports RASTREOmw
Imports MyGeneration.dOOdads


Public Class admingps_opciones_usuario_lista
    Inherits System.Web.UI.Page

    Public mi_usuario As New Usuario()
    Public usuario As String
    Public idUsuario As Integer

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
        permisoAdd = Utilidades.FindPermissionFor(mi_usuario.idUsuario, "OPCIONESUSUARIO_ADMIN-A")
        permisoDel = Utilidades.FindPermissionFor(mi_usuario.idUsuario, "OPCIONESUSUARIO_ADMIN-B")
        permisoMod = Utilidades.FindPermissionFor(mi_usuario.idUsuario, "OPCIONESUSUARIO_ADMIN-M")
        permisoQry = Utilidades.FindPermissionFor(mi_usuario.idUsuario, "OPCIONESUSUARIO_ADMIN-C")
        'Boton de "NUEVO"
        'btnAddOpcion.Visible = permisoAdd
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

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            idUsuario = CType(Request.QueryString("uid"), Integer)
            linkAgregar.NavigateUrl = String.Format(linkAgregar.NavigateUrl, idUsuario.ToString())
        Catch ex As Exception

        End Try
        If Request.QueryString("pid") <> "" And Request.QueryString("uid") <> "" Then
            Dim idPersona As Integer = CType(Request.QueryString("pid"), Integer)
            Dim tblUsuario As New rastreo_usuarios(cnn_str.CadenaDeConexion)
            tblUsuario.Where.Id_persona.Value = idPersona
            tblUsuario.Where.Idrastreo_usuarios.Value = idUsuario
            If tblUsuario.Query.Load() Then
                If tblUsuario.RowCount > 0 Then
                    idUsuario = tblUsuario.Idrastreo_usuarios
                    usuario = tblUsuario.Usuario & "  -  " & tblUsuario.Nombre_completo
                Else
                    Exit Sub
                End If
            Else
                Exit Sub
            End If
        End If
    End Sub

    'Protected Sub btnAddOpcion_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddOpcion.Click
    '    Try
    '        Server.TransferRequest("admingps_opciones_usuario.aspx?uid=" + idUsuario.ToString, False)
    '    Catch X As Exception
    '        Response.Write(X.Message)
    '    End Try
    'End Sub

    Protected Sub gv_Lista_Opciones_RowDeleted(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeletedEventArgs) Handles gv_Lista_Opciones.RowDeleted
        Response.Redirect(Request.Url.ToString, True)
    End Sub

    Protected Sub gv_Lista_Opciones_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gv_Lista_Opciones.RowDeleting
        Try
            Dim Indx As Integer = 0
            Indx = CType(e.Values("opcion_de_pantalla"), Integer)
            Dim tblOPUSU As New rastreogps_usuario_opciones(cnn_str.CadenaDeConexion)
            tblOPUSU.LoadByPrimaryKey(Indx, idUsuario)
            tblOPUSU.MarkAsDeleted()
            tblOPUSU.Save()


        Catch myex As Exception
            Session.Add("msjerror", myex.Message + "<br>" + myex.StackTrace)
        End Try
    End Sub

End Class



