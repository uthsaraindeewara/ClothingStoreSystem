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
    public partial class ForgetPassword : Form
    {
        public ForgetPassword()
        {
            InitializeComponent();
        }

        // Validate form inputs
        private bool ValidateForm()
        {
            if (string.IsNullOrEmpty(txtEmployeeID.Text.Trim()))
            {
                MessageBox.Show("Employee ID cannot be empty.", "Validation Error");
                return false;
            }

            if (string.IsNullOrEmpty(txtPassword.Text.Trim()) || string.IsNullOrEmpty(txtRetypePW.Text.Trim()))
            {
                MessageBox.Show("Passwords cannot be empty.", "Validation Error");
                return false;
            }

            if (txtPassword.Text.Trim() != txtRetypePW.Text.Trim())
            {
                MessageBox.Show("Passwords do not match. Please retype.", "Validation Error");
                return false;
            }

            return true;
        }

        // Check if Employee ID exists in the database
        private bool IsRegisteredUser(string employeeId)
        {
            string query = "SELECT COUNT(*) FROM login_info WHERE employeeID = @employeeID";

            using (MySqlConnection connection = Connector.getConnection())
            {
                try
                {
                    connection.Open();
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@employeeID", employeeId);

                    object result = cmd.ExecuteScalar();
                    if (result != null && Convert.ToInt32(result) > 0)
                    {
                        return true; // User exists
                    }
                    else
                    {
                        return false; 
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error");
                    return false;
                }
            }
        }

        // Change the password in the login_info table
        private void ChangePassword(string employeeId, string newPassword)
        {
            string query = "UPDATE login_info SET password = @newPassword WHERE employeeID = @employeeID";

            using (MySqlConnection connection = Connector.getConnection())
            {
                try
                {
                    connection.Open();
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@employeeID", employeeId);
                    cmd.Parameters.AddWithValue("@newPassword", newPassword);

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("Password changed successfully!", "Success");
                        ClearForm();
                    }
                    else
                    {
                        MessageBox.Show("Failed to change password. Please try again.", "Error");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error");
                }
            }
        }

        private void btnChangePW_Click(object sender, EventArgs e)
        {
            
            if (!ValidateForm()) return;

            string employeeId = txtEmployeeID.Text.Trim();
            string newPassword = txtPassword.Text.Trim();

            // Check if user is registered
            if (IsRegisteredUser(employeeId))
            {
                // change the password
                ChangePassword(employeeId, newPassword);
            }
            else
            {
                MessageBox.Show("Employee ID not found. Please register first.", "User Not Found");
            }
        }
        private void ClearForm()
        {
            txtEmployeeID.Clear();
            txtPassword.Clear();
            txtRetypePW.Clear();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void linkLabelLogin_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Close();
        }

        private void ForgetPassword_Load(object sender, EventArgs e)
        {

        }
    }
}
