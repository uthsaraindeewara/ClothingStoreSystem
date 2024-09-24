using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;
using InstoreSystem.Model;
using MySql.Data.MySqlClient;

namespace InstoreSystem.Interface
{
    public partial class ExpenseReportInput : Form
    {
        public ExpenseReportInput()
        {
            InitializeComponent();
        }

        private void ExpenseReportInput_Load(object sender, EventArgs e)
        {
            dtpMonth.Format = DateTimePickerFormat.Custom;
            dtpMonth.CustomFormat = "MMMM yyyy";
            dtpMonth.ShowUpDown = true;
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            int storeId = -1; // Initialize with an invalid value (in case the store is not found)

            string query = "SELECT storeID FROM store WHERE storeName = @storeName";

            using (MySqlConnection connection = Connector.getConnection())
            {
                try
                {
                    // Open the connection
                    connection.Open();

                    // Create a MySQL command
                    MySqlCommand command = new MySqlCommand(query, connection);

                    // Add parameter to the query
                    command.Parameters.AddWithValue("@storeName", cmbStore.Text);

                    // Execute the query and read the result
                    object result = command.ExecuteScalar();

                    // Check if the result is not null and assign the value to storeID
                    if (result != null)
                    {
                        storeId = Convert.ToInt32(result);
                    }
                    else
                    {
                        MessageBox.Show("Store not found.", "Info");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error");
                }
            }

            ReportDocument report = new ReportDocument();
            report.Load(@"D:\ClothingStoreSystem\InstoreSystem\InstoreSystem\Reports\MonthlyExpenses.rpt");

            report.SetParameterValue("month", dtpMonth.Value.Month.ToString());
            report.SetParameterValue("year", dtpMonth.Value.Year.ToString());
            report.SetParameterValue("storeId", storeId);

            Reports rpt = new Reports(report);
            rpt.ShowDialog();
        }
    }
}
