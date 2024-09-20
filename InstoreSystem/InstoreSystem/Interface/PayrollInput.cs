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
    public partial class PayrollInput : Form
    {
        public Decimal value;

        public PayrollInput(string type)
        {
            InitializeComponent();

            lblType.Text = type;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtValue.Text))
            {
                MessageBox.Show("The input field cannot be empty.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!decimal.TryParse(txtValue.Text, out _))
            {
                MessageBox.Show("Please enter a valid number.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            value = Convert.ToDecimal(txtValue.Text);

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
