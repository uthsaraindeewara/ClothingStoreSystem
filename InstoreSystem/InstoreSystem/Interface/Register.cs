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
    public partial class Register : Form
    {
        Login parentForm;
        public Register()
        {
            InitializeComponent();
        }

        private bool ValidateForm()
        {
            // Check if EmployeeID is not empty
            if (string.IsNullOrEmpty(txtEmployeeID.Text.Trim()))
            {
                MessageBox.Show("Employee ID cannot be empty.", "Validation Error");
                return false;
            }
            if (string.IsNullOrEmpty(txtUsername.Text.Trim()))
            {
                MessageBox.Show("Employee Username cannot be empty.", "Validation Error");
                return false;
            }

            // Check if Password is not empty and meets a minimum length
            if (string.IsNullOrEmpty(txtPassword.Text.Trim()) || txtPassword.Text.Trim().Length < 6)
            {
                MessageBox.Show("Password must be at least 6 characters long.", "Validation Error");
                return false;
            }

            
            if (txtPassword.Text.Trim() != txtRetypePW.Text.Trim())
            {
                MessageBox.Show("Passwords do not match.", "Validation Error");
                return false;
            }

            return true;  
        }

        // Check if the Employee exists in the employee table and is not already registered in login_info
        private bool IsEmployeeValid(string employeeID)
        {
            string query = "SELECT e.employeeID FROM employee e LEFT JOIN login_info li ON e.employeeID = li.employeeID WHERE e.employeeID = @employeeID AND li.employeeID IS NULL";

            using (MySqlConnection connection = Connector.getConnection())
            {
                try
                {
                    connection.Open();
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@employeeID", employeeID);
                    object result = cmd.ExecuteScalar();

                    
                    return result != null;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error validating employee: {ex.Message}", "Error");
                    return false;
                }
            }
        }


        private void Register_Load(object sender, EventArgs e)
        {

        }

        private void linkLabelLogin_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Hide();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            if (ValidateForm())  
            {
                string employeeID = txtEmployeeID.Text.Trim();
                string username= txtUsername.Text.Trim();
                string password = txtPassword.Text.Trim();


                // Check if employee exists and hasn't already registered
                if (!IsEmployeeValid(employeeID))
                {
                    MessageBox.Show("Employee ID does not exist or is already registered.", "Error");
                    return;
                }

               
                try
                {
                    string query = "INSERT INTO login_info (employeeID,username, password) VALUES (@employeeID,@username, @password)";

                    using (MySqlConnection connection = Connector.getConnection())
                    {
                        connection.Open();
                        MySqlCommand cmd = new MySqlCommand(query, connection);
                        cmd.Parameters.AddWithValue("@employeeID", employeeID);
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@password", password);

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Registration successful!", "Success");
                        ClearForm();  
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error registering: {ex.Message}", "Error");
                }
            }
        }
        private void ClearForm()
        {
            txtEmployeeID.Clear();
            txtUsername.Clear();
            txtPassword.Clear();
            txtRetypePW.Clear();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
