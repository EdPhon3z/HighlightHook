namespace HighlightHook;

partial class Form1
{
    private System.ComponentModel.IContainer components = null;
    private System.Windows.Forms.MenuStrip menuStrip1;
    private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem helpHowItWorksToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem helpChangeLogToolStripMenuItem;
    private System.Windows.Forms.Label lblTitle;
    private System.Windows.Forms.Label lblDescription;
    private System.Windows.Forms.Label lblTrackerPath;
    private System.Windows.Forms.TextBox txtTrackerPath;
    private System.Windows.Forms.Button btnBrowse;
    private System.Windows.Forms.Label lblDelay;
    private System.Windows.Forms.TrackBar trackBarDelay;
    private System.Windows.Forms.Label lblDelayValue;
    private System.Windows.Forms.Label lblReplayBuffer;
    private System.Windows.Forms.NumericUpDown numReplayBufferSeconds;
    private System.Windows.Forms.Label lblReplayBufferValue;
    private System.Windows.Forms.Label lblObsHost;
    private System.Windows.Forms.TextBox txtObsHost;
    private System.Windows.Forms.Label lblObsPort;
    private System.Windows.Forms.NumericUpDown numObsPort;
    private System.Windows.Forms.Label lblObsPassword;
    private System.Windows.Forms.TextBox txtObsPassword;
    private System.Windows.Forms.Button btnStart;
    private System.Windows.Forms.Button btnStop;
    private System.Windows.Forms.Label lblStatusLabel;
    private System.Windows.Forms.Label lblStatus;
    private System.Windows.Forms.GroupBox grpHowItWorks;
    private System.Windows.Forms.TextBox txtHowItWorks;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }

        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    private void InitializeComponent()
    {
        this.menuStrip1 = new System.Windows.Forms.MenuStrip();
        this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.helpHowItWorksToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.helpChangeLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.lblTitle = new System.Windows.Forms.Label();
        this.lblDescription = new System.Windows.Forms.Label();
        this.lblTrackerPath = new System.Windows.Forms.Label();
        this.txtTrackerPath = new System.Windows.Forms.TextBox();
        this.btnBrowse = new System.Windows.Forms.Button();
        this.lblDelay = new System.Windows.Forms.Label();
        this.trackBarDelay = new System.Windows.Forms.TrackBar();
        this.lblDelayValue = new System.Windows.Forms.Label();
        this.lblReplayBuffer = new System.Windows.Forms.Label();
        this.numReplayBufferSeconds = new System.Windows.Forms.NumericUpDown();
        this.lblReplayBufferValue = new System.Windows.Forms.Label();
        this.lblObsHost = new System.Windows.Forms.Label();
        this.txtObsHost = new System.Windows.Forms.TextBox();
        this.lblObsPort = new System.Windows.Forms.Label();
        this.numObsPort = new System.Windows.Forms.NumericUpDown();
        this.lblObsPassword = new System.Windows.Forms.Label();
        this.txtObsPassword = new System.Windows.Forms.TextBox();
        this.btnStart = new System.Windows.Forms.Button();
        this.btnStop = new System.Windows.Forms.Button();
        this.lblStatusLabel = new System.Windows.Forms.Label();
        this.lblStatus = new System.Windows.Forms.Label();
        this.grpHowItWorks = new System.Windows.Forms.GroupBox();
        this.txtHowItWorks = new System.Windows.Forms.TextBox();
        ((System.ComponentModel.ISupportInitialize)(this.trackBarDelay)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.numReplayBufferSeconds)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.numObsPort)).BeginInit();
        this.menuStrip1.SuspendLayout();
        this.grpHowItWorks.SuspendLayout();
        this.SuspendLayout();
        // 
        // menuStrip1
        // 
        this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
        this.helpToolStripMenuItem});
        this.menuStrip1.Location = new System.Drawing.Point(0, 0);
        this.menuStrip1.Name = "menuStrip1";
        this.menuStrip1.Size = new System.Drawing.Size(714, 24);
        this.menuStrip1.TabIndex = 0;
        this.menuStrip1.Text = "menuStrip1";
        // 
        // helpToolStripMenuItem
        // 
        this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
        this.helpHowItWorksToolStripMenuItem,
        this.helpChangeLogToolStripMenuItem});
        this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
        this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
        this.helpToolStripMenuItem.Text = "Help";
        // 
        // helpHowItWorksToolStripMenuItem
        // 
        this.helpHowItWorksToolStripMenuItem.Name = "helpHowItWorksToolStripMenuItem";
        this.helpHowItWorksToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
        this.helpHowItWorksToolStripMenuItem.Text = "How It Works";
        this.helpHowItWorksToolStripMenuItem.Click += new System.EventHandler(this.helpHowItWorksToolStripMenuItem_Click);
        // 
        // helpChangeLogToolStripMenuItem
        // 
        this.helpChangeLogToolStripMenuItem.Name = "helpChangeLogToolStripMenuItem";
        this.helpChangeLogToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
        this.helpChangeLogToolStripMenuItem.Text = "Change Log";
        this.helpChangeLogToolStripMenuItem.Click += new System.EventHandler(this.helpChangeLogToolStripMenuItem_Click);
        // 
        // lblTitle
        // 
        this.lblTitle.AutoSize = true;
        this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        this.lblTitle.Location = new System.Drawing.Point(16, 32);
        this.lblTitle.Name = "lblTitle";
        this.lblTitle.Size = new System.Drawing.Size(168, 25);
        this.lblTitle.TabIndex = 1;
        this.lblTitle.Text = "Highlight Hook";
        // 
        // lblDescription
        // 
        this.lblDescription.AutoSize = true;
        this.lblDescription.Location = new System.Drawing.Point(18, 64);
        this.lblDescription.Name = "lblDescription";
        this.lblDescription.Size = new System.Drawing.Size(339, 15);
        this.lblDescription.TabIndex = 2;
        this.lblDescription.Text = "Auto-save OBS replay buffer after highlight events";
        // 
        // lblTrackerPath
        // 
        this.lblTrackerPath.AutoSize = true;
        this.lblTrackerPath.Location = new System.Drawing.Point(18, 98);
        this.lblTrackerPath.Name = "lblTrackerPath";
        this.lblTrackerPath.Size = new System.Drawing.Size(103, 15);
        this.lblTrackerPath.TabIndex = 3;
        this.lblTrackerPath.Text = "Highlight tracker path:";
        // 
        // txtTrackerPath
        // 
        this.txtTrackerPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
        this.txtTrackerPath.Location = new System.Drawing.Point(18, 116);
        this.txtTrackerPath.Name = "txtTrackerPath";
        this.txtTrackerPath.Size = new System.Drawing.Size(582, 23);
        this.txtTrackerPath.TabIndex = 4;
        this.txtTrackerPath.TextChanged += new System.EventHandler(this.txtTrackerPath_TextChanged);
        // 
        // btnBrowse
        // 
        this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
        this.btnBrowse.Location = new System.Drawing.Point(606, 115);
        this.btnBrowse.Name = "btnBrowse";
        this.btnBrowse.Size = new System.Drawing.Size(90, 24);
        this.btnBrowse.TabIndex = 5;
        this.btnBrowse.Text = "Browse...";
        this.btnBrowse.UseVisualStyleBackColor = true;
        this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
        // 
        // lblDelay
        // 
        this.lblDelay.AutoSize = true;
        this.lblDelay.Location = new System.Drawing.Point(18, 154);
        this.lblDelay.Name = "lblDelay";
        this.lblDelay.Size = new System.Drawing.Size(176, 15);
        this.lblDelay.TabIndex = 6;
        this.lblDelay.Text = "After-highlight reaction delay:";
        // 
        // trackBarDelay
        // 
        this.trackBarDelay.Location = new System.Drawing.Point(18, 172);
        this.trackBarDelay.Maximum = 120;
        this.trackBarDelay.Minimum = 10;
        this.trackBarDelay.Name = "trackBarDelay";
        this.trackBarDelay.Size = new System.Drawing.Size(582, 45);
        this.trackBarDelay.TabIndex = 7;
        this.trackBarDelay.TickFrequency = 10;
        this.trackBarDelay.Value = 45;
        this.trackBarDelay.Scroll += new System.EventHandler(this.trackBarDelay_ValueChanged);
        // 
        // lblDelayValue
        // 
        this.lblDelayValue.AutoSize = true;
        this.lblDelayValue.Location = new System.Drawing.Point(606, 173);
        this.lblDelayValue.Name = "lblDelayValue";
        this.lblDelayValue.Size = new System.Drawing.Size(93, 15);
        this.lblDelayValue.TabIndex = 8;
        this.lblDelayValue.Text = "45 seconds";
        // 
        // lblReplayBuffer
        // 
        this.lblReplayBuffer.AutoSize = true;
        this.lblReplayBuffer.Location = new System.Drawing.Point(18, 214);
        this.lblReplayBuffer.Name = "lblReplayBuffer";
        this.lblReplayBuffer.Size = new System.Drawing.Size(143, 15);
        this.lblReplayBuffer.TabIndex = 9;
        this.lblReplayBuffer.Text = "Replay buffer length:";
        // 
        // numReplayBufferSeconds
        // 
        this.numReplayBufferSeconds.Location = new System.Drawing.Point(18, 232);
        this.numReplayBufferSeconds.Maximum = new decimal(new int[] {
        600,
        0,
        0,
        0});
        this.numReplayBufferSeconds.Minimum = new decimal(new int[] {
        10,
        0,
        0,
        0});
        this.numReplayBufferSeconds.Name = "numReplayBufferSeconds";
        this.numReplayBufferSeconds.Size = new System.Drawing.Size(100, 23);
        this.numReplayBufferSeconds.TabIndex = 10;
        this.numReplayBufferSeconds.Value = new decimal(new int[] {
        60,
        0,
        0,
        0});
        this.numReplayBufferSeconds.ValueChanged += new System.EventHandler(this.numReplayBufferSeconds_ValueChanged);
        // 
        // lblReplayBufferValue
        // 
        this.lblReplayBufferValue.AutoSize = true;
        this.lblReplayBufferValue.Location = new System.Drawing.Point(128, 234);
        this.lblReplayBufferValue.Name = "lblReplayBufferValue";
        this.lblReplayBufferValue.Size = new System.Drawing.Size(93, 15);
        this.lblReplayBufferValue.TabIndex = 11;
        this.lblReplayBufferValue.Text = "60 seconds";
        // 
        // lblObsHost
        // 
        this.lblObsHost.AutoSize = true;
        this.lblObsHost.Location = new System.Drawing.Point(18, 270);
        this.lblObsHost.Name = "lblObsHost";
        this.lblObsHost.Size = new System.Drawing.Size(78, 15);
        this.lblObsHost.TabIndex = 12;
        this.lblObsHost.Text = "OBS WebSocket:";
        // 
        // txtObsHost
        // 
        this.txtObsHost.Location = new System.Drawing.Point(18, 290);
        this.txtObsHost.Name = "txtObsHost";
        this.txtObsHost.Size = new System.Drawing.Size(200, 23);
        this.txtObsHost.TabIndex = 13;
        // 
        // lblObsPort
        // 
        this.lblObsPort.AutoSize = true;
        this.lblObsPort.Location = new System.Drawing.Point(234, 270);
        this.lblObsPort.Name = "lblObsPort";
        this.lblObsPort.Size = new System.Drawing.Size(31, 15);
        this.lblObsPort.TabIndex = 14;
        this.lblObsPort.Text = "Port:";
        // 
        // numObsPort
        // 
        this.numObsPort.Location = new System.Drawing.Point(234, 290);
        this.numObsPort.Maximum = new decimal(new int[] {
        65535,
        0,
        0,
        0});
        this.numObsPort.Minimum = new decimal(new int[] {
        1,
        0,
        0,
        0});
        this.numObsPort.Name = "numObsPort";
        this.numObsPort.Size = new System.Drawing.Size(100, 23);
        this.numObsPort.TabIndex = 15;
        this.numObsPort.Value = new decimal(new int[] {
        4455,
        0,
        0,
        0});
        // 
        // lblObsPassword
        // 
        this.lblObsPassword.AutoSize = true;
        this.lblObsPassword.Location = new System.Drawing.Point(346, 270);
        this.lblObsPassword.Name = "lblObsPassword";
        this.lblObsPassword.Size = new System.Drawing.Size(60, 15);
        this.lblObsPassword.TabIndex = 16;
        this.lblObsPassword.Text = "Password:";
        // 
        // txtObsPassword
        // 
        this.txtObsPassword.Location = new System.Drawing.Point(346, 290);
        this.txtObsPassword.Name = "txtObsPassword";
        this.txtObsPassword.PasswordChar = '*';
        this.txtObsPassword.Size = new System.Drawing.Size(250, 23);
        this.txtObsPassword.TabIndex = 17;
        // 
        // btnStart
        // 
        this.btnStart.Location = new System.Drawing.Point(18, 338);
        this.btnStart.Name = "btnStart";
        this.btnStart.Size = new System.Drawing.Size(110, 30);
        this.btnStart.TabIndex = 18;
        this.btnStart.Text = "Start Watching";
        this.btnStart.UseVisualStyleBackColor = true;
        this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
        // 
        // btnStop
        // 
        this.btnStop.Enabled = false;
        this.btnStop.Location = new System.Drawing.Point(138, 338);
        this.btnStop.Name = "btnStop";
        this.btnStop.Size = new System.Drawing.Size(90, 30);
        this.btnStop.TabIndex = 19;
        this.btnStop.Text = "Stop";
        this.btnStop.UseVisualStyleBackColor = true;
        this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
        // 
        // lblStatusLabel
        // 
        this.lblStatusLabel.AutoSize = true;
        this.lblStatusLabel.Location = new System.Drawing.Point(18, 388);
        this.lblStatusLabel.Name = "lblStatusLabel";
        this.lblStatusLabel.Size = new System.Drawing.Size(42, 15);
        this.lblStatusLabel.TabIndex = 20;
        this.lblStatusLabel.Text = "Status:";
        // 
        // lblStatus
        // 
        this.lblStatus.AutoSize = true;
        this.lblStatus.Location = new System.Drawing.Point(72, 388);
        this.lblStatus.Name = "lblStatus";
        this.lblStatus.Size = new System.Drawing.Size(143, 15);
        this.lblStatus.TabIndex = 21;
        this.lblStatus.Text = "Waiting for Highlight...";
        // 
        // grpHowItWorks
        // 
        this.grpHowItWorks.Controls.Add(this.txtHowItWorks);
        this.grpHowItWorks.Location = new System.Drawing.Point(18, 418);
        this.grpHowItWorks.Name = "grpHowItWorks";
        this.grpHowItWorks.Size = new System.Drawing.Size(678, 90);
        this.grpHowItWorks.TabIndex = 22;
        this.grpHowItWorks.TabStop = false;
        this.grpHowItWorks.Text = "How it works";
        // 
        // txtHowItWorks
        // 
        this.txtHowItWorks.BorderStyle = System.Windows.Forms.BorderStyle.None;
        this.txtHowItWorks.Dock = System.Windows.Forms.DockStyle.Fill;
        this.txtHowItWorks.Location = new System.Drawing.Point(3, 19);
        this.txtHowItWorks.Multiline = true;
        this.txtHowItWorks.Name = "txtHowItWorks";
        this.txtHowItWorks.ReadOnly = true;
        this.txtHowItWorks.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
        this.txtHowItWorks.Size = new System.Drawing.Size(672, 68);
        this.txtHowItWorks.TabIndex = 0;
        this.txtHowItWorks.Text = "1. Read NVIDIA's UGCTracker.json to find the last known Highlight ID.\r\n2. Watch the tracker file for new highlight entries.\r\n3. Wait the after-highlight delay.\r\n4. Save OBS replay buffer.\r\n5. Replay buffer length is entered manually because OBS does not expose it over WebSocket.\r\n6. Future batching can use the replay buffer length to group nearby highlights into one save.";
        // 
        // Form1
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(714, 524);
        this.Controls.Add(this.grpHowItWorks);
        this.Controls.Add(this.lblStatus);
        this.Controls.Add(this.lblStatusLabel);
        this.Controls.Add(this.btnStop);
        this.Controls.Add(this.btnStart);
        this.Controls.Add(this.txtObsPassword);
        this.Controls.Add(this.lblObsPassword);
        this.Controls.Add(this.numObsPort);
        this.Controls.Add(this.lblObsPort);
        this.Controls.Add(this.txtObsHost);
        this.Controls.Add(this.lblObsHost);
        this.Controls.Add(this.lblReplayBufferValue);
        this.Controls.Add(this.numReplayBufferSeconds);
        this.Controls.Add(this.lblReplayBuffer);
        this.Controls.Add(this.lblDelayValue);
        this.Controls.Add(this.trackBarDelay);
        this.Controls.Add(this.lblDelay);
        this.Controls.Add(this.btnBrowse);
        this.Controls.Add(this.txtTrackerPath);
        this.Controls.Add(this.lblTrackerPath);
        this.Controls.Add(this.lblDescription);
        this.Controls.Add(this.lblTitle);
        this.Controls.Add(this.menuStrip1);
        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
        this.MainMenuStrip = this.menuStrip1;
        this.MaximizeBox = false;
        this.Name = "Form1";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        this.Text = "Highlight Hook";
        this.Load += new System.EventHandler(this.Form1_Load);
        this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
        ((System.ComponentModel.ISupportInitialize)(this.trackBarDelay)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.numReplayBufferSeconds)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.numObsPort)).EndInit();
        this.menuStrip1.ResumeLayout(false);
        this.menuStrip1.PerformLayout();
        this.grpHowItWorks.ResumeLayout(false);
        this.grpHowItWorks.PerformLayout();
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    #endregion
}
