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

namespace University_Grade_Calculator
{
    public partial class HomePage : Form
    {
        private string id;
        private string name, semester;
        public HomePage(string id)
        {
            InitializeComponent();
            this.id = id;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if(button1.Text.Trim() == "Edit student")
            {
                tb_Name.Enabled = true;
                tb_semester.Enabled = true;
                button1.Text = "Save changes";
            }

            else
            {
                tb_studentID.Enabled = false;
                tb_Name.Enabled = false;
                tb_semester.Enabled = false;
                button1.Text = "Edit student";
                updateStudent();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            deleteStudent();
            this.Close();
           
        }

        private void HomePage_Load(object sender, EventArgs e)
        {
            retrieveStudent();
            retrieveAttendance();
            retrieveMarks();

        }

        private void BtnAttendance_Click(object sender, EventArgs e)
        {
            if (btnAttendance.Text.Trim().Equals("Update attendance"))
            {
                btnAttendance.Text = "Save changes";
                tb_attendance.Enabled = true;
                tb_Mid.Enabled = true;
                tb_Final.Enabled = true;
            }

            else
            {
                btnAttendance.Text = "Update attendance";
                tb_attendance.Enabled = false;
                tb_Mid.Enabled = false;
                tb_Final.Enabled = false;
                updateAttendance(tb_attendance.Text.Trim(), tb_Mid.Text.Trim(), tb_Final.Text.Trim());
            }
        }

        private void BtnMarks_Click(object sender, EventArgs e)
        {
            
        }

        private void deleteStudent()
        {
            try
            {
                using (SqlConnection con = new SqlConnection("Data Source=(localdb)\\ProjectsV13;Initial Catalog=grading_system;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
                {
                    con.Open();

                    SqlCommand studentCmd = new SqlCommand("DELETE FROM student WHERE id='" + Int32.Parse(tb_studentID.Text.Trim()) + "'", con);
                    SqlCommand attendanceCmd = new SqlCommand("DELETE FROM attendance WHERE student_id = @id",con);

                    attendanceCmd.Parameters.AddWithValue("@id", this.id);

                    attendanceCmd.ExecuteNonQuery();
                    studentCmd.ExecuteNonQuery();

                    MessageBox.Show("Student deleted!");
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err + "We can't edit this student details!");
            }

        }

        private void updateStudent()
        {
            try
            {
                using (SqlConnection con = new SqlConnection("Data Source=(localdb)\\ProjectsV13;Initial Catalog=grading_system;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand("UPDATE student SET name='" + tb_Name.Text.Trim() + "', semester='" + tb_semester.Text.Trim() + "' WHERE id='" + Int32.Parse(tb_studentID.Text.Trim()) + "'", con);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Student updated!");
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err + "We can't edit this student details!");
            }
        }

        private void retrieveStudent()
        {
            SqlConnection con = new SqlConnection("Data Source=(localdb)\\ProjectsV13;Initial Catalog=grading_system;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

            con.Open();

            using (SqlCommand command = new SqlCommand("SELECT * FROM student WHERE id = '" + this.id + "'", con))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        name = reader.GetString(1);
                        semester = reader.GetString(2);
                    }
                }
            }

            con.Close();

            tb_studentID.Text = id;
            tb_Name.Text = name;
            tb_semester.Text = semester;

            tb_studentID.Enabled = false;
            tb_Name.Enabled = false;
            tb_semester.Enabled = false;
        }

        private void retrieveAttendance()
        {
            string prelim ="", midterm = "", final = "";

            SqlConnection con = new SqlConnection("Data Source=(localdb)\\ProjectsV13;Initial Catalog=grading_system;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

            con.Open();

            using (SqlCommand command = new SqlCommand("SELECT * FROM attendance WHERE student_id = '" + this.id + "'", con))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        prelim = reader.GetInt32(2).ToString();
                        midterm = reader.GetInt32(3).ToString();
                        final = reader.GetInt32(4).ToString();
                    }
                }
            }

