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
    public partial class AddSchedule : Form
    {
        private Schedule selectedSchedule;

        public AddSchedule()
        {
            InitializeComponent();
            LoadScheduleData();
            AutoGenerateScheduleID();
        }
        private void AutoGenerateScheduleID()
        {
            string query = "SELECT MAX(scheduleID) + 1 AS NextScheduleID FROM schedule";

            using (MySqlConnection connection = Connector.getConnection())
            {
                try
                {
                    connection.Open();
                    MySqlCommand com = new MySqlCommand(query, connection);
                    object result = com.ExecuteScalar();

                    if (result != DBNull.Value && result != null)
                    {
                        txtScheduleID.Text = result.ToString();
                    }
                    else
                    {
                        txtScheduleID.Text = "1";
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error generating Complain ID: {ex.Message}", "Error");
                }
            }
        }
       /* private void LoadEmployees()
        {
            List<KeyValuePair<int, string>> employees = Schedule.GetEmployeeList();
            cmbEmployee.DataSource = new BindingSource(employees, null);
            cmbEmployee.DisplayMember = "Value";
            cmbEmployee.ValueMember = "Key";
        }*/

        private void LoadScheduleData()
        {
            string query = "SELECT scheduleID, date, startTime, endTime, storeID FROM schedule";
            using (MySqlConnection connection = Connector.getConnection())
            {
                try
                {
                    connection.Open();
                    MySqlCommand com = new MySqlCommand(query, connection);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(com);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading schedule data: {ex.Message}", "Error");
                }
            }
        }


        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                try
                {

                    DateTime date = dtpDate.Value;
                    TimeSpan startTime = dtpStartTime.Value.TimeOfDay;
                    TimeSpan endTime = dtpEndTime.Value.TimeOfDay;
                    int storeID = Convert.ToInt32(cmbStoreID.Text);

                    Schedule newSchedule = new Schedule(0, date, startTime, endTime, storeID);
                    newSchedule.AddSchedule();
                    LoadScheduleData();
                    ClearFields();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error");
                }
            }
        }

        

        private void ClearFields()
        {
            dtpDate.Value = DateTime.Today;
            dtpStartTime.Value = DateTime.Now;
            dtpEndTime.Value = DateTime.Now.AddHours(1); 
            txtScheduleID.Clear();
            
            selectedSchedule = null;

            
        }

        private bool ValidateForm()
        {
           
           
            if (dtpStartTime.Value.TimeOfDay >= dtpEndTime.Value.TimeOfDay)
            {
                MessageBox.Show("End time must be later than start time.", "Validation Error");
                return false;
            }

            return true;
        }

        private void btnCLear_Click_1(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void LoadStoreIDs()
        {
            string query = "SELECT storeID FROM store";

            using (MySqlConnection connection = Connector.getConnection())
            {
                try
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand(query, connection);
                    MySqlDataReader reader = command.ExecuteReader();

                    // Clear existing items in the combo box
                    cmbStoreID.Items.Clear();

                    // Add each storeID to the combo box
                    while (reader.Read())
                    {
                        cmbStoreID.Items.Add(reader["storeID"].ToString());
                    }

                    // Optionally, set the combo box to show the first item by default
                    if (cmbStoreID.Items.Count > 0)
                    {
                        cmbStoreID.SelectedIndex = 0;  // Select the first store ID
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading store IDs: {ex.Message}", "Error");
                }
            }
        }

        private void AddSchedule_Load(object sender, EventArgs e)
        {
            LoadStoreIDs();
        }

        private void dataGridView1_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            // Check if the clicked row index is valid
            if (e.RowIndex >= 0)
            {
                // Get the selected row
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                // Populate the fields with data from the selected row
                int scheduleID = Convert.ToInt32(row.Cells["scheduleID"].Value);
                selectedSchedule = new Schedule(scheduleID);  // Load the schedule from the database

                // Populate form fields
                txtScheduleID.Text = scheduleID.ToString();
                dtpDate.Value = selectedSchedule.Date;
                dtpStartTime.Value = DateTime.Today.Add(selectedSchedule.StartTime);
                dtpEndTime.Value = DateTime.Today.Add(selectedSchedule.EndTime);
                cmbStoreID.Text = selectedSchedule.StoreID.ToString();
                

               
                
               
            }

        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            AddEmployeesToSchedule schedule = new AddEmployeesToSchedule();
            schedule.ShowDialog();
        }
    }
}
