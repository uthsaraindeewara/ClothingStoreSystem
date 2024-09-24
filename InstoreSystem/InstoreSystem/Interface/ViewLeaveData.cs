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
    public partial class ViewLeaveData : Form
    {
        public ViewLeaveData()
        {
            InitializeComponent();
            LoadLeaveDetails();
        }

        // Load leavetbl data into DataGridView
        private void LoadLeaveDetails()
        {
            string query = "SELECT leaveID, date, time, employeeID FROM leavetbl";

            using (MySqlConnection connection = Connector.getConnection())
            {
                try
                {
                    connection.Open();
                    MySqlDataAdapter da = new MySqlDataAdapter(query, connection);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dataGridView1.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error");
                }
            }
        }

        private void ViewLeaveData_Load(object sender, EventArgs e)
        {

        }
    }
}
