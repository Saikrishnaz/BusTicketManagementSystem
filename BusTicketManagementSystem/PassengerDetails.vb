Imports System.Data.SqlClient

Public Class PassengerDetails
    Dim connectionString As String = ReturnConnectionString()
    Dim con As New SqlConnection(connectionString)
    Private Function IsAllPassengerFieldsFilled() As Boolean
        ' Check if all required fields are filled
        Return Not (
            String.IsNullOrWhiteSpace(Txt_EmergnPsgnMobile.Text) OrElse
            String.IsNullOrWhiteSpace(Txt_EmergnPsgnName.Text) OrElse
            String.IsNullOrWhiteSpace(Txt_EmergnPsgnRelation.Text) OrElse
            String.IsNullOrWhiteSpace(Txt_PsgnCity.Text) OrElse
            String.IsNullOrWhiteSpace(Txt_PsgnAge.Text) OrElse
            String.IsNullOrWhiteSpace(Txt_PsgnEmail.Text) OrElse
            String.IsNullOrWhiteSpace(Txt_PsgnMobile.Text) OrElse
            String.IsNullOrWhiteSpace(Txt_PsgnName.Text) OrElse
            String.IsNullOrWhiteSpace(Txt_PsgnPinCode.Text) OrElse
            Txt_PsgnState.SelectedIndex = -1 OrElse
            Txt_PsgnGender.SelectedIndex = -1 OrElse
            Txt_PsgnNationality.SelectedIndex = -1)
        ' Optionally, you can remove additional conditions (CheckBoxInternetBanking, CheckBoxMobilrBanking, CheckBoxChequebook, CheckBoxemailAlerts, CheckBoxestatement)
    End Function
    Private Sub ClearAllPassengerBusControls()
        ' Clear all input controls
        Dim controlsToClear() As Control = {Txt_EmergnPsgnName, Txt_EmergnPsgnRelation, Txt_PsgnCity, Txt_PsgnAge, Txt_PsgnEmail, Txt_PsgnMobile, Txt_PsgnName, Txt_PsgnPinCode, Txt_EmergnPsgnMobile}
        For Each control As Control In controlsToClear
            control.Text = String.Empty
        Next
        Txt_PsgnState.SelectedIndex = -1
        Txt_PsgnGender.SelectedIndex = -1
        Txt_PsgnNationality.SelectedIndex = -1
    End Sub

    Private Sub Guna2GradientButton3_Click(sender As Object, e As EventArgs) Handles Guna2GradientButton3.Click
        If Not IsAllPassengerFieldsFilled() Then
            MsgBox("Please fill in all the Passenger details.")
            Exit Sub
        End If
        If con.State = ConnectionState.Open Then
            con.Close()
        End If
        If CHECKeMAIL(con, Txt_PsgnMobile.Text, "PassengerData", "MobileNo") = False Then
            'MsgBox("Email does not exist. You can proceed.")
        Else
            MsgBox("Passenger with same Mobile Number Already Exist..")
            Exit Sub
        End If
        If CHECKeMAIL(con, Txt_PsgnEmail.Text, "PassengerData", "EnailID") = False Then
            'MsgBox("Email does not exist. You can proceed.")
        Else
            MsgBox("Passenger with same Email ID Already Exist..")
            Exit Sub
        End If
        Try
            con.Open()
            Dim query As String = "INSERT INTO PassengerData (Name, Gender, Age,EnailID,MobileNo,Nationality,States,CityName,PinCode,EmergenceName,EmergenceMobileNo,RelationShip) " &
                        "VALUES (@Name, @Gender, @Age,@EnailID,@MobileNo,@Nationality,@States,@CityName,@PinCode,@EmergenceName,@EmergenceMobileNo,@RelationShip)"
            Using cmd As New SqlCommand(query, con)
                ' Set parameter values...'
                cmd.Parameters.AddWithValue("@Name", If(Not String.IsNullOrEmpty(Txt_PsgnName.Text), Txt_PsgnName.Text, DBNull.Value))
                cmd.Parameters.AddWithValue("@Gender", If(Txt_PsgnGender.SelectedIndex <> -1, Txt_PsgnGender.SelectedItem.ToString(), DBNull.Value))
                cmd.Parameters.AddWithValue("@Age", If(Not String.IsNullOrEmpty(Txt_PsgnAge.Text), Txt_PsgnAge.Text, DBNull.Value))
                cmd.Parameters.AddWithValue("@EnailID", If(Not String.IsNullOrEmpty(Txt_PsgnEmail.Text), Txt_PsgnEmail.Text, DBNull.Value))
                cmd.Parameters.AddWithValue("@MobileNo", If(Not String.IsNullOrEmpty(Txt_PsgnMobile.Text), Txt_PsgnMobile.Text, DBNull.Value))
                cmd.Parameters.AddWithValue("@Nationality", If(Txt_PsgnNationality.SelectedIndex <> -1, Txt_PsgnNationality.SelectedItem.ToString(), DBNull.Value))
                cmd.Parameters.AddWithValue("@States", If(Txt_PsgnState.SelectedIndex <> -1, Txt_PsgnState.SelectedItem.ToString(), DBNull.Value))
                cmd.Parameters.AddWithValue("@CityName", If(Not String.IsNullOrEmpty(Txt_PsgnCity.Text), Txt_PsgnCity.Text, DBNull.Value))
                cmd.Parameters.AddWithValue("@PinCode", If(Not String.IsNullOrEmpty(Txt_PsgnPinCode.Text), Txt_PsgnPinCode.Text, DBNull.Value))
                cmd.Parameters.AddWithValue("@EmergenceName", If(Not String.IsNullOrEmpty(Txt_EmergnPsgnName.Text), Txt_EmergnPsgnName.Text, DBNull.Value))
                cmd.Parameters.AddWithValue("@EmergenceMobileNo", If(Not String.IsNullOrEmpty(Txt_EmergnPsgnMobile.Text), Txt_EmergnPsgnMobile.Text, DBNull.Value))
                cmd.Parameters.AddWithValue("@RelationShip", If(Not String.IsNullOrEmpty(Txt_EmergnPsgnRelation.Text), Txt_EmergnPsgnRelation.Text, DBNull.Value))

                ' Execute the query...'
                cmd.ExecuteNonQuery()
                MsgBox("New Pessenger Data has been scusses fully Registered.")
                ClearAllPassengerBusControls()
            End Using
        Catch ex As Exception
            ' Display specific error message...
            MsgBox("Error: " & ex.Message)
        End Try
        If con.State = ConnectionState.Open Then
            con.Close()
        End If
        ' Finally, repopulate the DataGridView and generate a unique invoice number
        Populatedvg(con, "PassengerData", DataGridView1)
    End Sub

    Private Sub PassengerDetails_Load(sender As Object, e As EventArgs) Handles Me.Load
        Populatedvg(con, "PassengerData", DataGridView1)
    End Sub

    Private Sub Guna2GradientButton2_Click(sender As Object, e As EventArgs) Handles Guna2GradientButton2.Click
        ClearAllPassengerBusControls()
        DataGridView1.ForeColor = Color.Black
    End Sub

    Private Sub Guna2GradientPanel3_Paint(sender As Object, e As PaintEventArgs) Handles Guna2GradientPanel3.Paint

    End Sub

    Private Sub Txt_PsgnMobile_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Txt_PsgnMobile.KeyPress
        If Not Char.IsControl(e.KeyChar) AndAlso Not Char.IsDigit(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

    Private Sub Txt_PsgnPinCode_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Txt_PsgnPinCode.KeyPress
        If Not Char.IsControl(e.KeyChar) AndAlso Not Char.IsDigit(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

    Private Sub Txt_EmergnPsgnMobile_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Txt_EmergnPsgnMobile.KeyPress
        If Not Char.IsControl(e.KeyChar) AndAlso Not Char.IsDigit(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

    Private Sub Txt_PsgnMobile_TextChanged(sender As Object, e As EventArgs) Handles Txt_PsgnMobile.TextChanged
        checkNum(Txt_PsgnMobile)
    End Sub

    Private Sub Txt_EmergnPsgnMobile_TextChanged(sender As Object, e As EventArgs) Handles Txt_EmergnPsgnMobile.TextChanged
        checkNum(Txt_EmergnPsgnMobile)
    End Sub

    Private Sub Txt_PsgnAge_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Txt_PsgnAge.KeyPress
        If Not Char.IsControl(e.KeyChar) AndAlso Not Char.IsDigit(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub
End Class