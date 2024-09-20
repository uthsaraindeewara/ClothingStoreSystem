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
    internal class Order
    {
        private int orderId;
        private DateTime date;
        private string status;
        private decimal amount;
        private int employeeId;
        private int customerId;

        // Constructor called when a new order is created
        public Order(DateTime date, string status, decimal amount, int employeeId, int customerId)
        {
            this.date = date;
            this.status = status;
            this.amount = amount;
            this.employeeId = employeeId;
            this.customerId = customerId;
        }

        // Constructor called when an existing order is retrieved
        public Order(int orderId)
        {
            string query = "SELECT orderID, date, status, amount, employeeID, cusID FROM order_tbl WHERE orderID = @orderId";

            using (MySqlConnection connection = Connector.getConnection())
            {
                try
                {
                    connection.Open();
                    MySqlCommand com = new MySqlCommand(query, connection);
                    com.Parameters.AddWithValue("@orderId", orderId);
                    MySqlDataReader dr = com.ExecuteReader();

                    if (dr.Read())
                    {
                        this.orderId = Convert.ToInt32(dr["orderID"]);
                        this.date = Convert.ToDateTime(dr["date"]);
                        this.status = dr["status"].ToString();
                        this.amount = Convert.ToDecimal(dr["amount"]);
                        this.employeeId = Convert.ToInt32(dr["employeeID"]);
                        this.customerId = Convert.ToInt32(dr["cusID"]);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error");
                }
            }
        }

        // Add new order
        public void addOrder()
        {
            string query = "INSERT INTO order_tbl (orderID, date, status, amount, employeeID, cusID) " +
                           "VALUES (@orderId, @date, @status, @amount, @employeeId, @customerId)";

            using (MySqlConnection connection = Connector.getConnection())
            {
                try
                {
                    connection.Open();
                    MySqlCommand com = new MySqlCommand(query, connection);

                    com.Parameters.AddWithValue("@orderId", orderId);
                    com.Parameters.AddWithValue("@date", date);
                    com.Parameters.AddWithValue("@status", status);
                    com.Parameters.AddWithValue("@amount", amount);
                    com.Parameters.AddWithValue("@employeeId", employeeId);
                    com.Parameters.AddWithValue("@customerId", customerId);

                    if (com.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("Order added successfully", "Information");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error");
                }
            }
        }

        // Update the order
        public void updateOrder()
        {
            string query = "UPDATE order_tbl SET " +
                           "date = @date, " +
                           "status = @status, " +
                           "amount = @amount, " +
                           "employeeID = @employeeId, " +
                           "cusID = @customerId " +
                           "WHERE orderID = @orderId";

            using (MySqlConnection connection = Connector.getConnection())
            {
                try
                {
                    connection.Open();
                    MySqlCommand com = new MySqlCommand(query, connection);

                    com.Parameters.AddWithValue("@orderId", orderId);
                    com.Parameters.AddWithValue("@date", date);
                    com.Parameters.AddWithValue("@status", status);
                    com.Parameters.AddWithValue("@amount", amount);
                    com.Parameters.AddWithValue("@employeeId", employeeId);
                    com.Parameters.AddWithValue("@customerId", customerId);

                    if (com.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("Order updated successfully", "Information");
                    }
                    else
                    {
                        MessageBox.Show("Updating order unsuccessful", "Error");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error");
                }
            }
        }

        // Return details as a dictionary of strings
        public Dictionary<String, String> getOrderDetails()
        {
            Dictionary<String, String> orderDetails = new Dictionary<String, String>()
            {
                { "orderId", this.orderId.ToString() },
                { "date", this.date.ToString("yyyy-MM-dd") },
                { "status", this.status },
                { "amount", this.amount.ToString("F2") },
                { "employeeId", this.employeeId.ToString() },
                { "customerId", this.customerId.ToString() }
            };

            return orderDetails;
        }

        // Getter for Order ID
        public int getOrderId()
        {
            return orderId;
        }

        // Setter for Order ID
        public void setOrderId(int orderId)
        {
            this.orderId = orderId;
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

        // Getter for Status
        public string getStatus()
        {
            return status;
        }

        // Setter for Status
        public void setStatus(string status)
        {
            this.status = status;
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

        // Getter for Customer ID
        public int getCustomerId()
        {
            return customerId;
        }

        // Setter for Customer ID
        public void setCustomerId(int customerId)
        {
            this.customerId = customerId;
        }
    }
}

