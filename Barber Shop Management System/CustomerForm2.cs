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
    public partial class CustomerForm2 : Form
    { 
    int idf2;
    public CustomerForm2(int i2)
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

    private void button9_Click(object sender, EventArgs e)
        {
            OpenChildForm(new AppointmentCustomer(idf2));
            //this.Hide();
            AppointmentCustomer a4 = new AppointmentCustomer(idf2);
            // f4.Show();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 f11 = new Form1();
            f11.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenChildForm(new DashBoard(idf2));
            //this.Hide();
            DashBoard a4 = new DashBoard(idf2);
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            OpenChildForm(new CusService(idf2));
            //this.Hide();
            CusService a4 = new CusService(idf2);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenChildForm(new CusProduct(idf2));
            //this.Hide();
            CusProduct a4 = new CusProduct(idf2);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenChildForm(new CusCategory(idf2));
            //this.Hide();
            CusCategory a4 = new CusCategory(idf2);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //OpenChildForm(new Payment());
            //this.Hide();
            Payment a4 = new Payment(idf2);
            a4.Show();
        }
    }
}
