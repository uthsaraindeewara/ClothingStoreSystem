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
    public partial class OrdersPanel : UserControl
    {
        public OrdersPanel()
        {
            InitializeComponent();
        }

        private void OrdersPanel_Load(object sender, EventArgs e)
        {
            string query = @"SELECT order_tbl.orderID, 
                            order_tbl.date, 
                            order_tbl.amount, 
                            order_tbl.discount, 
                            customer.address, 
                            customer.contactNo, 
                            order_tbl.status 
                     FROM order_tbl 
                     INNER JOIN customer ON order_tbl.cusID = customer.cusID
                     WHERE NOT order_tbl.status = 'Completed'";

            using (MySqlConnection connection = Connector.getConnection())
            {
                try
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand(query, connection);
                    MySqlDataReader dr = command.ExecuteReader();

                    // Clear existing rows (optional)
                    dataGridView1.Rows.Clear();

                    // Loop through the result and add rows to the DataGridView
                    while (dr.Read())
                    {
                        DateTime date = dr.GetDateTime("date");

                        // Add row to the DataGridView
                        dataGridView1.Rows.Add(dr.GetInt32("orderID"), date.ToString("yyyy-MM-dd"), dr.GetString("address"), dr.GetString("contactNo"), dr.GetDecimal("amount"), dr.GetDecimal("discount"), dr.GetString("status"));
                        
                        if (dr.GetString("status") == "Placed")
                        {
                            dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells["Status"].Style.BackColor = Color.Red;
                            dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells["ChangeStatus"].Value = "Process";
                        }
                        else if (dr.GetString("status") == "Processing")
                        {
                            dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells["Status"].Style.BackColor = Color.Yellow;
                            dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells["ChangeStatus"].Value = "Deliver";
                        }
                        else if (dr.GetString("status") == "Delivered")
                        {
                            dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells["Status"].Style.BackColor = Color.Green;
                            dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells["ChangeStatus"].Value = "Remove";
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error");
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == dataGridView1.Columns["ChangeStatus"].Index)
            {
                // Get the product ID from the selected row (assuming the product ID is in the first column)
                int orderId = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["ID"].Value);

                // Confirm deletion with the user
                DialogResult dialogResult = MessageBox.Show("Are you sure you want to change the status of the order?", "Confirm Change Status", MessageBoxButtons.YesNo);

                if (dialogResult == DialogResult.Yes)
                {
                    using (MySqlConnection connection = Connector.getConnection())
                    {
                        try
                        {
                            connection.Open();

                            // SQL query to delete the product by ID
                            string sql = "";

                            if (dataGridView1.Rows[e.RowIndex].Cells["ChangeStatus"].Value.ToString() == "Process")
                            {
                                sql = "UPDATE order_tbl SET status = 'Processing' WHERE orderID = @orderId";
                            }
                            else if (dataGridView1.Rows[e.RowIndex].Cells["ChangeStatus"].Value.ToString() == "Deliver")
                            {
                                sql = "UPDATE order_tbl SET status = 'Delivered' WHERE orderID = @orderId";
                            }
                            else if (dataGridView1.Rows[e.RowIndex].Cells["ChangeStatus"].Value.ToString() == "Remove")
                            {
                                sql = "UPDATE order_tbl SET status = 'Completed' WHERE orderID = @orderId";
                            }

                            using (MySqlCommand cmd = new MySqlCommand(sql, connection))
                            {
                                cmd.Parameters.AddWithValue("@orderID", orderId);
                                int ret = cmd.ExecuteNonQuery();

                                if (ret > 0)
                                {
                                    OrdersPanel_Load(this, null);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error: {ex.Message}", "Error");
                        }
                    }
                }
            }
            else if (e.RowIndex >= 0 && e.ColumnIndex == dataGridView1.Columns["Items"].Index)
            {
                int orderId = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["ID"].Value);

                OrderItems items = new OrderItems(orderId);
                items.ShowDialog();
            }
        }

        private void dataGridView1_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if ((e.ColumnIndex == dataGridView1.Columns["ChangeStatus"].Index || e.ColumnIndex == dataGridView1.Columns["Items"].Index) && e.RowIndex >= 0)
            {
                dataGridView1.Cursor = Cursors.Hand;
            }
        }

        private void dataGridView1_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.Cursor = Cursors.Default;
        }
    }
}
