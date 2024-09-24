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
    public partial class AddEmployeesToSchedule : Form
    {
        public AddEmployeesToSchedule()
        {
            InitializeComponent();
        }

        private List<Schedule> GetSchedules()
        {
            List<Schedule> schedules = new List<Schedule>();
            string query = "SELECT scheduleID, date, startTime, endTime, storeID FROM schedule";

            using (MySqlConnection connection = Connector.getConnection())
            {
                try
                {
                    connection.Open();
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    MySqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        schedules.Add(new Schedule(
                            Convert.ToInt32(dr["scheduleID"]),
                            Convert.ToDateTime(dr["date"]),
                            TimeSpan.Parse(dr["startTime"].ToString()),
                            TimeSpan.Parse(dr["endTime"].ToString()),
                            Convert.ToInt32(dr["storeID"])
                        ));
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error");
                }
            }

            return schedules;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (cmbScheduleID.SelectedItem == null || cmbEmployeeID.SelectedItem == null)
            {
                MessageBox.Show("Please select a valid schedule and employee.", "Error");
                return;
            }

            int scheduleID = (int)cmbScheduleID.SelectedValue;
            int employeeID = (int)cmbEmployeeID.SelectedValue;

            Schedule schedule = new Schedule(scheduleID);
            schedule.AddEmployeeToSchedule(employeeID);
            MessageBox.Show("Employee added successfully.");

            PopulateGridView(); // Refresh grid
        }

        private void AddEmployeesToSchedule_Load(object sender, EventArgs e)
        {
            // Populate ComboBoxes
            List<Schedule> schedules = GetSchedules(); 
            cmbScheduleID.DataSource = schedules;
            cmbScheduleID.DisplayMember = "ScheduleID";
            cmbScheduleID.ValueMember = "ScheduleID";

            List<KeyValuePair<int, string>> employees = Schedule.GetEmployeeList();
            cmbEmployeeID.DataSource = employees;
            cmbEmployeeID.DisplayMember = "Value"; 
            cmbEmployeeID.ValueMember = "Key";     

            // Populate grid with schedules and employees
            PopulateGridView();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (cmbScheduleID.SelectedItem == null || cmbEmployeeID.SelectedItem == null)
            {
                MessageBox.Show("Please select a valid schedule and employee.", "Error");
                return;
            }

            int scheduleID = (int)cmbScheduleID.SelectedValue;
            int employeeID = (int)cmbEmployeeID.SelectedValue;

            Schedule schedule = new Schedule(scheduleID);
            schedule.RemoveEmployeeFromSchedule(employeeID);
            MessageBox.Show("Employee removed successfully.");

            PopulateGridView(); // Refresh grid
        }

        private void PopulateGridView()
        {
            var schedules = GetSchedules(); // Get all schedules
            dataGridView1.Rows.Clear();

            foreach (var schedule in schedules)
            {
                Schedule s = new Schedule(schedule.ScheduleID);
                List<int> employeeList = s.GetEmployeesForSchedule();
                string employeeNames = string.Join(", ", employeeList); 

                dataGridView1.Rows.Add(s.ScheduleID, s.Date, s.StartTime, s.EndTime, s.StoreID, employeeNames);
            }
        }
    }
}
