Imports RASTREOmw
Imports MyGeneration.dOOdads
Imports System.IO
Imports Npgsql
Imports NpgsqlTypes
Imports System.Collections.Generic
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Drawing.Text
Imports System.Drawing.Drawing2D

Partial Class Rastreo_admin_vehiculo_fotos
    Inherits System.Web.UI.Page
    Public tmpimgs As New List(Of String)
    Dim idVehiculo As Integer = 0
    Dim idCliente As Integer = 0
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
        permisoAdd = Utilidades.FindPermissionFor(mi_usuario.idUsuario, "FOTOSVEHICULO_ADMIN-A")
        permisoDel = Utilidades.FindPermissionFor(mi_usuario.idUsuario, "FOTOSVEHICULO_ADMIN-B")
        permisoMod = Utilidades.FindPermissionFor(mi_usuario.idUsuario, "FOTOSVEHICULO_ADMIN-M")
        permisoQry = Utilidades.FindPermissionFor(mi_usuario.idUsuario, "FOTOSVEHICULO_ADMIN-C")
        'Boton de "NUEVO"
        btnAddFotos.Visible = permisoAdd
        'Esto solo funciona si la grilla siempre tiene "Editar" en penultimo lugar.
        gv_Lista_Vehiculos_Fotos.Columns(gv_Lista_Vehiculos_Fotos.Columns.Count - 2).Visible = permisoMod
        'Esto solo funciona si la grilla siempre tiene "Eliminar" al ultimo.
        gv_Lista_Vehiculos_Fotos.Columns(gv_Lista_Vehiculos_Fotos.Columns.Count - 1).Visible = permisoDel
        'Aqui seteamos a NADA el datasource si no tiene permisos para consultar...
        If Not permisoQry Then gv_Lista_Vehiculos_Fotos.Visible = False
        gv_Lista_Vehiculos_Fotos.DataBind()
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Request.QueryString("vid") <> String.Empty And Request.QueryString("cid") <> String.Empty Then
            idVehiculo = CType(Request.QueryString("vid"), Integer)
            idCliente = CType(Request.QueryString("cid"), Integer)
        Else
            btnAddFotos.Visible = False
            lbCTAG.Visible = False
            lbVTAG.Visible = False
        End If

        If Not IsPostBack Then
            lblCliente.Text = String.Empty
            lblVehiculo.Text = String.Empty
            If idVehiculo > 0 And idCliente > 0 Then
                Dim tblvehiculo As New rastreo_vehiculo(cnn_str.CadenaDeConexion)
                With tblvehiculo
                    .LoadByPrimaryKey(idVehiculo, idCliente)
                    Dim tblInfo As New rsview_cliente_vehiculo_equipogps(cnn_str.CadenaDeConexion)
                    tblInfo.Where.Idrastreo_vehiculo.Value = idVehiculo
                    tblInfo.Where.Idrastreo_persona.Value = idCliente
                    tblInfo.Query.Load()
                    If tblInfo.RowCount > 0 Then
                        lblVehiculo.Text = tblInfo.s_Identificador_rastreo + " - " + tblInfo.s_Marca + ", " + _
                                            tblInfo.s_Modelo + " - " + tblInfo.s_Matricula + " - " + tblInfo.s_Anho
                        lblCliente.Text = tblInfo.s_Cliente
                    End If
                End With
            End If
        End If
    End Sub

    Protected Sub btnAddFotos_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddFotos.Click
        Try
            pnlAddFoto.Visible = Not pnlAddFoto.Visible
        Catch X As Exception
            Response.Write(X.Message)
        End Try
    End Sub

    Protected Sub gv_Lista_Vehiculos_Fotos_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_Lista_Vehiculos_Fotos.RowDataBound
        Dim indice As Integer
        Integer.TryParse(e.Row.Cells(0).Text, indice)
        Dim tblfoto As New rastreo_vehiculo_fotos(cnn_str.CadenaDeConexion)
        With tblfoto
            .LoadByPrimaryKey(indice)
            If .RowCount > 0 Then
                If .Tipo_archivo.Contains("image") Then
                    Dim filetype As String = .Tipo_archivo.Split("/")(1)
                    Dim memStream As New MemoryStream(.Foto)
                    Dim filename As String = indice.ToString() + "_" + .Nombre_archivo
                    Dim bp As Bitmap = New Bitmap(memStream)
                    Dim ft As ImageFormat = ImageFormat.Jpeg
                    Select Case filetype
                        Case "jpg"
                        Case "jpeg"
                            ft = ImageFormat.Jpeg
                            Exit Select
                        Case "gif"
                            ft = ImageFormat.Gif
                            Exit Select
                        Case "png"
                            ft = ImageFormat.Png
                            Exit Select
                        Case Else
                            ft = ImageFormat.Bmp
                    End Select
                    bp.Save(Server.MapPath(filename), ft)
                    tmpimgs.Add(filename)
                End If
            End If
        End With

    End Sub

    Protected Sub gv_Lista_Vehiculos_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gv_Lista_Vehiculos_Fotos.RowDeleting
        Dim Indx As Integer = 0
        Integer.TryParse(e.Values("idrastreo_vehiculo_fotos"), Indx)
        Dim FileName As String = String.Empty
        Dim tblFotos As New rastreo_vehiculo_fotos(cnn_str.CadenaDeConexion)
        With tblFotos
            .LoadByPrimaryKey(Indx)
            If .RowCount Then
                FileName = .Nombre_archivo
                .MarkAsDeleted()
                .Save()
            End If
        End With
        Dim fname As String = Server.MapPath(Indx.ToString() + "_" + FileName)
        If File.Exists(fname) Then File.Delete(fname)
        Response.Redirect(Request.Url.ToString)
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

    Protected Sub btnUpfoto_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpfoto.Click
        If txtFotoDesc.Text <> String.Empty Then
            If fuAddFoto.HasFile Then
                Dim tblFoto As New rastreo_vehiculo_fotos(cnn_str.CadenaDeConexion)
                With tblFoto
                    .AddNew()
                    .Descripcion_foto = txtFotoDesc.Text.Trim
                    .Idvehiculo = idVehiculo
                    .Idcliente = idCliente
                    .Nombre_archivo = fuAddFoto.FileName
                    .Tipo_archivo = fuAddFoto.PostedFile.ContentType
                    .Foto = fuAddFoto.FileBytes
                    .Save()
                End With
                pnlAddFoto.Visible = False
                sqlds_Lista_Vehiculo_Fotos.DataBind()
                gv_Lista_Vehiculos_Fotos.DataBind()
            End If
        Else
            Session.Add("msjerror", "Ingrese una descripción para la foto.")
        End If
    End Sub

End Class
