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

namespace ABC_Car_Traders
{
    public partial class SearchCarDetails : Form
    {
        public SearchCarDetails()
        {
            InitializeComponent();
            // Load the initial data
            getManufacturer();
            getModel();
            getColour();
            getFuelType();
            getStatus();
        }

        private void DatabindtoGridView()
        {
            string sql = @"
            SELECT 
                c.Car_ID, 
                m.ManuName, 
                c.ManuCode, 
                mo.ModelName, 
                c.ModelCode, 
                c.Year, 
                c.Price, 
                col.ColourName, 
                c.ColourCode, 
                f.FuelTypeName, 
                c.FuelTypeCode, 
                c.EngineCapacity, 
                s.StatusName, 
                c.StatusCode 
            FROM 
                Car c
            INNER JOIN 
                Manufacturer m ON c.ManuCode = m.ManuCode
            INNER JOIN 
                Model mo ON c.ModelCode = mo.ModelCode
            INNER JOIN 
                Colour col ON c.ColourCode = col.ColourCode
            INNER JOIN 
                FuelType f ON c.FuelTypeCode = f.FuelTypeCode
            INNER JOIN 
                Status s ON c.StatusCode = s.StatusCode
            ORDER BY 
                c.Car_ID";
            dataGridView1.DataSource = DataHelper.getData(sql).Tables[0];
            dataGridView1.Columns["Car_ID"].HeaderText = "Car_ID";
            dataGridView1.Columns["Year"].DefaultCellStyle.Format = "yyyy-MM-dd";
            dataGridView1.Columns["Price"].DefaultCellStyle.Format = "C2"; // Format as currency
            dataGridView1.Columns["EngineCapacity"].HeaderText = "EngineCapacity (L)";
        }


        // Method to populate the Make ComboBox
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

        // Method to populate the Colour ComboBox
        private void getColour()
        {
            string sql = "SELECT * FROM Colour";
            cmbColour.DataSource = DataHelper.getData(sql).Tables[0];
            cmbColour.DisplayMember = "ColourName";
            cmbColour.ValueMember = "ColourCode";
            cmbColour.SelectedIndex = -1;
        }

        // Method to populate the Fuel Type ComboBox
        private void getFuelType()
        {
            string sql = "SELECT * FROM FuelType";
            cmbFuelType.DataSource = DataHelper.getData(sql).Tables[0];
            cmbFuelType.DisplayMember = "FuelTypeName";
            cmbFuelType.ValueMember = "FuelTypeCode";
            cmbFuelType.SelectedIndex = -1;
        }

        // Method to populate the Status ComboBox
        private void getStatus()
        {
            string sql = "SELECT * FROM Status";
            cmbStatus.DataSource = DataHelper.getData(sql).Tables[0];
            cmbStatus.DisplayMember = "StatusName";
            cmbStatus.ValueMember = "StatusCode";
            cmbStatus.SelectedIndex = -1;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string query = "SELECT c.Car_ID, m.ManuName, mo.ModelName, c.Year, c.Price, col.ColourName, f.FuelTypeName, c.EngineCapacity, s.StatusName FROM Car c " +
                       "INNER JOIN Manufacturer m ON c.ManuCode = m.ManuCode " +
                       "INNER JOIN Model mo ON c.ModelCode = mo.ModelCode " +
                       "INNER JOIN Colour col ON c.ColourCode = col.ColourCode " +
                       "INNER JOIN FuelType f ON c.FuelTypeCode = f.FuelTypeCode " +
                       "INNER JOIN Status s ON c.StatusCode = s.StatusCode WHERE 1=1 ";

            if (cmbManu.SelectedIndex != -1)
            {
                query += "AND c.ManuCode = '" + cmbManu.SelectedValue + "' ";
            }
            if (cmbModel.SelectedIndex != -1)
            {
                query += "AND c.ModelCode = '" + cmbModel.SelectedValue + "' ";
            }
            if (cmbColour.SelectedIndex != -1)
            {
                query += "AND c.ColourCode = '" + cmbColour.SelectedValue + "' ";
            }
            if (cmbFuelType.SelectedIndex != -1)
            {
                query += "AND c.FuelTypeCode = '" + cmbFuelType.SelectedValue + "' ";
            }
            if (cmbStatus.SelectedIndex != -1)
            {
                query += "AND c.StatusCode = '" + cmbStatus.SelectedValue + "' ";
            }

            // Bind the result to the DataGridView
            dataGridView1.DataSource = DataHelper.getData(query).Tables[0];
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

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                // Populate the form fields with the selected row's data
                txtCarID.Text = row.Cells["Car_ID"].Value.ToString();
                cmbManu.Text = row.Cells["ManuName"].Value.ToString();
                cmbModel.Text = row.Cells["ModelName"].Value.ToString();
                dateTimePicker1.Text = row.Cells["Year"].Value.ToString();
                txtPrice.Text = row.Cells["Price"].Value.ToString();
                cmbColour.Text = row.Cells["ColourName"].Value.ToString();
                cmbFuelType.Text = row.Cells["FuelTypeName"].Value.ToString();
                txtEngineCapacity.Text = row.Cells["EngineCapacity"].Value.ToString();
                cmbStatus.Text = row.Cells["StatusName"].Value.ToString();
            }
        }
    }
}
