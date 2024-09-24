using FinalTest.Model;
using InstoreSystem.Model;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InstoreSystem.Interface
{
    public partial class AddStock : Form
    {
        private Stock selectedStock;
        public AddStock()
        {
            InitializeComponent();
            AutoGenerateStockID();
            LoadStockData();
        }

        private void AutoGenerateStockID()
        {
            string query = "SELECT MAX(stockID) + 1 AS NextStockID FROM stock";

            using (MySqlConnection connection = Connector.getConnection())
            {
                try
                {
                    connection.Open();
                    MySqlCommand com = new MySqlCommand(query, connection);
                    object result = com.ExecuteScalar();

                    if (result != DBNull.Value && result != null)
                    {
                        txtStockID.Text = result.ToString();
                    }
                    else
                    {
                        txtStockID.Text = "1";
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error generating Stock ID: {ex.Message}", "Error");
                }
            }
        }
        private void LoadStockData()
        {
            string query = "SELECT stockID, cost, quantity, supplierID FROM stock";
            using (MySqlConnection connection = Connector.getConnection())
            {
                try
                {
                    connection.Open();
                    MySqlCommand com = new MySqlCommand(query, connection);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(com);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading schedule data: {ex.Message}", "Error");
                }
            }
        }

        // Validate form inputs
        private bool ValidateForm()
        {
            if (string.IsNullOrEmpty(txtCost.Text) || !decimal.TryParse(txtCost.Text, out _))
            {
                MessageBox.Show("Please enter a valid cost.", "Validation Error");
                return false;
            }

            if (string.IsNullOrEmpty(txtQuantity.Text) || !int.TryParse(txtQuantity.Text, out _))
            {
                MessageBox.Show("Please enter a valid quantity.", "Validation Error");
                return false;
            }

            if (string.IsNullOrEmpty(cmbSupplierID.Text) || !int.TryParse(cmbSupplierID.Text, out _))
            {
                MessageBox.Show("Please enter a valid Supplier ID.", "Validation Error");
                return false;
            }

            return true;
        }


        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Validate inputs 
            if (!ValidateForm()) return;

            try
            {
                int stockID = int.Parse(txtStockID.Text);
                decimal cost = decimal.Parse(txtCost.Text);
                int quantity = int.Parse(txtQuantity.Text);
                int supplierID = int.Parse(cmbSupplierID.Text);

                // Create a Stock object and add it to the database
                Stock newStock = new Stock(stockID, cost, quantity, supplierID);
                newStock.addStock();

                ClearForm();
                AutoGenerateStockID();
                LoadStockData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error");
            }

        }
        private void ClearForm()
        {
            txtCost.Clear();
            txtQuantity.Clear();
            AutoGenerateStockID();
            btnAdd.Enabled = true;
          
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedStock!= null && ValidateForm())
            {
                selectedStock.stockId = Convert.ToInt32(txtStockID.Text);
                selectedStock.cost = Convert.ToDecimal(txtCost.Text);
                selectedStock.quantity = Convert.ToInt32(txtQuantity.Text);
                selectedStock.supplierId = Convert.ToInt32(cmbSupplierID.Text);

                selectedStock.updateStock();
                LoadStockData();
                ClearForm();
                AutoGenerateStockID();
                btnAdd.Enabled = true;
              
            }

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Check if the clicked row index is valid
            if (e.RowIndex >= 0)
            {
                // Get the selected row
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                // Populate the fields with data from the selected row
                int stockID = Convert.ToInt32(row.Cells["stockID"].Value);
                selectedStock = new Stock(stockID);  // Load the stock from the database

                // Populate form fields
                txtStockID.Text = stockID.ToString();
                txtCost.Text = selectedStock.cost.ToString();
                txtQuantity.Text = selectedStock.quantity.ToString();
                cmbSupplierID.Text = selectedStock.supplierId.ToString();

                btnAdd.Enabled = false;

            }
        }

        // Method to load supplier IDs into the combo box
        private void LoadSupplierIDs()
        {
            string query = "SELECT supplierID FROM supplier";

            using (MySqlConnection connection = Connector.getConnection())
            {
                try
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand(query, connection);
                    MySqlDataReader reader = command.ExecuteReader();

                    // Clear existing items in the combo box
                    cmbSupplierID.Items.Clear();

                    // Add each supplierID to the combo box
                    while (reader.Read())
                    {
                        cmbSupplierID.Items.Add(reader["supplierID"].ToString());
                    }

                    // Optionally, set the combo box to show the first item by default
                    if (cmbSupplierID.Items.Count > 0)
                    {
                        cmbSupplierID.SelectedIndex = 0;  // Select the first supplier ID
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading supplier IDs: {ex.Message}", "Error");
                }
            }
        }


        private void AddStock_Load(object sender, EventArgs e)
        {
            LoadSupplierIDs();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (selectedStock != null)
            {

                selectedStock.deleteStock();
                LoadStockData();
                ClearForm();
                AutoGenerateStockID();
                btnAdd.Enabled = true;
            }

        }
    }
}
