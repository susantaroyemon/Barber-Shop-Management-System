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
    public partial class SignUp : Form
    {
        public SignUp()
        {
            InitializeComponent();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string connectionString = "data source=SUSANTA-ROG\\SQLEXPRESS; database=BarberBD; integrated security=SSPI";

            string UserID = IDtextBox1.Text;
            string UserName = NametextBox2.Text;
            string UserPass = textBox3.Text;
            string UserAdd = richTextBox1.Text;
            //string UserRole = textBox5.Text;
            string UserRole = "Customer";
            string UserEmail = textBox6.Text;
            string UserPNo = textBox7.Text;
            string UserSalary = "0";
            //string UserStartDate = textBox9.Text;
            string UserStartDate = dateTimePicker1.Value.ToString();

            if (string.IsNullOrWhiteSpace(UserID) || string.IsNullOrWhiteSpace(UserName) ||
               string.IsNullOrWhiteSpace(UserPass) || string.IsNullOrWhiteSpace(UserAdd) ||
               string.IsNullOrWhiteSpace(UserRole) || string.IsNullOrWhiteSpace(UserEmail) ||
               string.IsNullOrWhiteSpace(UserPNo) || string.IsNullOrWhiteSpace(UserSalary) ||
               string.IsNullOrWhiteSpace(UserStartDate))
            {
                MessageBox.Show("All fields must be filled out.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                var email = new System.Net.Mail.MailAddress(UserEmail);
            }
            catch (FormatException)
            {
                MessageBox.Show("Please enter a valid email address.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string query = "INSERT INTO userInfo (UserID, UserName, UserPass, UserPNo, UserAdd, UserStartDate, UserSalary,UserEmail,UserRole) VALUES (@UserID, @UserName, @UserPass, @UserPNo, @UserAdd, @UserStartDate, @UserSalary, @UserEmail, @UserRole)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", UserID);
                    command.Parameters.AddWithValue("@UserName", UserName);
                    command.Parameters.AddWithValue("@UserPass", UserPass);
                    command.Parameters.AddWithValue("@UserPNo", UserPNo);
                    command.Parameters.AddWithValue("@UserAdd", UserAdd);
                    command.Parameters.AddWithValue("@UserStartDate", UserStartDate);
                    command.Parameters.AddWithValue("@UserSalary", UserSalary);
                    command.Parameters.AddWithValue("@UserEmail", UserEmail);
                    command.Parameters.AddWithValue("@UserRole", UserRole);



                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Profile created successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Hide();
                        Form1 f1 = new Form1();
                        f1.Show();
                    }
                    else
                    {
                        MessageBox.Show("Failed to create the profile. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 f1 = new Form1();
            f1.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
