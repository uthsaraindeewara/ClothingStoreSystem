using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InstoreSystem.Employees;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace InstoreSystem.Model
{
    internal class Manager : Employee
    {
        private int managerId;
        private string role;
        private string email;
        private string qualification;
        public Manager(string employeeName, string address, string contactNo, DateTime dOB, string gender, double salary, int storeId, string role, string email, string qualification)
            : base(employeeName, address, contactNo, dOB, gender, salary, storeId)
        {
            this.role = role;
            this.email = email;
            this.qualification = qualification;
        }

        public Manager(int managerId)
            : base(managerId)
        {
            string query = "SELECT managerID, role, email, qualification FROM manager WHERE managerID = @managerId";

            using (MySqlConnection connection = Connector.getConnection())
            {
                try
                {
                    connection.Open();
                    MySqlCommand com = new MySqlCommand(query, connection);
                    com.Parameters.AddWithValue("@managerId", managerId);
                    MySqlDataReader dr = com.ExecuteReader();

                    if (dr.Read())
                    {
                        this.managerId = dr.GetInt32("managerID");
                        this.role = dr.GetString("role");
                        this.email = dr.GetString("email");
                        this.qualification = dr.GetString("qualification");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error");
                }
            }
        }

        public void addManager(int admin_id)
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
                        managerId = dr.GetInt32("employeeID");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error");
                }
            }

            string query = "INSERT INTO manager (managerID, role, email, qualification) " +
                           "VALUES (@managerId, @role, @email, @qualification)";

            using (MySqlConnection connection = Connector.getConnection())
            {
                try
                {
                    connection.Open();
                    MySqlCommand com = new MySqlCommand(query, connection);

                    com.Parameters.AddWithValue("@managerId", managerId);
                    com.Parameters.AddWithValue("@role", role);
                    com.Parameters.AddWithValue("@email", email);
                    com.Parameters.AddWithValue("@qualification", qualification);

                    if (com.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("Manager added successfully", "Information");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error");
                }
            }
        }

        public void updateManager()
        {
            updateEmployee();

            string query = "UPDATE manager SET " +
                           "role = @role, " +
                           "email = @email, " +
                           "qualification = @qualification " +
                           "WHERE managerID = @managerId";

            using (MySqlConnection connection = Connector.getConnection())
            {
                try
                {
                    connection.Open();
                    MySqlCommand com = new MySqlCommand(query, connection);

                    com.Parameters.AddWithValue("@managerId", managerId);
                    com.Parameters.AddWithValue("@role", role);
                    com.Parameters.AddWithValue("@email", email);
                    com.Parameters.AddWithValue("@qualification", qualification);

                    if (com.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("Manager updated successfully", "Information");
                    }
                    else
                    {
                        MessageBox.Show("Updating manager was unsuccessful", "Error");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error");
                }
            }
        }

        public Dictionary<String, String> getManagerDetails()
        {
            Dictionary<String, String> managerDetails = new Dictionary<String, String>()
            {
                { "role", this.role },
                { "email", this.email },
                { "qualification", this.qualification },
            };

            return managerDetails;
        }

        public void setRole(String role)
        {
            this.role = role;
        }

        public void setEmail(String email)
        {
            this.email = email;
        }

        public void setQualification(String qualification)
        {
            this.qualification = qualification;
        }
    }
}
