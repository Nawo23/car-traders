using ABC_Car_Traders;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace ABC_Car_Traders
{
    public partial class CusOrders : Form
    {
        public CusOrders()
        {
            InitializeComponent();
            GetStatus();
            DatabindtoGridView();
        }

        private void DatabindtoGridView()
        {
            string sql = @"
            SELECT 
                o.OrderID,
                o.CustomerID,
                o.OrderDate, 
                s.StatusName, 
                o.StatusCode, 
                o.TotalAmount
            FROM 
                CustomerOrder o
            INNER JOIN 
                Customer c ON o.CustomerID = c.CustomerID
            INNER JOIN 
                Status s ON o.StatusCode = s.StatusCode
            ORDER BY 
                o.OrderID";

            dataGridView1.DataSource = DataHelp.GetData(sql).Tables[0];

            // Customize column headers and format
            dataGridView1.Columns["OrderID"].HeaderText = "OrderID";
            dataGridView1.Columns["CustomerID"].HeaderText = "CustomerID";
            dataGridView1.Columns["OrderDate"].HeaderText = "OrderDate";
            dataGridView1.Columns["OrderDate"].DefaultCellStyle.Format = "yyyy-MM-dd"; // Format date
            dataGridView1.Columns["TotalAmount"].HeaderText = "TotalAmount";
            dataGridView1.Columns["TotalAmount"].DefaultCellStyle.Format = "C2"; // Format as currency
        }

        private void GetStatus()
        {
            string sql = "SELECT * FROM Status";
            cmbStatus.DataSource = DataHelp.GetData(sql).Tables[0];
            cmbStatus.DisplayMember = "StatusName";
            cmbStatus.ValueMember = "StatusCode";
            cmbStatus.SelectedIndex = -1;
        }


        private void BtnBack_Click(object sender, EventArgs e)
        {
            this.Hide();
            HomePage home = new HomePage();
            home.Show();
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void DataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

                txtOrderID.Text = selectedRow.Cells["OrderID"].Value.ToString();
                txtCustomerID.Text = selectedRow.Cells["CustomerID"].Value.ToString();
                dtpOrderDate.Value = Convert.ToDateTime(selectedRow.Cells["OrderDate"].Value);
                cmbStatus.SelectedValue = selectedRow.Cells["StatusCode"].Value.ToString();
                txtTotalAmount.Text = selectedRow.Cells["TotalAmount"].Value.ToString();
                txtCustomerID.Text = selectedRow.Cells["CustomerID"].Value.ToString();
            }
        }

        private void ClearOrderFields()
        {
            txtOrderID.Clear();
            dtpOrderDate.Value = DateTime.Now;
            cmbStatus.SelectedIndex = -1;
            txtTotalAmount.Clear();
            txtCustomerID.Clear();
        }

        private void BtnDeleteOrder_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtOrderID.Text))
            {
                MessageBox.Show("Please select an order to delete.");
                return;
            }

            // Confirm deletion
            DialogResult result = MessageBox.Show("Are you sure you want to delete this order?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                // Get the OrderID from the TextBox
                int orderID = Convert.ToInt32(txtOrderID.Text);

                // Build SQL to delete the order
                string sql = "DELETE FROM CustomerOrder WHERE OrderID = " + orderID;

                // Execute the query
                DataHelp.ExecuteQuery(sql);

                // Refresh the DataGridView to remove the deleted order
                DatabindtoGridView();

                // Optionally, clear the form fields after deletion
                ClearOrderFields();

                MessageBox.Show("Order deleted successfully.");
            }
        }

        private bool GetDataValid()
        {
            if (string.IsNullOrWhiteSpace(txtOrderID.Text))
            {
                MessageBox.Show("Please enter Order ID");
                return false;
            }
            if (dtpOrderDate.Value == null || dtpOrderDate.Value > DateTime.Now)
            {
                MessageBox.Show("Please enter a valid Order Date");
                return false;
            }
            if (cmbStatus.SelectedIndex == -1)
            {
                MessageBox.Show("Please select Status");
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtTotalAmount.Text))
            {
                MessageBox.Show("Please enter Total Amount");
                return false;
            }
            if (!decimal.TryParse(txtTotalAmount.Text, out _))
            {
                MessageBox.Show("Please enter a valid numeric value for Total Amount");
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtCustomerID.Text))
            {
                MessageBox.Show("Please enter CustomerID");
                return false;
            }
            return true;
        }

        private void BtnExit_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void BtnBack_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            HomePage home = new HomePage();
            home.Show();
        }

        private void BtnUpdateOrder_Click(object sender, EventArgs e)
        {
            try
            {
                // Ensure all necessary fields are filled
                if (string.IsNullOrEmpty(txtOrderID.Text) ||
                    string.IsNullOrEmpty(txtCustomerID.Text) ||
                    string.IsNullOrEmpty(txtTotalAmount.Text) ||
                    cmbStatus.SelectedItem == null)
                {
                    MessageBox.Show("Please fill all fields (OrderID, CustomerID, OrderDate, Status, and TotalAmount).", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Get the values from the form
                int orderId = int.Parse(txtOrderID.Text);
                int customerId = int.Parse(txtCustomerID.Text);
                DateTime orderDate = dtpOrderDate.Value;
                string status = cmbStatus.SelectedItem.ToString();
                decimal totalAmount = decimal.Parse(txtTotalAmount.Text);

                // Create an updated order object
                Order updatedOrder = new Order
                {
                    OrderID = orderId,
                    CustomerID = customerId,
                    OrderDate = orderDate,
                    Status = status,
                    TotalAmount = totalAmount
                };

                // Call the DatabaseManager method to update the order
                DatabaseManager.UpdateOrder(updatedOrder);

                MessageBox.Show("Order updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating order: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnDeleteOrder_Click_1(object sender, EventArgs e)
        {
            try
            {
                // Ensure that the OrderID is provided
                if (string.IsNullOrEmpty(txtOrderID.Text))
                {
                    MessageBox.Show("Please enter the OrderID to delete the order.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Get the OrderID from the text box
                int orderId = int.Parse(txtOrderID.Text);

                // Ask for confirmation before deleting the order
                DialogResult result = MessageBox.Show("Are you sure you want to delete this order?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    // Call the method to delete the order from the database
                    DatabaseManager.DeleteOrder(orderId);

                    MessageBox.Show("Order deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting order: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                // Ensure all necessary fields are filled
                if (string.IsNullOrEmpty(txtOrderID.Text) ||
                    string.IsNullOrEmpty(txtCustomerID.Text) ||
                    string.IsNullOrEmpty(txtTotalAmount.Text) ||
                    cmbStatus.SelectedItem == null)
                {
                    MessageBox.Show("Please fill all fields (OrderID, CustomerID, OrderDate, Status, and TotalAmount).", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Get the values from the form controls
                int orderId = int.Parse(txtOrderID.Text);
                int customerId = int.Parse(txtCustomerID.Text);
                DateTime orderDate = dtpOrderDate.Value;
                string status = cmbStatus.SelectedItem.ToString();
                decimal totalAmount = decimal.Parse(txtTotalAmount.Text);

                // Create a new order object
                Order newOrder = new Order
                {
                    OrderID = orderId,
                    CustomerID = customerId,
                    OrderDate = orderDate,
                    Status = status,
                    TotalAmount = totalAmount
                };

                // Call the method to add the order to the database
                DatabaseManager.AddOrder(newOrder);

                MessageBox.Show("Order added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Optionally, clear the form fields after adding the order
                ClearOrderForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding order: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Method to clear the form fields after adding the order
        private void ClearOrderForm()
        {
            txtOrderID.Clear();
            txtCustomerID.Clear();
            txtTotalAmount.Clear();
            cmbStatus.SelectedIndex = -1;
            dtpOrderDate.Value = DateTime.Now;
        }
    }
    }
    class DataHelp
    {
        public static string cs = ConfigurationManager.ConnectionStrings["dbcon"].ToString();

        public static void ExecuteQuery(string sql)
        {
            using (SqlConnection conn = new SqlConnection(cs))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

    public static DataSet GetData(string sql)
        {
            using (SqlConnection conn = new SqlConnection(cs))
            {
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds;
            }
        }
    }
    class CustomerOrder
    {
    private static string connectionString;

    public int OrderID { get; set; }
        public DateTime OrderDate { get; set; }
        public string StatusCode { get; set; }
        public decimal TotalAmount { get; set; }
        public int CustomerID { get; set; }

        // Method to place a new order
        public void PlaceOrder(CustomerOrder order)
        {
            string sql = @"INSERT INTO CustomerOrder (OrderDate, StatusCode, TotalAmount, CustomerID) VALUES 
                     ('" + order.OrderDate.ToString("yyyy-MM-dd") + "', '" + order.StatusCode + "', " + order.TotalAmount + " ," + order.CustomerID + ")";
            DataHelp.ExecuteQuery(sql);
        }

        // Method to update an existing order
        public void UpdateOrder(CustomerOrder order)
        {
            string sql = @"UPDATE CustomerOrder 
                       SET CustomerID = OrderDate = '" + order.OrderDate.ToString("yyyy-MM-dd") +
                               "', StatusCode = '" + order.StatusCode + "', TotalAmount = " + order.TotalAmount + "," + order.CustomerID + 
                               " WHERE OrderID = " + order.OrderID;
            DataHelp.ExecuteQuery(sql);
        }

        // Method to delete an order
        public void DeleteOrder(CustomerOrder order)
        {
            string sql = "DELETE FROM CustomerOrder WHERE OrderID = " + order.OrderID;
            DataHelp.ExecuteQuery(sql);
        }

        // Optional: Method to get order details
        public static DataSet GetOrderDetails(int orderID)
        {
            string sql = "SELECT * FROM CustomerOrder WHERE OrderID = " + orderID;
            return DataHelp.GetData(sql);
        }
    public static void AddOrder(Order order)
    {
        string query = "INSERT INTO Orders (OrderID, CustomerID, OrderDate, Status, TotalAmount) " +
                       "VALUES (@OrderID, @CustomerID, @OrderDate, @Status, @TotalAmount)";

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@OrderID", order.OrderID);
            cmd.Parameters.AddWithValue("@CustomerID", order.CustomerID);
            cmd.Parameters.AddWithValue("@OrderDate", order.OrderDate);
            cmd.Parameters.AddWithValue("@Status", order.Status);
            cmd.Parameters.AddWithValue("@TotalAmount", order.TotalAmount);

            conn.Open();
            cmd.ExecuteNonQuery();
        }
    }
}

