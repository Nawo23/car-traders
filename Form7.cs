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
    public partial class CusDetails : Form
    {
        public CusDetails()
        {
            InitializeComponent();
            DatabindtoGridView();
        }
        private void DatabindtoGridView()
        {
            string sql = @"
            SELECT
                FirstName, 
                LastName, 
                DateOfBirth,
                CustomerID,
                Address, 
                PhoneNumber, 
                EmailAddress, 
                Username, 
                Password
            FROM Customers
            ORDER BY CustomerID";
            dataGridView1.DataSource = DatabaseManager.getData(sql).Tables[0];

            // Customize column headers
            dataGridView1.Columns["FirstName"].HeaderText = "FirstName";
            dataGridView1.Columns["LastName"].HeaderText = "LastName";
            dataGridView1.Columns["DateOfBirth"].HeaderText = "DateOfBirth";
            dataGridView1.Columns["CustomerID"].HeaderText = "CustomerID";
            dataGridView1.Columns["Address"].HeaderText = "Address";
            dataGridView1.Columns["PhoneNumber"].HeaderText = "PhoneNumber";
            dataGridView1.Columns["EmailAddress"].HeaderText = "EmailAddress";
            dataGridView1.Columns["Username"].HeaderText = "Username";
            dataGridView1.Columns["Password"].HeaderText = "Password";
        }



        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (txtCustomerID.Text == "")
            {
                MessageBox.Show("Please select a customer to update.");
                return;
            }

            if (DataValid())
            {
                Customer customer = new Customer();
                customer.FirstName = txtFname.Text;
                customer.LastName = txtLname.Text;
                customer.DateOfBirth = dtpDOB.Value.ToString("yyyy-MM-dd");
                customer.CustomerID = txtCustomerID.Text; // Ensure NIC is set
                customer.Address = txtAddress.Text;
                customer.PhoneNumber = txtPhone.Text;
                customer.EmailAddress = txtEmail.Text;
                customer.Username = txtusername.Text;
                customer.Password = txtpassword.Text;

                customer.UpdateCustomer(customer);
                DatabindtoGridView();
                ClearFields();
            }
        }
        private void ClearFields()
        {
            txtFname.Text = "";
            txtLname.Text = "";
            dtpDOB.Value = DateTime.Now;
            txtCustomerID.Text = "";
            txtAddress.Text = "";
            txtPhone.Text = "";
            txtEmail.Text = "";
            txtusername.Text = "";
            txtpassword.Text = "";
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                // Populate the form fields with the selected row's data
                txtFname.Text = row.Cells["FirstName"].Value.ToString();
                txtLname.Text = row.Cells["LastName"].Value.ToString();

                if (row.Cells["DateOfBirth"].Value != DBNull.Value)
                {
                    dtpDOB.Value = Convert.ToDateTime(row.Cells["DateOfBirth"].Value);
                }
                else
                {
                    // Handle the case where DateOfBirth is NULL
                    dtpDOB.Value = DateTime.Today; // or set it to a default value
                }

                txtCustomerID.Text = row.Cells["CustomerID"].Value.ToString();
                txtAddress.Text = row.Cells["Address"].Value.ToString();
                txtPhone.Text = row.Cells["PhoneNumber"].Value.ToString();
                txtEmail.Text = row.Cells["EmailAddress"].Value.ToString();
                txtusername.Text = row.Cells["Username"].Value.ToString();
                txtpassword.Text = row.Cells["Password"].Value.ToString();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (txtCustomerID.Text == "")
            {
                MessageBox.Show("Please select a customer to delete.");
                return;
            }

            string CustomerID = txtCustomerID.Text;
            Customer customer = new Customer();
            customer.CustomerID = CustomerID;

            // Confirmation dialog before deleting
            var confirmResult = MessageBox.Show("Are you sure you want to delete this customer?",
                                                "Confirm Delete",
                                                MessageBoxButtons.YesNo);

            if (confirmResult == DialogResult.Yes)
            {
                customer.DeleteCustomer(customer);
                DatabindtoGridView();
                ClearCustomerFields();
            }
        }
        private void ClearCustomerFields()
        {
            txtFname.Text = "";
            txtLname.Text = "";
            dtpDOB.Value = DateTime.Now;
            txtCustomerID.Text = "";
            txtAddress.Text = "";
            txtPhone.Text = "";
            txtEmail.Text = "";
            txtusername.Text = "";
            txtpassword.Text = "";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (DataValid())
            {
                Customer customer = new Customer();

                customer.FirstName = txtFname.Text;
                customer.LastName = txtLname.Text;
                customer.DateOfBirth = dtpDOB.Value.ToString("yyyy-MM-dd");
                customer.CustomerID = txtCustomerID.Text;
                customer.Address = txtAddress.Text;
                customer.PhoneNumber = txtPhone.Text;
                customer.EmailAddress = txtEmail.Text;
                customer.Username = txtusername.Text;
                customer.Password = txtpassword.Text;

                customer.SaveCustomer(customer);
                DatabindtoGridView();
                ClearCustomerFields();
            }
        }
        private bool DataValid()
        {
            if (string.IsNullOrWhiteSpace(txtFname.Text))
            {
                MessageBox.Show("Please enter FirstName");
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtLname.Text))
            {
                MessageBox.Show("Please enter LastName");
                return false;
            }
            if (dtpDOB.Value.Date > DateTime.Now.Date)
            {
                MessageBox.Show("Please enter a valid DateOfBirth");
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtCustomerID.Text))
            {
                MessageBox.Show("Please enter CustomerID");
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                MessageBox.Show("Please enter PhoneNumber");
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Please enter EmailAddress");
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtusername.Text))
            {
                MessageBox.Show("Please enter Username");
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtpassword.Text))
            {
                MessageBox.Show("Please enter Password");
                return false;
            }
            return true;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Validate customer data
            if (DataValid()) // Assuming CustomerDataValid checks that all required fields are filled out
            {
                // Create a new Customer object
                Customer newCustomer = new Customer
                {
                    FirstName = txtFname.Text,
                    LastName = txtLname.Text,
                    DateOfBirth = dtpDOB.Value.ToString("yyyy-MM-dd"),
                    CustomerID = txtCustomerID.Text,
                    Address = txtAddress.Text,
                    PhoneNumber = txtPhone.Text,
                    EmailAddress = txtEmail.Text,
                    Username = txtusername.Text,
                    Password = txtpassword.Text
                };

                // Insert the new customer into the database
                newCustomer.AddCustomer(newCustomer);

                // Refresh the DataGridView to reflect the new data
                DatabindtoGridView();

                // Clear the form fields
                ClearCustomerFields();
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Hide();
            HomePage home = new HomePage();
            home.Show();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }

    class DatabaseManager
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

        internal static void AddOrder(Order newOrder)
        {
            throw new NotImplementedException();
        }

        internal static void DeleteOrder(int orderId)
        {
            throw new NotImplementedException();
        }

        internal static void SaveOrder(Order newOrder)
        {
            throw new NotImplementedException();
        }

        internal static void UpdateOrder(Order updatedOrder)
        {
            throw new NotImplementedException();
        }
    }
}

    class Customer
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DateOfBirth { get; set; }
        public string CustomerID { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

    public void SaveCustomer(Customer customer)
        {
            string sql = @"INSERT INTO Customer (FirstName, LastName, DateOfBirth, CustomerID, Address, PhoneNumber, EmailAddress, Username, Password) VALUES 
                     ('" + customer.FirstName + "', '" + customer.LastName + "', '" + customer.DateOfBirth + "', '" + customer.CustomerID + "', '" + customer.Address + "', '" + customer.PhoneNumber + "', '" + customer.EmailAddress + "', '" + customer.Username + "', '" + customer.Password + "')";

        DatabaseManager.ExecuteQuery(sql);
        }

        public void UpdateCustomer(Customer customer)
        {
            string sql = @"UPDATE Customer 
                       SET FirstName = '" + customer.FirstName + "', LastName = '" + customer.LastName + "', DateOfBirth = '" + customer.DateOfBirth + 
                       "', Address = '" + customer.Address + "', PhoneNumber = '" + customer.PhoneNumber + 
                       "', EmailAddress = '" + customer.EmailAddress + "', Username = '" + customer.Username + 
                       "', Password = '" + customer.Password + "' WHERE CustomerID = '" + customer.CustomerID + "'";
    

        DatabaseManager.ExecuteQuery(sql);
        }

        public void AddCustomer(Customer customer)
        {
             string sql = @"INSERT INTO Customer (FirstName, LastName, DateOfBirth, CustomerID, Address, PhoneNumber, EmailAddress, Username, Password) VALUES 
                     ('" + customer.FirstName + "', '" + customer.LastName + "', '" + customer.DateOfBirth + "', '" + customer.CustomerID + "', '" + customer.Address + "', '" + customer.PhoneNumber + "', '" + customer.EmailAddress + "', '" + customer.Username + "', '" + customer.Password + "')";

        DatabaseManager.ExecuteQuery(sql);
        }

    public void DeleteCustomer(string CustomerID)
        {
            string sql = "DELETE FROM Customer WHERE CustomerID = '" + CustomerID + "'";
            DatabaseManager.ExecuteQuery(sql);
        }

    internal void DeleteCustomer(Customer customer)
    {
        throw new NotImplementedException();
    }
}

internal static class DatabaseManager
{
    public static string ConnectionString = ConfigurationManager.ConnectionStrings["dbcon"].ToString();

    public static void ExecuteQuery(string sql)
    {
        // The implementation of ExecuteQuery
    }
}
