using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using InstoreSystem.Model;
using MySql.Data.MySqlClient;

namespace InstoreSystem.Interface
{
    public partial class AddDiscount : Form
    {
        int productId;

        public AddDiscount(int productId)
        {
            InitializeComponent();

            this.productId = productId;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Check if txtAmount is not empty and contains a valid decimal
            if (string.IsNullOrWhiteSpace(txtAmount.Text) || !decimal.TryParse(txtAmount.Text, out _))
            {
                MessageBox.Show("Please enter a valid discount amount.", "Validation Error");
                return;
            }

            string sql = @"DELETE FROM discount WHERE productID = @productId";

            using (MySqlConnection connection = Connector.getConnection())
            {
                try
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand(sql, connection);

                    // Assigning parameters directly from the input fields
                    command.Parameters.AddWithValue("@productID", int.Parse(txtProductId.Text));


                    // Execute the query
                    int rowsAffected = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error");
                }
            }

            // SQL query to insert or update the discount table
            string query = @"INSERT INTO discount (productID, amount, startDate, endDate) 
                     VALUES (@productID, @amount, @startDate, @endDate)
                     ON DUPLICATE KEY UPDATE amount = @amount, startDate = @startDate, endDate = @endDate";

            using (MySqlConnection connection = Connector.getConnection())
            {
                try
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand(query, connection);

                    // Assigning parameters directly from the input fields
                    command.Parameters.AddWithValue("@productID", int.Parse(txtProductId.Text));
                    command.Parameters.AddWithValue("@amount", decimal.Parse(txtAmount.Text));
                    command.Parameters.AddWithValue("@startDate", dtpStartDate.Value);
                    command.Parameters.AddWithValue("@endDate", dtpEndDate.Value);

                    // Execute the query
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Discount details saved successfully.", "Success");

                        this.Dispose();
                    }
                    else
                    {
                        MessageBox.Show("Failed to save discount details.", "Error");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error");
                }
            }
        }

        private void AddDiscount_Load(object sender, EventArgs e)
        {
            string query = @"SELECT product.productID, 
                                    product.productName, 
                                    IFNULL(discount.amount, 0) AS discountAmount, 
                                    IFNULL(discount.startDate, CURDATE()) AS startDate, 
                                    IFNULL(discount.endDate, CURDATE()) AS endDate
                             FROM product 
                             LEFT JOIN discount ON product.productID = discount.productID 
                             WHERE product.productID = @productID";

            using (MySqlConnection connection = Connector.getConnection())
            {
                try
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@productID", productId);
                    MySqlDataReader dr = command.ExecuteReader();

                    if (dr.Read())
                    {
                        txtProductId.Text = dr.GetInt32("productID").ToString();
                        lblProductName.Text = dr.GetString("productName");
                        txtAmount.Text = dr.GetDecimal("discountAmount").ToString();
                        dtpStartDate.Value = dr.GetDateTime("startDate");
                        dtpEndDate.Value = dr.GetDateTime("endDate");
                    }
                    else
                    {
                        MessageBox.Show("No product found with the given Product ID.", "Info");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error");
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
