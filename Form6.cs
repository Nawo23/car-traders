using Microsoft.VisualBasic;
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
    public partial class CarParts : Form
    {
        public CarParts()
        {
            InitializeComponent();
            getManufacturer();
            getModel();
            DatabindtoGridView();
        }

        private void DatabindtoGridView()
        {
            string sql = @"
            SELECT 
                p.PartID, 
                p.PartName, 
                m.ManuName, 
                p.ManuCode, 
                mo.ModelName, 
                p.ModelCode, 
                p.Price, 
                p.StockQuantity 
            FROM 
                CarParts p
            INNER JOIN 
                Manufacturer m ON p.ManuCode = m.ManuCode
            INNER JOIN 
                Model mo ON p.ModelCode = mo.ModelCode
            ORDER BY 
                p.PartID";

            dataGridView1.DataSource = DataHelper.getData(sql).Tables[0];

            // Customize the column headers and format
            dataGridView1.Columns["PartID"].HeaderText = "PartID";
            dataGridView1.Columns["PartName"].HeaderText = "PartName";
            dataGridView1.Columns["Price"].DefaultCellStyle.Format = "C2"; // Format as currency
            dataGridView1.Columns["StockQuantity"].HeaderText = "StockQuantity";
        }

        private void getManufacturer()
        {
            string sql = "SELECT * FROM Manufacturer";
            cmbManu.DataSource = DataHelper.getData(sql).Tables[0];
            cmbManu.DisplayMember = "ManuName";
            cmbManu.ValueMember = "ManuCode";
            cmbManu.SelectedIndex = -1;  // Clear selection initially
        }

        // Method to populate the Model ComboBox
        private void getModel()
        {
            string sql = "SELECT * FROM Model";
            cmbModel.DataSource = DataHelper.getData(sql).Tables[0];
            cmbModel.DisplayMember = "ModelName";
            cmbModel.ValueMember = "ModelCode";
            cmbModel.SelectedIndex = -1;
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                // Populate the form fields with the selected row's data
                txtPartID.Text = row.Cells["PartID"].Value.ToString();
                txtPartName.Text = row.Cells["PartName"].Value.ToString();
                cmbManu.Text = row.Cells["ManuName"].Value.ToString();
                cmbModel.Text = row.Cells["ModelName"].Value.ToString();
                txtPrice.Text = row.Cells["Price"].Value.ToString();
                nudStockQuantity.Value = Convert.ToInt32(row.Cells["StockQuantity"].Value);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (txtPartID.Text == "")
            {
                MessageBox.Show("Please select a car part to delete.");
                return;
            }

            int PartID = Convert.ToInt32(txtPartID.Text);
            CarPart part = new CarPart();
            part.PartID = PartID;

            // Confirmation dialog before deleting
            var confirmResult = MessageBox.Show("Are you sure you want to delete this car part?",
                                     "Confirm Delete",
                                     MessageBoxButtons.YesNo);

            if (confirmResult == DialogResult.Yes)
            {
                part.DeletePart(part);
                DatabindtoGridView();
                ClearPartFields();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (txtPartID.Text == "")
            {
                MessageBox.Show("Please select a car part to update.");
                return;
            }

            if (DataValid()) // Assuming DataValid method checks the required fields
            {
                CarPart part = new CarPart();
                part.PartID = Convert.ToInt32(txtPartID.Text); // Ensure PartID is set
                part.PartName = txtPartName.Text;
                part.ManuCode = cmbManu.SelectedValue.ToString(); // Assuming ManuCode is the selected value
                part.Price = Convert.ToDecimal(txtPrice.Text);
                part.StockQuantity = Convert.ToInt32(nudStockQuantity.Text);
                part.ModelCode = cmbModel.SelectedValue.ToString(); // Assuming ModelCode is the selected value

                part.UpdatePart(part); // Update the car part in the database
                DatabindtoGridView();  // Refresh the DataGridView to reflect changes
                ClearPartFields();         // Clear the input fields
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Validate data
            if (DataValid()) // Assuming DataValid checks that all required fields are filled out
            {
                // Create a new CarPart object
                CarPart newPart = new CarPart
                {
                    PartName = txtPartName.Text,
                    ManuCode = cmbManu.SelectedValue.ToString(), // Assuming ManuCode is the selected value in the ComboBox
                    ModelCode = cmbModel.SelectedValue.ToString(), // Assuming ModelCode is the selected value in the ComboBox
                    Price = Convert.ToDecimal(txtPrice.Text),
                    StockQuantity = Convert.ToInt32(nudStockQuantity.Text)
                };

                // Insert the new car part into the database
                newPart.AddPart(newPart);

                // Refresh the DataGridView to reflect the new data
                DatabindtoGridView();

                // Clear the form fields
                ClearPartFields();
            }
        }

        public void ClearPartFields()
        {
            txtPartID.Text = "";
            txtPartName.Text = "";
            txtPrice.Text = "";
            nudStockQuantity.Text = "";
            cmbManu.SelectedIndex = -1;
            cmbModel.SelectedIndex = -1;
        }
        private bool DataValid()
        {
            if (string.IsNullOrWhiteSpace(txtPartName.Text))
            {
                MessageBox.Show("Please enter Part Name");
                return false;
            }
            if (cmbManu.SelectedIndex == -1)
            {
                MessageBox.Show("Please select Manufacturer");
                return false;
            }
            if (cmbModel.SelectedIndex == -1)
            {
                MessageBox.Show("Please select Model");
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtPrice.Text))
            {
                MessageBox.Show("Please enter Price");
                return false;
            }
            if (!Information.IsNumeric(txtPrice.Text))
            {
                MessageBox.Show("Please enter a numeric value for Price");
                return false;
            }
            if (string.IsNullOrWhiteSpace(nudStockQuantity.Text))
            {
                MessageBox.Show("Please enter Stock Quantity");
                return false;
            }
            if (!Information.IsNumeric(nudStockQuantity.Text))
            {
                MessageBox.Show("Please enter a numeric value for Stock Quantity");
                return false;
            }
            return true;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            // Get search criteria from user input (e.g., PartID or PartName)
            string partID = txtPartID.Text.Trim();
            string partName = txtPartName.Text.Trim();

            string sql = "SELECT p.PartID, p.PartName, m.ManuName, p.ManuCode, mo.ModelName, p.ModelCode, p.Price, p.StockQuantity " +
                         "FROM CarParts p " +
                         "INNER JOIN Manufacturer m ON p.ManuCode = m.ManuCode " +
                         "INNER JOIN Model mo ON p.ModelCode = mo.ModelCode " +
                         "WHERE 1=1";

            // Append the WHERE clause based on input
            if (!string.IsNullOrEmpty(partID))
            {
                sql += " AND p.PartID = " + partID;
            }

            if (!string.IsNullOrEmpty(partName))
            {
                sql += " AND p.PartName LIKE '%" + partName + "%'";
            }

            sql += " ORDER BY p.PartID";

            // Execute the query and bind the result to the DataGridView
            dataGridView1.DataSource = DataHelper.getData(sql).Tables[0];
        }

        private void btnUpdateStock_Click(object sender, EventArgs e)
        {
            // Get the PartID and new stock quantity from user input
            string partID = txtPartID.Text.Trim();
            string newStockQuantity = nudStockQuantity.Text.Trim();

            // Ensure both PartID and StockQuantity are provided
            if (!string.IsNullOrEmpty(partID) && !string.IsNullOrEmpty(newStockQuantity))
            {
                string sql = "UPDATE CarParts SET StockQuantity = " + newStockQuantity + " WHERE PartID = " + partID;

                // Execute the update query
                DataHelper.ExecuteQuery(sql);

                // Inform the user about the successful update
                MessageBox.Show("Stock quantity updated successfully!");

                // Optionally, refresh the DataGridView to reflect the changes
                DatabindtoGridView();
            }
            else
            {
                MessageBox.Show("Please provide both PartID and new Stock Quantity.");
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

    class DataHelper
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

    // Car class for handling Car objects
    class CarPart
    {
        public int PartID { get; set; }
        public string PartName { get; set; }
        public string ManuCode { get; set; }
        public string ModelCode { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }

        // Method to add a new car part
        public void AddPart(CarPart part)
        {
            string sql = @"INSERT INTO CarParts (PartName, ManuCode, ModelCode, Price, StockQuantity) VALUES 
                     ('" + part.PartName + "', '" + part.ManuCode + "', '" + part.ModelCode + "', " + part.Price + ", " + part.StockQuantity + ")";
            DataHelper.ExecuteQuery(sql);
        }

        // Method to update an existing car part
        public void UpdatePart(CarPart part)
        {
            string sql = @"UPDATE CarParts 
                       SET PartName = '" + part.PartName + "', ManuCode = '" + part.ManuCode + "', ModelCode = '" + part.ModelCode +
                           "', Price = " + part.Price + ", StockQuantity = " + part.StockQuantity +
                           " WHERE PartID = " + part.PartID;
            DataHelper.ExecuteQuery(sql);
        }

        // Method to delete a car part
        public void DeletePart(CarPart part)
        {
            string sql = "DELETE FROM CarParts WHERE PartID = " + part.PartID;
            DataHelper.ExecuteQuery(sql);
        }

        // Optional: Method to update stock quantity of a part
        public void UpdateStock(int partID, int newStockQuantity)
        {
            string sql = "UPDATE CarParts SET StockQuantity = " + newStockQuantity + " WHERE PartID = " + partID;
            DataHelper.ExecuteQuery(sql);
        }
    }
}

