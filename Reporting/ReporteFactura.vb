﻿Imports System.IO
Imports Microsoft.Office.Interop.Excel
Imports System.Data.OleDb
Imports System.Windows.Forms


Public Module ReporteFactura
    Dim xlibro As Microsoft.Office.Interop.Excel.Application
    Public ESTADO As Date
    Public MOTONAVE As Integer
    Public ARRIBO As Date
    Public REGISTROCAPITANIA As Integer = 11
    Public VISITACAPITANIA As Date
    Public Eslra As Integer = 11
    Public Producto, TnljeAnncdoEmbrque, TnljeAnncdoDsmbrque, Procedencia, Destino, NmbreTpoBque As String


    Dim dtFactura As Data.DataTable = New Data.DataTable

    Public Sub AbrirDocumentoImprimir()
        'El siguiente codigo es para crear la ruta,entre comillas se pone la ruta donde esta el libro
        Dim Ruta As String = Path.Combine(Directory.GetCurrentDirectory(), "basedatos.xls")

        'El siguiente codigo es para abrir el libro y hacerlo visible, si se quiere dejar el libro oculto, se cambia la palabra True por False
        xlibro = CreateObject("Excel.Application")
        xlibro.Workbooks.Open(Ruta)
        xlibro.Visible = True
        xlibro.Sheets("MOTONAVE").Select() 'Nombre del libro
    End Sub

    Public Sub LlenarCliente()
        Dim cadena As String = "provider=Microsoft.Jet.OLEDB.4.0;Data Source='basedatos.xls';Extended Properties=Excel 8.0;"
        Dim TnljeAnncdoEmbrque As Double = 0
        Dim conn As New Data.OleDb.OleDbConnection(cadena)
        conn.Open()

        Dim da As New OleDbDataAdapter("select * from [detalle_ventas$]", conn)
        Dim ds As New DataSet
        da.Fill(ds)

        dtFactura = ds.Tables(0)

        Dim bs As New BindingSource
        bs.DataSource = dtFactura

        bs.Filter = "ID_CONTENEDOR =" & CONTENEDOR
        xlibro.Range("B6").Value = CONTENEDOR
        xlibro.Range("B7").Value = ESTADO
        xlibro.Range("B8").Value = Producto

        For Each view As DataRowView In bs
            Dim row = view.Row
            TnljeAnncdoEmbrque = row("TnljeAnncdoEmbrque")
            DESCRIPCION = row("DESCRIPCION")
            CANTIDAD = row("CANTIDAD")
            Destino = row("PRECIO")
            IMPUESTOS = row("IMPUESTOS")

            xlibro.Range("A" & REGISTROCAPITANIA).Value = TnljeAnncdoEmbrque
            xlibro.Range("B" & REGISTROCAPITANIA).Value = TnljeAnncdoDsmbrque
            xlibro.Range("C" & REGISTROCAPITANIA).Value = Procedencia
            xlibro.Range("D" & REGISTROCAPITANIA).Value = Destino
            xlibro.Range("E" & REGISTROCAPITANIA).Value = NmbreTpoBque
            REGISTROCAPITANIA += 1

        Next

        REGISTROCAPITANIA += 1
        With xlibro.Range("A" & REGISTROCAPITANIA & ":" & "F" & REGISTROCAPITANIA)
            .Interior.Color = RGB(179, 179, 179)
            .Font.Bold = True
        End With
        xlibro.Range("E" & REGISTROCAPITANIA).Value = "Total:"
        xlibro.Range("F" & REGISTROCAPITANIA).Value = TnljeAnncdoEmbrque
        'xlibro.ActiveWorkbook.Save()
        'xlibro.Quit()
        conn.Close()
    End Sub
    Private Sub KillExcelProcess()
        Try
            Dim Xcel() As Process = Process.GetProcessesByName("EXCEL")
            For Each Process As Process In Xcel
                Process.Kill()
            Next
        Catch ex As Exception
        End Try
    End Sub
    Public Sub EliminaFilasVacias()
        KillExcelProcess()
        AbrirDocumentoImprimir()
        xlibro.Sheets("motonave").Select()
        Dim lRow = xlibro.ActiveSheet.UsedRange.Rows.Count
        For fila As Integer = 1 To lRow
            If String.IsNullOrEmpty(xlibro.Range("A" & fila).Value) Then
                xlibro.Range("A" & fila).Select()
                xlibro.Range("A" & fila & ":Z" & fila).Delete()
                xlibro.ActiveWorkbook.Save()
            End If
        Next        '---------------------------------------------------------------
        xlibro.Sheets("motonave").Select()
        lRow = xlibro.ActiveSheet.UsedRange.Rows.Count
        For fila As Integer = 1 To lRow
            If String.IsNullOrEmpty(xlibro.Range("A" & fila).Value) Then
                xlibro.Range("A" & fila).Select()
                xlibro.Range("A" & fila & ":Z" & fila).Delete()
                xlibro.ActiveWorkbook.Save()
            End If
        Next        '-------------------------------------------------------------
        xlibro.Sheets("motonaves").Select()
        lRow = xlibro.ActiveSheet.UsedRange.Rows.Count
        For fila As Integer = 1 To lRow
            If String.IsNullOrEmpty(xlibro.Range("A" & fila).Value) Then
                xlibro.Range("A" & fila).Select()
                xlibro.Range("A" & fila & ":Z" & fila).Delete()
                xlibro.ActiveWorkbook.Save()
            End If
        Next        '----------------------------------------------------------------
        xlibro.Sheets("motonaves").Select()
        lRow = xlibro.ActiveSheet.UsedRange.Rows.Count
        For fila As Integer = 1 To lRow
            If String.IsNullOrEmpty(xlibro.Range("A" & fila).Value) Then
                xlibro.Range("A" & fila).Select()
                xlibro.Range("A" & fila & ":Z" & fila).Delete()
                xlibro.ActiveWorkbook.Save()
            End If
        Next        '--------------------------------------------------------------
        xlibro.Sheets("detalle_motonaves").Select()
        lRow = xlibro.ActiveSheet.UsedRange.Rows.Count
        For fila As Integer = 1 To lRow
            If String.IsNullOrEmpty(xlibro.Range("A" & fila).Value) Then
                xlibro.Range("A" & fila).Select()
                xlibro.Range("A" & fila & ":Z" & fila).Delete()
                xlibro.ActiveWorkbook.Save()
            End If
        Next
    End Sub
End Module

