using FinalTest.Model;
using InstoreSystem.Model;
using MySql.Data.MySqlClient;
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
    public partial class AddSpending : Form
    {
        private Spending selectedSpending;
        public AddSpending()
        {
            InitializeComponent();
            AutoGenerateSpendingID();
            LoadSpendingData();
        }

        private void LoadSpendingData()
        {
            string query = "SELECT spendingID, type, description, amount, storeID, date FROM spending";
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


        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Validate inputs
            if (!ValidateInputs()) return;

            try
            {

                int spendingID = int.Parse(txtSpendingID.Text);
                string type = cmbType.Text;
                string description = txtDescription.Text;
                decimal amount = decimal.Parse(txtAmount.Text);
                int storeID = int.Parse(cmbStoreID.Text);
                DateTime date= dtpDate.Value;


                // Create a Spending object and add it to the database
                Spending newSpending = new Spending(spendingID, type, description, amount, storeID, date);
                newSpending.addSpending();

                ClearFields();
                AutoGenerateSpendingID();
                LoadSpendingData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error");
            }

        }
        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(cmbType.Text))
            {
                MessageBox.Show("Please enter a type.", "Validation Error");
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtDescription.Text))
            {
                MessageBox.Show("Description cannot be empty.", "Validation Error");
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtAmount.Text) || !double.TryParse(txtAmount.Text, out _))
            {
                MessageBox.Show("Please enter a valid amount.", "Validation Error");
                return false;
            }
            if (string.IsNullOrWhiteSpace(cmbStoreID.Text) || !int.TryParse(cmbStoreID.Text, out _))
            {
                MessageBox.Show("Please enter a valid Store ID.", "Validation Error");
                return false;
            }
            return true;
        }

        private void AutoGenerateSpendingID()
        {
            string query = "SELECT MAX(spendingID) + 1 AS NextSpendingID FROM spending";

            using (MySqlConnection connection = Connector.getConnection())
            {
                try
                {
                    connection.Open();
                    MySqlCommand com = new MySqlCommand(query, connection);
                    object result = com.ExecuteScalar();

                    if (result != DBNull.Value && result != null)
                    {
                        txtSpendingID.Text = result.ToString();
                    }
                    else
                    {
                        txtSpendingID.Text = "1";
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error generating Complain ID: {ex.Message}", "Error");
                }
            }
        }
        private void ClearFields()
        {
           
            txtDescription.Clear();
            txtAmount.Clear();
           

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
            btnAdd.Enabled = true;
            AutoGenerateSpendingID();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            // Validate inputs before attempting to update
            if (!ValidateInputs()) return;

            try
            {
                // Retrieve data from input fields
                int spendingID = int.Parse(txtSpendingID.Text);
                string type = cmbType.Text;
                string description = txtDescription.Text;
                decimal amount = decimal.Parse(txtAmount.Text);
                int storeID = int.Parse(cmbStoreID.Text);
                DateTime date = dtpDate.Value;

                // Create a Spending object and call the update method
                Spending spendingToUpdate = new Spending(spendingID, type, description, amount, storeID, date);
                spendingToUpdate.updateSpending();

                
                ClearFields();
                AutoGenerateSpendingID();
                LoadSpendingData(); 
                btnAdd.Enabled = true;

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error");
            }

        }
        // load store IDs into the combo box
        private void LoadStoreIDs()
        {
            string query = "SELECT storeID FROM store";

            using (MySqlConnection connection = Connector.getConnection())
            {
                try
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand(query, connection);
                    MySqlDataReader reader = command.ExecuteReader();

                    // Clear existing items in the combo box
                    cmbStoreID.Items.Clear();

                    // Add each storeID to the combo box
                    while (reader.Read())
                    {
                        cmbStoreID.Items.Add(reader["storeID"].ToString());
                    }

                   
                    if (cmbStoreID.Items.Count > 0)
                    {
                        cmbStoreID.SelectedIndex = 0;  // Select the first store ID
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading store IDs: {ex.Message}", "Error");
                }
            }
        }

        private void AddSpending_Load(object sender, EventArgs e)
        {
            LoadStoreIDs();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Check if the clicked row index is valid
            if (e.RowIndex >= 0)
            {
                // Get the selected row
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                // Populate the fields with data from the selected row
                int spendingID = Convert.ToInt32(row.Cells["spendingID"].Value);
                selectedSpending = new Spending(spendingID);  

                // Populate form fields
                txtSpendingID.Text = spendingID.ToString();
                cmbType.Text = selectedSpending.type;
                txtDescription.Text = selectedSpending.description;
                txtAmount.Text = selectedSpending.amount.ToString();
                cmbStoreID.Text=selectedSpending.storeId.ToString();
                dtpDate.Value = selectedSpending.date;

                btnAdd.Enabled = false;

            }
        }
    }
}
