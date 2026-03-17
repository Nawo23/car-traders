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
    public partial class CarDetails : Form
    {
        private object dataGridView_SelectedRows;

        public CarDetails()
        {
            InitializeComponent();
            // Load the initial data
            getManufacturer();
            getModel();
            getColour();
            getFuelType();
            getStatus();
            DatabindtoGridView();
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

        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Validate data
            if (DataValid())
            {
                // Create a new Car object
                Car newCar = new Car
                {
                    ManuCode = cmbManu.SelectedValue.ToString(),
                    ModelCode = cmbModel.SelectedValue.ToString(),
                    Year = dateTimePicker1.Value.ToString("yyyy-MM-dd"),
                    Price = Convert.ToDecimal(txtPrice.Text),
                    ColourCode = cmbColour.SelectedValue.ToString(),
                    FuelTypeCode = cmbFuelType.SelectedValue.ToString(),
                    EngineCapacity = txtEngineCapacity.Text,
                    StatusCode = cmbStatus.SelectedValue.ToString()
                };

                // Insert the new car into the database
                newCar.AddCar(newCar);

                // Refresh the DataGridView to reflect the new data
                DatabindtoGridView();

                // Clear the form fields
                ClearFields();
            }
        }
        public void ClearFields()
        {
            txtCarID.Text = "";
            dateTimePicker1.Text = "";
            txtPrice.Text = "";
            txtEngineCapacity.Text = "";
            cmbManu.SelectedIndex = -1;
            cmbModel.SelectedIndex = -1;
            cmbColour.SelectedIndex = -1;
            cmbFuelType.SelectedIndex = -1;
            cmbStatus.SelectedIndex = -1;
        }

        private void Save_Click(object sender, EventArgs e)
        {
            if (DataValid())
            {
                Car car = new Car();

                car.ManuCode = cmbManu.Text;
                car.ModelCode = cmbModel.Text;
                car.Year = dateTimePicker1.Value.ToString("yyyy-MM-dd");
                car.Price = Convert.ToDecimal(txtPrice.Text);
                car.ColourCode = cmbColour.Text;
                car.FuelTypeCode = cmbFuelType.Text;
                car.EngineCapacity = txtEngineCapacity.Text;
                car.StatusCode = cmbStatus.Text;

                car.SaveCar(car);
                DatabindtoGridView();
                ClearFields();
            }
        }
        private bool DataValid()
        {
            if (cmbManu.SelectedIndex == -1)
            {
                MessageBox.Show("Please Select Manufacturer");
                return false;
            }
            if (cmbModel.SelectedIndex == -1)
            {
                MessageBox.Show("Please Select Model");
                return false;
            }
            if (txtPrice.Text == "")
            {
                MessageBox.Show("Please Enter Price");
                return false;
            }
            if (!Information.IsNumeric(txtPrice.Text))
            {
                MessageBox.Show("Please Enter numeric Price");
                return false;
            }
            return true;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (txtCarID.Text == "")
            {
                MessageBox.Show("Please select a car to update.");
                return;
            }

            if (DataValid())
            {
                Car car = new Car();
                car.Car_ID = Convert.ToInt32(txtCarID.Text); // Ensure CarID is set
                car.ManuCode = cmbManu.Text;
                car.ModelCode = cmbModel.Text;
                car.Year = dateTimePicker1.Value.ToString("yyyy-MM-dd");
                car.Price = Convert.ToDecimal(txtPrice.Text);
                car.ColourCode = cmbColour.Text;
                car.FuelTypeCode = cmbFuelType.Text;
                car.EngineCapacity = txtEngineCapacity.Text;
                car.StatusCode = cmbStatus.Text;

                car.UpdateCar(car);
                DatabindtoGridView();
                ClearFields();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (txtCarID.Text == "")
            {
                MessageBox.Show("Please select a car to delete.");
                return;
            }

            int Car_ID = Convert.ToInt32(txtCarID.Text);
            Car car = new Car();
            car.Car_ID = Car_ID;

            // Confirmation dialog before deleting
            var confirmResult = MessageBox.Show("Are you sure you want to delete this car?",
                                     "Confirm Delete",
                                     MessageBoxButtons.YesNo);

            if (confirmResult == DialogResult.Yes)
            {
                car.DeleteCar(car);
                DatabindtoGridView();
                ClearFields();
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
    class Data1
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
    class Car
    {
        public int Car_ID { get; set; }
        public string ManuCode { get; set; }
        public string ModelCode { get; set; }
        public string Year { get; set; }
        public decimal Price { get; set; }
        public string ColourCode { get; set; }
        public string FuelTypeCode { get; set; }
        public string EngineCapacity { get; set; }
        public string StatusCode { get; set; }

        public void SaveCar(Car car)
        {
            string sql = "INSERT INTO Car (ManuCode, ModelCode, Year, Price, ColourCode, FuelTypeCode, EngineCapacity, StatusCode) VALUES " +
                "('" + car.ManuCode + "', '" + car.ModelCode + "', " + car.Year + ", " + car.Price + ", '" + car.ColourCode + "', '" + car.FuelTypeCode + "', '" + car.EngineCapacity + "', '" + car.StatusCode + "')";
            DataHelper.ExecuteQuery(sql);
        }

        public void UpdateCar(Car car)
        {
            string sql = "UPDATE Car SET ManuCode = '" + car.ManuCode + "', ModelCode = '" + car.ModelCode + "', Year = " + car.Year + ", Price = " + car.Price +
                         ", ColourCode = '" + car.ColourCode + "', FuelTypeCode = '" + car.FuelTypeCode + "', EngineCapacity = '" + car.EngineCapacity + "', StatusCode = '" + car.StatusCode + "' WHERE Car_ID = " + car.Car_ID;
            DataHelper.ExecuteQuery(sql);
        }

        public void AddCar(Car car)
        {
            // SQL query to insert a new car record
            string sql = @" INSERT INTO Car (ManuCode, ModelCode, Year, Price, ColourCode, FuelTypeCode, EngineCapacity, StatusCode) VALUES
                 ('" + car.ManuCode + "', '" + car.ModelCode + "', " + car.Year + ", " + car.Price + ", '"
                     + car.ColourCode + "', '" + car.FuelTypeCode + "', " + car.EngineCapacity + ", '" + car.StatusCode + "')";

            // Execute the SQL query
            DataHelper.ExecuteQuery(sql);
        }

        internal void DeleteCar(Car car)
        {
            string sql = "DELETE FROM Car WHERE Car_ID = " + car.Car_ID;
            DataHelper.ExecuteQuery(sql);
        }
    }
}
