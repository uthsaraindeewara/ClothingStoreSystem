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
    public partial class ViewLeave : Form
    {
        public ViewLeave()
        {
            InitializeComponent();
            LoadLeaveRequests();
        }

        // Load leave requests into DataGridView
        private void LoadLeaveRequests()
        {

            string query = "SELECT leaveRequestID, date, time, employeeID, description FROM leave_request";

            using (MySqlConnection connection = Connector.getConnection())
            {
                try
                {
                    connection.Open();
                    MySqlDataAdapter da = new MySqlDataAdapter(query, connection);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dataGridView1.DataSource = dt;

                    // Check if 'Accept' and 'Deny' columns already exist
                    if (!dataGridView1.Columns.Contains("Accept"))
                    {
                        // Add 'Accept' button column
                        DataGridViewButtonColumn btnAccept = new DataGridViewButtonColumn();
                        btnAccept.HeaderText = "Accept";
                        btnAccept.Text = "Accept";
                        btnAccept.UseColumnTextForButtonValue = true;
                        btnAccept.Name = "Accept";
                        dataGridView1.Columns.Add(btnAccept);
                    }

                    if (!dataGridView1.Columns.Contains("Deny"))
                    {
                        // Add 'Deny' button column
                        DataGridViewButtonColumn btnDeny = new DataGridViewButtonColumn();
                        btnDeny.HeaderText = "Deny";
                        btnDeny.Text = "Deny";
                        btnDeny.UseColumnTextForButtonValue = true;
                        btnDeny.Name = "Deny";
                        dataGridView1.Columns.Add(btnDeny);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error");
                }
            }
        }

        private void ViewLeave_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // If the button clicked is the 'Accept' or 'Deny' button
            if (e.ColumnIndex == dataGridView1.Columns["Accept"].Index)
            {
                // Accept leave request
                int leaveRequestId = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["leaveRequestID"].Value);
                AcceptLeaveRequest(leaveRequestId);
            }
            else if (e.ColumnIndex == dataGridView1.Columns["Deny"].Index)
            {
                // Deny leave request (simply remove from leave_request table)
                int leaveRequestId = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["leaveRequestID"].Value);
                DenyLeaveRequest(leaveRequestId);
            }
        }

        // Accept leave request and move data to leavetbl
        private void AcceptLeaveRequest(int leaveRequestId)
        {
            using (MySqlConnection connection = Connector.getConnection())
            {
                try
                {
                    connection.Open();

                    // Select leave request details
                    string selectQuery = "SELECT date, time, employeeID FROM leave_request WHERE leaveRequestID = @leaveRequestId";
                    MySqlCommand selectCmd = new MySqlCommand(selectQuery, connection);
                    selectCmd.Parameters.AddWithValue("@leaveRequestId", leaveRequestId);
                    MySqlDataReader reader = selectCmd.ExecuteReader();

                    if (reader.Read())
                    {
                        DateTime date = Convert.ToDateTime(reader["date"]);
                        TimeSpan time = TimeSpan.Parse(reader["time"].ToString());
                        int employeeId = Convert.ToInt32(reader["employeeID"]);
                        reader.Close();

                        // Insert into leavetbl
                        string insertQuery = "INSERT INTO leavetbl (date, time, employeeID) VALUES (@date, @time, @employeeId)";
                        MySqlCommand insertCmd = new MySqlCommand(insertQuery, connection);
                        insertCmd.Parameters.AddWithValue("@date", date);
                        insertCmd.Parameters.AddWithValue("@time", time);
                        insertCmd.Parameters.AddWithValue("@employeeId", employeeId);
                        insertCmd.ExecuteNonQuery();

                        // Delete from leave_request
                        string deleteQuery = "DELETE FROM leave_request WHERE leaveRequestID = @leaveRequestId";
                        MySqlCommand deleteCmd = new MySqlCommand(deleteQuery, connection);
                        deleteCmd.Parameters.AddWithValue("@leaveRequestId", leaveRequestId);
                        deleteCmd.ExecuteNonQuery();

                        MessageBox.Show("Leave request accepted", "Success");
                        LoadLeaveRequests(); // Reload DataGridView
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error");
                }
            }
        }
        // Deny leave request (just remove from leave_request)
        private void DenyLeaveRequest(int leaveRequestId)
        {
            using (MySqlConnection connection = Connector.getConnection())
            {
                try
                {
                    connection.Open();
                    string query = "DELETE FROM leave_request WHERE leaveRequestID = @leaveRequestId";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@leaveRequestId", leaveRequestId);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Leave request denied", "Information");

                    LoadLeaveRequests(); // Reload DataGridView
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error");
                }
            }
        }

        private void btnViewLeave_Click(object sender, EventArgs e)
        {
            ViewLeaveData leave=new ViewLeaveData();
            leave.Show();
            this.Hide();    
        }
    }
}
