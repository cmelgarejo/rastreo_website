Imports RASTREOmw
Imports MyGeneration.dOOdads

Partial Class RASTREO_Administracion_admin_cliente_vehiculo_lista
    Inherits System.Web.UI.Page

    Dim idPersona As Integer = 0
    Dim idUsuario As Integer = 0
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
        permisoQry = Utilidades.FindPermissionFor(mi_usuario.idUsuario, "USUARIOS_ADMIN-C")
        'Aqui seteamos a NADA el datasource si no tiene permisos para consultar...
        If Not permisoQry Then gv_Lista_Vehiculos.Visible = False
        gv_Lista_Vehiculos.DataBind()
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Request.QueryString.Count > 0 Then
            If Request.QueryString("id") <> String.Empty Then
                Integer.TryParse(Request.QueryString("id"), idPersona)
            End If
            If Request.QueryString("uid") <> String.Empty Then
                Integer.TryParse(Request.QueryString("uid"), idUsuario)
            End If
        Else
            Response.Redirect("admin_personas_lista.aspx", False)
        End If
        If Not IsPostBack Then
            lblCliente.Text = String.Empty
            lblUser.Text = String.Empty
            If idPersona > 0 Then
                Dim tblInfo As New rsview_cliente_usuarios(cnn_str.CadenaDeConexion)
                With tblInfo
                    .Where.Id_persona.Value = idPersona
                    .Where.Idrastreo_usuarios.Value = idUsuario
                    If .Query.Load() Then
                        If .RowCount > 0 And .s_Id_persona <> String.Empty Then
                            lblCliente.Text = .Cliente
                            lblUser.Text = .Usuario
                        End If
                    End If
                End With
            Else

            End If
        End If
    End Sub

    Protected Sub gvListaUser_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvListaUser.RowCommand
        If e.CommandName = "Eliminar" Then
            If Not e.CommandArgument Is Nothing Then
                Dim idVehiculo As Integer = 0
                Integer.TryParse(e.CommandArgument, idVehiculo)
                If idVehiculo > 0 Then
                    Dim tblListaVehiculosUsuario As New rastreo_usuario_lista_vehiculo(cnn_str.CadenaDeConexion)
                    With tblListaVehiculosUsuario
                        .Where.Id_cliente.Value = idPersona
                        .Where.Id_usuarios.Value = idUsuario
                        .Where.Id_vehiculo.Value = idVehiculo
                        If .Query.Load() Then
                            If .RowCount > 0 Then
                                If .s_Id_listavehiculos <> String.Empty Then
                                    .MarkAsDeleted()
                                    .Save()
                                    Session.Add("msjerror", "El vehiculo fue eliminado de la lista del usuario.")
                                End If
                            End If
                        End If
                    End With
                End If
            End If
        End If
        sqlds_listauser.DataBind()
        sqlds_Lista_Cliente_Vehiculo.DataBind()
        gv_Lista_Vehiculos.DataBind()
        gvListaUser.DataBind()
    End Sub

    Protected Sub gv_Lista_Vehiculos_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_Lista_Vehiculos.RowCommand
        Try
            If e.CommandName = "Agregar" Then
                If Not e.CommandArgument Is Nothing Then
                    Dim idVehiculo As Integer = 0
                    Integer.TryParse(e.CommandArgument, idVehiculo)
                    If idVehiculo > 0 Then
                        Dim tblListaVehiculosUsuario As New rastreo_usuario_lista_vehiculo(cnn_str.CadenaDeConexion)
                        With tblListaVehiculosUsuario
                            .AddNew()
                            .Id_cliente = idPersona
                            .Id_usuarios = idUsuario
                            .Id_vehiculo = idVehiculo
                            .Visible = True
                            .Save()
                            Session.Add("msjerror", "El vehiculo es ahora visible por el usuario.")
                        End With
                    End If
                End If
            End If
            sqlds_listauser.DataBind()
            sqlds_Lista_Cliente_Vehiculo.DataBind()
            gv_Lista_Vehiculos.DataBind()
            gvListaUser.DataBind()
        Catch XE As Exception
            Session.Add("msjerror", XE.ToString())
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
            Session.Add("msjerror", myEX.Message + "<br>" + myEX.StackTrace)
        End Try
    End Sub

    
End Class
