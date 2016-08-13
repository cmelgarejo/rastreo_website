Imports RASTREOmw
Imports MyGeneration.dOOdads

Partial Class RASTREO_Administracion_admin_cliente_usuarios
    Inherits System.Web.UI.Page

    Dim idPersona As Integer = 0
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
        permisoAdd = Utilidades.FindPermissionFor(mi_usuario.idUsuario, "USUARIOS_ADMIN-A")
        permisoDel = Utilidades.FindPermissionFor(mi_usuario.idUsuario, "USUARIOS_ADMIN-B")
        permisoMod = Utilidades.FindPermissionFor(mi_usuario.idUsuario, "USUARIOS_ADMIN-M")
        permisoQry = Utilidades.FindPermissionFor(mi_usuario.idUsuario, "USUARIOS_ADMIN-C")
        'Boton de "NUEVO"
        btnEdit_UsuarioSistema.Visible = permisoAdd
        'Esto solo funciona si la grilla siempre tiene "Editar" en penultimo lugar.
        gv_Usuarios.Columns(gv_Usuarios.Columns.Count - 2).Visible = permisoMod
        'Esto solo funciona si la grilla siempre tiene "Eliminar" al ultimo.
        gv_Usuarios.Columns(gv_Usuarios.Columns.Count - 1).Visible = permisoDel
        'Aqui seteamos a NADA el datasource si no tiene permisos para consultar...
        If Not permisoQry Then gv_Usuarios.Visible = False

        gv_Usuarios.DataBind()
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Request.QueryString("id") <> String.Empty Then
            idPersona = CType(Request.QueryString("id"), Integer)
        End If
        If Not IsPostBack Then
            lblCliente.Text = String.Empty
            If idPersona > 0 Then
                Dim tblPersona As New rastreo_persona(cnn_str.CadenaDeConexion)
                With tblPersona
                    .LoadByPrimaryKey(idPersona)
                    If .RowCount > 0 And .s_Nro_documento <> String.Empty Then
                        Select Case .Tipo_persona
                            Case "F"
                                lblCliente.Text = .s_Nombre + " " + .s_Apellido
                            Case "J"
                                lblCliente.Text = .s_Razon_social
                        End Select
                    End If
                End With

            End If
        End If
    End Sub

    Protected Sub gv_Usuarios_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_Usuarios.RowCommand
        If permisoDel And e.CommandName = "Eliminar" Then
            Dim Indx As Integer = 0
            Indx = CType(e.CommandArgument, Integer)
            Dim tblDelete As New rastreo_usuarios(cnn_str.CadenaDeConexion)
            tblDelete.LoadByPrimaryKey(Indx)
            tblDelete.MarkAsDeleted()
            tblDelete.Save()
            Response.Redirect(Request.Url.ToString)
        ElseIf permisoMod And e.CommandName = "Editar" Then
            ''Si se cambia el layout de la grilla, cambiar esto tambien.
            Dim user As String = CType(sender.Rows(e.CommandArgument).Cells(1), System.Web.UI.WebControls.DataControlFieldCell).Text
            Dim nombre_completo As String = CType(sender.Rows(e.CommandArgument).Cells(5).Controls(1).Controls(1), System.Web.UI.WebControls.TextBox).Text
            Dim pass As String = CType(sender.Rows(e.CommandArgument).Cells(5).Controls(1).Controls(3), System.Web.UI.WebControls.TextBox).Text
            Dim su As Boolean = CType(sender.Rows(e.CommandArgument).Cells(5).Controls(1).Controls(5), System.Web.UI.WebControls.CheckBox).Checked
            Dim indx As Integer = 0
            Integer.TryParse(CType(e.CommandSource, System.Web.UI.WebControls.Button).ToolTip, indx)
            SaveUser(nombre_completo, user, pass, su, indx)
        End If
    End Sub

    Protected Sub gv_Usuarios_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gv_Usuarios.RowDeleting

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

    Protected Sub btnSave_Usuario_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave_Usuario.Click
        SaveUser(txtNOMBRE_usuario.Text, txtUSUARIO_usuario.Text.Trim(), txtUSUARIO_password.Text.Trim())
    End Sub

    Public Function SaveUser(ByVal nombre_completo As String, ByVal usuario As String, ByVal pass As String, Optional ByVal SU As Boolean = False, Optional ByVal idusuario As Integer = 0) As Boolean
        Dim mensaje As String = String.Empty
        If usuario <> String.Empty Then
            Dim tblUSER As New rastreo_usuarios(cnn_str.CadenaDeConexion)
            With tblUSER
                .Where.Usuario.Value = usuario.Trim
                If .Query.Load() Then
                    If .RowCount >= 1 Then
                        If .s_Usuario = usuario And idusuario = 0 Then
                            Session.Add("msjerror", "El usuario ya existe, elija otro nombre de usuario.")
                            Return False
                        End If
                    End If
                End If
                .FlushData()
                .LoadByPrimaryKey(idusuario)
                If .Usuario = String.Empty Then
                    .AddNew()
                    .Nombre_completo = txtNOMBRE_usuario.Text.ToUpperInvariant().Trim
                    .s_Usuario = usuario.Trim
                    .Fech_ins = Now
                    .User_ins = mi_usuario.idUsuario
                    .Id_persona = idPersona
                    mensaje = "Usuario creado satisfactoriamente."
                Else
                    .Fech_upd = Now
                    .User_upd = mi_usuario.idUsuario
                    mensaje = "Usuario actualizado satisfactoriamente."
                End If
                .Su = SU
                If pass <> String.Empty Then
                    .Intentos_login = 0
                    .s_Pass = Cryptografia.SHA256Hash(pass)
                End If
                If nombre_completo <> String.Empty Then
                    .Nombre_completo = nombre_completo.ToUpperInvariant().Trim()
                End If
                .Save()
                Dim tblOpcionesSistema As New rastreogps_opciones_de_pantalla(cnn_str.CadenaDeConexion)
                tblOpcionesSistema.Where.Opcion_de_pantalla.Value = "MENU_USUARIOS"
                If tblOpcionesSistema.Query.Load() Then
                    If tblOpcionesSistema.s_Idrastreogps_opciones_de_pantalla <> String.Empty Then
                        Dim tblOpcionesUsuario As New rastreogps_usuario_opciones(cnn_str.CadenaDeConexion)
                        tblOpcionesUsuario.LoadByPrimaryKey(tblOpcionesSistema.Idrastreogps_opciones_de_pantalla, .Idrastreo_usuarios)
                        If tblOpcionesUsuario.s_Opcion_de_pantalla = String.Empty Then
                            tblOpcionesUsuario.AddNew()
                            tblOpcionesUsuario.Id_usuario = .Idrastreo_usuarios
                            tblOpcionesUsuario.Opcion_de_pantalla = tblOpcionesSistema.Idrastreogps_opciones_de_pantalla
                            tblOpcionesUsuario.Save()
                        End If
                    End If
                End If
            End With
            sqlds_usuarios.DataBind()
            gv_Usuarios.DataBind()
            Session.Add("msjerror", mensaje)
            Return True
        Else
            Return False
        End If
    End Function

    Protected Sub btnEdit_UsuarioSistema_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEdit_UsuarioSistema.Click
        txtNOMBRE_usuario.Focus()
    End Sub
End Class
