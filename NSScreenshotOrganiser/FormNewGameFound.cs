using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NSScreenshotOrganiser_WinFormGui
{
    public partial class FormNewGameFound : Form
    {
        public string NewGameId { get; set; }

        public FormNewGameFound()
        {
            InitializeComponent();
        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            var processToStart = filePath;
            Process.Start(new ProcessStartInfo(@processToStart) { UseShellExecute = true });
        }

        private void btnSubmitNewGameTitle_Click(object sender, EventArgs e)
        {
            this.newGameTitle = txtGameTitle.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCopyID_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(NewGameId);
            MessageBox.Show($"Game ID '{NewGameId}' copied to clipboard.");
        }
    }
}
