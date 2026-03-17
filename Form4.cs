using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;

namespace ABC_Car_Traders
{
    public partial class RegisterPage : Form
    {
        public RegisterPage()
        {
            InitializeComponent();
        }

        private void Label2_Click(object sender, EventArgs e)
        {
            AdminLogin login = new AdminLogin();
            login.Show();
            this.Hide();
        }

        private void Label2_Click_1(object sender, EventArgs e)
        {

        }

        private void Label12_Click(object sender, EventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            bool isAnyEmpty = false;
            foreach(Control control in this.Controls)
            {
                if(control is TextBox)
                {
                    if (string.IsNullOrEmpty(control.Text))
                    {
                        isAnyEmpty = true;
                        break;
                    }
                }
                else if (control is DateTimePicker)
                {
                    if(((DateTimePicker)control).Value == null)
                    {
                        isAnyEmpty = true;
                        break;
                    }
                }
                else if (control is ComboBox)
                {
                    if (((ComboBox)control).SelectedIndex == -1)
                    {
                        isAnyEmpty = true;  
                        break;
                    }
                }
            }
            if (isAnyEmpty)
            {
                MessageBox.Show("One or more fields are empty, Please fill before submitting.");
            }
            else
            {
                SqlConnection con = new SqlConnection("Data Source=DESKTOP-G7QOK8K;Initial Catalog=registrationForm;Integrated Security=True");
                con.Open();
                string insertQuery = "INSERT INTO register VALUES (@fname, @lname, @dob, @NIC, @gender, @address, @phone_number, @email, @username, @password)";
                SqlCommand cmd = new SqlCommand(insertQuery, con);
                cmd.Parameters.AddWithValue("@fname", txtFname.Text);
                cmd.Parameters.AddWithValue("@lname", txtLname.Text);
                cmd.Parameters.AddWithValue("@dob", dptDOB.Value);
                cmd.Parameters.AddWithValue("@CustomerID", txtCustomerID.Text);
                cmd.Parameters.AddWithValue("@gender", cmbGender.SelectedItem.ToString());
                cmd.Parameters.AddWithValue("@address", txtAddress.Text);
                cmd.Parameters.AddWithValue("@phone_number", txtPhone.Text);
                cmd.Parameters.AddWithValue("@email", txtEmail.Text);
                cmd.Parameters.AddWithValue("@username", txtusername.Text);
                cmd.Parameters.AddWithValue("@password", txtpassword.Text);
                int v = cmd.ExecuteNonQuery();
                MessageBox.Show("Register successfully", "info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
