using System;
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
        public bool SpentGold(int amount)
        {
            if (currentGold - amount < 0) return false;

            currentGold -= amount;
            goldCoinsText.text = currentGold.ToString("D3");
            return true;
        }

        internal void ResetCurrentGold()
        {
            currentGold = 0;
            if (goldCoinsText == null) goldCoinsText = GameObject.Find(COIN_AMOUNT_TEXT).GetComponent<TMP_Text>();
            goldCoinsText.text = currentGold.ToString("D3");
        }
    }
}
