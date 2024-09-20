using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using InstoreSystem.Employees;

namespace InstoreSystem.Model
{
    internal class Cashier : Employee
    {
        private int cashierId;
        private int cashRegisterId;
        private string experience;

        // Constructor called when a new Cashier object is created
        public Cashier(string employeeName, string address, string contactNo, DateTime dOB, string gender, double salary, int storeId, int cashRegisterId, string experience)
            : base(employeeName, address, contactNo, dOB, gender, salary, storeId)
        {
            this.cashRegisterId = cashRegisterId;
            this.experience = experience;
        }

        // Constructor called when an existing Cashier object is created from the database
        public Cashier(int cashierId)
            : base(cashierId)
        {
            string query = "SELECT cashierID, cashRegisterNo, experience FROM cashier WHERE cashierID = @cashierId";

            using (MySqlConnection connection = Connector.getConnection())
            {
                try
                {
                    connection.Open();
                    MySqlCommand com = new MySqlCommand(query, connection);
                    com.Parameters.AddWithValue("@cashierId", cashierId);
                    MySqlDataReader dr = com.ExecuteReader();

                    if (dr.Read())
                    {
                        this.cashierId = dr.GetInt32("cashierID");
                        this.cashRegisterId = dr.GetInt32("cashRegisterNo");
                        this.experience = dr.GetString("experience");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error");
                }
            }
        }

        // Add a new Cashier to the database
        public void addCashier(int admin_id)
        {
            addEmployee(admin_id);

            string sql = "SELECT MAX(employeeID) AS employeeID FROM employee";

            using (MySqlConnection connection = Connector.getConnection())
            {
                try
                {
                    connection.Open();
                    MySqlCommand com = new MySqlCommand(sql, connection);
                    MySqlDataReader dr = com.ExecuteReader();

                    if (dr.Read())
                    {
                        cashierId = dr.GetInt32("employeeID");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error");
                }
            }

            string query = "INSERT INTO cashier (cashierID, cashRegisterNo, experience) " +
                           "VALUES (@cashierId, @cashRegisterId, @experience)";

            using (MySqlConnection connection = Connector.getConnection())
            {
                try
                {
                    connection.Open();
                    MySqlCommand com = new MySqlCommand(query, connection);

                    com.Parameters.AddWithValue("@cashierId", cashierId);
                    com.Parameters.AddWithValue("@cashRegisterId", cashRegisterId);
                    com.Parameters.AddWithValue("@experience", experience);

                    if (com.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("Cashier added successfully", "Information");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error");
                }
            }
        }

        // Update an existing Cashier in the database
        public void updateCashier()
        {
            updateEmployee();

            string query = "UPDATE cashier SET " +
                           "cashRegisterNo = @cashRegisterId, " +
                           "experience = @experience " +
                           "WHERE cashierID = @cashierId";

            using (MySqlConnection connection = Connector.getConnection())
            {
                try
                {
                    connection.Open();
                    MySqlCommand com = new MySqlCommand(query, connection);

                    com.Parameters.AddWithValue("@cashierId", cashierId);
                    com.Parameters.AddWithValue("@cashRegisterId", cashRegisterId);
                    com.Parameters.AddWithValue("@experience", experience);

                    if (com.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("Cashier updated successfully", "Information");
                    }
                    else
                    {
                        MessageBox.Show("Updating cashier unsuccessful", "Error");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error");
                }
            }
        }

        public Dictionary<String, String> getCashierDetails()
        {
            Dictionary<String, String> cashierDetails = new Dictionary<String, String>()
            {
                { "cashRegisterId", this.cashRegisterId.ToString() },
                { "experience", this.experience },
            };

            return cashierDetails;
        }

        public void setCashRegisterId(int cashRegisterId)
        {
            this.cashRegisterId = cashRegisterId;
        }

        public void setExperience(string experience)
        {
            this.experience = experience;
        }
    }
}
