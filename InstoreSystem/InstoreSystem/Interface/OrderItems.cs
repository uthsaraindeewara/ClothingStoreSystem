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
    public partial class OrderItems : Form
    {
        int orderId;

        public OrderItems(int orderId)
        {
            InitializeComponent();

            this.orderId = orderId;
        }

        private void OrderItems_Load(object sender, EventArgs e)
        {
            string query = @"SELECT order_item.product_id, 
                                    product.productName, 
                                    order_item.size, 
                                    product.price, 
                                    order_item.quantity 
                             FROM order_item 
                             INNER JOIN product ON order_item.product_id = product.productID
                             WHERE order_item.orderID = @orderId";

            using (MySqlConnection connection = Connector.getConnection())
            {
                try
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@orderId", orderId);
                    MySqlDataReader dr = command.ExecuteReader();

                    // Clear existing rows (optional)
                    dataGridView1.Rows.Clear();

                    // Loop through the result and add rows directly to the DataGridView
                    while (dr.Read())
                    {
                        dataGridView1.Rows.Add(dr.GetInt32("product_id"), dr.GetString("productName"), dr.GetString("size"), dr.GetDecimal("price"), dr.GetInt32("quantity"));
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
