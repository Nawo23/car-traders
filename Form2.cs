using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ABC_Car_Traders
{
    public partial class HomePage : Form
    {
        public HomePage()
        {
            InitializeComponent();
        }

        private void btnCarDetails_Click(object sender, EventArgs e)
        {
            this.Hide();
            CarDetails home = new CarDetails();
            home.Show();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Hide();
            AdminLogin home = new AdminLogin();
            home.Show();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnCarParts_Click(object sender, EventArgs e)
        {
            this.Hide();
            CarParts home = new CarParts();
            home.Show();
        }

        private void btnCusDetails_Click(object sender, EventArgs e)
        {
            this.Hide();
            CusDetails home = new CusDetails();
            home.Show();
        }

        private void btnCusOrder_Click(object sender, EventArgs e)
        {
            this.Hide();
            CusOrders home = new CusOrders();
            home.Show();
        }

        private void btnReports_Click(object sender, EventArgs e)
        {
            this.Hide();
            GenerateReports home = new GenerateReports();
            home.Show();
        }
    }
}
