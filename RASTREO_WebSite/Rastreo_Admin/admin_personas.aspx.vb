Imports RASTREOmw
Imports MyGeneration.dOOdads
Partial Class RASTREO_admin_personas
    Inherits System.Web.UI.Page

    Public mi_usuario As New Usuario()
    Dim Id_persona As Integer = 0
    Dim chkvalCLIENTE As Boolean
    Dim chkvalEMPLEADO As Boolean
    Public personaExiste As Boolean = False

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
            permisoAdd = Utilidades.FindPermissionFor(mi_usuario.idUsuario, "PERSONAS_ADMIN-A")
            permisoMod = Utilidades.FindPermissionFor(mi_usuario.idUsuario, "PERSONAS_ADMIN-M")
            permisoQry = Utilidades.FindPermissionFor(mi_usuario.idUsuario, "PERSONAS_ADMIN-C")
            If Not permisoAdd Or Not permisoMod Or Not permisoQry Then
                Response.Redirect("admin_personas_lista.aspx", False)
            End If
        Catch myEX As Exception
            Session.Add("msjerror", myEX.Message + "<br>" + myEX.StackTrace)
        End Try
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Request.QueryString("id") Is Nothing Then
                Id_persona = Request.QueryString("id")
            End If
            If Not IsPostBack Then
                If Not Request.UrlReferrer Is Nothing And ViewState("GO_BACK_URL") Is Nothing Then
                    ViewState.Add("GO_BACK_URL", Request.UrlReferrer.ToString)
                End If
                Bind_Paises()
                If Request.QueryString.Count < 1 Then
                    Dim Itemcito As New ListItem
                    Itemcito = ddlPais.Items.FindByText("PARAGUAY")
                    If Not Itemcito Is Nothing Then
                        ddlPais.SelectedIndex = -1
                        ddlPais.SelectedValue = Itemcito.Value
                        ddlPais.DataBind()
                        Itemcito = ddlDistrito.Items.FindByText("CENTRAL")
                        If Not Itemcito Is Nothing Then
                            ddlDistrito.SelectedIndex = -1
                            ddlDistrito.SelectedValue = Itemcito.Value
                            ddlDistrito.DataBind()
                            Itemcito = ddlCiudad.Items.FindByText("ASUNCION")
                            If Not Itemcito Is Nothing Then
                                ddlCiudad.SelectedIndex = -1
                                ddlCiudad.SelectedValue = Itemcito.Value
                                ddlCiudad.DataBind()
                            End If
                        End If
                    End If
                End If
                Bind_Tipodocumento()
                If Id_persona > 0 Then
                    If CargarDatos() Then
                        ViewState.Add("personaExiste", True)
                        ViewState.Add("admin_personas_update", True)
                        btnGUARDAR.Text = "Actualizar"
                        btnCANCELAR.Text = "Volver"
                    End If
                End If
            Else
                personaExiste = CType(ViewState("personaExiste"), Boolean)
                If Not ddlTipoDocumento.SelectedItem Is Nothing Then
                    ddlTipoDocumento.SelectedItem.Selected = False
                End If
                If ddlTipoPersona.SelectedValue = "F" Then
                    Dim ddlITEM As ListItem
                    ddlITEM = ddlTipoDocumento.Items.FindByText("CEDULA DE IDENTIDAD PARAGUAYA")
                    If Not ddlITEM Is Nothing Then
                        ddlITEM.Selected = True
                    End If
                Else
                    Dim ddlITEM As ListItem
                    ddlITEM = ddlTipoDocumento.Items.FindByText("RUC")
                    If Not ddlITEM Is Nothing Then
                        ddlITEM.Selected = True
                    End If
                End If

                'If txtPERSONA_Apellido.Text = String.Empty Then
                ' txtPERSONA_Apellido.Focus()
                'End If

                'If txtPERSONA_Nombre.Text = String.Empty Then
                ' txtPERSONA_Nombre.Focus()
                'End If

                'If txtPERSONA_NroDocumento.Text = String.Empty Then
                ' txtPERSONA_NroDocumento.Focus()
                'End If
            End If
        Catch myEX As Exception
            Session.Add("msjerror", myEX.ToString())
        End Try
    End Sub

    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete

        Dim mensaje As String = String.Empty
        mensaje = ViewState("msjerror")
        If mensaje = String.Empty Then
            mensaje = Session("msjerror")
        End If
        If mensaje <> String.Empty Then
            If Not mensaje.Contains("duplicado de ") And Not IsPostBack Then
                Session.Remove("msjerror")
            End If
            errMsgs.Text = mensaje
            errMsgs.Visible = True
            ViewState.Remove("msjerror")
        Else
            errMsgs.Visible = False
        End If

        If Not IsPostBack Then txtPERSONA_NroDocumento.Focus()

        If Not ddlTipoPersona.SelectedItem Is Nothing Then
            If ddlTipoPersona.SelectedItem.Text.ToLower.Trim.Contains("jurídica") Or _
                    ddlTipoPersona.SelectedItem.Text.ToLower.Trim.Contains("juridica") Then
                panelPERSONAJURIDICA.Visible = True
                panelPERSONAFISICA.Visible = False
            ElseIf ddlTipoPersona.SelectedItem.Text.ToLower.Trim.Contains("fisica") Or _
            ddlTipoPersona.SelectedItem.Text.ToLower.Trim.Contains("física") Then
                panelPERSONAJURIDICA.Visible = False
                panelPERSONAFISICA.Visible = True
            Else
                panelPERSONAJURIDICA.Visible = False
                panelPERSONAFISICA.Visible = False
            End If
        End If

        If Not ddl_Cliente_o_Empleado.SelectedItem Is Nothing Then
            Dim tblP As New rastreo_persona(cnn_str.CadenaDeConexion)
            Dim tblC As New rastreo_cliente(cnn_str.CadenaDeConexion)
            Dim tblE As New rastreo_empleados(cnn_str.CadenaDeConexion)
            With tblP
                If .LoadByPrimaryKey(Id_persona) Then
                    If .s_Id_seguro <> String.Empty Then
                        chkSeguro.Checked=True 
                        panelSEGURO.Visible = True
                    End If
                    If chkSeguro.Checked Then
                        Utilidades.fillDDL(ddlRepresentaEsteSeguro, _
                                           New rastreo_proviene_de_seguro(cnn_str.CadenaDeConexion), _
                                           rastreo_proviene_de_seguro.ColumnNames.Descripcion_del_seguro, _
                                           rastreo_proviene_de_seguro.ColumnNames.Idrastreo_proviene_de_seguro)
                    End If
                    If panelSEGURO.Visible Then
                        If ddlRepresentaEsteSeguro.Items.Count > 0 Then
                            ddlRepresentaEsteSeguro.SelectedItem.Selected = False
                            If .s_Id_seguro <> String.Empty Then
                                ddlRepresentaEsteSeguro.Items.FindByValue(.s_Id_seguro).Selected = True
                            End If
                        End If
                    End If
                    If .s_Idrastreo_persona <> String.Empty Then
                        If tblC.LoadByPrimaryKey(.Idrastreo_persona) Then
                            If tblC.s_Id_cliente <> String.Empty Then
                                'ddl_Cliente_o_Empleado.SelectedValue = 1
                                chkCLIENTE_acceso.Checked = IIf(tblC.s_Acceso_sistema <> String.Empty, tblC.s_Acceso_sistema, False)
                                chkCLIENTE_activo.Checked = IIf(tblC.s_Estado_cliente <> String.Empty, tblC.s_Estado_cliente, False)
                                chkvalCLIENTE = chkCLIENTE_activo.Checked
                                txtCLIENTE_ClavePersonal.Text = tblC.s_Clave_personal.Trim
                            End If
                        End If

                        If tblE.LoadByPrimaryKey(.Idrastreo_persona) Then
                            If tblE.s_Id_empleado <> String.Empty Then
                                'ddl_Cliente_o_Empleado.SelectedValue = 2
                                '19102010 - Pedro Silva. se ha agregado un chek Vendedor para los casos de que el empleado sea vendedor
                                chkEMPLEADO_instalador.Checked = IIf(tblE.s_Instalador <> String.Empty, tblE.s_Instalador, False)
                                'chkEMPLEADO_Vendedor.Checked = IIf(tblE.s_Vendedor <> String.Empty, tblE.s_Vendedor, False)
                                chkEMPLEADO_acceso.Checked = IIf(tblE.s_Acceso_sistema <> String.Empty, tblE.s_Acceso_sistema, False)
                                chkEMPLEADO_activo.Checked = IIf(tblE.s_Estado_empleado <> String.Empty, tblE.s_Estado_empleado, False)
                                chkvalEMPLEADO = chkEMPLEADO_activo.Checked
                            End If
                        End If
                    End If
                End If
            End With
            If ddl_Cliente_o_Empleado.SelectedValue = 1 Then
                panelCLIENTE.Visible = True
                panelEMPLEADO.Visible = False
            ElseIf ddl_Cliente_o_Empleado.SelectedValue = 2 Then
                panelCLIENTE.Visible = False
                panelEMPLEADO.Visible = True
            End If
        End If

    End Sub

    Private Function CargarDatos() As Boolean
        Try
            Dim tblP As New rastreo_persona(cnn_str.CadenaDeConexion)

            With tblP
                If .LoadByPrimaryKey(Id_persona) Then
                    If .s_Idrastreo_persona <> String.Empty Then

                        Dim tblC As New rastreo_cliente(cnn_str.CadenaDeConexion)
                        Dim tblE As New rastreo_empleados(cnn_str.CadenaDeConexion)
                        ddl_Cliente_o_Empleado.Items.Clear()
                        Dim itmAdd As New ListItem()
                        If tblC.LoadByPrimaryKey(Id_persona) Then
                            If tblC.s_Id_cliente <> String.Empty Then
                                itmAdd = New ListItem("Cliente", "1")
                            End If
                        End If
                        If tblE.LoadByPrimaryKey(Id_persona) Then
                            If tblE.s_Id_empleado <> String.Empty Then
                                itmAdd = New ListItem("Empleado", "2")
                            End If
                        End If
                        ddl_Cliente_o_Empleado.Items.Add(itmAdd)
                        If Not .s_Tipo_persona Is Nothing Then
                            ddlTipoPersona.SelectedValue = .s_Tipo_persona
                            If .s_Tipo_persona = "F" Then
                                txtPERSONA_Nombre.Text = .s_Nombre
                                txtPERSONA_Apellido.Text = .s_Apellido
                                If .Razon_social <> String.Empty Then .Razon_social = String.Empty
                            ElseIf .s_Tipo_persona = "J" Then
                                txtPERSONA_RazonSocial.Text = .s_Razon_social
                                If .Nombre <> String.Empty Or .Apellido <> String.Empty Then
                                    .Nombre = String.Empty
                                    .Apellido = String.Empty
                                End If
                            End If
                        End If

                        If Not .s_Tipo_documento Is Nothing Then
                            ddlTipoDocumento.SelectedValue = .s_Tipo_documento
                        End If

                        txtPERSONA_Direccion.Text = .s_Direccion
                        txtPERSONA_email.Text = .s_Email
                        txtPERSONA_NroDocumento.Text = .s_Nro_documento
                        txtPERSONA_tel_movil.Text = .s_Tel_movil
                        txtPERSONA_tel_ofi.Text = .s_Tel_ofi
                        txtPERSONA_tel_part.Text = .s_Tel_part


                        ddlPais.SelectedValue = .s_Id_pais
                        Bind_Distritos(.Id_pais)
                        Bind_Ciudad(.Id_pais, .Id_distrito)
                        ddlDistrito.SelectedValue = .s_Id_distrito
                        ddlCiudad.SelectedValue = .s_Id_ciudad
                        ddlPais_SelectedIndexChanged(Me, New System.EventArgs)

                        'Dim tblUSER As New rastreo_usuarios(cnn_str.CadenaDeConexion)
                        'With tblUSER
                        '    .Where.Id_persona.Value = Id_persona
                        '    If .Query.Load() Then
                        '        If .Usuario <> String.Empty Then
                        '            btnEdit_UsuarioSistema.ToolTip = "Edita el usuario del sistema de la persona."
                        '            btnEdit_UsuarioSistema.Text = "Editar usuario"
                        '            btnSave_Usuario.ToolTip = "Actualiza la contraseña del usuario, el campo de nombre de usuario tambien. mucho cuidado con esto ok?"
                        '            btnSave_Usuario.Text = "Actualizar usuario"
                        '            txtUSUARIO_usuario.Text = .s_Usuario
                        '            txtUSUARIO_password.Text = .s_Pass
                        '        Else
                        '            btnEdit_UsuarioSistema.Text = "Crear usuario web"
                        '        End If
                        '    End If
                        'End With
                        personaExiste = True
                    Else
                        'Session.Add("msjerror", "El usuario con id " + Id_persona.ToString + " no existe.")
                        personaExiste = False
                    End If
                End If
            End With
            CargarDatos = personaExiste
        Catch myEX As Exception
            CargarDatos = personaExiste
            Session.Add("msjerror", myEX.Message + "<br>" + myEX.StackTrace)
        End Try

    End Function

    Private Function NroDocumento_Repetido(Optional ByVal redirect As Boolean = True) As Boolean
        Dim tblobj As New rastreo_persona(cnn_str.CadenaDeConexion)
        Try
            lbREPE.Text = String.Empty

            If txtPERSONA_NroDocumento.Text.Trim.ToUpper <> String.Empty Then
                With tblobj
                    .Where.Tipo_documento.Value = ddlTipoDocumento.SelectedValue
                    '.Where.Nro_documento.Operator = WhereParameter.Operand.ILike
                    .Where.Nro_documento.Value = txtPERSONA_NroDocumento.Text.Trim.ToUpperInvariant()
                    .Query.Load()
                    If .RowCount > 0 Then
                        If .Idrastreo_persona = Id_persona Then
                            lbREPE.Text = "El documento pertenece a esta misma persona."
                            Return False
                        End If
                        Dim Nombre As String = String.Empty
                        If .Tipo_persona = "F" Then
                            Nombre = .Nombre + " " + .Apellido
                        Else
                            Nombre = .Razon_social
                        End If
                        If redirect Then
                            Session.Add("msjerror", "El documento (" + txtPERSONA_NroDocumento.Text.Trim.ToUpperInvariant() + ") tipo " + ddlTipoDocumento.SelectedItem.Text.Trim + " <br/> se encuentra duplicado, el cliente registrado con el mismo es: " & Nombre)
                            Response.Redirect("admin_personas.aspx?id=" & .Idrastreo_persona, False)
                        Else
                            lbREPE.Text = "El documento (" + txtPERSONA_NroDocumento.Text.Trim.ToUpperInvariant() + ") tipo " + ddlTipoDocumento.SelectedItem.Text.Trim + " <br/> se encuentra duplicado, el cliente registrado con el mismo es: " & Nombre
                        End If
                        Return True
                    Else
                        lbREPE.Text = "El documento del tipo documento " & ddlTipoDocumento.SelectedItem.Text & " no pertenece a ningun otro cliente/empleado."
                    End If
                End With
                Return False
            Else
                If txtPERSONA_NroDocumento.Text.Trim.ToUpperInvariant() <> String.Empty Then
                    Return False
                Else
                    Return True
                End If
            End If
            Return False
        Catch ex As Exception
            Session.Add("msjerror", ex.Message + "<br/><br/>" + ex.StackTrace)
            Return False
        Finally
            tblobj = Nothing
            'GC.Collect()
        End Try
    End Function

    Private Sub Guardar_Datos()
        If Not NroDocumento_Repetido() Then
            Dim tblPERSONA As New rastreo_persona(cnn_str.CadenaDeConexion)
            Dim Transaccion As TransactionMgr = TransactionMgr.ThreadTransactionMgr
            Try
                Transaccion.BeginTransaction()

                With tblPERSONA
                    If (ViewState("admin_personas_update")) Then
                        .LoadByPrimaryKey(Id_persona)
                        .s_Fech_upd = Now
                        .s_User_upd = mi_usuario.idUsuario
                    Else
                        .AddNew()
                        .s_Fech_ins = Now
                        .s_User_ins = mi_usuario.idUsuario
                    End If
                    .s_Nro_documento = txtPERSONA_NroDocumento.Text.Trim.ToUpperInvariant()
                    .s_Id_ciudad = ddlCiudad.SelectedValue
                    .s_Id_distrito = ddlDistrito.SelectedValue
                    .s_Id_pais = ddlPais.SelectedValue
                    .s_Tipo_documento = ddlTipoDocumento.SelectedValue
                    .s_Tipo_persona = ddlTipoPersona.SelectedValue
                    If panelPERSONAJURIDICA.Visible Then
                        .s_Razon_social = txtPERSONA_RazonSocial.Text.Trim.ToUpperInvariant()
                    ElseIf panelPERSONAFISICA.Visible Then
                        .s_Nombre = txtPERSONA_Nombre.Text.Trim.ToUpperInvariant()
                        .s_Apellido = txtPERSONA_Apellido.Text.Trim.ToUpperInvariant()
                    End If
                    .s_Direccion = txtPERSONA_Direccion.Text.Trim.ToUpperInvariant()
                    .s_Email = txtPERSONA_email.Text.Trim '.ToUpperInvariant()
                    .s_Tel_movil = txtPERSONA_tel_movil.Text
                    .s_Tel_ofi = txtPERSONA_tel_ofi.Text
                    .s_Tel_part = txtPERSONA_tel_part.Text
                    If panelSEGURO.Visible Then
                        .Id_seguro = ddlRepresentaEsteSeguro.SelectedValue
                    End If
                    .Save()
                    Id_persona = .Idrastreo_persona
                End With

                If panelCLIENTE.Visible Then
                    Dim tblCLIENTE As New rastreo_cliente(cnn_str.CadenaDeConexion)
                    With tblCLIENTE
                        If (ViewState("admin_personas_update")) Then
                            .LoadByPrimaryKey(Id_persona)
                            If .s_Id_cliente <> String.Empty Then
                                .s_Fech_upd = Now
                                .s_User_upd = mi_usuario.idUsuario
                            Else
                                .AddNew()
                                .s_Fech_ins = Now
                                .s_User_ins = mi_usuario.idUsuario
                                .s_Id_cliente = tblPERSONA.s_Idrastreo_persona
                            End If
                        Else
                            .AddNew()
                            .s_Fech_ins = Now
                            .s_User_ins = mi_usuario.idUsuario
                            .s_Id_cliente = tblPERSONA.s_Idrastreo_persona
                        End If
                        .Acceso_sistema = chkCLIENTE_acceso.Checked
                        .Estado_cliente = chkCLIENTE_activo.Checked
                        If chkCLIENTE_activo.Checked <> chkvalCLIENTE Then
                            .Estado_fecha = Now
                        End If
                        .s_Clave_personal = txtCLIENTE_ClavePersonal.Text.Trim.ToUpperInvariant()
                        .Save()
                    End With
                ElseIf panelEMPLEADO.Visible Then
                    Dim tblEMPLEADO As New rastreo_empleados(cnn_str.CadenaDeConexion)
                    With tblEMPLEADO
                        If (ViewState("admin_personas_update")) Then
                            .LoadByPrimaryKey(Id_persona)
                            If .s_Id_empleado <> String.Empty Then
                                .s_Fech_upd = Now
                                .s_User_upd = mi_usuario.idUsuario
                            Else
                                .AddNew()
                                .s_Fech_ins = Now
                                .s_User_ins = mi_usuario.idUsuario
                                .s_Id_empleado = tblPERSONA.s_Idrastreo_persona
                            End If
                        Else
                            .AddNew()
                            .s_Fech_ins = Now
                            .s_User_ins = mi_usuario.idUsuario
                            .s_Id_empleado = tblPERSONA.s_Idrastreo_persona
                        End If
                        .Instalador = chkEMPLEADO_instalador.Checked
                        .Acceso_sistema = chkEMPLEADO_acceso.Checked
                        .Estado_empleado = chkEMPLEADO_activo.Checked
                        ' 19102010. Pedro Silva. Se asigna el valor True o False a Vendedor segun este tildado o no
                        '.Vendedor = chkEMPLEADO_Vendedor.Checked

                        If chkEMPLEADO_activo.Checked <> chkvalEMPLEADO Then
                            .Estado_fecha = Now
                        End If
                        .Save()

                    End With
                End If
                Transaccion.CommitTransaction()
                If (ViewState("admin_personas_update")) Then
                    Session.Add("msjerror", "Persona actualizada exitosamente!")
                Else
                    Session.Add("msjerror", "Persona registrada exitosamente!")
                End If

                Response.Redirect("admin_personas.aspx?id=" + Id_persona.ToString(), False)
            Catch ex As Exception
                Transaccion.RollbackTransaction()
                If ex.Message.Contains("23505") Then
                    Session.Add("msjerror", "La persona que esta intentando ingresar tiene un numero (" + txtPERSONA_NroDocumento.Text.Trim + ") repetido de " + ddlTipoDocumento.SelectedItem.Text.Trim + " que ya esta ingresado en el sistema.")
                Else
                    Session.Add("msjerror", ex.Message + "<br/><br/>" + ex.StackTrace)
                End If
            End Try
        End If
    End Sub

    Protected Sub btnSAVE_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGUARDAR.Click
        btnCANCELAR.Enabled = False
        Guardar_Datos()
    End Sub

