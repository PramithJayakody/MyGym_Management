namespace MfGymManagement
{
    partial class ViewMembers
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.dgvMembers = new System.Windows.Forms.DataGridView();
            this.btnClose = new System.Windows.Forms.Button();
            this.picSelectedMember = new System.Windows.Forms.PictureBox();
            this.btnShowAll = new System.Windows.Forms.Button();
            this.lblHint = new System.Windows.Forms.Label();
            this.lblDueDate = new System.Windows.Forms.Label();
            this.lblDaysRemaining = new System.Windows.Forms.Label();
            this.btnEditMember = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMembers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSelectedMember)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(131, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Search by Name/Phone : ";
            // 
            // txtSearch
            // 
            this.txtSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSearch.Location = new System.Drawing.Point(163, 22);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(219, 20);
            this.txtSearch.TabIndex = 1;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(447, 22);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 2;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // dgvMembers
            // 
            this.dgvMembers.AllowUserToAddRows = false;
            this.dgvMembers.AllowUserToDeleteRows = false;
            this.dgvMembers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvMembers.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvMembers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMembers.Location = new System.Drawing.Point(29, 85);
            this.dgvMembers.Name = "dgvMembers";
            this.dgvMembers.ReadOnly = true;
            this.dgvMembers.Size = new System.Drawing.Size(517, 432);
            this.dgvMembers.TabIndex = 3;
            this.dgvMembers.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvMembers_CellDoubleClick);
            this.dgvMembers.SelectionChanged += new System.EventHandler(this.dgvMembers_SelectionChanged);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(964, 573);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(93, 38);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // picSelectedMember
            // 
            this.picSelectedMember.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picSelectedMember.Location = new System.Drawing.Point(672, 85);
            this.picSelectedMember.Name = "picSelectedMember";
            this.picSelectedMember.Size = new System.Drawing.Size(169, 187);
            this.picSelectedMember.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picSelectedMember.TabIndex = 5;
            this.picSelectedMember.TabStop = false;
            // 
            // btnShowAll
            // 
            this.btnShowAll.Location = new System.Drawing.Point(537, 22);
            this.btnShowAll.Name = "btnShowAll";
            this.btnShowAll.Size = new System.Drawing.Size(75, 23);
            this.btnShowAll.TabIndex = 6;
            this.btnShowAll.Text = "Show All";
            this.btnShowAll.UseVisualStyleBackColor = true;
            this.btnShowAll.Click += new System.EventHandler(this.btnShowAll_Click);
            // 
            // lblHint
            // 
            this.lblHint.AutoSize = true;
            this.lblHint.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHint.ForeColor = System.Drawing.Color.Red;
            this.lblHint.Location = new System.Drawing.Point(82, 573);
            this.lblHint.Name = "lblHint";
            this.lblHint.Size = new System.Drawing.Size(348, 17);
            this.lblHint.TabIndex = 7;
            this.lblHint.Text = "To View/Update Payments: Double-Click on a Member";
            // 
            // lblDueDate
            // 
            this.lblDueDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDueDate.Location = new System.Drawing.Point(672, 323);
            this.lblDueDate.Name = "lblDueDate";
            this.lblDueDate.Size = new System.Drawing.Size(255, 23);
            this.lblDueDate.TabIndex = 8;
            this.lblDueDate.Text = "Next Due Date : ";
            // 
            // lblDaysRemaining
            // 
            this.lblDaysRemaining.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDaysRemaining.Location = new System.Drawing.Point(675, 383);
            this.lblDaysRemaining.Name = "lblDaysRemaining";
            this.lblDaysRemaining.Size = new System.Drawing.Size(269, 23);
            this.lblDaysRemaining.TabIndex = 9;
            this.lblDaysRemaining.Text = "Status : ";
            // 
            // btnEditMember
            // 
            this.btnEditMember.BackColor = System.Drawing.SystemColors.HotTrack;
            this.btnEditMember.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEditMember.Location = new System.Drawing.Point(565, 85);
            this.btnEditMember.Name = "btnEditMember";
            this.btnEditMember.Size = new System.Drawing.Size(75, 28);
            this.btnEditMember.TabIndex = 10;
            this.btnEditMember.Text = "Edit";
            this.btnEditMember.UseVisualStyleBackColor = false;
            this.btnEditMember.Click += new System.EventHandler(this.btnEditMember_Click);
            // 
            // ViewMembers
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1157, 633);
            this.Controls.Add(this.btnEditMember);
            this.Controls.Add(this.lblDaysRemaining);
            this.Controls.Add(this.lblDueDate);
            this.Controls.Add(this.lblHint);
            this.Controls.Add(this.btnShowAll);
            this.Controls.Add(this.picSelectedMember);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.dgvMembers);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.label1);
            this.Name = "ViewMembers";
            this.Text = "ViewMembers";
            this.Load += new System.EventHandler(this.ViewMembers_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMembers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSelectedMember)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.DataGridView dgvMembers;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.PictureBox picSelectedMember;
        private System.Windows.Forms.Button btnShowAll;
        private System.Windows.Forms.Label lblHint;
        private System.Windows.Forms.Label lblDueDate;
        private System.Windows.Forms.Label lblDaysRemaining;
        private System.Windows.Forms.Button btnEditMember;
    }
}