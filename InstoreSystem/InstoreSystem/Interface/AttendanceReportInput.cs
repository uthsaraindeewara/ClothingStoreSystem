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
using CrystalDecisions.Shared;
using InstoreSystem.Model;
using MySql.Data.MySqlClient;

namespace InstoreSystem.Interface
{
    public partial class AttendanceReportInput : Form
    {
        public AttendanceReportInput()
        {
            InitializeComponent();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            if (rbDaily.Checked)
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
                report.Load(@"D:\ClothingStoreSystem\InstoreSystem\InstoreSystem\Reports\DailyAttendance.rpt");

                // Set up the connection to the ODBC data source
                ConnectionInfo connectionInfo = new ConnectionInfo
                {
                    ServerName = "MySQl",
                    UserID = "root",
                    Password = ""
                };

                // Apply the connection settings to each table in the report
                foreach (Table table in report.Database.Tables)
                {
                    TableLogOnInfo logOnInfo = table.LogOnInfo;
                    logOnInfo.ConnectionInfo = connectionInfo;
                    table.ApplyLogOnInfo(logOnInfo);
                }

                report.SetParameterValue("date", dtpPeriod.Value.Date);
                report.SetParameterValue("storeId", storeId);

                Reports rpt = new Reports(report);
                rpt.ShowDialog();
            }
            else if (rbYearly.Checked)
            {
                ReportDocument report = new ReportDocument();
                report.Load(@"D:\ClothingStoreSystem\InstoreSystem\InstoreSystem\Reports\YearlyAttendance.rpt");

                // Set up the connection to the ODBC data source
                ConnectionInfo connectionInfo = new ConnectionInfo
                {
                    ServerName = "MySQl",
                    UserID = "root",
                    Password = ""
                };

                // Apply the connection settings to each table in the report
                foreach (Table table in report.Database.Tables)
                {
                    TableLogOnInfo logOnInfo = table.LogOnInfo;
                    logOnInfo.ConnectionInfo = connectionInfo;
                    table.ApplyLogOnInfo(logOnInfo);
                }

                report.SetParameterValue("year", dtpPeriod.Value.Year.ToString());

                Reports rpt = new Reports(report);
                rpt.ShowDialog();
            }
        }

        private void rbDaily_CheckedChanged(object sender, EventArgs e)
        {
            reportTypeChange();
        }

        private void rbYearly_CheckedChanged(object sender, EventArgs e)
        {
            reportTypeChange();
        }

        private void reportTypeChange()
        {
            if (rbDaily.Checked)
            {
                lblType.Text = "Date";
                cmbStore.Enabled = true;
                dtpPeriod.Format = DateTimePickerFormat.Short;
                dtpPeriod.CustomFormat = null;
                dtpPeriod.ShowUpDown = false;
            }
            else if (rbYearly.Checked)
            {
                lblType.Text = "Year";
                cmbStore.Enabled = false;
                dtpPeriod.Format = DateTimePickerFormat.Custom;
                dtpPeriod.CustomFormat = "yyyy";
                dtpPeriod.ShowUpDown = true;
            }
        }
    }
}
