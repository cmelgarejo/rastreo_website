Imports System
Imports System.Security.Cryptography
Imports System.Text

Public Class Cryptografia
    ''' <summary>
    ''' Encripta una cadena de texto usando el algoritmo de encriptacion de hash MD5.
    ''' el "Message Digest" es una encriptacion de 128-bit y es usado comunmente para 
    ''' verificar datosis chequeando el "Checksum MD5", mas informacion se puede 
    ''' encontrar en:
    ''' 
    ''' http://www.faqs.org/rfcs/rfc1321.html
    ''' </summary>
    ''' <param name="Data">cadena conteniendo el string a hashear a MD5.</param>
    ''' <returns>Una cadena de texto conteniendo en forma encriptada la cadena ingresada.</returns>
    Public Shared Function MD5Hash(ByVal Data As String) As String
        Dim md5 As MD5 = New MD5CryptoServiceProvider()
        Dim hash As Byte() = md5.ComputeHash(Encoding.UTF8.GetBytes(Data))

        Dim stringBuilder As New StringBuilder()
        For Each b As Byte In hash
            stringBuilder.AppendFormat("{0:x2}", b)
        Next
        Return stringBuilder.ToString()
    End Function

    ''' <summary>
    ''' Encripta una cadena utilizando el algoritmo SHA256 (Secure Hash Algorithm)
    ''' Detalles: http://www.itl.nist.gov/fipspubs/fip180-1.htm
    ''' Esto trabaja de misma manera que el MD5, solo que utilizando una 
    ''' encriptacion en 256bits.
    ''' </summary>
    ''' <param name="Data">A string containing the data to encrypt.</param>
    ''' <returns>A string containing the string, encrypted with the SHA256 algorithm.</returns>
    Public Shared Function SHA256Hash(ByVal Data As String) As String
        Dim sha As SHA256 = New SHA256Managed()
        Dim hash As Byte() = sha.ComputeHash(Encoding.UTF8.GetBytes(Data))

        Dim stringBuilder As New StringBuilder()
        For Each b As Byte In hash
            stringBuilder.AppendFormat("{0:x2}", b)
        Next
        Return stringBuilder.ToString()
    End Function

    ''' <summary>
    ''' Encrypts a string using the SHA384(Secure Hash Algorithm) algorithm.
    ''' This works in the same manner as MD5, providing 384bit encryption.
    ''' </summary>
    ''' <param name="Data">A string containing the data to encrypt.</param>
    ''' <returns>A string containing the string, encrypted with the SHA384 algorithm.</returns>
    Public Shared Function SHA384Hash(ByVal Data As String) As String
        Dim sha As SHA384 = New SHA384Managed()
        Dim hash As Byte() = sha.ComputeHash(Encoding.UTF8.GetBytes(Data))

        Dim stringBuilder As New StringBuilder()
        For Each b As Byte In hash
            stringBuilder.AppendFormat("{0:x2}", b)
        Next
        Return stringBuilder.ToString()
    End Function


    ''' <summary>
    ''' Encrypts a string using the SHA512 (Secure Hash Algorithm) algorithm.
    ''' This works in the same manner as MD5, providing 512bit encryption.
    ''' </summary>
    ''' <param name="Data">A string containing the data to encrypt.</param>
    ''' <returns>A string containing the string, encrypted with the SHA512 algorithm.</returns>
    Public Shared Function SHA512Hash(ByVal Data As String) As String
        Dim sha As SHA512 = New SHA512Managed()
        Dim hash As Byte() = sha.ComputeHash(Encoding.UTF8.GetBytes(Data))

        Dim stringBuilder As New StringBuilder()
        For Each b As Byte In hash
            stringBuilder.AppendFormat("{0:x2}", b)
        Next
        Return stringBuilder.ToString()
    End Function
End Class
