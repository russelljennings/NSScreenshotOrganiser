using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NSScreenshotOrganiser_WinFormGui
{
    public partial class FormGameListView : Form
    {
        public FormGameListView()
        {
            InitializeComponent();
            var loadedGameIds = Program.LoadGameIds("gameids.json");

            GameListView.Columns.Add("Game Title", -2, HorizontalAlignment.Left);
            GameListView.Columns.Add("ID", -2, HorizontalAlignment.Left);

            foreach (KeyValuePair<string, string> kvp in loadedGameIds)
            {
                var gameIdItem = new ListViewItem(new[] { kvp.Value, kvp.Key});
                GameListView.Items.Add(gameIdItem); 
            }
            
        }

        private void btnCloseWindow_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
