using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Assignment2
{

    public struct HighscoreData
    {
        public List<int> highscores;
    }


    public class SaveUtils
    {
        private static SaveUtils instance;
        private StorageDevice storageDevice;
        private static string HIGHSCORE_FILE = "highscores.sav";
        private static string CONTAINER_NAME = "BreakoutA4";
        private string completePath;
        
        /// <summary>
        /// Constructor
        /// </summary>
        public SaveUtils()
        {
#if XBOX360
            Game1.instance.Components.Add(new GamerServicesComponent(Game1.instance));
#endif
#if WINDOWS
            //create the path to C:\ProgramData
            var systemPath = System.Environment.
                 GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            //append to make a folder for our game; C:\ProgramData\BreakoutA4
            var path = Path.Combine(systemPath, "BreakoutA4");

            //check if it exists; if it doesn't, create it
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            //append again to have a path with filename for saving
            completePath = Path.Combine(path, HIGHSCORE_FILE);
#endif
        }


        /// <summary>
        /// checks to see if storage device is registered.
        /// </summary>
        /// <returns>true if registered; false otherwise</returns>
        public bool storageRegistered()
        {
            if (storageDevice == null)
                return false;
            else
                return true;
        }


        /// <summary>
        /// Regitsters the storage device for Xbox360
        /// </summary>
        public void RegisterStorage()
        {
            //Get StorageDevice
            if (!Guide.IsVisible)
            {
                storageDevice = null;//reset device                
                IAsyncResult result = StorageDevice.BeginShowSelector(PlayerIndex.One, null, null);//storage device selected
                storageDevice = StorageDevice.EndShowSelector(result);//set storage device
            }
        }


        /// <summary>
        /// Saves the highscores.
        /// </summary>
        /// <param name="HighScore">list of highscores</param>
        public void saveHighScores(List<int> HighScore)
        {
#if WINDOWS
            SaveWindowsHighScores(HighScore);
#else
            SaveXboxHighScores(HighScore);
#endif
        }

        /// <summary>
        /// Saves the highscores.
        /// </summary>
        /// <param name="HighScore">list of highscores</param>
        private void SaveXboxHighScores(List<int> HighScore)
        {
            HighscoreData highscores = new HighscoreData();
            highscores.highscores = HighScore;


            if (storageDevice != null && storageDevice.IsConnected)
            {
                IAsyncResult result = storageDevice.BeginOpenContainer(CONTAINER_NAME, null, null);
                result.AsyncWaitHandle.WaitOne();
                StorageContainer container = storageDevice.EndOpenContainer(result);

                if (container.FileExists(HIGHSCORE_FILE))//if that file already exists
                {
                    container.DeleteFile(HIGHSCORE_FILE);//delete existing file
                }

                Stream stream = container.CreateFile(HIGHSCORE_FILE);//create file

                XmlSerializer serializer = new XmlSerializer(typeof(HighscoreData));
                serializer.Serialize(stream, highscores);

                stream.Close();//close the file
                container.Dispose();//disposing container commits changes to device
            }
        }

        /// <summary>
        /// Saves the highscores.
        /// </summary>
        /// <param name="HighScore">list of highscores</param>
        private void SaveWindowsHighScores(List<int> HighScore)
        {
            HighscoreData highscores = new HighscoreData();
            highscores.highscores = HighScore;

            //if the path isn't null
            if (completePath != null)
            {
                using (var stream = new FileStream(completePath, FileMode.Create))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(HighscoreData));
                    serializer.Serialize(stream, highscores);
                }
            }
        }

        /// <summary>
        /// loads the HighScores from file.
        /// </summary>
        /// <returns>the highscore data structure</returns>
        public HighscoreData loadHighScores()
        {
            HighscoreData data;
#if WINDOWS
            data = loadHighScoresWindows();
#else
            data = loadHighScoresXbox();
#endif
            return data;
        }

        /// <summary>
        /// load the HighScore file from storage.
        /// </summary>
        /// <returns>the highscore data structure</returns>
        private HighscoreData loadHighScoresXbox()
        {
            HighscoreData loadData;

            IAsyncResult result = storageDevice.BeginOpenContainer(CONTAINER_NAME, null, null);
            result.AsyncWaitHandle.WaitOne();
            StorageContainer container = storageDevice.EndOpenContainer(result);

            result.AsyncWaitHandle.Close();

            Stream stream = container.OpenFile(HIGHSCORE_FILE, FileMode.Open);//open file
            XmlSerializer serializer = new XmlSerializer(typeof(HighscoreData));
            loadData = (HighscoreData)serializer.Deserialize(stream);

            stream.Close();
            container.Dispose();

            return loadData;
        }

        /// <summary>
        /// load the HighScore file from PC.
        /// </summary>
        /// <returns>the highscore data structure</returns>
        private HighscoreData loadHighScoresWindows()
        {
            HighscoreData data;

            using (var stream = new FileStream(completePath, FileMode.Open))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(HighscoreData));
                data = (HighscoreData)serializer.Deserialize(stream);
            }
            return data;
        }

        /// <summary>
        /// checks if the HighScore file exists.
        /// </summary>
        /// <returns>true if the file exists; false otherwise.</returns>
        public bool CheckHighScoreExists()
        {
            bool exists = false;
#if WINDOWS
            exists = CheckFileExistsWindows(completePath);
#else
            exists = CheckFileExistsXbox(HIGHSCORE_FILE);
#endif
            return exists;
        }

        /// <summary>
        /// checks if the file already exists on the PC
        /// </summary>
        /// <returns>true if the file exists; false otherwise.</returns>
        private bool CheckFileExistsWindows(string filename)
        {
            return (File.Exists(filename));
        }

        /// <summary>
        /// checks if the file already exists on the xbox
        /// </summary>
        /// <returns>true if it exists; false otherwise.</returns>
        private bool CheckFileExistsXbox(string filename)
        {
            IAsyncResult result = storageDevice.BeginOpenContainer(CONTAINER_NAME, null, null);
            result.AsyncWaitHandle.WaitOne();
            StorageContainer container = storageDevice.EndOpenContainer(result);
            bool exists = container.FileExists(filename);
            container.Dispose();
            return exists;
        }

        public static SaveUtils getInstance()
        {
            if (instance == null)
            {
                instance = new SaveUtils();
            }

            return instance;
        }
    }
}
