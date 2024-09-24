using FinalTest.Model;
using InstoreSystem.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InstoreSystem.Interface
{
    public partial class LeaveRequest : Form
    {
        public LeaveRequest()
        {
            InitializeComponent();
            AutoGenerateRequestID();
        }

        // Auto-generate the next ComplainID and display it in the textbox
        private void AutoGenerateRequestID()
        {
            string query = "SELECT MAX(leaveRequestID) + 1 AS NextLeaveRequestID FROM leave_request";

            using (MySqlConnection connection = Connector.getConnection())
            {
                try
                {
                    connection.Open();
                    MySqlCommand com = new MySqlCommand(query, connection);
                    object result = com.ExecuteScalar();

                    if (result != DBNull.Value && result != null)
                    {
                        txtLeaveRequestID.Text = result.ToString();
                    }
                    else
                    {
                        txtLeaveRequestID.Text = "1";
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error generating Complain ID: {ex.Message}", "Error");
                }
            }
        }

        private void btnRequest_Click(object sender, EventArgs e)
        {
            // Get data from form inputs
            DateTime date = dtpDate.Value;
            TimeSpan time =dtpTime.Value.TimeOfDay; 
            int employeeId = Convert.ToInt32(txtEmployeeID.Text);
            string description = txtDescription.Text;

            // Validation 
            if (string.IsNullOrEmpty(txtEmployeeID.Text) || string.IsNullOrEmpty(description))
            {
                MessageBox.Show("Please fill all required fields!", "Validation Error");
                return;
            }

            // Check if the employeeID exists in the employee table
            if (!IsEmployeeIDValid(employeeId))
            {
                MessageBox.Show("Invalid Employee ID. Please enter a valid ID.", "Error");
                return;
            }

            string query = "INSERT INTO leave_request (date, time, employeeID, description) " +
                           "VALUES (@date, @time, @employeeId, @description)";

            using (MySqlConnection connection = Connector.getConnection())
            {
                try
                {
                    connection.Open();
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@date", date);
                    cmd.Parameters.AddWithValue("@time", time);
                    cmd.Parameters.AddWithValue("@employeeId", employeeId);
                    cmd.Parameters.AddWithValue("@description", description);

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("Leave request submitted successfully", "Success");
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Failed to submit leave request", "Error");
                    }

                    AutoGenerateRequestID();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error");
                }
            }
        }
        private bool IsEmployeeIDValid(int employeeID)
        {
            // Query the database to check if employeeID exists
            string query = "SELECT COUNT(*) FROM employee WHERE employeeID = @employeeID";

            using (MySqlConnection connection = Connector.getConnection())
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@employeeID", employeeID);

                connection.Open();

                // Use Convert.ToInt32 to handle null values safely
                int count = Convert.ToInt32(cmd.ExecuteScalar() ?? 0);

                connection.Close();

                return count > 0; // If count is greater than 0, the employeeID is valid
            }
        }

        private void btnCLear_Click(object sender, EventArgs e)
        {

            dtpDate.Value = DateTime.Today;
            dtpTime.Value = DateTime.Now;
            txtDescription.Clear();
            txtEmployeeID.Clear();
        }

        private void LeaveRequest_Load(object sender, EventArgs e)
        {

        }
    }
}
