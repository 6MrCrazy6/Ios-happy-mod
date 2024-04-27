using Newtonsoft.Json;
using System;
using System.Collections;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ModsCard
{
    public class CardManager : MonoBehaviour
    {
        public GameObject cardPrefab;
        public GameObject bigCardPrefab;
        
        public GameObject[] Categories;

        public GameObject fullCardCanvas;

        public static CardData SelectedMod;

        private void Start()
        {
            StartCoroutine(LoadAndCreateCards());
        }

        public void OnCategoryChanged()
        {
            ClearCards();
            StartCoroutine(LoadAndCreateCards());
        }

        private void ClearCards()
        {
            foreach (GameObject category in Categories)
            {
                foreach (Transform child in category.transform)
                {
                    Destroy(child.gameObject);
                }
            }
        }


        private IEnumerator LoadAndCreateCards()
        {
            ResourceRequest resourceRequest = Resources.LoadAsync<TextAsset>("mods");
            yield return resourceRequest;

            TextAsset jsonTextAsset = resourceRequest.asset as TextAsset;
            string jsonText = jsonTextAsset.text;

            ModsData modsData = JsonConvert.DeserializeObject<ModsData>(jsonText);

            foreach (var cardData in modsData.mods)
            {
                for (int i = 0; i < Categories.Length; i++)
                {
                    if (Categories[i].activeSelf && cardData.category.ToString() == "Category" + (i + 1).ToString())
                    {
                        CreateCard(cardData, Categories[i]);
                    }
                }
            }
        }

        private void CreateCard(CardData cardData, GameObject categoryObject)
        {
            GameObject cardObject = Instantiate(cardPrefab, categoryObject.transform);
            cardObject.GetComponent<Button>().onClick.AddListener(() => OnCardClick(cardData));

            Transform titleTransform = cardObject.transform.Find("Title");
            Transform descriptionTransform = cardObject.transform.Find("Description");
            Image iconImage = cardObject.transform.Find("Icon").GetComponent<Image>();

            if (titleTransform == null || descriptionTransform == null)
            {
                Debug.LogError("Failed to find Title or Description in card prefab.");
                Destroy(cardObject);
                return;
            }

            TextMeshProUGUI titleText = titleTransform.GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI descriptionText = descriptionTransform.GetComponent<TextMeshProUGUI>();

            titleText.text = cardData.title;

            string[] descriptionSentences = Regex.Split(cardData.description, @"(?<=[\.!\?])\s+");
            if (descriptionSentences.Length > 0)
            {
                string firstSentence = descriptionSentences[0].Trim();
                if (!firstSentence.EndsWith("."))
                {
                    firstSentence += ".";
                }
                descriptionText.text = firstSentence;
            }

            string spritePath = cardData.preview_path.Replace(".png", "").Replace(".jpg", "");
            iconImage.sprite = Resources.Load<Sprite>("mods" + spritePath);

            cardObject.transform.SetParent(categoryObject.transform, false);
        }

        private void OnCardClick(CardData cardData)
        {
            fullCardCanvas.SetActive(true);

            TextMeshProUGUI titleText = bigCardPrefab.transform.Find("Title").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI descriptionText = bigCardPrefab.transform.Find("Description").GetComponent<TextMeshProUGUI>();
            Image iconImage = bigCardPrefab.transform.Find("Icon").GetComponent<Image>();

            titleText.text = cardData.title;
            descriptionText.text = cardData.description;
            string spritePath = cardData.preview_path.Replace(".png", "").Replace(".jpg", "");
            iconImage.sprite = Resources.Load<Sprite>("mods" + spritePath);

            SelectedMod = cardData;
        }

        public void OnFullCardCanvasDeactivated()
        {
            TextMeshProUGUI titleText = bigCardPrefab.transform.Find("Title").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI descriptionText = bigCardPrefab.transform.Find("Description").GetComponent<TextMeshProUGUI>();
            Image iconImage = bigCardPrefab.transform.Find("Icon").GetComponent<Image>();

            titleText.text = "";
            descriptionText.text = "";
            iconImage.sprite = null;
        }

    }
}


