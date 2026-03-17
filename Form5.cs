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

namespace ABC_Car_Traders
{
    public partial class OrderCarPart : Form
    {
        public OrderCarPart()
        {
            InitializeComponent();
            PopulateStatusComboBox();
            LoadAvailableCarParts();
            BindDataToGridView();
        }

        private void BindDataToGridView()
        {
            // SQL Query to get Order Car Parts Data
            string sql = @"
                SELECT 
                    OrderID, 
                    CustomerID, 
                    PartID, 
                    Quantity, 
                    OrderDate, 
                    StatusCode, 
                    TotalAmount 
                FROM 
                    CarPartsOrder";

            // Execute the query and get data
            DataTable dt = GetData(sql);

            // Bind the data to the DataGridView
            dataGridView1.DataSource = dt;
        }

        private DataTable GetData(string sql)
        {
            DataTable dt = new DataTable();
            string connectionString = @"server=DESKTOP-G7QOK8K; database=ABC_Car_Traders;trusted_connection=yes;"; // Replace with your actual connection string

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                    da.Fill(dt);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error retrieving data: " + ex.Message);
                }
            }
            return dt;
        }

        private void PopulateStatusComboBox()
        {
            string sql = "SELECT StatusCode, StatusName FROM Status";
            cmbStatus.DataSource = DataHelper.getData(sql).Tables[0];
            cmbStatus.DisplayMember = "StatusName";
            cmbStatus.ValueMember = "StatusCode";
            cmbStatus.SelectedIndex = -1;  // Clear selection initially
        }

        private void LoadAvailableCarParts()
        {
            // SQL query to get available car parts
            string sql = "SELECT PartID, PartName, StockQuantity, Price FROM CarParts WHERE StockQuantity > 0";

            DataSet ds = DataHelper.getData(sql);  // Assuming DataHelper.getData() returns a DataSet

            if (ds.Tables[0].Rows.Count > 0)
            {
                dataGridView1.DataSource = ds.Tables[0];  // Bind the data to the DataGridView
            }
            else
            {
                MessageBox.Show("No car parts available in the store.");
                dataGridView1.DataSource = null;
            }
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

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (DataValid())
            {
                // Create an order object
                CarPartsOrder newOrder = new CarPartsOrder
                {
                    //OrderID = Convert.ToInt32(txtOrderID.Text),
                    CustomerID = Convert.ToInt32(txtCustomerID.Text),
                    PartID = Convert.ToInt32(txtPartID.Text),
                    Quantity = Convert.ToInt32(txtQuantity.Text),
                    OrderDate = dateTimePicker1.Value.ToString("yyyy-MM-dd"),
                    StatusCode = cmbStatus.SelectedValue.ToString(),
                    TotalAmount = Convert.ToDecimal(txtTotalAmount.Text)
                };

                // Insert the new order into the database
                newOrder.AddOrder(newOrder);

                // Refresh the DataGridView (if necessary) or notify the user
                MessageBox.Show("Order successfully placed!");

                // Clear the form fields
                ClearFields();
            }
        }

        private bool DataValid()
        {
            // Add validation for the form fields
            if (string.IsNullOrEmpty(txtCustomerID.Text) || string.IsNullOrEmpty(txtPartID.Text) ||
                string.IsNullOrEmpty(txtQuantity.Text) || string.IsNullOrEmpty(txtTotalAmount.Text))
            {
                MessageBox.Show("Please fill in all the fields.");
                return false;
            }

            if (!int.TryParse(txtQuantity.Text, out _) || !decimal.TryParse(txtTotalAmount.Text, out _))
            {
                MessageBox.Show("Please enter valid numeric values for Quantity and Total Amount.");
                return false;
            }

            if (cmbStatus.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a status.");
                return false;
            }

            return true;
        }

        private void ClearFields()
        {
            txtOrderID.Text = "";
            txtCustomerID.Text = "";
            txtPartID.Text = "";
            txtQuantity.Text = "";
            dateTimePicker1.Value = DateTime.Now;
            cmbStatus.SelectedIndex = -1;
            txtTotalAmount.Text = "";
        }

        private void OrderCarPart_Load(object sender, EventArgs e)
        {

        }
    }
    public static class DbHelper
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

        public static DataSet getData(string sql)
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
    public class CarPartsOrder
    {
        public int CustomerID { get; set; }
        public int PartID { get; set; }
        public int Quantity { get; set; }
        public string OrderDate { get; set; }
        public string StatusCode { get; set; }
        public decimal TotalAmount { get; set; }

        public void AddOrder(CarPartsOrder order)
        {
            // SQL query to insert the new car parts order
            string sql = @"INSERT INTO CarPartsOrder (CustomerID, PartID, Quantity, OrderDate, StatusCode, TotalAmount) 
                       VALUES (" + order.CustomerID + ", " + order.PartID + ", " + order.Quantity + ", '" + order.OrderDate + "', '"
                           + order.StatusCode + "', " + order.TotalAmount + ")";

            // Execute the query
            DataHelper.ExecuteQuery(sql);
        }
    }
}