#Region "Definiciones del panel de Paises, Distritos y Ciudades"
#Region "Binders para los combos de Paises, Distritos y Ciudades"
    Protected Sub Bind_Paises()
        Try
            Utilidades.fillDDL(ddlPais, New rastreo_pais(cnn_str.CadenaDeConexion), _
                                rastreo_pais.ColumnNames.Pais, rastreo_pais.ColumnNames.Idrastreo_pais)
            If (ddlPais.Items.Count) Then ddlPais.SelectedIndex = 0
        Catch myEX As Exception
            Session.Add("msjerror", myEX.Message + "<br>" + myEX.StackTrace)
        End Try
    End Sub

    Protected Sub Bind_Distritos(ByVal idPais As Integer)
        Try
            Dim tblDistrito As New rastreo_distrito(cnn_str.CadenaDeConexion)
            With tblDistrito
                .Where.Id_pais.Value = idPais
                .Query.Load()
            End With
            With ddlDistrito
                .DataSource = tblDistrito.DefaultView
                .DataTextField = rastreo_distrito.ColumnNames.Distrito
                .DataValueField = rastreo_distrito.ColumnNames.Idrastreo_distrito
                .DataBind()
            End With
        Catch myEX As Exception
            Session.Add("msjerror", myEX.Message + "<br>" + myEX.StackTrace)
        End Try
    End Sub

    Protected Sub Bind_Ciudad(ByVal idPais As Integer, ByVal idDistrito As Integer)
        Try
            Dim tblciudad As New rastreo_ciudad(cnn_str.CadenaDeConexion)
            With tblciudad
                .Where.Id_pais.Value = idPais
                .Where.Id_distrito.Value = idDistrito
                .Query.Load()
            End With
            With ddlCiudad
                .DataSource = tblciudad.DefaultView
                .DataTextField = rastreo_ciudad.ColumnNames.Ciudad
                .DataValueField = rastreo_ciudad.ColumnNames.Idrastreo_ciudad
                .DataBind()
            End With
        Catch myEX As Exception
            Session.Add("msjerror", myEX.Message + "<br>" + myEX.StackTrace)
        End Try
    End Sub
