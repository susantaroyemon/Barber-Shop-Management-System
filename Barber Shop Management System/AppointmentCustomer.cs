using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Barber_Shop_Management_System
{
    public partial class AppointmentCustomer : Form
    {
       
        int idf3;
        string connectionString = "data source=SUSANTA-ROG\\SQLEXPRESS; database=BarberBD; integrated security=SSPI";

        public AppointmentCustomer(int i3)
        {
            idf3 = i3;
            InitializeComponent();
            comboBox3.Items.Clear();  // Clear existing times

            // Load services and categories into comboBox1 and comboBox2
            LoadServiceNamesToComboBox2();
            LoadServiceNamesToComboBox3();

            // Define available time slots
            string[] time = new string[15];
            time[0] = "8:00 - 9:00 AM";
            time[1] = "9:00 - 10:00 AM";
            time[2] = "10:00 - 11:00 AM";
            time[3] = "11:00 - 12:00 PM";
            time[4] = "12:00 - 1:00 PM";
            time[5] = "1:00 - 2:00 PM";
            time[6] = "2:00 - 3:00 PM";
            time[7] = "3:00 - 4:00 PM";
            time[8] = "4:00 - 5:00 PM";
            time[9] = "5:00 - 6:00 PM";
            time[10] = "6:00 - 7:00 PM";
            time[11] = "7:00 - 8:00 PM";
            time[12] = "8:00 - 9:00 PM";
            time[13] = "9:00 - 10:00 PM";

            comboBox3.DataSource = time;

            // Update available times based on the selected appointment date
            dateTimePicker1.ValueChanged += DateTimePicker1_ValueChanged;
        }

        private void DateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            // Each time the user selects a new date, refresh available times
            RemoveUnavailableTimes();
        }

        // Load ServiceNames into comboBox1 (Services)
        private void LoadServiceNamesToComboBox2()
        {
            string query = "SELECT ServiceName FROM Service"; // Query to select ServiceName
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    // Clear ComboBox before adding new items
                    comboBox1.Items.Clear();

                    // Add each ServiceName to the ComboBox
                    while (reader.Read())
                    {
                        comboBox1.Items.Add(reader["ServiceName"].ToString());
                    }
                }
            }
        }

        // Load CategoryNames into comboBox2 (Categories)
        private void LoadServiceNamesToComboBox3()
        {
            string query = "SELECT CategoryName FROM Category"; // Query to select CategoryName
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    // Clear ComboBox before adding new items
                    comboBox2.Items.Clear();

                    // Add each CategoryName to the ComboBox
                    while (reader.Read())
                    {
                        comboBox2.Items.Add(reader["CategoryName"].ToString());
                    }
                }
            }
        }

        // Remove unavailable times from comboBox3 based on the selected date
        private void RemoveUnavailableTimes()
        {
            // Define all possible time slots
            var allTimes = new List<string>
    {
        "8:00 - 9:00 AM", "9:00 - 10:00 AM", "10:00 - 11:00 AM",
        "11:00 - 12:00 PM", "12:00 - 1:00 PM", "1:00 - 2:00 PM",
        "2:00 - 3:00 PM", "3:00 - 4:00 PM", "4:00 - 5:00 PM",
        "5:00 - 6:00 PM", "6:00 - 7:00 PM", "7:00 - 8:00 PM",
        "8:00 - 9:00 PM", "9:00 - 10:00 PM"
    };

            // Get the selected date
            string appointmentDate = dateTimePicker1.Value.Date.ToString("dd/MM/yyyy");

            // Query to get all booked AppointmentTimes for the selected date
            string query = "SELECT AppointmentTime FROM Appointment WHERE AppointmentDate = @AppointmentDate";

            var unavailableTimes = new List<string>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@AppointmentDate", appointmentDate);
                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        unavailableTimes.Add(reader["AppointmentTime"].ToString());
                    }
                }
            }

            // Filter available times by removing the unavailable ones
            var availableTimes = allTimes.Except(unavailableTimes).ToList();

            // Refresh comboBox3 with available times
            comboBox3.DataSource = availableTimes;
        }


        private void button2_Click_1(object sender, EventArgs e)
        {
            // Collect information from input fields
            int AppointmentID = idf3;
            string ServiceName = comboBox1.Text;
            string CategoryName = comboBox2.Text;
            string AppointmentDate = dateTimePicker1.Value.Date.ToString("dd/MM/yyyy");  // Use SQL-friendly date format
            string AppointmentTime = comboBox3.Text;

            // Validate inputs
            if ( string.IsNullOrWhiteSpace(AppointmentDate) ||
                string.IsNullOrWhiteSpace(ServiceName) || string.IsNullOrWhiteSpace(AppointmentTime) ||
                string.IsNullOrWhiteSpace(CategoryName))
            {
                MessageBox.Show("All fields must be filled out.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Insert new appointment into database
            string query = @"INSERT INTO Appointment (AppointmentID, ServiceName, CategoryName, AppointmentDate, AppointmentTime)
                             VALUES (@AppointmentID, @ServiceName, @CategoryName, @AppointmentDate, @AppointmentTime)";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Add parameters to avoid SQL injection
                        command.Parameters.AddWithValue("@AppointmentID", AppointmentID);
                        command.Parameters.AddWithValue("@ServiceName", ServiceName);
                        command.Parameters.AddWithValue("@CategoryName", CategoryName);
                        command.Parameters.AddWithValue("@AppointmentDate", AppointmentDate);
                        command.Parameters.AddWithValue("@AppointmentTime", AppointmentTime);

                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();

                        // Check if the record was successfully inserted
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Appointment created successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.Hide();
                            CustomerForm2 f1 = new CustomerForm2(idf3);
                            f1.Show();
                        }
                        else
                        {
                            MessageBox.Show("Failed to create the appointment. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

       
    }


}
