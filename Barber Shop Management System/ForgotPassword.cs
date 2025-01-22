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
    public partial class ForgotPassword : Form
    {
        public ForgotPassword()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string connectionString = "data source=SUSANTA-ROG\\SQLEXPRESS; database=BarberBD; integrated security=SSPI";
            string UserID = textBox1.Text.Trim();
            string UserPNo = textBox2.Text.Trim();

            if (string.IsNullOrWhiteSpace(UserID) || string.IsNullOrWhiteSpace(UserPNo))
            {
                MessageBox.Show("User ID and Phone Number are required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string query = "SELECT COUNT(*) FROM userInfo WHERE UserID = @UserID AND UserPNo = @UserPNo";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", UserID);
                    command.Parameters.AddWithValue("@UserPNo", UserPNo);

                    connection.Open();
                    int userExists = (int)command.ExecuteScalar();

                    if (userExists > 0)
                    {
                        MessageBox.Show("Details verified successfully. You can now reset your password.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Hide();
                        ResetPassword resetPasswordForm = new ResetPassword(int.Parse(UserID));
                        resetPasswordForm.Show();
                    }
                    else
                    {
                        MessageBox.Show("User ID or Phone Number is incorrect.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            Form1 f1 = new Form1();
            f1.Show();
        }
    }
}
