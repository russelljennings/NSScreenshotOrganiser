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
            if(lblInputFolder.Text != "No Album Folder Selected" && lblOutputFolder.Text != "No Output Folder Selected")
            {
                btnStartOrganising.Enabled = true;
            } else
            {
                btnStartOrganising.Enabled = false;
            }
        }

        private void lblStatus_Click(object sender, EventArgs e)
        {
            FormGameListView formGameListView = new FormGameListView();
            formGameListView.ShowDialog();
        }
    }
}