using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace InstoreSystem.Model
{
    internal class Accountant
    {
        private int accountantId;
        private string email;
        private string qualification;

        // Constructor for creating a new accountant
        public Accountant(int accountantId, string email, string qualification)
        {
            this.accountantId = accountantId;
            this.email = email;
            this.qualification = qualification;
        }

        // Constructor for fetching an existing accountant from the database
        public Accountant(int accountantId)
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
        public void addAccountant()
        {
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
    }
}
