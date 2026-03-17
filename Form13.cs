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

namespace ABC_Car_Traders
{
    public partial class TrackOrder : Form
    {
        public TrackOrder()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Hide();
            CusHomePage home = new CusHomePage();
            home.Show();
        }

        private void btnTrack_Click(object sender, EventArgs e)
        {
            string OrderID = txtOrderID.Text;

            if (string.IsNullOrEmpty(OrderID))
            {
                MessageBox.Show("Please enter an Order ID.");
                return;
            }

            // Query to retrieve the order details
            string query = @"
                SELECT OrderID, CustomerID, PartID, Quantity, OrderDate, StatusCode, TotalAmount 
                FROM CarPartsOrder 
                WHERE OrderID = @OrderID";

            // Retrieve data from the database
            DataTable orderData = GetOrderData(query, OrderID);

            if (orderData.Rows.Count > 0)
            {
                // Bind the data to a DataGridView or display it in Labels
                dataGridView1.DataSource = orderData;
            }
            else
            {
                MessageBox.Show("Order not found.");
                dataGridView1.DataSource = null;
            }
        }

        private DataTable GetOrderData(string query, string OrderID)
        {
            DataTable dt = new DataTable();
            string connectionString = @"server=DESKTOP-G7QOK8K; database=ABC_Car_Traders;trusted_connection=yes;""/>"; // Update this with your actual connection string

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@OrderID", OrderID);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error retrieving data: " + ex.Message);
                }
            }

            return dt;
        }
    }
}
