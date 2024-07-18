namespace NSScreenshotOrganiser_WinFormGui
{
    partial class FormMainWindow
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lblStatus = new Label();
            btnOutputFolder = new Button();
            btnInputFolder = new Button();
            btnStartOrganising = new Button();
            lblInputFolder = new Label();
            lblOutputFolder = new Label();
            progressBar1 = new ProgressBar();
            lblVersionString = new Label();
            SuspendLayout();
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Font = new Font("Segoe UI", 9F, FontStyle.Italic);
            lblStatus.Location = new Point(12, 124);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(121, 15);
            lblStatus.TabIndex = 0;
            lblStatus.Text = "0 known titles loaded.";
            lblStatus.Click += lblStatus_Click;
            lblStatus.MouseMove += lblTitleDetectionCount_MouseMove;
            // 
            // btnOutputFolder
            // 
            btnOutputFolder.Location = new Point(12, 43);
            btnOutputFolder.Name = "btnOutputFolder";
            btnOutputFolder.Size = new Size(158, 23);
            btnOutputFolder.TabIndex = 14;
            btnOutputFolder.Text = "Select Output Folder";
            btnOutputFolder.UseVisualStyleBackColor = true;
            btnOutputFolder.Click += btnOutputFolder_Click;
            // 
            // btnInputFolder
            // 
            btnInputFolder.Location = new Point(12, 11);
            btnInputFolder.Name = "btnInputFolder";
            btnInputFolder.Size = new Size(158, 23);
            btnInputFolder.TabIndex = 13;
            btnInputFolder.Text = "Select Album Folder";
            btnInputFolder.UseVisualStyleBackColor = true;
            btnInputFolder.Click += btnInputFolder_Click;
            // 
            // btnStartOrganising
            // 
            btnStartOrganising.Enabled = false;
            btnStartOrganising.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnStartOrganising.Location = new Point(12, 72);
            btnStartOrganising.Name = "btnStartOrganising";
            btnStartOrganising.Size = new Size(480, 49);
            btnStartOrganising.TabIndex = 16;
            btnStartOrganising.Text = "Let's-a go!";
            btnStartOrganising.UseVisualStyleBackColor = true;
            btnStartOrganising.Click += btnStartOrganising_Click;
            // 
            // lblInputFolder
            // 
            lblInputFolder.Location = new Point(176, 15);
            lblInputFolder.Name = "lblInputFolder";
            lblInputFolder.Size = new Size(300, 15);
            lblInputFolder.TabIndex = 18;
            lblInputFolder.Text = "No Album Folder Selected";
            // 
            // lblOutputFolder
            // 
            lblOutputFolder.Location = new Point(176, 47);
            lblOutputFolder.Name = "lblOutputFolder";
            lblOutputFolder.Size = new Size(300, 15);
            lblOutputFolder.TabIndex = 19;
            lblOutputFolder.Text = "No Output Folder Selected";
            // 
            // progressBar1
            // 
            progressBar1.Location = new Point(12, 146);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(480, 23);
            progressBar1.TabIndex = 20;
            // 
            // lblVersionString
            // 
            lblVersionString.AutoSize = true;
            lblVersionString.Font = new Font("Segoe UI", 9F, FontStyle.Italic);
            lblVersionString.Location = new Point(390, 124);
            lblVersionString.Name = "lblVersionString";
            lblVersionString.Size = new Size(102, 15);
            lblVersionString.TabIndex = 21;
            lblVersionString.Text = "Version: A1B2C3D";
            // 
            // FormMainWindow
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(504, 181);
            Controls.Add(lblVersionString);
            Controls.Add(progressBar1);
            Controls.Add(lblOutputFolder);
            Controls.Add(lblInputFolder);
            Controls.Add(btnStartOrganising);
            Controls.Add(btnOutputFolder);
            Controls.Add(btnInputFolder);
            Controls.Add(lblStatus);
            Name = "FormMainWindow";
            Text = "NSScreenshotOrganiser";
            MouseMove += FormMainWindow_MouseMove;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button btnOutputFolder;
        private Button btnInputFolder;
        private Label lblInputFolder;
        private Label lblOutputFolder;
        public Label lblStatus;
        public ProgressBar progressBar1;
        public Button btnStartOrganising;
        public Label lblVersionString;
    }
}