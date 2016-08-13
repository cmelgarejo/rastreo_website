Imports System.ComponentModel

Partial Class SoundPlayerI
    Inherits System.Web.UI.UserControl

    <Category("Archivo de Sonido")> _
    <Browsable(True)> _
    <Description("Setea la locación del archivo de sonido.")> _
    Property Archivo_de_Sonido() As String
        Get
            Dim s As String = CStr(ViewState("_SoundFile"))
            If s Is Nothing Then
                Return String.Empty
            Else
                Return s
            End If
        End Get

        Set(ByVal Value As String)
            ViewState("_SoundFile") = Value
        End Set
    End Property

End Class
