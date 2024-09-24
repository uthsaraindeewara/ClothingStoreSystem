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
    public partial class ManageSupplier : Form
    {
        private Supplier selectedSupplier;
        public ManageSupplier()
        {
            InitializeComponent();
            AutoGenerateSupplierID();
            LoadSupplierData();
        }

        private void AutoGenerateSupplierID()
        {
            string query = "SELECT MAX(supplierID) + 1 AS NextSupplierID FROM supplier";

            using (MySqlConnection connection = Connector.getConnection())
            {
                try
                {
                    connection.Open();
                    MySqlCommand com = new MySqlCommand(query, connection);
                    object result = com.ExecuteScalar();

                    if (result != DBNull.Value && result != null)
                    {
                        txtSupplierID.Text = result.ToString();
                    }
                    else
                    {
                        txtSupplierID.Text = "1";
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error generating Supplier ID: {ex.Message}", "Error");
                }
            }
        }

        private void LoadSupplierData()
        {
            string query = "SELECT supplierID, address, email FROM supplier";
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
                    MessageBox.Show($"Error loading supplier data: {ex.Message}", "Error");
                }
            }
        } 
        // Validate form inputs
        private bool ValidateForm()
        {
            if (string.IsNullOrEmpty(txtAddress.Text))
            {
                MessageBox.Show("Address can not be empty.", "Validation Error");
                return false;
            }

            if (string.IsNullOrEmpty(txtEmail.Text))
            {
                MessageBox.Show("Email can not be empty.", "Validation Error");
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
                int supplierID = int.Parse(txtSupplierID.Text);
                String address = txtAddress.Text;
                String email = txtEmail.Text;        

                // Create a Stock object and add it to the database
                Supplier newSupplier = new Supplier(supplierID, address,email);
                newSupplier.addSupplier();

                ClearForm();
                AutoGenerateSupplierID();
                LoadSupplierData(); 
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error");
            }


        }
        private void ClearForm()
        {
            txtAddress.Clear();
            txtEmail.Clear();
            
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
            AutoGenerateSupplierID();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Check if the clicked row index is valid
            if (e.RowIndex >= 0)
            {
                // Get the selected row
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                // Populate the fields with data from the selected row
                int supplierID = Convert.ToInt32(row.Cells["supplierID"].Value);
                selectedSupplier = new Supplier(supplierID); 

                // Populate form fields
                txtSupplierID.Text = supplierID.ToString();
                txtAddress.Text = selectedSupplier.address;
                txtEmail.Text = selectedSupplier.email;
                


                
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedSupplier != null && ValidateForm())
            {
                selectedSupplier.supplierId = Convert.ToInt32(txtSupplierID.Text);
                selectedSupplier.address = txtAddress.Text;
                selectedSupplier.email = txtEmail.Text;

                selectedSupplier.updateSupplier();
                LoadSupplierData();
                ClearForm();
                AutoGenerateSupplierID();
                
            }

        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (selectedSupplier != null)
            {

                selectedSupplier.deleteSupplier();
                LoadSupplierData();
                ClearForm();
                AutoGenerateSupplierID();
                
            }
        }

        private void ManageSupplier_Load(object sender, EventArgs e)
        {
        }
    }
}
