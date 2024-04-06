Imports System.Data.SqlClient

Public Class JourneyDetailsAdd
    Dim connectionString As String = "Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\pranj\Desktop\BusTicketManagementSystem\BusTicketManagementSystem\BusManagementDatabase.mdf;Integrated Security=True"
    Dim con As New SqlConnection(connectionString)
    Private Function IsAllRegisterBusFieldsFilled() As Boolean
        ' Check if all required fields are filled
        Return Not (
            String.IsNullOrWhiteSpace(Txt_Departure.Text) OrElse
            String.IsNullOrWhiteSpace(Txt_SourceLocation.Text) OrElse
            String.IsNullOrWhiteSpace(Txt_Destination.Text) OrElse
            String.IsNullOrWhiteSpace(Txt_ArrivalTime.Text) OrElse
            String.IsNullOrWhiteSpace(TextBox1.Text) OrElse
            String.IsNullOrWhiteSpace(Txt_JourneyName.Text))
        ' Optionally, you can remove additional conditions (CheckBoxInternetBanking, CheckBoxMobilrBanking, CheckBoxChequebook, CheckBoxemailAlerts, CheckBoxestatement)
    End Function
    Private Sub ClearAllRegisterBusControls()
        ' Clear all input controls
        Dim controlsToClear() As Control = {Txt_Departure, Txt_SourceLocation, Txt_Destination, Txt_ArrivalTime, Txt_JourneyName}
        For Each control As Control In controlsToClear
            control.Text = String.Empty
        Next
    End Sub

    Private Sub Guna2GradientButton1_Click(sender As Object, e As EventArgs) Handles Guna2GradientButton1.Click
        If Not IsAllRegisterBusFieldsFilled() Then
            MsgBox("Please fill in all the Journey details.")
            Exit Sub
        End If
        If con.State = ConnectionState.Open Then
            con.Close()
        End If
        If CHECKeMAIL(con, Txt_JourneyName.Text, "JourneyData", "JourneyName") = False Then
            'MsgBox("Email does not exist. You can proceed.")
        Else
            MsgBox("You Cant Re Enter A Same Journey Routes That Already Exist.")
            Exit Sub
        End If
        Try
            con.Open()
            Dim query As String = "INSERT INTO JourneyData (JourneyName, SourceLocation, Departure,Destination,ArrivalTime,JourneyPrice,BusName,BusType) " &
                        "VALUES (@JourneyName, @SourceLocation, @Departure,@Destination,@ArrivalTime,@JourneyPrice,@BusName,@BusType)"
            Using cmd As New SqlCommand(query, con)
                ' Set parameter values...'
                cmd.Parameters.AddWithValue("@JourneyName", If(Not String.IsNullOrEmpty(Txt_JourneyName.Text), Txt_JourneyName.Text, DBNull.Value))
                cmd.Parameters.AddWithValue("@SourceLocation", If(Not String.IsNullOrEmpty(Txt_SourceLocation.Text), Txt_SourceLocation.Text, DBNull.Value))
                cmd.Parameters.AddWithValue("@Departure", If(Not String.IsNullOrEmpty(Txt_Departure.Text), Txt_Departure.Text, DBNull.Value))
                cmd.Parameters.AddWithValue("@Destination", If(Not String.IsNullOrEmpty(Txt_Destination.Text), Txt_Destination.Text, DBNull.Value))
                cmd.Parameters.AddWithValue("@ArrivalTime", If(Not String.IsNullOrEmpty(Txt_ArrivalTime.Text), Txt_ArrivalTime.Text, DBNull.Value))
                cmd.Parameters.AddWithValue("@JourneyPrice", If(Not String.IsNullOrEmpty(TextBox1.Text), TextBox1.Text, DBNull.Value))
                cmd.Parameters.AddWithValue("@BusName", If(Txt_BusName.SelectedIndex <> -1, Txt_BusName.SelectedItem.ToString(), DBNull.Value))
                cmd.Parameters.AddWithValue("@BusType", If(Not String.IsNullOrEmpty(TextBox2.Text), TextBox2.Text, DBNull.Value))

                ' Execute the query...'
                cmd.ExecuteNonQuery()
                MsgBox("New Journey Data has been scusses fully Registered.")
                ClearAllRegisterBusControls()
            End Using
        Catch ex As Exception
            ' Display specific error message...
            MsgBox("Error: " & ex.Message)
        End Try
        If con.State = ConnectionState.Open Then
            con.Close()
        End If
        ' Finally, repopulate the DataGridView and generate a unique invoice number
        Populatedvg(con, "JourneyData", DataGridView1)
    End Sub

    Private Sub RegisterForm_Load(sender As Object, e As EventArgs) Handles Me.Load
        Populatedvg(con, "JourneyData", DataGridView1)
        FillcomboBox(con, Txt_BusName, "BusData", "BusName")

    End Sub



    Private Sub Txt_BusName_SelectionChangeCommitted(sender As Object, e As EventArgs) Handles Txt_BusName.SelectionChangeCommitted
        If (Txt_BusName.SelectedIndex <> -1) Then
            con.Open()
            Try
                ' Check if the staff ID exists in the database
                Dim query As String = "SELECT * FROM BusData WHERE BusName = @BusName"
                Using cmd As New SqlCommand(query, con)
                    cmd.Parameters.AddWithValue("@BusName", Txt_BusName.SelectedValue.ToString())
                    ' Execute the query and check if any rows were returned
                    Dim reader As SqlDataReader = cmd.ExecuteReader()
                    If reader.HasRows Then
                        reader.Read()
                        ' Populate the fields with retrieved data
                        TextBox2.Text = reader("BusType") ' Replace "ColumnName" with the actual column name
                    End If
                End Using
            Catch ex As Exception
                ' Handle the exception (e.g., display an error message)
                MessageBox.Show("An error occurred: " & ex.Message)
            Finally
                con.Close()
            End Try
        End If
    End Sub

End Class