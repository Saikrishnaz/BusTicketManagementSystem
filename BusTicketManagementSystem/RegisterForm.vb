Imports System.Data.SqlClient

Public Class RegisterForm

    Dim connectionString As String = ReturnConnectionString()
    Dim con As New SqlConnection(connectionString)
    Private Function IsAllRegisterBusFieldsFilled() As Boolean
        ' Check if all required fields are filled
        Return Not (
            String.IsNullOrWhiteSpace(Txt_BusChasisNo.Text) OrElse
            String.IsNullOrWhiteSpace(Txt_BusName.Text) OrElse
            String.IsNullOrWhiteSpace(Txt_BusNo.Text) OrElse
            String.IsNullOrWhiteSpace(Txt_NoOfSeat.Text) OrElse
            Txt_BusFuelType.SelectedIndex = -1 OrElse
            Txt_BusType.SelectedIndex = -1)
        ' Optionally, you can remove additional conditions (CheckBoxInternetBanking, CheckBoxMobilrBanking, CheckBoxChequebook, CheckBoxemailAlerts, CheckBoxestatement)
    End Function
    Private Sub ClearAllRegisterBusControls()
        ' Clear all input controls
        Dim controlsToClear() As Control = {Txt_BusChasisNo, Txt_BusName, Txt_BusNo, Txt_NoOfSeat}
        For Each control As Control In controlsToClear
            control.Text = String.Empty
        Next
        Txt_BusFuelType.SelectedIndex = -1
        Txt_BusType.SelectedIndex = -1
    End Sub

    Private Sub Guna2GradientButton1_Click(sender As Object, e As EventArgs) Handles Guna2GradientButton1.Click
        If Not IsAllRegisterBusFieldsFilled() Then
            MsgBox("Please fill in all the Bus details.")
            Exit Sub
        End If
        If con.State = ConnectionState.Open Then
            con.Close()
        End If
        If CHECKeMAIL(con, Txt_BusNo.Text, "BusData", "BusNo") = False Then
            'MsgBox("Email does not exist. You can proceed.")
        Else
            MsgBox("You Cant Re Enter A BusNo That Already Exist.")
            Exit Sub
        End If
        If CHECKeMAIL(con, Txt_BusChasisNo.Text, "BusData", "BusChasisNo") = False Then
            'MsgBox("Email does not exist. You can proceed.")
        Else
            MsgBox("Bus With Same Chasis Already Exist.")
            Exit Sub
        End If
        Try
            con.Open()
            Dim query As String = "INSERT INTO BusData (BusType, BusName, BusChasisNo,BusNo,FuelType,NoOfSeats) " &
                        "VALUES (@BusType, @BusName, @BusChasisNo,@BusNo,@FuelType,@NoOfSeats)"
            Using cmd As New SqlCommand(query, con)
                ' Set parameter values...'
                cmd.Parameters.AddWithValue("@BusType", If(Txt_BusType.SelectedIndex <> -1, Txt_BusType.SelectedItem.ToString(), DBNull.Value))
                cmd.Parameters.AddWithValue("@BusName", If(Not String.IsNullOrEmpty(Txt_BusName.Text), Txt_BusName.Text, DBNull.Value))
                cmd.Parameters.AddWithValue("@BusChasisNo", If(Not String.IsNullOrEmpty(Txt_BusChasisNo.Text), Txt_BusChasisNo.Text, DBNull.Value))
                cmd.Parameters.AddWithValue("@BusNo", If(Not String.IsNullOrEmpty(Txt_BusNo.Text), Txt_BusNo.Text, DBNull.Value))
                cmd.Parameters.AddWithValue("@FuelType", If(Txt_BusFuelType.SelectedIndex <> -1, Txt_BusFuelType.SelectedItem.ToString(), DBNull.Value))
                cmd.Parameters.AddWithValue("@NoOfSeats", If(Not String.IsNullOrEmpty(Txt_NoOfSeat.Text), Txt_NoOfSeat.Text, DBNull.Value))

                ' Execute the query...'
                cmd.ExecuteNonQuery()
                MsgBox("New Bus Data has been scusses fully Registered.")
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
        Populatedvg(con, "BusData", DataGridView1)
    End Sub

    Private Sub RegisterForm_Load(sender As Object, e As EventArgs) Handles Me.Load
        Populatedvg(con, "BusData", DataGridView1)

    End Sub

    Private Sub Guna2GradientPanel3_Paint(sender As Object, e As PaintEventArgs) Handles Guna2GradientPanel3.Paint

    End Sub
End Class