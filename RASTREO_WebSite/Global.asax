<%@ Application Language="VB" %>
<%@ Import Namespace="System.Reflection" %>
<%@ Import Namespace="System.Diagnostics" %>

<script runat="server">
    
    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs on application startup
    End Sub
    
    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs on application shutdown
        'comienza el codigo JMMZ
        'Dim runtime As HttpRuntime = (HttpRuntime) GetType(System.Web.HttpRuntime).InvokeMember("_theRuntime", BindingFlags.NonPublic Or  BindingFlags.Static Or  BindingFlags.GetField,                                                                                  Nothing, Nothing, Nothing)
        'If Runtime = Nothing Then
        '    Return
        'End If
        'Dim shutDownMessage As String = CType(Runtime.().InvokeMember("_shutDownMessage", BindingFlags.NonPublic Or BindingFlags.Instance Or BindingFlags.GetField, Nothing, Runtime, Nothing), String)

        'Dim shutDownStack As String = CType(Runtime.GetType().InvokeMember("_shutDownStack", BindingFlags.NonPublic Or BindingFlags.Instance Or BindingFlags.GetField, Nothing, Runtime, Nothing), String)

        'If Not EventLog.SourceExists(".NET Runtime") Then
        '    EventLog.CreateEventSource(".NET Runtime", "Application")

        'End If

        'Dim log As New EventLog()
        'log.Source = ".NET Runtime"
        'log.WriteEntry(String.Format("" & vbCrLf & "" & vbCrLf & "_shutDownMessage={0}" & vbCrLf & "" & vbCrLf & "_shutDownStack={1}", shutDownMessage, shutDownStack), EventLogEntryType.Error)
        'Termina el codigo JMMZ
    End Sub
        
    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs when an unhandled error occurs
    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs when a new session is started
    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs when a session ends. 
        ' Note: The Session_End event is raised only when the sessionstate mode
        ' is set to InProc in the Web.config file. If session mode is set to StateServer 
        ' or SQLServer, the event is not raised.
    End Sub
       
</script>