using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Barber_Shop_Management_System
{
    public partial class Form2 : Form
    {
        int idf2;
        public Form2(int i2)
        {
            idf2 = i2;
            InitializeComponent();
        }
        private Form activeForm = null;

        private void OpenChildForm(Form childForm)
        {
            if (activeForm != null)
                activeForm.Close();

            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;

            panel2.Controls.Add(childForm);
            panel2.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();


        }
        private void button1_Click(object sender, EventArgs e)
        {
            
            OpenChildForm(new UserDashBoard());
            //this.Hide();
            UserDashBoard f4 = new UserDashBoard();

        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 f11 = new Form1();
            f11.Show();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Application.Exit();

        }

        private void button2_Click(object sender, EventArgs e)
        {
           
            OpenChildForm(new Form4(idf2));
            //this.Hide();
            Form4 f4 = new Form4(idf2);
            // f4.Show();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            OpenChildForm(new Appointment(idf2));
            //this.Hide();
            Appointment a4 = new Appointment(idf2);
            // f4.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            OpenChildForm(new Service(idf2));
            //this.Hide();
            Service a4 = new Service(idf2);
            // f4.Show();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenChildForm(new Category(idf2));
            //this.Hide();
            Category a4 = new Category(idf2);
            // f4.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            OpenChildForm(new Billing(idf2));
            //this.Hide();
            Billing a4 = new Billing(idf2);
            // f4.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenChildForm(new Product(idf2));
            //this.Hide();
            Product a4 = new Product(idf2);
        }
    }
}
