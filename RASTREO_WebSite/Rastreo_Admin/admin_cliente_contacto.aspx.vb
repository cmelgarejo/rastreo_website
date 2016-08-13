Imports MyGeneration.dOOdads
Imports RASTREOmw

Partial Class RASTREO_Administracion_admin_cliente_contacto
    Inherits System.Web.UI.Page
    Public mi_usuario As New Usuario()
    Dim idrastreo_cliente_contacto As Integer = 0
    Dim id_cliente As Integer = 0

    Private permisoAdd As Boolean = False
    Private permisoMod As Boolean = False
    Private permisoQry As Boolean = False

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Try
            If Not Session("session_UsuarioRASTREO") Is Nothing Then
                mi_usuario = CType(Session("session_UsuarioRASTREO"), Usuario)
            Else
                Response.Redirect("../Login.aspx?rp=" + Me.Request.Url.AbsoluteUri(), False)
            End If
            permisoAdd = Utilidades.FindPermissionFor(mi_usuario.idUsuario, "CONTACTOS_ADMIN-A")
            permisoMod = Utilidades.FindPermissionFor(mi_usuario.idUsuario, "CONTACTOS_ADMIN-M")
            permisoQry = Utilidades.FindPermissionFor(mi_usuario.idUsuario, "CONTACTOS_ADMIN-C")
            If Not permisoAdd Or Not permisoMod Or Not permisoQry Then
                Response.Redirect("admin_personas_lista.aspx", False)
            End If
        Catch myEX As Exception
            Session.Add("msjerror", myEX.Message + "<br>" + myEX.StackTrace)
        End Try
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

            id_cliente = CType(Request.QueryString("cid"), Integer)
            idrastreo_cliente_contacto = CType(Request.QueryString("id"), Integer)

            If Not IsPostBack Then
                If Not Request.UrlReferrer Is Nothing And ViewState("GO_BACK_URL") Is Nothing Then
                    ViewState.Add("GO_BACK_URL", Request.UrlReferrer.ToString)
                End If
                If idrastreo_cliente_contacto > 0 And id_cliente > 0 Then
                    ViewState.Add("admin_cliente_contacto_update", True)
                    CargarDatos()
                    btnGUARDAR.Text = "Actualizar"
                    btnCANCELAR.Text = "Volver"
                End If
            End If

            txtCONTACTO_nomb.Focus()

        Catch myEX As Exception
            ViewState.Add("msjerror", myEX.Message + "<br>" + myEX.StackTrace)
        End Try

    End Sub

    Protected Sub btnGUARDAR_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGUARDAR.Click
        Guardar_Datos()
        If ViewState("GO_BACK_URL") <> String.Empty Then
            Response.Redirect(ViewState("GO_BACK_URL"))
        Else
            JavascriptHelper.Custom(Page, "history.back();", "goback")
        End If
    End Sub

    Private Sub CargarDatos()
        Try
            Dim tblCONTACTOS As New rastreo_cliente_contactos(cnn_str.CadenaDeConexion)
            With tblCONTACTOS
                .LoadByPrimaryKey(idrastreo_cliente_contacto, id_cliente)
                txtCONTACTO_relacion.Text = .s_Relacion
                txtCONTACTO_mail.Text = .Email
                txtCONTACTO_nomb.Text = .Nombre_apellido_razonsocial
                txtCONTACTO_nrodoc.Text = .Nrodoc_ruc
                txtCONTACTO_telefonos.Text = .Telefono
                If .s_Autorizado <> String.Empty Then
                    chkAutorizado.Checked = .Autorizado
                Else
                    chkAutorizado.Checked = False
                End If
                If .s_Titular_secundario <> String.Empty Then
                    chkTitularSecundario.Checked = .Titular_secundario
                Else
                    chkTitularSecundario.Checked = False
                End If

            End With
        Catch myEX As Exception
            ViewState.Add("msjerror", myEX.Message + "<br>" + myEX.StackTrace)
        End Try
    End Sub

    Private Sub Guardar_Datos()
        Dim tblCONTACTOS As New rastreo_cliente_contactos(cnn_str.CadenaDeConexion)
        Dim Transaccion As TransactionMgr = TransactionMgr.ThreadTransactionMgr
        Try
            Transaccion.BeginTransaction()
            With tblCONTACTOS
                If ViewState("admin_cliente_contacto_update") Then
                    .LoadByPrimaryKey(idrastreo_cliente_contacto, id_cliente)
                    .Fech_upd = Now
                    .User_upd = mi_usuario.idUsuario
                Else
                    .AddNew()
                    .Fech_ins = Now
                    .User_ins = mi_usuario.idUsuario
                    .s_Rastreo_cliente_id_cliente = id_cliente
                End If
                .s_Relacion = txtCONTACTO_relacion.Text.Trim.ToUpperInvariant()
                .s_Nombre_apellido_razonsocial = txtCONTACTO_nomb.Text.Trim.ToUpperInvariant()
                .s_Nrodoc_ruc = txtCONTACTO_nrodoc.Text.Trim.ToUpperInvariant()
                .s_Email = txtCONTACTO_mail.Text.Trim()
                .s_Telefono = txtCONTACTO_telefonos.Text.Trim.ToUpperInvariant()
                .Autorizado = chkAutorizado.Checked
                .Titular_secundario = chkTitularSecundario.Checked
                .Save()
            End With
            Transaccion.CommitTransaction()

            If Not ViewState("admin_cliente_contacto_update") Then
                Session.Add("msjerror", "Contacto registrado exitosamente!")
            Else
                Session.Add("msjerror", "Contacto actualizado exitosamente!")
            End If
        Catch myEX As Exception
            ViewState.Add("msjerror", myEX.Message + "<br>" + myEX.StackTrace)
            If Not Transaccion.HasBeenRolledBack Then Transaccion.RollbackTransaction()
        End Try
    End Sub


    Protected Sub btnCANCELAR_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCANCELAR.Click
        Try

        Catch myEX As Exception
            ViewState.Add("msjerror", myEX.Message + "<br>" + myEX.StackTrace)
        End Try
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
End Class
