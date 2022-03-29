Imports System.IO
Imports System.Data.OleDb

Public Class MotonaveRegistro
    Dim dtCliente As DataTable

    Private Sub verificaCamposSalida(sender As Object, e As EventArgs) Handles txtNombre.LostFocus, txtDireccion.LostFocus, txtTelefono.LostFocus
        Dim txt As New TextBox
        txt = sender
        If txt.Text = "" Then
            txt.Text = "<-complete los datos->"
        End If
        sender = txt
    End Sub

    Private Sub verificaCamposEntrada(sender As Object, e As EventArgs) Handles txtNombre.GotFocus, txtDireccion.GotFocus, txtTelefono.GotFocus
        Dim txt As New TextBox
        txt = sender
        If txt.Text = "<-complete los datos->" Then
            txt.Text = ""
        End If
        sender = txt
    End Sub

    Public Sub Llenar()
        Dim cadena As String = "provider=Microsoft.Jet.OLEDB.4.0;Data Source='basedatos.xls';Extended Properties=Excel 8.0;"


        Dim conn As New OleDbConnection(cadena)
        conn.Open()

        Dim da As New OleDbDataAdapter("select * from [cliente$]", conn)
        Dim ds As New DataSet
        da.Fill(ds)

        dtCliente = New DataTable
        dtCliente = ds.Tables(0)

        Dim bs As New BindingSource
        bs.DataSource = dtCliente

        bs.Filter = "NOMBRE like '%" & txtBuscar.Text & "%'"

        DataGridView1.DataSource = bs



        conn.Close()


    End Sub
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Llenar()
    End Sub

    Private Sub LlenaTexto()
        txtID.Text = DataGridView1.CurrentRow.Cells("ID").Value.ToString
        txtNombre.Text = DataGridView1.CurrentRow.Cells("NOMBRE").Value.ToString
        txtTelefono.Text = DataGridView1.CurrentRow.Cells("TELEFONO").Value.ToString
        txtDireccion.Text = DataGridView1.CurrentRow.Cells("DIRECCION").Value.ToString

    End Sub

    Public Sub LimpiarCampos()
        txtID.Clear()
        txtNombre.Clear()
        txtTelefono.Clear()
        txtDireccion.Clear()

        txtNombre.Focus()

    End Sub

    Private Sub CrearID()
        Dim ValorID As Integer = 0

        For Each Fila As DataRow In dtCliente.Rows
            If IsNumeric(Fila("ID")) = True Then
                If CInt(Fila("ID")) > CInt(ValorID) Then
                    ValorID = Fila("ID")
                End If

            End If
        Next

        ValorID += 1
        txtID.Text = ValorID

        txtNombre.Focus()
    End Sub




    Private Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles btnEliminar.Click
        Dim cadena As String = "provider=Microsoft.Jet.OLEDB.4.0;Data Source='basedatos.xls';Extended Properties=Excel 8.0;"


        Dim conn As New OleDbConnection(cadena)
        conn.Open()

        Dim cmd As New OleDbCommand("update [cliente$] set ID='0',NOMBRE='',TELEFONO='',DIRECCION='' where ID='" & txtID.Text & "'", conn)
        cmd.ExecuteNonQuery()

        conn.Close()
        Llenar()
    End Sub

    Private Sub btnActualizar_Click(sender As Object, e As EventArgs) Handles btnActualizar.Click
        Dim cadena As String = "provider=Microsoft.Jet.OLEDB.4.0;Data Source='basedatos.xls';Extended Properties=Excel 8.0;"


        Dim conn As New OleDbConnection(cadena)
        conn.Open()

        Dim cmd As New OleDbCommand("update [cliente$] set NOMBRE='" & txtNombre.Text & "',TELEFONO='" & txtTelefono.Text & "',DIRECCION='" & txtDireccion.Text & "' where ID='" & txtID.Text & "'", conn)
        cmd.ExecuteNonQuery()

        conn.Close()

        Llenar()
    End Sub

    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Dim cadena As String = "provider=Microsoft.Jet.OLEDB.4.0;Data Source='basedatos.xls';Extended Properties=Excel 8.0;"


        Dim conn As New OleDbConnection(cadena)
        conn.Open()

        Dim bs As New BindingSource
        bs.DataSource = dtCliente

        bs.Filter = "ID ='0'"

        If bs.Count > 0 Then
            Dim cmd As New OleDbCommand("update [cliente$] set ID='" & txtID.Text & "',NOMBRE='" & txtNombre.Text & "',TELEFONO='" & txtTelefono.Text & "',DIRECCION='" & txtDireccion.Text & "' where ID='0'", conn)
            cmd.ExecuteNonQuery()
        Else

            Dim cmd As New OleDbCommand("Insert into [cliente$] (ID,NOMBRE,TELEFONO,DIRECCION) values('" & txtID.Text & "','" & txtNombre.Text & "','" & txtTelefono.Text & "','" & txtDireccion.Text & "')", conn)
            cmd.ExecuteNonQuery()
        End If

        conn.Close()
        Llenar()
    End Sub

    Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        LimpiarCampos()

        CrearID()
    End Sub

    Private Sub txtBuscar_TextChanged(sender As Object, e As EventArgs) Handles txtBuscar.TextChanged
        Llenar()
    End Sub

    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        If DataGridView1.RowCount > 0 Then
            LlenaTexto()
        Else
            LimpiarCampos()
        End If
    End Sub

    Private Sub Label5_Click(sender As Object, e As EventArgs) Handles Label5.Click

    End Sub
End Class