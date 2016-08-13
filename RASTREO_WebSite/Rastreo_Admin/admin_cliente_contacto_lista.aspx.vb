Imports RASTREOmw
Imports MyGeneration.dOOdads

Partial Class RASTREO_Administracion_admin_cliente_contacto_lista
    Inherits System.Web.UI.Page

    Dim idCLIENTE As Integer = 0
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
        permisoAdd = Utilidades.FindPermissionFor(mi_usuario.idUsuario, "CONTACTOS_ADMIN-A")
        permisoDel = Utilidades.FindPermissionFor(mi_usuario.idUsuario, "CONTACTOS_ADMIN-B")
        permisoMod = Utilidades.FindPermissionFor(mi_usuario.idUsuario, "CONTACTOS_ADMIN-M")
        permisoQry = Utilidades.FindPermissionFor(mi_usuario.idUsuario, "CONTACTOS_ADMIN-C")
        'Boton de "NUEVO"
        btnAddContacto.Visible = permisoAdd
        'Esto solo funciona si la grilla siempre tiene "Editar" en penultimo lugar.
        gv_Lista_Contactos.Columns(gv_Lista_Contactos.Columns.Count - 2).Visible = permisoMod
        'Esto solo funciona si la grilla siempre tiene "Eliminar" al ultimo.
        gv_Lista_Contactos.Columns(gv_Lista_Contactos.Columns.Count - 1).Visible = permisoDel
        'Aqui seteamos a NADA el datasource si no tiene permisos para consultar...
        If Not permisoQry Then gv_Lista_Contactos.Visible = False

        gv_Lista_Contactos.DataBind()
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Request.QueryString("id") <> String.empty Then
            idCLIENTE = CType(Request.QueryString("id"), Integer)
        End If
        If Not IsPostBack Then
            BindDDLBusqueda()
            lblCliente.Text = String.empty
            If idCLIENTE > 0 Then
                Dim tblCliente As New rastreo_persona(cnn_str.CadenaDeConexion)
                With tblCliente
                    .LoadByPrimaryKey(idCLIENTE)
                    If .RowCount > 0 And .s_Nro_documento <> String.empty Then
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

    Protected Sub btnAddContacto_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddContacto.Click

        Response.Redirect("admin_cliente_contacto.aspx?cid=" + idCLIENTE.ToString, False)
    End Sub

    Protected Sub gv_Lista_Contactos_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_Lista_Contactos.RowCommand
        If permisoDel Then
            Dim Indx As Integer = 0
            Indx = CType(e.CommandArgument, Integer)
            Dim tblContactos As New rastreo_cliente_contactos(cnn_str.CadenaDeConexion)
            tblContactos.LoadByPrimaryKey(Indx, idCLIENTE)
            tblContactos.MarkAsDeleted()
            tblContactos.Save()
            Response.Redirect(Request.Url.ToString)
        End If
    End Sub

    Protected Sub gv_Lista_Contactos_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gv_Lista_Contactos.RowDeleting

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

    Private Function BindearGrilla_Personas(Optional ByVal TodosLosRegistros As Boolean = False) As Boolean
        Dim tblPersonasSearch As New rastreo_cliente_contactos(cnn_str.CadenaDeConexion)

        Try
            With tblPersonasSearch
                If TodosLosRegistros Then
                    gv_Lista_Contactos.DataSourceID = sqlds_Lista_Cliente_Contacto.ID
                    Return True
                Else
                    Dim Columna As String = Me.ddlBuscar.SelectedItem.Value.ToString().Trim()
                    Dim Valor As String = Me.txtBuscar.Text
                    If Valor <> String.Empty And Columna <> String.Empty Then
                        If Columna <> String.empty Then ' Se ha seleccionado alguna columna por la cual buscar?
                            Dim MyWhere As New WhereParameter(Columna, _
                                New Npgsql.NpgsqlParameter("@" & Columna, Valor))
                            MyWhere.Value = "%" & Valor & "%"
                            MyWhere.Operator = WhereParameter.Operand.ILike
                            .Query.AddWhereParameter(MyWhere)
                            .Query.Load()
                            gv_Lista_Contactos.DataSourceID = String.Empty
                            gv_Lista_Contactos.DataSource = .DefaultView
                        End If
                    End If
                End If


                If .RowCount < 1 Then
                    ViewState.Add("msjerror", "La busqueda no produjo ningun resultado.")
                    Return False
                End If

                gv_Lista_Contactos.DataBind()
                Return True
            End With


        Catch MYEX As Exception
            ViewState.Add("msjerror", MYEX.Message + "<br>" + MYEX.StackTrace)
            Return False
        End Try
    End Function

    Private Sub BindDDLBusqueda()
        Try
            With ddlBuscar
                .Items.Add(New ListItem("Nombre o Razon Social", rastreo_cliente_contactos.ColumnNames.Nombre_apellido_razonsocial))
                .Items.Add(New ListItem("Nro. Documento", rastreo_cliente_contactos.ColumnNames.Nrodoc_ruc))
                .Items.Add(New ListItem("Telefonos", rastreo_cliente_contactos.ColumnNames.Telefono))
                .Items.Add(New ListItem("e-mails", rastreo_cliente_contactos.ColumnNames.Email))
            End With
        Catch MYEX As Exception
            ViewState.Add("msjerror", MYEX.Message + "<br>" + MYEX.StackTrace)
        End Try
    End Sub

    Protected Sub btnBuscar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        BindearGrilla_Personas()
    End Sub

    Protected Sub btnLimpiarBusqueda_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLimpiarBusqueda.Click
        BindearGrilla_Personas(True)
    End Sub

End Class
