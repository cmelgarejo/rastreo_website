Imports RASTREOmw
Imports MyGeneration.dOOdads

Public Class admingps_opciones_usuario
    Inherits System.Web.UI.Page

    Public mi_usuario As New Usuario()
    Public idUsuario As Integer
    Public idOpcion As Integer

    Private permisoAdd As Boolean = False
    Private permisoMod As Boolean = False
    Private permisoQry As Boolean = False
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Not Session("session_UsuarioRASTREO") Is Nothing Then
            mi_usuario = CType(Session("session_UsuarioRASTREO"), Usuario)
        Else
            Response.Redirect("../Login.aspx?rp=" + Me.Request.Url.AbsoluteUri(), False)
        End If
        permisoAdd = Utilidades.FindPermissionFor(mi_usuario.idUsuario, "OPCIONESUSUARIO_ADMIN-A")
        permisoMod = Utilidades.FindPermissionFor(mi_usuario.idUsuario, "OPCIONESUSUARIO_ADMIN-M")
        permisoQry = Utilidades.FindPermissionFor(mi_usuario.idUsuario, "OPCIONESUSUARIO_ADMIN-C")
        If Not permisoAdd Or Not permisoMod Or Not permisoQry Then
            Response.Redirect("admingps_opciones_usuario_lista.aspx", False)
        End If
    End Sub

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            idUsuario = CType(Request.QueryString("uid"), Integer)
            idOpcion = CType(Request.QueryString("id"), Integer)

            If Not IsPostBack Then
                Utilidades.fillDDL(ddlOPCIONESGPS, _
                                   New rastreogps_opciones_de_pantalla(cnn_str.CadenaDeConexion), _
                                   rastreogps_opciones_de_pantalla.ColumnNames.Opcion_de_pantalla, _
                                   rastreogps_opciones_de_pantalla.ColumnNames.Idrastreogps_opciones_de_pantalla)
                Dim t As New rastreogps_opciones_de_pantalla(cnn_str.CadenaDeConexion)
                With t
                    .LoadAll()
                    chkOpciones.DataTextField = rastreogps_opciones_de_pantalla.ColumnNames.Opcion_de_pantalla
                    chkOpciones.DataValueField = rastreogps_opciones_de_pantalla.ColumnNames.Idrastreogps_opciones_de_pantalla
                    chkOpciones.DataSource = .DefaultView
                    'chkOpciones.
                    chkOpciones.DataBind()

                End With
                ddlOPCIONESGPS.Focus()

                If Not Request.UrlReferrer Is Nothing And ViewState("GO_BACK_URL") Is Nothing Then
                    ViewState.Add("GO_BACK_URL", Request.UrlReferrer.ToString)
                End If
                If idUsuario > 0 And idOpcion > 0 Then
                    ViewState.Add("admingps_opciones_usuario_update", True)
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

    Private Sub CargarDatos()
        Try
            Dim tblOpciones As New rastreogps_usuario_opciones(cnn_str.CadenaDeConexion)
            With tblOpciones
                .LoadByPrimaryKey(idOpcion, idUsuario)

                ddlOPCIONESGPS.SelectedItem.Selected = False
                ddlOPCIONESGPS.SelectedValue = .Opcion_de_pantalla

            End With
        Catch myEX As Exception
            ViewState.Add("msjerror", myEX.Message + "<br>" + myEX.StackTrace)
        End Try
    End Sub

    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        Try
            Dim mensaje As String = String.Empty
            mensaje = ViewState("msjerror")
            If mensaje = String.empty Then
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

    Private Function Guardar_Datos() As Boolean
        Dim tblUsuarioOpciones As New rastreogps_usuario_opciones(cnn_str.CadenaDeConexion)
        'Dim Transaccion As TransactionMgr = TransactionMgr.ThreadTransactionMgr
        Dim SelVal As Integer = 0
        Try
            Guardar_Datos = False
            'Transaccion.BeginTransaction()
            'SelVal = ddlOPCIONESGPS.SelectedValue

            For Each item As ListItem In chkOpciones.Items
                If item.Selected Then
                    With tblUsuarioOpciones
                        SelVal = Convert.ToInt32(item.Value)
                        Try
                            .LoadByPrimaryKey(SelVal, idUsuario)
                            .FlushData()
                            .AddNew()
                            .Id_usuario = idUsuario
                            .Fech_ins = Now
                            .User_ins = mi_usuario.idUsuario
                            .Opcion_de_pantalla = SelVal
                            .Save()
                        Catch ex As Exception

                        End Try
                    End With
                End If

            Next

            'Transaccion.CommitTransaction()

            If Not ViewState("admingps_opciones_usuario_update") Then
                Session.Add("msjerror", "Opción registrada exitosamente, puede seguir agregando opciones.")
            Else
                Session.Add("msjerror", "Opción actualizada exitosamente!")
            End If

            Response.Redirect("admingps_opciones_usuario_lista.aspx?uid="+idUsuario.ToString, False)

            Guardar_Datos = True
        Catch myEX As Exception
            Guardar_Datos = False
            If Not myEX.Message.Contains("msjerror") Then ViewState.Add("msjerror", myEX.Message + "<br>" + myEX.StackTrace)
            'If Not Transaccion.HasBeenRolledBack Then Transaccion.RollbackTransaction()
        End Try
    End Function

    Protected Sub btnGUARDAR_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGUARDAR.Click
        If Guardar_Datos() Then
            If Not ViewState("admingps_opciones_usuario_update") Then

            End If
        End If
    End Sub

    Protected Sub btnCANCELAR_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCANCELAR.Click
        If ViewState("GO_BACK_URL") <> String.Empty Then
            Response.Redirect(ViewState("GO_BACK_URL"), False)
        Else
            Response.Redirect("admingps_opciones_usuario_lista.aspx", False)
        End If
    End Sub
End Class
