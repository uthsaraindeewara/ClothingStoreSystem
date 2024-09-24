using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinalTest.Model;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using InstoreSystem.Model;


namespace FinalTest.Model
{
    internal class Complain
    {
        protected int complainId;
        protected DateTime date;
        protected string description;
        protected int employeeId;

        // Constructor for creating a new complain record
        public Complain(int complainId, DateTime date, string description, int employeeId)
        {
            this.complainId = complainId;
            this.date = date;
            this.description = description;
            this.employeeId = employeeId;
        }

        // Constructor for retrieving an existing complain record
        public Complain(int complainId)
        {
            string query = "SELECT complainID, date, description, employeeID FROM complain WHERE complainID = @complainId";

            using (MySqlConnection connection = Connector.getConnection())
            {
                try
                {
                    connection.Open();
                    MySqlCommand com = new MySqlCommand(query, connection);
                    com.Parameters.AddWithValue("@complainId", complainId);
                    MySqlDataReader dr = com.ExecuteReader();

                    if (dr.Read())
                    {
                        this.complainId = Convert.ToInt32(dr["complainID"]);
                        this.date = Convert.ToDateTime(dr["date"]);
                        this.description = dr["description"].ToString();
                        this.employeeId = Convert.ToInt32(dr["employeeID"]);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error");
                }
            }
        }

        // Add new complain record
        public void addComplain()
        {
            string query = "INSERT INTO complain (complainID, date, description, employeeID) " +
                           "VALUES (@complainId, @date, @description, @employeeId)";

            using (MySqlConnection connection = Connector.getConnection())
            {
                try
                {
                    connection.Open();
                    MySqlCommand com = new MySqlCommand(query, connection);

                    com.Parameters.AddWithValue("@complainId", complainId);
                    com.Parameters.AddWithValue("@date", date);
                    com.Parameters.AddWithValue("@description", description);
                    com.Parameters.AddWithValue("@employeeId", employeeId);

                    if (com.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("Complain record added successfully", "Information");
                    }
                    else
                    {
                        MessageBox.Show("Unable to add record. Please check your input and try again.", "Error");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error");
                }
            }
        }

        // Update the complain record
        public void updateComplain()
        {
            string query = "UPDATE complain SET " +
                           "date = @date, " +
                           "description = @description, " +
                           "employeeID = @employeeId " +
                           "WHERE complainID = @complainId";

            using (MySqlConnection connection = Connector.getConnection())
            {
                try
                {
                    connection.Open();
                    MySqlCommand com = new MySqlCommand(query, connection);

                    com.Parameters.AddWithValue("@complainId", complainId);
                    com.Parameters.AddWithValue("@date", date);
                    com.Parameters.AddWithValue("@description", description);
                    com.Parameters.AddWithValue("@employeeId", employeeId);

                    if (com.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("Complain record updated successfully", "Information");
                    }
                    else
                    {
                        MessageBox.Show("Updating complain record unsuccessful", "Error");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error");
                }
            }
        }

        // Return details as a dictionary of strings
        public Dictionary<String, String> getComplainDetails()
        {
            Dictionary<String, String> complainDetails = new Dictionary<String, String>()
            {
                { "complainId", this.complainId.ToString() },
                { "date", this.date.ToString("yyyy-MM-dd") },
                { "description", this.description },
                { "employeeId", this.employeeId.ToString() }
            };

            return complainDetails;
        }

        // Getter for Complain ID
        public int getComplainId()
        {
            return complainId;
        }

        // Setter for Complain ID
        public void setComplainId(int complainId)
        {
            this.complainId = complainId;
        }

        // Getter for Date
        public DateTime getDate()
        {
            return date;
        }

        // Setter for Date
        public void setDate(DateTime date)
        {
            this.date = date;
        }

        // Getter for Description
        public string getDescription()
        {
            return description;
        }

        // Setter for Description
        public void setDescription(string description)
        {
            this.description = description;
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
    }
}

