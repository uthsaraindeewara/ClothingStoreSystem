using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InstoreSystem.Model;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace InstoreSystem.Model
{
    internal class Payroll
    {
        protected int payrollId;
        protected int employeeId;
        protected DateTime payDate;
        protected decimal bonus;
        protected decimal deduction;
        protected decimal basicSalary;

        // Constructor called when a new payroll record is created
        public Payroll(int payrollId, int employeeId, DateTime payDate, decimal bonus, decimal deduction, decimal basicSalary)
        {
            this.payrollId = payrollId;
            this.employeeId = employeeId;
            this.payDate = payDate;
            this.bonus = bonus;
            this.deduction = deduction;
            this.basicSalary = basicSalary;
        }

        // Constructor called when an existing payroll record is retrieved
        public Payroll(int payrollId)
        {
            string query = "SELECT payroll_ID, emp_id, payDate, bonus, deduction, basicSalary FROM payroll WHERE payroll_ID = @payrollId";

            using (MySqlConnection connection = Connector.getConnection())
            {
                try
                {
                    connection.Open();
                    MySqlCommand com = new MySqlCommand(query, connection);
                    com.Parameters.AddWithValue("@payrollId", payrollId);
                    MySqlDataReader dr = com.ExecuteReader();

                    if (dr.Read())
                    {
                        this.payrollId = Convert.ToInt32(dr["payroll_ID"]);
                        this.employeeId = Convert.ToInt32(dr["emp_id"]);
                        this.payDate = Convert.ToDateTime(dr["payDate"]);
                        this.bonus = Convert.ToDecimal(dr["bonus"]);
                        this.deduction = Convert.ToDecimal(dr["deduction"]);
                        this.basicSalary = Convert.ToDecimal(dr["basicSalary"]);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error");
                }
            }
        }

        // Add new payroll record
        public void addPayroll()
        {
            string query = "INSERT INTO payroll (payroll_ID, emp_id, payDate, bonus, deduction, basicSalary) " +
                           "VALUES (@payrollId, @employeeId, @payDate, @bonus, @deduction, @basicSalary)";

            using (MySqlConnection connection = Connector.getConnection())
            {
                try
                {
                    connection.Open();
                    MySqlCommand com = new MySqlCommand(query, connection);

                    com.Parameters.AddWithValue("@payrollId", payrollId);
                    com.Parameters.AddWithValue("@employeeId", employeeId);
                    com.Parameters.AddWithValue("@payDate", payDate);
                    com.Parameters.AddWithValue("@bonus", bonus);
                    com.Parameters.AddWithValue("@deduction", deduction);
                    com.Parameters.AddWithValue("@basicSalary", basicSalary);

                    if (com.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("Payroll added successfully", "Information");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error");
                }
            }
        }

        // Update the payroll record
        public void updatePayroll()
        {
            string query = "UPDATE payroll SET " +
                           "emp_id = @employeeId, " +
                           "payDate = @payDate, " +
                           "bonus = @bonus, " +
                           "deduction = @deduction, " +
                           "basicSalary = @basicSalary " +
                           "WHERE payroll_ID = @payrollId";

            using (MySqlConnection connection = Connector.getConnection())
            {
                try
                {
                    connection.Open();
                    MySqlCommand com = new MySqlCommand(query, connection);

                    com.Parameters.AddWithValue("@payrollId", payrollId);
                    com.Parameters.AddWithValue("@employeeId", employeeId);
                    com.Parameters.AddWithValue("@payDate", payDate);
                    com.Parameters.AddWithValue("@bonus", bonus);
                    com.Parameters.AddWithValue("@deduction", deduction);
                    com.Parameters.AddWithValue("@basicSalary", basicSalary);

                    if (com.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("Payroll updated successfully", "Information");
                    }
                    else
                    {
                        MessageBox.Show("Updating payroll unsuccessful", "Error");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error");
                }
            }
        }

        // Return details as a dictionary of strings
        public Dictionary<String, String> getPayrollDetails()
        {
            Dictionary<String, String> payrollDetails = new Dictionary<String, String>()
            {
                { "payrollId", this.payrollId.ToString() },
                { "employeeId", this.employeeId.ToString() },
                { "payDate", this.payDate.ToString("yyyy-MM-dd") },
                { "bonus", this.bonus.ToString("F2") },
                { "deduction", this.deduction.ToString("F2") },
                { "basicSalary", this.basicSalary.ToString("F2") }
            };

            return payrollDetails;
        }

        // Getter for Payroll ID
        public int getPayrollId()
        {
            return payrollId;
        }

        // Setter for Payroll ID
        public void setPayrollId(int payrollId)
        {
            this.payrollId = payrollId;
        }

        // Getter for Employee ID
        public int getEmployeeId()
        {
            return employeeId;
        }

        // Setter for Employee ID
        public void setEmployeeId(int employeeId)
        {
            this.employeeId = employeeId;
        }

        // Getter for Pay Date
        public DateTime getPayDate()
        {
            return payDate;
        }

        // Setter for Pay Date
        public void setPayDate(DateTime payDate)
        {
            this.payDate = payDate;
        }

        // Getter for Bonus
        public decimal getBonus()
        {
            return bonus;
        }

        // Setter for Bonus
        public void setBonus(decimal bonus)
        {
            this.bonus = bonus;
        }

        // Getter for Deduction
        public decimal getDeduction()
        {
            return deduction;
        }

        // Setter for Deduction
        public void setDeduction(decimal deduction)
        {
            this.deduction = deduction;
        }

        // Getter for Basic Salary
        public decimal getBasicSalary()
        {
            return basicSalary;
        }

        // Setter for Basic Salary
        public void setBasicSalary(decimal basicSalary)
        {
            this.basicSalary = basicSalary;
        }
    }
}

