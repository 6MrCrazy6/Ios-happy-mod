using UnityEngine;
using UnityEngine.UI;

namespace UIScripts
{
    public class CategoryScriptrs : MonoBehaviour
    {
        public GameObject[] category;
        public Button[] categoryChangeButton;
        public Sprite inactiveSprite;
        public Sprite activeSprite;

        public void TurnOnCategory(int categorys)
        {
            for (int i = 0; i < category.Length; i++)
            {
                category[i].SetActive(false);
                categoryChangeButton[i].image.sprite = inactiveSprite;
            }
            category[categorys - 1].SetActive(true);
            categoryChangeButton[categorys - 1].image.sprite = activeSprite;
        }
    }
}

