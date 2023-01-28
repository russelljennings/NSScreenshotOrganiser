using Newtonsoft.Json;
using System.Diagnostics;
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
                //MessageBox.Show("Unable to load game ID file; one will be generated.");
                SaveGameIds(GenerateGameIdList(), "gameids.json");
            }
            
            Application.Run(new FormMainWindow());
        }

        public static void OrganiseScreenshots(string inputFolderPath, string outputFolderPath, Dictionary<string,string> gameIds, FormMainWindow applicationWindow)
        {
            // Regex strings
            Regex rgImageFileExt = new Regex(@".jpg$");
            Regex rgVideoFileExt = new Regex(@".mp4$");
            Regex rgFoundGameId = new Regex(@"-(.+)\.\w{3}$");
            Regex rgFileName = new Regex(@"\\(\d{16}.+$)");

            // Create a list of all .jpg and .mp4 files in selected folder.
            List<string> inputFiles = Directory.GetFiles(inputFolderPath, "*.*", SearchOption.AllDirectories)
                .Where(file => new string[] {".jpg", ".mp4"}
                .Contains(Path.GetExtension(file)))
                .ToList();

            applicationWindow.progressBar1.Maximum = inputFiles.Count;
            applicationWindow.progressBar1.Value = 0;

            // Iterate through each file found:
            foreach (string file in inputFiles)
            {
                applicationWindow.progressBar1.Value++;
                //Get GameID and FileType from file
                Match fileGameId = rgFoundGameId.Match(file);
                Match fileImageExtension = rgImageFileExt.Match(file);
                Match fileVideoExtension = rgVideoFileExt.Match(file);
                Match fileName = rgFileName.Match(file);
                String foundGameId = fileGameId.ToString().Replace("-", "").Replace(".mp4", "").Replace(".jpg", "");

                //Check GameID against list
                // If known
                if (gameIds.ContainsKey(foundGameId)) 
                {
                    // Check for game folder, create if needed
                    var fileOutputDirectory = outputFolderPath + "\\" + gameIds[foundGameId] + "\\" + fileName;

                    var fileOutputDirectoryCheck = new FileInfo(fileOutputDirectory);
                    if (fileOutputDirectoryCheck.Exists == false)
                    {
                        fileOutputDirectoryCheck.Directory.Create();
                    }

                    try // Copy file to game folder
                    {
                        Debug.WriteLine("Attempting copy of '" + file + "' to '" + fileOutputDirectory + "'.");
                        File.Copy(file, fileOutputDirectory);
                    }
                    catch (IOException){
                        Debug.WriteLine("File '" + file + "' already exists in destination.");
                    } catch (Exception ex)
                    {
                        MessageBox.Show("Was unable to copy '" + file + "' to '" + fileOutputDirectory + "': " + ex.Message);
                    }
                    

                    
                } else // If GameID is unknown
                {
                    FormNewGameFound formNewGameFound = new FormNewGameFound();
                    //  Open Form with Screenshot, or in the case of video, direct link to file so user can play it.
                    try
                    {
                        formNewGameFound.lblFilePath.Text = file;
                        
                    } catch
                    {
                        
                    }
                    if (Path.GetExtension(file) == ".mp4"){
                        formNewGameFound.lblVideoPlayerNotImplemented.Visible = true;
                    } else
                    {
                        formNewGameFound.lblVideoPlayerNotImplemented.Visible = false;
                        formNewGameFound.pictureBox1.ImageLocation = file;
                    }
                    formNewGameFound.textBox1.Enabled = true;
                    formNewGameFound.btnSkipNewGameTitle.Enabled = true;
                    formNewGameFound.btnSubmitNewGameTitle.Enabled = true;
                    formNewGameFound.btnOpenFile.Enabled = true;
                    formNewGameFound.NewGameId = foundGameId;

                    formNewGameFound.ShowDialog();
                    if(formNewGameFound.DialogResult.ToString() == "OK")
                    {
                        //MessageBox.Show("Adding new ID '"+ foundGameId + "' as '" + formNewGameFound.newGameTitle + "'.");
                        gameIds.Add(foundGameId, formNewGameFound.newGameTitle);
                        SaveGameIds(gameIds, "gameids.json");
                        //var string newgame = formNewGameFound.newGameTitle;
                    } else if (formNewGameFound.DialogResult.ToString() == "Continue" || formNewGameFound.DialogResult.ToString() == "Cancel")
                    {

                    } else
                    {
                        // what.
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

            MessageBox.Show("Copy complete!");
        }

        public static bool GameIdExists(string gameId, Dictionary<string, string> gameIdDb)
        {
            return gameIdDb.ContainsKey(gameId);
        }

        public static Dictionary<string,string>? LoadGameIds(string gameIdFilePath)
        {
            try
            {
                var gameIdFileContent = File.ReadAllText(gameIdFilePath);
            } catch
            {
                // catch for exception: file doesn't exist
                // - Generate one. Store a few popular gameIDs here.

                // catch for exception: unable to load
                //MessageBox.Show("Failed to load gameids.json");

            }
            var gameIds = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(gameIdFilePath));
            return gameIds;
        }

        // Returns a small GameId list with popular games.
        public static Dictionary<string, string>? GenerateGameIdList()
        {
            Dictionary<string, string> gameIds = new Dictionary<string, string>();
            gameIds.Add("02CB906EA538A35643C1E1484C4B947D", "Animal Crossing - New Horizons");
            gameIds.Add("397A963DA4660090D65D330174AC6B04", "Splatoon 2");
            gameIds.Add("4CE9651EE88A979D41F24CE8D6EA1C23", "Splatoon 3");
            return gameIds;
        }

        public static void SaveGameIds(Dictionary<string, string> gameIdDictionary, string gameIdFilePath)
        {
            try
            {
                string gameIdJson = JsonConvert.SerializeObject(gameIdDictionary);
                File.WriteAllText(gameIdFilePath, gameIdJson);
            } catch
            {
                MessageBox.Show("Unable to save gameids.json");
            }
        }

    }

}