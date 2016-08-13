Imports RASTREOmw
Imports MyGeneration.dOOdads

Partial Class RASTREO_Administracion_admin_equipogps_lista
    Inherits System.Web.UI.Page

    Dim idEQUIPOGPS As Integer = 0
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
        permisoAdd = Utilidades.FindPermissionFor(mi_usuario.idUsuario, "EQUIPOSGPS_ADMIN-A")
        permisoDel = Utilidades.FindPermissionFor(mi_usuario.idUsuario, "EQUIPOSGPS_ADMIN-B")
        permisoMod = Utilidades.FindPermissionFor(mi_usuario.idUsuario, "EQUIPOSGPS_ADMIN-M")
        permisoQry = Utilidades.FindPermissionFor(mi_usuario.idUsuario, "EQUIPOSGPS_ADMIN-C")
        'Boton de "NUEVO"
        btnAddEquipoGPS.Visible = permisoAdd
        'Esto solo funciona si la grilla siempre tiene "Editar" en penultimo lugar.
        gv_Lista_EquiposGPS.Columns(gv_Lista_EquiposGPS.Columns.Count - 2).Visible = permisoMod
        'Esto solo funciona si la grilla siempre tiene "Eliminar" al ultimo.
        gv_Lista_EquiposGPS.Columns(gv_Lista_EquiposGPS.Columns.Count - 1).Visible = permisoDel
        'Aqui seteamos a NADA el datasource si no tiene permisos para consultar...
        If Not permisoQry Then gv_Lista_EquiposGPS.Visible = False
        gv_Lista_EquiposGPS.DataBind()
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Request.QueryString("id") <> String.empty Then
            idEQUIPOGPS = CType(Request.QueryString("id"), Integer)
        Else

        End If
        If Not IsPostBack Then
            BindDDLBusqueda()
            If Not Request.UrlReferrer Is Nothing And ViewState("GO_BACK_URL") Is Nothing Then
                Session.Add("GO_BACK_URL", Request.UrlReferrer.ToString)
            End If
        End If
    End Sub

    Protected Sub btnAddEquipoGPS_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddEquipoGPS.Click
        Try
            Response.Redirect("admingps_equipogps.aspx?id=" + idEQUIPOGPS.ToString, False)
        Catch X As Exception
            Response.Write(X.Message)
        End Try

    End Sub

    Protected Sub gv_Lista_EquiposGPS_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_Lista_EquiposGPS.RowCommand
        Dim Indx As Integer = 0
        Try
            If e.CommandName = "Eliminar" Then
                If permisoDel Then
                    Indx = CType(e.CommandArgument, Integer)
                    Dim tblDelete As New rastreogps_equipogps(cnn_str.CadenaDeConexion)
                    tblDelete.LoadByPrimaryKey(Indx)
                    tblDelete.MarkAsDeleted()
                    tblDelete.Save()
                    Response.Redirect(Request.Url.ToString)
                End If
            End If
        Catch NpgsqlEX As Npgsql.NpgsqlException
            If NpgsqlEX.Code = "23503" Then
                Dim tblBuscarRegistro As New rsview_cliente_vehiculo_equipogps(cnn_str.CadenaDeConexion)
                With tblBuscarRegistro
                    .Where.Idequipogps.Value = Indx
                    If .Query.Load() Then
                        If .Id_equipo_gps <> String.Empty Then
                            Session.Add("msjerror", "El equipo " + _
                                        .Id_equipo_gps + " está instalado en el vehiculo " + _
                                        " con contraseña " + .Identificador_rastreo + _
                                        " instalado por: " + .Instalador)
                        End If
                    End If
                End With
            End If
        Catch EX As Exception
            Session.Add("msjerror", EX.Message + "<br>" + EX.StackTrace)
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
                Session.Remove("msjerror")
            Else
                errMsgs.Visible = False
            End If
        Catch myEX As Exception
            Session.Add("msjerror", myEX.Message + "<br>" + myEX.StackTrace)
        End Try
    End Sub

    Private Function BindearGrilla_EquipoGPS(Optional ByVal TodosLosRegistros As Boolean = False) As Boolean
        Dim tblSearch As New rsview_equipo_tipoequipo(cnn_str.CadenaDeConexion)
        Try
            With tblSearch
                If TodosLosRegistros Then
                    gv_Lista_EquiposGPS.DataSourceID = sqlds_Lista_equipogps.ID
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
                            gv_Lista_EquiposGPS.DataSourceID = String.Empty
                            gv_Lista_EquiposGPS.DataSource = .DefaultView
                        End If
                    End If
                End If


                If .RowCount < 1 Then
                    gv_Lista_EquiposGPS.DataBind()
                    Session.Add("msjerror", "La busqueda no produjo ningun resultado.")
                    Return False
                Else
                    gv_Lista_EquiposGPS.PageSize = .RowCount
                End If

                gv_Lista_EquiposGPS.DataBind()
                Return True
            End With


        Catch MYEX As Exception
            Session.Add("msjerror", MYEX.Message + "<br>" + MYEX.StackTrace)
            Return False
        End Try
    End Function

    Private Sub BindDDLBusqueda()
        Try
            With ddlBuscar
                .Items.Add(New ListItem("Id Equipo", rsview_equipo_tipoequipo.ColumnNames.Id_equipo_gps))
                .Items.Add(New ListItem("Vehiculo asociado", rsview_equipo_tipoequipo.ColumnNames.Identificador_rastreo))
                .Items.Add(New ListItem("Tipo Equipo", rsview_equipo_tipoequipo.ColumnNames.Tipo_de_equipo))
                .Items.Add(New ListItem("Nro. Telef. SIM", rsview_equipo_tipoequipo.ColumnNames.Sim_nro))
            End With
        Catch MYEX As Exception
            Session.Add("msjerror", MYEX.Message + "<br>" + MYEX.StackTrace)
        End Try
    End Sub

    Protected Sub btnBuscar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        BindearGrilla_EquipoGPS()
    End Sub

    Protected Sub btnLimpiarBusqueda_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLimpiarBusqueda.Click
        BindearGrilla_EquipoGPS(True)
    End Sub

End Class
