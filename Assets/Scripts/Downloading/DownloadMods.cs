using ModsCard;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Downloading
{
    public class DownloadMods : MonoBehaviour
    {
        public Slider downloadProgressSlider;
        public TextMeshProUGUI downloadText;
        public TextMeshProUGUI modTitleText;

        public Button downloadButton;
        public Button shareButton;

        private List<string> downloadedModTitles = new List<string>();
        private string modsFolder = "Mods";

        private void Update()
        {
            if (downloadedModTitles.Contains(modTitleText.text))
            {
                shareButton.gameObject.SetActive(true);
                downloadButton.gameObject.SetActive(false);
                
            }
            else
            {
                shareButton.gameObject.SetActive(false);
                downloadButton.gameObject.SetActive(true);
            }
        }

        public void OnDownloadButtonClicked()
        {
            if (CardManager.SelectedMod != null)
            {
                StartCoroutine(DownloadFile(CardManager.SelectedMod.file_path, CardManager.SelectedMod.title));

            }
            else
            {
                Debug.Log("No mod selected");
            }
        }

        private IEnumerator DownloadFile(string filePath, string modTitle)
        {
            string fullPath = "mods" + filePath;
            Debug.Log("Full path to load: " + fullPath);
            string sourcePath = Path.Combine(Application.dataPath, "Resources", fullPath);
            string destinationPath = Path.Combine(Application.dataPath, modsFolder, Path.GetFileName(filePath));

            if (File.Exists(sourcePath))
            {
                downloadProgressSlider.gameObject.SetActive(true);
                downloadText.gameObject.SetActive(true);
                for (float i = 0; i <= 1; i += Time.deltaTime)
                {
                    downloadProgressSlider.value = i;
                    yield return null;
                }

                downloadProgressSlider.gameObject.SetActive(false);
                downloadText.gameObject.SetActive(false);

                bool isModDownloaded = downloadedModTitles.Contains(modTitle);

                if (!isModDownloaded)
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(destinationPath));
                    File.Copy(sourcePath, destinationPath, true);
                    downloadedModTitles.Add(modTitleText.text);
                }

                shareButton.onClick.AddListener(() =>
                {
                    new NativeShare().AddFile(destinationPath).Share();
                });
            }
            else
            {
                Debug.Log("File not found: " + sourcePath);
            }
        }
      
    }
}

