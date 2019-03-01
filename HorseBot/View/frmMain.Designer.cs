namespace Betburger
{
    partial class frmMain
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.tblSureBet = new System.Windows.Forms.DataGridView();
            this.bindingSourceArb = new System.Windows.Forms.BindingSource(this.components);
            this.rtLog = new System.Windows.Forms.RichTextBox();
            this.timerKeep = new System.Windows.Forms.Timer(this.components);
            this.lblTitle = new System.Windows.Forms.Label();
            this.bindingSourceBet365 = new System.Windows.Forms.BindingSource(this.components);
            this.bindingSourceUnibet = new System.Windows.Forms.BindingSource(this.components);
            this.bindingSourceLadbrokes = new System.Windows.Forms.BindingSource(this.components);
            this.bindingSourcePinnacle = new System.Windows.Forms.BindingSource(this.components);
            this.lblStatus = new System.Windows.Forms.Label();
            this.btnMax = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnMin = new System.Windows.Forms.Button();
            this.btnIcon = new System.Windows.Forms.Button();
            this.picTitle = new System.Windows.Forms.PictureBox();
            this.btnBookies = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.bindingSourceBetting = new System.Windows.Forms.BindingSource(this.components);
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPercent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colROI = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSports = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEvent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOutCome = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.tblSureBet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceArb)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceBet365)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceUnibet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceLadbrokes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourcePinnacle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picTitle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceBetting)).BeginInit();
            this.SuspendLayout();
            // 
            // tblSureBet
            // 
            this.tblSureBet.AllowDrop = true;
            this.tblSureBet.AllowUserToAddRows = false;
            this.tblSureBet.AllowUserToDeleteRows = false;
            this.tblSureBet.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tblSureBet.AutoGenerateColumns = false;
            this.tblSureBet.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tblSureBet.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.tblSureBet.ColumnHeadersHeight = 30;
            this.tblSureBet.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.colPercent,
            this.colROI,
            this.colSports,
            this.colEvent,
            this.colOutCome});
            this.tblSureBet.DataSource = this.bindingSourceArb;
            this.tblSureBet.Location = new System.Drawing.Point(12, 76);
            this.tblSureBet.Name = "tblSureBet";
            this.tblSureBet.RowHeadersVisible = false;
            this.tblSureBet.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.tblSureBet.Size = new System.Drawing.Size(787, 226);
            this.tblSureBet.TabIndex = 4;
            // 
            // rtLog
            // 
            this.rtLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtLog.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.rtLog.Location = new System.Drawing.Point(12, 341);
            this.rtLog.Name = "rtLog";
            this.rtLog.ReadOnly = true;
            this.rtLog.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.rtLog.Size = new System.Drawing.Size(787, 200);
            this.rtLog.TabIndex = 6;
            this.rtLog.Text = "";
            // 
            // timerKeep
            // 
            this.timerKeep.Interval = 600000;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblTitle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(39, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(83, 16);
            this.lblTitle.TabIndex = 19;
            this.lblTitle.Text = "Horse Live";
            // 
            // lblStatus
            // 
            this.lblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(12, 316);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(58, 13);
            this.lblStatus.TabIndex = 20;
            this.lblStatus.Text = "Status Log";
            // 
            // btnMax
            // 
            this.btnMax.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMax.BackColor = System.Drawing.Color.Transparent;
            this.btnMax.BackgroundImage = global::Betburger.Properties.Resources.max;
            this.btnMax.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnMax.FlatAppearance.BorderSize = 0;
            this.btnMax.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnMax.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnMax.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMax.Location = new System.Drawing.Point(727, 6);
            this.btnMax.Name = "btnMax";
            this.btnMax.Size = new System.Drawing.Size(22, 22);
            this.btnMax.TabIndex = 18;
            this.btnMax.UseVisualStyleBackColor = false;
            this.btnMax.Visible = false;
            this.btnMax.Click += new System.EventHandler(this.btnMax_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.BackgroundImage = global::Betburger.Properties.Resources.Close;
            this.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Location = new System.Drawing.Point(783, 6);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(22, 22);
            this.btnClose.TabIndex = 16;
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnMin
            // 
            this.btnMin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMin.BackColor = System.Drawing.Color.Transparent;
            this.btnMin.BackgroundImage = global::Betburger.Properties.Resources.min;
            this.btnMin.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnMin.FlatAppearance.BorderSize = 0;
            this.btnMin.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnMin.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnMin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMin.Location = new System.Drawing.Point(755, 6);
            this.btnMin.Name = "btnMin";
            this.btnMin.Size = new System.Drawing.Size(22, 22);
            this.btnMin.TabIndex = 17;
            this.btnMin.UseVisualStyleBackColor = false;
            this.btnMin.Click += new System.EventHandler(this.btnMin_Click);
            // 
            // btnIcon
            // 
            this.btnIcon.BackColor = System.Drawing.Color.Transparent;
            this.btnIcon.BackgroundImage = global::Betburger.Properties.Resources.logo;
            this.btnIcon.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnIcon.FlatAppearance.BorderSize = 0;
            this.btnIcon.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnIcon.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnIcon.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnIcon.Location = new System.Drawing.Point(5, 5);
            this.btnIcon.Name = "btnIcon";
            this.btnIcon.Size = new System.Drawing.Size(24, 24);
            this.btnIcon.TabIndex = 11;
            this.btnIcon.UseVisualStyleBackColor = false;
            // 
            // picTitle
            // 
            this.picTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.picTitle.BackgroundImage = global::Betburger.Properties.Resources.Log_Title1;
            this.picTitle.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.picTitle.Location = new System.Drawing.Point(0, 0);
            this.picTitle.Name = "picTitle";
            this.picTitle.Size = new System.Drawing.Size(815, 36);
            this.picTitle.TabIndex = 10;
            this.picTitle.TabStop = false;
            this.picTitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picTitle_MouseDown);
            this.picTitle.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picTitle_MouseMove);
            // 
            // btnBookies
            // 
            this.btnBookies.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBookies.BackColor = System.Drawing.Color.Gray;
            this.btnBookies.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnBookies.BackgroundImage")));
            this.btnBookies.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnBookies.FlatAppearance.BorderSize = 0;
            this.btnBookies.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnBookies.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnBookies.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBookies.Location = new System.Drawing.Point(554, 555);
            this.btnBookies.Name = "btnBookies";
            this.btnBookies.Size = new System.Drawing.Size(70, 30);
            this.btnBookies.TabIndex = 8;
            this.btnBookies.Text = "Bookies";
            this.btnBookies.UseVisualStyleBackColor = false;
            this.btnBookies.Click += new System.EventHandler(this.btnBookies_Click);
            // 
            // btnStop
            // 
            this.btnStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStop.BackColor = System.Drawing.Color.Gray;
            this.btnStop.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnStop.BackgroundImage")));
            this.btnStop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnStop.Enabled = false;
            this.btnStop.FlatAppearance.BorderSize = 0;
            this.btnStop.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnStop.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStop.Location = new System.Drawing.Point(729, 555);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(70, 30);
            this.btnStop.TabIndex = 7;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = false;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnStart
            // 
            this.btnStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStart.BackColor = System.Drawing.Color.Gray;
            this.btnStart.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnStart.BackgroundImage")));
            this.btnStart.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnStart.FlatAppearance.BorderSize = 0;
            this.btnStart.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnStart.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStart.Location = new System.Drawing.Point(641, 555);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(70, 30);
            this.btnStart.TabIndex = 7;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // Column1
            // 
            this.Column1.DataPropertyName = "percent";
            this.Column1.HeaderText = "%";
            this.Column1.Name = "Column1";
            this.Column1.Width = 60;
            // 
            // colPercent
            // 
            this.colPercent.DataPropertyName = "bookie";
            this.colPercent.HeaderText = "Bookmaker";
            this.colPercent.Name = "colPercent";
            // 
            // colROI
            // 
            this.colROI.DataPropertyName = "sport";
            this.colROI.HeaderText = "Sport";
            this.colROI.Name = "colROI";
            this.colROI.Width = 130;
            // 
            // colSports
            // 
            this.colSports.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colSports.DataPropertyName = "race";
            this.colSports.HeaderText = "Race";
            this.colSports.Name = "colSports";
            // 
            // colEvent
            // 
            this.colEvent.DataPropertyName = "runner";
            this.colEvent.FillWeight = 60F;
            this.colEvent.HeaderText = "Runner";
            this.colEvent.Name = "colEvent";
            this.colEvent.Width = 150;
            // 
            // colOutCome
            // 
            this.colOutCome.DataPropertyName = "odds";
            this.colOutCome.HeaderText = "Odds";
            this.colOutCome.Name = "colOutCome";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 51);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 20;
            this.label1.Text = "Bets List";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gray;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(811, 598);
            this.ControlBox = false;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.btnMax);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnMin);
            this.Controls.Add(this.btnIcon);
            this.Controls.Add(this.picTitle);
            this.Controls.Add(this.btnBookies);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.rtLog);
            this.Controls.Add(this.tblSureBet);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(1700, 900);
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.tblSureBet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceArb)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceBet365)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceUnibet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceLadbrokes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourcePinnacle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picTitle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceBetting)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView tblSureBet;
        private System.Windows.Forms.RichTextBox rtLog;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.BindingSource bindingSourceArb;
        private System.Windows.Forms.BindingSource bindingSourceBetting;
        private System.Windows.Forms.Timer timerKeep;
        private System.Windows.Forms.PictureBox picTitle;
        private System.Windows.Forms.Button btnIcon;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnMax;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnMin;
        private System.Windows.Forms.Button btnBookies;
        private System.Windows.Forms.BindingSource bindingSourceBet365;
        private System.Windows.Forms.BindingSource bindingSourceUnibet;
        private System.Windows.Forms.BindingSource bindingSourceLadbrokes;
        private System.Windows.Forms.BindingSource bindingSourcePinnacle;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPercent;
        private System.Windows.Forms.DataGridViewTextBoxColumn colROI;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSports;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEvent;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOutCome;
        private System.Windows.Forms.Label label1;
    }
}

