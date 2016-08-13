Partial Class _Default
    Inherits System.Web.UI.Page

    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim sbScript As New StringBuilder()
        If Not IsPostBack Then
            ''Display name - Value for the Select query
            Dim ColumnName As ListItem
            ColumnName = New ListItem("School Name", " SchoolName")
            ddlSearchBy.Items.Add(ColumnName)
            ColumnName = New ListItem("Address", "Address")
            ddlSearchBy.Items.Add(ColumnName)
            ColumnName = New ListItem("Phone Number", "PhoneNo")
            ddlSearchBy.Items.Add(ColumnName)
        End If
    End Sub

    Protected Sub btnSUBMIT_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSUBMIT.Click
        SqlDataSource1.SelectCommand = "SELECT * FROM [Schools] WHERE " & ddlSearchBy.SelectedValue & _
                " LIKE '%" & txtSearch.Text.ToUpperInvariant() & "%'"
        SqlDataSource1.DataBind()
        grdDEMO.DataBind()
    End Sub

    Protected Sub btnCLEAR_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCLEAR.Click
        'SqlDataSource1.SelectCommand = "SELECT * FROM [Schools]"
        'SqlDataSource1.DataBind()
        'grdDEMO.DataBind()
        'Utilidades.putes()
    End Sub

    Protected Sub grdDEMO_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grdDEMO.ItemDataBound
        Try
            If SqlDataSource1.SelectCommand = "SELECT * FROM [Schools] WHERE " & ddlSearchBy.SelectedValue & " LIKE '%" & txtSearch.Text.ToUpperInvariant() & "%'" Then
                Dim sbScript As New StringBuilder()
                sbScript.Append("<script language='JavaScript' type='text/javascript'>" + ControlChars.Lf)
                sbScript.Append("move_map(" + e.Item.Cells(4).Text + "," + e.Item.Cells(5).Text + ");" + ControlChars.Lf)
                sbScript.Append("</script>" + ControlChars.Lf)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Onload_updatemap", _
                                                                      sbScript.ToString(), False)
            End If
        Catch
            ''Do nothing here.
        End Try
    End Sub

End Class
