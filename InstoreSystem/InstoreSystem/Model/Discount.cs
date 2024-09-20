using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace InstoreSystem.Model
{
    internal class Discount
    {
        private int discountID;
        private decimal amount;
        private DateTime startDate;
        private DateTime endDate;
        private string type;
        private int productID;

        // Constructor for creating a new discount
        public Discount(decimal amount, DateTime startDate, DateTime endDate, string type, int productID)
        {
            this.amount = amount;
            this.startDate = startDate;
            this.endDate = endDate;
            this.type = type;
            this.productID = productID;
        }

        // Constructor for retrieving an existing discount by ID
        public Discount(int discountID)
        {
            string query = "SELECT discountID, amount, startDate, endDate, type, productID FROM discount WHERE discountID = @discountID";

            using (MySqlConnection connection = Connector.getConnection())
            {
                try
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@discountID", discountID);
                    MySqlDataReader dr = command.ExecuteReader();

                    if (dr.Read())
                    {
                        this.discountID = Convert.ToInt32(dr["discountID"]);
                        this.amount = Convert.ToDecimal(dr["amount"]);
                        this.startDate = Convert.ToDateTime(dr["startDate"]);
                        this.endDate = Convert.ToDateTime(dr["endDate"]);
                        this.type = dr["type"].ToString();
                        this.productID = Convert.ToInt32(dr["productID"]);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error");
                }
            }
        }

        // Method to add a new discount
        public void addDiscount()
        {
            string query = "INSERT INTO discount (amount, startDate, endDate, type, productID) VALUES (@amount, @startDate, @endDate, @type, @productID)";

            using (MySqlConnection connection = Connector.getConnection())
            {
                try
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand(query, connection);

                    command.Parameters.AddWithValue("@amount", amount);
                    command.Parameters.AddWithValue("@startDate", startDate);
                    command.Parameters.AddWithValue("@endDate", endDate);
                    command.Parameters.AddWithValue("@type", type);
                    command.Parameters.AddWithValue("@productID", productID);

                    if (command.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("Discount added successfully", "Information");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error");
                }
            }
        }

        // Method to update an existing discount
        public void updateDiscount()
        {
            string query = "UPDATE discount SET amount = @amount, startDate = @startDate, endDate = @endDate, type = @type, productID = @productID WHERE discountID = @discountID";

            using (MySqlConnection connection = Connector.getConnection())
            {
                try
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand(query, connection);

                    command.Parameters.AddWithValue("@discountID", discountID);
                    command.Parameters.AddWithValue("@amount", amount);
                    command.Parameters.AddWithValue("@startDate", startDate);
                    command.Parameters.AddWithValue("@endDate", endDate);
                    command.Parameters.AddWithValue("@type", type);
                    command.Parameters.AddWithValue("@productID", productID);

                    if (command.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("Discount updated successfully", "Information");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error");
                }
            }
        }

        // Method to delete an existing discount by ID
        public void deleteDiscount()
        {
            string query = "DELETE FROM discount WHERE discountID = @discountID";

            using (MySqlConnection connection = Connector.getConnection())
            {
                try
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@discountID", discountID);

                    if (command.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("Discount deleted successfully", "Information");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error");
                }
            }
        }

        // Getter for Discount ID
        public int getDiscountID()
        {
            return discountID;
        }

        // Setter for Discount ID
        public void setDiscountID(int discountID)
        {
            this.discountID = discountID;
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

        // Getter for Start Date
        public DateTime getStartDate()
        {
            return startDate;
        }

        // Setter for Start Date
        public void setStartDate(DateTime startDate)
        {
            this.startDate = startDate;
        }

        // Getter for End Date
        public DateTime getEndDate()
        {
            return endDate;
        }

        // Setter for End Date
        public void setEndDate(DateTime endDate)
        {
            this.endDate = endDate;
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

        // Getter for Product ID
        public int getProductID()
        {
            return productID;
        }

        // Setter for Product ID
        public void setProductID(int productID)
        {
            this.productID = productID;
        }
    }
}
