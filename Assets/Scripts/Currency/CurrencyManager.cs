using TMPro;
using UnityEngine;

namespace SlowpokeStudio.Currency
{
    public class CurrencyManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI goldText;

        private int totalGold = 0;

        private void Start()
        {
            Debug.Log($"[CurrencyManager] Initial Gold: {totalGold}");
            UpdateGoldUI();
        }

        public void AddGold(int amount)
        {
            Debug.Log($"[CurrencyManager] AddGold called. Current: {totalGold}, Incoming Reward: {amount}");

            totalGold += amount;

            Debug.Log($"[CurrencyManager] Updated Total Gold: {totalGold}");
            UpdateGoldUI();
        }

        private void UpdateGoldUI()
        {
            goldText.text = $"{totalGold}";
            Debug.Log($"[CurrencyManager] UI Updated: {goldText.text}");
        }

        public int TotalGoldDebug()
        {
            return totalGold;
        }
    }
}

