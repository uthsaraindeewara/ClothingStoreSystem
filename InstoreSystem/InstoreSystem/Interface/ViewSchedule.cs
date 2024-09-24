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
    public partial class ViewSchedule : Form
    {
        public ViewSchedule()
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
        
        
        private void btnChangeSch_Click(object sender, EventArgs e)
        {
            AddSchedule schedule = new AddSchedule();
            schedule.ShowDialog();
        }

        private void ViewSchedule_Load(object sender, EventArgs e)
        {
            PopulateGridView();
        }

        public void disableChangeSch()
        {
            this.btnChangeSch.Enabled = false;
        }
    }
}
