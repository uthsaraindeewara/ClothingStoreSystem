using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using InstoreSystem.Model;
using K4os.Hash.xxHash;
using MySql.Data.MySqlClient;

namespace InstoreSystem.Employees
{
    public partial class AddEmployees : Form
    {
        int employeeId = 0;
        string position = "";
        
        // Constructor called when the form is created to add a new employee
        public AddEmployees()
        {
            InitializeComponent();

            setNextId();
        }

        public AddEmployees(int employeeId)
        {
            InitializeComponent();

            this.employeeId = employeeId;
            changeToUpdate();
            getPosition();
            loadEmployeeDetails();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (ValidateInputs())
            {
                string gender = "";
                int storeId = 0;

                // Get the store id for the selected store
                using (MySqlConnection con = Connector.getConnection())
                {
                    con.Open();

                    string query = "SELECT storeID FROM store WHERE storeName = @storeName";

                    using (MySqlCommand command = new MySqlCommand(query, con))
                    {
                        command.Parameters.AddWithValue("@storeName", cmbStore.Text);

                        MySqlDataReader dr = command.ExecuteReader();

                        if (dr.Read())
                        {
                            storeId = Convert.ToInt32(dr["storeID"]);
                        }
                    }

                    if (rbMale.Checked)
                    {
                        gender = "Male";
                    }
                    else
                    {

                        gender = "Female";
                    }

                    if (cmbPosition.Text == "Manager")
                    {
                        Manager man = new Manager(txtName.Text, txtAddress.Text, txtContactNo.Text, dtpDob.Value, gender, Convert.ToDouble(txtSalary.Text), storeId, cmbManagerRole.Text, txtManagerEmail.Text, txtManagerQualification.Text);
                        man.addManager(1);
                    }
                    else if (cmbPosition.Text == "Accountant")
                    {
                        Accountant acc = new Accountant(txtName.Text, txtAddress.Text, txtContactNo.Text, dtpDob.Value, gender, Convert.ToDouble(txtSalary.Text), storeId, txtAccountantEmail.Text, txtAccountantQualification.Text);
                        acc.addAccountant(1);
                    }
                    else if (cmbPosition.Text == "Sales Associate")
                    {
                        SalesAssociate sa = new SalesAssociate(txtName.Text, txtAddress.Text, txtContactNo.Text, dtpDob.Value, gender, Convert.ToDouble(txtSalary.Text), storeId, txtSalesAssociateExperience.Text, cmbSalesAssociateType.Text);
                        sa.addSalesAssociate(1);
                    }
                    else if (cmbPosition.Text == "Cashier")
                    {
                        Cashier cash = new Cashier(txtName.Text, txtAddress.Text, txtContactNo.Text, dtpDob.Value, gender, Convert.ToDouble(txtSalary.Text), storeId, Convert.ToInt32(txtCashierCashRegisterNo.Text), txtCashierExperience.Text);
                        cash.addCashier(1);
                    }

                    clearFields();
                }
            }
        }


