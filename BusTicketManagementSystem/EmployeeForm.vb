Imports System.Data.SqlClient

Public Class EmployeeForm
    Dim connectionString As String = ReturnConnectionString()
    Dim con As New SqlConnection(connectionString)
    Private Function IsAllEmployeeFieldsFilled() As Boolean
        ' Check if all required fields are filled
        Return Not (
            String.IsNullOrWhiteSpace(Txt_EmpPassword.Text) OrElse
            String.IsNullOrWhiteSpace(Txt_EmpUserName.Text) OrElse
            String.IsNullOrWhiteSpace(Txt_EmpAddress.Text) OrElse
            String.IsNullOrWhiteSpace(Txt_EmpAge.Text) OrElse
            String.IsNullOrWhiteSpace(Txt_EmpAddarNo.Text) OrElse
            String.IsNullOrWhiteSpace(Txt_EmpEmail.Text) OrElse
            String.IsNullOrWhiteSpace(Txt_EmpMobile.Text) OrElse
            String.IsNullOrWhiteSpace(Txt_EmpName.Text) OrElse
            Txt_EmpGender.SelectedIndex = -1 OrElse
            Txt_EmpQualification.SelectedIndex = -1)
        ' Optionally, you can remove additional conditions (CheckBoxInternetBanking, CheckBoxMobilrBanking, CheckBoxChequebook, CheckBoxemailAlerts, CheckBoxestatement)
    End Function
    Private Sub ClearAllEmployeeBusControls()
        ' Clear all input controls
        Dim controlsToClear() As Control = {Txt_EmpUserName, Txt_EmpAddarNo, Txt_EmpAddress, Txt_EmpAge, Txt_EmpEmail, Txt_EmpMobile, Txt_EmpName, Txt_EmpPassword}
        For Each control As Control In controlsToClear
            control.Text = String.Empty
        Next
        Txt_EmpGender.SelectedIndex = -1
        Txt_EmpQualification.SelectedIndex = -1
    End Sub

    Private Sub Guna2GradientButton3_Click(sender As Object, e As EventArgs) Handles Guna2GradientButton3.Click
        If Radio_Register.Checked Then


            If Not IsAllEmployeeFieldsFilled() Then
                MsgBox("Please fill in all the Employee details.")
                Exit Sub
            End If
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
            If CHECKeMAIL(con, Txt_EmpMobile.Text, "EmployeeData", "MobileNo") = False Then
                'MsgBox("Email does not exist. You can proceed.")
            Else
                MsgBox("Employee with same Mobile Number Already Exist..")
                Exit Sub
            End If
            If CHECKeMAIL(con, Txt_EmpEmail.Text, "EmployeeData", "EnailID") = False Then
                'MsgBox("Email does not exist. You can proceed.")
            Else
                MsgBox("Employee with same Email ID Already Exist..")
                Exit Sub
            End If
            If CHECKeMAIL(con, Txt_EmpUserName.Text, "EmployeeData", "UserName") = False Then
                'MsgBox("Email does not exist. You can proceed.")
            Else
                MsgBox("Employee with same UserName ID Already Exist..")
                Exit Sub
            End If
            Try
                con.Open()
                Dim query As String = "INSERT INTO EmployeeData (Name, Gender, Age,EnailID,MobileNo,Qualification,AddharNo,Address,UserName,Password) " &
                            "VALUES (@Name, @Gender, @Age,@EnailID,@MobileNo,@Qualification,@AddharNo,@Address,@UserName,@Password)"
                Using cmd As New SqlCommand(query, con)
                    ' Set parameter values...'
                    cmd.Parameters.AddWithValue("@Name", If(Not String.IsNullOrEmpty(Txt_EmpName.Text), Txt_EmpName.Text, DBNull.Value))
                    cmd.Parameters.AddWithValue("@Gender", If(Txt_EmpGender.SelectedIndex <> -1, Txt_EmpGender.SelectedItem.ToString(), DBNull.Value))
                    cmd.Parameters.AddWithValue("@Age", If(Not String.IsNullOrEmpty(Txt_EmpAge.Text), Txt_EmpAge.Text, DBNull.Value))
                    cmd.Parameters.AddWithValue("@EnailID", If(Not String.IsNullOrEmpty(Txt_EmpEmail.Text), Txt_EmpEmail.Text, DBNull.Value))
                    cmd.Parameters.AddWithValue("@MobileNo", If(Not String.IsNullOrEmpty(Txt_EmpMobile.Text), Txt_EmpMobile.Text, DBNull.Value))
                    cmd.Parameters.AddWithValue("@Qualification", If(Txt_EmpQualification.SelectedIndex <> -1, Txt_EmpQualification.SelectedItem.ToString(), DBNull.Value))
                    cmd.Parameters.AddWithValue("@AddharNo", If(Not String.IsNullOrEmpty(Txt_EmpAddarNo.Text), Txt_EmpAddarNo.Text, DBNull.Value))
                    cmd.Parameters.AddWithValue("@Address", If(Not String.IsNullOrEmpty(Txt_EmpAddress.Text), Txt_EmpAddress.Text, DBNull.Value))
                    cmd.Parameters.AddWithValue("@UserName", If(Not String.IsNullOrEmpty(Txt_EmpUserName.Text), Txt_EmpUserName.Text, DBNull.Value))
                    cmd.Parameters.AddWithValue("@Password", If(Not String.IsNullOrEmpty(Txt_EmpPassword.Text), Txt_EmpPassword.Text, DBNull.Value))

                    ' Execute the query...'
                    cmd.ExecuteNonQuery()
                    MsgBox("New Employee Data has been scusses fully Registered.")
                    ClearAllEmployeeBusControls()
                End Using
            Catch ex As Exception
                ' Display specific error message...
                MsgBox("Error: " & ex.Message)
            End Try
            If con.State = ConnectionState.Open Then
                con.Close()
            End If



        ElseIf Radio_UpdateDetail.Checked Then


            If Not IsAllEmployeeFieldsFilled() Then
                MsgBox("Please fill in all the Employee details.")
                Exit Sub
            End If

            If con.State = ConnectionState.Open Then
                con.Close()
            End If

            Try
                con.Open()


                ' Update query to modify the existing employee data
                Dim query As String = "UPDATE EmployeeData SET Name = @Name, Gender = @Gender, Age = @Age, EnailID = @EnailID, " &
                          "MobileNo = @MobileNo, Qualification = @Qualification, AddharNo = @AddharNo, " &
                          "Address = @Address, Password = @Password " &
                          "WHERE UserName = @UserName " ' Assuming EmployeeID is the unique identifier

                Using cmd As New SqlCommand(query, con)
                    cmd.Parameters.AddWithValue("@Name", If(Not String.IsNullOrEmpty(Txt_EmpName.Text), Txt_EmpName.Text, DBNull.Value))
                    cmd.Parameters.AddWithValue("@Gender", If(Txt_EmpGender.SelectedIndex <> -1, Txt_EmpGender.SelectedItem.ToString(), DBNull.Value))
                    cmd.Parameters.AddWithValue("@Age", If(Not String.IsNullOrEmpty(Txt_EmpAge.Text), Txt_EmpAge.Text, DBNull.Value))
                    cmd.Parameters.AddWithValue("@EnailID", If(Not String.IsNullOrEmpty(Txt_EmpEmail.Text), Txt_EmpEmail.Text, DBNull.Value))
                    cmd.Parameters.AddWithValue("@MobileNo", If(Not String.IsNullOrEmpty(Txt_EmpMobile.Text), Txt_EmpMobile.Text, DBNull.Value))
                    cmd.Parameters.AddWithValue("@Qualification", If(Txt_EmpQualification.SelectedIndex <> -1, Txt_EmpQualification.SelectedItem.ToString(), DBNull.Value))
                    cmd.Parameters.AddWithValue("@AddharNo", If(Not String.IsNullOrEmpty(Txt_EmpAddarNo.Text), Txt_EmpAddarNo.Text, DBNull.Value))
                    cmd.Parameters.AddWithValue("@Address", If(Not String.IsNullOrEmpty(Txt_EmpAddress.Text), Txt_EmpAddress.Text, DBNull.Value))
                    cmd.Parameters.AddWithValue("@UserName", If(Not String.IsNullOrEmpty(Txt_EmpUserName.Text), Txt_EmpUserName.Text, DBNull.Value))
                    cmd.Parameters.AddWithValue("@Password", If(Not String.IsNullOrEmpty(Txt_EmpPassword.Text), Txt_EmpPassword.Text, DBNull.Value))

                    ' Execute the update query
                    cmd.ExecuteNonQuery()
                    MsgBox("Employee Data has been updated successfully.")
                    ClearAllEmployeeBusControls()
                End Using
            Catch ex As Exception
                ' Display specific error message...
                MsgBox("Error: " & ex.Message)
            Finally
                If con.State = ConnectionState.Open Then
                    con.Close()
                End If
            End Try
        Else
            MsgBox("Please select the Action Type Register Or Update")
        End If

        ' Finally, repopulate the DataGridView and generate a unique invoice number
        Populatedvg(con, "EmployeeData", DataGridView1)
    End Sub

    Private Sub PassengerDetails_Load(sender As Object, e As EventArgs) Handles Me.Load
        Populatedvg(con, "EmployeeData", DataGridView1)
        AutoCompleteSearchBoxForTextBoxesTypeString(con, Txt_BusName, "UserName", "EmployeeData", 0)
    End Sub

    Private Sub Guna2GradientButton2_Click(sender As Object, e As EventArgs) Handles Guna2GradientButton2.Click
        ClearAllEmployeeBusControls()
        DataGridView1.ForeColor = Color.Black
    End Sub

    Private Sub Txt_BusName_TextChanged(sender As Object, e As EventArgs) Handles Txt_BusName.TextChanged
        If Txt_BusName.Text = "" Then
            Populatedvg(con, "EmployeeData", DataGridView1)
        Else
            DataGridView1.Columns.Clear()

            Try
                con.Open()

                ' Use parameterized query to avoid SQL injection
                Dim sql As String = "SELECT * FROM EmployeeData WHERE UserName = @UserName"
                Dim adapter As SqlDataAdapter = New SqlDataAdapter(sql, con)
                adapter.SelectCommand.Parameters.AddWithValue("@UserName", Txt_BusName.Text)

                Dim ds As DataSet = New DataSet()
                adapter.Fill(ds, "EmployeeData")

                ' Assign the DataTable to the DataGridView's DataSource
                DataGridView1.DataSource = ds.Tables("EmployeeData")

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
                Dim query As String = "DELETE FROM EmployeeData WHERE UserName = @UserName"
                Using cmd As New SqlCommand(query, con)
                    ' Set parameter values...'
                    cmd.Parameters.AddWithValue("@UserName", If(Not String.IsNullOrEmpty(Txt_BusName.Text), Txt_BusName.Text, DBNull.Value))

                    ' Execute the query...'
                    Dim rowsAffected As Integer = cmd.ExecuteNonQuery()
                    If rowsAffected > 0 Then
                        MsgBox("Employee Data has been successfully deleted.")
                        Txt_BusName.Clear()
                    Else
                        MsgBox("No matching record found for the provided Passemger Name.")
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
        Populatedvg(con, "EmployeeData", DataGridView1)
    End Sub

    Private Sub Radio_UpdateDetail_CheckedChanged(sender As Object, e As EventArgs) Handles Radio_UpdateDetail.CheckedChanged
        AutoCompleteSearchBoxForTextBoxesTypeString(con, Txt_EmpUserName, "UserName", "EmployeeData", 0)
        Txt_EmpAddress.Enabled = False
        Txt_EmpAge.Enabled = False
        Txt_EmpAddress.Enabled = False
        Txt_EmpEmail.Enabled = False
        Txt_EmpMobile.Enabled = False
        Txt_EmpName.Enabled = False
        Txt_EmpGender.Enabled = False
        Txt_EmpQualification.Enabled = False
        ClearAllEmployeeBusControls()
    End Sub

    Private Sub Txt_EmpPassword_TextChanged(sender As Object, e As EventArgs) Handles Txt_EmpPassword.TextChanged
        If Radio_UpdateDetail.Checked Then
            Dim enteredPassword As String = Txt_EmpPassword.Text.Trim()

            If Not String.IsNullOrEmpty(Txt_EmpUserName.Text) Then
                If Not String.IsNullOrEmpty(Txt_EmpPassword.Text) Then
                    Dim storedPassword As String = ""

                    ' Assuming con is a valid SqlConnection object
                    Try
                        con.Open()
                        ' Check if the staff ID exists in the database
                        Dim query As String = "SELECT Password FROM EmployeeData WHERE UserName = @UserName"
                        Using cmd As New SqlCommand(query, con)
                            cmd.Parameters.AddWithValue("@UserName", Txt_EmpUserName.Text)
                            ' Execute the query to retrieve the stored password
                            Dim result As Object = cmd.ExecuteScalar()
                            If result IsNot Nothing Then
                                storedPassword = result.ToString()
                            End If
                        End Using
                    Catch ex As Exception
                        ' Handle the exception (e.g., display an error message)
                        MessageBox.Show("An error occurred: " & ex.Message)
                    Finally
                        con.Close()
                    End Try

                    ' Check if the entered password matches the stored password
                    If String.Equals(enteredPassword, storedPassword, StringComparison.OrdinalIgnoreCase) Then
                        ' Enable the textboxes for editing employee details
                        Txt_EmpAddress.Enabled = True
                        Txt_EmpAge.Enabled = True
                        Txt_EmpAddress.Enabled = True
                        Txt_EmpEmail.Enabled = True
                        Txt_EmpMobile.Enabled = True
                        Txt_EmpName.Enabled = True
                        Txt_EmpGender.Enabled = True
                        Txt_EmpQualification.Enabled = True
                    Else
                        ' Passwords don't match, so disable the textboxes
                        Txt_EmpAddress.Enabled = False
                        Txt_EmpAge.Enabled = False
                        Txt_EmpAddress.Enabled = False
                        Txt_EmpEmail.Enabled = False
                        Txt_EmpMobile.Enabled = False
                        Txt_EmpName.Enabled = False
                        Txt_EmpGender.Enabled = False
                        Txt_EmpQualification.Enabled = False
                    End If
                End If
            End If
        End If
    End Sub


    Private Sub Txt_EmpUserName_TextChanged(sender As Object, e As EventArgs) Handles Txt_EmpUserName.TextChanged
        If Radio_UpdateDetail.Checked Then
            If Not String.IsNullOrEmpty(Txt_EmpUserName.Text) Then

                con.Open()
                Try
                    ' Check if the staff ID exists in the database
                    Dim query As String = "SELECT * FROM EmployeeData WHERE UserName = @UserName"
                    Using cmd As New SqlCommand(query, con)
                        cmd.Parameters.AddWithValue("@UserName", Txt_EmpUserName.Text)
                        ' Execute the query and check if any rows were returned
                        Dim reader As SqlDataReader = cmd.ExecuteReader()
                        If reader.HasRows Then
                            reader.Read()
                            ' Populate the fields with retrieved data
                            Txt_EmpName.Text = reader(1) ' Name
                            Txt_EmpGender.Text = reader(2) ' Gender
                            Txt_EmpAge.Text = reader(3) ' Age
                            Txt_EmpEmail.Text = reader(4) ' Email
                            Txt_EmpMobile.Text = reader(5) ' Mobile no
                            Txt_EmpQualification.Text = reader(6) ' Qualification
                            Txt_EmpAddarNo.Text = reader(7) ' addhar No
                            Txt_EmpAddress.Text = reader(8) ' Address
                        End If
                    End Using
                Catch ex As Exception
                    ' Handle the exception (e.g., display an error message)
                    MessageBox.Show("An error occurred: " & ex.Message)
                Finally
                    con.Close()
                End Try
            End If
        End If
    End Sub

    Private Sub Radio_Register_CheckedChanged(sender As Object, e As EventArgs) Handles Radio_Register.CheckedChanged
        Txt_EmpAddress.Enabled = True
        Txt_EmpAge.Enabled = True
        Txt_EmpEmail.Enabled = True
        Txt_EmpMobile.Enabled = True
        Txt_EmpName.Enabled = True
        Txt_EmpGender.Enabled = True
        Txt_EmpQualification.Enabled = True
        ClearAllEmployeeBusControls()
    End Sub

    Private Sub Guna2GradientPanel3_Paint(sender As Object, e As PaintEventArgs) Handles Guna2GradientPanel3.Paint

    End Sub

    Private Sub Txt_EmpMobile_TextChanged(sender As Object, e As EventArgs) Handles Txt_EmpMobile.TextChanged
        checkNum(Txt_EmpMobile)
    End Sub

    Private Sub Txt_EmpMobile_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Txt_EmpMobile.KeyPress
        If Not Char.IsControl(e.KeyChar) AndAlso Not Char.IsDigit(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

    Private Sub Txt_EmpAddarNo_TextChanged(sender As Object, e As EventArgs) Handles Txt_EmpAddarNo.TextChanged
        checkAddrarNum(Txt_EmpMobile)
    End Sub

    Private Sub Txt_EmpAddarNo_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Txt_EmpAddarNo.KeyPress
        If Not Char.IsControl(e.KeyChar) AndAlso Not Char.IsDigit(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub
End Class