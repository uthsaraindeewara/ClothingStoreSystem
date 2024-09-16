using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
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
        string cs = @"Server=localhost; Port=3307; Database=studentsdb; User Id=root;";
        public AddEmployees()
        {
            InitializeComponent();
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

                    Employee emp = new Employee(Convert.ToInt32(txtId.Text), txtName.Text, txtAddress.Text, txtContactNo.Text, dtpDob.Value, gender, Convert.ToDouble(txtSalary.Text), storeId);
                    emp.addEmployee(1);

                    if (cmbPosition.Text == "Manager")
                    {
                        Manager man = new Manager(Convert.ToInt32(txtId.Text), cmbManagerRole.Text, txtManagerEmail.Text, txtManagerQualification.Text);
                        man.addManager();
                    }
                    else if (cmbPosition.Text == "Accountant")
                    {
                        Accountant acc = new Accountant(Convert.ToInt32(txtId.Text), txtAccountantEmail.Text, txtAccountantQualification.Text);
                        acc.addAccountant();
                    }
                    else if (cmbPosition.Text == "Sales Associate")
                    {
                        SalesAssociate sa = new SalesAssociate(Convert.ToInt32(txtId.Text), txtSalesAssociateExperience.Text, cmbSalesAssociateType.Text);
                        sa.addSalesAssociate();
                    }
                    else if (cmbPosition.Text == "Cashier")
                    {
                        Cashier cash = new Cashier(Convert.ToInt32(txtId.Text), Convert.ToInt32(txtCashierCashRegisterNo.Text), txtCashierExperience.Text);
                        cash.addCashier();
                    }
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
    }
}
