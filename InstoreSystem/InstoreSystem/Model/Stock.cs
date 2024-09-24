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
    internal class Stock
    {
        public int stockId;
        public decimal cost;
        public int quantity;
        public int supplierId;

        // Constructor for creating a new stock record
        public Stock(int stockId, decimal cost, int quantity, int supplierId)
        {
            this.stockId = stockId;
            this.cost = cost;
            this.quantity = quantity;
            this.supplierId = supplierId;
        }

        // Constructor for retrieving an existing stock record
        public Stock(int stockId)
        {
            string query = "SELECT stockID, cost, quantity, supplierID FROM stock WHERE stockID = @stockId";

            using (MySqlConnection connection = Connector.getConnection())
            {
                try
                {
                    connection.Open();
                    MySqlCommand com = new MySqlCommand(query, connection);
                    com.Parameters.AddWithValue("@stockId", stockId);
                    MySqlDataReader dr = com.ExecuteReader();

                    if (dr.Read())
                    {
                        this.stockId = Convert.ToInt32(dr["stockID"]);
                        this.cost = Convert.ToDecimal(dr["cost"]);
                        this.quantity = Convert.ToInt32(dr["quantity"]);
                        this.supplierId = Convert.ToInt32(dr["supplierID"]);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error");
                }
            }
        }

        // Add new stock record
        public void addStock()
        {
            string query = "INSERT INTO stock (stockID, cost, quantity, supplierID) " +
                           "VALUES (@stockId, @cost, @quantity, @supplierId)";

            using (MySqlConnection connection = Connector.getConnection())
            {
                try
                {
                    connection.Open();
                    MySqlCommand com = new MySqlCommand(query, connection);

                    com.Parameters.AddWithValue("@stockId", stockId);
                    com.Parameters.AddWithValue("@cost", cost);
                    com.Parameters.AddWithValue("@quantity", quantity);
                    com.Parameters.AddWithValue("@supplierId", supplierId);

                    if (com.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("Stock record added successfully", "Information");
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

        // Update the stock record
        public void updateStock()
        {
            string query = "UPDATE stock SET " +
                           "cost = @cost, " +
                           "quantity = @quantity, " +
                           "supplierID = @supplierId " +
                           "WHERE stockID = @stockId";

            using (MySqlConnection connection = Connector.getConnection())
            {
                try
                {
                    connection.Open();
                    MySqlCommand com = new MySqlCommand(query, connection);

                    com.Parameters.AddWithValue("@stockId", stockId);
                    com.Parameters.AddWithValue("@cost", cost);
                    com.Parameters.AddWithValue("@quantity", quantity);
                    com.Parameters.AddWithValue("@supplierId", supplierId);

                    if (com.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("Stock record updated successfully", "Information");
                    }
                    else
                    {
                        MessageBox.Show("Updating stock record unsuccessful", "Error");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error");
                }
            }
        }

        //delete stock record
        public void deleteStock()
        {
            string query = "DELETE FROM stock WHERE stockID=@stockId";

            using (MySqlConnection connection = Connector.getConnection())
            {
                try
                {
                    connection.Open();
                    MySqlCommand com = new MySqlCommand(query, connection);

                    com.Parameters.AddWithValue("@stockId", stockId);
                    

                    if (com.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("Stock record deleted successfully", "Information");
                    }
                    else
                    {
                        MessageBox.Show("Deleting stock record unsuccessful", "Error");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error");
                }
            }
        }

        // Return details as a dictionary of strings
        public Dictionary<String, String> getStockDetails()
        {
            Dictionary<String, String> stockDetails = new Dictionary<String, String>()
            {
                { "stockId", this.stockId.ToString() },
                { "cost", this.cost.ToString() },
                { "quantity", this.quantity.ToString() },
                { "supplierId", this.supplierId.ToString() }
            };

            return stockDetails;
        }

        // Getter for Stock ID
        public int getStockId()
        {
            return stockId;
        }

        // Setter for Stock ID
        public void setStockId(int stockId)
        {
            this.stockId = stockId;
        }

        // Getter for Cost
        public decimal getCost()
        {
            return cost;
        }

        // Setter for Cost
        public void setCost(decimal cost)
        {
            this.cost = cost;
        }

        // Getter for Quantity
        public int getQuantity()
        {
            return quantity;
        }

        // Setter for Quantity
        public void setQuantity(int quantity)
        {
            this.quantity = quantity;
        }

        // Getter for Supplier ID
        public int getSupplierId()
        {
            return supplierId;
        }

        // Setter for Supplier ID
        public void setSupplierId(int supplierId)
        {
            this.supplierId = supplierId;
        }
    }
}
