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
    public partial class Form5 : Form
    {
        int id;//
        public Form5(int i)
        {
            id = i;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string connectionString = "data source=SUSANTA-ROG\\SQLEXPRESS; database=BarberBD; integrated security=SSPI";

            string currentPassword = textBox3.Text;
            string newPassword = textBox1.Text;
            string confirmNewPassword = textBox2.Text;
            

            // Validation: Ensure all fields are filled and the new password matches the confirm password.
            if (string.IsNullOrWhiteSpace(currentPassword) || string.IsNullOrWhiteSpace(newPassword) || string.IsNullOrWhiteSpace(confirmNewPassword))
            {
                MessageBox.Show("All fields must be filled out.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (newPassword != confirmNewPassword)
            {
                MessageBox.Show("New password and confirm password do not match.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Step 1: Check if the current password is correct.
            string queryCheckPassword = "SELECT UserPass FROM userInfo WHERE UserID = @UserID";
            string storedPassword = "";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(queryCheckPassword, connection))
                {
                    command.Parameters.AddWithValue("@UserID", id);
                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        storedPassword = reader["UserPass"].ToString();
                    }
                    reader.Close();
                }
            }

            // Step 2: If the current password is incorrect, show an error.
            if (currentPassword != storedPassword)
            {
                MessageBox.Show("Current password is incorrect.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Step 3: Update the password in the database.
            string queryUpdatePassword = "UPDATE userInfo SET UserPass = @NewPassword WHERE UserID = @UserID";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(queryUpdatePassword, connection))
                {
                    command.Parameters.AddWithValue("@NewPassword", newPassword); // Store the new password directly, consider hashing
                    command.Parameters.AddWithValue("@UserID", id);

                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Password changed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Hide(); // Optionally hide this form
                        Form1 f1 = new Form1(); // Optionally return to login screen
                        f1.Show();
                    }
                    else
                    {
                        MessageBox.Show("Failed to change the password. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form2 f2 = new Form2(id);
            f2.Show();
        }
    }
}
