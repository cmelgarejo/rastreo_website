Imports MyGeneration.dOOdads
Imports RASTREOmw
Imports System.IO
Imports Npgsql

Partial Class RASTREO_Administracion_admin_cliente_vehiculo
    Inherits System.Web.UI.Page
    Public mi_usuario As New Usuario()
    Dim idrastreo_vehiculo As Integer = 0
    Dim id_cliente As Integer = 0
    Dim DirectorioImagenes_IconosTransporte As String = Server.MapPath("~/App_Themes/CENTRAL/Imagenes/IconosTransporte/")
    Dim chkvalVEHICULOactivo As Boolean
    Dim chkvalVEHICULOdemo As Boolean

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
            permisoAdd = Utilidades.FindPermissionFor(mi_usuario.idUsuario, "VEHICULOS_ADMIN-A")
            permisoMod = Utilidades.FindPermissionFor(mi_usuario.idUsuario, "VEHICULOS_ADMIN-M")
            permisoQry = Utilidades.FindPermissionFor(mi_usuario.idUsuario, "VEHICULOS_ADMIN-C")
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
            idrastreo_vehiculo = CType(Request.QueryString("id"), Integer)

            If Not IsPostBack Then
                BindInstaladores()
                BindVendedores()
                BindSucursales()
                ObtenerImagenes()
                BindEquipoGPS()
                BindMarcas()
                BindTipoDeVehiculo()
                bindProvieneDe()
                txtVEHICULO_identificador_rastreo.Focus()
                'If Not Request.UrlReferrer Is Nothing And ViewState("GO_BACK_URL") Is Nothing Then
                ' ViewState.Add("GO_BACK_URL", Request.UrlReferrer.ToString)
                'End If

                If idrastreo_vehiculo > 0 And id_cliente > 0 Then
                    ViewState.Add("admin_cliente_vehiculo_update", True)
                    CargarDatos()
                    CheckEquipoGPS()
                    btnGUARDAR.Text = "Actualizar"
                    btnCANCELAR.Text = "Volver"
                End If

            Else

            End If

        Catch myEX As Exception
            Session.Add("msjerror", myEX.Message + "<br>" + myEX.StackTrace)
        End Try

    End Sub

    Protected Sub BindInstaladores()
        Dim tblInstaladores As New rsview_empleados(cnn_str.CadenaDeConexion)
        tblInstaladores.Where.Instalador.Value = True
        Utilidades.fillDDL(ddl_INSTALADORES, tblInstaladores, _
                           rsview_empleados.ColumnNames.Nombre_completo, _
                           rsview_empleados.ColumnNames.Id_empleado)
    End Sub

    '18102010 - Pedro Silva
    Protected Sub BindVendedores()
        Dim tblVendedores As New rsview_empleados(cnn_str.CadenaDeConexion)
        tblVendedores.Where.Vendedor.Value = True
        Utilidades.fillDDL(ddl_Vendedor, tblVendedores, _
                           rsview_empleados.ColumnNames.Nombre_completo, _
                           rsview_empleados.ColumnNames.Id_empleado)
    End Sub

    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        Try

            Dim mensaje As String = String.Empty
            mensaje = ViewState("msjerror")
            If mensaje = String.Empty Then
                mensaje = Session("msjerror")
                Session.Remove("msjerror")
            Else
                ViewState.Remove("msjerror")
            End If
            If mensaje <> String.Empty Then
                errMsgs.Text = mensaje
                errMsgs.Visible = True
            Else
                errMsgs.Visible = False
            End If
        Catch myEX As Exception
            Session.Add("msjerror", myEX.Message + "<br>" + myEX.StackTrace)
        End Try
    End Sub



    Private Sub ObtenerImagenes()
        If Directory.Exists(DirectorioImagenes_IconosTransporte) Then
            Dim MyDIR As New DirectoryInfo(DirectorioImagenes_IconosTransporte)
            Dim Tipos As String = "*.png;*.jpg;*.gif"
            For Each tipo As String In Tipos.Split(";")
                For Each Imagen As FileInfo In MyDIR.GetFiles(tipo, SearchOption.AllDirectories)
                    Dim Item As New ListItem(Imagen.Name, Convertir_Path_ImageUrl(Imagen.FullName))
                    ddlIconosTV.Items.Add(Item)
                Next
            Next
            If ddlIconosTV.Items.Count > 0 Then
                ddlIconosTV.SelectedItem.Selected = False
                ddlIconosTV.SelectedIndex = 0
                ddlIconosTV_SelectedIndexChanged(New Object, New System.EventArgs)
            End If
        End If
    End Sub

    Private Sub BindMarcas()
        Utilidades.fillDDL(ddlMarcas, New rastreo_marca_vehiculo(cnn_str.CadenaDeConexion), _
                           rastreo_marca_vehiculo.ColumnNames.Marca, _
                           rastreo_marca_vehiculo.ColumnNames.Idrastreo_marca_vehiculo)
        If ddlMarcas.SelectedValue <> String.Empty Then
            BindModelos(ddlMarcas.SelectedValue)
        End If
    End Sub

    Protected Sub ddlMarcas_SelectIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlMarcas.SelectedIndexChanged
        BindModelos(ddlMarcas.SelectedValue)
    End Sub

    Private Sub BindModelos(ByVal idMarca As Integer)
        Dim tblmodelos As New rastreo_modelo_vehiculo(cnn_str.CadenaDeConexion)
        With tblmodelos
            .Where.Id_marca.Value = idMarca
            .Query.AddOrderBy(rastreo_modelo_vehiculo.ColumnNames.Modelo, WhereParameter.Dir.ASC)
            If .Query.Load Then
                If .RowCount > 0 Then
                    ddlModelos.DataSource = .DefaultView
                    ddlModelos.DataTextField = rastreo_modelo_vehiculo.ColumnNames.Modelo
                    ddlModelos.DataValueField = rastreo_modelo_vehiculo.ColumnNames.Idrastreo_modelo_vehiculo
                    ddlModelos.DataBind()
                End If
            Else
                ddlModelos.Items.Clear()
                ddlModelos.DataSource = Nothing
                ddlModelos.DataBind()
            End If
        End With
    End Sub


    Private Sub BindSucursales()
        Utilidades.fillDDL(ddlSucursales, New rastreo_sucursales(cnn_str.CadenaDeConexion), _
                        rastreo_sucursales.ColumnNames.Descripcion, _
                        rastreo_sucursales.ColumnNames.Idrastreo_sucursal)
        ddlSucursales.Items.Add("Seleccionar lugar de instalacion, por favor.")
    End Sub

    Private Sub BindTipoDeVehiculo()
        Utilidades.fillDDL(ddlTipoDeVehiculo, New rastreo_tipo_vehiculo(cnn_str.CadenaDeConexion), _
                        rastreo_tipo_vehiculo.ColumnNames.Tipo_de_vehiculo, _
                        rastreo_tipo_vehiculo.ColumnNames.Idrastreo_tipo_vehiculo)
    End Sub

    Private Sub CargarDatos()
        Try
            Dim tblVEHICULO As New rastreo_vehiculo(cnn_str.CadenaDeConexion)
            With tblVEHICULO
                .LoadByPrimaryKey(idrastreo_vehiculo, id_cliente)
                txtVEHICULO_chofer.Text = .s_Chofer
                txtVEHICULO_chofer_contacto.Text = .s_Chofer_contacto
                txtVEHICULO_observaciones.Text = .s_Observacion
                txtVEHICULO_nropoliza.Text = .s_Poliza_nroorden
                txtFicha.Text = .s_Nro_ficha
                txtVEHICULO_fechaemisionpoliza.Text = .s_Poliza_emision
                txtVEHICULO_fechavencimientopoliza.Text = .s_Poliza_venc
                If .s_Instalacion_fechahora <> String.Empty Then
                    txtVEHICULO_fechainstalacion.Text = .Instalacion_fechahora.ToString("dd/MM/yyyy HH:mm")
                    '.Instalacion_fechahora.ToString(System.Globalization.DateTimeFormatInfo.InvariantInfo)
                End If
                If .s_Reinstalacion_fechahora <> String.Empty Then ' Pedro Silva aqui debo cambiar s_Instalacion_fechahora por Re
                    txtVEHICULO_refechainstalacion.Text = .Reinstalacion_fechahora.ToString("dd/MM/yyyy HH:mm")
                    '.Instalacion_fechahora.ToString(System.Globalization.DateTimeFormatInfo.InvariantInfo)
                End If

                If .s_Desinstalacion_fechahora <> String.Empty Then ' Pedro Silva chupa huevo
                    txtVEHICULO_desfechainstalacion.Text = .Desinstalacion_fechahora.ToString("dd/MM/yyyy HH:mm")
                    '.Instalacion_fechahora.ToString(System.Globalization.DateTimeFormatInfo.InvariantInfo)
                End If

                'Se agrega campo NroOrdenInstal 06122010 - Pedro Silva
                txtNroOrdenInstal.Text = .s_NroOrdenInstal
                If Not .s_Proviene_de Is Nothing Then
                    ddlVEHICULO_provienede.SelectedValue = .s_Proviene_de
                    If .s_Proviene_de = 1 Then
                        pnlNroOrden.Visible = True
                        txtNroOrdenInstal.Text = .s_NroOrdenInstal
                    End If
                End If
                'Se agrega campo NroOrdenInstal 06122010 - Pedro Silva

                chkVEHICULO_activo.Checked = IIf(.s_Activo <> String.Empty, .s_Activo, False)
                chkVEHICULO_demo.Checked = IIf(.s_Demo <> String.Empty, .s_Demo, False)
                chkvalVEHICULOactivo = IIf(.s_Activo <> String.Empty, .s_Activo, False)
                chkvalVEHICULOdemo = IIf(.s_Demo <> String.Empty, .s_Demo, False)
                txtVEHICULO_identificador_rastreo.Text = .Identificador_rastreo
                txtVEHICULO_kilometraje.Text = .s_Kilometraje
                txtVEHICULO_matricula.Text = .s_Matricula
                txtVEHICULO_motor.Text = .s_Motor
                txtVEHICULO_consumo.Text = .s_Consumo_aprox
                txtVEHICULO_anho.Text = .s_Anho
                txtVEHICULO_chasis.Text = .s_Chasis


                'cpVEHICULO_color.SelectedHexValue = .Color
                cpVEHICULO_color.Text = .s_Color


                If Not ddl_INSTALADORES.SelectedItem Is Nothing Then ddl_INSTALADORES.SelectedItem.Selected = False
                ' If Not ddl_Vendedor.SelectedItem Is Nothing Then ddl_Vendedor.SelectedItem.Selected = False
                If Not ddlVEHICULO_provienede.SelectedItem Is Nothing Then ddlVEHICULO_provienede.SelectedItem.Selected = False
                If Not ddlMarcas.SelectedItem Is Nothing Then ddlMarcas.SelectedItem.Selected = False
                If Not ddlModelos.SelectedItem Is Nothing Then ddlModelos.SelectedItem.Selected = False
                If Not ddlTipoDeVehiculo.SelectedItem Is Nothing Then ddlTipoDeVehiculo.SelectedItem.Selected = False
                If Not ddlEquipoGPS.SelectedItem Is Nothing Then ddlEquipoGPS.SelectedItem.Selected = False
                If Not ddlSucursales.SelectedItem Is Nothing Then ddlSucursales.SelectedItem.Selected = False

                If .s_Id_equipogps <> String.Empty Then ddlEquipoGPS.Items.FindByValue(.s_Id_equipogps).Selected = True
                ddlMarcas.Items.FindByValue(.s_Marca).Selected = True
                BindModelos(ddlMarcas.SelectedValue)
                ddlModelos.Items.FindByValue(.s_Modelo).Selected = True
                ddlTipoDeVehiculo.Items.FindByValue(.s_Tipo_vehiculo).Selected = True
                DisplayIcono()
                ddlVEHICULO_provienede.Items.FindByValue(.s_Proviene_de).Selected = True
                ' 19102010 Se ha agregado el campo .s_Id_vendedor, a fin de elegir en una lista desplegable los vendedores de la empresa
                If .s_Id_instalador <> String.Empty Then ddl_INSTALADORES.Items.FindByValue(.s_Id_instalador).Selected = False
                'If .s_Id_vendedor <> String.Empty Then ddl_Vendedor.Items.FindByValue(.s_Id_vendedor).Selected = True
                If .s_Sucursal_instalacion <> String.Empty Then
                    ddlSucursales.Items.FindByValue(.s_Sucursal_instalacion).Selected = True
                Else
                    ddlSucursales.SelectedIndex = ddlSucursales.Items.Count - 1
                End If

            End With
        Catch myEX As Exception
            Session.Add("msjerror", myEX.Message + "<br>" + myEX.StackTrace)
        End Try
    End Sub

    Private Function Guardar_Datos() As Boolean

        Guardar_Datos = False


        If ddlEquipoGPS.Items.Count < 1 Then
            Session.Add("msjerror", "No hay equipos GPS en existencia, ingrese uno.")
            Exit Function
        End If

        If ddlVEHICULO_provienede.Items.Count < 1 Then
            Session.Add("msjerror", "No hay procedencias/seguros en existencia, ingrese uno.")
            Exit Function
        End If

        If txtVEHICULO_identificador_rastreo.Text = String.Empty Then
            Session.Add("msjerror", "Debe ingresar un identificador o Contraseña.")
            Exit Function
        End If
        Dim tblVEHICULOS As New rastreo_vehiculo(cnn_str.CadenaDeConexion)

        Try

            With tblVEHICULOS

                If ViewState("admin_cliente_vehiculo_update") Then
                    .Where.Identificador_rastreo.Value = txtVEHICULO_identificador_rastreo.Text.Trim.ToUpperInvariant()
                    If .Query.Load() Then
                        If .RowCount > 0 Then
                            If .Idrastreo_vehiculo <> idrastreo_vehiculo Then
                                ViewState.Add("msjerror", "Ya existe un vehiculo con esa Contraseña.")
                                Throw New Exception("msjerror")
                            End If
                        End If
                    End If
                    .FlushData()
                    .LoadByPrimaryKey(idrastreo_vehiculo, id_cliente)
                    .Fech_upd = Now
                    .User_upd = mi_usuario.idUsuario
                Else
                    .Where.Identificador_rastreo.Value = txtVEHICULO_identificador_rastreo.Text.Trim.ToUpperInvariant()
                    If .Query.Load() Then
                        If .RowCount > 0 Then
                            ViewState.Add("msjerror", "Ya existe un vehiculo con esa Contraseña.")
                            Throw New Exception("msjerror")
                        End If
                    End If
                    .FlushData()

                    If ddlEquipoGPS.SelectedValue <> "DESINSTALADO" Then
                        .Where.Id_equipogps.Value = ddlEquipoGPS.SelectedValue
                        If .Query.Load() Then
                            If .RowCount > 0 Then
                                Session.Add("msjerror", "Ya existe un vehiculo con ese equipo GPS instalado - Contraseña: " + .Identificador_rastreo)
                                Throw New Exception("msjerror")
                            End If
                        End If
                        .FlushData()
                    End If
                    .AddNew()
                    .Fech_ins = Now
                    .User_ins = mi_usuario.idUsuario
                    .Id_cliente = id_cliente
                End If

                .s_Poliza_nroorden = txtVEHICULO_nropoliza.Text.Trim.ToUpperInvariant()
                .s_Nro_ficha = txtFicha.Text.Trim.ToUpperInvariant()
                .s_Proviene_de = ddlVEHICULO_provienede.SelectedItem.Value
                .s_NroOrdenInstal = Trim(txtNroOrdenInstal.Text)


                If txtVEHICULO_fechaemisionpoliza.Text <> String.Empty Then
                    Dim fechaemisionpolizaf As String = txtVEHICULO_fechaemisionpoliza.Text.Split(" ")(0)
                    Dim fechaemisionpoliza As String
                    fechaemisionpolizaf = fechaemisionpolizaf.Split("/")(2) + "-" + fechaemisionpolizaf.Split("/")(1) + "-" + fechaemisionpolizaf.Split("/")(0)
                    fechaemisionpoliza = fechaemisionpolizaf
                    .Poliza_emision = Convert.ToDateTime(fechaemisionpoliza)
                End If

                If txtVEHICULO_fechavencimientopoliza.Text <> String.Empty Then
                    Dim fechavencimientopolizaf As String = txtVEHICULO_fechavencimientopoliza.Text.Split(" ")(0)
                    Dim fechavencimientopoliza As String
                    fechavencimientopolizaf = fechavencimientopolizaf.Split("/")(2) + "-" + fechavencimientopolizaf.Split("/")(1) + "-" + fechavencimientopolizaf.Split("/")(0)
                    fechavencimientopoliza = fechavencimientopolizaf
                    .Poliza_venc = Convert.ToDateTime(fechavencimientopoliza)
                End If

                If txtVEHICULO_fechainstalacion.Text <> String.Empty Then
                    Dim instalacionf As String = txtVEHICULO_fechainstalacion.Text.Split(" ")(0)
                    Dim instalacionh As String = txtVEHICULO_fechainstalacion.Text.Split(" ")(1)
                    Dim instalacionfh As String
                    instalacionf = instalacionf.Split("/")(2) + "-" + instalacionf.Split("/")(1) + "-" + instalacionf.Split("/")(0)
                    instalacionfh = instalacionf + " " + instalacionh
                    .Instalacion_fechahora = Convert.ToDateTime(instalacionfh)
                End If

                If txtVEHICULO_refechainstalacion.Text <> String.Empty Then
                    Dim reinstalacionf As String = txtVEHICULO_refechainstalacion.Text.Split(" ")(0)
                    Dim reinstalacionh As String = txtVEHICULO_refechainstalacion.Text.Split(" ")(1)
                    Dim reinstalacionfh As String
                    reinstalacionf = reinstalacionf.Split("/")(2) + "-" + reinstalacionf.Split("/")(1) + "-" + reinstalacionf.Split("/")(0)
                    reinstalacionfh = reinstalacionf + " " + reinstalacionh
                    .Reinstalacion_fechahora = Convert.ToDateTime(reinstalacionfh)
                Else
                    .s_Reinstalacion_fechahora = String.Empty
                End If

                If txtVEHICULO_desfechainstalacion.Text <> String.Empty Then
                    Dim desinstalacionf As String = txtVEHICULO_desfechainstalacion.Text.Split(" ")(0)
                    Dim desinstalacionh As String = txtVEHICULO_desfechainstalacion.Text.Split(" ")(1)
                    Dim desinstalacionfh As String
                    desinstalacionf = desinstalacionf.Split("/")(2) + "-" + desinstalacionf.Split("/")(1) + "-" + desinstalacionf.Split("/")(0)
                    desinstalacionfh = desinstalacionf + " " + desinstalacionh
                    .Desinstalacion_fechahora = Convert.ToDateTime(desinstalacionfh)
                Else
                    .s_Desinstalacion_fechahora = String.Empty
                End If

                .Activo = chkVEHICULO_activo.Checked
                .Demo = chkVEHICULO_demo.Checked
                If chkvalVEHICULOactivo <> chkVEHICULO_activo.Checked Then
                    .Activo_fecha = Now
                End If

                .s_Identificador_rastreo = txtVEHICULO_identificador_rastreo.Text.Trim.ToUpperInvariant()
                .s_Anho = txtVEHICULO_anho.Text.Trim.ToUpper.PadRight(4, "0").Substring(0, 4)

                ' ''Por alguna extraña razón Firefox tira como value de colores en formato rgb(rojo,verde,azul)
                ' '' entonces tengo que convertir de esto a hexa, colo los 3 valores, y convierto a integer
                'If cpVEHICULO_color.SelectedHexValue.Contains("rgb") Then
                '    Dim array_colores(3) As Integer
                '    Dim i As Integer = 0
                '    Dim colores As String() = _B
                '        cpVEHICULO_color.SelectedHexValue.Replace("rgb(", String.Empty).Replace(")", String.Empty).Split(",")
                '    For Each color As String In colores
                '        array_colores(i) = CInt(color.Trim)
                '        i += 1
                '    Next
                '    ''Vuelvo a usar a "i" no como contador, sino como storage para el valor
                '    ''que voy a convertir de integer a hexa usando ToString("X") formato de int a hexa
                '    i = RGB(array_colores(0), array_colores(1), array_colores(2))
                '    .s_Color = "#" + i.ToString("X")
                'Else
                '    ''Funciona asi directamente en IE... omg! :S
                '    .s_Color = cpVEHICULO_color.SelectedHexValue
                'End If

                .s_Color = cpVEHICULO_color.Text.Trim.ToUpper

                If ddlEquipoGPS.SelectedValue = String.Empty Then
                    Session.Add("msjerror", "Debe seleccionar un equipo gps para el vehiculo.")
                    Throw New Exception("msjerror")
                Else
                    If ddlEquipoGPS.SelectedValue <> "DESINSTALADO" Then
                        .s_Id_equipogps = ddlEquipoGPS.SelectedValue
                    Else
                        .SetColumnNull(rastreo_vehiculo.ColumnNames.Id_equipogps)
                    End If
                End If
                If ddlTipoDeVehiculo.SelectedValue = String.Empty Then
                    Session.Add("msjerror", "Debe seleccionar un tipo especifico de vehiculo.")
                    Throw New Exception("msjerror")
                Else
                    .s_Tipo_vehiculo = ddlTipoDeVehiculo.SelectedValue
                End If

                If ddlMarcas.SelectedValue = String.Empty Or ddlModelos.SelectedValue = String.Empty Then
                    Session.Add("msjerror", "Debe seleccionar una marca y/o modelo.")
                    Throw New Exception("msjerror")
                Else
                    .s_Marca = ddlMarcas.SelectedValue
                    .s_Modelo = ddlModelos.SelectedValue
                End If

                .s_Chasis = txtVEHICULO_chasis.Text.Trim.ToUpperInvariant()
                .s_Motor = txtVEHICULO_motor.Text.Trim.ToUpperInvariant()
                .s_Consumo_aprox = txtVEHICULO_consumo.Text.Trim()
                .s_Matricula = txtVEHICULO_matricula.Text.Trim.ToUpperInvariant()
                Dim it As Integer
                If Integer.TryParse(txtVEHICULO_kilometraje.Text.Trim.ToUpperInvariant(), it) Then
                    .s_Kilometraje = txtVEHICULO_kilometraje.Text.Trim.ToUpperInvariant()
                Else
                    'Session.Add("msjerror", "Kilometraje inválido.")
                    'Throw New Exception("msjerror")
                End If

                .s_Chofer = txtVEHICULO_chofer.Text.ToUpperInvariant().Trim()
                .s_Chofer_contacto = txtVEHICULO_chofer_contacto.Text.ToUpperInvariant().Trim()
                .s_Observacion = txtVEHICULO_observaciones.Text.ToUpperInvariant().Trim()

                If ddl_INSTALADORES.SelectedValue = String.Empty Or ddl_INSTALADORES.SelectedValue = String.Empty Then
                    Session.Add("msjerror", "Debe seleccionar un instalador.")
                    Throw New Exception("msjerror")
                Else
                    .s_Id_instalador = ddl_INSTALADORES.SelectedValue
                End If

                If ddlSucursales.SelectedValue = String.Empty Then
                    Session.Add("msjerror", "Debe seleccionar una sucursal de instalacion.")
                    Throw New Exception("msjerror")
                Else
                    .s_Sucursal_instalacion = ddlSucursales.SelectedValue
                End If

                '29102010 Pedro Silva . If que controla la seleccion obligaria de un vendedor
                If ddl_Vendedor.SelectedValue = String.Empty Or ddl_Vendedor.SelectedValue = String.Empty Then
                    Session.Add("msjerror", "Debe seleccionar un vendedor.")
                    Throw New Exception("msjerror")
                Else
                    .s_Id_vendedor = ddl_Vendedor.SelectedValue
                End If

                .Save()

            End With

            If Not ViewState("admin_cliente_vehiculo_update") Then
                Session.Add("msjerror", "Vehiculo registrado exitosamente!")
            Else
                Session.Add("msjerror", "Vehiculo actualizado exitosamente!")
            End If

            Guardar_Datos = True

        Catch NpgsqlEx As NpgsqlException
            If NpgsqlEx.Code = "23503" Then
                Session.Add("msjerror", "No puede cargar un vehiculo a un empleado, o bien, no existe la persona")
            End If
        Catch myEX As Exception
            If Not myEX.Message.Contains("msjerror") Then
                Session.Add("msjerror", myEX.Message + "<br>" + myEX.StackTrace)
            End If
        End Try
    End Function

    Protected Sub btnGUARDAR_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGUARDAR.Click
        If Guardar_Datos() Then
            'If ViewState("GO_BACK_URL") <> String.Empty Then
            If id_cliente > 0 Then
                Response.Redirect("admin_cliente_vehiculo_lista.aspx?id=" + id_cliente.ToString, False)
            Else
                Response.Redirect("admin_cliente_vehiculo_lista.aspx", False)
            End If
            'Else
            '   JavascriptHelper.Custom(Page, "history.back();", "goback")
            'End If
        End If
    End Sub

    Protected Sub btnCANCELAR_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCANCELAR.Click
        Try
            If id_cliente > 0 Then
                Response.Redirect("admin_cliente_vehiculo_lista.aspx?id=" + id_cliente.ToString, False)
            Else
                Response.Redirect("admin_cliente_vehiculo_lista.aspx", False)
            End If
        Catch myEX As Exception
            Session.Add("msjerror", myEX.Message + "<br>" + myEX.StackTrace)
        End Try
    End Sub


    Protected Sub lnkmarca_nueva_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkmarca_nueva.Click
        panel_Marcas_newedit.Visible = True
        panel_Marca_select.Visible = False
        panel_Modelo_select.Visible = False
        txtMARCA_Add.Text = String.Empty
        txtMARCA_Add.Focus()
    End Sub

    Protected Sub lnkmarca_edit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkmarca_edit.Click
        If ddlMarcas.Items.Count > 0 Then
            panel_Marcas_newedit.Visible = True
            panel_Marca_select.Visible = False
            panel_Modelo_select.Visible = False
            ViewState.Add("EditMARCA", True)
            txtMARCA_Add.Text = ddlMarcas.SelectedItem.Text.Trim
            txtMARCA_Add.Focus()
        End If
    End Sub

    Protected Sub lnkmodelo_nuevo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkmodelo_nuevo.Click
        panel_Modelo_newedit.Visible = True
        panel_Modelo_select.Visible = False
        panel_Marca_select.Visible = False
        txtMODELO_Add.Text = String.Empty
        txtMODELO_Add.Focus()
    End Sub

    Protected Sub lnkmodelo_edit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkmodelo_edit.Click
        If ddlModelos.Items.Count > 0 Then
            panel_Modelo_newedit.Visible = True
            panel_Modelo_select.Visible = False
            panel_Marca_select.Visible = False
            ViewState.Add("EditMODELO", True)
            txtMODELO_Add.Text = ddlModelos.SelectedItem.Text.Trim
            txtMODELO_Add.Focus()
        End If
    End Sub

    Protected Sub imgbtn_marcasave_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbtn_marcasave.Click
        Dim tblMARCA As New rastreo_marca_vehiculo(cnn_str.CadenaDeConexion)
        With tblMARCA
            If ViewState("EditMARCA") Then
                .Where.Idrastreo_marca_vehiculo.Value = ddlMarcas.SelectedValue
                If Not .Query.Load() Then
                    Session.Add("msjerror", "NOSE QUE PASO MEN - MARCAS")
                    Exit Sub
                End If
                .Fech_upd = Now
                .User_upd = mi_usuario.idUsuario
                ViewState.Remove("EditMARCA")
            Else
                .Where.Marca.Value = txtMARCA_Add.Text.Trim.ToUpperInvariant()
                If .Query.Load() Then
                    If .RowCount > 0 Then
                        Session.Add("msjerror", "Esta marca ya existe.")
                        Exit Sub
                    End If
                End If
                .AddNew()
                .Fech_ins = Now
                .User_ins = mi_usuario.idUsuario
            End If
            .s_Marca = txtMARCA_Add.Text.Trim.ToUpperInvariant()
            .Save()
            Session.Add("msjerror", "Marca registrada.")
            BindMarcas()
            panel_Marca_select.Visible = True
            panel_Marcas_newedit.Visible = False
            panel_Modelo_select.Visible = True
        End With
    End Sub

    Protected Sub imgbtn_marcacancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbtn_marcacancel.Click
        panel_Marca_select.Visible = True
        panel_Marcas_newedit.Visible = False
        panel_Modelo_select.Visible = True
        txtMARCA_Add.Text = String.Empty
    End Sub

    Protected Sub imgbtn_modelosave_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbtn_modelosave.Click
        Dim tblMODELO As New rastreo_modelo_vehiculo(cnn_str.CadenaDeConexion)
        With tblMODELO
            If ViewState("EditMODELO") Then
                .Where.Id_marca.Value = ddlMarcas.SelectedValue
                .Where.Idrastreo_modelo_vehiculo.Value = ddlModelos.SelectedValue
                If Not .Query.Load() Then
                    Session.Add("msjerror", "NO SE QUE PASO MEN - MODELOS")
                    Exit Sub
                End If
                .Fech_upd = Now
                .User_upd = mi_usuario.idUsuario
                ViewState.Remove("EditMODELO")
            Else
                .Where.Modelo.Value = txtMODELO_Add.Text.Trim.ToUpperInvariant()
                .Where.Id_marca.Value = ddlMarcas.SelectedValue
                If .Query.Load() Then
                    If .RowCount > 0 Then
                        Session.Add("msjerror", "Este modelo ya existe.")
                        Exit Sub
                    End If
                End If
                .AddNew()
                .Id_marca = ddlMarcas.SelectedValue
                .Fech_ins = Now
                .User_ins = mi_usuario.idUsuario
            End If
            .s_Modelo = txtMODELO_Add.Text.Trim.ToUpperInvariant()
            .Save()
            If ViewState("EditMODELO") Then
                Session.Add("msjerror", "Modelo modificado exitosamente.")
            Else
                Session.Add("msjerror", "Modelo registrado.")
            End If
            BindModelos(ddlMarcas.SelectedValue)
            panel_Marca_select.Visible = True
            panel_Modelo_newedit.Visible = False
            panel_Modelo_select.Visible = True
        End With

    End Sub

    Protected Sub imgbtn_modelocancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbtn_modelocancel.Click
        panel_Marca_select.Visible = True
        panel_Modelo_newedit.Visible = False
        panel_Modelo_select.Visible = True
        txtMODELO_Add.Text = String.Empty
    End Sub

    Protected Sub lnk_edit_tipovehiculo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnk_edit_tipovehiculo.Click
        If Not ddlTipoDeVehiculo.SelectedItem Is Nothing Then
            panel_select_tipodevehiculo.Visible = False
            panel_newedit_tipodevehiculo.Visible = True
            ViewState.Add("EditTIPOVEHICULO", True)
            txtTIPOVEHICULO_Add.Text = ddlTipoDeVehiculo.SelectedItem.Text.Trim.ToUpperInvariant()
            txtTIPOVEHICULO_Add.Focus()
        End If
    End Sub

    Protected Sub lnk_new_tipovehiculo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnk_new_tipovehiculo.Click
        panel_select_tipodevehiculo.Visible = False
        panel_newedit_tipodevehiculo.Visible = True
        txtTIPOVEHICULO_Add.Focus()
    End Sub

    Protected Sub imgbtn_tipovehiculo_cancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbtn_tipovehiculo_cancel.Click
        panel_newedit_tipodevehiculo.Visible = False
        panel_select_tipodevehiculo.Visible = True
        txtTIPOVEHICULO_Add.Text = String.Empty
        ddlTipoDeVehiculo.Focus()
    End Sub

    Protected Sub imgbtn_tipovehiculo_save_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbtn_tipovehiculo_save.Click
        Dim tblTIPOVEHICULO As New rastreo_tipo_vehiculo(cnn_str.CadenaDeConexion)
        With tblTIPOVEHICULO
            If ViewState("EditTIPOVEHICULO") Then
                .Where.Idrastreo_tipo_vehiculo.Value = ddlTipoDeVehiculo.SelectedValue
                If Not .Query.Load() Then
                    Session.Add("msjerror", "NO SE QUE PASO MEN - TIPOVEHICULO")
                    Exit Sub
                End If
                .Fech_upd = Now
                .User_upd = mi_usuario.idUsuario
            Else
                .Where.Tipo_de_vehiculo.Value = txtTIPOVEHICULO_Add.Text.Trim.ToUpperInvariant()
                If .Query.Load() Then
                    If .RowCount > 0 Then
                        Session.Add("msjerror", "Ese tipo de vehiculo ya existe.")
                        Exit Sub
                    End If
                End If
                .AddNew()
                .Fech_ins = Now
                .User_ins = mi_usuario.idUsuario
            End If
            .s_Icono = ddlIconosTV.SelectedValue
            .s_Tipo_de_vehiculo = txtTIPOVEHICULO_Add.Text.Trim.ToUpperInvariant()
            .Save()
            If ViewState("EditTIPOVEHICULO") Then
                ViewState.Remove("EditTIPOVEHICULO")
                Session.Add("msjerror", "Modificado el tipo de vehiculo exitosamente.")
            Else
                Session.Add("msjerror", "Ingresado nuevo tipo de vehiculo.")
            End If
            BindTipoDeVehiculo()
            panel_newedit_tipodevehiculo.Visible = False
            panel_select_tipodevehiculo.Visible = True
            txtTIPOVEHICULO_Add.Text = String.Empty
        End With
    End Sub

    Protected Sub ddlIconosTV_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlIconosTV.SelectedIndexChanged
        If ddlIconosTV.SelectedValue <> String.Empty Then
            imgtempTV.ImageUrl = ddlIconosTV.SelectedValue
            imgtempTV.DataBind()
        End If
    End Sub

    Function Convertir_Path_ImageUrl(ByVal physicalPath As String) As String
        Convertir_Path_ImageUrl = "~/" + physicalPath.Replace(Server.MapPath("~"), String.Empty).Replace("\", "/")
    End Function

    Protected Sub ddlTipoDeVehiculo_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlTipoDeVehiculo.DataBound
        DisplayIcono()
    End Sub

    Protected Sub ddlTipoDeVehiculo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlTipoDeVehiculo.SelectedIndexChanged
        DisplayIcono()
    End Sub
    Private Sub DisplayIcono()
        Dim tblTV As New rastreo_tipo_vehiculo(cnn_str.CadenaDeConexion)
        tblTV.LoadByPrimaryKey(ddlTipoDeVehiculo.SelectedValue)
        Icono_de_TipoVehiculo.ImageUrl = tblTV.Icono
    End Sub

    Protected Sub btnEventosEquipo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEventosEquipo.Click
        pmlFRAMEEquipo.Visible = Not pmlFRAMEEquipo.Visible
        If pmlFRAMEEquipo.Visible = True Then
            btnEventosEquipo.Text = "Cerrar edición de eventos del equipo"
        Else
            btnEventosEquipo.Text = "Eventos del equipo GPS"
        End If
        BindEquipoGPS()
    End Sub

    Protected Sub btnFRAMEequipoGPS_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFRAMEequipoGPS.Click
        pnlIFRAME.Visible = Not pnlIFRAME.Visible
        If pnlIFRAME.Visible = True Then
            btnFRAMEequipoGPS.Text = "===> Cerrar y actualizar lista de Equipos GPS <==="
        Else
            btnFRAMEequipoGPS.Text = "Administrar Equipos GPS"
        End If
        BindEquipoGPS()
    End Sub

    Protected Sub imgRefreshListaEquiposGPS_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgRefreshListaEquiposGPS.Click
        BindEquipoGPS()
    End Sub

    Protected Sub lnkproviene_edit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkproviene_edit.Click
        If Not ddlVEHICULO_provienede.SelectedItem Is Nothing Then
            pnlProvieneSelect.Visible = False
            pnlProvieneNew.Visible = True
            ViewState.Add("EditPROVIENEDE", True)
            txtPROVIENEDE.Text = ddlVEHICULO_provienede.SelectedItem.Text.Trim
            txtPROVIENEDE.Focus()
        End If
    End Sub

    Protected Sub lnkproviene_new_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkproviene_new.Click
        pnlProvieneSelect.Visible = False
        pnlProvieneNew.Visible = True
        txtPROVIENEDE.Focus()
    End Sub

    Protected Sub imgbtn_provieneback_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbtn_provieneback.Click
        pnlProvieneNew.Visible = False
        pnlProvieneSelect.Visible = True
        txtPROVIENEDE.Text = String.Empty
        ddlVEHICULO_provienede.Focus()
    End Sub

    Protected Sub imgbtn_provienesave_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbtn_provienesave.Click
        Dim tblPROVIENEDE As New rastreo_proviene_de_seguro(cnn_str.CadenaDeConexion)
        With tblPROVIENEDE
            If ViewState("EditPROVIENEDE") Then
                .Where.Idrastreo_proviene_de_seguro.Value = ddlVEHICULO_provienede.SelectedValue
                If Not .Query.Load() Then
                    Session.Add("msjerror", "NO SE QUE PASO MEN - PROVIENE DE")
                    Exit Sub
                End If
                .Fech_upd = Now
                .User_upd = mi_usuario.idUsuario
            Else
                .Where.Descripcion_del_seguro.Value = txtPROVIENEDE.Text.Trim.ToUpperInvariant()
                If .Query.Load() Then
                    If .RowCount > 0 Then
                        Session.Add("msjerror", "Este seguro/procedencia ya existe.")
                        Exit Sub
                    End If
                End If
                .AddNew()
                .Fech_ins = Now
                .User_ins = mi_usuario.idUsuario
            End If
            .s_Descripcion_del_seguro = txtPROVIENEDE.Text
            .Save()
            If ViewState("EditTIPOVEHICULO") Then
                Session.Add("msjerror", "Ha modificado una descripcion de procedencia/seguro exitosamente!.")
            Else
                Session.Add("msjerror", "Ha ingresado una descripcion de procedencia/seguro exitosamente!")
            End If
            bindProvieneDe()
            pnlProvieneNew.Visible = False
            pnlProvieneSelect.Visible = True
            txtPROVIENEDE.Text = String.Empty
        End With
    End Sub

    Private Sub bindProvieneDe()
        Utilidades.fillDDL(ddlVEHICULO_provienede, New rastreo_proviene_de_seguro(cnn_str.CadenaDeConexion), _
                           rastreo_proviene_de_seguro.ColumnNames.Descripcion_del_seguro, _
                           rastreo_proviene_de_seguro.ColumnNames.Idrastreo_proviene_de_seguro)
    End Sub

    Private Sub BindEquipoGPS()
        ddlEquipoGPS.Items.Clear()
        Utilidades.fillDDL(ddlEquipoGPS, New rs_select_equipogps(cnn_str.CadenaDeConexion), _
                           rs_select_equipogps.ColumnNames.Ddlequipogps_desc, _
                           rs_select_equipogps.ColumnNames.Idrastreogps_equipogps)
        ddlEquipoGPS.Items.Add("DESINSTALADO")
    End Sub
    Protected Sub ddlEquipoGPS_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlEquipoGPS.DataBinding
        CheckEquipoGPS()
    End Sub

    Protected Sub ddlEquipoGPS_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlEquipoGPS.DataBound
        CheckEquipoGPS()
    End Sub

    Protected Sub ddlEquipoGPS_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlEquipoGPS.SelectedIndexChanged
        CheckEquipoGPS()
    End Sub

    Private Sub CheckEquipoGPS()
        Dim tblEquiposGPS As New rastreogps_equipogps(cnn_str.CadenaDeConexion)
        If Not ddlEquipoGPS.SelectedItem Is Nothing And ddlEquipoGPS.SelectedValue <> "DESINSTALADO" Then
            tblEquiposGPS.LoadByPrimaryKey(ddlEquipoGPS.SelectedValue)
            Dim tblVehiculos As New rastreo_vehiculo(cnn_str.CadenaDeConexion)
            tblVehiculos.Where.Id_equipogps.Value = tblEquiposGPS.Idrastreogps_equipogps
            If tblVehiculos.Query.Load() Then
                If tblVehiculos.Identificador_rastreo <> String.Empty Then
                    lbEquipoUsado.Text = "Equipo GPS utilizado por : " + tblVehiculos.Identificador_rastreo
                    Return
                End If
            Else
                lbEquipoUsado.Text = String.Empty
            End If
        Else
            lbEquipoUsado.Text = String.Empty
        End If
    End Sub


    Protected Sub txtVEHICULO_matricula_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtVEHICULO_matricula.TextChanged
        If MATRICULA_FindDuplicate(txtVEHICULO_matricula.Text.Trim().ToUpperInvariant()) Then
            'que hacer si hay
        Else
            'que hacer si no hay
        End If
    End Sub

    Function MATRICULA_FindDuplicate(ByVal matricula As String) As Boolean
        Try
            Dim tblVehiculos As New rastreo_vehiculo(cnn_str.CadenaDeConexion)
            With tblVehiculos
                .Where.Matricula.Value = matricula
                If .Query.Load() Then
                    If .RowCount > 0 Then
                        Session.Add("msjerror", "La matricula ya existe")
                    End If
                End If
            End With
        Catch ex As Exception
            Return MATRICULA_FindDuplicate
        End Try
    End Function

    Protected Sub ddlVEHICULO_provienede_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlVEHICULO_provienede.SelectedIndexChanged
        If 1 = ddlVEHICULO_provienede.SelectedValue Then
            pnlNroOrden.Visible = True
        Else
            pnlNroOrden.Visible = False
        End If
    End Sub
End Class
