Imports MyGeneration.dOOdads
Imports RASTREOmw

Partial Class Rastreo_Admin_Default
    Inherits System.Web.UI.Page

    Public mi_usuario As New Usuario()

    Private permisoAdd As Boolean = False
    Private permisoMod As Boolean = False
    Private permisoQry As Boolean = False
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then

                If Not Request.UrlReferrer Is Nothing And ViewState("GO_BACK_URL") Is Nothing Then
                    ViewState.Add("GO_BACK_URL", Request.UrlReferrer.ToString)
                End If
                BindTipoEquipoGPS()
            Else
                ':D
            End If

        Catch myEX As Exception
            Session.Add("msjerror", myEX.Message + "<br>" + myEX.StackTrace)
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
        Utilidades.fillDDL(ddlTipoDeEQUIPOGPS, New rastreogps_tipoequipo(cnn_str.CadenaDeConexion), _
                           rastreogps_tipoequipo.ColumnNames.Tipo_equipo, _
                           rastreogps_tipoequipo.ColumnNames.Idrastreogps_tipoequipo)
    End Sub

    Protected Sub lnktipodeequipogps_nueva_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkTipoDeEquipoGPS_nueva.Click
        panel_TipoDeEquipoGPSs_newedit.Visible = True
        panel_TipoDeEquipoGPS_select.Visible = False
        txtTipoDeEquipoGPS_Add.Text = String.Empty
        txtTipoReporte_Add.Text = String.Empty
        txtTipoDeEquipoGPS_Add.Focus()
    End Sub

    Protected Sub lnktipodeequipogps_edit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkTipoDeEquipoGPS_edit.Click
        If ddlTipoDeEquipoGPS.Items.Count > 0 Then
            panel_TipoDeEquipoGPSs_newedit.Visible = True
            panel_TipoDeEquipoGPS_select.Visible = False
            ViewState.Add("Edittipodeequipogps", True)
            txtTipoDeEquipoGPS_Add.Text = ddlTipoDeEquipoGPS.SelectedItem.Text.Trim
            Dim tblTipoEquipo As New rastreogps_tipoequipo(cnn_str.CadenaDeConexion)
            tblTipoEquipo.LoadByPrimaryKey(ddlTipoDeEquipoGPS.SelectedValue)
            txtTipoReporte_Add.Text = tblTipoEquipo.Tipo_de_reporte
            txtTipoDeEquipoGPS_Add.Focus()
        End If
    End Sub

    Protected Sub imgbtn_tipodeequipogpssave_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbtn_TipoDeEquipoGPSsave.Click
        Dim tbltipodeequipogps As New rastreogps_tipoequipo(cnn_str.CadenaDeConexion)
        With tbltipodeequipogps
            If ViewState("Edittipodeequipogps") Then
                .Where.Idrastreogps_tipoequipo.Value = ddlTipoDeEquipoGPS.SelectedValue
                If Not .Query.Load() Then
                    Session.Add("msjerror", "NOSE QUE PASO MEN - TipoEquipoGPS")
                    Exit Sub
                End If
                .Fech_upd = Now
                .User_upd = mi_usuario.idUsuario
            Else
                .Where.Tipo_equipo.Value = txtTipoDeEquipoGPS_Add.Text.Trim.ToUpperInvariant()
                If .Query.Load() Then
                    If .RowCount > 0 Then
                        Session.Add("msjerror", "Ese Tipo de equipo ya existe.")
                        Exit Sub
                    End If
                End If
                .AddNew()
                .Fech_ins = Now
                .User_ins = mi_usuario.idUsuario
            End If
            .s_Tipo_equipo = txtTipoDeEquipoGPS_Add.Text.Trim.ToUpperInvariant()
            .s_Tipo_de_reporte = txtTipoReporte_Add.Text.Trim.ToUpperInvariant()
            .Save()
            If ViewState("Edittipodeequipogps") Then
                Session.Add("msjerror", "Tipo de Equipo GPS modificado con exito.")
            Else
                Session.Add("msjerror", "Tipo de Equipo GPS registrado.")
            End If
            BindTipoEquipoGPS()
            panel_TipoDeEquipoGPS_select.Visible = True
            panel_TipoDeEquipoGPSs_newedit.Visible = False
            txtTipoDeEquipoGPS_Add.Text = String.Empty
            txtTipoReporte_Add.Text = String.Empty
        End With
    End Sub

    Protected Sub imgbtn_tipodeequipogpscancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbtn_TipoDeEquipoGPScancel.Click
        panel_TipoDeEquipoGPS_select.Visible = True
        panel_TipoDeEquipoGPSs_newedit.Visible = False
        txtTipoDeEquipoGPS_Add.Text = String.Empty
        txtTipoReporte_Add.Text = String.Empty
    End Sub
End Class
