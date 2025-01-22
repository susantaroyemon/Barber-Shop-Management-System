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
    public partial class Category : Form
    {

        int id;

        string connectionString = "data source=SUSANTA-ROG\\SQLEXPRESS; database=BarberBD; integrated security=SSPI";//BarBer

        public Category(int i)
        {
            id = i;
            InitializeComponent();

            string query = "SELECT * FROM Category";
            FillDataGridView(query);
            //LoadDetails();
            LoadServiceIDsToComboBox();

        }
        private void LoadServiceIDsToComboBox()
        {
            string query = "SELECT ServiceID FROM Service"; // Query to select ServiceID
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    // Clear ComboBox before adding new items
                    comboBox1.Items.Clear();

                    // Loop through the reader and add each ServiceID to the ComboBox
                    while (reader.Read())
                    {
                        comboBox1.Items.Add(reader["ServiceID"].ToString());
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
            string query = "SELECT * FROM Category"; // Define the query here.

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

            string query = @"SELECT * FROM Category
                     WHERE CategoryID LIKE @searchTerm 
                        OR CategoryName LIKE @searchTerm";

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
            textBox3.Clear();
            //comboBox1.Clear();

        }

        private void button6_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string CategoryID = textBox1.Text.Trim();
            string CategoryName = textBox2.Text.Trim();
            string CategoryPrice = textBox3.Text.Trim();
            string ServiceID = comboBox1.Text;


            // Validate input fields
            if (string.IsNullOrWhiteSpace(CategoryID) ||
                string.IsNullOrWhiteSpace(CategoryName) ||
                string.IsNullOrWhiteSpace(CategoryPrice) ||
                string.IsNullOrWhiteSpace(ServiceID))
            {
                MessageBox.Show("All fields must be filled out.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // SQL Insert Query
            string query = @"INSERT INTO Category (CategoryID, CategoryName, CategoryPrice, ServiceID) 
                     VALUES (@CategoryID, @CategoryName, @CategoryPrice, @ServiceID)";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Add parameters to avoid SQL injection
                        command.Parameters.AddWithValue("@CategoryID", CategoryID);
                        command.Parameters.AddWithValue("@CategoryName", CategoryName);
                        command.Parameters.AddWithValue("@CategoryPrice", CategoryPrice);
                        command.Parameters.AddWithValue("@ServiceID", ServiceID);

                        // Open connection and execute query
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();

                        // Check if the record was successfully added
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Record added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Refresh DataGridView
                            FillDataGridView("SELECT * FROM Category");

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
            string CategoryID = textBox1.Text.Trim();
            string CategoryName = textBox2.Text.Trim();
            string CategoryPrice = textBox3.Text.Trim();
            string ServiceID = comboBox1.Text;


            // Validate that all fields are filled
            if (string.IsNullOrWhiteSpace(CategoryID) ||
                string.IsNullOrWhiteSpace(CategoryName) ||
                string.IsNullOrWhiteSpace(CategoryPrice) ||
                string.IsNullOrWhiteSpace(ServiceID))
            {
                MessageBox.Show("All fields must be filled out.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // SQL Update Query
            string query = @"UPDATE Category
                     SET CategoryName = @CategoryName, CategoryPrice = @CategoryPrice, ServiceID=@ServiceID WHERE CategoryID = @CategoryID";

            // Update record in the database
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Add parameters to avoid SQL injection
                    command.Parameters.AddWithValue("@CategoryID", CategoryID);
                    command.Parameters.AddWithValue("@CategoryName", CategoryName);
                    command.Parameters.AddWithValue("@CategoryPrice", CategoryPrice);
                    command.Parameters.AddWithValue("@ServiceID", ServiceID);


                    // Open connection and execute query
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    // Check if the record was updated successfully
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Record updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        FillDataGridView("SELECT * FROM Category"); // Refresh the DataGridView
                    }
                    else
                    {
                        MessageBox.Show("No record was updated. Please check the CategoryID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            string CategoryID = textBox1.Text.Trim();

            // Validate that the Category field is not empty
            if (string.IsNullOrWhiteSpace(CategoryID))
            {
                MessageBox.Show("Please enter a CategoryID to delete.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                string query = "DELETE FROM Category WHERE CategoryID = @CategoryID";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Add parameter to avoid SQL injection
                        command.Parameters.AddWithValue("@CategoryID", CategoryID);

                        // Open connection and execute query
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();

                        // Check if the record was deleted successfully
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Record deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            FillDataGridView("SELECT * FROM Category"); // Refresh the DataGridView
                        }
                        else
                        {
                            MessageBox.Show("No record found with the provided CategoryID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            Form2 f2 = new Form2(id);
            f2.Show();
        }

        private void button5_Click_1(object sender, EventArgs e)
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
    }
}
