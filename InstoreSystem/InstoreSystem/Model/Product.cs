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
    internal class Product
    {
        private int productId;
        private string productName;
        private string type;
        private string sizing;
        private string description;
        private Dictionary<string, int> quantity = new Dictionary<string, int>();
        private string category;
        private decimal price;

        // Constructor for creating a new product
        public Product(int productId, string productName, string type, string sizing, Dictionary<string,int> quantity, string category, decimal price, string description)
        {
            this.productId = productId;
            this.productName = productName;
            this.type = type;
            this.sizing = sizing;
            this.quantity = quantity;
            this.category = category;
            this.price = price;
            this.description = description;
        }

        // Constructor for retrieving an existing product by ID
        public Product(int productId)
        {
            using (MySqlConnection connection = Connector.getConnection())
            {
                try
                {
                    connection.Open();

                    string sql = "SELECT productID, productName, type, sizing, description, catagory, price FROM product WHERE productID = @productId";

                    using (MySqlCommand com = new MySqlCommand(sql, connection))
                    {
                        com.Parameters.AddWithValue("@productId", productId);
                        MySqlDataReader dr = com.ExecuteReader();

                        if (dr.Read())
                        {
                            this.productId = Convert.ToInt32(dr["productID"]);
                            this.productName = dr["productName"].ToString();
                            this.type = dr["type"].ToString();
                            this.sizing = dr["sizing"].ToString();
                            this.description = dr["description"].ToString();
                            this.category = dr["catagory"].ToString();
                            this.price = Convert.ToDecimal(dr["price"]);
                        }

                        dr.Close();
                    }

                    string sql2 = "SELECT size, quantity FROM product_quantity WHERE product_id = @productId";

                    using (MySqlCommand com1 = new MySqlCommand(sql2, connection))
                    {
                        com1.Parameters.AddWithValue("@productId", productId);
                        MySqlDataReader dr1 = com1.ExecuteReader();

                        while (dr1.Read())
                        {
                            quantity.Add(dr1.GetString("size"), dr1.GetInt32("quantity"));
                        }

                        dr1.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error");
                }
            }
        }

        // Method to add a new product to the database
        public void addProduct()
        {
            using (MySqlConnection connection = Connector.getConnection())
            {
                try
                {
                    connection.Open();
                    int ret = 0;

                    string sql = "INSERT INTO product (productName, type, sizing, description, catagory, price) " +
                                   "VALUES (@productName, @type, @sizing, @description, @category, @price)";

                    using (MySqlCommand com = new MySqlCommand(sql, connection))
                    {
                        com.Parameters.AddWithValue("@productName", productName);
                        com.Parameters.AddWithValue("@type", type);
                        com.Parameters.AddWithValue("@sizing", sizing);
                        com.Parameters.AddWithValue("@description", description);
                        com.Parameters.AddWithValue("@category", category);
                        com.Parameters.AddWithValue("@price", price);

                        ret = com.ExecuteNonQuery();
                    }

                    if (ret == 1)
                    {
                        string sql1 = @"INSERT INTO product_quantity (product_id, size, quantity) VALUES(@productId, @size, @quantity)";

                        using (MySqlCommand com1 = new MySqlCommand(sql1, connection))
                        {
                            foreach (KeyValuePair<string, int> kvp in quantity)
                            {
                                com1.Parameters.AddWithValue("@productId", productId);
                                com1.Parameters.AddWithValue("@size", kvp.Key);
                                com1.Parameters.AddWithValue("@quantity", kvp.Value);
                                com1.ExecuteNonQuery();
                                com1.Parameters.Clear();
                            }
                        }

                        MessageBox.Show("Product added successfully", "Information");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Method to update product details
        public void updateProduct()
        {
            string query = "UPDATE product SET " +
                           "productName = @productName, " +
                           "type = @type, " +
                           "sizing = @sizing, " +
                           "description = @description, " +
                           "catagory = @category, " +
                           "price = @price " +
                           "WHERE productID = @productId";

            using (MySqlConnection connection = Connector.getConnection())
            {
                try
                {
                    connection.Open();
                    MySqlCommand com = new MySqlCommand(query, connection);

                    com.Parameters.AddWithValue("@productName", productName);
                    com.Parameters.AddWithValue("@type", type);
                    com.Parameters.AddWithValue("@sizing", sizing);
                    com.Parameters.AddWithValue("@description", description);
                    com.Parameters.AddWithValue("@category", category);
                    com.Parameters.AddWithValue("@price", price);

                    if (com.ExecuteNonQuery() > 0)
                    {
                        string sql1 = @"INSERT INTO product_quantity (product_id, size, quantity) VALUES(@productId, @size, @quantity) ON DUPLICATE KEY UPDATE quantity = @quantity";

                        using (MySqlCommand com1 = new MySqlCommand(sql1, connection))
                        {
                            foreach (KeyValuePair<string, int> kvp in quantity)
                            {
                                com1.Parameters.AddWithValue("@productId", productId);
                                com1.Parameters.AddWithValue("@size", kvp.Key);
                                com1.Parameters.AddWithValue("@quantity", kvp.Value);
                                com1.ExecuteNonQuery();
                                com1.Parameters.Clear();
                            }
                        }

                        MessageBox.Show("Product updated successfully", "Information");
                    }
                    else
                    {
                        MessageBox.Show("Updating product unsuccessful", "Error");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error");
                }
            }
        }

        // Method to retrieve product details as a dictionary
        public Dictionary<String, String> getProductDetails()
        {
            Dictionary<String, String> productDetails = new Dictionary<String, String>()
            {
                { "productId", this.productId.ToString() },
                { "productName", this.productName },
                { "type", this.type },
                { "sizing", this.sizing },
                { "description", this.description },
                { "category", this.category },
                { "price", this.price.ToString("F2") }
            };

            return productDetails;
        }

        // Getter for Product ID
        public int getProductId()
        {
            return productId;
        }

        // Setter for Product ID
        public void setProductId(int productId)
        {
            this.productId = productId;
        }

        // Getter for Product Name
        public string getProductName()
        {
            return productName;
        }

        // Setter for Product Name
        public void setProductName(string productName)
        {
            this.productName = productName;
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

        public string getSizing()
        {
            return sizing;
        }

        public void setSizing(string sizing)
        {
            this.sizing = sizing;
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

        // Getter for Quantity
        public Dictionary<string, int> getQuantity()
        {
            return quantity;
        }

        // Setter for Quantity
        public void setQuantity(Dictionary<string, int> quantity)
        {
            this.quantity = quantity;
        }

        // Getter for Category
        public string getCategory()
        {
            return category;
        }

        // Setter for Category
        public void setCategory(string category)
        {
            this.category = category;
        }

        // Getter for Price
        public decimal getPrice()
        {
            return price;
        }

        // Setter for Price
        public void setPrice(decimal price)
        {
            this.price = price;
        }
    }
}

