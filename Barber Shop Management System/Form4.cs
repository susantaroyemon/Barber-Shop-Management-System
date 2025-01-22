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
    public partial class Form4 : Form
    {
        int id;//
        public static int currentUserID; // Declare a static variable to hold the logged-in user's ID

        


        string connectionString = "data source=SUSANTA-ROG\\SQLEXPRESS; database=BarberBD; integrated security=SSPI";

        string currentUserRole = ""; // Declare a variable to store the current user's role

        public Form4(int i)
        {
            id = i;
            
            currentUserID = id; // Set this to the logged-in user's ID
            InitializeComponent();
            string query = "SELECT * FROM userInfo";
            FillDataGridView(query);
            LoadDetails(); // This will load user details and role
        }

        private void LoadDetails()
        {
            string query = "SELECT UserID, UserName, UserPNo, UserAdd, UserEmail, UserRole FROM userInfo WHERE UserID = @UserID";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", id);
                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        // Populate the text boxes with the retrieved data
                        textBox1.Text = reader["UserID"].ToString();
                        textBox2.Text = reader["UserName"].ToString();
                        textBox4.Text = reader["UserPNo"].ToString();
                        textBox6.Text = reader["UserEmail"].ToString();
                        richTextBox1.Text = reader["UserAdd"].ToString();

                        currentUserRole = reader["UserRole"].ToString(); // Store the role of the current user
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

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form3 f3 = new Form3(id);
            f3.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string UserId = textBox1.Text.Trim();
            string UserName = textBox2.Text.Trim();
            string UserAdd = richTextBox1.Text.Trim();
            string UserPNo = textBox4.Text.Trim();
            string UserEmail = textBox6.Text.Trim();

            // Check if the logged-in user is a staff member
            if (currentUserRole == "Stuff")
            {
                // Prevent staff from updating admin information
                string query = "SELECT UserRole FROM userInfo WHERE UserID = @UserID";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserID", UserId);
                        connection.Open();

                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            string targetRole = reader["UserRole"].ToString();

                            // If the user is trying to update an admin, show an error message
                            if (targetRole == "Admin")
                            {
                                MessageBox.Show("You can't make changes in admin information.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return; // Prevent update operation
                            }
                        }
                    }
                }
            }

            // Validation and update logic
            if (string.IsNullOrWhiteSpace(UserName) || string.IsNullOrWhiteSpace(UserAdd) || string.IsNullOrWhiteSpace(UserPNo)
                || string.IsNullOrWhiteSpace(UserEmail) || string.IsNullOrWhiteSpace(UserId))
            {
                MessageBox.Show("All fields must be filled out.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string updateQuery = "UPDATE userInfo SET UserName = @UserName, UserPNo = @UserPNo, UserAdd = @UserAdd, UserEmail = @UserEmail WHERE UserID = @UserID";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@UserId", UserId);
                    command.Parameters.AddWithValue("@UserName", UserName);
                    command.Parameters.AddWithValue("@UserPNo", UserPNo);
                    command.Parameters.AddWithValue("@UserAdd", UserAdd);
                    command.Parameters.AddWithValue("@UserEmail", UserEmail);

                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Record updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Close(); // Optionally close the form after a successful update
                    }
                    else
                    {
                        MessageBox.Show("No record was updated. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            string searchValue = textBox7.Text.Trim();

            if (string.IsNullOrWhiteSpace(searchValue))
            {
                MessageBox.Show("Please enter a search term.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string query = @"SELECT * FROM userInfo 
         WHERE UserId LIKE @searchTerm 
            OR UserPNo LIKE @searchTerm 
            OR UserName LIKE @searchTerm 
            OR UserRole LIKE @searchTerm
            OR UserAdd LIKE @searchTerm 
            OR UserEmail LIKE @searchTerm";


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

        private void button4_Click(object sender, EventArgs e)
        {
            // Prevent staff from deleting admin users
            if (currentUserRole == "Stuff")
            {
                string query = "SELECT UserRole FROM userInfo WHERE UserID = @UserID";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserID", id);
                        connection.Open();

                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            string targetRole = reader["UserRole"].ToString();

                            // If the user is trying to delete an admin, show an error message
                            if (targetRole == "Admin")
                            {
                                MessageBox.Show("You can't delete admin information.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return; // Prevent delete operation
                            }
                        }
                    }
                }
            }

            // Confirm deletion
            DialogResult result = MessageBox.Show(
                "Are you sure you want to delete this profile?",
                "Confirm Deletion",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );
            string userId = textBox1.Text.Trim();

            if (result == DialogResult.Yes)
            {
                string deleteQuery = "DELETE FROM userInfo WHERE UserID = @UserID";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(deleteQuery, connection))
                    {
                        command.Parameters.AddWithValue("@UserID", userId);

                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Profile deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Check if the deleted profile belongs to the current logged-in user
                            if (int.Parse(userId) == currentUserID)  // Assuming currentUserID is the logged-in user's ID
                            {
                                // If the deleted profile is the current user's profile, log out
                                MessageBox.Show("Your profile has been deleted. You will be logged out.", "Logged Out", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                Application.Exit(); // Log out the current user
                            }
                            
                        }
                        else
                        {
                            MessageBox.Show("No profile was found to delete.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }



        private void button5_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form5 f4 = new Form5(id);
            f4.Show();

        }

        private void button6_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
