using UnityEngine;
using TMPro;

namespace Searching
{
    public class CardSerching : MonoBehaviour
    {
        public TMP_InputField searchInput;
        private GameObject[] allCards;

        private void Start()
        {

            searchInput.onValueChanged.AddListener(delegate { SearchCards(); });
        }

        public void SearchCards()
        {
            allCards = GameObject.FindGameObjectsWithTag("Card");
            string searchTerm = searchInput.text.ToLower();

            if (string.IsNullOrEmpty(searchInput.text))
            {
                foreach (GameObject card in allCards)
                {
                    card.SetActive(true);
                }
            }
            else
            {
                foreach (GameObject card in allCards)
                {
                    string cardText = card.GetComponentInChildren<TextMeshProUGUI>().text.ToLower();
                    bool isVisible = cardText.Contains(searchTerm);
                    card.SetActive(isVisible);
                }
            }
        }
    }
}