        private bool ValidateInputs()
        {
            // Check if all text fields are not empty
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Please enter a name.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtAddress.Text))
            {
                MessageBox.Show("Please enter an address.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtContactNo.Text))
            {
                MessageBox.Show("Please enter a contact number.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtSalary.Text))
            {
                MessageBox.Show("Please enter a salary.");
                return false;
            }

            // Check if a store is selected
            if (string.IsNullOrWhiteSpace(cmbStore.Text))
            {
                MessageBox.Show("Please select a store.");
                return false;
            }

            // Check if one radio button is selected
            if (!rbFemale.Checked && !rbMale.Checked)
            {
                MessageBox.Show("Please select a gender.");
                return false;
            }

            // Check if a date is selected
            if (dtpDob.Value == DateTimePicker.MinimumDateTime)
            {
                MessageBox.Show("Please select a valid date of birth.");
                return false;
            }

            // Validate txtContactNo contains exactly 10 digits
            if (txtContactNo.Text.Length != 10 || !long.TryParse(txtContactNo.Text, out _))
            {
                MessageBox.Show("Contact number must contain exactly 10 digits.");
                return false;
            }

            // Validate txtSalary is an integer
            if (!int.TryParse(txtSalary.Text, out _))
            {
                MessageBox.Show("Salary must be a valid number.");
                return false;
            }

            if (string.IsNullOrEmpty(cmbPosition.Text))
            {
                MessageBox.Show("Please select a position.");
                return false;
            }

            if (cmbPosition.Text == "Manager")
            {
                if (string.IsNullOrEmpty(cmbManagerRole.Text))
                {
                    MessageBox.Show("Please select a manager role");
                    return false;
                }
                if (string.IsNullOrEmpty(txtManagerEmail.Text))
                {
                    MessageBox.Show("Please enter manager email");
                    return false;
                }
            }
            else if (cmbPosition.Text == "Accountant")
            {
                if (string.IsNullOrEmpty(txtAccountantEmail.Text))
                {
                    MessageBox.Show("Please enter accountant email");
                    return false;
                }
            }
            else if (cmbPosition.Text == "Sales Associate")
            {
                if (string.IsNullOrEmpty(cmbSalesAssociateType.Text))
                {
                    MessageBox.Show("Please select a sales associate type");
                    return false;
                }
            }
            else if (cmbPosition.Text == "Cashier")
            {
                if (!int.TryParse(txtCashierCashRegisterNo.Text, out _))
                {
                    MessageBox.Show("Please enter cash register no");
                    return false;
                }
            }

            return true;
        }

        private void cmbPosition_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbPosition.Text == "Manager")
            {
                pnlManagerDetails.Visible = true;
                pnlAccountantDetails.Visible = false;
                pnlSalesAssociateDetails.Visible = false;
                pnlCashierDetails.Visible = false;
            }
            else if (cmbPosition.Text == "Accountant")
            {
                pnlManagerDetails.Visible = false;
                pnlAccountantDetails.Visible = true;
                pnlSalesAssociateDetails.Visible = false;
                pnlCashierDetails.Visible = false;
            }
            else if (cmbPosition.Text == "Sales Associate")
            {
                pnlManagerDetails.Visible = false;
                pnlAccountantDetails.Visible = false;
                pnlSalesAssociateDetails.Visible = true;
                pnlCashierDetails.Visible = false;
            }
            else if (cmbPosition.Text == "Cashier")
            {
                pnlManagerDetails.Visible = false;
                pnlAccountantDetails.Visible = false;
                pnlSalesAssociateDetails.Visible = false;
                pnlCashierDetails.Visible = true;
            }
        }

        private void clearFields()
        {
            txtName.Text = "";
            dtpDob.Value = DateTime.Now;
            txtAddress.Text = "";
            txtContactNo.Text = "";
            rbMale.Checked = false;
            rbFemale.Checked = false;
            cmbStore.Text = "";
            txtSalary.Text = "";
            cmbPosition.Text = "";
            cmbManagerRole.Text = "";
            txtManagerEmail.Text = "";
            txtManagerQualification.Text = "";
            txtAccountantEmail.Text = "";
            txtAccountantQualification.Text = "";
            cmbSalesAssociateType.Text = "";
            txtSalesAssociateExperience.Text = "";
            txtCashierCashRegisterNo.Text = "";
            txtCashierExperience.Text = "";
        }

        private void getPosition()
        {
            string query = @"SELECT 
                                managerID, 'Manager' AS position 
                             FROM manager 
                             WHERE managerID = @employeeId 
                             UNION 
                             SELECT accountantID, 'Accountant' AS position 
                             FROM accountant 
                             WHERE accountantID = @employeeId 
                             UNION 
                             SELECT salesAssocID, 'Sales Associate' AS position 
                             FROM salesassociate 
                             WHERE salesAssocID = @employeeId 
                             UNION 
                             SELECT cashierID, 'Cashier' AS position 
                             FROM cashier 
                             WHERE cashierID = @employeeId";

            using (MySqlConnection connection = Connector.getConnection())
            {
                try
                {
                    connection.Open();
                    MySqlCommand com = new MySqlCommand(query, connection);

                    com.Parameters.AddWithValue("@employeeId", employeeId);
                    MySqlDataReader dr = com.ExecuteReader();

                    if (dr.Read())
                    {
                        position = dr.GetString("position");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error");
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (Validate())
            {
                int storeId = 0;

                using (MySqlConnection con = Connector.getConnection())
                {
                    con.Open();

                    string query = "SELECT storeID FROM store WHERE storeName = @storeName";

                    using (MySqlCommand command = new MySqlCommand(query, con))
                    {
                        command.Parameters.AddWithValue("@storeName", cmbStore.Text);

                        MySqlDataReader dr = command.ExecuteReader();

                        if (dr.Read())
                        {
                            storeId = Convert.ToInt32(dr["storeID"]);
                        }
                    }
                }

                if (position == "Manager")
                {
                    Manager emp = new Manager(employeeId);
                    emp.setName(txtName.Text);
                    emp.setContactNo(txtContactNo.Text);
                    emp.setAddress(txtAddress.Text);
                    emp.setdOB(dtpDob.Value);

                    if (rbMale.Checked)
                    {
                        emp.setGender("Male");
                    }
                    else if (rbFemale.Checked)
                    {
                        emp.setGender("Female");
                    }

                    emp.setSalary(Convert.ToDouble(txtSalary.Text));
                    emp.setStoreId(storeId);
                    emp.setRole(cmbManagerRole.Text);
                    emp.setEmail(txtManagerEmail.Text);
                    emp.setQualification(txtManagerQualification.Text);

                    emp.updateManager();
                }
                else if (position == "Accountant")
                {
                    Accountant emp = new Accountant(employeeId);
                    emp.setName(txtName.Text);
                    emp.setContactNo(txtContactNo.Text);
                    emp.setAddress(txtAddress.Text);
                    emp.setdOB(dtpDob.Value);

                    if (rbMale.Checked)
                    {
                        emp.setGender("Male");
                    }
                    else if (rbFemale.Checked)
                    {
                        emp.setGender("Female");
                    }

                    emp.setSalary(Convert.ToDouble(txtSalary.Text));
                    emp.setStoreId(storeId);
                    emp.setEmail(txtAccountantEmail.Text);
                    emp.setQualification(txtAccountantQualification.Text);

                    emp.updateAccountant();
                }
                else if (position == "Sales Associate")
                {
                    SalesAssociate emp = new SalesAssociate(employeeId);
                    emp.setName(txtName.Text);
                    emp.setContactNo(txtContactNo.Text);
                    emp.setAddress(txtAddress.Text);
                    emp.setdOB(dtpDob.Value);

                    if (rbMale.Checked)
                    {
                        emp.setGender("Male");
                    }
                    else if (rbFemale.Checked)
                    {
                        emp.setGender("Female");
                    }

                    emp.setSalary(Convert.ToDouble(txtSalary.Text));
                    emp.setStoreId(storeId);
                    emp.setType(cmbSalesAssociateType.Text);
                    emp.setExperience(txtSalesAssociateExperience.Text);

                    emp.updateSalesAssociate();
                }
                else if (position == "Cashier")
                {
                    Cashier emp = new Cashier(employeeId);
                    emp.setName(txtName.Text);
                    emp.setContactNo(txtContactNo.Text);
                    emp.setAddress(txtAddress.Text);
                    emp.setdOB(dtpDob.Value);

                    if (rbMale.Checked)
                    {
                        emp.setGender("Male");
                    }
                    else if (rbFemale.Checked)
                    {
                        emp.setGender("Female");
                    }

                    emp.setSalary(Convert.ToDouble(txtSalary.Text));
                    emp.setStoreId(storeId);
                    emp.setCashRegisterId(Convert.ToInt32(txtCashierCashRegisterNo.Text));
                    emp.setExperience(txtCashierExperience.Text);

                    emp.updateCashier();
                }

                this.Dispose();
            }
        }

        private void loadEmployeeDetails()
        {
            Employee emp = new Employee(employeeId);
            Dictionary<string, string> employeeDetails = emp.getEmployeeDetails();

            txtId.Text = employeeDetails["employeeId"];
            txtName.Text = employeeDetails["employeeName"];
            txtContactNo.Text = employeeDetails["contactNo"];
            txtAddress.Text = employeeDetails["address"];
            dtpDob.Value = Convert.ToDateTime(employeeDetails["dOB"]);

            if (employeeDetails["gender"] == "Male")
            {
                rbMale.Checked = true;
            }
            else if (employeeDetails["gender"] == "Female")
            {
                rbFemale.Checked = true;
            }

            txtSalary.Text = employeeDetails["salary"];

            string storeName = "";

            using (MySqlConnection con = Connector.getConnection())
            {
                con.Open();

                string query = "SELECT storeName FROM store WHERE storeID = @storeId";

                using (MySqlCommand command = new MySqlCommand(query, con))
                {
                    command.Parameters.AddWithValue("@storeId", Convert.ToInt32(employeeDetails["storeId"]));

                    MySqlDataReader dr = command.ExecuteReader();

                    if (dr.Read())
                    {
                        storeName = dr.GetString("storeName");
                    }
                }
            }

            cmbStore.SelectedItem = storeName;

            if (position == "Manager")
            {
                cmbPosition.SelectedItem = position;
                cmbPosition.Enabled = false;
                Manager man = new Manager(employeeId);
                Dictionary<string, string> managerDetails = man.getManagerDetails();
                cmbManagerRole.SelectedItem = managerDetails["role"];
                txtManagerEmail.Text = managerDetails["email"];
                txtManagerQualification.Text = managerDetails["qualification"];
            }
            else if (position == "Accountant")
            {
                cmbPosition.SelectedItem = position;
                cmbPosition.Enabled = false;
                Accountant acc = new Accountant(employeeId);
                Dictionary<string, string> accountantDetails = acc.getAccountantDetails();
                txtAccountantEmail.Text = accountantDetails["email"];
                txtAccountantQualification.Text = accountantDetails["qualification"];
            }
            else if (position == "Sales Associate")
            {
                cmbPosition.SelectedItem = position;
                cmbPosition.Enabled = false;
                SalesAssociate sa = new SalesAssociate(employeeId);
                Dictionary<string, string> salesAssociateDetails = sa.getSalesAssociateDetails();
                cmbSalesAssociateType.SelectedItem = salesAssociateDetails["type"];
                txtSalesAssociateExperience.Text = salesAssociateDetails["experience"];
            }
            else if (position == "Cashier")
            {
                cmbPosition.SelectedItem = position;
                cmbPosition.Enabled = false;
                Cashier cash = new Cashier(employeeId);
                Dictionary<string, string> cashierDetails = cash.getCashierDetails();
                txtCashierCashRegisterNo.Text = cashierDetails["cashRegisterId"];
                txtCashierExperience.Text = cashierDetails["experience"];
            }
        }

        private void changeToUpdate()
        {
            Text = "Update Employee";
            btnAdd.Text = "Update";
            btnAdd.Click -= btnAdd_Click;
            btnAdd.Click += btnUpdate_Click;
        }

        private void setNextId()
        {
            using (MySqlConnection con = Connector.getConnection())
            {
                con.Open();

                string query = "SELECT MAX(employeeID) AS employeeID FROM employee";

                using (MySqlCommand command = new MySqlCommand(query, con))
                {
                    MySqlDataReader dr = command.ExecuteReader();

                    if (dr.Read())
                    {
                        if (dr.GetValue(0).ToString() == "")
                        {
                            txtId.Text = "1";
                        }
                        else
                        {
                            txtId.Text = (Convert.ToInt32(dr.GetValue(0)) + 1).ToString();
                        }
                    }
                    else
                    {
                        txtId.Text = "1";
                    }
                }
            }
        }
    }
}
