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
            } catch (Exception ex)
            {
                MessageBox.Show("Was unable to load gameids.json: " + ex.Message);
            }

            Application.Run(new FormMainWindow());
            
        }

        public static void OrganiseScreenshots(string inputFolderPath, string outputFolderPath, Dictionary<string,string> gameIds, FormMainWindow applicationWindow)
        {
            // Regex strings
            Regex rgImageFileExt = new(@".jpg$");
            Regex rgVideoFileExt = new(@".mp4$");
            Regex rgFoundGameId = new(@"-(.+)\.\w{3}$");
            Regex rgFileName = new(@"\\(\d{16}.+$)");

            // Create a list of all .jpg and .mp4 files in selected folder.
            List<string> inputFiles = Directory.GetFiles(inputFolderPath, "*.*", SearchOption.AllDirectories)
                .Where(file => new string[] {".jpg", ".mp4"}
                .Contains(Path.GetExtension(file)))
                .ToList();

            // Reset progress bar.
            applicationWindow.progressBar1.Maximum = inputFiles.Count;
            applicationWindow.progressBar1.Value = 0;

            // Disable button until complete.
            applicationWindow.btnStartOrganising.Enabled = false;

            // For each file in Album folder
            foreach (string file in inputFiles)
            {
                // Update progress bar value.
                applicationWindow.progressBar1.Value++;

                // Update text progress value.
                applicationWindow.lblStatus.Text = "Processing file '" + applicationWindow.progressBar1.Value + "' of '" + inputFiles.Count + "'.";

                // Get GameID and extension from file
                Match fileName = rgFileName.Match(file);
                Match fileGameId = rgFoundGameId.Match(file);
                Match fileImageExtension = rgImageFileExt.Match(file);
                Match fileVideoExtension = rgVideoFileExt.Match(file);
                String foundGameId = fileGameId.ToString().Replace("-", "").Replace(".mp4", "").Replace(".jpg", "");

                // Check GameID against dictionary
                // If game is known
                if (gameIds.ContainsKey(foundGameId)) 
                {
                    
                    // Check for game folder, create if needed
                    var fileOutputDirectory = outputFolderPath + "\\" + gameIds[foundGameId] + "\\" + fileName;

                    // Attempt to copy file.
                    try
                    {
                        CopyMedia(file, fileOutputDirectory);
                    } catch (Exception ex)
                    {
                        MessageBox.Show("Was unable to copy '" + file + "' to '" + fileOutputDirectory + "': " + ex.Message);
                        break;
                    }

                    
                } else // GameID is unknown
                {
                    // Prepare NewGameFound Form
                    FormNewGameFound formNewGameFound = new()
                    {
                        // Define the file for the Form
                        filePath = file,
                        NewGameId = foundGameId
                    };
                    
                    // If file is an image, display preview.
                    if (Path.GetExtension(file) == ".jpg"){
                        formNewGameFound.lblVideoPlayerNotImplemented.Visible = false;
                        formNewGameFound.pictureBox1.ImageLocation = file;
                    } // If file is a video, display prompt to open file. 
                    else if (Path.GetExtension(file) == ".mp4"){
                        formNewGameFound.lblVideoPlayerNotImplemented.Visible = true;
                    }

                    // Show NewGameFound Form
                    formNewGameFound.ShowDialog();

                    // If game title is supplied, add to gameids.json for future use.
                    // Then attempt to copy the file.
                    if(formNewGameFound.DialogResult.ToString() == "OK")
                    { 
                        gameIds.Add(foundGameId, formNewGameFound.newGameTitle);
                        SaveGameIds(gameIds, "gameids.json");

                        // Check for game folder, create if needed
                        var fileOutputDirectory = outputFolderPath + "\\" + gameIds[foundGameId] + "\\" + fileName;
                        // Attempt to copy file.
                        try
                        {
                            CopyMedia(file, fileOutputDirectory);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Was unable to copy '" + file + "' to '" + fileOutputDirectory + "': " + ex.Message);
                            break;
                        }

                    } // If user clicks 'Skip', move on to next one.
                    else if (formNewGameFound.DialogResult.ToString() == "Continue")
                    {

                    } // If user closes the dialogue box, cancel the process.
                    else if (formNewGameFound.DialogResult.ToString() == "Cancel")
                    {
                        MessageBox.Show("Stopping copy process.");
                        applicationWindow.lblStatus.Text = "Process cancelled.";
                        break;
                    }
                }
            }

            // Enable button as process is ended.
            applicationWindow.btnStartOrganising.Enabled = true;
            // Reset progress bar.
            applicationWindow.progressBar1.Value = 0;
        }

        // Attempts to copy a given file. 
        // Will handle if the file already exists in destination, but will return any other exceptions.
        public static void CopyMedia(string filePath, string outputFolderPath)
        {
            var fileOutputDirectoryCheck = new FileInfo(outputFolderPath);
            if (fileOutputDirectoryCheck.Exists == false)
            {
                fileOutputDirectoryCheck.Directory.Create();
            }

            // Attempt to copy file to game folder
            try
            {
                Debug.WriteLine("Attempting copy of '" + filePath + "' to '" + outputFolderPath + "'.");
                File.Copy(filePath, outputFolderPath);
            } // If file already exists in Output, move on to next one.
            catch (IOException)
            {
                Debug.WriteLine("File '" + filePath + "' already exists in destination.");
            }
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
            }
            catch (FileNotFoundException)
            {
                Debug.WriteLine("Unable to find 'gameids.json', attempting to generate.");
                SaveGameIds(GenerateGameIdList(), "gameids.json");

            } catch (IOException ex) {
                Debug.WriteLine("Unable to load 'gameids.json', because '"+ ex.Message +"'.");

            } catch (Exception ex) {
                MessageBox.Show("Failed to load gameids.json: '" + ex.Message + "'");
                return null;
            }

            var gameIds = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(gameIdFilePath));

            return gameIds;
        }

        // Returns a small GameId list with popular games.
        public static Dictionary<string, string>? GenerateGameIdList()
        {
            Dictionary<string, string> gameIds = new()
            {
                { "02CB906EA538A35643C1E1484C4B947D", "Animal Crossing - New Horizons" },
                { "16851BE00BC6068871FE49D98876D6C5", "Mario Kart 8 Deluxe" },
                { "397A963DA4660090D65D330174AC6B04", "Splatoon 2" },
                { "4CE9651EE88A979D41F24CE8D6EA1C23", "Splatoon 3" }
            };

            return gameIds;
        }

        public static void SaveGameIds(Dictionary<string, string> gameIdDictionary, string gameIdFilePath)
        {
            try
            {
                string gameIdJson = JsonConvert.SerializeObject(gameIdDictionary);
                File.WriteAllText(gameIdFilePath, gameIdJson);
            } catch (Exception ex)
            {
                MessageBox.Show("Unable to save gameids.json: '" + ex.Message + "'");
            }
        }

    }

}