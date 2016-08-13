Imports RASTREOmw
Imports MyGeneration.dOOdads
Imports System.Collections.Generic
Imports System.IO

Partial Class RASTREO_Administracion_admin_personas_lista
    Inherits System.Web.UI.Page

    Dim idCLIENTE As Integer = 0
    Public mi_usuario As New Usuario
    Private permisoAdd As Boolean = False
    Private permisoMod As Boolean = False
    Private permisoDel As Boolean = False
    Private permisoQry As Boolean = False

    Protected Overloads Overrides Sub Render(ByVal writer As HtmlTextWriter)
        Utilidades.CompresionASPX(HttpContext.Current)
        MyBase.Render(writer)
    End Sub

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Not Session("session_UsuarioRASTREO") Is Nothing Then
            mi_usuario = CType(Session("session_UsuarioRASTREO"), Usuario)
        Else
            Response.Redirect("../Login.aspx?rp=" + Me.Request.Url.AbsoluteUri(), False)
        End If

        permisoAdd = Utilidades.FindPermissionFor(mi_usuario.idUsuario, "PERSONAS_ADMIN-A")
        permisoDel = Utilidades.FindPermissionFor(mi_usuario.idUsuario, "PERSONAS_ADMIN-B")
        permisoMod = Utilidades.FindPermissionFor(mi_usuario.idUsuario, "PERSONAS_ADMIN-M")
        permisoQry = Utilidades.FindPermissionFor(mi_usuario.idUsuario, "PERSONAS_ADMIN-C")
        'Boton de "NUEVO"
        btnAddPersona.Visible = permisoAdd
        'Esto solo funciona si la grilla siempre tiene "Editar" en penultimo lugar.
        gv_Lista_Personas.Columns(gv_Lista_Personas.Columns.Count - 2).Visible = permisoMod
        'Esto solo funciona si la grilla siempre tiene "Eliminar" al ultimo.
        gv_Lista_Personas.Columns(gv_Lista_Personas.Columns.Count - 1).Visible = False
        'gv_Lista_Personas.Columns(gv_Lista_Personas.Columns.Count - 1).Visible = permisoDel
        'Aqui seteamos a NADA el datasource si no tiene permisos para consultar...
        If Not permisoQry Then gv_Lista_Personas.Visible = Nothing

        gv_Lista_Personas.DataBind()

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            BindDDLBusqueda()
            Dim ps As Integer
            If Not Integer.TryParse(txtPageSize.Text, ps) Then
                ps = 20
            End If
            gv_Lista_Personas.PageSize = ps
            If chkFiltro.Checked Then
                chkTipoPersona_FISICA.Checked = False
                chkTipoPersona_JURIDICA.Checked = False
                sqlds_Lista_Personas.SelectCommand = "SELECT * FROM rsview_datos_personas WHERE idrastreo_persona NOT IN (SELECT id_cliente FROM rastreo_cliente WHERE id_cliente NOT IN (SELECT id_empleado FROM rastreo_empleados))"
                sqlds_Lista_Personas.DataBind()
                gv_Lista_Personas.DataSource = Nothing
                gv_Lista_Personas.DataSourceID = Nothing
                gv_Lista_Personas.DataBind()
                gv_Lista_Personas.DataSourceID = sqlds_Lista_Personas.ID
                gv_Lista_Personas.DataBind()
            ElseIf chkTipoPersona_FISICA.Checked Then
                chkFiltro.Checked = False
                chkTipoPersona_JURIDICA.Checked = False
                sqlds_Lista_Personas.SelectCommand = "SELECT * FROM rsview_datos_personas WHERE tipo_persona ILIKE '%F%SICA%' AND idrastreo_persona IN (SELECT id_cliente FROM rastreo_cliente WHERE id_cliente NOT IN (SELECT id_empleado FROM rastreo_empleados))"
                sqlds_Lista_Personas.DataBind()
                gv_Lista_Personas.DataSource = Nothing
                gv_Lista_Personas.DataSourceID = Nothing
                gv_Lista_Personas.DataBind()
                gv_Lista_Personas.DataSourceID = sqlds_Lista_Personas.ID
                gv_Lista_Personas.DataBind()
            ElseIf chkTipoPersona_JURIDICA.Checked Then
                chkTipoPersona_FISICA.Checked = False
                chkFiltro.Checked = False
                sqlds_Lista_Personas.SelectCommand = "SELECT * FROM rsview_datos_personas WHERE tipo_persona ILIKE '%JURIDICA%' AND idrastreo_persona IN (SELECT id_cliente FROM rastreo_cliente WHERE id_cliente NOT IN (SELECT id_empleado FROM rastreo_empleados))"
                sqlds_Lista_Personas.DataBind()
                gv_Lista_Personas.DataSource = Nothing
                gv_Lista_Personas.DataSourceID = Nothing
                gv_Lista_Personas.DataBind()
                gv_Lista_Personas.DataSourceID = sqlds_Lista_Personas.ID
                gv_Lista_Personas.DataBind()
            Else
                Dim filtrado As Boolean = False
                If Not Session("Filtrado") Is Nothing Then
                    filtrado = CType(Session("Filtrado"), Boolean)
                End If
                If Not filtrado Then
                    sqlds_Lista_Personas.SelectCommand = "SELECT * FROM rsview_datos_personas WHERE idrastreo_persona NOT IN (SELECT id_empleado FROM rastreo_empleados WHERE id_empleado NOT IN (SELECT id_cliente FROM rastreo_cliente))"
                    sqlds_Lista_Personas.DataBind()
                    gv_Lista_Personas.DataSource = Nothing
                    gv_Lista_Personas.DataSourceID = Nothing
                    gv_Lista_Personas.DataBind()
                    gv_Lista_Personas.DataSourceID = sqlds_Lista_Personas.ID
                    gv_Lista_Personas.DataBind()
                Else
                    BindearGrilla_Personas()
                End If

            End If
            Try
                If ExportaExcel.Value = True Then
                    ExportaExcel.Value = False
                    ExportExcel()
                    'Response.Redirect(Request.Url.ToString())
                End If
            Catch

            End Try
            'If Not IsPostBack Then
            'End If
        Catch MYEX As Exception
            Session.Add("msjerror", MYEX.Message + "<br>" + MYEX.StackTrace)
        End Try
    End Sub

    Private Function BindearGrilla_Personas(Optional ByVal TodosLosRegistros As Boolean = False) As Boolean
        Dim tblPersonasSearch As New rsview_datos_personas(cnn_str.CadenaDeConexion)
        Try
            With tblPersonasSearch
                If TodosLosRegistros Then
                    gv_Lista_Personas.DataSourceID = sqlds_Lista_Personas.ID
                    If Session("Filtrado") Is Nothing Then
                        Session.Add("Filtrado", False)
                    Else
                        Session("Filtrado") = False
                    End If
                    Return True
                Else
                    If Session("Filtrado") Is Nothing Then
                        Session.Add("Filtrado", True)
                    Else
                        Session("Filtrado") = True
                    End If
                    Dim Columna As String = ColVal.Value
                    Dim Valor As String = Me.txtBuscar.Text
                    ''Buscando a la persona por identificador de rastreo, tabla VEHICULOS
                    If Columna = rastreo_vehiculo.ColumnNames.Identificador_rastreo Or _
                       Columna = rastreo_vehiculo.ColumnNames.Matricula Then
                        Dim tblVehiculosSearch As New rastreo_vehiculo(cnn_str.CadenaDeConexion)
                        If Valor <> String.Empty And Columna <> String.Empty Then
                            If Columna <> String.Empty Then ' Se ha seleccionado alguna columna por la cual buscar?
                                If Columna Like rastreo_vehiculo.ColumnNames.Identificador_rastreo Then
                                    tblVehiculosSearch.Where.Identificador_rastreo.Operator = _
                                                                    WhereParameter.Operand.ILike
                                    tblVehiculosSearch.Where.Identificador_rastreo.Value = "%" + Valor + "%"
                                ElseIf Columna Like rastreo_vehiculo.ColumnNames.Matricula Then
                                    tblVehiculosSearch.Where.Matricula.Operator = _
                                                                    WhereParameter.Operand.ILike
                                    tblVehiculosSearch.Where.Matricula.Value = "%" + Valor + "%"
                                End If
                                If tblVehiculosSearch.Query.Load() Then
                                    If tblVehiculosSearch.RowCount > 0 Then
                                        If tblVehiculosSearch.s_Identificador_rastreo <> String.Empty Then
                                            Dim vehiculos_personas As String = String.Empty
                                            While Not tblVehiculosSearch.EOF
                                                vehiculos_personas += tblVehiculosSearch.Id_cliente.ToString() + ","
                                                tblVehiculosSearch.MoveNext()
                                            End While
                                            vehiculos_personas = vehiculos_personas.Substring(0, vehiculos_personas.Length - 1)
                                            tblPersonasSearch.Where.Idrastreo_persona.Operator = _
                                                                            WhereParameter.Operand.In
                                            tblPersonasSearch.Where.Idrastreo_persona.Value = _
                                                                            vehiculos_personas
                                            If tblPersonasSearch.Query.Load() Then
                                                gv_Lista_Personas.DataSourceID = String.Empty
                                                gv_Lista_Personas.PageSize = txtPageSize.Text
                                                gv_Lista_Personas.DataSource = .DefaultView
                                                Session.Add("msjerror", .DefaultView.Count.ToString() + " registro(s) encontrado(s).")
                                                gv_Lista_Personas.DataBind()
                                                Return True
                                            End If
                                        End If
                                    End If
                                End If
                            End If
                            gv_Lista_Personas.DataBind()
                            gv_Lista_Personas.PageSize = txtPageSize.Text
                            Session.Add("msjerror", "La busqueda no produjo ningun resultado.")
                            Return False
                        End If
                    End If
                    If Columna = rastreo_proviene_de_seguro.ColumnNames.Idrastreo_proviene_de_seguro Then
                        Dim tblProviene As New rastreo_proviene_de_seguro(cnn_str.CadenaDeConexion)
                        If Valor <> String.Empty And Columna <> String.Empty Then
                            If Columna <> String.Empty Then ' Se ha seleccionado alguna columna por la cual buscar?
                                tblProviene.Where.Descripcion_del_seguro.Operator = WhereParameter.Operand.ILike
                                tblProviene.Where.Descripcion_del_seguro.Value = "%" + Valor + "%"
                                Dim emisores As String = String.Empty
                                Dim vehiculos_por_aseguradora As String = String.Empty
                                If tblProviene.Query.Load() Then
                                    If tblProviene.RowCount > 0 Then
                                        While Not tblProviene.EOF
                                            If tblProviene.s_Idrastreo_proviene_de_seguro <> String.Empty Then
                                                emisores += tblProviene.Descripcion_del_seguro & " - "
                                                Dim idseguro As Integer = tblProviene.Idrastreo_proviene_de_seguro
                                                Dim tblVehiculoPorAseguradora As New rastreo_vehiculo(cnn_str.CadenaDeConexion)
                                                tblVehiculoPorAseguradora.Where.Proviene_de.Value = idseguro
                                                If tblVehiculoPorAseguradora.Query.Load() Then

                                                    While Not tblVehiculoPorAseguradora.EOF
                                                        vehiculos_por_aseguradora += tblVehiculoPorAseguradora.Id_cliente.ToString() + ","
                                                        tblVehiculoPorAseguradora.MoveNext()
                                                    End While
                                                    vehiculos_por_aseguradora = vehiculos_por_aseguradora.Substring(0, vehiculos_por_aseguradora.Length - 1)
                                                    tblPersonasSearch.Where.Idrastreo_persona.Operator = _
                                                                                    WhereParameter.Operand.In
                                                    tblPersonasSearch.Where.Idrastreo_persona.Value = _
                                                                                    vehiculos_por_aseguradora
                                                End If
                                            End If
                                            tblProviene.MoveNext()
                                        End While
                                    End If
                                    If tblPersonasSearch.Query.Load() Then
                                        gv_Lista_Personas.DataSourceID = String.Empty
                                        gv_Lista_Personas.DataSource = .DefaultView
                                        gv_Lista_Personas.PageSize = txtPageSize.Text
                                        Session.Add("msjerror", "Emisor: " & emisores & " " & .DefaultView.Count.ToString() + " registro(s) encontrado(s).")
                                        gv_Lista_Personas.DataBind()
                                        Return True
                                    End If
                                Else
                                    Session.Add("msjerror", "No existe aseguradora similar.")
                                End If
                            End If
                        End If
                        gv_Lista_Personas.DataBind()
                        gv_Lista_Personas.PageSize = txtPageSize.Text
                        Session.Add("msjerror", "La busqueda no produjo ningun resultado.")
                        Return False
                    ElseIf Columna = rastreo_cliente_contactos.ColumnNames.Nombre_apellido_razonsocial Then
                        Dim tblContactosSearch As New rastreo_cliente_contactos(cnn_str.CadenaDeConexion)
                        If Valor <> String.Empty And Columna <> String.Empty Then
                            If Columna <> String.Empty Then ' Se ha seleccionado alguna columna por la cual buscar?
                                Dim MyWhere As New WhereParameter(Columna, _
                                    New Npgsql.NpgsqlParameter("@" & Columna, Valor))
                                MyWhere.Value = "%" & Valor & "%"
                                tblContactosSearch.Where.Titular_secundario.Value = True
                                MyWhere.Operator = WhereParameter.Operand.ILike
                                tblContactosSearch.Query.AddWhereParameter(MyWhere)
                                tblContactosSearch.Query.Load()
                                If tblContactosSearch.RowCount > 0 Then
                                    If tblContactosSearch.s_Rastreo_cliente_id_cliente <> String.Empty Then
                                        .Where.Idrastreo_persona.Value = tblContactosSearch.Rastreo_cliente_id_cliente
                                        .Query.Load()
                                        gv_Lista_Personas.DataSourceID = String.Empty
                                        gv_Lista_Personas.DataSource = .DefaultView
                                        gv_Lista_Personas.PageSize = txtPageSize.Text
                                        If .RowCount < 1 Then
                                            gv_Lista_Personas.DataBind()
                                            Session.Add("msjerror", "La busqueda no produjo ningun resultado.")
                                            Return False
                                        End If
                                        Session.Add("msjerror", .DefaultView.Count.ToString() + " registro(s) encontrado(s).")
                                        gv_Lista_Personas.DataBind()
                                    End If
                                End If
                            End If
                            Return True
                        End If
                    End If
                    If Valor <> String.Empty And Columna <> String.Empty Then
                        If Columna <> String.Empty Then ' Se ha seleccionado alguna columna por la cual buscar?
                            Dim MyWhere As New WhereParameter(Columna, _
                                New Npgsql.NpgsqlParameter("@" & Columna, Valor))
                            MyWhere.Value = "%" & Valor & "%"
                            MyWhere.Operator = WhereParameter.Operand.ILike

                            .Query.AddWhereParameter(MyWhere)
                            .Query.Load()
                            gv_Lista_Personas.DataSourceID = String.Empty
                            gv_Lista_Personas.DataSource = .DefaultView
                            If .DefaultView.Count > 0 Then
                                gv_Lista_Personas.PageSize = txtPageSize.Text
                            End If
                            gv_Lista_Personas.PageSize = txtPageSize.Text
                        End If
                    End If
                End If

                If .RowCount < 1 Then
                    gv_Lista_Personas.DataBind()
                    Session.Add("msjerror", "La busqueda no produjo ningun resultado.")
                    If Session("Filtrado") Is Nothing Then
                        Session.Add("Filtrado", False)
                    Else
                        Session("Filtrado") = False
                    End If
                    Return False
                End If
                Session.Add("msjerror", .DefaultView.Count.ToString() + " registro(s) encontrado(s).")
                gv_Lista_Personas.DataBind()
                Return True
            End With

        Catch MYEX As Exception
            Session.Add("msjerror", MYEX.Message + "<br>" + MYEX.StackTrace)
            Return False
        End Try
    End Function

    Private Sub BindDDLBusqueda()
        Try
            ddlBuscar.Items.Clear()
            With ddlBuscar
                .Items.Add(New ListItem("Nombre y Apellido o Razon Social", rsview_datos_personas.ColumnNames.Cliente))
                .Items.Add(New ListItem("Nro. Documento", rsview_datos_personas.ColumnNames.Nro_documento))
                .Items.Add(New ListItem("Telefono particular", rsview_datos_personas.ColumnNames.Tel_part))
                .Items.Add(New ListItem("Telefono oficina", rsview_datos_personas.ColumnNames.Tel_ofi))
                .Items.Add(New ListItem("Telefono movil", rsview_datos_personas.ColumnNames.Tel_movil))
                .Items.Add(New ListItem("e-mails", rsview_datos_personas.ColumnNames.Email))
                .Items.Add(New ListItem("Titulares secundarios por nombre y apellido o razon social", rastreo_cliente_contactos.ColumnNames.Nombre_apellido_razonsocial))
                .Items.Add(New ListItem("Identificador de rastreo", rastreo_vehiculo.ColumnNames.Identificador_rastreo))
                .Items.Add(New ListItem("Matricula del vehiculo", rastreo_vehiculo.ColumnNames.Matricula))
                .Items.Add(New ListItem("Por emisor", rastreo_proviene_de_seguro.ColumnNames.Idrastreo_proviene_de_seguro))
            End With
        Catch MYEX As Exception
            Session.Add("msjerror", MYEX.Message + "<br>" + MYEX.StackTrace)
        End Try
    End Sub

    Protected Sub btnBuscar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Session("Filtrado") = False
        BindearGrilla_Personas()
    End Sub

    Protected Sub btnLimpiarBusqueda_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLimpiarBusqueda.Click
        BindearGrilla_Personas(True)
    End Sub

    Protected Sub btnAddPersona_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddPersona.Click
        Try
            Response.Redirect("admin_personas.aspx", False)
        Catch X As Exception
            Response.Write(X.Message)
        End Try

    End Sub

    Protected Sub gv_Lista_Personas_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_Lista_Personas.RowCommand
        'If e.CommandName = "SysUser" Then
        '    Dim _usuario As String = String.Empty
        '    Dim _password As String = String.Empty
        '    Dim idPersona As Integer = 0
        '    Dim cliente_row As GridViewRow = CType(sender, GridView).Rows(e.CommandArgument)
        '    _usuario = DirectCast(cliente_row.Cells(5).Controls(1).Controls(1), TextBox).Text.Trim
        '    _password = DirectCast(cliente_row.Cells(5).Controls(1).Controls(3), TextBox).Text.Trim
        '    idPersona = cliente_row.Cells(0).Text
        '    If _usuario <> String.Empty Then
        '        Dim tblUSER As New rastreo_usuarios(cnn_str.CadenaDeConexion)
        '        With tblUSER
        '            .Where.Id_persona.Value = idPersona
        '            .Query.Load()
        '            If .RowCount > 0 Then
        '                .Fech_upd = Now
        '                .User_upd = mi_usuario.idUsuario
        '            Else
        '                .AddNew()
        '                .Fech_ins = Now
        '                .User_ins = mi_usuario.idUsuario
        '            End If
        '            .Id_persona = idPersona
        '            .s_Usuario = _usuario
        '            If _password <> String.Empty Then
        '                .s_Pass = Cryptografia.SHA256Hash(_password)
        '            End If
        '            .Save()
        '            Response.Redirect(Request.Url.ToString(), False)
        '        End With
        '    End If
        'End If
        If e.CommandName = "Eliminar" Then
            If permisoDel Then
                Dim Indx As Integer = 0
                Indx = CType(e.CommandArgument, Integer)
                Dim tblPersona As New rastreo_persona(cnn_str.CadenaDeConexion)
                Dim tblCliente As New rastreo_cliente(cnn_str.CadenaDeConexion)
                Dim tblContactos As New rastreo_cliente_contactos(cnn_str.CadenaDeConexion)
                Dim tblEmpleado As New rastreo_empleados(cnn_str.CadenaDeConexion)
                Dim tblUsu As New rastreo_usuarios(cnn_str.CadenaDeConexion)
                Dim tblUsuOp As New rastreogps_usuario_opciones(cnn_str.CadenaDeConexion)
                Dim tblVehiculos As New rastreo_vehiculo(cnn_str.CadenaDeConexion)

                With tblUsu
                    .Where.Id_persona.Value = Indx
                    If .Query.Load() Then
                        If .RowCount > 0 Then
                            tblUsuOp.Where.Id_usuario.Value = .Idrastreo_usuarios
                            If tblUsuOp.Query.Load() Then
                                If .RowCount > 0 Then
                                    tblUsuOp.DeleteAll()
                                    tblUsuOp.Save()
                                End If
                            End If
                            .DeleteAll()
                            .Save()
                        End If
                    End If
                End With

                With tblContactos
                    .Where.Rastreo_cliente_id_cliente.Value = Indx
                    If .Query.Load() Then
                        If .s_User_ins <> String.Empty Then
                            If .RowCount > 0 Then
                                .DeleteAll()
                                .Save()
                            End If
                        End If
                    End If
                End With

                With tblVehiculos
                    .Where.Id_cliente.Value = Indx
                    If .Query.Load() Then
                        If .s_User_ins <> String.Empty Then
                            If .RowCount > 0 And Indx = .Id_cliente Then
                                .DeleteAll()
                                .Save()
                            End If
                        End If
                    End If
                End With

                With tblCliente
                    If .LoadByPrimaryKey(Indx) Then
                        If .s_User_ins <> String.Empty Then
                            If .RowCount > 0 And Indx = .Id_cliente Then
                                .MarkAsDeleted()
                                .Save()
                            End If
                        End If
                    End If
                End With

                With tblEmpleado
                    If .LoadByPrimaryKey(Indx) Then
                        If .s_User_ins <> String.Empty Then
                            If .RowCount > 0 And Indx = .Id_empleado Then
                                .MarkAsDeleted()
                                .Save()
                            End If
                        End If
                    End If
                End With

                With tblPersona
                    If .LoadByPrimaryKey(Indx) Then
                        If .s_User_ins <> String.Empty Then
                            If .RowCount > 0 Then
                                .MarkAsDeleted()
                                .DeleteAll()
                                .Save()
                            End If
                        End If
                    End If
                End With

                Response.Redirect(Request.Url.ToString)
            Else
                Session.Add("msjerror", "No tiene permisos para eliminar registros")
            End If
        End If
    End Sub

    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        Try
            Dim mensaje As String = String.Empty
            mensaje = Session("msjerror")
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

    Public Overloads Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)

    End Sub

    Protected Sub btnExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExcel.Click
    End Sub

    Public Sub ExportExcel()

        Dim sb As New StringBuilder()
        Dim sw As New StringWriter(sb)
        Dim htw As New HtmlTextWriter(sw)

        Try
            htw.Flush()

            'gv_Lista_Personas.PageSize = 1000
            'gv_Lista_Personas.DataBind()
            'gv_Lista_Personas.Columns(gv_Lista_Personas.Columns.Count - 1).Visible = False
            gv_Lista_Personas.Columns(gv_Lista_Personas.Columns.Count - 2).Visible = False
            gv_Lista_Personas.Columns(gv_Lista_Personas.Columns.Count - 4).Visible = False
            For Each row As GridViewRow In gv_Lista_Personas.Rows
                'row.Cells(0)
                row.Cells(3).Controls.RemoveAt(0)
                row.Cells(3).Controls.RemoveAt(1)
                row.Cells(3).Controls.RemoveAt(2)
                row.Cells(3).Controls.RemoveAt(3)
                row.Cells(3).Controls.RemoveAt(0)
                row.Cells(3).Controls.RemoveAt(0)

                row.Cells(5).Controls.RemoveAt(0)
                row.Cells(5).Controls.RemoveAt(1)
                row.Cells(5).Controls.RemoveAt(2)
                row.Cells(5).Controls.RemoveAt(3)
                row.Cells(5).Controls.RemoveAt(0)
                row.Cells(5).Controls.RemoveAt(0)
                'CType(row.Cells(3).Controls(1), AjaxControlToolkit.HoverMenuExtender) = DBNull.Value
                'CType(row.Cells(3).Controls(1), AjaxControlToolkit.HoverMenuExtender).Enabled = False
                'ctype(row.Cells(3).Controls(1),AjaxControlToolkit.HoverMenuExtender).
                'Dim i As Integer = 0
            Next

            gv_Lista_Personas.RenderControl(htw)

            HttpContext.Current.Response.Clear()
            HttpContext.Current.Response.Buffer = True
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel"
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=listapersonas_" & mi_usuario.Usuario & Now.ToString("_ddMMyyy_HHmm_") & ".xls")
            HttpContext.Current.Response.Charset = "UTF-8"
            HttpContext.Current.Response.ContentEncoding = Encoding.Default '  New System.Text.UTF8Encoding(False)
            HttpContext.Current.Response.Write(sb.ToString())
            HttpContext.Current.Response.End()

        Catch ex As Exception

        End Try
    End Sub

    Protected Sub ddlBuscar_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlBuscar.SelectedIndexChanged
        'Session("COLUMNA") = 77
    End Sub

    Protected Sub gv_Lista_Personas_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_Lista_Personas.RowDataBound

    End Sub

    Public Function GetContactos(ByVal id_persona As String) As String
        Dim idPersona As Integer = 0
        Dim tableContactos As String = String.Empty
        Dim tblContactos As New rastreo_cliente_contactos(cnn_str.CadenaDeConexion)
        With tblContactos
            If Int32.TryParse(id_persona, idPersona) Then
                .Where.Rastreo_cliente_id_cliente.Value = idPersona
                If .Query.Load() Then
                    While Not .EOF
                        tableContactos = tableContactos & "<table class='td' cellpadding='0' cellspacing='0' width='100%'>"
                        tableContactos = tableContactos & "<tr><td class='td'>Nombre:</td><td class='td'>" & _
                                                .s_Nombre_apellido_razonsocial & "</td></tr>"
                        tableContactos = tableContactos & "<tr><td class='td'>Relacion:</td><td class='td'>" & _
                                                .s_Relacion & "</td></tr>"
                        tableContactos = tableContactos & "<tr><td class='td'>Telefono:</td><td class='td'>" & _
                                                .s_Telefono & "</td></tr>"
                        tableContactos = tableContactos & "<tr><td class='td'>E-mail:</td><td class='td'>" & _
                                                .s_Email & "</td></tr>"
                        tableContactos = tableContactos & "</table>"
                        .MoveNext()
                    End While
                End If
            End If
        End With
        GetContactos = tableContactos
        Return GetContactos
    End Function

End Class