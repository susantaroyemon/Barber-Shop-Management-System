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
    public partial class Payment : Form
    {
        string connectionString = "data source=SUSANTA-ROG\\SQLEXPRESS; database=BarberBD; integrated security=SSPI";
        int idf2;
        public Payment(int i2)
        {
            idf2 = i2;
            InitializeComponent();
        }

        

        private void cardBtn_Click_1(object sender, EventArgs e)
        {
            if (panelCard.Visible)
            {
                mobileBankingPanel.Visible = false;
            }
            else
            {
                panelCard.Visible = true;
                mobileBankingPanel.Visible = false;
            }
        }

        private void rocketBtn_Click_1(object sender, EventArgs e)
        {
            if (panelCard.Visible)
            {
                panelCard.Visible = false;
            }
            else
            {
                panelCard.Visible = false;
                mobileBankingPanel.Visible = true;
            }
        }

        private void nagadBtn_Click_1(object sender, EventArgs e)
        {
            if (panelCard.Visible)
            {
                panelCard.Visible = false;
            }
            else
            {
                panelCard.Visible = false;
                mobileBankingPanel.Visible = true;
            }
        }

        private void bkashBtn_Click_1(object sender, EventArgs e)
        {
            if (panelCard.Visible)
            {
                panelCard.Visible = false;
            }
            else
            {
                panelCard.Visible = false;
                mobileBankingPanel.Visible = true;
            }
        }

        

        private void button2_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            CustomerForm2 f2 = new CustomerForm2(idf2);
            f2.Show();
        }
        //
        private void mobileBankingBtn_Click(object sender, EventArgs e)
        {
            // Input data from user (for example, these can be retrieved from text boxes on the form)
            string phoneNoInput = txtPhone.Text;
            string pinInput = txtPin.Text;

            // Check data in the database
            if (ValidateMobileBankingPayment(phoneNoInput, pinInput))
            {
                MessageBox.Show("Payment Successful", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Invalid Mobile Banking Information", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cardMakePaymentbtn_Click_1(object sender, EventArgs e)
        {
            // Get user input for verification
            string chName = txtCardHolderName.Text; // TextBox for cardholder's name
            string cardNo = txtCardNumber.Text; // TextBox for card number

            // Verify data with the database
            if (VerifyCardPaymentDetails(chName, cardNo))
            {
                MessageBox.Show("Payment Successful", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Invalid details. Payment failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private bool ValidateMobileBankingPayment(string phoneNo, string pin)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM Payment WHERE PhoneNo = @PhoneNo AND Pin = @Pin";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@PhoneNo", phoneNo);
                command.Parameters.AddWithValue("@Pin", pin);

                connection.Open();
                int count = (int)command.ExecuteScalar();
                return count > 0; // Return true if a match is found
            }
        }

        private bool VerifyCardPaymentDetails(string chName, string cardNo)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // SQL query to verify CHName and CardNo
                string query = "SELECT COUNT(1) FROM [Payment] WHERE [CHName] = @CHName AND [CardNo] = @CardNo";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@CHName", chName);
                command.Parameters.AddWithValue("@CardNo", cardNo);

                connection.Open();
                int count = Convert.ToInt32(command.ExecuteScalar());
                return count == 1; // Return true if a matching record is found
            }
        }

    }
}
