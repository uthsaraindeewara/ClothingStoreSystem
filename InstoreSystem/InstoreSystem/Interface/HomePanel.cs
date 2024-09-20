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
    public partial class HomePanel : UserControl
    {
        public HomePanel()
        {
            InitializeComponent();
        }

        private void btnMarkAttendance_Click(object sender, EventArgs e)
        {
            AttendanceMark att = new AttendanceMark();
            att.ShowDialog();
        }
    }
}
