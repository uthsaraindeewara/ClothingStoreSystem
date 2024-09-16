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
    internal class Manager
    {
        private int managerId;
        private string role;
        private string email;
        private string qualification;
        public Manager(int managetId, string role, string email, string qualification)
        {
            this.managerId = managetId;
            this.role = role;
            this.email = email;
            this.qualification = qualification;
        }

        public Manager(int managerId)
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

        public void addManager()
        {
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
    }
}
