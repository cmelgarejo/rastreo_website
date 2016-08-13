Imports MyGeneration.dOOdads
Imports RASTREOmw

Partial Class RASTREO_Administracion_eventosgps
    Inherits System.Web.UI.Page
    Public mi_usuario As New Usuario()
    Dim id_tipoequipo As Integer = 0
    Dim idrastreogps_tipoevento As Integer = 0

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
            permisoAdd = Utilidades.FindPermissionFor(mi_usuario.idUsuario, "EVENTOSGPS_ADMIN-A")
            permisoMod = Utilidades.FindPermissionFor(mi_usuario.idUsuario, "EVENTOSGPS_ADMIN-M")
            permisoQry = Utilidades.FindPermissionFor(mi_usuario.idUsuario, "EVENTOSGPS_ADMIN-C")
            If Not permisoAdd Or Not permisoMod Or Not permisoQry Then
                Response.Redirect("admingps_eventos_lista.aspx", False)
            End If
        Catch myEX As Exception
            Session.Add("msjerror", myEX.Message + "<br>" + myEX.StackTrace)
        End Try
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

            idrastreogps_tipoevento = CType(Request.QueryString("id"), Integer)
            id_tipoequipo = CType(Request.QueryString("tdeid"), Integer)

            If Not IsPostBack Then
                If id_tipoequipo = 0 Then
                    Response.Redirect("admingps_eventos_lista.aspx", False)
                Else
                    Dim tblTDE As New rastreogps_tipoequipo(cnn_str.CadenaDeConexion)
                    tblTDE.LoadByPrimaryKey(id_tipoequipo)
                    lbTipoEquipo.Text = "Tipo de Equipo GPS: " + tblTDE.s_Tipo_equipo + " - Tipo de reporte del equipo: " + tblTDE.s_Tipo_de_reporte
                End If
                If Not Request.UrlReferrer Is Nothing And ViewState("GO_BACK_URL") Is Nothing Then
                    ViewState.Add("GO_BACK_URL", Request.UrlReferrer.ToString)
                End If
                If idrastreogps_tipoevento > 0 Then
                    ViewState.Add("admingps_evento_update", True)
                    CargarDatos()
                    btnGUARDAR.Text = "Actualizar"
                    btnCANCELAR.Text = "Volver"
                End If
            Else
                ':D
            End If

        Catch myEX As Exception
            ViewState.Add("msjerror", myEX.Message + "<br>" + myEX.StackTrace)
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
            ViewState.Add("msjerror", myEX.Message + "<br>" + myEX.StackTrace)
        End Try
    End Sub

    Private Sub CargarDatos()
        Try
            Dim tblEQUIPOGPS As New rastreogps_tipoevento(cnn_str.CadenaDeConexion)
            With tblEQUIPOGPS
                .LoadByPrimaryKey(idrastreogps_tipoevento)

                txtEVENTO_DESC.Text = .Descripcion
                txtEVENTO_evento.Text = .Evento
                If .s_Color <> String.Empty Then
                    cpColor_Evento.SelectedHexValue = .Color
                End If
                If .s_Sonoro <> String.Empty Then
                    checkSonoro.Checked = .Sonoro
                End If
            End With
        Catch myEX As Exception
            ViewState.Add("msjerror", myEX.Message + "<br>" + myEX.StackTrace)
        End Try
    End Sub

    Private Function Guardar_Datos() As Boolean
        Dim tblEVENTOGPS As New rastreogps_tipoevento(cnn_str.CadenaDeConexion)
        Try
            Guardar_Datos = False

            With tblEVENTOGPS

                If ViewState("admingps_evento_update") Then
                    .LoadByPrimaryKey(idrastreogps_tipoevento)
                    .Fech_upd = Now
                    .User_upd = mi_usuario.idUsuario
                Else
                    .Where.Id_tipoequipo.Value = id_tipoequipo
                    '.Where.Descripcion.Value = txtEVENTO_DESC.Text
                    .Where.Evento.Value = txtEVENTO_evento.Text.ToUpperInvariant()
                    If .Query.Load() Then
                        If .RowCount > 0 Then
                            Session.Add("msjerror", "Ya existe un evento para este tipo de equipo.")
                            Return False
                        End If
                    End If
                    .FlushData()
                    .AddNew()
                    .Fech_ins = Now
                    .User_ins = mi_usuario.idUsuario
                End If

                .s_Evento = txtEVENTO_evento.Text.Trim.ToUpperInvariant()
                .s_Descripcion = txtEVENTO_DESC.Text.Trim.ToUpperInvariant()
                .Id_tipoequipo = id_tipoequipo
                ''Por alguna extraña razón Firefox tira como value de colores en formato rgb(rojo,verde,azul)
                '' entonces tengo que convertir de esto a hexa, colo los 3 valores, y convierto a integer
                If cpColor_Evento.SelectedHexValue.Contains("rgb") Then
                    Dim array_colores(3) As Integer
                    Dim i As Integer = 0
                    Dim colores As String() = _
                    cpColor_Evento.SelectedHexValue.Replace("rgb(", _
                                String.Empty).Replace(")", String.Empty).Split(",")
                    For Each color As String In colores
                        array_colores(i) = CInt(color.Trim)
                        i += 1
                    Next
                    ''Vuelvo a usar a "i" no como contador, sino como storage para el valor
                    ''que voy a convertir de integer a hexa usando ToString("X") formato de int a hexa
                    ''''i = RGB(array_colores(0), array_colores(1), array_colores(2))
                    .s_Color = "#" + array_colores(0).ToString("X").PadLeft(2, "0") + array_colores(1).ToString("X").PadLeft(2, "0") + array_colores(2).ToString("X").PadLeft(2, "0")
                Else
                    ''Funciona asi directamente en IE... omg! :S
                    .s_Color = cpColor_Evento.SelectedHexValue
                End If

                .Sonoro = checkSonoro.Checked
                ' Cambiar a que busque archivos de sonido en la maquina servidor y dar una lista para elegir.
                '.Arch_sonido = FileUpLoadSound.FileName

                .Save()

                Dim equiposgps As New rastreogps_equipogps(cnn_str.CadenaDeConexion)
                Dim requiposeventos As New rastreogps_equipo_eventos(cnn_str.CadenaDeConexion)
                equiposgps.Where.Tipoequipo.Value = .Id_tipoequipo
                equiposgps.Query.Load()
                If equiposgps.RowCount > 0 Then
                    If equiposgps.Id_equipo_gps <> String.Empty Then
                        While Not equiposgps.EOF
                            requiposeventos.LoadByPrimaryKey(equiposgps.Idrastreogps_equipogps, .Idrastreogps_tipo_evento)
                            If requiposeventos.s_Id_tipoevento = String.Empty Then
                                requiposeventos.AddNew()
                                requiposeventos.Habilitado = True
                                requiposeventos.Fech_ins = Now
                                requiposeventos.User_ins = mi_usuario.idUsuario
                                requiposeventos.Id_equipogps = equiposgps.Idrastreogps_equipogps
                                requiposeventos.Id_tipoevento = .Idrastreogps_tipo_evento
                                requiposeventos.Evento = .Descripcion
                                requiposeventos.Save()
                            End If
                            equiposgps.MoveNext()
                        End While
                    End If
                End If


            End With

            If Not ViewState("admingps_equipogps_update") Then
                Session.Add("msjerror", "Evento GPS actualizado exitosamente!")
            Else
                Session.Add("msjerror", "Evento GPS registrado exitosamente!")
            End If

            Guardar_Datos = True
        Catch myEX As Exception
            Guardar_Datos = False
            If Not myEX.Message.Contains("msjerror") Then Session.Add("msjerror", myEX.Message + "<br>" + myEX.StackTrace)
        End Try
    End Function

    Protected Sub btnGUARDAR_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGUARDAR.Click
        If Guardar_Datos() Then
            Response.Redirect("admingps_eventos_lista.aspx?id=" + id_tipoequipo.ToString, False)
        End If
    End Sub


End Class
