using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InstoreSystem.Model;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace InstoreSystem.Model
{
    internal class Leave
    {
        protected int leaveId;
        protected DateTime date;
        protected TimeSpan time;
        protected int employeeId;

        // Constructor called when a new leave record is created
        public Leave(int leaveId, DateTime date, TimeSpan time, int employeeId)
        {
            this.leaveId = leaveId;
            this.date = date;
            this.time = time;
            this.employeeId = employeeId;
        }

        // Constructor called when an existing leave record is retrieved
        public Leave(int leaveId)
        {
            string query = "SELECT leaveID, date, time, employeeID FROM leave WHERE leaveID = @leaveId";

            using (MySqlConnection connection = Connector.getConnection())
            {
                try
                {
                    connection.Open();
                    MySqlCommand com = new MySqlCommand(query, connection);
                    com.Parameters.AddWithValue("@leaveId", leaveId);
                    MySqlDataReader dr = com.ExecuteReader();

                    if (dr.Read())
                    {
                        this.leaveId = Convert.ToInt32(dr["leaveID"]);
                        this.date = Convert.ToDateTime(dr["date"]);
                        this.time = TimeSpan.Parse(dr["time"].ToString());
                        this.employeeId = Convert.ToInt32(dr["employeeID"]);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error");
                }
            }
        }

        // Add new leave record
        public void addLeave()
        {
            string query = "INSERT INTO leave (leaveID, date, time, employeeID) " +
                           "VALUES (@leaveId, @date, @time, @employeeId)";

            using (MySqlConnection connection = Connector.getConnection())
            {
                try
                {
                    connection.Open();
                    MySqlCommand com = new MySqlCommand(query, connection);

                    com.Parameters.AddWithValue("@leaveId", leaveId);
                    com.Parameters.AddWithValue("@date", date);
                    com.Parameters.AddWithValue("@time", time);
                    com.Parameters.AddWithValue("@employeeId", employeeId);

                    if (com.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("Leave record added successfully", "Information");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error");
                }
            }
        }

        // Update the leave record
        public void updateLeave()
        {
            string query = "UPDATE leave SET " +
                           "date = @date, " +
                           "time = @time, " +
                           "employeeID = @employeeId " +
                           "WHERE leaveID = @leaveId";

            using (MySqlConnection connection = Connector.getConnection())
            {
                try
                {
                    connection.Open();
                    MySqlCommand com = new MySqlCommand(query, connection);

                    com.Parameters.AddWithValue("@leaveId", leaveId);
                    com.Parameters.AddWithValue("@date", date);
                    com.Parameters.AddWithValue("@time", time);
                    com.Parameters.AddWithValue("@employeeId", employeeId);

                    if (com.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("Leave record updated successfully", "Information");
                    }
                    else
                    {
                        MessageBox.Show("Updating leave record unsuccessful", "Error");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error");
                }
            }
        }

        // Return details as a dictionary of strings
        public Dictionary<String, String> getLeaveDetails()
        {
            Dictionary<String, String> leaveDetails = new Dictionary<String, String>()
            {
                { "leaveId", this.leaveId.ToString() },
                { "date", this.date.ToString("yyyy-MM-dd") },
                { "time", this.time.ToString(@"hh\:mm\:ss") },
                { "employeeId", this.employeeId.ToString() }
            };

            return leaveDetails;
        }

        // Getter for Leave ID
        public int getLeaveId()
        {
            return leaveId;
        }

        // Setter for Leave ID
        public void setLeaveId(int leaveId)
        {
            this.leaveId = leaveId;
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

        // Getter for Time
        public TimeSpan getTime()
        {
            return time;
        }

        // Setter for Time
        public void setTime(TimeSpan time)
        {
            this.time = time;
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
    }
}
