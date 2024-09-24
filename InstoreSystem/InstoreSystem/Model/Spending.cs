using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using FinalTest.Model;
using InstoreSystem.Model;

namespace FinalTest.Model
{
    internal class Spending
    {
        public int spendingId;
        public string type;
        public string description;
        public decimal amount;
        public int storeId;
        public DateTime date;

        // Constructor for creating a new spending record
        public Spending(int spendingId, string type, string description, decimal amount, int storeId, DateTime date)
        {
            this.spendingId = spendingId;
            this.type = type;
            this.description = description;
            this.amount = amount;
            this.storeId = storeId;
            this.date = date;
        }

        // Constructor for retrieving an existing spending record
        public Spending(int spendingId)
        {
            string query = "SELECT spendingID, type, description, amount, storeID, date FROM spending WHERE spendingID = @spendingId";

            using (MySqlConnection connection = Connector.getConnection())
            {
                try
                {
                    connection.Open();
                    MySqlCommand com = new MySqlCommand(query, connection);
                    com.Parameters.AddWithValue("@spendingId", spendingId);
                    MySqlDataReader dr = com.ExecuteReader();

                    if (dr.Read())
                    {
                        this.spendingId = Convert.ToInt32(dr["spendingID"]);
                        this.type = dr["type"].ToString();
                        this.description = dr["description"].ToString();
                        this.amount = Convert.ToDecimal(dr["amount"]);
                        this.storeId = Convert.ToInt32(dr["storeID"]);
                        this.date = Convert.ToDateTime(dr["date"]);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error");
                }
            }
        }

        // Add new spending record
        public void addSpending()
        {
            string query = "INSERT INTO spending (spendingID, type, description, amount, storeID, date) " +
                           "VALUES (@spendingId, @type, @description, @amount, @storeId, @date)";

            using (MySqlConnection connection = Connector.getConnection())
            {
                try
                {
                    connection.Open();
                    MySqlCommand com = new MySqlCommand(query, connection);

                    com.Parameters.AddWithValue("@spendingId", spendingId);
                    com.Parameters.AddWithValue("@type", type);
                    com.Parameters.AddWithValue("@description", description);
                    com.Parameters.AddWithValue("@amount", amount);
                    com.Parameters.AddWithValue("@storeId", storeId); 
                    com.Parameters.AddWithValue("@date", date);

                    if (com.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("Spending record added successfully", "Information");
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
        public void updateSpending()
        {
            string query = "UPDATE spending SET " +
                           "type = @type, " +
                           "description = @description, " +
                           "amount = @amount, " +
                           "storeID = @storeId, " +
                           "date = @date " +               // Correctly formatted
                           "WHERE spendingID = @spendingId";

            using (MySqlConnection connection = Connector.getConnection())
            {
                try
                {
                    connection.Open();
                    MySqlCommand com = new MySqlCommand(query, connection);

                    com.Parameters.AddWithValue("@spendingId", spendingId);
                    com.Parameters.AddWithValue("@type", type);
                    com.Parameters.AddWithValue("@description", description);
                    com.Parameters.AddWithValue("@amount", amount);
                    com.Parameters.AddWithValue("@storeId", storeId);
                    com.Parameters.AddWithValue("@date", date);

                    if (com.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("Spending record updated successfully", "Information");
                    }
                    else
                    {
                        MessageBox.Show("Updating spending record unsuccessful", "Error");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error");
                }
            }
        }

        // Return details as a dictionary of strings
        public Dictionary<String, String> getSpendingDetails()
        {
            Dictionary<String, String> spendingDetails = new Dictionary<String, String>()
            {
                { "spendingId", this.spendingId.ToString() },
                { "type", this.type },
                { "description", this.description },
                { "amount", this.amount.ToString() },
                { "storeId", this.storeId.ToString() },
                { "date", this.date.ToString("yyyy-MM-dd") }
            };

            return spendingDetails;
        }

        // Getter for Spending ID
        public int getSpendingId()
        {
            return spendingId;
        }

        // Setter for Spending ID
        public void setSpendingId(int spendingId)
        {
            this.spendingId = spendingId;
        }

        // Getter for Type
        public string getType()
        {
            return type;
        }

        // Setter for Type
        public void setType(string type)
        {
            this.type = type;
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

        // Getter for Amount
        public decimal getAmount()
        {
            return amount;
        }

        // Setter for Amount
        public void setAmount(decimal amount)
        {
            this.amount = amount;
        }

        // Getter for Store ID
        public int getStoreId()
        {
            return storeId;
        }

        // Setter for Store ID
        public void setStoreId(int storeId)
        {
            this.storeId = storeId;
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
    }
}
