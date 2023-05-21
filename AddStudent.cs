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

namespace Student_Grading_System
{
    public partial class AddStudent : Form
    {
        public AddStudent()
        {
            InitializeComponent();
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection con = new SqlConnection("Data Source=(localdb)\\ProjectsV13;Initial Catalog=grading_system;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand("insert into[student](name, semester)values('" + txtName.Text.Trim() + "', '" + txtSemester.Text.Trim() + "')", con);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Student added!");
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err + "We can't add this student to the database.");
            }

            this.Close();
        }

    }
}
