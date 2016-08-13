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

Partial Class Rastreo_admin_clientes_documento
    Inherits System.Web.UI.Page
    Public tmpimgs As New List(Of String)
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
        permisoAdd = Utilidades.FindPermissionFor(mi_usuario.idUsuario, "DOCUMENTOSPERSONA_ADMIN-A")
        permisoDel = Utilidades.FindPermissionFor(mi_usuario.idUsuario, "DOCUMENTOSPERSONA_ADMIN-B")
        permisoMod = Utilidades.FindPermissionFor(mi_usuario.idUsuario, "DOCUMENTOSPERSONA_ADMIN-M")
        permisoQry = Utilidades.FindPermissionFor(mi_usuario.idUsuario, "DOCUMENTOSPERSONA_ADMIN-C")
        'Boton de "NUEVO"
        btnAdddocumento.Visible = permisoAdd
        'Esto solo funciona si la grilla siempre tiene "Editar" en penultimo lugar.
        gv_Lista_Vehiculos_Documentos.Columns(gv_Lista_Vehiculos_Documentos.Columns.Count - 2).Visible = permisoMod
        'Esto solo funciona si la grilla siempre tiene "Eliminar" al ultimo.
        gv_Lista_Vehiculos_Documentos.Columns(gv_Lista_Vehiculos_Documentos.Columns.Count - 1).Visible = permisoDel
        'Aqui seteamos a NADA el datasource si no tiene permisos para consultar...
        If Not permisoQry Then gv_Lista_Vehiculos_Documentos.Visible = False
        gv_Lista_Vehiculos_Documentos.DataBind()
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Request.QueryString("cid") <> String.Empty Then
            idCliente = CType(Request.QueryString("cid"), Integer)
        Else
            lbCTAG.Visible = False
        End If

        If Not IsPostBack Then
            lblCliente.Text = String.Empty
            If idCliente > 0 Then
                Dim tblInfo As New rsview_cliente_vehiculo_equipogps(cnn_str.CadenaDeConexion)
                tblInfo.Where.Idrastreo_persona.Value = idCliente
                tblInfo.Query.Load()
                If tblInfo.RowCount > 0 Then
                    lblCliente.Text = tblInfo.s_Cliente
                End If
            End If
        End If
    End Sub

    Protected Sub btnAddDocumento_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdddocumento.Click
        Try
            pnlAddDocumento.Visible = Not pnlAddDocumento.Visible
        Catch X As Exception
            Response.Write(X.Message)
        End Try
    End Sub

    Protected Sub gv_Lista_Vehiculos_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gv_Lista_Vehiculos_Documentos.RowDeleting
        Dim Indx As Integer = 0
        Integer.TryParse(e.Values("Idrastreo_cliente_documentos"), Indx)
        Dim FileName As String = String.Empty
        Dim tblDocumento As New rastreo_cliente_documentos(cnn_str.CadenaDeConexion)
        With tblDocumento
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

    Protected Sub btnUpDocumento_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpDocumento.Click
        If txtDocumentoDesc.Text <> String.Empty Then
            If fuAddDocumento.HasFile Then
                Dim tblDocumento As New rastreo_cliente_documentos(cnn_str.CadenaDeConexion)
                With tblDocumento
                    .AddNew()
                    .Descripcion = txtDocumentoDesc.Text.Trim.ToUpperInvariant()
                    .Idcliente = idCliente
                    .Nombre_archivo = fuAddDocumento.FileName
                    .Tipo_archivo = fuAddDocumento.PostedFile.ContentType
                    .Documento = fuAddDocumento.FileBytes
                    .Save()
                End With
                pnlAddDocumento.Visible = False
                sqlds_Lista_Vehiculo_Documentos.DataBind()
                gv_Lista_Vehiculos_Documentos.DataBind()
            End If
        Else
            Session.Add("msjerror", "Ingrese una descripción para la Documento.")
        End If
    End Sub

    Protected Sub DownloadFile(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim tblArchivos As New rastreo_cliente_documentos(cnn_str.CadenaDeConexion)
        With tblArchivos
            '
            .LoadByPrimaryKey(sender.CommandArgument)
            If .RowCount > 0 Then
                Dim doctype As String = .Tipo_archivo
                Dim docname As String = .Nombre_archivo
                '
                Response.Buffer = False
                Response.ClearHeaders()
                Response.ContentType = doctype
                Response.AddHeader("Content-Disposition", "attachment; filename=""" & docname & """")
                '
                'Code for streaming the object while writing
                Const ChunkSize As Integer = 1024
                Dim buffer As Byte() = New Byte(ChunkSize - 1) {}
                Dim binary As Byte() = TryCast(.Documento, Byte())
                Dim ms As New MemoryStream(binary)
                Dim SizeToWrite As Integer = ChunkSize
                Dim i As Integer = 0
                While i < binary.GetUpperBound(0) - 1
                    If Not Response.IsClientConnected Then
                        Return
                    End If
                    If i + ChunkSize >= binary.Length Then
                        SizeToWrite = binary.Length - i
                    End If
                    Dim chunk As Byte() = New Byte(SizeToWrite - 1) {}
                    ms.Read(chunk, 0, SizeToWrite)
                    Response.BinaryWrite(chunk)
                    Response.Flush()
                    i = i + ChunkSize
                End While
            End If
        End With

    End Sub

End Class
