using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.Windows.Forms;
using InstoreSystem.Model;
using MySql.Data.MySqlClient;

namespace InstoreSystem.Interface
{
    public partial class SalesReportInput : Form
    {
        public SalesReportInput()
        {
            InitializeComponent();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cmbStore.Text))
            {
                return;
            }

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

            if (rbDaily.Checked)
            {
                ReportDocument report = new ReportDocument();
                report.Load(@"D:\ClothingStoreSystem\InstoreSystem\InstoreSystem\Reports\DaylySales.rpt");

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

                report.SetParameterValue("date", dtpPeriod.Value.ToString());
                report.SetParameterValue("storeId", storeId);

                Reports rpt = new Reports(report);
                rpt.ShowDialog();
            }
            else if (rbMonthly.Checked)
            {
                ReportDocument report = new ReportDocument();
                report.Load(@"D:\ClothingStoreSystem\InstoreSystem\InstoreSystem\Reports\MonthlySales.rpt");

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

                report.SetParameterValue("month", dtpPeriod.Value.Month.ToString());
                report.SetParameterValue("year", dtpPeriod.Value.Year.ToString());
                report.SetParameterValue("storeId", storeId);

                Reports rpt = new Reports(report);
                rpt.ShowDialog();
            }
            else if (rbYearly.Checked)
            {
                ReportDocument report = new ReportDocument();
                report.Load(@"D:\ClothingStoreSystem\InstoreSystem\InstoreSystem\Reports\YearlySales.rpt");

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
                report.SetParameterValue("storeId", storeId);

                Reports rpt = new Reports(report);
                rpt.ShowDialog();
            }
        }

        private void rbDaily_CheckedChanged(object sender, EventArgs e)
        {
            reportTypeChange();
        }

        private void rbMonthly_CheckedChanged(object sender, EventArgs e)
        {
            reportTypeChange();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            reportTypeChange();
        }

        private void reportTypeChange()
        {
            if (rbDaily.Checked)
            {
                lblType.Text = "Date";
                dtpPeriod.Format = DateTimePickerFormat.Short;
                dtpPeriod.CustomFormat = null;
                dtpPeriod.ShowUpDown = false;
            }
            else if (rbMonthly.Checked)
            {
                lblType.Text = "Month";
                dtpPeriod.Format = DateTimePickerFormat.Custom;
                dtpPeriod.CustomFormat = "MMMM yyyy";
                dtpPeriod.ShowUpDown = true;
            }
            else if (rbYearly.Checked)
            {
                lblType.Text = "Year";
                dtpPeriod.Format = DateTimePickerFormat.Custom;
                dtpPeriod.CustomFormat = "yyyy";
                dtpPeriod.ShowUpDown = true;
            }
        }
    }
}
