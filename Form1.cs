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

namespace ABC_Car_Traders
{
    public partial class AdminLogin : Form
    {
        public AdminLogin()
        {
            InitializeComponent();
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection("Data Source=DESKTOP-G7QOK8K;Initial Catalog=cmblogin;Integrated Security=True;Encrypt=False");
            SqlCommand cmd = new SqlCommand("select * from login where username='" + txtUserName.Text + "' and password='" + txtPassword.Text + "'", con);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            string cmbItemValue = comboBox1.SelectedItem.ToString();
            if (dt.Rows.Count > 0 )
            {
                for ( int i = 0; i < dt.Rows.Count; ++i)
                {
                    if (dt.Rows[i]["usertype"].ToString() == cmbItemValue)
                    {
                        MessageBox.Show("You are successfully logged as " + dt.Rows[i][2]);
                        if (comboBox1.SelectedIndex==0)
                        {
                            HomePage home = new HomePage();
                            home.Show();
                            this.Hide();
                        }
                        else
                        {
                            CusHomePage ff = new CusHomePage();
                            ff.Show();
                            this.Hide();
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Incorrect username and password");
            }
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            int x = txtUserName.Text.IndexOf(txtUserName.Text);

            if (x == -1)
                txtUserName.Text = Convert.ToString(txtUserName.Text);
            txtUserName.Text.Remove(x);
            txtUserName.Clear();

            int y = txtPassword.Text.IndexOf(txtPassword.Text);

            if (y == -1)
                txtPassword.Text = Convert.ToString(txtPassword.Text);
            txtPassword.Text.Remove(y);
            txtPassword.Clear();
        }

        private void login_registerhere_Click(object sender, EventArgs e)
        {
            RegisterPage register = new RegisterPage();
            register.Show();
            this.Hide();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void AdminLogin_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtUserName_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
