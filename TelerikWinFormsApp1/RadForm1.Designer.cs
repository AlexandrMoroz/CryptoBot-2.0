namespace Cryptobot
{
    partial class RadForm1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RadForm1));
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition1 = new Telerik.WinControls.UI.TableViewDefinition();
            this.radPanel1 = new Telerik.WinControls.UI.RadPanel();
            this.SerializeButton = new Telerik.WinControls.UI.RadButton();
            this.TimerView = new System.Windows.Forms.Label();
            this.minimize = new System.Windows.Forms.Button();
            this.exit = new System.Windows.Forms.Button();
            this.CheakNamesCheakBox = new System.Windows.Forms.CheckBox();
            this.CheakWalletsCheakBox = new System.Windows.Forms.CheckBox();
            this.BTC_USD = new System.Windows.Forms.TextBox();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.EqualsButton = new Telerik.WinControls.UI.RadButton();
            this.TripleArbitration = new Telerik.WinControls.UI.RadButton();
            this.radButton1 = new Telerik.WinControls.UI.RadButton();
            this.radButton2 = new Telerik.WinControls.UI.RadButton();
            this.TranzactionButton = new Telerik.WinControls.UI.RadButton();
            this.radPanel2 = new Telerik.WinControls.UI.RadPanel();
            this.StocksCheakBox = new Telerik.WinControls.UI.RadCheckedListBox();
            this.StockWaitingBar = new Telerik.WinControls.UI.RadWaitingBar();
            this.dotsSpinnerWaitingBarIndicatorElement1 = new Telerik.WinControls.UI.DotsSpinnerWaitingBarIndicatorElement();
            this.dataGridView1 = new Telerik.WinControls.UI.RadGridView();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel1)).BeginInit();
            this.radPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SerializeButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.EqualsButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TripleArbitration)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton1)).BeginInit();
            this.radButton1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radButton2)).BeginInit();
            this.radButton2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TranzactionButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel2)).BeginInit();
            this.radPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.StocksCheakBox)).BeginInit();
            this.StocksCheakBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.StockWaitingBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radPanel1
            // 
            this.radPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(39)))), ((int)(((byte)(40)))));
            this.radPanel1.Controls.Add(this.SerializeButton);
            this.radPanel1.Controls.Add(this.TimerView);
            this.radPanel1.Controls.Add(this.minimize);
            this.radPanel1.Controls.Add(this.exit);
            this.radPanel1.Controls.Add(this.CheakNamesCheakBox);
            this.radPanel1.Controls.Add(this.CheakWalletsCheakBox);
            this.radPanel1.Controls.Add(this.BTC_USD);
            this.radPanel1.Controls.Add(this.linkLabel1);
            this.radPanel1.Controls.Add(this.EqualsButton);
            this.radPanel1.Controls.Add(this.TripleArbitration);
            this.radPanel1.Controls.Add(this.radButton1);
            this.radPanel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.radPanel1.Location = new System.Drawing.Point(0, 0);
            this.radPanel1.Name = "radPanel1";
            this.radPanel1.Size = new System.Drawing.Size(283, 771);
            this.radPanel1.TabIndex = 1;
            this.radPanel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.radPanel1_MouseDown);
            // 
            // SerializeButton
            // 
            this.SerializeButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(39)))), ((int)(((byte)(40)))));
            this.SerializeButton.Font = new System.Drawing.Font("Roboto Light", 12F);
            this.SerializeButton.ForeColor = System.Drawing.Color.White;
            this.SerializeButton.Location = new System.Drawing.Point(0, 639);
            this.SerializeButton.Name = "SerializeButton";
            this.SerializeButton.Size = new System.Drawing.Size(283, 74);
            this.SerializeButton.TabIndex = 18;
            this.SerializeButton.Text = "Serialize";
            this.SerializeButton.Click += new System.EventHandler(this.SerializeButton_Click);
            // 
            // TimerView
            // 
            this.TimerView.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.TimerView.ForeColor = System.Drawing.Color.Silver;
            this.TimerView.Location = new System.Drawing.Point(3, 584);
            this.TimerView.Name = "TimerView";
            this.TimerView.Size = new System.Drawing.Size(280, 52);
            this.TimerView.TabIndex = 17;
            this.TimerView.Text = "0";
            this.TimerView.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // minimize
            // 
            this.minimize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.minimize.ForeColor = System.Drawing.Color.Silver;
            this.minimize.Image = ((System.Drawing.Image)(resources.GetObject("minimize.Image")));
            this.minimize.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.minimize.Location = new System.Drawing.Point(58, 719);
            this.minimize.Name = "minimize";
            this.minimize.Size = new System.Drawing.Size(40, 40);
            this.minimize.TabIndex = 16;
            this.minimize.UseVisualStyleBackColor = true;
            this.minimize.Click += new System.EventHandler(this.minimize_Click);
            // 
            // exit
            // 
            this.exit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.exit.ForeColor = System.Drawing.Color.Silver;
            this.exit.Image = ((System.Drawing.Image)(resources.GetObject("exit.Image")));
            this.exit.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.exit.Location = new System.Drawing.Point(12, 719);
            this.exit.Name = "exit";
            this.exit.Size = new System.Drawing.Size(40, 40);
            this.exit.TabIndex = 15;
            this.exit.UseVisualStyleBackColor = true;
            this.exit.Click += new System.EventHandler(this.exit_Click);
            // 
            // CheakNamesCheakBox
            // 
            this.CheakNamesCheakBox.AutoSize = true;
            this.CheakNamesCheakBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.CheakNamesCheakBox.ForeColor = System.Drawing.Color.Silver;
            this.CheakNamesCheakBox.Location = new System.Drawing.Point(12, 369);
            this.CheakNamesCheakBox.Name = "CheakNamesCheakBox";
            this.CheakNamesCheakBox.Size = new System.Drawing.Size(190, 24);
            this.CheakNamesCheakBox.TabIndex = 14;
            this.CheakNamesCheakBox.Text = "Cheak Names of Coins";
            this.CheakNamesCheakBox.UseVisualStyleBackColor = true;
            // 
            // CheakWalletsCheakBox
            // 
            this.CheakWalletsCheakBox.AutoSize = true;
            this.CheakWalletsCheakBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.CheakWalletsCheakBox.ForeColor = System.Drawing.Color.Silver;
            this.CheakWalletsCheakBox.Location = new System.Drawing.Point(12, 339);
            this.CheakWalletsCheakBox.Name = "CheakWalletsCheakBox";
            this.CheakWalletsCheakBox.Size = new System.Drawing.Size(130, 24);
            this.CheakWalletsCheakBox.TabIndex = 13;
            this.CheakWalletsCheakBox.Text = "Cheak Wallets";
            this.CheakWalletsCheakBox.UseVisualStyleBackColor = true;
            // 
            // BTC_USD
            // 
            this.BTC_USD.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.BTC_USD.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BTC_USD.Location = new System.Drawing.Point(0, 225);
            this.BTC_USD.Name = "BTC_USD";
            this.BTC_USD.Size = new System.Drawing.Size(283, 20);
            this.BTC_USD.TabIndex = 12;
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.linkLabel1.ForeColor = System.Drawing.Color.Silver;
            this.linkLabel1.LinkColor = System.Drawing.Color.Silver;
            this.linkLabel1.Location = new System.Drawing.Point(39, 248);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(202, 20);
            this.linkLabel1.TabIndex = 11;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "BTC-USD Exchange Rates";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // EqualsButton
            // 
            this.EqualsButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(39)))), ((int)(((byte)(40)))));
            this.EqualsButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.EqualsButton.Font = new System.Drawing.Font("Roboto Light", 12F);
            this.EqualsButton.ForeColor = System.Drawing.Color.White;
            this.EqualsButton.Location = new System.Drawing.Point(0, 153);
            this.EqualsButton.Name = "EqualsButton";
            this.EqualsButton.Size = new System.Drawing.Size(283, 72);
            this.EqualsButton.TabIndex = 2;
            this.EqualsButton.Text = "Select all equals coins ";
            this.EqualsButton.Click += new System.EventHandler(this.EqualsButton_Click);
            // 
            // TripleArbitration
            // 
            this.TripleArbitration.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(39)))), ((int)(((byte)(40)))));
            this.TripleArbitration.Dock = System.Windows.Forms.DockStyle.Top;
            this.TripleArbitration.Font = new System.Drawing.Font("Roboto Light", 12F);
            this.TripleArbitration.ForeColor = System.Drawing.Color.White;
            this.TripleArbitration.Location = new System.Drawing.Point(0, 77);
            this.TripleArbitration.Name = "TripleArbitration";
            this.TripleArbitration.Size = new System.Drawing.Size(283, 76);
            this.TripleArbitration.TabIndex = 1;
            this.TripleArbitration.Text = "Triple Arbitration";
            this.TripleArbitration.Click += new System.EventHandler(this.TripleArbitration_Click);
            // 
            // radButton1
            // 
            this.radButton1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(39)))), ((int)(((byte)(40)))));
            this.radButton1.Controls.Add(this.radButton2);
            this.radButton1.Cursor = System.Windows.Forms.Cursors.Default;
            this.radButton1.DisplayStyle = Telerik.WinControls.DisplayStyle.Text;
            this.radButton1.Dock = System.Windows.Forms.DockStyle.Top;
            this.radButton1.Font = new System.Drawing.Font("Roboto Light", 12F);
            this.radButton1.ForeColor = System.Drawing.Color.White;
            this.radButton1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.radButton1.Location = new System.Drawing.Point(0, 0);
            this.radButton1.Name = "radButton1";
            // 
            // 
            // 
            this.radButton1.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.Auto;
            this.radButton1.RootElement.BorderHighlightColor = System.Drawing.Color.Transparent;
            this.radButton1.RootElement.BorderHighlightThickness = 0;
            this.radButton1.RootElement.Enabled = true;
            this.radButton1.RootElement.FocusBorderColor = System.Drawing.SystemColors.ButtonHighlight;
            this.radButton1.Size = new System.Drawing.Size(283, 77);
            this.radButton1.TabIndex = 0;
            this.radButton1.Text = "Tranzaction Algoritm";
            // 
            // radButton2
            // 
            this.radButton2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(39)))), ((int)(((byte)(40)))));
            this.radButton2.Controls.Add(this.TranzactionButton);
            this.radButton2.Cursor = System.Windows.Forms.Cursors.Default;
            this.radButton2.DisplayStyle = Telerik.WinControls.DisplayStyle.Text;
            this.radButton2.Dock = System.Windows.Forms.DockStyle.Top;
            this.radButton2.Font = new System.Drawing.Font("Roboto Light", 12F);
            this.radButton2.ForeColor = System.Drawing.Color.White;
            this.radButton2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.radButton2.Location = new System.Drawing.Point(0, 0);
            this.radButton2.Name = "radButton2";
            // 
            // 
            // 
            this.radButton2.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.Auto;
            this.radButton2.RootElement.BorderHighlightColor = System.Drawing.Color.Transparent;
            this.radButton2.RootElement.BorderHighlightThickness = 0;
            this.radButton2.RootElement.Enabled = true;
            this.radButton2.RootElement.FocusBorderColor = System.Drawing.SystemColors.ButtonHighlight;
            this.radButton2.Size = new System.Drawing.Size(283, 77);
            this.radButton2.TabIndex = 1;
            this.radButton2.Text = "Tranzaction Algoritm";
            // 
            // TranzactionButton
            // 
            this.TranzactionButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(39)))), ((int)(((byte)(40)))));
            this.TranzactionButton.Cursor = System.Windows.Forms.Cursors.Default;
            this.TranzactionButton.DisplayStyle = Telerik.WinControls.DisplayStyle.Text;
            this.TranzactionButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.TranzactionButton.Font = new System.Drawing.Font("Roboto Light", 12F);
            this.TranzactionButton.ForeColor = System.Drawing.Color.White;
            this.TranzactionButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.TranzactionButton.Location = new System.Drawing.Point(0, 0);
            this.TranzactionButton.Name = "TranzactionButton";
            // 
            // 
            // 
            this.TranzactionButton.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.Auto;
            this.TranzactionButton.RootElement.BorderHighlightColor = System.Drawing.Color.Transparent;
            this.TranzactionButton.RootElement.BorderHighlightThickness = 0;
            this.TranzactionButton.RootElement.Enabled = true;
            this.TranzactionButton.RootElement.FocusBorderColor = System.Drawing.SystemColors.ButtonHighlight;
            this.TranzactionButton.Size = new System.Drawing.Size(283, 77);
            this.TranzactionButton.TabIndex = 2;
            this.TranzactionButton.Text = "Tranzaction Algoritm";
            this.TranzactionButton.Click += new System.EventHandler(this.Tranzaction_Click);
            // 
            // radPanel2
            // 
            this.radPanel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(39)))), ((int)(((byte)(40)))));
            this.radPanel2.Controls.Add(this.StocksCheakBox);
            this.radPanel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.radPanel2.Location = new System.Drawing.Point(1354, 0);
            this.radPanel2.Name = "radPanel2";
            this.radPanel2.Size = new System.Drawing.Size(305, 771);
            this.radPanel2.TabIndex = 3;
            // 
            // StocksCheakBox
            // 
            this.StocksCheakBox.Controls.Add(this.StockWaitingBar);
            this.StocksCheakBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.StocksCheakBox.Location = new System.Drawing.Point(0, 0);
            this.StocksCheakBox.Name = "StocksCheakBox";
            this.StocksCheakBox.Size = new System.Drawing.Size(305, 771);
            this.StocksCheakBox.TabIndex = 0;
            // 
            // StockWaitingBar
            // 
            this.StockWaitingBar.BackColor = System.Drawing.Color.White;
            this.StockWaitingBar.Location = new System.Drawing.Point(112, 405);
            this.StockWaitingBar.Name = "StockWaitingBar";
            this.StockWaitingBar.Size = new System.Drawing.Size(70, 70);
            this.StockWaitingBar.TabIndex = 0;
            this.StockWaitingBar.Text = "radWaitingBar1";
            this.StockWaitingBar.WaitingIndicators.Add(this.dotsSpinnerWaitingBarIndicatorElement1);
            this.StockWaitingBar.WaitingSpeed = 100;
            this.StockWaitingBar.WaitingStyle = Telerik.WinControls.Enumerations.WaitingBarStyles.DotsSpinner;
            // 
            // dotsSpinnerWaitingBarIndicatorElement1
            // 
            this.dotsSpinnerWaitingBarIndicatorElement1.AccelerationSpeed = 16D;
            this.dotsSpinnerWaitingBarIndicatorElement1.BackColor = System.Drawing.Color.Transparent;
            this.dotsSpinnerWaitingBarIndicatorElement1.Name = "dotsSpinnerWaitingBarIndicatorElement1";
            // 
            // dataGridView1
            // 
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(283, 0);
            // 
            // 
            // 
            this.dataGridView1.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(1071, 771);
            this.dataGridView1.TabIndex = 4;
            // 
            // RadForm1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1659, 771);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.radPanel2);
            this.Controls.Add(this.radPanel1);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "RadForm1";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "RadForm1";
            this.Load += new System.EventHandler(this.RadForm1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.radPanel1)).EndInit();
            this.radPanel1.ResumeLayout(false);
            this.radPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SerializeButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.EqualsButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TripleArbitration)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton1)).EndInit();
            this.radButton1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radButton2)).EndInit();
            this.radButton2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.TranzactionButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel2)).EndInit();
            this.radPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.StocksCheakBox)).EndInit();
            this.StocksCheakBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.StockWaitingBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadPanel radPanel1;
        private Telerik.WinControls.UI.RadPanel radPanel2;
        private Telerik.WinControls.UI.RadCheckedListBox StocksCheakBox;
        private Telerik.WinControls.UI.RadGridView dataGridView1;
        private Telerik.WinControls.UI.RadButton radButton1;
        private Telerik.WinControls.UI.RadButton TripleArbitration;
        private Telerik.WinControls.UI.RadButton radButton2;
        private Telerik.WinControls.UI.RadButton TranzactionButton;
        private System.Windows.Forms.Button minimize;
        private System.Windows.Forms.Button exit;
        private System.Windows.Forms.CheckBox CheakNamesCheakBox;
        private System.Windows.Forms.CheckBox CheakWalletsCheakBox;
        private System.Windows.Forms.TextBox BTC_USD;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private Telerik.WinControls.UI.RadButton EqualsButton;
        private System.Windows.Forms.Label TimerView;
        private Telerik.WinControls.UI.RadWaitingBar StockWaitingBar;
        private Telerik.WinControls.UI.DotsSpinnerWaitingBarIndicatorElement dotsSpinnerWaitingBarIndicatorElement1;
        private Telerik.WinControls.UI.RadButton SerializeButton;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
    }
}