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
            this.lblStatus = new System.Windows.Forms.Label();
            this.btnOutputFolder = new System.Windows.Forms.Button();
            this.btnInputFolder = new System.Windows.Forms.Button();
            this.btnStartOrganising = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lblInputFolder = new System.Windows.Forms.Label();
            this.lblOutputFolder = new System.Windows.Forms.Label();
            this.txtNewGameTitle = new System.Windows.Forms.TextBox();
            this.lblGuide = new System.Windows.Forms.Label();
            this.btnSubmitNewGameTitle = new System.Windows.Forms.Button();
            this.btnSkipNewGameTitle = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point);
            this.lblStatus.Location = new System.Drawing.Point(190, 124);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(121, 15);
            this.lblStatus.TabIndex = 0;
            this.lblStatus.Text = "0 known titles loaded.";
            this.lblStatus.Click += new System.EventHandler(this.lblStatus_Click);
            this.lblStatus.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lblTitleDetectionCount_MouseMove);
            // 
            // btnOutputFolder
            // 
            this.btnOutputFolder.Location = new System.Drawing.Point(12, 43);
            this.btnOutputFolder.Name = "btnOutputFolder";
            this.btnOutputFolder.Size = new System.Drawing.Size(158, 23);
            this.btnOutputFolder.TabIndex = 14;
            this.btnOutputFolder.Text = "Select Output Folder";
            this.btnOutputFolder.UseVisualStyleBackColor = true;
            this.btnOutputFolder.Click += new System.EventHandler(this.btnOutputFolder_Click);
            // 
            // btnInputFolder
            // 
            this.btnInputFolder.Location = new System.Drawing.Point(12, 11);
            this.btnInputFolder.Name = "btnInputFolder";
            this.btnInputFolder.Size = new System.Drawing.Size(158, 23);
            this.btnInputFolder.TabIndex = 13;
            this.btnInputFolder.Text = "Select Album Folder";
            this.btnInputFolder.UseVisualStyleBackColor = true;
            this.btnInputFolder.Click += new System.EventHandler(this.btnInputFolder_Click);
            // 
            // btnStartOrganising
            // 
            this.btnStartOrganising.Enabled = false;
            this.btnStartOrganising.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnStartOrganising.Location = new System.Drawing.Point(12, 72);
            this.btnStartOrganising.Name = "btnStartOrganising";
            this.btnStartOrganising.Size = new System.Drawing.Size(480, 49);
            this.btnStartOrganising.TabIndex = 16;
            this.btnStartOrganising.Text = "Let\'s-a go!";
            this.btnStartOrganising.UseVisualStyleBackColor = true;
            this.btnStartOrganising.Click += new System.EventHandler(this.btnStartOrganising_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Silver;
            this.pictureBox1.Location = new System.Drawing.Point(12, 191);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(480, 272);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 17;
            this.pictureBox1.TabStop = false;
            // 
            // lblInputFolder
            // 
            this.lblInputFolder.Location = new System.Drawing.Point(176, 15);
            this.lblInputFolder.Name = "lblInputFolder";
            this.lblInputFolder.Size = new System.Drawing.Size(300, 15);
            this.lblInputFolder.TabIndex = 18;
            this.lblInputFolder.Text = "No Album Folder Selected";
            // 
            // lblOutputFolder
            // 
            this.lblOutputFolder.Location = new System.Drawing.Point(176, 47);
            this.lblOutputFolder.Name = "lblOutputFolder";
            this.lblOutputFolder.Size = new System.Drawing.Size(300, 15);
            this.lblOutputFolder.TabIndex = 19;
            this.lblOutputFolder.Text = "No Output Folder Selected";
            // 
            // txtNewGameTitle
            // 
            this.txtNewGameTitle.Enabled = false;
            this.txtNewGameTitle.Location = new System.Drawing.Point(12, 162);
            this.txtNewGameTitle.Name = "txtNewGameTitle";
            this.txtNewGameTitle.Size = new System.Drawing.Size(318, 23);
            this.txtNewGameTitle.TabIndex = 20;
            // 
            // lblGuide
            // 
            this.lblGuide.AutoSize = true;
            this.lblGuide.Enabled = false;
            this.lblGuide.Location = new System.Drawing.Point(12, 144);
            this.lblGuide.Name = "lblGuide";
            this.lblGuide.Size = new System.Drawing.Size(152, 15);
            this.lblGuide.TabIndex = 21;
            this.lblGuide.Text = "Enter name of below game:";
            // 
            // btnSubmitNewGameTitle
            // 
            this.btnSubmitNewGameTitle.Enabled = false;
            this.btnSubmitNewGameTitle.Location = new System.Drawing.Point(336, 162);
            this.btnSubmitNewGameTitle.Name = "btnSubmitNewGameTitle";
            this.btnSubmitNewGameTitle.Size = new System.Drawing.Size(75, 23);
            this.btnSubmitNewGameTitle.TabIndex = 22;
            this.btnSubmitNewGameTitle.Text = "Submit";
            this.btnSubmitNewGameTitle.UseVisualStyleBackColor = true;
            // 
            // btnSkipNewGameTitle
            // 
            this.btnSkipNewGameTitle.Enabled = false;
            this.btnSkipNewGameTitle.Location = new System.Drawing.Point(417, 162);
            this.btnSkipNewGameTitle.Name = "btnSkipNewGameTitle";
            this.btnSkipNewGameTitle.Size = new System.Drawing.Size(75, 23);
            this.btnSkipNewGameTitle.TabIndex = 23;
            this.btnSkipNewGameTitle.Text = "Skip";
            this.btnSkipNewGameTitle.UseVisualStyleBackColor = true;
            // 
            // FormMainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(504, 475);
            this.Controls.Add(this.btnSkipNewGameTitle);
            this.Controls.Add(this.btnSubmitNewGameTitle);
            this.Controls.Add(this.lblGuide);
            this.Controls.Add(this.txtNewGameTitle);
            this.Controls.Add(this.lblOutputFolder);
            this.Controls.Add(this.lblInputFolder);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btnStartOrganising);
            this.Controls.Add(this.btnOutputFolder);
            this.Controls.Add(this.btnInputFolder);
            this.Controls.Add(this.lblStatus);
            this.Name = "FormMainWindow";
            this.Text = "NSScreenshotOrganiser";
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.FormMainWindow_MouseMove);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Button btnOutputFolder;
        private Button btnInputFolder;
        private Button btnStartOrganising;
        private Label lblInputFolder;
        private Label lblOutputFolder;
        public Label lblStatus;
        public PictureBox pictureBox1;
        public TextBox txtNewGameTitle;
        public Label lblGuide;
        public Button btnSubmitNewGameTitle;
        public Button btnSkipNewGameTitle;
    }
}