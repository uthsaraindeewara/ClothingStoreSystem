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
                MessageBox.Show("Employee added successfully");
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

            return true;
        }
    }
}
