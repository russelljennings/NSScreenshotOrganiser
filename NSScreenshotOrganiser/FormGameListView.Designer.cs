namespace NSScreenshotOrganiser_WinFormGui
{
    partial class FormGameListView
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
            this.GameListView = new System.Windows.Forms.ListView();
            this.btnCloseWindow = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // GameListView
            // 
            this.GameListView.AccessibleDescription = "List of known game IDs.";
            this.GameListView.Location = new System.Drawing.Point(12, 12);
            this.GameListView.Name = "GameListView";
            this.GameListView.Size = new System.Drawing.Size(590, 208);
            this.GameListView.TabIndex = 0;
            this.GameListView.UseCompatibleStateImageBehavior = false;
            this.GameListView.View = System.Windows.Forms.View.Details;
            // 
            // btnCloseWindow
            // 
            this.btnCloseWindow.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnCloseWindow.Location = new System.Drawing.Point(271, 226);
            this.btnCloseWindow.Name = "btnCloseWindow";
            this.btnCloseWindow.Size = new System.Drawing.Size(75, 23);
            this.btnCloseWindow.TabIndex = 1;
            this.btnCloseWindow.Text = "Close";
            this.btnCloseWindow.UseVisualStyleBackColor = true;
            this.btnCloseWindow.Click += new System.EventHandler(this.btnCloseWindow_Click);
            // 
            // FormGameListView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(614, 261);
            this.Controls.Add(this.btnCloseWindow);
            this.Controls.Add(this.GameListView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "FormGameListView";
            this.Text = "Loaded Game IDs";
            this.ResumeLayout(false);

        }

        #endregion

        private ListView GameListView;
        private Button btnCloseWindow;
    }
}