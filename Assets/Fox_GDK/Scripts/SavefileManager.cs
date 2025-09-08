/*
 * Developer Name: Md. Imran Hossain
 * E-mail: sandsoftimer@gmail.com
 * FB: https://www.facebook.com/md.imran.hossain.902
 * in: https://www.linkedin.com/in/md-imran-hossain-69768826/
 * 
 * Features: 
 * Saving gameplay boxData
 * Loading gameplay boxData  
 * 
 * Requrements:
 * Newtonsoft Package Need to be Installed (com.unity.nuget.newtonsoft-json)
 */

using Newtonsoft.Json;
using System.IO;
using UnityEngine;

namespace Com.FunFox.Utility
{
    public class SavefileManager : MonoBehaviour
    {
        string saveFile;

        public void SaveGameData<T>(T gameData, string fileName = null) where T : new()
        {
            fileName ??= $"{typeof(T)}";
            saveFile = $"{Application.persistentDataPath}/{fileName}.json";
            string jsonData = JsonConvert.SerializeObject(gameData, Formatting.Indented, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            File.WriteAllText(saveFile, jsonData);
        }

        public T LoadGameData<T>(string fileName = null) where T : new()
        {
            fileName ??= $"{typeof(T)}";
            saveFile = $"{Application.persistentDataPath}/{fileName}.json";

            T gameData = new();
            if (File.Exists(saveFile))
            {
                JsonSerializerSettings settings = new()
                {
                    ObjectCreationHandling = ObjectCreationHandling.Replace
                };
                string jsonData = File.ReadAllText(saveFile);
                gameData = JsonConvert.DeserializeObject<T>(jsonData, settings);
                return gameData;
            }
            else
            {
                SaveGameData(gameData, fileName);
                Debug.LogError($"============ New Savefile Created============'\n'Path:{saveFile}");
            }
            return gameData;
        }
    }
}