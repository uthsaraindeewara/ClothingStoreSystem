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
    public partial class InventoryPanel : UserControl
    {
        public InventoryPanel()
        {
            InitializeComponent();
        }

        private void InventoryPanel_Load(object sender, EventArgs e)
        {
            txtSearch_TextChanged(this, null);
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();

            string id = "";
            string name = "";

            if (rbId.Checked)
            {
                id = txtSearch.Text;
            }
            else if (rbName.Checked)
            {
                name = txtSearch.Text;
            }

            using (MySqlConnection con = Connector.getConnection())
            {
                using (MySqlConnection con1 = Connector.getConnection())
                {
                    con.Open();
                    con1.Open();

                    // First query on the first connection
                    string sql = "SELECT productID, productName, price FROM product WHERE CAST(productID AS VARCHAR(10)) LIKE CONCAT('%', @productId, '%') AND productName LIKE CONCAT('%', @productName, '%')";

                    using (MySqlCommand com = new MySqlCommand(sql, con))
                    {
                        com.Parameters.AddWithValue("@productId", id);
                        com.Parameters.AddWithValue("@productName", name);

                        using (MySqlDataReader dr = com.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                dataGridView1.Rows.Add(dr.GetInt32("productID"), dr.GetString("productName"), "All", 0, dr.GetDouble("price"));
                                string sql1 = "SELECT size, quantity FROM product_quantity WHERE product_id = @product_id";

                                using (MySqlCommand com1 = new MySqlCommand(sql1, con1))
                                {
                                    com1.Parameters.AddWithValue("@product_id", dr.GetInt32("productID"));

                                    using (MySqlDataReader dr1 = com1.ExecuteReader())
                                    {
                                        DataGridViewComboBoxCell size = (DataGridViewComboBoxCell)dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells["ItemSize"];

                                        int quantity = 0;

                                        while (dr1.Read())
                                        {
                                            size.Items.Add(dr1.GetString("size"));
                                            quantity += dr1.GetInt32("quantity");
                                        }

                                        dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells["Quantity"].Value = quantity;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == dataGridView1.Columns["Delete"].Index)
            {
                // Get the product ID from the selected row (assuming the product ID is in the first column)
                int productId = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["ID"].Value);

                // Confirm deletion with the user
                DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete this product?", "Confirm Deletion", MessageBoxButtons.YesNo);

                if (dialogResult == DialogResult.Yes)
                {
                    using (MySqlConnection connection = Connector.getConnection())
                    {
                        try
                        {
                            connection.Open();

                            // SQL query to delete the product by ID
                            string sql = "DELETE FROM product_quantity WHERE product_id = @product_id";

                            using (MySqlCommand cmd = new MySqlCommand(sql, connection))
                            {
                                cmd.Parameters.AddWithValue("@product_id", productId);
                                int ret = cmd.ExecuteNonQuery();
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error: {ex.Message}", "Error");
                        }
                    }

                    using (MySqlConnection connection = Connector.getConnection())
                    {
                        try
                        {
                            connection.Open();

                            // SQL query to delete the product by ID
                            string sql = "DELETE FROM product WHERE productID = @productID";

                            using (MySqlCommand cmd = new MySqlCommand(sql, connection))
                            {
                                cmd.Parameters.AddWithValue("@productID", productId);
                                int ret = cmd.ExecuteNonQuery();

                                if (ret > 0)
                                {
                                    MessageBox.Show("Product deleted successfully!");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error: {ex.Message}", "Error");
                        }
                    }

                    txtSearch_TextChanged(this, null);
                }
            }
        }

        private void dataGridView1_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if ((e.ColumnIndex == dataGridView1.Columns["Update"].Index || e.ColumnIndex == dataGridView1.Columns["Delete"].Index) && e.RowIndex >= 0)
            {
                dataGridView1.Cursor = Cursors.Hand;
            }
        }

        private void dataGridView1_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.Cursor = Cursors.Default;
        }

        private void rbId_CheckedChanged(object sender, EventArgs e)
        {
            txtSearch.Text = "";
        }

        private void rbName_CheckedChanged(object sender, EventArgs e)
        {
            txtSearch.Text = "";
        }
    }
}
