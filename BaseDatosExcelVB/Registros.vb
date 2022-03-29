Imports System.Data.OleDb

Public Class Registros
    Dim dtCliente As DataTable
    Dim dtVentas As DataTable
    Dim dtDetalleVentas As DataTable

    Dim dtConsulta As DataTable
    Dim dtDetalleConsulta As DataTable

    Dim bs As New BindingSource
    Dim bsCliente As New BindingSource


    Private Sub VentasRealizadasForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LlenarCliente()
        LlenarVentas()
        LlenarDetalleVentas()

        ConsultaVentas()
    End Sub

    Public Sub LlenarCliente()
        Dim cadena As String = "provider=Microsoft.Jet.OLEDB.4.0;Data Source='basedatos.xls';Extended Properties=Excel 8.0;"

        Dim conn As New OleDbConnection(cadena)
        conn.Open()

        Dim da As New OleDbDataAdapter("select * from [cliente$]", conn)
        Dim ds As New DataSet
        da.Fill(ds)

        dtCliente = New DataTable
        dtCliente = ds.Tables(0)

        conn.Close()
    End Sub

    Public Sub LlenarVentas()
        Dim cadena As String = "provider=Microsoft.Jet.OLEDB.4.0;Data Source='basedatos.xls';Extended Properties=Excel 8.0;"

        Dim conn As New OleDbConnection(cadena)
        conn.Open()

        Dim da As New OleDbDataAdapter("select * from [ventas$]", conn)
        Dim ds As New DataSet
        da.Fill(ds)

        dtVentas = New DataTable
        dtVentas = ds.Tables(0)
        conn.Close()

    End Sub

    Public Sub LlenarDetalleVentas()
        Dim cadena As String = "provider=Microsoft.Jet.OLEDB.4.0;Data Source='basedatos.xls';Extended Properties=Excel 8.0;"

        Dim conn As New OleDbConnection(cadena)
        conn.Open()

        Dim da As New OleDbDataAdapter("select * from [detalle_ventas$]", conn)
        Dim ds As New DataSet
        da.Fill(ds)

        dtDetalleVentas = New DataTable
        dtDetalleVentas = ds.Tables(0)
        conn.Close()
    End Sub

    Public Sub ConsultaDetalleVentas()

        dtDetalleConsulta = New DataTable("TablaConsulta")

        dtDetalleConsulta.Columns.Add("ID_FACTURA")
        dtDetalleConsulta.Columns.Add("CODIGO")
        dtDetalleConsulta.Columns.Add("DESCRIPCION")
        dtDetalleConsulta.Columns.Add("CANTIDAD")
        dtDetalleConsulta.Columns.Add("PRECIO")
        dtDetalleConsulta.Columns.Add("IMPUESTOS")
        dtDetalleConsulta.Columns.Add("SUBTOTAL")

        Dim dr As DataRow

        For Each DETALLEV As DataRow In dtDetalleVentas.Rows

            dr = dtDetalleConsulta.NewRow()

            dr("ID_FACTURA") = DETALLEV("ID_FACTURA")
            dr("CODIGO") = DETALLEV("CODIGO")
            dr("DESCRIPCION") = DETALLEV("DESCRIPCION")
            dr("CANTIDAD") = DETALLEV("CANTIDAD")
            dr("PRECIO") = DETALLEV("PRECIO")
            dr("IMPUESTOS") = DETALLEV("IMPUESTOS")
            dr("SUBTOTAL") = (DETALLEV("CANTIDAD") * DETALLEV("PRECIO")) + DETALLEV("IMPUESTOS")

            dtDetalleConsulta.Rows.Add(dr)

        Next

        bs.DataSource = dtDetalleConsulta

        DataGridView1.AutoGenerateColumns = False
        DataGridView1.DataSource = bs
    End Sub

    Public Sub ConsultaVentas()

        dtConsulta = New DataTable("ConsultaDetalle")

        dtConsulta.Columns.Add("ID_CLIENTE")
        dtConsulta.Columns.Add("NOMBRE")
        dtConsulta.Columns.Add("ID_FACTURA")
        dtConsulta.Columns.Add("FECHA")

        Dim dr As DataRow

        For Each ventas As DataRow In dtVentas.Rows
            dr = dtConsulta.NewRow()

            For Each NOMBRECLI As DataRow In dtCliente.Rows
                If NOMBRECLI("ID") = ventas("ID_CLIENTE") Then
                    dr("NOMBRE") = NOMBRECLI("NOMBRE")
                    Exit For
                End If
            Next

            dr("ID_CLIENTE") = ventas("ID_CLIENTE")
            dr("ID_FACTURA") = ventas("ID_FACTURA")
            dr("FECHA") = ventas("FECHA")

            dtConsulta.Rows.Add(dr)

        Next

        bs.DataSource = dtConsulta
        DataGridView2.AutoGenerateColumns = False
        DataGridView2.DataSource = bs
    End Sub


    Private Sub txtBuscar_TextChanged(sender As Object, e As EventArgs) Handles txtBuscar.TextChanged
        ConsultaDetalleVentas()

        bsCliente.DataSource = dtConsulta

        bsCliente.Filter = "NOMBRE like '%" & txtBuscar.Text & "%'"
        DataGridView2.DataSource = bsCliente
    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick

    End Sub

    Private Sub DataGridView2_CellClick(sender As Object, e As DataGridViewCellEventArgs)
        If DataGridView2.RowCount > 0 Then
            bs.DataSource = dtDetalleConsulta

            Dim MiFact As String = DataGridView2.CurrentRow.Cells("id_facturaf").Value

            bs.Filter = "ID_FACTURA =" & MiFact
            DataGridView1.DataSource = bs
        End If
    End Sub

    Private Sub DataGridView2_CellContentClick(sender As Object, e As DataGridViewCellEventArgs)

    End Sub

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs)

    End Sub

    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click

    End Sub

    Private Sub Label3_Click(sender As Object, e As EventArgs) Handles Label3.Click

    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged

    End Sub

    Private Sub Label4_Click(sender As Object, e As EventArgs) Handles Label4.Click

    End Sub

    Private Sub Label6_Click(sender As Object, e As EventArgs) Handles Label6.Click

    End Sub

    Private Sub Label2_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub Label7_Click(sender As Object, e As EventArgs) Handles Label7.Click

    End Sub

    Private Sub Label11_Click(sender As Object, e As EventArgs) Handles Label11.Click

    End Sub
End Class