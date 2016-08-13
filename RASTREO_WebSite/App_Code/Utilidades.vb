Imports System
Imports System.Diagnostics
Imports System.Collections.Generic
Imports System.Text
Imports System.IO
Imports System.Web.UI
Imports System.Resources
Imports System.Data
Imports System.Configuration
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.HtmlControls

Imports Microsoft.VisualBasic
Imports RASTREOmw
Imports MyGeneration.dOOdads
Imports Npgsql
Imports NpgsqlTypes

Imports System.Drawing.Drawing2D
Imports System.Drawing.Color
Imports System.Drawing.Imaging
Imports System.Drawing
Imports System.Net
Imports System.IO.Compression
Imports System.Net.Mail

Public Class Utilidades

    '''' <summary>
    '''' localhost          = ABQIAAAA_r2VgoolhB6iO9xSBULQFxT2yXp_ZAY8_ufC3CFXhHIE1NvwkxS1gjBoSBKGYRWyikSCEeYHX8hJdQ
    '''' localhost:54321    = ABQIAAAA_r2VgoolhB6iO9xSBULQFxQ9pzGUvE4e-uE6JwppIOBVvhvAmRQysIA9otMz1DX9na4y4LGsV3YqDA
    '''' www.rastreo.com.py = ABQIAAAA_r2VgoolhB6iO9xSBULQFxRdTSOwTklp1zijXobIFghLmD1QXxQLokJLv163qA8gkCWwJOxR5oDSOQ
    '''' </summary>
    '''' <remarks></remarks>
    'Private Shared _GMAPS_API_KEY As String = "ABQIAAAA_r2VgoolhB6iO9xSBULQFxSKuZp3SzIZBr90mN7FkilXSG4VSRRoO9BOIhmhhgkSvwCeYRsokCi_iw"

    'Public Shared Property GMAPS_KEY() As String
    '    Get
    '        Return _GMAPS_API_KEY
    '    End Get
    '    Set(ByVal value As String)
    '        _GMAPS_API_KEY = value
    '    End Set
    'End Property

    Public Shared web_directory_txtimgs As String = "~/img/txt/"


    Private Shared Function HexByteVal(ByVal HC As String) As Long
        HexByteVal = 0
        If Len(HC) <> 2 Then Exit Function
        HexByteVal = HexDigitVal(Left(HC, 1)) * &H10 + HexDigitVal(Right(HC, 1))
    End Function

    Private Shared Function HexDigitVal(ByVal h As String) As Integer
        HexDigitVal = 0
        If (Len(h) > 1) Or (h = "") Then Exit Function
        If Not IsHexDigit(h) Then Exit Function
        Select Case h
            Case "0" : HexDigitVal = 0
            Case "1" : HexDigitVal = 1
            Case "2" : HexDigitVal = 2
            Case "3" : HexDigitVal = 3
            Case "4" : HexDigitVal = 4
            Case "5" : HexDigitVal = 5
            Case "6" : HexDigitVal = 6
            Case "7" : HexDigitVal = 7
            Case "8" : HexDigitVal = 8
            Case "9" : HexDigitVal = 9
            Case "A" : HexDigitVal = 10
            Case "B" : HexDigitVal = 11
            Case "C" : HexDigitVal = 12
            Case "D" : HexDigitVal = 13
            Case "E" : HexDigitVal = 14
            Case "F" : HexDigitVal = 15
        End Select

    End Function

    Private Shared Function IsHexDigit(ByVal STR As String) As Boolean
        Dim S As String
        Dim l As Integer
        Dim B As Boolean
        Dim b2 As Boolean

        IsHexDigit = False
        If STR = "" Then Exit Function
        B = False
        For l = 1 To Len(STR)
            If Not (B) Then
                S = Mid(STR, l, 1)
                b2 = False
                If (Asc(S) >= Asc("0")) And (Asc(S) <= Asc("9")) Then b2 = True
                If (Asc(UCase(S)) >= Asc("A")) And (Asc(UCase(S)) <= Asc("F")) Then b2 = True
                B = Not (b2)
            End If ' if not b
        Next
        IsHexDigit = Not (B)
    End Function

    Private Shared Function HexChar(ByVal n As Integer) As String
        If (Len(Hex(n)) Mod 2 <> 0) Then HexChar = "0" & Hex(n) Else HexChar = Hex(n)
    End Function

    ' convert from decimal to binary
    ' if you pass the Digits argument, the result is truncated to that number of 
    ' digits
    '
    ' you should always specify Digits if passing negative values

    Private Shared Function DecToBin(ByVal value As Long, Optional ByVal digits As Short = -1) As String
        ' convert to base-2
        Dim res As String = Convert.ToString(value, 2)

        ' truncate or extend the number
        If digits > 0 Then
            If digits < res.Length Then
                ' truncate the result
                res = res.Substring(res.Length - digits)
            ElseIf digits > res.Length Then
                ' we must extend the result to the left
                If value >= 0 Then
                    ' extend with zeroes if positive
                    res = res.PadLeft(digits, "0"c)
                Else
                    ' extend with ones if negative
                    res = res.PadLeft(digits, "1"c)
                End If
            End If
        End If
        ' return to caller
        Return res
    End Function

    Public Shared Function GetIO(ByVal IO As String) As String
        GetIO = DecToBin(HexByteVal(IO), 8)
        Return GetIO
    End Function

    Public Shared Function SendMailTo(ByVal from As String, ByVal mailTO As String, ByVal subject As String, _
                                      ByVal Message As String, ByVal SERVER As String, ByVal PORT As Integer, _
                                      ByVal USER As String, ByVal PASSWORD As String, ByVal SSL As Boolean) As Boolean
        Try
            Dim MSG As New MailMessage(from, mailTO)
            MSG.Subject = subject
            MSG.Body = Message
            MSG.IsBodyHtml = True
            'Dim Mailattach As New Attachment(FileNameAttach)
            'MSG.Attachments.Add(Mailattach)
            Dim smtpSender As New SmtpClient(SERVER, PORT)
            smtpSender.Timeout = 360000
            smtpSender.EnableSsl = SSL
            'smtpSender.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpSender.UseDefaultCredentials = False
            smtpSender.Credentials = New System.Net.NetworkCredential(USER, PASSWORD)
            smtpSender.Send(MSG)
            smtpSender = Nothing
            MSG.Dispose()
            'GC.Collect()
            Return True
        Catch Exy As Exception
            Dim err As String = Exy.ToString()
            Return False
        Finally
            'GC.Collect()
        End Try
    End Function

    Public Shared Function SendEmail(ByVal ToAddress As String, ByVal FromAddress As String, ByVal MessageSubject As String, _
                       ByVal MessageBody As String, ByVal SERVER As String, ByVal PORT As Integer, ByVal USER As String, _
                       ByVal PASSWORD As String, ByVal SSL As Boolean) As String

        Dim MessageHead As String = "<html><head>"
        MessageHead = MessageHead & "<style>"
        MessageHead = MessageHead & "body {background-color:#F7F7F7; color:#000; font-family:Nina, Segoe, arial narrow, arial, verdana,sans-serif; font-size:12px;}"
        MessageHead = MessageHead & "</style></head><body>"

        Dim MessageFoot As String = "</body></html>"

        MessageBody = MessageHead & MessageBody & MessageFoot

        Dim ReturnMessage As String = ""
        Dim mm As New MailMessage(FromAddress, ToAddress)
        Dim smtp As New SmtpClient

        mm.Subject = MessageSubject
        mm.Body = MessageBody
        mm.IsBodyHtml = True

        Try
            Dim smtpSender As New SmtpClient(SERVER, PORT)
            smtpSender.Timeout = 360000
            smtpSender.EnableSsl = SSL
            'smtpSender.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpSender.UseDefaultCredentials = False
            smtpSender.Credentials = New System.Net.NetworkCredential(USER, PASSWORD)
            smtpSender.Send(mm)
            smtpSender = Nothing
            mm.Dispose()

            ReturnMessage = "El mensaje para " & ToAddress & " ha sido enviado con éxito."

        Catch ex As Exception
            ReturnMessage = "Lo sentimos, ha ocurido un error al enviar a " & ToAddress & " : " & ex.Message
        Finally
            'GC.Collect()
        End Try

        SendEmail = ReturnMessage

    End Function

    Public Shared Function CompresionASPX(ByRef my_context As HttpContext) As Boolean
        CompresionASPX = False

        Dim my_request As HttpRequest = my_context.Request
        Dim acceptEncoding As String = my_request.Headers("Accept-Encoding")
        Dim response As HttpResponse = my_context.Response
        Try
            If Not String.IsNullOrEmpty(acceptEncoding) Then
                acceptEncoding = acceptEncoding.ToUpperInvariant()
                response.Filter = New GZipStream(my_context.Response.Filter, CompressionMode.Compress)
                If acceptEncoding.Contains("GZIP") Then
                    response.AppendHeader("Content-encoding", "gzip")
                ElseIf acceptEncoding.Contains("DEFLATE") Then
                    response.AppendHeader("Content-encoding", "deflate")
                End If
            End If
            response.Cache.VaryByHeaders("Accept-Encoding") = True
            CompresionASPX = True
        Catch

        Finally
            'GC.Collect()
        End Try

    End Function

    Public Shared Function GetReferenciaCercana(ByVal miUsuario As Usuario, ByVal vlat As Double, ByVal vlon As Double) As String
        Dim ReferenciaCercana As String = "--NINGUNA--"
        If Not miUsuario Is Nothing Then
            Dim tblTempRef As New rastreo_template_hoja_de_ruta(cnn_str.CadenaDeConexion)
            Dim tblTempRefPtos As New rastreo_template_puntos_hoja_de_ruta(cnn_str.CadenaDeConexion)
            With tblTempRef
                .Where.Id_usuarios.Value = miUsuario.idUsuario
                .Query.AddConjunction(WhereParameter.Conj.Or)
                .Query.OpenParenthesis()
                .Where.Id_persona.Value = miUsuario.idPersona
                .Where.Publico.Value = True
                .Query.CloseParenthesis()
                If .Query.Load() Then
                    While Not .EOF
                        tblTempRefPtos.Where.Id_recorridotemplate.Value = .Id_recorridotemplate
                        If tblTempRefPtos.Query.Load() Then
                            While (Not tblTempRefPtos.EOF)
                                Dim dist As Double = Utilidades.Calcular_distancia_entre_dos_puntos_MTS _
                                                        (vlat, vlon, tblTempRefPtos.Lat, tblTempRefPtos.Lon)
                                If Not String.IsNullOrEmpty(tblTempRefPtos.s_Metros) Then
                                    If dist < tblTempRefPtos.Metros Then
                                        ReferenciaCercana = _
                                                tblTempRefPtos.s_Nombre + "<br/><span style=""font-size:xx-small;color:blue;"">" + _
                                                tblTempRefPtos.s_Descripcion + _
                                                "</span>"
                                        Return ReferenciaCercana
                                        Exit While
                                    End If
                                End If
                                tblTempRefPtos.MoveNext()
                            End While
                        End If
                        .MoveNext()
                    End While
                End If
            End With
        End If
        Return ReferenciaCercana
    End Function

    Public Shared Function mapserver_ReverseGeocode(ByVal vlat As String, ByVal vlng As String) As String
        mapserver_ReverseGeocode = "N/A"
        Try
            Dim lat As String = vlat.ToString().Replace(",", ".")
            Dim lng As String = vlng.ToString().Replace(",", ".")
            Dim direccionweb As String = "http://" & Resources.RASTREAR.g_MapServerIP & _
                                      ":" & Resources.RASTREAR.g_MapServerPORT & _
                                      "/machear?POSX=" & lng & "&POSY=" & lat
            Dim webReq As HttpWebRequest = CType(WebRequest.Create(direccionweb), HttpWebRequest)
            webReq.Timeout = 500
            Dim WebResp As HttpWebResponse = DirectCast(webReq.GetResponse(), HttpWebResponse)
            Dim Answer As Stream = WebResp.GetResponseStream()
            Dim _Answer As New StreamReader(Answer, Encoding.UTF7)
            Dim resultado As String = _Answer.ReadToEnd()
            'mapserver_ReverseGeocode = resultado
            If resultado <> "|||||" And resultado.Contains("|") Then
                Dim direccion As String() = resultado.Split(Convert.ToChar("|"))
                If direccion.Length > 0 Then
                    mapserver_ReverseGeocode = _
                           Convert.ToString(IIf(String.IsNullOrEmpty(direccion(4)), "S/N", direccion(4))) & ", " & _
                           Convert.ToString(IIf(String.IsNullOrEmpty(direccion(3)), "S/N", direccion(3))) & ", " & _
                           Convert.ToString(IIf(String.IsNullOrEmpty(direccion(1)), "S/N", direccion(1)))
                End If
            Else
                'mapserver_ReverseGeocode = geonames_ReverseGeocode(vlat, vlng, True)
            End If
            Return mapserver_ReverseGeocode.ToUpperInvariant()
        Catch
            'mapserver_ReverseGeocode = geonames_ReverseGeocode(vlat, vlng, True)
            Return mapserver_ReverseGeocode.ToUpperInvariant()
        Finally
            'GC.Collect()
        End Try
    End Function

    Public Shared Function google_ReverseGeocode(ByVal vlat As Double, ByVal vlng As Double, ByVal Callback As Boolean) As String
        google_ReverseGeocode = "N/A..."
        Try
            Dim lat As String = Convert.ToString(vlat).Replace(",", ".")
            Dim lng As String = Convert.ToString(vlng).Replace(",", ".")
            Dim webReq As HttpWebRequest = CType( _
                    WebRequest.Create("http://maps.google.com/maps/geo?q=" + lat + "," + lng + "&output=csv&key=" + _
                                        Resources.RASTREAR._GOOGLE_MAPS_APIKEY_),  _
                                        HttpWebRequest)
            webReq.Timeout = 2345
            Dim WebResp As HttpWebResponse = DirectCast(webReq.GetResponse(), HttpWebResponse)
            Dim Answer As Stream = WebResp.GetResponseStream()
            Dim _Answer As New StreamReader(Answer)
            Dim resultado As String = _Answer.ReadToEnd()
            google_ReverseGeocode = resultado
            If resultado.Contains("200,") Then
                resultado = resultado.Substring(resultado.IndexOf(""""), resultado.Length - resultado.IndexOf(""""))
                google_ReverseGeocode = resultado.Replace("""", "")
            ElseIf resultado.Contains("620,") Then
                If Callback Then
                    'google_ReverseGeocode = geonames_ReverseGeocode(vlat, vlng, True)
                Else
                    google_ReverseGeocode = mapserver_ReverseGeocode(vlat, vlng)
                End If
            End If
            Return google_ReverseGeocode.ToUpperInvariant()
        Catch
            'google_ReverseGeocode = geonames_ReverseGeocode(vlat, vlng, True)
            Return google_ReverseGeocode.ToUpperInvariant()
        End Try
    End Function

    Public Shared Function geonames_ReverseGeocode(ByVal vlat As Double, ByVal vlng As Double, ByVal Callback As Boolean) As String
        Dim Pais As String = "-"
        Dim Distrito As String = "-"
        Dim Localidad1 As String = "-"
        Dim Localidad2 As String = "-"
        Dim Direccion As String = "-"
        Dim address As String = String.Empty
        Try
            Dim lat As String = Convert.ToString(vlat).Replace(",", ".")
            Dim lng As String = Convert.ToString(vlng).Replace(",", ".")
            Dim webReq As HttpWebRequest = CType(WebRequest.Create("http://ws.geonames.org/extendedFindNearby?lat=" + lat + "&lng=" + lng), HttpWebRequest)
            'webReq.TransferEncoding = "UTF8"
            webReq.Timeout = 500
            'From here on, it's all the same as above.
            Dim WebResp As HttpWebResponse = DirectCast(webReq.GetResponse(), HttpWebResponse)
            'Let's show some information about the response
            'Now, we read the response (the string), and output it.
            Dim Answer As Stream = WebResp.GetResponseStream()
            Dim _Answer As New StreamReader(Answer)

            'Congratulations, with these two functions in basic form, you just learned
            'the two basic forms of web surfing
            'This proves how easy it can be.
            Dim resultado As String = _Answer.ReadToEnd()

            Dim xml As New System.Xml.XmlDocument()

            xml.LoadXml(resultado)

            Try
                Pais = xml.SelectNodes("/geonames")(0).ChildNodes(2).ChildNodes(0).InnerText
                Distrito = xml.SelectNodes("/geonames")(0).ChildNodes(3).ChildNodes(0).InnerText
                Localidad1 = xml.SelectNodes("/geonames")(0).ChildNodes(4).ChildNodes(0).InnerText
                Localidad2 = xml.SelectNodes("/geonames")(0).ChildNodes(5).ChildNodes(0).InnerText.Replace("Banco San Miguel", "Barrio San Miguel")
            Catch
                address = Pais + ", " + Distrito + ", " + Localidad1 + _
                    ", " + Localidad2 + ", " + Direccion
                Return address.ToUpperInvariant()
            End Try
            address = Pais + ", " + Distrito + ", " + Localidad1 + _
                    ", " + Localidad2 + ", " + Direccion
            Return address.ToUpperInvariant()
        Catch
            If Not Callback Then
                address = mapserver_ReverseGeocode(vlat, vlng)
            Else
                address = Pais + ", " + Distrito + ", " + Localidad1 + _
                        ", " + Localidad2 + ", " + Direccion
            End If
            Return address.ToUpperInvariant()
        End Try
    End Function

    Public Shared Function GetCheckSum(ByVal msg As String) As String
        Dim ChkSum As Integer = 0
        For Each r As Char In msg
            If r = "*"c Then
                Exit For
            Else
                ChkSum = ChkSum Xor Convert.ToInt32(r)
            End If
        Next
        Dim resultado As String = Convert.ToString(ChkSum, 16).ToUpperInvariant()
        If resultado.Length < 2 Then
            Return String.Concat("0", resultado)
        Else
            Return resultado
        End If
    End Function

    Public Shared NumeralCounter As Integer = 7999
    Public Shared Function NumeralNumber() As String
        If NumeralCounter < 9000 Then
            NumeralCounter += 1
        Else
            NumeralNumber = 8000
        End If
        NumeralNumber = "#" + NumeralCounter.ToString()
        Return NumeralNumber
    End Function

    Public Shared Function FindPermissionFor(ByVal mi_user As Integer, ByVal opcion_permiso As String) As Boolean
        'Dim SQLSelect As String = String.Empty
        'Dim rastrear_conexion As New NpgsqlConnection(cnn_str.CadenaDeConexion)
        'rastrear_conexion.Open()

        'Dim rastrear_commando As New NpgsqlCommand(SQLSelect, rastrear_conexion)
        'rastrear_commando.CommandTimeout = 60
        'Dim rastrear_reader As New NpgsqlDataAdapter(rastrear_commando)
        'Dim tUSU As New DataSet()
        'rastrear_reader.Fill(tUSU)

        'With tUSU.Tables(0)

        'End With

        Dim tblUSU As New rastreo_usuarios(cnn_str.CadenaDeConexion)
        With tblUSU
            If .LoadByPrimaryKey(mi_user) Then
                'If .s_Usuario <> String.Empty Then
                If .s_Su <> String.Empty Then
                    If .Su Then
                        Return True
                    End If
                End If
                'End If
            End If
        End With

        Dim tblPermisos As New rastreogps_usuario_opciones(cnn_str.CadenaDeConexion)
        Dim tblOpcion As New rastreogps_opciones_de_pantalla(cnn_str.CadenaDeConexion)
        With tblOpcion
            .Where.Opcion_de_pantalla.Value = opcion_permiso
            If .Query.Load Then
                If .RowCount > 0 Then
                    tblPermisos.Where.Id_usuario.Value = mi_user
                    tblPermisos.Where.Opcion_de_pantalla.Value = .Idrastreogps_opciones_de_pantalla
                    If tblPermisos.Query.Load Then
                        If tblPermisos.RowCount > 0 Then
                            Return True
                        End If
                    End If
                End If
            End If
        End With
        Return False
    End Function

    Public Shared Sub fillDDL(ByRef dropDownList As WebControls.DropDownList, ByVal obj As PostgreSqlEntity, ByVal TextColumnName As String, ByVal ValueColumnName As String)
        Try
            'get data
            With obj
                .Query.AddResultColumn(TextColumnName)
                .Query.AddResultColumn(ValueColumnName)
                .Query.AddOrderBy(TextColumnName, WhereParameter.Dir.ASC)
                .Query.Load()
            End With
            'bind to combo box
            With dropDownList
                .DataSource = obj.DefaultView
                .DataTextField = TextColumnName
                .DataValueField = ValueColumnName
                .DataBind()
            End With
        Catch ex As Exception

        End Try
    End Sub

    Public Shared Sub fillDDL(ByRef dropDownList As WebControls.DropDownList, ByVal obj As PostgreSqlEntity, ByVal TextColumnName As String, ByVal ValueColumnName As String, ByVal ValorFiltro As String, ByVal Operando As WhereParameter.Operand)
        Try
            'get data
            With obj
                .Query.AddResultColumn(TextColumnName)
                .Query.AddResultColumn(ValueColumnName)
                .Query.AddOrderBy(TextColumnName, WhereParameter.Dir.ASC)
                Dim MyWhere As New WhereParameter(TextColumnName, _
                    New NpgsqlParameter("@" + TextColumnName, NpgsqlDbType.Text))
                If Operando = WhereParameter.Operand.Like Or _
                   Operando = WhereParameter.Operand.NotLike Or _
                   Operando = WhereParameter.Operand.ILike Then
                    MyWhere.Value = "%" + ValorFiltro.Trim + "%"
                Else
                    MyWhere.Value = ValorFiltro.Trim
                End If
                MyWhere.Operator = Operando
                .Query.AddWhereParameter(MyWhere)
                .Query.Load()
            End With
            'bind to combo box

            With dropDownList
                .DataSource = obj.DefaultView
                .DataTextField = TextColumnName
                .DataValueField = ValueColumnName
                .DataBind()
            End With
        Catch EX As Exception

        End Try
    End Sub

    Public Shared Function Manejador_de_Excepciones(ByVal myError As Exception) As Boolean
        Try
            Dim eventLogs As String = "RASTREOweb_error"
            Dim eventSource As String = "Error - Sitio Web de RASTREO"
            Dim myErrorMessage As String = String.Empty

            myErrorMessage += "RawData:" & vbCr & vbLf & myError.Message.ToString() & vbCr & vbLf & vbCr & vbLf
            myErrorMessage += "Mensaje:" & vbCr & vbLf & myError.Message.ToString() & vbCr & vbLf & vbCr & vbLf
            myErrorMessage += "Origen:" & vbCr & vbLf & myError.Source & vbCr & vbLf & vbCr & vbLf
            myErrorMessage += "Target site:" & vbCr & vbLf & myError.TargetSite.ToString() & vbCr & vbLf & vbCr & vbLf
            myErrorMessage += "Stack trace:" & vbCr & vbLf & myError.StackTrace & vbCr & vbLf & vbCr & vbLf
            myErrorMessage += ".ToString():" & vbCr & vbLf & vbCr & vbLf & myError.ToString()

            While myError.InnerException IsNot Nothing
                myErrorMessage += "RawData:" & vbCr & vbLf & myError.Message.ToString() & vbCr & vbLf & vbCr & vbLf
                myErrorMessage += "Mensaje:" & vbCr & vbLf & myError.Message.ToString() & vbCr & vbLf & vbCr & vbLf
                myErrorMessage += "Origen:" & vbCr & vbLf & myError.Source & vbCr & vbLf & vbCr & vbLf
                myErrorMessage += "Target site:" & vbCr & vbLf & myError.TargetSite.ToString() & vbCr & vbLf & vbCr & vbLf
                myErrorMessage += "Stack trace:" & vbCr & vbLf & myError.StackTrace & vbCr & vbLf & vbCr & vbLf
                myErrorMessage += ".ToString():" & vbCr & vbLf & vbCr & vbLf & myError.ToString()

                myError = myError.InnerException
            End While

            ' Make sure the Eventlog Exists
            If Not EventLog.SourceExists(eventLogs) Then
                EventLog.CreateEventSource(eventSource, eventLogs)
            End If

            ' Create an EventLog instance and assign its source.
            Dim myLog As New EventLog(eventLogs)
            myLog.Source = eventSource

            ' Write the error entry to the event log.    
            myLog.WriteEntry(("Error ocurrido en : " & eventSource & vbCr & vbLf & vbCr & vbLf) + myErrorMessage, EventLogEntryType.[Error])
            Return True
        Catch ex As Exception
            Console.WriteLine("Mensaje de Error:{0}", ex.ToString())
            Return False
        Finally
            'GC.Collect()
        End Try
    End Function

    Public Shared Function Informaciones_del_sistema(ByVal miMensaje As String, ByVal Tipo_de_Mensaje As EventLogEntryType) As Boolean
        Try
            Dim eventLogs As String = "RASTREOweb_info"
            Dim eventSource As String = Tipo_de_Mensaje.ToString() & " - Sitio Web de RASTREO"
            Dim myErrorMessage As String = miMensaje

            ' Make sure the Eventlog Exists
            If Not EventLog.SourceExists(eventLogs) Then
                EventLog.CreateEventSource(eventSource, eventLogs)
            End If

            Dim myLog As New EventLog(eventLogs)
            myLog.Source = eventSource

            ' Write the error entry to the event log.    
            myLog.WriteEntry(("Informacion proveniente de : " & eventSource & vbCr & vbLf & vbCr & vbLf) + myErrorMessage, Tipo_de_Mensaje)
            Return True
        Catch ex As Exception
            Console.WriteLine("Mensaje de Error:{0}", ex.ToString())
            Return False
        Finally
            'GC.Collect()
        End Try
    End Function

    ''' <summary>
    ''' Convierte un texto definido a imagen para ser utilizado por el sistema
    ''' </summary>
    ''' <param name="Texto"></param>
    ''' <param name="directorio"></param>
    ''' <param name="arch_nombre"></param>
    ''' <param name="Formato"></param>
    ''' <param name="Width"></param>
    ''' <param name="Height"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function Texto_a_Imagen(ByVal Texto As String, ByVal directorio As String, ByVal arch_nombre As String, ByVal Formato As ImageFormat, Optional ByVal forecolor As String = "Black", Optional ByVal backcolor As String = "Trasparent", Optional ByVal Width As Integer = 150, Optional ByVal Height As Integer = 12, Optional ByVal FontSize As Integer = 10) As String
        'Creamos un objeto Bitmap y especificamos las dimensiones ancho y alto, sera como nuestro lienzo
        'Nota.- El color por defecto de un nuevo Bitmap es negro
        arch_nombre += "." + Formato.ToString().ToLower()
        Dim web_filename As String = web_directory_txtimgs.Replace("~", ".") + arch_nombre
        Dim path_completo As String = directorio + "\" + arch_nombre
        If File.Exists(path_completo) Then
            Return web_filename
        End If
        If Not Directory.Exists(directorio) Then
            Directory.CreateDirectory(directorio)
        End If
        Dim objLienzo As Bitmap = New Bitmap(Width, Height)
        Dim objGraficar As Graphics = Graphics.FromImage(objLienzo)
        Dim objFont As New Font(FontFamily.GenericMonospace, FontSize, FontStyle.Bold)
        Dim objPincelFondo As New SolidBrush(Color.FromName(backcolor))
        Dim objPincelTexto As New SolidBrush(Color.FromName(forecolor))
        Try
            'Comenzamos a pintar el texto
            objGraficar.SmoothingMode = SmoothingMode.HighQuality
            objGraficar.FillRectangle(objPincelFondo, 0, 0, Width, Height)
            Dim sFormato As New StringFormat(StringFormatFlags.LineLimit)
            sFormato.LineAlignment = StringAlignment.Center
            objGraficar.DrawString(Texto, objFont, objPincelTexto, 0, 0)
            objLienzo.Save(path_completo, Formato)
            Return web_filename
        Catch ex As Exception
            Return web_filename
        Finally
            'GC.Collect()
        End Try
    End Function

    Public Shared Function Calcular_distancia_entre_dos_puntos_MTS(ByVal lat1 As Double, ByVal lon1 As Double, ByVal lat2 As Double, ByVal lon2 As Double) As Double
        Dim R As Integer = 6371
        ' km
        Dim dLat As Double = (lat2 - lat1) * Math.PI / 180
        Dim dLon As Double = (lon2 - lon1) * Math.PI / 180
        Dim a As Double = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) + Math.Cos(toRad(lat1)) * Math.Cos(toRad(lat2)) * Math.Sin(dLon / 2) * Math.Sin(dLon / 2)
        Dim c As Double = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a))
        Dim d As Double = R * c * 1000
        Return d
    End Function

    Public Shared Function dblCalcular_distancia_entre_dos_puntos_KM(ByVal lat1 As Double, ByVal lon1 As Double, ByVal lat2 As Double, ByVal lon2 As Double) As Double
        Dim R As Integer = 6371
        ' km
        Dim dLat As Double = (lat2 - lat1) * Math.PI / 180
        Dim dLon As Double = (lon2 - lon1) * Math.PI / 180
        Dim a As Double = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) + Math.Cos(toRad(lat1)) * Math.Cos(toRad(lat2)) * Math.Sin(dLon / 2) * Math.Sin(dLon / 2)
        Dim c As Double = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a))
        Dim d As Double = R * c
        Return d
    End Function

    Private Shared Function toRad(ByVal nro As Double) As Double
        Return nro * Math.PI / 180
    End Function

End Class

'''JS Helper by M$ :D
Public Class JavascriptHelper
    Private Const ScriptFormat As String = "<script type='text/javascript' language='javascript'>{0}</script>"

    Public Shared Function [Custom](ByVal script As String, ByVal key As String) As String
        Return [Custom](Nothing, script, key)
    End Function

    Public Shared Function [Custom](ByVal page As Page, ByVal script As String, ByVal key As String) As String
        Dim js As String = String.Format(ScriptFormat, script)

        Register(page, js, key)

        Return js
    End Function

    Public Shared Function Alert(ByVal message As String, ByVal key As String) As String
        Return Alert(Nothing, message, key)
    End Function

    Public Shared Function Alert(ByVal page As Page, ByVal message As String, ByVal key As String) As String
        Return [Custom](page, String.Format("alert('{0}');", message), key)
    End Function

    Private Shared Sub Register(ByVal page As Page, ByVal javascript As String, ByVal key As String)
        If page IsNot Nothing Then
            If Not page.ClientScript.IsClientScriptBlockRegistered(key) Then
                page.ClientScript.RegisterClientScriptBlock(page.[GetType](), key, javascript)
            End If
        End If
    End Sub
End Class

'M$Convertir  DataView a archivo CSV :D

Public Class CSVHelper
    ''' <summary>
    ''' Exports the given DataView to a CSV file
    ''' </summary>
    ''' <param name="page">The page making the request</param>
    ''' <param name="dataView">The DataView to convert</param>
    ''' <param name="columns">The columns of the DataView to export</param>
    ''' <param name="seperator">The string to seperate each value</param>
    ''' <param name="fileName">The default file name for the CSV file</param>
    ''' <param name="includeColumnNames">Indicates if the first line should be the column names</param>
    Public Shared Sub Export(ByVal page As Page, ByVal dataView As DataView, ByVal columns As String(), ByVal seperator As String, ByVal fileName As String, ByVal includeColumnNames As Boolean)
        Export(page, CreateDataTable(dataView, columns), seperator, fileName, includeColumnNames)
    End Sub

    ''' <summary>
    ''' Exports the given DataTable to a CSV file
    ''' </summary>
    ''' <param name="page">The page making the request</param>
    ''' <param name="dataTable">The DataTable to convert</param>
    ''' <param name="seperator">The string to seperate each value</param>
    ''' <param name="fileName">The default file name for the CSV file</param>
    ''' <param name="includeColumnNames">Indicates if the first line should be the column names</param>
    Public Shared Sub Export(ByVal page As Page, ByVal dataTable As DataTable, ByVal seperator As String, ByVal fileName As String, ByVal includeColumnNames As Boolean)
        Dim stringBuilder As New StringBuilder()

        If includeColumnNames Then
            'Write the column names out for the first line
            For i As Integer = 0 To dataTable.Columns.Count - 1
                stringBuilder.Append(dataTable.Columns(i).Caption)

                If Not i.Equals(dataTable.Columns.Count - 1) Then
                    stringBuilder.Append(seperator)
                End If
            Next

            stringBuilder.Append(Environment.NewLine)
        End If

        'Write each of the values for each row
        For Each dataRow As DataRow In dataTable.Rows
            For i As Integer = 0 To dataTable.Columns.Count - 1
                stringBuilder.Append(dataRow(i).ToString())

                If Not i.Equals(dataTable.Columns.Count - 1) Then
                    stringBuilder.Append(seperator)
                End If
            Next

            stringBuilder.Append(Environment.NewLine)
        Next

        page.Response.ClearHeaders()
        page.Response.AppendHeader("Content-Disposition", String.Format("attachment; filename={0}", fileName))
        page.Response.AppendHeader("Content-Length", stringBuilder.Length.ToString())
        page.Response.ContentType = "text/csv"
        page.Response.Write(stringBuilder.ToString())
        page.Response.[End]()
    End Sub

    ''' <summary>
    ''' Creates a DataTable from the selected DataView
    ''' </summary>
    ''' <param name="dataView">The DataView to create the table from</param>
    ''' <param name="columns">The DataColumns to be used in the new DataTable</param>
    Public Shared Function CreateDataTable(ByVal dataView As DataView, ByVal columns As String()) As DataTable
        Dim dataTable As New DataTable()

        Dim dataColumn As DataColumn = Nothing
        Dim newColumn As DataColumn = Nothing

        For Each column As String In columns
            If column IsNot Nothing Then
                dataColumn = dataView.Table.Columns(column)

                newColumn = New DataColumn(dataColumn.ColumnName, dataColumn.DataType)
                newColumn.Caption = dataColumn.Caption
                dataTable.Columns.Add(newColumn)
            End If
        Next


        Dim dataRow As DataRow = Nothing

        For Each dataRowView As DataRowView In dataView
            dataRow = dataTable.NewRow()

            For Each column As String In columns
                If column IsNot Nothing Then
                    dataRow(column) = dataRowView(column)
                End If
            Next

            dataTable.Rows.Add(dataRow)
        Next

        Return dataTable
    End Function

End Class
