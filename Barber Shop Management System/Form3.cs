using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.Net.NetworkInformation;

namespace Barber_Shop_Management_System
{
    public partial class Form3 : Form
    {
        int idf3;

        public Form3(int i3)
        {
            idf3 = i3;
            InitializeComponent();
            string[] role = new string[3];
            role[0] = "Admin";
            role[1] = "Staff";
            role[2] = "Customer";

            comboBox1.DataSource = role;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string connectionString = "data source=SUSANTA-ROG\\SQLEXPRESS; database=BarberBD; integrated security=SSPI";

            string UserID = IDtextBox1.Text;
            string UserName = NametextBox2.Text;
            string UserPass = textBox3.Text;
            string UserAdd = richTextBox1.Text;
            string UserRole = comboBox1.SelectedItem.ToString();
            string UserEmail = textBox6.Text;
            string UserPNo = textBox7.Text;
            string UserSalary = textBox8.Text;
            string UserStartDate = dateTimePicker1.Value.ToString();

            if (string.IsNullOrWhiteSpace(UserID) || string.IsNullOrWhiteSpace(UserName) ||
               string.IsNullOrWhiteSpace(UserPass) || string.IsNullOrWhiteSpace(UserAdd)||
               string.IsNullOrWhiteSpace(UserRole) || string.IsNullOrWhiteSpace(UserEmail) ||
               string.IsNullOrWhiteSpace(UserPNo) || string.IsNullOrWhiteSpace(UserSalary) ||
               string.IsNullOrWhiteSpace(UserStartDate))
            {
                MessageBox.Show("All fields must be filled out.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                        
                    }
                    else
                    {
                        MessageBox.Show("Failed to create the profile. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            Form2 f2 = new Form2(idf3);
            f2.Show();
        }
    }
}