#End Region

#Region "Eventos Databound y SelectedItemChanged de los combos de Paises, Distritos y Ciudades"
    Protected Sub ddlPais_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPais.DataBound
        Try
            If ddlPais.SelectedIndex >= 0 Then
                Bind_Distritos(ddlPais.SelectedValue)
                lblPais.Text = ddlPais.SelectedItem.Text
            Else
                lblPais.Text = String.Empty
            End If
        Catch MyEx As Exception
            Session.Add("msjerror", MyEx.ToString())
        End Try
    End Sub

    Protected Sub ddlPais_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPais.SelectedIndexChanged
        Try
            If ddlPais.SelectedIndex >= 0 Then
                Bind_Distritos(ddlPais.SelectedValue)
                lblPais.Text = ddlPais.SelectedItem.Text
            Else
                lblPais.Text = String.Empty
            End If
        Catch MyEx As Exception
            Session.Add("msjerror", MyEx.ToString())
        End Try

    End Sub

    Protected Sub ddlDistrito_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlDistrito.DataBound
        Try
            If ddlDistrito.SelectedIndex >= 0 Then
                Bind_Ciudad(ddlPais.SelectedValue, ddlDistrito.SelectedValue)
                lblDistrito.Text = ddlDistrito.SelectedItem.Text
            Else
                Bind_Ciudad(ddlPais.SelectedValue, 0)
                lblDistrito.Text = String.Empty
            End If
        Catch MyEx As Exception
            Session.Add("msjerror", MyEx.ToString())
        End Try
    End Sub

    Protected Sub ddlDistrito_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlDistrito.SelectedIndexChanged
        Try
            If ddlDistrito.SelectedIndex >= 0 Then
                Bind_Ciudad(ddlPais.SelectedValue, ddlDistrito.SelectedValue)
                lblDistrito.Text = ddlDistrito.SelectedItem.Text
            Else
                Bind_Ciudad(ddlPais.SelectedValue, 0)
                lblDistrito.Text = String.Empty
            End If
        Catch MyEx As Exception
            Session.Add("msjerror", MyEx.ToString())
        End Try
    End Sub

    Protected Sub ddlCiudad_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCiudad.DataBound
        Try
            If ddlCiudad.SelectedIndex >= 0 Then
                lblCiudad.Text = ddlCiudad.SelectedItem.Text
            Else
                lblCiudad.Text = String.Empty
            End If
        Catch MyEx As Exception
            Session.Add("msjerror", MyEx.ToString())
        End Try
    End Sub

    Protected Sub ddlCiudad_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCiudad.SelectedIndexChanged
        Try
            If ddlCiudad.SelectedIndex >= 0 Then
                lblCiudad.Text = ddlCiudad.SelectedItem.Text
            Else
                lblCiudad.Text = String.Empty
            End If
        Catch MyEx As Exception
            Session.Add("msjerror", MyEx.ToString())
        End Try
    End Sub
