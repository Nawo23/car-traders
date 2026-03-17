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
    public partial class SearchCarParts : Form
    {
        public SearchCarParts()
        {
            InitializeComponent();
            getManufacturer();
            getModel();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Hide();
            CusHomePage home = new CusHomePage();
            home.Show();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            // Get search criteria from user input
            string partName = txtPartName.Text.Trim();
            string manuCode = cmbManu.SelectedValue?.ToString();
            string modelCode = cmbModel.SelectedValue?.ToString();

            string sql = "SELECT p.PartID, p.PartName, m.ManuName, mo.ModelName, p.Price, p.StockQuantity " +
                         "FROM CarParts p " +
                         "INNER JOIN Manufacturer m ON p.ManuCode = m.ManuCode " +
                         "INNER JOIN Model mo ON p.ModelCode = mo.ModelCode " +
                         "WHERE 1=1";

            // Append the WHERE clause based on input
            if (!string.IsNullOrEmpty(partName))
            {
                sql += " AND p.PartName LIKE '%" + partName + "%'";
            }

            if (!string.IsNullOrEmpty(manuCode))
            {
                sql += " AND p.ManuCode = '" + manuCode + "'";
            }

            if (!string.IsNullOrEmpty(modelCode))
            {
                sql += " AND p.ModelCode = '" + modelCode + "'";
            }

            sql += " ORDER BY p.PartID";

            // Execute the query and bind the result to the DataGridView
            dataGridView1.DataSource = DataHelper.getData(sql).Tables[0];

            // Customize the column headers and format
            dataGridView1.Columns["PartID"].HeaderText = "Part ID";
            dataGridView1.Columns["PartName"].HeaderText = "Part Name";
            dataGridView1.Columns["Price"].DefaultCellStyle.Format = "C2"; // Format as currency
            dataGridView1.Columns["StockQuantity"].HeaderText = "Stock Quantity";
        }

        private void getManufacturer()
        {
            string sql = "SELECT * FROM Manufacturer";
            cmbManu.DataSource = DataHelper.getData(sql).Tables[0];
            cmbManu.DisplayMember = "ManuName";
            cmbManu.ValueMember = "ManuCode";
            cmbManu.SelectedIndex = -1; // Clear selection initially
        }

        private void getModel()
        {
            string sql = "SELECT * FROM Model";
            cmbModel.DataSource = DataHelper.getData(sql).Tables[0];
            cmbModel.DisplayMember = "ModelName";
            cmbModel.ValueMember = "ModelCode";
            cmbModel.SelectedIndex = -1; // Clear selection initially
        }
    }
}
