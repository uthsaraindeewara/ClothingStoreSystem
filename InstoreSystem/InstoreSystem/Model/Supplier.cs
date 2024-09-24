using InstoreSystem.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FinalTest.Model
{
    internal class Supplier
    {
        public int supplierId;
        public string address;
        public string email;

        // Constructor for creating a new supplier record
        public Supplier(int supplierId, string address, string email)
        {
            this.supplierId = supplierId;
            this.address = address;
            this.email = email;
        }

        // Constructor for retrieving an existing supplier record
        public Supplier(int supplierId)
        {
            string query = "SELECT supplierID, address, email FROM supplier WHERE supplierID = @supplierId";

            using (MySqlConnection connection = Connector.getConnection())
            {
                try
                {
                    connection.Open();
                    MySqlCommand com = new MySqlCommand(query, connection);
                    com.Parameters.AddWithValue("@supplierId", supplierId);
                    MySqlDataReader dr = com.ExecuteReader();

                    if (dr.Read())
                    {
                        this.supplierId = Convert.ToInt32(dr["supplierID"]);
                        this.address = dr["address"].ToString();
                        this.email = dr["email"].ToString();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error");
                }
            }
        }

        // Add new supplier record
        public void addSupplier()
        {
            string query = "INSERT INTO supplier (supplierID, address, email) " +
                           "VALUES (@supplierId, @address, @email)";

            using (MySqlConnection connection = Connector.getConnection())
            {
                try
                {
                    connection.Open();
                    MySqlCommand com = new MySqlCommand(query, connection);

                    com.Parameters.AddWithValue("@supplierId", supplierId);
                    com.Parameters.AddWithValue("@address", address);
                    com.Parameters.AddWithValue("@email", email);

                    if (com.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("Supplier record added successfully", "Information");
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

        // Update the supplier record
        public void updateSupplier()
        {
            string query = "UPDATE supplier SET " +
                           "address = @address, " +
                           "email = @email " +
                           "WHERE supplierID = @supplierId";

            using (MySqlConnection connection = Connector.getConnection())
            {
                try
                {
                    connection.Open();
                    MySqlCommand com = new MySqlCommand(query, connection);

                    com.Parameters.AddWithValue("@supplierId", supplierId);
                    com.Parameters.AddWithValue("@address", address);
                    com.Parameters.AddWithValue("@email", email);

                    if (com.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("Supplier record updated successfully", "Information");
                    }
                    else
                    {
                        MessageBox.Show("Updating supplier record unsuccessful", "Error");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error");
                }
            }
        }

        // Delete a supplier record
        public void deleteSupplier()
        {
            string query = "DELETE FROM supplier WHERE supplierID = @supplierId";

            using (MySqlConnection connection = Connector.getConnection())
            {
                try
                {
                    connection.Open();
                    MySqlCommand com = new MySqlCommand(query, connection);

                    com.Parameters.AddWithValue("@supplierId", supplierId);

                    if (com.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("Supplier record deleted successfully", "Information");
                    }
                    else
                    {
                        MessageBox.Show("Unable to delete supplier record.", "Error");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error");
                }
            }
        }

        // Return details as a dictionary of strings
        public Dictionary<String, String> getSupplierDetails()
        {
            Dictionary<String, String> supplierDetails = new Dictionary<String, String>()
            {
                { "supplierId", this.supplierId.ToString() },
                { "address", this.address },
                { "email", this.email }
            };

            return supplierDetails;
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

        // Getter for Address
        public string getAddress()
        {
            return address;
        }

        // Setter for Address
        public void setAddress(string address)
        {
            this.address = address;
        }

        // Getter for Email
        public string getEmail()
        {
            return email;
        }

        // Setter for Email
        public void setEmail(string email)
        {
            this.email = email;
        }
    }
}
