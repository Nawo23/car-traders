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
    public partial class OrderCar : Form
    {
        public OrderCar()
        {
            InitializeComponent();
            LoadStatus();
            DatabindtoGridView();
        }
        private void DatabindtoGridView()
        {
            // Define the SQL query to fetch data from the orders table
            // Define the SQL query to fetch data from the orders table
            string sql = @"
        SELECT 
            o.CustomerID, 
            o.CarID, 
            o.OrderDate, 
            s.StatusName, 
            o.StatusCode, 
            o.TotalAmount
        FROM 
            CarOrder o
        INNER JOIN 
            Status s ON o.StatusCode = s.StatusCode
        ORDER BY 
            o.OrderDate"; // You can order by any relevant column, e.g., OrderDate

            // Fetch data from the database
            DataTable orderData = DataHelper.getData(sql).Tables[0];

            // Bind the data to the DataGridView
            dataGridView1.DataSource = orderData;

            // Set the column headers (optional)
            dataGridView1.Columns["CustomerID"].HeaderText = "Customer ID";
            dataGridView1.Columns["CarID"].HeaderText = "Car ID";
            dataGridView1.Columns["OrderDate"].HeaderText = "Order Date";
            dataGridView1.Columns["StatusName"].HeaderText = "Status";
            dataGridView1.Columns["TotalAmount"].HeaderText = "Total Amount";

            // Format the OrderDate column as a date
            dataGridView1.Columns["OrderDate"].DefaultCellStyle.Format = "yyyy-MM-dd";

            // Format the TotalAmount column as currency
            dataGridView1.Columns["TotalAmount"].DefaultCellStyle.Format = "C2";
        }

        private void LoadStatus()
        {
            string sql = "SELECT * FROM Status";
            cmbStatus.DataSource = DataHelper.getData(sql).Tables[0];
            cmbStatus.DisplayMember = "StatusName";
            cmbStatus.ValueMember = "StatusCode";
            cmbStatus.SelectedIndex = -1;  // Clear selection initially
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

        private void btnOrderCar_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                CarOrder carOrder = new CarOrder
                {
                    CustomerID = txtCustomerID.Text,
                    CarID = txtCarID.Text,
                    Quantity = Convert.ToInt32(txtQuantity.Text),
                    OrderDate = dateTimePicker1.Value.ToString("yyyy-MM-dd"),
                    StatusCode = cmbStatus.SelectedValue.ToString(),
                    TotalAmount = Convert.ToDecimal(txtTotalAmount.Text)
                };

                carOrder.PlaceOrder(carOrder);

                MessageBox.Show("Car ordered successfully!");
                ClearFields();
            }
        }

        // Method to validate the order form
        private bool ValidateInput()
        {
            if (string.IsNullOrEmpty(txtCustomerID.Text))
            {
                MessageBox.Show("Please enter Customer ID.");
                return false;
            }
            if (string.IsNullOrEmpty(txtCarID.Text))
            {
                MessageBox.Show("Please enter Car ID.");
                return false;
            }
            if (string.IsNullOrEmpty(txtQuantity.Text) || !int.TryParse(txtQuantity.Text, out _))
            {
                MessageBox.Show("Please enter a valid Quantity.");
                return false;
            }
            if (cmbStatus.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a status.");
                return false;
            }
            if (string.IsNullOrEmpty(txtTotalAmount.Text) || !decimal.TryParse(txtTotalAmount.Text, out _))
            {
                MessageBox.Show("Please enter a valid Total Amount.");
                return false;
            }
            return true;
        }

        private void ClearFields()
        {
            txtCustomerID.Clear();
            txtCarID.Clear();
            txtQuantity.Clear();
            cmbStatus.SelectedIndex = -1;
            txtTotalAmount.Clear();
            dateTimePicker1.Value = DateTime.Now;
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

                // Populate the form fields with the selected row's data
                txtCustomerID.Text = selectedRow.Cells["CustomerID"].Value.ToString();
                txtCarID.Text = selectedRow.Cells["CarID"].Value.ToString();
                txtQuantity.Text = selectedRow.Cells["Quantity"].Value.ToString();
                dateTimePicker1.Value = Convert.ToDateTime(selectedRow.Cells["OrderDate"].Value);
                cmbStatus.Text = selectedRow.Cells["StatusName"].Value.ToString(); // Assumes StatusName is displayed in the ComboBox
                txtTotalAmount.Text = selectedRow.Cells["TotalAmount"].Value.ToString();
            }
        }
    }
    public class CarOrder
    {
        public string CustomerID { get; set; }
        public string CarID { get; set; }
        public int Quantity { get; set; }
        public string OrderDate { get; set; }
        public string StatusCode { get; set; }
        public decimal TotalAmount { get; set; }

        public void PlaceOrder(CarOrder order)
        {
            string sql = @"
            INSERT INTO CarOrder (CustomerID, CarID, Quantity, OrderDate, StatusCode, TotalAmount) 
            VALUES (@CustomerID, @CarID, @Quantity, @OrderDate, @StatusCode, @TotalAmount)";

            using (SqlConnection conn = new SqlConnection(DataHelper.cs))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@CustomerID", order.CustomerID);
                    cmd.Parameters.AddWithValue("@CarID", order.CarID);
                    cmd.Parameters.AddWithValue("@Quantity", order.Quantity);
                    cmd.Parameters.AddWithValue("@OrderDate", order.OrderDate);
                    cmd.Parameters.AddWithValue("@StatusCode", order.StatusCode);
                    cmd.Parameters.AddWithValue("@TotalAmount", order.TotalAmount);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
        }
    }
}
