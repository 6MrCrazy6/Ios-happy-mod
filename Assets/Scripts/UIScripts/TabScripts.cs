using UnityEngine;
using UnityEngine.UI;

namespace UIScripts
{
    public class TabScripts : MonoBehaviour
    {
        public GameObject[] tabs;

        public Button[] changeTabsButton;


        public void TurnOnTabs(int tab)
        {
            for (int i = 0; i < tabs.Length; i++)
            {
                tabs[i].SetActive(false);
                changeTabsButton[i].GetComponent<Image>().color = Color.white;

                if (i == 2)
                {
                    changeTabsButton[2].GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f); 
                }
                else
                {
                    
                    changeTabsButton[2].GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.5f); 
                }
            }
            tabs[tab - 1].SetActive(true);
            changeTabsButton[tab - 1].GetComponent<Image>().color = Color.black;
        }
    }
}

