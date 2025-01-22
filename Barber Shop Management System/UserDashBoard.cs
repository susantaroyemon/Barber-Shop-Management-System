using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


namespace Barber_Shop_Management_System
{
    public partial class UserDashBoard : Form
    {
        string conString = "data source=SUSANTA-ROG\\SQLEXPRESS; database=BarberBD; integrated security=SSPI";
        

        public UserDashBoard()
        {
            InitializeComponent();
            string query = "SELECT * FROM Category";
            FillDataGridView(query);
            string query1 = "SELECT * FROM Billing";
            FillDataGridView1(query1);


        }

        private void FillDataGridView(string query)
        {
            using (SqlConnection connection = new SqlConnection(conString))
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
        private void FillDataGridView1(string query)
        {
            using (SqlConnection connection = new SqlConnection(conString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    DataTable dataTable = new DataTable();
                    dataTable.Load(reader);
                    dataGridView2.DataSource = dataTable;
                }
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {

            string searchValue = textBox1.Text.Trim();

            if (string.IsNullOrWhiteSpace(searchValue))
            {
                MessageBox.Show("Please enter a search term.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string query = @"SELECT * FROM Billing 
               WHERE CustomerID LIKE @searchTerm ";


            using (SqlConnection connection = new SqlConnection(conString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@searchTerm", "%" + searchValue + "%");

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    dataGridView2.DataSource = dataTable;

                    if (dataTable.Rows.Count == 0)
                    {
                        MessageBox.Show("No matching rows found.", "Search Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }

        }

        private void label3_Click(object sender, EventArgs e)
        {
            try
            {
                // SQL Query to count total staff with role 'Staff'
                string query = "SELECT COUNT(UserID) as totalStaff FROM userInfo WHERE UserRole = 'Staff'";

                // Create a connection using the connection string
                using (SqlConnection con = new SqlConnection(conString))
                {
                    // Create SqlCommand and associate it with the connection
                    SqlCommand cmd = new SqlCommand(query, con);

                    // Open the connection
                    con.Open();

                    // Execute query and read the result
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        // Check if the value is not DBNull and update the label
                        if (reader["totalStaff"] != DBNull.Value)
                        {
                            int totalStaff = (int)reader["totalStaff"];
                            label3.Text = totalStaff.ToString(); // Change label text to total staff count
                        }
                    }

                    // Close the reader (implicitly done when using 'using' statements)
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message); // Handle any errors
            }
        }
    }

}