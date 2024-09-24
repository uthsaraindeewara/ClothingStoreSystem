using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FinalTest.Interface;

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

        private void btnRegister_Click(object sender, EventArgs e)
        {
            Register register = new Register();
            register.ShowDialog();
        }

        private void btnSchedules_Click(object sender, EventArgs e)
        {
            ViewSchedule schedule = new ViewSchedule();

            Application app = this.ParentForm as Application;

            if (app.position != "Manager")
            {
                schedule.disableChangeSch();
            }

            schedule.ShowDialog();
        }

        private void btnLeaveRequest_Click(object sender, EventArgs e)
        {
            LeaveRequest leave = new LeaveRequest();
            leave.ShowDialog();
        }
    }
}
