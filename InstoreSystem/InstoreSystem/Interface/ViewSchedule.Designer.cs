namespace InstoreSystem.Interface
{
    partial class ViewSchedule
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnChangeSch = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.ScheduleID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StartTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EndTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StoreID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EmployeeNames = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnChangeSch
            // 
            this.btnChangeSch.BackColor = System.Drawing.Color.Blue;
            this.btnChangeSch.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnChangeSch.ForeColor = System.Drawing.Color.White;
            this.btnChangeSch.Location = new System.Drawing.Point(377, 456);
            this.btnChangeSch.Name = "btnChangeSch";
            this.btnChangeSch.Size = new System.Drawing.Size(209, 109);
            this.btnChangeSch.TabIndex = 0;
            this.btnChangeSch.Text = "Change Schedule";
            this.btnChangeSch.UseVisualStyleBackColor = false;
            this.btnChangeSch.Click += new System.EventHandler(this.btnChangeSch_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView1.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.RaisedHorizontal;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ScheduleID,
            this.Date,
            this.StartTime,
            this.EndTime,
            this.StoreID,
            this.EmployeeNames});
            this.dataGridView1.Location = new System.Drawing.Point(13, 12);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 62;
            this.dataGridView1.RowTemplate.Height = 28;
            this.dataGridView1.Size = new System.Drawing.Size(960, 401);
            this.dataGridView1.TabIndex = 1;
            // 
            // ScheduleID
            // 
            this.ScheduleID.HeaderText = "Schedule ID";
            this.ScheduleID.MinimumWidth = 8;
            this.ScheduleID.Name = "ScheduleID";
            // 
            // Date
            // 
            this.Date.HeaderText = "Date";
            this.Date.MinimumWidth = 8;
            this.Date.Name = "Date";
            // 
            // StartTime
            // 
            this.StartTime.HeaderText = "Start Time";
            this.StartTime.MinimumWidth = 8;
            this.StartTime.Name = "StartTime";
            // 
            // EndTime
            // 
            this.EndTime.HeaderText = "End Time";
            this.EndTime.MinimumWidth = 8;
            this.EndTime.Name = "EndTime";
            // 
            // StoreID
            // 
            this.StoreID.HeaderText = "Store ID";
            this.StoreID.MinimumWidth = 8;
            this.StoreID.Name = "StoreID";
            // 
            // EmployeeNames
            // 
            this.EmployeeNames.HeaderText = "EmployeeID(s)";
            this.EmployeeNames.MinimumWidth = 8;
            this.EmployeeNames.Name = "EmployeeNames";
            // 
            // ViewSchedule
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(985, 630);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.btnChangeSch);
            this.Name = "ViewSchedule";
            this.Text = "ViewSchedule";
            this.Load += new System.EventHandler(this.ViewSchedule_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnChangeSch;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ScheduleID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Date;
        private System.Windows.Forms.DataGridViewTextBoxColumn StartTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn EndTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn StoreID;
        private System.Windows.Forms.DataGridViewTextBoxColumn EmployeeNames;
    }
}