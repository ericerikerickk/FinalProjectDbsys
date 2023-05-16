using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using University_Grade_Calculator;

namespace Student_Grading_System
{
    public partial class Dashboard : Form
    {
        private string username;

        public Dashboard()
        {
            InitializeComponent();

        }

        public Dashboard(string username)
        {
            InitializeComponent();
            this.username = username;
            header.Text = $"Welcome, {username}!";
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            LoginForm login = new LoginForm();
            this.Hide();
            login.Show();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            editUsername.Visible = true;
            dataGridView1.Visible = false;
            gridTitle.Text = "Edit profile";
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if ( !String.IsNullOrWhiteSpace(txtUsername.Text))
            {
                SqlConnection con = new SqlConnection("Data Source=(localdb)\\ProjectsV13;Initial Catalog=grading_system;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
                con.Open();
                SqlCommand insert = new SqlCommand("Update [teacher] set username='" + txtUsername.Text.Trim() + "' where username= '" + username + "'", con);
                insert.ExecuteNonQuery();
                con.Close();

                MessageBox.Show("Username updated successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                username = txtUsername.Text.Trim();
                header.Text = $"Welcome, {username}!";

                gridTitle.Text = "Student list";

                editUsername.Visible = false;
                dataGridView1.Visible = true;
            }

            else
            {
                MessageBox.Show("Invalid username. Please check!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void ChangePass_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ChangePasswordForm changePass = new ChangePasswordForm(username);
            changePass.ShowDialog();
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection con = new SqlConnection("Data Source=(localdb)\\ProjectsV13;Initial Catalog=grading_system;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
                {
                    // Retrieve only the available books
                    string command = "SELECT * FROM student";
                    con.Open();

                    SqlCommand cmd = new SqlCommand(command, con);
                    cmd.ExecuteNonQuery();

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable books = new DataTable();
                        adapter.Fill(books);
                        dataGridView1.DataSource = books;
                    }
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err + "We can't load data from our server.");
            }

        }
    }
}
