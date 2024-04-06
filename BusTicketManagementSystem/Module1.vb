Imports System.Data.SqlClient

Module Module1
    Function ReturnConnectionString() As String
        Dim connectionString As String = "Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\user\OneDrive\Desktop\BusTicketManagementSystem\BusTicketManagementSystem\BusTicketManagementSystem\BusManagementDatabase.mdf;Integrated Security=True"
        Return connectionString
    End Function
    Public Function GenerateUniqueSTicketNumber(ByVal con As SqlConnection) As String
        con.Open()

        Dim StudentCode As String = ""
        Dim isUnique As Boolean = False

        ' Loop until a unique StudentCode is generated
        While Not isUnique
            ' Generate a random 10-digit account number
            Dim rnd As New Random()
            Dim tempStudentcode As String = GenerateRandomTicketNumber()

            ' Check if the generated StudentCode already exists in the table
            Dim query As String = "SELECT COUNT(*) FROM BookedTicketData WHERE TicketNumber = @TicketNumber"
            Dim cmd As New SqlCommand(query, con)
            cmd.Parameters.AddWithValue("@TicketNumber", StudentCode)
            Dim count As Integer = Convert.ToInt32(cmd.ExecuteScalar())

            If count = 0 Then
                ' If the StudentCode is unique, assign it and break the loop
                StudentCode = tempStudentcode
                isUnique = True
            End If
        End While

        con.Close()
        Return StudentCode
    End Function

    Public Function GenerateRandomTicketNumber() As String
        ' Generate a random 10-digit account number
        Dim rnd As New Random()
        Dim StudentCode As String = rnd.Next(10000, 99999).ToString()
        Return StudentCode
    End Function
    Public Sub Populatedvg(con As SqlConnection, tablename As String, datagrid As DataGridView)
        ' Function to generate a Temporary Table from the Database into a DataGridView
        datagrid.Columns.Clear()
        con.Open()
        Dim sql = "SELECT * FROM " & tablename ' Remove the single quotes
        Dim adapter As SqlDataAdapter
        adapter = New SqlDataAdapter(sql, con)
        Dim builder As SqlCommandBuilder
        builder = New SqlCommandBuilder(adapter) ' This line should work correctly
        Dim ds As DataSet
        ds = New DataSet
        adapter.Fill(ds)
        datagrid.DataSource = ds.Tables(0)
        con.Close()
    End Sub

    Sub FillcomboBox(con As SqlConnection, cmbx As ComboBox, tblName As String, ColumnName As String)
        ' Open the connection
        con.Open()

        ' Create a SQL command to select all data from the specified table
        Dim cmd As New SqlCommand("SELECT * FROM " & tblName, con)

        ' Create a data adapter and a DataTable
        Dim adapter As New SqlDataAdapter(cmd)
        Dim Tbl As New DataTable

        ' Fill the DataTable with data from the database
        adapter.Fill(Tbl)

        ' Set the ComboBox's data source and member bindings
        cmbx.DataSource = Tbl
        cmbx.DisplayMember = ColumnName
        cmbx.ValueMember = ColumnName

        ' Close the connection
        con.Close()
    End Sub
    Public Function CHECKeMAIL(ByVal con As SqlConnection, email As String, table As String, Clm As String) As Boolean
        con.Open()

        ' Check if the generated StaffCode already exists in the table
        Dim query As String = "SELECT COUNT(*) FROM " & table & " WHERE " & Clm & " = @Email"
        Dim cmd As New SqlCommand(query, con)
        cmd.Parameters.AddWithValue("@Email", email)
        Dim count As Integer = Convert.ToInt32(cmd.ExecuteScalar())

        con.Close()

        If count = 0 Then
            Return False ' 0 or False for email not found
        Else
            Return True ' 1 or True for email found
        End If
    End Function

    Sub AutoCompleteSearchBoxForTextBoxesTypeString(con As SqlConnection, textbox1 As TextBox, ColumnName As String, TableName As String, columnNameIndex As Integer)
        con.Open()
        Dim Query As String = "SELECT " & ColumnName & " FROM " & TableName
        Dim Cmd As New SqlCommand(Query, con)
        Dim reader As SqlDataReader
        reader = Cmd.ExecuteReader
        Dim ElementsToSuggest As AutoCompleteStringCollection = New AutoCompleteStringCollection()

        While reader.Read
            ElementsToSuggest.Add(reader.GetString(columnNameIndex)) ' Use GetString to retrieve the column's value as a string

        End While
        textbox1.AutoCompleteCustomSource = ElementsToSuggest
        con.Close()
    End Sub
    Sub checkNum(textt1 As TextBox)
        Dim text As String = textt1.Text
        If text.Length > 10 Then
            textt1.Text = text.Substring(0, 10)
            MsgBox("Mobile number cant exceed 10 number")
        End If

    End Sub
    Sub checkAddrarNum(textt1 As TextBox)
        Dim text As String = textt1.Text
        If text.Length > 12 Then
            textt1.Text = text.Substring(0, 12)
            MsgBox("Addhar number cant exceed 12 number")
        End If

    End Sub
End Module
