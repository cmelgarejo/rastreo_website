Imports System
Imports System.Data
Imports System.Configuration
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.HtmlControls
Imports System.IO
Imports System.Globalization

''' <summary>
''' Summary description for ErrHandler
''' </summary>
Public Class ErrHandler
    Public Sub New()
        '
        ' TODO: Add constructor logic here
        '
    End Sub
    'Declaración

    Public Const vbLf As String = ""

    ''' Handles error by accepting the error message 
    ''' Displays the page on which the error occured
    Public Shared Sub WriteError(ByVal errorMessage As String)
        Try
            Dim DirectorioERROR = System.Web.HttpContext.Current.Server.MapPath("~/Error/")
            Dim path As String = "~/Error/" & DateTime.Today.ToString("dd-MM-yyyy") & ".txt"
            If Not Directory.Exists(DirectorioERROR) Then
                Directory.CreateDirectory(DirectorioERROR)
            End If
            If Not File.Exists(System.Web.HttpContext.Current.Server.MapPath(path)) Then
                File.Create(System.Web.HttpContext.Current.Server.MapPath(path)).Close()
            End If
            Using w As StreamWriter = File.AppendText(System.Web.HttpContext.Current.Server.MapPath(path))
                w.WriteLine(vbLf & vbLf & "Log Entry : ")
                w.WriteLine("{0}", DateTime.Now.ToString(CultureInfo.InvariantCulture))
                Dim err As String = ("Error in: " & System.Web.HttpContext.Current.Request.Url.ToString() & ". Error Message:") + errorMessage
                w.WriteLine(err)
                w.WriteLine("__________________________")
                w.Flush()
                w.Close()
            End Using
        Catch ex As Exception
            WriteError(ex.Message)

        End Try
    End Sub
End Class
