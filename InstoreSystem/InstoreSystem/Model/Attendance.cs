using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace InstoreSystem.Model
{
    internal class Attendance
    {
        private int attendanceId;
        private int employeeId;
        private DateTime date;
        private TimeSpan signInTime;

        // Constructor called when a new attendance record is created
        public Attendance(int employeeId, DateTime date, TimeSpan signInTime)
        {
            this.employeeId = employeeId;
            this.date = date;
            this.signInTime = signInTime;
        }

        // Constructor called when an existing attendance record is retrieved
        public Attendance(int attendanceId)
        {
            string query = "SELECT attendanceID, employee_id, date, signInTime FROM attendance WHERE attendanceID = @attendanceId";

            using (MySqlConnection connection = Connector.getConnection())
            {
                try
                {
                    connection.Open();
                    MySqlCommand com = new MySqlCommand(query, connection);
                    com.Parameters.AddWithValue("@attendanceId", attendanceId);
                    MySqlDataReader dr = com.ExecuteReader();

                    if (dr.Read())
                    {
                        this.attendanceId = Convert.ToInt32(dr["attendanceID"]);
                        this.employeeId = Convert.ToInt32(dr["employeeID"]);
                        this.date = Convert.ToDateTime(dr["date"]);
                        this.signInTime = TimeSpan.Parse(dr["signInTime"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error");
                }
            }
        }

        // Add new attendance record
        public void addAttendance()
        {
            string query = "INSERT INTO attendance (employee_id, date, signInTime) " +
                           "VALUES (@employeeId, @date, @signInTime)";

            using (MySqlConnection connection = Connector.getConnection())
            {
                try
                {
                    connection.Open();
                    MySqlCommand com = new MySqlCommand(query, connection);

                    com.Parameters.AddWithValue("@employeeId", employeeId);
                    com.Parameters.AddWithValue("@date", date);
                    com.Parameters.AddWithValue("@signInTime", signInTime);

                    if (com.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("Attendance record added successfully", "Information");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error");
                }
            }
        }

        // Return details as a dictionary of strings
        public Dictionary<String, String> getAttendanceDetails()
        {
            Dictionary<String, String> attendanceDetails = new Dictionary<String, String>()
        {
            { "attendanceId", this.attendanceId.ToString() },
            { "employeeId", this.employeeId.ToString() },
            { "date", this.date.ToString("yyyy-MM-dd") },
            { "signInTime", this.signInTime.ToString(@"hh\:mm\:ss") },
        };

            return attendanceDetails;
        }

        // Getter for Attendance ID
        public int getAttendanceId()
        {
            return attendanceId;
        }

        // Setter for Attendance ID
        public void setAttendanceId(int attendanceId)
        {
            this.attendanceId = attendanceId;
        }

        // Getter for Employee ID
        public int getEmployeeId()
        {
            return employeeId;
        }

        // Setter for Employee ID
        public void setEmployeeId(int employeeId)
        {
            this.employeeId = employeeId;
        }

        // Getter for Date
        public DateTime getDate()
        {
            return date;
        }

        // Setter for Date
        public void setDate(DateTime date)
        {
            this.date = date;
        }

        // Getter for SignInTime
        public TimeSpan getSignInTime()
        {
            return signInTime;
        }

        // Setter for SignInTime
        public void setSignInTime(TimeSpan signInTime)
        {
            this.signInTime = signInTime;
        }
    }
}
