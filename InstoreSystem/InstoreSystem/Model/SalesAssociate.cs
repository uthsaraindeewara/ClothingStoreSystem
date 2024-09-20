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
    internal class SalesAssociate : Employee
    {
        private int salesAssociateID;
        private string experience;
        private string type;

        // Constructor called when a new SalesAssociate object is created
        public SalesAssociate(string employeeName, string address, string contactNo, DateTime dOB, string gender, double salary, int storeId, string experience, string type)
            : base(employeeName, address, contactNo, dOB, gender, salary, storeId)
        {
            this.experience = experience;
            this.type = type;
        }

        // Constructor called when an existing SalesAssociate object is created from the database
        public SalesAssociate(int salesAssociateID)
            : base(salesAssociateID)
        {
            string query = "SELECT salesAssocID, experience, type FROM salesassociate WHERE salesAssocID = @salesAssociateID";

            using (MySqlConnection connection = Connector.getConnection())
            {
                try
                {
                    connection.Open();
                    MySqlCommand com = new MySqlCommand(query, connection);
                    com.Parameters.AddWithValue("@salesAssociateID", salesAssociateID);
                    MySqlDataReader dr = com.ExecuteReader();

                    if (dr.Read())
                    {
                        this.salesAssociateID = dr.GetInt32("salesAssocID");
                        this.experience = dr.GetString("experience");
                        this.type = dr.GetString("type");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error");
                }
            }
        }

        // Add new SalesAssociate to the database
        public void addSalesAssociate(int admin_id)
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
                        salesAssociateID = dr.GetInt32("employeeID");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error");
                }
            }

            string query = "INSERT INTO salesassociate (salesAssocID, experience, type) " +
                           "VALUES (@salesAssociateID, @experience, @type)";

            using (MySqlConnection connection = Connector.getConnection())
            {
                try
                {
                    connection.Open();
                    MySqlCommand com = new MySqlCommand(query, connection);

                    com.Parameters.AddWithValue("@salesAssociateID", salesAssociateID);
                    com.Parameters.AddWithValue("@experience", experience);
                    com.Parameters.AddWithValue("@type", type);

                    if (com.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("Sales Associate added successfully", "Information");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error");
                }
            }
        }

        // Update an existing SalesAssociate in the database
        public void updateSalesAssociate()
        {
            updateEmployee();

            string query = "UPDATE salesassociate SET " +
                           "experience = @experience, " +
                           "type = @type " +
                           "WHERE salesAssocID = @salesAssociateID";

            using (MySqlConnection connection = Connector.getConnection())
            {
                try
                {
                    connection.Open();
                    MySqlCommand com = new MySqlCommand(query, connection);

                    com.Parameters.AddWithValue("@salesAssociateID", salesAssociateID);
                    com.Parameters.AddWithValue("@experience", experience);
                    com.Parameters.AddWithValue("@type", type);

                    if (com.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("Sales Associate updated successfully", "Information");
                    }
                    else
                    {
                        MessageBox.Show("Updating Sales Associate unsuccessful", "Error");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error");
                }
            }
        }

        public Dictionary<String, String> getSalesAssociateDetails()
        {
            Dictionary<String, String> salesAssociateDetails = new Dictionary<String, String>()
            {
                { "experience", this.experience },
                { "type", this.type },
            };

            return salesAssociateDetails;
        }

        public void setType(String type)
        {
            this.type = type;
        }

        public void setExperience(String experience)
        {
            this.experience = experience;
        }
    }
}
