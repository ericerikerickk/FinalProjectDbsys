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
            int recentlyInsertedId;

            try
            {
                using (SqlConnection con = new SqlConnection("Data Source=(localdb)\\ProjectsV13;Initial Catalog=grading_system;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
                {
                    con.Open();

                    SqlCommand studentCmd= new SqlCommand("INSERT INTO student(name, semester) VALUES(@name, @semester); SELECT SCOPE_IDENTITY();", con);
                    studentCmd.Parameters.AddWithValue("@name", txtName.Text.Trim());
                    studentCmd.Parameters.AddWithValue("@semester", txtSemester.Text.Trim());

                    recentlyInsertedId = Convert.ToInt32(studentCmd.ExecuteScalar());
                }

                createMarks(recentlyInsertedId);
                createAttendance(recentlyInsertedId);
                
            }
            catch (Exception err)
            {
                MessageBox.Show(err + "We can't add this student to the database.");
            }

            this.Close();
        }

        private void createAttendance(int id)
        {
            try
            {
                using (SqlConnection con = new SqlConnection("Data Source=(localdb)\\ProjectsV13;Initial Catalog=grading_system;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
                {
                    con.Open();

                    SqlCommand attendanceCmd = new SqlCommand("INSERT INTO attendance(student_id) VALUES(@id)", con);
                    attendanceCmd.Parameters.AddWithValue("@id", id);
                    attendanceCmd.ExecuteNonQuery();
                }

                MessageBox.Show("Student added!");
            }
            catch (Exception err)
            {
                MessageBox.Show(err + "We can't add this student to the database.");
            }
        }

        private void createMarks(int id)
        {
            try
            {
                using (SqlConnection con = new SqlConnection("Data Source=(localdb)\\ProjectsV13;Initial Catalog=grading_system;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
                {
                    con.Open();

                    SqlCommand attendanceCmd = new SqlCommand("INSERT INTO marks(student_id) VALUES(@id)", con);
                    attendanceCmd.Parameters.AddWithValue("@id", id);
                    attendanceCmd.ExecuteNonQuery();
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err + "We can't add this student to the database.");
            }
        }
    }
}