#End Region

#Region "Rutinas de guardado y actualizacion de Paises, Distritos y Ciudades"
    Private Sub ubicacion_link_new_setup(ByVal pais As Boolean, ByVal distrito As Boolean, ByVal ciudad As Boolean)
        panelSelect_pais.Visible = pais
        panelSelect_distrito.Visible = distrito
        panelSelect_ciudad.Visible = ciudad
    End Sub

    Protected Sub link_new_pais_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles link_new_pais.Click
        panelInsert_pais.Visible = True
        ubicacion_link_new_setup(False, False, False)
        txtAdd_pais.Text = String.Empty
        txtAdd_pais.Focus()
    End Sub

    Protected Sub link_edit_pais_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles link_edit_pais.Click
        If (ddlPais.SelectedIndex >= 0) Then
            panelInsert_pais.Visible = True
            ubicacion_link_new_setup(False, False, False)
            ViewState.Add("UpdPais", True)
            txtAdd_pais.Text = ddlPais.SelectedItem.Text
            txtAdd_pais.Focus()
        End If
    End Sub

    Protected Sub link_new_distrito_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles link_new_distrito.Click
        If ddlPais.SelectedValue <> String.Empty Then
            panelInsert_Distrito.Visible = True
            panelSelect_distrito.Visible = False
            ubicacion_link_new_setup(False, False, False)
            txtAdd_distrito.Text = String.Empty
            txtAdd_distrito.Focus()
        Else
            Session.Add("msjerror", "Debe crear un pais para poder crear un distrito.")
        End If
    End Sub

    Protected Sub link_edit_distrito_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles link_edit_distrito.Click
        If (ddlDistrito.SelectedIndex >= 0) Then
            panelInsert_Distrito.Visible = True
            panelSelect_distrito.Visible = False
            ubicacion_link_new_setup(False, False, False)
            ViewState.Add("UpdDistrito", True)
            txtAdd_distrito.Text = ddlDistrito.SelectedItem.Text
            txtAdd_distrito.Focus()
        End If
    End Sub

    Protected Sub link_new_ciudad_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles link_new_ciudad.Click
        If ddlPais.SelectedValue <> String.Empty And ddlDistrito.SelectedValue <> String.Empty Then
            panelInsert_ciudad.Visible = True
            panelSelect_ciudad.Visible = False
            ubicacion_link_new_setup(False, False, False)
            txtAdd_ciudad.Text = String.Empty
            txtAdd_ciudad.Focus()
        Else
            Session.Add("msjerror", "Debe crear un distrito para crear una ciudad.")
        End If
    End Sub

    Protected Sub link_edit_ciudad_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles link_edit_ciudad.Click
        If (ddlPais.SelectedIndex >= 0) And (ddlDistrito.SelectedIndex >= 0) Then
            panelInsert_ciudad.Visible = True
            panelSelect_ciudad.Visible = False
            ubicacion_link_new_setup(False, False, False)
            ViewState.Add("UpdCiudad", True)
            txtAdd_ciudad.Text = ddlCiudad.SelectedItem.Text
            txtAdd_ciudad.Focus()
        End If
    End Sub

    Protected Sub link_save_pais_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles link_save_pais.Click
        Try
            Dim tblPais As New rastreo_pais(cnn_str.CadenaDeConexion)
            With tblPais
                Dim insString As String = txtAdd_pais.Text.Trim.ToUpper
                txtAdd_pais.Text = String.Empty
                If ViewState("UpdPais") Then
                    .Where.Idrastreo_pais.Value = ddlPais.SelectedValue
                    .Query.Load()
                    If .RowCount = 1 Then
                        .Pais = insString
                        .Fech_upd = Now
                        .User_upd = mi_usuario.idUsuario
                        .Save()
                    End If
                    ViewState.Remove("UpdPais")
                Else
                    .Where.Pais.Value = insString
                    .Query.Load()
                    If .RowCount < 1 Then
                        .AddNew()
                        .Pais = insString
                        .Fech_ins = DateTime.Now
                        .User_ins = mi_usuario.idUsuario
                        .Save()
                    Else
                        Session.Add("msjerror", "El pais ya existe.")
                    End If
                End If
                Bind_Paises()
            End With
        Catch myEX As Exception
            Session.Add("msjerror", myEX.Message + "<br>" + myEX.StackTrace)
        Finally
            panelInsert_pais.Visible = False
            panelSelect_pais.Visible = True
            ubicacion_link_new_setup(True, True, True)
            'GC.Collect()
        End Try
    End Sub

    Protected Sub link_cancel_pais_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles link_cancel_pais.Click
        panelInsert_pais.Visible = False
        panelSelect_pais.Visible = True
        ubicacion_link_new_setup(True, True, True)
        ViewState.Remove("UpdPais")
    End Sub

    Protected Sub link_save_distrito_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles link_save_distrito.Click
        If ddlPais.SelectedValue <> String.Empty Then
            Try
                Dim tblDistrito As New rastreo_distrito(cnn_str.CadenaDeConexion)
                With tblDistrito
                    Dim insString As String = txtAdd_distrito.Text.Trim.ToUpper
                    txtAdd_distrito.Text = String.Empty
                    .Where.Id_pais.Value = CInt(ddlPais.SelectedValue)
                    If ViewState("UpdDistrito") Then
                        .Where.Idrastreo_distrito.Value = ddlDistrito.SelectedValue
                        .Query.Load()
                        If .RowCount = 1 Then
                            .Distrito = insString
                            .Fech_upd = Now
                            .User_upd = mi_usuario.idUsuario
                            .Save()
                        End If
                        ViewState.Remove("UpdDistrito")
                    Else
                        .Where.Distrito.Value = insString
                        .Query.Load()
                        If .RowCount < 1 Then
                            .AddNew()
                            .Id_pais = CInt(ddlPais.SelectedValue)
                            .Distrito = insString
                            .Fech_ins = Now
                            .User_ins = mi_usuario.idUsuario
                            .Save()
                        Else
                            Session.Add("msjerror", "el distrito ya existe.")
                        End If
                    End If
                    Bind_Distritos(ddlPais.SelectedValue)
                    Bind_Ciudad(ddlPais.SelectedValue, .Idrastreo_distrito)
                End With
            Catch myEX As Exception
                Session.Add("msjerror", myEX.Message + "<br>" + myEX.StackTrace)
            Finally
                panelInsert_Distrito.Visible = False
                panelSelect_distrito.Visible = True
                ubicacion_link_new_setup(True, True, True)
                'GC.Collect()
            End Try
        Else
            ''Mensaje de error: debe estar seleccionado un pais 
            Session.Add("msjerror", "Seleccione pais.")
        End If
    End Sub

    Protected Sub link_cancel_distrito_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles link_cancel_distrito.Click
        panelInsert_Distrito.Visible = False
        panelSelect_distrito.Visible = True
        ubicacion_link_new_setup(True, True, True)
        ViewState.Remove("UpdDistrito")
    End Sub

    Protected Sub link_save_ciudad_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles link_save_ciudad.Click
        If ddlPais.SelectedValue <> String.Empty And ddlDistrito.SelectedValue <> String.Empty Then
            Try
                Dim tblCiudad As New rastreo_ciudad(cnn_str.CadenaDeConexion)
                With tblCiudad
                    Dim insString As String = txtAdd_ciudad.Text.Trim.ToUpper
                    txtAdd_ciudad.Text = String.Empty
                    .Where.Id_pais.Value = CInt(ddlPais.SelectedValue)
                    .Where.Id_distrito.Value = CInt(ddlDistrito.SelectedValue)
                    If ViewState("UpdCiudad") Then
                        .Where.Idrastreo_ciudad.Value = ddlCiudad.SelectedValue
                        .Query.Load()
                        If .RowCount = 1 Then
                            .Ciudad = insString
                            .Fech_upd = Now
                            .User_upd = mi_usuario.idUsuario
                            .Save()
                        End If
                        ViewState.Remove("UpdCiudad")
                    Else
                        .Where.Ciudad.Value = insString
                        .Query.Load()
                        If .RowCount < 1 Then
                            .AddNew()
                            .Id_pais = CInt(ddlPais.SelectedValue)
                            .Id_distrito = CInt(ddlDistrito.SelectedValue)
                            .Ciudad = insString
                            .Fech_ins = DateTime.Now
                            .User_ins = mi_usuario.idUsuario
                            .Save()
                        Else
                            Session.Add("msjerror", "la ciudad ya existe.")
                        End If
                    End If
                    Bind_Ciudad(ddlPais.SelectedValue, ddlDistrito.SelectedValue)
                End With
            Catch myEX As Exception
                Session.Add("msjerror", myEX.Message + "<br>" + myEX.StackTrace)
            Finally
                panelInsert_ciudad.Visible = False
                panelSelect_ciudad.Visible = True
                ubicacion_link_new_setup(True, True, True)
                'GC.Collect()
            End Try
        Else
            Session.Add("msjerror", "Seleccione pais y distrito.")
        End If
    End Sub

    Protected Sub link_cancel_ciudad_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles link_cancel_ciudad.Click
        panelInsert_ciudad.Visible = False
        panelSelect_ciudad.Visible = True
        ubicacion_link_new_setup(True, True, True)
        ViewState.Remove("UpdCiudad")
    End Sub
