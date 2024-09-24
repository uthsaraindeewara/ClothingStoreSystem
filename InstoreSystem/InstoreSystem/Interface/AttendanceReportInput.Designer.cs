namespace InstoreSystem.Interface
{
    partial class AttendanceReportInput
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.lblType = new System.Windows.Forms.Label();
            this.dtpPeriod = new System.Windows.Forms.DateTimePicker();
            this.cmbStore = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.rbDaily = new System.Windows.Forms.RadioButton();
            this.rbYearly = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(468, 212);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(102, 45);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(338, 212);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(102, 45);
            this.btnGenerate.TabIndex = 10;
            this.btnGenerate.Text = "Generate";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // lblType
            // 
            this.lblType.AutoSize = true;
            this.lblType.Location = new System.Drawing.Point(78, 93);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(53, 25);
            this.lblType.TabIndex = 9;
            this.lblType.Text = "Year";
            // 
            // dtpPeriod
            // 
            this.dtpPeriod.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpPeriod.Location = new System.Drawing.Point(165, 88);
            this.dtpPeriod.Name = "dtpPeriod";
            this.dtpPeriod.Size = new System.Drawing.Size(405, 30);
            this.dtpPeriod.TabIndex = 12;
            // 
            // cmbStore
            // 
            this.cmbStore.FormattingEnabled = true;
            this.cmbStore.Items.AddRange(new object[] {
            "Palawaththa",
            "Welisara"});
            this.cmbStore.Location = new System.Drawing.Point(165, 154);
            this.cmbStore.Name = "cmbStore";
            this.cmbStore.Size = new System.Drawing.Size(405, 33);
            this.cmbStore.TabIndex = 13;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(78, 157);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 25);
            this.label1.TabIndex = 14;
            this.label1.Text = "Store";
            // 
            // rbDaily
            // 
            this.rbDaily.AutoSize = true;
            this.rbDaily.Checked = true;
            this.rbDaily.Location = new System.Drawing.Point(134, 25);
            this.rbDaily.Name = "rbDaily";
            this.rbDaily.Size = new System.Drawing.Size(76, 29);
            this.rbDaily.TabIndex = 15;
            this.rbDaily.TabStop = true;
            this.rbDaily.Text = "Daily";
            this.rbDaily.UseVisualStyleBackColor = true;
            this.rbDaily.CheckedChanged += new System.EventHandler(this.rbDaily_CheckedChanged);
            // 
            // rbYearly
            // 
            this.rbYearly.AutoSize = true;
            this.rbYearly.Location = new System.Drawing.Point(357, 25);
            this.rbYearly.Name = "rbYearly";
            this.rbYearly.Size = new System.Drawing.Size(88, 29);
            this.rbYearly.TabIndex = 16;
            this.rbYearly.TabStop = true;
            this.rbYearly.Text = "Yearly";
            this.rbYearly.UseVisualStyleBackColor = true;
            this.rbYearly.CheckedChanged += new System.EventHandler(this.rbYearly_CheckedChanged);
            // 
            // AttendanceReportInput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(640, 289);
            this.Controls.Add(this.rbYearly);
            this.Controls.Add(this.rbDaily);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbStore);
            this.Controls.Add(this.dtpPeriod);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.lblType);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "AttendanceReportInput";
            this.Text = "AttrndanceReportInput";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.DateTimePicker dtpPeriod;
        private System.Windows.Forms.ComboBox cmbStore;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton rbDaily;
        private System.Windows.Forms.RadioButton rbYearly;
    }
}