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
    public partial class Product : Form
    {
        int id;

        string connectionString = "data source=SUSANTA-ROG\\SQLEXPRESS; database=BarberBD; integrated security=SSPI";//BarBer

        public Product(int i)
        {
            id = i;
            InitializeComponent();
            string query = "SELECT * FROM Product";
            FillDataGridView(query);
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
        private void ClearFields()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            //comboBox1.Clear();

        }
        private void button1_Click(object sender, EventArgs e)
        {
            string ProductID = textBox1.Text.Trim();
            string ProductName = textBox2.Text.Trim();
            string ProductPrice = textBox3.Text.Trim();
            string ProductExp_Date = dateTimePicker1.Value.Date.ToString("dd/MM/yyyy");


            // Validate input fields
            if (string.IsNullOrWhiteSpace(ProductID) || string.IsNullOrWhiteSpace(ProductName) || string.IsNullOrWhiteSpace(ProductPrice) || string.IsNullOrWhiteSpace(ProductExp_Date)) { 
                MessageBox.Show("All fields must be filled out.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // SQL Insert Query
            string query = @"INSERT INTO Product (ProductID, ProductName, ProductPrice, ProductExp_Date) VALUES (@ProductID, @ProductName, @ProductPrice, @ProductExp_Date)";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Add parameters to avoid SQL injection
                        command.Parameters.AddWithValue("@ProductID", ProductID);
                        command.Parameters.AddWithValue("@ProductName", ProductName);

                        command.Parameters.AddWithValue("@ProductPrice", ProductPrice);

                        command.Parameters.AddWithValue("@ProductExp_Date", ProductExp_Date);

                        // Open connection and execute query
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();

                        // Check if the record was successfully added
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Record added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Refresh DataGridView
                            FillDataGridView("SELECT * FROM Product");

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

        private void button2_Click(object sender, EventArgs e)
        {
            string ProductID = textBox1.Text.Trim();
            string ProductName = textBox2.Text.Trim();
            string ProductPrice = textBox3.Text.Trim();
            string ProductExp_Date = dateTimePicker1.Value.Date.ToString("dd/MM/yyyy");


            // Validate that all fields are filled
            if (string.IsNullOrWhiteSpace(ProductID) || string.IsNullOrWhiteSpace(ProductName) || string.IsNullOrWhiteSpace(ProductPrice) || string.IsNullOrWhiteSpace(ProductExp_Date))
            {
                MessageBox.Show("All fields must be filled out.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // SQL Update Query
            string query = @"UPDATE Product
                     SET  ProductName=@ProductName, ProductPrice = @ProductPrice, ProductExp_Date = @ProductExp_Date WHERE ProductID = @ProductID";

            // Update record in the database
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Add parameters to avoid SQL injection
                    command.Parameters.AddWithValue("@ProductID", ProductID);
                    command.Parameters.AddWithValue("@ProductName", ProductName);

                    command.Parameters.AddWithValue("@ProductPrice", ProductPrice);

                    command.Parameters.AddWithValue("@ProductExp_Date", ProductExp_Date);


                    // Open connection and execute query
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    // Check if the record was updated successfully
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Record updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        FillDataGridView("SELECT * FROM Product"); // Refresh the DataGridView
                    }
                    else
                    {
                        MessageBox.Show("No record was updated. Please check the Product ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string ProductID = textBox1.Text.Trim();
            // Validate that the ServiceID field is not empty
            if (string.IsNullOrWhiteSpace(ProductID))
            {
                MessageBox.Show("Please enter a ProductID to delete.", "ValidationError", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // Confirm deletion
            DialogResult result = MessageBox.Show("Are you sure you want to delete this record ? ", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                // SQL Delete Query
                string query = "DELETE FROM Product WHERE ProductID = @ProductID";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Add parameter to avoid SQL injection
                        command.Parameters.AddWithValue("@ProductID", ProductID);
                        // Open connection and execute query
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        // Check if the record was deleted successfully
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Record deleted successfully!",
                           "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            FillDataGridView("SELECT * FROM Product"); // Refresh the DataGridView
                        }
                        else
                        {
                            MessageBox.Show("No record found with the provided ProductID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form2 f2 = new Form2(id);
            f2.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string searchValue = textBox5.Text.Trim();

            if (string.IsNullOrWhiteSpace(searchValue))
            {
                MessageBox.Show("Please enter a search term.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string query = @"SELECT * FROM Product
                     WHERE ProductID LIKE @searchTerm
                     OR ProductName LIKE @searchTerm
                     OR ProductPrice LIKE @searchTerm"; 

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
