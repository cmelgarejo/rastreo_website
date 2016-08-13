Imports System.ComponentModel
Imports RASTREOmw
Imports MyGeneration.dOOdads

Partial Class Envio_De_Comandos
    Inherits System.Web.UI.Page

    Public miUsuario As Usuario

    Protected Overloads Overrides Sub Render(ByVal writer As HtmlTextWriter)
        Utilidades.CompresionASPX(HttpContext.Current)
        MyBase.Render(writer)
    End Sub

    Private Sub BindTipoEquipoDLL()
        Utilidades.fillDDL(ddlTipoEquipo, New rastreogps_tipoequipo(cnn_str.CadenaDeConexion), _
                           rastreogps_tipoequipo.ColumnNames.Tipo_equipo, _
                           rastreogps_tipoequipo.ColumnNames.Idrastreogps_tipoequipo)
    End Sub

    Private Sub BindEquipoDDL(ByVal idTipoEquipo As Integer)
        If Not String.IsNullOrEmpty(idTipoEquipo) Then
            Dim tblEquiposGPS As New rs_select_vehiculo_tipoequipo_equipogps(cnn_str.CadenaDeConexion)
            With tblEquiposGPS
                .Where.Descripcion.Operator = WhereParameter.Operand.ILike
                .Where.Descripcion.Value = "%" & ddlTipoEquipo.SelectedItem.Text & "%"
                If .Query.Load() Then
                    If .RowCount > 0 And Not String.IsNullOrEmpty(.s_Descripcion) Then
                        ddlEquipoGPS.DataSource = tblEquiposGPS.DefaultView
                        ddlEquipoGPS.DataTextField = rs_select_vehiculo_tipoequipo_equipogps.ColumnNames.Descripcion
                        ddlEquipoGPS.DataValueField = rs_select_vehiculo_tipoequipo_equipogps.ColumnNames.Idrastreogps_equipogps
                        ddlEquipoGPS.DataBind()
                    End If
                Else
                    ddlEquipoGPS.Items.Clear()
                    ddlEquipoGPS.Items.Add("NO HAY EQUIPOS DE ESTE TIPO INSTALADOS.")
                End If
            End With
        End If
    End Sub

    Private Sub BindCMD_De_Tipo_Equipo(ByVal idTipoEquipo As Integer)
        If Not String.IsNullOrEmpty(idTipoEquipo) Then
            Dim tblCMDTipoEquipoGPS As New rastreogps_tipoequipo_comandos(cnn_str.CadenaDeConexion)
            With tblCMDTipoEquipoGPS
                .Where.Id_tipoequipo.Value = idTipoEquipo
                If .Query.Load() Then
                    If .RowCount > 0 And Not String.IsNullOrEmpty(.s_Id_tipoequipo) Then
                        ddlComandos_de_TipoEquipo.DataSource = tblCMDTipoEquipoGPS.DefaultView
                        ddlComandos_de_TipoEquipo.DataTextField = _
                                rastreogps_tipoequipo_comandos.ColumnNames.Descripcion
                        ddlComandos_de_TipoEquipo.DataValueField = _
                                rastreogps_tipoequipo_comandos.ColumnNames.Idrastreogps_tipoequipo_comandos
                        ddlComandos_de_TipoEquipo.DataBind()
                    End If
                Else
                    ddlComandos_de_TipoEquipo.Items.Clear()
                    ddlComandos_de_TipoEquipo.Items.Add("SIN COMANDOS PARA ESTE TIPO DE EQUIPO.")
                End If
            End With
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim sbScript As New StringBuilder()
        sbScript.Append("<script language='JavaScript' type='text/javascript'>" + ControlChars.Lf)
        sbScript.Append("<!--" + ControlChars.Lf)
        'sbScript.Append("window.close();" + ControlChars.Lf)
        sbScript.Append("// -->" + ControlChars.Lf)
        sbScript.Append("</script>" + ControlChars.Lf)

        miUsuario = CType(Session("session_UsuarioRASTREO"), Usuario)
        If Not miUsuario Is Nothing Then
            If Not miUsuario.Empleado Then
                Response.Redirect("Principal.aspx", False)
            End If
        Else
            Session.Add("msjerror", "No esta logueado, logueese pues!")
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Onload_closewindow", _
                                                                  sbScript.ToString(), False)
            Return
        End If

        If Not IsPostBack Then
            BindTipoEquipoDLL()
        End If

        If Request.QueryString("eqid") Is Nothing Then
            'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Onload_closewindow", _
            '                                                      sbScript.ToString(), False)
            'Return
        Else
            If Not IsPostBack Then
                Dim tblEQUIPOGPS As New rastreogps_equipogps(cnn_str.CadenaDeConexion)
                If tblEQUIPOGPS.LoadByPrimaryKey(Request.QueryString("eqid")) Then
                    Dim tblTIPOEQUIPO As New rastreogps_tipoequipo(cnn_str.CadenaDeConexion)
                    If tblTIPOEQUIPO.LoadByPrimaryKey(tblEQUIPOGPS.Tipoequipo) Then
                        ddlTipoEquipo.SelectedItem.Selected = False
                        ddlTipoEquipo.Items.FindByValue(tblTIPOEQUIPO.Idrastreogps_tipoequipo).Selected = True
                        ddlTipoEquipo_SelectedIndexChanged(sender, e)
                    End If
                End If
                AddItem_ListaVehiculos(Request.QueryString("eqid"))
            End If
        End If

        If Not ddlTipoEquipo.SelectedItem Is Nothing Then
            'Para tener el valor del tipo de reporte del equipo en la pagina para su uso
            Dim pi As New rastreogps_tipoequipo(cnn_str.CadenaDeConexion)
            If pi.LoadByPrimaryKey(Convert.ToInt32(ddlTipoEquipo.SelectedValue)) Then
                TipoDeReporte.Value = pi.Tipo_de_reporte
                Session.Add("msjerror", TipoDeReporte.Value)
            End If
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
            ViewState.Add("msjerror", myEX.Message + "<br>" + myEX.StackTrace)
        End Try
    End Sub

    Protected Sub ddlTipoEquipo_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlTipoEquipo.DataBound
        BindEquipoDDL(ddlTipoEquipo.SelectedValue)
        BindCMD_De_Tipo_Equipo(ddlTipoEquipo.SelectedValue)
        listaEQUIPOSGPS.Items.Clear()
    End Sub

    Protected Sub ddlTipoEquipo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlTipoEquipo.SelectedIndexChanged
        BindEquipoDDL(ddlTipoEquipo.SelectedValue)
        BindCMD_De_Tipo_Equipo(ddlTipoEquipo.SelectedValue)
        txtTDEq_CMD.Text = String.Empty
        txtTDEq_DESC.Text = String.Empty
        listaEQUIPOSGPS.Items.Clear()
    End Sub

    Protected Sub btnAddEditCMDTpoEq_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddEditCMDTpoEq.Click
        pnlInsCMD.Visible = Not pnlInsCMD.Visible
        If Not ddlComandos_de_TipoEquipo.SelectedItem Is Nothing Then
            Dim tblComandos_TipoEquipo As New rastreogps_tipoequipo_comandos(cnn_str.CadenaDeConexion)
            With tblComandos_TipoEquipo
                If ddlComandos_de_TipoEquipo.SelectedValue <> "SIN COMANDOS PARA ESTE TIPO DE EQUIPO." Then
                    If .LoadByPrimaryKey(Convert.ToInt32(ddlComandos_de_TipoEquipo.SelectedValue)) Then
                        txtTDEq_CMD.Text = .s_Comando
                        txtTDEq_DESC.Text = .s_Descripcion
                        If ViewState("EditCMD") Is Nothing Then
                            ViewState.Add("EditCMD", True)
                        End If
                    End If
                End If
            End With
        End If
    End Sub

    Protected Sub btnCancelCMDTpoEq_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnCancelCMDTpoEq.Click
        pnlInsCMD.Visible = False
        If Not ViewState("EditCMD") Is Nothing Then
            ViewState.Remove("EditCMD")
        End If
    End Sub

    Protected Sub btnAddGPSList_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddGPSList.Click
        If Not String.IsNullOrEmpty(ddlEquipoGPS.SelectedValue) And _
        ddlComandos_de_TipoEquipo.SelectedItem.Text <> "SIN COMANDOS PARA ESTE TIPO DE EQUIPO." Or _
        pnlCustomComando.Visible Then
            AddItem_ListaVehiculos(ddlEquipoGPS.SelectedValue)
        End If
    End Sub

    Public Sub AddItem_ListaVehiculos(ByVal valor As String)
        Dim desc As String = String.Empty
        Dim tblPIPI As New rs_select_vehiculo_tipoequipo_equipogps(cnn_str.CadenaDeConexion)
        With tblPIPI
            .Where.Idrastreogps_equipogps.Value = valor
            If .Query.Load() Then
                desc = .Descripcion
            End If
        End With
        If desc <> String.Empty And valor <> String.Empty Then
            Dim Itemcito As New ListItem(desc, valor)
            If listaEQUIPOSGPS.Items.FindByValue(Itemcito.Value) Is Nothing Then
                listaEQUIPOSGPS.Items.Add(Itemcito)
            End If
        End If
    End Sub

    Protected Sub btnNewCMD_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNewCMD.Click
        If Not ViewState("EditCMD") Is Nothing Then
            ViewState.Remove("EditCMD")
        End If
        txtTDEq_CMD.Text = String.Empty
        txtTDEq_DESC.Text = String.Empty
    End Sub

    Protected Sub btnELIMINARLIST_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnELIMINARLIST.Click
        If Not listaEQUIPOSGPS.SelectedItem Is Nothing Then
            listaEQUIPOSGPS.Items.Remove(listaEQUIPOSGPS.SelectedItem)
        End If
    End Sub

    Protected Sub btnCLEARLIST_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCLEARLIST.Click
        listaEQUIPOSGPS.Items.Clear()
    End Sub

    Protected Sub btnCustomComando_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCustomComando.Click
        pnlCustomComando.Visible = Not pnlCustomComando.Visible
        pnlInsCMD.Visible = False
    End Sub

    Protected Sub btnSaveCMDTpoEq_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSaveCMDTpoEq.Click
        If Not ViewState("EditCMD") Is Nothing Then
            SaveComando(ddlTipoEquipo.SelectedValue, ddlComandos_de_TipoEquipo.SelectedValue)
            ViewState.Remove("EditCMD")
        Else
            SaveComando(ddlTipoEquipo.SelectedValue)
        End If
        pnlCustomComando.Visible = False
        pnlInsCMD.Visible = False
    End Sub

    Public Sub SaveComando(ByVal idTipoEquipo As Integer, Optional ByVal idCMD_TipoEquipo As Integer = 0)
        Try
            Dim tblComandos_TipoEquipo As New rastreogps_tipoequipo_comandos(cnn_str.CadenaDeConexion)
            With tblComandos_TipoEquipo
                If Not ViewState("EditCMD") Is Nothing Then
                    If Not .LoadByPrimaryKey(idCMD_TipoEquipo) Then
                        Return
                    End If
                Else
                    .AddNew()
                    .Fech_ins = Now
                    .User_ins = miUsuario.idUsuario
                    .Id_tipoequipo = idTipoEquipo
                End If
                .Descripcion = txtTDEq_DESC.Text.Trim.ToUpperInvariant()
                .Comando = txtTDEq_CMD.Text.Trim().ToUpperInvariant()
                .Save()
                BindCMD_De_Tipo_Equipo(idTipoEquipo)
            End With
        Catch ex As Exception
            Session.Add("msjerror", ex.ToString())
        End Try
    End Sub

    Protected Sub btnDELComandos_de_TipoEquipo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDELComandos_de_TipoEquipo.Click
        Dim tblComandos_TipoEquipo As New rastreogps_tipoequipo_comandos(cnn_str.CadenaDeConexion)
        With tblComandos_TipoEquipo
            If ddlComandos_de_TipoEquipo.SelectedValue <> "SIN COMANDOS PARA ESTE TIPO DE EQUIPO." Then
                If .LoadByPrimaryKey(Convert.ToInt32(ddlComandos_de_TipoEquipo.SelectedValue)) Then
                    .MarkAsDeleted()
                    .Save()
                End If
            End If
        End With
        BindCMD_De_Tipo_Equipo(ddlTipoEquipo.SelectedValue)
    End Sub

    Protected Sub btnIngresarCMD_encola_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnIngresarCMD_encola.Click
        Try
            If pnlCustomComando.Visible Then
                If listaEQUIPOSGPS.Items.Count > 0 Then
                    For Each Itmz As ListItem In listaEQUIPOSGPS.Items
                        Dim tblColCMD As New rastreogps_cola_de_comandos(cnn_str.CadenaDeConexion)
                        With tblColCMD
                            .AddNew()
                            .User_ins = miUsuario.idUsuario
                            .Fech_ins = Now
                            .Idequipogps = Itmz.Value
                            Dim Comando_a_Enviar As String = txtCustomComando.Text
                            'ddlEquipoGPS.SelectedValue
                            Dim equiposgps As New rastreogps_equipogps(cnn_str.CadenaDeConexion)
                            equiposgps.LoadByPrimaryKey(ddlEquipoGPS.SelectedValue)
                            Dim idEquipo As String = equiposgps.s_Id_equipo_gps
                            If TipoDeReporte.Value = "RGP" Then
                                Comando_a_Enviar = ">" & Comando_a_Enviar
                                Comando_a_Enviar &= ";ID=" & idEquipo.Trim()
                                Comando_a_Enviar &= ";" & Utilidades.NumeralNumber() & ";*"
                                Comando_a_Enviar &= Utilidades.GetCheckSum(Comando_a_Enviar) & "<"
                                Comando_a_Enviar = Comando_a_Enviar.Trim().ToUpperInvariant()
                            End If
                            Comando_a_Enviar = Comando_a_Enviar.Trim().ToUpperInvariant()
                            .Comando = Comando_a_Enviar
                            .Descripcion = "Comando enviado por " + miUsuario.NombreUsuario
                            .Enviado = False
                            .Save()
                        End With
                        sqldsComandosEnCola.DataBind()
                        gv_ComandosEnCola.DataBind()
                    Next
                End If
            Else
                If listaEQUIPOSGPS.Items.Count > 0 Then
                    Dim Comando_a_Enviar As String = ddlComandos_de_TipoEquipo.SelectedValue
                    If Not ddlComandos_de_TipoEquipo.SelectedItem Is Nothing Then
                        Dim tblComandos_TipoEquipo As New rastreogps_tipoequipo_comandos(cnn_str.CadenaDeConexion)
                        With tblComandos_TipoEquipo
                            If ddlComandos_de_TipoEquipo.SelectedValue <> "SIN COMANDOS PARA ESTE TIPO DE EQUIPO." Then
                                If .LoadByPrimaryKey(Convert.ToInt32(ddlComandos_de_TipoEquipo.SelectedValue)) Then
                                    Comando_a_Enviar = .s_Comando
                                End If
                            Else
                                Session.Add("msjerror", "Debe agregar un comando, o utilizar un comando custom.")
                                Return
                            End If
                        End With
                    End If
                    For Each Itmz As ListItem In listaEQUIPOSGPS.Items
                        Dim tblColCMD As New rastreogps_cola_de_comandos(cnn_str.CadenaDeConexion)
                        With tblColCMD
                            .AddNew()
                            .User_ins = miUsuario.idUsuario
                            .Fech_ins = Now
                            .Idequipogps = Itmz.Value
                            Dim PokerFace As String = Comando_a_Enviar
                            Dim idEquipo As String = String.Empty
                            'ddlEquipoGPS.SelectedValue
                            Dim equiposgps As New rastreogps_equipogps(cnn_str.CadenaDeConexion)
                            equiposgps.LoadByPrimaryKey(ddlEquipoGPS.SelectedValue)
                            idEquipo = equiposgps.s_Id_equipo_gps
                            If TipoDeReporte.Value = "RGP" Then
                                PokerFace = ">" & PokerFace
                                PokerFace &= ";ID=" & idEquipo.Trim()
                                PokerFace &= ";" & Utilidades.NumeralNumber() & ";*"
                                PokerFace &= Utilidades.GetCheckSum(PokerFace) & "<"
                                PokerFace = PokerFace.Trim().ToUpperInvariant()
                            End If
                            PokerFace = PokerFace.Trim().ToUpperInvariant()
                            .Comando = PokerFace
                            .Descripcion = ddlComandos_de_TipoEquipo.SelectedItem.Text
                            .Enviado = False
                            .Save()
                        End With
                        sqldsComandosEnCola.DataBind()
                        gv_ComandosEnCola.DataBind()
                    Next
                End If
            End If
        Catch ex As Exception
            Session.Add("msjerror", ex.ToString())
        End Try
    End Sub

    Protected Sub gv_ComandosEnCola_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_ComandosEnCola.RowCommand
        If e.CommandName = "Eliminar" Then
            Dim tblcola As New rastreogps_cola_de_comandos(cnn_str.CadenaDeConexion)
            With tblcola
                If .LoadByPrimaryKey(e.CommandArgument) Then
                    .MarkAsDeleted()
                    .Save()
                End If
            End With
            sqldsComandosEnCola.DataBind()
            gv_ComandosEnCola.DataBind()
        End If
    End Sub

    Protected Sub btnAddAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddAll.Click
        If ddlEquipoGPS.Items.Count > 0 Then
            listaEQUIPOSGPS.Items.Clear()
            For Each equipo As ListItem In ddlEquipoGPS.Items
                listaEQUIPOSGPS.Items.Add(equipo)
            Next
        End If
    End Sub

    Protected Sub btnActualizaLista_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnActualizaLista.Click
        sqldsComandosEnCola.DataBind()
        gv_ComandosEnCola.DataBind()
    End Sub

    Protected Sub ddlComandos_de_TipoEquipo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlComandos_de_TipoEquipo.SelectedIndexChanged
        MensajeEnvio.Value = ddlComandos_de_TipoEquipo.SelectedItem.Text
    End Sub
End Class
