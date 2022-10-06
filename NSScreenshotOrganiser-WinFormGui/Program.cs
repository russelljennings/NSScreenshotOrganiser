using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace NSScreenshotOrganiser_WinFormGui
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            try
            {
                var loadedGameIds = LoadGameIds("gameids.json");
            } catch
            {
                MessageBox.Show("Unable to load 'gameids.json'.");
            }
            
            Application.Run(new FormMainWindow());
        }

        public static void OrganiseScreenshots(string inputFolderPath, string outputFolderPath, Dictionary<string,string> gameIds, FormMainWindow applicationWindow)
        {
            // Regex strings
            Regex rgImageFileExt = new Regex(@".jpg$");
            Regex rgVideoFileExt = new Regex(@".mp4$");
            Regex rgFoundGameId = new Regex(@"-(.+)\.\w{3}$");
            

            // Create a list of all .jpg and .mp4 files in selected folder.
            List<string> inputFiles = Directory.GetFiles(inputFolderPath, "*.*", SearchOption.AllDirectories)
                .Where(file => new string[] {".jpg", ".mp4"}
                .Contains(Path.GetExtension(file)))
                .ToList();

            foreach (string file in inputFiles)
            {
                //Get GameID and FileType from file
                Match fileGameId = rgFoundGameId.Match(file);
                Match fileImageExtension = rgImageFileExt.Match(file);
                Match fileVideoExtension = rgVideoFileExt.Match(file);

                //Check GameID against list
                if (gameIds.ContainsKey(fileGameId.Value)) // If known
                {
                    applicationWindow.pictureBox1.ImageLocation = file;
                    MessageBox.Show($"Game detected: '{gameIds[fileGameId.Value]}'");
                    // Check for game folder, create if needed
                    // Move file to game folder
                } else // If GameID is unknown
                {
                    bool gameIsUnknown = true;
                    //  Open Form with Screenshot, or in the case of video, direct link to file so user can play it.
                    try
                    {
                        applicationWindow.pictureBox1.ImageLocation = file;
                    } catch
                    {
                        //applicationWindow.pictureBox1.Click += new System.EventHandler(btnStartOrganising_Click);
                    }
                    while (gameIsUnknown)
                    {
                        applicationWindow.lblStatus.Text = $"New game who this: {file}";
                        applicationWindow.txtNewGameTitle.Enabled = true;
                        applicationWindow.btnSkipNewGameTitle.Enabled = true;
                        applicationWindow.btnSubmitNewGameTitle.Enabled = true;
                    }

                }



                //  If user enters game name and clicks Submit
                //    Add game to GameIds, and perform actions as known.

                //  If user clicks Skip
                //    Ignore that GameId this run.

                //If GameID is known
                //  Check if GameName folder exists, create if not

                //  Move file to GameName folder
            }
        }

        public static Dictionary<string,string>? LoadGameIds(string gameIdFilePath)
        {
            try
            {
                var gameIdFileContent = File.ReadAllText(gameIdFilePath);
                var gameIds = JsonConvert.DeserializeObject<Dictionary<string, string>>(gameIdFileContent);
                return gameIds;
            } catch
            {
                MessageBox.Show("Failed to load gameids.json");
                return null;
            }
        }

    }

}