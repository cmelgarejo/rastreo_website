
Imports System.Collections.Generic

''' <summary>
''' Clase de soporte para el usuario de la sesion en el sistema
''' </summary>
<System.Serializable()> _
Public Class Usuario
    ''' <summary>
    ''' Propiedades privadas de la clase
    ''' </summary>
    ''' <remarks></remarks>
    Private _Usuario As String = String.Empty
    Private _id_Usuario As Integer = 0
    Private _id_Persona As Integer = 0
    Private _id_Seguro As Integer = 0
    Private _IP As String = String.Empty
    Private _NombreUsuario As String = String.Empty
    Private _Nombre As String = String.Empty
    Private _Es_Empleado As Boolean = False
    Private _Es_Cliente As Boolean = False
    Private _Es_Seguro As Boolean = False
    Private _SU As Boolean = False
    ''' <summary>
    ''' Constructor por defecto de la clase
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        _Usuario = String.Empty
        _id_Usuario = 0
        _IP = String.Empty
        _Es_Empleado = False
        _Es_Cliente = False
    End Sub

    ''' <summary>
    ''' Direccion IP actual del usuario.
    ''' </summary>
    ''' <value>IP</value>
    ''' <returns>Direccion IP</returns>
    ''' <remarks>Ninguno</remarks>
    Public Property IP() As String
        Get
            Return _IP
        End Get
        Set(ByVal value As String)
            _IP = value
        End Set
    End Property

    ''' <summary>
    ''' Nombre del usuario logueado
    ''' </summary>
    Public Property NombreUsuario() As String
        Get
            Return _NombreUsuario
        End Get
        Set(ByVal value As String)
            _NombreUsuario = value
        End Set
    End Property

    ''' <summary>
    ''' Nombre del usuario logueado
    ''' </summary>
    Public Property Nombre() As String
        Get
            Return _Nombre
        End Get
        Set(ByVal value As String)
            _Nombre = value
        End Set
    End Property


    ''' <summary>
    ''' Indice numerico del Usuario. - integer
    ''' </summary>
    Public Property idUsuario() As Integer
        Get
            Return _id_Usuario
        End Get
        Set(ByVal value As Integer)
            _id_Usuario = value
        End Set
    End Property

    ''' <summary>
    ''' Indice numerico de la persona. - integer
    ''' </summary>
    Public Property idPersona() As Integer
        Get
            Return _id_Persona
        End Get
        Set(ByVal value As Integer)
            _id_Persona = value
        End Set
    End Property


    ''' <summary>
    ''' Indice numerico del seguro el cual representa el usuario. - integer
    ''' </summary>
    Public Property idSeguro() As Integer
        Get
            Return _id_Seguro
        End Get
        Set(ByVal value As Integer)
            _id_Seguro = value
        End Set
    End Property

    ''' <summary>
    ''' Usuario que esta utilizando la aplicación. - string
    ''' </summary>
    Public Property Usuario() As String
        Get
            Return _Usuario
        End Get
        Set(ByVal value As String)
            _Usuario = value
        End Set
    End Property

    ''' <summary>
    ''' Propiedad booleana que indica si el usuario es empleado de la empresa
    ''' </summary>
    ''' <value>True or False</value>
    ''' <returns>Falso o Verdadero</returns>
    ''' <remarks>Ninguna.</remarks>
    Public Property Empleado() As Boolean
        Get
            Return _Es_Empleado
        End Get
        Set(ByVal value As Boolean)
            _Es_Empleado = value
        End Set
    End Property

    ''' <summary>
    ''' Propiedad booleana que indica si el usuario es cliente de la empresa
    ''' </summary>
    ''' <value>True or False</value>
    ''' <returns>Falso o Verdadero</returns>
    ''' <remarks>Ninguna.</remarks>
    Public Property Cliente() As Boolean
        Get
            Return _Es_Cliente
        End Get
        Set(ByVal value As Boolean)
            _Es_Cliente = value
        End Set
    End Property

    ''' <summary>
    ''' Propiedad booleana que indica si el usuario es seguro, y puede ver los de su seguro
    ''' </summary>
    ''' <value>True or False</value>
    ''' <returns>Falso o Verdadero</returns>
    ''' <remarks>Ninguna.</remarks>
    Public Property Seguro() As Boolean
        Get
            Return _Es_Seguro
        End Get
        Set(ByVal value As Boolean)
            _Es_Seguro = value
        End Set
    End Property

    ''' <summary>
    ''' Propiedad booleana que indica si el usuario es superusuario
    ''' </summary>
    ''' <value>True or False</value>
    ''' <returns>Falso o Verdadero</returns>
    ''' <remarks>Ninguna.</remarks>
    Public Property SU() As Boolean
        Get
            Return _SU
        End Get
        Set(ByVal value As Boolean)
            _SU = value
        End Set
    End Property
End Class
