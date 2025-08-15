using TMPro;
using UnityEngine;

namespace TopDown.SceneManagment
{
    public class EconomyManager : Singleton<EconomyManager>
    {
        private TMP_Text goldCoinsText;
        private int currentGold = 0;

        const string COIN_AMOUNT_TEXT = "GoldCoinsText";

        public void UpdateCurrentGold()
        {
            currentGold++;

            if (goldCoinsText == null) goldCoinsText = GameObject.Find(COIN_AMOUNT_TEXT).GetComponent<TMP_Text>();

            goldCoinsText.text = currentGold.ToString("D3");
        }
    }
}
