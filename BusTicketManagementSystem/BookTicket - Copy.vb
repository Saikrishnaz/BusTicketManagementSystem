Imports System.Data.SqlClient
Imports System.Drawing.Printing
Public Class BookTicket
    Dim connectionString As String = "Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\pranj\Desktop\BusTicketManagementSystem\BusTicketManagementSystem\BusManagementDatabase.mdf;Integrated Security=True "
    Dim con As New SqlConnection(connectionString)
    Private Function IsAllPassengerFieldsFilled() As Boolean
        ' Check if all required fields are filled
        Return Not (
            String.IsNullOrWhiteSpace(Txt_ArrivalTime.Text) OrElse
            String.IsNullOrWhiteSpace(Txt_Departure.Text) OrElse
            String.IsNullOrWhiteSpace(Txt_Destination.Text) OrElse
            String.IsNullOrWhiteSpace(Txt_FairPrice.Text) OrElse
            String.IsNullOrWhiteSpace(Txt_PsgnMobileNo.Text) OrElse
            String.IsNullOrWhiteSpace(Txt_PsgnName.Text) OrElse
            String.IsNullOrWhiteSpace(Txt_SourceLocation.Text) OrElse
            String.IsNullOrWhiteSpace(Txt_TickenNo.Text) OrElse
            Txt_JourneyName.SelectedIndex = -1 OrElse
            Txt_PsgnGender.SelectedIndex = -1)
        ' Optionally, you can remove additional conditions (CheckBoxInternetBanking, CheckBoxMobilrBanking, CheckBoxChequebook, CheckBoxemailAlerts, CheckBoxestatement)
    End Function
    Private Sub ClearAllPassengerBusControls()
        ' Clear all input controls
        Dim controlsToClear() As Control = {Txt_ArrivalTime, Txt_Departure, Txt_Destination, Txt_FairPrice, Txt_PsgnMobileNo, Txt_PsgnName, Txt_SourceLocation, Txt_TickenNo}
        For Each control As Control In controlsToClear
            control.Text = String.Empty
        Next
        Txt_JourneyName.SelectedIndex = -1
        Txt_PsgnGender.SelectedIndex = -1
        DateTimePicker1.Value = Date.Today
    End Sub

    Private Sub Guna2GradientButton3_Click(sender As Object, e As EventArgs) Handles Guna2GradientButton3.Click
        If Not IsAllPassengerFieldsFilled() Then
            MsgBox("Please fill in all the Passenger details.")
            Exit Sub
        End If
        If con.State = ConnectionState.Open Then
            con.Close()
        End If
        ' Define the array of TextBoxes
        ' Get the formatted date value from DateTimePicker1
        Dim formattedDate As String = DateTimePicker1.Value.Date.ToString("MM-dd-yyyy")

        If CHECKeMAIL(con, Txt_PsgnName.Text, "BookedTicketData", "PsgnName") = True Then
            ' Check if the formatted date already exists for the passenger
            If CHECKeMAIL(con, formattedDate, "BookedTicketData", "JourneyDate") = False Then
                ' The passenger and date are unique; proceed with booking
            Else
                MsgBox("Same Passenger Cannot Book Two Tickets of Same Date...")
                Exit Sub
            End If
        End If


        Try
            con.Open()
            Dim query As String = "INSERT INTO BookedTicketData (PsgnName, PsgnGender, PsgnMobileNo,TicketNumber,JourneyDate,JourneyName,SourceLocation,Depature,Destination,ArrivalTime,TicketFairPrice) " &
                        "VALUES (@PsgnName, @PsgnGender, @PsgnMobileNo,@TicketNumber,@JourneyDate,@JourneyName,@SourceLocation,@Depature,@Destination,@ArrivalTime,@TicketFairPrice)"
            Using cmd As New SqlCommand(query, con)
                ' Set parameter values...'
                cmd.Parameters.AddWithValue("@PsgnName", If(Not String.IsNullOrEmpty(Txt_PsgnName.Text), Txt_PsgnName.Text, DBNull.Value))
                cmd.Parameters.AddWithValue("@PsgnGender", If(Txt_PsgnGender.SelectedIndex <> -1, Txt_PsgnGender.SelectedItem.ToString(), DBNull.Value))
                cmd.Parameters.AddWithValue("@PsgnMobileNo", If(Not String.IsNullOrEmpty(Txt_PsgnMobileNo.Text), Txt_PsgnMobileNo.Text, DBNull.Value))
                cmd.Parameters.AddWithValue("@TicketNumber", If(Not String.IsNullOrEmpty(Txt_TickenNo.Text), Txt_TickenNo.Text, DBNull.Value))
                cmd.Parameters.AddWithValue("@JourneyDate", DateTimePicker1.Value.Date)
                cmd.Parameters.AddWithValue("@JourneyName", If(Txt_JourneyName.SelectedIndex <> -1, Txt_JourneyName.SelectedItem.ToString(), DBNull.Value))
                cmd.Parameters.AddWithValue("@SourceLocation", If(Not String.IsNullOrEmpty(Txt_SourceLocation.Text), Txt_SourceLocation.Text, DBNull.Value))
                cmd.Parameters.AddWithValue("@Depature", If(Not String.IsNullOrEmpty(Txt_Departure.Text), Txt_Departure.Text, DBNull.Value))
                cmd.Parameters.AddWithValue("@Destination", If(Not String.IsNullOrEmpty(Txt_Destination.Text), Txt_Destination.Text, DBNull.Value))
                cmd.Parameters.AddWithValue("@ArrivalTime", If(Not String.IsNullOrEmpty(Txt_ArrivalTime.Text), Txt_ArrivalTime.Text, DBNull.Value))
                cmd.Parameters.AddWithValue("@TicketFairPrice", If(Not String.IsNullOrEmpty(Txt_FairPrice.Text), Txt_FairPrice.Text, DBNull.Value))

                ' Execute the query...'
                cmd.ExecuteNonQuery()
                MsgBox("New Pessenger Ticket has been scusses fully Booked .")
            End Using
            con.Close()
            PPD.Document = PD
            PPD.ShowDialog()
        Catch ex As Exception
            ' Display specific error message...
            MsgBox("Error: " & ex.Message)
        End Try
        If con.State = ConnectionState.Open Then
            con.Close()

        End If
        ' Finally, repopulate the DataGridView and generate a unique invoice number
        Populatedvg(con, "BookedTicketData", DataGridView1)
        FillcomboBox(con, Txt_JourneyName, "JourneyData", "JourneyName")
        AutoCompleteSearchBoxForTextBoxesTypeString(con, Txt_PsgnName, "Name", "PassengerData", 0)
        ClearAllPassengerBusControls()
        Txt_TickenNo.Text = GenerateUniqueSTicketNumber(con)
    End Sub

    Private Sub PassengerDetails_Load(sender As Object, e As EventArgs) Handles Me.Load
        Populatedvg(con, "BookedTicketData", DataGridView1)
        DateTimePicker1.MinDate = Date.Today
        FillcomboBox(con, Txt_JourneyName, "JourneyData", "JourneyName")
        AutoCompleteSearchBoxForTextBoxesTypeString(con, Txt_PsgnName, "Name", "PassengerData", 0)
        Txt_TickenNo.Text = GenerateUniqueSTicketNumber(con)
    End Sub

    Private Sub Guna2GradientButton2_Click(sender As Object, e As EventArgs) Handles Guna2GradientButton2.Click
        ClearAllPassengerBusControls()
        DataGridView1.ForeColor = Color.Black
        AutoCompleteSearchBoxForTextBoxesTypeString(con, Txt_PsgnName, "Name", "PassengerData", 0)
        Txt_TickenNo.Text = GenerateUniqueSTicketNumber(con)
    End Sub

    Private Sub Txt_JourneyName_SelectionChangeCommitted(sender As Object, e As EventArgs) Handles Txt_JourneyName.SelectionChangeCommitted
        con.Open()
        Dim Query As String = "SELECT * FROM JourneyData WHERE JourneyName = @JourneyName"
        Dim cmd As New SqlCommand(Query, con)
        cmd.Parameters.AddWithValue("@JourneyName", Txt_JourneyName.SelectedValue.ToString())

        Dim dt As New DataTable
        Dim reader As SqlDataReader = cmd.ExecuteReader()
        If reader.Read() Then
            Txt_SourceLocation.Text = reader("SourceLocation").ToString()
            Txt_Departure.Text = reader("Departure").ToString()
            Txt_Destination.Text = reader("Destination").ToString()
            Txt_ArrivalTime.Text = reader("ArrivalTime").ToString()
            Txt_FairPrice.Text = reader("JourneyPrice").ToString()
        End If
        con.Close()

    End Sub

    Private Sub Txt_PsgnName_TextChanged(sender As Object, e As EventArgs) Handles Txt_PsgnName.TextChanged
        If Not String.IsNullOrEmpty(Txt_PsgnName.Text) Then
            con.Open()
            Try
                ' Check if the staff ID exists in the database
                Dim query As String = "SELECT * FROM PassengerData WHERE Name = @Name"
                Using cmd As New SqlCommand(query, con)
                    cmd.Parameters.AddWithValue("@Name", Txt_PsgnName.Text)

                    ' Execute the query and check if any rows were returned
                    Dim reader As SqlDataReader = cmd.ExecuteReader()
                    If reader.HasRows Then
                        reader.Read()
                        ' Populate the fields with retrieved data
                        Txt_PsgnGender.Text = If(Not String.IsNullOrEmpty(reader(2)), reader(2).ToString(), DBNull.Value) ' Gender
                        Txt_PsgnMobileNo.Text = If(Not String.IsNullOrEmpty(reader(5)), reader(5).ToString(), DBNull.Value) ' MobilrNo
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


    ' lets declare some global variable for print purpose
    Dim WithEvents PD As New PrintDocument
    Dim PPD As New PrintPreviewDialog
    Dim LongPaper As Integer

    Private Sub PD_BeginPrint(sender As Object, e As Printing.PrintEventArgs) Handles PD.BeginPrint
        Dim PageSetUp As New PageSettings
        Dim CustomPaperSize As New PaperSize("Custom", 150, 90)
        PageSetUp.PaperSize = CustomPaperSize ' Assign the custom PaperSize object to PageSetUp.PaperSize
        PD.DefaultPageSettings = PageSetUp ' Assuming PD is your PrintDocument object
    End Sub

    Private Sub PD_PrintPage(sender As Object, e As Printing.PrintPageEventArgs) Handles PD.PrintPage
        Dim f6 As New Font("Microsoft Himalaya", 6, FontStyle.Regular)
        Dim f7 As New Font("Microsoft Himalaya", 7, FontStyle.Regular)
        Dim f8 As New Font("Microsoft Himalaya", 8, FontStyle.Regular)
        Dim f8b As New Font("Microsoft Himalaya", 8, FontStyle.Bold)
        Dim f10 As New Font("Microsoft Himalaya", 10, FontStyle.Regular)
        Dim f10b As New Font("Microsoft Himalaya", 10, FontStyle.Bold)
        Dim f14 As New Font("Microsoft Himalaya", 14, FontStyle.Regular)
        Dim f16 As New Font("Microsoft Himalaya", 16, FontStyle.Bold)

        Dim leftMargin As Integer = e.MarginBounds.Left
        Dim CenterMargin As Integer = 75
        Dim RightMargin As Integer = e.MarginBounds.Right

        ' Font alignment
        Dim right As New StringFormat
        Dim Center As New StringFormat
        Dim left As New StringFormat
        right.Alignment = StringAlignment.Far
        Center.Alignment = StringAlignment.Center
        left.Alignment = StringAlignment.Near

        Dim line As String = "----------------------------------------------------------------------------------------------------"
        Dim starline As String = "************************************************************************************************"

        Dim content As String = " "

        'e.Graphics.DrawString("Right-aligned text", Font, Brush, x, y, right)
        ' e.Graphics.DrawString("Centered text", Font, Brush, x, y, Center)
        ' e.Graphics.DrawString("Left-aligned text", Font, Brush, x, y, left)

        Dim pageBackgroundColor As Brush = Brushes.LightGray ' Change this color as needed
        e.Graphics.FillRectangle(pageBackgroundColor, e.MarginBounds)

        ' Example content to print

        ' Drawing text on the print document
        e.Graphics.DrawString(line, f10, Brushes.Black, 75, 1, Center)
        e.Graphics.DrawString("Booked By Bus Ticket Master", f8b, Brushes.Black, 75, 9, Center)
        e.Graphics.DrawString(Txt_JourneyName.Text, f8, Brushes.Black, 75, 15, Center)
        e.Graphics.DrawString(line, f14, Brushes.Black, 75, 20, Center)
        e.Graphics.DrawString("JourneyDate : " & DateTimePicker1.Value.Date.ToString("MM-dd-yyyy"), f6, Brushes.Black, leftMargin, 30, Center)
        e.Graphics.DrawString("Ticket Number :", f6, Brushes.Black, 65, 37, right)
        e.Graphics.DrawString(Txt_TickenNo.Text, f6, Brushes.Black, leftMargin, 37, left)
        e.Graphics.DrawString("Passenger Name :", f6, Brushes.Black, 65, 42, right)
        e.Graphics.DrawString(Txt_PsgnName.Text, f6, Brushes.Black, leftMargin, 42, left)
        e.Graphics.DrawString("Passenger Noible No :", f6, Brushes.Black, 65, 47, right)
        e.Graphics.DrawString(Txt_PsgnMobileNo.Text, f6, Brushes.Black, leftMargin, 47, left)
        e.Graphics.DrawString("Loaction", f6, Brushes.Black, RightMargin, 55, Center)
        e.Graphics.DrawString("Time", f6, Brushes.Black, leftMargin, 55, Center)
        e.Graphics.DrawString(Txt_SourceLocation.Text, f6, Brushes.Black, RightMargin, 60, Center)
        e.Graphics.DrawString(Txt_Departure.Text, f6, Brushes.Black, leftMargin, 60, Center)
        e.Graphics.DrawString(Txt_Destination.Text, f6, Brushes.Black, RightMargin, 65, Center)
        e.Graphics.DrawString(Txt_ArrivalTime.Text, f6, Brushes.Black, leftMargin, 65, Center)
        e.Graphics.DrawString("Price :", f6, Brushes.Black, RightMargin, 72, right)
        e.Graphics.DrawString(Txt_FairPrice.Text, f6, Brushes.Black, leftMargin, 72, left)

        e.Graphics.DrawString(line, f14, Brushes.Black, CenterMargin, 75, Center)
        e.HasMorePages = False
    End Sub

    Private Sub Guna2GradientPanel3_Paint(sender As Object, e As PaintEventArgs) Handles Guna2GradientPanel3.Paint

    End Sub

    Private Sub Guna2GradientPanel1_Paint(sender As Object, e As PaintEventArgs) Handles Guna2GradientPanel1.Paint

    End Sub
End Class