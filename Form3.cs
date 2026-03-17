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
    public partial class CusHomePage : Form
    {
        public CusHomePage()
        {
            InitializeComponent();
        }

        private void btnSearchCarDetails_Click(object sender, EventArgs e)
        {
            this.Hide();
            SearchCarDetails home = new SearchCarDetails();
            home.Show();
        }

        private void btnSearchCarParts_Click(object sender, EventArgs e)
        {
            this.Hide();
            SearchCarParts home = new SearchCarParts();
            home.Show();
        }

        private void btnOrder_Click(object sender, EventArgs e)
        {
            this.Hide();
            OrderCar home = new OrderCar();
            home.Show();
        }

        private void btnOrderCarParts_Click(object sender, EventArgs e)
        {
            this.Hide();
            OrderCarPart home = new OrderCarPart();
            home.Show();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Hide();
            AdminLogin home = new AdminLogin();
            home.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            TrackOrder home = new TrackOrder();
            home.Show();
        }
    }
}
