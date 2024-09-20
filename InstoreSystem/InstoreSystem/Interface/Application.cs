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
    public partial class Application : Form
    {
        Dictionary<string, UserControl> panels = new Dictionary<string, UserControl>();
        Dictionary<String, Button> buttons = new Dictionary<String, Button>();

        public Application()
        {
            InitializeComponent();
        }

        private void Application_Load(object sender, EventArgs e)
        {
            HomePanel home = new HomePanel();
            this.Controls.Add(home);
            panels.Add("Home", home);

            InventoryPanel inventory = new InventoryPanel();
            this.Controls.Add(inventory);
            panels.Add("Inventory", inventory);

            EmployeesPanel employees = new EmployeesPanel();
            this.Controls.Add(employees);
            panels.Add("Employees", employees);

            CashierPanel cashier = new CashierPanel();
            this.Controls.Add(cashier);
            panels.Add("Cashier", cashier);

            FinancePanel finance = new FinancePanel();
            this.Controls.Add(finance);
            panels.Add("Finance", finance);

            OrdersPanel orders = new OrdersPanel();
            this.Controls.Add(orders);
            panels.Add("Orders", orders);

            buttons.Add("Home", btnHome);
            buttons.Add("Inventory", btnInventory);
            buttons.Add("Employees", btnEmployees);
            buttons.Add("Cashier", btnCashier);
            buttons.Add("Finance", btnFinance);
            buttons.Add("Orders", btnOrders);
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            panelChange("Home");
        }

        private void btnInventory_Click(object sender, EventArgs e)
        {
            panelChange("Inventory");
        }

        private void btnEmployees_Click(object sender, EventArgs e)
        {
            panelChange("Employees");
        }

        private void btnFinance_Click(object sender, EventArgs e)
        {
            panelChange("Finance");
        }

        private void btnCashier_Click(object sender, EventArgs e)
        {
            panelChange("Cashier");
        }

        private void btnOrders_Click(object sender, EventArgs e)
        {
            panelChange("Orders");
        }

        private void panelChange(string currentPanel)
        {
            foreach (KeyValuePair<string, UserControl> kvp in panels)
            {
                if (kvp.Key == currentPanel)
                {
                    kvp.Value.Visible = true;
                    buttons[kvp.Key].BackColor = Color.FromArgb(140, 140, 140);
                }
                else
                {
                    kvp.Value.Visible = false;
                    buttons[kvp.Key].BackColor = Color.FromArgb(43, 43, 43);
                }
            }
        }
    }
}
