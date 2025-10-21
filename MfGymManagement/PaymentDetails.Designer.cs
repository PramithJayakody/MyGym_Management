namespace MfGymManagement
{
    partial class PaymentDetails
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
            this.lblMemberName = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dgvPaymentHistory = new System.Windows.Forms.DataGridView();
            this.gbAddNewPayment = new System.Windows.Forms.GroupBox();
            this.btnSavePayment = new System.Windows.Forms.Button();
            this.cmbNewFeesMode = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtNewAmount = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtNewReceiptNo = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnClosePaymentForm = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPaymentHistory)).BeginInit();
            this.gbAddNewPayment.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblMemberName
            // 
            this.lblMemberName.AutoSize = true;
            this.lblMemberName.Location = new System.Drawing.Point(25, 32);
            this.lblMemberName.Name = "lblMemberName";
            this.lblMemberName.Size = new System.Drawing.Size(85, 13);
            this.lblMemberName.TabIndex = 0;
            this.lblMemberName.Text = "Member Name : ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 83);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Payment History";
            // 
            // dgvPaymentHistory
            // 
            this.dgvPaymentHistory.AllowUserToAddRows = false;
            this.dgvPaymentHistory.AllowUserToDeleteRows = false;
            this.dgvPaymentHistory.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPaymentHistory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPaymentHistory.Location = new System.Drawing.Point(28, 115);
            this.dgvPaymentHistory.Name = "dgvPaymentHistory";
            this.dgvPaymentHistory.ReadOnly = true;
            this.dgvPaymentHistory.Size = new System.Drawing.Size(437, 171);
            this.dgvPaymentHistory.TabIndex = 2;
            // 
            // gbAddNewPayment
            // 
            this.gbAddNewPayment.Controls.Add(this.btnSavePayment);
            this.gbAddNewPayment.Controls.Add(this.cmbNewFeesMode);
            this.gbAddNewPayment.Controls.Add(this.label4);
            this.gbAddNewPayment.Controls.Add(this.txtNewAmount);
            this.gbAddNewPayment.Controls.Add(this.label3);
            this.gbAddNewPayment.Controls.Add(this.txtNewReceiptNo);
            this.gbAddNewPayment.Controls.Add(this.label2);
            this.gbAddNewPayment.Location = new System.Drawing.Point(28, 326);
            this.gbAddNewPayment.Name = "gbAddNewPayment";
            this.gbAddNewPayment.Size = new System.Drawing.Size(437, 259);
            this.gbAddNewPayment.TabIndex = 3;
            this.gbAddNewPayment.TabStop = false;
            this.gbAddNewPayment.Text = "Add New Payment";
            // 
            // btnSavePayment
            // 
            this.btnSavePayment.Location = new System.Drawing.Point(315, 218);
            this.btnSavePayment.Name = "btnSavePayment";
            this.btnSavePayment.Size = new System.Drawing.Size(75, 23);
            this.btnSavePayment.TabIndex = 7;
            this.btnSavePayment.Text = "Save";
            this.btnSavePayment.UseVisualStyleBackColor = true;
            this.btnSavePayment.Click += new System.EventHandler(this.btnSavePayment_Click);
            // 
            // cmbNewFeesMode
            // 
            this.cmbNewFeesMode.FormattingEnabled = true;
            this.cmbNewFeesMode.Items.AddRange(new object[] {
            "Monthly",
            "Quarterly",
            "Half Yearly",
            "Yearly"});
            this.cmbNewFeesMode.Location = new System.Drawing.Point(107, 125);
            this.cmbNewFeesMode.Name = "cmbNewFeesMode";
            this.cmbNewFeesMode.Size = new System.Drawing.Size(121, 21);
            this.cmbNewFeesMode.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 128);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Fees Mode";
            // 
            // txtNewAmount
            // 
            this.txtNewAmount.Location = new System.Drawing.Point(108, 76);
            this.txtNewAmount.Name = "txtNewAmount";
            this.txtNewAmount.Size = new System.Drawing.Size(100, 20);
            this.txtNewAmount.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 81);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(43, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Amount";
            // 
            // txtNewReceiptNo
            // 
            this.txtNewReceiptNo.BackColor = System.Drawing.SystemColors.ControlLight;
            this.txtNewReceiptNo.Location = new System.Drawing.Point(107, 35);
            this.txtNewReceiptNo.Name = "txtNewReceiptNo";
            this.txtNewReceiptNo.ReadOnly = true;
            this.txtNewReceiptNo.Size = new System.Drawing.Size(185, 20);
            this.txtNewReceiptNo.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Receipt No";
            // 
            // btnClosePaymentForm
            // 
            this.btnClosePaymentForm.Location = new System.Drawing.Point(1020, 596);
            this.btnClosePaymentForm.Name = "btnClosePaymentForm";
            this.btnClosePaymentForm.Size = new System.Drawing.Size(101, 42);
            this.btnClosePaymentForm.TabIndex = 6;
            this.btnClosePaymentForm.Text = "Close";
            this.btnClosePaymentForm.UseVisualStyleBackColor = true;
            this.btnClosePaymentForm.Click += new System.EventHandler(this.btnClosePaymentForm_Click);
            // 
            // PaymentDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1168, 650);
            this.Controls.Add(this.btnClosePaymentForm);
            this.Controls.Add(this.gbAddNewPayment);
            this.Controls.Add(this.dgvPaymentHistory);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblMemberName);
            this.Name = "PaymentDetails";
            this.Text = "PaymentDetails";
            this.Load += new System.EventHandler(this.PaymentDetails_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPaymentHistory)).EndInit();
            this.gbAddNewPayment.ResumeLayout(false);
            this.gbAddNewPayment.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblMemberName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dgvPaymentHistory;
        private System.Windows.Forms.GroupBox gbAddNewPayment;
        private System.Windows.Forms.TextBox txtNewReceiptNo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtNewAmount;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbNewFeesMode;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnClosePaymentForm;
        private System.Windows.Forms.Button btnSavePayment;
    }
}