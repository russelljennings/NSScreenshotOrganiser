using System.Diagnostics;

namespace NSScreenshotOrganiser_WinFormGui
{
    public partial class FormMainWindow : Form
    {
        public FormMainWindow()
        {
            InitializeComponent();
            var loadedGameIds = Program.LoadGameIds("gameids.json");
            lblStatus.Text = $"{loadedGameIds.Count.ToString()} known titles loaded.";
        }

        private void btnShowConfigForm_Click(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnInputFolder_Click(object sender, EventArgs e)
        {
            var inputFolderForm = new FolderBrowserDialog();
            if (inputFolderForm.ShowDialog() == DialogResult.OK)
            {
                lblInputFolder.Text = inputFolderForm.SelectedPath;
            }
        }
        private void btnOutputFolder_Click(object sender, EventArgs e)
        {
            var outputFolderForm = new FolderBrowserDialog();
            if (outputFolderForm.ShowDialog() == DialogResult.OK)
            {
                lblOutputFolder.Text = outputFolderForm.SelectedPath;
            }
        }

        private void btnStartOrganising_Click(object sender, EventArgs e)
        {
            Program.OrganiseScreenshots(lblInputFolder.Text, lblOutputFolder.Text, Program.LoadGameIds("gameids.json"), this);
        }

        private void lblTitleDetectionCount_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void FormMainWindow_MouseMove(object sender, EventArgs e)
        {
            // Check if Input and Output folders are set.
            // If they're equal or if Output is inside Input, keep Start button disabled or else it'll loop forever.
            if (lblInputFolder.Text == lblOutputFolder.Text)
            {
                btnStartOrganising.Enabled = false;
            } // If either are in init state with no folder selected, keep Start button disabled until both are chosen.
            else if (lblInputFolder.Text != "No Album Folder Selected" && lblOutputFolder.Text != "No Output Folder Selected")
            {
                btnStartOrganising.Enabled = true;
            } // Enable Start button.
            else 
            {
                btnStartOrganising.Enabled = false;
            }
        }

        private void lblStatus_Click(object sender, EventArgs e)
        {
            FormGameListView formGameListView = new();
            formGameListView.ShowDialog();
        }

        private void btnSubmitNewGameTitle_Click(object sender, EventArgs e)
        {

        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            //Process.Start(lblFilePath.Text);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}