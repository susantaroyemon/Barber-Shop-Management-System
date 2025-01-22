using System;
using System.Collections;
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
    public partial class CusService : Form

    {
        int id;

        string connectionString = "data source=SUSANTA-ROG\\SQLEXPRESS; database=BarberBD; integrated security=SSPI";//BarBer

        public CusService(int i)
        {
            id = i;
            InitializeComponent();
            
            string query = "SELECT * FROM Service";
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
        
    }
}
