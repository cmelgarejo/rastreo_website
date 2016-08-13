
Partial Class _NOTREADY
    Inherits System.Web.UI.Page

    Public mi_usuario As New Usuario()

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        'If Not Session("session_UsuarioRASTREO") Is Nothing Then
        '    mi_usuario = CType(Session("session_UsuarioRASTREO"), Usuario)
        'Else
        '    Response.Redirect("Login.aspx?rp=" + Me.Request.Url.AbsoluteUri(), False)
        'End If
    End Sub

End Class
