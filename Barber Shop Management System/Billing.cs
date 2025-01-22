using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Barber_Shop_Management_System
{
    public partial class Billing : Form
    {
        string connectionString = "data source=SUSANTA-ROG\\SQLEXPRESS; database=BarberBD; integrated security=SSPI";
        DataTable addedProductsTable = new DataTable();
        int id;

        public Billing(int id1)
        {
            id = id1;
            InitializeComponent();
            LoadProductData();
            LoadCategoryData();
            InitializeAddedProductsTable();
        }

        private void LoadProductData()
        {
            string query = "SELECT * FROM Product";
            FillDataGridView(dataGridView1, query);
        }

        private void LoadCategoryData()
        {
            string query = "SELECT * FROM Category";
            FillDataGridView(dataGridView2, query);
        }

        private void FillDataGridView(DataGridView gridView, string query)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    con.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    DataTable dataTable = new DataTable();
                    dataTable.Load(reader);
                    gridView.DataSource = dataTable;
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                textBox1.Text = row.Cells["ProductID"].Value.ToString();
                textBox2.Text = row.Cells["ProductName"].Value.ToString();
                textBox3.Text = row.Cells["ProductPrice"].Value.ToString();
                // textBox4.Text = row.Cells["ProductQuantity"].Value.ToString();

            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView2.Rows[e.RowIndex];
                textBox6.Text = row.Cells["CategoryID"].Value.ToString();
                textBox5.Text = row.Cells["CategoryName"].Value.ToString();
                textBox10.Text = row.Cells["CategoryPrice"].Value.ToString();

            }
        }

        private void InitializeAddedProductsTable()
        {
            addedProductsTable.Columns.Add("ProductID", typeof(string));
            addedProductsTable.Columns.Add("ProductName", typeof(string));
            addedProductsTable.Columns.Add("ProductPrice", typeof(string));
            addedProductsTable.Columns.Add("ProductQuantity", typeof(string));
            addedProductsTable.Columns.Add("CategoryID", typeof(string));
            addedProductsTable.Columns.Add("CategoryName", typeof(string));
            addedProductsTable.Columns.Add("CategoryPrice", typeof(string));
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) ||
                string.IsNullOrWhiteSpace(textBox2.Text) ||
                string.IsNullOrWhiteSpace(textBox3.Text) ||
                string.IsNullOrWhiteSpace(textBox4.Text))
            {
                MessageBox.Show("Please fill in all product details.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            addedProductsTable.Rows.Add(
                textBox1.Text,
                textBox2.Text,
                textBox3.Text,
                textBox4.Text,
                null,
                null
            );

            //ClearProductTextBoxes();
            MessageBox.Show("Product added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            CalculateTotalPrice();
            UpdateTransposedView();
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox6.Text) || string.IsNullOrWhiteSpace(textBox5.Text) || string.IsNullOrWhiteSpace(textBox10.Text))
            {
                MessageBox.Show("Please fill in all category details.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (addedProductsTable.Rows.Count == 0)
            {
                MessageBox.Show("Please add product details first.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataRow lastRow = addedProductsTable.Rows[addedProductsTable.Rows.Count - 1];
            lastRow["CategoryID"] = textBox6.Text;
            lastRow["CategoryName"] = textBox5.Text;
            lastRow["CategoryPrice"] = textBox10.Text;

           // ClearCategoryTextBoxes();
            MessageBox.Show("Category details added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            CalculateTotalPrice();
            UpdateTransposedView();
        }

        private void CalculateTotalPrice()
        {
            decimal totalPrice = 0;

            foreach (DataRow row in addedProductsTable.Rows)
            {
                if (row["ProductPrice"] != DBNull.Value && row["ProductQuantity"] != DBNull.Value && row["CategoryPrice"] != DBNull.Value)
                {
                    decimal productPrice = Convert.ToDecimal(row["ProductPrice"]);
                    int productQuantity = Convert.ToInt32(row["ProductQuantity"]);
                    decimal categoryPrice = Convert.ToDecimal(row["CategoryPrice"]);

                    totalPrice += categoryPrice + (productPrice * productQuantity);
                }
            }

            textBox7.Text = totalPrice.ToString("F2");
        }

        private void UpdateTransposedView()
        {
            DataTable transposedTable = new DataTable();

            transposedTable.Columns.Add("Details");

            for (int i = 0; i < addedProductsTable.Rows.Count; i++)
            {
                transposedTable.Columns.Add($"Service {i + 1}");
            }

            for (int j = 0; j < addedProductsTable.Columns.Count; j++)
            {
                DataRow newRow = transposedTable.NewRow();
                newRow["Details"] = addedProductsTable.Columns[j].ColumnName;

                for (int i = 0; i < addedProductsTable.Rows.Count; i++)
                {
                    newRow[$"Service {i + 1}"] = addedProductsTable.Rows[i][j];
                }

                transposedTable.Rows.Add(newRow);
            }

            dataGridView3.DataSource = transposedTable;
        }

        private void ClearProductTextBoxes()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
        }

        private void ClearCategoryTextBoxes()
        {
            textBox6.Clear();
            textBox5.Clear();
            textBox10.Clear();

        }

        private void button5_Click(object sender, EventArgs e)
        {
            PrintPreviewDialog printPreviewDialog = new PrintPreviewDialog();
            PrintDocument printDocument = new PrintDocument();

            printDocument.PrintPage += PrintDocument_PrintPage;
            printPreviewDialog.Document = printDocument;

            printPreviewDialog.ShowDialog();

            // Validate that all necessary fields are filled
            if (string.IsNullOrWhiteSpace(textBox7.Text) ||
                string.IsNullOrWhiteSpace(textBox8.Text) ||
                string.IsNullOrWhiteSpace(textBox9.Text))
            {
                MessageBox.Show("Please ensure all fields (TotalBill, CustomerID, CustomerPhone) are filled before proceeding.",
                    "Validation Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            // Retrieve the data from the textboxes and form fields
            decimal totalBill = Convert.ToDecimal(textBox7.Text);
            string customerId = textBox8.Text;
            string customerPhone = textBox9.Text;
            string billingDate = dateTimePicker1.Value.ToShortDateString();
            string categoryId = textBox6.Text; // CategoryID from TextBox6
            string productId = textBox1.Text;  // ProductID from TextBox1

            // Insert the data into the Billing table using a parameterized query//[CategoryID]
            string query = "INSERT INTO Billing (TotalBill, CustomerID, BillingDate, CategoryID, ProductID) VALUES (@TotalBill, @CustomerID, @BillingDate, @CategoryID, @ProductID)";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    // Add parameters to the command
                    command.Parameters.AddWithValue("@TotalBill", totalBill);
                    command.Parameters.AddWithValue("@CustomerID", customerId);
                    command.Parameters.AddWithValue("@BillingDate", billingDate);
                    command.Parameters.AddWithValue("@CategoryID", categoryId);
                    command.Parameters.AddWithValue("@ProductID", productId);

                    try
                    {
                        con.Open();
                        int rowsAffected = command.ExecuteNonQuery(); // Execute the insert query

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Billing details have been successfully inserted into the database.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Failed to insert billing details.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"An error occurred while inserting the data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            // Optionally, you can clear the form or reset fields here
            // Example: textBox7.Clear(); textBox8.Clear(); textBox9.Clear();
        }


        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            int x = 50;
            int y = 60;
            int rowHeight = 40;
            int columnWidth = 140;

            e.Graphics.DrawString(" -----------Barber Shop----------- ", new Font("Arial", 25, FontStyle.Bold), Brushes.Black, x, y);
            y += rowHeight;

            e.Graphics.DrawString("Customer Details", new Font("Arial", 16, FontStyle.Bold), Brushes.Black, x, y);
            y += rowHeight;

            e.Graphics.DrawString($"Customer ID: {textBox8.Text}", new Font("Arial", 16), Brushes.Black, x, y);
            y += rowHeight;
            e.Graphics.DrawString($"Customer Phone: {textBox9.Text}", new Font("Arial", 16), Brushes.Black, x, y);
            y += rowHeight;
            e.Graphics.DrawString($"Billing Date: {dateTimePicker1.Value.ToShortDateString()}", new Font("Arial", 16), Brushes.Black, x, y);
            y += rowHeight;

            for (int i = 0; i < dataGridView3.Columns.Count; i++)
            {
                e.Graphics.DrawString(dataGridView3.Columns[i].HeaderText, new Font("Arial", 13, FontStyle.Bold), Brushes.Black, x, y);
                x += columnWidth;
            }

            y += rowHeight;
            x = 50;

            foreach (DataGridViewRow row in dataGridView3.Rows)
            {
                if (row.IsNewRow) continue;

                for (int i = 0; i < dataGridView3.Columns.Count; i++)
                {
                    e.Graphics.DrawString(row.Cells[i].Value?.ToString() ?? string.Empty, new Font("Arial", 12), Brushes.Black, x, y);
                    x += columnWidth;
                }

                y += rowHeight;
                x = 50;
            }

            e.Graphics.DrawString($"Total Bill : {textBox7.Text}", new Font("Arial", 16), Brushes.Black, x, y);
            y += rowHeight + 20;

            string paymentMethod = radioButton1.Checked ? "Cash" : radioButton2.Checked ? "Card" : "Not Selected";
            e.Graphics.DrawString($"Payment Method: {paymentMethod}", new Font("Arial", 16), Brushes.Black, x, y);
            y += rowHeight;

            e.Graphics.DrawString("----------------------------------------", new Font("Arial", 20, FontStyle.Bold), Brushes.Black, x, y);
            y += rowHeight;

            e.Graphics.DrawString("THANKS!!!!", new Font("Arial", 16, FontStyle.Bold), Brushes.Black, x, y);
            y += rowHeight;
        }


        private void button2_Click_1(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            textBox6.Clear();
            textBox5.Clear();
            textBox10.Clear();
        }

      
    }
}
