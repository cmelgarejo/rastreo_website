#Region "Using"

Imports System
Imports System.IO
Imports System.Web
Imports System.Web.UI
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.IO.Compression
Imports System.Web.Caching
Imports System.Net
Imports System.Collections.Generic

#End Region

Public Class ScriptCompressorHandler
    Implements IHttpHandler
    Private Const DAYS_IN_CACHE As Integer = 30

    ''' <summary>
    ''' Enables processing of HTTP Web requests by a custom 
    ''' HttpHandler that implements the <see cref="T:System.Web.IHttpHandler"></see> interface.
    ''' </summary>
    ''' <param name="context">An <see cref="T:System.Web.HttpContext"></see> object that provides 
    ''' references to the intrinsic server objects 
    ''' (for example, Request, Response, Session, and Server) used to service HTTP requests.
    ''' </param>
    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        Dim root As String = context.Request.Url.GetLeftPart(UriPartial.Authority)
        Dim path As String = context.Request.QueryString("path")
        If String.IsNullOrEmpty(path) Then
            path = context.Request.FilePath
        End If
        Dim content As String = String.Empty

        If Not String.IsNullOrEmpty(path) Then
            If context.Cache(path) Is Nothing Then
                Dim scripts As String() = path.Split(New String() {","}, StringSplitOptions.RemoveEmptyEntries)
                For Each script As String In scripts
                    ' We only want to serve resource files for security reasons.
                    If script.ToUpperInvariant().Contains("RESOURCE.AXD") Or script.ToUpperInvariant().Contains("CHARTIMG.AXD") Then
                        content += RetrieveScript(root + script) + Environment.NewLine
                    End If
                Next
                content = StripWhitespace(content)
                context.Cache.Insert(path, content, Nothing, Cache.NoAbsoluteExpiration, New TimeSpan(DAYS_IN_CACHE, 0, 0, 0))
            End If
        End If

        If Not String.IsNullOrEmpty(path) Then
            content = DirectCast(context.Cache(path), String)
        End If
        If Not String.IsNullOrEmpty(content) Then
            context.Response.Write(content)
            SetHeaders(content.GetHashCode(), context)
            Compress(context)
        End If
    End Sub

    ''' <summary>
    ''' Retrieves the specified remote script using a WebClient.
    ''' </summary>
    ''' <param name="file">The remote URL</param>
    Private Shared Function RetrieveScript(ByVal file As String) As String
        Dim script As String = Nothing

        Try
            Dim url As New Uri(file, UriKind.Absolute)

            Dim request As HttpWebRequest = DirectCast(WebRequest.Create(url), HttpWebRequest)
            request.Method = "GET"
            request.AutomaticDecompression = DecompressionMethods.GZip

            Using response As HttpWebResponse = DirectCast(request.GetResponse(), HttpWebResponse)
                Using reader As New StreamReader(response.GetResponseStream())
                    script = reader.ReadToEnd()
                End Using
            End Using
        Catch generatedExceptionName As System.Net.Sockets.SocketException
            ' The remote site is currently down. Try again next time.
        Catch generatedExceptionName As UriFormatException
            ' Only valid absolute URLs are accepted
        End Try

        Return script
    End Function

    ''' <summary>
    ''' Strips the whitespace from any .js file.
    ''' </summary>
    Private Shared Function StripWhitespace(ByVal body As String) As String
        Dim lines As String() = body.Split(New String() {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries)
        Dim emptyLines As New StringBuilder()
        For Each line As String In lines
            Dim s As String = line.Trim()
            If s.Length > 0 AndAlso Not s.StartsWith("//") Then
                emptyLines.AppendLine(s.Trim())
            End If
        Next

        body = emptyLines.ToString()

        ' remove C styles comments
        body = Regex.Replace(body, "/\*.*?\*/", [String].Empty, RegexOptions.Compiled Or RegexOptions.Singleline)
        '/ trim left
        body = Regex.Replace(body, "^\s*", [String].Empty, RegexOptions.Compiled Or RegexOptions.Multiline)
        '/ trim right
        body = Regex.Replace(body, "\s*[\r\n]", Microsoft.VisualBasic.vbCrLf, RegexOptions.Compiled Or RegexOptions.ECMAScript)
        ' remove whitespace beside of left curly braced
        body = Regex.Replace(body, "\s*{\s*", "{", RegexOptions.Compiled Or RegexOptions.ECMAScript)
        ' remove whitespace beside of coma
        body = Regex.Replace(body, "\s*,\s*", ",", RegexOptions.Compiled Or RegexOptions.ECMAScript)
        ' remove whitespace beside of semicolon
        body = Regex.Replace(body, "\s*;\s*", ";", RegexOptions.Compiled Or RegexOptions.ECMAScript)
        ' remove newline after keywords
        body = Regex.Replace(body, "\r\n(?<=\b(abstract|boolean|break|byte|case|catch|char|class|const|continue|default|delete|do|double|else|extends|false|final|finally|float|for|function|goto|if|implements|import|in|instanceof|int|interface|long|native|new|null|package|private|protected|public|return|short|static|super|switch|synchronized|this|throw|throws|transient|true|try|typeof|var|void|while|with)\r\n)", " ", RegexOptions.Compiled Or RegexOptions.ECMAScript)

        Return body
    End Function

    ''' <summary>
    ''' This will make the browser and server keep the output
    ''' in its cache and thereby improve performance.
    ''' </summary>
    Private Shared Sub SetHeaders(ByVal hash As Integer, ByVal context As HttpContext)
        context.Response.ContentType = "text/javascript"
        context.Response.Cache.VaryByHeaders("Accept-Encoding") = True
        context.Response.Cache.SetExpires(DateTime.Now.ToUniversalTime().AddDays(DAYS_IN_CACHE))
        context.Response.Cache.SetCacheability(HttpCacheability.[Public])
        context.Response.Cache.SetMaxAge(New TimeSpan(DAYS_IN_CACHE, 0, 0, 0))
        context.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches)
        context.Response.Cache.SetETag("""" & hash.ToString() & """")
    End Sub

#Region "Compression"

    Private Const GZIP As String = "gzip"
    Private Const DEFLATE As String = "deflate"

    Private Shared Sub Compress(ByVal context As HttpContext)
        If context.Request.UserAgent IsNot Nothing AndAlso context.Request.UserAgent.Contains("MSIE 6") Then
            Exit Sub
        End If

        If IsEncodingAccepted(GZIP) Then
            context.Response.Filter = New GZipStream(context.Response.Filter, CompressionMode.Compress)
            SetEncoding(GZIP)
        ElseIf IsEncodingAccepted(DEFLATE) Then
            context.Response.Filter = New DeflateStream(context.Response.Filter, CompressionMode.Compress)
            SetEncoding(DEFLATE)
        End If
    End Sub

    ''' <summary>
    ''' Checks the request headers to see if the specified
    ''' encoding is accepted by the client.
    ''' </summary>
    Private Shared Function IsEncodingAccepted(ByVal encoding As String) As Boolean
        Return HttpContext.Current.Request.Headers("Accept-encoding") IsNot Nothing AndAlso HttpContext.Current.Request.Headers("Accept-encoding").Contains(encoding)
    End Function

    ''' <summary>
    ''' Adds the specified encoding to the response headers.
    ''' </summary>
    ''' <param name="encoding"></param>
    Private Shared Sub SetEncoding(ByVal encoding As String)
        HttpContext.Current.Response.AppendHeader("Content-encoding", encoding)
    End Sub

#End Region

    ''' <summary>
    ''' Gets a value indicating whether another request can use the <see cref="T:System.Web.IHttpHandler"></see> instance.
    ''' </summary>
    ''' <value></value>
    ''' <returns>true if the <see cref="T:System.Web.IHttpHandler"></see> instance is reusable; otherwise, false.</returns>
    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class

