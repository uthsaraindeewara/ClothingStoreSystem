using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;
using FinalTest.Interface;
using InstoreSystem.Employees;
using InstoreSystem.Interface;
using InstoreSystem.Model;
using MySql.Data.MySqlClient;

namespace InstoreSystem
{
    public partial class EmployeesPanel : UserControl
    {
        public EmployeesPanel()
        {
            InitializeComponent();
        }

        private void EmployeesPanel_Load(object sender, EventArgs e)
        {
            cmbFilterFrom.SelectedItem = "All";
            txtSearch_TextChanged(this, null);
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();

            string id = "";
            string name = "";

            if (rbId.Checked)
            {
                id = txtSearch.Text;
            }
            else if (rbName.Checked)
            {
                name = txtSearch.Text;
            }

            using (MySqlConnection con = Connector.getConnection())
            {
                con.Open();

                string sql = "";

                if (cmbFilterFrom.Text == "All")
                {
                    sql = @"SELECT
                                e.employeeID,
                                e.employeeName,
                                CASE 
                                    WHEN a.attendanceID IS NOT NULL AND a.date = CURDATE() THEN 'Present'
                                    ELSE 'Absent'
                                END AS attendance,
                                e.contactNo,
                                s.storeName
                            FROM
                                employee e
                            LEFT JOIN
                                attendance a ON e.employeeID = a.employee_id AND a.date = CURDATE()
                            INNER JOIN
                                store s ON e.storeID = s.storeID
                            WHERE
                                CAST(e.employeeID AS VARCHAR(10)) LIKE CONCAT('%', @employeeId, '%') AND e.employeeName LIKE CONCAT('%', @employeeName, '%')";

                    using (MySqlCommand com = new MySqlCommand(sql, con))
                    {
                        com.Parameters.AddWithValue("@employeeId", id);
                        com.Parameters.AddWithValue("@employeeName", name);

                        using (MySqlDataReader dr = com.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                dataGridView1.Rows.Add(dr.GetInt32("employeeID"), dr.GetString("employeeName"), dr.GetString("attendance"), dr.GetString("contactNo"), dr.GetString("storeName"));
                            }
                        }
                    }
                }
                else
                {
                    sql = @"SELECT
                                e.employeeID,
                                e.employeeName,
                                CASE 
                                    WHEN a.attendanceID IS NOT NULL AND a.date = CURDATE() THEN 'Present'
                                    ELSE 'Absent'
                                END AS attendance,
                                e.contactNo,
                                s.storeName
                            FROM
                                employee e
                            LEFT JOIN
                                attendance a ON e.employeeID = a.employee_id AND a.date = CURDATE()
                            INNER JOIN
                                store s ON e.storeID = s.storeID
                            WHERE
                                CAST(e.employeeID AS VARCHAR(10)) LIKE CONCAT('%', @employeeId, '%') AND e.employeeName LIKE CONCAT('%', @employeeName, '%'), s.storeName == @storeName";

                    using (MySqlCommand com = new MySqlCommand(sql, con))
                    {
                        com.Parameters.AddWithValue("@productId", id);
                        com.Parameters.AddWithValue("@productName", name);
                        com.Parameters.AddWithValue("@storeName", cmbFilterFrom.Text);

                        using (MySqlDataReader dr = com.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                dataGridView1.Rows.Add(dr.GetInt32("employeeID"), dr.GetString("employeeName"), dr.GetString("attendance"), dr.GetString("contactNo"), dr.GetString("store"));
                            }
                        }
                    }
                }
            }
        }

        private void dataGridView1_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if ((e.ColumnIndex == dataGridView1.Columns["Update"].Index || e.ColumnIndex == dataGridView1.Columns["Delete"].Index) && e.RowIndex >= 0)
            {
                dataGridView1.Cursor = Cursors.Hand;
            }
        }

        private void dataGridView1_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
                dataGridView1.Cursor = Cursors.Default;
        }

        private void rbId_CheckedChanged(object sender, EventArgs e)
        {
            txtSearch.Text = "";
        }

        private void rbName_CheckedChanged(object sender, EventArgs e)
        {
            txtSearch.Text = "";
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == dataGridView1.Columns["Delete"].Index)
            {
                // Get the employee ID from the selected row
                int employeeId = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["ID"].Value);

                // Confirm deletion with the user
                DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete this employee and all related records?", "Confirm Deletion", MessageBoxButtons.YesNo);

                if (dialogResult == DialogResult.Yes)
                {
                    using (MySqlConnection connection = Connector.getConnection())
                    {
                        MySqlTransaction transaction = null;  // Declare the transaction outside the try block

                        try
                        {
                            connection.Open();

                            // Begin the transaction
                            transaction = connection.BeginTransaction();

                            // Delete related records from employee_schedule
                            string deleteEmployeeSchedule = "DELETE FROM employee_schedule WHERE employeeID = @employeeID";
                            using (MySqlCommand cmd = new MySqlCommand(deleteEmployeeSchedule, connection, transaction))
                            {
                                cmd.Parameters.AddWithValue("@employeeID", employeeId);
                                cmd.ExecuteNonQuery();
                            }

                            // Delete related records from employee_attendance
                            string deleteEmployeeAttendance = "DELETE FROM attendance WHERE employeeID = @employeeID";
                            using (MySqlCommand cmd = new MySqlCommand(deleteEmployeeAttendance, connection, transaction))
                            {
                                cmd.Parameters.AddWithValue("@employeeID", employeeId);
                                cmd.ExecuteNonQuery();
                            }

                            // Delete from attendance table (linked via employee_attendance)
                            string deleteAttendance = "DELETE a FROM attendance a INNER JOIN employee_attendance ea ON a.attendanceID = ea.attendanceID WHERE ea.employeeID = @employeeID";
                            using (MySqlCommand cmd = new MySqlCommand(deleteAttendance, connection, transaction))
                            {
                                cmd.Parameters.AddWithValue("@employeeID", employeeId);
                                cmd.ExecuteNonQuery();
                            }

                            // Delete from manager table (if applicable)
                            string deleteManager = "DELETE FROM manager WHERE managerID = @employeeID";
                            using (MySqlCommand cmd = new MySqlCommand(deleteManager, connection, transaction))
                            {
                                cmd.Parameters.AddWithValue("@employeeID", employeeId);
                                cmd.ExecuteNonQuery();
                            }

                            // Delete from accountant table (if applicable)
                            string deleteAccountant = "DELETE FROM accountant WHERE accountantID = @employeeID";
                            using (MySqlCommand cmd = new MySqlCommand(deleteAccountant, connection, transaction))
                            {
                                cmd.Parameters.AddWithValue("@employeeID", employeeId);
                                cmd.ExecuteNonQuery();
                            }

                            // Delete from salesassociate table (if applicable)
                            string deleteSalesAssociate = "DELETE FROM salesassociate WHERE salesAssocID = @employeeID";
                            using (MySqlCommand cmd = new MySqlCommand(deleteSalesAssociate, connection, transaction))
                            {
                                cmd.Parameters.AddWithValue("@employeeID", employeeId);
                                cmd.ExecuteNonQuery();
                            }

                            // Delete from cashier table (if applicable)
                            string deleteCashier = "DELETE FROM cashier WHERE cashierID = @employeeID";
                            using (MySqlCommand cmd = new MySqlCommand(deleteCashier, connection, transaction))
                            {
                                cmd.Parameters.AddWithValue("@employeeID", employeeId);
                                cmd.ExecuteNonQuery();
                            }

                            // Finally, delete the employee from the employee table
                            string deleteEmployee = "DELETE FROM employee WHERE employeeID = @employeeID";
                            using (MySqlCommand cmd = new MySqlCommand(deleteEmployee, connection, transaction))
                            {
                                cmd.Parameters.AddWithValue("@employeeID", employeeId);
                                cmd.ExecuteNonQuery();
                            }

                            // Commit the transaction
                            transaction.Commit();

                            MessageBox.Show("Employee and all related records deleted successfully!");
                        }
                        catch (Exception ex)
                        {
                            // Rollback the transaction in case of an error
                            if (transaction != null)
                            {
                                transaction.Rollback();
                            }
                            MessageBox.Show($"Error: {ex.Message}", "Error");
                        }
                    }

                    // Refresh the DataGridView after deletion
                    txtSearch_TextChanged(this, null);
                }
            }
            else if (e.RowIndex >= 0 && e.ColumnIndex == dataGridView1.Columns["Update"].Index)
            {
                int employeeId = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["ID"].Value);
                AddEmployees employee = new AddEmployees(employeeId);
                employee.ShowDialog();
                txtSearch_TextChanged(this, null);
            }
        }

        private void cmbFilterFrom_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtSearch.Text = "";
        }

        private void btnPayroll_Click(object sender, EventArgs e)
        {
            PayrollManager payroll = new PayrollManager();
            payroll.ShowDialog();
        }

        private void btnNewEmployee_Click(object sender, EventArgs e)
        {
            AddEmployees employees = new AddEmployees();
            employees.ShowDialog();
        }

        private void btnAttendance_Click(object sender, EventArgs e)
        {
            AttendanceReportInput attendanceReport = new AttendanceReportInput();
            attendanceReport.ShowDialog();
        }

        private void btnSchedules_Click(object sender, EventArgs e)
        {
            AddSchedule schedule = new AddSchedule();
            schedule.ShowDialog();
        }

        private void btnLeave_Click(object sender, EventArgs e)
        {
            ViewLeave leave = new ViewLeave();
            leave.ShowDialog();
        }
    }
}
