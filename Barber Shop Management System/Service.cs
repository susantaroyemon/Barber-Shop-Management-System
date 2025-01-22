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

namespace Barber_Shop_Management_System
{
    public partial class Service : Form
    {
       
        int id;

        string connectionString = "data source=SUSANTA-ROG\\SQLEXPRESS; database=BarberBD; integrated security=SSPI";//BarBer

        public Service(int i)
        {
            id = i;
            InitializeComponent();

            string query = "SELECT * FROM Service";
            FillDataGridView(query);
            //LoadDetails();
        }

        private void LoadDetails()
        {
            string query = "SELECT ServiceID, ServiceName FROM Service WHERE ServiceID = @ServiceID";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ServiceID", id);
                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        // Populate the text boxes with the retrieved data
                        textBox1.Text = reader["ServiceID"].ToString();
                        textBox2.Text = reader["ServiceName"].ToString();
                        
                    }
                    else
                    {
                        MessageBox.Show("No details found for the given ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Close(); // Close the form if no data is found
                    }
                }
            }
        }

        private void FillDataGridView(string query)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    DataTable dataTable = new DataTable();
                    dataTable.Load(reader);
                    dataGridView1.DataSource = dataTable;
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string query = "SELECT * FROM Service"; // Define the query here.

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    con.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    DataTable dataTable = new DataTable();
                    dataTable.Load(reader);
                    dataGridView1.DataSource = dataTable;
                }
            }
        }

       



        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {

            string searchValue = textBox5.Text.Trim();

            if (string.IsNullOrWhiteSpace(searchValue))
            {
                MessageBox.Show("Please enter a search term.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string query = @"SELECT * FROM Service
                     WHERE ServiceID LIKE @searchTerm 
                        OR ServiceName LIKE @searchTerm";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@searchTerm", "%" + searchValue + "%");

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    dataGridView1.DataSource = dataTable;

                    if (dataTable.Rows.Count == 0)
                    {
                        MessageBox.Show("No matching rows found.", "Search Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

            }

        }

       
        private void ClearFields()
        {
            textBox1.Clear();
            textBox2.Clear();
            
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            string ServiceID = textBox1.Text.Trim();
            string ServiceName = textBox2.Text.Trim();


            // Validate input fields
            if (string.IsNullOrWhiteSpace(ServiceID) ||
                string.IsNullOrWhiteSpace(ServiceName))
            {
                MessageBox.Show("All fields must be filled out.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // SQL Insert Query
            string query = @"INSERT INTO Service (ServiceID, ServiceName) 
                     VALUES (@ServiceID, @ServiceName)";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Add parameters to avoid SQL injection
                        command.Parameters.AddWithValue("@ServiceID", ServiceID);
                        command.Parameters.AddWithValue("@ServiceName", ServiceName);

                        // Open connection and execute query
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();

                        // Check if the record was successfully added
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Record added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Refresh DataGridView
                            FillDataGridView("SELECT * FROM Service");

                            // Clear text boxes after adding data
                            ClearFields();
                        }
                        else
                        {
                            MessageBox.Show("Failed to add the record. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            string ServiceID = textBox1.Text.Trim();
            string ServiceName = textBox2.Text.Trim();


            // Validate that all fields are filled
            if (string.IsNullOrWhiteSpace(ServiceID) || string.IsNullOrWhiteSpace(ServiceName))
            {
                MessageBox.Show("All fields must be filled out.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // SQL Update Query
            string query = @"UPDATE Service
                     SET ServiceName = @ServiceName
                         
                     WHERE ServiceID = @ServiceID";

            // Update record in the database
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Add parameters to avoid SQL injection
                    command.Parameters.AddWithValue("@ServiceID", ServiceID);
                    command.Parameters.AddWithValue("@ServiceName", ServiceName);


                    // Open connection and execute query
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    // Check if the record was updated successfully
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Record updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        FillDataGridView("SELECT * FROM Service"); // Refresh the DataGridView
                    }
                    else
                    {
                        MessageBox.Show("No record was updated. Please check the ServiceID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            string ServiceID = textBox1.Text.Trim();

            // Validate that the ServiceID field is not empty
            if (string.IsNullOrWhiteSpace(ServiceID))
            {
                MessageBox.Show("Please enter a ServiceID to delete.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Confirm deletion
            DialogResult result = MessageBox.Show("Are you sure you want to delete this record?",
                                                  "Confirm Deletion",
                                                  MessageBoxButtons.YesNo,
                                                  MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // SQL Delete Query
                string query = "DELETE FROM Service WHERE ServiceID = @ServiceID";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Add parameter to avoid SQL injection
                        command.Parameters.AddWithValue("@ServiceID", ServiceID);

                        // Open connection and execute query
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();

                        // Check if the record was deleted successfully
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Record deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            FillDataGridView("SELECT * FROM Service"); // Refresh the DataGridView
                        }
                        else
                        {
                            MessageBox.Show("No record found with the provided ServiceID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }

        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            Form2 f2 = new Form2(id);
            f2.Show();
        }
    }
}
