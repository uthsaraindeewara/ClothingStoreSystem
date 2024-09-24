using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using InstoreSystem.Model;
using MySql.Data.MySqlClient;
using System.Xml.Linq;
using System.Collections;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace InstoreSystem.Interface
{
    public partial class PayrollManager : Form
    {
        public PayrollManager()
        {
            InitializeComponent();
        }

        private void PayrollManager_Load(object sender, EventArgs e)
        {
            using (MySqlConnection con = Connector.getConnection())
            {
                string sql = @"WITH EmployeeAttendance AS (
                                    SELECT 
                                        e.employeeID,
                                        e.employeeName,
                                        e.salary,
                                        COUNT(a.attendanceID) AS attendanceCount
                                    FROM employee e
                                    LEFT JOIN attendance a ON e.employeeID = a.employee_id
                                    AND MONTH(a.date) = MONTH(CURRENT_DATE())
                                    AND YEAR(a.date) = YEAR(CURRENT_DATE())
                                    GROUP BY e.employeeID
                                ),
                                EmployeeLeave AS (
                                    SELECT 
                                        e.employeeID,
                                        COUNT(l.leaveID) AS leaveCount
                                    FROM employee e
                                    LEFT JOIN leavetbl l ON e.employeeID = l.employeeID
                                    AND MONTH(l.date) = MONTH(CURRENT_DATE())
                                    AND YEAR(l.date) = YEAR(CURRENT_DATE())
                                    GROUP BY e.employeeID
                                ),
                                EmployeesWithPayroll AS (
                                    SELECT employee_id
                                    FROM payroll
                                    WHERE MONTH(paydate) = MONTH(CURRENT_DATE())
                                    AND YEAR(paydate) = YEAR(CURRENT_DATE())
                                )
                                SELECT 
                                    ea.employeeID,
                                    ea.employeeName,
                                    ea.salary,
                                    IFNULL(ea.attendanceCount, 0) AS attendance,
                                    IFNULL(el.leaveCount, 0) AS leaves,
                                    24 - IFNULL(ea.attendanceCount, 0) - IFNULL(el.leaveCount, 0) AS absent,
                                    (24 - IFNULL(ea.attendanceCount, 0) - IFNULL(el.leaveCount, 0)) - IFNULL(el.leaveCount, 0) AS nopay,
                                    ((24 - IFNULL(ea.attendanceCount, 0) - IFNULL(el.leaveCount, 0)) - IFNULL(el.leaveCount, 0)) * 1000 AS deduction
                                FROM EmployeeAttendance ea
                                LEFT JOIN EmployeeLeave el ON ea.employeeID = el.employeeID
                                LEFT JOIN EmployeesWithPayroll ep ON ea.employeeID = ep.employee_id
                                WHERE ep.employee_id IS NULL";

                try
                {
                    con.Open();

                    using (MySqlCommand com = new MySqlCommand(sql, con))
                    {
                        MySqlDataReader dr = com.ExecuteReader();

                        while (dr.Read())
                        {
                            dataGridView1.Rows.Add(dr.GetInt32("employeeID"), dr.GetString("employeeName"), dr.GetDecimal("salary"), dr.GetInt32("attendance"), dr.GetInt32("absent"), dr.GetInt32("leaves"), dr.GetInt32("nopay"), dr.GetDecimal("deduction"));
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error");
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == dataGridView1.Columns["DeductionAdd"].Index)
            {
                using (PayrollInput deductionInput = new PayrollInput("Deduction"))
                {
                    if (deductionInput.ShowDialog() == DialogResult.OK)
                    {
                        Decimal deduction = Convert.ToDecimal(dataGridView1.Rows[e.RowIndex].Cells["Deductions"].Value);
                        deduction += deductionInput.value;
                        dataGridView1.Rows[e.RowIndex].Cells["Deductions"].Value = deduction;
                    }
                }
            }
            else if (e.RowIndex >= 0 && e.ColumnIndex == dataGridView1.Columns["BonusAdd"].Index)
            {
                using (PayrollInput bonusInput = new PayrollInput("BonusAdd"))
                {
                    if (bonusInput.ShowDialog() == DialogResult.OK)
                    {
                        Decimal bonus = Convert.ToDecimal(dataGridView1.Rows[e.RowIndex].Cells["Bonus"].Value);
                        bonus += bonusInput.value;
                        dataGridView1.Rows[e.RowIndex].Cells["Bonus"].Value = bonus;
                    }
                }
            }
            else if (e.RowIndex >= 0 && e.ColumnIndex == dataGridView1.Columns["GeneratePaySlip"].Index)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                Payroll pay = new Payroll(Convert.ToInt32(row.Cells["ID"].Value), DateTime.Today, Convert.ToDecimal(row.Cells["Bonus"].Value), Convert.ToDecimal(row.Cells["Deductions"].Value), Convert.ToDecimal(row.Cells["Salary"].Value));
                pay.addPayroll();

                ReportDocument report = new ReportDocument();
                report.Load(@"D:\ClothingStoreSystem\InstoreSystem\InstoreSystem\Reports\PaySlip.rpt");

                // Set up the connection to the ODBC data source
                ConnectionInfo connectionInfo = new ConnectionInfo
                {
                    ServerName = "MySQl", // Name of the ODBC Data Source
                    UserID = "root", // Your MySQL username
                    Password = "" // Your MySQL password
                };

                // Apply the connection settings to each table in the report
                foreach (Table table in report.Database.Tables)
                {
                    TableLogOnInfo logOnInfo = table.LogOnInfo;
                    logOnInfo.ConnectionInfo = connectionInfo;
                    table.ApplyLogOnInfo(logOnInfo);
                }

                report.SetParameterValue("employeeId", row.Cells["ID"].Value);
                report.SetParameterValue("noPay", Convert.ToDouble(row.Cells["NoPay"].Value) * 1000);
                report.SetParameterValue("month", DateTime.Today.Month.ToString());
                report.SetParameterValue("year", DateTime.Today.Year.ToString());

                Reports rpt = new Reports(report);
                rpt.ShowDialog();

                dataGridView1.Rows.Remove(row);
            }
        }

        private void dataGridView1_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if ((e.ColumnIndex == dataGridView1.Columns["DeductionAdd"].Index || e.ColumnIndex == dataGridView1.Columns["BonusAdd"].Index) && e.RowIndex >= 0 || e.ColumnIndex == dataGridView1.Columns["GeneratePaySlip"].Index)
            {
                dataGridView1.Cursor = Cursors.Hand;
            }
        }

        private void dataGridView1_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.Cursor = Cursors.Default;
        }
    }
}
