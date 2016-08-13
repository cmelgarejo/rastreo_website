Imports RASTREOmw
Imports MyGeneration.dOOdads

Partial Class RASTREO_Administracion_admin_historial
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
        permisoAdd = Utilidades.FindPermissionFor(mi_usuario.idUsuario, "HISTORIAL_ADMIN-A")
        permisoDel = Utilidades.FindPermissionFor(mi_usuario.idUsuario, "HISTORIAL_ADMIN-B")
        permisoMod = Utilidades.FindPermissionFor(mi_usuario.idUsuario, "HISTORIAL_ADMIN-M")
        permisoQry = Utilidades.FindPermissionFor(mi_usuario.idUsuario, "HISTORIAL_ADMIN-C")
        'Boton de "NUEVO"
        btnAddEvento.Visible = permisoAdd
        'Esto solo funciona si la grilla siempre tiene "Editar" en penultimo lugar.
        gv_Lista_Historial.Columns(gv_Lista_Historial.Columns.Count - 2).Visible = permisoMod
        'Esto solo funciona si la grilla siempre tiene "Eliminar" al ultimo.
        gv_Lista_Historial.Columns(gv_Lista_Historial.Columns.Count - 1).Visible = permisoDel
        'Aqui seteamos a NADA el datasource si no tiene permisos para consultar...
        If Not permisoQry Then gv_Lista_Historial.Visible = False
        gv_Lista_Historial.DataBind()
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Request.QueryString("cid") <> String.Empty Then
            idCLIENTE = CType(Request.QueryString("cid"), Integer)
        End If
        If Not IsPostBack Then
            BindVehiculos()
            BindDDLBusqueda()
            lblCliente.Text = String.Empty
            If idCLIENTE > 0 Then
                Dim tblCliente As New rastreo_persona(cnn_str.CadenaDeConexion)
                With tblCliente
                    .LoadByPrimaryKey(idCLIENTE)
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

    Protected Sub btnAddEvento_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddEvento.Click
        updpnlAddHistorial.Visible = Not updpnlAddHistorial.Visible
        If updpnlAddHistorial.Visible Then
            listVehiculosInvolucrados.Items.Clear()
            btnAddEvento.Text = "Cerrar"
        Else
            btnAddEvento.Text = "Agregar evento"
        End If
    End Sub

    Protected Sub gv_Lista_Historial_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_Lista_Historial.RowCommand
        Dim idrastreo_historial As Integer = CType(e.CommandArgument, Integer)
        If e.CommandName = "Editar" Then
            updpnlAddHistorial.Visible = True
            If idrastreo_historial > 0 Then
                Dim tblH As New rastreo_historial(cnn_str.CadenaDeConexion)
                With tblH
                    .LoadByPrimaryKey(idrastreo_historial, idCLIENTE)
                    If .RowCount > 0 Then
                        If .s_Idrastreo_historial <> String.Empty Then
                            txtHISTORIAL_fechaevento.Text = .Fecha_verificacion.ToString(System.Globalization.DateTimeFormatInfo.CurrentInfo)
                            txtObservacion.Text = .s_Observacion
                            listVehiculosInvolucrados.Items.Clear()
                            For Each vehiculo As String In .Vehiculos_involucrados.Split(";")
                                listVehiculosInvolucrados.Items.Add(vehiculo)
                            Next
                            ViewState.Add("editHistorial", True)
                            ViewState.Add("editINDEX", idrastreo_historial)
                        End If
                    End If
                End With
            End If
        ElseIf e.CommandName = "Eliminar" Then
            Dim tblDel As New rastreo_historial(cnn_str.CadenaDeConexion)
            With tblDel
                .LoadByPrimaryKey(idrastreo_historial, idCLIENTE)
                .MarkAsDeleted()
                .Save()
            End With
        End If
    End Sub

    Protected Sub gv_Historial_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gv_Lista_Historial.RowDeleting
        Dim Indx As Integer = 0
        Indx = CType(e.Values("idrastreo_historial"), Integer)
        Dim tblH As New rastreo_historial(cnn_str.CadenaDeConexion)
        tblH.LoadByPrimaryKey(Indx, idCLIENTE)
        tblH.MarkAsDeleted()
        tblH.Save()
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

    Private Function BindearGrilla_Historial(Optional ByVal TodosLosRegistros As Boolean = False) As Boolean
        Dim tblHistorialSearch As New rastreo_historial(cnn_str.CadenaDeConexion)

        Try
            With tblHistorialSearch
                If TodosLosRegistros Then
                    gv_Lista_Historial.DataSourceID = sqlds_Lista_Historial.ID
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
                            .Query.Load()
                            gv_Lista_Historial.DataSourceID = String.Empty
                            gv_Lista_Historial.DataSource = .DefaultView
                        End If
                    End If
                End If


                If .RowCount < 1 Then
                    ViewState.Add("msjerror", "La busqueda no produjo ningun resultado.")
                    Return False
                End If

                gv_Lista_Historial.DataBind()
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
                .Items.Add(New ListItem("Vehiculos involucrados", rastreo_historial.ColumnNames.Vehiculos_involucrados))
                .Items.Add(New ListItem("Observacion", rastreo_historial.ColumnNames.Observacion))
            End With
        Catch MYEX As Exception
            ViewState.Add("msjerror", MYEX.Message + "<br>" + MYEX.StackTrace)
        End Try
    End Sub

    Protected Sub btnBuscar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        BindearGrilla_Historial()
    End Sub

    Protected Sub btnLimpiarBusqueda_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLimpiarBusqueda.Click
        BindearGrilla_Historial(True)
    End Sub

    Private Sub BindVehiculos()
        Dim tblVehiculo As New rastreo_vehiculo(cnn_str.CadenaDeConexion)
        tblVehiculo.Where.Id_cliente.Value = idCLIENTE
        Utilidades.fillDDL(ddl_Vehiculos, tblVehiculo, rastreo_vehiculo.ColumnNames.Identificador_rastreo, _
                           rastreo_vehiculo.ColumnNames.Idrastreo_vehiculo)
    End Sub

    Protected Sub btnGUARDAR_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGUARDAR.Click
        Dim tblH As New rastreo_historial(cnn_str.CadenaDeConexion)
        Try
            With tblH
                Dim edit As Boolean = False
                Boolean.TryParse(ViewState("editHistorial"), edit)
                If edit Then
                    Dim editItem As Integer = 0
                    If Not Integer.TryParse(ViewState("editINDEX"), editItem) Then
                        Session.Add("msjerror", "Error al tratar de obtener el indice del historial a editar.")
                        Return
                    End If
                    .LoadByPrimaryKey(editItem, idCLIENTE)
                    .Fech_upd = Now
                    .User_upd = mi_usuario.idUsuario
                Else
                    .AddNew()
                    .Id_cliente = idCLIENTE
                    .Fech_ins = Now
                    .User_ins = mi_usuario.idUsuario
                End If
                Dim listadevehiculos As String = String.Empty
                For Each item As ListItem In listVehiculosInvolucrados.Items
                    If listadevehiculos = String.Empty Then
                        listadevehiculos += item.Text
                    Else
                        listadevehiculos += ";" + item.Text
                    End If
                Next
                .s_Fecha_verificacion = CType(txtHISTORIAL_fechaevento.Text, DateTime)
                .Vehiculos_involucrados = listadevehiculos.ToUpperInvariant()
                .Observacion = txtObservacion.Text.Trim.ToUpperInvariant()
                .Save()
            End With
            gv_Lista_Historial.DataBind()
            updpnlAddHistorial.Visible = False
            btnAddEvento.Text = "Agregar evento"
        Catch MYEX As Exception
            Session.Add("msjerror", MYEX.Message + "<br>" + MYEX.StackTrace)
        End Try
    End Sub

    Protected Sub btnCANCELAR_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCANCELAR.Click
        updpnlAddHistorial.Visible = False
    End Sub

    Protected Sub btnAgregarVehiculo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAgregarVehiculo.Click
        Try
            If Not listVehiculosInvolucrados.Items.FindByText(ddl_Vehiculos.SelectedItem.Text) Is Nothing Then
                Session.Add("msjerror", "El vehiculo " + ddl_Vehiculos.SelectedItem.Text + " ya está en la lista")
                Return
            End If
            listVehiculosInvolucrados.Items.Add(ddl_Vehiculos.SelectedItem.Text)
        Catch MYEX As Exception
            Session.Add("msjerror", MYEX.Message + "<br>" + MYEX.StackTrace)
        End Try
    End Sub

    Protected Sub btnEliminarVehiculo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEliminarVehiculo.Click
        Try
            If Not listVehiculosInvolucrados.SelectedItem Is Nothing Then
                listVehiculosInvolucrados.Items.Remove(listVehiculosInvolucrados.SelectedItem.Text)
            Else
                Session.Add("msjerror", "No ha seleccionado nada a eliminar")
                Return
            End If
        Catch MYEX As Exception
            Session.Add("msjerror", MYEX.Message + "<br>" + MYEX.StackTrace)
        End Try
    End Sub

    Protected Sub gv_Lista_Historial_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gv_Lista_Historial.SelectedIndexChanged

    End Sub
End Class
