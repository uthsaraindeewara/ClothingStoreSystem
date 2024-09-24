using InstoreSystem.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FinalTest.Model
{
    public class Schedule
    {
        // Properties (with getters and setters)
        public int ScheduleID { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int StoreID { get; set; }

        // Constructor for creating a new schedule
        public Schedule(int scheduleID, DateTime date, TimeSpan startTime, TimeSpan endTime, int storeID)
        {
            this.ScheduleID = scheduleID;
            this.Date = date;
            this.StartTime = startTime;
            this.EndTime = endTime;
            this.StoreID = storeID;
        }

        // Constructor for retrieving an existing schedule by ID
        public Schedule(int scheduleID)
        {
            string query = "SELECT scheduleID, date, startTime, endTime, storeID FROM schedule WHERE scheduleID = @scheduleID";

            using (MySqlConnection connection = Connector.getConnection())
            {
                try
                {
                    connection.Open();
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@scheduleID", scheduleID);
                    MySqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        this.ScheduleID = Convert.ToInt32(dr["scheduleID"]);
                        this.Date = Convert.ToDateTime(dr["date"]);
                        this.StartTime = TimeSpan.Parse(dr["startTime"].ToString());
                        this.EndTime = TimeSpan.Parse(dr["endTime"].ToString());
                        this.StoreID = Convert.ToInt32(dr["storeID"]);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error");
                }
            }
        }

        // Add new schedule
        public void AddSchedule()
        {
            if (StartTime >= EndTime)
            {
                MessageBox.Show("Start time must be before end time.", "Validation Error");
                return;
            }

            string query = "INSERT INTO schedule (date, startTime, endTime, storeID) VALUES (@date, @startTime, @endTime, @storeID)";

            using (MySqlConnection connection = Connector.getConnection())
            {
                try
                {
                    connection.Open();
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@date", Date);
                    cmd.Parameters.AddWithValue("@startTime", StartTime);
                    cmd.Parameters.AddWithValue("@endTime", EndTime);
                    cmd.Parameters.AddWithValue("@storeID", StoreID);

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("Schedule added successfully", "Information");
                    }
                    else
                    {
                        MessageBox.Show("Unable to add schedule. Please check your input.", "Error");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error");
                }
            }
        }

        // Update schedule
        public void UpdateSchedule()
        {
            if (StartTime >= EndTime)
            {
                MessageBox.Show("Start time must be before end time.", "Validation Error");
                return;
            }

            string query = "UPDATE schedule SET date = @date, startTime = @startTime, endTime = @endTime, storeID = @storeID WHERE scheduleID = @scheduleID";

            using (MySqlConnection connection = Connector.getConnection())
            {
                try
                {
                    connection.Open();
                    MySqlCommand cmd = new MySqlCommand(query, connection);

                    cmd.Parameters.AddWithValue("@scheduleID", ScheduleID);
                    cmd.Parameters.AddWithValue("@date", Date);
                    cmd.Parameters.AddWithValue("@startTime", StartTime);
                    cmd.Parameters.AddWithValue("@endTime", EndTime);
                    cmd.Parameters.AddWithValue("@storeID", StoreID);

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("Schedule updated successfully", "Information");
                    }
                    else
                    {
                        MessageBox.Show("Updating schedule unsuccessful", "Error");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error");
                }
            }
        }

        // Add employee to schedule
        public void AddEmployeeToSchedule(int employeeID)
        {
            string query = "INSERT INTO employee_schedule (scheduleID, employeeID) VALUES (@scheduleID, @employeeID)";

            using (MySqlConnection connection = Connector.getConnection())
            {
                try
                {
                    connection.Open();
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@scheduleID", ScheduleID);
                    cmd.Parameters.AddWithValue("@employeeID", employeeID);

                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error");
                }
            }
        }

        // Remove employee from schedule
        public void RemoveEmployeeFromSchedule(int employeeID)
        {
            string query = "DELETE FROM employee_schedule WHERE scheduleID = @scheduleID AND employeeID = @employeeID";

            using (MySqlConnection connection = Connector.getConnection())
            {
                try
                {
                    connection.Open();
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@scheduleID", ScheduleID);
                    cmd.Parameters.AddWithValue("@employeeID", employeeID);

                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error");
                }
            }
        }

        // Get employees assigned to this schedule
        public List<int> GetEmployeesForSchedule()
        {
            List<int> employees = new List<int>();
            string query = "SELECT employeeID FROM employee_schedule WHERE scheduleID = @scheduleID";

            using (MySqlConnection connection = Connector.getConnection())
            {
                try
                {
                    connection.Open();
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@scheduleID", ScheduleID);
                    MySqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        employees.Add(Convert.ToInt32(dr["employeeID"]));
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error");
                }
            }

            return employees;
        }
        public static List<KeyValuePair<int, string>> GetEmployeeList()
        {
            List<KeyValuePair<int, string>> employees = new List<KeyValuePair<int, string>>();
            string query = "SELECT employeeID, CONCAT(employeeName, ' (ID:', employeeID, ')') AS employeeDisplayName FROM employee";

            using (MySqlConnection connection = Connector.getConnection())
            {
                try
                {
                    connection.Open();
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    MySqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        int employeeID = Convert.ToInt32(dr["employeeID"]);
                        string displayName = dr["employeeDisplayName"].ToString();
                        employees.Add(new KeyValuePair<int, string>(employeeID, displayName));
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error");
                }
            }

            return employees;
        }
    }
}