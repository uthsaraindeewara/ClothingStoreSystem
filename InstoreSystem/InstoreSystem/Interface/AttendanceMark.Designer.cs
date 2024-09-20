namespace InstoreSystem.Interface
{
    partial class AttendanceMark
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
            this.lblDate = new System.Windows.Forms.Label();
            this.lblTime = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtId = new System.Windows.Forms.TextBox();
            this.lblLate = new System.Windows.Forms.Label();
            this.butnSubmit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDate.Location = new System.Drawing.Point(12, 9);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(0, 46);
            this.lblDate.TabIndex = 0;
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTime.Location = new System.Drawing.Point(433, 9);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(0, 46);
            this.lblTime.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(81, 146);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 38);
            this.label1.TabIndex = 2;
            this.label1.Text = "ID";
            // 
            // txtId
            // 
            this.txtId.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtId.Location = new System.Drawing.Point(148, 143);
            this.txtId.Name = "txtId";
            this.txtId.Size = new System.Drawing.Size(356, 45);
            this.txtId.TabIndex = 3;
            // 
            // lblLate
            // 
            this.lblLate.AutoSize = true;
            this.lblLate.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLate.ForeColor = System.Drawing.Color.Red;
            this.lblLate.Location = new System.Drawing.Point(132, 222);
            this.lblLate.Name = "lblLate";
            this.lblLate.Size = new System.Drawing.Size(0, 32);
            this.lblLate.TabIndex = 4;
            this.lblLate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // butnSubmit
            // 
            this.butnSubmit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(43)))), ((int)(((byte)(43)))));
            this.butnSubmit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.butnSubmit.ForeColor = System.Drawing.Color.White;
            this.butnSubmit.Location = new System.Drawing.Point(526, 143);
            this.butnSubmit.Name = "butnSubmit";
            this.butnSubmit.Size = new System.Drawing.Size(112, 49);
            this.butnSubmit.TabIndex = 5;
            this.butnSubmit.Text = "Submit";
            this.butnSubmit.UseVisualStyleBackColor = false;
            this.butnSubmit.Click += new System.EventHandler(this.butnSubmit_Click);
            // 
            // Attendance
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(704, 310);
            this.Controls.Add(this.butnSubmit);
            this.Controls.Add(this.lblLate);
            this.Controls.Add(this.txtId);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblTime);
            this.Controls.Add(this.lblDate);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Attendance";
            this.Text = "Attendance";
            this.Load += new System.EventHandler(this.Attendance_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtId;
        private System.Windows.Forms.Label lblLate;
        private System.Windows.Forms.Button butnSubmit;
    }
}