namespace TicTacToeUi
{
    partial class MainWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.panel1 = new System.Windows.Forms.Panel();
            this.mainTableLayloutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.groupBoxUtility = new System.Windows.Forms.GroupBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnNew = new System.Windows.Forms.Button();
            this.btnHistory = new System.Windows.Forms.Button();
            this.btnLoad = new System.Windows.Forms.Button();
            this.groupBoxDiff = new System.Windows.Forms.GroupBox();
            this.radioBtn1vs1 = new System.Windows.Forms.RadioButton();
            this.radioBtnHard = new System.Windows.Forms.RadioButton();
            this.radioBtnMedium = new System.Windows.Forms.RadioButton();
            this.radioBtnEasy = new System.Windows.Forms.RadioButton();
            this.txtBoxTrackBar = new System.Windows.Forms.TextBox();
            this.boardSizeTrackBar = new System.Windows.Forms.TrackBar();
            this.btnStart = new System.Windows.Forms.Button();
            this.txtBoxDraw = new System.Windows.Forms.TextBox();
            this.lblDraw = new System.Windows.Forms.Label();
            this.txtBoxO = new System.Windows.Forms.TextBox();
            this.txtBoxX = new System.Windows.Forms.TextBox();
            this.lblO = new System.Windows.Forms.Label();
            this.lblX = new System.Windows.Forms.Label();
            this.dataGrid = new System.Windows.Forms.DataGridView();
            this.panel1.SuspendLayout();
            this.groupBoxUtility.SuspendLayout();
            this.groupBoxDiff.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.boardSizeTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.Controls.Add(this.mainTableLayloutPanel);
            this.panel1.Location = new System.Drawing.Point(166, 73);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(126, 140);
            this.panel1.TabIndex = 0;
            // 
            // mainTableLayloutPanel
            // 
            this.mainTableLayloutPanel.AutoSize = true;
            this.mainTableLayloutPanel.ColumnCount = 1;
            this.mainTableLayloutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.mainTableLayloutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainTableLayloutPanel.Location = new System.Drawing.Point(0, 0);
            this.mainTableLayloutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.mainTableLayloutPanel.Name = "mainTableLayloutPanel";
            this.mainTableLayloutPanel.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.mainTableLayloutPanel.RowCount = 1;
            this.mainTableLayloutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.mainTableLayloutPanel.Size = new System.Drawing.Size(126, 140);
            this.mainTableLayloutPanel.TabIndex = 1;
            // 
            // groupBoxUtility
            // 
            this.groupBoxUtility.Controls.Add(this.btnSave);
            this.groupBoxUtility.Controls.Add(this.btnNew);
            this.groupBoxUtility.Controls.Add(this.btnHistory);
            this.groupBoxUtility.Controls.Add(this.btnLoad);
            this.groupBoxUtility.Location = new System.Drawing.Point(14, 229);
            this.groupBoxUtility.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBoxUtility.Name = "groupBoxUtility";
            this.groupBoxUtility.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBoxUtility.Size = new System.Drawing.Size(145, 215);
            this.groupBoxUtility.TabIndex = 14;
            this.groupBoxUtility.TabStop = false;
            this.groupBoxUtility.Text = "Utility";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(24, 44);
            this.btnSave.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(97, 28);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // btnNew
            // 
            this.btnNew.Location = new System.Drawing.Point(24, 147);
            this.btnNew.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(97, 28);
            this.btnNew.TabIndex = 4;
            this.btnNew.Text = "New Session";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.BtnNew_Click);
            // 
            // btnHistory
            // 
            this.btnHistory.Location = new System.Drawing.Point(24, 113);
            this.btnHistory.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnHistory.Name = "btnHistory";
            this.btnHistory.Size = new System.Drawing.Size(97, 28);
            this.btnHistory.TabIndex = 7;
            this.btnHistory.Text = "History";
            this.btnHistory.UseVisualStyleBackColor = true;
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(24, 77);
            this.btnLoad.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(97, 28);
            this.btnLoad.TabIndex = 6;
            this.btnLoad.Text = "Load";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.BtnLoad_Click);
            // 
            // groupBoxDiff
            // 
            this.groupBoxDiff.Controls.Add(this.radioBtn1vs1);
            this.groupBoxDiff.Controls.Add(this.radioBtnHard);
            this.groupBoxDiff.Controls.Add(this.radioBtnMedium);
            this.groupBoxDiff.Controls.Add(this.radioBtnEasy);
            this.groupBoxDiff.Location = new System.Drawing.Point(14, 73);
            this.groupBoxDiff.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBoxDiff.Name = "groupBoxDiff";
            this.groupBoxDiff.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBoxDiff.Size = new System.Drawing.Size(145, 148);
            this.groupBoxDiff.TabIndex = 13;
            this.groupBoxDiff.TabStop = false;
            this.groupBoxDiff.Text = "Difficulty";
            // 
            // radioBtn1vs1
            // 
            this.radioBtn1vs1.AutoSize = true;
            this.radioBtn1vs1.Checked = true;
            this.radioBtn1vs1.Location = new System.Drawing.Point(24, 109);
            this.radioBtn1vs1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.radioBtn1vs1.Name = "radioBtn1vs1";
            this.radioBtn1vs1.Size = new System.Drawing.Size(58, 20);
            this.radioBtn1vs1.TabIndex = 3;
            this.radioBtn1vs1.TabStop = true;
            this.radioBtn1vs1.Text = "1 vs 1";
            this.radioBtn1vs1.UseVisualStyleBackColor = true;
            // 
            // radioBtnHard
            // 
            this.radioBtnHard.AutoSize = true;
            this.radioBtnHard.Location = new System.Drawing.Point(24, 81);
            this.radioBtnHard.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.radioBtnHard.Name = "radioBtnHard";
            this.radioBtnHard.Size = new System.Drawing.Size(54, 20);
            this.radioBtnHard.TabIndex = 2;
            this.radioBtnHard.Text = "Hard";
            this.radioBtnHard.UseVisualStyleBackColor = true;
            // 
            // radioBtnMedium
            // 
            this.radioBtnMedium.AutoSize = true;
            this.radioBtnMedium.Location = new System.Drawing.Point(24, 53);
            this.radioBtnMedium.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.radioBtnMedium.Name = "radioBtnMedium";
            this.radioBtnMedium.Size = new System.Drawing.Size(74, 20);
            this.radioBtnMedium.TabIndex = 1;
            this.radioBtnMedium.TabStop = true;
            this.radioBtnMedium.Text = "Medium";
            this.radioBtnMedium.UseVisualStyleBackColor = true;
            // 
            // radioBtnEasy
            // 
            this.radioBtnEasy.AutoSize = true;
            this.radioBtnEasy.Location = new System.Drawing.Point(24, 24);
            this.radioBtnEasy.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.radioBtnEasy.Name = "radioBtnEasy";
            this.radioBtnEasy.Size = new System.Drawing.Size(52, 20);
            this.radioBtnEasy.TabIndex = 0;
            this.radioBtnEasy.TabStop = true;
            this.radioBtnEasy.Text = "Easy";
            this.radioBtnEasy.UseVisualStyleBackColor = true;
            // 
            // txtBoxTrackBar
            // 
            this.txtBoxTrackBar.Location = new System.Drawing.Point(853, 32);
            this.txtBoxTrackBar.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtBoxTrackBar.Name = "txtBoxTrackBar";
            this.txtBoxTrackBar.ReadOnly = true;
            this.txtBoxTrackBar.Size = new System.Drawing.Size(42, 22);
            this.txtBoxTrackBar.TabIndex = 12;
            // 
            // boardSizeTrackBar
            // 
            this.boardSizeTrackBar.LargeChange = 2;
            this.boardSizeTrackBar.Location = new System.Drawing.Point(14, 15);
            this.boardSizeTrackBar.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.boardSizeTrackBar.Maximum = 7;
            this.boardSizeTrackBar.Minimum = 3;
            this.boardSizeTrackBar.Name = "boardSizeTrackBar";
            this.boardSizeTrackBar.Size = new System.Drawing.Size(832, 45);
            this.boardSizeTrackBar.SmallChange = 2;
            this.boardSizeTrackBar.TabIndex = 11;
            this.boardSizeTrackBar.TabStop = false;
            this.boardSizeTrackBar.Value = 3;
            this.boardSizeTrackBar.Scroll += new System.EventHandler(this.BoardSizeTrackBar_Scroll);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(903, 28);
            this.btnStart.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(97, 28);
            this.btnStart.TabIndex = 10;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.BtnStart_Click);
            // 
            // txtBoxDraw
            // 
            this.txtBoxDraw.Location = new System.Drawing.Point(848, 636);
            this.txtBoxDraw.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtBoxDraw.Name = "txtBoxDraw";
            this.txtBoxDraw.ReadOnly = true;
            this.txtBoxDraw.Size = new System.Drawing.Size(33, 22);
            this.txtBoxDraw.TabIndex = 21;
            // 
            // lblDraw
            // 
            this.lblDraw.AutoSize = true;
            this.lblDraw.Location = new System.Drawing.Point(845, 616);
            this.lblDraw.Name = "lblDraw";
            this.lblDraw.Size = new System.Drawing.Size(38, 16);
            this.lblDraw.TabIndex = 20;
            this.lblDraw.Text = "Draw";
            // 
            // txtBoxO
            // 
            this.txtBoxO.Location = new System.Drawing.Point(927, 636);
            this.txtBoxO.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtBoxO.Name = "txtBoxO";
            this.txtBoxO.ReadOnly = true;
            this.txtBoxO.Size = new System.Drawing.Size(44, 22);
            this.txtBoxO.TabIndex = 19;
            // 
            // txtBoxX
            // 
            this.txtBoxX.Location = new System.Drawing.Point(761, 636);
            this.txtBoxX.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtBoxX.Name = "txtBoxX";
            this.txtBoxX.ReadOnly = true;
            this.txtBoxX.Size = new System.Drawing.Size(44, 22);
            this.txtBoxX.TabIndex = 18;
            // 
            // lblO
            // 
            this.lblO.AutoSize = true;
            this.lblO.Location = new System.Drawing.Point(924, 616);
            this.lblO.Name = "lblO";
            this.lblO.Size = new System.Drawing.Size(47, 16);
            this.lblO.TabIndex = 17;
            this.lblO.Text = "O Wins";
            // 
            // lblX
            // 
            this.lblX.AutoSize = true;
            this.lblX.Location = new System.Drawing.Point(757, 616);
            this.lblX.Name = "lblX";
            this.lblX.Size = new System.Drawing.Size(45, 16);
            this.lblX.TabIndex = 16;
            this.lblX.Text = "X Wins";
            // 
            // dataGrid
            // 
            this.dataGrid.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.dataGrid.AllowUserToAddRows = false;
            this.dataGrid.AllowUserToDeleteRows = false;
            this.dataGrid.AllowUserToResizeColumns = false;
            this.dataGrid.AllowUserToResizeRows = false;
            this.dataGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGrid.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.dataGrid.Location = new System.Drawing.Point(734, 83);
            this.dataGrid.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dataGrid.MultiSelect = false;
            this.dataGrid.Name = "dataGrid";
            this.dataGrid.ReadOnly = true;
            this.dataGrid.RowHeadersVisible = false;
            this.dataGrid.ShowCellErrors = false;
            this.dataGrid.ShowCellToolTips = false;
            this.dataGrid.ShowEditingIcon = false;
            this.dataGrid.ShowRowErrors = false;
            this.dataGrid.Size = new System.Drawing.Size(266, 521);
            this.dataGrid.TabIndex = 22;
            this.dataGrid.TabStop = false;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1016, 676);
            this.Controls.Add(this.dataGrid);
            this.Controls.Add(this.txtBoxDraw);
            this.Controls.Add(this.lblDraw);
            this.Controls.Add(this.txtBoxO);
            this.Controls.Add(this.txtBoxX);
            this.Controls.Add(this.lblO);
            this.Controls.Add(this.lblX);
            this.Controls.Add(this.groupBoxUtility);
            this.Controls.Add(this.groupBoxDiff);
            this.Controls.Add(this.txtBoxTrackBar);
            this.Controls.Add(this.boardSizeTrackBar);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.Name = "MainWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TicTacToe";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindow_FormClosing);
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBoxUtility.ResumeLayout(false);
            this.groupBoxDiff.ResumeLayout(false);
            this.groupBoxDiff.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.boardSizeTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel mainTableLayloutPanel;
        private System.Windows.Forms.GroupBox groupBoxUtility;
        public System.Windows.Forms.Button btnSave;
        public System.Windows.Forms.Button btnNew;
        public System.Windows.Forms.Button btnHistory;
        public System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.GroupBox groupBoxDiff;
        private System.Windows.Forms.RadioButton radioBtn1vs1;
        private System.Windows.Forms.RadioButton radioBtnHard;
        private System.Windows.Forms.RadioButton radioBtnMedium;
        private System.Windows.Forms.RadioButton radioBtnEasy;
        private System.Windows.Forms.TextBox txtBoxTrackBar;
        private System.Windows.Forms.TrackBar boardSizeTrackBar;
        public System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.TextBox txtBoxDraw;
        private System.Windows.Forms.Label lblDraw;
        private System.Windows.Forms.TextBox txtBoxO;
        private System.Windows.Forms.TextBox txtBoxX;
        private System.Windows.Forms.Label lblO;
        private System.Windows.Forms.Label lblX;
        private System.Windows.Forms.DataGridView dataGrid;
    }
}

