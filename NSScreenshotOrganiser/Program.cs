using Newtonsoft.Json;
using System.Diagnostics;
using System.Reflection;
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
                MessageBox.Show("Failed to load game IDs: '" + ex.Message + "'");
                return null;
            }

            var gameIds = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(gameIdFilePath));

            return gameIds;
        }

        // Returns a GameId list with some known games.
        public static Dictionary<string, string>? GenerateGameIdList()
        {
            Dictionary<string, string> gameIds = new()
            {
                { "57B4628D2267231D57E0FC1078C0596D", "Nintendo Switch" },
                { "CCFA659F4857F96DDA29AFEDB2E166E6", "Nintendo Switch" },
                { "50E2A11CE4BDDC72EF99DF78315D4938", "Nintendo Switch" },
                { "0C7E8798D2996597BAFD59B427BF6132", "Nintendo Switch" },
                { "1E95E5926F1CB99A87326D927F27B47E", "Nintendo Switch" },
                { "CF99E5A89997DEB91E571D16DB468578", "Nintendo Switch" },
                { "1AB131B6E6571375B79964211BB3F5AE", "Nintendo Switch" },
                { "A6DC14A9679BC66A83D595C7ED85B90E", "Mii Maker" },
                { "2B1F1288BC05B2D89D8431910DBA2878", "1-2-Switch" },
                { "A21E9AB1D7B96CA3C9DA5395D3D5A2A7", "51 Worldwide Games" },
                { "BA38CF6E861CAA312652C2296B28927C", "911 Operator" },
                { "83E7DB95268C5F43C3DEE7171F4FA3A8", "A Short Hike" },
                { "4CDB566B5D58A9E89C95E6A7529CA373", "A Fold Apart" },
                { "CAB4404D65FCA3773EAC39336F972074", "A Highland Song" },
                { "AC9EF172FDFECCD3126D6BEBFD24F94E", "A Memoir Blue" },
                { "C2D7B57FEA57B065055F8BECA79AB652", "A Winding Path" },
                { "D214335DF82E2CAAA9644B8D94EFC39F", "ABZU" },
                { "AC0F641AF6D3A018CC6910B1787620CB", "ACA NEOGEO Money Puzzle Exchanger" },
                { "0E9A64572AD9F817DE79A1B7DE96BA38", "Aegis Defenders" },
                { "4BA12B9378CF5ECE20770DD05536F626", "A Knight's Quest" },
                { "17A369D078429851D86294523F96554C", "Alien - Isolation" },
                { "ADE8B97BC8F67AD42B03EE7E31948F03", "Alwa's Awakening" },
                { "98237C399FCCC779C4B302EE6433DA5B", "Alwa's Legacy" },
                { "02CB906EA538A35643C1E1484C4B947D", "Animal Crossing - New Horizons" },
                { "985A2F024CB5EE9542FCD02B830B92B5", "Anyaroth - The Queen's Tyranny" },
                { "3E83999CFE72273C52897E706E694A54", "Ape Out" },
                { "CE54AF423EC089F7262EAC799A1E4540", "Aqua Kitty" },
                { "A67942211CD1A968913B304D86B5486A", "Arena of Valor" },
                { "412E0591DD033E78737A4B6DC2C70E50", "ARMS - Global Testpunch" },
                { "5175A9E8354E328724729A6641D0F22F", "ARMS" },
                { "34810733B9A0D51B3627F8CD84EC366F", "Art of Balance" },
                { "B14300D92ED6F8D3118E6D3BB07BF40E", "Asphalt 8" },
                { "23A6B5C3A1B8F8267A0DC91495AC1101", "Asterix & Obelix XXL2" },
                { "11AA966B116F8045D7A96D89A1C3FC19", "Asterix & Obelix XXL3: The Crystal Menhir" },
                { "257FD939428E4BFE6BF9E2F559D5037A", "Astral Chain" },
                { "C2D43B76F383A86B8EFE30B43FA207C5", "Attack on Titan 2 - Demo" },
                { "8A02A443096C4A41164DDD7DD5CF0967", "Automachef" },
                { "D6F3EB1178A90392C0B8A57476DADED0", "Axiom Verge" },
                { "C0A771DA35FEA8B9400647F18EA0169B", "Axiom Verge" },
                { "3D00A4AF4103506994D774CE8D4997BA", "Axiom Verge 2" },
                { "F639752F5FA028761EF9C25DB534FD07", "Azure Striker Gunvolt Striker Pack" },
                { "EAD9CDD267F449963C89657382E411AE", "Baba is You" },
                { "56A9001957E1FA9E6227F5C3F7F107DC", "Banner Saga 2" },
                { "39D29C8CA48151DFC2610042158503F2", "Bastion" },
                { "372BD77417B847E4D7050826C7C9B60F", "Battle Chasers Nightwar" },
                { "79EC1189088E767339D21942767BCF2E", "Battle Chef Brigade" },
                { "39044917CFF4239A6A8855A4FBFD24A3", "Bayonetta" },
                { "E27E5ADA5A86332E7C52B3562FCF5A27", "Bayonetta 2" },
                { "B554C648DDBE3A62C689D8D6F9AF5E45", "Bayonetta 3" },
                { "03465AEE7867A537C204A55FF938E34F", "Big Brain Academy - Brain vs. Brain" },
                { "BEB971B332CC1E976C3E50F87F99A4A0", "Blanc" },
                { "54D423DA5645E6DFF84020D96E3F2FEA", "Blasphemous - Demo" },
                { "603687AF492168C0AF7EC16CF33BED27", "Blaster Master Zero" },
                { "CD1FE00B5BD15B5DF513A8F56DF39FE3", "Blaster Master Zero - Demo" },
                { "5D7B299B19405E56EBE28CDE9FB8776A", "BlazBlue Cross-Tag Battle Beta" },
                { "E84DDD08AF543D1912A44E015F43C8B8", "Bleed 2" },
                { "B698A8BAECF292B9E0FDF66A777A9A9B", "Bloodstained Curse of the Moon" },
                { "F95B57762E79CABDA198AE6BBDC6C7D5", "Bloodstained Ritual of the Night" },
                { "4EFFA08DBFD74F27E0CE48F80F7A3379", "Blossom Tales - The Sleeping King" },
                { "6EDB189E502491BA0A0696C9D03A8FEE", "Borderlands 2" },
                { "87F15DF5B1168A79BAD3D501C9C0B14D", "Borderlands The Pre-Sequel" },
                { "2674F78B40BC97F0AE6BE0A4504CF33C", "Boyfriend Dungeon" },
                { "7A63035F5195A1D4C7928CC7518654C5", "Bridge Constructor Portal" },
                { "13BBCD125931ACA6E654AE761A3B927E", "Broforce" },
                { "E733C7B7CE26121F1DA0EA525A683779", "Bug Fables: The Everlasting Sapling" },
                { "692A036ACD6F85F010A46CF2B3F28834", "BUTCHER" },
                { "CCFE55975D68069E990E9064D7B828A9", "Cadence of Hyrule" },
                { "FE18FAEF72D654FB0AB3E66905246B60", "Cadence of Hyrule - Demo" },
                { "A6C056CABE0E1894654A3769FAF6D11E", "Captain Toad's Treasure Tracker" },
                { "D27D9A7941148AAB41D9F68B6E983975", "Captain Toad's Treasure Tracker - Demo" },
                { "FEC0D49D49E3AD5EC31C4029E4886D42", "Carto" },
                { "29DAC76F23A7892B4AE05C9C1BC67E22", "Castlevania Anniversary Collection" },
                { "7A5825B6334E63AF2B3C3BE850F1F62C", "Cat Quest" },
                { "D54AE256479D4E828ED6C1522C80697F", "Cat Quest II" },
                { "CFADE8C5F184728CD4776748B0E825D0", "Cathedral" },
                { "93FA958835AC3573C5186D5F5B0DB6B2", "Cave Story+" },
                { "75A32021BE3512D7AA96B2D72F764411", "Celeste" },
                { "5855558AA8FCD2E87BC21377436CFBF0", "Child of Light" },
                { "FEDDFB3BEB94843E6A8D2AF517CAE725", "Chocobo's Mystery Dungeon EVERY BUDDY" },
                { "BBBCE85F6EBFFC9C493F35F910EE935B", "ChromaGun - Demo" },
                { "79127164CA4A94F74187421B24A3AA8D", "Clubhouse Games 51 Worldwide Classics" },
                { "03F6D2DE86ACBC923C87A67D746E299C", "Clustertruck" },
                { "5AA6D6375173E472AF10BCBECFBA64F2", "Coffee Talk" },
                { "A6239926D03D31A6BB9D678AE59FBCCF", "Coffee Talk" },
                { "48A5A467593C21DFC93CCCF18D91FCE1", "Collection of Mana" },
                { "12450B0C782F38E35EF964906DB929D1", "Contra Anniversary Collection" },
                { "8473AB9A4E611AD942F3847AFE12FC7E", "Contra Operation Galuga Demo" },
                { "2FEDAF60AFC4B5A34F89ED5106B593A9", "Control Ultimate Edition - Cloud Version" },
                { "47DFA09BC4BB9CD2BF4E3159B178F3DC", "Cozy Grove" },
                { "3D8E1DE4D671F7453AFA0C395B825E90", "Crash Bandicoot N. Sane Trilogy" },
                { "AD808EFD2C53DCD6F8380113B64A0DD4", "Crash Team Racing Nitro-Fueled" },
                { "2D8807B3E76391BC96950ACB6CBB7481", "Cris Tales Demo" },
                { "55D4D0C8726FCBE734083765E1B29C71", "Cris Tales" },
                { "AD0B3B2F03800567E0E43C7157F8D14F", "CrossCode" },
                { "608312D02B18D5938B89BF5F1CF66177", "CROSSNIQ+" },
                { "5C78065CD98929D80FE9662AC4A6DDA2", "Crypt of the Necrodancer" },
                { "2772FDA84EC93AB84F7A7E900518B0E5", "Cyber Shadow" },
                { "D30E1C2C7666B65122781E72D3DC0467", "Cuphead" },
                { "CB3ABE0C3F62222914A4EB66C1A23B40", "Cursed Castilla" },
                { "9BB7C01E75A2A0FD6AD24165CAB02257", "Daemon X Machina - Demo" },
                { "EA5CEAD0C10C4580E4DCED858DDFCF69", "Dandara" },
                { "530067909FDC14FC5B146A2E7D1BE61F", "Danmaku Unlimited 3" },
                { "FDDDF51EBA3E388905A630B252B3F437", "Dark Souls Remastered - Beta" },
                { "527D63A98D52DBC42C0498FE710B325C", "Dauntless" },
                { "3A2955F9D12BBF56D01A07205E7F1E1D", "Dead Cells" },
                { "098161C74795DA7A0990D4F403EAC7C1", "Death Squared" },
                { "C42C62E3B4D89FA2408E162AB37BD278", "Death Squared - Demo" },
                { "B2AA81D02C39303EBD04D8D7F246F34A", "Deltarune" },
                { "411DDB1D4AF251569DFB4018C3679298", "Desert Child" },
                { "7BB8CB96FC4C0C4552E6E7F427383086", "Dex" },
                { "3D69A7ED02A1FF371048829E22A49194", "Diablo III" },
                { "79B02B5BA372774567A5DE1F7B872776", "Dicey Dungeons" },
                { "E2AB4F21DB23C6193556007948103D50", "Digimon Story Cyber Sleuth - Complete Edition" },
                { "D31988949C609FC3B6FD6AEFE9830EBE", "Disgaea 5 Complete" },
                { "EEC5FC79B39E6B34B03DC025D17F73F1", "Disgaea 5 Complete" },
                { "DF4DD10CFE6EDD53634E7E5973C64297", "Disgaea 5 Complete - Demo" },
                { "BECE05DFCE001945B919C8D6E0258C9D", "Dogurai" },
                { "BD0EB87287646F662EB9875856FE05AB", "Donkey Kong Country Tropical Freeze" },
                { "5C52A99F1B4D0B529AFEB9622AB95568", "Donut County" },
                { "CF035A1DEF1D6DADE285B7ACA9873642", "Doom" },
                { "2D2DD5427221CE0F9F97275FAAF2BD60", "Doom (1993)" },
                { "EA525566BC202E51840BCC68D0E37FC9", "Doom 2" },
                { "C13B802C8D80DD02C1AC0D765A88CC4B", "Down in Bermuda" },
                { "E9A2C3977990DCBB13CD00C0F35675FD", "Downwell" },
                { "DFA81AB57355EF9D9827B6FD8A23533B", "Dragon Ball FighterZ" },
                { "1085D38C2FC45212DEB112F7310042BE", "Dragon Ball FighterZ - Online Beta" },
                { "3CB55EE215BEA02B113D3BFB17EF3DBA", "Dragon Blaze" },
                { "5D13BE1872570FD9069C2886013D4CCA", "Dragon Quest Heroes I & II - Demo" },
                { "E31CB7B96F8F4E742CAFE365B3166C41", "Dragon Quest Builders" },
                { "9C5E15B335EDC22403FF3DB67DB446B4", "Dragon Quest Builders - Demo" },
                { "BA01866645036FD743F5AF389EA6DB86", "Dragon Quest Builders 2" },
                { "9A9EA606D08F9444D55FF58B122748A2", "Dragon Quest Builders 2 - Jumbo Demo" },
                { "E58C03FD2FF562F462515FB73597EDA6", "Dragon Quest XI - Echoes of an Elusive Age - Definitive Edition" },
                { "DC083B2CD4ED80E7BC9C8AD7A279BB45", "Dragon Quest XI - Echoes of an Elusive Age - Definitive Edition - Demo" },
                { "F2AC0CB0DC9D1C1A276954BFBC8700D4", "Dragons - Dawn of the New Riders" },
                { "52AEB7EC7AD361B124628FDE71929E05", "Drawn to Life Two Realms" },
                { "05E209A55616CF86BE4D114AD3C63465", "Duck Detective - The Secret Salami" },
                { "76391682B97DFFBB021EC237C60EB534", "Dungeons of Dreadrock" },
                { "5372EC1208C66B8A9E768B072A589617", "Earthlock" },
                { "C1A5557A441F0AA8396CC933539DB23B", "ElecHead" },
                { "6A3CCAF82790B712257DCED584E3E47E", "Embers of Mirrim - Demo" },
                { "8513FF90D97CC14D59F1785D604A6504", "Embracelet" },
                { "6A98CB21EFF4CD6480644A4459E050DE", "Europa Demo" },
                { "9DBA579AF45F98395E8447D2743F9CF0", "Evergate" },
                { "27F8680EEF6F1E65A01BC13DFA000F1F", "Everhood" },
                { "71F6E1DDAFD065C345B3DC4D0DCAFD71", "F-Zero 99" },
                { "40E06CD7167A6EF5742F08F9E8D2B84E", "Fallout Shelter" },
                { "F3837101C611B9DEEC54A1E0E535F416", "FAR - Lone Sails" },
                { "FD183AEA4E01C7D0DF70676D5506C223", "Farming Simulator" },
                { "993B0B446253E7FDAAC3C1A7681F46FE", "Fast RMX" },
                { "508B5C9AB0A1F713E1917FFEF14DA977", "Fatum Betula" },
                { "BB59BA50B4B529EC74388B9232DFFE92", "Fe" },
                { "062DD3BC3CF59885A6762E5A30A14CD1", "FIFA 18" },
                { "5DFDD0B600DAC7776C1ED0D78CC712D5", "FIFA 23 Legacy Edition" },
                { "70C9C91F6813515975DD22F967D94624", "FIGHTING EX LAYER ANOTHER DASH" },
                { "191C20337883858067A130FCA4076A04", "Figment" },
                { "C71F2BA23713B0380A896115D1AD4717", "Fire Emblem Engage" },
                { "0DC6ECE91CF3F6F02BAFC002E3FFBAAD", "Fire Emblem Three Houses" },
                { "E57AAD65AD5D1634D7B282D4A780233D", "Fire Emblem Warriors" },
                { "657E3280215BB0FAE9404BE13768E5D9", "Fire Emblem Warriors - Three Hopes" },
                { "0BA493FCDCF19E9FEE1D9B8EB13241BD", "Fire Emblem Warriors - Three Hopes - Demo" },
                { "C2510293942747C82713F3EB5124E054", "Firewatch" },
                { "835416945FAD6B2F21291A15D2F1790A", "Final Fantasy IX" },
                { "EECC236F99339A8D24BA58C2CCB08D91", "Final Fantasy VII" },
                { "BB94A4F7A98363EA778A47A67857CF7D", "Final Fantasy X HD" },
                { "046C460F6D16A3B4015E1403DA816ECD", "Final Fantasy XII" },
                { "2CC8B772AF83B29364B7B4AD907F1F51", "Final Fantasy XV - Pocket Edition" },
                { "F23196C95CC2B54E2B057ED8493ABB9C", "Flinthook" },
                { "56C052A34AC3FD7F9EC51BEA12162976", "Florence" },
                { "F489C99A244DF57DCBDC4BFD2DB926F1", "Fortnite" },
                { "FE6A6449B65FB0A4A26539786A3AA807", "Freedom Planet" },
                { "1DC473CEFCA17AB42EE8681FFF996D70", "Game Builder Garage" },
                { "36FB9647D509FCDAE5418FF0E8583142", "Gato Roboto" },
                { "F476AB43D70AF863FE443B5546246311", "Gear Club Unlimited" },
                { "6036C3614A1BB793D9D5440ABE001494", "Gear.Club Unlimited 2" },
                { "F64836112794B38AB195F3CAD1E2E9E6", "Go Vacation" },
                { "9DAA7FC507CF187C35793314C90FB2C7", "Graceful Explosion Machine" },
                { "D6AB485ABAF9CB9E56C9D2B1231D87D9", "Graveyard Keeper" },
                { "1D64153D8F77F9440E02383533AD2441", "Gravity Runner" },
                { "5B12FFF1991C5F076F945A1D8DA64BDF", "Golf Story" },
                { "1B870E5879A66E74173AEBCF1A59723C", "GoNNER" },
                { "D696F166B39F21D053E294E03172915D", "Good Job" },
                { "9ADF46CD78FA0B188F2A83078DA7A56B", "Gorogoa" },
                { "779697BD815BF5B537089BC25768FE9A", "Grapple Dog" },
                { "08EDDA01163A8D764ECD531CF6AEA797", "GRIS" },
                { "BB278FDC5DD434CF8F30F224CFBF517A", "Has-Been Heroes" },
                { "C7D8C4153CC9802713604F54C53304E0", "Hades" },
                { "7A0965FF3839B9AE5A5E324A8EBCE0BF", "Hide & Dance" },
                { "22A4BDEA5363AAA24F931D5AF2926082", "Hollow Knight" },
                { "78C6BDD71FCDA145DFA3B2AEB8493E98", "Horizon Chase Turbo" },
                { "5D315FFC05DFD3E4307A98D4E0AE3ED4", "Human Fall Flat" },
                { "EA167CCACA37E364FF7957C68F12AA0E", "Human Resource Machine" },
                { "0263CFE07655B982D6035111B38E8B72", "Hyrule Warriors Definitive Edition" },
                { "58651EB6190D84545B23A9E085BB315E", "Hyrule Warriors Age of Calamity" },
                { "D7BD945BC12F73CA28E54763B2FCE27F", "Hyrule Warriors Age of Calamity - Demo" },
                { "F5B8727C6EC23753411283E8A7B5F263", "I Am Setsuna" },
                { "B88DE87EDF442C3851CAB5A169075F71", "Iconoclasts" },
                { "7F84FCDD31F7A57993D7F9D20F9FE46B", "Ikaruga" },
                { "481B190B591222DB1A81A2FC933B5738", "Immortals Fenyx Rising" },
                { "158B6247012F39F8E7886A1CFEDE4392", "Into the Breach" },
                { "52CBCBAA3D3E7355D39F76358C363156", "Ironcast" },
                { "D83899EEBA9305445B9D20AF92C8ECD0", "It Takes Two" },
                { "09E2689986FDB90C28199859721FC030", "Ittle Dew2+" },
                { "5770D5FB0474ADEC0D4877AFF34430B2", "JDM Racing" },
                { "FE1F808C8E927187A76324DEC3919AAF", "Joysound" },
                { "0A347E222F9DAE29C9514AB6C262481F", "Jump Rope Challenge" },
                { "E604EEE6656E7AD488245F03678945A4", "Kamiko" },
                { "C7C3CAEB6C50DA2C777325EA990171EE", "Katamari Damacy Reroll" },
                { "027B204C5EF34E82824863A3DE9608A6", "Katamari Damacy Reroll" },
                { "A500ECE1923917961DA26DF21DEC3CCC", "Katamari Damacy Reroll" },
                { "051F2E17E4EB124107ADFF00712F68EA", "Katana Zero" },
                { "52F747696E13836ACFF89D9CDEC84A73", "Kathy Rain Director's Cut" },
                { "3B527963A4E21EDEF87C54FA5600E205", "Kero Blaster" },
                { "297CCD9310B5A3428DD72E1E8E1DB278", "Kill la Kill IF" },
                { "92A3FAB08CDD2357E14FB4642A802B12", "Kill-la-kill IF - Demo" },
                { "5BF876BA2390919262A7355705B7F52A", "Kingdom Hearts - Melody of Memory - Demo" },
                { "B39D51AA00F9AD40B82DE34991D36AEB", "Kingdom Rush" },
                { "6F1404E6C8413C7EFF592CBB30E5AB96", "Kingdom Rush Frontiers" },
                { "C29B99154E4514994FC995BD27790963", "Kingdom Rush Origins" },
                { "86C14151EAC920165D017C83A8A5AAE9", "Kingdom - New Lands" },
                { "C75EDE12F2CA92D7D9579C79D6B921D5", "Kirby Fighters 2 Demo" },
                { "B20FAEC679A3A9320864DC374CFB9713", "Kirby Star Allies" },
                { "52175DF068B9DD74E0A97B629F91F197", "Kirby Star Allies - Demo" },
                { "336DB1DA8BDC3BF38ED8609901964A6B", "Kirby and the Forgotten Land" },
                { "7D0C1B828C890ECB3FA7D45F3C22C59D", "Kirby and the Forgotten Land - Demo" },
                { "130BADDB3C2A1143C1AD942289F3D62D", "Kirby's Return to Dream Land Deluxe" },
                { "00D3E5613E7FE063538F981B90721C1A", "Kirby's Return to Dream Land Deluxe - Demo" },
                { "771D829F103A40CFF633BACBBEE39F39", "Layton's Mystery Journey - Katrielle and the Millionaires' Conspiracy - Deluxe Edition" },
                { "25A3BB4DBF243A95E54BF8E131AA488D", "LEGO City Undercover" },
                { "07D9814262B38BD7F7D0D29A25B162B8", "LEGO City Undercover" },
                { "FB761F89764FE6715E1C3E00FCB812D3", "LEGO Harry Potter Collection" },
                { "85BFEEBA8ADF3987441F89A8A598501E", "LEGO Marvel Super Heroes 2" },
                { "09DA83AF74AB162AEE162473AB4C99B6", "Lethal League Blaze - Demo" },
                { "8641FFC05355D64E5B4D4B4CABC3803A", "Lichtspeer" },
                { "E26BA87D9345DFF48529AE017EF97257", "Light Fingers" },
                { "466B5BDBC8CAAC6D679B16794C9D1E3F", "Little Inferno" },
                { "BD73E6433C2A382DAEEBE2F64C3DD7DE", "Little Town Hero" },
                { "8993C2158662E53F0E3DF0738C3A8084", "Lode Runner Legacy" },
                { "93D8BBCEFD41A2DA7BAB6A06BEA5C9DF", "Lonely Mountains: Downhill" },
                { "4A84EF7D776EC1E205FBA72441E5A4C4", "Loop Hero" },
                { "B9750AA0B5C62F9B88FC28A6A1D62955", "Lost in Play" },
                { "676B432413E13E677CB3B980D4A0B336", "Lost Sphear - Demo" },
                { "0C015090E6C5E3F06D97FEDE95458758", "Luigi's Mansion 3" },
                { "38236DD6F3CF1F60A8998C836EE22216", "LUMINES REMASTERED" },
                { "17F7D349D6A1508C316B144FC19A67A7", "Lumo" },
                { "D724F49ADA8115307C7971B0655F5441", "Lunar Lander Beyond Demo" },
                { "6DAE92E255B607316379F3E517CBDFC0", "Marble It Up!" },
                { "9600BAE614E6833B1A261F5FB229CDBA", "Mario + Rabbids Kingdom Battle" },
                { "31F74EF3D97CAC185D7E852926C715B8", "Mario & Sonic at the Olympic Games Tokyo 2020" },
                { "5BC8CFF78A758AD3FAEBD6FAA0A3113B", "Mario & Sonic at the Olympic Games Tokyo 2020 - Demo" },
                { "0E6C26C50905218949356D658355C289", "Mario Golf Super Rush" },
                { "16851BE00BC6068871FE49D98876D6C5", "Mario Kart 8 Deluxe" },
                { "E9FC2E92476F589F890674FE873B0D05", "Mario Party Superstars" },
                { "BA8E0FCB931BBA7F76DCA06732EF9A07", "Mario Tennis Aces" },
                { "9284336297EEC0BD32FD06C9B0209650", "Mario Tennis Aces - Pre-Launch Online Tournament" },
                { "AE97841C4CD6733E57D61B0EE4CE20D9", "Mario Tennis Aces - Pre-Launch Online Tournament" },
                { "7F2BDB49CB7EDF6904DE1190016DBC07", "Mark of the Ninja Remastered" },
                { "EEF8E9558A0946A299305EE281E121B4", "Marvel Ultimate Alliance 3 The Black Order" },
                { "A15180A3520B94075456E9EBE1CD7B71", "Marvels Guardians of the Galaxy" },
                { "8492207AF81A3F7901E89A2C3AC213DC", "Max The Curse of Brotherhood - Demo" },
                { "71FADCF8929C7E9963F20F9D9728672A", "Mega Man 11" },
                { "DA9F3713891E94D55927E9C7825B2A8D", "Mega Man 11 - Demo" },
                { "7806952E0DBDCB5223EC78FF8088406C", "Mega Man X Legacy Collection 1" },
                { "A72DB9B6A79458E56A0FF3CAA68C7EF4", "Mega Man Zero ZX Collection" },
                { "4D37E8E895F61DAB62161ACC7942945A", "Membrane" },
                { "167E856F0B36BF0AF1406A5873D4D2E3", "Metal Slug 2" },
                { "291F8A44D0318835B09A30B3A1A99B6A", "Metal Slug 3" },
                { "9FDBE9AA7A25826355B814E529BE6F1A", "Metal Slug X" },
                { "9A9B6E0371D34263D6B6577F9CBA54D5", "Metroid Dread" },
                { "33CD3D9D72A8B36D46FFDB8049CD4FD6", "Metroid Dread - Demo" },
                { "0B3EAA668777B67B76600A7290A94AFC", "Metroid Prime Remastered" },
                { "1CA528593A9112595542B3E5B78510B0", "Mighty Gunvolt Burst" },
                { "7111B191E2BE628CCF07FC39C001BACE", "Mighty Gunvolt Burst - Demo" },
                { "BE69DF9FDF680F1A7049C2306C81C12E", "Miitopia" },
                { "BE1AD9667CACC1E36CE22E56B692A651", "Miitopia - Demo" },
                { "11B64E28AD7A49CA9EC8AC007BE858C6", "Minecraft" },
                { "773F9627E0AC611AA92DA55E307BD361", "Minecraft - Nintendo Switch Edition" },
                { "7031CCD4553F03F65910A5C7CAEC6066", "Minecraft Dungeons" },
                { "69203F57A02BA39D7A6E8912E69294CD", "Minit" },
                { "D7439E0E29A2308707D5F9909E9BB210", "Mom Hid My Game" },
                { "5D7941328A974A7170516EC4A21B80B7", "Momonga Pinball Adventures" },
                { "E7678FEF120A0D21EAC958914F088988", "Monopoly for Nintendo Switch" },
                { "C57722BCA3D76018529DD7D1A46BB144", "Monster Boy and the Cursed Kingdom" },
                { "D76EEDD0E7AAC6E276F5E19A44D7E22B", "Monster Crown" },
                { "2AE606B8659FEA852E0ED6470C9814D9", "Monster Hunter Generations Ultimate" },
                { "73D31AEE5EAC355B115DF4CF13F8AE03", "Monster Hunter Generations Ultimate - Demo" },
                { "073C4133EF14AE3782A1342DC4E71592", "Monster Hunter Rise - Demo" },
                { "39510404A02B44F31B4FAEDE4BCF0FD9", "Monster Hunter XX - Demo" },
                { "50C7A05EF639900D18851C11C9A76C84", "moon" },
                { "69AA450BB058D7C0F3FABB7FC7C2DBA2", "Moonlighter" },
                { "00FB98F718BE3501DF21305CBA922135", "MotoGP 18" },
                { "060013937BC484C195B5B7D9DD63EBAC", "Mr. Shifty" },
                { "1CC3304E7F1DB4DC19A25F507C7DF490", "MXGP 3" },
                { "DA37E68BA125453A63BA66E971056FEF", "N++" },
                { "B7484CBF8E543B918A37D15859072B07", "Nairi: Tower of Shirin" },
                { "6EC44942405D964FCA42DCEB6BF54884", "Namco Museum - Free Multiplayer Version" },
                { "C15A47C7F7B07448C8F2E3BDF73AD439", "Namco Museum" },
                { "ECD841E3A4B1239EA5F8DD40056C010E", "Namco Museum Archives Volume 1" },
                { "4A3C965CA3E534C39F98E517C8AE2522", "Need for Speed Hot Pursuit Remastered" },
                { "93F32017A6AF2E56307B3CEA28C44519", "Neo Turf Masters" },
                { "A28465B16E89DA962874CA547F122F42", "Neuro Voider" },
                { "657EB330BAA5EAA4F56989E2CC1D0608", "New Frontier Days - Founding Pioneers" },
                { "2F53EC5ABE8D93C7B8CFE7A3C40C0A38", "New Super Luckys Tale" },
                { "652985A584146F03C1416056F78F45DB", "Ninjala Exclusive Ninja Club" },
                { "256AB0A1FF2B5824C5420994231869F2", "Night in the Woods" },
                { "C689313FCA0428D0A02B7CE197724761", "Nintendo Labo Toy-Con 01 Variety Kit" },
                { "5E92066D83C981FCBC05AE4D9711906A", "Nintendo Switch Online - Famicom" },
                { "483C6BBED1D1438BF0CAEDD31329B90E", "Nintendo Switch Online - Game Boy" },
                { "51944F40F35BE3688F8FD0D8CFE910FD", "Nintendo Switch Online - Game Boy Advance" },
                { "8F655652CF5441D7471D936F3F07324D", "Nintendo Switch Online - NES" },
                { "C46BE5741BFDE75BA1110C84E61676BA", "Nintendo Switch Online - Nintendo 64" },
                { "05DC14F80A13996B94160CD375AFD506", "Nintendo Switch Online - SNES" },
                { "80F923768D742848D9C9A08632967120", "Nintendo Switch Online - Super Famicom" },
                { "76ABD1FB8107400EA366E7B6FBE45ABB", "Nintendo Switch Online" },
                { "030CC84A0859ABD745D45D5F8AAD9975", "Nintendo Switch Sports" },
                { "BA9B30CEF9C64C54618C8C2277900085", "Nintendo Switch Sports - Demo" },
                { "C98E67C171A444427AF290122FE7CCD2", "Nuclear Throne" },
                { "A84B23FADD8D8E68E7CB37810B54E7C4", "Ocean's Heart" },
                { "D14B5C54F4BB20BE5C92599F7911004C", "Oceanhorn" },
                { "856082D450FA5F52542520A9FDF7DCDF", "Oceanhorn - Demo" },
                { "5F006BF2CE7EC39D3B861FA6F150B381", "Octodad: Dadliest Catch" },
                { "93C1C73A3BAF9123A15B9B24886B634B", "Octopath Traveller" },
                { "D0D1FD5D1C6CEA47DE7261E211A53786", "Octopath Traveller - Demo" },
                { "3C15B8B5FCFF64D185612CFC684FD589", "Okami" },
                { "ED60DEF35B747E088A2C783CA7E55BA0", "Okami" },
                { "B3C3E7BB605EB5A9F43D82368C00A682", "Old Man's Journey" },
                { "96F77A7F9F43AA96C23054FF48A167B7", "Once Upon a Jester" },
                { "A39D8E379899865C355A2EF9D9067F67", "One Strike" },
                { "74E60CA799C3278AD6279D11AE5FEBDA", "Onimusha Warlords HD" },
                { "F897DAA49269BC023482B5C9AAA6BE73", "Overcooked Special Edition" },
                { "D4DFEB2A86C8E2A460B41C59FCEB784E", "Oxenfree" },
                { "F869F29B8909D93B8F6A1EC5BDDAB49F", "Owlboy" },
                { "DE54A3217AFBFD1063CE3EE01BF1766E", "Pac-Man 99" },
                { "0103C1801C62C21D9770DA79C7FBBF1F", "Pac-Man Championship Edition 2 Plus" },
                { "6596C20F0074B600C19784F40D93ECEF", "Paladins" },
                { "28D351544DC829EEBE54E53AF29EB030", "PAN-PAN" },
                { "045B57CF1A7936DCD924C36515C75E20", "Paper Mario The Origami King" },
                { "D2DFD39BE614907EF698799150EED22C", "Penny's Big Breakaway Demo" },
                { "31B7926A16300FCAC90E7674F3916DE7", "Persona 5 Royal" },
                { "91F09C687E93595937340B10CA3C87E6", "Persona 5 Tactica" },
                { "DECD6107E5477CFA2919C447E5B68FF0", "Phoenix Wright Ace Attorney Trilogy" },
                { "539D575B96E556159C3F4667D1100DDD", "Picross S" },
                { "C3B4869C14F86DE0A1D8CB153002F6B4", "Picross S2" },
                { "984ADD1F1A3BDDA97C93D0D8657E7755", "Pic-A-Pix - Demo" },
                { "EBD4E01584652FFC5D69088746822397", "PictoQuest - The Cursed Grids" },
                { "2409F1185DA3EEAB2FE48E20ECCB1060", "Pikmin 3 Deluxe" },
                { "F84084125D49DCB722D20211987E0D83", "Pikmin 3 Deluxe - Demo" },
                { "3B2F3272A827E4B76CEFE30977C1DBCC", "Pikmin 4" },
                { "C8F8F89F4F6DD2E9206B07B8CF5BB1D7", "Pikuniku" },
                { "90646BC12AD199C21A10C53DE8289D0C", "Pinball FX3" },
                { "69B9969EC3D9DF6399D6DC160DC26E68", "Phantom Breaker Battle Grounds Overdrive" },
                { "AF9CC3C97938B51955EC2834487A7265", "Phantom Trigger" },
                { "C0BC54BB47ED74C0371EB410A4A63EEE", "Planet of Lana" },
                { "1CED1B76CE76DB49E48C6F360E022485", "Pode" },
                { "7DCC42E2AF4C1BBE54BB71700F7161B6", "Pok�mon Brilliant Diamond" },
                { "DCC3293E2A34D17106770DF1958475A9", "Pok�mon Caf� ReMix" },
                { "9D6B5AFEF371E7D57A4EF29F3A421F7A", "Pok�mon Home" },
                { "7CBCCE282CD36658AB28471FB4791102", "Pok�mon Legends Arceus" },
                { "94CAAF6C83EE682D358EB6183EEF7D28", "Pok�mon Let's Go Pikachu" },
                { "5F25EBBAB5987964E56ADA5BBDDE9DF2", "Pok�mon Let's Go Eevee" },
                { "5D500549174110191B5A65DD8B17A9E2", "Pok�mon Let's Go Pikachu Let's Go Eevee Trial Version" },
                { "A5A477B869AFD225B3DAF74249D70FF2", "Pok�mon Mystery Dungeon - Rescue Team DX" },
                { "2A2B87EF65984805F4D71920ECF2C80E", "Pokemon Mystery Dungeon: Rescue Team DX - Demo" },
                { "B8FAEF4816CAC2B76D11869B05CA7601", "Pok�mon Shield" },
                { "7F4AACC644EAC4BF4E5897B413FFD611", "Pokemon Shining Pearl" },
                { "3C66B776DB1AA06323037049FACD96D3", "Pok�mon Sword" },
                { "E4B364C957D95017CA1171810D655865", "Pok�mon Quest" },
                { "339470D1C820D6D2392320FD0992BB5E", "Pok�mon Unite" },
                { "B6CE40797459B0890BF7CEF68A4CE587", "Pok�mon Violet" },
                { "5A6F3157C944341CDE279FD85822A3AD", "Pokk�n Tournament DX" },
                { "17E1F78566D4581CA5F9E1088DA6ED51", "Pokk�n Tournament DX - Demo" },
                { "D3676443729E0AAD1EFE4330FC65C55D", "Prince of Persia The Lost Crown" },
                { "C31DA8706A6CDDCC04A04CFA35F47298", "Project Octopath Traveler - Demo" },
                { "FBE68646FA9AFCDB4576B6F3FFFFB54D", "Project Octopath Traveler - Demo" },
                { "E8D46EE901916DEEAC0E9B6F20CAAB3B", "Puyo Puyo Tetris - Demo" },
                { "502448D1CC582509ABCA0ECB86D27FCC", "Puyo Puyo Tetris - Demo" },
                { "0585E865DFB68B5298F19360A730EDB3", "Puyo Puyo Tetris" },
                { "27B43DBE1CF53CADD3897FC3CD79185F", "Puyo Puyo Tetris" },
                { "692EC865DB8D5C0B5E910C82FDFC2AEC", "Raiden V" },
                { "DA8EA86F7FB53E44A8C29BECE6617DA1", "Raji - An Ancient Epic - Demo" },
                { "316BFA7ADD8FAD56FD16235AEE761C70", "Rainworld" },
                { "4CE9BA887E04FB8D36B745ECC8660DE6", "Rayman Legends Definitive Edition - Demo" },
                { "251B606CAAE97CD7BB1849E9DAD12C82", "Rayman Legends Definitive Edition" },
                { "B4AFDAE2B9575D4DD0E2C589EBD2D66C", "Resident Evil 7 - Cloud Version" },
                { "638E7E1EEC4CD8A239243633C0345A07", "Ring Fit Adventure" },
                { "E8680D795743293DE312E20C9B85C1DD", "Risk of Rain 2" },
                { "A6823431F0DD63EA1A6E9C4B064A7463", "River City Girls" },
                { "D68C79758FC22A7A0BEF5CCBC1F68FF1", "Robonauts - Demo" },
                { "11260164DFCCB4C1980A116D57B6C9F9", "Rocket Fist" },
                { "6F4D679ED7D2A016B654B265B956C5F0", "Rocket League" },
                { "C253FD76D0B7E36F16E15D99989EAEE8", "R�ki" },
                { "5839699F090DB87D1C923B0338F124C4", "RPG Maker MV" },
                { "6ADA7D49177E88E12506768B364B794C", "Scott Pilgrim vs. The World The Game" },
                { "F00364C7A81ADE9ABC33F1760072F0F0", "Sea of Stars Demo" },
                { "CD83B7737C7B75F2E4D57CD542F4E0BD", "Sea of Stars" },
                { "66BE7C9621FFEDB7B48FF11CD07EDA01", "SEGA AGES Virtua Racing" },
                { "10913F964FE64B3373593633781B4FA9", "Severed" },
                { "FFDC724B30E62B7C8A1E6E613784DBFB", "Shadows of Adam" },
                { "E29DFF8692E980291D837B80E119388C", "Shalnor Legends - Sacred Lands" },
                { "65590FD04B4DF51BB1D8C39A50C8376C", "Shantae Half-Genie Hero" },
                { "25B77E52575617C0C2E4A4E0A346538C", "Shantae Half-Genie Hero" },
                { "C392D9DD4226F3430D460A366D67397E", "Shipped" },
                { "BD0E7D5235BE8956C727AA4CED0452C7", "Shovel Knight Specter of Torment" },
                { "F8D087CFC849713C76A06A954E9486D3", "Shovel Knight Treasure Trove" },
                { "E1A7E95F9E577242170CB3869DA3D3AF", "Sid Meier's Civilization VI" },
                { "42A65F67C20F2AF4737AE9D4D5A37ECB", "Slay the Spire" },
                { "E769E2D93519E8D860445ED00A4A5E3D", "Slime-san" },
                { "59D23C681619F4E7FBC748D9E5DC28D0", "Slime-San" },
                { "554C97481EFDFC30DCDF01FC5CC877A6", "Snake Pass" },
                { "466683549EC0B90D20925E0594733A31", "Snipperclips" },
                { "66C396DFF5BBB36B52AEA44F72B55406", "Snipperclips Plus" },
                { "DB4ACF91897DEE47A4F659070EBDEC8B", "Snipperclips - Demo" },
                { "3BACB884A7DEF1E57A9F0F5B11503213", "Snufkin Melody of Moonminvalley" },
                { "1E3A785D5F49FEF6A0923AC2CB1B873C", "Songbird Symphony" },
                { "3DFE0E14AB7E082F634D5F963D2E9723", "Sonic Forces - Demo" },
                { "1628E0CE3F839127054B0EE36E28E52A", "Sonic Mania" },
                { "C12B1E3DCD8B7FF99668AB64B7F9D79D", "Sparkle 2" },
                { "8A9A1AA3F3EED4406E14C8A6EFA369D3", "Spelunker Party - Demo" },
                { "6D311B9F9472ED7110A68EEEFB884101", "Sphinx and the Cursed Mummy" },
                { "F07F7B79366747AB415C6A1B4A8BBDE1", "Spiritfarer" },
                { "777AC1189ADA093280C624C7F7AA30B1", "Splatoon 2 - Global Testfire" },
                { "41E75AEC7E74EFAAEC3E284B0442621F", "Splatoon 2 - Splatfest World Premiere" },
                { "D1997608258569F211E4A950A67A764C", "Splatoon 2 - Splatfest World Premiere" },
                { "B1D15D71E6E66DE6DF5F52312ABD780E", "Splatoon 2 - Splatfest World Premiere" },
                { "2EF67D5CE7F5FDB2CBD3CE04B5F433AC", "Splatoon 2 - Special Demo" },
                { "CBA841B50A92A904E313AE06DF4EF71A", "Splatoon 2" },
                { "397A963DA4660090D65D330174AC6B04", "Splatoon 2" },
                { "96841E1AA566AB97AB99D428D3EB9FF9", "Splatoon 3 - Splatfest World Premiere" },
                { "4CE9651EE88A979D41F24CE8D6EA1C23", "Splatoon 3" },
                { "C2B49A475DF5A340494292A1BD398579", "Stardew Valley" },
                { "AF92B7A16C36C3E5DE1C76E5B20E0421", "Starlink - Battle for Atlas" },
                { "DE93618DDF64F9184ADD7AEC762AB7EB", "Star Ocean First Departure R" },
                { "1E02DD2B1BA399E6398606335E38E57C", "Star Renegades" },
                { "4D5A2A79927AEFDD7EBB9B111EF09EA1", "SteamWorld Dig" },
                { "6AFF601CA973FE393C7B5F67A146E224", "SteamWorld Dig 2" },
                { "D02D58137844CD4B939CD3571BCB8355", "SteamWorld Heist" },
                { "987323D95ADF303B4B8C6E1A0173D61C", "SteamWorld Quest" },
                { "FDD7B2FD0E944EEF60169AB42A73C202", "Steel Assault" },
                { "78B0328141E809C9F62BA16F0236D3CF", "Story of Seasons - Pioneers of Olive Town" },
                { "D0B9AC9357A3561F8D041C64209A70F0", "Strikey Sisters" },
                { "E07D9F37D97B71B15021DA310C0AFDB2", "Subsurface Circular" },
                { "7E4A7F5F54C8B4843683AD3C63C0BCE9", "Summer in Mara" },
                { "4E04D8234DC33FAEC8172DB7D0A88D27", "Sumikko Gurashi Sumikko Park he Youkoso" },
                { "A48B98FFF048C51E7516B0DAFCAE630B", "Super Arcade Racing" },
                { "96794F2558A5B75BC0DBCC69AA0119EE", "Super Bomberman R" },
                { "9281DE1C6303F73D75CC62DAA7E621F1", "Super Bomberman R Online" },
                { "C4398909C53BFEEDB9EB7D3128DB2DC1", "Super Chariot" },
                { "0DB172DD733179C0091C3FD49201B5AA", "Super Kirby Clash" },
                { "45998392CB216ABF034EB6F98E399A06", "Super Mario 3D All-Stars" },
                { "4E551BEEBAD303591E38565E64373519", "Super Mario 3D World + Bowser's Fury" },
                { "502BADF490303E7A09F1BF58AD4341AB", "Super Mario Bros. 35" },
                { "33B8CC310F76D76B17C6DB0011A75C8A", "Super Mario Bros. U Deluxe" },
                { "E5B04B40CFD924420DF6142200E473B4", "Super Mario Bros. Wonder"},
                { "BF19FBEA37724338D87F26F17A3B97B2", "Super Mario Maker 2" },
                { "8AEDFF741E2D23FBED39474178692DAF", "Super Mario Odyssey" },
                { "099ECEEF904DB62AEE3A76A3137C241B", "Super Mario Party" },
                { "E73E4976CD1992F1CF99AC0053695FD6", "Super Monkey Ball Banana Blitz HD" },
                { "0E7DF678130F4F0FA2C88AE72B47AFDF", "Super Smash Bros. Ultimate" },
                { "C6D726972790F87F6521C61FBA400A1D", "Super Smash Bros. Ultimate" },
                { "C6D726972790F87F6521C61FBA400A1DX", "Super Smash Bros. Ultimate" },
                { "5BB5C0DAC8FF51E1C76EDC6E659EA349", "Sushi Striker - The Way of Sushido - Demo" },
                { "F63470B5DA63E1F6A304B3E5EA994CD1", "Swords & Bones" },
                { "0C6883ADBF9C7FDBEE7483F98990EEEE", "Taiko No Tatsujin Drum 'n' Fun!" },
                { "64902EF0DF1ABA05E4076CB0BBFCD9A5", "Tales of Vesperia Definitive Edition" },
                { "9349210FA0267D208CECA1B424DCE082", "Team Sonic Racing" },
                { "20401E06C6C65A77CE058F23E9066EB1", "Terraria" },
                { "691C9B2C6D1F1E032DDC01FD026159FD", "Tetris 99" },
                { "9E6A5948F727B745C7EE32B5243A9155", "Tetris Effect" },
                { "3890C59FA557713F88E4F0CF54A6D233", "The Battle of Polytopia" },
                { "1B7686315C6209EBBD25E3E11E89316C", "The Binding of Isaac Afterbirth+" },
                { "D7B61DBE9E88E862593044D886181F8F", "The Binding of Isaac Afterbirth+" },
                { "A244BE0C68B41F10DC7C44DD62069B60", "The Book of Unwritten Tales 2" },
                { "74EA5D8C57EB2F39A242F585A490F51B", "The Elder Scrolls V Skyrim" },
                { "D537C2F4CAE9F1F822696AA5C9503C7D", "The End is Nigh" },
                { "97BCC51C1E39B4A461399DDD6104DF16", "The Final Station" },
                { "AB9D59D8B9DCD6278DD6CC3120FFDA00", "The First Tree" },
                { "610198057E9A81232F89483FDA0336FC", "The Flame in the Flood" },
                { "F1C11A22FAEE3B82F21B330E1B786A39", "The Legend of Zelda Breath of the Wild" },
                { "9129043EF2AAD7F1157CF852BACB8F7D", "The Legend of Zelda Link's Awakening" },
                { "75EA6B6DBAD83649DC8B76075B47ECDA", "The Legend of Zelda Skyward Sword HD" },
                { "CC47F0DEC75C1FD3B1F95FA9F9D57667", "The Legend of Zelda Tears of the Kingdom" },
                { "0F5E4644D09332F271659E6A79AF8823", "The Magnificent Trufflepigs" },
                { "0E5FEC5305745886DD7884A0129866BD", "The Messenger" },
                { "42D51332E98075DFC90EDDCE4B254E8B", "The Missing J.J. Macfield and the Island of Memories" },
                { "A3BBA2FE7C279F9AE1D54D75A25E4045", "The Mummy Demastered" },
                { "010558F429C25C6688B87B1AB272C2C5", "The Oregon Trail" },
                { "0AAEADBAE13F54655FB78D29C1832AFD", "The Pathless" },
                { "401181E3A6B098AB7C6ECAD116D88777", "The Red Lantern" },
                { "FBB327649A728DC50F4ADC09C6CD9625", "The Sexy Brutale" },
                { "9E19570E6059798A45AEC175873B4AC1", "The Stanley Parable Ultra Deluxe" },
                { "EFDDE81F58B4C9CBBED7D3AE6FD48B29", "The Swords of Ditto: Mormo's Curse" },
                { "170E1BFCAD62DC17198275870C69B410", "The Warlock of Firetop Mountain" },
                { "F9E4ED7281324AE9C62C7E2BBA082F61", "The World Ends With You Final Remix" },
                { "845B4527CFEEB9B508A5F98C4C6E7F1C", "Thimbleweed Park" },
                { "1DD1C6A5282A71BAD7DE6F2BD8F31E6F", "Thomas Was Alone" },
                { "34AA407B2A4D3F234171C5FE7DA3C5C2", "Three Fourths Home - Extended Edition" },
                { "CC100708A30C87729D59A1DC649A2AF0", "Timelie" },
                { "CA63AF28CAFDFD2BF117E5800B8A41BA", "Toki Tori" },
                { "6B14E6B278A4307BA29EB1F22D6CE79D", "TowerFall" },
                { "F56D9ACC5F1A4CB745C85B90662EE555", "Trash Quest" },
                { "F6201CAA76254472DE8B55A36D0DA720", "Triangle Strategy" },
                { "C7892DA72808DC3F7CBED36F86AE5894", "Triangle Strategy - Demo" },
                { "695E3FE0793D9789A57187ADD7649DCC", "Trine 4 - The Nightmare Prince" },
                { "9CE8BB05457E73DDE8496C95A8152157", "Trivial Pursuit Live!" },
                { "5454B78835FF8AD3903E719491ED39BB", "TRON Identity" },
                { "23C8C1366F9A2FCCE85255D7889D3412", "Tumbleseed" },
                { "E071EE2C2FF60B04627FA634C4BD2248", "Tumblestone" },
                { "32F20ACDF89C38EFCB7A04E4ABC29AA5", "Tunic" },
                { "A67952BE42B9E787BBDFA0C427ABB5C5", "Twelve Minutes" },
                { "92EDE0C7C89D5ED13D0CA773BE080402", "Ultra Street Fighter II The Final Challengers" },
                { "A29C3926C7AC223A657F1415D1C1D797", "Undertale" },
                { "5AD45D9CB39067CC619E159B4D20A827", "Unepic" },
                { "E9C28DFFD5D80C33F554CFF5BFE8CAB0", "Uno" },
                { "7B69D4A9D98569DDAB6210F1B6AA2D31", "Unpacking" },
                { "906D1DD5BCEDC3E72889D5D23917398E", "Unravel Two" },
                { "A10B9E983E22D9175CE28F2275CEC8E6", "Untitled Goose Game" },
                { "4F26C43AFCA5C03EE5FA461A886D1153", "Valkyria Chronicles 4" },
                { "C95A834355A2E5D272F9DECB9BFEE7AD", "Valkyria Chronicles 4 - Demo" },
                { "BA2B436FAB449C607F7FEE30F5124F3E", "Vampire Surviors" },
                { "94385799F4667FC55304697D8F49B8B7", "VA-11 HALL-A" },
                { "1E8C4E1B1FF5B57DC645B39CBFA5C4EF", "Venba" },
                { "BE6192BB8A650E1F18D9CE97AF97C045", "Vitamin Connection" },
                { "404652E38014828AC0ED1A3EE2F1DEA3", "VOEZ" },
                { "1AAFE12FC4DA32AA90B2C527DF73B08B", "VOEZ - Demo" },
                { "509005580AA0EE52748499526A3C91B9", "void tRrLM(); - Trial ver." },
                { "993AC1F3383F4FF879FEA9A85677F9F9", "VVVVVV" },
                { "B189E7FDF30356EC7B08C94ADE944BBB", "Wandersong" },
                { "F8DC9B4C1339AEC598320758EECD69C2", "Warface" },
                { "935E13FC47C481C979FEA5B1CC318284", "Wargroove" },
                { "2977E8F7226EFAAD8FD2D622FE543E75", "Warlock's Tower" },
                { "E63EF81FF154760556FE75A9747665FA", "WarioWare Get It Together! Demo" },
                { "9C9F8AAFB91A09EF4037E09F12BBAB01", "When the Past was Around" },
                { "9C16E60AD4F3166140A29F63C4B1173B", "Wide Ocean Big Jacket" },
                { "5534EAB89CEEE189F712F1C946A24BBB", "Wilmot's Warehouse" },
                { "8D46002FB026383429B61455D531B076", "Windbound" },
                { "67D01338887DAC4477826B5EA75BFB74", "Wizard of Legend" },
                { "E2DADADBE272660A316475FCCDECFECB", "Wonder Boy - The Dragon's Trap" },
                { "C4E9854DE59E1AAD6BFE8091E8A5B77D", "World of Goo" },
                { "D7B5A51A98F2DFCD491468473BD0C6DA", "WRC 8" },
                { "659B13F48903294AE2B3FA4F12DA9898", "Xenoblade Chronicles 2" },
                { "A862246CB76B2B6DC14022F4545399F5", "Xenoblade Chronicles 3" },
                { "4EBD015519178CCE78B40BB34B4A97AB", "Yoku's Island Express" },
                { "456F2C9BE4E84FE854619AD59C5D00A2", "Yonder - The Cloud Catcher Chronicles" },
                { "2C912087C21A306D0534D160A0A1CC50", "Yono and the Celestial Elephants" },
                { "B078B511A5B781471916CCD172F8038E", "Yooka Laylee" },
                { "703BDEE72B19135DD048E644FC452DE6", "Yooka Laylee and the Impossible Lair" },
                { "7AEA3B76283DF2B97E581259A12F733D", "Yoshi's Crafted World - Demo" },
                { "E0271681F1AC755DDE5456B234024A96", "Yoshi's Crafted World" },
                { "F1B09F862DFFB502186230E74E0CB2E1", "Ys Origin"}
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