Imports System.Xml
Imports System.Text
Imports System.IO
Imports System.Collections.Generic

Partial Class KMLCreator
    Inherits System.Web.UI.Page
    '    '' Cristhian, mi intencion es utilizar el GridView que contiene los datos cuando se despliega el Historico , 
    '    '' yo intento estirar los datos que ya aparecen en esa grilla y utilizar esos datos, por eso meti dentro de un For
    '    '' para recorrer y leer los datos.
    '    'Dim spdini As String = ""
    '    'Dim spdfin As String = ""
    '    'Dim evento As String = "SIN EVENTO"
    '    'Dim lat As String = ""
    '    'Dim lon As String = ""
    '    ''Indexex = 0
    '    ' ''cambiar
    '    'Dim sql_where_reportes As String = String.Empty
    '    'Dim rastrear_conexion As New NpgsqlConnection(cnn_str.CadenaDeConexion)
    '    '    rastrear_conexion.Open()

    '    'Dim sbScript As New StringBuilder()
    '    '    sql_where_reportes = " WHERE Idrastreo_vehiculo = " & idVehiculoSeleccionado.Value & _
    '    '                         " AND Gps_fechahora_reporte BETWEEN '" & Convert.ToDateTime(txtFECHAINICIO.Value).ToString("dd/MM/yyyy HH:mm:ss") & _
    '    '                         "' AND '" & Convert.ToDateTime(txtFECHAFIN.Value).ToString("dd/MM/yyyy HH:mm:ss") & "' "
    '    '    If evento <> "SIN EVENTO" Then
    '    '        sql_where_reportes += " AND Gps_evento = '" & evento & "' "
    '    '    End If
    '    '    If spdini <> "" And spdfin <> "" Then
    '    '        sql_where_reportes += " AND (Gps_velocidad BETWEEN " & spdini & " AND " & spdfin & ") "
    '    '    End If
    '    '    sql_where_reportes = sql_where_reportes + " AND gps_latitud <> 0 AND gps_longitud <> 0 "
    '    'Dim rastrear_commando As New NpgsqlCommand(g_selectcommand + sql_where_reportes, rastrear_conexion)
    '    '    rastrear_commando.CommandTimeout = 120
    '    'Dim rastrear_reader As New NpgsqlDataAdapter(rastrear_commando)
    '    'Dim tRecorrido As New DataSet()
    '    '    rastrear_reader.Fill(tRecorrido)

    '    '    HttpContext.Current.Response.Clear()
    '    '    HttpContext.Current.Response.ContentType() = "application/vnd.google-earth.kml+xml"
    '    ' ''uncomment following line to view raw xml
    '    ' ''My.Response.ContentType = "plain/text"
    '    '    HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8
    '    'Dim stream As New System.IO.MemoryStream
    '    'Dim XMLwrite As New XmlTextWriter(stream, System.Text.Encoding.UTF8)

    '    '    XMLwrite.WriteStartDocument()
    '    '    XMLwrite.WriteWhitespace(Environment.NewLine)
    '    '    XMLwrite.WriteStartElement("kml")
    '    '    XMLwrite.WriteAttributeString("xmlns", "http://earth.google.com/kml/2.0")
    '    '    XMLwrite.WriteWhitespace(Environment.NewLine)

    '    '    With tRecorrido.Tables(0)
    '    '        g_RowCount = .Rows.Count
    '    '        Indexex = 0
    '    'Dim iconoevento As String = "./img/center_normal.gif"
    '    '        If Not .Rows(Indexex).Item("Evento") Is DBNull.Value Then
    '    '            If .Rows(Indexex).Item("Evento").Contains("MARCHA") Or _
    '    '                .Rows(Indexex).Item("Evento").Contains("IDA") Or _
    '    '                .Rows(Indexex).Item("Evento").Contains("VUELTA") Then
    '    ''iconoevento = "./img/center_green.gif"
    '    '                iconoevento = "./img/" & .Rows(Indexex).Item("Rumbo") & ".png"
    '    '            ElseIf .Rows(Indexex).Item("Evento").Contains("APAGADO") Or .Rows(Indexex).Item("Evento").Contains("DETENIDO") Then
    '    '                iconoevento = "./img/stop_red.png"
    '    '            ElseIf .Rows(Indexex).Item("Evento").Contains("DERECHA") Or .Rows(Indexex).Item("Evento").Contains("IZQ") Then
    '    '                If .Rows(Indexex).Item("Rumbo") <> String.Empty Then
    '    '                    If .Rows(Indexex).Item("Evento").Contains("DERECHA") Then
    '    '                        iconoevento = "./img/vder" & Convert.ToString(.Rows(Indexex).Item("Rumbo")).ToLowerInvariant() & ".png"
    '    '                    ElseIf .Rows(Indexex).Item("Evento").Contains("IZQ") Then
    '    '                        iconoevento = "./img/vizq" & Convert.ToString(.Rows(Indexex).Item("Rumbo")).ToLowerInvariant() & ".png"
    '    '                    End If
    '    '                Else
    '    '                    If .Rows(Indexex).Item("Evento").Contains("DERECHA") Then
    '    '                        iconoevento = "./img/vder.png"
    '    '                    ElseIf .Rows(Indexex).Item("Evento").Contains("IZQ") Then
    '    '                        iconoevento = "./img/vizq.png"
    '    '                    End If
    '    '                End If
    '    '            End If
    '    '        Else
    '    '            iconoevento = "./img/stop_red.png"
    '    '        End If
    '    '        While Indexex < g_RowCount
    '    '            lon = Convert.ToString(.Rows(Indexex).Item("Gps_longitud")).Replace(",", ".")
    '    '            lat = Convert.ToString(.Rows(Indexex).Item("Gps_latitud")).Replace(",", ".")
    '    ''posPOINT = tmpLAT + "," + tmpLONG
    '    '            XMLwrite.WriteStartElement("Placemark")
    '    '            XMLwrite.WriteWhitespace(Environment.NewLine)
    '    '            XMLwrite.WriteElementString("Snippet", "KML generated by pedrosilva")
    '    '            XMLwrite.WriteWhitespace(Environment.NewLine)
    '    '            XMLwrite.WriteElementString("description", "") ' aqui quiero que aparezca Direccion Cercana que aparece en gridVehiculoSeleccionado
    '    '            XMLwrite.WriteElementString("name", "") ' aqui quiero que aparezca la fecha y hora de la posicion del movil que aparece en gridVehiculoSeleccionado
    '    '            XMLwrite.WriteWhitespace(Environment.NewLine)
    '    '            XMLwrite.WriteStartElement("LookAt")
    '    '            XMLwrite.WriteElementString("longitude", lon) ' Esta es la parte que mas me cuesta, aqui quiero leer la LONGITUD de donde estuvo el movil  en gridVehiculoSeleccionado
    '    '            XMLwrite.WriteWhitespace(Environment.NewLine)
    '    '            XMLwrite.WriteElementString("latitude", lat) ' Esta es la parte que mas me cuesta, aqui quiero leer la LATITUD de donde estuvo el movil en gridVehiculoSeleccionado
    '    '            XMLwrite.WriteWhitespace(Environment.NewLine)
    '    '            XMLwrite.WriteElementString("range", "300")
    '    '            XMLwrite.WriteWhitespace(Environment.NewLine)
    '    '            XMLwrite.WriteElementString("tilt", "20")
    '    '            XMLwrite.WriteWhitespace(Environment.NewLine)
    '    '            XMLwrite.WriteElementString("heading", "0")
    '    '            XMLwrite.WriteWhitespace(Environment.NewLine)
    '    '            XMLwrite.WriteEndElement()
    '    '            XMLwrite.WriteWhitespace(Environment.NewLine)
    '    '            XMLwrite.WriteElementString("visibility", "1")
    '    '            XMLwrite.WriteWhitespace(Environment.NewLine)
    '    '            XMLwrite.WriteStartElement("Style")
    '    '            XMLwrite.WriteWhitespace(Environment.NewLine)
    '    '            XMLwrite.WriteStartElement("IconStyle")
    '    '            XMLwrite.WriteWhitespace(Environment.NewLine)
    '    '            XMLwrite.WriteStartElement("Icon")
    '    '            XMLwrite.WriteWhitespace(Environment.NewLine)
    '    '            XMLwrite.WriteElementString("href", iconoevento)
    '    '            XMLwrite.WriteWhitespace(Environment.NewLine)
    '    '            XMLwrite.WriteElementString("w", "-1")
    '    '            XMLwrite.WriteWhitespace(Environment.NewLine)
    '    '            XMLwrite.WriteElementString("h", "-1")
    '    '            XMLwrite.WriteWhitespace(Environment.NewLine)
    '    '            XMLwrite.WriteEndElement()
    '    '            XMLwrite.WriteWhitespace(Environment.NewLine)
    '    '            XMLwrite.WriteEndElement()
    '    '            XMLwrite.WriteWhitespace(Environment.NewLine)
    '    '            XMLwrite.WriteEndElement()

    '    '            XMLwrite.WriteStartElement("Point")
    '    '            XMLwrite.WriteWhitespace(Environment.NewLine)
    '    '            XMLwrite.WriteElementString("extrude", "1")
    '    '            XMLwrite.WriteWhitespace(Environment.NewLine)
    '    '            XMLwrite.WriteElementString("altitudeMode", "relativeToGround")
    '    '            XMLwrite.WriteWhitespace(Environment.NewLine)
    '    '            XMLwrite.WriteElementString("coordinates", lon & ", " & lat & ", 0") 'aqui es una consecuencia luego de haber obtenido LATITUD y LONGITUD
    '    '            XMLwrite.WriteWhitespace(Environment.NewLine)
    '    '            XMLwrite.WriteEndElement()
    '    '            XMLwrite.WriteWhitespace(Environment.NewLine)
    '    '            XMLwrite.WriteEndElement()
    '    ''XMLwrite.WriteWhitespace(Environment.NewLine)
    '    ''XMLwrite.WriteEndElement()
    '    ''XMLwrite.WriteWhitespace(Environment.NewLine)
    '    ''XMLwrite.WriteEndDocument()
    '    '            XMLwrite.Flush()
    '    ''lat = .Rows(Indexex).Item("Gps_latitud")
    '    ''lon = .Rows(Indexex).Item("Gps_longitud")
    '    '            Indexex += 1
    '    '        End While

    '    '    End With
    '    'Dim reader As IO.StreamReader
    '    '    stream.Position = 0
    '    '    reader = New IO.StreamReader(stream)
    '    'Dim bytes() As Byte = System.Text.Encoding.UTF8.GetBytes(reader.ReadToEnd())
    '    '    HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=recorrido_" & vehiculo_seleccionado & Now.ToString("_ddMMyyy_HHmm_") & ".kml")
    '    '    HttpContext.Current.Response.BinaryWrite(bytes)
    '    '    HttpContext.Current.Response.End()
    '    Enum KMLAltitudeMode As Byte
    '        Absolute
    '        ClamToGround
    '        RelativeToGround
    '    End Enum
    '    Public Class KMLCoordinates
    '        Dim _Latitude As Double = 0.0
    '        Dim _Longitude As Double = 0.0
    '        Dim _Altitude As Double = 0.0
    '        Public Sub KMLCoordinates()

    '        End Sub
    '        Public Sub KMLCoordinates(ByVal oldlat As Double, ByVal oldlon As Double)
    '            _Longitude = oldlon
    '            _Latitude = oldlat
    '        End Sub
    '        Public Sub KmlCoordinates(ByVal oldlat As Double, ByVal oldlon As Double, ByVal altitude As Double)
    '            _Longitude = oldlon
    '            _Latitude = oldlat
    '            _Altitude = altitude
    '        End Sub
    '        Public Property oldlat() As Double
    '            Get
    '                Return _Latitude
    '            End Get
    '            Set(ByVal oldlat As Double)
    '                _Latitude = oldlat
    '            End Set
    '        End Property
    '        Public Property oldlan() As Double
    '            Get
    '                Return _Longitude
    '            End Get
    '            Set(ByVal oldlong As Double)
    '                _Longitude = oldlan
    '            End Set
    '        End Property
    '        Public Property Altitude() As Double
    '            Get
    '                Return _Altitude
    '            End Get
    '            Set(ByVal Altitude As Double)
    '                _Altitude = Altitude
    '            End Set
    '        End Property
    '    End Class
    '    Public Class KMLPoint
    '        Dim _Coords As KMLCoordinates
    '        Dim _AltitudeMode As KMLAltitudeMode = KMLAltitudeMode.ClamToGround
    '        Dim _KMLFilePath As String = "C:\\"
    '        Dim _KMLFileName As String = "Point.kml"
    '        Dim _Name As String = ""
    '        Dim _Description As String = ""
    '        Public Sub KMLPoint()

    '        End Sub
    '        Public Sub KMLPoint(ByVal Coords As KMLCoordinates, ByVal Name As String, ByVal Descripcion As String)
    '            _Description = Descripcion
    '            _Name = Name
    '            _Coords = Coords
    '        End Sub
    '        Public Property Coords() As KMLCoordinates
    '            Get
    '                Return _Coords
    '            End Get
    '            Set(ByVal Coords As KMLCoordinates)
    '                _Coords = Coords
    '            End Set
    '        End Property
    '        Public Property AltitudeMode() As KMLAltitudeMode
    '            Get
    '                Return _AltitudeMode
    '            End Get
    '            Set(ByVal AltitudeMode As KMLAltitudeMode)
    '                _AltitudeMode = AltitudeMode
    '            End Set
    '        End Property
    '        Public Property KMLFilePath() As String
    '            Get
    '                Return _KMLFilePath
    '            End Get
    '            Set(ByVal KMLFilePath As String)
    '                _KMLFilePath = KMLFilePath
    '                    If (_KMLFilePath.Substring(_KMLFilePath.Length -1,1) != "\\")
    '                    _KMLFilePath += "\\"
    '                End If
    '            End Set
    '        End Property
    '        Public Property KMLFileName() As String
    '            Get
    '                Return _KMLFileName
    '            End Get
    '            Set(ByVal KMLFileName As String)
    '                _KMLFileName = KMLFileName
    '            End Set
    '        End Property
    '        Public Property Name() As String
    '            Get
    '                Return _Name
    '            End Get
    '            Set(ByVal Name As String)
    '                _Name = Name
    '            End Set
    '        End Property
    '        Public Property Description() As String
    '            Get
    '                Return _Description
    '            End Get
    '            Set(ByVal Description As String)
    '                _Description = Description
    '            End Set
    '        End Property
    '        Public Sub Save()
    '            Dim FullFilePath As String = _KMLFilePath + _KMLFileName
    '            Dim xtr As New XmlTextWriter(FullFilePath, Nothing)
    '            '        xtr.WriteStartDocument()

    '            '        xtr.WriteStartElement("kml")
    '            '    xtr.WriteString(" xmlns=\"http://earth.google.com/kml/2.1\"") 
    '            '   {
    '            '        xtr.WriteStartElement("Placemark")
    '            '      {
    '            '        xtr.WriteStartElement("Name")
    '            '        xtr.WriteString(_Name)
    '            '        xtr.WriteEndElement()

    '            '        xtr.WriteStartElement("Description")
    '            '        xtr.WriteString(_Description)
    '            '        xtr.WriteEndElement()

    '            '        xtr.WriteStartElement("LookAt")
    '            '       {
    '            '        xtr.WriteStartElement("longitude")
    '            '        xtr.WriteString(_Coords.Longitude.ToString())
    '            '        xtr.WriteEndElement()
    '            '        xtr.WriteStartElement("latitude")
    '            '        xtr.WriteString(_Coords.Latitude.ToString())
    '            '        xtr.WriteEndElement()

    '            '        xtr.WriteStartElement("range")
    '            '        xtr.WriteString("400")
    '            '        xtr.WriteEndElement()

    '            '        xtr.WriteStartElement("tilt")
    '            '        xtr.WriteString("0")
    '            '        xtr.WriteEndElement()

    '            '        xtr.WriteStartElement("heading")
    '            '        xtr.WriteString("0")
    '            '        xtr.WriteEndElement()
    '            '       }
    '            '        xtr.WriteEndElement()

    '        End Sub

    '    End Class
End Class
