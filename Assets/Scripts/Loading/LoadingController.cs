using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using Plugins.Dropbox;
using UnityEngine.UI;
using TMPro;
using System.IO;

namespace Loading
{
    public class LoadingController : MonoBehaviour
    {
        public Slider progressBar;
        public TextMeshProUGUI progressText;
        public string[] dropboxConfigFileNames = { "data1.json", "data2.json", "data3.json"};
        public string savePath;

        async void Start()
        {
            await DropboxHelper.Initialize();

            foreach (string configFileName in dropboxConfigFileNames)
            {
                string configFilePath = "Config/" + configFileName;
                bool configExists = Resources.Load(configFilePath) != null;

                if (!configExists)
                {
                    Debug.Log($"No local config file {configFileName} found. Downloading from Dropbox...");
                    await DownloadConfigFile(configFileName);
                }
            }

            await DownloadModsFiles();

            await LoadMainScene();
        }

        async Task DownloadConfigFile(string configFilePath)
        {
            float downloadProgress = 0f;

            UpdateProgress(downloadProgress);

            await DropboxHelper.DownloadAndSaveFile(configFilePath);
        }

        async Task DownloadModsFiles()
        {
            string modsFolderName = "mods";
            string modsFolderPath = "mods.json";

            // Check if mods folder exists locally
            if (!Directory.Exists(Path.Combine(Application.dataPath, "Resources", modsFolderName)))
            {
                Debug.Log($"No local mods folder found. Downloading from Dropbox...");
                await DropboxHelper.DownloadAndSaveFolder(modsFolderName);
            }

            // Check if mods.json exists locally
            if (!File.Exists(Path.Combine(Application.dataPath, "Resources", modsFolderPath)))
            {
                Debug.Log($"No local mods.json file found. Downloading from Dropbox...");
                await DropboxHelper.DownloadAndSaveFile(modsFolderPath);
            }
        }

        void UpdateProgress(float progress)
        {
            progressBar.value = Mathf.Min(progress, 1f);
            progressText.text = Mathf.RoundToInt(progress * 100) + "%";
        }

        async Task LoadMainScene()
        {
            Debug.Log("Loading main scene...");
            await LoadSceneAsync();

        }

        async Task LoadSceneAsync()
        {
            float progress = 0f;
            while (progress < 1f)
            {
                progress += Time.deltaTime; 
                UpdateProgress(progress);
                await Task.Yield();
            }

            await Task.Delay(2000);

            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("Application", LoadSceneMode.Single);

            while (!asyncOperation.isDone)
            {
                await Task.Yield();
            }
        }
    }
}


