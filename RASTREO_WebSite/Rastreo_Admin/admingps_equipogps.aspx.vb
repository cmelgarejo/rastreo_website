Imports MyGeneration.dOOdads
Imports RASTREOmw

Partial Class RASTREO_admin_equipogps
    Inherits System.Web.UI.Page
    Public mi_usuario As New Usuario()
    Dim idrastreogps_equipogps As Integer = 0


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
            permisoAdd = Utilidades.FindPermissionFor(mi_usuario.idUsuario, "EQUIPOSGPS_ADMIN-A")
            permisoMod = Utilidades.FindPermissionFor(mi_usuario.idUsuario, "EQUIPOSGPS_ADMIN-M")
            permisoQry = Utilidades.FindPermissionFor(mi_usuario.idUsuario, "EQUIPOSGPS_ADMIN-C")
            If Not permisoAdd Or Not permisoMod Or Not permisoQry Then
                Response.Redirect("admin_personas_lista.aspx", False)
            End If
        Catch myEX As Exception
            Session.Add("msjerror", myEX.Message + "<br>" + myEX.StackTrace)
        End Try
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

            idrastreogps_equipogps = CType(Request.QueryString("id"), Integer)

            If Not IsPostBack Then

                If Not Request.UrlReferrer Is Nothing And ViewState("GO_BACK_URL") Is Nothing Then
                    ViewState.Add("GO_BACK_URL", Request.UrlReferrer.ToString)
                End If
                BindTipoEquipoGPS()
                BindSIM()
                If idrastreogps_equipogps > 0 Then
                    ViewState.Add("admingps_equipogps_update", True)
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

    Private Sub BindTipoEquipoGPS()
        Utilidades.fillDDL(ddlTipoDeEquipoGPS, New rastreogps_tipoequipo(cnn_str.CadenaDeConexion), _
                           rastreogps_tipoequipo.ColumnNames.Tipo_equipo, _
                           rastreogps_tipoequipo.ColumnNames.Idrastreogps_tipoequipo)
    End Sub

    Private Sub BindSIM(Optional ByVal selecteditem As String = "")
        Utilidades.fillDDL(ddlSIMs, New rastreogps_simcards(cnn_str.CadenaDeConexion), _
                           rastreogps_simcards.ColumnNames.Sim_nro, _
                           rastreogps_simcards.ColumnNames.Idrastreogps_simcards)
        ddlSIMs.Items.Add("DESINSTALADO")
        If selecteditem <> String.Empty Then
            ddlSIMs.SelectedItem.Selected = False
            ddlSIMs.Items.FindByText(selecteditem).Selected = True
        End If
        Utilidades.fillDDL(ddlSIMs2, New rastreogps_simcards(cnn_str.CadenaDeConexion), _
                           rastreogps_simcards.ColumnNames.Sim_nro, _
                           rastreogps_simcards.ColumnNames.Idrastreogps_simcards)
    End Sub
    Private Sub CargarDatos()
        Try
            Dim tblEQUIPOGPS As New rastreogps_equipogps(cnn_str.CadenaDeConexion)
            With tblEQUIPOGPS
                .LoadByPrimaryKey(idrastreogps_equipogps)

                txtEQUIPOGPS_id_equipo_gps.Text = .Id_equipo_gps
                txtEQUIPOGPS_nro_serie.Text = .Nro_serie_gps

                ddlTipoDeEquipoGPS.SelectedItem.Selected = False
                ddlSIMs.SelectedItem.Selected = False

                If .s_Tipoequipo <> "" Then ddlTipoDeEquipoGPS.Items.FindByValue(.s_Tipoequipo).Selected = True
                If .s_Id_simcard <> String.Empty Then
                    ddlSIMs.Items.FindByValue(.s_Id_simcard).Selected = True
                End If
                If .s_Id_simcard2 <> String.Empty Then
                    ddlSIMs2.SelectedItem.Selected = False
                    ddlSIMs.Items.FindByValue(.s_Id_simcard2).Selected = True
                End If

            End With
        Catch myEX As Exception
            ViewState.Add("msjerror", myEX.Message + "<br>" + myEX.StackTrace)
        End Try
    End Sub

    Private Function Guardar_Datos() As Boolean
        Dim tblEQUIPOGPS As New rastreogps_equipogps(cnn_str.CadenaDeConexion)
        Dim Transaccion As TransactionMgr = TransactionMgr.ThreadTransactionMgr
        Try
            Guardar_Datos = False
            Transaccion.BeginTransaction()

            With tblEQUIPOGPS

                If ViewState("admingps_equipogps_update") Then
                    .LoadByPrimaryKey(idrastreogps_equipogps)
                    .Fech_upd = Now
                    .User_upd = mi_usuario.idUsuario
                Else
                    .Where.Id_equipo_gps.Value = txtEQUIPOGPS_id_equipo_gps.Text.Trim.ToUpperInvariant()
                    .Where.Tipoequipo.Value = ddlTipoDeEquipoGPS.SelectedValue
                    If .Query.Load() Then
                        If .RowCount > 0 Then
                            Dim tblTE As New rastreogps_tipoequipo(cnn_str.CadenaDeConexion)
                            tblTE.Where.Idrastreogps_tipoequipo.Value = .Tipoequipo
                            tblTE.Query.Load()
                            ViewState.Add("msjerror", "Ya existe un equipo GPS con ese ID y tipo de equipo. Info: ID=" + .Id_equipo_gps + " Tipo Equipo=" + tblTE.s_Tipo_equipo)
                            Throw New Exception("msjerror")
                        End If
                    End If
                    .FlushData()

                    If ddlSIMs.SelectedValue <> "DESINSTALADO" Then
                        .Where.Id_simcard.Value = ddlSIMs.SelectedValue.Trim.ToUpperInvariant()
                        If .Query.Load() Then
                            If .RowCount > 0 Then
                                Session.Add("msjerror", "Ya existe un equipo GPS con ese SIMCard. Info: ID=" + .Id_equipo_gps)
                                Throw New Exception("msjerror")
                            End If
                        End If
                    End If
                    .FlushData()

                    .Where.Id_simcard2.Value = ddlSIMs2.SelectedValue.Trim
                    If .Query.Load() Then
                        If .RowCount > 0 Then
                            ViewState.Add("msjerror", "Ya existe un equipo GPS con ese SIMCard. Info: ID=" + .Id_equipo_gps)
                            Throw New Exception("msjerror")
                        End If
                    End If
                    .FlushData()

                    .AddNew()
                    .Fech_ins = Now
                    .User_ins = mi_usuario.idUsuario
                End If

                .s_Nro_serie_gps = txtEQUIPOGPS_nro_serie.Text.Trim.ToUpperInvariant()
                .s_Id_equipo_gps = txtEQUIPOGPS_id_equipo_gps.Text.Trim.ToUpperInvariant()

                If ddlTipoDeEquipoGPS.SelectedValue = String.Empty Then
                    ViewState.Add("msjerror", "Debe seleccionar un equipo gps para el vehiculo.")
                    Throw New Exception("msjerror")
                Else
                    .s_Tipoequipo = ddlTipoDeEquipoGPS.SelectedValue
                End If

                If ddlSIMs.SelectedValue = String.Empty Then
                    Session.Add("msjerror", "Debe seleccionar un tipo especifico de vehiculo.")
                    Throw New Exception("msjerror")
                Else
                    If ddlSIMs.SelectedValue <> "DESINSTALADO" Then
                        .s_Id_simcard = ddlSIMs.SelectedValue
                    Else
                        .SetColumnNull(rastreogps_equipogps.ColumnNames.Id_simcard)
                    End If
                End If

                If ddlSIMs.SelectedValue = String.Empty Then
                    Session.Add("msjerror", "Debe seleccionar un tipo especifico de vehiculo.")
                    Throw New Exception("msjerror")
                Else
                    If ddlSIMs.SelectedValue <> "DESINSTALADO" Then
                        .s_Id_simcard = ddlSIMs.SelectedValue
                        If (dualSIM.Checked) Then
                            .s_Id_simcard2 = ddlSIMs2.SelectedValue
                        Else
                            .SetColumnNull(rastreogps_equipogps.ColumnNames.Id_simcard2)
                        End If
                    Else
                        .SetColumnNull(rastreogps_equipogps.ColumnNames.Id_simcard)
                    End If
                End If

                .Save()

                If Not ViewState("admingps_equipogps_update") Then
                    Dim tblTDE As New rastreogps_tipoevento(cnn_str.CadenaDeConexion)
                    tblTDE.Where.Id_tipoequipo.Value = ddlTipoDeEquipoGPS.SelectedValue
                    tblTDE.Query.Load()
                    If tblTDE.RowCount > 0 Then
                        Dim tblEE As New rastreogps_equipo_eventos(cnn_str.CadenaDeConexion)
                        While Not tblTDE.EOF
                            tblEE.AddNew()
                            tblEE.Id_equipogps = .Idrastreogps_equipogps
                            tblEE.Id_tipoevento = tblTDE.Idrastreogps_tipo_evento
                            tblEE.Evento = tblTDE.Descripcion.ToUpperInvariant().Trim()
                            tblEE.Habilitado = True
                            tblEE.Arch_sonido = tblTDE.Arch_sonido
                            tblEE.Sonoro = tblTDE.Sonoro
                            tblEE.User_ins = mi_usuario.idUsuario
                            tblEE.Fech_ins = Now
                            tblEE.Save()
                            tblTDE.MoveNext()
                        End While
                    End If
                End If

            End With
            Transaccion.CommitTransaction()

            If Not ViewState("admingps_equipogps_update") Then
                Session.Add("msjerror", "EquipoGPS registrado exitosamente!")
            Else
                Session.Add("msjerror", "EquipoGPS actualizado exitosamente!")
            End If

            Guardar_Datos = True
        Catch myEX As Exception
            Guardar_Datos = False
            If Not myEX.Message.Contains("msjerror") Then ViewState.Add("msjerror", myEX.Message + "<br>" + myEX.StackTrace)
            If Not Transaccion.HasBeenRolledBack Then Transaccion.RollbackTransaction()
        End Try
    End Function

    Protected Sub btnGUARDAR_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGUARDAR.Click
        If Guardar_Datos() Then
            If ViewState("GO_BACK_URL") <> String.Empty Then
                Response.Redirect(ViewState("GO_BACK_URL"))
            Else
                JavascriptHelper.Custom(Page, "history.back();", "goback")
            End If
        End If
    End Sub

    Protected Sub lnktipodeequipogps_nueva_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkTipoDeEquipoGPS_nueva.Click
        panel_TipoDeEquipoGPSs_newedit.Visible = True
        panel_TipoDeEquipoGPS_select.Visible = False
        panel_SIM_select.Visible = False
        txtTipoDeEquipoGPS_Add.Text = String.Empty
        txtTipoDeEquipoGPS_Add.Focus()
    End Sub

    Protected Sub lnktipodeequipogps_edit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkTipoDeEquipoGPS_edit.Click
        panel_TipoDeEquipoGPSs_newedit.Visible = True
        panel_TipoDeEquipoGPS_select.Visible = False
        panel_SIM_select.Visible = False
        ViewState.Add("Edittipodeequipogps", True)
        txtTipoDeEquipoGPS_Add.Text = ddlTipoDeEquipoGPS.SelectedItem.Text.Trim
        txtTipoDeEquipoGPS_Add.Focus()
    End Sub

    Protected Sub lnkSIM_nuevo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkSIM_nuevo.Click
        panel_SIM_newedit.Visible = True
        panel_SIM_select.Visible = False
        panel_TipoDeEquipoGPS_select.Visible = False
        txtSIM_NRO.Text = String.Empty
        txtSIM_NRO.Focus()
    End Sub

    Protected Sub lnkSIM_edit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkSIM_edit.Click
        panel_SIM_newedit.Visible = True
        panel_SIM_select.Visible = False
        panel_TipoDeEquipoGPS_select.Visible = False
        ViewState.Add("EditSIM", True)
        LoadSIMData(ddlSIMs.SelectedValue)
        txtSIM_NRO.Focus()
    End Sub

    Private Sub LoadSIMData(ByVal nro As String)
        Dim tblSIM As New rastreogps_simcards(cnn_str.CadenaDeConexion)
        Try
            With tblSIM
                .LoadByPrimaryKey(nro)
                ddlPrefijos.SelectedItem.Selected = False
                ddlPrefijos.Items.FindByText(.s_Sim_nro.Substring(0, 3)).Selected = True
                txtSIM_NRO.Text = .s_Sim_nro.Substring(3)
                txtSIM_PIN.Text = .s_Sim_pin
                txtSIM_PIN2.Text = .s_Sim_pin2
                txtSIM_PUK.Text = .s_Sim_puk
                txtSIM_PUK2.Text = .s_Sim_puk2
            End With
        Catch myEX As Exception
            ViewState.Add("msjerror", myEX.Message + "<br>" + myEX.StackTrace)
        End Try
    End Sub

    Protected Sub imgbtn_tipodeequipogpssave_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbtn_TipoDeEquipoGPSsave.Click
        Dim tbltipodeequipogps As New rastreogps_tipoequipo(cnn_str.CadenaDeConexion)
        With tbltipodeequipogps
            If ViewState("Edittipodeequipogps") Then
                .Where.Idrastreogps_tipoequipo.Value = ddlTipoDeEquipoGPS.SelectedValue
                If Not .Query.Load() Then
                    ViewState.Add("msjerror", "NOSE QUE PASO MEN - TipoEquipoGPS")
                    Exit Sub
                End If
                .Fech_upd = Now
                .User_upd = mi_usuario.idUsuario
            Else
                .Where.Tipo_equipo.Value = txtTipoDeEquipoGPS_Add.Text.Trim
                If .Query.Load() Then
                    If .RowCount > 0 Then
                        ViewState.Add("msjerror", "Ese Tipo de equipo ya existe.")
                        Exit Sub
                    End If
                End If
                .AddNew()
                .Fech_ins = Now
                .User_ins = mi_usuario.idUsuario
            End If
            .s_Tipo_equipo = txtTipoDeEquipoGPS_Add.Text.Trim.ToUpperInvariant()
            .Save()
            If ViewState("Edittipodeequipogps") Then
                ViewState.Add("msjerror", "Tipo de Equipo GPS modificado con exito.")
            Else
                ViewState.Add("msjerror", "Tipo de Equipo GPS registrado.")
            End If
            BindTipoEquipoGPS()
            panel_TipoDeEquipoGPS_select.Visible = True
            panel_TipoDeEquipoGPSs_newedit.Visible = False
            panel_SIM_select.Visible = True
        End With
    End Sub

    Protected Sub imgbtn_tipodeequipogpscancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbtn_TipoDeEquipoGPScancel.Click
        panel_TipoDeEquipoGPS_select.Visible = True
        panel_TipoDeEquipoGPSs_newedit.Visible = False
        panel_SIM_select.Visible = True
        txtTipoDeEquipoGPS_Add.Text = String.Empty
    End Sub

    Protected Sub imgbtn_SIMsave_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbtn_SIMsave.Click
        Dim tblSIM As New rastreogps_simcards(cnn_str.CadenaDeConexion)
        With tblSIM
            If ViewState("EditSIM") Then
                .Where.Idrastreogps_simcards.Value = ddlSIMs.SelectedValue
                If Not .Query.Load() Then
                    ViewState.Add("msjerror", "NO SE QUE PASO MEN - SIMS")
                    Exit Sub
                End If
                .Fech_upd = Now
                .User_upd = mi_usuario.idUsuario
            Else
                .Where.Sim_nro.Value = ddlPrefijos.SelectedValue + txtSIM_NRO.Text.Trim
                If .Query.Load Then
                    If .RowCount > 0 Then
                        ViewState.Add("msjerror", "SIM con ese numero ya existe.")
                        Exit Sub
                    End If
                End If
                .AddNew()
                .Fech_ins = Now
                .User_ins = mi_usuario.idUsuario
            End If

            .s_Sim_nro = ddlPrefijos.SelectedValue + txtSIM_NRO.Text.Trim.ToUpperInvariant()
            .s_Sim_pin = txtSIM_PIN.Text.Trim.ToUpperInvariant()
            .s_Sim_pin2 = txtSIM_PIN2.Text.Trim.ToUpperInvariant()
            .s_Sim_puk = txtSIM_PUK.Text.Trim.ToUpperInvariant()
            .s_Sim_puk2 = txtSIM_PUK2.Text.Trim.ToUpperInvariant()
            .Save()
            If ViewState("EditSIM") Then
                ViewState.Add("msjerror", "SIM editado con exito.")
            Else
                ViewState.Add("msjerror", "SIM registrado.")
            End If
            BindSIM()
            panel_TipoDeEquipoGPS_select.Visible = True
            panel_SIM_newedit.Visible = False
            panel_SIM_select.Visible = True
        End With

    End Sub

    Protected Sub imgbtn_SIMcancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbtn_SIMcancel.Click
        panel_TipoDeEquipoGPS_select.Visible = True
        panel_SIM_newedit.Visible = False
        panel_SIM_select.Visible = True
        txtSIM_NRO.Text = String.Empty
        txtSIM_NRO.Text = String.Empty
        txtSIM_PIN.Text = String.Empty
        txtSIM_PIN2.Text = String.Empty
        txtSIM_PUK.Text = String.Empty
        txtSIM_PUK2.Text = String.Empty
    End Sub

End Class
