Imports RASTREOmw
Imports MyGeneration.dOOdads
Imports System.IO


Partial Class admin_cliente_vehiculo_lista
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
        permisoAdd = Utilidades.FindPermissionFor(mi_usuario.idUsuario, "VEHICULOS_ADMIN-A")
        permisoDel = Utilidades.FindPermissionFor(mi_usuario.idUsuario, "VEHICULOS_ADMIN-B")
        permisoMod = Utilidades.FindPermissionFor(mi_usuario.idUsuario, "VEHICULOS_ADMIN-M")
        permisoQry = Utilidades.FindPermissionFor(mi_usuario.idUsuario, "VEHICULOS_ADMIN-C")
        'Boton de "NUEVO"
        btnAddVehiculo.Visible = permisoAdd
        'Esto solo funciona si la grilla siempre tiene "Editar" en penultimo lugar.
        gv_Lista_Vehiculos.Columns(gv_Lista_Vehiculos.Columns.Count - 2).Visible = permisoMod
        'Esto solo funciona si la grilla siempre tiene "Eliminar" al ultimo.
        gv_Lista_Vehiculos.Columns(gv_Lista_Vehiculos.Columns.Count - 1).Visible = permisoDel
        'Aqui seteamos a NADA el datasource si no tiene permisos para consultar...
        If Not permisoQry Then gv_Lista_Vehiculos.Visible = False
        gv_Lista_Vehiculos.DataBind()
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Request.QueryString.Count > 0 Then
            If Request.QueryString("id") <> String.Empty Then
                Integer.TryParse(Request.QueryString("id"), idCLIENTE)
            End If
        End If

        If Request.QueryString("sa") <> String.Empty Then
            If chkInactivos.Checked Then
                sqlds_Lista_Cliente_Vehiculo.SelectCommand = "select distinct on (identificador_rastreo) *  from rsview_cliente_vehiculo_equipogps where activo = false order by identificador_rastreo, id_equipo_gps"
            Else
                sqlds_Lista_Cliente_Vehiculo.SelectCommand = "select distinct on (identificador_rastreo) *  from rsview_cliente_vehiculo_equipogps order by identificador_rastreo, id_equipo_gps"
            End If
            btnAddVehiculo.Visible = False
            gv_Lista_Vehiculos.DataSourceID = Nothing
            gv_Lista_Vehiculos.DataSource = Nothing
            gv_Lista_Vehiculos.PageSize = 5000
            gv_Lista_Vehiculos.DataBind()
        Else
            If Request.QueryString("id") <> String.Empty Then
                sqlds_Lista_Cliente_Vehiculo.SelectCommand = "select distinct on (identificador_rastreo) * from rsview_cliente_vehiculo_equipogps where idrastreo_persona = @id_cliente AND activo = NOT @activo AND idrastreo_vehiculo IS NOT NULL ORDER BY identificador_rastreo"
            End If
        End If

        gv_Lista_Vehiculos.DataSourceID = Nothing
        gv_Lista_Vehiculos.DataSource = Nothing
        'gv_Lista_Vehiculos.DataBind = Nothing
        sqlds_Lista_Cliente_Vehiculo.DataBind()
        gv_Lista_Vehiculos.DataSourceID() = sqlds_Lista_Cliente_Vehiculo.ID
        gv_Lista_Vehiculos.DataBind()

        If Not IsPostBack Then
            lblCliente.Text = String.Empty
            If idCLIENTE > 0 Then
                Dim tblCliente As New rastreo_persona(cnn_str.CadenaDeConexion)
                With tblCliente
                    .LoadByPrimaryKey(idCLIENTE)
                    If .RowCount > 0 And .s_nro_documento <> String.Empty Then
                        Select Case .Tipo_persona
                            Case "F"
                                lblCliente.Text = .s_Nombre + " " + .s_Apellido
                            Case "J"
                                lblCliente.Text = .s_Razon_social
                        End Select
                    End If
                End With
            Else
                'Dim rspersona As New rastreo_persona(cnn_str.CadenaDeConexion)
                'Dim rsclientes As New rastreo_cliente(cnn_str.CadenaDeConexion)
                'rsclientes.LoadAll()
                'While Not rsclientes.EOF
                '    rspersona.Where.Idrastreo_persona.Value = rsclientes.Id_cliente
                '    rspersona.LoadByPrimaryKey(rsclientes.Id_cliente)
                '    If rspersona.RowCount > 0 Then
                '        If rspersona.s_Idrastreo_persona <> String.Empty Then
                '            Dim nombre As String = String.Empty
                '            If rspersona.Tipo_persona = "F" Then
                '                nombre = rspersona.Nombre + " " + rspersona.Apellido
                '            Else
                '                nombre = rspersona.Razon_social
                '            End If
                '            Dim LI As New ListItem(nombre, rspersona.Idrastreo_persona)
                '            ddl_PickCliente.Items.Add(LI)
                '        End If
                '    End If
                '    rsclientes.MoveNext()
                'End While
                'rsview_cliente_usuarios
                Utilidades.fillDDL(Me.ddl_PickCliente, New rsview_cliente_usuarios(cnn_str.CadenaDeConexion), _
                                   rsview_cliente_usuarios.ColumnNames.Cliente, _
                                   rsview_cliente_usuarios.ColumnNames.Id_persona)
            End If
        End If
    End Sub

    Protected Sub btnAddVehiculo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddVehiculo.Click
        Try
            Response.Redirect("admin_cliente_vehiculo.aspx?cid=" + idCLIENTE.ToString, False)
        Catch X As Exception
            Response.Write(X.Message)
        End Try
    End Sub

    Protected Sub gv_Lista_Vehiculos_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_Lista_Vehiculos.RowCommand
        If permisoDel And e.CommandName = "Eliminar" Then
            Dim Indx As Integer = 0
            Indx = CType(e.CommandArgument, Integer)
            Dim tblVehiculos As New rastreo_vehiculo(cnn_str.CadenaDeConexion)
            With tblVehiculos
                .LoadByPrimaryKey(Indx, idCLIENTE)
                .MarkAsDeleted()
                .Save()
            End With
            Response.Redirect(Request.Url.ToString)
        End If
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

    Protected Sub btnSelectCliente_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelectCliente.Click
        Try
            If Not ddl_PickCliente.SelectedItem Is Nothing Then
                Response.Redirect("admin_cliente_vehiculo_lista.aspx?id=" + ddl_PickCliente.SelectedValue, False)
            End If
        Catch Myex As Exception
            Session.Add("msjerror", Myex.Message + "<br>" + Myex.StackTrace)
        End Try
    End Sub

    Protected Sub btnTodos_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnTodos.Click
        Try
            If Not ddl_PickCliente.SelectedItem Is Nothing Then
                Response.Redirect("admin_cliente_vehiculo_lista.aspx?sa=1", False)
            End If
        Catch Myex As Exception
            Session.Add("msjerror", Myex.Message + "<br>" + Myex.StackTrace)
        End Try
    End Sub

    Protected Sub btnExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExcel.Click
        ExportExcel()
    End Sub

    Public Overloads Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)

    End Sub

    Public Sub ExportExcel()

        Dim sb As New StringBuilder()
        Dim sw As New StringWriter(sb)
        Dim htw As New HtmlTextWriter(sw)


        'gv_Lista_Vehiculos.Columns(13).Visible = False 'mis cambios 28082010 - Pedro Silva
        'gv_Lista_Vehiculos.Columns(14).Visible = False 'mis cambios 28082010 - Pedro Silva
        'gv_Lista_Vehiculos.Columns(15).Visible = False 'mis cambios 28082010 - Pedro Silva
        'gv_Lista_Vehiculos.Columns(16).Visible = False
        gv_Lista_Vehiculos.Columns(17).Visible = False
        'gv_Lista_Vehiculos.Columns(18).Visible = False  'mis cambios 28082010 - Pedro Silva
        'gv_Lista_Vehiculos.Columns(19).Visible = False  'mis cambios 28082010 - Pedro Silva
        'gv_Lista_Vehiculos.Columns(20).Visible = False  'mis cambios 28082010 - Pedro Silva
        'gv_Lista_Vehiculos.Columns(21).Visible = False  'mis cambios 28082010 - Pedro Silva
        htw.Flush()
        gv_Lista_Vehiculos.RenderControl(htw)

        HttpContext.Current.Response.Clear()
        HttpContext.Current.Response.Buffer = True
        'HttpContext.Current.Response.ContentType = "application/vnd.ms-excel" ' application/vnd.oasis.opendocument.spreadsheet
        HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=listado_" & mi_usuario.NombreUsuario & Now.ToString("_ddMMyyy_HHmm_") & ".xlsx")
        HttpContext.Current.Response.Charset = "UTF-8"
        HttpContext.Current.Response.ContentEncoding = Encoding.Default '  New System.Text.UTF8Encoding(False)
        HttpContext.Current.Response.Write(sb.ToString())
        HttpContext.Current.Response.End()

    End Sub

End Class
