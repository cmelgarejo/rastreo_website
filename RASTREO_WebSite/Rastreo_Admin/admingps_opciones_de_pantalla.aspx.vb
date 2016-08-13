Imports RASTREOmw
Imports MyGeneration.dOOdads

Public Class admingps_opciones_de_pantalla
    Inherits System.Web.UI.Page

    Public mi_usuario As New Usuario()
    Public idrastreogps_opciones_de_pantalla As Integer

    Private permisoAdd As Boolean = False
    Private permisoMod As Boolean = False
    Private permisoQry As Boolean = False

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Not Session("session_UsuarioRASTREO") Is Nothing Then
            mi_usuario = CType(Session("session_UsuarioRASTREO"), Usuario)
        Else
            Response.Redirect("../Login.aspx?rp=" + Me.Request.Url.AbsoluteUri(), False)
        End If
        permisoAdd = Utilidades.FindPermissionFor(mi_usuario.idUsuario, "OPCIONESPANTALLA_ADMIN-A")
        permisoMod = Utilidades.FindPermissionFor(mi_usuario.idUsuario, "OPCIONESPANTALLA_ADMIN-M")
        permisoQry = Utilidades.FindPermissionFor(mi_usuario.idUsuario, "OPCIONESPANTALLA_ADMIN-C")
        If Not permisoAdd Or Not permisoMod Or Not permisoQry Then
            Response.Redirect("admingps_opciones_de_pantalla_lista.aspx", False)
        End If
    End Sub

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            idrastreogps_opciones_de_pantalla = CType(Request.QueryString("id"), Integer)

            If Not IsPostBack Then
                txtOPCIONESGPS.Focus()

                If Not Request.UrlReferrer Is Nothing And ViewState("GO_BACK_URL") Is Nothing Then
                    ViewState.Add("GO_BACK_URL", Request.UrlReferrer.ToString)
                End If
                If idrastreogps_opciones_de_pantalla > 0 Then
                    ViewState.Add("admingps_opciones_de_pantalla_update", True)
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
            Dim tblOpciones As New rastreogps_opciones_de_pantalla(cnn_str.CadenaDeConexion)
            With tblOpciones
                .LoadByPrimaryKey(idrastreogps_opciones_de_pantalla)

                txtOPCIONESGPS.Text = .Opcion_de_pantalla

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
        Dim tblOpciones As New rastreogps_opciones_de_pantalla(cnn_str.CadenaDeConexion)
        Dim Transaccion As TransactionMgr = TransactionMgr.ThreadTransactionMgr
        Try
            Guardar_Datos = False
            Transaccion.BeginTransaction()

            With tblOpciones

                If ViewState("admingps_opciones_de_pantalla_update") Then
                    .LoadByPrimaryKey(idrastreogps_opciones_de_pantalla)
                    .Fech_upd = Now
                    .User_upd = mi_usuario.idUsuario
                Else
                    .Where.Opcion_de_pantalla.Value = txtOPCIONESGPS.Text.Trim.ToUpperInvariant()
                    If .Query.Load() Then
                        If .RowCount > 0 Then
                            ViewState.Add("msjerror", "Ya existe una opcion de pantalla Info: ID=" + .s_Idrastreogps_opciones_de_pantalla + " Opcion=" + .s_Opcion_de_pantalla)
                            Throw New Exception("msjerror")
                        End If
                    End If
                    .FlushData()

                    .AddNew()
                    .Fech_ins = Now
                    .User_ins = mi_usuario.idUsuario
                End If

                .s_Opcion_de_pantalla = txtOPCIONESGPS.Text.Trim.ToUpperInvariant()
                .Save()
            End With
            Transaccion.CommitTransaction()

            If Not ViewState("admingps_opciones_de_pantalla_update") Then
                Session.Add("msjerror", "Opción actualizada exitosamente!")
            Else
                Session.Add("msjerror", "Opción registrada exitosamente!")
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

    Protected Sub btnCANCELAR_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCANCELAR.Click
        If ViewState("GO_BACK_URL") <> String.Empty Then
            Response.Redirect(ViewState("GO_BACK_URL"), False)
        Else
            Response.Redirect("admingps_opciones_de_pantalla_lista.aspx", False)
        End If
    End Sub
End Class
