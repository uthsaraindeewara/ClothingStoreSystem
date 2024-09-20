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
    internal class Accountant : Employee
    {
        private int accountantId;
        private string email;
        private string qualification;

        // Constructor for creating a new accountant
        public Accountant(string employeeName, string address, string contactNo, DateTime dOB, string gender, double salary, int storeId, string email, string qualification)
            : base(employeeName, address, contactNo, dOB, gender, salary, storeId)
        {
            this.email = email;
            this.qualification = qualification;
        }

        // Constructor for fetching an existing accountant from the database
        public Accountant(int accountantId)
            : base(accountantId)
        {
            string query = "SELECT accountantID, email, qualification FROM accountant WHERE accountantID = @accountantId";

            using (MySqlConnection connection = Connector.getConnection())
            {
                try
                {
                    connection.Open();
                    MySqlCommand com = new MySqlCommand(query, connection);
                    com.Parameters.AddWithValue("@accountantId", accountantId);
                    MySqlDataReader dr = com.ExecuteReader();

                    if (dr.Read())
                    {
                        this.accountantId = dr.GetInt32("accountantID");
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

        // Method to add a new accountant to the database
        public void addAccountant(int admin_id)
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
                        accountantId = dr.GetInt32("employeeID");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error");
                }
            }

            string query = "INSERT INTO accountant (accountantID, email, qualification) " +
                           "VALUES (@accountantId, @Email, @Qualification)";

            using (MySqlConnection connection = Connector.getConnection())
            {
                try
                {
                    connection.Open();
                    MySqlCommand com = new MySqlCommand(query, connection);
                    com.Parameters.AddWithValue("@accountantId", accountantId);
                    com.Parameters.AddWithValue("@Email", email);
                    com.Parameters.AddWithValue("@Qualification", qualification);

                    if (com.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("Accountant added successfully", "Information");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error");
                }
            }
        }

        // Method to update an existing accountant in the database
        public void updateAccountant()
        {
            updateEmployee();

            string query = "UPDATE accountant SET " +
                           "email = @Email, " +
                           "qualification = @Qualification " +
                           "WHERE accountantID = @accountantId";

            using (MySqlConnection connection = Connector.getConnection())
            {
                try
                {
                    connection.Open();
                    MySqlCommand com = new MySqlCommand(query, connection);
                    com.Parameters.AddWithValue("@accountantId", accountantId);
                    com.Parameters.AddWithValue("@Email", email);
                    com.Parameters.AddWithValue("@Qualification", qualification);

                    if (com.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("Accountant updated successfully", "Information");
                    }
                    else
                    {
                        MessageBox.Show("Updating accountant failed", "Error");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error");
                }
            }
        }

        public Dictionary<String, String> getAccountantDetails()
        {
            Dictionary<String, String> accountantDetails = new Dictionary<String, String>()
            {
                { "email", this.email },
                { "qualification", this.qualification },
            };

            return accountantDetails;
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
