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
using static System.Net.Mime.MediaTypeNames;

namespace InstoreSystem.Interface
{
    public partial class Login : Form
    {
        private Application parentForm;

        public Login()
        {
            InitializeComponent();
        }

        public Login(Application parentForm)
        {
            InitializeComponent();

            this.parentForm = parentForm;
        }

        // Validate form inputs
        private bool ValidateForm()
        {
            if (string.IsNullOrEmpty(txtEmployeeID.Text.Trim()))
            {
                MessageBox.Show("Employee ID cannot be empty.", "Validation Error");
                return false;
            }
            if (string.IsNullOrEmpty(txtUsername.Text.Trim()))
            {
                MessageBox.Show("Username cannot be empty.", "Validation Error");
                return false;
            }

            if (string.IsNullOrEmpty(txtPassword.Text.Trim()))
            {
                MessageBox.Show("Password cannot be empty.", "Validation Error");
                return false;
            }
           

            return true;
        }

        // Method to check employee credentials and determine employee type
        private string GetEmployeeType(string employeeID, string username,string password)
        {
            string employeeType = "";

            string query = "SELECT * FROM login_info WHERE employeeID = @employeeID AND password = @password AND username=@username";

            using (MySqlConnection connection = Connector.getConnection())
            {
                try
                {
                    connection.Open();
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@employeeID", employeeID);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", password);
                    

                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        reader.Close();

                      
                        if (IsEmployeeInTable(employeeID, "manager", "managerID"))
                        {
                            employeeType = "manager";
                        }
                        else if (IsEmployeeInTable(employeeID, "accountant", "accountantID"))
                        {
                            employeeType = "accountant";
                        }
                        else if (IsEmployeeInTable(employeeID, "cashier", "cashierID"))
                        {
                            employeeType = "cashier";
                        }
                        else if (IsEmployeeInTable(employeeID, "salesassociate", "salesAssocID"))
                        {
                            employeeType = "sales_associate";
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid Employee ID or Password.", "Login Failed");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error");
                }
            }

            return employeeType;
        }

        //method to check if employee exists in a specific role table
        private bool IsEmployeeInTable(string employeeID, string tableName, string columnID)
        {
            string query = $"SELECT COUNT(*) FROM {tableName} WHERE {columnID} = @employeeID";

            using (MySqlConnection connection = Connector.getConnection())
            {
                try
                {
                    connection.Open();

                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@employeeID", employeeID);

                    object result = cmd.ExecuteScalar();
                    return Convert.ToInt32(result) > 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error checking {tableName}: {ex.Message}", "Error");
                    return false;
                }
            }
        }

        private void linkLabelLogin_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ForgetPassword fp=new ForgetPassword();
            fp.ShowDialog();
        }

        private void linkLabelRegister_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Register rg=new Register();
            rg.ShowDialog();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            this.txtEmployeeID.Clear();
            this.txtPassword.Clear();
            this.txtUsername.Clear();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (!ValidateForm()) return;

            string employeeID = txtEmployeeID.Text.Trim();
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            
            string employeeType = GetEmployeeType(employeeID,username, password);


            if (!string.IsNullOrEmpty(employeeType))
            {
                // Set access limitations based on employee type
                if (employeeType == "manager")
                {
                    MessageBox.Show("Welcome, Manager!");
                    parentForm.userId = Convert.ToInt32(employeeID);
                    parentForm.position = "Manager";

                    /*
                    ManagerPanel.Visible = true;
                    AccountantPanel.Visible = false;
                    CashierPanel.Visible = false;
                    SalesPanel.Visible = false;*/
                }
                else if (employeeType == "accountant")
                {
                    MessageBox.Show("Welcome, Accountant!");
                    parentForm.userId = Convert.ToInt32(employeeID);
                    parentForm.position = "Accountant";

                    /*
                    ManagerPanel.Visible = false;
                    AccountantPanel.Visible = true;
                    CashierPanel.Visible = false;
                    SalesPanel.Visible = false;*/
                }
                else if (employeeType == "cashier")
                {
                    MessageBox.Show("Welcome, Cashier!");
                    parentForm.userId = Convert.ToInt32(employeeID);
                    parentForm.position = "Cashier";

                    /*
                    ManagerPanel.Visible = false;
                    AccountantPanel.Visible = false;
                    CashierPanel.Visible = true;
                    SalesPanel.Visible = false;*/
                }
                else if (employeeType == "sales_associate")
                {
                    MessageBox.Show("Welcome, Sales Associate!");
                    parentForm.userId = Convert.ToInt32(employeeID);
                    parentForm.position = "Sales Associate";

                    /*
                    ManagerPanel.Visible = false;
                    AccountantPanel.Visible = false;
                    CashierPanel.Visible = false;
                    SalesPanel.Visible = true;*/
                }

                this.Dispose();
            }
         
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }

        private void Login_Load_1(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }
    }
}
