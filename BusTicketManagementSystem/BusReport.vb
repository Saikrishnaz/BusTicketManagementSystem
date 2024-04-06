Imports System.Data.SqlClient

Public Class BusReport
    Dim connectionString As String = ReturnConnectionString()
    Dim con As New SqlConnection(connectionString)
    Private Sub BusReport_Load(sender As Object, e As EventArgs) Handles Me.Load
        Populatedvg(con, "BusData", DataGridView1)
        AutoCompleteSearchBoxForTextBoxesTypeString(con, Txt_BusName, "BusName", "BusData", 0)
    End Sub

    Private Sub Txt_BusName_TextChanged(sender As Object, e As EventArgs) Handles Txt_BusName.TextChanged
        If Txt_BusName.Text = "" Then
            Populatedvg(con, "BusData", DataGridView1)
        Else
            DataGridView1.Columns.Clear()

            Try
                con.Open()

                ' Use parameterized query to avoid SQL injection
                Dim sql As String = "SELECT * FROM BusData WHERE BusName = @BusName"
                Dim adapter As SqlDataAdapter = New SqlDataAdapter(sql, con)
                adapter.SelectCommand.Parameters.AddWithValue("@BusName", Txt_BusName.Text)

                Dim ds As DataSet = New DataSet()
                adapter.Fill(ds, "BusData")

                ' Assign the DataTable to the DataGridView's DataSource
                DataGridView1.DataSource = ds.Tables("BusData")

                con.Close()

            Catch ex As Exception
                ' Handle exceptions appropriately
                MessageBox.Show("Error: " & ex.Message)
                con.Close()
            End Try
            con.Close()
        End If
    End Sub

    Private Sub Guna2GradientButton1_Click(sender As Object, e As EventArgs) Handles Guna2GradientButton1.Click
        If Not Txt_BusName.Text = "" Then
            Try
                con.Open()
                Dim query As String = "DELETE FROM BusData WHERE BusName = @BusName"
                Using cmd As New SqlCommand(query, con)
                    ' Set parameter values...'
                    cmd.Parameters.AddWithValue("@BusName", If(Not String.IsNullOrEmpty(Txt_BusName.Text), Txt_BusName.Text, DBNull.Value))

                    ' Execute the query...'
                    Dim rowsAffected As Integer = cmd.ExecuteNonQuery()
                    If rowsAffected > 0 Then
                        MsgBox("Bus Data has been successfully deleted.")
                        Txt_BusName.Clear()
                    Else
                        MsgBox("No matching record found for the provided BusName.")
                    End If
                End Using
                con.Close()

            Catch ex As Exception
                ' Display specific error message...
                MsgBox("Error: " & ex.Message)
            Finally
                If con.State = ConnectionState.Open Then
                    con.Close()
                End If
            End Try
        End If
        Populatedvg(con, "BusData", DataGridView1)
    End Sub
End Class