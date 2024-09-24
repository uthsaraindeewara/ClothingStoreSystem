using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.Windows.Forms;
using InstoreSystem.Model;
using MySql.Data.MySqlClient;

namespace InstoreSystem.Interface
{
    public partial class CashierPanel : UserControl
    {
        double total = 0;
        Dictionary<int, double> discounts = new Dictionary<int, double>();

        public CashierPanel()
        {
            InitializeComponent();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView1.Columns["Delete"].Index && e.RowIndex >= 0)
            {
                dataGridView1.Cursor = Cursors.Hand;
            }
        }

        private void dataGridView1_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.Cursor = Cursors.Default;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == dataGridView1.Columns["Delete"].Index)
            {
                // Get the row index of the clicked cell
                int rowIndex = e.RowIndex;

                txtDiscount.Text = (Convert.ToDouble(txtDiscount.Text) - discounts[rowIndex]).ToString();

                discounts.Remove(rowIndex);

                dataGridView1.Rows.RemoveAt(rowIndex);

                dataGridView1.ClearSelection();
            }
        }

        private void txtIdSearch_TextChanged(object sender, EventArgs e)
        {
            using (MySqlConnection connection = Connector.getConnection())
            {
                try
                {
                    dataGridView2.Rows.Clear();

                    connection.Open();

                    string sql = "SELECT productID, productName FROM product WHERE productID LIKE '%' + @productId + '%'";

                    using (MySqlCommand com = new MySqlCommand(sql, connection))
                    {
                        com.Parameters.AddWithValue("@productId", txtIdSearch.Text);
                        MySqlDataReader dr = com.ExecuteReader();

                        while (dr.Read())
                        {
                            dataGridView2.Rows.Add(dr.GetInt32("productID"), dr.GetString("productName"));
                        }

                        if (dataGridView2.Rows.Count > 0)
                        {
                            dataGridView2.Rows[0].Selected = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error");
                }
            }
        }

        private void txtNameSearch_TextChanged(object sender, EventArgs e)
        {
            dataGridView2.Rows.Clear();

            if (txtNameSearch.Text != "")
            {
                using (MySqlConnection connection = Connector.getConnection())
                {
                    try
                    {
                        dataGridView2.Rows.Clear();

                        connection.Open();

                        string sql = "SELECT productID, productName FROM product WHERE productName LIKE CONCAT('%', @productName, '%')";

                        using (MySqlCommand com = new MySqlCommand(sql, connection))
                        {
                            com.Parameters.AddWithValue("@productName", txtNameSearch.Text);
                            MySqlDataReader dr = com.ExecuteReader();

                            while (dr.Read())
                            {
                                dataGridView2.Rows.Add(dr.GetInt32("productID"), dr.GetString("productName"));
                            }

                            if (dataGridView2.Rows.Count > 0)
                            {
                                dataGridView2.Rows[0].Selected = true;
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

        private void txtNameSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (dataGridView2.Rows.Count == 0)
            {
                return; // No rows in the DataGridView, so no need to handle arrow keys
            }

            // Get the currently selected row index
            int currentIndex = dataGridView2.CurrentCell?.RowIndex ?? -1;

            // Handle the Down arrow key
            if (e.KeyCode == Keys.Down)
            {
                if (currentIndex < dataGridView2.Rows.Count - 1)
                {
                    // Move to the next row
                    dataGridView2.Rows[currentIndex + 1].Selected = true;
                    dataGridView2.CurrentCell = dataGridView2.Rows[currentIndex + 1].Cells[0];
                }

                // Prevent the TextBox from losing focus
                e.Handled = true;
            }

            // Handle the Up arrow key
            if (e.KeyCode == Keys.Up)
            {
                if (currentIndex > 0)
                {
                    // Move to the previous row
                    dataGridView2.Rows[currentIndex - 1].Selected = true;
                    dataGridView2.CurrentCell = dataGridView2.Rows[currentIndex - 1].Cells[0];
                }

                // Prevent the TextBox from losing focus
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Enter)
            {
                // Disable the search boxes
                txtIdSearch.Enabled = false;
                txtNameSearch.Enabled = false;

                // Ensure a row is selected
                if (dataGridView2.SelectedRows.Count > 0)
                {
                    // Get the selected row
                    DataGridViewRow selectedRow = dataGridView2.SelectedRows[0];

                    // Remove all other rows except the selected one
                    for (int i = dataGridView2.Rows.Count - 1; i >= 0; i--)
                    {
                        if (!dataGridView2.Rows[i].Selected)
                        {
                            dataGridView2.Rows.RemoveAt(i);
                        }
                    }

                    // Set focus to the NumericUpDown control
                    nucQuantity.Focus();

                    // Keep the selected row highlighted
                    selectedRow.Selected = true;
                }

                // Suppress the default Enter key behavior
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void txtIdSearch_Enter(object sender, EventArgs e)
        {
            txtNameSearch.Text = "";
        }

        private void txtNameSearch_Enter(object sender, EventArgs e)
        {
            txtIdSearch.Text = "";
        }

        private void txtIdSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtNameSearch.Enabled = false;
                txtIdSearch.Enabled = false;
                nucQuantity.Focus();
            }
        }

        private void nucQuantity_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // Ensure a row is selected
                if (dataGridView2.SelectedRows.Count > 0)
                {
                    // Get the selected row
                    DataGridViewRow selectedRow = dataGridView2.SelectedRows[0];

                    // Get the ID from the selected row
                    int selectedId = Convert.ToInt32(selectedRow.Cells["ItemId"].Value); // Adjust the column name accordingly

                    using (MySqlConnection connection = Connector.getConnection())
                    {
                        try
                        {
                            dataGridView2.Rows.Clear();

                            connection.Open();

                            string sql = "SELECT p.productID, p.productName, p.price, COALESCE(d.amount, 0) AS discount FROM product p LEFT JOIN discount d ON p.productID = d.productID WHERE p.productID = @productId";

                            using (MySqlCommand com = new MySqlCommand(sql, connection))
                            {
                                com.Parameters.AddWithValue("@productId", selectedId);
                                MySqlDataReader dr = com.ExecuteReader();

                                if (dr.Read())
                                {
                                    discounts.Add(dataGridView1.RowCount, dr.GetDouble("discount") * Convert.ToDouble(nucQuantity.Value));

                                    txtDiscount.Text = (Convert.ToDouble(txtDiscount.Text) + (dr.GetDouble("discount") * Convert.ToDouble(nucQuantity.Value))).ToString();

                                    dataGridView1.Rows.Add(dr.GetInt32("productID"), dr.GetString("productName"), nucQuantity.Value, dr.GetDouble("price"), dr.GetDouble("price") * Convert.ToDouble(nucQuantity.Value));
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error: {ex.Message}", "Error");
                        }
                    }

                    // Clear and re-enable the search boxes
                    txtIdSearch.Clear();
                    txtNameSearch.Clear();
                    txtIdSearch.Enabled = true;
                    txtNameSearch.Enabled = true;
                    nucQuantity.Value = 1;

                    // Set focus to the txtIdSearch textbox
                    txtIdSearch.Focus();
                }

                // Suppress the default Enter key behavior
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
            else if (e.KeyCode == Keys.Escape)
            {
                // Clear text and enable the search boxes
                txtIdSearch.Clear();
                txtNameSearch.Clear();
                txtIdSearch.Enabled = true;
                txtNameSearch.Enabled = true;
                nucQuantity.Value = 1;

                // Optionally, you can set the focus back to the txtIdSearch if needed
                txtIdSearch.Focus();

                // Suppress the default Escape key behavior if needed
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void btnAddItem_Click(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedRows.Count > 0)
            {
                if (nucQuantity.Value > 0)
                {
                    // Get the selected row
                    DataGridViewRow selectedRow = dataGridView2.SelectedRows[0];

                    // Get the ID from the selected row
                    int selectedId = Convert.ToInt32(selectedRow.Cells["ItemId"].Value); // Adjust the column name accordingly

                    using (MySqlConnection connection = Connector.getConnection())
                    {
                        try
                        {
                            dataGridView2.Rows.Clear();

                            connection.Open();

                            string sql = "SELECT p.productID, p.productName, p.price, COALESCE(d.amount, 0) AS discount FROM product p LEFT JOIN discount d ON p.productID = d.productID WHERE p.productID = @productId";

                            using (MySqlCommand com = new MySqlCommand(sql, connection))
                            {
                                com.Parameters.AddWithValue("@productId", selectedId);
                                MySqlDataReader dr = com.ExecuteReader();

                                if (dr.Read())
                                {
                                    discounts.Add(dataGridView1.RowCount, dr.GetDouble("discount"));

                                    txtDiscount.Text = (Convert.ToDouble(txtDiscount.Text) + dr.GetDouble("discount")).ToString();

                                    dataGridView1.Rows.Add(dr.GetInt32("productID"), dr.GetString("productName"), nucQuantity.Value, dr.GetDouble("price"), dr.GetDouble("price") * Convert.ToDouble(nucQuantity.Value));
                                }
                            }

                            txtIdSearch.Text = "";
                            txtNameSearch.Text = "";
                            nucQuantity.Value = 1;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error: {ex.Message}", "Error");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please enter a valid quantity");
                }
            }
            else
            {
                MessageBox.Show("Please select a item");
            }
        }

        private void dataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            calculateTotal();
        }

        private void dataGridView1_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            calculateTotal();
        }

        private void calculateTotal()
        {
            total = 0;

            // Iterate through the rows of the DataGridView
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {

                // Retrieve the value from the specified column
                if (row.Cells["Price"].Value != null)
                {
                    total += Convert.ToDouble(row.Cells["Price"].Value);
                }
            }

            txtTotal.Text = total.ToString();
            txtNetTotal.Text = (total - Convert.ToDouble(txtDiscount.Text)).ToString();
        }

        private void txtCash_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtChange.Text = (Convert.ToDouble(txtCash.Text) - Convert.ToDouble(txtNetTotal.Text)).ToString();
                btnBill.Enabled = true;
            }
        }

        private void btnBill_Click(object sender, EventArgs e)
        {
            using (MySqlConnection connection = Connector.getConnection())
            {
                try
                {
                    connection.Open();

                    string sql = "INSERT INTO bill (time, date, billAmount, discount, store_id) VALUES(@time, @date, @billAmount, @discount, @storeId)";

                    using (MySqlCommand com = new MySqlCommand(sql, connection))
                    {
                        com.Parameters.AddWithValue("@time", DateTime.Now.TimeOfDay);
                        com.Parameters.AddWithValue("@date", DateTime.Now.Date);
                        com.Parameters.AddWithValue("@billAmount", total);
                        com.Parameters.AddWithValue("@discount", Convert.ToDecimal(txtDiscount.Text));
                        com.Parameters.AddWithValue("@storeId", 6);
                        com.ExecuteNonQuery();
                    }

                    int billId = 0;

                    string sql1 = "SELECT MAX(bill_ID) FROM bill";

                    using (MySqlCommand com = new MySqlCommand(sql1, connection))
                    {
                        MySqlDataReader dr = com.ExecuteReader();

                        if (dr.Read())
                        {
                            billId = dr.GetInt32(0);
                        }

                        dr.Close();
                    }

                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        string sql2 = "INSERT INTO product_bill (productID, bill_ID, quantity) VALUES(@productId, @billId, @quantity)";

                        using (MySqlCommand com = new MySqlCommand(sql2, connection))
                        {
                            com.Parameters.AddWithValue("@productId", Convert.ToInt32(row.Cells["ID"].Value));
                            com.Parameters.AddWithValue("@billId", billId);
                            com.Parameters.AddWithValue("@quantity", Convert.ToInt32(row.Cells["Quantity"].Value));
                            com.ExecuteNonQuery();
                        }
                    }

                    ReportDocument report = new ReportDocument();
                    report.Load(@"D:\ClothingStoreSystem\InstoreSystem\InstoreSystem\Reports\BillReciept.rpt");

                    // Set up the connection to the ODBC data source
                    ConnectionInfo connectionInfo = new ConnectionInfo
                    {
                        ServerName = "MYSQLDB", // Name of the ODBC Data Source
                        UserID = "root", // Your MySQL username
                        Password = "" // Your MySQL password
                    };

                    // Apply the connection settings to each table in the report
                    foreach (Table table in report.Database.Tables)
                    {
                        TableLogOnInfo logOnInfo = table.LogOnInfo;
                        logOnInfo.ConnectionInfo = connectionInfo;
                        table.ApplyLogOnInfo(logOnInfo);
                    }

                    report.SetParameterValue("billId", billId);
                    report.SetParameterValue("storeAddress", "No. 24, Palawaththa, Baththaramulla");
                    report.SetParameterValue("cash", Convert.ToDouble(txtCash.Text));

                    Reports rpt = new Reports(report);
                    rpt.ShowDialog();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error");
                }
            }
        }
    }
}
