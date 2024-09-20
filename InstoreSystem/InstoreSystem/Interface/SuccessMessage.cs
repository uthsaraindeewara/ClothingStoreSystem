using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp10
{
    public partial class SuccessMessage : Form
    {
        private Timer closeTimer;

        public SuccessMessage(string message)
        {
            InitializeComponent();

            lblMessage.Text = message;

            this.StartPosition = FormStartPosition.CenterParent;

            closeTimer = new Timer();
            closeTimer.Interval = 3000;
            closeTimer.Tick += CloseTimer_Tick;
            closeTimer.Start();
        }

        private void CloseTimer_Tick(object sender, EventArgs e)
        {
            closeTimer.Stop();
            this.Dispose();
        }
    }
}
