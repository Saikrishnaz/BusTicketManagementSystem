Imports System.Data.SqlClient

Public Class BookedTicketReport
    Dim connectionString As String = ReturnConnectionString()
    Dim con As New SqlConnection(connectionString)
    Private Sub BusReport_Load(sender As Object, e As EventArgs) Handles Me.Load
        Populatedvg(con, "BookedTicketData", DataGridView1)
        AutoCompleteSearchBoxForTextBoxesTypeString(con, Txt_BusName, "TicketNumber", "BookedTicketData", 0)
    End Sub

    Private Sub Txt_BusName_TextChanged(sender As Object, e As EventArgs) Handles Txt_BusName.TextChanged
        If Txt_BusName.Text = "" Then
            Populatedvg(con, "BookedTicketData", DataGridView1)
        Else
            DataGridView1.Columns.Clear()

            Try
                con.Open()

                ' Use parameterized query to avoid SQL injection
                Dim sql As String = "SELECT * FROM BookedTicketData WHERE TicketNumber = @TicketNumber"
                Dim adapter As SqlDataAdapter = New SqlDataAdapter(sql, con)
                adapter.SelectCommand.Parameters.AddWithValue("@TicketNumber", Txt_BusName.Text)

                Dim ds As DataSet = New DataSet()
                adapter.Fill(ds, "BookedTicketData")

                ' Assign the DataTable to the DataGridView's DataSource
                DataGridView1.DataSource = ds.Tables("BookedTicketData")

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

                Dim query As String = "DELETE FROM BookedTicketData WHERE TicketNumber = @TicketNumber"
                Using cmd As New SqlCommand(query, con)
                        ' Set parameter values...'
                        cmd.Parameters.AddWithValue("@TicketNumber", If(Not String.IsNullOrEmpty(Txt_BusName.Text), Txt_BusName.Text, DBNull.Value))

                    ' Execute the query...'
                    con.Open()
                    Dim rowsAffected As Integer = cmd.ExecuteNonQuery()
                    con.Close()

                    If rowsAffected > 0 Then
                            MsgBox("Bus Data has been successfully deleted.")
                            Txt_BusName.Clear()
                        Else
                            MsgBox("No matching record found for the provided TicketNumber.")
                        End If
                    End Using
                If con.State = ConnectionState.Open Then
                    con.Close()
                End If
                ' Repopulate the DataGridView after deletion
                Populatedvg(con, "BookedTicketData", DataGridView1)
            Catch ex As Exception
                ' Display specific error message...
                MsgBox("Error: " & ex.Message)
            End Try
        End If

    End Sub

    Private Sub Guna2GradientPanel3_Paint(sender As Object, e As PaintEventArgs) Handles Guna2GradientPanel3.Paint

    End Sub

    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click

    End Sub
End Class