#End Region
#End Region

#Region "Definiciones del panel de tipo de documento"
    Protected Sub Bind_Tipodocumento()
        Utilidades.fillDDL(ddlTipoDocumento, New rastreo_tipo_documento(cnn_str.CadenaDeConexion), _
                           rastreo_tipo_documento.ColumnNames.Descripcion, _
                           rastreo_tipo_documento.ColumnNames.Idrastreo_tipo_documento)
    End Sub

    Protected Sub link_new_tipodocumento_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles link_new_tipodocumento.Click
        panelInsert_TipoDocumento.Visible = True
        panelSelect_TipoDocumento.Visible = False
        txtAdd_tipodocumento.Text = String.Empty
        txtAdd_tipodocumento.Focus()
    End Sub

    Protected Sub link_edit_tipodocumento_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles link_edit_tipodocumento.Click
        panelInsert_TipoDocumento.Visible = True
        panelSelect_TipoDocumento.Visible = False
        ViewState.Add("UpdTipodocumento", True)
        txtAdd_tipodocumento.Text = ddlTipoDocumento.SelectedItem.Text
        txtAdd_tipodocumento.Focus()
    End Sub

    Protected Sub link_cancel_tipodocumento_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles link_cancel_tipodocumento.Click
        panelInsert_TipoDocumento.Visible = False
        panelSelect_TipoDocumento.Visible = True
        ViewState.Remove("UpdTipodocumento")
    End Sub

    Protected Sub link_save_tipodocumento_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles link_save_tipodocumento.Click
        Try
            Dim tblTipodocumento As New rastreo_tipo_documento(cnn_str.CadenaDeConexion)
            With tblTipodocumento
                Dim insString As String = txtAdd_tipodocumento.Text.Trim.ToUpperInvariant()
                txtAdd_tipodocumento.Text = String.Empty
                If ViewState("UpdTipodocumento") Then
                    .Where.Idrastreo_tipo_documento.Value = ddlTipoDocumento.SelectedValue
                    If .Query.Load() Then
                        .Descripcion = insString
                        .Fech_upd = Now
                        .User_upd = mi_usuario.idUsuario
                        .Save()
                    End If
                    ViewState.Remove("UpdTipodocumento")
                Else
                    .Where.Descripcion.Value = insString
                    .Query.Load()
                    If .RowCount < 1 Then
                        .AddNew()
                        .Descripcion = insString
                        .Fech_ins = Now
                        .User_ins = mi_usuario.idUsuario
                        .Save()
                    Else
                        Session.Add("msjerror", "tipo de documento existente.")
                    End If
                End If
                Bind_Tipodocumento()
            End With
        Catch ex As Exception
            Session.Add("msjerror", ex.Message + "<br/>" + ex.StackTrace)
        Finally
            panelInsert_TipoDocumento.Visible = False
            panelSelect_TipoDocumento.Visible = True
        End Try
    End Sub
#End Region

    Protected Sub btnCANCELAR_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCANCELAR.Click
        Try
            Response.Redirect("admin_personas_lista.aspx", False)
        Catch myEX As Exception
            Session.Add("msjerror", myEX.Message + "<br>" + myEX.StackTrace)
        End Try
    End Sub


    Protected Sub txtPERSONA_NroDocumento_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPERSONA_NroDocumento.TextChanged
        If NroDocumento_Repetido(False) Then
            txtPERSONA_NroDocumento.Focus()
        Else
            txtPERSONA_Nombre.Focus()
        End If
    End Sub

    Protected Sub btnNUEVO_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNUEVO.Click
        Try
            Response.Redirect("admin_personas.aspx", False)
        Catch myEX As Exception
            Session.Add("msjerror", myEX.Message + "<br>" + myEX.StackTrace)
        End Try
    End Sub

    Protected Sub chkSeguro_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkSeguro.CheckedChanged
        If chkSeguro.Checked Then
            panelSEGURO.Visible = True
        Else
            panelSEGURO.Visible = False
        End If
    End Sub

    
 End Class
