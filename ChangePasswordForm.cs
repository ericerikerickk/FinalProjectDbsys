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

namespace University_Grade_Calculator
{
    public partial class ChangePasswordForm : Form
    {
        private string username;
        public ChangePasswordForm(string username)
        {
            InitializeComponent();
            this.username = username;
        }
        SqlConnection con = new SqlConnection("Data Source=(localdb)\\ProjectsV13;Initial Catalog=grading_system;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (txtPassword.Text.ToString().Trim().ToLower() == txtConPass.Text.ToString().Trim().ToLower())
            {
                string UserName = username;
                string Password = Cryptography.Encrypt(txtPassword.Text.ToString());   // Passing the Password to Encrypt method and the method will return encrypted string and stored in Password variable.  
                con.Close();
                con.Open();
                SqlCommand insert = new SqlCommand("Update [teacher] set password='" + Password + "' where username= '" + UserName + "'", con);
                insert.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Record updated successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Hide();
                LoginForm login = new LoginForm();
                MessageBox.Show("You will be logged out!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                login.ShowDialog();
            }
            else
            {
                MessageBox.Show("Password and Confirm Password doesn't match!.. Please Check..", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);  //showing the error message if password and confirm password doesn't match  
            }
        }

        private void ChangePasswordForm_Load(object sender, EventArgs e)
        {

        }
    }
}
