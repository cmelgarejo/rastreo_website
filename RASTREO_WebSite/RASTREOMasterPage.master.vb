Imports RASTREOmw
Imports System.Data
Imports Npgsql

Partial Class RASTREOMasterPage
    Inherits System.Web.UI.MasterPage

    Public miUsuario As Usuario
    Public mi_usuario As String = String.Empty
    Public permiso_menu_admin As Boolean = False
    Public permiso_menu_usuarios As Boolean = False
    Public conteo As Integer

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Session("LANG") IsNot Nothing Then
            ddlLang.SelectedValue = Session("LANG")
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        miUsuario = CType(Session("session_UsuarioRASTREO"), Usuario)
        If Not miUsuario Is Nothing Then
            mi_usuario = CType(Session("session_UsuarioRASTREO"), Usuario).Nombre
            If miUsuario.idUsuario = 0 Or Utilidades.FindPermissionFor(miUsuario.idUsuario, "MENU_ADMINISTRADOR") Then
                permiso_menu_admin = True
                Load_Clientes_vehiculos()
            ElseIf miUsuario.idUsuario = 0 Or Utilidades.FindPermissionFor(miUsuario.idUsuario, "MENU_USUARIOS") Then
                permiso_menu_usuarios = True
            End If
        End If
    End Sub

    Private Sub Load_Clientes_vehiculos()
        Try
            Dim pg_Conn As NpgsqlConnection = New NpgsqlConnection(cnn_str.CadenaDeConexion)
            pg_Conn.Open()
            Dim count_Query As String = _
                "SELECT count(*) as conteo_clientes FROM rastreo_cliente WHERE estado_cliente = TRUE"
            Dim pg_SelectCMD As NpgsqlCommand = New NpgsqlCommand(count_Query, pg_Conn)
            pg_SelectCMD.CommandTimeout = 120
            Dim pg_RDR As NpgsqlDataReader = pg_SelectCMD.ExecuteReader(CommandBehavior.SingleRow)
            If pg_RDR.HasRows Then
                pg_RDR.Read()
                If Not lbCantidadClientes Is Nothing Then
                    lbCantidadClientes.Text = String.Empty + Convert.ToString(pg_RDR("conteo_clientes"))
                End If
            End If
            pg_RDR.Close()
            count_Query = _
                "SELECT count(*) as conteo_vehiculos FROM rastreo_vehiculo WHERE activo = TRUE and demo = FALSE"
            pg_SelectCMD.CommandText = count_Query
            pg_RDR = pg_SelectCMD.ExecuteReader(CommandBehavior.SingleRow)
            If pg_RDR.HasRows Then
                pg_RDR.Read()
                If Not lbCantidadVehiculos Is Nothing Then
                    If (Integer.TryParse(Convert.ToString(pg_RDR("conteo_vehiculos")), conteo)) Then
                        If conteo >= 1000 Then
                            lbCantidadVehiculos.ForeColor = Drawing.Color.Red
                            lbCantidadVehiculos.BorderWidth = Unit.Pixel(3)
                            lbCantidadVehiculos.BorderColor = Drawing.Color.Red
                        End If
                        lbCantidadVehiculos.Text = String.Empty + Convert.ToString(pg_RDR("conteo_vehiculos"))
                    End If
                End If
            End If
            pg_RDR.Close()
            pg_RDR.Close()
            count_Query = _
                "SELECT count(*) as conteo_vehiculos_demo FROM rastreo_vehiculo WHERE activo = TRUE and demo = TRUE"
            pg_SelectCMD.CommandText = count_Query
            pg_RDR = pg_SelectCMD.ExecuteReader(CommandBehavior.SingleRow)
            If pg_RDR.HasRows Then
                pg_RDR.Read()
                If Not lbCantidadVehiculos Is Nothing Then
                    lbCantidadVehiculosDEMO.Text = String.Empty + Convert.ToString(pg_RDR("conteo_vehiculos_demo"))
                End If
            End If
            pg_Conn.Close()
            pg_SelectCMD.Dispose()
            pg_Conn.Dispose()
        Catch ex As Exception
            Session.Add("msjerror", ex.ToString())
        Finally
            'GC.Collect()
        End Try
    End Sub

    Private Sub LogOUT()
        Session.Abandon()
        Response.Redirect("Login.aspx?rp=" + Me.Request.Url.AbsoluteUri(), False)
    End Sub

    Protected Sub RASTREO_LOGOUT_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RASTREO_LOGOUT.Click
        LogOUT()
    End Sub

    Protected Sub ddlLang_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlLang.SelectedIndexChanged
        If Not (Session.Item("LANG") Is Nothing) Then
            Session("LANG") = ddlLang.SelectedValue
        Else
            Session.Add("LANG", ddlLang.SelectedValue)
        End If
    End Sub
End Class

