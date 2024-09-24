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

namespace FinalTest.Interface
{
    public partial class ViewComplains : Form
    {
        public ViewComplains()
        {
            InitializeComponent();
            LoadScheduleData();
        }

        private void LoadScheduleData()
        {
            string query = "SELECT complainID, date, description, employeeID FROM complain";
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
                    MessageBox.Show($"Error loading complain data: {ex.Message}", "Error");
                }
            }
        }

        private void ViewComplains_Load(object sender, EventArgs e)
        {

        }
    }
}
