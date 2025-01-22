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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        
        private void button1_Click(object sender, EventArgs e)
        {
            string userID = textBox1.Text;
            string userPassword = textBox2.Text;

            if (string.IsNullOrWhiteSpace(userPassword) || string.IsNullOrWhiteSpace(userID))
            {
                MessageBox.Show("Please enter both Id and Name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string connectionString = "data source=SUSANTA-ROG\\SQLEXPRESS; database=BarberBD; integrated security=SSPI";

            // Update the query to also fetch the UserRole for the given credentials
            string query = "SELECT UserRole FROM userInfo WHERE UserID = @UserID AND UserPass COLLATE SQL_Latin1_General_CP1_CS_AS = @UserPass";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", userID);
                    command.Parameters.AddWithValue("@UserPass", userPassword);

                    connection.Open();

                    // Use ExecuteScalar to get the UserRole
                    var result = command.ExecuteScalar();

                    if (result != null)
                    {
                        string userRole = result.ToString();

                        MessageBox.Show("Login successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Hide();

                        // Navigate to the appropriate form based on the role
                        if (userRole == "Customer")
                        {
                            CustomerForm2 customerForm = new CustomerForm2(int.Parse(userID));
                            customerForm.Show();
                        }
                        else
                        {
                            Form2 f2 = new Form2(int.Parse(userID));
                            f2.Show();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid Id or Name.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            SignUp s1 = new SignUp();
            s1.Show();
        }

        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                // Show the password
                textBox2.PasswordChar = '\0';
            }
            else
            {
                // Hide the password
                textBox2.PasswordChar = '*';
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            ForgotPassword f1 = new ForgotPassword();
            f1.Show();
        }
    }
}