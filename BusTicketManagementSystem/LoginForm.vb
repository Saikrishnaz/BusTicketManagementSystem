Imports System.Data.SqlClient

Public Class LoginForm
    Dim connectionString As String = ReturnConnectionString()
    Dim con As New SqlConnection(connectionString)
    Private Sub Guna2GradientButton2_Click(sender As Object, e As EventArgs) Handles Guna2GradientButton2.Click
        Guna2TabControl1.SelectedTab = TabPage1
    End Sub

    Private Sub Guna2GradientButton7_Click(sender As Object, e As EventArgs) Handles Guna2GradientButton7.Click
        Guna2TabControl1.SelectedTab = TabPage2
    End Sub

    Private Sub LoginForm_Load(sender As Object, e As EventArgs) Handles Me.Load
        Guna2TabControl1.TabMenuVisible = False
    End Sub

    Private Sub Guna2GradientButton8_Click(sender As Object, e As EventArgs) Handles Guna2GradientButton8.Click
        If con.State = ConnectionState.Open Then
            con.Close()
        End If

        If Txt_EmpUserName.Text = "" And Txt_EmpPassword.Text = "" Then
            MsgBox("Please enter your UserName And Password to login.")
            Exit Sub
        End If

        If CHECKeMAIL(con, Txt_EmpUserName.Text, "EmployeeData", "UserName") = False Then
            MsgBox("UserName Does not Exist. Please check your Mail.")
            Exit Sub
        End If

        Dim enteredPassword As String = Txt_EmpUserName.Text.Trim()

        If Not String.IsNullOrEmpty(Txt_EmpUserName.Text) Then
            If Not String.IsNullOrEmpty(Txt_EmpUserName.Text) Then
                Dim storedPassword As String = ""
                ' Assuming con is a valid SqlConnection object
                Try
                    con.Open()
                    ' Check if the staff ID exists in the database
                    Dim query As String = "SELECT UserName FROM EmployeeData WHERE UserName = @UserName AND Password = @Password"
                    Using cmd As New SqlCommand(query, con)
                        cmd.Parameters.AddWithValue("@UserName", Txt_EmpUserName.Text)
                        cmd.Parameters.AddWithValue("@Password", Txt_EmpPassword.Text)
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
                    HomePage.Show()
                    HomePage.Label3.Text = storedPassword
                Else
                    MsgBox("You've Entered Wrong Credential....")
                End If
            End If
        End If
        Txt_EmpUserName.Clear()
        Txt_EmpPassword.Clear()
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked = True Then
            Txt_EmpPassword.PasswordChar = ControlChars.NullChar ' Display actual characters
        Else
            Txt_EmpPassword.PasswordChar = "*" ' Display asterisks
        End If
    End Sub

    Private Sub Guna2GradientButton1_Click(sender As Object, e As EventArgs) Handles Guna2GradientButton1.Click
        If Radio_UserName.Checked Then
            If con.State = ConnectionState.Open Then
                con.Close()
            End If

            If CHECKeMAIL(con, Txt_EmpUserName.Text, "EmployeeData", "UserName") = True Then
                MsgBox("Employee with the same UserName ID already exists.")
                Exit Sub
            End If

            Try
                con.Open()
                Dim query As String = "UPDATE EmployeeData SET UserName = @UserName " &
                            "WHERE Password = @Password AND AddharNo = @AddharNo"
                Using cmd As New SqlCommand(query, con)
                    ' Set parameter values...'
                    cmd.Parameters.AddWithValue("@AddharNo", If(Not String.IsNullOrEmpty(Txt_AddharNoUsrname.Text), Txt_AddharNoUsrname.Text, DBNull.Value))
                    cmd.Parameters.AddWithValue("@UserName", If(Not String.IsNullOrEmpty(Txt_UsrnameUsername.Text), Txt_UsrnameUsername.Text, DBNull.Value))
                    cmd.Parameters.AddWithValue("@Password", If(Not String.IsNullOrEmpty(Txt_PasswordUsrname.Text), Txt_PasswordUsrname.Text, DBNull.Value))

                    ' Execute the query...'
                    cmd.ExecuteNonQuery()
                    MsgBox("UserName has been successfully changed.")

                    Txt_AddharNoUsrname.Clear()
                    Txt_UsrnameUsername.Clear()
                    Txt_PasswordUsrname.Clear()
                End Using
            Catch ex As Exception
                ' Display specific error message...
                MsgBox("Error: " & ex.Message)
            Finally
                If con.State = ConnectionState.Open Then
                    con.Close()
                End If
            End Try
        ElseIf Radio_Password.Checked Then
            If Not (Txt_Pass1Password.Text = Txt_Pass2Password.Text) Then
                MsgBox("Both password fields do not match.")
                Exit Sub
            End If

            If con.State = ConnectionState.Open Then
                con.Close()
            End If

            Try
                con.Open()
                ' Update query to modify the existing employee data
                Dim query As String = "UPDATE EmployeeData SET Password = @Password " &
                            "WHERE UserName = @UserName AND AddharNo = @AddharNo" ' Assuming EmployeeID is the unique identifier

                Using cmd As New SqlCommand(query, con)
                    cmd.Parameters.AddWithValue("@AddharNo", If(Not String.IsNullOrEmpty(Txt_AddharNoPassword.Text), Txt_AddharNoPassword.Text, DBNull.Value))
                    cmd.Parameters.AddWithValue("@UserName", If(Not String.IsNullOrEmpty(Txt_UsrnamePassword.Text), Txt_UsrnamePassword.Text, DBNull.Value))
                    cmd.Parameters.AddWithValue("@Password", If(Not String.IsNullOrEmpty(Txt_Pass1Password.Text), Txt_Pass1Password.Text, DBNull.Value))

                    ' Execute the update query
                    cmd.ExecuteNonQuery()
                    MsgBox("Password has been updated successfully.")
                End Using
                Txt_AddharNoPassword.Clear()
                Txt_UsrnamePassword.Clear()
                Txt_Pass1Password.Clear()
                Txt_Pass2Password.Clear()
            Catch ex As Exception
                ' Display specific error message...
                MsgBox("Error: " & ex.Message)
            Finally
                If con.State = ConnectionState.Open Then
                    con.Close()
                End If
            End Try
        Else
            MsgBox("Please select the Action Type: Register or Update")
        End If
    End Sub

    Private Sub Radio_Register_CheckedChanged(sender As Object, e As EventArgs) Handles Radio_UserName.CheckedChanged
        Txt_AddharNoUsrname.ReadOnly = False
        Txt_UsrnameUsername.ReadOnly = False
        Txt_PasswordUsrname.ReadOnly = False

        Txt_AddharNoPassword.ReadOnly = True
        Txt_UsrnamePassword.ReadOnly = True
        Txt_Pass1Password.ReadOnly = True
        Txt_Pass2Password.ReadOnly = True

        Txt_AddharNoPassword.Clear()
        Txt_UsrnamePassword.Clear()
        Txt_Pass1Password.Clear()
        Txt_Pass2Password.Clear()
    End Sub

    Private Sub Radio_UpdateDetail_CheckedChanged(sender As Object, e As EventArgs) Handles Radio_Password.CheckedChanged
        Txt_AddharNoUsrname.ReadOnly = True
        Txt_UsrnameUsername.ReadOnly = True
        Txt_PasswordUsrname.ReadOnly = True

        Txt_AddharNoPassword.ReadOnly = False
        Txt_UsrnamePassword.ReadOnly = False
        Txt_Pass1Password.ReadOnly = False
        Txt_Pass2Password.ReadOnly = False

        Txt_AddharNoUsrname.Clear()
        Txt_UsrnameUsername.Clear()
        Txt_PasswordUsrname.Clear()
    End Sub

    Private Sub Guna2GradientPanel3_Paint(sender As Object, e As PaintEventArgs) Handles Guna2GradientPanel3.Paint

    End Sub
End Class