            con.Close();

            tb_attendance.Text = prelim;
            tb_Mid.Text = midterm;
            tb_Final.Text = final;

            tb_attendance.Enabled = false;
            tb_Mid.Enabled = false;
            tb_Final.Enabled = false;

        }

        private void retrieveMarks()
        {

        }

        private void updateAttendance(string prelim, string midterm, string finals)
        {
            try
            {
                using (SqlConnection con = new SqlConnection("Data Source=(localdb)\\ProjectsV13;Initial Catalog=grading_system;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand("UPDATE attendance SET prelim = @prelim, midterm = @midterm, final = @final", con);
                    cmd.Parameters.AddWithValue("@prelim", prelim);
                    cmd.Parameters.AddWithValue("@midterm", midterm);
                    cmd.Parameters.AddWithValue("@final", finals);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Attendance updated!");
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err + "We can't add this student to the database.");
            }
        }

        public void calculateGrade()
        {
            if (tb_attendance.Text == "" || tb_Final.Text == "" || tb_Mid.Text == "" || tb_quiz1.Text == "" || tb_quiz2.Text == "" || tb_quiz3.Text == "" || tb_quiz4.Text == "")
            {
                MessageBox.Show("Marking fields should not be left empty.");
            }
            else
            {
                int inatt, finalatt, mid, final, total, percentage, quizsum = 0;
                inatt = int.Parse(tb_attendance.Text);
                finalatt = (inatt * 30) / 28;
                lb_attendance.Text = finalatt.ToString() + "/30";
                lb_attendance.Visible = true;
                mid = int.Parse(tb_Mid.Text);
                lb_mid.Text = mid.ToString() + "/75";
                lb_mid.Visible = true;
                final = int.Parse(tb_Final.Text);
                lb_Final.Text = final.ToString() + "/150";
                lb_Final.Visible = true;

                int quiz1, quiz2, quiz3, quiz4;
                quiz1 = int.Parse(tb_quiz1.Text);
                quiz2 = int.Parse(tb_quiz2.Text);
                quiz3 = int.Parse(tb_quiz3.Text);
                quiz4 = int.Parse(tb_quiz4.Text);
                int[] array = { quiz1, quiz2, quiz3, quiz4 };

                for (int i = 0; i <= 2; i++)
                {
                    Array.Sort(array);
                    Array.Reverse(array);
                    quizsum += array[i];
                }
                lb_quiz.Text = quizsum.ToString() + "/45";
                lb_quiz.Visible = true;

                total = finalatt + mid + quizsum + final;
                lb_total.Text = total.ToString() + "/300";
                lb_total.Visible = true;
                percentage = (total * 100) / 300;

                if (percentage >= 80)
                    lb_Grade.Text = "A+";
                else if (percentage >= 75 && percentage <= 79)
                    lb_Grade.Text = "A";
                else if (percentage >= 70 && percentage <= 74)
                    lb_Grade.Text = "A-";
                else if (percentage >= 65 && percentage <= 69)
                    lb_Grade.Text = "B+";
                else if (percentage >= 64 && percentage <= 60)
                    lb_Grade.Text = "B";
                else if (percentage >= 55 && percentage <= 59)
                    lb_Grade.Text = "B-";
                else if (percentage >= 50 && percentage <= 54)
                    lb_Grade.Text = "C+";
                else if (percentage >= 45 && percentage <= 49)
                    lb_Grade.Text = "C";
                else if (percentage >= 40 && percentage <= 44)
                    lb_Grade.Text = "D";
                else
                    lb_Grade.Text = "F";

                lb_Grade.Visible = true;


                lb_displayresult.Text = tb_Name.Text + " with student id " + tb_studentID.Text + " obtained " + percentage.ToString() + "% marks in " + tb_semester.Text + " Semester.";
                lb_displayresult.Visible = true;
            }
        }
    }
}
