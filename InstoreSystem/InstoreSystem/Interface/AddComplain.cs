using FinalTest.Model;
using InstoreSystem.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace InstoreSystem.Interface
{
    public partial class AddComplain : Form
    {
        public AddComplain()
        {
            InitializeComponent();
            AutoGenerateComplainID();
        }
        // Auto-generate the next ComplainID and display it in the textbox
        private void AutoGenerateComplainID()
        {
            string query = "SELECT MAX(complainID) + 1 AS NextComplainID FROM complain";

            using (MySqlConnection connection = Connector.getConnection())
            {
                try
                {
                    connection.Open();
                    MySqlCommand com = new MySqlCommand(query, connection);
                    object result = com.ExecuteScalar();

                    if (result != DBNull.Value && result != null)
                    {
                        txtComplainID.Text = result.ToString();
                    }
                    else
                    {
                        txtComplainID.Text = "1";
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error generating Complain ID: {ex.Message}", "Error");
                }
            }
        }

        // Validate form inputs
        private bool ValidateForm()
        {
            if (string.IsNullOrEmpty(txtDescription.Text))
            {
                MessageBox.Show("Description cannot be empty.", "Validation Error");
                return false;
            }

            if (!int.TryParse(txtEmployeeID.Text, out int employeeId) || employeeId <= 0)
            {
                MessageBox.Show("Please enter a valid Employee ID.", "Validation Error");
                return false;
            }

            return true;
        }


        private void btnSubmit_Click(object sender, EventArgs e)
        {
            // Validate inputs 
            if (!ValidateForm()) return;

            try
            {
                int complainID = int.Parse(txtComplainID.Text);
                DateTime date = dtpDate.Value;
                string description = txtDescription.Text;
                int employeeID = int.Parse(txtEmployeeID.Text);

                // Check if the employeeID exists in the employee table
                if (!IsEmployeeIDValid(employeeID))
                {
                    MessageBox.Show("Invalid Employee ID. Please enter a valid ID.", "Error");
                    return;
                }

                // Create a Complain object and add it to the database
                Complain newComplain = new Complain(complainID, date, description, employeeID);
                newComplain.addComplain();

               
                ClearForm();
                AutoGenerateComplainID();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error");
            }
        }

        private bool IsEmployeeIDValid(int employeeID)
        {
            // Query the database to check if employeeID exists
            string query = "SELECT COUNT(*) FROM employee WHERE employeeID = @employeeID";

            using (MySqlConnection connection = Connector.getConnection())
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@employeeID", employeeID);

                connection.Open();

                // Use Convert.ToInt32 to handle null values safely
                int count = Convert.ToInt32(cmd.ExecuteScalar() ?? 0);

                connection.Close();

                return count > 0; // If count is greater than 0, the employeeID is valid
            }
        }
        private void ClearForm()
        {

            dtpDate.Value = DateTime.Now;
            txtDescription.Clear();
            txtEmployeeID.Clear();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void AddComplain_Load(object sender, EventArgs e)
        {

        }
    }
}