''' <summary>
''' Find scripts and change the src to the ScriptCompressorHandler.
''' </summary>
Public Class ScriptCompressorModule
    Implements IHttpModule

#Region "IHttpModule Members"

    Private Sub Dispose() Implements IHttpModule.Dispose
        ' Nothing to dispose; 
    End Sub

    Private Sub Init(ByVal context As HttpApplication) Implements IHttpModule.Init
        AddHandler context.PostRequestHandlerExecute, AddressOf context_BeginRequest
    End Sub

#End Region

    Private Sub context_BeginRequest(ByVal sender As Object, ByVal e As EventArgs)
        Dim app As HttpApplication = TryCast(sender, HttpApplication)
        If TypeOf app.Context.CurrentHandler Is Page AndAlso Not app.Request.RawUrl.Contains("serviceframe") Then
            'if (!app.Context.Request.Url.Scheme.Contains("https")) {
            '}
            app.Response.Filter = New WebResourceFilter(app.Response.Filter)
        End If
    End Sub

#Region "Stream filter"

    Private Class WebResourceFilter
        Inherits Stream

        Public Sub New(ByVal sink As Stream)
            _sink = sink
        End Sub

        Private _sink As Stream

#Region "Properites"

        Public Overloads Overrides ReadOnly Property CanRead() As Boolean
            Get
                Return True
            End Get
        End Property

        Public Overloads Overrides ReadOnly Property CanSeek() As Boolean
            Get
                Return True
            End Get
        End Property

        Public Overloads Overrides ReadOnly Property CanWrite() As Boolean
            Get
                Return True
            End Get
        End Property

        Public Overloads Overrides Sub Flush()
            _sink.Flush()
        End Sub

        Public Overloads Overrides ReadOnly Property Length() As Long
            Get
                Return 0
            End Get
        End Property

        Private _position As Long
        Public Overloads Overrides Property Position() As Long
            Get
                Return _position
            End Get
            Set(ByVal value As Long)
                _position = value
            End Set
        End Property

#End Region

#Region "Methods"

        Public Overloads Overrides Function Read(ByVal buffer As Byte(), ByVal offset As Integer, ByVal count As Integer) As Integer
            Return _sink.Read(buffer, offset, count)
        End Function

        Public Overloads Overrides Function Seek(ByVal offset As Long, ByVal origin As SeekOrigin) As Long
            Return _sink.Seek(offset, origin)
        End Function

        Public Overloads Overrides Sub SetLength(ByVal value As Long)
            _sink.SetLength(value)
        End Sub

        Public Overloads Overrides Sub Close()
            _sink.Close()
        End Sub

        Public Overloads Overrides Sub Write(ByVal buffer__1 As Byte(), ByVal offset As Integer, ByVal count As Integer)
            Dim data As Byte() = New Byte(count - 1) {}
            Buffer.BlockCopy(buffer__1, offset, data, 0, count)
            Dim html As String = System.Text.Encoding.[Default].GetString(buffer__1)
            Dim index As Integer = 0
            Dim list As New List(Of String)()

            Dim regex As New Regex("<script\s*src=""((?=[^""]*(webresource.axd|scriptresource.axd|chartimg.axd))[^""]*)""\s*type=""text/javascript""[^>]*>[^<]*(?:</script>)?", RegexOptions.IgnoreCase)

            For Each match As Match In regex.Matches(html)
                If index = 0 Then
                    index = html.IndexOf(match.Value)
                End If

                Dim relative As String = match.Groups(1).Value
                list.Add(relative)
                html = html.Replace(match.Value, String.Empty)
            Next

            If index > 0 Then
                Dim script As String = "<script type=""text/javascript"" src=""js.axd?path={0}""></script>"
                Dim path As String = String.Empty
                Dim temp As String = String.Empty
                For Each s As String In list
                    'limit the js.axd?path= link string to 2048 characters in IE
                    temp += HttpUtility.UrlEncode(s) & ","
                    If Not HttpContext.Current Is Nothing Then
                        If Not HttpContext.Current.Request.UserAgent Is Nothing Then
                            If HttpContext.Current.Request.UserAgent.Contains("MSIE") AndAlso temp.Length > 2036 Then
                                temp = String.Format(script, path) + Environment.NewLine
                                html = html.Insert(index, temp)
                                index += temp.Length
                                temp = HttpUtility.UrlEncode(s) & ","
                            End If
                            path = temp
                        End If
                    End If
                Next
                html = html.Insert(index, String.Format(script, path))
            End If

            Dim outdata As Byte() = System.Text.Encoding.[Default].GetBytes(html)
            _sink.Write(outdata, 0, outdata.GetLength(0))
        End Sub

#End Region

    End Class

#End Region

End Class
