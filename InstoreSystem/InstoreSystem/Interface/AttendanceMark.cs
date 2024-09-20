using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using InstoreSystem.Employees;
using InstoreSystem.Model;
using MySql.Data.MySqlClient;

namespace InstoreSystem.Interface
{
    public partial class AttendanceMark : Form
    {
        Timer clock = new Timer();

        public AttendanceMark()
        {
            InitializeComponent();

            clock.Interval = 1000;

            clock.Tick += (sender, e) =>
            {
                lblTime.Text = DateTime.Now.ToString("hh:mm:ss tt");

                TimeSpan lateTime = DateTime.Now.TimeOfDay - new TimeSpan(09, 0, 0);

                if (lateTime > TimeSpan.Zero && DateTime.Now.TimeOfDay < new TimeSpan(12, 0, 0))
                {
                    lblLate.Text = $"Late: {lateTime.ToString(@"hh\:mm")}";
                }
            };

            clock.Start();

            lblDate.Text = DateTime.Now.Date.ToString("yyyy-mm-dd");
        }

        private void Attendance_Load(object sender, EventArgs e)
        {

        }

        private void butnSubmit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtId.Text))
            {
                MessageBox.Show("Please enter the ID", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string sql = "SELECT signInTime FROM attendance WHERE employee_id = @employeeId AND date = @today";

            using (MySqlConnection connection = Connector.getConnection())
            {
                try
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand(sql, connection);
                    command.Parameters.AddWithValue("@employeeId", txtId.Text);
                    command.Parameters.AddWithValue("@today", DateTime.Today);

                    MySqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        MessageBox.Show("Record is there");
                        TimeSpan signIn = reader.GetTimeSpan("SignInTime");

                        if (DateTime.Now.TimeOfDay - signIn < TimeSpan.FromMinutes(1))
                        {
                            MessageBox.Show("Your are already signed in");
                            return;
                        }

                        reader.Close();

                        string sql1 = "UPDATE attendance SET signOutTime = @signOut WHERE employee_id = @employeeId AND date = @date";

                        MySqlCommand com = new MySqlCommand(sql1, connection);
                        com.Parameters.AddWithValue("@signOut", DateTime.Now.TimeOfDay);
                        com.Parameters.AddWithValue("@employeeId", Convert.ToInt32(txtId.Text));
                        com.Parameters.AddWithValue("@date", DateTime.Today);

                        int ret = com.ExecuteNonQuery();

                        if (ret > 0)
                        {
                            MessageBox.Show("Sign out record added successfully");
                        }
                    }
                    else
                    {
                        Attendance attend = new Attendance(Convert.ToInt32(txtId.Text), DateTime.Today, DateTime.Now.TimeOfDay);
                        attend.addAttendance();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
