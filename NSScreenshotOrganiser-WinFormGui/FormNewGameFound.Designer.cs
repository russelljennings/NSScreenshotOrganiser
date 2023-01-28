namespace NSScreenshotOrganiser_WinFormGui
{
    partial class FormNewGameFound
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
            this.lblFilePath = new System.Windows.Forms.Label();
            this.btnOpenFile = new System.Windows.Forms.Button();
            this.btnSkipNewGameTitle = new System.Windows.Forms.Button();
            this.btnSubmitNewGameTitle = new System.Windows.Forms.Button();
            this.lblGuide = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btnCopyID = new System.Windows.Forms.Button();
            this.lblVideoPlayerNotImplemented = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // lblFilePath
            // 
            this.lblFilePath.AutoSize = true;
            this.lblFilePath.Location = new System.Drawing.Point(222, 5);
            this.lblFilePath.Name = "lblFilePath";
            this.lblFilePath.Size = new System.Drawing.Size(0, 15);
            this.lblFilePath.TabIndex = 32;
            this.lblFilePath.Visible = false;
            // 
            // btnOpenFile
            // 
            this.btnOpenFile.Enabled = false;
            this.btnOpenFile.Location = new System.Drawing.Point(12, 48);
            this.btnOpenFile.Name = "btnOpenFile";
            this.btnOpenFile.Size = new System.Drawing.Size(75, 23);
            this.btnOpenFile.TabIndex = 31;
            this.btnOpenFile.Text = "Open File";
            this.btnOpenFile.UseVisualStyleBackColor = true;
            this.btnOpenFile.Click += new System.EventHandler(this.btnOpenFile_Click);
            // 
            // btnSkipNewGameTitle
            // 
            this.btnSkipNewGameTitle.DialogResult = System.Windows.Forms.DialogResult.Continue;
            this.btnSkipNewGameTitle.Enabled = false;
            this.btnSkipNewGameTitle.Location = new System.Drawing.Point(417, 48);
            this.btnSkipNewGameTitle.Name = "btnSkipNewGameTitle";
            this.btnSkipNewGameTitle.Size = new System.Drawing.Size(75, 23);
            this.btnSkipNewGameTitle.TabIndex = 30;
            this.btnSkipNewGameTitle.Text = "Skip";
            this.btnSkipNewGameTitle.UseVisualStyleBackColor = true;
            // 
            // btnSubmitNewGameTitle
            // 
            this.btnSubmitNewGameTitle.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnSubmitNewGameTitle.Enabled = false;
            this.btnSubmitNewGameTitle.Location = new System.Drawing.Point(336, 48);
            this.btnSubmitNewGameTitle.Name = "btnSubmitNewGameTitle";
            this.btnSubmitNewGameTitle.Size = new System.Drawing.Size(75, 23);
            this.btnSubmitNewGameTitle.TabIndex = 29;
            this.btnSubmitNewGameTitle.Text = "Submit";
            this.btnSubmitNewGameTitle.UseVisualStyleBackColor = true;
            this.btnSubmitNewGameTitle.Click += new System.EventHandler(this.btnSubmitNewGameTitle_Click);
            // 
            // lblGuide
            // 
            this.lblGuide.AutoSize = true;
            this.lblGuide.Enabled = false;
            this.lblGuide.Location = new System.Drawing.Point(18, 19);
            this.lblGuide.Name = "lblGuide";
            this.lblGuide.Size = new System.Drawing.Size(106, 15);
            this.lblGuide.TabIndex = 28;
            this.lblGuide.Text = "What is this game?";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Silver;
            this.pictureBox1.Location = new System.Drawing.Point(12, 77);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(480, 272);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 26;
            this.pictureBox1.TabStop = false;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(130, 16);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(362, 23);
            this.textBox1.TabIndex = 33;
            // 
            // btnCopyID
            // 
            this.btnCopyID.Location = new System.Drawing.Point(93, 48);
            this.btnCopyID.Name = "btnCopyID";
            this.btnCopyID.Size = new System.Drawing.Size(92, 23);
            this.btnCopyID.TabIndex = 34;
            this.btnCopyID.Text = "Copy GameID";
            this.btnCopyID.UseVisualStyleBackColor = true;
            this.btnCopyID.Click += new System.EventHandler(this.btnCopyID_Click);
            // 
            // lblVideoPlayerNotImplemented
            // 
            this.lblVideoPlayerNotImplemented.Location = new System.Drawing.Point(12, 77);
            this.lblVideoPlayerNotImplemented.Name = "lblVideoPlayerNotImplemented";
            this.lblVideoPlayerNotImplemented.Size = new System.Drawing.Size(480, 272);
            this.lblVideoPlayerNotImplemented.TabIndex = 35;
            this.lblVideoPlayerNotImplemented.Text = "Video previews are not currently implemented.\r\n\r\nPlease click Open File to view.";
            this.lblVideoPlayerNotImplemented.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblVideoPlayerNotImplemented.Visible = false;
            // 
            // FormNewGameFound
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(504, 361);
            this.Controls.Add(this.lblVideoPlayerNotImplemented);
            this.Controls.Add(this.btnCopyID);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.lblFilePath);
            this.Controls.Add(this.btnOpenFile);
            this.Controls.Add(this.btnSkipNewGameTitle);
            this.Controls.Add(this.btnSubmitNewGameTitle);
            this.Controls.Add(this.lblGuide);
            this.Controls.Add(this.pictureBox1);
            this.Name = "FormNewGameFound";
            this.Text = "New Game Found";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public Label lblFilePath;
        public Button btnOpenFile;
        public Button btnSkipNewGameTitle;
        public Button btnSubmitNewGameTitle;
        public Label lblGuide;
        public TextBox txtNewGameTitle;
        public PictureBox pictureBox1;
        public TextBox textBox1;
        private Button btnCopyID;
        public Label lblVideoPlayerNotImplemented;

        public string newGameTitle { get; set; }
    }
}